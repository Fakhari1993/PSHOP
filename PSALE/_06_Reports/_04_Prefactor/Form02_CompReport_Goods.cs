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
    public partial class Form02_CompReport_Goods : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1 = null, Date2 = null;

        public Form02_CompReport_Goods()
        {
            InitializeComponent();
        }

        private void Form11_CompReport_Goods_Load(object sender, EventArgs e)
        {
            gridEXFieldChooserControl1.GridEX = gridEX_Goods;
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            gridEX_Goods.DropDowns["Person"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_045_PersonInfo");
            gridEX_Goods.DropDowns["Center"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column02 from Table_030_ExpenseCenterInfo");
            gridEX_Goods.DropDowns["Project"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column02 from Table_035_ProjectInfo");
            gridEX_Goods.DropDowns["CountUnit"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_070_CountUnitInfo");
            DataTable GoodTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients");
            gridEX_Goods.DropDowns["GoodCode"].DataSource = GoodTable;
            gridEX_Goods.DropDowns["GoodName"].DataSource = GoodTable;

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
                string CommandText = null;

                CommandText = @"SELECT     dbo.Table_007_FactorBefore.columnid AS ID, dbo.Table_007_FactorBefore.column01 AS FactorNumber, dbo.Table_007_FactorBefore.column02 AS Date, 
                dbo.Table_007_FactorBefore.column03 AS Person, dbo.Table_007_FactorBefore.column05 AS SalePerson, dbo.Table_007_FactorBefore.column06 AS Description, 
                ISNULL(Child.NetPrice, 0) AS NetPrice, ISNULL(Extra.ExRed, 0) AS ExPrice, ISNULL(Red.ExRed, 0) AS RedPrice, ISNULL(Child.NetPrice, 0) + ISNULL(Extra.ExRed, 0) 
                - ISNULL(Red.ExRed, 0) AS TotalPrice, dbo.Table_008_Child1_FactorBefore.column02 AS GoodCode, dbo.Table_008_Child1_FactorBefore.column02 AS GoodName, 
                dbo.Table_008_Child1_FactorBefore.column03 AS CountUnit, dbo.Table_008_Child1_FactorBefore.column04 AS Box, 
                dbo.Table_008_Child1_FactorBefore.column05 AS Pack, dbo.Table_008_Child1_FactorBefore.column06 AS Joz, dbo.Table_008_Child1_FactorBefore.column07 AS Kol, 
                dbo.Table_008_Child1_FactorBefore.column08 AS BoxPrice, dbo.Table_008_Child1_FactorBefore.column09 AS PackPrice, 
                dbo.Table_008_Child1_FactorBefore.column10 AS SinglePrice, dbo.Table_008_Child1_FactorBefore.column11 AS KolPrice, 
                dbo.Table_008_Child1_FactorBefore.column16 AS LinDisPer, dbo.Table_008_Child1_FactorBefore.column17 AS LinDisPrice, 
                dbo.Table_008_Child1_FactorBefore.column18 AS LinExPer, dbo.Table_008_Child1_FactorBefore.column19 AS LinExPrice, 
                dbo.Table_008_Child1_FactorBefore.column21 AS Net, dbo.Table_008_Child1_FactorBefore.column22 AS Center, 
                dbo.Table_008_Child1_FactorBefore.column23 AS Project
                FROM         dbo.Table_007_FactorBefore INNER JOIN
                dbo.Table_008_Child1_FactorBefore ON dbo.Table_007_FactorBefore.columnid = dbo.Table_008_Child1_FactorBefore.column01 LEFT OUTER JOIN
                (SELECT     column01, SUM(column04) AS ExRed
                FROM          dbo.Table_009_Child2_FactorBefore AS Table_009_Child2_FactorBefore_2
                WHERE      (column05 = 1)
                GROUP BY column01) AS Red ON dbo.Table_007_FactorBefore.columnid = Red.column01 LEFT OUTER JOIN
                (SELECT     column01, SUM(column04) AS ExRed
                FROM          dbo.Table_009_Child2_FactorBefore AS Table_009_Child2_FactorBefore_1
                WHERE      (column05 = 0)
                GROUP BY column01) AS Extra ON dbo.Table_007_FactorBefore.columnid = Extra.column01 LEFT OUTER JOIN
                (SELECT     column01, SUM(column21) AS NetPrice
                FROM          dbo.Table_008_Child1_FactorBefore AS Table_008_Child1_FactorBefore_1
                
                GROUP BY column01) AS Child ON dbo.Table_007_FactorBefore.columnid = Child.column01
                WHERE     (dbo.Table_007_FactorBefore.column02 >='{0}') and (dbo.Table_007_FactorBefore.column02 <='{1}')
                ";

                CommandText = string.Format(CommandText, Date1, Date2);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                bindingSource1.DataSource = Table;
                gridEX_Goods.DataSource = bindingSource1;

            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            pageSetupDialog1.ShowDialog();
            printPreviewDialog1.ShowDialog();
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
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 18))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_001_PishFaktor")
                            {
                                item.BringToFront();
                                _05_Sale.Frm_001_PishFaktor frm = (_05_Sale.Frm_001_PishFaktor)item;
                                frm.txt_Search.Text = gridEX_Goods.GetRow().Cells["FactorNumber"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _05_Sale.Frm_001_PishFaktor frms = new _05_Sale.Frm_001_PishFaktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 19),
                            Convert.ToInt32(gridEX_Goods.GetValue("ID").ToString()));
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

        private void Form11_CompReport_Goods_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Goods.RootTable.Groups.Clear();
            gridEX_Goods.RemoveFilters();
        }

        private void Form11_CompReport_Goods_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }

        private void Form02_CompReport_Goods_Activated(object sender, EventArgs e)
        {
                faDatePickerStrip1.FADatePicker.Select();
        }
    }
}
