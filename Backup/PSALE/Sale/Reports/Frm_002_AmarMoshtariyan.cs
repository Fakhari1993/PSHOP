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
    public partial class Frm_002_AmarMoshtariyan : Form
    {
        db alldatabase = new db();
        
        public Frm_002_AmarMoshtariyan()
        {
            InitializeComponent();
        }

        private void Frm_002_AmarMoshtariyan_Load(object sender, EventArgs e)
        {
           db.constr=Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();

            gridEX1.DropDowns["d"].DataSource=alldatabase.get_list("SELECT * FROM table_004_CommodityAndIngredients");
            // TODO: This line of code loads data into the 'dataSet_Reports.DaftarForooshDetails' table. You can move, or remove it, as needed.
            toolStripComboBox1.SelectedIndex = 0;
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            if (toolStripComboBox1.SelectedIndex == 0)
            {
                this.amarMoshtariyanHeaderTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanHeader, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, false);
                this.amarMoshtariyanDetailsFaktorsTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanDetailsFaktors,false,faDatePickerStrip1.FADatePicker.Text,faDatePickerStrip2.FADatePicker.Text);
            }
            else
            {
                this.amarMoshtariyanHeaderTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanHeader, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, true);
                this.amarMoshtariyanDetailsFaktorsTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanDetailsFaktors,true,faDatePickerStrip1.FADatePicker.Text,faDatePickerStrip2.FADatePicker.Text);
            }
            
            amarMoshtariyanDetailsKalaTableAdapter.Fill(dataSet_Reports.AmarMoshtariyanDetailsKala);
            amarMoshtariyanDetailsTakhfifEzafeTableAdapter.Fill(dataSet_Reports.AmarMoshtariyanDetailsTakhfifEzafe);
           
          }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            if (toolStripComboBox1.SelectedIndex == 0)
            {
                this.amarMoshtariyanHeaderTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanHeader, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, false);
                this.amarMoshtariyanDetailsFaktorsTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanDetailsFaktors, false,faDatePickerStrip1.FADatePicker.Text,faDatePickerStrip2.FADatePicker.Text);
            }
            else
            {
                this.amarMoshtariyanHeaderTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanHeader, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, true);
                this.amarMoshtariyanDetailsFaktorsTableAdapter.Fill(this.dataSet_Reports.AmarMoshtariyanDetailsFaktors,true,faDatePickerStrip1.FADatePicker.Text,faDatePickerStrip2.FADatePicker.Text);
            }



            amarMoshtariyanDetailsKalaTableAdapter.Fill(dataSet_Reports.AmarMoshtariyanDetailsKala);
            amarMoshtariyanDetailsTakhfifEzafeTableAdapter.Fill(dataSet_Reports.AmarMoshtariyanDetailsTakhfifEzafe);
            if (toolStripComboBox1.SelectedIndex == 0)
                amarMoshtariyanDetailsFaktorsBindingSource.Filter = "Batel=False";
            else
                amarMoshtariyanDetailsFaktorsBindingSource.Filter = "Batel=True";
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarMoshtariyanHeaderBindingSource.MoveLast();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarMoshtariyanHeaderBindingSource.MoveNext();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarMoshtariyanHeaderBindingSource.MovePrevious();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                amarMoshtariyanHeaderBindingSource.MoveFirst();
            }
            catch
            {

            }
        }



    }
}
