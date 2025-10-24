using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PSALE
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          Application.Run(new MainForm("شرکت مهندسی پارسینا پردازان آریا","Zoro","1390",1,true,false,"Data Source=.;Initial Catalog=PSALE_XX_XXXX;Integrated Security=True"));

        }
    }
}
