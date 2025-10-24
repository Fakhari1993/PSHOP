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
    public partial class Frm_002_GardeshMoshtariBaHarMahsool : Form
    {
        public Frm_002_GardeshMoshtariBaHarMahsool()
        {
            InitializeComponent();
        }

        private void Frm_002_GardeshMoshtariBaHarMahsool_Load(object sender, EventArgs e)
        {

            db.constr = Properties.Settings.Default.PERP_ConnectionString;
            db data = new db();
            data.close();
            data.Connect();
            

  string s= @"SELECT     dbo.Table_045_PersonInfo.ColumnId AS ColumnId, dbo.Table_045_PersonInfo.Column01 AS Column01, dbo.Table_045_PersonInfo.Column02 AS Column02, 
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
gridEX2.DropDowns["d"].DataSource = data.get_list(s);

            gridEX2.DropDowns["d2"].DataSource = gridEX2.DropDowns["d"].DataSource;
            gridEX2.DropDowns["d3"].DataSource = gridEX2.DropDowns["d"].DataSource;
            gridEX2.DropDowns["d4"].DataSource = gridEX2.DropDowns["d"].DataSource;

            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            db database = new db();
            database.Connect();


            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;


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


            s = string.Format(s, table_005_OrderHeaderTableAdapter1.Connection.Database.ToString());
column03TextBox.DataSource = database.get_list(s);

            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            database.close();
            database.Connect();
            
           

            moshtariBaMahsoolTableAdapter.Fill(datatSet_Sefareshat.MoshtariBaMahsool, int.Parse(column03TextBox.Value.ToString()), faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
            
        
        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            moshtariBaMahsoolTableAdapter.Fill(datatSet_Sefareshat.MoshtariBaMahsool, int.Parse(column03TextBox.Value.ToString()), faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);

        }

        private void column03TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            column03TextBox.DroppedDown = true;
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                moshtariBaMahsoolBindingSource.EndEdit();
                moshtariBaMahsoolBindingSource.MoveLast();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                moshtariBaMahsoolBindingSource.EndEdit();
                moshtariBaMahsoolBindingSource.MoveNext();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                moshtariBaMahsoolBindingSource.EndEdit();
                moshtariBaMahsoolBindingSource.MovePrevious();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                moshtariBaMahsoolBindingSource.EndEdit();
                moshtariBaMahsoolBindingSource.MoveFirst();
            }
            catch
            {
            }
        }


    }
}
