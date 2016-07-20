using System;

namespace gpx
{
    public class TrackPoint
    {
        private double _latitude;

        public double Latitude
        {
            get { return _latitude; }
            set
            {
                if (Math.Abs(value) > 90) throw new InvalidValueException();
                _latitude = value;
            }
        } 
        public double Longtitude { get; set; } // TODO: Отработать варианты
        public DateTime Time { get; set; }
        public double Elevation { get; set; }
    }

    public class InvalidValueException : Exception
    {
    }
}
