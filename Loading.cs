using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Percent
{
    public partial class Loading : MetroFramework.Forms.MetroForm
    {
        public Loading()
        {
            InitializeComponent();
            this.metroProgressSpinner1.ForeColor = Color.Red;
            StartProgram();
        }

        private void StartProgram()
        {
            InitTimer();
        }

        private Timer timer1;
        public void InitTimer()
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 2500; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadP();
        }

        private void LoadP()
        {
            timer1.Stop();
            this.Visible = false;
            Startup s = new Startup();
            s.Show();         
        }

        private void Loading_Load(object sender, EventArgs e)
        {

        }

        private void metroProgressSpinner1_Click(object sender, EventArgs e)
        {
            
        }

        private void htmlLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
