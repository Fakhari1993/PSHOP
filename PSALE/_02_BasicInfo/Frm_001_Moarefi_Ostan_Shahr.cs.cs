using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace PSHOP._02_BasicInfo
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
            try
            {
                this.table_060_ProvinceInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_060_ProvinceInfo);
                this.table_065_CityInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_065_CityInfo);
                this.table_160_StatesTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_160_States);
            }
            catch
            {
            }

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                table_060_ProvinceInfoBindingSource.EndEdit();
                table_065_CityInfoBindingSource.EndEdit();
                this.table_160_StatesBindingSource.EndEdit();
                table_060_ProvinceInfoTableAdapter.Update(dataSet_EtelaatPaye.Table_060_ProvinceInfo);
                table_065_CityInfoTableAdapter.Update(dataSet_EtelaatPaye.Table_065_CityInfo);
                table_160_StatesTableAdapter.Update(dataSet_EtelaatPaye.Table_160_States);
                if (sender == bt_Save || sender == this)
                MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }


        }

        private void gridEX2_Enter(object sender, EventArgs e)
        {
            try
            {
                table_060_ProvinceInfoBindingSource.EndEdit();
            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,  this.Name);
            }
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception,  this.Name);
        }

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception,  this.Name);
        }

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (!_del)
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف استان مورد نظر هستید؟",  "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
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
                Class_BasicOperation.CheckExceptionType(ex,  this.Name);
            }

        }

        private void gridEX2_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (!_del)
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg( "", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
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
                dataSet_EtelaatPaye.EnforceConstraints = false;
                table_065_CityInfoTableAdapter.Fill(dataSet_EtelaatPaye.Table_065_CityInfo);
                this.table_160_StatesTableAdapter.Fill(dataSet_EtelaatPaye.Table_160_States);
                dataSet_EtelaatPaye.EnforceConstraints = true;
                Class_BasicOperation.CheckExceptionType(ex,  this.Name);
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
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
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
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
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
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
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
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void Frm_001_Moarefi_Ostan_Shahr_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }

        private void sentostanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                FileStream fs = ((FileStream)saveFileDialog1.OpenFile());
                gridEXExporter1.Export(fs);
                Class_BasicOperation.ShowMsg( "", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void sendshahrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX2;
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                FileStream fs = ((FileStream)saveFileDialog1.OpenFile());
                gridEXExporter1.Export(fs);
                Class_BasicOperation.ShowMsg( "", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void printallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bt_Save_Click(sender, e);
            foreach (Form item in Application.OpenForms)
            {
                if (item.Name == "FRM_001_ReportOstanShahr")
                {
                    item.BringToFront();
                    return;
                }
            }
            _02_BasicInfo.Reports.Frm_001_ReportOstanShahr ob = new _02_BasicInfo.Reports.Frm_001_ReportOstanShahr(0);
            try
            {
                ob.MdiParent = this.MdiParent;
            }
            catch { }
            ob.Show();
        }

        private void printoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bt_Save_Click(sender, e);
            if (gridEX1.RowCount < 1)
                return;
            try
            {

                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "FRM_001_ReportOstanShahr")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                _02_BasicInfo.Reports.Frm_001_ReportOstanShahr ob = new _02_BasicInfo.Reports.Frm_001_ReportOstanShahr(int.Parse(gridEX1.GetValue("Column00").ToString()));
                try
                {
                    ob.MdiParent = this.MdiParent;
                }
                catch { }
                ob.Show();
            }
            catch
            {
            }
        }

        private void Frm_001_Moarefi_Ostan_Shahr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.P)
                mnu_Print.ShowDropDown();
        }

        private void gridEX3_Enter(object sender, EventArgs e)
        {
            try
            {
                table_060_ProvinceInfoBindingSource.EndEdit();
                this.table_065_CityInfoBindingSource.EndEdit();
            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void gridEX3_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (!_del)
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف منطقه مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
            {
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
