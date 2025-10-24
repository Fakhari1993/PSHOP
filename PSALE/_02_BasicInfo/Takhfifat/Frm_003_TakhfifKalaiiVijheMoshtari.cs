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
    public partial class Frm_003_TakhfifKalaiiVijheMoshtari : Form
    {
        bool _BackSpace = false;
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);

        public Frm_003_TakhfifKalaiiVijheMoshtari()
        {
            InitializeComponent();
        }

        private void Frm_003_TakhfifKalaiiVijheMoshtari_Load(object sender, EventArgs e)
        {

            faDatePicker1.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePicker2.SelectedDateTime = DateTime.Now;

            DataSet DS = new DataSet();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
            Adapter.Fill(DS, "PersonInfo");
            gridEX_Customers.DropDowns["PersonCode"].SetDataBinding(DS.Tables["PersonInfo"], "");
            gridEX_Customers.DropDowns["PersonName"].SetDataBinding(DS.Tables["PersonInfo"], "");
            gridEX_All.DropDowns["PersonCode"].SetDataBinding(DS.Tables["PersonInfo"], "");
            gridEX_All.DropDowns["PersonName"].SetDataBinding(DS.Tables["PersonInfo"], "");

            gridEX_Customers.DropDowns["Group"].SetDataBinding(dataSet_001_Takhfif, "Table_040_PersonGroups");
            gridEX_All.DropDowns["Group"].SetDataBinding(dataSet_001_Takhfif, "Table_040_PersonGroups");

            gridEX_Discounts.DropDowns["PersonCode"].SetDataBinding(DS.Tables["PersonInfo"], "");
            gridEX_Discounts.DropDowns["PersonName"].SetDataBinding(DS.Tables["PersonInfo"], "");

            gridEX_All.DropDowns["PersonCode"].SetDataBinding(DS.Tables["PersonInfo"], "");
            gridEX_All.DropDowns["PersonName"].SetDataBinding(DS.Tables["PersonInfo"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients", ConWare);
            Adapter.Fill(DS, "Good");
            gridEX_Discounts.DropDowns["GoodCode"].SetDataBinding(DS.Tables["Good"], "");
            gridEX_Discounts.DropDowns["GoodName"].SetDataBinding(DS.Tables["Good"], "");
            gridEX_All.DropDowns["GoodCode"].SetDataBinding(DS.Tables["Good"], "");
            gridEX_All.DropDowns["GoodName"].SetDataBinding(DS.Tables["Good"], "");
            mlt_Good.DataSource = DS.Tables["Good"];

            this.table_040_PersonGroupsTableAdapter.Fill(this.dataSet_001_Takhfif.Table_040_PersonGroups);
            this.table_045_PersonScopeTableAdapter.Fill(this.dataSet_001_Takhfif.Table_045_PersonScope);
            try
            {
                this.table_027_Discount_CommoditySpecialCustomerTableAdapter.Fill(dataSet_001_Takhfif.Table_027_Discount_CommoditySpecialCustomer);
            }
            catch
            {
            }
            uiTab1.SelectedIndex = 0;
        }

        private void txt_FromPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else Class_BasicOperation.isEnter(e.KeyChar);
            }
            else if (sender is Janus.Windows.GridEX.EditControls.EditBox)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != 46)
                    e.Handled = true;
                else Class_BasicOperation.isEnter(e.KeyChar);
            }

        }

        private void txt_ToPrice_TextChanged(object sender, EventArgs e)
        {
            decimal Num;
            if (decimal.TryParse(((Janus.Windows.GridEX.EditControls.EditBox)sender).Text, out Num))
            {
                ((Janus.Windows.GridEX.EditControls.EditBox)sender).Text = string.Format("{0:N0}", Num);
                ((Janus.Windows.GridEX.EditControls.EditBox)sender).SelectionStart =
                    ((Janus.Windows.GridEX.EditControls.EditBox)sender).Text.Length;
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
                    if (mlt_Good.Text.Trim() != "" && txt_From.Text.Trim() != "" && txt_To.Text.Trim() != "" && txt_Percent.Text.Trim() != "" && gridEX_Customers.GetCheckedRows().Length > 0)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به اعمال تخفیف به افراد انتخاب شده در گروهها هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            foreach (GridEXRow item in gridEX_Customers.GetCheckedRows())
                            {
                                this.table_027_Discount_CommoditySpecialCustomerBindingSource.AddNew();
                                DataRowView NewRow = (DataRowView)this.table_027_Discount_CommoditySpecialCustomerBindingSource.CurrencyManager.Current;
                                NewRow["Column01"] = item.Cells["Column01"].Value.ToString();
                                NewRow["Column02"] = faDatePicker1.Text;
                                NewRow["Column03"] = faDatePicker2.Text;
                                NewRow["Column04"] = mlt_Good.Value.ToString();
                                NewRow["Column05"] = txt_From.Text.Replace(",", "");
                                NewRow["Column06"] = txt_To.Text.Replace(",", "");
                                NewRow["Column07"] = txt_Percent.Text;
                                NewRow["Column08"] = Class_BasicOperation._UserName;
                                NewRow["Column09"] = Class_BasicOperation.ServerDate();
                                NewRow["Column10"] = Class_BasicOperation._UserName;
                                NewRow["Column11"] = Class_BasicOperation.ServerDate();
                                NewRow["Column12"] = item.Cells["Column02"].Value.ToString();
                                this.table_027_Discount_CommoditySpecialCustomerBindingSource.EndEdit();
                                this.table_027_Discount_CommoditySpecialCustomerTableAdapter.Update(dataSet_001_Takhfif.Table_027_Discount_CommoditySpecialCustomer);


                            }
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

        private void Frm_003_TakhfifKalaiiVijheMoshtari_KeyDown(object sender, KeyEventArgs e)
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
                    if (txt_From.Text.Trim() != "" && txt_To.Text.Trim() != "" && txt_Percent.Text.Trim() != "" && gridEX_Customers.GetCheckedRows().Length > 0)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به اعمال تغییرات تخفیفات معرفی شده در این بازه زمانی، برای اشخاص انتخاب شده هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE))
                            {
                                ConSale.Open();
                                foreach (GridEXRow item in gridEX_Customers.GetCheckedRows())
                                {
                                    SqlCommand Update = new SqlCommand("UPDATE Table_027_Discount_CommoditySpecialCustomer  SET Column02='" + faDatePicker1.Text
                                        + "' , Column03='" + faDatePicker2.Text + "' ,Column04=" + mlt_Good.Value.ToString() + " , Column05=" + txt_From.Text.Replace(",", "") + " , Column06=" + txt_To.Text.Replace(",", "")
                                        + " , Column07=" + txt_Percent.Text + " , Column10='" + Class_BasicOperation._UserName + "' , Column11=getdate() where Column01=" + item.Cells["Column01"].Value.ToString()
                                        + " and Column12=" + item.Cells["Column02"].Value.ToString() + " and Column02='" + faDatePicker1.Text + "' and Column03='" + faDatePicker2.Text + "'", ConSale);
                                    Update.ExecuteNonQuery();
                                }
                            } try
                            {
                                dataSet_001_Takhfif.EnforceConstraints = false;
                                this.table_045_PersonScopeTableAdapter.Fill(dataSet_001_Takhfif.Table_045_PersonScope);

                                this.table_027_Discount_CommoditySpecialCustomerTableAdapter.Fill(dataSet_001_Takhfif.Table_027_Discount_CommoditySpecialCustomer);
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
                                    SqlCommand Delete = new SqlCommand("Delete From Table_027_Discount_CommoditySpecialCustomer  where Column01=" + item.Cells["Column01"].Value.ToString()
                                        + " and Column12=" + item.Cells["Column02"].Value.ToString() + " and Column02='" + faDatePicker1.Text + "' and Column03='" + faDatePicker2.Text + "'", ConSale);
                                    Delete.ExecuteNonQuery();
                                }
                            }
                            try
                            {
                                dataSet_001_Takhfif.EnforceConstraints = false;
                                this.table_045_PersonScopeTableAdapter.Fill(dataSet_001_Takhfif.Table_045_PersonScope);

                                this.table_027_Discount_CommoditySpecialCustomerTableAdapter.Fill(dataSet_001_Takhfif.Table_027_Discount_CommoditySpecialCustomer);
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

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                table_027_Discount_CommoditySpecialCustomerBindingSource.EndEdit();
                table_027_Discount_CommoditySpecialCustomerTableAdapter.Update(dataSet_001_Takhfif.Table_027_Discount_CommoditySpecialCustomer);
                table_027_Discount_CommoditySpecialCustomer_AllTableAdapter.Update(dataSet_001_Takhfif.Table_027_Discount_CommoditySpecialCustomer_All);
                try
                {
                    this.table_027_Discount_CommoditySpecialCustomerTableAdapter.Fill(dataSet_001_Takhfif.Table_027_Discount_CommoditySpecialCustomer);
                    table_027_Discount_CommoditySpecialCustomer_AllTableAdapter.Fill(dataSet_001_Takhfif.Table_027_Discount_CommoditySpecialCustomer_All);
                }
                catch
                {
                }
                Class_BasicOperation.ShowMsg("", "ثبت اطلاعات با موفقیت صورت گرفت", "Information");
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void uiTab1_SelectedTabChanged(object sender, Janus.Windows.UI.Tab.TabEventArgs e)
        {
            if (e.Page.Index != 0)
            {
                table_027_Discount_CommoditySpecialCustomer_AllTableAdapter.Fill(dataSet_001_Takhfif.Table_027_Discount_CommoditySpecialCustomer_All);
                bindingNavigator1.BindingSource = table_027_Discount_CommoditySpecialCustomer_AllBindingSource;
            }
            else
            {
                bindingNavigator1.BindingSource = table_027_Discount_CommoditySpecialCustomerBindingSource;
            }
        }

        private void mlt_Good_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
            {
                mlt_Good.DroppedDown = true;
            }
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void mlt_Good_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");
        }

        private void mlt_Good_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridEX_All.GetCheckedRows().Length > 0)
                {
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_All.GetCheckedRows())
                    {
                        item.Delete();
                    }
                }
            }
            catch
            {
            }
        }

        private void gridEX_Discounts_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_All_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }


    }
}
