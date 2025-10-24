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
    public partial class Form20_Visitors_Customers : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1, Date2;
        public Form20_Visitors_Customers()
        {
            InitializeComponent();
        }

        private void Form20_Visitors_Customers_Load(object sender, EventArgs e)
        {
            this.tbl_VitorsTableAdapter.Fill(this.dataSet_Report.Tbl_Vitors);
            string[] Dates = Properties.Settings.Default.Visitors_Customers.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue &&
             faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	PR_050_PeopleInAccounts @Date1 = '" + Date1
                    + "',	@Date2 = '" + Date2 + "', @DbName ='" + ConBase.Database + "'", ConAcnt);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                bindingSource1 = new BindingSource();
                bindingSource2 = new BindingSource();
                bindingSource1.DataSource = Table;
                Int64 Remain = 0;
                foreach (DataRowView item in bindingSource1)
                {
                    Remain = 0;
                    Remain = Int64.Parse(item["Bed"].ToString()) - Int64.Parse(item["Bes"].ToString());
                    item["Remain"] = Remain;
                    bindingSource1.EndEdit();
                }

                gridEXList.DataSource = bindingSource1;
                DataTable FactorsTbl = clDoc.ReturnTable(ConSale.ConnectionString, @"Select  DISTINCT Column03,Column05 from Table_010_SaleFactor
                where not(column05 is null) and LEN(Column05)>0");
                bindingSource2.DataSource = FactorsTbl;

                tbl_VitorsBindingSource_PositionChanged(sender, e);
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
                    faDatePickerStrip2.FADatePicker.Select();

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

        private void bt_SendToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEXList;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void Form20_Visitors_Customers_Activated(object sender, EventArgs e)
        {
           
        }

        private void Form20_Visitors_Customers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.Visitors_Customers = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEXList.RemoveFilters();
        }

        private void Form20_Visitors_Customers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
        }

        private void tbl_VitorsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                bindingSource2.Filter = "Column05=" + gridEX_visitors.GetValue("ColumnId").ToString();
                List<string> CusIds = new List<string>();
                foreach (DataRowView item in bindingSource2)
                {
                    CusIds.Add(item["Column03"].ToString());
                }
                bindingSource1.Filter = "ID IN ("+string.Join(",",CusIds.ToArray())+")";

              
               
            }
            catch 
            {
            }
        }

        private void gridEXList_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 37))
            {
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
                
                PACNT._3_BooksMenu.Form07_AccountsListOperation frm = new  PACNT._3_BooksMenu.Form07_AccountsListOperation(
                    gridEXList.GetRow().Cells["ACC_Code"].Value.ToString(), Convert.ToDateTime(FarsiLibrary.Utils.PersianDate.Parse(Date1)),
                     Convert.ToDateTime(FarsiLibrary.Utils.PersianDate.Parse(Date2)));
                try { frm.MdiParent = MainForm.ActiveForm; }
                catch { }
                frm.Show();

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_Visitor_Customer.Clone();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEXList.GetRows())
            {
                Table.Rows.Add(gridEX_visitors.GetValue("Column02").ToString(),
                    item.Cells["Code"].Value.ToString(),
                    item.Cells["Name"].Value.ToString(),
                    item.Cells["ACC_Code"].Value.ToString(),
                    item.Cells["ACC_Name"].Value.ToString(),
                    item.Cells["Bed"].Value.ToString(),
                    item.Cells["Bes"].Value.ToString(),
                    item.Cells["Remain"].Value.ToString(),
                    gridEX_visitors.GetValue("Column01").ToString());
            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 39,Date1,
                    Date2);
                frm.ShowDialog();
            }
        }
    }
}
