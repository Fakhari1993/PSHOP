using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._05_Sale.LegalFactors
{
    public partial class Frm_022_ViewLegalFactors : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlDataAdapter HeaderAdapter, Child1Adapter, Child2Adapter;
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;

        public Frm_022_ViewLegalFactors()
        {
            InitializeComponent();
        }
        private void gridEX_Extra_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            try
            {
                if (e.Row.RowType == Janus.Windows.GridEX.RowType.Record)
                {
                    if (e.Row.Cells["column05"].Value.ToString() == "True")
                        e.Row.RowHeaderImageIndex = 1;
                    else
                        e.Row.RowHeaderImageIndex = 0;
                }
            }
            catch { }
        }
        private void Form_008_ViewSaleFactors_Load(object sender, EventArgs e)
        {

            SqlDataAdapter Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(dataSet1, "Center");
            gridEX_List.DropDowns["Center"].SetDataBinding(dataSet1.Tables["Center"], "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
            Adapter.Fill(dataSet1, "Project");
            gridEX_List.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Project"], "");

            Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
            DataTable GoodTable = clGood.GoodInfo();
            gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            Adapter = new SqlDataAdapter("Select * from Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(dataSet1, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(dataSet1.Tables["CountUnit"], "");

            Adapter = new SqlDataAdapter("Select * from Table_024_Discount", ConSale);
            Adapter.Fill(dataSet1, "Extra");
            gridEX_Extra.DropDowns["Extra"].SetDataBinding(dataSet1.Tables["Extra"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_055_LegalFactors", ConSale);
            Adapter.Fill(dataSet1, "SaleFactor");
            gridEX_Header.DropDowns["SaleFactor"].SetDataBinding(dataSet1.Tables["SaleFactor"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
            Adapter.Fill(dataSet1, "Customer");
            gridEX_Header.DropDowns["Customer"].SetDataBinding(dataSet1.Tables["Customer"], "");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_055_CurrencyInfo");
            gridEX_Header.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");

            gridEX_Header.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT ColumnId,Column01,Column02 from Table_002_SalesTypes"), "");
            gridEX_Header.DropDowns["SaleId"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_010_SaleFactor"), "");
            gridEX_Header.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from ListPeople(5)"), "");

            string[] Dates = Properties.Settings.Default.ViewSaleFactors.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);

            //*******************************************
            HeaderAdapter = new SqlDataAdapter("Select * from Table_055_LegalFactors where Column02>='" + faDatePickerStrip1.FADatePicker.Text + "' and Column02<='" + faDatePickerStrip2.FADatePicker.Text + "'", ConSale);
            HeaderAdapter.Fill(dataSet1, "Header");

            Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_060_Child1_LegalFactors.*    FROM         dbo.Table_055_LegalFactors INNER JOIN
            dbo.Table_060_Child1_LegalFactors ON dbo.Table_055_LegalFactors.columnid = dbo.Table_060_Child1_LegalFactors.column01 WHERE     (dbo.Table_055_LegalFactors.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_055_LegalFactors.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
            Child1Adapter.Fill(dataSet1, "Child1");

            Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_065_Child2_LegalFactors.*    FROM         dbo.Table_055_LegalFactors INNER JOIN
            dbo.Table_065_Child2_LegalFactors ON dbo.Table_055_LegalFactors.columnid = dbo.Table_065_Child2_LegalFactors.column01 WHERE     (dbo.Table_055_LegalFactors.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_055_LegalFactors.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
            Child2Adapter.Fill(dataSet1, "Child2");

            DataRelation Relation1 = new DataRelation("R_Header_Child1", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child1"].Columns["Column01"]);
            DataRelation Relation2 = new DataRelation("R_Header_Child2", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child2"].Columns["Column01"]);

            ForeignKeyConstraint Fkc1 = new ForeignKeyConstraint("F_Header_Child1", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child1"].Columns["Column01"]);
            ForeignKeyConstraint Fkc2 = new ForeignKeyConstraint("F_Header_Child2", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child2"].Columns["Column01"]);
            Fkc1.UpdateRule = Rule.Cascade;
            Fkc1.AcceptRejectRule = AcceptRejectRule.None;
            Fkc1.DeleteRule = Rule.None;
            Fkc2.UpdateRule = Rule.Cascade;
            Fkc2.AcceptRejectRule = AcceptRejectRule.None;
            Fkc2.DeleteRule = Rule.None;

            dataSet1.Tables["Child1"].Constraints.Add(Fkc1);
            dataSet1.Tables["Child2"].Constraints.Add(Fkc2);

            dataSet1.Relations.Add(Relation1);
            dataSet1.Relations.Add(Relation2);

            gridEX_Header.DataSource = dataSet1.Tables["Header"];
            gridEX_List.DataSource = dataSet1.Tables["Header"];
            gridEX_Extra.DataSource = dataSet1.Tables["Header"];
            gridEX_List.DataMember = "R_Header_Child1";
            gridEX_Extra.DataMember = "R_Header_Child2";
            gridEX_Header_CurrentCellChanged(sender, e);

            txt_TotalPrice.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column28");
            txt_VolumeGroup.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column29");
            txt_SpecialGroup.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column30");
            txt_SpecialCustomer.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column31");
            txt_Extra.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column32");
            txt_Reductions.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column33");
            gridEX_Header.Row = gridEX_Header.FilterRow.Position;

        }
        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.Text.Length == 10 && faDatePickerStrip1.FADatePicker.Text.Length == 10)
            {
                dataSet1.EnforceConstraints = false;
                dataSet1.Tables["Header"].Clear();
                dataSet1.Tables["Child1"].Clear();
                dataSet1.Tables["Child2"].Clear();

                HeaderAdapter = new SqlDataAdapter("Select * from Table_055_LegalFactors where Column02>='" + faDatePickerStrip1.FADatePicker.Text + "' and Column02<='" + faDatePickerStrip2.FADatePicker.Text + "'", ConSale);
                HeaderAdapter.Fill(dataSet1, "Header");

                Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_060_Child1_LegalFactors.*    FROM         dbo.Table_055_LegalFactors INNER JOIN
                 dbo.Table_060_Child1_LegalFactors ON dbo.Table_055_LegalFactors.columnid = dbo.Table_060_Child1_LegalFactors.column01 WHERE     (dbo.Table_055_LegalFactors.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_055_LegalFactors.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
                Child1Adapter.Fill(dataSet1, "Child1");

                Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_065_Child2_LegalFactors.*    FROM         dbo.Table_055_LegalFactors INNER JOIN
                dbo.Table_065_Child2_LegalFactors ON dbo.Table_055_LegalFactors.columnid = dbo.Table_065_Child2_LegalFactors.column01 WHERE     (dbo.Table_055_LegalFactors.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_055_LegalFactors.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
                Child2Adapter.Fill(dataSet1, "Child2");

                dataSet1.EnforceConstraints = true;
            }
        }

        private void Form_008_ViewSaleFactors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                faDatePickerStrip1.FADatePicker.Select();
                faDatePickerStrip1.Select();
            }
            else if (e.Control && e.KeyCode == Keys.E)
                bt_Edit_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.KeyCode == Keys.F5)
                bt_Refresh_Click(sender, e);
        }

        private void Form_008_ViewSaleFactors_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                gridEX_Extra.RemoveFilters();
                gridEX_Header.RemoveFilters();
                gridEX_List.RemoveFilters();
                if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                    Properties.Settings.Default.ViewSaleFactors = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
                Properties.Settings.Default.Save();
            }
            catch { }
        }
        private void bt_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridEX_Header.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 122))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_021_LegalFactor")
                            {
                                item.BringToFront();
                                Frm_021_LegalFactor frm = (Frm_021_LegalFactor)item;
                                frm.txt_Search.Text = gridEX_Header.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _05_Sale.LegalFactors.Frm_021_LegalFactor frms = new  Frm_021_LegalFactor(int.Parse(gridEX_Header.GetValue("ColumnId").ToString()));
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
            try
            {
                if (this.gridEX_Header.RowCount > 0)
                {
                    if (gridEX_Header.GetCheckedRows().Length == 0)
                    {
                        _05_Sale.LegalFactors.Reports.Form_LegalFactorPrint frm = new  Reports.Form_LegalFactorPrint(
                            int.Parse(gridEX_Header.GetValue("Column01").ToString()));
                        frm.ShowDialog();
                    }
                    else
                    {
                        List<string> List = new List<string>();
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetCheckedRows())
                        {
                            List.Add(item.Cells["ColumnId"].Value.ToString());
                        }
                        _05_Sale.LegalFactors.Reports.Form_LegalFactorPrint frm = new   Reports.Form_LegalFactorPrint(List);
                        frm.Form_FactorPrint_Load(sender, e);
                    }
                }
            }
            catch { }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            bt_Search_Click(sender, e);
        }
        private void gridEX_Header_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) - Convert.ToDouble(txt_VolumeGroup.Value.ToString()) - Convert.ToDouble(txt_SpecialGroup.Value.ToString()) - Convert.ToDouble(txt_SpecialCustomer.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }

        private void gridEX_List_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            try
            {
                if (e.Row.RowType == Janus.Windows.GridEX.RowType.Record && e.Row.Cells["column30"].Value.ToString() == "True")
                    e.Row.RowHeaderImageIndex =2;
            }
            catch { }
        }

        private void gridEX_Header_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            bt_Edit_Click(sender, e);
        }

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Header;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void mnu_ExportToExcel_Detail_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_List;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

    }
}
