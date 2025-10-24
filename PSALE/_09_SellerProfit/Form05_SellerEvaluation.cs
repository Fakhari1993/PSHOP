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
    public partial class Form05_SellerEvaluation : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        public Form05_SellerEvaluation()
        {
            InitializeComponent();
        }

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX2_AddingRecord(object sender, CancelEventArgs e)
        {
            gridEX2.SetValue("Column24", Class_BasicOperation._UserName);
            gridEX2.SetValue("Column25", Class_BasicOperation.ServerDate());
            gridEX2.SetValue("Column26", Class_BasicOperation._UserName);
            gridEX2.SetValue("Column27", Class_BasicOperation.ServerDate());
        }

        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX2.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "Column01")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column01", "Column01", "Column02", gridEX2.EditTextBox.Text, Class_BasicOperation.FilterColumnType.ACCColumn);
            }
            catch { }
            gridEX2.SetValue("Column26", Class_BasicOperation._UserName);
            gridEX2.SetValue("Column27", Class_BasicOperation.ServerDate());
        }

        private void gridEX2_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column01");
            }
            catch { }
        }

        private void Form05_SellerEvaluation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
        }

        private void Form05_SellerEvaluation_Load(object sender, EventArgs e)
        {
            gridEX2.DropDowns[0].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)"), "");

            // TODO: This line of code loads data into the 'dataSet1.Table_74_SaleManEvaluation' table. You can move, or remove it, as needed.
            this.table_74_SaleManEvaluationTableAdapter.Fill(this.dataSet1.Table_74_SaleManEvaluation);
            gridEX2.Select();
            gridEX2.Col = 1;
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX2.UpdateData();
                table_74_SaleManEvaluationBindingSource.EndEdit();
                this.table_74_SaleManEvaluationTableAdapter.Update(this.dataSet1.Table_74_SaleManEvaluation);

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
            gridEXExporter1.GridEX = gridEX2;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void gridEX2_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 227))
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                return;
            }
        }
    }
}
