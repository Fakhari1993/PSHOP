using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._06_Reports._01_Orders
{
    public partial class Form08_TotalOrderQtyRpt : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        string Date1 = null, Date2 = null;

        public Form08_TotalOrderQtyRpt()
        {
            InitializeComponent();
        }

        private void Form08_TotalOrderReport_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
        }

        private void faDatePickerStrip1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox = 
                    (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


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


        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && 
                faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                
                this.Cursor = Cursors.WaitCursor;
                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     View_06_GoodInfo.GoodId, View_06_GoodInfo.MainGroupName, View_06_GoodInfo.SubGroupName, View_06_GoodInfo.GoodCode, View_06_GoodInfo.GoodName, View_06_GoodInfo.InCartoon, 
                      ISNULL(View_01_TOrderSale.COrder, 0) AS COrder, ISNULL(View_02_TOrderCancel.CCancel, 0) AS CCancel, ISNULL(View_03_TOrderMali.CMali, 0) AS CMali, ISNULL(View_04_TInExit.CInExit, 0) 
                      AS CInExit, ISNULL(View_05_TOrderExit.CExit, 0) AS CExit
FROM         (SELECT     {2}.dbo.table_004_CommodityAndIngredients.columnid AS GoodId, {2}.dbo.table_004_CommodityAndIngredients.column01 AS GoodCode, 
                                              {2}.dbo.table_004_CommodityAndIngredients.column02 AS GoodName, {2}.dbo.table_004_CommodityAndIngredients.column09 AS InCartoon, 
                                              {2}.dbo.table_002_MainGroup.column02 AS MainGroupName, {2}.dbo.table_003_SubsidiaryGroup.column03 AS SubGroupName
                       FROM          {2}.dbo.table_002_MainGroup INNER JOIN
                                              {2}.dbo.table_003_SubsidiaryGroup ON {2}.dbo.table_002_MainGroup.columnid = {2}.dbo.table_003_SubsidiaryGroup.column01 INNER JOIN
                                              {2}.dbo.table_004_CommodityAndIngredients ON 
                                              {2}.dbo.table_003_SubsidiaryGroup.columnid = {2}.dbo.table_004_CommodityAndIngredients.column04) AS View_06_GoodInfo LEFT OUTER JOIN
                          (SELECT     Table_006_OrderDetails_5.column02, SUM(Table_006_OrderDetails_5.column16) AS CExit
                            FROM          dbo.Table_005_OrderHeader AS Table_005_OrderHeader_5 INNER JOIN
                                                   dbo.Table_006_OrderDetails AS Table_006_OrderDetails_5 ON Table_005_OrderHeader_5.columnid = Table_006_OrderDetails_5.column01
                            WHERE      (Table_005_OrderHeader_5.column13 = 0) AND (Table_005_OrderHeader_5.column02 BETWEEN N'{0}' AND N'{1}') AND (Table_005_OrderHeader_5.column33 = 1)
                            GROUP BY Table_006_OrderDetails_5.column02) AS View_05_TOrderExit ON View_06_GoodInfo.GoodId = View_05_TOrderExit.column02 LEFT OUTER JOIN
                          (SELECT     Table_006_OrderDetails_1.column02, SUM(Table_006_OrderDetails_1.column04) AS COrder
                            FROM          dbo.Table_005_OrderHeader AS Table_005_OrderHeader_1 INNER JOIN
                                                   dbo.Table_006_OrderDetails AS Table_006_OrderDetails_1 ON Table_005_OrderHeader_1.columnid = Table_006_OrderDetails_1.column01
                            WHERE      (Table_005_OrderHeader_1.column09 = 1) AND (Table_005_OrderHeader_1.column02 BETWEEN N'{0}' AND N'{1}')
                            GROUP BY Table_006_OrderDetails_1.column02) AS View_01_TOrderSale ON View_06_GoodInfo.GoodId = View_01_TOrderSale.column02 LEFT OUTER JOIN
                          (SELECT     Table_006_OrderDetails_4.column02, SUM(Table_006_OrderDetails_4.column04) AS CInExit
                            FROM          dbo.Table_005_OrderHeader AS Table_005_OrderHeader_4 INNER JOIN
                                                   dbo.Table_006_OrderDetails AS Table_006_OrderDetails_4 ON Table_005_OrderHeader_4.columnid = Table_006_OrderDetails_4.column01
                            WHERE      (Table_005_OrderHeader_4.column13 = 0) AND (Table_005_OrderHeader_4.column02 BETWEEN N'{0}' AND N'{1}') AND (Table_005_OrderHeader_4.column22 = 1) AND
                                                    (Table_005_OrderHeader_4.column33 = 0)
                            GROUP BY Table_006_OrderDetails_4.column02) AS View_04_TInExit ON View_06_GoodInfo.GoodId = View_04_TInExit.column02 LEFT OUTER JOIN
                          (SELECT     dbo.Table_006_OrderDetails.column02, SUM(dbo.Table_006_OrderDetails.column04) AS CMali
                            FROM          dbo.Table_005_OrderHeader INNER JOIN
                                                   dbo.Table_006_OrderDetails ON dbo.Table_005_OrderHeader.columnid = dbo.Table_006_OrderDetails.column01
                            WHERE      (dbo.Table_005_OrderHeader.column13 = 0) AND (dbo.Table_005_OrderHeader.column02 BETWEEN N'{0}' AND N'{1}') AND 
                                                   (dbo.Table_005_OrderHeader.column18 = 1)
                            GROUP BY dbo.Table_006_OrderDetails.column02) AS View_03_TOrderMali ON View_06_GoodInfo.GoodId = View_03_TOrderMali.column02 LEFT OUTER JOIN
                          (SELECT     Table_006_OrderDetails_2.column02, SUM(Table_006_OrderDetails_2.column04) AS CCancel
                            FROM          dbo.Table_005_OrderHeader AS Table_005_OrderHeader_2 INNER JOIN
                                                   dbo.Table_006_OrderDetails AS Table_006_OrderDetails_2 ON Table_005_OrderHeader_2.columnid = Table_006_OrderDetails_2.column01
                            WHERE      (Table_005_OrderHeader_2.column09 = 1) AND (Table_005_OrderHeader_2.column13 = 1) AND (Table_005_OrderHeader_2.column02 BETWEEN N'{0}' AND N'{1}')
                            GROUP BY Table_006_OrderDetails_2.column02) AS View_02_TOrderCancel ON View_06_GoodInfo.GoodId = View_02_TOrderCancel.column02
WHERE     (ISNULL(View_01_TOrderSale.COrder, 0) > 0) OR
                      (ISNULL(View_02_TOrderCancel.CCancel, 0) > 0) OR
                      (ISNULL(View_03_TOrderMali.CMali, 0) > 0) OR
                      (ISNULL(View_04_TInExit.CInExit, 0) > 0) OR
                      (ISNULL(View_05_TOrderExit.CExit, 0) > 0)", ConSale);

                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, 
                    Date1, Date2, 
                    ConWare.Database);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                gridEX1.DataSource = Table;
                this.Cursor = Cursors.Default;
                gridEX1.Row = gridEX1.FilterRow.Position;

            }

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
            {
                gridEXPrintDocument1.PageHeaderCenter = "گزارش تعداد سفارشات از تاریخ : " +
                    Date1 + " تا تاریخ : " +
                    Date2;
            }
        }

        private void Form08_TotalOrderReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Search_Click(sender, e);
        }

        private void Form08_TotalOrderReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
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



    }
}
