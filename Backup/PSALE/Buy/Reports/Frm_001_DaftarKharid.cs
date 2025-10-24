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
    public partial class Frm_001_DaftarKharid : Form
    {
        db alldatabase = new db();
        
        public Frm_001_DaftarKharid()
        {
            InitializeComponent();
        }

        private void Frm_001_DaftarKharid_Load(object sender, EventArgs e)
        {

            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            toolStripComboBox1.ComboBox.SelectedIndex = 0;
            daftarKharidHeaderTableAdapter.Fill(dataSet_Reports.DaftarKharidHeader, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
            daftarKharidDetailsTableAdapter.Fill(dataSet_Reports.DaftarKharidDetails);
            daftareKharidTakhfifEzafeTableAdapter.Fill(dataSet_Reports.DaftareKharidTakhfifEzafe);
            if (toolStripComboBox1.ComboBox.SelectedIndex == 0)
            {
                daftarKharidHeaderBindingSource.Filter = "Batel=0";
            }
            else
            {
                daftarKharidHeaderBindingSource.Filter = "Batel=1";
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            daftarKharidHeaderTableAdapter.Fill(dataSet_Reports.DaftarKharidHeader, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
            daftarKharidDetailsTableAdapter.Fill(dataSet_Reports.DaftarKharidDetails);
            daftareKharidTakhfifEzafeTableAdapter.Fill(dataSet_Reports.DaftareKharidTakhfifEzafe);
            if (toolStripComboBox1.ComboBox.SelectedIndex == 0)
            {
                daftarKharidHeaderBindingSource.Filter = "Batel=0";
            }
            else
            {
                daftarKharidHeaderBindingSource.Filter = "Batel=1";
            }

        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                daftarKharidHeaderBindingSource.MoveLast();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                daftarKharidHeaderBindingSource.MoveNext();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                daftarKharidHeaderBindingSource.MovePrevious();
            }
            catch
            {

            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                daftarKharidHeaderBindingSource.MoveFirst();
            }
            catch
            {

            }
        }




    }
}
