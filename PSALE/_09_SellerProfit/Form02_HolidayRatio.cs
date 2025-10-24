using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PSHOP._09_SellerProfit
{
    public partial class Form02_HolidayRatio : Form
    {
        public Form02_HolidayRatio()
        {
            InitializeComponent();
        }

        private void Form02_HolidayRatio_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.Table_71_HolidayRatio' table. You can move, or remove it, as needed.
            this.table_71_HolidayRatioTableAdapter.Fill(this.dataSet1.Table_71_HolidayRatio);
            gridEX1.Select();
            gridEX1.Col = 1;
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

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_71_HolidayRatioBindingSource.EndEdit();
                this.table_71_HolidayRatioTableAdapter.Update(this.dataSet1.Table_71_HolidayRatio);
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

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void Form02_HolidayRatio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
        }

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 221))
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                return;
            }
        }
    }
}
