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
    public partial class Form14_CustomerGoods : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1=null, Date2 = null;

        public Form14_CustomerGoods()
        {
            InitializeComponent();
        }

        private void Form14_CustomerGoods_Load(object sender, EventArgs e)
        {
            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            string[] Dates = Properties.Settings.Default.CustomerGoods.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
            gridEX_Goods.DropDowns["Province"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_060_ProvinceInfo"), "");
            gridEX_Goods.DropDowns["City"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column01,Column02 from Table_065_CityInfo"), "");
            gridEX_Goods.DropDowns["State"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column03 from Table_160_States"), "");
            gridEX_Goods.DropDowns["Currency"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_055_CurrencyInfo"), "");
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
            gridEX_Goods.DropDowns["MainGroup"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString,"Select ColumnId,Column02 from table_002_MainGroup"), "");
            gridEX_Goods.DropDowns["SubGroup"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column03 from table_003_SubsidiaryGroup"), "");
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
               
                    CommandText = @"SELECT     dbo.Table_010_SaleFactor.column03 AS CustomerId, PersonTable.Column01 AS CustomerCode, PersonTable.Column02 AS CustomerName, 
                      dbo.Table_011_Child1_SaleFactor.column02 AS GoodId, GoodTable.column01 AS GoodCode, GoodTable.column02 AS GoodName, CountInfo.Column01 AS CountUnit, 
                      SUM(dbo.Table_011_Child1_SaleFactor.column04) AS BoxNumber, SUM(dbo.Table_011_Child1_SaleFactor.column05) AS PackNumber, 
                      SUM(dbo.Table_011_Child1_SaleFactor.column06) AS DetailNumber, SUM(dbo.Table_011_Child1_SaleFactor.column07) AS TotalNumber, 
                      SUM(dbo.Table_011_Child1_SaleFactor.column11) AS TotalPrice, 0.000 as TotalPrice2,SUM(dbo.Table_011_Child1_SaleFactor.column17) AS TotalDiscount, 0.000 as TotalDiscount2,
                      SUM(dbo.Table_011_Child1_SaleFactor.column19) AS TotalExtra,0.000 as TotalExtra2,SUM(dbo.Table_011_Child1_SaleFactor.Column20) as NetPrice, 0.000 as NetPrice2,
                      PersonTable.Column21 AS Province, PersonTable.Column22 AS City, PersonTable.column29 AS State, GoodTable.column03 AS MainGroup,  GoodTable.column04 AS SubGroup
                      ,Table_010_SaleFactor.Column12 as FactorType,Table_010_SaleFactor.Column40 as CurType,Table_010_SaleFactor.Column41 as CurValue,
                Table_011_Child1_SaleFactor.Column34 as BuildSeri,Table_011_Child1_SaleFactor.Column35 as ExpDate,Table_011_Child1_SaleFactor.Column36 as SingleWeight,SUM(Table_011_Child1_SaleFactor.COlumn37) as TotalWeight
                      FROM         dbo.Table_010_SaleFactor INNER JOIN
                      dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                          (SELECT     columnid, column01, column02, column03, column04
                             FROM         {3}.dbo.table_004_CommodityAndIngredients) AS GoodTable ON 
                      dbo.Table_011_Child1_SaleFactor.column02 = GoodTable.columnid INNER JOIN
                          (SELECT     Column00, Column01
                             FROM         {2}.dbo.Table_070_CountUnitInfo) AS CountInfo ON dbo.Table_011_Child1_SaleFactor.column03 = CountInfo.Column00 LEFT OUTER JOIN
                          (SELECT     ColumnId, Column01, Column02, Column21, Column22, column29
                             FROM         {2}.dbo.Table_045_PersonInfo) AS PersonTable ON dbo.Table_010_SaleFactor.column03 = PersonTable.ColumnId
                     WHERE     (dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND '{1}')
                     GROUP BY dbo.Table_010_SaleFactor.column03, PersonTable.Column01, PersonTable.Column02, dbo.Table_011_Child1_SaleFactor.column02, GoodTable.column01, 
                      GoodTable.column02, CountInfo.Column01, PersonTable.Column21, PersonTable.Column22, PersonTable.Column29, GoodTable.column03, GoodTable.column04, 
                      dbo.Table_010_SaleFactor.column12, dbo.Table_010_SaleFactor.Column41, dbo.Table_010_SaleFactor.Column40,
                      Table_011_Child1_SaleFactor.Column34,Table_011_Child1_SaleFactor.Column35,Table_011_Child1_SaleFactor.COlumn36";
               
                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database, ConWare.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                Table.Columns["TotalPrice2"].Expression = "IIF(FactorType=1, TotalPrice* CurValue,TotalPrice )";
                Table.Columns["TotalDiscount2"].Expression = "IIF(FactorType=1, TotalDiscount* CurValue,TotalDiscount )";
                Table.Columns["TotalExtra2"].Expression = "IIF(FactorType=1, TotalExtra* CurValue,TotalExtra )";
                Table.Columns["NetPrice2"].Expression = "IIF(FactorType=1, NetPrice* CurValue,NetPrice )";


                bindingSource2.DataSource = Table;

                Adapter = new SqlDataAdapter(@"SELECT     TOP (100) PERCENT dbo.Table_011_Child1_SaleFactor.column02 AS GoodId, dbo.Table_010_SaleFactor.column01 AS Number, 
                      dbo.Table_010_SaleFactor.column02 AS Date, dbo.Table_010_SaleFactor.column03 AS CustomerId, dbo.Table_010_SaleFactor.columnid as ID
            FROM         dbo.Table_011_Child1_SaleFactor INNER JOIN
                      dbo.Table_010_SaleFactor ON dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
            WHERE     (dbo.Table_010_SaleFactor.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"')
            GROUP BY dbo.Table_011_Child1_SaleFactor.column02, dbo.Table_010_SaleFactor.column01, dbo.Table_010_SaleFactor.column02, 
                      dbo.Table_010_SaleFactor.column03, 
                      dbo.Table_010_SaleFactor.columnid
            ORDER BY GoodId", ConSale);
                DataTable Table2 = new DataTable();
                Adapter.Fill(Table2);
                bindingSource1.DataSource = Table2;

                bindingSource2_PositionChanged(sender, e);
            }

        }

        private void bindingSource2_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.Filter = "GoodId=" + gridEX_Goods.GetValue("GoodId").ToString()+" and CustomerId="+gridEX_Goods.GetValue("CustomerId").ToString();
            }
            catch
            {
            }
        }

        private void gridEX_Factors_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
            {
                if (this.gridEX_Factors.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 20))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_002_Faktor")
                            {
                                item.BringToFront();
                                _05_Sale.Frm_002_Faktor frm = (_05_Sale.Frm_002_Faktor)item;
                                frm.txt_Search.Text = gridEX_Factors.GetRow().Cells["Number"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        _05_Sale.Frm_002_Faktor frms = new _05_Sale.Frm_002_Faktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 21),
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

        private void mnu_ExportToExcel_Detail_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Factors;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
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

        private void Form14_CustomerGoods_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
           
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Table = dataSet_Report.Rpt_Sale_CustomerGood.Clone();
                gridEX_Goods.RootTable.Groups.Clear();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                {
                    Table.ImportRow(((DataRowView)item.DataRow).Row);
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 37, Date1, Date2);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void Form14_CustomerGoods_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.CustomerGoods = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_Goods.RootTable.Groups.Clear();
            gridEX_Goods.RemoveFilters();
        }
    }
}
