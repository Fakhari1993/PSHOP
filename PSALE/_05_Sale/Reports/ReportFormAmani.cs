using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._05_Sale.Reports
{
    public partial class ReportFormAmani : Form
    {
        Int16 _FormNo = 0;
        DataTable _Table1 = new DataTable();
        DataTable _Table2 = new DataTable();
        DataTable _Table3 = new DataTable();
        DataTable _Table4 = new DataTable();
        DataTable _Table5 = new DataTable();
        DataTable _Table6 = new DataTable();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        DataTable HeaderTable;
       
        int _SaleNumber;
        string[] Sign;

        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        public ReportFormAmani(int SaleNumber, Int16 FormNo)
        {
            InitializeComponent();
            _SaleNumber = SaleNumber;
            _FormNo = FormNo;
        }
        public ReportFormAmani(Int16 FormNo)
        {
            InitializeComponent();
            _FormNo = FormNo;
        }
        public ReportFormAmani(Int16 FormNo, DataTable Table)
        {
            InitializeComponent();
            _FormNo = FormNo;
            _Table1 = Table;
        }
        public ReportFormAmani(Int16 FormNo, DataTable Table1, DataTable Table2, DataTable Table3, DataTable Table4, DataTable Table5, DataTable Table6)
        {
            InitializeComponent();
            _FormNo = FormNo;
            _Table1 = Table1;
            _Table2 = Table2;
            _Table3 = Table3;
            _Table4 = Table4;
            _Table5 = Table5;
            _Table6 = Table6;
        }
        private void ReportForm_Load(object sender, EventArgs e)
        {
            //تسویه فاکتورها
            if (_FormNo == 1)
            {
                _05_Sale.Reports.Rpt_04_SettleFactor rpt = new Rpt_04_SettleFactor();
                rpt.SetDataSource(_Table1);
                rpt.Subreports["Logo"].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.Subreports["Cash"].SetDataSource(_Table2);
                rpt.Subreports["Chq"].SetDataSource(_Table3);
                rpt.Subreports["Return"].SetDataSource(_Table4);
                rpt.Subreports["Discount"].SetDataSource(_Table5);
                rpt.Subreports["FromTo"].SetDataSource(_Table6);
                crystalReportViewer1.ReportSource = rpt;
            }
            else
                if (_FormNo == 2)
                {
                    string HeaderSelectText = null;
                    HeaderSelectText = @"SELECT     TOP (100) PERCENT FactorTable.FactorID AS ID, FactorTable.DetailId, FactorTable.Serial, FactorTable.LegalNumber, FactorTable.Date, FactorTable.Responsible, 
                      FactorTable.CustomerID, FactorTable.P2Name, FactorTable.P2NationalCode, FactorTable.P2ECode, FactorTable.P2SabtCode, FactorTable.P2Address, 
                      FactorTable.P2Tel, FactorTable.Mobile, FactorTable.P2Fax, FactorTable.P2PostalCode, FactorTable.P2Code, FactorTable.GoodCode, FactorTable.GoodName, 
                      FactorTable.Box, FactorTable.BoxPrice, FactorTable.Pack, FactorTable.PackPrice, FactorTable.Number, FactorTable.TotalNumber, FactorTable.SinglePrice, 
                      FactorTable.TotalPrice, FactorTable.DiscountPercent, FactorTable.DiscountPrice, FactorTable.TaxPrice, FactorTable.TotalWeight, FactorTable.SingleWeight, 
                      FactorTable.TaxPercent, FactorTable.NetPrice, FactorTable.Description, 'SettleInfo' AS SettleInfo, FactorTable.NumberInBox, FactorTable.RowDesc, FactorTable.Zarib, 
                      FactorTable.Series, FactorTable.ExpirationDate, FactorTable.NumberInBox AS Expr1, FactorTable.NumberInPack, CityTable.Column02 AS CityName, 
                      ProvinceTable.Column01 AS ProvinceName, FactorTable.Feild, FactorTable.GoodDesc ,'-' AS charPrice,FactorTable.Column35 as ExpDate,FactorTable.Column34 as BuildSeri
FROM         (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
                       FROM           " + ConBase.Database + @".dbo.Table_060_ProvinceInfo) AS ProvinceTable INNER JOIN
                          (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                            FROM           " + ConBase.Database + @".dbo.Table_065_CityInfo) AS CityTable ON ProvinceTable.Column00 = CityTable.Column00 RIGHT OUTER JOIN
                        ( SELECT     dbo.Table_070_AmaniFactor.columnid AS FactorID, dbo.Table_075_Child_AmaniFactor.columnid AS DetailId, dbo.Table_070_AmaniFactor.column01 AS Serial, 
                      dbo.Table_070_AmaniFactor.Column37 AS LegalNumber, dbo.Table_070_AmaniFactor.column02 AS Date, dbo.Table_070_AmaniFactor.column03 AS CustomerID, 
                      PersonTable.Column02 AS P2Name, PersonTable.Column09 AS P2NationalCode, PersonTable.Column141 AS P2ECode, PersonTable.Column142 AS P2SabtCode, 
                      PersonTable.Column01 AS P2Code, PersonTable.Column06 AS P2Address, PersonTable.Column07 AS P2Tel, PersonTable.Column19 AS Mobile, 
                      PersonTable.Column08 AS P2Fax, PersonTable.Column13 AS P2PostalCode, PersonTable.Column10 AS Feild, GoodTable.column01 AS GoodCode, 
                      dbo.Table_075_Child_AmaniFactor.column04 AS Box, dbo.Table_075_Child_AmaniFactor.column08 AS BoxPrice, 
                      dbo.Table_075_Child_AmaniFactor.column05 AS Pack, dbo.Table_075_Child_AmaniFactor.column09 AS PackPrice, 
                      dbo.Table_075_Child_AmaniFactor.column06 AS Number, dbo.Table_075_Child_AmaniFactor.column07 AS TotalNumber, 
                      dbo.Table_075_Child_AmaniFactor.column10 AS SinglePrice, dbo.Table_075_Child_AmaniFactor.column11 AS TotalPrice, 
                      dbo.Table_075_Child_AmaniFactor.column16 AS DiscountPercent, dbo.Table_075_Child_AmaniFactor.column17 AS DiscountPrice, 
                      dbo.Table_075_Child_AmaniFactor.column18 AS TaxPercent, dbo.Table_075_Child_AmaniFactor.column19 AS TaxPrice, 
                      dbo.Table_075_Child_AmaniFactor.Column37 AS TotalWeight, dbo.Table_075_Child_AmaniFactor.Column36 AS SingleWeight, 
                      dbo.Table_075_Child_AmaniFactor.column20 AS NetPrice, GoodTable.column02 AS GoodName, dbo.Table_070_AmaniFactor.column05 AS Responsible, 
                      dbo.Table_070_AmaniFactor.column06 AS Description, CountUnitTable.Column01 AS CountUnitName, dbo.Table_075_Child_AmaniFactor.column23 AS RowDesc, 
                      dbo.Table_075_Child_AmaniFactor.Column31 AS NumberInBox, dbo.Table_075_Child_AmaniFactor.Column32 AS NumberInPack, 
                      dbo.Table_075_Child_AmaniFactor.Column33 AS Zarib, dbo.Table_075_Child_AmaniFactor.Column34 AS Series, 
                      dbo.Table_075_Child_AmaniFactor.Column35 AS ExpirationDate, PersonTable.Column21 AS ProvinceId, PersonTable.Column22 AS CityId, 
                      dbo.Table_070_AmaniFactor.column21 AS PayCash, dbo.Table_075_Child_AmaniFactor.column23 AS GoodDesc, dbo.Table_075_Child_AmaniFactor.column34, dbo.Table_075_Child_AmaniFactor.column35
FROM         dbo.Table_070_AmaniFactor INNER JOIN
                      dbo.Table_075_Child_AmaniFactor ON dbo.Table_070_AmaniFactor.columnid = dbo.Table_075_Child_AmaniFactor.column01 INNER JOIN
                          (SELECT     ColumnId, Column00, Column01, Column02, Column03, Column04, Column05, ISNULL
                                                       ((SELECT     Column01
                                                           FROM          " + ConBase.Database + @".dbo.Table_060_ProvinceInfo AS tpi
                                                           WHERE     (Column00 =  " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column21)), ' ') + ' ' + ISNULL
                                                       ((SELECT     Column02
                                                           FROM          " + ConBase.Database + @".dbo.Table_065_CityInfo AS tpi
                                                           WHERE     (Column01 =  " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column22)), ' ') + ' ' + ISNULL(Column06, ' ') AS Column06, Column07, Column08, 
                                                   Column09, Column10, Column11, Column12, Column13, Column21, Column22, Column19, Column141, Column142
                            FROM           " + ConBase.Database + @".dbo.Table_045_PersonInfo) AS PersonTable ON dbo.Table_070_AmaniFactor.column03 = PersonTable.ColumnId LEFT OUTER JOIN
                          (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, 
                                                   column14, column15, column16, column17, column18, column19, column20, column21, column22, column23, column24, column25, column26, column27, 
                                                   column28, column29, column30, column31
                            FROM           " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients) AS GoodTable ON 
                      dbo.Table_075_Child_AmaniFactor.column02 = GoodTable.columnid LEFT OUTER JOIN
                          (SELECT     Column00, Column01
                            FROM           " + ConBase.Database + @".dbo.Table_070_CountUnitInfo) AS CountUnitTable ON dbo.Table_075_Child_AmaniFactor.column03 = CountUnitTable.Column00)
                       AS FactorTable ON CityTable.Column01 = FactorTable.CityId LEFT OUTER JOIN
                          (SELECT     PersonId, Groups
                            FROM          " + ConBase.Database + @" .dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1) AS derivedtbl_1 ON 
                      FactorTable.CustomerID = derivedtbl_1.PersonId LEFT OUTER JOIN
                          (SELECT     ColumnId, Column01, Column02, Column21, Column22
                            FROM           " + ConBase.Database + @".dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1) AS PersonInfoTable ON 
                      FactorTable.Responsible = PersonInfoTable.ColumnId
WHERE     (FactorTable.Serial = " + _SaleNumber + @")
ORDER BY ID, FactorTable.DetailId";

                    HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
                    foreach (DataRow item in HeaderTable.Rows)
                    {
                        double FinalPrice = Convert.ToDouble(item["NetPrice"].ToString());
                        item["CharPrice"] = FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(FinalPrice)) + " ریال";
                    }
                    //                    DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);
                    Sign = clDoc.Signature(10);
                    DataTable Org = Class_BasicOperation.LogoTable();
                    crystalReportViewer1.Visible = true;
                    this.Cursor = Cursors.WaitCursor;
                    _05_Sale.Reports.Rpt_02_SaleFactor14Amani Rpt1 = new Rpt_02_SaleFactor14Amani();
                    Rpt1.SetDataSource(HeaderTable);
                    Rpt1.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                    Rpt1.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                    Rpt1.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                    Rpt1.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                    Rpt1.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                    Rpt1.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
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

                }
        }

        private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
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
    }
}
