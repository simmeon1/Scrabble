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

        public int Count { get; set; }

    }
}
