using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scrabble.Models;

namespace Scrabble.Controllers
{
    public class HomeController : Controller
    {
        ScrabbleContext _scrabbleContext;

        public HomeController(ScrabbleContext context)
        {
            _scrabbleContext = context;
        }

        public IActionResult Index()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            return View();
        }

        public IActionResult About()
        {
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            ViewData["Message"] = "About message.";
            //game.
            //game.Board = _scrabbleContext.Boards.Single(b => b.ID == game.BoardID);
            //game.Board.BoardTiles = _scrabbleContext.BoardTiles.Where(b => b.BoardID == game.BoardID).ToList();
            //game.Pouch = _scrabbleContext.Pouchs.Single(p => p.ID == game.PouchID);
            //game.Racks = _scrabbleContext.Racks.Where(r => r.GameID == game.ID).ToList();
            //game.Players = _scrabbleContext.Players.Where(p => p.GameID == game.ID).ToList();
            //List<Pouch_CharTile> listOfPouchCharTiles = _scrabbleContext.Pouch_CharTiles.Where(p => p.GameID == game.ID).ToList();
            //List<Rack_CharTile> listOfRackCharTiles = _scrabbleContext.Rack_CharTiles.Where(r => r.Pouch_CharTileID == game.ID).ToList();
            //List<CharTile> pouch = new List<CharTile>();
            /*foreach (Pouch_CharTile c in listOfPouchCharTiles)
            {
                for (int i = 0; i < c.Count; i++)
                {
                    CharTile charTileToAdd = _scrabbleContext.CharTiles.Single(charTile => charTile.ID == c.ID);
                    charTileToAdd.GameLanguage = _scrabbleContext.GameLanguages.Single(l => l.ID == charTileToAdd.GameLanguageID);
                    pouch.Add(charTileToAdd);
                }
            }*/
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
