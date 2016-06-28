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

        public TrackSegment[] GetSegments()
        {
            TrackSegment[] segments = new TrackSegment[_segments.Count];
            for (int i = 0; i < _segments.Count; i++)
            {
                segments[i] = _segments[i];
            }
            return segments;
        }
    }
}
