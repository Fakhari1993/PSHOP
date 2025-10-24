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
    public partial class Form08_ExtraReduction : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        string Date1, Date2;

        public Form08_ExtraReduction()
        {
            InitializeComponent();
        }

        private void Form10_ExtraReduction_Load(object sender, EventArgs e)
        {
            gridEX_List.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount_Buy");
            DataTable CustomerTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX_BuyFactor.DropDowns["CustomerName"].SetDataBinding(CustomerTable, "");
            gridEX_Returned.DropDowns["CustomerName"].SetDataBinding(CustomerTable, "");

            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            string[] Dates = Properties.Settings.Default.ExtraReduction1.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
            bt_Display_Click(sender, e);


        }

        private void bt_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_BuyFactor;
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

                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_015_BuyFactor.columnid, dbo.Table_015_BuyFactor.column01 AS SaleNumber, dbo.Table_015_BuyFactor.column02 AS Date, 
                        dbo.Table_015_BuyFactor.column03 AS Customer, dbo.Table_015_BuyFactor.Column20 AS NetPrice, dbo.Table_015_BuyFactor.column19 AS Cancel, 
                        dbo.Table_015_BuyFactor.column17 AS Returned, dbo.Table_017_Child2_BuyFactor.column02 AS ExID, dbo.Table_017_Child2_BuyFactor.column03 AS [Percent], 
                        dbo.Table_017_Child2_BuyFactor.column04 AS Price,Table_015_BuyFactor.Column15
                        FROM         dbo.Table_015_BuyFactor INNER JOIN
                        dbo.Table_017_Child2_BuyFactor ON dbo.Table_015_BuyFactor.columnid = dbo.Table_017_Child2_BuyFactor.column01
                        WHERE     (dbo.Table_015_BuyFactor.column02 >= '{0}') AND (dbo.Table_015_BuyFactor.column02 <= '{1}')", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, Date1, Date2);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                bindingSource1.DataSource = Table;


                //Returned
                Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_021_MarjooiBuy.columnid , dbo.Table_021_MarjooiBuy.column01 AS RNumber, dbo.Table_021_MarjooiBuy.column02 AS RDate, 
                dbo.Table_021_MarjooiBuy.column03 AS RCustomerId, dbo.Table_021_MarjooiBuy.Column18 AS RNetPrice, dbo.Table_023_Child2_MarjooiBuy.column02 AS RExID,
                dbo.Table_023_Child2_MarjooiBuy.column03 AS RPercent, dbo.Table_023_Child2_MarjooiBuy.column04 AS RPrice,Table_021_MarjooiBuy.Column15
                FROM         dbo.Table_021_MarjooiBuy INNER JOIN
                dbo.Table_023_Child2_MarjooiBuy ON dbo.Table_021_MarjooiBuy.columnid = dbo.Table_023_Child2_MarjooiBuy.column01
                WHERE     (dbo.Table_021_MarjooiBuy.column02 >= '{0}') AND (dbo.Table_021_MarjooiBuy.column02 <='{1}')", ConSale);
                DataTable Table2 = new DataTable();
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, Date1, Date2);
                Adapter.Fill(Table2);
                bindingSource2.DataSource = Table2;

                gridEX_ExtraRed_CurrentCellChanged(sender, e);
            }

        }

        private void Form10_ExtraReduction_Activated(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.Select();
        }

        private void Form10_ExtraReduction_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.ExtraReduction1 = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_BuyFactor.RemoveFilters();
            gridEX_Returned.RemoveFilters();
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
                if (this.gridEX_BuyFactor.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 20))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_003_FaktorKharid")
                            {
                                item.BringToFront();
                                _04_Buy.Frm_003_FaktorKharid frm = (_04_Buy.Frm_003_FaktorKharid)item;
                                frm.txt_Search.Text = gridEX_List.GetRow().Cells["SaleNumber"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _04_Buy.Frm_003_FaktorKharid frms = new _04_Buy.Frm_003_FaktorKharid(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 29),
                            Convert.ToInt32(gridEX_BuyFactor.GetValue("columnid")));
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
                if (gridEX_List.RowCount > 0)
                {
                    bindingSource1.Filter = "ExID=" + gridEX_List.GetValue("ColumnId").ToString();
                    bindingSource2.Filter = "RExID=" + gridEX_List.GetValue("ColumnId").ToString();
                }
            }
            catch { }
        }

        private void bt_ExportBottomTable_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Returned;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void gridEX_Returned_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
            {
                if (this.gridEX_Returned.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 30))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_013_ReturnFactor")
                            {
                                item.BringToFront();
                                _04_Buy.Frm_014_ReturnFactor frm = (_04_Buy.Frm_014_ReturnFactor)item;
                                frm.txt_Search.Text = gridEX_Returned.GetRow().Cells["SaleNumber"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _04_Buy.Frm_014_ReturnFactor frms = new _04_Buy.Frm_014_ReturnFactor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 31), Convert.ToInt32(gridEX_Returned.GetValue("columnid")));
                        frms.MdiParent = MainForm.ActiveForm;
                        frms.Show();
                    }
                    else
                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
                }
            }
            catch { }

        }

        private void bt_PrintAll_Sale_Click(object sender, EventArgs e)
        {
            if (this.gridEX_List.RowCount>0)
            {
                DataTable Table = dataSet_Report.Rpt_ExtRedReport.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow List in gridEX_List.GetRows())
                {
                    gridEX_List.MoveTo(List);
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_BuyFactor.GetRows())
                    {
                        Table.Rows.Add(item.Cells["SaleNumber"].Value.ToString(),
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
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 32, "از تاریخ:" + Date1, "تا تاریخ: " + Date2, "فاکتورهای خرید");
                    frm.ShowDialog();
                }
            }
        }

        private void bt_PrintThis_Sale_Click(object sender, EventArgs e)
        {
            if (this.gridEX_List.RowCount > 0)
            {
                DataTable Table = dataSet_Report.Rpt_ExtRedReport.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_BuyFactor.GetRows())
                {
                    Table.Rows.Add(item.Cells["SaleNumber"].Value.ToString(),
                        item.Cells["Date"].Value.ToString(),
                        item.Cells["Customer"].Text.ToString(),
                        item.Cells["NetPrice"].Value.ToString(),
                        item.Cells["Percent"].Value.ToString(),
                        item.Cells["Price"].Value.ToString(),
                        bool.Parse(item.Cells["Returned"].Value.ToString()),
                        bool.Parse(item.Cells["Cancel"].Value.ToString()),
                        gridEX_List.GetRow().Cells["Column01"].Text.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 32, "از تاریخ:" + Date1, "تا تاریخ: " + Date2, "فاکتورهای خرید");
                    frm.ShowDialog();
                }
            }
        }

        private void bt_PrintAll_Return_Click(object sender, EventArgs e)
        {
            if (this.gridEX_List.RowCount > 0)
            {
                DataTable Table = dataSet_Report.Rpt_ExtRedReport.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow List in gridEX_List.GetRows())
                {
                    gridEX_List.MoveTo(List);
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Returned.GetRows())
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
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 33, "از تاریخ:" + Date1, "تا تاریخ: " + Date2, "فاکتورهای مرجوعی");
                    frm.ShowDialog();
                }
            }
        }

        private void bt_PrintThis_Returned_Click(object sender, EventArgs e)
        {
            if (this.gridEX_List.RowCount > 0)
            {
                DataTable Table = dataSet_Report.Rpt_ExtRedReport.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Returned.GetRows())
                {
                    Table.Rows.Add(item.Cells["SaleNumber"].Value.ToString(),
                        item.Cells["Date"].Value.ToString(),
                        item.Cells["Customer"].Text.ToString(),
                        item.Cells["NetPrice"].Value.ToString(),
                        item.Cells["Percent"].Value.ToString(),
                        item.Cells["Price"].Value.ToString(),
                        false,false,
                        gridEX_List.GetRow().Cells["Column01"].Text.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 33, "از تاریخ:" + Date1, "تا تاریخ: " + Date2, "فاکتورهای مرجوعی");
                    frm.ShowDialog();
                }
            }
        }
    }
}
