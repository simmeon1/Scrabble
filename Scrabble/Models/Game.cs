﻿using System;
using System.Collections.Generic;

namespace Scrabble.Models
{
    public class Game
    {
        public int ID { get; set; }             
        public Language GameLanguage { get; set; }
        public int RackSize { get; set; }

        public int CurrentPlayerID { get; set; }
        public Player CurrentPlayer { get; set; }

        public List<Player> Players { get; set; }

        public int WordDictionaryID { get; set; }
        public WordDictionary WordDictionary { get; set; }

        public int BoardID { get; set; }
        public Board Board { get; set; }

        public int PouchID { get; set; }
        public Pouch Pouch { get; set; }

        public Game()
        {
            /*Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            GameLanguage = Language.English;
            WordDictionary = new WordDictionary(GameLanguage);
            Pouch = new Pouch(WordDictionary);
            RackSize = 7;
            Players = new List<Player>();
            Board = new Board(15, 15, WordDictionary);
            CurrentPlayer = null;*/
        }

        public Game (Language gameLanguage,
            int rackSize, int rows, int columns)
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            GameLanguage = gameLanguage;
            WordDictionary = new WordDictionary(GameLanguage);
            Pouch = new Pouch(WordDictionary);
            RackSize = rackSize;
            Players = new List<Player>();
            Board = new Board(rows, columns, WordDictionary);
            CurrentPlayer = null;
        }

        public void AddPlayer (string id, bool isHuman)
        {
            if (CurrentPlayer == null)
            {
                Players.Add(new Player(id, isHuman, new Rack(RackSize), 0, Pouch));
                CurrentPlayer = Players[0];
            }
            Players.Add(new Player(id, isHuman, new Rack(RackSize), 0, Pouch));
        }
    }   
}