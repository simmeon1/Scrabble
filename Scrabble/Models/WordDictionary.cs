using System.Collections.Generic;
using System.Linq;

namespace Scrabble.Models
{
    public class WordDictionary
    {
        public int ID { get; set; }
        public List<CharTile> LetterTiles { get; set; }

        public WordDictionary()
        {
            LetterTiles = new List<CharTile>();
            {
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('*', 0), 2).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('A', 1), 9).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('B', 3), 2).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('C', 3), 2).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('D', 2), 4).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('E', 1), 12).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('F', 4), 2).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('G', 2), 3).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('H', 4), 2).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('I', 1), 9).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('J', 8), 1).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('K', 5), 1).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('L', 1), 4).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('M', 3), 2).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('N', 1), 6).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('O', 1), 8).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('P', 3), 2).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('Q', 10), 1).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('R', 1), 6).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('S', 1), 4).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('T', 1), 6).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('U', 1), 4).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('V', 4), 2).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('W', 4), 2).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('X', 8), 1).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('Y', 4), 2).ToList());
                LetterTiles.AddRange(Enumerable.Repeat(new CharTile('Z', 10), 1).ToList());

            };
        }

        public WordDictionary(GameLanguages.Language language)
        {
            if (language == GameLanguages.Language.English)
            {
                LetterTiles = new List<CharTile>();
                {
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('*', 0), 2).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('A', 1), 9).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('B', 3), 2).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('C', 3), 2).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('D', 2), 4).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('E', 1), 12).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('F', 4), 2).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('G', 2), 3).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('H', 4), 2).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('I', 1), 9).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('J', 8), 1).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('K', 5), 1).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('L', 1), 4).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('M', 3), 2).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('N', 1), 6).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('O', 1), 8).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('P', 3), 2).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('Q', 10), 1).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('R', 1), 6).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('S', 1), 4).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('T', 1), 6).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('U', 1), 4).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('V', 4), 2).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('W', 4), 2).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('X', 8), 1).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('Y', 4), 2).ToList());
                    LetterTiles.AddRange(Enumerable.Repeat(new CharTile('Z', 10), 1).ToList());
                }
            };
        }
    }
}