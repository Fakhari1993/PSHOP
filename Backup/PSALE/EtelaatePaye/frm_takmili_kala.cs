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
    public partial class frm_takmili_kala : Form
    {
        bool _del;
        public frm_takmili_kala(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        private void frm_takmili_kala_Load(object sender, EventArgs e)
        {
          
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_003_InformationProductCash' table. You can move, or remove it, as needed.
             // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_004_ProductAdditionalInformation' table. You can move, or remove it, as needed.
            this.table_004_ProductAdditionalInformationTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_004_ProductAdditionalInformation);
            try
            {
                db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
                db database = new db();
                database.Connect();
                multiColumnCombo1.DataSource = database.get_list("SELECT * FROM table_002_MainGroup");
                multiColumnCombo2.DataSource = database.get_list("SELECT * FROM table_003_SubsidiaryGroup");
            }

            catch
            {

            }
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.table_004_CommodityAndIngredients' table. You can move, or remove it, as needed.
            this.table_004_CommodityAndIngredientsTableAdapter.Fill(this.dataSet_EtelaatPaye.table_004_CommodityAndIngredients);
            this.table_003_InformationProductCashTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_003_InformationProductCash);
            table_004_ProductAdditionalInformationBindingSource_PositionChanged(sender, e);
            gridEX1.DropDowns["d"].DataSource = gridEX1.DataSource;
                }

        private void gridEX1_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

        private void table_004_CommodityAndIngredientsBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void table_004_CommodityAndIngredientsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (table_004_ProductAdditionalInformationBindingSource.Count < 1)
            {
                toolStripButton9.Enabled = true;
                uiGroupBox2.Enabled = false;
                toolStripButton8.Enabled = false;
                gridEX2.Enabled = false;

            }
            else
            {
                toolStripButton8.Enabled = true;
                toolStripButton9.Enabled = false;
                uiGroupBox2.Enabled = true;
                gridEX2.Enabled = true;
            }
        }

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception,"");
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            try
            {
                if (table_004_ProductAdditionalInformationBindingSource.Count < 1)
                {

                    table_004_ProductAdditionalInformationBindingSource.AddNew();
                    uiGroupBox2.Enabled = true;
                    toolStripButton9.Enabled = false;
                    toolStripButton8.Enabled = true;
                    column01TextBox1.Text = columnidTextBox.Text;
                    column02TextBox1.Focus();

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (table_004_ProductAdditionalInformationBindingSource.Count < 1)
            {
                return;
            }

            try
            {


                table_004_ProductAdditionalInformationBindingSource.EndEdit();
                table_004_ProductAdditionalInformationTableAdapter.Update(dataSet_EtelaatPaye.Table_004_ProductAdditionalInformation);
                table_003_InformationProductCashBindingSource.EndEdit();
                table_003_InformationProductCashTableAdapter.Update(dataSet_EtelaatPaye.Table_003_InformationProductCash);
                Class_BasicOperation.ShowMsg("", "اطلاعات تکمیلی ذخیره شد", "Information");

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");

            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (table_004_ProductAdditionalInformationBindingSource.Count < 1)
            {
                return;
            }

            if (_del)
            {
                if (this.table_004_ProductAdditionalInformationBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف اطلاعات تکمیلی مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            this.table_004_ProductAdditionalInformationBindingSource.RemoveCurrent();
                            this.table_004_ProductAdditionalInformationBindingSource.EndEdit();
                            this.table_004_ProductAdditionalInformationTableAdapter.Update(dataSet_EtelaatPaye.Table_004_ProductAdditionalInformation);
                            table_004_CommodityAndIngredientsBindingSource_PositionChanged(sender, e);
                            Class_BasicOperation.ShowMsg("", "اطلاعات تکمیلی مورد نظر حذف شد", "Information");
                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, "");
                        }
                    }
                }
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");

        }

        private void gridEX2_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (_del)
            {
                if (this.table_003_InformationProductCashBindingSource.Count > 0)
                {
                    if (DialogResult.No == MessageBox.Show("آیا مایل به حذف شناسه ریالی مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                e.Cancel = true;

            }
        }

     

        private void table_004_ProductAdditionalInformationBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (table_004_ProductAdditionalInformationBindingSource.Count < 1)
            {
                gridEX2.Enabled = false;

            }
            else
            {
                gridEX2.Enabled=true;
            }
        }

        private void gridEX2_RecordsDeleted(object sender, EventArgs e)
        {
            try
            {
                table_003_InformationProductCashBindingSource.EndEdit();
                table_003_InformationProductCashTableAdapter.Update(dataSet_EtelaatPaye.Table_003_InformationProductCash);
            }
            catch(Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void frm_takmili_kala_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void frm_takmili_kala_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                toolStripButton8_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                toolStripButton9_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                table_004_CommodityAndIngredientsBindingSource.EndEdit();
                table_004_CommodityAndIngredientsBindingSource.MoveLast();

            }

            catch
            {

            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

            try
            {
                table_004_CommodityAndIngredientsBindingSource.EndEdit();
                table_004_CommodityAndIngredientsBindingSource.MoveNext();

            }

            catch
            {

            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {

            try
            {
                table_004_CommodityAndIngredientsBindingSource.EndEdit();
                table_004_CommodityAndIngredientsBindingSource.MovePrevious();

            }

            catch
            {

            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {

            try
            {
                table_004_CommodityAndIngredientsBindingSource.EndEdit();
                table_004_CommodityAndIngredientsBindingSource.MoveFirst();

            }

            catch
            {

            }
        }

    }
}
