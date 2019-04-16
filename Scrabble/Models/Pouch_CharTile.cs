using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Models
{
    /// <summary>
    /// Represent tile inforamtion in a Pouch
    /// Example: Pouch IIISA has 3 entries:
    /// I: 3, S: 1 and A: 1
    /// </summary>
    public class Pouch_CharTile
    {
        public int ID { get; set; }

        public int PouchID { get; set; }
        public virtual Pouch Pouch { get; set; }

        public int CharTileID { get; set; }
        public virtual CharTile CharTile { get; set; }

        public int Count { get; set; }

        public override string ToString()
        {
            var output = "";
            for (int i = 0; i < Count; i++)
            {
                output += CharTile.Letter;
            }
            return output;
        }
    }

   
}
