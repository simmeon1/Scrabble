namespace Scrabble.Models
{
    public class CharTile
    {
        public char Letter { get; set; }
        public int Score { get; set; }
        public CharTile(char letter, int score)
        {
            Letter = letter;
            Score = score;
        }

        public override string ToString()
        {
            return Letter + "";
        }
    }
}