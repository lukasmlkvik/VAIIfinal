using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VAII.Models
{
    public class FoundDetailModel
    {
        public List<double> o { get; set; }
        public List<double> h { get; set; }
        public List<double> l { get; set; }
        public List<double> c { get; set; }
        public List<double> v { get; set; }
        public List<double> t { get; set; }
        public string s { get; set; }
        public string name { get; set; }
        public string logo { get; set; }
        public string Symbol { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int Resolution { get; set; }
    }
}
