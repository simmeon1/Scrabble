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

        public void SubstractFromRack (CharTile tile)
        {
            List<Rack_CharTile> rackTiles = Rack_CharTiles.ToList();
            var tileEntryInDb = rackTiles.Where(c => c.CharTileID == tile.ID).FirstOrDefault();
            if (tileEntryInDb.Count == 1)
            {
                rackTiles.RemoveAll(c => c.CharTileID == tile.ID);
            }
            else tileEntryInDb.Count--;
            Rack_CharTiles = rackTiles;
        }

        public void DrawFromPouch()
        {
            List<Rack_CharTile> rackTiles = Rack_CharTiles.ToList();
            int countOfRackTiles = 0;
            foreach (Rack_CharTile charTileDbEntry in Rack_CharTiles)
            {
                countOfRackTiles += charTileDbEntry.Count;
            }
            for (int i = countOfRackTiles; i < RackSize; i++)
            {
                var randomTile = Pouch.PickRandomTile();
                if (randomTile == null)
                {
                    break;
                }
                if (rackTiles.Any(t => t.CharTileID == randomTile.ID)) {
                    var tileEntryInDb = rackTiles.Where(t => t.CharTileID == randomTile.ID).FirstOrDefault();
                    tileEntryInDb.Count++;
                } else
                {
                    rackTiles.Add(new Rack_CharTile { RackID = ID, CharTileID = randomTile.ID, Count = 1 });
                }
            }
            Rack_CharTiles = rackTiles;
        }
    }
}