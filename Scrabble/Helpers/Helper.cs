using DawgSharp;
using Microsoft.Extensions.Primitives;
using Scrabble.Classes;
using Scrabble.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Scrabble.Helpers
{
    public static class Helper
    {
        //public static void MakeEnglishDictionary()
        //{
        //    var dawgBuilder = new DawgBuilder<bool>(); // <bool> is the value type.
        //                                               // Key type is always string.
        //    string[] lines = File.ReadLines(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\englishWords.txt").ToArray();

        //    foreach (string key in new[] { "Aaron", "abacus", "abashed" })
        //    {
        //        dawgBuilder.Insert(key, true);
        //    }

        //    var dawg = dawgBuilder.BuildDawg(); // Computer is working.  Please wait ...

        //    dawg.SaveTo(File.Create(@"C:\Users\Simeon\Desktop\Scrabble\Scrabble\Helpers\englishDawg.bin"));

        //}

        /// <summary>
        /// Loads a DAWG (a dictionary of words as a compact trie).
        /// </summary>
        /// <param name="language">Language of dictionary to load</param>
        /// <returns></returns>
        public static Dawg<bool> LoadDawg(GameLanguage language)
        {
            Stream fs = File.Open(language.Language + "Dawg.bin", FileMode.Open, FileAccess.Read);
            var dawg = Dawg<bool>.Load(fs);
            return dawg;
        }

        /// <summary>
        /// Using word, checks if word is valid in dictionary
        /// </summary>
        /// <param name="dawg">Dawg to use</param>
        /// <param name="word">Word to check</param>
        /// <param name="alwaysExists">Says if any word is considered as valid</param>
        /// <returns></returns>
        public static bool CheckWordValidity(Dawg<bool> dawg, string word, bool alwaysExists = false)
        {

            if (!alwaysExists && !dawg[word.ToUpper()])
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the score of a played word on a board
        /// </summary>
        /// <param name="wordBoardTiles">Word that is played and the board tiles used for it</param>
        /// <returns></returns>
        public static int GetWordScore(List<BoardTile> wordBoardTiles)
        {
            int score = 0;
            var doubleWordTilesUsed = 0;
            var tripleWordTilesUsed = 0;
            foreach (BoardTile letter in wordBoardTiles)
            {
                if (!letter.IsFilled) {
                    switch (letter.BoardTileType.ID)
                    {
                        case BoardTileTypeEnum.Double_Letter:
                            score += letter.CharTile.Score * 2;
                            break;
                        case BoardTileTypeEnum.Triple_Letter:
                            score += letter.CharTile.Score * 3;
                            break;
                        case BoardTileTypeEnum.Double_Word:
                            doubleWordTilesUsed += 1;
                            score += letter.CharTile.Score;
                            break;
                        case BoardTileTypeEnum.Triple_Word:
                            tripleWordTilesUsed += 1;
                            score += letter.CharTile.Score;
                            break;
                        default:
                            score += letter.CharTile.Score;
                            break;
                    }
                }
                else score += letter.CharTile.Score;
            }
            if (doubleWordTilesUsed != 0)
            {
                score *= 2;
            }
            if (tripleWordTilesUsed != 0)
            {
                score *= 3;
            }
            return score;
        }

        /// <summary>
        /// Gets all played words from POST request ajax data
        /// </summary>
        /// <param name="data">The ajax data</param>
        /// <returns></returns>
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
        //public static Dictionary<BoardTile, List<CharTile>> GetValidCrossChecksOneWay(BoardTile[,] boardArray, WordDictionary dictionary)
        //{
        //    var dawg = LoadDawg(dictionary.GameLanguage);
        //    Dictionary<BoardTile, List<CharTile>> validCrossChecks = new Dictionary<BoardTile, List<CharTile>>();
        //    List<int[]> tilesAlreadyChecked = new List<int[]>();
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < boardArray.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < boardArray.GetLength(1); j++)
        //        {
        //            sb.Clear();
        //            if (boardArray[i, j].CharTile == null)
        //            {
        //                var upIndexCounter = i;
        //                var downIndexCounter = i;

        //                while (upIndexCounter > 0 && boardArray[upIndexCounter - 1, j].CharTile != null)
        //                {
        //                    sb.Insert(0, boardArray[upIndexCounter - 1, j].CharTile.Letter);
        //                    upIndexCounter--;
        //                }
        //                sb.Append("_");
        //                while (downIndexCounter < boardArray.GetLength(0) - 1 && boardArray[downIndexCounter + 1, j].CharTile != null)
        //                {
        //                    sb.Append(boardArray[downIndexCounter + 1, j].CharTile.Letter);
        //                    downIndexCounter++;
        //                }
        //                if (sb.Length == 1)
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    var wordWithUnderscore = sb.ToString();
        //                    var sbTemp = new StringBuilder(wordWithUnderscore);
        //                    if (!validCrossChecks.ContainsKey(boardArray[i, j]))
        //                    {
        //                        validCrossChecks.Add(boardArray[i, j], new List<CharTile>());
        //                    }
        //                    foreach (var c in dictionary.CharTiles)
        //                    {
        //                        if (c.Score == 0) continue;
        //                        sbTemp.Replace('_', c.Letter);
        //                        if (CheckWordValidity(dawg, sbTemp.ToString()))
        //                        {
        //                            if (validCrossChecks.ContainsKey(boardArray[i, j]))
        //                            {
        //                                if (!validCrossChecks[boardArray[i, j]].Contains(c))
        //                                {
        //                                    validCrossChecks[boardArray[i, j]].Add(c);
        //                                }
        //                            }
        //                        }
        //                        sbTemp.Clear();
        //                        sbTemp.Append(wordWithUnderscore);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return validCrossChecks;
        //}

        /// <summary>
        /// Goes through a board and gets all anchors and crosschecks for board tiles
        /// </summary>
        /// <param name="boardArray">Array to go over. May be untransposed or transposed</param>
        /// <param name="dictionary">Collection of CharTiles to look up to</param>
        /// <param name="validCrossChecks">List of valid cross checks to update</param>
        /// <param name="anchors">List of anchors to update</param>
        public static void GetValidCrossChecksAndAnchors(BoardTile[,] boardArray, WordDictionary dictionary, Dictionary<BoardTile, List<CharTile>> validCrossChecks, List<int[]> anchors)
        {
            var dawg = LoadDawg(dictionary.GameLanguage);
            StringBuilder sb = new StringBuilder();
            var noMovesMadeYet = false;
            var startTileRow = 0;
            var startTileColumn = 0;
            for (int i = 0; i < boardArray.GetLength(0); i++)
            {
                for (int j = 0; j < boardArray.GetLength(1); j++)
                {
                    ///If board tile has been played on the Start tile, the game hasn't started
                    if (boardArray[i, j].CharTile == null)
                    {
                        if (boardArray[i, j].BoardTileTypeID == BoardTileTypeEnum.Start)
                        {
                            startTileRow = i;
                            startTileColumn = j;
                            noMovesMadeYet = true;
                        }


                        ///Gets letters from above and below current tile to form a word with a gap from the currently iterated board tile
                        sb.Clear();
                        var upIndexCounter = i;
                        var downIndexCounter = i;

                        while (upIndexCounter > 0 && boardArray[upIndexCounter - 1, j].CharTile != null)
                        {
                            sb.Insert(0, boardArray[upIndexCounter - 1, j].CharTile.Letter);
                            upIndexCounter--;
                        }
                        sb.Append("_");
                        while (downIndexCounter < boardArray.GetLength(0) - 1 && boardArray[downIndexCounter + 1, j].CharTile != null)
                        {
                            sb.Append(boardArray[downIndexCounter + 1, j].CharTile.Letter);
                            downIndexCounter++;
                        }

                        //If no word is formed, continue to next board tile
                        if (sb.Length == 1)
                        {
                            continue;
                        }

                        //Else section replaces gap with all letters in alphabet to see which letters would fit to make a valid word
                        //Letters form the valid cross check set
                        //Cross check is stored as a dictionary object of board tile and a collection of char tiles
                        //This means for a given board tile, the following letters from the char tiles are valid
                        //If the collection is empty, no letter is a valid fit for the board tile
                        //If board tile (key) entry doesn't exist in the list, all letters are valid for that board tile (for now)
                        else
                        {
                            var wordWithUnderscore = sb.ToString();
                            var sbTemp = new StringBuilder(wordWithUnderscore);
                            if (!validCrossChecks.ContainsKey(boardArray[i, j]))
                            {
                                validCrossChecks.Add(boardArray[i, j], new List<CharTile>());
                            }
                            foreach (var c in dictionary.CharTiles)
                            {
                                if (c.Score == 0) continue;
                                sbTemp.Replace('_', c.Letter);
                                if (CheckWordValidity(dawg, sbTemp.ToString()))
                                {
                                    if (validCrossChecks.ContainsKey(boardArray[i, j]))
                                    {
                                        if (!validCrossChecks[boardArray[i, j]].Contains(c))
                                        {
                                            validCrossChecks[boardArray[i, j]].Add(c);
                                        }
                                    }
                                }
                                sbTemp.Clear();
                                sbTemp.Append(wordWithUnderscore);
                            }
                        }
                    //Checks surrounding tiles if they can be used as anchors
                    } else
                    {                        
                        if (i > 0 && boardArray[i - 1, j].CharTile == null)
                        {
                            if (anchors != null)
                            {
                                var coordinates = new int[] { i - 1, j };
                                if (!anchors.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) anchors.Add(coordinates);
                            }
                        }

                        if (i < boardArray.GetLength(0) - 1 && boardArray[i + 1, j].CharTile == null)
                        {
                            if (anchors != null)
                            {
                                var coordinates = new int[] { i + 1, j };
                                if (!anchors.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) anchors.Add(coordinates);
                            }
                        }

                        if (j > 0 && boardArray[i, j - 1].CharTile == null)
                        {
                            {
                                var coordinates = new int[] { i, j - 1 };
                                if (!anchors.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) anchors.Add(coordinates);
                            }
                        }

                        if (j < boardArray.GetLength(1) - 1 && boardArray[i, j + 1].CharTile == null)
                        {
                            var coordinates = new int[] { i, j + 1 };
                            if (!anchors.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) anchors.Add(coordinates);
                        }
                    }
                }
            }
            if (noMovesMadeYet)
            {
                anchors.Clear();
                anchors.Add(new int[] { startTileRow, startTileColumn });
            }
            return;
        }


        //public static Dictionary<BoardTile, List<CharTile>> GetValidCrossChecksCombined(Dictionary<BoardTile, List<CharTile>> validUntransposedCossChecks, Dictionary<BoardTile, List<CharTile>> validTransposedCrossChecks)
        //{
        //    Dictionary<BoardTile, List<CharTile>> validCrossChecks = new Dictionary<BoardTile, List<CharTile>>();
        //    for (int i = 0; i < validUntransposedCossChecks.Count; i++)
        //    {
        //        var keyAtIndex = validUntransposedCossChecks.Keys.ElementAt(i);
        //        var valueAtIndex = validUntransposedCossChecks.Values.ElementAt(i);
        //        validCrossChecks.Add(keyAtIndex, valueAtIndex);
        //    }
        //    for (int i = 0; i < validTransposedCrossChecks.Count; i++)
        //    {
        //        var keyAtIndex = validTransposedCrossChecks.Keys.ElementAt(i);
        //        var valueAtIndex = validTransposedCrossChecks.Values.ElementAt(i);
        //        if (!validCrossChecks.ContainsKey(keyAtIndex))
        //        {
        //            validCrossChecks.Add(keyAtIndex, valueAtIndex);
        //        }
        //        else
        //        {
        //            List<CharTile> validTilesForBothChecks = valueAtIndex.Intersect(validCrossChecks[keyAtIndex]).ToList();
        //            validCrossChecks[keyAtIndex] = validTilesForBothChecks;
        //        }
        //    }

        //    //for (int i = 0; i < validCrossChecks.Count; i++)
        //    //{
        //    //    var keyAtIndex = validCrossChecks.Keys.ElementAt(i);
        //    //    var valueAtIndex = validCrossChecks.Values.ElementAt(i);
        //    //    if (validUntransposedCossChecks.ContainsKey(keyAtIndex))
        //    //    {
        //    //        validUntransposedCossChecks[keyAtIndex] = valueAtIndex;
        //    //    }
        //    //    if (validTransposedCrossChecks.ContainsKey(keyAtIndex))
        //    //    {
        //    //        validTransposedCrossChecks[keyAtIndex] = valueAtIndex;
        //    //    }
        //    //}
        //    return validCrossChecks;

        //}

        /// <summary>
        /// Gets inforamtion from data object from POST request ajax object
        /// </summary>
        /// <param name="data">Ajax data to scan</param>
        /// <param name="property">Property of which to get information from</param>
        /// <returns></returns>
        public static string GetValueFromAjaxData(List<KeyValuePair<string, StringValues>> data, string property)
        {
            string value = "";
            foreach (var dataRow in data)
            {
                if (dataRow.Key.Contains(property))
                {
                    value += dataRow.Value;
                }
            }
            return value;
        }

        /// <summary>
        /// Gets the played rack tiles of the current move
        /// </summary>
        /// <param name="data">Ajax data to scan for played rack tiles</param>
        /// <returns></returns>
        public static string[] GetPlayedRackTiles(List<KeyValuePair<string, StringValues>> data)
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

        //public static void GetBoardArrayFromHtml(List<KeyValuePair<string, StringValues>> data)
        //{
        //    string boardArrayString = "";
        //    foreach (var dataRow in data)
        //    {
        //        if (dataRow.Key.Contains("boardArray"))
        //        {
        //            boardArrayString += dataRow.Value + ";";
        //        }
        //    }
        //    if (boardArrayString.Length >= 1)
        //    {
        //        boardArrayString = boardArrayString.Remove(boardArrayString.Length - 1, 1);
        //    }
        //    var boardArrayAsRows = boardArrayString.Split(";");
        //    var rows = boardArrayAsRows.Length;
        //    int columns = boardArrayAsRows[0].Count(f => f == ',');
        //    var boardArray = new int[rows, columns];
        //}

        /// <summary>
        /// Returns move generator
        /// </summary>
        /// <param name="game">Game from which to build up generator</param>
        /// <param name="gameMoves">List of moves to fill with valid moves</param>
        /// <param name="timeLimit">Time limit in seconds to limit move searching</param>
        /// <returns></returns>
        public static MoveGenerator GetMoveGenerator(Game game, List<Move> gameMoves, int timeLimit = 0)
        {
            var boardArray = game.Board.ConvertTo2DArray();
            var transposedBoardArray = game.Board.Transpose2DArray(boardArray);


            var validUntransposedCrossChecks = new Dictionary<BoardTile, List<CharTile>>();
            var validTransposedCrossChecks = new Dictionary<BoardTile, List<CharTile>>();
            var listOfValidAnchorCoordinatesOnUntransposedBoard = new List<int[]>();
            var listOfValidAnchorCoordinatesOnTransposedBoard = new List<int[]>();


            //Dictionary<BoardTile, List<CharTile>> validUntransposedCrossChecks = Helper.GetValidCrossChecksOneWay(boardArray, game.WordDictionary);
            //Dictionary<BoardTile, List<CharTile>> validTransposedCrossChecks = Helper.GetValidCrossChecksOneWay(transposedBoardArray, game.WordDictionary);
            //var listOfValidAnchorCoordinatesOnUntransposedBoard = game.Board.GetAnchors(boardArray);
            //var listOfValidAnchorCoordinatesOnTransposedBoard = game.Board.GetAnchors(transposedBoardArray);

            Helper.GetValidCrossChecksAndAnchors(boardArray, game.WordDictionary, validUntransposedCrossChecks, listOfValidAnchorCoordinatesOnUntransposedBoard);
            Helper.GetValidCrossChecksAndAnchors(transposedBoardArray, game.WordDictionary, validTransposedCrossChecks, listOfValidAnchorCoordinatesOnTransposedBoard);

            MoveGenerator moveValidator = new MoveGenerator(game, boardArray, transposedBoardArray, Helper.LoadDawg(game.GameLanguage), listOfValidAnchorCoordinatesOnUntransposedBoard,
                listOfValidAnchorCoordinatesOnTransposedBoard, validUntransposedCrossChecks, validTransposedCrossChecks, game.WordDictionary, gameMoves, timeLimit);
            return moveValidator;
        }

        /// <summary>
        /// Gets details of a board tile submitted form the web page as a string.
        /// </summary>
        /// <param name="tile">Tile to scan</param>
        /// <returns></returns>
        public static string[] GetTileDetails(string tile)
        {
            var tileDetails = tile.Split("_");
            var tileX = tileDetails[0];
            var tileY = tileDetails[1];
            var tileCharTileId = tileDetails[2];
            if (Int32.Parse(tileCharTileId) == 1)
            {
                var blankLetter = tileDetails[3];
                return new string[] { tileX, tileY, tileCharTileId, blankLetter };
            }

            return new string[] { tileX, tileY, tileCharTileId };
        }


        //public static bool IsRackPlayOneWayAndConnected(string[] rackTiles)
        //{
        //    HashSet<int> rowsUsed = new HashSet<int>();
        //    HashSet<int> columnsUsed = new HashSet<int>();
        //    foreach (var tile in rackTiles)
        //    {
        //        string[] tileDetails = GetTileDetails(tile);
        //        rowsUsed.Add(Int32.Parse(tileDetails[0]));
        //        columnsUsed.Add(Int32.Parse(tileDetails[1]));
        //    }
        //    if (rowsUsed.Count > 1 && columnsUsed.Count > 1)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //public static bool ValidateStart(BoardTile[,] boardAsArray, string[] playedRackTiles)
        //{
        //    int[] startBoardTileCoordinates = null;
        //    for (int i = 0; i < boardAsArray.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < boardAsArray.GetLength(1); j++)
        //        {
        //            if (boardAsArray[i, j].BoardTileType.Type == "Start")
        //            {
        //                startBoardTileCoordinates = new int[] { i, j };
        //            }
        //            if (boardAsArray[i, j].CharTile != null) return true;
        //        }
        //    }
        //    bool startTileIsFilled = false;
        //    bool tilesBeforeStartFilled = false;
        //    foreach (var tile in playedRackTiles)
        //    {
        //        var tileDetails = GetTileDetails(tile);
        //        var rowIndex = Int32.Parse(tileDetails[0]);
        //        var columnIndex = Int32.Parse(tileDetails[1]);
        //        if (rowIndex == startBoardTileCoordinates[0] && columnIndex == startBoardTileCoordinates[1])
        //        {
        //            startTileIsFilled = true;
        //        }
        //        if (rowIndex < startBoardTileCoordinates[0] || columnIndex < startBoardTileCoordinates[1])
        //        {
        //            tilesBeforeStartFilled = true;
        //        }
        //    }
        //    if (startTileIsFilled && !tilesBeforeStartFilled)
        //    {
        //        return true;
        //    }
        //    return false;

        //}
        //public static bool IsRackPlayConnected(BoardTile[,] boardAsArray, string[] rackTiles)
        //{
        //    HashSet<int> rowsUsed = new HashSet<int>();
        //    HashSet<int> columnsUsed = new HashSet<int>();
        //    foreach (var tile in rackTiles)
        //    {
        //        var tileDetails = GetTileDetails(tile);
        //        var rowIndex = Int32.Parse(tileDetails[0]);
        //        var columnIndex = Int32.Parse(tileDetails[1]);
        //        rowsUsed.Add(rowIndex);
        //        columnsUsed.Add(columnIndex);
        //    }
        //    if (rowsUsed.Count > 1 && columnsUsed.Count > 1)
        //    {
        //        return false;
        //    }
        //    if (rowsUsed.Count == 1 && columnsUsed.Count == 1)
        //    {
        //        var tileIsConnected = false;
        //        var rowIndex = rowsUsed.FirstOrDefault();
        //        var columnIndex = columnsUsed.FirstOrDefault();
        //        if ((boardAsArray[rowIndex + 1, columnIndex] != null && boardAsArray[rowIndex + 1, columnIndex].CharTile != null)
        //            || (boardAsArray[rowIndex - 1, columnIndex] != null && boardAsArray[rowIndex - 1, columnIndex].CharTile != null)
        //            || (boardAsArray[rowIndex, columnIndex + 1] != null && boardAsArray[rowIndex, columnIndex + 1].CharTile != null)
        //            || (boardAsArray[rowIndex, columnIndex - 1] != null && boardAsArray[rowIndex, columnIndex - 1].CharTile != null))
        //        {
        //            tileIsConnected = true;
        //        }
        //        if (!tileIsConnected)
        //        {
        //            return false;
        //        }
        //    }
        //    var directionOfPlay = "";
        //    var rowsUsedSorted = rowsUsed.ToList();
        //    rowsUsedSorted.Sort();
        //    var columnsUsedSorted = columnsUsed.ToList();
        //    columnsUsedSorted.Sort();
        //    var multipleIndexes = rowsUsedSorted.Count > columnsUsedSorted.Count ? rowsUsedSorted : columnsUsedSorted;
        //    var secondaryIndex = rowsUsedSorted.Count > columnsUsedSorted.Count ? columnsUsedSorted : rowsUsedSorted;
        //    directionOfPlay = rowsUsedSorted.Count > columnsUsedSorted.Count ? "transposed" : "untransposed";
        //    for (var i = multipleIndexes[0]; i < multipleIndexes[multipleIndexes.Count - 1]; i++)
        //    {
        //        var checkedTile = directionOfPlay == "untransposed" ? boardAsArray[secondaryIndex[0], i] : boardAsArray[i, secondaryIndex[0]];
        //        if (checkedTile.CharTile == null)
        //        {
        //            var boardTileHasTemporaryPlacedRackTile = false;
        //            foreach (var rackTile in rackTiles)
        //            {
        //                var tileDetails = GetTileDetails(rackTile);
        //                var rowIndex = Int32.Parse(tileDetails[0]);
        //                var columnIndex = Int32.Parse(tileDetails[1]);
        //                if (rowIndex == checkedTile.BoardLocationX && columnIndex == checkedTile.BoardLocationY)
        //                {
        //                    boardTileHasTemporaryPlacedRackTile = true;
        //                    break;
        //                }
        //            }
        //            if (!boardTileHasTemporaryPlacedRackTile)
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}


        /// <summary>
        /// Given a tile, checks the vertical word that it forms
        /// </summary>
        /// <param name="board">Board to scan</param>
        /// <param name="rackTileCoordinates">Coordinates of rack tile played</param>
        /// <returns></returns>
        public static List<BoardTile> GetVerticalPlays(BoardTile[,] board, int[] rackTileCoordinates)
        {
            var word = new List<BoardTile>();
            var upIndexCounter = rackTileCoordinates[0];
            var downIndexCounter = rackTileCoordinates[0];
            var columnIndex = rackTileCoordinates[1];

            while (upIndexCounter > 0 && board[upIndexCounter - 1, columnIndex].CharTile != null)
            {
                word.Insert(0, board[upIndexCounter - 1, columnIndex]);
                upIndexCounter--;
            }
            word.Add(board[rackTileCoordinates[0], rackTileCoordinates[1]]);
            while (downIndexCounter < board.GetLength(0) - 1 && board[downIndexCounter + 1, columnIndex].CharTile != null)
            {
                word.Add(board[downIndexCounter + 1, columnIndex]);
                downIndexCounter++;
            }
            if (word.Count < 2) return null;
            return word;           
        }

        /// <summary>
        /// Plays moves on board, updates board and scores
        /// </summary>
        /// <param name="game">Game object to update</param>
        /// <param name="data">Data sent from user</param>
        /// <returns></returns>
        public static HttpStatusCodeResult GetWordScores(Game game, List<KeyValuePair<string, StringValues>> data = null)
        {
            var dawg = LoadDawg(game.GameLanguage);
            var playerAtHand = game.GetPlayerAtHand();
            var playedWords = GetPlayedWords(data);
            var playedRackTiles = GetPlayedRackTiles(data);
            var allUsedBoardTiles = new List<BoardTile>();
            string logBuilder = "Player" + playerAtHand.ID + " played ";

            //Marks all moves as old and increments move number counter
            playerAtHand.Moves = playerAtHand.Moves.Select(c => { c.IsNew = false; return c; }).ToList();
            int moveNumber = playerAtHand.Moves.Count == 0 ? 1 : (playerAtHand.Moves.OrderByDescending(m => m.MoveNumber).FirstOrDefault().MoveNumber) + 1;
            for (int i = 0; i < playedWords.Length; i++)
            {
                var playedWord = playedWords[i];
                var currentScoreOfMove = 0;
                var playedWordString = "";
                var usedBoardTiles = new List<BoardTile>();
                var playedTiles = playedWord.Split(",");
                foreach (string playedTile in playedTiles)
                {
                    var tileDetails = GetTileDetails(playedTile);
                    var tileX = Int32.Parse(tileDetails[0]);
                    var tileY = Int32.Parse(tileDetails[1]);
                    allUsedBoardTiles.Add(game.Board.ConvertTo2DArray()[tileX,tileY]);
                    var tileCharTileId = Int32.Parse(tileDetails[2]);
                    if (tileCharTileId == 1)
                    {
                        var blankLetter = tileDetails[3][0];
                        playedWordString += blankLetter;
                        game.Board.PlayTile(tileX, tileY, tileCharTileId, usedBoardTiles, blankLetter.ToString());
                    }
                    else
                    {
                        playedWordString += game.WordDictionary.CharTiles.Where(c => c.ID == tileCharTileId).FirstOrDefault().Letter;
                        game.Board.PlayTile(tileX, tileY, tileCharTileId, usedBoardTiles);
                    }
                }

                if (!CheckWordValidity(dawg, playedWordString, false))
                {
                    return new HttpStatusCodeResult(400, playedWordString + " is not a legal word.");
                }
                currentScoreOfMove = GetWordScore(usedBoardTiles);
                var isHorizontal = usedBoardTiles[0].BoardLocationX == usedBoardTiles[1].BoardLocationX ? true : false;
                var start = isHorizontal ? usedBoardTiles[0].BoardLocationY : usedBoardTiles[0].BoardLocationX;
                var end = isHorizontal ? usedBoardTiles[usedBoardTiles.Count-1].BoardLocationY : usedBoardTiles[usedBoardTiles.Count - 1].BoardLocationX;
                var index = isHorizontal ? usedBoardTiles[0].BoardLocationX : usedBoardTiles[0].BoardLocationY;
                playedWordString = playedWordString.ToUpper();
                game.AddScoreToPlayer(playerAtHand, currentScoreOfMove);                
                playerAtHand.Moves.Add(new Move { PlayerID = playerAtHand.ID, GameID = game.ID, Word = playedWordString, Score = currentScoreOfMove,
                    IsHorizontal = isHorizontal, Index = index, Start = start, End = end, IsNew = true, MoveNumber = moveNumber });
                logBuilder += playedWordString + " for " + currentScoreOfMove + " points";
                if (i == playedWords.Length - 1)
                {
                    logBuilder += ". ";
                }
                else if (i == playedWords.Length - 2)
                {
                    logBuilder += " and ";
                }
                else
                {
                    logBuilder += " , ";
                }
            }

            if (playedRackTiles.Length == playerAtHand.Rack.RackSize)
            {
                game.AddScoreToPlayer(playerAtHand, 50);
            }

            foreach (var boardTile in allUsedBoardTiles)
            {
                boardTile.IsFilled = true;
            }

            foreach (var playedRackTile in playedRackTiles)
            {
                var tileDetails = GetTileDetails(playedRackTile);
                var tileCharTileId = Int32.Parse(tileDetails[2]);
                playerAtHand.Rack.SubstractFromRack(game.WordDictionary.CharTiles.Where(c => c.ID == tileCharTileId).FirstOrDefault());
                playerAtHand.Rack.RefillRackFromPouch();
            }
            if (playerAtHand.Rack.Rack_CharTiles.Count == 0)
            {
                game.FinalizeResults(playerAtHand);
            }

            game.Log += logBuilder;
            game.Log += "Player" + playerAtHand.ID + " now has " + playerAtHand.Score + " points.";
            game.Log += "-------------------------";
            return new HttpStatusCodeResult(200, "Good move :)");
        }
        //public static string ReverseString(string s)
        //{
        //    char[] charArray = s.ToCharArray();
        //    Array.Reverse(charArray);
        //    return new string(charArray);
        //}

        /// <summary>
        /// Checks if timer has reached an amount of seconds
        /// </summary>
        /// <param name="stopwatch"Stopwatch object></param>
        /// <param name="timeLimit">Time limit in seconds</param>
        /// <param name="anchorSearchIsFinished"></param>
        /// <returns></returns>
        public static bool CheckIfTimeLimitIsReached(Stopwatch stopwatch, int timeLimit, bool anchorSearchIsFinished = false)
        {
            if (timeLimit == 0) return false;
            if (Convert.ToInt32(stopwatch.Elapsed.TotalSeconds) > timeLimit)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reorders elements in list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        //public static void InsertInArray(object[] array, int index, object val)
        //{
        //    for (int i = index; i < array.Length; i++)
        //    {
        //        array[i] = array[i - 1];
        //    }
        //    array[index] = val;
        //}
    }
}
