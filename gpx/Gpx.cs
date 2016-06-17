using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gpx
{
    public class Gpx
    {
        private List<Track> _tracks;

        public Gpx()
        {
            _tracks = new List<Track>();
        }

        public void AddTrack(Track track)
        {
            _tracks.Add(track);
        }
    }
}
