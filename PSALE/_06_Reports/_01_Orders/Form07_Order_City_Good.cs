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
    
    public partial class Form07_Order_City_Good : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        BindingSource ProvinceBindingSource, CityBindingSource;
        string Date1 = null, Date2 = null;

        public Form07_Order_City_Good()
        {
            InitializeComponent();
        }
        private void Form03_OrderGoodsWay_Load(object sender, EventArgs e)
        {
            crystalReportViewer1.BackColor = Color.White;
            ProvinceBindingSource = new BindingSource();
            ProvinceBindingSource.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_060_ProvinceInfo");
            mlt_Province.DataSource = ProvinceBindingSource;

            CityBindingSource = new BindingSource();
            CityBindingSource.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_065_CityInfo");
            mlt_City.DataSource = CityBindingSource;
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            SqlDataAdapter Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients", ConWare);
            DataTable GoodTable = new DataTable();
            Adapter.Fill(GoodTable);
            gridEX1.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX1.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
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

            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue && mlt_Province.Text.Trim() != "")
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
              
                this.Cursor = Cursors.WaitCursor; 
                SqlDataAdapter Adapter;
                if(mlt_City.Text.Trim()=="")
                {
                    Adapter = new SqlDataAdapter(@"SELECT     TOP (100) PERCENT dbo.Table_006_OrderDetails.column02 AS GoodID, SUM(dbo.Table_006_OrderDetails.column03) AS Order_Pack, 
                    SUM(dbo.Table_006_OrderDetails.column04) AS Order_Box, SUM(dbo.Table_006_OrderDetails.column05) AS Order_Detail, 
                    SUM(dbo.Table_006_OrderDetails.column06) AS Order_Total, ProvinceTable.Column01 AS ProvinceName, ProvinceTable.Column00 AS ProvinceID, 
                    SUM(dbo.Table_006_OrderDetails.column14) AS Exit_Detail, SUM(dbo.Table_006_OrderDetails.column15) AS Exit_Pack, 
                    SUM(dbo.Table_006_OrderDetails.column16) AS Exit_Box, SUM(dbo.Table_006_OrderDetails.column17) AS Exit_Total, GoodTable.MainGroup, 
                    GoodTable.SubGroup
                    FROM         (SELECT     {4}.dbo.table_004_CommodityAndIngredients.columnid, {4}.dbo.table_004_CommodityAndIngredients.column01, 
                    {4}.dbo.table_004_CommodityAndIngredients.column02, {4}.dbo.table_002_MainGroup.column02 AS MainGroup, 
                    {4}.dbo.table_003_SubsidiaryGroup.column03 AS SubGroup
                    FROM         {4}.dbo.table_004_CommodityAndIngredients INNER JOIN
                    {4}.dbo.table_003_SubsidiaryGroup ON 
                    {4}.dbo.table_004_CommodityAndIngredients.column04 = {4}.dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
                    {4}.dbo.table_002_MainGroup ON 
                    {4}.dbo.table_003_SubsidiaryGroup.column01 = {4}.dbo.table_002_MainGroup.columnid) AS GoodTable RIGHT OUTER JOIN
                    dbo.Table_005_OrderHeader INNER JOIN
                    dbo.Table_006_OrderDetails ON dbo.Table_005_OrderHeader.columnid = dbo.Table_006_OrderDetails.column01 ON 
                    GoodTable.columnid = dbo.Table_006_OrderDetails.column02 LEFT OUTER JOIN
                    (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
                    FROM         {3}.dbo.Table_060_ProvinceInfo) AS ProvinceTable INNER JOIN
                    (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                    FROM         {3}.dbo.Table_065_CityInfo) AS CityTable ON ProvinceTable.Column00 = CityTable.Column00 ON 
                    dbo.Table_005_OrderHeader.column05 = CityTable.Column01
                    WHERE     (dbo.Table_005_OrderHeader.column02 >= '{0}') AND (dbo.Table_005_OrderHeader.column02 <= '{1}') AND (ProvinceTable.Column00 = {2})
                    GROUP BY dbo.Table_006_OrderDetails.column02, ProvinceTable.Column01, ProvinceTable.Column00, GoodTable.MainGroup, GoodTable.SubGroup", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, Date1, 
                    Date2, mlt_Province.Value.ToString(),ConBase.Database);
                }
                 else 
                {
                    Adapter = new SqlDataAdapter(@"SELECT     TOP (100) PERCENT dbo.Table_006_OrderDetails.column02 AS GoodID, SUM(dbo.Table_006_OrderDetails.column03) AS Order_Pack, 
                      SUM(dbo.Table_006_OrderDetails.column04) AS Order_Box, SUM(dbo.Table_006_OrderDetails.column05) AS Order_Detail, 
                      SUM(dbo.Table_006_OrderDetails.column06) AS Order_Total, dbo.Table_005_OrderHeader.column05 AS CityID, CityTable.Column02 AS CityName, 
                      ProvinceTable.Column01 AS ProvinceName, ProvinceTable.Column00 AS ProvinceID, SUM(dbo.Table_006_OrderDetails.column14) AS Exit_Detail, 
                      SUM(dbo.Table_006_OrderDetails.column15) AS Exit_Pack, SUM(dbo.Table_006_OrderDetails.column16) AS Exit_Box, SUM(dbo.Table_006_OrderDetails.column17) 
                      AS Exit_Total, GoodTable.MainGroup, GoodTable.SubGroup
FROM         (SELECT     {5}.dbo.table_004_CommodityAndIngredients.columnid, {5}.dbo.table_004_CommodityAndIngredients.column01, 
                                              {5}.dbo.table_004_CommodityAndIngredients.column02, {5}.dbo.table_002_MainGroup.column02 AS MainGroup, 
                                              {5}.dbo.table_003_SubsidiaryGroup.column03 AS SubGroup
                        FROM         {5}.dbo.table_004_CommodityAndIngredients INNER JOIN
                                              {5}.dbo.table_003_SubsidiaryGroup ON 
                                              {5}.dbo.table_004_CommodityAndIngredients.column04 = {5}.dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
                                              {5}.dbo.table_002_MainGroup ON 
                                              {5}.dbo.table_003_SubsidiaryGroup.column01 = {5}.dbo.table_002_MainGroup.columnid) AS GoodTable RIGHT OUTER JOIN
                      dbo.Table_005_OrderHeader INNER JOIN
                      dbo.Table_006_OrderDetails ON dbo.Table_005_OrderHeader.columnid = dbo.Table_006_OrderDetails.column01 ON 
                      GoodTable.columnid = dbo.Table_006_OrderDetails.column02 LEFT OUTER JOIN
                          (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
                             FROM         {4}.dbo.Table_060_ProvinceInfo) AS ProvinceTable INNER JOIN
                          (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                             FROM         {4}.dbo.Table_065_CityInfo) AS CityTable ON ProvinceTable.Column00 = CityTable.Column00 ON 
                      dbo.Table_005_OrderHeader.column05 = CityTable.Column01
WHERE     (dbo.Table_005_OrderHeader.column02 >= '{0}') AND (dbo.Table_005_OrderHeader.column02 <= '{1}') AND (ProvinceTable.Column00 = {2}) AND 
                      (dbo.Table_005_OrderHeader.column05 = {3})
GROUP BY dbo.Table_006_OrderDetails.column02, dbo.Table_005_OrderHeader.column05, CityTable.Column02, ProvinceTable.Column01, ProvinceTable.Column00, 
                      GoodTable.MainGroup, GoodTable.SubGroup", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, Date1,
                    Date2, mlt_Province.Value.ToString(), mlt_City.Value.ToString(), ConBase.Database,ConWare.Database);
                }

                
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                gridEX1.DataSource = Table;


                DataTable PrintTable = dataSet_Report.Rpt_GoodWay.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    PrintTable.Rows.Add(mlt_Province.Text, item.Cells["GoodName"].Text.ToString(), item.Cells["Order_Box"].Value.ToString(), item.Cells["Exit_Box"].Value.ToString());
                }
                if (PrintTable.Rows.Count > 0)
                {
                    _06_Reports._01_Orders.Report01_GoodWay Rpt = new Report01_GoodWay();
                    TextObject Title = (TextObject)Rpt.ReportDefinition.ReportObjects["Text3"];
                    if (mlt_City.Text.Trim() != "")
                    {
                        Title.Text = "نمودار سفارش محصولات در شهرستان " + mlt_City.Text + " استان " + mlt_Province.Text + " از تاریخ " + Date1 + " تا تاریخ " + Date2;
                    }
                    else
                    {
                        Title.Text = "نمودار سفارش محصولات در استان " + mlt_Province.Text + " از تاریخ " + Date1 + " تا تاریخ " + Date2;
                    }
                    Rpt.SetDataSource(PrintTable);
                    crystalReportViewer1.ReportSource = Rpt;
                    crystalReportViewer1.Refresh();
                    crystalReportViewer1.Zoom(1);
                }

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
                mlt_Province.Select();
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
                mlt_Province.DroppedDown = true;
            else if (e.KeyChar == 13)
                mlt_City.Select();

        }

        private void bt_PrintChart_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.PrintReport();
        }

        private void mlt_Province_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                CityBindingSource.Filter = "Column00=" + mlt_Province.Value.ToString();
            }
            catch {
                CityBindingSource.RemoveFilter();
            }

        }

        private void mlt_City_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                mlt_City.DroppedDown = true;
            else if (e.KeyChar == 13)
                faDatePickerStrip1.FADatePicker.Select();
        }

        private void bt_PrintTable_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_GoodWay.Clone();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                Table.Rows.Add(item.Cells["GoodName"].Text, item.Cells["GoodCode"].Text, item.Cells["Order_Box"].Value.ToString(), item.Cells["Exit_Box"].Value.ToString(), "استان:"+mlt_Province.Text+(mlt_City.Text.Trim()==""?"":"                  شهرستان:"+mlt_City.Text));
            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 6, Date1, Date2);
                frm.ShowDialog();
            }
        }

        private void chk_Box_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Total.Checked)
            {
                gridEX1.RootTable.Columns["Order_Total"].Visible = true;
                gridEX1.RootTable.Columns["Exit_Total"].Visible = true;
            }
            else
            {
                gridEX1.RootTable.Columns["Order_Total"].Visible = false;
                gridEX1.RootTable.Columns["Exit_Total"].Visible = false;
            }

            if (chk_Box.Checked)
            {
                gridEX1.RootTable.Columns["Order_Box"].Visible = true;
                gridEX1.RootTable.Columns["Exit_Box"].Visible = true;
            }
            else
            {
                gridEX1.RootTable.Columns["Order_Box"].Visible = false;
                gridEX1.RootTable.Columns["Exit_Box"].Visible = false;
            }
        }

    }
}
