using System;
using System.Windows;
using gpx;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using Microsoft.Win32;

namespace gpxEdit
{    
    public partial class StartEndDateWindow
    {
        private readonly Track _track;

        public StartEndDateWindow()
        {
            InitializeComponent();
        }

        public StartEndDateWindow(Track track) 
            : this()
        {
            _track = track;
        }        

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Сделать проверки
            if (StartDtControl.Value >= FinishDtControl.Value) MessageBox.Show("Конечное время не может быть меньше ");
            else
            {
                DateTime startTime = StartDtControl.Value;
                DateTime finishTime = FinishDtControl.Value;
                TimeSpan fullSpan = finishTime - startTime;
                int pieceCount = _track.GetSegments().Sum(segment => segment.GetPoints().Length);
                TimeSpan pieceSpan = new TimeSpan(fullSpan.Ticks / pieceCount);
                

                object[] pointsXEs = new object[pieceCount];
                int i = 0;

                foreach (var segment in _track.GetSegments())
                {
                    foreach (var point in segment.GetPoints())
                    {
                        DateTime date = startTime + new TimeSpan(pieceSpan.Ticks * i);
                        XElement curPointXe = new XElement("trkpt", 
                            new XAttribute("lat", point.Latitude.ToString(CultureInfo.InvariantCulture)),
                            new XAttribute("lon", point.Longtitude.ToString(CultureInfo.InvariantCulture)),
                            new XElement("time", date.ToString("yyyy-MM-ddTHH:mm:ss")));
                        pointsXEs[i] = curPointXe;
                        i++;
                        
                    }
                }
                XElement gpxXe = new XElement("gpx", 
                    new XElement("trk",
                        new XElement("trkseg", pointsXEs)
                    )
                );

                SaveFileDialog sfd = new SaveFileDialog
                {
                    DefaultExt = ".gpx",
                    Filter = "Gpx Files (*.gpx)|*.gpx"
                };
                bool? result = sfd.ShowDialog();

                if (result == true)
                {
                    gpxXe.Save(sfd.FileName);
                }

                Close();
            }
        }
    }
}
