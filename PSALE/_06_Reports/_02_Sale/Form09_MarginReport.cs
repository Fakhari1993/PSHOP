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
    public partial class Form09_MarginReport : Form
    {
        bool _BackSpace = false;
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        string Date1, Date2;

        public Form09_MarginReport()
        {
            InitializeComponent();
        }

        private void Form09_MarginReport_Load(object sender, EventArgs e)
        {
            DataTable GoodTable=clDoc.ReturnTable(ConWare.ConnectionString,"Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients");
            gridEX1.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX1.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            string[] Dates = Properties.Settings.Default.MarginReport.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);

            ////faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            ////faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
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

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (this.gridEX1.RowCount > 0)
            {
                DataTable Table = dataSet_Report.Rpt_Margin.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    Table.Rows.Add(item.Cells["Column02"].Text,
                        item.Cells["Column002"].Text,
                        item.Cells["TotalNumber"].Value.ToString(),
                        item.Cells["TotalDay"].Value.ToString(),
                        item.Cells["TotalFactor"].Value.ToString(),
                        item.Cells["TotalCurrency"].Value.ToString(),
                        item.Cells["AvgFactor"].Value.ToString(),
                        item.Cells["AvgDay"].Value.ToString(),
                        item.Cells["AvgCurrency"].Value.ToString(),
                        item.Cells["TotalFee"].Value.ToString(),
                        item.Cells["AvgFee"].Value.ToString(),
                        item.Cells["MarginFactor"].Value.ToString(),
                        item.Cells["MarginDay"].Value.ToString(),
                        item.Cells["TotalMargin_Factor"].Value.ToString(),
                        item.Cells["TotalMargin_Day"].Value.ToString()
                        
                        
                        );
                }

                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 28, Date1, Date1);
                    frm.ShowDialog();
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
                    bt_Display_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }
      
        private void Form09_MarginReport_Activated(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.Select();
        }

        private void Form09_MarginReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.MarginReport = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX1.RemoveFilters();
        }

        private void Form09_MarginReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Display_Click(sender, e);
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

        private void bt_Display_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT column02,
       TotalNumber,
       TotalDay,
       TotalFactor,
       TotalCurrency,
       TotalFactor / TotalNumber AS AvgFactor,
       TotalDay / TotalNumber AS AvgDay,
       TotalCurrency / TotalNumber AS AvgCurrency,
       TotalFee,
       AvgFee,
       (TotalFactor - TotalFee) / TotalNumber AS MarginFactor,
       (TotalDay - TotalFee) 
       / TotalNumber AS MarginDay,
       TotalFactor - TotalFee AS TotalMargin_Factor,
       TotalDay - TotalFee AS TotalMargin_Day,
        Project
FROM   (
           SELECT column02,
                  SUM(TotalNumber) AS TotalNumber,
                  SUM(Riali_DayDate) AS TotalDay,
                  SUM(Riali_FactorDate) AS TotalFactor,
                  SUM(TotalCurrency) AS TotalCurrency,
                  SUM(TotalFee) AS TotalFee ,
                  SUM(AvgFee) as AvgFee,
                  Project
           FROM   (
                      SELECT ChildDraft.column16 AS TotalFee,
                             ChildDraft.column16 / factor.TotalNumber AS AvgFee,
                             CASE 
                                  WHEN factor.column14 
                                       IS NULL THEN factor.column11
                                  ELSE factor.column11 
                                       * CurrencyTable.Column02
                             END AS Riali_DayDate,
                             factor.column02,
                             factor.TotalNumber,
                             factor.Riali_FactorDate,
                             factor.TotalCurrency,
                             factor.Project

                             --,
                             --*
                      FROM   (
                                 SELECT TOP(100) PERCENT ROW_NUMBER() OVER(
                                            PARTITION BY 
                                            Table_011_Child1_SaleFactor.column01
                                            ORDER BY Table_011_Child1_SaleFactor.columnid,
                                            Table_011_Child1_SaleFactor.column02
                                        ) AS Row,
                                        dbo.Table_011_Child1_SaleFactor.column02,
                                        dbo.Table_010_SaleFactor.column02 AS 
                                        [date],
                                        dbo.Table_011_Child1_SaleFactor.column07 AS 
                                        TotalNumber,
                                        dbo.Table_011_Child1_SaleFactor.column14,
                                        dbo.Table_011_Child1_SaleFactor.column11,
                                        dbo.Table_010_SaleFactor.column09,
                                        CASE 
                                             WHEN dbo.Table_011_Child1_SaleFactor.column14 
                                                  IS NULL THEN dbo.Table_011_Child1_SaleFactor.column11
                                             ELSE dbo.Table_011_Child1_SaleFactor.column11 
                                                  * Table_011_Child1_SaleFactor.Column15
                                        END AS Riali_FactorDate,
                                        CASE 
                                             WHEN Table_011_Child1_SaleFactor.Column14 
                                                  IS 
                                                  NULL THEN 0
                                             ELSE Table_011_Child1_SaleFactor.Column11
                                        END AS TotalCurrency
                                        ,ppg.Column02 as Project
                                 FROM   dbo.Table_010_SaleFactor
                                        INNER JOIN dbo.Table_011_Child1_SaleFactor
                                             ON  dbo.Table_010_SaleFactor.columnid = 
                                                 dbo.Table_011_Child1_SaleFactor.column01
                                 left join " + ConBase.Database + @".dbo.Table_035_ProjectInfo ppg on ppg.Column00=dbo.Table_011_Child1_SaleFactor.column22
                                        
                             ) AS factor
                             LEFT OUTER JOIN (
                                      SELECT Column00,
                                             Column01,
                                             Column02,
                                             Column03,
                                             Column04
                                      FROM   {2}.dbo.Table_055_CurrencyInfo
                                  ) AS CurrencyTable
                                  ON  factor.column14 = CurrencyTable.Column00
                             INNER JOIN (
                                      SELECT ROW_NUMBER() OVER(PARTITION BY column01 ORDER BY columnid, column02) AS 
                                             Row,
                                             columnid,
                                             column01,
                                             column02,
                                             column03,
                                             column04,
                                             column05,
                                             column06,
                                             column07,
                                             column08,
                                             column09,
                                             column10,
                                             column11,
                                             column12,
                                             column13,
                                             column14,
                                             column15,
                                             column16,
                                             column17,
                                             column18,
                                             column19,
                                             column20,
                                             column21,
                                             column22,
                                             column23,
                                             column24,
                                             column25,
                                             column26,
                                             column27,
                                             column28,
                                             column29
                                      FROM    {3}.dbo.Table_008_Child_PwhrsDraft
                                  ) AS ChildDraft
                                  ON  factor.column09 = ChildDraft.column01
                                  AND factor.column02 = ChildDraft.column02
                      WHERE  (factor.date >= '{0}')
                             AND (factor.date <= '{1}')
                             AND (factor.column09 <> 0)
                             AND factor.Row = ChildDraft.Row
                  ) AS TotalTable
           GROUP BY
                  column02,
                  TotalFee,
                  AvgFee,Project
       ) AS Table2", ConSale);

                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText,
                    Date1, Date2,ConBase.Database, ConWare.Database);
                
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                gridEX1.DataSource = Table;
                gridEX1.Row = gridEX1.FilterRow.Position;
            }
        }
    }
}
