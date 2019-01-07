using System;
using System.Collections.Generic;

namespace Scrabble.Models
{
    public class Player
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsHuman { get; set; }
        public int Score { get; set; }

        public int RackID { get; set; }
        public Rack Rack { get; set; }

        public int PouchID { get; set; }
        public Pouch Pouch { get; set; }

        public List<Game> Games { get; set; }

        public Player()
        {
            /*Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            Name = "Simeon";
            IsHuman = true;
            Rack = new Rack(7);
            Score = 0;
            Pouch = pouch;*/
        }

        public Player (string name, bool isHuman, Rack rack, int score, Pouch pouch)
        {
            Name = name;
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