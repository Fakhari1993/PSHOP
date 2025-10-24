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
    /// <summary>
    /// /
    //   در تاریخ 1399/12/26
    //طبق صحبت با مهندس قرار شد
    //1-برای همه فاکتورهای بسته نشده تا تاریخ بستن صندوق و بدون حواله، حواله به تاریخ فاکتور صادر میشود
    //2-محاسبه موجودی برای همه کالاهای  فاکتورهای بسته نشده تا تاریخ بستن صندوق
    //فرمول محاسبه موجودی=رسیدهای کل انبارها تا تاریخ بستن صندوق- حواله های کل انبارها تا تاریخ بستن صندوق
    //اگر حتی یک کالا با موجودی منفی داشته باشد که کالا  تیک اجازه موجودی منفی هم نداشته باشد، اجازه بستن صندوق رو نمیدهد
    //3- برای حواله همه فاکتورهای بسته نشده تا تاریخ بستن صندوق ارزش محاسبه میشود
    //چک میشود اگر فاکتور سند نداشت، سند هم صادر میشود
    //سطرهای بهای تمام شده سند خورده میشود

    /// </summary>
    public partial class Frm_029_CloseCash_Old : Form
    {
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Class_UserScope UserScope = new Class_UserScope();
        DataTable ReturnFactors = new DataTable();


        DataTable CheckTable = new DataTable();
        //DataTable CheckHavaleTable = new DataTable();
        //DataTable otherCheckHavaleTable = new DataTable();

        DataTable waredt = new DataTable();
        DataTable discountdt = new DataTable();
        DataTable taxdt = new DataTable();
        DataTable factordt = new DataTable();
        DataTable Sanaddt = new DataTable();
        DataTable iddt = new DataTable();
        DataTable bahaDT = new DataTable();
        DataTable RetutnbahaDT = new DataTable();
        bool _BackSpace = false;
        bool Isadmin = false;
        DataTable storefactor = new DataTable();
        //DataTable arzeshtable2 = new DataTable();


        Int16 projectId = -100;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();



        public Frm_029_CloseCash_Old()
        {
            InitializeComponent();
        }

        private void Frm_029_CloseCash_Load(object sender, EventArgs e)
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


            DataTable Person = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_045_PersonInfo where Column12=1");
            this.gx_recivefromcustomer.DropDowns[0].SetDataBinding(Person, "");

            DataTable Person2 = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_045_PersonInfo where Column12=1");
            this.gx_losefromcash.DropDowns["Person"].SetDataBinding(Person2, "");


            DataTable ACCINfo = clDoc.ReturnTable(Properties.Settings.Default.BANK, "Select ColumnId,Column01,Column02,isnull(Column12,'') as Column12 from Table_020_BankCashAccInfo where Column01=0");
            this.gx_recivefrombank.DropDowns[0].SetDataBinding(ACCINfo, "");


            DataTable ACC = clDoc.ReturnTable(Properties.Settings.Default.ACNT, "SELECT ACC_Code,ACC_Name from AllHeaders()");
            this.gx_losefromcash.DropDowns["ACC"].SetDataBinding(ACC, "");


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
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;


        }

        private float FirstRemain(int GoodCode, string ware, string date, int? drafid)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                string CommandText = string.Empty;
                if (drafid == null)
                    CommandText = @" SELECT j.InValue -j.OutValue AS Remain
FROM   ( SELECT *
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
            /// <summary>
            /// /
            //   در تاریخ 1399/12/26
            //طبق صحبت با مهندس قرار شد
            //1-برای همه فاکتورهای بسته نشده تا تاریخ بستن صندوق و بدون حواله، حواله به تاریخ فاکتور صادر میشود
            //2-محاسبه موجودی برای همه کالاهای  فاکتورهای بسته نشده تا تاریخ بستن صندوق
            //فرمول محاسبه موجودی=رسیدهای کل انبارها تا تاریخ بستن صندوق- حواله های کل انبارها تا تاریخ بستن صندوق
            //اگر حتی یک کالا با موجودی منفی داشته باشد که کالا  تیک اجازه موجودی منفی هم نداشته باشد، اجازه بستن صندوق رو نمیدهد
            //3- برای حواله همه فاکتورهای بسته نشده تا تاریخ بستن صندوق ارزش محاسبه میشود
            //چک میشود اگر فاکتور سند نداشت، سند هم صادر میشود
            //سطرهای بهای تمام شده سند خورده میشود

            /// </summary>
            DataTable NotExist = new DataTable();

            try
            {
                if (gx_factors.GetRows().Length > 0 || ReturnFactors.Rows.Count > 0)
                {

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


                    chehckessentioal();


                    bool ok1 = true;
                    if (faDatePickerStrip1.FADatePicker.Text != string.Empty)
                    {

                        {
                            this.Cursor = Cursors.WaitCursor;
                            bt_ExportDoc.Enabled = false;

                            #region برای همه فاکتورهای بدون حواله، حواله به تاریخ فاکتور صادر میشود

                            //////////////
                            string sanadcmd = string.Empty;
                            sanadcmd = "   declare @draftkey int  declare @DraftNum int  ";
                            foreach (DataRow idro in iddt.Rows)
                            {


                                Sanaddt = new DataTable();

                                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT FactorTable.columnid,
                                               FactorTable.column01,
                                               FactorTable.date,
                                               ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
                                               ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
                                               FactorTable.Bed,
                                               FactorTable.Bes,
                                               FactorTable.NetTotal,FactorTable.person,FactorTable.saletype,FactorTable.Func,FactorTable.ware,FactorTable.Project
                                        FROM   (
                                                   SELECT dbo.Table_010_SaleFactor.columnid,
                                                          dbo.Table_010_SaleFactor.column01,
                                                          dbo.Table_010_SaleFactor.column02 AS Date,
                                                          dbo.Table_010_SaleFactor.column03 AS person,
                                                          dbo.Table_010_SaleFactor.column36 AS saletype,
                                                          dbo.Table_010_SaleFactor.column42 AS ware,
                                                          dbo.Table_010_SaleFactor.column43 AS Func,
                                                          OtherPrice.PlusPrice AS Ezafat,
                                                          OtherPrice.MinusPrice AS Kosoorat,
                                                          OtherPrice.Bed,
                                                          OtherPrice.Bes,
                                                          dbo.Table_010_SaleFactor.Column28 AS NetTotal,
                                                          dbo.Table_010_SaleFactor.Column44 AS Project

                                                   FROM   dbo.Table_010_SaleFactor
                                                         
                                                          LEFT OUTER JOIN (
                                                                   SELECT columnid,
                                                                          SUM(PlusPrice) AS PlusPrice,
                                                                          SUM(MinusPrice) AS MinusPrice,
                                                                          Bed,
                                                                          Bes
                                                                   FROM   (
                                                                              SELECT Table_010_SaleFactor_2.columnid,
                                                                                     SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                                                     PlusPrice,
                                                                                     0 AS MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = dbo.Table_012_Child2_SaleFactor.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_2
                                                                                          ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                                                              Table_010_SaleFactor_2.columnid
                                                                              WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_2.columnid,
                                                                                     dbo.Table_012_Child2_SaleFactor.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                              UNION ALL
                                                                              SELECT Table_010_SaleFactor_1.columnid,
                                                                                     0 AS PlusPrice,
                                                                                     SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                                                     MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                                                     Table_012_Child2_SaleFactor_1
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = 
                                                                                              Table_012_Child2_SaleFactor_1.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_1
                                                                                          ON  
                                                                                              Table_012_Child2_SaleFactor_1.column01 = 
                                                                                              Table_010_SaleFactor_1.columnid
                                                                              WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_1.columnid,
                                                                                     Table_012_Child2_SaleFactor_1.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                          ) AS OtherPrice_1
                                                                   GROUP BY
                                                                          columnid,
                                                                          OtherPrice_1.Bed,
                                                                          OtherPrice_1.Bes
                                                               ) AS OtherPrice
                                                               ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid where (dbo.Table_010_SaleFactor.Column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                               ) AS FactorTable
                                        WHERE  FactorTable.columnid=" + int.Parse(idro["columnid"].ToString()) + @"
                                                                                                           ", ConSale);
                                Adapter.Fill(Sanaddt);

                                if (int.Parse(idro["Column09"].ToString()) == 0)
                                {
                                    sanadcmd += " set @DraftNum=( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft)";
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
                                                                                               ,[Column26]) VALUES(@DraftNum,'" + Sanaddt.Rows[0]["date"] + "'," + Sanaddt.Rows[0]["ware"]
                                                                                                             + "," + (Sanaddt.Rows[0]["Func"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Func"].ToString()) ? Sanaddt.Rows[0]["Func"] : waredt.Rows[0]["Column02"]) + @", " + Sanaddt.Rows[0]["person"] + ",'" + "حواله صادره بابت فاکتور فروش ش" + Sanaddt.Rows[0]["column01"] +
                                                                                                         "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + Sanaddt.Rows[0]["columnid"] + ",0,0,0,0,0,0,0,null,0,0); SET @draftkey=SCOPE_IDENTITY()";

                                    Adapter = new SqlDataAdapter(
                                                                     @"SELECT  [columnid] ,[column01] ,[column02] ,[column03] ,[column04] ,[column05] ,[column06] ,[column07] ,[column08] ,[column09]
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
                                                                    ,Column34,Column35,Column36,Column37
                                                                  FROM  [dbo].[Table_011_Child1_SaleFactor] WHERE column01 =" + Sanaddt.Rows[0]["columnid"], ConSale);
                                    DataTable Child1 = new DataTable();
                                    Adapter.Fill(Child1);

                                    foreach (DataRow item1 in Child1.Rows)
                                    {
                                        if (clDoc.IsGood(item1["Column02"].ToString()))
                                        {
                                            double value = Convert.ToDouble(item1["Column07"]);
                                            string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                                                "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                                item1["Column02"].ToString());


                                            if (item1["column03"].ToString() != orginalunit)
                                            {
                                                float h = clDoc.GetZarib(Convert.ToInt32(item1["Column02"]), Convert.ToInt16(item1["column03"]), Convert.ToInt16(orginalunit));
                                                value = value * h;
                                            }

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
                                                                               ,[Column35]) VALUES(@draftkey," + item1["Column02"].ToString() + "," +
                                                                                                                   orginalunit + "," + item1["Column04"].ToString() + "," + item1["Column05"].ToString() + "," + value + "," +
                                                                                                                    value + "," + item1["Column08"].ToString() + "," + item1["Column09"].ToString() + "," + item1["Column10"].ToString() + "," +
                                                                                                                    item1["Column11"].ToString() + ",NULL,NULL," + (item1["Column22"].ToString().Trim() == "" ? "NULL" : item1["Column22"].ToString())
                                                                                                                    + ",0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL," +
                                                                                                                    (item1["Column14"].ToString().Trim() == "" ? "NULL" : item1["Column14"].ToString()) + "," +
                                                                                                                    item1["Column15"].ToString() +
                                                                                                                        ",0,0,0,0," + (item1["Column30"].ToString() == "True" ? "1" : "0") + "," +
                                                                                                                        (item1["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column34"].ToString() + "'") + "," +
                                                                                                                        (item1["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column35"].ToString() + "'")
                                                                                                                        + "," + item1["Column31"].ToString()
                                                                                                                        + "," + item1["Column32"].ToString() + "," + item1["Column36"].ToString() + "," + item1["Column37"].ToString() + ")";
                                        }
                                    }
                                    sanadcmd += "Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column09=@draftkey where ColumnId=" + Sanaddt.Rows[0]["columnid"];

                                }
                            }
                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                            {
                                Con.Open();

                                SqlTransaction sqlTran = Con.BeginTransaction();
                                SqlCommand Command = Con.CreateCommand();
                                Command.Transaction = sqlTran;

                                try
                                {
                                    Command.CommandText = sanadcmd;
                                    Command.ExecuteNonQuery();
                                    sqlTran.Commit();

                                }
                                catch (Exception es)
                                {
                                    ok1 = false;
                                    this.Cursor = Cursors.Default;
                                    Class_BasicOperation.CheckExceptionType(es, this.Name);
                                }
                            }
                            #endregion
                            #region محاسبه موجودی برای همه کالاهای  فاکتورهای بسته نشده تا تاریخ بستن صندوق////فرمول محاسبه موجودی=رسیدهای کل انبارها تا تاریخ بستن صندوق- حواله های کل انبارها تا تاریخ بستن صندوق

                            if (ok1)
                            {
                                DataTable otherCheckHavaleTable = new DataTable();

                                SqlDataAdapter Adapter1 = new SqlDataAdapter(
                                                                            @"SELECT h.column02  AS column02,
                                                                                   ISNULL(
                                                                                       (
                                                                                           SELECT SUM(tcpr.column07)
                                                                                           FROM   " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt AS tcpr
                                                                                                  JOIN " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt AS tpr
                                                                                                       ON  tpr.columnid = tcpr.column01
                                                                                           WHERE  tcpr.column02 = h.column02
                                                                                                  AND tpr.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                       ),
                                                                                       0
                                                                                   )
                                                                                   -ISNULL(
                                                                                       (
                                                                                           SELECT SUM(tcpr.column07)
                                                                                           FROM   " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft AS tcpr
                                                                                                  JOIN " + ConWare.Database + @".dbo.Table_007_PwhrsDraft AS tpr
                                                                                                       ON  tpr.columnid = tcpr.column01
                                                                                           WHERE  tcpr.column02 = h.column02
                                                                                                  AND tpr.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                       ),
                                                                                       0
                                                                                   )           AS Remain,
                                                                                   (
                                                                                       SELECT ISNULL(
                                                                                                  (
                                                                                                      SELECT ISNULL(Column16, 0) AS Column16
                                                                                                      FROM   " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients
                                                                                                      WHERE  ColumnId = h.column02
                                                                                                  ),
                                                                                                  0
                                                                                              )
                                                                                   )           AS mojodimanfi,
                                                                                   (
                                                                                       SELECT ISNULL(
                                                                                                  (
                                                                                                      SELECT ISNULL(Column01, 0)
                                                                                                      FROM   " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients
                                                                                                      WHERE  ColumnId = h.column02
                                                                                                  ),
                                                                                                  0
                                                                                              )
                                                                                   )           AS GoodCode,
                                                                                   (
                                                                                       SELECT ISNULL(
                                                                                                  (
                                                                                                      SELECT ISNULL(Column02, 0)
                                                                                                      FROM   " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients
                                                                                                      WHERE  ColumnId = h.column02
                                                                                                  ),
                                                                                                  0
                                                                                              )
                                                                                   )           AS GoodName
                                                                            FROM   Table_011_Child1_SaleFactor h
                                                                                   JOIN Table_010_SaleFactor tsf
                                                                                        ON  tsf.columnid = h.column01
                                                                            WHERE  (
                                                                                       tsf.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                   )
                                                                                   AND (tsf.column17 = 0)--باطل نيست
                                                                                                         --AND (tsf.column19 = 0)--مرجوع نيست
                                                                                   AND tsf.Column53 = 0-- بسته نشده
                                                                                   AND tsf.Column09 != 0
                                                                                   AND (
                                                                                           tsf.Column44 = " + projectId + @"
                                                                                           OR '" + (Isadmin) + @"' = N'True'
                                                                                       )
                                                                                   AND (
                                                                                           ISNULL(
                                                                                               (
                                                                                                   SELECT SUM(tcpr.column07)
                                                                                                   FROM   " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt AS tcpr
                                                                                                          JOIN " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt AS tpr
                                                                                                               ON  tpr.columnid = tcpr.column01
                                                                                                   WHERE  tcpr.column02 = h.column02
                                                                                                          AND tpr.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                               ),
                                                                                               0
                                                                                           )
                                                                                           -ISNULL(
                                                                                               (
                                                                                                   SELECT SUM(tcpr.column07)
                                                                                                   FROM   " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft AS tcpr
                                                                                                          JOIN " + ConWare.Database + @".dbo.Table_007_PwhrsDraft AS tpr
                                                                                                               ON  tpr.columnid = tcpr.column01
                                                                                                   WHERE  tcpr.column02 = h.column02
                                                                                                          AND tpr.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                               ),
                                                                                               0
                                                                                           )
                                                                                       ) < 0
                                                                                   AND (
                                                                                           SELECT ISNULL(
                                                                                                      (
                                                                                                          SELECT ISNULL(Column16, 0) AS Column16
                                                                                                          FROM   " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients
                                                                                                          WHERE  ColumnId = h.column02
                                                                                                      ),
                                                                                                      0
                                                                                                  )
                                                                                       ) = 0
                                                                            GROUP BY
                                                                                   h.column02
                                                                                      ", ConSale);
                                Adapter1.Fill(otherCheckHavaleTable);


                                if (otherCheckHavaleTable.Rows.Count > 0)
                                {
                                    this.Cursor = Cursors.Default;
                                    ok1 = false;
                                    _05_Sale.GoodList2 frm1 = new GoodList2(otherCheckHavaleTable, "لیست کالاهای ناموجود برای حواله های صادر شده");
                                    frm1.ShowDialog();
                                }

                            }


                            #endregion
                            #region برای حواله همه فاکتورهای بسته نشده تا تاریخ بستن صندوق ارزش محاسبه میشو
                            if (ok1)
                            {
                                //محاسبه ارزش کالا های حواله 
                                string upcmd = string.Empty;
                                string notset = string.Empty;
                                string Returnupcmd = string.Empty;
                                string Returnnotset = string.Empty;
                                sanadcmd = "    declare @DocNum int    declare @DocID int";


                                SqlDataAdapter Adapter = new SqlDataAdapter(
                                                            @"SELECT  
                                                                            tsf.columnid,
                                                                            tsf.column01,
                                                                            tsf.Column09,
                                                                            tsf.Column02 as factordate,
                                                                            tsf.Column10 as DocID,
                                                                            dff.column02 as date,
                                                                            dff.column03 as ware,
                                                                            tsf.Column44 as Project
                                                                    FROM   Table_010_SaleFactor tsf 
                                                                            join " + ConWare.Database + @".dbo.Table_007_PwhrsDraft dff on dff.columnid=tsf.Column09
                                                                    WHERE  (
                                                                               tsf.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                           )
                                                                          
                                                                           AND ( tsf.column17 = 0)--باطل نيست
                                                                          -- AND (tsf.column19 = 0)--مرجوع نيست
                                                                           AND tsf.Column53 = 0-- بسته نشده
                                                                           AND (tsf.Column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                                                           order by   tsf.column02,tsf.column14,tsf.columnid
                                                                           
                                                                             ", ConSale);
                                DataTable ArzeshTable = new DataTable();

                                Adapter.Fill(ArzeshTable);

                                Adapter = new SqlDataAdapter(
                                                       @"SELECT  
                                                                            tsf.columnid,
                                                                            tsf.column01,
                                                                            tsf.Column09,
                                                                            tsf.Column02 as factordate,
                                                                            tsf.Column10 as DocID,
                                                                            dff.column02 as date,
                                                                            dff.column03 as ware,
                                                                            tsf.Column30 as Project
                                                                    FROM   Table_018_MarjooiSale tsf 
                                                                            join " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt dff on dff.columnid=tsf.Column09
                                                                    WHERE  (
                                                                               tsf.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                           )
                                                                          
                                                                           AND tsf.Column27 = 0-- بسته نشده
                                                                           AND (tsf.Column30=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                                                           order by   tsf.column02,tsf.column14,tsf.columnid
                                                                           
                                                                             ", ConSale);
                                DataTable ReturnArzeshTable = new DataTable();
                                Adapter.Fill(ReturnArzeshTable);




                                using (SqlConnection Con2 = new SqlConnection(Properties.Settings.Default.WHRS))
                                {
                                    Con2.Open();

                                    foreach (DataRow ar1 in ArzeshTable.Rows)
                                    {

                                        DataRow ar2 = clDoc.ReturnTable(Properties.Settings.Default.SALE, @"SELECT  
                                                                           tsf.columnid,tsf.column01, tsf.Column09, tsf.Column10 as DocID,dff.column02 as date,dff.column03 as ware,tsf.Column44 Project
                                                                    FROM   Table_010_SaleFactor tsf join " + ConWare.Database + @".dbo.Table_007_PwhrsDraft dff on dff.columnid=tsf.Column09
                                                                    WHERE tsf.columnid=" + ar1["columnid"]).Rows[0];


                                        try
                                        {
                                            string bahas = string.Empty;

                                            SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + ar2["Column09"], ConWare);
                                            DataTable Table = new DataTable();
                                            goodAdapter.Fill(Table);

                                            //محاسبه ارزش و ذخیره آن در جدول Child1
                                            double value = 0;
                                            foreach (DataRow item2 in Table.Rows)
                                            {
                                                if (Class_BasicOperation._WareType)
                                                {
                                                    Adapter = new SqlDataAdapter("EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + ar2["ware"].ToString(), ConWare);
                                                    DataTable TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + ar2["Column09"] + " and DetailID=" + item2["Columnid"].ToString());
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                                        + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con2);
                                                    UpdateCommand.ExecuteNonQuery();
                                                    value += Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4);
                                                }

                                                else
                                                {
                                                    Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + ar2["ware"].ToString() + ",@Date='" + ar2["date"].ToString() + "',@id=" + ar2["Column09"] + ",@residid=0", ConWare);
                                                    DataTable TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                                  + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con2);
                                                    UpdateCommand.ExecuteNonQuery();
                                                    value += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4);
                                                }
                                            }

                                            if (int.Parse(ar1["DocID"].ToString()) == 0)
                                            {
                                                int LastDocnum = LastDocNum(ar1["factordate"].ToString());
                                                if (LastDocnum > 0)
                                                    sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                                                else
                                                    sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                                                    VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + ar1["factordate"] + "',2,0,N'فاکتور فروش','" + Class_BasicOperation._UserName +
                                               "',getdate()); SET @DocID=SCOPE_IDENTITY()";


                                                Sanaddt = new DataTable();

                                                Adapter = new SqlDataAdapter(@"SELECT FactorTable.columnid,
                                               FactorTable.column01,
                                               FactorTable.date,
                                               ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
                                               ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
                                               FactorTable.Bed,
                                               FactorTable.Bes,
                                               FactorTable.NetTotal,FactorTable.person,FactorTable.saletype,FactorTable.Func,FactorTable.ware,FactorTable.Project
                                        FROM   (
                                                   SELECT dbo.Table_010_SaleFactor.columnid,
                                                          dbo.Table_010_SaleFactor.column01,
                                                          dbo.Table_010_SaleFactor.column02 AS Date,
                                                          dbo.Table_010_SaleFactor.column03 AS person,
                                                          dbo.Table_010_SaleFactor.column36 AS saletype,
                                                          dbo.Table_010_SaleFactor.column42 AS ware,
                                                          dbo.Table_010_SaleFactor.column43 AS Func,
                                                          OtherPrice.PlusPrice AS Ezafat,
                                                          OtherPrice.MinusPrice AS Kosoorat,
                                                          OtherPrice.Bed,
                                                          OtherPrice.Bes,
                                                          dbo.Table_010_SaleFactor.Column28 AS NetTotal,
                                                          dbo.Table_010_SaleFactor.Column44 AS Project

                                                   FROM   dbo.Table_010_SaleFactor
                                                         
                                                          LEFT OUTER JOIN (
                                                                   SELECT columnid,
                                                                          SUM(PlusPrice) AS PlusPrice,
                                                                          SUM(MinusPrice) AS MinusPrice,
                                                                          Bed,
                                                                          Bes
                                                                   FROM   (
                                                                              SELECT Table_010_SaleFactor_2.columnid,
                                                                                     SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                                                     PlusPrice,
                                                                                     0 AS MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = dbo.Table_012_Child2_SaleFactor.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_2
                                                                                          ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                                                              Table_010_SaleFactor_2.columnid
                                                                              WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_2.columnid,
                                                                                     dbo.Table_012_Child2_SaleFactor.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                              UNION ALL
                                                                              SELECT Table_010_SaleFactor_1.columnid,
                                                                                     0 AS PlusPrice,
                                                                                     SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                                                     MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                                                     Table_012_Child2_SaleFactor_1
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = 
                                                                                              Table_012_Child2_SaleFactor_1.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_1
                                                                                          ON  
                                                                                              Table_012_Child2_SaleFactor_1.column01 = 
                                                                                              Table_010_SaleFactor_1.columnid
                                                                              WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_1.columnid,
                                                                                     Table_012_Child2_SaleFactor_1.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                          ) AS OtherPrice_1
                                                                   GROUP BY
                                                                          columnid,
                                                                          OtherPrice_1.Bed,
                                                                          OtherPrice_1.Bes
                                                               ) AS OtherPrice
                                                               ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid where (dbo.Table_010_SaleFactor.Column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                               ) AS FactorTable
                                        WHERE  FactorTable.columnid=" + ar1["columnid"] + @"
                                                                                                           ", ConSale);
                                                Adapter.Fill(Sanaddt);

                                                factordt = new DataTable();
                                                Adapter = new SqlDataAdapter(
                                                                          @"SELECT       isnull( column08,'') as Column07,isnull(column14,'') as Column13
                                                                    FROM            Table_002_SalesTypes
                                                                    WHERE        (columnid = " + Sanaddt.Rows[0]["saletype"] + ") ", ConBase);
                                                Adapter.Fill(factordt);

                                                string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());

                                                sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
          ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                            " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                            " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                            " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                            " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                            " + Sanaddt.Rows[0]["person"] + @", NULL , " + ((Sanaddt.Rows[0]["Project"] != DBNull.Value && Sanaddt.Rows[0]["Project"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Project"].ToString())) ? Sanaddt.Rows[0]["Project"] : "NULL") + @" ,
               " + "'فاکتور فروش " + Sanaddt.Rows[0]["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";


                                                _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                                                sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
          ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                            " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                            " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                            " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                            " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                            NULL, NULL , " + ((Sanaddt.Rows[0]["Project"] != DBNull.Value && Sanaddt.Rows[0]["Project"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Project"].ToString())) ? Sanaddt.Rows[0]["Project"] : "NULL") + @" ,
               " + "'فاکتور فروش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                            NULL, NULL , " + ((Sanaddt.Rows[0]["Project"] != DBNull.Value && Sanaddt.Rows[0]["Project"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Project"].ToString())) ? Sanaddt.Rows[0]["Project"] : "NULL") + @" ,
               " + "'تخفیف فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                        _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                                                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
          ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                            " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                            " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                            " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                            " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                           " + int.Parse(Sanaddt.Rows[0]["person"].ToString()) + @", NULL , " + ((Sanaddt.Rows[0]["Project"] != DBNull.Value && Sanaddt.Rows[0]["Project"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Project"].ToString())) ? Sanaddt.Rows[0]["Project"] : "NULL") + @" ,
               " + "'تخفیف فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                            " + int.Parse(Sanaddt.Rows[0]["person"].ToString()) + @", NULL , " + ((Sanaddt.Rows[0]["Project"] != DBNull.Value && Sanaddt.Rows[0]["Project"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Project"].ToString())) ? Sanaddt.Rows[0]["Project"] : "NULL") + @" ,
               " + "'ارزش افزوده فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                        _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                                                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
          ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                            " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                            " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                            " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                            " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                            NULL, NULL , " + ((Sanaddt.Rows[0]["Project"] != DBNull.Value && Sanaddt.Rows[0]["Project"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Project"].ToString())) ? Sanaddt.Rows[0]["Project"] : "NULL") + @" ,
               " + "'ارزش افزوده فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                                    }


                                                }

                                                if (Class_BasicOperation._FinType)//بهای تمام شده
                                                {
                                                    if (value > 0)
                                                    {
                                                        _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column07"].ToString());

                                                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + bahaDT.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NUll, NULL , " + ((ar2["Project"] != DBNull.Value && ar2["Project"] != null && !string.IsNullOrWhiteSpace(ar2["Project"].ToString())) ? ar2["Project"] : "NULL") + @" ,
                   " + "'بهای تمام شده فاکتور فروش ش " + ar2["column01"] + "'," + Convert.ToInt64(value) + @",0,0,0,-1,26," + int.Parse(ar2["Column09"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                        _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column13"].ToString());

                                                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + bahaDT.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((ar2["Project"] != DBNull.Value && ar2["Project"] != null && !string.IsNullOrWhiteSpace(ar2["Project"].ToString())) ? ar2["Project"] : "NULL") + @" ,
                   " + "'بهای تمام شده فاکتور فروش ش " + ar2["column01"] + "',0," + Convert.ToInt64(value) + @",0,0,-1,26," + int.Parse(ar2["Column09"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";





                                                    }
                                                }


                                                sanadcmd += " Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07=@DocID,column10='" + Class_BasicOperation._UserName + "',column11=getdate() where ColumnId=" + ar2["Column09"];
                                                sanadcmd += " Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column10=@DocID where ColumnId =" + Sanaddt.Rows[0]["columnid"].ToString();



                                            }
                                            else
                                            {
                                                if (Class_BasicOperation._FinType)//بهای تمام شده
                                                {
                                                    if (value > 0 && Convert.ToInt32(ar2["DocID"]) > 0)
                                                    {
                                                        string[] _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column07"].ToString());

                                                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + ar2["DocID"] + @",'" + bahaDT.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NUll, NULL , " + ((ar2["Project"] != DBNull.Value && ar2["Project"] != null && !string.IsNullOrWhiteSpace(ar2["Project"].ToString())) ? ar2["Project"] : "NULL") + @" ,
                   " + "'بهای تمام شده فاکتور فروش ش " + ar2["column01"] + "'," + Convert.ToInt64(value) + @",0,0,0,-1,26," + int.Parse(ar2["Column09"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                        _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column13"].ToString());

                                                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + ar2["DocID"] + @",'" + bahaDT.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((ar2["Project"] != DBNull.Value && ar2["Project"] != null && !string.IsNullOrWhiteSpace(ar2["Project"].ToString())) ? ar2["Project"] : "NULL") + @" ,
                   " + "'بهای تمام شده فاکتور فروش ش " + ar2["column01"] + "',0," + Convert.ToInt64(value) + @",0,0,-1,26," + int.Parse(ar2["Column09"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";





                                                    }
                                                }
                                            }


                                            upcmd += ar1["columnid"].ToString() + ",";




                                        }
                                        catch
                                        { notset += ar2["column01"].ToString() + ","; }

                                    }
                                    foreach (DataRow ar1 in ReturnArzeshTable.Rows)
                                    {
                                        DataRow ar2 = clDoc.ReturnTable(Properties.Settings.Default.SALE, @"SELECT  
                                                                           tsf.columnid,tsf.column01, tsf.Column09, tsf.Column10 as DocID,dff.column02 as date,dff.column03 as ware,tsf.Column30 Project
                                                                    FROM   Table_018_MarjooiSale tsf join " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt dff on dff.columnid=tsf.Column09
                                                                    WHERE tsf.columnid=" + ar1["columnid"]).Rows[0];

                                        try
                                        {
                                            string bahas = string.Empty;

                                            SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_012_Child_PwhrsReceipt where Column01=" + ar2["Column09"], ConWare);
                                            DataTable Table = new DataTable();
                                            goodAdapter.Fill(Table);

                                            //محاسبه ارزش و ذخیره آن در جدول Child1
                                            double value = 0;
                                            foreach (DataRow item2 in Table.Rows)
                                            {
                                                if (Class_BasicOperation._WareType)
                                                {
                                                    Adapter = new SqlDataAdapter("EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + ar2["ware"].ToString(), ConWare);
                                                    DataTable TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + ar2["Column09"] + " and DetailID=" + item2["Columnid"].ToString());
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  column20=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                                        + " , column21=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con2);
                                                    UpdateCommand.ExecuteNonQuery();
                                                    value += Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4);
                                                }

                                                else
                                                {
                                                    Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + ar2["ware"].ToString() + ",@Date='" + ar2["date"].ToString() + "',@id=0,@residid=" + ar2["Column09"] + "", ConWare);
                                                    DataTable TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_012_Child_PwhrsReceipt SET  column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                                  + " , column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con2);
                                                    UpdateCommand.ExecuteNonQuery();
                                                    value += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4);
                                                }
                                            }
                                            {
                                                if (Class_BasicOperation._FinType)//بهای تمام شده
                                                {
                                                    if (value > 0 && Convert.ToInt32(ar2["DocID"]) > 0)
                                                    {
                                                        string[] _AccInfo = clDoc.ACC_Info(RetutnbahaDT.Rows[0]["Column07"].ToString());

                                                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + ar2["DocID"] + @",'" + RetutnbahaDT.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NUll, NULL , " + ((ar2["Project"] != DBNull.Value && ar2["Project"] != null && !string.IsNullOrWhiteSpace(ar2["Project"].ToString())) ? ar2["Project"] : "NULL") + @" ,
                   " + "'بهای تمام شده فاکتور مرجوعی ش " + ar2["column01"] + "'," + Convert.ToInt64(value) + @",0,0,0,-1,27," + int.Parse(ar2["Column09"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                        _AccInfo = clDoc.ACC_Info(RetutnbahaDT.Rows[0]["Column13"].ToString());

                                                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + ar2["DocID"] + @",'" + RetutnbahaDT.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((ar2["Project"] != DBNull.Value && ar2["Project"] != null && !string.IsNullOrWhiteSpace(ar2["Project"].ToString())) ? ar2["Project"] : "NULL") + @" ,
                   " + "'بهای تمام شده فاکتور مرجوعی ش " + ar2["column01"] + "',0," + Convert.ToInt64(value) + @",0,0,-1,27," + int.Parse(ar2["Column09"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";





                                                    }
                                                }
                                            }


                                            Returnupcmd += ar1["columnid"].ToString() + ",";




                                        }
                                        catch
                                        { Returnnotset += ar2["column01"].ToString() + ","; }


                                    }

                                    if (!string.IsNullOrWhiteSpace(upcmd) || !string.IsNullOrWhiteSpace(Returnupcmd))
                                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                                        {
                                            Con.Open();

                                            SqlTransaction sqlTran = Con.BeginTransaction();
                                            SqlCommand Command = Con.CreateCommand();
                                            Command.Transaction = sqlTran;

                                            try
                                            {
                                                Command.CommandText = sanadcmd;
                                                Command.ExecuteNonQuery();
                                                sqlTran.Commit();


                                                #region بستن صندوق
                                                if (!string.IsNullOrWhiteSpace(upcmd) || !string.IsNullOrWhiteSpace(Returnupcmd))
                                                {



                                                    bool ok = false;
                                                    string closcmd = " ";
                                                    SqlParameter DocNum;
                                                    DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                                                    DocNum.Direction = ParameterDirection.Output;

                                                    SqlParameter DocID;
                                                    DocID = new SqlParameter("DocID", SqlDbType.Int);
                                                    DocID.Direction = ParameterDirection.Output;


                                                    table_96_CloseCashBindingSource.EndEdit();
                                                    table_97_ReceivedFromCustomersBindingSource.EndEdit();
                                                    table_98_ReceivedFromBankBindingSource.EndEdit();
                                                    table_99_LosesFromCashBindingSource.EndEdit();
                                                    this.table_96_CloseCashTableAdapter.Update(this.dataSet6.Table_96_CloseCash);
                                                    this.table_99_LosesFromCashTableAdapter.Update(this.dataSet6.Table_99_LosesFromCash);
                                                    this.table_98_ReceivedFromBankTableAdapter.Update(this.dataSet6.Table_98_ReceivedFromBank);
                                                    this.table_97_ReceivedFromCustomersTableAdapter.Update(this.dataSet6.Table_97_ReceivedFromCustomers);


                                                    closcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + faDatePickerStrip1.FADatePicker.Text + "',2,0,' صدور سند بستن صندوق شماره " + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + "','" + Class_BasicOperation._UserName +
                                        "',getdate()); SET @DocID=SCOPE_IDENTITY()";


                                                    foreach (Janus.Windows.GridEX.GridEXRow item in gx_losefromcash.GetDataRows())
                                                    {

                                                        if (Convert.ToDouble(item.Cells["Column04"].Value) > 0)
                                                        {
                                                            ok = true;
                                                            string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column02"].Value.ToString());

                                                            closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + item.Cells["Column02"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (item.Cells["Column03"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["Column03"].Value.ToString()) ? (item.Cells["Column03"].Value.ToString()) : "NULL") + @", NULL , " + (projectId != -100 ? projectId.ToString() : "NULL") + @" ,
                   " + "'بابت واریز/برداشت روزانه از صندوق'," + Convert.ToInt64(item.Cells["Column04"].Value) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                                                        }
                                                    }

                                                    foreach (Janus.Windows.GridEX.GridEXRow item in gx_recivefrombank.GetDataRows())
                                                    {
                                                        if (Convert.ToDouble(item.Cells["Column03"].Value) > 0)
                                                        {
                                                            ok = true;

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
                                NULL, NULL , " + (projectId != -100 ? projectId.ToString() : "NULL") + @" ,
                   " + "'واریز/برداشت روزانه صندوق'," + Convert.ToInt64(item.Cells["Column03"].Value) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                        }
                                                    }
                                                    customerrecive = Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
                                                    losefromcash = Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                                                    if (customerrecive + losefromcash > 0)
                                                    {
                                                        ok = true;

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
                                NULL, NULL , " + (projectId != -100 ? projectId.ToString() : "NULL") + @" ,
                   " + "'بابت واریز/برداشت روزانه از صندوق' " + ",0," + Convert.ToInt64(customerrecive + losefromcash) + @",0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                                    }
                                                    DataTable ddt = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT isnull(tst.Column02,'') as bed,isnull(tst.Column02,'') as bes
                                                                                                                FROM  Table_030_Setting tst
                                                                                                                WHERE  tst.ColumnId = 64");
                                                    decimal CashDeductionAddition = Convert.ToDecimal(txt_CashDeductionAddition.Value);
                                                    if (CashDeductionAddition != Convert.ToDecimal(0))
                                                    {
                                                        ok = true;

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
                                NULL, NULL , " + (projectId != -100 ? projectId.ToString() : "NULL") + @" ,
                   " + "'بابت کسر و اضافه صندوق'," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                                            _AccInfo = clDoc.ACC_Info((ddt.Rows[0]["bes"].ToString()));

                                                            closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + ddt.Rows[0]["bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId.ToString() : "NULL") + @" ,
                   " + "'بابت کسر و اضافه صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                                NULL, NULL , " + (projectId != -100 ? projectId.ToString() : "NULL") + @" ,
                   " + "'بابت کسر و اضافه صندوق'," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                            _AccInfo = clDoc.ACC_Info((acc.Rows[0]["column08"].ToString()));

                                                            closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId.ToString() : "NULL") + @" ,
                   " + "'بابت کسر و اضافه صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                                        }

                                                    }
                                                    decimal CashAcountDifference = Convert.ToDecimal(txt_CashAcountDifference.Value);
                                                    if (CashAcountDifference != Convert.ToDecimal(0))
                                                    {
                                                        ok = true;

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
                                NULL, NULL , " + (projectId != -100 ? projectId.ToString() : "NULL") + @" ,
                   " + "'بابت اختلاف حساب صندوق'," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                                            _AccInfo = clDoc.ACC_Info((acc.Rows[0]["column08"].ToString()));

                                                            closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId.ToString() : "NULL") + @" ,
                   " + "'بابت اختلاف حساب صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                                NULL, NULL , " + (projectId != -100 ? projectId.ToString() : "NULL") + @" ,
                   " + "'بابت اختلاف حساب صندوق'," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                                            _AccInfo = clDoc.ACC_Info((ddt.Rows[0]["bes"].ToString()));

                                                            closcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + ddt.Rows[0]["bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + (projectId != -100 ? projectId.ToString() : "NULL") + @" ,
                   " + "'بابت اختلاف حساب صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,-1,300," + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                                        }

                                                    }

                                                    using (SqlConnection Con1 = new SqlConnection(Properties.Settings.Default.ACNT))
                                                    {
                                                        Con1.Open();

                                                        SqlTransaction sqlTran1 = Con1.BeginTransaction();
                                                        SqlCommand Command1 = Con1.CreateCommand();
                                                        Command1.Transaction = sqlTran1;


                                                        try
                                                        {
                                                            if (ok)
                                                            {

                                                                if (!string.IsNullOrWhiteSpace(upcmd))
                                                                    closcmd += " update " + ConSale.Database + ".dbo.Table_010_SaleFactor set column53=1,Column67=" + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + ",Column62='" + Class_BasicOperation._UserName + "',Column63=getdate() where columnid in  (" + upcmd.TrimEnd(',') + ")  AND ( Column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')";
                                                                if (!string.IsNullOrWhiteSpace(Returnupcmd))
                                                                    closcmd += " update " + ConSale.Database + ".dbo.Table_018_MarjooiSale set Column27=1,Column31=" + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + " where columnid in  (" + Returnupcmd.TrimEnd(',') + ")     AND ( Column30=" + projectId + " or '" + (Isadmin) + @"'=N'True')";
                                                                closcmd += " update " + ConSale.Database + ".dbo.Table_96_CloseCash set Column16=@DocID where ColumnId=" + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"];


                                                                Command1.CommandText = closcmd;
                                                                Command1.Parameters.Add(DocNum);
                                                                Command1.Parameters.Add(DocID);
                                                                Command1.ExecuteNonQuery();
                                                                sqlTran1.Commit();
                                                                table_96_CloseCashBindingSource_PositionChanged(null, null);
                                                                bt_ExportDoc.Enabled = false;
                                                                bt_Search.Enabled = true;
                                                                string messag = string.Empty;
                                                                if (!Class_BasicOperation._FinType)
                                                                {
                                                                    if (!string.IsNullOrWhiteSpace(notset))
                                                                        messag += "محاسبه ارزش حواله فاکتور فروش/های زیر با خطا مواجه شد و بستن صندوق برای این فاکتور/های فروش انجام نشد" + Environment.NewLine + notset.TrimEnd(',');

                                                                    if (!string.IsNullOrWhiteSpace(Returnnotset))
                                                                        messag += Environment.NewLine + "محاسبه ارزش رسید فاکتور مرجوعی/های زیر با خطا مواجه شد و بستن صندوق برای این فاکتور/های مرجوعی انجام نشد" + Environment.NewLine + Returnnotset.TrimEnd(',');

                                                                }
                                                                if (Class_BasicOperation._FinType)
                                                                {
                                                                    if (!string.IsNullOrWhiteSpace(notset))
                                                                        messag += "محاسبه ارزش حواله و صدور سند بهای تمام شده ی فاکتور فروش/های زیر با خطا مواجه شد و بستن صندوق برای این فاکتور/های فروش انجام نشد" + Environment.NewLine + notset.TrimEnd(',');
                                                                    if (!string.IsNullOrWhiteSpace(Returnnotset))
                                                                        messag += Environment.NewLine + "محاسبه ارزش رسید و صدور سند بهای تمام شده ی فاکتور مرجوعی/های زیر با خطا مواجه شد و بستن صندوق برای این فاکتور/های مرجوعی انجام نشد" + Environment.NewLine + Returnnotset.TrimEnd(',');

                                                                }

                                                                if (!string.IsNullOrWhiteSpace(messag))
                                                                    Class_BasicOperation.ShowMsg("", messag, "Stop");

                                                                else
                                                                    MessageBox.Show("عملیات و صدور سند شماره " + DocNum.Value + " با موفقیت انجام شد");

                                                            }
                                                            else
                                                            {
                                                                if (!string.IsNullOrWhiteSpace(upcmd))
                                                                    closcmd = " update " + ConSale.Database + ".dbo.Table_010_SaleFactor set column53=1,Column67=" + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + ",Column62='" + Class_BasicOperation._UserName + "',Column63=getdate() where columnid in  (" + upcmd.TrimEnd(',') + ")  AND ( Column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')";
                                                                if (!string.IsNullOrWhiteSpace(Returnupcmd))
                                                                    closcmd += " update " + ConSale.Database + ".dbo.Table_018_MarjooiSale set Column27=1,Column31=" + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + " where columnid in  (" + Returnupcmd.TrimEnd(',') + ")    AND ( Column30=" + projectId + " or '" + (Isadmin) + @"'=N'True')";
                                                                Command1.CommandText = closcmd;
                                                                Command1.ExecuteNonQuery();
                                                                sqlTran1.Commit();
                                                                table_96_CloseCashBindingSource_PositionChanged(null, null);

                                                                bt_ExportDoc.Enabled = false;
                                                                bt_Search.Enabled = true;
                                                                string messag = string.Empty;
                                                                if (!Class_BasicOperation._FinType)
                                                                {
                                                                    if (!string.IsNullOrWhiteSpace(notset))
                                                                        messag += "محاسبه ارزش حواله فاکتور فروش/های زیر با خطا مواجه شد و بستن صندوق برای این فاکتور/های فروش انجام نشد" + Environment.NewLine + notset.TrimEnd(',');

                                                                    if (!string.IsNullOrWhiteSpace(Returnnotset))
                                                                        messag += Environment.NewLine + "محاسبه ارزش رسید فاکتور مرجوعی/های زیر با خطا مواجه شد و بستن صندوق برای این فاکتور/های مرجوعی انجام نشد" + Environment.NewLine + Returnnotset.TrimEnd(',');

                                                                }
                                                                if (Class_BasicOperation._FinType)
                                                                {
                                                                    if (!string.IsNullOrWhiteSpace(notset))
                                                                        messag += "محاسبه ارزش حواله و صدور سند بهای تمام شده ی فاکتور فروش/های زیر با خطا مواجه شد و بستن صندوق برای این فاکتور/های فروش انجام نشد" + Environment.NewLine + notset.TrimEnd(',');
                                                                    if (!string.IsNullOrWhiteSpace(Returnnotset))
                                                                        messag += Environment.NewLine + "محاسبه ارزش رسید و صدور سند بهای تمام شده ی فاکتور مرجوعی/های زیر با خطا مواجه شد و بستن صندوق برای این فاکتور/های مرجوعی انجام نشد" + Environment.NewLine + Returnnotset.TrimEnd(',');

                                                                }

                                                                if (!string.IsNullOrWhiteSpace(messag))
                                                                    Class_BasicOperation.ShowMsg("", messag, "Stop");

                                                                else
                                                                    MessageBox.Show("عملیات با موفقیت انجام شد");

                                                            }


                                                        }
                                                        catch (Exception es)
                                                        {
                                                            sqlTran1.Rollback();
                                                            this.Cursor = Cursors.Default;
                                                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                                                        }
                                                    }
                                                }

                                                #endregion





                                            }
                                            catch (Exception es)
                                            {

                                                this.Cursor = Cursors.Default;
                                                Class_BasicOperation.CheckExceptionType(es, this.Name);
                                            }

                                            this.Cursor = Cursors.Default;
                                        }



                                }
                            }

                            #endregion



                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }


        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            DataTable allfactor = new DataTable();
            try
            {
                if (faDatePickerStrip1.FADatePicker.Text != string.Empty)
                {



                    /* SqlDataAdapter Adapter = new SqlDataAdapter(
                                                                 @"SELECT h.column02 AS column02,h.column03 as unit,
                                                                             tsf.column02 as [date],
                                                                            SUM(h.column07) AS column07 ,tsf.Column42 as ware
                                                                     FROM   Table_011_Child1_SaleFactor h
                                                                            JOIN Table_010_SaleFactor tsf
                                                                                 ON  tsf.columnid = h.column01
                                                                     WHERE  (
                                                                                tsf.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                            )
                                                                         
                                                                            AND (tsf.column17 = 0)--باطل نيست
                                                                            AND (tsf.column19 = 0)--مرجوع نيست
                                                                            AND tsf.Column53 = 0-- بسته نشده
                                                                            AND tsf.Column09=0
                                                                             AND (tsf.Column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                                                             GROUP BY
                                                                            h.column02,h.column03,
                                                                             tsf.column02,tsf.Column42
                                                                         ORDER BY tsf.column02 

                                                                              ", ConSale);
                     CheckHavaleTable = new DataTable();
                     Adapter.Fill(CheckHavaleTable);

                     otherCheckHavaleTable = new DataTable();
                     //if (Convert.ToBoolean(storefactor.Rows[0]["stock"]))
                     //{
                     Adapter = new SqlDataAdapter(
                                                                 @"SELECT h.column02 AS column02,h.column03 as unit,
                                                                             tsf.column02 as [date],
                                                                            SUM(h.column07) AS column07, tsf.Column09,tsf.Column42 as ware
                                                                     FROM   Table_011_Child1_SaleFactor h
                                                                            JOIN Table_010_SaleFactor tsf
                                                                             
                                                                                 ON  tsf.columnid = h.column01
                                                                     WHERE  (
                                                                                tsf.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                            )
                                                                            
                                                                            AND (tsf.column17 = 0)--باطل نيست
                                                                            AND (tsf.column19 = 0)--مرجوع نيست
                                                                            AND tsf.Column53 = 0-- بسته نشده
                                                                            AND tsf.Column09!=0
                                                                             AND (tsf.Column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')

                                                                             GROUP BY
                                                                              h.column02,h.column03,
                                                                             tsf.column02,tsf.Column09,tsf.Column42
                                                                              ORDER BY tsf.column02 
                                                                              ", ConSale);
                     Adapter.Fill(otherCheckHavaleTable);*/
                    //}
                    SqlDataAdapter Adapter = new SqlDataAdapter(
                                                            @"SELECT        columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, 
                         column15, column16, column17, column18, column19, column20, column21, column23, column24, column25, column27, Column37, Column38, Column39, Column40, 
                         Column42, Column43, Column44, Column45, Column49, Column50, Column51, Column53, Column54, Column55, Column56, Column57, Column58, Column59, 
                         Column61, Column62, Column63, Column60, Column64, column22, column26, Column28, Column29, Column30, Column31, Column32, Column33, Column34, 
                         Column35, Column36, Column41, Column46, Column47, Column48, Column52,Column28 - Column29 -Column30 - Column31 + Column32- Column33 as FinalPrice
                                                FROM            Table_010_SaleFactor
                                                WHERE      column02<='" + faDatePickerStrip1.FADatePicker.Text + @"'     and column53=0 
                                                                            ---and column19=0 
                                                                            and column17=0 
                                                                            AND ( Column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')

                                                                             ", ConSale);
                    Adapter.Fill(allfactor);

                    DataTable error = new DataTable();
                    Adapter = new SqlDataAdapter(
                                                           @"SELECT       *
                    FROM            Table_018_MarjooiSale
                    WHERE      column02<='" + faDatePickerStrip1.FADatePicker.Text + @"'     and Column27=0  and (Column10=0 or Column09=0) AND ( Column30=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                                                                                 ", ConSale);
                    Adapter.Fill(error);
                    string error1 = string.Empty;
                    foreach (DataRow dr in error.Rows)
                    {
                        error1 += dr["column01"].ToString() + ",";
                    }
                    if (!string.IsNullOrWhiteSpace(error1))
                    {
                        Class_BasicOperation.ShowMsg("", "فاکتور/های مرجوعی زیر سند/رسید ندارند، برای بستن صندوق ابتدا برای این فاکتورهای مرجوعی صدور سند و رسید انجام دهید" + Environment.NewLine + error1.TrimEnd(','), "Stop");
                        return;

                    }
                    else
                    {
                        gx_factors.DataSource = allfactor;
                        bt_New_Click(null, null);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Frm_029_CloseCash_FormClosing(object sender, FormClosingEventArgs e)
        {
            gx_factors.RemoveFilters();

        }

        private void chehckessentioal()
        {

            discountdt = new DataTable();
            taxdt = new DataTable();
            factordt = new DataTable();
            waredt = new DataTable();
            Sanaddt = new DataTable();
            iddt = new DataTable();
            bahaDT = new DataTable();
            RetutnbahaDT = new DataTable();

            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));
            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));

            if (Class_BasicOperation._FinType)///سیستم دائمی
            {
                SqlDataAdapter Adapter1 = new SqlDataAdapter(
                                                                       @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                    Column14, Column15, Column16
                                                                        FROM            Table_105_SystemTransactionInfo
                                                                        WHERE        (Column00 = 8) ", ConBase);
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

                Adapter1 = new SqlDataAdapter(@" SELECT  
                                                                           tsf.columnid,
                                                                                tsf.column01,
                                                                                tsf.Column09,
                                                                                tsf.Column10 as DocID,
                                                                                tsf.Column44 Project
                                                                    FROM   Table_010_SaleFactor tsf  
                                                                    WHERE   (
                                                                               tsf.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                           )
                                                                          
                                                                           AND (tsf.column17 = 0)--باطل نيست
                                                                           --AND (tsf.column19 = 0)--مرجوع نيست
                                                                           AND tsf.Column53 = 0-- بسته نشده
                                                                           AND (tsf.Column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                                                           order by   tsf.column02,tsf.column14,tsf.columnid", ConSale);





                DataTable arzeshtable2 = new DataTable();
                Adapter1.Fill(arzeshtable2);


                foreach (DataRow dr in arzeshtable2.Rows)
                {

                    All_Controls_Row1(bahaDT.Rows[0]["Column13"].ToString(), null, null, ((dr["Project"] != null && !string.IsNullOrWhiteSpace(dr["Project"].ToString()) ? Convert.ToInt16(dr["Project"]) : (Int16?)null)));
                    All_Controls_Row1(bahaDT.Rows[0]["Column07"].ToString(), null, null, ((dr["Project"] != null && !string.IsNullOrWhiteSpace(dr["Project"].ToString()) ? Convert.ToInt16(dr["Project"]) : (Int16?)null)));
                }



                Adapter1 = new SqlDataAdapter(
                                                                    @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                    Column14, Column15, Column16
                                                                        FROM            Table_105_SystemTransactionInfo
                                                                        WHERE        (Column00 = 14) ", ConBase);
                Adapter1.Fill(RetutnbahaDT);


                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + RetutnbahaDT.Rows[0]["Column13"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("شماره حساب معتبر برای بهای تمام شده مرجوعی فروش را در تنظیمات فروش وارد کنید");


                }



                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + RetutnbahaDT.Rows[0]["Column07"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("شماره حساب معتبر برای بهای تمام شده مرجوعی فروش را در تنظیمات فروش وارد کنید");
                }



                Adapter1 = new SqlDataAdapter(@" SELECT  
                                                                                tsf.columnid,
                                                                                tsf.column01,
                                                                                tsf.Column30 Project
                                                                    FROM   Table_018_MarjooiSale tsf  
                                                                    WHERE   (
                                                                               tsf.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                           )
                                                                           AND tsf.Column27 = 0-- بسته نشده
                                                                           AND (tsf.Column30=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                                                            ", ConSale);





                DataTable Returnarzeshtable2 = new DataTable();
                Adapter1.Fill(Returnarzeshtable2);


                foreach (DataRow dr in Returnarzeshtable2.Rows)
                {

                    All_Controls_Row1(RetutnbahaDT.Rows[0]["Column13"].ToString(), null, null, ((dr["Project"] != null && !string.IsNullOrWhiteSpace(dr["Project"].ToString()) ? Convert.ToInt16(dr["Project"]) : (Int16?)null)));
                    All_Controls_Row1(RetutnbahaDT.Rows[0]["Column07"].ToString(), null, null, ((dr["Project"] != null && !string.IsNullOrWhiteSpace(dr["Project"].ToString()) ? Convert.ToInt16(dr["Project"]) : (Int16?)null)));
                }


            }



            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT DISTINCT tsf.columnid,tsf.Column09,Column10
                                            FROM   Table_010_SaleFactor tsf
                                            WHERE  (tsf.column09 = 0 OR tsf.column10 = 0)
                                                   AND tsf.Column53 = 0-- بسته نشده
                                                  AND (tsf.column17 = 0)--باطل نيست
                                                  AND (tsf.column19 = 0)--مرجوع نيست
                                                   AND tsf.column02 <= N'" + faDatePickerStrip1.FADatePicker.Text + "' AND (tsf.column44=" + projectId + " or '" + (Isadmin) + @"'=N'True') ", ConSale);
            Adapter.Fill(iddt);

            if (iddt.Rows.Count > 0)
            {

                if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 69))
                    throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");
                Adapter = new SqlDataAdapter(
                                                          @"SELECT        isnull(Column02,0) as Column02
                                                                        FROM           Table_030_Setting
                                                                        WHERE        (ColumnId in (45,46)) order by ColumnId  ", ConBase);
                Adapter.Fill(waredt);
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
                            throw new Exception("عملکرد حواله فاکتور فروش انتخاب نشده است");
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
                    throw new Exception("  عملکرد حواله فاکتور فروش تعریف نشده است");


                Adapter = new SqlDataAdapter(
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









                foreach (DataRow idro in iddt.Rows)
                {

                    Sanaddt = new DataTable();
                    factordt = new DataTable();
                    Adapter = new SqlDataAdapter(@"SELECT FactorTable.columnid,
                                               FactorTable.column01,
                                               FactorTable.date,
                                               ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
                                               ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
                                               FactorTable.Bed,
                                               FactorTable.Bes,
                                               FactorTable.NetTotal,FactorTable.person,FactorTable.saletype,FactorTable.ware,FactorTable.func,FactorTable.Project
                                        FROM   (
                                                   SELECT dbo.Table_010_SaleFactor.columnid,
                                                          dbo.Table_010_SaleFactor.column01,
                                                          dbo.Table_010_SaleFactor.column02 AS Date,
                                                          dbo.Table_010_SaleFactor.column03 AS person,
                                                          dbo.Table_010_SaleFactor.column36 AS saletype,
                                                          dbo.Table_010_SaleFactor.column42 AS ware,
                                                          dbo.Table_010_SaleFactor.column43 AS func,



                                                          OtherPrice.PlusPrice AS Ezafat,
                                                          OtherPrice.MinusPrice AS Kosoorat,
                                                          OtherPrice.Bed,
                                                          OtherPrice.Bes,
                                                          dbo.Table_010_SaleFactor.Column28 AS NetTotal,
                                                          dbo.Table_010_SaleFactor.Column44 AS Project

                                                   FROM   dbo.Table_010_SaleFactor
                                                         
                                                          LEFT OUTER JOIN (
                                                                   SELECT columnid,
                                                                          SUM(PlusPrice) AS PlusPrice,
                                                                          SUM(MinusPrice) AS MinusPrice,
                                                                          Bed,
                                                                          Bes
                                                                   FROM   (
                                                                              SELECT Table_010_SaleFactor_2.columnid,
                                                                                     SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                                                     PlusPrice,
                                                                                     0 AS MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = dbo.Table_012_Child2_SaleFactor.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_2
                                                                                          ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                                                              Table_010_SaleFactor_2.columnid
                                                                              WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_2.columnid,
                                                                                     dbo.Table_012_Child2_SaleFactor.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                              UNION ALL
                                                                              SELECT Table_010_SaleFactor_1.columnid,
                                                                                     0 AS PlusPrice,
                                                                                     SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                                                     MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                                                     Table_012_Child2_SaleFactor_1
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = 
                                                                                              Table_012_Child2_SaleFactor_1.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_1
                                                                                          ON  
                                                                                              Table_012_Child2_SaleFactor_1.column01 = 
                                                                                              Table_010_SaleFactor_1.columnid
                                                                              WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_1.columnid,
                                                                                     Table_012_Child2_SaleFactor_1.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                          ) AS OtherPrice_1
                                                                   GROUP BY
                                                                          columnid,
                                                                          OtherPrice_1.Bed,
                                                                          OtherPrice_1.Bes
                                                               ) AS OtherPrice
                                                               ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid where (dbo.Table_010_SaleFactor.column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                               ) AS FactorTable
                                        WHERE  FactorTable.columnid=" + int.Parse(idro["columnid"].ToString()) + @"
                                                                                                           ", ConSale);
                    Adapter.Fill(Sanaddt);


                    Adapter = new SqlDataAdapter(
                                                                    @"SELECT       isnull( column08,'') as Column07,isnull(column14,'') as Column13
                                                                        FROM            Table_002_SalesTypes
                                                                        WHERE        (columnid = " + Sanaddt.Rows[0]["saletype"] + ") ", ConBase);
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
                            throw new Exception("شماره حساب معتبر را در تعریف انواع فروش وارد کنید");


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
                            throw new Exception("شماره حساب معتبر را در تعریف انواع فروش وارد کنید");
                    }
                    TPerson.Rows.Clear();
                    TAccounts.Rows.Clear();
                    //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
                    clDoc.CheckForValidationDate(Sanaddt.Rows[0]["date"].ToString());


                    int LastDocnum = LastDocNum(Sanaddt.Rows[0]["date"].ToString());
                    if (LastDocnum > 0)
                        clDoc.IsFinal(LastDocnum);
                    if (
                   Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()) <= Convert.ToDouble(0)
                   )
                        throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");



                    All_Controls_Row1(factordt.Rows[0]["Column07"].ToString(), int.Parse(Sanaddt.Rows[0]["person"].ToString()), null, ((Sanaddt.Rows[0]["Project"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Project"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Project"]) : (Int16?)null)));
                    All_Controls_Row1(factordt.Rows[0]["Column13"].ToString(), null, null, ((Sanaddt.Rows[0]["Project"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Project"].ToString()) ? Convert.ToInt16(Sanaddt.Rows[0]["Project"]) : (Int16?)null)));
                    TPerson.Rows.Add(Int32.Parse(Sanaddt.Rows[0]["person"].ToString()), factordt.Rows[0]["Column07"].ToString(), Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()));
                    TAccounts.Rows.Add(factordt.Rows[0]["Column13"].ToString(), (-1 * Convert.ToDouble((Sanaddt.Rows[0]["NetTotal"]))));
                    TAccounts.Rows.Add(factordt.Rows[0]["Column07"].ToString(), (Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"])));

                    foreach (DataRow dr in Sanaddt.Rows)
                    {
                        if (dr["ware"] == DBNull.Value || dr["ware"] == null || string.IsNullOrWhiteSpace(dr["ware"].ToString()))
                            throw new Exception("انبار در فاکتور فروش ش " + dr["column01"] + "تعریف نشده است");


                        using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Conacnt.Open();
                            SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + dr["ware"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                            if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0)
                                throw new Exception("برای صدور حواله فاکتور فروش ش " + dr["column01"] + " به انبار انتخاب شده دسترسی ندارید");

                        }

                        Adapter = new SqlDataAdapter(
                                                                  @"SELECT      ff.*,(select column02 from " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients where columnid=ff.column02)as good
                                                                        FROM            Table_011_Child1_SaleFactor ff
                                                                        WHERE        (ff.column01 = " + idro["columnid"] + ") ", ConSale);
                        DataTable gooddt = new DataTable();
                        Adapter.Fill(gooddt);

                        foreach (DataRow gg in gooddt.Rows)
                        {

                            if (gg["good"] == DBNull.Value || gg["good"] == null || string.IsNullOrWhiteSpace(gg["good"].ToString()))
                                throw new Exception("کالا با آی دی " + gg["column02"].ToString() + " وجود ندارد");
                            if (!clGood.IsGoodInWare(Int16.Parse(dr["ware"].ToString()),
                               int.Parse(gg["column02"].ToString())))
                                throw new Exception("کالای " + gg["good"] +
                                    " در انبار فاکتور فروش ش " + dr["column01"] + " فعال نمی باشد");
                        }
                        if (Convert.ToDouble(dr["Ezafat"]) > 0)
                        {
                            All_Controls_Row1(dr["Bed"].ToString(), int.Parse(dr["person"].ToString()), null, ((dr["Project"] != null && !string.IsNullOrWhiteSpace(dr["Project"].ToString()) ? Convert.ToInt16(dr["Project"]) : (Int16?)null)));
                            All_Controls_Row1(dr["Bes"].ToString(), null, null, ((dr["Project"] != null && !string.IsNullOrWhiteSpace(dr["Project"].ToString()) ? Convert.ToInt16(dr["Project"]) : (Int16?)null)));
                            TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Ezafat"])));
                            TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Ezafat"])));
                            TPerson.Rows.Add(Int32.Parse(dr["person"].ToString()), dr["Bed"].ToString(), Convert.ToDouble(dr["Ezafat"]));


                        }
                        if (Convert.ToDouble(dr["Kosoorat"]) > 0)
                        {
                            All_Controls_Row1(dr["Bes"].ToString(), int.Parse(dr["person"].ToString()), null, ((dr["Project"] != null && !string.IsNullOrWhiteSpace(dr["Project"].ToString()) ? Convert.ToInt16(dr["Project"]) : (Int16?)null)));
                            All_Controls_Row1(dr["Bed"].ToString(), null, null, ((dr["Project"] != null && !string.IsNullOrWhiteSpace(dr["Project"].ToString()) ? Convert.ToInt16(dr["Project"]) : (Int16?)null)));
                            TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Kosoorat"])));
                            TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Kosoorat"])));
                            TPerson.Rows.Add(Int32.Parse(dr["person"].ToString()), dr["Bes"].ToString(), Convert.ToDouble(dr["Kosoorat"]));


                        }



                    }




                }
                Classes.CheckCredits clCredit = new Classes.CheckCredits();

                clCredit.CheckAccountCredit(TAccounts, 0);
                clCredit.CheckPersonCredit(TPerson, 0);
                //سند اختتامیه صادر نشده باشد
                clDoc.CheckExistFinalDoc();
            }

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

            if (faDatePickerStrip1.FADatePicker.SelectedDateTime == null)
                throw new Exception("تاتاریخ را وارد کنید");


            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(faDatePickerStrip1.FADatePicker.Text);
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
            else if (e.KeyCode == Keys.F && bt_Search.Enabled)
            {
                bt_Search_Click(sender, e);
            }

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

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {

                ReturnFactors = new DataTable();
                SqlDataAdapter Adapter = new SqlDataAdapter(
                                                       @"SELECT       *
                    FROM            Table_018_MarjooiSale
                    WHERE      column02<='" + faDatePickerStrip1.FADatePicker.Text + @"'     and Column27=0    AND ( Column30=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                                                                                 ", ConSale);
                Adapter.Fill(ReturnFactors);

                if (gx_factors.GetDataRows().Count() <= 0 && ReturnFactors.Rows.Count == 0)
                {


                    txt_FactorsSum.Value = 0;
                    txt_ReturnFactorsSum.Value = 0;
                    txt_NetAmount.Value = 0;
                    txt_CashSale.Value = 0;
                    txt_EtebariSale.Value = 0;
                    txt_CashAcountRemain.Value = 0;
                    txt_CashStockSum.Value = 0;
                    dataSet6.EnforceConstraints = false;
                    this.table_96_CloseCashTableAdapter.FillByID(this.dataSet6.Table_96_CloseCash, -1);
                    this.table_99_LosesFromCashTableAdapter.FillByHeaderID(this.dataSet6.Table_99_LosesFromCash, -1);
                    this.table_98_ReceivedFromBankTableAdapter.FillByHeaderID(this.dataSet6.Table_98_ReceivedFromBank, -1);
                    this.table_97_ReceivedFromCustomersTableAdapter.FillByHeaderID(this.dataSet6.Table_97_ReceivedFromCustomers, -1);
                    dataSet6.EnforceConstraints = true;
                    Class_BasicOperation.ShowMsg("", "فاکتوری برای بستن صندوق وجود ندارد", "Stop");

                    return;
                }
                dataSet6.EnforceConstraints = false;
                this.table_96_CloseCashTableAdapter.FillByID(this.dataSet6.Table_96_CloseCash, -1);
                this.table_99_LosesFromCashTableAdapter.FillByHeaderID(this.dataSet6.Table_99_LosesFromCash, -1);
                this.table_98_ReceivedFromBankTableAdapter.FillByHeaderID(this.dataSet6.Table_98_ReceivedFromBank, -1);
                this.table_97_ReceivedFromCustomersTableAdapter.FillByHeaderID(this.dataSet6.Table_97_ReceivedFromCustomers, -1);
                dataSet6.EnforceConstraints = true;



                table_96_CloseCashBindingSource.AddNew();
                DataRowView Row = (DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current;
                Row["Column01"] = faDatePickerStrip1.FADatePicker.Text;
                Row["Column17"] = Class_BasicOperation._UserName;
                Row["Column18"] = Class_BasicOperation.ServerDate();

                DataTable InfoTable = clDoc.ReturnTable(Properties.Settings.Default.SALE, @"SELECT (
                                                                                           SELECT ISNULL(
                                                                                                      SUM(
                                                                                                          Column28 - Column29 -Column30 - Column31 +
                                                                                                          Column32 - Column33
                                                                                                      ),
                                                                                                      0
                                                                                                  ) AS SalePrice
                                                                                           FROM   Table_010_SaleFactor tsf
                                                                                           WHERE  column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                  AND column53 = 0
                                                                                                  AND column17 = 0
                                                                                                  AND (tsf.column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')  
                                                                                       ) AS txt_FactorsSum,
                                                                                       (
                                                                                           SELECT ISNULL(SUM(tms.Column18 + tms.Column19 -tms.Column20), 0) AS 
                                                                                                  ReturnPrice
                                                                                           FROM   Table_018_MarjooiSale tms
                                                                                           WHERE  tms.column02 <= 
                                                                                                  '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                  AND tms.Column27 = 0 AND (tms.Column30=" + projectId + " or '" + (Isadmin) + @"'=N'True') 
                                                                                       ) AS txt_ReturnFactorsSum,
                                                                                       (
                                                                                           SELECT ISNULL(
                                                                                                      SUM(
                                                                                                          Column28 - Column29 -Column30 - Column31 +
                                                                                                          Column32 - Column33
                                                                                                      ),
                                                                                                      0
                                                                                                  ) -(
                                                                                                      SELECT ISNULL(SUM(tms.Column18 + tms.Column19 -tms.Column20), 0)
                                                                                                      FROM   Table_018_MarjooiSale tms
                                                                                                      WHERE  tms.column02 <= 
                                                                                                             '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                             AND tms.Column27 = 0  AND (tms.Column30=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                                                                                  ) AS NetPrice
                                                                                           FROM   Table_010_SaleFactor tsf
                                                                                           WHERE  column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                  AND column53 = 0
                                                                                                  AND column17 = 0 AND (tsf.column44=" + projectId + " or '" + (Isadmin) + @"'=N'True') 
                                                                                       ) AS txt_NetAmount,
                                                                                       (
                                                                                           SELECT ISNULL(
                                                                                                      SUM(
                                                                                                          Column28 - Column29 -Column30 - Column31 +
                                                                                                          Column32 - Column33
                                                                                                      ),
                                                                                                      0
                                                                                                  )  AS CashSale
                                                                                           FROM   Table_010_SaleFactor tsf
                                                                                                  JOIN " + ConBase.Database + @".dbo.Table_002_SalesTypes tst
                                                                                                       ON  tst.columnid = tsf.Column36
                                                                                           WHERE  tsf.column02 <= 
                                                                                                  '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                  AND tsf.column53 = 0
                                                                                                  AND tsf.column17 = 0
                                                                                                  AND tst.Column21 = 1 AND (tsf.column44=" + projectId + " or '" + (Isadmin) + @"'=N'True') 
                                                                                       ) AS txt_CashSale,
                                                                                       (
                                                                                           SELECT ISNULL(
                                                                                                      SUM(
                                                                                                          Column28 - Column29 -Column30 - Column31 +
                                                                                                          Column32 - Column33
                                                                                                      ),
                                                                                                      0
                                                                                                  ) -(
                                                                                                      SELECT ISNULL(SUM(tms.Column18 + tms.Column19 -tms.Column20), 0) AS 
                                                                                                             ReturnPrice
                                                                                                      FROM   Table_018_MarjooiSale tms
                                                                                                      WHERE  tms.column02 <= 
                                                                                                             '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                             AND tms.Column27 = 0 AND (tms.Column30=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                                                                                  ) AS EtebariSale
                                                                                           FROM   Table_010_SaleFactor tsf
                                                                                                  JOIN " + ConBase.Database + @".dbo.Table_002_SalesTypes tst
                                                                                                       ON  tst.columnid = tsf.Column36
                                                                                           WHERE  tsf.column02 <= 
                                                                                                  '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                  AND tsf.column53 = 0
                                                                                                  AND tsf.column17 = 0
                                                                                                  AND tst.Column21 = 0 AND (tsf.column44=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                                                                       ) AS txt_EtebariSale,
                                                                                        (
                                                                                           SELECT ISNULL(SUM(ISNULL(tsd.Column11, 0) -ISNULL(tsd.Column12, 0)), 0) AS txt_CashAcountRemain
                                                                                          FROM   " + ConAcnt.Database + @".dbo.Table_065_SanadDetail tsd
                                                                                                   JOIN " + ConAcnt.Database + @".dbo.Table_060_SanadHead tsh
                                                                                                        ON  tsh.ColumnId = tsd.Column00
                                                                                        WHERE  tsd.Column01 IN (SELECT column08
                                                                                                                FROM   " + ConBase.Database + @".dbo.Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1)  AND (tsd.column09=" + projectId + " or '" + (Isadmin) + @"'=N'True') AND tsh.Column01 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                       ) AS txt_CashAcountRemain
                                                                                       ");



                if (InfoTable.Rows.Count > 0)
                {
                    txt_FactorsSum.Value = InfoTable.Rows[0]["txt_FactorsSum"];
                    txt_ReturnFactorsSum.Value = InfoTable.Rows[0]["txt_ReturnFactorsSum"];
                    txt_NetAmount.Value = InfoTable.Rows[0]["txt_NetAmount"];
                    txt_CashSale.Value = InfoTable.Rows[0]["txt_CashSale"];
                    txt_EtebariSale.Value = InfoTable.Rows[0]["txt_EtebariSale"];
                    txt_CashAcountRemain.Value = InfoTable.Rows[0]["txt_CashAcountRemain"];
                    txt_CashStockSum.Value = InfoTable.Rows[0]["txt_CashSale"];
                }
                gx_recivefrombank.Enabled = true;
                gx_recivefromcustomer.Enabled = true;
                gx_losefromcash.Enabled = true;
                bt_ExportDoc.Enabled = true;
                bt_Search.Enabled = false;
                table_96_CloseCashBindingSource.EndEdit();

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
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
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column06");
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

                string numsanad = clDoc.ExScalar(ConSale.ConnectionString, @"  SELECT        " + ConAcnt.Database + @".dbo.Table_060_SanadHead.Column00
FROM            dbo.Table_96_CloseCash INNER JOIN
                         " + ConAcnt.Database + @".dbo.Table_060_SanadHead ON dbo.Table_96_CloseCash.Column16 = " + ConAcnt.Database + @".dbo.Table_060_SanadHead.ColumnId
WHERE        (dbo.Table_96_CloseCash.ColumnId = " + txt_Id.Text + ")");
                _05_Sale.Frm_030_CloseCashPrint frm =
                     new Frm_030_CloseCashPrint(faDatePickerStrip1.FADatePicker.Text, ((PSHOP._05_Sale.DataSet6)(table_96_CloseCashBindingSource.DataSource)).Table_96_CloseCash,
                         ((PSHOP._05_Sale.DataSet6)(table_96_CloseCashBindingSource.DataSource)).Table_97_ReceivedFromCustomers,
                         ((PSHOP._05_Sale.DataSet6)(table_96_CloseCashBindingSource.DataSource)).Table_98_ReceivedFromBank,
                         ((PSHOP._05_Sale.DataSet6)(table_96_CloseCashBindingSource.DataSource)).Table_99_LosesFromCash, numsanad);

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
                else
                    bt_Print.Enabled = false;

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

        private void faDatePickerStrip1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip1.FADatePicker.HideDropDown();
                    faDatePickerStrip1.FADatePicker.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }
    }
}
