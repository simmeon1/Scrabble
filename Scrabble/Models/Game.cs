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

        public virtual ICollection<Player> Players { get; set; }

        public int BoardID { get; set; }
        public virtual Board Board { get; set; }

        public int PouchID { get; set; }
        public virtual Pouch Pouch { get; set; }

        public int WordDictionaryID { get; set; }
        [ForeignKey("WordDictionaryID")]
        public virtual WordDictionary WordDictionary { get; set; }

        public string Log { get; set; }

        public virtual ICollection<Rack> Racks { get; set; }
        public virtual ICollection<Move> Moves { get; set; }

        public bool IsFinished { get; set; }

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
            if (IsFinished && !newPlayerAtHand.IsHuman)
            {
                SwitchToNextPlayer();
            }
            else if (!newPlayerAtHand.IsHuman)
            {
                var moveGenerator = Helper.GetMoveGenerator(this, Moves.Where(m => m.GameID == ID).ToList(), 1);
                var validUntransposedMovesList = moveGenerator.GetValidMoves(true);
                var validTransposedMovesList = moveGenerator.GetValidMoves(false);
                //HashSet<GeneratedMove> validTransposedMovesList = new HashSet<GeneratedMove>();
                var allValidMoves = validUntransposedMovesList.Concat(validTransposedMovesList).ToList();
                var allValidMovesSorted = allValidMoves.OrderByDescending(m => m.Score).ToList();
                if (allValidMovesSorted.Count > 0) MakeGeneratedMove(allValidMovesSorted[0]);
                else
                {
                    newPlayerAtHand.SkipsOrRedrawsUsed++;
                    if (newPlayerAtHand.Pouch.Pouch_CharTiles.Count > 0) newPlayerAtHand.Redraw();
                }
                if (newPlayerAtHand.Rack.Rack_CharTiles.Count == 0) IsFinished = true;
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
            int moveNumber = playerAtHand.Moves.Count == 0 ? 1 : (playerAtHand.Moves.OrderByDescending(m => m.MoveNumber).FirstOrDefault().MoveNumber) + 1;
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
            playerAtHand.Moves = playerAtHand.Moves.Select(c => { c.IsNew = false; return c; }).ToList();

            playerAtHand.Moves.Add(new Move { PlayerID = playerAtHand.ID, GameID = ID, Word = playedWord, Score = Helper.GetWordScore(playedWordBoardTiles),
                Start = move.StartIndex, End = move.EndIndex, Index = move.Anchor[0], IsHorizontal = move.IsHorizontal, IsNew = true, MoveNumber = moveNumber
            });

            List<string> listOfWords = new List<string>();
            foreach(var extraWord in playedExtraWords)
            {
                var word = "";
                foreach (var boardTile in extraWord)
                {
                    word += boardTile.CharTile.Letter;
                }
                
                var isHorizontal = extraWord[0].BoardLocationX == extraWord[1].BoardLocationX ? true : false;
                var start = isHorizontal ? extraWord[0].BoardLocationY : extraWord[0].BoardLocationX;
                var end = isHorizontal ? extraWord[extraWord.Count - 1].BoardLocationY : extraWord[extraWord.Count - 1].BoardLocationX;
                var index = isHorizontal ? extraWord[0].BoardLocationX : extraWord[0].BoardLocationY;

                playerAtHand.Moves.Add(new Move
                {
                    PlayerID = playerAtHand.ID,
                    GameID = ID,
                    Word = word,
                    Score = Helper.GetWordScore(extraWord),
                    IsHorizontal = isHorizontal,
                    Start = start,
                    End = end,
                    Index = index,
                    IsNew = true,
                    MoveNumber = moveNumber
                });
            }
            AddScoreToPlayer(playerAtHand, (move.Score + (rackTilesUsed.Count == playerAtHand.Rack.RackSize ? 50 : 0)));

            foreach (var rackTileUsed in rackTilesUsed)
            {
                playerAtHand.Rack.SubstractFromRack(board[rackTileUsed[0], rackTileUsed[1]].CharTile);
                playerAtHand.Rack.RefillRackFromPouch();
            }
            playerAtHand.Rack.RefillRackFromPouch();
            if (playerAtHand.Rack.Rack_CharTiles.Count == 0) IsFinished = true;

        }
    }
}