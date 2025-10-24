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
    public partial class Form19_Visitors : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1, Date2;
        public Form19_Visitors()
        {
            InitializeComponent();
            gridEXFieldChooserControl1.GridEX = gridEX_Goods;
        }

        private void Form19_Visitors_Load(object sender, EventArgs e)
        {
            this.tbl_VitorsTableAdapter.Fill(this.dataSet_Report.Tbl_Vitors);
            string[] Dates = Properties.Settings.Default.Visitors.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
           
            gridEX_Goods.DropDowns["Province"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString,
                "Select Column00,Column01 from Table_060_ProvinceInfo"), "");
            gridEX_Goods.DropDowns["City"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString,
                "Select Column01,Column02 from Table_065_CityInfo"), "");
            gridEX_Goods.DropDowns["State"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString,
                "Select ColumnId,Column03 from Table_160_States"), "");
            gridEX_Goods.DropDowns["Visitor"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString,
                "SELECT ColumnId,Column01,Column02 FROM Table_045_PersonInfo"), "");
         
            DataTable PersonGroup = clDoc.ReturnTable(ConBase.ConnectionString, @"Select * from(
            Select distinct Tbl2.PersonId, 
            substring((Select ','+Tbl1.GroupName   AS [text()]
            From (SELECT     dbo.Table_045_PersonInfo.ColumnId AS PersonId, ISNULL(dbo.Table_040_PersonGroups.Column01, 'عمومی') AS GroupName
            FROM         dbo.Table_040_PersonGroups INNER JOIN
            dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 RIGHT OUTER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId) Tbl1
            Where Tbl1.PersonId = Tbl2.PersonId
              
            For XML PATH ('')),2, 1000) [PersonGroup]
            From (SELECT     dbo.Table_045_PersonInfo.ColumnId AS PersonId, ISNULL(dbo.Table_040_PersonGroups.Column01, 'عمومی') AS GroupName
            FROM         dbo.Table_040_PersonGroups INNER JOIN
            dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 RIGHT OUTER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId) Tbl2) as PersonGroup");
            
            gridEX_Goods.DropDowns["PersonGroup"].SetDataBinding(PersonGroup, "");
            gridEX_Goods.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_002_SalesTypes"), "");
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                string CommandText = null;
                CommandText = @"SELECT     TOP (100) PERCENT dbo.Table_010_SaleFactor.column03 AS CustomerId, PersonTable.Column01 AS CustomerCode, PersonTable.Column02 AS CustomerName, 
            dbo.Table_010_SaleFactor.column05 AS Visitor, PersonTable.Column21 AS Province, PersonTable.Column22 AS City, PersonTable.Column29 AS State, 
            dbo.Table_010_SaleFactor.Column36 AS SaleType, dbo.Table_010_SaleFactor.columnid AS SaleID, dbo.Table_010_SaleFactor.column01 AS SaleNumber, 
            dbo.Table_010_SaleFactor.column02 AS SaleDate, 
            dbo.Table_010_SaleFactor.Column28 - dbo.Table_010_SaleFactor.Column29 - dbo.Table_010_SaleFactor.Column30 - dbo.Table_010_SaleFactor.Column31 + dbo.Table_010_SaleFactor.Column32
            - dbo.Table_010_SaleFactor.Column33 AS FactorNetPrice, dbo.Table_010_SaleFactor.Column29 AS VolumeGroup, 
            dbo.Table_010_SaleFactor.Column30 AS SpecialGroup, dbo.Table_010_SaleFactor.Column31 AS SpecialCustomer, dbo.Table_010_SaleFactor.Column32 AS Extra, 
            dbo.Table_010_SaleFactor.Column33 AS Reduction, dbo.Table_010_SaleFactor.Column28 AS FactorPrice
            FROM         dbo.Table_010_SaleFactor LEFT OUTER JOIN
            (SELECT     ColumnId, Column01, Column02, Column21, Column22, Column29
            FROM         {2}.dbo.Table_045_PersonInfo) AS PersonTable ON dbo.Table_010_SaleFactor.column03 = PersonTable.ColumnId
            WHERE     (dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND '{1}')
            ORDER BY SaleNumber";

                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                bindingSource1.DataSource = Table;
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
            gridEXExporter1.GridEX = gridEX_Goods;
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
                if (this.gridEX_Goods.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 20))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_002_Faktor")
                            {
                                item.BringToFront();
                                _05_Sale.Frm_002_Faktor frm = (_05_Sale.Frm_002_Faktor)item;
                                frm.txt_Search.Text = gridEX_Goods.GetRow().Cells["SaleNumber"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _05_Sale.Frm_002_Faktor frms = new _05_Sale.Frm_002_Faktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 21),
                            Convert.ToInt32(gridEX_Goods.GetValue("SaleID").ToString()));
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
                Properties.Settings.Default.Visitors = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_Goods.RootTable.Groups.Clear();
            gridEX_Goods.RemoveFilters();
        }

        private void tbl_VitorsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.Filter = "Visitor=" + gridEX_visitors.GetValue("ColumnId").ToString();
            }
            catch 
            {
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_Sale_CustomerBase.Clone();

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
            {
                DataRow NewRow = Table.NewRow();
                //*************Header Information
                NewRow["FactorNum"] = item.Cells["SaleNumber"].Value.ToString();
                NewRow["Date"] = item.Cells["SaleDate"].Value.ToString();
                NewRow["Customer"] = item.Cells["CustomerName"].Text;
                NewRow["Seller"] = item.Cells["Visitor"].Text.ToString();
                NewRow["PreFactor"] = 0;
                NewRow["DraftNum"] = 0;
                NewRow["DocNum"] = 0;
                NewRow["Cancel"] = 0;
                NewRow["Return"] = 0;
                NewRow["ReturnNum"] = 0;
                NewRow["NetPrice"] = item.Cells["FactorPrice"].Value.ToString();
                NewRow["VolDis"] = item.Cells["VolumeGroup"].Value.ToString();
                NewRow["SpecGroupDis"] = item.Cells["SpecialGroup"].Value.ToString();
                NewRow["SpecCustomer"] = item.Cells["SpecialCustomer"].Value.ToString();
                NewRow["Extra"] = item.Cells["Extra"].Value.ToString();
                NewRow["Dis"] = item.Cells["Reduction"].Value.ToString();
                NewRow["FinalPrice"] = item.Cells["FactorNetPrice"].Value.ToString();
                NewRow["Description"] = "";
                Table.Rows.Add(NewRow);
            }

            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 38, Date1, Date2);
                frm.ShowDialog();
            }
        }

        private void یکمسئولفروشToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
