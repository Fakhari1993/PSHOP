using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._07_Services
{
    public partial class Form01_DefineServices : Form
    {
        bool _Del = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);

        public Form01_DefineServices(bool Del)
        {
            InitializeComponent();
            _Del = Del;
        }

        private void Form01_DefineServices_Load(object sender, EventArgs e)
        {
            foreach (Janus.Windows.GridEX.GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
            }

            DataTable CountUnitTbl = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_070_CountUnitInfo");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(CountUnitTbl, "");
            gridEX1.DropDowns["CountUnit"].SetDataBinding(CountUnitTbl, "");

            this.table_030_ServicesTableAdapter.Fill(this.dataset_Services.Table_030_Services);

        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                //gridEX1.Enabled = true;
                gridEX1.MoveToNewRecord();
                gridEX1.Select();
                gridEX1.Col = 0;
                //bt_New.Enabled = false;

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (this.table_030_ServicesBindingSource.Count > 0)
            {
                try
                {
                    if (((DataRowView)this.table_030_ServicesBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                    {
                        gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_030_Services", "Column01").ToString());
                    }
                    this.table_030_ServicesBindingSource.EndEdit();
                    this.table_030_ServicesTableAdapter.Update(dataset_Services.Table_030_Services);
                    if (sender == bt_Save || sender == this)
                    Class_BasicOperation.ShowMsg("", "ثبت اطلاعات با موفقیت انجام شد", "Information");

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (this.table_030_ServicesBindingSource.Count > 0)
            {
                try
                {
                    if (!_Del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف اطلاعات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        this.table_030_ServicesBindingSource.RemoveCurrent();
                        this.table_030_ServicesTableAdapter.Update(dataset_Services.Table_030_Services);
                        Class_BasicOperation.ShowMsg("", "حذ ف اطلاعات با موفقیت صورت گرفت", "Information");
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                    this.table_030_ServicesTableAdapter.Fill(dataset_Services.Table_030_Services);
                }
            }
        }

        private void mnu_Print_Click(object sender, EventArgs e)
        {
            if (this.table_030_ServicesBindingSource.Count > 0)
            {
                DataTable Table = dataset_Services.Rpt_Services.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    Table.Rows.Add(item.Cells["Column01"].Value.ToString(),
                        item.Cells["Column02"].Text.ToString(),
                        item.Cells["Column03"].Text.ToString(),
                        item.Cells["Column04"].Value.ToString(),
                        item.Cells["Column05"].Text.ToString(),
                        item.Cells["Column06"].Text.ToString());
                }
                if (Table.Rows.Count > 0)
                {
                    _07_Services.Reports.ReportForm frm = new Reports.ReportForm(1, Table);
                    frm.ShowDialog();
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                System.IO.FileStream fs = ((System.IO.FileStream)saveFileDialog1.OpenFile());
                gridEXExporter1.Export(fs);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;
        }

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "Column05")
                {
                    gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.None;
                }
                else gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;

            }
            catch
            {
            }
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_RowEditCanceled(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            //if (!bt_New.Enabled)
            //{
                gridEX1.MoveToNewRecord();
                gridEX1.Select();
                gridEX1.Col = 1;
            //    bt_New.Enabled = false;
            //}
        }

        private void Form01_DefineServices_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
            {
                gridEX_List.Row = gridEX_List.FilterRow.Position;
            }
            else if (e.Control && e.KeyCode == Keys.P)
                mnu_Print_Click(sender, e);
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                if (dataset_Services.Table_030_Services.GetChanges() != null)
                {
                        if (((DataRowView)this.table_030_ServicesBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                        {
                            gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_030_Services", "Column01").ToString());
                        }
                        this.table_030_ServicesBindingSource.EndEdit();
                        this.table_030_ServicesTableAdapter.Update(dataset_Services.Table_030_Services);
                }
                this.table_030_ServicesBindingSource.EndEdit();
                this.table_030_ServicesBindingSource.MoveFirst();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                if (dataset_Services.Table_030_Services.GetChanges() != null)
                {
                    if (dataset_Services.Table_030_Services.GetChanges() != null)
                    {
                        if (((DataRowView)this.table_030_ServicesBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                        {
                            gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_030_Services", "Column01").ToString());
                        }
                        this.table_030_ServicesBindingSource.EndEdit();
                        this.table_030_ServicesTableAdapter.Update(dataset_Services.Table_030_Services);
                    }
                }
                this.table_030_ServicesBindingSource.EndEdit();
                this.table_030_ServicesBindingSource.MovePrevious();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                if (dataset_Services.Table_030_Services.GetChanges() != null)
                {
                    if (dataset_Services.Table_030_Services.GetChanges() != null)
                    {
                        if (((DataRowView)this.table_030_ServicesBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                        {
                            gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_030_Services", "Column01").ToString());
                        }
                        this.table_030_ServicesBindingSource.EndEdit();
                        this.table_030_ServicesTableAdapter.Update(dataset_Services.Table_030_Services);
                    }
                }
                this.table_030_ServicesBindingSource.EndEdit();
                this.table_030_ServicesBindingSource.MoveNext();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                if (dataset_Services.Table_030_Services.GetChanges() != null)
                {
                    if (dataset_Services.Table_030_Services.GetChanges() != null)
                    {
                        if (((DataRowView)this.table_030_ServicesBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                        {
                            gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_030_Services", "Column01").ToString());
                        }
                        this.table_030_ServicesBindingSource.EndEdit();
                        this.table_030_ServicesTableAdapter.Update(dataset_Services.Table_030_Services);
                    }
                }
                this.table_030_ServicesBindingSource.EndEdit();
                this.table_030_ServicesBindingSource.MoveLast();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void gridEX1_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Value.ToString().Trim() == "")
                    e.Value = DBNull.Value;
            }
            catch 
            {
                e.Value = DBNull.Value;
            }
        }

   
    }
}
