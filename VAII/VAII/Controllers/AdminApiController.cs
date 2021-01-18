using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAII.Data;

namespace VAII.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {

        private ApplicationDbContext _db;

        public AdminApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{email}")]
        public void Delete(string email)
        {
            if (!User.IsInRole("Admin")) return;//pre istotu
            if (User.Identity.Name.Equals(email)) return;
            try
            {
                _db.Users.Remove(_db.Users.Single(u => u.Email.Equals(email)));
                _db.SaveChanges();
            }
            catch { }
        }
    }
}
