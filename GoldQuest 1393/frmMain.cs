using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace GoldQuest_1393
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        /* 
             By Arash Jafari
             ArashRJ@Gmail.com
             http://ArashJafari.com 
         */

        public class Person
        { 
            public byte Month { get; set; }
            public bool Include { get; set; }
            public bool Benefit { get; set; }
        }

        private bool bBreak = false;
        private List<Person> People = new List<Person>();
        private int iMonthForBenefit = 0;

        private void btnRun_Click(object sender, EventArgs e)
        {
            People.Clear();
            txtResults.Text = "";
            btnStop.Enabled = true;
            btnRun.Enabled = false; 
            btnClear.Enabled = false;
            btnSave.Enabled = false;
            btnChart.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            bBreak = false;
            var watch = Stopwatch.StartNew(); 

            //*************************************************************************
            AddLog("Month" + "\t" + "Out" + "\t" + "In" + "\t" + "Benefit");
            AddLog("--------------------------------------------------------------");
            AddPeople(ref People, 1);
            int Level = 0;
            CalcMonthForBenefit();
            while(true){
                Level++;
                Status("Proccessing Month: " + Level.ToString());
                Application.DoEvents(); 
                Process(ref People);
                AddLog(Level + "\t" + GetNoPeopleOut() + "\t" + GetNoPeopleIn() + "\t" + GetNoBenefit());
                if ((People.Count >= int.Parse(txtMaxOfPopulation.Text) )|| bBreak) 
                    break;
                Application.DoEvents(); 
            }
            //*************************************************************************
            watch.Stop();
            
            if (bBreak)
                Status("Canceled by user!");
            else
            {
                Status("Proccess compleated.\n Running for :" + string.Format("{0:00}:{1:00}:{2:00}", watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds ) );

            }
            btnStop.Enabled = false;
            btnRun.Enabled = true; 
            btnClear.Enabled = true;
            btnSave.Enabled = true;
            btnChart.Enabled = true;

            btnRun.Focus();
            this.Cursor = Cursors.Default; 
        }

        private void CalcMonthForBenefit()
        {
            List<Person> TmpPeople = new List<Person>();
            iMonthForBenefit = 0; 
            AddPeople(ref TmpPeople, 1);  
            while (true)
            {
                iMonthForBenefit++;  
                Process(ref TmpPeople);
                if (iMonthForBenefit >= int.Parse(txtMinimumTimeToBenefit.Text) && (TmpPeople.Count() - 1) >= int.Parse(txtMinimumOfSubsets.Text))
                    break; 
            } 
        }

        private void Process(ref List<Person> People)
        {
            var toUpdate = People.Where(x => x.Include == true || x.Benefit==false );
            int count = 0;

            
            foreach (var item in toUpdate)
            {    
                if (item.Month ==  int.Parse(txtStopTime.Text))
                    item.Include = false;
                else
                    count++;

                if (item.Month == iMonthForBenefit)
                    item.Benefit = true;
 
                item.Month++;
            }

            if (People.Count + count * int.Parse(txtIncludedPeople.Text) >= int.Parse(txtMaxOfPopulation.Text))
                AddPeople(ref People, int.Parse(txtMaxOfPopulation.Text) - People.Count); 
            else
                AddPeople(ref People,count * int.Parse(txtIncludedPeople.Text)); 
             
        }
        
        private void AddPeople(ref List<Person> PeopleTmp, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Person person = new Person();
                person.Include = true;
                person.Benefit = false;
                person.Month = 0;
                PeopleTmp.Add(person);
            } 
        }

        private int GetNoPeopleIn()
        {
            return People.Where(x => x.Include == true).Count();
        }

        private int GetNoPeopleOut()
        {
            return People.Where(x => x.Include == false).Count();
        }
         
        private int GetNoBenefit()
        {
            return People.Where(x => x.Benefit == true).Count();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            bBreak = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter Sw = new StreamWriter(saveFileDialog1.FileName);
                Sw.Write(txtResults.Text);
                Sw.Close();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            btnClear.Enabled = false;
            btnSave.Enabled = false;
            btnChart.Enabled = false;
            txtResults.Text = "";
            Status("Not Started !");
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            frmChart fChart = new frmChart();
            string []strArr = txtResults.Text.Replace("\r","").Split('\n');
            for (int i = 2; i < strArr.Length; i++)
            {
                string[] strData = strArr[i].Split('\t'); 
                fChart.chart1.Series["Out"].Points.Add(int.Parse(strData[1].ToString()));
                fChart.chart1.Series["In"].Points.Add(int.Parse(strData[2].ToString()));
                fChart.chart1.Series["Benefit"].Points.Add(int.Parse(strData[3].ToString()));

                if (i == strArr.Length-1)
                    fChart.txtSummery.Text = string.Format("Total:{0} \t Number of involved persons:{1}({2}%) \t Number of inactive persons:{3}({4}%) \t Number of benefited persons:{5}({6}%)", txtMaxOfPopulation.Text, strData[2].ToString(), Percent( txtMaxOfPopulation.Text,strData[2].ToString()), strData[1].ToString(),  Percent( txtMaxOfPopulation.Text,strData[1].ToString()), strData[3].ToString(),  Percent( txtMaxOfPopulation.Text,strData[3].ToString()));

            }
              
            fChart.ShowDialog();
        }

        private string Percent(string strTotal, string strNum)
        {
            int t = int.Parse(strTotal);
            int n = int.Parse(strNum);
            double r = n * 100.0 / t;
            return string.Format("{0:0.00}", r);
        }

        private void AddLog(string strLog)
        {
            if (txtResults.Text != "")
                txtResults.Text += "\r\n";
            txtResults.Text += strLog;
        }
         
        private void Status(string strStatus)
        {
            lblStatus.Text = strStatus;
        }

        

    }
}

