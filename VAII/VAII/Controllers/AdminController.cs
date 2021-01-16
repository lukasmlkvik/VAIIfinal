using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VAII.Data;

namespace VAII.Controllers
{
    public class AdminController : Controller
    {

        private ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<string> l = new List<string>();
            if ("admin@admin.admin".Equals(User.Identity.Name))
            {
                l = _db.Users.Select(u=>u.Email).ToList();
            }
            return View(l);
        }
    }
}