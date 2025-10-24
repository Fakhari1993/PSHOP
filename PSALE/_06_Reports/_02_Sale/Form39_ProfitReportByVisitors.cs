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
    public partial class Form39_ProfitReportByVisitors : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        bool _BackSpace = false;
        string Date1, Date2;
        public Form39_ProfitReportByVisitors()
        {
            InitializeComponent();
        }

        private void Form38_ProfitReportByGoodGroup_Load(object sender, EventArgs e)
        {
            string[] Dates = Properties.Settings.Default.ProfitReportByVisitors.Split('-');
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
                Properties.Settings.Default.ProfitReportByVisitors = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
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
                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT (SUM(g.TotalPrice) -SUM(g.ReturnTotal) -SUM(g.TotalValue)) AS totalsood,
                                                                                   (SUM(g.NetPrice) -SUM(g.ReturnNet) -SUM(g.TotalValue)) AS netsood,
                                                                                   SUM(g.NetPrice) AS NetPrice,
                                                                                   SUM(g.TotalPrice) AS TotalPrice,
                                                                                   SUM(g.ReturnNet) AS ReturnNet,
                                                                                   SUM(g.ReturnTotal) AS ReturnTotal,
                                                                                   SUM(g.TotalValue) AS TotalValue,
                                                                                   g.column05 AS visitorId,
                                                                                   tpi.Column02 AS visitor,
                                                                                   isnull((SUM(g.TotalPrice) -SUM(g.ReturnTotal) -SUM(g.TotalValue)),0)/nullif(SUM(isnull(g.TotalValue,0)),0) *100 as totaldarsad,
                                                                                   isnull((SUM(g.NetPrice) -SUM(g.ReturnNet) -SUM(g.TotalValue)),0)/nullif(SUM(isnull(g.TotalValue,0)),0) *100  as netdarsad

                                                                            FROM   (
                                                                                       SELECT tsf.column05,
                                                                                              (
                                                                                                  tsf.Column28 - tsf.Column29 -tsf.Column30 - tsf.Column31 +
                                                                                                  tsf.Column32 
                                                                                                  - tsf.Column33
                                                                                              ) AS NetPrice,
                                                                                             (SELECT sum(tcsf.column11) from Table_011_Child1_SaleFactor tcsf where tcsf.column01 = tsf.columnid) AS TotalPrice,
                                                                                              0 AS ReturnNet,
                                                                                              0 AS ReturnTotal,
                                                                                              0 AS TotalValue
                                                                                       FROM   Table_010_SaleFactor tsf
                                                                                        WHERE tsf.column02 BETWEEN   '" + Date1 + "' AND '" + Date2 + @"'
                                                                                                AND tsf.column17 = 0  
                                                                                       UNION all 
                                                                                       SELECT tms.column05,
                                                                                              0 AS NetPrice,
                                                                                              0 AS TotalPrice,
                                                                                              (tms.Column18 + tms.Column19 -tms.Column20) AS ReturnNet,
                                                                                              (select sum(tcms.column11) from Table_019_Child1_MarjooiSale tcms where tcms.column01 = tms.columnid) AS ReturnTotal,
                                                                                              0 AS TotalValue
                                                                                       FROM   Table_018_MarjooiSale tms
                                                                                        WHERE tms.column02 BETWEEN   '" + Date1 + "' AND '" + Date2 + @"'
                                                                                       UNION all SELECT tsf.column05,
                                                                                                    0 AS NetPrice,
                                                                                                    0 AS TotalPrice,
                                                                                                    0 AS ReturnNet,
                                                                                                    0 AS ReturnTotal,
                                                                                                    (select sum(tcpd.column16) from  " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft tcpd where     tcpd.column01 = tsf.column09) AS TotalValue
                                                                                             FROM   
                                                                                                      Table_010_SaleFactor tsf
                                                                                        WHERE tsf.column02 BETWEEN   '" + Date1 + "' AND '" + Date2 + @"' AND tsf.column17 = 0
                                                                                   ) AS g
                                                                                   LEFT JOIN " +ConBase.Database+@".dbo.Table_045_PersonInfo tpi
                                                                                        ON  tpi.ColumnId = g.column05
                                                                            GROUP BY
                                                                                   g.column05,
                                                                                   tpi.Column02");

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
                        gridEXPrintDocument1.PageHeaderRight = " گزارش سود مسئولین فروش  ";
                        printPreviewDialog1.ShowDialog();
                    }
            }
            catch { }
        }
    }

}
