using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._06_Reports._02_Sale
{
    public partial class Form38_ProfitReportByGoodGroup : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        bool _BackSpace = false;
        string Date1, Date2;
        public Form38_ProfitReportByGoodGroup()
        {
            InitializeComponent();
        }

        private void Form38_ProfitReportByGoodGroup_Load(object sender, EventArgs e)
        {
            string[] Dates = Properties.Settings.Default.ProfitReportByGoodGroup.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
        }
        private void faDatePickerStrip1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox = (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


                if (textBox.FADatePicker.Text.Length == 4)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
                else if (textBox.FADatePicker.Text.Length == 7)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
            }
        }

        private void faDatePickerStrip1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip1.FADatePicker.HideDropDown();
                    faDatePickerStrip2.FADatePicker.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePickerStrip2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip2.FADatePicker.HideDropDown();
                    bt_Search_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }
        private void Form18_MarginReport_GoodCustomerFactor_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.ProfitReportByGoodGroup = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX1.RemoveFilters();
        }

        private void bt_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                //برای محاسبه سود جمع قیمت جزء فاکتور و برای ارزش جمع ارزش جزء تقسیم بر تعداد حواله
                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT f.MainGroup,
                                                                               SUM(f.TotalPrice) AS TotalPrice,
                                                                               SUM(f.TotalValue) AS TotalValue,
                                                                               SUM(f.Number) AS Number,
                                                                               (ISNULL(SUM(f.TotalPrice), 0) -ISNULL(SUM(f.TotalValue), 0)) / NULLIF(SUM(f.Number), 0)
                                                                               / NULLIF(ISNULL(SUM(f.TotalValue), 0) / SUM(f.Number), 0) AS UMRatio,
                                                                               (
                                                                                   (
                                                                                       (ISNULL(SUM(f.TotalPrice), 0) -ISNULL(SUM(f.TotalValue), 0)) /
                                                                                       NULLIF(SUM(f.Number), 0) / NULLIF(ISNULL(SUM(f.TotalValue), 0) / SUM(f.Number), 0)
                                                                                   ) / NULLIF((ISNULL(SUM(f.TotalValue), 0) / SUM(f.Number)), 0)
                                                                               ) * 100 AS DRatio,
                                                                               (ISNULL(SUM(f.TotalPrice), 0) -ISNULL(SUM(f.TotalValue), 0)) AS ProfitAmount,
                                                                             (  (ISNULL(SUM(f.TotalPrice), 0) -ISNULL(SUM(f.TotalValue), 0))/NULLIF(ISNULL(SUM(f.TotalValue), 0),0))*100 AS ProfitPre
                                                                        FROM   (
                                                                                   SELECT factor.*,
                                                                                          ChildDraftTable.column16 AS TotalValue,
                                                                                          tcai.column03 AS MainGroupID,
                                                                                          tmg.column02 AS MainGroup
                                                                                   FROM   (
                                                                                              SELECT ROW_NUMBER() OVER(
                                                                                                         PARTITION BY Table_011_Child1_SaleFactor.column01
                                                                                                         ORDER BY Table_011_Child1_SaleFactor.columnid,
                                                                                                         Table_011_Child1_SaleFactor.column02
                                                                                                     ) AS Row,
                                                                                                     dbo.Table_010_SaleFactor.column02 AS FactorDate,
                                                                                                     dbo.Table_011_Child1_SaleFactor.column02 AS 
                                                                                                     GoodCode,
                                                                                                     dbo.Table_011_Child1_SaleFactor.column07 AS Number,
                                                                                                     dbo.Table_011_Child1_SaleFactor.column20 AS 
                                                                                                     TotalPrice,
                                                                                                     0.000 AS TotalMargin,
                                                                                                     0.000 Ratio,
                                                                                                     dbo.Table_010_SaleFactor.column09
                                                                                              FROM   dbo.Table_010_SaleFactor
                                                                                                     INNER JOIN dbo.Table_011_Child1_SaleFactor
                                                                                                          ON  dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                                                                                                    AND dbo.Table_010_SaleFactor.column17 = 0 AND dbo.Table_010_SaleFactor.column19 = 0
                                                                                          ) AS factor
                                                                                          INNER JOIN (
                                                                                                   SELECT ROW_NUMBER() OVER(PARTITION BY column01 ORDER BY columnid, column02) AS 
                                                                                                          Row,
                                                                                                          column01,
                                                                                                          column02,
                                                                                                          column07,
                                                                                                          column16
                                                                                                   FROM   " + ConWare.Database+@".dbo.Table_008_Child_PwhrsDraft
                                                                                               ) AS ChildDraftTable
                                                                                               ON  factor.column09 = ChildDraftTable.column01
                                                                                               AND factor.GoodCode = ChildDraftTable.column02
                                                                                               AND factor.Row = ChildDraftTable.Row
                                                                                          JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                                               ON  tcai.columnid = factor.GoodCode
                                                                                          JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                                                               ON  tmg.columnid = tcai.column03
                                                                                                   WHERE factor.FactorDate BETWEEN   '" + Date1 + "' AND '" + Date2 + @"'
                                                                               ) AS f
                                                                        GROUP BY
                                                                               f.MainGroupID,
                                                                               f.MainGroup");

                gridEX1.DataSource = Table;
                // gridEX1_FilterApplied(sender, e);
                gridEX1.Row = gridEX1.FilterRow.Position;
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string j = " از تاریخ:" + faDatePickerStrip1.FADatePicker.Text + " تا تاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageHeaderLeft = j;
                        gridEXPrintDocument1.PageHeaderRight = " گزارش سود براساس گروه کالا  ";
                        printPreviewDialog1.ShowDialog();
                    } 
            }
            catch { }
        }
    }

}
