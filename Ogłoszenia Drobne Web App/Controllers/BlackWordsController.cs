using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ogłoszenia_Drobne_Web_App.Data;
using Ogłoszenia_Drobne_Web_App.Models;

namespace Ogłoszenia_Drobne_Web_App.Controllers
{
    public class BlackWordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlackWordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BlackWords
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlackWords.ToListAsync());
        }

        // GET: BlackWords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blackWord = await _context.BlackWords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blackWord == null)
            {
                return NotFound();
            }

            return View(blackWord);
        }

        // GET: BlackWords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlackWords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Word")] BlackWord blackWord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blackWord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blackWord);
        }

        // GET: BlackWords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blackWord = await _context.BlackWords.FindAsync(id);
            if (blackWord == null)
            {
                return NotFound();
            }
            return View(blackWord);
        }

        // POST: BlackWords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Word")] BlackWord blackWord)
        {
            if (id != blackWord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blackWord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlackWordExists(blackWord.Id))
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
            return View(blackWord);
        }

        // GET: BlackWords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blackWord = await _context.BlackWords
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blackWord == null)
            {
                return NotFound();
            }

            return View(blackWord);
        }

        // POST: BlackWords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blackWord = await _context.BlackWords.FindAsync(id);
            _context.BlackWords.Remove(blackWord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlackWordExists(int id)
        {
            return _context.BlackWords.Any(e => e.Id == id);
        }
    }
}
