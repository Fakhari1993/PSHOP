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
    public partial class Form15_CompReport_Goods : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConACNT = new SqlConnection(Properties.Settings.Default.ACNT);

        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1 = null, Date2 = null;

        public Form15_CompReport_Goods()
        {
            InitializeComponent();
        }

        private void Form14_CustomerGoods_Load(object sender, EventArgs e)
        {
            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            string[] Dates = Properties.Settings.Default.CompReport_Goods.Split('-');
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
            gridEX_Goods.DropDowns["MainGroup"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column02 from table_002_MainGroup"), "");
            gridEX_Goods.DropDowns["SubGroup"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column03 from table_003_SubsidiaryGroup"), "");
            gridEX_Goods.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_002_SalesTypes"), "");
            gridEX_Goods.DropDowns["Currency"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_055_CurrencyInfo"), "");

            gridEX_Goods.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_007_PwhrsDraft"), "");
            gridEX_Goods.DropDowns["Order"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_005_OrderHeader"), "");
            gridEX_Goods.DropDowns["Sanad"].SetDataBinding(clDoc.ReturnTable(ConACNT.ConnectionString, "Select * from Table_060_SanadHead"), "");

            gridEX_Goods.DropDowns["Project"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_035_ProjectInfo"), "");


            cmb_Cancel.SelectedIndex = 0;


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


                if (cmb_Cancel.ComboBox.SelectedIndex == 0)
                {
                    CommandText = @"

                                        SELECT   N'فاکتور فروش' AS [type],
                                               dbo.Table_010_SaleFactor.column03 AS CustomerId,
                                               PersonTable.Column01 AS CustomerCode,
                                               PersonTable.Column02 AS CustomerName,
                                               dbo.Table_010_SaleFactor.column05 AS Visitor,
                                               dbo.Table_011_Child1_SaleFactor.column02 AS GoodId,
                                               GoodTable.column01 AS GoodCode,
                                               GoodTable.column02 AS GoodName,
                                               CountInfo.Column01 AS CountUnit,
                                               dbo.Table_011_Child1_SaleFactor.column04 AS BoxNumber,
                                               dbo.Table_011_Child1_SaleFactor.column05 AS PackNumber,
                                               dbo.Table_011_Child1_SaleFactor.column06 AS DetailNumber,
                                               dbo.Table_011_Child1_SaleFactor.column07 AS TotalNumber,
                                               dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice,
                                               0.000 AS TotalPrice2,
                                               dbo.Table_011_Child1_SaleFactor.column16 AS DiscountPercent,
                                               dbo.Table_011_Child1_SaleFactor.column17 AS TotalDiscount,
                                               0.000 AS TotalDiscount2,
                                               dbo.Table_011_Child1_SaleFactor.column18 AS ExtraPercent,
                                               dbo.Table_011_Child1_SaleFactor.column19 AS TotalExtra,
                                               0.000 AS TotalExtra2,
                                               dbo.Table_011_Child1_SaleFactor.column20 AS NetPrice,
                                               0.000 AS NetPrice2,
                                               PersonTable.Column21 AS Province,
                                               PersonTable.Column22 AS City,
                                               PersonTable.Column29 AS STATE,
                                               GoodTable.column03 AS MainGroup,
                                               GoodTable.column04 AS SubGroup,
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
                                               dbo.Table_011_Child1_SaleFactor.column30 AS Gift,
                                               Table_010_SaleFactor.Column12 AS FactorType,
                                               Table_010_SaleFactor.Column40 AS CurType,
                                               Table_010_SaleFactor.Column41 AS CurValue,
                                               Table_011_Child1_SaleFactor.Column34 AS BuildSeri,
                                               Table_011_Child1_SaleFactor.Column35 AS ExpDate,
                                               Table_011_Child1_SaleFactor.Column36 AS SingleWeight,
                                               Table_011_Child1_SaleFactor.Column37 AS TotalWeight,
                                               Table_011_Child1_SaleFactor.column23 AS Discription,
                                               Table_010_SaleFactor.Column13 AS [user],
                                                cast(isnull(dbo.Table_011_Child1_SaleFactor.column11/nullif(dbo.Table_011_Child1_SaleFactor.column07,0),0) as decimal(18,3)) as Avarge 
                                                ,ppg.Column02 as Project,
                                               Table_010_SaleFactor.Column08 AS [OrderNum],
                                               Table_010_SaleFactor.Column09 AS [DraftNum],dbo.Table_011_Child1_SaleFactor.column10 as saleprice,
                                                Table_011_Child1_SaleFactor.Column38 as Brand,
                                                Table_011_Child1_SaleFactor.Column39 as Suplyer,
                                                dbo.Table_010_SaleFactor.column10 as Sanad,
                                                dbo.Table_010_SaleFactor.column06 as FactorDesc,
                                                dbo.Table_010_SaleFactor.Column44 AS FatorProject,isnull(dbo.Table_010_SaleFactor.column24,0) as Etebar,
                                                 tep.column01 as ExitNumber
                                        FROM   dbo.Table_010_SaleFactor
                                               INNER JOIN dbo.Table_011_Child1_SaleFactor
                                                    ON  dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                                               left JOIN " + ConWare.Database+ @".dbo.Table_007_PwhrsDraft tpd
                                                    ON  tpd.columnid = dbo.Table_010_SaleFactor.column09
                                              left JOIN "+ConWare.Database+ @".dbo.Table_009_ExitPwhrs tep
                                                    ON  tep.columnid = tpd.column15     
                                               left join " + ConBase.Database + @".dbo.Table_035_ProjectInfo ppg on ppg.Column00=dbo.Table_011_Child1_SaleFactor.column22
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
                                                   dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND '{1}'
                                                   AND dbo.Table_010_SaleFactor.column17 = 0
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
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column04 AS BoxNumber,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column05 AS PackNumber,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column06 AS DetailNumber,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column07 AS TotalNumber,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column11 AS TotalPrice,
                                               0.000 AS TotalPrice2,
                                               (-1) * Table_019_Child1_MarjooiSale.column16 AS DiscountPercent,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column17 AS TotalDiscount,
                                               0.000 AS TotalDiscount2,
                                               dbo.Table_019_Child1_MarjooiSale.column18 AS ExtraPercent,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column19 AS TotalExtra,
                                               0.000 AS TotalExtra2,
                                               (-1) * dbo.Table_019_Child1_MarjooiSale.column20 AS NetPrice,
                                               0.000 AS NetPrice2,
                                               PersonTable.Column21 AS Province,
                                               PersonTable.Column22 AS City,
                                               PersonTable.Column29 AS STATE,
                                               GoodTable.column03 AS MainGroup,
                                               GoodTable.column04 AS SubGroup,
                                               NULL AS SaleType,
                                               dbo.Table_018_MarjooiSale.columnid AS SaleID,
                                               dbo.Table_018_MarjooiSale.column01 AS SaleNumber,
                                               dbo.Table_018_MarjooiSale.column02 AS SaleDate,
                                               (-1) * (
                                                   dbo.Table_018_MarjooiSale.Column18 - dbo.Table_018_MarjooiSale.Column20 
                                                   +
                                                   dbo.Table_018_MarjooiSale.Column19  
                                               ) AS FactorNetPrice,
                                               0.000 AS FactorNetPrice2,
                                               0.000 AS VolumeGroup,
                                               0.000 AS VolumeGroup2,
                                               0.000 AS SpecialGroup,
                                               0.000 AS SpecialGroup2,
                                               0.000 AS SpecialCustomer,
                                               0.000 AS SpecialCustomer2,
                                               dbo.Table_018_MarjooiSale.Column19 AS Extra,
                                               0.000 AS Extra2,
                                               dbo.Table_018_MarjooiSale.Column20 AS Reduction,
                                               0.000 AS Reduction2,
                                               CAST(0 AS BIT) AS Gift,
                                               Table_018_MarjooiSale.Column12 AS FactorType,
                                               Table_018_MarjooiSale.Column23 AS CurType,
                                               Table_018_MarjooiSale.Column24 AS CurValue,
                                               Table_019_Child1_MarjooiSale.Column32 AS BuildSeri,
                                               Table_019_Child1_MarjooiSale.Column33 AS ExpDate,
                                               Table_019_Child1_MarjooiSale.Column34 AS SingleWeight,
                                               (-1) * Table_019_Child1_MarjooiSale.Column35 AS TotalWeight,
                                               Table_019_Child1_MarjooiSale.Column23 AS Discription,
                                               dbo.Table_018_MarjooiSale.Column13 AS [user],
                                         cast(isnull(dbo.Table_019_Child1_MarjooiSale.column11/nullif(dbo.Table_019_Child1_MarjooiSale.column07,0),0) as decimal(18,3)) as Avarge
                                                ,ppg.Column02 as Project,
                                               Table_018_MarjooiSale.Column08 AS OrderNum,
                                              null AS DrfatNum,(-1) * dbo.Table_019_Child1_MarjooiSale.column10 as saleprice,
                                                 Table_019_Child1_MarjooiSale.Column36 as Brand,
                                                 Table_019_Child1_MarjooiSale.Column37 as Suplyer,Table_018_MarjooiSale.Column10 as Sanad,dbo.Table_018_MarjooiSale.column06 as FactorDesc,
                                        '' AS FatorProject,0 as Etebar,'' as ExitNumber


                                        FROM   dbo.Table_018_MarjooiSale 
                                               INNER JOIN dbo.Table_019_Child1_MarjooiSale
                                                    ON  dbo.Table_018_MarjooiSale.columnid = dbo.Table_019_Child1_MarjooiSale.column01
                                                left join " + ConBase.Database + @".dbo.Table_035_ProjectInfo ppg on ppg.Column00=dbo.Table_019_Child1_MarjooiSale.column22
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
                                        WHERE  (dbo.Table_018_MarjooiSale.column02 BETWEEN '{0}' AND '{1}')";
                }
                else

                    CommandText = @"SELECT     TOP (100) PERCENT N'فاکتور فروش' AS [type],dbo.Table_010_SaleFactor.column03 AS CustomerId,
                PersonTable.Column01 AS CustomerCode, PersonTable.Column02 AS CustomerName, 
                dbo.Table_010_SaleFactor.column05 AS Visitor, 
                dbo.Table_011_Child1_SaleFactor.column02 AS GoodId, GoodTable.column01 AS GoodCode, GoodTable.column02 AS GoodName, CountInfo.Column01 AS CountUnit, 
                dbo.Table_011_Child1_SaleFactor.column04 AS BoxNumber,  dbo.Table_011_Child1_SaleFactor.column05 AS PackNumber, 
                dbo.Table_011_Child1_SaleFactor.column06 AS DetailNumber,  dbo.Table_011_Child1_SaleFactor.column07 AS TotalNumber, 
                dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice,0.000 as TotalPrice2,  dbo.Table_011_Child1_SaleFactor.column16 as DiscountPercent,
                dbo.Table_011_Child1_SaleFactor.column17 AS TotalDiscount, 0.000 as TotalDiscount2,dbo.Table_011_Child1_SaleFactor.column18 as ExtraPercent,
                dbo.Table_011_Child1_SaleFactor.column19 AS TotalExtra,0.000 as TotalExtra2, dbo.Table_011_Child1_SaleFactor.column20 AS NetPrice, 0.000 as NetPrice2,
                PersonTable.Column21 AS Province, PersonTable.Column22 AS City, PersonTable.Column29 AS State, GoodTable.column03 AS MainGroup, 
                GoodTable.column04 AS SubGroup, dbo.Table_010_SaleFactor.Column36 AS SaleType, dbo.Table_010_SaleFactor.columnid AS SaleID, 
                dbo.Table_010_SaleFactor.column01 AS SaleNumber, dbo.Table_010_SaleFactor.column02 AS SaleDate, 
                dbo.Table_010_SaleFactor.Column28 - dbo.Table_010_SaleFactor.Column29 - dbo.Table_010_SaleFactor.Column30 - dbo.Table_010_SaleFactor.Column31 + dbo.Table_010_SaleFactor.Column32
                - dbo.Table_010_SaleFactor.Column33 AS FactorNetPrice,0.000 as FactorNetPrice2, dbo.Table_010_SaleFactor.Column29 AS VolumeGroup, 0.000 as VolumeGroup2,
                dbo.Table_010_SaleFactor.Column30 AS SpecialGroup,0.000 as SpecialGroup2, dbo.Table_010_SaleFactor.Column31 AS SpecialCustomer,0.000 as SpecialCustomer2,
                dbo.Table_010_SaleFactor.Column32 AS Extra, 0.000 as Extra2,dbo.Table_010_SaleFactor.Column33 AS Reduction,0.000 as Reduction2, dbo.Table_011_Child1_SaleFactor.column30 AS Gift,
                Table_010_SaleFactor.Column12 as FactorType, Table_010_SaleFactor.Column40 as CurType,Table_010_SaleFactor.Column41 as CurValue,
                Table_011_Child1_SaleFactor.Column34 as BuildSeri,Table_011_Child1_SaleFactor.Column35 as ExpDate,Table_011_Child1_SaleFactor.Column36 as SingleWeight,Table_011_Child1_SaleFactor.Column37 as TotalWeight,
                   cast( isnull(dbo.Table_011_Child1_SaleFactor.column11/nullif(dbo.Table_011_Child1_SaleFactor.column07,0),0) as decimal(18,3))as Avarge,ppg.Column02 as Project
               ,Table_011_Child1_SaleFactor.column23 as Discription, Table_010_SaleFactor.Column13 AS [user],  Table_010_SaleFactor.Column08 AS [OrderNum],
                Table_010_SaleFactor.Column09 AS [DraftNum],dbo.Table_011_Child1_SaleFactor.column10 as saleprice,   Table_011_Child1_SaleFactor.Column38 as Brand,
                Table_011_Child1_SaleFactor.Column39 as Suplyer,dbo.Table_010_SaleFactor.column10 as Sanad,dbo.Table_010_SaleFactor.column06 as FactorDesc, dbo.Table_010_SaleFactor.Column44 AS FatorProject,isnull(dbo.Table_010_SaleFactor.column24,0) as Etebar, tep.column01 as ExitNumber
                FROM         dbo.Table_010_SaleFactor INNER JOIN dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                 left JOIN " + ConWare.Database + @".dbo.Table_007_PwhrsDraft tpd
                                                    ON  tpd.columnid = dbo.Table_010_SaleFactor.column09
                                              left JOIN " + ConWare.Database + @".dbo.Table_009_ExitPwhrs tep
                                                    ON  tep.columnid = tpd.column15     
                left join " + ConBase.Database + @".dbo.Table_035_ProjectInfo ppg on ppg.Column00=dbo.Table_011_Child1_SaleFactor.column22
                INNER JOIN  (SELECT     columnid, column01, column02, column03, column04 FROM         {3}.dbo.table_004_CommodityAndIngredients) AS GoodTable ON 
                dbo.Table_011_Child1_SaleFactor.column02 = GoodTable.columnid INNER JOIN (SELECT     Column00, Column01  FROM         {2}.dbo.Table_070_CountUnitInfo) AS CountInfo ON dbo.Table_011_Child1_SaleFactor.column03 = CountInfo.Column00 LEFT OUTER JOIN
                (SELECT     ColumnId, Column01, Column02, Column21, Column22, Column29  FROM         {2}.dbo.Table_045_PersonInfo) AS PersonTable ON dbo.Table_010_SaleFactor.column03 = PersonTable.ColumnId
                WHERE     (dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND '{1}' )  AND dbo.Table_010_SaleFactor.column17 = 0   AND dbo.Table_010_SaleFactor.column19 = 0
                ORDER BY SaleNumber";


                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database, ConWare.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                try
                {
                    Adapter.Fill(Table);
                }
                catch
                {
                }
                Table.Columns["VolumeGroup2"].Expression = "IIF(FactorType=1, VolumeGroup* CurValue, VolumeGroup)";
                Table.Columns["SpecialGroup2"].Expression = "IIF(FactorType=1, SpecialGroup* CurValue, SpecialGroup)";
                Table.Columns["SpecialCustomer2"].Expression = "IIF(FactorType=1, SpecialCustomer* CurValue,SpecialCustomer )";
                Table.Columns["Reduction2"].Expression = "IIF(FactorType=1, Reduction* CurValue,Reduction )";
                Table.Columns["Extra2"].Expression = "IIF(FactorType=1, Extra* CurValue, Extra)";
                Table.Columns["FactorNetPrice2"].Expression = "IIF(FactorType=1, FactorNetPrice* CurValue,FactorNetPrice )";
                Table.Columns["TotalPrice2"].Expression = "IIF(FactorType=1, TotalPrice* CurValue,TotalPrice )";
                Table.Columns["TotalDiscount2"].Expression = "IIF(FactorType=1, TotalDiscount* CurValue,TotalDiscount )";
                Table.Columns["TotalExtra2"].Expression = "IIF(FactorType=1, TotalExtra* CurValue,TotalExtra )";
                Table.Columns["NetPrice2"].Expression = "IIF(FactorType=1, NetPrice* CurValue,NetPrice )";

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
                        if (gridEX_Goods.CurrentRow.Cells["type"].Text == "فاکتور فروش")
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
                        {
                            foreach (Form item in Application.OpenForms)
                            {
                                if (item.Name == "Frm_013_ReturnFactor")
                                {
                                    item.BringToFront();
                                    _05_Sale.Frm_013_ReturnFactor frm = (_05_Sale.Frm_013_ReturnFactor)item;
                                    frm.txt_Search.Text = gridEX_Goods.GetRow().Cells["SaleNumber"].Text;
                                    frm.bt_Search_Click(sender, e);
                                    return;
                                }
                            }
                            _05_Sale.Frm_013_ReturnFactor frms = new _05_Sale.Frm_013_ReturnFactor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 23),
                                Convert.ToInt32(gridEX_Goods.GetValue("SaleID").ToString()));
                            try
                            {
                                frms.MdiParent = MainForm.ActiveForm;
                            }
                            catch { }
                            frms.Show();
                        }

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
                Properties.Settings.Default.CompReport_Goods = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
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
