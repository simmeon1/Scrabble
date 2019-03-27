using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Models
{
    public class Move
    {
        public int ID { get; set; }
        public string Word { get; set; }
        public int Score { get; set; }

        public int PlayerID { get; set; }
        [ForeignKey("PlayerID")]
        public virtual Player Player { get; set; }

        public int GameID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }
    }
}
