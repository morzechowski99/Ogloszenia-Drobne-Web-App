using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ogłoszenia_Drobne_Web_App.Data;
using Ogłoszenia_Drobne_Web_App.Models;
using Ogłoszenia_Drobne_Web_App.Models.Enums;
using Ogłoszenia_Drobne_Web_App.ViewModels;

namespace Ogłoszenia_Drobne_Web_App.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Categories.Include(c => c.ParentCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.CategoryAtributes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            var parentCategory = await _context.Categories
                                        .Include(c => c.CategoryAtributes)
                                        .Where(c => c.Id == category.ParentCategoryId)
                                        .FirstOrDefaultAsync();

            while (parentCategory != null)
            {
                foreach(var attribute in parentCategory.CategoryAtributes)
                {
                    category.CategoryAtributes.Add(attribute);
                }

                parentCategory = await _context.Categories
                                        .Include(c => c.CategoryAtributes)
                                        .Where(c => c.Id == parentCategory.ParentCategoryId)
                                        .FirstOrDefaultAsync();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName",null);
            var categories = from AtributeTypes at in Enum.GetValues(typeof(AtributeTypes))
                             select new { Id = (int)at, Name = at.ToString() };
            ViewData["AtributeTypes"] = new SelectList(categories, "Id", "Name", null);
            return View();
        }

        public async Task<IActionResult> GetCategoriesWithParent()
        {
            try
            {
                var categories = await _context.Categories.Include(c => c.ParentCategory).ToListAsync();
                return Ok(categories);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel categoryDto)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(categoryDto);
                _context.Add(category);
                if (categoryDto.Attributes != null)
                {
                    foreach (var atributeView in categoryDto.Attributes)
                    {
                        Atribute atribute = GetAttributeFromType(atributeView.Type);
                        atribute.Category = category;
                        atribute.Name = atributeView.Name;
                        _context.Add(atribute);
                    }
                }
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var categories = from AtributeTypes at in Enum.GetValues(typeof(AtributeTypes))
                             select new { Id = (int)at, Name = at.ToString() };
            ViewData["AtributeTypes"] = new SelectList(categories, "Id", "Name", null);
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", null);
            return View(categoryDto);
        }

        private Atribute GetAttributeFromType(AtributeTypes type)
        {
            switch (type)
            {
                case AtributeTypes.Checkbox:
                    return new BoolAtribute();
                case AtributeTypes.Data:
                    return new DateTimeAtribute();
                case AtributeTypes.Numer:
                    return new NumberAtribute();
                case AtributeTypes.Tekstowy:
                    return new TextAtribute();
                case AtributeTypes.Zmiennoprzecinkowy:
                    return new DoubleAtribute();
                default:
                    return null;
            }
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", category.ParentCategoryId);
            return View(category);
        }

        public async Task<IActionResult> EditAttributes(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
               .Include(c => c.ParentCategory)
               .Include(c => c.CategoryAtributes)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

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

            ViewBag.CategoryName = category.CategoryName;
            ViewBag.CategoryId = category.Id;
            return View(category.CategoryAtributes);

        }

        [HttpPost]
        public async Task<IActionResult> AddAttribute(int? id, CategoryAtributeViewModel attribute)
        {
            if (id == null || attribute == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var newAttribute = GetAttributeFromType(attribute.Type);
                newAttribute.Name = attribute.Name;
                newAttribute.CategoryId = (int)id;

                _context.Add(newAttribute);

                await _context.SaveChangesAsync();

                return RedirectToAction("EditAttributes", new { id = id });
            }
            return RedirectToAction("EditAttributes", new { id = id }); ;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAttribute(int? id)
        {
            try
            {
                if (id == null)
                    return BadRequest();

                var attribute = await _context.Atributes.Include(a => a.OfferAtributes).Where(a => a.Id == id).FirstOrDefaultAsync();

                if (attribute == null)
                    return NotFound();

                foreach (var item in attribute.OfferAtributes)
                {
                    _context.Remove(item);
                }
                _context.Remove(attribute);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAttribute( Atribute attribute)
        {
            if (attribute == null)
                return BadRequest();

            var attributeDb = await _context.Atributes
                                    .FindAsync(attribute.Id);

            if (attributeDb == null)
                return NotFound();

            attributeDb.Name = attribute.Name;

            _context.Update(attributeDb);

            await _context.SaveChangesAsync();

            return RedirectToAction("EditAttributes", new { id = attributeDb.CategoryId });
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.CategoryAtributes)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            try
            {
                foreach(var attribute in category.CategoryAtributes)
                {
                    _context.Remove(attribute);
                }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = "Tej kategorii nie można aktualnie usunąć.";
                return View(category);
            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
