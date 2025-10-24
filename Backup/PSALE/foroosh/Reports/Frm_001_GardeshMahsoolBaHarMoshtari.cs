using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSALE.foroosh.Reports
{
    public partial class Frm_001_GardeshMahsoolBaHarMoshtari : Form
    {
        public Frm_001_GardeshMahsoolBaHarMoshtari()
        {
            InitializeComponent();
        }

        private void Frm_001_GardeshMahsoolBaHarMoshtari_Load(object sender, EventArgs e)
        {

            db.constr = Properties.Settings.Default.PERP_ConnectionString;
            db database = new db();
            database.Connect();


            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;



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


            s = string.Format(s, table_005_OrderHeaderTableAdapter1.Connection.Database.ToString());
column03TextBox.DataSource = database.get_list(s);

            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            database.close();
            database.Connect();
            
            gridEX2.DropDowns["d"].DataSource = database.get_list("SELECT columnid,column01,column02 FROM table_004_CommodityAndIngredients");
            gridEX2.DropDowns["d2"].DataSource = database.get_list("SELECT columnid,column01,column02 FROM table_004_CommodityAndIngredients");

            mahsoolBaMoshtariTableAdapter.Fill(datatSet_Sefareshat.MahsoolBaMoshtari, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, int.Parse(column03TextBox.Value.ToString()));
            
        
        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            mahsoolBaMoshtariTableAdapter.Fill(datatSet_Sefareshat.MahsoolBaMoshtari, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, int.Parse(column03TextBox.Value.ToString()));

        }

        private void column03TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            column03TextBox.DroppedDown = true;
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                mahsoolBaMoshtariBindingSource.EndEdit();
                mahsoolBaMoshtariBindingSource.MoveLast();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                mahsoolBaMoshtariBindingSource.EndEdit();
                mahsoolBaMoshtariBindingSource.MoveNext();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                mahsoolBaMoshtariBindingSource.EndEdit();
                mahsoolBaMoshtariBindingSource.MovePrevious();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                mahsoolBaMoshtariBindingSource.EndEdit();
                mahsoolBaMoshtariBindingSource.MoveFirst();
            }
            catch
            {
            }
        }
    }
}
