
namespace QLBN_COVID
{
    partial class Report
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chartStatus = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pieChart1 = new LiveCharts.WinForms.PieChart();
            ((System.ComponentModel.ISupportInitialize)(this.chartStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chartStatus
            // 
            chartArea5.Name = "ChartArea1";
            this.chartStatus.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chartStatus.Legends.Add(legend5);
            this.chartStatus.Location = new System.Drawing.Point(369, 12);
            this.chartStatus.Name = "chartStatus";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.chartStatus.Series.Add(series5);
            this.chartStatus.Size = new System.Drawing.Size(96, 137);
            this.chartStatus.TabIndex = 1;
            this.chartStatus.Text = "chart1";
            // 
            // chart1
            // 
            chartArea6.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chart1.Legends.Add(legend6);
            this.chart1.Location = new System.Drawing.Point(41, 45);
            this.chart1.Name = "chart1";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Status";
            this.chart1.Series.Add(series6);
            this.chart1.Size = new System.Drawing.Size(300, 300);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            title3.Name = "Title1";
            title3.Text = "Trang thai benh nhan";
            this.chart1.Titles.Add(title3);
            // 
            // pieChart1
            // 
            this.pieChart1.Location = new System.Drawing.Point(408, 167);
            this.pieChart1.Name = "pieChart1";
            this.pieChart1.Size = new System.Drawing.Size(334, 243);
            this.pieChart1.TabIndex = 3;
            this.pieChart1.Text = "Tinh trang benh nhan";
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pieChart1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.chartStatus);
            this.Name = "Report";
            this.Text = "Report";
            this.Load += new System.EventHandler(this.Report_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chartStatus;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private LiveCharts.WinForms.PieChart pieChart1;
    }
}