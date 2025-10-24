using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Design;
using Stimulsoft.Database;
namespace PSHOP._06_Reports
{

    public partial class Form01_StimulateReportForm : Form
    {
        DataTable _Table = new DataTable();
        DataTable _Table2 = new DataTable();
        int _FormNumber = 0;
        string _Param1, _Param2, _Param3;
        public Form01_StimulateReportForm(DataTable Table, int FormNumber)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table;
        }
        public Form01_StimulateReportForm(DataTable Table, int FormNumber, string Param1, string Param2)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table;
            _Param1 = Param1;
            _Param2 = Param2;
        }
        public Form01_StimulateReportForm(DataTable Table1, DataTable Table2, int FormNumber, string Param1, string Param2)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table1;
            _Table2 = Table2;
            _Param1 = Param1;
            _Param2 = Param2;
        }
        public Form01_StimulateReportForm(DataTable Table1, int FormNumber, string Param1, string Param2, string Param3)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table1;
            _Param1 = Param1;
            _Param2 = Param2;
            _Param3 = Param3;
        }
        private void Form01_ReportForm_Load(object sender, EventArgs e)
        {

            if (_FormNumber == 1)
            {

                try
                {
                    this.Cursor = Cursors.WaitCursor;


                    StiReport stireport = new StiReport();
                    stireport.Load("Rpt25_GoodReportByVisitors.mrt");


                    stireport.Compile();
                    StiOptions.Viewer.AllowUseDragDrop = false;


                    stireport.RegData("DataTable4", _Table);



                    stireport.RegData("Table_000_OrgInfo", Class_BasicOperation.LogoTable());









                    stireport["Param1"] = _Param1;
                    stireport["Param2"] = _Param2;
                    stireport["Param3"] = _Param3;

                    this.Cursor = Cursors.Default;
                    stireport.Select();
                    stireport.Render(false);
                    stiViewerControl1.Report = stireport;
                    stiViewerControl1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Cursor = Cursors.Default;

                }

            }

            if (_FormNumber == 2)
            {

                try
                {
                    this.Cursor = Cursors.WaitCursor;


                    StiReport stireport = new StiReport();
                    stireport.Load("GoodList.mrt");


                    stireport.Compile();
                    StiOptions.Viewer.AllowUseDragDrop = false;
                    stireport.RegData("DataTable1", _Table);
                    stireport.RegData("Table_000_OrgInfo", Class_BasicOperation.LogoTable());
                    this.Cursor = Cursors.Default;
                    stireport.Select();
                    stireport.Render(false);
                    stiViewerControl1.Report = stireport;
                    stiViewerControl1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Cursor = Cursors.Default;

                }

            }

        }

        private void Form01_ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void btn_view_Click(object sender, EventArgs e)
        {
            if (_FormNumber == 1)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;


                    StiReport stireport = new StiReport();
                    stireport.Load("Rpt25_GoodReportByVisitors.mrt");


                    stireport.Compile();
                    StiOptions.Viewer.AllowUseDragDrop = false;


                    stireport.RegData("DataTable4", _Table);



                    stireport.RegData("Table_000_OrgInfo", Class_BasicOperation.LogoTable());









                    stireport["Param1"] = _Param1;
                    stireport["Param2"] = _Param2;
                    stireport["Param3"] = _Param3;

                    this.Cursor = Cursors.Default;
                    stireport.Select();
                    stireport.Render(false);
                    stiViewerControl1.Report = stireport;
                    stiViewerControl1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Cursor = Cursors.Default;

                }
            }
            if (_FormNumber == 2)
            {

                try
                {
                    this.Cursor = Cursors.WaitCursor;


                    StiReport stireport = new StiReport();
                    stireport.Load("GoodList.mrt");


                    stireport.Compile();
                    StiOptions.Viewer.AllowUseDragDrop = false;
                    stireport.RegData("DataTable1", _Table);
                    stireport.RegData("Table_000_OrgInfo", Class_BasicOperation.LogoTable());
                    this.Cursor = Cursors.Default;
                    stireport.Select();
                    stireport.Render(false);
                    stiViewerControl1.Report = stireport;
                    stiViewerControl1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.Cursor = Cursors.Default;

                }

            }
        }

        private void btn_Design_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            if (_FormNumber == 1)

                stireport.Load("Rpt25_GoodReportByVisitors.mrt");
            if (_FormNumber == 2)
                stireport.Load("GoodList.mrt");

            stireport.Design();

        }
    }
}
