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

namespace PSHOP._05_Sale
{
    public partial class Frm_001_PishFaktor2 : Form
    {
        bool _del;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        int _ID = 0;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        List<string> CustomerGroupList = new List<string>();
        string[] CustomerGroupsArray;


        public Frm_001_PishFaktor2(bool del)
        {
            _del = del;
            InitializeComponent();
        }
        public Frm_001_PishFaktor2(bool del, int ID)
        {
            InitializeComponent();
            _del = del;
            _ID = ID;
        }
        private void Frm_002_PishFaktor_Load(object sender, EventArgs e)
        {

            try
            {

                gridEXFieldChooserControl1.GridEX = gridEX_List;


                //GoodBindingSource.DataSource = clGood.GoodInfo();
                //DataTable GoodTable = clGood.GoodInfo();
                //gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                //gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

                DataTable CustomerTable = clDoc.ReturnTable
                (ConBase.ConnectionString, @"SELECT     dbo.Table_045_PersonInfo.ColumnId AS id, dbo.Table_045_PersonInfo.Column01 AS code, dbo.Table_045_PersonInfo.Column02 AS name, 
                dbo.Table_065_CityInfo.Column02 AS shahr, dbo.Table_060_ProvinceInfo.Column01 AS ostan, dbo.Table_045_PersonInfo.Column06 AS Address, 
                dbo.Table_045_PersonInfo.Column30,Table_045_PersonInfo.Column07 
                FROM         dbo.Table_060_ProvinceInfo INNER JOIN
                dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00 INNER JOIN
                dbo.Table_045_PersonInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                WHERE     (dbo.Table_045_PersonInfo.Column12 = 1)");
                gridEX1.DropDowns["Customer"].SetDataBinding(CustomerTable, "");
                gridEX1.DropDowns["SaleType"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_002_SalesTypes");
                gridEX1.DropDowns["Seller"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.BASE, "Select * from PeopleScope(8,3)");
                gridEX1.DropDowns["Sale"].DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_010_SaleFactor");
                gridEX1.DropDowns["Order"].DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select Columnid,Column01 from Table_005_OrderHeader");


                DataTable type = new DataTable();
                type.Columns.Add("ID", typeof(Int32));
                type.Columns.Add("Name", typeof(string));
                type.Rows.Add(0, "اضافه");
                type.Rows.Add(1, "تخفیف");



                gridEX_Ezafat.DropDowns["d"].DataSource = type;


                if (_ID == 0)
                {
                    this.table_007_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_007_FactorBefore2, -1);
                    this.table_009_Child2_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_009_Child2_FactorBefore2, -1);
                    this.table_008_Child1_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_008_Child1_FactorBefore2, -1);

                }
                else
                {
                    this.table_007_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_007_FactorBefore2, _ID);
                    this.table_009_Child2_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_009_Child2_FactorBefore2, _ID);
                    this.table_008_Child1_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_008_Child1_FactorBefore2, _ID);
                }


                table_007_FactorBeforeBindingSource_PositionChanged(sender, e);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }
        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.AllowEdit = InheritableBoolean.True;
                gridEX1.AllowAddNew = InheritableBoolean.True;
                gridEX_List.AllowAddNew = InheritableBoolean.True;
                gridEX_List.AllowEdit = InheritableBoolean.True;
                gridEX_Ezafat.AllowAddNew = InheritableBoolean.True;
                gridEX_Ezafat.AllowDelete = InheritableBoolean.True;
                gridEX_List.AllowDelete = InheritableBoolean.True;



                gridEX1.Enabled = true;
                dataSet6.EnforceConstraints = false;
                this.table_007_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_007_FactorBefore2, -1);
                this.table_009_Child2_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_009_Child2_FactorBefore2, -1);
                this.table_008_Child1_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_008_Child1_FactorBefore2, -1);

                dataSet6.EnforceConstraints = true;

                txt_Fintotal.Value = 0;
                txt_Kosoorat.Value = 0;
                txt_TotalExtra.Value = 0;
                txt_TotalPrice.Value = 0;

                gridEX1.MoveToNewRecord();
                gridEX1.Select();
                gridEX1.Focus();
                gridEX1.Col = 2;

                bt_New.Enabled = false;



                gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd"));
                gridEX1.SetValue("Column07", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column08", Class_BasicOperation.ServerDate());
                gridEX1.SetValue("Column09", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column10", Class_BasicOperation.ServerDate());

                if (gridEX1.GetRow().Cells["Column06"].Text.Trim() == "")
                    gridEX1.SetValue("Column06", Properties.Settings.Default.PrefactorDescription);


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
                gridEX1.UpdateData();
                if (this.table_007_FactorBefore2BindingSource.Count > 0 &&
                   gridEX_List.AllowEdit == InheritableBoolean.True &&
                   gridEX1.GetRow().Cells["Column03"].Text.Trim() != "")
                {
                    this.Cursor = Cursors.WaitCursor;
                    gridEX_List.UpdateData();
                    gridEX_Ezafat.UpdateData();
                    DataRowView Row = (DataRowView)this.table_007_FactorBefore2BindingSource.CurrencyManager.Current;
                    if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(Row["column02"].ToString().Substring(0, 4)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(5, 2)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(8, 2))))
                    {

                        Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
                        this.Cursor = Cursors.Default;

                        return;

                    }
                    if (Row["Column01"].ToString().StartsWith("-"))
                    {
                        gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_007_FactorBefore2", "Column01").ToString());
                        this.table_007_FactorBefore2BindingSource.EndEdit();
                    }
                    txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column05"],
                    AggregateFunction.Sum).ToString());

                    txt_Fintotal.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                    Convert.ToDouble(txt_TotalExtra.Value.ToString()) -
                    Convert.ToDouble(txt_Kosoorat.Value.ToString());
                    double Total = double.Parse(txt_TotalPrice.Value.ToString());

                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Ezafat.GetRows())
                    {
                        if (double.Parse(item.Cells["Column02"].Value.ToString()) > 0)
                        {
                            item.BeginEdit();
                            item.Cells["Column03"].Value = Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column02"].Value.ToString()) / 100);
                            item.EndEdit();

                        }
                    }
                    Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Ezafat.RootTable.Columns["Column04"], ConditionOperator.Equal, "اضافه");
                    txt_TotalExtra.Value = gridEX_Ezafat.GetTotal(gridEX_Ezafat.RootTable.Columns["Column03"], AggregateFunction.Sum, Filter).ToString();
                    Filter.Value1 = "تخفیف";
                    txt_Kosoorat.Value = gridEX_Ezafat.GetTotal(gridEX_Ezafat.RootTable.Columns["Column03"], AggregateFunction.Sum, Filter).ToString();


                    Properties.Settings.Default.PrefactorDescription = gridEX1.GetRow().Cells["Column06"].Text;
                    Properties.Settings.Default.Save();
                    table_007_FactorBefore2BindingSource.EndEdit();
                    this.table_007_FactorBefore2TableAdapter.Update(this.dataSet6.Table_007_FactorBefore2);

                    this.table_008_Child1_FactorBefore2BindingSource.EndEdit();
                    this.table_008_Child1_FactorBefore2TableAdapter.Update(this.dataSet6.Table_008_Child1_FactorBefore2);

                    this.table_009_Child2_FactorBefore2BindingSource.EndEdit();
                    this.table_009_Child2_FactorBefore2TableAdapter.Update(this.dataSet6.Table_009_Child2_FactorBefore2);


                    int _ID = int.Parse(Row["ColumnId"].ToString());
                    dataSet6.EnforceConstraints = false;
                    this.table_007_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_007_FactorBefore2, _ID);
                    this.table_009_Child2_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_009_Child2_FactorBefore2, _ID);
                    this.table_008_Child1_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_008_Child1_FactorBefore2, _ID);

                    dataSet6.EnforceConstraints = true;
                    table_007_FactorBeforeBindingSource_PositionChanged(sender, e);
                    bt_New.Enabled = true;

                    if (sender == bt_Save || sender == this)
                        Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "Information");


                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }

        }

        private void table_007_FactorBeforeBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (table_007_FactorBefore2BindingSource.Count > 0)
                {
                    txt_TotalPrice.Value = gridEX_List.GetTotalRow().Cells["Column05"].Value.ToString();
                    txt_Kosoorat.Value = (dataSet6.Table_009_Child2_FactorBefore2.Compute("SUM(Column03)", "(Column04=1) AND (Column00=" + gridEX1.GetValue("ColumnId").ToString() + ")").ToString());
                    txt_TotalExtra.Value = (dataSet6.Table_009_Child2_FactorBefore2.Compute("SUM(Column03)", "(Column04=0)AND(Column00=" + gridEX1.GetValue("ColumnId").ToString() + ")").ToString());
                    txt_Fintotal.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_TotalExtra.Value.ToString()) - Convert.ToDouble(txt_Kosoorat.Value.ToString());
                    gridEX1.DropDowns["Sale"].DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_010_SaleFactor");
                    gridEX1.DropDowns["Order"].DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select Columnid,Column01 from Table_005_OrderHeader");

                    //CustomerGroupList.Clear();
                    //CustomerGroupsArray = CustomerGroupList.ToArray();
                    ////حذف آیتم های قبلی از آرایه
                    //if (CustomerGroupsArray.Length > 0)
                    //    Array.Clear(CustomerGroupsArray, 0, CustomerGroupsArray.Length);

                    //DataTable Table = clDoc.ReturnTable(ConBase.ConnectionString, "select Column02 from Table_045_PersonScope where Column01=" + gridEX1.GetValue("Column03").ToString());
                    //foreach (DataRow item in Table.Rows)
                    //{
                    //    CustomerGroupList.Add(item["Column02"].ToString());
                    //}
                    //if (CustomerGroupList.Count > 0)
                    //    CustomerGroupsArray = CustomerGroupList.ToArray();
                }

            }
            catch { }
            try
            {
                if (table_007_FactorBefore2BindingSource.Count > 0)
                {
                    gridEX1.Enabled = true;
                    if (gridEX1.GetValue("Column12").ToString() != "" &&
                        gridEX1.GetValue("Column12").ToString() != "0")
                    {
                        gridEX1.AllowEdit = InheritableBoolean.False;
                        gridEX_List.AllowEdit = InheritableBoolean.False;
                        gridEX_Ezafat.AllowEdit = InheritableBoolean.False;

                        gridEX_List.AllowAddNew = InheritableBoolean.False;
                        gridEX_Ezafat.AllowAddNew = InheritableBoolean.False;
                        gridEX1.AllowAddNew = InheritableBoolean.False;

                        gridEX_Ezafat.AllowDelete = InheritableBoolean.False;
                        gridEX_List.AllowDelete = InheritableBoolean.False;
                    }
                    else
                    {

                        gridEX1.AllowEdit = InheritableBoolean.True;
                        gridEX_List.AllowEdit = InheritableBoolean.True;
                        gridEX_Ezafat.AllowEdit = InheritableBoolean.True;

                        gridEX_List.AllowAddNew = InheritableBoolean.True;
                        gridEX_Ezafat.AllowAddNew = InheritableBoolean.True;
                        gridEX1.AllowAddNew = InheritableBoolean.True;

                        gridEX_Ezafat.AllowDelete = InheritableBoolean.True;
                        gridEX_List.AllowDelete = InheritableBoolean.True;

                        if (gridEX1.GetValue("Column15").ToString() == "True")
                            gridEX1.RootTable.Columns["Column16"].EditType = EditType.TextBox;
                        else
                            gridEX1.RootTable.Columns["Column16"].EditType = EditType.NoEdit;

                        if (gridEX1.GetValue("Column17").ToString() == "True")
                            gridEX1.RootTable.Columns["Column18"].EditType = EditType.TextBox;
                        else
                            gridEX1.RootTable.Columns["Column18"].EditType = EditType.NoEdit;

                        if (gridEX1.GetValue("Column19").ToString() == "True")
                        {
                            gridEX1.RootTable.Columns["Column20"].EditType = EditType.TextBox;
                            gridEX1.RootTable.Columns["Column21"].EditType = EditType.TextBox;
                        }
                        else
                        {
                            gridEX1.RootTable.Columns["Column20"].EditType = EditType.NoEdit;
                            gridEX1.RootTable.Columns["Column21"].EditType = EditType.NoEdit;
                        }

                    }
                }
            }
            catch
            {
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (_del)
            {
                if (this.table_007_FactorBefore2BindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف پیش فاکتور مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            string OrderId = ((DataRowView)this.table_007_FactorBefore2BindingSource.CurrencyManager.Current)["Column11"].ToString();

                            foreach (DataRowView item in table_008_Child1_FactorBefore2BindingSource)
                            {
                                item.Delete();
                            }
                            //dataSet6.Table_008_Child1_FactorBefore2.AcceptChanges();
                            table_008_Child1_FactorBefore2BindingSource.EndEdit();

                            this.table_008_Child1_FactorBefore2TableAdapter.Update(this.dataSet6.Table_008_Child1_FactorBefore2);


                            foreach (DataRowView item in table_009_Child2_FactorBefore2BindingSource)
                            {
                                item.Delete();
                            }

                            table_009_Child2_FactorBefore2BindingSource.EndEdit();
                            this.table_009_Child2_FactorBefore2TableAdapter.Update(dataSet6.Table_009_Child2_FactorBefore2);

                            this.table_007_FactorBefore2BindingSource.RemoveCurrent();
                            this.table_007_FactorBefore2BindingSource.EndEdit();
                            this.table_007_FactorBefore2TableAdapter.Update(dataSet6.Table_007_FactorBefore2);
                            if (OrderId.Trim() != "")
                                clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_005_OrderHeader SET Column32=NULL where ColumnId=" + OrderId);
                            table_007_FactorBeforeBindingSource_PositionChanged(sender, e);
                            Class_BasicOperation.ShowMsg("", "پیش فاکتور مورد نظر حذف شد", "Information");
                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                        }
                    }
                }
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");

        }

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "");
        }

        private void gridEX2_Enter(object sender, EventArgs e)
        {
            try
            {

                table_007_FactorBefore2BindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                table_008_Child1_FactorBefore2BindingSource.CancelEdit();
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void gridEX1_Enter(object sender, EventArgs e)
        {
            try
            {

                table_007_FactorBefore2BindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                table_008_Child1_FactorBefore2BindingSource.CancelEdit();
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }


        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }




        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (_del)
            {
                if (this.table_009_Child2_FactorBefore2BindingSource.Count > 0)
                {
                    if (DialogResult.No == MessageBox.Show("آیا مایل به حذف کسر/اضافه مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                e.Cancel = true;

            }
        }


        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_List.CurrentCellDroppedDown = true;
            txt_TotalPrice.Value = gridEX_List.GetTotalRow().Cells["Column05"].Value.ToString();

        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_Ezafat.CurrentCellDroppedDown = true;
        }

        private void Frm_001_PishFaktor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
                txt_Search.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (this.table_007_FactorBefore2BindingSource.Count > 0)
            {
                try
                {
                    bt_Save_Click(sender, e);
                    _05_Sale.Reports.Form_PreFactorPrint2 frm = new Reports.Form_PreFactorPrint2(int.Parse(gridEX1.GetValue("Column01").ToString()));
                    frm.ShowDialog();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void txt_Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void fa_Date_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;

            Class_BasicOperation.isEnter(e.KeyChar);

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void mlt_Buy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void txt_Admin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                gridEX_List.Focus();
                gridEX_List.Select();
            }
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
                bt_Search_Click(sender, e);
        }

        public void bt_Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
            {
                try
                {
                    gridEX_Ezafat.UpdateData();
                    gridEX_List.UpdateData();
                    this.table_007_FactorBefore2BindingSource.EndEdit();
                    this.table_008_Child1_FactorBefore2BindingSource.EndEdit();
                    this.table_009_Child2_FactorBefore2BindingSource.EndEdit();
                    if (dataSet6.Table_007_FactorBefore2.GetChanges() != null || dataSet6.Table_008_Child1_FactorBefore2.GetChanges() != null || dataSet6.Table_009_Child2_FactorBefore2.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            bt_Save_Click(sender, e);
                        }
                    }

                    dataSet6.EnforceConstraints = false;

                    this.table_007_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_007_FactorBefore2, ReturnIDNumber(int.Parse(txt_Search.Text)));
                    this.table_009_Child2_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_009_Child2_FactorBefore2, ReturnIDNumber(int.Parse(txt_Search.Text)));
                    this.table_008_Child1_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_008_Child1_FactorBefore2, ReturnIDNumber(int.Parse(txt_Search.Text)));
                    dataSet6.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_007_FactorBeforeBindingSource_PositionChanged(sender, e);
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                    txt_Search.SelectAll();
                }
            }
        }

        private int ReturnIDNumber(int FactorNum)
        {
            using (SqlConnection consale = new SqlConnection(Properties.Settings.Default.SALE))
            {
                consale.Open();
                int ID = 0;
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_007_FactorBefore2 where column01=" + FactorNum, consale);
                try
                {
                    ID = int.Parse(Commnad.ExecuteScalar().ToString());
                    return ID;
                }
                catch
                {
                    throw new Exception("شماره پیش فاکتور وارد شده نامعتبر است");
                }
            }
        }

        private void mnu_Customers_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 5))
            {
                PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;

                PACNT._1_BasicMenu.Form03_Persons frm = new PACNT._1_BasicMenu.Form03_Persons(
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 6));
                frm.ShowDialog();

                DataTable CustomerTable = clDoc.ReturnTable
                 (ConBase.ConnectionString, @"SELECT     dbo.Table_045_PersonInfo.ColumnId AS id, dbo.Table_045_PersonInfo.Column01 AS code, dbo.Table_045_PersonInfo.Column02 AS name, 
                dbo.Table_065_CityInfo.Column02 AS shahr, dbo.Table_060_ProvinceInfo.Column01 AS ostan, dbo.Table_045_PersonInfo.Column06 AS Address, 
                dbo.Table_045_PersonInfo.Column30,Table_045_PersonInfo.Column07 
                FROM         dbo.Table_060_ProvinceInfo INNER JOIN
                dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00 INNER JOIN
                dbo.Table_045_PersonInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                WHERE     (dbo.Table_045_PersonInfo.Column12 = 1)");
                gridEX1.DropDowns["Customer"].SetDataBinding(CustomerTable, "");
                gridEX1.DropDowns["Seller"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.BASE, "Select * from PeopleScope(8,3)");
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }




        private void mnu_ViewPrefactor_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 64))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_002_ViewPrefactors2")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                _05_Sale.Frm_002_ViewPrefactors2 frm = new Frm_002_ViewPrefactors2();
                try
                {
                    frm.MdiParent = MainForm.ActiveForm;
                }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }





        private void gridEX_List_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            //try
            //{
            //    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
            //    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column002");
            //}
            //catch 
            //{
            //}
            try
            {
                //if (e.Column.Key == "column002")
                //    gridEX_List.SetValue("Column02", gridEX_List.GetValue("Column002").ToString());
                //else if (e.Column.Key == "column02")
                //    gridEX_List.SetValue("Column002", gridEX_List.GetValue("Column02").ToString());

                if (e.Column.Key == "column02" || e.Column.Key == "column002")
                {
                    //this.GoodBindingSource.Filter = "GoodID=" + gridEX_List.GetValue("Column02").ToString();
                    //gridEX_List.SetValue("tedaddarkartoon", ((DataRowView)GoodBindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                    //gridEX_List.SetValue("tedaddarbaste", ((DataRowView)GoodBindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                    //gridEX_List.SetValue("column03", ((DataRowView)GoodBindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                    ////gridEX_List.SetValue("column10", Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodBindingSource.CurrencyManager.Current)["SalePrice"].ToString())));
                    ////gridEX_List.SetValue("column09", Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodBindingSource.CurrencyManager.Current)["SalePackPrice"].ToString())));
                    ////gridEX_List.SetValue("column08", Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodBindingSource.CurrencyManager.Current)["SaleBoxPrice"].ToString())));
                    //gridEX_List.SetValue("column16", ((DataRowView)GoodBindingSource.CurrencyManager.Current)["Discount"].ToString());
                    //gridEX_List.SetValue("column18", ((DataRowView)GoodBindingSource.CurrencyManager.Current)["Extra"].ToString());

                    //                    //فراخوانی قیمتهای پیش فرض از  سیاستهای فروش
                    //                    if (CustomerGroupList.Count == 0)
                    //                    {
                    //                        gridEX_List.SetValue("column10",
                    //                            Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodBindingSource.CurrencyManager.Current)["SalePrice"].ToString())));
                    //                        gridEX_List.SetValue("column09",
                    //                            Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodBindingSource.CurrencyManager.Current)["SalePackPrice"].ToString())));
                    //                        gridEX_List.SetValue("column08",
                    //                            Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodBindingSource.CurrencyManager.Current)["SaleBoxPrice"].ToString())));
                    //                    }
                    //                    else
                    //                    {
                    //                        CustomerPricingbindingSource.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, @"select * from Table_029_CustomerGroupGoodPricing   
                    //                            where Column01 IN (" + string.Join(",", CustomerGroupsArray) + ") and Column02=" +
                    //                         gridEX_List.GetValue("column02").ToString() +
                    //                         " and Column03<='" + gridEX1.GetValue("Column02").ToString() +
                    //                         "' and Column04>='" + gridEX1.GetValue("Column02").ToString() + "'  order by Column01,Column02,Column03,Column04");
                    //                        if (CustomerPricingbindingSource.Count > 0)
                    //                        {
                    //                            gridEX_List.SetValue("column10",
                    //                               Convert.ToInt64(Convert.ToDouble( ((DataRowView)CustomerPricingbindingSource.CurrencyManager.Current)["Column07"].ToString())));
                    //                            gridEX_List.SetValue("column09",
                    //                                Convert.ToInt64(Convert.ToDouble(((DataRowView)CustomerPricingbindingSource.CurrencyManager.Current)["Column05"].ToString())));
                    //                            gridEX_List.SetValue("column08",
                    //                                Convert.ToInt64(Convert.ToDouble(((DataRowView)CustomerPricingbindingSource.CurrencyManager.Current)["Column06"].ToString())));
                    //                        }
                    //                        else
                    //                        {
                    //                            gridEX_List.SetValue("column10",
                    //                           Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodBindingSource.CurrencyManager.Current)[
                    //                                "SalePrice"].ToString())));
                    //                            gridEX_List.SetValue("column09",
                    //                                Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodBindingSource.CurrencyManager.Current)[
                    //                                "SalePackPrice"].ToString())));
                    //                            gridEX_List.SetValue("column08",
                    //                                Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodBindingSource.CurrencyManager.Current)[
                    //                                "SaleBoxPrice"].ToString())));
                    //                        }
                    //                    }
                }
                //gridEX_List.SetValue("column07", Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarkartoon")));
                //gridEX_List.SetValue("column07", Convert.ToDouble(gridEX_List.GetValue("column07")) + (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarbaste"))));
                //gridEX_List.SetValue("column07", Convert.ToDouble(gridEX_List.GetValue("column07")) + Convert.ToDouble(gridEX_List.GetValue("column06")));

                //if (!mnu_CalculatePrice.Checked)
                //{
                //    gridEX_List.SetValue("column11", Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("column08")));
                //    gridEX_List.SetValue("column11", Convert.ToDouble(gridEX_List.GetValue("column11")) + (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("column09"))));
                //    gridEX_List.SetValue("column11", Convert.ToDouble(gridEX_List.GetValue("column11")) + (Convert.ToDouble(gridEX_List.GetValue("column06")) * Convert.ToDouble(gridEX_List.GetValue("column10"))));
                //}
                //else
                //{
                //    gridEX_List.SetValue("Column11",
                //            Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column07").ToString()) *
                //         Convert.ToDouble(gridEX_List.GetValue("column10"))));
                //}

                if (e.Column.Key == "Column04" || e.Column.Key == "Column03")
                {
                    try
                    {
                        gridEX_List.SetValue("Column05", Convert.ToDouble(gridEX_List.GetValue("Column03")) * Convert.ToDouble(gridEX_List.GetValue("Column04")));
                    }
                    catch
                    {
                    }
                }
                //در غیر این صورت بر اساس اعداد صحیح
                Int64 jam, takhfif, ezafe;
                jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column05")));
                //takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
                //ezafe = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) *
                //  Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);

                //gridEX_List.SetValue("column17", takhfif);
                //gridEX_List.SetValue("column19", ezafe);
                //gridEX_List.SetValue("column21", (jam - takhfif) + ezafe);

                //محاسبه قیمتهای انتهای فاکتور
                txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column05"],
                    AggregateFunction.Sum).ToString());
                txt_Fintotal.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                    Convert.ToDouble(txt_TotalExtra.Value.ToString()) -
                    Convert.ToDouble(txt_Kosoorat.Value.ToString());

                gridEX_List.SetValue("Column10", Class_BasicOperation._UserName);
                gridEX_List.SetValue("Column11", Class_BasicOperation.ServerDate());
            }
            catch { }
        }

        private void Frm_001_PishFaktor_Activated(object sender, EventArgs e)
        {
            txt_Search.Focus();
        }


        private void gridEX_Ezafat_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                //if (e.Column.Key == "column02")
                //{
                //    gridEX_Ezafat.SetValue("column05", (gridEX_Ezafat.DropDowns["d"].GetValue("column02")));
                //    gridEX_Ezafat.SetValue("column04", "0");
                //    gridEX_Ezafat.SetValue("column03", "0");
                //    if (gridEX_Ezafat.DropDowns["d"].GetValue("column03").ToString() == "True")
                //    {
                //        gridEX_Ezafat.SetValue("column04", gridEX_Ezafat.DropDowns["d"].GetValue("column04").ToString());
                //    }
                //    else
                //    {
                //        gridEX_Ezafat.SetValue("column03", gridEX_Ezafat.DropDowns["d"].GetValue("column04").ToString());
                //        Double darsad;
                //        darsad = Convert.ToDouble(gridEX_Ezafat.DropDowns["d"].GetValue("column04").ToString());
                //        Double kol;
                //        kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column21"].Value.ToString());
                //        if (kol == 0)
                //            return;
                //        gridEX_Ezafat.SetValue("column04", (kol / 100) * darsad);
                //    }
                //}
                //else
                if (e.Column.Key == "Column02")
                {
                    Double darsad;
                    darsad = Convert.ToDouble(gridEX_Ezafat.GetValue("Column02").ToString());
                    Double kol;
                    kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["Column05"].Value.ToString());
                    if (kol == 0)
                        return;
                    gridEX_Ezafat.SetValue("Column03", (kol / 100) * darsad);
                }
            }
            catch { }
        }

        private void bt_Prefactor_Signatures_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 121))
            {
                _05_Sale.Frm_024_Prefactor_Signatures frm = new Frm_024_Prefactor_Signatures();
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }


        private void mnu_DefinePrefactorDescription_Click(object sender, EventArgs e)
        {
            Frm_026_PrefactorDefaultDescription frm = new Frm_026_PrefactorDefaultDescription();
            frm.ShowDialog();
        }

        private void mnu_ViewSaleFactor_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 20))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_002_Faktor")
                    {
                        item.BringToFront();
                        PSHOP._05_Sale.Frm_002_Faktor frm = (PSHOP._05_Sale.Frm_002_Faktor)item;
                        frm.txt_Search.Text = (gridEX1.GetRow().Cells["Column12"].Text.Trim() != "" ?
                            gridEX1.GetRow().Cells["Column12"].Value.ToString().Trim() : "0");
                        frm.bt_Search_Click(sender, e);
                        return;
                    }
                }

                PSHOP._05_Sale.Frm_002_Faktor frms = new PSHOP._05_Sale.Frm_002_Faktor(UserScope.CheckScope
                    (Class_BasicOperation._UserName, "Column11", 21), int.Parse((gridEX1.GetRow().Cells["Column12"].Text.Trim() != "" ?
                    gridEX1.GetRow().Cells["Column12"].Value.ToString().Trim() : "0")));
                try
                {
                    frms.MdiParent = MainForm.ActiveForm;
                }
                catch { }
                frms.Show();


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }


        private void gridEX_List_CancelingCellEdit(object sender, ColumnActionCancelEventArgs e)
        {
            //    try
            //    {
            //        Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
            //        Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column002");
            //    }
            //    catch
            //    {
            //    }
        }

        private void gridEX_Ezafat_RowCountChanged(object sender, EventArgs e)
        {
            try
            {


                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Ezafat.RootTable.Columns["Column04"], ConditionOperator.Equal, "اضافه");
                txt_TotalExtra.Value = gridEX_Ezafat.GetTotal(gridEX_Ezafat.RootTable.Columns["Column03"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = "تخفیف";
                txt_Kosoorat.Value = gridEX_Ezafat.GetTotal(gridEX_Ezafat.RootTable.Columns["Column03"], AggregateFunction.Sum, Filter).ToString();
                txt_Fintotal.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_TotalExtra.Value.ToString()) - Convert.ToDouble(txt_Kosoorat.Value.ToString());
            }
            catch
            {
            }
        }

        private void gridEX1_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column03");
            }
            catch { }
            try
            {
                if (e.Column.Key == "column14")
                {
                    DateTime FactorDate = FarsiLibrary.Utils.PersianDate.Parse(gridEX1.GetValue("Column02").ToString());
                    DateTime Add = FactorDate.AddDays(int.Parse(gridEX1.GetValue("Column14").ToString()));
                    gridEX1.SetValue("Column22", FarsiLibrary.Utils.PersianDateConverter.ToPersianDate(Add).ToString("0000/00/00"));
                }
                else if (e.Column.Key == "column03")
                {
                    try
                    {
                        //پر کردن نوع فروش بر اساس مشتری
                        gridEX1.SetValue("Column23",
                           ((DataRowView)gridEX1.RootTable.Columns["Column03"].DropDown.FindItem(
                            gridEX1.GetValue("Column03")))["Column30"].ToString());
                    }
                    catch { }
                    try
                    {
                        CustomerGroupList.Clear();
                        CustomerGroupsArray = CustomerGroupList.ToArray();
                        //حذف آیتم های قبلی از آرایه
                        if (CustomerGroupsArray.Length > 0)
                            Array.Clear(CustomerGroupsArray, 0, CustomerGroupsArray.Length);

                        DataTable Table = clDoc.ReturnTable(ConBase.ConnectionString, "select Column02 from Table_045_PersonScope where Column01=" +
                            gridEX1.GetValue("Column03").ToString());
                        foreach (DataRow item in Table.Rows)
                        {
                            CustomerGroupList.Add(item["Column02"].ToString());
                        }
                        if (CustomerGroupList.Count > 0)
                            CustomerGroupsArray = CustomerGroupList.ToArray();
                    }
                    catch { }

                }

                else if (e.Column.Key == "column15")
                {
                    if (gridEX1.GetValue("Column15").ToString() == "True")
                    {
                        gridEX1.RootTable.Columns["Column16"].EditType = EditType.TextBox;
                        gridEX1.SetValue("Column16", 0);
                    }
                    else
                    {
                        gridEX1.RootTable.Columns["Column16"].EditType = EditType.NoEdit;
                        gridEX1.SetValue("Column16", 0);
                    }

                }
                else if (e.Column.Key == "column17")
                {
                    if (gridEX1.GetValue("column17").ToString() == "True")
                    {
                        gridEX1.RootTable.Columns["Column18"].EditType = EditType.TextBox;
                        gridEX1.SetValue("Column18", 0);
                    }
                    else
                    {
                        gridEX1.RootTable.Columns["Column18"].EditType = EditType.NoEdit;
                        gridEX1.SetValue("Column18", 0);
                    }

                }
                else if (e.Column.Key == "column19")
                {
                    if (gridEX1.GetValue("column19").ToString() == "True")
                    {
                        gridEX1.RootTable.Columns["Column20"].EditType = EditType.TextBox;
                        gridEX1.RootTable.Columns["Column21"].EditType = EditType.TextBox;
                        gridEX1.SetValue("Column20", 0);
                        gridEX1.SetValue("Column21", 0);
                    }
                    else
                    {
                        gridEX1.RootTable.Columns["Column20"].EditType = EditType.NoEdit;
                        gridEX1.RootTable.Columns["Column21"].EditType = EditType.NoEdit;
                        gridEX1.SetValue("Column20", 0);
                        gridEX1.SetValue("Column21", 0);
                    }

                }

            }
            catch
            {
            }
        }

        private void gridEX1_CellValueChanged_1(object sender, ColumnActionEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Control)
                gridEX1.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "column03")
                {
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column03", "code", "name", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
                }
            }
            catch
            {
            }
        }

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "column07")
                {
                    gridEX1.EnterKeyBehavior = EnterKeyBehavior.None;
                    gridEX_List.Select();
                    gridEX_List.Focus();
                }
                else gridEX1.EnterKeyBehavior = EnterKeyBehavior.NextCell;

            }
            catch
            {
            }
        }

        private void gridEX1_RowEditCanceled(object sender, RowActionEventArgs e)
        {
            gridEX1.Enabled = false;
            bt_New.Enabled = true;
        }

        private void gridEX1_UpdatingCell(object sender, UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Value.ToString().Trim() == "")
                    e.Value = DBNull.Value;
            }
            catch
            {
                if (e.Value.ToString().Trim() == "")
                    e.Value = DBNull.Value;
            }

        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                //gridEX1.UpdateData();

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select min(Column01) from Table_007_FactorBefore2),0) as Row");

                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_007_FactorBefore2 where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet6.EnforceConstraints = false;
                    this.table_007_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_007_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_009_Child2_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_009_Child2_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_008_Child1_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_008_Child1_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet6.EnforceConstraints = true;
                    this.table_007_FactorBeforeBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            try
            {
                //gridEX1.UpdateData();

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString,
                       "Select ISNULL((Select max(Column01) from Table_007_FactorBefore2 where Column01<" +
                       ((DataRowView)this.table_007_FactorBefore2BindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");

                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_007_FactorBefore2 where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet6.EnforceConstraints = false;
                    this.table_007_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_007_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_009_Child2_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_009_Child2_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_008_Child1_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_008_Child1_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet6.EnforceConstraints = true;
                    this.table_007_FactorBeforeBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            try
            {
                //gridEX1.UpdateData();

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select Min(Column01) from Table_007_FactorBefore2 where Column01>" + ((DataRowView)this.table_007_FactorBefore2BindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_007_FactorBefore2 where Column01=" + Table.Rows[0]["Row"].ToString());

                    dataSet6.EnforceConstraints = false;
                    this.table_007_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_007_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_009_Child2_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_009_Child2_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_008_Child1_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_008_Child1_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet6.EnforceConstraints = true;
                    this.table_007_FactorBeforeBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                //gridEX1.UpdateData();
                //table_007_FactorBeforeBindingSource.EndEdit();
                //this.table_008_Child1_FactorBeforeBindingSource.EndEdit();
                //this.table_009_Child2_FactorBeforeBindingSource.EndEdit();

                //if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                //    dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                //{
                //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                //    {
                //        bt_Save_Click(sender, e);
                //    }
                //}

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select max(Column01) from Table_007_FactorBefore2),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_007_FactorBefore2 where Column01=" + Table.Rows[0]["Row"].ToString());

                    dataSet6.EnforceConstraints = false;
                    this.table_007_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_007_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_009_Child2_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_009_Child2_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_008_Child1_FactorBefore2TableAdapter.Fill_New(this.dataSet6.Table_008_Child1_FactorBefore2, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet6.EnforceConstraints = true;

                    this.table_007_FactorBeforeBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
        }

        private void gridEX_List_AddingRecord(object sender, CancelEventArgs e)
        {
            gridEX_List.SetValue("Column08", Class_BasicOperation._UserName);
            gridEX_List.SetValue("Column09", Class_BasicOperation.ServerDate());
            gridEX_List.SetValue("Column10", Class_BasicOperation._UserName);
            gridEX_List.SetValue("Column11", Class_BasicOperation.ServerDate());
        }







    }
}
