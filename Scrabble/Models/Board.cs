using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        public void PlayTile (int x, int y, int charTileId, List<BoardTile> usedBoardTiles)
        {
            List<BoardTile> boardTilesList = BoardTiles.ToList();
            foreach (BoardTile b in boardTilesList)
            {
                if (b.BoardLocationX == x && b.BoardLocationY == y)
                {
                    b.CharTileID = charTileId;
                    usedBoardTiles.Add(b);
                }
            }
            BoardTiles = boardTilesList;
        }

        public BoardTile[,] ConvertTo2DArray ()
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
            List<BoardTile[]> rotatedArrayList = new List<BoardTile[]>();
            for (var i = 0; i < boardArray.GetLength(1); i++)
            {
                var boardColumnAsARow = new List<BoardTile>();
                for (var j = 0; j < boardArray.GetLength(0); j++)
                {
                    boardColumnAsARow.Add(boardArray[j, i]);
                }
                rotatedArrayList.Add(boardColumnAsARow.ToArray());
            }
            rotatedArrayList.Reverse();
            var rotatedArray = rotatedArrayList.ToArray();
            BoardTile[,] resultArray = new BoardTile[boardArray.GetLength(1), boardArray.GetLength(0)];
            for (int i = 0; i < resultArray.GetLength(0); i++)
            {
                for (int j = 0; j < resultArray.GetLength(1); j++)
                {
                    resultArray[i, j] = rotatedArray[i][j];
                }
            }
            return resultArray;
        }

        public bool[,] GetAnchors(BoardTile[,] boardArray, List<int[]> listOfValidAnchorCoordinates = null)
        {
            bool[,] arrayWithAnchors = new bool[Rows, Columns];
            for (int i = 0; i < arrayWithAnchors.GetLength(0); i++)
            {
                for (int j = 0; j < arrayWithAnchors.GetLength(1); j++)
                {
                    arrayWithAnchors[i, j] = false;
                }
            }
            for (int i = 0; i < boardArray.GetLength(0); i++)
            {
                for (int j = 0; j < boardArray.GetLength(1); j++)
                {
                    if (boardArray[i,j].CharTile != null)
                    {
                        if (i > 0 && boardArray[i-1, j].CharTile == null)
                        {                            
                            arrayWithAnchors[i - 1, j] = true;
                            if (listOfValidAnchorCoordinates != null) {
                                var coordinates = new int[] { i - 1, j };
                                if (!listOfValidAnchorCoordinates.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) listOfValidAnchorCoordinates.Add(coordinates);
                            }
                        }
                        if (i < boardArray.GetLength(0) - 1 && boardArray[i + 1, j].CharTile == null)
                        {
                            arrayWithAnchors[i + 1, j] = true;
                            if (listOfValidAnchorCoordinates != null)
                            {
                                var coordinates = new int[] { i + 1, j };
                                if (!listOfValidAnchorCoordinates.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) listOfValidAnchorCoordinates.Add(coordinates);
                            }
                        }
                        if (j > 0 && boardArray[i, j - 1].CharTile == null)
                        {
                            arrayWithAnchors[i, j - 1] = true;
                            if (listOfValidAnchorCoordinates != null)
                            {
                                var coordinates = new int[] { i, j - 1 };
                                if (!listOfValidAnchorCoordinates.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) listOfValidAnchorCoordinates.Add(coordinates);
                            }
                        }
                        if (j < boardArray.GetLength(1) - 1 && boardArray[i, j + 1].CharTile == null)
                        {
                            arrayWithAnchors[i, j + 1] = true;
                            if (listOfValidAnchorCoordinates != null)
                            {
                                var coordinates = new int[] { i, j + 1 };
                                if (!listOfValidAnchorCoordinates.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) listOfValidAnchorCoordinates.Add(coordinates);
                            }
                        }
                    }

                }
            }
            return arrayWithAnchors;
        }

        public bool CheckIfAnchorIsUsed (string[] playedRackTiles, BoardTile[,] boardArray)
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
                var anchors = GetAnchors(boardArray);
                if (anchors[tileX, tileY] == true)
                {
                    return true;
                }
            }
            return false;
        }

        public string IntToCSSWidth (int measurement)
        {
            string temp = "";
            temp = ((1.0 / measurement) * 100) + "";
            temp = temp.Replace(',', '.');
            return temp;
        }
    }
}