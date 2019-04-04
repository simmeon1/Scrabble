using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Scrabble.Models
{
    public class WordDictionary
    {
        public int ID { get; set; }

        public int GameLanguageID { get; set; }
        [ForeignKey("GameLanguageID")]
        public virtual GameLanguage GameLanguage { get; set; }

        public virtual ICollection<CharTile> CharTiles { get; set; }
        public virtual ICollection<Game> Games { get; set; }

    }
}