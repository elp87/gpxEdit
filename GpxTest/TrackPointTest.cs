using gpx;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GpxTest
{
    [TestClass]
    public class TrackPointTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidValueException))]
        public void Latitude_InvalidValueException()
        {
            const double value = 95;
            new TrackPoint {Latitude = value};
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidValueException))]
        public void Latitude_InvalidValueException_Minus()
        {
            const double value = -95;
            new TrackPoint { Latitude = value };
        }
    }
}
