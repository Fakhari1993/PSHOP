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
    public partial class Form04_CustomerGroupRatio : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();

        public Form04_CustomerGroupRatio()
        {
            InitializeComponent();
        }

        private void Form02_HolidayRatio_Load(object sender, EventArgs e)
        {
            this.table_73_CustomerGroupRatioTableAdapter.Fill(this.dataSet1.Table_73_CustomerGroupRatio);
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
            gridEX1.CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "Column00")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column00","Column00", "Column01", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }
            gridEX1.SetValue("Column04", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column05", Class_BasicOperation.ServerDate());
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_73_CustomerGroupRatioBindingSource.EndEdit();
                this.table_73_CustomerGroupRatioTableAdapter.Update(this.dataSet1.Table_73_CustomerGroupRatio);

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

        private void gridEX1_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column00");
            }
            catch { }
        }

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 225))
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                return;
            }
        }
    }
}
