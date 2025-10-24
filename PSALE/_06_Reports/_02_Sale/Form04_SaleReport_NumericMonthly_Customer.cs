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

namespace PSHOP._06_Reports._02_Sale
{
    public partial class Form04_SaleReport_NumericMonthly_Customer : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
              SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents ClsDoc = new Classes.Class_Documents();
        DataTable CustomerTable;
        public Form04_SaleReport_NumericMonthly_Customer()
        {
            InitializeComponent();
        }

        private void Form04_SaleReport_NumericMonthly_Load(object sender, EventArgs e)
        {
            this.rpt_Sale_MonthlyNumeric_CustomerTableAdapter.Fill(dataSet_Report.Rpt_Sale_MonthlyNumeric_Customer, Class_BasicOperation._Year);
            this.rpt_Sale_MonthlyNumeric_Customer_DetailTableAdapter.Fill(dataSet_Report.Rpt_Sale_MonthlyNumeric_Customer_Detail,
                Class_BasicOperation._Year);

             CustomerTable= ClsDoc.ReturnTable(ConBase.ConnectionString, "SELECT     ColumnId, Column01, Column02 FROM         dbo.Table_045_PersonInfo");
           


            DataTable GoodTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from dbo.table_004_CommodityAndIngredients");
            gridEX1.DropDowns["CustomerCode"].SetDataBinding(CustomerTable, "");
            gridEX1.DropDowns["CustomerName"].SetDataBinding(CustomerTable, "");
            gridEX_Goods.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_Goods.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
            chk_Kol.Checked = true;
            crystalReportViewer1.BackColor = Color.White;
        }

        private void mnu_PrintTable_Click(object sender, EventArgs e)
        {
            if (chk_Kol.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_MonthlyNumeric_Customer_Detail.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                {
                    Table.Rows.Add(FarsiLibrary.Utils.PersianDate.Now.Year, 1, 1,
                        item.Cells["Far"].Value.ToString(), 0, 0, 0,
                        item.Cells["Ord"].Value.ToString(), 0, 0, 0,
                        item.Cells["Khordad"].Value.ToString(), 0, 0, 0,
                        item.Cells["Tir"].Value.ToString(), 0, 0, 0,
                        item.Cells["Mordad"].Value.ToString(), 0, 0, 0,
                        item.Cells["Shahr"].Value.ToString(), 0, 0, 0,
                        item.Cells["Mehr"].Value.ToString(), 0, 0, 0,
                        item.Cells["Aban"].Value.ToString(), 0, 0, 0,
                        item.Cells["Azar"].Value.ToString(), 0, 0, 0,
                        item.Cells["Dey"].Value.ToString(), 0, 0, 0,
                        item.Cells["Bahman"].Value.ToString(), 0, 0, 0,
                        item.Cells["Esf"].Value.ToString(), 0, 0, 0,
                        item.Cells["GoodName"].Text,
                        item.Cells["GoodCode"].Text);
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 34,
                        gridEX1.GetRow().Cells["CustomerCode"].Text, gridEX1.GetRow().Cells["CustomerName"].Text,
                        "بر اساس تعداد کل");
                    frm.ShowDialog();
                }
            }
            else if (chk_Box.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_MonthlyNumeric_Customer_Detail.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                {
                    Table.Rows.Add(FarsiLibrary.Utils.PersianDate.Now.Year, 1, 1,
                         item.Cells["FarBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["OrdBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["KhordadBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["TirBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["MordadBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["ShahrBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["MehrBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["AbanBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["AzarBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["DeyBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["BahmanBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["EsfBox"].Value.ToString(), 0, 0, 0,
                        item.Cells["GoodName"].Text,
                        item.Cells["GoodCode"].Text);
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 34,
                        gridEX1.GetRow().Cells["CustomerCode"].Text, gridEX1.GetRow().Cells["CustomerName"].Text,
                        "بر اساس تعداد کارتن");
                    frm.ShowDialog();
                }
            }
            else if (chk_Pack.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_MonthlyNumeric_Customer_Detail.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                {
                    Table.Rows.Add(FarsiLibrary.Utils.PersianDate.Now.Year, 1, 1,
                         item.Cells["FarPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["OrdPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["KhordadPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["TirPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["MordadPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["ShahrPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["MehrPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["AbanPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["AzarPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["DeyPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["BahmanPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["EsfPack"].Value.ToString(), 0, 0, 0,
                        item.Cells["GoodName"].Text,
                        item.Cells["GoodCode"].Text);
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 34,
                        gridEX1.GetRow().Cells["CustomerCode"].Text, gridEX1.GetRow().Cells["CustomerName"].Text,
                        "بر اساس تعداد بسته");
                    frm.ShowDialog();
                }
            }
            else if (chk_Joz.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_MonthlyNumeric_Customer_Detail.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                {
                    Table.Rows.Add(FarsiLibrary.Utils.PersianDate.Now.Year, 1, 1,
                         item.Cells["FarDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["OrdDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["KhordadDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["TirDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["MordadDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["ShahrDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["MehrDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["AbanDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["AzarDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["DeyDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["BahmanDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["EsfDetail"].Value.ToString(), 0, 0, 0,
                        item.Cells["GoodName"].Text,
                        item.Cells["GoodCode"].Text);
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 34,
                        gridEX1.GetRow().Cells["CustomerCode"].Text, gridEX1.GetRow().Cells["CustomerName"].Text,
                        "بر اساس تعداد جز");
                    frm.ShowDialog();
                }
            }
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
                    for (int i =5; i <= 50; i = i + 4)
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

        private void Form04_SaleReport_NumericMonthly_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.E)
                cmb_ExportToExcel.ShowDropDown();
            else if (e.Control && e.KeyCode == Keys.P)
                mnu_PrintTable_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
                gridEX_Goods.Row = gridEX_Goods.FilterRow.Position;
        }

        private void Form04_SaleReport_NumericMonthly_Activated(object sender, EventArgs e)
        {
            gridEX_Goods.Row = gridEX_Goods.FilterRow.Position;
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
                gridEX_Goods.RootTable.Columns["FarDetail"].Visible =  false;
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

        private void mnu_PrintAll_Click(object sender, EventArgs e)
        {
            if (chk_Kol.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetRows())
                {
                    gridEX1.MoveTo(Row);
                   
                        Table.Rows.Add(Row.Cells["CustomerCode"].Text.ToString(),
                            Row.Cells["CustomerName"].Text.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Far"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Ord"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Khordad"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Tir"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Mordad"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Shahr"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Mehr"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Aban"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Azar"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Dey"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Bahman"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Esf"].Value.ToString());
                }
               
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 36,
                       Class_BasicOperation._Year,"بر اساس تعداد کل");
                    frm.ShowDialog();
                }
            }
            else if (chk_Box.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetRows())
                {
                    gridEX1.MoveTo(Row);

                    Table.Rows.Add(Row.Cells["CustomerCode"].Text.ToString(),
                        Row.Cells["CustomerName"].Text.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["FarBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["OrdBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["KhordadBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["TirBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MordadBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["ShahrBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MehrBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AbanBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AzarBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["DeyBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["BahmanBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["EsfBox"].Value.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 36,
                        Class_BasicOperation._Year, "بر اساس تعداد کارتن");
                    frm.ShowDialog();
                }
            }
            else if (chk_Pack.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetRows())
                {
                    gridEX1.MoveTo(Row);

                    Table.Rows.Add(Row.Cells["CustomerCode"].Text.ToString(),
                        Row.Cells["CustomerName"].Text.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["FarPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["OrdPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["KhordadPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["TirPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MordadPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["ShahrPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MehrPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AbanPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AzarPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["DeyPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["BahmanPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["EsfPack"].Value.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 36,
                       Class_BasicOperation._Year, "بر اساس تعداد بسنه");
                    frm.ShowDialog();
                }
            }
            else if (chk_Joz.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetRows())
                {
                    gridEX1.MoveTo(Row);

                    Table.Rows.Add(Row.Cells["CustomerCode"].Text.ToString(),
                        Row.Cells["CustomerName"].Text.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["FarDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["OrdDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["KhordadDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["TirDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MordadDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["ShahrDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MehrDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AbanDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AzarDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["DeyDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["BahmanDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["EsfDetail"].Value.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 36,
                       Class_BasicOperation._Year, "بر اساس تعداد جز");
                    frm.ShowDialog();
                }
            }
        }

        private void gridEX_Goods_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gridEX_Goods.HitTest(e.X, e.Y) == Janus.Windows.GridEX.GridArea.TotalRow)
            {
                try{
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
                        Title.Text = "نمودار مقایسه ماهیانه تعداد کل کالای فروخته شده به  " + gridEX1.GetRow().Cells["CustomerName"].Text;
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
                        Title.Text = "نمودار مقایسه ماهیانه تعداد کارتن کالاهای فروخته شده به  " + gridEX1.GetRow().Cells["CustomerName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
                else if (chk_Pack.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i =5; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetTotalRow().Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار مقایسه ماهیانه تعداد بسته کالاهای فروخته شده به  " + gridEX1.GetRow().Cells["CustomerName"].Text;
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
                        Title.Text = "نمودار مقایسه ماهیانه تعداد  کالای فروخته شده به  " + gridEX1.GetRow().Cells["CustomerName"].Text;
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
