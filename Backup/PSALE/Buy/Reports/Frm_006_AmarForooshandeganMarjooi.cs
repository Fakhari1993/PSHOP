using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSALE.Buy.Reports
{
    public partial class Frm_006_AmarForooshandeganMarjooi : Form
    {
        db alldatabase = new db();
        
        public Frm_006_AmarForooshandeganMarjooi()
        {
            InitializeComponent();
        }

        private void Frm_006_AmarForooshandeganMarjooi_Load(object sender, EventArgs e)
        {
            amarForooshandeganMarjooiTableAdapter.Fill(dataSet_Buy_Reports.AmarForooshandeganMarjooi, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
            // TODO: This line of code loads data into the 'dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsKala' table. You can move, or remove it, as needed.
            this.amarForooshandeganMarjooiDetailsKalaTableAdapter.Fill(this.dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsKala);
            // TODO: This line of code loads data into the 'dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsTakhfifEzafe' table. You can move, or remove it, as needed.
            this.amarForooshandeganMarjooiDetailsTakhfifEzafeTableAdapter.Fill(this.dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsTakhfifEzafe);
            // TODO: This line of code loads data into the 'dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsFaktors' table. You can move, or remove it, as needed.
            this.amarForooshandeganMarjooiDetailsFaktorsTableAdapter.Fill(this.dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsFaktors,faDatePickerStrip1.FADatePicker.Text,faDatePickerStrip2.FADatePicker.Text);
           
            
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

            amarForooshandeganMarjooiTableAdapter.Fill(dataSet_Buy_Reports.AmarForooshandeganMarjooi, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
            // TODO: This line of code loads data into the 'dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsKala' table. You can move, or remove it, as needed.
            this.amarForooshandeganMarjooiDetailsKalaTableAdapter.Fill(this.dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsKala);
            // TODO: This line of code loads data into the 'dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsTakhfifEzafe' table. You can move, or remove it, as needed.
            this.amarForooshandeganMarjooiDetailsTakhfifEzafeTableAdapter.Fill(this.dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsTakhfifEzafe);
            // TODO: This line of code loads data into the 'dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsFaktors' table. You can move, or remove it, as needed.
            this.amarForooshandeganMarjooiDetailsFaktorsTableAdapter.Fill(this.dataSet_Buy_Reports.AmarForooshandeganMarjooiDetailsFaktors, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
     
    

    }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarForooshandeganMarjooiBindingSource.MoveLast();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarForooshandeganMarjooiBindingSource.MoveNext();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarForooshandeganMarjooiBindingSource.MovePrevious();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarForooshandeganMarjooiBindingSource.MoveFirst();
            }
            catch
            {

            }
        }



 



    }
}
