using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using VAII.Data;
using VAII.Models;

namespace VAII.Controllers
{
    public class ProfileController : Controller
    {

        private ApplicationDbContext _db;

        public ProfileController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult FoundsManage()
        {
            if (!User.Identity.IsAuthenticated) return View(new List<FoundModel>());
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var list1 = _db.UsersFounds.Where(
                u => u.Id.Equals(userId));
            List<FoundModel> list = list1.Join(_db.Founds,
                uf => uf.symbol, f => f.symbol, (uf, f) => f).ToList();
            return View(list);
        }

        public IActionResult Index()
        {

            return View();
        }
    }
}