using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._04_Buy
{
    public partial class Frm_011_DraftInformationDialog : Form
    {
        public string FunctionValue;
        SqlConnection Conware = new SqlConnection(Properties.Settings.Default.WHRS);
        public Frm_011_DraftInformationDialog()
        {
            InitializeComponent();
        }

        private void Frm_010_DraftInformationDialog_Load(object sender, EventArgs e)
        {
            DataSet DS = new DataSet();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=1", Conware);
            Adapter.Fill(DS, "Fun");
            mlt_Function.DataSource = DS.Tables["Fun"];

        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Function.Text.Trim() == "")
                    throw new Exception("اطلاعات مورد نیاز را تکمیل کنید");

                FunctionValue = mlt_Function.Value.ToString();
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

       

        private void mlt_Function_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
