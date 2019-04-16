using Scrabble.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrabble.Models
{
    /// <summary>
    /// Represents a tile with a letter
    /// Can be located in rack or on the board
    /// </summary>
    public class CharTile
    {
        public int ID { get; set; }
        public char Letter { get; set; }
        public int Score { get; set; }

        public LanguageEnum GameLanguageID { get; set; }
        [ForeignKey("GameLanguageID")]
        public virtual GameLanguage GameLanguage { get; set; }

        public int WordDictionaryID { get; set; }
        [ForeignKey("WordDictionaryID")]
        public virtual WordDictionary WordDictionary { get; set; }

        public override string ToString()
        {
            return Letter.ToString() == "" ? "_" : Letter.ToString();
        }

    }
}