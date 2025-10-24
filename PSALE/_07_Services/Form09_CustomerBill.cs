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
    public partial class Form09_CustomerBill : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        string Date1, Date2;

        bool _BackSpace = false;
        public Form09_CustomerBill()
        {
            InitializeComponent();
        }

        private void Form17_CustomerBill_Load(object sender, EventArgs e)
        {
            this.rpt_CustomerBill_HeaderTableAdapter.Fill(this.dataSet_Sale4.Rpt_CustomerBill_Header);
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            DataTable PersonTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX_Person.DropDowns["CustomerCode"].SetDataBinding(PersonTable, "");
            gridEX_Person.DropDowns["CustomerName"].SetDataBinding(PersonTable, "");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString,
                "SELECT * FROM Table_070_CountUnitInfo"), "");

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

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                rpt_CustomerBill_ChildTableAdapter.Fill(dataSet_Sale4.Rpt_CustomerBill_Child,Date1,
                    Date2);
            }
        }

        private void Form17_CustomerBill_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
                bt_Search_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridEX_Person.GetCheckedRows().Length > 0)
                {
                    DataTable Table = dataSet_Sale4.Rpt_PrintCustomerBill.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow header in gridEX_Person.GetCheckedRows())
                    {
                        gridEX_Person.MoveTo(header);
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                        {
                            Table.Rows.Add(header.Cells["CustomerCode"].Text,
                                header.Cells["CustomerName"].Text,
                                item.Cells["Number"].Value.ToString(),
                                item.Cells["Date"].Value.ToString(),
                                item.Cells["ServID"].Text.ToString(),
                                item.Cells["CountUnit"].Text.ToString(),
                                Convert.ToDouble(item.Cells["Count"].Value.ToString()),
                                Convert.ToDouble(item.Cells["SinglePrice"].Value.ToString()),
                                Convert.ToDouble(item.Cells["TotalPrice"].Value.ToString()),
                               item.Cells["Description"].Text.Trim());

                        }
                    }

                    if (Table.Rows.Count > 0)
                    {
                        _07_Services.Reports.ReportForm frm = new Reports.ReportForm(7, Table, null, null, Date1,
                            Date2);
                        frm.Show();
                    }
                }
            }
            catch { }
        }

       

    }
}
