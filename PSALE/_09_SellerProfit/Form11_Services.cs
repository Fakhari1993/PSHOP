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
    public partial class Form11_Services : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        public Form11_Services()
        {
            InitializeComponent();
        }

        private void Form11_Services_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.Table_75_SaleType' table. You can move, or remove it, as needed.
            this.table_75_SaleTypeTableAdapter.Fill(this.dataSet1.Table_75_SaleType);
            // TODO: This line of code loads data into the 'dataSet1.Table_84_Services' table. You can move, or remove it, as needed.

            DataTable dst = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_75_SaleType where Column07=1 ");
            gridEX1.DropDowns[0].DataSource = dst;
            this.table_84_ServicesTableAdapter.Fill(this.dataSet1.Table_84_Services);
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
            gridEX1.SetValue("Column03", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column04", Class_BasicOperation.ServerDate());
            gridEX1.SetValue("Column05", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column06", Class_BasicOperation.ServerDate());
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.SetValue("Column05", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column06", Class_BasicOperation.ServerDate());
        }

        private void Form11_Services_KeyDown(object sender, KeyEventArgs e)
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
                table_84_ServicesBindingSource.EndEdit();
                this.table_84_ServicesTableAdapter.Update(this.dataSet1.Table_84_Services);


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

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 229))
            {
                if (this.table_84_ServicesBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف رکورد جاری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            this.table_84_ServicesBindingSource.RemoveCurrent();
                            this.table_84_ServicesBindingSource.EndEdit();
                            this.table_84_ServicesTableAdapter.Update(this.dataSet1.Table_84_Services);

                            Class_BasicOperation.ShowMsg("", "حذف اطلاعات انجام شد", "Information");
                            this.table_84_ServicesTableAdapter.Fill(this.dataSet1.Table_84_Services);


                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, this.Name);
                            this.table_84_ServicesTableAdapter.Fill(this.dataSet1.Table_84_Services);

                        }
                    }
                }
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف رکورد جاری را ندارید", "Warning");
        }
    }
}
