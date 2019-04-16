using Scrabble.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Scrabble.Models
{
    /// <summary>
    /// REpresents dictionary of a game
    /// </summary>
    public class WordDictionary
    {
        public int ID { get; set; }

        public LanguageEnum GameLanguageID { get; set; }
        [ForeignKey("GameLanguageID")]
        public virtual GameLanguage GameLanguage { get; set; }

        public virtual ICollection<CharTile> CharTiles { get; set; }
        public virtual ICollection<Game> Games { get; set; }

    }
}