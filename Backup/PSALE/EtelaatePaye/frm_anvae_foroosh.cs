using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSALE.EtelaatePaye
{
    public partial class frm_anvae_foroosh : Form
    {
        bool _del;
        db alldatabase = new db();
        public frm_anvae_foroosh(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        private void frm_anvae_foroosh_Load(object sender, EventArgs e)
        {


            db.constr = Properties.Settings.Default.PACNT_ConnectionString.ToString();
            db ob = new db();
            try
            {
                ob.Connect();
                multiColumnCombo1.DataSource = ob.get_list("SELECT     * FROM         dbo.AllHeaders()");
                multiColumnCombo2.DataSource = multiColumnCombo1.DataSource;
                // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_002_SalesTypes' table. You can move, or remove it, as needed.

            }
            catch
            {

            }
            this.table_002_SalesTypesTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_002_SalesTypes);
            gridEX1.DropDowns["d"].DataSource = gridEX1.DataSource;

        }

        private void multiColumnCombo1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                column03TextBox.Text = multiColumnCombo1.DropDownList.GetValue("GroupCode").ToString();
                column04TextBox.Text = multiColumnCombo1.DropDownList.GetValue("KolCode").ToString();
                column05TextBox.Text = multiColumnCombo1.DropDownList.GetValue("MoeinCode").ToString();
                column06TextBox.Text = multiColumnCombo1.DropDownList.GetValue("TafsiliCode").ToString();
                column07TextBox.Text = multiColumnCombo1.DropDownList.GetValue("JozCode").ToString();
            }
            catch
            {
            }
        }

        private void multiColumnCombo2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                column09TextBox.Text = multiColumnCombo2.DropDownList.GetValue("GroupCode").ToString();
                column10TextBox.Text = multiColumnCombo2.DropDownList.GetValue("KolCode").ToString();
                column11TextBox.Text = multiColumnCombo2.DropDownList.GetValue("MoeinCode").ToString();
                column12TextBox.Text = multiColumnCombo2.DropDownList.GetValue("TafsiliCode").ToString();
                column13TextBox.Text = multiColumnCombo2.DropDownList.GetValue("JozCode").ToString();
            }
            catch
            {
            }
        }

 

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                
                this.table_002_SalesTypesBindingSource.AddNew();
                db.constr=Properties.Settings.Default.PSALE_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                column01TextBox.Text = dataSet_EtelaatPaye.Tables["Table_002_SalesTypes"].Compute("MAX(column01)+1", null).ToString();

                column01TextBox.Focus();

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void multiColumnCombo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            multiColumnCombo1.DroppedDown = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void multiColumnCombo2_KeyPress(object sender, KeyPressEventArgs e)
        {
            multiColumnCombo2.DroppedDown = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                table_002_SalesTypesBindingSource.EndEdit();
                table_002_SalesTypesTableAdapter.Update(dataSet_EtelaatPaye.Table_002_SalesTypes);
                Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "Information");
            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (_del)
            {
                if (this.table_002_SalesTypesBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف نوع فروش مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            this.table_002_SalesTypesBindingSource.RemoveCurrent();
                            this.table_002_SalesTypesBindingSource.EndEdit();
                            this.table_002_SalesTypesTableAdapter.Update(dataSet_EtelaatPaye.Table_002_SalesTypes);
                            Class_BasicOperation.ShowMsg("", "نوع فروش مورد نظر حذف شد", "Information");
                        }
                        catch (Exception ex)
                        {
                            table_002_SalesTypesTableAdapter.Fill(dataSet_EtelaatPaye.Table_002_SalesTypes);
                            Class_BasicOperation.CheckExceptionType(ex, "");

                        }
                    }
                }
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_002_SalesTypesBindingSource.EndEdit();
                this.table_002_SalesTypesBindingSource.MoveLast();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_002_SalesTypesBindingSource.EndEdit();
                this.table_002_SalesTypesBindingSource.MoveNext();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_002_SalesTypesBindingSource.EndEdit();
                this.table_002_SalesTypesBindingSource.MovePrevious();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_002_SalesTypesBindingSource.EndEdit();
                this.table_002_SalesTypesBindingSource.MoveFirst();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "");
        }

        private void frm_anvae_foroosh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
        }



        private void column01TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void column02TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

    

        private void column15TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode==Keys.Enter)
            {
                bindingNavigator1.Focus();
                bt_New.Select();
            }
        }




    }
}
