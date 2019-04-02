using Scrabble.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        public virtual ICollection<Move> Moves { get; set; }

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

        public Player GetPlayerAtHand()
        {
            List<Player> playersList = Players.ToList();
            for (int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i].AtHand)
                {
                    return playersList[i];
                }
            }
            return null;
        }

        public void AddScoreToPlayer(Player player, int score)
        {
            player.Score += score;
        }

        public void SwitchToNextPlayer()
        {
            Player newPlayerAtHand = null;
            List<Player> playersList = Players.ToList();
            for (int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i].AtHand)
                {
                    playersList[i].AtHand = false;
                    if (i == playersList.Count - 1)
                    {
                        playersList[0].AtHand = true;
                        newPlayerAtHand = playersList[0];
                    }
                    else
                    {
                        playersList[i + 1].AtHand = true;
                        newPlayerAtHand = playersList[i + 1];
                    }
                    break;
                }
            }
            if (!newPlayerAtHand.IsHuman)
            {
                var moveGenerator = Helper.GetMoveGenerator(this, Moves.Where(m => m.GameID == ID).ToList(), 5);
                var validUntransposedMovesList = moveGenerator.GetValidMoves(true);
                var validTransposedMovesList = moveGenerator.GetValidMoves(false);
                var allValidMoves = validUntransposedMovesList.Concat(validTransposedMovesList).ToList();
                var allValidMovesSorted = allValidMoves.OrderByDescending(m => m.Score).ToList();
                if (allValidMovesSorted.Count > 0) MakeGeneratedMove(allValidMovesSorted[0]);
                SwitchToNextPlayer();
            }
            Players = playersList;
        }

        public void MakeGeneratedMove(GeneratedMove move)
        {
            var board = move.IsHorizontal ? Board.ConvertTo2DArray() : Board.Transpose2DArray(Board.ConvertTo2DArray());
            var playerAtHand = GetPlayerAtHand();
            var playedWordTiles = move.TilesUsed;
            var playedExtraWords = move.ExtraWordsPlayed;
            var rackTilesUsed = move.RackTilesUsedCoordinates;
            List<BoardTile> playedWordBoardTiles = new List<BoardTile>();
            var playedWord = "";
            for (int i = 0; i < playedWordTiles.Count; i++)
            {
                var boardTileFromMove = playedWordTiles.Keys.ElementAt(i);
                var charTileFromMove = playedWordTiles.Values.ElementAt(i);
                var boardTileOnBoard = Board.BoardTiles.Where(t => t.BoardLocationX == boardTileFromMove.BoardLocationX && t.BoardLocationY == boardTileFromMove.BoardLocationY).FirstOrDefault();
                if (boardTileOnBoard.CharTile == null)
                {
                    boardTileOnBoard.CharTile = charTileFromMove;
                }
                playedWord += boardTileFromMove.CharTile.Letter;
                playedWordBoardTiles.Add(boardTileFromMove);
                boardTileOnBoard.CharTile = charTileFromMove;
            }
            playerAtHand.Moves.Add(new Move { PlayerID = playerAtHand.ID, GameID = ID, Word = playedWord, Score = Helper.GetWordScore(playedWordBoardTiles) });

            List<string> listOfWords = new List<string>();
            foreach(var extraWord in playedExtraWords)
            {
                var word = "";
                foreach (var boardTile in extraWord)
                {
                    word += boardTile.CharTile.Letter;
                }
                playerAtHand.Moves.Add(new Move { PlayerID = playerAtHand.ID, GameID = ID, Word = word, Score = Helper.GetWordScore(extraWord) });
            }
            AddScoreToPlayer(playerAtHand, move.Score);

            foreach (var rackTileUsed in rackTilesUsed)
            {
                playerAtHand.Rack.SubstractFromRack(board[rackTileUsed[0], rackTileUsed[1]].CharTile);
                playerAtHand.Rack.RefillRackFromPouch();
            }
            playerAtHand.Rack.RefillRackFromPouch();
        }
    }
}