using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSHOP._02_BasicInfo.Takhfifat
{
    public partial class Frm_001_TakhfifatGoroohMoshtari : Form
    {
        public Frm_001_TakhfifatGoroohMoshtari()
        {
            InitializeComponent();
        }

        private void Frm_001_TakhfifatGoroohMoshtari_Load(object sender, EventArgs e)
        {
            this.table_040_PersonGroupsTableAdapter.Fill(this.dataSet_001_Takhfif.Table_040_PersonGroups);
            this.table_025_Discount_Customer_GroupTableAdapter.Fill(this.dataSet_001_Takhfif.Table_025_Discount_Customer_Group);
            this.table_025_Discount_Customer_Group1TableAdapter.Fill(this.dataSet_001_Takhfif.Table_025_Discount_Customer_Group1);

        }

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "");
        }

        private void gridEX3_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "");
        }

        private void gridEX2_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
                gridEX2.SetValue("column08", Class_BasicOperation._UserName);
            }
            catch
            { }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                table_025_Discount_Customer_Group1BindingSource.EndEdit();
                table_025_Discount_Customer_GroupBindingSource.EndEdit();
                table_025_Discount_Customer_Group1TableAdapter.Update(dataSet_001_Takhfif.Table_025_Discount_Customer_Group1);
                table_025_Discount_Customer_GroupTableAdapter.Update(dataSet_001_Takhfif.Table_025_Discount_Customer_Group);
                Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "Information");
            }

            catch(Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void gridEX3_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
                gridEX3.SetValue("column08", Class_BasicOperation._UserName);
            }
            catch
            {
            }
        }

        private void gridEX3_Error_1(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void Frm_001_TakhfifatGoroohMoshtari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
            {
                bt_Save_Click(sender, e);
            }
        }

 

     
    }
}
