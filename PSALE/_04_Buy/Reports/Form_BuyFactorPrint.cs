using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.Xml;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Design;
using Stimulsoft.Database;
using Stimulsoft.Report.Dictionary;
namespace PSHOP._04_Buy.Reports
{
    public partial class Form_BuyFactorPrint : DevComponents.DotNetBar.OfficeForm
    {
        short _StyleNumber = 1;
        int _barcode = 1;

        Classes.Class_Settle cl_Settle = new Classes.Class_Settle();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConMAIN = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConACNT = new SqlConnection(Properties.Settings.Default.ACNT);

        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false, _Cust = false;
        string _BuyNumber = "";
        string[] Sign;
        DataTable Org;
        DataTable DetailTable;
        DataTable HeaderTable;
        DataTable _dt;
        int _customer = 0; string _date = string.Empty; string _todate = string.Empty;
        bool Isadmin = false;
        Int16 projectId;
        public Form_BuyFactorPrint(string BuyNumber)
        {
            InitializeComponent();
            _BuyNumber = BuyNumber;
            btn_barcode.Visible = false;
            uiGroupBox1.Visible = true;
            chk_Logo.Visible = true;

        }
        public Form_BuyFactorPrint(int customer, string date, string todate)
        {
            InitializeComponent();
            _customer = customer;
            _date = date;
            _todate = todate;
            _Cust = true;
            btn_barcode.Visible = false;
            uiGroupBox1.Visible = false;
            chk_Logo.Visible = true;

        }
        public Form_BuyFactorPrint(DataTable dt)
        {
            InitializeComponent();
            _dt = dt;
            _StyleNumber = 6;
            btn_barcode.Visible = true;
            uiGroupBox1.Visible = false;
            chk_Logo.Visible = false;
        }
        private void bt_Display_Click(object sender, EventArgs e)
        {
            if (_StyleNumber != 6)
            {
                string HeaderSelectText = @"SELECT FactorTable.Serial,
       FactorTable.Date,
       FactorTable.Responsible,
       FactorTable.P2Name,
        FactorTable.P2NationalCode,
       FactorTable.P2ECode,
       FactorTable.P2SabtCode,
       FactorTable.P2Address,
       FactorTable.P2Tel,
       FactorTable.P2Fax,
       FactorTable.P2PostalCode,
       FactorTable.P2Code,
       FactorTable.GoodCode,
       FactorTable.GoodName,
       FactorTable.Box,
       FactorTable.BoxPrice,
       FactorTable.Pack,
       FactorTable.PackPrice,
       FactorTable.Number,
       FactorTable.TotalNumber,
       FactorTable.SinglePrice,
       FactorTable.TotalPrice,
       FactorTable.DiscountPercent,
       FactorTable.DiscountPrice,
       FactorTable.TaxPrice,
       FactorTable.NetPrice,
       FactorTable.NetTotal,
       ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
       ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
       PersonInfoTable.Column02,
       FactorTable.Description,
       FactorTable.CountUnitName,
       'Zero' AS CharPrice,
       FactorType,
       Zarib,
        GoodDesc,FactorTable.SingelWeight,FactorTable.TotalWeight,
       FactorTable.InsertUser,
       FactorTable.ConfirmtUser,
       FactorTable.InsertUserSign,
       FactorTable.ConfirmUserSign,FactorTable.Sanad,
       FactorTable.endpricesale,
	   FactorTable.Description2

FROM   (
           SELECT dbo.Table_015_BuyFactor.columnid AS FactorID,
                  dbo.Table_015_BuyFactor.column01 AS Serial,
                  dbo.Table_015_BuyFactor.column02 AS Date,
                  PersonTable.Column02 AS P2Name,
                 PersonTable.Column09 AS P2NationalCode,
                       PersonTable.Column141 AS P2ECode,
                       PersonTable.Column142 AS P2SabtCode,
                  PersonTable.Column01 AS P2Code,
                  PersonTable.Column06 AS P2Address,
                  PersonTable.Column07 AS P2Tel,
                  PersonTable.Column08 AS P2Fax,
                  PersonTable.Column13 AS P2PostalCode,
                  GoodTable.column01 AS GoodCode,
                  dbo.Table_016_Child1_BuyFactor.column04 AS Box,
                  dbo.Table_016_Child1_BuyFactor.column08 AS BoxPrice,
                  dbo.Table_016_Child1_BuyFactor.column05 AS Pack,
                  dbo.Table_016_Child1_BuyFactor.column09 AS PackPrice,
                  dbo.Table_016_Child1_BuyFactor.column06 AS Number,
                  dbo.Table_016_Child1_BuyFactor.column07 AS TotalNumber,
                  dbo.Table_016_Child1_BuyFactor.column10 AS SinglePrice,
                  dbo.Table_016_Child1_BuyFactor.column11 AS TotalPrice,
                  dbo.Table_016_Child1_BuyFactor.column16 AS DiscountPercent,
                  dbo.Table_016_Child1_BuyFactor.column17 AS DiscountPrice,
                  dbo.Table_016_Child1_BuyFactor.column19 AS TaxPrice,
                  dbo.Table_016_Child1_BuyFactor.column20 AS NetPrice,

                  dbo.Table_016_Child1_BuyFactor.Column34 AS SingelWeight,
                  dbo.Table_016_Child1_BuyFactor.Column35 AS TotalWeight,
				  dbo.Table_016_Child1_BuyFactor.Column38 AS endpricesale,
				   dbo.Table_016_Child1_BuyFactor.Column23 AS Description2,

                  dbo.Table_015_BuyFactor.Column20 AS NetTotal,
                  GoodTable.column02 AS GoodName,
                  OtherPrice.PlusPrice AS Ezafat,
                  OtherPrice.MinusPrice AS Kosoorat,
                  dbo.Table_015_BuyFactor.column14 AS Responsible,
                  dbo.Table_015_BuyFactor.column04 AS DESCRIPTION,
                  CountUnitTable.Column01 AS CountUnitName,
                  CASE 
                       WHEN Table_015_BuyFactor.Column15 = 0 THEN 
                            '*** ريالي ***'
                       ELSE '*** ارزي ***'
                  END AS FactorType,
                  Table_016_Child1_BuyFactor.Column31 AS Zarib,
                    dbo.Table_016_Child1_BuyFactor.column23 as GoodDesc, 
                    dbo.Table_015_BuyFactor.column05 as InsertUser,
                   (case when dbo.Table_015_BuyFactor.Column31 = 1 then  dbo.Table_015_BuyFactor.Column32 else '' end) as ConfirmtUser,
                    (select top 1 Column39 from " + ConMAIN.Database + @".dbo.Table_010_UserInfo where Column00=dbo.Table_015_BuyFactor.column05  and  Column05=" + Class_BasicOperation._OrgCode + @" and  Column06=N'" + Class_BasicOperation._Year + @"' ) as InsertUserSign,
                    ( case when dbo.Table_015_BuyFactor.Column31 = 1 then (select top 1 Column39 from " + ConMAIN.Database + @".dbo.Table_010_UserInfo where Column00=dbo.Table_015_BuyFactor.Column32  and  Column05=" + Class_BasicOperation._OrgCode + @" and  Column06=N'" + Class_BasicOperation._Year + @"' ) else null end ) as ConfirmUserSign
                       ,isnull((select Column00 from " + ConACNT.Database + @".dbo.Table_060_SanadHead where ColumnId =Table_015_BuyFactor.column11 ),0) as Sanad

                  ,dbo.Table_015_BuyFactor.Column29

           FROM   dbo.Table_015_BuyFactor
                  INNER JOIN dbo.Table_016_Child1_BuyFactor
                       ON  dbo.Table_015_BuyFactor.columnid = dbo.Table_016_Child1_BuyFactor.column01
                  INNER JOIN (
                           SELECT ColumnId,
                                  Column00,
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
                                  Column13,
                                  Column141,
                                       Column142
                           FROM   {0}.dbo.Table_045_PersonInfo
                       ) AS PersonTable
                       ON  dbo.Table_015_BuyFactor.column03 = PersonTable.ColumnId
                  LEFT OUTER JOIN (
                           SELECT columnid,
                                  SUM(PlusPrice) AS PlusPrice,
                                  SUM(MinusPrice) AS MinusPrice
                           FROM   (
                                      SELECT Table_015_BuyFactor_2.columnid,
                                             SUM(dbo.Table_017_Child2_BuyFactor.column04) AS 
                                             PlusPrice,
                                             0 AS MinusPrice
                                      FROM   dbo.Table_017_Child2_BuyFactor
                                             INNER JOIN dbo.Table_015_BuyFactor AS 
                                                  Table_015_BuyFactor_2
                                                  ON  dbo.Table_017_Child2_BuyFactor.column01 = 
                                                      Table_015_BuyFactor_2.columnid
                                      WHERE  (dbo.Table_017_Child2_BuyFactor.column05 = 0)
                                      GROUP BY
                                             Table_015_BuyFactor_2.columnid,
                                             dbo.Table_017_Child2_BuyFactor.column05
                                      UNION ALL
                                      SELECT Table_015_BuyFactor_1.columnid,
                                             0 AS PlusPrice,
                                             SUM(Table_017_Child2_BuyFactor_1.column04) AS 
                                             MinusPrice
                                      FROM   dbo.Table_017_Child2_BuyFactor AS 
                                             Table_017_Child2_BuyFactor_1
                                             INNER JOIN dbo.Table_015_BuyFactor AS 
                                                  Table_015_BuyFactor_1
                                                  ON  
                                                      Table_017_Child2_BuyFactor_1.column01 = 
                                                      Table_015_BuyFactor_1.columnid
                                      WHERE  (Table_017_Child2_BuyFactor_1.column05 = 1)
                                      GROUP BY
                                             Table_015_BuyFactor_1.columnid,
                                             Table_017_Child2_BuyFactor_1.column05
                                  ) AS OtherPrice_1
                           GROUP BY
                                  columnid
                       ) AS OtherPrice
                       ON  dbo.Table_015_BuyFactor.columnid = OtherPrice.columnid
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
                       ON  dbo.Table_016_Child1_BuyFactor.column02 = GoodTable.columnid
                  LEFT OUTER JOIN (
                           SELECT Column00,
                                  Column01
                           FROM   {0}.dbo.Table_070_CountUnitInfo
                       ) AS CountUnitTable
                       ON  dbo.Table_016_Child1_BuyFactor.column03 = 
                           CountUnitTable.Column00
       ) AS FactorTable
       LEFT OUTER JOIN (
                SELECT ColumnId,
                       Column01,
                       Column02
                FROM   {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1
            ) AS PersonInfoTable
            ON  FactorTable.Responsible = PersonInfoTable.ColumnId";
                HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);

                string DetailSelectText = @"SELECT     dbo.Table_024_Discount_Buy.column01 AS Name, dbo.Table_017_Child2_BuyFactor.column04 AS Price, 
                      CASE WHEN Table_024_Discount_Buy.column02 = 0 THEN '+' ELSE '-' END AS Type, dbo.Table_017_Child2_BuyFactor.column01 AS HeaderId, 
                      dbo.Table_015_BuyFactor.column01 AS HeaderNum, dbo.Table_015_BuyFactor.column02 AS HeaderDate
                      FROM         dbo.Table_017_Child2_BuyFactor INNER JOIN
                      dbo.Table_015_BuyFactor ON dbo.Table_017_Child2_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid LEFT OUTER JOIN
                      dbo.Table_024_Discount_Buy ON dbo.Table_017_Child2_BuyFactor.column02 = dbo.Table_024_Discount_Buy.columnid";


                if (rdb_CurrentNumber.Checked)
                {
                    HeaderSelectText += " WHERE     (FactorTable.Serial in ( " + _BuyNumber.TrimEnd(',') + ")) and (FactorTable.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')";
                    DetailSelectText += " WHERE (Table_015_BuyFactor.Column01 in ( " + _BuyNumber.TrimEnd(',') + ")) and (Table_015_BuyFactor.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')";
                }
                else if (rdb_FromNumber.Checked && txt_From.Text.Trim() != "" && txt_To.Text.Trim() != "")
                {
                    HeaderSelectText += " WHERE     (FactorTable.Serial between  " + txt_From.Text + " and " + txt_To.Text + ") and (FactorTable.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')";
                    DetailSelectText += @" WHERE (Table_015_BuyFactor.Column01 between " + txt_From.Text + " and " + txt_To.Text + ") and (Table_015_BuyFactor.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')";
                }
                else if (rdb_Date.Checked && faDatePicker1.SelectedDateTime.HasValue && faDatePicker2.SelectedDateTime.HasValue
                    && faDatePicker1.Text.Length == 10 && faDatePicker2.Text.Length == 10)
                {
                    HeaderSelectText += " WHERE     (FactorTable.Date between  '" + faDatePicker1.Text + "' and '" + faDatePicker2.Text + "') and (FactorTable.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')";
                    DetailSelectText += @" WHERE (Table_015_BuyFactor.Column02 between '" + faDatePicker1.Text + "' and '" + faDatePicker2.Text + "') and (Table_015_BuyFactor.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')";
                }

                HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
                foreach (DataRow item in HeaderTable.Rows)
                {
                    double FinalPrice = Convert.ToDouble(item["NetTotal"].ToString()) +
                        Convert.ToDouble(item["Ezafat"].ToString()) - Convert.ToDouble(item["Kosoorat"].ToString());

                    item["CharPrice"] = FarsiLibrary.Utils.ToWords.ToString(long.Parse(Math.Round(FinalPrice, 0).ToString()));
                }

                DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);


                //فراخوانی عناوین امضا
                Sign = clDoc.Signature(9);

                Org = Class_BasicOperation.LogoTable();


                switch (_StyleNumber)
                {
                    case 1:
                        bt_First_Click(sender, e);
                        break;
                    case 2:
                        bt_Second_Click(sender, e);
                        break;
                    case 3:
                        bt_Third_Click(sender, e);
                        break;
                    case 4:
                        bt_4_Click(sender, e);
                        break;
                    case 5:
                        btn_Custom_Click(sender, e);
                        break;
                    case 6:
                        btn_barcodeprint_Click(sender, e);
                        break;

                }
            }
            else
            {
                switch (_barcode)
                {
                    case 1:
                        btn_barcodeprint_Click(sender, e);
                        break;
                    case 2:
                        btn_barcodeprint2_Click(sender, e);
                        break;
                    case 3:
                        btn_barcodeprint3_Click(sender, e);

                        break;
                    case 4:
                        btn_barcodeprint4_Click(sender, e);

                        break;
                }
            }

        }
        private void bt_TotalDisplay_Click(object sender, EventArgs e)
        {

            string HeaderSelectText = @"SELECT MAX(FactorTable.Serial)  Serial,
        MAX(FactorTable.FactorID) as ID,
       FactorTable.Date,
       '' Responsible,
       FactorTable.CustomerID,
       FactorTable.P2Name,
       FactorTable.P2NationalCode,
       FactorTable.P2ECode,
       FactorTable.P2SabtCode,
       FactorTable.P2Address,
       FactorTable.P2Tel,
       FactorTable.P2Fax,
       FactorTable.P2PostalCode,
       FactorTable.P2Code,
       FactorTable.GoodCode,
       FactorTable.GoodName,
       0 Box,
       0 BoxPrice,
       0 Pack,
       0 PackPrice,
       SUM(FactorTable.Number) Number,
       SUM(FactorTable.TotalNumber) TotalNumber,
       FactorTable.SinglePrice,
       SUM(FactorTable.TotalPrice) TotalPrice,
       SUM(FactorTable.DiscountPercent) DiscountPercent,
       SUM(FactorTable.DiscountPrice) DiscountPrice,
       SUM(FactorTable.TaxPrice) TaxPrice,
       SUM(FactorTable.NetPrice) NetPrice,
       SUM(FactorTable.NetTotal)NetTotal,
       SUM(ISNULL(FactorTable.Ezafat, 0)) AS Ezafat,
       SUM(ISNULL(FactorTable.Kosoorat, 0)) AS Kosoorat,
       '' Column02,
       '' Description,
       FactorTable.CountUnitName,
       'Zero' AS CharPrice,
       '***فاکتور ریالی***' FactorType,
       MAX(FactorTable.Zarib) Zarib,
       '' GoodDesc,
       FactorTable.SingelWeight,
       SUM(FactorTable.TotalWeight)  TotalWeight,
       '' InsertUser,
       '' ConfirmtUser,
       null InsertUserSign,
       null ConfirmUserSign,
       0 Sanad,
       0 endpricesale,
       '' Description2

FROM   (
           SELECT dbo.Table_015_BuyFactor.columnid AS FactorID,
                  dbo.Table_015_BuyFactor.column01 AS Serial,
                  dbo.Table_015_BuyFactor.column02 AS Date,
                 dbo.Table_015_BuyFactor.column03 AS CustomerID,

                  PersonTable.Column02 AS P2Name,
                 PersonTable.Column09 AS P2NationalCode,
                       PersonTable.Column141 AS P2ECode,
                       PersonTable.Column142 AS P2SabtCode,
                  PersonTable.Column01 AS P2Code,
                  PersonTable.Column06 AS P2Address,
                  PersonTable.Column07 AS P2Tel,
                  PersonTable.Column08 AS P2Fax,
                  PersonTable.Column13 AS P2PostalCode,
                  GoodTable.column01 AS GoodCode,
                  dbo.Table_016_Child1_BuyFactor.column04 AS Box,
                  dbo.Table_016_Child1_BuyFactor.column08 AS BoxPrice,
                  dbo.Table_016_Child1_BuyFactor.column05 AS Pack,
                  dbo.Table_016_Child1_BuyFactor.column09 AS PackPrice,
                  dbo.Table_016_Child1_BuyFactor.column06 AS Number,
                  dbo.Table_016_Child1_BuyFactor.column07 AS TotalNumber,
                  dbo.Table_016_Child1_BuyFactor.column10 AS SinglePrice,
                  dbo.Table_016_Child1_BuyFactor.column11 AS TotalPrice,
                  dbo.Table_016_Child1_BuyFactor.column16 AS DiscountPercent,
                  dbo.Table_016_Child1_BuyFactor.column17 AS DiscountPrice,
                  dbo.Table_016_Child1_BuyFactor.column19 AS TaxPrice,
                  dbo.Table_016_Child1_BuyFactor.column20 AS NetPrice,

                  dbo.Table_016_Child1_BuyFactor.Column34 AS SingelWeight,
                  dbo.Table_016_Child1_BuyFactor.Column35 AS TotalWeight,
				  dbo.Table_016_Child1_BuyFactor.Column38 AS endpricesale,
				   dbo.Table_016_Child1_BuyFactor.Column23 AS Description2,

                  dbo.Table_015_BuyFactor.Column20 AS NetTotal,
                  GoodTable.column02 AS GoodName,
                  OtherPrice.PlusPrice AS Ezafat,
                  OtherPrice.MinusPrice AS Kosoorat,
                  dbo.Table_015_BuyFactor.column14 AS Responsible,
                  dbo.Table_015_BuyFactor.column04 AS DESCRIPTION,
                  CountUnitTable.Column01 AS CountUnitName,
                  CASE 
                       WHEN Table_015_BuyFactor.Column15 = 0 THEN 
                            '*** ريالي ***'
                       ELSE '*** ارزي ***'
                  END AS FactorType,
                  Table_016_Child1_BuyFactor.Column31 AS Zarib,
                    dbo.Table_016_Child1_BuyFactor.column23 as GoodDesc, 
                    dbo.Table_015_BuyFactor.column05 as InsertUser,
                   (case when dbo.Table_015_BuyFactor.Column31 = 1 then  dbo.Table_015_BuyFactor.Column32 else '' end) as ConfirmtUser,
                    (select top 1 Column39 from " + ConMAIN.Database + @".dbo.Table_010_UserInfo where Column00=dbo.Table_015_BuyFactor.column05  and  Column05=" + Class_BasicOperation._OrgCode + @" and  Column06=N'" + Class_BasicOperation._Year + @"' ) as InsertUserSign,
                    ( case when dbo.Table_015_BuyFactor.Column31 = 1 then (select top 1 Column39 from " + ConMAIN.Database + @".dbo.Table_010_UserInfo where Column00=dbo.Table_015_BuyFactor.Column32  and  Column05=" + Class_BasicOperation._OrgCode + @" and  Column06=N'" + Class_BasicOperation._Year + @"' ) else null end ) as ConfirmUserSign
                       ,isnull((select Column00 from " + ConACNT.Database + @".dbo.Table_060_SanadHead where ColumnId =Table_015_BuyFactor.column11 ),0) as Sanad

                  ,dbo.Table_015_BuyFactor.Column29  

           FROM   dbo.Table_015_BuyFactor
                  INNER JOIN dbo.Table_016_Child1_BuyFactor
                       ON  dbo.Table_015_BuyFactor.columnid = dbo.Table_016_Child1_BuyFactor.column01
                  INNER JOIN (
                           SELECT ColumnId,
                                  Column00,
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
                                  Column13,
                                  Column141,
                                       Column142
                           FROM   {0}.dbo.Table_045_PersonInfo
                       ) AS PersonTable
                       ON  dbo.Table_015_BuyFactor.column03 = PersonTable.ColumnId
                  LEFT OUTER JOIN (
                           SELECT columnid,
                                  SUM(PlusPrice) AS PlusPrice,
                                  SUM(MinusPrice) AS MinusPrice
                           FROM   (
                                      SELECT Table_015_BuyFactor_2.columnid,
                                             SUM(dbo.Table_017_Child2_BuyFactor.column04) AS 
                                             PlusPrice,
                                             0 AS MinusPrice
                                      FROM   dbo.Table_017_Child2_BuyFactor
                                             INNER JOIN dbo.Table_015_BuyFactor AS 
                                                  Table_015_BuyFactor_2
                                                  ON  dbo.Table_017_Child2_BuyFactor.column01 = 
                                                      Table_015_BuyFactor_2.columnid
                                      WHERE  (dbo.Table_017_Child2_BuyFactor.column05 = 0)
                                      GROUP BY
                                             Table_015_BuyFactor_2.columnid,
                                             dbo.Table_017_Child2_BuyFactor.column05
                                      UNION ALL
                                      SELECT Table_015_BuyFactor_1.columnid,
                                             0 AS PlusPrice,
                                             SUM(Table_017_Child2_BuyFactor_1.column04) AS 
                                             MinusPrice
                                      FROM   dbo.Table_017_Child2_BuyFactor AS 
                                             Table_017_Child2_BuyFactor_1
                                             INNER JOIN dbo.Table_015_BuyFactor AS 
                                                  Table_015_BuyFactor_1
                                                  ON  
                                                      Table_017_Child2_BuyFactor_1.column01 = 
                                                      Table_015_BuyFactor_1.columnid
                                      WHERE  (Table_017_Child2_BuyFactor_1.column05 = 1)
                                      GROUP BY
                                             Table_015_BuyFactor_1.columnid,
                                             Table_017_Child2_BuyFactor_1.column05
                                  ) AS OtherPrice_1
                           GROUP BY
                                  columnid
                       ) AS OtherPrice
                       ON  dbo.Table_015_BuyFactor.columnid = OtherPrice.columnid
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
                       ON  dbo.Table_016_Child1_BuyFactor.column02 = GoodTable.columnid
                  LEFT OUTER JOIN (
                           SELECT Column00,
                                  Column01
                           FROM   {0}.dbo.Table_070_CountUnitInfo
                       ) AS CountUnitTable
                       ON  dbo.Table_016_Child1_BuyFactor.column03 = 
                           CountUnitTable.Column00
       ) AS FactorTable
       LEFT OUTER JOIN (
                SELECT ColumnId,
                       Column01,
                       Column02
                FROM   {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1
            ) AS PersonInfoTable
            ON  FactorTable.Responsible = PersonInfoTable.ColumnId
WHERE  FactorTable.CustomerID = " + _customer + @"
       AND FactorTable.Date >= '" + _date + @"' AND FactorTable.Date <= '" + _todate + @"'
            and (FactorTable.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')
            GROUP BY  FactorTable.GoodCode,
       FactorTable.GoodName,
              FactorTable.Date,
 FactorTable.P2Name,
       FactorTable.P2NationalCode,
       FactorTable.P2ECode,
       FactorTable.P2SabtCode,
       FactorTable.P2Address,
       FactorTable.CustomerID,

       FactorTable.P2Tel,
       FactorTable.P2Fax,
       FactorTable.P2PostalCode,
       FactorTable.P2Code,
       FactorTable.SinglePrice,FactorTable.CountUnitName,FactorTable.SingelWeight
";
            HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);

            string DetailSelectText = @"SELECT dbo.Table_024_Discount_Buy.column01 AS NAME,
                               SUM(dbo.Table_017_Child2_BuyFactor.column04) AS Price,
                               CASE 
                                    WHEN Table_024_Discount_Buy.column02 = 0 THEN '+'
                                    ELSE '-'
                               END AS TYPE,
                               MAX(dbo.Table_017_Child2_BuyFactor.column01) AS HeaderId,
                               MAX(dbo.Table_015_BuyFactor.column01) AS HeaderNum,
                               '" + _date + @"' AS HeaderDate
                        FROM   dbo.Table_017_Child2_BuyFactor
                               INNER JOIN dbo.Table_015_BuyFactor
                                    ON  dbo.Table_017_Child2_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid
                               LEFT OUTER JOIN dbo.Table_024_Discount_Buy
                                    ON  dbo.Table_017_Child2_BuyFactor.column02 = dbo.Table_024_Discount_Buy.columnid
                        WHERE  Table_015_BuyFactor.column03 = " + _customer + @"
                               AND dbo.Table_015_BuyFactor.column02 >= '" + _date + @"'
                               AND dbo.Table_015_BuyFactor.column02 <= '" + _todate + @"'
                               and (dbo.Table_015_BuyFactor.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')

                        GROUP BY
                               dbo.Table_024_Discount_Buy.column01,
                               Table_024_Discount_Buy.column02";



            

            HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
            

            DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);
            if (HeaderTable.Rows.Count == 0)
            {
                MessageBox.Show("اطلاعاتی برای چاپ وجود ندارد");
                this.Close();
                this.Cursor = Cursors.Default;
                return;
            }
            int id = Convert.ToInt32(HeaderTable.Rows[0]["ID"]);
            int serial = Convert.ToInt32(HeaderTable.Rows[0]["Serial"]);
            foreach (DataRow dr in HeaderTable.Rows)
            {
                dr.BeginEdit();
                dr["ID"] = id;
                dr["Serial"] = serial;
                dr.EndEdit();
            }
            foreach (DataRow dr in DetailTable.Rows)
            {
                dr.BeginEdit();
                dr["Column01"] = id;
                dr["HeaderNum"] = serial;
                dr["HeaderDate"] = _date;
                dr.EndEdit();
            }

            //فراخوانی عناوین امضا
            Sign = clDoc.Signature(9);

            Org = Class_BasicOperation.LogoTable();
            switch (_StyleNumber)
            {
                case 1:
                    bt_First_Click(sender, e);
                    break;
                case 2:
                    bt_Second_Click(sender, e);
                    break;
                case 3:
                    bt_Third_Click(sender, e);
                    break;
                case 4:
                    bt_4_Click(sender, e);
                    break;
                case 5:
                    btn_Custom_Click(sender, e);
                    break;
                case 6:
                    btn_barcodeprint_Click(sender, e);
                    break;

            }




        }

        private void Form_FactorPrint_Load(object sender, EventArgs e)
        {

            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.MAIN))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("Select Column02 from Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" +
                Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + "'", Con);
                Isadmin = (bool.Parse(Select.ExecuteScalar().ToString()));

            }

            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT tsi.Column05
                                                                        FROM   dbo.Table_295_StoreInfo AS tsi
                                                                               JOIN dbo.Table_296_StoreUsers AS tsu
                                                                                    ON  tsu.Column00 = tsi.ColumnId
                                                                        WHERE tsu.Column01='" + Class_BasicOperation._UserName + "'", ConBase);
            DataTable StoreTable = new DataTable();

            Adapter.Fill(StoreTable);

            if (!Isadmin && StoreTable.Rows.Count == 0) { Class_BasicOperation.ShowMsg("", "کاربر گرامی، فروشگاه شما تعیین نشده است و امکان کار با این فرم را ندارید ", "Stop"); this.Dispose(); }

            else if (StoreTable.Rows.Count > 0) projectId = Convert.ToInt16(StoreTable.Rows[0]["Column05"]);

            if (!_Cust)
            {
                faDatePicker1.SelectedDateTime = DateTime.Now.AddMonths(-1);
                faDatePicker2.SelectedDateTime = DateTime.Now;
                if (_StyleNumber != 6)
                    _StyleNumber = Properties.Settings.Default.BuyFactorStyle;
                else
                    _barcode = Properties.Settings.Default.barcodeprint;
                bt_Display_Click(sender, e);
            }
            else
            {
                chk_Logo.Checked = Properties.Settings.Default.DontShowLogo;
                bt_TotalDisplay_Click(sender, e);
            }
        }

        private void txt_From_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
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
                    bt_Display.Focus();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
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

        private void bt_First_Click(object sender, EventArgs e)
        {
            //طرح اول
            stiViewerControl1.Visible = false;

            crystalReportViewer1.Visible = true;
            this.Cursor = Cursors.WaitCursor;
            _04_Buy.Reports.Rpt_01_BuyFactor rpt = new Reports.Rpt_01_BuyFactor();

            if (!chk_Logo.Checked)
                rpt.Subreports[0].SetDataSource(Org);
            else rpt.Subreports[0].SetDataSource(Org.Clone());

            rpt.SetDataSource(HeaderTable);
            rpt.Subreports[1].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                rpt.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                rpt.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                rpt.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                rpt.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                rpt.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                rpt.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                rpt.SetParameterValue("P1Name", " ");
                rpt.SetParameterValue("P1ECode", " ");
                rpt.SetParameterValue("P1NCode", " ");
                rpt.SetParameterValue("P1Address", " ");
                rpt.SetParameterValue("P1Tel", " ");
                rpt.SetParameterValue("P1PostalCode", " ");
            }

            rpt.SetParameterValue("Param3", Sign[0]);
            rpt.SetParameterValue("Param4", Sign[1]);
            rpt.SetParameterValue("Param5", Sign[2]);
            rpt.SetParameterValue("Param6", Sign[3]);
            rpt.SetParameterValue("Param7", Sign[4]);
            rpt.SetParameterValue("Param8", Sign[5]);
            rpt.SetParameterValue("Param9", Sign[6]);
            rpt.SetParameterValue("Param10", Sign[7]);

            crystalReportViewer1.ReportSource = rpt;
            _StyleNumber = 1;
            this.Cursor = Cursors.Default;
        }

        private void bt_Second_Click(object sender, EventArgs e)
        {
            //طرح دوم
            stiViewerControl1.Visible = false;

            crystalReportViewer1.Visible = true;
            this.Cursor = Cursors.WaitCursor;
            _04_Buy.Reports.Rpt_01_BuyFactor2 rpt2 = new Rpt_01_BuyFactor2();
            if (!chk_Logo.Checked)
                rpt2.Subreports[0].SetDataSource(Org);
            else rpt2.Subreports[0].SetDataSource(Org.Clone());

            rpt2.SetDataSource(HeaderTable);
            rpt2.Subreports[1].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                rpt2.SetParameterValue("P1Name", " ");
                rpt2.SetParameterValue("P1ECode", " ");
                rpt2.SetParameterValue("P1NCode", " ");
                rpt2.SetParameterValue("P1Address", " ");
                rpt2.SetParameterValue("P1Tel", " ");
                rpt2.SetParameterValue("P1PostalCode", " ");
            }
            rpt2.SetParameterValue("Param3", Sign[0]);
            rpt2.SetParameterValue("Param4", Sign[1]);
            rpt2.SetParameterValue("Param5", Sign[2]);
            rpt2.SetParameterValue("Param6", Sign[3]);
            rpt2.SetParameterValue("Param7", Sign[4]);
            rpt2.SetParameterValue("Param8", Sign[5]);
            rpt2.SetParameterValue("Param9", Sign[6]);
            rpt2.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = rpt2;
            _StyleNumber = 2;
            this.Cursor = Cursors.Default;
        }

        private void bt_Third_Click(object sender, EventArgs e)
        {
            //طرح عمودی
            stiViewerControl1.Visible = false;

            crystalReportViewer1.Visible = true;
            this.Cursor = Cursors.WaitCursor;
            _04_Buy.Reports.Rpt_01_BuyFactor3 rpt3 = new Rpt_01_BuyFactor3();
            if (!chk_Logo.Checked)
                rpt3.Subreports[0].SetDataSource(Org);
            else rpt3.Subreports[0].SetDataSource(Org.Clone());

            rpt3.SetDataSource(HeaderTable);
            rpt3.Subreports[1].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                rpt3.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                rpt3.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                rpt3.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                rpt3.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                rpt3.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                rpt3.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                rpt3.SetParameterValue("P1Name", " ");
                rpt3.SetParameterValue("P1ECode", " ");
                rpt3.SetParameterValue("P1NCode", " ");
                rpt3.SetParameterValue("P1Address", " ");
                rpt3.SetParameterValue("P1Tel", " ");
                rpt3.SetParameterValue("P1PostalCode", " ");
            }

            rpt3.SetParameterValue("Param3", Sign[0]);
            rpt3.SetParameterValue("Param4", Sign[1]);
            rpt3.SetParameterValue("Param5", Sign[2]);
            rpt3.SetParameterValue("Param6", Sign[3]);
            rpt3.SetParameterValue("Param7", Sign[4]);
            rpt3.SetParameterValue("Param8", Sign[5]);
            rpt3.SetParameterValue("Param9", Sign[6]);
            rpt3.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = rpt3;
            _StyleNumber = 3;
            this.Cursor = Cursors.Default;
        }

        private void Form_BuyFactorPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_StyleNumber != 6)
                Properties.Settings.Default.BuyFactorStyle = _StyleNumber;
            else
                Properties.Settings.Default.barcodeprint = _barcode;
            Properties.Settings.Default.DontShowLogo = chk_Logo.Checked;
            Properties.Settings.Default.Save();
        }

        private void bt_4_Click(object sender, EventArgs e)
        {
            //طرح دوم
            stiViewerControl1.Visible = false;

            crystalReportViewer1.Visible = true;
            this.Cursor = Cursors.WaitCursor;
            _04_Buy.Reports.Rpt_01_BuyFactor4 rpt2 = new Rpt_01_BuyFactor4();
            if (!chk_Logo.Checked)
                rpt2.Subreports[0].SetDataSource(Org);
            else rpt2.Subreports[0].SetDataSource(Org.Clone());

            rpt2.SetDataSource(HeaderTable);
            rpt2.Subreports[1].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                rpt2.SetParameterValue("P1Name", " ");
                rpt2.SetParameterValue("P1ECode", " ");
                rpt2.SetParameterValue("P1NCode", " ");
                rpt2.SetParameterValue("P1Address", " ");
                rpt2.SetParameterValue("P1Tel", " ");
                rpt2.SetParameterValue("P1PostalCode", " ");
            }
            rpt2.SetParameterValue("Param3", Sign[0]);
            rpt2.SetParameterValue("Param4", Sign[1]);
            rpt2.SetParameterValue("Param5", Sign[2]);
            rpt2.SetParameterValue("Param6", Sign[3]);
            rpt2.SetParameterValue("Param7", Sign[4]);
            rpt2.SetParameterValue("Param8", Sign[5]);
            rpt2.SetParameterValue("Param9", Sign[6]);
            rpt2.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = rpt2;
            _StyleNumber = 4;
            this.Cursor = Cursors.Default;
        }

        private void btn_Design_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("Rpt_01_BuyFactor.mrt");
            stireport.Design();
        }

        private void btn_Custom_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = true;
            crystalReportViewer1.Visible = false;
            this.Cursor = Cursors.WaitCursor;
            StiReport stireport = new StiReport();
            stireport.Load("Rpt_01_BuyFactor.mrt");
            stireport.Pages["Page1"].Enabled = true;
            stireport.Compile();
            StiOptions.Viewer.AllowUseDragDrop = false;

            if (!chk_Logo.Checked)
                stireport.RegData("Table_000_OrgInfo", Org);
            else stireport.RegData("Table_000_OrgInfo", Org.Clone());


            stireport.RegData("BuyFactor", HeaderTable);
            stireport.RegData("Rpt_Factor_ExtraReduction", DetailTable);

            if (!chk_Logo.Checked)
            {
                try
                {
                    stireport["P1Name"] = Org.Rows[0]["Column01"].ToString();
                    stireport["P1ECode"] = Org.Rows[0]["Column06"].ToString();
                    stireport["P1NCode"] = Org.Rows[0]["Column07"].ToString();
                    stireport["P1Address"] = Org.Rows[0]["Column02"].ToString();
                    stireport["P1Tel"] = Org.Rows[0]["Column03"].ToString();
                    stireport["P1PostalCode"] = Org.Rows[0]["Column14"].ToString();
                }
                catch { }
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
            //this.Cursor = Cursors.Default;
            //stireport.Render(false);
            //stiViewerControl1.Report = stireport;
            stireport.IsSelected = true;
            stireport.Select();
            //stireport.Show();
            stireport.Render(false);
            stiViewerControl1.Report = stireport;
            stiViewerControl1.Refresh();
            _StyleNumber = 5;
            this.Cursor = Cursors.Default;

        }

        private void btn_barcodedesign_Click(object sender, EventArgs e)
        {

            StiReport stireport = new StiReport();
            stireport.Load("Whrs_Rpt01_Barcode.mrt");
            stireport.Design();
        }

        private void btn_barcodeprint_Click(object sender, EventArgs e)
        {
            _barcode = 1;
            stiViewerControl1.Visible = true;
            crystalReportViewer1.Visible = false;
            this.Cursor = Cursors.WaitCursor;
            StiReport stireport = new StiReport();
            stireport.Load("Whrs_Rpt01_Barcode.mrt");
            stireport.Pages.Items[0].Enabled = true;
            stireport.Pages.Items[1].Enabled = false;
            stireport.Pages.Items[2].Enabled = false;
            stireport.Pages.Items[3].Enabled = false;
            stireport.Compile();
            StiOptions.Viewer.AllowUseDragDrop = false;
            stireport.RegData("Barcode", _dt);
            stireport.IsSelected = true;
            stireport.Select();
            //stireport.Show();
            stireport.Render(false);
            stiViewerControl1.Report = stireport;
            stiViewerControl1.Refresh();
            this.Cursor = Cursors.Default;
        }

        private void btn_barcodeprint2_Click(object sender, EventArgs e)
        {
            _barcode = 2;

            stiViewerControl1.Visible = true;
            crystalReportViewer1.Visible = false;
            this.Cursor = Cursors.WaitCursor;
            StiReport stireport = new StiReport();
            stireport.Load("Whrs_Rpt01_Barcode.mrt");
            stireport.Pages.Items[0].Enabled = false;
            stireport.Pages.Items[1].Enabled = true;
            stireport.Pages.Items[2].Enabled = false;
            stireport.Pages.Items[3].Enabled = false;
            stireport.Compile();
            StiOptions.Viewer.AllowUseDragDrop = false;
            stireport.RegData("Barcode", _dt);
            stireport.IsSelected = true;
            stireport.Select();
            //stireport.Show();
            stireport.Render(false);
            stiViewerControl1.Report = stireport;
            stiViewerControl1.Refresh();
            this.Cursor = Cursors.Default;
        }

        private void btn_barcodeprint3_Click(object sender, EventArgs e)
        {
            _barcode = 3;

            stiViewerControl1.Visible = true;
            crystalReportViewer1.Visible = false;
            this.Cursor = Cursors.WaitCursor;
            StiReport stireport = new StiReport();
            stireport.Load("Whrs_Rpt01_Barcode.mrt");
            stireport.Pages.Items[0].Enabled = false;
            stireport.Pages.Items[1].Enabled = false;
            stireport.Pages.Items[2].Enabled = true;
            stireport.Pages.Items[3].Enabled = false;
            stireport.Compile();
            StiOptions.Viewer.AllowUseDragDrop = false;
            stireport.RegData("Barcode", _dt);
            stireport.IsSelected = true;
            stireport.Select();
            //stireport.Show();
            stireport.Render(false);
            stiViewerControl1.Report = stireport;
            stiViewerControl1.Refresh();
            this.Cursor = Cursors.Default;
        }

        private void btn_barcodeprint4_Click(object sender, EventArgs e)
        {
            _barcode = 4;

            stiViewerControl1.Visible = true;
            crystalReportViewer1.Visible = false;
            this.Cursor = Cursors.WaitCursor;
            StiReport stireport = new StiReport();
            stireport.Load("Whrs_Rpt01_Barcode.mrt");
            stireport.Pages.Items[0].Enabled = false;
            stireport.Pages.Items[1].Enabled = false;
            stireport.Pages.Items[2].Enabled = false;
            stireport.Pages.Items[3].Enabled = true;
            stireport.Compile();
            StiOptions.Viewer.AllowUseDragDrop = false;
            stireport.RegData("Barcode", _dt);
            stireport.IsSelected = true;
            stireport.Select();
            //stireport.Show();
            stireport.Render(false);
            stiViewerControl1.Report = stireport;
            stiViewerControl1.Refresh();
            this.Cursor = Cursors.Default;
        }
    }
}