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
    public partial class Frm_029_CloseCash_Edit : Form
    {
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Class_UserScope UserScope = new Class_UserScope();



        DataTable CheckTable = new DataTable();
        DataTable CheckHavaleTable = new DataTable();
        DataTable otherCheckHavaleTable = new DataTable();

        DataTable waredt = new DataTable();
        DataTable discountdt = new DataTable();
        DataTable taxdt = new DataTable();
        DataTable factordt = new DataTable();
        DataTable Sanaddt = new DataTable();
        DataTable iddt = new DataTable();
        DataTable bahaDT = new DataTable();
        string date = string.Empty;
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool Isadmin = false;
        DataTable storefactor = new DataTable();
        Int16 projectId = -100;

        int Num;

        public Frm_029_CloseCash_Edit(int _Num)
        {
            InitializeComponent();
            Num = _Num;
        }


        private void Frm_029_CloseCash_Load(object sender, EventArgs e)
        {


            this.WindowState = FormWindowState.Normal;


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




            gx_recivefromcustomer.AllowEdit = InheritableBoolean.True;
            gx_factors.AllowEdit = InheritableBoolean.True;
            gx_losefromcash.AllowEdit = InheritableBoolean.True;

            DataTable Person = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_045_PersonInfo where Column12=1");
            this.gx_recivefromcustomer.DropDowns[0].SetDataBinding(Person, "");

            DataTable Person2 = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_045_PersonInfo where Column12=1");
            this.gx_losefromcash.DropDowns["Person"].SetDataBinding(Person2, "");


            DataTable ACCINfo = clDoc.ReturnTable(Properties.Settings.Default.BANK, "Select ColumnId,Column01,Column02,isnull(Column12,'') as Column12 from Table_020_BankCashAccInfo where Column01=0");
            this.gx_recivefrombank.DropDowns[0].SetDataBinding(ACCINfo, "");


            DataTable ACC = clDoc.ReturnTable(Properties.Settings.Default.ACNT, "SELECT ACC_Code,ACC_Name from AllHeaders()");
            this.gx_losefromcash.DropDowns["ACC"].SetDataBinding(ACC, "");

            mlt_Doc.DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, @" select ColumnId,Column00 from  Table_060_SanadHead");
            DataTable CustomerTable = clDoc.ReturnTable
            (ConBase.ConnectionString, @"select [ColumnId]
                                                              ,[Column00]
                                                              ,[Column01]
                                                              ,[Column02]
                                    FROM   dbo.Table_045_PersonInfo
                                            
                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");
            gx_factors.DropDowns[3].SetDataBinding(CustomerTable, "");

            DataTable saletype = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_002_SalesTypes");
            gx_factors.DropDowns["saletype"].SetDataBinding(saletype, "");


            DataTable sanad = clDoc.ReturnTable(ConAcnt.ConnectionString, "select * from Table_060_SanadHead");
            gx_factors.DropDowns["sanad"].SetDataBinding(sanad, "");



            DataTable draft = clDoc.ReturnTable(ConWare.ConnectionString, "select * from Table_007_PwhrsDraft");
            gx_factors.DropDowns["draft"].SetDataBinding(draft, "");

            gx_losefromcash.DropDowns["Numsanad"].DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, @"select * from Table_060_SanadHead");
            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            searech();
            bt_Print.Enabled = false;

        }

        private float FirstRemain(int GoodCode, string ware, string date, int? drafid)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                string CommandText = string.Empty;
                if (drafid == null)
                    CommandText = @"  SELECT *
           FROM   (
                      SELECT ISNULL(
                                 (
                                     SELECT SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS 
                                            InValue
                                     FROM   dbo.Table_011_PwhrsReceipt
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt
                                                 ON  dbo.Table_011_PwhrsReceipt.columnid = 
                                                     dbo.Table_012_Child_PwhrsReceipt.column01
                                     WHERE  (dbo.Table_011_PwhrsReceipt.column03 = {0})
                                            AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                                            AND dbo.Table_011_PwhrsReceipt.column02 
                                                <= '{2}'
                                     GROUP BY
                                            dbo.Table_012_Child_PwhrsReceipt.column02
                                 ),
                                 0
                             ) AS InValue,
                             (
                                 SELECT ISNULL(
                                            (
                                                SELECT ISNULL(SUM(dbo.Table_008_Child_PwhrsDraft.column07), 0) AS 
                                                       OutValue
                                                FROM   dbo.Table_007_PwhrsDraft
                                                       INNER JOIN dbo.Table_008_Child_PwhrsDraft
                                                            ON  dbo.Table_007_PwhrsDraft.columnid = 
                                                                dbo.Table_008_Child_PwhrsDraft.column01
                                                WHERE  (dbo.Table_007_PwhrsDraft.column03 = {0})
                                                       AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                                                       AND dbo.Table_007_PwhrsDraft.column02 
                                                           <= '{2}'
                                                GROUP BY
                                                       dbo.Table_008_Child_PwhrsDraft.column02
                                            ),
                                            0
                                        )
                             ) AS OutValue
                  ) AS f
       ) AS j";
                else
                    CommandText = @"SELECT j.InValue -j.OutValue AS Remain
FROM   (
           SELECT *
           FROM   (
                      SELECT ISNULL(
                                 (
                                     SELECT SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS 
                                            InValue
                                     FROM   dbo.Table_011_PwhrsReceipt
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt
                                                 ON  dbo.Table_011_PwhrsReceipt.columnid = 
                                                     dbo.Table_012_Child_PwhrsReceipt.column01
                                     WHERE  (dbo.Table_011_PwhrsReceipt.column03 = {0})
                                            AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                                            AND dbo.Table_011_PwhrsReceipt.column02 
                                                <= '{2}'
                                     GROUP BY
                                            dbo.Table_012_Child_PwhrsReceipt.column02
                                 ),
                                 0
                             ) AS InValue,
                             (
                                 SELECT ISNULL(
                                            (
                                                SELECT ISNULL(SUM(dbo.Table_008_Child_PwhrsDraft.column07), 0) AS 
                                                       OutValue
                                                FROM   dbo.Table_007_PwhrsDraft
                                                       INNER JOIN dbo.Table_008_Child_PwhrsDraft
                                                            ON  dbo.Table_007_PwhrsDraft.columnid = 
                                                                dbo.Table_008_Child_PwhrsDraft.column01
                                                WHERE  (dbo.Table_007_PwhrsDraft.column03 = {0})
                                                       AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                                                       AND dbo.Table_007_PwhrsDraft.column02 
                                                           <= '{2}' and dbo.Table_007_PwhrsDraft.columnid!=" + drafid + @"
                                                GROUP BY
                                                       dbo.Table_008_Child_PwhrsDraft.column02
                                            ),
                                            0
                                        )
                             ) AS OutValue
                  ) AS f
       ) AS j";
                CommandText = string.Format(CommandText, ware, GoodCode, date);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        /*
        private float FirstRemain(int GoodCode, string ware, string date, int? drafid)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                string CommandText = string.Empty;
                if (drafid == null)
                    CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
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
                else
                    CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
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
                       WHERE     (dbo.Table_007_PwhrsDraft.column03 = {0}) AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1}) and dbo.Table_007_PwhrsDraft.columnid!=" + drafid + @"
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02) AS derivedtbl_1
                       WHERE     (Date <= '{2}')";
                CommandText = string.Format(CommandText, ware, GoodCode, date);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }
       */


        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            //bt_Search.Enabled =
            bt_ExportDoc.Enabled = false;
            DataTable NotExist = new DataTable();

            try
            {
                //if (gx_factors.GetRows().Length > 0)
                //{



                chehckessentioal();








                #region بستن صندوق

                gx_recivefrombank.UpdateData();
                gx_recivefromcustomer.UpdateData();
                gx_losefromcash.UpdateData();


                decimal customerrecive1 = Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum));
                decimal customerrecive = Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
                decimal losefromcash = Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["Column07"] = customerrecive1;
                ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["Column12"] = customerrecive;
                ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["Column11"] = losefromcash;

                txt_CashStockSum.Value = Convert.ToDecimal(txt_CashSale.Value) + customerrecive1;
                decimal temp = 0;
                temp = (Convert.ToDecimal(txt_CashCashStock.Value) + customerrecive) + losefromcash;
                txt_CashRealStock.Value = temp;
                txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashStockSum.Value) - temp;
                txt_CashAcountDifference.Value = Convert.ToDecimal(txt_CashAcountRemain.Value) - customerrecive - losefromcash - Convert.ToDecimal(txt_CashDeductionAddition.Value);



                table_96_CloseCashBindingSource.EndEdit();
                table_97_ReceivedFromCustomersBindingSource.EndEdit();
                table_98_ReceivedFromBankBindingSource.EndEdit();
                table_99_LosesFromCashBindingSource.EndEdit();




                #endregion



                #region بستن صندوق

                string closcmd = "";
                bool exsixt = false;
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Select = new SqlCommand(@"IF EXISTS (
                                                               SELECT *
                                                               FROM   Table_060_SanadHead AS tsh
                                                               WHERE  tsh.ColumnId = " + mlt_Doc.Value + @"
                                                           )
                                                            SELECT 1 AS e
                                                        ELSE
                                                            SELECT 0 AS e", Con);
                    exsixt = Convert.ToBoolean(Select.ExecuteScalar());
                }

                if (!exsixt)
                {

                    if (DialogResult.Yes == MessageBox.Show("سند بستن صندوق جاری وجود ندارد، آیا مایل به صدور سند جدید هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
                    {
                        closcmd = @" Declare @DocId int 
                        INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                    VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + date + "',2,0,' صدور سند بستن صندوق شماره " + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + "','" + Class_BasicOperation._UserName +
                                "',getdate()); SET @DocID=SCOPE_IDENTITY()";
                    }
                    else
                    {
                        clDoc.RunSqlCommand(Properties.Settings.Default.SALE, "update Table_96_CloseCash set Column16 =0 where ColumnId=" + Num);
                        this.table_96_CloseCashTableAdapter.Update(this.dataSet6.Table_96_CloseCash);
                        this.table_99_LosesFromCashTableAdapter.Update(this.dataSet6.Table_99_LosesFromCash);
                        this.table_98_ReceivedFromBankTableAdapter.Update(this.dataSet6.Table_98_ReceivedFromBank);
                        this.table_97_ReceivedFromCustomersTableAdapter.Update(this.dataSet6.Table_97_ReceivedFromCustomers);
                        searech();
                        table_96_CloseCashBindingSource_PositionChanged(null, null);
                        // bt_Search.Enabled =
                        bt_ExportDoc.Enabled = true;
                        Class_BasicOperation.ShowMsg("", "ویرایش بستن صندوق انجام شد ولی سند ثبت نشد", "Warning");
                        return;
                    }
                }

                else
                    closcmd = @" Declare @DocId int
                            set  @DocId=" + mlt_Doc.Value +
                                " Delete from Table_065_SanadDetail where Column00=" + mlt_Doc.Value + " AND Column16=300 ANd Column17=" + txt_Id.Text;
                foreach (Janus.Windows.GridEX.GridEXRow item in gx_losefromcash.GetDataRows())
                {

                    if (Convert.ToDouble(item.Cells["Column04"].Value) > 0)
                    {
                        string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column02"].Value.ToString());

                        closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + item.Cells["Column02"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (item.Cells["Column03"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["Column03"].Value.ToString()) ? (item.Cells["Column03"].Value.ToString()) : "NULL") + @", NULL , " + (projectId != -100 ? projectId : (Int16?)null) + @" ,
                   " + "N'بابت واریز/برداشت روزانه از صندوق'," + Convert.ToInt64(item.Cells["Column04"].Value) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                    }
                }

                foreach (Janus.Windows.GridEX.GridEXRow item in gx_recivefrombank.GetDataRows())
                {
                    if (Convert.ToDouble(item.Cells["Column03"].Value) > 0)
                    {
                        string acc = clDoc.ExScalarQuery(ConBank.ConnectionString, "select Column12 from Table_020_BankCashAccInfo where ColumnId=" + item.Cells["Column02"].Value);
                        //item.Cells["Column02"].Column.DropDown.GetValue("Column12").ToString();

                        string[] _AccInfo = clDoc.ACC_Info(acc);

                        closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + acc + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId : (Int16?)null) + @" ,
                   " + "N'واریز/برداشت روزانه صندوق'," + Convert.ToInt64(item.Cells["Column03"].Value) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                    }
                }
                customerrecive = Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
                losefromcash = Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                if (customerrecive + losefromcash > 0)
                {

                    DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");

                    string[] _AccInfo = clDoc.ACC_Info(acc.Rows[0]["column08"].ToString());

                    closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId : (Int16?)null) + @" ,
                   " + "N'بابت واریز/برداشت روزانه از صندوق' " + ",0," + Convert.ToInt64(customerrecive + losefromcash) + @",0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                }
                DataTable ddt = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT isnull(tst.Column02,'') as bed,isnull(tst.Column02,'') as bes
                                                                                                                FROM  Table_030_Setting tst
                                                                                                                WHERE  tst.ColumnId = 64");
                decimal CashDeductionAddition = Convert.ToDecimal(txt_CashDeductionAddition.Value);
                if (CashDeductionAddition != Convert.ToDecimal(0))
                {
                    DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");



                    if (CashDeductionAddition < Convert.ToDecimal(0))
                    {


                        string[] _AccInfo = clDoc.ACC_Info(acc.Rows[0]["column08"].ToString());

                        closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId : (Int16?)null) + @" ,
                   " + "N'بابت کسر و اضافه صندوق'," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                        _AccInfo = clDoc.ACC_Info((ddt.Rows[0]["bes"].ToString()));

                        closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + ddt.Rows[0]["bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId : (Int16?)null) + @" ,
                   " + "N'بابت کسر و اضافه صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";




                    }
                    else
                    {


                        string[] _AccInfo = clDoc.ACC_Info(ddt.Rows[0]["bed"].ToString());

                        closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + ddt.Rows[0]["bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId : (Int16?)null) + @" ,
                   " + "N'بابت کسر و اضافه صندوق'," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                        _AccInfo = clDoc.ACC_Info((acc.Rows[0]["column08"].ToString()));

                        closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId : (Int16?)null) + @" ,
                   " + "N'بابت کسر و اضافه صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                    }

                }
                decimal CashAcountDifference = Convert.ToDecimal(txt_CashAcountDifference.Value);
                if (CashAcountDifference != Convert.ToDecimal(0))
                {
                    DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");



                    if (CashAcountDifference < Convert.ToDecimal(0))
                    {


                        string[] _AccInfo = clDoc.ACC_Info(ddt.Rows[0]["bed"].ToString());

                        closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + ddt.Rows[0]["bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId : (Int16?)null) + @" ,
                   " + "N'بابت اختلاف حساب صندوق'," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                        _AccInfo = clDoc.ACC_Info((acc.Rows[0]["column08"].ToString()));

                        closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId : (Int16?)null) + @" ,
                   " + "N'بابت اختلاف حساب صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                    }
                    else
                    {



                        string[] _AccInfo = clDoc.ACC_Info(acc.Rows[0]["column08"].ToString());

                        closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId : (Int16?)null) + @" ,
                   " + "N'بابت اختلاف حساب صندوق'," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                        _AccInfo = clDoc.ACC_Info((ddt.Rows[0]["bes"].ToString()));

                        closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + ddt.Rows[0]["bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId : (Int16?)null) + @" ,
                   " + "N'بابت اختلاف حساب صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                    }

                }
                closcmd += " update " + ConSale.Database + ".dbo.Table_96_CloseCash set Column16 =@DocID where ColumnId=" + Num;

                using (SqlConnection Con1 = new SqlConnection(Properties.Settings.Default.ACNT))
                {


                    Con1.Open();
                    SqlTransaction sqlTran1 = Con1.BeginTransaction();
                    SqlCommand Command1 = Con1.CreateCommand();
                    Command1.Transaction = sqlTran1;
                    try
                    {

                        Command1.CommandText = closcmd;
                        Command1.ExecuteNonQuery();
                        sqlTran1.Commit();
                        this.table_96_CloseCashTableAdapter.Update(this.dataSet6.Table_96_CloseCash);
                        this.table_99_LosesFromCashTableAdapter.Update(this.dataSet6.Table_99_LosesFromCash);
                        this.table_98_ReceivedFromBankTableAdapter.Update(this.dataSet6.Table_98_ReceivedFromBank);
                        this.table_97_ReceivedFromCustomersTableAdapter.Update(this.dataSet6.Table_97_ReceivedFromCustomers);
                        mlt_Doc.DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, @" select ColumnId,Column00 from  Table_060_SanadHead");
                        searech();
                        table_96_CloseCashBindingSource_PositionChanged(null, null);
                        MessageBox.Show("عملیات ویرایش صندوق به شماره سند " + mlt_Doc.Text + " با موفقیت انجام شد");



                    }
                    catch (Exception es)
                    {
                        sqlTran1.Rollback();
                        this.Cursor = Cursors.Default;
                        Class_BasicOperation.CheckExceptionType(es, this.Name);

                    }
                }
                //}
                //catch (Exception es)
                //{
                //    sqlTran.Rollback();
                //    this.Cursor = Cursors.Default;
                //    Class_BasicOperation.CheckExceptionType(es, this.Name);
                //}
                #endregion

                this.Cursor = Cursors.Default;
                bt_Print.Enabled = true;




            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }

            // bt_Search.Enabled =
            bt_ExportDoc.Enabled = true;


        }

        public void bt_Search_Click(object sender, EventArgs e)
        {

            #region searchnumber

            table_96_CloseCashBindingSource.EndEdit();
            table_97_ReceivedFromCustomersBindingSource.EndEdit();
            table_98_ReceivedFromBankBindingSource.EndEdit();
            table_99_LosesFromCashBindingSource.EndEdit();

            dataSet6.EnforceConstraints = false;
            table_96_CloseCashTableAdapter.FillByID(dataSet6.Table_96_CloseCash, int.Parse(txt_Search.Text));
            if (table_96_CloseCashBindingSource.Count > 0)
            {

                table_97_ReceivedFromCustomersTableAdapter.FillByHeaderID(dataSet6.Table_97_ReceivedFromCustomers, int.Parse(txt_Search.Text));
                table_98_ReceivedFromBankTableAdapter.FillByHeaderID(dataSet6.Table_98_ReceivedFromBank, int.Parse(txt_Search.Text));
                table_99_LosesFromCashTableAdapter.FillByHeaderID(dataSet6.Table_99_LosesFromCash, int.Parse(txt_Search.Text));
                dataSet6.EnforceConstraints = true;


            }

            else
            {
                MessageBox.Show("این شماره صندوق موجود نمی باشد ");
            }



            #endregion






        }

        public void searech()
        {
            DataTable allfactor = new DataTable();

            table_96_CloseCashBindingSource.EndEdit();
            table_97_ReceivedFromCustomersBindingSource.EndEdit();
            table_98_ReceivedFromBankBindingSource.EndEdit();
            table_99_LosesFromCashBindingSource.EndEdit();
            dataSet6.EnforceConstraints = false;
            table_96_CloseCashTableAdapter.FillByID(dataSet6.Table_96_CloseCash, Num);
            if (table_96_CloseCashBindingSource.Count > 0)
            {

                table_97_ReceivedFromCustomersTableAdapter.FillByHeaderID(dataSet6.Table_97_ReceivedFromCustomers, Num);
                table_98_ReceivedFromBankTableAdapter.FillByHeaderID(dataSet6.Table_98_ReceivedFromBank, Num);
                table_99_LosesFromCashTableAdapter.FillByHeaderID(dataSet6.Table_99_LosesFromCash, Num);
                dataSet6.EnforceConstraints = true;

                txt_Search.Text = Num.ToString();
                date = ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["Column01"].ToString();

                SqlDataAdapter Adapter = new SqlDataAdapter(
                                                  @"SELECT        columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, 
                         column15, column16, column17, column18, column19, column20, column21, column23, column24, column25, column27, Column37, Column38, Column39, Column40, 
                         Column42, Column43, Column44, Column45, Column49, Column50, Column51, Column53, Column54, Column55, Column56, Column57, Column58, Column59, 
                         Column61, Column62, Column63, Column60, Column64, column22, column26, Column28, Column29, Column30, Column31, Column32, Column33, Column34, 
                         Column35, Column36, Column41, Column46, Column47, Column48, Column52,Column28 - Column29 -Column30 - Column31 + Column32- Column33 as FinalPrice
                                FROM            Table_010_SaleFactor
                                WHERE   Column67=" + Num, ConSale);
                Adapter.Fill(allfactor);
                gx_factors.DataSource = allfactor;


            }

            else
            {
                MessageBox.Show("این شماره صندوق موجود نمی باشد ");
            }

        }

        private void Frm_029_CloseCash_FormClosing(object sender, FormClosingEventArgs e)
        {
            gx_factors.RemoveFilters();

        }

        private void chehckessentioal()
        {


            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));
            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));
            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

            #region بستن صندوق

            foreach (Janus.Windows.GridEX.GridEXRow item in gx_recivefrombank.GetDataRows())
            {
                //string acc = item.Cells["Column02"].Column.DropDown.GetValue("Column12").ToString();
                string acc = clDoc.ExScalarQuery(ConBank.ConnectionString, "select isnull(Column12,'') as Column12 from Table_020_BankCashAccInfo where ColumnId=" + item.Cells["Column02"].Value);

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + acc + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("سرفصل معتبر در حساب های بانکی  وارد کنید");
                }



            }
            DataTable SalesTypes = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");
            foreach (DataRow dr1 in SalesTypes.Rows)
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + dr1["column08"] + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("سرفصل معتبر در انواع فروش وارد کنید");
                }
            }
            DataTable ddt = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT isnull(tst.Column02,'') as bed,isnull(tst.Column02,'') as bes
                                                                                                                FROM  Table_030_Setting tst
                                                                                                                WHERE  tst.ColumnId = 64");
            if (ddt.Rows.Count == 1)
            {

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + ddt.Rows[0]["bed"] + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("سرفصل معتبر برای کسر/اضافه صندوق در تنطیمات وارد کنید");




                }

            }
            else
                throw new Exception("سرفصل کسر و اضافه صندوق تعریف نشده است");

            if (string.IsNullOrWhiteSpace(date))
                throw new Exception("تاتاریخ را وارد کنید");


            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(date);
            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

            TPerson.Rows.Clear();
            TAccounts.Rows.Clear();
            foreach (Janus.Windows.GridEX.GridEXRow item in gx_losefromcash.GetDataRows())
            {
                if (item.Cells["Column03"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["Column03"].Value.ToString()))
                {
                    All_Controls_Row1(item.Cells["Column02"].Value.ToString(), 1, null, (projectId != -100 ? projectId : (Int16?)null));
                    TPerson.Rows.Add(Int32.Parse(item.Cells["Column03"].Value.ToString()), item.Cells["Column02"].Value, Convert.ToDouble(item.Cells["Column04"].Value));

                }
                else
                    All_Controls_Row1(item.Cells["Column02"].Value.ToString(), null, null, (projectId != -100 ? projectId : (Int16?)null));

                TAccounts.Rows.Add(item.Cells["Column02"].Value, (Convert.ToDouble(item.Cells["Column04"].Value)));


            }

            foreach (Janus.Windows.GridEX.GridEXRow item in gx_recivefrombank.GetDataRows())
            {
                //string acc = item.Cells["Column02"].Column.DropDown.GetValue("Column12").ToString();
                string acc = clDoc.ExScalarQuery(ConBank.ConnectionString, "select Column12 from Table_020_BankCashAccInfo where ColumnId=" + item.Cells["Column02"].Value);

                All_Controls_Row1(acc, null, null, (projectId != -100 ? projectId : (Int16?)null));
                TAccounts.Rows.Add(acc, (Convert.ToDouble(item.Cells["Column03"].Value)));

            }

            decimal customerrecive = Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
            decimal losefromcash = Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

            if (customerrecive + losefromcash > 0)
            {

                DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");

                if (acc.Rows.Count != 1)
                    throw new Exception("سرفصل نوع فروش نقدی تعریف نشده است");


                All_Controls_Row1(acc.Rows[0]["column08"].ToString(), null, null, (projectId != -100 ? projectId : (Int16?)null));

                TAccounts.Rows.Add(acc.Rows[0]["column08"].ToString(), (-1 * Convert.ToDouble(customerrecive + losefromcash)));

            }
            decimal CashDeductionAddition = Convert.ToDecimal(txt_CashDeductionAddition.Value);
            if (CashDeductionAddition != Convert.ToDecimal(0))
            {
                DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");

                if (acc.Rows.Count != 1)
                    throw new Exception("سرفصل نوع فروش نقدی تعریف نشده است");

                if (CashDeductionAddition < Convert.ToDecimal(0))
                {
                    TAccounts.Rows.Add(acc.Rows[0]["column08"].ToString(), (Convert.ToDouble(Math.Abs(CashDeductionAddition))));
                    TAccounts.Rows.Add(ddt.Rows[0]["bes"], (-1 * Convert.ToDouble(Math.Abs(CashDeductionAddition))));

                    All_Controls_Row1(ddt.Rows[0]["bes"].ToString(), null, null, (projectId != -100 ? projectId : (Int16?)null));
                    All_Controls_Row1(acc.Rows[0]["column08"].ToString(), null, null, (projectId != -100 ? projectId : (Int16?)null));

                }
                else
                {
                    TAccounts.Rows.Add(ddt.Rows[0]["bed"], (Convert.ToDouble(CashDeductionAddition)));
                    TAccounts.Rows.Add(acc.Rows[0]["column08"].ToString(), (-1 * Convert.ToDouble(CashDeductionAddition)));
                    All_Controls_Row1(ddt.Rows[0]["bed"].ToString(), null, null, (projectId != -100 ? projectId : (Int16?)null));


                }

            }
            decimal CashAcountDifference = Convert.ToDecimal(txt_CashAcountDifference.Value);
            if (CashAcountDifference != Convert.ToDecimal(0))
            {
                DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");

                if (acc.Rows.Count != 1)
                    throw new Exception("سرفصل نوع فروش نقدی تعریف نشده است");

                if (CashAcountDifference < Convert.ToDecimal(0))
                {
                    TAccounts.Rows.Add(acc.Rows[0]["column08"].ToString(), (-1 * Convert.ToDouble(Math.Abs(CashAcountDifference))));
                    TAccounts.Rows.Add(ddt.Rows[0]["bed"], (Convert.ToDouble(Math.Abs(CashAcountDifference))));

                }
                else
                {
                    TAccounts.Rows.Add(acc.Rows[0]["column08"].ToString(), (Convert.ToDouble(CashAcountDifference)));
                    TAccounts.Rows.Add(ddt.Rows[0]["bes"], (-1 * Convert.ToDouble(CashAcountDifference)));

                }

            }


            Classes.CheckCredits clCredit1 = new Classes.CheckCredits();
            clCredit1.CheckAccountCredit(TAccounts, 0);
            clCredit1.CheckPersonCredit(TPerson, 0);

            // clDoc.IsValidNumber(int.Parse(mlt_Doc.Text));
            clDoc.IsFinal(int.Parse(mlt_Doc.Text));



            #endregion





        }
        public void All_Controls_Row1(string AccountCode, int? Person, Int16? Center, Int16? Project)
        {

            //*********Control Person
            if (AccHasPerson(AccountCode) == 1)
            {
                if (Person == null)
                {

                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasPerson(AccountCode) == 0)
            {
                if (Person != null)
                {

                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************ Control Center
            if (AccHasCenter(AccountCode) == 1)
            {
                if (Center == null)
                {

                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasCenter(AccountCode) == 0)
            {
                if (Center != null)
                {

                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************* Control Project
            if (AccHasProject(AccountCode) == 1)
            {
                if (Project == null)
                {

                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasProject(AccountCode) == 0)
            {
                if (Project != null)
                {

                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
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

        private void Frm_029_CloseCash_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control && bt_ExportDoc.Enabled)
                bt_ExportDoc_Click(sender, e);
            //else if (e.KeyCode == Keys.F && bt_Search.Enabled)
            //{
            //    bt_Search_Click(sender, e);
            //}

            else if (e.KeyCode == Keys.P && bt_Print.Enabled)
            {
                bt_Print_Click(sender, e);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                _05_Sale.Frm_003_MojoodiMaghtaiTedadi frm1 = new Frm_003_MojoodiMaghtaiTedadi();
                frm1.ShowDialog();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gx_recivefromcustomer_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gx_recivefromcustomer.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "Column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column02", "Column01", "Column02", gx_recivefromcustomer.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }



        }

        private void gx_recivefromcustomer_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gx_recivefromcustomer_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column02");
            }
            catch { }
            try
            {
                if (e.Column.Key == "Column02")
                    gx_recivefromcustomer.SetValue("Person", gx_recivefromcustomer.CurrentRow.Cells["Column02"].Text);
            }
            catch
            {
            }
            try
            {
                if (e.Column.Key == "Column03")
                {
                    gx_recivefromcustomer.UpdateData();
                    decimal customerrecive = Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum));

                    txt_CashStockSum.Value = Convert.ToDecimal(txt_CashSale.Value) + customerrecive;
                }
            }
            catch
            {
            }
        }

        private void gx_recivefrombank_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gx_recivefrombank.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "Column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column02", "Column02", "Column02", gx_recivefrombank.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }
        }

        private void gx_losefromcash_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gx_losefromcash.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "Column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column02", "ACC_Code", "ACC_Name", gx_losefromcash.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }

            try
            {
                if (e.Column.Key == "Column03")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column03", "Column01", "Column02", gx_losefromcash.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }
            //try
            //{
            //    if (e.Column.Key == "Column07")
            //        Class_BasicOperation.FilterGridExDropDown(sender, "Column00", "Column01", "Column02", gx_losefromcash.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            //}
            //catch { }

        }

        private void gx_losefromcash_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column02");
            }
            catch { }
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column03");
            }
            catch { }
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column07");
            }
            catch { }
            try
            {
                if (e.Column.Key == "Column02")
                    gx_losefromcash.SetValue("ACC_Name", gx_losefromcash.CurrentRow.Cells["Column02"].Text);
            }
            catch
            {
            }

            try
            {
                if (e.Column.Key == "Column03")
                    gx_losefromcash.SetValue("Person", gx_losefromcash.CurrentRow.Cells["Column03"].Text);
            }
            catch
            {
            }


            try
            {
                if (e.Column.Key == "Column04" && e.Column.Key == "Column08")
                {
                    calcut();
                }
            }

            catch
            {
            }


            //try
            //{
            //    if (e.Column.Key == "Column07")
            //        gx_losefromcash.SetValue("Numsanad", gx_losefromcash.CurrentRow.Cells["Column07"].Text);
            //}
            //catch
            //{
            //}
        }



        private void gx_recivefromcustomer_Enter(object sender, EventArgs e)
        {
            try
            {
                table_96_CloseCashBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gx_recivefromcustomer_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                decimal customerrecive = Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum));

                txt_CashStockSum.Value = Convert.ToDecimal(txt_CashSale.Value) + customerrecive;
            }
            catch
            {
            }
        }

        private void gx_recivefromcustomer_AddingRecord(object sender, CancelEventArgs e)
        {
            gx_recivefromcustomer.SetValue("Column04", Class_BasicOperation._UserName);
            gx_recivefromcustomer.SetValue("Column05", Class_BasicOperation.ServerDate());


        }

        private void gx_recivefromcustomer_RegionChanged(object sender, EventArgs e)
        {

        }

        private void gx_recivefrombank_AddingRecord(object sender, CancelEventArgs e)
        {
            gx_recivefrombank.SetValue("Column04", Class_BasicOperation._UserName);
            gx_recivefrombank.SetValue("Column05", Class_BasicOperation.ServerDate());

        }

        private void gx_recivefrombank_RowCountChanged(object sender, EventArgs e)
        {
            calcut();
        }

        private void calcut()
        {
            try
            {
                gx_recivefrombank.UpdateData();
                gx_losefromcash.UpdateData();
                decimal customerrecive = Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
                decimal losefromcash = Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                decimal temp = 0;
                temp = (Convert.ToDecimal(txt_CashCashStock.Value) + customerrecive) + losefromcash;
                txt_CashRealStock.Value = temp;
                txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashStockSum.Value) - temp;
                // txt_CashAcountDifference.Value = temp - Convert.ToDecimal(txt_CashAcountRemain.Value);
                txt_CashAcountDifference.Value = Convert.ToDecimal(txt_CashAcountRemain.Value) - customerrecive - losefromcash - Convert.ToDecimal(txt_CashDeductionAddition.Value);

            }
            catch
            {
            }
        }

        private void gx_recivefrombank_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column02");
            }
            catch { }

            try
            {
                if (e.Column.Key == "Column02")
                    gx_recivefrombank.SetValue("Bank", gx_recivefrombank.CurrentRow.Cells["Column02"].Text);
            }
            catch
            {
            }


            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column02");
                calcut();

            }
            catch { }


        }

        private void gx_losefromcash_RowCountChanged(object sender, EventArgs e)
        {
            calcut();

        }

        private void txt_CashCashStock_TextChanged(object sender, EventArgs e)
        {
            calcut();

        }

        private void gx_losefromcash_AddingRecord(object sender, CancelEventArgs e)
        {
            gx_losefromcash.SetValue("Column05", Class_BasicOperation._UserName);
            gx_losefromcash.SetValue("Column06", Class_BasicOperation.ServerDate());

        }

        private void txt_CashStockSum_TextChanged(object sender, EventArgs e)
        {
            calcut();

        }

        private void txt_CashAcountRemain_TextChanged(object sender, EventArgs e)
        {
            calcut();

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {

                DataRowView Row = (DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current;

                DataTable CloseCash = clDoc.ReturnTable(ConSale.ConnectionString, "select * from Table_96_CloseCash where ColumnId=" + Row["ColumnId"]);

                DataTable ReceivedFromCustomers = clDoc.ReturnTable(ConSale.ConnectionString, @"select h.*,j.column02 as Person from Table_97_ReceivedFromCustomers h
                                                                                            join " + ConBase.Database + @".dbo.Table_045_PersonInfo j on h.Column02=j.columnid
                                                                                        where h.Column01=" + Row["ColumnId"]);
                DataTable ReceivedFromBank = clDoc.ReturnTable(ConSale.ConnectionString, @"select h.*,j.column02 as Bank from Table_98_ReceivedFromBank h
                                                join " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo j on j.columnid=h.column02 where h.Column01=" + Row["ColumnId"]);


                DataTable LosesFromCash = clDoc.ReturnTable(ConSale.ConnectionString, @"select h.*,j.column02 as Person,k.ACC_Name from Table_99_LosesFromCash h left join
                                                                                           " + ConBase.Database + @".dbo.Table_045_PersonInfo j on h.Column03=j.columnid 
                                                                                            join " + ConAcnt.Database + @".dbo.AllHeaders() k on h.Column02=k.ACC_Code    
                                                                                            where h.Column01=" + Row["ColumnId"]);


                string numsanad = clDoc.ExScalar(ConSale.ConnectionString, @"  SELECT        " + ConAcnt.Database + @".dbo.Table_060_SanadHead.Column00
FROM            dbo.Table_96_CloseCash INNER JOIN
                         " + ConAcnt.Database + @".dbo.Table_060_SanadHead ON dbo.Table_96_CloseCash.Column16 = " + ConAcnt.Database + @".dbo.Table_060_SanadHead.ColumnId
WHERE        (dbo.Table_96_CloseCash.ColumnId = " + txt_Id.Text + ")");

                _05_Sale.Frm_030_CloseCashPrint frm =
                    new Frm_030_CloseCashPrint("", CloseCash,
                        ReceivedFromCustomers,
                        ReceivedFromBank,
                        LosesFromCash, numsanad);

                frm.ShowDialog();
            }
            catch (Exception es)
            {

                this.Cursor = Cursors.Default;
                Class_BasicOperation.CheckExceptionType(es, this.Name);
            }


        }

        private void table_96_CloseCashBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (table_96_CloseCashBindingSource.Count > 0)
            {
                DataRowView Row = (DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current;

                if (Convert.ToInt32(Row["ColumnId"]) > 0)
                    bt_Print.Enabled = true;
                //else
                //    bt_Print.Enabled = false;

            }
            else
                bt_Print.Enabled = false;

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                _05_Sale.Frm_003_MojoodiMaghtai_NegativeDate frm2 = new Frm_003_MojoodiMaghtai_NegativeDate();
                frm2.ShowDialog();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void faDatePickerStrip1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox = (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


                if (textBox.FADatePicker.Text.Length == 4)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
                else if (textBox.FADatePicker.Text.Length == 7)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
            }
        }

        //private void faDatePickerStrip1_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (Class_BasicOperation.isNotDigit(e.KeyChar))
        //        e.Handled = true;
        //    else
        //        if (e.KeyChar == 13)
        //        {
        //            faDatePickerStrip1.FADatePicker.HideDropDown();
        //            faDatePickerStrip1.FADatePicker.Select();
        //        }

        //    if (e.KeyChar == 8)
        //        _BackSpace = true;
        //    else
        //        _BackSpace = false;
        //}

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))

                e.Handled = true;
            else if (e.KeyChar == 13)
                bt_Search_Click(sender, e);
        }

        private void uiPanel6_Click(object sender, EventArgs e)
        {

        }

        private void panelEx3_Click(object sender, EventArgs e)
        {

        }
    }
}
