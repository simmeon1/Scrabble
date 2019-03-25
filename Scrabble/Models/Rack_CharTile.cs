using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace Scrabble.Models
{
    public class Rack_CharTile
    {
        public int ID { get; set; }

        public int RackID { get; set; }
        [ForeignKey("RackID")]
        public virtual Rack Rack { get; set; }

        public int CharTileID { get; set; }
        [ForeignKey("CharTileID")]
        public virtual CharTile CharTile { get; set; }

        /*public int Pouch_CharTileID { get; set; }
        [ForeignKey("Pouch_CharTileID")]
        public Pouch_CharTile Pouch_CharTile { get; set; }*/

        /*public int GameID { get; set; }
        [ForeignKey("GameID")]
        public Game Game { get; set; }*/

        public int Count { get; set; }

        //public Rack_CharTile (int id, int rackId, int charTileId)
        //{
        //    ID = id;
        //    RackID = rackId;
        //    CharTileID = charTileId;
        //    Count = 1;
        //}
    }
}
