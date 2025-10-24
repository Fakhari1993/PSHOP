using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Janus.Windows.GridEX;

namespace PSHOP._02_BasicInfo.Takhfifat
{
    public partial class Frm_002_TakhfifVijheMoshtari : Form
    {
        bool _BackSpace = false;
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);

        public Frm_002_TakhfifVijheMoshtari()
        {
            InitializeComponent();
        }

        private void Frm_002_TakhfifVijheMoshtari_Load(object sender, EventArgs e)
        {
            try
            {
                faDatePicker1.SelectedDateTime = DateTime.Now.AddMonths(-1);
                faDatePicker2.SelectedDateTime = DateTime.Now;

                DataSet DS = new DataSet();
                SqlDataAdapter Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
                Adapter.Fill(DS, "PersonInfo");
                gridEX_Customers.DropDowns["PersonCode"].SetDataBinding(DS.Tables["PersonInfo"], "");
                gridEX_Customers.DropDowns["PersonName"].SetDataBinding(DS.Tables["PersonInfo"], "");
                gridEX_Customers.DropDowns["Group"].SetDataBinding(dataSet_001_Takhfif, "Table_040_PersonGroups");
                gridEX_Discounts.DropDowns["PersonCode"].SetDataBinding(DS.Tables["PersonInfo"], "");
                gridEX_Discounts.DropDowns["PersonName"].SetDataBinding(DS.Tables["PersonInfo"], "");
                try
                {
                    this.table_040_PersonGroupsTableAdapter.Fill(this.dataSet_001_Takhfif.Table_040_PersonGroups);
                    this.table_045_PersonScopeTableAdapter.Fill(this.dataSet_001_Takhfif.Table_045_PersonScope);
                    this.table_026_Discount_SpecialCustomerTableAdapter.Fill(this.dataSet_001_Takhfif.Table_026_Discount_SpecialCustomer);
                }
                catch
                {
                }
            }
            catch
            { }
        
        }

        private void mlt_CustomerGroup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else Class_BasicOperation.isEnter(e.KeyChar);
            }
            else if (sender is Janus.Windows.GridEX.EditControls.EditBox)
            {
               if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar!=46)
                    e.Handled = true;
                else Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void txt_FromPrice_TextChanged(object sender, EventArgs e)
        {
            decimal Num;
            if (decimal.TryParse(((Janus.Windows.GridEX.EditControls.EditBox)sender).Text , out Num))
            {
                ((Janus.Windows.GridEX.EditControls.EditBox)sender).Text = string.Format("{0:N0}", Num);
                ((Janus.Windows.GridEX.EditControls.EditBox)sender).SelectionStart = ((Janus.Windows.GridEX.EditControls.EditBox)sender).Text.Length;
            }
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
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;

            Class_BasicOperation.isEnter(e.KeyChar);

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void bt_Do_Click(object sender, EventArgs e)
        {
            try
            {
                if (faDatePicker1.SelectedDateTime.HasValue && faDatePicker2.SelectedDateTime.HasValue && faDatePicker1.Text.Length == 10 && faDatePicker2.Text.Length == 10)
                {
                    if (txt_FromPrice.Text.Trim() != "" && txt_ToPrice.Text.Trim() != "" && txt_Percent.Text.Trim() != "" && gridEX_Customers.GetCheckedRows().Length>0)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به اعمال تخفیف به افراد انتخاب شده در گروهها هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            foreach (GridEXRow item in gridEX_Customers.GetCheckedRows())
                            {
                                this.table_026_Discount_SpecialCustomerBindingSource.AddNew();
                                DataRowView NewRow = (DataRowView)this.table_026_Discount_SpecialCustomerBindingSource.CurrencyManager.Current;
                                NewRow["Column01"] = item.Cells["Column01"].Value.ToString();
                                NewRow["Column02"] = faDatePicker1.Text;
                                NewRow["Column03"] = faDatePicker2.Text;
                                NewRow["Column04"] = txt_FromPrice.Text.Replace(",", "");
                                NewRow["Column05"] = txt_ToPrice.Text.Replace(",", "");
                                NewRow["Column06"] = txt_Percent.Text;
                                NewRow["Column07"] = Class_BasicOperation._UserName;
                                NewRow["Column08"] = Class_BasicOperation.ServerDate();
                                NewRow["Column09"] = Class_BasicOperation._UserName;
                                NewRow["Column10"] = Class_BasicOperation.ServerDate();
                                NewRow["Column11"] = item.Cells["Column02"].Value.ToString();
                                this.table_026_Discount_SpecialCustomerBindingSource.EndEdit();
                                this.table_026_Discount_SpecialCustomerTableAdapter.Update(dataSet_001_Takhfif.Table_026_Discount_SpecialCustomer);


                            }
                            Class_BasicOperation.ShowMsg("", "ذخیره تغییرات با موفقیت انجام گرفت", "Information");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void mnu_SelectAll_Click(object sender, EventArgs e)
        {
            GridEXRow groupRow = gridEX_Customers.CurrentRow;
            if (groupRow.RowType == RowType.GroupHeader)
            {
                GridEXRow[] childRows = groupRow.GetChildRecords();
                foreach (GridEXRow child in childRows)
                {
                    child.BeginEdit();
                    child.CheckState = RowCheckState.Checked;
                    child.EndEdit();
                }
            }
        }

        private void mnu_DeselectAll_Click(object sender, EventArgs e)
        {
            GridEXRow groupRow = gridEX_Customers.CurrentRow;
            if (groupRow.RowType == RowType.GroupHeader)
            {
                GridEXRow[] childRows = groupRow.GetChildRecords();
                foreach (GridEXRow child in childRows)
                {
                    child.BeginEdit();
                    child.CheckState = RowCheckState.Unchecked;
                    child.EndEdit();
                }
            }
        }

        private void Frm_002_TakhfifVijheMoshtari_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePicker1.Select();
            else if (e.Control && e.KeyCode == Keys.S)
                bt_Do_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.E)
                bt_Edit_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Delete_Click(sender, e);
        }

      
        private void bt_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (faDatePicker1.SelectedDateTime.HasValue && faDatePicker2.SelectedDateTime.HasValue && faDatePicker1.Text.Length == 10 && faDatePicker2.Text.Length == 10)
                {
                    if (txt_FromPrice.Text.Trim() != "" && txt_ToPrice.Text.Trim() != "" && txt_Percent.Text.Trim() != "" && gridEX_Customers.GetCheckedRows().Length>0)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به اعمال تغییرات تخفیفات موجود در این بازه زمانی، برای اشخاص انتخاب شده هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE))
                            {
                                ConSale.Open();
                                foreach (GridEXRow item in gridEX_Customers.GetCheckedRows())
                                {
                                    SqlCommand Update = new SqlCommand("UPDATE Table_026_Discount_SpecialCustomer  SET Column02='" + faDatePicker1.Text
                                        + "' , Column03='" + faDatePicker2.Text + "' , Column04=" + txt_FromPrice.Text.Replace(",", "") + " , Column05=" + txt_ToPrice.Text.Replace(",", "")
                                        + " , Column06=" + txt_Percent.Text + " , Column09='" + Class_BasicOperation._UserName + "' , Column10=getdate() where Column01=" + item.Cells["Column01"].Value.ToString()
                                        + " and Column11=" + item.Cells["Column02"].Value.ToString() + " and Column02='" + faDatePicker1.Text + "' and Column03='" + faDatePicker2.Text + "'", ConSale);
                                    Update.ExecuteNonQuery();
                                }
                            }
                            try
                            {
                                dataSet_001_Takhfif.EnforceConstraints = false;
                                this.table_045_PersonScopeTableAdapter.Fill(dataSet_001_Takhfif.Table_045_PersonScope);
                                this.table_026_Discount_SpecialCustomerTableAdapter.Fill(dataSet_001_Takhfif.Table_026_Discount_SpecialCustomer);
                                dataSet_001_Takhfif.EnforceConstraints = true;
                            }
                            catch
                            {
                            }
                            Class_BasicOperation.ShowMsg("", "ذخیره تغییرات با موفقیت انجام گرفت", "Information");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

     

        private void bt_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (faDatePicker1.SelectedDateTime.HasValue && faDatePicker2.SelectedDateTime.HasValue && faDatePicker1.Text.Length == 10 && faDatePicker2.Text.Length == 10)
                {
                    if (gridEX_Customers.GetCheckedRows().Length > 0)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف تخفیفات معرفی شده در این بازه زمانی، برای اشخاص انتخاب شده هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE))
                            {
                                ConSale.Open();
                                foreach (GridEXRow item in gridEX_Customers.GetCheckedRows())
                                {
                                    SqlCommand Delete = new SqlCommand("Delete From Table_026_Discount_SpecialCustomer  where Column01=" + item.Cells["Column01"].Value.ToString()
                                        + " and Column11=" + item.Cells["Column02"].Value.ToString() + " and Column02='" + faDatePicker1.Text + "' and Column03='" + faDatePicker2.Text + "'", ConSale);
                                    Delete.ExecuteNonQuery();
                                }
                            }
                            try
                            {
                                dataSet_001_Takhfif.EnforceConstraints = false;
                                this.table_045_PersonScopeTableAdapter.Fill(dataSet_001_Takhfif.Table_045_PersonScope);
                                this.table_026_Discount_SpecialCustomerTableAdapter.Fill(dataSet_001_Takhfif.Table_026_Discount_SpecialCustomer);
                                dataSet_001_Takhfif.EnforceConstraints = true;
                            }
                            catch
                            {
                            }
                            Class_BasicOperation.ShowMsg("", "حذف تخفیفات با موفقیت انجام گرفت", "Information");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

    }
}
