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
    public partial class Form31_ReturnFactors_Visitors : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Class_UserScope UserScope = new Class_UserScope();

        bool _BackSpace = false;
        string Date1, Date2;

        public Form31_ReturnFactors_Visitors()
        {
            InitializeComponent();
            gridEXFieldChooserControl1.GridEX = gridEX_Factors;
        }

        private void Form19_Visitors_Load(object sender, EventArgs e)
        {
            this.tbl_VitorsTableAdapter.Fill(this.dataSet_Report.Tbl_Vitors);
            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            string[] Dates = Properties.Settings.Default.ReturnFactors_Visitors.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);

            gridEX_Factors.DropDowns["Customer"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_045_PersonInfo");
            gridEX_Factors.DropDowns["Receipt"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"), "");
            gridEX_Factors.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            gridEX_Factors.DropDowns["Sale"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_010_SaleFactor"), "");
            gridEX_Factors.DropDowns["Order"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_005_OrderHeader"), "");
            gridEX_Factors.DropDowns["Prefactor"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_007_FactorBefore"), "");
            gridEX_Factors.DropDowns["Currency"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_055_CurrencyInfo");
            
            gridEX_visitors.Select();
            gridEX_visitors.Row = gridEX_visitors.FilterRow.Position;

        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                string CommandText = null;
                CommandText = @"SELECT        columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, 
                column15, column16, column17, Column18, Column19, Column20, Column21, Column22, Column23, Column24, 
                Column18 + Column19 - Column20 AS FinalPrice
                FROM            Table_018_MarjooiSale
                WHERE        (column02 >= '{0}') AND (column02 <= '{1}')";

                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                bindingSource1.DataSource = Table;
                gridEX_Factors.DataSource = bindingSource1;
                gridEX_visitors.MoveFirst();
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

        private void Form19_Visitors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Factors;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void gridEX_Goods_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
            {
                if (this.gridEX_Factors.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 22))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_013_ReturnFactor")
                            {
                                item.BringToFront();
                                _05_Sale.Frm_013_ReturnFactor frm = (_05_Sale.Frm_013_ReturnFactor)item;
                                frm.txt_Search.Text = gridEX_Factors.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _05_Sale.Frm_013_ReturnFactor frms = new _05_Sale.Frm_013_ReturnFactor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 23),
                            Convert.ToInt32(gridEX_Factors.GetValue("ColumnId").ToString()));
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

        private void Form19_Visitors_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.ReturnFactors_Visitors = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_visitors.RemoveFilters();
            gridEX_Factors.RootTable.Groups.Clear();
            gridEX_Factors.RemoveFilters();
        }

        private void tbl_VitorsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.Filter = "Column05=" + gridEX_visitors.GetValue("ColumnId").ToString();
            }
            catch 
            {
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_ReturnFactors_Visitors.Clone();
            if (gridEX_visitors.GetCheckedRows().Length > 0)
            {
                foreach (Janus.Windows.GridEX.GridEXRow Head in gridEX_visitors.GetCheckedRows())
                {
                    gridEX_visitors.MoveTo(Head);
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Factors.GetRows())
                    {
                        Table.Rows.Add(item.Cells["ColumnId"].Value.ToString(),
                            item.Cells["Column01"].Value.ToString(),
                            item.Cells["Column03"].Text.Trim(),
                            item.Cells["Column18"].Value.ToString(),
                            item.Cells["Column19"].Value.ToString(),
                            item.Cells["Column20"].Value.ToString(),
                            item.Cells["FinalPrice"].Value.ToString(),
                            item.Cells["Column02"].Value.ToString(),
                            Head.Cells["ColumnId"].Value.ToString(),
                            Head.Cells["Column02"].Value.ToString());
                    }
                }
            }
            else Class_BasicOperation.ShowMsg("", "ابتدا مسئولین فروش مورد نظر خود را مشخص کنید", "None");
           

            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 44, Date1, Date2);
                frm.ShowDialog();
            }
        }
    }
}
