using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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