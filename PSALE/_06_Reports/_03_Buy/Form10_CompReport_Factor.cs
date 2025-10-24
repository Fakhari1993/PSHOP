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
    public partial class Form10_CompReport_Factor : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1 = null, Date2 = null;

        public Form10_CompReport_Factor()
        {
            InitializeComponent();
        }

        private void Form10_CompReport_Factor_Load(object sender, EventArgs e)
        {
            gridEXFieldChooserControl1.GridEX = gridEX_Factors;
            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            string[] Dates = Properties.Settings.Default.CompReport_Factor.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);

            gridEX_Factors.DropDowns["Person"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_045_PersonInfo");
            gridEX_Factors.DropDowns["Receipt"].DataSource = clDoc.ReturnTable(ConWare.ConnectionString, "Select Columnid,Column01 from Table_011_PwhrsReceipt");
            gridEX_Factors.DropDowns["Doc"].DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead");
            gridEX_Factors.DropDowns["Return"].DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select Columnid,Column01 from Table_021_MarjooiBuy");
            gridEX_Factors.DropDowns["Currency"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_055_CurrencyInfo"), "");
            gridEX_Factors.DropDowns["Project"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column02 from Table_035_ProjectInfo"), "");


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
                    CommandText = @"SELECT    dbo.Table_015_BuyFactor.columnId AS ID, dbo.Table_015_BuyFactor.column15 AS FactorType, dbo.Table_015_BuyFactor.column01 AS FactorNumber, dbo.Table_015_BuyFactor.column02 AS Date, 
                    dbo.Table_015_BuyFactor.column03 AS Person, dbo.Table_015_BuyFactor.column14 AS BuyRespon, dbo.Table_015_BuyFactor.column10 AS ReceiptNum, 
                    dbo.Table_015_BuyFactor.column11 AS DocNum, dbo.Table_015_BuyFactor.column18 AS ReturnFactorNum, dbo.Table_015_BuyFactor.column19 AS Cancel, 
                    dbo.Table_015_BuyFactor.column04 AS Description, dbo.Table_015_BuyFactor.Column25 AS CurrencyType, dbo.Table_015_BuyFactor.Column26 AS CurrencyValue, 
                    dbo.Table_015_BuyFactor.column12 AS PayConditions, dbo.Table_024_Discount_Buy.column01 AS ExRedName, 
                    dbo.Table_017_Child2_BuyFactor.column03 AS ExRedPercent, dbo.Table_017_Child2_BuyFactor.column04 AS ExRedPrice, 
                    dbo.Table_017_Child2_BuyFactor.column05 AS ExRedType, dbo.Table_015_BuyFactor.Column20 AS TotalFactor, dbo.Table_015_BuyFactor.Column21 AS TotalEx, 
                    dbo.Table_015_BuyFactor.Column22 AS TotalRed, 
                    dbo.Table_015_BuyFactor.Column20 + dbo.Table_015_BuyFactor.Column21 - dbo.Table_015_BuyFactor.Column22 AS FinalPrice
                    ,dbo.Table_015_BuyFactor.column29 as Project
                    FROM         dbo.Table_017_Child2_BuyFactor RIGHT OUTER JOIN
                    dbo.Table_015_BuyFactor ON dbo.Table_017_Child2_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid LEFT OUTER JOIN
                    dbo.Table_024_Discount_Buy ON dbo.Table_017_Child2_BuyFactor.column02 = dbo.Table_024_Discount_Buy.columnid
                    WHERE     (dbo.Table_015_BuyFactor.column02 >= '{0}') AND (dbo.Table_015_BuyFactor.column02 <= '{1}')";
                }
                else
                {
                    CommandText = @"SELECT     dbo.Table_015_BuyFactor.columnId AS ID,column15 AS FactorType, column01 AS FactorNumber, column02 AS Date, column03 AS Person, column14 AS BuyRespon, column10 AS ReceiptNum, 
                    column11 AS DocNum, column18 AS ReturnFactorNum, column19 AS Cancel, column04 AS Description, Column25 AS CurrencyType, Column26 AS CurrencyValue, 
                    column12 AS PayConditions, Column20 AS TotalFactor, Column21 AS TotalEx, Column22 AS TotalRed, Column20 + Column21 - Column22 AS FinalPrice,dbo.Table_015_BuyFactor.column29 as Project
                    FROM         dbo.Table_015_BuyFactor
                     WHERE     (column02 >= '{0}') AND (column02 <= '{1}')";
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
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 28))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_003_FaktorKharid")
                            {
                                item.BringToFront();
                                _04_Buy.Frm_003_FaktorKharid frm = (_04_Buy.Frm_003_FaktorKharid)item;
                                frm.txt_Search.Text = gridEX_Factors.GetRow().Cells["FactorNumber"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _04_Buy.Frm_003_FaktorKharid frms = new _04_Buy.Frm_003_FaktorKharid(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 29),
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
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.CompReport_Factor = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_Factors.RootTable.Groups.Clear();
            gridEX_Factors.RemoveFilters();
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            pageSetupDialog1.ShowDialog();
            printPreviewDialog1.ShowDialog();
        }

        private void Form10_CompReport_Factor_Activated(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.Select();
        }

       
    }
}
