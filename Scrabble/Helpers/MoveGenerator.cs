using DawgSharp;
using Scrabble.Classes;
using Scrabble.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Helpers
{
    public class MoveGenerator
    {
        public Game Game { get; set; }
        public Rack RackOfCurrentPlayer { get; set; }
        public BoardTile[,] BoardArray { get; set; }
        public BoardTile[,] TransposedBoardArray { get; set; }
        public Dawg<bool> Dawg { get; set; }
        public List<int[]> ListOfValidAnchorCoordinatesOnUntransposedBoard { get; set; }
        public List<int[]> ListOfValidAnchorCoordinatesOnTransposedBoard { get; set; }
        public Dictionary<BoardTile, List<CharTile>> ValidCrossChecks { get; set; }
        public WordDictionary Dictionary { get; set; }
        public List<Move> Moves { get; set; }

        public MoveGenerator(Game game, BoardTile[,] boardArray, BoardTile[,] transposedBoardArray, Dawg<bool> dawg, List<int[]> listOfValidAnchorCoordinatesOnUntransposedBoard, List<int[]> listOfValidAnchorCoordinatesOnTransposedBoard, Dictionary<BoardTile,
            List<CharTile>> validCrossChecks, WordDictionary dictionary, List<Move> moves)
        {
            Game = game;
            RackOfCurrentPlayer = game.GetPlayerAtHand().Rack;
            BoardArray = boardArray;
            TransposedBoardArray = transposedBoardArray;
            Dawg = dawg;
            ListOfValidAnchorCoordinatesOnUntransposedBoard = listOfValidAnchorCoordinatesOnUntransposedBoard;
            ListOfValidAnchorCoordinatesOnTransposedBoard = listOfValidAnchorCoordinatesOnTransposedBoard;
            ValidCrossChecks = validCrossChecks;
            Dictionary = dictionary;
            Moves = moves;
        }

        public HashSet<GeneratedMove> GetValidMoves(bool boardIsHorizontal)
        {
            //ListOfValidAnchorCoordinates = new List<int[]>(new int[][] { new int[] { 7, 7 }, new int[] { 7, 8 }, new int[] { 7, 9 } });
            var boardArray = boardIsHorizontal ? BoardArray : TransposedBoardArray;
            var listOfValidAnchorCoordinates = boardIsHorizontal ? ListOfValidAnchorCoordinatesOnUntransposedBoard : ListOfValidAnchorCoordinatesOnTransposedBoard;
            var validCrossChecks = ValidCrossChecks;
            HashSet<GeneratedMove> validMovesList = new HashSet<GeneratedMove>(new GeneratedMoveEqualityComparer());
            foreach (var anchor in listOfValidAnchorCoordinates)
            {
                var limit = 0;
                var columnIndex = anchor[1];
                var wordBeforeAnchor = "";
                while (columnIndex > 0)
                {
                    if (boardArray[anchor[0], columnIndex - 1].CharTile == null)
                    {
                        limit++;
                    }
                    else if (boardArray[anchor[0], columnIndex - 1].CharTile != null)
                    {
                        if (limit == 0)
                        {
                            wordBeforeAnchor += wordBeforeAnchor.Insert(0, boardArray[anchor[0], columnIndex - 1].CharTile.ToString());
                        }
                    }
                    columnIndex--;
                    if (columnIndex > 0 && wordBeforeAnchor.Length != 0 && boardArray[anchor[0], columnIndex - 1].CharTile == null)
                    {
                        ExtendRight(wordBeforeAnchor, anchor, boardArray, validCrossChecks, validMovesList, boardIsHorizontal);
                        break;
                    }
                }
                if (wordBeforeAnchor.Length == 0) LeftPart("", limit, anchor, boardArray, validCrossChecks, validMovesList, boardIsHorizontal);

            }
            return validMovesList;
        }

        public void LeftPart(string partialWord, int limit, int[] anchor, BoardTile[,] boardArray, Dictionary<BoardTile, List<CharTile>> validCrossChecks, HashSet<GeneratedMove> validMovesList, bool boardIsHorizontal)
        {
            ExtendRight(partialWord, anchor, boardArray, validCrossChecks, validMovesList, boardIsHorizontal);
            if (limit > 0)
            {
                HashSet<string> labelsOfDawgEdges = new HashSet<string>(new DawgEdgeEqualityComparer());
                var wordsWithCommonPreffix = Dawg.MatchPrefix(partialWord);
                foreach (var word in wordsWithCommonPreffix)
                {
                    labelsOfDawgEdges.Add(word.Key.Substring(partialWord.Length));
                }
                foreach (var label in labelsOfDawgEdges)
                {
                    if (label == "") continue;
                    if (RackOfCurrentPlayer.CheckIfTileIsInRack(label[0]) &&
                        ((!validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1] - 1 - partialWord.Length])
                        || validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1] - 1 - partialWord.Length] &&
                        c.Value.Any(x => x.Letter == label[0])))))
                    {
                        RackOfCurrentPlayer.SubstractFromRack(label[0]);
                        LeftPart(partialWord + label[0], limit - 1, anchor, boardArray, validCrossChecks, validMovesList, boardIsHorizontal);
                        RackOfCurrentPlayer.AddToRack(label[0]);
                    }
                }
            }
        }

        public void ExtendRight(string partialWord, int[] anchor, BoardTile[,] boardArray, Dictionary<BoardTile, List<CharTile>> validCrossChecks, HashSet<GeneratedMove> validMovesList, bool boardIsHorizontal)
        {
            if (boardArray[anchor[0], anchor[1]].CharTile == null)
            {
                if (Helper.CheckWordValidity(Dawg, partialWord))
                {
                    if (!Moves.Any(m => m.Word.Equals(partialWord)))
                    {
                        Dictionary<BoardTile, CharTile> tilesUsed = new Dictionary<BoardTile, CharTile>();
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            tilesUsed.Add(boardArray[anchor[0], anchor[1] + i - partialWord.Length], Dictionary.CharTiles.Where(c => c.Letter == partialWord[i]).FirstOrDefault());
                        }
                        validMovesList.Add(new GeneratedMove(boardIsHorizontal, anchor[1] - partialWord.Length, anchor[1], anchor[0], tilesUsed));
                    }
                }
                HashSet<string> labelsOfDawgEdges = new HashSet<string>(new DawgEdgeEqualityComparer());
                var wordsWithCommonPreffix = Dawg.MatchPrefix(partialWord);
                foreach (var word in wordsWithCommonPreffix)
                {
                    labelsOfDawgEdges.Add(word.Key.Substring(partialWord.Length));
                }
                foreach (var label in labelsOfDawgEdges)
                {
                    if (label == "") continue;
                    if (RackOfCurrentPlayer.CheckIfTileIsInRack(label[0]) &&
                        ((!validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1]])
                        || validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1]] && c.Value.Any(x => x.Letter == label[0])))))
                    {
                        RackOfCurrentPlayer.SubstractFromRack(label[0]);
                        if (anchor[1] + 1 < boardArray.GetLength(1) - 1)
                        {
                            ExtendRight(partialWord + label[0], new int[] { anchor[0], anchor[1] + 1 }, boardArray, validCrossChecks, validMovesList, boardIsHorizontal);
                        }
                        RackOfCurrentPlayer.AddToRack(label[0]);
                    }
                }
            }
            else
            {
                var tile = boardArray[anchor[0], anchor[1]].CharTile;
                HashSet<string> labelsOfDawgEdges = new HashSet<string>(new DawgEdgeEqualityComparer());
                var wordsWithCommonPreffix = Dawg.MatchPrefix(partialWord + tile.Letter);
                foreach (var word in wordsWithCommonPreffix)
                {
                    labelsOfDawgEdges.Add(word.Key.Substring((partialWord + tile.Letter).Length));
                }
                if (labelsOfDawgEdges.Any() && anchor[1] + 1 < boardArray.GetLength(1) - 1)
                {
                    ExtendRight(partialWord + tile.Letter, new int[] { anchor[0], anchor[1] + 1 }, boardArray, validCrossChecks, validMovesList, boardIsHorizontal);
                }
            }
        }
    }
}
