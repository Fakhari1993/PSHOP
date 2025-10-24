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
    public partial class Form_SaleFactorPrint2 : DevComponents.DotNetBar.OfficeForm
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


        public Form_SaleFactorPrint2(int SaleNumber, bool ClubPrint, short? typ3)
        {
            InitializeComponent();
            _SaleNumber = SaleNumber;
            _ClubPrint = ClubPrint;
            _PrintStyle = typ3 ?? -1;

        }

        public Form_SaleFactorPrint2(List<string> _List, bool ClubPrint)
        {
            InitializeComponent();
            List = _List;
            _ClubPrint = ClubPrint;
        }

        public Form_SaleFactorPrint2(DataTable _HeaderTable, DataTable _DetailTable, bool ClubPrint)
        {
            InitializeComponent();
            HeaderTable = _HeaderTable;
            DetailTable = _DetailTable;
            _ClubPrint = ClubPrint;
            _Agg = true;
        }


        public void Form_FactorPrint_Load(object sender, EventArgs e)
        {

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

            //_PrintStyle= Properties.Settings.Default.SaleFactorStyle;
            mlt_ACC.DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ACC_Code,ACC_Name from AllHeaders()");
            if (Properties.Settings.Default.SaveACCRemain.Trim() != "")
                mlt_ACC.Value = Properties.Settings.Default.SaveACCRemain;


        }

        private void Form_FactorPrint_FormClosing(object sender, FormClosingEventArgs e)
        {


        }

        private void txt_From_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }




        private void bt_Display_Click(object sender, EventArgs e)
        {

            string HeaderSelectText = null;

            HeaderSelectText = @"  SELECT FactorTable.FactorID AS ID,
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
       FactorTable.TotalWeight,
       FactorTable.TaxPercent,
       FactorTable.NetPrice,
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
       FactorTable.NumberInBox,
       FactorTable.RowDesc,
       FactorTable.Zarib,
       FactorTable.Series,
       FactorTable.ExpirationDate,
       FactorTable.NumberInBox AS Expr1,
       FactorTable.NumberInPack,
       CityTable.Column02 AS CityName,
       ProvinceTable.Column01 AS ProvinceName,
       FactorTable.PayCash,
       FactorTable.DraftNumber,
       FactorTable.DocID,
       FactorTable.Feild,
       PersonInfoTable.Column01 AS SellerCode,
       '" + Class_BasicOperation._UserName + @"' AS Cashier
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
                       Table_011_Child1_SaleFactor.ColumnId AS DetailId,
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
                                       Column06,
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
            ON  FactorTable.Responsible = PersonInfoTable.ColumnId";

            HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);

            string DetailSelectText = @"SELECT     dbo.Table_024_Discount.column01 AS Name, dbo.Table_012_Child2_SaleFactor.column04 AS Price, 
                      CASE WHEN Table_024_Discount.column02 = 0 THEN '+' ELSE '-' END AS Type, dbo.Table_012_Child2_SaleFactor.column01 AS Column01 ,
                      dbo.Table_010_SaleFactor.column01 AS HeaderNum, dbo.Table_010_SaleFactor.column02 AS HeaderDate
                      FROM         dbo.Table_012_Child2_SaleFactor INNER JOIN
                      dbo.Table_010_SaleFactor ON dbo.Table_012_Child2_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid LEFT OUTER JOIN
                      dbo.Table_024_Discount ON dbo.Table_012_Child2_SaleFactor.column02 = dbo.Table_024_Discount.columnid";

            if (List.Count > 0)
            {
                HeaderSelectText += " WHERE     FactorTable.FactorID IN(" + string.Join(",", List.ToArray()) + ")";
                DetailSelectText += " WHERE     dbo.Table_012_Child2_SaleFactor.column01 IN (" + string.Join(",", List.ToArray()) + ")";

            }


            HeaderSelectText += " WHERE     (FactorTable.Serial = " + _SaleNumber + ")";
            DetailSelectText += " WHERE (Table_010_SaleFactor.Column01= " + _SaleNumber + ")";



            HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
            DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);
            double FinalPrice = 0;
            foreach (DataRow item in HeaderTable.Rows)
            {
                FinalPrice = Convert.ToDouble(item["NetTotal"].ToString()) +
                          Convert.ToDouble(item["Ezafat"].ToString()) - Convert.ToDouble(item["Kosoorat"].ToString()) -
                          Convert.ToDouble(item["SpecialGroup"].ToString()) - Convert.ToDouble(item["SpecialCustomer"].ToString())
                          - Convert.ToDouble(item["VolumeGroup"].ToString());
                item["CharPrice"] = FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(FinalPrice)) + " ریال";
            }




            //فراخوانی عناوین امضا
            Sign = clDoc.Signature(10);


            //جدول اطلاعات تسویه فاکتورها
            DataTable CustomerTable = HeaderTable.DefaultView.ToTable(true, new string[] { "CustomerId", "ID", "Date", "DocId" });
            TotalSettleInfo = dataSet_Sale21.FN_01_SettleInfo.Clone();

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
                    if (SettelmentWithFactor.Checked)
                    {
                        if (item["DocId"].ToString() != "0")
                            Sentence = " مانده حساب با احتساب فاکتور: " + string.Format("{0:#,##0.###}", (Remain));
                        else
                            Sentence = " مانده حساب با احتساب فاکتور: " + string.Format("{0:#,##0.###}", (Remain + FinalPrice));


                    }
                    else Sentence = " مانده حساب بدون احتساب این فاکتور: " + string.Format("{0:#,##0.###}", (Remain));
                    TotalSettleInfo.Rows.Add(item["CustomerId"].ToString(),
                        Sentence,
                        "0",
                        "0",
                        "0",
                        "0", item["ID"].ToString());

                }
            }


            Org = Class_BasicOperation.LogoTable();

            bt_First_Click(null, null);

        }

        private void bt_First_Click(object sender, EventArgs e)
        {

            //فاکتور عمودی اولی
            this.Cursor = Cursors.WaitCursor;


            StiReport stireport = new StiReport();
            stireport.Load("Rpt_02_SaleFactor_19.mrt");


            stireport.Compile();
            StiOptions.Viewer.AllowUseDragDrop = false;


            stireport.RegData("Rpt_SaleTable", HeaderTable);
            stireport.RegData("Rpt_SaleExtra_Table", DetailTable);
            stireport.RegData("Table_000_OrgInfo", Org);
            stireport.RegData("FN_01_SettleInfo", TotalSettleInfo);

            stireport["P1Tel"] = Org.Rows[0]["Column03"].ToString();
            stireport["P1Name"] = Org.Rows[0]["Column01"].ToString();
            stireport["P1ECode"] = Org.Rows[0]["Column06"].ToString();
            stireport["P1NCode"] = Org.Rows[0]["Column07"].ToString();
            stireport["P1Address"] = Org.Rows[0]["Column02"].ToString();
            stireport["P1PostalCode"] = Org.Rows[0]["Column14"].ToString();




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
            stireport["NotShowDate"] = !chk_ShowDate.Checked;
            stireport["ShowEcoCode"] = ChkEcoCode.Checked;

            this.Cursor = Cursors.Default;
            _PrintStyle = 1;
            stireport.Render();

            stireport.Show();

            this.Cursor = Cursors.Default;

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("Rpt_02_SaleFactor_19.mrt");
            stireport.Design();
        }



    }
}