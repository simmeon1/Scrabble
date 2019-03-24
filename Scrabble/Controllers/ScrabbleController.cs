using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Scrabble.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

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
                List<string> playedWords = new List<string>();
                foreach (KeyValuePair<string, StringValues> wordData in data)
                {
                    playedWords.Add(wordData.Value);
                }
                //var playedWords = data[0].Value.ToString().Split(",");              
                foreach (string playedWord in playedWords)
                {
                    var currentScoreOfMove = 0;
                    var doubleWordTilesUsed = 0;
                    var tripleWordTilesUsed = 0;
                    var usedBoardTiles = new List<BoardTile>();
                    var playedWordString = "";
                    var playedTiles = playedWord.Split(",");
                    foreach (string playedTile in playedTiles)
                    {
                        var tileDetails = playedTile.Split("_");
                        int tileX = Int32.Parse(tileDetails[0]);
                        int tileY = Int32.Parse(tileDetails[1]);
                        int tileCharTileId = Int32.Parse(tileDetails[2]);
                        playedWordString += game.WordDictionary.CharTiles.Where(i => i.ID == tileCharTileId).FirstOrDefault().Letter;
                        game.Board.PlayTile(tileX, tileY, tileCharTileId, usedBoardTiles);
                        //currentScoreOfMove += game.WordDictionary.CharTiles.Where(i => i.ID == tileCharTileId).FirstOrDefault().Score;
                    }

                    foreach (BoardTile b in usedBoardTiles)
                    {

                        switch (b.BoardTileType.Type)
                        {
                            case "DoubleLetter":
                                currentScoreOfMove += b.CharTile.Score * 2;
                                break;
                            case "TripleLetter":
                                currentScoreOfMove += b.CharTile.Score * 3;
                                break;
                            case "DoubleWord":
                                doubleWordTilesUsed += 1;
                                currentScoreOfMove += b.CharTile.Score;
                                break;
                            case "TripleWord":
                                tripleWordTilesUsed += 1;
                                currentScoreOfMove += b.CharTile.Score;
                                break;
                            default:
                                currentScoreOfMove += b.CharTile.Score;
                                break;
                        }
                    }
                    for (int i = 0; i < doubleWordTilesUsed; i++)
                    {
                        currentScoreOfMove *= 2;
                    }
                    for (int i = 0; i < tripleWordTilesUsed; i++)
                    {
                        currentScoreOfMove *= 3;
                    }
                    playedWordString = playedWordString.ToUpper();
                    game.Log += "\nPlayer played " + playedWordString + " for " + currentScoreOfMove + " points.";                   
                    //ScrabbleContext context = new ScrabbleContext();
                }
                _scrabbleContext.SaveChanges();
            }
            return View(game);
        }

        public void ResetBoard()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            foreach (BoardTile t in game.Board.BoardTiles)
            {
                t.CharTileID = null;
            }
            game.Log = "Enjoy the game!";
            _scrabbleContext.SaveChanges();
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