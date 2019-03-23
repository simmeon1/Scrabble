using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scrabble.Models
{
    public class Game
    {
        public int ID { get; set; }         
        
        public int GameLanguageID { get; set; }
        public virtual GameLanguage GameLanguage { get; set; }
        //public int RackSize { get; set; }

       /* //[NotMapped]
        public int CurrentPlayerID { get; set; }
        //[ForeignKey("PlayerID")]
        [NotMapped]
        public Player CurrentPlayer { get; set; }*/

        public virtual ICollection<Player> Players { get; set; }
        //public virtual ICollection<Rack> Racks { get; set; }

        //public int WordDictionaryID { get; set; }
        //[ForeignKey("WordDictionaryID")]
        //public WordDictionary WordDictionary { get; set; }

        public int BoardID { get; set; }
        //[ForeignKey("BoardID")]
        public virtual Board Board { get; set; }

        public int PouchID { get; set; }
        //[ForeignKey("PouchID")]
        public virtual Pouch Pouch { get; set; }

        public int WordDictionaryID { get; set; }
        [ForeignKey("WordDictionaryID")]
        public virtual WordDictionary WordDictionary { get; set; }

        public string Log { get; set; }

        /*public int Pouch_CharTileID { get; set; }
        [ForeignKey("Pouch_CharTileID")]
        public virtual Pouch_CharTile Pouch_CharTile { get; set; }*/

        public virtual ICollection<Rack> Racks { get; set; }

        /*public int Rack_Pouch_CharTileID { get; set; }
        [ForeignKey("Rack_Pouch_CharTileID")]
        public Rack_Pouch_CharTile Pouch { get; set; }*/

        //public Game() { }
        /*public Game()
        {
            ID = 1;
            User userHuman = new User("SimeonUser", true);
            User userBot = new User("BotUser", false);
            Player playerHuman = new Player(userHuman);
            Player playerBot = new Player(userBot);
            Board = new Board();
            BoardID = Board.ID;
            Pouch = new Pouch();
            PouchID = Pouch.ID;
            Player = new Player(userHuman);
            Players = new List<Player>();
            Racks = new List<Rack>();
            Players.Add(playerHuman);
            Players.Add(playerBot);
            foreach (Player p in Players)
            {
                Racks.Add(new Rack(p, 7, Pouch));
            }
            foreach (Rack r in Racks)
            {
                r.Draw();
            }
            /*Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            GameLanguage = Language.English;
            WordDictionary = new WordDictionary(GameLanguage);
            Pouch = new Pouch(WordDictionary);
            RackSize = 7;
            Players = new List<Player>();
            Board = new Board(15, 15, WordDictionary);
            CurrentPlayer = null;
        }*/

        /*public Game (Language gameLanguage,
            int rackSize, int rows, int columns)
        {
            Random rnd = new Random();
            ID = rnd.Next(1, 5000);
            GameLanguage = gameLanguage;
            //WordDictionary = new WordDictionary(GameLanguage);
            //Pouch = new Pouch(WordDictionary);
            RackSize = rackSize;
            //Players = new List<Player>();
            //Board = new Board(rows, columns, WordDictionary);
            //CurrentPlayer = null;
        }*/

        /*public void AddPlayer (string id, bool isHuman)
        {
            if (CurrentPlayer == null)
            {
                Players.Add(new Player(id, isHuman, new Rack(RackSize), 0, Pouch));
                CurrentPlayer = Players[0];
            }
            Players.Add(new Player(id, isHuman, new Rack(RackSize), 0, Pouch));
        }*/
    }   
}