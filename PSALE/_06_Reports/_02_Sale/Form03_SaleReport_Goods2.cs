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
    public partial class Form03_SaleReport_Goods2 : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);

        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        string Date1, Date2;

        public Form03_SaleReport_Goods2()
        {
            InitializeComponent();
        }

        private void Form03_SaleReport_Goods_Load(object sender, EventArgs e)
        {
            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-2);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            string[] Dates = Properties.Settings.Default.SaleReport_Goods.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);




            cmb_Cancel.SelectedIndex = 0;
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
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                if (cmb_Cancel.ComboBox.SelectedIndex == 0)
                {

                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT tmg.column02 AS MainGroup,
                                                                           tsg.column03 AS SubGroup,
                                                                           tcai.column01 AS GoodCode,
                                                                           tcai.column02 AS GoodName,
                                                                           k.column02 AS GoodID,
                                                                           SUM(k.column04) AS BoxNumber,
                                                                           SUM(k.column05) AS PackNumber,
                                                                           SUM(k.column06) AS DetailNumber,
                                                                           SUM(k.column07) AS TotalNumber,
                                                                           SUM(k.column11) AS TotalPrice,
                                                                           SUM(k.column17) AS Discount,
                                                                           SUM(k.column19) AS Extra,
                                                                           SUM(k.column20) AS Net,
                                                                           SUM(k.column20) / NULLIF(SUM(k.column07), 0) AS Fee
                                                                    FROM   Table_011_Child1_SaleFactor k
                                                                            join Table_010_SaleFactor l on l.columnid=k.column01
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                                ON  tcai.columnid = k.column02
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                                                ON  tsg.column01 = tcai.column03
                                                                                AND tsg.columnid = tcai.column04
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                                                ON  tmg.columnid = tsg.column01
                                                                            where l.column02>='" + Date1 + @"' and l.column02<='" + Date2 + @"'
                                                                    GROUP BY
                                                                           k.column02,
                                                                           tmg.column02,
                                                                           tsg.column03,
                                                                           tcai.column01,
                                                                           tcai.column02", ConSale);
                    DataTable Goods = new DataTable();
                    Adapter.Fill(Goods);
                    gridEX_Goods.DataSource = Goods;

                }
                else
                {
                    SqlDataAdapter Adapter;
                    DataTable Goods;
                    switch (cmb_Cancel.ComboBox.SelectedIndex)
                    {
                        case 1:
                            Adapter = new SqlDataAdapter(@"SELECT tmg.column02 AS MainGroup,
                                                                           tsg.column03 AS SubGroup,
                                                                           tcai.column01 AS GoodCode,
                                                                           tcai.column02 AS GoodName,
                                                                           k.column02 AS GoodID,
                                                                           SUM(k.column04) AS BoxNumber,
                                                                           SUM(k.column05) AS PackNumber,
                                                                           SUM(k.column06) AS DetailNumber,
                                                                           SUM(k.column07) AS TotalNumber,
                                                                           SUM(k.column11) AS TotalPrice,
                                                                           SUM(k.column17) AS Discount,
                                                                           SUM(k.column19) AS Extra,
                                                                           SUM(k.column20) AS Net,
                                                                           SUM(k.column20) / NULLIF(SUM(k.column07), 0) AS Fee
                                                                    FROM   Table_011_Child1_SaleFactor k JOIN Table_010_SaleFactor tsf ON tsf.columnid = k.column01
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                                ON  tcai.columnid = k.column02
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                                                ON  tsg.column01 = tcai.column03
                                                                                AND tsg.columnid = tcai.column04
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                                                ON  tmg.columnid = tsg.column01
                                                                    WHERE tsf.column17=1 and tsf.column02>='" + Date1 + @"' and tsf.column02<='" + Date2 + @"'
                                                                    GROUP BY
                                                                           k.column02,
                                                                           tmg.column02,
                                                                           tsg.column03,
                                                                           tcai.column01,
                                                                           tcai.column02", ConSale);
                            Goods = new DataTable();
                            Adapter.Fill(Goods);
                            gridEX_Goods.DataSource = Goods;

                            break;
                        case 2:
                            Adapter = new SqlDataAdapter(@"SELECT tmg.column02 AS MainGroup,
                                                                           tsg.column03 AS SubGroup,
                                                                           tcai.column01 AS GoodCode,
                                                                           tcai.column02 AS GoodName,
                                                                           k.column02 AS GoodID,
                                                                           SUM(k.column04) AS BoxNumber,
                                                                           SUM(k.column05) AS PackNumber,
                                                                           SUM(k.column06) AS DetailNumber,
                                                                           SUM(k.column07) AS TotalNumber,
                                                                           SUM(k.column11) AS TotalPrice,
                                                                           SUM(k.column17) AS Discount,
                                                                           SUM(k.column19) AS Extra,
                                                                           SUM(k.column20) AS Net,
                                                                           SUM(k.column20) / NULLIF(SUM(k.column07), 0) AS Fee
                                                                    FROM   Table_011_Child1_SaleFactor k JOIN Table_010_SaleFactor tsf ON tsf.columnid = k.column01
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                                ON  tcai.columnid = k.column02
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                                                ON  tsg.column01 = tcai.column03
                                                                                AND tsg.columnid = tcai.column04
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                                                ON  tmg.columnid = tsg.column01
                                                                    WHERE tsf.column17=0 and tsf.column02>='" + Date1 + @"' and tsf.column02<='" + Date2 + @"'
                                                                    GROUP BY
                                                                           k.column02,
                                                                           tmg.column02,
                                                                           tsg.column03,
                                                                           tcai.column01,
                                                                           tcai.column02", ConSale);
                            Goods = new DataTable();
                            Adapter.Fill(Goods);
                            gridEX_Goods.DataSource = Goods;
                            break;
                    }
                }
            }
        }

        private void Form03_SaleReport_Goods_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.SaleReport_Goods = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();

            gridEX_Goods.RemoveFilters();
        }

        private void Form03_SaleReport_Goods_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();

        }

        private void Form03_SaleReport_Goods_Activated(object sender, EventArgs e)
        {
            cmb_Cancel.ComboBox.Select();
        }

        private void mnu_PrintThis_Click(object sender, EventArgs e)
        {
            //DataTable Table = dataSet_Report.Rpt_Sale_Goods.Clone();

            //foreach (Janus.Windows.GridEX.GridEXRow CustomerRow in gridEX_Customers.GetRows())
            //{
            //    gridEX_Customers.MoveTo(CustomerRow);
            //    foreach (Janus.Windows.GridEX.GridEXRow Factor in gridEX_Factors.GetRows())
            //    {
            //        DataRow New = Table.NewRow();

            //        //Good Info
            //        New["GoodCode"] = gridEX_Goods.GetRow().Cells["GoodCode"].Text.Trim();
            //        New["GoodName"] = gridEX_Goods.GetRow().Cells["GoodName"].Text.Trim();
            //        New["TotalNumber"] = gridEX_Goods.GetRow().Cells["TotalNumber"].Value.ToString();
            //        New["Fee"] = gridEX_Goods.GetRow().Cells["Fee"].Value.ToString();
            //        New["TotalPrice"] = gridEX_Goods.GetRow().Cells["TotalPrice"].Value.ToString();

            //        //Customer Info
            //        New["CustomerName"] = CustomerRow.Cells["CustomerName"].Text;

            //        //Factor Info
            //        New["FactorNumber"] = Factor.Cells["FactorNumber"].Value.ToString();
            //        New["FactorDate"] = Factor.Cells["Date"].Value.ToString();
            //        New["Factor_TotalNumber"] = Factor.Cells["TotalNumber"].Value.ToString();
            //        New["Factor_TotalPrice"] = Factor.Cells["TotalPrice"].Value.ToString();
            //        Table.Rows.Add(New);
            //    }

            //}
            //if (Table.Rows.Count > 0)
            //{
            //    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 11, "از تاریخ: "+Date1, "تا تاریخ: "+Date2);
            //    frm.ShowDialog();
            //}
        }

        private void mnu_PrintAll_Click(object sender, EventArgs e)
        {
            //DataTable Table = dataSet_Report.Rpt_Sale_Goods.Clone();
            //foreach (Janus.Windows.GridEX.GridEXRow GoodRow in gridEX_Goods.GetRows())
            //{
            //    gridEX_Goods.MoveTo(GoodRow);
            //    foreach (Janus.Windows.GridEX.GridEXRow CustomerRow in gridEX_Customers.GetRows())
            //    {
            //        gridEX_Customers.MoveTo(CustomerRow);
            //        foreach (Janus.Windows.GridEX.GridEXRow Factor in gridEX_Factors.GetRows())
            //        {
            //            DataRow New = Table.NewRow();

            //            //Good Info
            //            New["GoodCode"] = GoodRow.Cells["GoodCode"].Text.Trim();
            //            New["GoodName"] = GoodRow.Cells["GoodName"].Text.Trim();
            //            New["TotalNumber"] = GoodRow.Cells["TotalNumber"].Value.ToString();
            //            New["Fee"] = GoodRow.Cells["Fee"].Value.ToString();
            //            New["TotalPrice"] = GoodRow.Cells["TotalPrice"].Value.ToString();

            //            //Customer Info
            //            New["CustomerName"] = CustomerRow.Cells["CustomerName"].Text;

            //            //Factor Info
            //            New["FactorNumber"] = Factor.Cells["FactorNumber"].Value.ToString();
            //            New["FactorDate"] = Factor.Cells["Date"].Value.ToString();
            //            New["Factor_TotalNumber"] = Factor.Cells["TotalNumber"].Value.ToString();
            //            New["Factor_TotalPrice"] = Factor.Cells["TotalPrice"].Value.ToString();
            //            Table.Rows.Add(New);
            //        }

            //    }
            //}
            //if (Table.Rows.Count > 0)
            //{
            //    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 11, "از تاریخ: " + Date1, "تا تاریخ: " + Date2);
            //    frm.ShowDialog();
            //}
        }

        private void mnu_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void cmb_Cancel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                faDatePickerStrip1.FADatePicker.Select();
                cmb_Cancel.ComboBox.DroppedDown = false;
            }
            else
                cmb_Cancel.ComboBox.DroppedDown = true;
        }










    }
}
