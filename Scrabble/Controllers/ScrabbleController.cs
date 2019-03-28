using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Scrabble.Classes;
using Scrabble.Helpers;
using Scrabble.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            TempData["FlipBoard"] = true;
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
                //return this.Json(new { success = false, message = "Uuups, something went wrong!" });
            } else
            {
                var boardArray = game.Board.ConvertTo2DArray();
                var transposedBoardArray = game.Board.Transpose2DArray(boardArray);
                List<int[]> listOfValidAnchorCoordinatesOnHorizontalBoard = new List<int[]>();
                List<int[]> listOfValidAnchorCoordinatesOnVerticalBoard = new List<int[]>();
                Dictionary<BoardTile, List<CharTile>> validHorizontalCrossChecks = Helpers.Helper.GetValidCrossChecksOneWay(boardArray, game.WordDictionary, false);
                Dictionary<BoardTile, List<CharTile>> validVerticalCrossChecks = Helpers.Helper.GetValidCrossChecksOneWay(transposedBoardArray, game.WordDictionary, true);
                Helpers.Helper.GetValidCrossChecksCombined(validHorizontalCrossChecks, validVerticalCrossChecks);               
                game.Board.GetAnchors(boardArray, listOfValidAnchorCoordinatesOnHorizontalBoard);
                game.Board.GetAnchors(transposedBoardArray, listOfValidAnchorCoordinatesOnVerticalBoard);
                MoveGenerator moveValidator = new MoveGenerator(game, boardArray, transposedBoardArray, Helper.LoadDawg(game.GameLanguage), listOfValidAnchorCoordinatesOnHorizontalBoard, listOfValidAnchorCoordinatesOnVerticalBoard, validHorizontalCrossChecks, validVerticalCrossChecks);
                Dictionary<int[], string> validHorizontalMovesList = new Dictionary<int[], string>();
                Dictionary<int[], string> validVerticalMovesList = new Dictionary<int[], string>();
                validHorizontalMovesList = moveValidator.GetValidMoves(true);
                validVerticalMovesList = moveValidator.GetValidMoves(false);
                //Dictionary<int[], string> validMoves = Helpers.Helper.GetValidMoves(game, boardArray, transposedBoardArray, listOfValidAnchorCoordinates, validCrossChecks);
                return StatusCode(200, "hi");
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