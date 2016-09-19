using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework;

namespace Percent
{
    public partial class Startup : MetroFramework.Forms.MetroForm
    {

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public Startup()
        {
            InitializeComponent();
            this.DisplayHeader = false;
            this.StyleManager = metroStyleManager1;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void Startup_Load(object sender, EventArgs e)
        {

        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("BlackDesert64");
            Process p = processes.FirstOrDefault();
            if (p != null)
            {

                Mainmenu n = new Mainmenu();

                Console.WriteLine(Application.OpenForms[0].ToString());
                this.Visible = false;
                ShowWindow(processes[0].MainWindowHandle, 9);
                SetForegroundWindow(processes[0].MainWindowHandle);
                n.Show();
            }
            else
            {
                MetroMessageBox.Show(this, "Error: Black Desert Online is not open!");
                Application.Exit();
            }
        }
    }
}
