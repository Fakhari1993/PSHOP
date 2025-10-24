using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace PSHOP._07_Services.Reports
{
    public partial class ReportForm : Form
    {
        Int16 _FormNo = 0;
        DataTable _Table1 = new DataTable();
        DataTable _Table2 = new DataTable();
        DataTable _Table3 = new DataTable();
        string _Param1,_Param2;

        public ReportForm(Int16 FormNo)
        {
            InitializeComponent();
            _FormNo = FormNo;
        }
        public ReportForm(Int16 FormNo,DataTable Table)
        {
            InitializeComponent();
            _FormNo = FormNo;
            _Table1 = Table;
        }
        public ReportForm(Int16 FormNo, DataTable Table1,DataTable Table2,DataTable Table3,string Param1,string Param2)
        {
            InitializeComponent();
            _FormNo = FormNo;
            _Table1 = Table1;
            _Table2 = Table2;
            _Table3 = Table3;
            _Param1 = Param1;
            _Param2 = Param2;
        }
        private void ReportForm_Load(object sender, EventArgs e)
        {
            //گزارش سرویسهای معرفی شده
            if (_FormNo == 1)
            {
                _07_Services.Reports.Rpt01_Services Rpt = new Rpt01_Services();
                Rpt.SetDataSource(_Table1);
                Rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                crystalReportViewer1.ReportSource = Rpt;
            }
            else if (_FormNo == 2)
            {
                uiGroupBox1.Visible = true;
                _07_Services.Reports.Rpt02_PrintFactor Rpt = new Rpt02_PrintFactor();
                Rpt.SetDataSource(_Table1);
                DataTable OrgInfo=Class_BasicOperation.LogoTable();
                if (!chk_Logo.Checked)
                    Rpt.Subreports[0].SetDataSource(OrgInfo);
                else Rpt.Subreports[0].SetDataSource(OrgInfo.Clone());
                Rpt.Subreports["x1"].SetDataSource(OrgInfo);
                Rpt.Subreports["x2"].SetDataSource(_Table2);
                Rpt.Subreports["x3"].SetDataSource(_Table3);
                Rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = Rpt;
            }
            else if (_FormNo == 3)
            {
                DataSet DataSet1 = new DataSet();
                DataSet1.Tables.Add(_Table1);
                DataSet1.Tables.Add(_Table2);
                _07_Services.Reports.Rpt03_Report_Doc Rpt = new Rpt03_Report_Doc();
                Rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                Rpt.SetDataSource(DataSet1);
                Rpt.Subreports["x1"].SetDataSource(_Table3);
                Rpt.SetParameterValue("Param1", _Param1);
                Rpt.SetParameterValue("Param2", _Param2);
                crystalReportViewer1.ReportSource = Rpt;

            }
            else if (_FormNo == 4)
            {
                _07_Services.Reports.Rpt04_Report_Customers Rpt = new Rpt04_Report_Customers();
                Rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                Rpt.SetDataSource(_Table1);
                Rpt.SetParameterValue("Param1", _Param1);
                Rpt.SetParameterValue("Param2", _Param2);
                crystalReportViewer1.ReportSource = Rpt;

            }
            else if (_FormNo == 5)
            {
                _07_Services.Reports.Rpt05_Report_Services Rpt = new Rpt05_Report_Services();
                Rpt.SetDataSource(_Table1);
                Rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                Rpt.SetParameterValue("Param1", _Param1);
                Rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = Rpt;
            }
            else if (_FormNo == 6)
            {
                _07_Services.Reports.Rpt06_ExtraReductionList Rpt = new Rpt06_ExtraReductionList();
                Rpt.SetDataSource(_Table1);
                Rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                Rpt.SetParameterValue("Param1", _Param1);
                Rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = Rpt;
            }
                //صورتحساب مشتری
            else if (_FormNo == 7)
            {
                _07_Services.Reports.Rpt07_Sale_CustomerBill Rpt = new Rpt07_Sale_CustomerBill();
                Rpt.SetDataSource(_Table1);
                Rpt.SetParameterValue("Param1", _Param1);
                Rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = Rpt;
            }

        }

        private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void chk_Logo_CheckedChanged(object sender, EventArgs e)
        {
            uiGroupBox1.Visible = true;
            _07_Services.Reports.Rpt02_PrintFactor Rpt = new Rpt02_PrintFactor();
            Rpt.SetDataSource(_Table1);
            DataTable OrgInfo = Class_BasicOperation.LogoTable();
            if (!chk_Logo.Checked)
                Rpt.Subreports[0].SetDataSource(OrgInfo);
            else Rpt.Subreports[0].SetDataSource(OrgInfo.Clone());
            Rpt.Subreports["x1"].SetDataSource(OrgInfo);
            Rpt.Subreports["x2"].SetDataSource(_Table2);
            Rpt.Subreports["x3"].SetDataSource(_Table3);
            Rpt.SetParameterValue("Param1", _Param1);
            crystalReportViewer1.ReportSource = Rpt;
        }
    }
}
