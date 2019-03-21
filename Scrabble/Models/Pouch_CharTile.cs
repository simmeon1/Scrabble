using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Models
{
    public class Pouch_CharTile
    {
        public int ID { get; set; }

        public int PouchID { get; set; }
        //[ForeignKey("PouchID")]
        public virtual Pouch Pouch { get; set; }

        public int CharTileID { get; set; }
        //[ForeignKey("CharTileID")]
        public virtual CharTile CharTile { get; set; }

        /*public int GameID { get; set; }
        [ForeignKey("GameID")]
        public Game Game { get; set; }*/

        public int Count { get; set; }
    }
}
