using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Janus.Windows;

namespace PSHOP._07_Services
{
    public partial class Form06_ServiceReport_Customers : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);

        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        string Date1, Date2;

        public Form06_ServiceReport_Customers()
        {
            InitializeComponent();
        }

        private void Form06_ServiceReport_Customers_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_070_CountUnitInfo"), "");
            gridEX_List.DropDowns["Service"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_030_Services"), "");
            gridEX_Extra.DropDowns["Extra"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount"), "");
            gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");


            DataTable PersonTable = clDoc.ReturnTable(ConBase.ConnectionString, @"Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX_Header.DropDowns["Customer"].SetDataBinding(PersonTable,"");
            gridEX_Customers.DropDowns["Person"].SetDataBinding(PersonTable, "");

            gridEX_Header.Select();
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
                table_CustomersTableAdapter.Fill(dataset_Services.Table_Customers, Date1, Date2);
                table_031_ServiceFactorTableAdapter.FillBy(dataset_Services.Table_031_ServiceFactor, Date1, Date2);
                this.table_032_ServiceFactor_Child1TableAdapter.FillBy(this.dataset_Services.Table_032_ServiceFactor_Child1);
                this.table_033_ServiceFactor_Child2TableAdapter.FillBy(this.dataset_Services.Table_033_ServiceFactor_Child2);
                gridEX_Customers.MoveFirst();
                gridEX_Customers_CurrentCellChanged(sender, e);

                List<double> Points = new List<double>();
                List<string> Names = new List<string>();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Customers.GetRows())
                {
                    Points.Add(double.Parse(item.Cells["Total"].Value.ToString()));
                    Names.Add(item.Cells["Customer"].Text);
                }
                microChart1.DataPoints = Points;
                microChart1.DataPointTooltips = Names;
                gridEX_Customers.Focus();
            }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_070_CountUnitInfo"), "");
            gridEX_List.DropDowns["Service"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_030_Services"), "");
            gridEX_Extra.DropDowns["Extra"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount"), "");
            gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            DataTable PersonTable = clDoc.ReturnTable(ConBase.ConnectionString, @"Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX_Header.DropDowns["Customer"].SetDataBinding(PersonTable, "");
            gridEX_Customers.DropDowns["Person"].SetDataBinding(PersonTable, "");
            bt_Search_Click(sender, e);
        }

        private void bt_ExportToExcel_Factors_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Header;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Customers;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void Form06_ServiceReport_Customers_Activated(object sender, EventArgs e)
        {
            try
            {
                gridEX_Customers.Row = gridEX_Customers.FilterRow.Position;
            }
            catch 
            {
            }
        }

        private void Form06_ServiceReport_Customers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.KeyCode == Keys.F5)
                bt_Refresh_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Search_Click(sender, e);
        }

        private void Form06_ServiceReport_Customers_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Customers.RemoveFilters();
            gridEX_Header.RemoveFilters();
            gridEX_List.RemoveFilters();
        }

        private void gridEX_Customers_Enter(object sender, EventArgs e)
        {
            try
            {
                microChart1.DataPoints.Clear();
                microChart1.DataPointTooltips.Clear();
                List<double> Points = new List<double>();
                List<string> Names = new List<string>();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Customers.GetRows())
                {
                    Points.Add(double.Parse(item.Cells["Total"].Value.ToString()));
                    Names.Add(item.Cells["Customer"].Text);
                }
                microChart1.DataPoints = Points;
                microChart1.DataPointTooltips = Names;
              
            }
            catch { }
        }

        private void gridEX_Customers_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                table_031_ServiceFactorBindingSource.Filter = "Column03=" + gridEX_Customers.GetValue("Customer");
            }
            catch
            {
            }
        }

        private void gridEX_Header_Enter(object sender, EventArgs e)
        {
            try
            {
                microChart1.DataPoints.Clear();
                microChart1.DataPointTooltips.Clear();
                List<double> Points = new List<double>();
                List<string> Names = new List<string>();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetRows())
                {
                    Points.Add(double.Parse(item.Cells["Column09"].Value.ToString()));
                    Names.Add("فاکتور شماره "+item.Cells["Column01"].Text);
                }
                microChart1.DataPoints = Points;
                microChart1.DataPointTooltips = Names;
            }
            catch { }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (this.table_CustomersBindingSource.Count > 0)
            {
                try
                {
                    DataTable Table = dataset_Services.Rpt_Customers.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow CusotmerRow in gridEX_Customers.GetRows())
                    {
                        gridEX_Customers.MoveTo(CusotmerRow);
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetRows())
                        {
                            Table.Rows.Add(CusotmerRow.Cells["Customer"].Value.ToString(),
                                CusotmerRow.Cells["Customer"].Text,
                                item.Cells["Column01"].Value.ToString(),
                                item.Cells["Column02"].Value.ToString(),
                                item.Cells["Column05"].Text,
                                item.Cells["Column06"].Value.ToString(),
                                item.Cells["Column08"].Value.ToString(),
                                item.Cells["Column07"].Value.ToString(),
                                item.Cells["Column09"].Value.ToString(),
                                item.Cells["Column04"].Text.Trim());

                        }
                        
                    }
                    _07_Services.Reports.ReportForm frm = new Reports.ReportForm(4, Table, null, null, Date1, Date2);
                    frm.ShowDialog();
                }
                catch { }
            }
        }
       
    }
}
