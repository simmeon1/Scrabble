using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrabble.Models
{
    public class Rack
    {
        public int ID { get; set; }
        public int RackSize { get; set; }
       
        public int PlayerID { get; set; }
        [ForeignKey("PlayerID")]
        public Player Player { get; set; }

        public List<CharTile> RackTiles { get; set; }

        public Rack()
        {
            /*Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            RackTiles = new List<CharTile>();
            RackSize = 7;*/
        }

        public Rack (int size)
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
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