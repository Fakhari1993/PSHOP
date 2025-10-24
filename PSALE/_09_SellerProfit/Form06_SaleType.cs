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
    public partial class Form06_SaleType : Form
    {
        public Form06_SaleType()
        {
            InitializeComponent();
        }

        private void Form02_HolidayRatio_Load(object sender, EventArgs e)
        {
            this.table_75_SaleTypeTableAdapter.Fill(this.dataSet1.Table_75_SaleType);
            gridEX1.Select();
            gridEX1.Col = 1;
        }

        private void gridEX1_AddingRecord(object sender, CancelEventArgs e)
        {
            gridEX1.SetValue("Column03", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column04", Class_BasicOperation.ServerDate());
            gridEX1.SetValue("Column05", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column06", Class_BasicOperation.ServerDate());
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;

            gridEX1.SetValue("Column05", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column06", Class_BasicOperation.ServerDate());
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_75_SaleTypeBindingSource.EndEdit();
                this.table_75_SaleTypeTableAdapter.Update(this.dataSet1.Table_75_SaleType);

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
            if (e.KeyCode == Keys.D && e.Control)
                bt_Del_Click(sender, e);
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 229))
            {
                if (this.table_75_SaleTypeBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف رکورد جاری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            this.table_75_SaleTypeBindingSource.RemoveCurrent();
                            this.table_75_SaleTypeBindingSource.EndEdit();
                            this.table_75_SaleTypeTableAdapter.Update(this.dataSet1.Table_75_SaleType);
                            Class_BasicOperation.ShowMsg("", "حذف اطلاعات انجام شد", "Information");
                            this.table_75_SaleTypeTableAdapter.Fill(this.dataSet1.Table_75_SaleType);

                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, this.Name);
                            this.table_75_SaleTypeTableAdapter.Fill(this.dataSet1.Table_75_SaleType);

                        }
                    }
                }
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف رکورد جاری را ندارید", "Warning");
        }
    }
}
