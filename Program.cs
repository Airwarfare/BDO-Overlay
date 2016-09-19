using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Automation;

namespace Percent
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;
            Automation.AddAutomationFocusChangedEventHandler(Mainmenu.OnFocusChangedHandler);
            Automation.AddAutomationFocusChangedEventHandler(Settings.OnFocusChangedHandler);
            Automation.AddAutomationFocusChangedEventHandler(BossTimers.OnFocusChangedHandler);
            Automation.AddAutomationFocusChangedEventHandler(PercentParser.OnFocusChangedHandler);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            start = new Loading();
            start.TopMost = true;
            Application.Run(start);
        }


        public static Loading start;
    }
}
