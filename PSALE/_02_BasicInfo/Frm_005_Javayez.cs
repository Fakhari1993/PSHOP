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
    public partial class Frm_005_Javayez : Form
    {
        bool _del;
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);

        public Frm_005_Javayez(bool del)
        {
            _del = del;
            InitializeComponent();
        }
        
        private void Frm_005_Javayez_Load(object sender, EventArgs e)
        {
            faDatePicker1.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePicker2.SelectedDateTime = DateTime.Now;

            this.table_040_PersonGroupsTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_040_PersonGroups);
            this.table_028_AwardTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_028_Award);


            DataTable GoodTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients");
            gridEX2.DropDowns[0].DataSource = GoodTable;
            mlt_GiftGood.DataSource = GoodTable;
            mlt_Good.DataSource = GoodTable;

            gridEX2.DropDowns[1].SetDataBinding(dataSet_EtelaatPaye, "Table_040_PersonGroups");

        }
       
        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (this.table_028_AwardBindingSource.Count > 0)
            {
                try
                {
                    this.Validate();
                    this.table_028_AwardBindingSource.EndEdit();
                    this.table_028_AwardTableAdapter.Update(dataSet_EtelaatPaye.Table_028_Award);
                      if(sender==bt_Save || sender==this)
                    Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "Information");
                }
                catch (System.Data.SqlClient.SqlException es)
                {
                    Class_BasicOperation.CheckSqlExp(es,this.Name);
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;

                }
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (this.table_028_AwardBindingSource.Count > 0)
            {
                try
                {
                    if (!_del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف اطلاعات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        this.table_028_AwardBindingSource.RemoveCurrent();
                        this.table_028_AwardBindingSource.EndEdit();
                        this.table_028_AwardTableAdapter.Update(dataSet_EtelaatPaye.Table_028_Award);
                        Class_BasicOperation.ShowMsg("", "حذف اطلاعات با موفقیت انجام گرفت", "Information");
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }

        private void Frm_005_Javayez_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }


        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX2.CurrentCellDroppedDown = true;
            try
            {
                gridEX2.SetValue("Column15", Class_BasicOperation._UserName);
                gridEX2.SetValue("Column16", Class_BasicOperation.ServerDate());
            }
            catch 
            {
            }
        }

        private void bt_ExportToExcel_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                System.IO.FileStream fs = ((System.IO.FileStream)saveFileDialog1.OpenFile());
                gridEXExporter1.Export(fs);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void gridEX2_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
                gridEX2.SetValue("Column13", Class_BasicOperation._UserName);
                gridEX2.SetValue("Column14", Class_BasicOperation.ServerDate());
                gridEX2.SetValue("Column15", Class_BasicOperation._UserName);
                gridEX2.SetValue("Column16", Class_BasicOperation.ServerDate());
            }
            catch 
            {
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (this.table_028_AwardBindingSource.Count > 0)
            {
                bt_Save_Click(sender, e);
                DataTable Table = dataSet_EtelaatPaye.Rpt_Gift.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
                {
                    Table.Rows.Add(item.Cells["Column01"].Text,
                        item.Cells["Column02"].Text,
                        item.Cells["Column03"].Text,
                        double.Parse(item.Cells["Column06"].Value.ToString()),
                        double.Parse(item.Cells["Column07"].Value.ToString()),
                        item.Cells["Column08"].Text,
                        double.Parse(item.Cells["Column11"].Value.ToString())
                        ,item.Cells["Column13"].Text);
                }
                if (Table.Rows.Count > 0)
                {
                    _02_BasicInfo.Reports.ReportForm frm = new Reports.ReportForm(1, Table);
                    frm.ShowDialog();
                }
            }
        }

        private void gridEX2_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (!_del)
            {
                e.Cancel = true;
                MessageBox.Show("کاربر جاری، دسترسی لازم جهت حذف اطلاعات معرفی شده را ندارد");
            }
            else
            {
                this.table_028_AwardBindingSource.RemoveCurrent();
                this.table_028_AwardBindingSource.EndEdit();
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

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = 
                    (FarsiLibrary.Win.Controls.FADatePicker)sender;


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

        private void mlt_Good_KeyPress(object sender, KeyPressEventArgs e)
        {
            mlt_Good.DroppedDown = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void multiColumnCombo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            mlt_GiftGood.DroppedDown = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void txt_To_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != 46)
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (!faDatePicker1.SelectedDateTime.HasValue ||
                !faDatePicker2.SelectedDateTime.HasValue ||
                mlt_Good.Text.Trim() == "" || mlt_GiftGood.Text.Trim() == "" ||
                txt_From.Text.Trim() == "" || txt_To.Text.Trim() == "" ||
                txt_GiftCount.Text.Trim() == "")
            {
                MessageBox.Show("لطفا اطلاعات مورد نیاز را به طور صحیح وارد نمایید");
                faDatePicker1.Select();
            }
            else
            {
                for (int i = 0; i < gridEX1.RowCount; i++)
                {
                    gridEX1.Row = i;

                    if (Convert.ToBoolean(gridEX1.GetValue("Selector")))
                    {
                        gridEX2.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;

                        gridEX2.MoveToNewRecord();
                        gridEX2.SetValue("column17", gridEX1.GetValue("Column00"));
                        gridEX2.SetValue("column01", mlt_Good.Value); 
                        gridEX2.SetValue("column02", faDatePicker1.Text); 
                        gridEX2.SetValue("column03", faDatePicker2.Text); 
                        gridEX2.SetValue("column06", txt_From.Text);
                        gridEX2.SetValue("column07", txt_To.Text);
                        gridEX2.SetValue("column08", mlt_GiftGood.Value);
                        gridEX2.SetValue("column11", txt_GiftCount.Text);

                        gridEX2.UpdateData();

                        gridEX2.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
                    }
                }

                gridEX1.MoveFirst();
            }

        }

        private void mlt_Good_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                Class_BasicOperation.FilterMultiColumns(sender, "Column02", null);
            }
            catch 
            {
            }
        }

        private void multiColumnCombo1_Leave(object sender, EventArgs e)
        {
            try
            {
                Class_BasicOperation.MultiColumnsRemoveFilter(sender);
            }
            catch 
            {
            }
        }

     

        

    }
}
