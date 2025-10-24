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

namespace PSHOP._06_Reports._01_Orders
{
    
    public partial class Form03_OrderGoodsWay_Province : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        string Date1 = null, Date2 = null;

        public Form03_OrderGoodsWay_Province()
        {
            InitializeComponent();
        }
        private void Form03_OrderGoodsWay_Load(object sender, EventArgs e)
        {
            crystalReportViewer1.BackColor = Color.White;
            mlt_Good.DataSource = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT     dbo.table_004_CommodityAndIngredients.columnid, dbo.table_004_CommodityAndIngredients.column01, dbo.table_004_CommodityAndIngredients.column02, 
            dbo.table_002_MainGroup.column02 AS MainGroup, dbo.table_003_SubsidiaryGroup.column03 AS SubGroup
            FROM         dbo.table_004_CommodityAndIngredients INNER JOIN
            dbo.table_003_SubsidiaryGroup ON dbo.table_004_CommodityAndIngredients.column04 = dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
            dbo.table_002_MainGroup ON dbo.table_003_SubsidiaryGroup.column01 = dbo.table_002_MainGroup.columnid");
            mlt_Province.DataSource = clDoc.ReturnTable(Properties.Settings.Default.BASE, "Select Column00,Column01 from Table_060_ProvinceInfo");
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            mlt_Good.Focus();
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
            
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker
                .SelectedDateTime.HasValue && mlt_Good.Text.Trim()!="" && mlt_Province.Text.Trim()!="")
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                this.Cursor = Cursors.WaitCursor;
                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     TOP (100) PERCENT dbo.Table_006_OrderDetails.column02, SUM(dbo.Table_006_OrderDetails.column03) AS Pack, SUM(dbo.Table_006_OrderDetails.column04) 
                      AS Box, SUM(dbo.Table_006_OrderDetails.column05) AS Detail, SUM(dbo.Table_006_OrderDetails.column06) AS OrderNumber, 
                      dbo.Table_005_OrderHeader.column02 AS Date, SUM(dbo.Table_006_OrderDetails.column15) AS ExitPack, SUM(dbo.Table_006_OrderDetails.column16) AS ExitBox, 
                      SUM(dbo.Table_006_OrderDetails.column14) AS ExitDetail, SUM(dbo.Table_006_OrderDetails.column17) AS ExitNumber, 
                      dbo.Table_005_OrderHeader.column05 AS City, derivedtbl_2.Column00 AS Province
                    FROM         dbo.Table_005_OrderHeader INNER JOIN
                    dbo.Table_006_OrderDetails ON dbo.Table_005_OrderHeader.columnid = dbo.Table_006_OrderDetails.column01 LEFT OUTER JOIN
                    (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                    FROM         {4}.dbo.Table_065_CityInfo) AS derivedtbl_2 ON dbo.Table_005_OrderHeader.column05 = derivedtbl_2.Column01
                    WHERE     (dbo.Table_005_OrderHeader.column02 >= '{0}') AND (dbo.Table_005_OrderHeader.column02 <= '{1}') AND 
                    (dbo.Table_006_OrderDetails.column02 = {2}) AND  (derivedtbl_2.Column00={3})
                    GROUP BY dbo.Table_006_OrderDetails.column02, dbo.Table_005_OrderHeader.column02, dbo.Table_005_OrderHeader.column05, derivedtbl_2.Column00", ConSale);
                    Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, 
                        Date1, Date2,mlt_Good.Value.ToString(),mlt_Province.Value.ToString(),
                        ConBase.Database);
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);
                    gridEX1.DataSource = Table;
                 
                    this.Cursor = Cursors.Default;
            }
        }

        private void Form03_OrderGoodsWay_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
        }

        private void Form03_OrderGoodsWay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
                bt_Print.ShowDropDown();
            else if (e.Control && e.KeyCode == Keys.F)
            {
                faDatePickerStrip1.FADatePicker.Select();
                faDatePickerStrip1.Select();
            }
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
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);

        }

        private void bt_PrintChart_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.PrintReport();
        }

        private void bt_PrintTable_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_GoodWay.Clone();
            if (chk_Total.Checked)
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    Table.Rows.Add(mlt_Good.Text, item.Cells["Date"].Text, item.Cells["OrderNumber"].Value.ToString(), item.Cells["ExitNumber"].Value.ToString());
                }
            }
            else if (chk_Box.Checked)
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    Table.Rows.Add(mlt_Good.Text, item.Cells["Date"].Text, item.Cells["Box"].Value.ToString(), item.Cells["ExitBox"].Value.ToString());
                }
            }
            else if (chk_Pack.Checked)
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    Table.Rows.Add(mlt_Good.Text, item.Cells["Date"].Text, item.Cells["Pack"].Value.ToString(), item.Cells["ExitPack"].Value.ToString());
                }
            }
            else if (chk_Detail.Checked)
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    Table.Rows.Add(mlt_Good.Text, item.Cells["Date"].Text, item.Cells["Detail"].Value.ToString(), item.Cells["ExitDetail"].Value.ToString());
                }

            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 2, Date1, Date2,
                    "نام استان: "+mlt_Province.Text);
                frm.ShowDialog();
            }
        }

        private void chk_Box_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Total.Checked)
            {
                gridEX1.RootTable.Columns["OrderNumber"].Visible = true;
                gridEX1.RootTable.Columns["ExitNumber"].Visible = true;
            }
            else
            {
                gridEX1.RootTable.Columns["OrderNumber"].Visible = false;
                gridEX1.RootTable.Columns["ExitNumber"].Visible = false;
            }

            if (chk_Box.Checked)
            {
                gridEX1.RootTable.Columns["Box"].Visible = true;
                gridEX1.RootTable.Columns["ExitBox"].Visible = true;
            }
            else
            {
                gridEX1.RootTable.Columns["Box"].Visible = false;
                gridEX1.RootTable.Columns["ExitBox"].Visible = false;
            }

            if (chk_Pack.Checked)
            {
                gridEX1.RootTable.Columns["Pack"].Visible = true;
                gridEX1.RootTable.Columns["ExitPack"].Visible = true;
            }
            else
            {
                gridEX1.RootTable.Columns["Pack"].Visible = false;
                gridEX1.RootTable.Columns["ExitPack"].Visible = false;
            }

            if (chk_Detail.Checked)
            {
                gridEX1.RootTable.Columns["Detail"].Visible = true;
                gridEX1.RootTable.Columns["ExitDetail"].Visible = true;
            }
            else
            {
                gridEX1.RootTable.Columns["Detail"].Visible = false;
                gridEX1.RootTable.Columns["ExitDetail"].Visible = false;
            }
        }

        private void bt_ViewChart_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable PrintTable = dataSet_Report.Rpt_GoodWay.Clone();
            if (chk_Box.Checked)
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    PrintTable.Rows.Add(mlt_Good.Text, item.Cells["Date"].Value.ToString(), item.Cells["Box"].Value.ToString(), item.Cells["ExitBox"].Value.ToString());
                }
            }
            else if (chk_Pack.Checked)
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    PrintTable.Rows.Add(mlt_Good.Text, item.Cells["Date"].Value.ToString(), item.Cells["Pack"].Value.ToString(), item.Cells["ExitPack"].Value.ToString());
                }
            }
            else if (chk_Detail.Checked)
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    PrintTable.Rows.Add(mlt_Good.Text, item.Cells["Date"].Value.ToString(), item.Cells["Detail"].Value.ToString(), item.Cells["ExitDetail"].Value.ToString());
                }

            }
            else if (chk_Total.Checked)
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    PrintTable.Rows.Add(mlt_Good.Text, item.Cells["Date"].Value.ToString(), item.Cells["OrderNumber"].Value.ToString(), item.Cells["ExitNumber"].Value.ToString());
                }

            }
            if (PrintTable.Rows.Count > 0)
            {
                _06_Reports._01_Orders.Report01_GoodWay Rpt = new Report01_GoodWay();
                TextObject Title = (TextObject)Rpt.ReportDefinition.ReportObjects["Text3"];
                Title.Text = "نمودار روند سفارشات محصول " + mlt_Good.Text + " از تاریخ " + Date1 +
                    " تا تاریخ " + Date2+"**نام استان:  "+mlt_Province.Text; 
                Rpt.SetDataSource(PrintTable);
                Rpt.SetDataSource(PrintTable);
                crystalReportViewer1.ReportSource = Rpt;
                crystalReportViewer1.Refresh();
                crystalReportViewer1.Zoom(1);
            }
            this.Cursor = Cursors.Default;
        }

       
    }
}
