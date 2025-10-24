using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace PSHOP._07_Services.Reports
{

    public partial class PrintFactor : Form
    {
        Int16 _FormNo = 0;
        DataTable _Table1 = new DataTable();
        DataTable _Table2 = new DataTable();
        DataTable _Table3 = new DataTable();
        short _PrintStyle = 1;
        string _Param1, _Param2;
        DataTable OrgInfo;
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();

        int _factorNum;
        public PrintFactor(Int16 FormNo, DataTable Table1, DataTable Table2, DataTable Table3, string Param1, string Param2, int factorNum)
        {
            InitializeComponent();

            _Table1 = Table1;
            _Table2 = Table2;
            _Table3 = Table3;
            _Param1 = Param1;
            _Param2 = Param2;
            _factorNum = factorNum;
        }
        private void ReportForm_Load(object sender, EventArgs e)
        {
            chk_Logo.Checked = Properties.Settings.Default.DontShowLogo;
            _PrintStyle = Properties.Settings.Default.ServiceFactorPrintLayout;
            bt_Display_Click(sender, e);

        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            switch (_PrintStyle)
            {
                case 1:
                    bt_1_Click(sender, e);
                    break;
                case 2: bt_2_Click(sender, e);
                    break;
                case 3: btn_3_Click(sender, e);
                    break;
            }

        }




        private void bt_1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ServiceFactorPrintLayout = 1;
            Properties.Settings.Default.DontShowLogo = chk_Logo.Checked;
            Properties.Settings.Default.Save();

            _07_Services.Reports.Rpt02_PrintFactor Rpt = new Rpt02_PrintFactor();
            Rpt.SetDataSource(_Table1);
            OrgInfo = Class_BasicOperation.LogoTable();
            if (!chk_Logo.Checked)
                Rpt.Subreports[0].SetDataSource(OrgInfo);
            else Rpt.Subreports[0].SetDataSource(OrgInfo.Clone());
            Rpt.Subreports["x1"].SetDataSource(OrgInfo);
            Rpt.Subreports["x2"].SetDataSource(_Table2);
            Rpt.Subreports["x3"].SetDataSource(_Table3);
            Rpt.SetParameterValue("Param1", _Param1);
            crystalReportViewer1.ReportSource = Rpt;
        }

        private void bt_2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ServiceFactorPrintLayout = 2;
            Properties.Settings.Default.DontShowLogo = chk_Logo.Checked;
            Properties.Settings.Default.Save();

            _07_Services.Reports.Rpt02_PrintFactor_2 Rpt = new Rpt02_PrintFactor_2();
            Rpt.SetDataSource(_Table1);
            OrgInfo = Class_BasicOperation.LogoTable();
            if (!chk_Logo.Checked)
                Rpt.Subreports[0].SetDataSource(OrgInfo);
            else Rpt.Subreports[0].SetDataSource(OrgInfo.Clone());
            Rpt.Subreports["x1"].SetDataSource(OrgInfo);
            Rpt.Subreports["x2"].SetDataSource(_Table2);
            Rpt.Subreports["x3"].SetDataSource(_Table3);
            Rpt.SetParameterValue("Param1", _Param1);
            crystalReportViewer1.ReportSource = Rpt;
        }

        private void btn_3_Click(object sender, EventArgs e)
        {
            string HeaderSelectText = null;
            HeaderSelectText = @"SELECT FactorTable.FactorID AS ID,
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
       FactorTable.SingleWeight,
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
       FactorTable.GoodDesc,
       '' AS ExpDate,
       '' AS BuildSeri
FROM   (
           SELECT Column00,
                  Column01,
                  Column02,
                  Column03,
                  Column04,
                  Column05,
                  Column06
           FROM   " + ConBase.Database + @".dbo.Table_060_ProvinceInfo
       ) AS ProvinceTable
       left JOIN (
                SELECT Column00,
                       Column01,
                       Column02,
                       Column03,
                       Column04,
                       Column05,
                       Column06,
                       Column07,
                       Column08
                FROM   " + ConBase.Database + @".dbo.Table_065_CityInfo
            ) AS CityTable
            ON  ProvinceTable.Column00 = CityTable.Column00
       RIGHT OUTER JOIN (
                SELECT dbo.Table_031_ServiceFactor.columnid AS FactorID,
                       Table_032_ServiceFactor_Child1.ColumnId AS DetailId,
                       dbo.Table_031_ServiceFactor.column01 AS Serial,
                       0 AS LegalNumber,
                       dbo.Table_031_ServiceFactor.column02 AS Date,
                       dbo.Table_031_ServiceFactor.column03 AS CustomerID,
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
                       dbo.Table_032_ServiceFactor_Child1.column04 AS Box,
                       0 AS BoxPrice,
                       dbo.Table_032_ServiceFactor_Child1.column05 AS Pack,
                       0 AS PackPrice,
                       dbo.Table_032_ServiceFactor_Child1.column03 AS Number,
                       dbo.Table_032_ServiceFactor_Child1.column03 AS TotalNumber,
                       dbo.Table_032_ServiceFactor_Child1.Column05 AS SinglePrice,
                       dbo.Table_032_ServiceFactor_Child1.Column06 AS TotalPrice,
                       0 AS 
                       DiscountPercent,
                       0 AS DiscountPrice,
                       0 AS TaxPercent,
                        0 AS TaxPrice,
                       0 AS TotalWeight,
                       0 AS SingleWeight,
                       dbo.Table_032_ServiceFactor_Child1.Column06 AS NetPrice,
                       GoodTable.column03 AS GoodName,
                       OtherPrice.PlusPrice AS Ezafat,
                       OtherPrice.MinusPrice AS Kosoorat,
                       dbo.Table_031_ServiceFactor.column05 AS Responsible,
                       dbo.Table_031_ServiceFactor.Column09 AS NetTotal,
                       0 AS VolumeGroup,
                       0 AS SpecialGroup,
                       0 AS SpecialCustomer,
                       dbo.Table_031_ServiceFactor.column06 AS DESCRIPTION,
                       CountUnitTable.Column01 AS CountUnitName,
                       '' AS FactorType,
                       dbo.Table_032_ServiceFactor_Child1.column07 AS RowDesc,
                       0 AS NumberInBox,
                       0 AS NumberInPack,
                       0 AS Zarib,
                       0 AS Series,
                       0 AS 
                       ExpirationDate,
                       PersonTable.Column21 AS ProvinceId,
                       PersonTable.Column22 AS CityId,
                       0 AS PayCash,
                     0 AS DraftNumber,
                       0 AS DocId,
                       dbo.Table_032_ServiceFactor_Child1.column07 AS GoodDesc 
                FROM   dbo.Table_031_ServiceFactor
                       INNER JOIN dbo.Table_032_ServiceFactor_Child1
                            ON  dbo.Table_031_ServiceFactor.columnid = dbo.Table_032_ServiceFactor_Child1.column01
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
                                               FROM   " + ConBase.Database + @".dbo.Table_060_ProvinceInfo 
                                                      tpi
                                               WHERE  tpi.Column00 = " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column21
                                           ),
                                           ' '
                                       ) + ' ' + ISNULL(
                                           (
                                               SELECT tpi.Column02
                                               FROM   " + ConBase.Database + @".dbo.Table_065_CityInfo 
                                                      tpi
                                               WHERE  tpi.Column01 = " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column22
                                           ),
                                           ' '
                                       ) + ' ' + ISNULL(" + ConBase.Database + @".dbo.Table_045_PersonInfo.Column06, ' ') AS 
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
                                FROM   " + ConBase.Database + @".dbo.Table_045_PersonInfo
                            ) AS PersonTable
                            ON  dbo.Table_031_ServiceFactor.column03 = PersonTable.ColumnId
                       LEFT OUTER JOIN (
                                SELECT columnid,
                                       SUM(PlusPrice) AS PlusPrice,
                                       SUM(MinusPrice) AS MinusPrice
                                FROM   (
                                           SELECT Table_031_ServiceFactor_2.columnid,
                                                  SUM(dbo.Table_033_ServiceFactor_Child2.column04) AS 
                                                  PlusPrice,
                                                  0 AS MinusPrice
                                           FROM   dbo.Table_033_ServiceFactor_Child2
                                                  INNER JOIN dbo.Table_031_ServiceFactor AS 
                                                       Table_031_ServiceFactor_2
                                                       ON  dbo.Table_033_ServiceFactor_Child2.column01 = 
                                                           Table_031_ServiceFactor_2.columnid
                                           WHERE  (dbo.Table_033_ServiceFactor_Child2.column05 = 0)
                                           GROUP BY
                                                  Table_031_ServiceFactor_2.columnid,
                                                  dbo.Table_033_ServiceFactor_Child2.column05
                                           UNION ALL
                                           SELECT Table_031_ServiceFactor_1.columnid,
                                                  0 AS PlusPrice,
                                                  SUM(Table_033_ServiceFactor_Child2_1.column04) AS 
                                                  MinusPrice
                                           FROM   dbo.Table_033_ServiceFactor_Child2 AS 
                                                  Table_033_ServiceFactor_Child2_1
                                                  INNER JOIN dbo.Table_031_ServiceFactor AS 
                                                       Table_031_ServiceFactor_1
                                                       ON  
                                                           Table_033_ServiceFactor_Child2_1.column01 = 
                                                           Table_031_ServiceFactor_1.columnid
                                           WHERE  (Table_033_ServiceFactor_Child2_1.column05 = 1)
                                           GROUP BY
                                                  Table_031_ServiceFactor_1.columnid,
                                                  Table_033_ServiceFactor_Child2_1.column05
                                       ) AS OtherPrice_1
                                GROUP BY
                                       columnid
                            ) AS OtherPrice
                            ON  dbo.Table_031_ServiceFactor.columnid = OtherPrice.columnid
                       LEFT OUTER JOIN (
                                SELECT columnid,
                                       column01,
                                       column02,
                                       column03,
                                       column04,
                                       column05,
                                       column06 
                                        
                                  
                                FROM   Table_030_Services
                            ) AS GoodTable
                            ON  dbo.Table_032_ServiceFactor_Child1.column02 = 
                                GoodTable.columnid
                       LEFT OUTER JOIN (
                                SELECT Column00,
                                       Column01
                                FROM   " + ConBase.Database + @".dbo.Table_070_CountUnitInfo
                            ) AS CountUnitTable
                            ON  dbo.Table_032_ServiceFactor_Child1.column04 = 
                                CountUnitTable.Column00
            ) AS FactorTable
            ON  CityTable.Column01 = FactorTable.CityId
       LEFT OUTER JOIN (
                SELECT PersonId,
                       Groups
                FROM   " + ConBase.Database + @".dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1
            ) AS derivedtbl_1
            ON  FactorTable.CustomerID = derivedtbl_1.PersonId
       LEFT OUTER JOIN (
                SELECT ColumnId,
                       Column01,
                       Column02,
                       Column21,
                       Column22
                FROM   " + ConBase.Database + @".dbo.Table_045_PersonInfo AS 
                       Table_045_PersonInfo_1
            ) AS PersonInfoTable
            ON  FactorTable.Responsible = PersonInfoTable.ColumnId";

            string DetailSelectText = @"SELECT dbo.Table_024_Discount.column01 AS NAME,
       dbo.Table_033_ServiceFactor_Child2.column04 AS Price,
       CASE 
            WHEN Table_024_Discount.column02 = 0 THEN '+'
            ELSE '-'
       END AS TYPE,
       dbo.Table_033_ServiceFactor_Child2.column01 AS Column01,
       dbo.Table_031_ServiceFactor.column01 AS HeaderNum,
       dbo.Table_031_ServiceFactor.column02 AS HeaderDate
FROM   dbo.Table_033_ServiceFactor_Child2
       INNER JOIN dbo.Table_031_ServiceFactor
            ON  dbo.Table_033_ServiceFactor_Child2.column01 = dbo.Table_031_ServiceFactor.columnid
       LEFT OUTER JOIN dbo.Table_024_Discount
            ON  dbo.Table_033_ServiceFactor_Child2.column02 = dbo.Table_024_Discount.columnid";
            HeaderSelectText += " WHERE     (FactorTable.Serial = " + _factorNum + ")  ";
            DetailSelectText += " WHERE (Table_031_ServiceFactor.Column01= " + _factorNum + ")";

            DataTable HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
            DataTable DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);

            foreach (DataRow item in HeaderTable.Rows)
            {
                double FinalPrice = Convert.ToDouble(item["NetTotal"].ToString()) +
                            Convert.ToDouble(item["Ezafat"].ToString()) - Convert.ToDouble(item["Kosoorat"].ToString()) -
                            Convert.ToDouble(item["SpecialGroup"].ToString()) - Convert.ToDouble(item["SpecialCustomer"].ToString())
                            - Convert.ToDouble(item["VolumeGroup"].ToString());
                item["CharPrice"] = FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(FinalPrice)) + " ریال";
            }

            try
            {

                crystalReportViewer1.Visible = true;
                //طرح عمودی
                this.Cursor = Cursors.WaitCursor;
                _07_Services.Reports.Rpt_02_SaleFactor_24 Rpt2 = new Rpt_02_SaleFactor_24();


                Rpt2.SetDataSource(HeaderTable);
                Rpt2.SetParameterValue("ShowEcoCode", false);


                crystalReportViewer1.ReportSource = Rpt2;
                this.Cursor = Cursors.Default;
                Properties.Settings.Default.ServiceFactorPrintLayout = 3;
                Properties.Settings.Default.DontShowLogo = chk_Logo.Checked;
                Properties.Settings.Default.Save();

                //System.Drawing.Printing.PrinterSettings getprinterName = new System.Drawing.Printing.PrinterSettings();
                //Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                //Rpt2.PrintToPrinter(1, true, 1, 1000);

            }
            catch { }
        }


    }
}
