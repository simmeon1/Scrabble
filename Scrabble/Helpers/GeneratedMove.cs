using Scrabble.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Helpers
{
    /// <summary>
    /// Represents a generated move from the Move Generator.
    /// </summary>
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
        public Rack Rack { get; set; }
        public double RackScore { get; set; }
        public BoardTile[,] BoardBeforeMove { get; set; }
        public BoardTile[,] BoardAfterMove { get; set; }
        public GeneratedMove(bool isHorizontal, int startIndex, int endIndex, int[] anchor, Dictionary<BoardTile, CharTile> tilesUsed, BoardTile[,] boardBeforeMove, Rack rack)
        {
            IsHorizontal = isHorizontal;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Anchor = anchor;
            TilesUsed = tilesUsed;
            Word = GetWord();
            BoardBeforeMove = new BoardTile[boardBeforeMove.GetLength(0), boardBeforeMove.GetLength(1)];
            RackTilesUsedCoordinates = new List<int[]>();
            ExtraWordsPlayed = new List<List<BoardTile>>();

            //Creates a copy of the original board before move generation
            //Used to get the scores of it and words connected.
            for (int i = 0; i < BoardBeforeMove.GetLength(0); i++)
            {
                for (int j = 0; j < BoardBeforeMove.GetLength(1); j++)
                {
                    BoardBeforeMove[i, j] = new BoardTile
                    {
                        BoardLocationX = boardBeforeMove[i, j].BoardLocationX,
                        BoardLocationY = boardBeforeMove[i, j].BoardLocationY,
                        BoardTileType = boardBeforeMove[i, j].BoardTileType,
                        CharTile = boardBeforeMove[i, j].CharTile,
                        IsFilled = boardBeforeMove[i, j].IsFilled
                    };
                    foreach (var tileUsed in TilesUsed)
                    {
                        if (BoardBeforeMove[i, j].BoardLocationX == tileUsed.Key.BoardLocationX
                            && BoardBeforeMove[i, j].BoardLocationY == tileUsed.Key.BoardLocationY
                            && BoardBeforeMove[i, j].CharTile == null)
                        {
                            RackTilesUsedCoordinates.Add(new int[] { i, j });
                            BoardBeforeMove[i, j].CharTile = tileUsed.Value;

                        }
                    }
                }
            }
            BoardAfterMove = BoardBeforeMove;
            Score = GetScore();
            Rack = new Rack { Rack_CharTiles = new List<Rack_CharTile>(rack.Rack_CharTiles) };
            RackScore = GetRackScore(Rack);
        }

        public double GetRackScore(Rack rack)
        {
            double score = 0;
            int vowels = 0;
            int consonants = 0;
            int letters = 0;
            double vcMix = 0;
            var listOfRackTiles = rack.Rack_CharTiles.ToList();
            foreach (var entry in listOfRackTiles)
            {
                for (int i = 0; i < entry.Count; i++)
                {
                    if (entry.CharTile.Letter == 'A') score += (1.0-(3.0*i));
                    else if (entry.CharTile.Letter == 'B') score += (-3.5-(3.0*i));
                    else if (entry.CharTile.Letter == 'C') score += (-0.5-(3.5*i));
                    else if (entry.CharTile.Letter == 'D') score += 0-(2.5*i);
                    else if (entry.CharTile.Letter == 'E') score += 4-(2.5*i);
                    else if (entry.CharTile.Letter == 'F') score += -2.0-(2.0*i);
                    else if (entry.CharTile.Letter == 'G') score += -2.0-(2.5*i);
                    else if (entry.CharTile.Letter == 'H') score += 0.5-(3.5*i);
                    else if (entry.CharTile.Letter == 'I') score += -0.5-(4.0 * i);
                    else if (entry.CharTile.Letter == 'J') score += -3.0 -(0.0 * i);
                    else if (entry.CharTile.Letter == 'K') score += -2.5-(0.0 * i);
                    else if (entry.CharTile.Letter == 'L') score += -1.0-(2.0 * i);
                    else if (entry.CharTile.Letter == 'M') score += -1.0-(2.0 * i);
                    else if (entry.CharTile.Letter == 'N') score += 0.5-(2.5 * i);
                    else if (entry.CharTile.Letter == 'O') score += -1.5-(3.5 * i);
                    else if (entry.CharTile.Letter == 'P') score += -1.5-(2.5 * i);
                    else if (entry.CharTile.Letter == 'Q') score += -11.5-(0.0 * i);
                    else if (entry.CharTile.Letter == 'R') score += 1.5-(3.5 * i);
                    else if (entry.CharTile.Letter == 'S') score += 7.5-(4.0 * i);
                    else if (entry.CharTile.Letter == 'T') score += 0-(2.5 * i);
                    else if (entry.CharTile.Letter == 'U') score += -3.0-(3.0 * i);
                    else if (entry.CharTile.Letter == 'V') score += -5.5-(3.5 * i);
                    else if (entry.CharTile.Letter == 'W') score += -4.0-(4.5 * i);
                    else if (entry.CharTile.Letter == 'X') score += 3.5-(0.0 * i);
                    else if (entry.CharTile.Letter == 'Y') score += -2.0-(4.5 * i);
                    else if (entry.CharTile.Letter == 'Z') score += 2.0-(0.0 * i);
                    else if (entry.CharTile.Letter == '*') score += 24.5-(15.0 * i);
                    letters++;
                    if ("aeiouAEIOU".Contains(entry.CharTile.Letter)) vowels++;
                    else if (entry.CharTile.Letter == '*')
                    {
                        vowels++;
                        consonants++;
                    }
                    else consonants++;
                }
            }
            vcMix = Math.Min(3 * vowels + 1, 3 * consonants) - Math.Min(3 * vowels + 1 - letters, 2 * letters - 3 * vowels);
            score += vcMix;
            return score;
        }

        public int GetScore()
        {
            int score = 0;
            List<BoardTile> tilesWithLetters = new List<BoardTile>();
            foreach (var entry in TilesUsed)
            {
                tilesWithLetters.Add(new BoardTile { CharTile = entry.Value, BoardTileType = entry.Key.BoardTileType,
                    BoardLocationX = entry.Key.BoardLocationX, BoardLocationY = entry.Key.BoardLocationY, IsFilled = entry.Key.IsFilled });
            }
            foreach (var rackTileCoordinates in RackTilesUsedCoordinates)
            {
                var word = Helper.GetVerticalPlays(BoardAfterMove, rackTileCoordinates);
                if (word != null)
                {
                    ExtraWordsPlayed.Add(word);
                }            
            }
            foreach (var extraWord in ExtraWordsPlayed)
            {
                score += Helper.GetWordScore(extraWord);
            }
            if (RackTilesUsedCoordinates.Count == 7) score += 50;
            return score + Helper.GetWordScore(tilesWithLetters);
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
                " anchored at " + Anchor[0] + ", " + Anchor[1] + ", Score: " + Score + " points, Rack Score: " + RackScore;
        }
    }
}
