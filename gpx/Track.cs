using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gpx
{
    public class Track
    {
        private List<TrackSegment> _segments;

        public Track()
        {
            _segments = new List<TrackSegment>();
        }

        public void AddSegment(TrackSegment segment)
        {
            _segments.Add(segment);
        }
    }
}
