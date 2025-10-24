using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PSHOP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm("ابزار مارکت زارعی",
                    "admin", "1399",13,false, true,
                    //  "Admin", "1395", 5, false, true,
                    //"Data Source=PC-02;Initial Catalog=PSALE_1_1397;Integrated Security=True; Connect Timeout=60"));
                // "Data Source=.;Initial Catalog=PSALE_1_1399;Persist Security Info=True;User ID=sa; password=Pars@63"));
                 "Data Source=P-RDP\\PERP2017;Initial Catalog=PSALE_13_1399;Persist Security Info=True;User ID=sa; password=Pars@63"));

              //  AND (NOT(Center in (" + String.Join(",", clCheck.Rows(Class_BasicOperation._UserName, 3)) + @")) OR Center IS NULL)
                          // AND (NOT(Project in (" + String.Join(",", clCheck.Rows(Class_BasicOperation._UserName, 4)) + @")) OR Project  IS NULL)";

            }
            catch
            {
            }
        }
    }
}
