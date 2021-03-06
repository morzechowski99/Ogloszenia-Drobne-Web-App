using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ogłoszenia_Drobne_Web_App.Data;
using Ogłoszenia_Drobne_Web_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ogłoszenia_Drobne_Web_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserList()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult AddAdminAlert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAdminAlert([Bind("Content")] Alert alert)
        {
            if (ModelState.IsValid)
            {
                alert.CreateDate = DateTime.Now;
                _context.Add(alert);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> BlockUser(string id, DateTime? endDate)
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return Unauthorized();

            if (endDate == null)
                endDate = DateTime.Now.AddYears(100);

            await _userManager.SetLockoutEnabledAsync(user, true);

            await _userManager.SetLockoutEndDateAsync(user, endDate);

            return RedirectToAction("UserList", "Admin");
        }

        public async Task<IActionResult> UnblockUser(string id)
        {
            AppUser user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return Unauthorized();

            await _userManager.SetLockoutEnabledAsync(user, false);

            await _userManager.SetLockoutEndDateAsync(user, null);

            return RedirectToAction("UserList", "Admin");
        }

        public async Task<IActionResult> ReportedOffersList()
        {
            var reportedUsers = await _context.Offers.Where(o => o.reported == true).ToListAsync();
            return View(reportedUsers);
        }

        public async Task<IActionResult> DeleteOffer(int? id)
        {
            if (id == null) return BadRequest();

            var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == id);

            if (offer == null) return NotFound();

            return View(offer);
        }

        public async Task<IActionResult> DeleteOfferConfirm(int? id)
        {
            if (id == null) return BadRequest();

            var offer = await _context.Offers.Include(o => o.OfferAtributes).FirstOrDefaultAsync(o => o.Id == id);

            if (offer == null) return NotFound();

            foreach(var item in offer.OfferAtributes)
            {
                _context.Remove(item);
            }
            _context.Remove(offer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ReportedOffersList));
        }

        public async Task<IActionResult> DeleteReport(int? id)
        {
            if (id == null) return BadRequest();

            var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == id);

            if (offer == null) return NotFound();

            offer.reported = false;

            _context.Update(offer);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ReportedOffersList));
        }

        public async Task<IActionResult> RemoveAlerts()
        {
            _context.Alerts.RemoveRange(_context.Alerts);
            await  _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}