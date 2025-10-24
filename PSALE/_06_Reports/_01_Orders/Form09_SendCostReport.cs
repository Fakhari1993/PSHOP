using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._06_Reports._01_Orders
{
    public partial class Form09_SendCostReport : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        string Date1 = null, Date2 = null;

        public Form09_SendCostReport()
        {
            InitializeComponent();
        }

        private void Form09_SendCostReport_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            cmb_ExtraReduction.ComboBox.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_024_Discount");
            cmb_ExtraReduction.ComboBox.DisplayMember = "Column01";
            cmb_ExtraReduction.ComboBox.ValueMember = "ColumnId";
            gridEX1.DropDowns["PersonGroup"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, @"Select * from(
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
            dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId) Tbl2) as PersonGroup"), "");
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

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_009_ExitPwhrs.column18 AS OrderID, dbo.Table_009_ExitPwhrs.column19 AS DraftID, dbo.Table_007_PwhrsDraft.column16 AS FactorID, 
                    dbo.Table_009_ExitPwhrs.column23 AS DocId, ISNULL(OrderTable.column01, 0) AS OrderNum, ISNULL(dbo.Table_009_ExitPwhrs.column01, 0) AS ExitNum, 
                    ISNULL(dbo.Table_007_PwhrsDraft.column01, 0) AS DraftNum, ISNULL(SaleFactorTable.column01, 0) AS FactorNum, ISNULL(SanadTable.Column00, 0) AS DocNum, 
                    dbo.Table_009_ExitPwhrs.column22 AS PayWithCustomer, dbo.Table_009_ExitPwhrs.column21 AS SendCost, dbo.Table_009_ExitPwhrs.column05 AS CustomerID, 
                    PersonTable.Column01 AS CustomerCode, PersonTable.Column02 AS CustomerName, dbo.Table_009_ExitPwhrs.column02 AS Date, 
                    dbo.Table_009_ExitPwhrs.column09 AS DriverName, ProvinceTable.Column01 AS Province, CityTable.Column02 AS City, 
                    dbo.Table_009_ExitPwhrs.column15 AS SendTo, dbo.Table_009_ExitPwhrs.column16 AS Address, ISNULL(Child2_SaleFactor.Expr1, 0.0) AS SumExtraReduction, 
                    dbo.Table_009_ExitPwhrs.column21 - ISNULL(Child2_SaleFactor.Expr1, 0.0) AS Difference
                    FROM         (SELECT     columnid, column01
                    FROM         {1}.dbo.Table_010_SaleFactor) AS SaleFactorTable INNER JOIN
                    (SELECT     SUM(column04) AS Expr1, column01
                    FROM         {1}.dbo.Table_012_Child2_SaleFactor
                    WHERE     (column02 = {5})
                    GROUP BY column01) AS Child2_SaleFactor ON SaleFactorTable.columnid = Child2_SaleFactor.column01 RIGHT OUTER JOIN
                    dbo.Table_007_PwhrsDraft ON SaleFactorTable.columnid = dbo.Table_007_PwhrsDraft.column16 RIGHT OUTER JOIN
                    dbo.Table_009_ExitPwhrs LEFT OUTER JOIN
                    (SELECT     Column01, Column02
                    FROM         {0}.dbo.Table_065_CityInfo) AS CityTable ON dbo.Table_009_ExitPwhrs.column14 = CityTable.Column01 LEFT OUTER JOIN
                    (SELECT     Column00, Column01
                    FROM         {0}.dbo.Table_060_ProvinceInfo) AS ProvinceTable ON dbo.Table_009_ExitPwhrs.column13 = ProvinceTable.Column00 LEFT OUTER JOIN
                    (SELECT     ColumnId, Column01, Column02
                    FROM         {0}.dbo.Table_045_PersonInfo) AS PersonTable ON dbo.Table_009_ExitPwhrs.column05 = PersonTable.ColumnId LEFT OUTER JOIN
                    (SELECT     ColumnId, Column00
                    FROM         {2}.dbo.Table_060_SanadHead) AS SanadTable ON dbo.Table_009_ExitPwhrs.column23 = SanadTable.ColumnId ON 
                    dbo.Table_007_PwhrsDraft.columnid = dbo.Table_009_ExitPwhrs.column19 LEFT OUTER JOIN
                    (SELECT     columnid, column01
                    FROM         {1}.dbo.Table_005_OrderHeader) AS OrderTable ON dbo.Table_009_ExitPwhrs.column18 = OrderTable.columnid
                    WHERE     (dbo.Table_009_ExitPwhrs.column02 BETWEEN '{3}' AND '{4}')", ConWare);

                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, 
                    ConBase.Database, ConSale.Database, ConAcnt.Database,Date1,
                    Date2,cmb_ExtraReduction.ComboBox.SelectedValue.ToString());
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                gridEX1.DataSource = Table;
                gridEX1.Row = gridEX1.FilterRow.Position;


            }
        }

        private void Form09_SendCostReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
                bt_Search_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }

        private void Form09_SendCostReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_SendCostReport.Clone();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                Table.Rows.Add(item.Cells["OrderID"].Text.ToString(),
                    item.Cells["ExitID"].Text.ToString(),
                    item.Cells["DraftID"].Text.ToString(),
                    item.Cells["FactorID"].Text.ToString(),
                    item.Cells["DocID"].Text.ToString(),
                    item.Cells["PayWithCustomer"].Value.ToString(),
                    item.Cells["SendCost"].Value.ToString(),
                    item.Cells["CustomerCode"].Text.ToString(),
                    item.Cells["CustomerName"].Text.ToString(),
                    item.Cells["Date"].Text.ToString(),
                    item.Cells["DriverName"].Text.ToString(),
                    item.Cells["Province"].Text.ToString(),
                    item.Cells["City"].Text.ToString(),
                    item.Cells["SendTo"].Text.ToString(),
                    item.Cells["Address"].Text.ToString());
            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 8,  Date1,  Date2);
                frm.ShowDialog();
            }
        }

        private void mnu_ExportToExcel_Click(object sender, EventArgs e)
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
