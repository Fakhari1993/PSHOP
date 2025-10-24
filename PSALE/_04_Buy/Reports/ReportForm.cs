using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._04_Buy.CrystalReports
{
    public partial class ReportForm : Form
    {
        DataTable _Table = new DataTable();
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        int _FormNumber = 0;
        public ReportForm(int FormNumber,DataTable Table)
        {
            InitializeComponent();
            _Table = Table;
            _FormNumber = FormNumber;

        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            if (_FormNumber == 1)
            {
                _04_Buy.Reports.Rpt_01_PrintRequest rpt = new _04_Buy.Reports.Rpt_01_PrintRequest();
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetDataSource(_Table);
                crystalReportViewer1.ReportSource = rpt;
            }
        }
    }
}
