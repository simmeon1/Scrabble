using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Scrabble.Models
{
    public class Player
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsHuman { get; set; }
        public bool AtHand { get; set; }
        public int Score { get; set; }
        public int SkipsOrRedrawsUsed { get; set; }

        public int RackID { get; set; }
        public virtual Rack Rack { get; set; }

        public int PouchID { get; set; }
        [ForeignKey("PouchID")]
        public virtual Pouch Pouch { get; set; }

        public int GameID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        public virtual ICollection<Move> Moves { get; set; }

        public void Redraw()
        {
            var rack = Rack.Rack_CharTiles.ToList();
            var tilesToDraw = 0;
            for (int i = 0; i < rack.Count; i++)
            {
                for (int j = 0; j < rack[i].Count; j++)
                {
                    Rack.Pouch.AddToPouch(rack[i].CharTile);
                    tilesToDraw++;
                }
            }
            rack.Clear();
            Rack.Rack_CharTiles = rack;
            Rack.RefillRackFromPouch();
        }
    }
}