using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSALE.Buy
{
    public partial class Frm_001_SabteDarkhastKharid : Form
    {
        bool _del;
        int _idhead;
        db alldatabase = new db();
        public Frm_001_SabteDarkhastKharid(bool del,int id_head)
        {
            _idhead = id_head;
            _del = del;
            InitializeComponent();
        }

        private void Frm_001_SabteDarkhastKharid_Load(object sender, EventArgs e)
        {
     
             
            
            
            
            this.table_013_RequestBuyTableAdapter.Fill_For_New(this.dataSet_Buy.Table_013_RequestBuy,_idhead);
            
            // TODO: This line of code loads data into the 'dataSet_Buy.Table_014_Child_RequestBuy' table. You can move, or remove it, as needed.
            this.table_014_Child_RequestBuyTableAdapter.Fill_For_new(this.dataSet_Buy.Table_014_Child_RequestBuy,_idhead);
            // TODO: This line of code loads data into the 'dataSet_Buy.Table_013_RequestBuy' table. You can move, or remove it, as needed.

            if (_idhead != -1)
            {
                bt_New.Enabled = false;
            }
            
            
            string s = "";
            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();

             s = @"SELECT     dbo.table_004_CommodityAndIngredients.columnid AS id, dbo.table_004_CommodityAndIngredients.column01 AS code, 
                      dbo.table_004_CommodityAndIngredients.column02 AS name, dbo.table_002_MainGroup.column02 AS goroohasli, 
                      dbo.table_003_SubsidiaryGroup.column03 AS goroohfari, dbo.table_004_CommodityAndIngredients.column09 AS tedad_dar_kartoon, 
                      dbo.table_004_CommodityAndIngredients.column08 AS tedad_dar_baste, 
                        dbo.table_004_CommodityAndIngredients.column07 AS vahedshomareshid,
                      {0}.dbo.Table_004_ProductAdditionalInformation.column09 AS gheymat_kartoon, 
                      {0}.dbo.Table_004_ProductAdditionalInformation.column08 AS gheymat_baste, 
                      {0}.dbo.Table_004_ProductAdditionalInformation.column02 AS gheymat_vahed, 
                      {0}.dbo.Table_004_ProductAdditionalInformation.column05 AS takhfif, 
                      {0}.dbo.Table_004_ProductAdditionalInformation.column06 AS ezafe
                      FROM         dbo.table_004_CommodityAndIngredients INNER JOIN
                      dbo.table_002_MainGroup ON dbo.table_004_CommodityAndIngredients.column03 = dbo.table_002_MainGroup.columnid INNER JOIN
                      dbo.table_003_SubsidiaryGroup ON dbo.table_004_CommodityAndIngredients.column04 = dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
                      {0}.dbo.Table_004_ProductAdditionalInformation ON 
                      dbo.table_004_CommodityAndIngredients.columnid = {0}.dbo.Table_004_ProductAdditionalInformation.column01
                      WHERE dbo.table_004_CommodityAndIngredients.column28=1";



            s = string.Format(s, table_014_Child_RequestBuyTableAdapter.Connection.Database.ToString());
            gridEX1.DropDowns["d"].SetDataBinding(alldatabase.get_list(s), "");

            db.constr = Properties.Settings.Default.PERP_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            gridEX1.DropDowns["d2"].DataSource = alldatabase.get_list("SELECT * FROM Table_070_CountUnitInfo");


            db.constr = Properties.Settings.Default.PSALE_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            s = @"SELECT        PERP_Base.dbo.Table_045_PersonInfo.*
                FROM            dbo.GetListPepleForOneSystemAndOneGroup(8,4) AS GetListPepleForOneSystemAndOneGroup_1 INNER JOIN
                         PERP_Base.dbo.Table_045_PersonInfo ON GetListPepleForOneSystemAndOneGroup_1.ColumnId = PERP_Base.dbo.Table_045_PersonInfo.ColumnId";

            multiColumnCombo2.DataSource = alldatabase.get_list(s);
            table_013_RequestBuyBindingSource_PositionChanged(sender, e);
        }

        private void column02TextBox_SelectedDateTimeChanged(object sender, EventArgs e)
        {
            column02TextBox1.Text = column02TextBox.Text;
            if (column02TextBox.SelectedDateTime.ToString() == "")
            {
                column02TextBox1.Text = "";

            }
        }

        private void table_013_RequestBuyBindingSource_PositionChanged(object sender, EventArgs e)
        {

            try
            {

                column02TextBox.Text = column02TextBox1.Text;


            }
            catch
            {

            }
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            if (_idhead != -1)
                return;
            int max = 0;
            try
            {
                FarsiLibrary.Utils.PersianDate ob = new FarsiLibrary.Utils.PersianDate();
                table_013_RequestBuyBindingSource.EndEdit();
                table_013_RequestBuyTableAdapter.Update(dataSet_Buy.Table_013_RequestBuy);

                table_013_RequestBuyBindingSource.AddNew();
                    column02TextBox1.Text=ob.ToString("####/##/##");
                    column02TextBox.Text = column02TextBox1.Text;
                    try
                    {
                        max =int.Parse(alldatabase.get_one_fiald("SELECT MAX(column01) AS Code FROM Table_013_RequestBuy"));
                    }
                    catch
                    {
                        max = 0;
                    }

                    max++;
                    editBox1.Text = max.ToString();
                    editBox3.Text = Class_BasicOperation._UserName;
                    column06TextBox.Text = alldatabase.get_one_fiald("SELECT getdate()");
                    column07TextBox.Text = Class_BasicOperation._UserName;
                    column08TextBox.Text = column06TextBox.Text;
                    multiColumnCombo2.Focus();

            }

            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex,"");
            }
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                table_013_RequestBuyBindingSource.EndEdit();
                table_013_RequestBuyBindingSource.MoveLast();

            }
            catch(Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                table_013_RequestBuyBindingSource.EndEdit();
                table_013_RequestBuyBindingSource.MoveNext();

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
                table_013_RequestBuyBindingSource.EndEdit();
                table_013_RequestBuyBindingSource.MovePrevious();

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
                table_013_RequestBuyBindingSource.EndEdit();
                table_013_RequestBuyBindingSource.MoveFirst();

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (columnidTextBox.Text == "")
                return;
            try
            {
                table_013_RequestBuyBindingSource.EndEdit();
                table_013_RequestBuyTableAdapter.Update(dataSet_Buy.Table_013_RequestBuy);
                table_014_Child_RequestBuyBindingSource.EndEdit();
                table_014_Child_RequestBuyTableAdapter.Update(dataSet_Buy.Table_014_Child_RequestBuy);

                MessageBox.Show("درخواست خرید با موفقیت ذخیره شد");
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (_del)
            {
                 if (this.table_013_RequestBuyBindingSource.Count > 0)
                     if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف درخواست خرید مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                     {
                         try
                         {

                             table_014_Child_RequestBuyBindingSource.EndEdit();
                             table_014_Child_RequestBuyTableAdapter.Update(dataSet_Buy.Table_014_Child_RequestBuy);
                             table_013_RequestBuyBindingSource.RemoveCurrent();
                             table_013_RequestBuyBindingSource.EndEdit();
                             table_013_RequestBuyTableAdapter.Update(dataSet_Buy.Table_013_RequestBuy);
                             MessageBox.Show("درخواست خرید مورد نظر با موفقیت حذف شد");
                             table_013_RequestBuyBindingSource_PositionChanged(sender, e);
                             
                         }
                         catch (Exception ex)
                         {
                             Class_BasicOperation.CheckExceptionType(ex,"");
                         }

                     }
            }
            else
               Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
        }

        private void gridEX1_Enter(object sender, EventArgs e)
        {
            try
            {
                table_013_RequestBuyBindingSource.EndEdit();
            }
            catch(Exception ex)
            {
                table_013_RequestBuyBindingSource.CancelEdit();
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception,"");
        }

        private void gridEX1_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
                gridEX1.SetValue("column12", Class_BasicOperation._UserName);
                gridEX1.SetValue("column13", alldatabase.get_one_fiald("SELECT getdate()"));
                gridEX1.SetValue("column14", Class_BasicOperation._UserName);
                gridEX1.SetValue("column15", alldatabase.get_one_fiald("SELECT getdate()"));

            }
            catch
            {
            }
        }



        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;
            if (e.Column.Key == "column02")
            {
                try
                {
                    gridEX1.SetValue("column03", gridEX1.DropDowns["d"].GetValue("vahedshomareshid").ToString());
                }
                catch
                {
                }
            }
            try
            {

                gridEX1.SetValue("column14", Class_BasicOperation._UserName);
                gridEX1.SetValue("column15", alldatabase.get_one_fiald("SELECT getdate()"));

            }
            catch
            {
            }
        }

        private void editBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void column02TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void multiColumnCombo2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void editBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void editBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void Frm_001_SabteDarkhastKharid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
        }

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (_del)
            {
                if (this.table_014_Child_RequestBuyBindingSource.Count > 0)
                {
                    if (DialogResult.No == MessageBox.Show("آیا مایل به حذف محصول مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
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


    }
}
