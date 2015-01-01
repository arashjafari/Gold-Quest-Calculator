using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Windows.Forms.DataVisualization.Charting;

namespace GoldQuest_1393
{
    public partial class frmChart : Form
    {
        public frmChart()
        {
            InitializeComponent();
        }

        private void frmChart_Load(object sender, EventArgs e)
        {
             
            cboChartType.SelectedIndex = cboChartType.Items.IndexOf("Spline");
        }

        private void cboChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                chart1.Series["In"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), cboChartType.Items[cboChartType.SelectedIndex].ToString());
                chart1.Series["Out"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), cboChartType.Items[cboChartType.SelectedIndex].ToString());
                chart1.Series["Benefit"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), cboChartType.Items[cboChartType.SelectedIndex].ToString());
            }
            catch { }
        } 

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) 
                chart1.SaveImage(saveFileDialog1.FileName, ImageFormat.Png);
             
        }

 
    }
}
