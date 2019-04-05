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

        public void SubstractFromRack(CharTile tile)
        {
            List<Rack_CharTile> rackTiles = Rack_CharTiles.ToList();
            var tileEntryInDb = rackTiles.Where(c => c.CharTileID == tile.ID).FirstOrDefault();
            if (tileEntryInDb == null)
            {
                tileEntryInDb = rackTiles.Where(x => x.CharTile.Letter == '*').FirstOrDefault();
            }
            if (tileEntryInDb.Count == 1)
            {
                rackTiles.RemoveAll(c => c.CharTileID == tileEntryInDb.CharTile.ID);
            }
            else tileEntryInDb.Count--;
            Rack_CharTiles = rackTiles;
        }

        public CharTile SubstractFromRack(char c)
        {
            CharTile tile = null;
            List<Rack_CharTile> rackTiles = Rack_CharTiles.ToList();
            var tileEntryInDb = rackTiles.Where(t => t.CharTile.Letter == c && t.CharTile.Score != 0).FirstOrDefault();
            if (tileEntryInDb == null)
            {
                tileEntryInDb = rackTiles.Where(x => x.CharTile.Letter == '*').FirstOrDefault();
            }
            tile = tileEntryInDb.CharTile;
            if (tileEntryInDb.Count == 1)
            {
                rackTiles.RemoveAll(t => t.CharTile.Letter == tileEntryInDb.CharTile.Letter
                && t.CharTile.Score == tileEntryInDb.CharTile.Score);
            }
            else tileEntryInDb.Count--;
            Rack_CharTiles = rackTiles;
            return tile;
        }

        public CharTile GetTile(char c)
        {
            CharTile tile = null;
            List<Rack_CharTile> rackTiles = Rack_CharTiles.ToList();
            var tileEntryInDb = rackTiles.Where(t => t.CharTile.Letter == c && t.CharTile.Score != 0).FirstOrDefault();
            if (tileEntryInDb == null)
            {
                tileEntryInDb = rackTiles.Where(x => x.CharTile.Letter == '*').FirstOrDefault();
            }
            tile = tileEntryInDb.CharTile;
            return tile;
        }

        public void AddToRack(CharTile tile)
        {
            List<Rack_CharTile> rackTiles = Rack_CharTiles.ToList();
            var tileCount = 0;
            foreach (var entry in rackTiles)
            {
                tileCount += entry.Count;
            }
                var tileEntryInDb = rackTiles.Where(c => c.CharTileID == tile.ID).FirstOrDefault();
                if (tileEntryInDb == null)
                {
                    rackTiles.Add(new Rack_CharTile { CharTileID = tile.ID, RackID = ID, Count = 1, CharTile = tile, Rack = this });
                }
                else tileEntryInDb.Count++;
                Rack_CharTiles = rackTiles;
        }

        public bool CheckIfTileIsInRack(char c, bool includingBlanks)
        {
            List<Rack_CharTile> rackTiles = Rack_CharTiles.ToList();
            var tileEntryInDb = rackTiles.Where(x => x.CharTile.Letter == c).FirstOrDefault();
            if (tileEntryInDb == null)
            {
                if (includingBlanks)
                {
                    return rackTiles.Any(x => x.CharTile.Letter == '*');
                }
                return false;
            }
            return true;
        }

        public bool CheckIfWordIsPlayable(string word)
        {
            bool playable = true;
            List<CharTile> listOfTiles = new List<CharTile>();
            foreach (char c in word)
            {
                if (CheckIfTileIsInRack(c, true))
                {
                    listOfTiles.Add(SubstractFromRack(c));
                }
                else
                {
                    playable = false;
                    break;
                }
            }
            foreach (var tile in listOfTiles)
            {
                AddToRack(tile);
            }
            return playable;
        }

        public void RefillRackFromPouch()
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
                if (rackTiles.Any(t => t.CharTileID == randomTile.ID))
                {
                    var tileEntryInDb = rackTiles.Where(t => t.CharTileID == randomTile.ID).FirstOrDefault();
                    tileEntryInDb.Count++;
                }
                else
                {
                    rackTiles.Add(new Rack_CharTile { RackID = ID, CharTileID = randomTile.ID, Count = 1 });
                }
            }
            Rack_CharTiles = rackTiles;
        }
    }
}