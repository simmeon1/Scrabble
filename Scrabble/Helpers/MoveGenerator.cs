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
        public List<int[]> ListOfValidAnchorCoordinates { get; set; }
        public Dictionary<int[], List<CharTile>> ValidCrossChecks { get; set; }

        public MoveGenerator(Game game, BoardTile[,] boardArray, BoardTile[,] transposedBoardArray, Dawg<bool> dawg,
            List<int[]> listOfValidAnchorCoordinates, Dictionary<int[], List<CharTile>> validCrossChecks)
        {
            Game = game;
            RackOfCurrentPlayer = game.GetPlayerAtHand().Rack;
            BoardArray = boardArray;
            TransposedBoardArray = transposedBoardArray;
            Dawg = dawg;
            ListOfValidAnchorCoordinates = listOfValidAnchorCoordinates;
            ValidCrossChecks = validCrossChecks;
        }

        public void GetValidMoves()
        {
            ListOfValidAnchorCoordinates = new List<int[]>(new int[][] { new int[] { 7, 7 }, new int[] { 7, 8 }, new int[] { 7, 9 } });
            Dictionary<int[], string> validMoves = new Dictionary<int[], string>();
            foreach (var anchor in ListOfValidAnchorCoordinates)
            {
                var limit = 0;
                var columnIndex = anchor[1];
                while (columnIndex > 0 && !ListOfValidAnchorCoordinates.Any(a => a.SequenceEqual(new int[] { anchor[0], columnIndex - 1 })))
                {
                    limit++;
                    columnIndex--;
                }

                LeftPart("", limit, anchor, validMoves);
                
            }
            var x = 0;
        }

        public void LeftPart(string partialWord, int limit, int[] anchor, Dictionary<int[], string> validMoves)
        {
            ExtendRight(partialWord, anchor, validMoves);
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
                    if (RackOfCurrentPlayer.CheckIfTileIsInRack(label[0]))
                    {
                        RackOfCurrentPlayer.SubstractFromRack(label[0]);
                        LeftPart(partialWord + label[0], limit - 1, anchor, validMoves);
                        RackOfCurrentPlayer.AddToRack(label[0]);
                    }
                }
            }
        }

        public void ExtendRight(string partialWord, int[] anchor, Dictionary<int[], string> validMoves)
        {
            if (BoardArray[anchor[0], anchor[1]].CharTile == null)
            {
                if (Helper.CheckWordValidity(Dawg, partialWord))
                {
                    validMoves.Add(new int[] { anchor[0], anchor[1] }, partialWord);
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
                        (!ValidCrossChecks.Any(c => c.Key[0] == anchor[0] && c.Key[1] == anchor[1]) 
                        || ValidCrossChecks.Any(c => c.Key[0] == anchor[0] && c.Key[1] == anchor[1] && c.Value.Any(x => x.Letter == label[0]))))
                    {
                        RackOfCurrentPlayer.SubstractFromRack(label[0]);
                        if (anchor[1] + 1 < BoardArray.GetLength(1) - 1)
                        {
                            ExtendRight(partialWord + label[0], new int[] { anchor[0], anchor[1] + 1 }, validMoves);
                        }
                        RackOfCurrentPlayer.AddToRack(label[0]);
                    }
                }
            }
            else
            {
                var tile = BoardArray[anchor[0], anchor[1]].CharTile;
                HashSet<string> labelsOfDawgEdges = new HashSet<string>(new DawgEdgeEqualityComparer());
                var wordsWithCommonPreffix = Dawg.MatchPrefix(partialWord + tile.Letter);
                foreach (var word in wordsWithCommonPreffix)
                {
                    labelsOfDawgEdges.Add(word.Key.Substring((partialWord + tile.Letter).Length));
                }
                if (labelsOfDawgEdges.Any() && anchor[1] + 1 < BoardArray.GetLength(1) - 1)
                {
                    ExtendRight(partialWord + tile.Letter, new int[] { anchor[0], anchor[1] + 1 }, validMoves);
                }
            }
        }
    }
}
