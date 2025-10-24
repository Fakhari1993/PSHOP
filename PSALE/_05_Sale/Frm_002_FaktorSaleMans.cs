using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._05_Sale
{
    public partial class Frm_002_FaktorSaleMans : Form
    {
        int factorid = 0;
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        public Frm_002_FaktorSaleMans(string _factorid)
        {
            InitializeComponent();
            factorid = Convert.ToInt32(_factorid);
        }

        private void Frm_002_FaktorSaleMans_Load(object sender, EventArgs e)
        {
            gridEX1.DropDowns[0].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)"), "");
            this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, factorid);
            try
            {
                this.table_012_SaleFactorSellerTableAdapter.Fill(this.dataSet_Sale.Table_012_SaleFactorSeller, factorid);
            }
            catch
            {
            }
            gridEX1.Select();
            gridEX1.Col = 2;
        }



        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                if (gridEX1.RowCount>0 &&  Convert.ToDouble(gridEX1.GetTotalRow().Cells["Column03"].Value) != Convert.ToDouble(100))
                {
                    Class_BasicOperation.ShowMsg("", "مجموع درصد همکاری 100 درصد نیست", "Stop");
                    return;

                }

                table_012_SaleFactorSellerBindingSource.EndEdit();
                this.table_012_SaleFactorSellerTableAdapter.Update(this.dataSet_Sale.Table_012_SaleFactorSeller);
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

        private void gridEX1_AddingRecord(object sender, CancelEventArgs e)
        {
            gridEX1.SetValue("Column01", factorid);
            gridEX1.SetValue("Column04", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column05", Class_BasicOperation.ServerDate());
            gridEX1.SetValue("Column06", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column07", Class_BasicOperation.ServerDate());
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "Column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column02", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }


            gridEX1.SetValue("Column06", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column07", Class_BasicOperation.ServerDate());
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column02");
            }
            catch { }
        }

        private void Frm_002_FaktorSaleMans_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
        }

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 218))
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                return;
            }
        }
    }
}
