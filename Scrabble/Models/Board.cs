using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Scrabble.Models
{
    /// <summary>
    /// Represents a board
    /// </summary>
    public class Board
    {
        public int ID { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public int GameID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        /// <summary>
        /// A 15x15 board has 225 spaces (board tiles).
        /// Those spaces may be empty or taken by a letter (CharTile).
        /// Often turned into a 2D array to handle horizontal/vertical operations.
        /// </summary>
        public virtual ICollection<BoardTile> BoardTiles { get; set; }

        /// <summary>
        /// Puts a CharTile on the board on the given coordinates.
        /// </summary>
        /// <param name="x">Row to put on</param>
        /// <param name="y">Column to put on</param>
        /// <param name="charTileId">ID of CharTile to put</param>
        /// <param name="usedBoardTiles">Updates a list containing the filled board tiles</param>
        /// <param name="blankLetter">If a CharTile is a polymorphed blank, the relevant CharTile of score 0 is retrieved.</param>
        public void PlayTile(int x, int y, int charTileId, List<BoardTile> usedBoardTiles, string blankLetter = null)
        {
            List<BoardTile> boardTilesList = BoardTiles.ToList();
            foreach (BoardTile b in boardTilesList)
            {
                if (b.BoardLocationX == x && b.BoardLocationY == y)
                {
                    if (blankLetter != null)
                    {
                        var charTileIdBlank = Game.WordDictionary.CharTiles.Where(c => c.Letter == blankLetter[0]).FirstOrDefault().ID;
                        
                        //A CharTile for a game can be a *, A-Z with scores or A-Z with score 0.
                        //Letter A with score 0 will follow the last character in the alphabet (Z).
                        //By knowing the count of letters in an alphabet, we can get the ID of the CharTile with 
                        //a given letter but with a score of 0 instead.
                        b.CharTileID = charTileIdBlank + Game.GameLanguage.CountOfLetters;

                    }
                    else b.CharTileID = charTileId;
                    usedBoardTiles.Add(b);
                }
            }
            BoardTiles = boardTilesList;
        }

        /// <summary>
        /// Converts BoardTile list to multidimensional array
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Transposes an array (turns rows to columns and columns to rows).
        /// </summary>
        /// <param name="boardArray">Original untransposed array.</param>
        /// <returns></returns>
        public BoardTile[,] Transpose2DArray(BoardTile[,] boardArray)
        {
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

        //public List<int[]> GetAnchors(BoardTile[,] boardArray)
        //{
        //    List<int[]> listOfValidAnchorCoordinates = new List<int[]>();
        //    for (int i = 0; i < boardArray.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < boardArray.GetLength(1); j++)
        //        {
        //            if (boardArray[i, j].BoardTileType.Type == "Start" && boardArray[i, j].CharTile == null)
        //            {
        //                listOfValidAnchorCoordinates.Clear();
        //                listOfValidAnchorCoordinates.Add(new int[] { i, j });
        //                return listOfValidAnchorCoordinates;
        //            }
        //            if (boardArray[i, j].CharTile != null)
        //            {
        //                if (i > 0 && boardArray[i - 1, j].CharTile == null)
        //                {
        //                    if (listOfValidAnchorCoordinates != null)
        //                    {
        //                        var coordinates = new int[] { i - 1, j };
        //                        if (!listOfValidAnchorCoordinates.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) listOfValidAnchorCoordinates.Add(coordinates);
        //                    }
        //                }

                        
        //                if (i < boardArray.GetLength(0) - 1 && boardArray[i + 1, j].CharTile == null)
        //                {
        //                    if (listOfValidAnchorCoordinates != null)
        //                    {
        //                        var coordinates = new int[] { i + 1, j };
        //                        if (!listOfValidAnchorCoordinates.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) listOfValidAnchorCoordinates.Add(coordinates);
        //                    }
        //                }
                        
        //                if (j > 0 && boardArray[i, j - 1].CharTile == null)
        //                {
        //                    if (listOfValidAnchorCoordinates != null)
        //                    {
        //                        var coordinates = new int[] { i, j - 1 };
        //                        if (!listOfValidAnchorCoordinates.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) listOfValidAnchorCoordinates.Add(coordinates);
        //                    }
        //                }
                        
        //                if (j < boardArray.GetLength(1) - 1 && boardArray[i, j + 1].CharTile == null)
        //                {
        //                    if (listOfValidAnchorCoordinates != null)
        //                    {
        //                        var coordinates = new int[] { i, j + 1 };
        //                        if (!listOfValidAnchorCoordinates.Any(c => c[0] == coordinates[0] && c[1] == coordinates[1])) listOfValidAnchorCoordinates.Add(coordinates);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    return listOfValidAnchorCoordinates;
        //}

        //public bool CheckIfAnchorIsUsed(string[] playedRackTiles, BoardTile[,] boardArray)
        //{
        //    for (int i = 0; i < boardArray.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < boardArray.GetLength(1); j++)
        //        {
        //            if (boardArray[i, j].BoardTileType.Type == "Start")
        //            {
        //                if (boardArray[i, j].CharTile == null) return true;
        //            }
        //        }
        //    }
        //    foreach (var tile in playedRackTiles)
        //    {
        //        var tileDetails = tile.Split("_");
        //        int tileX = Int32.Parse(tileDetails[0]);
        //        int tileY = Int32.Parse(tileDetails[1]);
        //        int tileCharTileId = Int32.Parse(tileDetails[2]);
        //    }
        //    return false;
        //}

        /// <summary>
        /// Splits a measurement in equal parts to form 100%
        /// Splits a row of 15 tiles to around 6.67% each.
        /// </summary>
        /// <param name="measurement">Measurement of board</param>
        /// <returns></returns>
        public string IntToCSSWidth(int measurement)
        {
            string temp = "";
            temp = ((1.0 / measurement) * 100) + "";
            temp = temp.Replace(',', '.');
            return temp;
        }

        //public void ResetBoardTileTypes()
        //{
        //    foreach (var boardTile in BoardTiles)
        //    {
        //        if (boardTile.CharTile != null)
        //        {
        //            boardTile.BoardTileType.ID = 1;
        //            boardTile.BoardTileType.Type = "Normal";
        //        }
        //    }
        //}
    }
}