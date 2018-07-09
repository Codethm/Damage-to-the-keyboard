using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Damage_to_the_keyboard_2._0
{
    public partial class Form1 : Form
    {
        decimal Time = 0;
        decimal avg = 0;
        int count = 0;
        Timer T = new Timer();
        Timer Col = new Timer();
        int keypersec = 0;
        Timer sec = new Timer();

        public Form1()
        {
            InitializeComponent();
            T.Interval = 90;
            Col.Interval = 45;
            sec.Interval = 900;
            T.Tick += T_Tick;
            Col.Tick += Col_Tick;
            sec.Tick += Sec_Tick;
        }

        private void start(object sender, KeyEventArgs e)
        {
            label5.Hide();
            sec.Start();
            T.Start();
            chart1.Series["Key/Sec"].Points.AddXY(0.05, 0);
            chart1.Series["AVG."].Points.AddXY(0.05, 0);

            this.KeyDown -= start;
            this.KeyDown += GO;

        }
        private void GO(object sender, KeyEventArgs e)
        {
            count++;
            keypersec++;
            this.BackColor = Color.Orange;
            Col.Start();
        }
        private void End(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                Time = 0;
                avg = 0;
                count = 0;
                label2.Text = 30.ToString();
                label3.Text = 0.ToString();
                label1.Text = 0.ToString();
                panel1.Width = 40;
                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                this.KeyDown -= End;
                this.KeyDown += start;
            }
        }


        private void Sec_Tick(object sender, EventArgs e)
        {
            chart1.Series["Key/Sec"].Points.AddXY(Time, keypersec);
            keypersec = 0;
        }
        private void T_Tick(object sender, EventArgs e)
        {
            Time += 0.1m;
            avg = count / (Time);

            label1.Text = avg.ToString();
            label2.Text = (30 - Time).ToString();
            label3.Text = count.ToString();

            chart1.Series["AVG."].Points.AddXY(Time, avg);

            panel1.Width = (int)(avg * 15);
            if (avg > 35)
                panel1.BackColor = Color.Red;
            else if (avg > 27)
                panel1.BackColor = Color.Yellow;
            else if (avg > 22)
                panel1.BackColor = Color.GreenYellow;
            else if (avg > 10)
                panel1.BackColor = Color.Green;
            if ((30 - Time) == 0)
            {
                label5.Show();
                T.Stop();
                sec.Stop();
                this.KeyDown += End;
                this.KeyDown -= GO;
            }
        }
        private void Col_Tick(object sender, EventArgs e)
        {
            this.BackColor = Color.Blue;
            Col.Stop();
        }
    }
}
