using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Scrabble.Models
{
    public class Rack
    {
        public int ID { get; set; }
        public int RackSize { get; set; }
        //public int RackCount { get; set; }

        public int PlayerID { get; set; }
        [ForeignKey("PlayerID")]
        public virtual Player Player { get; set; }

        public int PouchID { get; set; }
        [ForeignKey("PouchID")]
        public virtual Pouch Pouch { get; set; }

        public int GameID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        //[NotMapped]
        public virtual ICollection<Rack_CharTile> Rack_CharTiles { get; set; }

        /*public Rack()
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            //RackTiles = new List<CharTile>();
            RackSize = 7;
            Pouch = new Pouch();
            RackCount = 0;
        }

        public Rack (Player player, int size, Pouch pouch)
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            Player = player;
            PlayerID = Player.ID;
            Pouch = pouch;
            PouchID = Pouch.ID;
            CharTiles = new List<CharTile>();
            //RackTiles = new List<CharTile>();
            RackSize = size;
            RackCount = 0;
        }

        public void Draw()
        {
            for (int i = RackCount; i < RackSize; i++)
            {
                CharTiles.Add(Pouch.CharTiles[0]);
                Pouch.CharTiles.RemoveAt(0);
            }
        }*/

        public override string ToString()
        {
            string temp = "";
            foreach (Rack_CharTile c in Rack_CharTiles)
            {
                for (int i = 0; i < c.Count; i++)
                {
                    temp = temp + c.CharTile.Letter;
                }            
            }
            return temp;
        }
    }
}