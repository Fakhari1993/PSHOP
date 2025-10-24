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

    public partial class Form43_Goods_Customer : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlDataAdapter Adapter;
        Class_UserScope UserScope = new Class_UserScope();
        DataTable Person = new DataTable();
        bool _BackSpace = false;
        string Date1 = null, Date2 = null;

        public Form43_Goods_Customer()
        {
            InitializeComponent();
        }
        private void Form03_OrderGoodsWay_Load(object sender, EventArgs e)
        {
            try
            {


                Adapter = new SqlDataAdapter(@"SELECT     ColumnId, Column01, Column02
FROM         dbo.Table_045_PersonInfo", ConBase);

                Adapter.Fill(Person);
                gridEX1.DropDowns["CustomerCode"].SetDataBinding(Person, "");
                gridEX1.DropDowns["CustomerName"].SetDataBinding(Person, "");

                //crystalReportViewer1.BackColor = Color.White;
                DataTable dt = new DataTable();

                dt = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT     dbo.table_004_CommodityAndIngredients.columnid, dbo.table_004_CommodityAndIngredients.column01, dbo.table_004_CommodityAndIngredients.column02, 
            dbo.table_002_MainGroup.column02 AS MainGroup, dbo.table_003_SubsidiaryGroup.column03 AS SubGroup
            FROM         dbo.table_004_CommodityAndIngredients INNER JOIN
            dbo.table_003_SubsidiaryGroup ON dbo.table_004_CommodityAndIngredients.column04 = dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
            dbo.table_002_MainGroup ON dbo.table_003_SubsidiaryGroup.column01 = dbo.table_002_MainGroup.columnid");
                faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
                faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
                mlt_Good.DataSource = dt;


            }
            catch { }
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

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue && mlt_Good.Text.Trim() != "")
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                this.Cursor = Cursors.WaitCursor;
                string CommandStr = null;

                CommandStr = @"SELECT GoodID,
                                           SUM( isnull(Tedad,0)) AS Tedad,
                                           SUM( isnull(Total,0)) AS Total,
                                           SUM( isnull(Khales,0)) AS Khales,
                                           SUM( isnull(Takhfif,0)) AS Takhfif,
                                           SUM( isnull(Ezafe,0)) AS Ezafe,
                                           SUM( isnull(carton,0)) AS carton,
                                           SUM( isnull(baste,0)) AS baste,
                                           SUM( isnull(joz,0)) AS joz,
       
                                           CustomerID,
                                           SaleManID
                                    FROM   (
                                               SELECT TOP(100) PERCENT dbo.Table_011_Child1_SaleFactor.column02 AS 
                                                      GoodID,
                                                      (dbo.Table_011_Child1_SaleFactor.column04) AS carton,
                                                      (dbo.Table_011_Child1_SaleFactor.column05) AS baste, 
                                                      (dbo.Table_011_Child1_SaleFactor.column06) AS joz, 
                                                      (dbo.Table_011_Child1_SaleFactor.column07) AS Tedad,
                                                      (dbo.Table_011_Child1_SaleFactor.column11) AS Total,
                                                      (dbo.Table_011_Child1_SaleFactor.column20) AS Khales,
                                                      (dbo.Table_011_Child1_SaleFactor.column17) AS Takhfif,
                                                      (dbo.Table_011_Child1_SaleFactor.column19) AS Ezafe,
                  
                                                      dbo.Table_010_SaleFactor.column03 AS CustomerID,
                                                      dbo.Table_010_SaleFactor.column05 AS SaleManID,
                                                      Table_011_Child1_SaleFactor.columnid
                                               FROM   dbo.Table_010_SaleFactor
                                                      INNER JOIN dbo.Table_011_Child1_SaleFactor
                                                           ON  dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                                               WHERE  (dbo.Table_010_SaleFactor.column02 >= '{0}')
                                                      AND (dbo.Table_010_SaleFactor.column02 <= '{1}')
                                                      AND (dbo.Table_011_Child1_SaleFactor.column02 = {2})
                                                      AND dbo.Table_010_SaleFactor.column17=0 AND dbo.Table_010_SaleFactor.column19=0
           
           
                                               UNION  all
                                               SELECT TOP(100) PERCENT dbo.Table_019_Child1_MarjooiSale.column02 AS 
                                                      GoodID,
                                                      (-1) *(dbo.Table_019_Child1_MarjooiSale.column04) AS carton,
                                                      (-1) *(dbo.Table_019_Child1_MarjooiSale.column05) AS baste, 
                                                      (-1) * (dbo.Table_019_Child1_MarjooiSale.column06) AS joz, 
                                                      (-1) * (isnull(dbo.Table_019_Child1_MarjooiSale.column07,0)) AS Tedad,
                                                      (-1) * (isnull(dbo.Table_019_Child1_MarjooiSale.column11,0)) AS Total,
                                                      (-1) * (isnull(dbo.Table_019_Child1_MarjooiSale.column20,0)) AS Khales,
                                                      (-1) * (isnull(dbo.Table_019_Child1_MarjooiSale.column17,0)) AS Takhfif,
                                                      (-1) *(isnull(dbo.Table_019_Child1_MarjooiSale.column19,0)) AS Ezafe,
                  
                                                      dbo.Table_018_MarjooiSale.column03 AS CustomerID,
                                                      dbo.Table_018_MarjooiSale.column05 AS SaleManID,
                                                      Table_019_Child1_MarjooiSale.columnid
                                               FROM   dbo.Table_018_MarjooiSale
                                                      INNER JOIN dbo.Table_019_Child1_MarjooiSale
                                                           ON  dbo.Table_018_MarjooiSale.columnid = dbo.Table_019_Child1_MarjooiSale.column01
                                                               WHERE  (dbo.Table_018_MarjooiSale.column02 >= '{0}')
                                                                      AND (dbo.Table_018_MarjooiSale.column02 <= '{1}')
                                                                      AND (dbo.Table_019_Child1_MarjooiSale.column02 = {2})
                                           ) AS fg
                                    GROUP BY
                                           GoodID,
                                           CustomerID,
                                           SaleManID";

                CommandStr = string.Format(CommandStr, Date1, Date2, mlt_Good.Value.ToString());

                gridEX1.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, CommandStr);
                //DataTable PrintTable = dataSet_Report.Rpt_GoodWay.Clone();
                //foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                //{
                //    PrintTable.Rows.Add(mlt_Good.Text, item.Cells["CustomerName"].Text.ToString(), item.Cells["baste"].Value.ToString(), item.Cells["carton"].Value.ToString());
                //}
                //if (PrintTable.Rows.Count > 0)
                //{
                //    _06_Reports._01_Orders.Report01_GoodWay Rpt = new Report01_GoodWay();
                //    TextObject Title = (TextObject)Rpt.ReportDefinition.ReportObjects["Text3"];
                //    Title.Text = "نمودار سفارشات مشتریان از محصول " + mlt_Good.Text + " از تاریخ " + Date1 + " تا تاریخ " + Date2;
                //    Rpt.SetDataSource(PrintTable);
                //    crystalReportViewer1.ReportSource = Rpt;
                //    crystalReportViewer1.Refresh();
                //    crystalReportViewer1.Zoom(1);
                //}
                this.Cursor = Cursors.Default;

                this.txt_Total.Value =0;
                this.txt_Takhfif.Value =0;
                txt_Ezafe.Value =0;
                txt_Count.Value = 0;
                txt_netamount.Value =0;
                txt_count1.Value = 0;
            }

        }

        private void Form03_OrderGoodsWay_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
        }

        private void Form03_OrderGoodsWay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(null, null);
            else if (e.Control && e.KeyCode == Keys.F)
                mlt_Good.Select();
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Search_Click(sender, e);
        }

        private void mnu_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void mlt_Good_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                mlt_Good.DroppedDown = true;
            else if (e.KeyChar == 13)
                faDatePickerStrip1.FADatePicker.Select();

        }

        private void bt_PrintChart_Click(object sender, EventArgs e)
        {
            //crystalReportViewer1.PrintReport();
        }

        private void bt_PrintTable_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_GoodWay.Clone();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                Table.Rows.Add(mlt_Good.Text, item.Cells["CustomerID"].Text, item.Cells["Order_Box"].Value.ToString(), item.Cells["Exit_Box"].Value.ToString(), item.Cells["CustomerName"].Text);
            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 3, Date1, Date2);
                frm.ShowDialog();
            }
        }

        private void gridEX1_RowCheckStateChanged(object sender, Janus.Windows.GridEX.RowCheckStateChangeEventArgs e)
        {
            try
            {
                if (gridEX1.GetCheckedRows().Length > 0)
                {
                    uiGroupBox1.Visible = true;


                    this.txt_Total.Value = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Total"].Value.ToString()));
                    this.txt_Takhfif.Value = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Takhfif"].Value.ToString()));
                    txt_Ezafe.Value = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Ezafe"].Value.ToString()));
                    txt_Count.Value = gridEX1.GetCheckedRows().Length;
                    txt_netamount.Value = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Khales"].Value.ToString()));
                    txt_count1.Value = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Tedad"].Value.ToString()));
                }
                else uiGroupBox1.Visible = false;


            }
            catch { }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {

                gridEXPrintDocument1.GridEX = this.gridEX1;

                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        //  string j = " تاریخ تهیه گزارش:" + FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");
                        gridEXPrintDocument1.PageHeaderLeft = "ازتاریخ: " + faDatePickerStrip1.FADatePicker.Text + "تاتاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageHeaderRight = " نام کالا: " + mlt_Good.Text;
                        gridEXPrintDocument1.PageHeaderCenter = "گزارش فروش کالا";
                        printPreviewDialog1.ShowDialog();
                    }
            }
            catch
            {
            }
        }

        private void mlt_Good_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                Class_BasicOperation.FilterMultiColumns(sender, "Column02", null);
            }
            catch
            {
            }
        }

        private void mlt_Good_Leave(object sender, EventArgs e)
        {
            try
            {
                Class_BasicOperation.MultiColumnsRemoveFilter(sender);
            }
            catch
            {
            }
        }

    }
}
