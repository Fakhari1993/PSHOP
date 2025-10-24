using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._05_Sale
{
    public partial class Frm_040_SaleFactorGroupConfirm : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        public Frm_040_SaleFactorGroupConfirm()
        {
            InitializeComponent();
        }




        private void Frm_040_SaleFactorGroupConfirm_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_RDetail.RemoveFilters();
            gridEX_RHeader.RemoveFilters();
        }




        private void Frm_040_SaleFactorGroupConfirm_Load(object sender, EventArgs e)
        {
            DataSet DS = new DataSet();
            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * from Table_007_PwhrsDraft", ConWare);
            Adapter.Fill(DS, "Draft");
            gridEX_RHeader.DropDowns["Draft"].SetDataBinding(DS.Tables["Draft"], "");


            Adapter = new SqlDataAdapter("SELECT * from Table_045_PersonInfo", ConBase);
            Adapter.Fill(DS, "Person");
            gridEX_RHeader.DropDowns["Person"].SetDataBinding(DS.Tables["Person"], "");

            Adapter = new SqlDataAdapter("SELECT * from Table_007_FactorBefore", ConSale);
            Adapter.Fill(DS, "PreFactor");
            gridEX_RHeader.DropDowns["PreFactor"].SetDataBinding(DS.Tables["PreFactor"], "");

            Adapter = new SqlDataAdapter("SELECT * from Table_005_OrderHeader", ConSale);
            Adapter.Fill(DS, "Order");
            gridEX_RHeader.DropDowns["Order"].SetDataBinding(DS.Tables["Order"], "");

            Adapter = new SqlDataAdapter("SELECT * from Table_060_SanadHead", ConAcnt);
            Adapter.Fill(DS, "Sanad");
            gridEX_RHeader.DropDowns["Sanad"].SetDataBinding(DS.Tables["Sanad"], "");


            Adapter = new SqlDataAdapter("SELECT * from table_005_PwhrsOperation", ConWare);
            Adapter.Fill(DS, "Func");
            gridEX_RHeader.DropDowns["Func"].SetDataBinding(DS.Tables["Func"], "");


            Adapter = new SqlDataAdapter("SELECT * from Table_001_PWHRS", ConWare);
            Adapter.Fill(DS, "Ware");
            gridEX_RHeader.DropDowns["Ware"].SetDataBinding(DS.Tables["Ware"], "");


            Adapter = new SqlDataAdapter("SELECT * from Table_002_SalesTypes", ConBase);
            Adapter.Fill(DS, "SaleType");
            gridEX_RHeader.DropDowns["SaleType"].SetDataBinding(DS.Tables["SaleType"], "");


            Adapter = new SqlDataAdapter("SELECT * from Table_035_ProjectInfo", ConBase);
            Adapter.Fill(DS, "Project");
            gridEX_RHeader.DropDowns["Project"].SetDataBinding(DS.Tables["Project"], "");


            Adapter = new SqlDataAdapter("SELECT * from Table_035_ProjectInfo", ConBase);
            Adapter.Fill(DS, "Project");
            gridEX_RDetail.DropDowns["Project"].SetDataBinding(DS.Tables["Project"], "");


            Adapter = new SqlDataAdapter("SELECT * from Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(DS, "Center");
            gridEX_RDetail.DropDowns["Center"].SetDataBinding(DS.Tables["Center"], "");


            Adapter = new SqlDataAdapter("SELECT * from Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_RDetail.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

            Adapter = new SqlDataAdapter("SELECT * from table_004_CommodityAndIngredients", ConWare);
            Adapter.Fill(DS, "Good");
            gridEX_RDetail.DropDowns["Good"].SetDataBinding(DS.Tables["Good"], "");



            bt_Refresh_Click(null, null);

        }




        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                dataSet_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill(this.dataSet_Sale.Table_010_SaleFactor);
                this.table_011_Child1_SaleFactorTableAdapter.Fill(this.dataSet_Sale.Table_011_Child1_SaleFactor);
                dataSet_Sale.EnforceConstraints = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bt_Save_Receipts_Click(object sender, EventArgs e)
        {
            gridEX_RHeader.UpdateData();
            gridEX_RHeader.RemoveFilters();

            try
            {

                table_010_SaleFactorBindingSource.EndEdit();
                this.table_010_SaleFactorTableAdapter.Update(this.dataSet_Sale.Table_010_SaleFactor);
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
        }

        private void gridEX_RHeader_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {

            if (e.Column.Key == "Column61")
            {
                gridEX_RHeader.SetValue("Column62", Class_BasicOperation._UserName);
                gridEX_RHeader.SetValue("Column63", Class_BasicOperation.ServerDate());
            }

        }

        private void Frm_040_SaleFactorGroupConfirm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Receipts_Click(sender, e);
            else if (e.KeyCode == Keys.R && e.Control)
                bt_Refresh_Click(sender, e);
        }

        private void gridEX_RHeader_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }
    }
}
