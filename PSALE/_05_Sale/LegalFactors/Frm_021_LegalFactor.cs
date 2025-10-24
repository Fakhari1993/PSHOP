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

namespace PSHOP._05_Sale.LegalFactors
{
    public partial class Frm_021_LegalFactor : Form
    {
        int _ID = 0;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_Discounts ClDiscount = new Classes.Class_Discounts();
        Class_UserScope UserScope = new Class_UserScope();
        DataSet DS = new DataSet();
        List<string> CustomerGroupList = new List<string>();
        string[] CustomerGroupsArray;


        public Frm_021_LegalFactor()
        {
            InitializeComponent();
        }

        public Frm_021_LegalFactor( int ID)
        {
            _ID = ID;
            InitializeComponent();
        }

        private void Frm_002_PishFaktor_Load(object sender, EventArgs e)
        { 
            foreach (GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
                if (col.Key == "Column13" || col.Key == "Column15")
                    col.DefaultValue = Class_BasicOperation._UserName;
                if (col.Key == "Column14" || col.Key == "Column16")
                    col.DefaultValue = Class_BasicOperation.ServerDate();
            }

            GoodbindingSource.DataSource = clGood.MahsoolInfo("");
            DataTable GoodTable = clGood.MahsoolInfo("");
            gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            CustomerPricingbindingSource.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, @"Select * from Table_029_CustomerGroupGoodPricing 
                    inner join   (
                    select Min(ColumnId) as ID from Table_029_CustomerGroupGoodPricing as Table_029_CustomerGroupGoodPricing_1
                    group by Column02,Column01   ) as Tbl
                    ON Table_029_CustomerGroupGoodPricing.ColumnId=Tbl.ID
                    order by Table_029_CustomerGroupGoodPricing.ColumnId
                    ");

            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_045_PersonInfo.ColumnId AS id, dbo.Table_045_PersonInfo.Column01 AS code, dbo.Table_045_PersonInfo.Column02 AS name, 
                      dbo.Table_065_CityInfo.Column02 AS shahr, dbo.Table_060_ProvinceInfo.Column01 AS ostan, dbo.Table_045_PersonInfo.Column06 AS Address
FROM         dbo.Table_060_ProvinceInfo INNER JOIN
                      dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00 INNER JOIN
                      dbo.Table_045_PersonInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
WHERE     (dbo.Table_045_PersonInfo.Column12 = 1)", ConBase);
            Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, ConSale.Database);
            Adapter.Fill(DS, "Customer");
            gridEX1.DropDowns["Customer"].SetDataBinding(DS.Tables["Customer"], "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], ""); 

            Adapter = new SqlDataAdapter("Select * FROM Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(DS, "Center");
            gridEX_List.DropDowns["Center"].SetDataBinding(DS.Tables["Center"], "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_035_ProjectInfo", ConBase);
            Adapter.Fill(DS, "Project");
            gridEX_List.DropDowns["Project"].SetDataBinding(DS.Tables["Project"], "");


            gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)"), "");
            gridEX1.DropDowns["SaleFactor"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString,
                "Select ColumnId,Column01 from Table_010_SaleFactor where Column38=1"), "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
            Adapter.Fill(DS, "Discount");
            gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");

            gridEX1.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT ColumnId,Column01,Column02 from Table_002_SalesTypes"), "");
            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_055_CurrencyInfo");
            gridEX1.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");

            if (_ID != 0)
            {
                this.table_055_LegalFactorsTableAdapter.Fill_HeaderId(this.dataSet_Sale2.Table_055_LegalFactors, _ID);
                this.table_065_Child2_LegalFactorsTableAdapter.Fill(this.dataSet_Sale2.Table_065_Child2_LegalFactors, _ID);
                this.table_060_Child1_LegalFactorsTableAdapter.Fill(this.dataSet_Sale2.Table_060_Child1_LegalFactors, _ID);
                Table_055_LegalFactorsBindingSource_PositionChanged(sender, e);
            }

        }

     
      
        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (this.table_055_LegalFactorsBindingSource.Count > 0 &&
                gridEX_List.AllowEdit == InheritableBoolean.True &&
                gridEX1.GetRow().Cells["Column03"].Text.Trim() != "")
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    gridEX_List.UpdateData();
                    gridEX_Extra.UpdateData();
                    DataRowView Row = (DataRowView)this.table_055_LegalFactorsBindingSource.CurrencyManager.Current;
                    if (Row["Column01"].ToString().StartsWith("-"))
                    {
                        gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_055_LegalFactors", "Column01").ToString());
                        this.table_055_LegalFactorsBindingSource.EndEdit();
                    }
                    txt_TotalPrice.Value = Convert.ToDouble(
                  gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                  AggregateFunction.Sum).ToString());
                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) -
                        Convert.ToDouble(txt_VolumeGroup.Value.ToString()) -
                        Convert.ToDouble(txt_SpecialGroup.Value.ToString()) -
                        Convert.ToDouble(txt_SpecialCustomer.Value.ToString());
                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                        Convert.ToDouble(txt_Extra.Value.ToString()) -
                        Convert.ToDouble(txt_Reductions.Value.ToString());
                    double Total = double.Parse(txt_TotalPrice.Value.ToString());
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                    {
                        if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                        {
                            item.BeginEdit();
                            item.Cells["Column04"].Value = Total * double.Parse(item.Cells["Column03"].Value.ToString()) / 100;
                            item.EndEdit();

                        }
                    }
                    Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                    txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                    Filter.Value1 = true;
                    txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                    Row["Column15"] = Class_BasicOperation._UserName;
                    Row["Column16"] = Class_BasicOperation.ServerDate();
                    Row["Column34"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                    Row["Column35"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();


                    //Extra-Reductions
                    Row["Column32"] = txt_Extra.Value.ToString();
                    Row["Column33"] = txt_Reductions.Value.ToString();

                    this.table_055_LegalFactorsBindingSource.EndEdit();
                    this.table_060_Child1_LegalFactorsBindingSource.EndEdit();
                    this.table_065_Child2_LegalFactorsBindingSource.EndEdit();
                    this.table_055_LegalFactorsTableAdapter.Update(dataSet_Sale2.Table_055_LegalFactors);
                    this.table_060_Child1_LegalFactorsTableAdapter.Update(dataSet_Sale2.Table_060_Child1_LegalFactors);
                    this.table_065_Child2_LegalFactorsTableAdapter.Update(dataSet_Sale2.Table_065_Child2_LegalFactors);

                      if(sender==bt_Save || sender==this)
                    Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                    int _ID = int.Parse(Row["ColumnId"].ToString());
                    dataSet_Sale2.EnforceConstraints = false;
                    this.table_055_LegalFactorsTableAdapter.Fill_HeaderId(this.dataSet_Sale2.Table_055_LegalFactors, _ID);
                    this.table_065_Child2_LegalFactorsTableAdapter.Fill(this.dataSet_Sale2.Table_065_Child2_LegalFactors, _ID);
                    this.table_060_Child1_LegalFactorsTableAdapter.Fill(this.dataSet_Sale2.Table_060_Child1_LegalFactors, _ID);
                    dataSet_Sale2.EnforceConstraints = true;
                    Table_055_LegalFactorsBindingSource_PositionChanged(sender, e);
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
            if (this.table_055_LegalFactorsBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName,"Column11",123))
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                    DataRowView Row = (DataRowView)this.table_055_LegalFactorsBindingSource.CurrencyManager.Current;

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف کامل فاکتور هستید؟" 
                        , "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                      //حذف فاکتور
                        foreach (DataRowView item in this.table_060_Child1_LegalFactorsBindingSource)
                        {
                            item.Delete();
                        }
                        this.table_060_Child1_LegalFactorsBindingSource.EndEdit();
                        this.table_060_Child1_LegalFactorsTableAdapter.Update(dataSet_Sale2.Table_060_Child1_LegalFactors);
                        foreach (DataRowView item in this.table_065_Child2_LegalFactorsBindingSource)
                        {
                            item.Delete();
                        }
                        int SaleId=int.Parse(gridEX1.GetValue("Column37").ToString());
                        this.table_065_Child2_LegalFactorsBindingSource.EndEdit();
                        this.table_065_Child2_LegalFactorsTableAdapter.Update(dataSet_Sale2.Table_065_Child2_LegalFactors);
                        this.table_055_LegalFactorsBindingSource.RemoveCurrent();
                        this.table_055_LegalFactorsBindingSource.EndEdit();
                        this.table_055_LegalFactorsTableAdapter.Update(dataSet_Sale2.Table_055_LegalFactors);
                        clDoc.RunSqlCommand(Properties.Settings.Default.SALE, "UPDATE Table_010_SaleFactor SET Column37=null,Column38=0,Column39=null where ColumnId=" + SaleId);
                        Class_BasicOperation.ShowMsg("", "حذف فاکتور با موفقیت انجام گرفت", "Information");
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }

        private void gridEX_List_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_List_Enter(object sender, EventArgs e)
        {
            try
            {

                table_055_LegalFactorsBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                gridEX1.Focus();
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Extra_Enter(object sender, EventArgs e)
        {
            try
            {

                table_055_LegalFactorsBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                gridEX1.Focus();
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Extra_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {
            if (e.Column.Key == "column02")
            {

                gridEX_Extra.SetValue("column05", (gridEX_Extra.DropDowns["Type"].GetValue("column02")));
                gridEX_Extra.SetValue("column04", "0");
                gridEX_Extra.SetValue("column03", "0");

                if (gridEX_Extra.DropDowns["Type"].GetValue("column03").ToString() == "True")
                {
                    gridEX_Extra.SetValue("column04", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
                }
                else
                {

                    gridEX_Extra.SetValue("column03", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
                    Double darsad;
                    darsad = Convert.ToDouble(gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());

                    Double kol;
                    kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column20"].Value.ToString());
                    if (kol == 0)
                        return;
                    gridEX_Extra.SetValue("column04", kol * darsad / 100);
                }
            }
            else if (e.Column.Key == "column03")
            {
                Double darsad;
                darsad = Convert.ToDouble(e.Value.ToString());
                Double kol;
                kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column20"].Value.ToString());
                if (kol == 0)
                    return;
                gridEX_Extra.SetValue("column04", kol * darsad / 100);
            }
        }

        private void gridEX_Extra_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "");
        }

        private void gridEX_List_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_List.CurrentCellDroppedDown = true;
        }

        private void gridEX_Extra_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_Extra.CurrentCellDroppedDown = true;
        }

        private void Table_055_LegalFactorsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.table_055_LegalFactorsBindingSource.Count > 0)
                {
                    DataRowView Row = (DataRowView)this.table_055_LegalFactorsBindingSource.CurrencyManager.Current;
                    //اگر برای فاکتور فقط حواله صادر شده باشد 
                    if (Row["Column09"].ToString() != "0" && Row["Column10"].ToString() == "0")
                    {
                        gridEX1.AllowEdit = InheritableBoolean.False;
                        gridEX1.Enabled = true;
                        gridEX_List.AllowAddNew = InheritableBoolean.True;
                        gridEX_List.AllowEdit = InheritableBoolean.True;
                        gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                        gridEX_Extra.AllowDelete = InheritableBoolean.True;
                        gridEX_List.AllowDelete = InheritableBoolean.True;
                    }
                        //در صورت اینکه فاکتور دارای سند باشد، یا مرجوعی باشد یا باطل شده باشد یا دارای پیش فاکتور باشد
                    else if (Row["Column10"].ToString().Trim() != "0"
                            || Row["Column07"].ToString().Trim() != "0"
                            || Row["Column17"].ToString().Trim() != "False"
                            || Row["Column19"].ToString().Trim() != "False")
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
                   
                        try
                        {
                            txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) - Convert.ToDouble(txt_VolumeGroup.Value.ToString()) - Convert.ToDouble(txt_SpecialGroup.Value.ToString()) - Convert.ToDouble(txt_SpecialCustomer.Value.ToString());
                            txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());

                            CustomerGroupList.Clear();
                            CustomerGroupsArray = CustomerGroupList.ToArray();
                            //حذف آیتم های قبلی از آرایه
                            if (CustomerGroupsArray.Length > 0)
                                Array.Clear(CustomerGroupsArray, 0, CustomerGroupsArray.Length);

                            DataTable Table = clDoc.ReturnTable(ConBase.ConnectionString, "select Column02 from Table_045_PersonScope where Column01=" + gridEX1.GetValue("Column03").ToString());
                            foreach (DataRow item in Table.Rows)
                            {
                                CustomerGroupList.Add(item["Column02"].ToString());
                            }
                            if (CustomerGroupList.Count > 0)
                                CustomerGroupsArray = CustomerGroupList.ToArray();

                        }
                        catch
                        {
                        }
                }

            }
            catch
            { }
        }

        private void Frm_002_Faktor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
            {
                txt_Search.Select();
                txt_Search.SelectAll();
            }
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
          
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (this.table_055_LegalFactorsBindingSource.Count > 0)
            {
                bt_Save_Click(sender, e);
                _05_Sale.LegalFactors.Reports.Form_LegalFactorPrint frm = new  Reports.Form_LegalFactorPrint(
                    int.Parse(gridEX1.GetValue("Column01").ToString()));
                frm.ShowDialog();
            }
        }

        public void bt_Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
            {
                try
                {
                    gridEX_Extra.UpdateData();
                    gridEX_List.UpdateData();
                    //this.table_055_LegalFactorsBindingSource.EndEdit();
                    //this.table_060_Child1_LegalFactorsBindingSource.EndEdit();
                    //this.table_065_Child2_LegalFactorsBindingSource.EndEdit();
                    //if (dataSet_Sale2.Table_055_LegalFactors.GetChanges() != null || 
                    //    dataSet_Sale2.Table_060_Child1_LegalFactors.GetChanges() != null
                    //    || dataSet_Sale2.Table_065_Child2_LegalFactors.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        bt_Save_Click(sender, e);
                    //    }
                    //}

                    dataSet_Sale2.EnforceConstraints = false;
                    this.table_055_LegalFactorsTableAdapter.Fill_HeaderId(dataSet_Sale2.Table_055_LegalFactors, ReturnIDNumber(int.Parse(txt_Search.Text)));
                    this.table_060_Child1_LegalFactorsTableAdapter.Fill(dataSet_Sale2.Table_060_Child1_LegalFactors, ReturnIDNumber(int.Parse(txt_Search.Text)));
                    this.table_065_Child2_LegalFactorsTableAdapter.Fill(dataSet_Sale2.Table_065_Child2_LegalFactors, ReturnIDNumber(int.Parse(txt_Search.Text)));
                    dataSet_Sale2.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.Table_055_LegalFactorsBindingSource_PositionChanged(sender, e);
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
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                con.Open();
                int ID = 0;
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_055_LegalFactors where column01=" + FactorNum, con);
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

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
                bt_Search_Click(sender, e);
        }

        private void bt_ViewFactors_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 67))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_008_ViewSaleFactors")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                _05_Sale.Frm_008_ViewSaleFactors frm = new Frm_008_ViewSaleFactors();
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
                 
        private void Frm_002_Faktor_Activated(object sender, EventArgs e)
        {
            txt_Search.Focus();
        }

        private void mnu_ExtraDiscount_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 45))
            {
                _02_BasicInfo.Frm_002_TakhfifEzafeSale ob = new _02_BasicInfo.Frm_002_TakhfifEzafeSale(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 46));
                ob.ShowDialog();
                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
                DS.Tables["Discount"].Rows.Clear();
                Adapter.Fill(DS, "Discount");
                //gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_GoodInformation_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 5))
            {
                _02_BasicInfo.Frm_009_AdditionalGoodsInfo ob = new _02_BasicInfo.Frm_009_AdditionalGoodsInfo(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 6));
                ob.ShowDialog();
                GoodbindingSource.DataSource = clGood.GoodInfo();
                DataTable Table = clGood.GoodInfo();
                gridEX_List.DropDowns["GoodCode"].SetDataBinding(Table, "");
                gridEX_List.DropDowns["GoodName"].SetDataBinding(Table, "");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
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
                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_045_PersonInfo.ColumnId AS id, dbo.Table_045_PersonInfo.Column01 AS code, dbo.Table_045_PersonInfo.Column02 AS name, 
                dbo.Table_065_CityInfo.Column02 AS shahr, dbo.Table_060_ProvinceInfo.Column01 AS ostan, dbo.Table_045_PersonInfo.Column06 AS Address
                FROM         dbo.Table_060_ProvinceInfo INNER JOIN
                dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00 INNER JOIN
                dbo.Table_045_PersonInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                WHERE     (dbo.Table_045_PersonInfo.Column12 = 1)", ConBase);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, ConSale.Database);
                DS.Tables["Customer"].Rows.Clear();
                Adapter.Fill(DS, "Customer");
                gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)"), "");
            }
             else
                 Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void Frm_002_Faktor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chk_Award_Box.Checked)
            {
                Properties.Settings.Default.AwardCompute = "Box";
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.AwardCompute = "Detail";
                Properties.Settings.Default.Save();
            }
        }

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "column13")
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

        private void gridEX1_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Control)
                gridEX1.CurrentCellDroppedDown = true;
                gridEX1.SetValue("Column15", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column16", Class_BasicOperation.ServerDate());

                if (e.Column.Key == "column12")
                {
                    if (gridEX1.GetValue("Column12").ToString() == "True")
                    {
                        gridEX1.RootTable.Columns["Column40"].Selectable = true;
                        gridEX1.RootTable.Columns["Column41"].Selectable = true;
                    }
                    else
                    {
                        gridEX1.RootTable.Columns["Column40"].Selectable = false;
                        gridEX1.RootTable.Columns["Column41"].Selectable = false;
                    }
                }

        }

        private void gridEX_Extra_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value= gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) - Convert.ToDouble(txt_VolumeGroup.Value.ToString()) - Convert.ToDouble(txt_SpecialGroup.Value.ToString()) - Convert.ToDouble(txt_SpecialCustomer.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch 
            {
            }
        }

        private void gridEX_List_EditingCell(object sender, EditingCellEventArgs e)
        {
            //try
            //{
            //    if (((DataRowView)this.Table_055_LegalFactorsBindingSource.CurrencyManager.Current)[
            //        "Column09"].ToString() != "0" &&
            //        ((DataRowView)this.Table_055_LegalFactorsBindingSource.CurrencyManager.Current)[
            //        "Column10"].ToString() == "0")
            //    {
            //        if (e.Column.Key == "column08" || e.Column.Key == "column09" || 
            //            e.Column.Key == "column10" || e.Column.Key == "column11" ||
            //            e.Column.Key == "column16" || e.Column.Key == "column18")
            //            e.Cancel = false;
            //        else
            //            e.Cancel = true;
            //    }
            //    else
            //    {
            //        if (gridEX_List.GetRow().Cells["column30"].Value.ToString() == "True")
            //            if (e.Column.Key != "column02" && e.Column.Key != "GoodCode")
            //                e.Cancel = false;
            //            else
            //                e.Cancel = true;
            //    }

            //}
            //catch
            //{
            //}
        }

        private void gridEX_List_FormattingRow(object sender, RowLoadEventArgs e)
        {
            if (this.table_055_LegalFactorsBindingSource.Count > 0)
            {
                try
                {
                    if (e.Row.RowType == Janus.Windows.GridEX.RowType.Record &&  
                        e.Row.Cells["column30"].Value.ToString() == "True")
                        e.Row.RowHeaderImageIndex = 0;
                }
                catch { }
            }
        }

        private void mnu_SaleType_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 3))
            {
                _02_BasicInfo.Frm_007_SaleType ob = new _02_BasicInfo.Frm_007_SaleType(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 4));
                ob.ShowDialog();
                gridEX1.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT ColumnId,Column01,Column02 from Table_002_SalesTypes"), "");

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX_List_DeletingRecord(object sender, RowActionCancelEventArgs e)
        {
            if (e.Row.Cells["column30"].Value.ToString() == "True")
            {
                if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف کالا و ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {
                    e.Row.Delete();
                    this.table_055_LegalFactorsBindingSource.EndEdit();
                    this.table_060_Child1_LegalFactorsBindingSource.EndEdit();
                    this.table_065_Child2_LegalFactorsBindingSource.EndEdit();
                    this.table_055_LegalFactorsTableAdapter.Update(dataSet_Sale2.Table_055_LegalFactors);
                    this.table_060_Child1_LegalFactorsTableAdapter.Update(dataSet_Sale2.Table_060_Child1_LegalFactors);
                    this.table_065_Child2_LegalFactorsTableAdapter.Update(dataSet_Sale2.Table_065_Child2_LegalFactors);
                }
                else
                    e.Cancel = true;
            }
            
        }

        private void gridEX1_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (e.Column.Key == "column03")
                {
                    CustomerGroupList.Clear();
                    CustomerGroupsArray = CustomerGroupList.ToArray();
                    //حذف آیتم های قبلی از آرایه
                    if(CustomerGroupsArray.Length>0)
                        Array.Clear(CustomerGroupsArray,0,CustomerGroupsArray.Length);

                    DataTable Table = clDoc.ReturnTable(ConBase.ConnectionString, "select Column02 from Table_045_PersonScope where Column01=" + gridEX1.GetValue("Column03").ToString());
                    foreach (DataRow item in Table.Rows)
                    {
                        CustomerGroupList.Add(item["Column02"].ToString());
                    }
                    if(CustomerGroupList.Count>0)
                        CustomerGroupsArray=CustomerGroupList.ToArray();
                }

                else if (e.Column.Key == "Column40")
                {
                    object Value = gridEX1.GetValue("Column40");
                    DataRowView Row = (DataRowView)this.gridEX1.RootTable.Columns["Column40"].DropDown.FindItem(Value);
                    gridEX1.SetValue("Column41", Row["Column02"]);
                }

                if (gridEX1.GetRow().Cells["Column40"].Text.Trim() != "" && gridEX1.GetRow().Cells["Column41"].Text.Trim() != "")
                {
                    gridEX_List.RootTable.Columns["Column14"].DefaultValue = gridEX1.GetValue("Column40");
                    gridEX_List.RootTable.Columns["Column15"].DefaultValue = gridEX1.GetValue("Column41");
                }
                else if (e.Column.Key == "column12")
                {
                    if (gridEX1.GetValue("Column12").ToString() == "False")
                    {
                        gridEX1.SetValue("Column40", DBNull.Value);
                        gridEX1.SetValue("Column41", 0);
                    }
                }

            }
            catch
            {
            }
        }

        private void gridEX_List_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (e.Column.Key == "column02")
                    gridEX_List.SetValue("GoodCode", gridEX_List.GetValue("column02").ToString());
                else if (e.Column.Key == "GoodCode")
                    gridEX_List.SetValue("column02", gridEX_List.GetValue("GoodCode").ToString());

                if (e.Column.Key == "column02" || e.Column.Key == "GoodCode" ||
                    gridEX_List.GetRow().Cells["column30"].Text.ToString() == "True")
                {
                    GoodbindingSource.Filter = "GoodID=" +
                        gridEX_List.GetRow().Cells["column02"].Value.ToString();
                    gridEX_List.SetValue("tedaddarkartoon",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                    gridEX_List.SetValue("tedaddarbaste",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                    gridEX_List.SetValue("column03",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                    gridEX_List.SetValue("column16",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());
                    gridEX_List.SetValue("column18",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Extra"].ToString());

                    if (CustomerGroupList.Count == 0)
                    {
                        gridEX_List.SetValue("column10",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePrice"].ToString());
                        gridEX_List.SetValue("column09",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePackPrice"].ToString());
                        gridEX_List.SetValue("column08",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SaleBoxPrice"].ToString());
                    }
                    else
                    {
                        CustomerPricingbindingSource.Filter = "Column01 IN (" +
                            string.Join(",", CustomerGroupsArray) + ") and Column02=" +
                            gridEX_List.GetValue("column02").ToString() +
                            " and Column03<='" + gridEX1.GetValue("Column02").ToString() +
                            "' and Column04>='" + gridEX1.GetValue("Column02").ToString() + "'";
                        if (CustomerPricingbindingSource.Count > 0)
                        {
                            gridEX_List.SetValue("column10",
                                ((DataRowView)CustomerPricingbindingSource.CurrencyManager.Current)["Column07"].ToString());
                            gridEX_List.SetValue("column09",
                                ((DataRowView)CustomerPricingbindingSource.CurrencyManager.Current)["Column05"].ToString());
                            gridEX_List.SetValue("column08",
                                ((DataRowView)CustomerPricingbindingSource.CurrencyManager.Current)["Column06"].ToString());
                        }
                        else
                        {
                            gridEX_List.SetValue("column10",
                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                "SalePrice"].ToString());
                            gridEX_List.SetValue("column09",
                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                "SalePackPrice"].ToString());
                            gridEX_List.SetValue("column08",
                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                "SaleBoxPrice"].ToString());
                        }
                    }
                }
                else if (e.Column.Key == "column14")
                {
                    object Value = gridEX_List.GetValue("Column14");
                    DataRowView Row = (DataRowView)gridEX_List.RootTable.Columns["Column14"].DropDown.FindItem(Value);
                    gridEX_List.SetValue("Column15", Row["Column02"]);
                }

                if (gridEX_List.GetRow().Cells["Column14"].Text.Trim() == "" && gridEX_List.GetRow().Cells["Column15"].Text.Trim() == "")
                {
                    if (gridEX1.GetRow().Cells["Column40"].Text.Trim() != "" &&
                          gridEX1.GetRow().Cells["Column41"].Text.Trim() != "")
                    {
                        gridEX_List.SetValue("Column14", gridEX1.GetValue("Column40").ToString());
                        gridEX_List.SetValue("Column15", gridEX1.GetValue("Column41").ToString());
                    }
                }
                gridEX_List.SetValue("column07",
                        (Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarkartoon"))) +
                        (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarbaste"))) +
                        Convert.ToDouble(gridEX_List.GetValue("column06")));


                    gridEX_List.SetValue("column11",
                        (gridEX1.GetValue("Column12").ToString() == "True" ?
                        (Convert.ToDouble(gridEX_List.GetValue("column04")) *
                        Convert.ToDouble(gridEX_List.GetValue("column08"))) +
                          (Convert.ToDouble(gridEX_List.GetValue("column05")) *
                        Convert.ToDouble(gridEX_List.GetValue("column09"))) +
                        (Convert.ToDouble(gridEX_List.GetValue("column06")) *
                        Convert.ToDouble(gridEX_List.GetValue("column10")))
                        : Convert.ToInt64((Convert.ToDouble(gridEX_List.GetValue("column04")) *
                        Convert.ToDouble(gridEX_List.GetValue("column08"))) +
                          (Convert.ToDouble(gridEX_List.GetValue("column05")) *
                        Convert.ToDouble(gridEX_List.GetValue("column09"))) +
                        (Convert.ToDouble(gridEX_List.GetValue("column06")) *
                        Convert.ToDouble(gridEX_List.GetValue("column10")))))
                        );


                Double jam, takhfif, ezafe;
                jam = Convert.ToDouble(gridEX_List.GetValue("column11"));
                takhfif = (jam * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                ezafe = ((jam - takhfif) *
                    (Convert.ToDouble(gridEX_List.GetValue("column18")) / 100));
                gridEX_List.SetValue("column17", (gridEX1.GetValue("Column12").ToString() == "True" ? takhfif : Convert.ToInt64(takhfif)));
                gridEX_List.SetValue("column19", (gridEX1.GetValue("Column12").ToString() == "True" ? ezafe : Convert.ToInt64(ezafe)));
                gridEX_List.SetValue("column20", (gridEX1.GetValue("Column12").ToString() == "True" ?
                    (jam - takhfif) + ezafe
                    : Convert.ToInt64((jam - takhfif) + ezafe)));

                gridEX_List.UpdateData();

                txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                    AggregateFunction.Sum).ToString());
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) -
                    Convert.ToDouble(txt_VolumeGroup.Value.ToString()) -
                    Convert.ToDouble(txt_SpecialGroup.Value.ToString()) -
                    Convert.ToDouble(txt_SpecialCustomer.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                    Convert.ToDouble(txt_Reductions.Value.ToString());


            }
            catch
            { }
        }

        private void bt_AddExtraDiscounts_Click(object sender, EventArgs e)
        {
            if (gridEX_Extra.AllowAddNew == InheritableBoolean.True && 
                this.table_055_LegalFactorsBindingSource.Count > 0 && 
                this.table_060_Child1_LegalFactorsBindingSource.Count > 0)
            {
                try
                {
                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount");
                    foreach (DataRow item in Table.Rows)
                    {
                        this.table_065_Child2_LegalFactorsBindingSource.AddNew();
                        DataRowView New = (DataRowView)this.table_065_Child2_LegalFactorsBindingSource.CurrencyManager.Current;
                        New["Column02"] = item["ColumnId"].ToString();
                        if (item["Column03"].ToString() == "True")
                        {
                            New["Column03"] = 0;
                            New["Column04"] = item["Column04"].ToString();
                        }
                        else
                        {
                            New["Column03"] = item["Column04"].ToString();
                            New["Column04"] = double.Parse(item["Column04"].ToString()) *
                                double.Parse(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString()) / 100;
                        }
                        New["Column05"] = item["Column02"].ToString();
                        this.table_065_Child2_LegalFactorsBindingSource.EndEdit();
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_DefineSignatures_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 126))
            {
                _05_Sale.Frm_019_Sale_Signatures frm = new Frm_019_Sale_Signatures();
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void txt_TotalPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_SpecialGroup.Focus();
        }

        private void txt_SpecialGroup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_VolumeGroup.Focus();
        }

        private void txt_VolumeGroup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_SpecialCustomer.Focus();
        }

        private void txt_SpecialCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_AfterDis.Focus();
        }

        private void txt_AfterDis_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_Extra.Focus();
        }

        private void txt_Extra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_Reductions.Focus();
        }

        private void txt_Reductions_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_EndPrice.Focus();
        }



    }
}
