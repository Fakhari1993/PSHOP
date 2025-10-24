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
    public partial class Form03_TimeRatio : Form
    {
        public Form03_TimeRatio()
        {
            InitializeComponent();
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

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    DateTime myDate;

                    if (DateTime.TryParse(item.Cells["Column00"].Value.ToString(), out myDate))
                    {

                        string time = myDate.ToString("HH:mm");
                        string hour = time.Substring(0, 2);

                        int hourInt = int.Parse(hour);

                        if (hourInt >= 24)
                        {

                            Class_BasicOperation.ShowMsg("", item.Cells["Column00"].Value.ToString() + "نامعتبر", "Stop");
                            return;
                        }



                    }
                    else
                    {

                        Class_BasicOperation.ShowMsg("", item.Cells["Column00"].Value.ToString() + "نامعتبر", "Stop");
                        return;
                    }

                    if (DateTime.TryParse(item.Cells["Column01"].Value.ToString(), out myDate))
                    {

                        string time = myDate.ToString("HH:mm");
                        string hour = time.Substring(0, 2);

                        int hourInt = int.Parse(hour);

                        if (hourInt >= 24)
                        {

                            Class_BasicOperation.ShowMsg("", item.Cells["Column01"].Value.ToString() + "نامعتبر", "Stop");
                            return;
                        }



                    }
                    else
                    {

                        Class_BasicOperation.ShowMsg("", item.Cells["Column01"].Value.ToString() + "نامعتبر", "Stop");
                        return;
                    }
                    DateTime tStartA, tEndA;
                    DateTime.TryParse(item.Cells["Column00"].Value.ToString(), out tStartA);
                    DateTime.TryParse(item.Cells["Column01"].Value.ToString(), out tEndA);
                    if (tStartA > tEndA)
                    {
                        {

                            Class_BasicOperation.ShowMsg("", " از ساعت " + item.Cells["Column00"].Value.ToString() + " تا ساعت " + item.Cells["Column01"].Value.ToString() + " نامعتبر است ", "Stop");

                            return;
                        }
                    }

                    foreach (Janus.Windows.GridEX.GridEXRow item2 in gridEX1.GetRows())
                    {
                        DateTime tStartB, tEndB;

                        if (Convert.ToInt32(item.Cells["ColumnId"].Value) != Convert.ToInt32(item2.Cells["ColumnId"].Value))
                        {
                            DateTime.TryParse(item.Cells["Column00"].Value.ToString(), out tStartA);
                            DateTime.TryParse(item.Cells["Column01"].Value.ToString(), out tEndA);

                            DateTime.TryParse(item2.Cells["Column00"].Value.ToString(), out tStartB);
                            DateTime.TryParse(item2.Cells["Column01"].Value.ToString(), out tEndB);


                            bool overlap = tStartA < tEndB && tStartB < tEndA;
                            if (overlap)
                            {
                                Class_BasicOperation.ShowMsg("", " از ساعت " + item.Cells["Column00"].Value.ToString() + " تا ساعت " + item.Cells["Column01"].Value.ToString() + " همپوشانی دارد ", "Stop");

                                return;
                            }
                        }
                    }
                }

                table_72_TimeRatioBindingSource.EndEdit();
                this.table_72_TimeRatioTableAdapter.Update(this.dataSet1.Table_72_TimeRatio);

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

        private void Form03_TimeRatio_Load_1(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.Table_72_TimeRatio' table. You can move, or remove it, as needed.
            this.table_72_TimeRatioTableAdapter.Fill(this.dataSet1.Table_72_TimeRatio);
            gridEX1.Select();
            gridEX1.Col = 1;
        }

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 223))
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                return;
            }
        }
    }
}
