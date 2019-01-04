namespace Scrabble.Models
{
    public class BoardTile
    {
        public BoardTileTypes.Type BoardTileType { get; set; }
        public CharTile LetterTile { get; set; }
        public WordDictionary WordDictionary { get; set; }

        public BoardTile ()
        {
            LetterTile = null;
            BoardTileType = BoardTileTypes.Type.None;
        }

        public BoardTile(char c, int score)
        {
            WordDictionary = new WordDictionary(GameLanguages.Language.English);
            LetterTile = new CharTile(c, score);
            BoardTileType = BoardTileTypes.Type.None;
        }
    }
}