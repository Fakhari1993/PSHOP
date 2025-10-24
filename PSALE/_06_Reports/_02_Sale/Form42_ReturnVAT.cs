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
    public partial class Form42_ReturnVAT : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();

        public Form42_ReturnVAT()
        {


            InitializeComponent();

            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-3);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            string command = @"SELECT tpi.Column02 AS [name],
                                       (CASE WHEN tpi.Column03 = 0 THEN N'حقيقي' ELSE N'حقوقي' END) AS [tyep],
                                       tpi.Column13 AS postalcode,
                                       tpi.Column09 AS nationalcode,
                                       tpi.Column06 AS [address],
                                       tpi.Column141 AS Ecode,
                                       tpi2.Column00 AS ostancode,
                                       tpi2.Column01 AS ostanname,
                                       tci.Column01 AS citycode,
                                       tci.Column02 AS cityname,
                                       tcai.column02 AS [kala],
                                       tsf.column02 AS [date],
                                       tcsf.column23 AS [desc],
                                       tsf.column01 AS Num,
                                       tcsf.column11 AS Totalvalue,
                                       tcsf.column17 AS [discount],
                                     (case when tcai.Column49=0 then 0 else (( (tcsf.column11 - tcsf.column17) *(" + Convert.ToDecimal(Properties.Settings.Default.tax) + @"))/100) end )AS tax,
                                     (case when tcai.Column49=0 then 0 else (( (tcsf.column11 - tcsf.column17) *(" + Convert.ToDecimal(Properties.Settings.Default.taxes) + @"))/100)end ) AS taxes,
                                       tsf.column09 AS [havale],
                                       tsf.column10 AS [sanad]
                                FROM  Table_019_Child1_MarjooiSale  tcsf
                                       JOIN  Table_018_MarjooiSale tsf
                                            ON  tsf.columnid = tcsf.column01
                                       JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                            ON  tcai.columnid = tcsf.column02
                                       JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi
                                            ON  tpi.ColumnId = tsf.column03
                                       LEFT JOIN " + ConBase.Database + @".dbo.Table_060_ProvinceInfo tpi2
                                            ON  tpi2.Column00 = tpi.Column21
                                       LEFT  JOIN " + ConBase.Database + @".dbo.Table_065_CityInfo tci
                                            ON  tci.Column01 = tpi.Column22
            
                                WHERE    tsf.column02>='" + faDatePickerStrip1.FADatePicker.Text + "' AND tsf.column02<='" + faDatePickerStrip2.FADatePicker.Text + "' AND tsf.column17=0 ";

            gridEXGroup.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, command);
        }

        private void Form41_VAT_Load(object sender, EventArgs e)
        {

        }

        private void Form41_VAT_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEXGroup.RemoveFilters();
        }

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEXGroup;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                gridEXPrintDocument1.GridEX = gridEXGroup;
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string j = " از تاریخ:" + faDatePickerStrip1.FADatePicker.Text + " تا تاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageHeaderLeft = j;
                        gridEXPrintDocument1.PageHeaderRight = "گزارش مالیات بر ارزش افزوده مروجوعی فروش";
                        printPreviewDialog1.ShowDialog();
                    }
            }
            catch { }
        }
    }
}
