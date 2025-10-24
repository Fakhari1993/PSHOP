using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._06_Reports._03_Buy
{
    public partial class Form07_ReturnBuyReport_Good : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        string Date1, Date2;

        public Form07_ReturnBuyReport_Good()
        {
            InitializeComponent();
        }

        private void Form03_SaleReport_Goods_Load(object sender, EventArgs e)
        {
            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-2);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            string[] Dates = Properties.Settings.Default.ReturnBuyReport_Good.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);

            SqlDataAdapter Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients", ConWare);
            DataTable Goods = new DataTable();
            Adapter.Fill(Goods);
            gridEX_Goods.DropDowns["GoodCode"].SetDataBinding(Goods, "");
            gridEX_Goods.DropDowns["GoodName"].SetDataBinding(Goods, "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
            DataTable Person = new DataTable();
            Adapter.Fill(Person);
            gridEX_Customers.DropDowns["CustomerName"].SetDataBinding(Person, "");
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
                    this.rpt_ReturnBuy_Good_HeaderTableAdapter.Fill(dataSet_Buy.Rpt_ReturnBuy_Good_Header, Date1, Date2,chk_Currency.Checked);
                    this.rpt_ReturnBuy_Good_SuppliersTableAdapter.Fill(dataSet_Buy.Rpt_ReturnBuy_Good_Suppliers, Date1, Date2, chk_Currency.Checked);
                    this.rpt_ReturnBuy_Good_FactorsTableAdapter.Fill(dataSet_Buy.Rpt_ReturnBuy_Good_Factors, Date1, Date2, chk_Currency.Checked);
            }
        }

        private void Form03_SaleReport_Goods_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.ReturnBuyReport_Good = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_Factors.RemoveFilters();
            gridEX_Customers.RemoveFilters();
            gridEX_Goods.RemoveFilters();
        }

        private void Form03_SaleReport_Goods_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print.ShowDropDown();
        }

        private void Form03_SaleReport_Goods_Activated(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.Select();
        }

        private void mnu_PrintThis_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_Sale_Goods.Clone();

            foreach (Janus.Windows.GridEX.GridEXRow CustomerRow in gridEX_Customers.GetRows())
            {
                gridEX_Customers.MoveTo(CustomerRow);
                foreach (Janus.Windows.GridEX.GridEXRow Factor in gridEX_Factors.GetRows())
                {
                    DataRow New = Table.NewRow();

                    //Good Info
                    New["GoodCode"] = gridEX_Goods.GetRow().Cells["GoodCode"].Text.Trim();
                    New["GoodName"] = gridEX_Goods.GetRow().Cells["GoodName"].Text.Trim();
                    New["TotalNumber"] = gridEX_Goods.GetRow().Cells["TotalNumber"].Value.ToString();
                    New["Fee"] = gridEX_Goods.GetRow().Cells["Fee"].Value.ToString();
                    New["TotalPrice"] = gridEX_Goods.GetRow().Cells["TotalPrice"].Value.ToString();

                    //Customer Info
                    New["CustomerName"] = CustomerRow.Cells["CustomerName"].Text;
                   
                    //Factor Info
                    New["FactorNumber"] = Factor.Cells["FactorNumber"].Value.ToString();
                    New["FactorDate"] = Factor.Cells["Date"].Value.ToString();
                    New["Factor_TotalNumber"] = Factor.Cells["TotalNumber"].Value.ToString();
                    New["Factor_TotalPrice"] = Factor.Cells["TotalPrice"].Value.ToString();
                    Table.Rows.Add(New);
                }

            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 27, "از تاریخ: "+Date1, "تا تاریخ: "+Date2);
                frm.ShowDialog();
            }
        }

        private void mnu_PrintAll_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_Sale_Goods.Clone();
            foreach (Janus.Windows.GridEX.GridEXRow GoodRow in gridEX_Goods.GetRows())
            {
                gridEX_Goods.MoveTo(GoodRow);
                foreach (Janus.Windows.GridEX.GridEXRow CustomerRow in gridEX_Customers.GetRows())
                {
                    gridEX_Customers.MoveTo(CustomerRow);
                    foreach (Janus.Windows.GridEX.GridEXRow Factor in gridEX_Factors.GetRows())
                    {
                        DataRow New = Table.NewRow();

                        //Good Info
                        New["GoodCode"] = GoodRow.Cells["GoodCode"].Text.Trim();
                        New["GoodName"] = GoodRow.Cells["GoodName"].Text.Trim();
                        New["TotalNumber"] = GoodRow.Cells["TotalNumber"].Value.ToString();
                        New["Fee"] = GoodRow.Cells["Fee"].Value.ToString();
                        New["TotalPrice"] = GoodRow.Cells["TotalPrice"].Value.ToString();

                        //Customer Info
                        New["CustomerName"] = CustomerRow.Cells["CustomerName"].Text;

                        //Factor Info
                        New["FactorNumber"] = Factor.Cells["FactorNumber"].Value.ToString();
                        New["FactorDate"] = Factor.Cells["Date"].Value.ToString();
                        New["Factor_TotalNumber"] = Factor.Cells["TotalNumber"].Value.ToString();
                        New["Factor_TotalPrice"] = Factor.Cells["TotalPrice"].Value.ToString();
                        Table.Rows.Add(New);
                    }

                }
            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table,27, "از تاریخ: " + Date1, "تا تاریخ: " + Date2);
                frm.ShowDialog();
            }
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

    

    
    }
}
