using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrabble.Models
{
    public class Player
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsHuman { get; set; }
        public bool AtHand { get; set; }
        public int Score { get; set; }

        public int RackID { get; set; }
        //[ForeignKey("RackID")]
        public virtual Rack Rack { get; set; }

        public int PouchID { get; set; }
        [ForeignKey("PouchID")]
        public virtual Pouch Pouch { get; set; }

        public int GameID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        /*public int Pouch_CharTileID { get; set; }
        [ForeignKey("Pouch_CharTileID")]
        public Pouch_CharTile Pouch_CharTile { get; set; }*/

        //public int UserID { get; set; }
        //[ForeignKey("UserID")]
        //public virtual User User { get; set; }

        //EF Core can't do many to many
        //public List<Game> Games { get; set; }

        /*public int GameID { get; set; }
        [ForeignKey("GameID")]
        public Game Game { get; set; }*/

        /*public Player(User user)
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            Name = "SimeonPlayer";
            User = user;
            /*Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            Name = "Simeon";
            IsHuman = true;
            Rack = new Rack(7);
            Score = 0;
            Pouch = pouch;
        }

        public Player()
        {
            ID = 1;
            Name = "SimeonPlayer";
            User = new User();
        }*/

        /*public Player (string name, Rack rack, int score, Pouch pouch)
        {
            Name = name;
            //IsHuman = isHuman;
            Rack = rack;
            Score = score;
            Pouch = pouch;
        }*/

        /*public void DrawTilesFromPouch()
        {
            while (Rack.RackTiles.Count < Rack.RackSize && Pouch.PouchTiles.Count > 0)
            {
                Pouch.ShufflePouchTiles();
                Rack.RackTiles.Add(Pouch.PouchTiles[0]);
                Pouch.PouchTiles.RemoveAt(0);
            }
        }*/
    }
}