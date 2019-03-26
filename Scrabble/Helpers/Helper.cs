using DawgSharp;
using Microsoft.Extensions.Primitives;
using Scrabble.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Scrabble.Helpers
{
    public static class Helper
    {
        public static void MakeEnglishDictionary()
        {
            var dawgBuilder = new DawgBuilder<bool>(); // <bool> is the value type.
                                                       // Key type is always string.
            string[] lines = File.ReadLines(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\englishWords.txt").ToArray();
            //foreach (string key in lines)
            //{
            //    dawgBuilder.Insert(key, true);
            //}

            foreach (string key in lines)
            {
                dawgBuilder.Insert(key, true);
            }

            //foreach (string key in new[] { "Aaron", "abacus", "abashed" })
            //{
            //    dawgBuilder.Insert(key, true);
            //}

            var dawg = dawgBuilder.BuildDawg(); // Computer is working.  Please wait ...

            //dawg.SaveTo(File.Create(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\EnglishDAWG.bin"));
            dawg.SaveTo(File.Create(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\test.bin"));
            //dawg.SaveTo(File.Create(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\dawg.bin"));

            //var h = "hello";
        }
        public static bool CheckWordValidity(string word, bool alwaysExists = false)
        {
            Stream fs = File.Open(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\englishDawg.bin", FileMode.Open, FileAccess.Read);
            var dawg = Dawg<bool>.Load(fs);

            if (!alwaysExists && !dawg[word.ToLower()])
            {
                return false;
            }
            return true;
        }
        public static int GetWordScore (List<BoardTile> word)
        {
            int score = 0;
            var doubleWordTilesUsed = 0;
            var tripleWordTilesUsed = 0;
            foreach (BoardTile letter in word)
            {
                switch (letter.BoardTileType.Type)
                {
                    case "DoubleLetter":
                        score += letter.CharTile.Score * 2;
                        break;
                    case "TripleLetter":
                        score += letter.CharTile.Score * 3;
                        break;
                    case "DoubleWord":
                        doubleWordTilesUsed += 1;
                        score += letter.CharTile.Score;
                        break;
                    case "TripleWord":
                        tripleWordTilesUsed += 1;
                        score += letter.CharTile.Score;
                        break;
                    default:
                        score += letter.CharTile.Score;
                        break;
                }
            }
            if (doubleWordTilesUsed != 0)
            {
                score = score * (2 * doubleWordTilesUsed);
            }
            if (tripleWordTilesUsed != 0)
            {
                score = score * (3 * tripleWordTilesUsed);
            }
            return score;
        }
        public static string[] GetPlayedWords(List<KeyValuePair<string, StringValues>> data)
        {
            string words = "";
            foreach (var dataRow in data)
            {
                if (dataRow.Key.Contains("playedWords"))
                {
                    words += dataRow.Value + ";";
                }
            }
            if (words.Length >= 1)
            {
                words = words.Remove(words.Length - 1, 1);
            }         
            return words.Split(";");
        }
        public static string[] GetPlayedRackTiles (List<KeyValuePair<string, StringValues>> data)
        {
            string tiles = "";
            foreach (var dataRow in data)
            {
                if (dataRow.Key.Contains("playedRackTiles"))
                {
                    tiles += dataRow.Value;
                }
            }
            return tiles.Split(",");
        }
        public static int[] GetTileDetails (string tile)
        {
            var tileDetails = tile.Split("_");
            int tileX = Int32.Parse(tileDetails[0]);
            int tileY = Int32.Parse(tileDetails[1]);
            int tileCharTileId = Int32.Parse(tileDetails[2]);
            return new int[] { tileX, tileY, tileCharTileId };
        }
        public static bool IsRackPlayOneWayAndConnected (string[] rackTiles)
        {
            HashSet<int> rowsUsed = new HashSet<int>();
            HashSet<int> columnsUsed = new HashSet<int>();
            foreach (var tile in rackTiles)
            {
                int[] tileDetails = GetTileDetails(tile);
                rowsUsed.Add(tileDetails[0]);
                columnsUsed.Add(tileDetails[1]);
            }
            if (rowsUsed.Count > 1 && columnsUsed.Count > 1)
            {
                return false;
            }
            return true;
        }
        public static bool ValidateStart (BoardTile[,] boardAsArray, string[] playedRackTiles)
        {
            int[] startBoardTileCoordinates = null;
            for (int i = 0; i < boardAsArray.GetLength(0); i++)
            {
                for(int j = 0; j < boardAsArray.GetLength(1); j++)
                {
                    if (boardAsArray[i, j].BoardTileType.Type == "Start")
                    {
                        startBoardTileCoordinates = new int[] { i, j };
                    }
                    if (boardAsArray[i, j].CharTile != null) return true;
                }
            }
            bool startTileIsFilled = false;
            bool tilesBeforeStartFilled = false;
            foreach (var tile in playedRackTiles)
            {
                var tileDetails = GetTileDetails(tile);
                if (tileDetails[0] == startBoardTileCoordinates[0] && tileDetails[1] == startBoardTileCoordinates[1])
                {
                    startTileIsFilled = true;
                }
                if (tileDetails[0] < startBoardTileCoordinates[0] || tileDetails[1] < startBoardTileCoordinates[1])
                {
                    tilesBeforeStartFilled = true;
                }
            }
            if (startTileIsFilled && !tilesBeforeStartFilled)
            {
                return true;
            }
            return false;

        }
        public static bool IsRackPlayConnected(BoardTile[,] boardAsArray, string[] rackTiles)
        {
            HashSet<int> rowsUsed = new HashSet<int>();
            HashSet<int> columnsUsed = new HashSet<int>();           
            foreach (var tile in rackTiles)
            {
                int[] tileDetails = GetTileDetails(tile);
                rowsUsed.Add(tileDetails[0]);
                columnsUsed.Add(tileDetails[1]);
            }
            if (rowsUsed.Count > 1 && columnsUsed.Count > 1)
            {
                return false;
            }
            if (rowsUsed.Count == 1 && columnsUsed.Count == 1)
            {
                var tileIsConnected = false;
                var rowIndex = rowsUsed.FirstOrDefault();
                var columnIndex = columnsUsed.FirstOrDefault();
                if ((boardAsArray[rowIndex + 1, columnIndex] != null && boardAsArray[rowIndex + 1, columnIndex].CharTile != null)
                    || (boardAsArray[rowIndex - 1, columnIndex] != null && boardAsArray[rowIndex - 1, columnIndex].CharTile != null)
                    || (boardAsArray[rowIndex, columnIndex + 1] != null && boardAsArray[rowIndex, columnIndex + 1].CharTile != null)
                    || (boardAsArray[rowIndex, columnIndex - 1] != null && boardAsArray[rowIndex, columnIndex - 1].CharTile != null))
                {
                    tileIsConnected = true;
                }
                if (!tileIsConnected)
                {
                    return false;
                }
            }
            var directionOfPlay = "";
            var rowsUsedSorted = rowsUsed.ToList();
            rowsUsedSorted.Sort();
            var columnsUsedSorted = columnsUsed.ToList();
            columnsUsedSorted.Sort();
            var multipleIndexes = rowsUsedSorted.Count > columnsUsedSorted.Count ? rowsUsedSorted : columnsUsedSorted;
            var secondaryIndex = rowsUsedSorted.Count > columnsUsedSorted.Count ? columnsUsedSorted : rowsUsedSorted;
            directionOfPlay = rowsUsedSorted.Count > columnsUsedSorted.Count ? "vertical" : "horizontal";
            for (var i = multipleIndexes[0]; i < multipleIndexes[multipleIndexes.Count - 1]; i++)
            {
                var checkedTile = directionOfPlay == "horizontal" ? boardAsArray[secondaryIndex[0], i] : boardAsArray[i, secondaryIndex[0]];
                if (checkedTile.CharTile == null)
                {
                    var boardTileHasTemporaryPlacedRackTile = false;
                    foreach (var rackTile in rackTiles)
                    {                     
                        var tileDetails = GetTileDetails(rackTile);
                        if (tileDetails[0] == checkedTile.BoardLocationX && tileDetails[1] == checkedTile.BoardLocationY)
                        {
                            boardTileHasTemporaryPlacedRackTile = true;
                            break;
                        }
                    }
                    if (!boardTileHasTemporaryPlacedRackTile)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static HttpStatusCodeResult GetWordScores(Game game, List<KeyValuePair<string, StringValues>> data) {
            var playedWords = GetPlayedWords(data);
            var playedRackTiles = GetPlayedRackTiles(data);
            string logBuilder = "Player" + game.GetPlayerAtHand().ID + " played ";            
            for (int i = 0; i < playedWords.Length; i++)
            {
                var playedWord = playedWords[i];
                var currentScoreOfMove = 0;
                var playedWordString = "";
                var usedBoardTiles = new List<BoardTile>();                
                var playedTiles = playedWord.Split(",");
                foreach (string playedTile in playedTiles)
                {
                    int[] tileDetails = GetTileDetails(playedTile);
                    int tileX = tileDetails[0];
                    int tileY = tileDetails[1];
                    int tileCharTileId = tileDetails[2];
                    playedWordString += game.WordDictionary.CharTiles.Where(c => c.ID == tileCharTileId).FirstOrDefault().Letter;
                    game.Board.PlayTile(tileX, tileY, tileCharTileId, usedBoardTiles);
                }                

                if (!CheckWordValidity(playedWordString, true))
                {
                    return new HttpStatusCodeResult(400, playedWordString + " is not a legal word.");
                }
                currentScoreOfMove = GetWordScore(usedBoardTiles);
                playedWordString = playedWordString.ToUpper();
                game.AddScoreToPlayer(game.GetPlayerAtHand(), currentScoreOfMove);
                logBuilder += playedWordString + " for " + currentScoreOfMove + " points";
                if (i == playedWords.Length - 1)
                {
                    logBuilder += ". ";
                } else if (i == playedWords.Length - 2)
                {
                    logBuilder += " and ";
                } else
                {
                    logBuilder += " , ";
                }
            }

            foreach (var playedRackTile in playedRackTiles)
            {
                var tileDetails = GetTileDetails(playedRackTile);
                game.GetPlayerAtHand().Rack.SubstractFromRack(game.WordDictionary.CharTiles.Where(c => c.ID == tileDetails[2]).FirstOrDefault());
                game.GetPlayerAtHand().Rack.DrawFromPouch();
            }

            game.Log += logBuilder;
            game.Log += "Player" + game.GetPlayerAtHand().ID + " now has " + game.GetPlayerAtHand().Score + " points.";
            game.Log += "-------------------------";
            return new HttpStatusCodeResult(200, "Good move :)");
        }
    }
}
