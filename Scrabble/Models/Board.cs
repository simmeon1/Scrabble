using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrabble.Models
{
    public class Board
    {
        public int ID { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }      

        public int GameID { get; set; }
        [ForeignKey("GameID")]
        public Game Game { get; set; }

        /*public int WordDictionaryID { get; set; }
        [ForeignKey("WordDictionaryID")]
        public WordDictionary WordDictionary { get; set; }*/

        public List<BoardTile> BoardTiles { get; set; }

        public Board()
        {
            /*Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            Rows = 0;
            Columns = 0;
            BoardTiles = new List<BoardTile>();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    BoardTiles.Add(new BoardTile(i, j));
                }
            }*/
        }

        public Board (int rows, int columns, WordDictionary wordDictionary)
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            Rows = rows;
            Columns = columns;
            WordDictionary = wordDictionary;
            BoardTiles = new List<BoardTile>();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    BoardTiles.Add(new BoardTile(i, j));
                }               
            }
        }

        public BoardTile[,] ConvertTo2DArray ()
        {
            BoardTile[,] array = new BoardTile[Rows,Columns];
            foreach (var boardTile in BoardTiles)
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