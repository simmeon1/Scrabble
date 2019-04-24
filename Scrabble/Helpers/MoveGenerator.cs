using DawgSharp;
using Scrabble.Classes;
using Scrabble.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Helpers
{
    /// <summary>
    /// Creates Move Generator object
    /// Uses information known about an untransposed and transposed board to build 
    /// moves for each.
    /// </summary>
    public class MoveGenerator
    {
        public Game Game { get; set; }
        public Rack RackOfCurrentPlayer { get; set; }
        public Rack OriginalRackOfPlayer { get; set; }
        public BoardTile[,] BoardArray { get; set; }
        public BoardTile[,] TransposedBoardArray { get; set; }
        public Dawg<bool> Dawg { get; set; }
        public List<int[]> ListOfValidAnchorCoordinatesOnUntransposedBoard { get; set; }
        public List<int[]> ListOfValidAnchorCoordinatesOnTransposedBoard { get; set; }
        public Dictionary<BoardTile, List<CharTile>> ValidUntransposedCrossChecks { get; set; }
        public Dictionary<BoardTile, List<CharTile>> ValidTransposedCrossChecks { get; set; }
        public WordDictionary Dictionary { get; set; }
        public List<Move> Moves { get; set; }
        public int TimeLimit { get; set; }
        public Stopwatch Stopwatch { get; set; }

        public MoveGenerator(Game game, BoardTile[,] boardArray, BoardTile[,] transposedBoardArray, Dawg<bool> dawg, List<int[]> listOfValidAnchorCoordinatesOnUntransposedBoard, List<int[]> listOfValidAnchorCoordinatesOnTransposedBoard, Dictionary<BoardTile,
            List<CharTile>> validUntransposedCrossChecks, Dictionary<BoardTile, List<CharTile>> validTransposedCrossChecks, WordDictionary dictionary, List<Move> moves, int timeLimit)
        {
            Game = game;
            RackOfCurrentPlayer = game.GetPlayerAtHand().Rack;
            OriginalRackOfPlayer = new Rack { Rack_CharTiles = new List<Rack_CharTile>() };

            //Recreates rack of player
            foreach (var entry in RackOfCurrentPlayer.Rack_CharTiles)
            {
                OriginalRackOfPlayer.Rack_CharTiles.Add(new Rack_CharTile
                {
                    CharTile = entry.CharTile,
                    CharTileID = entry.CharTileID,
                    Count = entry.Count,
                    ID = entry.ID,
                    Rack = entry.Rack,
                    RackID = entry.ID
                });
            }
            BoardArray = boardArray;
            TransposedBoardArray = transposedBoardArray;
            Dawg = dawg;
            ListOfValidAnchorCoordinatesOnUntransposedBoard = listOfValidAnchorCoordinatesOnUntransposedBoard;
            ListOfValidAnchorCoordinatesOnTransposedBoard = listOfValidAnchorCoordinatesOnTransposedBoard;
            ValidUntransposedCrossChecks = validUntransposedCrossChecks;
            ValidTransposedCrossChecks = validTransposedCrossChecks;
            Dictionary = dictionary;
            Moves = moves;

            //Sets equal limit for untransposed and transposed checks
            TimeLimit = timeLimit / 2;
            if (TimeLimit == 0 && timeLimit == 1) TimeLimit = 1;
            Stopwatch = new Stopwatch();
            Stopwatch.Reset();
        }

        /// <summary>
        /// Builds and returns possible moves for the current board
        /// The same algorithm is used for a untransposed(horizontal) and transposed (vertical) board.
        /// This limits the problem to one dimension - with the given rack, anchors and cross checks,
        /// build all possible words on this row.
        /// </summary>
        /// <param name="boardIsHorizontal"></param>
        /// <param name="giveOneResult"></param>
        /// <returns></returns>
        public HashSet<GeneratedMove> GetValidMoves(bool boardIsHorizontal, bool giveOneResult = false)
        {
            var retriesPerAnchor = 2;
            var boardArray = boardIsHorizontal ? BoardArray : TransposedBoardArray;

            //Recreates board before any moves are generated
            var boardBeforeMove = new BoardTile[boardArray.GetLength(0), boardArray.GetLength(1)];
            for (int i = 0; i < boardArray.GetLength(0); i++)
            {
                for (int j = 0; j < boardArray.GetLength(1); j++)
                {
                    boardBeforeMove[i, j] = new BoardTile
                    {
                        BoardLocationX = boardArray[i, j].BoardLocationX,
                        BoardLocationY = boardArray[i, j].BoardLocationY,
                        BoardTileType = boardArray[i, j].BoardTileType,
                        CharTile = boardArray[i, j].CharTile,
                        Board = boardArray[i, j].Board,
                        BoardID = boardArray[i, j].BoardID,
                        BoardTileTypeID = boardArray[i, j].BoardTileTypeID,
                        CharTileID = boardArray[i, j].CharTileID,
                        ID = boardArray[i, j].ID,
                        IsFilled = boardArray[i, j].IsFilled
                    };
                }
            }
            var listOfValidAnchorCoordinates = boardIsHorizontal ? ListOfValidAnchorCoordinatesOnUntransposedBoard : ListOfValidAnchorCoordinatesOnTransposedBoard;
            var validCrossChecks = boardIsHorizontal ? ValidUntransposedCrossChecks : ValidTransposedCrossChecks;
            HashSet<GeneratedMove> validMovesList = new HashSet<GeneratedMove>(new GeneratedMoveEqualityComparer());

            //Shuffles anchors to add unpredictability to move generation
            listOfValidAnchorCoordinates.Shuffle();
            var originalTimeLimit = TimeLimit;
            var retriesForCurrentAnchor = retriesPerAnchor;

            //Starts stopwatch. Move generation stops if time limit has been reached after a recursive call has returned to its original state.
            Stopwatch.Start();
            //listOfValidAnchorCoordinates = new List<int[]>();
            //listOfValidAnchorCoordinates.Add(new int[] { 12, 7 });
            //listOfValidAnchorCoordinates.Add(new int[] { 12, 8 });
            //listOfValidAnchorCoordinates.Add(new int[] { 12, 9 });

            //Tries to attach words to all anchors on the board
            for (int i = 0; i < listOfValidAnchorCoordinates.Count; i++)
            {
                if (retriesForCurrentAnchor == 0)
                {
                    retriesForCurrentAnchor = retriesPerAnchor;
                    continue;
                }
                var anchor = listOfValidAnchorCoordinates[i];

                //Checks if any letter can be played in tile. If not, target next anchor
                var crossCheckEntry = validCrossChecks.Where(c => c.Key == boardArray[anchor[0], anchor[1]]).FirstOrDefault().Value;
                if (crossCheckEntry != null && crossCheckEntry.Count == 0) continue;
                var limit = 0;
                var columnIndex = anchor[1];
                var wordBeforeAnchor = "";

                //Builds up word before anchor if it exists, otherwise builds a limit to tiles we can place before anchor
                while (columnIndex > 0)
                {
                    if (boardArray[anchor[0], columnIndex - 1].CharTile == null && !listOfValidAnchorCoordinates.Any(c => c[0] == anchor[0] && c[1] == columnIndex - 1))
                    {
                        limit++;
                        columnIndex--;
                    }
                    else if (boardArray[anchor[0], columnIndex - 1].CharTile != null)
                    {
                        while (columnIndex > 0 && boardArray[anchor[0], columnIndex - 1].CharTile != null)
                        {
                            wordBeforeAnchor = wordBeforeAnchor.Insert(0, boardArray[anchor[0], columnIndex - 1].CharTile.Letter.ToString());
                            columnIndex--;
                        }
                        break;
                    }
                    else if (boardArray[anchor[0], columnIndex - 1].CharTile == null)
                    {
                        break;
                    }
                }

                //If no letter can be placed before anchor, try to build right part after anchor
                if (limit == 0)
                {
                    ExtendRight(wordBeforeAnchor, anchor, boardArray, validCrossChecks, validMovesList, boardIsHorizontal, boardBeforeMove);
                }

                //Otherwise places a valid tile from rack
                else if (wordBeforeAnchor.Length == 0)
                {
                    foreach (var entry in Dictionary.CharTiles.Where(d => d.Score != 0))
                    {
                        if (RackOfCurrentPlayer.CheckIfTileIsInRack(entry.Letter, true)
                            && (!validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1]])
                            || validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1]]
                            && c.Value.Any(x => x.Letter == entry.Letter)))
                            && anchor[1] < boardArray.GetLength(1) - 1)
                        {
                            var tile = RackOfCurrentPlayer.SubstractFromRack(entry.Letter);
                            boardArray[anchor[0], anchor[1]].CharTile = tile;

                            //Once a tile is placed to the left of anchor, LeftPart is called which firstly tries to build all possible right parts for that anchor
                            //Once the extension to the right is complete, goes back to try and build a new left part
                            LeftPart(tile.Letter.ToString(), limit, new int[] { anchor[0], anchor[1] + 1 }, boardArray, validCrossChecks, validMovesList, boardIsHorizontal, boardBeforeMove);
                            RackOfCurrentPlayer.AddToRack(tile);
                            boardArray[anchor[0], anchor[1]].CharTile = null;
                            if (Helper.CheckIfTimeLimitIsReached(Stopwatch, TimeLimit) && validMovesList.Count > 0)
                            {
                                Stopwatch.Reset();
                                return validMovesList;
                            }
                        }
                    }
                }
                if (giveOneResult)
                {
                    return validMovesList;
                }
                //If timer is reached and no moves are found for anchor, increase time limit and try again for that anchor
                //If there are still no moves after two retries, move to different anchor
                if (Helper.CheckIfTimeLimitIsReached(Stopwatch, TimeLimit))
                {
                    if (validMovesList.Count == 0)
                    {
                        if (retriesForCurrentAnchor != 0)
                        {
                            TimeLimit += 1;
                            retriesForCurrentAnchor--;
                            i--;
                            Stopwatch.Restart();
                        }
                    }
                    else
                    {
                        TimeLimit = originalTimeLimit;
                        Stopwatch.Reset();
                        return validMovesList;
                    }
                }

            }
            return validMovesList;
        }

        /// <summary>
        /// Builds left part of word before anchor
        /// </summary>
        /// <param name="partialWord">Word preceding anchor. Might already be on the board or placed from previous LeftPart calls</param>
        /// <param name="limit">Limit of tiles we can place before anchor</param>
        /// <param name="anchor">Anchor from which to build left and right part</param>
        /// <param name="boardArray">Board array to work with (untransposed or transposed)</param>
        /// <param name="validCrossChecks">A list of crosschecks which say what tiles can be played in each board tile</param>
        /// <param name="validMovesList">A list of valid moves which is continuously updated</param>
        /// <param name="boardIsHorizontal">Board is untransposed or transposed</param>
        /// <param name="boardBeforeMove">Board before any moves were generated</param>
        public void LeftPart(string partialWord, int limit, int[] anchor, BoardTile[,] boardArray, Dictionary<BoardTile, List<CharTile>> validCrossChecks,
            HashSet<GeneratedMove> validMovesList, bool boardIsHorizontal, BoardTile[,] boardBeforeMove)
        {
            //Tries to build all right parts for given left part
            ExtendRight(partialWord, anchor, boardArray, validCrossChecks, validMovesList, boardIsHorizontal, boardBeforeMove);
            if (limit > 0)
            {
                //Gets a list of letters that can follow the current left part and can be placed to the left of the anchor
                HashSet<string> labelsOfDawgEdges = new HashSet<string>(new DawgEdgeEqualityComparer());
                var wordsWithCommonPreffix = Dawg.MatchPrefix(partialWord);
                foreach (var word in wordsWithCommonPreffix)
                {
                    labelsOfDawgEdges.Add(word.Key.Substring(partialWord.Length));
                }
                foreach (var label in labelsOfDawgEdges)
                {
                    if (label == "") continue;
                    if (OriginalRackOfPlayer.CheckIfWordIsPlayable(partialWord + label[0]) &&
                        ((!validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1] - 1])
                        || validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1] - 1] &&
                        c.Value.Any(x => x.Letter == label[0])))))
                    {
                        CharTile tileToWorkWith = null;
                        if (RackOfCurrentPlayer.CheckIfTileIsInRack(label[0], false))
                        {
                            tileToWorkWith = Dictionary.CharTiles.Where(c => c.Letter == label[0] && c.Score != 0).FirstOrDefault();
                        }
                        else
                        {
                            tileToWorkWith = Dictionary.CharTiles.Where(c => c.Letter == '*').FirstOrDefault();
                        }

                        //Rebuilds left part with new tiles
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            boardArray[anchor[0], anchor[1] - 1 - i].CharTile = null;
                        }
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            var tileToAdd = OriginalRackOfPlayer.GetTile(partialWord[i]);
                            boardArray[anchor[0], anchor[1] - partialWord.Length - 1 + i].CharTile = tileToAdd;
                        }
                        RackOfCurrentPlayer.SubstractFromRack(tileToWorkWith);
                        boardArray[anchor[0], anchor[1] - 1].CharTile = tileToWorkWith;
                        bool validPrefix = true;

                        //Checks if the newly formed left part's components are valid for the board
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            var boardTile = boardArray[anchor[0], anchor[1] - 1 - partialWord.Length + i];
                            if (!(!validCrossChecks.Any(c => c.Key == boardTile)
                                   || validCrossChecks.Any(c => c.Key == boardTile &&
                                    c.Value.Any(x => x.Letter == partialWord[i]))))
                            {
                                validPrefix = false;
                                break;
                            }
                        }

                        //If they are valid, call LeftPart again, which will call ExtendRight for all possible right moves and eventually try a new left part
                        if (validPrefix) LeftPart(partialWord + label[0], limit - 1, anchor, boardArray, validCrossChecks, validMovesList, boardIsHorizontal, boardBeforeMove);
                        RackOfCurrentPlayer.AddToRack(tileToWorkWith);
                        boardArray[anchor[0], anchor[1] - 1].CharTile = null;

                        //Resets the left part once done
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            boardArray[anchor[0], anchor[1] - partialWord.Length - 1 + i].CharTile = null;
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Builds all possible right parts for given anchor
        /// </summary>
        /// <param name="partialWord">Word preceding right part</param>
        /// <param name="anchor">Tile from which to build up right part</param>
        /// <param name="boardArray">Board to work with</param>
        /// <param name="validCrossChecks">List of valid cross checks for all empty tiles</param>
        /// <param name="validMovesList">List of valid moves that gets updated</param>
        /// <param name="boardIsHorizontal">Board is untransposed or transposed</param>
        /// <param name="boardBeforeMove">Board before any moves were generated</param>
        /// <param name="tileExtendedWith">Tile that was used to extend the right part</param>
        public void ExtendRight(string partialWord, int[] anchor, BoardTile[,] boardArray, Dictionary<BoardTile,
            List<CharTile>> validCrossChecks, HashSet<GeneratedMove> validMovesList, bool boardIsHorizontal, BoardTile[,] boardBeforeMove,
            CharTile tileExtendedWith = null)
        {
            //If no tile is present..
            if (boardArray[anchor[0], anchor[1]].CharTile == null)
            {
                //If word up until current tile is valid..
                if (Helper.CheckWordValidity(Dawg, partialWord))
                {
                    //If the move (word, row and column indexes) has not been added already..
                    if (!Moves.Any(m => m.Word.Equals(partialWord)))
                    {
                        //Adds generated move to list of valid moves
                        Dictionary<BoardTile, CharTile> tilesUsed = new Dictionary<BoardTile, CharTile>();

                        //Adds tiles that were used in move
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            var letter = partialWord[i];
                            var boardTile = boardArray[anchor[0], anchor[1] - partialWord.Length + i];

                            if (boardTile.CharTile != null)
                            {
                                tilesUsed.Add(boardTile, boardTile.CharTile);
                            }
                            else
                            {
                                tilesUsed.Add(boardTile, tileExtendedWith);
                            }
                        }
                        validMovesList.Add(new GeneratedMove(boardIsHorizontal, anchor[1] - partialWord.Length, anchor[1] - 1, anchor, tilesUsed, boardBeforeMove, RackOfCurrentPlayer));
                    }
                }

                //GEts all letters that can follow current right part word
                HashSet<string> labelsOfDawgEdges = new HashSet<string>(new DawgEdgeEqualityComparer());
                var wordsWithCommonPreffix = Dawg.MatchPrefix(partialWord);
                foreach (var word in wordsWithCommonPreffix)
                {
                    labelsOfDawgEdges.Add(word.Key.Substring(partialWord.Length));
                }
                foreach (var label in labelsOfDawgEdges)
                {
                    //If the valid letter is in our rack and can be played on the board tile, places it and extends right again
                    //with the newly filled board tile used as anchor if board limit is not reached
                    if (label == "") continue;
                    if (RackOfCurrentPlayer.CheckIfTileIsInRack(label[0], true) &&
                        (!validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1]])
                        || validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1]] && c.Value.Any(x => x.Letter == label[0]))))
                    {
                        CharTile tileToWorkWith = null;
                        if (RackOfCurrentPlayer.CheckIfTileIsInRack(label[0], false))
                        {
                            tileToWorkWith = Dictionary.CharTiles.Where(c => c.Letter == label[0] && c.Score != 0).FirstOrDefault();
                        }
                        else
                        {
                            tileToWorkWith = Dictionary.CharTiles.Where(c => c.Letter == '*').FirstOrDefault();
                        }
                        RackOfCurrentPlayer.SubstractFromRack(tileToWorkWith);
                        boardArray[anchor[0], anchor[1]].CharTile = Dictionary.CharTiles.Where(c => c.Letter == label[0] && c.Score == tileToWorkWith.Score).FirstOrDefault();
                        if (anchor[1] < boardArray.GetLength(1) - 1)
                        {
                            ExtendRight(partialWord + label[0], new int[] { anchor[0], anchor[1] + 1 }, boardArray, validCrossChecks, validMovesList, boardIsHorizontal, boardBeforeMove, tileToWorkWith);
                        }

                        //Otherwise places the tile on the last empty square of the board, checks if its valid and doesn't attempt to extend anymore
                        else
                        {
                            var finalWord = partialWord + boardArray[anchor[0], anchor[1]].CharTile.Letter;
                            if (Helper.CheckWordValidity(Dawg, finalWord))
                            {
                                if (!Moves.Any(m => m.Word.Equals(finalWord)))
                                {
                                    Dictionary<BoardTile, CharTile> tilesUsed = new Dictionary<BoardTile, CharTile>();
                                    for (int i = 0; i < finalWord.Length; i++)
                                    {
                                        var letter = finalWord[i];
                                        var boardTile = boardArray[anchor[0], anchor[1] - finalWord.Length + 1 + i];
                                        if (boardTile.CharTile != null)
                                        {
                                            tilesUsed.Add(boardTile, boardTile.CharTile);
                                        }
                                        else
                                        {
                                            tilesUsed.Add(boardTile, tileToWorkWith);
                                        }
                                    }
                                    validMovesList.Add(new GeneratedMove(boardIsHorizontal, anchor[1] - finalWord.Length + 1, anchor[1], anchor, tilesUsed, boardBeforeMove, RackOfCurrentPlayer));
                                }
                            }
                        }
                        RackOfCurrentPlayer.AddToRack(tileToWorkWith);
                        boardArray[anchor[0], anchor[1]].CharTile = null;


                    }
                }
            }
            //Otherwise if the current tile is already taken and not empty, used letter from board tile to build to the right again
            else
            {
                var tile = boardArray[anchor[0], anchor[1]].CharTile;
                HashSet<string> labelsOfDawgEdges = new HashSet<string>(new DawgEdgeEqualityComparer());
                var wordsWithCommonPreffix = Dawg.MatchPrefix(partialWord + tile.Letter);
                foreach (var word in wordsWithCommonPreffix)
                {
                    labelsOfDawgEdges.Add(word.Key.Substring((partialWord + tile.Letter).Length));
                }

                //Extends right if any letters can follow the current right part
                if (labelsOfDawgEdges.Any() && anchor[1] < boardArray.GetLength(1) - 1)
                {
                    ExtendRight(partialWord + tile.Letter, new int[] { anchor[0], anchor[1] + 1 }, boardArray, validCrossChecks, validMovesList, boardIsHorizontal, boardBeforeMove, tile);
                }
            }
        }
    }
}
