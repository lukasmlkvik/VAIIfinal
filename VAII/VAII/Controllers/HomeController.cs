using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var list1 = _db.UsersFounds.Where(u => u.Email.Equals(User.Identity.Name));
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
            ServerInfoModel sm = _db.ServerInfo.Where(s => s.id == 1).Take(1).ToList()[0];
            if (DateTime.Now - sm.date >= TimeSpan.FromHours(24))
            {
                var uri = "https://finnhub.io/api/v1/company-news?symbol=AAPL&from=2020-04-30&to=2020-05-01&token=bu6lbcn48v6pfj0ol470";
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
            else {
                newsList.NewsList = _db.News.ToList();
            }
            return View(newsList);
        }


        public IActionResult Founds(int page = 1, int count = 10, string search = "")
        {
            /*var uri = "https://finnhub.io/api/v1/stock/symbol?exchange=US&token=bu6lbcn48v6pfj0ol470";
            var client = new System.Net.WebClient();
            var data = client.DownloadString(uri);*/
            List<FoundModel> list = search == null ? _db.Founds.ToList() : _db.Founds.Where(f => f.symbol.Contains(search) || f.description.Contains(search)).ToList();
           /* DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<FoundModel>));
            using (var m = new MemoryStream(Encoding.Unicode.GetBytes(data)))
            {
                list = ((List<FoundModel>)serializer.ReadObject(m)).OrderBy(a => a.symbol).ToList();
            }*/
            if (count == -1)
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

        public IActionResult Found(string s)
        {

            var uri = "https://finnhub.io/api/v1/forex/candle?symbol=" + s + "&resolution=D&from=1572651390&to=1575243390&token=bu6lbcn48v6pfj0ol470";
            var client = new System.Net.WebClient();
            var data = client.DownloadString(uri);
            FoundDetailModel found = null;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(FoundDetailModel));
            using (var m = new MemoryStream(Encoding.Unicode.GetBytes(data)))
            {
                found = (FoundDetailModel)serializer.ReadObject(m);
            }

           /* List<AxisModel> list = new List<AxisModel>();
            for (int i = 0; i < found.t.Count; i++)
            {
                list.Add(new AxisModel(found.t[i], found.o[i]));
            }*/


            return View(found);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
