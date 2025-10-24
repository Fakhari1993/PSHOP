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

namespace PSHOP._04_Buy
{
    public partial class Frm_014_ReturnFactor : Form
    {
        bool _del;
        int _ID = 0;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);

        Class_UserScope UserScope = new Class_UserScope();
        DataSet DS = new DataSet();
        InputLanguage original;
        DataTable discountdt = new DataTable();
        DataTable taxdt = new DataTable();
        DataTable factordt = new DataTable();
        //DataTable waredt = new DataTable();
        DataTable Sanaddt = new DataTable();
        int LastDocnum = 0;
        bool ExtraMethod = false;
        bool Isadmin = false;
        Int16 projectId;
        DataTable storefactor = new DataTable();
        public Frm_014_ReturnFactor(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        public Frm_014_ReturnFactor(bool del, int ID)
        {
            _del = del;
            _ID = ID;
            InitializeComponent();
        }

        private void Frm_013_ReturnFactor_Load(object sender, EventArgs e)
        {

            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.MAIN))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("Select Column02 from Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" +
                Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + "'", Con);
                Isadmin = (bool.Parse(Select.ExecuteScalar().ToString()));

            }

            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT tsi.Column05
                                                                        FROM   dbo.Table_295_StoreInfo AS tsi
                                                                               JOIN dbo.Table_296_StoreUsers AS tsu
                                                                                    ON  tsu.Column00 = tsi.ColumnId
                                                                        WHERE tsu.Column01='" + Class_BasicOperation._UserName + "'", ConBase);
            DataTable StoreTable = new DataTable();

            Adapter.Fill(StoreTable);

            if (!Isadmin && StoreTable.Rows.Count == 0) { Class_BasicOperation.ShowMsg("", "کاربر گرامی، فروشگاه شما تعیین نشده است و امکان کار با این فرم را ندارید ", "Stop"); this.Dispose(); }

            else if (StoreTable.Rows.Count > 0) projectId = Convert.ToInt16(StoreTable.Rows[0]["Column05"]);


            storefactor = clDoc.GetDefaultValues();

            Adapter = new SqlDataAdapter("Select * from Table_030_Setting", ConBase);
            DataTable setting = new DataTable();
            Adapter.Fill(setting);
            if (setting.Rows.Count <= 65)
            {
                Class_BasicOperation.ShowMsg("", "اطلاعات جدول تنظیمات کامل نیست، با پشتیبانی تماس بگیرید", "Stop");
                this.Close();

            }
            ExtraMethod = Convert.ToBoolean(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, "SELECT Column02 FROM [Table_030_Setting] where ColumnId=71"));


            this.WindowState = FormWindowState.Maximized;
            foreach (GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
                if (col.Key == "Column05" || col.Key == "Column07")
                    col.DefaultValue = Class_BasicOperation._UserName;
                if (col.Key == "Column06" || col.Key == "Column08")
                    col.DefaultValue = Class_BasicOperation.ServerDate();
            }

            gridEX1.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString,
                "Select ColumnId,Column01 from Table_007_PwhrsDraft"), "");
            gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString,
                "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            gridEX1.DropDowns["Buy"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString,
                "Select ColumnId,Column01 from Table_015_BuyFactor"), "");



            gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from ListPeople(5)"), "");
            gridEX1.DropDowns["Project"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_035_ProjectInfo"), "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 FROM Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(DS, "Center");
            gridEX_List.DropDowns["Center"].SetDataBinding(DS.Tables["Center"], "");

            Adapter = new SqlDataAdapter("SELECT Column00,Column01,Column02 FROM Table_035_ProjectInfo", ConBase);
            Adapter.Fill(DS, "Project");
            gridEX_List.DropDowns["Project"].SetDataBinding(DS.Tables["Project"], "");
            gridEX1.DropDowns["Buyer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,4)"), "");



            gridEX1.DropDowns["Ware"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from Table_001_PWHRS"), "");
            gridEX1.DropDowns["Operation"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1"), "");


            Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount_Buy", ConSale);
            Adapter.Fill(DS, "Discount");
            gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");

            Adapter = new SqlDataAdapter("select ColumnId,Column01 from Table_013_RequestBuy", ConSale);
            Adapter.Fill(DS, "PreFactor");
            gridEX_List.DropDowns["Factor"].SetDataBinding(DS.Tables["PreFactor"], "");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_055_CurrencyInfo");
            gridEX1.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");


            if (_ID != 0)
            {
                this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_021_MarjooiBuy, _ID);
                this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_022_Child1_MarjooiBuy, _ID);
                this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_023_Child2_MarjooiBuy, _ID);
                table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);
            }

            if (storefactor.Rows.Count == 0)
                throw new Exception("کاربر نامعتبر است");
            if (!Convert.ToBoolean(storefactor.Rows[0]["admin"]) &&
                (storefactor.Rows[0]["project"] == DBNull.Value ||
                storefactor.Rows[0]["project"] == null ||
                string.IsNullOrWhiteSpace(storefactor.Rows[0]["project"].ToString())))
                throw new Exception("فروشگاه کاربر تعیین نشده است");

            if (!Convert.ToBoolean(storefactor.Rows[0]["admin"]))
            {
                //  mlt_Ware.ReadOnly = true;
                // mlt_project.ReadOnly = true;
                //mlt_PersonSale.ReadOnly = true;

                gridEX1.RootTable.Columns["Column28"].Selectable = false;
                gridEX1.RootTable.Columns["Column26"].Selectable = false;


            }
            else
            {
                //mlt_Ware.ReadOnly = false;
                //mlt_project.ReadOnly = false;
                //mlt_PersonSale.ReadOnly = false;


                gridEX1.RootTable.Columns["Column28"].Selectable = true;
                gridEX1.RootTable.Columns["Column26"].Selectable = true;


            }
            //            waredt = new DataTable();
            //            Adapter = new SqlDataAdapter(
            //                                                           @"SELECT        isnull(Column02,0) as Column02
            //                                                                        FROM           Table_030_Setting
            //                                                                        WHERE        (ColumnId in (45,46)) order by ColumnId   ", ConBase);
            //            Adapter.Fill(waredt);
                string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + StoreTable.Rows[0]["Column05"] + "),0)");
            if (controlremain=="True")
            {
                GoodbindingSource.DataSource = clGood.MahsoolInfo(((storefactor.Rows[0]["ware"] != DBNull.Value
                && storefactor.Rows[0]["ware"] != null
                && !string.IsNullOrWhiteSpace(storefactor.Rows[0]["ware"].ToString())) ? Convert.ToInt16(storefactor.Rows[0]["ware"]) : Convert.ToInt16(0)));
                DataTable GoodTable = clGood.MahsoolInfo(((storefactor.Rows[0]["ware"] != DBNull.Value
                    && storefactor.Rows[0]["ware"] != null
                    && !string.IsNullOrWhiteSpace(storefactor.Rows[0]["ware"].ToString())) ? Convert.ToInt16(storefactor.Rows[0]["ware"]) : Convert.ToInt16(0)));
                gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
            }
            else
            {
                GoodbindingSource.DataSource = clGood.GoodInfo();
                DataTable GoodTable = clGood.GoodInfo();
                gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
            }


        }



        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.Enabled = true;
                dataSet_Buy.EnforceConstraints = false;
                this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_021_MarjooiBuy, 0);
                this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_022_Child1_MarjooiBuy, 0);
                this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_023_Child2_MarjooiBuy, 0);
                dataSet_Buy.EnforceConstraints = true;
                gridEX1.MoveToNewRecord();
                gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd"));
                gridEX1.SetValue("Column05", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column06", Class_BasicOperation.ServerDate());
                gridEX1.SetValue("Column07", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column08", Class_BasicOperation.ServerDate());
                if (
                   (storefactor.Rows[0]["project"] != DBNull.Value &&
                   storefactor.Rows[0]["project"] != null &&
                   !string.IsNullOrWhiteSpace(storefactor.Rows[0]["project"].ToString())))
                {
                    gridEX1.SetValue("Column26", Convert.ToInt16(storefactor.Rows[0]["ware"]));
                    gridEX1.SetValue("Column28", Convert.ToInt16(storefactor.Rows[0]["project"]));


                }


                gridEX1.RootTable.Columns["Column23"].Selectable = false;
                gridEX1.RootTable.Columns["Column24"].Selectable = false;
                gridEX1.Select();
                gridEX1.Col = 3;
                bt_New.Enabled = false;

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void SaveEvent(object sender, EventArgs e)
        {
            gridEX1.UpdateData();
            if (this.table_021_MarjooiBuyBindingSource.Count > 0 &&
                       gridEX_List.AllowEdit == InheritableBoolean.True &&
                       gridEX1.GetRow().Cells["Column03"].Text.Trim() != "")
            {

                gridEX_List.UpdateData();
                gridEX_Extra.UpdateData();

                int OldDraftNum = 0;

                DataRowView Row = (DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current;
                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }
                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(Row["column02"].ToString().Substring(0, 4)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(5, 2)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(8, 2))))
                {

                    Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
                    this.Cursor = Cursors.Default;

                    return;

                }
                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }
                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column07"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }

                if (Row["column26"].ToString() == "" || Row["column27"].ToString() == "")
                {
                    MessageBox.Show("لطفا اطلاعات انبار را تکمیل کنید");
                    this.Cursor = Cursors.Default;

                    return;
                }

                if (!clDoc.AllService(table_022_Child1_MarjooiBuyBindingSource))
                {
                    //چک باقی مانده کالا
                    foreach (DataRowView item in table_022_Child1_MarjooiBuyBindingSource)
                    {
                        if (clDoc.IsGood(item["Column02"].ToString()))
                        {
                            bool mojoodimanfi = false;
                            try
                            {
                                using (SqlConnection ConWareGood = new SqlConnection(Properties.Settings.Default.WHRS))
                                {

                                    ConWareGood.Open();
                                    SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                               (
                                                                                   SELECT ISNULL(Column16, 0) AS Column16
                                                                                   FROM   table_004_CommodityAndIngredients
                                                                                   WHERE  ColumnId = " + item["Column02"] + @"
                                                                               ),
                                                                               0
                                                                           ) AS Column16", ConWareGood);
                                    mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                                }
                            }
                            catch
                            {
                            }
                            float Remain = FirstRemain(int.Parse(item["Column02"].ToString()), Row["column02"].ToString(), Convert.ToInt32(gridEX1.GetValue("Column26")));
                            if (Remain < float.Parse(item["Column07"].ToString()) && !mojoodimanfi)
                            {
                                throw new Exception("موجودی کالای " + clDoc.ExScalar(ConWare.ConnectionString,
                                    "table_004_CommodityAndIngredients", "Column02", "ColumnId",
                                    item["Column02"].ToString()) + " کمتر از تعداد مشخص شده در فاکتور است");
                            }
                        }
                    }
                }
                chehckessentioal(Row["column02"].ToString());
                using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Conacnt.Open();
                    SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + Row["Column26"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                    if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0)
                        throw new Exception("برای صدور حواله به انبار انتخاب شده دسترسی ندارید");

                }

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(Row["Column26"].ToString()),
                        int.Parse(item.Cells["column02"].Value.ToString())))
                        throw new Exception("کالای " + item.Cells["column02"].Text +
                            " در انبار انتخاب شده فعال نمی باشد");
                }
                if (Row["Column01"].ToString().StartsWith("-"))
                {
                    gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column01").ToString());
                    this.table_021_MarjooiBuyBindingSource.EndEdit();
                }

                txt_TotalPrice.Value = Convert.ToDouble(
                  gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                  AggregateFunction.Sum).ToString());


                double Total = double.Parse(txt_TotalPrice.Value.ToString());
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                {
                    if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                    {
                        item.BeginEdit();
                        item.Cells["Column04"].Value = (gridEX1.GetValue("Column15").ToString() == "True" ?
                            Total * double.Parse(item.Cells["Column03"].Value.ToString()) / 100
                            : Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100));

                        item.EndEdit();

                    }
                }
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) -
                       Convert.ToDouble(txt_Reductions.Value.ToString());

                if (Convert.ToDouble(txt_EndPrice.Value) < Convert.ToDouble(0))
                {
                    Class_BasicOperation.ShowMsg("", "جمع کل فاکتور منفی شده است", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }
                string RowID = ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                int DocId = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column11", RowID);
                int DraftId = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column10", RowID);
                this.Cursor = Cursors.WaitCursor;


                string command = string.Empty;
                Boolean ok = true;
                if (DocId > 0)
                {
                    clDoc.IsFinal_ID(DocId);
                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=20 and Column17=" + RowID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=20 and Column17=" + RowID;



                    command += "Update     " + ConSale.Database + ".dbo.Table_021_MarjooiBuy set  Column11=0  where   columnid=" + RowID;


                }
                if (DraftId > 0)
                {
                    OldDraftNum = Convert.ToInt32(clDoc.ExScalarQuery(Properties.Settings.Default.WHRS, @"SELECT ISNULL(
                                                                                                                   (
                                                                                                                       SELECT column01
                                                                                                                       FROM   Table_007_PwhrsDraft
                                                                                                                       WHERE  columnid = " + DraftId + @"
                                                                                                                   ),
                                                                                                                   0
                                                                                                               ) AS column01"));


                    command += "Delete  from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where column01=" + DraftId;
                    command += "Delete  from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft where   columnid=" + DraftId;
                    command += "Update     " + ConSale.Database + ".dbo.Table_021_MarjooiBuy set  Column10=0   where   columnid=" + RowID;



                }





                if (!string.IsNullOrWhiteSpace(command))
                {
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                    {
                        Con.Open();

                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;
                        try
                        {
                            Command.CommandText = command;
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();


                        }
                        catch (Exception es)
                        {
                            ok = false;
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;

                            Class_BasicOperation.CheckExceptionType(es, this.Name);

                        }
                    }
                }

                if (ok)
                {
                   


                  

                    Row["Column07"] = Class_BasicOperation._UserName;
                    Row["Column08"] = Class_BasicOperation.ServerDate();
                    Row["Column18"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString();
                    Row["Column21"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                    Row["Column22"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                    //Extra-Reductions
                    Janus.Windows.GridEX.GridEXFilterCondition Filter2 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                    Row["Column19"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                    Filter2.Value1 = true;
                    Row["Column20"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();

                    this.table_021_MarjooiBuyBindingSource.EndEdit();
                    this.table_022_Child1_MarjooiBuyBindingSource.EndEdit();
                    this.table_023_Child2_MarjooiBuyBindingSource.EndEdit();
                    this.table_021_MarjooiBuyTableAdapter.Update(dataSet_Buy.Table_021_MarjooiBuy);
                    this.table_022_Child1_MarjooiBuyTableAdapter.Update(dataSet_Buy.Table_022_Child1_MarjooiBuy);
                    this.table_023_Child2_MarjooiBuyTableAdapter.Update(dataSet_Buy.Table_023_Child2_MarjooiBuy);
                    Row = (DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current;
                    checksanad();
                    string sanadcmd = string.Empty;
                    SqlParameter DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
                    DraftNum.Direction = ParameterDirection.Output;

                    SqlParameter DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                    DocNum.Direction = ParameterDirection.Output;

                    SqlParameter draftkey = new SqlParameter("draftkey", SqlDbType.Int);
                    draftkey.Direction = ParameterDirection.Output;
                    sanadcmd = "     declare @DocID int set @DraftNum=" + (OldDraftNum > 0 ? OldDraftNum.ToString() : "( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft)") + @"";
                    sanadcmd += @" INSERT INTO " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ([column01]
                                                                                               ,[column02]
                                                                                               ,[column03]
                                                                                               ,[column04]
                                                                                               ,[column05]
                                                                                               ,[column06]
                                                                                               ,[column07]
                                                                                               ,[column08]
                                                                                               ,[column09]
                                                                                               ,[column10]
                                                                                               ,[column11]
                                                                                               ,[column12]
                                                                                               ,[column13]
                                                                                               ,[column14]
                                                                                               ,[column15]
                                                                                               ,[column16]
                                                                                               ,[column17]
                                                                                               ,[column18]
                                                                                               ,[column19]
                                                                                               ,[Column20]
                                                                                               ,[Column21]
                                                                                               ,[Column22]
                                                                                               ,[Column23]
                                                                                               ,[Column24]
                                                                                               ,[Column25]
                                                                                               ,[Column26]) VALUES(@DraftNum,'" + Row["column02"].ToString() + "'," + Row["column26"].ToString()
                                 + "," + Row["column27"].ToString() + @", " + Row["column03"].ToString() + ",'" + "حواله صادره بابت مرجوعی خرید ش" + Row["column01"].ToString() +
                                 "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0,0," + ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + ",0,0,0,0,null,0,1); SET @draftkey=SCOPE_IDENTITY()";

                    foreach (DataRowView item in table_022_Child1_MarjooiBuyBindingSource)
                    {
                        if (clDoc.IsGood(item["Column02"].ToString()))
                        {
                            sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]) VALUES(@draftkey," + item["Column02"].ToString() + "," +
                                      item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                                      item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," +
                                      item["Column11"].ToString() + ",NULL," +
                                      (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString())
                                      + "," + (double.Parse(item["Column07"].ToString()) > 0 ? double.Parse(item["Column20"].ToString()) / double.Parse(item["Column07"].ToString()) : 0) + "," +
                                      item["Column20"].ToString() + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                                      + "',getdate(),NULL,NULL," +
                                      (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," +
                                      item["Column14"].ToString() + ",0,0,0,0,0," + (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column30"].ToString().Trim() + "'")
                                           + "," + (item["Column31"].ToString().Trim() == "" ? "NULL" : "'" + item["Column31"].ToString().Trim() + "'") + "," +
                                      item["Column28"].ToString() + "," + item["Column29"].ToString() + "," + item["Column32"].ToString() + "," + item["Column33"].ToString() + ")";
                        }
                    }

                    sanadcmd += "Update " + ConSale.Database + ".dbo.Table_021_MarjooiBuy set Column10=@draftkey,column07='" + Class_BasicOperation._UserName + "',column08=getdate() where ColumnId=" + ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    if (LastDocnum > 0)
                        sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                    else
                        sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + Row["column02"] + "',2,0,N'فاکتور مرجوعی خرید','" + Class_BasicOperation._UserName +
                   "',getdate()); SET @DocID=SCOPE_IDENTITY()";

                    string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());
                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + Row["column03"] + @", NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @" ,
                   " + "'فاکتور مرجوعی خرید " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                         Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";


                    _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @"  ,
                   " + "'فاکتور مرجوعی خرید " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";
                    foreach (DataRow dr in Sanaddt.Rows)
                    {
                        if (dr["Kosoorat"] != null &&
                            dr["Kosoorat"].ToString() != string.Empty &&
                            Convert.ToDouble(dr["Kosoorat"]) > Convert.ToDouble(0))
                        {


                            _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @"  ,
                   " + "'تخفیف فاکتور مرجوعی خرید ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               " + Row["column03"] + @", NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @"  ,
                   " + "'تخفیف فاکتور مرجوعی خرید ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                        }

                        if (dr["Ezafat"] != null &&
                          dr["Ezafat"].ToString() != string.Empty &&
                          Convert.ToDouble(dr["Ezafat"]) > Convert.ToDouble(0))
                        {

                            _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + Row["column03"] + @", NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @"  ,
                   " + "'ارزش افزوده فاکتور مرجوعی خرید ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @"  ,
                   " + "'ارزش افزوده فاکتور مرجوعی خرید ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                        }


                    }
                    sanadcmd += " Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07=@DocID,column10='" + Class_BasicOperation._UserName + "',column11=getdate() where ColumnId=@draftkey";
                    sanadcmd += " Update " + ConSale.Database + ".dbo.Table_021_MarjooiBuy set Column11=@DocID,column07='" + Class_BasicOperation._UserName + "',column08=getdate() where ColumnId =" + ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                    {
                        Con.Open();

                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;

                        try
                        {
                            Command.CommandText = sanadcmd;
                            Command.Parameters.Add(DocNum);
                            Command.Parameters.Add(DraftNum);
                            Command.Parameters.Add(draftkey);
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();


                            bool dok = true;

                            try
                            {

                                using (SqlConnection Con1 = new SqlConnection(Properties.Settings.Default.WHRS))
                                {
                                    Con1.Open();
                                    SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=(select columnid from Table_007_PwhrsDraft where column01=" + DraftNum.Value + ")", Con1);
                                    DataTable Table = new DataTable();
                                    goodAdapter.Fill(Table);

                                    //محاسبه ارزش و ذخیره آن در جدول Child1



                                    foreach (DataRow item in Table.Rows)
                                    {
                                        // اگر فاکتور مرجوعی فاکتور خرید داشت ارزش رسید ان فاکتور خرید خوانده میشود
                                        SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT ISNULL(Table_012_Child_PwhrsReceipt.column20,0) AS column20
                                                                                    FROM   Table_012_Child_PwhrsReceipt
                                                                                           JOIN Table_011_PwhrsReceipt tpr
                                                                                                ON  tpr.columnid = Table_012_Child_PwhrsReceipt.column01
                                                                                           JOIN " + ConSale.Database + @".dbo.Table_015_BuyFactor tbf
                                                                                                ON  tbf.column10 = tpr.columnid
                                                                                           JOIN " + ConSale.Database + @".dbo.Table_021_MarjooiBuy tmb
                                                                                                ON  tmb.column17 = tbf.columnid
                                                                                    WHERE  tmb.columnid = " + Row["ColumnId"] + @"
                                                                                           AND Table_012_Child_PwhrsReceipt.column02 =" + item["Column02"].ToString(), Con1);
                                        DataTable TurnTable = new DataTable();
                                        Adapter.Fill(TurnTable);
                                        if (TurnTable.Rows.Count > 0)
                                        {
                                            SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["column20"].ToString()), 4)
                                               + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["column20"].ToString()) * double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con1);
                                            UpdateCommand.ExecuteNonQuery();
                                        }
                                        // در غیر این صورت ارزش میانیگین محاسبه میشود
                                        else
                                        {
                                            if (Class_BasicOperation._WareType)
                                            {
                                                Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO] " : "  [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column26"], Con1);
                                                TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", " Date<='" + Row["Column02"].ToString() + "'").ToString());
                                                DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                                if (SelectedRow.Count() > 0)
                                                {
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(SelectedRow[0]["RemainFee"].ToString()), 4)
                                                        + " , Column16=" + Math.Round(double.Parse(SelectedRow[0]["RemainFee"].ToString()) * double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con1);
                                                    UpdateCommand.ExecuteNonQuery();
                                                }
                                            }
                                            else
                                            {
                                                Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column26"] + ",@Date='" + Row["Column02"].ToString() + "',@id=" + draftkey.Value + ",@residid=0", Con1);
                                                TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                                {
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                            + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con1);
                                                    UpdateCommand.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                dok = false;
                            }
                            if ((sender == bt_Save || sender == this) && dok)
                                Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                                  "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره حواله انبار: " + DraftNum.Value, "Information");
                            else
                                Class_BasicOperation.ShowMsg("", "ثبت فاکتور موفقیت انجام شد" + Environment.NewLine +
                                "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره حواله انبار: " + DraftNum.Value + Environment.NewLine + "محاسبه ارزش حواله با خطا مواجه شد", "Information");

                            if (DialogResult.Yes == MessageBox.Show("آیا مایل به چاپ فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            {
                                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 131))
                                {
                                    //bt_Save_Click(sender, e);
                                    _04_Buy.Reports.Form_ReturnBuyFactorPrint frm = new _04_Buy.Reports.Form_ReturnBuyFactorPrint
                                        (int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column01"].ToString()));
                                    frm.ShowDialog();
                                }
                                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                            }
                            int k = int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                            dataSet_Buy.EnforceConstraints = false;
                            table_021_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_021_MarjooiBuy, k);
                            table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_022_Child1_MarjooiBuy, k);
                            table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_023_Child2_MarjooiBuy, k);
                            dataSet_Buy.EnforceConstraints = true;
                            table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);

                            bt_New_Click(null, null);


                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }

                        this.Cursor = Cursors.Default;






                        ////آپدیت کردن ارزش کالا در حواله
                        ////خالص خطی تقسیم بر تعداد
                        //if (Convert.ToInt32(gridEX1.GetValue("column10")) > 0)
                        //{
                        //    double _arzeshvahed = 0;


                        //    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                        //    {
                        //        if (Convert.ToDouble(item.Cells["column07"].Value.ToString()) == 0)
                        //            _arzeshvahed = 0;
                        //        else
                        //            _arzeshvahed = Convert.ToDouble(item.Cells["column20"].Value.ToString()) /
                        //            Convert.ToDouble(item.Cells["column07"].Value.ToString());

                        //        clDoc.RunSqlCommand(ConWare.ConnectionString,
                        //               "update Table_008_Child_PwhrsDraft " +
                        //               "set column08=" + item.Cells["column08"].Value.ToString() +
                        //               ",column09=" + item.Cells["column09"].Value.ToString() +
                        //               ",column10=" + item.Cells["column10"].Value.ToString() + "," +
                        //               "column11=" + item.Cells["column11"].Value.ToString() +
                        //               ",column15=" + _arzeshvahed.ToString() + ",column16=" +
                        //               item.Cells["column20"].Value.ToString() +
                        //               " where column01=" + gridEX1.GetValue("column10").ToString() +
                        //               " and column02=" + item.Cells["column02"].Value.ToString());
                        //    }
                        // }

                        bt_New.Enabled = true;



                    }


                }

            }
            else if (gridEX_List.AllowEdit == InheritableBoolean.False)
            {
                Class_BasicOperation.ShowMsg("", "امکان ثبت تغییرات وجود ندارد", "Stop");

                return;
            }

        }
        private void SaveEvent1(object sender, EventArgs e)
        {
            gridEX1.UpdateData();
            if (this.table_021_MarjooiBuyBindingSource.Count > 0 &&
                       gridEX_List.AllowEdit == InheritableBoolean.True &&
                       gridEX1.GetRow().Cells["Column03"].Text.Trim() != "")
            {

                gridEX_List.UpdateData();
                gridEX_Extra.UpdateData();

                int OldDraftNum = 0;

                DataRowView Row = (DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current;
                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }
                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(Row["column02"].ToString().Substring(0, 4)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(5, 2)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(8, 2))))
                {

                    Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
                    this.Cursor = Cursors.Default;

                    return;

                }
                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }
                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column07"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }


                if (Row["column26"].ToString() == "" || Row["column27"].ToString() == "")
                {
                    MessageBox.Show("لطفا اطلاعات انبار را تکمیل کنید");
                    this.Cursor = Cursors.Default;

                    return;
                }

                if (!clDoc.AllService(table_022_Child1_MarjooiBuyBindingSource))
                {
                    //چک باقی مانده کالا
                    foreach (DataRowView item in table_022_Child1_MarjooiBuyBindingSource)
                    {
                        if (clDoc.IsGood(item["Column02"].ToString()))
                        {
                            bool mojoodimanfi = false;
                            try
                            {
                                using (SqlConnection ConWareGood = new SqlConnection(Properties.Settings.Default.WHRS))
                                {

                                    ConWareGood.Open();
                                    SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                               (
                                                                                   SELECT ISNULL(Column16, 0) AS Column16
                                                                                   FROM   table_004_CommodityAndIngredients
                                                                                   WHERE  ColumnId = " + item["Column02"] + @"
                                                                               ),
                                                                               0
                                                                           ) AS Column16", ConWareGood);
                                    mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                                }
                            }
                            catch
                            {
                            }
                            float Remain = FirstRemain(int.Parse(item["Column02"].ToString()), Row["column02"].ToString(), Convert.ToInt32(gridEX1.GetValue("Column26")));
                            if (Remain < float.Parse(item["Column07"].ToString()) && !mojoodimanfi)
                            {
                                throw new Exception("موجودی کالای " + clDoc.ExScalar(ConWare.ConnectionString,
                                    "table_004_CommodityAndIngredients", "Column02", "ColumnId",
                                    item["Column02"].ToString()) + " کمتر از تعداد مشخص شده در فاکتور است");
                            }
                        }
                    }
                }
                chehckessentioal(Row["column02"].ToString());
                using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Conacnt.Open();
                    SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + Row["Column26"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                    if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0)
                        throw new Exception("برای صدور حواله به انبار انتخاب شده دسترسی ندارید");

                }

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(Row["Column26"].ToString()),
                        int.Parse(item.Cells["column02"].Value.ToString())))
                        throw new Exception("کالای " + item.Cells["column02"].Text +
                            " در انبار انتخاب شده فعال نمی باشد");
                }
                if (Row["Column01"].ToString().StartsWith("-"))
                {
                    gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column01").ToString());
                    this.table_021_MarjooiBuyBindingSource.EndEdit();
                }

                txt_TotalPrice.Value = Convert.ToDouble(
                   gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                   AggregateFunction.Sum).ToString());


                double Total = double.Parse(txt_TotalPrice.Value.ToString());
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                {
                    if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                    {
                        item.BeginEdit();
                        item.Cells["Column04"].Value = (gridEX1.GetValue("Column15").ToString() == "True" ?
                            Total * double.Parse(item.Cells["Column03"].Value.ToString()) / 100
                            : Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100));

                        item.EndEdit();

                    }
                }
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) -
                       Convert.ToDouble(txt_Reductions.Value.ToString());

                if (Convert.ToDouble(txt_EndPrice.Value) < Convert.ToDouble(0))
                {
                    Class_BasicOperation.ShowMsg("", "جمع کل فاکتور منفی شده است", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }
                string RowID = ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                int DocId = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column11", RowID);
                int DraftId = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column10", RowID);
                this.Cursor = Cursors.WaitCursor;


                string command = string.Empty;
                Boolean ok = true;
                if (DocId > 0)
                {
                    clDoc.IsFinal_ID(DocId);
                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=20 and Column17=" + RowID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=20 and Column17=" + RowID;



                    command += "Update     " + ConSale.Database + ".dbo.Table_021_MarjooiBuy set  Column11=0  where   columnid=" + RowID;


                }
                if (DraftId > 0)
                {
                    OldDraftNum = Convert.ToInt32(clDoc.ExScalarQuery(Properties.Settings.Default.WHRS, @"SELECT ISNULL(
                                                                                                                   (
                                                                                                                       SELECT column01
                                                                                                                       FROM   Table_007_PwhrsDraft
                                                                                                                       WHERE  columnid = " + DraftId + @"
                                                                                                                   ),
                                                                                                                   0
                                                                                                               ) AS column01"));


                    command += "Delete  from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where column01=" + DraftId;
                    command += "Delete  from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft where   columnid=" + DraftId;
                    command += "Update     " + ConSale.Database + ".dbo.Table_021_MarjooiBuy set  Column10=0   where   columnid=" + RowID;



                }





                if (!string.IsNullOrWhiteSpace(command))
                {
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                    {
                        Con.Open();

                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;
                        try
                        {
                            Command.CommandText = command;
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();


                        }
                        catch (Exception es)
                        {
                            ok = false;
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;

                            Class_BasicOperation.CheckExceptionType(es, this.Name);

                        }
                    }
                }

                if (ok)
                {
                  

                   

                    Row["Column07"] = Class_BasicOperation._UserName;
                    Row["Column08"] = Class_BasicOperation.ServerDate();
                    Row["Column18"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString();
                    Row["Column21"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                    Row["Column22"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                    //Extra-Reductions
                    Janus.Windows.GridEX.GridEXFilterCondition Filter2 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                    Row["Column19"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                    Filter2.Value1 = true;
                    Row["Column20"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();

                    this.table_021_MarjooiBuyBindingSource.EndEdit();
                    this.table_022_Child1_MarjooiBuyBindingSource.EndEdit();
                    this.table_023_Child2_MarjooiBuyBindingSource.EndEdit();
                    this.table_021_MarjooiBuyTableAdapter.Update(dataSet_Buy.Table_021_MarjooiBuy);
                    this.table_022_Child1_MarjooiBuyTableAdapter.Update(dataSet_Buy.Table_022_Child1_MarjooiBuy);
                    this.table_023_Child2_MarjooiBuyTableAdapter.Update(dataSet_Buy.Table_023_Child2_MarjooiBuy);
                    Row = (DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current;
                    checksanad();
                    string sanadcmd = string.Empty;
                    SqlParameter DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
                    DraftNum.Direction = ParameterDirection.Output;

                    SqlParameter DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                    DocNum.Direction = ParameterDirection.Output;

                    SqlParameter draftkey = new SqlParameter("draftkey", SqlDbType.Int);
                    draftkey.Direction = ParameterDirection.Output;
                    sanadcmd = "     declare @DocID int set @DraftNum=" + (OldDraftNum > 0 ? OldDraftNum.ToString() : "( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft)") + @"";
                    sanadcmd += @" INSERT INTO " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ([column01]
                                                                                               ,[column02]
                                                                                               ,[column03]
                                                                                               ,[column04]
                                                                                               ,[column05]
                                                                                               ,[column06]
                                                                                               ,[column07]
                                                                                               ,[column08]
                                                                                               ,[column09]
                                                                                               ,[column10]
                                                                                               ,[column11]
                                                                                               ,[column12]
                                                                                               ,[column13]
                                                                                               ,[column14]
                                                                                               ,[column15]
                                                                                               ,[column16]
                                                                                               ,[column17]
                                                                                               ,[column18]
                                                                                               ,[column19]
                                                                                               ,[Column20]
                                                                                               ,[Column21]
                                                                                               ,[Column22]
                                                                                               ,[Column23]
                                                                                               ,[Column24]
                                                                                               ,[Column25]
                                                                                               ,[Column26]) VALUES(@DraftNum,'" + Row["column02"].ToString() + "'," + Row["column26"].ToString()
                                 + "," + Row["column27"].ToString() + @", " + Row["column03"].ToString() + ",'" + "حواله صادره بابت مرجوعی خرید ش" + Row["column01"].ToString() +
                                 "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0,0," + ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + ",0,0,0,0,null,0,1); SET @draftkey=SCOPE_IDENTITY()";

                    foreach (DataRowView item in table_022_Child1_MarjooiBuyBindingSource)
                    {
                        if (clDoc.IsGood(item["Column02"].ToString()))
                        {
                            sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]) VALUES(@draftkey," + item["Column02"].ToString() + "," +
                                      item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                                      item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," +
                                      item["Column11"].ToString() + ",NULL," +
                                      (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString())
                                      + "," + (double.Parse(item["Column07"].ToString()) > 0 ? double.Parse(item["Column20"].ToString()) / double.Parse(item["Column07"].ToString()) : 0) + "," +
                                      item["Column20"].ToString() + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                                      + "',getdate(),NULL,NULL," +
                                      (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," +
                                      item["Column14"].ToString() + ",0,0,0,0,0," + (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column30"].ToString().Trim() + "'")
                                           + "," + (item["Column31"].ToString().Trim() == "" ? "NULL" : "'" + item["Column31"].ToString().Trim() + "'") + "," +
                                      item["Column28"].ToString() + "," + item["Column29"].ToString() + "," + item["Column32"].ToString() + "," + item["Column33"].ToString() + ")";
                        }
                    }

                    sanadcmd += "Update " + ConSale.Database + ".dbo.Table_021_MarjooiBuy set Column10=@draftkey,column07='" + Class_BasicOperation._UserName + "',column08=getdate() where ColumnId=" + ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    if (LastDocnum > 0)
                        sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                    else
                        sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + Row["column02"] + "',2,0,N'فاکتور مرجوعی خرید','" + Class_BasicOperation._UserName +
                   "',getdate()); SET @DocID=SCOPE_IDENTITY()";

                    string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());
                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + Row["column03"] + @", NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @" ,
                   " + "'فاکتور مرجوعی خرید " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                         Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";


                    _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @"  ,
                   " + "'فاکتور مرجوعی خرید " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";
                    foreach (DataRow dr in Sanaddt.Rows)
                    {
                        if (dr["Kosoorat"] != null &&
                            dr["Kosoorat"].ToString() != string.Empty &&
                            Convert.ToDouble(dr["Kosoorat"]) > Convert.ToDouble(0))
                        {


                            _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @"  ,
                   " + "'تخفیف فاکتور مرجوعی خرید ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               " + Row["column03"] + @", NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @"  ,
                   " + "'تخفیف فاکتور مرجوعی خرید ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                        }

                        if (dr["Ezafat"] != null &&
                          dr["Ezafat"].ToString() != string.Empty &&
                          Convert.ToDouble(dr["Ezafat"]) > Convert.ToDouble(0))
                        {

                            _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + Row["column03"] + @", NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @"  ,
                   " + "'ارزش افزوده فاکتور مرجوعی خرید ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((Row["column28"] != null && !string.IsNullOrWhiteSpace(Row["column28"].ToString())) ? Row["column28"] : "NULL") + @"  ,
                   " + "'ارزش افزوده فاکتور مرجوعی خرید ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,20," + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                        }


                    }
                    sanadcmd += " Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07=@DocID,column10='" + Class_BasicOperation._UserName + "',column11=getdate() where ColumnId=@draftkey";
                    sanadcmd += " Update " + ConSale.Database + ".dbo.Table_021_MarjooiBuy set Column11=@DocID,column07='" + Class_BasicOperation._UserName + "',column08=getdate() where ColumnId =" + ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                    {
                        Con.Open();

                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;

                        try
                        {
                            Command.CommandText = sanadcmd;
                            Command.Parameters.Add(DocNum);
                            Command.Parameters.Add(DraftNum);
                            Command.Parameters.Add(draftkey);
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();


                            bool dok = true;

                            try
                            {

                                using (SqlConnection Con1 = new SqlConnection(Properties.Settings.Default.WHRS))
                                {
                                    Con1.Open();
                                    SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=(select columnid from Table_007_PwhrsDraft where column01=" + DraftNum.Value + ")", Con1);
                                    DataTable Table = new DataTable();
                                    goodAdapter.Fill(Table);

                                    //محاسبه ارزش و ذخیره آن در جدول Child1



                                    foreach (DataRow item in Table.Rows)
                                    {
                                        // اگر فاکتور مرجوعی فاکتور خرید داشت ارزش رسید ان فاکتور خرید خوانده میشود
                                        SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT ISNULL(Table_012_Child_PwhrsReceipt.column20,0) AS column20
                                                                                    FROM   Table_012_Child_PwhrsReceipt
                                                                                           JOIN Table_011_PwhrsReceipt tpr
                                                                                                ON  tpr.columnid = Table_012_Child_PwhrsReceipt.column01
                                                                                           JOIN " + ConSale.Database + @".dbo.Table_015_BuyFactor tbf
                                                                                                ON  tbf.column10 = tpr.columnid
                                                                                           JOIN " + ConSale.Database + @".dbo.Table_021_MarjooiBuy tmb
                                                                                                ON  tmb.column17 = tbf.columnid
                                                                                    WHERE  tmb.columnid = " + Row["ColumnId"] + @"
                                                                                           AND Table_012_Child_PwhrsReceipt.column02 =" + item["Column02"].ToString(), Con1);
                                        DataTable TurnTable = new DataTable();
                                        Adapter.Fill(TurnTable);
                                        if (TurnTable.Rows.Count > 0)
                                        {
                                            SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["column20"].ToString()), 4)
                                               + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["column20"].ToString()) * double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con1);
                                            UpdateCommand.ExecuteNonQuery();
                                        }
                                        // در غیر این صورت ارزش میانیگین محاسبه میشود
                                        else
                                        {
                                            if (Class_BasicOperation._WareType)
                                            {
                                                Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO] " : "  [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column26"], Con1);
                                                TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", " Date<='" + Row["Column02"].ToString() + "'").ToString());
                                                DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                                if (SelectedRow.Count() > 0)
                                                {
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(SelectedRow[0]["RemainFee"].ToString()), 4)
                                                        + " , Column16=" + Math.Round(double.Parse(SelectedRow[0]["RemainFee"].ToString()) * double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con1);
                                                    UpdateCommand.ExecuteNonQuery();
                                                }
                                            }
                                            else
                                            {
                                                Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column26"] + ",@Date='" + Row["Column02"].ToString() + "',@id=" + draftkey.Value + ",@residid=0", Con1);
                                                TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                                {
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                            + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con1);
                                                    UpdateCommand.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                dok = false;
                            }
                            if (dok)
                                Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                                  "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره حواله انبار: " + DraftNum.Value, "Information");
                            else
                                Class_BasicOperation.ShowMsg("", "ثبت فاکتور موفقیت انجام شد" + Environment.NewLine +
                                "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره حواله انبار: " + DraftNum.Value + Environment.NewLine + "محاسبه ارزش حواله با خطا مواجه شد", "Information");


                            int k = int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                            dataSet_Buy.EnforceConstraints = false;
                            table_021_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_021_MarjooiBuy, k);
                            table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_022_Child1_MarjooiBuy, k);
                            table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_023_Child2_MarjooiBuy, k);
                            dataSet_Buy.EnforceConstraints = true;
                            table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);



                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }

                        this.Cursor = Cursors.Default;






                        ////آپدیت کردن ارزش کالا در حواله
                        ////خالص خطی تقسیم بر تعداد
                        //if (Convert.ToInt32(gridEX1.GetValue("column10")) > 0)
                        //{
                        //    double _arzeshvahed = 0;


                        //    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                        //    {
                        //        if (Convert.ToDouble(item.Cells["column07"].Value.ToString()) == 0)
                        //            _arzeshvahed = 0;
                        //        else
                        //            _arzeshvahed = Convert.ToDouble(item.Cells["column20"].Value.ToString()) /
                        //            Convert.ToDouble(item.Cells["column07"].Value.ToString());

                        //        clDoc.RunSqlCommand(ConWare.ConnectionString,
                        //               "update Table_008_Child_PwhrsDraft " +
                        //               "set column08=" + item.Cells["column08"].Value.ToString() +
                        //               ",column09=" + item.Cells["column09"].Value.ToString() +
                        //               ",column10=" + item.Cells["column10"].Value.ToString() + "," +
                        //               "column11=" + item.Cells["column11"].Value.ToString() +
                        //               ",column15=" + _arzeshvahed.ToString() + ",column16=" +
                        //               item.Cells["column20"].Value.ToString() +
                        //               " where column01=" + gridEX1.GetValue("column10").ToString() +
                        //               " and column02=" + item.Cells["column02"].Value.ToString());
                        //    }
                        // }

                        bt_New.Enabled = true;



                    }


                }

            }
            else if (gridEX_List.AllowEdit == InheritableBoolean.False)
            {
                Class_BasicOperation.ShowMsg("", "امکان ثبت تغییرات وجود ندارد", "Stop");

                return;
            }

        }


        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                SaveEvent(sender, e);

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }

        }

        //private bool CheckRequests()
        //{
        //    Janus.Windows.GridEX.GridEXFormatStyle DefaultStyle = new GridEXFormatStyle();
        //    Janus.Windows.GridEX.GridEXFormatStyle NotInFactorStyle = new GridEXFormatStyle();
        //    DefaultStyle.BackColor = Color.White;
        //    NotInFactorStyle.BackColor = Color.Pink;
        //    Janus.Windows.GridEX.GridEXFormatStyle NotConfirmStyle = new GridEXFormatStyle();
        //    NotConfirmStyle.BackColor = Color.Yellow;
        //    Janus.Windows.GridEX.GridEXFormatStyle DifferStyle = new GridEXFormatStyle();
        //    DifferStyle.BackColor = Color.Cyan;

        //    bool changed = false;
        //    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
        //    {
        //        Con.Open();
        //        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
        //        {
        //            DataRowView Row = (DataRowView)item.DataRow;

        //            if (item.Cells["Column24"].Text.Trim() != "" && item.Cells["Column24"].Text.Trim() != "0")
        //            {
        //                SqlCommand Command = new SqlCommand("SELECT ISNULL ((Select ColumnId from Table_014_Child_RequestBuy where Column02=" +
        //                    item.Cells["Column02"].Value.ToString() + " and Column01=" + item.Cells["Column24"].Value.ToString() + "), 0)", Con);
        //                int ChildId = 0;
        //                ChildId = int.Parse(Command.ExecuteScalar().ToString());
        //                if (ChildId > 0)
        //                {
        //                    item.BeginEdit();
        //                    item.Cells["Column27"].Value = ChildId;
        //                    item.EndEdit();

        //                    SqlCommand Command2 = new SqlCommand("Select Column09,Column08 from Table_014_Child_RequestBuy where Column02=" +
        //                        item.Cells["Column02"].Value.ToString() + " and Column01=" + item.Cells["Column24"].Value.ToString(), Con);
        //                    SqlDataReader reader = Command2.ExecuteReader();
        //                    reader.Read();
        //                    bool Confirm = bool.Parse(reader["Column09"].ToString());

        //                    if (!Confirm)
        //                    {
        //                        item.RowStyle = NotConfirmStyle;
        //                        changed = true;
        //                    }
        //                    else
        //                    {
        //                        if (float.Parse(item.Cells["Column07"].Value.ToString()) > float.Parse(reader["Column08"].ToString()))
        //                            item.RowStyle = DifferStyle;
        //                        else item.RowStyle = DefaultStyle;
        //                    }
        //                    reader.Close();
        //                }
        //                else
        //                {
        //                    item.RowStyle = NotInFactorStyle;
        //                    changed = true;
        //                }
        //            }
        //            else item.RowStyle = DefaultStyle;
        //        }

        //        return changed;
        //    }
        //}

        public void bt_Del_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            DataTable Table = new DataTable();
            if (this.table_021_MarjooiBuyBindingSource.Count > 0)
            {
                try
                {
                    if (!_del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");



                    string RowID = ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    int DocID = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column11", RowID);
                    int DraftID = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column10", RowID);
                    int BuyID = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column17", RowID);

                    if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری و حواله انبار مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        if (DocID > 0)
                        {
                            clDoc.IsFinal_ID(DocID);
                            //حذف سند فاکتور 
                            // clDoc.DeleteDetail_ID(DocID, 20, int.Parse(RowID));
                            Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocID + " and Column16=20 and Column17=" + int.Parse(RowID));
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=20  and Column17=" + int.Parse(RowID);






                            //حذف سند مربوط به حواله
                            //  clDoc.DeleteDetail_ID(DocID, 13, DraftID);

                            Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocID + " and Column16=13 and Column17=" + DraftID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=13  and Column17=" + DraftID;



                        }
                        //حذف فاکتور
                        //foreach (DataRowView item in this.table_022_Child1_MarjooiBuyBindingSource)
                        //{
                        //    item.Delete();
                        //}
                        //this.table_022_Child1_MarjooiBuyBindingSource.EndEdit();
                        //this.table_022_Child1_MarjooiBuyTableAdapter.Update(dataSet_Buy.Table_022_Child1_MarjooiBuy);
                        //foreach (DataRowView item in this.table_023_Child2_MarjooiBuyBindingSource)
                        //{
                        //    item.Delete();
                        //}
                        //this.table_023_Child2_MarjooiBuyBindingSource.EndEdit();
                        //this.table_023_Child2_MarjooiBuyTableAdapter.Update(dataSet_Buy.Table_023_Child2_MarjooiBuy);
                        //this.table_021_MarjooiBuyBindingSource.RemoveCurrent();
                        //this.table_021_MarjooiBuyBindingSource.EndEdit();
                        //this.table_021_MarjooiBuyTableAdapter.Update(dataSet_Buy.Table_021_MarjooiBuy);
                        command += "Delete from " + ConWare.Database + ".dbo. Table_008_Child_PwhrsDraft where column01=" + DraftID;
                        command += "Delete from " + ConWare.Database + ".dbo. Table_007_PwhrsDraft where ColumnId=" + DraftID;

                        command += " Delete from " + ConSale.Database + ".dbo.Table_023_Child2_MarjooiBuy  Where column01 =" + int.Parse(RowID);
                        command += " Delete from " + ConSale.Database + ".dbo.Table_022_Child1_MarjooiBuy  Where column01 =" + int.Parse(RowID);
                        command += " Delete from " + ConSale.Database + ".dbo.Table_021_MarjooiBuy  Where columnid =" + int.Parse(RowID);





                        //صفر کردن شماره فاکتور مرجوعی در فاکتور خرید
                        if (BuyID > 0)
                        {
                            //clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_015_BuyFactor set Column17=0 , Column18=0 where ColumnId=" + BuyID);
                            command += " UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor SET Column17=0,Column18=0  where ColumnId=" + BuyID;

                        }

                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Con.Open();

                            SqlTransaction sqlTran = Con.BeginTransaction();
                            SqlCommand Command = Con.CreateCommand();
                            Command.Transaction = sqlTran;

                            try
                            {
                                Command.CommandText = command;
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();

                                Class_BasicOperation.ShowMsg("", "حذف فاکتور با موفقیت انجام گرفت", "Information");
                                dataSet_Buy.EnforceConstraints = false;
                                this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_021_MarjooiBuy, 0);
                                this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_022_Child1_MarjooiBuy, 0);
                                this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_023_Child2_MarjooiBuy, 0);
                                dataSet_Buy.EnforceConstraints = true;

                            }
                            catch (Exception es)
                            {
                                sqlTran.Rollback();
                                this.Cursor = Cursors.Default;
                                Class_BasicOperation.CheckExceptionType(es, this.Name);

                            }

                            this.Cursor = Cursors.Default;
                        }
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void gridEX1_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_List_Enter(object sender, EventArgs e)
        {
            try
            {

                table_021_MarjooiBuyBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                gridEX1.Focus();
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_List_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            ((Janus.Windows.GridEX.GridEX)sender).CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column02", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

                else if (e.Column.Key == "GoodCode")
                    Class_BasicOperation.FilterGridExDropDown(sender, "GoodCode", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);
            }
            catch { }
        }

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            DataTable Table = new DataTable();
            try
            {
                if (this.table_021_MarjooiBuyBindingSource.Count > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 94))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    if (((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                    {
                        throw new Exception("فاکتور ثبت نشده است");

                    }

                    int RowID = int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int DocID = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column11", RowID.ToString());
                    int DraftID = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column10", RowID.ToString());
                    int BuyID = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column17", RowID.ToString());

                    if (DocID > 0)
                    {

                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف حواله و سند حسابداری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.IsFinal_ID(DocID);
                            //حذف سند فاکتور 
                            // clDoc.DeleteDetail_ID(DocID, 20, RowID);

                            Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocID + " and Column16=20 and Column17=" + RowID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=20 and Column17=" + RowID;






                            //حذف سند مربوط به حواله
                            // clDoc.DeleteDetail_ID(DocID, 13, RowID);

                            Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocID + " and Column16=13 and Column17=" + RowID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=13 and Column17=" + RowID;




                            //حذف حواله
                            //clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_007_PwhrsDraft SET Column07=0 where ColumnId=" + DraftID);
                            command += " UPDATE " + ConWare.Database + ".dbo.Table_007_PwhrsDraft SET Column07=0 where ColumnId=" + DraftID;

                            command += "Delete from " + ConWare.Database + ".dbo. Table_008_Child_PwhrsDraft where column01=" + DraftID;
                            command += "Delete from " + ConWare.Database + ".dbo. Table_007_PwhrsDraft where ColumnId=" + DraftID;

                            //آپدیت شماره سند  در فاکتور
                            //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column11", "ColumnId", RowID, 0);

                            command += " UPDATE " + ConSale.Database + ".dbo.Table_021_MarjooiBuy SET Column11=0,Column10=0,Column07='" + Class_BasicOperation._UserName + "', Column08=getdate() where ColumnId=" + RowID;



                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                            {
                                Con.Open();

                                SqlTransaction sqlTran = Con.BeginTransaction();
                                SqlCommand Command = Con.CreateCommand();
                                Command.Transaction = sqlTran;
                                try
                                {
                                    Command.CommandText = command;
                                    Command.ExecuteNonQuery();
                                    sqlTran.Commit();
                                    Class_BasicOperation.ShowMsg("", "حذف سند حسابداری و حواله با موفقیت صورت گرفت", "Information");

                                }
                                catch (Exception es)
                                {
                                    sqlTran.Rollback();
                                    this.Cursor = Cursors.Default;
                                    Class_BasicOperation.CheckExceptionType(es, this.Name);
                                }
                                this.Cursor = Cursors.Default;
                            }
                        }
                    }
                    gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                    dataSet_Buy.EnforceConstraints = false;
                    this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_021_MarjooiBuy, RowID);
                    this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_022_Child1_MarjooiBuy, RowID);
                    this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_023_Child2_MarjooiBuy, RowID);
                    dataSet_Buy.EnforceConstraints = true;
                    table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        public void bt_Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
            {
                try
                {
                    bt_New.Enabled = true;
                    gridEX_Extra.UpdateData();
                    gridEX_List.UpdateData();
                    this.table_021_MarjooiBuyBindingSource.EndEdit();
                    this.table_022_Child1_MarjooiBuyBindingSource.EndEdit();
                    this.table_023_Child2_MarjooiBuyBindingSource.EndEdit();
                    //if (dataSet_Buy.Table_010_SaleFactor.GetChanges() != null || dataSet_Buy.Table_011_Child1_SaleFactor.GetChanges() != null || dataSet_Buy.Table_012_Child2_SaleFactor.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        bt_Save_Click(sender, e);
                    //    }
                    //}

                    dataSet_Buy.EnforceConstraints = false;
                    int ReturnedID = ReturnIDNumber(int.Parse(txt_Search.Text.Trim()));
                    this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_021_MarjooiBuy, ReturnedID);
                    this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_022_Child1_MarjooiBuy, ReturnedID);
                    this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_023_Child2_MarjooiBuy, ReturnedID);
                    dataSet_Buy.EnforceConstraints = true;
                    gridEX1.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_007_PwhrsDraft"), "");
                    gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                    gridEX1.DropDowns["Buy"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_015_BuyFactor"), "");
                    txt_Search.SelectAll();
                    this.table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
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
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_021_MarjooiBuy where column01=" + FactorNum + " and (Column28=" + projectId + " or '" + (Isadmin) + "'=N'True')", Con);
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

        private void Frm_013_ReturnFactor_Activated(object sender, EventArgs e)
        {
            txt_Search.Focus();
        }

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            int SanadId = 0;
            if (this.table_021_MarjooiBuyBindingSource.Count > 0)
                SanadId = (((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column11"].ToString() == "0" ? 0 : int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column11"].ToString()));
            string sanadnum = clDoc.ExScalar(ConAcnt.ConnectionString, @"select Column00 from Table_060_SanadHead where Columnid=" + ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column11"].ToString() + "");

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
                        txt_S.Text = sanadnum;
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

        private void mnu_ExtraDiscount_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 47))
            {
                _02_BasicInfo.Frm_003_TakhfifEzafeBuy ob = new _02_BasicInfo.Frm_003_TakhfifEzafeBuy(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 48));
                ob.ShowDialog();
                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
                DS.Tables["Discount"].Rows.Clear();
                Adapter.Fill(DS, "Discount");
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
                Int16 ware = 0;

                if (this.table_021_MarjooiBuyBindingSource.CurrencyManager.Count > 0 &&
                    ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column26"] != null &&
                    !string.IsNullOrWhiteSpace(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column26"].ToString())
                    )
                    ware = Convert.ToInt16(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column26"]);
                else if (storefactor.Rows[0]["ware"] != null &&
                 !string.IsNullOrWhiteSpace(storefactor.Rows[0]["ware"].ToString()))
                    ware = Convert.ToInt16(storefactor.Rows[0]["ware"]);
                string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + gridEX1.GetValue("Column28") + "),0)");
                if (controlremain=="True")
                {
                    GoodbindingSource.DataSource = clGood.MahsoolInfo(ware);
                    DataTable GoodTable = clGood.MahsoolInfo(ware);
                    gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                    gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                }
                else
                {
                    GoodbindingSource.DataSource = clGood.GoodInfo();
                    DataTable GoodTable = clGood.GoodInfo();
                    gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                    gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                }
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
                gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from ListPeople(5)"), "");
                gridEX1.DropDowns["Buyer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,4)"), "");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "column05")
                {
                    gridEX1.EnterKeyBehavior = EnterKeyBehavior.None;
                    txt_GoodCode.Select();
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

        private void gridEX1_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Control)
                gridEX1.CurrentCellDroppedDown = true;

            gridEX1.SetValue("Column07", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column08", Class_BasicOperation.ServerDate());

            if (e.Column.Key == "column15")
            {
                if (gridEX1.GetValue("Column15").ToString() == "True")
                {
                    gridEX1.RootTable.Columns["Column23"].Selectable = true;
                    gridEX1.RootTable.Columns["Column24"].Selectable = true;
                }
                else
                {
                    gridEX1.RootTable.Columns["Column23"].Selectable = false;
                    gridEX1.RootTable.Columns["Column24"].Selectable = false;
                }
            }

            if (e.Column.Key == "column03")
                Class_BasicOperation.FilterGridExDropDown(sender, "Column03", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            if (e.Column.Key == "Column28")
                Class_BasicOperation.FilterGridExDropDown(sender, "Column28", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);


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
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }

        private void gridEX_List_EditingCell(object sender, EditingCellEventArgs e)
        {
            try
            {
                if (((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column10"].ToString() != "0" &&
                    ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column11"].ToString() == "0")
                    if (e.Column.Key == "column08" || e.Column.Key == "column09" || e.Column.Key == "column10" || e.Column.Key == "column11" ||
                        e.Column.Key == "column16" || e.Column.Key == "column18")
                        e.Cancel = false;
                    else
                        e.Cancel = true;

            }
            catch
            {
            }
        }

        private void bt_ViewFactors_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 96))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_017_ViewReturnBuyFactors")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                _04_Buy.Frm_017_ViewReturnBuyFactors frm = new Frm_017_ViewReturnBuyFactors();
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

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            if (this.table_021_MarjooiBuyBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 93))
                        throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");

                    //if (((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                    SaveEvent(sender, e);

                    string RowID = ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    //اگر سند صادر شده باشد، اطلاعات به روز رسانی میشوند 
                    if (clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column11", RowID) != 0)
                    {
                        dataSet_Buy.EnforceConstraints = false;
                        this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_021_MarjooiBuy, int.Parse(RowID));
                        this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_022_Child1_MarjooiBuy, int.Parse(RowID));
                        this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_023_Child2_MarjooiBuy, int.Parse(RowID));
                        dataSet_Buy.EnforceConstraints = true;
                        gridEX1.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_007_PwhrsDraft"), "");
                        gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                        gridEX1.DropDowns["Buy"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_015_BuyFactor"), "");
                        this.table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);
                        throw new Exception("برای این فاکتور سند حسابداری صادر شده است");
                    }

                    SaveEvent(sender, e);

                    if (clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column10", RowID) > 0)
                    {
                        _04_Buy.Frm_015_ExportDocInformation_ReturnFactor frm = new Frm_015_ExportDocInformation_ReturnFactor(true, false, int.Parse(RowID));
                        frm.ShowDialog();

                    }
                    //اگر حواله صادر نشده باشد
                    else
                    {
                        _04_Buy.Frm_015_ExportDocInformation_ReturnFactor frm = new Frm_015_ExportDocInformation_ReturnFactor(true, true, int.Parse(RowID));
                        frm.ShowDialog();
                    }

                    dataSet_Buy.EnforceConstraints = false;
                    this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_021_MarjooiBuy, int.Parse(RowID));
                    this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_022_Child1_MarjooiBuy, int.Parse(RowID));
                    this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_023_Child2_MarjooiBuy, int.Parse(RowID));
                    dataSet_Buy.EnforceConstraints = true;
                    gridEX1.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_007_PwhrsDraft"), "");
                    gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                    gridEX1.DropDowns["Buy"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_015_BuyFactor"), "");
                    this.table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_ExportDraft_Click(object sender, EventArgs e)
        {
            if (this.table_021_MarjooiBuyBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                string Date = ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column02"].ToString();
                try
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 95))
                        throw new Exception("کاربر گرامی شما امکان صدور حواله انبار را ندارید");
                    SaveEvent(sender, e);


                    if (clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column10", RowID) > 0)
                        throw new Exception("برای این فاکتور، حواله انبار صادر شده است");





                    DataTable Table = new DataTable();
                    Table.Columns.Add("GoodId", Type.GetType("System.String"));
                    Table.Columns.Add("GoodName", Type.GetType("System.String"));
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        Table.Rows.Add(item.Cells["Column02"].Value.ToString(),
                            item.Cells["Column02"].Text.Trim());
                    }
                    _04_Buy.Frm_016_DraftInformation_ReturnFactor frm =
                        new Frm_016_DraftInformation_ReturnFactor(Table, int.Parse(RowID), Date);
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        string Function = frm.Function;
                        string Ware = frm.Ware.ToString();
                        int DraftNum = 0;
                        if (frm.havalenum > 0)
                            DraftNum = frm.havalenum;
                        else
                            DraftNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01");
                        //, int.Parse(Ware));

                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                        {
                            DataRowView Row = (DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current;
                            Con.Open();
                            //**Draft Header
                            SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                            key.Direction = ParameterDirection.Output;
                            SqlCommand InsertHeader = new SqlCommand(@"INSERT INTO Table_007_PwhrsDraft ([column01]
                                                                                               ,[column02]
                                                                                               ,[column03]
                                                                                               ,[column04]
                                                                                               ,[column05]
                                                                                               ,[column06]
                                                                                               ,[column07]
                                                                                               ,[column08]
                                                                                               ,[column09]
                                                                                               ,[column10]
                                                                                               ,[column11]
                                                                                               ,[column12]
                                                                                               ,[column13]
                                                                                               ,[column14]
                                                                                               ,[column15]
                                                                                               ,[column16]
                                                                                               ,[column17]
                                                                                               ,[column18]
                                                                                               ,[column19]
                                                                                               ,[Column20]
                                                                                               ,[Column21]
                                                                                               ,[Column22]
                                                                                               ,[Column23]
                                                                                               ,[Column24]
                                                                                               ,[Column25]
                                                                                               ,[Column26]) VALUES(" + DraftNum + ",'" + Row["Column02"].ToString() + "'," + Ware
                           + "," + Function + "," + Row["Column03"].ToString() + ",'" + "حواله صادره از فاکتور مرجوعی خرید ش" + Row["Column01"].ToString() +
                           "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0,0," +
                           Row["Columnid"].ToString() + ",0,0,0," +
                           (Row["Column15"].ToString() == "True" ? "1" : "0") + "," +
                           (Row["Column23"].ToString().Trim() == "" ? "NULL" : Row["Column23"].ToString()) + "," +
                           Row["Column24"].ToString() + ",1); SET @Key=SCOPE_IDENTITY()", Con);
                            InsertHeader.Parameters.Add(key);
                            InsertHeader.ExecuteNonQuery();
                            int DraftId = int.Parse(key.Value.ToString());

                            //هنگام صدور حواله ارزش کالا در حواله برابر با خالص خطی فاکتور مرجوعی خرید تقسیم بر تعداد قرار می گیرد

                            //Draft Detail
                            foreach (DataRowView item in table_022_Child1_MarjooiBuyBindingSource)
                            {
                                SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_008_Child_PwhrsDraft ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35],Column36,Column37) VALUES(" + DraftId + "," + item["Column02"].ToString() + "," +
                                item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                                item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," +
                                item["Column11"].ToString() + ",NULL," + (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString())
                                + "," + (double.Parse(item["Column07"].ToString()) > 0 ? double.Parse(item["Column20"].ToString()) / double.Parse(item["Column07"].ToString()) : 0) + "," +
                                item["Column20"].ToString() + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
                                "',getdate(),NULL,NULL," +
                                (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString())
                                + "," + item["Column14"].ToString() + ",0,0,0,0,0," + (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column30"].ToString().Trim() + "'")
                                + "," + (item["Column31"].ToString().Trim() == "" ? "NULL" : "'" + item["Column31"].ToString().Trim() + "'") + "," + item["Column28"].ToString() + "," +
                                item["Column29"].ToString() + "," + item["Column32"].ToString() + "," + item["Column33"].ToString() + "," + (item["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item["Column34"].ToString().Trim() + "'") + "," + (item["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item["Column35"].ToString().Trim() + "'") + ")", Con);
                                InsertDetail.ExecuteNonQuery();
                            }
                            try
                            {
                                SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + DraftId, Con);
                                DataTable DTable = new DataTable();
                                goodAdapter.Fill(DTable);

                                //محاسبه ارزش و ذخیره آن در جدول Child1

                                foreach (DataRow item in DTable.Rows)
                                {
                                    // اگر فاکتور مرجوعی فاکتور خرید داشت ارزش رسید ان فاکتور خرید خوانده میشود
                                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT ISNULL(Table_012_Child_PwhrsReceipt.column20,0) AS column20
                                                                                    FROM   Table_012_Child_PwhrsReceipt
                                                                                           JOIN Table_011_PwhrsReceipt tpr
                                                                                                ON  tpr.columnid = Table_012_Child_PwhrsReceipt.column01
                                                                                           JOIN " + ConSale.Database + @".dbo.Table_015_BuyFactor tbf
                                                                                                ON  tbf.column10 = tpr.columnid
                                                                                           JOIN " + ConSale.Database + @".dbo.Table_021_MarjooiBuy tmb
                                                                                                ON  tmb.column17 = tbf.columnid
                                                                                    WHERE  tmb.columnid = " + RowID + @"
                                                                                           AND Table_012_Child_PwhrsReceipt.column02 =" + item["Column02"].ToString(), Con);
                                    DataTable TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    if (TurnTable.Rows.Count > 0)
                                    {
                                        SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["column20"].ToString()), 4)
                                           + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["column20"].ToString()) * double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                        UpdateCommand.ExecuteNonQuery();
                                    }
                                    // در غیر این صورت ارزش میانیگین محاسبه میشود
                                    else
                                    {
                                        if (Class_BasicOperation._WareType)
                                        {
                                            Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO] " : "  [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Ware, Con);
                                            TurnTable = new DataTable();
                                            Adapter.Fill(TurnTable);
                                            int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", " Date<='" + Row["Column02"].ToString() + "'").ToString());
                                            DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                            if (SelectedRow.Count() > 0)
                                            {
                                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(SelectedRow[0]["RemainFee"].ToString()), 4)
                                                    + " , Column16=" + Math.Round(double.Parse(SelectedRow[0]["RemainFee"].ToString()) * double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                                UpdateCommand.ExecuteNonQuery();
                                            }
                                        }
                                        else
                                        {
                                            Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Ware + ",@Date='" + Row["Column02"].ToString() + "',@id=" + DraftId + ",@residid=0", Con);
                                            TurnTable = new DataTable();
                                            Adapter.Fill(TurnTable);
                                            if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                            {
                                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                        + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                                UpdateCommand.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }

                            //درج شماره حواله در فاکتور مرجوعی
                            gridEX1.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_007_PwhrsDraft"), "");
                            Row["Column10"] = DraftId;
                            this.table_021_MarjooiBuyBindingSource.EndEdit();
                            this.table_021_MarjooiBuyTableAdapter.Update(dataSet_Buy.Table_021_MarjooiBuy);

                            Class_BasicOperation.ShowMsg("", "ثبت حواله انبار با موفقیت انجام شد", "Information");
                        }
                    }



                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                dataSet_Buy.EnforceConstraints = false;
                this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_021_MarjooiBuy, int.Parse(RowID));
                this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_022_Child1_MarjooiBuy, int.Parse(RowID));
                this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_023_Child2_MarjooiBuy, int.Parse(RowID));
                dataSet_Buy.EnforceConstraints = true;
                gridEX1.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_007_PwhrsDraft"), "");
                gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                gridEX1.DropDowns["Buy"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_015_BuyFactor"), "");
                this.table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);
            }
        }



        private void Frm_013_ReturnFactor_KeyDown(object sender, KeyEventArgs e)
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
            //else if (e.Control && e.KeyCode == Keys.E)
            //    bt_ExportDoc_Click(sender, e);
            //else if (e.Control && e.KeyCode == Keys.L)
            //    bt_DelDoc_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F8)

                mnu_Operations.ShowDropDown();
        }

        private void mnu_Drafts_Click(object sender, EventArgs e)
        {
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 24))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form06_RegisterDrafts")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts frm = new PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts
                    (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 25),
                    (this.table_021_MarjooiBuyBindingSource.Count > 0 ? int.Parse(((DataRowView)
                    this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column10"].ToString()) : -1));
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ViewBuyFactors_Click(object sender, EventArgs e)
        {
            try
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 79))
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_012_ViewBuyFactors")
                        {
                            child.Focus();
                            return;
                        }
                    }
                    _04_Buy.Frm_012_ViewBuyFactors frm = new Frm_012_ViewBuyFactors();
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
            catch { }

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 131))
                {
                    //bt_Save_Click(sender, e);
                    if (this.table_021_MarjooiBuyBindingSource.Count > 0)
                    {
                        SaveEvent1(sender, e);
                        _04_Buy.Reports.Form_ReturnBuyFactorPrint frm = new _04_Buy.Reports.Form_ReturnBuyFactorPrint
                            (int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column01"].ToString()));
                        frm.ShowDialog();
                    }
                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void table_021_MarjooiBuyBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.table_021_MarjooiBuyBindingSource.Count > 0)
                {
                    DataRowView Row = (DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current;

                    if (Convert.ToInt32(Row["ColumnId"]) > 0)
                    {
                        if (Row["Column26"] == DBNull.Value || Row["Column27"] == DBNull.Value ||
                            Row["Column26"] == null || Row["Column27"] == null
                            || string.IsNullOrWhiteSpace(Row["Column26"].ToString()) || string.IsNullOrWhiteSpace(Row["Column27"].ToString()))
                        {
                            if (Convert.ToInt32(Row["column10"]) > 0)
                            {
                                DataTable dt = new DataTable();

                                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT column03,column04 FROM Table_007_PwhrsDraft where columnid=" + Row["column10"], ConWare);
                                Adapter.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    Row["Column26"] = dt.Rows[0]["column03"];
                                    Row["Column27"] = dt.Rows[0]["column04"];
                                }
                               // else
                                    //MessageBox.Show("انبار فاکتور انتخاب نشده است");
                            }

                         //   else

                               // MessageBox.Show("انبار فاکتور انتخاب نشده است");

                        }

                    }



                    ////اگر برای فاکتور فقط رسید صادر شده باشد 
                    //if (Row["Column10"].ToString() != "0" && Row["Column11"].ToString() == "0")
                    //{
                    //    gridEX1.AllowEdit = InheritableBoolean.False;
                    //    gridEX1.Enabled = true;
                    //    gridEX_List.AllowAddNew = InheritableBoolean.False;
                    //    gridEX_List.AllowEdit = InheritableBoolean.True;
                    //    gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                    //    gridEX_Extra.AllowDelete = InheritableBoolean.True;
                    //    gridEX_List.AllowDelete = InheritableBoolean.False;
                    //}
                    ////در صورت اینکه فاکتور دارای سند باشد 
                    //else if (Row["Column11"].ToString().Trim() != "0")
                    //{
                    //    gridEX1.AllowEdit = InheritableBoolean.False;
                    //    gridEX1.Enabled = true;
                    //    gridEX_List.AllowEdit = InheritableBoolean.False;
                    //    gridEX_Extra.AllowEdit = InheritableBoolean.False;
                    //    gridEX_List.AllowAddNew = InheritableBoolean.False;
                    //    gridEX_Extra.AllowAddNew = InheritableBoolean.False;
                    //    gridEX_Extra.AllowDelete = InheritableBoolean.False;
                    //    gridEX_List.AllowDelete = InheritableBoolean.False;
                    //}
                    //else
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

                    gridEX1.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString,
              "Select ColumnId,Column01 from Table_007_PwhrsDraft"), "");
                    gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString,
                        "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                    gridEX1.DropDowns["Buy"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString,
                        "Select ColumnId,Column01 from Table_015_BuyFactor"), "");

                    try
                    {
                        txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
                    }
                    catch
                    {
                    }
                }

            }
            catch
            { }
        }

        private void gridEX_List_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                try
                {
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");

                }
                catch { }

                if (e.Column.Key == "column02")
                    gridEX_List.SetValue("GoodCode", gridEX_List.GetValue("column02").ToString());
                else if (e.Column.Key == "GoodCode")
                    gridEX_List.SetValue("column02", gridEX_List.GetValue("GoodCode").ToString());

                if (e.Column.Key == "column02" || e.Column.Key == "GoodCode")
                {
                    GoodbindingSource.Filter = "GoodID=" + gridEX_List.GetValue("column02").ToString();
                    //  gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                    gridEX_List.SetValue("tedaddarkartoon", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                    gridEX_List.SetValue("tedaddarbaste", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                    gridEX_List.SetValue("column03", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                    gridEX_List.SetValue("column10", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["BuyPrice"].ToString());
                    gridEX_List.SetValue("column09", 0);
                    gridEX_List.SetValue("column08", 0);
                    if (!Class_BasicOperation.CalLinearDis(int.Parse(gridEX1.GetValue("Column03").ToString())))
                    {
                        gridEX_List.SetValue("column16", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());
                        gridEX_List.SetValue("column18", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Extra"].ToString());
                    }
                    else
                    {
                        double[] array = clDoc.LastLinearDiscount(int.Parse(gridEX1.GetValue("Column03").ToString()), int.Parse(gridEX_List.GetValue("Column02").ToString()));
                        gridEX_List.SetValue("column16", array[0]);
                        gridEX_List.SetValue("column18", array[1]);
                    }
                    gridEX_List.SetValue("Column32", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Weight"].ToString());
                }

                else if (e.Column.Key == "Column13")
                {
                    object Value = gridEX_List.GetValue("Column13");
                    DataRowView Row = (DataRowView)gridEX_List.RootTable.Columns["Column13"].DropDown.FindItem(Value);
                    gridEX_List.SetValue("Column14", Row["Column02"]);
                }

                if (gridEX_List.GetRow().Cells["Column13"].Text.Trim() == "" &&
                    Convert.ToDouble(gridEX_List.GetRow().Cells["Column14"].Text.Trim()) == 0)
                {
                    if (gridEX1.GetRow().Cells["Column23"].Text.Trim() != "" &&
                          gridEX1.GetRow().Cells["Column24"].Text.Trim() != "")
                    {
                        gridEX_List.SetValue("Column13", gridEX1.GetValue("Column23").ToString());
                        gridEX_List.SetValue("Column14", gridEX1.GetValue("Column24").ToString());
                    }
                }
                gridEX_List.SetValue("column07",
                (Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarkartoon"))) +
                (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarbaste"))) +
                Convert.ToDouble(gridEX_List.GetValue("column06")));

                gridEX_List.SetValue("Column33", Convert.ToDouble(gridEX_List.GetValue("Column32")) * Convert.ToDouble(gridEX_List.GetValue("Column07")));




                gridEX_List.SetValue("column11",
               Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column07")) * Convert.ToDouble(gridEX_List.GetValue("column10")))
               );


                if (e.Column.Key == "column17")
                    gridEX_List.SetValue("Column16", 0);

                if (e.Column.Key == "column16")
                {
                    if (gridEX1.GetValue("Column15").ToString() == "True")
                        gridEX_List.SetValue("column17", Convert.ToDouble(gridEX_List.GetValue("column11")) * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                    else
                        gridEX_List.SetValue("column17", Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                }


                if (gridEX1.GetValue("Column15").ToString() == "True")
                {
                    Double jam, takhfif, ezafe;
                    jam = Convert.ToDouble(gridEX_List.GetValue("column11"));
                    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                        takhfif = (jam * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                    else takhfif = Convert.ToDouble(gridEX_List.GetValue("Column17").ToString());
                    if (ExtraMethod)
                        ezafe = Convert.ToInt64(((jam - takhfif) *
                         (Convert.ToDouble(gridEX_List.GetValue("column18")) / 100)));
                    else
                        ezafe = (jam *
                            (Convert.ToDouble(gridEX_List.GetValue("column18")) / 100));
                    gridEX_List.SetValue("column17", takhfif);
                    gridEX_List.SetValue("column19", ezafe);
                    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);
                }
                else
                {
                    Int64 jam, takhfif, ezafe;
                    jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")));
                    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                        takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) *
                            Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
                    else takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column17").ToString()));
                    if (ExtraMethod)
                        ezafe = Convert.ToInt64(((jam - takhfif) *
                       (Convert.ToDouble(gridEX_List.GetValue("column18")) / 100)));
                    else
                        ezafe = Convert.ToInt64(jam *
                            Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);

                    gridEX_List.SetValue("column17", takhfif);
                    gridEX_List.SetValue("column19", ezafe);
                    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);
                }



                if (gridEX_List.Row != -1)
                {
                    gridEX_List.UpdateData();
                    txt_TotalPrice.Value = Convert.ToDouble(
                      gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                      AggregateFunction.Sum).ToString());
                    txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                        Convert.ToDouble(txt_Extra.Value.ToString()) -
                        Convert.ToDouble(txt_Reductions.Value.ToString());
                }
                txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                    AggregateFunction.Sum).ToString());
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) -
                       Convert.ToDouble(txt_Reductions.Value.ToString());

            }
            catch
            {
            }
        }

        private void gridEX_List_RecordAdded(object sender, EventArgs e)
        {
            gridEX_List.UpdateData();
            txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                    AggregateFunction.Sum).ToString());
            txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                Convert.ToDouble(txt_Extra.Value.ToString()) -
                Convert.ToDouble(txt_Reductions.Value.ToString());
        }

        private void gridEX_Extra_CellUpdated(object sender, ColumnActionEventArgs e)
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
                    gridEX_Extra.SetValue("column04", (gridEX1.GetValue("Column15").ToString() == "True" ? kol * darsad / 100
                        : Convert.ToInt64(kol * darsad / 100)))
                        ;
                }
            }
            else if (e.Column.Key == "column03")
            {
                Double darsad;
                darsad = Convert.ToDouble(gridEX_Extra.GetValue("Column03").ToString());
                Double kol;
                kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column20"].Value.ToString());
                if (kol == 0)
                    return;
                gridEX_Extra.SetValue("column04", (gridEX1.GetValue("Column15").ToString() == "True" ? kol * darsad / 100
                     : Convert.ToInt64(kol * darsad / 100)))
                     ;
            }

            try
            {
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"],
                    AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                    Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch { }
        }

        private void bt_DefineSignature_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 121))
            {
                _04_Buy.Frm_019_ReturnBuy_Signatures frm = new Frm_019_ReturnBuy_Signatures();
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX1_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                try
                {

                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column03");
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column28");


                }
                catch { }

                if (e.Column.Key == "Column23")
                {
                    object Value = gridEX1.GetValue("Column23");
                    DataRowView Row = (DataRowView)this.gridEX1.RootTable.Columns["Column23"].DropDown.FindItem(Value);
                    gridEX1.SetValue("Column24", Row["Column02"]);

                }
                if (e.Column.Key == "Column23" || e.Column.Key == "Column24")
                {
                    gridEX_List.RootTable.Columns["Column13"].DefaultValue = gridEX1.GetValue("Column23");
                    gridEX_List.RootTable.Columns["Column14"].DefaultValue = gridEX1.GetValue("Column24");
                }

                if (e.Column.Key == "column15")
                {
                    if (gridEX1.GetValue("Column15").ToString() == "False")
                    {
                        gridEX1.SetValue("Column23", DBNull.Value);
                        gridEX1.SetValue("Column24", 0);
                    }
                }
            }
            catch
            {
            }
        }

        private void mnu_DelDraft_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_021_MarjooiBuyBindingSource.Count > 0)
                {
                    int RowID = int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int DraftID = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column10", RowID.ToString());

                    if (DraftID > 0)
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 25))
                        {
                            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
                            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
                            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
                            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;

                            PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts frm = new PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts
                            (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 25), -1);
                            frm.txt_Search.Text = clDoc.ExScalar(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01", "ColumnId", DraftID.ToString());
                            frm.bt_Search_Click(sender, e);
                            frm.bt_Del_Click(sender, e);

                        }
                        else
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف حواله انبار را ندارید", "None");

                    }
                    gridEX1.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_007_PwhrsDraft"), "");
                    gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                    dataSet_Buy.EnforceConstraints = false;
                    this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_021_MarjooiBuy, RowID);
                    this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_022_Child1_MarjooiBuy, RowID);
                    this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(dataSet_Buy.Table_023_Child2_MarjooiBuy, RowID);
                    dataSet_Buy.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bt_NotConfirmDraft_Click(object sender, EventArgs e)
        {
            if (this.table_021_MarjooiBuyBindingSource.Count > 0)
            {
                int DraftId = clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column10", ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                if (DraftId != 0)

                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 69))
                    {
                        string Message = null;

                        if (clDoc.ExScalar(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column26", "ColumnId", DraftId.ToString()) == "True")
                        {
                            Message = "آیا مایل به غیر قطعی کردن حواله انبار هستید؟";
                            if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_007_PwhrsDraft SET Column26=0 where ColumnId=" +
                                    DraftId);
                                Class_BasicOperation.ShowMsg("", "غیرقطعی کردن حواله انبار با موفقیت انجام گرفت", "Information");
                            }

                        }
                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان غیر قطعی کردن حواله را ندارید", "None");
            }
        }

        private void gridEX_List_CancelingCellEdit(object sender, ColumnActionCancelEventArgs e)
        {
            try
            {

                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");

            }
            catch { }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_021_MarjooiBuyBindingSource.EndEdit();
                this.table_022_Child1_MarjooiBuyBindingSource.EndEdit();
                this.table_023_Child2_MarjooiBuyBindingSource.EndEdit();

                //if (dataSet_Buy.Table_021_MarjooiBuy.GetChanges() != null || dataSet_Buy.Table_022_Child1_MarjooiBuy.GetChanges() != null ||
                //    dataSet_Buy.Table_023_Child2_MarjooiBuy.GetChanges() != null)
                //{
                //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                //    {
                //        SaveEvent(sender, e);
                //    }
                //}

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select min(Column01) from Table_021_MarjooiBuy where (Column28=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_021_MarjooiBuy where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Buy.EnforceConstraints = false;
                    this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_021_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_022_Child1_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_023_Child2_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_Buy.EnforceConstraints = true;
                    this.table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (this.table_021_MarjooiBuyBindingSource.Count > 0)
            {
                try
                {
                    gridEX1.UpdateData();
                    table_021_MarjooiBuyBindingSource.EndEdit();
                    this.table_022_Child1_MarjooiBuyBindingSource.EndEdit();
                    this.table_023_Child2_MarjooiBuyBindingSource.EndEdit();

                    //if (dataSet_Buy.Table_021_MarjooiBuy.GetChanges() != null || dataSet_Buy.Table_022_Child1_MarjooiBuy.GetChanges() != null ||
                    //    dataSet_Buy.Table_023_Child2_MarjooiBuy.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        SaveEvent(sender, e);
                    //    }
                    //}


                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString,
                        "Select ISNULL((Select max(Column01) from Table_021_MarjooiBuy where Column01<" +
                        ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column01"].ToString() + " and (Column28=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_021_MarjooiBuy where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_Buy.EnforceConstraints = false;
                        this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_021_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_022_Child1_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_023_Child2_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_Buy.EnforceConstraints = true;
                        this.table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (this.table_021_MarjooiBuyBindingSource.Count > 0)
            {

                try
                {
                    gridEX1.UpdateData();
                    table_021_MarjooiBuyBindingSource.EndEdit();
                    this.table_022_Child1_MarjooiBuyBindingSource.EndEdit();
                    this.table_023_Child2_MarjooiBuyBindingSource.EndEdit();

                    //if (dataSet_Buy.Table_021_MarjooiBuy.GetChanges() != null || dataSet_Buy.Table_022_Child1_MarjooiBuy.GetChanges() != null ||
                    //    dataSet_Buy.Table_023_Child2_MarjooiBuy.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        SaveEvent(sender, e);
                    //    }
                    //}

                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select Min(Column01) from Table_021_MarjooiBuy where Column01>" + ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column01"].ToString() + " and (Column28=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_021_MarjooiBuy where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_Buy.EnforceConstraints = false;
                        this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_021_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_022_Child1_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_023_Child2_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_Buy.EnforceConstraints = true;
                        this.table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_021_MarjooiBuyBindingSource.EndEdit();
                this.table_022_Child1_MarjooiBuyBindingSource.EndEdit();
                this.table_023_Child2_MarjooiBuyBindingSource.EndEdit();

                //if (dataSet_Buy.Table_021_MarjooiBuy.GetChanges() != null || dataSet_Buy.Table_022_Child1_MarjooiBuy.GetChanges() != null ||
                //    dataSet_Buy.Table_023_Child2_MarjooiBuy.GetChanges() != null)
                //{
                //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                //    {
                //        SaveEvent(sender, e);
                //    }
                //}

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select max(Column01) from Table_021_MarjooiBuy where (Column28=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_021_MarjooiBuy where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Buy.EnforceConstraints = false;
                    this.table_021_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_021_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_022_Child1_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_022_Child1_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_023_Child2_MarjooiBuyTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_023_Child2_MarjooiBuy, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_Buy.EnforceConstraints = true;
                    this.table_021_MarjooiBuyBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
        }



        private void txt_GoodCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                InitialNewRow();
            }


            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txt_GoodCode_Enter(object sender, EventArgs e)
        {
            original = InputLanguage.CurrentInputLanguage;
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            var language = InputLanguage.FromCulture(culture);
            if (InputLanguage.InstalledInputLanguages.IndexOf(language) >= 0)
                InputLanguage.CurrentInputLanguage = language;
            else
                InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage;
            try
            {
                /// setnull();
                // table_010_SaleFactorBindingSource.EndEdit();
                table_021_MarjooiBuyBindingSource.EndEdit();
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void txt_GoodCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add)
            {
                txt_Count.Text = (Convert.ToInt32(txt_Count.Text) + 1).ToString();
            }
            else if (e.KeyCode == Keys.Subtract)
            {
                if ((Convert.ToInt32(txt_Count.Text) - 1) > 0)

                    txt_Count.Text = (Convert.ToInt32(txt_Count.Text) - 1).ToString();

            }

        }

        private void txt_GoodCode_MouseEnter(object sender, EventArgs e)
        {

        }

        private void txt_GoodCode_Leave(object sender, EventArgs e)
        {

            var culture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");
            var language = InputLanguage.FromCulture(culture);
            InputLanguage.CurrentInputLanguage = language;
        }

        private void InitialNewRow()
        {
            try
            {
                bool isthere = false;
                if (txt_GoodCode.Text != string.Empty)
                {

                    long codeid = 0;
                    int ok = 0;
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        Con.Open();
                        SqlCommand Comm = new SqlCommand("SELECT tcc.columnid FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.column06='" + txt_GoodCode.Text + "'", Con);
                        codeid = Convert.ToInt64(Comm.ExecuteScalar());
                        Comm = new SqlCommand(@"if exists (select * from table_004_CommodityAndIngredients where column06='" + txt_GoodCode.Text + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok", Con);
                        ok = Convert.ToInt32(Comm.ExecuteScalar());


                    }
                    if (ok == 1)
                    {




                        if (gridEX_List.GetRows().Count() > 0)
                        {
                            string goodcode;
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                            {
                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                                {
                                    Con.Open();
                                    SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                    goodcode = (Comm.ExecuteScalar().ToString());

                                }


                                if (goodcode == txt_GoodCode.Text)
                                {

                                    isthere = true;
                                    item.BeginEdit();
                                    item.Cells["Column07"].Value = Convert.ToInt32(item.Cells["Column07"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);
                                    item.Cells["Column06"].Value = Convert.ToInt32(item.Cells["Column06"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);

                                    item.EndEdit();
                                    double TotalPrice;
                                    if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(item.Cells["Column07"].Value.ToString());
                                        item.BeginEdit();
                                        item.Cells["column11"].Value = TotalPrice;
                                        if (!Class_BasicOperation.CalLinearDis(int.Parse(gridEX1.GetValue("Column03").ToString())))
                                        {
                                            gridEX_List.SetValue("column16", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());
                                            gridEX_List.SetValue("column18", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Extra"].ToString());
                                        }
                                        else
                                        {
                                            double[] array = clDoc.LastLinearDiscount(int.Parse(gridEX1.GetValue("Column03").ToString()), int.Parse(gridEX_List.GetValue("Column02").ToString()));
                                            gridEX_List.SetValue("column16", array[0]);
                                            gridEX_List.SetValue("column18", array[1]);
                                        }
                                        Int64 jam, takhfif = 0, ezafe;
                                        jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")));
                                        if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                                            takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) *
                                                Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
                                        else takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column17").ToString()));
                                        if (ExtraMethod)
                                            ezafe = Convert.ToInt64((Convert.ToDouble(jam)
                                                - Convert.ToDouble(takhfif)) *
                                                Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);
                                        else
                                            ezafe = Convert.ToInt64((Convert.ToDouble(jam)) * Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);


                                        gridEX_List.SetValue("column17", takhfif);
                                        gridEX_List.SetValue("column19", ezafe);
                                        gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);

                                        item.EndEdit();

                                    }
                                    gridEX_List.UpdateData();

                                    //محاسبه قیمتهای انتهای فاکتور
                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());


                                    txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());



                                    break;

                                }

                            }
                            if (!isthere)
                            {

                                gridEX_List.MoveToNewRecord();
                                gridEX_List.SetValue("GoodCode", codeid);
                                gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                                gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                                gridEX_List.SetValue("column02", codeid);



                                GoodbindingSource.Filter = "GoodID=" +
                                        gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                if (GoodbindingSource.CurrencyManager.Position > -1)
                                {
                                    gridEX_List.SetValue("tedaddarkartoon",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                    gridEX_List.SetValue("tedaddarbaste",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());

                                    //gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                    gridEX_List.SetValue("column03",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                    if (!Class_BasicOperation.CalLinearDis(int.Parse(gridEX1.GetValue("Column03").ToString())))
                                    {
                                        gridEX_List.SetValue("column16", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());
                                        gridEX_List.SetValue("column18", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Extra"].ToString());
                                    }
                                    else
                                    {
                                        double[] array = clDoc.LastLinearDiscount(int.Parse(gridEX1.GetValue("Column03").ToString()), int.Parse(gridEX_List.GetValue("Column02").ToString()));
                                        gridEX_List.SetValue("column16", array[0]);
                                        gridEX_List.SetValue("column18", array[1]);
                                    }




                                    gridEX_List.SetValue("column10",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                    "BuyPrice"].ToString());


                                    gridEX_List.SetValue("column09",
                                          0);
                                    gridEX_List.SetValue("column08",
                                       0);

                                    double TotalPrice;
                                    if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                        gridEX_List.SetValue("column11", TotalPrice);


                                        Int64 jam, takhfif = 0, ezafe;
                                        jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")));
                                        if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                                            takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) *
                                                Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
                                        else takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column17").ToString()));
                                        if (ExtraMethod)
                                            ezafe = Convert.ToInt64((Convert.ToDouble(jam)
                                                - Convert.ToDouble(takhfif)) *
                                                Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);
                                        else
                                            ezafe = Convert.ToInt64((Convert.ToDouble(jam)) * Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);


                                        gridEX_List.SetValue("column17", takhfif);
                                        gridEX_List.SetValue("column19", ezafe);
                                        gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);

                                    }


                                    gridEX_List.UpdateData();
                                    //محاسبه قیمتهای انتهای فاکتور
                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());

                                    txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());




                                }



                            }


                        }
                        else
                        {

                            gridEX_List.MoveToNewRecord();
                            gridEX_List.SetValue("GoodCode", codeid);
                            gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                            gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                            gridEX_List.SetValue("column02", codeid);


                            {
                                GoodbindingSource.Filter = "GoodID=" +
                                    gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                if (GoodbindingSource.CurrencyManager.Position > -1)
                                {
                                    gridEX_List.SetValue("tedaddarkartoon",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                    gridEX_List.SetValue("tedaddarbaste",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());

                                    //gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                    gridEX_List.SetValue("column03",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                    if (!Class_BasicOperation.CalLinearDis(int.Parse(gridEX1.GetValue("Column03").ToString())))
                                    {
                                        gridEX_List.SetValue("column16", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());
                                        gridEX_List.SetValue("column18", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Extra"].ToString());
                                    }
                                    else
                                    {
                                        double[] array = clDoc.LastLinearDiscount(int.Parse(gridEX1.GetValue("Column03").ToString()), int.Parse(gridEX_List.GetValue("Column02").ToString()));
                                        gridEX_List.SetValue("column16", array[0]);
                                        gridEX_List.SetValue("column18", array[1]);
                                    }



                                    gridEX_List.SetValue("column10",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                    "BuyPrice"].ToString());
                                    gridEX_List.SetValue("column09",
                                          0);
                                    gridEX_List.SetValue("column08",
                                       0);
                                    double TotalPrice;
                                    if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                        gridEX_List.SetValue("column11", TotalPrice);
                                        Int64 jam, takhfif = 0, ezafe;
                                        jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")));
                                        if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                                            takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) *
                                                Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
                                        else takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column17").ToString()));
                                        if (ExtraMethod)
                                            ezafe = Convert.ToInt64((Convert.ToDouble(jam)
                                                - Convert.ToDouble(takhfif)) *
                                                Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);
                                        else
                                            ezafe = Convert.ToInt64((Convert.ToDouble(jam)) * Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);
                                        gridEX_List.SetValue("column17", takhfif);
                                        gridEX_List.SetValue("column19", ezafe);
                                        gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);
                                    }

                                    gridEX_List.UpdateData();

                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());

                                    txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());



                                }
                            }
                        }


                        addnew();
                    }
                    else//کد کالا در جدول کالا نبوده است
                    {

                        MessageBox.Show("کد کالا نامعتبر است");

                    }
                }
            }
            catch (Exception ex)
            {
            }
            addnew();

        }
        private void addnew()
        {
            txt_Count.Text = "1";
            txt_GoodCode.Text = string.Empty;
            txt_GoodCode.Focus();
            txt_GoodCode.SelectAll();

        }
        private void chehckessentioal(string date)
        {

            discountdt = new DataTable();
            taxdt = new DataTable();
            factordt = new DataTable();


            SqlDataAdapter Adapter = new SqlDataAdapter(
                                               @"SELECT       isnull( column10,'') as column10 , isnull(column16,'') as column16
                                                                    FROM            Table_024_Discount_Buy
                                                                     group by column10,column16
                                                                     ", ConSale);
            discountdt = new DataTable();
            Adapter.Fill(discountdt);
            foreach (DataRow dr in discountdt.Rows)
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + dr["Column10"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات خرید وارد کنید");

                }





                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + dr["Column16"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات خرید وارد کنید");
                }


            }


            Adapter = new SqlDataAdapter(
                                                                   @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                    Column14, Column15, Column16
                                                                        FROM            Table_105_SystemTransactionInfo
                                                                        WHERE        (Column00 = 13) ", ConBase);
            Adapter.Fill(factordt);


            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + factordt.Rows[0]["Column13"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                    throw new Exception("شماره حساب معتبر را در تنظیمات فروش وارد کنید");


            }



            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + factordt.Rows[0]["Column07"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                    throw new Exception("شماره حساب معتبر را در تنظیمات فروش وارد کنید");
            }


            if (((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
            {
                LastDocnum = LastDocNum(date);
                if (LastDocnum > 0)
                    clDoc.IsFinal(LastDocnum);
            }
            else if (Convert.ToInt32(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["column11"]) > 0)
                LastDocnum = Convert.ToInt32(clDoc.ExScalarQuery(Properties.Settings.Default.ACNT, @"SELECT ISNULL(
                                                                                                            (select Column00 from Table_060_SanadHead where ColumnId=" + ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["column11"] + @"),0) AS column01"));




            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["Column02"].ToString());

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

        }
        private void checksanad()
        {
            Sanaddt = new DataTable();

            SqlDataAdapter Adapter = new SqlDataAdapter(@"
                                                                             
SELECT FactorTable.columnid,
       FactorTable.column01,
       FactorTable.date,
       ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
       ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
       FactorTable.Bed,
       FactorTable.Bes,
       FactorTable.NetTotal,
       FactorTable.person,
       FactorTable.Column28

FROM   (
           SELECT dbo.Table_021_MarjooiBuy.columnid,
                  dbo.Table_021_MarjooiBuy.column01,
                  dbo.Table_021_MarjooiBuy.column02 AS Date,
                  dbo.Table_021_MarjooiBuy.column03 AS person,
                  OtherPrice.PlusPrice AS Ezafat,
                  OtherPrice.MinusPrice AS Kosoorat,
                  OtherPrice.Bed,
                  OtherPrice.Bes,
                  dbo.Table_021_MarjooiBuy.Column18 AS NetTotal,
                  dbo.Table_021_MarjooiBuy.Column28  

           FROM   dbo.Table_021_MarjooiBuy
                  
                  LEFT OUTER JOIN (
                           SELECT columnid,
                                  SUM(PlusPrice) AS PlusPrice,
                                  SUM(MinusPrice) AS MinusPrice,
                                  Bed,
                                  Bes
                           FROM   (
                                      SELECT Table_021_MarjooiBuy_2.columnid,
                                             SUM(dbo.Table_023_Child2_MarjooiBuy.column04) AS 
                                             PlusPrice,
                                             0 AS MinusPrice,
                                             td.column10 AS Bes,
                                             td.column16 AS Bed
                                      FROM   dbo.Table_023_Child2_MarjooiBuy
                                             JOIN Table_024_Discount_Buy td
                                                  ON  td.columnid = dbo.Table_023_Child2_MarjooiBuy.column02
                                             INNER JOIN dbo.Table_021_MarjooiBuy AS 
                                                  Table_021_MarjooiBuy_2
                                                  ON  dbo.Table_023_Child2_MarjooiBuy.column01 = 
                                                      Table_021_MarjooiBuy_2.columnid
                                      WHERE  (dbo.Table_023_Child2_MarjooiBuy.column05 = 0)
                                      GROUP BY
                                             Table_021_MarjooiBuy_2.columnid,
                                             dbo.Table_023_Child2_MarjooiBuy.column05,
                                             td.column10,
                                             td.column16
                                      UNION ALL
                                      SELECT Table_021_MarjooiBuy_1.columnid,
                                             0 AS PlusPrice,
                                             SUM(Table_023_Child2_MarjooiBuy_1.column04) AS 
                                             MinusPrice,
                                             td.column10 AS Bes,
                                             td.column16 AS Bed
                                      FROM   dbo.Table_023_Child2_MarjooiBuy AS 
                                             Table_023_Child2_MarjooiBuy_1
                                             JOIN Table_024_Discount_Buy td
                                                  ON  td.columnid = 
                                                      Table_023_Child2_MarjooiBuy_1.column02
                                             INNER JOIN dbo.Table_021_MarjooiBuy AS 
                                                  Table_021_MarjooiBuy_1
                                                  ON  
                                                      Table_023_Child2_MarjooiBuy_1.column01 = 
                                                      Table_021_MarjooiBuy_1.columnid
                                      WHERE  (Table_023_Child2_MarjooiBuy_1.column05 = 1)
                                      GROUP BY
                                             Table_021_MarjooiBuy_1.columnid,
                                             Table_023_Child2_MarjooiBuy_1.column05,
                                             td.column10,
                                             td.column16
                                  ) AS OtherPrice_1
                           GROUP BY
                                  columnid,
                                  OtherPrice_1.Bed,
                                  OtherPrice_1.Bes
                       ) AS OtherPrice
                       ON  dbo.Table_021_MarjooiBuy.columnid = OtherPrice.columnid
       ) AS FactorTable
                                        WHERE  FactorTable.columnid=" + int.Parse(((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + @"
                                                                                                           ", ConSale);
            Adapter.Fill(Sanaddt);

            if (Convert.ToDouble(txt_EndPrice.Value) <= Convert.ToDouble(0) ||
                Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()) <= Convert.ToDouble(0)
                )
                throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));

            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));

            All_Controls_Row1(factordt.Rows[0]["Column07"].ToString(), Convert.ToInt32(Sanaddt.Rows[0]["person"].ToString()), null, ((Sanaddt.Rows[0]["Column28"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column28"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column28"]) : (Int16?)null)));
            All_Controls_Row1(factordt.Rows[0]["Column13"].ToString(), null, null, ((Sanaddt.Rows[0]["Column28"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column28"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column28"]) : (Int16?)null)));

            TPerson.Rows.Add(Int32.Parse(Sanaddt.Rows[0]["person"].ToString()), factordt.Rows[0]["Column07"].ToString(), Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()));
            TAccounts.Rows.Add(factordt.Rows[0]["Column13"].ToString(), (-1 * Convert.ToDouble((Sanaddt.Rows[0]["NetTotal"]))));
            TAccounts.Rows.Add(factordt.Rows[0]["Column07"].ToString(), (Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"])));

            foreach (DataRow dr in Sanaddt.Rows)
            {


                if (Convert.ToDouble(dr["Ezafat"]) > 0)
                {
                    All_Controls_Row1(dr["Bed"].ToString(), int.Parse(dr["person"].ToString()), null, ((Sanaddt.Rows[0]["Column28"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column28"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column28"]) : (Int16?)null)));
                    All_Controls_Row1(dr["Bes"].ToString(), null, null, ((Sanaddt.Rows[0]["Column28"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column28"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column28"]) : (Int16?)null)));
                    TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Ezafat"])));
                    TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Ezafat"])));
                    TPerson.Rows.Add(Int32.Parse(dr["person"].ToString()), dr["Bed"].ToString(), Convert.ToDouble(dr["Ezafat"]));


                }
                if (Convert.ToDouble(dr["Kosoorat"]) > 0)
                {
                    All_Controls_Row1(dr["Bes"].ToString(), int.Parse(dr["person"].ToString()), null, ((Sanaddt.Rows[0]["Column28"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column28"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column28"]) : (Int16?)null)));
                    All_Controls_Row1(dr["Bed"].ToString(), null, null, ((Sanaddt.Rows[0]["Column28"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column28"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column28"]) : (Int16?)null)));
                    TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Kosoorat"])));
                    TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Kosoorat"])));
                    TPerson.Rows.Add(Int32.Parse(dr["person"].ToString()), dr["Bes"].ToString(), Convert.ToDouble(dr["Kosoorat"]));


                }



            }
            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);

        }

        public void All_Controls_Row1(string AccountCode, int? Person, Int16? Center, Int16? Project)
        {
            string m = "فاکتور شماره ی " + ((DataRowView)this.table_021_MarjooiBuyBindingSource.CurrencyManager.Current)["column01"].ToString() + "دخیره شد اما صدور سند به دلیل زیر با خطا مواجه شد" + Environment.NewLine;
            //*********Control Person
            if (AccHasPerson(AccountCode) == 1)
            {
                if (Person == null)
                {
                    bt_New.Enabled = true;
                    throw new Exception(m + "انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasPerson(AccountCode) == 0)
            {
                if (Person != null)
                {
                    bt_New.Enabled = true;
                    throw new Exception(m + "انتخاب شخص برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************ Control Center
            if (AccHasCenter(AccountCode) == 1)
            {
                if (Center == null)
                {

                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasCenter(AccountCode) == 0)
            {
                if (Center != null)
                {
                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************* Control Project
            if (AccHasProject(AccountCode) == 1)
            {
                if (Project == null)
                {
                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasProject(AccountCode) == 0)
            {
                if (Project != null)
                {
                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب پروژه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }


            //using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            //{
            //    ConAcnt.Open();
            //    SqlCommand Command = new SqlCommand("Select Control_Person from AllHeaders() where ACC_Code='" + AccountCode + "'", ConAcnt);
            //    if (Person == null && bool.Parse(Command.ExecuteScalar().ToString()))
            //    {
            //        Row.Cells["Column07"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            //        Row.Cells["Column07"].FormatStyle.BackColor = Color.Yellow;
            //        throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }

            ////    Command.CommandText = "Select Control_Center from AllHeaders() where ACC_Code='" + AccountCode + "'";
            ////    if (Center == null && bool.Parse(Command.ExecuteScalar().ToString()))
            ////    {
            ////        Row.Cells["Column08"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            ////        Row.Cells["Column08"].FormatStyle.BackColor = Color.Yellow;
            ////        throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }

            ////    Command.CommandText = "Select Control_Project from AllHeaders() where ACC_Code='" + AccountCode + "'";
            ////    if (Project == null && bool.Parse(Command.ExecuteScalar().ToString()))
            ////    {
            ////        Row.Cells["Column09"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            ////        Row.Cells["Column09"].FormatStyle.BackColor = Color.Yellow;
            ////        throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }
            //}

        }
        public Int16 AccHasPerson(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Person from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        private string AccountName(string AccountCode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("Select ACC_Name from AllHeaders() where ACC_Code='" + AccountCode + "'", Con);
                string _AccountName = Select.ExecuteScalar().ToString();
                return _AccountName;
            }
        }
        private Int16 AccHasCenter(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Center from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        private Int16 AccHasProject(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Project from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        public int LastDocNum(string date)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Isnull((Select Max(isnull( Column00,0)) from Table_060_SanadHead where Column01='" + date + "'),0)", ConAcnt);
                int Result = int.Parse(Select.ExecuteScalar().ToString());
                return Result;
            }
        }
        private float FirstRemain(int GoodCode, string date, int ware)
        {
            using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                ConWare.Open();
                string CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE      (dbo.Table_011_PwhrsReceipt.column03 = {0}) AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE     (dbo.Table_007_PwhrsDraft.column03 = {0}) AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02) AS derivedtbl_1
                       WHERE     (Date <= '{2}')";
                CommandText = string.Format(CommandText, ware, GoodCode, date);
                SqlCommand Command = new SqlCommand(CommandText, ConWare);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }




    }
}
