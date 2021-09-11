using LiveCharts;
using LiveCharts.Wpf;
using QLBN_COVID.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBN_COVID
{
    public partial class Report : Form
    {
        private CovidDataContext db;
        public Report()
        {
            InitializeComponent();
            
        }

        private void Report_Load(object sender, EventArgs e)
        {
            db = new CovidDataContext();
            var F0 = (from p in db.Patients
                        where p.IDStatus == 1
                        select new
                        {
                            IDStatus = p.IDStatus,

                        }).Count();
            var F1 = (from p in db.Patients
                      where p.IDStatus == 2
                      select new
                      {
                          IDStatus = p.IDStatus,

                      }).Count();
            var F2 = (from p in db.Patients
                      where p.IDStatus == 3
                      select new
                      {
                          IDStatus = p.IDStatus,

                      }).Count();
            var F3 = (from p in db.Patients
                      where p.IDStatus == 3
                      select new
                      {
                          IDStatus = p.IDStatus,

                      }).Count();
            var none = (from p in db.Patients
                      where p.IDStatus == 6
                      select new
                      {
                          IDStatus = p.IDStatus,

                      }).Count();

            //chartStatus.DataSource = list;
            //var series = chartStatus.Series.Add("series1");
            //series.XValueMember = "IDStatus";
            //series.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            ////series.YValueMembers = "Kind_Of_Status";
            //series.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;

            chart1.Series["Status"].Points.Add(F0);
            chart1.Series["Status"].Points[0].Color = Color.Red;
            chart1.Series["Status"].Points[0].AxisLabel = "F0";
            chart1.Series["Status"].Points[0].LegendText = "F0";
            chart1.Series["Status"].Points[0].Label = F0.ToString();

            chart1.Series["Status"].Points.Add(F1);
            chart1.Series["Status"].Points[1].Color = Color.Orange;
            chart1.Series["Status"].Points[1].AxisLabel = "F1";
            chart1.Series["Status"].Points[1].LegendText = "F1";
            chart1.Series["Status"].Points[1].Label = F1.ToString();

            chart1.Series["Status"].Points.Add(F2);
            chart1.Series["Status"].Points[2].Color = Color.Yellow;
            chart1.Series["Status"].Points[2].AxisLabel = "F2";
            chart1.Series["Status"].Points[2].LegendText = "F2";
            chart1.Series["Status"].Points[2].Label = F2.ToString();

            chart1.Series["Status"].Points.Add(F3);
            chart1.Series["Status"].Points[3].Color = Color.Blue;
            chart1.Series["Status"].Points[3].AxisLabel = "F3";
            chart1.Series["Status"].Points[3].LegendText = "F3";
            chart1.Series["Status"].Points[3].Label = F3.ToString();

            chart1.Series["Status"].Points.Add(none);
            chart1.Series["Status"].Points[4].Color = Color.Green;
            chart1.Series["Status"].Points[4].AxisLabel = "Khoi benh";
            chart1.Series["Status"].Points[4].LegendText = "Khoi benh";
            chart1.Series["Status"].Points[4].Label = none.ToString();

            //Pie chart
            //chartStatus.Series["Series1"].XValueMember = "F0";

            //chartStatus.Series["Series1"].Points.AddXY("F1", F1);
            //chartStatus.Series["Series1"].Points.AddXY("F2", F2);
            //chartStatus.Series["Series1"].Points.AddXY("F2", F3);
            //chartStatus.Series["Series1"].Points.AddXY("Khoi benh", none);
            Func<ChartPoint, string> labelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            pieChart1.Series = new SeriesCollection
            {
                 new PieSeries
                {
                    Title = "F0",
                    Values = new ChartValues<int> {F0},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                 new PieSeries
                {
                    Title = "F1",
                    Values = new ChartValues<int> {F1},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                 new PieSeries
                {
                    Title = "F2",
                    Values = new ChartValues<int> {F2 } ,
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                 new PieSeries
                {
                    Title = "F3",
                    Values = new ChartValues<int> {F3},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
                 new PieSeries
                {
                    Title = "Khoi benh",
                    Values = new ChartValues<int> {none},
                    DataLabels = true,
                    LabelPoint = labelPoint
                },
            };
            pieChart1.LegendLocation = LegendLocation.Bottom;
        }
    }
}
