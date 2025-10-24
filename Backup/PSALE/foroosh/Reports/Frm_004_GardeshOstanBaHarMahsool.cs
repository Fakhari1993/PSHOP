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
    public partial class Frm_004_GardeshOstanBaHarMahsool : Form
    {
        public Frm_004_GardeshOstanBaHarMahsool()
        {
            InitializeComponent();
        }

        private void Frm_004_GardeshOstanBaHarMahsool_Load(object sender, EventArgs e)
        {


            

 

            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            db database = new db();
            database.Connect();


            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;


string s = @"SELECT     dbo.table_004_CommodityAndIngredients.columnid AS id, dbo.table_004_CommodityAndIngredients.column01 AS code, 
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
            
           

            ostanhaBaHArMahsoolTableAdapter.Fill(datatSet_Sefareshat.OstanhaBaHArMahsool, int.Parse(column03TextBox.Value.ToString()), faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
            
        
        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ostanhaBaHArMahsoolTableAdapter.Fill(datatSet_Sefareshat.OstanhaBaHArMahsool, int.Parse(column03TextBox.Value.ToString()), faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);

        }

        private void column03TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            column03TextBox.DroppedDown = true;
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ostanhaBaHArMahsoolBindingSource.EndEdit();
                ostanhaBaHArMahsoolBindingSource.MoveLast();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ostanhaBaHArMahsoolBindingSource.EndEdit();
                ostanhaBaHArMahsoolBindingSource.MoveNext();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ostanhaBaHArMahsoolBindingSource.EndEdit();
                ostanhaBaHArMahsoolBindingSource.MovePrevious();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ostanhaBaHArMahsoolBindingSource.EndEdit();
                ostanhaBaHArMahsoolBindingSource.MoveFirst();
            }
            catch
            {
            }
        }




    }
}
