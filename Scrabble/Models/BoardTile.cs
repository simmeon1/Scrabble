namespace Scrabble.Models
{
    public class BoardTile
    {
        public BoardTileTypes.Type BoardTileType { get; set; }
        public CharTile LetterTile { get; set; }

        public BoardTile ()
        {
            LetterTile = null;
            BoardTileType = BoardTileTypes.Type.None;
        }
    }
}