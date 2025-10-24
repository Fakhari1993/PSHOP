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
    public partial class Frm_013_ReturnFactor : Form
    {
        bool _del;
        int _ID = 0;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        DataSet DS = new DataSet();
        InputLanguage original;
        DataTable discountdt = new DataTable();
        DataTable taxdt = new DataTable();
        DataTable factordt = new DataTable();
        // DataTable waredt = new DataTable();
        DataTable Sanaddt = new DataTable();
        int LastDocnum = 0;
        bool Isadmin = false;
        Int16 projectId;
        DataTable storefactor = new DataTable();
        //DataTable bahaDT = new DataTable();

        public Frm_013_ReturnFactor(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        public Frm_013_ReturnFactor(bool del, int ID)
        {
            _del = del;
            _ID = ID;
            InitializeComponent();
        }

        private void Frm_013_ReturnFactor_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

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


            foreach (GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
                if (col.Key == "Column13" || col.Key == "Column15")
                    col.DefaultValue = Class_BasicOperation._UserName;
                if (col.Key == "Column14" || col.Key == "Column16")
                    col.DefaultValue = Class_BasicOperation.ServerDate();
            }


            gridEX1.DropDowns["Project"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_035_ProjectInfo"), "");

            gridEX1.DropDowns["Receipt"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"), "");
            gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            gridEX1.DropDowns["Sale"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT     dbo.Table_010_SaleFactor.columnid, dbo.Table_010_SaleFactor.column01, dbo.Table_010_SaleFactor.column02 AS Date, derivedtbl_1.Column02 AS Customer
            FROM         dbo.Table_010_SaleFactor INNER JOIN
                          (SELECT     ColumnId, Column02
                            FROM          " + ConBase.Database + ".dbo.Table_045_PersonInfo) AS derivedtbl_1 ON dbo.Table_010_SaleFactor.column03 = derivedtbl_1.ColumnId"), "");
            gridEX1.DropDowns["Order"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_005_OrderHeader"), "");
            gridEX1.DropDowns["Prefactor"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_007_FactorBefore"), "");


            gridEX1.DropDowns["Ware"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from Table_001_PWHRS"), "");
            gridEX1.DropDowns["operation"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0"), "");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_055_CurrencyInfo");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX1.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");






            Adapter = new SqlDataAdapter(@"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
       dbo.Table_045_PersonInfo.Column01 AS code,
       dbo.Table_045_PersonInfo.Column02 AS NAME,
       dbo.Table_065_CityInfo.Column02 AS shahr,
       dbo.Table_060_ProvinceInfo.Column01 AS ostan,
       dbo.Table_045_PersonInfo.Column06 AS ADDRESS
FROM   dbo.Table_045_PersonInfo
       LEFT JOIN dbo.Table_065_CityInfo
            ON  dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
       LEFT JOIN dbo.Table_060_ProvinceInfo
            ON  dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00
WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)", ConBase);
            Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, ConSale.Database);
            Adapter.Fill(DS, "Customer");
            gridEX1.DropDowns["Customer"].SetDataBinding(DS.Tables["Customer"], "");

            Adapter = new SqlDataAdapter("SELECT  [Column00] AS countiD, Column01 AS countName FROM Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

            Adapter = new SqlDataAdapter("Select * FROM Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(DS, "Center");
            gridEX_List.DropDowns["Center"].SetDataBinding(DS.Tables["Center"], "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_035_ProjectInfo", ConBase);
            Adapter.Fill(DS, "Project");
            gridEX_List.DropDowns["Project"].SetDataBinding(DS.Tables["Project"], "");


            gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId  ,
                                           dbo.Table_045_PersonInfo.Column01,
                                           dbo.Table_045_PersonInfo.Column02,
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
                                           JOIN Table_045_PersonScope tps
                                                ON  tps.Column01 = Table_045_PersonInfo.ColumnId
                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)
                                           AND tps.Column02 = 3"), "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
            Adapter.Fill(DS, "Discount");
            gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");


            if (_ID != 0)
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SALE))
                {
                    con.Open();
                    int ID = 0;
                    SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_018_MarjooiSale where columnid=" + _ID + " and (Column30=" + projectId + " or '" + (Isadmin) + "'=N'True')", con);
                    try
                    {
                        ID = int.Parse(Commnad.ExecuteScalar().ToString());
                        this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, _ID);
                        this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale, _ID);
                        this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, _ID);
                        table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                    }
                    catch
                    {
                        MessageBox.Show("شماره فاکتور وارد شده نامعتبر است");
                        this.Close();
                    }
                }
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
                gridEX1.RootTable.Columns["Column30"].Selectable = false;
                gridEX1.RootTable.Columns["column05"].Selectable = false;

            }
            else
            {
                //mlt_Ware.ReadOnly = false;
                //mlt_project.ReadOnly = false;
                //mlt_PersonSale.ReadOnly = false;


                gridEX1.RootTable.Columns["Column28"].Selectable = true;
                gridEX1.RootTable.Columns["Column30"].Selectable = true;
                gridEX1.RootTable.Columns["column05"].Selectable = true;

            }


            //            Adapter = new SqlDataAdapter(
            //                                                            @"SELECT        isnull(Column02,0) as Column02
            //                                                                        FROM           Table_030_Setting
            //                                                                        WHERE        (ColumnId in (49,46)) order by ColumnId desc  ", ConBase);
            //            Adapter.Fill(waredt);
            GoodbindingSource.DataSource = clGood.MahsoolInfo(((storefactor.Rows[0]["ware"] != DBNull.Value
                && storefactor.Rows[0]["ware"] != null
                && !string.IsNullOrWhiteSpace(storefactor.Rows[0]["ware"].ToString())) ? Convert.ToInt16(storefactor.Rows[0]["ware"]) : Convert.ToInt16(0)));
                string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + StoreTable.Rows[0]["Column05"] + "),0)");
            if (controlremain=="True")
            {
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

        private void table_018_MarjooiSaleBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.table_018_MarjooiSaleBindingSource.Count > 0)
                {
                    DataRowView Row = (DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current;
                    if (Convert.ToInt32(Row["ColumnId"]) > 0)
                    {
                        if (Row["Column28"] == DBNull.Value || Row["Column29"] == DBNull.Value ||
                            Row["Column29"] == null || Row["Column28"] == null
                            || string.IsNullOrWhiteSpace(Row["Column29"].ToString()) || string.IsNullOrWhiteSpace(Row["Column28"].ToString()))
                        {
                            if (Convert.ToInt32(Row["column09"]) > 0)
                            {
                                DataTable dt = new DataTable();

                                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT column03,column04 FROM Table_011_PwhrsReceipt where columnid=" + Row["column09"], ConWare);
                                Adapter.Fill(dt);
                                if (dt.Rows.Count > 0)
                                {
                                    Row["Column28"] = dt.Rows[0]["column03"];
                                    Row["Column29"] = dt.Rows[0]["column04"];
                                }
                               // else
                                   // MessageBox.Show("انبار فاکتور انتخاب نشده است");


                            }

                          //  else

                                //MessageBox.Show("انبار فاکتور انتخاب نشده است");

                        }

                    }
                    ////اگر برای فاکتور فقط رسید صادر شده باشد 
                    //if (Row["Column09"].ToString() != "0" && Row["Column10"].ToString() == "0")
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
                    //else
                    if (/*Row["Column10"].ToString().Trim() != "0"*/Convert.ToBoolean(Row["Column27"]))
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

                    gridEX1.DropDowns["Receipt"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"), "");
                    gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                    gridEX1.DropDowns["Sale"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT     dbo.Table_010_SaleFactor.columnid, dbo.Table_010_SaleFactor.column01, dbo.Table_010_SaleFactor.column02 AS Date, derivedtbl_1.Column02 AS Customer
                         FROM         dbo.Table_010_SaleFactor INNER JOIN
                          (SELECT     ColumnId, Column02
                            FROM          " + ConBase.Database + ".dbo.Table_045_PersonInfo) AS derivedtbl_1 ON dbo.Table_010_SaleFactor.column03 = derivedtbl_1.ColumnId"), "");
                    gridEX1.DropDowns["Order"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_005_OrderHeader"), "");
                    gridEX1.DropDowns["Prefactor"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_007_FactorBefore"), "");


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

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {



                gridEX1.Enabled = true;
                dataSet_Sale.EnforceConstraints = false;
                this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, 0);
                this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale, 0);
                this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, 0);
                dataSet_Sale.EnforceConstraints = true;
                gridEX1.MoveToNewRecord();
                //                gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column01").ToString());
                gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd"));
                gridEX1.SetValue("Column13", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column14", Class_BasicOperation.ServerDate());
                gridEX1.SetValue("Column15", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column16", Class_BasicOperation.ServerDate());
                if ( !string.IsNullOrWhiteSpace( Properties.Settings.Default.Returnfunction.ToString()))
                {
                    gridEX1.SetValue("Column29", Properties.Settings.Default.Returnfunction);

                }
                if (
                   (storefactor.Rows[0]["project"] != DBNull.Value &&
                   storefactor.Rows[0]["project"] != null &&
                   !string.IsNullOrWhiteSpace(storefactor.Rows[0]["project"].ToString())))
                {
                    gridEX1.SetValue("Column28", Convert.ToInt16(storefactor.Rows[0]["ware"]));
                    gridEX1.SetValue("Column30", Convert.ToInt16(storefactor.Rows[0]["project"]));

                }

                if (
                   (storefactor.Rows[0]["saleman"] != DBNull.Value &&
                   storefactor.Rows[0]["saleman"] != null &&
                   !string.IsNullOrWhiteSpace(storefactor.Rows[0]["saleman"].ToString())))
                    gridEX1.SetValue("column05", Convert.ToInt32(storefactor.Rows[0]["saleman"]));


                if (
                 (storefactor.Rows[0]["buyer"] != DBNull.Value &&
                 storefactor.Rows[0]["buyer"] != null &&
                 !string.IsNullOrWhiteSpace(storefactor.Rows[0]["buyer"].ToString())))
                    gridEX1.SetValue("column03", Convert.ToInt32(storefactor.Rows[0]["buyer"]));

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
            if (this.table_018_MarjooiSaleBindingSource.Count > 0 &&
             gridEX_List.AllowEdit == InheritableBoolean.True &&
             gridEX1.GetRow().Cells["Column03"].Text.Trim() != "")
            {
                gridEX1.UpdateData();
                gridEX_List.UpdateData();
                gridEX_Extra.UpdateData();
                this.Cursor = Cursors.WaitCursor;

                int OldDraftNum = 0;
                gridEX_Extra.MoveToNewRecord();
                gridEX_List.MoveToNewRecord();
                DataRowView Row = (DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current;
                if (Convert.ToBoolean(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column27"]))
                {
                    throw new Exception("به علت بسته شدن صندوق امکان ذخیره وجود ندارد");

                }
                if (Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column17"]) > 0)
                {
                    if (clDoc.ExScalar(Properties.Settings.Default.SALE, @" SELECT ISNULL(
                                                                       (select ISNULL(Column45,0)Column45 from Table_010_SaleFactor where columnid=" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column17"].ToString() + @"),
                                                                       0
                                                                   )") == "True")
                        throw new Exception("به علت تسویه فاکتور فروش مربوط به فاکتور جاری، امکان ذخیره وجود ندارد");
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

                if (Row["Column28"].ToString() == "" || Row["Column29"].ToString() == "")
                {
                    this.Cursor = Cursors.Default;

                    MessageBox.Show(" لطفا اطلاعات انبار را تکمیل نمایید");
                    return;
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
                                                                          AND tuai.Column02 = " + Row["Column28"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);

                    if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0)
                        throw new Exception("برای صدور رسید به انبار انتخاب شده دسترسی ندارید");

                }
                string NumberName = "";

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(Row["Column28"].ToString()),
                        int.Parse(item.Cells["column02"].Value.ToString())))
                        throw new Exception("کالای " + item.Cells["column02"].Text +
                            " در انبار انتخاب شده فعال نمی باشد");


                    if (gridEX1.GetValue("Column17").ToString() != "" || gridEX1.GetValue("Column17").ToString() != "0")
                    {

                        table_018_MarjooiSaleBindingSource.EndEdit();
                        DataTable dt = new DataTable();
                        dt = clDoc.ReturnTable(ConSale.ConnectionString, @"select *, t.SUMD - t.SUMR AS r from (SELECT dbo.Table_011_Child1_SaleFactor.column07 AS SUMD , (select isnull((select dbo.Table_019_Child1_MarjooiSale.column07 AS SumR
FROM            dbo.Table_018_MarjooiSale INNER JOIN
                         dbo.Table_019_Child1_MarjooiSale ON dbo.Table_018_MarjooiSale.columnid = dbo.Table_019_Child1_MarjooiSale.column01

where         (dbo.Table_018_MarjooiSale.column17 = " + gridEX1.GetValue("Column17") + @") AND (dbo.Table_019_Child1_MarjooiSale.column02 =  " + item.Cells["COlumn02"].Value + @") AND(dbo.Table_018_MarjooiSale.columnid<>" + gridEX1.GetValue("Columnid") + @")),0) )AS SumR
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01

where         (dbo.Table_010_SaleFactor.columnid = " + gridEX1.GetValue("Column17") + @") AND (dbo.Table_011_Child1_SaleFactor.column02 =  " + item.Cells["COlumn02"].Value + @")
) AS t ");

                        if (dt.Rows.Count > 0)
                        {

                            if (Convert.ToDouble(item.Cells["Column07"].Value) > Convert.ToDouble(dt.Rows[0]["r"]))
                            {


                                NumberName += (((DataRowView)gridEX_List.RootTable.Columns["GoodCode"].DropDown.FindItem(item.Cells["GoodCode"].Value))["GoodCode"].ToString()) + ",";


                            }
                        }
                    }

                }

                if (NumberName != "")
                {
                    Class_BasicOperation.ShowMsg("", " تعداد کد کالا های زیر بیش از حد مجاز، فاکتور فروش مورد نظر می باشد " + Environment.NewLine + NumberName.TrimEnd(','), "Information");
                    this.Cursor = Cursors.Default;
                    return;

                }

                if (Row["Column01"].ToString().StartsWith("-"))
                {
                    gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column01").ToString());
                    this.table_018_MarjooiSaleBindingSource.EndEdit();
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
                        item.Cells["Column04"].Value = (gridEX1.GetValue("Column12").ToString() == "True" ?
                            Total * double.Parse(item.Cells["Column03"].Value.ToString()) / 100 :
                            Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100));
                        item.EndEdit();

                    }
                }
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                Convert.ToDouble(txt_Extra.Value.ToString()) -
                Convert.ToDouble(txt_Reductions.Value.ToString());


                if (Convert.ToDouble(txt_EndPrice.Value) < Convert.ToDouble(0))
                {
                    Class_BasicOperation.ShowMsg("", "جمع کل فاکتور منفی شده است", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }

                string RowID = ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                int DocId = clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column10", RowID);
                int DraftId = clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column09", RowID);
                this.Cursor = Cursors.WaitCursor;
                string command = string.Empty;
                Boolean ok = true;
                if (DocId > 0)
                {
                    clDoc.IsFinal_ID(DocId);
                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=17 and Column17=" + RowID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=17 and Column17=" + RowID;

                    Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=27 and Column17=" + DraftId);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=27 and Column17=" + DraftId;




                    command += "Update     " + ConSale.Database + ".dbo.Table_018_MarjooiSale set  Column10=0  where   columnid=" + RowID;


                }
                if (DraftId > 0)
                {
                    OldDraftNum = Convert.ToInt32(clDoc.ExScalarQuery(Properties.Settings.Default.WHRS, @"SELECT ISNULL(
                                                                                                                   (
                                                                                                                       SELECT column01
                                                                                                                       FROM   Table_011_PwhrsReceipt
                                                                                                                       WHERE  columnid = " + DraftId + @"
                                                                                                                   ),
                                                                                                                   0
                                                                                                               ) AS column01"));


                    command += "Delete  from " + ConWare.Database + ".dbo.Table_012_Child_PwhrsReceipt where column01=" + DraftId;
                    command += "Delete  from " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt where   columnid=" + DraftId;
                    command += "Update     " + ConSale.Database + ".dbo.Table_018_MarjooiSale set  Column09=0   where   columnid=" + RowID;



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
                    Row["Column15"] = Class_BasicOperation._UserName;
                    Row["Column16"] = Class_BasicOperation.ServerDate();
                    Row["Column18"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString();
                    //ذخیره اضافه و کسر خطی
                    Row["Column21"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                    Row["Column22"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                    //Extra-Reductions
                    Janus.Windows.GridEX.GridEXFilterCondition Filter1 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                    Row["Column19"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter1).ToString();
                    Filter1.Value1 = true;
                    Row["Column20"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter1).ToString();
                    txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString())
                       - Convert.ToDouble(txt_Reductions.Value.ToString());
                    this.table_018_MarjooiSaleBindingSource.EndEdit();
                    this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                    this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();
                    this.table_018_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_018_MarjooiSale);
                    this.table_019_Child1_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_019_Child1_MarjooiSale);
                    this.table_020_Child2_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_020_Child2_MarjooiSale);
                    Row = (DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current;



                    checksanad();
                    string sanadcmd = string.Empty;
                    SqlParameter DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
                    DraftNum.Direction = ParameterDirection.Output;
                    SqlParameter draftkey = new SqlParameter("draftkey", SqlDbType.Int);
                    draftkey.Direction = ParameterDirection.Output;
                    SqlParameter DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                    DocNum.Direction = ParameterDirection.Output;
                    sanadcmd = "      declare @DocID int set @DraftNum=" + (OldDraftNum > 0 ? OldDraftNum.ToString() : "( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt)") + @"";
                    sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt (
                                                                            [column01],
                                                                            [column02],
                                                                            [column03],
                                                                            [column04],
                                                                            [column05],
                                                                            [column06],
                                                                            [column07],
                                                                            [column08],
                                                                            [column09],
                                                                            [column10],
                                                                            [column11],
                                                                            [column12],
                                                                            [column13],
                                                                            [column14],
                                                                            [Column15],
                                                                            [Column16],
                                                                            [Column17],
                                                                            [Column18],
                                                                            [Column19],
                                                                            [Column20]
                                                                          ) VALUES (@DraftNum,'" + Row["column02"] + "'," +
                Row["Column28"].ToString() + "," + Row["Column29"].ToString() + "," + Row["column03"].ToString() + ",'" + "رسید صادر شده از فاکتور مرجوعی ش " +
                Row["Column01"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
                "',getdate(),0,0," + Row["ColumnId"].ToString() + ",0,0,NULL,0,1,null); SET @draftkey=Scope_Identity()";


                    foreach (DataRowView item in table_019_Child1_MarjooiSaleBindingSource)
                    {
                        if (clDoc.IsGood(item["Column02"].ToString()))
                        {
                            double value = Convert.ToDouble(item["Column07"]);
                            string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                                "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                item["Column02"].ToString());


                            if (item["column03"].ToString() != orginalunit)
                            {
                                float h = clDoc.GetZarib(Convert.ToInt32(item["Column02"]), Convert.ToInt16(item["column03"]), Convert.ToInt16(orginalunit));
                                value = value * h;
                            }
                            sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35]) VALUES (@draftkey," + item["Column02"].ToString() + "," +
                             orginalunit + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + value + "," +
                             value + "," + item["Column08"].ToString() + " ," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                             (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                             + "',getdate(),0," + double.Parse(item["Column11"].ToString()) / double.Parse(item["Column07"].ToString()) + "," + item["Column11"].ToString()
                             + ",0,NULL,NULL," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                             item["Column15"].ToString()
                             + ",0,0,0," +
                                (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                                (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," + item["Column30"].ToString() + "," +
                             item["Column31"].ToString() + "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + ")";

                        }
                    }

                    sanadcmd += "Update     " + ConSale.Database + ".dbo.Table_018_MarjooiSale set  Column09=@draftkey,column15='" + Class_BasicOperation._UserName + "',column16=getdate()  where   columnid=" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"];

                    if (LastDocnum > 0)
                        sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                    else
                        sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + Row["column02"] + "',2,0,N' فاکتور مرجوعی فروش ','" + Class_BasicOperation._UserName +
                   "',getdate()); SET @DocID=SCOPE_IDENTITY()";

                    string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());

                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'فاکتور مرجوعی فروش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                         Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";

                    _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + Row["column03"] + @", NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'فاکتور مرجوعی فروش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                                " + Row["column03"] + @", NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'تخفیف فاکتور مرجوعی فروش ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               NULL, NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'تخفیف فاکتور مرجوعی فروش ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                                 NULL, NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'ارزش افزوده فاکتور مرجوعی فروش ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + Row["column03"] + @", NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'ارزش افزوده فاکتور مرجوعی فروش ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                        }


                    }
                    sanadcmd += " Update " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt set Column07=@DocID,column10='" + Class_BasicOperation._UserName + "',column11=getdate() where ColumnId=@draftkey";
                    sanadcmd += " Update " + ConSale.Database + ".dbo.Table_018_MarjooiSale set Column10=@DocID,column15='" + Class_BasicOperation._UserName + "',column16=getdate() where ColumnId =" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    Properties.Settings.Default.Returnfunction = gridEX1.GetValue("Column29").ToString();
                    Properties.Settings.Default.Save();
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
                            string notok = string.Empty;
                            try
                            {
                                double value = 0;

                                using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.WHRS))
                                {

                                    conware.Open();


                                    //اگر فاکتور مرجوعی فاقد شماره فاکتور فروش باشد
                                    //ارزش کالا به صورت آخرین ارزش حواله  بزرگتر از صفر در انبار درج می شود

                                    SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_012_Child_PwhrsReceipt where Column01=(select columnid from Table_011_PwhrsReceipt where column01= " + DraftNum.Value + ")", conware);
                                    DataTable Table = new DataTable();
                                    goodAdapter.Fill(Table);


                                    #region
                                    if (Row["Column17"].ToString() == "0")
                                    {

                                        foreach (DataRow item in Table.Rows)
                                        {
                                            if (Class_BasicOperation._WareType)
                                            {
                                                SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO]  " : " [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column28"], conware);
                                                DataTable TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                TurnTable.DefaultView.RowFilter = "Kind=2 and DTotalPrice<>0 and Date<='" + Row["Column02"].ToString() + "'";
                                                if (TurnTable.Rows.Count > 0 && TurnTable.DefaultView.Count > 0)
                                                {
                                                    int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", "Kind=2 and DTotalPrice<>0 and Date<='" + Row["Column02"].ToString() + "'").ToString());
                                                    DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4)
                                                        + " , Column21=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) *
                                                        Convert.ToDouble(item["Column07"].ToString())
                                                        + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                    UpdateCommand.ExecuteNonQuery();
                                                    value += Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) *
                                                        Convert.ToDouble(item["Column07"].ToString());


                                                }
                                            }
                                            else
                                            {
                                                SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column28"] + ",@Date='" + Row["Column02"].ToString() + "',@id=0,@residid=" + draftkey.Value, conware);
                                                DataTable TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                                {
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                            + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                    UpdateCommand.ExecuteNonQuery();

                                                    value += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4);
                                                }
                                                else
                                                {
                                                    Adapter = new SqlDataAdapter(@"SELECT  [dbo].[Table_008_Child_PwhrsDraft].column15
                                                                                                FROM    [dbo].[Table_008_Child_PwhrsDraft]
                                                                                                       JOIN  [dbo].Table_007_PwhrsDraft tpd
                                                                                                            ON  tpd.columnid =  [dbo].[Table_008_Child_PwhrsDraft].column01
                                                                                                WHERE  tpd.column02 <= '" + Row["Column02"].ToString() + @"'
                                                                                                       AND tpd.column03 = " + Row["Column28"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column02 = " + item["Column02"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column15>0
                                                                                                ORDER BY tpd.column02 desc", conware);
                                                    TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    if (TurnTable.Rows.Count > 0)
                                                    {
                                                        SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4)
                                          + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                        UpdateCommand.ExecuteNonQuery();
                                                        value += Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4);
                                                    }
                                                }
                                            }


                                        }

                                    }
                                    #endregion



                                    //اگر فاکتور مرجوعی فروش دارای شماره فاکتور فروش بود
                                    // ارزش رسید بر اساس ارزش کالا در حواله مربوط به فاکتور فروش خوانده می شود
                                    #region
                                    else
                                    {
                                        DataTable DraftChildTable = clDoc.ReturnTable(conware.ConnectionString, "Select * from Table_008_Child_PwhrsDraft where Column01=" +
                                            clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", Row["Column17"].ToString()));
                                        foreach (DataRow item in Table.Rows)
                                        {
                                            DataRow[] FoundRows = DraftChildTable.Select("Column02=" + item["Column02"].ToString());
                                            //اگر در حواله مذکور چنین کالایی موجود باشد از اولین کالا مقدار ارزش واحد خوانده می شود 
                                            if (FoundRows.Length > 0)
                                            {
                                                Double SingleValue = Convert.ToDouble(FoundRows[0]["Column15"].ToString());

                                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + SingleValue
                                                           + " , Column21=" + Convert.ToDouble(item["Column07"].ToString()) * SingleValue
                                                           + " where Column02=" + item["Column02"], conware);
                                                UpdateCommand.ExecuteNonQuery();

                                                value += Convert.ToDouble(item["Column07"].ToString()) * SingleValue;

                                            }
                                            else
                                            //اگر چنین کالایی در حواله نباشد ارزش بر اساس پروسجر محاسبه می شود
                                            {

                                                if (Class_BasicOperation._WareType)
                                                {
                                                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO]  " : " [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column28"], conware);
                                                    DataTable TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    TurnTable.DefaultView.RowFilter = "Kind=2 and DTotalPrice<>0";
                                                    if (TurnTable.Rows.Count > 0 && TurnTable.DefaultView.Count > 0)
                                                    {
                                                        int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", "Kind=2 and DTotalPrice<>0").ToString());
                                                        DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                                        SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4)
                                                            + " , Column21=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) * Convert.ToDouble(item["Column07"].ToString())
                                                            + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                        UpdateCommand.ExecuteNonQuery();
                                                        value += Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) * Convert.ToDouble(item["Column07"].ToString());
                                                    }
                                                }
                                                else
                                                {
                                                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column28"] + ",@Date='" + Row["Column02"].ToString() + "',@id=0,@residid=" + draftkey.Value, conware);
                                                    DataTable TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                                    {
                                                        SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                                + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                        UpdateCommand.ExecuteNonQuery();
                                                        value += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4);
                                                    }
                                                    else
                                                    {
                                                        Adapter = new SqlDataAdapter(@"SELECT  [dbo].[Table_008_Child_PwhrsDraft].column15
                                                                                                FROM    [dbo].[Table_008_Child_PwhrsDraft]
                                                                                                       JOIN  [dbo].Table_007_PwhrsDraft tpd
                                                                                                            ON  tpd.columnid =  [dbo].[Table_008_Child_PwhrsDraft].column01
                                                                                                WHERE  tpd.column02 <= '" + Row["Column02"].ToString() + @"'
                                                                                                       AND tpd.column03 = " + Row["Column28"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column02 = " + item["Column02"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column15>0
                                                                                                ORDER BY tpd.column02 desc", conware);
                                                        TurnTable = new DataTable();
                                                        Adapter.Fill(TurnTable);
                                                        if (TurnTable.Rows.Count > 0)
                                                        {
                                                            SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4)
                                              + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                            UpdateCommand.ExecuteNonQuery();
                                                            value += Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4);
                                                        }
                                                    }
                                                }


                                            }
                                        }
                                    }
                                    #endregion


                                }

                                /*
                                   string bahas = string.Empty;

                                   string did = clDoc.ExScalar(ConWare.ConnectionString,
                                                         "Table_011_PwhrsReceipt", "ColumnId", "Column01",
                                                         DraftNum.Value.ToString());

                                   string docid = clDoc.ExScalar(ConAcnt.ConnectionString,
                                                           "Table_060_SanadHead", "ColumnId", "Column00",
                                                           DocNum.Value.ToString());

                                   if (Class_BasicOperation._FinType)//بهای تمام شده
                                   {
                                       if (value > 0 && Convert.ToInt32(DocNum.Value) > 0)
                                       {
                                           _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column07"].ToString());

                                           bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
             ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                   VALUES(" + docid + @",'" + bahaDT.Rows[0]["Column07"].ToString() + @"',
                               " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                               " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                               " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                               " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               NUll, NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                  " + "'بهای تمام شده فاکتور مرجوعی فروش ش " + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString() + "'," + Convert.ToInt64(value) + @",0,0,0,-1,27," + did + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                             Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                                           _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column13"].ToString());


                                           bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
             ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                   VALUES(" + docid + @",'" + bahaDT.Rows[0]["Column13"].ToString() + @"',
                               " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                               " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                               " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                               " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               NULL, NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                  " + "'بهای تمام شده فاکتور مرجوعی فروش ش " + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString() + "',0," + Convert.ToInt64(value) + @",0,0,-1,27," + did + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                             Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                                           clDoc.RunSqlCommand(Properties.Settings.Default.ACNT, bahas);



                                       }
                                   }*/



                            }
                            catch
                            {
                                notok = "محاسبه ارزش کالاهای رسید با خطا مواجه شد";
                            }



                            dataSet_Sale.EnforceConstraints = false;
                            this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_Sale.Table_018_MarjooiSale, Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"]));
                            this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_Sale.Table_019_Child1_MarjooiSale, Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"]));
                            this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_Sale.Table_020_Child2_MarjooiSale, Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"]));
                            dataSet_Sale.EnforceConstraints = true;
                            this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);

                            if ((sender == bt_Save || sender == this) && string.IsNullOrWhiteSpace(notok))
                                Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                                  "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره رسید انبار: " + DraftNum.Value, "Information");
                            else if (!string.IsNullOrWhiteSpace(notok))
                                Class_BasicOperation.ShowMsg("", "ثبت فاکتور موفقیت انجام شد" + Environment.NewLine +
                                  "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره رسید انبار: " + DraftNum.Value + Environment.NewLine + notok, "Information");
                            if (DialogResult.Yes == MessageBox.Show("آیا مایل به چاپ فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            {
                                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 130))
                                {
                                    _05_Sale.Reports.Form_ReturnSaleFactorPrint frm = new Reports.Form_ReturnSaleFactorPrint(
                                        int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.
                                        CurrencyManager.Current)["Column01"].ToString()));
                                    frm.ShowDialog();
                                }
                                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                            }
                            bt_New_Click(null, null);
                          

                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }

                        this.Cursor = Cursors.Default;

                    }


                    bt_New.Enabled = true;
                   

                }

            }
            else if (gridEX_List.AllowEdit == InheritableBoolean.False)
            {
                Class_BasicOperation.ShowMsg("", "امکان ثبت تغییرات وجود ندارد", "Stop");

                return;
            }
            this.Cursor = Cursors.Default;

            //int id = Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"]);
            //dataSet_Sale.EnforceConstraints = false;
            //this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_Sale.Table_018_MarjooiSale, id);
            //this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_Sale.Table_019_Child1_MarjooiSale, id);
            //this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_Sale.Table_020_Child2_MarjooiSale, id);
            //dataSet_Sale.EnforceConstraints = true;
            //// txt_Search.SelectAll();
            //this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
        }

        private void SaveEvent1(object sender, EventArgs e)
        {
            gridEX1.UpdateData();
            if (this.table_018_MarjooiSaleBindingSource.Count > 0 &&
             gridEX_List.AllowEdit == InheritableBoolean.True &&
             gridEX1.GetRow().Cells["Column03"].Text.Trim() != "")
            {
                gridEX1.UpdateData();
                gridEX_List.UpdateData();
                gridEX_Extra.UpdateData();
                this.Cursor = Cursors.WaitCursor;

                int OldDraftNum = 0;
                gridEX_Extra.MoveToNewRecord();
                gridEX_List.MoveToNewRecord();
                DataRowView Row = (DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current;
                if (Convert.ToBoolean(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column27"]))
                {
                    Class_BasicOperation.ShowMsg("", "به علت بسته شدن صندوق امکان ذخیره وجود ندارد", "Warning");
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column17"]) > 0)
                {
                    if (clDoc.ExScalar(Properties.Settings.Default.SALE, @" SELECT ISNULL(
                                                                       (select ISNULL(Column45,0) Column45 from Table_010_SaleFactor where columnid=" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column17"].ToString() + @"),
                                                                       0
                                                                   )") == "True")
                        throw new Exception("به علت تسویه فاکتور فروش مربوط به فاکتور جاری، امکان ذخیره وجود ندارد");
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
                if (Row["Column28"].ToString() == "" || Row["Column29"].ToString() == "")
                {
                    this.Cursor = Cursors.Default;

                    MessageBox.Show(" لطفا اطلاعات انبار را تکمیل نمایید");
                    return;
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
                                                                          AND tuai.Column02 = " + Row["Column28"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);

                    if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0)
                        throw new Exception("برای صدور رسید به انبار انتخاب شده دسترسی ندارید");

                }

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(Row["Column28"].ToString()),
                        int.Parse(item.Cells["column02"].Value.ToString())))
                        throw new Exception("کالای " + item.Cells["column02"].Text +
                            " در انبار انتخاب شده فعال نمی باشد");
                }
                if (Row["Column01"].ToString().StartsWith("-"))
                {
                    gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column01").ToString());
                    this.table_018_MarjooiSaleBindingSource.EndEdit();
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
                        item.Cells["Column04"].Value = (gridEX1.GetValue("Column12").ToString() == "True" ?
                            Total * double.Parse(item.Cells["Column03"].Value.ToString()) / 100 :
                            Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100));
                        item.EndEdit();

                    }
                }
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                Convert.ToDouble(txt_Extra.Value.ToString()) -
                Convert.ToDouble(txt_Reductions.Value.ToString());


                if (Convert.ToDouble(txt_EndPrice.Value) < Convert.ToDouble(0))
                {
                    Class_BasicOperation.ShowMsg("", "جمع کل فاکتور منفی شده است", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }

                string RowID = ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                int DocId = clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column10", RowID);
                int DraftId = clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column09", RowID);
                this.Cursor = Cursors.WaitCursor;
                string command = string.Empty;
                Boolean ok = true;
                if (DocId > 0)
                {
                    clDoc.IsFinal_ID(DocId);
                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=17 and Column17=" + RowID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=17 and Column17=" + RowID;

                    Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=27 and Column17=" + DraftId);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=27 and Column17=" + DraftId;




                    command += "Update     " + ConSale.Database + ".dbo.Table_018_MarjooiSale set  Column10=0  where   columnid=" + RowID;


                }
                if (DraftId > 0)
                {
                    OldDraftNum = Convert.ToInt32(clDoc.ExScalarQuery(Properties.Settings.Default.WHRS, @"SELECT ISNULL(
                                                                                                                   (
                                                                                                                       SELECT column01
                                                                                                                       FROM   Table_011_PwhrsReceipt
                                                                                                                       WHERE  columnid = " + DraftId + @"
                                                                                                                   ),
                                                                                                                   0
                                                                                                               ) AS column01"));


                    command += "Delete  from " + ConWare.Database + ".dbo.Table_012_Child_PwhrsReceipt where column01=" + DraftId;
                    command += "Delete  from " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt where   columnid=" + DraftId;
                    command += "Update     " + ConSale.Database + ".dbo.Table_018_MarjooiSale set  Column09=0   where   columnid=" + RowID;



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


                    

                   

                    Row["Column15"] = Class_BasicOperation._UserName;
                    Row["Column16"] = Class_BasicOperation.ServerDate();
                    Row["Column18"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString();
                    //ذخیره اضافه و کسر خطی
                    Row["Column21"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                    Row["Column22"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                    //Extra-Reductions
                    Janus.Windows.GridEX.GridEXFilterCondition Filter1 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                    Row["Column19"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter1).ToString();
                    Filter1.Value1 = true;
                    Row["Column20"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter1).ToString();
                    txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString())
                       - Convert.ToDouble(txt_Reductions.Value.ToString());
                    this.table_018_MarjooiSaleBindingSource.EndEdit();
                    this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                    this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();
                    this.table_018_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_018_MarjooiSale);
                    this.table_019_Child1_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_019_Child1_MarjooiSale);
                    this.table_020_Child2_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_020_Child2_MarjooiSale);
                    Row = (DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current;



                    checksanad();
                    string sanadcmd = string.Empty;
                    SqlParameter DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
                    DraftNum.Direction = ParameterDirection.Output;
                    SqlParameter draftkey = new SqlParameter("draftkey", SqlDbType.Int);
                    draftkey.Direction = ParameterDirection.Output;
                    SqlParameter DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                    DocNum.Direction = ParameterDirection.Output;
                    sanadcmd = "      declare @DocID int set @DraftNum=" + (OldDraftNum > 0 ? OldDraftNum.ToString() : "( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt)") + @"";
                    sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt (
                                                                            [column01],
                                                                            [column02],
                                                                            [column03],
                                                                            [column04],
                                                                            [column05],
                                                                            [column06],
                                                                            [column07],
                                                                            [column08],
                                                                            [column09],
                                                                            [column10],
                                                                            [column11],
                                                                            [column12],
                                                                            [column13],
                                                                            [column14],
                                                                            [Column15],
                                                                            [Column16],
                                                                            [Column17],
                                                                            [Column18],
                                                                            [Column19],
                                                                            [Column20]
                                                                          ) VALUES (@DraftNum,'" + Row["column02"] + "'," +
                Row["Column28"].ToString() + "," + Row["Column29"].ToString() + "," + Row["column03"].ToString() + ",'" + "رسید صادر شده از فاکتور مرجوعی ش " +
                Row["Column01"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName +
                "',getdate(),0,0," + Row["ColumnId"].ToString() + ",0,0,NULL,0,1,null); SET @draftkey=Scope_Identity()";


                    foreach (DataRowView item in table_019_Child1_MarjooiSaleBindingSource)
                    {
                        if (clDoc.IsGood(item["Column02"].ToString()))
                        {
                            double value = Convert.ToDouble(item["Column07"]);
                            string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                                "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                item["Column02"].ToString());


                            if (item["column03"] != orginalunit)
                            {
                                float h = clDoc.GetZarib(Convert.ToInt32(item["Column02"]), Convert.ToInt16(item["column03"]), Convert.ToInt16(orginalunit));
                                value = value * h;
                            }
                            sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35]) VALUES (@draftkey," + item["Column02"].ToString() + "," +
                             orginalunit + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + value + "," +
                             value + "," + item["Column08"].ToString() + " ," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                             (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                             + "',getdate(),0," + double.Parse(item["Column11"].ToString()) / double.Parse(item["Column07"].ToString()) + "," + item["Column11"].ToString()
                             + ",0,NULL,NULL," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                             item["Column15"].ToString()
                             + ",0,0,0," +
                                (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                                (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," + item["Column30"].ToString() + "," +
                             item["Column31"].ToString() + "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + ")";

                        }
                    }

                    sanadcmd += "Update     " + ConSale.Database + ".dbo.Table_018_MarjooiSale set  Column09=@draftkey,column15='" + Class_BasicOperation._UserName + "',column16=getdate()  where   columnid=" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"];

                    if (LastDocnum > 0)
                        sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                    else
                        sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + Row["column02"] + "',2,0,N' فاکتور مرجوعی فروش ','" + Class_BasicOperation._UserName +
                   "',getdate()); SET @DocID=SCOPE_IDENTITY()";

                    string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());

                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'فاکتور مرجوعی فروش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                         Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";

                    _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + Row["column03"] + @", NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'فاکتور مرجوعی فروش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                                " + Row["column03"] + @", NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'تخفیف فاکتور مرجوعی فروش ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               NULL, NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'تخفیف فاکتور مرجوعی فروش ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                                 NULL, NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'ارزش افزوده فاکتور مرجوعی فروش ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + Row["column03"] + @", NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                   " + "'ارزش افزوده فاکتور مرجوعی فروش ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,17," + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                        }


                    }
                    sanadcmd += " Update " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt set Column07=@DocID,column10='" + Class_BasicOperation._UserName + "',column11=getdate() where ColumnId=@draftkey";
                    sanadcmd += " Update " + ConSale.Database + ".dbo.Table_018_MarjooiSale set Column10=@DocID,column15='" + Class_BasicOperation._UserName + "',column16=getdate() where ColumnId =" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

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
                            string notok = string.Empty;
                            try
                            {
                                double value = 0;

                                using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.WHRS))
                                {

                                    conware.Open();


                                    //اگر فاکتور مرجوعی فاقد شماره فاکتور فروش باشد
                                    //ارزش کالا به صورت آخرین ارزش حواله  بزرگتر از صفر در انبار درج می شود

                                    SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_012_Child_PwhrsReceipt where Column01=(select columnid from Table_011_PwhrsReceipt where column01= " + DraftNum.Value + ")", conware);
                                    DataTable Table = new DataTable();
                                    goodAdapter.Fill(Table);


                                    #region
                                    if (Row["Column17"].ToString() == "0")
                                    {

                                        foreach (DataRow item in Table.Rows)
                                        {
                                            if (Class_BasicOperation._WareType)
                                            {
                                                SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO]  " : " [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column28"], conware);
                                                DataTable TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                TurnTable.DefaultView.RowFilter = "Kind=2 and DTotalPrice<>0 and Date<='" + Row["Column02"].ToString() + "'";
                                                if (TurnTable.Rows.Count > 0 && TurnTable.DefaultView.Count > 0)
                                                {
                                                    int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", "Kind=2 and DTotalPrice<>0 and Date<='" + Row["Column02"].ToString() + "'").ToString());
                                                    DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4)
                                                        + " , Column21=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) *
                                                        Convert.ToDouble(item["Column07"].ToString())
                                                        + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                    UpdateCommand.ExecuteNonQuery();
                                                    value += Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) *
                                                        Convert.ToDouble(item["Column07"].ToString());


                                                }
                                            }
                                            else
                                            {
                                                SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column28"] + ",@Date='" + Row["Column02"].ToString() + "',@id=0,@residid=" + draftkey.Value, conware);
                                                DataTable TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                                {
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                            + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                    UpdateCommand.ExecuteNonQuery();

                                                    value += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4);
                                                }
                                                else
                                                {
                                                    Adapter = new SqlDataAdapter(@"SELECT  [dbo].[Table_008_Child_PwhrsDraft].column15
                                                                                                FROM    [dbo].[Table_008_Child_PwhrsDraft]
                                                                                                       JOIN  [dbo].Table_007_PwhrsDraft tpd
                                                                                                            ON  tpd.columnid =  [dbo].[Table_008_Child_PwhrsDraft].column01
                                                                                                WHERE  tpd.column02 <= '" + Row["Column02"].ToString() + @"'
                                                                                                       AND tpd.column03 = " + Row["Column28"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column02 = " + item["Column02"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column15>0
                                                                                                ORDER BY tpd.column02 desc", conware);
                                                    TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    if (TurnTable.Rows.Count > 0)
                                                    {
                                                        SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4)
                                          + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                        UpdateCommand.ExecuteNonQuery();
                                                        value += Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4);
                                                    }
                                                }
                                            }


                                        }

                                    }
                                    #endregion



                                    //اگر فاکتور مرجوعی فروش دارای شماره فاکتور فروش بود
                                    // ارزش رسید بر اساس ارزش کالا در حواله مربوط به فاکتور فروش خوانده می شود
                                    #region
                                    else
                                    {
                                        DataTable DraftChildTable = clDoc.ReturnTable(conware.ConnectionString, "Select * from Table_008_Child_PwhrsDraft where Column01=" +
                                            clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", Row["Column17"].ToString()));
                                        foreach (DataRow item in Table.Rows)
                                        {
                                            DataRow[] FoundRows = DraftChildTable.Select("Column02=" + item["Column02"].ToString());
                                            //اگر در حواله مذکور چنین کالایی موجود باشد از اولین کالا مقدار ارزش واحد خوانده می شود 
                                            if (FoundRows.Length > 0)
                                            {
                                                Double SingleValue = Convert.ToDouble(FoundRows[0]["Column15"].ToString());

                                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + SingleValue
                                                           + " , Column21=" + Convert.ToDouble(item["Column07"].ToString()) * SingleValue
                                                           + " where Column02=" + item["Column02"], conware);
                                                UpdateCommand.ExecuteNonQuery();

                                                value += Convert.ToDouble(item["Column07"].ToString()) * SingleValue;

                                            }
                                            else
                                            //اگر چنین کالایی در حواله نباشد ارزش بر اساس پروسجر محاسبه می شود
                                            {

                                                if (Class_BasicOperation._WareType)
                                                {
                                                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO]  " : " [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column28"], conware);
                                                    DataTable TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    TurnTable.DefaultView.RowFilter = "Kind=2 and DTotalPrice<>0";
                                                    if (TurnTable.Rows.Count > 0 && TurnTable.DefaultView.Count > 0)
                                                    {
                                                        int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", "Kind=2 and DTotalPrice<>0").ToString());
                                                        DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                                        SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4)
                                                            + " , Column21=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) * Convert.ToDouble(item["Column07"].ToString())
                                                            + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                        UpdateCommand.ExecuteNonQuery();
                                                        value += Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) * Convert.ToDouble(item["Column07"].ToString());
                                                    }
                                                }
                                                else
                                                {
                                                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Row["Column28"] + ",@Date='" + Row["Column02"].ToString() + "',@id=0,@residid=" + draftkey.Value, conware);
                                                    DataTable TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                                    {
                                                        SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                                + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                        UpdateCommand.ExecuteNonQuery();
                                                        value += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4);
                                                    }
                                                    else
                                                    {
                                                        Adapter = new SqlDataAdapter(@"SELECT  [dbo].[Table_008_Child_PwhrsDraft].column15
                                                                                                FROM    [dbo].[Table_008_Child_PwhrsDraft]
                                                                                                       JOIN  [dbo].Table_007_PwhrsDraft tpd
                                                                                                            ON  tpd.columnid =  [dbo].[Table_008_Child_PwhrsDraft].column01
                                                                                                WHERE  tpd.column02 <= '" + Row["Column02"].ToString() + @"'
                                                                                                       AND tpd.column03 = " + Row["Column28"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column02 = " + item["Column02"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column15>0
                                                                                                ORDER BY tpd.column02 desc", conware);
                                                        TurnTable = new DataTable();
                                                        Adapter.Fill(TurnTable);
                                                        if (TurnTable.Rows.Count > 0)
                                                        {
                                                            SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4)
                                              + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                            UpdateCommand.ExecuteNonQuery();
                                                            value += Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4);
                                                        }
                                                    }
                                                }


                                            }
                                        }
                                    }
                                    #endregion


                                }

                                //if (!Convert.ToBoolean(storefactor.Rows[0]["stock"]))
                                /* {
                                     string bahas = string.Empty;

                                     string did = clDoc.ExScalar(ConWare.ConnectionString,
                                                           "Table_011_PwhrsReceipt", "ColumnId", "Column01",
                                                           DraftNum.Value.ToString());

                                     string docid = clDoc.ExScalar(ConAcnt.ConnectionString,
                                                             "Table_060_SanadHead", "ColumnId", "Column00",
                                                             DocNum.Value.ToString());

                                     if (Class_BasicOperation._FinType)//بهای تمام شده
                                     {
                                         if (value > 0 && Convert.ToInt32(DocNum.Value) > 0)
                                         {
                                             _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column07"].ToString());

                                             bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
               ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                     VALUES(" + docid + @",'" + bahaDT.Rows[0]["Column07"].ToString() + @"',
                                 " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                 " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                 " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                 " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                 NUll, NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                    " + "'بهای تمام شده فاکتور مرجوعی فروش ش " + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString() + "'," + Convert.ToInt64(value) + @",0,0,0,-1,27," + did + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                               Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                                             _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column13"].ToString());


                                             bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
               ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                     VALUES(" + docid + @",'" + bahaDT.Rows[0]["Column13"].ToString() + @"',
                                 " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                 " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                 " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                 " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                 NULL, NULL , " + ((Row["Column30"] != null && !string.IsNullOrWhiteSpace(Row["Column30"].ToString())) ? Row["Column30"] : "NULL") + @" ,
                    " + "'بهای تمام شده فاکتور مرجوعی فروش ش " + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString() + "',0," + Convert.ToInt64(value) + @",0,0,-1,27," + did + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                               Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                                             clDoc.RunSqlCommand(Properties.Settings.Default.ACNT, bahas);



                                         }
                                     }

                                 }*/

                            }
                            catch
                            {
                                notok = "محاسبه ارزش کالاهای رسید با خطا مواجه شد";
                            }



                            dataSet_Sale.EnforceConstraints = false;
                            this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_Sale.Table_018_MarjooiSale, Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"]));
                            this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_Sale.Table_019_Child1_MarjooiSale, Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"]));
                            this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_Sale.Table_020_Child2_MarjooiSale, Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"]));
                            dataSet_Sale.EnforceConstraints = true;
                            this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);

                            if (string.IsNullOrWhiteSpace(notok))
                                Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                                  "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره رسید انبار: " + DraftNum.Value, "Information");
                            else if (!string.IsNullOrWhiteSpace(notok))
                                Class_BasicOperation.ShowMsg("", "ثبت فاکتور موفقیت انجام شد" + Environment.NewLine +
                                  "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره رسید انبار: " + DraftNum.Value + Environment.NewLine + notok, "Information");
                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }

                        this.Cursor = Cursors.Default;

                    }


                    bt_New.Enabled = true;


                }

            }
            else if (gridEX_List.AllowEdit == InheritableBoolean.False)
            {
                Class_BasicOperation.ShowMsg("", "امکان ثبت تغییرات وجود ندارد", "Stop");

                return;
            }
            this.Cursor = Cursors.Default;

            //int id = Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"]);
            //dataSet_Sale.EnforceConstraints = false;
            //this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_Sale.Table_018_MarjooiSale, id);
            //this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_Sale.Table_019_Child1_MarjooiSale, id);
            //this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_Sale.Table_020_Child2_MarjooiSale, id);
            //dataSet_Sale.EnforceConstraints = true;
            //// txt_Search.SelectAll();
            //this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
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

        public void bt_Del_Click(object sender, EventArgs e)
        {

            string command = string.Empty;
            DataTable Table = new DataTable();
            if (this.table_018_MarjooiSaleBindingSource.Count > 0)
            {
                try
                {
                    if (!_del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                    DataRowView Row = (DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current;

                    if (Convert.ToBoolean(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column27"]))
                    {
                        throw new Exception("به علت بسته شدن صندوق امکان حذف وجود ندارد");

                    }

                    string RowID = Row["ColumnId"].ToString();
                    int DocID = clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column10", RowID);
                    int ResidID = clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column09", RowID);
                    int SaleID = clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column17", RowID);


                    if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری و رسید انبار مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        bool ok = true;



                        if (ResidID > 0)
                        {
                            string good = string.Empty;
                            DataTable rt = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from  Table_011_PwhrsReceipt where columnid=" + ResidID);

                            //چک باقی مانده کالا
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                            {

                                float Remain = DeleteRemain(int.Parse(item.Cells["Column02"].Value.ToString()), rt.Rows[0]["column03"].ToString(), rt.Rows[0]["column02"].ToString(), ResidID);
                                if (Remain < Convert.ToDouble(0) || TotalDeleteRemain(int.Parse(item.Cells["Column02"].Value.ToString()), rt.Rows[0]["column03"].ToString(), ResidID) < Convert.ToDouble(0))
                                {
                                    good += item.Cells["Column02"].Text + ",";

                                }
                            }

                            if (!string.IsNullOrWhiteSpace(good))
                            {
                                good = good.TrimEnd(',');

                                string Message1 = "";
                                Message1 = "موجودی کالاهای زیر منفی می شود آیا مایل به حذف رسید هستید؟" + Environment.NewLine + good;
                                if (DialogResult.Yes == MessageBox.Show(Message1, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                                    ok = true;
                                else
                                    ok = false;

                            }

                        }

                        if (ok)
                        {

                            if (DocID > 0)
                            {
                                //if (clDoc.SanadType(ConAcnt.ConnectionString, DocID, int.Parse(RowID), 29) != 29)
                                //{

                                clDoc.IsFinal_ID(DocID);


                                DataTable dtTrun = clDoc.ReturnTable(ConAcnt.ConnectionString, @"
                                    declare @SaleId int
                                    declare @ResidId int
                                 set @SaleId=(select column17 from " + ConSale.Database + ".dbo.Table_018_MarjooiSale where ColumnId = " + RowID + @")
                                 set @ResidId=(select column09 from " + ConSale.Database + ".dbo.Table_018_MarjooiSale where ColumnId = " + RowID + @")

                                select * from " + ConAcnt.Database + ".dbo.Table_065_SanadDetail where " +
                                                "(( Column16=17 and Column17=" + RowID + @") or" +
                                                " (column16=27 and Column17=@ResidId) or " +
                                                " (column16=29 and Column17=" + RowID + @") or " +
                                                 "(column27=29 and column17 in (select Columnid from " + ConBank.Database + ".dbo.Table_065_TurnReception where Column01 in (select Column66 from " + ConSale.Database + ".dbo.Table_010_SaleFactor where Columnid =@SaleId))))");

                                DataTable dtdelet = dtTrun.Select("Column27=29").Length > 0 ? dtTrun.Select("Column27=29").CopyToDataTable() : null;

                                if (dtdelet != null)
                                {
                                    command += "Delete from " + ConBank.Database + ".dbo. Table_065_TurnReception where Columnid=" + dtdelet.Rows[0]["Column17"];
                                }
                                command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=17 and Column17=" + RowID;
                                command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=29 and Column17=" + RowID;
                                command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=27 and Column17=" + ResidID;
                                command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column27=29 and column17 in (select Columnid from " + ConBank.Database + ".dbo.Table_065_TurnReception where Column01 in (select Column66 from " + ConSale.Database + ".dbo.Table_010_SaleFactor where Columnid =" + SaleID + "))";
                                command += " UPDATE " + ConSale.Database + ".dbo.Table_018_MarjooiSale SET Column10=0,Column09=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + RowID;

                            }
                            if (ResidID > 0)
                            {

                                command += "Delete from " + ConWare.Database + ".dbo. Table_012_Child_PwhrsReceipt where column01=" + ResidID;
                                command += "Delete from " + ConWare.Database + ".dbo. Table_011_PwhrsReceipt where ColumnId=" + ResidID;



                            }

                            if (SaleID > 0)
                            {

                                command += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column19=0 where ColumnId=" + SaleID;


                            }


                            command += " Delete from " + ConSale.Database + ".dbo.Table_020_Child2_MarjooiSale  Where column01 =" + int.Parse(RowID);
                            command += " Delete from " + ConSale.Database + ".dbo.Table_019_Child1_MarjooiSale  Where column01 =" + int.Parse(RowID);
                            command += " Delete from " + ConSale.Database + ".dbo.Table_018_MarjooiSale  Where columnid =" + int.Parse(RowID);


                        }

                        Class_BasicOperation.ShowMsg("", "حذف سند حسابداری و رسید با موفقیت صورت گرفت", "Information");
                        Class_BasicOperation.SqlTransactionMethodExecuteNonQuery(ConAcnt.ConnectionString, command);
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, int.Parse(RowID));
                        this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowID));
                        this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowID));
                        dataSet_Sale.EnforceConstraints = true;
                        table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                        this.Cursor = Cursors.Default;
                        bt_New.Enabled = true;
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

                table_018_MarjooiSaleBindingSource.EndEdit();


                if (gridEX1.GetValue("Column17").ToString() != "" || gridEX1.GetValue("COlumn17").ToString() != "0")
                {

                    DataTable dt = new DataTable();

                    DataTable dtsale = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT        dbo.Table_011_Child1_SaleFactor.column02
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
WHERE        (dbo.Table_010_SaleFactor.columnid = " + gridEX1.GetValue("Column17") + ")");

                    if (dtsale.Rows.Count > 0)
                    {

                        string Idgood = "";
                        foreach (DataRow item in dtsale.Rows)
                        {
                            Idgood += item["column02"].ToString() + ",";
                        }
                        dt = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT dbo.table_004_CommodityAndIngredients.columnid AS GoodID,
       dbo.table_004_CommodityAndIngredients.column01 AS GoodCode,
       dbo.table_004_CommodityAndIngredients.column02 AS GoodName,
       dbo.table_004_CommodityAndIngredients.column03 AS MainGroup,
       dbo.table_004_CommodityAndIngredients.column04 AS SubGroup,
       dbo.table_004_CommodityAndIngredients.column07 AS CountUnit,
       dbo.table_004_CommodityAndIngredients.column22 AS WEIGHT,
       dbo.table_004_CommodityAndIngredients.column29 AS Khas ,
	       CASE 
            WHEN table_006.Column07 IS NULL THEN dbo.table_004_CommodityAndIngredients.column09
            ELSE table_006.Column07
       END                               AS NumberInBox,
       CASE 
            WHEN table_006.Column06 IS NULL THEN dbo.table_004_CommodityAndIngredients.column08
            ELSE table_006.Column06
       END                               AS NumberInPack,
       CASE 
            WHEN table_006.Column12 IS NULL THEN dbo.table_004_CommodityAndIngredients.column24
            ELSE table_006.Column12
       END                               AS Tavan,
       CASE 
            WHEN table_006.Column13 IS NULL THEN dbo.table_004_CommodityAndIngredients.column25
            ELSE table_006.Column13
       END                               AS Hajm,
       ISNULL(table_006.Column18, 1)     AS Active1,
	   CASE 
            WHEN TS003.Column03 IS NULL THEN dbo.table_004_CommodityAndIngredients.Column35
            ELSE TS003.Column03
       END                               AS BuyPrice,
       CASE 
            WHEN TS003.Column07 IS NULL THEN dbo.table_004_CommodityAndIngredients.Column34
            ELSE TS003.Column07
       END                               AS SalePrice,
       CASE 
            WHEN TS003.Column09 IS NULL THEN dbo.table_004_CommodityAndIngredients.column39
            ELSE ts003.Column09
       END                               AS SalePackPrice,
       CASE 
            WHEN Ts003.Column10 IS NULL THEN dbo.table_004_CommodityAndIngredients.Column40
            ELSE ts003.column10
       END                               AS SaleBoxPrice,
       CASE 
            WHEN Ts003.Column04 IS NULL THEN dbo.table_004_CommodityAndIngredients.Column36
            ELSE ts003.column04
       END                               AS UsePrice,
       CASE 
            WHEN Ts003.Column05 IS NULL THEN dbo.table_004_CommodityAndIngredients.Column37
            ELSE ts003.column05
       END                               AS Discount,
       CASE 
            WHEN Ts003.Column06 IS NULL THEN dbo.table_004_CommodityAndIngredients.Column38
            ELSE ts003.column06
       END                               AS Extra,
       ISNULL(TS003.Column11, 1)         AS Active2,
       dbo.table_004_CommodityAndIngredients.column28 AS Active3,
	          ISNULL(
        (
               SELECT SUM(tcpr.column07) AS Expr1
               FROM   dbo.Table_012_Child_PwhrsReceipt AS tcpr
                      INNER JOIN dbo.Table_011_PwhrsReceipt AS tpr
                           ON  tpr.columnid = tcpr.column01
               WHERE  (
                          tcpr.column02 = dbo.table_004_CommodityAndIngredients.columnid
                      )
                      AND (
                              tpr.column02 <= '" + FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##") + @"'
                          )
           )
		   ,
           0) 
           -
       ISNULL(
           (
               SELECT SUM(tcpr.column07) AS Expr1
               FROM   dbo.Table_008_Child_PwhrsDraft AS tcpr
                      INNER JOIN dbo.Table_007_PwhrsDraft AS tpr
                           ON  tpr.columnid = tcpr.column01
               WHERE  (
                          tcpr.column02 = dbo.table_004_CommodityAndIngredients.columnid
                      )
                      AND (
                              tpr.column02 <= '" + FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##") + @"'
                          )
           ),
           0
       )                                 AS totalremain,
       ISNULL(
           (
               SELECT SUM(tcpr.column07) AS Expr1
               FROM   dbo.Table_012_Child_PwhrsReceipt AS tcpr
                      INNER JOIN dbo.Table_011_PwhrsReceipt AS tpr
                           ON  tpr.columnid = tcpr.column01
               WHERE  (
                          tcpr.column02 = dbo.table_004_CommodityAndIngredients.columnid
                      )
                      AND (
                              tpr.column02 <= '" + FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##") + @"'
                          )
                      AND (tpr.column03 = " + gridEX1.GetValue("COlumn28") + @")
           ),
           0
       )                              
	 -
       ISNULL(
           (
               SELECT SUM(tcpr.column07) AS Expr1
               FROM   dbo.Table_008_Child_PwhrsDraft AS tcpr
                      INNER JOIN dbo.Table_007_PwhrsDraft AS tpr
                           ON  tpr.columnid = tcpr.column01
               WHERE  (
                          tcpr.column02 = dbo.table_004_CommodityAndIngredients.columnid
                      )
                      AND (
                              tpr.column02 <= '" + FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##") + @"'
                          )
                      AND (tpr.column03 = " + gridEX1.GetValue("COlumn28") + @")
           ),
           0
       )                                 AS wareremain,
	dbo.table_002_MainGroup.column02  AS MainGroupName,
       dbo.table_003_SubsidiaryGroup.column03 AS SubGroupName
	   from table_004_CommodityAndIngredients
	   left join   dbo.table_006_CommodityChanges      AS table_006
	ON  table_006.column01 = dbo.table_004_CommodityAndIngredients.columnid
	left join 
	 dbo.Table_003_InformationProductCash   AS TS003
	  ON  TS003.column01 = dbo.table_004_CommodityAndIngredients.columnid and TS003.Column11=1
	  left join table_002_MainGroup 
	   ON  dbo.table_002_MainGroup.columnid = dbo.table_004_CommodityAndIngredients.column03
       left JOIN dbo.table_003_SubsidiaryGroup
            ON  dbo.table_004_CommodityAndIngredients.column04 = dbo.table_003_SubsidiaryGroup.columnid
			WHERE  (dbo.table_004_CommodityAndIngredients.column19 = 1)
       AND (dbo.table_004_CommodityAndIngredients.column28 = 1) AND 
                         (dbo.table_004_CommodityAndIngredients.columnid in (" + Idgood.TrimEnd(',') + "))");


                        gridEX_List.DropDowns["GoodCode"].SetDataBinding(dt, "");
                        gridEX_List.DropDowns["GoodName"].SetDataBinding(dt, "");
                    }

                }




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
                if (this.table_018_MarjooiSaleBindingSource.Count > 0)
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 89))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    if (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                    {
                        throw new Exception("فاکتور ثبت نشده است");

                    }
                    if (Convert.ToBoolean(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column27"]))
                    {
                        throw new Exception("به علت بسته شدن صندوق امکان حذف وجود ندارد");

                    }

                    if (Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column17"]) > 0)
                    {
                        if (clDoc.ExScalar(Properties.Settings.Default.SALE, @" SELECT ISNULL(
                                                                       (select ISNULL(Column45,0) Column45 from Table_010_SaleFactor where columnid=" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column17"].ToString() + @"),
                                                                       0
                                                                   )") == "True")
                            throw new Exception("به علت تسویه فاکتور فروش مربوط به فاکتور جاری، حذف سند حسابداری امکانپذیر نمی باشد");
                    }
                    int RowID = int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int DocID = clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column10", RowID.ToString());
                    int ResidID = clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column09", RowID.ToString());
                    if (DocID > 0)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف سند و رسید مربوط به این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            //حذف سند فاکتور 

                            string good = string.Empty;
                            bool ok = true;
                            DataTable rt = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from  Table_011_PwhrsReceipt where columnid=" + ResidID);

                            //چک باقی مانده کالا
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                            {

                                float Remain = DeleteRemain(int.Parse(item.Cells["Column02"].Value.ToString()), rt.Rows[0]["column03"].ToString(), rt.Rows[0]["column02"].ToString(), ResidID);

                                if (Remain < Convert.ToDouble(0) || TotalDeleteRemain(int.Parse(item.Cells["Column02"].Value.ToString()), rt.Rows[0]["column03"].ToString(), ResidID) < Convert.ToDouble(0))
                                {
                                    good += item.Cells["Column02"].Text + ",";

                                }
                            }

                            if (!string.IsNullOrWhiteSpace(good))
                            {
                                good = good.TrimEnd(',');

                                string Message1 = "";
                                Message1 = "موجودی کالاهای زیر منفی می شود آیا مایل به حذف رسید هستید؟" + Environment.NewLine + good;
                                if (DialogResult.Yes == MessageBox.Show(Message1, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                                    ok = true;
                                else
                                    ok = false;

                            }

                            if (ok)
                            {

                                //                                  
                                DataTable dtTrun = clDoc.ReturnTable(ConAcnt.ConnectionString, @"
                                    declare @SaleId int
                                    declare @ResidId int
                                 set @SaleId=(select column17 from " + ConSale.Database + ".dbo.Table_018_MarjooiSale where ColumnId = " + RowID + @")
                                 set @ResidId=(select column09 from " + ConSale.Database + ".dbo.Table_018_MarjooiSale where ColumnId = " + RowID + @")

                                select * from " + ConAcnt.Database + ".dbo.Table_065_SanadDetail where " +
                                                   "(( Column16=17 and Column17=" + RowID + @") or" +
                                                   " (column16=27 and Column17=@ResidId) or " +
                                                   " (column16=29 and Column17=" + RowID + @") or " +
                                                    "(column27=29 and column17 in (select Columnid from " + ConBank.Database + ".dbo.Table_065_TurnReception where Column01 in (select Column66 from " + ConSale.Database + ".dbo.Table_010_SaleFactor where Columnid =@SaleId))))");

                                DataTable dtdelet = dtTrun.Select("Column27=29").Length > 0 ? dtTrun.Select("Column27=29").CopyToDataTable() : null;

                                if (dtdelet != null)
                                {
                                    command += "Delete from " + ConBank.Database + ".dbo. Table_065_TurnReception where Columnid=" + dtdelet.Rows[0]["Column17"];
                                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column27=29 and column17  = " + dtdelet.Rows[0]["Column17"];


                                }
                                command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=17 and Column17=" + RowID;
                                command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=29 and Column17=" + RowID;
                                command += " Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=27 and Column17=" + ResidID;


                                if (ResidID > 0)
                                {

                                    command += "Delete from " + ConWare.Database + ".dbo. Table_012_Child_PwhrsReceipt where column01=" + ResidID;
                                    command += "Delete from " + ConWare.Database + ".dbo. Table_011_PwhrsReceipt where ColumnId=" + ResidID;

                                }



                                command += " UPDATE " + ConSale.Database + ".dbo.Table_018_MarjooiSale SET Column10=0,Column09=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + RowID;

                                Class_BasicOperation.ShowMsg("", "حذف سند حسابداری و رسید با موفقیت صورت گرفت", "Information");

                                Class_BasicOperation.SqlTransactionMethodExecuteNonQuery(ConAcnt.ConnectionString, command);

                                this.Cursor = Cursors.Default;

                            }
                        }
                        gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, RowID);
                        this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale, RowID);
                        this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, RowID);
                        dataSet_Sale.EnforceConstraints = true;
                        table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                    }
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
                    this.table_018_MarjooiSaleBindingSource.EndEdit();
                    this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                    this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();
                    //if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null || dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        bt_Save_Click(sender, e);
                    //    }
                    //}
                    int RowID = ReturnIDNumber(int.Parse(txt_Search.Text));
                    dataSet_Sale.EnforceConstraints = false;
                    this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_Sale.Table_018_MarjooiSale, RowID);
                    this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_Sale.Table_019_Child1_MarjooiSale, RowID);
                    this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_Sale.Table_020_Child2_MarjooiSale, RowID);
                    dataSet_Sale.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
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
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                con.Open();
                int ID = 0;
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_018_MarjooiSale where column01=" + FactorNum + " and  (Column30=" + projectId + " or '" + (Isadmin) + "'=N'True')", con);
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
            if (this.table_018_MarjooiSaleBindingSource.Count > 0)
                SanadId = (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column10"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column10"].ToString()));
            string sanadnum = clDoc.ExScalar(ConAcnt.ConnectionString, @"select Column00 from Table_060_SanadHead where Columnid=" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column10"].ToString() + "");

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
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 45))
            {
                _02_BasicInfo.Frm_002_TakhfifEzafeSale ob = new _02_BasicInfo.Frm_002_TakhfifEzafeSale(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 46));
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
                if (this.table_018_MarjooiSaleBindingSource.CurrencyManager.Count > 0 &&
                    ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column28"] != null &&
                    !string.IsNullOrWhiteSpace(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column28"].ToString())
                    )
                    ware = Convert.ToInt16(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column28"]);
                else if (storefactor.Rows[0]["ware"] != null &&
                 !string.IsNullOrWhiteSpace(storefactor.Rows[0]["ware"].ToString()))
                    ware = Convert.ToInt16(storefactor.Rows[0]["ware"]);
                string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" +gridEX1.GetValue("Column30") + "),0)");
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

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "column13")
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
            try
            {
                if (e.Column.Key == "column03")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column03", "code", "name", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }

            try
            {
                if (e.Column.Key == "Column30")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column30", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }

            if (Control.ModifierKeys != Keys.Control)
                gridEX1.CurrentCellDroppedDown = true;
            gridEX1.SetValue("Column15", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column16", Class_BasicOperation.ServerDate());

            if (e.Column.Key == "column12")
            {
                if (gridEX1.GetValue("Column12").ToString() == "False")
                {
                    gridEX1.RootTable.Columns["Column23"].Selectable = false;
                    gridEX1.RootTable.Columns["Column24"].Selectable = false;
                }
                else
                {
                    gridEX1.RootTable.Columns["Column23"].Selectable = true;
                    gridEX1.RootTable.Columns["Column24"].Selectable = true;
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
                if (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column09"].ToString() != "0" &&
                    ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column10"].ToString() == "0")
                {
                    if (e.Column.Key == "column08" || e.Column.Key == "column09" || e.Column.Key == "column10" || e.Column.Key == "column11" ||
                        e.Column.Key == "column16" || e.Column.Key == "column18")
                        e.Cancel = false;
                    else
                        e.Cancel = true;
                }

            }
            catch
            {
            }
        }

        private void bt_ViewFactors_Click(object sender, EventArgs e)
        {
            try
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 92))
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_016_ViewReturnSaleFactors")
                        {
                            child.Focus();
                            return;
                        }
                    }
                    _05_Sale.Frm_016_ViewReturnSaleFactors frm = new Frm_016_ViewReturnSaleFactors();
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

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            if (this.table_018_MarjooiSaleBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 91))
                        throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");

                    // if (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                    SaveEvent(sender, e);


                    string RowID = ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column10", RowID) != 0)
                    {
                        gridEX1.DropDowns["Receipt"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"), "");
                        gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, int.Parse(RowID));
                        this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowID));
                        this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowID));
                        dataSet_Sale.EnforceConstraints = true;
                        this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                        throw new Exception("برای این فاکتور سند صادر شده است");
                    }

                    SaveEvent(sender, e);
                    if (clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column09", RowID) != 0)
                    {
                        //***************************if Finance Type is Periodic: Just export Doc for factor and Doc number will be save in Draft's n
                        //سیستم ادواری
                        if (!Class_BasicOperation._FinType)
                        {
                            _05_Sale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, false, false, int.Parse(RowID));
                            frm.ShowDialog();
                        }
                        //سیستم دائمی
                        else
                        {
                            _05_Sale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, false, true, int.Parse(RowID));
                            frm.ShowDialog();
                        }

                    }
                    //اگر رسید صادر نشده باشد
                    else
                    {
                        //سیستم ادواری
                        if (!Class_BasicOperation._FinType)
                        {
                            _05_Sale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, true, false, int.Parse(RowID));
                            frm.ShowDialog();
                        }
                        //سیستم دائمی
                        else
                        {
                            _05_Sale.Frm_015_ExportDoc_Return frm = new Frm_015_ExportDoc_Return(true, true, true, int.Parse(RowID));
                            frm.ShowDialog();
                        }
                    }

                    gridEX1.DropDowns["Receipt"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"), "");
                    gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                    dataSet_Sale.EnforceConstraints = false;
                    this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, int.Parse(RowID));
                    this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowID));
                    this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowID));
                    dataSet_Sale.EnforceConstraints = true;
                    this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_ExportResid_Click(object sender, EventArgs e)
        {
            if (this.table_018_MarjooiSaleBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                try
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 90))
                        throw new Exception("کاربر گرامی شما امکان صدور رسید انبار را ندارید");
                    SaveEvent(sender, e);


                    if (clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column09", RowID) != 0)
                        throw new Exception("برای این فاکتور رسید انبار صادر شده است");


                    DataTable GoodTable = new DataTable();
                    GoodTable.Columns.Add("GoodId", Type.GetType("System.String"));
                    GoodTable.Columns.Add("GoodName", Type.GetType("System.String"));
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        GoodTable.Rows.Add(item.Cells["Column02"].Value.ToString(),
                            item.Cells["Column02"].Text.Trim());
                    }

                    _05_Sale.Frm_014_ResidDialog_Return frm = new Frm_014_ResidDialog_Return(GoodTable);

                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        string Function = frm.FunctionValue;
                        string Ware = frm.WareCode.ToString();
                        int ResidNum = 0;
                        if (frm.residnum > 0)
                            ResidNum = frm.residnum;
                        else
                            ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
                        //, int.Parse(Ware));

                        //**Resid Header
                        #region
                        SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                        key.Direction = ParameterDirection.Output;
                        using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.WHRS))
                        {
                            DataRowView Row = (DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current;
                            conware.Open();
                            SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_011_PwhrsReceipt (
                                                                            [column01],
                                                                            [column02],
                                                                            [column03],
                                                                            [column04],
                                                                            [column05],
                                                                            [column06],
                                                                            [column07],
                                                                            [column08],
                                                                            [column09],
                                                                            [column10],
                                                                            [column11],
                                                                            [column12],
                                                                            [column13],
                                                                            [column14],
                                                                            [Column15],
                                                                            [Column16],
                                                                            [Column17],
                                                                            [Column18],
                                                                            [Column19],
                                                                            [Column20]
                                                                          ) VALUES (" + ResidNum + ",'" + Row["Column02"].ToString() + "'," +
                            Ware + "," + Function + "," + Row["Column03"].ToString() + ",'" + "رسید صادر شده از فاکتور مرجوعی ش " +
                             Row["Column01"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                             Class_BasicOperation._UserName + "',getdate(),0,0," + Row["ColumnId"].ToString() + ",0," +
                              (Row["Column12"].ToString().Trim() == "True" ? "1" : "0") + "," +
                             (Row["Column23"].ToString().Trim() == "" ? "NULL" : Row["Column23"].ToString().Trim()) + "," +
                             Row["Column24"].ToString() + ",1,null); SET @Key=Scope_Identity()", conware);
                            Insert.Parameters.Add(key);
                            Insert.ExecuteNonQuery();
                            int ResidId = int.Parse(key.Value.ToString());
                        #endregion



                            //Resid Detail


                            //اگر فاکتور مرجوعی فاقد شماره فاکتور فروش باشد
                            //ارزش کالا به صورت آخرین ارزش حواله  بزرگتر از صفر در انبار درج می شود
                            //در تاریخ 980520قرار شد ارزش رسید از پروسیجر میانیگن خوانده شود در صورت صفر بود اخرین ارزش حواله بزرگتر صفر خوانده شود
                            #region
                            if (gridEX1.GetRow().Cells["Column17"].Text.Trim() == "0")
                            {

                                foreach (DataRowView item in table_019_Child1_MarjooiSaleBindingSource)
                                {
                                    SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35],Column36,Column37) VALUES (" + ResidId + "," + item["Column02"].ToString() + "," +
                                     item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                                     item["Column07"].ToString() + "," + item["Column08"].ToString() + " ," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                                     (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                                     + "',getdate(),0," + double.Parse(item["Column11"].ToString()) / double.Parse(item["Column07"].ToString()) + "," + item["Column11"].ToString()
                                     + ",0,NULL,NULL," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                                     item["Column15"].ToString()
                                     + ",0,0,0," +
                                     (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                                     (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") +
                                     "," + item["Column30"].ToString() + "," +
                                     item["Column31"].ToString() + "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + "," +
                                      (item["Column36"].ToString().Trim() == "" ? "NULL" : "'" + item["Column36"].ToString() + "'") + "," +
                                     (item["Column37"].ToString().Trim() == "" ? "NULL" : "'" + item["Column37"].ToString() + "'") +

                                     ")", conware);
                                    InsertDetail.ExecuteNonQuery();
                                }

                                SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_012_Child_PwhrsReceipt where Column01=" + ResidId, conware);
                                DataTable Table = new DataTable();
                                goodAdapter.Fill(Table);

                                foreach (DataRow item in Table.Rows)
                                {
                                    if (Class_BasicOperation._WareType)
                                    {
                                        SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO]  " : " [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Ware, conware);
                                        DataTable TurnTable = new DataTable();
                                        Adapter.Fill(TurnTable);
                                        TurnTable.DefaultView.RowFilter = "Kind=2 and DTotalPrice<>0 and Date<='" + Row["Column02"].ToString() + "'";
                                        if (TurnTable.Rows.Count > 0 && TurnTable.DefaultView.Count > 0)
                                        {
                                            int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", "Kind=2 and DTotalPrice<>0 and Date<='" + Row["Column02"].ToString() + "'").ToString());
                                            DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                            SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4)
                                                + " , Column21=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) *
                                                Convert.ToDouble(item["Column07"].ToString())
                                                + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                            UpdateCommand.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Ware + ",@Date='" + Row["Column02"].ToString() + "',@id=0,@residid=" + ResidId, conware);
                                        DataTable TurnTable = new DataTable();
                                        Adapter.Fill(TurnTable);
                                        if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                        {
                                            SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                    + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                            UpdateCommand.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            Adapter = new SqlDataAdapter(@"SELECT  [dbo].[Table_008_Child_PwhrsDraft].column15
                                                                                                FROM    [dbo].[Table_008_Child_PwhrsDraft]
                                                                                                       JOIN  [dbo].Table_007_PwhrsDraft tpd
                                                                                                            ON  tpd.columnid =  [dbo].[Table_008_Child_PwhrsDraft].column01
                                                                                                WHERE  tpd.column02 <= '" + Row["Column02"].ToString() + @"'
                                                                                                       AND tpd.column03 = " + Ware + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column02 = " + item["Column02"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column15>0
                                                                                                ORDER BY tpd.column02 desc", conware);
                                            TurnTable = new DataTable();
                                            Adapter.Fill(TurnTable);
                                            if (TurnTable.Rows.Count > 0)
                                            {
                                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4)
                                  + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), conware);
                                                UpdateCommand.ExecuteNonQuery();
                                            }

                                        }

                                    }
                                }

                            }
                            #endregion



                            //اگر فاکتور مرجوعی فروش دارای شماره فاکتور فروش بود
                            // ارزش رسید بر اساس ارزش کالا در حواله مربوط به فاکتور فروش خوانده می شود
                            #region
                            else
                            {
                                DataTable DraftChildTable = clDoc.ReturnTable(conware.ConnectionString, "Select * from Table_008_Child_PwhrsDraft where Column01=" +
                                    clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", gridEX1.GetValue("Column17").ToString()));
                                foreach (DataRowView item in table_019_Child1_MarjooiSaleBindingSource)
                                {
                                    DataRow[] FoundRows = DraftChildTable.Select("Column02=" + item["Column02"].ToString());
                                    //اگر در حواله مذکور چنین کالایی موجود باشد از اولین کالا مقدار ارزش واحد خوانده می شود 
                                    if (FoundRows.Length > 0)
                                    {
                                        Double SingleValue = Convert.ToDouble(FoundRows[0]["Column15"].ToString());

                                        SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35]) VALUES (" + ResidId + "," + item["Column02"].ToString() + "," +
                                     item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                                     item["Column07"].ToString() + "," + item["Column08"].ToString() + " ," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                                     (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                                     + "',getdate(),0," + SingleValue + "," + Convert.ToDouble(item["Column07"].ToString()) * SingleValue
                                     + ",0,NULL,NULL," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                                     item["Column15"].ToString()
                                     + ",0,0,0," +
                            (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                            (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," + item["Column30"].ToString() + "," +
                                     item["Column31"].ToString() + "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + ")", conware);
                                        InsertDetail.ExecuteNonQuery();
                                    }
                                    else
                                    //اگر چنین کالایی در حواله نباشد ارزش بر اساس پروسجر محاسبه می شود
                                    {
                                        SqlParameter DetailKey = new SqlParameter("DetailKey", SqlDbType.Int);
                                        DetailKey.Direction = ParameterDirection.Output;
                                        SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35]) VALUES (" + ResidId + "," + item["Column02"].ToString() + "," +
                                     item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                                     item["Column07"].ToString() + "," + item["Column08"].ToString() + " ," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                                     (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                                     + "',getdate(),0," + double.Parse(item["Column11"].ToString()) / double.Parse(item["Column07"].ToString()) + "," + item["Column11"].ToString()
                                     + ",0,NULL,NULL," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                                     item["Column15"].ToString()
                                     + ",0,0,0," +
                            (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                            (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," + item["Column30"].ToString() + "," +
                                     item["Column31"].ToString() + "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + "); SET @DetailKey=SCOPE_IDENTITY()", conware);
                                        InsertDetail.Parameters.Add(DetailKey);
                                        InsertDetail.ExecuteNonQuery();
                                        int DetailId = int.Parse(DetailKey.Value.ToString());

                                        if (Class_BasicOperation._WareType)
                                        {
                                            SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO]  " : " [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Ware, conware);
                                            DataTable TurnTable = new DataTable();
                                            Adapter.Fill(TurnTable);
                                            TurnTable.DefaultView.RowFilter = "Kind=2 and DTotalPrice<>0";
                                            if (TurnTable.Rows.Count > 0 && TurnTable.DefaultView.Count > 0)
                                            {
                                                int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", "Kind=2 and DTotalPrice<>0").ToString());
                                                DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4)
                                                    + " , Column21=" + Math.Round(double.Parse(SelectedRow[0]["DsinglePrice"].ToString()), 4) * Convert.ToDouble(item["Column07"].ToString())
                                                    + " where ColumnId=" + DetailId, conware);
                                                UpdateCommand.ExecuteNonQuery();
                                            }
                                        }
                                        else
                                        {
                                            SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + Ware + ",@Date='" + Row["Column02"].ToString() + "',@id=0,@residid=" + ResidId, conware);
                                            DataTable TurnTable = new DataTable();
                                            Adapter.Fill(TurnTable);
                                            if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                            {
                                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                        + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + DetailId, conware);
                                                UpdateCommand.ExecuteNonQuery();
                                            }
                                            else
                                            {
                                                Adapter = new SqlDataAdapter(@"SELECT  [dbo].[Table_008_Child_PwhrsDraft].column15
                                                                                                FROM    [dbo].[Table_008_Child_PwhrsDraft]
                                                                                                       JOIN  [dbo].Table_007_PwhrsDraft tpd
                                                                                                            ON  tpd.columnid =  [dbo].[Table_008_Child_PwhrsDraft].column01
                                                                                                WHERE  tpd.column02 <= '" + Row["Column02"].ToString() + @"'
                                                                                                       AND tpd.column03 = " + Ware + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column02 = " + item["Column02"].ToString() + @"
                                                                                                       AND  [dbo].[Table_008_Child_PwhrsDraft].column15>0
                                                                                                ORDER BY tpd.column02 desc", conware);
                                                TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                if (TurnTable.Rows.Count > 0)
                                                {
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4)
                                      + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["column15"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + DetailId, conware);
                                                    UpdateCommand.ExecuteNonQuery();
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                            #endregion


                            //درج شماره رسید در فاکتور مرجوعی
                            gridEX1.DropDowns["Receipt"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"), "");
                            Row["Column09"] = ResidId;
                            this.table_018_MarjooiSaleBindingSource.EndEdit();
                            this.table_018_MarjooiSaleTableAdapter.Update(dataSet_Sale.Table_018_MarjooiSale);

                            table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                            Class_BasicOperation.ShowMsg("", "ثبت رسید انبار با موفقیت انجام شد", "Information");
                        }
                    }
                }

                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                gridEX1.DropDowns["Receipt"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_011_PwhrsReceipt"), "");
                gridEX1.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                dataSet_Sale.EnforceConstraints = false;
                this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, int.Parse(RowID));
                this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowID));
                this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowID));
                dataSet_Sale.EnforceConstraints = true;
                this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
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
            //bt_DelDoc_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F8)

                mnu_Operations.ShowDropDown();
        }

        private void mnu_Drafts_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 20))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form03_WareReceipt")
                    {
                        ((PWHRS._03_AmaliyatAnbar.Form03_WareReceipt)item).txt_Search.Text =
                            (gridEX1.GetRow().Cells["Column09"].Text.Trim() != "" ?
                            gridEX1.GetValue("Column09").ToString() : "0");
                        item.BringToFront();
                        return;
                    }
                }
                PWHRS._03_AmaliyatAnbar.Form03_WareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form03_WareReceipt
                    (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
                    (this.table_018_MarjooiSaleBindingSource.Count > 0 ? int.Parse(((DataRowView)
                    this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column09"].ToString()) : -1));
                frm.ShowDialog();
                int ReturnSaleId = int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                dataSet_Sale.EnforceConstraints = false;
                this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_Sale.Table_018_MarjooiSale, ReturnSaleId);
                this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_Sale.Table_019_Child1_MarjooiSale, ReturnSaleId);
                this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_Sale.Table_020_Child2_MarjooiSale, ReturnSaleId);
                dataSet_Sale.EnforceConstraints = true;
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ViewSaleFactors_Click(object sender, EventArgs e)
        {
            try
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 67))
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_008_ViewSaleFactors")
                        {
                            child.Focus();
                            return;
                        }
                    }
                    _05_Sale.Frm_008_ViewSaleFactors frm = new _05_Sale.Frm_008_ViewSaleFactors();
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
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 130))
                {
                    if (this.table_018_MarjooiSaleBindingSource.Count > 0)
                    {

                        if (!Convert.ToBoolean(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column27"])
                            &&
                         clDoc.ExScalar(Properties.Settings.Default.SALE, @" SELECT ISNULL(
                                                                       (select ISNULL(Column45,0) Column45 from Table_010_SaleFactor where columnid=" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column17"].ToString() + @"),
                                                                       0
                                                                   )") == "False")

                            SaveEvent1(null, null);
                        _05_Sale.Reports.Form_ReturnSaleFactorPrint frm = new Reports.Form_ReturnSaleFactorPrint(
                            int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.
                            CurrencyManager.Current)["Column01"].ToString()));
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
                    gridEX_List.SetValue("tedaddarkartoon", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                    gridEX_List.SetValue("tedaddarbaste", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                    gridEX_List.SetValue("column03", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                    gridEX_List.SetValue("column10", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePrice"].ToString());
                    gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");
                    //gridEX_List.SetValue("column09", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePackPrice"].ToString());
                    //gridEX_List.SetValue("column08", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SaleBoxPrice"].ToString());
                    //اضافه و تخفیف خطی
                    //if (!Class_BasicOperation.CalLinearDis(int.Parse(gridEX1.GetValue("Column03").ToString())))
                    //{
                    //    gridEX_List.SetValue("column16", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());
                    //    gridEX_List.SetValue("column18", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Extra"].ToString());
                    //}
                    //else
                    //{
                    //    double[] array = clDoc.LastLinearDiscount(int.Parse(gridEX1.GetValue("Column03").ToString()), int.Parse(gridEX_List.GetValue("Column02").ToString()));
                    //    gridEX_List.SetValue("column16", array[0]);
                    //    gridEX_List.SetValue("column18", array[1]);
                    //}

                    //gridEX_List.SetValue("Column34", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Weight"].ToString());
                }
                else if (e.Column.Key == "column14")
                {
                    object Value = gridEX_List.GetValue("Column14");
                    DataRowView Row = (DataRowView)gridEX_List.RootTable.Columns["Column14"].DropDown.FindItem(Value);
                    gridEX_List.SetValue("Column15", Row["Column02"]);
                }


                if (gridEX_List.GetRow().Cells["Column14"].Text.Trim() == "" && float.Parse(gridEX_List.GetRow().Cells["Column15"].Text.Trim()) == 0)
                {
                    if (gridEX1.GetRow().Cells["Column23"].Text.Trim() != "" &&
                          gridEX1.GetRow().Cells["Column24"].Text.Trim() != "")
                    {
                        gridEX_List.SetValue("Column14", gridEX1.GetValue("Column23").ToString());
                        gridEX_List.SetValue("Column15", gridEX1.GetValue("Column24").ToString());
                    }
                }

                //gridEX_List.SetValue("column07",
                //(Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarkartoon"))) +
                //(Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarbaste"))) +
                //Convert.ToDouble(gridEX_List.GetValue("column06")));
                if (e.Column.Key == "column03")
                {
                    gridEX_List.SetValue("column10",
                         ((DataRowView)gridEX_List.RootTable.Columns["column03"].DropDown.FindItem(gridEX_List.GetValue("column03")))["sale"].ToString());
                    //if (gridEX_List.GetRow().Cells["column06"].Text.Trim() != "")
                    //{
                    //    float h = clDoc.GetZarib(Convert.ToInt32(gridEX_List.GetValue("GoodCode")), Convert.ToInt16(gridEX_List.GetValue("column03")));
                    //    gridEX_List.SetValue("column07", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);
                    //    gridEX_List.SetValue("column06", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);

                    //}
                }

                if (e.Column.Key == "column06")
                {
                    //float h = clDoc.GetZarib(Convert.ToInt32(gridEX_List.GetValue("GoodCode")), Convert.ToInt16(gridEX_List.GetValue("column03")));

                    //gridEX_List.SetValue("column07", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);
                    //gridEX_List.SetValue("column06", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);
                    gridEX_List.SetValue("column07", gridEX_List.GetValue("column06"));


                }

                //محاسبه وزن کل
                //  gridEX_List.SetValue("Column35", Convert.ToDouble(gridEX_List.GetValue("Column07")) * Convert.ToDouble(gridEX_List.GetValue("Column34")));


                Double TotalPrice = (gridEX1.GetValue("Column12").ToString() == "True" ?
                    (Convert.ToDouble(gridEX_List.GetValue("Column07").ToString()) *
                    Convert.ToDouble(gridEX_List.GetValue("column10"))) :
                        Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column07").ToString()) *
                     Convert.ToDouble(gridEX_List.GetValue("column10"))));
                gridEX_List.SetValue("Column11", TotalPrice);



                //if (e.Column.Key == "column17")
                //    gridEX_List.SetValue("Column16", 0);

                //if (e.Column.Key == "column16")
                //{
                //    if (gridEX1.GetValue("Column12").ToString() == "True")
                //        gridEX_List.SetValue("column17", Convert.ToDouble(gridEX_List.GetValue("column11")) * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                //    else
                //        gridEX_List.SetValue("column17", Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                //}

                //if (gridEX1.GetValue("Column12").ToString() == "True")
                //{
                //    Double jam, takhfif, ezafe;
                //    jam = Convert.ToDouble(gridEX_List.GetValue("column11"));
                //    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                //        takhfif = (jam * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                //    else takhfif = Convert.ToDouble(Convert.ToDouble(gridEX_List.GetValue("Column17").ToString()));
                //    if (Properties.Settings.Default.ExtraMethod)
                //        ezafe = Convert.ToInt64(((jam - takhfif) *
                //            (Convert.ToDouble(gridEX_List.GetValue("column18")) / 100)));
                //    else
                //        ezafe = Convert.ToInt64(((jam) *
                //      (Convert.ToDouble(gridEX_List.GetValue("column18")) / 100)));
                //    gridEX_List.SetValue("column17", takhfif);
                //    gridEX_List.SetValue("column19", ezafe);
                //    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);
                //}
                //else
                {
                    Int64 jam, takhfif, ezafe;
                    jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")));
                    //if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                    //    takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11"))
                    //        * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
                    //else takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column17").ToString()));
                    //if (Properties.Settings.Default.ExtraMethod)

                    //    ezafe = Convert.ToInt64((jam - takhfif) * Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);
                    //else
                    //    ezafe = Convert.ToInt64((jam) * Convert.ToDouble(gridEX_List.GetValue("column18")) / 100);
                    //gridEX_List.SetValue("column17", takhfif);
                    //gridEX_List.SetValue("column19", ezafe);
                    //gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);
                    gridEX_List.SetValue("column17", 0);
                    gridEX_List.SetValue("column19", 0);
                    gridEX_List.SetValue("column20", jam);
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
            try
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
                    darsad = Convert.ToDouble(gridEX_Extra.GetValue("Column03").ToString());
                    Double kol;
                    kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column20"].Value.ToString());
                    if (kol == 0)
                        return;
                    gridEX_Extra.SetValue("column04", kol * darsad / 100);
                }
            }
            catch { }
            try
            {
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch { }

        }

        private void bt_DefineSignatures_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 121))
            {
                _05_Sale.Frm_020_ReturnSale_Signatures frm = new Frm_020_ReturnSale_Signatures();
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
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column30");

                }
                catch { }

                if (e.Column.Key == "Column23")
                {
                    object Value = gridEX1.GetValue("Column23");
                    DataRowView Row = (DataRowView)this.gridEX1.RootTable.Columns["Column23"].DropDown.FindItem(Value);
                    gridEX1.SetValue("Column24", Row["Column02"]);
                    gridEX_List.RootTable.Columns["Column14"].DefaultValue = gridEX1.GetValue("Column23");
                    gridEX_List.RootTable.Columns["Column15"].DefaultValue = gridEX1.GetValue("Column24");
                }
                else if (e.Column.Key == "Column24")
                {
                    gridEX_List.RootTable.Columns["Column14"].DefaultValue = gridEX1.GetValue("Column23");
                    gridEX_List.RootTable.Columns["Column15"].DefaultValue = gridEX1.GetValue("Column24");
                }
                else if (e.Column.Key == "column05")
                {
                    try
                    {
                        double dd = 0;
                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
                        {
                            Con.Open();
                            SqlCommand Comm = new SqlCommand("SELECT tcc.Column143 FROM   Table_045_PersonInfo tcc WHERE  tcc.ColumnId=" + gridEX1.GetValue("column05") + " ", Con);
                            dd = Convert.ToDouble(Comm.ExecuteScalar());

                        }

                        gridEX1.SetValue("Column25", dd);
                    }
                    catch
                    {
                    }
                }
                if (e.Column.Key == "column12")
                {
                    if (gridEX1.GetValue("Column12").ToString() == "False")
                    {
                        gridEX1.SetValue("Column23", DBNull.Value);
                        gridEX1.SetValue("Column24", 0);
                    }
                }
            }
            catch { }
        }

        private void mnu_DelReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_018_MarjooiSaleBindingSource.Count > 0)
                {
                    int RowID = int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int ResidId = clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column09", RowID.ToString());

                    if (ResidId > 0)
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21))
                        {

                            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
                            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
                            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
                            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;

                            PWHRS._03_AmaliyatAnbar.Form03_WareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form03_WareReceipt
                            (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21), -1);
                            frm.txt_Search.Text = clDoc.ExScalar(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01", "ColumnId", ResidId.ToString());
                            frm.bt_Search_Click(sender, e);
                            frm.bt_Del_Click(sender, e);


                        }
                        else
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف رسید انبار را ندارید", "None");
                    }
                    dataSet_Sale.EnforceConstraints = false;
                    this.table_018_MarjooiSaleTableAdapter.Fill_ID(dataSet_Sale.Table_018_MarjooiSale, RowID);
                    this.table_019_Child1_MarjooiSaleTableAdapter.Fill(dataSet_Sale.Table_019_Child1_MarjooiSale, RowID);
                    this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(dataSet_Sale.Table_020_Child2_MarjooiSale, RowID);
                    dataSet_Sale.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);


                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bt_NotConfirmReceipt_Click(object sender, EventArgs e)
        {
            if (this.table_018_MarjooiSaleBindingSource.Count > 0)
            {
                int ReceiptId = clDoc.OperationalColumnValue("Table_018_MarjooiSale", "Column09", ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString());

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 69))
                {
                    string Message = null;

                    if (clDoc.ExScalar(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column19", "ColumnId", ReceiptId.ToString()) == "True")
                    {
                        Message = "آیا مایل به غیر قطعی کردن رسید انبار هستید؟";
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_011_PwhrsReceipt SET Column19=0 where ColumnId=" +
                               ReceiptId);
                            Class_BasicOperation.ShowMsg("", "غیرقطعی کردن رسید انبار با موفقیت انجام گرفت", "Information");
                        }

                    }
                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان غیر قطعی کردن رسید انبار را ندارید", "None");
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
                table_018_MarjooiSaleBindingSource.EndEdit();
                this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();

                if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                    dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        SaveEvent(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select min(Column01) from Table_018_MarjooiSale where  (Column30=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_018_MarjooiSale where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Sale.EnforceConstraints = false;
                    this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_Sale.EnforceConstraints = true;
                    this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (this.table_018_MarjooiSaleBindingSource.Count > 0)
            {
                try
                {
                    gridEX1.UpdateData();
                    table_018_MarjooiSaleBindingSource.EndEdit();
                    this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                    this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();

                    if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                        dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            SaveEvent(sender, e);
                        }
                    }


                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString,
                        "Select ISNULL((Select max(Column01) from Table_018_MarjooiSale where Column01<" +
                        ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString() + " and  (Column30=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_018_MarjooiSale where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_Sale.EnforceConstraints = true;
                        this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
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
            if (this.table_018_MarjooiSaleBindingSource.Count > 0)
            {

                try
                {
                    gridEX1.UpdateData();
                    table_018_MarjooiSaleBindingSource.EndEdit();
                    this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                    this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();

                    if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                        dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            SaveEvent(sender, e);
                        }
                    }

                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select Min(Column01) from Table_018_MarjooiSale where Column01>" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["Column01"].ToString() + " and  (Column30=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_018_MarjooiSale where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_Sale.EnforceConstraints = true;
                        this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
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
                table_018_MarjooiSaleBindingSource.EndEdit();
                this.table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();

                if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                    dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        SaveEvent(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select max(Column01) from Table_018_MarjooiSale where  (Column30=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_018_MarjooiSale where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Sale.EnforceConstraints = false;
                    this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_019_Child1_MarjooiSaleTableAdapter.Fill(this.dataSet_Sale.Table_019_Child1_MarjooiSale, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_Sale.EnforceConstraints = true;
                    this.table_018_MarjooiSaleBindingSource_PositionChanged(sender, e);
                }

            }
            catch
            {
            }
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
                table_018_MarjooiSaleBindingSource.EndEdit();
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void txt_GoodCode_Leave(object sender, EventArgs e)
        {
            var culture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");
            var language = InputLanguage.FromCulture(culture);
            InputLanguage.CurrentInputLanguage = language;
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

        private void txt_GoodCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                InitialNewRow();
            }


            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
        }

        private void addnew()
        {
            txt_Count.Text = "1";
            txt_GoodCode.Text = string.Empty;
            txt_GoodCode.Focus();
            txt_GoodCode.SelectAll();

        }

        private void InitialNewRow()
        {
            try
            {
                bool isthere = false;
                if (txt_GoodCode.Text != string.Empty)
                {



                    string Codegood = clDoc.ExScalar(ConWare.ConnectionString, @"select isNull((select Columnid from table_004_CommodityAndIngredients where Column06='" + txt_GoodCode.Text + "'),0)");

                    if (Codegood != "0")
                    {

                        DataTable dtsale = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT        dbo.Table_011_Child1_SaleFactor.column02
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
WHERE        (dbo.Table_010_SaleFactor.columnid = " + gridEX1.GetValue("Column17") + ")");
                        bool num = false;
                        foreach (DataRow item in dtsale.Rows)
                        {

                            if (item["Column02"].ToString() != Codegood)
                            {
                                num = true;

                            }

                        }
                        if (num)
                        {
                            Class_BasicOperation.ShowMsg("", "این بارکد در فاکتور فروش مورد نظر موجود نمی باشد", "Warning");
                            this.Cursor = Cursors.Default;
                            return;

                        }

                    }
                

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
                            Int16 unit = 0;
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                            {
                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                                {
                                    Con.Open();
                                    SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                    goodcode = (Comm.ExecuteScalar().ToString());
                                    Comm = new SqlCommand("SELECT tcc.column07 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                    unit = Convert.ToInt16(Comm.ExecuteScalar());
                                }


                                if (goodcode == txt_GoodCode.Text && unit == Convert.ToInt16(item.Cells["column03"].Value))
                                {

                                    isthere = true;
                                    item.BeginEdit();
                                    //float h = clDoc.GetZarib(Convert.ToInt32(codeid), Convert.ToInt16(item.Cells["column03"].Value));
                                    //item.Cells["Column07"].Value = (float.Parse(this.txt_Count.Text) * h) + Convert.ToDouble(item.Cells["Column07"].Value);
                                    //item.Cells["Column06"].Value = (float.Parse(this.txt_Count.Text) * h) + Convert.ToDouble(item.Cells["Column06"].Value);

                                    item.Cells["Column07"].Value = Convert.ToInt32(item.Cells["Column07"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);
                                    item.Cells["Column06"].Value = Convert.ToInt32(item.Cells["Column06"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);

                                    item.EndEdit();
                                    double TotalPrice;
                                    if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(item.Cells["Column07"].Value.ToString());
                                        item.BeginEdit();
                                        item.Cells["column11"].Value = TotalPrice;
                                        item.Cells["column16"].Value = 0;
                                        item.Cells["column18"].Value = 0;
                                        item.Cells["column17"].Value = 0;
                                        item.Cells["Column19"].Value = 0;
                                        item.Cells["Column20"].Value = TotalPrice;

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
                                    gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                    gridEX_List.SetValue("column03",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                    gridEX_List.SetValue("column16",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());



                                    //DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                    //this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                    //gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                    //double amunt = 0;
                                    //if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                    //{
                                    //    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                    //    if (dr.Count() > 0)
                                    //    {
                                    //        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                    //        gridEX_List.SetValue("column10",
                                    //         dr[0].ItemArray[3]);
                                    //    }
                                    //}

                                    //if (amunt == Convert.ToDouble(0))
                                    //{
                                    gridEX_List.SetValue("column10",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                    "SalePrice"].ToString());

                                    //}
                                    gridEX_List.SetValue("column09",
                                          0);
                                    gridEX_List.SetValue("column08",
                                       0);

                                    double TotalPrice;
                                    if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                        gridEX_List.SetValue("column11", TotalPrice);
                                        gridEX_List.SetValue("Column16", 0);
                                        gridEX_List.SetValue("Column18", 0);
                                        gridEX_List.SetValue("column17", 0);
                                        gridEX_List.SetValue("Column19", 0);

                                        gridEX_List.SetValue("Column20", TotalPrice);
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
                                    gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                    gridEX_List.SetValue("column03",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                    gridEX_List.SetValue("column16",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());

                                    //DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                    //this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                    //gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                    //double amunt = 0;
                                    //if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                    //{
                                    //    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                    //    if (dr.Count() > 0)
                                    //    {
                                    //        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                    //        gridEX_List.SetValue("column10",
                                    //         dr[0].ItemArray[3]);
                                    //    }
                                    //}

                                    //if (amunt == Convert.ToDouble(0))
                                    //{
                                    gridEX_List.SetValue("column10",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                    "SalePrice"].ToString());

                                    //}
                                    gridEX_List.SetValue("column09",
                                          0);
                                    gridEX_List.SetValue("column08",
                                       0);
                                    double TotalPrice;
                                    if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                        gridEX_List.SetValue("column11", TotalPrice);
                                        gridEX_List.SetValue("Column16", 0);
                                        gridEX_List.SetValue("Column18", 0);
                                        gridEX_List.SetValue("column17", 0);
                                        gridEX_List.SetValue("Column19", 0);

                                        gridEX_List.SetValue("Column20", TotalPrice);
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

        private void gridEX_List_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (
                    //((Janus.Windows.GridEX.GridEX)(sender)).Col == 5 &&

                 gridEX_List.GetValue("GoodCode") != null)
                {
                    gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                }

            }
            catch { }
        }

        private void chehckessentioal(string date)
        {

            discountdt = new DataTable();
            taxdt = new DataTable();
            factordt = new DataTable();
            //waredt = new DataTable();
            //bahaDT = new DataTable();



            /* if (Class_BasicOperation._FinType  && !Convert.ToBoolean(storefactor.Rows[0]["stock"]) )///سیستم دائمی
             {
                 SqlDataAdapter Adapter1 = new SqlDataAdapter(
                                                                        @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                     Column14, Column15, Column16
                                                                         FROM            Table_105_SystemTransactionInfo
                                                                         WHERE        (Column00 = 14) ", ConBase);
                 Adapter1.Fill(bahaDT);


                 using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                 {
                     Con.Open();
                     SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                    SELECT *
                                                                    FROM   AllHeaders()
                                                                    WHERE  ACC_Code = '" + bahaDT.Rows[0]["Column13"].ToString() + @"'
                                                                )
                                                                 SELECT 1 AS ok
                                                             ELSE
                                                                 SELECT 0 AS ok", Con);
                     if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                         throw new Exception("شماره حساب معتبر برای بهای تمام شده را در تنظیمات فروش وارد کنید");


                 }



                 using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                 {
                     Con.Open();
                     SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                    SELECT *
                                                                    FROM   AllHeaders()
                                                                    WHERE  ACC_Code = '" + bahaDT.Rows[0]["Column07"].ToString() + @"'
                                                                )
                                                                 SELECT 1 AS ok
                                                             ELSE
                                                                 SELECT 0 AS ok", Con);
                     if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                         throw new Exception("شماره حساب معتبر برای بهای تمام شده را در تنظیمات فروش وارد کنید");
                 }

             }*/


            SqlDataAdapter Adapter = new SqlDataAdapter(
                                               @"SELECT       isnull( column10,'') as column10 , isnull(column16,'') as column16
                                                                    FROM            Table_024_Discount
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
                        throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");

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
                        throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");
                }


            }


            Adapter = new SqlDataAdapter(
                                                                   @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                    Column14, Column15, Column16
                                                                        FROM            Table_105_SystemTransactionInfo
                                                                        WHERE        (Column00 = 12) ", ConBase);
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



            //            if (waredt.Rows.Count == 2)
            //            {
            //                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            //                {
            //                    Con.Open();
            //                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
            //                                                                   SELECT *
            //                                                                   FROM   table_005_PwhrsOperation
            //                                                                   WHERE  columnid = " + waredt.Rows[0]["Column02"] + @"
            //                                                               )
            //                                                                SELECT 1 AS ok
            //                                                            ELSE
            //                                                                SELECT 0 AS ok", Con);
            //                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
            //                        throw new Exception("عملکرد انتخاب نشده است");
            //                }



            //                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            //                {
            //                    Con.Open();
            //                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
            //                                                                   SELECT *
            //                                                                   FROM   Table_001_PWHRS
            //                                                                   WHERE  columnid = " + waredt.Rows[1]["Column02"] + @"
            //                                                               )
            //                                                                SELECT 1 AS ok
            //                                                            ELSE
            //                                                                SELECT 0 AS ok", Con);
            //                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
            //                        throw new Exception("انبار انتخاب نشده است");
            //                }

            //            }
            //            else
            //                throw new Exception("انبار و عملرکد تعریف نشده است");
            if (((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
            {
                LastDocnum = LastDocNum(date);
                if (LastDocnum > 0)
                    clDoc.IsFinal(LastDocnum);
            }
            else if (Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column10"]) > 0)
                LastDocnum = Convert.ToInt32(clDoc.ExScalarQuery(Properties.Settings.Default.ACNT, @"SELECT ISNULL(
                                                                                                            (select Column00 from Table_060_SanadHead where ColumnId=" + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column10"] + @"),0) AS column01"));





            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(date);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

        }
        private void checksanad()
        {
            Sanaddt = new DataTable();
            // bahaDT = new DataTable();

            SqlDataAdapter Adapter = new SqlDataAdapter(@"
SELECT FactorTable.columnid,
       FactorTable.column01,
       FactorTable.date,
       ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
       ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
       FactorTable.Bed,
       FactorTable.Bes,
       FactorTable.NetTotal,FactorTable.Column30
FROM   (
           SELECT dbo.Table_018_MarjooiSale.columnid,
                  dbo.Table_018_MarjooiSale.column01,
                  dbo.Table_018_MarjooiSale.column02 AS Date,
                  OtherPrice.PlusPrice AS Ezafat,
                  OtherPrice.MinusPrice AS Kosoorat,
                  OtherPrice.Bed,
                  OtherPrice.Bes,
                  dbo.Table_018_MarjooiSale.Column18 AS NetTotal,
                  dbo.Table_018_MarjooiSale.Column30

           FROM   dbo.Table_018_MarjooiSale
                  
                  LEFT OUTER JOIN (
                           SELECT columnid,
                                  SUM(PlusPrice) AS PlusPrice,
                                  SUM(MinusPrice) AS MinusPrice,
                                  Bed,
                                  Bes
                           FROM   (
                                      SELECT Table_018_MarjooiSale_2.columnid,
                                             SUM(dbo.Table_020_Child2_MarjooiSale.column04) AS 
                                             PlusPrice,
                                             0 AS MinusPrice,
                                             td.column10 AS Bes,
                                             td.column16 AS Bed
                                      FROM   dbo.Table_020_Child2_MarjooiSale
                                             JOIN Table_024_Discount td
                                                  ON  td.columnid = dbo.Table_020_Child2_MarjooiSale.column02
                                             INNER JOIN dbo.Table_018_MarjooiSale AS 
                                                  Table_018_MarjooiSale_2
                                                  ON  dbo.Table_020_Child2_MarjooiSale.column01 = 
                                                      Table_018_MarjooiSale_2.columnid
                                      WHERE  (dbo.Table_020_Child2_MarjooiSale.column05 = 0)
                                      GROUP BY
                                             Table_018_MarjooiSale_2.columnid,
                                             dbo.Table_020_Child2_MarjooiSale.column05,
                                             td.column10,
                                             td.column16
                                      UNION ALL
                                      SELECT Table_018_MarjooiSale_1.columnid,
                                             0 AS PlusPrice,
                                             SUM(Table_020_Child2_MarjooiSale_1.column04) AS 
                                             MinusPrice,
                                             td.column10 AS Bes,
                                             td.column16 AS Bed
                                      FROM   dbo.Table_020_Child2_MarjooiSale AS 
                                             Table_020_Child2_MarjooiSale_1
                                             JOIN Table_024_Discount td
                                                  ON  td.columnid = 
                                                      Table_020_Child2_MarjooiSale_1.column02
                                             INNER JOIN dbo.Table_018_MarjooiSale AS 
                                                  Table_018_MarjooiSale_1
                                                  ON  
                                                      Table_020_Child2_MarjooiSale_1.column01 = 
                                                      Table_018_MarjooiSale_1.columnid
                                      WHERE  (Table_020_Child2_MarjooiSale_1.column05 = 1)
                                      GROUP BY
                                             Table_018_MarjooiSale_1.columnid,
                                             Table_020_Child2_MarjooiSale_1.column05,
                                             td.column10,
                                             td.column16
                                  ) AS OtherPrice_1
                           GROUP BY
                                  columnid,
                                  OtherPrice_1.Bed,
                                  OtherPrice_1.Bes
                       ) AS OtherPrice
                       ON  dbo.Table_018_MarjooiSale.columnid = OtherPrice.columnid
       ) AS FactorTable
                                        WHERE  FactorTable.columnid=" + int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + @"
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

            All_Controls_Row1(factordt.Rows[0]["Column07"].ToString(), null, null, ((Sanaddt.Rows[0]["Column30"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column30"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column30"]) : (Int16?)null)));
            All_Controls_Row1(factordt.Rows[0]["Column13"].ToString(), int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column03"].ToString()), null, ((Sanaddt.Rows[0]["Column30"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column30"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column30"]) : (Int16?)null)));
            TPerson.Rows.Add(Int32.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column03"].ToString()), factordt.Rows[0]["Column13"].ToString(), Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()));
            TAccounts.Rows.Add(factordt.Rows[0]["Column13"].ToString(), (-1 * Convert.ToDouble((Sanaddt.Rows[0]["NetTotal"]))));
            TAccounts.Rows.Add(factordt.Rows[0]["Column07"].ToString(), (Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"])));

            foreach (DataRow dr in Sanaddt.Rows)
            {


                if (Convert.ToDouble(dr["Ezafat"]) > 0)
                {
                    All_Controls_Row1(dr["Bed"].ToString(), null, null, ((Sanaddt.Rows[0]["Column30"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column30"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column30"]) : (Int16?)null)));
                    All_Controls_Row1(dr["Bes"].ToString(), int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column03"].ToString()), null, ((Sanaddt.Rows[0]["Column30"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column30"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column30"]) : (Int16?)null)));
                    TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Ezafat"])));
                    TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Ezafat"])));
                    TPerson.Rows.Add(Int32.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column03"].ToString()), dr["Bes"].ToString(), Convert.ToDouble(dr["Ezafat"]));


                }
                if (Convert.ToDouble(dr["Kosoorat"]) > 0)
                {
                    All_Controls_Row1(dr["Bes"].ToString(), null, null, ((Sanaddt.Rows[0]["Column30"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column30"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column30"]) : (Int16?)null)));
                    All_Controls_Row1(dr["Bed"].ToString(), int.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column03"].ToString()), null, ((Sanaddt.Rows[0]["Column30"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column30"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column30"]) : (Int16?)null)));
                    TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Kosoorat"])));
                    TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Kosoorat"])));
                    TPerson.Rows.Add(Int32.Parse(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column03"].ToString()), dr["Bed"].ToString(), Convert.ToDouble(dr["Kosoorat"]));


                }



            }
            //            if (Class_BasicOperation._FinType /* && !Convert.ToBoolean(storefactor.Rows[0]["stock"])*/)
            //            {
            //                SqlDataAdapter Adapter1 = new SqlDataAdapter(
            //                                                                       @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
            //                                                                                                    Column14, Column15, Column16
            //                                                                        FROM            Table_105_SystemTransactionInfo
            //                                                                        WHERE        (Column00 = 14) ", ConBase);
            //                Adapter1.Fill(bahaDT);
            //                All_Controls_Row1(bahaDT.Rows[0]["Column13"].ToString(), null, null, ((Sanaddt.Rows[0]["Column30"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column30"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column30"]) : (Int16?)null)));
            //                All_Controls_Row1(bahaDT.Rows[0]["Column07"].ToString(), null, null, ((Sanaddt.Rows[0]["Column30"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column30"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column30"]) : (Int16?)null)));

            //            }

            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);

        }

        public void All_Controls_Row1(string AccountCode, int? Person, Int16? Center, Int16? Project)
        {
            string m = "فاکتور شماره ی " + ((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["column01"].ToString() + "دخیره شد اما صدور سند به دلیل زیر با خطا مواجه شد" + Environment.NewLine;
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
        private float DeleteRemain(int GoodCode, string ware, string date, int id)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                string CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE      (dbo.Table_011_PwhrsReceipt.column03 = {0}) AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1}) AND dbo.Table_011_PwhrsReceipt.columnid!=" + id + @" 
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
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }


        private float TotalDeleteRemain(int GoodCode, string ware, int id)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                string CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE      (dbo.Table_011_PwhrsReceipt.column03 = {0}) AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1}) AND dbo.Table_011_PwhrsReceipt.columnid!=" + id + @" 
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE     (dbo.Table_007_PwhrsDraft.column03 = {0}) AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02) AS derivedtbl_1
                              ";
                CommandText = string.Format(CommandText, ware, GoodCode);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        private void gridEX_List_UpdatingCell(object sender, UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Column.Key == "column03")
                {


                    if (gridEX_List.GetRow().Cells["column06"].Text.Trim() != "")
                    {
                        float h = clDoc.GetZarib(Convert.ToInt32(gridEX_List.GetValue("GoodCode")), Convert.ToInt16(e.InitialValue), Convert.ToInt16(e.Value));
                        gridEX_List.SetValue("column07", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);
                        gridEX_List.SetValue("column06", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);

                    }
                }
            }
            catch { }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {

            p_tax.Visible = false;
            gridEX_Extra.CancelCurrentEdit();
            table_020_Child2_MarjooiSaleBindingSource.CancelEdit();
            this.table_020_Child2_MarjooiSaleTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_020_Child2_MarjooiSale, Convert.ToInt32(((DataRowView)this.table_018_MarjooiSaleBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
            Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
            txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
            Filter.Value1 = true;
            txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

            txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
            txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                {
                    if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                    {
                        item.BeginEdit();
                        item.Cells["Column04"].Value = Convert.ToInt64(Convert.ToDouble(
                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                        AggregateFunction.Sum).ToString()) * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100);
                        item.EndEdit();

                    }
                }
                gridEX_Extra.UpdateData();

                table_020_Child2_MarjooiSaleBindingSource.EndEdit();
                p_tax.Visible = false;

                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("ثبت اضافات و کسورات با خطا موجه شد. شرح خطا" + ex.Message);
            }
        }

        private void btn_addtax_Click(object sender, EventArgs e)
        {
            try
            {

                table_018_MarjooiSaleBindingSource.EndEdit();
                table_019_Child1_MarjooiSaleBindingSource.EndEdit();
                if (table_018_MarjooiSaleBindingSource.Count > 0 && table_019_Child1_MarjooiSaleBindingSource.Count > 0)
                {
                    p_tax.Visible = true;
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }

        }

        private void bt_AddExtraDiscounts_Click(object sender, EventArgs e)
        {
            if (gridEX_Extra.AllowAddNew == InheritableBoolean.True && this.table_018_MarjooiSaleBindingSource.Count > 0 && this.table_019_Child1_MarjooiSaleBindingSource.Count > 0)
            {
                try
                {
                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount");
                    foreach (DataRow item in Table.Rows)
                    {
                        this.table_020_Child2_MarjooiSaleBindingSource.AddNew();
                        DataRowView New = (DataRowView)this.table_020_Child2_MarjooiSaleBindingSource.CurrencyManager.Current;
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
                        this.table_020_Child2_MarjooiSaleBindingSource.EndEdit();
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }


    }
}
