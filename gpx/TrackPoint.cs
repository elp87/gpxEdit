using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gpx
{
    public class TrackPoint
    {
        public double Latitude { get; set; } // TODO: Отработать варианты
        public double Longtitude { get; set; } // TODO: Отработать варианты
        public DateTime Time { get; set; }
        public double Elevation { get; set; }
    }
}
