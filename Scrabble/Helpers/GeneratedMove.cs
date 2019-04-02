using Scrabble.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble.Helpers
{
    public class GeneratedMove
    {
        public bool IsHorizontal { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int[] AnchorCoordinates { get; set; }
        public BoardTile Anchor { get; set; }
        public string Word { get; set; }
        public Dictionary<BoardTile, CharTile> TilesUsed { get; set; }
        public List<BoardTile> RackTilesUsed { get; set; }
        public List<List<BoardTile>> ExtraWordsPlayed { get; set; }
        public int Score { get; set; }
        public BoardTile[,] BoardBeforeMove { get; set; }
        public BoardTile[,] BoardAfterMove { get; set; }

        public GeneratedMove(bool isHorizontal, int startIndex, int endIndex, int[] anchorCoordinates, BoardTile anchor, Dictionary<BoardTile, CharTile> tilesUsed, BoardTile[,] boardBeforeMove)
        {
            IsHorizontal = isHorizontal;
            StartIndex = startIndex;
            EndIndex = endIndex;
            AnchorCoordinates = anchorCoordinates;
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
            RackTilesUsed = new List<BoardTile>();
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
                            RackTilesUsed.Add(BoardBeforeMove[i, j]);
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
            foreach (var rackTile in RackTilesUsed)
            {
                var word = Helpers.Helper.GetVerticalPlay(BoardAfterMove, rackTile);
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
            StringBuilder sb = new StringBuilder();
            foreach (var entry in TilesUsed)
            {
                sb.Append(entry.Value.Letter);
            }
            return sb.ToString();
        }

        public string GetExtraWordsMessage ()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var word in ExtraWordsPlayed)
            {
                foreach (var letter in word)
                {
                    sb.Append(letter.CharTile.Letter);
                }
                sb.Append(";");
            }
            sb.Insert(0, ExtraWordsPlayed.Count + " - ");
            return sb.ToString();
        }

        public override string ToString()
        {
            
            return Word + ", " + (IsHorizontal ? "Horizontal" : "Vertical") + ", " + "Extra words: " + GetExtraWordsMessage() + ", "
                + ", Start Index " + StartIndex + " to " + EndIndex +
                " anchored at " + AnchorCoordinates[0] + ", " + AnchorCoordinates[1] + ", " + Score + " points";
        }
    }
}
