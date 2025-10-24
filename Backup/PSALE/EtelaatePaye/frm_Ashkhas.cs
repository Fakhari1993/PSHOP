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
    public partial class frm_Ashkhas : Form
    {
        bool _Del = false;
        Classes.DeleteRelatedInfo CheckRelation = new PSALE.Classes.DeleteRelatedInfo();
        db alldatabase = new db();
        public frm_Ashkhas(bool Del)
        {
            InitializeComponent();
            _Del = Del;
        }

        private void Form03_Persons_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet01_BasicInfo.Table_040_PersonGroupsForEdit' table. You can move, or remove it, as needed.
            this.table_040_PersonGroupsForEditTableAdapter.Fill(this.dataSet01_BasicInfo.Table_040_PersonGroupsForEdit);
            // TODO: This line of code loads data into the 'dataSet01_BasicInfo.Table_045_PersonScopeForEdit' table. You can move, or remove it, as needed.
            this.table_045_PersonScopeForEditTableAdapter.Fill(this.dataSet01_BasicInfo.Table_045_PersonScopeForEdit);
            // TODO: This line of code loads data into the 'dataSet01_BasicInfo.Table_045_PersonInfoForEdit' table. You can move, or remove it, as needed.
            this.table_045_PersonInfoForEditTableAdapter.Fill(this.dataSet01_BasicInfo.Table_045_PersonInfoForEdit);




            this.table_045_PersonScopeForEditTableAdapter.Fill(dataSet01_BasicInfo.Table_045_PersonScopeForEdit);
            this.table_045_PersonInfoBindingSource.MoveLast();
            this.table_045_PersonInfoBindingSource_PositionChanged(sender, e);

        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_045_PersonInfoBindingSource.AddNew();
                rdb_Legal.Checked = true;
                rdb_Active_CheckedChanged(sender, e);
                rdb_Inactive.Checked = false;
                rdb_Inactive_CheckedChanged(sender, e);
                txt_Code.Text= dataSet01_BasicInfo.Table_045_PersonInfoForEdit.Compute("MAX(Column01)+1", null).ToString();
                txt_Code.Focus();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "Form07_StandardDescription");
            }

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
          {
               if (!chk_General.Checked && cmb_Scope.Text.Trim() == "")
                    throw new Exception("گروه شخص را مشخص کنید");

                cmb_Scope.UpdateValueDataSource();
                this.table_045_PersonInfoBindingSource.EndEdit();
                this.table_045_PersonInfoForEditTableAdapter.Update(dataSet01_BasicInfo.Table_045_PersonInfoForEdit);
                this.table_045_PersonScopeBindingSource.EndEdit();
                this.table_045_PersonScopeForEditTableAdapter.Update(dataSet01_BasicInfo.Table_045_PersonScopeForEdit);
                Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "Information");
            }
                catch (System.Data.SqlClient.SqlException es)
              {
                Class_BasicOperation.CheckSqlExp(es, "Form03_Persons");
              }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "Form03_Persons");
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (_Del)
            {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف این شخص هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            CheckRelation.PersonInSanad(int.Parse(
                                    ((DataRowView)this.table_045_PersonInfoBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));

                            this.table_045_PersonInfoBindingSource.RemoveCurrent();
                            this.table_045_PersonInfoBindingSource.EndEdit();
                            this.table_045_PersonInfoForEditTableAdapter.Update(dataSet01_BasicInfo.Table_045_PersonInfoForEdit);
                            Class_BasicOperation.ShowMsg("", "شخص مورد نظر حذف گردید", "Information");
                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, "Form03_Persons");
                        }

                    }
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Warning");
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_045_PersonInfoBindingSource.EndEdit();
                if (!chk_General.Checked && cmb_Scope.Text.Trim() == "")
                    throw new Exception("گروه شخص را مشخص کنید");
                this.table_045_PersonInfoBindingSource.MoveLast();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "Form03_Persons");
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_045_PersonInfoBindingSource.EndEdit();
                if (!chk_General.Checked && cmb_Scope.Text.Trim() == "")
                    throw new Exception("گروه شخص را مشخص کنید");
                this.table_045_PersonInfoBindingSource.MoveNext();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "Form03_Persons");
            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_045_PersonInfoBindingSource.EndEdit();
                if (!chk_General.Checked && cmb_Scope.Text.Trim() == "")
                    throw new Exception("گروه شخص را مشخص کنید");
                this.table_045_PersonInfoBindingSource.MovePrevious();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "Form03_Persons");
            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_045_PersonInfoBindingSource.EndEdit();
                if (!chk_General.Checked && cmb_Scope.Text.Trim() == "")
                    throw new Exception("گروه شخص را مشخص کنید");
                this.table_045_PersonInfoBindingSource.MoveFirst();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "Form03_Persons");
            }
        }

        private void txt_Code_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }



        private void rdb_Righful_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Righful.Checked)
            {
                rdb_Legal.Checked = false;
                txt_Name.ReadOnly = true;
                txt_FirstName.ReadOnly = false;
                txt_LastName.ReadOnly = false;
            }
            else
            {
                rdb_Legal.Checked = true;
                txt_Name.ReadOnly = false;
                txt_FirstName.ReadOnly = true;
                txt_LastName.ReadOnly = true;
            }
        }

        private void rdb_Legal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Legal.Checked)
            {
                rdb_Righful.Checked = false;
                txt_Name.ReadOnly = false;
                txt_FirstName.ReadOnly = true;
                txt_LastName.ReadOnly = true;
            }
            else
            {
                rdb_Righful.Checked = true;
                txt_Name.ReadOnly = true;
                txt_FirstName.ReadOnly = false;
                txt_LastName.ReadOnly = false;
            }
        }

        private void rdb_Active_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Active.Checked)
                rdb_Inactive.Checked = false;
            else
                rdb_Inactive.Checked = true;
        }

        private void rdb_Inactive_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_Inactive.Checked)
                rdb_Active.Checked = false;
            else
                rdb_Active.Checked = true;
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "Form03_Persons");
        }

        private void Form03_Persons_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
            else if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.D && e.Control)
                bt_Del_Click(sender, e);
        }

        private void txt_FirstName_TextChanged(object sender, EventArgs e)
        {
            txt_Name.Text = txt_LastName.Text + " " + txt_FirstName.Text;
        }

        private void table_045_PersonInfoBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (table_045_PersonInfoBindingSource.Count > 0)
            {
                rdb_Active_CheckedChanged(sender, e);
                rdb_Inactive_CheckedChanged(sender, e);
                rdb_Legal_CheckedChanged(sender, e);
                rdb_Righful_CheckedChanged(sender, e);
            }
        }

        private void chk_General_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_General.Checked)
                cmb_Scope.Enabled = false;
            else
                cmb_Scope.Enabled = true;
                
        }

        private void cmb_Scope_CheckedValuesChanged(object sender, EventArgs e)
        {

        }

        private void frm_Ashkhas_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void txt_NationalCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txt_Tel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txt_Fax_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txt_PostalCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
