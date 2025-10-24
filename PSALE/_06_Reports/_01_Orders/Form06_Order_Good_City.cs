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
    
    public partial class Form06_Order_Good_City : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        string Date1 = null, Date2 = null;

        public Form06_Order_Good_City()
        {
            InitializeComponent();
        }
        private void Form03_OrderGoodsWay_Load(object sender, EventArgs e)
        {
            crystalReportViewer1.BackColor = Color.White;
            crystalReportViewer2.BackColor = Color.White;
            mlt_Good.DataSource = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT     dbo.table_004_CommodityAndIngredients.columnid, dbo.table_004_CommodityAndIngredients.column01, dbo.table_004_CommodityAndIngredients.column02, 
            dbo.table_002_MainGroup.column02 AS MainGroup, dbo.table_003_SubsidiaryGroup.column03 AS SubGroup
            FROM         dbo.table_004_CommodityAndIngredients INNER JOIN
            dbo.table_003_SubsidiaryGroup ON dbo.table_004_CommodityAndIngredients.column04 = dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
            dbo.table_002_MainGroup ON dbo.table_003_SubsidiaryGroup.column01 = dbo.table_002_MainGroup.columnid");
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            chk_Box.Checked = true;
            chk_Total.Checked = false;

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
           
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue && mlt_Good.Text.Trim()!="")
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                this.Cursor = Cursors.WaitCursor;
                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     TOP (100) PERCENT dbo.Table_006_OrderDetails.column02 AS GoodID, SUM(dbo.Table_006_OrderDetails.column03) AS Order_Pack, 
                    SUM(dbo.Table_006_OrderDetails.column04) AS Order_Box, SUM(dbo.Table_006_OrderDetails.column05) AS Order_Detail, 
                    SUM(dbo.Table_006_OrderDetails.column06) AS Order_Total, ProvinceTable.Column01 AS ProvinceName, ProvinceTable.Column00 AS ProvinceID, 
                    SUM(dbo.Table_006_OrderDetails.column14) AS Exit_Detail, SUM(dbo.Table_006_OrderDetails.column15) AS Exit_Pack, 
                    SUM(dbo.Table_006_OrderDetails.column16) AS Exit_Box, SUM(dbo.Table_006_OrderDetails.column17) AS Exit_Total
                    FROM         (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
                    FROM         {3}.dbo.Table_060_ProvinceInfo) AS ProvinceTable INNER JOIN
                    (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                    FROM         {3}.dbo.Table_065_CityInfo) AS CityTable ON ProvinceTable.Column00 = CityTable.Column00 RIGHT OUTER JOIN
                    dbo.Table_005_OrderHeader INNER JOIN
                    dbo.Table_006_OrderDetails ON dbo.Table_005_OrderHeader.columnid = dbo.Table_006_OrderDetails.column01 ON 
                    CityTable.Column01 = dbo.Table_005_OrderHeader.column05
                    WHERE     (dbo.Table_005_OrderHeader.column02 >= '{0}') AND (dbo.Table_005_OrderHeader.column02 <= '{1}') AND 
                    (dbo.Table_006_OrderDetails.column02 = {2}
)
                    GROUP BY dbo.Table_006_OrderDetails.column02, ProvinceTable.Column01, ProvinceTable.Column00", ConSale);
                
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, Date1, Date2, mlt_Good.Value.ToString(), ConBase.Database);
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);
                    gridEX1.DataSource = Table;


                    
                Adapter = new SqlDataAdapter(@"SELECT     TOP (100) PERCENT dbo.Table_006_OrderDetails.column02 AS GoodID, SUM(dbo.Table_006_OrderDetails.column03) AS Order_Pack, 
                    SUM(dbo.Table_006_OrderDetails.column04) AS Order_Box, SUM(dbo.Table_006_OrderDetails.column05) AS Order_Detail, 
                    SUM(dbo.Table_006_OrderDetails.column06) AS Order_Total, dbo.Table_005_OrderHeader.column05 AS CityID, CityTable.Column02 AS CityName, 
                    ProvinceTable.Column01 AS ProvinceName, ProvinceTable.Column00 AS ProvinceID, SUM(dbo.Table_006_OrderDetails.column14) AS Exit_Detail, 
                    SUM(dbo.Table_006_OrderDetails.column15) AS Exit_Pack, SUM(dbo.Table_006_OrderDetails.column16) AS Exit_Box, SUM(dbo.Table_006_OrderDetails.column17) 
                    AS Exit_Total
                    FROM         (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
                    FROM         {3}.dbo.Table_060_ProvinceInfo) AS ProvinceTable INNER JOIN
                    (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                    FROM         {3}.dbo.Table_065_CityInfo) AS CityTable ON ProvinceTable.Column00 = CityTable.Column00 RIGHT OUTER JOIN
                    dbo.Table_005_OrderHeader INNER JOIN
                    dbo.Table_006_OrderDetails ON dbo.Table_005_OrderHeader.columnid = dbo.Table_006_OrderDetails.column01 ON 
                    CityTable.Column01 = dbo.Table_005_OrderHeader.column05
                    WHERE     (dbo.Table_005_OrderHeader.column02 >= '{0}') AND (dbo.Table_005_OrderHeader.column02 <= '{1}') AND 
                    (dbo.Table_006_OrderDetails.column02 = {2})
                    GROUP BY dbo.Table_006_OrderDetails.column02, dbo.Table_005_OrderHeader.column05, CityTable.Column02, ProvinceTable.Column01, ProvinceTable.Column00", ConSale);
                    Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, Date1, Date2, mlt_Good.Value.ToString(), ConBase.Database);
                    DataTable DetailTable = new DataTable();
                    Adapter.Fill(DetailTable);
                    bindingSource1.DataSource = DetailTable;
                    gridEX2.DataSource = DetailTable;


                    DataTable PrintTable= dataSet_Report.Rpt_GoodWay.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                    {
                        PrintTable.Rows.Add(mlt_Good.Text, item.Cells["ProvinceName"].Text.ToString(), item.Cells["Order_Box"].Value.ToString(), item.Cells["Exit_Box"].Value.ToString());
                    }
                    if (PrintTable.Rows.Count > 0)
                    {
                        _06_Reports._01_Orders.Report01_GoodWay Rpt = new Report01_GoodWay();
                        TextObject Title = (TextObject)Rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار سفارشات " + mlt_Good.Text + " در استانها";
                        Rpt.SetDataSource(PrintTable);
                        crystalReportViewer1.ReportSource = Rpt;
                        crystalReportViewer1.Refresh();
                        crystalReportViewer1.Zoom(1);
                    }
                    gridEX1_CurrentCellChanged(sender, e);
              //*****************Detail Chart
                 

                

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
            else if(e.KeyChar==13)
            faDatePickerStrip1.FADatePicker.Select();

        }

        private void bt_PrintChart_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.PrintReport();
        }


        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.RemoveFilter();
                bindingSource1.Filter = "ProvinceID=" + gridEX1.GetValue("ProvinceID").ToString();
                DataTable DetailPrint = dataSet_Report.Rpt_GoodWay.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
                {
                    DetailPrint.Rows.Add(mlt_Good.Text, item.Cells["CityName"].Text.ToString(), item.Cells["Order_Box"].Value.ToString(), item.Cells["Exit_Box"].Value.ToString());
                }
                if (DetailPrint.Rows.Count > 0)
                {
                    _06_Reports._01_Orders.Report01_GoodWay Rpt = new Report01_GoodWay();
                    TextObject Title = (TextObject)Rpt.ReportDefinition.ReportObjects["Text3"];
                    Title.Text = "نمودار سفارشات " + mlt_Good.Text + " در شهرستانهای استان " + gridEX1.GetValue("ProvinceName").ToString() + " از تاریخ " + Date1 + " تا تاریخ " + Date2; ;
                    Rpt.SetDataSource(DetailPrint);
                    crystalReportViewer2.ReportSource = Rpt;
                    crystalReportViewer2.Refresh();
                    crystalReportViewer2.Zoom(1);
                }
            }
            catch { }
        }

        private void mnu_CityChart_Click(object sender, EventArgs e)
        {
            crystalReportViewer2.PrintReport();
        }

        private void bt_PrintTable_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_Order_Province.Clone();
            bindingSource1.RemoveFilter();
            foreach (DataRowView item in bindingSource1)
            {
                Table.Rows.Add(item["ProvinceName"].ToString(), item["CityName"].ToString(),
                   item["Order_Box"].ToString(), item["Exit_Box"].ToString(), mlt_Good.Text);
            }
            gridEX1_CurrentCellChanged(sender, e);
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 5, Date1, Date2);
                frm.ShowDialog();
            }
        }

        private void chk_Box_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Total.Checked)
            {
                gridEX1.RootTable.Columns["Order_Total"].Visible = true;
                gridEX1.RootTable.Columns["Exit_Total"].Visible = true;
                gridEX2.RootTable.Columns["Order_Total"].Visible = true;
                gridEX2.RootTable.Columns["Exit_Total"].Visible = true;
            }
            else
            {
                gridEX1.RootTable.Columns["Order_Total"].Visible = false;
                gridEX1.RootTable.Columns["Exit_Total"].Visible = false;
                gridEX2.RootTable.Columns["Order_Total"].Visible = false;
                gridEX2.RootTable.Columns["Exit_Total"].Visible = false;
            }

            if (chk_Box.Checked)
            {
                gridEX1.RootTable.Columns["Order_Box"].Visible = true;
                gridEX1.RootTable.Columns["Exit_Box"].Visible = true;

                gridEX2.RootTable.Columns["Order_Box"].Visible = true;
                gridEX2.RootTable.Columns["Exit_Box"].Visible = true;
            }
            else
            {
                gridEX1.RootTable.Columns["Order_Box"].Visible = false;
                gridEX1.RootTable.Columns["Exit_Box"].Visible = false;
                gridEX2.RootTable.Columns["Order_Box"].Visible = false;
                gridEX2.RootTable.Columns["Exit_Box"].Visible = false;
            }
        }

       
    }
}
