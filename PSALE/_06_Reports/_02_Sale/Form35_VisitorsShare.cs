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
    public partial class Form35_VisitorsShare : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1, Date2;
        public Form35_VisitorsShare()
        {
            InitializeComponent();
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;



                string CommandText = null;
                CommandText = @"
                                            SELECT --Num,
                                                   Visitor,
                                                   MainGroup,
                                                   SubGroup,
                                                   GoodCode,
                                                   GoodName,
                                                   SUM(ISNULL(dd.Count, 0)) AS [Count],
                                                   SUM(ISNULL(Amount, 0)) AS [Amount],
                                                   SUM(ISNULL(NetAmount, 0)) AS [NetAmount],
                                                   SUM(ISNULL(MCount, 0)) AS MCount,
                                                   SUM(ISNULL(MAmount, 0)) AS MAmount,
                                                   SUM(ISNULL(MNetAmount, 0)) AS MNetAmount,
                                                   SUM(ISNULL(dd.Count, 0)) -SUM(ISNULL(MCount, 0)) AS FCount,
                                                   SUM(ISNULL(Amount, 0)) -SUM(ISNULL(MAmount, 0)) AS FAmount,
                                                   SUM(ISNULL(NetAmount, 0)) -SUM(ISNULL(MNetAmount, 0)) AS FNetAmount,
                                                   SharePrc,
                                                   (SUM(ISNULL(NetAmount, 0)) -SUM(ISNULL(MNetAmount, 0))) * ISNULL(SharePrc, 0)
                                                   / 100 AS NetShareValue,
                                                   (SUM(ISNULL(Amount, 0)) -SUM(ISNULL(MAmount, 0))) * ISNULL(SharePrc, 0) / 
                                                   100 AS ShareValue
                                            FROM   (
                                                       SELECT dbo.Table_010_SaleFactor.column01 AS Num,
                                                              dbo.Table_010_SaleFactor.column05 AS Visitor,
                                                              tmg.column02 AS MainGroup,
                                                              tsg.column03 AS SubGroup,
                                                              tcai.column01 AS GoodCode,
                                                              tcai.column02 AS GoodName,
                                                              (tcsf.column07) AS [Count],
                                                              (tcsf.column11) AS [Amount],
                                                              (tcsf.column20) AS [NetAmount],
                                                              0.000 AS MCount,
                                                              0.000 AS MAmount,
                                                              0.000 AS MNetAmount,
                                                              ISNULL(dbo.Table_010_SaleFactor.Column55, 0) AS SharePrc,tcsf.columnid
                                                       FROM   dbo.Table_010_SaleFactor
                                                              JOIN Table_011_Child1_SaleFactor tcsf
                                                                   ON  tcsf.column01 = dbo.Table_010_SaleFactor.columnid
                                                              JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                   ON  tcai.columnid = tcsf.column02
                                                              LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                                   ON  tmg.columnid = tcai.column03
                                                              LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                                   ON  tsg.column01 = tcai.column03
                                                                   AND tsg.columnid = tcai.column04
                                                        WHERE  (dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND '{1}') AND Table_010_SaleFactor.column17!=1
                                                       UNION  all
                                                       SELECT tms.column17 AS Num,
                                                              tms.column05 AS Visitor,
                                                              tmg.column02 AS MainGroup,
                                                              tsg.column03 AS SubGroup,
                                                              tcai.column01 AS GoodCode,
                                                              tcai.column02 AS GoodName,
                                                              0.000 AS [Count],
                                                              0.000 AS [Amount],
                                                              0.000 AS [NetAmount],
                                                              (tcms.column07) AS [MCount],
                                                              (tcms.column11) AS [MAmount],
                                                              (tcms.column20) AS [MNetAmount],
                                                              ISNULL(tms.Column25, 0) AS SharePrc,tcms.columnid
                                                       FROM   Table_018_MarjooiSale tms
                                                              JOIN Table_019_Child1_MarjooiSale tcms
                                                                   ON  tcms.column01 = tms.columnid
                                                              JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                   ON  tcai.columnid = tcms.column02
                                                              LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                                   ON  tmg.columnid = tcai.column03
                                                              LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                                   ON  tsg.column01 = tcai.column03
                                                                   AND tsg.columnid = tcai.column04
                                                                       WHERE  (tms.column02 BETWEEN '{0}' AND '{1}')
                                                   ) AS dd
                                            GROUP BY
                                                   -- Num,
                                                   Visitor,
                                                   SharePrc,
                                                   dd.MainGroup,
                                                   SubGroup,
                                                   GoodCode,
                                                   dd.GoodName
                                                    ";

                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);




                CommandText = @"
                                            SELECT --Num,
                                                   Visitor,
                                                   MainGroup,
                                                   SubGroup,
                                                   SUM(ISNULL(dd.Count, 0)) AS [Count],
                                                   SUM(ISNULL(Amount, 0)) AS [Amount],
                                                   SUM(ISNULL(NetAmount, 0)) AS [NetAmount],
                                                   SUM(ISNULL(MCount, 0)) AS MCount,
                                                   SUM(ISNULL(MAmount, 0)) AS MAmount,
                                                   SUM(ISNULL(MNetAmount, 0)) AS MNetAmount,
                                                   SUM(ISNULL(dd.Count, 0)) -SUM(ISNULL(MCount, 0)) AS FCount,
                                                   SUM(ISNULL(Amount, 0)) -SUM(ISNULL(MAmount, 0)) AS FAmount,
                                                   SUM(ISNULL(NetAmount, 0)) -SUM(ISNULL(MNetAmount, 0)) AS FNetAmount,
                                                   SharePrc,
                                                   (SUM(ISNULL(NetAmount, 0)) -SUM(ISNULL(MNetAmount, 0))) * ISNULL(SharePrc, 0)
                                                   / 100 AS NetShareValue,
                                                   (SUM(ISNULL(Amount, 0)) -SUM(ISNULL(MAmount, 0))) * ISNULL(SharePrc, 0) / 
                                                   100 AS ShareValue
                                            FROM   (
                                                       SELECT dbo.Table_010_SaleFactor.column01 AS Num,
                                                              dbo.Table_010_SaleFactor.column05 AS Visitor,
                                                              tmg.column02 AS MainGroup,
                                                              tsg.column03 AS SubGroup,
                                                              tcai.column01 AS GoodCode,
                                                              tcai.column02 AS GoodName,
                                                              (tcsf.column07) AS [Count],
                                                              (tcsf.column11) AS [Amount],
                                                              (tcsf.column20) AS [NetAmount],
                                                              0.000 AS MCount,
                                                              0.000 AS MAmount,
                                                              0.000 AS MNetAmount,
                                                              ISNULL(dbo.Table_010_SaleFactor.Column55, 0) AS SharePrc,
                                                              tcsf.columnid
                                                       FROM   dbo.Table_010_SaleFactor
                                                              JOIN Table_011_Child1_SaleFactor tcsf
                                                                   ON  tcsf.column01 = dbo.Table_010_SaleFactor.columnid
                                                              JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                   ON  tcai.columnid = tcsf.column02
                                                              LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                                   ON  tmg.columnid = tcai.column03
                                                              LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                                   ON  tsg.column01 = tcai.column03
                                                                   AND tsg.columnid = tcai.column04
                                                        WHERE  (dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND '{1}') AND Table_010_SaleFactor.column17!=1
                                                       UNION  all
                                                       SELECT tms.column17 AS Num,
                                                              tms.column05 AS Visitor,
                                                              tmg.column02 AS MainGroup,
                                                              tsg.column03 AS SubGroup,
                                                              tcai.column01 AS GoodCode,
                                                              tcai.column02 AS GoodName,
                                                              0.000 AS [Count],
                                                              0.000 AS [Amount],
                                                              0.000 AS [NetAmount],
                                                              (tcms.column07) AS [MCount],
                                                              (tcms.column11) AS [MAmount],
                                                              (tcms.column20) AS [MNetAmount],
                                                              ISNULL(tms.Column25, 0) AS SharePrc,
                                                             tcms.columnid
                                                       FROM   Table_018_MarjooiSale tms
                                                              JOIN Table_019_Child1_MarjooiSale tcms
                                                                   ON  tcms.column01 = tms.columnid
                                                              JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                   ON  tcai.columnid = tcms.column02
                                                              LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                                   ON  tmg.columnid = tcai.column03
                                                              LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                                   ON  tsg.column01 = tcai.column03
                                                                   AND tsg.columnid = tcai.column04
                                                                       WHERE  (tms.column02 BETWEEN '{0}' AND '{1}')
                                                   ) AS dd
                                            GROUP BY
                                                   -- Num,
                                                   Visitor,
                                                   SharePrc,
                                                   dd.MainGroup,
                                                   SubGroup
                                                  
                                                    ";

                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database);
                Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable GroupTable = new DataTable();
                Adapter.Fill(GroupTable);
              
                string k = bindingSource2.Filter;
                bindingSource2.RemoveFilter();
                bindingSource2.DataSource = GroupTable;
                try
                {
                    bindingSource2.Filter = k;
                }

                catch
                {
                }





                CommandText = @"SELECT d.ColumnId,
                                           tpi.Column01,
                                           tpi.Column02,
                                           SUM(d.Total) AS Total,
                                           SUM(d.[Return]) AS [Return],
                                           SUM(d.Total) -SUM(d.[Return]) AS Net
                                    FROM   (
                                               SELECT *
                                               FROM   (
                                                          SELECT ColumnId,
                                                                 0.000 AS Total,
                                                                 0.000 AS [Return]
                                                          FROM   dbo.PeopleScope(8, 3) AS PeopleScope_1
                                                      ) AS person
                                               UNION all (
                                                   SELECT tsf.column05 AS ColumnId,
                                                          sum(tcsf.column20) AS Total,
                                                          0.000 AS [Return]
                                                   FROM   " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor tcsf
                                                    join " + ConSale.Database + @".dbo.Table_010_SaleFactor tsf  ON  tcsf.column01 = tsf.columnid
                                                   WHERE  tsf.column17 = 0 group by tsf.column05
                                               ) UNION  all (
                                                   SELECT tms.column05 AS ColumnId,
                                                          0.000 AS Total,
                                                          Sum(tcms.column20) AS [Return]
                                                   FROM  " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale tcms join
                                                        " + ConSale.Database + @".dbo.Table_018_MarjooiSale tms
                                                                 ON  tcms.column01 = tms.columnid group by tms.column05
                                               )
                                           ) AS d
                                           LEFT JOIN Table_045_PersonInfo tpi
                                                ON  d.ColumnId = tpi.ColumnId
                                    GROUP BY
                                           d.ColumnId,
                                           tpi.Column01,
                                           tpi.Column02";

                Adapter = new SqlDataAdapter(CommandText, ConBase);
                DataTable Person = new DataTable();
                Adapter.Fill(Person);
                gridEX_visitors.DataSource = Person;
                k = bindingSource1.Filter;
                bindingSource1.RemoveFilter();
                bindingSource1.DataSource = Table;
                try
                {
                    bindingSource1.Filter = k;
                }
                catch
                {
                }
                tbl_VitorsBindingSource_PositionChanged(sender, e);
            }
        }

        private void Form35_VisitorsShare_Load(object sender, EventArgs e)
        {
            string[] Dates = Properties.Settings.Default.GoodReportByVisitors.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
            this.tbl_VitorsTableAdapter.Fill(this.dataSet_Report.Tbl_Vitors);
            bt_Search_Click(null, null);
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

        private void faDatePickerStrip1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip1.FADatePicker.HideDropDown();
                    faDatePickerStrip2.FADatePicker.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePickerStrip2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip2.FADatePicker.HideDropDown();
                    bt_Search_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void Form19_Visitors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click_1(sender, e);
        }

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }



        private void Form19_Visitors_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.GoodReportByVisitors = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_Goods.RootTable.Groups.Clear();
            gridEX_Goods.RemoveFilters();
            gridEX_visitors.RemoveFilters();
            gridEXGroup.RemoveFilters();
        }

        private void tbl_VitorsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX_visitors.GetValue("ColumnId") != null && !string.IsNullOrWhiteSpace(gridEX_visitors.GetValue("ColumnId").ToString()))
                {
                    bindingSource1.Filter = "Visitor=" + gridEX_visitors.GetValue("ColumnId").ToString();
                    bindingSource2.Filter = "Visitor=" + gridEX_visitors.GetValue("ColumnId").ToString();
                }
            }
            catch
            {
            }
        }

        private void bt_Print_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (uiPanel2.IsActive)
                    gridEXPrintDocument1.GridEX = gridEXGroup;

                else
                    gridEXPrintDocument1.GridEX = gridEX_Goods;

                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string j = " از تاریخ:" + faDatePickerStrip1.FADatePicker.Text + " تا تاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageHeaderLeft = j;
                        gridEXPrintDocument1.PageHeaderRight = " گزارش درصد همکاری مسئول فروش" + gridEX_visitors.GetRow().Cells["Column02"].Text;
                        printPreviewDialog1.ShowDialog();
                    }
            }
            catch { }
        }

        private void print_right_Click(object sender, EventArgs e)
        {
            try
            {
                gridEXPrintDocument1.GridEX = gridEX_visitors;
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string j = " از تاریخ:" + faDatePickerStrip1.FADatePicker.Text + " تا تاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageHeaderLeft = j;
                        gridEXPrintDocument1.PageHeaderRight = " گزارش درصد همکاری مسئول فروش" + gridEX_visitors.GetRow().Cells["Column02"].Text;
                        printPreviewDialog1.ShowDialog();
                    }
            }
            catch { }
        }
    }
}
