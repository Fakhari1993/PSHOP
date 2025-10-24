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

namespace PSHOP._07_Services
{
    public partial class Form02_RegisterServiceFactor : Form
    {
        bool _Del = false;
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        int _ID = 0;

        public Form02_RegisterServiceFactor(int ID, bool Del)
        {
            InitializeComponent();
            _Del = Del;
            _ID = ID;
        }


        private void Form02_RegisterServiceFactor_Load(object sender, EventArgs e)
        {
            foreach (GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
            }

            if (_ID != 0)
            {
                this.table_031_ServiceFactorTableAdapter.Fill(dataset_Services.Table_031_ServiceFactor, _ID);
                this.table_032_ServiceFactor_Child1TableAdapter.Fill(dataset_Services.Table_032_ServiceFactor_Child1, _ID);
                this.table_033_ServiceFactor_Child2TableAdapter.Fill(dataset_Services.Table_033_ServiceFactor_Child2, _ID);
                table_031_ServiceFactorBindingSource_PositionChanged(sender, e);
            }

            gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");

            gridEX1.DropDowns["Customer"].SetDataBinding(clDoc.ReturnTable
        (ConBase.ConnectionString, @"SELECT     dbo.Table_045_PersonInfo.ColumnId as ColumnId, dbo.Table_045_PersonInfo.Column01 AS Column01, dbo.Table_045_PersonInfo.Column02 AS Column02, 
                      dbo.Table_065_CityInfo.Column02 AS City, dbo.Table_060_ProvinceInfo.Column01 AS Province, dbo.Table_045_PersonInfo.Column06 AS Address, 
                      dbo.Table_045_PersonInfo.Column30
FROM         dbo.Table_060_ProvinceInfo INNER JOIN
                      dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00 INNER JOIN
                      dbo.Table_045_PersonInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
WHERE     (dbo.Table_045_PersonInfo.Column12 = 1)"), "");

            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_070_CountUnitInfo"), "");
            gridEX_List.DropDowns["Service"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01,Column02,Column03,Column04 from Table_030_Services"), "");
            gridEX_Extra.DropDowns["Type"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "SELECT * FROM Table_024_Discount"), "");


        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.Enabled = true;
                gridEX1.MoveToNewRecord();
                gridEX1.Select();
                gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd"));
                gridEX1.Col = 1;
                bt_New.Enabled = false;
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (this.table_031_ServiceFactorBindingSource.Count > 0 &&
               gridEX_List.AllowEdit == Janus.Windows.GridEX.InheritableBoolean.True)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    gridEX_List.UpdateData();
                    gridEX_Extra.UpdateData();
                    DataRowView Row = (DataRowView)this.table_031_ServiceFactorBindingSource.CurrencyManager.Current;
                    if (Row["Column01"].ToString().StartsWith("-"))
                    {
                        gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_031_ServiceFactor", "Column01").ToString());
                        this.table_031_ServiceFactorBindingSource.EndEdit();
                    }
                    Row["Column06"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column06"], Janus.Windows.GridEX.AggregateFunction.Sum).ToString();

                    //Extra-Reductions
                    Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                    Row["Column08"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                    Filter.Value1 = true;
                    Row["Column07"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                    this.table_031_ServiceFactorBindingSource.EndEdit();
                    this.table_032_ServiceFactor_Child1BindingSource.EndEdit();
                    this.table_033_ServiceFactor_Child2BindingSource.EndEdit();
                    this.table_031_ServiceFactorTableAdapter.Update(dataset_Services.Table_031_ServiceFactor);
                    this.table_032_ServiceFactor_Child1TableAdapter.Update(dataset_Services.Table_032_ServiceFactor_Child1);
                    this.table_033_ServiceFactor_Child2TableAdapter.Update(dataset_Services.Table_033_ServiceFactor_Child2);
                    Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                    table_031_ServiceFactorBindingSource_PositionChanged(sender, e);
                    bt_New.Enabled = true;
                    this.Cursor = Cursors.Default;


                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }



        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (this.table_031_ServiceFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!_Del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                    DataRowView Row = (DataRowView)this.table_031_ServiceFactorBindingSource.CurrencyManager.Current;

                    int DocId = (Row["Column05"].ToString().Trim() != "0" ? int.Parse(Row["Column05"].ToString()) : 0);
                    if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        if (DocId > 0)
                        {
                            //حذف سند فاکتور 
                            clDoc.DeleteDetail_ID(DocId, 22, int.Parse(Row["ColumnId"].ToString()));
                        }
                        //حذف فاکتور
                        foreach (DataRowView item in this.table_032_ServiceFactor_Child1BindingSource)
                        {
                            item.Delete();
                        }
                        this.table_032_ServiceFactor_Child1BindingSource.EndEdit();
                        this.table_032_ServiceFactor_Child1TableAdapter.Update(dataset_Services.Table_032_ServiceFactor_Child1);
                        foreach (DataRowView item in this.table_033_ServiceFactor_Child2BindingSource)
                        {
                            item.Delete();
                        }
                        this.table_033_ServiceFactor_Child2BindingSource.EndEdit();
                        this.table_033_ServiceFactor_Child2TableAdapter.Update(dataset_Services.Table_033_ServiceFactor_Child2);
                        this.table_031_ServiceFactorBindingSource.RemoveCurrent();
                        this.table_031_ServiceFactorBindingSource.EndEdit();
                        this.table_031_ServiceFactorTableAdapter.Update(dataset_Services.Table_031_ServiceFactor);
                        Class_BasicOperation.ShowMsg("", "حذف فاکتور با موفقیت انجام گرفت", "Information");
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            gridEX_List.UpdateData();
            gridEX_Extra.UpdateData();
            gridEX1.UpdateData();
            bt_Save_Click(null, null);
            if (this.table_031_ServiceFactorBindingSource.Count > 0
                && gridEX1.GetValue("Column01") != null && Convert.ToInt32(gridEX1.GetValue("Column01")) > 0)
            {


                try
                {
                    //مشتری
                    string CommandText = @"select Column02 as Name,Column01 as Code,Column09 as Economic,
                         Column07 as Tel,Column09 as NationalCode,Column13 as PostalCode,Column06 as Address from {0}.dbo.Table_045_PersonInfo 
                         where ColumnId=" + gridEX1.GetValue("Column03").ToString();
                    CommandText = string.Format(CommandText, ConBase.Database);

                    DataTable CustomerTable = clDoc.ReturnTable(ConBase.ConnectionString, CommandText);

                    //خدمات
                    DataTable ItemTable = dataset_Services.Rpt_PrintFactor_Items.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        ItemTable.Rows.Add(item.Cells["Column02"].Text, item.Cells["Column04"].Text,
                            item.Cells["Column03"].Value.ToString(),
                            item.Cells["Column05"].Value.ToString(),
                            item.Cells["Column06"].Value.ToString(),
                            item.Cells["Column07"].Text.Trim(),
                            gridEX1.GetValue("Column01").ToString(),
                            gridEX1.GetValue("Column02").ToString(),
                            gridEX1.GetValue("Column04").ToString(),
                            txt_Extra.Value, txt_Reductions.Value);
                    }

                    //اضافات و کسورات
                    DataTable ExReTable = dataset_Services.Rpt_PrintFactor_ExtraReducitons.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                    {
                        ExReTable.Rows.Add(item.Cells["Column02"].Text,
                            (item.Cells["Column05"].Value.ToString() == "True" ? "-" : "+"),
                            item.Cells["Column03"].Value.ToString(), item.Cells["Column04"].Value.ToString());
                    }

                    _07_Services.Reports.PrintFactor frm = new Reports.PrintFactor(2, ItemTable, CustomerTable, ExReTable,
                        FarsiLibrary.Utils.ToWords.ToString(Int64.Parse(txt_EndPrice.Value.ToString())), " ", Convert.ToInt32(gridEX1.GetValue("Column01")));
                    frm.ShowDialog();
                }
                catch
                {
                }
            }
        }

        private void table_031_ServiceFactorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.table_031_ServiceFactorBindingSource.Count > 0)
                {
                    DataRowView Row = (DataRowView)this.table_031_ServiceFactorBindingSource.CurrencyManager.Current;
                    //در صورت اینکه فاکتور دارای سند باشد
                    if (Row["Column05"].ToString().Trim() != "0")
                    {
                        gridEX1.AllowEdit = InheritableBoolean.False;
                        gridEX1.Enabled = true;
                        gridEX_List.AllowEdit = InheritableBoolean.False;
                        gridEX_Extra.AllowEdit = InheritableBoolean.False;
                        gridEX_List.AllowAddNew = InheritableBoolean.False;
                        gridEX_Extra.AllowAddNew = InheritableBoolean.False;
                        gridEX_Extra.AllowDelete = InheritableBoolean.False;
                        gridEX_List.AllowDelete = InheritableBoolean.False;
                    }
                    else
                    {
                        gridEX1.AllowEdit = InheritableBoolean.True;
                        gridEX1.Enabled = true;
                        gridEX_List.AllowEdit = InheritableBoolean.True;
                        gridEX_Extra.AllowEdit = InheritableBoolean.True;
                        gridEX_List.AllowAddNew = InheritableBoolean.True;
                        gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                        gridEX_Extra.AllowDelete = InheritableBoolean.True;
                        gridEX_List.AllowDelete = InheritableBoolean.True;
                    }

                    txt_EndPrice.Value = Int64.Parse(txt_TotalPrice.Value.ToString()) + Int64.Parse(txt_Extra.Value.ToString()) - Int64.Parse(txt_Reductions.Value.ToString());
                }

            }
            catch
            { }
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
                    dataset_Services.EnforceConstraints = false;
                    this.table_031_ServiceFactorTableAdapter.Fill(dataset_Services.Table_031_ServiceFactor, ReturnIDNumber(int.Parse(txt_Search.Text)));
                    this.table_032_ServiceFactor_Child1TableAdapter.Fill(dataset_Services.Table_032_ServiceFactor_Child1, ReturnIDNumber(int.Parse(txt_Search.Text)));
                    this.table_033_ServiceFactor_Child2TableAdapter.Fill(dataset_Services.Table_033_ServiceFactor_Child2, ReturnIDNumber(int.Parse(txt_Search.Text)));
                    dataset_Services.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_031_ServiceFactorBindingSource_PositionChanged(sender, e);
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
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                Con.Open();
                int ID = 0;
                SqlCommand Commnad = new SqlCommand("Select ISNULL(ColumnId,0) from Table_031_ServiceFactor where Column01=" + FactorNum, Con);
                try
                {
                    ID = int.Parse(Commnad.ExecuteScalar().ToString());
                    return ID;
                }
                catch
                {
                    throw new Exception("شماره فاکتور وارد شده نامعتبر است");
                }
            }
        }

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            if (this.table_031_ServiceFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 107))
                        throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");

                    DataRowView Row = (DataRowView)this.table_031_ServiceFactorBindingSource.CurrencyManager.Current;
                    if (Row["Column05"].ToString() != "0")
                        throw new Exception("برای این فاکتور سند صادر شده است");

                    _07_Services.Form04_ExportDocDialog frm = new Form04_ExportDocDialog(int.Parse(Row["ColumnId"].ToString()));
                    frm.ShowDialog();

                    gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                    int _ID = int.Parse(Row["ColumnId"].ToString());
                    dataset_Services.EnforceConstraints = false;
                    this.table_031_ServiceFactorTableAdapter.Fill(this.dataset_Services.Table_031_ServiceFactor, _ID);
                    this.table_032_ServiceFactor_Child1TableAdapter.Fill(this.dataset_Services.Table_032_ServiceFactor_Child1, _ID);
                    this.table_033_ServiceFactor_Child2TableAdapter.Fill(this.dataset_Services.Table_033_ServiceFactor_Child2, _ID);
                    dataset_Services.EnforceConstraints = true;
                    this.table_031_ServiceFactorBindingSource_PositionChanged(sender, e);

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_031_ServiceFactorBindingSource.Count > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 108))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    DataRowView Row = (DataRowView)this.table_031_ServiceFactorBindingSource.CurrencyManager.Current;

                    int _HeaderId = int.Parse(Row["ColumnId"].ToString());

                    if (int.Parse(Row["Column05"].ToString()) > 0)
                    {
                        string Message = "آیا مایل به حذف سند مربوط به این فاکتور هستید؟";

                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            //حذف سند فاکتور 
                            clDoc.DeleteDetail_ID(int.Parse(Row["Column05"].ToString()), 22, _HeaderId);

                            //آپدیت شماره سند  در فاکتور
                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_031_ServiceFactor", "Column05", "ColumnId", int.Parse(Row["ColumnId"].ToString()), 0);

                            dataset_Services.EnforceConstraints = false;
                            this.table_031_ServiceFactorTableAdapter.Fill(this.dataset_Services.Table_031_ServiceFactor, _HeaderId);
                            this.table_032_ServiceFactor_Child1TableAdapter.Fill(this.dataset_Services.Table_032_ServiceFactor_Child1, _HeaderId);
                            this.table_033_ServiceFactor_Child2TableAdapter.Fill(this.dataset_Services.Table_033_ServiceFactor_Child2, _HeaderId);
                            dataset_Services.EnforceConstraints = true;
                            this.table_031_ServiceFactorBindingSource_PositionChanged(sender, e);

                            Class_BasicOperation.ShowMsg("", "حذف سند حسابداری با موفقیت صورت گرفت", "Information");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void bt_ViewFactors_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 109))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form03_ViewServiceFactors")
                    {
                        child.Focus();
                        return;
                    }
                }

                _07_Services.Form03_ViewServiceFactors frm = new _07_Services.Form03_ViewServiceFactors();
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

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            ((GridEX)sender).CurrentCellDroppedDown = true;
        }

        private void gridEX1_RowEditCanceled(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            gridEX1.Enabled = false;
            bt_New.Enabled = true;
        }

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "Column05")
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

        private void gridEX_List_Enter(object sender, EventArgs e)
        {
            try
            {
                table_031_ServiceFactorBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                gridEX1.Focus();
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_List_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_List_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_List.CurrentCellDroppedDown = true;
        }

        private void gridEX_List_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.Key == "Column02")
            {
                try
                {
                    gridEX_List.SetValue("Column04", gridEX_List.DropDowns["Service"].GetValue("Column02").ToString());
                    gridEX_List.SetValue("Column05", gridEX_List.DropDowns["Service"].GetValue("Column04").ToString());
                }
                catch { }
            }
            try
            {
                gridEX_List.SetValue("Column06", double.Parse(gridEX_List.GetValue("Column03").ToString()) * double.Parse(gridEX_List.GetValue("Column05").ToString()));
            }
            catch { }
        }


        private void gridEX_Extra_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                txt_EndPrice.Value = Int64.Parse(txt_TotalPrice.Value.ToString()) + Int64.Parse(txt_Extra.Value.ToString()) - Int64.Parse(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }

        private void gridEX_Extra_UpdatingCell(object sender, UpdatingCellEventArgs e)
        {
            if (e.Column.Key == "Column02")
            {
                try
                {

                    gridEX_Extra.SetValue("Column05", (gridEX_Extra.DropDowns["Type"].GetValue("Column02")));
                    gridEX_Extra.SetValue("Column04", "0");
                    gridEX_Extra.SetValue("Column03", "0");

                    if (gridEX_Extra.DropDowns["Type"].GetValue("Column03").ToString() == "True")
                    {
                        gridEX_Extra.SetValue("Column04", gridEX_Extra.DropDowns["Type"].GetValue("Column04").ToString());
                    }
                    else
                    {

                        gridEX_Extra.SetValue("Column03", gridEX_Extra.DropDowns["Type"].GetValue("Column04").ToString());
                        Double darsad;
                        darsad = Convert.ToDouble(gridEX_Extra.DropDowns["Type"].GetValue("Column04").ToString());

                        Double kol;
                        kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["Column06"].Value.ToString());
                        if (kol == 0)
                            return;
                        gridEX_Extra.SetValue("Column04", kol * darsad / 100);
                    }
                }
                catch { }
            }
            else if (e.Column.Key == "Column03")
            {
                try
                {
                    Double darsad;
                    darsad = Convert.ToDouble(e.Value.ToString());
                    Double kol;
                    kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["Column06"].Value.ToString());
                    if (kol == 0)
                        return;
                    gridEX_Extra.SetValue("Column04", kol * darsad / 100);
                }
                catch { }
            }
        }

        private void gridEX_List_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                //Update TotalTextbox
                txt_TotalPrice.Value = Int64.Parse(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column06"], AggregateFunction.Sum).ToString());
                txt_EndPrice.Value = Int64.Parse(txt_TotalPrice.Value.ToString()) + Int64.Parse(txt_Extra.Value.ToString()) - Int64.Parse(txt_Reductions.Value.ToString());
            }
            catch
            {
            }

        }

        private void mnu_Customers_Click(object sender, EventArgs e)
        {
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 5))
            {
                PACNT._1_BasicMenu.Form03_Persons frm = new PACNT._1_BasicMenu.Form03_Persons(
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 6));
                frm.ShowDialog();
                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT   * from ListPeople(5)", ConBase);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, ConSale.Database);
                DataTable CustomerTbl = new DataTable();
                Adapter.Fill(CustomerTbl);
                gridEX1.DropDowns["Customer"].SetDataBinding(CustomerTbl, "");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_DefineServices_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 103))
            {
                _07_Services.Form01_DefineServices frm = new _07_Services.Form01_DefineServices(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 104));
                frm.ShowDialog();
                gridEX_List.DropDowns["Service"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01,Column02,Column03,Column04 from Table_030_Services"), "");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ExtraDiscount_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 45))
            {
                _02_BasicInfo.Frm_002_TakhfifEzafeSale ob = new _02_BasicInfo.Frm_002_TakhfifEzafeSale(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 46));
                ob.ShowDialog();
                gridEX_Extra.DropDowns["Type"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "SELECT * FROM Table_024_Discount"), "");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            int SanadId = 0;
            if (this.table_031_ServiceFactorBindingSource.Count > 0)
                SanadId = (((DataRowView)this.table_031_ServiceFactorBindingSource.CurrencyManager.Current)["Column05"].ToString() == "0" ? 0 : int.Parse(((DataRowView)this.table_031_ServiceFactorBindingSource.CurrencyManager.Current)["Column05"].ToString()));
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 19))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form01_AccDocument")
                    {
                        item.BringToFront();
                        TextBox txt_S = (TextBox)item.ActiveControl;
                        txt_S.Text = SanadId.ToString();
                        PACNT._2_DocumentMenu.Form01_AccDocument frms = (PACNT._2_DocumentMenu.Form01_AccDocument)item;
                        frms.bt_Search_Click(sender, e);
                        return;
                    }
                }
                PACNT._2_DocumentMenu.Form01_AccDocument frm = new PACNT._2_DocumentMenu.Form01_AccDocument(
                  UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 20), int.Parse(SanadId.ToString()));
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

        private void Form02_RegisterServiceFactor_Activated(object sender, EventArgs e)
        {
            txt_Search.Focus();
        }

        private void Form02_RegisterServiceFactor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control && bt_New.Enabled)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
            {
                txt_Search.Select();
                txt_Search.SelectAll();
            }
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.E)
                bt_ExportDoc_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.L)
                bt_DelDoc_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F8)
                toolStripButton7.ShowDropDown();
        }

        private void bt_AddExtra_Click(object sender, EventArgs e)
        {
            if (gridEX_Extra.AllowAddNew == InheritableBoolean.True && this.table_031_ServiceFactorBindingSource.Count > 0 && this.table_032_ServiceFactor_Child1BindingSource.Count > 0)
            {
                try
                {
                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount");
                    foreach (DataRow item in Table.Rows)
                    {
                        this.table_033_ServiceFactor_Child2BindingSource.AddNew();
                        DataRowView New = (DataRowView)this.table_033_ServiceFactor_Child2BindingSource.CurrencyManager.Current;
                        New["Column02"] = item["ColumnId"].ToString();
                        if (item["Column03"].ToString() == "True")
                            New["Column04"] = item["Column04"].ToString();
                        else
                        {
                            New["Column03"] = item["Column04"].ToString();
                            New["Column04"] = double.Parse(item["Column04"].ToString()) *
                                double.Parse(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column06"], AggregateFunction.Sum).ToString()) / 100;
                        }
                        New["Column05"] = item["Column02"].ToString();
                        this.table_033_ServiceFactor_Child2BindingSource.EndEdit();
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }




    }
}
