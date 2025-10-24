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
    public partial class Form35_GoodRestingReport : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);

        public Form35_GoodRestingReport()
        {
            InitializeComponent();
        }

        private void Form35_GoodRestingReport_Load(object sender, EventArgs e)
        {
            DataTable GoodTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients");
            gridEX1.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX1.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
            DataTable Table = clDoc.ReturnTable(ConWare.ConnectionString, @"DECLARE @TurnTable TABLE (
                                                                                    RowNumber INT,
                                                                                    Kind NVARCHAR(1),
                                                                                    ID INT,
                                                                                    DetailID INT,
                                                                                    Date NVARCHAR(10),
                                                                                    GoodID INT,
                                                                                    RNumber DECIMAL(18, 4),
                                                                                    RsinglePrice DECIMAL(18, 4),
                                                                                    RTotalPrice DECIMAL(18, 4),
                                                                                    DNumber DECIMAL(18, 4),
                                                                                    DSinglePrice DECIMAL(18, 4),
                                                                                    DTotalPrice DECIMAL(18, 4),
                                                                                    Remain DECIMAL(18, 4),
                                                                                    RemainFee DECIMAL(18, 4),
                                                                                    TotalFee DECIMAL(18, 4),
                                                                                    Marjoo INT DEFAULT 0,
                                                                                    Factor INT DEFAULT 0,
                                                                                    [Transfer] INT DEFAULT 0,
                                                                                    Produce INT DEFAULT 0,
                                                                                    WHRS SMALLINT,
                                                                                    ArzeshKolWHRS DECIMAL(18, 4),
                                                                                    TedadKolWHRS DECIMAL(18, 4),
                                                                                    ArzeshMinWHRS DECIMAL(18, 4),
                                                                                    daydiff INT,
                                                                                    daycount DECIMAL(18, 4)
                                                                                )

                                                                        INSERT INTO @TurnTable
                                                                          (
                                                                            RowNumber,
                                                                            Kind,
                                                                            ID,
                                                                            DetailID,
                                                                            Date,
                                                                            GoodID,
                                                                            RNumber,
                                                                            RSinglePrice,
                                                                            RTotalPrice,
                                                                            DNumber,
                                                                            DSinglePrice,
                                                                            DTotalPrice,
                                                                            Remain,
                                                                            RemainFee,
                                                                            TotalFee,
                                                                            Marjoo,
                                                                            Factor,
                                                                            [Transfer],
                                                                            Produce,
                                                                            WHRS,
                                                                            ArzeshKolWHRS,
                                                                            TedadKolWHRS,
                                                                            ArzeshMinWHRS,
                                                                            daydiff,
                                                                            daycount
                                                                          )
                                                                        SELECT ROW_NUMBER() OVER(ORDER BY Date, MiladiDate) AS Row,
                                                                               TYPE,
                                                                               ID,
                                                                               DetailID,
                                                                               Date,
                                                                               GoodID,
                                                                               RNumber,
                                                                               RSinglePrice,
                                                                               RTotalPrice,
                                                                               DNumber,
                                                                               DSinglePrice,
                                                                               DTotalPrice,
                                                                               0 AS Remain,
                                                                               0 AS RemainFee,
                                                                               0 AS TotalFee,
                                                                               Marjoo,
                                                                               Factor,
                                                                               [Transfer],
                                                                               Produce,
                                                                               WHRS,
                                                                               ArzeshKolWHRS,
                                                                               TedadKolWHRS,
                                                                               ArzeshMinWHRS,
                                                                               daydiff,
                                                                               daycount
                                                                        FROM   (
                                                                                   SELECT '1' AS TYPE,
                                                                                          dbo.Table_011_PwhrsReceipt.columnid AS ID,
                                                                                          dbo.Table_012_Child_PwhrsReceipt.ColumnId AS DetailID,
                                                                                          dbo.Table_011_PwhrsReceipt.column02 AS Date,
                                                                                          dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodID,
                                                                                          dbo.Table_012_Child_PwhrsReceipt.column07 AS RNumber,
                                                                                          dbo.Table_012_Child_PwhrsReceipt.column20 AS RSinglePrice,
                                                                                          dbo.Table_012_Child_PwhrsReceipt.column21 AS RTotalPrice,
                                                                                          0 AS DNumber,
                                                                                          0 AS DSinglePrice,
                                                                                          0 AS DTotalPrice,
                                                                                          Table_012_Child_PwhrsReceipt.Column16 AS MiladiDate,
                                                                                          dbo.Table_011_PwhrsReceipt.column14 AS Marjoo,
                                                                                          dbo.Table_011_PwhrsReceipt.column13 AS Factor,
                                                                                          ISNULL(dbo.Table_011_PwhrsReceipt.Column26, 0) AS [Transfer],
                                                                                          ISNULL(dbo.Table_011_PwhrsReceipt.Column15, 0) AS Produce,
                                                                                          dbo.Table_011_PwhrsReceipt.Column03 AS WHRS,
                                                                                          0 AS ArzeshKolWHRS,
                                                                                          0 AS TedadKolWHRS,
                                                                                          0 AS ArzeshMinWHRS,
                                                                                          DATEDIFF(
                                                                                              DAY,
                                                                                              CAST(
                                                                                                  " + ConMain.Database + @".dbo.PersianToGregorian(dbo.Table_011_PwhrsReceipt.column02) 
                                                                                                  AS date
                                                                                              ),
                                                                                              CAST(GETDATE() AS date)
                                                                                          ) AS daydiff,
                                                                                          (
                                                                                              dbo.Table_012_Child_PwhrsReceipt.column07 * DATEDIFF(
                                                                                                  DAY,
                                                                                                  CAST(
                                                                                                      " +ConMain.Database+@".dbo.PersianToGregorian(dbo.Table_011_PwhrsReceipt.column02) 
                                                                                                      AS date
                                                                                                  ),
                                                                                                  CAST(GETDATE() AS date)
                                                                                              )
                                                                                          ) AS daycount
                                                                                   FROM   dbo.Table_011_PwhrsReceipt
                                                                                          INNER JOIN dbo.Table_012_Child_PwhrsReceipt
                                                                                               ON  dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
           
                                                                                   UNION ALL
                                                                                   SELECT '2' AS TYPE,
                                                                                          dbo.Table_007_PwhrsDraft.columnid AS ID,
                                                                                          dbo.Table_008_Child_PwhrsDraft.ColumnId AS DetailID,
                                                                                          dbo.Table_007_PwhrsDraft.column02 AS Date,
                                                                                          dbo.Table_008_Child_PwhrsDraft.column02 AS GoodID,
                                                                                          0 AS RNumber,
                                                                                          0 AS RSinlgePrice,
                                                                                          0 AS RTotalPrice,
                                                                                          -1 * dbo.Table_008_Child_PwhrsDraft.column07 AS DNumber,
                                                                                          Table_008_Child_PwhrsDraft.Column15 AS DSinglePrice,
                                                                                          Table_008_Child_PwhrsDraft.Column16 AS DTotalPrice,
                                                                                          Table_008_Child_PwhrsDraft.Column18 AS MiladiDate,
                                                                                          Table_007_PwhrsDraft.Column19 AS Marjoo,
                                                                                          Table_007_PwhrsDraft.Column16 AS Factor,
                                                                                          ISNULL(Table_007_PwhrsDraft.Column31, 0) AS [Transfer],
                                                                                          0 AS Produce,
                                                                                          dbo.Table_007_PwhrsDraft.Column03 AS WHRS,
                                                                                          0 AS ArzeshKolWHRS,
                                                                                          0 AS TedadKolWHRS,
                                                                                          0 AS ArzeshMinWHRS,
                                                                                          DATEDIFF(
                                                                                              DAY,
                                                                                              CAST(
                                                                                                  " + ConMain.Database + @".dbo.PersianToGregorian(dbo.Table_007_PwhrsDraft.column02) 
                                                                                                  AS date
                                                                                              ),
                                                                                              CAST(GETDATE() AS date)
                                                                                          ) AS daydiff,
                                                                                          -1 * (
                                                                                              dbo.Table_008_Child_PwhrsDraft.column07 * DATEDIFF(
                                                                                                  DAY,
                                                                                                  CAST(
                                                                                                      " + ConMain.Database + @".dbo.PersianToGregorian(dbo.Table_007_PwhrsDraft.column02) 
                                                                                                      AS date
                                                                                                  ),
                                                                                                  CAST(GETDATE() AS date)
                                                                                              )
                                                                                          ) AS daycount
                                                                                   FROM   dbo.Table_007_PwhrsDraft
                                                                                          INNER JOIN dbo.Table_008_Child_PwhrsDraft
                                                                                               ON  dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                                                                               ) AS TurnTable
                                                                        ORDER BY
                                                                               TurnTable. goodid,
                                                                               TurnTable.Date
		     
                                                                        SELECT GoodID,
                                                                               SUM(RNumber) + SUM(DNumber) AS mande,
                                                                               SUM(daycount) AS daycount,
                                                                              ROUND( SUM(daycount) / NULLIF(SUM(RNumber) + SUM(DNumber), 0),0) AS [day]
                                                                        FROM   @TurnTable
                                                                        GROUP BY
                                                                               GoodID");
            gridEX1.DataSource = Table;


        }

        private void bt_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }
    }
}
