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
    public partial class Form01_SaleDocuments : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        string Date1, Date2;

        public Form01_SaleDocuments()
        {
            InitializeComponent();
        }

        private void Form01_SaleDocuments_Load(object sender, EventArgs e)
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

                gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                gridEX_Header.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_007_PwhrsDraft"), "");
                gridEX_Header.DropDowns["Prefactor"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_007_FactorBefore"), "");
                gridEX_Header.DropDowns["Return"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_018_MarjooiSale"), "");
                DataTable CurrencyTable=clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_055_CurrencyInfo");
                gridEX_Header.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
                gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");


               

                Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
                Adapter.Fill(dataSet1, "Customer");
                gridEX_Header.DropDowns["Customer"].SetDataBinding(dataSet1.Tables["Customer"], "");



                gridEX_Header.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from ListPeople(5)"), "");

                DataTable dt = new DataTable();
                Adapter = new SqlDataAdapter(@"Select [columnid]
                                              ,[column01]
                                              ,[column02] from Table_002_SalesTypes", ConBase);
                Adapter.Fill(dt);
                this.gridEX_Header.DropDowns["SaleType"].DataSource = dt;
                this.gridEX_Header.DropDowns[9].DataSource = dt;

                string[] Dates = Properties.Settings.Default.SaleDocument.Split('-');
                faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
                faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);

                //*******************************************

                gridEX_Header.Select();
                gridEX_Header.Row = gridEX_Header.FilterRow.Position;
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                table_010_SaleFactorTableAdapter.FillForSaleDocument(dataSet_Sale.Table_010_SaleFactor, Date1, Date2);
                table_011_Child1_SaleFactorTableAdapter.FillBy_Date(dataSet_Sale.Table_011_Child1_SaleFactor);
                table_012_Child2_SaleFactorTableAdapter.FillBy_Date(dataSet_Sale.Table_012_Child2_SaleFactor);
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

        private void Form01_SaleDocuments_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.KeyCode == Keys.F5)
                bt_Refresh_Click(sender, e);
        }

        private void Form01_SaleDocuments_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Header.RemoveFilters();
            gridEX_List.RemoveFilters();
                if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                    Properties.Settings.Default.SaleDocument = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text; 
                Properties.Settings.Default.Save();
                GC.Collect();
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            gridEX_Header.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_007_PwhrsDraft"), "");
            gridEX_Header.DropDowns["Prefactor"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_007_FactorBefore"), "");
            gridEX_Header.DropDowns["Return"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_018_MarjooiSale"), "");
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_SaleDocument.Clone();
            DataTable Table2 = dataSet_Report.Rpt_SaleDocument_child.Clone();

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetRows())
            {
                gridEX_Header.MoveTo(item);
                foreach (Janus.Windows.GridEX.GridEXRow goodItem in gridEX_List.GetRows())
                {
                    DataRow NewRow = Table.NewRow();
                    //*************Header Information
                    NewRow["FactorNum"] = item.Cells["Column01"].Value.ToString();
                    NewRow["Date"] = item.Cells["Column02"].Value.ToString();
                    NewRow["Customer"] = item.Cells["Column03"].Text.ToString();
                    NewRow["Seller"] = item.Cells["Column05"].Text.ToString();
                    NewRow["PreFactor"] = item.Cells["Column07"].Text.ToString();
                    NewRow["DraftNum"] = item.Cells["Column09"].Text.ToString();
                    NewRow["DocNum"] = item.Cells["Column10"].Text.ToString();
                    NewRow["Cancel"] = item.Cells["Column17"].Value.ToString();
                    NewRow["Return"] = item.Cells["Column19"].Value.ToString();
                    NewRow["ReturnNum"] = item.Cells["Column20"].Text.ToString();
                    NewRow["NetPrice"] = item.Cells["Column28"].Value.ToString();
                    NewRow["VolDis"] = item.Cells["Column29"].Value.ToString();
                    NewRow["SpecGroupDis"] = item.Cells["Column30"].Value.ToString();
                    NewRow["SpecCustomer"] = item.Cells["Column31"].Value.ToString();
                    NewRow["Extra"] = item.Cells["Column32"].Value.ToString();
                    NewRow["Dis"] = item.Cells["Column33"].Value.ToString();
                    NewRow["FinalPrice"] = item.Cells["FinalPrice"].Value.ToString();
                    NewRow["Description"] = item.Cells["Column06"].Text;

                    //*************Detail Information
                    NewRow["GoodCode"] = goodItem.Cells["GoodCode"].Text;
                    NewRow["GoodName"] = goodItem.Cells["Column02"].Text;
                    NewRow["TotalCount"] = goodItem.Cells["Column07"].Value.ToString();
                    NewRow["TotalPrice"] = goodItem.Cells["Column11"].Value.ToString();
                    NewRow["LinDis"] = goodItem.Cells["Column17"].Value.ToString();
                    NewRow["LinExtra"] = goodItem.Cells["Column19"].Value.ToString();
                    NewRow["LinNet"] = goodItem.Cells["Column20"].Value.ToString();
                    NewRow["Project"] = goodItem.Cells["Column22"].Text;

                    Table.Rows.Add(NewRow);
                }
                foreach (Janus.Windows.GridEX.GridEXRow ExtraItem in gridEX_Extra.GetRows())
                {
                    Table2.Rows.Add(item.Cells["Column01"].Value.ToString(), ExtraItem.Cells["Column05"].Value.ToString(),
                        ExtraItem.Cells["Column02"].Text, ExtraItem.Cells["Column04"].Value.ToString());
                }
            }

            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table,Table2, 9,  Date1,  Date2);
                frm.ShowDialog();
            }
        }

        private void mnu_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Header;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void gridEX_Header_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
            {
                if (this.gridEX_Header.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 20))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_002_Faktor")
                            {
                                item.BringToFront();
                                _05_Sale.Frm_002_Faktor frm = (_05_Sale.Frm_002_Faktor)item;
                                frm.txt_Search.Text = gridEX_Header.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _05_Sale.Frm_002_Faktor frms = new _05_Sale.Frm_002_Faktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 21), Convert.ToInt32(gridEX_Header.GetValue("ColumnId").ToString()));
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
