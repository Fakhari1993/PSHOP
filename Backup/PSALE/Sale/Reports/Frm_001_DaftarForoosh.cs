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
    public partial class Frm_001_DaftarForoosh : Form
    {
        db alldatabase = new db();
        
        public Frm_001_DaftarForoosh()
        {
            InitializeComponent();
        }

        private void Frm_001_DaftarForoosh_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_Reports.DaftareForooshTakhfifEzafe' table. You can move, or remove it, as needed.
            this.daftareForooshTakhfifEzafeTableAdapter.Fill(this.dataSet_Reports.DaftareForooshTakhfifEzafe);
            db.constr=Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();

            gridEX1.DropDowns["d"].DataSource=alldatabase.get_list("SELECT * FROM table_004_CommodityAndIngredients");
            // TODO: This line of code loads data into the 'dataSet_Reports.DaftarForooshDetails' table. You can move, or remove it, as needed.
            toolStripComboBox1.SelectedIndex = 0;
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            daftarForooshHeaderTableAdapter.Fill(dataSet_Reports.DaftarForooshHeader, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
                      this.daftarForooshDetailsTableAdapter.Fill(this.dataSet_Reports.DaftarForooshDetails);
 if(toolStripComboBox1.SelectedIndex==0)
            daftarForooshHeaderBindingSource.Filter = "Batel=False";
           else
               daftarForooshHeaderBindingSource.Filter = "Batel=True";
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            daftarForooshHeaderTableAdapter.Fill(dataSet_Reports.DaftarForooshHeader, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
            if (toolStripComboBox1.SelectedIndex == 0)
                daftarForooshHeaderBindingSource.Filter = "Batel=False";
            else
                daftarForooshHeaderBindingSource.Filter = "Batel=True";
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                daftarForooshHeaderBindingSource.MoveLast();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                daftarForooshHeaderBindingSource.MoveNext();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                daftarForooshHeaderBindingSource.MovePrevious();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                daftarForooshHeaderBindingSource.MoveFirst();
            }
            catch
            {

            }
        }


    }
}
