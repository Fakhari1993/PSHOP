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
using Stimulsoft.Report;

namespace PSHOP._05_Sale
{
    public partial class Frm_002_ClubFactor : Form
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
        Class_UserScope UserScope = new Class_UserScope();
        bool SalePrice, DiscountLiner, DiscountEnd = false;

        public Frm_002_ClubFactor(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        public Frm_002_ClubFactor(bool del, int ID)
        {
            _del = del;
            _ID = ID;
            InitializeComponent();
        }

        private void Frm_002_PishFaktor_Load(object sender, EventArgs e)
        {
            ToastNotification.ToastBackColor = Color.Aquamarine;
            ToastNotification.ToastForeColor = Color.Black;

            gridEX1.RootTable.Columns["Column13"].DefaultValue = Class_BasicOperation._UserName;
            gridEX1.RootTable.Columns["Column15"].DefaultValue = Class_BasicOperation._UserName;
            gridEX1.RootTable.Columns["Column14"].DefaultValue = Class_BasicOperation.ServerDate();
            gridEX1.RootTable.Columns["Column16"].DefaultValue = Class_BasicOperation.ServerDate();

            GoodbindingSource.DataSource = clGood.MahsoolInfo("");
            DataTable GoodTable = clGood.MahsoolInfo("");
            gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");


            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString,"SELECT * FROM Table_070_CountUnitInfo"), "");
            gridEX_List.DropDowns["Center"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString,"Select Column00,Column01,Column02 FROM Table_030_ExpenseCenterInfo"), "");
            gridEX_List.DropDowns["Project"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString,"SELECT Column00,Column01,Column02 FROM Table_035_ProjectInfo"), "");
            //مسئول فروش
            gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from PeopleScope(8,3)"), "");
            gridEX1.DropDowns["Customer"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select Columnid,Column02 from Table_045_PersonInfo");
            gridEX1.DropDowns["CustomerClub"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column03+' '+Column02 as Column03,Column04,Column13 from Table_215_CustomerClub");

            gridEX_Extra.DropDowns["Type"].DataSource=clDoc.ReturnTable(ConSale.ConnectionString,"SELECT * FROM Table_024_Discount");
            gridEX1.DropDowns["Ware"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from Table_001_PWHRS");
            gridEX1.DropDowns["Func"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from table_005_PwhrsOperation where Column16=1");

            gridEX1.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes"), "");


            if (_ID != 0)
            {
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, _ID);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, _ID);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, _ID);
                table_010_SaleFactorBindingSource_PositionChanged(sender, e);
            }
            SalePrice = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 162);
            DiscountLiner = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 163);
            DiscountEnd = UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 164);

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
                if (gridEX1.GetRow().Cells["Column06"].Text.Trim() == "")
                    gridEX1.SetValue("Column06", Properties.Settings.Default.SaleDescription);
            

                gridEX1.Select();
             
                bt_New.Enabled = false;

                gridEX1.AllowEdit = InheritableBoolean.True;
                gridEX1.AllowAddNew = InheritableBoolean.True;
                gridEX_List.AllowAddNew = InheritableBoolean.True;
                gridEX_List.AllowEdit = InheritableBoolean.True;
                gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                gridEX_Extra.AllowDelete = InheritableBoolean.True;
                gridEX_List.AllowDelete = InheritableBoolean.True;

                
                gridEX1.Col = 4;

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void Save_Event(object sender,EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0 &&
               gridEX_List.AllowEdit == InheritableBoolean.True &&
               gridEX1.GetRow().Cells["Column03"].Text.Trim() != "")
            {
                 if (Properties.Settings.Default.ShowPriceAlert > 0)
                        CheckGoodsPrice();
                    this.Cursor = Cursors.WaitCursor;
                    gridEX_List.UpdateData();
                    gridEX_Extra.UpdateData();
                    DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;
                    if (Row["Column01"].ToString().StartsWith("-"))
                    {
                        gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "Column01").ToString());
                        Row["Column61"] = 0;

                        this.table_010_SaleFactorBindingSource.EndEdit();
                    }
               
                    txt_TotalPrice.Value = Convert.ToDouble(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],AggregateFunction.Sum).ToString());
                    txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
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
                  

                    //Extra-Reductions
                    Janus.Windows.GridEX.GridEXFilterCondition Filter2 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                    Row["Column32"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                    Filter2.Value1 = true;
                    Row["Column33"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();

                    this.table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                    this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_Sale.Table_011_Child1_SaleFactor);
                    this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_Sale.Table_012_Child2_SaleFactor);


                   
                   
                    if (sender == bt_Save || sender == this)
                        Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                    int _ID = int.Parse(Row["ColumnId"].ToString());
               


                    if (gridEX1.GetRow().Cells["Column11"].Text.Trim() == "")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ارسال پیامک به خریدار فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            DataTable TextTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_220_SMSText where columnId=1");
                            DataTable LineTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_175_SMS where Columnid=" + TextTable.Rows[0]["Column03"].ToString());
                            try
                            {
                                if (((DataRowView)gridEX1.RootTable.Columns["Column18"].DropDown.FindItem(gridEX1.GetValue("Column18")))["Column04"].ToString().Trim() == "")
                                    throw new Exception("تلفن همراه خریدار مشخص نشده است");

                                if (LineTable.Rows[0]["Usr"].ToString().Trim() == "" || LineTable.Rows[0]["Pass"].ToString().Trim() == "" ||
                                    LineTable.Rows[0]["Line"].ToString().Trim() == "")
                                    throw new Exception("اطلاعات مربوط به پنل پیامک را مشخص کنید");


                                IWebService Ws = new IWebService();
                                Random rand = new Random();
                                string crand = rand.Next(11111111, 99999999).ToString();
                                string returnCode = Ws.SendSms(LineTable.Rows[0]["Usr"].ToString().Trim(), LineTable.Rows[0]["Pass"].ToString().Trim(),
                                    TextTable.Rows[0]["Column02"].ToString().Trim(),
                                    ((DataRowView)gridEX1.RootTable.Columns["Column18"].DropDown.FindItem(gridEX1.GetValue("Column18")))["Column04"].ToString().Trim()
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

                    dataSet_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, _ID);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, _ID);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, _ID);
                    dataSet_Sale.EnforceConstraints = true;
                    table_010_SaleFactorBindingSource_PositionChanged(sender, e);
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
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
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

                    if (clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_034_SaleFactor_Child3 where Column01=" +RowID).Rows.Count > 0)
                        throw new Exception("این فاکتور دارای اطلاعات مربوط به تسویه است. جهت حذف فاکتور ابتدا، اطلاعات مربوطه را حذف کنید");

                    int DocId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID);
                    int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID);
                    int PrefactorId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column07", RowID);

                    if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری مربوط نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        if (DocId > 0)
                        {
                            clDoc.IsFinal_ID(DocId);
                            //حذف سند فاکتور 
                            clDoc.DeleteDetail_ID(DocId, 15, int.Parse(RowID));
                            //حذف سند مربوط به حواله
                            clDoc.DeleteDetail_ID(DocId, 26, DraftId);
                        }
                       
                        //حذف فاکتور
                        foreach (DataRowView item in this.table_011_Child1_SaleFactorBindingSource)
                        {
                            item.Delete();
                        }
                        this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                        this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_Sale.Table_011_Child1_SaleFactor);
                        foreach (DataRowView item in this.table_012_Child2_SaleFactorBindingSource)
                        {
                            item.Delete();
                        }
                        this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                        this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_Sale.Table_012_Child2_SaleFactor);
                        this.table_010_SaleFactorBindingSource.RemoveCurrent();
                        this.table_010_SaleFactorBindingSource.EndEdit();
                        this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);

                        if (DraftId > 0)
                        {
                            //درج صفر در شماره سند حواله و صفر در شماره فاکتور فروش حواله
                            clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_007_PwhrsDraft SET Column07=0 , Column16=0 where ColumnId=" + DraftId);
                        }
                        if (PrefactorId > 0)
                        {
                            //درج صفر در شماره فاکتور فروش پیش فاکتور
                            clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_007_FactorBefore set Column12=0 where ColumnId=" + PrefactorId);
                        }

                        Class_BasicOperation.ShowMsg("", "حذف فاکتور با موفقیت انجام گرفت", "Information");
                        bt_New.Enabled = true;
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

                table_010_SaleFactorBindingSource.EndEdit();
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

                table_010_SaleFactorBindingSource.EndEdit();
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
            try
            {
                if ((e.Column.Key == "column02" || e.Column.Key == "GoodCode")  && gridEX_List.Row==-1)
                {
                    gridEX_List.SetValue("column02", gridEX_List.GetValue("GoodCode").ToString());
                    gridEX_List.SetValue("Column06", 1);
                    
                    //درج تخفیف، اضافه خطی، واحد شمارش، تعداد در کارتن، تعداد در بسته
                  
                        GoodbindingSource.Filter = "GoodID=" +
                            gridEX_List.GetRow().Cells["column02"].Value.ToString();
                        gridEX_List.SetValue("tedaddarkartoon",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                        gridEX_List.SetValue("tedaddarbaste",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                        gridEX_List.SetValue("column03",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                       

                        gridEX_List.SetValue("column10",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePrice"].ToString());
                        gridEX_List.SetValue("column09",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePackPrice"].ToString());
                        gridEX_List.SetValue("column08",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SaleBoxPrice"].ToString());



                    

                    //محاسبه تعداد کل
                    gridEX_List.SetValue("column07",
                            (Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarkartoon"))) +
                            (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarbaste"))) +
                            Convert.ToDouble(gridEX_List.GetValue("column06")));



                    double TotalPrice = (gridEX1.GetValue("Column12").ToString() == "True" ?
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
                        Convert.ToDouble(gridEX_List.GetValue("column10")))));

                    TotalPrice = TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column33").ToString()) / 100;
                    gridEX_List.SetValue("column11", TotalPrice);


                   

                    //در غیر این صورت بر اساس اعداد صحیح
                    Int64 jam, takhfif, ezafe;
                    jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")));
                    takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
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
                    gridEX_List.DataChanged = true;
                    gridEX_List.UpdateData();
                    gridEX_List.MoveToNewRecord();
                  
                    
                    //محاسبه قیمتهای انتهای فاکتور
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

        private void gridEX_Extra_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_Extra.CurrentCellDroppedDown = true;
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
                    ReturnDocId = 0;
                    ReturnDocId = 0;

                    DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;
                    //اگر برای فاکتور حواله صادر شده باشد 
                    //در صورت اینکه فاکتور دارای سند باشد، یا مرجوعی باشد یا باطل شده باشد یا دارای پیش فاکتور باشد
                     if (Row["Column10"].ToString().Trim() != "0" || Row["Column09"].ToString() != "0"
                            || Row["Column17"].ToString().Trim() != "False"
                            || Row["Column19"].ToString().Trim() != "False")
                    {
                       
                        gridEX1.Enabled = false;
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

                      
                    }
                }
            }
            catch
            { }
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
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
        
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            //if (this.table_010_SaleFactorBindingSource.Count > 0)
            //{
            //    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
            //    {
            //        _05_Sale.Reports.Form_SaleFactorPrint frm = new Reports.Form_SaleFactorPrint(
            //                int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()),true);
            //        frm.ShowDialog();
            //    }
            //    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
            //}
            Save_Event(sender, e);
            if (gridEX_List.GetRows().Count() > 0)
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                dt.Columns.Add("Name"); dt.Columns.Add("Count"); dt.Columns.Add("Fi"); dt.Columns.Add("Sum");
                dt1.Columns.Add("Type"); dt1.Columns.Add("Titel"); dt1.Columns.Add("Pay");

                for (int i = 0; i < gridEX_List.GetRows().Count(); i++)
                {
                    dt.Rows.Add();
                    dt.Rows[dt.Rows.Count - 1]["Name"] = gridEX_List.GetRow(i).Cells["column02"].Text.ToString();

                    if (gridEX_List.GetRow(i).Cells["column06"].Value.ToString().Trim() != "")
                        dt.Rows[dt.Rows.Count - 1]["Count"] = gridEX_List.GetRow(i).Cells["column06"].Value.ToString();
                    else
                        dt.Rows[dt.Rows.Count - 1]["Count"] = "0";

                    if (gridEX_List.GetRow(i).Cells["column10"].Value.ToString().Trim() != "")
                        dt.Rows[dt.Rows.Count - 1]["Fi"] = gridEX_List.GetRow(i).Cells["column10"].Value.ToString();
                    else
                        dt.Rows[dt.Rows.Count - 1]["Fi"] = "0";

                    if (gridEX_List.GetRow(i).Cells["column11"].Value.ToString().Trim() != "")
                        dt.Rows[dt.Rows.Count - 1]["Sum"] = gridEX_List.GetRow(i).Cells["column11"].Value.ToString();
                    else
                        dt.Rows[dt.Rows.Count - 1]["Sum"] = "0";
                }

                for (int i = 0; i < gridEX_Extra.GetRows().Count(); i++)
                {
                    dt1.Rows.Add();

                    if (gridEX_Extra.GetRow(i).Cells["column05"].Value.ToString().Trim() == "False")
                        dt1.Rows[dt1.Rows.Count - 1]["Type"] = "+";
                    else
                        dt1.Rows[dt1.Rows.Count - 1]["Type"] = "-";

                    dt1.Rows[dt1.Rows.Count - 1]["Titel"] = gridEX_Extra.GetRow(i).Cells["column02"].Text.ToString();

                    if (gridEX_Extra.GetRow(i).Cells["column04"].Text.ToString().Trim() != "")
                        dt1.Rows[dt1.Rows.Count - 1]["Pay"] = gridEX_Extra.GetRow(i).Cells["column04"].Text.ToString();
                    else
                        dt1.Rows[dt1.Rows.Count - 1]["Pay"] = "0";
                }

                if (dt.Rows.Count > 0)
                {
                    StiReport st = new StiReport();
                    st.Load("Sale_Rpt01_SaleFactor.mrt");
                    st.Pages.Items[0].Height = 2;
                    st.Pages.Items[0].Height += (dt.Rows.Count * 0.5) + 3;
                    if (dt1.Rows.Count > 0)
                    {
                        st.Pages.Items[0].Height += (dt1.Rows.Count * 0.5) + 2;
                    }
                    st.RegData("Factor", dt);
                    st.RegData("PN", dt1);
                    st.Compile();
                    st["Customer"] = gridEX1.GetRow().Cells["column18"].Text.ToString();
                    st["Date"] = gridEX1.GetRow().Cells["column02"].Text.ToString();
                    st["Number"] = gridEX1.GetRow().Cells["column01"].Text.ToString();
                    st["Company"] = Class_BasicOperation.LogoTable().Rows[0]["Column01"].ToString();
                    st["sumfactor"] = txt_TotalPrice.Text.Trim();
                    st["sumn"] = Double.Parse(txt_Reductions.Text.Trim());
                    st["sump"] = Double.Parse(txt_Extra.Text.Trim());
                    st["sum"] = txt_EndPrice.Text.Trim();

                    st.Render();
                    st.Show();
                }
            }
            else
            {
                MessageBox.Show("لیست محصولات بدون مقدار است", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                gridEX_Extra.DropDowns["Type"].DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "SELECT * FROM Table_024_Discount");
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
                (ConBase.ConnectionString, @"Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
                gridEX1.DropDowns["Customer"].SetDataBinding(CustomerTable, "");
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
            if (Control.ModifierKeys != Keys.Control)
                gridEX1.CurrentCellDroppedDown = true;
            gridEX1.SetValue("Column15", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column16", Class_BasicOperation.ServerDate());

            if (e.Column.Key == "column18")
            {
                Class_BasicOperation.FilterGridExDropDown(sender, "column18", "Column04", "Column03", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.ACCColumn);
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
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column18");
                }
                catch 
                {     }

                if (e.Column.Key == "column18")
                {
                    gridEX1.SetValue("Column03", ((DataRowView)gridEX1.RootTable.Columns["column18"].DropDown.FindItem(gridEX1.GetValue("column18")))["Column13"]);
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
                    gridEX_List.SetValue("tedaddarkartoon",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                    gridEX_List.SetValue("tedaddarbaste",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                    gridEX_List.SetValue("column03",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                    //gridEX_List.SetValue("column16",
                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());
                    //gridEX_List.SetValue("column18",
                    //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Extra"].ToString());

                   
                        gridEX_List.SetValue("column10",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePrice"].ToString());
                        gridEX_List.SetValue("column09",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePackPrice"].ToString());
                        gridEX_List.SetValue("column08",
                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SaleBoxPrice"].ToString());
                  
              
                    
                }
               
                //محاسبه تعداد کل
                gridEX_List.SetValue("column07",
                        (Convert.ToDouble(gridEX_List.GetValue("column04")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarkartoon"))) +
                        (Convert.ToDouble(gridEX_List.GetValue("column05")) * Convert.ToDouble(gridEX_List.GetValue("tedaddarbaste"))) +
                        Convert.ToDouble(gridEX_List.GetValue("column06")));


             
                    double TotalPrice = (gridEX1.GetValue("Column12").ToString() == "True" ?
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
                        Convert.ToDouble(gridEX_List.GetValue("column10")))));

                    TotalPrice = TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column33").ToString()) / 100;
                    gridEX_List.SetValue("column11", TotalPrice);
             

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
              
                    //در غیر این صورت بر اساس اعداد صحیح
                    Int64 jam, takhfif, ezafe;
                    jam = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")));
                    takhfif = Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16")) / 100);
                    ezafe = Convert.ToInt64(
                        //جمع
                        (Convert.ToDouble(gridEX_List.GetValue("column11")) - 
                        //تخفیف
                        (Convert.ToDouble(gridEX_List.GetValue("column11")) * Convert.ToDouble(gridEX_List.GetValue("column16"))/100)) *
                        //درصد
                      Convert.ToDouble(gridEX_List.GetValue("column18"))
                      / 100);

                    gridEX_List.SetValue("column17", takhfif);
                    gridEX_List.SetValue("column19", ezafe);
                    gridEX_List.SetValue("column20", (jam - takhfif) + ezafe);


                //محاسبه قیمتهای انتهای فاکتور
                txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                    AggregateFunction.Sum).ToString());
              
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                    Convert.ToDouble(txt_Reductions.Value.ToString());


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
                        New["Column05"] = item["Column02"].ToString();
                        this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
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

        private void mnu_DefaultDescription_Click(object sender, EventArgs e)
        {
            Frm_025_SaleDefaultDescription frm = new Frm_025_SaleDefaultDescription();
            frm.ShowDialog();
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

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_010_SaleFactorBindingSource.EndEdit();
                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null || 
                    dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges()!=null)
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
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
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
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
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
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
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
                }

            }
            catch
            {
            }
        }

        private void bt_DefineClubMember_Click(object sender, EventArgs e)
        {
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 144))
            {
                PACNT._1_BasicMenu.Form25_CustomerClub frm = new PACNT._1_BasicMenu.Form25_CustomerClub();
                frm.ShowDialog();

                gridEX1.DropDowns["CustomerClub"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column03+' '+Column02 as Column03,Column04,Column13 from Table_215_CustomerClub");

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_DefineSmsText_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 150))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_025_SMSSetting")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_025_SMSSetting frm = new _02_BasicInfo.Frm_025_SMSSetting();
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

        private void gridEX_List_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
                int GoodId = int.Parse(gridEX_List.GetValue("Column02").ToString());
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    if (int.Parse(item.Cells["Column02"].Value.ToString()) == GoodId)
                    {
                        item.BeginEdit();
                        item.Cells["Column06"].Value = Convert.ToDouble(item.Cells["Column06"].Value.ToString()) + Convert.ToDouble(gridEX_List.GetValue("Column06").ToString());
                        //محاسبه تعداد کل
                        item.Cells["column07"].Value = Convert.ToDouble(item.Cells["column06"].Value.ToString());



                        double TotalPrice = (gridEX1.GetValue("Column12").ToString() == "True" ?
                            (Convert.ToDouble(item.Cells["column04"].Value.ToString()) *
                            Convert.ToDouble(item.Cells["column08"].Value.ToString())) +
                              (Convert.ToDouble(item.Cells["column05"].Value.ToString()) *
                            Convert.ToDouble(item.Cells["column09"].Value.ToString())) +
                            (Convert.ToDouble(item.Cells["column06"].Value.ToString()) *
                            Convert.ToDouble(item.Cells["column10"].Value.ToString()))
                            : Convert.ToInt64((Convert.ToDouble(item.Cells["column04"].Value.ToString()) *
                            Convert.ToDouble(item.Cells["column08"].Value.ToString())) +
                              (Convert.ToDouble(item.Cells["column05"].Value.ToString()) *
                            Convert.ToDouble(item.Cells["column09"].Value.ToString())) +
                            (Convert.ToDouble(item.Cells["column06"].Value.ToString()) *
                            Convert.ToDouble(item.Cells["column10"].Value.ToString()))));

                        TotalPrice = TotalPrice * Convert.ToDouble(item.Cells["Column33"].Value.ToString()) / 100;
                       item.Cells["column11"].Value= TotalPrice;




                        //در غیر این صورت بر اساس اعداد صحیح
                        Int64 jam, takhfif, ezafe;
                        jam = Convert.ToInt64(Convert.ToDouble(item.Cells["column11"].Value.ToString()));
                        takhfif = Convert.ToInt64(Convert.ToDouble(item.Cells["column11"].Value.ToString()) * Convert.ToDouble(item.Cells["column16"].Value.ToString()) / 100);
                        ezafe = Convert.ToInt64(
                            //جمع
                            (Convert.ToDouble(item.Cells["column11"].Value.ToString()) -
                            //تخفیف
                            (Convert.ToDouble(item.Cells["column11"].Value.ToString()) * Convert.ToDouble(item.Cells["column16"].Value.ToString()) / 100)) *
                            //درصد
                          Convert.ToDouble(item.Cells["column18"].Value.ToString())
                          / 100);

                       item.Cells["column17"].Value= takhfif;
                       item.Cells["column19"].Value= ezafe;
                       item.Cells["column20"].Value= (jam - takhfif) + ezafe;
                       item.EndEdit();
                       e.Cancel = true;
                       gridEX_List.CancelCurrentEdit();
                        
                    }
                }
                //محاسبه قیمتهای انتهای فاکتور
                txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                    AggregateFunction.Sum).ToString());

                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) +
                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                    Convert.ToDouble(txt_Reductions.Value.ToString());

                gridEX_List.Col = 0;
            }
            catch 
            {
            }
        }

     
    }
}
