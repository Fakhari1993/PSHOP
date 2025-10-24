using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;

namespace PSHOP._06_Reports._02_Sale
{
    public partial class Form27_Legal_NumericMonthly_Customer : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents ClsDoc = new Classes.Class_Documents();
        DataTable CustomerTable;
        SqlDataAdapter HeaderAdapter, DetailAdapter;
        DataSet DS1 = new DataSet();

        public Form27_Legal_NumericMonthly_Customer()
        {
            InitializeComponent();
        }

        private void Form04_SaleReport_NumericMonthly_Load(object sender, EventArgs e)
        {
           
             CustomerTable= ClsDoc.ReturnTable(ConBase.ConnectionString, "SELECT     ColumnId, Column01, Column02 FROM         dbo.Table_045_PersonInfo");
           


            DataTable GoodTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from dbo.table_004_CommodityAndIngredients");
            gridEX1.DropDowns["CustomerCode"].SetDataBinding(CustomerTable, "");
            gridEX1.DropDowns["CustomerName"].SetDataBinding(CustomerTable, "");
            gridEX_Goods.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_Goods.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            string HeaderSelect = @"SELECT        CustomerId
            FROM            (SELECT        TOP (100) PERCENT SUBSTRING(Table_010_SaleFactor.column02, 1, 4) AS Year, SUBSTRING(Table_010_SaleFactor.column02, 6, 2) AS Month, 
            Table_010_SaleFactor.column03 AS CustomerId, Table_011_Child1_SaleFactor.column02 AS GoodID, SUM(Table_011_Child1_SaleFactor.column07) 
            AS TotalNumber, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Far, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04)
            ELSE 0 END AS FarBox, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS FarPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02,
            6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS FarDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Ord,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) 
            ELSE 0 END AS OrdBox, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS OrdPack, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) 
            ELSE 0 END AS OrdDetail, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Khord, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 
            6, 2) = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS KhordBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS KhordPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS KhordDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Tir, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) 
            ELSE 0 END AS TirBox, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS TirPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 
            6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS TirDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Mordad, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS MordadBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS MordadPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS MordadDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Shahr, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS ShahrBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS ShahrPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS ShahrDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Mehr, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS MehrBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS MehrPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS MehrDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Aban, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS AbanBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS AbanPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS AbanDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Azar, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS AzarBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS AzarPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS AzarDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Dey, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS DeyBox, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02,
            6, 2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS DeyPack, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) 
            ELSE 0 END AS DeyDetail, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Bahman, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) 
            ELSE 0 END AS BahmanBox, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS BahmanPack, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) 
            ELSE 0 END AS BahmanDetail, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Esf, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 
            2) = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS EsfBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS EsfPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS EsfDetail
            FROM            Table_010_SaleFactor INNER JOIN
            Table_011_Child1_SaleFactor ON Table_010_SaleFactor.columnid = Table_011_Child1_SaleFactor.column01
            GROUP BY SUBSTRING(Table_010_SaleFactor.column02, 1, 4), Table_011_Child1_SaleFactor.column02, SUBSTRING(Table_010_SaleFactor.column02, 6, 2), 
            Table_010_SaleFactor.column03
            ORDER BY Year, Month, GoodID) AS Step1
            WHERE        (Year = {0})
            GROUP BY CustomerId";
            HeaderAdapter = new SqlDataAdapter(string.Format(HeaderSelect, Class_BasicOperation._Year), ConSale);
            HeaderAdapter.Fill(DS1, "Header");

            string DetailSelect = @"SELECT        Year, CustomerId, GoodID, SUM(Far) AS Far, SUM(FarBox) AS FarBox, SUM(FarPack) AS FarPack, SUM(FarDetail) AS FarDetail, SUM(Ord) AS Ord, SUM(OrdBox) 
            AS OrdBox, SUM(OrdPack) AS OrdPack, SUM(OrdDetail) AS OrdDetail, SUM(Khord) AS Khordad, SUM(KhordBox) AS KhordadBox, SUM(KhordPack) AS KhordadPack, 
            SUM(KhordDetail) AS KhordadDetail, SUM(Tir) AS Tir, SUM(TirBox) AS TirBox, SUM(TirPack) AS TirPack, SUM(TirDetail) AS TirDetail, SUM(Mordad) AS Mordad, 
            SUM(MordadBox) AS MordadBox, SUM(MordadPack) AS MordadPack, SUM(MordadDetail) AS MordadDetail, SUM(Shahr) AS Shahr, SUM(ShahrBox) AS ShahrBox, 
            SUM(ShahrPack) AS ShahrPack, SUM(ShahrDetail) AS ShahrDetail, SUM(Mehr) AS Mehr, SUM(MehrBox) AS MehrBox, SUM(MehrPack) AS MehrPack, SUM(MehrDetail)
            AS MehrDetail, SUM(Aban) AS Aban, SUM(AbanBox) AS AbanBox, SUM(AbanPack) AS AbanPack, SUM(AbanDetail) AS AbanDetail, SUM(Azar) AS Azar, SUM(AzarBox) 
            AS AzarBox, SUM(AzarPack) AS AzarPack, SUM(AzarDetail) AS AzarDetail, SUM(Dey) AS Dey, SUM(DeyBox) AS DeyBox, SUM(DeyPack) AS DeyPack, SUM(DeyDetail) 
            AS DeyDetail, SUM(Bahman) AS Bahman, SUM(BahmanBox) AS BahmanBox, SUM(BahmanPack) AS BahmanPack, SUM(BahmanDetail) AS BahmanDetail, SUM(Esf) 
            AS Esf, SUM(EsfBox) AS EsfBox, SUM(EsfPack) AS EsfPack, SUM(EsfDetail) AS EsfDetail
            FROM            (SELECT        TOP (100) PERCENT SUBSTRING(Table_010_SaleFactor.column02, 1, 4) AS Year, SUBSTRING(Table_010_SaleFactor.column02, 6, 2) AS Month, 
            Table_010_SaleFactor.column03 AS CustomerId, Table_011_Child1_SaleFactor.column02 AS GoodID, SUM(Table_011_Child1_SaleFactor.column07) 
            AS TotalNumber, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Far, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04)
            ELSE 0 END AS FarBox, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS FarPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02,
            6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS FarDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Ord,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) 
            ELSE 0 END AS OrdBox, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS OrdPack, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) 
            ELSE 0 END AS OrdDetail, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Khord, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 
            6, 2) = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS KhordBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS KhordPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS KhordDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Tir, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) 
            ELSE 0 END AS TirBox, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS TirPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 
            6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS TirDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Mordad, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS MordadBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS MordadPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS MordadDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Shahr, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS ShahrBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS ShahrPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS ShahrDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Mehr, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS MehrBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS MehrPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS MehrDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Aban, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS AbanBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS AbanPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS AbanDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Azar, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS AzarBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS AzarPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS AzarDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) 
            ELSE 0 END AS Dey, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS DeyBox, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02,
            6, 2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS DeyPack, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) 
            ELSE 0 END AS DeyDetail, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Bahman, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) 
            ELSE 0 END AS BahmanBox, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS BahmanPack, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) 
            ELSE 0 END AS BahmanDetail, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Esf, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 
            2) = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS EsfBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) 
            ELSE 0 END AS EsfPack, CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) 
            = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS EsfDetail
            FROM            Table_010_SaleFactor INNER JOIN
            Table_011_Child1_SaleFactor ON Table_010_SaleFactor.columnid = Table_011_Child1_SaleFactor.column01
            GROUP BY SUBSTRING(Table_010_SaleFactor.column02, 1, 4), Table_011_Child1_SaleFactor.column02, SUBSTRING(Table_010_SaleFactor.column02, 6, 2), 
            Table_010_SaleFactor.column03
            ORDER BY Year, Month, GoodID) AS Step1
            WHERE        (Year = {0})
            GROUP BY Year, CustomerId, GoodID
            ORDER BY Year, CustomerId, GoodID";
            DetailAdapter = new SqlDataAdapter(string.Format(DetailSelect, Class_BasicOperation._Year), ConSale);
            DetailAdapter.Fill(DS1, "Detail");

            DataRelation Relation1 = new DataRelation("R_Header_Detail", DS1.Tables["Header"].Columns["CustomerId"], DS1.Tables["Detail"].Columns["CustomerId"]);

            ForeignKeyConstraint Fkc1 = new ForeignKeyConstraint("F_Header_Detail", DS1.Tables["Header"].Columns["CustomerId"], DS1.Tables["Detail"].Columns["CustomerId"]);
            Fkc1.UpdateRule = Rule.Cascade;
            Fkc1.AcceptRejectRule = AcceptRejectRule.None;
            Fkc1.DeleteRule = Rule.None;

            DS1.Tables["Detail"].Constraints.Add(Fkc1);
            DS1.Relations.Add(Relation1);

            gridEX1.DataSource = DS1.Tables["Header"];
            gridEX_Goods.DataSource = DS1.Tables["Header"];
            gridEX_Goods.DataMember = "R_Header_Detail";

            chk_Kol.Checked = true;
            crystalReportViewer1.BackColor = Color.White;
        }

        private void mnu_PrintTable_Click(object sender, EventArgs e)
        {
            if (chk_Kol.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_MonthlyNumeric_Customer_Detail.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                {
                    Table.Rows.Add(FarsiLibrary.Utils.PersianDate.Now.Year, 1, 1,
                        item.Cells["Far"].Value.ToString(), 0, 0, 0,
                        item.Cells["Ord"].Value.ToString(), 0, 0, 0,
                        item.Cells["Khordad"].Value.ToString(), 0, 0, 0,
                        item.Cells["Tir"].Value.ToString(), 0, 0, 0,
                        item.Cells["Mordad"].Value.ToString(), 0, 0, 0,
                        item.Cells["Shahr"].Value.ToString(), 0, 0, 0,
                        item.Cells["Mehr"].Value.ToString(), 0, 0, 0,
                        item.Cells["Aban"].Value.ToString(), 0, 0, 0,
                        item.Cells["Azar"].Value.ToString(), 0, 0, 0,
                        item.Cells["Dey"].Value.ToString(), 0, 0, 0,
                        item.Cells["Bahman"].Value.ToString(), 0, 0, 0,
                        item.Cells["Esf"].Value.ToString(), 0, 0, 0,
                        item.Cells["GoodName"].Text,
                        item.Cells["GoodCode"].Text);
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 34,
                        gridEX1.GetRow().Cells["CustomerCode"].Text, gridEX1.GetRow().Cells["CustomerName"].Text,
                        "بر اساس تعداد کل");
                    frm.ShowDialog();
                }
            }
            else if (chk_Box.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_MonthlyNumeric_Customer_Detail.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                {
                    Table.Rows.Add(FarsiLibrary.Utils.PersianDate.Now.Year, 1, 1,
                         item.Cells["FarBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["OrdBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["KhordadBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["TirBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["MordadBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["ShahrBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["MehrBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["AbanBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["AzarBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["DeyBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["BahmanBox"].Value.ToString(), 0, 0, 0,
                         item.Cells["EsfBox"].Value.ToString(), 0, 0, 0,
                        item.Cells["GoodName"].Text,
                        item.Cells["GoodCode"].Text);
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 34,
                        gridEX1.GetRow().Cells["CustomerCode"].Text, gridEX1.GetRow().Cells["CustomerName"].Text,
                        "بر اساس تعداد کارتن");
                    frm.ShowDialog();
                }
            }
            else if (chk_Pack.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_MonthlyNumeric_Customer_Detail.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                {
                    Table.Rows.Add(FarsiLibrary.Utils.PersianDate.Now.Year, 1, 1,
                         item.Cells["FarPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["OrdPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["KhordadPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["TirPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["MordadPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["ShahrPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["MehrPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["AbanPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["AzarPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["DeyPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["BahmanPack"].Value.ToString(), 0, 0, 0,
                         item.Cells["EsfPack"].Value.ToString(), 0, 0, 0,
                        item.Cells["GoodName"].Text,
                        item.Cells["GoodCode"].Text);
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 34,
                        gridEX1.GetRow().Cells["CustomerCode"].Text, gridEX1.GetRow().Cells["CustomerName"].Text,
                        "بر اساس تعداد بسته");
                    frm.ShowDialog();
                }
            }
            else if (chk_Joz.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_MonthlyNumeric_Customer_Detail.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                {
                    Table.Rows.Add(FarsiLibrary.Utils.PersianDate.Now.Year, 1, 1,
                         item.Cells["FarDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["OrdDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["KhordadDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["TirDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["MordadDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["ShahrDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["MehrDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["AbanDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["AzarDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["DeyDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["BahmanDetail"].Value.ToString(), 0, 0, 0,
                         item.Cells["EsfDetail"].Value.ToString(), 0, 0, 0,
                        item.Cells["GoodName"].Text,
                        item.Cells["GoodCode"].Text);
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 34,
                        gridEX1.GetRow().Cells["CustomerCode"].Text, gridEX1.GetRow().Cells["CustomerName"].Text,
                        "بر اساس تعداد جز");
                    frm.ShowDialog();
                }
            }
        }


        private void mnu_Excel_Table_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void mnu_Send_Chart_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.ExportReport();
        }

        private void gridEX_Accounts_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            try
            {
                if (chk_Kol.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i = 3; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, e.Row.Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار ماهیانه تعداد کل فروخته شده کالای  " + e.Row.Cells["GoodName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
                else if (chk_Box.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i = 4; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, e.Row.Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار ماهیانه تعداد کارتن فروخته شده کالای  " + e.Row.Cells["GoodName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
                else if (chk_Pack.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i =5; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, e.Row.Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار ماهیانه تعداد بسته فروخته شده کالای  " + e.Row.Cells["GoodName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
                else if (chk_Joz.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i = 6; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, e.Row.Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار ماهیانه تعداد فروخته شده کالای  " + e.Row.Cells["GoodName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
            }
            catch 
            {
            }
        }

        private void Form04_SaleReport_NumericMonthly_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.E)
                cmb_ExportToExcel.ShowDropDown();
            else if (e.Control && e.KeyCode == Keys.P)
                mnu_PrintTable_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
                gridEX_Goods.Row = gridEX_Goods.FilterRow.Position;
        }

        private void Form04_SaleReport_NumericMonthly_Activated(object sender, EventArgs e)
        {
            gridEX_Goods.Row = gridEX_Goods.FilterRow.Position;
        }


        private void chk_Kol_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Kol.Checked)
            {
                gridEX_Goods.RootTable.Columns["Far"].Visible = true;
                gridEX_Goods.RootTable.Columns["Ord"].Visible = true;
                gridEX_Goods.RootTable.Columns["Khordad"].Visible = true;
                gridEX_Goods.RootTable.Columns["Tir"].Visible = true;
                gridEX_Goods.RootTable.Columns["Mordad"].Visible = true;
                gridEX_Goods.RootTable.Columns["Shahr"].Visible = true;
                gridEX_Goods.RootTable.Columns["Mehr"].Visible = true;
                gridEX_Goods.RootTable.Columns["Aban"].Visible = true;
                gridEX_Goods.RootTable.Columns["Azar"].Visible = true;
                gridEX_Goods.RootTable.Columns["Dey"].Visible = true;
                gridEX_Goods.RootTable.Columns["Bahman"].Visible = true;
                gridEX_Goods.RootTable.Columns["Esf"].Visible = true;
            }
            else
            {
                gridEX_Goods.RootTable.Columns["Far"].Visible = false;
                gridEX_Goods.RootTable.Columns["Ord"].Visible = false;
                gridEX_Goods.RootTable.Columns["Khordad"].Visible = false;
                gridEX_Goods.RootTable.Columns["Tir"].Visible = false;
                gridEX_Goods.RootTable.Columns["Mordad"].Visible = false;
                gridEX_Goods.RootTable.Columns["Shahr"].Visible = false;
                gridEX_Goods.RootTable.Columns["Mehr"].Visible = false;
                gridEX_Goods.RootTable.Columns["Aban"].Visible = false;
                gridEX_Goods.RootTable.Columns["Azar"].Visible = false;
                gridEX_Goods.RootTable.Columns["Dey"].Visible = false;
                gridEX_Goods.RootTable.Columns["Bahman"].Visible = false;
                gridEX_Goods.RootTable.Columns["Esf"].Visible = false;
            }

            if (chk_Box.Checked)
            {
                gridEX_Goods.RootTable.Columns["FarBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["OrdBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["KhordadBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["TirBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["MordadBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["ShahrBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["MehrBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["AbanBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["AzarBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["DeyBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["BahmanBox"].Visible = true;
                gridEX_Goods.RootTable.Columns["EsfBox"].Visible = true;
            }
            else
            {
                gridEX_Goods.RootTable.Columns["FarBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["OrdBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["KhordadBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["TirBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["MordadBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["ShahrBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["MehrBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["AbanBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["AzarBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["DeyBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["BahmanBox"].Visible = false;
                gridEX_Goods.RootTable.Columns["EsfBox"].Visible = false;
            }

            if (chk_Pack.Checked)
            {
                gridEX_Goods.RootTable.Columns["FarPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["OrdPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["KhordadPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["TirPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["MordadPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["ShahrPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["MehrPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["AbanPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["AzarPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["DeyPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["BahmanPack"].Visible = true;
                gridEX_Goods.RootTable.Columns["EsfPack"].Visible = true;
            }
            else
            {
                gridEX_Goods.RootTable.Columns["FarPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["OrdPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["KhordadPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["TirPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["MordadPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["ShahrPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["MehrPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["AbanPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["AzarPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["DeyPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["BahmanPack"].Visible = false;
                gridEX_Goods.RootTable.Columns["EsfPack"].Visible = false;
            }
             
            if (chk_Joz.Checked)
            {
                gridEX_Goods.RootTable.Columns["FarDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["OrdDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["KhordadDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["TirDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["MordadDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["ShahrDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["MehrDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["AbanDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["AzarDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["DeyDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["BahmanDetail"].Visible = true;
                gridEX_Goods.RootTable.Columns["EsfDetail"].Visible = true;
            }
            else
            {
                gridEX_Goods.RootTable.Columns["FarDetail"].Visible =  false;
                gridEX_Goods.RootTable.Columns["OrdDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["KhordadDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["TirDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["MordadDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["ShahrDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["MehrDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["AbanDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["AzarDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["DeyDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["BahmanDetail"].Visible = false;
                gridEX_Goods.RootTable.Columns["EsfDetail"].Visible = false;
            }
        }

        private void mnu_PrintAll_Click(object sender, EventArgs e)
        {
            if (chk_Kol.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetRows())
                {
                    gridEX1.MoveTo(Row);
                   
                        Table.Rows.Add(Row.Cells["CustomerCode"].Text.ToString(),
                            Row.Cells["CustomerName"].Text.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Far"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Ord"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Khordad"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Tir"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Mordad"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Shahr"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Mehr"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Aban"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Azar"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Dey"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Bahman"].Value.ToString(),
                            gridEX_Goods.GetTotalRow().Cells["Esf"].Value.ToString());
                }
               
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 36,
                       Class_BasicOperation._Year,"بر اساس تعداد کل");
                    frm.ShowDialog();
                }
            }
            else if (chk_Box.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetRows())
                {
                    gridEX1.MoveTo(Row);

                    Table.Rows.Add(Row.Cells["CustomerCode"].Text.ToString(),
                        Row.Cells["CustomerName"].Text.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["FarBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["OrdBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["KhordadBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["TirBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MordadBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["ShahrBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MehrBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AbanBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AzarBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["DeyBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["BahmanBox"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["EsfBox"].Value.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 36,
                        Class_BasicOperation._Year, "بر اساس تعداد کارتن");
                    frm.ShowDialog();
                }
            }
            else if (chk_Pack.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetRows())
                {
                    gridEX1.MoveTo(Row);

                    Table.Rows.Add(Row.Cells["CustomerCode"].Text.ToString(),
                        Row.Cells["CustomerName"].Text.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["FarPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["OrdPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["KhordadPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["TirPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MordadPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["ShahrPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MehrPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AbanPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AzarPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["DeyPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["BahmanPack"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["EsfPack"].Value.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 36,
                       Class_BasicOperation._Year, "بر اساس تعداد بسنه");
                    frm.ShowDialog();
                }
            }
            else if (chk_Joz.Checked)
            {
                DataTable Table = dataSet_Report.Rpt_Sale_Monthly_Value_Customer.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX1.GetRows())
                {
                    gridEX1.MoveTo(Row);

                    Table.Rows.Add(Row.Cells["CustomerCode"].Text.ToString(),
                        Row.Cells["CustomerName"].Text.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["FarDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["OrdDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["KhordadDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["TirDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MordadDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["ShahrDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["MehrDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AbanDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["AzarDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["DeyDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["BahmanDetail"].Value.ToString(),
                        gridEX_Goods.GetTotalRow().Cells["EsfDetail"].Value.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 36,
                       Class_BasicOperation._Year, "بر اساس تعداد جز");
                    frm.ShowDialog();
                }
            }
        }

        private void gridEX_Goods_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gridEX_Goods.HitTest(e.X, e.Y) == Janus.Windows.GridEX.GridArea.TotalRow)
            {
                try{
                if (chk_Kol.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i = 3; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetTotalRow().Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار مقایسه ماهیانه تعداد کل کالای فروخته شده به  " + gridEX1.GetRow().Cells["CustomerName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
                else if (chk_Box.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i = 4; i <= 50; i = i + 4)
                    {
                       Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetTotalRow().Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار مقایسه ماهیانه تعداد کارتن کالاهای فروخته شده به  " + gridEX1.GetRow().Cells["CustomerName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
                else if (chk_Pack.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i =5; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetTotalRow().Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار مقایسه ماهیانه تعداد بسته کالاهای فروخته شده به  " + gridEX1.GetRow().Cells["CustomerName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
                else if (chk_Joz.Checked)
                {
                    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                    for (int i = 6; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetTotalRow().Cells[i].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                        rpt.SetDataSource(Table);
                        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                        Title.Text = "نمودار مقایسه ماهیانه تعداد  کالای فروخته شده به  " + gridEX1.GetRow().Cells["CustomerName"].Text;
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel0.AutoHideActive = true;
                    }
                }
            }
            catch 
            {
            }
            }
        }

     
    }
}
