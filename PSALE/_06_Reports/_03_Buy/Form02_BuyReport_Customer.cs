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
    public partial class Form02_BuyReport_Customer : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        string Date1, Date2;

        public Form02_BuyReport_Customer()
        {
            InitializeComponent();
        }

        private void Form02_SaleReport_Customer_Load(object sender, EventArgs e)
        {
            crystalReportViewer1.BackColor = Color.White;

            gridEX_List.DropDowns["Center"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo"), "");
            gridEX_List.DropDowns["Project"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_035_ProjectInfo"), "");

            Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
            DataTable GoodTable = clGood.GoodInfo();
            gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString,"Select * from Table_070_CountUnitInfo"),"");
            gridEX_Extra.DropDowns["Extra"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount_Buy"), "");
            gridEX_Factors.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            gridEX_Factors.DropDowns["Receipt"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"), "");
            gridEX_List.DropDowns["Request"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_013_RequestBuy"), "");
            gridEX_Factors.DropDowns["Return"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_021_MarjooiBuy"), "");
            gridEX_Customers.DropDowns["Person"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo"), "");

            gridEX_Factors.DropDowns["Buyer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString,"SELECT        *  FROM  PeopleScope(8, 4)"),"");
            gridEX_Factors.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT        *  FROM  ListPeople(5)"), "");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_055_CurrencyInfo");
            gridEX_Factors.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");

            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-2);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;


            string[] Dates = Properties.Settings.Default.BuyReport_Customer.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);



            cmb_Cancel.SelectedIndex = 0;

            //*******************************************

            gridEX_Factors.Select();
            gridEX_Factors.Row = gridEX_Factors.FilterRow.Position;
        }

        private void mnu_ExportToExcel_Customers_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Customers;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void mnu_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Factors;
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
                    bt_Search_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                if (cmb_Cancel.ComboBox.SelectedIndex == 0)
                {
                    this.rpt_BuyFactor_CustomerTableAdapter.FillBy_All(dataSet_Buy.Rpt_BuyFactor_Customer, Date1, Date2);
                    this.rpt_BuyFactor_Customer_DetailTableAdapter.FillBy_All(dataSet_Buy.Rpt_BuyFactor_Customer_Detail, Date1, Date2);
                    this.rpt_BuyFactor_Customer_Child1TableAdapter.Fill_Date(dataSet_Buy.Rpt_BuyFactor_Customer_Child1, Date1, Date2);
                    this.rpt_BuyFactor_Customer_Child2TableAdapter.Fill_Date(dataSet_Buy.Rpt_BuyFactor_Customer_Child2, Date1, Date2);
                }
                else
                {
                    switch (cmb_Cancel.ComboBox.SelectedIndex)
                    {
                        case 1:
                    this.rpt_BuyFactor_CustomerTableAdapter.Fill_Cancel(dataSet_Buy.Rpt_BuyFactor_Customer, Date1, Date2,true);
                    this.rpt_BuyFactor_Customer_DetailTableAdapter.Fill_Cancel(dataSet_Buy.Rpt_BuyFactor_Customer_Detail, Date1, Date2,true);
                    this.rpt_BuyFactor_Customer_Child1TableAdapter.Fill_Date(dataSet_Buy.Rpt_BuyFactor_Customer_Child1, Date1, Date2);
                    this.rpt_BuyFactor_Customer_Child2TableAdapter.Fill_Date(dataSet_Buy.Rpt_BuyFactor_Customer_Child2, Date1, Date2);

                            break;
                        case 2:
                   this.rpt_BuyFactor_CustomerTableAdapter.Fill_Cancel(dataSet_Buy.Rpt_BuyFactor_Customer, Date1, Date2,false);
                    this.rpt_BuyFactor_Customer_DetailTableAdapter.Fill_Cancel(dataSet_Buy.Rpt_BuyFactor_Customer_Detail, Date1, Date2,false);
                    this.rpt_BuyFactor_Customer_Child1TableAdapter.Fill_Date(dataSet_Buy.Rpt_BuyFactor_Customer_Child1, Date1, Date2);
                    this.rpt_BuyFactor_Customer_Child2TableAdapter.Fill_Date(dataSet_Buy.Rpt_BuyFactor_Customer_Child2, Date1, Date2);
                            break;
                    }
                }
            }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            gridEX_Factors.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            gridEX_Factors.DropDowns["Receipt"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"), "");
            gridEX_Factors.DropDowns["Return"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_021_MarjooiBuy"), "");
            gridEX_List.DropDowns["Request"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_013_RequestBuy"), "");
            this.Cursor = Cursors.Default;
        }

        private void Form02_SaleReport_Customer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print.ShowDropDown();
            else if (e.KeyCode == Keys.F5)
                bt_Refresh_Click(sender, e);
        }

        private void Form02_SaleReport_Customer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.BuyReport_Customer = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_Factors.RemoveFilters();
            gridEX_List.RemoveFilters();
        }

        private void gridEX_Customers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
                if (gridEX_Customers.HitTest(e.X, e.Y) == Janus.Windows.GridEX.GridArea.ColumnHeader)
                {
                    Janus.Windows.GridEX.GridEXColumn ColClicked = gridEX_Customers.ColumnFromPoint(e.X, e.Y);
                    if (ColClicked.Key == "Total")
                    {
                        DataTable Table = dataSet_Report.Rpt_Chart.Clone();
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Customers.GetRows())
                        {
                            Table.Rows.Add(item.Cells["Customer"].Text, item.Cells["Total"].Value.ToString());
                        }
                        if (Table.Rows.Count > 0)
                        {
                            _06_Reports._03_Buy.Rpt02_Buy_Charts rpt = new Rpt02_Buy_Charts();
                            rpt.SetDataSource(Table);
                            crystalReportViewer1.ReportSource = rpt;
                            uiPanel6.Text = "نمودار-جمع مبلغ خریداری شده از تأمین کننده";
                        }
                    }

                }
        }

        private void gridEX_Factors_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gridEX_Factors.HitTest(e.X, e.Y) == Janus.Windows.GridEX.GridArea.ColumnHeader)
            {
                Janus.Windows.GridEX.GridEXColumn ColClicked = gridEX_Factors.ColumnFromPoint(e.X, e.Y);
                if (ColClicked.Key == "Column20")
                {
                    DataTable Table = dataSet_Report.Rpt_Chart.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Factors.GetRows())
                    {
                        Table.Rows.Add(item.Cells["column01"].Text, item.Cells["Column20"].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._03_Buy.Rpt02_Buy_Charts rpt = new Rpt02_Buy_Charts();
                        rpt.SetDataSource(Table);
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel6.Text = "نمودار-مبلغ خالص فاکتورها";
                    }
                }
               
                else if (ColClicked.Key == "Column21")
                {
                    DataTable Table = dataSet_Report.Rpt_Chart.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Factors.GetRows())
                    {
                        Table.Rows.Add(item.Cells["column01"].Text, item.Cells["Column21"].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._03_Buy.Rpt02_Buy_Charts rpt = new Rpt02_Buy_Charts();
                        rpt.SetDataSource(Table);
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel6.Text = "نمودار اضافات";
                    }
                }
                else if (ColClicked.Key == "Column22")
                {
                    DataTable Table = dataSet_Report.Rpt_Chart.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Factors.GetRows())
                    {
                        Table.Rows.Add(item.Cells["column01"].Text, item.Cells["Column22"].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._03_Buy.Rpt02_Buy_Charts rpt = new Rpt02_Buy_Charts();
                        rpt.SetDataSource(Table);
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel6.Text = "نمودار کسورات";
                    }
                }

            }
        }

        private void gridEX_List_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gridEX_List.HitTest(e.X, e.Y) == Janus.Windows.GridEX.GridArea.ColumnHeader)
            {
                Janus.Windows.GridEX.GridEXColumn ColClicked = gridEX_List.ColumnFromPoint(e.X, e.Y);
                if (ColClicked.Key == "column07")
                {
                    DataTable Table = dataSet_Report.Rpt_Chart.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        Table.Rows.Add(item.Cells["column02"].Text, item.Cells["column07"].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._03_Buy.Rpt02_Buy_Charts rpt = new Rpt02_Buy_Charts();
                        rpt.SetDataSource(Table);
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel6.Text = "نمودار تعداد کل";
                    }
                }
                else if (ColClicked.Key == "column11")
                {
                    DataTable Table = dataSet_Report.Rpt_Chart.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        Table.Rows.Add(item.Cells["column02"].Text, item.Cells["column11"].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._03_Buy.Rpt02_Buy_Charts rpt = new Rpt02_Buy_Charts();
                        rpt.SetDataSource(Table);
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel6.Text = "نمودار مبلغ کل";
                    }
                }
                else if (ColClicked.Key == "column20")
                {
                    DataTable Table = dataSet_Report.Rpt_Chart.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        Table.Rows.Add(item.Cells["column02"].Text, item.Cells["column20"].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._03_Buy.Rpt02_Buy_Charts rpt = new Rpt02_Buy_Charts();
                        rpt.SetDataSource(Table);
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel6.Text = "نمودار-خالص خطی";
                    }
                }
            }
        }

        private void mnu_PrintThis_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_Sale_CustomerBase.Clone();

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Factors.GetRows())
            {
                DataRow NewRow = Table.NewRow();
                //*************Header Information
                NewRow["FactorNum"] = item.Cells["Column01"].Value.ToString();
                NewRow["Date"] = item.Cells["Column02"].Value.ToString();
                NewRow["Customer"] = gridEX_Customers.GetRow().Cells["Customer"].Text;
                NewRow["Seller"] = item.Cells["Column14"].Text.ToString();
                NewRow["PreFactor"] = 0;
                NewRow["DraftNum"] = item.Cells["Column10"].Text.ToString();
                NewRow["DocNum"] = item.Cells["Column11"].Text.ToString();
                NewRow["Cancel"] = item.Cells["Column19"].Value.ToString();
                NewRow["Return"] = item.Cells["Column17"].Value.ToString();
                NewRow["ReturnNum"] = item.Cells["Column18"].Text.ToString();
                NewRow["NetPrice"] = item.Cells["Column20"].Value.ToString();
                NewRow["VolDis"] = 0;
                NewRow["SpecGroupDis"] = 0;
                NewRow["SpecCustomer"] = 0;
                NewRow["Extra"] = item.Cells["Column21"].Value.ToString();
                NewRow["Dis"] = item.Cells["Column22"].Value.ToString();
                NewRow["FinalPrice"] = item.Cells["FinalPrice"].Value.ToString();
                NewRow["Description"] = item.Cells["Column04"].Text;
                Table.Rows.Add(NewRow);
            }

            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 17,  Date1, Date2);
                frm.ShowDialog();
            }
        }

        private void mnu_PrintAll_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_Sale_CustomerBase.Clone();

            foreach (Janus.Windows.GridEX.GridEXRow CustomerRow in gridEX_Customers.GetRows())
            {
                gridEX_Customers.MoveTo(CustomerRow);
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Factors.GetRows())
                {
                    DataRow NewRow = Table.NewRow();
                    //*************Header Information
                    NewRow["FactorNum"] = item.Cells["Column01"].Value.ToString();
                    NewRow["Date"] = item.Cells["Column02"].Value.ToString();
                    NewRow["Customer"] = CustomerRow.Cells["Customer"].Text;
                    NewRow["Seller"] = item.Cells["Column14"].Text.ToString();
                    NewRow["PreFactor"] = 0;
                    NewRow["DraftNum"] = item.Cells["Column10"].Text.ToString();
                    NewRow["DocNum"] = item.Cells["Column11"].Text.ToString();
                    NewRow["Cancel"] = item.Cells["Column19"].Value.ToString();
                    NewRow["Return"] = item.Cells["Column17"].Value.ToString();
                    NewRow["ReturnNum"] = item.Cells["Column18"].Text.ToString();
                    NewRow["NetPrice"] = item.Cells["Column20"].Value.ToString();
                    NewRow["VolDis"] = 0;
                    NewRow["SpecGroupDis"] = 0;
                    NewRow["SpecCustomer"] = 0;
                    NewRow["Extra"] = item.Cells["Column21"].Value.ToString();
                    NewRow["Dis"] = item.Cells["Column22"].Value.ToString();
                    NewRow["FinalPrice"] = item.Cells["FinalPrice"].Value.ToString();
                    NewRow["Description"] = item.Cells["Column04"].Text;
                    Table.Rows.Add(NewRow);
                }
            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 17,  Date1, Date2);
                frm.ShowDialog();
            }
        }

    }
}
