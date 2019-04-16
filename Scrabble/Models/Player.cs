using Scrabble.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Scrabble.Models
{
    /// <summary>
    /// Represents player
    /// Can be human or bot
    /// </summary>
    public class Player
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsHuman { get; set; }
        public BotTypeEnum? BotTypeID { get; set; }
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

        /// <summary>
        /// Trades all their rack tiles for new ones from the pouch
        /// </summary>
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

        /// <summary>
        /// Trades some of their rack tiles for new ones from the pouch
        /// </summary>
        public void Redraw(string letters, string counts)
        {
            var lettersArray = letters.Split(",");
            var countsArray = counts.Split(",");
            var rack = Rack.Rack_CharTiles.ToList();
            var tilesToDraw = 0;

            for (int i = 0; i < lettersArray.Length; i++)
            {
                for (int j = 0; j < Int32.Parse(countsArray[i]); j++)
                {
                    Pouch.AddToPouch(Rack.SubstractFromRack(lettersArray[i][0]));
                    tilesToDraw++;
                }
            }
            Rack.RefillRackFromPouch();
        }
    }
}