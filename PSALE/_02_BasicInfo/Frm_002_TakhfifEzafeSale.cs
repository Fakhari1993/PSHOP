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
    public partial class Frm_002_TakhfifEzafeSale : Form
    {
        bool _del;
        public Frm_002_TakhfifEzafeSale(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        private void Frm_002_TakhfifEzafeSale_Load(object sender, EventArgs e)
        {
            this.table_024_DiscountTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_024_Discount);
            
            SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
            DataTable Header = new DataTable();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from AllHeaders()", ConAcnt);
            Adapter.Fill(Header);
            multiColumnCombo1.DataSource = Header;
            multiColumnCombo2.DataSource = Header;


        }
        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                table_024_DiscountBindingSource.EndEdit();
                table_024_DiscountBindingSource.AddNew();
                column01TextBox.Select();
            }
            catch(Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,  this.Name);
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                table_024_DiscountBindingSource.EndEdit();
                table_024_DiscountTableAdapter.Update(dataSet_EtelaatPaye.Table_024_Discount);
                  if(sender==bt_Save || sender==this)
                MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,  this.Name);
            }
        }


        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (_del)
            {
                if (this.table_024_DiscountBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف رکورد مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            this.table_024_DiscountBindingSource.RemoveCurrent();
                            this.table_024_DiscountBindingSource.EndEdit();
                            this.table_024_DiscountTableAdapter.Update(dataSet_EtelaatPaye.Table_024_Discount);
                            Class_BasicOperation.ShowMsg("", "رکورد مورد نظر حذف شد", "Information");
                        }
                        catch (Exception ex)
                        {
                      
                            Class_BasicOperation.CheckExceptionType(ex,  this.Name);

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
                this.table_024_DiscountBindingSource.EndEdit();
                this.table_024_DiscountBindingSource.MoveLast();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,  this.Name);
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_024_DiscountBindingSource.EndEdit();
                this.table_024_DiscountBindingSource.MoveNext();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,  this.Name);
            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_024_DiscountBindingSource.EndEdit();
                this.table_024_DiscountBindingSource.MovePrevious();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,  this.Name);
            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_024_DiscountBindingSource.EndEdit();
                this.table_024_DiscountBindingSource.MoveFirst();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,  this.Name);
            }
        }

        private void numericEditBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }


        private void Frm_002_TakhfifEzafeSale_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
                gridEX1.Row = gridEX1.FilterRow.Position;
        }

        private void column02TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception,  this.Name);
        }

        private void multiColumnCombo1_ValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                column05TextBox.Text = multiColumnCombo1.DropDownList.GetValue("GroupCode").ToString();
                column06TextBox.Text = multiColumnCombo1.DropDownList.GetValue("KolCode").ToString();
                column07TextBox.Text = multiColumnCombo1.DropDownList.GetValue("MoeinCode").ToString();
                column08TextBox.Text = multiColumnCombo1.DropDownList.GetValue("TafsiliCode").ToString();
                column09TextBox.Text = multiColumnCombo1.DropDownList.GetValue("JozCode").ToString();
            }
            catch
            {
            }
        }

      

        private void multiColumnCombo2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                column11TextBox.Text = multiColumnCombo2.DropDownList.GetValue("GroupCode").ToString();
                column12TextBox.Text = multiColumnCombo2.DropDownList.GetValue("KolCode").ToString();
                column13TextBox.Text = multiColumnCombo2.DropDownList.GetValue("MoeinCode").ToString();
                column14TextBox.Text = multiColumnCombo2.DropDownList.GetValue("TafsiliCode").ToString();
                column15TextBox.Text = multiColumnCombo2.DropDownList.GetValue("JozCode").ToString();
            }
            catch
            {
            }
        }

      

        private void uiRadioButton2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                uiRadioButton1.Focus();
        }

        private void uiRadioButton1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                uiRadioButton3.Focus();
        }

        private void uiRadioButton3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                uiRadioButton4.Focus();
        }

        private void multiColumnCombo2_KeyPress(object sender, KeyPressEventArgs e)
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





  

      

     


   

    
    }
}
