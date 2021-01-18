using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAII.Data;

namespace VAII.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {

            List<string> l =  _db.Users.Where(u=> !u.Email.Equals(User.Identity.Name)).Select(u=>u.Email).ToList();

            return View(l);
        }

    }
}