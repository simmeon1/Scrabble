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

        /*public int WordDictionaryID { get; set; }
        [ForeignKey("WordDictionaryID")]
        public WordDictionary WordDictionary { get; set; }*/

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
                    //switch (b.BoardTileType)
                    //{
                    //    case "DoubleLetter":
                    //        Console.WriteLine("Case 1");
                    //        break;
                    //    case "TripleLetter":
                    //        Console.WriteLine("Case 1");
                    //        break;
                    //    default:
                    //        currentScoreOfMove += char
                    //        break;
                    //}
                }
            }
            BoardTiles = boardTilesList;
        }

        /*public Board()
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            Rows = 15;
            Columns = 15;
            BoardTiles = new List<BoardTile>();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    BoardTiles.Add(new BoardTile(i, j));
                }
            }
        }*/

        /*public Board (int rows, int columns, WordDictionary wordDictionary = null)
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            Rows = rows;
            Columns = columns;
            //WordDictionary = wordDictionary;
            BoardTiles = new List<BoardTile>();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    BoardTiles.Add(new BoardTile(i, j));
                }               
            }
        }*/

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

        public string IntToCSSWidth (int measurement)
        {
            string temp = "";
            temp = ((1.0 / measurement) * 100) + "";
            temp = temp.Replace(',', '.');
            return temp;
        }

        /*public void SetBoardTileType(BoardTileTypes.Type type, int[] tileIds)
        {
            for (int i = 0; i < tileIds.Length; i++)
            {
                int tileId = tileIds[i];
                BoardTiles[tileId % 10][tileId / 10].BoardTileType = type;
            }
        }*/
    }
}