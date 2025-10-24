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
    public partial class Frm_031_Class : Form
    {
        public Frm_031_Class()
        {
            InitializeComponent();
        }

        private void Frm_031_Class_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_000_Class' table. You can move, or remove it, as needed.
            this.table_000_ClassTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_000_Class);

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

        private void Frm_031_Class_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);

            if (e.KeyCode == Keys.D && e.Control)
                bt_Del_Click(sender, e);
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_000_ClassBindingSource.EndEdit();
                this.table_000_ClassTableAdapter.Update(this.dataSet_EtelaatPaye.Table_000_Class);
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

        private void bt_Del_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 242))
            {
                if (this.table_000_ClassBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف رکورد جاری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            this.table_000_ClassBindingSource.RemoveCurrent();
                            this.table_000_ClassBindingSource.EndEdit();
                            this.table_000_ClassTableAdapter.Update(this.dataSet_EtelaatPaye.Table_000_Class);
                            Class_BasicOperation.ShowMsg("", "حذف اطلاعات انجام شد", "Information");
                            this.table_000_ClassTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_000_Class);


                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, this.Name);
                            this.table_000_ClassTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_000_Class);


                        }
                    }
                }
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف رکورد جاری را ندارید", "Warning");
        }
    }
}
