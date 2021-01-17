using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.Logging;
using VAII.Data;
using VAII.Models;

namespace VAII.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
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
                uf => uf.symbol, f => f.symbol,(uf,f) => f).ToList();
            return View(list);
        }

        public IActionResult Profile()
        {

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult News()
        {
            NewsListModel newsList = new NewsListModel();
            ServerInfoModel sm = _db.ServerInfo.Single(s => s.id == 1);
            if (DateTime.Now - sm.date >= TimeSpan.FromHours(24))
            {

                try
                {
                    var uri = "https://finnhub.io/api/v1/company-news?symbol=AAPL&from="+ (DateTime.Now - TimeSpan.FromDays(5)).ToString("yyyy-MM-dd")+"&to=" + DateTime.Now.ToString("yyyy-MM-dd") + "&token=" + Constants.API_TOKEN;
                    var client = new System.Net.WebClient();
                    var data = client.DownloadString(uri);
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<NewsModel>));
                    using (var m = new MemoryStream(Encoding.Unicode.GetBytes(data)))
                    {
                        newsList.NewsList = (List<NewsModel>)serializer.ReadObject(m);
                        _db.News.RemoveRange(_db.News);
                        _db.News.AddRange(newsList.NewsList);

                        sm.date = DateTime.Now;
                        _db.ServerInfo.Update(sm);

                        _db.SaveChanges();
                    }
                }
                catch (System.Net.WebException)
                {

                }
                
            }
            else {
                newsList.NewsList = _db.News.ToList();
            }
            return View(newsList);
        }


        public IActionResult Founds(int page = 1, int count = 10, string search = "")
        {
            /*var uri = "https://finnhub.io/api/v1/stock/symbol?exchange=US&token=" + Constants.API_TOKEN;
            var client = new System.Net.WebClient();
            var data = client.DownloadString(uri);*/
            List<FoundModel> list = search == null ? _db.Founds.ToList() :  _db.Founds.Where(
                    f => f.symbol.Contains(search) || (f.description != null && f.description.Contains(search)) || (f.name != null && f.name.Contains(search))).ToList();
           /* DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<FoundModel>));
            using (var m = new MemoryStream(Encoding.Unicode.GetBytes(data)))
            {
                list = ((List<FoundModel>)serializer.ReadObject(m)).OrderBy(a => a.symbol).ToList();
            }*/
            if (count <= 0)
            {
                count = list.Count;
            }
            FoundsData dataF = new FoundsData();

           /* foreach (var d in list)
            {
                _db.Founds.Add(d);
            }
            _db.SaveChanges();*/

            dataF.List = list.Take((page) * count).TakeLast(count).ToList();
            dataF.Count = count;
            dataF.Page = page;
            dataF.Search = search;
            dataF.MaxCount = list.Count;
            dataF.MaxPage = list.Count % count == 0 ? list.Count / count : list.Count / count + 1;
            return View(dataF);
        }
        private static long offset => new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;
        public IActionResult Found(string s, string from = "2021-01-01T00:00", string to = "2021-01-20T00:00", int resolution = 1)
        {
            DateTime From = DateTime.Parse("2021-01-01T00:00");
            DateTime To = DateTime.Parse("2021-01-20T00:00");
            try {
                From = DateTime.Parse(from);
            } catch (Exception) { }
            try
            {
                To = DateTime.Parse(to);
            }
            catch (Exception) { }

            string[] resolutions = { "1", "5", "15", "30", "60", "D", "W", "M" };

            int r = resolution < 0 ? 0 : (resolution >= resolutions.Length ? 0 : resolution);

            FoundDetailModel found = null;
            try
            {
                var uri = "https://finnhub.io/api/v1/forex/candle?symbol=" + s + "&resolution=" + resolutions[r] + "&from=" + ((From.Ticks- offset) /10000000) + "&to=" + ((To.Ticks-offset) / 10000000) + "&token=" + Constants.API_TOKEN;
                var client = new System.Net.WebClient();
                var data = client.DownloadString(uri);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(FoundDetailModel));
                using (var m = new MemoryStream(Encoding.Unicode.GetBytes(data)))
                {
                    found = (FoundDetailModel)serializer.ReadObject(m);
                }
            }
            catch (System.Net.WebException)
            {
                found = new FoundDetailModel();
            }
            try {
                FoundModel m = _db.Founds.Single(f => f.symbol.Equals(s));
                found.logo = m.logo;
                found.name = m.name;
            } catch (Exception) { }
            found.Symbol = s;
            found.Resolution = resolution;
            found.From = from;
            found.To = to;
            return View(found);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
