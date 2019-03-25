using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Scrabble.Models
{
    public class Pouch
    {
        public int ID { get; set; }

        public int GameID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        public virtual ICollection<Pouch_CharTile> Pouch_CharTiles { get; set; }

        /*public int GameID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        [NotMapped]
        //public ICollection<CharTile> CharTiles { get; set; }

        //public List<Player> Players { get; set; }

        /*public Pouch()
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            CharTiles = new List<CharTile>();
            {
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('*', 0), 2).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('A', 1), 9).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('B', 3), 2).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('C', 3), 2).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('D', 2), 4).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('E', 1), 12).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('F', 4), 2).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('G', 2), 3).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('H', 4), 2).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('I', 1), 9).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('J', 8), 1).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('K', 5), 1).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('L', 1), 4).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('M', 3), 2).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('N', 1), 6).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('O', 1), 8).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('P', 3), 2).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('Q', 10), 1).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('R', 1), 6).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('S', 1), 4).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('T', 1), 6).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('U', 1), 4).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('V', 4), 2).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('W', 4), 2).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('X', 8), 1).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('Y', 4), 2).ToList());
                CharTiles.AddRange(Enumerable.Repeat(new CharTile('Z', 10), 1).ToList());
            }
            /*Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            PouchTiles = new WordDictionary(Language.English).CharTiles;
        }

        public Pouch(WordDictionary wordDictionary)
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            //PouchTiles = wordDictionary.CharTiles;
        }

        /*public void ShufflePouchTiles()
        {
            Random rng = new Random();
            int n = PouchTiles.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                CharTile tile = PouchTiles[k];
                PouchTiles[k] = PouchTiles[n];
                PouchTiles[n] = tile;
            }
        }*/

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

        public override string ToString()
        {
            var output = "";
            List<Pouch_CharTile> pouchTilesList = Pouch_CharTiles.ToList();
            foreach (Pouch_CharTile entry in pouchTilesList)
            {
                output += entry.ToString();
            }
            return output;
        }
    }
}