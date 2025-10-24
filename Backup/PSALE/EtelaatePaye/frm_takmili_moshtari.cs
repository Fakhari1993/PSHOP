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


    public partial class frm_takmili_moshtari : Form
    {
        bool _del;
        public frm_takmili_moshtari(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        private void frm_takmili_moshtari_Load(object sender, EventArgs e)
        {

        }

        private void frm_takmili_moshtari_Load_1(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_065_CityInfo' table. You can move, or remove it, as needed.
            this.table_065_CityInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_065_CityInfo);
            this.table_060_ProvinceInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_060_ProvinceInfo);
              
            

            db.constr = Properties.Settings.Default.PERP_ConnectionString;
            db database = new db();


            this.table_040_PersonGroupsTableAdapter1.Fill(this.dataSet_EtelaatPaye.Table_040_PersonGroups);
  
            this.table_045_PersonInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_045_PersonInfo);
         
            this.table_001_CustomerAdditionalInformationTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_001_CustomerAdditionalInformation);

            this.table_045_PersonScopeTableAdapter1.Fill(this.dataSet_EtelaatPaye.Table_045_PersonScope);

            gridEX1.DropDowns["d"].DataSource = gridEX1.DataSource;
            
            table_045_PersonInfoBindingSource_PositionChanged(sender, e);
 
       }

  

        private void frm_takmili_moshtari_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void column02Combo_ValueChanged(object sender, EventArgs e)
        {
            if (column03Combo.DropDownList.RecordCount < 1)
            {
                column03Combo.Text = "";
                column03Combo.Value = "";
                column03Combo.Enabled = false;

            }
            else
            {
                column03Combo.Enabled = true;
                column03Combo.SelectedIndex = 0;
            }
        }

 
        private void column03Combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            column03Combo.DroppedDown = true;
        }

 

        private void column03uiComboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            column03uiComboBox1.DroppedDown = true;
        }

       

    

        private void bt_New_Click(object sender, EventArgs e)
        {

    
                table_001_CustomerAdditionalInformationBindingSource.EndEdit();

                Class_UserScope UserScope = new Class_UserScope();

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 15))
                {

                    frm_Ashkhas ob = new frm_Ashkhas(false);
                    
                    ob.ShowDialog();

                        dataSet_EtelaatPaye.Clear();

                    this.table_065_CityInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_065_CityInfo);
                    this.table_060_ProvinceInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_060_ProvinceInfo);



                    db.constr = Properties.Settings.Default.PERP_ConnectionString;
                    db database = new db();

                    
                    this.table_040_PersonGroupsTableAdapter1.Fill(this.dataSet_EtelaatPaye.Table_040_PersonGroups);
                    // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_001_CustomerAdditionalInformation' table. You can move, or remove it, as needed.
                    this.table_045_PersonInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_045_PersonInfo);

                    this.table_001_CustomerAdditionalInformationTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_001_CustomerAdditionalInformation);

                    this.table_045_PersonScopeTableAdapter1.Fill(this.dataSet_EtelaatPaye.Table_045_PersonScope);

                    table_045_PersonInfoBindingSource_PositionChanged(sender, e);

                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

            




            }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (table_001_CustomerAdditionalInformationBindingSource.Count < 1)
                {

                    table_001_CustomerAdditionalInformationBindingSource.AddNew();
                    uiGroupBox2.Enabled = true;
                    toolStripButton1.Enabled = false;
                    toolStripButton2.Enabled=true;
                    column01TextBox.Text = columnIdTextBox1.Text;
                    column02Combo.Focus();

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
            
        }

        private void table_045_PersonInfoBindingSource_PositionChanged(object sender, EventArgs e)
        {
            
            if (table_001_CustomerAdditionalInformationBindingSource.Count < 1)
            {
                toolStripButton1.Enabled = true;
                uiGroupBox2.Enabled = false;
                toolStripButton2.Enabled = false;

            }
            else
            {
                toolStripButton2.Enabled = true;
                toolStripButton1.Enabled = false;
                uiGroupBox2.Enabled = true;
            }
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception,"");
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

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

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
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            
            if (table_001_CustomerAdditionalInformationBindingSource.Count < 1)
                {
                    return;
                }
            
            if (_del)
            {
                if (this.table_001_CustomerAdditionalInformationBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف اطلاعات تکمیلی مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            this.table_001_CustomerAdditionalInformationBindingSource.RemoveCurrent();
                            this.table_001_CustomerAdditionalInformationBindingSource.EndEdit();
                            this.table_001_CustomerAdditionalInformationTableAdapter.Update(dataSet_EtelaatPaye.Table_001_CustomerAdditionalInformation);
                            table_045_PersonInfoBindingSource_PositionChanged(sender,e);
                            Class_BasicOperation.ShowMsg("", "اطلاعات تکمیلی مورد نظر حذف شد", "Information");
                        }
                        catch (Exception ex)
                        {
                           

                            table_001_CustomerAdditionalInformationTableAdapter.Fill(dataSet_EtelaatPaye.Table_001_CustomerAdditionalInformation);

                            Class_BasicOperation.CheckExceptionType(ex, "");
                           
                        }
                    }
                }
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            
            
            
            
            
            
            
            
            
            if (table_001_CustomerAdditionalInformationBindingSource.Count < 1)
            {
                return;
            }
            
            try
            {


                table_001_CustomerAdditionalInformationBindingSource.EndEdit();
                table_001_CustomerAdditionalInformationTableAdapter.Update(dataSet_EtelaatPaye.Table_001_CustomerAdditionalInformation);
                Class_BasicOperation.ShowMsg("", "اطلاعات تکمیلی ذخیره شد", "Information");

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");

            }
        }

        private void frm_takmili_moshtari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
            {
                toolStripButton2_Click(sender, e);
            }
            else if (e.KeyCode == Keys.N && e.Control)
                toolStripButton1_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
    
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                table_045_PersonInfoBindingSource.CancelEdit();
            }
            catch
            {

            }
        }

        private void table_045_PersonInfoBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

  

        private void cmb_Scope_Enter(object sender, EventArgs e)
        {
            try
            {

                table_045_PersonInfoBindingSource.EndEdit();
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, "");
                column01numericEditBox.Focus();
            }
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            EtelaatePaye.Frm_001_Moarefi_Ostan_Shahr ob = new Frm_001_Moarefi_Ostan_Shahr(false);
            ob.ShowDialog();
            dataSet_EtelaatPaye.EnforceConstraints = false;
            dataSet_EtelaatPaye.EnforceConstraints = true ;
            table_060_ProvinceInfoTableAdapter.Fill(dataSet_EtelaatPaye.Table_060_ProvinceInfo);
            table_065_CityInfoTableAdapter.Fill(dataSet_EtelaatPaye.Table_065_CityInfo);
            table_001_CustomerAdditionalInformationTableAdapter.Fill(dataSet_EtelaatPaye.Table_001_CustomerAdditionalInformation);

        }

   
    
       

        


   

    }
}
