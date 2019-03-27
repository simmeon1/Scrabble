using DawgSharp;
using Microsoft.Extensions.Primitives;
using Scrabble.Classes;
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

            foreach (string key in new[] { "Aaron", "abacus", "abashed" })
            {
                dawgBuilder.Insert(key, true);
            }

            var dawg = dawgBuilder.BuildDawg(); // Computer is working.  Please wait ...

            dawg.SaveTo(File.Create(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\englishDawg.bin"));

        }
        public static Dawg<bool> LoadDawg (GameLanguage language)
        {
            Stream fs = File.Open(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\" + language.Language + "Dawg.bin", FileMode.Open, FileAccess.Read);
            var dawg = Dawg<bool>.Load(fs);
            return dawg;
        }
        public static bool CheckWordValidity(Dawg<bool> dawg, string word, bool alwaysExists = false)
        {
            
            if (!alwaysExists && !dawg[word.ToUpper()])
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

        public static Dictionary<int[], List<CharTile>> GetValidCrossChecksOneWay(BoardTile[,] boardArray, WordDictionary dictionary, bool boardIsTransposed)
        {
            var dawg = LoadDawg(dictionary.GameLanguage);
            Dictionary<int[], List<CharTile>> validCrossChecks = new Dictionary<int[], List<CharTile>>(new CoordinatesEqualityComparer());
            List<int[]> tilesAlreadyChecked = new List<int[]>();
            for (int i = 0; i < boardArray.GetLength(0); i++)
            {
                for (int j = 0; j < boardArray.GetLength(1); j++) {
                    if (boardArray[i,j].CharTile == null)
                    {
                        var wordAboveEmptyTile = "";
                        var wordUnderEmptyTile = "";
                        var upIndexCounter = i;
                        var downIndexCounter = i;                       
                        
                        while (upIndexCounter > 0 && boardArray[upIndexCounter - 1, j].CharTile != null)
                        {
                            wordAboveEmptyTile = wordAboveEmptyTile.Insert(0, boardArray[upIndexCounter - 1, j].CharTile.Letter.ToString());
                            upIndexCounter--;
                        }
                        
                        while (downIndexCounter < boardArray.GetLength(0) - 1 && boardArray[downIndexCounter + 1, j].CharTile != null)
                        {
                            wordUnderEmptyTile += boardArray[downIndexCounter + 1, j].CharTile.Letter;
                            downIndexCounter++;
                        }
                        var combinedWord = "";
                        combinedWord = wordAboveEmptyTile + "_" + wordUnderEmptyTile;
                        if (boardIsTransposed)
                        {

                            combinedWord = ReverseString(combinedWord);
                        }
                        else
                        {
                            combinedWord = wordAboveEmptyTile + "_" + wordUnderEmptyTile;
                        }
                        if (combinedWord == "_")
                        {
                            continue;
                        } else {
                            var rowIndexOnOriginalBoard = boardArray[i, j].BoardLocationX;
                            var columnIndexOnOriginalBoard = boardArray[i, j].BoardLocationY;
                            if (!validCrossChecks.ContainsKey(new int[] { rowIndexOnOriginalBoard, columnIndexOnOriginalBoard }))
                            {
                                validCrossChecks[new int[] { rowIndexOnOriginalBoard, columnIndexOnOriginalBoard }] = new List<CharTile>();
                            }
                            foreach (var c in dictionary.CharTiles)
                            {
                                var combinedWordTemp = String.Copy(combinedWord);
                                combinedWordTemp = combinedWordTemp.Replace('_', c.Letter);
                                if (CheckWordValidity(dawg, combinedWordTemp))
                                {
                                    if (validCrossChecks.ContainsKey(new int[] { rowIndexOnOriginalBoard, columnIndexOnOriginalBoard } )) {
                                        if (!validCrossChecks[new int[] { rowIndexOnOriginalBoard, columnIndexOnOriginalBoard }].Contains(c))
                                        {
                                            validCrossChecks[new int[] { rowIndexOnOriginalBoard, columnIndexOnOriginalBoard }].Add(c);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return validCrossChecks;
        }
        public static Dictionary<int[], List<CharTile>> GetValidCrossChecksCombined(Dictionary<int[], List<CharTile>> validHorizontalCrossChecks, Dictionary<int[], List<CharTile>> validVerticalCrossChecks)
        {
            Dictionary<int[], List<CharTile>> validCrossChecks = new Dictionary<int[], List<CharTile>>(new CoordinatesEqualityComparer());
            for (int i = 0; i < validHorizontalCrossChecks.Count; i++)
            {
                var keyAtIndex = validHorizontalCrossChecks.Keys.ElementAt(i);
                var valueAtIndex = validHorizontalCrossChecks.Values.ElementAt(i);
                validCrossChecks.Add(keyAtIndex, valueAtIndex);
            }
            for (int i = 0; i < validVerticalCrossChecks.Count; i++)
            {
                var keyAtIndex = validVerticalCrossChecks.Keys.ElementAt(i);
                var valueAtIndex = validVerticalCrossChecks.Values.ElementAt(i);
                if (!validCrossChecks.ContainsKey(keyAtIndex))
                {
                    validCrossChecks.Add(keyAtIndex, valueAtIndex);
                }
                else
                {
                    List<CharTile> validTilesForBothChecks = valueAtIndex.Intersect(validCrossChecks[keyAtIndex]).ToList();
                    validCrossChecks[keyAtIndex] = validTilesForBothChecks;
                }
            }
            return validCrossChecks;
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
        public static void GetBoardArrayFromHtml(List<KeyValuePair<string, StringValues>> data)
        {
            string boardArrayString = "";
            foreach (var dataRow in data)
            {
                if (dataRow.Key.Contains("boardArray"))
                {
                    boardArrayString += dataRow.Value + ";";
                }
            }
            if (boardArrayString.Length >= 1)
            {
                boardArrayString = boardArrayString.Remove(boardArrayString.Length - 1, 1);
            }
            var boardArrayAsRows = boardArrayString.Split(";");
            var rows = boardArrayAsRows.Length;
            int columns = boardArrayAsRows[0].Count(f => f == ',');
            var boardArray = new int[rows, columns];
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
            var dawg = LoadDawg(game.GameLanguage);
            var playerAtHand = game.GetPlayerAtHand();
            var playedWords = GetPlayedWords(data);
            var playedRackTiles = GetPlayedRackTiles(data);
            string logBuilder = "Player" + playerAtHand.ID + " played ";            
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
              
                if (!CheckWordValidity(dawg, playedWordString, false))
                {
                    return new HttpStatusCodeResult(400, playedWordString + " is not a legal word.");
                }
                currentScoreOfMove = GetWordScore(usedBoardTiles);
                playedWordString = playedWordString.ToUpper();
                game.AddScoreToPlayer(playerAtHand, currentScoreOfMove);
                playerAtHand.Moves.Add(new Move {PlayerID = playerAtHand.ID, GameID = game.ID, Word = playedWordString, Score = currentScoreOfMove });
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
                playerAtHand.Rack.SubstractFromRack(game.WordDictionary.CharTiles.Where(c => c.ID == tileDetails[2]).FirstOrDefault());
                playerAtHand.Rack.DrawFromPouch();
            }

            game.Log += logBuilder;
            game.Log += "Player" + playerAtHand.ID + " now has " + playerAtHand.Score + " points.";
            game.Log += "-------------------------";
            return new HttpStatusCodeResult(200, "Good move :)");
        }
        public static string ReverseString(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
