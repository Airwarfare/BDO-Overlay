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
    public partial class Mainmenu : MetroFramework.Forms.MetroForm
    {

        public static DateTime[] BossTimers = new DateTime[5];

        static Mainmenu main = new Mainmenu();

        static Form Current = MetroForm.ActiveForm;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        const int MYACTION_HOTKEY_ID = 1;

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

        public Mainmenu()
        {
            InitializeComponent();
            this.StyleManager = metroStyleManager1;
            this.TopMost = true;
            Overlay();
            main = this;
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID, 2, (int)Keys.Insert);
        }

        private void Mainmenu_Load(object sender, EventArgs e)
        {

        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

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

        public void Overlay()
        {
            Process[] processes = Process.GetProcessesByName("BlackDesert64");
            Console.WriteLine(processes.FirstOrDefault() + " Main menu");
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

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                Console.WriteLine("VISIBLE?" + metroTile1.Visible);
                Console.WriteLine(Application.OpenForms[1] + "    " + Application.OpenForms[2] + " | " + Application.OpenForms[3] + " | " + Application.OpenForms[4]);
                if (Application.OpenForms[4].Visible == false)
                {
                    Logic.OpenIt();
                }
                else if (Application.OpenForms[4].Visible == true)
                {
                    Logic.CloseAll();
                }

            }
            base.WndProc(ref m);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            Application.Exit();
        }

        private const int WS_SYSMENU = 0x80000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WS_SYSMENU;
                return cp;
                
            }
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            BossTimers bt = new BossTimers();
            bt.TopMost = true;
            bt.Show();
        }

        private void metroTile2_Click(object sender, EventArgs e)
        {
            PercentParser pp = new PercentParser();
            pp.TopMost = true;
            pp.Show();
        }

        private void metroTile3_Click(object sender, EventArgs e)
        {
            Settings s = new Settings();
            s.TopMost = true;
            s.Show();
        }

        private void metroTile4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
}
