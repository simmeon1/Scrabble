using System.Collections.Generic;

namespace Scrabble.Models
{
    public class Game
    {
        public string Id { get; set; }
        //public List<Player> Players { get; set; }
        public List<Player> PlayerData { get; set; }
        public Board Board { get; set; }
        public string GameLanguage { get; set; }
        public WordDictionary WordDictionary { get; set; }
        public Pouch Pouch { get; set; }
        public int RackSize { get; set; }
        public Player CurrentPlayer { get; set; }

        public Game (GameLanguages.Language language,
            int rackSize, int rows, int columns)
        {
            GameLanguage = language.ToString();
            WordDictionary = new WordDictionary(language);
            Pouch = new Pouch(WordDictionary);
            RackSize = rackSize;
            PlayerData = new List<Player>();
            Board = new Board(rows, columns);
            CurrentPlayer = null;
        }

        public void AddPlayer (string id, bool isHuman)
        {
            if (CurrentPlayer == null)
            {
                PlayerData.Add(new Player(id, isHuman, new Rack(RackSize), 0, Pouch));
                CurrentPlayer = PlayerData[0];
            }
            PlayerData.Add(new Player(id, isHuman, new Rack(RackSize), 0, Pouch));
        }
    }   
}