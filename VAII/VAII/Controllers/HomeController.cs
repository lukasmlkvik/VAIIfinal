﻿using System;
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


        public IActionResult Founds(int page = 1, int count = 10)
        {
            var uri = "https://finnhub.io/api/v1/stock/symbol?exchange=US&token=bu6lbcn48v6pfj0ol470";
            var client = new System.Net.WebClient();
            var data = client.DownloadString(uri);
            List<FoundModel> list = null;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<FoundModel>));
            using (var m = new MemoryStream(Encoding.Unicode.GetBytes(data)))
            {
                list = (List<FoundModel>)serializer.ReadObject(m);
            }
            if (count == -1)
            {
                count = list.Count;
            }
            FoundsData dataF = new FoundsData();
            dataF.List = list.OrderBy(a => a.symbol).Take((page) * count).TakeLast(count).ToList();
            dataF.Count = count;
            dataF.Page = page;
            dataF.MaxCount = list.Count;
            dataF.MaxPage = list.Count % count == 0 ? list.Count / count : list.Count % count + 1;
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
