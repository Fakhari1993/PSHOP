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
    public partial class Frm_003_GardeshMahsoolDarHarOstan : Form
    {
        public Frm_003_GardeshMahsoolDarHarOstan()
        {
            InitializeComponent();
        }

        private void Frm_003_GardeshMahsoolDarHarOstan_Load(object sender, EventArgs e)
        {

            db.constr = Properties.Settings.Default.PERP_ConnectionString;
            db database = new db();
            database.Connect();


            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;



            string s = "SELECT * FROM Table_060_ProvinceInfo";


           
               column03TextBox.DataSource = database.get_list(s);

            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            database.close();
            database.Connect();
            
            gridEX2.DropDowns["d"].DataSource = database.get_list("SELECT columnid,column01,column02 FROM table_004_CommodityAndIngredients");
            gridEX2.DropDowns["d2"].DataSource = database.get_list("SELECT columnid,column01,column02 FROM table_004_CommodityAndIngredients");

            try
            {
                ostanBaHarMahsoolTableAdapter.Fill(datatSet_Sefareshat.OstanBaHarMahsool, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, int.Parse(column03TextBox.Value.ToString()));
            }
            catch
            {
            }
        
        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ostanBaHarMahsoolTableAdapter.Fill(datatSet_Sefareshat.OstanBaHarMahsool, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, int.Parse(column03TextBox.Value.ToString()));

        }

        private void column03TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            column03TextBox.DroppedDown = true;
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ostanBaHarMahsoolBindingSource.EndEdit();
                ostanBaHarMahsoolBindingSource.MoveLast();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ostanBaHarMahsoolBindingSource.EndEdit();
                ostanBaHarMahsoolBindingSource.MoveNext();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ostanBaHarMahsoolBindingSource.EndEdit();
                ostanBaHarMahsoolBindingSource.MovePrevious();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ostanBaHarMahsoolBindingSource.EndEdit();
                ostanBaHarMahsoolBindingSource.MoveFirst();
            }
            catch
            {
            }
        }


    }
}
