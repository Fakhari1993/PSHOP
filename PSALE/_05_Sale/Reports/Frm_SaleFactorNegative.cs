using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Stimulsoft.Report;

namespace PSHOP._05_Sale.Reports
{
    public partial class Frm_SaleFactorNegative : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
  
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        string Whrs;
        string Date1,Date2;
        
        public Frm_SaleFactorNegative(string Idwhrs, string date1, string date2)
        {
            Whrs = Idwhrs;
            Date1=date1;
            Date2=date2;
            InitializeComponent();
        }

        private void Frm_SaleFactorNegative_Load(object sender, EventArgs e)
        {
            try
            {
                if ( Whrs != "0")
                {


                    DataTable dtNegative = ClDoc.ReturnTable(ConWare.ConnectionString, @" SELECT    *, 
                          Dtsale.TotalNumber + ROUND(Turn.ITotalNumber - Turn.OTotalNumber, 3) AS Firstperiod, dbo.Table_001_PWHRS.column02 AS WhrsSale, 
                         " + ConBase.Database + @".dbo.Table_070_CountUnitInfo.Column01 AS CountSale
FROM            " + ConBase.Database + @".dbo.Table_070_CountUnitInfo INNER JOIN
                             (SELECT        TurnTable.GoodCode AS GoodID, TurnTable.Project, ROUND(SUM(TurnTable.INumberInBox), 3) AS INumberInBox, 
                                                         ROUND(SUM(TurnTable.TINumberInBox), 3) AS TINumberInBox, ROUND(SUM(TurnTable.INumberInPack), 3) AS INumberInPack, 
                                                         ROUND(SUM(TurnTable.TINumberInPack), 3) AS TINumberInPack, ROUND(SUM(TurnTable.IDetailNumber), 3) AS IDetailNumber, 
                                                         ROUND(SUM(TurnTable.ITotalNumber), 3) AS ITotalNumber, ROUND(SUM(TurnTable.ONumberInBox), 3) AS ONumberInBox, 
                                                         ROUND(SUM(TurnTable.TONumberInBox), 3) AS TONumberInBox, ROUND(SUM(TurnTable.ONumberInPack), 3) AS ONumberInPack, 
                                                         ROUND(SUM(TurnTable.TONumberInPack), 3) AS TONumberInPack, ROUND(SUM(TurnTable.ODetailNumber), 3) AS ODetailNumber, 
                                                         ROUND(SUM(TurnTable.OTotalNumber), 3) AS OTotalNumber, ROUND(SUM(TurnTable.INumberInBox) - SUM(TurnTable.ONumberInBox), 3) 
                                                         AS RNumberInBox, ROUND(SUM(TurnTable.TINumberInBox) - SUM(TurnTable.TONumberInBox), 3) AS TRNumberInBox, 
                                                         ROUND(SUM(TurnTable.INumberInPack) - SUM(TurnTable.ONumberInPack), 3) AS RNumberInPack, ROUND(SUM(TurnTable.TINumberInPack) 
                                                         - SUM(TurnTable.TONumberInPack), 3) AS TRNumberInPack, ROUND(SUM(TurnTable.IDetailNumber) - SUM(TurnTable.ODetailNumber), 3) 
                                                         AS RDetailNumber, ROUND(SUM(TurnTable.ITotalNumber) - SUM(TurnTable.OTotalNumber), 3) AS RTotalNumber, 
                                                         dbo.table_004_CommodityAndIngredients.column02 AS GoodName, dbo.table_004_CommodityAndIngredients.column01 AS GoodCode, 
                                                         dbo.table_004_CommodityAndIngredients.column07 AS UnitCount, dbo.table_004_CommodityAndIngredients.column05 AS CompName, 
                                                         dbo.table_003_SubsidiaryGroup.column03 AS SubGroupName, dbo.table_002_MainGroup.column02 AS MainGroupName, 
                                                         ROUND(SUM(TurnTable.OSingleWeight), 3) AS OSingleWeight, ROUND(SUM(TurnTable.OTotalWeight), 3) AS OTotalWeight, 
                                                         ROUND(SUM(TurnTable.TOTotalWeight), 3) AS TOTotalWeight, ROUND(SUM(TurnTable.ISingleWeight), 3) AS ISingleWeight, 
                                                         ROUND(SUM(TurnTable.ITotalWeight), 3) AS ITotalWeight, ROUND(SUM(TurnTable.TITotalWeight), 3) AS TITotalWeight, 
                                                         ROUND(SUM(TurnTable.ISingleWeight) - SUM(TurnTable.OSingleWeight), 3) AS RSingleWeight, ROUND(SUM(TurnTable.ITotalWeight) 
                                                         - SUM(TurnTable.OTotalWeight), 3) AS RTotalWeight, ROUND(SUM(TurnTable.TITotalWeight) - SUM(TurnTable.TOTotalWeight), 3) AS TRTotalWeight, 
                                                         dbo.table_004_CommodityAndIngredients.column26 AS PROPERTY
                               FROM            (SELECT        dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, " + ConBase.Database + @".dbo.Table_035_ProjectInfo.Column02 AS Project, 
                                                                                   SUM(dbo.Table_008_Child_PwhrsDraft.column04) AS ONumberInBox, 
                                                                                   CAST(ROUND(ISNULL(SUM(dbo.Table_008_Child_PwhrsDraft.column07) / NULLIF
                                                                                       ((SELECT        column09
                                                                                           FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                           WHERE        (columnid = dbo.Table_008_Child_PwhrsDraft.column02)), 0), 0), 3) AS DECIMAL(36, 3)) AS TONumberInBox, 
                                                                                   SUM(dbo.Table_008_Child_PwhrsDraft.column05) AS ONumberInPack, 
                                                                                   CAST(ROUND(ISNULL(SUM(dbo.Table_008_Child_PwhrsDraft.column07) / NULLIF
                                                                                       ((SELECT        column08
                                                                                           FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                           WHERE        (columnid = dbo.Table_008_Child_PwhrsDraft.column02)), 0), 0), 3) AS DECIMAL(36, 3)) AS TONumberInPack, 
                                                                                   SUM(dbo.Table_008_Child_PwhrsDraft.column06) AS ODetailNumber, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OTotalNumber,
                                                                                    SUM(ISNULL(dbo.Table_008_Child_PwhrsDraft.Column34, 0)) AS OSingleWeight, 
                                                                                   SUM(ISNULL(dbo.Table_008_Child_PwhrsDraft.Column35, 0)) AS OTotalWeight, SUM(dbo.Table_008_Child_PwhrsDraft.column07) 
                                                                                   * ISNULL
                                                                                       ((SELECT        column22
                                                                                           FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                           WHERE        (columnid = dbo.Table_008_Child_PwhrsDraft.column02)), 0) AS TOTotalWeight, CAST(0 AS FLOAT) AS INumberInBox, 
                                                                                   CAST(0 AS FLOAT) AS TINumberInBox, CAST(0 AS FLOAT) AS INumberInPack, CAST(0 AS FLOAT) AS TINumberInPack, CAST(0 AS FLOAT) 
                                                                                   AS IDetailNumber, CAST(0 AS FLOAT) AS ITotalNumber, CAST(0 AS FLOAT) AS ISingleWeight, CAST(0 AS FLOAT) AS ITotalWeight, 
                                                                                   CAST(0 AS FLOAT) AS TITotalWeight
                                                         FROM            dbo.Table_007_PwhrsDraft INNER JOIN
                                                                                   dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01 LEFT OUTER JOIN
                                                                                   " + ConBase.Database + @".dbo.Table_035_ProjectInfo ON 
                                                                                   dbo.Table_008_Child_PwhrsDraft.column14 = " + ConBase.Database + @".dbo.Table_035_ProjectInfo.Column00
                                                         WHERE        (dbo.Table_007_PwhrsDraft.column03 IN (" + Whrs.TrimEnd(',') + @"))
                                                         GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_008_Child_PwhrsDraft.column14, 
                                                                                   " + ConBase.Database + @".dbo.Table_035_ProjectInfo.Column02
                                                         UNION ALL
                                                         SELECT        dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, Table_035_ProjectInfo_1.Column02 AS Project, CAST(0 AS FLOAT) 
                                                                                  AS INumberInBox, CAST(0 AS FLOAT) AS TINumberInBox, CAST(0 AS FLOAT) AS INumberInPack, CAST(0 AS FLOAT) AS TINumberInPack, 
                                                                                  CAST(0 AS FLOAT) AS IDetailNumber, CAST(0 AS FLOAT) AS ITotalNumber, CAST(0 AS FLOAT) AS OSingleWeight, CAST(0 AS FLOAT) 
                                                                                  AS OTotalWeight, CAST(0 AS FLOAT) AS TOTotalWeight, SUM(dbo.Table_012_Child_PwhrsReceipt.column04) AS ONumberInBox, 
                                                                                  CAST(ROUND(ISNULL(SUM(dbo.Table_012_Child_PwhrsReceipt.column07) / NULLIF
                                                                                      ((SELECT        column09
                                                                                          FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                          WHERE        (columnid = dbo.Table_012_Child_PwhrsReceipt.column02)), 0), 0), 3) AS DECIMAL(36, 3)) AS TONumberInBox, 
                                                                                  SUM(dbo.Table_012_Child_PwhrsReceipt.column05) AS ONumberInPack, 
                                                                                  CAST(ROUND(ISNULL(SUM(dbo.Table_012_Child_PwhrsReceipt.column07) / NULLIF
                                                                                      ((SELECT        column08
                                                                                          FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                          WHERE        (columnid = dbo.Table_012_Child_PwhrsReceipt.column02)), 0), 0), 3) AS DECIMAL(36, 3)) AS TONumberInPack, 
                                                                                  SUM(dbo.Table_012_Child_PwhrsReceipt.column06) AS ODetailNumber, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) 
                                                                                  AS OTotalNumber, SUM(ISNULL(dbo.Table_012_Child_PwhrsReceipt.Column34, 0)) AS ISingleWeight, 
                                                                                  SUM(ISNULL(dbo.Table_012_Child_PwhrsReceipt.Column35, 0)) AS ITotalWeight, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) 
                                                                                  * ISNULL
                                                                                      ((SELECT        column22
                                                                                          FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                          WHERE        (columnid = dbo.Table_012_Child_PwhrsReceipt.column02)), 0) AS TITotalWeight
                                                         FROM            dbo.Table_011_PwhrsReceipt INNER JOIN
                                                                                  dbo.Table_012_Child_PwhrsReceipt ON 
                                                                                  dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01 LEFT OUTER JOIN
                                                                                  " + ConBase.Database + @".dbo.Table_035_ProjectInfo AS Table_035_ProjectInfo_1 ON 
                                                                                  dbo.Table_012_Child_PwhrsReceipt.column14 = Table_035_ProjectInfo_1.Column00
                                                         WHERE        (dbo.Table_011_PwhrsReceipt.column03 IN (" + Whrs.TrimEnd(',') + @"))
                                                         GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.column14, Table_035_ProjectInfo_1.Column02) 
                                                         AS TurnTable INNER JOIN
                                                         dbo.table_004_CommodityAndIngredients ON TurnTable.GoodCode = dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
                                                         dbo.table_003_SubsidiaryGroup ON dbo.table_004_CommodityAndIngredients.column04 = dbo.table_003_SubsidiaryGroup.columnid AND 
                                                         dbo.table_004_CommodityAndIngredients.column03 = dbo.table_003_SubsidiaryGroup.column01 INNER JOIN
                                                         dbo.table_002_MainGroup ON dbo.table_003_SubsidiaryGroup.column01 = dbo.table_002_MainGroup.columnid
                               GROUP BY dbo.table_004_CommodityAndIngredients.column26, TurnTable.GoodCode, dbo.table_004_CommodityAndIngredients.column02, 
                                                         dbo.table_004_CommodityAndIngredients.column01, dbo.table_004_CommodityAndIngredients.column07, TurnTable.Project, 
                                                         dbo.table_004_CommodityAndIngredients.column05, dbo.table_003_SubsidiaryGroup.column03, dbo.table_002_MainGroup.column02
                               HAVING         (ROUND(SUM(TurnTable.ITotalNumber) - SUM(TurnTable.OTotalNumber), 3) < 0)) AS Turn ON 
                         " + ConBase.Database + @".dbo.Table_070_CountUnitInfo.Column00 = Turn.UnitCount RIGHT OUTER JOIN
                             (SELECT        " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column02 AS Godcode, " + ConSale.Database + @".dbo.Table_010_SaleFactor.Column42 AS WHRS, 
                                                         SUM(" + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column07) AS TotalNumber, 
                                                         " + ConSale.Database + @".dbo.Table_010_SaleFactor.column02 AS Date
                               FROM            " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor INNER JOIN
                                                         " + ConSale.Database + @".dbo.Table_010_SaleFactor ON 
                                                         " + ConSale.Database + @".dbo.Table_010_SaleFactor.columnid = " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column01
                               GROUP BY " + ConSale.Database + @".dbo.Table_010_SaleFactor.column02, " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column02, 
                                                         " + ConSale.Database + @".dbo.Table_010_SaleFactor.Column42
                               HAVING         (" + ConSale.Database + @".dbo.Table_010_SaleFactor.column02 >=  '" + Date1 + @"') AND (" + ConSale.Database + @".dbo.Table_010_SaleFactor.column02 <=  '" + Date2 + @"')) 
                         AS Dtsale INNER JOIN
                         dbo.Table_001_PWHRS ON Dtsale.WHRS = dbo.Table_001_PWHRS.columnid ON Turn.GoodID = Dtsale.Godcode
WHERE        (Turn.GoodID IS NOT NULL) AND (Dtsale.WHRS in( " + Whrs.TrimEnd(',') + @"))  ");


                    StiReport stireport = new StiReport();
                    stireport.Load("Rpt_02_Sale_Negative.mrt");
                    stireport.Pages["Page1"].Enabled = true;
                    stireport.Compile();
                    stireport.RegData("dtNegative", dtNegative);
                    this.Cursor = Cursors.Default;
                    stireport.Render(false);
                    stiViewerControl1.Report = stireport;
                    stiViewerControl1.Refresh();

                }

                else  if ( Whrs=="0" )
                {
                    DataTable dt1negative = ClDoc.ReturnTable(ConWare.ConnectionString, @"

 SELECT     * , Dtsale.TotalNumber + ROUND(Turn.ITotalNumber - Turn.OTotalNumber, 3) AS Firstperiod, dbo.Table_001_PWHRS.column02 AS WhrsSale, 
                         " + ConBase.Database + @".dbo.Table_070_CountUnitInfo.Column01 AS CountSale
FROM            " + ConBase.Database + @".dbo.Table_070_CountUnitInfo INNER JOIN
                             (SELECT        TurnTable.GoodCode AS GoodID, TurnTable.Project, ROUND(SUM(TurnTable.INumberInBox), 3) AS INumberInBox, 
                                                         ROUND(SUM(TurnTable.TINumberInBox), 3) AS TINumberInBox, ROUND(SUM(TurnTable.INumberInPack), 3) AS INumberInPack, 
                                                         ROUND(SUM(TurnTable.TINumberInPack), 3) AS TINumberInPack, ROUND(SUM(TurnTable.IDetailNumber), 3) AS IDetailNumber, 
                                                         ROUND(SUM(TurnTable.ITotalNumber), 3) AS ITotalNumber, ROUND(SUM(TurnTable.ONumberInBox), 3) AS ONumberInBox, 
                                                         ROUND(SUM(TurnTable.TONumberInBox), 3) AS TONumberInBox, ROUND(SUM(TurnTable.ONumberInPack), 3) AS ONumberInPack, 
                                                         ROUND(SUM(TurnTable.TONumberInPack), 3) AS TONumberInPack, ROUND(SUM(TurnTable.ODetailNumber), 3) AS ODetailNumber, 
                                                         ROUND(SUM(TurnTable.OTotalNumber), 3) AS OTotalNumber, ROUND(SUM(TurnTable.INumberInBox) - SUM(TurnTable.ONumberInBox), 3) 
                                                         AS RNumberInBox, ROUND(SUM(TurnTable.TINumberInBox) - SUM(TurnTable.TONumberInBox), 3) AS TRNumberInBox, 
                                                         ROUND(SUM(TurnTable.INumberInPack) - SUM(TurnTable.ONumberInPack), 3) AS RNumberInPack, ROUND(SUM(TurnTable.TINumberInPack) 
                                                         - SUM(TurnTable.TONumberInPack), 3) AS TRNumberInPack, ROUND(SUM(TurnTable.IDetailNumber) - SUM(TurnTable.ODetailNumber), 3) 
                                                         AS RDetailNumber, ROUND(SUM(TurnTable.ITotalNumber) - SUM(TurnTable.OTotalNumber), 3) AS RTotalNumber, 
                                                         dbo.table_004_CommodityAndIngredients.column02 AS GoodName, dbo.table_004_CommodityAndIngredients.column01 AS GoodCode, 
                                                         dbo.table_004_CommodityAndIngredients.column07 AS UnitCount, dbo.table_004_CommodityAndIngredients.column05 AS CompName, 
                                                         dbo.table_003_SubsidiaryGroup.column03 AS SubGroupName, dbo.table_002_MainGroup.column02 AS MainGroupName, 
                                                         ROUND(SUM(TurnTable.OSingleWeight), 3) AS OSingleWeight, ROUND(SUM(TurnTable.OTotalWeight), 3) AS OTotalWeight, 
                                                         ROUND(SUM(TurnTable.TOTotalWeight), 3) AS TOTotalWeight, ROUND(SUM(TurnTable.ISingleWeight), 3) AS ISingleWeight, 
                                                         ROUND(SUM(TurnTable.ITotalWeight), 3) AS ITotalWeight, ROUND(SUM(TurnTable.TITotalWeight), 3) AS TITotalWeight, 
                                                         ROUND(SUM(TurnTable.ISingleWeight) - SUM(TurnTable.OSingleWeight), 3) AS RSingleWeight, ROUND(SUM(TurnTable.ITotalWeight) 
                                                         - SUM(TurnTable.OTotalWeight), 3) AS RTotalWeight, ROUND(SUM(TurnTable.TITotalWeight) - SUM(TurnTable.TOTotalWeight), 3) AS TRTotalWeight, 
                                                         dbo.table_004_CommodityAndIngredients.column26 AS property
                               FROM            (SELECT        dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, " + ConBase.Database + @".dbo.Table_035_ProjectInfo.Column02 AS Project, 
                                                                                   SUM(dbo.Table_008_Child_PwhrsDraft.column04) AS ONumberInBox, 
                                                                                   CAST(ROUND(ISNULL(SUM(dbo.Table_008_Child_PwhrsDraft.column07) / NULLIF
                                                                                       ((SELECT        column09
                                                                                           FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                           WHERE        (columnid = dbo.Table_008_Child_PwhrsDraft.column02)), 0), 0), 3) AS DECIMAL(36, 3)) AS TONumberInBox, 
                                                                                   SUM(dbo.Table_008_Child_PwhrsDraft.column05) AS ONumberInPack, 
                                                                                   CAST(ROUND(ISNULL(SUM(dbo.Table_008_Child_PwhrsDraft.column07) / NULLIF
                                                                                       ((SELECT        column08
                                                                                           FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                           WHERE        (columnid = dbo.Table_008_Child_PwhrsDraft.column02)), 0), 0), 3) AS DECIMAL(36, 3)) AS TONumberInPack, 
                                                                                   SUM(dbo.Table_008_Child_PwhrsDraft.column06) AS ODetailNumber, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OTotalNumber,
                                                                                    SUM(ISNULL(dbo.Table_008_Child_PwhrsDraft.Column34, 0)) AS OSingleWeight, 
                                                                                   SUM(ISNULL(dbo.Table_008_Child_PwhrsDraft.Column35, 0)) AS OTotalWeight, SUM(dbo.Table_008_Child_PwhrsDraft.column07) 
                                                                                   * ISNULL
                                                                                       ((SELECT        column22
                                                                                           FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                           WHERE        (columnid = dbo.Table_008_Child_PwhrsDraft.column02)), 0) AS TOTotalWeight, CAST(0 AS float) AS INumberInBox, 
                                                                                   CAST(0 AS float) AS TINumberInBox, CAST(0 AS float) AS INumberInPack, CAST(0 AS float) AS TINumberInPack, CAST(0 AS float) 
                                                                                   AS IDetailNumber, CAST(0 AS float) AS ITotalNumber, CAST(0 AS FLOAT) AS ISingleWeight, CAST(0 AS FLOAT) AS ITotalWeight, 
                                                                                   CAST(0 AS FLOAT) AS TITotalWeight
                                                         FROM            dbo.Table_007_PwhrsDraft INNER JOIN
                                                                                   dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01 LEFT OUTER JOIN
                                                                                   " + ConBase.Database + @".dbo.Table_035_ProjectInfo ON 
                                                                                   dbo.Table_008_Child_PwhrsDraft.column14 = " + ConBase.Database + @".dbo.Table_035_ProjectInfo.Column00
                                            WHERE       dbo.Table_007_PwhrsDraft.column03 not in (select Column02 from " + ConAcnt.Database + @".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')   " + @"
                    GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_008_Child_PwhrsDraft.column14, 
                                                                                   " + ConBase.Database + @".dbo.Table_035_ProjectInfo.Column02
                                                         UNION ALL
                                                         SELECT        dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, Table_035_ProjectInfo_1.Column02 AS Project, CAST(0 AS float) 
                                                                                  AS INumberInBox, CAST(0 AS float) AS TINumberInBox, CAST(0 AS float) AS INumberInPack, CAST(0 AS float) AS TINumberInPack, 
                                                                                  CAST(0 AS float) AS IDetailNumber, CAST(0 AS float) AS ITotalNumber, CAST(0 AS FLOAT) AS OSingleWeight, CAST(0 AS FLOAT) 
                                                                                  AS OTotalWeight, CAST(0 AS FLOAT) AS TOTotalWeight, SUM(dbo.Table_012_Child_PwhrsReceipt.column04) AS ONumberInBox, 
                                                                                  CAST(ROUND(ISNULL(SUM(dbo.Table_012_Child_PwhrsReceipt.column07) / NULLIF
                                                                                      ((SELECT        column09
                                                                                          FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                          WHERE        (columnid = dbo.Table_012_Child_PwhrsReceipt.column02)), 0), 0), 3) AS DECIMAL(36, 3)) AS TONumberInBox, 
                                                                                  SUM(dbo.Table_012_Child_PwhrsReceipt.column05) AS ONumberInPack, 
                                                                                  CAST(ROUND(ISNULL(SUM(dbo.Table_012_Child_PwhrsReceipt.column07) / NULLIF
                                                                                      ((SELECT        column08
                                                                                          FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                          WHERE        (columnid = dbo.Table_012_Child_PwhrsReceipt.column02)), 0), 0), 3) AS DECIMAL(36, 3)) AS TONumberInPack, 
                                                                                  SUM(dbo.Table_012_Child_PwhrsReceipt.column06) AS ODetailNumber, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) 
                                                                                  AS OTotalNumber, SUM(ISNULL(dbo.Table_012_Child_PwhrsReceipt.Column34, 0)) AS ISingleWeight, 
                                                                                  SUM(ISNULL(dbo.Table_012_Child_PwhrsReceipt.Column35, 0)) AS ITotalWeight, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) 
                                                                                  * ISNULL
                                                                                      ((SELECT        column22
                                                                                          FROM            dbo.table_004_CommodityAndIngredients AS tcai
                                                                                          WHERE        (columnid = dbo.Table_012_Child_PwhrsReceipt.column02)), 0) AS TITotalWeight
                                                         FROM            dbo.Table_011_PwhrsReceipt INNER JOIN
                                                                                  dbo.Table_012_Child_PwhrsReceipt ON 
                                                                                  dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01 LEFT OUTER JOIN
                                                                                  " + ConBase.Database + @".dbo.Table_035_ProjectInfo AS Table_035_ProjectInfo_1 ON 
                                                                                  dbo.Table_012_Child_PwhrsReceipt.column14 = Table_035_ProjectInfo_1.Column00
                                                        WHERE        dbo.Table_011_PwhrsReceipt.column03 not in (select Column02 from " + ConAcnt.Database + @".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')  
                          GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.column14, Table_035_ProjectInfo_1.Column02) 
                                                         AS TurnTable INNER JOIN
                                                         dbo.table_004_CommodityAndIngredients ON TurnTable.GoodCode = dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
                                                         dbo.table_003_SubsidiaryGroup ON dbo.table_004_CommodityAndIngredients.column04 = dbo.table_003_SubsidiaryGroup.columnid AND 
                                                         dbo.table_004_CommodityAndIngredients.column03 = dbo.table_003_SubsidiaryGroup.column01 INNER JOIN
                                                         dbo.table_002_MainGroup ON dbo.table_003_SubsidiaryGroup.column01 = dbo.table_002_MainGroup.columnid
                               GROUP BY dbo.table_004_CommodityAndIngredients.column26, TurnTable.GoodCode, dbo.table_004_CommodityAndIngredients.column02, 
                                                         dbo.table_004_CommodityAndIngredients.column01, dbo.table_004_CommodityAndIngredients.column07, TurnTable.Project, 
                                                         dbo.table_004_CommodityAndIngredients.column05, dbo.table_003_SubsidiaryGroup.column03, dbo.table_002_MainGroup.column02
                               HAVING         (ROUND(SUM(TurnTable.ITotalNumber) - SUM(TurnTable.OTotalNumber), 3) < 0)) AS Turn ON 
                         " + ConBase.Database + @".dbo.Table_070_CountUnitInfo.Column00 = Turn.UnitCount RIGHT OUTER JOIN
                             (SELECT        " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column02 AS Godcode, " + ConSale.Database + @".dbo.Table_010_SaleFactor.Column42 AS WHRS, 
                                                         SUM(" + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column07) AS TotalNumber, 
                                                         " + ConSale.Database + @".dbo.Table_010_SaleFactor.column02 AS Date
                               FROM            " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor INNER JOIN
                                                         " + ConSale.Database + @".dbo.Table_010_SaleFactor ON 
                                                         " + ConSale.Database + @".dbo.Table_010_SaleFactor.columnid = " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column01
                               GROUP BY " + ConSale.Database + @".dbo.Table_010_SaleFactor.column02, " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor.column02, 
                                                         " + ConSale.Database + @".dbo.Table_010_SaleFactor.Column42
                               HAVING         (" + ConSale.Database + @".dbo.Table_010_SaleFactor.column02 >=  '" + Date1 + @"') AND (" + ConSale.Database + @".dbo.Table_010_SaleFactor.column02 <=  '" + Date2 + @"')) 
                         AS Dtsale INNER JOIN
                         dbo.Table_001_PWHRS ON Dtsale.WHRS = dbo.Table_001_PWHRS.columnid ON Turn.GoodID = Dtsale.Godcode
WHERE        (Turn.GoodID IS NOT NULL)



                        ");


                    StiReport stireport1 = new StiReport();
                    stireport1.Load("Rpt_02_Sale_Negative.mrt");
                    stireport1.Pages["Page1"].Enabled = true;
                    stireport1.Compile();
                    stireport1.RegData("dtNegative", dt1negative);
                    this.Cursor = Cursors.Default;
                    stireport1.Render(false);
                    stiViewerControl1.Report = stireport1;
                    stiViewerControl1.Refresh();
                }

            }
            catch(Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            
            }

          
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Frm_SaleFactorNegative_Load(sender, e);
         
        }

        private void btn_dig_Click(object sender, EventArgs e)
        {
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("Rpt_02_Sale_Negative.mrt");
            r.Design();
        }

        private void btn_Prw_Click(object sender, EventArgs e)
        {
            Frm_SaleFactorNegative_Load(sender, e);
         
        }






    }
}
