using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace PSHOP._06_Reports._02_Sale
{
    public partial class Form05_SaleReport_Monthly_Value_Customer_Visitor : Form
    {
        public Form05_SaleReport_Monthly_Value_Customer_Visitor()
        {
            InitializeComponent();
        }
        Classes.Class_Documents ClsDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);


        private void Form04_SaleReport_NumericMonthly_Visitor_Load(object sender, EventArgs e)
        {
            DataTable PersonTable = ClsDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX_Visitors.DropDowns["Seller1"].SetDataBinding(PersonTable, "");
            gridEX_Visitors.DropDowns["Seller2"].SetDataBinding(PersonTable, "");
            gridEX_Goods.DropDowns["CustomerCode"].SetDataBinding(PersonTable, "");
            gridEX_Goods.DropDowns["CustomerName"].SetDataBinding(PersonTable, "");

            rpt_Sale_MonthlyNumeric_VisitorTableAdapter.Fill(dataSet_Report.Rpt_Sale_MonthlyNumeric_Visitor, Class_BasicOperation._Year);
            rpt_Sale_Monthly_Value_Visitor_CustomerTableAdapter.Fill(dataSet_Report.Rpt_Sale_Monthly_Value_Visitor_Customer, Class_BasicOperation._Year);

            crystalReportViewer1.BackColor = Color.White;

        }

        private void mnu_Excel_Table_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void mnu_Send_Chart_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.ExportReport();
        }


        private void mnu_PrintAll_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_Monthly_Numeric_Table.Clone();
            foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_Visitors.GetCheckedRows())
            {
                gridEX_Visitors.MoveTo(Row);
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                {
                    Table.Rows.Add(item.Cells["CustomerCode"].Text.ToString(),
                        item.Cells["CustomerName"].Text,
                        item.Cells["Far"].Value.ToString(), item.Cells["Ord"].Value.ToString(),
                        item.Cells["Khordad"].Value.ToString(), item.Cells["Tir"].Value.ToString(),
                        item.Cells["Mordad"].Value.ToString(), item.Cells["Shahr"].Value.ToString(),
                        item.Cells["Mehr"].Value.ToString(), item.Cells["Aban"].Value.ToString(),
                        item.Cells["Azar"].Value.ToString(), item.Cells["Dey"].Value.ToString(),
                        item.Cells["Bahman"].Value.ToString(), item.Cells["Esf"].Value.ToString(),
                        Row.Cells["VisitorId"].Value.ToString(),
                        Row.Cells["VisitorCode"].Text.ToString(),
                        Row.Cells["VisitorName"].Text.ToString());
                }
            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 42, Class_BasicOperation._Year," ");
                frm.ShowDialog();
            }
        }

        private void Form04_SaleReport_NumericMonthly_Goods_Visitor_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode== Keys.F)
            mnu_PrintAll_Click(sender, e);
        }

        private void gridEX_Goods_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
                try
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i = 3; i <= 14; i++)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, e.Row.Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار ماهیانه میزان فروش ریالی   " + e.Row.Cells["CustomerName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
                catch
                {
                }
        }

    }
}
