using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Janus.Windows.GridEX;
namespace PSALE.Sale
{
    public partial class Frm_005_FaktorForooshMarjooi : Form
    {
        bool _del;
        db alldatabase = new db();
        public Frm_005_FaktorForooshMarjooi(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        private void Frm_002_PishFaktor_Load(object sender, EventArgs e)
        {    
            
            this.table_018_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_018_MarjooiSale);
           
            // TODO: This line of code loads data into the 'dataSet_Sale.Table_020_Child2_MarjooiSale' table. You can move, or remove it, as needed.
            this.table_020_Child2_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_020_Child2_MarjooiSale);
            // TODO: This line of code loads data into the 'dataSet_Sale.Table_019_Child1_MarjooiSale' table. You can move, or remove it, as needed.
            this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale);
            // TODO: This line of code loads data into the 'dataSet_Sale.Table_018_MarjooiSale' table. You can move, or remove it, as needed.
      
            db.constr = Properties.Settings.Default.PACNT_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            // TODO: This line of code loads data into the 'dataSet_Sale.Table_010_SaleFactor' table. You can move, or remove it, as needed.
   
            
            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();


            multiColumnCombo3.DataSource = alldatabase.get_list("SELECT * FROM Table_011_PwhrsReceipt");

          
            
            
            string   s = @"SELECT     dbo.table_004_CommodityAndIngredients.columnid AS id, dbo.table_004_CommodityAndIngredients.column01 AS code, 
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



            s = string.Format(s, table_018_MarjooiSaleTableAdapter.Connection.Database.ToString());
            gridEX2.DropDowns["d"].SetDataBinding(alldatabase.get_list(s), "");

            
            
            
            
            
            
            
            db.constr = Properties.Settings.Default.PERP_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();

             s = @"SELECT     dbo.Table_045_PersonInfo.ColumnId AS id, dbo.Table_045_PersonInfo.Column01 AS code, dbo.Table_045_PersonInfo.Column02 AS name, 
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


            s = string.Format(s, table_018_MarjooiSaleTableAdapter.Connection.Database.ToString());
            gridEX2.DropDowns["d2"].DataSource = alldatabase.get_list("SELECT * FROM Table_070_CountUnitInfo");
            gridEX2.DropDowns["d3"].DataSource = alldatabase.get_list("SELECT * FROM Table_030_ExpenseCenterInfo");
            gridEX2.DropDowns["d4"].DataSource = alldatabase.get_list("SELECT * FROM Table_035_ProjectInfo");

            multiColumnCombo1.DataSource = alldatabase.get_list(s);


            db.constr = Properties.Settings.Default.PSALE_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            s = @"SELECT        PERP_Base.dbo.Table_045_PersonInfo.*
                FROM            dbo.GetListPepleForOneSystemAndOneGroup(8, 3) AS GetListPepleForOneSystemAndOneGroup_1 INNER JOIN
                         PERP_Base.dbo.Table_045_PersonInfo ON GetListPepleForOneSystemAndOneGroup_1.ColumnId = PERP_Base.dbo.Table_045_PersonInfo.ColumnId";

            multiColumnCombo2.DataSource = alldatabase.get_list(s);
            gridEX1.DropDowns["d"].DataSource = alldatabase.get_list("SELECT * FROM Table_024_Discount");

            table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
        }

        private void column01TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isNotDigit(e.KeyChar);
                
        }

        private void column14TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isNotDigit(e.KeyChar);
               
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                table_018_MarjooiSaleBindingSource.EndEdit();
                table_018_MarjooiSaleBindingSource.MoveLast();

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
                table_018_MarjooiSaleBindingSource.EndEdit();
                table_018_MarjooiSaleBindingSource.MoveNext();

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
                table_018_MarjooiSaleBindingSource.EndEdit();
                table_018_MarjooiSaleBindingSource.MovePrevious();

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
                table_018_MarjooiSaleBindingSource.EndEdit();
                table_018_MarjooiSaleBindingSource.MoveFirst();

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bt_New_Click(object sender, EventArgs e)
        {

            if (column02TextBox.Text == "" && columnidTextBox.Text!="")
            {
                MessageBox.Show("اطلاعات مورد نیاز را تکمیل کنید");
                return;
            }
            
            
            db.constr = Properties.Settings.Default.PSALE_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            try
            {
                table_018_MarjooiSaleBindingSource.EndEdit();
                table_018_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_018_MarjooiSale);
                table_018_MarjooiSaleBindingSource.AddNew();

                column07TextBox.Text = Class_BasicOperation._UserName;
                editBox1.Text = alldatabase.get_one_fiald("SELECT getdate() as date");
                column09TextBox.Text = Class_BasicOperation._UserName;
                editBox2.Text = editBox1.Text;
                FarsiLibrary.Utils.PersianDate ob=new FarsiLibrary.Utils.PersianDate();

                faDatePicker1.Text = ob.ToString("####/##/##");
                column02TextBox.Text = ob.ToString("####/##/##");
                column01TextBox.Text = alldatabase.get_one_fiald("SELECT MAX(column01) AS code FROM Table_018_MarjooiSale");
                column01TextBox.Text = (Int32.Parse(column01TextBox.Text) + 1).ToString();

                multiColumnCombo1.Focus();
            
            }
            catch(Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex,"");
            }
        }

        private void multiColumnCombo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
            multiColumnCombo1.DroppedDown = true;
        }

        private void multiColumnCombo2_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
            multiColumnCombo2.DroppedDown = true;
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {

            if (editBox4.Text != "" && editBox4.Text != "0")
            {

                MessageBox.Show("برای فاکتور مورد نظر سند صادر شده است امکان ذخیره وجود ندارد");
                return;
            }
            column01TextBox.Focus();
            if (column02TextBox.Text == "")
            {
                MessageBox.Show("اطلاعات مورد نیاز را تکمیل کنید");
                return;
            }


           try
            {
               db.constr=Properties.Settings.Default.PSALE_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                column09TextBox.Text = Class_BasicOperation._UserName;
                editBox2.Text = alldatabase.get_one_fiald("SELECT getdate() AS date");
                table_018_MarjooiSaleBindingSource.EndEdit();
                table_018_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_018_MarjooiSale);
                table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                table_019_Child1_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_019_Child1_MarjooiSale);
                table_020_Child2_MarjooiSaleBindingSource.EndEdit();
                table_020_Child2_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_020_Child2_MarjooiSale);
                MessageBox.Show("اطلاعات ذخیره شد");
           }
          catch(Exception ex)
            {
             Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {

        }



        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (_del)
            {
                if (this.table_018_MarjooiSaleBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف فاکتور مرجوعی فروش مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                            table_019_Child1_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_019_Child1_MarjooiSale);
                            table_020_Child2_MarjooiSaleBindingSource.EndEdit();
                            table_020_Child2_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_020_Child2_MarjooiSale);
                           
                            this.table_018_MarjooiSaleBindingSource.RemoveCurrent();
                            this.table_018_MarjooiSaleBindingSource.EndEdit();
                            this.table_018_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_018_MarjooiSale);
                            table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                            Class_BasicOperation.ShowMsg("", " فاکتور فروش مرجوعی مورد نظر حذف شد", "Information");
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

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "");
        }

   


        private void gridEX2_Enter(object sender, EventArgs e)
        {
            try
            {

                table_018_MarjooiSaleBindingSource.EndEdit();
            }
            catch(Exception ex)
            {
                table_018_MarjooiSaleBindingSource.CancelEdit();
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void gridEX2_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {



        }

        private void gridEX1_Enter(object sender, EventArgs e)
        {
            try
            {

                table_018_MarjooiSaleBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                table_018_MarjooiSaleBindingSource.CancelEdit();
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void gridEX1_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {
            if (e.Column.Key == "column02")
            {

               gridEX1.SetValue("column05",(gridEX1.DropDowns["d"].GetValue("column02")));

               gridEX1.SetValue("column04", "0");
               gridEX1.SetValue("column03", "0");

               if (gridEX1.DropDowns["d"].GetValue("column03").ToString() == "True")
               {
                   gridEX1.SetValue("column04", gridEX1.DropDowns["d"].GetValue("column04").ToString());
               }
               else
               {

                   gridEX1.SetValue("column03", gridEX1.DropDowns["d"].GetValue("column04").ToString());
                   Double darsad;
                   darsad = Convert.ToDouble(gridEX1.DropDowns["d"].GetValue("column04").ToString());

                   Double kol;
                   kol =Convert.ToDouble(gridEX2.GetTotalRow().Cells["column20"].Value.ToString());
                   if (kol == 0)
                       return;
                   gridEX1.SetValue("column04", (kol/100)*darsad);
              
               
               
               
               
               
               }


            }
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception,"");
        }

        private void gridEX1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }



        private void gridEX2_CurrentCellChanged(object sender, EventArgs e)
        {
            numericEditBox1.Value = gridEX2.GetTotalRow().Cells["column20"].Value.ToString();
        }



        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (columnidTextBox.Text != "")
            {
                          numericEditBox3.Value =(dataSet_Sale.Table_020_Child2_MarjooiSale.Compute("SUM(column04)", "(column05=1) AND (column01="+columnidTextBox.Text+")").ToString());
                          numericEditBox2.Value = (dataSet_Sale.Table_020_Child2_MarjooiSale.Compute("SUM(column04)", "(column05=0)AND(column01=" + columnidTextBox.Text + ")").ToString());
            }

        }

        private void numericEditBox1_ValueChanged(object sender, EventArgs e)
        {
            numericEditBox4.Value =Int64.Parse(numericEditBox1.Value.ToString()) + Int64.Parse(numericEditBox2.Value.ToString()) -Int64.Parse(numericEditBox3.Value.ToString());
        }

        private void numericEditBox2_ValueChanged(object sender, EventArgs e)
        {
            numericEditBox4.Value = Int64.Parse(numericEditBox1.Value.ToString()) + Int64.Parse(numericEditBox2.Value.ToString()) - Int64.Parse(numericEditBox3.Value.ToString());

        }

        private void numericEditBox3_ValueChanged(object sender, EventArgs e)
        {
            numericEditBox4.Value = Int64.Parse(numericEditBox1.Value.ToString()) + Int64.Parse(numericEditBox2.Value.ToString()) - Int64.Parse(numericEditBox3.Value.ToString());

        }

        private void gridEX2_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (_del)
            {
                if (this.table_019_Child1_MarjooiSaleBindingSource.Count > 0)
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

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (_del)
            {
                if (this.table_020_Child2_MarjooiSaleBindingSource.Count > 0)
                {
                    if (DialogResult.No == MessageBox.Show("آیا مایل به حذف کسر اضافه مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
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




    

   





        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX2.CurrentCellDroppedDown = true;

        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;
        }


        private void editBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
        }

     

        private void faDatePicker1_SelectedDateTimeChanging(object sender, FarsiLibrary.Win.Events.SelectedDateTimeChangingEventArgs e)
        {
            column02TextBox.Text = faDatePicker1.Text;
            if (faDatePicker1.SelectedDateTime.ToString() == "")
            {
                column02TextBox.Text = "";

            }
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (column09TextBox1.Text == "")
            {
                Frm_006_ResidDialog ob = new Frm_006_ResidDialog(Int32.Parse(columnidTextBox.Text));
                ob.ShowDialog();
                if (ob.state == 1)
                {
                    db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
                    alldatabase.close();
                    alldatabase.Connect();
                    column09TextBox1.Text = ob.id_resid.ToString();
                    table_018_MarjooiSaleBindingSource.EndEdit();
                    table_018_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_018_MarjooiSale);
                    multiColumnCombo3.DataSource = alldatabase.get_list("SELECT * FROM Table_011_PwhrsReceipt");

                }
            }
            else
            {
                MessageBox.Show("برای فاکتور مورد نظر قبلا سند صادر شده است");
            }






        }

        private void gridEX2_CellUpdated(object sender, ColumnActionEventArgs e)
        {





            try
            {
                if (e.Column.Key == "column04" || e.Column.Key == "column05" || e.Column.Key == "column06" || e.Column.Key == "column02")
                {

                    string s2;
                    string s;
                    s = "SELECT    * FROM GetProductCashChanges({0},'{1}')";
                    s2 = @"SELECT    * FROM GetCommodityChanges({0},'{1}')";

                    FarsiLibrary.Utils.PersianDate ob = new FarsiLibrary.Utils.PersianDate();

                    s2 = string.Format(s2, gridEX2.GetValue("column02").ToString(), ob.ToString("####/##/##"));
                    s = string.Format(s, gridEX2.GetValue("column02").ToString(), ob.ToString("####/##/##"));
                    alldatabase.close();
                    db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
                    alldatabase.Connect();

                    if (alldatabase.get_count(s2) > 0)
                    {
                        DataTable dt = new DataTable();
                        dt = alldatabase.get_list(s2);
                        gridEX2.SetValue("tedaddarkartoon", dt.Rows[0]["tkartoon"].ToString());
                        gridEX2.SetValue("tedaddarbaste", dt.Rows[0]["tbaste"].ToString());
                    }
                    else
                    {
                        gridEX2.SetValue("tedaddarkartoon", "0");
                        gridEX2.SetValue("tedaddarbaste", "0");

                    }
                    gridEX2.SetValue("column03", gridEX2.DropDowns["d"].GetValue("vahedshomareshid").ToString());

                    db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                    alldatabase.close();
                    alldatabase.Connect();
                    if (alldatabase.get_count(s) > 0)
                    {
                        DataTable dt = new DataTable();
                        dt = alldatabase.get_list(s);
                        gridEX2.SetValue("column10", dt.Rows[0]["gheymatVahed"].ToString());
                        gridEX2.SetValue("column09", dt.Rows[0]["baste"].ToString());
                        gridEX2.SetValue("column08", dt.Rows[0]["karton"].ToString());
                        gridEX2.SetValue("column16", dt.Rows[0]["takhfif"].ToString());
                        gridEX2.SetValue("column18", dt.Rows[0]["ezafe"].ToString());

                    }
                    else
                    {
                        gridEX2.SetValue("column10", "0");
                        gridEX2.SetValue("column09", "0");
                        gridEX2.SetValue("column08", "0");
                        gridEX2.SetValue("column16", "0");
                        gridEX2.SetValue("column18", "0");
                    }









                    gridEX2.SelectCurrentCellText();
                    if (gridEX2.GetValue("column05")==null)
                        gridEX2.SetValue("column05", "0");
                    if (gridEX2.GetValue("column04") == null)
                        gridEX2.SetValue("column04", "0");
                    if (gridEX2.GetValue("column06") == null)
                        gridEX2.SetValue("column06", "0");


                    gridEX2.SetValue("column07", Convert.ToDouble(gridEX2.GetValue("column05")) * Convert.ToDouble(gridEX2.GetValue("tedaddarbaste")));
                    gridEX2.SetValue("column07", Convert.ToDouble(gridEX2.GetValue("column07")) + (Convert.ToDouble(gridEX2.GetValue("column04")) * Convert.ToDouble(gridEX2.GetValue("tedaddarbaste"))));
                    gridEX2.SetValue("column07", Convert.ToDouble(gridEX2.GetValue("column07")) + Convert.ToDouble(gridEX2.GetValue("column06")));





                    gridEX2.SetValue("column11", Convert.ToDouble(gridEX2.GetValue("column04")) * Convert.ToDouble(gridEX2.GetValue("column08")));
                    gridEX2.SetValue("column11", Convert.ToDouble(gridEX2.GetValue("column11")) + (Convert.ToDouble(gridEX2.GetValue("column05")) * Convert.ToDouble(gridEX2.GetValue("column09"))));
                    gridEX2.SetValue("column11", Convert.ToDouble(gridEX2.GetValue("column11")) + (Convert.ToDouble(gridEX2.GetValue("column06")) * Convert.ToDouble(gridEX2.GetValue("column10"))));

                    double jam, takhfif, ezafe;
                    jam = Convert.ToDouble(gridEX2.GetValue("column11"));
                    takhfif = (jam * (Convert.ToDouble(gridEX2.GetValue("column16")) / 100));
                    ezafe = (Convert.ToDouble(gridEX2.GetValue("column11")) * (Convert.ToDouble(gridEX2.GetValue("column18")) / 100));
                    gridEX2.SetValue("column11", jam);
                    gridEX2.SetValue("column17", Convert.ToInt64(takhfif));
                    gridEX2.SetValue("column19", Convert.ToInt64(ezafe));
                    gridEX2.SetValue("column20", (jam - takhfif) + ezafe);
                    gridEX2.SetValue("column11", Convert.ToInt64(gridEX2.GetValue("column11")));





                }

                numericEditBox1.Value = gridEX2.GetTotalRow().Cells["column11"].Value.ToString();
            }
            catch (Exception ex)
            {
       
            }
        }

        private void column09TextBox1_TextChanged(object sender, EventArgs e)
        {
            if ((column09TextBox1.Text != "" && column09TextBox1.Text != "0")  || (editBox4.Text!="" && editBox4.Text!="0"))
            {
                uiGroupBox1.Enabled = false;
                uiGroupBox2.Enabled = false;
                uiGroupBox3.Enabled = false;
                gridEX1.Enabled = false;
                gridEX2.Enabled = false;

            }
            else
            {
                uiGroupBox1.Enabled = true;
                uiGroupBox2.Enabled = true;
                gridEX1.Enabled = true;
                gridEX2.Enabled = true;

            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {


            if (editBox4.Text != "" && editBox4.Text != "0")
            {
                MessageBox.Show("برای فاکتور مرجوعی فروش مورد نظر حواله صادر شده است");
                return;
            }
           bool advari = false;
            db.constr = Properties.Settings.Default.PERP_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            advari=bool.Parse(alldatabase.get_one_fiald("SELECT Column15 FROM Table_000_OrgInfo WHERE ColumnId=" +  Class_BasicOperation._OrgCode));

            if (advari == true && (multiColumnCombo3.Value.ToString() != "" && multiColumnCombo3.Value.ToString() != "0"))
            {
                db.constr=Properties.Settings.Default.PWHRS_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();

                   if (alldatabase.get_one_fiald("SELECT Column07 FROM Table_011_PwhrsReceipt WHERE Columnid=" + multiColumnCombo3.Value).ToString() != "" && alldatabase.get_one_fiald("SELECT Column07 FROM Table_011_PwhrsReceipt WHERE Columnid=" + multiColumnCombo3.Value).ToString() != "0")
                {
                    MessageBox.Show("سیستم مالی به صورت ادواری می باشد برای این فاکتور رسید انبار و سند صادر شده است");
                    return;
                }
            }
            Frm_007_SodoorSanadMarjooiDialog ob = new Frm_007_SodoorSanadMarjooiDialog(int.Parse(columnidTextBox.Text));
            ob.ShowDialog();
            if (ob.codesanad != "0")
            {
                db.constr = Properties.Settings.Default.PACNT_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();

                editBox4.Text = ob.codesanad.ToString();
                table_018_MarjooiSaleBindingSource.EndEdit();
                table_018_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_018_MarjooiSale);
            }
            }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (editBox4.Text == "" || editBox4.Text == "0")
            {
                MessageBox.Show("سندی برای فاکتور مرجوعی فروش مورد نظر ثبت نشده است");
                return;
            }
            
            
            
            
            db.constr = Properties.Settings.Default.PACNT_ConnectionString;
            if (table_018_MarjooiSaleBindingSource.Count < 1)
                return;
            if (editBox4.Text != "")
            {
                if (DialogResult.Yes == MessageBox.Show(" آیا سند مربوط به فاکتور مرجوعی فروش شماره " + column01TextBox.Text + " را حذف می کنید؟ ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                {
                    try
                    {
                        alldatabase.close();
                        alldatabase.Connect();
                        if (alldatabase.get_count("SELECT * FROM Table_060_SanadHead WHERE Column03=1 AND Column00=" + editBox4.Text) > 0)
                        {
                            MessageBox.Show(" سند شماره " + editBox4.Text + " قطعی شده است امکان حذف وجود ندارد ");
                            return;
                        }


                        
                        alldatabase.delete_update_all("DELETE FROM Table_065_SanadDetail WHERE column17=" + columnidTextBox.Text + " AND column16=17");
                        editBox4.Text ="0";
                        ((DataRowView)table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column10"]=0;

                        table_018_MarjooiSaleBindingSource.EndEdit();
                        table_018_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_018_MarjooiSale);
                        MessageBox.Show("سند مورد نظر حذف شد");
                    }
                    catch (Exception ex)
                    {
                        Class_BasicOperation.CheckExceptionType(ex, "");
                    }


                }
            }
        }

        private void editBox4_TextChanged(object sender, EventArgs e)
        {
            if ((column09TextBox1.Text != "" && column09TextBox1.Text != "0") || (editBox4.Text != "" && editBox4.Text != "0"))
            {
                uiGroupBox1.Enabled = false;
                uiGroupBox2.Enabled = false;
                uiGroupBox3.Enabled = false;
                gridEX1.Enabled = false;
                gridEX2.Enabled = false;

            }
            else
            {
                gridEX1.Enabled = true;
                gridEX2.Enabled = true;
                uiGroupBox1.Enabled = true;
                uiGroupBox2.Enabled = true;
                uiGroupBox3.Enabled = true;

            }
        }



        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true ;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (toolStripTextBox1.TextBox.Text!="")
            {
            table_018_MarjooiSaleBindingSource.Position=     table_018_MarjooiSaleBindingSource.Find("column01",toolStripTextBox1.TextBox.Text);
    
            }
        }

        private void column04TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void column06TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void column07TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void multiColumnCombo3_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void column06TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Frm_005_FaktorForooshMarjooi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
        }

        private void table_018_MarjooiSaleBindingSource_PositionChanged(object sender, EventArgs e)
        {

            try
            {

                faDatePicker1.Text = column02TextBox.Text;

                numericEditBox1.Value = gridEX2.GetTotalRow().Cells["column20"].Value.ToString();
                if (columnidTextBox.Text != "")
                {
                    numericEditBox3.Value = (dataSet_Sale.Table_020_Child2_MarjooiSale.Compute("SUM(column04)", "(column05=1) AND (column01=" + columnidTextBox.Text + ")").ToString());
                    numericEditBox2.Value = (dataSet_Sale.Table_020_Child2_MarjooiSale.Compute("SUM(column04)", "(column05=0)AND(column01=" + columnidTextBox.Text + ")").ToString());
                }

            }
            catch
            {

            }


        }

        private void column09TextBox1_TextChanged_1(object sender, EventArgs e)
        {
            if ((column09TextBox1.Text != "" && column09TextBox1.Text != "0") || (editBox4.Text != "" && editBox4.Text != "0"))
            {
                uiGroupBox1.Enabled = false;
                uiGroupBox2.Enabled = false;
                uiGroupBox3.Enabled = false;
                gridEX1.Enabled = false;
                gridEX2.Enabled = false;

            }
            else
            {
                gridEX1.Enabled = true;
                gridEX2.Enabled = true;
                uiGroupBox1.Enabled = true;
                uiGroupBox2.Enabled = true;
                uiGroupBox3.Enabled = true;

            }
        }

 




        }



      


      


 

    }
