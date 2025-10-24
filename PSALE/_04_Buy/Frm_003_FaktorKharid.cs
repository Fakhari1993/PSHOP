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
using DevComponents.DotNetBar;


namespace PSHOP._04_Buy
{
    public partial class Frm_003_FaktorKharid : Form
    {
        bool _del;
        int _ID = 0, ReturnId = 0, ReturnNum = 0, DraftID = 0, DraftNum = 0, _ResidID = 0;

        SqlParameter ReturnDocNum;
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_Discounts ClDiscount = new Classes.Class_Discounts();
        Class_UserScope UserScope = new Class_UserScope();
        DataSet DS = new DataSet();
        InputLanguage original;
        string ReturnDate = null;
        DataTable ChildResidTbl = new DataTable();
        DataTable ResidHeaderTbl = new DataTable();
        bool _FromRecipt = false;
        SqlCommand _UpdatePriceInReceipt;
        double _arzeshvahed = 0;
        DataTable discountdt = new DataTable();
        DataTable taxdt = new DataTable();
        DataTable factordt = new DataTable();
        DataTable waredt = new DataTable();
        DataTable Sanaddt = new DataTable();
        int LastDocnum = 0;
        bool ExtraMethod = false;
        Int16 projectId;
        DataTable storefactor = new DataTable();
        bool Isadmin = false;


        public Frm_003_FaktorKharid(bool del, int ID)
        {
            _del = del;
            InitializeComponent();
            _ID = ID;

        }
        public Frm_003_FaktorKharid(bool del, int ResidID, bool FromRecipt)
        {

            InitializeComponent();
            _ResidID = ResidID;
            _del = del;
            _FromRecipt = true;


        }
        private void Frm_003_FaktorKharid_Load(object sender, EventArgs e)
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

            ReturnDocNum = new SqlParameter("ReturnDocNum", SqlDbType.Int);
            ReturnDocNum.Direction = ParameterDirection.Output;
            ToastNotification.ToastBackColor = Color.Aquamarine;
            ToastNotification.ToastForeColor = Color.Black;


            foreach (GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
                if (col.Key == "Column05" || col.Key == "Column07")
                    col.DefaultValue = Class_BasicOperation._UserName;
                if (col.Key == "Column06" || col.Key == "Column08")
                    col.DefaultValue = Class_BasicOperation.ServerDate();
            }



            string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + StoreTable.Rows[0]["Column05"] + "),0)");


            if (controlremain=="True")
            {
                GoodbindingSource.DataSource = clGood.MahsoolInfo(0);
                DataTable GoodTable = clGood.MahsoolInfo(0);
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

            gridEX1.DropDowns["project"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from [Table_035_ProjectInfo]"), "");
            gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from ListPeople(5)"), "");
            gridEX1.DropDowns["SaleType"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes");
            gridEX1.DropDowns["WHRS"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_001_PWHRS"), "");
            Adapter = new SqlDataAdapter("SELECT * FROM Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");






            gridEX1.DropDowns["Buyer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,4)"), "");


            Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount_Buy", ConSale);
            Adapter.Fill(DS, "Discount");
            gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");

            gridEX_List.DropDowns["Factor"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select Columnid,Column01 from Table_023_RequestBuy"), "");

            Adapter = new SqlDataAdapter(
                                                                  @"SELECT        isnull(Column02,0) as Column02
                                                                                    FROM           Table_030_Setting
                                                                                    WHERE        (ColumnId in (49,46)) order by ColumnId desc   ", ConBase);
            Adapter.Fill(waredt);
            if (_FromRecipt)
            {
                ResidHeaderTbl = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_011_PwhrsReceipt where ColumnId=" + _ResidID);
                ChildResidTbl = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_012_Child_PwhrsReceipt where Column01=" + _ResidID);
                this.table_015_BuyFactorBindingSource.AddNew();

                DataRowView HeaderRow = (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;
                HeaderRow["Column02"] = FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00");
                HeaderRow["Column03"] = ResidHeaderTbl.Rows[0]["Column05"].ToString();
                HeaderRow["Column04"] = "فاکتور خرید صادره از رسید شماره " + ResidHeaderTbl.Rows[0]["Column01"].ToString() + " به تاریخ " + ResidHeaderTbl.Rows[0]["Column02"].ToString();
                HeaderRow["Column05"] = Class_BasicOperation._UserName;
                HeaderRow["Column06"] = Class_BasicOperation.ServerDate();
                HeaderRow["Column07"] = Class_BasicOperation._UserName;
                HeaderRow["Column08"] = Class_BasicOperation.ServerDate();
                HeaderRow["Column09"] = 0;
                HeaderRow["Column10"] = _ResidID;
                HeaderRow["Column11"] = 0;
                HeaderRow["Column14"] = ResidHeaderTbl.Rows[0]["Column20"];
                HeaderRow["Column15"] = ResidHeaderTbl.Rows[0]["Column16"];
                HeaderRow["Column17"] = 0;
                HeaderRow["Column18"] = 0;
                HeaderRow["Column19"] = 0;
                HeaderRow["Column25"] = (ResidHeaderTbl.Rows[0]["Column17"].ToString().Trim() == "" ? "-1" : ResidHeaderTbl.Rows[0]["Column17"].ToString());
                HeaderRow["Column26"] = ResidHeaderTbl.Rows[0]["Column18"].ToString();
                HeaderRow["Column27"] = ResidHeaderTbl.Rows[0]["Column03"];
                HeaderRow["Column28"] = ResidHeaderTbl.Rows[0]["Column04"];
                HeaderRow["Column31"] = 0;

                this.table_015_BuyFactorBindingSource.EndEdit();
                double t = 0;
                foreach (DataRow item in ChildResidTbl.Rows)
                {
                    this.table_016_Child1_BuyFactorBindingSource.AddNew();
                    DataRowView Row = (DataRowView)this.table_016_Child1_BuyFactorBindingSource.CurrencyManager.Current;
                    Row["Column02"] = item["Column02"].ToString();
                    Row["Column03"] = item["Column03"].ToString();
                    Row["Column04"] = item["Column04"].ToString();
                    Row["Column05"] = item["Column05"].ToString();
                    Row["Column06"] = item["Column06"].ToString();
                    Row["Column07"] = item["Column07"].ToString();
                    //به درخواست اقای رکابدار به صورت پیشفرض از اخرین فاکتور خرید غیر صفر میخورد در غیر این صورت از رسید میخواند
                    DataTable Table = new DataTable();
                    Adapter = new SqlDataAdapter(@"SELECT top 1 tcbf.column08,
                                                                                   tcbf.column09,
                                                                                   tcbf.column10
                                                                            FROM   Table_016_Child1_BuyFactor tcbf
                                                                                   JOIN Table_015_BuyFactor tbf
                                                                                        ON  tbf.columnid = tcbf.column01
                                                                            WHERE  (tcbf.column08>0 or tcbf.column09>0 or tcbf.column10>0 )
                                                                                and   tcbf.column02=" + item["Column02"] + @" 
                                                                                and  tbf.column03 = " + ResidHeaderTbl.Rows[0]["Column05"] + @"
                                                                            ORDER BY
                                                                                   tbf.column02 DESC", ConSale);
                    Adapter.Fill(Table);
                    SqlCommand InsertDetail = new SqlCommand();
                    double TotalPrice = 0;
                    if (Table.Rows.Count > 0 && (Convert.ToDecimal(Table.Rows[0]["column08"]) > 0 ||
                                                 Convert.ToDecimal(Table.Rows[0]["column09"]) > 0 ||
                                                 Convert.ToDecimal(Table.Rows[0]["column10"]) > 0)
                                             && global ::PWHRS.Properties.Settings.Default.BuyFactorFromLastFactor
                        )
                    {

                        TotalPrice = (double.Parse(item["Column04"].ToString()) * double.Parse(Table.Rows[0]["column08"].ToString())) +
                                                (double.Parse(item["Column05"].ToString()) * double.Parse(Table.Rows[0]["column09"].ToString())) +
                                                (double.Parse(item["Column06"].ToString()) * double.Parse(Table.Rows[0]["column10"].ToString()));

                        Row["Column08"] = Table.Rows[0]["column08"];
                        Row["Column09"] = Table.Rows[0]["column09"];
                        Row["Column10"] = Table.Rows[0]["column10"];
                        Row["Column11"] = TotalPrice;
                    }
                    else
                    {
                        //TotalPrice = (double.Parse(item["Column04"].ToString()) * double.Parse(item["Column08"].ToString())) +
                        //          (double.Parse(item["Column05"].ToString()) * double.Parse(item["Column09"].ToString())) +
                        //          (double.Parse(item["Column06"].ToString()) * double.Parse(item["Column10"].ToString()));
                        Row["Column08"] = item["Column08"].ToString();
                        Row["Column09"] = item["Column09"].ToString();
                        Row["Column10"] = item["Column10"].ToString();


                        TotalPrice =

                           Convert.ToInt64(Convert.ToDouble(item["Column07"]) * Convert.ToDouble(item["Column10"]));


                        Row["Column11"] = TotalPrice;



                    }
                    Row["Column13"] = (item["Column25"].ToString().Trim() == "" ? (object)DBNull.Value : item["Column25"].ToString());
                    Row["Column14"] = item["Column26"].ToString();
                    Row["Column16"] = 0;
                    Row["Column17"] = 0;
                    Row["Column18"] = 0;
                    Row["Column19"] = 0;
                    Row["column20"] = TotalPrice;
                    t += TotalPrice;
                    Row["Column21"] = (item["Column13"].ToString().Trim() != "" ? item["Column13"].ToString().Trim() : (object)DBNull.Value);
                    Row["Column22"] = (item["Column14"].ToString().Trim() != "" ? item["Column14"].ToString() : (object)DBNull.Value);
                    Row["Column23"] = (item["Column12"].ToString().Trim() != "" ? item["Column12"].ToString().Trim() : (object)DBNull.Value);
                    Row["Column24"] = 0;
                    Row["Column25"] = item["columnid"];
                    Row["Column26"] = _ResidID;
                    Row["Column27"] = item["Column28"].ToString();
                    Row["Column28"] = 0;
                    Row["Column29"] = item["Column32"].ToString();
                    Row["Column30"] = item["Column33"].ToString();
                    Row["Column31"] = 100;
                    Row["Column32"] = (item["Column30"].ToString().Trim() == "" ? (object)DBNull.Value : item["Column30"].ToString());
                    Row["Column33"] = (item["Column31"].ToString().Trim() == "" ? (object)DBNull.Value : item["Column31"].ToString());
                    Row["Column34"] = item["Column34"].ToString();
                    Row["Column35"] = item["Column35"].ToString();
                    Row["Column36"] = (item["Column36"].ToString().Trim() == "" ? (object)DBNull.Value : item["Column36"].ToString());
                    Row["Column37"] = (item["Column37"].ToString().Trim() == "" ? (object)DBNull.Value : item["Column37"].ToString());
                    Row.EndEdit();
                    table_016_Child1_BuyFactorBindingSource.EndEdit();
                }
                gridEX_List.UpdateData();

                ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column20"] = t;
                /* Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                    AggregateFunction.Sum).ToString());*/
                txt_EndPrice.Value = t + Convert.ToDouble(txt_Extra.Value.ToString())
                       - Convert.ToDouble(txt_Reductions.Value.ToString());
            }

            if (_ID != 0)
            {
                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, _ID);
                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, _ID);
                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, _ID);
                table_015_BuyFactorBindingSource_PositionChanged(sender, e);

            }
            try
            {
                using (SqlConnection ConWare1 = new SqlConnection(Properties.Settings.Default.WHRS))
                {
                    ConWare1.Open();
                    SqlCommand Update = new SqlCommand(@"UPDATE Table_032_GoodPrice
                                                SET    [Column01] = REPLACE([Column01], NCHAR(1610), NCHAR(1740))

                                                UPDATE Table_032_GoodPrice
                                                SET    [Column01] = REPLACE([Column01], NCHAR(1603), NCHAR(1705))", ConWare1);
                    Update.ExecuteNonQuery();

                }
                using (SqlConnection Conbase1 = new SqlConnection(Properties.Settings.Default.BASE))
                {
                    Conbase1.Open();
                    SqlCommand Update1 = new SqlCommand(@"UPDATE Table_002_SalesTypes
                                                    SET    [Column02] = REPLACE([Column02], NCHAR(1610), NCHAR(1740))

                                                    UPDATE Table_002_SalesTypes
                                                    SET    [Column02] = REPLACE([Column02], NCHAR(1603), NCHAR(1705))", Conbase1);
                    Update1.ExecuteNonQuery();


                }


            }
            catch
            {
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

                gridEX1.RootTable.Columns["Column27"].Selectable = false;
                gridEX1.RootTable.Columns["Column29"].Selectable = false;


            }
            else
            {
                //mlt_Ware.ReadOnly = false;
                //mlt_project.ReadOnly = false;
                //mlt_PersonSale.ReadOnly = false;


                gridEX1.RootTable.Columns["Column27"].Selectable = true;
                gridEX1.RootTable.Columns["Column29"].Selectable = true;


            }
            this.WindowState = FormWindowState.Maximized;


        }

        private void table_015_BuyFactorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {

                if (this.table_015_BuyFactorBindingSource.Count > 0)
                {
                    DataRowView Row = (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;

                    if (Row["Column10"].ToString() != "0" && (string.IsNullOrWhiteSpace(Row["Column27"].ToString()) || string.IsNullOrWhiteSpace(Row["Column28"].ToString())))
                    {

                        DataTable dt = new DataTable();
                        SqlDataAdapter RecipttAdapter = new SqlDataAdapter("SELECT * from Table_011_PwhrsReceipt where columnid=" + Row["Column10"] + "", ConWare);
                        RecipttAdapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Row["Column27"] = dt.Rows[0]["column03"];
                            Row["Column28"] = dt.Rows[0]["column04"];
                        }
                        else
                        {
                            Class_BasicOperation.ShowMsg("", "رسید وجود ندارد", "Information");
                            return;

                        }
                    }






                    // //اگر برای فاکتور فقط رسید صادر شده باشد 
                    // if (Row["Column10"].ToString() != "0" && Row["Column11"].ToString() == "0")
                    // {
                    //     gridEX1.AllowEdit = InheritableBoolean.False;
                    //     gridEX1.Enabled = true;
                    //     gridEX_List.AllowAddNew = InheritableBoolean.False;
                    //     gridEX_List.AllowEdit = InheritableBoolean.True;
                    //     gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                    //     gridEX_Extra.AllowDelete = InheritableBoolean.True;
                    //     gridEX_List.AllowDelete = InheritableBoolean.False;
                    // }
                    //// در صورت اینکه فاکتور دارای سند باشد، یا مرجوعی باشد یا باطل شده باشد 
                    // else
                    if (/* Row["Column11"].ToString().Trim() != "0"
                            ||*/  Row["Column17"].ToString().Trim() != "False"
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
                        txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
                    }
                    catch
                    {
                    }
                    DraftID = 0;
                    DraftNum = 0;
                    ReturnId = 0;
                    ReturnNum = 0;
                    ReturnDate = null;

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
                dataSet_Buy.EnforceConstraints = false;
                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, 0);
                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, 0);
                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, 0);
                dataSet_Buy.EnforceConstraints = true;
                gridEX1.MoveToNewRecord();
                //gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_015_BuyFactor", "Column01").ToString());
                gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd"));
                gridEX1.SetValue("Column05", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column06", Class_BasicOperation.ServerDate());
                gridEX1.SetValue("Column07", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column08", Class_BasicOperation.ServerDate());

                // if (waredt.Rows[1]["Column02"] != null && !string.IsNullOrWhiteSpace(waredt.Rows[1]["Column02"].ToString()))
                //{
                //    gridEX1.SetValue("Column27", Convert.ToInt16(waredt.Rows[1]["Column02"]));

                if (
                  (storefactor.Rows[0]["project"] != DBNull.Value &&
                  storefactor.Rows[0]["project"] != null &&
                  !string.IsNullOrWhiteSpace(storefactor.Rows[0]["project"].ToString())))
                {
                    gridEX1.SetValue("Column27", Convert.ToInt16(storefactor.Rows[0]["ware"]));
                    gridEX1.SetValue("Column29", Convert.ToInt16(storefactor.Rows[0]["project"]));

                    string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + Convert.ToInt16(storefactor.Rows[0]["project"]) + "),0)");
                    if (controlremain=="True")
                    {
                        GoodbindingSource.DataSource = clGood.MahsoolInfo(Convert.ToInt16(storefactor.Rows[0]["ware"]));
                        DataTable GoodTable = clGood.MahsoolInfo(Convert.ToInt16(storefactor.Rows[0]["ware"]));
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

                //}

                gridEX1.RootTable.Columns["Column25"].Selectable = false;
                gridEX1.RootTable.Columns["Column26"].Selectable = false;
                //gridEX1.Select();
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


            if (this.table_015_BuyFactorBindingSource.Count > 0 &&
                gridEX_List.AllowEdit == InheritableBoolean.True &&
                gridEX1.GetRow().Cells["Column03"].Text.Trim() != "" &&
                gridEX1.GetRow().Cells["Column27"] != null && gridEX1.GetRow().Cells["Column27"].Text.Trim() != "")
            {

                gridEX_List.UpdateData();
                gridEX_Extra.UpdateData();
                int OldDraftNum = 0;
                DataRowView Row =
                    (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;

                //if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column31"]))
                //{
                //    Class_BasicOperation.ShowMsg("", "فاکتور تائید شده است امکان ذخیره اطلاعات وجود ندارد", "Stop");
                //    this.Cursor = Cursors.Default;

                //    return;

                //}


                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(Row["column02"].ToString().Substring(0, 4)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(5, 2)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(8, 2))))
                {

                    Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
                    this.Cursor = Cursors.Default;

                    return;

                }

                string Ware = gridEX1.GetRow().Cells["Column27"].Text;
                string Person = gridEX1.GetRow().Cells["Column03"].Text;
                if (Ware.All(char.IsDigit) || Person.All(char.IsDigit))
                {
                    MessageBox.Show("اطلاعات وارد شده نامعتبر است");
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
                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column10"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با قیمت صفر وجود دارد", "Warning");
                    this.Cursor = Cursors.Default;

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
                                                                          AND tuai.Column02 = " + Row["Column27"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                    if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0 && !_FromRecipt)
                        throw new Exception("برای صدور رسید به انبار انتخاب شده دسترسی ندارید");

                }
                if (!_FromRecipt)
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        if (!clGood.IsGoodInWare(Int16.Parse(Row["Column27"].ToString()),
                            int.Parse(item.Cells["column02"].Value.ToString())))
                            throw new Exception("کالای " + item.Cells["column02"].Text +
                                " در انبار انتخاب شده فعال نمی باشد");
                    }

                if (Row["Column01"].ToString().StartsWith("-"))
                {
                    gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString,
                        "Table_015_BuyFactor", "Column01").ToString());
                    this.table_015_BuyFactorBindingSource.EndEdit();
                }


                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                int DocId = clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column11", RowID);
                int DraftId = clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", RowID);
                this.Cursor = Cursors.WaitCursor;


                string command = string.Empty;
                Boolean ok = true;
                if (DocId > 0 && !_FromRecipt)
                {
                    clDoc.IsFinal_ID(DocId);
                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=19 and Column17=" + RowID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=19 and Column17=" + RowID;

                    command += "Update  " + ConSale.Database + ".dbo.Table_015_BuyFactor set  Column11=0  where   columnid=" + RowID;


                }
                if (DraftId > 0 && !_FromRecipt)
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
                    command += "Update     " + ConSale.Database + ".dbo.Table_015_BuyFactor set  Column10=0   where   columnid=" + RowID;



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


                    txt_TotalPrice.Value = Convert.ToDouble(
                  gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                  AggregateFunction.Sum).ToString());




                    Row["Column07"] = Class_BasicOperation._UserName;
                    Row["Column08"] = Class_BasicOperation.ServerDate();
                    Row["Column20"] = gridEX_List.GetTotal(
                        gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString();
                    Row["Column23"] = gridEX_List.GetTotal(
                        gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                    Row["Column24"] = gridEX_List.GetTotal(
                        gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                    //Extra-Reductions

                    double Total = double.Parse(txt_TotalPrice.Value.ToString());
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                    {
                        if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                        {
                            item.BeginEdit();
                            item.Cells["Column04"].Value = (gridEX1.GetValue("Column15").ToString().Trim() == "True" ?
                                Total * double.Parse(item.Cells["Column03"].Value.ToString()) / 100 :
                                Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100));
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
                    string saletyp = string.Empty;
                    if (gridEX1.GetRow().Cells["Column34"].Value != null && !string.IsNullOrWhiteSpace(gridEX1.GetRow().Cells["Column34"].Value.ToString()))
                        saletyp = gridEX1.GetRow().Cells["Column34"].Text.Trim();
                    this.table_015_BuyFactorBindingSource.EndEdit();
                    this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                    this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                    this.table_015_BuyFactorTableAdapter.Update(dataSet_Buy.Table_015_BuyFactor);
                    this.table_016_Child1_BuyFactorTableAdapter.Update(
                        dataSet_Buy.Table_016_Child1_BuyFactor);
                    this.table_017_Child2_BuyFactorTableAdapter.Update(
                        dataSet_Buy.Table_017_Child2_BuyFactor);
                    Row =
                   (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;

                    #region آپدیت قیمت فروش و خرید کالا
                    //در تاریخ 1399/12/20 به درخواست مشتری و مهندس در صورتی قیمت خرید و فروش کالا آپدیت میشه که تاریخ فاکتور با تاریخ روز برابر باشه
                    string buyprice = string.Empty;

                    if (Class_BasicOperation.ServerDate().Date == FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(gridEX1.GetRow().Cells["column02"].Text))
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                        {
                            try
                            {
                                if (item.Cells["Column38"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["Column38"].Value.ToString())
                                    && Convert.ToDouble(item.Cells["Column38"].Value) > 0)
                                {
                                    if (!string.IsNullOrWhiteSpace(saletyp))

                                        clDoc.RunSqlCommand(Properties.Settings.Default.WHRS, @" 
                                                                        IF EXISTS(
                                                                               SELECT *
                                                                               FROM   Table_032_GoodPrice tgp
                                                                               WHERE  Column00 = " + item.Cells["GoodCode"].Value + @"
                                                                                      AND Column01 = N'" + saletyp + @"'
                                                                           )
                                                                        BEGIN
                                                                            UPDATE Table_032_GoodPrice
                                                                            SET    Column02      = " + item.Cells["Column38"].Value + @"
                                                                            WHERE  Column00      = " + item.Cells["GoodCode"].Value + @"
                                                                                   AND Column01  = N'" + saletyp + @"'
                                                                        END
                                                                        ELSE
                                                                        BEGIN
                                                                            INSERT INTO .[Table_032_GoodPrice]
                                                                              (
                                                                                [Column00],
                                                                                [Column01],
                                                                                [Column02]
                                                                              )
                                                                            VALUES
                                                                              (
                                                                                " + item.Cells["GoodCode"].Value + @",
                                                                                N'" + saletyp + @"',
                                                                               " + item.Cells["Column38"].Value + @"
                                                                              )
                                                                        END");
                                    else
                                        clDoc.RunSqlCommand(Properties.Settings.Default.WHRS, "Update table_004_CommodityAndIngredients set Column34=" + item.Cells["Column38"].Value + " where columnid=" + item.Cells["GoodCode"].Value);
                                }
                            }
                            catch { }
                           decimal numberbuy =((Convert.ToDecimal(item.Cells["column11"].Value )- Convert.ToDecimal( item.Cells["column17"].Value)) / Convert.ToDecimal( item.Cells["column07"].Value));
                            buyprice += " Update " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients set Column35=" + numberbuy + " where columnid=" + item.Cells["GoodCode"].Value;
                        }

                    #endregion


                    if (!_FromRecipt)
                    {
                        checksanad();
                        string sanadcmd = string.Empty;
                        SqlParameter DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
                        DraftNum.Direction = ParameterDirection.Output;

                        SqlParameter DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                        DocNum.Direction = ParameterDirection.Output;
                        sanadcmd = " declare @DetialID int  declare @draftkey int     declare @DocID int " +
                            "set @DraftNum=" + (OldDraftNum > 0 ? OldDraftNum.ToString() : "( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt)") + @"";

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
                             Row["Column27"] + "," + waredt.Rows[0]["Column02"] + "," + Row["column03"] + ",'" + "رسید صادره بابت فاکتور خرید ش" +
                             Row["column01"].ToString() + " تاریخ " + Row["column02"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                            Row["Column09"] + "," + Row["ColumnId"]
                            + ",0,0,0,NULL,0,1,null); SET @draftkey=Scope_Identity()";



                        foreach (Janus.Windows.GridEX.GridEXRow ChildItem in gridEX_List.GetRows())
                        {
                            if (clDoc.IsGood(ChildItem.Cells["Column02"].Value.ToString()))
                            {
                                DataTable value = new DataTable();
                                SqlDataAdapter Adapter = new SqlDataAdapter(@"DECLARE @share  FLOAT,
                                                                            @sum    DECIMAL(18, 3),
                                                                            @Net    DECIMAL(18, 3)

                                                                    SET @sum = (
                                                                            SELECT SUM(ISNULL(tt.VE, 0))
                                                                            FROM   (
                                                                                       SELECT (
                                                                                                  CASE 
                                                                                                       WHEN tcbf.column05 = 0 THEN tcbf.column04
                                                                                                       ELSE ((-1) * tcbf.column04)
                                                                                                  END
                                                                                              ) AS VE
                                                                                       FROM   Table_017_Child2_BuyFactor tcbf
                                                                                              JOIN Table_024_Discount_Buy tdb
                                                                                                   ON  tdb.columnid = tcbf.column02
                                                                                       WHERE  tdb.Column18 = 1
                                                                                              AND tcbf.column01 = " + Row["ColumnId"] + @"
                                                                                   ) AS tt
                                                                        )

                                                                    SET @Net =isnull( (
                                                                            SELECT tbf.Column20
                                                                            FROM   Table_015_BuyFactor tbf
                                                                            WHERE  tbf.columnid = " + Row["ColumnId"] + @"
                                                                        ),0)
    
                                                                    SET @share = isnull(@sum /nullif( @Net,0),0)
                                                                    DECLARE @unitvalue   DECIMAL(18, 3),
                                                                            @totalvalue  DECIMAL(18, 3)

                                                                            SET @unitvalue =(CASE WHEN @share>0 then (
                                                                            ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column20)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + ChildItem.Cells["Column02"].Value + @"
                                                                                           AND tcbf.column01 = " + Row["ColumnId"] + @"
                                                                                ),
                                                                                0
                                                                            ) / nullif( ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column07)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + ChildItem.Cells["Column02"].Value + @"
                                                                                           AND tcbf.column01 = " + Row["ColumnId"] + @"
                                                                                ),
                                                                                0
                                                                            ),0)
                                                                        ) * (1 + @share) else isnull(" + Convert.ToDouble(ChildItem.Cells["Column20"].Value) + @" /nullif( " + Convert.ToDouble(ChildItem.Cells["Column07"].Value) + @",0),0) END)

                                                                    SET @totalvalue = @unitvalue * ISNULL(
                                                                            (
                                                                                SELECT SUM(tcbf.column07)
                                                                                FROM   Table_016_Child1_BuyFactor tcbf
                                                                                WHERE  tcbf.column02 = " + ChildItem.Cells["Column02"].Value + @"
                                                                                       AND tcbf.column01 = " + Row["ColumnId"] + @"
                                                                            ),
                                                                            0
                                                                        )  

                                                                    SELECT 1+@share AS share,
                                                                          isnull( @unitvalue,0) AS unitvalue,
                                                                         isnull(  @totalvalue,0) AS totalvalue

                                                                    ", ConSale);
                                Adapter.Fill(value);




                                sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt([column01]
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
           ,[Column35]) VALUES (@draftkey," + ChildItem.Cells["Column02"].Value.ToString() + "," +
                                                ChildItem.Cells["Column03"].Value.ToString() + "," + ChildItem.Cells["Column04"].Value.ToString() + "," + ChildItem.Cells["Column05"].Value.ToString() + "," + ChildItem.Cells["Column06"].Value.ToString() + "," + ChildItem.Cells["Column07"].Value.ToString() + "," +
                                                ChildItem.Cells["Column08"].Value.ToString() + "," + ChildItem.Cells["Column09"].Value.ToString() + "," + ChildItem.Cells["Column10"].Value.ToString() + "," + ChildItem.Cells["Column11"].Value.ToString() + ",NULL," +
                                                (ChildItem.Cells["Column21"].Value.ToString().Trim() == "" ? "NULL" : ChildItem.Cells["Column21"].Value.ToString()) + "," + (ChildItem.Cells["Column22"].Value.ToString().Trim() == "" ? "NULL" : ChildItem.Cells["Column22"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                                                + "',getdate(),0," + (Convert.ToDouble(ChildItem.Cells["Column07"].Value.ToString()) > 0 ? (Convert.ToDouble(ChildItem.Cells["Column20"].Value.ToString()) / Convert.ToDouble(ChildItem.Cells["Column07"].Value.ToString())) * Convert.ToDouble(value.Rows[0]["share"]) : 0) +
                                                "," + (Convert.ToDouble(ChildItem.Cells["Column20"].Value) * Convert.ToDouble(value.Rows[0]["share"])) + "," + ChildItem.Cells["ColumnId"].Value.ToString()
                                                + ",NULL,NULL,NULL,0,0,0,0," +
                                                (ChildItem.Cells["Column32"].Text.ToString().Trim() == "" ? "NULL" : "'" + ChildItem.Cells["Column32"].Value.ToString() + "'") + "," +
                                                (ChildItem.Cells["Column33"].Text.ToString().Trim() == "" ? "NULL" : "'" + ChildItem.Cells["Column33"].Value.ToString() + "'") + "," + ChildItem.Cells["Column29"].Value.ToString() +
                                                "," + ChildItem.Cells["Column30"].Value.ToString() + "," + ChildItem.Cells["Column34"].Value.ToString() + "," + ChildItem.Cells["Column35"].Value.ToString() + ")";


                            }
                        }
                        sanadcmd += "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor SET Column10=@draftkey  where ColumnId=" + Row["ColumnId"];

                        if (LastDocnum > 0)
                            sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                        else
                            sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + Row["column02"] + "',2,0,N'فاکتور خرید','" + Class_BasicOperation._UserName +
                       "',getdate()); SET @DocID=SCOPE_IDENTITY()";

                        string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());

                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               NULL, NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'فاکتور خرید " + Row["column01"].ToString() + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL); set @DetialID=SCOPE_IDENTITY()";


                        DataTable Child1 = new DataTable();
                        Child1 = clDoc.ReturnTable(ConSale.ConnectionString, @"select * from Table_016_Child1_BuyFactor where Column01=" + ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"] + "");
                        string detal = clDoc.ExScalar(ConBase.ConnectionString, @"select Column02 from Table_030_Setting where Columnid=73");

                        if (detal == "True")
                            foreach (DataRow citem1 in Child1.Rows)
                            {

                                sanadcmd += @" INSERT INTO Table_075_SanadDetailNotes (Column00,Column01,Column02,Column03,Column04) Values (@DetialID,1,(select Column02 from " + ConWare.Database + ".dbo.table_004_CommodityAndIngredients where ColumnId=" + citem1["Column02"].ToString() + " ) ," +
                                    citem1["Column07"].ToString() + "," + citem1["Column10"].ToString() + ")";

                            }

                        _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(Row["Column03"].ToString()) ? "NULL" : Row["Column03"].ToString()) + @", NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'فاکتور خرید " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                      

                  

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
                                " + (string.IsNullOrWhiteSpace(Row["Column03"].ToString()) ? "NULL" : Row["Column03"].ToString()) + @", NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'تخفیف فاکتور خرید ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                                sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               NULL, NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'تخفیف فاکتور خرید ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                                NULL, NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'ارزش افزوده فاکتور خرید ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                                sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(Row["Column03"].ToString()) ? "NULL" : Row["Column03"].ToString()) + @", NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'ارزش افزوده فاکتور خرید ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                            }


                        }

                        sanadcmd += "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor SET Column10=@draftkey, Column11=@DocID where ColumnId=" + Row["ColumnId"];
                        sanadcmd += " UPDATE " + this.ConWare.Database + ".dbo.Table_011_PwhrsReceipt SET  column07=@DocID where ColumnId= @draftkey";
                        sanadcmd += buyprice;
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
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();
                                dataSet_Buy.EnforceConstraints = false;

                                this.table_015_BuyFactorTableAdapter.Fill_New(dataSet_Buy.Table_015_BuyFactor, Convert.ToInt32(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"]));
                                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(dataSet_Buy.Table_016_Child1_BuyFactor, Convert.ToInt32(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"]));
                                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(dataSet_Buy.Table_017_Child2_BuyFactor, Convert.ToInt32(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"]));

                                dataSet_Buy.EnforceConstraints = true;

                                this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);
                                if (sender == bt_Save || sender == this)
                                    Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                                      "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره رسید انبار: " + DraftNum.Value, "Information");


                                if (DialogResult.Yes == MessageBox.Show("آیا مایل به چاپ فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                {

                                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 129))
                                    {
                                        // bt_Save_Click(sender, e);
                                        _04_Buy.Reports.Form_BuyFactorPrint frm = new _04_Buy.Reports.Form_BuyFactorPrint
                                            (int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()).ToString());
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


                    }



                    if (_FromRecipt)
                    {
                        //قرار دادن شماره فاکتور خرید در رسید انبار
                        clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_011_PwhrsReceipt SET  Column13=" +
                            Row["ColumnId"].ToString() + " where ColumnId=" + _ResidID);
                        if (sender == bt_Save || sender == this)
                            Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                    }




                    // bt_New.Enabled = true;
                }
            }
            else if (gridEX_List.AllowEdit == InheritableBoolean.False && sender != چاپبارکدToolStripMenuItem)
            {
                Class_BasicOperation.ShowMsg("", "امکان ثبت تغییرات وجود ندارد", "Stop");

                return;
            }
            //در هر صورت بعد از ذخیره اطلاعات دوباره اطلاعات به روز رسانی می شوند

            //int _ID = int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
            //dataSet_Buy.EnforceConstraints = false;
            //this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, _ID);
            //this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, _ID);
            //this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, _ID);
            //dataSet_Buy.EnforceConstraints = true;
            //table_015_BuyFactorBindingSource_PositionChanged(sender, e);


        }

        private void SaveEvent1(object sender, EventArgs e)
        {
            gridEX1.UpdateData();


            if (this.table_015_BuyFactorBindingSource.Count > 0 &&
                gridEX_List.AllowEdit == InheritableBoolean.True &&
                gridEX1.GetRow().Cells["Column03"].Text.Trim() != "" &&
                gridEX1.GetRow().Cells["Column27"] != null && gridEX1.GetRow().Cells["Column27"].Text.Trim() != "")
            {

                gridEX_List.UpdateData();
                gridEX_Extra.UpdateData();
                int OldDraftNum = 0;
                DataRowView Row =
                    (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;

                //if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column31"]))
                //{
                //    Class_BasicOperation.ShowMsg("", "فاکتور تائید شده است امکان ذخیره اطلاعات وجود ندارد", "Stop");
                //    this.Cursor = Cursors.Default;

                //    return;

                //}


                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(Row["column02"].ToString().Substring(0, 4)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(5, 2)),
                 Convert.ToInt32(Row["column02"].ToString().Substring(8, 2))))
                {

                    //Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
                    //this.Cursor = Cursors.Default;

                    //return;
                    throw new Exception("تاریخ معتبر نیست");

                }

                string Ware = gridEX1.GetRow().Cells["Column27"].Text;
                string Person = gridEX1.GetRow().Cells["Column03"].Text;
                if (Ware.All(char.IsDigit) || Person.All(char.IsDigit))
                {
                    throw new Exception("اطلاعات وارد شده نامعتبر است");

                    //MessageBox.Show("اطلاعات وارد شده نامعتبر است");
                    //return;
                }
                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    //Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    //this.Cursor = Cursors.Default;

                    //return;
                    throw new Exception("کالایی ثبت نشده است");

                }

                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column07"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    //Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد", "Warning");
                    //this.Cursor = Cursors.Default;

                    //return;
                    throw new Exception("در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد");

                }
                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column10"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    //Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با قیمت صفر وجود دارد", "Warning");
                    //this.Cursor = Cursors.Default;

                    //return;
                    throw new Exception("در میان کالاهای وارد شده کالایی با قیمت صفر وجود دارد");

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
                                                                          AND tuai.Column02 = " + Row["Column27"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                    if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0 && !_FromRecipt)
                        throw new Exception("برای صدور رسید به انبار انتخاب شده دسترسی ندارید");

                }
                if (!_FromRecipt)
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        if (!clGood.IsGoodInWare(Int16.Parse(Row["Column27"].ToString()),
                            int.Parse(item.Cells["column02"].Value.ToString())))
                            throw new Exception("کالای " + item.Cells["column02"].Text +
                                " در انبار انتخاب شده فعال نمی باشد");
                    }

                if (Row["Column01"].ToString().StartsWith("-"))
                {
                    gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString,
                        "Table_015_BuyFactor", "Column01").ToString());
                    this.table_015_BuyFactorBindingSource.EndEdit();
                }


                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                int DocId = clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column11", RowID);
                int DraftId = clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", RowID);
                this.Cursor = Cursors.WaitCursor;


                string command = string.Empty;
                Boolean ok = true;
                if (DocId > 0 && !_FromRecipt)
                {
                    clDoc.IsFinal_ID(DocId);
                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=19 and Column17=" + RowID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=19 and Column17=" + RowID;

                    command += "Update     " + ConSale.Database + ".dbo.Table_015_BuyFactor set  Column11=0  where   columnid=" + RowID;


                }
                if (DraftId > 0 && !_FromRecipt)
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
                    command += "Update     " + ConSale.Database + ".dbo.Table_015_BuyFactor set  Column10=0   where   columnid=" + RowID;



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


                    txt_TotalPrice.Value = Convert.ToDouble(
                  gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                  AggregateFunction.Sum).ToString());




                    Row["Column07"] = Class_BasicOperation._UserName;
                    Row["Column08"] = Class_BasicOperation.ServerDate();
                    Row["Column20"] = gridEX_List.GetTotal(
                        gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString();
                    Row["Column23"] = gridEX_List.GetTotal(
                        gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                    Row["Column24"] = gridEX_List.GetTotal(
                        gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                    //Extra-Reductions

                    double Total = double.Parse(txt_TotalPrice.Value.ToString());
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                    {
                        if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                        {
                            item.BeginEdit();
                            item.Cells["Column04"].Value = (gridEX1.GetValue("Column15").ToString().Trim() == "True" ?
                                Total * double.Parse(item.Cells["Column03"].Value.ToString()) / 100 :
                                Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100));
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
                        //Class_BasicOperation.ShowMsg("", "جمع کل فاکتور منفی شده است", "Warning");
                        //this.Cursor = Cursors.Default;

                        //return;
                        throw new Exception("جمع کل فاکتور منفی شده است");

                    }
                    string saletyp = string.Empty;
                    if (gridEX1.GetRow().Cells["Column34"].Value != null && !string.IsNullOrWhiteSpace(gridEX1.GetRow().Cells["Column34"].Value.ToString()))
                        saletyp = gridEX1.GetRow().Cells["Column34"].Text.Trim();
                    this.table_015_BuyFactorBindingSource.EndEdit();
                    this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                    this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                    this.table_015_BuyFactorTableAdapter.Update(dataSet_Buy.Table_015_BuyFactor);
                    this.table_016_Child1_BuyFactorTableAdapter.Update(
                        dataSet_Buy.Table_016_Child1_BuyFactor);
                    this.table_017_Child2_BuyFactorTableAdapter.Update(
                        dataSet_Buy.Table_017_Child2_BuyFactor);

                    #region آپدیت قیمت فروش و خرید کالا
                    //در تاریخ 1399/12/20 به درخواست مشتری و مهندس در صورتی قیمت خرید و فروش کالا آپدیت میشه که تاریخ فاکتور با تاریخ روز برابر باشه

                    string buyprice = string.Empty;
                    if (Class_BasicOperation.ServerDate().Date == FarsiLibrary.Utils.PersianDateConverter.ToGregorianDateTime(gridEX1.GetRow().Cells["column02"].Text))
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                        {
                            try
                            {
                                if (item.Cells["Column38"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["Column38"].Value.ToString())
                                    && Convert.ToDouble(item.Cells["Column38"].Value) > 0)
                                {
                                    if (!string.IsNullOrWhiteSpace(saletyp))

                                        clDoc.RunSqlCommand(Properties.Settings.Default.WHRS, @" 
                                                                        IF EXISTS(
                                                                               SELECT *
                                                                               FROM   Table_032_GoodPrice tgp
                                                                               WHERE  Column00 = " + item.Cells["GoodCode"].Value + @"
                                                                                      AND Column01 = N'" + saletyp + @"'
                                                                           )
                                                                        BEGIN
                                                                            UPDATE Table_032_GoodPrice
                                                                            SET    Column02      = " + item.Cells["Column38"].Value + @"
                                                                            WHERE  Column00      = " + item.Cells["GoodCode"].Value + @"
                                                                                   AND Column01  = N'" + saletyp + @"'
                                                                        END
                                                                        ELSE
                                                                        BEGIN
                                                                            INSERT INTO .[Table_032_GoodPrice]
                                                                              (
                                                                                [Column00],
                                                                                [Column01],
                                                                                [Column02]
                                                                              )
                                                                            VALUES
                                                                              (
                                                                                " + item.Cells["GoodCode"].Value + @",
                                                                                N'" + saletyp + @"',
                                                                               " + item.Cells["Column38"].Value + @"
                                                                              )
                                                                        END");
                                    else
                                        clDoc.RunSqlCommand(Properties.Settings.Default.WHRS, "Update table_004_CommodityAndIngredients set Column34=" + item.Cells["Column38"].Value + " where columnid=" + item.Cells["GoodCode"].Value);
                                }
                            }
                            catch { }
                            buyprice += " Update " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients set Column35=" + item.Cells["column10"].Value + " where columnid=" + item.Cells["GoodCode"].Value;
                        }

                    #endregion


                    if (!_FromRecipt)
                    {
                        checksanad();
                        string sanadcmd = string.Empty;
                        SqlParameter DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
                        DraftNum.Direction = ParameterDirection.Output;

                        SqlParameter DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                        DocNum.Direction = ParameterDirection.Output;
                        sanadcmd = "   declare @draftkey int declare @DocID int set @DraftNum=" + (OldDraftNum > 0 ? OldDraftNum.ToString() : "( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt)") + @"";
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
                             Row["Column27"] + "," + waredt.Rows[0]["Column02"] + "," + Row["column03"] + ",'" + "رسید صادره بابت فاکتور خرید ش" +
                             Row["column01"].ToString() + " تاریخ " + Row["column02"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                            Row["Column09"] + "," + Row["ColumnId"]
                            + ",0,0,0,NULL,0,1,null); SET @draftkey=Scope_Identity()";



                        foreach (Janus.Windows.GridEX.GridEXRow ChildItem in gridEX_List.GetRows())
                        {
                            if (clDoc.IsGood(ChildItem.Cells["Column02"].Value.ToString()))
                            {
                                DataTable value = new DataTable();
                                SqlDataAdapter Adapter = new SqlDataAdapter(@"DECLARE @share  FLOAT,
                                                                            @sum    DECIMAL(18, 3),
                                                                            @Net    DECIMAL(18, 3)

                                                                    SET @sum = (
                                                                            SELECT SUM(ISNULL(tt.VE, 0))
                                                                            FROM   (
                                                                                       SELECT (
                                                                                                  CASE 
                                                                                                       WHEN tcbf.column05 = 0 THEN tcbf.column04
                                                                                                       ELSE ((-1) * tcbf.column04)
                                                                                                  END
                                                                                              ) AS VE
                                                                                       FROM   Table_017_Child2_BuyFactor tcbf
                                                                                              JOIN Table_024_Discount_Buy tdb
                                                                                                   ON  tdb.columnid = tcbf.column02
                                                                                       WHERE  tdb.Column18 = 1
                                                                                              AND tcbf.column01 = " + Row["ColumnId"] + @"
                                                                                   ) AS tt
                                                                        )

                                                                    SET @Net =isnull( (
                                                                            SELECT tbf.Column20
                                                                            FROM   Table_015_BuyFactor tbf
                                                                            WHERE  tbf.columnid = " + Row["ColumnId"] + @"
                                                                        ),0)
    
                                                                    SET @share = isnull(@sum /nullif( @Net,0),0)
                                                                    DECLARE @unitvalue   DECIMAL(18, 3),
                                                                            @totalvalue  DECIMAL(18, 3)

                                                                            SET @unitvalue =(CASE WHEN @share>0 then (
                                                                            ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column20)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + ChildItem.Cells["Column02"].Value + @"
                                                                                           AND tcbf.column01 = " + Row["ColumnId"] + @"
                                                                                ),
                                                                                0
                                                                            ) / nullif( ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column07)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + ChildItem.Cells["Column02"].Value + @"
                                                                                           AND tcbf.column01 = " + Row["ColumnId"] + @"
                                                                                ),
                                                                                0
                                                                            ),0)
                                                                        ) * (1 + @share) else isnull(" + Convert.ToDouble(ChildItem.Cells["Column20"].Value) + @" /nullif( " + Convert.ToDouble(ChildItem.Cells["Column07"].Value) + @",0),0) END)

                                                                    SET @totalvalue = @unitvalue * ISNULL(
                                                                            (
                                                                                SELECT SUM(tcbf.column07)
                                                                                FROM   Table_016_Child1_BuyFactor tcbf
                                                                                WHERE  tcbf.column02 = " + ChildItem.Cells["Column02"].Value + @"
                                                                                       AND tcbf.column01 = " + Row["ColumnId"] + @"
                                                                            ),
                                                                            0
                                                                        )  

                                                                    SELECT 1+@share AS share,
                                                                          isnull( @unitvalue,0) AS unitvalue,
                                                                         isnull(  @totalvalue,0) AS totalvalue

                                                                    ", ConSale);
                                Adapter.Fill(value);




                                sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt([column01]
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
           ,[Column35]) VALUES (@draftkey," + ChildItem.Cells["Column02"].Value.ToString() + "," +
                                                ChildItem.Cells["Column03"].Value.ToString() + "," + ChildItem.Cells["Column04"].Value.ToString() + "," + ChildItem.Cells["Column05"].Value.ToString() + "," + ChildItem.Cells["Column06"].Value.ToString() + "," + ChildItem.Cells["Column07"].Value.ToString() + "," +
                                                ChildItem.Cells["Column08"].Value.ToString() + "," + ChildItem.Cells["Column09"].Value.ToString() + "," + ChildItem.Cells["Column10"].Value.ToString() + "," + ChildItem.Cells["Column11"].Value.ToString() + ",NULL," +
                                                (ChildItem.Cells["Column21"].Value.ToString().Trim() == "" ? "NULL" : ChildItem.Cells["Column21"].Value.ToString()) + "," + (ChildItem.Cells["Column22"].Value.ToString().Trim() == "" ? "NULL" : ChildItem.Cells["Column22"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                                                + "',getdate(),0," + (Convert.ToDouble(ChildItem.Cells["Column07"].Value.ToString()) > 0 ? (Convert.ToDouble(ChildItem.Cells["Column20"].Value.ToString()) / Convert.ToDouble(ChildItem.Cells["Column07"].Value.ToString())) * Convert.ToDouble(value.Rows[0]["share"]) : 0) +
                                                "," + (Convert.ToDouble(ChildItem.Cells["Column20"].Value) * Convert.ToDouble(value.Rows[0]["share"])) + "," + ChildItem.Cells["ColumnId"].Value.ToString()
                                                + ",NULL,NULL,NULL,0,0,0,0," +
                                                (ChildItem.Cells["Column32"].Text.ToString().Trim() == "" ? "NULL" : "'" + ChildItem.Cells["Column32"].Value.ToString() + "'") + "," +
                                                (ChildItem.Cells["Column33"].Text.ToString().Trim() == "" ? "NULL" : "'" + ChildItem.Cells["Column33"].Value.ToString() + "'") + "," + ChildItem.Cells["Column29"].Value.ToString() +
                                                "," + ChildItem.Cells["Column30"].Value.ToString() + "," + ChildItem.Cells["Column34"].Value.ToString() + "," + ChildItem.Cells["Column35"].Value.ToString() + ")";


                            }
                        }
                        sanadcmd += "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor SET Column10=@draftkey  where ColumnId=" + Row["ColumnId"];

                        if (LastDocnum > 0)
                            sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                        else
                            sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + Row["column02"] + "',2,0,N'فاکتور خرید','" + Class_BasicOperation._UserName +
                       "',getdate()); SET @DocID=SCOPE_IDENTITY()";

                        string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());

                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               NULL, NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'فاکتور خرید " + Row["column01"].ToString() + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";


                        _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(Row["Column03"].ToString()) ? "NULL" : Row["Column03"].ToString()) + @", NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'فاکتور خرید " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                                " + (string.IsNullOrWhiteSpace(Row["Column03"].ToString()) ? "NULL" : Row["Column03"].ToString()) + @", NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'تخفیف فاکتور خرید ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                                sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               NULL, NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'تخفیف فاکتور خرید ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                                NULL, NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'ارزش افزوده فاکتور خرید ش " + Row["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                                sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(Row["Column03"].ToString()) ? "NULL" : Row["Column03"].ToString()) + @", NULL , " + ((Row["Column29"] != null && !string.IsNullOrWhiteSpace(Row["Column29"].ToString())) ? Row["Column29"] : "NULL") + @" ,
                   " + "'ارزش افزوده فاکتور خرید ش " + Row["column01"] + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,19," + int.Parse(Row["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                            }


                        }

                        sanadcmd += "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor SET Column10=@draftkey, Column11=@DocID where ColumnId=" + Row["ColumnId"];
                        sanadcmd += " UPDATE " + this.ConWare.Database + ".dbo.Table_011_PwhrsReceipt SET  column07=@DocID where ColumnId= @draftkey";
                        sanadcmd += buyprice;
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
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();


                                dataSet_Buy.EnforceConstraints = false;

                                this.table_015_BuyFactorTableAdapter.Fill_New(dataSet_Buy.Table_015_BuyFactor, Convert.ToInt32(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"]));
                                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(dataSet_Buy.Table_016_Child1_BuyFactor, Convert.ToInt32(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"]));
                                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(dataSet_Buy.Table_017_Child2_BuyFactor, Convert.ToInt32(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"]));

                                dataSet_Buy.EnforceConstraints = true;

                                this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);
                                if (sender == bt_Save || sender == this)
                                    Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                                      "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره رسید انبار: " + DraftNum.Value, "Information");


                                //if (DialogResult.Yes == MessageBox.Show("آیا مایل به چاپ فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                //{

                                //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 129))
                                //    {
                                //        // bt_Save_Click(sender, e);
                                //        _04_Buy.Reports.Form_BuyFactorPrint frm = new _04_Buy.Reports.Form_BuyFactorPrint
                                //            (int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()).ToString());
                                //        frm.ShowDialog();
                                //    }
                                //    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                                //}
                                //bt_New_Click(null, null);

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



                    if (_FromRecipt)
                    {
                        //قرار دادن شماره فاکتور خرید در رسید انبار
                        clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_011_PwhrsReceipt SET  Column13=" +
                            Row["ColumnId"].ToString() + " where ColumnId=" + _ResidID);
                        if (sender == bt_Save || sender == this)
                            Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                    }




                    bt_New.Enabled = true;
                }
            }
            //else if (gridEX_List.AllowEdit == InheritableBoolean.False)
            //{
            //    Class_BasicOperation.ShowMsg("", "امکان ثبت تغییرات وجود ندارد", "Stop");

            //    return;
            //}
            //در هر صورت بعد از ذخیره اطلاعات دوباره اطلاعات به روز رسانی می شوند

            int _ID = int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
            dataSet_Buy.EnforceConstraints = false;
            this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, _ID);
            this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, _ID);
            this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, _ID);
            dataSet_Buy.EnforceConstraints = true;
            table_015_BuyFactorBindingSource_PositionChanged(sender, e);


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
            DataTable Table = new DataTable();
            string command = string.Empty;

            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {


                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                int DocId = clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column11", RowID);
                int ResidId = clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", RowID);
                try
                {
                    if (!_del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");


                    //if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column31"]))
                    //{
                    //    throw new Exception("فاکتور تائید شده است امکان حذف وجود ندارد");

                    //}


                    if (int.Parse(RowID) > 0 && clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column17", "ColumnId", RowID) == "True")
                    {
                        dataSet_Buy.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(dataSet_Buy.Table_015_BuyFactor, int.Parse(RowID));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowID));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowID));
                        dataSet_Buy.EnforceConstraints = true;
                        throw new Exception("به علت ارجاع این فاکتور، حذف آن امکانپذیر نمی باشد");
                    }
                    if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور رسید انبار و سند حسابداری مربوطه حذف می شود،آیا مایل به حذف فاکتور جاری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        string good = string.Empty;
                        bool ok = true;
                        DataTable rt = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from  Table_011_PwhrsReceipt where columnid=" + ResidId);
                        if (ResidId > 0)
                        {
                            //چک باقی مانده کالا
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                            {

                                float Remain = DeleteRemain(int.Parse(item.Cells["Column02"].Value.ToString()), rt.Rows[0]["column03"].ToString(), rt.Rows[0]["column02"].ToString(), ResidId);
                                if (Remain < Convert.ToDouble(0) || TotalDeleteRemain(int.Parse(item.Cells["Column02"].Value.ToString()), rt.Rows[0]["column03"].ToString(), ResidId) < Convert.ToDouble(0))
                                {
                                    good += item.Cells["Column02"].Text + ",";

                                }
                            }

                            if (!string.IsNullOrWhiteSpace(good))
                            {
                                good = good.TrimEnd(',');

                                string Message1 = "";
                                Message1 = "موجودی کالاهای زیر منفی می شود آیا مایل به حذف فاکتور هستید؟" + Environment.NewLine + good;
                                if (DialogResult.Yes == MessageBox.Show(Message1, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                                    ok = true;
                                else
                                    ok = false;

                            }
                        }
                        if (ok)
                        //if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {

                            if (DocId > 0)
                            {
                                clDoc.IsFinal_ID(DocId);
                                //حذف سند فاکتور 
                                //clDoc.DeleteDetail_ID(DocId, 19, int.Parse(RowID));

                                Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=19 and Column17=" + int.Parse(RowID));
                                foreach (DataRow item in Table.Rows)
                                {
                                    command += "Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                                }

                                command += "Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=19 and Column17=" + int.Parse(RowID);




                                //حذف سند مربوط به رسید
                                // clDoc.DeleteDetail_ID(DocId, 12, int.Parse(RowID));

                                Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=12 and Column17=" + int.Parse(RowID));
                                foreach (DataRow item in Table.Rows)
                                {
                                    command += "Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                                }

                                command += "Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=12 and Column17=" + int.Parse(RowID);




                            }
                            if (ResidId > 0)
                            {
                                //درج صفر در شماره سند رسید و صفر در شماره فاکتور خرید رسید
                                //clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_011_PwhrsReceipt SET Column07=0 , Column13=0 where ColumnId=" + ResidId);
                                command += "Delete " + ConWare.Database + ".dbo.Table_012_Child_PwhrsReceipt   where Column01=" + ResidId;
                                command += "Delete " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt    where ColumnId=" + ResidId;



                            }
                            //حذف فاکتور
                            //foreach (DataRowView item in this.table_016_Child1_BuyFactorBindingSource)
                            //{
                            //    item.Delete();
                            //}
                            //this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                            //this.table_016_Child1_BuyFactorTableAdapter.Update(dataSet_Buy.Table_016_Child1_BuyFactor);
                            //foreach (DataRowView item in this.table_017_Child2_BuyFactorBindingSource)
                            //{
                            //    item.Delete();
                            //}
                            //this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                            //this.table_017_Child2_BuyFactorTableAdapter.Update(dataSet_Buy.Table_017_Child2_BuyFactor);
                            //this.table_015_BuyFactorBindingSource.RemoveCurrent();
                            //this.table_015_BuyFactorBindingSource.EndEdit();
                            //this.table_015_BuyFactorTableAdapter.Update(dataSet_Buy.Table_015_BuyFactor);

                            command += " Delete from " + ConSale.Database + ".dbo.Table_017_Child2_BuyFactor  Where column01 =" + int.Parse(RowID);
                            command += " Delete from " + ConSale.Database + ".dbo.Table_016_Child1_BuyFactor  Where column01 =" + int.Parse(RowID);
                            command += " Delete from " + ConSale.Database + ".dbo.Table_015_BuyFactor  Where columnid =" + int.Parse(RowID);


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
                                    this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, 0);
                                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, 0);
                                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, 0);
                                    dataSet_Buy.EnforceConstraints = true;
                                    bt_New.Enabled = true;

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
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        //private void gridEX1_Error(object sender, ErrorEventArgs e)
        //{
        //    e.DisplayErrorMessage = false;
        //    Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        //}

        //private void gridEX_List_Enter(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        table_015_BuyFactorBindingSource.EndEdit();
        //    }
        //    catch (Exception ex)
        //    {
        //        gridEX1.Focus();
        //        Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
        //    }
        //}

        //private void gridEX_Extra_UpdatingCell(object sender, UpdatingCellEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Column.Key == "column02")
        //        {

        //            gridEX_Extra.SetValue("column05", (gridEX_Extra.DropDowns["Type"].GetValue("column02")));
        //            gridEX_Extra.SetValue("column04", "0");
        //            gridEX_Extra.SetValue("column03", "0");

        //            if (gridEX_Extra.DropDowns["Type"].GetValue("column03").ToString() == "True")
        //            {
        //                gridEX_Extra.SetValue("column04", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
        //            }
        //            else
        //            {

        //                gridEX_Extra.SetValue("column03", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
        //                Double darsad;
        //                darsad = Convert.ToDouble(gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());

        //                Double kol;
        //                kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column20"].Value.ToString());
        //                if (kol == 0)
        //                    return;
        //                gridEX_Extra.SetValue("column04",
        //                    (gridEX1.GetValue("Column15").ToString() == "False") ?
        //                    Convert.ToInt64(kol * darsad / 100) : kol * darsad / 100);
        //            }
        //        }
        //        else if (e.Column.Key == "column03")
        //        {
        //            Double darsad;
        //            darsad = Convert.ToDouble(e.Value.ToString());
        //            Double kol;
        //            kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column20"].Value.ToString());
        //            if (kol == 0)
        //                return;
        //            gridEX_Extra.SetValue("column04",
        //                  (gridEX1.GetValue("Column15").ToString() == "False") ?
        //                  Convert.ToInt64(kol * darsad / 100) : kol * darsad / 100);
        //        }
        //    }
        //    catch
        //    {
        //    }
        //}

        //private void gridEX_List_CellValueChanged(object sender, ColumnActionEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Column.Key == "column02")
        //            Class_BasicOperation.FilterGridExDropDown(sender, "column02", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

        //        else if (e.Column.Key == "GoodCode")
        //            Class_BasicOperation.FilterGridExDropDown(sender, "GoodCode", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);
        //    }
        //    catch { }
        //    try
        //    {
        //        if (e.Column.Key == "column22")
        //            Class_BasicOperation.FilterGridExDropDown(sender, "column22", "Column01", "Column02", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
        //    }
        //    catch { }
        //    try
        //    {
        //        if (e.Column.Key == "column21")
        //            Class_BasicOperation.FilterGridExDropDown(sender, "column21", "Column01", "Column02", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
        //    }
        //    catch { }

        //    ((Janus.Windows.GridEX.GridEX)sender).CurrentCellDroppedDown = true;
        //}

        private void bt_ExportResid_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 74))
                        throw new Exception("کاربر گرامی شما امکان صدور رسید انبار را ندارید");

                    SaveEvent(sender, e);

                    if (clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", RowID) != 0)
                        throw new Exception("برای این فاکتور رسید انبار صادر شده است");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", RowID) == "True")
                        throw new Exception("به علت باطل شدن این فاکتور امکان صدور رسید انبار وجود ندارد");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column17", "ColumnId", RowID) == "True")
                        throw new Exception("به علت مرجوع شدن این فاکتور امکان صدور رسید انبار وجود ندارد");


                    DataTable Table = new DataTable();
                    Table.Columns.Add("GoodId", Type.GetType("System.String"));
                    Table.Columns.Add("GoodName", Type.GetType("System.String"));
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        Table.Rows.Add(item.Cells["Column02"].Value.ToString(),
                            item.Cells["Column02"].Text.Trim());
                    }

                    if (gridEX1.GetRow().Cells["Column27"].Text.Trim() != "" && gridEX1.GetRow().Cells["Column28"].Text.Trim() != "")
                    {
                        try
                        {
                            int ok = 0;
                            using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.ACNT))
                            {
                                Conacnt.Open();
                                SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column27"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                                ok = int.Parse(Commnad.ExecuteScalar().ToString());

                            }
                            if (ok == 0)
                            {
                                dataSet_Buy.EnforceConstraints = false;
                                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, 0);
                                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, 0);
                                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, 0);
                                dataSet_Buy.EnforceConstraints = true;
                                MessageBox.Show("به انبار این قاکتور دسترسی ندارید");
                                return;

                            }


                        }
                        catch
                        {
                        }
                        _04_Buy.Frm_010_ResidInformationDialog frm = new Frm_010_ResidInformationDialog(
                            this.table_016_Child1_BuyFactorBindingSource,
                            ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current),
                            Table, Convert.ToInt16(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column27"]),
                            Convert.ToInt16(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column28"]));
                        frm.ShowDialog();

                    }
                    else Class_BasicOperation.ShowMsg("", "انبار و نوع رسید را مشخص کنید", "None");

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }

                dataSet_Buy.EnforceConstraints = false;
                this.table_015_BuyFactorTableAdapter.Fill_New(
                    this.dataSet_Buy.Table_015_BuyFactor, int.Parse(RowID));
                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(
                    this.dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowID));
                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(
                    this.dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowID));
                dataSet_Buy.EnforceConstraints = true;
                this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

            }
        }

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 75))
                        throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");

                    SaveEvent(sender, e);

                    string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column11", RowID) != 0)
                    {

                        dataSet_Buy.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, int.Parse(RowID));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(
                            this.dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowID));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(
                            this.dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowID));
                        dataSet_Buy.EnforceConstraints = true;
                        this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                        throw new Exception("برای این فاکتور سند حسابداری صادر شده است");
                    }

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", RowID) == "True")
                        throw new Exception("به علت باطل شدن این فاکتور امکان صدور سند وجود ندارد");


                    SaveEvent(sender, e);
                    //*******************1:اگر رسید قبلا صادر شده باشد
                    if (gridEX1.GetRow().Cells["Column27"].Text.Trim() != "" && gridEX1.GetRow().Cells["Column28"].Text.Trim() != "")
                    {
                        try
                        {
                            int ok = 0;
                            using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.ACNT))
                            {
                                Conacnt.Open();
                                SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column27"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                                ok = int.Parse(Commnad.ExecuteScalar().ToString());

                            }
                            if (ok == 0)
                            {
                                dataSet_Buy.EnforceConstraints = false;
                                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, 0);
                                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, 0);
                                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, 0);
                                dataSet_Buy.EnforceConstraints = true;
                                MessageBox.Show("به انبار این قاکتور دسترسی ندارید");
                                return;

                            }


                        }
                        catch
                        {
                        }

                        DataRowView Row = (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;
                        if (int.Parse(Row["Column10"].ToString()) > 0)
                        {
                            //***************************if Finance Type is Periodic: Just export Doc for factor and Doc number will be save in Draft's n
                            _04_Buy.Frm_009_ExportDocInformation frm =
                                new Frm_009_ExportDocInformation(true,
                                    int.Parse(Row["Column10"].ToString()), int.Parse(RowID), Convert.ToInt16(gridEX1.GetRow().Cells["Column27"].Value), Convert.ToInt16(gridEX1.GetRow().Cells["Column28"].Value));
                            frm.ShowDialog();

                        }
                        //اگر رسید صادر نشده باشد
                        else
                        {
                            _04_Buy.Frm_009_ExportDocInformation frm =
                                new Frm_009_ExportDocInformation(true, 0,
                                    int.Parse(RowID), Convert.ToInt16(gridEX1.GetRow().Cells["Column27"].Value), Convert.ToInt16(gridEX1.GetRow().Cells["Column28"].Value));
                            frm.ShowDialog();
                        }

                        dataSet_Buy.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, int.Parse(RowID));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(
                            this.dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowID));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(
                            this.dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowID));
                        dataSet_Buy.EnforceConstraints = true;
                        this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                    }
                    else Class_BasicOperation.ShowMsg("", "انبار و نوع رسید را مشخص کنید", "None");



                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            DataTable Table = new DataTable();
            try
            {
                if (this.table_015_BuyFactorBindingSource.Count > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 76))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    int SanadID = clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column11", RowID);
                    int ResidID = clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", RowID);

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column17", "ColumnId", RowID) == "True")
                        throw new Exception("به علت ارجاع این فاکتور، حذف سند حسابداری امکانپذیر نمی باشد");


                    if (SanadID != 0)
                    {
                        string Message = "آیا مایل به حذف سند مربوط به این فاکتور هستید؟";
                        if (ResidID != 0)
                        {
                            Message = "برای این فاکتور، رسید انبار نیز صادر شده است. در صورت تأیید ثبت مربوط به رسید نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف سند این فاکتور هستید؟";
                        }
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.IsFinal_ID(SanadID);
                            //حذف سند فاکتور 
                            //  clDoc.DeleteDetail_ID(SanadID, 19, int.Parse(RowID));

                            Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=19 and Column17=" + int.Parse(RowID));
                            foreach (DataRow item in Table.Rows)
                            {
                                command += "Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += "Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=19 and Column17=" + int.Parse(RowID);



                            //حذف سند مربوط به رسید
                            //clDoc.DeleteDetail_ID(SanadID, 12, int.Parse(RowID));


                            Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=12 and Column17=" + int.Parse(RowID));
                            foreach (DataRow item in Table.Rows)
                            {
                                command += "Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += "Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=12 and Column17=" + int.Parse(RowID);





                            //درج صفر در شماره سند رسید
                            //clDoc.RunSqlCommand(ConWare.ConnectionString,
                            //    "UPDATE Table_011_PwhrsReceipt SET Column07=0 where ColumnId=" + ResidID);

                            command += "UPDATE " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt set Column07=0 where ColumnId=" + ResidID;




                            //آپدیت شماره سند در فاکتور
                            // clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_015_BuyFactor", "Column11", "ColumnId", int.Parse(RowID), 0);
                            command += "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor set Column11=0 where ColumnId=" + int.Parse(RowID);

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

                                    Class_BasicOperation.ShowMsg("", "حذف سند حسابداری  با موفقیت صورت گرفت", "Information");


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

                    dataSet_Buy.EnforceConstraints = false;
                    this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, int.Parse(RowID));
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowID));
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowID));
                    dataSet_Buy.EnforceConstraints = true;
                    this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bt_ReturnFactor_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 77))
                        throw new Exception("کاربر گرامی شما امکان مرجوع کردن فاکتور خرید را ندارید");



                    DataRowView Row = (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;

                    if (Row["Column17"].ToString() != "" && Row["Column17"].ToString() == "True")
                        throw new Exception("این فاکتور قبلا مرجوع شده است");

                    SaveEvent1(sender, e);
                    Row = (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;


                    if (((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                    {
                        throw new Exception("فاکتور ثبت نشده است");

                    }


                    if (Row["Column11"].ToString().Trim() == "" || Row["Column11"].ToString() == "0")
                        throw new Exception("جهت ارجاع یک فاکتور صدور سند حسابداری و رسید انبار، الزامیست");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به مرجوع کردن این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        //چک باقی مانده کالا
                        foreach (DataRowView item in this.table_016_Child1_BuyFactorBindingSource)
                        {
                            if (clDoc.IsGood(item["Column02"].ToString()))
                            {

                                float Remain = FirstRemain(int.Parse(item["Column02"].ToString()));
                                if (Remain < float.Parse(item["Column07"].ToString()))
                                {
                                    throw new Exception("موجودی کالای " + clDoc.ExScalar(ConWare.ConnectionString, "table_004_CommodityAndIngredients", "Column02", "ColumnId", item["Column02"].ToString()) + " کمتر از تعداد مشخص شده در فاکتور است");
                                }
                            }

                        }

                        //صدور حواله در صورت صادر شدن رسید برای فاکتور خرید

                        InsertDraft(Row);
                        if (ReturnId > 0 && DraftID > 0)
                        {
                            //ثبت عکس فاکتور فروش
                            InvertDoc(Row);
                            Class_BasicOperation.ShowMsg("", "ارجاع فاکتور با موفقیت انجام شد" + Environment.NewLine + "شماره سند حسابداری:" + ReturnDocNum.Value + Environment.NewLine + "شماره حواله انبار:" + DraftNum, "Information");

                            int RowId = int.Parse(Row["ColumnId"].ToString());
                            dataSet_Buy.EnforceConstraints = false;
                            this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, RowId);
                            this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, RowId);
                            this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, RowId);
                            dataSet_Buy.EnforceConstraints = true;
                            this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                        }
                    }


                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private float FirstRemain(int GoodCode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
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
                CommandText = string.Format(CommandText, clDoc.ExScalar(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column03", "ColumnId",
                    ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column10"].ToString()), GoodCode,
                     ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column02"].ToString());
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        private void TurnBack(DataRowView Row ,string Function )
        {
            if (clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column18", Row["ColumnId"].ToString()) != 0)
                throw new Exception("برای این فاکتور، فاکتور مرجوعی صادر شده است");

            ReturnDate = InputBox.Show("تاریخ ارجاع را وارد کنید:");
            if (!string.IsNullOrEmpty(ReturnDate))
            {

                //درج هدر مرجوعی
                ReturnNum = clDoc.MaxNumber(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column01");
                ReturnId = 0;
                SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                Key.Direction = ParameterDirection.Output;
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                {
                    Con.Open();
                    SqlCommand InsertHeader = new SqlCommand(@"INSERT INTO Table_021_MarjooiBuy  ([column01]
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
           ,[Column18]
           ,[Column19]
           ,[Column20]
           ,[Column21]
           ,[Column22]
           ,[Column23]
           ,[Column24]
           ,[Column26]
           ,[Column27]
           ,[Column28]
                ) VALUES(" + ReturnNum + ",'" + ReturnDate + "'," + Row["Column03"].ToString() + ",'"
                       + "ارجاع فاکتور خرید ش " + Row["Column01"].ToString() + " تاریخ " + Row["Column02"].ToString() + "','" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                        "0,0,0," + (Row["Column12"].ToString().Trim() == "" ? "NULL" : "'" + Row["Column12"].ToString().Trim() + "'") + "," +
                        (Row["Column13"].ToString().Trim() == "" ? "NULL" : "'" + Row["Column13"].ToString().Trim() + "'") + "," +
                        (Row["Column14"].ToString().Trim() == "" ? "NULL" : Row["Column14"].ToString().Trim())
                        + "," + (Row["Column15"].ToString() == "True" ? 1 : 0)
                        + ",0," + Row["ColumnId"].ToString() + "," + Row["Column20"].ToString() + "," + Row["Column21"].ToString() + "," + Row["Column22"].ToString() + "," + Row["Column23"].ToString()
                        + "," + Row["Column24"].ToString() + "," +
                        (Row["Column25"].ToString().Trim() == "" ? "NULL" : Row["Column25"].ToString()) + "," +
                        (Row["Column15"].ToString() == "True" ? Row["Column26"].ToString() : "0") + "," +
                        (Row["Column27"]!=null && !string.IsNullOrWhiteSpace( Row["Column27"].ToString().Trim()) ?  Row["Column27"] :"NULL")+ "," +
                        Function + "," +
                        (Row["Column29"]!=null && !string.IsNullOrWhiteSpace( Row["Column29"].ToString().Trim()) ?  Row["Column29"] :"NULL")+  
                        "); SET @Key=SCOPE_IDENTITY()", Con);
                    InsertHeader.Parameters.Add(Key);
                    InsertHeader.ExecuteNonQuery();
                    ReturnId = int.Parse(Key.Value.ToString());

                    //درج دیتیل1
                    foreach (DataRowView item in this.table_016_Child1_BuyFactorBindingSource)
                    {
                        SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_022_Child1_MarjooiBuy        ([column01]
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
           ,[Column27]
           ,[Column28]
           ,[Column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]) VALUES(" + ReturnId + "," + item["Column02"].ToString() +
                            "," + item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() +
                            "," + item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," +
                            item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                            (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString())
                            + "," + item["Column14"].ToString() + ",NULL," + item["Column16"].ToString() + "," + item["Column17"].ToString() + "," + item["Column18"].ToString() + "," + item["Column19"].ToString() + "," + item["Column20"].ToString() +
                            "," + (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," +
                            (item["Column22"].ToString().Trim() != "" ? item["Column22"].ToString() : "NULL") + ",NULL," +
                            (item["Column27"].ToString() != "" ? item["Column27"].ToString() : "0") + ",0,0," +
                            (item["Column28"].ToString() != "" ? item["Column28"].ToString() : "0") + "," +
                            item["Column29"].ToString() + "," + item["Column30"].ToString() +
                            "," + (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                            (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") +
                            "," + item["Column34"].ToString() + "," + item["Column35"].ToString() + ")", Con);
                        InsertDetail.ExecuteNonQuery();
                    }

                    //درج دیتیل 2
                    foreach (DataRowView item in this.table_017_Child2_BuyFactorBindingSource)
                    {
                        clDoc.RunSqlCommand(ConSale.ConnectionString, "INSERT INTO Table_023_Child2_MarjooiBuy VALUES(" + ReturnId + "," + item["Column02"].ToString()
                            + "," + item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + (item["Column05"].ToString() == "True" ? 1 : 0) + "," +
                            (item["Column06"].ToString().Trim() == "" ? "NULL" : item["Column06"].ToString()) + ")");

                    }
                    clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_015_BuyFactor SET Column17=1 , Column18=" + ReturnId + " Where ColumnId=" + Row["ColumnId"].ToString());
                }
            }
        }

        private void InsertDraft(DataRowView Row)
        {
            if (Row["Column10"].ToString().Trim() == "" || Row["Column10"].ToString() == "0")
                return;

            _04_Buy.Frm_011_DraftInformationDialog frm = new Frm_011_DraftInformationDialog();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                string Function = frm.FunctionValue;
                
                //صدور فاکتور مرجوعی
                TurnBack(Row, Function);

                //ResidTable
                DataTable ResidTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_011_PwhrsReceipt where ColumnId=" + Row["Column10"].ToString());
                DataTable ResidChild = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_012_Child_PwhrsReceipt where Column01=" + Row["Column10"].ToString());

                DraftNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01");
                //, int.Parse(ResidTable.Rows[0]["Column03"].ToString()));

                //**draft Header
                SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                key.Direction = ParameterDirection.Output;
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                {
                    Con.Open();
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
                                                                                               ,[Column26]) VALUES(" + DraftNum + ",'" + ReturnDate + "'," + ResidTable.Rows[0]["Column03"].ToString()
                     + "," + Function + "," + ResidTable.Rows[0]["Column05"].ToString() + ",'" + "رسید صادرشده از فاکتور مرجوعی شماره " +
                     ReturnNum + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                     + "',getdate(),0,NULL,NULL,0,0,0,0," + ReturnId + ",0,0,0," +
                     (ResidTable.Rows[0]["Column16"].ToString() == "True" ? 1 : 0) + "," +
                     (ResidTable.Rows[0]["Column17"].ToString().Trim() != "" ? ResidTable.Rows[0]["Column17"].ToString() : "NULL") + "," +
                     ResidTable.Rows[0]["Column18"].ToString()
                     + ",1); SET @Key=SCOPE_IDENTITY()", Con);
                    InsertHeader.Parameters.Add(key);
                    InsertHeader.ExecuteNonQuery();
                    DraftID = int.Parse(key.Value.ToString());

                    //Resid Detail
                    foreach (DataRow item in ResidChild.Rows)
                    {
                        if (clDoc.IsGood(item["Column02"].ToString()))
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
           ,[Column35]) VALUES(" + DraftID + "," + item["Column02"].ToString() + "," +
                             item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                             item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," +
                             item["Column11"].ToString() + ",NULL," + (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) +
                             "," + item["Column20"].ToString() +
                             "," + item["Column21"].ToString() + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                             + "',getdate(),NULL,NULL," +
                             (item["Column25"].ToString().Trim() == "" ? "NULL" : item["Column25"].ToString()) + "," +
                             item["Column26"].ToString() + ",0,0,0,0,0," +
                             (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column30"].ToString().Trim() + "'") + "," +
                             (item["Column31"].ToString().Trim() == "" ? "NULL" : "'" + item["Column31"].ToString().Trim() + "'") +
                             "," + item["Column32"].ToString() + "," + item["Column33"].ToString() + "," +
                             item["Column34"].ToString() + "," + item["Column35"].ToString() + ")", Con);
                            InsertDetail.ExecuteNonQuery();
                        }
                    }
                    //درج شماره حواله در فاکتور مرجوعی
                    clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column10", "ColumnId", ReturnId, DraftID);
                }
            }


        }

        private void InvertDoc(DataRowView Row)
        {
            if (Row["Column11"].ToString().Trim() == "" || Row["Column11"].ToString() == "0")
                return;

            DataTable PreDoc = clDoc.ReturnTable(ConAcnt.ConnectionString,
                "Select * from Table_065_SanadDetail where Column00=" +
                Row["Column11"].ToString() + " and (Column16=19) and Column17=" + Row["ColumnId"].ToString());
            if (PreDoc.Rows.Count > 0)
            {

                string CommandTxt = string.Empty;
                CommandTxt = "declare @Key int declare @DetialID int declare @ResidID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)   ";
                //Header
                // ReturnDocNum = clDoc.LastDocNum() + 1;
                CommandTxt += @" set @ReturnDocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1  INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + ReturnDate + "',2,0,'فاکتور مرجوعی','" + Class_BasicOperation._UserName +
                         "',getdate()); SET @Key=SCOPE_IDENTITY()";
                //ReturnDocId = clDoc.ExportDoc_Header(ReturnDocNum, ReturnDate, "فاکتور مرجوعی", Class_BasicOperation._UserName);

                //Detail
                foreach (DataRow item in PreDoc.Rows)
                {
                    string[] _AccInfo = clDoc.ACC_Info(item["Column01"].ToString());

                    CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
                VALUES (@Key,'" + item["Column01"].ToString() + @"',
                               " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                '" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                 " + (string.IsNullOrWhiteSpace(item["Column07"].ToString().Trim()) ? "NULL" : item["Column07"].ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item["Column08"].ToString().Trim()) ? "NULL" : item["Column08"].ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item["Column09"].ToString().Trim()) ? "NULL" : item["Column09"].ToString()) + @",
                                ' مرجوعی " + item["Column10"].ToString().Trim() + @"',
                                " + Convert.ToInt64(item["Column12"].ToString()) + @",
                                " + Convert.ToInt64(item["Column11"].ToString()) + @",
                                " + Convert.ToDouble(item["Column14"].ToString()) + @",
                                " + Convert.ToDouble(item["Column13"].ToString()) + @",
                                " + Int16.Parse(item["Column15"].ToString()) + @",
                                " + short.Parse((item["Column16"].ToString() == "12" ? 13 : 20).ToString()) + @",
                                " + ReturnId + @",
                                '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                " + Convert.ToDouble(item["Column22"].ToString()) + @",
                                NULL)";




                    //clDoc.ExportDoc_Detail(ReturnDocId, item["Column01"].ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                    //, (item["Column07"].ToString().Trim() == "" ? "NULL" : item["Column07"].ToString()), (item["Column08"].ToString().Trim() == "" ? "NULL" : item["Column08"].ToString()),
                    //(item["Column09"].ToString().Trim() == "" ? "NULL" : item["Column09"].ToString()), "مرجوعی-" + item["Column10"].ToString().Trim(), Convert.ToInt64(item["Column12"].ToString()),
                    //Convert.ToInt64(item["Column11"].ToString()),
                    //Convert.ToDouble(item["Column14"].ToString()),
                    //Convert.ToDouble(item["Column13"].ToString()),
                    //Int16.Parse(item["Column15"].ToString()),
                    //short.Parse((item["Column16"].ToString() == "12" ? 13 : 20).ToString()), ReturnId, Class_BasicOperation._UserName,
                    //Convert.ToDouble(item["Column22"].ToString()), (short?)null);
                }
                //درج شماره سند در فاکتور مرجوعی
                //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column11", "ColumnId", ReturnId, ReturnDocId);
                CommandTxt += " Update " + ConSale.Database + ".dbo.Table_021_MarjooiBuy set Column11=@Key where ColumnId=" + ReturnId;

                //درج شماره سند در حواله انبار
                //clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column07", "ColumnId", DraftID, ReturnDocId);
                CommandTxt += " Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07=@Key where ColumnId=" + DraftID;


                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();

                    SqlTransaction sqlTran = Con.BeginTransaction();
                    SqlCommand Command = Con.CreateCommand();
                    Command.Parameters.Add(ReturnDocNum);

                    Command.Transaction = sqlTran;

                    try
                    {
                        Command.CommandText = CommandTxt;
                        Command.ExecuteNonQuery();
                        sqlTran.Commit();



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

        private void Frm_003_FaktorKharid_KeyDown(object sender, KeyEventArgs e)
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
            else if (e.Control && e.KeyCode == Keys.O)
                چاپبارکدToolStripMenuItem_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.E)
                bt_NotConfirmReceipt_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.L)
                mnu_DelReceipt_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F8)

                bt_NotConfirmReceipt.ShowDropDown();
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 129))
                {


                    SaveEvent1(sender, e);
                    if (this.table_015_BuyFactorBindingSource.Count > 0)
                    {
                        gridEX1.AllowEdit = InheritableBoolean.False;
                        gridEX1.Enabled = true;
                        gridEX_List.AllowEdit = InheritableBoolean.False;
                        gridEX_Extra.AllowEdit = InheritableBoolean.False;
                        gridEX_List.AllowAddNew = InheritableBoolean.False;
                        gridEX_Extra.AllowAddNew = InheritableBoolean.False;
                        gridEX_Extra.AllowDelete = InheritableBoolean.False;
                        gridEX_List.AllowDelete = InheritableBoolean.False;
                        _04_Buy.Reports.Form_BuyFactorPrint frm = new _04_Buy.Reports.Form_BuyFactorPrint
                            (int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()).ToString());

                        frm.ShowDialog();
                        //this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                    }

                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            this.Cursor = Cursors.Default;
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
                    this.table_015_BuyFactorBindingSource.EndEdit();
                    this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                    this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                    //if (dataSet_Buy.Table_015_BuyFactor.GetChanges() != null || dataSet_Buy.Table_016_Child1_BuyFactor.GetChanges() != null || dataSet_Buy.Table_017_Child2_BuyFactor.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        bt_Save_Click(sender, e);
                    //    }
                    //}

                    dataSet_Buy.EnforceConstraints = false;
                    int ReturnedID = ReturnIDNumber(int.Parse(txt_Search.Text.Trim()));
                    this.table_015_BuyFactorTableAdapter.Fill_New(dataSet_Buy.Table_015_BuyFactor, ReturnedID);
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(dataSet_Buy.Table_016_Child1_BuyFactor, ReturnedID);
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(dataSet_Buy.Table_017_Child2_BuyFactor, ReturnedID);

                    dataSet_Buy.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

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
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_015_BuyFactor where column01=" + FactorNum + " and (Column29=" + projectId + " or '" + (Isadmin) + "'=N'True')", Con);
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
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 79))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_012_ViewBuyFactors")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                _04_Buy.Frm_012_ViewBuyFactors frm = new Frm_012_ViewBuyFactors();
                try
                {
                    frm.MdiParent = this.MdiParent;
                }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void Frm_003_FaktorKharid_Activated(object sender, EventArgs e)
        {
            txt_Search.Focus();
        }

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            int SanadId = 0;
            if (this.table_015_BuyFactorBindingSource.Count > 0)
                SanadId = (((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column11"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column11"].ToString()));
            string sanadnum = clDoc.ExScalar(ConAcnt.ConnectionString, @"select Column00 from Table_060_SanadHead where Columnid=" + ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column11"].ToString() + "");

            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
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
                    frm.MdiParent = this.MdiParent;
                }
                catch { }
                frm.Show();
                int Buyid = int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());

                dataSet_Buy.EnforceConstraints = false;
                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, Buyid);
                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, Buyid);
                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, Buyid);
                dataSet_Buy.EnforceConstraints = true;
                this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ViewResids_Click(object sender, EventArgs e)
        {
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;

            if (gridEX1.GetRow().Cells["Column10"].Text.Trim() == "0" || gridEX1.GetRow().Cells["Column10"].Text.Trim() == "")
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 22))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Form04_ViewWareReceipt")
                        {
                            item.BringToFront();
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form04_ViewWareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form04_ViewWareReceipt();
                    try
                    {
                        frm.MdiParent = this.MdiParent;
                    }
                    catch { }
                    frm.Show();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
            else
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 20))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Form03_WareReceipt")
                        {
                            item.BringToFront();
                            ((PWHRS._03_AmaliyatAnbar.Form03_WareReceipt)item).txt_Search.Text = gridEX1.GetRow().Cells["Column10"].Text;
                            ((PWHRS._03_AmaliyatAnbar.Form03_WareReceipt)item).bt_Search_Click(sender, e);
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form03_WareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form03_WareReceipt(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
                        int.Parse(gridEX1.GetValue("Column10").ToString()));
                    frm.ShowDialog();
                    int Buyid = int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    dataSet_Buy.EnforceConstraints = false;
                    this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, Buyid);
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, Buyid);
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, Buyid);
                    dataSet_Buy.EnforceConstraints = true;
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
        }

        private void mnu_ExtraDiscount_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 47))
            {
                _02_BasicInfo.Frm_003_TakhfifEzafeBuy ob = new _02_BasicInfo.Frm_003_TakhfifEzafeBuy(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 48));
                ob.ShowDialog();
                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount_Buy", ConSale);
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
                Int16 ware = 0;

                if (this.table_015_BuyFactorBindingSource.CurrencyManager.Count > 0 &&
                    ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column27"] != null &&
                    !string.IsNullOrWhiteSpace(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column27"].ToString())
                    )
                    ware = Convert.ToInt16(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column27"]);
                else if (storefactor.Rows[0]["ware"] != null &&
                 !string.IsNullOrWhiteSpace(storefactor.Rows[0]["ware"].ToString()))
                    ware = Convert.ToInt16(storefactor.Rows[0]["ware"]);
            string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + Convert.ToInt16(storefactor.Rows[0]["project"]) + "),0)");
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
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;

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

                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "column04")
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
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column03", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }
            try
            {
                if (e.Column.Key == "Column34")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column34", "column01", "column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }

            try
            {
                if (e.Column.Key == "Column27")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column27", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }
            try
            {
                if (e.Column.Key == "Column29")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column29", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }

            if (Control.ModifierKeys != Keys.Control)
                gridEX1.CurrentCellDroppedDown = true;

            gridEX1.SetValue("Column07", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column08", Class_BasicOperation.ServerDate());

            if (e.Column.Key == "column15")
            {
                if (gridEX1.GetValue("Column15").ToString() == "True")
                {
                    gridEX1.RootTable.Columns["Column25"].Selectable = true;
                    gridEX1.RootTable.Columns["Column26"].Selectable = true;
                }
                else
                {
                    gridEX1.RootTable.Columns["Column25"].Selectable = false;
                    gridEX1.RootTable.Columns["Column26"].Selectable = false;
                }
            }
            if (e.Column.Key == "Column27")
            {
                try
                {
                    if (gridEX1.GetValue("Column27") != null && !string.IsNullOrWhiteSpace(gridEX1.GetValue("Column27").ToString()) && gridEX1.GetValue("Column27").ToString().All(char.IsDigit))
                    {
                        string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + gridEX1.GetValue("Column29") + "),0)");
                        if (controlremain=="True")
                        {
                            GoodbindingSource.DataSource = clGood.MahsoolInfo(Convert.ToInt16(gridEX1.GetValue("Column27")));
                            DataTable GoodTable = clGood.MahsoolInfo(Convert.ToInt16(gridEX1.GetValue("Column27")));
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
                }
                catch
                {
                }
            }
            //try
            //{
            //    if (e.Column.Key == "column03")
            //    {
            //        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
            //        {
            //            Con.Open();
            //            SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL(Column30,0) from Table_045_PersonInfo where ColumnId=" + gridEX1.GetValue("column03") + "),0)", Con);
            //            if (int.Parse(Comm.ExecuteScalar().ToString()) > 0)
            //                gridEX1.SetValue("Column34", int.Parse(Comm.ExecuteScalar().ToString()));


            //        }
            //    }
            //}
            //catch { }
        }

        //private void gridEX_Extra_RowCountChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //Extra-Reductions
        //        Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
        //        txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
        //        Filter.Value1 = true;
        //        txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
        //        txt_TotalPrice.Value = Convert.ToDouble(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
        //        txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
        //    }
        //    catch
        //    {
        //    }
        //}

        private void gridEX_List_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                //Update TotalTextbox
                txt_TotalPrice.Value = Convert.ToDouble(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                try
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 78))
                        throw new Exception("کاربر گرامی شما امکان ابطال فاکتور خرید را ندارید");
                    if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column31"]))
                    {
                        throw new Exception("فاکتور تائید شده است امکان ابطال وجود ندارد");

                    }


                    if (clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", RowID) != 0)
                        throw new Exception("به علت صدور رسید برای این فاکتور، ابطال فاکتور امکانپذیر نیست");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", RowID) == "False")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ابطال این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", int.Parse(RowID), 1);

                        }
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                dataSet_Buy.EnforceConstraints = false;
                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, int.Parse(RowID));
                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowID));
                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowID));
                dataSet_Buy.EnforceConstraints = true;
                this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

            }
        }

        private void mnu_CancelCancel_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 78))
                        throw new Exception("کاربر گرامی شما امکان لغو ابطال فاکتور خرید را ندارید");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", RowID) == "True")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به لغو ابطال این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", int.Parse(RowID), 0);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                dataSet_Buy.EnforceConstraints = false;
                this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, int.Parse(RowID));
                this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowID));
                this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowID));
                dataSet_Buy.EnforceConstraints = true;
                this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

            }
        }

        private void gridEX_List_EditingCell(object sender, EditingCellEventArgs e)
        {
            try
            {
                if (((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() != "0" &&
                    ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column11"].ToString() == "0")
                    if (e.Column.Key == "column08" || e.Column.Key == "column09" || e.Column.Key == "column10" || e.Column.Key == "column11" ||
                        e.Column.Key == "column16" || e.Column.Key == "column18" || e.Column.Key == "column17")
                        e.Cancel = false;
                    else
                        e.Cancel = true;

            }
            catch
            {
            }
        }

        private void mnu_ViewReturnFactor_Click(object sender, EventArgs e)
        {
            int ReturnFactorID = 0;
            if (this.table_015_BuyFactorBindingSource.Count > 0)
                ReturnFactorID = (((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column18"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column18"].ToString()));
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 30))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_014_ReturnFactor")
                    {
                        item.BringToFront();
                        TextBox txt_S = (TextBox)item.ActiveControl;
                        txt_S.Text = ReturnFactorID.ToString();
                        PSHOP._04_Buy.Frm_014_ReturnFactor frms = (PSHOP._04_Buy.Frm_014_ReturnFactor)item;
                        frms.bt_Search_Click(sender, e);
                        return;
                    }
                }
                PSHOP._04_Buy.Frm_014_ReturnFactor frm = new Frm_014_ReturnFactor(
                  UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 31), int.Parse(ReturnFactorID.ToString()));
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

        private void gridEX_List_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                try
                {


                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column21");
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column22");
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
                    gridEX_List.SetValue("column10", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["BuyPrice"].ToString());
                    DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                    double amunt = 0;
                    if (gridEX1.GetValue("Column34") != null && !string.IsNullOrWhiteSpace(gridEX1.GetValue("Column34").ToString()))
                    {
                        DataRow[] dr = dt.Select("Column01='" + gridEX1.GetRow().Cells["Column34"].Text.Trim() + "'");
                        if (dr.Count() > 0)
                        {
                            amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                            gridEX_List.SetValue("Column38",
                             dr[0].ItemArray[3]);
                        }

                    }
                    else
                        gridEX_List.SetValue("Column38", ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
               "SalePrice"].ToString());

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
                    gridEX_List.SetValue("Column34", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Weight"].ToString());
                }

                else if (e.Column.Key == "Column13")
                {
                    object Value = gridEX_List.GetValue("Column13");
                    DataRowView Row = (DataRowView)gridEX_List.RootTable.Columns["Column13"].DropDown.FindItem(Value);
                    gridEX_List.SetValue("Column14", Row["Column02"]);
                }

                if (gridEX_List.GetRow().Cells["Column13"].Text.Trim() == "" && gridEX_List.GetRow().Cells["Column14"].Text.Trim() == "")
                {
                    if (gridEX1.GetRow().Cells["Column25"].Text.Trim() != "" &&
                          gridEX1.GetRow().Cells["Column26"].Text.Trim() != "")
                    {
                        gridEX_List.SetValue("Column13", gridEX1.GetValue("Column25").ToString());
                        gridEX_List.SetValue("Column14", gridEX1.GetValue("Column26").ToString());
                    }
                }

                gridEX_List.SetValue("column07",
                 (Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarkartoon")))
               + (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarbaste"))) +
               +Convert.ToDouble(gridEX_List.GetValue("column06")));

                gridEX_List.SetValue("Column35", Convert.ToDouble(gridEX_List.GetValue("Column34")) * Convert.ToDouble(gridEX_List.GetValue("column07")));



                double TotalPrice = (gridEX1.GetValue("Column15").ToString().Trim() == "True" ?

                   (Convert.ToDouble(gridEX_List.GetValue("column07")) * Convert.ToDouble(gridEX_List.GetValue("column10")))
                   : Convert.ToInt64((Convert.ToDouble(gridEX_List.GetValue("column07")) * Convert.ToDouble(gridEX_List.GetValue("column10")))));

                gridEX_List.SetValue("column11", TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column31").ToString()) / 100);


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
                    Double jam, takhfif = 0, ezafe;
                    jam = Convert.ToDouble(gridEX_List.GetValue("column11"));
                    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                        takhfif = (jam * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                    else takhfif = Convert.ToDouble(gridEX_List.GetValue("Column17").ToString());
                    if (ExtraMethod)

                        ezafe = (jam - takhfif) * Convert.ToDouble(gridEX_List.GetValue("column18")) / 100;
                    else
                        ezafe = (jam) * Convert.ToDouble(gridEX_List.GetValue("column18")) / 100;

                    gridEX_List.SetValue("column17", takhfif);
                    gridEX_List.SetValue("column19", ezafe);
                    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);
                }
                else
                {
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

        private void bt_DefineSignatures_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 121))
            {
                _04_Buy.Frm_018_Buy_Signatures frm = new Frm_018_Buy_Signatures();
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
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

                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column34");

            }
            catch { }
            try
            {

                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column27");

            }
            catch { }
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column29");
            }
            catch { }
            try
            {
                if (e.Column.Key == "Column25")
                {
                    object Value = gridEX1.GetValue("Column25");
                    DataRowView Row = (DataRowView)this.gridEX1.RootTable.Columns["Column25"].DropDown.FindItem(Value);
                    gridEX1.SetValue("Column26", Row["Column02"]);
                    gridEX_List.RootTable.Columns["Column13"].DefaultValue = gridEX1.GetValue("Column25");
                    gridEX_List.RootTable.Columns["Column14"].DefaultValue = gridEX1.GetValue("Column26");
                }
                else if (e.Column.Key == "Column26")
                {
                    gridEX_List.RootTable.Columns["Column13"].DefaultValue = gridEX1.GetValue("Column25");
                    gridEX_List.RootTable.Columns["Column14"].DefaultValue = gridEX1.GetValue("Column26");
                }

            }
            catch
            {
            }
        }

        private void mnu_DelReceipt_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                try
                {
                    if (((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                    {
                        throw new Exception("فاکتور ثبت نشده است");

                    }
                    int RowID = int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int ReceiptId = clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", RowID.ToString());
                    int SanadID = clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column11", RowID.ToString());

                    if (ReceiptId != 0)
                    {

                        if (clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column17", "columnid", RowID.ToString()) == "True")
                            throw new Exception("به علت ارجاع این فاکتور، حذف سند حسابداری امکانپذیر نمی باشد");

                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 76))
                        {

                            if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف سند و رسید مربوط به این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                string good = string.Empty;
                                bool ok = true;
                                DataTable rt = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from  Table_011_PwhrsReceipt where columnid=" + ReceiptId);

                                //چک باقی مانده کالا
                                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                                {

                                    float Remain = DeleteRemain(int.Parse(item.Cells["Column02"].Value.ToString()), rt.Rows[0]["column03"].ToString(), rt.Rows[0]["column02"].ToString(), ReceiptId);
                                    if (Remain < Convert.ToDouble(0) || TotalDeleteRemain(int.Parse(item.Cells["Column02"].Value.ToString()), rt.Rows[0]["column03"].ToString(), ReceiptId) < Convert.ToDouble(0))
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

                                    clDoc.IsFinal_ID(SanadID);
                                    string command = string.Empty;
                                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=19 and Column17=" + RowID);
                                    foreach (DataRow item in Table.Rows)
                                    {
                                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                                    }

                                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=19 and Column17=" + RowID;



                                    Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=12 and Column17=" + ReceiptId);
                                    foreach (DataRow item in Table.Rows)
                                    {
                                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                                    }

                                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=12 and Column17=" + ReceiptId;

                                    command += "Delete from " + ConWare.Database + ".dbo. Table_012_Child_PwhrsReceipt where column01=" + ReceiptId;
                                    command += "Delete from " + ConWare.Database + ".dbo. Table_011_PwhrsReceipt where ColumnId=" + ReceiptId;



                                    command += " UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor SET Column10=0,Column11=0,Column07='" + Class_BasicOperation._UserName + "', Column08=getdate() where ColumnId=" + RowID;


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
                                            Class_BasicOperation.ShowMsg("", "حذف سند حسابداری و رسید با موفقیت صورت گرفت", "Information");

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

                        }
                        else
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما   دسترسی برای حذف سند ندارید", "None");
                    }

                    dataSet_Buy.EnforceConstraints = false;
                    this.table_015_BuyFactorTableAdapter.Fill_New(dataSet_Buy.Table_015_BuyFactor, RowID);
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(dataSet_Buy.Table_016_Child1_BuyFactor, RowID);
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(dataSet_Buy.Table_017_Child2_BuyFactor, RowID);
                    dataSet_Buy.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
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

        private void bt_ViewRequests_Click(object sender, EventArgs e)
        {
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;


            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 79))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form04_ViewWareReceipt")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                PWHRS._07_RequestBuy.Form02_ViewRequests frm = new PWHRS._07_RequestBuy.Form02_ViewRequests();
                try
                {
                    frm.MdiParent = this.MdiParent;
                }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void gridEX_List_RowDoubleClick(object sender, RowActionEventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 38))
                {
                    PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                    PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
                    PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
                    PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
                    PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                    PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;

                    PWHRS._05_Gozareshat.Frm_010_KardexRiyali frms = new PWHRS._05_Gozareshat.Frm_010_KardexRiyali(
                        int.Parse(gridEX_List.GetValue("Column02").ToString()),
                        DateTime.Now.AddMonths(-1),
                        DateTime.Now);
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
            catch
            {
            }
        }

        private void gridEX_List_ColumnButtonClick(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (gridEX_List.GetValue("Column02").ToString() != "")
                {
                    string Txt = "";
                    DataTable Table = clDoc.GoodRemain(gridEX_List.GetValue("Column02").ToString(), gridEX1.GetValue("Column02").ToString());
                    foreach (DataRow item in Table.Rows)
                    {
                        Txt += " انبار:" + item["WareName"].ToString() + " " + Convert.ToDouble(item["Remain"].ToString()).ToString("#,##0.###")
                            + Environment.NewLine;
                    }
                    Txt += " آخرین قیمت خرید:" + LastBuyGoodPrice(int.Parse(gridEX_List.GetValue("Column02").ToString())).ToString("#,##0.###") + Environment.NewLine;
                    Txt += " آخرین قیمت خرید براساس تامین کننده:" + LastBuyGoodPrice(int.Parse(gridEX_List.GetValue("Column02").ToString()), int.Parse(gridEX1.GetValue("column03").ToString())).ToString("#,##0.###") + Environment.NewLine;

                    if (Txt.Trim() != "")
                        ToastNotification.Show(this, Txt, 3000, eToastPosition.MiddleCenter);
                }
            }
            catch
            {

            }
        }

        private Decimal LastBuyGoodPrice(int GoodCode)
        {
            DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, @"declare @t table(GoodCode int,Date nvarchar(50), Price decimal(18,3));
            insert into @t SELECT     Table_016_Child1_BuyFactor.column02,  MAX(Table_015_BuyFactor.column02) AS Date,1
            FROM         Table_016_Child1_BuyFactor INNER JOIN
            Table_015_BuyFactor ON Table_016_Child1_BuyFactor.column01 = Table_015_BuyFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @"
            GROUP BY Table_016_Child1_BuyFactor.column02
            order by Table_016_Child1_BuyFactor.column02;
            
            declare @t2 table(codekala2 int , gheymat2 int,date2 nvarchar(50)
            ,UNIQUE (codekala2,gheymat2,date2)
            );

            insert into @t2 SELECT   dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, 
            dbo.Table_015_BuyFactor.column02 AS Date
            FROM         dbo.Table_016_Child1_BuyFactor INNER JOIN
            dbo.Table_015_BuyFactor ON dbo.Table_016_Child1_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @"
            GROUP BY dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, dbo.Table_015_BuyFactor.column02;
            update @t set Price=gheymat2 from @t2 as main_table where GoodCode=codekala2 and Date=date2; 
            select * from @t");

            if (Table.Rows.Count == 0)
                return 0;
            else
                return Convert.ToDecimal(Table.Rows[0]["Price"].ToString());

        }

        private Decimal LastBuyGoodPrice(int GoodCode, int provider)
        {
            DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, @"declare @t table(GoodCode int,Date nvarchar(50), Price decimal(18,3));
            insert into @t SELECT     Table_016_Child1_BuyFactor.column02,  MAX(Table_015_BuyFactor.column02) AS Date,1
            FROM         Table_016_Child1_BuyFactor INNER JOIN
            Table_015_BuyFactor ON Table_016_Child1_BuyFactor.column01 = Table_015_BuyFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @" AND Table_015_BuyFactor.column03=" + provider + @"
            GROUP BY Table_016_Child1_BuyFactor.column02
            order by Table_016_Child1_BuyFactor.column02;
            
            declare @t2 table(codekala2 int , gheymat2 int,date2 nvarchar(50)
            ,UNIQUE (codekala2,gheymat2,date2)
            );

            insert into @t2 SELECT   dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, 
            dbo.Table_015_BuyFactor.column02 AS Date
            FROM         dbo.Table_016_Child1_BuyFactor INNER JOIN
            dbo.Table_015_BuyFactor ON dbo.Table_016_Child1_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @" AND Table_015_BuyFactor.column03=" + provider + @"
            GROUP BY dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, dbo.Table_015_BuyFactor.column02;
            update @t set Price=gheymat2 from @t2 as main_table where GoodCode=codekala2 and Date=date2; 
            select * from @t");

            if (Table.Rows.Count == 0)
                return 0;
            else
                return Convert.ToDecimal(Table.Rows[0]["Price"].ToString());

        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_015_BuyFactorBindingSource.EndEdit();
                this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                this.table_017_Child2_BuyFactorBindingSource.EndEdit();

                if (dataSet_Buy.Table_015_BuyFactor.GetChanges() != null || dataSet_Buy.Table_016_Child1_BuyFactor.GetChanges() != null ||
                    dataSet_Buy.Table_017_Child2_BuyFactor.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        SaveEvent(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select min(Column01) from Table_015_BuyFactor where (Column29=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_015_BuyFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Buy.EnforceConstraints = false;
                    this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_Buy.EnforceConstraints = true;
                    this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                try
                {
                    gridEX1.UpdateData();
                    table_015_BuyFactorBindingSource.EndEdit();
                    this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                    this.table_017_Child2_BuyFactorBindingSource.EndEdit();

                    if (dataSet_Buy.Table_015_BuyFactor.GetChanges() != null || dataSet_Buy.Table_016_Child1_BuyFactor.GetChanges() != null ||
                        dataSet_Buy.Table_017_Child2_BuyFactor.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            SaveEvent(sender, e);
                        }
                    }


                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString,
                        "Select ISNULL((Select max(Column01) from Table_015_BuyFactor where Column01<" +
                        ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + " and (Column29=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_015_BuyFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_Buy.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_Buy.EnforceConstraints = true;
                        this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

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
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {

                try
                {
                    gridEX1.UpdateData();
                    table_015_BuyFactorBindingSource.EndEdit();
                    this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                    this.table_017_Child2_BuyFactorBindingSource.EndEdit();

                    if (dataSet_Buy.Table_015_BuyFactor.GetChanges() != null || dataSet_Buy.Table_016_Child1_BuyFactor.GetChanges() != null ||
                        dataSet_Buy.Table_017_Child2_BuyFactor.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            SaveEvent(sender, e);
                        }
                    }

                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select Min(Column01) from Table_015_BuyFactor where Column01>" + ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + " and (Column29=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_015_BuyFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_Buy.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_Buy.EnforceConstraints = true;
                        this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

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
                table_015_BuyFactorBindingSource.EndEdit();
                this.table_016_Child1_BuyFactorBindingSource.EndEdit();
                this.table_017_Child2_BuyFactorBindingSource.EndEdit();

                if (dataSet_Buy.Table_015_BuyFactor.GetChanges() != null || dataSet_Buy.Table_016_Child1_BuyFactor.GetChanges() != null ||
                    dataSet_Buy.Table_017_Child2_BuyFactor.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        SaveEvent(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select max(Column01) from Table_015_BuyFactor where (Column29=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_015_BuyFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Buy.EnforceConstraints = false;
                    this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_Buy.EnforceConstraints = true;
                    this.table_015_BuyFactorBindingSource_PositionChanged(sender, e);

                }

            }
            catch
            {
            }
        }


        private void bt_Attachments_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                // if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 95))
                {
                    try
                    {
                        DataRowView Row = (DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current;
                        Form03_BuyFactorAppendix frm = new Form03_BuyFactorAppendix(
                            int.Parse(Row["ColumnId"].ToString()),
                            int.Parse(Row["Column01"].ToString()));
                        frm.ShowDialog();
                    }
                    catch
                    {
                    }
                }
                //  else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
        }

        private void تعریفپروژهToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 3))
            {
                PACNT._1_BasicMenu.Form02_Projects frm = new PACNT._1_BasicMenu.Form02_Projects(
                     UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 4));
                frm.ShowDialog();
                DataTable dt = new DataTable();
                SqlDataAdapter ProjectAdapter = new SqlDataAdapter("SELECT * from Table_035_ProjectInfo", ConBase);
                ProjectAdapter.Fill(dt);
                gridEX1.DropDowns["project"].SetDataBinding(dt, "");
                gridEX_List.DropDowns["Project"].SetDataBinding(dt, "");


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX_List_RecordAdded(object sender, EventArgs e)
        {
            try
            {


                txt_TotalPrice.Value = Convert.ToDouble(
           gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
           AggregateFunction.Sum).ToString());

                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) -
                      Convert.ToDouble(txt_Reductions.Value.ToString());

            }
            catch
            {
            }

        }

        private void txt_TotalPrice_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txt_TotalPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_Comfirm_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {

                if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["column17"])
                    || Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["column19"])
                    )
                {
                    Class_BasicOperation.ShowMsg("", "فاکتور باطل/مرجوع شده است امکان تائید وجود ندارد", "Stop");
                    return;
                }
                string command = string.Empty;
                Class_UserScope UserScope = new Class_UserScope();
                if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 237))
                {
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما دسترسی تائید / لغو تائید را ندارید", "Stop");
                    return;
                }
                string RowID = ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                if (Convert.ToBoolean(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column31"]))
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به لغو تائید فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))

                        command = "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor set Column31=0,Column32='" + Class_BasicOperation._UserName + "',Column33=getdate() where columnid=" + RowID;


                }
                else
                {

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به تائید فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))

                        command = "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor set Column31=1,Column32='" + Class_BasicOperation._UserName + "',Column33=getdate() where columnid=" + RowID;
                }




                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
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
                        Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد", "Information");
                        dataSet_Buy.EnforceConstraints = false;
                        this.table_015_BuyFactorTableAdapter.Fill_New(this.dataSet_Buy.Table_015_BuyFactor, int.Parse(RowID));
                        this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, int.Parse(RowID));
                        this.table_016_Child1_BuyFactorTableAdapter.Fill_headerID(this.dataSet_Buy.Table_016_Child1_BuyFactor, int.Parse(RowID));
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

        private void gridEX_List_FormattingRow(object sender, RowLoadEventArgs e)
        {

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
                if (table_015_BuyFactorBindingSource.Count > 0)
                {

                    if (gridEX1.GetValue("Column03") == null || gridEX1.GetValue("Column03") == DBNull.Value || gridEX1.GetValue("Column03").ToString() == "" || gridEX1.GetValue("Column03").ToString() == "0")
                    {
                        MessageBox.Show("اطلاعات تامین کننده معتبر نمی باشد");
                        //gridEX1.Focus();
                        //gridEX1.Col = 3;
                        return;
                    }
                    else if (gridEX1.GetValue("Column02") == null || gridEX1.GetValue("Column02") == DBNull.Value || gridEX1.GetValue("Column02").ToString() == "")
                    {
                        MessageBox.Show("اطلاعات تاریخ معتبر نمی باشد");
                        //gridEX1.Focus();
                        //gridEX1.Col = 2;
                        return;
                    }

                    table_015_BuyFactorBindingSource.EndEdit();
                }
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

        private void txt_GoodCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                InitialNewRow();
            }


            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
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



                                    DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                    double amunt = 0;
                                    if (gridEX1.GetValue("Column34") != null && !string.IsNullOrWhiteSpace(gridEX1.GetValue("Column34").ToString()))
                                    {
                                        DataRow[] dr = dt.Select("Column01='" + gridEX1.GetRow().Cells["Column34"].Text.Trim() + "'");
                                        if (dr.Count() > 0)
                                        {
                                            amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                            gridEX_List.SetValue("Column38",
                                             dr[0].ItemArray[3]);
                                        }

                                    }
                                    else
                                        gridEX_List.SetValue("Column38", ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                               "SalePrice"].ToString());




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


                                    DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                    double amunt = 0;
                                    if (gridEX1.GetValue("Column34") != null && !string.IsNullOrWhiteSpace(gridEX1.GetValue("Column34").ToString()))
                                    {
                                        DataRow[] dr = dt.Select("Column01='" + gridEX1.GetRow().Cells["Column34"].Text.Trim() + "'");
                                        if (dr.Count() > 0)
                                        {
                                            amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                            gridEX_List.SetValue("Column38",
                                             dr[0].ItemArray[3]);
                                        }

                                    }
                                    else
                                        gridEX_List.SetValue("Column38", ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                               "SalePrice"].ToString());

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
            // waredt = new DataTable();

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
                        throw new Exception("کد حساب معتبر را در معرفی اضافات و کسورات خرید وارد کنید");

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
                        throw new Exception("کد حساب معتبر را در معرفی اضافات و کسورات خرید وارد کنید");
                }


            }


            Adapter = new SqlDataAdapter(
                                                                   @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                    Column14, Column15, Column16
                                                                        FROM            Table_105_SystemTransactionInfo
                                                                        WHERE        (Column00 = 9) ", ConBase);
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
                    throw new Exception("کد حساب معتبر را در تنظیمات فروش وارد کنید");


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
                    throw new Exception("کد حساب معتبر را در تنظیمات فروش وارد کنید");
            }



            if (waredt.Rows.Count >= 1)
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   table_005_PwhrsOperation
                                                                   WHERE  columnid = " + waredt.Rows[0]["Column02"] + @"
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("عملکرد انبار در تنظیمات فروشگاه انتخاب نشده است");
                }



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

            }
            else
                throw new Exception("عملکرد انبار در تنظیمات فروشگاه تعریف نشده است");
            ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column28"] = waredt.Rows[0]["Column02"];
            if (((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
            {
                LastDocnum = LastDocNum(date);
                if (LastDocnum > 0)
                    clDoc.IsFinal(LastDocnum);
            }
            else if (Convert.ToInt32(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["column11"]) > 0)
                LastDocnum = Convert.ToInt32(clDoc.ExScalarQuery(Properties.Settings.Default.ACNT, @"SELECT ISNULL(
                                                                                                            (select Column00 from Table_060_SanadHead where ColumnId=" + ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["column11"] + @"),0) AS column01"));


            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column02"].ToString());

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
                                                                                       FactorTable.NetTotal,FactorTable.person,FactorTable.Column29
                                                                                FROM   (
                                                                                           SELECT dbo.Table_015_BuyFactor.columnid,
                                                                                                  dbo.Table_015_BuyFactor.column01,
                                                                                                  dbo.Table_015_BuyFactor.column02 AS Date,
                                                                                                  dbo.Table_015_BuyFactor.column03 AS person,
                                                                                                  OtherPrice.PlusPrice AS Ezafat,
                                                                                                  OtherPrice.MinusPrice AS Kosoorat,
                                                                                                  OtherPrice.Bed,
                                                                                                  OtherPrice.Bes,
                                                                                                  dbo.Table_015_BuyFactor.Column20 AS NetTotal,
                                                                                                  dbo.Table_015_BuyFactor.Column29

                                                                                           FROM   dbo.Table_015_BuyFactor
                                                                                                        LEFT OUTER JOIN (
                                                                                                           SELECT columnid,
                                                                                                                  SUM(PlusPrice) AS PlusPrice,
                                                                                                                  SUM(MinusPrice) AS MinusPrice,
                                                                                                                  Bed,
                                                                                                                  Bes
                                                                                                           FROM   (
                                                                                                                      SELECT Table_015_BuyFactor_2.columnid,
                                                                                                                             SUM(dbo.Table_017_Child2_BuyFactor.column04) AS 
                                                                                                                             PlusPrice,
                                                                                                                             0 AS MinusPrice,
                                                                                                                             td.column10 AS Bed,
                                                                                                                             td.column16 AS Bes
                                                                                                                      FROM   dbo.Table_017_Child2_BuyFactor
                                                                                                                             JOIN Table_024_Discount_Buy td
                                                                                                                                  ON  td.columnid = dbo.Table_017_Child2_BuyFactor.column02
                                                                                                                             INNER JOIN dbo.Table_015_BuyFactor AS 
                                                                                                                                  Table_015_BuyFactor_2
                                                                                                                                  ON  dbo.Table_017_Child2_BuyFactor.column01 = 
                                                                                                                                      Table_015_BuyFactor_2.columnid
                                                                                                                      WHERE  (dbo.Table_017_Child2_BuyFactor.column05 = 0)
                                                                                                                      GROUP BY
                                                                                                                             Table_015_BuyFactor_2.columnid,
                                                                                                                             dbo.Table_017_Child2_BuyFactor.column05,
                                                                                                                             td.column10,
                                                                                                                             td.column16
                                                                                                                      UNION ALL
                                                                                                                      SELECT Table_015_BuyFactor_1.columnid,
                                                                                                                             0 AS PlusPrice,
                                                                                                                             SUM(Table_017_Child2_BuyFactor_1.column04) AS 
                                                                                                                             MinusPrice,
                                                                                                                             td.column10 AS Bed,
                                                                                                                             td.column16 AS Bes
                                                                                                                      FROM   dbo.Table_017_Child2_BuyFactor AS 
                                                                                                                             Table_017_Child2_BuyFactor_1
                                                                                                                             JOIN Table_024_Discount_Buy td
                                                                                                                                  ON  td.columnid = 
                                                                                                                                      Table_017_Child2_BuyFactor_1.column02
                                                                                                                             INNER JOIN dbo.Table_015_BuyFactor AS 
                                                                                                                                  Table_015_BuyFactor_1
                                                                                                                                  ON  
                                                                                                                                      Table_017_Child2_BuyFactor_1.column01 = 
                                                                                                                                      Table_015_BuyFactor_1.columnid
                                                                                                                      WHERE  (Table_017_Child2_BuyFactor_1.column05 = 1)
                                                                                                                      GROUP BY
                                                                                                                             Table_015_BuyFactor_1.columnid,
                                                                                                                             Table_017_Child2_BuyFactor_1.column05,
                                                                                                                             td.column10,
                                                                                                                             td.column16
                                                                                                                  ) AS OtherPrice_1
                                                                                                           GROUP BY
                                                                                                                  columnid,
                                                                                                                  OtherPrice_1.Bed,
                                                                                                                  OtherPrice_1.Bes
                                                                                                       ) AS OtherPrice
                                                                                                       ON  dbo.Table_015_BuyFactor.columnid = OtherPrice.columnid
                                                                                       ) AS FactorTable
                                        WHERE  FactorTable.columnid=" + int.Parse(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + @"
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

            All_Controls_Row1(factordt.Rows[0]["Column07"].ToString(), null, null, ((Sanaddt.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column29"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column29"]) : (Int16?)null)));
            All_Controls_Row1(factordt.Rows[0]["Column13"].ToString(), Convert.ToInt32(Sanaddt.Rows[0]["person"].ToString()), null, ((Sanaddt.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column29"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column29"]) : (Int16?)null)));

            TPerson.Rows.Add(Int32.Parse(Sanaddt.Rows[0]["person"].ToString()), factordt.Rows[0]["Column13"].ToString(), Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()));
            TAccounts.Rows.Add(factordt.Rows[0]["Column13"].ToString(), (-1 * Convert.ToDouble((Sanaddt.Rows[0]["NetTotal"]))));
            TAccounts.Rows.Add(factordt.Rows[0]["Column07"].ToString(), (Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"])));

            foreach (DataRow dr in Sanaddt.Rows)
            {


                if (Convert.ToDouble(dr["Ezafat"]) > 0)
                {
                    All_Controls_Row1(dr["Bed"].ToString(), null, null, ((Sanaddt.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column29"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column29"]) : (Int16?)null)));
                    All_Controls_Row1(dr["Bes"].ToString(), int.Parse(dr["person"].ToString()), null, ((Sanaddt.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column29"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column29"]) : (Int16?)null)));
                    TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Ezafat"])));
                    TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Ezafat"])));
                    TPerson.Rows.Add(Int32.Parse(dr["person"].ToString()), dr["Bes"].ToString(), Convert.ToDouble(dr["Ezafat"]));


                }
                if (Convert.ToDouble(dr["Kosoorat"]) > 0)
                {
                    All_Controls_Row1(dr["Bes"].ToString(), null, null, ((Sanaddt.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column29"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column29"]) : (Int16?)null)));
                    All_Controls_Row1(dr["Bed"].ToString(), int.Parse(dr["person"].ToString()), null, ((Sanaddt.Rows[0]["Column29"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Column29"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Column29"]) : (Int16?)null)));
                    TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Kosoorat"])));
                    TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Kosoorat"])));
                    TPerson.Rows.Add(Int32.Parse(dr["person"].ToString()), dr["Bed"].ToString(), Convert.ToDouble(dr["Kosoorat"]));


                }



            }
            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);

        }

        public void All_Controls_Row1(string AccountCode, int? Person, Int16? Center, Int16? Project)
        {
            string m = "فاکتور شماره ی " + ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["column01"].ToString() + "دخیره شد اما صدور سند به دلیل زیر با خطا مواجه شد" + Environment.NewLine;
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

        private void چاپبارکدToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridEX_List.GetCheckedRows().Count() == 0)
            {
                MessageBox.Show("برای چاپ بارکد کالا انتخاب کنید");
                return;
            }
            string barcode = string.Empty;
            string name = string.Empty;

            DataTable dt = new DataTable();
            dt.Columns.Add("SN", typeof(string));
            dt.Columns.Add("Name", typeof(string));

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetCheckedRows())
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand("SELECT isnull( tcc.column06,'') FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["column02"].Value + "", Con);
                    barcode = (Comm.ExecuteScalar().ToString());
                    Comm = new SqlCommand("SELECT isnull( tcc.column02,'') FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["column02"].Value + "", Con);
                    name = (Comm.ExecuteScalar().ToString());

                }

                if (!string.IsNullOrWhiteSpace(barcode))
                {
                    for (int i = 1; i <= Convert.ToInt32(item.Cells["column07"].Value); i++)
                    {
                        dt.Rows.Add(barcode, name);

                    }
                }
            }

            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 129))
                {
                    bt_Save_Click(sender, e);
                    _04_Buy.Reports.Form_BuyFactorPrint frm = new _04_Buy.Reports.Form_BuyFactorPrint(dt);
                    frm.ShowDialog();
                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
            }
            catch { }
            this.Cursor = Cursors.Default;
        }

        private void bt_NotConfirmReceipt_Click(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactorBindingSource.Count > 0)
            {
                int ReceiptId = clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", ((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());

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

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void gridEX_Extra_RowCountChanged(object sender, EventArgs e)
        {

        }

        private void gridEX_Extra_UpdatingCell(object sender, UpdatingCellEventArgs e)
        {

        }

        private void gridEX_Extra_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX_Extra.CurrentCellDroppedDown = true;
        }

        private void gridEX_Extra_Enter(object sender, EventArgs e)
        {
            try
            {
                if (table_015_BuyFactorBindingSource.Count > 0)
                {

                    if (gridEX1.GetValue("Column03") == null || gridEX1.GetValue("Column03") == DBNull.Value || gridEX1.GetValue("Column03").ToString() == "" || gridEX1.GetValue("Column03").ToString() == "0")
                    {
                        MessageBox.Show("اطلاعات تامین کننده معتبر نمی باشد");
                        //gridEX1.Focus();
                        //gridEX1.Col = 3;
                        return;
                    }
                    else if (gridEX1.GetValue("Column02") == null || gridEX1.GetValue("Column02") == DBNull.Value || gridEX1.GetValue("Column02").ToString() == "")
                    {
                        MessageBox.Show("اطلاعات تاریخ معتبر نمی باشد");
                        //gridEX1.Focus();
                        //gridEX1.Col = 2;
                        return;
                    }

                    table_015_BuyFactorBindingSource.EndEdit();
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Extra_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_Extra_UpdatingCell_1(object sender, UpdatingCellEventArgs e)
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
                        gridEX_Extra.SetValue("column04",

                            Convert.ToInt64(kol * darsad / 100));
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
                    gridEX_Extra.SetValue("column04",

                           Convert.ToInt64(kol * darsad / 100));
                }
            }
            catch
            {
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            p_tax.Visible = false;
            gridEX_Extra.CancelCurrentEdit();
            table_017_Child2_BuyFactorBindingSource.CancelEdit();
            this.table_017_Child2_BuyFactorTableAdapter.Fill_HeaderID(this.dataSet_Buy.Table_017_Child2_BuyFactor, Convert.ToInt32(((DataRowView)this.table_015_BuyFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
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

                table_017_Child2_BuyFactorBindingSource.EndEdit();
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

        private void اضافاتوکسوراتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (((DataRowView)table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() == "0" || ((DataRowView)table_015_BuyFactorBindingSource.CurrencyManager.Current)["Column11"].ToString() == "0")
                {


                    table_015_BuyFactorBindingSource.EndEdit();
                    table_016_Child1_BuyFactorBindingSource.EndEdit();
                    if (table_015_BuyFactorBindingSource.Count > 0 && table_016_Child1_BuyFactorBindingSource.Count > 0)
                    {
                        p_tax.Visible = true;
                    }
                }
                else
                {
                    MessageBox.Show("این فاکتور داری سند و رسید می باشد ");
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }

        }

        private void bt_AddExtraDiscounts_Click(object sender, EventArgs e)
        {
            if (gridEX_Extra.AllowAddNew == InheritableBoolean.True && this.table_015_BuyFactorBindingSource.Count > 0 && this.table_016_Child1_BuyFactorBindingSource.Count > 0)
            {
                try
                {
                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount");
                    foreach (DataRow item in Table.Rows)
                    {
                        this.table_017_Child2_BuyFactorBindingSource.AddNew();
                        DataRowView New = (DataRowView)this.table_017_Child2_BuyFactorBindingSource.CurrencyManager.Current;
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
                        this.table_017_Child2_BuyFactorBindingSource.EndEdit();
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void gridEX_List_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            try
            {


                if (e.Column.Key == "column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column02", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

                else if (e.Column.Key == "GoodCode")
                    Class_BasicOperation.FilterGridExDropDown(sender, "GoodCode", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);
            }
            catch { }
            try
            {
                if (e.Column.Key == "column22")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column22", "Column01", "Column02", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }
            try
            {
                if (e.Column.Key == "column21")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column21", "Column01", "Column02", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }

            ((Janus.Windows.GridEX.GridEX)sender).CurrentCellDroppedDown = true;
        }



        private void gridEX1_Enter(object sender, EventArgs e)
        {

        }

        private void btn_UpdateDataGood_Click(object sender, EventArgs e)
        {
            string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + gridEX1.GetValue("Column29") + "),0)");
            if (controlremain=="True")
            {
                GoodbindingSource.DataSource = clGood.MahsoolInfo(0);
                DataTable GoodTable = clGood.MahsoolInfo(0);
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

        private void gridEX_List_Enter(object sender, EventArgs e)
        {
            try
            {
                if (table_015_BuyFactorBindingSource.Count > 0)
                {

                    if (gridEX1.GetValue("Column03") == null || gridEX1.GetValue("Column03") == DBNull.Value || gridEX1.GetValue("Column03").ToString() == "" || gridEX1.GetValue("Column03").ToString() == "0")
                    {
                        MessageBox.Show("اطلاعات تامین کننده معتبر نمی باشد");
                        //gridEX1.Focus();
                        //gridEX1.Col = 3;
                        return;
                    }
                    else if (gridEX1.GetValue("Column02") == null || gridEX1.GetValue("Column02") == DBNull.Value || gridEX1.GetValue("Column02").ToString() == "")
                    {
                        MessageBox.Show("اطلاعات تاریخ معتبر نمی باشد");
                        //gridEX1.Focus();
                        //gridEX1.Col = 2;
                        return;
                    }

                    table_015_BuyFactorBindingSource.EndEdit();
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }




    }

}