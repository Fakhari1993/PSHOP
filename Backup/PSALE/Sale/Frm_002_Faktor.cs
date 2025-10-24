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
    public partial class Frm_002_Faktor : Form
    {
        bool _del;
        db alldatabase = new db();
        public Frm_002_Faktor(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        private void Frm_002_PishFaktor_Load(object sender, EventArgs e)
        { 
            db.constr = Properties.Settings.Default.PACNT_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            // TODO: This line of code loads data into the 'dataSet_Sale.Table_010_SaleFactor' table. You can move, or remove it, as needed.
   
            
            this.table_010_SaleFactorTableAdapter.Fill(this.dataSet_Sale.Table_010_SaleFactor);

            // TODO: This line of code loads data into the 'dataSet_Sale.Table_012_Child2_SaleFactor' table. You can move, or remove it, as needed.
            this.table_012_Child2_SaleFactorTableAdapter.Fill(this.dataSet_Sale.Table_012_Child2_SaleFactor);
            // TODO: This line of code loads data into the 'dataSet_Sale.Table_011_Child1_SaleFactor' table. You can move, or remove it, as needed.
            this.table_011_Child1_SaleFactorTableAdapter.Fill(this.dataSet_Sale.Table_011_Child1_SaleFactor);
            // TODO: This line of code loads data into the 'dataSet_Sale.Table_010_SaleFactor' table. You can move, or remove it, as needed.
            // TODO: This line of code loads data into the 'dataSet_Sale.Table_011_Child1_SaleFactor' table. You can move, or remove it, as needed.
            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
           
            
            multiColumnCombo3.DataSource = alldatabase.get_list("SELECT * FROM Table_007_PwhrsDraft");

          
            
            
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



            s = string.Format(s, table_010_SaleFactorTableAdapter.Connection.Database.ToString());
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


            s = string.Format(s, table_010_SaleFactorTableAdapter.Connection.Database.ToString());
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

            table_010_SaleFactorBindingSource_PositionChanged(sender, e);
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
                table_010_SaleFactorBindingSource.EndEdit();
                table_010_SaleFactorBindingSource.MoveLast();

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
                table_010_SaleFactorBindingSource.EndEdit();
                table_010_SaleFactorBindingSource.MoveNext();

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
                table_010_SaleFactorBindingSource.EndEdit();
                table_010_SaleFactorBindingSource.MovePrevious();

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
                table_010_SaleFactorBindingSource.EndEdit();
                table_010_SaleFactorBindingSource.MoveFirst();

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
                table_010_SaleFactorBindingSource.EndEdit();
                table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                table_010_SaleFactorBindingSource.AddNew();

                column07TextBox.Text = Class_BasicOperation._UserName;
                editBox1.Text = alldatabase.get_one_fiald("SELECT getdate() as date");
                column09TextBox.Text = Class_BasicOperation._UserName;
                editBox2.Text = editBox1.Text;
                FarsiLibrary.Utils.PersianDate ob=new FarsiLibrary.Utils.PersianDate();

                faDatePicker1.Text = ob.ToString("####/##/##");
                column02TextBox.Text = ob.ToString("####/##/##");
                column01TextBox.Text = alldatabase.get_one_fiald("SELECT MAX(column01) AS code FROM Table_010_SaleFactor");
                column01TextBox.Text = (Int32.Parse(column01TextBox.Text) + 1).ToString();

                multiColumnCombo1.Focus();
                column19CheckBox.CheckState = CheckState.Unchecked;
            
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

            if (column09TextBox1.Text != "" && column09TextBox1.Text != "0")
            {

                MessageBox.Show("برای فاکتور مورد نظر حواله صادر شده است امکان ذخیره وجود ندارد");
                return;
            }
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
                table_010_SaleFactorBindingSource.EndEdit();
                table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                table_011_Child1_SaleFactorBindingSource.EndEdit();
                table_011_Child1_SaleFactorTableAdapter.Update(dataSet_Sale.Table_011_Child1_SaleFactor);
                table_012_Child2_SaleFactorBindingSource.EndEdit();
                table_012_Child2_SaleFactorTableAdapter.Update(dataSet_Sale.Table_012_Child2_SaleFactor);
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
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف فاکتور فروش مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            table_011_Child1_SaleFactorBindingSource.EndEdit();
                            table_011_Child1_SaleFactorTableAdapter.Update(dataSet_Sale.Table_011_Child1_SaleFactor);
                            table_012_Child2_SaleFactorBindingSource.EndEdit();
                            table_012_Child2_SaleFactorTableAdapter.Update(dataSet_Sale.Table_012_Child2_SaleFactor);
                           
                            this.table_010_SaleFactorBindingSource.RemoveCurrent();
                            this.table_010_SaleFactorBindingSource.EndEdit();
                            this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                            table_010_SaleFactorBindingSource_PositionChanged(sender, e);
                            Class_BasicOperation.ShowMsg("", " فاکتور فروش مورد نظر حذف شد", "Information");
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

                table_010_SaleFactorBindingSource.EndEdit();
            }
            catch(Exception ex)
            {
                table_010_SaleFactorBindingSource.CancelEdit();
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

                table_010_SaleFactorBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                table_010_SaleFactorBindingSource.CancelEdit();
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
                          numericEditBox3.Value =(dataSet_Sale.Table_012_Child2_SaleFactor.Compute("SUM(column04)", "(column05=1) AND (column01="+columnidTextBox.Text+")").ToString());
                          numericEditBox2.Value = (dataSet_Sale.Table_012_Child2_SaleFactor.Compute("SUM(column04)", "(column05=0)AND(column01=" + columnidTextBox.Text + ")").ToString());
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
                if (this.table_011_Child1_SaleFactorBindingSource.Count > 0)
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
                if (this.table_012_Child2_SaleFactorBindingSource.Count > 0)
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

        private void table_010_SaleFactorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {

                faDatePicker1.Text = column02TextBox.Text;

                numericEditBox1.Value = gridEX2.GetTotalRow().Cells["column20"].Value.ToString();
                if (columnidTextBox.Text != "")
                {
                    numericEditBox3.Value = (dataSet_Sale.Table_012_Child2_SaleFactor.Compute("SUM(column04)", "(column05=1) AND (column01=" + columnidTextBox.Text + ")").ToString());
                    numericEditBox2.Value = (dataSet_Sale.Table_012_Child2_SaleFactor.Compute("SUM(column04)", "(column05=0)AND(column01=" + columnidTextBox.Text + ")").ToString());
                }

            }
            catch
            {

            }
        }

        private void editBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            if (DialogResult.No == MessageBox.Show("آیا مایل به اضافه کردن پیش فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
            {
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
                db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                column09TextBox.Text = Class_BasicOperation._UserName;
                editBox2.Text = alldatabase.get_one_fiald("SELECT getdate() AS date");
                table_010_SaleFactorBindingSource.EndEdit();
                table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
                return;
            }

            
            
            if (editBox3.Text != "")
            {
                db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                string id_PishFaktor="-1";
                if (alldatabase.get_count("SELECT columnid FROM Table_007_FactorBefore WHERE column01=" + editBox3.Text) > 0)
                {
                    id_PishFaktor = alldatabase.get_one_fiald("SELECT columnid FROM Table_007_FactorBefore WHERE column01=" + editBox3.Text);
                }
                
                if (alldatabase.get_count("SELECT * FROM Table_008_Child1_FactorBefore WHERE column01=" + id_PishFaktor)>0)
                {
                    DataTable dt = new DataTable();
                    dt = alldatabase.get_list("SELECT * FROM Table_008_Child1_FactorBefore WHERE column01=" +id_PishFaktor);
                    foreach (Janus.Windows.GridEX.GridEXRow row in gridEX2.GetRows())
                    {
                        if (row.Cells["column24"].Value.ToString() == id_PishFaktor)
                        {
                            row.Delete();
                        }

                    }
                    foreach (Janus.Windows.GridEX.GridEXRow row in gridEX1.GetRows())
                    {

                            row.Delete();
                    }


                    DataTable dt2 = new DataTable();
                    dt2 = alldatabase.get_list("SELECT * FROM Table_009_Child2_FactorBefore WHERE column01=" + id_PishFaktor);

                    foreach (DataRow rows in dt2.Select())
                    {

                        DataRow dr = dataSet_Sale.Table_012_Child2_SaleFactor.NewRow();
                        dr["column01"] = columnidTextBox.Text;
                        dr["column02"] = rows["column02"];
                        dr["column03"] = rows["column03"];
                         dr["column04"]=rows["column04"];
                         dr["column05"] = rows["column05"];
                         dr["column06"] = rows["column06"];
                         dataSet_Sale.Table_012_Child2_SaleFactor.Rows.Add(dr);
                    }


               
                        foreach (DataRow rows in dt.Select())
                        {
                            try
                               {
                            DataRow dr = dataSet_Sale.Table_011_Child1_SaleFactor.NewRow();
                            dr["column01"] = columnidTextBox.Text;
                            dr["column02"] = rows["column02"];
                            dr["column03"] = rows["column03"];
                            dr["column04"] = rows["column04"];
                            dr["column05"] = rows["column05"];
                            dr["column06"] = rows["column06"];
                            dr["column07"] = rows["column07"];
                            dr["column08"] = rows["column08"];
                            dr["column09"] = rows["column09"];
                            dr["column10"] = rows["column10"];
                            dr["column11"] = rows["column11"];
                            dr["column16"] = rows["column16"];
                            dr["column17"] = rows["column17"];
                            dr["column18"] = rows["column18"];
                            dr["column19"] = rows["column19"];
                            dr["column20"] = rows["column21"];
                            dr["column21"] = rows["column22"];
                            dr["column22"] = rows["column23"];
                            dr["column23"] = rows["column20"];
                            dr["column24"] = rows["column01"];
                            dr["column25"] = rows["column25"];
                            dr["column27"] = rows["column24"];
                            dr["column28"] = rows["columnid"];
                            dataSet_Sale.Table_011_Child1_SaleFactor.Rows.Add(dr);
                            table_011_Child1_SaleFactorBindingSource.EndEdit();
                       

                    }
                    catch(Exception ex)
                    {
                        Class_BasicOperation.CheckExceptionType(ex, "");
                    } 
                        }
gridEX1.MoveToNewRecord();

gridEX2.MoveToNewRecord();
                }
                else
                {
                    MessageBox.Show("شماره پیش فاکتور صحیح نمی باشد");
                }

            }
        }

        private void faDatePicker1_SelectedDateTimeChanging(object sender, FarsiLibrary.Win.Events.SelectedDateTimeChangingEventArgs e)
        {
            column02TextBox.Text = faDatePicker1.Text;
            if (faDatePicker1.SelectedDateTime.ToString() == "")
            {
                column02TextBox.Text = "";

            }
        }

        private void uiGroupBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {

                MessageBox.Show("فاکتور مورد نظر باطل شده است");
                return;
            }

            
            if (column09TextBox1.Text == "")
            {
                Frm_003_HavaleDialog ob = new Frm_003_HavaleDialog(Int32.Parse(columnidTextBox.Text));
                ob.ShowDialog();
                if (ob.state == 1)
                {
                    db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
                    alldatabase.close();
                    alldatabase.Connect();
                    multiColumnCombo3.DataSource = alldatabase.get_list("SELECT * FROM Table_007_PwhrsDraft");
                    column09TextBox1.Text = ob.id_havale.ToString();
                    table_010_SaleFactorBindingSource.EndEdit();
                    table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);

                }
            }
            else
            {
                MessageBox.Show("برای فاکتور مورد نظر قبلا حواله صادر شده است");
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
            if (checkBox1.CheckState == CheckState.Checked)
            {

                MessageBox.Show("فاکتور مورد نظر باطل شده است");
                return;
            }

            if (editBox4.Text != "" && editBox4.Text != "0")
            {
                MessageBox.Show("برای فاکتور مورد نظر حواله صادر شده است");
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
                if (alldatabase.get_one_fiald("SELECT Column07 FROM Table_007_PwhrsDraft WHERE Columnid=" + multiColumnCombo3.Value).ToString() != "" && alldatabase.get_one_fiald("SELECT Column07 FROM Table_007_PwhrsDraft WHERE Columnid=" + multiColumnCombo3.Value).ToString() != "0")
                {
                    MessageBox.Show("سیستم مالی به صورت ادواری می باشد برای این فاکتور حواله و سند صادر شده است");
                    return;
                }
            }
            Frm_004_SodoorSanadDialog ob = new Frm_004_SodoorSanadDialog(int.Parse(columnidTextBox.Text));
            ob.ShowDialog();
            if (ob.codesanad != "0")
            {
                db.constr = Properties.Settings.Default.PACNT_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();

                editBox4.Text = ob.codesanad.ToString();
                table_010_SaleFactorBindingSource.EndEdit();
                table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
            }
            }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (editBox4.Text == "" || editBox4.Text == "0")
            {
                MessageBox.Show("سندی برای فاکتور فروش مورد نظر ثبت نشده است");
                return;
            }
            
            
            
            
            db.constr = Properties.Settings.Default.PACNT_ConnectionString;
            if (table_010_SaleFactorBindingSource.Count < 1)
                return;
            if (editBox4.Text != "")
            {
                if (DialogResult.Yes == MessageBox.Show(" آیا سند مربوط به فاکتور فروش شماره " + column01TextBox.Text + " را حذف می کنید؟ ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
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


                        
                        alldatabase.delete_update_all("DELETE FROM Table_065_SanadDetail WHERE column17=" + columnidTextBox.Text + " AND column16=15");
                        editBox4.Text = DBNull.Value.ToString();
                 ((DataRowView)table_010_SaleFactorBindingSource.CurrencyManager.Current)["column10"]=0;

                        table_010_SaleFactorBindingSource.EndEdit();
                        table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
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

            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

            if (checkBox1.CheckState == CheckState.Checked)
            {

                MessageBox.Show("فاکتور مورد نظر باطل شده است");
                return;
            }

            try
            {
                if (table_010_SaleFactorBindingSource.Count < 1) return;
                db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                if (column19CheckBox.CheckState == CheckState.Unchecked || column19CheckBox.CheckState== CheckState.Indeterminate)
                {
                    if (DialogResult.No == MessageBox.Show("آیا مایل به مرجوع فاکتور فروش مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        return;
                    int marjooi_cdoe = 0;
                    int marjooi_id;
                    try
                    {
                        marjooi_cdoe = int.Parse(alldatabase.get_one_fiald("SELECT MAX(column01) AS Code FROM Table_018_MarjooiSale").ToString());
                    }
                    catch
                    {

                    }
                    marjooi_cdoe++;
                    string s = @"INSERT INTO Table_018_MarjooiSale
                      (column01, column02, column03, column04, column05, column06, column07, column08, column13, column14, column15, column16, column17)
                        SELECT     {0} , column02, column03, column04, column05, column06, column07, column08, '{1}' AS Expr2, GETDATE() AS Expr3, '{1}' AS Expr4, GETDATE() AS Expr5, 
                                              columnid
                        FROM         Table_010_SaleFactor
                        WHERE     (columnid = {2})";
                    s = string.Format(s, marjooi_cdoe.ToString(), Class_BasicOperation._UserName, columnidTextBox.Text);
                    alldatabase.delete_update_all(s);
                    marjooi_id = int.Parse(alldatabase.get_one_fiald("SELECT columnid FROM Table_018_MarjooiSale WHERE column01=" + marjooi_cdoe.ToString()));

                    s = @"INSERT INTO Table_019_Child1_MarjooiSale
                      (column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, column15, 
                      column16, column17, column18, column19, column20, column21, column22, column23, column24, column25, column27, column28)
SELECT     {0}, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, 
                      column15, column16, column17, column18, column19, column20, column21, column22, column23, column24, column25, column27, column28
FROM         Table_011_Child1_SaleFactor
where column01={1}";
                    s = string.Format(s, marjooi_id.ToString(), columnidTextBox.Text);

                    alldatabase.delete_update_all(s);



                    s = @"INSERT INTO Table_020_Child2_MarjooiSale
                      (column01, column02, column03, column04, column05, column06)
SELECT     {0}, column02, column03, column04, column05, column06
FROM         Table_012_Child2_SaleFactor WHERE column01={1}";

                    s = string.Format(s,marjooi_id.ToString(),columnidTextBox.Text);

                 alldatabase.delete_update_all(s);

















 
                    column20TextBox.Text = marjooi_id.ToString();

                    column19CheckBox.CheckState = CheckState.Checked;
                    table_010_SaleFactorBindingSource.EndEdit();
                    table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                    MessageBox.Show("فاکتور مرود نظر مرجوع شد");


                }
                else
                {
                    MessageBox.Show("فاکتور قبلا مرجوع شده است");
                }
            }
                catch(Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, "");
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
            table_010_SaleFactorBindingSource.Position=     table_010_SaleFactorBindingSource.Find("column01",toolStripTextBox1.TextBox.Text);
    
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

        private void Frm_002_Faktor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
        }

        private void column02TextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (column19CheckBox.CheckState != CheckState.Unchecked)
            {
                MessageBox.Show("فاکتور مورد نظر مرجوع شده است امکان باطل کردن فاکتور وجود ندارد");
                return;
            }
            if (multiColumnCombo3.Value.ToString() != "" && multiColumnCombo3.Value.ToString() != "")
            {
                MessageBox.Show("برای فاکتور مورد نظر حواله صادر شده است امکان باطل کردن فاکتور مورد نظر وجود ندارد");
                return;
            }
            if (editBox4.Text != "" && editBox4.Text != "0")
            {
                MessageBox.Show("برای فاکتور مرود نظر سند صادر شده است امکان باطل کردن فاکتور مورد نظر وجورد ندارد");
            }
            if (checkBox1.CheckState != CheckState.Checked)
            {
                checkBox1.CheckState = CheckState.Checked;
                table_010_SaleFactorBindingSource.EndEdit();
                table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                MessageBox.Show("فاکتور مورد نظر باطل شد");
            }
        }




        }



      


      


 

    }
