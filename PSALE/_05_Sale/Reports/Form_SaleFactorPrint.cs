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
using Stimulsoft.Report.Dictionary;

namespace PSHOP._05_Sale.Reports
{
    public partial class Form_SaleFactorPrint : Form
    {
        Classes.Class_Settle cl_Settle = new Classes.Class_Settle();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMAIN = new SqlConnection(Properties.Settings.Default.MAIN);

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


        public Form_SaleFactorPrint(int SaleNumber, bool ClubPrint)
        {
            InitializeComponent();
            _SaleNumber = SaleNumber;
            _ClubPrint = ClubPrint;
        }

        public Form_SaleFactorPrint(List<string> _List, bool ClubPrint)
        {
            InitializeComponent();
            List = _List;
            _ClubPrint = ClubPrint;
        }

        public Form_SaleFactorPrint(DataTable _HeaderTable, DataTable _DetailTable, bool ClubPrint)
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
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 197))
                {
                    chk_ShowCustomerBill.Enabled = true;
                    chk_ShowCustomerBill.Checked = Properties.Settings.Default.ShowCustomerBill;
                }
                else
                {
                    chk_ShowCustomerBill.Enabled = false;
                    chk_ShowCustomerBill.Checked = false;
                }

                chk_ShowSen.Checked = Properties.Settings.Default.ShowSaleFactorSentence;
                chk_Logo.Checked = Properties.Settings.Default.DontShowLogo;
                chk_gift.Checked = Properties.Settings.Default.giftOrder;
                _PrintStyle = Properties.Settings.Default.SaleFactorStyle;
                mlt_ACC.DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ACC_Code,ACC_Name from AllHeaders()");
                if (Properties.Settings.Default.SaveACCRemain.Trim() != "")
                    mlt_ACC.Value = Properties.Settings.Default.SaveACCRemain;

                rb_goodCode.CheckedChanged -= rb_goodCode_CheckedChanged;
                rb_goodname.CheckedChanged -= rb_goodname_CheckedChanged;
                rb_insertorder.CheckedChanged -= rb_insertorder_CheckedChanged;
                if (Properties.Settings.Default.printorder == 1)
                    rb_goodCode.Checked = true;
                else if (Properties.Settings.Default.printorder == 2)
                    rb_goodname.Checked = true;
                else
                    rb_insertorder.Checked = true;

                rb_goodCode.CheckedChanged += rb_goodCode_CheckedChanged;
                rb_goodname.CheckedChanged += rb_goodname_CheckedChanged;
                rb_insertorder.CheckedChanged += rb_insertorder_CheckedChanged;

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
            Properties.Settings.Default.giftOrder = chk_gift.Checked;

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
                    HeaderSelectText = @"  SELECT     FactorTable.FactorID AS ID,DetailId, FactorTable.Serial, FactorTable.LegalNumber, FactorTable.Date, FactorTable.Responsible, FactorTable.CustomerID, FactorTable.P2Name, 
                                FactorTable.P2NationalCode,  FactorTable.P2ECode,FactorTable.P2SabtCode,
                                FactorTable.P2Address, FactorTable.P2Tel,FactorTable.Mobile, FactorTable.P2Fax, FactorTable.P2PostalCode, FactorTable.P2Code, FactorTable.GoodCode, 
                                FactorTable.GoodName, FactorTable.Box, FactorTable.BoxPrice, FactorTable.Pack, FactorTable.PackPrice, FactorTable.Number, FactorTable.TotalNumber, 
                                FactorTable.SinglePrice, FactorTable.TotalPrice, FactorTable.DiscountPercent, FactorTable.DiscountPrice, FactorTable.TaxPrice,FactorTable.TotalWeight,FactorTable.SingleWeight,FactorTable.TaxPercent, FactorTable.NetPrice, 
                                ISNULL(FactorTable.Ezafat, 0) AS Ezafat, ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat, PersonInfoTable.Column02, FactorTable.NetTotal, FactorTable.VolumeGroup, 
                                FactorTable.SpecialGroup, FactorTable.SpecialCustomer, FactorTable.Description, FactorTable.CountUnitName, derivedtbl_1.Groups, '-' AS charPrice, 
                                'SettleInfo' AS SettleInfo, FactorTable.FactorType, FactorTable.NumberInBox, FactorTable.RowDesc, FactorTable.Zarib,FactorTable.Series,FactorTable.ExpirationDate,FactorTable.Gift, FactorTable.NumberInBox AS Expr1, 
                                FactorTable.NumberInPack, CityTable.Column02 AS CityName, ProvinceTable.Column01 AS ProvinceName,FactorTable.PayCash,FactorTable.DraftNumber,FactorTable.BarCode,FactorTable.DocID,FactorTable.Feild, FactorTable.GoodDesc,FactorTable.Column35 as ExpDate,FactorTable.Column34 as BuildSeri,FactorTable.SaleCondition,FactorTable.Ware,FactorTable.InsertUser,
                                    FactorTable.ConfirmUser,
                                    FactorTable.InsertUserSign,
                                    FactorTable.ConfirmUserSign
                                FROM         (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
                                FROM         {0}.dbo.Table_060_ProvinceInfo) AS ProvinceTable INNER JOIN
                                (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                                FROM         {0}.dbo.Table_065_CityInfo) AS CityTable ON ProvinceTable.Column00 = CityTable.Column00 RIGHT OUTER JOIN
                                (SELECT     dbo.Table_010_SaleFactor.columnid AS FactorID,Table_011_Child1_SaleFactor.ColumnId as DetailId, dbo.Table_010_SaleFactor.column01 AS Serial, dbo.Table_010_SaleFactor.column37 AS LegalNumber, 
                                dbo.Table_010_SaleFactor.column02 AS Date, dbo.Table_010_SaleFactor.column03 AS CustomerID, PersonTable.Column02 AS P2Name, 
                                PersonTable.Column09 AS P2NationalCode,PersonTable.Column141 AS P2ECode,PersonTable.Column142 AS P2SabtCode,
                                PersonTable.Column01 AS P2Code, PersonTable.Column06 AS P2Address, PersonTable.Column07 AS P2Tel, PersonTable.Column19 AS Mobile, 
                                PersonTable.Column08 AS P2Fax, PersonTable.Column13 AS P2PostalCode,PersonTable.Column10 as Feild, GoodTable.column01 AS GoodCode, 
                                dbo.Table_011_Child1_SaleFactor.column04 AS Box, dbo.Table_011_Child1_SaleFactor.column08 AS BoxPrice, 
                                dbo.Table_011_Child1_SaleFactor.column05 AS Pack, dbo.Table_011_Child1_SaleFactor.column09 AS PackPrice, 
                                dbo.Table_011_Child1_SaleFactor.column06 AS Number, dbo.Table_011_Child1_SaleFactor.column07 AS TotalNumber, 
                                dbo.Table_011_Child1_SaleFactor.column10 AS SinglePrice, dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice, 
                                dbo.Table_011_Child1_SaleFactor.column16 AS DiscountPercent, dbo.Table_011_Child1_SaleFactor.column17 AS DiscountPrice, dbo.Table_011_Child1_SaleFactor.column18 as TaxPercent,
                                dbo.Table_011_Child1_SaleFactor.column19 AS TaxPrice,dbo.Table_011_Child1_SaleFactor.column37 as TotalWeight,dbo.Table_011_Child1_SaleFactor.column36 AS SingleWeight, dbo.Table_011_Child1_SaleFactor.column20 AS NetPrice, GoodTable.column02 AS GoodName,
                                OtherPrice.PlusPrice AS Ezafat, OtherPrice.MinusPrice AS Kosoorat, dbo.Table_010_SaleFactor.column05 AS Responsible, 
                                dbo.Table_010_SaleFactor.Column28 AS NetTotal, dbo.Table_010_SaleFactor.Column29 AS VolumeGroup, 
                                dbo.Table_010_SaleFactor.Column30 AS SpecialGroup, dbo.Table_010_SaleFactor.Column31 AS SpecialCustomer, 
                                dbo.Table_010_SaleFactor.column06 AS Description, CountUnitTable.Column01 AS CountUnitName, 
                                CASE WHEN Table_010_SaleFactor.Column12 = 0 THEN '***فاکتور ریالی***' ELSE '***فاکتور ارزی***' END AS FactorType, 
                                dbo.Table_011_Child1_SaleFactor.column23 AS RowDesc, dbo.Table_011_Child1_SaleFactor.column31 AS NumberInBox, 
                                dbo.Table_011_Child1_SaleFactor.column32 AS NumberInPack, dbo.Table_011_Child1_SaleFactor.Column33 AS Zarib, dbo.Table_011_Child1_SaleFactor.Column34 AS Series,dbo.Table_011_Child1_SaleFactor.Column35 AS ExpirationDate,dbo.Table_011_Child1_SaleFactor.Column30 AS Gift,
                                PersonTable.Column21 AS ProvinceId, PersonTable.Column22 AS CityId,Table_010_SaleFactor.column21 as PayCash,
                                (Select Column01 from " + ConWare.Database + @".dbo.Table_007_PwhrsDraft where Columnid=Table_010_SaleFactor.Column09) as DraftNumber,
                                   (Select column06 from " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients where Columnid=dbo.Table_011_Child1_SaleFactor.column02) as BarCode,
                                Table_010_SaleFactor.Column10 as DocId, dbo.Table_011_Child1_SaleFactor.Column23 AS GoodDesc, dbo.Table_011_Child1_SaleFactor.column34, dbo.Table_011_Child1_SaleFactor.column35,dbo.Table_010_SaleFactor.column04 AS SaleCondition,tp.column02 AS Ware,dbo.Table_010_SaleFactor.column13 AS InsertUser,
                      (case when dbo.Table_010_SaleFactor.Column61=1 then dbo.Table_010_SaleFactor.Column62 else '' end ) AS ConfirmUser,
                       (
                           SELECT top 1 Column39
                           FROM   " + ConMAIN.Database + @".dbo.Table_010_UserInfo
                           WHERE  Column00 = dbo.Table_010_SaleFactor.column13 and  Column05=" + Class_BasicOperation._OrgCode + @" and  Column06=N'" + Class_BasicOperation._Year + @"' 
                       )AS InsertUserSign,
                       (case when dbo.Table_010_SaleFactor.Column61=1 then (
                           SELECT top 1 Column39
                           FROM   " + ConMAIN.Database + @".dbo.Table_010_UserInfo
                           WHERE  Column00 = dbo.Table_010_SaleFactor.Column62  and  Column05=" + Class_BasicOperation._OrgCode + @" and  Column06=N'" + Class_BasicOperation._Year + @"' 
                       ) else null end) AS ConfirmUserSign
                                FROM         dbo.Table_010_SaleFactor
                                                LEFT JOIN " + ConWare.Database + @".dbo.Table_001_PWHRS tp ON tp.columnid=dbo.Table_010_SaleFactor.Column42
 
                                INNER JOIN
                                dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                                (SELECT     ColumnId, Column00, Column01, Column02, Column03, Column04, Column05, ISNULL(
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
                               ) + ' ' + ISNULL({0}.dbo.Table_045_PersonInfo.Column06, ' ')  AS Column06, Column07, Column08, Column09, Column10, 
                                Column11, Column12, Column13, Column21, Column22, Column19, Column141,Column142
                                       
                                FROM         {0}.dbo.Table_045_PersonInfo) AS PersonTable ON dbo.Table_010_SaleFactor.column03 = PersonTable.ColumnId LEFT OUTER JOIN
                                (SELECT     columnid, SUM(PlusPrice) AS PlusPrice, SUM(MinusPrice) AS MinusPrice
                                FROM         (SELECT     Table_010_SaleFactor_2.columnid, SUM(dbo.Table_012_Child2_SaleFactor.column04) AS PlusPrice, 0 AS MinusPrice
                                FROM         dbo.Table_012_Child2_SaleFactor INNER JOIN
                                dbo.Table_010_SaleFactor AS Table_010_SaleFactor_2 ON 
                                dbo.Table_012_Child2_SaleFactor.column01 = Table_010_SaleFactor_2.columnid
                                WHERE     (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                GROUP BY Table_010_SaleFactor_2.columnid, dbo.Table_012_Child2_SaleFactor.column05
                                UNION ALL
                                SELECT     Table_010_SaleFactor_1.columnid, 0 AS PlusPrice, SUM(Table_012_Child2_SaleFactor_1.column04) AS MinusPrice
                                FROM         dbo.Table_012_Child2_SaleFactor AS Table_012_Child2_SaleFactor_1 INNER JOIN
                                dbo.Table_010_SaleFactor AS Table_010_SaleFactor_1 ON 
                                Table_012_Child2_SaleFactor_1.column01 = Table_010_SaleFactor_1.columnid
                                WHERE     (Table_012_Child2_SaleFactor_1.column05 = 1)
                                GROUP BY Table_010_SaleFactor_1.columnid, Table_012_Child2_SaleFactor_1.column05) AS OtherPrice_1
                                GROUP BY columnid) AS OtherPrice ON dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid LEFT OUTER JOIN
                                (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, 
                                column12, column13, column14, column15, column16, column17, column18, column19, column20, column21, column22, column23, 
                                column24, column25, column26, column27, column28, column29, column30, column31
                                FROM         {1}.dbo.table_004_CommodityAndIngredients) AS GoodTable ON 
                                dbo.Table_011_Child1_SaleFactor.column02 = GoodTable.columnid LEFT OUTER JOIN
                                (SELECT     Column00, Column01
                                FROM         {0}.dbo.Table_070_CountUnitInfo) AS CountUnitTable ON dbo.Table_011_Child1_SaleFactor.column03 = CountUnitTable.Column00) 
                                AS FactorTable ON CityTable.Column01 = FactorTable.CityId LEFT OUTER JOIN
                                (SELECT     PersonId, Groups
                                FROM         {0}.dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1) AS derivedtbl_1 ON FactorTable.CustomerID = derivedtbl_1.PersonId LEFT OUTER JOIN
                                (SELECT     ColumnId, Column01, Column02, Column21, Column22
                                FROM         {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1) AS PersonInfoTable ON FactorTable.Responsible = PersonInfoTable.ColumnId";
                }
                else
                {
                    HeaderSelectText = @"SELECT     FactorTable.FactorID AS ID,DetailId, FactorTable.Serial, FactorTable.LegalNumber, FactorTable.Date, FactorTable.Responsible, FactorTable.CustomerID, FactorTable.P2Name, 
                       '' as P2NationalCode,  FactorTable.P2ECode,'' as P2SabtCode, FactorTable.P2Address, FactorTable.P2Tel, FactorTable.P2Fax, FactorTable.P2PostalCode, FactorTable.P2Code, FactorTable.GoodCode, 
                      FactorTable.GoodName, FactorTable.Box, FactorTable.BoxPrice, FactorTable.Pack, FactorTable.PackPrice, FactorTable.Number, FactorTable.TotalNumber, 
                      FactorTable.SinglePrice, FactorTable.TotalPrice, FactorTable.DiscountPercent, FactorTable.DiscountPrice, FactorTable.TaxPrice,FactorTable.TotalWeight,FactorTable.SingleWeight,FactorTable.TaxPercent, FactorTable.NetPrice, 
                      ISNULL(FactorTable.Ezafat, 0) AS Ezafat, ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat, PersonInfoTable.Column02, FactorTable.NetTotal, FactorTable.VolumeGroup, 
                      FactorTable.SpecialGroup, FactorTable.SpecialCustomer, FactorTable.Description, FactorTable.CountUnitName, derivedtbl_1.Groups, '-' AS charPrice, 
                      'SettleInfo' AS SettleInfo, FactorTable.FactorType, FactorTable.NumberInBox, FactorTable.RowDesc, FactorTable.Zarib,FactorTable.Series,FactorTable.ExpirationDate,FactorTable.Gift,FactorTable.NumberInBox AS Expr1, 
                      FactorTable.NumberInPack, FactorTable.PayCash, FactorTable.GoodDesc
                      FROM         (SELECT     PersonId, Groups
                       FROM          {0}.dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1) AS derivedtbl_1 RIGHT OUTER JOIN
                          (SELECT     dbo.Table_010_SaleFactor.columnid AS FactorID, Table_011_Child1_SaleFactor.ColumnId as DetailId, dbo.Table_010_SaleFactor.column01 AS Serial, dbo.Table_010_SaleFactor.column37 AS LegalNumber, 
                                                   dbo.Table_010_SaleFactor.column02 AS Date, dbo.Table_010_SaleFactor.column18 AS CustomerID, 
                                                   PersonTable.Column03 + N' ' + PersonTable.Column02 AS P2Name, PersonTable.Column07 AS P2ECode, PersonTable.Column01 AS P2Code, 
                                                   PersonTable.Column08 AS P2Address, PersonTable.Column04 AS P2Tel, PersonTable.Column05 AS P2Fax, PersonTable.Column09 AS P2PostalCode, 
                                                   GoodTable.column01 AS GoodCode, dbo.Table_011_Child1_SaleFactor.column04 AS Box, dbo.Table_011_Child1_SaleFactor.column08 AS BoxPrice, 
                                                   dbo.Table_011_Child1_SaleFactor.column05 AS Pack, dbo.Table_011_Child1_SaleFactor.column09 AS PackPrice, 
                                                   dbo.Table_011_Child1_SaleFactor.column06 AS Number, dbo.Table_011_Child1_SaleFactor.column07 AS TotalNumber, 
                                                   dbo.Table_011_Child1_SaleFactor.column10 AS SinglePrice, dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice, 
                                                   dbo.Table_011_Child1_SaleFactor.column16 AS DiscountPercent, dbo.Table_011_Child1_SaleFactor.column17 AS DiscountPrice, dbo.Table_011_Child1_SaleFactor.column18 as TaxPercent,
                                                   dbo.Table_011_Child1_SaleFactor.column19 AS TaxPrice,Table_011_Child1_SaleFactor.Column37 as TotalWeight,dbo.Table_011_Child1_SaleFactor.column36 AS SingleWeight, dbo.Table_011_Child1_SaleFactor.column20 AS NetPrice, GoodTable.column02 AS GoodName,
                                                    OtherPrice.PlusPrice AS Ezafat, OtherPrice.MinusPrice AS Kosoorat, dbo.Table_010_SaleFactor.column05 AS Responsible, 
                                                   dbo.Table_010_SaleFactor.Column28 AS NetTotal, dbo.Table_010_SaleFactor.Column29 AS VolumeGroup, 
                                                   dbo.Table_010_SaleFactor.Column30 AS SpecialGroup, dbo.Table_010_SaleFactor.Column31 AS SpecialCustomer, 
                                                   dbo.Table_010_SaleFactor.column06 AS Description, CountUnitTable.Column01 AS CountUnitName, 
                                                   CASE WHEN Table_010_SaleFactor.Column12 = 0 THEN '***فاکتور ریالی***' ELSE '***فاکتور ارزی***' END AS FactorType, 
                                                   dbo.Table_011_Child1_SaleFactor.column23 AS RowDesc, dbo.Table_011_Child1_SaleFactor.column31 AS NumberInBox, 
                                                   dbo.Table_011_Child1_SaleFactor.column32 AS NumberInPack, dbo.Table_011_Child1_SaleFactor.Column33 AS Zarib, dbo.Table_011_Child1_SaleFactor.Column34 AS Series,dbo.Table_011_Child1_SaleFactor.Column35 AS ExpirationDate,dbo.Table_011_Child1_SaleFactor.Column30 AS Gift,
                                                   dbo.Table_010_SaleFactor.column21 AS PayCash,  dbo.Table_011_Child1_SaleFactor.Column23 AS GoodDesc
                            FROM          dbo.Table_010_SaleFactor INNER JOIN
                                                   dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                                                       (SELECT     ColumnId, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, 
                                                                                Column12, Column13
                                                         FROM          {0}.dbo.Table_215_CustomerClub) AS PersonTable ON 
                                                   dbo.Table_010_SaleFactor.column18 = PersonTable.ColumnId LEFT OUTER JOIN
                                                       (SELECT     columnid, SUM(PlusPrice) AS PlusPrice, SUM(MinusPrice) AS MinusPrice
                                                         FROM          (SELECT     Table_010_SaleFactor_2.columnid, SUM(dbo.Table_012_Child2_SaleFactor.column04) AS PlusPrice, 0 AS MinusPrice
                                                                                 FROM          dbo.Table_012_Child2_SaleFactor INNER JOIN
                                                                                                        dbo.Table_010_SaleFactor AS Table_010_SaleFactor_2 ON 
                                                                                                        dbo.Table_012_Child2_SaleFactor.column01 = Table_010_SaleFactor_2.columnid
                                                                                 WHERE      (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                                 GROUP BY Table_010_SaleFactor_2.columnid, dbo.Table_012_Child2_SaleFactor.column05
                                                                                 UNION ALL
                                                                                 SELECT     Table_010_SaleFactor_1.columnid, 0 AS PlusPrice, SUM(Table_012_Child2_SaleFactor_1.column04) AS MinusPrice
                                                                                 FROM         dbo.Table_012_Child2_SaleFactor AS Table_012_Child2_SaleFactor_1 INNER JOIN
                                                                                                       dbo.Table_010_SaleFactor AS Table_010_SaleFactor_1 ON 
                                                                                                       Table_012_Child2_SaleFactor_1.column01 = Table_010_SaleFactor_1.columnid
                                                                                 WHERE     (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                                 GROUP BY Table_010_SaleFactor_1.columnid, Table_012_Child2_SaleFactor_1.column05) AS OtherPrice_1
                                                         GROUP BY columnid) AS OtherPrice ON dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid LEFT OUTER JOIN
                                                       (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, 
                                                                                column12, column13, column14, column15, column16, column17, column18, column19, column20, column21, column22, column23, 
                                                                                column24, column25, column26, column27, column28, column29, column30, column31
                                                         FROM          {1}.dbo.table_004_CommodityAndIngredients) AS GoodTable ON 
                                                   dbo.Table_011_Child1_SaleFactor.column02 = GoodTable.columnid LEFT OUTER JOIN
                                                       (SELECT     Column00, Column01
                                                         FROM          {0}.dbo.Table_070_CountUnitInfo) AS CountUnitTable ON dbo.Table_011_Child1_SaleFactor.column03 = CountUnitTable.Column00) 
                      AS FactorTable ON derivedtbl_1.PersonId = FactorTable.CustomerID LEFT OUTER JOIN
                          (SELECT     ColumnId, Column01, Column02, Column21, Column22
                            FROM          {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1) AS PersonInfoTable ON FactorTable.Responsible = PersonInfoTable.ColumnId";
                }
                HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);

                string DetailSelectText = @"SELECT     dbo.Table_024_Discount.column01 AS Name, dbo.Table_012_Child2_SaleFactor.column03 AS Darsad, dbo.Table_012_Child2_SaleFactor.column04 AS Price, 
                      CASE WHEN Table_024_Discount.column02 = 0 THEN '+' ELSE '-' END AS Type, dbo.Table_012_Child2_SaleFactor.column01 AS Column01 ,
                      dbo.Table_010_SaleFactor.column01 AS HeaderNum, dbo.Table_010_SaleFactor.column02 AS HeaderDate
                      FROM         dbo.Table_012_Child2_SaleFactor INNER JOIN
                      dbo.Table_010_SaleFactor ON dbo.Table_012_Child2_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid LEFT OUTER JOIN
                      dbo.Table_024_Discount ON dbo.Table_012_Child2_SaleFactor.column02 = dbo.Table_024_Discount.columnid";

                if (List.Count > 0)
                {
                    HeaderSelectText += " WHERE     FactorTable.FactorID IN(" + string.Join(",", List.ToArray()) + ")  ";
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
                    HeaderSelectText += " WHERE     (FactorTable.Date between  '" + faDatePicker1.Text + "' and '" + faDatePicker2.Text + "')  ";
                    DetailSelectText += @" WHERE (Table_010_SaleFactor.Column02 between '" + faDatePicker1.Text + "' and '" + faDatePicker2.Text + "')";
                }

                if (rb_goodCode.Checked)
                    HeaderSelectText += "order by FactorTable.FactorID," + (chk_gift.Checked ? "FactorTable.Gift," : " ") + "FactorTable.GoodCode";
                else if (rb_goodname.Checked)
                    HeaderSelectText += "order by FactorTable.FactorID," + (chk_gift.Checked ? "FactorTable.Gift," : " ") + "FactorTable.GoodName";
                else
                    HeaderSelectText += "order by FactorTable.FactorID," + (chk_gift.Checked ? "FactorTable.Gift," : " ") + "DetailId";


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
                String Sentence = null;

                if (chk_ShowCustomerBill.Checked && mlt_ACC.Text.Trim() != "")
                {
                    foreach (DataRow item in CustomerTable.Rows)
                    {

                        double X = 0;
                        double Y = 0;

                        double Remain = Convert.ToInt64(clDoc.ReturnTable(ConAcnt.ConnectionString, @"select ISNULL((
                    Select SUM(Table_065_SanadDetail.Column11)-SUM(Table_065_SanadDetail.Column12) from Table_065_SanadDetail inner join Table_060_SanadHead
                    ON Table_065_SanadDetail.Column00=Table_060_SanadHead.ColumnId
                    where Table_065_SanadDetail.Column07=" + item["CustomerId"].ToString() + @" and   Table_065_SanadDetail.Column01='" + mlt_ACC.Value.ToString() + @"' and
                    Table_060_SanadHead.Column01<='" + item["Date"].ToString() + @"'
                    group by Table_065_SanadDetail.Column07),0) as Remain").Rows[0][0].ToString());

                        Sentence = null;
                        double FinalPrice = Convert.ToDouble(HeaderTable.Compute("Max( NetTotal )", "ID=" + item["ID"]))
                                             + Convert.ToDouble(HeaderTable.Compute("Max( Ezafat )", "ID=" + item["ID"]))
                                             - Convert.ToDouble(HeaderTable.Compute("Max( Kosoorat )", "ID=" + item["ID"]))
                                             - Convert.ToDouble(HeaderTable.Compute("Max( SpecialGroup )", "ID=" + item["ID"]))
                                            - Convert.ToDouble(HeaderTable.Compute("Max( SpecialCustomer )", "ID=" + item["ID"]))
                                             - Convert.ToDouble(HeaderTable.Compute("Max( VolumeGroup )", "ID=" + item["ID"]));

                        if (item["DocId"].ToString() != "0")
                        {


                            X = Remain - FinalPrice;
                            Y = X + FinalPrice;

                            Sentence = " مانده حساب بدون احتساب این فاکتور : " + Math.Abs(X).ToString("n0") + " ریال" + (X < 0 ? " بستانکار " : (X > 0 ? " بدهکار " : " ")) + Environment.NewLine +
                                    " مانده حساب بااحتساب این فاکتور: " + Math.Abs(Y).ToString("n0") + " ریال" + (Y < 0 ? " بستانکار " : (Y > 0 ? " بدهکار " : " "));

                        }
                        else
                        {



                            X = Remain;
                            Y = X + FinalPrice;

                            Sentence = " مانده حساب بدون احتساب این فاکتور : " + Math.Abs(X).ToString("n0") + " ریال" + (X < 0 ? " بستانکار " : (X > 0 ? " بدهکار " : " ")) + Environment.NewLine +
                                   " مانده حساب با احتساب این فاکتور: " + Math.Abs(Y).ToString("n0") + " ریال" + (Y < 0 ? " بستانکار " : (Y > 0 ? " بدهکار " : " "));
                        }



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

                        double X = 0;
                        double Y = 0;

                        Int64 Remain = Convert.ToInt64(clDoc.ReturnTable(ConAcnt.ConnectionString, @"select ISNULL((
                                Select SUM(Table_065_SanadDetail.Column11)-SUM(Table_065_SanadDetail.Column12) from Table_065_SanadDetail inner join Table_060_SanadHead
                                ON Table_065_SanadDetail.Column00=Table_060_SanadHead.ColumnId
                                where Table_065_SanadDetail.Column07=" + item["CustomerId"].ToString() + @" and
                                Table_060_SanadHead.Column01<='" + item["Date"].ToString() + @"'
                                group by Table_065_SanadDetail.Column07),0) as Remain").Rows[0][0].ToString());
                        Sentence = null;
                        double FinalPrice = Convert.ToDouble(HeaderTable.Compute("Max( NetTotal )", "ID=" + item["ID"]))
                                             + Convert.ToDouble(HeaderTable.Compute("Max( Ezafat )", "ID=" + item["ID"]))
                                             - Convert.ToDouble(HeaderTable.Compute("Max( Kosoorat )", "ID=" + item["ID"]))
                                             - Convert.ToDouble(HeaderTable.Compute("Max( SpecialGroup )", "ID=" + item["ID"]))
                                            - Convert.ToDouble(HeaderTable.Compute("Max( SpecialCustomer )", "ID=" + item["ID"]))
                                             - Convert.ToDouble(HeaderTable.Compute("Max( VolumeGroup )", "ID=" + item["ID"]));

                        if (item["DocId"].ToString() != "0")
                        {

                            X = Remain - FinalPrice;
                            Y = X + FinalPrice;

                            Sentence = " مانده حساب بدون احتساب این فاکتور : " + Math.Abs(X).ToString("n0") + " ریال" + (X < 0 ? " بستانکار " : (X > 0 ? " بدهکار " : " ")) + Environment.NewLine +
                                     " مانده حساب با احتساب این فاکتور: " + Math.Abs(Y).ToString("n0") + " ریال" + (Y < 0 ? " بستانکار " : (Y > 0 ? " بدهکار " : " "));
                        }
                        else
                        {
                            X = Remain;
                            Y = X + FinalPrice;

                            Sentence = " مانده حساب بدون احتساب این فاکتور : " + Math.Abs(X).ToString("n0") + " ریال" + (X < 0 ? " بستانکار " : (X > 0 ? " بدهکار " : " ")) + Environment.NewLine +
                                    " مانده حساب با احتساب این فاکتور: " + Math.Abs(Y).ToString("n0") + " ریال" + (Y < 0 ? " بستانکار " : (Y > 0 ? " بدهکار " : " "));
                        }



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


            Org = Class_BasicOperation.LogoTable();
            if (_PrintStyle == 26 || _PrintStyle == 27)
            {
                stiViewerControl1.Visible = true;
                crystalReportViewer1.Visible = false;
            }

            else
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
            }

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
                case 27: btn_27_Click(sender, e);
                    StiOptions.Viewer.ViewerTitleText = "";
                    break;
                case 28: btn_30_Click(sender, e);
                    break;
                case 29: bt_custom14_Click(sender, e);
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
                _PrintStyle = 14;
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
            try
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
            catch
            {
            }
        }

        private void bt_18_Click(object sender, EventArgs e)
        {
            try
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
            catch
            {
            }
        }

        private void bt_19_Click(object sender, EventArgs e)
        {
            try
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
            catch
            {
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
                stiViewerControl1.Visible = true;
                crystalReportViewer1.Visible = false;
                this.Cursor = Cursors.WaitCursor;

                _PrintStyle = 26;


                StiReport stireport = new StiReport();
                stireport.Load("LCustomReport.mrt");
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
                    stireport["P1Tel"] = "";
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
                if (List.Count > 0)
                {

                    stireport.Print(showPrintDialog: false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Cursor = Cursors.Default;

            }

        }



        private void btn_Design_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("LCustomReport.mrt");
            stireport.Design();
        }

        private void rb_goodname_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_goodname.Checked)
            {
                Properties.Settings.Default.printorder = 2;
                Properties.Settings.Default.Save();
                bt_Display_Click(null, null);
            }
        }

        private void rb_goodCode_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_goodCode.Checked)
            {
                Properties.Settings.Default.printorder = 1;
                Properties.Settings.Default.Save();
                bt_Display_Click(null, null);
            }
        }

        private void rb_insertorder_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_insertorder.Checked)
            {
                Properties.Settings.Default.printorder = 0;
                Properties.Settings.Default.Save();
                bt_Display_Click(null, null);
            }
        }

        private void btn_P_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("PCustomReport.mrt");
            stireport.Design();
        }

        private void btn_27_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = true;
                crystalReportViewer1.Visible = false;
                this.Cursor = Cursors.WaitCursor;

                _PrintStyle = 27;


                StiReport stireport = new StiReport();
                stireport.Load("PCustomReport.mrt");
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
                    stireport["P1Tel"] = "";
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
                if (List.Count > 0)
                {

                    stireport.Print(showPrintDialog: false);
                }

            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
                this.Cursor = Cursors.Default;

            }

        }

        private void btn_30_Click(object sender, EventArgs e)
        {

            crystalReportViewer1.Visible = true;
            this.Cursor = Cursors.WaitCursor;
            _05_Sale.Reports.Rpt_02_SaleFactor14 Rpt1 = new Rpt_02_SaleFactor14();
            //TextObject Type1 = (TextObject)Rpt1.ReportDefinition.ReportObjects["Text31"];
            //TextObject Type2 = (TextObject)Rpt1.ReportDefinition.ReportObjects["Text35"];
            //TextObject Type3 = (TextObject)Rpt1.ReportDefinition.ReportObjects["Text36"];

            //DataTable PriceTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_002_SalesTypes");
            //Type1.Text = PriceTable.Rows[0]["Column02"].ToString();
            //Type2.Text = PriceTable.Rows[1]["Column02"].ToString();
            //Type3.Text = PriceTable.Rows[2]["Column02"].ToString();

            if (!chk_Logo.Checked)
            {
                Rpt1.Subreports["Logo"].SetDataSource(Org);
                Rpt1.Subreports["Company"].SetDataSource(Org);
            }
            else
            {
                Rpt1.Subreports["Logo"].SetDataSource(Org.Clone());
                Rpt1.Subreports["Company"].SetDataSource(Org.Clone());
            }

            Rpt1.SetDataSource(HeaderTable);
            Rpt1.Subreports["X1"].SetDataSource(DetailTable);

            if (!chk_Logo.Checked)
            {

                Rpt1.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt1.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt1.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt1.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt1.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt1.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt1.SetParameterValue("P1Name", " ");
                Rpt1.SetParameterValue("P1ECode", " ");
                Rpt1.SetParameterValue("P1NCode", " ");
                Rpt1.SetParameterValue("P1Address", " ");
                Rpt1.SetParameterValue("P1Tel", " ");
                Rpt1.SetParameterValue("P1PostalCode", " ");
            }
            Rpt1.SetParameterValue("Param3", Sign[0]);
            Rpt1.SetParameterValue("Param4", Sign[1]);
            Rpt1.SetParameterValue("Param5", Sign[2]);
            Rpt1.SetParameterValue("Param6", Sign[3]);
            Rpt1.SetParameterValue("Param7", Sign[4]);
            Rpt1.SetParameterValue("Param8", Sign[5]);
            Rpt1.SetParameterValue("Param9", Sign[6]);
            Rpt1.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = Rpt1;
            this.Cursor = Cursors.Default;
            _PrintStyle = 28;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt1.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt1.PrintToPrinter(1, true, 1, 1000);
            }
        }

        private void btn__27_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.Visible = true;
            this.Cursor = Cursors.WaitCursor;
            _05_Sale.Reports.Rpt_02_SaleFactor_31 Rpt1 = new Rpt_02_SaleFactor_31();
            //TextObject Type1 = (TextObject)Rpt1.ReportDefinition.ReportObjects["Text31"];
            //TextObject Type2 = (TextObject)Rpt1.ReportDefinition.ReportObjects["Text35"];
            //TextObject Type3 = (TextObject)Rpt1.ReportDefinition.ReportObjects["Text36"];

            //DataTable PriceTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_002_SalesTypes");
            //Type1.Text = PriceTable.Rows[0]["Column02"].ToString();
            //Type2.Text = PriceTable.Rows[1]["Column02"].ToString();
            //Type3.Text = PriceTable.Rows[2]["Column02"].ToString();

            if (!chk_Logo.Checked)
            {
                Rpt1.Subreports["Logo"].SetDataSource(Org);
                Rpt1.Subreports["Company"].SetDataSource(Org);
            }
            else
            {
                Rpt1.Subreports["Logo"].SetDataSource(Org.Clone());
                Rpt1.Subreports["Company"].SetDataSource(Org.Clone());
            }

            Rpt1.SetDataSource(HeaderTable);
            if (!chk_Logo.Checked)
            {

                Rpt1.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt1.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt1.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt1.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt1.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt1.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt1.SetParameterValue("P1Name", " ");
                Rpt1.SetParameterValue("P1ECode", " ");
                Rpt1.SetParameterValue("P1NCode", " ");
                Rpt1.SetParameterValue("P1Address", " ");
                Rpt1.SetParameterValue("P1Tel", " ");
                Rpt1.SetParameterValue("P1PostalCode", " ");
            }
            Rpt1.SetParameterValue("Param3", Sign[0]);
            Rpt1.SetParameterValue("Param4", Sign[1]);
            Rpt1.SetParameterValue("Param5", Sign[2]);
            Rpt1.SetParameterValue("Param6", Sign[3]);
            Rpt1.SetParameterValue("Param7", Sign[4]);
            Rpt1.SetParameterValue("Param8", Sign[5]);
            Rpt1.SetParameterValue("Param9", Sign[6]);
            Rpt1.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = Rpt1;
            this.Cursor = Cursors.Default;
            _PrintStyle = 28;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt1.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt1.PrintToPrinter(1, true, 1, 1000);
            }
        }

        private void bt_custom14_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = true;
                crystalReportViewer1.Visible = false;
                this.Cursor = Cursors.WaitCursor;

                _PrintStyle = 29;


                StiReport stireport = new StiReport();
                stireport.Load("Rpt_02_SaleFactor14.mrt");
                stireport.Pages["Page1"].Enabled = true;
                stireport.Compile();
                StiOptions.Viewer.AllowUseDragDrop = false;
                stireport.RegData("Rpt_SaleTable", HeaderTable);
                stireport.RegData("Rpt_SaleExtra_Table", DetailTable);
                if (!chk_Logo.Checked)
                {

                    stireport.RegData("Table_000_OrgInfo", Org);

                }
                else
                {
                    stireport.RegData("Table_000_OrgInfo", Org.Clone());
                }



                if (!chk_Logo.Checked)
                {

                    stireport["P1Name"] = Org.Rows[0]["Column01"].ToString();
                    stireport["P1ECode"] = Org.Rows[0]["Column06"].ToString();
                    stireport["P1NCode"] = Org.Rows[0]["Column07"].ToString();
                    stireport["P1Address"] = Org.Rows[0]["Column02"].ToString();
                    stireport["P1Tel"] = Org.Rows[0]["Column03"].ToString();
                    stireport["P1PostalCode"] = Org.Rows[0]["Column14"].ToString();
                }
                else
                {
                    stireport["P1Name"] = " ";
                    stireport["P1ECode"] = " ";
                    stireport["P1NCode"] = " ";
                    stireport["P1Address"] = " ";
                    stireport["P1Tel"] = " ";
                    stireport["P1PostalCode"] = " ";
                }



                stireport["Param3"] = Sign[0];
                stireport["Param4"] = Sign[1];
                stireport["Param5"] = Sign[2];
                stireport["Param6"] = Sign[3];
                stireport["Param7"] = Sign[4];
                stireport["Param8"] = Sign[5];
                stireport["Param9"] = Sign[6];
                stireport["Param10"] = Sign[7];

                this.Cursor = Cursors.Default;



                stireport.IsSelected = true;
                stireport.Select();
                //stireport.Show();
                stireport.Render(false);
                stiViewerControl1.Report = stireport;
                stiViewerControl1.Refresh();
                if (List.Count > 0)
                {

                    stireport.Print(showPrintDialog: false);
                }

            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
                this.Cursor = Cursors.Default;

            }
        }

        private void btn_14design_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("Rpt_02_SaleFactor14.mrt");
            stireport.Design();
        }







    }
}