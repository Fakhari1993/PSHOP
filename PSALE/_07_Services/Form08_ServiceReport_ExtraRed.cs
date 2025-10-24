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
    public partial class Form08_ServiceReport_ExtraRed : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        string Date1, Date2;

        public Form08_ServiceReport_ExtraRed()
        {
            InitializeComponent();
        }

        private void Form08_ServiceReport_ExtraRed_Load(object sender, EventArgs e)
        {
            gridEX_Extra.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount");
            DataTable CustomerTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX_Factors.DropDowns["CustomerName"].SetDataBinding(CustomerTable, "");

            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            bt_Display_Click(sender, e);

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (this.gridEX_Extra.RowCount > 0)
            {
                DataTable Table = dataSet_Report.Rpt_ExtRedReport.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow List in gridEX_Extra.GetRows())
                {
                    gridEX_Extra.MoveTo(List);
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Factors.GetRows())
                    {
                        Table.Rows.Add(item.Cells["SaleNumber"].Value.ToString(),
                            item.Cells["Date"].Value.ToString(),
                            item.Cells["Customer"].Text.ToString(),
                            item.Cells["NetPrice"].Value.ToString(),
                            item.Cells["Percent"].Value.ToString(),
                            item.Cells["Price"].Value.ToString(),
                            false,
                            false,
                            List.Cells["Column01"].Value.ToString());
                    }
                }
                if (Table.Rows.Count > 0)
                {
                    _07_Services.Reports.ReportForm frm = new Reports.ReportForm(6, Table, null, null, "از تاریخ:" + Date1, "تا تاریخ: " + Date2);
                    frm.ShowDialog();
                }
            }
        }

        private void bt_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Factors;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_031_ServiceFactor.ColumnId , dbo.Table_031_ServiceFactor.column01 AS SaleNumber, dbo.Table_031_ServiceFactor.column02 AS Date, 
                      dbo.Table_031_ServiceFactor.column03 AS Customer, dbo.Table_031_ServiceFactor.Column09 AS NetPrice,  dbo.Table_033_ServiceFactor_Child2.column02 AS ExID, dbo.Table_033_ServiceFactor_Child2.column03 AS [Percent], 
                      dbo.Table_033_ServiceFactor_Child2.column04 AS Price
                        FROM         dbo.Table_031_ServiceFactor INNER JOIN
                                              dbo.Table_033_ServiceFactor_Child2 ON dbo.Table_031_ServiceFactor.columnid = dbo.Table_033_ServiceFactor_Child2.column01
                        WHERE     (dbo.Table_031_ServiceFactor.column02 >= '{0}') AND (dbo.Table_031_ServiceFactor.column02 <= '{1}')", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, Date1, Date2);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                bindingSource1.DataSource = Table;
                gridEX_Extra_CurrentCellChanged(sender, e);
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

        private void Form08_ServiceReport_ExtraRed_Activated(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.Select();
        }

        private void Form08_ServiceReport_ExtraRed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Display_Click(sender, e);
        }

        private void Form08_ServiceReport_ExtraRed_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Factors.RemoveFilters();
            gridEX_Extra.RemoveFilters();
        }

        private void gridEX_Factors_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
            {
                if (this.gridEX_Factors.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 105))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Form02_RegisterServiceFactor")
                            {
                                item.BringToFront();
                                _07_Services.Form02_RegisterServiceFactor frm = (_07_Services.Form02_RegisterServiceFactor)item;
                                frm.txt_Search.Text = gridEX_Factors.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _07_Services.Form02_RegisterServiceFactor frms = new _07_Services.Form02_RegisterServiceFactor(Convert.ToInt32(gridEX_Factors.GetValue("ColumnId").ToString()), UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 106));
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

        private void gridEX_Extra_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX_Extra.RowCount > 0)
                {
                    bindingSource1.Filter = "ExID=" + gridEX_Extra.GetValue("ColumnId").ToString();
                }
            }
            catch { }
        }
    }
}
