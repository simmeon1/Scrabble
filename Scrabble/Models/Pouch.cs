using System;
using System.Collections.Generic;

namespace Scrabble.Models
{
    public class Pouch
    {
        public int ID { get; set; }
        public List<CharTile> PouchTiles { get; set; }

        public Pouch()
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            PouchTiles = new WordDictionary(GameLanguages.Language.English).LetterTiles;
        }

        public Pouch(WordDictionary wordDictionary)
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            PouchTiles = wordDictionary.LetterTiles;
        }

        public void ShufflePouchTiles()
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
        }
    }
}