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
    public partial class Form22_MergeReport : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        Classes.Class_Documents clDoc=new Classes.Class_Documents();
        bool _BackSpace = false;
        DataSet DataSet1 = new DataSet();
        SqlDataAdapter MainAdapter, GoodAdapter, ExtraAdapter;

        public Form22_MergeReport()
        {
            InitializeComponent();
        }

        private void Form22_MergeReport_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-2);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            DataTable PersonTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX1.DropDowns["PersonCode"].SetDataBinding(PersonTable, "");
            gridEX1.DropDowns["PersonName"].SetDataBinding(PersonTable, "");
            
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
            gridEX1.DropDowns["PersonGroup"].SetDataBinding(PersonGroup, "");

            cmb_CarryPrice.ComboBox.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_024_Discount");
            cmb_CarryPrice.ComboBox.DisplayMember = "Column01";
            cmb_CarryPrice.ComboBox.ValueMember = "ColumnId";

            DataTable GoodTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients");
            gridEX_Good.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_Good.DropDowns["GoodName"].SetDataBinding(GoodTable, "");


        }

        private void bt_ExportToExcel_MainTable_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void bt_ExportToExcel_Factors_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Good;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue && cmb_CarryPrice.Text.Trim()!="")
            {
                    string SelectCommand = @"SELECT     MainTable.SaleID, MainTable.Number, MainTable.Customer, MainTable.Box, CAST(MainTable.TotalPrice AS decimal(18, 3)) AS TotalPrice, 
                    CAST(MainTable.Discount AS decimal(18, 3)) AS Discount, CAST(MainTable.TotalPrice - MainTable.Discount AS decimal(18, 3)) AS PriceAfterDis, 
                    CAST(MainTable.Gift AS decimal(18, 3)) AS Gift, CAST(MainTable.TotalPrice - MainTable.Discount - MainTable.Gift AS decimal(18, 3)) AS AfterGift, 
                    CAST(ISNULL(Carry_1.CarryPrice, 0) AS decimal(18, 3)) AS CarryPrice, CAST(ISNULL(Reduction_1.Reduction, 0) AS decimal(18, 3)) AS Reduction, 
                    CAST(ISNULL(Extra_1.Extra, 0) AS decimal(18, 3)) AS Extra,0.000 as TotalReduction, 0.000 as FinalPrice
                    FROM         (SELECT     TOP (100) PERCENT dbo.Table_010_SaleFactor.columnid AS SaleID, dbo.Table_010_SaleFactor.column01 AS Number, 
                    dbo.Table_010_SaleFactor.column03 AS Customer, SUM(dbo.Table_011_Child1_SaleFactor.column04) AS Box, 
                    CASE WHEN Table_010_SaleFactor.Column12 = 0 THEN SUM(dbo.Table_011_Child1_SaleFactor.column11) 
                    ELSE SUM(dbo.Table_011_Child1_SaleFactor.column11 * Table_010_SaleFactor.column41) END AS TotalPrice, 
                    SUM(CASE WHEN Table_011_Child1_SaleFactor.column16 < 100 THEN CASE WHEN Table_010_SaleFactor.Column12 = 0 THEN dbo.Table_011_Child1_SaleFactor.column17
                    ELSE dbo.Table_011_Child1_SaleFactor.column17 * Table_010_SaleFactor.column41 END ELSE 0 END) AS Discount, 
                    SUM(CASE WHEN Table_011_Child1_SaleFactor.column16 = 100 THEN CASE WHEN Table_010_SaleFactor.column12 = 0 THEN dbo.Table_011_Child1_SaleFactor.column17
                    ELSE dbo.Table_011_Child1_SaleFactor.column17 * Table_010_SaleFactor.Column41 END ELSE 0 END) AS Gift
                    FROM         dbo.Table_010_SaleFactor LEFT OUTER JOIN
                    dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                    WHERE     (dbo.Table_010_SaleFactor.column02 >= '" + faDatePickerStrip1.FADatePicker.Text + @"') AND (dbo.Table_010_SaleFactor.column02 <= '" + faDatePickerStrip2.FADatePicker.Text + @"')
                    GROUP BY dbo.Table_010_SaleFactor.columnid, dbo.Table_010_SaleFactor.column01, dbo.Table_010_SaleFactor.column03, 
                    dbo.Table_010_SaleFactor.column12
                    ORDER BY SaleID) AS MainTable LEFT OUTER JOIN
                    (SELECT     Table_010_SaleFactor_3.columnId AS SaleID, SUM(Table_012_Child2_SaleFactor_2.column04) AS Extra
                    FROM         dbo.Table_010_SaleFactor AS Table_010_SaleFactor_3 INNER JOIN
                    dbo.Table_012_Child2_SaleFactor AS Table_012_Child2_SaleFactor_2 ON 
                    Table_010_SaleFactor_3.columnid = Table_012_Child2_SaleFactor_2.column01
                    WHERE     (Table_012_Child2_SaleFactor_2.column02 <> N'0') AND (Table_012_Child2_SaleFactor_2.column02 <> " + cmb_CarryPrice.ComboBox.SelectedValue.ToString() + @") AND 
                    (Table_012_Child2_SaleFactor_2.column05 = 0)
                    GROUP BY Table_010_SaleFactor_3.columnid) AS Extra_1 ON MainTable.SaleID = Extra_1.SaleID LEFT OUTER JOIN
                    (SELECT     Table_010_SaleFactor_2.columnid, SUM(Table_012_Child2_SaleFactor_1.column04) AS Reduction
                    FROM         dbo.Table_010_SaleFactor AS Table_010_SaleFactor_2 INNER JOIN
                    dbo.Table_012_Child2_SaleFactor AS Table_012_Child2_SaleFactor_1 ON 
                    Table_010_SaleFactor_2.columnid = Table_012_Child2_SaleFactor_1.column01
                    WHERE     (Table_012_Child2_SaleFactor_1.column05 = 1) AND (Table_012_Child2_SaleFactor_1.column02 > 0) AND 
                    (Table_012_Child2_SaleFactor_1.column02 <> " + cmb_CarryPrice.ComboBox.SelectedValue.ToString() + @")
                    GROUP BY Table_010_SaleFactor_2.columnid) AS Reduction_1 ON MainTable.SaleID = Reduction_1.columnId LEFT OUTER JOIN
                    (SELECT     Table_010_SaleFactor_1.columnId AS SaleID, SUM(dbo.Table_012_Child2_SaleFactor.column04) AS CarryPrice
                    FROM         dbo.Table_010_SaleFactor AS Table_010_SaleFactor_1 INNER JOIN
                    dbo.Table_012_Child2_SaleFactor ON Table_010_SaleFactor_1.columnid = dbo.Table_012_Child2_SaleFactor.column01
                    WHERE     (dbo.Table_012_Child2_SaleFactor.column02 = " + cmb_CarryPrice.ComboBox.SelectedValue.ToString() + @")
                    GROUP BY Table_010_SaleFactor_1.columnid) AS Carry_1 ON MainTable.SaleID = Carry_1.SaleID";

                    DataSet1.Reset();
                    MainAdapter = new SqlDataAdapter(SelectCommand, ConSale);
                    MainAdapter.Fill(DataSet1, "Main");
                    DataSet1.Tables["Main"].Columns["TotalReduction"].Expression = "Discount+Gift+CarryPrice+Reduction";
                    DataSet1.Tables["Main"].Columns["FinalPrice"].Expression = "TotalPrice-TotalReduction+Extra";

                    string DetailCommand = @"SELECT     dbo.Table_010_SaleFactor.columnid as SaleID, dbo.Table_010_SaleFactor.column01 as Number, dbo.Table_011_Child1_SaleFactor.column02 as GoodID, 
                    dbo.Table_011_Child1_SaleFactor.column04 as Box, 
                    CAST(CASE WHEN dbo.Table_010_SaleFactor.column12 = 0 THEN dbo.Table_011_Child1_SaleFactor.Column11 ELSE Table_010_SaleFactor.Column41 * dbo.Table_011_Child1_SaleFactor.Column11
                    END AS decimal(18, 3)) AS TotalPrice, 
                    CAST(CASE WHEN Table_011_Child1_SaleFactor.column16 < 100 THEN CASE WHEN Table_010_SaleFactor.Column12 = 0 THEN dbo.Table_011_Child1_SaleFactor.column17
                    ELSE dbo.Table_011_Child1_SaleFactor.column17 * Table_010_SaleFactor.column41 END ELSE 0 END AS decimal(18, 3)) AS Discount, 
                    CAST(CASE WHEN Table_011_Child1_SaleFactor.column16 = 100 THEN CASE WHEN Table_010_SaleFactor.column12 = 0 THEN dbo.Table_011_Child1_SaleFactor.column17
                    ELSE dbo.Table_011_Child1_SaleFactor.column17 * Table_010_SaleFactor.Column41 END ELSE 0 END AS decimal(18, 3)) AS Gift,0.000 as AfterDis,0.000 as AfterGift
                    FROM         dbo.Table_010_SaleFactor INNER JOIN
                    dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                    WHERE     (dbo.Table_010_SaleFactor.column02 >= '" + faDatePickerStrip1.FADatePicker.Text + "') AND (dbo.Table_010_SaleFactor.column02 <= '" + faDatePickerStrip2.FADatePicker.Text + "')";

                    GoodAdapter = new SqlDataAdapter(DetailCommand, ConSale);
                    GoodAdapter.Fill(DataSet1, "Detail");
                    DataSet1.Tables["Detail"].Columns["AfterDis"].Expression = "TotalPrice-Discount";
                    DataSet1.Tables["Detail"].Columns["AfterGift"].Expression = "TotalPrice-Discount-Gift";


                    string ExtraCommand = @"SELECT     dbo.Table_010_SaleFactor.columnid AS SaleID, dbo.Table_010_SaleFactor.column01 AS Number, dbo.Table_024_Discount.column01 AS Name, 
                    dbo.Table_012_Child2_SaleFactor.column04 * CASE WHEN dbo.Table_012_Child2_SaleFactor.column05 = 0 THEN 1 ELSE - 1 END AS Price
                    FROM         dbo.Table_010_SaleFactor INNER JOIN
                    dbo.Table_012_Child2_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_012_Child2_SaleFactor.column01 LEFT OUTER JOIN
                    dbo.Table_024_Discount ON dbo.Table_012_Child2_SaleFactor.column02 = dbo.Table_024_Discount.columnid
                    WHERE     (dbo.Table_010_SaleFactor.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "') AND (dbo.Table_010_SaleFactor.column02 <='" + faDatePickerStrip2.FADatePicker.Text + "')";

                    ExtraAdapter = new SqlDataAdapter(ExtraCommand, ConSale);
                    ExtraAdapter.Fill(DataSet1, "Extra");

                    DataRelation Main_Detail = new DataRelation("Main_Detail", DataSet1.Tables["Main"].Columns["SaleID"], DataSet1.Tables["Detail"].Columns["SaleID"], false);
                    ForeignKeyConstraint Fkc = new ForeignKeyConstraint("FK_Main_Detail", DataSet1.Tables["Main"].Columns["SaleID"], DataSet1.Tables["Detail"].Columns["SaleID"]);
                    Fkc.AcceptRejectRule = AcceptRejectRule.None;
                    Fkc.UpdateRule = Rule.Cascade;
                    Fkc.DeleteRule = Rule.None;
                    DataSet1.Tables["Detail"].Constraints.Add(Fkc);
                    DataSet1.Relations.Add(Main_Detail);


                    DataRelation Main_Extra = new DataRelation("Main_Extra", DataSet1.Tables["Main"].Columns["SaleID"], DataSet1.Tables["Extra"].Columns["SaleID"], false);
                    ForeignKeyConstraint Fkc_Extra = new ForeignKeyConstraint("FK_Main_Extra", DataSet1.Tables["Main"].Columns["SaleID"], DataSet1.Tables["Extra"].Columns["SaleID"]);
                    Fkc_Extra.AcceptRejectRule = AcceptRejectRule.None;
                    Fkc_Extra.UpdateRule = Rule.Cascade;
                    Fkc_Extra.DeleteRule = Rule.None;
                    DataSet1.Tables["Extra"].Constraints.Add(Fkc_Extra);
                    DataSet1.Relations.Add(Main_Extra);


                    gridEX1.DataSource = DataSet1.Tables["Main"];
                    gridEX_Good.DataSource = DataSet1.Tables["Main"];
                    gridEX_Good.DataMember = "Main_Detail";
                    gridEX_Extra.DataSource = DataSet1.Tables["Main"];
                    gridEX_Extra.DataMember = "Main_Extra";


               
            }
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
                    bt_Display_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void Form22_MergeReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Display_Click(sender, e);
        }

     

        
    }
}
