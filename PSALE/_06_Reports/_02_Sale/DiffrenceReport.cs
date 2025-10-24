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
    public partial class DiffrenceReport : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();

        public DiffrenceReport()
        {
            InitializeComponent();
        }

        private void DiffrenceReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
        }

        private void DiffrenceReport_Load(object sender, EventArgs e)
        {

            bt_Display_Click(null, null);
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string s = @"
--10049077 Duplicatecode
  

SELECT *,
       good.SaleCount -good.ReturnSaleCount AS sale,
       good.BuyCount -good.ReturnBuyCount AS buy,
       good.SumWare + (good.BuyCount -good.ReturnBuyCount) -(good.SaleCount -good.ReturnSaleCount) AS 
       realremain,
       good.ReceiptCount -good.DrfatCount AS systemremian,
       (
           good.SumWare + (good.BuyCount -good.ReturnBuyCount) -(good.SaleCount -good.ReturnSaleCount)
       ) -(good.ReceiptCount -good.DrfatCount) AS diff
FROM   (
           SELECT tcai.columnid AS goodid,
                  tcai.column01 AS goodcode,
                  tcai.column02 AS goodname,
                  ISNULL(
                      (
                          SELECT TOP 1 mojoodi
                          FROM   StoreWare
                          WHERE  goodcode = tcai.column01
                      ),
                      0
                  ) AS StoreWare,
                  ISNULL(
                      (
                          SELECT TOP 1 mojoodi
                          FROM   MainWare
                          WHERE  goodcode = tcai.column01
                      ),
                      0
                  ) AS MainWare,
                  ISNULL(
                      (
                          SELECT TOP 1 mojoodi
                          FROM   MainWare
                          WHERE  goodcode = tcai.column01
                      ),
                      0
                  ) + ISNULL(
                      (
                          SELECT TOP 1 mojoodi
                          FROM   StoreWare
                          WHERE  goodcode = tcai.column01
                      ),
                      0
                  ) AS SumWare,
                  ISNULL(
                      (
                          SELECT SUM(tcsf.column07)
                          FROM   " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor 
                                 tcsf
                                 JOIN " + ConSale.Database + @".dbo.Table_010_SaleFactor tsf
                                      ON  tsf.columnid = tcsf.column01
                          WHERE  tcsf.column02 = tcai.columnid
                                 AND tsf.column17 = 0
                      ),
                      0
                  ) AS SaleCount,
                  ISNULL(
                      (
                          SELECT SUM(tcsf.column07)
                          FROM   " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale 
                                 tcsf
                                 JOIN " + ConSale.Database + @".dbo.Table_018_MarjooiSale tsf
                                      ON  tsf.columnid = tcsf.column01
                          WHERE  tcsf.column02 = tcai.columnid
                      ),
                      0
                  ) AS ReturnSaleCount,
                  ISNULL(
                      (
                          SELECT SUM(tcsf.column07)
                          FROM   " + ConSale.Database + @".dbo.Table_016_Child1_BuyFactor 
                                 tcsf
                                 JOIN " + ConSale.Database + @".dbo.Table_015_BuyFactor tsf
                                      ON  tsf.columnid = tcsf.column01
                          WHERE  tcsf.column02 = tcai.columnid
                                 AND tsf.column19 = 0
                      ),
                      0
                  ) AS BuyCount,
                  ISNULL(
                      (
                          SELECT SUM(tcsf.column07)
                          FROM   " + ConSale.Database + @".dbo.Table_022_Child1_MarjooiBuy 
                                 tcsf
                                 JOIN " + ConSale.Database + @".dbo.Table_021_MarjooiBuy tsf
                                      ON  tsf.columnid = tcsf.column01
                          WHERE  tcsf.column02 = tcai.columnid
                      ),
                      0
                  ) AS ReturnBuyCount,
                  ISNULL(
                      (
                          SELECT SUM(tcpd.column07)
                          FROM   Table_008_Child_PwhrsDraft tcpd
                                 JOIN Table_007_PwhrsDraft tpd
                                      ON  tpd.columnid = tcpd.column01
                          WHERE  tcpd.column02 = tcai.columnid
                      ),
                      0
                  ) AS DrfatCount,
                  ISNULL(
                      (
                          SELECT SUM(tcpd.column07)
                          FROM   Table_012_Child_PwhrsReceipt tcpd
                                 JOIN Table_011_PwhrsReceipt tpd
                                      ON  tpd.columnid = tcpd.column01
                          WHERE  tcpd.column02 = tcai.columnid
                      ),
                      0
                  ) AS ReceiptCount
           FROM   table_004_CommodityAndIngredients tcai
       ) AS good
 ";
                DataTable sefareshTbl = clDoc.ReturnTable(ConWare.ConnectionString, s);
                gridEX1.DataSource = sefareshTbl;
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid object name 'StoreWare'"))
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("این گزارش برای سیستم شما طراحی نشده است");
                    this.Close();
                }

                else
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                this.Cursor = Cursors.Default;
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
                        gridEXPrintDocument1.PageHeaderRight = "گزارش مغایرت موجودی واقعی و سیستم";

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

        private void DiffrenceReport_KeyDown(object sender, KeyEventArgs e)
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
    }
}
