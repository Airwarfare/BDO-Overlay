using MetroFramework.Forms;
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

namespace Percent
{
    public partial class Settings : MetroFramework.Forms.MetroForm
    {
        static Settings main;
        
        public Settings()
        {
            InitializeComponent();
            main = this;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            this.StyleManager = metroStyleManager1;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        public static void UpdateFocus()
        {
            Process[] processes = Process.GetProcessesByName("BlackDesert64");
            Process p = processes.FirstOrDefault();
            Process pp = Process.GetCurrentProcess();
            if (p != null)
            {

            }
            bool isInFocus = ApplicationIsActivated(p.Id);
            bool isInFocusN = ApplicationIsActivated(pp.Id);
            Console.WriteLine("BDO: " + isInFocus + " MY: " + isInFocusN);
            if (isInFocus == false && isInFocusN == false)
            {
                Console.WriteLine("Minimizing");
                Minimize();
            }
            else
            {
                Console.WriteLine("Normal State");
                Logic.OpenIt();
            }
        }

        public static void OnFocusChangedHandler(object src, AutomationFocusChangedEventArgs args)
        {
            UpdateFocus();
        }

        public static void Minimize()
        {
            Logic.CloseAll();
        }

        public static bool ApplicationIsActivated(int n)
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            int procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);
            if (activeProcId == n)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
