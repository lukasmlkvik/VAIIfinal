using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VAII.Data;
using VAII.Models;

namespace VAII.Controllers
{

    public class DataFo
    {
        public string logo { get; set; }
        public string name { get; set; }
    }
    public class UpdateController : Controller
    {

        private ApplicationDbContext _db;

        public UpdateController(ApplicationDbContext db)
        {
            _db = db;
        }
        public string Index()
        {
            var list = _db.Founds.ToList();

            for(int i = 0; i < list.Count; ++i)
            {
                if (list[i].name == null)
                {
                    try {
                        var uri = "https://finnhub.io/api/v1/stock/profile2?symbol=" + list[i].symbol + "&token=bu6lbcn48v6pfj0ol470";
                        var client = new System.Net.WebClient();
                        var data = client.DownloadString(uri);
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DataFo));
                        using (var m = new MemoryStream(Encoding.Unicode.GetBytes(data)))
                        {
                            DataFo d = (DataFo)serializer.ReadObject(m);
                            list[i].name = d.name;
                            list[i].logo = d.logo;
                            _db.Founds.Update(list[i]);
                            _db.SaveChanges();
                        }
                    } catch (System.Net.WebException)
                    {
                        --i;
                        Thread.Sleep(10000);
                    }

                }
            }


            return " ";
        }
    }
}