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
    public partial class Frm_025_SaleDefaultDescription : DevComponents.DotNetBar.OfficeForm
    {
        public Frm_025_SaleDefaultDescription()
        {
            InitializeComponent();
        }

        private void Frm_025_SaleDefaultDescription_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.SaleDescription = richTextBox1.Text.Trim();
            Properties.Settings.Default.Save();
        }

        private void Frm_025_SaleDefaultDescription_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = Properties.Settings.Default.SaleDescription;
        }
    }
}