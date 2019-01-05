namespace Scrabble.Models
{
    public class Player
    {
        //public int ID { get; set; }
        public string Name { get; set; }
        public bool IsHuman { get; set; }
        public Rack Rack { get; set; }
        public int Score { get; set; }
        public Pouch Pouch { get; set; }

        public Player()
        {
            Name = "Simeon";
            IsHuman = true;
            Rack = new Rack(7);
            Score = 0;
            Pouch = new Pouch(new WordDictionary(GameLanguages.Language.English));
        }

        public Player (string id, bool isHuman, Rack rack, int score, Pouch pouch)
        {
            Name = id;
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