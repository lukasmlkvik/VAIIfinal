using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VAII.Models
{
    public class AxisModel
    {
        public AxisModel(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double x { get; set; }
        public double y { get; set; }
    }
}
