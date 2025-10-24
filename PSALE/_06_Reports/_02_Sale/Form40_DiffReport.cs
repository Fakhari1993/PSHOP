using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._06_Reports._02_Sale
{

    public partial class Form40_DiffReport : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWHRD = new SqlConnection(Properties.Settings.Default.WHRS);

        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        public Form40_DiffReport()
        {
            InitializeComponent();
        }

        private void Form40_DiffReport_Load(object sender, EventArgs e)
        {
            try
            {
                gridEX1.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, @" SELECT * FROM (
                                                                                            SELECT View_1.FactorId,
                                                                                                   View_1.FactorDate,
                                                                                                   View_1.FactorNo,
                                                                                                   View_1.GoodId,
                                                                                                   View_1.GoodCode,
                                                                                                   View_1.GoodName,
                                                                                                   View_1.TotalQty AS TotalFactor,
                                                                                                   View_1.DraftId,
                                                                                                   View_2.DraftNo,
                                                                                                   View_2.TotalQty AS TotalDraft,
                                                                                                   View_1.TotalQty -(ISNULL(View_2.TotalQty, 0)) AS DifTotal
                                                                                            FROM   (
                                                                                                       SELECT dbo.Table_010_SaleFactor.columnid AS FactorId,
                                                                                                              dbo.Table_010_SaleFactor.column01 AS FactorNo,
                                                                                                              dbo.Table_010_SaleFactor.column02 AS FactorDate,
                                                                                                              dbo.Table_011_Child1_SaleFactor.column02 AS GoodId,
                                                                                                              " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS 
                                                                                                              GoodCode,
                                                                                                              " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column02 AS 
                                                                                                              GoodName,
                                                                                                              SUM(dbo.Table_011_Child1_SaleFactor.column07) AS TotalQty,
                                                                                                              dbo.Table_010_SaleFactor.column09 AS DraftId
                                                                                                       FROM   dbo.Table_011_Child1_SaleFactor
                                                                                                              INNER JOIN dbo.Table_010_SaleFactor
                                                                                                                   ON  dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
                                                                                                              INNER JOIN " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients
                                                                                                                   ON  dbo.Table_011_Child1_SaleFactor.column02 = 
                                                                                                                       " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.columnid
                                                                                                       GROUP BY
                                                                                                              dbo.Table_010_SaleFactor.columnid,
                                                                                                              dbo.Table_010_SaleFactor.column01,
                                                                                                              dbo.Table_010_SaleFactor.column02,
                                                                                                              " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column01,
                                                                                                              " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column02,
                                                                                                              dbo.Table_010_SaleFactor.column09,
                                                                                                              dbo.Table_011_Child1_SaleFactor.column02
                                                                                                   ) AS View_1
                                                                                                   LEFT JOIN (
                                                                                                            SELECT " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.columnid AS DraftId,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column01 AS DraftNo,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column02 AS 
                                                                                                                   DraftDate,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft.column02 AS 
                                                                                                                   GoodId,
                                                                                                                   " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS 
                                                                                                                   GoodCode,
                                                                                                                   " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column02 AS 
                                                                                                                   GoodName,
                                                                                                                   SUM(" + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft.column07) AS 
                                                                                                                   TotalQty,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column16 AS 
                                                                                                                   FactorId
                                                                                                            FROM   " + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft
                                                                                                                   INNER JOIN " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft
                                                                                                                        ON  " + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft.column01 = 
                                                                                                                            " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.columnid
                                                                                                                   INNER JOIN " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients
                                                                                                                        ON  " + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft.column02 = 
                                                                                                                            " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.columnid
                                                                                                            GROUP BY
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.columnid,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column01,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column02,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft.column02,
                                                                                                                   " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column01,
                                                                                                                   " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column02,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column16
                                                                                                        ) AS View_2
                                                                                                        ON  View_1.GoodId = View_2.GoodId
                                                                                                        AND View_1.FactorId = View_2.FactorId
                                                                                                        AND View_1.DraftId = View_2.DraftId
                                                                                            WHERE  View_1.TotalQty -(ISNULL(View_2.TotalQty, 0)) != 0) AS ff

                                                                                            UNION  all
                                                                                            (
                                                                                            SELECT View_2.FactorId,
                                                                                                   View_2.FactorDate,
                                                                                                   View_2.FactorNo,
                                                                                                   View_1.GoodId,
                                                                                                   View_1.GoodCode,
                                                                                                   View_1.GoodName,
                                                                                                   View_2.TotalQty AS TotalFactor,
                                                                                                   View_1.DraftId,
                                                                                                   View_1.DraftNo,
                                                                                                   View_1.TotalQty AS TotalDraft,
                                                                                                   (ISNULL(View_2.TotalQty, 0)) -(ISNULL(View_1.TotalQty, 0))  AS DifTotal
                                                                                            FROM   (
                                                                                                        SELECT " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.columnid AS DraftId,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column01 AS DraftNo,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column02 AS 
                                                                                                                   DraftDate,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft.column02 AS 
                                                                                                                   GoodId,
                                                                                                                   " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS 
                                                                                                                   GoodCode,
                                                                                                                   " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column02 AS 
                                                                                                                   GoodName,
                                                                                                                   SUM(" + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft.column07) AS 
                                                                                                                   TotalQty,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column16 AS 
                                                                                                                   FactorId
                                                                                                            FROM   " + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft
                                                                                                                   INNER JOIN " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft
                                                                                                                        ON  " + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft.column01 = 
                                                                                                                            " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.columnid
                                                                                                                   INNER JOIN " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients
                                                                                                                        ON  " + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft.column02 = 
                                                                                                                            " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.columnid
                                                                                                            GROUP BY
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.columnid,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column01,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column02,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_008_Child_PwhrsDraft.column02,
                                                                                                                   " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column01,
                                                                                                                   " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column02,
                                                                                                                   " + ConWHRD.Database + @".dbo.Table_007_PwhrsDraft.column16
                                                                                                   ) AS View_1
                                                                                                   LEFT JOIN (
                
                                                                                                             SELECT dbo.Table_010_SaleFactor.columnid AS FactorId,
                                                                                                              dbo.Table_010_SaleFactor.column01 AS FactorNo,
                                                                                                              dbo.Table_010_SaleFactor.column02 AS FactorDate,
                                                                                                              dbo.Table_011_Child1_SaleFactor.column02 AS GoodId,
                                                                                                              " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS 
                                                                                                              GoodCode,
                                                                                                              " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column02 AS 
                                                                                                              GoodName,
                                                                                                              SUM(dbo.Table_011_Child1_SaleFactor.column07) AS TotalQty,
                                                                                                              dbo.Table_010_SaleFactor.column09 AS DraftId
                                                                                                       FROM   dbo.Table_011_Child1_SaleFactor
                                                                                                              INNER JOIN dbo.Table_010_SaleFactor
                                                                                                                   ON  dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
                                                                                                              INNER JOIN " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients
                                                                                                                   ON  dbo.Table_011_Child1_SaleFactor.column02 = 
                                                                                                                       " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.columnid
                                                                                                       GROUP BY
                                                                                                              dbo.Table_010_SaleFactor.columnid,
                                                                                                              dbo.Table_010_SaleFactor.column01,
                                                                                                              dbo.Table_010_SaleFactor.column02,
                                                                                                              " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column01,
                                                                                                              " + ConWHRD.Database + @".dbo.table_004_CommodityAndIngredients.column02,
                                                                                                              dbo.Table_010_SaleFactor.column09,
                                                                                                              dbo.Table_011_Child1_SaleFactor.column02
              
                                                                                                        ) AS View_2
                                                                                                        ON  View_1.GoodId = View_2.GoodId
                                                                                                        AND View_1.FactorId = View_2.FactorId
                                                                                                        AND View_1.DraftId = View_2.DraftId
                                                                                            WHERE  (ISNULL(View_2.TotalQty, 0)) -(ISNULL(View_1.TotalQty, 0)) != 0 AND View_1.FactorId!=0 ) ");
            }
            catch
            {
            }
        }
        private void bt_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX =gridEX1 ;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }
    }
}
