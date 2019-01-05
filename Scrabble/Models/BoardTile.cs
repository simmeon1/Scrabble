namespace Scrabble.Models
{
    public class BoardTile
    {
        public string ID { get; set; }
        public int BoardLocationX { get; set; }
        public int BoardLocationY { get; set; }
        public BoardTileTypes.Type BoardTileType { get; set; }
        public CharTile LetterTile { get; set; }
        public WordDictionary WordDictionary { get; set; }

        /*public BoardTile ()
        {

            LetterTile = null;
            BoardTileType = BoardTileTypes.Type.None;
        }*/

        public BoardTile(int x, int y, CharTile c = null)
        {
            BoardLocationX = x;
            BoardLocationY = y;
            ID = "" + x + "&" + y;
            WordDictionary = new WordDictionary(GameLanguages.Language.English);
            LetterTile = c;
            BoardTileType = BoardTileTypes.Type.None;
        }
    }
}