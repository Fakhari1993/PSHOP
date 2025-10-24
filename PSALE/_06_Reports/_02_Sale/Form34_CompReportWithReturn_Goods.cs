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
    public partial class Form34_CompReportWithReturn_Goods : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1=null, Date2 = null;

        public Form34_CompReportWithReturn_Goods()
        {
            InitializeComponent();
        }

        private void Form14_CustomerGoods_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_Sale4.DataTable1' table. You can move, or remove it, as needed.
           // this.dataTable1TableAdapter.Fill(this.dataSet_Sale4.DataTable1);
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

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
            gridEX_Goods.DropDowns["MainGroup"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column02 from table_002_MainGroup"), "");
            gridEX_Goods.DropDowns["SubGroup"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column03 from table_003_SubsidiaryGroup"), "");
            gridEX_Goods.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_002_SalesTypes"), "");
            gridEX_Goods.DropDowns["Currency"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_055_CurrencyInfo"), "");
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

                CommandText = @"SELECT N'فاکتور فروش' AS [type],
                                               dbo.Table_010_SaleFactor.column03 AS CustomerId,
                                               PersonTable.Column01 AS CustomerCode,
                                               PersonTable.Column02 AS CustomerName,
                                               dbo.Table_010_SaleFactor.column05 AS Visitor,
                                               dbo.Table_011_Child1_SaleFactor.column02 AS GoodId,
                                               GoodTable.column01 AS GoodCode,
                                               GoodTable.column02 AS GoodName,
                                               CountInfo.Column01 AS CountUnit,
                                               dbo.Table_011_Child1_SaleFactor.column07 AS TotalNumber,
                                               dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice,
                                               dbo.Table_011_Child1_SaleFactor.column17 AS TotalDiscount,
                                               dbo.Table_011_Child1_SaleFactor.column19 AS TotalExtra,
                                               dbo.Table_011_Child1_SaleFactor.column20 AS NetPrice,
                                               PersonTable.Column21 AS Province,
                                               PersonTable.Column22 AS City,
                                               PersonTable.Column29 AS STATE,
                                               GoodTable.column03 AS MainGroup,
                                               GoodTable.column04 AS SubGroup,
                                               dbo.Table_010_SaleFactor.columnid AS SaleID,
                                               dbo.Table_010_SaleFactor.column01 AS SaleNumber,
                                               dbo.Table_010_SaleFactor.column02 AS SaleDate,
                                               dbo.Table_010_SaleFactor.Column28 - dbo.Table_010_SaleFactor.Column29 - 
                                               dbo.Table_010_SaleFactor.Column30 - dbo.Table_010_SaleFactor.Column31 +
                                               dbo.Table_010_SaleFactor.Column32
                                               - dbo.Table_010_SaleFactor.Column33 AS FactorNetPrice,
                                               dbo.Table_011_Child1_SaleFactor.column30 AS Gift,
                                               Table_011_Child1_SaleFactor.Column34 AS BuildSeri,
                                               Table_011_Child1_SaleFactor.Column35 AS ExpDate,
                                               dbo.Table_010_SaleFactor.Column13
                                        FROM   dbo.Table_010_SaleFactor
                                               INNER JOIN dbo.Table_011_Child1_SaleFactor
                                                    ON  dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                                               INNER JOIN (
                                                        SELECT columnid,
                                                               column01,
                                                               column02,
                                                               column03,
                                                               column04
                                                        FROM   {3}.dbo.table_004_CommodityAndIngredients
                                                    ) AS GoodTable
                                                    ON  dbo.Table_011_Child1_SaleFactor.column02 = GoodTable.columnid
                                               INNER JOIN (
                                                        SELECT Column00,
                                                               Column01
                                                        FROM   {2}.dbo.Table_070_CountUnitInfo
                                                    ) AS CountInfo
                                                    ON  dbo.Table_011_Child1_SaleFactor.column03 = CountInfo.Column00
                                               LEFT OUTER JOIN (
                                                        SELECT ColumnId,
                                                               Column01,
                                                               Column02,
                                                               Column21,
                                                               Column22,
                                                               Column29
                                                        FROM   {2}.dbo.Table_045_PersonInfo
                                                    ) AS PersonTable
                                                    ON  dbo.Table_010_SaleFactor.column03 = PersonTable.ColumnId
                                        WHERE  (
                                                   dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND 
                                                  '{1}' AND dbo.Table_010_SaleFactor.column17=0
                                               )
                                               UNION  all
                                        SELECT N' مرجوعي فاکتور فروش' AS [type],
                                               dbo.Table_018_MarjooiSale.column03 AS CustomerId,
                                               PersonTable.Column01 AS CustomerCode,
                                               PersonTable.Column02 AS CustomerName,
                                               dbo.Table_018_MarjooiSale.column05 AS Visitor,
                                               dbo.Table_019_Child1_MarjooiSale.column02 AS GoodId,
                                               GoodTable.column01 AS GoodCode,
                                               GoodTable.column02 AS GoodName,
                                               CountInfo.Column01 AS CountUnit,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column07 AS TotalNumber,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column11 AS TotalPrice,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column17 AS TotalDiscount,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column19 AS TotalExtra,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column20 AS NetPrice,
                                               PersonTable.Column21 AS Province,
                                               PersonTable.Column22 AS City,
                                               PersonTable.Column29 AS STATE,
                                               GoodTable.column03 AS MainGroup,
                                               GoodTable.column04 AS SubGroup,
                                               dbo.Table_018_MarjooiSale.columnid AS SaleID,
                                               dbo.Table_018_MarjooiSale.column01 AS SaleNumber,
                                               dbo.Table_018_MarjooiSale.column02 AS SaleDate,
                                               (-1) * (
                                                   dbo.Table_018_MarjooiSale.Column18 - dbo.Table_018_MarjooiSale.Column20 
                                                   +
                                                   dbo.Table_018_MarjooiSale.Column19
                                               ) AS FactorNetPrice,
                                               CAST(0 AS BIT) AS Gift,
                                               Table_019_Child1_MarjooiSale.Column32 AS BuildSeri,
                                               Table_019_Child1_MarjooiSale.Column33 AS ExpDate,
                                               dbo.Table_018_MarjooiSale.Column13

                                        FROM   dbo.Table_018_MarjooiSale
                                               INNER JOIN dbo.Table_019_Child1_MarjooiSale
                                                    ON  dbo.Table_018_MarjooiSale.columnid = dbo.Table_019_Child1_MarjooiSale.column01
                                               INNER JOIN (
                                                        SELECT columnid,
                                                               column01,
                                                               column02,
                                                               column03,
                                                               column04
                                                        FROM   {3}.dbo.table_004_CommodityAndIngredients
                                                    ) AS GoodTable
                                                    ON  dbo.Table_019_Child1_MarjooiSale.column02 = GoodTable.columnid
                                               INNER JOIN (
                                                        SELECT Column00,
                                                               Column01
                                                        FROM   {2}.dbo.Table_070_CountUnitInfo
                                                    ) AS CountInfo
                                                    ON  dbo.Table_019_Child1_MarjooiSale.column03 = CountInfo.Column00
                                               LEFT OUTER JOIN (
                                                        SELECT ColumnId,
                                                               Column01,
                                                               Column02,
                                                               Column21,
                                                               Column22,
                                                               Column29
                                                        FROM   {2}.dbo.Table_045_PersonInfo
                                                    ) AS PersonTable
                                                    ON  dbo.Table_018_MarjooiSale.column03 = PersonTable.ColumnId
                                        WHERE  (
                                                   dbo.Table_018_MarjooiSale.column02 BETWEEN '{0}' AND 
                                                  '{1}'
                                               )";
               
                
                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database, ConWare.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);

                gridEX_Goods.DataSource = Table;

            }
        }

        private void gridEX_Factors_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
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

        private void Form14_CustomerGoods_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Goods.RootTable.Groups.Clear();
            gridEX_Goods.RemoveFilters();
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            pageSetupDialog1.ShowDialog();
            printPreviewDialog1.ShowDialog();
        }
    }
}
