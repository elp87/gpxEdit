using gpx;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace gpxEdit
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenGpxRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            Gpx gpxFile = new Gpx();
            string gpxFilename = OpenGpxDialog();
            if (gpxFilename != null)
            {
                XElement gpxXE = XElement.Load(gpxFilename);

                var trkList = gpxXE.Elements().Where(el => el.Name.LocalName == "trk").ToList();
                foreach (var trkXE in trkList)
                {
                    Track track = new Track();
                    foreach (var trksegXE in trkXE.Elements().Where(el => el.Name.LocalName == "trkseg").ToList())
                    {
                        TrackSegment segment = new TrackSegment();
                        foreach (var trkptXE in trksegXE.Elements().Where(el => el.Name.LocalName == "trkpt").ToList())
                        {

                            double latValue, longValue;
                            DateTime time;
                            latValue = double.Parse(trkptXE.Attribute("lat").Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                            longValue = double.Parse(trkptXE.Attribute("lon").Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                            time = DateTime.Parse(trkptXE.Elements().First(el => el.Name.LocalName == "time").Value);
                            segment.AddPoint(new TrackPoint { Latitude = latValue, Longtitude = longValue });
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
        }

        private void Waypoints2TrackRibbonButton_Click(object sender, RoutedEventArgs e)
        {
            Gpx gpxFile = new Gpx();
            string gpxFilename = OpenGpxDialog();
            if (gpxFilename != null)
            {
                XElement gpxXE = XElement.Load(gpxFilename);
                Track track = new Track();
                TrackSegment segment = new TrackSegment();

                var rteList = gpxXE.Elements().Where(el => el.Name.LocalName == "rte").ToList();
                foreach (var rteXE in rteList)
                {
                    var rteptList = rteXE.Elements().Where(el => el.Name.LocalName == "rtept").ToList();
                    foreach (var rteptXE in rteptList)
                    {
                        double latValue, longValue;
                        latValue = double.Parse(rteptXE.Attribute("lat").Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        longValue = double.Parse(rteptXE.Attribute("lon").Value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
                        segment.AddPoint(new TrackPoint { Latitude = latValue, Longtitude = longValue });
                    }
                }
                track.AddSegment(segment);
                gpxFile.AddTrack(track);
                
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
        }

        private static string OpenGpxDialog()
        {
            Gpx gpxFile = new Gpx();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".gpx";
            ofd.Filter = "Gpx Files (*.gpx)|*.gpx";
            bool? result = ofd.ShowDialog();
            if (result == true) return ofd.FileName;
            else return null;
        }

        private static MapPolyline CreateMapLine(LocationCollection locations)
        {
            MapPolyline line = new MapPolyline();
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.StrokeThickness = 3;
            line.Opacity = 0.7;
            line.Locations = locations;
            return line;
        }
    }
}
