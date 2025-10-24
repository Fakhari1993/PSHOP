using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.Xml;
using System.Drawing.Printing;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Design;
using Stimulsoft.Database;

namespace PSHOP._05_Sale.Reports
{
    public partial class Form_SaleFactorTotalPrint : Form
    {
        Classes.Class_Settle cl_Settle = new Classes.Class_Settle();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        DataTable HeaderTable;
        DataTable DetailTable;
        DataTable Org;
        DataTable TotalSettleInfo;
        string[] Sign;
        short _PrintStyle = 1;
        List<string> List = new List<string>();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        int _SaleNumber;
        bool _BackSpace = false, _Agg = false, _ClubPrint;


        public Form_SaleFactorTotalPrint(int SaleNumber, bool ClubPrint)
        {
            InitializeComponent();
            _SaleNumber = SaleNumber;
            _ClubPrint = ClubPrint;
        }

        public Form_SaleFactorTotalPrint(List<string> _List, bool ClubPrint)
        {
            InitializeComponent();
            List = _List;
            _ClubPrint = ClubPrint;
        }

        public Form_SaleFactorTotalPrint(DataTable _HeaderTable, DataTable _DetailTable, bool ClubPrint)
        {
            InitializeComponent();
            HeaderTable = _HeaderTable;
            DetailTable = _DetailTable;
            _ClubPrint = ClubPrint;
            _Agg = true;
        }

        public void Form_FactorPrint_Load(object sender, EventArgs e)
        {
            try
            {



                faDatePicker1.SelectedDateTime = DateTime.Now.AddMonths(-1);
                faDatePicker2.SelectedDateTime = DateTime.Now;
                chk_ShowCustomerBill.Checked = Properties.Settings.Default.ShowCustomerBill;
                chk_ShowSen.Checked = Properties.Settings.Default.ShowSaleFactorSentence;
                chk_Logo.Checked = Properties.Settings.Default.DontShowLogo;
                _PrintStyle = Properties.Settings.Default.SaleFactorStyle;
                mlt_ACC.DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ACC_Code,ACC_Name from AllHeaders()");
                if (Properties.Settings.Default.SaveACCRemain.Trim() != "")
                    mlt_ACC.Value = Properties.Settings.Default.SaveACCRemain;

                bt_Display_Click(sender, e);
            }
            catch { }

        }

        private void Form_FactorPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.ShowSaleFactorSentence = chk_ShowSen.Checked;
            Properties.Settings.Default.ShowCustomerBill = chk_ShowCustomerBill.Checked;
            Properties.Settings.Default.SaleFactorStyle = _PrintStyle;
            Properties.Settings.Default.DontShowLogo = chk_Logo.Checked;
            if (mlt_ACC.Text.Trim() != "")
                Properties.Settings.Default.SaveACCRemain = mlt_ACC.Value.ToString();
            Properties.Settings.Default.Save();

            try
            {
                var Rpt = (CrystalDecisions.CrystalReports.Engine.ReportDocument)crystalReportViewer1.ReportSource;
                Rpt.Database.Dispose();
                Rpt.Close();
                Rpt.Dispose();
                GC.Collect();
            }
            catch
            {
            }

        }

        private void txt_From_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = (FarsiLibrary.Win.Controls.FADatePicker)sender;


                if (textBox.Text.Length == 4)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                else if (textBox.Text.Length == 7)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker1.HideDropDown();
                    faDatePicker2.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePicker2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker2.HideDropDown();
                    chk_ShowCustomerBill.Focus();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            if (!_Agg)
            {
                string HeaderSelectText = null;
                if (!_ClubPrint)
                {
                    HeaderSelectText = @"
SELECT FactorTable.FactorID AS ID,
       DetailId,
       FactorTable.Serial,
       FactorTable.LegalNumber,
       FactorTable.Date,
       FactorTable.Responsible,
       FactorTable.CustomerID,
       FactorTable.P2Name,
       FactorTable.P2NationalCode,
       FactorTable.P2ECode,
       FactorTable.P2SabtCode,
       FactorTable.P2Address,
       FactorTable.P2Tel,
       FactorTable.Mobile,
       FactorTable.P2Fax,
       FactorTable.P2PostalCode,
       FactorTable.P2Code,
       --CAST(FactorTable.maincode AS NVARCHAR) + CAST(FactorTable.subcode AS NVARCHAR) AS 
      -- GoodCode,
        CAST(FactorTable.subcode AS NVARCHAR) AS 
       GoodCode,
       --CAST(FactorTable.mainname AS NVARCHAR) + '-' + CAST(FactorTable.subname AS NVARCHAR) AS 
      -- GoodName,
          CAST(FactorTable.subname AS NVARCHAR) AS 
       GoodName,
       SUM(isnull(FactorTable.Box,0)) AS Box,
       SUM(isnull(FactorTable.BoxPrice,0)) AS BoxPrice,
        SUM(isnull(FactorTable.Pack,0)) AS Pack,
       SUM(isnull( FactorTable.PackPrice,0)) AS PackPrice,
       SUM(isnull(FactorTable.Number,0)) AS Number,
       SUM(isnull(FactorTable.TotalNumber,0)) AS TotalNumber,
       SUM(isnull(FactorTable.SinglePrice,0)) AS SinglePrice,
       SUM(isnull(FactorTable.TotalPrice,0)) AS TotalPrice ,
       SUM(isnull(FactorTable.DiscountPercent,0)) AS DiscountPercent,
       SUM(isnull(FactorTable.DiscountPrice,0)) AS DiscountPrice,
       SUM(isnull(FactorTable.TaxPrice,0)) AS TaxPrice,
       SUM(isnull(FactorTable.TotalWeight,0)) AS TotalWeight,
      SUM(isnull( FactorTable.TaxPercent,0)) AS TaxPercent,
        SUM(isnull(FactorTable.NetPrice,0)) AS NetPrice,
       ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
       ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
       PersonInfoTable.Column02,
       FactorTable.NetTotal,
       FactorTable.VolumeGroup,
       FactorTable.SpecialGroup,
       FactorTable.SpecialCustomer,
       FactorTable.Description,
       FactorTable.CountUnitName,
       derivedtbl_1.Groups,
       '-' AS charPrice,
       'SettleInfo' AS SettleInfo,
       FactorTable.FactorType,
       SUM( ISNULL(FactorTable.NumberInBox, 0)) AS NumberInBox,
       FactorTable.RowDesc,
       SUM( ISNULL(FactorTable.Zarib, 0)) AS Zarib,
       FactorTable.Series,
       FactorTable.ExpirationDate,
       SUM(ISNULL(FactorTable.NumberInBox , 0)) AS Expr1,
       SUM(ISNULL(FactorTable.NumberInPack, 0)) AS NumberInPack,
       CityTable.Column02 AS CityName,
       ProvinceTable.Column01 AS ProvinceName,
       FactorTable.PayCash,
       FactorTable.DraftNumber,
       FactorTable.DocID,
       FactorTable.Feild
FROM   (
           SELECT Column00,
                  Column01,
                  Column02,
                  Column03,
                  Column04,
                  Column05,
                  Column06
           FROM   {0}.dbo.Table_060_ProvinceInfo
       ) AS ProvinceTable
       INNER JOIN (
                SELECT Column00,
                       Column01,
                       Column02,
                       Column03,
                       Column04,
                       Column05,
                       Column06,
                       Column07,
                       Column08
                FROM   {0}.dbo.Table_065_CityInfo
            ) AS CityTable
            ON  ProvinceTable.Column00 = CityTable.Column00
       RIGHT OUTER JOIN (
                SELECT dbo.Table_010_SaleFactor.columnid AS FactorID,
                       dbo.Table_010_SaleFactor.columnid AS DetailId,
                       dbo.Table_010_SaleFactor.column01 AS Serial,
                       dbo.Table_010_SaleFactor.column37 AS LegalNumber,
                       dbo.Table_010_SaleFactor.column02 AS Date,
                       dbo.Table_010_SaleFactor.column03 AS CustomerID,
                       PersonTable.Column02 AS P2Name,
                       PersonTable.Column09 AS P2NationalCode,
                       PersonTable.Column141 AS P2ECode,
                       PersonTable.Column142 AS P2SabtCode,
                       PersonTable.Column01 AS P2Code,
                       PersonTable.Column06 AS P2Address,
                       PersonTable.Column07 AS P2Tel,
                       PersonTable.Column19 AS Mobile,
                       PersonTable.Column08 AS P2Fax,
                       PersonTable.Column13 AS P2PostalCode,
                       PersonTable.Column10 AS Feild,
                       GoodTable.column01 AS GoodCode,
                       dbo.Table_011_Child1_SaleFactor.column04 AS Box,
                       dbo.Table_011_Child1_SaleFactor.column08 AS BoxPrice,
                       dbo.Table_011_Child1_SaleFactor.column05 AS Pack,
                       dbo.Table_011_Child1_SaleFactor.column09 AS PackPrice,
                       dbo.Table_011_Child1_SaleFactor.column06 AS Number,
                       dbo.Table_011_Child1_SaleFactor.column07 AS TotalNumber,
                       dbo.Table_011_Child1_SaleFactor.column10 AS SinglePrice,
                       dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice,
                       dbo.Table_011_Child1_SaleFactor.column16 AS 
                       DiscountPercent,
                       dbo.Table_011_Child1_SaleFactor.column17 AS DiscountPrice,
                       dbo.Table_011_Child1_SaleFactor.column18 AS TaxPercent,
                       dbo.Table_011_Child1_SaleFactor.column19 AS TaxPrice,
                       dbo.Table_011_Child1_SaleFactor.column37 AS TotalWeight,
                       dbo.Table_011_Child1_SaleFactor.column20 AS NetPrice,
                       GoodTable.column02 AS GoodName,
                       OtherPrice.PlusPrice AS Ezafat,
                       OtherPrice.MinusPrice AS Kosoorat,
                       dbo.Table_010_SaleFactor.column05 AS Responsible,
                       dbo.Table_010_SaleFactor.Column28 AS NetTotal,
                       dbo.Table_010_SaleFactor.Column29 AS VolumeGroup,
                       dbo.Table_010_SaleFactor.Column30 AS SpecialGroup,
                       dbo.Table_010_SaleFactor.Column31 AS SpecialCustomer,
                       dbo.Table_010_SaleFactor.column06 AS DESCRIPTION,
                       CountUnitTable.Column01 AS CountUnitName,
                      -- main.code AS maincode,
                      -- main.name AS mainname,
                       sub.code AS subcode,
                       sub.name AS subname,
                       CASE 
                            WHEN Table_010_SaleFactor.Column12 = 0 THEN 
                                 '***فاکتور ريالي***'
                            ELSE '***فاکتور ارزي***'
                       END AS FactorType,
                       dbo.Table_011_Child1_SaleFactor.column23 AS RowDesc,
                       dbo.Table_011_Child1_SaleFactor.column31 AS NumberInBox,
                       dbo.Table_011_Child1_SaleFactor.column32 AS NumberInPack,
                       dbo.Table_011_Child1_SaleFactor.Column33 AS Zarib,
                       dbo.Table_011_Child1_SaleFactor.Column34 AS Series,
                       dbo.Table_011_Child1_SaleFactor.Column35 AS 
                       ExpirationDate,
                       PersonTable.Column21 AS ProvinceId,
                       PersonTable.Column22 AS CityId,
                       Table_010_SaleFactor.column21 AS PayCash,
                       (
                           SELECT Column01
                           FROM   " + ConWare.Database + @".dbo.Table_007_PwhrsDraft
                           WHERE  Columnid = Table_010_SaleFactor.Column09
                       ) AS DraftNumber,
                       Table_010_SaleFactor.Column10 AS DocId
                FROM   dbo.Table_010_SaleFactor
                       INNER JOIN dbo.Table_011_Child1_SaleFactor
                            ON  dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                       INNER JOIN (
                                SELECT ColumnId,
                                       Column00,
                                       Column01,
                                       Column02,
                                       Column03,
                                       Column04,
                                       Column05,
                                         ISNULL(
                                           (
                                               SELECT tpi.Column01
                                               FROM   {0}.dbo.Table_060_ProvinceInfo tpi
                                               WHERE  tpi.Column00 = {0}.dbo.Table_045_PersonInfo.Column21
                                           ),
                                           ' '
                                       ) + ' ' + ISNULL(
                                           (
                                               SELECT tpi.Column02
                                               FROM   {0}.dbo.Table_065_CityInfo tpi
                                               WHERE  tpi.Column01 = {0}.dbo.Table_045_PersonInfo.Column22
                                           ),
                                           ' '
                                       ) + ' ' + ISNULL({0}.dbo.Table_045_PersonInfo.Column06, ' ') AS Column06,
                                       Column07,
                                       Column08,
                                       Column09,
                                       Column10,
                                       Column11,
                                       Column12,
                                       Column13,
                                       Column21,
                                       Column22,
                                       Column19,
                                       Column141,
                                       Column142
                                FROM   {0}.dbo.Table_045_PersonInfo
                            ) AS PersonTable
                            ON  dbo.Table_010_SaleFactor.column03 = PersonTable.ColumnId
                       LEFT OUTER JOIN (
                                SELECT columnid,
                                       SUM(PlusPrice) AS PlusPrice,
                                       SUM(MinusPrice) AS MinusPrice
                                FROM   (
                                           SELECT Table_010_SaleFactor_2.columnid,
                                                  SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                  PlusPrice,
                                                  0 AS MinusPrice
                                           FROM   dbo.Table_012_Child2_SaleFactor
                                                  INNER JOIN dbo.Table_010_SaleFactor AS 
                                                       Table_010_SaleFactor_2
                                                       ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                           Table_010_SaleFactor_2.columnid
                                           WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                           GROUP BY
                                                  Table_010_SaleFactor_2.columnid,
                                                  dbo.Table_012_Child2_SaleFactor.column05
                                           UNION ALL
                                           SELECT Table_010_SaleFactor_1.columnid,
                                                  0 AS PlusPrice,
                                                  SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                  MinusPrice
                                           FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                  Table_012_Child2_SaleFactor_1
                                                  INNER JOIN dbo.Table_010_SaleFactor AS 
                                                       Table_010_SaleFactor_1
                                                       ON  
                                                           Table_012_Child2_SaleFactor_1.column01 = 
                                                           Table_010_SaleFactor_1.columnid
                                           WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                           GROUP BY
                                                  Table_010_SaleFactor_1.columnid,
                                                  Table_012_Child2_SaleFactor_1.column05
                                       ) AS OtherPrice_1
                                GROUP BY
                                       columnid
                            ) AS OtherPrice
                            ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid
                       LEFT OUTER JOIN (
                                SELECT columnid,
                                       column01,
                                       column02,
                                       column03,
                                       column04,
                                       column05,
                                       column06,
                                       column07,
                                       column08,
                                       column09,
                                       column10,
                                       column11,
                                       column12,
                                       column13,
                                       column14,
                                       column15,
                                       column16,
                                       column17,
                                       column18,
                                       column19,
                                       column20,
                                       column21,
                                       column22,
                                       column23,
                                       column24,
                                       column25,
                                       column26,
                                       column27,
                                       column28,
                                       column29,
                                       column30,
                                       column31
                                FROM   {1}.dbo.table_004_CommodityAndIngredients
                            ) AS GoodTable
                            ON  dbo.Table_011_Child1_SaleFactor.column02 = 
                                GoodTable.columnid
                       LEFT OUTER JOIN (
                                SELECT Column00,
                                       Column01
                                FROM   {0}.dbo.Table_070_CountUnitInfo
                            ) AS CountUnitTable
                            ON  dbo.Table_011_Child1_SaleFactor.column03 = 
                                CountUnitTable.Column00
                       --LEFT OUTER JOIN (
                                --SELECT columnid,
                                       --column01 AS code,
                                     --  column02 AS NAME
                              --  FROM   {1}.dbo.table_002_MainGroup
                           -- ) AS main
                            --ON  main.columnid=GoodTable.column03
                            
                             LEFT OUTER JOIN (
                                SELECT columnid,
                                       column02 AS code,
                                       column03 AS NAME
                                FROM   {1}.dbo.table_003_SubsidiaryGroup
                            ) AS sub
                            ON  sub.columnid=GoodTable.column04
            ) AS FactorTable
            ON  CityTable.Column01 = FactorTable.CityId
       LEFT OUTER JOIN (
                SELECT PersonId,
                       Groups
                FROM   {0}.dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1
            ) AS derivedtbl_1
            ON  FactorTable.CustomerID = derivedtbl_1.PersonId
       LEFT OUTER JOIN (
                SELECT ColumnId,
                       Column01,
                       Column02,
                       Column21,
                       Column22
                FROM   {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1
            ) AS PersonInfoTable
            ON  FactorTable.Responsible = PersonInfoTable.ColumnId
     ";
                }
                else
                {
                    HeaderSelectText = @"SELECT FactorTable.FactorID AS ID,
       DetailId,
       FactorTable.Serial,
       FactorTable.LegalNumber,
       FactorTable.Date,
       FactorTable.Responsible,
       FactorTable.CustomerID,
       FactorTable.P2Name,
       FactorTable.P2ECode,
       FactorTable.P2Address,
       FactorTable.P2Tel,
       FactorTable.P2Fax,
       FactorTable.P2PostalCode,
       FactorTable.P2Code,
      -- CAST(FactorTable.maincode AS NVARCHAR) + CAST(FactorTable.subcode AS NVARCHAR) AS 
       --GoodCode,
      -- CAST(FactorTable.mainname AS NVARCHAR) + '-' + CAST(FactorTable.subname AS NVARCHAR) AS 
      -- GoodName,
      CAST(FactorTable.subcode AS NVARCHAR) AS 
       GoodCode,
         CAST(FactorTable.subname AS NVARCHAR) AS 
       GoodName,
       SUM(ISNULL(FactorTable.Box, 0)) AS Box,
       SUM(ISNULL(FactorTable.BoxPrice, 0)) AS BoxPrice,
       SUM(ISNULL(FactorTable.Pack, 0)) AS Pack,
       SUM(ISNULL(FactorTable.PackPrice, 0)) AS PackPrice,
       SUM(ISNULL(FactorTable.Number, 0)) AS Number,
       SUM(ISNULL(FactorTable.TotalNumber, 0)) AS TotalNumber,
       SUM(ISNULL(FactorTable.SinglePrice, 0)) AS SinglePrice,
       SUM(ISNULL(FactorTable.TotalPrice, 0)) AS TotalPrice,
       SUM(ISNULL(FactorTable.DiscountPercent, 0)) AS DiscountPercent,
       SUM(ISNULL(FactorTable.DiscountPrice, 0)) AS DiscountPrice,
       SUM(ISNULL(FactorTable.TaxPrice, 0)) AS TaxPrice,
       SUM(ISNULL(FactorTable.TotalWeight, 0)) AS TotalWeight,
       SUM(ISNULL(FactorTable.TaxPercent, 0)) AS TaxPercent,
       SUM(ISNULL(FactorTable.NetPrice, 0)) AS NetPrice,
       ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
       ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
       PersonInfoTable.Column02,
       FactorTable.NetTotal,
       FactorTable.VolumeGroup,
       FactorTable.SpecialGroup,
       FactorTable.SpecialCustomer,
       FactorTable.Description,
       FactorTable.CountUnitName,
       derivedtbl_1.Groups,
       '-' AS charPrice,
       'SettleInfo' AS SettleInfo,
       FactorTable.FactorType,
       SUM(ISNULL(FactorTable.NumberInBox, 0)) AS NumberInBox,
       FactorTable.RowDesc,
       SUM(ISNULL(FactorTable.Zarib, 0)) AS Zarib,
       FactorTable.Series,
       FactorTable.ExpirationDate,
       SUM(ISNULL(FactorTable.NumberInBox, 0)) AS Expr1,
       SUM(ISNULL(FactorTable.NumberInPack, 0)) AS NumberInPack,
       FactorTable.PayCash
FROM   (
           SELECT PersonId,
                  Groups
           FROM   {0}.dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1
       ) AS derivedtbl_1
       RIGHT OUTER JOIN (
                SELECT dbo.Table_010_SaleFactor.columnid AS FactorID,
                       dbo.Table_010_SaleFactor.columnid AS DetailId,
                       dbo.Table_010_SaleFactor.column01 AS Serial,
                       dbo.Table_010_SaleFactor.column37 AS LegalNumber,
                       dbo.Table_010_SaleFactor.column02 AS Date,
                       dbo.Table_010_SaleFactor.column18 AS CustomerID,
                       PersonTable.Column03 + N' ' + PersonTable.Column02 AS 
                       P2Name,
                       PersonTable.Column07 AS P2ECode,
                       PersonTable.Column01 AS P2Code,
                       PersonTable.Column08 AS P2Address,
                       PersonTable.Column04 AS P2Tel,
                       PersonTable.Column05 AS P2Fax,
                       PersonTable.Column09 AS P2PostalCode,
                       GoodTable.column01 AS GoodCode,
                       dbo.Table_011_Child1_SaleFactor.column04 AS Box,
                       dbo.Table_011_Child1_SaleFactor.column08 AS BoxPrice,
                       dbo.Table_011_Child1_SaleFactor.column05 AS Pack,
                       dbo.Table_011_Child1_SaleFactor.column09 AS PackPrice,
                       dbo.Table_011_Child1_SaleFactor.column06 AS Number,
                       dbo.Table_011_Child1_SaleFactor.column07 AS TotalNumber,
                       dbo.Table_011_Child1_SaleFactor.column10 AS SinglePrice,
                       dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice,
                       dbo.Table_011_Child1_SaleFactor.column16 AS 
                       DiscountPercent,
                       dbo.Table_011_Child1_SaleFactor.column17 AS DiscountPrice,
                       dbo.Table_011_Child1_SaleFactor.column18 AS TaxPercent,
                       dbo.Table_011_Child1_SaleFactor.column19 AS TaxPrice,
                       Table_011_Child1_SaleFactor.Column37 AS TotalWeight,
                       dbo.Table_011_Child1_SaleFactor.column20 AS NetPrice,
                       GoodTable.column02 AS GoodName,
                       OtherPrice.PlusPrice AS Ezafat,
                       OtherPrice.MinusPrice AS Kosoorat,
                       dbo.Table_010_SaleFactor.column05 AS Responsible,
                       dbo.Table_010_SaleFactor.Column28 AS NetTotal,
                       dbo.Table_010_SaleFactor.Column29 AS VolumeGroup,
                       dbo.Table_010_SaleFactor.Column30 AS SpecialGroup,
                       dbo.Table_010_SaleFactor.Column31 AS SpecialCustomer,
                       dbo.Table_010_SaleFactor.column06 AS DESCRIPTION,
                       CountUnitTable.Column01 AS CountUnitName,
                      -- main.code AS maincode,
                     --  main.name AS mainname,
                       sub.code AS subcode,
                       sub.name AS subname,
                       CASE 
                            WHEN Table_010_SaleFactor.Column12 = 0 THEN 
                                 '***فاکتور ريالي***'
                            ELSE '***فاکتور ارزي***'
                       END AS FactorType,
                       dbo.Table_011_Child1_SaleFactor.column23 AS RowDesc,
                       dbo.Table_011_Child1_SaleFactor.column31 AS NumberInBox,
                       dbo.Table_011_Child1_SaleFactor.column32 AS NumberInPack,
                       dbo.Table_011_Child1_SaleFactor.Column33 AS Zarib,
                       dbo.Table_011_Child1_SaleFactor.Column34 AS Series,
                       dbo.Table_011_Child1_SaleFactor.Column35 AS 
                       ExpirationDate,
                       dbo.Table_010_SaleFactor.column21 AS PayCash
                FROM   dbo.Table_010_SaleFactor
                       INNER JOIN dbo.Table_011_Child1_SaleFactor
                            ON  dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                       INNER JOIN (
                                SELECT ColumnId,
                                       Column01,
                                       Column02,
                                       Column03,
                                       Column04,
                                       Column05,
                                       Column06,
                                       Column07,
                                       Column08,
                                       Column09,
                                       Column10,
                                       Column11,
                                       Column12,
                                       Column13
                                FROM   {0}.dbo.Table_215_CustomerClub
                            ) AS PersonTable
                            ON  dbo.Table_010_SaleFactor.column18 = PersonTable.ColumnId
                       LEFT OUTER JOIN (
                                SELECT columnid,
                                       SUM(PlusPrice) AS PlusPrice,
                                       SUM(MinusPrice) AS MinusPrice
                                FROM   (
                                           SELECT Table_010_SaleFactor_2.columnid,
                                                  SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                  PlusPrice,
                                                  0 AS MinusPrice
                                           FROM   dbo.Table_012_Child2_SaleFactor
                                                  INNER JOIN dbo.Table_010_SaleFactor AS 
                                                       Table_010_SaleFactor_2
                                                       ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                           Table_010_SaleFactor_2.columnid
                                           WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                           GROUP BY
                                                  Table_010_SaleFactor_2.columnid,
                                                  dbo.Table_012_Child2_SaleFactor.column05
                                           UNION ALL
                                           SELECT Table_010_SaleFactor_1.columnid,
                                                  0 AS PlusPrice,
                                                  SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                  MinusPrice
                                           FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                  Table_012_Child2_SaleFactor_1
                                                  INNER JOIN dbo.Table_010_SaleFactor AS 
                                                       Table_010_SaleFactor_1
                                                       ON  
                                                           Table_012_Child2_SaleFactor_1.column01 = 
                                                           Table_010_SaleFactor_1.columnid
                                           WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                           GROUP BY
                                                  Table_010_SaleFactor_1.columnid,
                                                  Table_012_Child2_SaleFactor_1.column05
                                       ) AS OtherPrice_1
                                GROUP BY
                                       columnid
                            ) AS OtherPrice
                            ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid
                       LEFT OUTER JOIN (
                                SELECT columnid,
                                       column01,
                                       column02,
                                       column03,
                                       column04,
                                       column05,
                                       column06,
                                       column07,
                                       column08,
                                       column09,
                                       column10,
                                       column11,
                                       column12,
                                       column13,
                                       column14,
                                       column15,
                                       column16,
                                       column17,
                                       column18,
                                       column19,
                                       column20,
                                       column21,
                                       column22,
                                       column23,
                                       column24,
                                       column25,
                                       column26,
                                       column27,
                                       column28,
                                       column29,
                                       column30,
                                       column31
                                FROM   {1}.dbo.table_004_CommodityAndIngredients
                            ) AS GoodTable
                            ON  dbo.Table_011_Child1_SaleFactor.column02 = 
                                GoodTable.columnid
                       LEFT OUTER JOIN (
                                SELECT Column00,
                                       Column01
                                FROM   {0}.dbo.Table_070_CountUnitInfo
                            ) AS CountUnitTable
                            ON  dbo.Table_011_Child1_SaleFactor.column03 = 
                                CountUnitTable.Column00
                     --  LEFT OUTER JOIN (
                              --  SELECT columnid,
                                   ---    column01 AS code,
                                      -- column02 AS NAME
                             --   FROM   {1}.dbo.table_002_MainGroup
                          --  ) AS main
                          --  ON  main.columnid = GoodTable.column03
                       LEFT OUTER JOIN (
                                SELECT columnid,
                                       column02 AS code,
                                       column03 AS NAME
                                FROM   {1}.dbo.table_003_SubsidiaryGroup
                            ) AS sub
                            ON  sub.columnid = GoodTable.column04
            ) AS FactorTable
            ON  derivedtbl_1.PersonId = FactorTable.CustomerID
       LEFT OUTER JOIN (
                SELECT ColumnId,
                       Column01,
                       Column02,
                       Column21,
                       Column22
                FROM   {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1
            ) AS PersonInfoTable
            ON  FactorTable.Responsible = PersonInfoTable.ColumnId
";
                }
                HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);

                string DetailSelectText = @"SELECT     dbo.Table_024_Discount.column01 AS Name, dbo.Table_012_Child2_SaleFactor.column04 AS Price, 
                      CASE WHEN Table_024_Discount.column02 = 0 THEN '+' ELSE '-' END AS Type, dbo.Table_012_Child2_SaleFactor.column01 AS Column01 ,
                      dbo.Table_010_SaleFactor.column01 AS HeaderNum, dbo.Table_010_SaleFactor.column02 AS HeaderDate
                      FROM         dbo.Table_012_Child2_SaleFactor INNER JOIN
                      dbo.Table_010_SaleFactor ON dbo.Table_012_Child2_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid LEFT OUTER JOIN
                      dbo.Table_024_Discount ON dbo.Table_012_Child2_SaleFactor.column02 = dbo.Table_024_Discount.columnid";

                if (List.Count > 0)
                {
                    HeaderSelectText += " WHERE     FactorTable.FactorID IN(" + string.Join(",", List.ToArray()) + ")   ";
                    DetailSelectText += " WHERE     dbo.Table_012_Child2_SaleFactor.column01 IN (" + string.Join(",", List.ToArray()) + ")";

                }
                else if (rdb_CurrentNumber.Checked)
                {
                    HeaderSelectText += " WHERE     (FactorTable.Serial = " + _SaleNumber + ")  ";
                    DetailSelectText += " WHERE (Table_010_SaleFactor.Column01= " + _SaleNumber + ")";
                }
                else if (rdb_FromNumber.Checked && txt_From.Text.Trim() != "" && txt_To.Text.Trim() != "")
                {
                    HeaderSelectText += " WHERE     (FactorTable.Serial between  " + txt_From.Text + " and " + txt_To.Text + ")  ";
                    DetailSelectText += @" WHERE (Table_010_SaleFactor.Column01 between " + txt_From.Text + " and " + txt_To.Text + ")";
                }
                else if (rdb_Date.Checked && faDatePicker1.SelectedDateTime.HasValue && faDatePicker2.SelectedDateTime.HasValue
                    && faDatePicker1.Text.Length == 10 && faDatePicker2.Text.Length == 10)
                {
                    HeaderSelectText += " WHERE     (FactorTable.Date between  '" + faDatePicker1.Text + "' and '" + faDatePicker2.Text + "') ";
                    DetailSelectText += @" WHERE (Table_010_SaleFactor.Column02 between '" + faDatePicker1.Text + "' and '" + faDatePicker2.Text + "')";
                }
                if (!_ClubPrint)
                {
                    HeaderSelectText += @" GROUP BY
       --FactorTable.maincode,
       FactorTable.subcode,
       FactorTable.FactorID,
       FactorTable.DetailId,
       FactorTable.Serial,
       FactorTable.LegalNumber,
       FactorTable.Date,
       FactorTable.Responsible,
       FactorTable.CustomerID,
       FactorTable.P2Name,
      FactorTable.P2NationalCode,
       FactorTable.P2ECode,
       FactorTable.P2SabtCode,
       FactorTable.P2Address,
       FactorTable.P2Address,
       FactorTable.P2Tel,
       FactorTable.Mobile,
       FactorTable.P2Fax,
       FactorTable.P2PostalCode,
       FactorTable.P2Code,
       --CAST(FactorTable.mainname AS NVARCHAR) + '-' + CAST(FactorTable.subname AS NVARCHAR)   ,
             CAST(FactorTable.subname AS NVARCHAR)   ,
       ISNULL(FactorTable.Ezafat, 0),
       ISNULL(FactorTable.Kosoorat, 0),
       PersonInfoTable.Column02,
       FactorTable.NetTotal,
       FactorTable.VolumeGroup,
       FactorTable.SpecialGroup,
       FactorTable.SpecialCustomer,
       FactorTable.[DESCRIPTION],
       FactorTable.CountUnitName,
       derivedtbl_1.Groups,
       FactorTable.FactorType,
       FactorTable.RowDesc,
       FactorTable.Series,
       FactorTable.ExpirationDate,
       CityTable.Column02,
       ProvinceTable.Column01,
       FactorTable.PayCash,
       FactorTable.DraftNumber,
       FactorTable.DocId,
       FactorTable.Feild";
                }
                else
                {

                    HeaderSelectText += @"GROUP BY
                                --FactorTable.maincode,
                               FactorTable.subcode,
                               FactorTable.FactorID,
                               FactorTable.DetailId,
                               FactorTable.Serial,
                               FactorTable.LegalNumber,
                               FactorTable.Date,
                               FactorTable.Responsible,
                               FactorTable.CustomerID,
                               FactorTable.P2Name,
                               FactorTable.P2ECode,
                               FactorTable.P2Address,
                               FactorTable.P2Tel,
                               FactorTable.P2Fax,
                               FactorTable.P2PostalCode,
                               FactorTable.P2Code,
                              -- CAST(FactorTable.mainname AS NVARCHAR) + '-' + CAST(FactorTable.subname AS NVARCHAR),
                                   CAST(FactorTable.subname AS NVARCHAR),
                               ISNULL(FactorTable.Ezafat, 0),
                               ISNULL(FactorTable.Kosoorat, 0),
                               PersonInfoTable.Column02,
                               FactorTable.NetTotal,
                               FactorTable.VolumeGroup,
                               FactorTable.SpecialGroup,
                               FactorTable.SpecialCustomer,
                               FactorTable.[DESCRIPTION],
                               FactorTable.CountUnitName,
                               derivedtbl_1.Groups,
                               FactorTable.FactorType,
                               FactorTable.RowDesc,
                               FactorTable.Series,
                               FactorTable.ExpirationDate,
                               FactorTable.PayCash";
                }
                HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
                DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);
            }

            foreach (DataRow item in HeaderTable.Rows)
            {
                double FinalPrice = Convert.ToDouble(item["NetTotal"].ToString()) +
                            Convert.ToDouble(item["Ezafat"].ToString()) - Convert.ToDouble(item["Kosoorat"].ToString()) -
                            Convert.ToDouble(item["SpecialGroup"].ToString()) - Convert.ToDouble(item["SpecialCustomer"].ToString())
                            - Convert.ToDouble(item["VolumeGroup"].ToString());
                item["CharPrice"] = FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(FinalPrice)) + " ریال";
            }




            //فراخوانی عناوین امضا
            Sign = clDoc.Signature(10);


            //جدول اطلاعات تسویه فاکتورها
            DataTable CustomerTable = HeaderTable.DefaultView.ToTable(true, new string[] { "CustomerId", "ID", "Date", "DocId" });
            //DataTable CustomerSettleInfoTbl = new DataTable();
            TotalSettleInfo = dataSet_Sale21.FN_01_SettleInfo.Clone();
            //foreach (DataRow item in CustomerTable.Rows)
            //{
            //    SqlDataAdapter Adapter = new SqlDataAdapter("Select * from FN_01_SettleInfo(" + item["CustomerId"].ToString() + ")", ConSale);
            //    CustomerSettleInfoTbl = new DataTable();
            //    Adapter.Fill(CustomerSettleInfoTbl);
            //    TotalSettleInfo.Rows.Add(item["CustomerId"].ToString(),
            //        CustomerSettleInfoTbl.Rows[0]["Column00"].ToString(),
            //        (CustomerSettleInfoTbl.Rows[0]["Column01"].ToString().Trim()==""?"0":CustomerSettleInfoTbl.Rows[0]["Column01"].ToString()),
            //        (CustomerSettleInfoTbl.Rows[0]["Column02"].ToString().Trim() == "" ? "0" : CustomerSettleInfoTbl.Rows[0]["Column02"].ToString()),
            //        (CustomerSettleInfoTbl.Rows[0]["Column03"].ToString().Trim() == "" ? "0" : CustomerSettleInfoTbl.Rows[0]["Column03"].ToString()),
            //        (CustomerSettleInfoTbl.Rows[0]["Column04"].ToString().Trim()==""?"0":CustomerSettleInfoTbl.Rows[0]["Column04"].ToString()), item["ID"].ToString());

            //}
            try
            {
                if (chk_ShowCustomerBill.Checked && mlt_ACC.Text.Trim() != "")
                {
                    foreach (DataRow item in CustomerTable.Rows)
                    {
                        //SqlDataAdapter Adapter = new SqlDataAdapter("Select * from FN_01_SettleInfo(" + item["CustomerId"].ToString() + ")", ConSale);
                        //CustomerSettleInfoTbl = new DataTable();
                        //Adapter.Fill(CustomerSettleInfoTbl);
                        Int64 Remain = Convert.ToInt64(clDoc.ReturnTable(ConAcnt.ConnectionString, @"select ISNULL((
                    Select SUM(Table_065_SanadDetail.Column11)-SUM(Table_065_SanadDetail.Column12) from Table_065_SanadDetail inner join Table_060_SanadHead
                    ON Table_065_SanadDetail.Column00=Table_060_SanadHead.ColumnId
                    where Table_065_SanadDetail.Column07=" + item["CustomerId"].ToString() + @" and   Table_065_SanadDetail.Column01='" + mlt_ACC.Value.ToString() + @"' and
                    Table_060_SanadHead.Column01<='" + item["Date"].ToString() + @"'
                    group by Table_065_SanadDetail.Column07),0) as Remain").Rows[0][0].ToString());

                        String Sentence = null;
                        if (item["DocId"].ToString() != "0")
                            Sentence = "مانده حساب تا تاریخ فاکتور:  " + Remain.ToString("n0") + " ریال";
                        else Sentence = "مانده حساب بدون احتساب این فاکتور:   " + Remain.ToString("n0") + " ریال";
                        TotalSettleInfo.Rows.Add(item["CustomerId"].ToString(),
                            Sentence,
                            "0",
                            "0",
                            "0",
                            "0", item["ID"].ToString());

                    }
                }
                else if (chk_ShowCustomerBill.Checked && mlt_ACC.Text.Trim() == "")
                {

                    foreach (DataRow item in CustomerTable.Rows)
                    {

                        Int64 Remain = Convert.ToInt64(clDoc.ReturnTable(ConAcnt.ConnectionString, @"select ISNULL((
                    Select SUM(Table_065_SanadDetail.Column11)-SUM(Table_065_SanadDetail.Column12) from Table_065_SanadDetail inner join Table_060_SanadHead
                    ON Table_065_SanadDetail.Column00=Table_060_SanadHead.ColumnId
                    where Table_065_SanadDetail.Column07=" + item["CustomerId"].ToString() + @" and
                    Table_060_SanadHead.Column01<='" + item["Date"].ToString() + @"'
                    group by Table_065_SanadDetail.Column07),0) as Remain").Rows[0][0].ToString());

                        String Sentence = null;
                        if (item["DocId"].ToString() != "0")
                            Sentence = "مانده حساب تا تاریخ فاکتور:   " + Remain.ToString("n0") + " ریال";
                        else Sentence = "مانده حساب بدون احتساب این فاکتور:   " + Remain.ToString("n0") + " ریال";
                        TotalSettleInfo.Rows.Add(item["CustomerId"].ToString(),
                            Sentence,
                            "0",
                            "0",
                            "0",
                            "0", item["ID"].ToString());

                    }


                }
            }
            catch { }
            if (_PrintStyle == 26)
            {
                stiViewerControl1.Visible = true;
                crystalReportViewer1.Visible = false;
            }

            else
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
            }

            Org = Class_BasicOperation.LogoTable();

            switch (_PrintStyle)
            {
                case 1:
                    bt_First_Click(sender, e);
                    break;
                case 2: bt_Second_Click(sender, e);
                    break;
                case 3: bt_Third_Click(sender, e);
                    break;
                case 4: bt_4_Click(sender, e);
                    break;
                case 5: bt_5_Click(sender, e);
                    break;
                case 6: bt_6_Click(sender, e);
                    break;
                case 7: bt_7_Click(sender, e);
                    break;
                case 8: bt_8_Click(sender, e);
                    break;
                case 9: bt_Ninth_Click(sender, e);
                    break;
                case 10: bt_10_Click(sender, e);
                    break;
                case 11: bt_11_Click(sender, e);
                    break;
                case 12: bt_12_Click(sender, e);
                    break;
                case 13: bt_13_Click(sender, e);
                    break;
                case 14: bt_14_Click(sender, e);
                    break;
                case 15: _15_Click(sender, e);
                    break;
                case 16: bt_16_Click(sender, e);
                    break;
                case 17: bt_17_Click(sender, e);
                    break;
                case 18: bt_18_Click(sender, e);
                    break;
                case 19: bt_19_Click(sender, e);
                    break;
                case 20: bt_20_Click(sender, e);
                    break;
                case 21: bt_21_Click(sender, e);
                    break;
                case 22: bt_22_Click(sender, e);
                    break;
                case 23: bt_23_Click(sender, e);
                    break;
                case 24: bt_24_Click(sender, e);
                    break;
                case 25: bt_25_Click(sender, e);
                    break;
                case 26: btn_26_Click(sender, e);
                    StiOptions.Viewer.ViewerTitleText = "";
                    break;
            }

        }

        private void bt_First_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            //طرح رسمی
            this.Cursor = Cursors.WaitCursor;
            _05_Sale.Reports.Rpt_02_SaleFactor_01 Rpt2 = new Rpt_02_SaleFactor_01();

            if (!chk_Logo.Checked)
                Rpt2.Subreports[0].SetDataSource(Org);
            else Rpt2.Subreports[0].SetDataSource(Org.Clone());

            Rpt2.SetDataSource(HeaderTable);
            Rpt2.Subreports["x1"].SetDataSource(DetailTable);
            Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
            if (!chk_Logo.Checked)
            {
                Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt2.SetParameterValue("P1Name", " ");
                Rpt2.SetParameterValue("P1ECode", " ");
                Rpt2.SetParameterValue("P1NCode", " ");
                Rpt2.SetParameterValue("P1Address", " ");
                Rpt2.SetParameterValue("P1Tel", " ");
                Rpt2.SetParameterValue("P1PostalCode", " ");
            }
            Rpt2.SetParameterValue("Param3", Sign[0]);
            Rpt2.SetParameterValue("Param4", Sign[1]);
            Rpt2.SetParameterValue("Param5", Sign[2]);
            Rpt2.SetParameterValue("Param6", Sign[3]);
            Rpt2.SetParameterValue("Param7", Sign[4]);
            Rpt2.SetParameterValue("Param8", Sign[5]);
            Rpt2.SetParameterValue("Param9", Sign[6]);
            Rpt2.SetParameterValue("Param10", Sign[7]);
            Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
            Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
            Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
            Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
            crystalReportViewer1.ReportSource = Rpt2;
            this.Cursor = Cursors.Default;
            _PrintStyle = 1;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt2.PrintToPrinter(1, true, 1, 1000);
                Rpt2.Close();
                Rpt2.Dispose();
                GC.Collect();
            }
        }

        private void bt_Third_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح بدون اطلاعات فروشنده
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_03 Rpt2 = new Rpt_02_SaleFactor_03();
                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["x1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 3;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_4_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح پنجم افقی
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_04 Rpt2 = new Rpt_02_SaleFactor_04();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());

                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 4;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_5_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح افقی سوم
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_05 Rpt2 = new Rpt_02_SaleFactor_05();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());

                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 5;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_6_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح افقی دوم
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_06 Rpt2 = new Rpt_02_SaleFactor_06();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());
                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 6;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_7_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح افقی اول
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_07 Rpt2 = new Rpt_02_SaleFactor_07();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());

                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 7;
                if (List.Count > 0)
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
            }
            catch { }
        }

        private void bt_8_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح عمودی
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_08 Rpt2 = new Rpt_02_SaleFactor_08();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());

                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 8;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_Second_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح بدون شماره فاکتور
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_02 Rpt2 = new Rpt_02_SaleFactor_02();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());

                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["x1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 2;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_Ninth_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_09 Rpt2 = new Rpt_02_SaleFactor_09();
                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());

                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 9;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_10_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح رسمی با ضریب
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_10 Rpt2 = new Rpt_02_SaleFactor_10();
                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());
                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["x1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 10;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_11_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح 11 قیمت بسته
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_11 Rpt2 = new Rpt_02_SaleFactor_11();
                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());
                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 11;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_12_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح بدون شماره فاکتور
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_12 Rpt2 = new Rpt_02_SaleFactor_12();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());
                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 12;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }

        }

        private void bt_13_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_13 Rpt2 = new Rpt_02_SaleFactor_13();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());
                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["x1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 13;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_14_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_14 Rpt2 = new Rpt_02_SaleFactor_14();

                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 13;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void _15_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح افقی دوم
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_15 Rpt2 = new Rpt_02_SaleFactor_15();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());
                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 15;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_16_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_16 Rpt2 = new Rpt_02_SaleFactor_16();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());
                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 16;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }

        }

        private void bt_17_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            _05_Sale.Reports.Rpt_02_SaleFactor_17 Rpt2 = new Rpt_02_SaleFactor_17();

            Rpt2.SetDataSource(HeaderTable);

            if (!chk_Logo.Checked)
                Rpt2.Subreports[0].SetDataSource(Org);
            else Rpt2.Subreports[0].SetDataSource(Org.Clone());


            Rpt2.Subreports[1].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt2.SetParameterValue("P1Name", " ");
                Rpt2.SetParameterValue("P1ECode", " ");
                Rpt2.SetParameterValue("P1NCode", " ");
                Rpt2.SetParameterValue("P1Address", " ");
                Rpt2.SetParameterValue("P1Tel", " ");
                Rpt2.SetParameterValue("P1PostalCode", " ");
            }
            Rpt2.SetParameterValue("Param3", Sign[0]);
            Rpt2.SetParameterValue("Param4", Sign[1]);
            Rpt2.SetParameterValue("Param5", Sign[2]);
            Rpt2.SetParameterValue("Param6", Sign[3]);
            Rpt2.SetParameterValue("Param7", Sign[4]);
            Rpt2.SetParameterValue("Param8", Sign[5]);
            Rpt2.SetParameterValue("Param9", Sign[6]);
            Rpt2.SetParameterValue("Param10", Sign[7]);
            Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
            crystalReportViewer1.ReportSource = Rpt2;
            _PrintStyle = 17;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt2.PrintToPrinter(1, true, 1, 1000);
            }
        }

        private void bt_18_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            _05_Sale.Reports.Rpt_02_SaleFactor_18 Rpt2 = new Rpt_02_SaleFactor_18();

            Rpt2.SetDataSource(HeaderTable);
            if (!chk_Logo.Checked)
                Rpt2.Subreports[0].SetDataSource(Org);
            else Rpt2.Subreports[0].SetDataSource(Org.Clone());


            Rpt2.Subreports[1].SetDataSource(DetailTable);
            Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
            if (!chk_Logo.Checked)
            {
                Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt2.SetParameterValue("P1Name", " ");
                Rpt2.SetParameterValue("P1ECode", " ");
                Rpt2.SetParameterValue("P1NCode", " ");
                Rpt2.SetParameterValue("P1Address", " ");
                Rpt2.SetParameterValue("P1Tel", " ");
                Rpt2.SetParameterValue("P1PostalCode", " ");
            }
            Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
            Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
            Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
            Rpt2.SetParameterValue("Param3", Sign[0]);
            Rpt2.SetParameterValue("Param4", Sign[1]);
            Rpt2.SetParameterValue("Param5", Sign[2]);
            Rpt2.SetParameterValue("Param6", Sign[3]);
            Rpt2.SetParameterValue("Param7", Sign[4]);
            Rpt2.SetParameterValue("Param8", Sign[5]);
            Rpt2.SetParameterValue("Param9", Sign[6]);
            Rpt2.SetParameterValue("Param10", Sign[7]);
            Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
            crystalReportViewer1.ReportSource = Rpt2;
            _PrintStyle = 18;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt2.PrintToPrinter(1, true, 1, 1000);
            }
        }

        private void bt_19_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            _05_Sale.Reports.Rpt_02_SaleFactor_19 Rpt2 = new Rpt_02_SaleFactor_19();

            Rpt2.SetDataSource(HeaderTable);
            if (!chk_Logo.Checked)
                Rpt2.Subreports[0].SetDataSource(Org);
            else Rpt2.Subreports[0].SetDataSource(Org.Clone());


            Rpt2.Subreports[1].SetDataSource(DetailTable);
            Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);

            if (!chk_Logo.Checked)
            {
                Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt2.SetParameterValue("P1Name", " ");
                Rpt2.SetParameterValue("P1ECode", " ");
                Rpt2.SetParameterValue("P1NCode", " ");
                Rpt2.SetParameterValue("P1Address", " ");
                Rpt2.SetParameterValue("P1Tel", " ");
                Rpt2.SetParameterValue("P1PostalCode", " ");
            }
            Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
            Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
            Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
            Rpt2.SetParameterValue("Param3", Sign[0]);
            Rpt2.SetParameterValue("Param4", Sign[1]);
            Rpt2.SetParameterValue("Param5", Sign[2]);
            Rpt2.SetParameterValue("Param6", Sign[3]);
            Rpt2.SetParameterValue("Param7", Sign[4]);
            Rpt2.SetParameterValue("Param8", Sign[5]);
            Rpt2.SetParameterValue("Param9", Sign[6]);
            Rpt2.SetParameterValue("Param10", Sign[7]);
            Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
            crystalReportViewer1.ReportSource = Rpt2;
            _PrintStyle = 19;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt2.PrintToPrinter(1, true, 1, 1000);
            }
        }

        private void bt_20_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح بدون اطلاعات فروشنده
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_20 Rpt2 = new Rpt_02_SaleFactor_20();
                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["x1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 20;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void chk_ShowCustomerBill_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_ShowCustomerBill.Checked)
                mlt_ACC.Enabled = true;
            else mlt_ACC.Enabled = false;
        }

        private void mlt_ACC_KeyPress(object sender, KeyPressEventArgs e)
        {
            mlt_ACC.DroppedDown = true;
        }

        private void bt_21_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح افقی سوم
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_21 Rpt2 = new Rpt_02_SaleFactor_21();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());

                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 21;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_22_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح افقی سوم
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_22 Rpt2 = new Rpt_02_SaleFactor_22();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());

                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 22;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_23_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح عمودی
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_23 Rpt2 = new Rpt_02_SaleFactor_23();

                if (!chk_Logo.Checked)
                    Rpt2.Subreports[0].SetDataSource(Org);
                else Rpt2.Subreports[0].SetDataSource(Org.Clone());

                Rpt2.SetDataSource(HeaderTable);
                Rpt2.Subreports["X1"].SetDataSource(DetailTable);
                Rpt2.Subreports["X2"].SetDataSource(TotalSettleInfo);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("Param3", Sign[0]);
                Rpt2.SetParameterValue("Param4", Sign[1]);
                Rpt2.SetParameterValue("Param5", Sign[2]);
                Rpt2.SetParameterValue("Param6", Sign[3]);
                Rpt2.SetParameterValue("Param7", Sign[4]);
                Rpt2.SetParameterValue("Param8", Sign[5]);
                Rpt2.SetParameterValue("Param9", Sign[6]);
                Rpt2.SetParameterValue("Param10", Sign[7]);
                Rpt2.SetParameterValue("ShowSettleInfo", chk_ShowCustomerBill.Checked);
                Rpt2.SetParameterValue("ShowSentence", chk_ShowSen.Checked);
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 23;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_24_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح عمودی
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_24 Rpt2 = new Rpt_02_SaleFactor_24();


                Rpt2.SetDataSource(HeaderTable);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);


                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 24;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void bt_25_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                //طرح عمودی
                this.Cursor = Cursors.WaitCursor;
                _05_Sale.Reports.Rpt_02_SaleFactor_25 Rpt2 = new Rpt_02_SaleFactor_25();


                Rpt2.SetDataSource(HeaderTable);
                if (!chk_Logo.Checked)
                {
                    Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
                }
                else
                {
                    Rpt2.SetParameterValue("P1Name", " ");
                    Rpt2.SetParameterValue("P1ECode", " ");
                    Rpt2.SetParameterValue("P1NCode", " ");
                    Rpt2.SetParameterValue("P1Address", " ");
                    Rpt2.SetParameterValue("P1Tel", " ");
                    Rpt2.SetParameterValue("P1PostalCode", " ");
                }
                Rpt2.SetParameterValue("NotShowDate", chk_ShowDate.Checked);
                Rpt2.SetParameterValue("ShowEcoCode", chk_ShowEcoCode.Checked);
                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                _PrintStyle = 25;
                if (List.Count > 0)
                {
                    PrinterSettings getprinterName = new PrinterSettings();
                    Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                    Rpt2.PrintToPrinter(1, true, 1, 1000);
                }
            }
            catch { }
        }

        private void btn_26_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                stiViewerControl1.Visible = true;
                crystalReportViewer1.Visible = false;
                _PrintStyle = 26;


                StiReport stireport = new StiReport();
                stireport.Load("CustomReport.mrt");
                stireport.Pages["Page1"].Enabled = true;
                stireport.Compile();
                StiOptions.Viewer.AllowUseDragDrop = false;
                if (!chk_Logo.Checked)
                {
                    stireport.RegData("Table_000_OrgInfo", Org);
                    stireport["Site"] = Org.Rows[0]["Column13"].ToString();
                    stireport["P1Tel"] = Org.Rows[0]["Column03"].ToString();
                }
                else
                {
                    stireport.RegData("Table_000_OrgInfo", Org.Clone());
                    stireport["Site"] = "";
                    stireport["P1Tel"] ="";
                }

                stireport.RegData("Rpt_SaleTable", HeaderTable);
                stireport.RegData("Rpt_SaleExtra_Table", DetailTable);
                stireport.RegData("FN_01_SettleInfo", TotalSettleInfo);

                stireport["ChkSite"] = false;
                stireport["ChkTell"] = false;
                stireport["ChkTitr"] = false;
                stireport["ChkSoTitr"] = false;
                stireport["ChkFactorNum"] = false;
                stireport["ChkPageNum"] = false;
                stireport["ChkSaleMan"] = false;
                stireport["NotShowDate"] = false;

               
                stireport["P1Name"] = " ";
                stireport["P1ECode"] = " ";
                stireport["P1NCode"] = " ";
                stireport["P1Address"] = " ";
                stireport["P1PostalCode"] = " ";
                stireport["Param3"] = Sign[0];
                stireport["Param4"] = Sign[1];
                stireport["Param5"] = Sign[2];
                stireport["Param6"] = Sign[3];
                stireport["Param7"] = Sign[4];
                stireport["Param8"] = Sign[5];
                stireport["Param9"] = Sign[6];
                stireport["Param10"] = Sign[7];
                stireport["ShowSettleInfo"] = chk_ShowCustomerBill.Checked;
                stireport["ShowSentence"] = chk_ShowSen.Checked;
                stireport["NotShowDate"] = chk_ShowDate.Checked;
                this.Cursor = Cursors.Default;
                stireport.IsSelected = true;
                stireport.Select();
                //stireport.Show();
                stireport.Render(false);
                stiViewerControl1.Report = stireport;
                stiViewerControl1.Refresh();

            }
            catch
            {
            }

        }



        private void btn_Design_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("CustomReport.mrt");
            stireport.Design();
        }





    }
}