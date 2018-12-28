namespace Scrabble.Models
{
    public class Board
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public BoardTile[][] BoardTiles { get; set; }

        public Board (int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            BoardTiles = new BoardTile[Rows][];
            for (int i = 0; i < Rows; i++)
            {
                BoardTiles[i] = new BoardTile[Columns];
            }
        }

        public string IntToCSSWidth (int measurement)
        {
            return ((1.0 / measurement) * 100) + "".Replace(',','.');
        }
    }
}