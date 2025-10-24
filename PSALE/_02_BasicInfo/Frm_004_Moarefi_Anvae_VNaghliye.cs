using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSHOP._02_BasicInfo
{
    public partial class Frm_004_Moarefi_Anvae_VNaghliye : Form
    {
        public Frm_004_Moarefi_Anvae_VNaghliye()
        {
            InitializeComponent();
        }

        private void Frm_004_Moarefi_Anvae_VNaghliye_Load(object sender, EventArgs e)
        {
            this.table_115_VehicleTypeTableAdapter.Fill(this.dataSet_Foroosh.Table_115_VehicleType);

        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception,  this.Name);
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                table_115_VehicleTypeBindingSource.EndEdit();
                table_115_VehicleTypeBindingSource.AddNew();
            }
            catch
            {
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                table_115_VehicleTypeBindingSource.EndEdit();
                table_115_VehicleTypeTableAdapter.Update(dataSet_Foroosh.Table_115_VehicleType);
                MessageBox.Show("اطلاعات ذخیره شد");
            }
            catch(Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,  this.Name);
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if(table_115_VehicleTypeBindingSource.Count>0)
            {
                table_115_VehicleTypeBindingSource.EndEdit();
                table_115_VehicleTypeBindingSource.RemoveCurrent();

            }
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                table_115_VehicleTypeBindingSource.EndEdit();
                table_115_VehicleTypeBindingSource.MoveLast();
            }
            catch(Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                table_115_VehicleTypeBindingSource.EndEdit();
                table_115_VehicleTypeBindingSource.MoveNext();
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
                table_115_VehicleTypeBindingSource.EndEdit();
                table_115_VehicleTypeBindingSource.MovePrevious();
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
                table_115_VehicleTypeBindingSource.EndEdit();
                table_115_VehicleTypeBindingSource.MoveFirst();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,  this.Name);
            }
        }

        private void Frm_004_Moarefi_Anvae_VNaghliye_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.N)
                gridEX1.Row = -1 ;
        }
    }
}
