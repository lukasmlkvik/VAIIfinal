using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using VAII.Data;
using VAII.Models;

namespace VAII.Controllers
{
    public class FoundsController : Controller
    {

        private ApplicationDbContext _db;

        public FoundsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index(int page = 1, int count = 10, string search = "")
        {

            List<FoundModel> list = search == null ? _db.Founds.ToList() : _db.Founds.Where(
                    f => f.symbol.Contains(search) || (f.description != null && f.description.Contains(search)) || (f.name != null && f.name.Contains(search))).ToList();

            if (count <= 0)
            {
                count = list.Count;
            }
            FoundsData dataF = new FoundsData();


            dataF.List = list.Take((page) * count).TakeLast(count).ToList();
            dataF.Count = count;
            dataF.Page = page;
            dataF.Search = search;
            dataF.MaxCount = list.Count;
            dataF.MaxPage = list.Count % count == 0 ? list.Count / count : list.Count / count + 1;
            return View(dataF);
        }


        private static long offset => new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks;
        public IActionResult Found(string s, string from = "2022-01-01T00:00", string to = "2022-01-20T00:00", int resolution = 1)
        {
            DateTime From = DateTime.Parse("2022-01-01T00:00");
            DateTime To = DateTime.Parse("2022-01-20T00:00");
            try
            {
                From = DateTime.Parse(from);
            }
            catch (Exception) { }
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
                var uri = "https://finnhub.io/api/v1/stock/candle?symbol=" + s + "&resolution=" + resolutions[r] + "&from=" + ((From.Ticks - offset) / 10000000) + "&to=" + ((To.Ticks - offset) / 10000000) + "&token=" + Constants.API_TOKEN;
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
            try
            {
                FoundModel m = _db.Founds.Single(f => f.symbol.Equals(s));
                found.logo = m.logo;
                found.name = m.name;
            }
            catch (Exception) { }
            found.Symbol = s;
            found.Resolution = resolution;
            found.From = from;
            found.To = to;
            return View(found);
        }
    }
}