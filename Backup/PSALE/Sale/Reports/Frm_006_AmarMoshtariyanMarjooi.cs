using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSALE.Sale.Reports
{
    public partial class Frm_006_AmarMoshtariyanMarjooi : Form
    {
        db alldatabase = new db();
        
        public Frm_006_AmarMoshtariyanMarjooi()
        {
            InitializeComponent();
        }

        private void Frm_006_AmarMoshtariyanMarjooi_Load(object sender, EventArgs e)
        {
            amarMoshtariyanMarjooiHeaderTableAdapter.Fill(dataSet_Reports.AmarMoshtariyanMarjooiHeader,faDatePickerStrip1.FADatePicker.Text,faDatePickerStrip2.FADatePicker.Text);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarMoshtaryianMarjooiDetailsKala' table. You can move, or remove it, as needed.
            this.amarMoshtaryianMarjooiDetailsKalaTableAdapter.Fill(this.dataSet_Reports.AmarMoshtaryianMarjooiDetailsKala);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarMoshtariyanMarjooiDetailsTakhfifEzafe' table. You can move, or remove it, as needed.
            this.amarMoshtariyanMarjooiDetailsTakhfifEzafeTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanMarjooiDetailsTakhfifEzafe);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarMoshtariyanMarjooiDetailsFaktors' table. You can move, or remove it, as needed.
            this.amarMoshtariyanMarjooiDetailsFaktorsTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanMarjooiDetailsFaktors,faDatePickerStrip1.FADatePicker.Text,faDatePickerStrip2.FADatePicker.Text);
           db.constr=Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();

            gridEX1.DropDowns["d"].DataSource=alldatabase.get_list("SELECT * FROM table_004_CommodityAndIngredients");
            // TODO: This line of code loads data into the 'dataSet_Reports.DaftarForooshDetails' table. You can move, or remove it, as needed.
    
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
 

          }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            amarMoshtariyanMarjooiHeaderTableAdapter.Fill(dataSet_Reports.AmarMoshtariyanMarjooiHeader, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarMoshtaryianMarjooiDetailsKala' table. You can move, or remove it, as needed.
            this.amarMoshtaryianMarjooiDetailsKalaTableAdapter.Fill(this.dataSet_Reports.AmarMoshtaryianMarjooiDetailsKala);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarMoshtariyanMarjooiDetailsTakhfifEzafe' table. You can move, or remove it, as needed.
            this.amarMoshtariyanMarjooiDetailsTakhfifEzafeTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanMarjooiDetailsTakhfifEzafe);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarMoshtariyanMarjooiDetailsFaktors' table. You can move, or remove it, as needed.
            this.amarMoshtariyanMarjooiDetailsFaktorsTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanMarjooiDetailsFaktors,faDatePickerStrip1.FADatePicker.Text,faDatePickerStrip2.FADatePicker.Text);
          
    }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
               amarMoshtariyanMarjooiHeaderBindingSource.MoveLast();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarMoshtariyanMarjooiHeaderBindingSource.MoveNext();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarMoshtariyanMarjooiHeaderBindingSource.MovePrevious();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarMoshtariyanMarjooiHeaderBindingSource.MoveFirst();
            }
            catch
            {

            }
        }

 



    }
}
