using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Scrabble.Models
{
    /// <summary>
    /// Represents pouch
    /// Contains a collection of char tiles
    /// Collection is represented as Pouch_CharTile list
    /// </summary>
    public class Pouch
    {
        public int ID { get; set; }

        public int GameID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        public virtual ICollection<Pouch_CharTile> Pouch_CharTiles { get; set; }

        public CharTile PickRandomTile()
        {
            List<Pouch_CharTile> pouchTiles = Pouch_CharTiles.ToList();
            if (pouchTiles.Count <= 0)
            {
                return null;
            }
            Random rnd = new Random();
            int randomIndex = rnd.Next(0, pouchTiles.Count);
            var tileEntryInDb = pouchTiles[randomIndex];
            while (tileEntryInDb.Count == 0)
            {
                pouchTiles.RemoveAt(randomIndex);
                randomIndex = rnd.Next(0, pouchTiles.Count);
                tileEntryInDb = pouchTiles[randomIndex];
            }
            if (tileEntryInDb.Count <= 1)
            {
                pouchTiles.RemoveAt(randomIndex);
            }
            else
            {
                tileEntryInDb.Count--;
            }
            Pouch_CharTiles = pouchTiles;
            return tileEntryInDb.CharTile;
        }

        public void AddToPouch (CharTile tile)
        {
            List<Pouch_CharTile> pouchTiles = Pouch_CharTiles.ToList();
            var tileEntryInDb = Pouch_CharTiles.Where(c => c.ID == tile.ID).FirstOrDefault();
            if (tileEntryInDb == null)
            {
                pouchTiles.Add(new Pouch_CharTile { CharTileID = tile.ID, PouchID = ID, Count = 1 });
            }
            else tileEntryInDb.Count++;
            Pouch_CharTiles = pouchTiles;
        }

        public override string ToString()
        {
            var output = "";
            List<Pouch_CharTile> pouchTilesList = Pouch_CharTiles.ToList();
            foreach (Pouch_CharTile entry in pouchTilesList)
            {
                output += entry.ToString();
            }
            char[] a = output.ToCharArray();
            Array.Sort(a);
            return new string(a);
        }
    }
}