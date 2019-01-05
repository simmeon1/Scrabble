using System.Collections.Generic;

namespace Scrabble.Models
{
    public class Rack
    {
        public int RackSize { get; set; }
        public List<CharTile> RackTiles { get; set; }

        public Rack()
        {
            RackTiles = new List<CharTile>();
            RackSize = 7;
        }

        public Rack (int size)
        {
            RackTiles = new List<CharTile>();
            RackSize = size;
        }

        public override string ToString()
        {
            string temp = "";
            foreach (CharTile c in RackTiles)
            {
                temp = temp + c.Letter;
            }
            return temp;
        }
    }
}