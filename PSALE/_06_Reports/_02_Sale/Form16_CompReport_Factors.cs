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
    public partial class Form16_CompReport_Factors : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);

        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1=null, Date2 = null;

        public Form16_CompReport_Factors()
        {
            InitializeComponent();
        }

        private void Form14_CustomerGoods_Load(object sender, EventArgs e)
        {
            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            string[] Dates = Properties.Settings.Default.CompReport_Factors.Split('-');
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
            //gridEX_Goods.DropDowns["MainGroup"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column02 from table_002_MainGroup"), "");
            //gridEX_Goods.DropDowns["SubGroup"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column03 from table_003_SubsidiaryGroup"), "");
            gridEX_Goods.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_002_SalesTypes"), "");
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
           if(faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
           {
               Date1 = null; Date2 = null;
               Date1 = faDatePickerStrip1.FADatePicker.Text;
               Date2 = faDatePickerStrip2.FADatePicker.Text;
            string CommandText = null;

            CommandText = @"SELECT TOP(100) PERCENT
                                   tpi.Column02 AS seller,
                                   tpd.column01 AS havale,
                                   tsh.Column00 AS sanad,
                                   tpi2.Column02 AS project,
                                   (
                                       SELECT SUM(tcsf.column20)
                                       FROM   Table_011_Child1_SaleFactor tcsf
                                       WHERE  tcsf.column01 = dbo.Table_010_SaleFactor.columnid
                                   ) AS khales,
                                   dbo.Table_010_SaleFactor.column03 AS CustomerId,
                                   PersonTable.Column01 AS CustomerCode,
                                   PersonTable.Column02 AS CustomerName,
                                   PersonTable.Column21 AS Province,
                                   PersonTable.Column22 AS City,
                                   PersonTable.Column29 AS STATE,
                                   dbo.Table_010_SaleFactor.Column36 AS SaleType,
                                   dbo.Table_010_SaleFactor.columnid AS SaleID,
                                   dbo.Table_010_SaleFactor.column01 AS SaleNumber,
                                   dbo.Table_010_SaleFactor.column02 AS SaleDate,
                                   dbo.Table_010_SaleFactor.Column28 - dbo.Table_010_SaleFactor.Column29 - 
                                   dbo.Table_010_SaleFactor.Column30 - dbo.Table_010_SaleFactor.Column31 +
                                   dbo.Table_010_SaleFactor.Column32
                                   - dbo.Table_010_SaleFactor.Column33 AS FactorNetPrice,
                                   0.000 AS FactorNetPrice2,
                                   dbo.Table_010_SaleFactor.Column29 AS VolumeGroup,
                                   0.000 AS VolumeGroup2,
                                   dbo.Table_010_SaleFactor.Column30 AS SpecialGroup,
                                   0.000 AS SpecialGroup2,
                                   dbo.Table_010_SaleFactor.Column31 AS SpecialCustomer,
                                   0.000 AS SpecialCustomer2,
                                   dbo.Table_010_SaleFactor.Column32 AS Extra,
                                   0.000 AS Extra2,
                                   dbo.Table_010_SaleFactor.Column33 AS Reduction,
                                   0.000 AS Reduction2,
                                   dbo.Table_024_Discount.column01 AS ExReName,
                                   ISNULL(dbo.Table_012_Child2_SaleFactor.column03, 0.0) AS ExRePercent,
                                   ISNULL(dbo.Table_012_Child2_SaleFactor.column04, 0) AS ExRePrice,
                                   0.000 AS ExRePrice2,
                                   dbo.Table_012_Child2_SaleFactor.column05 AS ExRe,
                                   Table_010_SaleFactor.Column12 AS FactorType,
                                   Table_010_SaleFactor.Column40 AS CurType,
                                   Table_010_SaleFactor.Column41 AS CurValue
                            FROM   dbo.Table_012_Child2_SaleFactor
                                   INNER JOIN dbo.Table_024_Discount
                                        ON  dbo.Table_012_Child2_SaleFactor.column02 = dbo.Table_024_Discount.columnid
                                   RIGHT OUTER JOIN dbo.Table_010_SaleFactor
                                        ON  dbo.Table_012_Child2_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
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
                                   LEFT  JOIN {2}.dbo.Table_045_PersonInfo tpi
                                        ON  tpi.ColumnId = dbo.Table_010_SaleFactor.column05
                                   LEFT     JOIN {3}.dbo.Table_007_PwhrsDraft tpd
                                        ON  tpd.columnid = dbo.Table_010_SaleFactor.column09
                                   LEFT JOIN {4}.dbo.Table_060_SanadHead tsh
                                        ON  tsh.ColumnId = dbo.Table_010_SaleFactor.column10
                                   LEFT JOIN {2}.dbo.Table_035_ProjectInfo tpi2
                                        ON  tpi2.Column00 = dbo.Table_010_SaleFactor.Column44
                            WHERE  (dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND '{1}')
                            ORDER BY
                                   SaleNumber";
           
            CommandText = string.Format(CommandText, Date1, Date2,
                ConBase.Database, ConWare.Database,ConAcnt.Database);
            SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            Table.Columns["FactorNetPrice2"].Expression = "IIF(FactorType=1, FactorNetPrice* CurValue, FactorNetPrice)";
            Table.Columns["VolumeGroup2"].Expression = "IIF(FactorType=1, VolumeGroup* CurValue, VolumeGroup)";
            Table.Columns["SpecialGroup2"].Expression = "IIF(FactorType=1, SpecialGroup* CurValue, SpecialGroup)";
            Table.Columns["SpecialCustomer2"].Expression = "IIF(FactorType=1, SpecialCustomer* CurValue, SpecialCustomer)";
            Table.Columns["Extra2"].Expression = "IIF(FactorType=1, Extra* CurValue, Extra)";
            Table.Columns["Reduction2"].Expression = "IIF(FactorType=1, Reduction* CurValue, Reduction)";
            Table.Columns["ExRePrice2"].Expression = "IIF(FactorType=1, ExRePrice* CurValue, ExRePrice)";


            bindingSource2.DataSource = Table;

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

            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.CompReport_Factors = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
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
