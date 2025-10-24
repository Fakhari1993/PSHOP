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
    public partial class Form11_CompReport_Goods : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1 = null, Date2 = null;

        public Form11_CompReport_Goods()
        {
            InitializeComponent();
        }

        private void Form11_CompReport_Goods_Load(object sender, EventArgs e)
        {
            gridEXFieldChooserControl1.GridEX = gridEX_Goods;
            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;


            string[] Dates = Properties.Settings.Default.CompReport_Goods1.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);


            gridEX_Goods.DropDowns["Person"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_045_PersonInfo");
            gridEX_Goods.DropDowns["Receipt"].DataSource = clDoc.ReturnTable(ConWare.ConnectionString, "Select Columnid,Column01 from Table_011_PwhrsReceipt");
            gridEX_Goods.DropDowns["Doc"].DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead");
            gridEX_Goods.DropDowns["Return"].DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select Columnid,Column01 from Table_021_MarjooiBuy");
            gridEX_Goods.DropDowns["Currency"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_055_CurrencyInfo"), "");
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

                CommandText = @"SELECT     dbo.Table_015_BuyFactor.columnid AS ID, dbo.Table_015_BuyFactor.column15 AS FactorType, dbo.Table_015_BuyFactor.column01 AS FactorNum, 
                      dbo.Table_015_BuyFactor.column02 AS Date, dbo.Table_015_BuyFactor.column03 AS Person, dbo.Table_015_BuyFactor.column14 AS BuyRespon, 
                      dbo.Table_015_BuyFactor.column10 AS Receipt, dbo.Table_015_BuyFactor.column11 AS Doc, dbo.Table_015_BuyFactor.column18 AS ReturnNumber, 
                      dbo.Table_015_BuyFactor.column19 AS Cancel, dbo.Table_015_BuyFactor.Column20 AS TotalPrice, dbo.Table_015_BuyFactor.Column21 AS Extra, 
                      dbo.Table_015_BuyFactor.Column22 AS Red, 
                      dbo.Table_015_BuyFactor.Column20 + dbo.Table_015_BuyFactor.Column21 - dbo.Table_015_BuyFactor.Column22 AS FinalPrice, 
                      dbo.Table_016_Child1_BuyFactor.column02 AS GoodCode, dbo.Table_016_Child1_BuyFactor.column02 AS GoodName, 
                      dbo.Table_016_Child1_BuyFactor.column03 AS CountUnit, dbo.Table_016_Child1_BuyFactor.column04 AS Box, dbo.Table_016_Child1_BuyFactor.column05 AS Pack, 
                      dbo.Table_016_Child1_BuyFactor.column06 AS Joz, dbo.Table_016_Child1_BuyFactor.column07 AS TotalCount, 
                      dbo.Table_016_Child1_BuyFactor.column08 AS BoxPrice, dbo.Table_016_Child1_BuyFactor.column09 AS PackPrice, 
                      dbo.Table_016_Child1_BuyFactor.column10 AS JozPrice, dbo.Table_016_Child1_BuyFactor.column11 AS KolPrice, 
                      dbo.Table_016_Child1_BuyFactor.Column34 AS SingleWeight, dbo.Table_016_Child1_BuyFactor.Column35 AS TotalWeight, 
                      dbo.Table_016_Child1_BuyFactor.Column31 AS Zarib, dbo.Table_016_Child1_BuyFactor.column16 AS DiscountPercent, 
                      dbo.Table_016_Child1_BuyFactor.column17 AS DiscountPrice, dbo.Table_016_Child1_BuyFactor.column18 AS ExtraPercent, 
                      dbo.Table_016_Child1_BuyFactor.column19 AS ExtraPrice, dbo.Table_016_Child1_BuyFactor.column20 AS NetPrice, 
                      dbo.Table_016_Child1_BuyFactor.column21 AS Center, dbo.Table_016_Child1_BuyFactor.column22 AS Project, dbo.Table_016_Child1_BuyFactor.Column32 AS Seri, 
                      dbo.Table_016_Child1_BuyFactor.Column33 AS ExpDate, dbo.Table_015_BuyFactor.Column25 AS CurrencyType, 
                      dbo.Table_015_BuyFactor.Column26 AS CurrencyValue,tmg.column02 AS MainGroup,
                            tsg.column03 AS SubGroup
                      FROM         dbo.Table_016_Child1_BuyFactor RIGHT OUTER JOIN
                      dbo.Table_015_BuyFactor ON dbo.Table_016_Child1_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid
                         JOIN " + ConWare.Database+@".dbo.table_004_CommodityAndIngredients tcai
                            ON  tcai.columnid = Table_016_Child1_BuyFactor.column02
                       JOIN "+ConWare.Database+@".dbo.table_003_SubsidiaryGroup tsg
                            ON  tsg.columnid = tcai.column04
                       JOIN "+ConWare.Database+@".dbo.table_002_MainGroup tmg
                            ON  tmg.columnid = tcai.column03
                      WHERE     (dbo.Table_015_BuyFactor.column02 >='{0}') and (dbo.Table_015_BuyFactor.column02 <='{1}')";

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
            //pageSetupDialog1.ShowDialog();
            //printPreviewDialog1.ShowDialog();
            DataSet_Report ds = new DataSet_Report();
            DataTable Table = ds.Rpt_BuyCompReport_Goods.Clone();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
            {
                Table.Rows.Add(item.Cells["FactorNum"].Text, item.Cells["Date"].Text,
                    item.Cells["Person"].Text, Convert.ToDouble(item.Cells["TotalCount"].Value.ToString()),
                    Convert.ToDouble(item.Cells["JozPrice"].Value.ToString()), Convert.ToDouble(item.Cells["KolPrice"].Value.ToString()),
                    item.Cells["GoodCode"].Text, item.Cells["GoodName"].Text, item.Cells["CountUnit"].Text,
                    Convert.ToDouble(item.Cells["DiscountPercent"].Value.ToString()), Convert.ToDouble(item.Cells["DiscountPrice"].Value.ToString()),
                    Convert.ToDouble(item.Cells["ExtraPercent"].Value.ToString()), Convert.ToDouble(item.Cells["ExtraPrice"].Value.ToString()),
                    Convert.ToDouble(item.Cells["NetPrice"].Value.ToString()));
            }

            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 45, faDatePickerStrip1.FADatePicker.Text,
                    faDatePickerStrip2.FADatePicker.Text);
                frm.ShowDialog();
            }
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
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 28))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_003_FaktorKharid")
                            {
                                item.BringToFront();
                                _04_Buy.Frm_003_FaktorKharid frm = (_04_Buy.Frm_003_FaktorKharid)item;
                                frm.txt_Search.Text = gridEX_Goods.GetRow().Cells["FactorNumber"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _04_Buy.Frm_003_FaktorKharid frms = new _04_Buy.Frm_003_FaktorKharid(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 29),
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
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.CompReport_Goods1 = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
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

        private void Form11_CompReport_Goods_Activated(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.Select();
        }
    }
}
