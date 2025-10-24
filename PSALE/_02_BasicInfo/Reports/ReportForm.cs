using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace PSHOP._02_BasicInfo.Reports
{
    public partial class ReportForm : Form
    {
        Int16 _FormNo = 0;
        DataTable _Table = new DataTable();

        public ReportForm(Int16 FormNo)
        {
            InitializeComponent();
            _FormNo = FormNo;
        }
        public ReportForm(Int16 FormNo,DataTable Table)
        {
            InitializeComponent();
            _FormNo = FormNo;
            _Table = Table;
        }
        private void ReportForm_Load(object sender, EventArgs e)
        {
            if (_FormNo == 1)
            {
                _02_BasicInfo.Reports.Rpt_01_Gifts Rpt = new Rpt_01_Gifts();
                Rpt.SetDataSource(_Table);
                Rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                crystalReportViewer1.ReportSource = Rpt;
            }
                //گزارش قیمت گذاری کالاهای فروش
            else if (_FormNo == 2)
            {
                _02_BasicInfo.Reports.Rpt_04_Pricing Rpt = new Rpt_04_Pricing();
                Rpt.SetDataSource(_Table);
                Rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                crystalReportViewer1.ReportSource = Rpt;
            }
        }

    }
}
