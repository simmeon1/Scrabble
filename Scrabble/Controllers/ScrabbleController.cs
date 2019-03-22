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
            try {
                data = Request.Form.ToList();
            } catch (Exception e)
            {
                //continue;
            }          
            if (data != null)
            {
                var playedTiles = data[0].Value.ToString().Split(",");
                foreach (string playedTile in playedTiles)
                {
                    var tileDetails = playedTile.Split("_");
                    int tileX = Int32.Parse(tileDetails[0]);
                    int tileY = Int32.Parse(tileDetails[1]);
                    int tileCharTile = Int32.Parse(tileDetails[2]);
                    game.Board.PlayTile(tileX, tileY, tileCharTile);
                }
                _scrabbleContext.SaveChanges();
                //ScrabbleContext context = new ScrabbleContext();
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