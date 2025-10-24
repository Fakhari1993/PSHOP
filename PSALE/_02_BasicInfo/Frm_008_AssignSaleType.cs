using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._02_BasicInfo
{
    public partial class Frm_008_AssignSaleType : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);

        public Frm_008_AssignSaleType()
        {
            InitializeComponent();
        }

        private void Form07_AssignGoodType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            
        }

        private void Form07_AssignGoodType_Load(object sender, EventArgs e)
        {
            try{
            if (Convert.ToInt32(Properties.Settings.Default.FactorPrice) == 1)
            {
                MessageBox.Show("نحوه محاسبه قیمت در فاکتور فروش را براساس نوع فروش انتخاب کنید");
                

            }
            DataTable dt = clDoc.ReturnTable(ConWare.ConnectionString, "Select [columnid],[column01],[column02] from table_004_CommodityAndIngredients where column28 =1  ");
            gridEX_Goods.DropDowns[0].DataSource = dt;
            gridEX_Goods.DropDowns[1].DataSource = dt;


            mlt_ACC.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_002_SalesTypes");
            gridEX_Goods.DropDowns["SaleType"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_002_SalesTypes");

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
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }

        }

        private void Form07_AssignGoodType_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Goods.RemoveFilters();
        }

        private void bt_Apply_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_ACC.Value != null && !string.IsNullOrWhiteSpace(mlt_ACC.Value.ToString()))
                    table_032_GoodPriceTableAdapter.FillBySaleType(dataSet_05_Awards.Table_032_GoodPrice, mlt_ACC.Text);
            }  
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

    

        private void gridEX_Goods_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try{
            gridEX_Goods.SetValue("Column04", Class_BasicOperation._UserName);
            gridEX_Goods.SetValue("Column05", Class_BasicOperation.ServerDate());
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridEX_Goods.UpdateData();
                gridEX_Goods.RemoveFilters();
                table_032_GoodPriceBindingSource.EndEdit();
                table_032_GoodPriceTableAdapter.Update(dataSet_05_Awards.Table_032_GoodPrice);
                table_032_GoodPriceTableAdapter.FillBySaleType(dataSet_05_Awards.Table_032_GoodPrice, mlt_ACC.Text);
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
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            try{
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bt_Apply_Click_1(object sender, EventArgs e)
        {
            try{
            if (mlt_ACC.Value != null && !string.IsNullOrWhiteSpace(mlt_ACC.Text))
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable gooddt = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT tcai.columnid AS GoodID
                                                            FROM   table_004_CommodityAndIngredients tcai
                                                            WHERE  tcai.column28 = 1
                                                                   AND tcai.column19 = 1 and tcai.columnid not in (select Column00 from Table_032_GoodPrice where Column01=N'" + mlt_ACC.Text + "')");
                gridEX_Goods.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                int p = 0;
                foreach (DataRow dr in gooddt.Rows)
                {
                    table_032_GoodPriceBindingSource.AddNew();
                    DataRowView HeaderRow = (DataRowView)this.table_032_GoodPriceBindingSource.CurrencyManager.Current;
                    HeaderRow["Column00"] = dr["GoodID"];
                    HeaderRow["Column01"] = mlt_ACC.Text;
                    HeaderRow["Column02"] = 0;
                    table_032_GoodPriceBindingSource.EndEdit();
                    if (p == 0)
                        p = table_032_GoodPriceBindingSource.Position;
                }
                gridEX_Goods.UpdateData();
                gridEX_Goods.MoveTo(p);

                gridEX_Goods.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
                this.Cursor = Cursors.Default;

            }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                Frm_034_ImportGoodsFromExcel frm = new Frm_034_ImportGoodsFromExcel();
                frm.ShowDialog();
                if (frm.dtGood.Rows.Count > 0)
                {
                    DataTable dt = frm.dtGood;

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (mlt_ACC.Value != null && !string.IsNullOrWhiteSpace(mlt_ACC.Text))
                            {
                                this.Cursor = Cursors.WaitCursor;

                                //                            DataTable gooddt = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT tcai.columnid AS GoodID
                                //                                                            FROM   table_004_CommodityAndIngredients tcai
                                //                                                            WHERE  tcai.column28 = 1
                                //                                                                   AND tcai.column19 = 1 and tcai.columnid not in (select Column00 from Table_032_GoodPrice where Column01=N'" + mlt_ACC.Text + "')");
                                //                            gridEX_Goods.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                                int p = 0;
                                //                            foreach (DataRow dr in gooddt.Rows)
                                //                            {
                                table_032_GoodPriceBindingSource.AddNew();
                                DataRowView HeaderRow = (DataRowView)this.table_032_GoodPriceBindingSource.CurrencyManager.Current;
                                //HeaderRow["Column00"] = clDoc.ExScalarQuery(ConWare.ConnectionString, @"select columnid from table_004_CommodityAndIngredients where column01=" + item[0].ToString());
                                HeaderRow["Column00"] = clDoc.ExScalarQuery(ConWare.ConnectionString, @"select columnid from table_004_CommodityAndIngredients where column01=N'" + item[0].ToString() + "'");
                                HeaderRow["Column01"] = mlt_ACC.Text;
                                HeaderRow["Column02"] = item[1].ToString();
                                table_032_GoodPriceBindingSource.EndEdit();
                                if (p == 0)
                                    p = table_032_GoodPriceBindingSource.Position;
                                // }
                                gridEX_Goods.UpdateData();
                                gridEX_Goods.MoveTo(p);

                                gridEX_Goods.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
                                this.Cursor = Cursors.Default;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Goods_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }
    }
}
