using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Percent
{
    class Logic
    {
        public static void CloseAll()
        {
            for(int i =0;i<Application.OpenForms.Count;i++)
            {
                Application.OpenForms[i].Visible = false;
            }
        }

        public static void OpenIt()
        {
            for(int i =0;i<Application.OpenForms.Count; i++)
            {
                if(Application.OpenForms[i].Name == "Mainmenu")
                {
                    Application.OpenForms[i].Visible = true;
                }
            }
        }
    }
}
