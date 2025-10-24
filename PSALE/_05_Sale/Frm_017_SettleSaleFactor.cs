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
    public partial class Frm_017_SettleSaleFactor : Form
    {
        int _SaleFactor;
        SqlParameter DocNum;
        SqlParameter DocID;
        bool _Del = false, _BackSpace = false, _HasRecord = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Settle clSettle = new Classes.Class_Settle();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);

        public Frm_017_SettleSaleFactor(int SaleFactor, bool Del)
        {
            InitializeComponent();
            _Del = Del;
            _SaleFactor = SaleFactor;
        }

        private void Frm_017_SettleSaleFactor_Load(object sender, EventArgs e)
        {
            foreach (GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
            }

            faDatePicker1.SelectedDateTime = DateTime.Now;

            if (!Class_BasicOperation._FinType)
                groupBox2.Enabled = false;
            msk_ReturnDate.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00");

            //Set Header Default Value
            gridEX_Cash.RootTable.Columns["Column01"].DefaultValue = _SaleFactor;
            gridEX_Pay.RootTable.Columns["Column01"].DefaultValue = _SaleFactor;
            gridEX_Discount.RootTable.Columns["Column01"].DefaultValue = _SaleFactor;
            gridEX_Return.RootTable.Columns["Column01"].DefaultValue = _SaleFactor;
            gridEX_From.RootTable.Columns["Column01"].DefaultValue = _SaleFactor;
            gridEX_To.RootTable.Columns["Column01"].DefaultValue = _SaleFactor;

            //Fill dropDowns

            //--Gridex Factor
            gridEX1.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString,
            @"select Columnid,Column01 from Table_007_PwhrsDraft " +
            "where columnid in (select column09 from " + ConSale.Database + ".dbo.Table_010_SaleFactor " +
            "where columnid=" + _SaleFactor + ")"), "");

            gridEX1.DropDowns["OrderNum"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, @"Select ColumnId,Column01 from 
            Table_005_OrderHeader where columnid in (select column08 from Table_010_SaleFactor where columnid=" + _SaleFactor + ")"), "");

            gridEX1.DropDowns["Customer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, @"select ColumnId,Column01,Column02 from Table_045_PersonInfo
            where ColumnId IN (select Column03 from " + ConSale.Database + ".dbo.Table_010_SaleFactor " +
            "where ColumnId=" + _SaleFactor + ")"), "");

            DataTable DocTable = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead");
            gridEX1.DropDowns["Doc"].SetDataBinding(DocTable, "");

            gridEX1.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT ColumnId,Column01,Column02 from Table_002_SalesTypes"), "");
            gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)"), "");


            //Gridex_Cash
            DataTable BankTable = clDoc.ReturnTable(ConBank.ConnectionString, "Select ColumnId,Column01,Column02,Column35 from Table_020_BankCashAccInfo");
            gridEX_Cash.DropDowns["Bank"].SetDataBinding(BankTable, "");

            DataTable ProjectTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_035_ProjectInfo");
            gridEX_Cash.DropDowns["Project"].SetDataBinding(ProjectTable, "");

            gridEX_Cash.DropDowns["Doc"].SetDataBinding(DocTable, "");

            DataTable HeadersTable = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ACC_Code,ACC_Name from AllHeaders()");
            gridEX_Cash.DropDowns["Bed"].SetDataBinding(HeadersTable, "");
            gridEX_Cash.DropDowns["Bes"].SetDataBinding(HeadersTable, "");

            //gridEX_Pay
            gridEX_Pay.DropDowns["CashBank"].SetDataBinding(BankTable, "");
            gridEX_Pay.DropDowns["Doc"].SetDataBinding(DocTable, "");
            gridEX_Pay.DropDowns["Project"].SetDataBinding(ProjectTable, "");
            gridEX_Pay.DropDowns["Status"].SetDataBinding(clDoc.ReturnTable(ConBank.ConnectionString, "Select ColumnId,Column02 from Table_060_ChequeStatus where Column01=0"), "");
            gridEX_Pay.DropDowns["Bed"].SetDataBinding(HeadersTable, "");
            gridEX_Pay.DropDowns["Bes"].SetDataBinding(HeadersTable, "");
            gridEX_Pay.DropDowns["Bank"].SetDataBinding(clDoc.ReturnTable(ConBank.ConnectionString, "Select Column00,Column01 from Table_010_BankNames"), "");

            //gridex_Discount
            gridEX_Discount.DropDowns["Doc"].SetDataBinding(DocTable, "");
            gridEX_Discount.DropDowns["Bed"].SetDataBinding(HeadersTable, "");
            gridEX_Discount.DropDowns["Bes"].SetDataBinding(HeadersTable, "");

            //gridex_Return
            gridEX_Return.DropDowns["Doc"].SetDataBinding(DocTable, "");
            mlt_ReturnBed.DataSource = HeadersTable;
            mlt_ReturnBes.DataSource = HeadersTable;
            mlt_ValueBed.DataSource = HeadersTable;
            mlt_ValueBes.DataSource = HeadersTable;
            mlt_Function.DataSource = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from table_005_PwhrsOperation where Column16=0");
            gridEX_Return.DropDowns["Paper"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_018_MarjooiSale"), "");
            gridEX_Return.DropDowns["Project"].SetDataBinding(ProjectTable, "");
            GoodbindingSource.DataSource = FactorGoods();
            DataTable GoodTable = FactorGoods();
            gridEX_Return.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_Return.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
            gridEX_Return.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT * FROM Table_070_CountUnitInfo"), "");
            mlt_ReturnBed.Value = clDoc.Account(12, "Column07").Trim();
            mlt_ReturnBes.Value = clDoc.Account(12, "Column13").Trim();
            mlt_ValueBed.Value = clDoc.Account(14, "Column07").Trim();
            mlt_ValueBes.Value = clDoc.Account(14, "Column13".Trim());

            //gridex_From
            DataTable SaleFactortbl = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_010_SaleFactor");
            gridEX_From.DropDowns["Factor"].SetDataBinding(SaleFactortbl, "");

            //gridex_to
            gridEX_To.DropDowns["Factor"].SetDataBinding(SaleFactortbl, "");

            //gridex_Extra
            gridEX_Extra.DropDowns["Type"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount"), "");

            //Fill Tables
            table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, _SaleFactor);
            table_034_SaleFactor_Child3TableAdapter.Fill(dataSet_Sale.Table_034_SaleFactor_Child3, _SaleFactor);
            this.Text = "تسویه فاکتور فروش شماره  " + gridEX1.GetValue("Column01").ToString();
            RefreshPrices();

        }

        private void gridEX_Cash_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            ((Janus.Windows.GridEX.GridEX)sender).CurrentCellDroppedDown = true;
        }

        private void gridEX_Cash_EditingCell(object sender, EditingCellEventArgs e)
        {
            try
            {
                if (((Janus.Windows.GridEX.GridEX)sender)
                    .GetValue("Column08").ToString() != "0")
                    e.Cancel = true;
            }
            catch
            {
            }
        }

        private void CheckEssentialItems(object sender, EventArgs e, DataRowView SaleRow)
        {
            if (rdb_To.Checked && txt_To.Text.Trim() == "")
                throw new Exception("شماره سند مورد نظر را وارد کنید");
            if (rdb_last.Checked && txt_LastNum.Text.Trim() != "")
            {
                clDoc.IsFinal(int.Parse(txt_LastNum.Text.Trim()));
            }
            else if (rdb_To.Checked && txt_To.Text.Trim() != "")
            {
                clDoc.IsValidNumber(int.Parse(txt_To.Text.Trim()));
                clDoc.IsFinal(int.Parse(txt_To.Text.Trim()));
                txt_To_Leave(sender, e);
            }
            //********چک اطلاعات سند حسابداری

            if (Convert.ToDouble(gridEX1.GetRow().Cells["Column28"].Value.ToString()) == 0)
                throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

            if (!faDatePicker1.SelectedDateTime.HasValue || txt_Cover.Text.Trim() == "")
                throw new Exception("اطلاعات مورد نیاز جهت صدور سند حسابداری را تکمیل کنید");

            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));

            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));

            //Person--Center--Project//
            int? Person = null;
            Int16? Center = null;
            Int16? Project = null;




            //*********نقد
            gridEX_Cash.MoveToNewRecord();
            if (gridEX_Cash.RowCount > 0)
            {
                gridEX_Cash.Focus();
                gridEX_Cash.MoveFirst();
                foreach (GridEXRow item in gridEX_Cash.GetRows())
                {
                    if (item.Cells["Column08"].Text == "0")
                    {
                        if (item.Cells["Column04"].Text == "0" || item.Cells["Column05"].Text.Trim() == "" || item.Cells["Bed"].Text.Trim() == "" || item.Cells["Bes"].Text.Trim() == "")
                            throw new Exception("اطلاعات مورد نیاز جهت صدور برگه دریافت نقد را کامل کنید");
                    }
                }

                TPerson.Rows.Clear();
                TAccounts.Rows.Clear();
                //چک سند حسابداری
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Cash.GetRows())
                {
                    if (item.Cells["Column08"].Text == "0")
                    {
                        Person = null;
                        Center = null;
                        Project = null;
                        Person = int.Parse(SaleRow["Column03"].ToString());

                        if (item.Cells["Column30"].Text.Trim() != "")
                            Project = Int16.Parse(item.Cells["Column30"].Value.ToString());

                        clCredit.All_Controls_Row(item.Cells["Bes"].Value.ToString(), Person, Center, Project, item);
                        Person = null;
                        if (item.Cells["Column32"].Text.Trim() != "")
                            Person = Int16.Parse(item.Cells["Column32"].Value.ToString());
                        clCredit.All_Controls_Row(item.Cells["Bed"].Value.ToString(), Person, null, null, item);

                        //**********Check Person Credit************//
                        TPerson.Rows.Add(Int32.Parse(SaleRow["Column03"].ToString()), item.Cells["Bes"].Value.ToString(),
                            Convert.ToDouble(item.Cells["Column04"].Value.ToString()) * -1);
                        Person = null;
                        if (item.Cells["Column32"].Text.Trim() != "")
                            Person = Int32.Parse(item.Cells["Column32"].Value.ToString());
                        if (Person != null)
                            TPerson.Rows.Add(Person, item.Cells["Bed"].Value.ToString(),
                                Convert.ToDouble(item.Cells["Column04"].Value.ToString()));


                        //**********Check Account's nature****//
                        TAccounts.Rows.Add(item.Cells["Bed"].Value.ToString(), Convert.ToDouble(item.Cells["Column04"].Value.ToString()));
                        TAccounts.Rows.Add(item.Cells["Bes"].Value.ToString(), Convert.ToDouble(item.Cells["Column04"].Value.ToString()) * -1);
                    }
                }

                if (TAccounts.Rows.Count > 0)
                    clCredit.CheckAccountCredit(TAccounts, 0);
                if (TPerson.Rows.Count > 0)
                    clCredit.CheckPersonCredit(TPerson, 0);

            }

            //*********چک
            gridEX_Pay.MoveToNewRecord();
            if (gridEX_Pay.RowCount > 0)
            {

                foreach (GridEXRow item in gridEX_Pay.GetRows())
                {
                    if (item.Cells["Column08"].Text == "0")
                    {
                        if (item.Cells["Bed"].Text.Trim() == "" || item.Cells["Bes"].Text.Trim() == "" ||
                            item.Cells["Column02"].Text.Trim() == "" || item.Cells["Column04"].Text.Trim() == "0" ||
                              item.Cells["Column05"].Text.Trim() == "" || item.Cells["Column09"].Text.Trim() == "" ||
                              item.Cells["Column10"].Text.Trim() == "" || item.Cells["Column11"].Text.Trim() == "" ||
                             item.Cells["Column12"].Text.Trim() == "" || item.Cells["Column14"].Text.Trim() == "")
                            throw new Exception("اطلاعات مورد نیاز جهت صدور برگه دریافت چک را کامل کنید");
                    }
                }

                TPerson.Rows.Clear();
                TAccounts.Rows.Clear();
                //چک سند حسابداری
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Pay.GetRows())
                {
                    if (item.Cells["Column08"].Text.Trim() == "0")
                    {
                        Person = null;
                        Center = null;
                        Project = null;
                        Person = int.Parse(SaleRow["Column03"].ToString());

                        if (item.Cells["Column30"].Text.Trim() != "")
                            Project = Int16.Parse(item.Cells["Column30"].Value.ToString());

                        clCredit.All_Controls_Row(item.Cells["Bes"].Value.ToString(), Person, Center, Project, item);
                        Person = null;
                        if (item.Cells["Column32"].Text.Trim() != "")
                            Person = Int16.Parse(item.Cells["Column32"].Value.ToString());
                        clCredit.All_Controls_Row(item.Cells["Bed"].Value.ToString(), Person, null, null, item);

                        //**********Check Person Credit************//
                        TPerson.Rows.Add(Int32.Parse(SaleRow["Column03"].ToString()), item.Cells["Bes"].Value.ToString(),
                           Convert.ToDouble(item.Cells["Column04"].Value.ToString()) * -1);
                        Person = null;
                        if (item.Cells["Column32"].Text.Trim() != "")
                            Person = Int32.Parse(item.Cells["Column32"].Value.ToString());
                        if (Person != null)
                            TPerson.Rows.Add(Person, item.Cells["Bed"].Value.ToString(),
                                Convert.ToDouble(item.Cells["Column04"].Value.ToString()));


                        //**********Check Account's nature****//
                        TAccounts.Rows.Add(item.Cells["Bed"].Value.ToString(), Convert.ToDouble(item.Cells["Column04"].Value.ToString()));
                        TAccounts.Rows.Add(item.Cells["Bes"].Value.ToString(), Convert.ToDouble(item.Cells["Column04"].Value.ToString()) * -1);
                    }
                }

                if (TAccounts.Rows.Count > 0)
                    clCredit.CheckAccountCredit(TAccounts, 0);
                if (TPerson.Rows.Count > 0)
                    clCredit.CheckPersonCredit(TPerson, 0);

            }

            //***********************تخفیفات
            gridEX_Discount.MoveToNewRecord();
            if (gridEX_Discount.RowCount > 0)
            {

                foreach (GridEXRow item in gridEX_Discount.GetRows())
                {
                    if (item.Cells["Column08"].Text == "0")
                    {
                        if (item.Cells["Bed"].Text.Trim() == "" || item.Cells["Bes"].Text.Trim() == "" ||
                            item.Cells["Column02"].Text.Trim() == "" || item.Cells["Column04"].Text.Trim() == "0" ||
                              item.Cells["Column06"].Text.Trim() == "")
                            throw new Exception("اطلاعات مورد نیاز جهت ثبت تخفیفات را کامل کنید");
                    }
                }

                TPerson.Rows.Clear();
                TAccounts.Rows.Clear();
                //چک سند حسابداری
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Discount.GetRows())
                {
                    if (item.Cells["Column08"].Text.Trim() == "0")
                    {
                        Person = null;
                        Center = null;
                        Project = null;
                        Person = int.Parse(SaleRow["Column03"].ToString());
                        clCredit.All_Controls_Row(item.Cells["Bes"].Value.ToString(), Person, Center, Project, item);

                        //**********Check Person Credit************//
                        TPerson.Rows.Add(Int32.Parse(SaleRow["Column03"].ToString()), item.Cells["Bes"].Value.ToString(), Convert.ToInt64(Convert.ToDouble(item.Cells["Column04"].Value.ToString())) * -1);

                        //**********Check Account's nature****//
                        TAccounts.Rows.Add(item.Cells["Bed"].Value.ToString(), Convert.ToDouble(item.Cells["Column04"].Value.ToString()));
                        TAccounts.Rows.Add(item.Cells["Bes"].Value.ToString(), Convert.ToDouble(item.Cells["Column04"].Value.ToString()) * -1);
                    }
                }

                if (TAccounts.Rows.Count > 0)
                    clCredit.CheckAccountCredit(TAccounts, 0);
                if (TPerson.Rows.Count > 0)
                    clCredit.CheckPersonCredit(TPerson, 0);

            }

            //*********فاکتور مرجوعی
            gridEX_Return.MoveToNewRecord();
            if (gridEX_Return.RowCount > 0)
            {
                GridEXFilterCondition filter = new GridEXFilterCondition(gridEX_Return.RootTable.Columns["Column08"], ConditionOperator.Equal, 0);
                if (int.Parse(gridEX_Return.GetTotal(gridEX_Return.RootTable.Columns["Column02"], AggregateFunction.Count, filter).ToString()) > 0)
                {
                    if (msk_ReturnDate.Text.Trim() == "" || mlt_ReturnBed.Text.Trim() == "" || mlt_ReturnBes.Text.Trim() == "" || mlt_Function.Text.Trim() == "")
                        throw new Exception("اطلاعات مورد نیاز جهت صدور فاکتور مرجوعی را کامل کنید");
                    if (groupBox2.Enabled && (mlt_ValueBed.Text.Trim() == "" || mlt_ValueBes.Text.Trim() == ""))
                        throw new Exception("اطلاعات مورد نیاز جهت صدور فاکتور مرجوعی را کامل کنید");
                }

                _HasRecord = false;
                List<int> Count = new List<int>();
                foreach (GridEXRow item in gridEX_Return.GetRows())
                {
                    if (item.Cells["Column08"].Text == "0")
                    {
                        Count.Add(int.Parse(item.Cells["Column01"].Value.ToString()));
                        if (item.Cells["Column15"].Text.Trim() == "" || item.Cells["Column16"].Text.Trim() == "")
                            throw new Exception("اطلاعات مورد نیاز جهت صدور فاکتور مرجوعی را کامل کنید");

                    }
                }
                if (Count.Count > 0)
                    _HasRecord = true;
            }

            //انتقال به فاکتور
            gridEX_To.MoveToNewRecord();
            if (gridEX_To.RowCount > 0)
            {
                foreach (GridEXRow item in gridEX_To.GetRows())
                {
                    if (item.Cells["Column31"].Value.ToString() == "False")
                    {
                        if (item.Cells["Column02"].Text.Trim() == "" || item.Cells["Column04"].Text.Trim() == "0" || item.Cells["Column07"].Text.Trim() == "0")
                            throw new Exception("اطلاعات مورد نیاز جهت انتقال مبلغ به فاکتور دیگر را تکمیل کنید");
                        ValidFactor(item.Cells["Column07"].Value.ToString(), item.Cells["Column07"].Text.Trim());
                    }
                }
            }

        }

        private void ReturnDocId()
        {

            string headercomman = string.Empty;


            DocNum = new SqlParameter("DocNum", SqlDbType.Int);
            DocNum.Direction = ParameterDirection.Output;
            DocID = new SqlParameter("DocID", SqlDbType.Int);
            DocID.Direction = ParameterDirection.Output;
            if (rdb_last.Checked)
            {
                //DocNum = clDoc.LastDocNum();
                //DocID = clDoc.DocID(DocNum);
                headercomman = " set @DocNum=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0)))";

            }
            else if (rdb_To.Checked)
            {
                //DocNum = int.Parse(txt_To.Text.Trim());
                //DocID = clDoc.DocID(DocNum);
                headercomman = " set @DocNum=" + int.Parse(txt_To.Text.Trim()) + "    SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + int.Parse(txt_To.Text.Trim()) + ")";

            }
            else if (rdb_New.Checked)
            {
                //DocNum = clDoc.LastDocNum() + 1;
                //DocID = clDoc.ExportDoc_Header( DocNum, faDatePicker1.Text, txt_Cover.Text, Class_BasicOperation._UserName);
                headercomman = @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + faDatePicker1.Text + "',2,0,'" + txt_Cover.Text + "','" + Class_BasicOperation._UserName +
             "',getdate()); SET @DocID=SCOPE_IDENTITY()";
            }


            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();

                SqlTransaction sqlTran = Con.BeginTransaction();
                SqlCommand Command = Con.CreateCommand();
                Command.Transaction = sqlTran;

                try
                {
                    Command.CommandText = headercomman;
                    Command.Parameters.Add(DocNum);
                    Command.Parameters.Add(DocID);
                    Command.ExecuteNonQuery();
                    sqlTran.Commit();


                }
                catch (Exception es)
                {
                    sqlTran.Rollback();
                    MessageBox.Show(es.Message);
                    return;
                }

                this.Cursor = Cursors.Default;



            }



            //if (rdb_New.Checked)
            //{
            //    int Count = 0;
            //    table_034_SaleFactor_Child3BindingSource.Filter = "(Column03=1 OR Column03=2 OR Column03=3 OR Column04=4) and (Column08=0)";
            //    Count = table_010_SaleFactorBindingSource.Count;
            //    table_034_SaleFactor_Child3BindingSource.RemoveFilter();
            //    if (Count > 0)
            //    {
            //        DocNum = clDoc.LastDocNum() + 1;
            //        DocID = clDoc.ExportDoc_Header(DocNum, faDatePicker1.Text, txt_Cover.Text, Class_BasicOperation._UserName);
            //    }
            //}
            //else if (rdb_last.Checked)
            //{
            //    DocNum = clDoc.LastDocNum();
            //    DocID = clDoc.DocID(DocNum);
            //}
            //else if (rdb_To.Checked)
            //{
            //    DocNum = int.Parse(txt_To.Text.Trim());
            //    DocID = clDoc.DocID(DocNum);

            //}

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (this.table_034_SaleFactor_Child3BindingSource.Count > 0)
            {
                try
                {

                    gridEX_Cash.Refetch();
                    gridEX_Discount.Refetch();
                    gridEX_Pay.Refetch();
                    gridEX_Return.Refetch();
                    gridEX_To.Refetch();
                    gridEX_From.Refetch();
                    gridEX_Return.UpdateData();
                    DataRowView SaleRow = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                    if (gridEX_Return.GetDataRows().Count() > 0 &&
                        ((SaleRow["Column09"] == null || string.IsNullOrWhiteSpace(SaleRow["Column09"].ToString())
                        || Convert.ToInt32(SaleRow["Column09"]) == 0)))
                    {
                        throw new Exception("فاکتور حواله ندارد امکان ثبت مرجوعی فروش وجود ندارد");

                    }

                    Int64 FactorPrice = Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column28").ToString())) -
                        (Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column29").ToString())) +
                        Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column30").ToString())) +
                        Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column31").ToString())) +
                        Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column33").ToString()))) +
                        Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column32").ToString()));


                    Int64 TotalSettle = (Convert.ToInt64(Convert.ToDouble(gridEX_Cash.GetTotalRow().Cells["Column04"].Value.ToString())) +
                        Convert.ToInt64(Convert.ToDouble(gridEX_Pay.GetTotalRow().Cells["Column04"].Value.ToString())) +
                        Convert.ToInt64(Convert.ToDouble(gridEX_Discount.GetTotalRow().Cells["Column04"].Value.ToString())) +
                        Convert.ToInt64(Convert.ToDouble(gridEX_Return.GetTotalRow().Cells["Column29"].Value.ToString())) +
                        Convert.ToInt64(Convert.ToDouble(gridEX_From.GetTotalRow().Cells["Column04"].Value.ToString())));
                    Int64 To = Convert.ToInt64(Convert.ToDouble(gridEX_To.GetTotalRow().Cells["Column04"].Value.ToString()));
                    if ((TotalSettle - FactorPrice) > 0)
                    {
                        if (To > (TotalSettle - FactorPrice))
                            throw new Exception("جمع مبلغ انتقالی بیشتر از مانده فاکتور می باشد");
                        //if (To == 0)
                        //    throw new Exception("جمع مبالغ تسویه بیشتر از مبلغ کل فاکتور می باشد" + Environment.NewLine +
                        //        "انتقال مازاد مبلغ به فاکتور دیگر، الزامیست");
                        //if(To< (TotalSettle-FactorPrice))
                        //    throw new Exception("مبلغ پرداخت شده، بیشتر از مبلغ کل فاکتور است"+Environment.NewLine+"جهت ادامه انتقال مانده 
                    }
                    else if ((FactorPrice - TotalSettle) > 0)
                    {
                        if (To > 0)
                            throw new Exception("به علت باز بودن این فاکتور، انتقال به فاکتور دیگر میسر نمی باشد");
                    }

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به صدور برگه و ذخیره اطلاعات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {

                        CheckEssentialItems(sender, e, SaleRow);
                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                        string masool = string.Empty;
                        if (gridEX1.CurrentRow.Cells["column05"].Value != null && !string.IsNullOrWhiteSpace(gridEX1.CurrentRow.Cells["column05"].Text))
                            masool = " :مسئول فروش " + gridEX1.CurrentRow.Cells["column05"].Text;


                        //***********************************TRANSFER TO FACTOR
                        if (gridEX_To.RowCount > 0)
                        {
                            foreach (GridEXRow item in gridEX_To.GetRows())
                            {
                                if (item.Cells["Column31"].Value.ToString() == "False")
                                {
                                    clDoc.RunSqlCommand(ConSale.ConnectionString, "INSERT INTO Table_034_SaleFactor_Child3 VALUES(" + item.Cells["Column07"].Value.ToString()
                                        + ",'" + item.Cells["Column02"].Value.ToString() + "',5," + item.Cells["Column04"].Value.ToString() + ",NULL," +
                                        (item.Cells["Column06"].Text.Trim() == "" ? "NULL" : "'" + item.Cells["Column06"].Text.Trim() + "'") + "," + _SaleFactor
                                        + ",0,null,null,null,null,null,null,null,null,0,0,0,0,0,0,0,0,0,0,0,0,0,NULL,1,NULL,NULL,NULL,0,0)");
                                    item.BeginEdit();
                                    item.Cells["Column31"].Value = true;
                                    item.EndEdit();
                                }
                            }
                            this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                            this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                        }

                        //***********************************END OF TRANSFER TO FACTOR

                        //*******************************************CASH
                        if (gridEX_Cash.RowCount > 0)
                        {
                            using (SqlConnection conbank = new SqlConnection(Properties.Settings.Default.BANK))
                            {
                                conbank.Open();
                                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Cash.GetRows())
                                {
                                    if (item.Cells["Column08"].Value.ToString() == "0")
                                    {

                                        ReturnDocId();
                                        if (Convert.ToInt32(DocID.Value) > 0)
                                        {
                                            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                                            Key.Direction = ParameterDirection.Output;
                                            SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_045_ReceiveCash ([Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                            ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                            ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                            ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]      ,[Column23]
                                            ,[Column24]) VALUES (" + item.Cells["Column05"].Value.ToString()
                                        + ",'" + item.Cells["Column02"].Value.ToString() + "'," + item.Cells["Column04"].Value.ToString() + ",'پرداخت نقد بابت فاکتور فروش شماره " +
                                        SaleRow["Column01"].ToString() + " '," + SaleRow["Column03"].ToString() + "," + (item.Cells["Column30"].Text == "" ? "NULL" : item.Cells["Column30"].Value.ToString())
                                        + ",NULL," + DocID.Value + ",0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                                        (item.Cells["Column32"].Text.Trim() == "" ? "NULL" : item.Cells["Column32"].Value.ToString()) + ",0,null,0); SET @Key=SCOPE_IDENTITY()", conbank);
                                            Insert.Parameters.Add(Key);
                                            Insert.ExecuteNonQuery();
                                            int PaperId = int.Parse(Key.Value.ToString());

                                            this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                                            this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);

                                            string[] _BedInfo = clDoc.ACC_Info(item.Cells["Bed"].Value.ToString());
                                            clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), item.Cells["Bed"].Value.ToString(), Int16.Parse(_BedInfo[0]),
                                                _BedInfo[1], _BedInfo[2], _BedInfo[3], _BedInfo[4], (item.Cells["Column32"].Text.Trim() == "" ? "NULL" : item.Cells["Column32"].Value.ToString()), null, null,
                                                "واریز نقد به " + item.Cells["Column05"].Text + " توسط " + gridEX1.GetRow().Cells["Column03"].Text + " بابت فاکتور فروش ش " + SaleRow["Column01"].ToString() + masool
                                                , Convert.ToInt64(Convert.ToDouble(item.Cells["Column04"].Value.ToString())), 0, 0, 0, -1, 24, PaperId, Class_BasicOperation._UserName, 0, 24);

                                            string[] _BesInfo = clDoc.ACC_Info(item.Cells["Bes"].Value.ToString());
                                            clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), item.Cells["Bes"].Value.ToString(), Int16.Parse(_BesInfo[0]),
                                               _BesInfo[1], _BesInfo[2], _BesInfo[3], _BesInfo[4], SaleRow["Column03"].ToString(), null,
                                               (item.Cells["Column30"].Text.Trim() == "" ? "" : item.Cells["Column30"].Value.ToString()),
                                               "واریز نقد به " + item.Cells["Column05"].Text + " توسط " + gridEX1.GetRow().Cells["Column03"].Text + " بابت فاکتور فروش ش " + SaleRow["Column01"].ToString() + masool, 0,
                                               Convert.ToInt64(Convert.ToDouble(item.Cells["Column04"].Value.ToString())), 0, 0, -1, 24, PaperId, Class_BasicOperation._UserName, 0, 24);

                                            //قراردادن شماره برگه و شماره سند در سطر مربوط به پرداخت
                                            item.BeginEdit();
                                            item.Cells["Column07"].Value = PaperId;
                                            item.Cells["Column08"].Value = DocID.Value;
                                            item.EndEdit();
                                            //قرار دادن شماره سند در برگه پرداخت نقد
                                            clDoc.RunSqlCommand(ConBank.ConnectionString, "UPDATE Table_045_ReceiveCash SET Column08=" + DocID.Value + " where ColumnId=" + PaperId);
                                        }
                                    }
                                }
                            }
                            this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                            this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);

                        }
                        //*******************************************END OF CASH
                        //*******************************************Cheques
                        if (gridEX_Pay.RowCount > 0)
                        {
                            using (SqlConnection conbank = new SqlConnection(Properties.Settings.Default.BANK))
                            {
                                conbank.Open();
                                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Pay.GetRows())
                                {
                                    if (item.Cells["Column08"].Value.ToString() == "0")
                                    {
                                        ReturnDocId();
                                        if (Convert.ToInt32(DocID.Value) > 0)
                                        {
                                            int BackNumber = clDoc.MaxNumber(conbank.ConnectionString, "Table_035_ReceiptCheques", "Column00");
                                            //درج برگه
                                            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                                            Key.Direction = ParameterDirection.Output;
                                            SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_035_ReceiptCheques ([Column00]
                                                                                                   ,[Column01]
                                                                                                   ,[Column02]
                                                                                                   ,[Column03]
                                                                                                   ,[Column04]
                                                                                                   ,[Column05]
                                                                                                   ,[Column06]
                                                                                                   ,[Column07]
                                                                                                   ,[Column08]
                                                                                                   ,[Column09]
                                                                                                   ,[Column10]
                                                                                                   ,[Column11]
                                                                                                   ,[Column12]
                                                                                                   ,[Column15]
                                                                                                   ,[Column41]
                                                                                                   ,[Column42]
                                                                                                   ,[Column43]
                                                                                                   ,[Column44]
                                                                                                   ,[Column45]
                                                                                                   ,[Column46]
                                                                                                   ,[Column48]
                                                                                                   ,[Column49]
                                                                                                   ,[Column50]
                                                                                                   ,[Column51]) VALUES (" + BackNumber
                                            + "," + item.Cells["Column05"].Value.ToString() + ",'" + item.Cells["Column02"].Value.ToString() + "','" + item.Cells["Column09"].Value.ToString() + "','"
                                            + item.Cells["Column10"].Value.ToString() + "'," + item.Cells["Column04"].Value.ToString() + ",' بابت فاکتور فروش ش " + gridEX1.GetValue("Column01").ToString() + "'," + gridEX1.GetValue("Column03").ToString() + "," +
                                            item.Cells["Column11"].Value.ToString() + "," + (item.Cells["Column12"].Text.Trim() != "" ? "'" + item.Cells["Column12"].Value.ToString() + "'" :
                                            "NULL") + "," + (item.Cells["Column13"].Text.Trim() != "" ? "'" + item.Cells["Column13"].Value.ToString() + "'" : "NULL") + ",null,null," +
                                            (item.Cells["Column30"].Text.Trim() != "" ? item.Cells["Column30"].Value.ToString() : "NULL") + ",null,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                            Class_BasicOperation._UserName + "',getdate()," + (item.Cells["Column32"].Text.Trim() != "" ? item.Cells["Column32"].Value.ToString() : "NULL") + "," + item.Cells["Column14"].Value.ToString() +
                                            ",0,null,0); SET @Key=SCOPE_IDENTITY()", conbank);

                                            Insert.Parameters.Add(Key);
                                            Insert.ExecuteNonQuery();
                                            int PaperId = int.Parse(Key.Value.ToString());
                                            //در تاریخ 980327 نوع 28 به وضعیت چک تغییر کرد

                                            //درج گردش
                                            string[] _BedInfo = clDoc.ACC_Info(item.Cells["Bed"].Value.ToString());

                                            this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                                            this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);

                                            int _TurnID = clDoc.AddTurnReception(PaperId, short.Parse(item.Cells["Column14"].Value.ToString()), item.Cells["Column02"].Value.ToString(),
                                            (item.Cells["Column32"].Text.Trim() == "" ? "NULL" : item.Cells["Column32"].Value.ToString()), item.Cells["Column05"].Value.ToString(),
                                            item.Cells["Bed"].Value.ToString(), Int16.Parse(_BedInfo[0].ToString()),
                                            (_BedInfo[1].ToString() == "" ? "NULL" : _BedInfo[1].ToString()), (_BedInfo[2].ToString() == string.Empty ? "NULL" : _BedInfo[2].ToString())
                                            , (_BedInfo[3].ToString() == "" ? "NULL" : _BedInfo[3].ToString()), (_BedInfo[4].ToString() == string.Empty ? "NULL" : _BedInfo[4].ToString()), Convert.ToInt32(DocID.Value), "NULL",
                                            "NULL", Class_BasicOperation._UserName, "NULL", "False", "", 0.0);

                                            clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), item.Cells["Bed"].Value.ToString(), Int16.Parse(_BedInfo[0]),
                                             _BedInfo[1], _BedInfo[2], _BedInfo[3], _BedInfo[4], (item.Cells["Column32"].Text.Trim() == "" ? "NULL" : item.Cells["Column32"].Value.ToString()), null, null,
                                             item.Cells["Column14"].Text + "-" + " ش " + item.Cells["Column09"].Value.ToString() + " تاریخ " + item.Cells["Column10"].Value.ToString() +
                                             " به " + item.Cells["Column05"].Text + " از " + gridEX1.GetRow().Cells["Column03"].Text + " بابت فاکتور فروش ش " + gridEX1.GetValue("Column01").ToString() + masool
                                             , Convert.ToInt64(Convert.ToDouble(item.Cells["Column04"].Value.ToString())), 0, 0, 0, -1, Convert.ToInt16(item.Cells["Column14"].Value), _TurnID, Class_BasicOperation._UserName, 0, 28);

                                            string[] _BesInfo = clDoc.ACC_Info(item.Cells["Bes"].Value.ToString());
                                            clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), item.Cells["Bes"].Value.ToString(), Int16.Parse(_BesInfo[0]),
                                               _BesInfo[1], _BesInfo[2], _BesInfo[3], _BesInfo[4], SaleRow["Column03"].ToString(), null,
                                               (item.Cells["Column30"].Text.Trim() == "" ? "" : item.Cells["Column30"].Value.ToString()),
                                               item.Cells["Column14"].Text + "-" + " ش " + item.Cells["Column09"].Value.ToString() + " تاریخ " + item.Cells["Column10"].Value.ToString() +
                                                " به " + item.Cells["Column05"].Text + " از " + gridEX1.GetRow().Cells["Column03"].Text + " بابت فاکتور فروش ش " + gridEX1.GetValue("Column01").ToString() + masool, 0,
                                               Convert.ToInt64(Convert.ToDouble(item.Cells["Column04"].Value.ToString())), 0, 0, -1, Convert.ToInt16(item.Cells["Column14"].Value), _TurnID, Class_BasicOperation._UserName, 0, 28);

                                            //قراردادن شماره برگه و شماره سند در سطر مربوط به پرداخت
                                            item.BeginEdit();
                                            item.Cells["Column07"].Value = PaperId;
                                            item.Cells["Column08"].Value = DocID.Value;
                                            item.EndEdit();
                                        }
                                    }
                                }

                            }
                            this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                            this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                        }
                        //*******************************************END OF CHEQUE
                        //*******************************************DISCOUNT
                        if (gridEX_Discount.RowCount > 0)
                        {
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Discount.GetRows())
                            {
                                if (item.Cells["Column08"].Value.ToString() == "0")
                                {
                                    ReturnDocId();
                                    if (Convert.ToInt32(DocID.Value) > 0)
                                    {
                                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                                        this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);

                                        string[] _BedInfo = clDoc.ACC_Info(item.Cells["Bed"].Value.ToString());
                                        clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), item.Cells["Bed"].Value.ToString(), Int16.Parse(_BedInfo[0]),
                                            _BedInfo[1], _BedInfo[2], _BedInfo[3], _BedInfo[4], null, null, null,
                                             item.Cells["Column06"].Text + " - " + " بابت فاکتور فروش ش " + SaleRow["Column01"].ToString() + masool
                                            , Convert.ToInt64(Convert.ToDouble(item.Cells["Column04"].Value.ToString())), 0, 0, 0, -1, 30, int.Parse(item.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0, (short?)null);

                                        string[] _BesInfo = clDoc.ACC_Info(item.Cells["Bes"].Value.ToString());
                                        clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), item.Cells["Bes"].Value.ToString(), Int16.Parse(_BesInfo[0]),
                                           _BesInfo[1], _BesInfo[2], _BesInfo[3], _BesInfo[4], SaleRow["Column03"].ToString(), null, null,
                                           item.Cells["Column06"].Text + " - " + " بابت فاکتور فروش ش " + SaleRow["Column01"].ToString() + masool, 0,
                                           Convert.ToInt64(Convert.ToDouble(item.Cells["Column04"].Value.ToString())), 0, 0, -1, 30, int.Parse(item.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0, (short?)null);

                                        //قراردادن شماره برگه و شماره سند در سطر مربوط به پرداخت
                                        item.BeginEdit();
                                        item.Cells["Column08"].Value = DocID.Value;
                                        item.EndEdit();
                                    }
                                }
                            }

                            this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                            this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                        }
                        //******************************************END OF DISCOUNT
                        //******************************************RETURN FACTOR
                        if (gridEX_Return.RowCount > 0)
                        {
                            if (_HasRecord)
                            {
                                ReturnDocId();
                                if (Convert.ToInt32(DocID.Value) > 0)
                                {
                                    int ReturnNum = clDoc.MaxNumber(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column01");
                                    int ReturnId = 0;
                                    SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                                    Key.Direction = ParameterDirection.Output;
                                    using (SqlConnection consale = new SqlConnection(Properties.Settings.Default.SALE))
                                    {
                                        consale.Open();
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
                                                                               ) VALUES(" + ReturnNum + ",'" + msk_ReturnDate.Text + "'," + SaleRow["Column03"].ToString() + "," +
                                                                            (SaleRow["Column04"].ToString().Trim() == "" ? "NULL" : "'" + SaleRow["Column04"].ToString().Trim() + "'") + "," +
                                                                            (SaleRow["Column05"].ToString().Trim() == "" ? "NULL" : SaleRow["Column05"].ToString().Trim()) + ",'" + "ارجاع فاکتور فروش ش " + SaleRow["Column01"].ToString() + " تاریخ " + SaleRow["Column02"].ToString() +
                                                                            "',0,0,0,0,0,0,'" + Class_BasicOperation._UserName
                                                                            + "',Getdate(),'" + Class_BasicOperation._UserName + "',Getdate()," + SaleRow["ColumnId"].ToString() + ",0,0,0,0,0,null,0); SET @Key=SCOPE_IDENTITY()", consale);
                                        InsertHeader.Parameters.Add(Key);
                                        InsertHeader.ExecuteNonQuery();
                                        ReturnId = int.Parse(Key.Value.ToString());
                                        //درج دیتیل1
                                        foreach (GridEXRow item in gridEX_Return.GetRows())
                                        {
                                            if (item.Cells["Column08"].Value.ToString() == "0")
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
           ,[Column35]) VALUES(" + ReturnId + "," + item.Cells["Column15"].Value.ToString() +
                                                    "," + item.Cells["Column16"].Value.ToString() + "," + item.Cells["Column17"].Value.ToString() + "," + item.Cells["Column18"].Value.ToString() + ","
                                                    + item.Cells["Column19"].Value.ToString() + "," + item.Cells["Column20"].Value.ToString() + "," + item.Cells["Column21"].Value.ToString() + ","
                                                    + item.Cells["Column22"].Value.ToString() + "," + item.Cells["Column23"].Value.ToString() + "," + item.Cells["Column24"].Value.ToString() +
                                                    ",NULL,NULL,NULL,0," + item.Cells["Column25"].Value.ToString() + "," + item.Cells["Column26"].Value.ToString() + ","
                                                    + item.Cells["Column27"].Value.ToString() + "," + item.Cells["Column28"].Value.ToString() + "," + item.Cells["Column29"].Value.ToString() +
                                                    ",NULL," + (item.Cells["Column30"].Value.ToString().Trim() != "" ? item.Cells["Column30"].Value.ToString() : "NULL") + ",NULL,"
                                                    + (SaleRow["Column07"].ToString().Trim() != "" ? SaleRow["Column07"].ToString() : "0") + ",0,0,0,0,0," +
                                                    item.Cells["tedaddarkartoon"].Value.ToString() + "," +
                                                    item.Cells["tedaddarbaste"].Value.ToString() + ",null,null,0,0)", consale);
                                                InsertDetail.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    //درج دیتیل 2
                                    //foreach (DataRowView item in this.table_034_SaleFactor_Child3BindingSource)
                                    //{
                                    //    clDoc.RunSqlCommand(ConSale.ConnectionString, "INSERT INTO Table_020_Child2_MarjooiSale VALUES(" + ReturnId + "," + item["Column02"].ToString()
                                    //        + "," + item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + (item["Column05"].ToString() == "True" ? 1 : 0) + "," +
                                    //        (item["Column06"].ToString().Trim() == "" ? "NULL" : item["Column06"].ToString()) + ")");

                                    //}
                                    //به روز رسانی مقادیر مبالغ در فاکتور مرجوعی
                                    DataTable ChildTable = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_019_Child1_MarjooiSale where Column01=" + ReturnId);
                                    clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_018_MarjooiSale SET Column18=" + ChildTable.Compute("SUM(Column20)", "").ToString() +
                                        " , Column21=" + ChildTable.Compute("SUM(Column19)", "").ToString() + " , Column22=" + ChildTable.Compute("SUM(Column17)", "").ToString() +
                                        " where ColumnId=" + ReturnId);



                                    //درج رسید انبار
                                    if (ReturnId > 0)
                                    {
                                        //DraftTable
                                        DataTable DraftTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_007_PwhrsDraft where ColumnId=" + SaleRow["Column09"].ToString());
                                        DataTable DraftChild = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_008_Child_PwhrsDraft where Column01=" + SaleRow["Column09"].ToString());

                                        int ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
                                        //, int.Parse(DraftTable.Rows[0]["Column03"].ToString()));

                                        //**Resid Header
                                        using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.WHRS))
                                        {
                                            conware.Open();
                                            SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                                            key.Direction = ParameterDirection.Output;
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
                                                                          ) VALUES (" + ResidNum + ",'" + msk_ReturnDate.Text + "'," +
                                             DraftTable.Rows[0]["Column03"].ToString() + "," + mlt_Function.Value.ToString() + "," + DraftTable.Rows[0]["Column05"].ToString() + ",'" + "رسید صادرشده از تسویه فاکتور مرجوعی شماره " +
                                             ReturnNum + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,0," + ReturnId + ",0,0,null,0,1,null); SET @Key=Scope_Identity()", conware);
                                            Insert.Parameters.Add(key);
                                            Insert.ExecuteNonQuery();
                                            int ResidId = int.Parse(key.Value.ToString());

                                            //Resid Detail
                                            foreach (GridEXRow item in gridEX_Return.GetRows())
                                            {
                                                if (item.Cells["Column08"].Text.Trim() == "0")
                                                {
                                                    DraftChild.DefaultView.RowFilter = "Column02=" + item.Cells["Column15"].Value.ToString();
                                                    if (DraftChild.DefaultView.Count > 0)
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
                                                                                       ,[Column35]) VALUES (" + ResidId + "," + item.Cells["Column15"].Value.ToString() + "," +
                                                           item.Cells["Column16"].Value.ToString() + "," + item.Cells["Column17"].Value.ToString() + "," + item.Cells["Column18"].Value.ToString() +
                                                           "," + item.Cells["Column19"].Value.ToString() + "," + item.Cells["Column20"].Value.ToString() + ",0,0," + item.Cells["Column23"].Value.ToString() + "," + item.Cells["Column24"].Value.ToString() + ",NULL,NULL,"
                                                            + (item.Cells["Column30"].Text.ToString() == "" ? "NULL" : item.Cells["Column30"].Value.ToString()) + ",'" + Class_BasicOperation._UserName
                                                            + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0," + DraftChild.Rows[0]["Column15"].ToString() + ","
                                                            + double.Parse(DraftChild.Rows[0]["Column15"].ToString()) * double.Parse(item.Cells["Column20"].Value.ToString()) +
                                                            ",0,NULL,NULL,NULL,0,0,0,0,NULL,NULL," +
                                                            item.Cells["tedaddarkartoon"].Value.ToString() + "," +
                                                            item.Cells["tedaddarbaste"].Value.ToString() + ",0,0)", conware);
                                                        InsertDetail.ExecuteNonQuery();
                                                    }
                                                }
                                            }

                                            //درج شماره رسید در فاکتور مرجوعی
                                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column09", "ColumnId", ReturnId, ResidId);

                                            //درج سند حسابداری مربوط به فاکتور مرجوعی و رسید انبار در صورت دائمی بودن سیستم مالی

                                            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     Project, Total, Discount, Adding, column01, Total - Discount + Adding AS Net
                                        FROM         (SELECT     column22 AS Project, ISNULL(SUM(column11), 0) AS Total, ISNULL(SUM(column17), 0) AS Discount, ISNULL(SUM(column19), 0) AS Adding, column01
                                        FROM          dbo.Table_019_Child1_MarjooiSale
                                        GROUP BY column22, column01
                                        HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                                            Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, ReturnId);
                                            DataTable Table = new DataTable();
                                            Adapter.Fill(Table);

                                            //درج سند فاکتور مرجوعی
                                            string[] _BedInfo = clDoc.ACC_Info(mlt_ReturnBed.Value.ToString());
                                            foreach (DataRow item in Table.Rows)
                                            {

                                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), mlt_ReturnBed.Value.ToString(), Int16.Parse(_BedInfo[0]),
                                                _BedInfo[1], _BedInfo[2], _BedInfo[3], _BedInfo[4], null, null,
                                                (item["Project"].ToString().Trim() == "" ? null : item["Project"].ToString()),
                                                 "فاکتور مرجوعی فروش ش " + ReturnNum + " به تاریخ " + msk_ReturnDate.Text + masool,
                                                 Convert.ToInt64(Convert.ToDouble(item["Net"].ToString())), 0,
                                                 0, 0, -1, 29, ReturnId, Class_BasicOperation._UserName, 0, (short?)null);
                                            }
                                            string[] _BesInfo = clDoc.ACC_Info(mlt_ReturnBes.Value.ToString());
                                            clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), mlt_ReturnBes.Value.ToString(), Int16.Parse(_BesInfo[0]),
                                                _BesInfo[1], _BesInfo[2], _BesInfo[3], _BesInfo[4], SaleRow["Column03"].ToString(), null, null,
                                                "فاکتور مرجوعی فروش ش " + ReturnNum + " به تاریخ " + msk_ReturnDate.Text + masool, 0,
                                                Convert.ToInt64(Convert.ToDouble(ChildTable.Compute("SUM(Column20)", "").ToString())), 0, 0, -1, 29, ReturnId, Class_BasicOperation._UserName, 0, (short?)null);


                                            //درج سند بهای تمام شده
                                            if (groupBox2.Enabled)
                                            {
                                                double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_012_Child_PwhrsReceipt", "ISNULL(SUM(Column21),0)", "Column01", ResidId.ToString()));

                                                //********Bed
                                                string[] _ValueBedInfo = clDoc.ACC_Info(mlt_ValueBed.Value.ToString());
                                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), mlt_ValueBed.Value.ToString(), Int16.Parse(_ValueBedInfo[0]),
                                                _ValueBedInfo[1], _ValueBedInfo[2], _ValueBedInfo[3], _ValueBedInfo[4], null, null, null,
                                                "بهای تمام شده- فاکتور مرجوعی فروش ش " + ReturnNum + " به تاریخ " + msk_ReturnDate.Text + masool,
                                                Convert.ToInt64(TotalValue), 0,
                                                0, 0, -1, 27, ResidId, Class_BasicOperation._UserName, 0, (short?)null);

                                                //*********Bes
                                                string[] _ValueBesInfo = clDoc.ACC_Info(mlt_ValueBes.Value.ToString());
                                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), mlt_ValueBes.Value.ToString(), Int16.Parse(_ValueBesInfo[0]),
                                                _ValueBesInfo[1], _ValueBesInfo[2], _ValueBesInfo[3], _ValueBesInfo[4], null, null, null,
                                                "بهای تمام شده- فاکتور مرجوعی فروش ش " + ReturnNum + " به تاریخ " + msk_ReturnDate.Text + masool, 0,
                                                Convert.ToInt64(TotalValue),
                                                0, 0, -1, 27, ResidId, Class_BasicOperation._UserName, 0, (short?)null);

                                            }

                                            //درج شماره سند در فاکتور مرجوعی
                                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column10", "ColumnId", ReturnId, Convert.ToInt32(DocID.Value));

                                            //درج شماره سند در رسید انبار
                                            if (groupBox2.Enabled)
                                                clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column07", "ColumnId", ResidId, Convert.ToInt32(DocID.Value));

                                            foreach (GridEXRow item in gridEX_Return.GetRows())
                                            {
                                                if (item.Cells["Column08"].Value.ToString() == "0")
                                                {
                                                    item.BeginEdit();
                                                    item.Cells["Column07"].Value = ReturnId;
                                                    item.Cells["Column08"].Value = DocID.Value;
                                                    item.EndEdit();
                                                }
                                            }
                                        }
                                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                                        this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                                    }
                                }
                            }
                        }
                        //***********************************ENF OF FACTORS





                        DataTable DocTable = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead");
                        gridEX_Cash.DropDowns["Doc"].SetDataBinding(DocTable, "");
                        gridEX_Pay.DropDowns["Doc"].SetDataBinding(DocTable, "");
                        gridEX_Discount.DropDowns["Doc"].SetDataBinding(DocTable, "");
                        gridEX_Return.DropDowns["Doc"].SetDataBinding(DocTable, "");
                        gridEX_Return.DropDowns["Paper"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_018_MarjooiSale"), "");
                        if (sender == bt_Save || sender == this)
                            Class_BasicOperation.ShowMsg("", "صدور برگه و ذخیره اطلاعات با موفقیت صورت گرفت", "Information");
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void gridEX_Cash_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (e.Column.Key == "Column05")
                {
                    //سرفصل اصلی بانک یا صندوق به عنوان بدهکار انتخاب می شود
                    if (gridEX_Cash.GetRow().Cells["Bed"].Text.Trim() == "")
                        gridEX_Cash.SetValue("Bed", clDoc.ExScalar(ConBank.ConnectionString, "Table_020_BankCashAccInfo",
                            "Column12", "ColumnId", gridEX_Cash.GetValue("Column05").ToString()));

                    //سرفصل بستانکار از روی تراکنشها پیشنهاد داده می شود
                    if (gridEX_Cash.GetRow().Cells["Bes"].Text.Trim() == "")
                        gridEX_Cash.SetValue("Bes", clDoc.Account(2, "Column13"));

                    gridEX_Cash.SetValue("Column32", gridEX_Cash.DropDowns["Bank"].GetValue("Column35").ToString());
                    if (gridEX_Cash.GetRow().Cells["Column32"].Text.Trim() == "")
                        gridEX_Cash.SetValue("Column32", DBNull.Value);
                }

            }
            catch { }
        }

        private void rdb_New_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_New.Checked)
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                txt_Cover.Text = "تسویه فاکتورها";
                faDatePicker1.SelectedDateTime = DateTime.Now;
                txt_LastNum.Text = null;
                txt_To.Text = null;
            }
            else
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
            }
        }

        private void rdb_last_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_last.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
                int LastNum = clDoc.LastDocNum();
                txt_LastNum.Text = LastNum.ToString();
                faDatePicker1.Text = clDoc.DocDate(LastNum);
                txt_Cover.Text = clDoc.Cover(LastNum);

            }
            else
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                faDatePicker1.SelectedDateTime = DateTime.Now;
                txt_Cover.Text = null;
            }
        }

        private void rdb_To_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_To.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
                txt_LastNum.Text = null;
                txt_To.Text = null;

            }
            else
            {
                txt_To.Text = null;
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
            }
        }

        private void txt_To_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txt_To.Text.Trim() != "")
                {
                    clDoc.IsValidNumber(int.Parse(txt_To.Text.Trim()));
                    faDatePicker1.Text = clDoc.DocDate(int.Parse(txt_To.Text.Trim()));
                    txt_Cover.Text = clDoc.Cover(int.Parse(txt_To.Text.Trim()));
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker1.HideDropDown();
                    Class_BasicOperation.isEnter(e.KeyChar);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = (FarsiLibrary.Win.Controls.FADatePicker)sender;


                if (textBox.Text.Length == 4)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                else if (textBox.Text.Length == 7)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void txt_To_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
        }

        private void gridEX_Cash_ColumnButtonClick(object sender, ColumnActionEventArgs e)
        {
            if (gridEX_Cash.RowCount > 0)
            {
                try
                {
                    if (gridEX_Cash.GetRow().Cells["Column08"].Text != "0")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف برگه دریافت نقد و سند حسابداری مربوط هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            //حذف سند
                            clDoc.DeleteDetail_ID(int.Parse(gridEX_Cash.GetValue("Column08").ToString()), 24, int.Parse(gridEX_Cash.GetValue("Column07").ToString()));
                            //حذف برگه
                            clDoc.RunSqlCommand(ConBank.ConnectionString, "DELETE FROM Table_045_ReceiveCash where ColumnId=" + gridEX_Cash.GetValue("Column07").ToString());
                            gridEX_Cash.SetValue("Column07", 0);
                            gridEX_Cash.SetValue("Column08", 0);
                            gridEX_Cash.UpdateData();
                            this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                            this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);

                            //سرفصل اصلی بانک یا صندوق به عنوان بدهکار انتخاب می شود
                            if (gridEX_Cash.GetRow().Cells["Bed"].Text.Trim() == "")
                                gridEX_Cash.SetValue("Bed", clDoc.ExScalar(ConBank.ConnectionString, "Table_020_BankCashAccInfo",
                                    "Column12", "ColumnId", gridEX_Cash.GetValue("Column05").ToString()));

                            //سرفصل بستانکار از روی تراکنشها پیشنهاد داده می شود
                            if (gridEX_Cash.GetRow().Cells["Bes"].Text.Trim() == "")
                                gridEX_Cash.SetValue("Bes", clDoc.Account(2, "Column13"));


                            Class_BasicOperation.ShowMsg("", "حذف برگه و سند حسابداری با موفقیت صورت گرفت", "Information");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void mnu_ViewDocs_Click(object sender, EventArgs e)
        {
            int SanadId = 0;
            try
            {
                if (this.table_034_SaleFactor_Child3BindingSource.Count > 0)
                    SanadId = int.Parse(((DataRowView)this.table_034_SaleFactor_Child3BindingSource.CurrencyManager.Current)["Column08"].ToString());
            }
            catch { }
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 19))
            {
                PACNT._2_DocumentMenu.Form01_AccDocument frm = new PACNT._2_DocumentMenu.Form01_AccDocument(
                  UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 20), int.Parse(SanadId.ToString()));
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ViewCashPaper_Click(object sender, EventArgs e)
        {
            int PaperId = 0;
            try
            {
                PaperId = int.Parse(gridEX_Cash.GetValue("Column07").ToString());
            }
            catch { }
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 20))
            {
                PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
                PACNT._2_Cash_Operation.Form05_ViewReceivedCash frm = new PACNT._2_Cash_Operation.Form05_ViewReceivedCash(PaperId);
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ViewCheques_Click(object sender, EventArgs e)
        {
            int PaperId = 0;
            try
            {
                PaperId = int.Parse(gridEX_Pay.GetValue("Column07").ToString());
            }
            catch { }
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 27))
            {
                PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
                PACNT._3_Cheque_Operation.Form01_ReceiveChq frm = new PACNT._3_Cheque_Operation.Form01_ReceiveChq(
                      UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 28), UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 29),
                       UserScope.CheckScope(Class_BasicOperation._UserName, "Column09", 30), PaperId);
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void mnu_ViewReturnFactors_Click(object sender, EventArgs e)
        {
            int PaperId = 0;
            try
            {
                PaperId = int.Parse(gridEX_Return.GetValue("Column07").ToString());
            }
            catch { }
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 22))
            {
                _05_Sale.Frm_013_ReturnFactor frm = new Frm_013_ReturnFactor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 23), PaperId);
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX_Cash_DeletingRecord(object sender, RowActionCancelEventArgs e)
        {
            try
            {
                if (gridEX_Cash.GetValue("Column08").ToString() != "0")
                    e.Cancel = true;
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف این سطر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        e.Row.Delete();
                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                        this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                    }
                    else e.Cancel = true;
                }
            }
            catch
            {
            }
        }

        private void gridEX_Cash_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void uiPanel0_SelectedPanelChanged(object sender, Janus.Windows.UI.Dock.PanelActionEventArgs e)
        {
            if (e.Panel.Name == "uiPanel1")
                this.table_034_SaleFactor_Child3BindingSource.Filter = "Column03=1";
            else if (e.Panel.Name == "uiPanel2")
                this.table_034_SaleFactor_Child3BindingSource.Filter = "Column03=2";
            else if (e.Panel.Name == "uiPanel3")
                this.table_034_SaleFactor_Child3BindingSource.Filter = "Column03=3";
            else if (e.Panel.Name == "uiPanel4")
                this.table_034_SaleFactor_Child3BindingSource.Filter = "Column03=4";
            else if (e.Panel.Name == "uiPanel6")
                this.table_034_SaleFactor_Child3BindingSource.Filter = "Column03=6";
        }

        private void gridEX_Pay_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (e.Column.Key == "Column05")
                {
                    if (gridEX_Pay.GetRow().Cells["Column14"].Text.Trim() != "")
                    {
                        //سرفصل بدهکار از روی وضعیت چک تعیین می شود
                        gridEX_Pay.SetValue("Bed", clDoc.ExScalar(ConBank.ConnectionString, "Table_060_ChequeStatus", "Column03", "ColumnId", gridEX_Pay.GetValue("Column14").ToString()));

                        //سرفصل بستانکار هم از روی وضعیت چک تعیین می شود
                        gridEX_Pay.SetValue("Bes", clDoc.ExScalar(ConBank.ConnectionString, "Table_060_ChequeStatus", "Column09", "ColumnId", gridEX_Pay.GetValue("Column14").ToString()));
                    }

                    gridEX_Pay.SetValue("Column32", gridEX_Pay.DropDowns["CashBank"].GetValue("Column35").ToString());
                    if (gridEX_Pay.GetRow().Cells["Column32"].Text.Trim() == "")
                        gridEX_Pay.SetValue("Column32", DBNull.Value);
                }
            }
            catch { }
        }

        private void gridEX_Pay_ColumnButtonClick(object sender, ColumnActionEventArgs e)
        {
            if (gridEX_Pay.RowCount > 0)
            {
                try
                {
                    if (gridEX_Pay.GetRow().Cells["Column08"].Text != "0")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف برگه دریافت چک و سند حسابداری مربوط هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            //حذف سند
                            Janus.Windows.GridEX.GridEXRow item = gridEX_Pay.GetRow();
                            if (item.Cells["Column08"].Value.ToString() != "0")
                            {

                                //حذف گردش و اسناد
                                SqlDataAdapter SelectAdapter = new SqlDataAdapter("Select * from Table_065_TurnReception where Column01=" + item.Cells["Column07"].Value.ToString(), ConBank);
                                DataTable TurnRows = new DataTable();
                                SelectAdapter.Fill(TurnRows);
                                if (TurnRows.Rows.Count > 1)
                                {
                                    if (DialogResult.Yes == MessageBox.Show("برای این چک علاوه بر گردش دریافت، گردشهای دیگری نیز ثبت شده است" + Environment.NewLine +
                                        "در صورت تأیید تمام گردشها و اسناد مربوطه حذف خواهند شد" + Environment.NewLine + "آیا مایل به ادامه هستید؟", "", MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                                    {
                                        foreach (DataRow Row in TurnRows.Rows)
                                        {
                                            //حذف سند
                                            clDoc.IsFinal_ID(int.Parse(Row["Column13"].ToString()));
                                            //حذف سند اول که با نوع 28 درج می شود
                                            clDoc.DeleteDetail_ID(int.Parse(Row["Column13"].ToString()), 28, int.Parse(Row["ColumnId"].ToString()));
                                            clDoc.DeleteDetail_ID(int.Parse(Row["Column13"].ToString()), short.Parse(Row["Column02"].ToString()), int.Parse(Row["ColumnId"].ToString()));
                                            clDoc.DeleteTurnReception(long.Parse(Row["ColumnId"].ToString()));
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (DataRow Row in TurnRows.Rows)
                                    {
                                        //حذف سند اول که با نوع 28 درج می شود
                                        clDoc.DeleteDetail_ID(int.Parse(Row["Column13"].ToString()), 28, int.Parse(Row["ColumnId"].ToString()));
                                        clDoc.IsFinal_ID(int.Parse(Row["Column13"].ToString()));
                                        clDoc.DeleteDetail_ID(int.Parse(Row["Column13"].ToString()), short.Parse(Row["Column02"].ToString()), int.Parse(Row["ColumnId"].ToString()));
                                        clDoc.DeleteTurnReception(long.Parse(Row["ColumnId"].ToString()));
                                    }
                                }
                            }

                            //حذف برگه
                            clDoc.RunSqlCommand(ConBank.ConnectionString, "DELETE FROM Table_035_ReceiptCheques where ColumnId=" + gridEX_Pay.GetValue("Column07").ToString());
                            gridEX_Pay.SetValue("Column07", 0);
                            gridEX_Pay.SetValue("Column08", 0);
                            gridEX_Pay.UpdateData();
                            this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                            this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);

                            if (gridEX_Pay.GetRow().Cells["Column14"].Text.Trim() != "")
                            {
                                //سرفصل بدهکار از روی وضعیت چک تعیین می شود
                                if (gridEX_Pay.GetRow().Cells["Bed"].Text.Trim() == "")
                                    gridEX_Pay.SetValue("Bed", clDoc.ExScalar(ConBank.ConnectionString, "Table_060_ChequeStatus", "Column03", "ColumnId", gridEX_Pay.GetValue("Column14").ToString()));

                                //سرفصل بستانکار هم از روی وضعیت چک تعیین می شود
                                if (gridEX_Pay.GetRow().Cells["Bes"].Text.Trim() == "")
                                    gridEX_Pay.SetValue("Bes", clDoc.ExScalar(ConBank.ConnectionString, "Table_060_ChequeStatus", "Column09", "ColumnId", gridEX_Pay.GetValue("Column14").ToString()));
                            }


                            Class_BasicOperation.ShowMsg("", "حذف برگه و سند حسابداری با موفقیت صورت گرفت", "Information");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void gridEX_Pay_DeletingRecord(object sender, RowActionCancelEventArgs e)
        {
            try
            {
                if (gridEX_Pay.GetValue("Column08").ToString() != "0")
                    e.Cancel = true;
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف این سطر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        e.Row.Delete();
                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                        this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                    }
                    else e.Cancel = true;
                }
            }
            catch
            {
            }
        }

        private void gridEX_Discount_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                //درج سرفصل بدهکار و بستانکار از روی تراکنش تخفیفات خطی انتهای فاکتور فروش
                if (gridEX_Discount.GetRow().Cells["Bed"].Text.Trim() == "")
                    gridEX_Discount.SetValue("Bed", clDoc.Account(6, "Column07").Trim());

                if (gridEX_Discount.GetRow().Cells["Bes"].Text.Trim() == "")
                    gridEX_Discount.SetValue("Bes", clDoc.Account(6, "Column13").Trim());

            }
            catch { }
        }

        private void gridEX_Discount_ColumnButtonClick(object sender, ColumnActionEventArgs e)
        {
            if (gridEX_Discount.RowCount > 0)
            {
                try
                {
                    if (gridEX_Discount.GetRow().Cells["Column08"].Text != "0")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف سند حسابداری مربوط هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            //حذف سند
                            clDoc.DeleteDetail_ID(int.Parse(gridEX_Discount.GetValue("Column08").ToString()), 30, int.Parse(gridEX_Discount.GetValue("ColumnId").ToString()));
                            gridEX_Discount.SetValue("Column08", 0);
                            gridEX_Discount.UpdateData();
                            this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                            this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);

                            //درج سرفصل بدهکار و بستانکار از روی تراکنش تخفیفات خطی انتهای فاکتور فروش
                            if (gridEX_Discount.GetRow().Cells["Bed"].Text.Trim() == "")
                                gridEX_Discount.SetValue("Bed", clDoc.Account(6, "Column07").Trim());

                            if (gridEX_Discount.GetRow().Cells["Bes"].Text.Trim() == "")
                                gridEX_Discount.SetValue("Bes", clDoc.Account(6, "Column13").Trim());


                            Class_BasicOperation.ShowMsg("", "حذف برگه و سند حسابداری با موفقیت صورت گرفت", "Information");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void gridEX_Discount_DeletingRecord(object sender, RowActionCancelEventArgs e)
        {
            try
            {
                if (gridEX_Discount.GetValue("Column08").ToString() != "0")
                    e.Cancel = true;
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف این سطر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        e.Row.Delete();
                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                        this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                    }
                    else e.Cancel = true;
                }
            }
            catch
            {
            }
        }

        private void mlt_ReturnBed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else Class_BasicOperation.isEnter(e.KeyChar);
            }
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void gridEX_Return_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (e.Column.Key == "Column15")
                    gridEX_Return.SetValue("GoodCode", gridEX_Return.GetValue("Column15").ToString());
                else if (e.Column.Key == "GoodCode")
                    gridEX_Return.SetValue("Column15", gridEX_Return.GetValue("GoodCode").ToString());

                if (e.Column.Key == "Column15" || e.Column.Key == "GoodCode")
                {
                    GoodbindingSource.Filter = "GoodID=" + gridEX_Return.GetRow().Cells["Column15"].Value.ToString();
                    gridEX_Return.SetValue("tedaddarkartoon", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                    gridEX_Return.SetValue("tedaddarbaste", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                    gridEX_Return.SetValue("Column16", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                    gridEX_Return.SetValue("Column25", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());
                    gridEX_Return.SetValue("Column27", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Extra"].ToString());
                    gridEX_Return.SetValue("Column23", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePrice"].ToString());
                    gridEX_Return.SetValue("Column22", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePackPrice"].ToString());
                    gridEX_Return.SetValue("Column21", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["SaleBoxPrice"].ToString());
                    gridEX_Return.SetValue("Column17", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["BoxNumber"].ToString());
                    gridEX_Return.SetValue("Column18", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["PackNumber"].ToString());
                    gridEX_Return.SetValue("Column19", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["DetailNumber"].ToString());
                    gridEX_Return.SetValue("BoxNumber", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["BoxNumber"].ToString());
                    gridEX_Return.SetValue("PackNumber", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["PackNumber"].ToString());
                    gridEX_Return.SetValue("DetailNumber", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["DetailNumber"].ToString());
                    gridEX_Return.SetValue("Column20", Convert.ToDouble(gridEX_Return.GetValue("Column17")) * Convert.ToDouble(gridEX_Return.GetValue("tedaddarkartoon")));
                    gridEX_Return.SetValue("Column20", Convert.ToDouble(gridEX_Return.GetValue("Column20")) + (Convert.ToDouble(gridEX_Return.GetValue("Column18")) * Convert.ToDouble(gridEX_Return.GetValue("tedaddarbaste"))));
                    gridEX_Return.SetValue("Column20", Convert.ToDouble(gridEX_Return.GetValue("Column20")) + Convert.ToDouble(gridEX_Return.GetValue("Column19")));
                    gridEX_Return.SetValue("Column24", Convert.ToDouble(gridEX_Return.GetValue("Column17")) * Convert.ToDouble(gridEX_Return.GetValue("Column21")));
                    gridEX_Return.SetValue("Column24", Convert.ToDouble(gridEX_Return.GetValue("Column24")) + (Convert.ToDouble(gridEX_Return.GetValue("Column18")) * Convert.ToDouble(gridEX_Return.GetValue("Column22"))));
                    gridEX_Return.SetValue("Column24", Convert.ToDouble(gridEX_Return.GetValue("Column24")) + (Convert.ToDouble(gridEX_Return.GetValue("Column19")) * Convert.ToDouble(gridEX_Return.GetValue("Column23"))));
                    double jam, takhfif, ezafe;
                    jam = Convert.ToDouble(gridEX_Return.GetValue("Column24"));
                    takhfif = (jam * (Convert.ToDouble(gridEX_Return.GetValue("Column25")) / 100));
                    ezafe = (Convert.ToDouble(gridEX_Return.GetValue("Column24")) * (Convert.ToDouble(gridEX_Return.GetValue("Column27")) / 100));
                    gridEX_Return.SetValue("Column26", Convert.ToInt64(takhfif));
                    gridEX_Return.SetValue("Column28", Convert.ToInt64(ezafe));
                    gridEX_Return.SetValue("Column29", (jam - takhfif) + ezafe);
                }

            }
            catch
            { }
        }

        private void gridEX_Return_DeletingRecord(object sender, RowActionCancelEventArgs e)
        {
            try
            {
                if (gridEX_Return.GetValue("Column08").ToString() != "0")
                    e.Cancel = true;
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف این سطر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        e.Row.Delete();
                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                        this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                    }
                    else e.Cancel = true;
                }
            }
            catch
            {
            }
        }

        private void gridEX_Return_UpdatingCell(object sender, UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Column.Key == "Column17" || e.Column.Key == "Column18" || e.Column.Key == "Column19")
                {
                    gridEX_Return.SetValue("Column20", Convert.ToDouble(gridEX_Return.GetValue("Column17")) * Convert.ToDouble(gridEX_Return.GetValue("tedaddarkartoon")));
                    gridEX_Return.SetValue("Column20", Convert.ToDouble(gridEX_Return.GetValue("Column20")) + (Convert.ToDouble(gridEX_Return.GetValue("Column18")) * Convert.ToDouble(gridEX_Return.GetValue("tedaddarbaste"))));
                    gridEX_Return.SetValue("Column20", Convert.ToDouble(gridEX_Return.GetValue("Column20")) + Convert.ToDouble(gridEX_Return.GetValue("Column19")));
                }
                gridEX_Return.SetValue("Column24", Convert.ToDouble(gridEX_Return.GetValue("Column17")) * Convert.ToDouble(gridEX_Return.GetValue("Column21")));
                gridEX_Return.SetValue("Column24", Convert.ToDouble(gridEX_Return.GetValue("Column24")) + (Convert.ToDouble(gridEX_Return.GetValue("Column18")) * Convert.ToDouble(gridEX_Return.GetValue("Column22"))));
                gridEX_Return.SetValue("Column24", Convert.ToDouble(gridEX_Return.GetValue("Column24")) + (Convert.ToDouble(gridEX_Return.GetValue("Column19")) * Convert.ToDouble(gridEX_Return.GetValue("Column23"))));
                double jam, takhfif, ezafe;
                jam = Convert.ToDouble(gridEX_Return.GetValue("Column24"));
                takhfif = (jam * (Convert.ToDouble(gridEX_Return.GetValue("Column25")) / 100));
                ezafe = (Convert.ToDouble(gridEX_Return.GetValue("Column24")) * (Convert.ToDouble(gridEX_Return.GetValue("Column27")) / 100));
                gridEX_Return.SetValue("Column26", Convert.ToInt64(takhfif));
                gridEX_Return.SetValue("Column28", Convert.ToInt64(ezafe));
                gridEX_Return.SetValue("Column29", (jam - takhfif) + ezafe);
            }
            catch { }
        }

        private void gridEX_Return_ColumnButtonClick(object sender, ColumnActionEventArgs e)
        {
            if (gridEX_Return.RowCount > 0)
            {
                try
                {
                    if (gridEX_Return.GetRow().Cells["Column08"].Text != "0")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف سند حسابداری مربوط هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            //حذف رسید انبار
                            int ResidId = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column09", "ColumnId", gridEX_Return.GetValue("Column07").ToString()));
                            clDoc.RunSqlCommand(ConWare.ConnectionString, "DELETE FROM Table_012_Child_PwhrsReceipt Where Column01=" + ResidId);
                            clDoc.RunSqlCommand(ConWare.ConnectionString, "DELETE FROM Table_011_PwhrsReceipt Where ColumnId=" + ResidId);

                            //حذف فاکتور مرجوعی
                            clDoc.RunSqlCommand(ConSale.ConnectionString, "DELETE FROM Table_019_Child1_MarjooiSale WHERE Column01=" + gridEX_Return.GetValue("Column07").ToString());
                            clDoc.RunSqlCommand(ConSale.ConnectionString, "DELETE FROM Table_020_Child2_MarjooiSale WHERE Column01=" + gridEX_Return.GetValue("Column07").ToString());
                            clDoc.RunSqlCommand(ConSale.ConnectionString, "DELETE FROM Table_018_MarjooiSale WHERE ColumnId=" + gridEX_Return.GetValue("Column07").ToString());

                            //حذف سند
                            clDoc.DeleteDetail_ID(int.Parse(gridEX_Return.GetValue("Column08").ToString()), 29, int.Parse(gridEX_Return.GetValue("Column07").ToString()));
                            clDoc.DeleteDetail_ID(int.Parse(gridEX_Return.GetValue("Column08").ToString()), 27, ResidId);
                            int PaperId = int.Parse(gridEX_Return.GetValue("Column07").ToString());
                            int DocId = int.Parse(gridEX_Return.GetValue("Column08").ToString());
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Return.GetRows())
                            {
                                if (item.Cells["Column07"].Value.ToString() == PaperId.ToString() && item.Cells["Column08"].Value.ToString() == DocId.ToString())
                                {
                                    item.BeginEdit();
                                    item.Cells["Column07"].Value = 0;
                                    item.Cells["Column08"].Value = 0;
                                    item.EndEdit();
                                }
                            }
                            gridEX_Return.SetValue("Column08", 0);
                            gridEX_Return.SetValue("Column07", 0);
                            gridEX_Return.UpdateData();
                            this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                            this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);

                            GoodbindingSource.Filter = "GoodID=" + gridEX_Return.GetRow().Cells["Column15"].Value.ToString();
                            gridEX_Return.SetValue("BoxNumber", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["BoxNumber"].ToString());
                            gridEX_Return.SetValue("PackNumber", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["PackNumber"].ToString());
                            gridEX_Return.SetValue("DetailNumber", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["DetailNumber"].ToString());
                            gridEX_Return.SetValue("tedaddarkartoon", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                            gridEX_Return.SetValue("tedaddarbaste", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());


                            Class_BasicOperation.ShowMsg("", "حذف برگه ها و سند حسابداری با موفقیت صورت گرفت", "Information");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void msk_ReturnDate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                gridEX_Return.RootTable.Columns["Column02"].DefaultValue = msk_ReturnDate.Text;
            }
            catch { }
        }

        private void mnu_ViewWareReceipt_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 22))
            {
                PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
                PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
                PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PWHRS._03_AmaliyatAnbar.Form04_ViewWareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form04_ViewWareReceipt();
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void tabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.TabStripTabChangedEventArgs e)
        {
            if (tabControl1.SelectedTab == tabItem2)
                gridEX_Cash.Refetch();
            else if (tabControl1.SelectedTab == tabItem1)
                gridEX_Pay.Refetch();
            else if (tabControl1.SelectedTab == tabItem3)
                gridEX_Discount.Refetch();
            else if (tabControl1.SelectedTab == tabItem4)
            {
                gridEX_Return.Refetch();
                foreach (GridEXRow item in gridEX_Return.GetRows())
                {
                    if (item.Cells["Column08"].Text.Trim() == "0")
                    {
                        GoodbindingSource.Filter = "GoodID=" + item.Cells["Column15"].Value.ToString();
                        item.BeginEdit();
                        item.Cells["BoxNumber"].Value = ((DataRowView)GoodbindingSource.CurrencyManager.Current)["BoxNumber"].ToString();
                        item.Cells["PackNumber"].Value = ((DataRowView)GoodbindingSource.CurrencyManager.Current)["PackNumber"].ToString();
                        item.Cells["DetailNumber"].Value = ((DataRowView)GoodbindingSource.CurrencyManager.Current)["DetailNumber"].ToString();
                        item.Cells["tedaddarkartoon"].Value = ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString();
                        item.Cells["tedaddarbaste"].Value = ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString();
                        item.EndEdit();
                    }
                }
            }
            else if (tabControl1.SelectedTab == tabItem5)
            {
                gridEX_From.Refetch();
                gridEX_To.Refetch();
            }

        }

        private void gridEX_To_EditingCell(object sender, EditingCellEventArgs e)
        {
            try
            {
                if (gridEX_To.GetValue("Column31").ToString() == "True")
                    e.Cancel = true;
            }
            catch
            {
            }
        }

        private void gridEX_To_ColumnButtonClick(object sender, ColumnActionEventArgs e)
        {
            if (gridEX_To.RowCount > 0)
            {
                try
                {
                    if (gridEX_To.GetRow().Cells["Column31"].Value.ToString() == "True")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف انتقال صورت گرفته هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.RunSqlCommand(ConSale.ConnectionString, "DELETE FROM Table_034_SaleFactor_Child3 where Column01=" + gridEX_To.GetValue("Column07").ToString() +
                                " and Column03=5 and Column07=" + _SaleFactor);
                            gridEX_To.SetValue("Column31", false);
                            gridEX_To.MoveToNewRecord();
                            this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                            this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);

                            Class_BasicOperation.ShowMsg("", "حذف انتقال با موفقیت صورت گرفت", "Information");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void gridEX_To_DeletingRecord(object sender, RowActionCancelEventArgs e)
        {
            try
            {
                if (gridEX_To.GetValue("Column31").ToString() == "True")
                    e.Cancel = true;
                else
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف این سطر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        e.Row.Delete();
                        //this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                        //this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                    }
                    else e.Cancel = true;
                }
            }
            catch
            {
            }
        }

        private void gridEX_To_RecordAdded(object sender, EventArgs e)
        {
            try
            {
                gridEX_From.Refetch();
            }
            catch
            {
            }
        }

        private void ValidFactor(string FactorId, string FactorNumber)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand("Select (Column17|Column19) as ReturnCancel , Column10 from Table_010_SaleFactor where columnid=" + FactorId, Con);
                SqlDataReader Reader = Command.ExecuteReader();
                Reader.Read();
                if (Reader.HasRows)
                {
                    if (Reader["Column10"].ToString() == "0")
                    {
                        Reader.Close();
                        throw new Exception("به علت عدم صدور سند حسابداری برای فاکتور شماره " + FactorNumber + " انتقال مبلغ امکانپذیر نیست");
                    }
                    if (Reader["ReturnCancel"].ToString() == "1")
                    {
                        Reader.Close();
                        throw new Exception("به علت مرجوع شدن، یا باطل بودن فاکتور شماره " + FactorNumber + " انتقال مبلغ امکانپذیر نیست");
                    }
                }
                else
                {
                    Reader.Close();
                    throw new Exception("شماره فاکتور وارد شده نامعتبر می باشد");
                }

                Reader.Close();
            }

        }

        private void mnu_ViewSettleFactors_Click(object sender, EventArgs e)
        {
            try
            {
                int FactorNumber = 0;
                try
                {
                    FactorNumber = int.Parse(gridEX_To.GetRow().Cells["Column07"].Value.ToString());
                }
                catch
                {
                }
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 165))
                {
                    _05_Sale.Frm_017_SettleSaleFactor frm = new Frm_017_SettleSaleFactor(FactorNumber, false);
                    frm.ShowDialog();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
            catch { }
        }

        private DataTable FactorGoods()
        {
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     GoodTable.GoodID, GoodTable.GoodCode, GoodTable.GoodName, GoodTable.CountUnit, GoodTable.NumberInBox, GoodTable.NumberInPack, 
                    GoodTable.SubGroupName, GoodTable.MainGroupName, Table_011_Child1_SaleFactor_1.column04 AS BoxNumber, 
                    Table_011_Child1_SaleFactor_1.column05 AS PackNumber, Table_011_Child1_SaleFactor_1.column06 AS DetailNumber, 
                    Table_011_Child1_SaleFactor_1.column07 AS TotalNumber, Table_011_Child1_SaleFactor_1.column08 AS SaleBoxPrice, 
                    Table_011_Child1_SaleFactor_1.column09 AS SalePackPrice, Table_011_Child1_SaleFactor_1.column10 AS SalePrice, 
                    Table_011_Child1_SaleFactor_1.column16 AS Discount, Table_011_Child1_SaleFactor_1.column18 AS Extra
                    FROM         (SELECT     GoodsInformation.id AS GoodID, GoodsInformation.Code AS GoodCode, GoodsInformation.name AS GoodName, 
                    GoodsInformation.CountUnit,ISNULL(GoodsInformation.NumberInBox, 0) AS NumberInBox, ISNULL(GoodsInformation.NumberInPack, 0) AS NumberInPack, 
                    GoodsInformation.Active1, GoodsInformation.Active2, 
                    {0}.dbo.table_003_SubsidiaryGroup.column03 AS SubGroupName, 
                    {0}.dbo.table_002_MainGroup.column02 AS MainGroupName
                    FROM          (SELECT     {0}.dbo.table_004_CommodityAndIngredients.columnid AS id, 
                    {0}.dbo.table_004_CommodityAndIngredients.column01 AS Code, 
                    {0}.dbo.table_004_CommodityAndIngredients.column02 AS name, 
                    {0}.dbo.table_004_CommodityAndIngredients.column03 AS goroohasli, 
                    {0}.dbo.table_004_CommodityAndIngredients.column04 AS goroohfari, 
                    {0}.dbo.table_004_CommodityAndIngredients.column07 AS CountUnit, 
                    CASE WHEN {0}.dbo.table_006_CommodityChanges.Column07 IS NULL 
                    THEN {0}.dbo.table_004_CommodityAndIngredients.column09 ELSE {0}.dbo.table_006_CommodityChanges.Column07
                    END AS NumberInBox, CASE WHEN {0}.dbo.table_006_CommodityChanges.Column06 IS NULL 
                    THEN {0}.dbo.table_004_CommodityAndIngredients.column08 ELSE {0}.dbo.table_006_CommodityChanges.Column06
                    END AS NumberInPack, ISNULL({0}.dbo.table_006_CommodityChanges.column18, 1) AS Active1, ISNULL(TS003.Column11, 1) AS Active2, 
                    {0}.dbo.table_004_CommodityAndIngredients.column28 AS Active3
                    FROM          {0}.dbo.table_004_CommodityAndIngredients LEFT OUTER JOIN
                  
                    (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, 
                            Column11
                    FROM         {0}.dbo.Table_003_InformationProductCash) AS TS003 ON 
                    {0}.dbo.table_004_CommodityAndIngredients.columnid = TS003.column01 LEFT OUTER JOIN
                    {0}.dbo.table_006_CommodityChanges ON 
                    {0}.dbo.table_004_CommodityAndIngredients.columnid = {0}.dbo.table_006_CommodityChanges.column01) 
                    AS GoodsInformation INNER JOIN
                    {0}.dbo.table_003_SubsidiaryGroup ON GoodsInformation.goroohfari = {0}.dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
                    {0}.dbo.table_002_MainGroup ON 
                    {0}.dbo.table_003_SubsidiaryGroup.column01 = {0}.dbo.table_002_MainGroup.columnid
                    WHERE      (GoodsInformation.Active1 = 1) AND (GoodsInformation.Active2 = 1) AND (GoodsInformation.Active3 = 1) AND (GoodsInformation.id IN
                    (SELECT     column02 AS GoodID
                    FROM          dbo.Table_011_Child1_SaleFactor
                    WHERE      (column01 = {1})))) AS GoodTable INNER JOIN
                    dbo.Table_011_Child1_SaleFactor AS Table_011_Child1_SaleFactor_1 ON GoodTable.GoodID = Table_011_Child1_SaleFactor_1.column02
                    WHERE     (Table_011_Child1_SaleFactor_1.column01 = {1})", ConSale);

            Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, ConWare.Database, _SaleFactor);
            DataTable GoodTable = new DataTable();
            Adapter.Fill(GoodTable);
            return GoodTable;

        }

        private void gridEX_Return_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX_Return.CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "Column17" || e.Column.Key == "Column18" || e.Column.Key == "Column19")
                {
                    GoodbindingSource.Filter = "GoodID=" + gridEX_Return.GetRow().Cells["Column15"].Value.ToString();

                    if (e.Column.Key == "Column17")
                    {
                        if (double.Parse(gridEX_Return.GetValue("Column17").ToString()) > double.Parse(gridEX_Return.GetValue("BoxNumber").ToString()))
                        {
                            gridEX_Return.SetValue("Column17", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["BoxNumber"].ToString());
                            gridEX_Return.SetValue("BoxNumber", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["BoxNumber"].ToString());
                        }

                    }
                    else if (e.Column.Key == "Column18")
                    {
                        if (double.Parse(gridEX_Return.GetValue("Column18").ToString()) > double.Parse(gridEX_Return.GetValue("PackNumber").ToString()))
                        {
                            gridEX_Return.SetValue("Column18", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["PackNumber"].ToString());
                            gridEX_Return.SetValue("PackNumber", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["PackNumber"].ToString());
                        }
                    }
                    else if (e.Column.Key == "Column19")
                    {
                        if (double.Parse(gridEX_Return.GetValue("Column19").ToString()) > double.Parse(gridEX_Return.GetValue("DetailNumber").ToString()))
                        {
                            gridEX_Return.SetValue("Column19", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["DetailNumber"].ToString());
                            gridEX_Return.SetValue("DetailNumber", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["DetailNumber"].ToString());
                        }
                    }
                }
            }
            catch { }
        }

        private void RefreshPrices()
        {
            //try
            //{
            Int64 FactorPrice = Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column28").ToString())) -
                 (Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column29").ToString())) +
                 Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column30").ToString())) +
                 Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column31").ToString())) +
                 Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column33").ToString()))) +
                 Convert.ToInt64(Convert.ToDouble(gridEX1.GetValue("Column32").ToString()));


            Int64 TotalSettle = (Convert.ToInt64(Convert.ToDouble(gridEX_Cash.GetTotalRow().Cells["Column04"].Value.ToString())) +
                Convert.ToInt64(Convert.ToDouble(gridEX_Pay.GetTotalRow().Cells["Column04"].Value.ToString())) +
                Convert.ToInt64(Convert.ToDouble(gridEX_Discount.GetTotalRow().Cells["Column04"].Value.ToString())) +
                Convert.ToInt64(Convert.ToDouble(gridEX_Return.GetTotalRow().Cells["Column29"].Value.ToString())) +
                Convert.ToInt64(Convert.ToDouble(gridEX_From.GetTotalRow().Cells["Column04"].Value.ToString())) -
                Convert.ToInt64(Convert.ToDouble(gridEX_To.GetTotalRow().Cells["Column04"].Value.ToString())));
            if (FactorPrice >= TotalSettle)
                lbl_PriceStatus.Text = " مانده فاکتور: " + Class_BasicOperation.ToRial(FactorPrice - TotalSettle);
            else if (FactorPrice < TotalSettle)
                lbl_PriceStatus.Text = "مبلغ قابل انتقال: " + Class_BasicOperation.ToRial(TotalSettle - FactorPrice);
            //}
            //catch { }

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            bt_Save_Click(sender, e);
            DataTable FactorTable = dataSet_Sale.Settle_Factor.Clone();
            FactorTable.Rows.Add(gridEX1.GetRow().Cells["Column01"].Value.ToString(),
                gridEX1.GetRow().Cells["Column02"].Value.ToString(),
                gridEX1.GetRow().Cells["Column03"].Text.ToString(),
                gridEX1.GetRow().Cells["Column28"].Value.ToString());

            gridEX_Cash.Refetch();
            gridEX_Discount.Refetch();
            gridEX_Pay.Refetch();
            gridEX_Return.Refetch();
            gridEX_To.Refetch();
            gridEX_From.Refetch();

            DataTable CashTable = dataSet_Sale.Settle_Cash.Clone();
            foreach (GridEXRow item in gridEX_Cash.GetRows())
            {
                CashTable.Rows.Add(item.Cells["Column02"].Value.ToString(),
                    item.Cells["Column04"].Value.ToString(),
                    item.Cells["Column05"].Text,
                    item.Cells["Column06"].Text.Trim(),
                    item.Cells["Column07"].Text,
                    item.Cells["Column08"].Text,
                    item.Cells["Column30"].Text);
            }

            DataTable ChqTable = dataSet_Sale.Settle_Chq.Clone();
            foreach (GridEXRow item in gridEX_Pay.GetRows())
            {
                ChqTable.Rows.Add(item.Cells["Column02"].Value.ToString(),
                    item.Cells["Column04"].Value.ToString(),
                    item.Cells["Column05"].Text.Trim(),
                    item.Cells["Column06"].Text.Trim(),
                    item.Cells["Column07"].Text,
                    item.Cells["Column08"].Text,
                    item.Cells["Column09"].Value.ToString(),
                    item.Cells["Column10"].Value.ToString(),
                    item.Cells["Column11"].Text,
                    item.Cells["Column12"].Text.Trim(),
                    item.Cells["Column13"].Value.ToString(),
                    item.Cells["Column14"].Text,
                    item.Cells["Column30"].Text.Trim());
            }

            DataTable ReturnTable = dataSet_Sale.Settle_ReturnFactor.Clone();
            foreach (GridEXRow item in gridEX_Return.GetRows())
            {
                ReturnTable.Rows.Add(item.Cells["Column02"].Value.ToString(),
                    item.Cells["Column07"].Text.Trim(),
                    item.Cells["Column08"].Text.Trim(),
                    item.Cells["GoodCode"].Text.Trim(),
                    item.Cells["Column15"].Text.Trim(),
                    item.Cells["Column16"].Text.Trim(),
                    item.Cells["Column17"].Value.ToString(),
                    item.Cells["Column18"].Value.ToString(),
                    item.Cells["Column19"].Value.ToString(),
                    item.Cells["Column20"].Value.ToString(),
                    item.Cells["Column21"].Value.ToString(),
                    item.Cells["Column22"].Value.ToString(),
                    item.Cells["Column23"].Value.ToString(),
                    item.Cells["Column24"].Value.ToString(),
                    item.Cells["Column25"].Value.ToString(),
                    item.Cells["Column26"].Value.ToString(),
                    item.Cells["Column27"].Value.ToString(),
                    item.Cells["Column28"].Value.ToString(),
                    item.Cells["Column29"].Value.ToString(),
                    item.Cells["Column30"].Text.ToString());
            }

            DataTable DiscountTable = dataSet_Sale.Settle_Discounts.Clone();
            foreach (GridEXRow item in gridEX_Discount.GetRows())
            {
                DiscountTable.Rows.Add(item.Cells["Column02"].Value.ToString(),
                    item.Cells["Column04"].Value.ToString(),
                    item.Cells["Column06"].Value.ToString(),
                    item.Cells["Column08"].Text.Trim());
            }

            DataTable FromToTable = dataSet_Sale.Settle_FromFactor.Clone();
            foreach (GridEXRow item in gridEX_From.GetRows())
            {
                FromToTable.Rows.Add(item.Cells["Column07"].Text,
                    item.Cells["Column02"].Value.ToString(),
                    item.Cells["Column04"].Value.ToString(),
                    item.Cells["Column06"].Value.ToString(),
                    "From");
            }

            foreach (GridEXRow item in gridEX_To.GetRows())
            {
                FromToTable.Rows.Add(item.Cells["Column07"].Text,
                    item.Cells["Column02"].Value.ToString(),
                    item.Cells["Column04"].Value.ToString(),
                    item.Cells["Column06"].Value.ToString(),
                    "To");
            }


            _05_Sale.Reports.ReportFormAmani frm = new Reports.ReportFormAmani(1, FactorTable, CashTable, ChqTable, ReturnTable, DiscountTable, FromToTable);
            frm.ShowDialog();
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            RefreshPrices();
        }

        private void Frm_017_SettleSaleFactor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.KeyCode == Keys.F5)
                bt_Refresh_Click(sender, e);
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            bool All = false;
            if (sender.ToString() == "حذف کلیه اطلاعات")
                All = true;

            if (this.table_034_SaleFactor_Child3BindingSource.Count > 0)
            {
                gridEX_Cash.Refetch();
                gridEX_Discount.Refetch();
                gridEX_Pay.Refetch();
                gridEX_Return.Refetch();
                gridEX_To.Refetch();
                gridEX_From.Refetch();

                string Message = "آیا مایل به حذف برگه ها، اسناد حسابداری و انتقالات صورت گرفته هستید؟";
                if (All)
                    Message = "آیا مایل به حذف کلیه اطلاعات هستید؟";

                if (DialogResult.Yes == MessageBox.Show(Message,
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RightAlign))
                {
                    //****************CASH
                    foreach (GridEXRow item in gridEX_Cash.GetRows())
                    {
                        if (item.Cells["Column08"].Value.ToString() != "0")
                        {
                            //حذف سند
                            clDoc.DeleteDetail_ID(int.Parse(item.Cells["Column08"].Value.ToString()), 24, int.Parse(item.Cells["Column07"].Value.ToString()));
                            //حذف برگه
                            clDoc.RunSqlCommand(ConBank.ConnectionString, "DELETE FROM Table_045_ReceiveCash where ColumnId=" + item.Cells["Column07"].Value.ToString());

                            item.BeginEdit();
                            item.Cells["Column07"].Value = 0;
                            item.Cells["Column08"].Value = 0;
                            item.EndEdit();

                        }
                        if (All)
                            item.Delete();

                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                        this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                    }

                    //****************CHEQUE
                    foreach (GridEXRow item in gridEX_Pay.GetRows())
                    {
                        if (item.Cells["Column08"].Value.ToString() != "0")
                        {

                            //حذف گردش و اسناد
                            SqlDataAdapter SelectAdapter = new SqlDataAdapter("Select * from Table_065_TurnReception where Column01=" + item.Cells["Column07"].Value.ToString(), ConBank);
                            DataTable TurnRows = new DataTable();
                            SelectAdapter.Fill(TurnRows);
                            if (TurnRows.Rows.Count > 1)
                            {
                                if (DialogResult.Yes == MessageBox.Show("برای این چک علاوه بر گردش دریافت، گردشهای دیگری نیز ثبت شده است" + Environment.NewLine +
                                    "در صورت تأیید تمام گردشها و اسناد مربوطه حذف خواهند شد" + Environment.NewLine + "آیا مایل به ادامه هستید؟", "", MessageBoxButtons.YesNo,
                                     MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                                {
                                    foreach (DataRow Row in TurnRows.Rows)
                                    {
                                        //حذف سند
                                        clDoc.IsFinal_ID(int.Parse(Row["Column13"].ToString()));
                                        //در تاریخ 980327 نوع 28 به وضعیت چک تغییر کرد

                                        //حذف سند اول که با نوع 28 درج می شود
                                        clDoc.DeleteDetail_ID(int.Parse(Row["Column13"].ToString()), Convert.ToInt16(Row["Column02"]), int.Parse(Row["ColumnId"].ToString()));
                                        clDoc.DeleteDetail_ID(int.Parse(Row["Column13"].ToString()), short.Parse(Row["Column02"].ToString()), int.Parse(Row["ColumnId"].ToString()));
                                        clDoc.DeleteTurnReception(long.Parse(Row["ColumnId"].ToString()));
                                    }
                                }
                            }
                            else
                            {
                                foreach (DataRow Row in TurnRows.Rows)
                                {
                                    //در تاریخ 980327 نوع 28 به وضعیت چک تغییر کرد
                                    //حذف سند اول که با نوع 28 درج می شود
                                    clDoc.DeleteDetail_ID(int.Parse(Row["Column13"].ToString()), Convert.ToInt16(Row["Column02"]), int.Parse(Row["ColumnId"].ToString()));
                                    clDoc.IsFinal_ID(int.Parse(Row["Column13"].ToString()));
                                    clDoc.DeleteDetail_ID(int.Parse(Row["Column13"].ToString()), short.Parse(Row["Column02"].ToString()), int.Parse(Row["ColumnId"].ToString()));
                                    clDoc.DeleteTurnReception(long.Parse(Row["ColumnId"].ToString()));
                                }
                            }



                            //حذف برگه
                            clDoc.RunSqlCommand(ConBank.ConnectionString, "DELETE FROM Table_035_ReceiptCheques where ColumnId=" + item.Cells["Column07"].Value.ToString());

                            item.BeginEdit();
                            item.Cells["Column07"].Value = 0;
                            item.Cells["Column08"].Value = 0;
                            item.EndEdit();

                        }
                        if (All)
                            item.Delete();

                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                        this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                    }

                    //****************DISCOUNT
                    foreach (GridEXRow item in gridEX_Discount.GetRows())
                    {
                        if (item.Cells["Column08"].Value.ToString() != "0")
                        {
                            //حذف سند
                            clDoc.DeleteDetail_ID(int.Parse(item.Cells["Column08"].Value.ToString()), 30, int.Parse(item.Cells["ColumnId"].Value.ToString()));

                            item.BeginEdit();
                            item.Cells["Column08"].Value = 0;
                            item.EndEdit();
                        }
                        if (All)
                            item.Delete();

                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                        this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                    }

                    //****************RETURN
                    foreach (GridEXRow item in gridEX_Return.GetRows())
                    {
                        if (item.Cells["Column08"].Value.ToString() != "0")
                        {
                            //حذف رسید انبار
                            int ResidId = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column09", "ColumnId", item.Cells["Column07"].Value.ToString()));
                            clDoc.RunSqlCommand(ConWare.ConnectionString, "DELETE FROM Table_012_Child_PwhrsReceipt Where Column01=" + ResidId);
                            clDoc.RunSqlCommand(ConWare.ConnectionString, "DELETE FROM Table_011_PwhrsReceipt Where ColumnId=" + ResidId);

                            //حذف سند
                            clDoc.DeleteDetail_ID(int.Parse(item.Cells["Column08"].Value.ToString()), 29, int.Parse(item.Cells["Column07"].Value.ToString()));
                            clDoc.DeleteDetail_ID(int.Parse(item.Cells["Column08"].Value.ToString()), 27, ResidId);
                            int PaperId = int.Parse(item.Cells["Column07"].Value.ToString());
                            int DocId = int.Parse(item.Cells["Column08"].Value.ToString());
                            foreach (Janus.Windows.GridEX.GridEXRow item1 in gridEX_Return.GetRows())
                            {
                                if (item1.Cells["Column07"].Value.ToString() == PaperId.ToString() && item1.Cells["Column08"].Value.ToString() == DocId.ToString())
                                {
                                    item1.BeginEdit();
                                    item1.Cells["Column07"].Value = 0;
                                    item1.Cells["Column08"].Value = 0;
                                    item1.EndEdit();
                                }
                            }

                            //حذف فاکتور مرجوعی
                            clDoc.RunSqlCommand(ConSale.ConnectionString, "DELETE FROM Table_019_Child1_MarjooiSale WHERE Column01=" + PaperId);
                            clDoc.RunSqlCommand(ConSale.ConnectionString, "DELETE FROM Table_018_MarjooiSale WHERE ColumnId=" + PaperId);



                            item.BeginEdit();
                            item.Cells["Column07"].Value = 0;
                            item.Cells["Column08"].Value = 0;
                            GoodbindingSource.Filter = "GoodID=" + item.Cells["Column15"].Value.ToString();
                            item.Cells["BoxNumber"].Value = ((DataRowView)GoodbindingSource.CurrencyManager.Current)["BoxNumber"].ToString();
                            item.Cells["PackNumber"].Value = ((DataRowView)GoodbindingSource.CurrencyManager.Current)["PackNumber"].ToString();
                            item.Cells["DetailNumber"].Value = ((DataRowView)GoodbindingSource.CurrencyManager.Current)["DetailNumber"].ToString();
                            item.Cells["tedaddarkartoon"].Value = ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString();
                            item.Cells["tedaddarbaste"].Value = ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString();
                            item.EndEdit();
                        }
                        if (All)
                            item.Delete();

                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                        this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                    }

                    //*********************TRANSFER
                    foreach (GridEXRow item in gridEX_To.GetRows())
                    {

                        if (item.Cells["Column31"].Value.ToString() == "True")
                        {
                            clDoc.RunSqlCommand(ConSale.ConnectionString, "DELETE FROM Table_034_SaleFactor_Child3 where Column01=" + item.Cells["Column07"].Value.ToString() +
                                " and Column03=5 and Column07=" + _SaleFactor);

                            item.BeginEdit();
                            item.Cells["Column31"].Value = false;
                            item.EndEdit();
                        }
                        if (All)
                            item.Delete();
                        this.table_034_SaleFactor_Child3BindingSource.EndEdit();
                        this.table_034_SaleFactor_Child3TableAdapter.Update(dataSet_Sale.Table_034_SaleFactor_Child3);
                    }
                    Class_BasicOperation.ShowMsg("", "حذف برگه ها و اسناد حسابداری با موفقیت صورت گرفت", "Information");
                    RefreshPrices();
                }
            }
        }

        private void bt_CustomerStatus_Click(object sender, EventArgs e)
        {
            string[] Info = new string[4];
            Info = clSettle.CustomerSettleInfo(ConSale, int.Parse(gridEX1.GetRow().Cells["Column03"].Value.ToString()));
            DevComponents.DotNetBar.Balloon b = new DevComponents.DotNetBar.Balloon();
            b.Style = DevComponents.DotNetBar.eBallonStyle.Office2007Alert;
            b.CaptionText = "اطلاعات فاکتورهای مشتری";
            b.CaptionFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            b.CaptionImage = balloonTip1.CaptionImage.Clone() as Image;
            b.Text = Info[0] + Environment.NewLine + Info[1] + Environment.NewLine + Info[2] + Environment.NewLine + Info[3];
            b.AlertAnimation = DevComponents.DotNetBar.eAlertAnimation.TopToBottom;
            b.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            b.AutoResize();
            b.Show(uiButton1, false);
        }

        private void bt_RemoveAll_Click(object sender, EventArgs e)
        {
            bt_Del_Click(sender, e);
        }



        //private void gridEX_Extra_UpdatingCell(object sender, UpdatingCellEventArgs e)
        //{
        //    if (e.Column.Key == "column02")
        //    {

        //        gridEX_Extra.SetValue("column05", (gridEX_Extra.DropDowns["Type"].GetValue("column02")));
        //        gridEX_Extra.SetValue("column04", "0");
        //        gridEX_Extra.SetValue("column03", "0");

        //        if (gridEX_Extra.DropDowns["Type"].GetValue("column03").ToString() == "True")
        //        {
        //            gridEX_Extra.SetValue("column04", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
        //        }
        //        else
        //        {

        //            gridEX_Extra.SetValue("column03", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
        //            Double darsad;
        //            darsad = Convert.ToDouble(gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());

        //            Double kol;
        //            GridEXFilterCondition filter = new GridEXFilterCondition(gridEX_Return.RootTable.Columns["Column08"],
        //                 ConditionOperator.Equal, 0);
        //            kol = Convert.ToDouble(gridEX_Return.GetTotal(gridEX_Return.RootTable.Columns["Column29"]
        //                , AggregateFunction.Sum, filter).ToString());
        //            if (kol == 0)
        //                return;
        //            gridEX_Extra.SetValue("column04", kol * darsad / 100);
        //        }
        //    }
        //    else if (e.Column.Key == "column03")
        //    {
        //        Double darsad;
        //        darsad = Convert.ToDouble(e.Value.ToString());
        //        Double kol;
        //        GridEXFilterCondition filter = new GridEXFilterCondition(gridEX_Return.RootTable.Columns["Column08"],
        //               ConditionOperator.Equal, 0);
        //        kol = Convert.ToDouble(gridEX_Return.GetTotal(gridEX_Return.RootTable.Columns["Column29"]
        //                , AggregateFunction.Sum, filter).ToString());
        //        if (kol == 0)
        //            return;
        //        gridEX_Extra.SetValue("column04", kol * darsad / 100);
        //    }
        //}





    }
}
