namespace Scrabble.Models
{
    public class BoardTile
    {
        public string ID { get; set; }
        public int BoardLocationX { get; set; }
        public int BoardLocationY { get; set; }
        public BoardTileType BoardTileType { get; set; }

        public int BoardID { get; set; }
        public Board Board { get; set; }

        public int LetterTileID { get; set; }
        public CharTile LetterTile { get; set; }

        public BoardTile ()
        {
            /*BoardLocationX = -1;
            BoardLocationY = -1;
            ID = "" + BoardLocationX + "&" + BoardLocationY;
            //WordDictionary = new WordDictionary(Language.English);
            LetterTile = null;
            BoardTileType = BoardTileTypes.Type.None;*/
        }

        public BoardTile(int boardLocationX, int boardLocationY, CharTile letterTile = null)
        {
            BoardLocationX = boardLocationX;
            BoardLocationY = boardLocationY;
            ID = "" + BoardLocationX + "&" + BoardLocationY;
            //WordDictionary = new WordDictionary(Language.English);
            LetterTile = letterTile;
            BoardTileType = BoardTileType.None;
        }
    }
}