using System.Collections.Generic;

namespace Scrabble.Models
{
    public class Game
    {
        public int ID { get; set; }
        public List<Player> Players { get; set; }
        public Board Board { get; set; }
        public GameLanguages.Language GameLanguage { get; set; }
        public WordDictionary WordDictionary { get; set; }
        public Pouch Pouch { get; set; }
        public int RackSize { get; set; }
        public Player CurrentPlayer { get; set; }

        public Game()
        {
            GameLanguage = GameLanguages.Language.English;
            WordDictionary = new WordDictionary(GameLanguage);
            Pouch = new Pouch(WordDictionary);
            RackSize = 7;
            Players = new List<Player>();
            Board = new Board(15, 15);
            CurrentPlayer = null;
        }

        public Game (GameLanguages.Language gameLanguage,
            int rackSize, int rows, int columns)
        {
            GameLanguage = gameLanguage;
            WordDictionary = new WordDictionary(GameLanguage);
            Pouch = new Pouch(WordDictionary);
            RackSize = rackSize;
            Players = new List<Player>();
            Board = new Board(rows, columns);
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