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
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_024_Discount' table. You can move, or remove it, as needed.
            this.table_024_DiscountTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_024_Discount);
         
            try
            {
               // db.constr = Properties.Settings.Default.PACNT_ConnectionString.ToString();
               // db ob = new db();
               // ob.Connect();
               // multiColumnCombo1.DataSource = ob.get_list("SELECT     * FROM         dbo.AllHeaders()");
            }
            catch
            {

            }
          
            }

        private void bt_New_Click(object sender, EventArgs e)
        {
   
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {





                
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
          
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {
                    }

    

        private void multiColumnCombo1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
              //  column03TextBox.Text = multiColumnCombo1.DropDownList.GetValue("GroupCode").ToString();
              //  column04TextBox.Text = multiColumnCombo1.DropDownList.GetValue("KolCode").ToString();
              //  column05TextBox.Text = multiColumnCombo1.DropDownList.GetValue("MoeinCode").ToString();
              //  column06TextBox.Text = multiColumnCombo1.DropDownList.GetValue("TafsiliCode").ToString();
              //  column07TextBox.Text = multiColumnCombo1.DropDownList.GetValue("JozCode").ToString();
            }
            catch
            {
            }
        }

      

        private void bt_Del_Click(object sender, EventArgs e)
        {
           
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
               // this.Table_001_PWHRSBindingSource.EndEdit();
               // this.Table_001_PWHRSBindingSource.MoveLast();
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
             //   this.Table_001_PWHRSBindingSource.EndEdit();
               // this.Table_001_PWHRSBindingSource.MoveNext();
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
              //  this.Table_001_PWHRSBindingSource.EndEdit();
              //  this.Table_001_PWHRSBindingSource.MovePrevious();
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
                //this.Table_001_PWHRSBindingSource.EndEdit();
               // this.Table_001_PWHRSBindingSource.MoveFirst();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
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
        }

        private void column02TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "");
        }





  

      

     


   

    
    }
}
