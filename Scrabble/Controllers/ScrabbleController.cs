using Microsoft.AspNetCore.Mvc;
using Scrabble.Models;
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
            /*Game game = new Game(Language.English, 7, 15, 15);
            game.Board.BoardTiles[10] = (new BoardTile(0, 10, new CharTile('C',3)));
            game.Board.BoardTiles[25] = (new BoardTile(1, 10, new CharTile('A', 1)));
            game.Board.BoardTiles[40] = (new BoardTile(2, 10, new CharTile('S', 1)));
            game.Board.BoardTiles[55] = (new BoardTile(3, 10, new CharTile('H', 4)));*/
            //game.AddPlayer("Simeon", true);
            //game.AddPlayer("Dob", true);
            /*foreach (Player p in game.Players)
            {
                p.DrawTilesFromPouch();
            }*/
            //Game game = new Game();
            Game game = _scrabbleContext.Games.Single(g => g.ID == 1);
            //ScrabbleContext context = new ScrabbleContext();
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