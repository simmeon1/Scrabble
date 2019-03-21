﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrabble.Models
{
    public class CharTile
    {
        public int ID { get; set; }
        public char Letter { get; set; }
        public int Score { get; set; }

        public int GameLanguageID { get; set; }
        [ForeignKey("GameLanguageID")]
        public virtual GameLanguage GameLanguage { get; set; }

        /*public int WordDictionaryID { get; set; }
        [ForeignKey("WordDictionaryID")]
        public WordDictionary WordDictionary { get; set; }

        public List<BoardTile> BoardTiles { get; set; }*/

        /* public CharTile()
         {
             /*Random rnd = new Random();
             ID = rnd.Next(1, 5000);
             Letter = '.';
             Score = 0;
         }

         public CharTile(char letter, int score)
         {
             Random rnd = new Random();
             ID = letter - 0;
             Letter = letter;
             Score = score;
         }

         public override string ToString()
         {
             return Letter + "";
         }*/
    }
}