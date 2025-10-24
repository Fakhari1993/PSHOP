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
    public partial class Form13_LegalFactors_ExtraReduction : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        string Date1, Date2;

        public Form13_LegalFactors_ExtraReduction()
        {
            InitializeComponent();
        }

        private void Form10_ExtraReduction_Load(object sender, EventArgs e)
        {
            gridEX1.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount");
            DataTable CustomerTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX_SaleFactor.DropDowns["CustomerName"].SetDataBinding(CustomerTable, "");

            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
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

                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_055_LegalFactors.columnid , dbo.Table_055_LegalFactors.column01 AS LegalNumber, dbo.Table_055_LegalFactors.column02 AS Date, 
                      dbo.Table_055_LegalFactors.column03 AS Customer, dbo.Table_055_LegalFactors.Column28 AS NetPrice, dbo.Table_055_LegalFactors.column17 AS Cancel, 
                      dbo.Table_055_LegalFactors.column19 AS Returned, dbo.Table_065_Child2_LegalFactors.column02 AS ExID, dbo.Table_065_Child2_LegalFactors.column03 AS [Percent], 
                      dbo.Table_065_Child2_LegalFactors.column04 AS Price,Table_055_LegalFactors.Column12
                        FROM         dbo.Table_055_LegalFactors INNER JOIN
                                              dbo.Table_065_Child2_LegalFactors ON dbo.Table_055_LegalFactors.columnid = dbo.Table_065_Child2_LegalFactors.column01
                        WHERE     (dbo.Table_055_LegalFactors.column02 >= '{0}') AND (dbo.Table_055_LegalFactors.column02 <= '{1}')", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, Date1, Date2);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                bindingSource1.DataSource = Table;
                gridEX_ExtraRed_CurrentCellChanged(sender, e);
            }

        }

        private void Form10_ExtraReduction_Activated(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.Select();
        }

        private void Form10_ExtraReduction_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_SaleFactor.RemoveFilters();
        }

        private void Form10_ExtraReduction_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print.ShowDropDown();
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Display_Click(sender, e);
        }

        private void gridEX1_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
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
                        _05_Sale.Frm_002_Faktor frms = new _05_Sale.Frm_002_Faktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 21), Convert.ToInt32(gridEX_SaleFactor.GetValue("ColumnId").ToString()));
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

        private void gridEX_ExtraRed_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RowCount > 0)
                {
                    bindingSource1.Filter = "ExID=" + gridEX1.GetValue("ColumnId").ToString();
                }
            }
            catch { }
        }

       

        private void bt_PrintAll_Sale_Click(object sender, EventArgs e)
        {
            if (this.gridEX1.RowCount>0)
            {
                DataTable Table = dataSet_Report.Rpt_ExtRedReport.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow List in gridEX1.GetRows())
                {
                    gridEX1.MoveTo(List);
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_SaleFactor.GetRows())
                    {
                        Table.Rows.Add(item.Cells["LegalNumber"].Value.ToString(),
                            item.Cells["Date"].Value.ToString(),
                            item.Cells["Customer"].Text.ToString(),
                            item.Cells["NetPrice"].Value.ToString(),
                            item.Cells["Percent"].Value.ToString(),
                            item.Cells["Price"].Value.ToString(),
                            bool.Parse(item.Cells["Returned"].Value.ToString()),
                            bool.Parse(item.Cells["Cancel"].Value.ToString()),
                            List.Cells["Column01"].Value.ToString());
                    }
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 29, Date1,  Date2, "فاکتورهای فروش");
                    frm.ShowDialog();
                }
            }
        }

        private void bt_PrintThis_Sale_Click(object sender, EventArgs e)
        {
            if (this.gridEX1.RowCount > 0)
            {
                DataTable Table = dataSet_Report.Rpt_ExtRedReport.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_SaleFactor.GetRows())
                {
                    Table.Rows.Add(item.Cells["LegalNumber"].Value.ToString(),
                        item.Cells["Date"].Value.ToString(),
                        item.Cells["Customer"].Text.ToString(),
                        item.Cells["NetPrice"].Value.ToString(),
                        item.Cells["Percent"].Value.ToString(),
                        item.Cells["Price"].Value.ToString(),
                        bool.Parse(item.Cells["Returned"].Value.ToString()),
                        bool.Parse(item.Cells["Cancel"].Value.ToString()),
                        gridEX1.GetRow().Cells["Column01"].Text.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 29, Date1, Date2, "فاکتورهای فروش");
                    frm.ShowDialog();
                }
            }
        }

    }
}
