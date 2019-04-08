using Scrabble.Classes;

namespace Scrabble.Models
{
    /// <summary>
    /// Represents board tile type
    /// Can be normal, start, double/triple letter, double/triple word
    /// </summary>
    public class BoardTileType

    {
        public BoardTileTypeEnum ID { get; set; }

        public string Type { get; set; }
    }
}
