namespace Scrabble.Models
{
    public class Player
    {
        public string Id { get; set; }
        public bool IsHuman { get; set; }
        public Rack Rack { get; set; }
        public int Score { get; set; }
        public Pouch Pouch { get; set; }

        public Player (string id, bool isHuman, Rack rack, int score, Pouch pouch)
        {
            Id = id;
            IsHuman = isHuman;
            Rack = rack;
            Score = score;
            Pouch = pouch;
        }

        public void DrawTilesFromPouch()
        {
            while (Rack.RackTiles.Count < Rack.RackSize && Pouch.PouchTiles.Count > 0)
            {
                Pouch.ShufflePouchTiles();
                Rack.RackTiles.Add(Pouch.PouchTiles[0]);
                Pouch.PouchTiles.RemoveAt(0);
            }
        }
    }
}