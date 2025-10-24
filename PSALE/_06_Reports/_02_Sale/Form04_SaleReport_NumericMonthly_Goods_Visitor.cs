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
    public partial class Form04_SaleReport_NumericMonthly_Goods_Visitor : Form
    {
        public Form04_SaleReport_NumericMonthly_Goods_Visitor()
        {
            InitializeComponent();
        }
        Classes.Class_Documents ClsDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);


        private void Form04_SaleReport_NumericMonthly_Visitor_Load(object sender, EventArgs e)
        {
            rpt_Sale_MonthlyNumeric_VisitorTableAdapter.Fill(dataSet_Report.Rpt_Sale_MonthlyNumeric_Visitor, Class_BasicOperation._Year);
            rpt_Sale_MonthlyNumeric_VisitorDetailsTableAdapter.Fill(dataSet_Report.Rpt_Sale_MonthlyNumeric_VisitorDetails, Class_BasicOperation._Year);

            DataTable PersonTable = ClsDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX_Visitors.DropDowns["Seller1"].SetDataBinding(PersonTable, "");
            gridEX_Visitors.DropDowns["Seller2"].SetDataBinding(PersonTable, "");


            DataTable GoodTable = ClsDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients");
            gridEX_Goods.DropDowns["Good1"].SetDataBinding(GoodTable, "");
            gridEX_Goods.DropDowns["Good2"].SetDataBinding(GoodTable, "");


//            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     Step1.Year, Step1.GoodID, SUM(Step1.Far) AS Far, SUM(Step1.Ord) AS Ord, SUM(Step1.Khord) AS Khordad, SUM(Step1.Tir) AS Tir, SUM(Step1.Mordad) AS Mordad, 
//            SUM(Step1.Shahr) AS Shahr, SUM(Step1.Mehr) AS Mehr, SUM(Step1.Aban) AS Aban, SUM(Step1.Azar) AS Azar, SUM(Step1.Dey) AS Dey, SUM(Step1.Bahman) 
//            AS Bahman, SUM(Step1.Esf) AS Esf, GoodTable.column01 AS GoodCode, GoodTable.column02 AS GoodName
//            FROM         (SELECT     TOP (100) PERCENT SUBSTRING(dbo.Table_010_SaleFactor.column02, 1, 4) AS Year, SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) AS Month, 
//            dbo.Table_011_Child1_SaleFactor.column02 AS GoodID, SUM(dbo.Table_011_Child1_SaleFactor.Column11) AS TotalNumber, 
//            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) ELSE 0 END AS Far, 
//            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) ELSE 0 END AS Ord, 
//            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) ELSE 0 END AS Khord,
//            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) ELSE 0 END AS Tir, 
//            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) 
//            ELSE 0 END AS Mordad, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
//            = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) ELSE 0 END AS Shahr, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 
//            2) = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) ELSE 0 END AS Mehr, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 
//            2) = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) ELSE 0 END AS Aban, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 
//            2) = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) ELSE 0 END AS Azar, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 
//            2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) ELSE 0 END AS Dey, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 
//            2) = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) ELSE 0 END AS Bahman, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02,
//            6, 2) = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.Column11) ELSE 0 END AS Esf
//            FROM          dbo.Table_010_SaleFactor INNER JOIN
//            dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
//            where Table_010_SaleFactor.Column12=0
//            GROUP BY SUBSTRING(dbo.Table_010_SaleFactor.column02, 1, 4), dbo.Table_011_Child1_SaleFactor.column02, SUBSTRING(dbo.Table_010_SaleFactor.column02, 
//            6, 2)
//            ORDER BY Year, Month, GoodID) AS Step1 LEFT OUTER JOIN
//            (SELECT     columnid, column01, column02
//            FROM          {1}.dbo.table_004_CommodityAndIngredients) AS GoodTable ON Step1.GoodID = GoodTable.columnid
//            WHERE     (Step1.Year = '{0}')
//            GROUP BY Step1.Year, Step1.GoodID, GoodTable.column01, GoodTable.column02", ConSale);
//            Adapter.SelectCommand.CommandText = String.Format(Adapter.SelectCommand.CommandText, Class_BasicOperation._Year, ConWare.Database);
//            DataTable Table = new DataTable();
//            Adapter.Fill(Table);
//            gridEX_Goods.DataSource = Table;
//            BindingSource BindSource = new BindingSource();
//            BindSource.DataSource = Table;
//            bindingNavigator1.BindingSource = BindSource;

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
            try
            {
                if (chk_Kol.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_Visitors.GetCheckedRows())
                    {
                        gridEX_Visitors.MoveTo(Row);
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                        {
                            Table.Rows.Add(item.Cells["GoodCode"].Text,
                                item.Cells["GoodName"].Text,
                                item.Cells["Far"].Value.ToString(),
                                item.Cells["Ord"].Value.ToString(),
                                item.Cells["Khordad"].Value.ToString(),
                                item.Cells["Tir"].Value.ToString(),
                                item.Cells["Mordad"].Value.ToString(),
                                item.Cells["Shahr"].Value.ToString(),
                                item.Cells["Mehr"].Value.ToString(),
                                item.Cells["Aban"].Value.ToString(),
                                item.Cells["Azar"].Value.ToString(),
                                item.Cells["Dey"].Value.ToString(),
                                item.Cells["Bahman"].Value.ToString(),
                                item.Cells["Esf"].Value.ToString(),
                                Row.Cells["VisitorCode"].Text,
                                Row.Cells["VisitorName"].Text,
                                Row.Cells["VisitorId"].Value.ToString());
                        }
                    }

                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 40,
                           Class_BasicOperation._Year, "بر اساس تعداد کل");
                        frm.ShowDialog();
                    }
                }
                else if (chk_Box.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_Visitors.GetCheckedRows())
                    {
                        gridEX_Visitors.MoveTo(Row);
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                        {

                            Table.Rows.Add(item.Cells["GoodCode"].Text,
                                   item.Cells["GoodName"].Text,
                                item.Cells["FarBox"].Value.ToString(),
                                item.Cells["OrdBox"].Value.ToString(),
                                item.Cells["KhordadBox"].Value.ToString(),
                                item.Cells["TirBox"].Value.ToString(),
                                item.Cells["MordadBox"].Value.ToString(),
                                item.Cells["ShahrBox"].Value.ToString(),
                                item.Cells["MehrBox"].Value.ToString(),
                                item.Cells["AbanBox"].Value.ToString(),
                                item.Cells["AzarBox"].Value.ToString(),
                                item.Cells["DeyBox"].Value.ToString(),
                                item.Cells["BahmanBox"].Value.ToString(),
                                item.Cells["EsfBox"].Value.ToString(),
                                Row.Cells["VisitorCode"].Text,
                                Row.Cells["VisitorName"].Text,
                                Row.Cells["VisitorId"].Value.ToString());
                        }
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 40,
                            Class_BasicOperation._Year, "بر اساس تعداد کارتن");
                        frm.ShowDialog();
                    }
                }
                else if (chk_Pack.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_Visitors.GetCheckedRows())
                    {
                        gridEX_Visitors.MoveTo(Row);
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                        {

                            Table.Rows.Add(item.Cells["GoodCode"].Text,
                                 item.Cells["GoodName"].Text,
                                item.Cells["FarPack"].Value.ToString(),
                                item.Cells["OrdPack"].Value.ToString(),
                                item.Cells["KhordadPack"].Value.ToString(),
                                item.Cells["TirPack"].Value.ToString(),
                                item.Cells["MordadPack"].Value.ToString(),
                                item.Cells["ShahrPack"].Value.ToString(),
                                item.Cells["MehrPack"].Value.ToString(),
                                item.Cells["AbanPack"].Value.ToString(),
                                item.Cells["AzarPack"].Value.ToString(),
                                item.Cells["DeyPack"].Value.ToString(),
                                item.Cells["BahmanPack"].Value.ToString(),
                                item.Cells["EsfPack"].Value.ToString(),
                                Row.Cells["VisitorCode"].Text,
                                Row.Cells["VisitorName"].Text,
                                Row.Cells["VisitorId"].Value.ToString());
                        }
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 40,
                           Class_BasicOperation._Year, "بر اساس تعداد بسنه");
                        frm.ShowDialog();
                    }
                }
                else if (chk_Joz.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_Visitors.GetCheckedRows())
                    {
                        gridEX_Visitors.MoveTo(Row);
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                        {

                            Table.Rows.Add(item.Cells["GoodCode"].Text,
                                  item.Cells["GoodName"].Text,
                                item.Cells["FarDetail"].Value.ToString(),
                                item.Cells["OrdDetail"].Value.ToString(),
                                item.Cells["KhordadDetail"].Value.ToString(),
                                item.Cells["TirDetail"].Value.ToString(),
                                item.Cells["MordadDetail"].Value.ToString(),
                                item.Cells["ShahrDetail"].Value.ToString(),
                                item.Cells["MehrDetail"].Value.ToString(),
                                item.Cells["AbanDetail"].Value.ToString(),
                                item.Cells["AzarDetail"].Value.ToString(),
                                item.Cells["DeyDetail"].Value.ToString(),
                                item.Cells["BahmanDetail"].Value.ToString(),
                                item.Cells["EsfDetail"].Value.ToString(),
                                Row.Cells["VisitorCode"].Text,
                                Row.Cells["VisitorName"].Text,
                                Row.Cells["VisitorId"].Value.ToString());
                        }
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 40,
                           Class_BasicOperation._Year, "بر اساس تعداد جز");
                        frm.ShowDialog();
                    }

                }
            }
            catch { }
        }

        private void gridEX_Goods_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
            {
                if (chk_Kol.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i = 3; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, e.Row.Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار ماهیانه تعداد کل فروخته شده کالای  " + e.Row.Cells["GoodName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
                else if (chk_Box.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i = 4; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, e.Row.Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار ماهیانه تعداد کارتن فروخته شده کالای  " + e.Row.Cells["GoodName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
                else if (chk_Pack.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i = 5; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, e.Row.Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار ماهیانه تعداد بسته فروخته شده کالای  " + e.Row.Cells["GoodName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
                else if (chk_Joz.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i = 6; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, e.Row.Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار ماهیانه تعداد فروخته شده کالای  " + e.Row.Cells["GoodName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
            }
            catch
            {
            }
        }

        private void chk_Kol_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Kol.Checked)
            {
                gridEX_Goods.RootTable.Columns["Far"].Visible = true;
                gridEX_Goods.RootTable.Columns["Ord"].Visible = true;
                gridEX_Goods.RootTable.Columns["Khordad"].Visible = true;
                gridEX_Goods.RootTable.Columns["Tir"].Visible = true;
                gridEX_Goods.RootTable.Columns["Mordad"].Visible = true;
                gridEX_Goods.RootTable.Columns["Shahr"].Visible = true;
                gridEX_Goods.RootTable.Columns["Mehr"].Visible = true;
                gridEX_Goods.RootTable.Columns["Aban"].Visible = true;
                gridEX_Goods.RootTable.Columns["Azar"].Visible = true;
                gridEX_Goods.RootTable.Columns["Dey"].Visible = true;
                gridEX_Goods.RootTable.Columns["Bahman"].Visible = true;
                gridEX_Goods.RootTable.Columns["Esf"].Visible = true;
            }
            else
            {
                gridEX_Goods.RootTable.Columns["Far"].Visible = false;
                gridEX_Goods.RootTable.Columns["Ord"].Visible = false;
                gridEX_Goods.RootTable.Columns["Khordad"].Visible = false;
                gridEX_Goods.RootTable.Columns["Tir"].Visible = false;
                gridEX_Goods.RootTable.Columns["Mordad"].Visible = false;
                gridEX_Goods.RootTable.Columns["Shahr"].Visible = false;
                gridEX_Goods.RootTable.Columns["Mehr"].Visible = false;
                gridEX_Goods.RootTable.Columns["Aban"].Visible = false;
                gridEX_Goods.RootTable.Columns["Azar"].Visible = false;
                gridEX_Goods.RootTable.Columns["Dey"].Visible = false;
                gridEX_Goods.RootTable.Columns["Bahman"].Visible = false;
                gridEX_Goods.RootTable.Columns["Esf"].Visible = false;
            }

            if (chk_Box.Checked)
            {
                gridEX_Goods.RootTable.Columns["FarBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["OrdBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["KhordadBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["TirBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["MordadBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["ShahrBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["MehrBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["AbanBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["AzarBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["DeyBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["BahmanBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["EsfBox"].Visible = true;
            }
            else
            {
                gridEX_Goods.RootTable.Columns["FarBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["OrdBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["KhordadBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["TirBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["MordadBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["ShahrBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["MehrBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["AbanBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["AzarBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["DeyBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["BahmanBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["EsfBox"].Visible = false;
            }

            if (chk_Pack.Checked)
            {
                gridEX_Goods.RootTable.Columns["FarPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["OrdPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["KhordadPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["TirPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["MordadPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["ShahrPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["MehrPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["AbanPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["AzarPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["DeyPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["BahmanPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["EsfPack"].Visible = true;
            }
            else
            {
                gridEX_Goods.RootTable.Columns["FarPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["OrdPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["KhordadPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["TirPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["MordadPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["ShahrPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["MehrPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["AbanPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["AzarPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["DeyPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["BahmanPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["EsfPack"].Visible = false;
            }

            if (chk_Joz.Checked)
            {
                gridEX_Goods.RootTable.Columns["FarDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["OrdDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["KhordadDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["TirDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["MordadDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["ShahrDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["MehrDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["AbanDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["AzarDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["DeyDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["BahmanDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["EsfDetail"].Visible = true;
            }
            else
            {
                gridEX_Goods.RootTable.Columns["FarDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["OrdDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["KhordadDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["TirDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["MordadDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["ShahrDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["MehrDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["AbanDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["AzarDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["DeyDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["BahmanDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["EsfDetail"].Visible = false;
            }
        }

        private void Form04_SaleReport_NumericMonthly_Goods_Visitor_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode== Keys.F)
            mnu_PrintAll_Click(sender, e);
        }

        private void gridEX_Goods_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gridEX_Goods.HitTest(e.X, e.Y) == Janus.Windows.GridEX.GridArea.TotalRow)
            {
                try
                {
                    if (chk_Kol.Checked)
                    {
                        DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                        for (int i = 3; i <= 50; i = i + 4)
                        {
                            Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetTotalRow().Cells[i].Value.ToString());
                        }
                        if (Table.Rows.Count > 0)
                        {
                            _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                            rpt.SetDataSource(Table);
                            TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                            Title.Text = "نمودار مقایسه ماهیانه تعداد کل کالای فروخته شده توسط  " + gridEX_Visitors.GetRow().Cells["VisitorName"].Text;
                            crystalReportViewer1.ReportSource = rpt;
                            uiPanel0.AutoHideActive = true;
                        }
                    }
                    else if (chk_Box.Checked)
                    {
                        DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                        for (int i = 4; i <= 50; i = i + 4)
                        {
                            Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetTotalRow().Cells[i].Value.ToString());
                        }
                        if (Table.Rows.Count > 0)
                        {
                            _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                            rpt.SetDataSource(Table);
                            TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                            Title.Text = "نمودار مقایسه ماهیانه تعداد کارتن کالاهای فروخته شده توسط  " + gridEX_Visitors.GetRow().Cells["VisitorName"].Text;
                            crystalReportViewer1.ReportSource = rpt;
                            uiPanel0.AutoHideActive = true;
                        }
                    }
                    else if (chk_Pack.Checked)
                    {
                        DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                        for (int i = 5; i <= 50; i = i + 4)
                        {
                            Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetTotalRow().Cells[i].Value.ToString());
                        }
                        if (Table.Rows.Count > 0)
                        {
                            _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                            rpt.SetDataSource(Table);
                            TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                            Title.Text = "نمودار مقایسه ماهیانه تعداد بسته کالاهای فروخته شده توسط  " + gridEX_Visitors.GetRow().Cells["VisitorName"].Text;
                            crystalReportViewer1.ReportSource = rpt;
                            uiPanel0.AutoHideActive = true;
                        }
                    }
                    else if (chk_Joz.Checked)
                    {
                        DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                        for (int i = 6; i <= 50; i = i + 4)
                        {
                            Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetTotalRow().Cells[i].Value.ToString());
                        }
                        if (Table.Rows.Count > 0)
                        {
                            _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                            rpt.SetDataSource(Table);
                            TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                            Title.Text = "نمودار مقایسه ماهیانه تعداد  کالای فروخته شده توسط  " + gridEX_Visitors.GetRow().Cells["VisitorName"].Text;
                            crystalReportViewer1.ReportSource = rpt;
                            uiPanel0.AutoHideActive = true;
                        }
                    }
                }
                catch
                {
                }
            }
        }
    }
}
