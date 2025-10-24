using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Janus.Windows.GridEX;

namespace PSALE.foroosh
{
    public partial class frm_moshahede_sefareshat_foroosh : Form
    {
        db alldatabase = new db();
        bool _taeed;

        public frm_moshahede_sefareshat_foroosh(bool taeed) 
        {
            _taeed = taeed;

            InitializeComponent();
        }

        private void frm_moshahede_sefareshat_mali_Load(object sender, EventArgs e)
        {   
            this.table_005_OrderHeaderTableAdapter.Fill(this.dataSet_Foroosh.Table_005_OrderHeader);

            // TODO: This line of code loads data into the 'dataSet_Foroosh.Table_006_OrderDetails' table. You can move, or remove it, as needed.
            this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);
            // TODO: This line of code loads data into the 'dataSet_Foroosh.Table_005_OrderHeader' table. You can move, or remove it, as needed.
         
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



             gridEX2.DropDowns["d"].SetDataBinding(database.get_list(s), "");
             db.constr = Properties.Settings.Default.PERP_ConnectionString;
             database.close();
             database.Connect();
             gridEX2.DropDowns["d2"].SetDataBinding(database.get_list("SELECT     dbo.Table_065_CityInfo.Column01 AS id, dbo.Table_060_ProvinceInfo.Column01 + ' - ' + dbo.Table_065_CityInfo.Column02 AS shahr FROM         dbo.Table_060_ProvinceInfo INNER JOIN dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00"), "");

             gridEX2.DropDowns["d3"].SetDataBinding(database.get_list("SELECT Column00,Column01 FROM Table_115_VehicleType"), "");

             db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
             database.close();
             database.Connect();
             gridEX1.DropDowns["d"].SetDataBinding(database.get_list("SELECT columnid,column02 FROM table_004_CommodityAndIngredients"), "");
             try
             {
                 gridEX2.MoveFirst();
             }
             catch
             {
             }
             multicombo1.DataSource = gridEX2.DropDowns["d"].DataSource;
             gridEX2.DropDowns["d4"].DataSource = gridEX2.DropDowns["d"].DataSource;
             barrasinashodeToolStripMenuItem_Click(sender, e);
        }





        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (gridEX2.GetCheckedRows().Length < 1)
                return;

            if (_taeed)
            {

                if (DialogResult.Yes == MessageBox.Show(" آیا مایل به تایید " + gridEX2.GetCheckedRows().Length + " سفارش هستید؟ ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {
                    foreach (GridEXRow row in gridEX2.GetCheckedRows())
                    {

                        table_005_OrderHeaderBindingSource.Position = table_005_OrderHeaderBindingSource.Find("columnid", row.Cells["columnid"].Value.ToString());

                        if (uiCheckBox2.CheckState == CheckState.Checked || table_005_OrderHeaderBindingSource.Count < 1)
                        {
                            continue;
                        }
                        db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                        alldatabase.close();
                        alldatabase.Connect();
                        string anbar;
                        anbar = alldatabase.get_one_fiald("SELECT column18 from Table_005_OrderHeader WHERE columnid=" + gridEX2.CurrentRow.Cells["columnid"].Value.ToString());
                        if (anbar == "True")
                        {
                            Class_BasicOperation.ShowMsg("", " کاربر گرامی سفارش شماره " + row.Cells["column01"].Value.ToString() + " توسط مدیر مالی بررسی شده است اجازه تایید یا عدم تایید را ندارید ", "None");
                            continue;
                        }

                        anbar = alldatabase.get_one_fiald("SELECT column13 from Table_005_OrderHeader WHERE columnid=" + gridEX2.CurrentRow.Cells["columnid"].Value.ToString());
                        if (anbar == "True")
                        {
                            Class_BasicOperation.ShowMsg("", " کاربر گرامی سفارش شماره " + row.Cells["column01"].Value.ToString() + " انصراف داده شده است اجازه تایید یا عدم تایید را ندارید ", "None");
                            continue;
                        }
                        uiCheckBox2.CheckState = CheckState.Checked;

                        editBox17.Text = Class_BasicOperation._UserName;

                        FarsiLibrary.Utils.PersianDate pd3 = FarsiLibrary.Utils.PersianDateConverter.ToPersianDate(DateTime.Now);
                        editBox16.Text = pd3.ToString("####/##/##").ToString();
                        db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                        alldatabase.close();
                        alldatabase.Connect();
                        gridEX2.SetValue("column11", alldatabase.get_one_fiald("SELECT getdate()"));
                        table_005_OrderHeaderBindingSource.EndEdit();
                        table_005_OrderHeaderTableAdapter.Update(dataSet_Foroosh.Table_005_OrderHeader);
                    }
                    gridEX2.Refresh();
                    gridEX2.UnCheckAllRecords();
                        
                     }

                
                }   
                 else
                {
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما اجازه تایید یا عدم تایید سفارش را ندارید", "None");
                }
            
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (gridEX2.GetCheckedRows().Length < 1)
                return;

            if (_taeed)
            {
                if (DialogResult.Yes == MessageBox.Show(" آیا مایل به لغو تاییدیه " + gridEX2.GetCheckedRows().Length + " سفارش هستید؟ ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    foreach (GridEXRow row in gridEX2.GetCheckedRows())
                    {
                        table_005_OrderHeaderBindingSource.Position = table_005_OrderHeaderBindingSource.Find("columnid", row.Cells["columnid"].Value.ToString());

                        if (uiCheckBox2.CheckState == CheckState.Unchecked || table_005_OrderHeaderBindingSource.Count < 1)
                            continue;


                        db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                        alldatabase.close();
                        alldatabase.Connect();
                        string anbar;
                        anbar = alldatabase.get_one_fiald("SELECT column18 from Table_005_OrderHeader WHERE columnid=" + gridEX2.CurrentRow.Cells["columnid"].Value.ToString());
                        if (anbar == "True")
                        {
                            Class_BasicOperation.ShowMsg(""," کاربر گرامی سفارش شماره " + row.Cells["column01"].Value.ToString() + " توسط مدیر مالی بررسی شده است اجازه تایید یا عدم تایید را ندارید ", "None");
                            continue;
                        }

                        anbar = alldatabase.get_one_fiald("SELECT column13 from Table_005_OrderHeader WHERE columnid=" + gridEX2.CurrentRow.Cells["columnid"].Value.ToString());
                        if (anbar == "True")
                        {
                            Class_BasicOperation.ShowMsg(""," کاربر گرامی سفارش شماره " + row.Cells["column01"].Value.ToString() + " انصراف داده شده است اجازه تایید یا عدم تایید را ندارید ", "None");
                            continue;
                        }
                        uiCheckBox2.CheckState = CheckState.Unchecked;
                        editBox17.Text = Class_BasicOperation._UserName;
                        FarsiLibrary.Utils.PersianDate pd3 = FarsiLibrary.Utils.PersianDateConverter.ToPersianDate(DateTime.Now);
                        editBox16.Text = pd3.ToString("####/##/##").ToString();
                        db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                        alldatabase.close();
                        alldatabase.Connect();
                        gridEX2.SetValue("column11", alldatabase.get_one_fiald("SELECT getdate()"));
                        table_005_OrderHeaderBindingSource.EndEdit();
                        table_005_OrderHeaderTableAdapter.Update(dataSet_Foroosh.Table_005_OrderHeader);
                        gridEX2.Refresh(); 
                        gridEX2.UnCheckAllRecords();


                    }
               

            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما اجازه تایید یا عدم تایید سفارش را ندارید", "None");
            }
            





        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (table_005_OrderHeaderBindingSource.Count > 0)
            {
                Frm_001_MoshahadeSefareshat ob = new Frm_001_MoshahadeSefareshat(true, true, true, Convert.ToInt32(gridEX2.CurrentRow.Cells["columnid"].Value), true);
                ob.ShowDialog();
           
                dataSet_Foroosh.Clear();
                  table_005_OrderHeaderTableAdapter.Fill(dataSet_Foroosh.Table_005_OrderHeader);
                 table_006_OrderDetailsTableAdapter.Fill(dataSet_Foroosh.Table_006_OrderDetails);
               
          


            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                table_005_OrderHeaderBindingSource.EndEdit();
                table_005_OrderHeaderBindingSource.MoveFirst();
            }

            catch(Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, "");
            }


        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
              try
            {
                table_005_OrderHeaderBindingSource.EndEdit();
                table_005_OrderHeaderBindingSource.MovePrevious();
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
                table_005_OrderHeaderBindingSource.EndEdit();
                table_005_OrderHeaderBindingSource.MoveNext();
            }

            catch(Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
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

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table_005_OrderHeaderBindingSource.Filter = "(column18 IS NULL OR column18=False) AND column22 IS NULL AND column13 IS NULL";
        }

     
        private void adametaeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table_005_OrderHeaderBindingSource.Filter = "(column18 IS NULL OR column18=False) AND column22 IS NULL AND column13 IS NULL AND column09=False";

        }

        private void taeedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table_005_OrderHeaderBindingSource.Filter = "(column18 IS NULL OR column18=False) AND column22 IS NULL AND column13 IS NULL AND column09=True";
        }

        private void barrasinashodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table_005_OrderHeaderBindingSource.Filter = "(column18 IS NULL OR column18=False) AND column22 IS NULL AND column09 IS NULL AND column13 IS NULL";

        }

  

        private void gridEX2_SelectionChanged(object sender, EventArgs e)
        {
          
        }

        private void multicombo1_ValueChanged(object sender, EventArgs e)
        {
            try
            {

                if (multicombo1.SelectedIndex == -1)
                {
                    multicombo1.Value = DBNull.Value;
                    editBox11.Text = "";
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
                editBox11.Text = multicombo1.DropDownList.GetValue("name").ToString();
                editBox1.Text = multicombo1.DropDownList.GetValue("ostan").ToString();
                editBox2.Text = multicombo1.DropDownList.GetValue("shahr").ToString();
                editBox3.Text = multicombo1.DropDownList.GetValue("telmoshtari").ToString();
                editBox4.Text = multicombo1.DropDownList.GetValue("faxmoshtari").ToString();
                editBox5.Text = multicombo1.DropDownList.GetValue("codepostimoshtari").ToString();
                editBox6.Text = multicombo1.DropDownList.GetValue("adresmoshtari").ToString();
                editBox7.Text = multicombo1.DropDownList.GetValue("column06").ToString();
                editBox8.Text = multicombo1.DropDownList.GetValue("column07").ToString();
                editBox9.Text = multicombo1.DropDownList.GetValue("column05").ToString();
                editBox10.Text = multicombo1.DropDownList.GetValue("column04").ToString();
            }
            catch
            {
            }

        }

        private void frm_moshahede_sefareshat_mali_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX2.MoveToNewRecord();
                table_005_OrderHeaderBindingSource.EndEdit();
                table_005_OrderHeaderTableAdapter.Update(dataSet_Foroosh.Table_005_OrderHeader);
                Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "None");
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void frm_moshahede_sefareshat_foroosh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                dataSet_Foroosh.EnforceConstraints = true;
                dataSet_Foroosh.EnforceConstraints = false;
                table_005_OrderHeaderTableAdapter.Fill(dataSet_Foroosh.Table_005_OrderHeader);
                table_006_OrderDetailsTableAdapter.Fill(dataSet_Foroosh.Table_006_OrderDetails);
                MessageBox.Show("به روز رسانی اطلاعات با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }
    }
}
