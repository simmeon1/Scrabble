using Scrabble.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Scrabble.Models
{
    public class Game
    {
        public int ID { get; set; }

        public int GameLanguageID { get; set; }
        public virtual GameLanguage GameLanguage { get; set; }

        public virtual ICollection<Player> Players { get; set; }

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


        public virtual ICollection<Rack> Racks { get; set; }
        public virtual ICollection<Move> Moves { get; set; }

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
            var rackTilesUsed = move.RackTilesUsed;
            List<BoardTile> playedWordBoardTiles = new List<BoardTile>(playedWordTiles.Keys);
            List<BoardTile> rackTilesPlayed = new List<BoardTile>();
            var playedWord = new StringBuilder();
            Board.PlayTiles(playedWordTiles);
            //for (int i = 0; i < playedWordTiles.Count; i++)
            //{
            //    var boardTileFromMove = playedWordTiles.Keys.ElementAt(i);
            //    var charTileFromMove = playedWordTiles.Values.ElementAt(i);
            //    for (int x = 0; x < board.GetLength(0); x++)
            //    {
            //        for (int y = 0; y < board.GetLength(1); y++)
            //        {
            //            if (boardTileFromMove == board[x, y])
            //            {
            //                Board.PlayTile(x, y, charTileFromMove.ID, playedWordBoardTiles, rackTilesPlayed, null, board);
            //                playedWord.Append(boardTileFromMove.CharTile.Letter);
            //                break;
            //            }
            //        }
            //    }
            //}
            playerAtHand.Moves.Add(new Move { PlayerID = playerAtHand.ID, GameID = ID, Word = playedWord.ToString(), Score = Helper.GetWordScore(playedWordBoardTiles) });

            foreach (var extraWord in playedExtraWords)
            {
                var word = new StringBuilder();
                foreach (var boardTile in extraWord)
                {
                    word.Append(boardTile.CharTile.Letter);
                }
                playerAtHand.Moves.Add(new Move { PlayerID = playerAtHand.ID, GameID = ID, Word = word.ToString(), Score = Helper.GetWordScore(extraWord) });
            }
            AddScoreToPlayer(playerAtHand, move.Score);
            //var allWordsPlayed = 
            var crossChecksOfBoardTilesToUpdateFirstAxis = Board.GetCrossCheckBoardTiles(playedWordBoardTiles, false);
            Helper.UpdateCrossChecks(crossChecksOfBoardTilesToUpdateFirstAxis, false, WordDictionary, GameLanguage);
            var crossChecksOfBoardTilesToUpdateSecondAxis = Board.GetCrossCheckBoardTiles(playedWordBoardTiles, true);
            Helper.UpdateCrossChecks(crossChecksOfBoardTilesToUpdateSecondAxis, true, WordDictionary, GameLanguage);
            //foreach (var playedExtraWord in playedExtraWords)
            //{
            //    var crossChecksOfBoardTilesToUpdateFirstAxis = Board.GetCrossCheckBoardTiles(playedExtraWord, false);
            //    Helper.UpdateCrossChecks(crossChecksOfBoardTilesToUpdateFirstAxis, false, WordDictionary, GameLanguage);
            //    var crossChecksOfBoardTilesToUpdateSecondAxis = Board.GetCrossCheckBoardTiles(playedExtraWord, true);
            //    Helper.UpdateCrossChecks(crossChecksOfBoardTilesToUpdateSecondAxis, true, WordDictionary, GameLanguage);
            //}

            foreach (var rackTileUsed in rackTilesUsed)
            {
                playerAtHand.Rack.SubstractFromRack(rackTileUsed.CharTile);
                playerAtHand.Rack.RefillRackFromPouch();
            }
            playerAtHand.Rack.RefillRackFromPouch();
        }
    }
}