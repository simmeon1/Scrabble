﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Scrabble.Models
{
    public class BoardTile
    {
        public int ID { get; set; }
        public int BoardLocationX { get; set; }
        public int BoardLocationY { get; set; }
        public bool IsTaken { get; set; }

        public int BoardTileTypeID { get; set; }
        [ForeignKey("BoardTileTypeID")]
        public virtual BoardTileType BoardTileType { get; set; }

        public int BoardID { get; set; }
        [ForeignKey("BoardID")]
        public virtual Board Board { get; set; }

        public int? CharTileID { get; set; }
        [ForeignKey("CharTileID")]
        public virtual CharTile CharTile { get; set; }

        public override string ToString()
        {
            return "X" + BoardLocationX + ", Y" + BoardLocationY + ", " + (CharTile == null ? "_" : CharTile.Letter.ToString());
        }
    }
}