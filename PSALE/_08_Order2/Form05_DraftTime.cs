using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSHOP._08_Order2
{
    public partial class Form05_DraftTime : Form
    {
        public string Date = null;
        public string Cause = null;
        bool _BackSpace = false;

        public Form05_DraftTime()
        {
            InitializeComponent();
        }

        private void Form05_CancelOrdersDialog_Load(object sender, EventArgs e)
        {
            faDatePicker1.Select();
            faDatePicker1.SelectedDateTime = DateTime.Now;
        }

        private void bt_Yes_Click(object sender, EventArgs e)
        {
            if (faDatePicker1.Text.Trim() != "" )
            {
              
                Date = faDatePicker1.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                this.Close();
            }
        }

        private void bt_No_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = (FarsiLibrary.Win.Controls.FADatePicker)sender;


                if (textBox.Text.Length == 4)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                else if (textBox.Text.Length == 7)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker1.HideDropDown();
                    bt_Yes.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }
    }
}
