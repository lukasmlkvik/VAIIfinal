using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
