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
using VAII.Models;

namespace VAII.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult News()
        {

            var uri = "https://finnhub.io/api/v1/company-news?symbol=AAPL&from=2020-04-30&to=2020-05-01&token=bu6lbcn48v6pfj0ol470";
            var client = new System.Net.WebClient();
            var data = client.DownloadString(uri);
            NewsListModel newsList = new NewsListModel();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<NewsModel>));
            using (var m = new MemoryStream(Encoding.Unicode.GetBytes(data)))
            {
                newsList.NewsList = (List<NewsModel>)serializer.ReadObject(m);
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
