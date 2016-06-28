using System;
using System.Windows;
using gpx;
using System.Linq;
using System.Xml.Linq;
using System.Globalization;
using Microsoft.Win32;

namespace gpxEdit
{    
    public partial class StartEndDateWindow : Window
    {
        private Track _track;

        public StartEndDateWindow()
        {
            InitializeComponent();
            StartDTPicker.Value = DateTime.Now;
            FinishDTPicker.Value = DateTime.Now;            
        }

        public StartEndDateWindow(Track track) 
            : this()
        {
            _track = track;
        }        

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Сделать проверки
            if (FinishDTPicker.Value <= StartDTPicker.Value) MessageBox.Show("Конечное время не может быть меньше ");
            else
            {
                DateTime startTime = StartDTPicker.Value.Value;
                DateTime finishTime = FinishDTPicker.Value.Value;
                TimeSpan fullSpan = finishTime - startTime;
                int pieceCount = _track.GetSegments().Sum(segment => segment.GetPoints().Count());
                TimeSpan pieceSpan = new TimeSpan(fullSpan.Ticks / pieceCount);
                

                XElement[] pointsXEs = new XElement[pieceCount];
                int i = 0;

                foreach (var segment in _track.GetSegments())
                {
                    foreach (var point in segment.GetPoints())
                    {
                        DateTime date = startTime + new TimeSpan(pieceSpan.Ticks * i);
                        XElement curPointXE = new XElement("trkpt", 
                            new XAttribute("lat", point.Latitude.ToString(CultureInfo.InvariantCulture)),
                            new XAttribute("lon", point.Longtitude.ToString(CultureInfo.InvariantCulture)),
                            new XElement("time", date.ToString("yyyy-MM-ddTHH:mm:ss")));
                        pointsXEs[i] = curPointXE;
                        i++;
                        
                    }
                }
                XElement gpxXE = new XElement("gpx", 
                    new XElement("trk",
                        new XElement("trkseg", pointsXEs)
                    )
                );

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = ".gpx";
                sfd.Filter = "Gpx Files (*.gpx)|*.gpx";
                bool? result = sfd.ShowDialog();

                if (result == true)
                {
                    gpxXE.Save(sfd.FileName);
                }
            }
        }
    }
}
