using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._09_SellerProfit
{
    public partial class Form14_CalcuteSellerProfit : Form
    {
        bool _BackSpace = false;
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);

        DataTable dt = new DataTable();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        public Form14_CalcuteSellerProfit()
        {
            InitializeComponent();
        }

        private void cmb_Print_Click(object sender, EventArgs e)
        {
            try
            {
                gridEXPrintDocument1.GridEX = gridEX_Goods;
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string g = string.Empty;
                        if (rb_Average.Checked)
                            g = "میانگین";
                        if (rb_LastBuyAmount.Checked)
                            g = "آخرین قیمت خرید";
                        if (rb_RTable.Checked)
                            g = "جدول پیشنهادی";
                        string j = " از تاریخ:" + faDatePicker1.Text + " تا تاریخ:" + faDatePicker2.Text + " شخص:" + mlt_SaleMan.Text + " فی خرید براساس:" + g;
                        gridEXPrintDocument1.PageHeaderLeft = j;
                        gridEXPrintDocument1.PageHeaderRight = "محاسبه پورسانت مسئول فروش";
                        printPreviewDialog1.ShowDialog();
                        gridEXPrintDocument1.PageFooterLeft =
                      FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd") +
                      "**" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                        gridEXPrintDocument1.PageFooterRight =
                          " کاربر " + Class_BasicOperation._UserName;
                    }
            }
            catch { }
        }

        private void btn_Calc_Click(object sender, EventArgs e)
        {
            if (faDatePicker1.SelectedDateTime != null && faDatePicker2.SelectedDateTime != null
                && mlt_SaleMan.Value != null && !string.IsNullOrWhiteSpace(mlt_SaleMan.Value.ToString())
                && (rb_Average.Checked || rb_LastBuyAmount.Checked || rb_RTable.Checked))
            {
                string commad = string.Empty;
                commad = @"
SELECT total.*,
       total.TotalScore * total.RialProfitCoefficient AS SaleManProfitAmount
FROM   (
           SELECT final.*,
                  ISNULL(final.ScoreSum / NULLIF(CAST(final.Goal AS DECIMAL), 0), 1) AS 
                  GoalRatio,
                  (
                      (
                          (
                              CASE 
                                   WHEN final.HolidayRatio > 1 THEN final.HolidayRatio
                                   WHEN final.HolidayRatio = 1
                              AND final.TimeRatio > 1 THEN final.TimeRatio
                                  ELSE 1 END
                          ) + final.CustomerGroupRatio + final.SalefactorTypeRatio 
                          + final.EvaluationRatio
                          + ISNULL(final.ScoreSum / NULLIF(CAST(final.Goal AS DECIMAL), 0), 1)
                      ) * final.DiscountRatio * final.Score
                  ) AS TotalScore
           FROM   (
                      SELECT *,
                             (
                                 tt.BuyFee + ISNULL(
                                     (
                                         (
                                             (
                                                 CASE 
                                                      WHEN tt.DeallerAmount = 0 THEN 
                                                           tt.GoodAmount
                                                      ELSE tt.DeallerAmount
                                                 END
                                             )
                                             -tt.GoodAmount
                                         ) * tt.DeallerPrecent
                                     ) / CAST(100 AS DECIMAL),
                                     0
                                 )
                             ) * tt.GoodValue AS BuyAmount,
                             (
                                 (
                                     tt.BuyFee + ISNULL(
                                         (
                                             (
                                                 (
                                                     CASE 
                                                          WHEN tt.DeallerAmount 
                                                               = 0 THEN tt.GoodAmount
                                                          ELSE tt.DeallerAmount
                                                     END
                                                 )
                                                 -tt.GoodAmount
                                             ) * tt.DeallerPrecent
                                         ) / CAST(100 AS DECIMAL),
                                         0
                                     )
                                 ) * tt.GoodValue
                             ) -tt.NetPrice AS ProfitAmount,
                             (
                                 (
                                     (
                                         (
                                             tt.BuyFee + ISNULL(
                                                 (
                                                     (
                                                         (
                                                             CASE 
                                                                  WHEN tt.DeallerAmount 
                                                                       = 0 THEN 
                                                                       tt.GoodAmount
                                                                  ELSE tt.DeallerAmount
                                                             END
                                                         )
                                                         -tt.GoodAmount
                                                     ) * tt.DeallerPrecent
                                                 ) / CAST(100 AS DECIMAL),
                                                 0
                                             )
                                         ) * tt.GoodValue
                                     ) -tt.NetPrice
                                 ) / ISNULL(CAST(ScoreCoefficient AS DECIMAL), 0)
                             ) * tt.DarsadHamkari AS Score,
                             ISNULL(
                                 (
                                     SELECT Column02
                                     FROM   Table_78_DiscountRatio tdr
                                     WHERE  tdr.Column00 >= tt.DiscountP
                                            AND tdr.Column01 <= tt.DiscountP
                                 ),
                                 1
                             ) AS DiscountRatio,
                             ISNULL(
                                 (
                                     SELECT thr.Column01
                                     FROM   Table_71_HolidayRatio thr
                                     WHERE  thr.Column00 = tt.FactorDate
                                 ),
                                 1
                             ) AS HolidayRatio,
                             ISNULL(
                                 (
                                     SELECT ttr.Column02
                                     FROM   Table_72_TimeRatio ttr
                                     WHERE  CAST(ttr.Column00 AS TIME) >= CAST(tt.FactorTime AS TIME)
                                            AND CAST(ttr.Column01 AS TIME) <= 
                                                CAST(tt.FactorTime AS TIME)
                                 ),
                                 1
                             ) AS TimeRatio,
                             ISNULL(
                                 (
                                     (
                                         (
                                             CASE 
                                                  WHEN tt.DeallerAmount = 0 THEN 
                                                       tt.GoodAmount
                                                  ELSE tt.DeallerAmount
                                             END
                                         )
                                         -tt.GoodAmount
                                     ) * tt.DeallerPrecent
                                 ) / CAST(100 AS DECIMAL),
                                 0
                             ) AS DeallerShare,
                             (
                                 SELECT ISNULL(
                                            SUM(
                                                ISNULL(
                                                    (
                                                        (
                                                            (
                                                                (
                                                                    (
                                                                        gg.BuyFee 
                                                                        + ISNULL(
                                                                            (
                                                                                (
                                                                                    (
                                                                                        CASE 
                                                                                             WHEN 
                                                                                                  gg.DeallerAmount 
                                                                                                  =
                                                                                                  0 THEN 
                                                                                                  gg.GoodAmount
                                                                                             ELSE 
                                                                                                  gg.DeallerAmount
                                                                                        END
                                                                                    )
                                                                                    -
                                                                                    gg.GoodAmount
                                                                                ) 
                                                                                *
                                                                                gg.DeallerPrecent
                                                                            ) /
                                                                            CAST(100 AS DECIMAL),
                                                                            0
                                                                        )
                                                                    ) * gg.GoodValue
                                                                ) -gg.NetPrice
                                                            ) / ISNULL(CAST(ScoreCoefficient AS DECIMAL), 0)
                                                        ) * gg.DarsadHamkari
                                                    ),
                                                    0
                                                )
                                            ),
                                            0
                                        ) AS Score
                                 FROM   (
                                            SELECT tcsf.column07 AS [GoodValue],
                                                   tcsf.column10 AS [GoodAmount],
                                                   tcsf.column20 AS NetPrice,
                                                   ISNULL(
                                                       (
                                                           CASE 
                                                                WHEN " + (rb_Average.Checked ? "1" : "0") + @" = 1 THEN (
                                                                         SELECT 
                                                                                ISNULL(tcpd.column15, 0)
                                                                         FROM   
                                                                                " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft 
                                                                                tcpd
                                                                         WHERE  
                                                                                tcpd.column02 = 
                                                                                tcai.columnid
                                                                                AND 
                                                                                    tcpd.column01 = 
                                                                                    tsf.column09
                                                                     )
                                                                WHEN " + (rb_LastBuyAmount.Checked ? "2" : "0") + @" = 2 THEN (
                                                                         SELECT 
                                                                                TOP 
                                                                                1 
                                                                                tcbf.column10
                                                                         FROM   
                                                                                Table_015_BuyFactor 
                                                                                tbf
                                                                                JOIN 
                                                                                     Table_016_Child1_BuyFactor 
                                                                                     tcbf
                                                                                     ON  
                                                                                         tcbf.column01 = 
                                                                                         tbf.columnid
                                                                         WHERE  
                                                                                tcbf.column02 = 
                                                                                tcai.columnid
                                                                                AND 
                                                                                    tbf.column02 
                                                                                    <= 
                                                                                    tsf.column02
                                                                         ORDER BY
                                                                                tbf.column02 
                                                                                DESC
                                                                     )
                                                                WHEN " + (rb_RTable.Checked ? "3" : "0") + @" = 3 THEN (
                                                                         SELECT 
                                                                                TOP 
                                                                                1(tgp.Column01)
                                                                         FROM   
                                                                                Table_70_GoodPrice 
                                                                                tgp
                                                                         WHERE  
                                                                                tgp.Column00 = 
                                                                                tcai.columnid
                                                                                AND 
                                                                                    tgp.Column07 
                                                                                    <= 
                                                                                    tsf.column02
                                                                         ORDER BY
                                                                                tgp.Column07 
                                                                                DESC,
                                                                                tgp.Column01 
                                                                                DESC
                                                                     )
                                                                ELSE 0
                                                           END
                                                       ),
                                                       0
                                                   ) AS BuyFee,
                                                   tsfs.Column03 AS 
                                                   DarsadHamkari,
                                                   (
                                                       SELECT tsi.Column02
                                                       FROM   " + ConBase.Database + @".dbo.Table_030_Setting 
                                                              tsi
                                                       WHERE  tsi.ColumnId = 41
                                                   ) AS ScoreCoefficient,
                                                   ISNULL(
                                                       (
                                                           SELECT TOP 1 ISNULL(tpsc.Column04, tcsf.column10)
                                                           FROM   
                                                                  Table_83_PriceStatementChild 
                                                                  tpsc
                                                                  JOIN 
                                                                       Table_82_PriceStatement 
                                                                       tps2
                                                                       ON  tps2.ColumnId = 
                                                                           tpsc.Column00
                                                           WHERE  tpsc.Column01 = 
                                                                  tcsf.column02
                                                                  AND tps2.Column04 = 
                                                                      tpi4.Column30--نوع فروش واسطه
                                                                  AND tps2.Column01 
                                                                      <= tsf.column02
                                                           ORDER BY
                                                                  tps2.Column01 
                                                                  DESC
                                                       ),
                                                       tcsf.column10
                                                   ) AS DeallerAmount,
                                                   ISNULL(tsfd.Column03, 0) AS 
                                                   DeallerPrecent
                                            FROM   Table_010_SaleFactor tsf
                                                   LEFT JOIN 
                                                        Table_012_SaleFactorDealler 
                                                        tsfd
                                                        ON  tsfd.Column01 = tsf.columnid
                                                   LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo 
                                                        tpi4
                                                        ON  tpi4.ColumnId = tsfd.Column02
                                                   JOIN 
                                                        Table_012_SaleFactorSeller 
                                                        tsfs
                                                        ON  tsfs.Column01 = tsf.columnid
                                                   JOIN 
                                                        Table_011_Child1_SaleFactor 
                                                        tcsf
                                                        ON  tcsf.column01 = tsf.columnid
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients 
                                                        tcai
                                                        ON  tcai.columnid = tcsf.column02
                                                   LEFT JOIN 
                                                        Table_76_AssignGoodType 
                                                        tagt
                                                        ON  tagt.Column00 = tcai.columnid
                                                   LEFT    JOIN 
                                                        Table_75_SaleType tst1
                                                        ON  tst1.ColumnId = tagt.Column01
                                            WHERE  tsf.column17 = 0
                                                   AND tsf.column19 = 0
                                                   AND tsf.column02 >=  '" + faDatePicker1.Text + @"'
                                                   AND tsf.column02 <=  '" + faDatePicker2.Text + @"'
                                                   AND tst1.ColumnId = saletypid
                                                       -- AND tsf.columnid != salefactorid
                                                   AND tsfs.Column02 = sellerid
                                        ) AS gg
                             ) AS ScoreSum
                      FROM   (
                                 SELECT tsfs.Column02 AS sellerid,
                                        tpi.Column01 AS PersonCode,
                                        tpi.Column02 PersonName,
                                        tpg.Column01 AS PersonGroup,
                                        tsf.column02 AS FactorDate,
                                        CAST(CAST(tsf.column14 AS TIME) AS NVARCHAR(5)) AS 
                                        FactorTime,
                                        tsf.column01 AS FactorNum,
                                        tcai.column01 AS GoodCode,
                                        tcai.column02 GoodName,
                                        tst.Column02 AS [SaleType],
                                        tcsf.column07 AS [GoodValue],
                                        tcsf.column10 AS [GoodAmount],
                                        tcsf.column11 AS [TotalAmount],
                                        tcsf.column20 AS NetPrice,
                                        tcsf.column17 AS DiscountAmount,
                                        ISNULL(
                                            (
                                                (tcsf.column17 * 100) / NULLIF(CAST(tcsf.column20 AS DECIMAL), 0)
                                            ),
                                            0
                                        ) AS DiscountP,
                                        ISNULL(
                                            (
                                                CASE 
                                                     WHEN " + (rb_Average.Checked ? "1" : "0") + @"=1 THEN (
                                                              SELECT ISNULL(tcpd.column15, 0)
                                                              FROM   
                                                                     " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft 
                                                                     tcpd
                                                              WHERE  tcpd.column02 = 
                                                                     tcai.columnid
                                                                     AND tcpd.column01 = 
                                                                         tsf.column09
                                                          )
                                                     WHEN " + (rb_LastBuyAmount.Checked ? "2" : "0") + @"=2 THEN (
                                                              SELECT TOP 1 tcbf.column10
                                                              FROM   
                                                                     Table_015_BuyFactor 
                                                                     tbf
                                                                     JOIN 
                                                                          Table_016_Child1_BuyFactor 
                                                                          tcbf
                                                                          ON  
                                                                              tcbf.column01 = 
                                                                              tbf.columnid
                                                              WHERE  tcbf.column02 = 
                                                                     tcai.columnid
                                                                     AND tbf.column02 
                                                                         <= tsf.column02
                                                              ORDER BY
                                                                     tbf.column02 
                                                                     DESC
                                                          )
                                                     WHEN " + (rb_RTable.Checked ? "3" : "0") + @"=3 THEN (
                                                              SELECT TOP 1(tgp.Column01)
                                                              FROM   
                                                                     Table_70_GoodPrice 
                                                                     tgp
                                                              WHERE  tgp.Column00 = 
                                                                     tcai.columnid
                                                                     AND tgp.Column07 
                                                                         <= tsf.column02
                                                              ORDER BY
                                                                     tgp.Column07 
                                                                     DESC,
                                                                     tgp.Column01 
                                                                     DESC
                                                          )
                                                     ELSE 0
                                                END
                                            ),
                                            0
                                        ) AS BuyFee,
                                        tsfs.Column03 AS DarsadHamkari,
                                        tsg.Column03 AS Goal,
                                        ISNULL(tcgr.Column01, 1) AS 
                                        CustomerGroupRatio,
                                        ISNULL(
                                            (
                                                SELECT TOP 1 tsme.Column23
                                                FROM   
                                                       Table_74_SaleManEvaluation 
                                                       tsme
                                                WHERE  tsme.Column01 = tsfs.Column02
                                                       AND tsme.Column00 <= tsf.column02
                                                ORDER BY
                                                       tsme.Column00 DESC
                                            ),
                                            1
                                        ) AS EvaluationRatio,
                                        (
                                            SELECT tsi.Column02
                                            FROM   " + ConBase.Database + @".dbo.Table_030_Setting 
                                                   tsi
                                            WHERE  tsi.ColumnId = 42
                                        ) AS RialProfitCoefficient,
                                        (
                                            SELECT tsi.Column02
                                            FROM   " + ConBase.Database + @".dbo.Table_030_Setting 
                                                   tsi
                                            WHERE  tsi.ColumnId = 41
                                        ) AS ScoreCoefficient,
                                        ISNULL(
                                            (
                                                SELECT TOP 1 ISNULL(tpsc.Column04, tcsf.column10)
                                                FROM   
                                                       Table_83_PriceStatementChild 
                                                       tpsc
                                                       JOIN 
                                                            Table_82_PriceStatement 
                                                            tps2
                                                            ON  tps2.ColumnId = 
                                                                tpsc.Column00
                                                WHERE  tpsc.Column01 = tcsf.column02
                                                       AND tps2.Column04 = tpi4.Column30--نوع فروش واسطه
                                                       AND tps2.Column01 <= tsf.column02
                                                ORDER BY
                                                       tps2.Column01 DESC
                                            ),
                                            tcsf.column10
                                        ) AS DeallerAmount,
                                        ISNULL(tsfd.Column03, 0) AS 
                                        DeallerPrecent,
                                        tst.ColumnId AS saletypid,
                                        tsf.columnid AS salefactorid,
                                        ISNULL(
                                            (
                                                SELECT tfst.Column01
                                                FROM   Table_79_FactorSaleType 
                                                       tfst
                                                WHERE  tfst.Column00 = tsf.Column36
                                            ),
                                            1
                                        ) AS SalefactorTypeRatio
                                 FROM   Table_010_SaleFactor tsf
                                        LEFT JOIN Table_012_SaleFactorDealler 
                                             tsfd
                                             ON  tsfd.Column01 = tsf.columnid
                                        LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo 
                                             tpi4
                                             ON  tpi4.ColumnId = tsfd.Column02
                                        JOIN Table_012_SaleFactorSeller tsfs
                                             ON  tsfs.Column01 = tsf.columnid
                                        JOIN Table_011_Child1_SaleFactor tcsf
                                             ON  tcsf.column01 = tsf.columnid
                                        JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients 
                                             tcai
                                             ON  tcai.columnid = tcsf.column02
                                        JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo 
                                             tpi
                                             ON  tpi.ColumnId = tsf.column03
                                        LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonScope 
                                             tps
                                             ON  tps.Column01 = tpi.ColumnId
                                        LEFT JOIN " + ConBase.Database + @".dbo.Table_040_PersonGroups 
                                             tpg
                                             ON  tpg.Column00 = tps.Column02
                                        LEFT JOIN Table_76_AssignGoodType tagt
                                             ON  tagt.Column00 = tcai.columnid
                                        LEFT    JOIN Table_75_SaleType tst
                                             ON  tst.ColumnId = tagt.Column01
                                        LEFT JOIN Table_77_SaleGoal tsg
                                             ON  tsg.Column00 = tsfs.Column02
                                             AND tsg.Column02 = tst.ColumnId
                                        LEFT JOIN Table_73_CustomerGroupRatio 
                                             tcgr
                                             ON  tcgr.Column00 = tpg.Column00
                                 WHERE  tsf.column17 = 0
                                        AND tsf.column19 = 0
                                        AND tsf.column02 >= '" + faDatePicker1.Text + @"'
                                        AND tsf.column02 <= '" + faDatePicker2.Text + @"'
                                        AND ( tsg.Column01=SUBSTRING('" + faDatePicker2.Text + @"',6,2) or tsg.Column01 is null )--اگر برای مسئول فروش هدف تعیین نکرده باشد
                                        AND tsfs.Column02 = " + mlt_SaleMan.Value + @"
                             ) AS tt
                  ) AS final
       ) total";
                DataTable dt = clDoc.ReturnTable(ConSale.ConnectionString, commad);
                gridEX_Goods.DataSource = dt;
            }
            else
            {
                MessageBox.Show("اطلاعات مورد نیاز را تکمیل کنید");

            }
        }

        private void Form10_CalcuteSellerProfit_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Goods.RemoveFilters();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void Form10_CalcuteSellerProfit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.E && e.Control)
                toolStripDropDownButton1_Click(sender, e);
            else if (e.KeyCode == Keys.P && e.Control)
                cmb_Print_Click(sender, e);
            else if (e.KeyCode == Keys.R && e.Control)
                btn_Calc_Click(sender, e);
        }


        private void mlt_SaleMan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
            else
                Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void rb_Average_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
            else
                Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void Form10_CalcuteSellerProfit_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.test' table. You can move, or remove it, as needed.
            //this.testTableAdapter.Fill(this.dataSet1.test);
            // TODO: This line of code loads data into the 'dataSet1.DataTable1' table. You can move, or remove it, as needed.
            mlt_SaleMan.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)");
            faDatePicker1.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePicker2.SelectedDateTime = DateTime.Now;
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;

            faDatePicker1.Select();

        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;

            Class_BasicOperation.isEnter(e.KeyChar);

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePicker2_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox =
                    (FarsiLibrary.Win.Controls.FADatePicker)sender;


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

        private void faDatePickerStrip1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip1.FADatePicker.HideDropDown();
                    bt_Save.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePickerStrip1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox = (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


                if (textBox.FADatePicker.Text.Length == 4)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
                else if (textBox.FADatePicker.Text.Length == 7)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime != null && faDatePicker1.SelectedDateTime != null && faDatePicker2.SelectedDateTime != null
                && mlt_SaleMan.Value != null && !string.IsNullOrWhiteSpace(mlt_SaleMan.Value.ToString())
                && (rb_Average.Checked || rb_LastBuyAmount.Checked || rb_RTable.Checked))
            {
                int g = 0;
                if (rb_Average.Checked)
                    g = 0;
                if (rb_LastBuyAmount.Checked)
                    g = 1;
                if (rb_RTable.Checked)
                    g = 2;
                string command = string.Empty;
                SqlParameter DocNum;
                DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                DocNum.Direction = ParameterDirection.Output;
                command = @"INSERT INTO  [dbo].[Table_86_SaleManProfitHeader]
                                       ([Column00]
                                       ,[Column01]
                                       ,[Column02]
                                       ,[Column03]
                                       ,[Column04]
                                       ,[Column05]
                                       ,[Column06])
                                 VALUES
                                       ('" + faDatePicker1.Text + @"'
                                       ,'" + faDatePicker2.Text + @"'
                                       ,'" + faDatePickerStrip1.FADatePicker.Text + @"'
                                       ," + mlt_SaleMan.Value + @"
                                       ," + g + @"
                                       ,'" + Class_BasicOperation._UserName + @"'
                                       ,getdate()); SET @DocNum=SCOPE_IDENTITY();";
                command += @" INSERT INTO  [dbo].[Table_87_SaleManProfitChild]
                                                   ([sellerid]
                                                   ,[PersonCode]
                                                   ,[PersonName]
                                                   ,[PersonGroup]
                                                   ,[FactorDate]
                                                   ,[FactorTime]
                                                   ,[FactorNum]
                                                   ,[GoodCode]
                                                   ,[GoodName]
                                                   ,[SaleType]
                                                   ,[GoodValue]
                                                   ,[GoodAmount]
                                                   ,[TotalAmount]
                                                   ,[NetPrice]
                                                   ,[DiscountAmount]
                                                   ,[DiscountP]
                                                   ,[BuyFee]
                                                   ,[DarsadHamkari]
                                                   ,[Goal]
                                                   ,[CustomerGroupRatio]
                                                   ,[EvaluationRatio]
                                                   ,[RialProfitCoefficient]
                                                   ,[ScoreCoefficient]
                                                   ,[DeallerAmount]
                                                   ,[DeallerPrecent]
                                                   ,[saletypid]
                                                   ,[salefactorid]
                                                   ,[SalefactorTypeRatio]
                                                   ,[BuyAmount]
                                                   ,[ProfitAmount]
                                                   ,[Score]
                                                   ,[DiscountRatio]
                                                   ,[HolidayRatio]
                                                   ,[TimeRatio]
                                                   ,[DeallerShare]
                                                   ,[ScoreSum]
                                                   ,[GoalRatio]
                                                   ,[TotalScore]
                                                   ,[SaleManProfitAmount]
                                                   ,[HeaderID])

SELECT total.*,
       total.TotalScore * total.RialProfitCoefficient AS SaleManProfitAmount,@DocNum
FROM   (
           SELECT final.*,
                  ISNULL(final.ScoreSum / NULLIF(CAST(final.Goal AS DECIMAL), 0), 1) AS 
                  GoalRatio,
                  (
                      (
                          (
                              CASE 
                                   WHEN final.HolidayRatio > 1 THEN final.HolidayRatio
                                   WHEN final.HolidayRatio = 1
                              AND final.TimeRatio > 1 THEN final.TimeRatio
                                  ELSE 1 END
                          ) + final.CustomerGroupRatio + final.SalefactorTypeRatio 
                          + final.EvaluationRatio
                          + ISNULL(final.ScoreSum / NULLIF(CAST(final.Goal AS DECIMAL), 0), 1)
                      ) * final.DiscountRatio * final.Score
                  ) AS TotalScore
           FROM   (
                      SELECT *,
                             (
                                 tt.BuyFee + ISNULL(
                                     (
                                         (
                                             (
                                                 CASE 
                                                      WHEN tt.DeallerAmount = 0 THEN 
                                                           tt.GoodAmount
                                                      ELSE tt.DeallerAmount
                                                 END
                                             )
                                             -tt.GoodAmount
                                         ) * tt.DeallerPrecent
                                     ) / CAST(100 AS DECIMAL),
                                     0
                                 )
                             ) * tt.GoodValue AS BuyAmount,
                             (
                                 (
                                     tt.BuyFee + ISNULL(
                                         (
                                             (
                                                 (
                                                     CASE 
                                                          WHEN tt.DeallerAmount 
                                                               = 0 THEN tt.GoodAmount
                                                          ELSE tt.DeallerAmount
                                                     END
                                                 )
                                                 -tt.GoodAmount
                                             ) * tt.DeallerPrecent
                                         ) / CAST(100 AS DECIMAL),
                                         0
                                     )
                                 ) * tt.GoodValue
                             ) -tt.NetPrice AS ProfitAmount,
                             (
                                 (
                                     (
                                         (
                                             tt.BuyFee + ISNULL(
                                                 (
                                                     (
                                                         (
                                                             CASE 
                                                                  WHEN tt.DeallerAmount 
                                                                       = 0 THEN 
                                                                       tt.GoodAmount
                                                                  ELSE tt.DeallerAmount
                                                             END
                                                         )
                                                         -tt.GoodAmount
                                                     ) * tt.DeallerPrecent
                                                 ) / CAST(100 AS DECIMAL),
                                                 0
                                             )
                                         ) * tt.GoodValue
                                     ) -tt.NetPrice
                                 ) / ISNULL(CAST(ScoreCoefficient AS DECIMAL), 0)
                             ) * tt.DarsadHamkari AS Score,
                             ISNULL(
                                 (
                                     SELECT Column02
                                     FROM   Table_78_DiscountRatio tdr
                                     WHERE  tdr.Column00 >= tt.DiscountP
                                            AND tdr.Column01 <= tt.DiscountP
                                 ),
                                 1
                             ) AS DiscountRatio,
                             ISNULL(
                                 (
                                     SELECT thr.Column01
                                     FROM   Table_71_HolidayRatio thr
                                     WHERE  thr.Column00 = tt.FactorDate
                                 ),
                                 1
                             ) AS HolidayRatio,
                             ISNULL(
                                 (
                                     SELECT ttr.Column02
                                     FROM   Table_72_TimeRatio ttr
                                     WHERE  CAST(ttr.Column00 AS TIME) >= CAST(tt.FactorTime AS TIME)
                                            AND CAST(ttr.Column01 AS TIME) <= 
                                                CAST(tt.FactorTime AS TIME)
                                 ),
                                 1
                             ) AS TimeRatio,
                             ISNULL(
                                 (
                                     (
                                         (
                                             CASE 
                                                  WHEN tt.DeallerAmount = 0 THEN 
                                                       tt.GoodAmount
                                                  ELSE tt.DeallerAmount
                                             END
                                         )
                                         -tt.GoodAmount
                                     ) * tt.DeallerPrecent
                                 ) / CAST(100 AS DECIMAL),
                                 0
                             ) AS DeallerShare,
                             (
                                 SELECT ISNULL(
                                            SUM(
                                                ISNULL(
                                                    (
                                                        (
                                                            (
                                                                (
                                                                    (
                                                                        gg.BuyFee 
                                                                        + ISNULL(
                                                                            (
                                                                                (
                                                                                    (
                                                                                        CASE 
                                                                                             WHEN 
                                                                                                  gg.DeallerAmount 
                                                                                                  =
                                                                                                  0 THEN 
                                                                                                  gg.GoodAmount
                                                                                             ELSE 
                                                                                                  gg.DeallerAmount
                                                                                        END
                                                                                    )
                                                                                    -
                                                                                    gg.GoodAmount
                                                                                ) 
                                                                                *
                                                                                gg.DeallerPrecent
                                                                            ) /
                                                                            CAST(100 AS DECIMAL),
                                                                            0
                                                                        )
                                                                    ) * gg.GoodValue
                                                                ) -gg.NetPrice
                                                            ) / ISNULL(CAST(ScoreCoefficient AS DECIMAL), 0)
                                                        ) * gg.DarsadHamkari
                                                    ),
                                                    0
                                                )
                                            ),
                                            0
                                        ) AS Score
                                 FROM   (
                                            SELECT tcsf.column07 AS [GoodValue],
                                                   tcsf.column10 AS [GoodAmount],
                                                   tcsf.column20 AS NetPrice,
                                                   ISNULL(
                                                       (
                                                           CASE 
                                                                WHEN " + (rb_Average.Checked ? "1" : "0") + @" = 1 THEN (
                                                                         SELECT 
                                                                                ISNULL(tcpd.column15, 0)
                                                                         FROM   
                                                                                " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft 
                                                                                tcpd
                                                                         WHERE  
                                                                                tcpd.column02 = 
                                                                                tcai.columnid
                                                                                AND 
                                                                                    tcpd.column01 = 
                                                                                    tsf.column09
                                                                     )
                                                                WHEN " + (rb_LastBuyAmount.Checked ? "2" : "0") + @" = 2 THEN (
                                                                         SELECT 
                                                                                TOP 
                                                                                1 
                                                                                tcbf.column10
                                                                         FROM   
                                                                                Table_015_BuyFactor 
                                                                                tbf
                                                                                JOIN 
                                                                                     Table_016_Child1_BuyFactor 
                                                                                     tcbf
                                                                                     ON  
                                                                                         tcbf.column01 = 
                                                                                         tbf.columnid
                                                                         WHERE  
                                                                                tcbf.column02 = 
                                                                                tcai.columnid
                                                                                AND 
                                                                                    tbf.column02 
                                                                                    <= 
                                                                                    tsf.column02
                                                                         ORDER BY
                                                                                tbf.column02 
                                                                                DESC
                                                                     )
                                                                WHEN " + (rb_RTable.Checked ? "3" : "0") + @" = 3 THEN (
                                                                         SELECT 
                                                                                TOP 
                                                                                1(tgp.Column01)
                                                                         FROM   
                                                                                Table_70_GoodPrice 
                                                                                tgp
                                                                         WHERE  
                                                                                tgp.Column00 = 
                                                                                tcai.columnid
                                                                                AND 
                                                                                    tgp.Column07 
                                                                                    <= 
                                                                                    tsf.column02
                                                                         ORDER BY
                                                                                tgp.Column07 
                                                                                DESC,
                                                                                tgp.Column01 
                                                                                DESC
                                                                     )
                                                                ELSE 0
                                                           END
                                                       ),
                                                       0
                                                   ) AS BuyFee,
                                                   tsfs.Column03 AS 
                                                   DarsadHamkari,
                                                   (
                                                       SELECT tsi.Column02
                                                       FROM   " + ConBase.Database + @".dbo.Table_030_Setting 
                                                              tsi
                                                       WHERE  tsi.ColumnId = 41
                                                   ) AS ScoreCoefficient,
                                                   ISNULL(
                                                       (
                                                           SELECT TOP 1 ISNULL(tpsc.Column04, tcsf.column10)
                                                           FROM   
                                                                  Table_83_PriceStatementChild 
                                                                  tpsc
                                                                  JOIN 
                                                                       Table_82_PriceStatement 
                                                                       tps2
                                                                       ON  tps2.ColumnId = 
                                                                           tpsc.Column00
                                                           WHERE  tpsc.Column01 = 
                                                                  tcsf.column02
                                                                  AND tps2.Column04 = 
                                                                      tpi4.Column30--نوع فروش واسطه
                                                                  AND tps2.Column01 
                                                                      <= tsf.column02
                                                           ORDER BY
                                                                  tps2.Column01 
                                                                  DESC
                                                       ),
                                                       tcsf.column10
                                                   ) AS DeallerAmount,
                                                   ISNULL(tsfd.Column03, 0) AS 
                                                   DeallerPrecent
                                            FROM   Table_010_SaleFactor tsf
                                                   LEFT JOIN 
                                                        Table_012_SaleFactorDealler 
                                                        tsfd
                                                        ON  tsfd.Column01 = tsf.columnid
                                                   LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo 
                                                        tpi4
                                                        ON  tpi4.ColumnId = tsfd.Column02
                                                   JOIN 
                                                        Table_012_SaleFactorSeller 
                                                        tsfs
                                                        ON  tsfs.Column01 = tsf.columnid
                                                   JOIN 
                                                        Table_011_Child1_SaleFactor 
                                                        tcsf
                                                        ON  tcsf.column01 = tsf.columnid
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients 
                                                        tcai
                                                        ON  tcai.columnid = tcsf.column02
                                                   LEFT JOIN 
                                                        Table_76_AssignGoodType 
                                                        tagt
                                                        ON  tagt.Column00 = tcai.columnid
                                                   LEFT    JOIN 
                                                        Table_75_SaleType tst1
                                                        ON  tst1.ColumnId = tagt.Column01
                                            WHERE  tsf.column17 = 0
                                                   AND tsf.column19 = 0
                                                   AND tsf.column02 >=  '" + faDatePicker1.Text + @"'
                                                   AND tsf.column02 <=  '" + faDatePicker2.Text + @"'
                                                   AND tst1.ColumnId = saletypid
                                                       -- AND tsf.columnid != salefactorid
                                                   AND tsfs.Column02 = sellerid
                                        ) AS gg
                             ) AS ScoreSum
                      FROM   (
                                 SELECT tsfs.Column02 AS sellerid,
                                        tpi.Column01 AS PersonCode,
                                        tpi.Column02 PersonName,
                                        tpg.Column01 AS PersonGroup,
                                        tsf.column02 AS FactorDate,
                                        CAST(CAST(tsf.column14 AS TIME) AS NVARCHAR(5)) AS 
                                        FactorTime,
                                        tsf.column01 AS FactorNum,
                                        tcai.column01 AS GoodCode,
                                        tcai.column02 GoodName,
                                        tst.Column02 AS [SaleType],
                                        tcsf.column07 AS [GoodValue],
                                        tcsf.column10 AS [GoodAmount],
                                        tcsf.column11 AS [TotalAmount],
                                        tcsf.column20 AS NetPrice,
                                        tcsf.column17 AS DiscountAmount,
                                        ISNULL(
                                            (
                                                (tcsf.column17 * 100) / NULLIF(CAST(tcsf.column20 AS DECIMAL), 0)
                                            ),
                                            0
                                        ) AS DiscountP,
                                        ISNULL(
                                            (
                                                CASE 
                                                     WHEN " + (rb_Average.Checked ? "1" : "0") + @"=1 THEN (
                                                              SELECT ISNULL(tcpd.column15, 0)
                                                              FROM   
                                                                     " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft 
                                                                     tcpd
                                                              WHERE  tcpd.column02 = 
                                                                     tcai.columnid
                                                                     AND tcpd.column01 = 
                                                                         tsf.column09
                                                          )
                                                     WHEN " + (rb_LastBuyAmount.Checked ? "2" : "0") + @"=2 THEN (
                                                              SELECT TOP 1 tcbf.column10
                                                              FROM   
                                                                     Table_015_BuyFactor 
                                                                     tbf
                                                                     JOIN 
                                                                          Table_016_Child1_BuyFactor 
                                                                          tcbf
                                                                          ON  
                                                                              tcbf.column01 = 
                                                                              tbf.columnid
                                                              WHERE  tcbf.column02 = 
                                                                     tcai.columnid
                                                                     AND tbf.column02 
                                                                         <= tsf.column02
                                                              ORDER BY
                                                                     tbf.column02 
                                                                     DESC
                                                          )
                                                     WHEN " + (rb_RTable.Checked ? "3" : "0") + @"=3 THEN (
                                                              SELECT TOP 1(tgp.Column01)
                                                              FROM   
                                                                     Table_70_GoodPrice 
                                                                     tgp
                                                              WHERE  tgp.Column00 = 
                                                                     tcai.columnid
                                                                     AND tgp.Column07 
                                                                         <= tsf.column02
                                                              ORDER BY
                                                                     tgp.Column07 
                                                                     DESC,
                                                                     tgp.Column01 
                                                                     DESC
                                                          )
                                                     ELSE 0
                                                END
                                            ),
                                            0
                                        ) AS BuyFee,
                                        tsfs.Column03 AS DarsadHamkari,
                                        tsg.Column03 AS Goal,
                                        ISNULL(tcgr.Column01, 1) AS 
                                        CustomerGroupRatio,
                                        ISNULL(
                                            (
                                                SELECT TOP 1 tsme.Column23
                                                FROM   
                                                       Table_74_SaleManEvaluation 
                                                       tsme
                                                WHERE  tsme.Column01 = tsfs.Column02
                                                       AND tsme.Column00 <= tsf.column02
                                                ORDER BY
                                                       tsme.Column00 DESC
                                            ),
                                            1
                                        ) AS EvaluationRatio,
                                        (
                                            SELECT tsi.Column02
                                            FROM   " + ConBase.Database + @".dbo.Table_030_Setting 
                                                   tsi
                                            WHERE  tsi.ColumnId = 42
                                        ) AS RialProfitCoefficient,
                                        (
                                            SELECT tsi.Column02
                                            FROM   " + ConBase.Database + @".dbo.Table_030_Setting 
                                                   tsi
                                            WHERE  tsi.ColumnId = 41
                                        ) AS ScoreCoefficient,
                                        ISNULL(
                                            (
                                                SELECT TOP 1 ISNULL(tpsc.Column04, tcsf.column10)
                                                FROM   
                                                       Table_83_PriceStatementChild 
                                                       tpsc
                                                       JOIN 
                                                            Table_82_PriceStatement 
                                                            tps2
                                                            ON  tps2.ColumnId = 
                                                                tpsc.Column00
                                                WHERE  tpsc.Column01 = tcsf.column02
                                                       AND tps2.Column04 = tpi4.Column30--نوع فروش واسطه
                                                       AND tps2.Column01 <= tsf.column02
                                                ORDER BY
                                                       tps2.Column01 DESC
                                            ),
                                            tcsf.column10
                                        ) AS DeallerAmount,
                                        ISNULL(tsfd.Column03, 0) AS 
                                        DeallerPrecent,
                                        tst.ColumnId AS saletypid,
                                        tsf.columnid AS salefactorid,
                                        ISNULL(
                                            (
                                                SELECT tfst.Column01
                                                FROM   Table_79_FactorSaleType 
                                                       tfst
                                                WHERE  tfst.Column00 = tsf.Column36
                                            ),
                                            1
                                        ) AS SalefactorTypeRatio
                                 FROM   Table_010_SaleFactor tsf
                                        LEFT JOIN Table_012_SaleFactorDealler 
                                             tsfd
                                             ON  tsfd.Column01 = tsf.columnid
                                        LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo 
                                             tpi4
                                             ON  tpi4.ColumnId = tsfd.Column02
                                        JOIN Table_012_SaleFactorSeller tsfs
                                             ON  tsfs.Column01 = tsf.columnid
                                        JOIN Table_011_Child1_SaleFactor tcsf
                                             ON  tcsf.column01 = tsf.columnid
                                        JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients 
                                             tcai
                                             ON  tcai.columnid = tcsf.column02
                                        JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo 
                                             tpi
                                             ON  tpi.ColumnId = tsf.column03
                                        LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonScope 
                                             tps
                                             ON  tps.Column01 = tpi.ColumnId
                                        LEFT JOIN " + ConBase.Database + @".dbo.Table_040_PersonGroups 
                                             tpg
                                             ON  tpg.Column00 = tps.Column02
                                        LEFT JOIN Table_76_AssignGoodType tagt
                                             ON  tagt.Column00 = tcai.columnid
                                        LEFT    JOIN Table_75_SaleType tst
                                             ON  tst.ColumnId = tagt.Column01
                                        LEFT JOIN Table_77_SaleGoal tsg
                                             ON  tsg.Column00 = tsfs.Column02
                                             AND tsg.Column02 = tst.ColumnId
                                        LEFT JOIN Table_73_CustomerGroupRatio 
                                             tcgr
                                             ON  tcgr.Column00 = tpg.Column00
                                 WHERE  tsf.column17 = 0
                                        AND tsf.column19 = 0
                                        AND tsf.column02 >= '" + faDatePicker1.Text + @"'
                                        AND tsf.column02 <= '" + faDatePicker2.Text + @"'
                                        AND ( tsg.Column01=SUBSTRING('" + faDatePicker2.Text + @"',6,2) or tsg.Column01 is null )--اگر برای مسئول فروش هدف تعیین نکرده باشد
                                        AND tsfs.Column02 = " + mlt_SaleMan.Value + @"
                             ) AS tt
                  ) AS final
       ) total

";
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                {
                    Con.Open();

                    SqlTransaction sqlTran = Con.BeginTransaction();
                    SqlCommand Command = Con.CreateCommand();
                    Command.Transaction = sqlTran;

                    try
                    {
                        Command.CommandText = command;
                        Command.Parameters.Add(DocNum);
                        Command.ExecuteNonQuery();
                        sqlTran.Commit();
                        Class_BasicOperation.ShowMsg("", "گزارش پورسانت با شماره " + DocNum.Value + " با موفقیت ثبت گردید", "Information");
                        this.DialogResult = DialogResult.Yes;
                    }
                    catch (Exception es)
                    {
                        sqlTran.Rollback();
                        this.Cursor = Cursors.Default;
                        Class_BasicOperation.CheckExceptionType(es, this.Name);
                    }

                    this.Cursor = Cursors.Default;



                }


            }
        }

        private void mlt_SaleMan_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column01", "Column02");
        }

        private void mlt_SaleMan_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }
    }
}
