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
    public partial class Form11_DiscountReport : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        string Date1, Date2;

        public Form11_DiscountReport()
        {
            InitializeComponent();
        }

        private void Form10_ExtraReduction_Load(object sender, EventArgs e)
        {
            DataTable CustomerTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX_SaleFactor.DropDowns["CustomerName"].SetDataBinding(CustomerTable, "");

            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            string[] Dates = Properties.Settings.Default.DiscountReport.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);

            bt_Display_Click(sender, e);


        }

        private void bt_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_SaleFactor;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
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
                    bt_Display_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     column01 AS SaleNumber, column02 AS Date, column03 AS Customer, Column28 AS NetPrice, Column30 AS CustomerGroup, Column29 AS Volume, 
                Column31 AS SpecialCustomer
                FROM         dbo.Table_010_SaleFactor
                WHERE     (column02 BETWEEN '{0}' AND '{1}') AND (Column30 > 0) OR
                (column02 BETWEEN '{0}' AND '{1}') AND (Column29 > 0) OR
                (column02 BETWEEN '{0}' AND '{1}') AND (Column31 > 0)", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, Date1, Date2);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                bindingSource1.DataSource = Table;
            }

        }

        private void Form10_ExtraReduction_Activated(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.Select();
        }

        private void Form10_ExtraReduction_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.DiscountReport = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_SaleFactor.RemoveFilters();
        }

        private void Form10_ExtraReduction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Display_Click(sender, e);
        }


        private void bt_ExportBottomTable_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (gridEX_SaleFactor.RowCount > 0)
            {
                DataTable Table = dataSet_Report.Rpt_Discounts.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_SaleFactor.GetRows())
                {
                    Table.Rows.Add(item.Cells["SaleNumber"].Value.ToString(),
                        item.Cells["Date"].Value.ToString(),
                        item.Cells["Customer"].Text,
                        item.Cells["NetPrice"].Value.ToString(),
                        item.Cells["CustomerGroup"].Value.ToString(),
                        item.Cells["Volume"].Value.ToString(),
                        item.Cells["SpecialCustomer"].Value.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 31, "از تاریخ:" + Date1, "تا تاریخ: " + Date2);
                    frm.Show();
                }
            }
        }

        private void gridEX_SaleFactor_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
            {
                if (this.gridEX_SaleFactor.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 20))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_002_Faktor")
                            {
                                item.BringToFront();
                                _05_Sale.Frm_002_Faktor frm = (_05_Sale.Frm_002_Faktor)item;
                                frm.txt_Search.Text = gridEX_SaleFactor.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _05_Sale.Frm_002_Faktor frms = new _05_Sale.Frm_002_Faktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 21), Convert.ToInt32(gridEX_SaleFactor.GetValue("columnid")));
                        try
                        {
                            frms.MdiParent = MainForm.ActiveForm;
                        }
                        catch { }
                        frms.Show();
                    }
                    else
                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
                }
            }
            catch { }
        }

   

      
    }
}
