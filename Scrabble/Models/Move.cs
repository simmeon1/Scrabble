using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Scrabble.Models
{
    /// <summary>
    /// Represents a move (play) and it's details
    /// </summary>
    public class Move
    {
        public int ID { get; set; }
        public bool IsHorizontal { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int Index { get; set; }
        public string Word { get; set; }
        public int Score { get; set; }
        public bool IsNew { get; set; }

        //Multiple moves/words can be part of the same play
        public int MoveNumber { get; set; }

        public int PlayerID { get; set; }
        [ForeignKey("PlayerID")]
        public virtual Player Player { get; set; }

        public int GameID { get; set; }
        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        public string GetStringForPage()
        {
            return IsHorizontal + "_" + IsNew + "_" + Start + "_" + End + "_" + Index + "_" + Word + "_" + Score + "_" + MoveNumber; 
        }
    }
}
