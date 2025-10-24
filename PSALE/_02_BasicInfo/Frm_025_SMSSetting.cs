using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._02_BasicInfo
{
    public partial class Frm_025_SMSSetting : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);

        public Frm_025_SMSSetting()
        {
            InitializeComponent();
        }

        private void Frm_025_SMSSetting_Load(object sender, EventArgs e)
        {
            this.table_220_SMSTextTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_220_SMSText);
            this.table_175_SMSTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_175_SMS);
            gridEX_SmsText.DropDowns["Line"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Line from Table_175_SMS");
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                this.table_175_SMSBindingSource.EndEdit();
                this.table_175_SMSTableAdapter.Update(dataSet_EtelaatPaye.Table_175_SMS);

                this.table_220_SMSTextBindingSource.EndEdit();
                this.table_220_SMSTextTableAdapter.Update(dataSet_EtelaatPaye.Table_220_SMSText);

                Class_BasicOperation.ShowMsg("", "ذخیره اطلاعات با موفقیت صورت گرفت", "Information");
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;

            }
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_ColumnButtonClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (this.table_175_SMSBindingSource.Count > 0)
            {
                try
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف اطلاعات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        this.table_175_SMSBindingSource.RemoveCurrent();
                        this.table_175_SMSBindingSource.EndEdit();
                        this.table_175_SMSTableAdapter.Update(dataSet_EtelaatPaye.Table_175_SMS);
                        Class_BasicOperation.ShowMsg("", "حذف اطلاعات با موفقیت انجام گرفت", "Information");
                    }
                }
                catch (Exception ex)
                {
                    table_175_SMSTableAdapter.Fill(dataSet_EtelaatPaye.Table_175_SMS);
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }

        private void Frm_025_SMSSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
        }
    }
}
