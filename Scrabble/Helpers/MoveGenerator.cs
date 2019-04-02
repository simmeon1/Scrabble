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
    public class MoveGenerator
    {
        public Game Game { get; set; }
        public Rack RackOfCurrentPlayer { get; set; }
        public Rack OriginalRackOfPlayer { get; set; }
        public Board Board { get; set; }
        public Dawg<bool> Dawg { get; set; }
        public WordDictionary Dictionary { get; set; }
        public List<int[]> ListOfValidAnchors { get; set; }
        public List<Move> Moves { get; set; }
        public int TimeLimit { get; set; }
        public Stopwatch Stopwatch { get; set; }

        public MoveGenerator(Game game, Dawg<bool> dawg, WordDictionary dictionary, List<Move> moves, int timeLimit)
        {
            Game = game;
            RackOfCurrentPlayer = game.GetPlayerAtHand().Rack;
            OriginalRackOfPlayer = new Rack { Rack_CharTiles = new List<Rack_CharTile>() };
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
            };
            Board = Game.Board;
            Dawg = dawg;
            Dictionary = dictionary;
            Moves = moves;
            TimeLimit = timeLimit;
            Stopwatch = new Stopwatch();
            Stopwatch.Reset();
        }

        //public MoveGenerator(Game game, BoardTile[,] boardArray, BoardTile[,] transposedBoardArray, Dawg<bool> dawg, HashSet<BoardTile> listOfValidAnchorCoordinates, List<int[]> listOfValidAnchorCoordinatesOnTransposedBoard, Dictionary<BoardTile,
        //    List<CharTile>> validUntransposedCrossChecks, Dictionary<BoardTile, List<CharTile>> validTransposedCrossChecks, WordDictionary dictionary, List<Move> moves, int timeLimit)
        //{
        //    Game = game;
        //    RackOfCurrentPlayer = game.GetPlayerAtHand().Rack;
        //    OriginalRackOfPlayer = new Rack { Rack_CharTiles = new List<Rack_CharTile>() };
        //    foreach (var entry in RackOfCurrentPlayer.Rack_CharTiles)
        //    {
        //        OriginalRackOfPlayer.Rack_CharTiles.Add(new Rack_CharTile
        //        {
        //            CharTile = entry.CharTile,
        //            CharTileID = entry.CharTileID,
        //            Count = entry.Count,
        //            ID = entry.ID,
        //            Rack = entry.Rack,
        //            RackID = entry.ID
        //        });
        //    }
        //    BoardArray = boardArray;
        //    TransposedBoardArray = transposedBoardArray;
        //    Dawg = dawg;
        //    ListOfValidAnchorCoordinatesOnUntransposedBoard = listOfValidAnchorCoordinatesOnUntransposedBoard;
        //    ListOfValidAnchorCoordinatesOnTransposedBoard = listOfValidAnchorCoordinatesOnTransposedBoard;
        //    ValidUntransposedCrossChecks = validUntransposedCrossChecks;
        //    ValidTransposedCrossChecks = validTransposedCrossChecks;
        //    Dictionary = dictionary;
        //    Moves = moves;
        //    TimeLimit = (timeLimit / 2) + 1;
        //    Stopwatch = new Stopwatch();
        //    Stopwatch.Reset();
        //}

        public HashSet<GeneratedMove> GetValidMoves(bool boardIsHorizontal, bool giveOneResult = false)
        {
            var retriesPerAnchor = 2;
            var boardArray = boardIsHorizontal ? Board.ConvertTo2DArray() : Board.Transpose2DArray(Board.ConvertTo2DArray());
            var listOfAnchors = new List<int[]>();
            for (int i = 0; i < boardArray.GetLength(0); i++)
            {
                for (int j = 0; j < boardArray.GetLength(1); j++)
                {
                    if (boardArray[i,j].IsAnchor)
                    {
                        listOfAnchors.Add(new int[] { i, j });
                    }
                }
            }
            ListOfValidAnchors = listOfAnchors;
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
                        ID = boardArray[i, j].ID
                    };
                }
            }
            HashSet<GeneratedMove> validMovesList = new HashSet<GeneratedMove>(new GeneratedMoveEqualityComparer());
            ListOfValidAnchors.Shuffle();
            var originalTimeLimit = TimeLimit;
            var retriesForCurrentAnchor = retriesPerAnchor;

            Stopwatch.Start();
            for (int i = 0; i < ListOfValidAnchors.Count; i++)
            {
                if (retriesForCurrentAnchor == 0) {
                    retriesForCurrentAnchor = retriesPerAnchor;
                    continue;
                }          
                var anchorCoordinates = ListOfValidAnchors[i];
                var anchorAsBoardTile = boardArray[anchorCoordinates[0], anchorCoordinates[1]];
                var anchorCrossCheck = boardIsHorizontal ? anchorAsBoardTile.UntransposedCrossCheck : anchorAsBoardTile.TransposedCrossCheck;
                if (anchorCrossCheck == "") continue;
                var limit = 0;
                var anchorRowIndex = anchorCoordinates[0];
                var anchorColumnIndex = anchorCoordinates[1];               
                //for (int x = 0; x < boardArray.GetLength(0); x++)
                //{
                //    for (int y = 0; y < boardArray.GetLength(1); y++)
                //    {
                //        if (boardArray[x,y] == anchor)
                //        {
                //            anchorRowIndex = x;
                //            anchorColumnIndex = y;
                //        }
                //    }
                //}
                var anchorColumnIndexCopy = anchorColumnIndex;
                var wordBeforeAnchor = "";
                while (anchorColumnIndexCopy > 0)
                {
                    if (boardArray[anchorRowIndex, anchorColumnIndexCopy - 1].CharTile == null)
                    {
                        limit++;
                    }
                    else if (boardArray[anchorRowIndex, anchorColumnIndexCopy - 1].CharTile != null)
                    {
                        if (limit == 0)
                        {
                            wordBeforeAnchor = wordBeforeAnchor.Insert(0, boardArray[anchorRowIndex, anchorColumnIndexCopy - 1].CharTile.Letter.ToString());
                        }
                    }
                    anchorColumnIndexCopy--;
                    if (anchorColumnIndexCopy > 0 && wordBeforeAnchor.Length != 0 && boardArray[anchorRowIndex, anchorColumnIndexCopy - 1].CharTile == null)
                    {
                        ExtendRight(wordBeforeAnchor, anchorAsBoardTile, new int[] { anchorRowIndex, anchorColumnIndex }, boardArray, validMovesList, boardIsHorizontal, boardBeforeMove);
                        break;
                    }
                }

                if (wordBeforeAnchor.Length == 0)
                {
                    foreach (var entry in Dictionary.CharTiles.Where(d => d.Score != 0))
                    {
                        if (RackOfCurrentPlayer.CheckIfTileIsInRack(entry.Letter, true)
                            && ((anchorCrossCheck == null) || (anchorCrossCheck != null && anchorCrossCheck.Contains(entry.Letter)
                                && anchorColumnIndex < boardArray.GetLength(1) - 1)))
                        {
                            if (Helper.CheckIfTimeLimitIsReached(Stopwatch, TimeLimit) && validMovesList.Count > 0)
                            {
                                return validMovesList;
                            }
                            var tile = RackOfCurrentPlayer.SubstractFromRack(entry.Letter);
                            anchorAsBoardTile.CharTile = tile;
                            LeftPart(tile.Letter.ToString(), limit, boardArray[anchorRowIndex, anchorColumnIndex + 1], new int[] { anchorRowIndex, anchorColumnIndex + 1 }, boardArray, validMovesList, boardIsHorizontal, boardBeforeMove);
                            RackOfCurrentPlayer.AddToRack(tile);
                            anchorAsBoardTile.CharTile = null;
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
                if (Helper.CheckIfTimeLimitIsReached(Stopwatch, TimeLimit))
                {
                    if (validMovesList.Count == 0)
                    {
                        if (retriesForCurrentAnchor != 0)
                        {
                            TimeLimit += 5;
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

        public void LeftPart(string partialWord, int limit, BoardTile anchor, int[] anchorCoordinates, BoardTile[,] boardArray,
            HashSet<GeneratedMove> validMovesList, bool boardIsHorizontal, BoardTile[,] boardBeforeMove)
        {
            ExtendRight(partialWord, anchor, anchorCoordinates, boardArray, validMovesList, boardIsHorizontal, boardBeforeMove);
            if (limit > 0)
            {
                var boardTileToTheLeft = boardArray[anchorCoordinates[0], anchorCoordinates[1] - 1];
                HashSet<string> labelsOfDawgEdges = new HashSet<string>(new DawgEdgeEqualityComparer());
                var wordsWithCommonPreffix = Dawg.MatchPrefix(partialWord);
                foreach (var word in wordsWithCommonPreffix)
                {
                    labelsOfDawgEdges.Add(word.Key.Substring(partialWord.Length));
                }
                var anchorCrossCheck = boardIsHorizontal ? boardTileToTheLeft.UntransposedCrossCheck : boardTileToTheLeft.TransposedCrossCheck;
                foreach (var label in labelsOfDawgEdges)
                {
                    if (label == "") continue;

                    if (OriginalRackOfPlayer.CheckIfWordIsPlayable(partialWord + label[0]) &&
                        ((anchorCrossCheck == null)
                        || (anchorCrossCheck != null && anchorCrossCheck.Contains(label[0]))))
                    {
                        CharTile tileToWorkWith = null;
                        if (RackOfCurrentPlayer.CheckIfTileIsInRack(label[0], false))
                        {
                            tileToWorkWith = Dictionary.CharTiles.Where(c => c.Letter == label[0] && c.Score != 0).FirstOrDefault();
                        }
                        else
                        {
                            tileToWorkWith = Dictionary.CharTiles.Where(c => c.ID == 1).FirstOrDefault();
                        }
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            boardArray[anchorCoordinates[0], anchorCoordinates[1] - 1 - i].CharTile = null;
                        }
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            var tileToAdd = OriginalRackOfPlayer.GetTile(partialWord[i]);
                            boardArray[anchorCoordinates[0], anchorCoordinates[1] - partialWord.Length - 1 + i].CharTile = tileToAdd;
                        }
                        if (Helper.CheckIfTimeLimitIsReached(Stopwatch, TimeLimit) && validMovesList.Count > 0)
                        {
                            return;
                        }
                        RackOfCurrentPlayer.SubstractFromRack(tileToWorkWith);
                        boardTileToTheLeft.CharTile = tileToWorkWith;
                        bool validPrefix = true;
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            var boardTile = boardArray[anchorCoordinates[0], anchorCoordinates[1] - 1 - partialWord.Length + i];
                            var ancorCrossCheckTemp = boardIsHorizontal ? boardTile.UntransposedCrossCheck : boardTile.TransposedCrossCheck;
                            if (!(ancorCrossCheckTemp == null
                                   || (ancorCrossCheckTemp != null && ancorCrossCheckTemp.Contains(partialWord[i]))))
                            {

                                //ValidCrossChecks.TryGetValue(boardTileToTheLeft, out List<CharTile> value) && value.Any(x => x.Letter == label[0]
                                validPrefix = false;
                                break;
                            }
                        }

                        if (validPrefix) LeftPart(partialWord + label[0], limit - 1, anchor, anchorCoordinates, boardArray, validMovesList, boardIsHorizontal, boardBeforeMove);
                        RackOfCurrentPlayer.AddToRack(tileToWorkWith);
                        boardTileToTheLeft.CharTile = null;
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            boardArray[anchorCoordinates[0], anchorCoordinates[1] - partialWord.Length - 1 + i].CharTile = null;
                        }
                        if (Helper.CheckIfTimeLimitIsReached(Stopwatch, TimeLimit) && validMovesList.Count > 0)
                        {
                            return;
                        }
                    }
                }
            }
        }

        public void ExtendRight(string partialWord, BoardTile anchor, int[] anchorCoordinates, BoardTile[,] boardArray
            , HashSet<GeneratedMove> validMovesList, bool boardIsHorizontal, BoardTile[,] boardBeforeMove,
            CharTile tileExtendedWith = null)
        {

            if (anchor.CharTile == null)
            {
                if (Helper.CheckWordValidity(Dawg, partialWord))
                {
                    if (!Moves.Any(m => m.Word.Equals(partialWord)))
                    {
                        Dictionary<BoardTile, CharTile> tilesUsed = new Dictionary<BoardTile, CharTile>();
                        Dictionary<BoardTile, CharTile> rackTilesUsed = new Dictionary<BoardTile, CharTile>();
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            var letter = partialWord[i];
                            var boardTile = boardArray[anchorCoordinates[0], anchorCoordinates[1] - partialWord.Length + i];

                            if (boardTile.CharTile != null)
                            {
                                tilesUsed.Add(boardTile, boardTile.CharTile);
                            }
                            else
                            {
                                tilesUsed.Add(boardTile, tileExtendedWith);
                            }
                        }
                        validMovesList.Add(new GeneratedMove(boardIsHorizontal, anchorCoordinates[1] - partialWord.Length, anchorCoordinates[1] - 1, anchorCoordinates, anchor, tilesUsed, boardBeforeMove));
                    }
                }
                HashSet<string> labelsOfDawgEdges = new HashSet<string>(new DawgEdgeEqualityComparer());
                var wordsWithCommonPreffix = Dawg.MatchPrefix(partialWord);
                foreach (var word in wordsWithCommonPreffix)
                {
                    labelsOfDawgEdges.Add(word.Key.Substring(partialWord.Length));
                }
                var anchorCrossCheck = boardIsHorizontal ? anchor.UntransposedCrossCheck : anchor.TransposedCrossCheck;
                foreach (var label in labelsOfDawgEdges)
                {
                    if (label == "") continue;
                    if (RackOfCurrentPlayer.CheckIfTileIsInRack(label[0], true) &&
                        (anchorCrossCheck == null
                        || (anchorCrossCheck != null && anchorCrossCheck.Contains(label[0]))))
                    {
                        CharTile tileToWorkWith = null;
                        if (RackOfCurrentPlayer.CheckIfTileIsInRack(label[0], false))
                        {
                            tileToWorkWith = Dictionary.CharTiles.Where(c => c.Letter == label[0] && c.Score != 0).FirstOrDefault();
                        }
                        else
                        {
                            tileToWorkWith = Dictionary.CharTiles.Where(c => c.ID == 1).FirstOrDefault();
                        }
                        if (Helper.CheckIfTimeLimitIsReached(Stopwatch, TimeLimit) && validMovesList.Count > 0)
                        {
                            return;
                        }
                        RackOfCurrentPlayer.SubstractFromRack(tileToWorkWith);
                        anchor.CharTile = Dictionary.CharTiles.Where(c => c.Letter == label[0] && c.Score == tileToWorkWith.Score).FirstOrDefault();
                        if (anchorCoordinates[1] < boardArray.GetLength(1) - 1)
                        {
                            ExtendRight(partialWord + label[0], boardArray[anchorCoordinates[0], anchorCoordinates[1] + 1], new int[] { anchorCoordinates[0], anchorCoordinates[1] + 1 }, boardArray, validMovesList, boardIsHorizontal, boardBeforeMove, tileToWorkWith);
                        }
                        else
                        {
                            var finalWord = partialWord + anchor.CharTile.Letter;
                            if (Helper.CheckWordValidity(Dawg, finalWord))
                            {
                                if (!Moves.Any(m => m.Word.Equals(finalWord)))
                                {
                                    Dictionary<BoardTile, CharTile> tilesUsed = new Dictionary<BoardTile, CharTile>();
                                    for (int i = 0; i < finalWord.Length; i++)
                                    {
                                        var letter = finalWord[i];
                                        var boardTile = boardArray[anchorCoordinates[0], anchorCoordinates[1] - finalWord.Length + 1 + i];
                                        if (boardTile.CharTile != null)
                                        {
                                            tilesUsed.Add(boardTile, boardTile.CharTile);
                                        }
                                        else
                                        {
                                            tilesUsed.Add(boardTile, tileToWorkWith);
                                        }
                                    }
                                    validMovesList.Add(new GeneratedMove(boardIsHorizontal, anchorCoordinates[1] - finalWord.Length + 1, anchorCoordinates[1], anchorCoordinates, anchor, tilesUsed, boardBeforeMove));
                                }
                            }
                        }
                        RackOfCurrentPlayer.AddToRack(tileToWorkWith);
                        anchor.CharTile = null;
                        if (Helper.CheckIfTimeLimitIsReached(Stopwatch, TimeLimit) && validMovesList.Count > 0)
                        {
                            return;
                        }
                    }
                }
            }
            else
            {
                var tile = anchor.CharTile;
                HashSet<string> labelsOfDawgEdges = new HashSet<string>(new DawgEdgeEqualityComparer());
                var wordsWithCommonPreffix = Dawg.MatchPrefix(partialWord + tile.Letter);
                foreach (var word in wordsWithCommonPreffix)
                {
                    labelsOfDawgEdges.Add(word.Key.Substring((partialWord + tile.Letter).Length));
                }
                if (labelsOfDawgEdges.Any() && anchorCoordinates[1] < boardArray.GetLength(1) - 1)
                {
                    ExtendRight(partialWord + tile.Letter, boardArray[anchorCoordinates[0], anchorCoordinates[1] + 1], new int[] { anchorCoordinates[0], anchorCoordinates[1] + 1 }, boardArray, validMovesList, boardIsHorizontal, boardBeforeMove, tile);
                }
            }
        }
    }
}
