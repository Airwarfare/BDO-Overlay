using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace Percent
{
    public partial class BossTimers : MetroFramework.Forms.MetroForm
    {
        static BossTimers main;
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public BossTimers()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
            SetupTable();
            Overlay();
            this.TopMost = true;
            
            metroGrid1.CellEndEdit += UpdateRows;
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            CheckTimers();
        }

        private void CheckTimers()
        {
            for(int i = 0;i<5;i++)
            {
                if(Mainmenu.BossTimers[i] != null)
                {
                    DataGridViewRow row = (DataGridViewRow)metroGrid1.Rows[i];
                    row.Cells[1].Value = Mainmenu.BossTimers[i].Day + " " + Mainmenu.BossTimers[i].Hour + ":" + Mainmenu.BossTimers[i].Minute;
                    if (row.Cells[1].Value.ToString() != "1 0:0")
                    {
                        EstimateBossTime(row.Cells[0].ToString(), Mainmenu.BossTimers[i], i);
                    }
                    else
                    {
                        row.Cells[1].Value = "";
                    }
                }
            }     
        }



        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            main = this;
        }

        private void BossTimers_Load(object sender, EventArgs e)
        {

        }

        public static void OnFocusChangedHandler(object src, AutomationFocusChangedEventArgs args)
        {
            Console.WriteLine("FocusChange");
            UpdateFocus();
        }

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

        private void SetupTable()
        {
            string[] Kzarka = new string[] { "Kzarka" };
            string[] DimTree = new string[] { "Dim Tree" };
            string[] Bheg = new string[] { "Bheg" };
            string[] Mudster = new string[] { "Mudster" };
            string[] RedNose = new string[] { "Red Nose" };

            metroGrid1.Rows.Add(Kzarka);
            metroGrid1.Rows.Add(DimTree);
            metroGrid1.Rows.Add(Bheg);
            metroGrid1.Rows.Add(Mudster);
            metroGrid1.Rows.Add(RedNose);

            
        }

        public void Overlay()
        {
            Process[] processes = Process.GetProcessesByName("BlackDesert64");
            Console.WriteLine(Process.GetProcessesByName("BlackDesert64") + " Boss Timers");

            Process p = processes.FirstOrDefault();
            IntPtr windowHandle;
            if (p != null)
            {


                windowHandle = p.MainWindowHandle;


                RECT rect = new RECT();
                GetWindowRect(windowHandle, ref rect);
                Console.WriteLine("Minimized: " + IsIconic(windowHandle));
                if (rect.Left == -32000)
                {

                }
                else
                {
                    this.WindowState = FormWindowState.Normal;

                    //this.Location = new Point(rect.Left, rect.Top);
                    //this.Size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);

                    //this.BackColor = Color.Black;
                    //this.TransparencyKey = Color.Black;
                    //this.FormBorderStyle = FormBorderStyle.None;
                }

            }


        }

        private void metroGrid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UpdateRows(object sender, DataGridViewCellEventArgs es)
        {
            Console.WriteLine("EditFinshed");
            DataGridViewRow row = (DataGridViewRow)metroGrid1.Rows[es.RowIndex];
            string a = row.Cells[1].Value.ToString();
            DateTime time = new DateTime(); // Passed result if succeed 
            if(a == null)
            {
                return;
            }
            if (DateTime.TryParseExact(a, "dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out time))
            {
                Console.WriteLine("pass");
                DateTime newDate = DateTime.Now;
                DateTime convert = Convert.ToDateTime(time);
                convert = convert.AddMonths(newDate.Month - 1);
                Mainmenu.BossTimers[es.RowIndex] = convert;
                EstimateBossTime(row.Cells[0].ToString(), convert, es.RowIndex);
            }
            else
            {
                Console.WriteLine("fail");
            }
            
        }

        private void UpdateRowHere(int rowIndex)
        {
            DataGridViewRow row = (DataGridViewRow)metroGrid1.Rows[rowIndex];
            row.Cells[2].Value = "D:" + 2 + "H:" + 2 + "M:" + 2;
        }

        
        public void EstimateBossTime(string boss, DateTime t, int rowIndex)
        {
            DateTime min;
            DateTime max;

            DataGridViewRow row = (DataGridViewRow)metroGrid1.Rows[rowIndex];
            if (boss == "Nouver" || boss == "Kzarkha")
            {
                if (boss == "Kzarkha")
                {
                    //Window 24-35
                    DateTime est = t;
                    Random r = new Random();
                    int random = r.Next(0, 660);
                    Console.WriteLine("Random: " + random);
                    est = est.AddMinutes(1440);
                    min = est;
                    est = est.AddMinutes(random);
                    DateTime tmax = t;
                    tmax = tmax.AddMinutes(2100);
                    max = tmax;
                    Console.WriteLine(tmax);

                    row.Cells[2].Value = "D:" + est.Day + "H:" + est.Hour + "M:" + est.Minute;
                    row.Cells[3].Value = "D:" + min.Day + "H:" + min.Hour + "M:" + min.Minute;
                    row.Cells[4].Value = "D:" + max.Day + "H:" + max.Hour + "M:" + max.Minute;
                    DateTime currentDate = DateTime.Now;


                    Console.WriteLine(currentDate);

                    int result = DateTime.Compare(currentDate, t.AddMinutes(1440));
                    int result2 = DateTime.Compare(currentDate, tmax);
                    Console.WriteLine(result);
                    Console.WriteLine(result2);
                    
                    if (result > 0 && result2 < 0)
                    {
                        row.Cells[5].Value = "True";
                    }
                    else
                    {
                        row.Cells[5].Value = "False";
                    }
                }
                else if (boss == "Nouver")
                {
                    DateTime est = t;
                    Console.WriteLine(t);
                    Console.WriteLine(est);
                    Random r = new Random();
                    int random = r.Next(0, 660);
                    Console.WriteLine("Random: " + random);
                    
                    
                    est = est.AddMinutes(1440);
                    min = est;
                    est = est.AddMinutes(random);
                    DateTime tmax = t;
                    tmax = tmax.AddMinutes(2100);
                    max = tmax;
                    Console.WriteLine(est);
                    row.Cells[2].Value = "D:" + est.Day + "H:" + est.Hour + "M:" + est.Minute;
                    row.Cells[3].Value = "D:" + min.Day + "H:" + min.Hour + "M:" + min.Minute;
                    row.Cells[4].Value = "D:" + max.Day + "H:" + max.Hour + "M:" + max.Minute;
                    DateTime currentDate = DateTime.Now;

                    Console.WriteLine(currentDate);

                    
                    Console.WriteLine(tmax);
                    int result = DateTime.Compare(currentDate, t.AddMinutes(1440));
                    int result2 = DateTime.Compare(currentDate, tmax);
                    if (result > 0 && result2 < 0)
                    {
                        row.Cells[5].Value = "True";
                    }
                    else
                    {
                        row.Cells[5].Value = "False";
                    }
                }
            }
            else
            {
                //10-19
                DateTime est = t;
                Console.WriteLine(t);
                Console.WriteLine(est);
                Random r = new Random();
                int random = r.Next(0, 540);
                Console.WriteLine("Random: " + random);


                est = est.AddMinutes(600);
                min = est;
                est = est.AddMinutes(random);
                DateTime tmax = t;
                tmax = tmax.AddMinutes(1140);
                max = tmax;
                Console.WriteLine(est);
                row.Cells[2].Value = "D:" + est.Day + "H:" + est.Hour + "M:" + est.Minute;
                row.Cells[3].Value = "D:" + min.Day + "H:" + min.Hour + "M:" + min.Minute;
                row.Cells[4].Value = "D:" + max.Day + "H:" + max.Hour + "M:" + max.Minute;
                DateTime currentDate = DateTime.Now;
                Console.WriteLine(currentDate);

                tmax = tmax.AddMinutes(1140);
                Console.WriteLine(tmax);
                int result = DateTime.Compare(currentDate, t.AddMinutes(600));
                int result2 = DateTime.Compare(currentDate, tmax);

                if (result > 0 && result2 < 0)
                {
                    Console.WriteLine("TTT");
                    row.Cells[5].Value = "True";
                }
                else
                {
                    Console.WriteLine("TTT");
                    row.Cells[5].Value = "False";
                }
            }
        }
       


    }
}
