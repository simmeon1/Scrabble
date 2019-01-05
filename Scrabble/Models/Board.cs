using System.Collections.Generic;

namespace Scrabble.Models
{
    public class Board
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public List<BoardTile> BoardTiles { get; set; }

        public Board (int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            BoardTiles = new List<BoardTile>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
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