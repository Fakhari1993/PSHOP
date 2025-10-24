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
    public partial class Form01_GoodPrice : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        bool _BackSpace = false;

        public Form01_GoodPrice()
        {
            InitializeComponent();
        }

        private void Form01_GoodPrice_Load(object sender, EventArgs e)
        {



            DataTable dt = clDoc.ReturnTable(ConWare.ConnectionString, "Select [columnid],[column01],[column02] from table_004_CommodityAndIngredients where column28 =1  ");
            gridEX_Goods.DropDowns[0].DataSource = dt;
            gridEX_Goods.DropDowns[1].DataSource = dt;

            DataTable dt1 = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT tcai.columnid,
                                                                           tmg.column02 AS MainGroup,
                                                                           tsg.column03 AS SubGroup
                                                                    FROM   table_004_CommodityAndIngredients tcai
                                                                           JOIN table_003_SubsidiaryGroup tsg
                                                                                ON  tsg.column01 = tcai.column03
                                                                                AND tsg.columnid = tcai.column04
                                                                           JOIN table_002_MainGroup tmg
                                                                                ON  tmg.columnid = tsg.column01");
            gridEX_Goods.DropDowns[2].DataSource = dt1;
            gridEX_Goods.DropDowns[3].DataSource = dt1;


            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            this.table_70_GoodPriceTableAdapter.FillByDate(this.dataSet1.Table_70_GoodPrice, faDatePickerStrip1.FADatePicker.Text);

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
                //                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
                //                {
                //                    if (item.Cells["Column01"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["Column01"].Value.ToString())
                //                        && Convert.ToInt32(item.Cells["Column01"].Value) > 0)

                //                        if (Convert.ToInt32(item.Cells["ColumnId"].Value) == -1)
                //                        {
                //                            if (item.Cells["Column01"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["Column01"].Value.ToString())
                //                                 && Convert.ToInt32(item.Cells["Column01"].Value) > 0)
                //                                command += @" INSERT INTO  [dbo].[Table_70_GoodPrice]
                //                                                       ([Column00]
                //                                                       ,[Column01]
                //                                                       ,[Column02]
                //                                                       ,[Column03]
                //                                                       ,[Column04]
                //                                                       ,[Column05]
                //                                                       ,[Column06])
                //                                                 VALUES
                //                                                       (" + item.Cells["GoodCode"].Value + @"
                //                                                       ," + item.Cells["Column01"].Value + @"
                //                                                       ," + item.Cells["CurrentPrice"].Value + @"
                //                                                       ,'" + Class_BasicOperation._UserName + @"'
                //                                                       ,getdate()
                //                                                       ,'" + Class_BasicOperation._UserName + @"'
                //                                                       ,getdate())";
                //                        }
                //                        else
                //                        {
                //                            command += @" UPDATE Table_70_GoodPrice
                //                                                SET
                //	 
                //	                                                Column00 = " + item.Cells["GoodCode"].Value + @",
                //	                                                Column01 = " + item.Cells["Column01"].Value + @",
                //	                                                Column02 = " + item.Cells["Column02"].Value + @",
                //	                                                Column05 = '" + item.Cells["Column05"].Value + @"',
                //	                                                Column06 = CAST('" + item.Cells["Column06"].Value.ToString().Replace("ب.ظ", "PM").Replace("ق.ظ", "AM") + @"'  AS DATETIME)
                //                                                WHERE  ColumnId=" + item.Cells["ColumnId"].Value + "";
                //                        }
                //                }

                //                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                //                {
                //                    Con.Open();

                //                    SqlTransaction sqlTran = Con.BeginTransaction();
                //                    SqlCommand Command = Con.CreateCommand();
                //                    Command.Transaction = sqlTran;

                //                    try
                //                    {
                //                        Command.CommandText = command;
                //                        Command.ExecuteNonQuery();
                //                        sqlTran.Commit();
                //                        bt_Refresh_Click(null, null);
                //                        Class_BasicOperation.ShowMsg("", " ثبت با موفقیت انجام شد", "Information");
                //                        bt_Save.Enabled = true;


                //                    }
                //                    catch (Exception es)
                //                    {
                //                        sqlTran.Rollback();
                //                        this.Cursor = Cursors.Default;
                //                        bt_Save.Enabled = true;

                //                        Class_BasicOperation.CheckExceptionType(es, this.Name);
                //                    }
                //                }

                //            }
                //            catch (Exception es)
                //            {
                //                Class_BasicOperation.CheckExceptionType(es, this.Name);
                //                bt_Save.Enabled = true;

                //            }



                table_70_GoodPriceBindingSource.EndEdit();
                this.table_70_GoodPriceTableAdapter.Update(this.dataSet1.Table_70_GoodPrice);
                this.table_70_GoodPriceTableAdapter.FillByDate(this.dataSet1.Table_70_GoodPrice, faDatePickerStrip1.FADatePicker.Text);

                Class_BasicOperation.ShowMsg("", " ثبت با موفقیت انجام شد", "Information");

            }
            catch (SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, this.Name);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }

            this.Cursor = Cursors.Default;
            bt_Save.Enabled = true;



        }



        private void gridEX_Goods_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_Goods.SetValue("Column05", Class_BasicOperation._UserName);
            gridEX_Goods.SetValue("Column06", Class_BasicOperation.ServerDate());
            // if(e.Column.Key=="Column01")
            gridEX_Goods.SetValue("Column02", gridEX_Goods.GetValue("Column08"));
        }

        private void Form01_GoodPrice_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Goods.RemoveFilters();
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            this.table_70_GoodPriceTableAdapter.FillByDate(this.dataSet1.Table_70_GoodPrice, faDatePickerStrip1.FADatePicker.Text);

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

        private void Form01_GoodPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.R && e.Control)
                bt_Refresh_Click(sender, e);
        }


        private void faDatePickerStrip1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip1.FADatePicker.HideDropDown();
                    btn_Insert.Select();
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

        private void btn_Desply_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(faDatePickerStrip1.FADatePicker.Text))
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable gooddt = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT tcai.columnid AS GoodID,
                                                                   ISNULL(
                                                                       (
                                                                           SELECT TOP 1 tcbf.column10
                                                                           FROM   " + ConSale.Database + @".dbo.Table_015_BuyFactor tbf
                                                                                  JOIN " + ConSale.Database + @".dbo.Table_016_Child1_BuyFactor 
                                                                                       tcbf
                                                                                       ON  tcbf.column01 = tbf.columnid
                                                                           WHERE  tbf.column19 = 0
                                                                                  AND tcbf.column02 = tcai.columnid
                                                                                  AND tbf.column02<='" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                           ORDER BY
                                                                                  tbf.column02 DESC,
                                                                                  tbf.column06 DESC
                                                                       ),
                                                                       0
                                                                   ) AS Price
                                                            FROM   table_004_CommodityAndIngredients tcai
                                                            WHERE  tcai.column28 = 1
                                                                   AND tcai.column19 = 1");
                gridEX_Goods.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                int p = 0;
                foreach (DataRow dr in gooddt.Rows)
                {
                    table_70_GoodPriceBindingSource.AddNew();
                    DataRowView HeaderRow = (DataRowView)this.table_70_GoodPriceBindingSource.CurrencyManager.Current;
                    HeaderRow["Column00"] = dr["GoodID"];
                    HeaderRow["Column01"] = dr["Price"];
                    HeaderRow["Column02"] = dr["Price"];
                    HeaderRow["Column03"] = Class_BasicOperation._UserName;
                    HeaderRow["Column04"] = Class_BasicOperation.ServerDate();

                    HeaderRow["Column05"] = Class_BasicOperation._UserName;
                    HeaderRow["Column06"] = Class_BasicOperation.ServerDate();
                    HeaderRow["Column07"] = faDatePickerStrip1.FADatePicker.Text;
                    HeaderRow["Column08"] = dr["Price"];

                    table_70_GoodPriceBindingSource.EndEdit();
                    if (p == 0)
                        p = table_70_GoodPriceBindingSource.Position;

                }
                gridEX_Goods.UpdateData();
                gridEX_Goods.MoveTo(p);
                gridEX_Goods.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
                this.Cursor = Cursors.Default;

            }

        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (gridEX_Goods.GetCheckedRows().Count() == 0)
            {
                MessageBox.Show("کالایی انتخاب نشده است");
                return;
            }
            int i = 0;
            if (!string.IsNullOrWhiteSpace(txt_Search.Text) && Convert.ToDouble(txt_Search.Text) > Convert.ToDouble(0))
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetCheckedRows())
                {
                    item.BeginEdit();

                    item.Cells["Column05"].Value = Class_BasicOperation._UserName;
                    item.Cells["Column06"].Value = Class_BasicOperation.ServerDate();
                    item.Cells["Column01"].Value = txt_Search.Text;

                    item.EndEdit();
                }
            
            //this.table_70_GoodPriceTableAdapter.FillByDate(this.dataSet1.Table_70_GoodPrice, faDatePickerStrip1.FADatePicker.Text);
            //txt_Search.Text = string.Empty;
        }

        private void txt_Search_KeyDown(object sender, KeyEventArgs e)
        {


        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
                btn_Insert.Select();
        }

    }
}
