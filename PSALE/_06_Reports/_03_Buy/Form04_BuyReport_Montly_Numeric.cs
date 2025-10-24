using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;

namespace PSHOP._06_Reports._03_Buy
{
    public partial class Form04_BuyReport_Montly_Numeric : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        public Form04_BuyReport_Montly_Numeric()
        {
            InitializeComponent();
        }

        private void Form04_SaleReport_NumericMonthly_Load(object sender, EventArgs e)
        {
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     Step1.Year, Step1.GoodID, SUM(Step1.Far) AS Far, SUM(Step1.Ord) AS Ord, SUM(Step1.Khord) AS Khordad, SUM(Step1.Tir) AS Tir, SUM(Step1.Mordad) AS Mordad, 
            SUM(Step1.Shahr) AS Shahr, SUM(Step1.Mehr) AS Mehr, SUM(Step1.Aban) AS Aban, SUM(Step1.Azar) AS Azar, SUM(Step1.Dey) AS Dey, SUM(Step1.Bahman) 
            AS Bahman, SUM(Step1.Esf) AS Esf, GoodTable.column01 AS GoodCode, GoodTable.column02 AS GoodName
            FROM         (SELECT     TOP (100) PERCENT SUBSTRING(dbo.Table_015_BuyFactor.column02, 1, 4) AS Year, SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 2) AS Month, 
            dbo.Table_016_Child1_BuyFactor.column02 AS GoodID, SUM(dbo.Table_016_Child1_BuyFactor.column07) AS TotalNumber, 
            CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 2) = '01' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) ELSE 0 END AS Far, 
            CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) ELSE 0 END AS Ord, 
            CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 2) = '03' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) ELSE 0 END AS Khord,
            CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 2) = '04' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) ELSE 0 END AS Tir, 
            CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 2) = '05' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) 
            ELSE 0 END AS Mordad, CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 2) 
            = '06' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) ELSE 0 END AS Shahr, CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 
            2) = '07' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) ELSE 0 END AS Mehr, CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 
            2) = '08' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) ELSE 0 END AS Aban, CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 
            2) = '09' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) ELSE 0 END AS Azar, CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 
            2) = '10' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) ELSE 0 END AS Dey, CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02, 6, 
            2) = '11' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) ELSE 0 END AS Bahman, CASE WHEN SUBSTRING(dbo.Table_015_BuyFactor.column02,
            6, 2) = '12' THEN SUM(dbo.Table_016_Child1_BuyFactor.column07) ELSE 0 END AS Esf
            FROM          dbo.Table_015_BuyFactor INNER JOIN
            dbo.Table_016_Child1_BuyFactor ON dbo.Table_015_BuyFactor.columnid = dbo.Table_016_Child1_BuyFactor.column01
            GROUP BY SUBSTRING(dbo.Table_015_BuyFactor.column02, 1, 4), dbo.Table_016_Child1_BuyFactor.column02, SUBSTRING(dbo.Table_015_BuyFactor.column02, 
            6, 2)
            ORDER BY Year, Month, GoodID) AS Step1 LEFT OUTER JOIN
            (SELECT     columnid, column01, column02
            FROM          {1}.dbo.table_004_CommodityAndIngredients) AS GoodTable ON Step1.GoodID = GoodTable.columnid
            WHERE     (Step1.Year = '{0}')
            GROUP BY Step1.Year, Step1.GoodID, GoodTable.column01, GoodTable.column02", ConSale);
            Adapter.SelectCommand.CommandText = String.Format(Adapter.SelectCommand.CommandText, Class_BasicOperation._Year,ConWare.Database);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            gridEX_Goods.DataSource = Table;
            BindingSource BindSource = new BindingSource();
            BindSource.DataSource = Table;
            bindingNavigator1.BindingSource = BindSource;

            crystalReportViewer1.BackColor = Color.White;
        }

        private void mnu_PrintTable_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_Monthly_Numeric_Table.Clone();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
            {
                Table.Rows.Add(item.Cells["GoodCode"].Value.ToString(), item.Cells["GoodName"].Value.ToString(),
                    item.Cells["Far"].Value.ToString(), item.Cells["Ord"].Value.ToString(),
                    item.Cells["Khordad"].Value.ToString(), item.Cells["Tir"].Value.ToString(),
                    item.Cells["Mordad"].Value.ToString(), item.Cells["Shahr"].Value.ToString(),
                    item.Cells["Mehr"].Value.ToString(), item.Cells["Aban"].Value.ToString(),
                    item.Cells["Azar"].Value.ToString(), item.Cells["Dey"].Value.ToString(),
                    item.Cells["Bahman"].Value.ToString(), item.Cells["Esf"].Value.ToString());
            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 19);
                frm.ShowDialog();
            }
        }

        private void mnu_PrintChart_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.PrintReport();
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

        private void gridEX_Accounts_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
            {
                DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                for (int i = 3; i <=14; i++)
                {
                    Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, e.Row.Cells[i].Value.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new  _02_Sale.Rpt05_Monthly_Charts();
                    rpt.SetDataSource(Table);
                    TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                    Title.Text = "نمودار ماهیانه تعداد خریداری شده کالای  " + e.Row.Cells["GoodName"].Text;
                    crystalReportViewer1.ReportSource = rpt;
                    uiPanel0.AutoHideActive = true;
                }
            }
            catch 
            {
            }
        }

        private void Form04_SaleReport_NumericMonthly_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.E)
                cmb_ExportToExcel.ShowDropDown();
            else if (e.Control && e.KeyCode == Keys.P)
                cmb_Print.ShowDropDown();
            else if (e.Control && e.KeyCode == Keys.F)
                gridEX_Goods.Row = gridEX_Goods.FilterRow.Position;
        }

        private void Form04_SaleReport_NumericMonthly_Activated(object sender, EventArgs e)
        {
            gridEX_Goods.Row = gridEX_Goods.FilterRow.Position;
        }

        private void mnu_PrintBoth_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                for (int i = 3; i <= 14; i++)
                {
                    Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetRow().Cells[i].Value.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 20, gridEX_Goods.GetRow().Cells["GoodCode"].Text, gridEX_Goods.GetRow().Cells["GoodName"].Text);
                    frm.ShowDialog();
                }
            }
            catch { }
        }
    }
}
