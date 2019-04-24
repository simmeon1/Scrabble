using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Scrabble.Classes;
using Scrabble.Helpers;
using Scrabble.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Helpers;

namespace Scrabble.Controllers
{
    public class ScrabbleController : Controller
    {
        // 
        // GET: /Scrabble/

        ScrabbleContext _scrabbleContext;

        public ScrabbleController(ScrabbleContext context)
        {
            _scrabbleContext = context;
        }

        /// <summary>
        /// Home page. If data is provided, it means user has played words and pressed Submit.
        /// In that case, returns an updated game object. Otherwise returns a currently existing one.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            foreach (var p in game.Players)
            {
                p.Rack.RefillRackFromPouch();
            }
            if (TempData["FlipBoard"] != null)
            {
                ViewBag.FlipBoard = TempData["FlipBoard"].ToString();
            }
            List<KeyValuePair<string, StringValues>> data = null;
            try
            {
                data = Request.Form.ToList();
            }
            catch (Exception e)
            {
                //return StatusCode(400, e.Message);
            }
            if (data == null)
            {
                //return StatusCode(400, "No data entered.");
            }
            else
            {
                var playedWords = Helper.GetPlayedWords(data);
                var playedRackTiles = Helper.GetPlayedRackTiles(data);
                var boardArray = game.Board.ConvertTo2DArray();
                if (playedRackTiles.Length == 0)
                {
                    return StatusCode(400, "You have not played a tile.");
                }
                //if (!game.Board.CheckIfAnchorIsUsed(playedRackTiles, boardArray))
                //{
                //    return StatusCode(400, "Anchor is not used.");
                //}
                //if (!Helpers.Helper.ValidateStart(boardArray, playedRackTiles))
                //{
                //    return StatusCode(400, "Invalid start.");
                //}
                //if (!Helpers.Helper.IsRackPlayConnected(boardArray, playedRackTiles))
                //{
                //    return StatusCode(400, "Play is not connected.");
                //}

                var result = Helper.GetWordScores(game, data);
                if (result.StatusCode != 200)
                {
                    return StatusCode(result.StatusCode, result.StatusDescription);
                }
                game.SwitchToNextPlayer();
            }
            _scrabbleContext.SaveChanges();
            return View(game);
        }

        /// <summary>
        /// Transposes board
        /// Used mainly for testing.
        /// </summary>
        /// <returns></returns>
        public IActionResult FlipBoard()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            List<KeyValuePair<string, StringValues>> data = null;
            try
            {
                data = Request.Form.ToList();
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
            if (data == null)
            {
                //return StatusCode(400, "Something went wrong.");
            }
            else
            {
                TempData["FlipBoard"] = Helper.GetValueFromAjaxData(data, "flipBoard");
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Resets all relevant game information.
        /// </summary>
        /// <returns></returns>
        public IActionResult ResetGame()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            foreach (BoardTile t in game.Board.BoardTiles)
            {
                if (t.CharTile != null) game.Pouch.AddToPouch(t.CharTile);
                t.CharTileID = null;
                t.CharTile = null;
                t.IsFilled = false;
            }
            foreach (var p in game.Players)
            {
                p.Moves.Clear();
                p.Score = 0;
                foreach(var tile in p.Rack.Rack_CharTiles)
                {
                    for (int i = 0; i < tile.Count; i++)
                    {
                        game.Pouch.AddToPouch(tile.CharTile);
                    }
                }
                p.AtHand = false;
                p.Rack.Rack_CharTiles.Clear();
                p.Rack.RefillRackFromPouch();
                p.SkipsOrRedrawsUsed = 0;
            }
            game.Players.ElementAt(0).AtHand = true;
            game.IsFinished = false;
            game.Moves.Clear();
            _scrabbleContext.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Trades rack tiles for new ones. If no data is provided, all are traded
        /// Otherwise the sepcified ones are traded.
        /// </summary>
        /// <returns></returns>
        public IActionResult Redraw()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            var currentPlayer = game.GetPlayerAtHand();
            currentPlayer.SkipsOrRedrawsUsed++;
            List<KeyValuePair<string, StringValues>> data = null;
            try
            {
                data = Request.Form.ToList();
            }
            catch (Exception e)
            {
                //return StatusCode(400, e.Message);
            }
            if (data == null)
            {
                currentPlayer.Redraw();
            }
            else
            {
                currentPlayer.Redraw(Helper.GetValueFromAjaxData(data, "lettersToTrade"), Helper.GetValueFromAjaxData(data, "timesToTradeLetters"));
            }
            game.SwitchToNextPlayer();
            _scrabbleContext.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Skips a turn
        /// </summary>
        /// <returns></returns>
        public IActionResult Skip()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            var currentPlayer = game.GetPlayerAtHand();
            currentPlayer.SkipsOrRedrawsUsed++;
            game.SwitchToNextPlayer();
            _scrabbleContext.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Finalizez game.
        /// </summary>
        /// <returns></returns>
        public IActionResult EndGame()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            game.FinalizeResults();
            _scrabbleContext.SaveChanges();
            return RedirectToAction("Index");
        }

        //public void MakeEnglishDictionary()
        //{
        //    Helper.MakeEnglishDictionary();
        //}

        /// <summary>
        /// Creates a Move Generator and gets as many moves as it can for the time limit given the current rack and board.
        /// </summary>
        /// <returns></returns>
        public IActionResult GetMoves()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);

            var moveGenerator = Helper.GetMoveGenerator(game, _scrabbleContext.Moves.Where(m => m.GameID == game.ID).ToList(), 2);
            var validUntransposedMovesList = moveGenerator.GetValidMoves(true);
            var validTransposedMovesList = moveGenerator.GetValidMoves(false);
            //var validTransposedMovesList = new HashSet<GeneratedMove>();
            var allValidMoves = validUntransposedMovesList.Concat(validTransposedMovesList).ToList();
            var allValidMovesSorted = allValidMoves.OrderByDescending(m => m.Score).ToList();
            List<Dictionary<string, string>> allValidMovesJson = new List<Dictionary<string, string>>();
            foreach (var move in allValidMovesSorted)
            {
                var entry = new Dictionary<string, string>();
                entry.Add("Word", move.Word);
                entry.Add("Direction", move.IsHorizontal ? "Horizontal" : "Vertical");
                entry.Add("Extra Words", move.GetExtraWordsMessage());
                entry.Add("Start", move.StartIndex.ToString());
                entry.Add("End",  move.EndIndex.ToString());
                entry.Add("Anchor", move.Anchor[0].ToString());
                entry.Add("Score", move.Score.ToString());
                allValidMovesJson.Add(entry);
            }
            return Json(allValidMovesJson);
        }
    }
}