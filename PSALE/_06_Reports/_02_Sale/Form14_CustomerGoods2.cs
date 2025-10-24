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
    public partial class Form14_CustomerGoods2 : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1 = null, Date2 = null;

        public Form14_CustomerGoods2()
        {
            InitializeComponent();
        }

        private void Form14_CustomerGoods_Load(object sender, EventArgs e)
        {
            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            string[] Dates = Properties.Settings.Default.CustomerGoods.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
            DataTable PersonGroup = clDoc.ReturnTable(ConBase.ConnectionString, @"Select * from(
            Select distinct Tbl2.PersonId, 
            substring((Select ','+Tbl1.GroupName   AS [text()]
            From (SELECT     dbo.Table_045_PersonInfo.ColumnId AS PersonId, ISNULL(dbo.Table_040_PersonGroups.Column01, 'عمومی') AS GroupName
            FROM         dbo.Table_040_PersonGroups INNER JOIN
            dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 RIGHT OUTER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId) Tbl1
            Where Tbl1.PersonId = Tbl2.PersonId
              
            For XML PATH ('')),2, 1000) [PersonGroup]
            From (SELECT     dbo.Table_045_PersonInfo.ColumnId AS PersonId, ISNULL(dbo.Table_040_PersonGroups.Column01, 'عمومی') AS GroupName
            FROM         dbo.Table_040_PersonGroups INNER JOIN
            dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 RIGHT OUTER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId) Tbl2) as PersonGroup");

            gridEX_Goods.DropDowns["PersonGroup"].SetDataBinding(PersonGroup, "");
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

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                string CommandText = null;

                CommandText = @"SELECT dbo.Table_010_SaleFactor.column03 AS CustomerId,
                                                               PersonTable.Column01 AS CustomerCode,
                                                               PersonTable.Column02 AS CustomerName
                                                        FROM   dbo.Table_010_SaleFactor
      
                                                               LEFT OUTER JOIN (
                                                                        SELECT ColumnId,
                                                                               Column01,
                                                                               Column02,
                                                                               Column21,
                                                                               Column22,
                                                                               column29
                                                                        FROM   {2}.dbo.Table_045_PersonInfo
                                                                    ) AS PersonTable
                                                                    ON  dbo.Table_010_SaleFactor.column03 = PersonTable.ColumnId
                                                        WHERE  (dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND '{1}')
                                                        GROUP BY
                                                               dbo.Table_010_SaleFactor.column03,
                                                               PersonTable.Column01,
                                                               PersonTable.Column02";

                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database, ConWare.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);

                bindingSource2.DataSource = Table;


                Adapter = new SqlDataAdapter(@"SELECT tmg.column02 AS MainGroup,
                                                                           tsg.column03 AS SubGroup,
                                                                           tcai.column01 AS GoodCode,
                                                                           tcai.column02 AS GoodName,
                                                                           k.column02 AS GoodID,
                                                                           SUM(k.column04) AS BoxNumber,
                                                                           SUM(k.column05) AS PackNumber,
                                                                           SUM(k.column06) AS DetailNumber,
                                                                           SUM(k.column07) AS TotalNumber,
                                                                           SUM(k.column11) AS TotalPrice,
                                                                           SUM(k.column17) AS Discount,
                                                                           SUM(k.column19) AS Extra,
                                                                           SUM(k.column20) AS Net,
                                                                           SUM(k.column20) / NULLIF(SUM(k.column07), 0) AS Fee,
                                                                           tsf.column03 AS CustomerId
                                                                    FROM   Table_011_Child1_SaleFactor k JOIN Table_010_SaleFactor tsf ON tsf.columnid = k.column01
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                                ON  tcai.columnid = k.column02
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                                                ON  tsg.column01 = tcai.column03
                                                                                AND tsg.columnid = tcai.column04
                                                                           LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                                                ON  tmg.columnid = tsg.column01
                                                                    WHERE (tsf.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"')
                                                                    GROUP BY
                                                                           k.column02,
                                                                           tmg.column02,
                                                                           tsg.column03,
                                                                           tcai.column01,
                                                                           tcai.column02,tsf.column03", ConSale);



                DataTable Table2 = new DataTable();
                Adapter.Fill(Table2);
                bindingSource1.DataSource = Table2;

                bindingSource2_PositionChanged(sender, e);
            }

        }

        private void bindingSource2_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.Filter = " CustomerId=" + gridEX_Goods.GetValue("CustomerId").ToString();
            }
            catch
            {
            }
        }



        private void mnu_ExportToExcel_Detail_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
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

        private void Form14_CustomerGoods_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable Table = dataSet_Report.Rpt_Sale_CustomerGood.Clone();
                gridEX1.RootTable.Groups.Clear();
                foreach (Janus.Windows.GridEX.GridEXRow dr1 in gridEX_Goods.GetCheckedRows())
                {
                    gridEX_Goods.MoveTo(dr1);
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                    {
                        DataRow dr = Table.NewRow();
                        dr["CustomerId"] = dr1.Cells["CustomerId"].Value;
                        dr["CustomerCode"] = dr1.Cells["CustomerCode"].Text;
                        dr["CustomerName"] = dr1.Cells["CustomerName"].Text;
                        dr["GoodId"] = item.Cells["GoodID"].Value;
                        dr["GoodCode"] = item.Cells["GoodCode"].Value;
                        dr["GoodName"] = item.Cells["GoodName"].Value;
                        dr["CountUnit"] = item.Cells["Fee"].Value;
                        dr["BoxNumber"] = item.Cells["BoxNumber"].Value;
                        dr["PackNumber"] = item.Cells["PackNumber"].Value;
                        dr["DetailNumber"] = item.Cells["DetailNumber"].Value;
                        dr["TotalNumber"] = item.Cells["TotalNumber"].Value;
                        dr["TotalPrice"] = item.Cells["TotalPrice"].Value;
                        dr["TotalDiscount"] = item.Cells["Discount"].Value;
                        dr["TotalExtra"] = item.Cells["Extra"].Value;
                        dr["NetPrice"] = item.Cells["Net"].Value;
                        Table.Rows.Add(dr);
                    }
                }
                if (Table.Rows.Count > 0)
                {
                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 37, Date1, Date2);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void Form14_CustomerGoods_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.CustomerGoods = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_Goods.RootTable.Groups.Clear();
            gridEX_Goods.RemoveFilters();
        }
    }
}
