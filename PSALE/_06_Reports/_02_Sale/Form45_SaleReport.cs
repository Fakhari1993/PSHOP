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
    public partial class Form45_SaleReport : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        public Form45_SaleReport()
        {
            InitializeComponent();

            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-3);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
        }

        private void Form45_SaleReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(faDatePickerStrip1.FADatePicker.Text) && !string.IsNullOrWhiteSpace(faDatePickerStrip2.FADatePicker.Text))
                {
                    string command = @"SELECT tcai.column01 AS GoodCode,main.column02 as MainGroup,sub.column03 as SubGroup,
                                   tcai.column02 AS GoodName,
                                   tcui.Column01 AS GoodUnit,
                                   SUM(tcsf.column04) AS Karton,
                                   SUM(tcsf.column05) AS Baste,
                                   SUM(tcsf.column06) AS Joz,
                                   SUM(tcsf.column07) AS Kol
                            FROM   Table_011_Child1_SaleFactor tcsf
                                   JOIN Table_010_SaleFactor tsf
                                        ON  tsf.columnid = tcsf.column01
                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                        ON  tcai.columnid = tcsf.column02
                                   JOIN " + ConBase.Database + @".dbo.Table_070_CountUnitInfo tcui
                                        ON  tcui.Column00 = tcsf.column03
                                    left join " + ConWare.Database + @".dbo.table_002_MainGroup main on main.columnid=tcai.column03
                                    left join " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup sub on sub.columnid=tcai.column04

                            WHERE  tsf.column17 = 0
                                   AND tsf.column19 = 0 AND tsf.column02 >='" + faDatePickerStrip1.FADatePicker.Text + @"' AND tsf.column02<='" + faDatePickerStrip2.FADatePicker.Text + @"'
                            GROUP BY
                                   tcai.column01,
                                   tcai.column02,
                                   tcui.Column01,main.column02,sub.column03 ";

                    this.gridEX1.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, command);
                }
            }
            catch(Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }

        private void cmb_Print_Click(object sender, EventArgs e)
        {
            try
            {
                gridEXPrintDocument1.GridEX = gridEX1;
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string j = " از تاریخ:" + faDatePickerStrip1.FADatePicker.Text + " تا تاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageHeaderLeft = j;
                        gridEXPrintDocument1.PageHeaderRight = "گزارش تعدادی فروش";
                        printPreviewDialog1.ShowDialog();
                    }
            }
            catch { }
        }

        private void cmb_ExportToExcel_Click(object sender, EventArgs e)
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
