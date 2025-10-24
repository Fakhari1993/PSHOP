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
    public partial class Form30_SeasonReport : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1=null, Date2 = null;

        public Form30_SeasonReport()
        {
            InitializeComponent();
        }

        private void Form14_CustomerGoods_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            gridEX1.DropDowns["Ware"].DataSource = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from Table_001_PWHRS");
            DataTable GoodTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select Columnid,Column01,Column02 from table_004_CommodityAndIngredients");
            gridEX1.DropDowns["GoodCode"].DataSource = GoodTable;
            gridEX1.DropDowns["GoodName"].DataSource = GoodTable;
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

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                string CommandText = null;

                CommandText = @"SELECT     dbo.Table_010_SaleFactor.columnid AS SaleId, dbo.Table_010_SaleFactor.column01 AS SaleNumber, dbo.Table_010_SaleFactor.column02 AS Date, 
                DraftTbl.column03 AS WareId, dbo.Table_010_SaleFactor.column03 AS PersonId, dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice, 
                dbo.Table_011_Child1_SaleFactor.column17 AS DiscountPrice, 
                dbo.Table_011_Child1_SaleFactor.column11 - dbo.Table_011_Child1_SaleFactor.column17 AS AfterDis, dbo.Table_011_Child1_SaleFactor.column19 AS Tax, 
                dbo.Table_011_Child1_SaleFactor.column20 AS FinalPrice, PersonTbl.Column01 AS PersonCode, PersonTbl.Column02 AS PersonName, 
                PersonTbl.Column06 AS Address, PersonTbl.Column07 AS Tel, PersonTbl.Column09 AS EcoCode, PersonTbl.Column13 AS PostalCode, 
                dbo.Table_011_Child1_SaleFactor.column02 AS GoodID
                FROM         dbo.Table_011_Child1_SaleFactor INNER JOIN
                dbo.Table_010_SaleFactor ON dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid LEFT OUTER JOIN
                (SELECT     ColumnId, Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, 
                    Column12, Column13
                FROM         {0}.dbo.Table_045_PersonInfo) AS PersonTbl ON dbo.Table_010_SaleFactor.column03 = PersonTbl.ColumnId LEFT OUTER JOIN
                (SELECT     columnid, column03
                FROM         {1}.dbo.Table_007_PwhrsDraft) AS DraftTbl ON dbo.Table_010_SaleFactor.column09 = DraftTbl.columnid
                 WHERE     (dbo.Table_010_SaleFactor.column02 >='{2}' and dbo.Table_010_SaleFactor.column02 <= '{3}')";

               
                CommandText = string.Format(CommandText,ConBase.Database, ConWare.Database,Date1,Date2);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
             
                bindingSource1.DataSource = Table;

            }
        }

       

        private void gridEX_Factors_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
            {
                if (this.gridEX1.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 20))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_002_Faktor")
                            {
                                item.BringToFront();
                                _05_Sale.Frm_002_Faktor frm = (_05_Sale.Frm_002_Faktor)item;
                                frm.txt_Search.Text = gridEX1.GetRow().Cells["SaleNumber"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _05_Sale.Frm_002_Faktor frms = new _05_Sale.Frm_002_Faktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 21),
                            Convert.ToInt32(gridEX1.GetValue("SaleID").ToString()));
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

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void Form14_CustomerGoods_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }

        private void Form14_CustomerGoods_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RootTable.Groups.Clear();
            gridEX1.RemoveFilters();
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            pageSetupDialog1.ShowDialog();
            printPreviewDialog1.ShowDialog();
        }
    }
}
