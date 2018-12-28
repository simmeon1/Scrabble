using Microsoft.AspNetCore.Mvc;
using Scrabble.Models;
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