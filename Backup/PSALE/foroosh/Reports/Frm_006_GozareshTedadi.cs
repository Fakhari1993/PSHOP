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
    public partial class Frm_006_GozareshTedadi : Form
    {
        public Frm_006_GozareshTedadi()
        {
            InitializeComponent();
        }



        private void fillToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            gozareshTedadiTableAdapter.Fill(datatSet_Sefareshat.GozareshTedadi, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
        }


        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                gozareshTedadiBindingSource.EndEdit();
                gozareshTedadiBindingSource.MoveLast();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                gozareshTedadiBindingSource.EndEdit();
                gozareshTedadiBindingSource.MoveNext();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                gozareshTedadiBindingSource.EndEdit();
                gozareshTedadiBindingSource.MovePrevious();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                gozareshTedadiBindingSource.EndEdit();
                gozareshTedadiBindingSource.MoveFirst();
            }
            catch
            {
            }
        }

        private void Frm_006_GozareshTedadi_Load_1(object sender, EventArgs e)
        {
            db database = new db();



            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;






            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            database.close();
            database.Connect();

            gridEX2.DropDowns["d"].DataSource = database.get_list("SELECT columnid,column01,column02 FROM table_004_CommodityAndIngredients");
            gridEX2.DropDowns["d2"].DataSource = database.get_list("SELECT columnid,column01,column02 FROM table_004_CommodityAndIngredients");

            try
            {
                gozareshTedadiTableAdapter.Fill(datatSet_Sefareshat.GozareshTedadi, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
            }
            catch
            {
            }
        }

   


    }
}
