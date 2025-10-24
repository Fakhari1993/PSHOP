using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Janus.Data;
namespace PSALE.foroosh
{
    public partial class frm_Sabt_sefareshat : Form
    {
      
        bool _del;
 

        db saledatabase = new db();
        
        public frm_Sabt_sefareshat(bool del)
        {
     


            _del = del;
            InitializeComponent();
        }

        private void frm_sefareshat_Load(object sender, EventArgs e)
        {


            db.constr = Properties.Settings.Default.PERP_ConnectionString;
            db database = new db();
            database.Connect();

            




            string s = @"SELECT     dbo.Table_045_PersonInfo.ColumnId AS id, dbo.Table_045_PersonInfo.Column01 AS code, dbo.Table_045_PersonInfo.Column02 AS name, 
                      dbo.Table_065_CityInfo.Column02 AS shahr, dbo.Table_060_ProvinceInfo.Column01 AS ostan, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column03 AS CodeShahr, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column04, {0}.dbo.Table_001_CustomerAdditionalInformation.column05, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column06, {0}.dbo.Table_001_CustomerAdditionalInformation.column07, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column08, {0}.dbo.Table_001_CustomerAdditionalInformation.column09, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column10, {0}.dbo.Table_001_CustomerAdditionalInformation.column11, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column12, {0}.dbo.Table_001_CustomerAdditionalInformation.column13, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column14, {0}.dbo.Table_001_CustomerAdditionalInformation.column15, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column16, {0}.dbo.Table_001_CustomerAdditionalInformation.column17, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column18, {0}.dbo.Table_001_CustomerAdditionalInformation.column19, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column20, {0}.dbo.Table_001_CustomerAdditionalInformation.column21, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column22, {0}.dbo.Table_001_CustomerAdditionalInformation.column23, 
                      {0}.dbo.Table_001_CustomerAdditionalInformation.column24, dbo.Table_045_PersonInfo.Column07 AS telmoshtari, 
                      dbo.Table_045_PersonInfo.Column08 AS faxmoshtari, dbo.Table_045_PersonInfo.Column13 AS codepostimoshtari, 
                      dbo.Table_045_PersonInfo.Column06 AS adresmoshtari
FROM         dbo.Table_060_ProvinceInfo INNER JOIN
                      dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00 INNER JOIN
                      dbo.Table_045_PersonInfo INNER JOIN
                      {0}.dbo.Table_001_CustomerAdditionalInformation ON 
                      dbo.Table_045_PersonInfo.ColumnId = {0}.dbo.Table_001_CustomerAdditionalInformation.column01 ON 
                      dbo.Table_065_CityInfo.Column01 = {0}.dbo.Table_001_CustomerAdditionalInformation.column03

WHERE     (dbo.Table_045_PersonInfo.Column12 = 1)";


            s = string.Format(s, table_005_OrderHeaderTableAdapter.Connection.Database.ToString());

            
           
            column03TextBox.DataSource =database.get_list(s);
           
            s = "SELECT * FROM Table_065_CityInfo";
            column05TextBox.DataSource = database.get_list(s);
            column07TextBox.DataSource = database.get_list("SELECT * FROM Table_115_VehicleType");



            db.constr = Properties.Settings.Default.PSALE_ConnectionString;
            database.close();
            database.Connect();
            column08TextBox.DataSource = database.get_list("SELECT * FROM Table_002_SalesTypes");

            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            database.close();
            database.Connect();

            s = @"SELECT     dbo.table_004_CommodityAndIngredients.columnid AS id, dbo.table_004_CommodityAndIngredients.column01 AS code, 
                      dbo.table_004_CommodityAndIngredients.column02 AS name, dbo.table_002_MainGroup.column02 AS goroohasli, 
                      dbo.table_003_SubsidiaryGroup.column03 AS goroohfari, dbo.table_004_CommodityAndIngredients.column09 AS tedad_dar_kartoon, 
                      dbo.table_004_CommodityAndIngredients.column08 AS tedad_dar_baste, 
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


            s = string.Format(s,table_005_OrderHeaderTableAdapter.Connection.Database.ToString());
            

            gridEX2.DropDowns["d"].SetDataBinding(database.get_list(s), "");






                  this.table_005_OrderHeaderTableAdapter.Fill_Fornew(this.dataSet_Foroosh.Table_005_OrderHeader);
                 this.table_006_OrderDetailsTableAdapter.Fill_Fornew(this.dataSet_Foroosh.Table_006_OrderDetails);
                 table_005_OrderHeaderBindingSource_PositionChanged(sender, e);
     

        }


        private void bt_New_Click(object sender, EventArgs e)
        {
            if (!is_record() && table_005_OrderHeaderBindingSource.Count > 0)
            {
                MessageBox.Show("هیچ محصولی انتخاب نشده است");
                return;
            }
            int a = 0;
            try
            {
                
                a = Convert.ToInt32(columnidTextBox.Text);
            }
            catch
            {

            }


                if ( a < 0)
                {
                    bt_Save_Click(sender, e);

                    try
                    {
                        table_005_OrderHeaderBindingSource.EndEdit();
                    }
                    catch
                    {

                        return;
                    }
                }

            try
            {     
                db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                saledatabase.close();
                saledatabase.Connect();


                table_005_OrderHeaderBindingSource.AddNew();
                column01TextBox.Text = saledatabase.get_one_fiald("SELECT MAX(column01) AS code FROM Table_005_OrderHeader");
                column01TextBox.Text = (Convert.ToInt32(column01TextBox.Text) + 1).ToString();
                column02TextBox.SelectedDateTime = DateTime.Now;
                column02TextBox1.Text = column02TextBox.Text;
                column03TextBox.Focus();
                column29TextBox.Text = Class_BasicOperation._UserName;
                column31TextBox.Text = Class_BasicOperation._UserName;
                column30DateTimePicker.Text = saledatabase.get_one_fiald("SELECT     GETDATE() AS time");
        


               column03TextBox.DroppedDown = true;
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        
        }

        private void column05TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void column03TextBox_ValueChanged(object sender, EventArgs e)
        {
    try
            {

                if (column03TextBox.SelectedIndex == -1)
                {
                    column05TextBox.Value = DBNull.Value;
                    editBox1.Text = "";
                    editBox2.Text = "";
                    editBox3.Text = "";
                    editBox4.Text = "";
                    editBox5.Text = "";
                    editBox6.Text = "";
                    editBox7.Text = "";
                    editBox8.Text = "";
                    editBox9.Text = "";
                    editBox10.Text = "";
                    return;
                }

            }
            catch
            {
            }

            try
            {
                column05TextBox.Value = column03TextBox.DropDownList.GetValue("CodeShahr").ToString();
                editBox1.Text = column03TextBox.DropDownList.GetValue("ostan").ToString();
                editBox2.Text = column03TextBox.DropDownList.GetValue("shahr").ToString();
                editBox3.Text = column03TextBox.DropDownList.GetValue("telmoshtari").ToString();
                editBox4.Text = column03TextBox.DropDownList.GetValue("faxmoshtari").ToString();
                editBox5.Text = column03TextBox.DropDownList.GetValue("codepostimoshtari").ToString();
                editBox6.Text = column03TextBox.DropDownList.GetValue("adresmoshtari").ToString();
                editBox7.Text = column03TextBox.DropDownList.GetValue("column06").ToString();
                editBox8.Text = column03TextBox.DropDownList.GetValue("column07").ToString();
                editBox9.Text = column03TextBox.DropDownList.GetValue("column05").ToString();
                editBox10.Text = column03TextBox.DropDownList.GetValue("column04").ToString();
            }
            catch
            {
            }


            
        }

  

        private void column03TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            column03TextBox.DroppedDown = true;
        }

        private void column08TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            column08TextBox.DroppedDown = true;
        }

        private void column07TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            column07TextBox.DroppedDown = true;
        }

        private void frm_sefareshat_KeyPress(object sender, KeyPressEventArgs e)
        {
          Class_BasicOperation.isEnter(e.KeyChar);
        }

  

    

        private void column02TextBox_SelectedDateTimeChanged(object sender, EventArgs e)
        {
            if (column02TextBox.SelectedDateTime.ToString() == "")
            {
                column02TextBox1.Text = DBNull.Value.ToString(); ;
                return;
            }

            try
            {
                column02TextBox1.Text = column02TextBox.Text;
            }
            catch
            {

            }
        }

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
         
            Class_BasicOperation.CheckExceptionType(e.Exception,"");

        }

        bool is_record()
        {
            if (table_006_OrderDetailsBindingSource.Count < 1)
            {
                return false;
                
            }
            else
                return true;
        }
        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (!is_record())
            {
                MessageBox.Show("هیچ محصولی انتخاب نشده است");
                return;
            }
            if (uiCheckBox7.CheckState == CheckState.Checked)
            {
                Class_BasicOperation.ShowMsg("", "این سفارش توسط مدیر مالی تایید شده و غیر قابل ویرایش است", "None");
                table_005_OrderHeaderBindingSource.CancelEdit();
                return;
            }


            
            
            if (column02TextBox.SelectedDateTime.ToString() == "")
            {
                column02TextBox1.Text = null;
                
            }

        
            
            try
            {
                saledatabase.close();
                db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                saledatabase.Connect();
                column30DateTimePicker.Text = saledatabase.get_one_fiald("SELECT     GETDATE() AS time");

                column31TextBox.Text = Class_BasicOperation._UserName;
                column02TextBox1.Text = column02TextBox.Text;
                
                table_005_OrderHeaderBindingSource.EndEdit();
                table_005_OrderHeaderTableAdapter.Update(dataSet_Foroosh.Table_005_OrderHeader);
                table_006_OrderDetailsBindingSource.EndEdit();
                table_006_OrderDetailsTableAdapter.Update(dataSet_Foroosh.Table_006_OrderDetails);

              
                Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "None");

           }
            catch (Exception ex)
           {
               Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }



        private void gridEX2_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {

            if (uiCheckBox7.CheckState==CheckState.Checked)
            {


                Class_BasicOperation.ShowMsg("","این سفارش توسط مدیر مالی تایید شده و غیر قابل ویرایش است","None");
                e.Cancel = true;
                
                return;
            }

            
            
            
            try
            {
                String s;

                s = @"SELECT     column10 AS gheymat_kartoon, column09 AS gheymat_baste, column07 AS gheymat_vahed, column05 AS takhfif, column06 AS ezafe
FROM         dbo.Table_003_InformationProductCash
WHERE     (column02 =
                          (SELECT     MAX(column02) AS Expr1
                             FROM         dbo.Table_003_InformationProductCash AS Table_003_InformationProductCash_1
                             WHERE     (column01 = {0})))";
                string s2;
                    s2 = @"SELECT     column07 AS tedad_dar_kartoon, column06 AS tedad_dar_baste
                    FROM         dbo.table_006_CommodityChanges
                    WHERE     (column02 =
                    (SELECT     MAX(column02) AS Expr1
                     FROM         dbo.table_006_CommodityChanges AS table_006_CommodityChanges_1 WHERE (column01 = {0})))";



                if (e.Column.Key == "column02")
                {

                    s = string.Format(s, gridEX2.DropDowns["d"].GetValue("id").ToString());
                    saledatabase.close();
                    db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                    saledatabase.Connect();
                    
                    if (saledatabase.get_count(s) > 0)
                    {
                        DataTable dt = new DataTable();
                        dt = saledatabase.get_list(s);
                        gridEX2.SetValue("column10", dt.Rows[0][0].ToString());
                        gridEX2.SetValue("column09", dt.Rows[0][1].ToString());
                        gridEX2.SetValue("column08", dt.Rows[0][2].ToString());
                        gridEX2.SetValue("column11", dt.Rows[0][3].ToString());
                        gridEX2.SetValue("column12", dt.Rows[0][4].ToString());

                    }
                    else
                    {
                        gridEX2.SetValue("column10", gridEX2.DropDowns["d"].GetValue("gheymat_kartoon").ToString());
                        gridEX2.SetValue("column09", gridEX2.DropDowns["d"].GetValue("gheymat_baste").ToString());
                        gridEX2.SetValue("column08", gridEX2.DropDowns["d"].GetValue("gheymat_vahed").ToString());
                        gridEX2.SetValue("column11", gridEX2.DropDowns["d"].GetValue("takhfif").ToString());
                        gridEX2.SetValue("column12", gridEX2.DropDowns["d"].GetValue("ezafe").ToString());

                    }


                    s2 = string.Format(s2, gridEX2.DropDowns["d"].GetValue("id").ToString());
                    
                    saledatabase.close();
                    db.constr=Properties.Settings.Default.PWHRS_ConnectionString;
                    saledatabase.Connect();
                        if (saledatabase.get_count(s2) > 0)
                    {
                        DataTable dt = new DataTable();
                        dt = saledatabase.get_list(s2);
                        gridEX2.SetValue("column28", dt.Rows[0][0].ToString());
                        gridEX2.SetValue("column29", dt.Rows[0][1].ToString());
                    }
                    else
                    {
                        gridEX2.SetValue("column28", gridEX2.DropDowns["d"].GetValue("tedad_dar_kartoon").ToString());
                        gridEX2.SetValue("column29", gridEX2.DropDowns["d"].GetValue("tedad_dar_baste").ToString());



                    }

                       


                }








            }
            catch(Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }





            try
            {
                if (e.Column.Key == "column04" || e.Column.Key == "column03" || e.Column.Key == "column05" || e.Column.Key == "column02")
                {
                    gridEX2.SelectCurrentCellText();
                    gridEX2.SetValue("column06",Convert.ToDouble(gridEX2.GetValue("column04"))*Convert.ToDouble(gridEX2.GetValue("column28")));
                    gridEX2.SetValue("column06", Convert.ToDouble(gridEX2.GetValue("column06")) + (Convert.ToDouble(gridEX2.GetValue("column03")) * Convert.ToDouble(gridEX2.GetValue("column29"))));
                    gridEX2.SetValue("column06", Convert.ToDouble(gridEX2.GetValue("column06")) + Convert.ToDouble(gridEX2.GetValue("column05")));





                    gridEX2.SetValue("column13", Convert.ToDouble(gridEX2.GetValue("column04")) * Convert.ToDouble(gridEX2.GetValue("column10")));
                    gridEX2.SetValue("column13", Convert.ToDouble(gridEX2.GetValue("column13"))+(Convert.ToDouble(gridEX2.GetValue("column03")) * Convert.ToDouble(gridEX2.GetValue("column09"))));
                    gridEX2.SetValue("column13", Convert.ToDouble(gridEX2.GetValue("column13")) + (Convert.ToDouble(gridEX2.GetValue("column05")) * Convert.ToDouble(gridEX2.GetValue("column08"))));

                    double jam,takhfif,ezafe;
                    jam=Convert.ToDouble(gridEX2.GetValue("column13"));
                    takhfif=( jam * (Convert.ToDouble(gridEX2.GetValue("column11")) / 100));
                    ezafe=( Convert.ToDouble(gridEX2.GetValue("column13")) * (Convert.ToDouble(gridEX2.GetValue("column12")) / 100));
                    gridEX2.SetValue("column13",(jam-takhfif)+ezafe);


                   gridEX2.SetValue("column13", Convert.ToInt64(gridEX2.GetValue("column13")));





                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }

        }

   



        private void gridEX2_UpdatingRecord(object sender, CancelEventArgs e)
        {
            db.constr = Properties.Settings.Default.PERP_ConnectionString;
            saledatabase.close();
            saledatabase.Connect();

            gridEX2.SetValue("column21", Class_BasicOperation._UserName.ToString());
            gridEX2.SetValue("column22", saledatabase.get_one_fiald("SELECT     GETDATE() AS time"));
        }

        private void gridEX2_AddingRecord(object sender, CancelEventArgs e)
        {
            gridEX2.SetValue("column19", Class_BasicOperation._UserName.ToString());
    
        }

        private void frm_sefareshat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control )
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D )
                bt_Del_Click(sender, e);
        }

        private void gridEX2_CellEdited(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {

           }

        private void gridEX2_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (_del)
            {
                if (this.table_006_OrderDetailsBindingSource.Count > 0)
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

   

        private void gridEX2_Enter(object sender, EventArgs e)
        {

            try
            {

                table_005_OrderHeaderBindingSource.EndEdit();
               // SendKeys.Send("{TAB}");

            }

            catch (Exception ex)
            {
                table_006_OrderDetailsBindingSource.CancelEdit();
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
                

        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            if (!is_record() && table_005_OrderHeaderBindingSource.Count > 0)
            {
                MessageBox.Show("هیچ محصولی انتخاب نشده است");
                return;
            }
            try
            {
                table_005_OrderHeaderBindingSource.EndEdit();
                table_005_OrderHeaderBindingSource.MoveFirst();

            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            if (!is_record() && table_005_OrderHeaderBindingSource.Count > 0)
            {
                MessageBox.Show("هیچ محصولی انتخاب نشده است");
                return;
            }
            try
            {
                table_005_OrderHeaderBindingSource.EndEdit();
                table_005_OrderHeaderBindingSource.MovePrevious();

            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            if (!is_record() && table_005_OrderHeaderBindingSource.Count > 0)
            {
                MessageBox.Show("هیچ محصولی انتخاب نشده است");
                return;
            }
            try
            {
                table_005_OrderHeaderBindingSource.EndEdit();
                table_005_OrderHeaderBindingSource.MoveNext();

            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            if (!is_record() && table_005_OrderHeaderBindingSource.Count > 0)
            {
                MessageBox.Show("هیچ محصولی انتخاب نشده است");
                return;
            }

            try
            {
                table_005_OrderHeaderBindingSource.EndEdit();
                table_005_OrderHeaderBindingSource.MoveLast();

            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {

            if (uiCheckBox7.CheckState == CheckState.Checked)
            {
                Class_BasicOperation.ShowMsg("", "این سفارش توسط مدیر مالی تایید شده و غیر قابل ویرایش است", "None");
                table_005_OrderHeaderBindingSource.CancelEdit();
                return;
            }
         
            if (_del)
            {
                if (this.table_005_OrderHeaderBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف سفارش مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            table_006_OrderDetailsBindingSource.EndEdit();
                            table_006_OrderDetailsTableAdapter.Update(dataSet_Foroosh.Table_006_OrderDetails);
                            this.table_005_OrderHeaderBindingSource.RemoveCurrent();
                            this.table_005_OrderHeaderBindingSource.EndEdit();
                            this.table_005_OrderHeaderTableAdapter.Update(dataSet_Foroosh.Table_005_OrderHeader);
                            table_005_OrderHeaderBindingSource_PositionChanged(sender, e);
                            Class_BasicOperation.ShowMsg("", "سفارش مورد نظر حذف شد", "Information");
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

       

        
        private void table_005_OrderHeaderBindingSource_PositionChanged(object sender, EventArgs e)
        {



            if (table_005_OrderHeaderBindingSource.Position == -1)
            {
                gridEX2.Enabled = false;
                uiGroupBox1.Enabled = false;
                uiGroupBox2.Enabled = false;
            }
            else
            {
                gridEX2.Enabled = true;
                uiGroupBox1.Enabled = true;
                uiGroupBox2.Enabled = true;
            }
            try
            {
                column02TextBox.Text = column02TextBox1.Text;
            }
            catch
            {
            }


            if (uiCheckBox7.CheckState==CheckState.Checked || uiCheckBox5.CheckState==CheckState.Checked)
            {
                

                column02TextBox.Enabled = false;
                column03TextBox.ReadOnly = true;
                column08TextBox.ReadOnly = true;
                column07TextBox.ReadOnly = true;
                column05TextBox.ReadOnly = true;
                column06TextBox.ReadOnly = true;
                column04TextBox.ReadOnly = true;
                editBox11.ReadOnly = true;
                gridEX2.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;

            }

            else
            {

                column02TextBox.Enabled = true;
                column03TextBox.ReadOnly = false;
                column08TextBox.ReadOnly = false;
                column07TextBox.ReadOnly = false;
                column05TextBox.ReadOnly = false;
                column06TextBox.ReadOnly = false;
                column04TextBox.ReadOnly = false;
                editBox11.ReadOnly = false;
                gridEX2.Enabled = true;
                gridEX2.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.True;
            }
           
        }


        private void gridEX2_RecordsDeleted(object sender, EventArgs e)
        {
            try
            {
                table_006_OrderDetailsBindingSource.EndEdit();
             //   table_006_OrderDetailsTableAdapter.Update(dataSet_Foroosh.Table_006_OrderDetails);

            }
            catch
            {

            }
        }

      



        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX2.CurrentCellDroppedDown = true;
        }

        private void uiGroupBox2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            db.constr = Properties.Settings.Default.PSALE_ConnectionString;
            db alldatabase = new db();
            alldatabase.close();
            alldatabase.Connect();
            if (column32TextBox.Text == "" || column32TextBox.Text == "0")
            {
                int code = 0;
                try
                {

                    code = int.Parse(alldatabase.get_one_fiald("SELECT isnull(max(column01),0) AS code FROM Table_007_FactorBefore"));
                }
                catch
                {
                }
                FarsiLibrary.Utils.PersianDate fa=new FarsiLibrary.Utils.PersianDate();
                code++;
                string id;
                string s;
                s = @"INSERT INTO Table_007_FactorBefore
                      (column01, column02, column03, column07, column08, column09, column10, column11)
SELECT     {0} AS code, '{1}' AS Tarikh, column03, '{2}' AS Expr1, GETDATE(), '{3}' AS Expr2, GETDATE() AS Expr3, columnid
FROM         Table_005_OrderHeader
WHERE columnid={4}";
                s = string.Format(s,code.ToString(),fa.ToString("####/##/##"),Class_BasicOperation._UserName,Class_BasicOperation._UserName,columnidTextBox.Text);
                alldatabase.delete_update_all(s);


                id = alldatabase.get_one_fiald("SELECT columnid FROM Table_007_FactorBefore WHERE column01="+code.ToString());


                MessageBox.Show(" پیش فاکتور با شماره " + code.ToString() +" ذخیره شد ");
            }
            else
            {
                MessageBox.Show("برای سفارش مورد نظر قبلا پیش فاکتور ثبت شده است");
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            db alldatabase = new db();
            alldatabase.Connect();
            string s2 = @"SELECT    hajm FROM GetCommodityChanges({0},'{1}')";
            string s;
            FarsiLibrary.Utils.PersianDate ob = new FarsiLibrary.Utils.PersianDate();
            foreach (Janus.Windows.GridEX.GridEXRow row in gridEX2.GetRows())
            {
                try
                {
                    s = s2;
                    s = string.Format(s, row.Cells["column02"].Value.ToString(), ob.ToString("####/##/##"));

                    row.BeginEdit();
                    row.Cells["Hajm"].Value = decimal.Parse(row.Cells["column06"].Value.ToString()) * decimal.Parse(alldatabase.get_one_fiald(s));
                    row.EndEdit();

                }
                catch
                {
                }


            }
        }




        
      

    }
}
