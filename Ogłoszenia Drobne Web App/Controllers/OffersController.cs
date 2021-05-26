using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ogłoszenia_Drobne_Web_App.Data;
using Ogłoszenia_Drobne_Web_App.Models;
using Ogłoszenia_Drobne_Web_App.Pagination;

namespace Ogłoszenia_Drobne_Web_App.Controllers
{
    [Authorize]
    public class OffersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OffersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Offers
        public async Task<IActionResult> Index(string? searchString, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            var offers = from s in _context.Offers
                         select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                offers = _context.Offers.Where(s => s.Title.ToLower().Contains(searchString) ||
                s.Description.ToLower().Contains(searchString) || searchString.Contains(s.Category.CategoryName.ToLower()));
            }
            var page = HttpContext.Request.Cookies["page"];
            int pageSize = 5;
            if (page != null)
            {
                pageSize = Int32.Parse(page);
            }

            return View(await PaginatedList<Offer>.CreateAsync(offers.Include(o => o.Category).Include(o => o.User).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Offers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Category)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // GET: Offers/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CategoryId,Title,Description,Wage")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                string email = User.Identity.Name;
                AppUser user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null) return null;
                offer.UserId = user.Id;
                offer.ExpirationDate = DateTime.Now.AddDays(14);
                offer.LastModificationDate = DateTime.Now;
                offer.CreationDate = DateTime.Now;
                offer.ViewCounter = 0; // Marcin

                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyOffers));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", offer.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", offer.UserId);
            return View(offer);
        }

        // GET: Offers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            string email = User.Identity.Name;
            AppUser user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return Unauthorized();
            if (offer.UserId != user.Id) return Unauthorized();
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", offer.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", offer.UserId);
            return View(offer);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Title,Description,Wage")] Offer offer)
        {
            string email = User.Identity.Name;
            AppUser user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return NotFound();

            if (id != offer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (offer.UserId == user.Id)
                    {
                        offer.LastModificationDate = DateTime.Now;
                        _context.Update(offer);
                        await _context.SaveChangesAsync();
                    }
                    else
                        return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", offer.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", offer.UserId);
            return View(offer);
        }

        // GET: Offers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Category)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }
            string email = User.Identity.Name;
            AppUser user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return Unauthorized();
            if (offer.UserId != user.Id) return Unauthorized();
            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string email = User.Identity.Name;
            AppUser user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return NotFound();

            var offer = await _context.Offers.FindAsync(id);
            if (offer.UserId == user.Id)
            {
                _context.Offers.Remove(offer);
                await _context.SaveChangesAsync();
            }
            else
                return Unauthorized();

            return RedirectToAction(nameof(MyOffers));
        }

        private bool OfferExists(int id)
        {
            return _context.Offers.Any(e => e.Id == id);
        }

        public async Task<IActionResult> MyOffers()
        {
            string email = User.Identity.Name;
            AppUser user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return NotFound();
            var offers = await _context.Offers.Where(o => o.User.Id == user.Id).ToListAsync();
            return View(offers);
        }
    }
}