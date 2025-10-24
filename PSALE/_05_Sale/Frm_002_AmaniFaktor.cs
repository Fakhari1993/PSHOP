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
    public partial class Frm_002_AmaniFaktor : Form
    {
        bool _del;
        int _ID = 0, ReturnId = 0, ReturnNum = 0, ResidId = 0, ResidNum = 0, ReturnDocId = 0, ReturnDocNum = 0;
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
        public Frm_002_AmaniFaktor(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        public Frm_002_AmaniFaktor(bool del, int ID)
        {
            _del = del;
            _ID = ID;
            InitializeComponent();
        }

        private void Frm_002_PishFaktor_Load(object sender, EventArgs e)
        {
            ToastNotification.ToastBackColor = Color.Aquamarine;
            ToastNotification.ToastForeColor = Color.Black;

            //string Award = Properties.Settings.Default.AwardCompute;
            //if (Award == "Box")
            //{
            //    chk_Award_Box.Checked = true;
            //    chk_Award_Detial.Checked = false;
            //}
            //else
            //{
            //    chk_Award_Detial.Checked = true;
            //    chk_Award_Box.Checked = false;
            //}

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

            DocAdapter = new SqlDataAdapter("Select ColumnId,Column00 from Table_060_SanadHead", ConAcnt);
            DocAdapter.Fill(DS, "Doc");
            gridEX1.DropDowns["Doc"].SetDataBinding(DS.Tables["Doc"], "");


            DataTable resid = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT     columnid, column01
FROM         Table_011_PwhrsReceipt");
            gridEX1.DropDowns["Recipt"].DataSource = resid;


            ReturnAdapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_018_MarjooiSale", ConSale);
            ReturnAdapter.Fill(DS, "Return");
            gridEX1.DropDowns["Return"].SetDataBinding(DS.Tables["Return"], "");
            DataTable dt = new DataTable();
            SqlDataAdapter ProjectAdapter = new SqlDataAdapter("SELECT * from Table_035_ProjectInfo", ConBase);
            ProjectAdapter.Fill(dt);
            gridEX1.DropDowns["project"].SetDataBinding(dt, "");
            GoodbindingSource.DataSource = clGood.MahsoolInfo("");
            DataTable GoodTable = clGood.MahsoolInfo("");
            gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            DataTable CustomerTable = clDoc.ReturnTable
            (ConBase.ConnectionString, @"SELECT     dbo.Table_045_PersonInfo.ColumnId AS id, dbo.Table_045_PersonInfo.Column01 AS code, dbo.Table_045_PersonInfo.Column02 AS name, 
                      dbo.Table_065_CityInfo.Column02 AS shahr, dbo.Table_060_ProvinceInfo.Column01 AS ostan, dbo.Table_045_PersonInfo.Column06 AS Address, 
                      dbo.Table_045_PersonInfo.Column30,Table_045_PersonInfo.Column07 
FROM    dbo.Table_045_PersonInfo    left join dbo.Table_065_CityInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22    
    left join dbo.Table_060_ProvinceInfo   ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00   
WHERE     (dbo.Table_045_PersonInfo.Column12 = 1)");


            gridEX1.DropDowns["Customer"].SetDataBinding(CustomerTable, "");
            gridEX1.DropDowns["Tel"].SetDataBinding(CustomerTable, "");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from  Table_055_CurrencyInfo");
            gridEX1.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");




            gridEX1.DropDowns["Ware"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from Table_001_PWHRS");
            gridEX1.DropDowns["Func"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from table_005_PwhrsOperation where Column16=1");



            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

            Adapter = new SqlDataAdapter("Select * FROM Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(DS, "Center");
            gridEX_List.DropDowns["Center"].SetDataBinding(DS.Tables["Center"], "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_035_ProjectInfo", ConBase);
            Adapter.Fill(DS, "Project");
            gridEX_List.DropDowns["Project"].SetDataBinding(DS.Tables["Project"], "");


            gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)"), "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
            Adapter.Fill(DS, "Discount");
            // gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");

            gridEX1.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes"), "");
            gridEX1.DropDowns["OrderNum"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_005_OrderHeader"), "");
            gridEX1.DropDowns["Prefactor"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select Columnid,Column01 from Table_007_FactorBefore"), "");
            SalePrice = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 162);
            DiscountLiner = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 163);
            DiscountEnd = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 164);
            if (_ID != 0)
            {
                this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, _ID);
                this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, _ID);

                table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
            }



            this.WindowState = FormWindowState.Maximized;
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.Enabled = true;
                dataSet_Sale5.EnforceConstraints = false;
                this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, _ID);
                this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, _ID);
                dataSet_Sale5.EnforceConstraints = true;
                gridEX1.MoveToNewRecord();
                //gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_070_AmaniFactor", "Column01").ToString());
                gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd"));
                gridEX1.SetValue("Column13", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column14", Class_BasicOperation.ServerDate());
                gridEX1.SetValue("Column15", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column16", Class_BasicOperation.ServerDate());

                ((DataRowView)table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column28"] = 0;

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

                if (gridEX1.GetRow().Cells["Column06"].Text.Trim() == "")
                    gridEX1.SetValue("Column06", Properties.Settings.Default.SaleDescription);
                //gridEX1.RootTable.Columns["Column40"].Selectable = false;
                //gridEX1.RootTable.Columns["Column41"].Selectable = false;

                gridEX1.Select();

                bt_New.Enabled = false;

                gridEX1.AllowEdit = InheritableBoolean.True;
                gridEX1.AllowAddNew = InheritableBoolean.True;
                gridEX_List.AllowAddNew = InheritableBoolean.True;
                gridEX_List.AllowEdit = InheritableBoolean.True;

                gridEX_List.AllowDelete = InheritableBoolean.True;

                foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                {
                    if (
                        item.Key == "column17" || item.Key == "column19" || item.Key == "column20")
                        item.Selectable = false;
                    else item.Selectable = true;
                }
                gridEX1.Col = 3;

                if (Properties.Settings.Default.Ware != string.Empty)
                    gridEX1.SetValue("Column42", Properties.Settings.Default.Ware);
                if (Properties.Settings.Default.Masool != string.Empty)
                    gridEX1.SetValue("Column05", Properties.Settings.Default.Masool);

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



            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void Save_Event(object sender, EventArgs e)
        {
            if (this.table_070_AmaniFactorBindingSource.Count > 0 &&
               gridEX_List.AllowEdit == InheritableBoolean.True &&
               gridEX1.GetRow().Cells["Column03"].Text.Trim() != "")
            {

                if (Properties.Settings.Default.ShowPriceAlert > 0)
                    CheckGoodsPrice();
                this.Cursor = Cursors.WaitCursor;
                gridEX_List.UpdateData();

                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    return;
                }

                DataRowView Row = (DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current;
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
                    gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_070_AmaniFactor", "Column01").ToString());
                    this.table_070_AmaniFactorBindingSource.EndEdit();
                }
                txt_TotalPrice.Value = Convert.ToDouble(
                gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                AggregateFunction.Sum).ToString());





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
                // if (Row["Column12"].ToString() == "False")
                // {
                //NetTotal = ClDiscount.SpecialGroup(
                //    Convert.ToDouble(Row["Column28"].ToString()), CustomerCode, Date);
                //Row["Column30"] = NetTotal;

                //NetTotal = ClDiscount.VolumeGroup(Convert.ToDouble(Row["Column28"].ToString()) -
                //    Convert.ToDouble(Row["Column30"].ToString()), CustomerCode, Date);
                //Row["Column29"] = NetTotal;

                //object Value = gridEX1.GetValue("Column36");
                //if (Value != null && !string.IsNullOrWhiteSpace(Value.ToString()))
                //{
                //    NetTotal = ClDiscount.SpecialCustomer(
                //          Convert.ToDouble(Row["Column28"].ToString()) -
                //          Convert.ToDouble(Row["Column30"].ToString()) -
                //          Convert.ToDouble(Row["Column29"].ToString()), Convert.ToInt32(gridEX1.GetValue("Column36")));
                //}

                //if (NetTotal == Convert.ToDouble(0))
                //    NetTotal = ClDiscount.SpecialCustomer(
                //        Convert.ToDouble(Row["Column28"].ToString()) -
                //        Convert.ToDouble(Row["Column30"].ToString()) -
                //        Convert.ToDouble(Row["Column29"].ToString()), CustomerCode, Date);
                //Row["Column31"] = NetTotal;
                //}


                this.table_070_AmaniFactorBindingSource.EndEdit();
                this.table_075_Child_AmaniFactorBindingSource.EndEdit();

                this.table_070_AmaniFactorTableAdapter.Update(dataSet_Sale5.Table_070_AmaniFactor);
                this.table_075_Child_AmaniFactorTableAdapter.Update(dataSet_Sale5.Table_075_Child_AmaniFactor);


                //if (Row["Column09"].ToString() == "0")
                {
                    //if (Row["Column12"].ToString() == "False")
                    {
                        //اگر نمایش پیغام تنظیم شده باشد آن را نشان می دهد
                        //در غیر این صورت بدون نمایش پیغام عملیات محاسبه جایزه را به صورت اتومات انجام می دهد
                        //if (Properties.Settings.Default.ShowCalculateGiftDuringSave)
                        //{
                        //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به محاسبه جوایز هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        //        (sender == bt_Save || sender == this ? MessageBoxDefaultButton.Button1 : MessageBoxDefaultButton.Button2), MessageBoxOptions.RightAlign))
                        //        if (chk_Award_Box.Checked)
                        //            Classes.Class_Award.SaleAward_Box(int.Parse(Row["ColumnId"].ToString()), Row["Column02"].ToString(),0, mnu_CalculatePrice.Checked);
                        //        else Classes.Class_Award.SaleAward_Detial(int.Parse(Row["ColumnId"].ToString()), Row["Column02"].ToString(), 0);
                        //}
                        //else
                        //{
                        //    if (chk_Award_Box.Checked)
                        //        Classes.Class_Award.SaleAward_Box(int.Parse(Row["ColumnId"].ToString()), Row["Column02"].ToString(), 0, mnu_CalculatePrice.Checked);
                        //    else Classes.Class_Award.SaleAward_Detial(int.Parse(Row["ColumnId"].ToString()), Row["Column02"].ToString(), 0);
                        //}
                    }
                }
                if (sender == bt_Save || sender == this)
                    Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                int _ID = int.Parse(Row["ColumnId"].ToString());
                dataSet_Sale5.EnforceConstraints = false;
                this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, _ID);
                this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, _ID);

                dataSet_Sale5.EnforceConstraints = true;
                table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
                bt_New.Enabled = true;
                this.Cursor = Cursors.Default;



            }
            else
            {
                this.table_070_AmaniFactorBindingSource.EndEdit();
                this.table_075_Child_AmaniFactorBindingSource.EndEdit();
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Save_Event(sender, e);
                Properties.Settings.Default.Ware = gridEX1.GetValue("Column42").ToString();
                Properties.Settings.Default.Masool = gridEX1.GetValue("Column05").ToString();
                Properties.Settings.Default.Save();
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
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!_del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                    string RowID = ((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    //if (clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column20", RowID) != 0)
                    //{
                    //    dataSet_Sale5.EnforceConstraints = false;
                    //    this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, _ID);
                    //    this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, _ID);

                    //    dataSet_Sale5.EnforceConstraints = true;
                    //    throw new Exception("به علت ارجاع این فاکتور، حذف آن امکانپذیر نمی باشد");
                    //}

                    //if (clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_034_SaleFactor_Child3 where Column01=" + RowID).Rows.Count > 0)
                    //    throw new Exception("این فاکتور دارای اطلاعات مربوط به تسویه است. جهت حذف فاکتور ابتدا، اطلاعات مربوطه را حذف کنید");

                    //int DocId = clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column10", RowID);
                    //int DraftId = clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column09", RowID);
                    //int PrefactorId = clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column07", RowID);

                    if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        int i = 0;
                        //if (DocId > 0)
                        //{
                        //    clDoc.IsFinal_ID(DocId);
                        //    //حذف سند فاکتور 

                        //    i = clDoc.DeleteDetail_ID(DocId, 15, int.Parse(RowID));
                        //    //حذف سند مربوط به حواله
                        //    clDoc.DeleteDetail_ID(DocId, 26, DraftId);
                        //}
                        // if ((i > 0 && DocId > 0) || DocId == 0)
                        {
                            //حذف فاکتور
                            foreach (DataRowView item in this.table_075_Child_AmaniFactorBindingSource)
                            {
                                item.Delete();
                            }


                            this.table_070_AmaniFactorTableAdapter.Update(dataSet_Sale5.Table_070_AmaniFactor);
                            this.table_075_Child_AmaniFactorTableAdapter.Update(dataSet_Sale5.Table_075_Child_AmaniFactor);

                            this.table_070_AmaniFactorBindingSource.EndEdit();
                            this.table_075_Child_AmaniFactorBindingSource.EndEdit();
                            this.table_070_AmaniFactorBindingSource.RemoveCurrent();
                            this.table_070_AmaniFactorTableAdapter.Update(dataSet_Sale5.Table_070_AmaniFactor);
                            this.table_075_Child_AmaniFactorTableAdapter.Update(dataSet_Sale5.Table_075_Child_AmaniFactor);




                            //if (DraftId > 0)
                            //{
                            //    //درج صفر در شماره سند حواله و صفر در شماره فاکتور فروش حواله
                            //    clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_007_PwhrsDraft SET Column07=0 , Column16=0 where ColumnId=" + DraftId);
                            //}
                            //if (PrefactorId > 0)
                            //{
                            //    //درج صفر در شماره فاکتور فروش پیش فاکتور
                            //    clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_007_FactorBefore set Column12=0 where ColumnId=" + PrefactorId);
                            //}

                            Class_BasicOperation.ShowMsg("", "حذف فاکتور با موفقیت انجام گرفت", "Information");
                            bt_New.Enabled = true;
                        }
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

                table_070_AmaniFactorBindingSource.EndEdit();
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

                table_070_AmaniFactorBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                gridEX1.Focus();
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }


        //private void gridEX_List_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        //{
        //    gridEX_List.CurrentCellDroppedDown = true;
        //    try
        //    {
        //        if (e.Column.Key == "column02")
        //            Class_BasicOperation.FilterGridExDropDown(sender, "column02", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

        //        else if (e.Column.Key == "GoodCode")
        //            Class_BasicOperation.FilterGridExDropDown(sender, "GoodCode", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);
        //    }
        //    catch { }
        //}



        private void table_070_AmaniFactorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.table_070_AmaniFactorBindingSource.Count > 0)
                {


                    ResidId = 0;
                    ResidNum = 0;
                    ReturnId = 0;
                    ReturnNum = 0;
                    ReturnDate = null;
                    ReturnDocId = 0;
                    ReturnDocId = 0;

                    DataRowView Row = (DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current;

                    ///اگر فاکتور فروش حواله داشته باشد ولی انبار و عملکرد نداشته باشد
                    //try
                    //{
                    //    if (Row["Column09"].ToString() != "0" && (string.IsNullOrWhiteSpace(Row["Column42"].ToString()) || string.IsNullOrWhiteSpace(Row["Column43"].ToString())))
                    //    {
                    //        DataTable dt = new DataTable();
                    //        DraftAdapter = new SqlDataAdapter("SELECT * from Table_007_PwhrsDraft where columnid=" + Row["Column09"] + "", ConWare);
                    //        DraftAdapter.Fill(dt);
                    //        if (dt.Rows.Count > 0)
                    //        {
                    //            Row["Column42"] = dt.Rows[0]["column03"];
                    //            Row["Column43"] = dt.Rows[0]["column04"];
                    //        }
                    //        else
                    //        {
                    //            Class_BasicOperation.ShowMsg("", "حواله  وجود ندارد", "Information");
                    //            return;

                    //        }

                    //    }
                    //}
                    //catch
                    //{
                    //}
                    //اگر برای فاکتور فقط حواله صادر شده باشد 
                    //if (Row["Column09"].ToString() != "0" && Row["Column10"].ToString() == "0")
                    //{
                    //    ///دسترسی برای ویرایش در صورت داشتن حواله ندارد
                    //    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 171))
                    //    {
                    //        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                    //        {
                    //            if (item.Key == "column06" || item.Key == "column13" ||
                    //                item.Key == "Column40" || item.Key == "Column41" || item.Key == "column12" || item.Key == "column05" || item.Key == "column21" || item.Key == "column22" || item.Key == "column23" || item.Key == "column24"
                    //                 || item.Key == "column25" || item.Key == "column26" || item.Key == "column27" || item.Key == "Column36")
                    //                item.Selectable = true;
                    //            else item.Selectable = false;

                    //        }
                    //        gridEX1.Enabled = true;
                    //        gridEX_List.AllowAddNew = InheritableBoolean.False;
                    //        gridEX_List.AllowEdit = InheritableBoolean.True;
                    //        gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                    //        gridEX_Extra.AllowDelete = InheritableBoolean.True;
                    //        gridEX_List.AllowDelete = InheritableBoolean.False;

                    //        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX_List.RootTable.Columns)
                    //        {
                    //            if (item.Key == "column02" || item.Key == "column03" ||
                    //                item.Key == "column04" || item.Key == "column05" || item.Key == "column06" || item.Key == "column07"
                    //                || item.Key == "column20" || item.Key == "column11"
                    //                || item.Key == "GoodCode"
                    //                )
                    //                item.Selectable = false;
                    //            else item.Selectable = true;

                    //            if (item.Key == "column08" || item.Key == "column09" || item.Key == "column10")
                    //            {
                    //                if (SalePrice)
                    //                    item.Selectable = true;
                    //                else item.Selectable = false;
                    //            }
                    //            if (item.Key == "column16" || item.Key == "column17" || item.Key == "column18")
                    //            {
                    //                if (DiscountLiner)
                    //                    item.Selectable = true;
                    //                else
                    //                    item.Selectable = false;

                    //            }


                    //        }

                    //    }

                    //    else//دسترسی ویرایش با وجود حواله دارد به خاطر دوئل اضافه شد 
                    //    {
                    //        gridEX1.Enabled = true;
                    //        gridEX_List.AllowAddNew = InheritableBoolean.True;
                    //        gridEX_List.AllowEdit = InheritableBoolean.True;
                    //        gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                    //        gridEX_Extra.AllowDelete = InheritableBoolean.True;
                    //        gridEX_List.AllowDelete = InheritableBoolean.True;

                    //        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                    //        {
                    //            if (item.Key == "column07" || item.Key == "column08" || item.Key == "Column09" || item.Key == "Column10" ||
                    //                item.Key == "column17" || item.Key == "column19" || item.Key == "column20")
                    //                item.Selectable = false;
                    //            else item.Selectable = true;
                    //        }

                    //        foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX_List.RootTable.Columns)
                    //        {
                    //            if (item.Key == "column03" || item.Key == "column07" ||
                    //                item.Key == "Column36" || item.Key == "Column37" || item.Key == "column11" || item.Key == "column19"
                    //                || item.Key == "column30"
                    //                || item.Key == "column20"
                    //                )
                    //                item.Selectable = false;
                    //            else item.Selectable = true;

                    //            if (item.Key == "column08" || item.Key == "column09" || item.Key == "column10")
                    //            {
                    //                if (SalePrice)
                    //                    item.Selectable = true;
                    //                else item.Selectable = false;
                    //            }
                    //            if (item.Key == "column16" || item.Key == "column17" || item.Key == "column18")
                    //            {
                    //                if (DiscountLiner)
                    //                    item.Selectable = true;
                    //                else
                    //                    item.Selectable = false;

                    //            }


                    //        }
                    //    }
                    //    //foreach (Janus.Windows.GridEX.GridEXColumn item in this.gridEX_List.RootTable.Columns)
                    //    //{
                    //    //    if (item.Key == "column08" || item.Key == "column09" || item.Key == "column10")
                    //    //    {
                    //    //        if (SalePrice)
                    //    //            item.Selectable = true;
                    //    //        else item.Selectable = false;
                    //    //    }
                    //    //    if (item.Key == "column16" || item.Key == "column17" || item.Key == "column18")
                    //    //    {
                    //    //        if (DiscountLiner)
                    //    //            item.Selectable = true;
                    //    //        else
                    //    //            item.Selectable = false;

                    //    //    }
                    //    //}
                    //    //if (DiscountEnd)
                    //    //    this.gridEX_Extra.Enabled = true;
                    //    //else
                    //    //    gridEX_Extra.Enabled = false;




                    //}
                    //در صورت اینکه فاکتور دارای سند باشد، یا مرجوعی باشد یا باطل شده باشد یا دارای پیش فاکتور باشد
                    //else if (Row["Column10"].ToString().Trim() != "0"
                    //    //|| Row["Column07"].ToString().Trim() != "0"
                    //        || Row["Column17"].ToString().Trim() != "False"
                    //        || Row["Column19"].ToString().Trim() != "False")
                    //{
                    //    foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
                    //    {
                    //        if (item.Key == "column06" || item.Key == "column13" || item.Key == "Column40" || item.Key == "Column41"
                    //            || item.Key == "column05" || item.Key == "column21" || item.Key == "column22" || item.Key == "column23" || item.Key == "column24"
                    //             || item.Key == "column25" || item.Key == "column26" || item.Key == "column27")
                    //            item.Selectable = true;
                    //        else item.Selectable = false;
                    //    }
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
                        gridEX1.Enabled = true;
                        gridEX1.AllowEdit = InheritableBoolean.True;
                        gridEX_List.AllowEdit = InheritableBoolean.True;

                        gridEX_List.AllowAddNew = InheritableBoolean.True;

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
                            if (item.Key == "column03" || item.Key == "column07" ||
                                item.Key == "Column36" || item.Key == "Column37" || item.Key == "column11" || item.Key == "column19"
                                || item.Key == "column30"
                                || item.Key == "column20"
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
                }
            }
            catch
            { }
            try
            {
                if (this.table_070_AmaniFactorBindingSource.Count > 0)
                {

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
            //else if (e.KeyCode == Keys.F9)
            //    toolStripButton7.ShowDropDown();
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_070_AmaniFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        _05_Sale.Reports.ReportForm frm = new Reports.ReportForm(
                                int.Parse(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), false, " فاکتور امانی ", 1, Convert.ToDouble(txt_TotalPrice.Value));
                        frm.ShowDialog();
                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void bt_Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
            {
                try
                {
                    bt_New.Enabled = true;
                    //     gridEX_Extra.UpdateData();
                    gridEX_List.UpdateData();
                    this.table_070_AmaniFactorBindingSource.EndEdit();
                    this.table_075_Child_AmaniFactorBindingSource.EndEdit();

                    //if (dataSet_Sale5.Table_070_AmaniFactor.GetChanges() != null || dataSet_Sale5.Table_075_Child_AmaniFactor.GetChanges() != null || dataSet_Sale5.Table_012_Child2_SaleFactor.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        bt_Save_Click(sender, e);
                    //    }
                    //}

                    dataSet_Sale5.EnforceConstraints = false;
                    int RowID = ReturnIDNumber(int.Parse(txt_Search.Text));
                    this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, RowID);
                    this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, RowID);

                    dataSet_Sale5.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
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
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_070_AmaniFactor where column01=" + FactorNum, con);
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
                    if (item.Name == "Frm_008_ViewAmaniFactors")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                _05_Sale.Frm_008_ViewAmaniFactors frm = new Frm_008_ViewAmaniFactors();
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

                DataTable CustomerTable = clDoc.ReturnTable
                (ConBase.ConnectionString, @"SELECT     dbo.Table_045_PersonInfo.ColumnId AS id, dbo.Table_045_PersonInfo.Column01 AS code, dbo.Table_045_PersonInfo.Column02 AS name, 
            dbo.Table_065_CityInfo.Column02 AS shahr, dbo.Table_060_ProvinceInfo.Column01 AS ostan, dbo.Table_045_PersonInfo.Column06 AS Address, 
            dbo.Table_045_PersonInfo.Column30,Table_045_PersonInfo.Column07 
            FROM         dbo.Table_060_ProvinceInfo INNER JOIN
            dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00 INNER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
            WHERE     (dbo.Table_045_PersonInfo.Column12 = 1)");
                gridEX1.DropDowns["Customer"].SetDataBinding(CustomerTable, "");
                gridEX1.DropDowns["Tel"].SetDataBinding(CustomerTable, "");

                gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)"), "");


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void Frm_002_Faktor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (chk_Award_Box.Checked)
            //{
            //    Properties.Settings.Default.AwardCompute = "Box";
            //    Properties.Settings.Default.Save();
            //}
            //else
            //{
            //    Properties.Settings.Default.AwardCompute = "Detail";
            //    Properties.Settings.Default.Save();
            //}
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




        private void gridEX_List_EditingCell(object sender, EditingCellEventArgs e)
        {
            //try
            //{
            //    if (((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)[
            //        "Column09"].ToString() != "0" &&
            //        ((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)[
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
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
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
                if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف کالا و ذخیره تغییرات هستید؟",
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {
                    e.Row.Delete();
                    this.table_070_AmaniFactorBindingSource.EndEdit();
                    this.table_075_Child_AmaniFactorBindingSource.EndEdit();

                    this.table_070_AmaniFactorTableAdapter.Update(dataSet_Sale5.Table_070_AmaniFactor);
                    this.table_075_Child_AmaniFactorTableAdapter.Update(dataSet_Sale5.Table_075_Child_AmaniFactor);

                }
                else
                    e.Cancel = true;
            }

            ////DataRowView Row = (DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current;
            ////       //اگر برای فاکتور فقط حواله صادر شده باشد 
            ////if (Row["Column09"].ToString() != "0" && Row["Column10"].ToString() == "0")
            ////{
            ////    e.Cancel = true;
            ////}

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

                    gridEX1.SetValue("Column36", (((DataRowView)gridEX1.RootTable.Columns["Column03"].DropDown.FindItem(gridEX1.GetValue("Column03")))["Column30"].ToString().Trim() == "" ? DBNull.Value :
                       (object)((DataRowView)gridEX1.RootTable.Columns["Column03"].DropDown.FindItem(gridEX1.GetValue("Column03")))["Column30"].ToString().Trim()));

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

        //        private void gridEX_List_CellUpdated(object sender, ColumnActionEventArgs e)
        //        {
        //            try
        //            {
        //                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
        //                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");

        //            }
        //            catch { }

        //            try
        //            {
        //                //درج نام کالا، کد کالا
        //                if (e.Column.Key == "column02")
        //                    gridEX_List.SetValue("GoodCode", gridEX_List.GetValue("column02").ToString());
        //                else if (e.Column.Key == "GoodCode")
        //                    gridEX_List.SetValue("column02", gridEX_List.GetValue("GoodCode").ToString());

        //                //درج تخفیف، اضافه خطی، واحد شمارش، تعداد در کارتن، تعداد در بسته
        //                if (e.Column.Key == "column02" || e.Column.Key == "GoodCode" ||
        //                    gridEX_List.GetRow().Cells["column30"].Text.ToString() == "True")
        //                {
        //                    GoodbindingSource.Filter = "GoodID=" +
        //                        gridEX_List.GetRow().Cells["column02"].Value.ToString();
        //                    gridEX_List.SetValue("tedaddarkartoon",
        //                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
        //                    gridEX_List.SetValue("tedaddarbaste",
        //                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
        //                    gridEX_List.SetValue("column03",
        //                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
        //                    //اضافه و تخفیف خطی
        //                    if (!Class_BasicOperation.CalLinearDis(int.Parse(gridEX1.GetValue("Column03").ToString())))
        //                    {

        //                        gridEX_List.SetValue("column16",
        //                           ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());
        //                        gridEX_List.SetValue("column18",
        //                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Extra"].ToString());
        //                        object Value = gridEX1.GetValue("Column36");
        //                        if (Value != null && !string.IsNullOrWhiteSpace(Value.ToString()))
        //                        {
        //                            DataRowView Row = (DataRowView)gridEX1.RootTable.Columns["Column36"].DropDown.FindItem(Value);
        //                            gridEX_List.SetValue("column16", Row["Column16"]);
        //                            gridEX_List.SetValue("column18", Row["Column17"]);
        //                        }

        //                    }
        //                    else
        //                    {
        //                        double[] array = clDoc.LastLinearDiscount(int.Parse(gridEX1.GetValue("Column03").ToString()), int.Parse(gridEX_List.GetValue("Column02").ToString()));
        //                        gridEX_List.SetValue("column16", array[0]);
        //                        gridEX_List.SetValue("column18", array[1]);
        //                    }
        //                    gridEX_List.SetValue("Column36", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Weight"].ToString());


        //                    //فراخوانی قیمتهای پیش فرض از  سیاستهای فروش
        //                    if (CustomerGroupList.Count == 0)
        //                    {
        //                        gridEX_List.SetValue("column10",
        //                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePrice"].ToString());
        //                        gridEX_List.SetValue("column09",
        //                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePackPrice"].ToString());
        //                        gridEX_List.SetValue("column08",
        //                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SaleBoxPrice"].ToString());
        //                    }
        //                    else
        //                    {
        //                        CustomerPricingbindingSource.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, @"select * from Table_029_CustomerGroupGoodPricing   
        //                            where Column01 IN (" + string.Join(",", CustomerGroupsArray) + ") and Column02=" +
        //                            gridEX_List.GetValue("column02").ToString() +
        //                            " and Column03<='" + gridEX1.GetValue("Column02").ToString() +
        //                            "' and Column04>='" + gridEX1.GetValue("Column02").ToString() + "'  order by Column01,Column02,Column03,Column04");

        //                        if (CustomerPricingbindingSource.Count > 0)
        //                        {
        //                            gridEX_List.SetValue("column10",
        //                                ((DataRowView)CustomerPricingbindingSource.CurrencyManager.Current)["Column07"].ToString());
        //                            gridEX_List.SetValue("column09",
        //                                ((DataRowView)CustomerPricingbindingSource.CurrencyManager.Current)["Column05"].ToString());
        //                            gridEX_List.SetValue("column08",
        //                                ((DataRowView)CustomerPricingbindingSource.CurrencyManager.Current)["Column06"].ToString());
        //                        }
        //                        else
        //                        {
        //                            gridEX_List.SetValue("column10",
        //                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
        //                                "SalePrice"].ToString());
        //                            gridEX_List.SetValue("column09",
        //                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
        //                                "SalePackPrice"].ToString());
        //                            gridEX_List.SetValue("column08",
        //                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
        //                                "SaleBoxPrice"].ToString());
        //                        }
        //                    }
        //                }
        //                //درج نرخ ارز از  جدول پرنت
        //                else if (e.Column.Key == "column14")
        //                {
        //                    object Value = gridEX_List.GetValue("Column14");
        //                    DataRowView Row = (DataRowView)gridEX_List.RootTable.Columns["Column14"].DropDown.FindItem(Value);
        //                    gridEX_List.SetValue("Column15", Row["Column02"]);
        //                }
        //                //درج تخفیف خطی از  سیاست فروش

        //                if (gridEX_List.GetRow().Cells["Column14"].Text.Trim() == "" && gridEX_List.GetRow().Cells["Column15"].Text.Trim() == "")
        //                {
        //                    if (gridEX1.GetRow().Cells["Column40"].Text.Trim() != "" &&
        //                          gridEX1.GetRow().Cells["Column41"].Text.Trim() != "")
        //                    {
        //                        gridEX_List.SetValue("Column14", gridEX1.GetValue("Column40").ToString());
        //                        gridEX_List.SetValue("Column15", gridEX1.GetValue("Column41").ToString());
        //                    }
        //                }
        //                //محاسبه تعداد کل
        //                gridEX_List.SetValue("column07",
        //                        (Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarkartoon"))) +
        //                        (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarbaste"))) +
        //                        Convert.ToDouble(gridEX_List.GetValue("column06")));


        //                //محاسبه وزن کل
        //                //gridEX_List.SetValue("Column37", Convert.ToDouble(gridEX_List.GetValue("Column07")) * Convert.ToDouble(gridEX_List.GetValue("Column36")));

        //                //محاسبه بر اساس کارتن
        //                if (!mnu_CalculatePrice.Checked)
        //                {
        //                    double TotalPrice = (gridEX1.GetValue("Column12").ToString() == "True" ?
        //                        (Convert.ToDouble(gridEX_List.GetValue("column04")) *
        //                        Convert.ToDouble(gridEX_List.GetValue("column08"))) +
        //                          (Convert.ToDouble(gridEX_List.GetValue("column05")) *
        //                        Convert.ToDouble(gridEX_List.GetValue("column09"))) +
        //                        (Convert.ToDouble(gridEX_List.GetValue("column06")) *
        //                        Convert.ToDouble(gridEX_List.GetValue("column10")))
        //                        : Convert.ToInt64((Convert.ToDouble(gridEX_List.GetValue("column04")) *
        //                        Convert.ToDouble(gridEX_List.GetValue("column08"))) +
        //                          (Convert.ToDouble(gridEX_List.GetValue("column05")) *
        //                        Convert.ToDouble(gridEX_List.GetValue("column09"))) +
        //                        (Convert.ToDouble(gridEX_List.GetValue("column06")) *
        //                        Convert.ToDouble(gridEX_List.GetValue("column10")))));

        //                    TotalPrice = TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column33").ToString()) / 100;
        //                    gridEX_List.SetValue("column11", TotalPrice);
        //                }
        //                //محاسبه بر اساس جز
        //                else
        //                {
        //                    Double TotalPrice = 
        //                        //(gridEX1.GetValue("Column12").ToString() == "True" ?
        //                        //(Convert.ToDouble(gridEX_List.GetValue("Column07").ToString()) *
        //                        //Convert.ToDouble(gridEX_List.GetValue("column10"))) :
        //                            Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column07").ToString()) *
        //                         Convert.ToDouble(gridEX_List.GetValue("column10")));
        //                    gridEX_List.SetValue("Column11", TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column33").ToString()) / 100);
        //                }

        //                if (e.Column.Key != "column16")
        //                {
        //                    DataTable DisTbl = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT     Column01, Column04, Column05, Column06, SUM(Column07) AS SumDis
        //                                                        FROM         dbo.Table_027_Discount_CommoditySpecialCustomer
        //                                                        WHERE     (Column02 <= N'" + gridEX1.GetValue("Column02").ToString() + @"') AND (Column03 >= N'" + gridEX1.GetValue("Column02").ToString() + @"')
        //                                                        and (Column01=" + gridEX1.GetValue("Column03").ToString() + ") AND (Column05 <= " + gridEX_List.GetValue("Column07").ToString() +
        //                    ") AND (Column06 >= " + gridEX_List.GetValue("Column07").ToString() + ") AND (Column04=" + gridEX_List.GetValue("Column02").ToString() + ") GROUP BY Column01, Column04, Column05, Column06");
        //                    if (DisTbl.Rows.Count > 0)
        //                    {
        //                        gridEX_List.SetValue("Column16", DisTbl.Rows[0]["SumDis"].ToString());
        //                    }
        //                }
        //                //اگر فاکتور ارزی بود تمام قیمتها بر اساس ریال میشود
        //                if (gridEX1.GetValue("Column12").ToString() == "True")
        //                {
        //                    Double jam, takhfif, ezafe;
        //                    jam = Convert.ToDouble(gridEX_List.GetValue("column11"));
        //                    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
        //                        takhfif = (jam * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
        //                    else takhfif = Convert.ToDouble(gridEX_List.GetValue("Column17").ToString());
        //                    ezafe = ((jam - takhfif) *
        //                        (Convert.ToDouble(gridEX_List.GetValue("column18")) / 100));
        //                    gridEX_List.SetValue("column17", takhfif);
        //                    gridEX_List.SetValue("column19", ezafe);
        //                    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);

        //                }
        //                else
        //                {
        //                    //در غیر این صورت بر اساس اعداد صحیح
        //                    Int64 jam, takhfif, ezafe;
        //                    jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")));
        //                    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
        //                        takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
        //                    else takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column17").ToString()));
        //                    ezafe = Convert.ToInt64(
        //                        //جمع
        //                        (Convert.ToDouble(gridEX_List.GetValue("column11")) -
        //                        //تخفیف
        //                        (Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100)) *
        //                        //درصد
        //                      Convert.ToDouble(gridEX_List.GetValue("column18"))
        //                      / 100);

        //                    gridEX_List.SetValue("column17", takhfif);
        //                    gridEX_List.SetValue("column19", ezafe);
        //                    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);
        //                }

        //                //محاسبه قیمتهای انتهای فاکتور
        //                txt_TotalPrice.Value = Convert.ToDouble(
        //                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
        //                    AggregateFunction.Sum).ToString());


        //                gridEX1.SetValue("Column15", Class_BasicOperation._UserName);
        //                gridEX1.SetValue("Column16", Class_BasicOperation.ServerDate());
        //            }
        //            catch
        //            { }
        //        }

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
                    gridEX_List.SetValue("tedaddarkartoon",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                    gridEX_List.SetValue("tedaddarbaste",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
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
                    //    gridEX_List.SetValue("Column36", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Weight"].ToString());


                    //فراخوانی قیمتهای پیش فرض از  سیاستهای فروش
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
                        CustomerPricingbindingSource.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, @"select * from Table_029_CustomerGroupGoodPricing   
                            where Column01 IN (" + string.Join(",", CustomerGroupsArray) + ") and Column02=" +
                            gridEX_List.GetValue("column02").ToString() +
                            " and Column03<='" + gridEX1.GetValue("Column02").ToString() +
                            "' and Column04>='" + gridEX1.GetValue("Column02").ToString() + "'  order by Column01,Column02,Column03,Column04");

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
                //درج نرخ ارز از  جدول پرنت
                else if (e.Column.Key == "column14")
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
                //محاسبه تعداد کل
                gridEX_List.SetValue("column07",
                        (Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarkartoon"))) +
                        (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarbaste"))) +
                        Convert.ToDouble(gridEX_List.GetValue("column06")));


                //محاسبه وزن کل
                // gridEX_List.SetValue("Column37", Convert.ToDouble(gridEX_List.GetValue("Column07")) * Convert.ToDouble(gridEX_List.GetValue("Column36")));

                //محاسبه بر اساس کارتن
                if (!mnu_CalculatePrice.Checked)
                {
                    double TotalPrice = (Convert.ToInt64((Convert.ToDouble(gridEX_List.GetValue("column04")) *
                        Convert.ToDouble(gridEX_List.GetValue("column08"))) +
                          (Convert.ToDouble(gridEX_List.GetValue("column05")) *
                        Convert.ToDouble(gridEX_List.GetValue("column09"))) +
                        (Convert.ToDouble(gridEX_List.GetValue("column06")) *
                        Convert.ToDouble(gridEX_List.GetValue("column10")))));

                    TotalPrice = TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column33").ToString()) / 100;
                    gridEX_List.SetValue("column11", TotalPrice);
                }
                //محاسبه بر اساس جز
                else
                {
                    Double TotalPrice = //(gridEX1.GetValue("Column12").ToString() == "True" ?
                        //(Convert.ToDouble(gridEX_List.GetValue("Column07").ToString()) *
                        //Convert.ToDouble(gridEX_List.GetValue("column10"))) :
                            Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column07").ToString()) *
                         Convert.ToDouble(gridEX_List.GetValue("column10")));
                    gridEX_List.SetValue("Column11", TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column33").ToString()) / 100);
                }

                if (e.Column.Key != "column16")
                {
                    DataTable DisTbl = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT     Column01, Column04, Column05, Column06, SUM(Column07) AS SumDis
                                                        FROM         dbo.Table_027_Discount_CommoditySpecialCustomer
                                                        WHERE     (Column02 <= N'" + gridEX1.GetValue("Column02").ToString() + @"') AND (Column03 >= N'" + gridEX1.GetValue("Column02").ToString() + @"')
                                                        and (Column01=" + gridEX1.GetValue("Column03").ToString() + ") AND (Column05 <= " + gridEX_List.GetValue("Column07").ToString() +
                    ") AND (Column06 >= " + gridEX_List.GetValue("Column07").ToString() + ") AND (Column04=" + gridEX_List.GetValue("Column02").ToString() + ") GROUP BY Column01, Column04, Column05, Column06");
                    if (DisTbl.Rows.Count > 0)
                    {
                        gridEX_List.SetValue("Column16", DisTbl.Rows[0]["SumDis"].ToString());
                    }
                }
                ////اگر فاکتور ارزی بود تمام قیمتها بر اساس ریال میشود
                //if (gridEX1.GetValue("Column12").ToString() == "True")
                //{
                //    Double jam, takhfif, ezafe;
                //    jam = Convert.ToDouble(gridEX_List.GetValue("column11"));
                //    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                //        takhfif = (jam * (Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                //    else takhfif = Convert.ToDouble(gridEX_List.GetValue("Column17").ToString());
                //    ezafe = ((jam - takhfif) *
                //        (Convert.ToDouble(gridEX_List.GetValue("column18")) / 100));
                //    gridEX_List.SetValue("column17", takhfif);
                //    gridEX_List.SetValue("column19", ezafe);
                //    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);

                //}
                //else
                {

                    if (e.Column.Key == "column17")
                        gridEX_List.SetValue("Column16", 0);

                    if (e.Column.Key == "column16")
                    {
                        gridEX_List.SetValue("column17", Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100));
                    }
                    //در غیر این صورت بر اساس اعداد صحیح
                    Int64 jam, takhfif, ezafe;
                    jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")));
                    if (Convert.ToDouble(gridEX_List.GetValue("column16")) > 0)
                        takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
                    else takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column17").ToString()));
                    ezafe = Convert.ToInt64(
                        //جمع
                        (Convert.ToDouble(gridEX_List.GetValue("column11")) -
                        //تخفیف
                        (Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100)) *
                        //درصد
                      Convert.ToDouble(gridEX_List.GetValue("column18"))
                      / 100);

                    gridEX_List.SetValue("column17", takhfif);
                    gridEX_List.SetValue("column19", ezafe);
                    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);
                }

                //محاسبه قیمتهای انتهای فاکتور
                txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                    AggregateFunction.Sum).ToString());
                //txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) -
                //    Convert.ToDouble(txt_VolumeGroup.Value.ToString()) -
                //    Convert.ToDouble(txt_SpecialGroup.Value.ToString()) -
                //    Convert.ToDouble(txt_SpecialCustomer.Value.ToString());
                //txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                //    Convert.ToDouble(txt_Extra.Value.ToString()) -
                //    Convert.ToDouble(txt_Reductions.Value.ToString());

                gridEX1.SetValue("Column15", Class_BasicOperation._UserName);
                gridEX1.SetValue("Column16", Class_BasicOperation.ServerDate());
            }
            catch
            { }
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


        private void mnu_ViewReturnFactor_Click(object sender, EventArgs e)
        {
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;

            if (gridEX1.GetRow().Cells["Column55"].Text.Trim() == "0" || gridEX1.GetRow().Cells["Column55"].Text.Trim() == "")
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
                            ((PWHRS._03_AmaliyatAnbar.Form03_WareReceipt)item).txt_Search.Text = gridEX1.GetRow().Cells["Column55"].Text;
                            ((PWHRS._03_AmaliyatAnbar.Form03_WareReceipt)item).bt_Search_Click(sender, e);
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form03_WareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form03_WareReceipt(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
                        int.Parse(gridEX1.GetValue("Column55").ToString()));
                    frm.ShowDialog();
                    int SaleId = int.Parse(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    dataSet_Sale5.EnforceConstraints = false;
                    this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, SaleId);
                    this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, SaleId);
                    dataSet_Sale5.EnforceConstraints = true;
                    table_070_AmaniFactorBindingSource_PositionChanged(sender, e);

                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
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

        private void chk_Award_Detial_Click(object sender, EventArgs e)
        {
            //if (chk_Award_Detial.Checked)
            //{
            //    chk_Award_Detial.Checked = false;
            //    chk_Award_Box.Checked = true;
            //}
            //else
            //{
            //    chk_Award_Detial.Checked = true;
            //    chk_Award_Box.Checked = false;
            //}
        }

        private void chk_Award_Box_Click(object sender, EventArgs e)
        {
            //if (chk_Award_Box.Checked)
            //{
            //    chk_Award_Detial.Checked = true;
            //    chk_Award_Box.Checked = false;
            //}
            //else
            //{
            //    chk_Award_Detial.Checked = false;
            //    chk_Award_Box.Checked = true;
            //}
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




        private Decimal LastBuyGoodPrice(int GoodCode)
        {
            DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, @"declare @t table(GoodCode int,Date nvarchar(50), Price decimal(18,3));
            insert into @t SELECT     Table_016_Child1_BuyFactor.column02,  MAX(Table_070_AmaniFactor.column02) AS Date,1
            FROM         Table_016_Child1_BuyFactor INNER JOIN
            Table_070_AmaniFactor ON Table_016_Child1_BuyFactor.column01 = Table_070_AmaniFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @"
            GROUP BY Table_016_Child1_BuyFactor.column02
            order by Table_016_Child1_BuyFactor.column02;
            
            declare @t2 table(codekala2 int , gheymat2 int,date2 nvarchar(50)
            ,UNIQUE (codekala2,gheymat2,date2)
            );

            insert into @t2 SELECT   dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, 
            dbo.Table_070_AmaniFactor.column02 AS Date
            FROM         dbo.Table_016_Child1_BuyFactor INNER JOIN
            dbo.Table_070_AmaniFactor ON dbo.Table_016_Child1_BuyFactor.column01 = dbo.Table_070_AmaniFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @"
            GROUP BY dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, dbo.Table_070_AmaniFactor.column02;
            update @t set Price=gheymat2 from @t2 as main_table where GoodCode=codekala2 and Date=date2; 
            select * from @t");

            if (Table.Rows.Count == 0)
                return 0;
            else
                return Convert.ToDecimal(Table.Rows[0]["Price"].ToString());

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
                table_070_AmaniFactorBindingSource.EndEdit();
                this.table_075_Child_AmaniFactorBindingSource.EndEdit();


                if (dataSet_Sale5.Table_070_AmaniFactor.GetChanges() != null || dataSet_Sale5.Table_075_Child_AmaniFactor.GetChanges() != null
                )
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save_Event(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select min(Column01) from Table_070_AmaniFactor),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_070_AmaniFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Sale5.EnforceConstraints = false;
                    this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));

                    this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_Sale5.EnforceConstraints = true;
                    this.table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
            {
                try
                {
                    gridEX1.UpdateData();
                    table_070_AmaniFactorBindingSource.EndEdit();
                    this.table_075_Child_AmaniFactorBindingSource.EndEdit();


                    if (dataSet_Sale5.Table_070_AmaniFactor.GetChanges() != null || dataSet_Sale5.Table_075_Child_AmaniFactor.GetChanges() != null
                      )
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save_Event(sender, e);
                        }
                    }


                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString,
                        "Select ISNULL((Select max(Column01) from Table_070_AmaniFactor where Column01<" +
                        ((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_070_AmaniFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_Sale5.EnforceConstraints = false;
                        this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));

                        this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_Sale5.EnforceConstraints = true;
                        this.table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
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
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
            {

                try
                {
                    gridEX1.UpdateData();
                    table_070_AmaniFactorBindingSource.EndEdit();
                    this.table_075_Child_AmaniFactorBindingSource.EndEdit();


                    if (dataSet_Sale5.Table_070_AmaniFactor.GetChanges() != null || dataSet_Sale5.Table_075_Child_AmaniFactor.GetChanges() != null
                    )
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save_Event(sender, e);
                        }
                    }

                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select Min(Column01) from Table_070_AmaniFactor where Column01>" + ((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + "),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_070_AmaniFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_Sale5.EnforceConstraints = false;
                        this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));

                        this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_Sale5.EnforceConstraints = true;
                        this.table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
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
                table_070_AmaniFactorBindingSource.EndEdit();
                this.table_075_Child_AmaniFactorBindingSource.EndEdit();


                if (dataSet_Sale5.Table_070_AmaniFactor.GetChanges() != null || dataSet_Sale5.Table_075_Child_AmaniFactor.GetChanges() != null
                 )
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save_Event(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select max(Column01) from Table_070_AmaniFactor),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_070_AmaniFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Sale5.EnforceConstraints = false;
                    this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));

                    this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_Sale5.EnforceConstraints = true;
                    this.table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
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
        private void gridEX_List_RecordAdded(object sender, EventArgs e)
        {
            try
            {
                txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                    AggregateFunction.Sum).ToString());

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

        private void bt_Attachments_Click(object sender, EventArgs e)
        {
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
            {
                // if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 95))
                {
                    try
                    {
                        DataRowView Row = (DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current;
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

        private void btn_TotalPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_070_AmaniFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        _05_Sale.Reports.Form_SaleFactorTotalPrint frm = new Reports.Form_SaleFactorTotalPrint(
                                int.Parse(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), false);
                        frm.ShowDialog();
                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch { }
        }

        private void bt_ExportResid_Click(object sender, EventArgs e)
        {
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 71))
                        throw new Exception("کاربر گرامی شما امکان صدور حواله انبار را ندارید");

                    if (clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column56", RowID) != 0)
                        throw new Exception("برای این فاکتور حواله صادر شده است");
                    if (clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column55", RowID) != 0)
                        throw new Exception("برای این فاکتور رسید صادر شده است");



                    if (clDoc.AllService(table_075_Child_AmaniFactorBindingSource))
                        throw new Exception("به علت عدم وجود کالا، حواله ای برای این فاکتور صادر نخواهد شد");

                    Save_Event(sender, e);



                    DataTable Table = new DataTable();
                    Table.Columns.Add("GoodID", Type.GetType("System.String"));
                    Table.Columns.Add("GoodName", Type.GetType("System.String"));
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        Table.Rows.Add(item.Cells["Column02"].Value,
                            item.Cells["Column02"].Text);
                    }

                    if (gridEX1.GetRow().Cells["Column58"].Text.Trim() != "" && gridEX1.GetRow().Cells["Column59"].Text.Trim() != "")
                    {

                        _05_Sale.Frm_035_TransferInformationDialog frm = new Frm_035_TransferInformationDialog(this.table_075_Child_AmaniFactorBindingSource, ((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current),
                            ((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column02"].ToString(),
                            Table, Convert.ToInt16(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column58"]),
                            Convert.ToInt16(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column59"]));
                        frm.ShowDialog();
                    }
                    else Class_BasicOperation.ShowMsg("", "انبار مبدا و مقصد را مشخص کنید", "None");

                }

                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                DS.Tables["Draft"].Clear();
                DraftAdapter.Fill(DS, "Draft");


                gridEX1.DropDowns["Recipt"].DataSource = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT     columnid, column01
FROM         Table_011_PwhrsReceipt");

                int _ID = int.Parse(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                dataSet_Sale5.EnforceConstraints = false;
                this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, _ID);
                this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, _ID);
                dataSet_Sale5.EnforceConstraints = true;
                this.table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
            }
        }

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 69))
                        throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");

                    if (((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                        Save_Event(sender, e);

                    string RowID = ((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column57", RowID) != 0)
                    {
                        dataSet_Sale5.EnforceConstraints = false;
                        this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, int.Parse(RowID));
                        this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, int.Parse(RowID));

                        dataSet_Sale5.EnforceConstraints = true;
                        DS.Tables["Doc"].Clear();
                        DocAdapter.Fill(DS, "Doc");
                        DS.Tables["Draft"].Clear();
                        DraftAdapter.Fill(DS, "Draft");
                        gridEX1.DropDowns["Recipt"].DataSource = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT     columnid, column01
FROM         Table_011_PwhrsReceipt");


                        this.table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
                        throw new Exception("برای این فاکتور سند حسابداری صادر شده است");
                    }


                    Save_Event(sender, e);

                    //بعد از سیو کردن اطلاعات سطر خالی می شود
                    DataRowView Row = (DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current;







                    if (gridEX1.GetRow().Cells["Column58"].Text.Trim() != "" && gridEX1.GetRow().Cells["Column59"].Text.Trim() != "")
                    {
                        if (Row["Column56"].ToString().Trim() != "" && int.Parse(Row["Column56"].ToString()) > 0)
                        {
                            //***************************if Finance Type is Periodic: Just export Doc for factor and Doc number will be save in Draft's n
                            //سیستم ادواری
                            if (!Class_BasicOperation._FinType)
                            {
                                _05_Sale.Frm_036_ExportDocInformationAmaniFactor frm = new Frm_036_ExportDocInformationAmaniFactor(true, false, false, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column58"]), Convert.ToInt16(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column59"]));
                                frm.ShowDialog();
                            }
                            //سیستم دائمی
                            else
                            {
                                _05_Sale.Frm_036_ExportDocInformationAmaniFactor frm = new Frm_036_ExportDocInformationAmaniFactor(true, false, true, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column58"]), Convert.ToInt16(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column59"]));
                                frm.ShowDialog();
                            }

                        }
                        //اگر حواله صادر نشده باشد
                        else
                        {
                            bool _DraftPanel = true;
                            if (clDoc.AllService(table_075_Child_AmaniFactorBindingSource))
                                _DraftPanel = false;

                            //سیستم ادواری
                            if (!Class_BasicOperation._FinType)
                            {
                                _05_Sale.Frm_036_ExportDocInformationAmaniFactor frm = new Frm_036_ExportDocInformationAmaniFactor(true, _DraftPanel, false, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column58"]), Convert.ToInt16(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column59"]));
                                frm.ShowDialog();
                            }
                            //سیستم دائمی
                            else
                            {
                                _05_Sale.Frm_036_ExportDocInformationAmaniFactor frm = new Frm_036_ExportDocInformationAmaniFactor(true, _DraftPanel, true, int.Parse(Row["ColumnId"].ToString()), Convert.ToInt16(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column58"]), Convert.ToInt16(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column59"]));
                                frm.ShowDialog();
                            }
                        }
                        dataSet_Sale5.EnforceConstraints = false;
                        this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, int.Parse(RowID));
                        this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, int.Parse(RowID));
                        dataSet_Sale5.EnforceConstraints = true;
                        DS.Tables["Doc"].Clear();
                        DocAdapter.Fill(DS, "Doc");
                        DS.Tables["Draft"].Clear();
                        DraftAdapter.Fill(DS, "Draft");
                        gridEX1.DropDowns["Recipt"].DataSource = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT     columnid, column01
FROM         Table_011_PwhrsReceipt");

                        this.table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
                    }
                    else Class_BasicOperation.ShowMsg("", "انبار مبدا و مقصد را مشخص کنید", "None");

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void mnu_DelDraft_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
            {
                try
                {
                    int RowID = int.Parse(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int DraftId = clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column56", RowID.ToString());

                    if (DraftId != 0)
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 24) && UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 25))
                        {



                            clDoc.ConfirmedDraftReceipt("Draft", DraftId.ToString());


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
                    gridEX1.DropDowns["Recipt"].DataSource = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT     columnid, column01
FROM         Table_011_PwhrsReceipt");
                    dataSet_Sale5.EnforceConstraints = false;
                    this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, (RowID));
                    this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, (RowID));
                    dataSet_Sale5.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }
        private void DelPaper()
        {
            string id = ((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
            string RowIDDraft = clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column56", id).ToString();
            int SanadID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column07", RowIDDraft);
            int SaleID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column16", RowIDDraft);
            int ReturnID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column19", RowIDDraft);
            int OrderID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column17", RowIDDraft);
            int ExitID = clDoc.WHRSOperationalColumnValue("Table_007_PwhrsDraft", "Column15", RowIDDraft);
            string AmaniID = ((DataRowView)table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
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



                    Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowIDDraft));
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowIDDraft);

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



                    Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowIDDraft));
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + int.Parse(RowIDDraft);
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
                    WHERE " + ConSale.Database + @".dbo.Table_006_OrderDetails.column01=" + OrderID + " AND b.Column01=" + RowIDDraft;
                //اگر محاسبه جایزه در سفارش بر اساس خروجی باشد کلیه جوایز مربوط به سفارش حذف می شود
                if (Convert.ToBoolean(clDoc.ExScalar(ConSale.ConnectionString, "Table_005_OrderHeader", "Column34", "ColumnId", OrderID.ToString())))
                {

                    command += " Delete " + ConSale.Database + ".dbo.Table_006_OrderDetails where Column31=1 and Column01=" + OrderID;

                }
            }

            //****Delete Exit Paper
            if (ExitID != 0)
            {
                command += " Delete " + this.ConWare.Database + ".dbo.Table_009_ExitPwhrs where Column19=" + RowIDDraft;
            }
            //***Delete Detail
            command += " Delete " + this.ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where Column01=" + RowIDDraft;

            //***Delete Header
            command += " Delete " + this.ConWare.Database + ".dbo.Table_007_PwhrsDraft where ColumnId=" + RowIDDraft;

            //Update Table_035_DraftRequest
            command += " UPDATE " + this.ConWare.Database + ".dbo.Table_035_DraftRequest SET Column07=0 , Column12=0 where Column07=" + RowIDDraft;

            command += " UPDATE  " + ConSale.Database + ".dbo.Table_070_AmaniFactor SET Column56=0 where ColumnId=" + AmaniID;

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
        private void bt_NotConfirmDraft_Click(object sender, EventArgs e)
        {
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
            {
                int DraftId = clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column56", ((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
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

        private void mnu_DelRecipt_Click(object sender, EventArgs e)
        {
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
            {
                try
                {
                    int RowID = int.Parse(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int ReceiptId = clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column55", RowID.ToString());

                    if (ReceiptId != 0)
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 20))
                        {

                            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
                            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
                            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
                            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;

                            PWHRS._03_AmaliyatAnbar.Form03_WareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form03_WareReceipt
                            (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21), -1);
                            frm.txt_Search.Text = clDoc.ExScalar(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01", "ColumnId", ReceiptId.ToString());
                            frm.bt_Search_Click(sender, e);
                            frm.bt_Del_Click(sender, e);

                            clDoc.RunSqlCommand(ConSale.ConnectionString, @"update Table_070_AmaniFactor set column55=0 where columnid= " + RowID);


                        }
                        else
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
                    }
                    //DS.Tables["Doc"].Clear();
                    //DocAdapter.Fill(DS, "Doc");
                    //DS.Tables["Resid"].Clear();
                    gridEX1.DropDowns["Recipt"].DataSource = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT     columnid, column01
FROM         Table_011_PwhrsReceipt");
                    dataSet_Sale5.EnforceConstraints = false;
                    this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, (RowID));
                    this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, (RowID));
                    dataSet_Sale5.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_NotConfirmRecipt_Click(object sender, EventArgs e)
        {
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
            {
                int ReceiptId = clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column55", ((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());

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

        private void mnu_Drafts_Click(object sender, EventArgs e)
        {
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;

            if (gridEX1.GetRow().Cells["Column56"].Text.Trim() == "0" || gridEX1.GetRow().Cells["Column56"].Text.Trim() == "")
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
                            ((PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts)item).txt_Search.Text = gridEX1.GetRow().Cells["Column56"].Text;
                            ((PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts)item).bt_Search_Click(sender, e);
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts frm = new PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
                        int.Parse(gridEX1.GetValue("Column56").ToString()));
                    frm.ShowDialog();
                    int SaleId = int.Parse(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    dataSet_Sale5.EnforceConstraints = false;
                    this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, SaleId);
                    this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, SaleId);

                    table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
                    dataSet_Sale5.EnforceConstraints = true;
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
        }

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            int SanadId = 0;
            if (this.table_070_AmaniFactorBindingSource.Count > 0)
                SanadId = (((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column57"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column57"].ToString()));

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

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            try
            {
                if (this.table_070_AmaniFactorBindingSource.Count > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 70))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    int RowID = int.Parse(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int SanadID = int.Parse(((DataRowView)this.table_070_AmaniFactorBindingSource.CurrencyManager.Current)["Column57"].ToString());



                    if (SanadID != 0)
                    {
                        string Message = "آیا مایل به حذف سند مربوط به این فاکتور هستید؟";


                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.IsFinal_ID(SanadID);

                            DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=93 and Column17=" + RowID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=93 and Column17=" + RowID;




                            command += " UPDATE " + ConSale.Database + ".dbo.Table_070_AmaniFactor SET Column57=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + RowID;


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
                    dataSet_Sale5.EnforceConstraints = false;
                    this.table_070_AmaniFactorTableAdapter.FillByID(this.dataSet_Sale5.Table_070_AmaniFactor, RowID);
                    this.table_075_Child_AmaniFactorTableAdapter.FillBy_HeaderID(this.dataSet_Sale5.Table_075_Child_AmaniFactor, RowID);

                    table_070_AmaniFactorBindingSource_PositionChanged(sender, e);
                    dataSet_Sale5.EnforceConstraints = true;
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

    }
}
