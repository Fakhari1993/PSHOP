using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace PSHOP._06_Reports._01_Orders
{
  
    public partial class Form02_FinalPrice_Print : Form
    {  
        DataTable _Table = new DataTable();
        DataTable _Table2 = new DataTable();
        string _Param1, _Param2,_Param3;
      
        public Form02_FinalPrice_Print(DataTable Table, string Param1,string Param2)
        {
            InitializeComponent();
            _Table = Table;
            _Param1 = Param1;
            _Param2 = Param2;
        }
     
    
        private void Form01_ReportForm_Load(object sender, EventArgs e)
        {
            //نمایش گزارش لیست آخرین قیمت محصولات
            
            
        }

        private void bt_First_Click(object sender, EventArgs e)
        {
            _06_Reports._01_Orders.Report02_FinalPriceList_2 rpt = new _01_Orders.Report02_FinalPriceList_2();
            rpt.SetDataSource(_Table);
            rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
            crystalReportViewer1.ReportSource = rpt;
        }

        private void bt_Second_Click(object sender, EventArgs e)
        {
            _06_Reports._01_Orders.Report02_FinalPriceList rpt = new _01_Orders.Report02_FinalPriceList();
            rpt.SetDataSource(_Table);
            rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
            crystalReportViewer1.ReportSource = rpt;
        }

       
    }
}
