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
    public partial class Form04_SaleReport_NumericMonthly : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        public Form04_SaleReport_NumericMonthly()
        {
            InitializeComponent();
        }

        private void Form04_SaleReport_NumericMonthly_Load(object sender, EventArgs e)
        {
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     Step1.Year, Step1.GoodID, SUM(Step1.Far) AS Far,SUM(Step1.FarBox) AS FarBox,SUM(Step1.FarPack) AS FarPack,SUM(Step1.FarDetail) AS FarDetail,
			SUM(Step1.Ord) AS Ord,SUM(Step1.OrdBox) AS OrdBox,SUM(Step1.OrdPack) AS OrdPack,SUM(Step1.OrdDetail) AS OrdDetail,
			SUM(Step1.Khord) AS Khordad,SUM(Step1.KhordBox) AS KhordadBox,SUM(Step1.KhordPack) AS KhordadPack,SUM(Step1.KhordDetail) AS KhordadDetail,
			SUM(Step1.Tir) AS Tir,SUM(Step1.TirBox) AS TirBox,SUM(Step1.TirPack) AS TirPack,SUM(Step1.TirDetail) AS TirDetail,
			SUM(Step1.Mordad) AS Mordad,SUM(Step1.MordadBox) AS MordadBox,SUM(Step1.MordadPack) AS MordadPack,SUM(Step1.MordadDetail) AS MordadDetail,
			SUM(Step1.Shahr) AS Shahr,SUM(Step1.ShahrBox) AS ShahrBox,SUM(Step1.ShahrPack) AS ShahrPack,SUM(Step1.ShahrDetail) AS ShahrDetail,
			SUM(Step1.Mehr) AS Mehr,SUM(Step1.MehrBox) AS MehrBox,SUM(Step1.MehrPack) AS MehrPack,SUM(Step1.MehrDetail) AS MehrDetail,
			SUM(Step1.Aban) AS Aban,SUM(Step1.AbanBox) AS AbanBox,SUM(Step1.AbanPack) AS AbanPack,SUM(Step1.AbanDetail) AS AbanDetail,
			SUM(Step1.Azar) AS Azar,SUM(Step1.AzarBox) AS AzarBox,SUM(Step1.AzarPack) AS AzarPack,SUM(Step1.AzarDetail) AS AzarDetail,
			SUM(Step1.Dey) AS Dey,SUM(Step1.DeyBox) AS DeyBox,SUM(Step1.DeyPack) AS DeyPack,SUM(Step1.DeyDetail) AS DeyDetail,
			SUM(Step1.Bahman) AS Bahman,SUM(Step1.BahmanBox) AS BahmanBox,SUM(Step1.BahmanPack) AS BahmanPack,SUM(Step1.BahmanDetail) AS BahmanDetail,
			SUM(Step1.Esf) AS Esf,SUM(Step1.EsfBox) AS EsfBox,SUM(Step1.EsfPack) AS EsfPack,SUM(Step1.EsfDetail) AS EsfDetail,
			GoodTable.column01 AS GoodCode, GoodTable.column02 AS GoodName,GoodTable.SubGroup,GoodTable.MainGroup
			,SUM(Step1.FarWeight) as FarWeight
			,SUM(Step1.OrdWeight) as OrdWeight
			,SUM(Step1.khoWeight) as khoWeight
			,SUM(Step1.tirWeight) as tirWeight
			,SUM(Step1.morWeight) as morWeight
			,SUM(Step1.shaWeight) as shaWeight
			,SUM(Step1.mehWeight) as mehWeight
			,SUM(Step1.abaWeight) as abaWeight
			,SUM(Step1.AzaWeight) as AzaWeight
			,SUM(Step1.deyWeight) as deyWeight
			,SUM(Step1.bahWeight) as bahWeight
			,SUM(Step1.esfWeight) as esfWeight
            FROM         (SELECT     TOP (100) PERCENT SUBSTRING(dbo.Table_010_SaleFactor.column02, 1, 4) AS Year, SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) AS Month, 
            dbo.Table_011_Child1_SaleFactor.column02 AS GoodID, SUM(dbo.Table_011_Child1_SaleFactor.column07) AS TotalNumber, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Far, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS FarBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS FarPack, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS FarDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Ord, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS OrdBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS OrdPack, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS OrdDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Khord,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS KhordBox,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS KhordPack,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS KhordDetail,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Tir, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS TirBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS TirPack, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS TirDetail, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Mordad,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS MordadBox,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS MordadPack,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS MordadDetail,
            
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Shahr,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS ShahrBox,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS ShahrPack,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS ShahrDetail,
            
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Mehr,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS MehrBox,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS MehrPack,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS MehrDetail,
            
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Aban,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS AbanBox,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS AbanPack,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS AbanDetail,
            
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Azar,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS AzarBox,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS AzarPack,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS AzarDetail,
            
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Dey, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS DeyBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS DeyPack, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS DeyDetail, 
            
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Bahman, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS BahmanBox, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS BahmanPack, 
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS BahmanDetail, 
            
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column07) ELSE 0 END AS Esf,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column04) ELSE 0 END AS EsfBox,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column05) ELSE 0 END AS EsfPack,
            CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column06) ELSE 0 END AS EsfDetail
           
           ,CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '01' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS FarWeight, 
           CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '02' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS OrdWeight, 
           CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '03' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS KhoWeight, 
           CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '04' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS TirWeight, 
           CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '05' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS MorWeight, 
           CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '06' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS ShaWeight, 
           CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '07' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS MehWeight, 
           CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '08' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS AbaWeight, 
           CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '09' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS AzaWeight, 
           CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '10' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS DeyWeight, 
           CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '11' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS BahWeight, 
           CASE WHEN SUBSTRING(dbo.Table_010_SaleFactor.column02, 6, 2) = '12' THEN SUM(dbo.Table_011_Child1_SaleFactor.column37) ELSE 0 END AS EsfWeight
           
            FROM          dbo.Table_010_SaleFactor INNER JOIN
            dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
            GROUP BY SUBSTRING(dbo.Table_010_SaleFactor.column02, 1, 4), dbo.Table_011_Child1_SaleFactor.column02, SUBSTRING(dbo.Table_010_SaleFactor.column02, 
            6, 2)
            ORDER BY Year, Month, GoodID) AS Step1 LEFT OUTER JOIN
            (SELECT     {1}.dbo.table_004_CommodityAndIngredients.columnid, {1}.dbo.table_004_CommodityAndIngredients.column01, {1}.dbo.table_004_CommodityAndIngredients.column02, 
                     {1}. dbo.table_003_SubsidiaryGroup.column03 AS SubGroup, {1}.dbo.table_002_MainGroup.column02 AS MainGroup
            FROM        {1}.dbo.table_004_CommodityAndIngredients INNER JOIN
            {1}.dbo.table_003_SubsidiaryGroup ON {1}.dbo.table_004_CommodityAndIngredients.column04 = {1}.dbo.table_003_SubsidiaryGroup.columnid AND 
            {1}.dbo.table_004_CommodityAndIngredients.column03 = {1}.dbo.table_003_SubsidiaryGroup.column01 INNER JOIN
            {1}.dbo.table_002_MainGroup ON {1}.dbo.table_003_SubsidiaryGroup.column01 = {1}.dbo.table_002_MainGroup.columnid) AS GoodTable ON Step1.GoodID = GoodTable.columnid
            WHERE     (Step1.Year = '{0}')
            GROUP BY Step1.Year, Step1.GoodID, GoodTable.column01, GoodTable.column02,SubGroup,MainGroup
            order by GoodName", ConSale);
            Adapter.SelectCommand.CommandText = String.Format(Adapter.SelectCommand.CommandText, Class_BasicOperation._Year,ConWare.Database);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);

          
            BindingSource BindSource = new BindingSource();
            BindSource.DataSource = Table;
            gridEX_Goods.DataSource = BindSource;
            bindingNavigator1.BindingSource = BindSource;
            chk_Kol.Checked = true;
            crystalReportViewer1.BackColor = Color.White;
        }

        private void mnu_PrintTable_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_Monthly_Numeric_Table.Clone();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
            {
                    Table.Rows.Add(item.Cells["GoodCode"].Value.ToString(), item.Cells["GoodName"].Value.ToString(),
                    item.Cells["Far"].Value.ToString(), item.Cells["Ord"].Value.ToString(),
                    item.Cells["Khordad"].Value.ToString(), item.Cells["Tir"].Value.ToString(),
                    item.Cells["Mordad"].Value.ToString(), item.Cells["Shahr"].Value.ToString(),
                    item.Cells["Mehr"].Value.ToString(), item.Cells["Aban"].Value.ToString(),
                    item.Cells["Azar"].Value.ToString(), item.Cells["Dey"].Value.ToString(),
                    item.Cells["Bahman"].Value.ToString(), item.Cells["Esf"].Value.ToString(),null,null,null,
                    item.Cells["FarWeight"].Value.ToString(),
                    item.Cells["OrdWeight"].Value.ToString(),
                    item.Cells["KhoWeight"].Value.ToString(),
                    item.Cells["TirWeight"].Value.ToString(),
                    item.Cells["MorWeight"].Value.ToString(),
                    item.Cells["ShaWeight"].Value.ToString(),
                    item.Cells["MehWeight"].Value.ToString(),
                    item.Cells["AbaWeight"].Value.ToString(),
                    item.Cells["AzaWeight"].Value.ToString(),
                    item.Cells["DeyWeight"].Value.ToString(),
                    item.Cells["BahWeight"].Value.ToString(),
                    item.Cells["EsfWeight"].Value.ToString()
                    );
            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 12);
                frm.ShowDialog();
            }
        }

        private void mnu_PrintChart_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.PrintReport();
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
                cmb_Print.ShowDropDown();
            else if (e.Control && e.KeyCode == Keys.F)
                gridEX_Goods.Row = gridEX_Goods.FilterRow.Position;
        }

        private void Form04_SaleReport_NumericMonthly_Activated(object sender, EventArgs e)
        {
            gridEX_Goods.Row = gridEX_Goods.FilterRow.Position;
        }

        private void mnu_PrintBoth_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                if (chk_Kol.Checked)
                {
                    for (int i = 3; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetRow().Cells[i].Value.ToString());
                    }
                }
                else if (chk_Box.Checked)
                {
                    for (int i = 4; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetRow().Cells[i].Value.ToString());
                    }
                }
                else if (chk_Pack.Checked)
                {
                    for (int i = 5; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetRow().Cells[i].Value.ToString());
                    }
                }
                else if (chk_Joz.Checked)
                {
                    for (int i = 6; i <= 50; i = i + 4)
                    {
                        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetRow().Cells[i].Value.ToString());
                    }
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 13, gridEX_Goods.GetRow().Cells["GoodCode"].Text,
                        gridEX_Goods.GetRow().Cells["GoodName"].Text, "بر اساس کارتن");
                    frm.ShowDialog();
                }
            }
            catch { }
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
            if (chk_Weigth.Checked)
            {
                gridEX_Goods.RootTable.Columns["FarWeight"].Visible = true;
                gridEX_Goods.RootTable.Columns["OrdWeight"].Visible = true;
                gridEX_Goods.RootTable.Columns["KhoWeight"].Visible = true;
                gridEX_Goods.RootTable.Columns["MorWeight"].Visible = true;
                gridEX_Goods.RootTable.Columns["TirWeight"].Visible = true;
                gridEX_Goods.RootTable.Columns["ShaWeight"].Visible = true;
                gridEX_Goods.RootTable.Columns["MehWeight"].Visible = true;
                gridEX_Goods.RootTable.Columns["abaWeight"].Visible = true;
                gridEX_Goods.RootTable.Columns["AzaWeight"].Visible = true;
                gridEX_Goods.RootTable.Columns["DeyWeight"].Visible = true;
                gridEX_Goods.RootTable.Columns["BahWeight"].Visible = true;
                gridEX_Goods.RootTable.Columns["EsfWeight"].Visible = true;
            }
            else
            {
                gridEX_Goods.RootTable.Columns["FarWeight"].Visible = false;
                gridEX_Goods.RootTable.Columns["OrdWeight"].Visible = false;
                gridEX_Goods.RootTable.Columns["KhoWeight"].Visible = false;
                gridEX_Goods.RootTable.Columns["MorWeight"].Visible = false;
                gridEX_Goods.RootTable.Columns["TirWeight"].Visible = false;
                gridEX_Goods.RootTable.Columns["ShaWeight"].Visible = false;
                gridEX_Goods.RootTable.Columns["MehWeight"].Visible = false;
                gridEX_Goods.RootTable.Columns["abaWeight"].Visible = false;
                gridEX_Goods.RootTable.Columns["AzaWeight"].Visible = false;
                gridEX_Goods.RootTable.Columns["DeyWeight"].Visible = false;
                gridEX_Goods.RootTable.Columns["BahWeight"].Visible = false;
                gridEX_Goods.RootTable.Columns["EsfWeight"].Visible = false;
            }
        }

        private void gridEX_Goods_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (gridEX_Goods.HitTest(e.X, e.Y) == Janus.Windows.GridEX.GridArea.TotalRow)
            {
                try
                {

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
                            Title.Text = "نمودار ماهیانه تعداد کل فروش کالاها  ";
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
                            Title.Text = "نمودار ماهیانه تعداد کل کارتنهای فروخته شده   " ;
                            crystalReportViewer1.ReportSource = rpt;
                            uiPanel0.AutoHideActive = true;
                        }
                    }
                    else if (chk_Pack.Checked)
                    {
                        DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
                        for (int i = 5; i <= 50; i = i + 4)
                        {
                            Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, gridEX_Goods.GetTotalRow().Cells[i].Value.ToString());
                        }
                        if (Table.Rows.Count > 0)
                        {
                            _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
                            rpt.SetDataSource(Table);
                            TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
                            Title.Text = "نمودار ماهیانه تعداد کل بسته های فروخته شده   ";
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
                            Title.Text = "نمودار ماهیانه تعداد  کل فروخته شده   ";
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
