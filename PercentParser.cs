using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace Percent
{
    public partial class PercentParser : MetroFramework.Forms.MetroForm
    {
        int yellow = 0;

        static PercentParser main;
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        public PercentParser()
        {
            InitializeComponent();
            main = this;
            PercentTimer();
            this.StyleManager = metroStyleManager1;
        }

        private void PercentParser_Load(object sender, EventArgs e)
        {

        }

        private void metroLabel1_Click(object sender, EventArgs e)
        {
            
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

        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }

        private System.Windows.Forms.Timer timer2;
        public void PercentTimer()
        {
            timer2 = new System.Windows.Forms.Timer();
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = 10000; // in miliseconds
            timer2.Start();
            Console.WriteLine("Started Percent Timer");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            timer2.Stop();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Low Mode");
            yellow = 0;
            int x = 855;
            do
            {
                Console.WriteLine("Loop ");
                Point newPoint = new Point();
                newPoint.X = x;
                newPoint.Y = 43;
                var c = GetColorAt(newPoint);
                if (c.R == 187 && c.G == 133 && c.B == 23)
                {
                    yellow += 1;
                }
                if (c.R == 197 && c.G == 193 && c.B == 185)
                {
                    yellow += 1;
                }

                if (x >= 1065)
                {
                    break;
                }
                x += 10;
                Thread.Yield();
            } while (true);

            if (yellow == 0)
            {
                Console.WriteLine("Target: None");
                metroLabel2.Text = "Target: None";
                metroLabel2.Refresh();
            }
            else if (yellow > 0)
            {
                metroLabel1.Text = "Percentage: " + (yellow * 100) / 21 + "%";
                metroLabel2.Text = "Target: Something";
                metroLabel1.Refresh();
                metroLabel2.Refresh();
            }

        }

        private void metroLabel2_Click(object sender, EventArgs e)
        {

        }
    }
}
