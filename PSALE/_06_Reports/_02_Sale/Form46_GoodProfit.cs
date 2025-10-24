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
    public partial class Form46_GoodProfit : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;

        public Form46_GoodProfit()
        {
            InitializeComponent();
        }

        private void DiffrenceReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
        }

        private void DiffrenceReport_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime=FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime( FarsiLibrary.Utils.PersianDate.Now.Year.ToString() + "/01/01");
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            bt_Display_Click(null, null);
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue  )
                {
                    string s = @"

SELECT *,
       finaltable.[count] * finaltable.buyprice AS buyamount,
       
        finaltable.saleamount -(finaltable.[count] * finaltable.buyprice) AS profit
FROM   (
           SELECT *,
                  (
                      CASE 
                           WHEN factortable.buyamount1 IS NOT NULL THEN 
                                factortable.buyamount1
                           WHEN factortable.buyamount1 IS NULL
                      AND factortable.buyamount2 IS NOT NULL THEN factortable.buyamount2
                          WHEN factortable.buyamount1 IS NULL
                      AND factortable.buyamount2 IS NULL
                      AND factortable.buyamount3 IS NOT NULL
                          THEN factortable.buyamount3
                          WHEN factortable.buyamount1 IS NULL
                      AND factortable.buyamount2 IS NULL
                      AND factortable.buyamount3 IS NULL THEN 
                          goodbuy 
                          
                          END
                  ) AS buyprice
           FROM   (
                      SELECT SUM(tcsf.column20) AS saleamount,
                             SUM(tcsf.column20) / SUM(tcsf.column07) AS saleprice,
                             tsf.column42 AS ware,
                             tsf.column02 AS fdate,
                             SUM(tcsf.column07) AS [count],
                             tcsf.column02 AS goodid,
                             tcai.column01 goodcode,
                             tcai.column02 goodname,
                             tsf.column01 AS factornum,
                             (
                                 SELECT TOP 1 tcbf.column20 / tcbf.column07
                                 FROM   Table_016_Child1_BuyFactor tcbf
                                        JOIN Table_015_BuyFactor tbf
                                             ON  tbf.columnid = tcbf.column01
                                 WHERE  tbf.column02 >= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                        AND tbf.column02 <= '" + faDatePickerStrip2.FADatePicker.Text + @"'
                                        AND tbf.column17 = 0
                                        AND tbf.column19 = 0
                                        AND tcbf.column02 = tcsf.column02
                                 ORDER BY
                                        tbf.column02 DESC
                             ) AS buyamount1,
                             (
                                 SELECT TOP 1 tcbf.column20 / tcbf.column07
                                 FROM   Table_016_Child1_BuyFactor tcbf
                                        JOIN Table_015_BuyFactor tbf
                                             ON  tbf.columnid = tcbf.column01
                                 WHERE  tbf.column02 <= '" + faDatePickerStrip2.FADatePicker.Text + @"'
                                        AND tbf.column17 = 0
                                        AND tbf.column19 = 0
                                        AND tcbf.column02 = tcsf.column02
                                 ORDER BY
                                        tbf.column02 DESC
                             ) AS buyamount2,
                             (
                                 SELECT TOP 1 tcbf.column20 / tcbf.column07
                                 FROM   Table_016_Child1_BuyFactor tcbf
                                        JOIN Table_015_BuyFactor tbf
                                             ON  tbf.columnid = tcbf.column01
                                 WHERE  tbf.column17 = 0
                                        AND tbf.column19 = 0
                                        AND tcbf.column02 = tcsf.column02
                                 ORDER BY
                                        tbf.column02 DESC
                             ) AS buyamount3,
                             tcai.column35 AS goodbuy
                        
                      FROM   Table_011_Child1_SaleFactor tcsf
                             JOIN Table_010_SaleFactor tsf
                                  ON  tsf.columnid = tcsf.column01
                             JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients 
                                  tcai
                                  ON  tcai.columnid = tcsf.column02
                      WHERE  tsf.column02 >= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                             AND tsf.column02 <= '" + faDatePickerStrip2.FADatePicker.Text + @"'
                             AND tsf.column17 = 0
                             AND tsf.column19 = 0
                      GROUP BY
                             tcsf.column02,
                             tsf.column01,
                             tcai.column01,
                             tcai.column02,
                             tsf.column42,
                             tsf.column02,
                             tcai.column35
                  ) factortable
       ) finaltable
 ";
                    DataTable sefareshTbl = clDoc.ReturnTable(ConSale.ConnectionString, s);
                    gridEX1.DataSource = sefareshTbl;
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                gridEXPrintDocument1.GridEX = gridEX1;
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        gridEXPrintDocument1.PageHeaderRight = "گزارش سود براساس فاکتور";
                        gridEXPrintDocument1.PageHeaderLeft = " از تاریخ " + faDatePickerStrip1.FADatePicker.Text + " تا تاریخ " + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageFooterLeft =
                      FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd") +
                      "**" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                        gridEXPrintDocument1.PageFooterRight =
                          " کاربر " + Class_BasicOperation._UserName;
                        printPreviewDialog1.ShowDialog();
                    }
            }
            catch { }
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

        private void Form46_GoodProfit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && e.Control)
                bt_Display_Click(sender, e);
            else if (e.KeyCode == Keys.P && e.Control)
            {
                bt_Print_Click(sender, e);

            }
            else if (e.Control && e.KeyCode == Keys.E)
                bt_ExportToExcel_Click(sender, e);
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

        private void faDatePickerStrip2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip2.FADatePicker.HideDropDown();
                    bt_Display_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePickerStrip2_TextChanged(object sender, EventArgs e)
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
    }
}
