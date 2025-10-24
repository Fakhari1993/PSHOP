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
    public partial class Form05_ServiceReport_Doc : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1, Date2; 

        public Form05_ServiceReport_Doc()
        {
            InitializeComponent();
        }


        private void Form05_ServiceReport_Doc_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_070_CountUnitInfo"), "");
            gridEX_List.DropDowns["Service"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_030_Services"), "");
            gridEX_Extra.DropDowns["Extra"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount"), "");
            gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");


            string s = @"Select ColumnId,Column01,Column02 from Table_045_PersonInfo";
            s = string.Format(s, ConSale.Database);
            gridEX_Header.DropDowns["Customer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, s), "");

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
                table_031_ServiceFactorTableAdapter.FillBy(dataset_Services.Table_031_ServiceFactor, Date1, Date2);
                this.table_032_ServiceFactor_Child1TableAdapter.FillBy(this.dataset_Services.Table_032_ServiceFactor_Child1);
                this.table_033_ServiceFactor_Child2TableAdapter.FillBy(this.dataset_Services.Table_033_ServiceFactor_Child2);
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

        private void Form05_ServiceReport_Doc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print.ShowDropDown();
            else if (e.KeyCode == Keys.F5)
                bt_Refresh_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Search_Click(sender, e);
        }

        private void Form05_ServiceReport_Doc_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Header.RemoveFilters();
            gridEX_List.RemoveFilters();
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_070_CountUnitInfo"), "");
            gridEX_List.DropDowns["Service"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_030_Services"), "");
            gridEX_Extra.DropDowns["Extra"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount"), "");
            gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            gridEX_Header.DropDowns["Customer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo"), "");
            bt_Search_Click(sender, e);
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

        private void bt_PrintThis_Click(object sender, EventArgs e)
        {
            if (this.table_031_ServiceFactorBindingSource.Count > 0)
            {
                try
                {
                    DataTable HeaderTbl = dataset_Services.Rpt_Header_Doc.Clone();
                    Janus.Windows.GridEX.GridEXRow RowHeader = gridEX_Header.GetRow();
                    HeaderTbl.Rows.Add(RowHeader.Cells["Column01"].Value.ToString(),
                        RowHeader.Cells["Column02"].Value.ToString(),
                        RowHeader.Cells["Column03"].Text,
                        RowHeader.Cells["Column04"].Text.Trim(),
                        RowHeader.Cells["Column05"].Text,
                        RowHeader.Cells["Column06"].Value.ToString(),
                        RowHeader.Cells["Column07"].Value.ToString(),
                        RowHeader.Cells["Column08"].Value.ToString(),
                        RowHeader.Cells["Column09"].Value.ToString(),
                        RowHeader.Cells["ColumnId"].Value.ToString());

                    DataTable Child1Tbl = dataset_Services.Rpt_Child1_Doc.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        Child1Tbl.Rows.Add(item.Cells["Column01"].Value.ToString(),
                            item.Cells["Column02"].Text.ToString(),
                            item.Cells["Column03"].Value.ToString(),
                            item.Cells["Column04"].Text,
                            item.Cells["Column05"].Value.ToString(),
                            item.Cells["Column06"].Value.ToString(),
                            item.Cells["Column07"].Text.ToString().Trim());
                    }

                    DataTable Child2Tbl = dataset_Services.Rpt_Child2_Doc.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                    {
                        Child2Tbl.Rows.Add(item.Cells["Column01"].Value.ToString(),
                            item.Cells["Column02"].Text,
                            (item.Cells["Column05"].Value.ToString() == "True" ? "-" : "+"),
                            item.Cells["Column04"].Value.ToString());
                    }
                    _07_Services.Reports.ReportForm frm = new Reports.ReportForm(3, HeaderTbl, Child1Tbl, Child2Tbl," "," ");
                    frm.ShowDialog();

                }
                catch { }
            }
        }

        private void bt_PrintAll_Click(object sender, EventArgs e)
        {
            if (gridEX_Header.RowCount > 0)
            {
                try
                {
                    DataTable HeaderTbl = dataset_Services.Rpt_Header_Doc.Clone();
                    DataTable Child1Tbl = dataset_Services.Rpt_Child1_Doc.Clone();
                    DataTable Child2Tbl = dataset_Services.Rpt_Child2_Doc.Clone();

                    foreach (Janus.Windows.GridEX.GridEXRow Header in gridEX_Header.GetRows())
                    {
                        gridEX_Header.MoveTo(Header);
                        HeaderTbl.Rows.Add(Header.Cells["Column01"].Value.ToString(),
                     Header.Cells["Column02"].Value.ToString(),
                     Header.Cells["Column03"].Text,
                     Header.Cells["Column04"].Text.Trim(),
                     Header.Cells["Column05"].Text,
                     Header.Cells["Column06"].Value.ToString(),
                     Header.Cells["Column07"].Value.ToString(),
                     Header.Cells["Column08"].Value.ToString(),
                     Header.Cells["Column09"].Value.ToString(),
                     Header.Cells["ColumnId"].Value.ToString());

                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                        {
                            Child1Tbl.Rows.Add(item.Cells["Column01"].Value.ToString(),
                                item.Cells["Column02"].Text.ToString(),
                                item.Cells["Column03"].Value.ToString(),
                                item.Cells["Column04"].Text,
                                item.Cells["Column05"].Value.ToString(),
                                item.Cells["Column06"].Value.ToString(),
                                item.Cells["Column07"].Text.ToString().Trim());
                        }

                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                        {
                            Child2Tbl.Rows.Add(item.Cells["Column01"].Value.ToString(),
                                item.Cells["Column02"].Text,
                                (item.Cells["Column05"].Value.ToString() == "True" ? "-" : "+"),
                                item.Cells["Column04"].Value.ToString());
                        }
                    }

                    _07_Services.Reports.ReportForm frm = new Reports.ReportForm(3, HeaderTbl, Child1Tbl, Child2Tbl, Date1,Date2);
                    frm.ShowDialog();
                }
                catch { }
            }
        }

        private void Form05_ServiceReport_Doc_Activated(object sender, EventArgs e)
        {
            try
            {
                gridEX_Header.Row = gridEX_Header.FilterRow.Position;
            }
            catch { }
        }

      
    }
}
