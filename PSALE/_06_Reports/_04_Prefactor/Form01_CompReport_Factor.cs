using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._06_Reports._04_Prefactor
{
    public partial class Form01_CompReport_Factor : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1 = null, Date2 = null;

        public Form01_CompReport_Factor()
        {
            InitializeComponent();
        }

        private void Form10_CompReport_Factor_Load(object sender, EventArgs e)
        {
            gridEXFieldChooserControl1.GridEX = gridEX_Factors;
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            gridEX_Factors.DropDowns["Person"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_045_PersonInfo");
            gridEX_Factors.DropDowns["Sale"].DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_010_SaleFactor");
            gridEX_Factors.DropDowns["Order"].DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_005_OrderHeader");
            gridEX_Factors.DropDowns["SaleType"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select Columnid,Column02 from Table_002_SalesTypes");



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
                if (chk_ShowExRed.Checked)
                {
                    CommandText = @"SELECT     dbo.Table_007_FactorBefore.columnid AS ID, dbo.Table_007_FactorBefore.column01 AS FactorNumber, dbo.Table_007_FactorBefore.column02 AS Date, 
                    dbo.Table_007_FactorBefore.column03 AS Person, dbo.Table_007_FactorBefore.column05 AS SalePerson, dbo.Table_007_FactorBefore.Column23 AS SaleType, 
                    dbo.Table_007_FactorBefore.column22 AS ExDate, ISNULL(dbo.Table_007_FactorBefore.column15,0) AS PayWay, ISNULL(dbo.Table_007_FactorBefore.column16,0) AS CashPrice, 
                    ISNULL(dbo.Table_007_FactorBefore.column17,0) AS Credit, dbo.Table_007_FactorBefore.column18 AS CreditTime, ISNULL(dbo.Table_007_FactorBefore.column19,0) AS Chq, 
                    ISNULL(dbo.Table_007_FactorBefore.column20,0) AS ChqPrice, dbo.Table_007_FactorBefore.column21 AS ChqTime, dbo.Table_007_FactorBefore.column06 AS Description, 
                    dbo.Table_007_FactorBefore.column11 AS OrderId, dbo.Table_007_FactorBefore.column12 AS SaleId, dbo.Table_024_Discount.column01 AS ExRedName, 
                    dbo.Table_009_Child2_FactorBefore.column03 AS ExRedPercent, dbo.Table_009_Child2_FactorBefore.column04 AS ExRedPrice, 
                    dbo.Table_009_Child2_FactorBefore.column05 AS ExRedType, ISNULL(Child.NetPrice,0) as NetPrice, ISNULL(Extra.ExRed, 0) AS ExPrice, ISNULL(Red.ExRed, 0) AS RedPrice, 
                    ISNULL(Child.NetPrice,0) + ISNULL(Extra.ExRed, 0) - ISNULL(Red.ExRed, 0) AS TotalPrice,
                    dbo.Table_007_FactorBefore.column04 AS SaleConditions
                    FROM         dbo.Table_007_FactorBefore LEFT OUTER JOIN
                    (SELECT     column01, SUM(column04) AS ExRed
                    FROM          dbo.Table_009_Child2_FactorBefore AS Table_009_Child2_FactorBefore_2
                    WHERE      (column05 = 1)
                    GROUP BY column01) AS Red ON dbo.Table_007_FactorBefore.columnid = Red.column01 LEFT OUTER JOIN
                    (SELECT     column01, SUM(column04) AS ExRed
                    FROM          dbo.Table_009_Child2_FactorBefore AS Table_009_Child2_FactorBefore_1
                    WHERE      (column05 = 0)
                    GROUP BY column01) AS Extra ON dbo.Table_007_FactorBefore.columnid = Extra.column01 LEFT OUTER JOIN
                    (SELECT     column01, SUM(column21) AS NetPrice
                    FROM          dbo.Table_008_Child1_FactorBefore
                    GROUP BY column01) AS Child ON dbo.Table_007_FactorBefore.columnid = Child.column01 LEFT OUTER JOIN
                    dbo.Table_009_Child2_FactorBefore LEFT OUTER JOIN
                    dbo.Table_024_Discount ON dbo.Table_009_Child2_FactorBefore.column02 = dbo.Table_024_Discount.columnid ON 
                    dbo.Table_007_FactorBefore.columnid = dbo.Table_009_Child2_FactorBefore.column01
                    WHERE     (dbo.Table_007_FactorBefore.column02 >= '{0}') and
                    (dbo.Table_007_FactorBefore.column02 <= '{1}')";
                }
                else
                {
                    CommandText = @"SELECT     dbo.Table_007_FactorBefore.columnid AS ID, dbo.Table_007_FactorBefore.column01 AS FactorNumber, dbo.Table_007_FactorBefore.column02 AS Date, 
                    dbo.Table_007_FactorBefore.column03 AS Person, dbo.Table_007_FactorBefore.column05 AS SalePerson, dbo.Table_007_FactorBefore.Column23 AS SaleType, 
                    dbo.Table_007_FactorBefore.column22 AS ExDate, ISNULL(dbo.Table_007_FactorBefore.column15,0) AS PayWay, ISNULL(dbo.Table_007_FactorBefore.column16,0) AS CashPrice, 
                    ISNULL(dbo.Table_007_FactorBefore.column17,0) AS Credit, dbo.Table_007_FactorBefore.column18 AS CreditTime, ISNULL(dbo.Table_007_FactorBefore.column19,0) AS Chq, 
                    ISNULL(dbo.Table_007_FactorBefore.column20,0) AS ChqPrice, dbo.Table_007_FactorBefore.column21 AS ChqTime, dbo.Table_007_FactorBefore.column06 AS Description, 
                    dbo.Table_007_FactorBefore.column11 AS OrderId, dbo.Table_007_FactorBefore.column12 AS SaleId, Child.NetPrice, ISNULL(Extra.ExRed, 0) AS ExPrice, 
                    ISNULL(Red.ExRed, 0) AS RedPrice,
                    ISNULL(Child.NetPrice,0) + ISNULL(Extra.ExRed, 0) - ISNULL(Red.ExRed, 0) AS TotalPrice,dbo.Table_007_FactorBefore.column04 AS SaleConditions
                    FROM         dbo.Table_007_FactorBefore LEFT OUTER JOIN
                    (SELECT     column01, SUM(column04) AS ExRed
                    FROM          dbo.Table_009_Child2_FactorBefore AS Table_009_Child2_FactorBefore_2
                    WHERE      (column05 = 1)
                    GROUP BY column01) AS Red ON dbo.Table_007_FactorBefore.columnid = Red.column01 LEFT OUTER JOIN
                    (SELECT     column01, SUM(column04) AS ExRed
                    FROM          dbo.Table_009_Child2_FactorBefore AS Table_009_Child2_FactorBefore_1
                    WHERE      (column05 = 0)
                    GROUP BY column01) AS Extra ON dbo.Table_007_FactorBefore.columnid = Extra.column01 LEFT OUTER JOIN
                    (SELECT     column01, SUM(column21) AS NetPrice
                    FROM          dbo.Table_008_Child1_FactorBefore
                    GROUP BY column01) AS Child ON dbo.Table_007_FactorBefore.columnid = Child.column01
                    WHERE     (dbo.Table_007_FactorBefore.column02 >= '{0}') AND (dbo.Table_007_FactorBefore.column02 <= '{1}')";
                }
                CommandText = string.Format(CommandText, Date1, Date2,
                ConBase.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                bindingSource1.DataSource = Table;
                gridEX_Factors.DataSource = bindingSource1;

            }
        }

        private void gridEX_Goods_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
            {
                if (this.gridEX_Factors.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 18))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_001_PishFaktor")
                            {
                                item.BringToFront();
                                _05_Sale.Frm_001_PishFaktor frm = (_05_Sale.Frm_001_PishFaktor)item;
                                frm.txt_Search.Text = gridEX_Factors.GetRow().Cells["FactorNumber"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _05_Sale.Frm_001_PishFaktor frms = new _05_Sale.Frm_001_PishFaktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 19),
                            Convert.ToInt32(gridEX_Factors.GetValue("ID").ToString()));
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
            gridEXExporter1.GridEX = gridEX_Factors;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void Form10_CompReport_Factor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }

        private void Form10_CompReport_Factor_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Factors.RootTable.Groups.Clear();
            gridEX_Factors.RemoveFilters();
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            pageSetupDialog1.ShowDialog();
            printPreviewDialog1.ShowDialog();
        }

        private void Form01_CompReport_Factor_Activated(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.Select();
        }

       
    }
}
