using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace PSHOP._05_Sale
{
    public partial class Frm_026_PrefactorDefaultDescription : DevComponents.DotNetBar.OfficeForm
    {
        public Frm_026_PrefactorDefaultDescription()
        {
            InitializeComponent();
        }

        private void Frm_025_SaleDefaultDescription_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.PrefactorDescription = richTextBox1.Text.Trim();
            Properties.Settings.Default.Save();
        }

        private void Frm_025_SaleDefaultDescription_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = Properties.Settings.Default.PrefactorDescription;
        }
    }
}