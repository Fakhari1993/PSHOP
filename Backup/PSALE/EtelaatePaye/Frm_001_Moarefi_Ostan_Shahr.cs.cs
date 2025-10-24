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
    public partial class Frm_001_Moarefi_Ostan_Shahr : Form
    {
        bool _del;
        public Frm_001_Moarefi_Ostan_Shahr(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        private void Frm_001_Moarefi_Ostan_Shahr_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_065_CityInfo' table. You can move, or remove it, as needed.
            this.table_065_CityInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_065_CityInfo);
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_060_ProvinceInfo' table. You can move, or remove it, as needed.
            this.table_060_ProvinceInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_060_ProvinceInfo);

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                table_060_ProvinceInfoBindingSource.EndEdit();
                table_065_CityInfoBindingSource.EndEdit();
                table_060_ProvinceInfoTableAdapter.Update(dataSet_EtelaatPaye.Table_060_ProvinceInfo);
                table_065_CityInfoTableAdapter.Update(dataSet_EtelaatPaye.Table_065_CityInfo);
                MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                


            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }


        }

        private void gridEX2_Enter(object sender, EventArgs e)
        {
            try
            {
                table_060_ProvinceInfoBindingSource.EndEdit();
                table_060_ProvinceInfoTableAdapter.Update(dataSet_EtelaatPaye.Table_060_ProvinceInfo);
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

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "");
        }

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (!_del)
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف استان مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
            {
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void gridEX1_RecordsDeleted(object sender, EventArgs e)
        {
            try
            {
                table_060_ProvinceInfoBindingSource.EndEdit();
                table_060_ProvinceInfoTableAdapter.Update(dataSet_EtelaatPaye.Table_060_ProvinceInfo);
            }
            catch(Exception ex)
            {
                table_060_ProvinceInfoTableAdapter.Fill(dataSet_EtelaatPaye.Table_060_ProvinceInfo);
                Class_BasicOperation.CheckExceptionType(ex, "");
            }

        }

        private void gridEX2_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (!_del)
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف شهر مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
            {
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void gridEX2_RecordsDeleted(object sender, EventArgs e)
        {
            try
            {
                table_065_CityInfoBindingSource.EndEdit();
                table_065_CityInfoTableAdapter.Update(dataSet_EtelaatPaye.Table_065_CityInfo);
            }
            catch (Exception ex)
            {
                table_065_CityInfoTableAdapter.Fill(dataSet_EtelaatPaye.Table_065_CityInfo);
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                table_060_ProvinceInfoBindingSource.EndEdit();
                table_060_ProvinceInfoBindingSource.MoveFirst();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,"");
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                table_060_ProvinceInfoBindingSource.EndEdit();
                table_060_ProvinceInfoBindingSource.MovePrevious();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,"");
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                table_060_ProvinceInfoBindingSource.EndEdit();
                table_060_ProvinceInfoBindingSource.MoveNext();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,"");
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                table_060_ProvinceInfoBindingSource.EndEdit();
                table_060_ProvinceInfoBindingSource.MoveLast();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,"");
            }
        }
    }
}
