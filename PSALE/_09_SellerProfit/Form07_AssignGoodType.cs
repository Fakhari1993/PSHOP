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
    public partial class Form07_AssignGoodType : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        public Form07_AssignGoodType()
        {
            InitializeComponent();
        }

        private void Form07_AssignGoodType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.R && e.Control)
                bt_Refresh_Click(sender, e);
        }

        private void Form07_AssignGoodType_Load(object sender, EventArgs e)
        {


            DataTable gooddt = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT ISNULL(tagt.ColumnId, -1) as ColumnId,
                                                                                           tcai.columnid AS Column00,
                                                                                           tagt.Column01,
                                                                                           tagt.Column02,
                                                                                           tagt.Column03,
                                                                                           tagt.Column04,
                                                                                           tagt.Column05
                                                                                    FROM   " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                                           LEFT JOIN Table_76_AssignGoodType tagt
                                                                                                ON  tcai.columnid = tagt.Column00
                                                                                    WHERE  tcai.column19 = 1
                                                                                           AND tcai.column28 = 1");

            gridEX_Goods.DataSource = gooddt;
            DataTable dt = clDoc.ReturnTable(ConWare.ConnectionString, "Select [columnid],[column01],[column02] from table_004_CommodityAndIngredients where column28 =1  ");
            gridEX_Goods.DropDowns[0].DataSource = dt;
            gridEX_Goods.DropDowns[1].DataSource = dt;
            DataTable dst = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_75_SaleType  ");
            gridEX_Goods.DropDowns[2].DataSource = dst;
            mlt_ACC.DataSource = dst;

            DataTable dst1 = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT tcai.columnid,
                                                                       tmg.column02 AS MainGroup,
                                                                       tsg.column03 AS SubGroup
                                                                FROM   table_004_CommodityAndIngredients tcai
                                                                       JOIN table_003_SubsidiaryGroup tsg
                                                                            ON  tsg.column01 = tcai.column03
                                                                            AND tsg.columnid = tcai.column04
                                                                       JOIN table_002_MainGroup tmg
                                                                            ON  tmg.columnid = tsg.column01");
            gridEX_Goods.DropDowns[3].DataSource = dst1;
            gridEX_Goods.DropDowns[4].DataSource = dst1;


        }

        private void Form07_AssignGoodType_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Goods.RemoveFilters();
        }

        private void bt_Apply_Click(object sender, EventArgs e)
        {
            if (gridEX_Goods.GetCheckedRows().Length > 0 && mlt_ACC.Text.Trim() != "")
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetCheckedRows())
                {
                    item.BeginEdit();
                    item.Cells["Column01"].Value = mlt_ACC.Value;
                    item.EndEdit();
                }
            }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            DataTable gooddt = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT ISNULL(tagt.ColumnId, -1) as ColumnId,
                                                                                           tcai.columnid AS Column00,
                                                                                           tagt.Column01,
                                                                                           tagt.Column02,
                                                                                           tagt.Column03,
                                                                                           tagt.Column04,
                                                                                           tagt.Column05
                                                                                    FROM   " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                                           LEFT JOIN Table_76_AssignGoodType tagt
                                                                                                ON  tcai.columnid = tagt.Column00
                                                                                    WHERE  tcai.column19 = 1
                                                                                           AND tcai.column28 = 1");
            gridEX_Goods.DataSource = gooddt;
        }

        private void gridEX_Goods_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_Goods.SetValue("Column04", Class_BasicOperation._UserName);
            gridEX_Goods.SetValue("Column05", Class_BasicOperation.ServerDate());
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridEX_Goods.RemoveFilters();
                gridEX_Goods.UpdateData();
                string command = string.Empty;
                bt_Save.Enabled = false;
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetCheckedRows())
                {
                    if (item.Cells["Column01"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["Column01"].Value.ToString()))
                    {
                        if (Convert.ToInt32(item.Cells["ColumnId"].Value) == -1)
                        {
                            command += @" INSERT INTO  [dbo].[Table_76_AssignGoodType]
                                                       ([Column00]
                                                       ,[Column01]
                                                       ,[Column02]
                                                       ,[Column03]
                                                       ,[Column04]
                                                       ,[Column05])
                                                 VALUES
                                                       (" + item.Cells["GoodCode"].Value + @"
                                                       ," + item.Cells["Column01"].Value + @"
                                                       ,'" + Class_BasicOperation._UserName + @"'
                                                       ,getdate()
                                                       ,'" + Class_BasicOperation._UserName + @"'
                                                       ,getdate())";
                        }
                        else
                        {
                            command += @" UPDATE Table_76_AssignGoodType
                                                SET
	                                                Column00 = " + item.Cells["GoodCode"].Value + @",
	                                                Column01 = " + item.Cells["Column01"].Value + @",
	                                                Column04 = '" + item.Cells["Column04"].Value + @"',
	                                                Column05 = CAST('" + item.Cells["Column05"].Value.ToString().Replace("ب.ظ", "PM").Replace("ق.ظ", "AM") + @"'  AS DATETIME)
                                                WHERE  ColumnId=" + item.Cells["ColumnId"].Value + "";
                        }
                    }
                }

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                {
                    Con.Open();

                    SqlTransaction sqlTran = Con.BeginTransaction();
                    SqlCommand Command = Con.CreateCommand();
                    Command.Transaction = sqlTran;

                    try
                    {
                        Command.CommandText = command;
                        Command.ExecuteNonQuery();
                        sqlTran.Commit();
                        bt_Refresh_Click(null, null);
                        Class_BasicOperation.ShowMsg("", " ثبت با موفقیت انجام شد", "Information");
                        bt_Save.Enabled = true;


                    }
                    catch (Exception es)
                    {
                        sqlTran.Rollback();
                        this.Cursor = Cursors.Default;
                        bt_Save.Enabled = true;

                        Class_BasicOperation.CheckExceptionType(es, this.Name);
                    }
                }

            }
            catch (Exception es)
            {
                Class_BasicOperation.CheckExceptionType(es, this.Name);
                bt_Save.Enabled = true;

            }





            this.Cursor = Cursors.Default;
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
    }
}
