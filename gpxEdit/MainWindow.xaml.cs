using System;
using gpx;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Win32;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace gpxEdit
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenGpxRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            Gpx gpxFile = new Gpx();
            string gpxFilename = OpenGpxDialog();
            if (gpxFilename == null) return;
            XElement gpxXe = XElement.Load(gpxFilename);

            var trkList = gpxXe.Elements().Where(el => el.Name.LocalName == "trk").ToList();
            foreach (var trkXe in trkList)
            {
                Track track = new Track();
                foreach (var trksegXe in trkXe.Elements().Where(el => el.Name.LocalName == "trkseg").ToList())
                {
                    TrackSegment segment = new TrackSegment();
                    foreach (var trkptXe in trksegXe.Elements().Where(el => el.Name.LocalName == "trkpt").ToList())
                    {
                        double latValue = double.Parse(trkptXe.Attribute("lat").Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        double longValue = double.Parse(trkptXe.Attribute("lon").Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        DateTime time = DateTime.Parse(trkptXe.Elements().First(el => el.Name.LocalName == "time").Value);
                        segment.AddPoint(new TrackPoint { Latitude = latValue, Longtitude = longValue, Time = time});
                    }
                    track.AddSegment(segment);

                    LocationCollection locations = new LocationCollection();
                    foreach (var point in segment.GetPoints())
                    {
                        locations.Add(new Location { Latitude = point.Latitude, Longitude = point.Longtitude });
                    }
                    MapPolyline line = CreateMapLine(locations);
                    TracksMap.Children.Add(line);
                }
                gpxFile.AddTrack(track);
            }
        }

        private void Waypoints2TrackRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            Gpx gpxFile = new Gpx();
            string gpxFilename = OpenGpxDialog();
            if (gpxFilename == null) return;
            XElement gpxXe = XElement.Load(gpxFilename);
            Track track = new Track();
            TrackSegment segment = new TrackSegment();

            var rteList = gpxXe.Elements().Where(el => el.Name.LocalName == "rte" ).ToList();
            if (rteList.Count != 0)
            {
                foreach (var rteXe in rteList)
                {
                    var rteptList = rteXe.Elements().Where(el => el.Name.LocalName == "rtept").ToList();
                    foreach (var rteptXe in rteptList)
                    {
                        double latValue = double.Parse(rteptXe.Attribute("lat").Value,
                            System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        double longValue = double.Parse(rteptXe.Attribute("lon").Value,
                            System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        segment.AddPoint(new TrackPoint {Latitude = latValue, Longtitude = longValue});
                    }
                }
                track.AddSegment(segment);
                gpxFile.AddTrack(track);
            }
            var trkList = gpxXe.Elements().Where(el => el.Name.LocalName == "trk").ToList();
            foreach (var trkXe in trkList)
            {
                var trksegList = trkXe.Elements().Where(el => el.Name.LocalName == "trkseg").ToList();
                foreach (var trksegXe in trksegList)
                {
                    var trkptList = trksegXe.Elements().Where(el => el.Name.LocalName == "trkpt").ToList();
                    foreach (var trkptXe in trkptList)
                    {
                        double latValue = double.Parse(trkptXe.Attribute("lat").Value,
                            System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        double longValue = double.Parse(trkptXe.Attribute("lon").Value,
                            System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        segment.AddPoint(new TrackPoint {Latitude = latValue, Longtitude = longValue});
                    }
                }
                track.AddSegment(segment);
                gpxFile.AddTrack(track);
            }

            LocationCollection locations = new LocationCollection();
            foreach (var point in segment.GetPoints())
            {
                locations.Add(new Location { Latitude = point.Latitude, Longitude = point.Longtitude });
            }
            MapPolyline line = CreateMapLine(locations);
            TracksMap.Children.Add(line);

            StartEndDateWindow window = new StartEndDateWindow(track);
            window.ShowDialog();
        }

        private static string OpenGpxDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = ".gpx",
                Filter = "Gpx Files (*.gpx)|*.gpx"
            };
            bool? result = ofd.ShowDialog();
            return result == true ? ofd.FileName : null;
        }

        private static MapPolyline CreateMapLine(LocationCollection locations)
        {
            MapPolyline line = new MapPolyline
            {
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 3,
                Opacity = 0.7,
                Locations = locations
            };
            return line;
        }
    }
}
