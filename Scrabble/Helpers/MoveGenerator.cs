﻿using DawgSharp;
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

        public MoveGenerator(Game game, BoardTile[,] boardArray, BoardTile[,] transposedBoardArray, Dawg<bool> dawg, List<int[]> listOfValidAnchorCoordinatesOnUntransposedBoard, List<int[]> listOfValidAnchorCoordinatesOnTransposedBoard, Dictionary<BoardTile,
            List<CharTile>> validUntransposedCrossChecks, Dictionary<BoardTile, List<CharTile>> validTransposedCrossChecks, WordDictionary dictionary, List<Move> moves)
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
                    ID = entry.ID
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
        }

        public HashSet<GeneratedMove> GetValidMoves(bool boardIsHorizontal, bool giveOneResult = false)
        {
            var boardArray = boardIsHorizontal ? BoardArray : TransposedBoardArray;
            var listOfValidAnchorCoordinates = boardIsHorizontal ? ListOfValidAnchorCoordinatesOnUntransposedBoard : ListOfValidAnchorCoordinatesOnTransposedBoard;
            var validCrossChecks = boardIsHorizontal ? ValidUntransposedCrossChecks : ValidTransposedCrossChecks;
            HashSet<GeneratedMove> validMovesList = new HashSet<GeneratedMove>(new GeneratedMoveEqualityComparer());
            foreach (var anchor in listOfValidAnchorCoordinates)
            {
                var crossCheckEntry = validCrossChecks.Where(c => c.Key == boardArray[anchor[0], anchor[1]]).FirstOrDefault().Value;
                if (crossCheckEntry != null && crossCheckEntry.Count == 0) continue;
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
                            wordBeforeAnchor = wordBeforeAnchor.Insert(0, boardArray[anchor[0], columnIndex - 1].CharTile.Letter.ToString());
                        }
                    }
                    columnIndex--;
                    if (columnIndex > 0 && wordBeforeAnchor.Length != 0 && boardArray[anchor[0], columnIndex - 1].CharTile == null)
                    {
                        ExtendRight(wordBeforeAnchor, anchor, boardArray, validCrossChecks, validMovesList, boardIsHorizontal);
                        break;
                    }
                }
                if (wordBeforeAnchor.Length == 0)
                {
                    foreach (var entry in Dictionary.CharTiles.Where(d => d.Score != 0)) {
                        if (RackOfCurrentPlayer.CheckIfTileIsInRack(entry.Letter, true) 
                            && (!validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1]])
                            || validCrossChecks.Any(c => c.Key == boardArray[anchor[0], anchor[1]]
                            && c.Value.Any(x => x.Letter == entry.Letter))) 
                            && anchor[1] < boardArray.GetLength(1))
                        {
                            var tile = RackOfCurrentPlayer.SubstractFromRack(entry.Letter);
                            boardArray[anchor[0], anchor[1]].CharTile = tile;
                            LeftPart(tile.Letter.ToString(), limit, new int []{ anchor[0], anchor[1] + 1}, boardArray, validCrossChecks, validMovesList, boardIsHorizontal);
                            RackOfCurrentPlayer.AddToRack(tile);
                            boardArray[anchor[0], anchor[1]].CharTile = null;
                        }
                    }
                }
                if (giveOneResult)
                {
                    return validMovesList;
                }

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
                            tileToWorkWith = Dictionary.CharTiles.Where(c => c.ID == 1).FirstOrDefault();
                        }
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

                        if (validPrefix) LeftPart(partialWord + label[0], limit - 1, anchor, boardArray, validCrossChecks, validMovesList, boardIsHorizontal);
                        RackOfCurrentPlayer.AddToRack(tileToWorkWith);
                        boardArray[anchor[0], anchor[1] - 1].CharTile = null;
                        for (int i = 0; i < partialWord.Length; i++)
                        {
                            boardArray[anchor[0], anchor[1] - partialWord.Length - 1 + i].CharTile = null;
                        }
                    }
                }
            }
        }

        public void ExtendRight(string partialWord, int[] anchor, BoardTile[,] boardArray, Dictionary<BoardTile,
            List<CharTile>> validCrossChecks, HashSet<GeneratedMove> validMovesList, bool boardIsHorizontal, CharTile tileExtendedWith = null)
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
                        validMovesList.Add(new GeneratedMove(boardIsHorizontal, anchor[1] - partialWord.Length, anchor[1], anchor, tilesUsed));
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
                            tileToWorkWith = Dictionary.CharTiles.Where(c => c.ID == 1).FirstOrDefault();
                        }
                        RackOfCurrentPlayer.SubstractFromRack(tileToWorkWith);
                        boardArray[anchor[0], anchor[1]].CharTile = Dictionary.CharTiles.Where(c => c.Letter == label[0] && c.Score == tileToWorkWith.Score).FirstOrDefault();
                        if (anchor[1] < boardArray.GetLength(1) - 1)
                        {
                            ExtendRight(partialWord + label[0], new int[] { anchor[0], anchor[1] + 1 }, boardArray, validCrossChecks, validMovesList, boardIsHorizontal, tileToWorkWith);
                        }
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
                                    validMovesList.Add(new GeneratedMove(boardIsHorizontal, anchor[1] - finalWord.Length, anchor[1], anchor, tilesUsed));
                                }
                            }
                        }
                        RackOfCurrentPlayer.AddToRack(tileToWorkWith);
                        boardArray[anchor[0], anchor[1]].CharTile = null;
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
                if (labelsOfDawgEdges.Any() && anchor[1] < boardArray.GetLength(1))
                {
                    ExtendRight(partialWord + tile.Letter, new int[] { anchor[0], anchor[1] + 1 }, boardArray, validCrossChecks, validMovesList, boardIsHorizontal, tile);
                }
            }
        }
    }
}
