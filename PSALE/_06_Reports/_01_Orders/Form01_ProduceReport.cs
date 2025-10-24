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
    public partial class Form01_ProduceReport : Form
    {
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlDataAdapter Adapter;
        DataSet DS_MojodiRpt;
        BindingSource BindSource;
        bool _BackSpace = false;

        public Form01_ProduceReport()
        {
            InitializeComponent();
        }

        private void Frm_007_ProduceReport_Load(object sender, EventArgs e)
        {
            DS_MojodiRpt = new DataSet();

            Adapter = new SqlDataAdapter(
                "Select * From Table_001_PWHRS", ConWare);
            Adapter.Fill(DS_MojodiRpt, "Table_001_PWHRS");

            Adapter = new SqlDataAdapter(
                "Select * From table_002_MainGroup", ConWare);
            Adapter.Fill(DS_MojodiRpt, "table_002_MainGroup");

            Adapter = new SqlDataAdapter(
                "Select * From table_003_SubsidiaryGroup", ConWare);
            Adapter.Fill(DS_MojodiRpt, "table_003_SubsidiaryGroup");

            Adapter = new SqlDataAdapter(
                "Select * From table_004_CommodityAndIngredients", ConWare);
            Adapter.Fill(DS_MojodiRpt, "table_004_CommodityAndIngredients");

            cmb_Ware.DataSource = DS_MojodiRpt.Tables["Table_001_PWHRS"];
            cmb_Ware.DisplayMember = "Column02";
            cmb_Ware.ValueMember = "ColumnId";

            gridEX1.DropDowns[0].SetDataBinding(DS_MojodiRpt, "Table_001_PWHRS");
            gridEX1.DropDowns[1].SetDataBinding(DS_MojodiRpt, "table_002_MainGroup");
            gridEX1.DropDowns[2].SetDataBinding(DS_MojodiRpt, "table_003_SubsidiaryGroup");
            gridEX1.DropDowns[3].SetDataBinding(DS_MojodiRpt, "table_004_CommodityAndIngredients");
            gridEX1.DropDowns[4].SetDataBinding(DS_MojodiRpt, "table_004_CommodityAndIngredients");
            gridEX1.DropDowns[5].SetDataBinding(DS_MojodiRpt, "table_004_CommodityAndIngredients");


            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
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

        private void faDatePickerStrip2_TextChanged(object sender, EventArgs e)
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

        private void chk_AllGroups_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_AllGroups.Checked)
                cmb_Ware.Enabled = false;
            else cmb_Ware.Enabled = true;
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RowCount > 0)
                    DS_MojodiRpt.Tables["View_OrderMojodi"].Clear();

                if (faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                {

                    string Command;
                    if (chk_AllGroups.Checked)
                        Command = @"SELECT     TOP (100) PERCENT OrderNotExit.kala, SUM(ISNULL(View_Resid.residcartoon, 0)) AS residcartoon, SUM(ISNULL(View_Resid.residbaste, 0)) AS residbaste, 
        SUM(ISNULL(View_Resid.residjoz, 0)) AS residjoz, SUM(ISNULL(View_Resid.residkol, 0)) AS residkol, SUM(ISNULL(View_havaleh.havalehcartoon, 0)) 
        AS havalehcartoon, SUM(ISNULL(View_havaleh.havalehbaste, 0)) AS havalehbaste, SUM(ISNULL(View_havaleh.havalejoz, 0)) AS havalejoz, 
        SUM(ISNULL(View_havaleh.havalehkol, 0)) AS havalehko, ISNULL(SUM(View_Resid.residcartoon), 0) - ISNULL(SUM(View_havaleh.havalehcartoon), 0) 
        AS mojodicartoon, ISNULL(SUM(View_Resid.residbaste), 0) - ISNULL(SUM(View_havaleh.havalehbaste), 0) AS mojodibaste, ISNULL(SUM(View_Resid.residjoz), 0) 
        - ISNULL(SUM(View_havaleh.havalejoz), 0) AS mojodijoz, ISNULL(SUM(View_Resid.residkol), 0) - ISNULL(SUM(View_havaleh.havalehkol), 0) AS mojodikol, 
        OrderNotExit.ordercartoon, OrderNotExit.orderbaste, OrderNotExit.orderjoz, OrderNotExit.orderkol, ISNULL(InErsal.ordercartoon, 0) AS inersalcartoon, 
        ISNULL(InErsal.orderbaste, 0) AS inersalbaste, ISNULL(InErsal.orderjoz, 0) AS inersaljoz, ISNULL(InErsal.orderkol, 0) AS inersalkol, ISNULL(MaliOk.ordercartoon, 0) 
        AS malicartoon, ISNULL(MaliOk.orderbaste, 0) AS malibaste, ISNULL(MaliOk.orderjoz, 0) AS malijoz, ISNULL(MaliOk.orderkol, 0) AS malikol, 
          (ISNULL(SUM(View_Resid.residcartoon), 0) - ISNULL(SUM(View_havaleh.havalehcartoon), 0)) -OrderNotExit.ordercartoon  AS kasricartoon, 
        OrderNotExit.orderbaste - (ISNULL(SUM(View_Resid.residbaste), 0) - ISNULL(SUM(View_havaleh.havalehbaste), 0)) AS kasribaste, 
        OrderNotExit.orderjoz - (ISNULL(SUM(View_Resid.residjoz), 0) - ISNULL(SUM(View_havaleh.havalejoz), 0)) AS kasrijoz, 
        OrderNotExit.orderkol - (ISNULL(SUM(View_Resid.residkol), 0) - ISNULL(SUM(View_havaleh.havalehkol), 0)) AS kasrikol, ISNULL(MaliOk.ordercartoon, 0) 
        - ISNULL(InErsal.ordercartoon, 0) AS mandehcartoon, ISNULL(MaliOk.orderbaste, 0) - ISNULL(InErsal.orderbaste, 0) AS mandehbaste, ISNULL(MaliOk.orderjoz, 0) 
        - ISNULL(InErsal.orderjoz, 0) AS mandehjoz, ISNULL(MaliOk.orderkol, 0) - ISNULL(InErsal.orderkol, 0) AS mandehkol, 
        dbo.table_004_CommodityAndIngredients.column03 AS MainGroup, dbo.table_004_CommodityAndIngredients.column04 AS SubGroup
        FROM         (SELECT     {1}.dbo.Table_006_OrderDetails.column02 AS kala, SUM({1}.dbo.Table_006_OrderDetails.column04) AS ordercartoon, 
        SUM({1}.dbo.Table_006_OrderDetails.column03) AS orderbaste, SUM({1}.dbo.Table_006_OrderDetails.column05) AS orderjoz, 
        SUM({1}.dbo.Table_006_OrderDetails.column06) AS orderkol
        FROM         {1}.dbo.Table_005_OrderHeader INNER JOIN
        {1}.dbo.Table_006_OrderDetails ON 
        {1}.dbo.Table_005_OrderHeader.columnid = {1}.dbo.Table_006_OrderDetails.column01
        WHERE     ({1}.dbo.Table_005_OrderHeader.column02 <= N'{0}') AND ({1}.dbo.Table_005_OrderHeader.column33 = 0) AND 
        ({1}.dbo.Table_005_OrderHeader.column13 = 0)
        GROUP BY {1}.dbo.Table_006_OrderDetails.column02) AS OrderNotExit INNER JOIN
        dbo.table_004_CommodityAndIngredients ON OrderNotExit.kala = dbo.table_004_CommodityAndIngredients.columnid LEFT OUTER JOIN
        (SELECT     Table_006_OrderDetails_2.column02 AS kala, SUM(Table_006_OrderDetails_2.column04) AS ordercartoon, SUM(Table_006_OrderDetails_2.column03) 
        AS orderbaste, SUM(Table_006_OrderDetails_2.column05) AS orderjoz, SUM(Table_006_OrderDetails_2.column06) AS orderkol
        FROM         {1}.dbo.Table_005_OrderHeader AS Table_005_OrderHeader_2 INNER JOIN
        {1}.dbo.Table_006_OrderDetails AS Table_006_OrderDetails_2 ON 
        Table_005_OrderHeader_2.columnid = Table_006_OrderDetails_2.column01
        WHERE     (Table_005_OrderHeader_2.column02 <= N'{0}') AND (Table_005_OrderHeader_2.column33 = 0) AND (Table_005_OrderHeader_2.column13 = 0) 
        AND (Table_005_OrderHeader_2.column18 = 1)
        GROUP BY Table_006_OrderDetails_2.column02) AS MaliOk ON OrderNotExit.kala = MaliOk.kala LEFT OUTER JOIN
        (SELECT     Table_006_OrderDetails_1.column02 AS kala, SUM(Table_006_OrderDetails_1.column04) AS ordercartoon, SUM(Table_006_OrderDetails_1.column03) 
        AS orderbaste, SUM(Table_006_OrderDetails_1.column05) AS orderjoz, SUM(Table_006_OrderDetails_1.column06) AS orderkol
        FROM         {1}.dbo.Table_005_OrderHeader AS Table_005_OrderHeader_1 INNER JOIN
        {1}.dbo.Table_006_OrderDetails AS Table_006_OrderDetails_1 ON 
        Table_005_OrderHeader_1.columnid = Table_006_OrderDetails_1.column01
        WHERE     (Table_005_OrderHeader_1.column02 <= N'{0}') AND (Table_005_OrderHeader_1.column33 = 0) AND (Table_005_OrderHeader_1.column13 = 0) 
        AND (Table_005_OrderHeader_1.column22 = 1)
        GROUP BY Table_006_OrderDetails_1.column02) AS InErsal ON OrderNotExit.kala = InErsal.kala LEFT OUTER JOIN
        (SELECT     dbo.Table_011_PwhrsReceipt.column03 AS Anbar, dbo.Table_012_Child_PwhrsReceipt.column02 AS Kala, 
        SUM(dbo.Table_012_Child_PwhrsReceipt.column04) AS residcartoon, SUM(dbo.Table_012_Child_PwhrsReceipt.column05) AS residbaste, 
        SUM(dbo.Table_012_Child_PwhrsReceipt.column06) AS residjoz, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS residkol
        FROM         dbo.Table_011_PwhrsReceipt INNER JOIN
        dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
        WHERE     (dbo.Table_011_PwhrsReceipt.column02 <= N'{0}')
        GROUP BY dbo.Table_011_PwhrsReceipt.column03, dbo.Table_012_Child_PwhrsReceipt.column02) AS View_Resid ON 
        OrderNotExit.kala = View_Resid.Kala LEFT OUTER JOIN
        (SELECT     dbo.Table_007_PwhrsDraft.column03 AS Anbar, dbo.Table_008_Child_PwhrsDraft.column02 AS Kala, SUM(dbo.Table_008_Child_PwhrsDraft.column04) 
        AS havalehcartoon, SUM(dbo.Table_008_Child_PwhrsDraft.column05) AS havalehbaste, SUM(dbo.Table_008_Child_PwhrsDraft.column06) AS havalejoz, 
        SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS havalehkol
        FROM         dbo.Table_007_PwhrsDraft INNER JOIN
        dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
        WHERE     (dbo.Table_007_PwhrsDraft.column02 <= N'{0}')
        GROUP BY dbo.Table_007_PwhrsDraft.column03, dbo.Table_008_Child_PwhrsDraft.column02) AS View_havaleh ON View_Resid.Anbar = View_havaleh.Anbar AND 
        View_Resid.Kala = View_havaleh.Kala
        GROUP BY OrderNotExit.kala, OrderNotExit.ordercartoon, OrderNotExit.orderbaste, OrderNotExit.orderjoz, OrderNotExit.orderkol, ISNULL(InErsal.ordercartoon, 0), 
        ISNULL(InErsal.orderbaste, 0), ISNULL(InErsal.orderjoz, 0), ISNULL(InErsal.orderkol, 0), ISNULL(MaliOk.ordercartoon, 0), ISNULL(MaliOk.orderbaste, 0), 
        ISNULL(MaliOk.orderjoz, 0), ISNULL(MaliOk.orderkol, 0), ISNULL(MaliOk.ordercartoon, 0) - ISNULL(InErsal.ordercartoon, 0), ISNULL(MaliOk.orderbaste, 0) 
        - ISNULL(InErsal.orderbaste, 0), ISNULL(MaliOk.orderjoz, 0) - ISNULL(InErsal.orderjoz, 0), ISNULL(MaliOk.orderkol, 0) - ISNULL(InErsal.orderkol, 0), 
        dbo.table_004_CommodityAndIngredients.column03, dbo.table_004_CommodityAndIngredients.column04
        ORDER BY OrderNotExit.kala";


                    else
                        Command = @"SELECT     TOP (100) PERCENT View_Resid.Anbar, OrderNotExit.kala, ISNULL(View_Resid.residcartoon, 0) AS residcartoon, ISNULL(View_Resid.residbaste, 0) AS residbaste, 
                      ISNULL(View_Resid.residjoz, 0) AS residjoz, ISNULL(View_Resid.residkol, 0) AS residkol, ISNULL(View_havaleh.havalehcartoon, 0) AS havalehcartoon, 
                      ISNULL(View_havaleh.havalehbaste, 0) AS havalehbaste, ISNULL(View_havaleh.havalejoz, 0) AS havalejoz, ISNULL(View_havaleh.havalehkol, 0) AS havalehko, 
                      ISNULL(View_Resid.residcartoon, 0) - ISNULL(View_havaleh.havalehcartoon, 0) AS mojodicartoon, ISNULL(View_Resid.residbaste, 0) 
                      - ISNULL(View_havaleh.havalehbaste, 0) AS mojodibaste, ISNULL(View_Resid.residjoz, 0) - ISNULL(View_havaleh.havalejoz, 0) AS mojodijoz, 
                      ISNULL(View_Resid.residkol, 0) - ISNULL(View_havaleh.havalehkol, 0) AS mojodikol, OrderNotExit.ordercartoon, OrderNotExit.orderbaste, OrderNotExit.orderjoz, 
                      OrderNotExit.orderkol, ISNULL(InErsal.ordercartoon, 0) AS inersalcartoon, ISNULL(InErsal.orderbaste, 0) AS inersalbaste, ISNULL(InErsal.orderjoz, 0) AS inersaljoz, 
                      ISNULL(InErsal.orderkol, 0) AS inersalkol, ISNULL(MaliOk.ordercartoon, 0) AS malicartoon, ISNULL(MaliOk.orderbaste, 0) AS malibaste, ISNULL(MaliOk.orderjoz, 0) 
                      AS malijoz, ISNULL(MaliOk.orderkol, 0) AS malikol,   (ISNULL(View_Resid.residcartoon, 0) - ISNULL(View_havaleh.havalehcartoon, 0)) -OrderNotExit.ordercartoon
                      AS kasricartoon, OrderNotExit.orderbaste - (ISNULL(View_Resid.residbaste, 0) - ISNULL(View_havaleh.havalehbaste, 0)) AS kasribaste, 
                      OrderNotExit.orderjoz - (ISNULL(View_Resid.residjoz, 0) - ISNULL(View_havaleh.havalejoz, 0)) AS kasrijoz, OrderNotExit.orderkol - (ISNULL(View_Resid.residkol, 0) 
                      - ISNULL(View_havaleh.havalehkol, 0)) AS kasrikol, ISNULL(MaliOk.ordercartoon, 0) - ISNULL(InErsal.ordercartoon, 0) AS mandehcartoon, ISNULL(MaliOk.orderbaste, 
                      0) - ISNULL(InErsal.orderbaste, 0) AS mandehbaste, ISNULL(MaliOk.orderjoz, 0) - ISNULL(InErsal.orderjoz, 0) AS mandehjoz, ISNULL(MaliOk.orderkol, 0) 
                      - ISNULL(InErsal.orderkol, 0) AS mandehkol, dbo.table_004_CommodityAndIngredients.column03 AS MainGroup, 
                      dbo.table_004_CommodityAndIngredients.column04 AS SubGroup
FROM         (SELECT     {1}.dbo.Table_006_OrderDetails.column02 AS kala, SUM({1}.dbo.Table_006_OrderDetails.column04) AS ordercartoon, 
                                              SUM({1}.dbo.Table_006_OrderDetails.column03) AS orderbaste, SUM({1}.dbo.Table_006_OrderDetails.column05) AS orderjoz, 
                                              SUM({1}.dbo.Table_006_OrderDetails.column06) AS orderkol
                        FROM         {1}.dbo.Table_005_OrderHeader INNER JOIN
                                              {1}.dbo.Table_006_OrderDetails ON 
                                              {1}.dbo.Table_005_OrderHeader.columnid = {1}.dbo.Table_006_OrderDetails.column01
                        WHERE     ({1}.dbo.Table_005_OrderHeader.column02 <= N'{0}') AND ({1}.dbo.Table_005_OrderHeader.column33 = 0) AND 
                                              ({1}.dbo.Table_005_OrderHeader.column13 = 0)
                        GROUP BY {1}.dbo.Table_006_OrderDetails.column02) AS OrderNotExit INNER JOIN
                      dbo.table_004_CommodityAndIngredients ON OrderNotExit.kala = dbo.table_004_CommodityAndIngredients.columnid LEFT OUTER JOIN
                          (SELECT     Table_006_OrderDetails_2.column02 AS kala, SUM(Table_006_OrderDetails_2.column04) AS ordercartoon, SUM(Table_006_OrderDetails_2.column03) 
                                                   AS orderbaste, SUM(Table_006_OrderDetails_2.column05) AS orderjoz, SUM(Table_006_OrderDetails_2.column06) AS orderkol
                             FROM         {1}.dbo.Table_005_OrderHeader AS Table_005_OrderHeader_2 INNER JOIN
                                                   {1}.dbo.Table_006_OrderDetails AS Table_006_OrderDetails_2 ON 
                                                   Table_005_OrderHeader_2.columnid = Table_006_OrderDetails_2.column01
                             WHERE     (Table_005_OrderHeader_2.column02 <= N'{0}') AND (Table_005_OrderHeader_2.column33 = 0) AND (Table_005_OrderHeader_2.column13 = 0) 
                                                   AND (Table_005_OrderHeader_2.column18 = 1)
                             GROUP BY Table_006_OrderDetails_2.column02) AS MaliOk ON OrderNotExit.kala = MaliOk.kala LEFT OUTER JOIN
                          (SELECT     Table_006_OrderDetails_1.column02 AS kala, SUM(Table_006_OrderDetails_1.column04) AS ordercartoon, SUM(Table_006_OrderDetails_1.column03) 
                                                   AS orderbaste, SUM(Table_006_OrderDetails_1.column05) AS orderjoz, SUM(Table_006_OrderDetails_1.column06) AS orderkol
                             FROM         {1}.dbo.Table_005_OrderHeader AS Table_005_OrderHeader_1 INNER JOIN
                                                   {1}.dbo.Table_006_OrderDetails AS Table_006_OrderDetails_1 ON 
                                                   Table_005_OrderHeader_1.columnid = Table_006_OrderDetails_1.column01
                             WHERE     (Table_005_OrderHeader_1.column02 <= N'{0}') AND (Table_005_OrderHeader_1.column33 = 0) AND (Table_005_OrderHeader_1.column13 = 0) 
                                                   AND (Table_005_OrderHeader_1.column22 = 1)
                             GROUP BY Table_006_OrderDetails_1.column02) AS InErsal ON OrderNotExit.kala = InErsal.kala LEFT OUTER JOIN
                          (SELECT     dbo.Table_011_PwhrsReceipt.column03 AS Anbar, dbo.Table_012_Child_PwhrsReceipt.column02 AS Kala, 
                                                   SUM(dbo.Table_012_Child_PwhrsReceipt.column04) AS residcartoon, SUM(dbo.Table_012_Child_PwhrsReceipt.column05) AS residbaste, 
                                                   SUM(dbo.Table_012_Child_PwhrsReceipt.column06) AS residjoz, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS residkol
                             FROM         dbo.Table_011_PwhrsReceipt INNER JOIN
                                                   dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                             WHERE     (dbo.Table_011_PwhrsReceipt.column02 <= N'{0}')
                             GROUP BY dbo.Table_011_PwhrsReceipt.column03, dbo.Table_012_Child_PwhrsReceipt.column02) AS View_Resid ON 
                      OrderNotExit.kala = View_Resid.Kala LEFT OUTER JOIN
                          (SELECT     dbo.Table_007_PwhrsDraft.column03 AS Anbar, dbo.Table_008_Child_PwhrsDraft.column02 AS Kala, SUM(dbo.Table_008_Child_PwhrsDraft.column04) 
                                                   AS havalehcartoon, SUM(dbo.Table_008_Child_PwhrsDraft.column05) AS havalehbaste, SUM(dbo.Table_008_Child_PwhrsDraft.column06) AS havalejoz, 
                                                   SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS havalehkol
                             FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                                   dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                             WHERE     (dbo.Table_007_PwhrsDraft.column02 <= N'{0}')
                             GROUP BY dbo.Table_007_PwhrsDraft.column03, dbo.Table_008_Child_PwhrsDraft.column02) AS View_havaleh ON View_Resid.Anbar = View_havaleh.Anbar AND 
                      View_Resid.Kala = View_havaleh.Kala
WHERE     (View_Resid.Anbar = " + cmb_Ware.SelectedValue.ToString()+@")
ORDER BY OrderNotExit.kala";

                    Command = string.Format(Command, faDatePickerStrip2.FADatePicker.Text,
                        ConSale.Database);
                    Adapter = new SqlDataAdapter(Command, ConWare);
                    Adapter.Fill(DS_MojodiRpt, "View_OrderMojodi");

                    BindSource = new BindingSource();
                    BindSource.DataSource = DS_MojodiRpt.Tables["View_OrderMojodi"];
                    gridEX1.DataSource = BindSource;


                    gridEX1.RootTable.Caption = "موجودی در تاریخ : " + 
                        faDatePickerStrip2.FADatePicker.Text +
                        (chk_AllGroups.Checked ? " بر اساس تمام انبارها" : " بر اساس " +
                        cmb_Ware.Text);

                    DS_MojodiRpt.EnforceConstraints = true;
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void Frm_007_ProduceReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip2.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Search_Click(sender, e);
        }

        private void Frm_007_ProduceReport_FormClosing(object sender, FormClosingEventArgs e)
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

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            //gridEXPrintDocument1.PageHeaderCenter += faDatePickerStrip2.FADatePicker.Text;
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                printPreviewDialog1.Show();
        }


    }
}
