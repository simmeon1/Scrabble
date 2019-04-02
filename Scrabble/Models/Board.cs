using Scrabble.Classes;
using Scrabble.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Scrabble.Models
{
    public class Board
    {
        public int ID { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public int GameID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        public virtual ICollection<BoardTile> BoardTiles { get; set; }

        public void PlayTile(int x, int y, int charTileId, List<BoardTile> usedBoardTiles, List<BoardTile> newlyUsedBoardTiles, string blankLetter = null, BoardTile[,] boardArray = null, bool playedByHuman = true)
        {
            if (boardArray == null) boardArray = ConvertTo2DArray();
            List<BoardTile> boardTilesList = BoardTiles.ToList();
            for (int i = 0; i < boardArray.GetLength(0); i++)
            {
                for (int j = 0; j < boardArray.GetLength(1); j++)
                {
                    var b = boardArray[i, j];
                    if (b.BoardLocationX == x && b.BoardLocationY == y)
                    {
                        if (b.CharTile == null)
                        {
                            CharTile charTileToPlace = null;
                            if (blankLetter != null)
                            {
                                charTileToPlace = Game.WordDictionary.CharTiles.Where(c => c.Letter == blankLetter[0] && c.Score == 0).FirstOrDefault();
                            }
                            else charTileToPlace = Game.WordDictionary.CharTiles.Where(c => c.ID == charTileId).FirstOrDefault();
                            b.CharTile = charTileToPlace;
                            if (playedByHuman) newlyUsedBoardTiles.Add(b);
                        }
                        else
                        {
                            b.UntransposedCrossCheck = "";
                            b.TransposedCrossCheck = "";
                        }
                        if (playedByHuman) usedBoardTiles.Add(b);
                    }
                    UpdateAnchors(boardArray, i, j);
                }
            }
            BoardTiles = boardTilesList;
        }

        public void PlayTiles(Dictionary<BoardTile, CharTile> boardAndCharTiles)
        {
            foreach (var entry in boardAndCharTiles)
            {
                entry.Key.CharTile = entry.Value;
                entry.Key.UntransposedCrossCheck = "";
                entry.Key.TransposedCrossCheck = "";
            }
            var board = ConvertTo2DArray();
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x,y].CharTile != null)
                    {
                        UpdateAnchors(board, x, y);
                    }
                }
            }           
        }


        public HashSet<BoardTileCrossCheck> GetCrossCheckBoardTiles(List<BoardTile> boardTiles, bool isTransposed)
        {
            var boardArray =  !isTransposed ? ConvertTo2DArray() : Transpose2DArray(ConvertTo2DArray());
            HashSet<BoardTileCrossCheck> crossChecksOfBoardTilesToUpdate = new HashSet<BoardTileCrossCheck>();
            foreach (var boardTile in boardTiles)
            {
                BoardTile boardTileOnTop = null;
                BoardTile boardTileOnBottom = null;
                var upIndexCounter = 0;
                var downIndexCounter = 0;
                var rowIndexOriginal = 0;
                var columnIndex = 0;
                for (int x = 0; x < boardArray.GetLength(0); x++)
                {
                    for (int y = 0; y < boardArray.GetLength(1); y++)
                    {
                        if (boardArray[x, y] == boardTile)
                        {
                            upIndexCounter = x;
                            downIndexCounter = x;
                            rowIndexOriginal = x;
                            columnIndex = y;
                        }
                    }
                }
                var word = new StringBuilder();
                while (upIndexCounter > 0 && boardArray[upIndexCounter - 1, columnIndex].CharTile != null)
                {
                    word.Insert(0, boardArray[upIndexCounter - 1, columnIndex].CharTile.Letter);
                    boardTileOnTop = boardArray[upIndexCounter - 1, columnIndex];
                    upIndexCounter--;
                }
                if (upIndexCounter > 0 && boardArray[upIndexCounter - 1, columnIndex].CharTile == null) boardTileOnTop = boardArray[upIndexCounter - 1, columnIndex];
                else boardTileOnTop = null;
                word.Append(boardArray[rowIndexOriginal, columnIndex].CharTile.Letter);
                while (downIndexCounter < boardArray.GetLength(0) - 1 && boardArray[downIndexCounter + 1, columnIndex].CharTile != null)
                {
                    word.Append(boardArray[downIndexCounter + 1, columnIndex].CharTile.Letter);
                    boardTileOnBottom = boardArray[downIndexCounter + 1, columnIndex];
                    downIndexCounter++;
                }
                if (downIndexCounter < boardArray.GetLength(0) - 1 && boardArray[downIndexCounter + 1, columnIndex].CharTile == null) boardTileOnBottom = boardArray[downIndexCounter + 1, columnIndex];
                else boardTileOnBottom = null;
                var wordString = word.ToString();
                if (boardTileOnTop != null)
                {
                    var wordTemp = String.Copy(wordString);
                    wordTemp = wordTemp.Insert(0, "_");
                    crossChecksOfBoardTilesToUpdate.Add(new BoardTileCrossCheck { Tile = boardTileOnTop, WordAttachedTo = wordTemp });
                }
                if (boardTileOnBottom != null)
                {
                    var wordTemp = String.Copy(wordString);
                    wordTemp += "_";
                    crossChecksOfBoardTilesToUpdate.Add(new BoardTileCrossCheck { Tile = boardTileOnBottom, WordAttachedTo = wordTemp });
                }
            }          
            return crossChecksOfBoardTilesToUpdate;
        }

        public BoardTile[,] ConvertTo2DArray()
        {
            List<BoardTile> boardTilesList = BoardTiles.ToList();
            int indexOfLastBoardTile = boardTilesList.Count - 1;
            BoardTile[,] array = new BoardTile[boardTilesList[indexOfLastBoardTile].BoardLocationX + 1, boardTilesList[indexOfLastBoardTile].BoardLocationY + 1];
            foreach (var boardTile in boardTilesList)
            {
                array[boardTile.BoardLocationX, boardTile.BoardLocationY] = boardTile;
            }
            return array;
        }

        public BoardTile[,] Transpose2DArray(BoardTile[,] boardArray)
        {
            //List<BoardTile[]> rotatedArrayList = new List<BoardTile[]>();
            //for (var i = 0; i < boardArray.GetLength(1); i++)
            //{
            //    var boardColumnAsARow = new List<BoardTile>();
            //    for (var j = 0; j < boardArray.GetLength(0); j++)
            //    {
            //        boardColumnAsARow.Add(boardArray[j, i]);
            //    }
            //    rotatedArrayList.Add(boardColumnAsARow.ToArray());
            //}
            //rotatedArrayList.Reverse();
            //var rotatedArray = rotatedArrayList.ToArray();
            //BoardTile[,] resultArray = new BoardTile[boardArray.GetLength(1), boardArray.GetLength(0)];
            //for (int i = 0; i < resultArray.GetLength(0); i++)
            //{
            //    for (int j = 0; j < resultArray.GetLength(1); j++)
            //    {
            //        resultArray[i, j] = rotatedArray[i][j];
            //    }
            //}
            //return resultArray;
            int w = boardArray.GetLength(0);
            int h = boardArray.GetLength(1);

            BoardTile[,] result = new BoardTile[h, w];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j, i] = boardArray[i, j];
                }
            }

            return result;
        }

        public HashSet<BoardTile> GetAnchors(BoardTile[,] boardArray)
        {
            HashSet<BoardTile> listOfValidAnchors = new HashSet<BoardTile>();
            for (int i = 0; i < boardArray.GetLength(0); i++)
            {
                for (int j = 0; j < boardArray.GetLength(1); j++)
                {
                    if (boardArray[i, j].BoardTileType.Type == "Start" && boardArray[i, j].CharTile == null)
                    {
                        listOfValidAnchors.Clear();
                        listOfValidAnchors.Add(boardArray[i, j]);
                        return listOfValidAnchors;
                    }
                    if (boardArray[i, j].CharTile != null)
                    {
                        if (i > 0 && boardArray[i - 1, j].CharTile == null)
                        {
                            listOfValidAnchors.Add(boardArray[i - 1, j]);
                        }
                        if (i < boardArray.GetLength(0) - 1 && boardArray[i + 1, j].CharTile == null)
                        {
                            listOfValidAnchors.Add(boardArray[i + 1, j]);
                        }

                        if (j > 0 && boardArray[i, j - 1].CharTile == null)
                        {
                            listOfValidAnchors.Add(boardArray[i, j - 1]);
                        }

                        if (j < boardArray.GetLength(1) - 1 && boardArray[i, j + 1].CharTile == null)
                        {
                            listOfValidAnchors.Add(boardArray[i, j + 1]);
                        }
                    }

                }
            }
            return listOfValidAnchors;
        }

        public void UpdateAnchors(BoardTile[,] boardArray, int i, int j)
        {
            if (boardArray[i, j].BoardTileType.Type == "Start" && boardArray[i, j].CharTile == null)
            {
                return;
            }
            if (boardArray[i, j].CharTile != null)
            {
                boardArray[i, j].IsAnchor = false;
                if (i > 0 && boardArray[i - 1, j].CharTile == null)
                {
                    boardArray[i - 1, j].IsAnchor = true;
                }
                if (i < boardArray.GetLength(0) - 1 && boardArray[i + 1, j].CharTile == null)
                {
                    boardArray[i + 1, j].IsAnchor = true;
                }

                if (j > 0 && boardArray[i, j - 1].CharTile == null)
                {
                    boardArray[i, j - 1].IsAnchor = true;
                }

                if (j < boardArray.GetLength(1) - 1 && boardArray[i, j + 1].CharTile == null)
                {
                    boardArray[i, j + 1].IsAnchor = true;
                }
            }
        }

        public bool CheckIfAnchorIsUsed(string[] playedRackTiles, BoardTile[,] boardArray)
        {
            for (int i = 0; i < boardArray.GetLength(0); i++)
            {
                for (int j = 0; j < boardArray.GetLength(1); j++)
                {
                    if (boardArray[i, j].BoardTileType.Type == "Start")
                    {
                        if (boardArray[i, j].CharTile == null) return true;
                    }
                }
            }
            foreach (var tile in playedRackTiles)
            {
                var tileDetails = tile.Split("_");
                int tileX = Int32.Parse(tileDetails[0]);
                int tileY = Int32.Parse(tileDetails[1]);
                int tileCharTileId = Int32.Parse(tileDetails[2]);
            }
            return false;
        }

        public string IntToCSSWidth(int measurement)
        {
            string temp = "";
            temp = ((1.0 / measurement) * 100) + "";
            temp = temp.Replace(',', '.');
            return temp;
        }
    }
}