﻿using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
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
                //continue;
            }
            if (data == null)
            {
                //return this.Json(new { success = false, message = "Uuups, something went wrong!" });
            }
            else
            {
                var playedWords = Helpers.Helper.GetPlayedWords(data);
                var playedRackTiles = Helpers.Helper.GetPlayedRackTiles(data);
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

                var result = Helpers.Helper.GetWordScores(game, data);
                if (result.StatusCode != 200)
                {
                    return StatusCode(result.StatusCode, result.StatusDescription);
                }
                game.SwitchToNextPlayer();
                _scrabbleContext.SaveChanges();
            }
            return View(game);
        }

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
                //continue;
            }
            if (data == null)
            {
                //return this.Json(new { success = false, message = "Uuups, something went wrong!" });
            }
            else
            {
                TempData["FlipBoard"] = Helper.GetValueFromAjaxData(data, "flipBoard");
            }
            return RedirectToAction("Index");
        }

        public void ResetBoard()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            foreach (BoardTile t in game.Board.BoardTiles)
            {
                t.CharTileID = null;
            }
            game.Log = "Enjoy the game!";
            foreach (Player p in game.Players)
            {
                p.Score = 0;
            }
            _scrabbleContext.SaveChanges();
        }

        public IActionResult Redraw()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            var currentPlayer = game.GetPlayerAtHand();
            var rack = currentPlayer.Rack.Rack_CharTiles.ToList();
            var tilesToDraw = 0;
            for (int i = 0; i < rack.Count; i++)
            {
                for (int j = 0; j < rack[i].Count; j++)
                {
                    currentPlayer.Rack.Pouch.AddToPouch(rack[i].CharTile);
                    tilesToDraw++;
                }
            }
            rack.Clear();
            currentPlayer.Rack.Rack_CharTiles = rack;
            currentPlayer.Rack.RefillRackFromPouch();
            game.SwitchToNextPlayer();
            _scrabbleContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Skip()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            game.SwitchToNextPlayer();
            return RedirectToAction("Index");
        }

        public void MakeEnglishDictionary()
        {
            Helpers.Helper.MakeEnglishDictionary();
        }

        public IActionResult GetMoves()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            List<KeyValuePair<string, StringValues>> data = null;
            try
            {
                data = Request.Form.ToList();
            }
            catch (Exception e)
            {
                //continue;
            }
            if (data == null)
            {
                return this.Json(new { success = false, message = "Uuups, something went wrong!" });
            }
            else
            {
                var boardArray = game.Board.ConvertTo2DArray();
                var transposedBoardArray = game.Board.Transpose2DArray(boardArray);
                Dictionary<BoardTile, List<CharTile>> validUntransposedCrossChecks = Helpers.Helper.GetValidCrossChecksOneWay(boardArray, game.WordDictionary);
                Dictionary<BoardTile, List<CharTile>> validTransposedCrossChecks = Helpers.Helper.GetValidCrossChecksOneWay(transposedBoardArray, game.WordDictionary);
                var validCrossChecks = Helpers.Helper.GetValidCrossChecksCombined(validUntransposedCrossChecks, validTransposedCrossChecks);
                var listOfValidAnchorCoordinatesOnUntransposedBoard = game.Board.GetAnchors(boardArray);
                var listOfValidAnchorCoordinatesOnTransposedBoard = game.Board.GetAnchors(transposedBoardArray);
                MoveGenerator moveValidator = new MoveGenerator(game, boardArray, transposedBoardArray, Helper.LoadDawg(game.GameLanguage), listOfValidAnchorCoordinatesOnUntransposedBoard,
                    listOfValidAnchorCoordinatesOnTransposedBoard, validCrossChecks, _scrabbleContext.WordDictionaries.Where(d => d.GameLanguageID == game.GameLanguageID).FirstOrDefault(),
                    _scrabbleContext.Moves.Where(m => m.GameID == game.ID).ToList());
                var validUntransposedMovesList = moveValidator.GetValidMoves(true);
                var validTransposedMovesList = moveValidator.GetValidMoves(false);
                var allValidMoves = validUntransposedMovesList.Concat(validTransposedMovesList).ToList();
                var allValidMovesSorted = allValidMoves.OrderByDescending(m => m.Score);
                List<Dictionary<string, string>> allValidMovesJson = new List<Dictionary<string, string>>();
                foreach (var move in allValidMovesSorted)
                {
                    var entry = new Dictionary<string, string>();
                    entry.Add("Word", move.Word);
                    entry.Add("Direction", move.IsHorizontal ? "Horizontal" : "Vertical");
                    entry.Add("Start", move.StartIndex.ToString());
                    entry.Add("End", move.StartIndex == move.EndIndex ? move.SecondaryIndex.ToString() : move.EndIndex.ToString());
                    entry.Add("Secondary", move.StartIndex == move.EndIndex ? move.EndIndex.ToString() : move.SecondaryIndex.ToString());
                    entry.Add("Score", move.Score.ToString());
                    allValidMovesJson.Add(entry);
                }
                return Json(allValidMovesJson);
                //return StatusCode(200, "hi");
            }
            return StatusCode(400, "error");
        }

        // 
        // GET: /Scrabble/Welcome/ 

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }
    }
}