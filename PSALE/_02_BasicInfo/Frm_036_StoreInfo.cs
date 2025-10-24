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
    public partial class Frm_036_StoreInfo : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_Discounts ClDiscount = new Classes.Class_Discounts();
        Class_UserScope UserScope = new Class_UserScope();
        bool Repeat = false;
        public Frm_036_StoreInfo()
        {
            InitializeComponent();
        }

        private void mlt_Ware_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);

            }
        }

        private void mlt_Ware_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");

        }

        private void mlt_Ware_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void mlt_PersonSale_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "name", "code");

        }

        private void mlt_SaleType_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "column02", "column01");
        }

        private void Frm_036_StoreInfo_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_296_StoreUsers' table. You can move, or remove it, as needed.
            DataTable CustomerTable = clDoc.ReturnTable
             (ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
                                           dbo.Table_045_PersonInfo.Column01 AS code,
                                           dbo.Table_045_PersonInfo.Column02 AS NAME,
                                           dbo.Table_065_CityInfo.Column02 AS shahr,
                                           dbo.Table_060_ProvinceInfo.Column01 AS ostan,
                                           dbo.Table_045_PersonInfo.Column06 AS ADDRESS,
                                           dbo.Table_045_PersonInfo.Column30,
                                           Table_045_PersonInfo.Column07,
                                           Table_045_PersonInfo.Column19 AS Mobile
                                    FROM   dbo.Table_045_PersonInfo
                                           LEFT JOIN dbo.Table_065_CityInfo
                                                ON  dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                                           LEFT JOIN dbo.Table_060_ProvinceInfo
                                                ON  dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00
                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");
            mlt_Customer.DataSource = CustomerTable;
            gridEX1.DropDowns["buyer"].DataSource = CustomerTable;

            mlt_PersonSale.DataSource = (clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId AS id,Column01 AS code,Column02 AS NAME from PeopleScope(8,3)"));
            gridEX1.DropDowns["saleman"].DataSource = (clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId AS id,Column01 AS code,Column02 AS NAME from PeopleScope(8,3)"));


            mlt_SaleType.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes");

            gridEX1.DropDowns["saletype"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes");

            mlt_Ware.DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from Table_001_PWHRS where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + "')");

            gridEX1.DropDowns["ware"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from Table_001_PWHRS where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + "')");

            mlt_project.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "SELECT * from Table_035_ProjectInfo");

            gridEX1.DropDowns["project"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "SELECT * from Table_035_ProjectInfo");

            mlt_ExpenseCenter.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "SELECT * from Table_030_ExpenseCenterInfo");
            gridEX1.DropDowns["center"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "SELECT * from Table_030_ExpenseCenterInfo");

            table_295_StoreInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_295_StoreInfo);
            this.table_296_StoreUsersTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_296_StoreUsers);

            gridEX2.DropDowns[0].DataSource = clDoc.ReturnTable(ConMain.ConnectionString, "SELECT distinct Column00   from Table_010_UserInfo where Column06='" + Class_BasicOperation._Year + "' and Column05=" + Class_BasicOperation._OrgCode);
          //  DataTable dt = clDoc.ReturnTable(ConBase.ConnectionString, @"select COlumn29,Column30 from Table_295_StoreInfo where Columnid=" + txt_ID.Text + "");
          //rdb_TypeCash.Checked= Convert.ToBoolean( dt.Rows[0]["Column29"]);
          //  rbd_ReciptCash.Checked = Convert.ToBoolean(dt.Rows[0]["Column30"]);

        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                table_295_StoreInfoBindingSource.EndEdit();
                table_295_StoreInfoBindingSource.AddNew();
                ((DataRowView)this.table_295_StoreInfoBindingSource.CurrencyManager.Current)["Column10"] = Class_BasicOperation._UserName;
                ((DataRowView)this.table_295_StoreInfoBindingSource.CurrencyManager.Current)["Column11"] = Class_BasicOperation.ServerDate();
                txt_code.Value = clDoc.MaxNumber(ConBase.ConnectionString, "Table_295_StoreInfo", "Column00");
                txt_code.Select();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }
     
        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
           
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
               

                if (gridEX2.GetDataRows().Count() == 0)
                    throw new Exception("کاربر/ان فروشگاه تعیین نشده است");
                ((DataRowView)this.table_295_StoreInfoBindingSource.CurrencyManager.Current)["Column12"] = Class_BasicOperation._UserName;
                ((DataRowView)this.table_295_StoreInfoBindingSource.CurrencyManager.Current)["Column13"] = Class_BasicOperation.ServerDate();

                if (((DataRowView)this.table_295_StoreInfoBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                    txt_code.Value = clDoc.MaxNumber(ConBase.ConnectionString, "Table_295_StoreInfo", "Column00");
               
                dataSet_EtelaatPaye.EnforceConstraints = false;
                table_295_StoreInfoBindingSource.EndEdit();
                table_295_StoreInfoTableAdapter.Update(this.dataSet_EtelaatPaye.Table_295_StoreInfo);
                table_296_StoreUsersBindingSource.EndEdit();
                this.table_296_StoreUsersTableAdapter.Update(this.dataSet_EtelaatPaye.Table_296_StoreUsers);
                dataSet_EtelaatPaye.EnforceConstraints = true;

                gridEX2.UpdateData();
                gridEX2.RemoveFilters();

                if (sender == bt_Save || sender == this)
                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void mlt_ExpenseCenter_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");

        }

        private void Frm_036_StoreInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
                gridEX1.Row = gridEX1.FilterRow.Position;

        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 348))
            {
                if (this.table_295_StoreInfoBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف رکورد مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {

                            //                            DataTable dtCheck = clDoc.ReturnTable(Properties.Settings.Default.SALE, @"SELECT tbl.*,
                            //                                                                                       " + ConMain.Database + @".dbo.Table_000_OrgInfo.Column01 AS CompName,
                            //                                                                                       " + ConMain.Database + @".dbo.Table_000_OrgInfo.Column00 AS Codecomp
                            //                                                                                FROM   (
                            //                                                                                           SELECT NAME,
                            //                                                                                                  SUBSTRING(NAME, 7, (CHARINDEX('_', NAME, 7) -7)) AS code
                            //                                                                                           FROM   sys.databases
                            //                                                                                           WHERE  NAME LIKE 'PSALE_%'
                            //                                                                                       ) AS tbl
                            //                                                                                       LEFT OUTER JOIN " + ConMain.Database + @".dbo.Table_000_OrgInfo
                            //                                                                                            ON  " + ConMain.Database + @".dbo.Table_000_OrgInfo.ColumnId = code
                            //                                                                                WHERE  code = " + Class_BasicOperation._OrgCode + "");
                            //                            if (dtCheck.Rows.Count > 0)
                            //                            {
                            //                                foreach (DataRow dr in dtCheck.Rows)
                            //                                {

                            //                                    SqlConnection con = new SqlConnection(Properties.Settings.Default.SALE);
                            //                                    using (SqlConnection ConSALE = new SqlConnection(Properties.Settings.Default.SALE.Replace(con.Database, dr[0].ToString())))
                            //                                    {
                            //                                        ConSALE.Open();
                            //                                        SqlCommand Select = new SqlCommand(@"
                            //                                                                                IF EXISTS(
                            //                                                                                       SELECT *
                            //                                                                                       FROM   Table_010_SaleFactor tsf
                            //                                                                                       WHERE  Column67 = " + ((DataRowView)this.table_295_StoreInfoBindingSource.CurrencyManager.Current)["ColumnId"] + @"
                            //                                                                                   )
                            //                                                                                   OR EXISTS (
                            //                                                                                          SELECT *
                            //                                                                                          FROM   Table_015_BuyFactor tsf
                            //                                                                                          WHERE  Column35 = " + ((DataRowView)this.table_295_StoreInfoBindingSource.CurrencyManager.Current)["ColumnId"] + @"
                            //                                                                                      )
                            //                                                                                   OR EXISTS (
                            //                                                                                          SELECT *
                            //                                                                                          FROM   Table_018_MarjooiSale tsf
                            //                                                                                          WHERE  Column30 = " + ((DataRowView)this.table_295_StoreInfoBindingSource.CurrencyManager.Current)["ColumnId"] + @"
                            //                                                                                      )
                            //                                                                                   OR EXISTS (
                            //                                                                                          SELECT *
                            //                                                                                          FROM   Table_021_MarjooiBuy tsf
                            //                                                                                          WHERE  Column28 = " + ((DataRowView)this.table_295_StoreInfoBindingSource.CurrencyManager.Current)["ColumnId"] + @"
                            //                                                                                      )
                            //                                                                                BEGIN
                            //                                                                                    SELECT 0 ok
                            //                                                                                END
                            //                                                                                ELSE
                            //                                                                                    SELECT 1 ok", ConSALE);
                            //                                        if (Select.ExecuteScalar().ToString() == "0")
                            //                                            throw new Exception("به علت استفاده از این فروشگاه در فاکتور/های ثبت شده در سال مالی " + dr[0].ToString().Substring(dr[0].ToString().Length - 4, 4) + "، حذف آن امکانپذیر نمی باشد");
                            //                                    }

                            //                                }
                            //                            }



                            this.table_295_StoreInfoBindingSource.RemoveCurrent();
                            this.table_295_StoreInfoBindingSource.EndEdit();
                            table_295_StoreInfoTableAdapter.Update(this.dataSet_EtelaatPaye.Table_295_StoreInfo);
                            Class_BasicOperation.ShowMsg("", "رکورد مورد نظر حذف شد", "Information");
                        }
                        catch (Exception ex)
                        {

                            Class_BasicOperation.CheckExceptionType(ex, this.Name);

                        }
                    }
                }
                dataSet_EtelaatPaye.EnforceConstraints = false;
                table_295_StoreInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_295_StoreInfo);
                this.table_296_StoreUsersTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_296_StoreUsers);
                dataSet_EtelaatPaye.EnforceConstraints = true;


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");

        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
                bt_Search_Click(sender, e);
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
            {
                try
                {

                    int Id = ReturnIDNumber(int.Parse(txt_Search.Text));

                    Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition
                        (gridEX1.RootTable.Columns["ColumnId"], Janus.Windows.GridEX.ConditionOperator.Equal, Id);
                    gridEX1.Find(filter, -1, 1);
                    txt_Search.SelectAll();



                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    txt_Search.SelectAll();
                }
            }
        }


        private int ReturnIDNumber(int ResidNum)
        {
            using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.BASE))
            {
                ConWare.Open();
                int ID = 0;
                SqlCommand Commnad = new SqlCommand(
                    "Select ISNULL(ColumnId,0) from Table_295_StoreInfo where Column00=" + ResidNum, ConWare);
                try
                {
                    ID = int.Parse(Commnad.ExecuteScalar().ToString());
                    return ID;
                }
                catch
                {
                    txt_Search.Text = string.Empty;
                    throw new Exception("شماره وارد شده نامعتبر است");
                }
            }
        }

        private void Frm_036_StoreInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
            gridEX2.RemoveFilters();

        }

        private void gridEX2_Enter(object sender, EventArgs e)
        {
            try
            {
                table_295_StoreInfoBindingSource.EndEdit();
                Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition
                       (gridEX1.RootTable.Columns["Column00"], Janus.Windows.GridEX.ConditionOperator.Equal, txt_code.Value);
                gridEX1.Find(filter, -1, 1);

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX2.CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "Column01")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column01", "Column00", "Column00", gridEX2.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

            }
            catch { }

            gridEX2.SetValue("Column04", Class_BasicOperation._UserName);
            gridEX2.SetValue("Column05", Class_BasicOperation.ServerDate());
        }

        private void gridEX2_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
               

                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column01");


            }
            catch { }
        }

        private void gridEX2_AddingRecord(object sender, CancelEventArgs e)
        {
            
     
            gridEX2.SetValue("Column02", Class_BasicOperation._UserName);
            gridEX2.SetValue("Column03", Class_BasicOperation.ServerDate());

        }

        private void txt_desc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                {
                    gridEX2.Select();
                    gridEX2.Col = 2;
                }
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    gridEX2.Select();
                    gridEX2.Col = 2;
                }

            }
        }

       

        
        private void check_Fi_CheckedChanged_1(object sender, EventArgs e)
        {
           
            if (check_Fi.Checked ==false)
            {
                check_Fi.Checked = false;
            }
            else if (check_Fi.Checked == true)
            {
                check_Fi.Checked = true;
            }

            
        }

        private void check_Number_CheckedChanged(object sender, EventArgs e)
        {
            if (check_Number.Checked==false)
            {
                check_Number.Checked = false;
            }
            else if (check_Number.Checked==true)
            {
                check_Number.Checked = true;
                
            }
        }

        private void cheek_Print8_CheckedChanged(object sender, EventArgs e)
        {
            if (cheek_Print8.Checked == false)
            {
                cheek_Print8.Checked = false;
            }
            else if (cheek_Print8.Checked == true)
            {
                cheek_Print8.Checked = true;

            }
        }

        private void Chek_Tasvie_CheckedChanged(object sender, EventArgs e)
        {
            if (Chek_Tasvie.Checked == false)
            {
                Chek_Tasvie.Checked = false;
            }
            else if (Chek_Tasvie.Checked == true)
            {
                Chek_Tasvie.Checked = true;

            }
        }

        private void rdb_TypeCash_CheckedChanged(object sender, EventArgs e)
        {
            

        }

        private void rbd_ReciptCash_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void table_295_StoreInfoBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                rbd_ReciptCash.Checked = !(rdb_TypeCash.Checked);

            }
            catch 
            {

              
            }
            
        }

        private void cheek_PriceUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (cheek_PriceUpdate.Checked == false)
            {
                cheek_PriceUpdate.Checked = false;
            }
            else if (cheek_PriceUpdate.Checked == true)
            {
                cheek_PriceUpdate.Checked = true;

            }
           
        }

        private void chek_ControlRemain_CheckedChanged(object sender, EventArgs e)
        {
            if (chek_ControlRemain.Checked ==true)
            {
                chek_ControlRemain.Checked = true;
            }
             else if (chek_ControlRemain.Checked ==false)
            {
                chek_ControlRemain.Checked = false;

            }
        }
    }
}
