using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using VAII.Data;
using VAII.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VAII.Controllers
{
    [Route("api/[controller]")]
    public class UserFoundsController : Controller
    {

        private ApplicationDbContext _db;

        public UserFoundsController(ApplicationDbContext db)
        {
            _db = db;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<FoundModel> Get()
        {
            if (!User.Identity.IsAuthenticated) return new List<FoundModel>();
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var list1 = _db.UsersFounds.Where(
                u => u.Id.Equals(userId));
            IEnumerable<FoundModel> list = list1.Join(_db.Founds,
                uf => uf.symbol, f => f.symbol, (uf, f) => f);
            return list;
        }


        // PUT api/<controller>/5
        [HttpPut("{symbol}")]
        public void Put(string symbol)
        {
            if (!User.Identity.IsAuthenticated) return;
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var list = _db.Founds.ToList();
            foreach (var f in list)
            {
                if (f.symbol.Equals(symbol))
                {
                    try {
                        _db.UsersFounds.Add(
                                new UserFoundModel(userId, symbol)
                            );
                        _db.SaveChanges();
                    }
                    catch (Microsoft.EntityFrameworkCore.DbUpdateException)
                    { }                    
                    break;
                }
            }

        }

        // DELETE api/<controller>/5
        [HttpDelete("{symbol}")]
        public void Delete(string symbol)
        {
            if (!User.Identity.IsAuthenticated) return;
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var list = _db.Founds.ToList();
            foreach (var f in list)
            {
                if (f.symbol.Equals(symbol))
                {
                    try {
                        UserFoundModel uf = _db.UsersFounds.Where(u => u.Id.Equals(userId) && u.symbol.Equals(symbol)).Single();

                        _db.UsersFounds.Remove(uf);
                        _db.SaveChanges();
                    }
                    catch(System.InvalidOperationException) { }                
                        
                    
                    break;
                }
            }
        }
    }
}
