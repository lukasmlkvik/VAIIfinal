using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
