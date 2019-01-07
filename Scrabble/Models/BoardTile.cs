using System.ComponentModel.DataAnnotations.Schema;

namespace Scrabble.Models
{
    public class BoardTile
    {
        public string ID { get; set; }
        public int BoardLocationX { get; set; }
        public int BoardLocationY { get; set; }
        public BoardTileType BoardTileType { get; set; }

        public int BoardID { get; set; }
        [ForeignKey("BoardID")]
        public Board Board { get; set; }

        public int CharTileID { get; set; }
        [ForeignKey("CharTileID")]
        public CharTile CharTile { get; set; }

        public BoardTile ()
        {
            /*BoardLocationX = -1;
            BoardLocationY = -1;
            ID = "" + BoardLocationX + "&" + BoardLocationY;
            //WordDictionary = new WordDictionary(Language.English);
            LetterTile = null;
            BoardTileType = BoardTileTypes.Type.None;*/
        }

        public BoardTile(int boardLocationX, int boardLocationY, CharTile charTile = null)
        {
            BoardLocationX = boardLocationX;
            BoardLocationY = boardLocationY;
            ID = "" + BoardLocationX + "&" + BoardLocationY;
            //WordDictionary = new WordDictionary(Language.English);
            CharTile = charTile;
            BoardTileType = BoardTileType.None;
        }
    }
}