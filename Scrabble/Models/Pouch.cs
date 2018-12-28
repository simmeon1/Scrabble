using System;
using System.Collections.Generic;

namespace Scrabble.Models
{
    public class Pouch
    {
        public List<CharTile> PouchTiles { get; set; }
        public Pouch(WordDictionary wordDictionary)
        {
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