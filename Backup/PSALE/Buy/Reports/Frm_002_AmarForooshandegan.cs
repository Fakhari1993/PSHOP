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
    public partial class Frm_002_AmarMoshtariyan : Form
    {
        db alldatabase = new db();
        
        public Frm_002_AmarMoshtariyan()
        {
            InitializeComponent();
        }

        private void Frm_002_AmarMoshtariyan_Load(object sender, EventArgs e)
        {
            toolStripComboBox1.SelectedIndex = 0;
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            amarForooshandeganHeaderTableAdapter.Fill(DataSet_Buy_Reports.AmarForooshandeganHeader, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, false);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarForooshandeganDetailsKala' table. You can move, or remove it, as needed.
            this.amarForooshandeganDetailsKalaTableAdapter.Fill(this.DataSet_Buy_Reports.AmarForooshandeganDetailsKala);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarForooshandeganDetailsTakhfifEzafe' table. You can move, or remove it, as needed.
            this.amarForooshandeganDetailsTakhfifEzafeTableAdapter.Fill(this.DataSet_Buy_Reports.AmarForooshandeganDetailsTakhfifEzafe);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarForooshandeganDetailsFaktors' table. You can move, or remove it, as needed.
            this.amarForooshandeganDetailsFaktorsTableAdapter.Fill(this.DataSet_Buy_Reports.AmarForooshandeganDetailsFaktors,false,faDatePickerStrip1.FADatePicker.Text,faDatePickerStrip2.FADatePicker.Text);
           db.constr=Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();

            gridEX1.DropDowns["d"].DataSource=alldatabase.get_list("SELECT * FROM table_004_CommodityAndIngredients");
            // TODO: This line of code loads data into the 'dataSet_Reports.DaftarForooshDetails' table. You can move, or remove it, as needed.
    

          }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            bool batel;
            if (toolStripComboBox1.SelectedIndex == 0)
                batel = false;
            else
                batel = true;

            amarForooshandeganHeaderTableAdapter.Fill(DataSet_Buy_Reports.AmarForooshandeganHeader, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, batel);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarForooshandeganDetailsKala' table. You can move, or remove it, as needed.
            this.amarForooshandeganDetailsKalaTableAdapter.Fill(this.DataSet_Buy_Reports.AmarForooshandeganDetailsKala);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarForooshandeganDetailsTakhfifEzafe' table. You can move, or remove it, as needed.
            this.amarForooshandeganDetailsTakhfifEzafeTableAdapter.Fill(this.DataSet_Buy_Reports.AmarForooshandeganDetailsTakhfifEzafe);
            // TODO: This line of code loads data into the 'dataSet_Reports.AmarForooshandeganDetailsFaktors' table. You can move, or remove it, as needed.
            this.amarForooshandeganDetailsFaktorsTableAdapter.Fill(this.DataSet_Buy_Reports.AmarForooshandeganDetailsFaktors,batel,faDatePickerStrip1.FADatePicker.Text,faDatePickerStrip2.FADatePicker.Text);
         
         }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarForooshandeganHeaderBindingSource.MoveLast();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarForooshandeganHeaderBindingSource.MoveNext();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarForooshandeganHeaderBindingSource.MovePrevious();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarForooshandeganHeaderBindingSource.MoveFirst();
            }
            catch
            {

            }
        }





    }
}
