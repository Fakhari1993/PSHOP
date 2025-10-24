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
    public partial class Form08_TotalOrderReport : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        string Date1 = null, Date2 = null;

        public Form08_TotalOrderReport()
        {
            InitializeComponent();
        }

        private void Form08_TotalOrderReport_Load(object sender, EventArgs e)
        {
            DataTable ProvinceTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 FROM Table_060_ProvinceInfo");
            DataTable CityTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column01,Column02 From Table_065_CityInfo");
            DataTable StateTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column03 from Table_160_States");
            gridEX1.DropDowns["Province"].SetDataBinding(ProvinceTable, "");
            gridEX1.DropDowns["City"].SetDataBinding(CityTable, "");
            gridEX1.DropDowns["State"].SetDataBinding(StateTable, "");
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


            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
        }

        private void faDatePickerStrip1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox =
                    (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


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

        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents ClsDoc = new Classes.Class_Documents();
        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue &&
                faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                  Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;
                string CommandStr = null;
               

                    this.Cursor = Cursors.WaitCursor;

                    CommandStr = @"SELECT dbo.Table_006_OrderDetails.column02 AS 
            GoodID, dbo.Table_005_OrderHeader.column03 AS CustomerID, 
            SUM(dbo.Table_006_OrderDetails.column06) AS TotalOrder, 
            SUM(dbo.Table_006_OrderDetails.column04) AS BoxOrder, 
            SUM(dbo.Table_006_OrderDetails.column03)as PackOrder,
            SUM(dbo.Table_006_OrderDetails.column17) AS TotalExit,
            SUM(dbo.Table_006_OrderDetails.column16) AS BoxExit,
            SUM(dbo.Table_006_OrderDetails.column15) AS PackExit
            , SUM(dbo.Table_006_OrderDetails.column04)-SUM(dbo.Table_006_OrderDetails.column16) as BoxDiff,
            SUM(dbo.Table_006_OrderDetails.column03)-SUM(dbo.Table_006_OrderDetails.column15) as PackDiff,
            SUM(dbo.Table_006_OrderDetails.column06)-SUM(dbo.Table_006_OrderDetails.column17) as TotalDiff
            , PersonTable.Column01 AS CustomerCode, PersonTable.Column02 AS CustomerName, 
            GoodTable.column01 AS GoodCode, 
            GoodTable.column02 AS GoodName, SubGroup.column03 AS SubGroupName, 
            MainGroup.column02 AS MainGroupName, PersonTable.Column21 AS Province, PersonTable.Column22 AS City, PersonTable.column29 AS State
            FROM         (SELECT     columnid, column01, column02, column03, column04
            FROM          {3}.dbo.table_003_SubsidiaryGroup) AS SubGroup INNER JOIN
            (SELECT     columnid, column01, column02, column03
            FROM          {3}.dbo.table_002_MainGroup) AS MainGroup ON SubGroup.column01 = 
            MainGroup.columnid RIGHT OUTER JOIN
            (SELECT     columnid, column01, column02, column03, column04
            FROM          {3}.dbo.table_004_CommodityAndIngredients) AS GoodTable ON 
            SubGroup.columnid = GoodTable.column04 RIGHT OUTER JOIN
            dbo.Table_005_OrderHeader INNER JOIN
            dbo.Table_006_OrderDetails ON dbo.Table_005_OrderHeader.columnid = 
            dbo.Table_006_OrderDetails.column01 ON 
            GoodTable.columnid = dbo.Table_006_OrderDetails.column02 LEFT OUTER JOIN
            (SELECT     ColumnId, Column01, Column02, Column21, Column22, column29
            FROM          {2}.dbo.Table_045_PersonInfo) AS PersonTable ON 
            dbo.Table_005_OrderHeader.column03 = PersonTable.ColumnId
            WHERE     (dbo.Table_005_OrderHeader.column02 >= '{0}') AND 
            (dbo.Table_005_OrderHeader.column02 <=  '{1}')
            GROUP BY dbo.Table_006_OrderDetails.column02, dbo.Table_005_OrderHeader.column03, 
            PersonTable.Column01, PersonTable.Column02, GoodTable.column01, 
            GoodTable.column02, SubGroup.column03, MainGroup.column02, 
            PersonTable.Column21, PersonTable.Column22, PersonTable.column29
            ORDER BY GoodID";
              
                CommandStr = string.Format(CommandStr,
                     Date1, Date2,
                     ConBase.Database, ConWare.Database);

                gridEX1.DataSource = ClsDoc.ReturnTable(ConSale.ConnectionString, CommandStr);
                this.Cursor = Cursors.Default;
                gridEX1.Row = gridEX1.FilterRow.Position;

            }

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_CompeleteOrderReport.Clone();
            for (int i = 0; i < gridEX1.RootTable.Groups.Count; i++)
            {
                gridEX1.RootTable.Groups.RemoveAt(i);
            }
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                Table.Rows.Add(item.Cells["MainGroupName"].Text, item.Cells["SubGroupName"].Text,
                    item.Cells["GoodCode"].Text, item.Cells["GoodName"].Text,
                    item.Cells["CustomerCode"].Text,
                    item.Cells["CustomerName"].Text, item.Cells["TotalOrder"].Value.ToString(),
                    item.Cells["TotalExit"].Value.ToString(), item.Cells["TotalDiff"].Value.ToString());
            }
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 7,
                    Date1, Date2);
                frm.ShowDialog();
            }
        }

        private void Form08_TotalOrderReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Search_Click(sender, e);
        }

        private void Form08_TotalOrderReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
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
