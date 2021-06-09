using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ogłoszenia_Drobne_Web_App.Data;
using Ogłoszenia_Drobne_Web_App.Models;
using Ogłoszenia_Drobne_Web_App.Pagination;
using Ogłoszenia_Drobne_Web_App.ViewModels;

namespace Ogłoszenia_Drobne_Web_App.Controllers
{
    [Authorize]
    public class OffersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OffersController(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Offers
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, int? pageNumber)
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
        [AllowAnonymous]
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
                .Include(o => o.OfferAtributes)
                .ThenInclude(oa => oa.Atribute)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (offer == null)
            {
                return NotFound();
            }

            offer.ViewCounter++;
            _context.Update(offer);
            await _context.SaveChangesAsync();

            return View(offer);
        }

        // GET: Offers/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", null); 
            return View();
        }

        // POST: Offers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOfferViewModel offerViewModel)
        {
            if (ModelState.IsValid)
            {
                var blackWords = await _context.BlackWords.ToListAsync();
                foreach (var blackword in blackWords)
                {
                    if (offerViewModel.Description.ToLower().Contains(blackword.Word.ToLower()))
                    {
                        ModelState.AddModelError("description", "Opis zawiera zakazane słowa");
                        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", offerViewModel.CategoryId);

                        return View(offerViewModel);
                    }

                    if (offerViewModel.Title.ToLower().Contains(blackword.Word.ToLower())) {

                        ModelState.AddModelError("title", "Tytuł zawiera zakazane słowa");
                        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", offerViewModel.CategoryId);

                        return View(offerViewModel);
                    }
                }
                
                string email = User.Identity.Name;
                AppUser user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null) return NotFound();
                var offer = _mapper.Map<Offer>(offerViewModel);
                offer.UserId = user.Id;
                offer.ExpirationDate = DateTime.Now.AddDays(14);
                offer.LastModificationDate = DateTime.Now;
                offer.CreationDate = DateTime.Now;
                offer.ViewCounter = 0;

                foreach(var attribute in offerViewModel.Attributes)
                {
                    var offerAttribute = new OfferAtribute()
                    {
                        Value = attribute.Value,
                        AtributeId = attribute.Id,
                        Offer = offer
                    };
                    _context.Add(offerAttribute);
                }

                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyOffers));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", offerViewModel.CategoryId);
          
            return View(offerViewModel);
        }

        // GET: Offers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Offer offer = await _context.Offers
                .Include(o => o.Category)
                .Include(o => o.OfferAtributes)
                .ThenInclude(o => o.Atribute)
                .Where(o => o.Id == id).FirstOrDefaultAsync();
            
            if (offer == null)
            {
                return NotFound();
            }

            string email = User.Identity.Name;
            AppUser user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return Unauthorized();
            if (offer.UserId != user.Id) return Unauthorized();

            var offerDto = _mapper.Map<EditOfferViewModel>(offer);
           
            
            return View(offerDto);
        }

        // POST: Offers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( EditOfferViewModel offerDto)
        {

            if (offerDto == null) return NotFound();
            Offer offerDb = null;
            if (ModelState.IsValid)
            {
                 offerDb = await _context.Offers.Include(o => o.Category).AsNoTracking().FirstOrDefaultAsync(o => o.Id == offerDto.Id);
                var blackWords = await _context.BlackWords.ToListAsync();
                foreach (var blackword in blackWords)
                {
                    if (offerDto.Description.ToLower().Contains(blackword.Word.ToLower()))
                    {
                        ModelState.AddModelError("description", "Opis zawiera zakazane słowa");
                        offerDto.Category = offerDb.Category;
                        foreach(var item in offerDto.OfferAtributes)
                        {
                            item.Atribute = await _context.Atributes.FirstOrDefaultAsync(a => a.Id == item.AtributeId);
                        }
                        return View(offerDto);
                    }

                    if (offerDto.Title.ToLower().Contains(blackword.Word.ToLower()))
                    {

                        ModelState.AddModelError("title", "Tytuł zawiera zakazane słowa");
                        offerDto.Category = offerDb.Category;
                        foreach (var item in offerDto.OfferAtributes)
                        {
                            item.Atribute = await _context.Atributes.FirstOrDefaultAsync(a => a.Id == item.AtributeId);
                        }
                        return View(offerDto);
                    }
                }
                
                string email = User.Identity.Name;
                AppUser user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null || offerDb.UserId != user.Id) return Unauthorized();

                offerDb = _mapper.Map(offerDto, offerDb);

                _context.Update(offerDb);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyOffers));
            }
            offerDto.Category = offerDb.Category;
            foreach (var item in offerDto.OfferAtributes)
            {
                item.Atribute = await _context.Atributes.FirstOrDefaultAsync(a => a.Id == item.AtributeId);
            }
            return View(offerDto);
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

        public async Task<IActionResult> AttributesPartial(int? categoryId)
        {
            if (categoryId == null)
                return BadRequest();

            var category = await _context.Categories
                .Include(c => c.CategoryAtributes)
                .Include(c => c.ParentCategory)
                .Where(c => c.Id == categoryId)
                .FirstOrDefaultAsync();

            if (category == null)
                return NotFound();

            var parentCategory = await _context.Categories
                                        .Include(c => c.CategoryAtributes)
                                        .Where(c => c.Id == category.ParentCategoryId)
                                        .FirstOrDefaultAsync();

            while (parentCategory != null)
            {
                foreach (var attribute in parentCategory.CategoryAtributes)
                {
                    category.CategoryAtributes.Add(attribute);
                }

                parentCategory = await _context.Categories
                                        .Include(c => c.CategoryAtributes)
                                        .Where(c => c.Id == parentCategory.ParentCategoryId)
                                        .FirstOrDefaultAsync();
            }

            return PartialView("_AttributesPartial", category.CategoryAtributes);
        }

        public async Task<IActionResult> Report(int? id)
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
            try
            {
                offer.reported = true;
                _context.Update(offer);
                await _context.SaveChangesAsync();
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
    }
}