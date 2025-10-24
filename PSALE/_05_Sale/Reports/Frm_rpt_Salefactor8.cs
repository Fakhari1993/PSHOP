using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Report;
using System.Data.SqlClient;
namespace PSHOP._05_Sale.Reports
{
    public partial class Frm_rpt_Salefactor8 : Form
    {

        Classes.Class_Settle cl_Settle = new Classes.Class_Settle();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        DataTable HeaderTable;
        DataTable DetailTable;
        short _PrintStyle = 1;
        List<string> List = new List<string>();
        int _SaleNumber;
        public Frm_rpt_Salefactor8(int SaleNumber)
        {
            _SaleNumber = SaleNumber;

            InitializeComponent();
        }
        public Frm_rpt_Salefactor8(List<string> _List, int SaleNumber, short style)
        {
            InitializeComponent();
            List = _List;
            _PrintStyle = style;
            _SaleNumber = SaleNumber;
        }
        public void Frm_rpt_Salefactor8_Load(object sender, EventArgs e)
        {
            string HeaderSelectText = null;

            HeaderSelectText = @"SELECT     FactorTable.FactorID AS ID,DetailID, FactorTable.Serial, FactorTable.LegalNumber, FactorTable.Date, FactorTable.Responsible, FactorTable.CustomerID, FactorTable.P2Name, FactorTable.Mobile,
                                 FactorTable.P2NationalCode,
                                   FactorTable.P2ECode,FactorTable.Description2,{1}.dbo.table_004_CommodityAndIngredients.Column36 as Ficonsumer ,{1}.dbo.Table_003_InformationProductCash.column04 ,
       FactorTable.GoodNameTotal,
                                   FactorTable.P2SabtCode, FactorTable.P2Address, FactorTable.P2Tel, FactorTable.P2Fax, FactorTable.P2PostalCode, FactorTable.P2Code, FactorTable.GoodCode, 
                                FactorTable.GoodName, FactorTable.Box, FactorTable.BoxPrice, FactorTable.Pack, FactorTable.PackPrice, FactorTable.Number, FactorTable.TotalNumber, 
                                FactorTable.SinglePrice, FactorTable.TotalPrice, FactorTable.DiscountPercent, FactorTable.DiscountPrice, FactorTable.TaxPrice,FactorTable.TotalWeight,FactorTable.TaxPercent, FactorTable.NetPrice, 
                                ISNULL(FactorTable.Ezafat, 0) AS Ezafat, ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat, PersonInfoTable.Column02, FactorTable.NetTotal,FactorTable.Cash,FactorTable.Cart,FactorTable.Etebari,FactorTable.[Check],FactorTable.Bon,FactorTable.VolumeGroup,
                                FactorTable.SpecialGroup, FactorTable.SpecialCustomer, FactorTable.Description, FactorTable.CountUnitName, derivedtbl_1.Groups, '-' AS charPrice, 
                                'SettleInfo' AS SettleInfo, FactorTable.FactorType, FactorTable.NumberInBox, FactorTable.RowDes, FactorTable.Zarib, FactorTable.NumberInBox AS Expr1, 
                                FactorTable.NumberInPack, CityTable.Column02 AS CityName, ProvinceTable.Column01 AS ProvinceName,FactorTable.PayCash,FactorTable.DraftNumber,FactorTable.DocID,FactorTable.Project,FactorTable.BuyerName,FactorTable.SaleType,FactorTable.DocNum
                                FROM         (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
                                FROM         {0}.dbo.Table_060_ProvinceInfo) AS ProvinceTable INNER JOIN
                                (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                                FROM         {0}.dbo.Table_065_CityInfo) AS CityTable ON ProvinceTable.Column00 = CityTable.Column00 RIGHT OUTER JOIN
                                (SELECT     dbo.Table_010_SaleFactor.columnid AS FactorID, dbo.Table_010_SaleFactor.column01 AS Serial, dbo.Table_010_SaleFactor.column37 AS LegalNumber, 
                                dbo.Table_010_SaleFactor.column02 AS Date,isnull(dbo.Table_010_SaleFactor.Column65,'') AS BuyerName,isnull((select column02 from {0}.dbo.Table_002_SalesTypes where columnid=dbo.Table_010_SaleFactor.Column36),'') as SaleType, dbo.Table_010_SaleFactor.column03 AS CustomerID, PersonTable.Column02 AS P2Name,PersonTable.Column19 AS Mobile, 
                                  PersonTable.Column09 AS P2NationalCode,
                                                   PersonTable.Column141 AS P2ECode,
                                                   PersonTable.Column142 AS P2SabtCode, PersonTable.Column01 AS P2Code, PersonTable.Column06 AS P2Address, PersonTable.Column07 AS P2Tel, 
                                PersonTable.Column08 AS P2Fax, PersonTable.Column13 AS P2PostalCode, GoodTable.column01 AS GoodCode, 
                                dbo.Table_011_Child1_SaleFactor.column04 AS Box, dbo.Table_011_Child1_SaleFactor.column08 AS BoxPrice, 
                                dbo.Table_011_Child1_SaleFactor.column05 AS Pack, dbo.Table_011_Child1_SaleFactor.column09 AS PackPrice, 
                                dbo.Table_011_Child1_SaleFactor.column06 AS Number, dbo.Table_011_Child1_SaleFactor.column07 AS TotalNumber, 
                                dbo.Table_011_Child1_SaleFactor.column10 AS SinglePrice, dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice, dbo.Table_011_Child1_SaleFactor.column23 AS Description2,
                                dbo.Table_011_Child1_SaleFactor.column16 AS DiscountPercent, dbo.Table_011_Child1_SaleFactor.column17 AS DiscountPrice, dbo.Table_011_Child1_SaleFactor.column18 as TaxPercent,
                                dbo.Table_011_Child1_SaleFactor.column19 AS TaxPrice,dbo.Table_011_Child1_SaleFactor.column37 as TotalWeight, dbo.Table_011_Child1_SaleFactor.column20 AS NetPrice, GoodTable.column02 AS GoodName,GoodTable.column05 AS GoodNameTotal, 
                                OtherPrice.PlusPrice AS Ezafat, OtherPrice.MinusPrice AS Kosoorat, dbo.Table_010_SaleFactor.column05 AS Responsible, 
                                dbo.Table_010_SaleFactor.Column28 AS NetTotal, dbo.Table_010_SaleFactor.column46 AS Cash,dbo.Table_010_SaleFactor.column47 AS Cart,dbo.Table_010_SaleFactor.column48 AS Etebari,
                                dbo.Table_010_SaleFactor.column52 AS [Check],dbo.Table_010_SaleFactor.column54 AS [Bon],dbo.Table_010_SaleFactor.Column29 AS VolumeGroup, 
                                dbo.Table_010_SaleFactor.Column30 AS SpecialGroup, dbo.Table_010_SaleFactor.Column31 AS SpecialCustomer, 
                                dbo.Table_010_SaleFactor.column06 AS Description, CountUnitTable.Column01 AS CountUnitName, 
                                CASE WHEN Table_010_SaleFactor.Column12 = 0 THEN '***فاکتور ریالی***' ELSE '***فاکتور ارزی***' END AS FactorType, 
                                dbo.Table_011_Child1_SaleFactor.column23 AS RowDes, dbo.Table_011_Child1_SaleFactor.column31 AS NumberInBox, 
                                dbo.Table_011_Child1_SaleFactor.column32 AS NumberInPack, dbo.Table_011_Child1_SaleFactor.Column33 AS Zarib, 
                                PersonTable.Column21 AS ProvinceId, PersonTable.Column22 AS CityId,Table_010_SaleFactor.column21 as PayCash,
                                (Select Column01 from  {1}.dbo.Table_007_PwhrsDraft where Columnid=Table_010_SaleFactor.Column09) as DraftNumber,
                                Table_010_SaleFactor.Column10 as DocId,(select isnull(Column00,0) from " + ConAcnt.Database+@".dbo.Table_060_SanadHead where ColumnId=Table_010_SaleFactor.Column10)  as DocNum,       Project.column02 as Project,dbo.Table_011_Child1_SaleFactor.columnid as DetailID
 

                                FROM         dbo.Table_010_SaleFactor INNER JOIN
                                dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                                (SELECT     ColumnId, Column00, Column01, Column02,Column19, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, 
                                Column11, Column12, Column13, Column21, Column22       ,Column141,
                                                                   Column142
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
                                FROM         {0}.dbo.Table_070_CountUnitInfo) AS CountUnitTable ON dbo.Table_011_Child1_SaleFactor.column03 = CountUnitTable.Column00  LEFT OUTER JOIN (
                                SELECT Column00,
                                       Column02
                                FROM   {0}.dbo.Table_035_ProjectInfo
                            ) AS Project
                            ON  dbo.Table_011_Child1_SaleFactor.column22 = 
                                Project.Column00 ) 
                                AS FactorTable ON CityTable.Column01 = FactorTable.CityId LEFT OUTER JOIN
                                (SELECT     PersonId, Groups
                                FROM         {0}.dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1) AS derivedtbl_1 ON FactorTable.CustomerID = derivedtbl_1.PersonId LEFT OUTER JOIN
                                (SELECT     ColumnId, Column01, Column02, Column21, Column22
                                FROM         {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1) AS PersonInfoTable ON FactorTable.Responsible = PersonInfoTable.ColumnId    LEFT OUTER JOIN
                         {1}.dbo.table_004_CommodityAndIngredients ON FactorTable.GoodCode = {1}.dbo.table_004_CommodityAndIngredients.column01   left outer join 
					   {1}.dbo.Table_003_InformationProductCash ON 
                          {1}.dbo.table_004_CommodityAndIngredients.columnid =  {1}.dbo.Table_003_InformationProductCash.column01";



            string DetailSelectText = @"SELECT     dbo.Table_024_Discount.column01 AS Name, dbo.Table_012_Child2_SaleFactor.column04 AS Price, 
                      CASE WHEN Table_024_Discount.column02 = 0 THEN '+' ELSE '-' END AS Type, dbo.Table_012_Child2_SaleFactor.column01 AS Column01 ,
                      dbo.Table_010_SaleFactor.column01 AS HeaderNum, dbo.Table_010_SaleFactor.column02 AS HeaderDate
                      FROM         dbo.Table_012_Child2_SaleFactor INNER JOIN
                      dbo.Table_010_SaleFactor ON dbo.Table_012_Child2_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid LEFT OUTER JOIN
                      dbo.Table_024_Discount ON dbo.Table_012_Child2_SaleFactor.column02 = dbo.Table_024_Discount.columnid";


            HeaderSelectText += " WHERE     (FactorTable.Serial = " + _SaleNumber + ")";
            DetailSelectText += " WHERE (Table_010_SaleFactor.Column01= " + _SaleNumber + ")";
            HeaderSelectText += " ORDER BY  FactorTable.FactorID,FactorTable.DetailID";

            HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);
            HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
            DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);

            try
            {
            

                StiReport stireport = new StiReport();
                stireport.Load("Sales Invoice 80mm.mrt");
                stireport.Pages["Page3"].Enabled = true;
                stireport.Compile();
                stireport.RegData("Rpt_SaleTable", HeaderTable);
                stireport.RegData("Rpt_SaleExtra_Table", DetailTable);
                //stireport["Image"] = Image.FromStream(stream);
                this.Cursor = Cursors.Default;
                stireport.Render(false);
                stiViewerControl1.Report = stireport;



            }

            catch (Exception ex)
            { Class_BasicOperation.CheckExceptionType(ex, this.Name); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("Sales Invoice 80mm.mrt");
            stireport.Design();
        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {
            Frm_rpt_Salefactor8_Load(sender, e);
        }
    }
}
