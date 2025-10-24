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
    public partial class Form09_DiscountRatio : Form
    {
        public Form09_DiscountRatio()
        {
            InitializeComponent();
        }

        private void Form09_DiscountRatio_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.Table_78_DiscountRatio' table. You can move, or remove it, as needed.
            this.table_78_DiscountRatioTableAdapter.Fill(this.dataSet1.Table_78_DiscountRatio);
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

        private void Form09_DiscountRatio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    if (Convert.ToInt32(item.Cells["Column00"].Value) > 100 || Convert.ToInt32(item.Cells["Column00"].Value) < 0)
                    {
                        {

                            Class_BasicOperation.ShowMsg("", item.Cells["Column00"].Value.ToString() + "نامعتبر", "Stop");
                            return;
                        }

                    }

                    if (Convert.ToInt32(item.Cells["Column01"].Value) > 100 || Convert.ToInt32(item.Cells["Column01"].Value) < 0)
                    {
                        {

                            Class_BasicOperation.ShowMsg("", item.Cells["Column01"].Value.ToString() + "نامعتبر", "Stop");
                            return;
                        }

                    }

                    if (Convert.ToInt32(item.Cells["Column00"].Value) > Convert.ToInt32(item.Cells["Column01"].Value))
                    {
                        {

                            Class_BasicOperation.ShowMsg("", " از درصد " + Convert.ToInt32(item.Cells["Column00"].Value) + " تا درصد " + item.Cells["Column01"].Value.ToString() + " نا معتبر است ", "Stop");
                            return;
                        }
                    }
                    foreach (Janus.Windows.GridEX.GridEXRow item2 in gridEX1.GetRows())
                    {
                        if (Convert.ToInt32(item.Cells["ColumnId"].Value) != Convert.ToInt32(item2.Cells["ColumnId"].Value))
                        {
                            bool overlap = Convert.ToInt32(item.Cells["Column00"].Value) < Convert.ToInt32(item2.Cells["Column01"].Value) && Convert.ToInt32(item2.Cells["Column00"].Value) < Convert.ToInt32(item.Cells["Column01"].Value);
                            if (overlap)
                            {
                                Class_BasicOperation.ShowMsg("", " از درصد " + Convert.ToInt32(item.Cells["Column00"].Value) + " تا درصد " + item.Cells["Column01"].Value.ToString() + " همپوشانی دارد ", "Stop");

                                return;
                            }
                        }

                    }
                }
                table_78_DiscountRatioBindingSource.EndEdit();
                this.table_78_DiscountRatioTableAdapter.Update(this.dataSet1.Table_78_DiscountRatio);
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

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 234))
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                return;
            }
        }
    }
}
