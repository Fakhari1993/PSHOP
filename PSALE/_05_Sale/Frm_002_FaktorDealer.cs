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
    public partial class Frm_002_FaktorDealer : Form
    {
        int factorid = 0;

        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        public Frm_002_FaktorDealer(string _factorid)
        {
            InitializeComponent();
            factorid = Convert.ToInt32(_factorid);

        }

        private void Frm_002_FaktorSaleMans_Load(object sender, EventArgs e)
        {
            mlt_dealer.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_045_PersonInfo");
            this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, factorid);
            this.table_012_SaleFactorDeallerTableAdapter.FillByHeaderID(this.dataSet_Sale.Table_012_SaleFactorDealler, factorid);


        }



        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (table_012_SaleFactorDeallerBindingSource.CurrencyManager.Count == 0)
                {
                    Class_BasicOperation.ShowMsg("", " از دکمه جدید استفاده کنید", "Stop");
                    return;
                }
                ((DataRowView)table_012_SaleFactorDeallerBindingSource.CurrencyManager.Current)["Column01"] = factorid;
                column06TextBox.Text = Class_BasicOperation._UserName;
                column07DateTimePicker.Value = Class_BasicOperation.ServerDate();
                table_012_SaleFactorDeallerBindingSource.EndEdit();
                this.table_012_SaleFactorDeallerTableAdapter.Update(this.dataSet_Sale.Table_012_SaleFactorDealler);
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




        private void Frm_002_FaktorSaleMans_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            if (e.KeyCode == Keys.D && e.Control)
                bt_Del_Click(sender, e);
            if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
        }

        private void mlt_dealer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
            else
                Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void mlt_dealer_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");

        }

        private void mlt_dealer_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void bt_New_Click(object sender, EventArgs e)
        {

            this.table_012_SaleFactorDeallerTableAdapter.FillByHeaderID(this.dataSet_Sale.Table_012_SaleFactorDealler, 0);
            table_012_SaleFactorDeallerBindingSource.AddNew();
            column04TextBox.Text = Class_BasicOperation._UserName;
            column05DateTimePicker.Value = Class_BasicOperation.ServerDate();
            column06TextBox.Text = Class_BasicOperation._UserName;
            column07DateTimePicker.Value = Class_BasicOperation.ServerDate();
            mlt_dealer.Select();


        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 229))
            {
                if (this.table_012_SaleFactorDeallerBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف رکورد جاری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            this.table_012_SaleFactorDeallerBindingSource.RemoveCurrent();
                            this.table_012_SaleFactorDeallerBindingSource.EndEdit();
                            this.table_012_SaleFactorDeallerTableAdapter.Update(this.dataSet_Sale.Table_012_SaleFactorDealler);
                            Class_BasicOperation.ShowMsg("", "حذف اطلاعات انجام شد", "Information");
                            this.table_012_SaleFactorDeallerTableAdapter.FillByHeaderID(this.dataSet_Sale.Table_012_SaleFactorDealler, factorid);


                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, this.Name);
                            this.table_012_SaleFactorDeallerTableAdapter.FillByHeaderID(this.dataSet_Sale.Table_012_SaleFactorDealler, factorid);

                        }
                    }
                }
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف رکورد جاری را ندارید", "Warning");
        }




    }
}
