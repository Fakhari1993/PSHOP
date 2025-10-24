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

namespace PSHOP._05_Sale
{
    public partial class Frm_002_NewFaktor : Form
    {
        bool _del;
        int _ID = 0, ReturnId = 0, ReturnNum = 0, ResidId = 0, ResidNum = 0;
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
        SqlDataAdapter DraftAdapter, DocAdapter, ReturnAdapter;
        string ReturnDate = null;
        List<string> CustomerGroupList = new List<string>();
        string[] CustomerGroupsArray;
        InputLanguage original;
        bool SalePrice, DiscountLiner, DiscountEnd = false;
        SqlParameter ReturnDocNum;

        public Frm_002_NewFaktor(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        public Frm_002_NewFaktor(bool del, int ID)
        {
            _del = del;
            _ID = ID;
            InitializeComponent();
        }

        private void Frm_002_PishFaktor_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_Sale.Table_032_GoodPrice' table. You can move, or remove it, as needed.

            ToastNotification.ToastBackColor = Color.Aquamarine;
            ToastNotification.ToastForeColor = Color.Black;
            ReturnDocNum = new SqlParameter("ReturnDocNum", SqlDbType.Int);
            ReturnDocNum.Direction = ParameterDirection.Output;
            string Award = Properties.Settings.Default.AwardCompute;
            if (Award == "Box")
            {
                chk_Award_Box.Checked = true;
                chk_Award_Detial.Checked = false;
            }
            else
            {
                chk_Award_Detial.Checked = true;
                chk_Award_Box.Checked = false;
            }

            mnu_CalculatePrice.Checked = Properties.Settings.Default.SalePriceCompute;

            foreach (GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
                if (col.Key == "Column13" || col.Key == "Column15")
                    col.DefaultValue = Class_BasicOperation._UserName;
                if (col.Key == "Column14" || col.Key == "Column16")
                    col.DefaultValue = Class_BasicOperation.ServerDate();
            }

            DraftAdapter = new SqlDataAdapter("SELECT ColumnId,Column01 from Table_007_PwhrsDraft", ConWare);
            DraftAdapter.Fill(DS, "Draft");
            gridEX1.DropDowns["Draft"].SetDataBinding(DS.Tables["Draft"], "");
            DataTable dt = new DataTable();
            SqlDataAdapter ProjectAdapter = new SqlDataAdapter("SELECT * from Table_035_ProjectInfo", ConBase);
            ProjectAdapter.Fill(dt);
            gridEX1.DropDowns["project"].SetDataBinding(dt, "");
            DocAdapter = new SqlDataAdapter("Select ColumnId,Column00 from Table_060_SanadHead", ConAcnt);
            DocAdapter.Fill(DS, "Doc");
            gridEX1.DropDowns["Doc"].SetDataBinding(DS.Tables["Doc"], "");

            ReturnAdapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_018_MarjooiSale", ConSale);
            ReturnAdapter.Fill(DS, "Return");
            gridEX1.DropDowns["Return"].SetDataBinding(DS.Tables["Return"], "");



            DataTable dt1 = new DataTable();
            SqlDataAdapter classAdapter = new SqlDataAdapter("SELECT * from Table_000_Class", ConSale);
            classAdapter.Fill(dt1);
            gridEX1.DropDowns["Gride"].SetDataBinding(dt1, "");

            GoodbindingSource.DataSource = clGood.MahsoolInfo(0);
            DataTable GoodTable = clGood.MahsoolInfo( 0);
            gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            DataTable CustomerTable = clDoc.ReturnTable
            (ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
                                           dbo.Table_045_PersonInfo.Column01 AS code,
                                           dbo.Table_045_PersonInfo.Column02 AS NAME,
                                           dbo.Table_065_CityInfo.Column02 AS shahr,
                                           dbo.Table_060_ProvinceInfo.Column01 AS ostan,
                                           dbo.Table_045_PersonInfo.Column06 AS ADDRESS,
                                           dbo.Table_045_PersonInfo.Column30,
                                           Table_045_PersonInfo.Column07,
                                           Table_045_PersonInfo.Column19 AS Mobile,dbo.Table_045_PersonInfo.Column146
                                    FROM   dbo.Table_045_PersonInfo
                                           LEFT JOIN dbo.Table_065_CityInfo
                                                ON  dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                                           LEFT JOIN dbo.Table_060_ProvinceInfo
                                                ON  dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00
                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");
            gridEX1.DropDowns["Customer"].SetDataBinding(CustomerTable, "");
            gridEX1.DropDowns["Tel"].SetDataBinding(CustomerTable, "");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from  Table_055_CurrencyInfo");
            gridEX1.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");




            gridEX1.DropDowns["Ware"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from Table_001_PWHRS where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + "')");
            gridEX1.DropDowns["Func"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from table_005_PwhrsOperation where Column16=1");



            //SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_070_CountUnitInfo", ConBase);
            //Adapter.Fill(DS, "CountUnit");
            //gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

            SqlDataAdapter Adapter = new SqlDataAdapter("Select * FROM Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(DS, "Center");
            gridEX_List.DropDowns["Center"].SetDataBinding(DS.Tables["Center"], "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_035_ProjectInfo", ConBase);
            Adapter.Fill(DS, "Project");
            gridEX_List.DropDowns["Project"].SetDataBinding(DS.Tables["Project"], "");


            gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)"), "");
            gridEX_Extra.DropDowns["Person"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_045_PersonInfo where Column12=1"), "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
            Adapter.Fill(DS, "Discount");
            gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");

            gridEX1.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes"), "");
            gridEX1.DropDowns["OrderNum"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_005_OrderHeader"), "");
            gridEX1.DropDowns["Prefactor"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select Columnid,Column01 from Table_007_FactorBefore"), "");
            SalePrice = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 162);
            DiscountLiner = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 163);
            DiscountEnd = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 164);
            if (_ID != 0)
            {
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, _ID);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, _ID);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, _ID);
                table_010_SaleFactorBindingSource_PositionChanged(sender, e);

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
            try
            {
                SalePrice = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 162);
                DiscountLiner = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 163);
                DiscountEnd = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 164);

                if (DiscountEnd)
                {
                    gridEX_Extra.Enabled = true;
                    bt_AddExtraDiscounts.Enabled = true;
                }
                else
                {
                    gridEX_Extra.Enabled = false;
                    bt_AddExtraDiscounts.Enabled = false;

                }
            }
            catch
            {
            }
            this.WindowState = FormWindowState.Maximized;

        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.Enabled = true;
                dataSet_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, 0);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, 0);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, 0);
                dataSet_Sale.EnforceConstraints = true;
                gridEX1.MoveToNewRecord();
                //gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "Column01").ToString());
                gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd"));
                gridEX1.SetValue("Column13", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column14", Class_BasicOperation.ServerDate());
                gridEX1.SetValue("Column15", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column16", Class_BasicOperation.ServerDate());



                using (SqlConnection ConWHRS = new SqlConnection(Properties.Settings.Default.WHRS))
                {

                    ConWHRS.Open();
                    SqlCommand Command = new SqlCommand("Select top 1 columnid from Table_001_PWHRS  ", ConWHRS);
                    gridEX1.SetValue("Column42", Convert.ToInt16(Command.ExecuteScalar()));
                }

                using (SqlConnection ConWHRS = new SqlConnection(Properties.Settings.Default.WHRS))
                {

                    ConWHRS.Open();
                    SqlCommand Command = new SqlCommand("Select top 1 columnid from table_005_PwhrsOperation where column16=1  ", ConWHRS);
                    gridEX1.SetValue("Column43", Convert.ToInt16(Command.ExecuteScalar()));
                }

                if (Properties.Settings.Default.Ware != string.Empty)
                    gridEX1.SetValue("Column42", Properties.Settings.Default.Ware);

                try
                {
                    GoodbindingSource.DataSource = clGood.MahsoolInfo( 0);

                    DataTable GoodTable = (Properties.Settings.Default.ShowMojodi ? clGood.MahsoolInfoForNewFactor(gridEX1.GetValue("Column02").ToString(), gridEX1.GetValue("Column42")) : clGood.MahsoolInfo( 0));

                    gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                    gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                }
                catch
                {
                }


                if (Properties.Settings.Default.Masool != string.Empty)
                {
                    gridEX1.SetValue("Column05", Properties.Settings.Default.Masool);
                    try
                    {
                        double dd = 0;
                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
                        {
                            Con.Open();
                            SqlCommand Comm = new SqlCommand("SELECT tcc.Column143 FROM   Table_045_PersonInfo tcc WHERE  tcc.ColumnId=" + gridEX1.GetValue("column05") + " ", Con);
                            dd = Convert.ToDouble(Comm.ExecuteScalar());

                        }

                        gridEX1.SetValue("Column55", dd);
                    }
                    catch
                    {
                    }
                }

                if (gridEX1.GetRow().Cells["Column06"].Text.Trim() == "")
                    gridEX1.SetValue("Column06", Properties.Settings.Default.SaleDescription);
                gridEX1.RootTable.Columns["Column40"].Selectable = false;
                gridEX1.RootTable.Columns["Column41"].Selectable = false;

                gridEX1.Select();

                bt_New.Enabled = false;

                gridEX1.AllowEdit = InheritableBoolean.True;
                gridEX1.AllowAddNew = InheritableBoolean.True;
                gridEX_List.AllowAddNew = InheritableBoolean.True;
                gridEX_List.AllowEdit = InheritableBoolean.True;
                gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                gridEX_Extra.AllowDelete = InheritableBoolean.True;
                gridEX_List.AllowDelete = InheritableBoolean.True;

                foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                {
                    if (item.Key == "column07" || item.Key == "column08" || item.Key == "Column09" || item.Key == "Column10" ||
                        item.Key == "column17" || item.Key == "column19" || item.Key == "column20")
                        item.Selectable = false;
                    else item.Selectable = true;
                }
                gridEX1.Col = 4;
                var culture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");
                var language = InputLanguage.FromCulture(culture);
                InputLanguage.CurrentInputLanguage = language;
                foreach (Janus.Windows.GridEX.GridEXColumn item in this.gridEX_List.RootTable.Columns)
                {
                    if (item.Key == "column20")
                        item.Selectable = false;
                    if (item.Key == "column11")
                        item.Selectable = false;
                    if (item.Key == "column08" || item.Key == "column09" || item.Key == "column10")
                    {
                        if (SalePrice)
                            item.Selectable = true;
                        else item.Selectable = false;
                    }
                    if (item.Key == "column16" || item.Key == "column17" || item.Key == "column18")
                    {
                        if (DiscountLiner)
                            item.Selectable = true;
                        else
                            item.Selectable = false;

                    }
                }
                if (DiscountEnd)
                {
                    gridEX_Extra.Enabled = true;
                    bt_AddExtraDiscounts.Enabled = true;
                }
                else
                {
                    gridEX_Extra.Enabled = false;
                    bt_AddExtraDiscounts.Enabled = false;

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }
        private void export()
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 69))
                        throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");



                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID) != 0)
                    {
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                        dataSet_Sale.EnforceConstraints = true;
                        DS.Tables["Doc"].Clear();
                        DocAdapter.Fill(DS, "Doc");
                        DS.Tables["Draft"].Clear();
                        DraftAdapter.Fill(DS, "Draft");
                        this.table_010_SaleFactorBindingSource_PositionChanged(null, null);

                        throw new Exception("برای این فاکتور سند حسابداری صادر شده است");
                    }

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                        throw new Exception("به علت باطل شدن این فاکتور امکان صدور سند وجود ندارد");



                    //بعد از سیو کردن اطلاعات سطر خالی می شود
                    DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                    if (Row["Column12"].ToString() == "True")
                    {
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                        {
                            if (item.Cells["Column14"].Text.Trim() == "" || Convert.ToDouble(item.Cells["Column15"].Value.ToString()) <= 0)
                                throw new Exception("نوع ارز و ارزش ارز اقلام فاکتور را مشخص کنید");
                        }
                    }





                    if (gridEX1.GetRow().Cells["Column42"].Text.Trim() != "" && gridEX1.GetRow().Cells["Column43"].Text.Trim() != "")
                    {
                        if (Row["Column09"].ToString().Trim() != "" && int.Parse(Row["Column09"].ToString()) > 0)
                        {
                            //***************************if Finance Type is Periodic: Just export Doc for factor and Doc number will be save in Draft's n
                            //سیستم ادواری
                            if (!Class_BasicOperation._FinType)
                            {
                                _05_Sale.Frm_009_ExportDocInformation frm = new Frm_009_ExportDocInformation(true, false, false, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"]), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"]));
                                frm.ShowDialog();
                            }
                            //سیستم دائمی
                            else
                            {
                                _05_Sale.Frm_009_ExportDocInformation frm = new Frm_009_ExportDocInformation(true, false, true, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"]), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"]));
                                frm.ShowDialog();
                            }

                        }
                        //اگر حواله صادر نشده باشد
                        else
                        {
                            bool _DraftPanel = true;
                            if (clDoc.AllService(table_011_Child1_SaleFactorBindingSource))
                                _DraftPanel = false;

                            //سیستم ادواری
                            if (!Class_BasicOperation._FinType)
                            {
                                _05_Sale.Frm_009_ExportDocInformation frm = new Frm_009_ExportDocInformation(true, _DraftPanel, false, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"]), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"]));
                                frm.ShowDialog();
                            }
                            //سیستم دائمی
                            else
                            {
                                _05_Sale.Frm_009_ExportDocInformation frm = new Frm_009_ExportDocInformation(true, _DraftPanel, true, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"]), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"]));
                                frm.ShowDialog();
                            }
                        }
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                        dataSet_Sale.EnforceConstraints = true;
                        DS.Tables["Doc"].Clear();
                        DocAdapter.Fill(DS, "Doc");
                        DS.Tables["Draft"].Clear();
                        DraftAdapter.Fill(DS, "Draft");
                        this.table_010_SaleFactorBindingSource_PositionChanged(null, null);

                    }
                    else Class_BasicOperation.ShowMsg("", "انبار و نوع حواله را مشخص کنید", "None");

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }
        private void Save_Event(object sender, EventArgs e)
        {
            gridEX_List.UpdateData();
            gridEX_Extra.UpdateData();
            gridEX1.UpdateData();
            if (this.table_010_SaleFactorBindingSource.Count > 0 &&
               gridEX_List.AllowEdit == InheritableBoolean.True &&
               gridEX1.GetRow().Cells["Column03"].Text.Trim() != "")
            {
                if (Properties.Settings.Default.ShowPriceAlert > 0)
                    CheckGoodsPrice();
                this.Cursor = Cursors.WaitCursor;

                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }

                DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;
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
                    gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "Column01").ToString());
                    Row["Column61"] = 0;
                    this.table_010_SaleFactorBindingSource.EndEdit();
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
                        item.Cells["Column04"].Value = (gridEX1.GetValue("Column12").ToString() == "True" ?
                            Total * double.Parse(item.Cells["Column03"].Value.ToString()) / 100 :
                            Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100))
                            ;
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

                //****************Calculate Discounts

                double NetTotal = Convert.ToDouble(gridEX_List.GetTotal(
                    gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
                int CustomerCode = int.Parse(Row["Column03"].ToString());
                string Date = Row["Column02"].ToString();
                Row["Column28"] = NetTotal;
                if (Row["Column12"].ToString() == "False")
                {
                    NetTotal = ClDiscount.SpecialGroup(
                        Convert.ToDouble(Row["Column28"].ToString()), CustomerCode, Date);
                    Row["Column30"] = NetTotal;

                    NetTotal = ClDiscount.VolumeGroup(Convert.ToDouble(Row["Column28"].ToString()) -
                        Convert.ToDouble(Row["Column30"].ToString()), CustomerCode, Date);
                    Row["Column29"] = NetTotal;

                    object Value = gridEX1.GetValue("Column36");
                    if (Value != null && !string.IsNullOrWhiteSpace(Value.ToString()))
                    {
                        NetTotal = ClDiscount.SpecialCustomer(
                              Convert.ToDouble(Row["Column28"].ToString()) -
                              Convert.ToDouble(Row["Column30"].ToString()) -
                              Convert.ToDouble(Row["Column29"].ToString()), Convert.ToInt32(gridEX1.GetValue("Column36")));
                    }

                    if (NetTotal == Convert.ToDouble(0))
                        NetTotal = ClDiscount.SpecialCustomer(
                            Convert.ToDouble(Row["Column28"].ToString()) -
                            Convert.ToDouble(Row["Column30"].ToString()) -
                            Convert.ToDouble(Row["Column29"].ToString()), CustomerCode, Date);
                    Row["Column31"] = NetTotal;
                }

                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter2 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                Row["Column32"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                Filter2.Value1 = true;
                Row["Column33"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                ////////
                if (Convert.ToBoolean(gridEX1.GetValue("Column56")) == true)
                {
                    string msg = "موجودی کالا(های) زیر بیشتر از مقدار فروش امانی می باشد:";
                    int good = 0;
                    //calculate mojodi factor amani 
                    foreach (Janus.Windows.GridEX.GridEXRow dr in gridEX_List.GetRows())
                    {
                        double mojodiAmani = Convert.ToDouble(clDoc.ExScalarQuery(ConSale.ConnectionString, @"select isnull((SELECT     SUM(dbo.Table_075_Child_AmaniFactor.column07) AS Count
                        FROM         dbo.Table_070_AmaniFactor INNER JOIN
                                              dbo.Table_075_Child_AmaniFactor ON dbo.Table_070_AmaniFactor.columnid = dbo.Table_075_Child_AmaniFactor.column01
                        where  dbo.Table_070_AmaniFactor.Column02<='" + gridEX1.GetValue("Column02").ToString() + @"'
                        GROUP BY dbo.Table_075_Child_AmaniFactor.column02, dbo.Table_070_AmaniFactor.column03
                        HAVING      (dbo.Table_070_AmaniFactor.column03 = " + gridEX1.GetValue("Column03").ToString() + ") AND (dbo.Table_075_Child_AmaniFactor.column02 = " + dr.Cells["Column02"].Value + ") ),0) "));
                        double mojodimarjoie = Convert.ToDouble(clDoc.ExScalarQuery(ConSale.ConnectionString, @"select isnull((SELECT     SUM(dbo.Table_085_ReturnAmaniChild.column07) AS Count
                        FROM         dbo.Table_080_ReturnAmani INNER JOIN
                                              dbo.Table_085_ReturnAmaniChild ON dbo.Table_080_ReturnAmani.columnid = dbo.Table_085_ReturnAmaniChild.column01
                        where  dbo.Table_080_ReturnAmani.Column02<='" + gridEX1.GetValue("Column02").ToString() + @"'
                        GROUP BY dbo.Table_085_ReturnAmaniChild.column02, dbo.Table_080_ReturnAmani.column03
                        HAVING      (dbo.Table_080_ReturnAmani.column03 = " + gridEX1.GetValue("Column03").ToString() + ") AND (dbo.Table_085_ReturnAmaniChild.column02 = " + dr.Cells["Column02"].Value + ") ),0) "));

                        double mojodiForosh = Convert.ToDouble(clDoc.ExScalarQuery(ConSale.ConnectionString, @"select isnull((SELECT     SUM(dbo.Table_011_Child1_SaleFactor.column07) AS Count
                        FROM         dbo.Table_010_SaleFactor INNER JOIN
                                              dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                        WHERE     (dbo.Table_010_SaleFactor.column03 =  " + gridEX1.GetValue("Column03").ToString() + @") AND (NOT (dbo.Table_010_SaleFactor.column01 IN (" + gridEX1.GetValue("Column01").ToString() + @")))   and  dbo.Table_010_SaleFactor.column02<='" + gridEX1.GetValue("Column02").ToString() + @"' and dbo.Table_010_SaleFactor.Column56=1

                        GROUP BY dbo.Table_011_Child1_SaleFactor.column02
                        HAVING      (dbo.Table_011_Child1_SaleFactor.column02 = " + dr.Cells["Column02"].Value + ")),0)"));
                        if ((mojodiAmani - mojodimarjoie) - mojodiForosh > 0)
                        {
                            if (Convert.ToDouble(dr.Cells["Column07"].Value) > (mojodiAmani - mojodimarjoie) - mojodiForosh)
                            { msg = msg + dr.Cells["Column02"].Text + "-"; good = good + 1; }

                        }
                        else { msg = msg + dr.Cells["Column02"].Text + "-"; good = good + 1; }

                    }
                    if (good > 0)
                    {
                        this.Cursor = Cursors.Default; throw new Exception(msg);

                        return;
                    }

                }
                ////////
                this.table_010_SaleFactorBindingSource.EndEdit();
                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_Sale.Table_011_Child1_SaleFactor);
                this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_Sale.Table_012_Child2_SaleFactor);

                if (Row["Column09"].ToString() == "0")
                {
                    if (Row["Column12"].ToString() == "False")
                    {
                        //اگر نمایش پیغام تنظیم شده باشد آن را نشان می دهد
                        //در غیر این صورت بدون نمایش پیغام عملیات محاسبه جایزه را به صورت اتومات انجام می دهد
                        if (Properties.Settings.Default.ShowCalculateGiftDuringSave)
                        {
                            if (DialogResult.Yes == MessageBox.Show("آیا مایل به محاسبه جوایز هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                (sender == bt_Save || sender == this ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2), MessageBoxOptions.RightAlign))
                                if (chk_Award_Box.Checked)
                                    Classes.Class_Award.SaleAward_Box(int.Parse(Row["ColumnId"].ToString()), Row["Column02"].ToString(), (Row["Column07"].ToString() == "" ? 0 : int.Parse(Row["Column07"].ToString())), mnu_CalculatePrice.Checked);
                                else Classes.Class_Award.SaleAward_Detial(int.Parse(Row["ColumnId"].ToString()), Row["Column02"].ToString(), (Row["Column07"].ToString() == "" ? 0 : int.Parse(Row["Column07"].ToString())));
                        }
                        else
                        {
                            if (chk_Award_Box.Checked)
                                Classes.Class_Award.SaleAward_Box(int.Parse(Row["ColumnId"].ToString()), Row["Column02"].ToString(), (Row["Column07"].ToString() == "" ? 0 : int.Parse(Row["Column07"].ToString())), mnu_CalculatePrice.Checked);
                            else Classes.Class_Award.SaleAward_Detial(int.Parse(Row["ColumnId"].ToString()), Row["Column02"].ToString(), (Row["Column07"].ToString() == "" ? 0 : int.Parse(Row["Column07"].ToString())));
                        }
                    }
                }
                if (sender == bt_Save || sender == this)
                    Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                int _ID = int.Parse(Row["ColumnId"].ToString());
                dataSet_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, _ID);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, _ID);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, _ID);
                dataSet_Sale.EnforceConstraints = true;
                table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                DataTable GoodTable = clGood.MahsoolInfoForFactor(null, null);
                gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                bt_New.Enabled = true;
                this.Cursor = Cursors.Default;



            }
            else
            {
                this.table_010_SaleFactorBindingSource.EndEdit();
                this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Save_Event(sender, e);

                DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                if (!Row["Column01"].ToString().StartsWith("-"))
                {
                    Properties.Settings.Default.Ware = gridEX1.GetValue("Column42").ToString();
                    Properties.Settings.Default.Masool = gridEX1.GetValue("Column05").ToString();
                    Properties.Settings.Default.Save();
                }
                if (Properties.Settings.Default.ShowExportSanad)
                    export();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void CheckGoodsPrice()
        {
            List<string> Codes = new List<string>();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
            {
                Codes.Add(item.Cells["Column02"].Value.ToString());
            }

            DataTable Table = clDoc.ReturnTable(ConWare.ConnectionString, @"declare @t table(GoodCode int,Date nvarchar(50), Price decimal(18,3));
            insert into @t SELECT     Table_012_Child_PwhrsReceipt.column02,  MAX(Table_011_PwhrsReceipt.column02) AS Date,1
            FROM         Table_012_Child_PwhrsReceipt INNER JOIN
            Table_011_PwhrsReceipt ON Table_012_Child_PwhrsReceipt.column01 = Table_011_PwhrsReceipt.columnid
            where Table_012_Child_PwhrsReceipt.column02 in (" + string.Join(",", Codes.ToArray()) + @")
            GROUP BY Table_012_Child_PwhrsReceipt.column02
            order by Table_012_Child_PwhrsReceipt.column02;
            
            declare @t2 table(codekala2 int , gheymat2 decimal(18,3),date2 nvarchar(50)
            ,UNIQUE (codekala2,gheymat2,date2)
            );

            insert into @t2 SELECT   dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.column10, 
            dbo.Table_011_PwhrsReceipt.column02 AS Date
            FROM         dbo.Table_012_Child_PwhrsReceipt INNER JOIN
            dbo.Table_011_PwhrsReceipt ON dbo.Table_012_Child_PwhrsReceipt.column01 = dbo.Table_011_PwhrsReceipt.columnid
            where Table_012_Child_PwhrsReceipt.column02 in (" + string.Join(",", Codes.ToArray()) + @")
            GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.column10, dbo.Table_011_PwhrsReceipt.column02;
            update @t set Price=gheymat2 from @t2 as main_table where GoodCode=codekala2 and Date=date2; 
            select * from @t");

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
            {
                DataRow[] Row = Table.Select("GoodCode=" + item.Cells["Column02"].Value.ToString());

                if (Row.Length > 0)
                {
                    if (Properties.Settings.Default.ShowPriceAlert == 2)
                    {
                        if (Convert.ToDouble(Row[0]["Price"].ToString()) > Convert.ToDouble(item.Cells["Column10"].Value.ToString()))
                            throw new Exception("قیمت مندرج برای کالای " + item.Cells["Column02"].Text + Environment.NewLine + " کمتر از آخرین قیمت است" +
                                Environment.NewLine + " آخرین ورود این کالا در تاریخ " + Row[0]["Date"].ToString() + " و با قیمت " +
                                Convert.ToDouble(Row[0]["Price"].ToString()).ToString("#,##0.###") + " صورت گرفته است");
                    }
                    else
                    {
                        if (Convert.ToDouble(Row[0]["Price"].ToString()) > Convert.ToDouble(item.Cells["Column10"].Value.ToString()))
                            Class_BasicOperation.ShowMsg("", "قیمت مندرج برای کالای " + item.Cells["Column02"].Text + Environment.NewLine + " کمتر از آخرین قیمت است" +
                                    Environment.NewLine + " آخرین ورود این کالا در تاریخ " + Row[0]["Date"].ToString() + " و با قیمت " +
                                    Convert.ToDouble(Row[0]["Price"].ToString()).ToString("#,##0.###") + " صورت گرفته است", "Warning");
                    }
                }
            }



        }

        public void bt_Del_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!_del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", RowID) != 0)
                    {
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                        dataSet_Sale.EnforceConstraints = true;
                        throw new Exception("به علت ارجاع این فاکتور، حذف آن امکانپذیر نمی باشد");
                    }

                    if (clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_034_SaleFactor_Child3 where Column01=" + RowID).Rows.Count > 0)
                        throw new Exception("این فاکتور دارای اطلاعات مربوط به تسویه است. جهت حذف فاکتور ابتدا، اطلاعات مربوطه را حذف کنید");

                    int DocId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID);
                    int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID);
                    int PrefactorId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column07", RowID);
                    DataTable Table = new DataTable();
                    if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        string command = string.Empty;
                        int i = 0;
                        if (DocId > 0)
                        {
                            clDoc.IsFinal_ID(DocId);
                            //حذف سند فاکتور 

                            //i = clDoc.DeleteDetail_ID(DocId, 15, int.Parse(RowID));

                            Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + int.Parse(RowID));
                            foreach (DataRow item in Table.Rows)
                            {
                                command += "Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += "Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + int.Parse(RowID);




                            //حذف سند مربوط به حواله
                            //clDoc.DeleteDetail_ID(DocId, 26, DraftId);

                            Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + DraftId);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += "Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += "Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + DraftId;


                        }
                        //if ((i > 0 && DocId > 0) || DocId == 0)
                        //{
                        //حذف فاکتور

                        command += " Delete from " + ConSale.Database + ".dbo.Table_012_Child2_SaleFactor  Where column01 =" + int.Parse(RowID);
                        command += " Delete from " + ConSale.Database + ".dbo.Table_011_Child1_SaleFactor  Where column01 =" + int.Parse(RowID);
                        command += " Delete from " + ConSale.Database + ".dbo.Table_010_SaleFactor  Where columnid =" + int.Parse(RowID);


                        //foreach (DataRowView item in this.table_011_Child1_SaleFactorBindingSource)
                        //{
                        //    item.Delete();
                        //}
                        //this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                        //this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_Sale.Table_011_Child1_SaleFactor);
                        //foreach (DataRowView item in this.table_012_Child2_SaleFactorBindingSource)
                        //{
                        //    item.Delete();
                        //}
                        //this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                        //this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_Sale.Table_012_Child2_SaleFactor);
                        //this.table_010_SaleFactorBindingSource.RemoveCurrent();
                        //this.table_010_SaleFactorBindingSource.EndEdit();
                        //this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);




                        if (DraftId > 0)
                        {
                            //درج صفر در شماره سند حواله و صفر در شماره فاکتور فروش حواله
                            //clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_007_PwhrsDraft SET Column07=0 , Column16=0 where ColumnId=" + DraftId + "AND Column16= " + RowID);
                            command += "UPDATE " + ConWare.Database + ".dbo.Table_007_PwhrsDraft SET Column07=0 , Column16=0 where ColumnId=" + DraftId + "AND Column16= " + RowID;


                        }
                        if (PrefactorId > 0)
                        {
                            //درج صفر در شماره فاکتور فروش پیش فاکتور
                            //clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_007_FactorBefore set Column12=0 where ColumnId=" + PrefactorId);
                            command += "UPDATE " + ConSale.Database + ".dbo.Table_007_FactorBefore set Column12=0 where ColumnId=" + PrefactorId;

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
                                dataSet_Sale.EnforceConstraints = false;
                                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, 0);
                                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, 0);
                                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, 0);
                                dataSet_Sale.EnforceConstraints = true;

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


                        //}
                        //else
                        //    Class_BasicOperation.ShowMsg("", "حذف فاکتور فروش یه دلیل عدم حذف سند مربوطه انجام نشد", "Information");

                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
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

                table_010_SaleFactorBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                gridEX1.Focus();
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Extra_Enter(object sender, EventArgs e)
        {
            try
            {

                table_010_SaleFactorBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                gridEX1.Focus();
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
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
                    gridEX_Extra.SetValue("column04",
                        (gridEX1.GetValue("Column12").ToString() == "False") ?
                        Convert.ToInt64(kol * darsad / 100) : kol * darsad / 100);
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
                       (gridEX1.GetValue("Column12").ToString() == "False") ?
                       Convert.ToInt64(kol * darsad / 100) : kol * darsad / 100);
            }
        }

        private void gridEX_Extra_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_List_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_List.CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column02", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

                else if (e.Column.Key == "GoodCode")
                    Class_BasicOperation.FilterGridExDropDown(sender, "GoodCode", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);
            }
            catch { }
        }

        private void gridEX_Extra_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_Extra.CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "column07")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column07", "Column01", "Column02", gridEX_Extra.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);


            }
            catch { }
        }

        private void table_010_SaleFactorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    ResidId = 0;
                    ResidNum = 0;
                    ReturnId = 0;
                    ReturnNum = 0;
                    ReturnDate = null;


                    DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;


                    ///اگر فاکتور فروش حواله داشته باشد ولی انبار و عملکرد نداشته باشد

                    if (Row["Column09"].ToString() != "0" && (string.IsNullOrWhiteSpace(Row["Column42"].ToString()) || string.IsNullOrWhiteSpace(Row["Column43"].ToString())))
                    {
                        DataTable dt = new DataTable();
                        DraftAdapter = new SqlDataAdapter("SELECT * from Table_007_PwhrsDraft where columnid=" + Row["Column09"] + "", ConWare);
                        DraftAdapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            Row["Column42"] = dt.Rows[0]["column03"];
                            Row["Column43"] = dt.Rows[0]["column04"];
                        }
                        else
                        {
                            Class_BasicOperation.ShowMsg("", "حواله  وجود ندارد", "Information");
                            return;

                        }

                    }




                    //اگر برای فاکتور فقط حواله صادر شده باشد 
                    if (Row["Column09"].ToString() != "0" && Row["Column10"].ToString() == "0")
                    {
                        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                        {
                            if (item.Key == "column06" || item.Key == "column13" ||
                                item.Key == "Column40" || item.Key == "Column41" || item.Key == "column12" || item.Key == "column05" || item.Key == "column21" || item.Key == "column22" || item.Key == "column23" || item.Key == "column24"
                                 || item.Key == "column25" || item.Key == "column26" || item.Key == "column27" || item.Key == "Column36")
                                item.Selectable = true;
                            else item.Selectable = false;

                        }
                        gridEX1.Enabled = true;
                        gridEX_List.AllowAddNew = InheritableBoolean.False;
                        gridEX_List.AllowEdit = InheritableBoolean.True;
                        gridEX_List.AllowDelete = InheritableBoolean.False;

                        gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                        gridEX_Extra.AllowEdit = InheritableBoolean.True;
                        gridEX_Extra.AllowDelete = InheritableBoolean.True;

                        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX_List.RootTable.Columns)
                        {
                            if (item.Key == "column02" || item.Key == "column03" ||
                                item.Key == "column04" || item.Key == "column05" || item.Key == "column06" || item.Key == "column07"
                                || item.Key == "GoodCode" || item.Key == "column20" || item.Key == "column11"
                                )
                                item.Selectable = false;
                            else item.Selectable = true;

                            if (item.Key == "column08" || item.Key == "column09" || item.Key == "column10")
                            {
                                if (SalePrice)
                                    item.Selectable = true;
                                else item.Selectable = false;
                            }
                            if (item.Key == "column16" || item.Key == "column17" || item.Key == "column18")
                            {
                                if (DiscountLiner)
                                    item.Selectable = true;
                                else
                                    item.Selectable = false;

                            }


                        }


                    }
                    //در صورت اینکه فاکتور دارای سند باشد، یا مرجوعی باشد یا باطل شده باشد یا دارای پیش فاکتور باشد
                    else if (Row["Column10"].ToString().Trim() != "0"
                        //|| Row["Column07"].ToString().Trim() != "0"
                            || Row["Column17"].ToString().Trim() != "False"
                            || Row["Column19"].ToString().Trim() != "False")
                    {
                        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                        {
                            if (item.Key == "column06" || item.Key == "column13" || item.Key == "Column40" || item.Key == "Column41"
                                || item.Key == "column05" || item.Key == "column21" || item.Key == "column22" || item.Key == "column23" || item.Key == "column24"
                                 || item.Key == "column25" || item.Key == "column26" || item.Key == "column27")
                                item.Selectable = true;
                            else item.Selectable = false;
                        }
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
                        gridEX1.Enabled = true;
                        gridEX1.AllowEdit = InheritableBoolean.True;
                        gridEX_List.AllowEdit = InheritableBoolean.True;
                        gridEX_Extra.AllowEdit = InheritableBoolean.True;
                        gridEX_List.AllowAddNew = InheritableBoolean.True;
                        gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                        gridEX_Extra.AllowDelete = InheritableBoolean.True;
                        gridEX_List.AllowDelete = InheritableBoolean.True;

                        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                        {
                            if (item.Key == "column07" || item.Key == "column08" || item.Key == "Column09" || item.Key == "Column10" ||
                                item.Key == "column17" || item.Key == "column19" || item.Key == "column20")
                                item.Selectable = false;
                            else item.Selectable = true;
                        }

                        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX_List.RootTable.Columns)
                        {
                            if (item.Key == "column07" ||
                                item.Key == "Column36" || item.Key == "Column37" || item.Key == "column11" || item.Key == "column19"
                                || item.Key == "column30" || item.Key == "column20"
                                )
                                item.Selectable = false;
                            else item.Selectable = true;

                            if (item.Key == "column08" || item.Key == "column09" || item.Key == "column10")
                            {
                                if (SalePrice)
                                    item.Selectable = true;
                                else item.Selectable = false;
                            }
                            if (item.Key == "column16" || item.Key == "column17" || item.Key == "column18")
                            {
                                if (DiscountLiner)
                                    item.Selectable = true;
                                else
                                    item.Selectable = false;

                            }


                        }
                    }

                    if (Convert.ToInt32(Row["ColumnId"]) > 0)
                    {
                        btn_Seller.Enabled = true;
                        btn_Dealer.Enabled = true;
                    }
                    else
                    {
                        btn_Seller.Enabled = false;
                        btn_Dealer.Enabled = false;
                    }


                }
                else
                {
                    btn_Seller.Enabled = false;
                    btn_Dealer.Enabled = false;

                }
            }
            catch
            { }
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
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
            }
            catch
            {
            }
            try
            {
                if (table_010_SaleFactorBindingSource.Count > 0 && gridEX_List.GetRows().Length > 0)
                {
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(item.Cells["GoodCode"].Value.ToString())), "");
                        gridEX_List.CurrentRow.Format();
                        gridEX_List.Refresh();
                    }
                }

                if (DiscountEnd)
                {
                    gridEX_Extra.Enabled = true;
                    bt_AddExtraDiscounts.Enabled = true;
                }
                else
                {
                    gridEX_Extra.Enabled = false;
                    bt_AddExtraDiscounts.Enabled = false;

                }
            }
            catch
            {
            }


        }

        private void bt_ExportDraft_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 71))
                        throw new Exception("کاربر گرامی شما امکان صدور حواله انبار را ندارید");

                    Save_Event(sender, e);

                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID) != 0)
                        throw new Exception("برای این فاکتور حواله صادر شده است");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                        throw new Exception("به علت باطل شدن این فاکتور امکان صدور حواله وجود ندارد");

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", RowID) != 0)
                        throw new Exception("به علت مرجوع شدن این فاکتور امکان صدور حواله انبار وجود ندارد");

                    if (clDoc.AllService(table_011_Child1_SaleFactorBindingSource))
                        throw new Exception("به علت عدم وجود کالا، حواله ای برای این فاکتور صادر نخواهد شد");




                    DataTable Table = new DataTable();
                    Table.Columns.Add("GoodID", Type.GetType("System.String"));
                    Table.Columns.Add("GoodName", Type.GetType("System.String"));
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        Table.Rows.Add(item.Cells["Column02"].Value,
                            item.Cells["Column02"].Text);
                    }

                    if (gridEX1.GetRow().Cells["Column42"].Text.Trim() != "" && gridEX1.GetRow().Cells["Column43"].Text.Trim() != "")
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
                                                                          AND tuai.Column02 = " + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                                ok = int.Parse(Commnad.ExecuteScalar().ToString());

                            }
                            if (ok == 0)
                            {
                                dataSet_Sale.EnforceConstraints = false;
                                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, 0);
                                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, 0);
                                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, 0);
                                dataSet_Sale.EnforceConstraints = true;
                                MessageBox.Show("به انبار این فاکتور دسترسی ندارید");
                                return;

                            }


                        }
                        catch
                        {
                        }
                        _05_Sale.Frm_010_NewDraftInformationDialog frm = new Frm_010_NewDraftInformationDialog(this.table_011_Child1_SaleFactorBindingSource, ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current),
                            ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column02"].ToString(),
                            Table, Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"]),
                            Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"]));
                        frm.ShowDialog();
                    }
                    else Class_BasicOperation.ShowMsg("", "انبار و نوع حواله را مشخص کنید", "None");

                }

                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                DS.Tables["Draft"].Clear();
                DraftAdapter.Fill(DS, "Draft");
                int _ID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                dataSet_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, _ID);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, _ID);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, _ID);
                dataSet_Sale.EnforceConstraints = true;
                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

            }
        }

        private void bt_Export_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 69))
                        throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");
                    Save_Event(sender, e);

                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID) != 0)
                    {
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                        dataSet_Sale.EnforceConstraints = true;
                        DS.Tables["Doc"].Clear();
                        DocAdapter.Fill(DS, "Doc");
                        DS.Tables["Draft"].Clear();
                        DraftAdapter.Fill(DS, "Draft");
                        this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                        throw new Exception("برای این فاکتور سند حسابداری صادر شده است");
                    }

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                        throw new Exception("به علت باطل شدن این فاکتور امکان صدور سند وجود ندارد");


                    //بعد از سیو کردن اطلاعات سطر خالی می شود
                    DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                    if (Row["Column12"].ToString() == "True")
                    {
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                        {
                            if (item.Cells["Column14"].Text.Trim() == "" || Convert.ToDouble(item.Cells["Column15"].Value.ToString()) <= 0)
                                throw new Exception("نوع ارز و ارزش ارز اقلام فاکتور را مشخص کنید");
                        }
                    }

                    if (gridEX1.GetRow().Cells["Column42"].Text.Trim() != "" && gridEX1.GetRow().Cells["Column43"].Text.Trim() != "")
                    {
                        if (Row["Column09"].ToString().Trim() != "" && int.Parse(Row["Column09"].ToString()) > 0)
                        {

                            //***************************if Finance Type is Periodic: Just export Doc for factor and Doc number will be save in Draft's n

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
                                                                          AND tuai.Column02 = " + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                                    ok = int.Parse(Commnad.ExecuteScalar().ToString());

                                }
                                if (ok == 0)
                                {
                                    dataSet_Sale.EnforceConstraints = false;
                                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, 0);
                                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, 0);
                                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, 0);
                                    dataSet_Sale.EnforceConstraints = true;
                                    MessageBox.Show("به انبار این فاکتور دسترسی ندارید");
                                    return;

                                }


                            }
                            catch
                            {
                            }
                            //سیستم ادواری
                            if (!Class_BasicOperation._FinType)
                            {
                                _05_Sale.Frm_009_NewExportDocInformation frm = new Frm_009_NewExportDocInformation(true, false, false, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"]), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"]));
                                frm.ShowDialog();
                            }
                            //سیستم دائمی
                            else
                            {
                                _05_Sale.Frm_009_NewExportDocInformation frm = new Frm_009_NewExportDocInformation(true, false, true, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"]), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"]));
                                frm.ShowDialog();
                            }

                        }
                        //اگر حواله صادر نشده باشد
                        else
                        {
                            bool _DraftPanel = true;
                            if (clDoc.AllService(table_011_Child1_SaleFactorBindingSource))
                                _DraftPanel = false;

                            //سیستم ادواری
                            if (!Class_BasicOperation._FinType)
                            {
                                _05_Sale.Frm_009_NewExportDocInformation frm = new Frm_009_NewExportDocInformation(true, _DraftPanel, false, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"]), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"]));
                                frm.ShowDialog();
                            }
                            //سیستم دائمی
                            else
                            {
                                _05_Sale.Frm_009_NewExportDocInformation frm = new Frm_009_NewExportDocInformation(true, _DraftPanel, true, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"]), Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"]));
                                frm.ShowDialog();
                            }
                        }
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                        dataSet_Sale.EnforceConstraints = true;
                        DS.Tables["Doc"].Clear();
                        DocAdapter.Fill(DS, "Doc");
                        DS.Tables["Draft"].Clear();
                        DraftAdapter.Fill(DS, "Draft");
                        this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                    }
                    else Class_BasicOperation.ShowMsg("", "انبار و نوع حواله را مشخص کنید", "None");

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
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 70))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    int RowID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int SanadID = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID.ToString());
                    int DraftID = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID.ToString());

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", RowID.ToString()) != 0)
                        throw new Exception("به علت ارجاع این فاکتور، حذف سند حسابداری امکانپذیر نمی باشد");

                    if (SanadID != 0)
                    {
                        string Message = "آیا مایل به حذف سند مربوط به این فاکتور هستید؟";

                        if (DraftID != 0)
                        {
                            Message = "برای این فاکتور، حواله انبار نیز صادر شده است. در صورت تأیید ثبت مربوط به حواله نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف سند این فاکتور هستید؟";
                        }
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.IsFinal_ID(SanadID);

                            DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=15 and Column17=" + RowID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=15 and Column17=" + RowID;



                            Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + DraftID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + DraftID;

                            command += " UPDATE " + ConWare.Database + ".dbo. Table_007_PwhrsDraft SET Column07=0,Column10='" + Class_BasicOperation._UserName + "', Column11=getdate() where ColumnId=" + DraftID;

                            command += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column10=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + RowID;


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
                                    Class_BasicOperation.ShowMsg("", "حذف سند حسابداری با موفقیت صورت گرفت", "Information");

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
                    dataSet_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, RowID);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, RowID);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, RowID);
                    dataSet_Sale.EnforceConstraints = true;
                    DS.Tables["Doc"].Clear();
                    DocAdapter.Fill(DS, "Doc");
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bt_ReturnFactor_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    Save_Event(sender, e);

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 72))
                        throw new Exception("کاربر گرامی شما امکان مرجوع کردن فاکتور فروش را ندارید");

                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", RowID) != 0)
                        throw new Exception("این فاکتور قبلا مرجوع شده است");

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID) == 0)
                        throw new Exception("جهت ارجاع یک فاکتور صدور سند حسابداری و حواله انبار، الزامیست");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به مرجوع کردن این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        //صدور رسید در صورت صادر شدن حواله برای فاکتور فروش
                        DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;
                        InsertReceipt(Row);
                        if (ReturnId > 0 && ResidId > 0)
                        {
                            //ثبت عکس فاکتور فروش
                            InvertDoc(Row);
                            Class_BasicOperation.ShowMsg("", "ارجاع فاکتور با موفقیت انجام شد" + Environment.NewLine + "شماره سند حسابداری:" + ReturnDocNum.Value + Environment.NewLine + "شماره رسید انبار:" + ResidNum, "Information");

                            dataSet_Sale.EnforceConstraints = false;
                            this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                            dataSet_Sale.EnforceConstraints = true;
                            DS.Tables["Return"].Clear();
                            ReturnAdapter.Fill(DS, "Return");
                            this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                        }
                    }


                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void TurnBack(DataRowView Row)
        {
            if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", Row["ColumnId"].ToString()) != 0)
                throw new Exception("برای این فاکتور، فاکتور مرجوعی صادر شده است");

            ReturnDate = InputBox.Show("تاریخ ارجاع را وارد کنید:");
            if (!string.IsNullOrEmpty(ReturnDate))
            {

                //درج هدر مرجوعی
                ReturnNum = clDoc.MaxNumber(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column01");
                ReturnId = 0;
                SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                {
                    Con.Open();
                    Key.Direction = ParameterDirection.Output;
                    SqlCommand InsertHeader = new SqlCommand(@"INSERT INTO Table_018_MarjooiSale  ([column01]
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
           ) VALUES(" + ReturnNum + ",'" + ReturnDate + "'," + Row["Column03"].ToString() + "," +
                        (Row["Column04"].ToString().Trim() == "" ? "NULL" : "'" + Row["Column04"].ToString().Trim() + "'") + "," +
                        (Row["Column05"].ToString().Trim() == "" ? "NULL" : Row["Column05"].ToString().Trim()) + ",'" + "ارجاع فاکتور فروش ش " + Row["Column01"].ToString() + " تاریخ " + Row["Column02"].ToString() + "'," +
                        (Row["Column07"].ToString().Trim() == "" ? "NULL" : Row["Column07"].ToString().Trim()) + ",0,0,0,0," +
                        (Row["Column12"].ToString() == "True" ? 1 : 0) + ",'" + Class_BasicOperation._UserName
                        + "',Getdate(),'" + Class_BasicOperation._UserName + "',Getdate()," + Row["ColumnId"].ToString() + "," + Row["Column28"].ToString() + "," +
                        Row["Column32"].ToString() + "," + Row["Column33"].ToString() + "," + Row["Column34"].ToString() + "," + Row["Column35"].ToString() +
                        "," + (Row["Column40"].ToString().Trim() == "" ? "NULL" : Row["Column40"].ToString()) + "," +
                         Row["Column41"].ToString() +
                        "); SET @Key=SCOPE_IDENTITY()", Con);
                    InsertHeader.Parameters.Add(Key);
                    InsertHeader.ExecuteNonQuery();
                    ReturnId = int.Parse(Key.Value.ToString());

                    //درج دیتیل1
                    foreach (DataRowView item in this.table_011_Child1_SaleFactorBindingSource)
                    {
                        SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_019_Child1_MarjooiSale ([column01]
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
           ,[Column35]) VALUES(" + ReturnId + "," + item["Column02"].ToString() +
                            "," + item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() +
                            "," + item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," +
                            item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL,NULL," +
                            (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                            item["Column15"].ToString() + "," + item["Column16"].ToString() + "," + item["Column17"].ToString() + "," + item["Column18"].ToString() + "," + item["Column19"].ToString() + "," + item["Column20"].ToString() +
                            ",NULL," + (item["Column22"].ToString().Trim() != "" ? item["Column22"].ToString() : "NULL") + ",NULL," + (Row["Column07"].ToString().Trim() != "" ? Row["Column07"].ToString() : "0") + ",0,0,0,0,0," +
                            item["Column31"].ToString() + "," + item["Column32"].ToString() + "," +
                            (item["Column34"].ToString() == "" ? "NULL" : "'" + item["Column34"].ToString() + "'") + "," +
                            (item["Column35"].ToString() == "" ? "NULL" : "'" + item["Column35"].ToString() + "'") + "," + item["Column36"].ToString() + "," + item["Column37"].ToString() + ")", Con);
                        InsertDetail.ExecuteNonQuery();
                    }

                    //درج دیتیل 2
                    foreach (DataRowView item in this.table_012_Child2_SaleFactorBindingSource)
                    {
                        clDoc.RunSqlCommand(ConSale.ConnectionString, "INSERT INTO Table_020_Child2_MarjooiSale VALUES(" + ReturnId + "," + item["Column02"].ToString()
                            + "," + item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + (item["Column05"].ToString() == "True" ? 1 : 0) + "," +
                            (item["Column06"].ToString().Trim() == "" ? "NULL" : item["Column06"].ToString()) + ")");

                    }
                    clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_010_SaleFactor SET Column19=1 , Column20=" + ReturnId + " Where ColumnId=" + Row["ColumnId"].ToString());
                }
            }
        }

        private void InsertReceipt(DataRowView Row)
        {
            if (Row["Column09"].ToString() == "0")
                return;


            _05_Sale.Frm_011_ResidInformationDialog frm = new Frm_011_ResidInformationDialog();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                //صدور فاکتور مرجوعی
                TurnBack(Row);


                //DraftTable
                DataTable DraftTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_007_PwhrsDraft where ColumnId=" + Row["Column09"].ToString());
                DataTable DraftChild = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_008_Child_PwhrsDraft where Column01=" + Row["Column09"].ToString());

                string Function = frm.FunctionValue;
                ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
                //, int.Parse(DraftTable.Rows[0]["Column03"].ToString()));

                //**Resid Header
                SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                key.Direction = ParameterDirection.Output;
                using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.WHRS))
                {
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
                                                                          ) VALUES (" + ResidNum + ",'" + ReturnDate + "'," +
                     DraftTable.Rows[0]["Column03"].ToString() + "," + Function + "," + DraftTable.Rows[0]["Column05"].ToString() + ",'" + "رسید صادرشده از فاکتور مرجوعی شماره " +
                     ReturnNum + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,0," + ReturnId + ",0," +
                     +(DraftTable.Rows[0]["Column23"].ToString() == "True" ? 1 : 0) + "," +
                            (DraftTable.Rows[0]["Column24"].ToString().Trim() == "" ? "NULL" : DraftTable.Rows[0]["Column24"].ToString()) + "," +
                             DraftTable.Rows[0]["Column25"].ToString()
                     + ",1,null); SET @Key=Scope_Identity()", conware);
                    Insert.Parameters.Add(key);
                    Insert.ExecuteNonQuery();
                    ResidId = int.Parse(key.Value.ToString());

                    //Resid Detail
                    //در هنگام صدور فاکتور مرجوعی فروش اگر شماره فاکتور فروش مشخص بود ارزش کالا در حواله مربوطه خوانده شده
                    // وعینا در رسید مربو به فاکتور مرجوعی فروش درج میگردد
                    foreach (DataRow item in DraftChild.Rows)
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
           ,[Column35]) VALUES (" + ResidId + "," + item["Column02"].ToString() + "," +
                            item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + ",0 ,0,0,0,NULL," +
                            (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                            + "',getdate(),0," + item["Column15"].ToString() + "," + item["Column16"].ToString() +
                            ",0,NULL,NULL," +
                            (item["Column23"].ToString().Trim() == "" ? "NULL" : item["Column23"].ToString()) + "," +
                            item["Column24"].ToString() + ",0,0,0," +
                            (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column30"].ToString() + "'") + "," +
                            (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column31"].ToString() + "'") + "," +
                            item["Column32"].ToString() + "," + item["Column33"].ToString() + "," + item["Column34"].ToString() + "," +
                            item["Column35"].ToString() + ")", conware);
                        InsertDetail.ExecuteNonQuery();
                    }
                }
                //درج شماره رسید در فاکتور مرجوعی
                clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column09", "ColumnId", ReturnId, ResidId);
            }


        }

        private void InvertDoc(DataRowView Row)
        {
            if (Row["Column10"].ToString().Trim() == "" || Row["Column10"].ToString() == "0")
                return;

            DataTable PreDoc = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select * from Table_065_SanadDetail where Column00=" +
                Row["Column10"].ToString() + " and (Column16=15 and Column17=" + Row["ColumnId"].ToString() +
                    ") or (Column16=26 and Column17=" + Row["Column09"].ToString() + ")");

            if (PreDoc.Rows.Count > 0)
            {
                //Header
                //ReturnDocNum = clDoc.LastDocNum() + 1;
                //ReturnDocId = clDoc.ExportDoc_Header(ReturnDocNum, ReturnDate, "فاکتور مرجوعی", Class_BasicOperation._UserName);
                string CommandTxt = string.Empty;
                CommandTxt = "declare @Key int declare @DetialID int declare @ResidID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)   ";
                CommandTxt += @" set @ReturnDocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1  INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + ReturnDate + "',2,0,'فاکتور مرجوعی','" + Class_BasicOperation._UserName +
                       "',getdate()); SET @Key=SCOPE_IDENTITY()";

                //Detail
                foreach (DataRow item in PreDoc.Rows)
                {
                    string[] _AccInfo = clDoc.ACC_Info(item["Column01"].ToString());
                    // clDoc.ExportDoc_Detail(ReturnDocId, item["Column01"].ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                    // , (item["Column07"].ToString().Trim() == "" ? "NULL" : item["Column07"].ToString()), (item["Column08"].ToString().Trim() == "" ? "NULL" : item["Column08"].ToString()),
                    // (item["Column09"].ToString().Trim() == "" ? "NULL" : item["Column09"].ToString()), "مرجوعی-" + item["Column10"].ToString().Trim(),
                    //Convert.ToInt64(item["Column12"].ToString()),
                    //Convert.ToInt64(item["Column11"].ToString()),
                    //Convert.ToDouble(item["Column13"].ToString()),
                    //Convert.ToDouble(item["Column14"].ToString()),
                    //(item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1))
                    //, short.Parse((item["Column16"].ToString() == "26" ? 27 : 17).ToString()), (item["Column16"].ToString() == "26" ? ResidId : ReturnId), Class_BasicOperation._UserName,
                    //Convert.ToDouble(item["Column22"].ToString()), (short?)null);

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
                                " + Convert.ToDouble(item["Column13"].ToString()) + @",
                                " + Convert.ToDouble(item["Column14"].ToString()) + @",
                                " + (item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
                                " + short.Parse((item["Column16"].ToString() == "26" ? 27 : 17).ToString()) + @",
                                " + (item["Column16"].ToString() == "26" ? ResidId : ReturnId) + @",
                                '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                " + Convert.ToDouble(item["Column22"].ToString()) + @",
                                NULL)";

                }

                //درج شماره سند در فاکتور مرجوعی
                //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column10", "ColumnId", ReturnId, ReturnDocId);
                CommandTxt += " Update " + ConSale.Database + ".dbo.Table_018_MarjooiSale set Column10=@Key where ColumnId=" + ReturnId;

                //درج شماره سند در رسید انبار
                //DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select * from Table_065_SanadDetail where Column00=" + ReturnDocId + " and Column16=27");
                //if (Table.Rows.Count > 0)
                //clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column07", "ColumnId", ResidId, ReturnDocId);
                CommandTxt += @" IF (Select count(*) from Table_065_SanadDetail where Column00=@Key and Column16=27) >0 Begin  Update " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt set Column07=@Key where ColumnId=" + ResidId + " END";

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

        private void Frm_002_Faktor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control && bt_New.Enabled)
            {
                bt_New_Click(sender, e);

            }
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
            //    bt_Export_Click(sender, e);
            //else if (e.Control && e.KeyCode == Keys.L)
            //    bt_DelDoc_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F8)

                toolStripButton7.ShowDropDown();
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        _05_Sale.Reports.Form_SaleFactorPrint frm = new Reports.Form_SaleFactorPrint(
                                int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), false);
                        frm.ShowDialog();
                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch { }
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
                    this.table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    //if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null || dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        bt_Save_Click(sender, e);
                    //    }
                    //}

                    dataSet_Sale.EnforceConstraints = false;
                    int RowID = ReturnIDNumber(int.Parse(txt_Search.Text));
                    this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, RowID);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, RowID);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, RowID);
                    dataSet_Sale.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                    DataTable GoodTable = clGood.MahsoolInfoForFactor(null, null);
                    gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                    gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
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
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_010_SaleFactor where column01=" + FactorNum, con);
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

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            int SanadId = 0;
            if (this.table_010_SaleFactorBindingSource.Count > 0)
                SanadId = (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString()));

            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;


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
                catch
                {
                }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_Drafts_Click(object sender, EventArgs e)
        {
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;

            if (gridEX1.GetRow().Cells["Column09"].Text.Trim() == "0" || gridEX1.GetRow().Cells["Column09"].Text.Trim() == "")
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 26))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Form07_ViewDrafts")
                        {
                            item.BringToFront();
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form07_ViewDrafts frm = new PWHRS._03_AmaliyatAnbar.Form07_ViewDrafts();
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
            else
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 24))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Form06_RegisterDrafts")
                        {
                            item.BringToFront();
                            ((PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts)item).txt_Search.Text = gridEX1.GetRow().Cells["Column09"].Text;
                            ((PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts)item).bt_Search_Click(sender, e);
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts frm = new PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
                        int.Parse(gridEX1.GetValue("Column09").ToString()));
                    frm.ShowDialog();
                    int SaleId = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    dataSet_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, SaleId);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, SaleId);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, SaleId);
                    dataSet_Sale.EnforceConstraints = true;
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
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
                GoodbindingSource.DataSource = clGood.MahsoolInfo( 0);

                DataTable GoodTable = (Properties.Settings.Default.ShowMojodi ? clGood.MahsoolInfoForFactor(null, null) : clGood.MahsoolInfo( 0));
                gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
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

                DataTable CustomerTable = clDoc.ReturnTable
          (ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
                                           dbo.Table_045_PersonInfo.Column01 AS code,
                                           dbo.Table_045_PersonInfo.Column02 AS NAME,
                                           dbo.Table_065_CityInfo.Column02 AS shahr,
                                           dbo.Table_060_ProvinceInfo.Column01 AS ostan,
                                           dbo.Table_045_PersonInfo.Column06 AS ADDRESS,
                                           dbo.Table_045_PersonInfo.Column30,
                                           Table_045_PersonInfo.Column07,
                                           Table_045_PersonInfo.Column19 AS Mobile,dbo.Table_045_PersonInfo.Column146
                                    FROM   dbo.Table_045_PersonInfo
                                           LEFT JOIN dbo.Table_065_CityInfo
                                                ON  dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                                           LEFT JOIN dbo.Table_060_ProvinceInfo
                                                ON  dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00
                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");
                gridEX1.DropDowns["Customer"].SetDataBinding(CustomerTable, "");
                gridEX1.DropDowns["Tel"].SetDataBinding(CustomerTable, "");

                gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)"), "");
                gridEX_Extra.DropDowns["Person"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_045_PersonInfo where Column12=1"), "");


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void Frm_002_Faktor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.CurrencyManager.Position > -1)
            {
                //{
                DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                if (!Row["Column01"].ToString().StartsWith("-"))
                {
                    Properties.Settings.Default.Ware = gridEX1.GetValue("Column42").ToString();
                    Properties.Settings.Default.Masool = gridEX1.GetValue("Column05").ToString();
                    Properties.Settings.Default.Save();
                }
            }

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

        private void gridEX1_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
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
                if (e.Column.Key == "Column44")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column44", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }

            try
            {
                if (e.Column.Key == "Column42")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column42", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
                try
                {

                    DataTable GoodTable = (Properties.Settings.Default.ShowMojodi ? clGood.MahsoolInfoForNewFactor(gridEX1.GetValue("Column02").ToString(), gridEX1.GetValue("Column42")) : clGood.MahsoolInfo( 0));
                    GoodbindingSource.DataSource = (Properties.Settings.Default.ShowMojodi ? clGood.MahsoolInfoForFactor(null, null) : clGood.MahsoolInfo( 0));

                    gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                    gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                }
                catch
                {

                }

            }
            catch { }

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

        private void gridEX1_ColumnButtonClick(object sender, ColumnActionEventArgs e)
        {
            if (gridEX_List.AllowAddNew == InheritableBoolean.True)
            {
                try
                {
                    string _PreFactorCode = InputBox.Show(
                        "در صورت تمایل به بازخوانی اطلاعات مربوط به پیش فاکتور، شماره پیش فاکتور مورد نظر را وارد نمایید", "");
                    if (_PreFactorCode.ToString().Trim() != "")
                    {
                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                        {
                            Con.Open();
                            int _MaxCode = clDoc.MaxNumber(Con.ConnectionString,
                                "Table_010_SaleFactor", "Column01");
                            SqlCommand Select = new SqlCommand(
                                "Select * from Table_007_FactorBefore where Column01=" +
                                _PreFactorCode, Con);
                            SqlDataReader Reader = Select.ExecuteReader();
                            Reader.Read();
                            if (Reader.HasRows)
                            {
                                int _ID;
                                int _PrefactorID = int.Parse(Reader["ColumnId"].ToString());



                                SqlDataAdapter Adapter;
                                SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                                Key.Direction = ParameterDirection.Output;

                                //***************************INSERT HEADER

                                SqlCommand Insert = new SqlCommand(
                                    @"INSERT INTO Table_010_SaleFactor ([column01]
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
                                                                       ,[Column28]
                                                                       ,[Column29]
                                                                       ,[Column30]
                                                                       ,[Column31]
                                                                       ,[Column32]
                                                                       ,[Column33]
                                                                       ,[Column34]
                                                                       ,[Column35]
                                                                       ,[Column36]
                                                                       ,[Column37]
                                                                       ,[Column38]
                                                                       ,[Column39]
                                                                       ,[Column40]
                                                                       ,[Column41]
                                                                       ,[Column42]
                                                                       ,[Column43],Column61) VALUES( " + _MaxCode + " ,'" +
                                    Reader["Column02"].ToString()
                                    + "' , " + Reader["Column03"].ToString() + "," +
                                    (Reader["Column04"].ToString().Trim() != "" ? "'" +
                                    Reader["Column04"].ToString() + "'" : "NULL") +
                                    "," + (Reader["Column05"].ToString().Trim() == "" ? "NULL" : Reader[
                                    "Column05"].ToString().Trim()) + "," +
                                    (Reader["Column06"].ToString().Trim() != "" ? "'" +
                                    Reader["Column06"].ToString() + "'" : "NULL") +
                                    "," + Reader["ColumnId"].ToString() + ",0,0,0,null,0,'" +
                                    Class_BasicOperation._UserName + "',getdate(),'" +
                                    Class_BasicOperation._UserName
                                    + @"',getdate(),0,null,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
                                NULL,NULL,0,null,Null,0,NULL,NULL,0); SET @Key= SCOPE_IDENTITY()", Con);
                                Reader.Close();

                                SqlCommand CountCommand = new SqlCommand(
                                   @"SELECT ISNULL((Select ISNULL(ColumnId,0) 
from Table_010_SaleFactor where Column07=" + _PrefactorID + "),0)", Con);
                                if (CountCommand.ExecuteScalar().ToString() != "0")
                                {
                                    gridEX1.CancelCurrentEdit();
                                    gridEX1.Enabled = false;
                                    bt_New.Enabled = true;
                                    throw new Exception("برای این پیش فاکتور، قبلا فاکتور صادر شده است");
                                }

                                Insert.Parameters.Add(Key);
                                Insert.ExecuteNonQuery();
                                _ID = int.Parse(Key.Value.ToString());


                                //***************************INSERT GOOD LIST
                                Adapter = new SqlDataAdapter(
                                    "Select * from Table_008_Child1_FactorBefore where Column01=" +
                                    _PrefactorID, Con);
                                DataTable Child1 = new DataTable();
                                Adapter.Fill(Child1);
                                foreach (DataRow item in Child1.Rows)
                                {
                                    SqlCommand ChildInsert = new SqlCommand(
                                        @"INSERT INTO Table_011_Child1_SaleFactor ([column01]
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
           ,[column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]
           ,[Column36]
           ,[Column37]) VALUES(" + _ID + "," +
                                        item["Column02"].ToString()
                                        + "," + item["Column03"].ToString() + "," +
                                        item["Column04"].ToString() + "," + item["Column05"].ToString() +
                                        "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + "," + item["Column08"].ToString() +
                                        "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() +
                                        ",null,null,null,0," + item["Column16"].ToString() + "," +
                                        item["Column17"].ToString() + "," + item["Column18"].ToString() + "," + item["Column19"].ToString() + ","
                                        + item["Column21"].ToString() + "," +
                                        (item["Column22"].ToString().Trim() != "" ? "'" +
                                        item["Column22"].ToString() + "'" : "NULL") +
                                        "," + (item["Column23"].ToString().Trim() != "" ? "'" +
                                        item["Column23"].ToString() + "'" : "NULL") + "," +
                                        (item["Column20"].ToString().Trim() != "" ? "'" +
                                        item["Column20"].ToString() + "'" : "NULL") +
                                        "," + _PrefactorID + ",0,0,0," +
                                        item["ColumnId"].ToString() + ",0,0," +
                                        item["Column27"].ToString() + "," + item["Column28"].ToString() +
                                        ",100,null,null,0,0)", Con);

                                    ChildInsert.ExecuteNonQuery();
                                }

                                //***************************INSERT EXTRA/Reductions
                                Adapter = new SqlDataAdapter(
                                    "Select * from Table_009_Child2_FactorBefore where Column01=" +
                                    _PrefactorID, Con);
                                DataTable Child2 = new DataTable();
                                Adapter.Fill(Child2);
                                foreach (DataRow item in Child2.Rows)
                                {
                                    SqlCommand ChildInsert = new SqlCommand(
                                        @"INSERT INTO  Table_012_Child2_SaleFactor ([column01]
                                                                           ,[column02]
                                                                           ,[column03]
                                                                           ,[column04]
                                                                           ,[column05]
                                                                           ,[column06]) VALUES(" + _ID + "," +
                                        item["Column02"].ToString() + "," + item["Column03"].ToString() +
                                        "," + item["Column04"].ToString() +
                                        "," + ((bool)item["Column05"] == true ? "1" : "0") +
                                        //(item["Column05"].ToString().Trim() == "FALSE" ? "0" : "1")
                                        "," + (item["Column06"].ToString().Trim() != "" ? "'" +
                                        item["Column06"].ToString() + "'" : "NULL") + ")", Con);
                                    ChildInsert.ExecuteNonQuery();
                                }
                                clDoc.Update_Des_Table(Con.ConnectionString,
                                    "Table_007_FactorBefore", "Column12", "ColumnId", _PrefactorID, _ID);
                                dataSet_Sale.EnforceConstraints = false;
                                this.table_010_SaleFactorTableAdapter.Fill_ID(
                                    dataSet_Sale.Table_010_SaleFactor, _ID);
                                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(
                                    dataSet_Sale.Table_011_Child1_SaleFactor, _ID);
                                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(
                                    dataSet_Sale.Table_012_Child2_SaleFactor, _ID);
                                dataSet_Sale.EnforceConstraints = true;

                                //محاسبه تخفیفات و سایر اطلاعات فاکتور 
                                DataRowView Row =
                                    (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                                Row["Column15"] = Class_BasicOperation._UserName;
                                Row["Column16"] = Class_BasicOperation.ServerDate();
                                Row["Column34"] = gridEX_List.GetTotal(
                                    gridEX_List.RootTable.Columns["Column19"],
                                    AggregateFunction.Sum).ToString();
                                Row["Column35"] = gridEX_List.GetTotal(
                                    gridEX_List.RootTable.Columns["Column17"],
                                    AggregateFunction.Sum).ToString();

                                //****************Calculate Discounts
                                double NetTotal = Convert.ToDouble(
                                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                                    AggregateFunction.Sum).ToString());
                                int CustomerCode = int.Parse(Row["Column03"].ToString());
                                string Date = Row["Column02"].ToString();
                                Row["Column28"] = NetTotal;

                                NetTotal = ClDiscount.SpecialGroup(
                                    Convert.ToDouble(Row["Column28"].ToString()), CustomerCode, Date);
                                Row["Column30"] = NetTotal;

                                NetTotal = ClDiscount.VolumeGroup(
                                    Convert.ToDouble(Row["Column28"].ToString()) -
                                    Convert.ToDouble(Row["Column30"].ToString()), CustomerCode, Date);
                                Row["Column29"] = NetTotal;

                                object Value = gridEX1.GetValue("Column36");
                                if (Value != null && !string.IsNullOrWhiteSpace(Value.ToString()))
                                {
                                    NetTotal = ClDiscount.SpecialCustomer(
                                          Convert.ToDouble(Row["Column28"].ToString()) -
                                          Convert.ToDouble(Row["Column30"].ToString()) -
                                          Convert.ToDouble(Row["Column29"].ToString()), Convert.ToInt32(gridEX1.GetValue("Column36")));
                                }

                                if (NetTotal == Convert.ToDouble(0))
                                    NetTotal = ClDiscount.SpecialCustomer(
                                        Convert.ToDouble(Row["Column28"].ToString()) -
                                        Convert.ToDouble(Row["Column30"].ToString()) -
                                        Convert.ToDouble(Row["Column29"].ToString()), CustomerCode, Date);
                                Row["Column31"] = NetTotal;

                                //Extra-Reductions
                                Janus.Windows.GridEX.GridEXFilterCondition Filter =
                                    new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"],
                                        ConditionOperator.Equal, false);
                                Row["Column32"] = gridEX_Extra.GetTotal(
                                    gridEX_Extra.RootTable.Columns["Column04"],
                                    AggregateFunction.Sum, Filter).ToString();
                                Filter.Value1 = true;
                                Row["Column33"] = gridEX_Extra.GetTotal(
                                    gridEX_Extra.RootTable.Columns["Column04"],
                                    AggregateFunction.Sum, Filter).ToString();

                                if (Row["Column09"].ToString() == "0")
                                {
                                    //if (DialogResult.Yes == MessageBox.Show("آیا مایل به محاسبه جوایز هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                                    Classes.Class_Award.SaleAward_Box(int.Parse(Row["ColumnId"].ToString()),
                                        Row["Column02"].ToString(), (Row["Column07"].ToString() == "" ? 0 : int.Parse(Row["Column07"].ToString())), mnu_CalculatePrice.Checked);
                                }

                                this.table_010_SaleFactorBindingSource.EndEdit();
                                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                                this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                                this.table_010_SaleFactorTableAdapter.Update(
                                    dataSet_Sale.Table_010_SaleFactor);
                                this.table_011_Child1_SaleFactorTableAdapter.Update(
                                    dataSet_Sale.Table_011_Child1_SaleFactor);
                                this.table_012_Child2_SaleFactorTableAdapter.Update(
                                    dataSet_Sale.Table_012_Child2_SaleFactor);
                                bt_New.Enabled = true;

                                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                                Class_BasicOperation.ShowMsg("", "عملیات بازخوانی با موفقیت انجام شد", "Information");
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

        private void gridEX_Extra_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
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
            //    if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)[
            //        "Column09"].ToString() != "0" &&
            //        ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)[
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
            if (this.table_010_SaleFactorBindingSource.Count > 0)
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

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                try
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 73))
                        throw new Exception("کاربر گرامی شما امکان ابطال فاکتور فروش را ندارید");

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID) != 0 || clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID) != 0)
                        throw new Exception("به علت صدور حواله برای این فاکتور، ابطال فاکتور امکانپذیر نیست");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID.ToString()) == "True")
                        throw new Exception("این فاکتور باطل شده است");


                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ابطال این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", int.Parse(RowID), 1);
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                dataSet_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                dataSet_Sale.EnforceConstraints = true;
                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

            }
        }

        private void mnu_CancelCancel_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                try
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 73))
                        throw new Exception("کاربر گرامی شما امکان لغو ابطال فاکتور فروش را ندارید");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به لغو ابطال این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", int.Parse(RowID), 0);
                        }
                    }


                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", int.Parse(RowID), 0);
                dataSet_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                dataSet_Sale.EnforceConstraints = true;
                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

            }
        }

        private void mnu_SaleType_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 3))
            {
                _02_BasicInfo.Frm_007_SaleType ob = new _02_BasicInfo.Frm_007_SaleType(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 4));
                ob.ShowDialog();
                gridEX1.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes"), "");


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX_List_DeletingRecord(object sender, RowActionCancelEventArgs e)
        {
            if (e.Row.Cells["column30"].Value.ToString() == "True")
            {
                if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف کالا و ذخیره تغییرات هستید؟",
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {
                    e.Row.Delete();
                    this.table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                    this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_Sale.Table_011_Child1_SaleFactor);
                    this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_Sale.Table_012_Child2_SaleFactor);
                }
                else
                    e.Cancel = true;
            }

        }

        private void gridEX1_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                try
                {
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column03");
                }
                catch { }
                try
                {
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column44");
                }
                catch { }

                try
                {
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column42");
                }
                catch { }

                if (e.Column.Key == "column03")
                {
                    gridEX1.SetValue("Tel", gridEX1.GetValue("Column03"));
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

                    gridEX1.SetValue("Column36", (((DataRowView)gridEX1.RootTable.Columns["Column03"].DropDown.FindItem(gridEX1.GetValue("Column03")))["Column30"] != null &&
                       !string.IsNullOrWhiteSpace(((DataRowView)gridEX1.RootTable.Columns["Column03"].DropDown.FindItem(gridEX1.GetValue("Column03")))["Column30"].ToString().Trim()) ?
                        (object)((DataRowView)gridEX1.RootTable.Columns["Column03"].DropDown.FindItem(gridEX1.GetValue("Column03")))["Column30"].ToString().Trim() : DBNull.Value));

                    gridEX1.SetValue("Column64", (((DataRowView)gridEX1.RootTable.Columns["Column03"].DropDown.FindItem(gridEX1.GetValue("Column03")))["Column146"] != null &&
                       !string.IsNullOrWhiteSpace(((DataRowView)gridEX1.RootTable.Columns["Column03"].DropDown.FindItem(gridEX1.GetValue("Column03")))["Column146"].ToString().Trim()) ?

                       (object)((DataRowView)gridEX1.RootTable.Columns["Column03"].DropDown.FindItem(gridEX1.GetValue("Column03")))["Column146"].ToString().Trim() : DBNull.Value));



                    if (gridEX_List.RowCount > 0)
                    {
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                        {
                            DataTable DisTbl = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT     Column01, Column04, Column05, Column06, SUM(Column07) AS SumDis
                    FROM         dbo.Table_027_Discount_CommoditySpecialCustomer
                    WHERE     (Column02 <= N'" + gridEX1.GetValue("Column02").ToString() + @"') AND (Column03 >= N'" + gridEX1.GetValue("Column02").ToString() + @"')
                    and (Column01=" + gridEX1.GetValue("Column03").ToString() + ") AND (Column05 <= " + item.Cells["Column07"].Value.ToString() +
                     ") AND (Column06 >= " + item.Cells["Column07"].Value.ToString() + ") AND (Column04=" + item.Cells["Column02"].Value.ToString() + ") GROUP BY Column01, Column04, Column05, Column06");
                            if (DisTbl.Rows.Count > 0)
                            {
                                item.BeginEdit();
                                item.Cells["Column16"].Value = DisTbl.Rows[0]["SumDis"].ToString();
                                item.EndEdit();
                            }
                        }

                    }
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

                        gridEX1.SetValue("Column55", dd);
                    }
                    catch
                    {
                    }
                }
                else if (e.Column.Key == "Column40")
                {
                    object Value = gridEX1.GetValue("Column40");
                    DataRowView Row = (DataRowView)this.gridEX1.RootTable.Columns["Column40"].DropDown.FindItem(Value);
                    gridEX1.SetValue("Column41", Row["Column02"]);
                    gridEX_List.RootTable.Columns["Column14"].DefaultValue = gridEX1.GetValue("Column40");
                    gridEX_List.RootTable.Columns["Column15"].DefaultValue = gridEX1.GetValue("Column41");
                }
                else if (e.Column.Key == "Column41")
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
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");

            }
            catch { }

            try
            {
                //درج نام کالا، کد کالا
                if (e.Column.Key == "column02")
                    gridEX_List.SetValue("GoodCode", gridEX_List.GetValue("column02").ToString());
                else if (e.Column.Key == "GoodCode")
                    gridEX_List.SetValue("column02", gridEX_List.GetValue("GoodCode").ToString());

                //درج تخفیف، اضافه خطی، واحد شمارش، تعداد در کارتن، تعداد در بسته
                if (e.Column.Key == "column02" || e.Column.Key == "GoodCode" ||
                    gridEX_List.GetRow().Cells["column30"].Text.ToString() == "True")
                {
                    GoodbindingSource.Filter = "GoodID=" +
                        gridEX_List.GetRow().Cells["column02"].Value.ToString();
                    gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");
                    this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                    gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                    gridEX_List.SetValue("tedaddarkartoon",
                        0);
                    gridEX_List.SetValue("tedaddarbaste",
                        0);
                    gridEX_List.SetValue("column03",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                    //اضافه و تخفیف خطی
                    if (!Class_BasicOperation.CalLinearDis(int.Parse(gridEX1.GetValue("Column03").ToString())))
                    {
                        gridEX_List.SetValue("column16",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());
                        gridEX_List.SetValue("column18",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Extra"].ToString());
                        object Value = gridEX1.GetValue("Column36");
                        if (Value != null && !string.IsNullOrWhiteSpace(Value.ToString()))
                        {
                            DataRowView Row = (DataRowView)gridEX1.RootTable.Columns["Column36"].DropDown.FindItem(Value);
                            gridEX_List.SetValue("column16", Row["Column16"]);
                            gridEX_List.SetValue("column18", Row["Column17"]);
                        }


                    }
                    else
                    {
                        double[] array = clDoc.LastLinearDiscount(int.Parse(gridEX1.GetValue("Column03").ToString()), int.Parse(gridEX_List.GetValue("Column02").ToString()));
                        gridEX_List.SetValue("column16", array[0]);
                        gridEX_List.SetValue("column18", array[1]);
                    }
                    gridEX_List.SetValue("Column36", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Weight"].ToString());
                    double amunt = 0;
                    double boxamount = 0;
                    double packamount = 0;
                    double jozamount = 0;

                    ///قیمت فروش براساس نوع فروش
                    if (Convert.ToInt32(Properties.Settings.Default.FactorPrice) == 0
                        && gridEX1.GetValue("Column36") != null
                        && !string.IsNullOrWhiteSpace(gridEX1.GetValue("Column36").ToString()))
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("ID", typeof(Int32));
                        dt.Columns.Add("Column00", typeof(Int32));
                        dt.Columns.Add("Column01", typeof(String));
                        dt.Columns.Add("Column02", typeof(Double));

                        dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");

                        DataRow[] dr = dt.Select("Column01='" + gridEX1.GetRow().Cells["column36"].Text + "'");
                        if (dr.Count() > 0)
                        {
                            amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                            gridEX_List.SetValue("column10",
                           Convert.ToDouble(dr[0].ItemArray[3]));
                        }
                    }
                    ///قیمت فروش براساس اعلامیه قیمت

                    else if (Convert.ToInt32(Properties.Settings.Default.FactorPrice) == 1
                              && gridEX1.GetValue("Column36") != null
                              && !string.IsNullOrWhiteSpace(gridEX1.GetValue("Column36").ToString())
                              && gridEX1.GetValue("Column64") != null
                              && !string.IsNullOrWhiteSpace(gridEX1.GetValue("Column64").ToString()))
                    {

                        DataTable dt = clDoc.ReturnTable(this.ConSale.ConnectionString, @"SELECT TOP 1 tpsc.Column02,
                                                                                                       tpsc.Column03,
                                                                                                       tpsc.Column04
                                                                                                FROM   Table_82_PriceStatement tps
                                                                                                       JOIN Table_83_PriceStatementChild tpsc
                                                                                                            ON  tpsc.Column00 = tps.ColumnId
                                                                                                WHERE  tpsc.Column01 = " + gridEX_List.GetValue("column02") + @"
                                                                                                       AND tps.Column03 = " + gridEX1.GetValue("Column64") + @"
                                                                                                       AND tps.Column04 = " + gridEX1.GetValue("Column36") + @"
                                                                                                       AND tps.Column01 <= '" + gridEX1.GetValue("column02") + @"'
                                                                                                ORDER BY
                                                                                                       tps.Column01 DESC ");

                        if (dt.Rows.Count > 0)
                        {
                            boxamount = Convert.ToDouble(dt.Rows[0][0]);
                            packamount = Convert.ToDouble(dt.Rows[0][1]);
                            jozamount = Convert.ToDouble(dt.Rows[0][2]);
                            gridEX_List.SetValue("column08", boxamount);
                            gridEX_List.SetValue("column09", packamount);
                            gridEX_List.SetValue("column10", jozamount);
                        }


                    }

                    //فراخوانی قیمتهای پیش فرض از  سیاستهای فروش
                    if (CustomerGroupList.Count == 0 && amunt == Convert.ToDouble(0)
                        && boxamount == Convert.ToDouble(0)
                        && packamount == Convert.ToDouble(0)
                        && jozamount == Convert.ToDouble(0)


                        )
                    {
                        gridEX_List.SetValue("column10",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePrice"]);
                        gridEX_List.SetValue("column09",
                            0);
                        gridEX_List.SetValue("column08",
                           0);
                    }
                    else
                    {
                        CustomerPricingbindingSource.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, @"select * from Table_029_CustomerGroupGoodPricing   
                            where Column01 IN (" + string.Join(",", CustomerGroupsArray) + ") and Column02=" +
                            gridEX_List.GetValue("column02").ToString() +
                            " and Column03<='" + gridEX1.GetValue("Column02").ToString() +
                            "' and Column04>='" + gridEX1.GetValue("Column02").ToString() + "'  order by Column01,Column02,Column03,Column04");

                        if (CustomerPricingbindingSource.Count > 0)
                        {
                            gridEX_List.SetValue("column10",
                                ((DataRowView)CustomerPricingbindingSource.CurrencyManager.Current)["Column07"]);
                            gridEX_List.SetValue("column09",
                                0);
                            gridEX_List.SetValue("column08",
                              0);
                        }
                        else if (amunt == Convert.ToDouble(0) && boxamount == Convert.ToDouble(0)
                        && packamount == Convert.ToDouble(0)
                        && jozamount == Convert.ToDouble(0))
                        {
                            gridEX_List.SetValue("column10",
                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                            "SalePrice"]);
                            gridEX_List.SetValue("column09",
                               0);
                            gridEX_List.SetValue("column08",
                               0);
                        }
                    }
                }

                if (e.Column.Key == "column06")
                {
                    //float h = clDoc.GetZarib(Convert.ToInt32(gridEX_List.GetValue("GoodCode")), Convert.ToInt16(gridEX_List.GetValue("column03")));

                    //gridEX_List.SetValue("column07", Convert.ToDouble(gridEX_List.GetValue("column06").ToString()) * h);
                    gridEX_List.SetValue("column07", Convert.ToDouble(gridEX_List.GetValue("column06").ToString()));


                }
                //درج نرخ ارز از  جدول پرنت
                if (e.Column.Key == "column14")
                {
                    object Value = gridEX_List.GetValue("Column14");
                    DataRowView Row = (DataRowView)gridEX_List.RootTable.Columns["Column14"].DropDown.FindItem(Value);
                    gridEX_List.SetValue("Column15", Row["Column02"]);
                }


                //درج تخفیف خطی از  سیاست فروش

                if (gridEX_List.GetRow().Cells["Column14"].Text.Trim() == "" && gridEX_List.GetRow().Cells["Column15"].Text.Trim() == "")
                {
                    if (gridEX1.GetRow().Cells["Column40"].Text.Trim() != "" &&
                          gridEX1.GetRow().Cells["Column41"].Text.Trim() != "")
                    {
                        gridEX_List.SetValue("Column14", gridEX1.GetValue("Column40").ToString());
                        gridEX_List.SetValue("Column15", gridEX1.GetValue("Column41").ToString());
                    }
                }
                if (e.Column.Key == "column03")
                {
                    //gridEX_List.SetValue("column10",
                    //     ((DataRowView)gridEX_List.RootTable.Columns["column03"].DropDown.FindItem(gridEX_List.GetValue("column03")))["sale"].ToString());
                    //if (gridEX_List.GetRow().Cells["column06"].Text.Trim() != "")
                    //{
                    //    float h = clDoc.GetZarib(Convert.ToInt32(gridEX_List.GetValue("GoodCode")), Convert.ToInt16(gridEX_List.GetValue("column03")));
                    //    gridEX_List.SetValue("column07", Convert.ToDouble(gridEX_List.GetValue("column06").ToString()) * h);
                    //}
                    gridEX_List.SetValue("column07", Convert.ToDouble(gridEX_List.GetValue("column06").ToString()));

                }


                //محاسبه تعداد کل
                //gridEX_List.SetValue("column07",
                //        (Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarkartoon"))) +
                //        (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarbaste"))) +
                //        Convert.ToDouble(gridEX_List.GetValue("column06")));

                //محاسبه وزن کل
                gridEX_List.SetValue("Column37", Convert.ToDouble(gridEX_List.GetValue("Column07")) * Convert.ToDouble(gridEX_List.GetValue("Column36")));

                //محاسبه بر اساس کارتن
                //if (!mnu_CalculatePrice.Checked)
                //{
                //    double TotalPrice = (gridEX1.GetValue("Column12").ToString() == "True" ?
                //        (Convert.ToDouble(gridEX_List.GetValue("column04")) *
                //        Convert.ToDouble(gridEX_List.GetValue("column08"))) +
                //          (Convert.ToDouble(gridEX_List.GetValue("column05")) *
                //        Convert.ToDouble(gridEX_List.GetValue("column09"))) +
                //        (Convert.ToDouble(gridEX_List.GetValue("column06")) *
                //        Convert.ToDouble(gridEX_List.GetValue("column10")))
                //        : Convert.ToInt64((Convert.ToDouble(gridEX_List.GetValue("column04")) *
                //        Convert.ToDouble(gridEX_List.GetValue("column08"))) +
                //          (Convert.ToDouble(gridEX_List.GetValue("column05")) *
                //        Convert.ToDouble(gridEX_List.GetValue("column09"))) +
                //        (Convert.ToDouble(gridEX_List.GetValue("column06")) *
                //        Convert.ToDouble(gridEX_List.GetValue("column10")))));

                //    TotalPrice = TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column33").ToString()) / 100;
                //    gridEX_List.SetValue("column11", TotalPrice);
                //}
                ////محاسبه بر اساس جز
                //else
                {
                    Double TotalPrice = (gridEX1.GetValue("Column12").ToString() == "True" ?
                        (Convert.ToDouble(gridEX_List.GetValue("Column07").ToString()) *
                        Convert.ToDouble(gridEX_List.GetValue("column10"))) :
                            Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column07").ToString()) *
                         Convert.ToDouble(gridEX_List.GetValue("column10"))));
                    gridEX_List.SetValue("Column11", TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column33").ToString()) / 100);
                }

                if (e.Column.Key != "column16")
                {
                    DataTable DisTbl = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT     Column01, Column04, Column05, Column06, SUM(Column07) AS SumDis
                                                        FROM         dbo.Table_027_Discount_CommoditySpecialCustomer
                                                        WHERE     (Column02 <= N'" + gridEX1.GetValue("Column02").ToString() + @"') AND (Column03 >= N'" + gridEX1.GetValue("Column02").ToString() + @"')
                                                        and (Column01=" + gridEX1.GetValue("Column03").ToString() + ") AND (Column05 <= " + gridEX_List.GetValue("Column07").ToString() +
                    ") AND (Column06 >= " + gridEX_List.GetValue("Column07").ToString() + ") AND (Column04=" + gridEX_List.GetValue("column02").ToString() + ") GROUP BY Column01, Column04, Column05, Column06");
                    if (DisTbl.Rows.Count > 0)
                    {
                        gridEX_List.SetValue("Column16", DisTbl.Rows[0]["SumDis"].ToString());
                    }
                }

                if (e.Column.Key == "column17")
                    gridEX_List.SetValue("Column16", 0);

                if (e.Column.Key == "column16")
                {
                    if (gridEX1.GetValue("Column12").ToString() == "True")
                        gridEX_List.SetValue("column17", Convert.ToDouble(gridEX_List.GetValue("column11")) * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                    else
                        gridEX_List.SetValue("column17", Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                }

                //اگر فاکتور ارزی بود تمام قیمتها بر اساس ریال میشود
                if (gridEX1.GetValue("Column12").ToString() == "True")
                {
                    Double jam, takhfif, ezafe;
                    jam = Convert.ToDouble(gridEX_List.GetValue("column11"));
                    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                        takhfif = (jam * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                    else takhfif = Convert.ToDouble(gridEX_List.GetValue("Column17").ToString());
                    if (Properties.Settings.Default.ExtraMethod)
                        ezafe = ((jam - takhfif) *
                            (Convert.ToDouble(gridEX_List.GetValue("column18")) / 100));
                    else
                        ezafe = ((jam) *
                        (Convert.ToDouble(gridEX_List.GetValue("column18")) / 100));
                    gridEX_List.SetValue("column17", takhfif);
                    gridEX_List.SetValue("Column40", (jam - takhfif));
                    gridEX_List.SetValue("column19", ezafe);
                    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);

                }
                else
                {
                    //در غیر این صورت بر اساس اعداد صحیح
                    Int64 jam, takhfif, ezafe;
                    jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")));
                    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                        takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
                    else takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column17").ToString()));
                    if (Properties.Settings.Default.ExtraMethod)
                        ezafe = Convert.ToInt64(
                            //جمع
                            (Convert.ToDouble(gridEX_List.GetValue("column11")) -
                            //تخفیف
                            takhfif) *
                            //درصد
                          Convert.ToDouble(gridEX_List.GetValue("column18"))
                          / 100);
                    else
                        ezafe = Convert.ToInt64(
                            //جمع
                       (Convert.ToDouble(gridEX_List.GetValue("column11"))) *
                            //درصد
                     Convert.ToDouble(gridEX_List.GetValue("column18"))
                     / 100);

                    gridEX_List.SetValue("column17", takhfif);
                    gridEX_List.SetValue("Column40", (jam - takhfif));

                    gridEX_List.SetValue("column19", ezafe);
                    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);
                }

                //محاسبه قیمتهای انتهای فاکتور
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
                gridEX1.SetValue("Column15", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column16", Class_BasicOperation.ServerDate());



            }
            catch
            { }
        }

        private void bt_AddExtraDiscounts_Click(object sender, EventArgs e)
        {
            if (gridEX_Extra.AllowAddNew == InheritableBoolean.True && this.table_010_SaleFactorBindingSource.Count > 0 && this.table_011_Child1_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount");
                    foreach (DataRow item in Table.Rows)
                    {
                        this.table_012_Child2_SaleFactorBindingSource.AddNew();
                        DataRowView New = (DataRowView)this.table_012_Child2_SaleFactorBindingSource.CurrencyManager.Current;
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
                        New["column07"] = gridEX1.GetValue("column03");
                        New["Column05"] = item["Column02"].ToString();
                        this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_SettleFactor_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 165))
                {
                    try
                    {
                        string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                        if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID) == 0)
                            throw new Exception("جهت تسویه فاکتور، صدور سند حسابداری الزامیست");

                        if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", RowID) != 0)
                            throw new Exception("به علت ارجاع این فاکتور تسویه آن امکانپذیر نمی باشد");

                        if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                            throw new Exception("به علت ابطال این فاکتور تسویه آن امکانپذیر نمی باشد");

                        if (bool.Parse(gridEX1.GetValue("Column12").ToString()))
                            throw new Exception("تسویه فاکتورهای ارزی امکانپذیر نمی باشد");

                        _05_Sale.Frm_017_SettleSaleFactor frm = new Frm_017_SettleSaleFactor(
                            int.Parse(RowID),
                            UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 115));
                        frm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                    }
                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان تسویه فاکتور را ندارید", "None");
            }
        }

        private void mnu_ViewReturnFactor_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 22))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_013_ReturnFactor")
                    {
                        item.BringToFront();
                        ((PSHOP._05_Sale.Frm_013_ReturnFactor)item).txt_Search.Text = (gridEX1.GetRow().Cells["Column20"].Text.ToString() != "0" ?
                             gridEX1.GetRow().Cells["Column20"].Text.ToString() : "0");
                        ((PSHOP._04_Buy.Frm_003_FaktorKharid)item).bt_Search_Click(sender, e);
                        return;
                    }
                }

                PSHOP._05_Sale.Frm_013_ReturnFactor ob = new Frm_013_ReturnFactor(UserScope.CheckScope
                    (Class_BasicOperation._UserName, "Column11", 23),
                    (gridEX1.GetRow().Cells["Column20"].Text.Trim() != "" ? int.Parse(gridEX1.GetRow().Cells["Column20"].Value.ToString()) :
                    0));

                try
                {
                    ob.MdiParent = MainForm.ActiveForm;
                }
                catch { }
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ViewWareStock_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 31))
            {
                PWHRS._05_Gozareshat.Frm_003_MojoodiMaghtaiTedadi ob = new PWHRS._05_Gozareshat.Frm_003_MojoodiMaghtaiTedadi();
                try
                {
                    ob.MdiParent = MainForm.ActiveForm;
                }
                catch { }
                ob.Show();

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("آیا از کپی کردن این فاکتور مطمئن هستید؟",
                    "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes &&
                    gridEX_List.RowCount > 0)
                {
                    Save_Event(this, e);

                    //درج هدر فاکتور فروش

                    SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                    Key.Direction = ParameterDirection.Output;
                    int FactorNum = clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "column01");

                    string _CopyCmd = @"INSERT INTO Table_010_SaleFactor ([column01]
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
                                                                       ,[Column28]
                                                                       ,[Column29]
                                                                       ,[Column30]
                                                                       ,[Column31]
                                                                       ,[Column32]
                                                                       ,[Column33]
                                                                       ,[Column34]
                                                                       ,[Column35]
                                                                       ,[Column36]
                                                                       ,[Column37]
                                                                       ,[Column38]
                                                                       ,[Column39]
                                                                       ,[Column40]
                                                                       ,[Column41]
                                                                       ,[Column42]
                                                                       ,[Column43],Column61) VALUES(" +
                    FactorNum.ToString() + ", '" + gridEX1.GetValue("column02").ToString() + "', " +
                    gridEX1.GetValue("column03").ToString() + ", " +
                    (gridEX1.GetRow().Cells["Column04"].Text.Trim() == "" ? "NULL" : "'" + gridEX1.GetValue("column04").ToString() + "'") + "," +
                    (gridEX1.GetRow().Cells["Column05"].Text.Trim() == "" ? "Null" : gridEX1.GetValue("column05").ToString()) + "," +
                    (gridEX1.GetRow().Cells["Column06"].Text.Trim() == "" ? "NULL" : "'" + gridEX1.GetValue("column06").ToString() + "'")
                    + ", 0, 0, 0, 0, 0, " +
                    (gridEX1.GetValue("Column12").ToString() == "True" ? 1 : 0) + ", '" +
                    Class_BasicOperation._UserName + "', getdate(), '" +
                    Class_BasicOperation._UserName + "', getdate(), 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, " +
                    txt_TotalPrice.Value.ToString() + "," +
                    txt_VolumeGroup.Value.ToString() + "," +
                    txt_SpecialGroup.Value.ToString() + "," +
                    txt_SpecialCustomer.Value.ToString() + "," +
                    txt_Extra.Value.ToString() + "," +
                    txt_Reductions.Value.ToString() + "," +
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column19"],
                    AggregateFunction.Sum).ToString() + "," +
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column17"],
                    AggregateFunction.Sum).ToString() + "," +
                    (gridEX1.GetValue("column36").ToString().Trim() == "" ? "Null" : gridEX1.GetValue("Column36").ToString()) +
                    ",null,0,null," +
                    (gridEX1.GetRow().Cells["Column40"].Text.Trim() == "" ? "NULL" : gridEX1.GetValue("Column40").ToString()) + "," +
                    gridEX1.GetValue("Column41").ToString() + "," + gridEX1.GetValue("Column42").ToString() + "," + gridEX1.GetValue("Column43").ToString() + ",0 ); SET @Key=SCOPE_IDENTITY()";
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                    {
                        Con.Open();
                        SqlCommand InsertHeader = new SqlCommand(_CopyCmd, Con);

                        InsertHeader.Parameters.Add(Key);
                        InsertHeader.ExecuteNonQuery();
                        int FactorId = int.Parse(Key.Value.ToString());

                        //درج کالاهای فاکتور فروش
                        for (int ij = 0; ij < gridEX_List.RowCount; ij++)
                        {
                            gridEX_List.Row = ij;

                            _CopyCmd = @"INSERT INTO Table_011_Child1_SaleFactor ([column01]
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
           ,[column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]
           ,[Column36]
           ,[Column37]) VALUES(" +
                            FactorId.ToString() + ", " +
                            gridEX_List.GetValue("column02").ToString() + ", " +
                            gridEX_List.GetValue("column03").ToString() + ", " +
                            gridEX_List.GetValue("column04").ToString() + ", " +
                            gridEX_List.GetValue("column05").ToString() + ", " +
                            gridEX_List.GetValue("column06").ToString() + ", " +
                            gridEX_List.GetValue("column07").ToString() + ", " +
                            gridEX_List.GetValue("column08").ToString() + ", " +
                            gridEX_List.GetValue("column09").ToString() + ", " +
                            gridEX_List.GetValue("column10").ToString() + ", " +
                            gridEX_List.GetValue("column11").ToString() + ", 0, 0, " +
                            (gridEX_List.GetValue("Column14").ToString().Trim() == "" ? "Null" : gridEX_List.GetValue("Column14").ToString()) +
                            "," + (gridEX_List.GetValue("Column15").ToString().Trim() == "" ? "0" : gridEX_List.GetValue("Column15").ToString()) + "," +
                            gridEX_List.GetValue("column16").ToString() + ", " +
                            gridEX_List.GetValue("column17").ToString() + ", " +
                            gridEX_List.GetValue("column18").ToString() + ", " +
                            gridEX_List.GetValue("column19").ToString() + ", " +
                            gridEX_List.GetValue("column20").ToString() + ", NULL, NUll, '" +
                            gridEX_List.GetValue("column23").ToString() + "', 0, 0, 0, 0, 0, 0, '" +
                            gridEX_List.GetValue("column30").ToString() + "'," +
                            gridEX_List.GetValue("Tedaddarkartoon").ToString() + "," +
                            gridEX_List.GetValue("Tedaddarbaste").ToString() + "," +
                            gridEX_List.GetValue("Column33").ToString() + "," +
                            (gridEX_List.GetRow().Cells["Column34"].Text.Trim() == "" ? "NULL" : "'" + gridEX_List.GetValue("Column34").ToString() + "'") + "," +
                            (gridEX_List.GetRow().Cells["Column35"].Text.Trim() == "" ? "NULL" : "'" + gridEX_List.GetValue("Column35").ToString() + "'")
                            + "," + gridEX_List.GetValue("Column36").ToString() + "," + gridEX_List.GetValue("Column37").ToString() + ")";

                            SqlCommand InsertChild1 = new SqlCommand(_CopyCmd, Con);

                            InsertChild1.ExecuteNonQuery();
                        }

                        gridEX_List.MoveFirst();


                        //درج اضافات و کسورات فاکتور فروش
                        for (int ik = 0; ik < gridEX_Extra.RowCount; ik++)
                        {
                            gridEX_Extra.Row = ik;

                            _CopyCmd = @"INSERT INTO Table_012_Child2_SaleFactor ([column01]
                                                                           ,[column02]
                                                                           ,[column03]
                                                                           ,[column04]
                                                                           ,[column05]
                                                                           ,[column06]) VALUES(" +
                            FactorId.ToString() + ", " +
                            gridEX_Extra.GetValue("column02").ToString() + ", " +
                            gridEX_Extra.GetValue("column03").ToString() + ", " +
                            gridEX_Extra.GetValue("column04").ToString() + ", '" +
                            gridEX_Extra.GetValue("column05").ToString() + "', '" +
                            gridEX_Extra.GetValue("column06").ToString() + "')";

                            SqlCommand InsertChild2 = new SqlCommand(_CopyCmd, Con);

                            InsertChild2.ExecuteNonQuery();
                        }

                        gridEX_Extra.MoveFirst();


                        MessageBox.Show("فاکتور شماره " + FactorNum.ToString() + " صادر شد");
                    }
                }
            }
            catch
            {
            }
        }

        private void chk_Award_Detial_Click(object sender, EventArgs e)
        {
            if (chk_Award_Detial.Checked)
            {
                chk_Award_Detial.Checked = false;
                chk_Award_Box.Checked = true;
            }
            else
            {
                chk_Award_Detial.Checked = true;
                chk_Award_Box.Checked = false;
            }
        }

        private void chk_Award_Box_Click(object sender, EventArgs e)
        {
            if (chk_Award_Box.Checked)
            {
                chk_Award_Detial.Checked = true;
                chk_Award_Box.Checked = false;
            }
            else
            {
                chk_Award_Detial.Checked = false;
                chk_Award_Box.Checked = true;
            }
        }

        private void bt_DefineSignatures_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 121))
            {
                _05_Sale.Frm_019_Sale_Signatures frm = new Frm_019_Sale_Signatures();
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_DefineCurrency_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 7))
            {
                PACNT.Class_ChangeConnectionString.CurrentConnection = Class_ChangeConnectionString.CurrentConnection;
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT._1_BasicMenu.Form04_Currency frm = new PACNT._1_BasicMenu.Form04_Currency(
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 8));
                frm.ShowDialog();
                SqlDataAdapter Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_055_CurrencyInfo", ConBase);
                DataTable TCurrency = new DataTable();
                Adapter.Fill(TCurrency);
                gridEX1.DropDowns["Currency"].SetDataBinding(TCurrency, "");
                gridEX_List.DropDowns["Currency"].SetDataBinding(TCurrency, "");
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_CalculatePrice_Click(object sender, EventArgs e)
        {
            if (mnu_CalculatePrice.Checked)
                mnu_CalculatePrice.CheckState = CheckState.Unchecked;
            else
                mnu_CalculatePrice.CheckState = CheckState.Checked;

            Properties.Settings.Default.SalePriceCompute = mnu_CalculatePrice.Checked;
        }

        private void mnu_DefaultDescription_Click(object sender, EventArgs e)
        {
            Frm_025_SaleDefaultDescription frm = new Frm_025_SaleDefaultDescription();
            frm.ShowDialog();
        }

        private void mnu_DelDraft_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    int RowID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID.ToString());

                    if (DraftId != 0)
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 24) && UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 25))
                        {



                            clDoc.ConfirmedDraftReceipt("Draft", DraftId.ToString());

                            if (clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column20", DraftId.ToString()) != 0)
                                throw new Exception("به علت دارا بودن کارت تولید، حذف این حواله امکانپذیر نمی باشد" + Environment.NewLine + "جهت حذف از کارت تولید مربوط اقدام کنید");

                            string Message = "";
                            if ((clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column07", DraftId.ToString()) != 0) &&
                                clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column15", DraftId.ToString()) != 0)
                                Message = "برای این حواله سند حسابداری و برگه خروج صادر شده است. در صورت تأیید حذف برگه خروج و سند مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف این حواله هستید؟";
                            else if (clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column07", DraftId.ToString()) != 0)
                                Message = "برای این حواله سند حسابداری صادر شده است. در صورت تأیید حذف، سند مربوط نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف این حواله هستید؟";
                            else if (clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column15", DraftId.ToString()) != 0)
                                Message = "برای این حواله برگه خروج صادر شده است. در صورت تأیید حذف، برگه مربوطه نیز حذف خواهد شد" + Environment.NewLine + "آیا مایل به حذف این حواله هستید؟";

                            if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف این حواله هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                if (Message.Trim() != "")
                                {
                                    if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                                        DelPaper();
                                }
                                else
                                    DelPaper();



                            }

                        }
                        else
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
                    }
                    DS.Tables["Draft"].Clear();
                    DraftAdapter.Fill(DS, "Draft");
                    dataSet_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, RowID);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, RowID);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, RowID);
                    dataSet_Sale.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }


        }
        private void DelPaper()
        {
            string id = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
            string RowID = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", id).ToString();
            int SanadID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column07", RowID);
            int SaleID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column16", RowID);
            int ReturnID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column19", RowID);
            int OrderID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column17", RowID);
            int ExitID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column15", RowID);
            string command = string.Empty;
            //اگر حواله دارای سند باشد
            if (SanadID != 0)
            {
                if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 59))
                    throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید \n\r حذف حواله امکانپذیر نیست");

                clDoc.IsFinal_ID(SanadID);

                //***Delete Doc
                if (SaleID != 0)
                {


                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=15 and Column17=" + SaleID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=15 and Column17=" + SaleID;
                    command += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column10=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + SaleID;



                    Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=13 and Column17=" + SaleID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=13 and Column17=" + SaleID;



                    Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowID));
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowID);

                }
                else if (ReturnID != 0)
                {
                    //حذف سند فاکتور مرجوعی خرید 
                    clDoc.DeleteDetail_ID(SanadID, 20, ReturnID);

                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=20 and Column17=" + ReturnID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=20 and Column17=" + ReturnID;

                    command += " UPDATE " + ConSale.Database + ".dbo.Table_021_MarjooiBuy SET Column11=0  where ColumnId=" + ReturnID;



                }
                else
                //حذف سند مربوط به حواله
                {
                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=13 and Column17=" + SaleID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=13 and Column17=" + SaleID;



                    Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowID));
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowID);
                }

            }
            //صفر شدن شماره حواله در فاکتور فروش
            if (SaleID != 0)
                command += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column09=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + SaleID;



            //صفر شدن شماره حواله در فاکتور مرجوعی خرید
            if (ReturnID != 0)
                command += " UPDATE " + ConSale.Database + ".dbo.Table_021_MarjooiBuy SET Column10=0  where ColumnId=" + ReturnID;


            //اگر حواله دارای شماره سفارش باشد خروج کالاهای سفارش صفر شده و خروج برگه سفارش نیز برداشته می شود 
            if (OrderID != 0)
            {

                command += " UPDATE " + ConSale.Database + ".dbo.Table_005_OrderHeader SET Column33=0  where ColumnId=" + OrderID;


                command +=
                    @"UPDATE " + ConSale.Database + @".dbo.Table_006_OrderDetails
                    SET    Column16  = " + ConSale.Database + @".dbo.Table_006_OrderDetails. Column16 -b.column04,
                           column15  = " + ConSale.Database + @".dbo.Table_006_OrderDetails.column15 - b.column05,
                           column14  = " + ConSale.Database + @".dbo.Table_006_OrderDetails.column14 -b.column06,
                           column17  = " + ConSale.Database + @".dbo.Table_006_OrderDetails.column17 -b.column07,
                           Column23  = 0,
                           Column24  = NULL,
                           Column25  = NULL,
                           Column26  = NULL,
                           Column27  = NULL
                    FROM   " + ConSale.Database + @".dbo.Table_006_OrderDetails
                           JOIN " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft AS b
                                ON  b.column28 = " + ConSale.Database + @".dbo.Table_006_OrderDetails.columnid
                                AND b.column27 = " + ConSale.Database + @".dbo.Table_006_OrderDetails.column01
                    WHERE " + ConSale.Database + @".dbo.Table_006_OrderDetails.column01=" + OrderID + " AND b.Column01=" + RowID;
                //اگر محاسبه جایزه در سفارش بر اساس خروجی باشد کلیه جوایز مربوط به سفارش حذف می شود
                if (Convert.ToBoolean(clDoc.ExScalar(ConSale.ConnectionString, "Table_005_OrderHeader", "Column34", "ColumnId", OrderID.ToString())))
                {

                    command += " Delete " + ConSale.Database + ".dbo.Table_006_OrderDetails where Column31=1 and Column01=" + OrderID;

                }
            }

            //****Delete Exit Paper
            if (ExitID != 0)
            {
                command += " Delete " + this.ConWare.Database + ".dbo.Table_009_ExitPwhrs where Column19=" + RowID;
            }
            //***Delete Detail
            command += " Delete " + this.ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where Column01=" + RowID;

            //***Delete Header
            command += " Delete " + this.ConWare.Database + ".dbo.Table_007_PwhrsDraft where ColumnId=" + RowID;

            //Update Table_035_DraftRequest
            command += " UPDATE " + this.ConWare.Database + ".dbo.Table_035_DraftRequest SET Column07=0 , Column12=0 where Column07=" + RowID;


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
                    Class_BasicOperation.ShowMsg("", "حذف حواله با موفقیت صورت گرفت", "Information");

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
                    Txt += " آخرین قیمت خرید:" + LastBuyGoodPrice(int.Parse(gridEX_List.GetValue("Column02").ToString())).ToString("#,##0.###");

                    try
                    {
                        DataTable SaleTable = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT     TOP (1) PERCENT dbo.Table_010_SaleFactor.column03 AS CustomerId, dbo.Table_010_SaleFactor.column02 AS Date, 
                        dbo.Table_011_Child1_SaleFactor.column02 AS GoodID, dbo.Table_010_SaleFactor.column01 AS FactorNumber, 
                        dbo.Table_011_Child1_SaleFactor.column08 AS BoxPrice, dbo.Table_011_Child1_SaleFactor.column09 AS PackPrice, 
                        dbo.Table_011_Child1_SaleFactor.column10 AS JozPrice
                        FROM         dbo.Table_011_Child1_SaleFactor INNER JOIN
                        dbo.Table_010_SaleFactor ON dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
                        WHERE     (dbo.Table_010_SaleFactor.column03 = " + gridEX1.GetValue("Column03") + @") AND (dbo.Table_011_Child1_SaleFactor.column02 = " + gridEX_List.GetValue("Column02") + @")
                        ORDER BY Date DESC, FactorNumber DESC");

                        Txt += Environment.NewLine + "اطلاعات آخرین فروش این کالا به مشتری مشخص شده:" + Environment.NewLine +
                        "شماره فاکتور: " + SaleTable.Rows[0]["FactorNumber"].ToString() + "-- تاریخ: " + SaleTable.Rows[0]["Date"].ToString() + " -- قیمت کارتن:" +
                        Convert.ToInt64(Convert.ToDouble(SaleTable.Rows[0]["BoxPrice"].ToString())).ToString("n0") + " -- قیمت بسته: " +
                        Convert.ToInt64(Convert.ToDouble(SaleTable.Rows[0]["PackPrice"].ToString())).ToString("n0") + " -- قیمت جز: " +
                        Convert.ToInt64(Convert.ToDouble(SaleTable.Rows[0]["JozPrice"].ToString())).ToString("n0");

                    }
                    catch
                    {
                    }

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

        private void bt_NotConfirmDraft_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                if (DraftId == 0)
                    return;
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
                table_010_SaleFactorBindingSource.EndEdit();
                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                    dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save_Event(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select min(Column01) from Table_010_SaleFactor),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_Sale.EnforceConstraints = true;
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                    DataTable GoodTable = clGood.MahsoolInfoForFactor(null, null);
                    gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                    gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    gridEX1.UpdateData();
                    table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                    if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                        dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save_Event(sender, e);
                        }
                    }


                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString,
                        "Select ISNULL((Select max(Column01) from Table_010_SaleFactor where Column01<" +
                        ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_Sale.EnforceConstraints = true;
                        this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                        DataTable GoodTable = clGood.MahsoolInfoForFactor(null, null);
                        gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                        gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

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
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {

                try
                {
                    gridEX1.UpdateData();
                    table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                    if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                        dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save_Event(sender, e);
                        }
                    }

                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select Min(Column01) from Table_010_SaleFactor where Column01>" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_Sale.EnforceConstraints = true;
                        this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                        DataTable GoodTable = clGood.MahsoolInfoForFactor(null, null);
                        gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                        gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

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
                table_010_SaleFactorBindingSource.EndEdit();
                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                    dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save_Event(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select max(Column01) from Table_010_SaleFactor),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_Sale.EnforceConstraints = true;
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                    DataTable GoodTable = clGood.MahsoolInfoForFactor(null, null);
                    gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                    gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

                }

            }
            catch
            {
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


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SendSms_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {


                    if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                        Save_Event(sender, e);
                    {

                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ارسال پیامک به خریدار فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            // DataTable TextTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_220_SMSText where columnId=1");
                            DataTable LineTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select top 1 * from Table_175_SMS ");
                            try
                            {
                                if (((DataRowView)this.gridEX1.RootTable.Columns["column03"].DropDown.FindItem(this.gridEX1.GetValue("column03")))["Mobile"] == null ||
                            ((DataRowView)this.gridEX1.RootTable.Columns["column03"].DropDown.FindItem(this.gridEX1.GetValue("column03")))["Mobile"].ToString() == string.Empty)
                                    throw new Exception("تلفن همراه خریدار مشخص نشده است");

                                if (LineTable.Rows[0]["Usr"].ToString().Trim() == "" || LineTable.Rows[0]["Pass"].ToString().Trim() == "" ||
                                    LineTable.Rows[0]["Line"].ToString().Trim() == "")
                                    throw new Exception("اطلاعات مربوط به پنل پیامک را مشخص کنید");

                                string txt_Message = "خریدار محترم " + ((DataRowView)this.gridEX1.RootTable.Columns["column03"].DropDown.FindItem(this.gridEX1.GetValue("column03")))["name"] + " فاکتور شماره" + gridEX1.GetValue("column01").ToString() + " در تاریخ " + gridEX1.GetValue("column02").ToString() + " با جمع خالص پرداختی " + txt_EndPrice.Text + " صادر گردید";

                                IWebService Ws = new IWebService();
                                Random rand = new Random();
                                string crand = rand.Next(11111111, 99999999).ToString();
                                string returnCode = Ws.SendSms(LineTable.Rows[0]["Usr"].ToString().Trim(), LineTable.Rows[0]["Pass"].ToString().Trim(),
                                   txt_Message, ((DataRowView)this.gridEX1.RootTable.Columns["column03"].DropDown.FindItem(this.gridEX1.GetValue("column03")))["Mobile"].ToString()

                                    , crand, 1, 2, null, "parsina",
                                    //null);
                                    LineTable.Rows[0]["Line"].ToString());

                                if (returnCode.Length < 3 || returnCode.StartsWith("-"))
                                {
                                    MessageBox.Show(Ws.ShowError(returnCode, "FA"));

                                }
                                else
                                {
                                    ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column11"] = returnCode;
                                    this.table_010_SaleFactorBindingSource.EndEdit();
                                    this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                                    Class_BasicOperation.ShowMsg("", "ارسال پیام با موفقیت انجام شد", "Information");

                                }
                            }
                            catch
                            {
                                MessageBox.Show("تنظیمات اتصال به اینترنت را برسی نمائید");

                            }
                        }


                    }
                }

                catch
                {
                    MessageBox.Show("ارسال پیام کوتاه انجام نشد.تنظیمات اتصال به اینترنت را برسی نمائید");

                }
            }
        }

        private void gridEX_List_SelectionChanged(object sender, EventArgs e)
        {
            if (
                //((Janus.Windows.GridEX.GridEX)(sender)).Col == 5 &&

              gridEX_List.GetValue("GoodCode") != null)
            {
                gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

            }
            if (
                //((Janus.Windows.GridEX.GridEX)(sender)).Col == 14 &&

             gridEX_List.GetValue("GoodCode") != null)
            {
                this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
            }
            // gridEX_List.DropDowns["GoodPrice"].SetDataBinding(clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   "), "");
            //this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));

        }

        private void bt_Print_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        _05_Sale.Reports.Form_SaleFactorPrint frm = new Reports.Form_SaleFactorPrint(
                                int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), false);
                        frm.ShowDialog();
                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch { }
        }

        private void gridEX_List_RecordAdded(object sender, EventArgs e)
        {
            try
            {
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
            {
            }

        }

        private void gridEX_List_CurrentCellChanging(object sender, CurrentCellChangingEventArgs e)
        {
            if (e.Column != null)
            {
                original = InputLanguage.CurrentInputLanguage;

                if (e.Column.Key == "GoodCode")
                {
                    var culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                    var language = InputLanguage.FromCulture(culture);
                    if (InputLanguage.InstalledInputLanguages.IndexOf(language) >= 0)
                        InputLanguage.CurrentInputLanguage = language;
                    else
                        InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage;
                }


                else
                {
                    var culture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");
                    var language = InputLanguage.FromCulture(culture);
                    InputLanguage.CurrentInputLanguage = language;
                }
            }


        }

        private void gridEX_List_AddingRecord(object sender, CancelEventArgs e)
        {

            //long codeid = 0;

            //string txt_GoodCode1 = string.Empty;


            //if (gridEX_List.GetRows().Count() > 0)
            //{
            //    if (gridEX_List.GetValue("column02").ToString() != string.Empty)
            //    {
            //        codeid = Convert.ToInt64(gridEX_List.GetValue("column02").ToString());
            //        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            //        {
            //            Con.Open();
            //            SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + codeid + " ", Con);
            //            txt_GoodCode1 = (Comm.ExecuteScalar()).ToString();

            //        }

            //        string goodcode;
            //        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
            //        {
            //            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            //            {
            //                Con.Open();
            //                SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
            //                goodcode = (Comm.ExecuteScalar().ToString());

            //            }


            //            if (goodcode == txt_GoodCode1 && Convert.ToBoolean(item.Cells["column30"].Value) == false)
            //            {

            //                e.Cancel = true;
            //                gridEX_List.CancelCurrentEdit();
            //            }
            //        }

            //    }


            //}

        }

        private void btn_attach_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                // if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 95))
                {
                    try
                    {
                        DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;
                        Form03_FactorAppendix frm = new Form03_FactorAppendix(
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

        private void btn_person_Click(object sender, EventArgs e)
        {
            try
            {
                PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 40))
                {
                    System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                    DateTime dt = new DateTime(Convert.ToInt32(FarsiLibrary.Utils.PersianDate.Now.Year),
                           Convert.ToInt32(1),
                           Convert.ToInt32(1), pc);
                    PACNT._4_Person_Menu.Form01_PersonOperationList frm = new PACNT._4_Person_Menu.Form01_PersonOperationList
                        (gridEX1.GetValue("column03").ToString(), dt, Class_BasicOperation.ServerDate());

                    frm.ShowDialog();



                }
                else
                    Class_BasicOperation.ShowMsg("",
                        "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
            catch
            {
            }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            try
            {
                GoodbindingSource.DataSource = clGood.MahsoolInfo( 0);

                DataTable GoodTable = (Properties.Settings.Default.ShowMojodi ? clGood.MahsoolInfoForNewFactor(gridEX1.GetValue("Column02").ToString(), gridEX1.GetValue("Column42")) : clGood.MahsoolInfo( 0));

                gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
            }
            catch
            {
            }
        }

        private void gridEX_Extra_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column07");


            }
            catch { }
            try
            {
                if (e.Column.Key != "column07")
                    gridEX_Extra.SetValue("column07", gridEX1.GetValue("column03"));
            }
            catch
            {
            }
        }

        private void btn_Dealer_Click(object sender, EventArgs e)
        {

            try
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 217))
                {
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما دسترسی ثبت اطلاعات را ندارید", "Stop");
                    return;
                }
            }
            catch
            {
            }
            string seller = string.Empty;
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {



                try
                {

                    // Save_Event(sender, e);
                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    _05_Sale.Frm_002_FaktorDealer fr = new _05_Sale.Frm_002_FaktorDealer(RowID);
                    fr.ShowDialog();

                }
                catch (SqlException es)
                {
                    Class_BasicOperation.CheckSqlExp(es, this.Name);
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }

            }
        }

        private void btn_Seller_Click(object sender, EventArgs e)
        {
            try
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 217))
                {
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما دسترسی ثبت اطلاعات را ندارید", "Stop");
                    return;
                }
            }
            catch
            {
            }
            string seller = string.Empty;
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {


                try
                {

                    Save_Event(sender, e);
                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    _05_Sale.Frm_002_FaktorSaleMans fr = new _05_Sale.Frm_002_FaktorSaleMans(RowID);
                    fr.ShowDialog();

                }
                catch (SqlException es)
                {
                    Class_BasicOperation.CheckSqlExp(es, this.Name);
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }

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

    }
}
