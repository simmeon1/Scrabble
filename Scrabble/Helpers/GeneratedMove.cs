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
        public List<int[]> RackTilesUsedCoordinates { get; set; }
        public List<List<BoardTile>> ExtraWordsPlayed { get; set; }
        public int Score { get; set; }
        public BoardTile[,] BoardBeforeMove { get; set; }
        public BoardTile[,] BoardAfterMove { get; set; }

        public GeneratedMove(bool isHorizontal, int startIndex, int endIndex, int[] anchor, Dictionary<BoardTile, CharTile> tilesUsed, BoardTile[,] boardBeforeMove)
        {
            IsHorizontal = isHorizontal;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Anchor = anchor;
            TilesUsed = tilesUsed;
            Word = GetWord();
            BoardBeforeMove = new BoardTile[boardBeforeMove.GetLength(0), boardBeforeMove.GetLength(1)];
            for (int i = 0; i < BoardBeforeMove.GetLength(0); i++)
            {
                for (int j = 0; j < BoardBeforeMove.GetLength(1); j++)
                {
                    BoardBeforeMove[i, j] = new BoardTile
                    {
                        BoardLocationX = boardBeforeMove[i, j].BoardLocationX,
                        BoardLocationY = boardBeforeMove[i, j].BoardLocationY,
                        BoardTileType = boardBeforeMove[i, j].BoardTileType,
                        CharTile = boardBeforeMove[i, j].CharTile
                    };
                }
            }
            RackTilesUsedCoordinates = new List<int[]>();
            ExtraWordsPlayed = new List<List<BoardTile>>();
            foreach (var tileUsed in TilesUsed)
            {
                for (int i = 0; i < BoardBeforeMove.GetLength(0); i++)
                {
                    for (int j = 0; j < BoardBeforeMove.GetLength(1); j++)
                    {
                        if (BoardBeforeMove[i,j].BoardLocationX == tileUsed.Key.BoardLocationX
                            && BoardBeforeMove[i, j].BoardLocationY == tileUsed.Key.BoardLocationY
                            && BoardBeforeMove[i, j].CharTile == null)
                        {
                            RackTilesUsedCoordinates.Add(new int[] { i, j });
                            BoardBeforeMove[i,j].CharTile = tileUsed.Value;

                        }
                    }
                }
            }
            BoardAfterMove = BoardBeforeMove;
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
            foreach (var rackTileCoordinates in RackTilesUsedCoordinates)
            {
                var word = Helpers.Helper.GetVerticalPlays(BoardAfterMove, rackTileCoordinates);
                if (word != null)
                {
                    ExtraWordsPlayed.Add(word);
                }            
            }
            foreach (var extraWord in ExtraWordsPlayed)
            {
                score += Helper.GetWordScore(extraWord);
            }
            return score + Helpers.Helper.GetWordScore(tilesWithLetters);
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

        public string GetExtraWordsMessage ()
        {
            var extraWordsMessage = "";
            foreach (var word in ExtraWordsPlayed)
            {
                foreach (var letter in word)
                {
                    extraWordsMessage += letter.CharTile.Letter;
                }
                extraWordsMessage += ";";
            }
            return extraWordsMessage.Insert(0, ExtraWordsPlayed.Count + " - ");
        }

        public override string ToString()
        {
            
            return Word + ", " + (IsHorizontal ? "Horizontal" : "Vertical") + ", " + "Extra words: " + GetExtraWordsMessage() + ", "
                + ", Start Index " + StartIndex + " to " + EndIndex +
                " anchored at " + Anchor[0] + ", " + Anchor[1] + ", " + Score + " points";
        }
    }
}
