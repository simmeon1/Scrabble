using Microsoft.AspNetCore.Mvc;
using Scrabble.Models;
using System.Collections.Generic;
using System.Text.Encodings.Web;

namespace Scrabble.Controllers
{
    public class ScrabbleController : Controller
    {
        // 
        // GET: /Scrabble/

        public IActionResult Index()
        {
            Game game = new Game(GameLanguages.Language.English, 7, 15, 15);
            game.Board.BoardTiles[0][10] = new BoardTile('C', 3);
            game.Board.BoardTiles[1][10] = new BoardTile('A', 1);
            game.Board.BoardTiles[2][10] = new BoardTile('S', 1);
            game.Board.BoardTiles[3][10] = new BoardTile('H', 4);
            game.AddPlayer("Simeon", true);
            game.AddPlayer("Dob", true);
            foreach (Player p in game.PlayerData)
            {
                p.DrawTilesFromPouch();
            }
            return View(game);
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