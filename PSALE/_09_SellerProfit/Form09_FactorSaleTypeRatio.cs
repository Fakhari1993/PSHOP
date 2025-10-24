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
    public partial class Form09_FactorSaleTypeRatio : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        public Form09_FactorSaleTypeRatio()
        {
            InitializeComponent();
        }

        private void Form09_FactorSaleType_Load(object sender, EventArgs e)
        {
            gridEX1.DropDowns[0].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_002_SalesTypes"), "");
           
            this.table_79_FactorSaleTypeTableAdapter.Fill(this.dataSet1.Table_79_FactorSaleType);
            gridEX1.Select();
            gridEX1.Col = 1;
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_AddingRecord(object sender, CancelEventArgs e)
        {
            gridEX1.SetValue("Column02", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column03", Class_BasicOperation.ServerDate());
            gridEX1.SetValue("Column04", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column05", Class_BasicOperation.ServerDate());
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.SetValue("Column04", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column05", Class_BasicOperation.ServerDate());
        }

        private void Form09_FactorSaleType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_79_FactorSaleTypeBindingSource.EndEdit();
                this.table_79_FactorSaleTypeTableAdapter.Update(this.dataSet1.Table_79_FactorSaleType);
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
    }
}
