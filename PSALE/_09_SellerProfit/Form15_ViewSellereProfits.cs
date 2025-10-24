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
    public partial class Form15_ViewSellereProfits : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);

        public Form15_ViewSellereProfits()
        {
            InitializeComponent();
        }

        private void Form15_ViewSellereProfits_Load(object sender, EventArgs e)
        {
            gridEX1.DropDowns[0].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)");

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Value", typeof(string));
            dt.Rows.Add(0, "میانگین");
            dt.Rows.Add(1, "آخرین قیمت خرید کالا");
            dt.Rows.Add(2, "مطابق جدول پیشنهادی");
            gridEX1.DropDowns[1].DataSource = dt;


            this.table_86_SaleManProfitHeaderTableAdapter.Fill(this.dataSet1.Table_86_SaleManProfitHeader);
            this.table_87_SaleManProfitChildTableAdapter.Fill(this.dataSet1.Table_87_SaleManProfitChild);

        }

        private void Form15_ViewSellereProfits_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
            gridEX_Goods.RemoveFilters();
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (this.table_86_SaleManProfitHeaderBindingSource.Count > 0)
            {
                try
                {
                    Class_UserScope UserScope = new Class_UserScope();
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 254))
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف گزارش جاری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        foreach (DataRowView item in this.table_87_SaleManProfitChildBindingSource)
                        {
                            item.Delete();
                        }
                        this.table_87_SaleManProfitChildBindingSource.EndEdit();
                        this.table_86_SaleManProfitHeaderTableAdapter.Update(this.dataSet1.Table_86_SaleManProfitHeader);
                        this.table_86_SaleManProfitHeaderBindingSource.RemoveCurrent();
                        this.table_86_SaleManProfitHeaderBindingSource.EndEdit();
                        this.table_86_SaleManProfitHeaderTableAdapter.Update(this.dataSet1.Table_86_SaleManProfitHeader);
                        dataSet1.EnforceConstraints = false;
                        this.table_86_SaleManProfitHeaderTableAdapter.Fill(this.dataSet1.Table_86_SaleManProfitHeader);
                        this.table_87_SaleManProfitChildTableAdapter.Fill(this.dataSet1.Table_87_SaleManProfitChild);
                        dataSet1.EnforceConstraints = true;

                        Class_BasicOperation.ShowMsg("", " حذف با موفقیت انجام شد", "Information");

                    }

                }

                catch (SqlException es)
                {
                    Class_BasicOperation.CheckSqlExp(es, this.Name);
                    dataSet1.EnforceConstraints = false;
                    this.table_86_SaleManProfitHeaderTableAdapter.Fill(this.dataSet1.Table_86_SaleManProfitHeader);
                    this.table_87_SaleManProfitChildTableAdapter.Fill(this.dataSet1.Table_87_SaleManProfitChild);
                    dataSet1.EnforceConstraints = true;
                }
                catch (Exception ex)
                {
                    dataSet1.EnforceConstraints = false;
                    this.table_86_SaleManProfitHeaderTableAdapter.Fill(this.dataSet1.Table_86_SaleManProfitHeader);
                    this.table_87_SaleManProfitChildTableAdapter.Fill(this.dataSet1.Table_87_SaleManProfitChild);
                    dataSet1.EnforceConstraints = true;
                }
            }
        }

        private void cmb_Print_Click(object sender, EventArgs e)
        {
            try
            {
                gridEXPrintDocument1.GridEX = gridEX_Goods;
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK && gridEX1.GetRow() != null )
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {

                        string j = " از تاریخ:" + gridEX1.GetRow().Cells["Column00"].Text + " تا تاریخ:" + gridEX1.GetRow().Cells["Column00"].Text + " شخص:" + gridEX1.GetRow().Cells["Column03"].Text + " فی خرید براساس:" + gridEX1.GetRow().Cells["Column04"].Text;
                        gridEXPrintDocument1.PageHeaderLeft = j;
                        gridEXPrintDocument1.PageHeaderRight = "محاسبه پورسانت مسئول فروش";
                        printPreviewDialog1.ShowDialog();
                        gridEXPrintDocument1.PageFooterLeft =
                      FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd") +
                      "**" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                        gridEXPrintDocument1.PageFooterRight =
                          " کاربر " + Class_BasicOperation._UserName;
                    }
            }
            catch { }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                dataSet1.EnforceConstraints = false;
                this.table_86_SaleManProfitHeaderTableAdapter.Fill(this.dataSet1.Table_86_SaleManProfitHeader);
                this.table_87_SaleManProfitChildTableAdapter.Fill(this.dataSet1.Table_87_SaleManProfitChild);
                dataSet1.EnforceConstraints = true;
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }

        }
    }
}
