using Scrabble.Classes;
using Scrabble.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Scrabble.Models
{
    /// <summary>
    /// Represents game object
    /// Has knowledge of most objects used in the game
    /// </summary>
    public class Game
    {
        public int ID { get; set; }

        public LanguageEnum GameLanguageID { get; set; }
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

        /// <summary>
        /// Gets the players who is waited on to make a play
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Switches to next player if a move was done by the previous at hand
        /// </summary>
        public void SwitchToNextPlayer()
        {
            //Updating hte player at hand
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
            //If game is finished and it is AI's turn, it passes the game on until a human player receives control
            //An ended game can be checked out by the human player on the main page
            if (IsFinished && !newPlayerAtHand.IsHuman)
            {
                SwitchToNextPlayer();
            }
            else if (!newPlayerAtHand.IsHuman)
            {
                //Creates move generator and gets possible horizontal and vertical moves
                //Stops as soon as it can and is past the timer of 1 second
                var moveGenerator = Helper.GetMoveGenerator(this, Moves.Where(m => m.GameID == ID).ToList(), 2);
                var validUntransposedMovesList = moveGenerator.GetValidMoves(true);
                var validTransposedMovesList = moveGenerator.GetValidMoves(false);
                //HashSet<GeneratedMove> validTransposedMovesList = new HashSet<GeneratedMove>();
                var allValidMoves = validUntransposedMovesList.Concat(validTransposedMovesList).ToList();

                //High Scorer sorts by best score, Rack balancer sorts by best rack score.
                var allValidMovesSorted = newPlayerAtHand.BotTypeID == BotTypeEnum.High_Scorer ? allValidMoves.OrderByDescending(m => m.Score).ToList()
                    : allValidMoves.OrderBy(m => m.RackScore).ToList();

                //Skips or redraws if no move is possible.
                if (allValidMovesSorted.Count > 0) MakeGeneratedMove(allValidMovesSorted[0]);
                else
                {
                    newPlayerAtHand.SkipsOrRedrawsUsed++;
                    if (newPlayerAtHand.Pouch.Pouch_CharTiles.Count > 0) newPlayerAtHand.Redraw();
                }

                //Finishes game if pouch is empty and AI has played its entire rack and passes play to human
                if (newPlayerAtHand.Rack.Rack_CharTiles.Count == 0)
                {
                    FinalizeResults(newPlayerAtHand);
                }
                SwitchToNextPlayer();
            }
            Players = playersList;
        }

        //Performs the actions detailed in a generated move from the move generator.
        public void MakeGeneratedMove(GeneratedMove move)
        {
            var board = move.IsHorizontal ? Board.ConvertTo2DArray() : Board.Transpose2DArray(Board.ConvertTo2DArray());
            var playerAtHand = GetPlayerAtHand();
            var usedBoardTilesForMainWord = move.TilesUsed;
            var usedBoardTilesForExtraMoves = move.ExtraWordsPlayed;
            var RackTilesUsedCoordinates = move.RackTilesUsedCoordinates;
            List<BoardTile> playedWordBoardTiles = new List<BoardTile>();
            var playedWord = "";
            int moveNumber = playerAtHand.Moves.Count == 0 ? 1 : (playerAtHand.Moves.OrderByDescending(m => m.MoveNumber).FirstOrDefault().MoveNumber) + 1;

            //Places CharTiles on the board tiles from the generated move.
            for (int i = 0; i < usedBoardTilesForMainWord.Count; i++)
            {
                var boardTileFromMove = usedBoardTilesForMainWord.Keys.ElementAt(i);
                var charTileFromMove = usedBoardTilesForMainWord.Values.ElementAt(i);
                var boardTileOnBoard = Board.BoardTiles.Where(t => t.BoardLocationX == boardTileFromMove.BoardLocationX && t.BoardLocationY == boardTileFromMove.BoardLocationY).FirstOrDefault();
                if (boardTileOnBoard.CharTile == null)
                {
                    boardTileOnBoard.CharTile = charTileFromMove;
                }
                playedWord += boardTileFromMove.CharTile.Letter;
                playedWordBoardTiles.Add(boardTileFromMove);
                boardTileOnBoard.CharTile = charTileFromMove;
            }

            //Marks all moves as old and current ones as new
            playerAtHand.Moves = playerAtHand.Moves.Select(c => { c.IsNew = false; return c; }).ToList();

            playerAtHand.Moves.Add(new Move { PlayerID = playerAtHand.ID, GameID = ID, Word = playedWord, Score = Helper.GetWordScore(playedWordBoardTiles),
                Start = move.StartIndex, End = move.EndIndex, Index = move.Anchor[0], IsHorizontal = move.IsHorizontal, IsNew = true, MoveNumber = moveNumber
            });

            List<string> listOfWords = new List<string>();

            //Goes through and adds each extra word
            foreach(var extraWord in usedBoardTilesForExtraMoves)
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
            AddScoreToPlayer(playerAtHand, (move.Score + (RackTilesUsedCoordinates.Count == playerAtHand.Rack.RackSize ? 50 : 0)));

            //Marks now filled tiles as filled (to remove premium square bonus)
            foreach (var entry in move.TilesUsed)
            {
                entry.Key.IsFilled = true;
            }

            //Removes char tiles used from the rack and refills the rack from the pouch
            foreach (var rackTileUsed in RackTilesUsedCoordinates)
            {
                playerAtHand.Rack.SubstractFromRack(board[rackTileUsed[0], rackTileUsed[1]].CharTile);
                playerAtHand.Rack.RefillRackFromPouch();
            }

            //If nothing in pouch is left, game is finished
            playerAtHand.Rack.RefillRackFromPouch();
            if (playerAtHand.Rack.Rack_CharTiles.Count == 0) IsFinished = true;

        }

        /// <summary>
        /// Ends game and penalizes all players for their unplayed tiles.
        /// </summary>
        public void FinalizeResults()
        {
            IsFinished = true;
            foreach (var player in Players)
            {
                foreach (var tile in player.Rack.Rack_CharTiles)
                {
                    var penalty = tile.CharTile.Score * tile.Count;
                    player.Score -= penalty;
                }
            }
        }

        /// <summary>
        /// Ends game and penalizes all players that have tiles in their racks and adds their score to
        /// the player that played all of their own rack tiles.
        /// </summary>
        /// <param name="playerWithFinalPlay"></param>
        public void FinalizeResults (Player playerWithFinalPlay)
        {
            IsFinished = true;
            var bonusScore = 0;
            foreach (var player in Players)
            {
                if (player == playerWithFinalPlay) continue;
                foreach (var tile in player.Rack.Rack_CharTiles)
                {
                    var penalty = tile.CharTile.Score * tile.Count;
                    player.Score -= penalty;
                    bonusScore += penalty;
                }
            }
            playerWithFinalPlay.Score += bonusScore;
        }
    }
}