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

        public int PlayerID { get; set; }
        [ForeignKey("PlayerID")]
        public virtual Player Player { get; set; }

        public int PouchID { get; set; }
        [ForeignKey("PouchID")]
        public virtual Pouch Pouch { get; set; }

        public int GameID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        public virtual ICollection<Rack_CharTile> Rack_CharTiles { get; set; }

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

        public void SubstractFromRack(char c)
        {
            List<Rack_CharTile> rackTiles = Rack_CharTiles.ToList();
            var tileEntryInDb = rackTiles.Where(x => x.CharTile.Letter == c).FirstOrDefault();
            if (tileEntryInDb.Count == 1)
            {
                rackTiles.RemoveAll(x => x.CharTile.Letter == c);
            }
            else tileEntryInDb.Count--;
            Rack_CharTiles = rackTiles;
        }

        public void AddToRack(CharTile tile)
        {
            List<Rack_CharTile> rackTiles = Rack_CharTiles.ToList();
            var tileEntryInDb = rackTiles.Where(c => c.CharTileID == tile.ID).FirstOrDefault();
            if (tileEntryInDb == null)
            {
                rackTiles.Add(new Rack_CharTile { CharTileID = tile.ID, RackID = ID, Count = 1 });
            }
            else tileEntryInDb.Count++;
            Rack_CharTiles = rackTiles;
        }

        public void AddToRack(char x)
        {
            List<Rack_CharTile> rackTiles = Rack_CharTiles.ToList();
            var tileEntryInDb = rackTiles.Where(c => c.CharTile.Letter == x).FirstOrDefault();
            if (tileEntryInDb == null)
            {
                rackTiles.Add(new Rack_CharTile { CharTileID = Game.WordDictionary.CharTiles.Where(c => c.Letter == x).FirstOrDefault().ID,
                    RackID = ID, Count = 1 });
            }
            else tileEntryInDb.Count++;
            Rack_CharTiles = rackTiles;
        }

        public bool CheckIfTileIsInRack(char c)
        {
            List<Rack_CharTile> rackTiles = Rack_CharTiles.ToList();
            var tileEntryInDb = rackTiles.Where(x => x.CharTile.Letter == c).FirstOrDefault();
            if (tileEntryInDb == null)
            {
                return false;
            }
            return true;
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