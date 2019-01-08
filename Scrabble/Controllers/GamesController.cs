using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scrabble.Models;

namespace Scrabble.Controllers
{
    public class GamesController : Controller
    {
        private readonly ScrabbleContext _context;

        public GamesController(ScrabbleContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var scrabbleContext = _context.Game.Include(g => g.Board).Include(g => g.Pouch).Include(g => g.WordDictionary);
            return View(await scrabbleContext.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Board)
                .Include(g => g.Pouch)
                .Include(g => g.WordDictionary)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            ViewData["BoardID"] = new SelectList(_context.Set<Board>(), "ID", "ID");
            ViewData["PouchID"] = new SelectList(_context.Set<Pouch>(), "ID", "ID");
            ViewData["WordDictionaryID"] = new SelectList(_context.Set<WordDictionary>(), "ID", "ID");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GameLanguage,RackSize,WordDictionaryID,BoardID,PouchID")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoardID"] = new SelectList(_context.Set<Board>(), "ID", "ID", game.BoardID);
            ViewData["PouchID"] = new SelectList(_context.Set<Pouch>(), "ID", "ID", game.PouchID);
            ViewData["WordDictionaryID"] = new SelectList(_context.Set<WordDictionary>(), "ID", "ID", game.WordDictionaryID);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["BoardID"] = new SelectList(_context.Set<Board>(), "ID", "ID", game.BoardID);
            ViewData["PouchID"] = new SelectList(_context.Set<Pouch>(), "ID", "ID", game.PouchID);
            ViewData["WordDictionaryID"] = new SelectList(_context.Set<WordDictionary>(), "ID", "ID", game.WordDictionaryID);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,GameLanguage,RackSize,WordDictionaryID,BoardID,PouchID")] Game game)
        {
            if (id != game.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoardID"] = new SelectList(_context.Set<Board>(), "ID", "ID", game.BoardID);
            ViewData["PouchID"] = new SelectList(_context.Set<Pouch>(), "ID", "ID", game.PouchID);
            ViewData["WordDictionaryID"] = new SelectList(_context.Set<WordDictionary>(), "ID", "ID", game.WordDictionaryID);
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .Include(g => g.Board)
                .Include(g => g.Pouch)
                .Include(g => g.WordDictionary)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Game.FindAsync(id);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.ID == id);
        }
    }
}
