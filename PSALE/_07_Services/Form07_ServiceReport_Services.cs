using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._07_Services
{
    public partial class Form07_ServiceReport_Services : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        string Date1, Date2;

        public Form07_ServiceReport_Services()
        {
            InitializeComponent();
        }

        private void Form07_ServiceReport_Services_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            gridEX_Customers.DropDowns["Customer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_045_PersonInfo"), "");
            gridEX_Services.DropDowns["Service"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01,Column02,Column03,Column04 from Table_030_Services"), "");

        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                this.table_Services_HeaderTableAdapter.Fill(dataset_Services.Table_Services_Header,Date1,Date2);
                this.table_Services_CustomerTableAdapter.Fill(this.dataset_Services.Table_Services_Customer);
                this.table_Service_FactorsTableAdapter.Fill(this.dataset_Services.Table_Service_Factors);
            }
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

        private void bt_Print_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_Sale_Goods.Clone();
            foreach (Janus.Windows.GridEX.GridEXRow ServiceRow in gridEX_Services.GetRows())
            {
                gridEX_Services.MoveTo(ServiceRow);
                foreach (Janus.Windows.GridEX.GridEXRow CustomerRow in gridEX_Customers.GetRows())
                {
                    gridEX_Customers.MoveTo(CustomerRow);
                    foreach (Janus.Windows.GridEX.GridEXRow Factor in gridEX_Factors.GetRows())
                    {
                        DataRow New = Table.NewRow();

                        //Good Info
                        New["GoodCode"] = ServiceRow.Cells["ServiceName"].Value.ToString();
                        New["GoodName"] = ServiceRow.Cells["ServiceName"].Text.Trim();
                        New["TotalNumber"] = ServiceRow.Cells["TotalNumber"].Value.ToString();
                        New["Fee"] = ServiceRow.Cells["Fee"].Value.ToString();
                        New["TotalPrice"] = ServiceRow.Cells["TotalPrice"].Value.ToString();

                        //Customer Info
                        New["CustomerName"] = CustomerRow.Cells["Customer"].Text;

                        //Factor Info
                        New["FactorNumber"] = Factor.Cells["FactorNumber"].Value.ToString();
                        New["FactorDate"] = Factor.Cells["Date"].Value.ToString();
                        New["Factor_TotalNumber"] = Factor.Cells["Number"].Value.ToString();
                        New["Factor_TotalPrice"] = Factor.Cells["Price"].Value.ToString();
                        Table.Rows.Add(New);
                    }

                }
            }
            if (Table.Rows.Count > 0)
            {
               _07_Services.Reports.ReportForm frm=new Reports.ReportForm(5,Table,null,null,"از تاریخ: " + Date1, "تا تاریخ: " + Date2);
                frm.ShowDialog();
            }
        }

        private void Form07_ServiceReport_Services_Activated(object sender, EventArgs e)
        {
            try
            {
                gridEX_Services.Row = gridEX_Services.FilterRow.Position;
            }
            catch 
            {
            }
        }

        private void Form07_ServiceReport_Services_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Search_Click(sender, e);
        }

        private void Form07_ServiceReport_Services_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Services.RemoveFilters();
            gridEX_Factors.RemoveFilters();
            gridEX_Customers.RemoveFilters();
        }

      
    }
}
