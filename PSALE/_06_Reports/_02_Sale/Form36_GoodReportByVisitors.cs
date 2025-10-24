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
    public partial class Form36_GoodReportByVisitors : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1, Date2;
        public Form36_GoodReportByVisitors()
        {
            InitializeComponent();
        }

        private void Form36_GoodReportByVisitors_Load(object sender, EventArgs e)
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
                bt_Print_Click(sender, e);
        }

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            if (gridEX_Goods.Focused)
                gridEXExporter1.GridEX = gridEX_Goods;
            if (gridEX1.Focused)
                gridEXExporter1.GridEX = gridEX1;
            if (gridEX2.Focused)
                gridEXExporter1.GridEX = gridEX2;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                string CommandText = null;
                CommandText = @"SELECT Visitor,
                                               MainGroup,
                                               SubGroup,
                                               GoodCode,
                                               GoodName,
                                               Unit,
                                               SUM(COUNT) AS [Count],
                                               SUM(Amount) AS [Amount],
                                               SUM(NetAmount) AS [NetAmount],
                                               ISNULL(
                                                   SUM(NetAmount) / NULLIF((ISNULL(SUM(ISNULL([Count], 0)), 0)), 0),
                                                   0
                                               ) AS Avrage,
                                               SUM(MCount) AS [MCount],
                                               SUM(MAmount) AS [MAmount],
                                               SUM(MNetAmount) AS [MNetAmount],
                                               ISNULL(
                                                   SUM(MNetAmount) / NULLIF((ISNULL(SUM(ISNULL([MCount], 0)), 0)), 0),
                                                   0
                                               ) AS MAvrage,
                                               SUM(ISNULL(COUNT, 0)) - SUM(ISNULL(MCOUNT, 0)) AS Tcount,
                                               SUM(ISNULL(Amount, 0)) - SUM(ISNULL(MAmount, 0)) AS TAmount,
                                               SUM(ISNULL(NetAmount, 0)) - SUM(ISNULL(MNetAmount, 0)) AS TNetAmount,
                                               (
                                                   (
                                                       ISNULL(
                                                           SUM(NetAmount) / NULLIF((ISNULL(SUM(ISNULL([Count], 0)), 0)), 0),
                                                           0
                                                       )
                                                   ) -(
                                                       ISNULL(
                                                           SUM(MNetAmount) / NULLIF((ISNULL(SUM(ISNULL([MCount], 0)), 0)), 0),
                                                           0
                                                       )
                                                   )
                                               ) AS TAvrage
                                        FROM   (
                                                   SELECT dbo.Table_010_SaleFactor.column05 AS Visitor,
                                                          tmg.column02 AS MainGroup,
                                                          tsg.column03 AS SubGroup,
                                                          tcai.column01 AS GoodCode,
                                                          tcai.column02 AS GoodName,
                                                          gg.Column01 AS Unit,
                                                          (tcsf.column07) AS [Count],
                                                          (tcsf.column11) AS [Amount],
                                                          (tcsf.column20) AS [NetAmount],
                                                          0.000 AS [MCount],
                                                          0.000 AS [MAmount],
                                                          0.000 AS [MNetAmount] ,tcsf.columnid
                 
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
                                                          LEFT JOIN " + ConBase.Database + @".dbo.Table_070_CountUnitInfo gg
                                                               ON  gg.Column00 = tcsf.column03
                                                   WHERE  (
                                                              dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND 
                                                              '{1}'
                                                          )
                                                          AND dbo.Table_010_SaleFactor.column17 = 0
           
           
           
           
                                                   UNION  all
           
           
           
                                                   SELECT dbo.Table_018_MarjooiSale.column05 AS Visitor,
                                                          tmg.column02 AS MainGroup,
                                                          tsg.column03 AS SubGroup,
                                                          tcai.column01 AS GoodCode,
                                                          tcai.column02 AS GoodName,
                                                          gg.Column01 AS Unit,
                                                          0.000 AS [Count],
                                                          0.000 AS [Amount],
                                                          0.000 AS [NetAmount],
                
                                                          tcsf.column07 AS [MCount],
                                                          tcsf.column11 AS [MAmount],
                                                          tcsf.column20 AS [MNetAmount],tcsf.columnid
                                                   FROM   dbo.Table_018_MarjooiSale
                                                          JOIN Table_019_Child1_MarjooiSale tcsf
                                                               ON  tcsf.column01 = dbo.Table_018_MarjooiSale.columnid
                                                          JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                               ON  tcai.columnid = tcsf.column02
                                                          LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                               ON  tmg.columnid = tcai.column03
                                                          LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                               ON  tsg.column01 = tcai.column03
                                                               AND tsg.columnid = tcai.column04
                                                          LEFT JOIN " + ConBase.Database + @".dbo.Table_070_CountUnitInfo gg
                                                               ON  gg.Column00 = tcsf.column03
                                                   WHERE  (
                                                              dbo.Table_018_MarjooiSale.column02 BETWEEN '{0}' 
                                                              AND '{1}'
                                                          )
                                               ) AS dd
                                        GROUP BY
                                               Visitor,
                                               MainGroup,
                                               SubGroup,
                                               GoodCode,
                                               GoodName,
                                               Unit ";

                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                bindingSource1.DataSource = Table;

                string command = string.Empty;
                command = @"
                                        SELECT --[Type],
                                                Visitor,
                                               MainGroup,
                                               SubGroup,
                                               GoodCode,
                                               GoodName,
                                               Unit,
                                               SUM(COUNT) AS [Count],
                                               Vahed,
                                               SUM(Amount) AS [Amount],
                                               SUM(dd.Takhfif) AS Takhfif,
                                               SUM(dd.Ezafe) AS Ezafe,
                                               SUM(NetAmount) AS [NetAmount]
                                        FROM   (
                                                   SELECT --N'فاکتور فروش'  as [Type],
                                                          dbo.Table_010_SaleFactor.column05 AS Visitor,
                                                          tmg.column02 AS MainGroup,
                                                          tsg.column03 AS SubGroup,
                                                          tcai.column01 AS GoodCode,
                                                          tcai.column02 AS GoodName,
                                                          gg.Column01 AS Unit,
                                                          (tcsf.column07) AS [Count],
                                                          tcsf.column10 AS Vahed,
                                                          tcsf.column17 AS Takhfif,
                                                          tcsf.column19 AS Ezafe,
                                                          (tcsf.column11) AS [Amount],
                                                          (tcsf.column20) AS [NetAmount],
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
                                                          LEFT JOIN " + ConBase.Database + @".dbo.Table_070_CountUnitInfo gg
                                                               ON  gg.Column00 = tcsf.column03
                                                   WHERE  (
                                                              dbo.Table_010_SaleFactor.column02 BETWEEN '{0}'  AND 
                                                              '{1}' 
                                                          )
                                                          AND dbo.Table_010_SaleFactor.column17 = 0
           
           
           
           
                                                   UNION all
           
           
           
                                                   SELECT --N'مرجوعي فاکتور فروش'  as [Type],
                                                          dbo.Table_018_MarjooiSale.column05 AS Visitor,
                                                          tmg.column02 AS MainGroup,
                                                          tsg.column03 AS SubGroup,
                                                          tcai.column01 AS GoodCode,
                                                          tcai.column02 AS GoodName,
                                                          gg.Column01 AS Unit,
                                                          (-1) * tcsf.column07 AS [ Count],
                                                            tcsf.column10 AS Vahed,
                                                          (-1) * tcsf.column17 AS Takhfif,
                                                          (-1) * tcsf.column19 AS Ezafe,
                                                          (-1) * tcsf.column11 AS [ Amount],
                                                          (-1) * tcsf.column20 AS [ NetAmount],
                                                          tcsf.columnid
                                                   FROM   dbo.Table_018_MarjooiSale
                                                          JOIN Table_019_Child1_MarjooiSale tcsf
                                                               ON  tcsf.column01 = dbo.Table_018_MarjooiSale.columnid
                                                          JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                               ON  tcai.columnid = tcsf.column02
                                                          LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                               ON  tmg.columnid = tcai.column03
                                                          LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                               ON  tsg.column01 = tcai.column03
                                                               AND tsg.columnid = tcai.column04
                                                          LEFT JOIN " + ConBase.Database + @".dbo.Table_070_CountUnitInfo gg
                                                               ON  gg.Column00 = tcsf.column03
                                                   WHERE  (
                                                              dbo.Table_018_MarjooiSale.column02 BETWEEN '{0}' 
                                                              AND '{1}' 
                                                          )
                                               ) AS dd
                                        GROUP BY
                                       -- [Type],
                                               Vahed,
                                               Visitor,
                                               MainGroup,
                                               SubGroup,
                                               GoodCode,
                                               GoodName,
                                               Unit
                                        ORDER BY
                                               dd.GoodCode,
                                               dd.Vahed desc";


                command = string.Format(command, Date1, Date2,
                   ConBase.Database);
                Adapter = new SqlDataAdapter(command, ConSale);
                DataTable Table1 = new DataTable();
                Adapter.Fill(Table1);
                dataTable4BindingSource.DataSource = Table1;

                tbl_VitorsBindingSource_PositionChanged(sender, e);


            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (gridEX_visitors.GetCheckedRows().Count() != 0)
            {
                string visitors = string.Empty;
                string visitorsName = string.Empty;
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_visitors.GetCheckedRows())
                {
                    visitors += Row.Cells["ColumnId"].Value + ",";
                    using (SqlConnection ConBASE = new SqlConnection(Properties.Settings.Default.BASE))
                    {

                        ConBASE.Open();
                        SqlCommand Command = new SqlCommand("Select top 1 Column02 from Table_045_PersonInfo where ColumnId=" + Row.Cells["ColumnId"].Value, ConBASE);
                        visitorsName += Command.ExecuteScalar().ToString() + " ,";

                    }
                }
                visitors = visitors.TrimEnd(',');
                visitorsName = visitorsName.TrimEnd(',');
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                string CommandText = null;


                CommandText = @"SELECT Visitor,
                                               MainGroup,
                                               SubGroup,
                                               GoodCode,
                                               GoodName,
                                               Unit,
                                               SUM(COUNT) AS [Count],
                                               SUM(Amount) AS [Amount],
                                               SUM(NetAmount) AS [NetAmount],
                                               ISNULL(
                                                   SUM(NetAmount) / NULLIF((ISNULL(SUM(ISNULL([Count], 0)), 0)), 0),
                                                   0
                                               ) AS Avrage,
                                               SUM(MCount) AS [MCount],
                                               SUM(MAmount) AS [MAmount],
                                               SUM(MNetAmount) AS [MNetAmount],
                                               ISNULL(
                                                   SUM(MNetAmount) / NULLIF((ISNULL(SUM(ISNULL([MCount], 0)), 0)), 0),
                                                   0
                                               ) AS MAvrage,
                                               SUM(ISNULL(COUNT, 0)) - SUM(ISNULL(MCOUNT, 0)) AS Tcount,
                                               SUM(ISNULL(Amount, 0)) - SUM(ISNULL(MAmount, 0)) AS TAmount,
                                               SUM(ISNULL(NetAmount, 0)) - SUM(ISNULL(MNetAmount, 0)) AS TNetAmount,
                                               (
                                                   (
                                                       ISNULL(
                                                           SUM(NetAmount) / NULLIF((ISNULL(SUM(ISNULL([Count], 0)), 0)), 0),
                                                           0
                                                       )
                                                   ) -(
                                                       ISNULL(
                                                           SUM(MNetAmount) / NULLIF((ISNULL(SUM(ISNULL([MCount], 0)), 0)), 0),
                                                           0
                                                       )
                                                   )
                                               ) AS TAvrage
                                        FROM   (
                                                   SELECT dbo.Table_010_SaleFactor.column05 AS Visitor,
                                                          tmg.column02 AS MainGroup,
                                                          tsg.column03 AS SubGroup,
                                                          tcai.column01 AS GoodCode,
                                                          tcai.column02 AS GoodName,
                                                          gg.Column01 AS Unit,
                                                          (tcsf.column07) AS [Count],
                                                          (tcsf.column11) AS [Amount],
                                                          (tcsf.column20) AS [NetAmount],
                                                          0.000 AS [MCount],
                                                          0.000 AS [MAmount],
                                                          0.000 AS [MNetAmount], 
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
                                                          LEFT JOIN " + ConBase.Database + @".dbo.Table_070_CountUnitInfo gg
                                                               ON  gg.Column00 = tcsf.column03
                                                   WHERE  (
                                                              dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND 
                                                              '{1}' and dbo.Table_010_SaleFactor.column05 in (" + visitors + @")
                                                          )
                                                          AND dbo.Table_010_SaleFactor.column17 = 0
           
           
           
           
                                                   UNION  all
           
           
           
                                                   SELECT dbo.Table_018_MarjooiSale.column05 AS Visitor,
                                                          tmg.column02 AS MainGroup,
                                                          tsg.column03 AS SubGroup,
                                                          tcai.column01 AS GoodCode,
                                                          tcai.column02 AS GoodName,
                                                          gg.Column01 AS Unit,
                                                          0.000 AS [Count],
                                                          0.000 AS [Amount],
                                                          0.000 AS [NetAmount],
                
                                                          tcsf.column07 AS [MCount],
                                                          tcsf.column11 AS [MAmount],
                                                          tcsf.column20 AS [MNetAmount],
                                                          tcsf.columnid

                                                   FROM   dbo.Table_018_MarjooiSale
                                                          JOIN Table_019_Child1_MarjooiSale tcsf
                                                               ON  tcsf.column01 = dbo.Table_018_MarjooiSale.columnid
                                                          JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                               ON  tcai.columnid = tcsf.column02
                                                          LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                               ON  tmg.columnid = tcai.column03
                                                          LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                               ON  tsg.column01 = tcai.column03
                                                               AND tsg.columnid = tcai.column04
                                                          LEFT JOIN " + ConBase.Database + @".dbo.Table_070_CountUnitInfo gg
                                                               ON  gg.Column00 = tcsf.column03
                                                   WHERE  (
                                                              dbo.Table_018_MarjooiSale.column02 BETWEEN '{0}' 
                                                              AND '{1}' and dbo.Table_018_MarjooiSale.column05 in (" + visitors + @")
                                                          )
                                               ) AS dd
                                        GROUP BY
                                               Visitor,
                                               MainGroup,
                                               SubGroup,
                                               GoodCode,
                                               GoodName,
                                               Unit ";

                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);


                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 46, Date1, Date2, visitorsName);
                    frm.ShowDialog();
                }
            }
        }
        private void Form19_Visitors_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.GoodReportByVisitors = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_Goods.RootTable.Groups.Clear();
            gridEX2.RootTable.Groups.Clear();
            gridEX2.RemoveFilters();
            gridEX_Goods.RemoveFilters();
            gridEX1.RootTable.Groups.Clear();
            gridEX1.RemoveFilters();
            gridEX_visitors.RemoveFilters();

        }

        private void tbl_VitorsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.Filter = "Visitor=" + gridEX_visitors.GetValue("ColumnId").ToString();
                dataTable4BindingSource.Filter = "Visitor=" + gridEX_visitors.GetValue("ColumnId").ToString();

                this.txt_Total.Value = 0;
                this.txt_Takhfif.Value = 0;
                txt_Ezafe.Value = 0;
                txt_Count.Value = 0;
                txt_netamount.Value = 0;
                txt_count1.Value = 0;

                this.txt_Amount2.Value = 0;
                this.txt_Net2.Value = 0;
                txt_count2.Value = 0;
                txt_count3.Value = 0;
                txt_Avaege.Value = 0;

            }
            catch
            {
            }
        }

        private void یکمسئولفروشToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridEX_Goods.Focused)
                gridEXPrintDocument1.GridEX = this.gridEX_Goods;
            else
                gridEXPrintDocument1.GridEX = this.gridEX1;

            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    //  string j = " تاریخ تهیه گزارش:" + FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");
                    gridEXPrintDocument1.PageHeaderLeft = "ازتاریخ: " + faDatePickerStrip1.FADatePicker.Text + "تاتاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                    gridEXPrintDocument1.PageHeaderRight = " نام مسئول فروش: " + gridEX_visitors.CurrentRow.Cells["Column02"].Text;
                    gridEXPrintDocument1.PageHeaderCenter = "گزارش فروش کالا";
                    printPreviewDialog1.ShowDialog();
                }
        }

        private void gridEX1_RowCheckStateChanged(object sender, Janus.Windows.GridEX.RowCheckStateChangeEventArgs e)
        {
            try
            {
                if (gridEX1.GetCheckedRows().Length > 0)
                {
                    uiGroupBox1.Visible = true;


                    this.txt_Total.Value = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Amount"].Value.ToString()));
                    this.txt_Takhfif.Value = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Takhfif"].Value.ToString()));
                    txt_Ezafe.Value = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Ezafe"].Value.ToString()));
                    txt_Count.Value = gridEX1.GetCheckedRows().Length;
                    txt_netamount.Value = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["NetAmount"].Value.ToString()));
                    txt_count1.Value = gridEX1.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Count"].Value.ToString()));
                }
                else uiGroupBox1.Visible = false;


            }
            catch { }
        }

        private void gridEX_Goods_RowCheckStateChanged(object sender, Janus.Windows.GridEX.RowCheckStateChangeEventArgs e)
        {
            try
            {
                if (gridEX_Goods.GetCheckedRows().Length > 0)
                {
                    uiGroupBox2.Visible = true;


                    this.txt_Amount2.Value = gridEX_Goods.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["TAmount"].Value.ToString()));
                    this.txt_Net2.Value = gridEX_Goods.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["TNetAmount"].Value.ToString()));
                    txt_count2.Value = gridEX_Goods.GetCheckedRows().Length;
                    txt_count3.Value = gridEX_Goods.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Count"].Value.ToString()));
                    txt_Avaege.Value = Convert.ToDecimal(gridEX_Goods.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["TAmount"].Value.ToString()))) /
                        Convert.ToDecimal(gridEX_Goods.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Count"].Value.ToString())));
                }
                else uiGroupBox2.Visible = false;


            }
            catch { }
        }

        private void چندمسئولفروشبهتفکیکToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridEX_visitors.GetCheckedRows().Count() != 0)
            {
                string visitors = string.Empty;
                string visitorsName = string.Empty;
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_visitors.GetCheckedRows())
                {
                    visitors += Row.Cells["ColumnId"].Value + ",";
                    using (SqlConnection ConBASE = new SqlConnection(Properties.Settings.Default.BASE))
                    {

                        ConBASE.Open();
                        SqlCommand Command = new SqlCommand("Select top 1 Column02 from Table_045_PersonInfo where ColumnId=" + Row.Cells["ColumnId"].Value, ConBASE);
                        visitorsName += Command.ExecuteScalar().ToString() + " ,";

                    }
                }
                visitors = visitors.TrimEnd(',');
                visitorsName = visitorsName.TrimEnd(',');
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                string CommandText = null;


                CommandText = @"SELECT --[Type],
                                                Visitor,
                                               MainGroup,
                                               SubGroup,
                                               GoodCode,
                                               GoodName,
                                               Unit,
                                               SUM(COUNT) AS [Count],
                                               Vahed,
                                               SUM(Amount) AS [Amount],
                                               SUM(dd.Takhfif) AS Takhfif,
                                               SUM(dd.Ezafe) AS Ezafe,
                                               SUM(NetAmount) AS [NetAmount]
                                        FROM   (
                                                   SELECT --N'فاکتور فروش'  as [Type],
                                                          dbo.Table_010_SaleFactor.column05 AS Visitor,
                                                          tmg.column02 AS MainGroup,
                                                          tsg.column03 AS SubGroup,
                                                          tcai.column01 AS GoodCode,
                                                          tcai.column02 AS GoodName,
                                                          gg.Column01 AS Unit,
                                                          (tcsf.column07) AS [Count],
                                                          tcsf.column10 AS Vahed,
                                                          tcsf.column17 AS Takhfif,
                                                          tcsf.column19 AS Ezafe,
                                                          (tcsf.column11) AS [Amount],
                                                          (tcsf.column20) AS [NetAmount],
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
                                                          LEFT JOIN " + ConBase.Database + @".dbo.Table_070_CountUnitInfo gg
                                                               ON  gg.Column00 = tcsf.column03
                                                   WHERE  (
                                                              dbo.Table_010_SaleFactor.column02 BETWEEN '{0}'  AND 
                                                              '{1}' and dbo.Table_010_SaleFactor.column05 in (" + visitors + @")
                                                          )
                                                          AND dbo.Table_010_SaleFactor.column17 = 0
           
           
           
           
                                                   UNION  all
           
           
           
                                                   SELECT --N'مرجوعي فاکتور فروش'  as [Type],
                                                          dbo.Table_018_MarjooiSale.column05 AS Visitor,
                                                          tmg.column02 AS MainGroup,
                                                          tsg.column03 AS SubGroup,
                                                          tcai.column01 AS GoodCode,
                                                          tcai.column02 AS GoodName,
                                                          gg.Column01 AS Unit,
                                                          (-1) * tcsf.column07 AS [ Count],
                                                            tcsf.column10 AS Vahed,
                                                          (-1) * tcsf.column17 AS Takhfif,
                                                          (-1) * tcsf.column19 AS Ezafe,
                                                          (-1) * tcsf.column11 AS [ Amount],
                                                          (-1) * tcsf.column20 AS [ NetAmount],
                                                          tcsf.columnid
                                                   FROM   dbo.Table_018_MarjooiSale
                                                          JOIN Table_019_Child1_MarjooiSale tcsf
                                                               ON  tcsf.column01 = dbo.Table_018_MarjooiSale.columnid
                                                          JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                               ON  tcai.columnid = tcsf.column02
                                                          LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                               ON  tmg.columnid = tcai.column03
                                                          LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                               ON  tsg.column01 = tcai.column03
                                                               AND tsg.columnid = tcai.column04
                                                          LEFT JOIN " + ConBase.Database + @".dbo.Table_070_CountUnitInfo gg
                                                               ON  gg.Column00 = tcsf.column03
                                                   WHERE  (
                                                              dbo.Table_018_MarjooiSale.column02 BETWEEN '{0}' 
                                                              AND '{1}' and dbo.Table_018_MarjooiSale.column05 in (" + visitors + @")
                                                          )
                                               ) AS dd
                                        GROUP BY
                                       -- [Type],
                                               Vahed,
                                               Visitor,
                                               MainGroup,
                                               SubGroup,
                                               GoodCode,
                                               GoodName,
                                               Unit
                                        ORDER BY
                                               dd.GoodCode,
                                               dd.Vahed desc";

                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);


                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_StimulateReportForm frm = new Form01_StimulateReportForm(Table, 1, Date1, Date2, visitorsName);
                    frm.ShowDialog();
                }
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (gridEX_visitors.GetCheckedRows().Count() != 0)
            {
                string visitors = string.Empty;
                string visitorsName = string.Empty;
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_visitors.GetCheckedRows())
                {
                    visitors += Row.Cells["ColumnId"].Value + ",";
                    using (SqlConnection ConBASE = new SqlConnection(Properties.Settings.Default.BASE))
                    {

                        ConBASE.Open();
                        SqlCommand Command = new SqlCommand("Select top 1 Column02 from Table_045_PersonInfo where ColumnId=" + Row.Cells["ColumnId"].Value, ConBASE);
                        visitorsName += Command.ExecuteScalar().ToString() + " ,";

                    }
                }
                visitors = visitors.TrimEnd(',');
                visitorsName = visitorsName.TrimEnd(',');
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                string CommandText = null;


                CommandText = @"SELECT --[Type],
                                               
                                               MainGroup,
                                               SubGroup,
                                               GoodCode,
                                               GoodName,
                                               Unit,
                                               SUM(COUNT) AS [Count],
                                               Vahed,
                                               SUM(Amount) AS [Amount],
                                               SUM(dd.Takhfif) AS Takhfif,
                                               SUM(dd.Ezafe) AS Ezafe,
                                               SUM(NetAmount) AS [NetAmount]
                                        FROM   (
                                                   SELECT --N'فاکتور فروش'  as [Type],
                                                          dbo.Table_010_SaleFactor.column05 AS Visitor,
                                                          tmg.column02 AS MainGroup,
                                                          tsg.column03 AS SubGroup,
                                                          tcai.column01 AS GoodCode,
                                                          tcai.column02 AS GoodName,
                                                          gg.Column01 AS Unit,
                                                          (tcsf.column07) AS [Count],
                                                          tcsf.column10 AS Vahed,
                                                          tcsf.column17 AS Takhfif,
                                                          tcsf.column19 AS Ezafe,
                                                          (tcsf.column11) AS [Amount],
                                                          (tcsf.column20) AS [NetAmount],
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
                                                          LEFT JOIN " + ConBase.Database + @".dbo.Table_070_CountUnitInfo gg
                                                               ON  gg.Column00 = tcsf.column03
                                                   WHERE  (
                                                              dbo.Table_010_SaleFactor.column02 BETWEEN '{0}'  AND 
                                                              '{1}' and dbo.Table_010_SaleFactor.column05 in (" + visitors + @")
                                                          )
                                                          AND dbo.Table_010_SaleFactor.column17 = 0
           
           
           
           
                                                   UNION all 
           
           
           
                                                   SELECT --N'مرجوعي فاکتور فروش'  as [Type],
                                                          dbo.Table_018_MarjooiSale.column05 AS Visitor,
                                                          tmg.column02 AS MainGroup,
                                                          tsg.column03 AS SubGroup,
                                                          tcai.column01 AS GoodCode,
                                                          tcai.column02 AS GoodName,
                                                          gg.Column01 AS Unit,
                                                          (-1) * tcsf.column07 AS [ Count],
                                                            tcsf.column10 AS Vahed,
                                                          (-1) * tcsf.column17 AS Takhfif,
                                                          (-1) * tcsf.column19 AS Ezafe,
                                                          (-1) * tcsf.column11 AS [ Amount],
                                                          (-1) * tcsf.column20 AS [ NetAmount],
                                                          tcsf.columnid
                                                   FROM   dbo.Table_018_MarjooiSale
                                                          JOIN Table_019_Child1_MarjooiSale tcsf
                                                               ON  tcsf.column01 = dbo.Table_018_MarjooiSale.columnid
                                                          JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                               ON  tcai.columnid = tcsf.column02
                                                          LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                               ON  tmg.columnid = tcai.column03
                                                          LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                               ON  tsg.column01 = tcai.column03
                                                               AND tsg.columnid = tcai.column04
                                                          LEFT JOIN " + ConBase.Database + @".dbo.Table_070_CountUnitInfo gg
                                                               ON  gg.Column00 = tcsf.column03
                                                   WHERE  (
                                                              dbo.Table_018_MarjooiSale.column02 BETWEEN '{0}' 
                                                              AND '{1}' and dbo.Table_018_MarjooiSale.column05 in (" + visitors + @")
                                                          )
                                               ) AS dd
                                        GROUP BY
                                       -- [Type],
                                               Vahed,
                                            
                                               MainGroup,
                                               SubGroup,
                                               GoodCode,
                                               GoodName,
                                               Unit
                                        ORDER BY
                                               dd.GoodCode,
                                               dd.Vahed desc";

                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);


                gridEX2.DataSource = Table;


                this.txt_Total2.Value = 0;
                this.txt_Takhfif2.Value = 0;
                txt_Ezafe2.Value = 0;
                txt_Count4.Value = 0;
                txt_netamount2.Value = 0;
                txt_count5.Value = 0;
            }

            else
            {
                DataTable dt = new DataTable();
                gridEX2.DataSource = dt;
            }
        }

        private void gridEX2_RowCheckStateChanged(object sender, Janus.Windows.GridEX.RowCheckStateChangeEventArgs e)
        {
            try
            {
                if (gridEX2.GetCheckedRows().Length > 0)
                {
                    uiGroupBox3.Visible = true;


                    this.txt_Total2.Value = gridEX2.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Amount"].Value.ToString()));
                    this.txt_Takhfif2.Value = gridEX2.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Takhfif"].Value.ToString()));
                    txt_Ezafe2.Value = gridEX2.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Ezafe"].Value.ToString()));
                    txt_Count4.Value = gridEX2.GetCheckedRows().Length;
                    txt_netamount2.Value = gridEX2.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["NetAmount"].Value.ToString()));
                    txt_count5.Value = gridEX2.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Count"].Value.ToString()));
                }
                else uiGroupBox3.Visible = false;


            }
            catch { }
        }

        private void نمودارToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable PrintTable = dataSet_Report.DataTable4;
            
            
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
                {
                    PrintTable.Rows.Add("",1,item.Cells["MainGroup"].Value ,
                        item.Cells["SubGroup"].Value ,
                        item.Cells["GoodCode"].Value ,
                        item.Cells["GoodName"].Value ,
                        item.Cells["Unit"].Value ,
                        item.Cells["Count"].Value ,
                        item.Cells["Vahed"].Value ,
                        item.Cells["Amount"].Value ,
                        item.Cells["Takhfif"].Value ,
                        item.Cells["Ezafe"].Value ,
                        item.Cells["NetAmount"].Value 

                        );
                }
            }
           
            if (PrintTable.Rows.Count > 0)
            {
                string name = string.Empty;
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_visitors.GetCheckedRows())
                {
                    name += Row.Cells["Column02"].Text + ",";
                }
                _06_Reports._02_Sale.Rpt27_GoodReportByVisitors Rpt = new Rpt27_GoodReportByVisitors();
                TextObject Title = (TextObject)Rpt.ReportDefinition.ReportObjects["Text3"];
                Title.Text = " نمودار فروش مستولین فروش: " + name.TrimEnd(',') + " - از تاریخ " + Date1 + " تا تاریخ " + Date2;
                Rpt.SetDataSource(PrintTable);
                Rpt.SetDataSource(PrintTable);
                crystalReportViewer1.ReportSource = Rpt;
                crystalReportViewer1.Refresh();
                crystalReportViewer1.Zoom(1);
            }
            this.Cursor = Cursors.Default;
        }

        private void چاپتجمیعیToolStripMenuItem_Click(object sender, EventArgs e)
        {

            gridEXPrintDocument1.GridEX = this.gridEX2;
            string name = string.Empty;
            foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_visitors.GetCheckedRows())
            {
                name += Row.Cells["Column02"].Text + ",";
            }
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    //  string j = " تاریخ تهیه گزارش:" + FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");
                    gridEXPrintDocument1.PageHeaderLeft = "ازتاریخ: " + faDatePickerStrip1.FADatePicker.Text + "تاتاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                    gridEXPrintDocument1.PageHeaderRight = " نام مسئول فروش: " + name.TrimEnd(',');
                    gridEXPrintDocument1.PageHeaderCenter = "گزارش فروش کالا";
                    printPreviewDialog1.ShowDialog();
                }
        }

    }
}
