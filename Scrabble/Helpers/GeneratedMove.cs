using Scrabble.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Helpers
{
    public class GeneratedMove
    {
        public bool IsHorizontal { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int[] Anchor { get; set; }
        public string Word { get; set; }
        public Dictionary<BoardTile, CharTile> TilesUsed { get; set; }
        public int Score { get; set; }

        public GeneratedMove(bool isHorizontal, int startIndex, int endIndex, int[] anchor, Dictionary<BoardTile, CharTile> tilesUsed)
        {
            IsHorizontal = isHorizontal;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Anchor = anchor;
            TilesUsed = tilesUsed;
            Word = GetWord();
            Score = GetScore();
        }

        public int GetScore()
        {
            int score = 0;
            List<BoardTile> tilesWithLetters = new List<BoardTile>();
            foreach (var entry in TilesUsed)
            {
                tilesWithLetters.Add(new BoardTile { CharTile = entry.Value, BoardTileType = entry.Key.BoardTileType });
            }
            return Helpers.Helper.GetWordScore(tilesWithLetters);
        }

        public string GetWord()
        {
            string word = "";
            foreach (var entry in TilesUsed)
            {
                word += entry.Value.Letter;
            }
            return word;
        }

        public override string ToString()
        {
            return Word + ", " + (IsHorizontal ? "Horizontal" : "Vertical") + ", " + StartIndex + " to " + EndIndex +
                " anchored at " + Anchor[0] + ", " + Anchor[1] + ", " + Score + " points";
        }
    }
}
