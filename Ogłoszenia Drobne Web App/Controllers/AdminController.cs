using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
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

        //public bool LockUser(string email, DateTime? endDate)
        //{
        //    if (endDate == null)
        //        endDate = EndDate;

        //    var userTask = _userManager.FindByEmailAsync(email);
        //    userTask.Wait();
        //    var user = userTask.Result;

        //    var lockUserTask = _userManager.SetLockoutEnabledAsync(user, true);
        //    lockUserTask.Wait();

        //    var lockDateTask = _userManager.SetLockoutEndDateAsync(user, endDate);
        //    lockDateTask.Wait();

        //    return lockDateTask.Result.Succeeded && lockUserTask.Result.Succeeded;
        //}
    }
}