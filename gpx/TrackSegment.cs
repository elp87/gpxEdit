using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gpx
{
    public class TrackSegment
    {
        private List<TrackPoint> _points;

        public TrackSegment()
        {
            _points = new List<TrackPoint>();
        }

        public void AddPoint(TrackPoint point)
        {
            _points.Add(point);
        }

        public TrackPoint[] GetPoints()
        {
            TrackPoint[] points = new TrackPoint[_points.Count];
            for (int i = 0; i < _points.Count; i++)
            {
                points[i] = _points[i];
            }
            return points;
        }
    }
}
