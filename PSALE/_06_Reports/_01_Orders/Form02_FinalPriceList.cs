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
    public partial class Form02_FinalPriceList : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        
        public Form02_FinalPriceList()
        {
            InitializeComponent();
        }

        private void Form02_FinalPriceList_Load(object sender, EventArgs e)
        {
            cmb_Groups.DataSource = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from table_002_MainGroup");
            cmb_Groups.DisplayMember = "Column02";
            cmb_Groups.ValueMember = "ColumnId";

            txt_Date.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00");
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     GoodsInformation.id AS GoodID, GoodsInformation.Code AS GoodCode, GoodsInformation.name AS GoodName, GoodsInformation.goroohasli AS MainGroupID, 
                      GoodsInformation.CountUnit, GoodsInformation.goroohfari AS SubGroupID, ISNULL(GoodsInformation.NumberInBox, 0) AS NumberInBox, 
                      ISNULL(GoodsInformation.NumberInPack, 0) AS NumberInPack, ISNULL(GoodsInformation.Tavan, 0) AS Tavan, ISNULL(GoodsInformation.Hajm, 0) AS Hajm, 
                      GoodsInformation.Active1, ISNULL(GoodsInformation.BuyPrice, 0) AS BuyPrice, ISNULL(GoodsInformation.SalePrice, 0) AS SalePrice, 
                      ISNULL(GoodsInformation.SalePackPrice, 0) AS SalePackPrice, ISNULL(GoodsInformation.SaleBoxPrice, 0) AS SaleBoxPrice, ISNULL(GoodsInformation.UsePrice, 0) 
                      AS UserPrice, ISNULL(GoodsInformation.Discount, 0) AS Discount, ISNULL(GoodsInformation.Extra, 0) AS Extra, GoodsInformation.Active2, 
                      {0}.dbo.table_003_SubsidiaryGroup.column03 AS SubGroupName, {0}.dbo.table_002_MainGroup.column02 AS MainGroupName
FROM         (SELECT     {0}.dbo.table_004_CommodityAndIngredients.columnid AS id, {0}.dbo.table_004_CommodityAndIngredients.column01 AS Code, 
                                              {0}.dbo.table_004_CommodityAndIngredients.column02 AS name, 
                                              {0}.dbo.table_004_CommodityAndIngredients.column03 AS goroohasli, 
                                              {0}.dbo.table_004_CommodityAndIngredients.column04 AS goroohfari, 
                                              {0}.dbo.table_004_CommodityAndIngredients.column07 AS CountUnit, 
                                              CASE WHEN {0}.dbo.table_006_CommodityChanges.Column07 IS NULL 
                                              THEN {0}.dbo.table_004_CommodityAndIngredients.column09 ELSE {0}.dbo.table_006_CommodityChanges.Column07 END AS NumberInBox,
                                               CASE WHEN {0}.dbo.table_006_CommodityChanges.Column06 IS NULL 
                                              THEN {0}.dbo.table_004_CommodityAndIngredients.column08 ELSE {0}.dbo.table_006_CommodityChanges.Column06 END AS NumberInPack,
                                               CASE WHEN table_006_CommodityChanges.Column12 IS NULL 
                                              THEN {0}.dbo.table_004_CommodityAndIngredients.column24 ELSE table_006_CommodityChanges.Column12 END AS Tavan, 
                                              CASE WHEN table_006_CommodityChanges.Column13 IS NULL 
                                              THEN {0}.dbo.table_004_CommodityAndIngredients.column25 ELSE table_006_CommodityChanges.Column13 END AS Hajm, 
                                              ISNULL({0}.dbo.table_006_CommodityChanges.Column18, 1) AS Active1, CASE WHEN TS003.Column03 IS NULL 
                                              THEN {0}.dbo.table_004_CommodityAndIngredients.Column35 ELSE TS003.Column03 END AS BuyPrice, CASE WHEN TS003.Column07 IS NULL 
                                              THEN {0}.dbo.table_004_CommodityAndIngredients.Column34 ELSE TS003.Column07 END AS SalePrice, CASE WHEN TS003.Column09 IS NULL 
                                              THEN {0}.dbo.table_004_CommodityAndIngredients.Column39 ELSE ts003.Column09 END AS SalePackPrice, CASE WHEN Ts003.Column10 IS NULL 
                                              THEN {0}.dbo.table_004_CommodityAndIngredients.Column40 ELSE ts003.column10 END AS SaleBoxPrice, CASE WHEN Ts003.Column04 IS NULL 
                                              THEN {0}.dbo.table_004_CommodityAndIngredients.Column36 ELSE ts003.column04 END AS UsePrice, CASE WHEN Ts003.Column05 IS NULL 
                                              THEN {0}.dbo.table_004_CommodityAndIngredients.Column37 ELSE ts003.column05 END AS Discount, CASE WHEN Ts003.Column06 IS NULL 
                                              THEN {0}.dbo.table_004_CommodityAndIngredients.Column38 ELSE ts003.column06 END AS Extra, ISNULL(TS003.Column11, 1) AS Active2, 
                                              {0}.dbo.table_004_CommodityAndIngredients.column28 AS Active3
                        FROM         {0}.dbo.table_004_CommodityAndIngredients LEFT OUTER JOIN
                                                 
                                                  (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, Column11
                                                     FROM         {0}.dbo.Table_003_InformationProductCash) AS TS003 ON 
                                              {0}.dbo.table_004_CommodityAndIngredients.columnid = TS003.column01 LEFT OUTER JOIN
                                              {0}.dbo.table_006_CommodityChanges ON 
                                              {0}.dbo.table_004_CommodityAndIngredients.columnid = {0}.dbo.table_006_CommodityChanges.column01
                        WHERE     ({0}.dbo.table_004_CommodityAndIngredients.column19 = 1)) AS GoodsInformation INNER JOIN
                      {0}.dbo.table_003_SubsidiaryGroup ON GoodsInformation.goroohfari = {0}.dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
                      {0}.dbo.table_002_MainGroup ON 
                      {0}.dbo.table_003_SubsidiaryGroup.column01 = {0}.dbo.table_002_MainGroup.columnid
WHERE     (GoodsInformation.Active1 = 1) AND (GoodsInformation.Active2 = 1) AND (GoodsInformation.Active3 = 1)", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, ConWare.Database);

                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                BindingSource BindSource = new BindingSource();
                BindSource.DataSource = Table;
                gridEX1.DataSource = BindSource;
                if (chk_AllGroups.Checked)
                    BindSource.RemoveFilter();
                else BindSource.Filter = "MainGroupID=" + cmb_Groups.SelectedValue.ToString();

           

        }

        private void chk_AllGroups_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_AllGroups.Checked)
                cmb_Groups.Enabled = false;
            else
                cmb_Groups.Enabled = true;
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            DataTable Table = dataSet_Report.Rpt_FinalLastPrice.Clone();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                Table.Rows.Add(txt_Date.Text, item.Cells["MainGroupName"].Text, item.Cells["GoodCode"].Text,
                    item.Cells["GoodName"].Text, item.Cells["SalePrice"].Value.ToString(), item.Cells["BuyPrice"].Value.ToString(),
                    item.Cells["UsePrice"].Value.ToString(), item.Cells["Discount"].Value.ToString(), item.Cells["Extra"].Value.ToString(),
                    item.Cells["SaleBoxPrice"].Value.ToString(), item.Cells["SalePackPrice"].Value.ToString(),
                    item.Cells["NumberInBox"].Value.ToString(), item.Cells["NumberInPack"].Value.ToString());
            }

            if (Table.Rows.Count > 0)
            { 
                _06_Reports._01_Orders.Form02_FinalPrice_Print frm = new Form02_FinalPrice_Print(Table, " "," ");
                frm.ShowDialog();
            }
        }

        private void Form02_FinalPriceList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }

        private void Form02_FinalPriceList_FormClosing(object sender, FormClosingEventArgs e)
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
