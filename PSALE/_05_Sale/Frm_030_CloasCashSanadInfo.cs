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
    public partial class Frm_030_CloasCashSanadInfo : Form
    {
        //DataRowView SaleRow;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();

        //int DocNum = 0, DocID = 0;
        //Classes.CheckCredits clCredit = new PACNT.Classes.CheckCredits();
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConBNK = new SqlConnection(Properties.Settings.Default.BANK);
        int ok1 = 0;


        DataTable TAccounts = new DataTable();
        DataTable CloseTable;
        DataTable setting = new DataTable();
        DataTable setting1 = new DataTable();
        DataTable setting2 = new DataTable();
        DataTable setting3 = new DataTable();
        DataTable setting4 = new DataTable();
        DataTable setting5 = new DataTable();


        string Date;
        string _user;
        public Frm_030_CloasCashSanadInfo()
        {
            InitializeComponent();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));
            chk_person.Checked = Properties.Settings.Default.chksanadPerson;

        }
        public Frm_030_CloasCashSanadInfo(DataTable closeTable, string date, DataTable checkTable, string user)
        {
            InitializeComponent();
            chk_person.Checked = Properties.Settings.Default.chksanadPerson;

            CloseTable = closeTable;

            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));
            _user = user;
            try
            {
                ////فروش
                SqlDataAdapter Adapter = new SqlDataAdapter(
                                                                           @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                             Column14, Column15, Column16
                                                                    FROM            Table_105_SystemTransactionInfo
                                                                    WHERE        (Column00 = 4) ", ConBase);
                setting = new DataTable();
                Adapter.Fill(setting);
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting.Rows[0]["Column13"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر برای سرفصل فروش در تنظیمات فروش وارد کنید");
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting.Rows[0]["Column07"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر برای سرفصل فروش در تنظیمات فروش وارد کنید");

                ///صندوق
                Adapter = new SqlDataAdapter(
                                                                            @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                             Column14, Column15, Column16
                                                                    FROM            Table_105_SystemTransactionInfo
                                                                    WHERE        (Column00 = 66) ", ConBase);
                setting1 = new DataTable();
                Adapter.Fill(setting1);




                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting1.Rows[0]["Column07"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر برای سرفصل صندوق در تنظیمات فروش وارد کنید");

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting1.Rows[0]["Column13"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر برای سرفصل صندوق در تنظیمات فروش وارد کنید");

                //////////////سایر
                Adapter = new SqlDataAdapter(
                                                               @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                             Column14, Column15, Column16
                                                                    FROM            Table_105_SystemTransactionInfo
                                                                    WHERE        (Column00 = 67) ", ConBase);
                setting2 = new DataTable();
                Adapter.Fill(setting2);



                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting2.Rows[0]["Column07"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر برای سرفصل اعتباری در تنظیمات فروش وارد کنید");

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting2.Rows[0]["Column13"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر برای سرفصل اعتباری در تنظیمات فروش وارد کنید");
                /////////بن 
                Adapter = new SqlDataAdapter(
                                                             @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                             Column14, Column15, Column16
                                                                    FROM            Table_105_SystemTransactionInfo
                                                                    WHERE        (Column00 = 93) ", ConBase);
                setting5 = new DataTable();
                Adapter.Fill(setting5);



                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting5.Rows[0]["Column07"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر برای سرفصل بن در تنظیمات فروش وارد کنید");

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + setting5.Rows[0]["Column13"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                }

                if (ok1 == 0)
                    throw new Exception("شماره حساب معتبر برای سرفصل بن در تنظیمات فروش وارد کنید");

                /////////تخفیف

                Adapter = new SqlDataAdapter(
                                                            @"SELECT        column10
                                                                    FROM            Table_024_Discount
                                                                    ", ConSale);
                setting3 = new DataTable();
                Adapter.Fill(setting3);


                foreach (DataRow dr in setting3.Rows)
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
                        ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                    }

                    if (ok1 == 0)
                        throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");
                }
                ///////ارزش افزوده
                Adapter = new SqlDataAdapter(
                                                             @"SELECT        column16
                                                                    FROM            Table_024_Discount
                                                                    ", ConSale);
                setting4 = new DataTable();
                Adapter.Fill(setting4);

                foreach (DataRow dr in setting4.Rows)
                {

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
                        ok1 = int.Parse(Comm.ExecuteScalar().ToString());
                    }

                    if (ok1 == 0)
                        throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");
                }



            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
            Date = date;
            SqlDataAdapter ViewAdapter = new SqlDataAdapter("Select * from AllHeaders()", ConAcnt);
            DataTable Headers = new DataTable();
            ViewAdapter.Fill(Headers);
            mlt_ACC.DataSource = Headers;
            mlt_ACC.Value = setting1.Rows[0]["Column07"];
            mlt_ACC.Select();
            mlt_ACC.SelectAll();

        }
        private void txt_To_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            Class_BasicOperation.isEnter(e.KeyChar);
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
        private void txt_Cover_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is FarsiLibrary.Win.Controls.FADatePicker)
            {
                if (Class_BasicOperation.isNotDigit(e.KeyChar))
                    e.Handled = true;
                if (e.KeyChar == 8)
                    _BackSpace = true;
                else
                    _BackSpace = false;
                if (e.KeyChar == 13)
                {
                    Class_BasicOperation.isEnter(e.KeyChar);

                }
            }
        }
        private float FirstRemain(int GoodCode, string ware, string date)
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
                CommandText = string.Format(CommandText, ware, GoodCode, date);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }
        private void Btn_Save_Click(object sender, EventArgs e)
        {
            //*********Just Accounting Document
            try
            {
                if (ok1 == 1)
                {


                    if (mlt_ACC.Value != null && !string.IsNullOrWhiteSpace(mlt_ACC.Value.ToString()))
                    {
                        int ok45 = 0;
                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Con.Open();
                            SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + mlt_ACC.Value.ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                            ok45 = int.Parse(Comm.ExecuteScalar().ToString());
                        }
                        if (ok45 == 0)
                        {
                            MessageBox.Show("شماره حساب معتبر برای سرفصل صندوق انتخاب کنید");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("شماره حساب معتبر برای سرفصل صندوق انتخاب کنید");
                        return;
                    }

                    string Message = "آیا مایل به صدور سند حسابداری و حواله انبار هستید؟";

                    if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {

                        CheckEssentialItems(sender, e);

                        if (ok1 == 1)
                        {
                            Btn_Save.Enabled = false;
                            DataTable he = new DataTable();

                            //برای فاکتورهایی که تحویل کالا نداشته اند حواله صادر نمی شود
                            SqlDataAdapter ag = new SqlDataAdapter(
                                                              @"  SELECT Table_010_SaleFactor.column02 AS date,
                                                                                Table_010_SaleFactor.column42 as column40,
                                                                                Table_010_SaleFactor.Column03,
                                                                                Table_010_SaleFactor.ColumnId
                                                                            FROM   Table_010_SaleFactor
                                                                                   
                                                                            WHERE  (
                                                                                       Table_010_SaleFactor.column02 <=
                                                                                       N'" + Date + @"'
                                                                                   )
                                                                                   AND (
                                                                                           Table_010_SaleFactor.column13 =
                                                                                           N'" + _user + @"'
                                                                                       )
                                                                                    
                                                                                 AND (Table_010_SaleFactor.column17 = 0)--باطل نيست
                                                                                   AND (Table_010_SaleFactor.column19 = 0)--مرجوع نيست
                                                                                   AND Table_010_SaleFactor.Column45 = 1--نسويه شده
                                                                                   AND Table_010_SaleFactor.Column53 = 0-- بسته نشده
                                                                                   AND  Table_010_SaleFactor.Column50=1--تحویل داده شده 
                                                                                   AND  Table_010_SaleFactor.column09=0---حواله نخورده
                                                                                    GROUP BY
                                                                                   Table_010_SaleFactor.column02,
                                                                                    Table_010_SaleFactor.column42,
                                                                                    Table_010_SaleFactor.Column03,Table_010_SaleFactor.ColumnId", ConSale);

                            ag.Fill(he);


                            foreach (DataRow item10 in he.Rows)
                            {


                                int DraftID = 0, DraftNum = 0;

                                //درج هدر حواله
                                SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                                Key.Direction = ParameterDirection.Output;
                                DraftNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01");
                                //, int.Parse(mlt_Ware.Value.ToString()));
                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                                {
                                    Con.Open();
                                    SqlCommand InsertHeader = new SqlCommand(@"INSERT INTO Table_007_PwhrsDraft([column01]
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
                                                                                               ,[Column26]) VALUES(" + DraftNum + ",'" + item10["date"].ToString() + "'," + item10["Column40"].ToString()
                                        + ",2, " + item10["Column03"].ToString() + ",'" + "حواله صادره بابت فاکتور فروش تاریخ" + item10["date"].ToString() +
                                        "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + item10["ColumnId"].ToString() + ",0,0,0,0,0,0,0,null,0,0); SET @Key=SCOPE_IDENTITY()", Con);
                                    InsertHeader.Parameters.Add(Key);
                                    InsertHeader.ExecuteNonQuery();
                                    DraftID = int.Parse(Key.Value.ToString());
                                    //  clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", int.Parse(item["ColumnId"].ToString()), DraftID);







                                    SqlDataAdapter Adapter = new SqlDataAdapter(
                                                                            @"SELECT   
                                                                       [column02]
                                                                      ,[column03]
                                                                      ,sum([column04]) as column04
                                                                      ,sum([column05]) as column05
                                                                      ,sum([column06]) as column06
                                                                      ,sum([column07]) as column07
                                                                      ,sum([column08]) as column08
                                                                      ,sum([column09]) as column09
                                                                      ,sum([column10]) as column10
                                                                      ,sum([column11]) as column11
                                                                      ,sum([column12]) as column12
                                                                      ,sum([column13]) as column13
                                                                      ,sum([column15]) as column15
                                                                      ,sum([column16]) as column16
                                                                      ,sum([column17]) as column17
                                                                      ,sum([column18]) as column18
                                                                      ,sum([column19]) as column19
                                                                      ,sum([column20]) as column20
                                                                      ,sum([Column31]) as column31
                                                                      ,sum([Column32]) as column32
                                                                      ,sum([Column33]) as column33,Column30
                                                                    ,Column34,Column35,sum(Column36) as Column36,sum(Column37) Column37 
                                                                  FROM  [dbo].[Table_011_Child1_SaleFactor] WHERE column01 IN (SELECT Table_010_SaleFactor.ColumnId  
                                                                        
                                                                                                                                                    FROM   Table_010_SaleFactor
                                                                                                                                                    WHERE  (
                                                                                                                                                               Table_010_SaleFactor.column02 =
                                                                                                                                                               '" + item10["date"].ToString() + @"'
                                                                                                                                                           )
                                                                                                                                                           AND (
                                                                                                                                                                   Table_010_SaleFactor.column13 =
                                                                                                                                                                   '" + _user + @"'
                                                                                                                                                               )
                                                                                                                                                            AND     Table_010_SaleFactor.column03=   " + item10["Column03"].ToString() + @" 
                                                                                                                                                             AND  Table_010_SaleFactor.ColumnId= " + item10["ColumnId"].ToString() + @"
                                                                                                                                                           AND (Table_010_SaleFactor.column17 = 0)--باطل نيست
                                                                                                                                                           AND (Table_010_SaleFactor.column19 = 0)--مرجوع نيست
                                                                                                                                                           AND Table_010_SaleFactor.Column45 = 1--نسويه شده
                                                                                                                                                           AND Table_010_SaleFactor.Column53 = 0-- بسته نشده
                                                                                                                                                           AND  Table_010_SaleFactor.Column50=1--تحویل داده شده 
                                                                                                                                                               AND  Table_010_SaleFactor.column09=0---حواله نخورده
                                                                                                                                                            )     GROUP BY   column02  ,column03 ,Column34  ,Column35  ,Column30                                                                                                                                              
                                                                                                                                                            ", ConSale);
                                    DataTable Child1 = new DataTable();
                                    Adapter.Fill(Child1);



                                    //درج کالاهای موجود در حواله 
                                    foreach (DataRow item1 in Child1.Rows)
                                    {
                                        if (clDoc.IsGood(item1["Column02"].ToString()))
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
           ,[Column35]) VALUES(" + DraftID + "," + item1["Column02"].ToString() + "," +
                                                  clDoc.ExScalar(ConWare.ConnectionString,
                                        "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                        item1["Column02"].ToString()) + "," + item1["Column04"].ToString() + "," + item1["Column05"].ToString() + "," + item1["Column07"].ToString() + "," +
                                                item1["Column07"].ToString() + "," + item1["Column08"].ToString() + "," + item1["Column09"].ToString() + "," + item1["Column10"].ToString() + "," +
                                                item1["Column11"].ToString() + ",NULL,NULL, NULL ,0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL, NULL ," +
                                                item1["Column15"].ToString() +
                                                    ",0,0,0,0," + (item1["Column30"].ToString() == "True" ? "1" : "0") + "," +
                                                    (item1["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column34"].ToString() + "'") + "," +
                                                    (item1["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column35"].ToString() + "'")
                                                    + "," + item1["Column31"].ToString()
                                                    + "," + item1["Column32"].ToString() + "," + item1["Column36"].ToString() + "," + item1["Column37"].ToString() + ")", Con);
                                            InsertDetail.ExecuteNonQuery();
                                        }

                                    }
                                    try
                                    {
                                        SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + DraftID, ConWare);
                                        DataTable Table = new DataTable();
                                        goodAdapter.Fill(Table);

                                        //محاسبه ارزش و ذخیره آن در جدول Child1

                                        foreach (DataRow item2 in Table.Rows)
                                        {
                                            if (Class_BasicOperation._WareType)
                                            {
                                                Adapter = new SqlDataAdapter("EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + item10["Column40"].ToString(), Con);
                                                DataTable TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftID + " and DetailID=" + item2["Columnid"].ToString());
                                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                                    + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con);
                                                UpdateCommand.ExecuteNonQuery();
                                            }

                                            else
                                            {
                                                Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + item10["Column40"].ToString() + ",@Date='" + item10["date"].ToString() + "',@id=" + DraftID + ",@residid=0", ConWare);
                                                DataTable TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                              + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con);
                                                UpdateCommand.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                    catch
                                    { }

                                    using (SqlConnection Con1 = new SqlConnection(ConSale.ConnectionString))
                                    {
                                        Con1.Open();
                                        SqlCommand Update = new SqlCommand(@"UPDATE Table_010_SaleFactor SET Column09=" + DraftID + @",Column15='" + Class_BasicOperation._UserName + @"',Column16=getdate() where ColumnId IN (SELECT Table_010_SaleFactor.ColumnId  
                                                                        
                                                                                                                                                    FROM   Table_010_SaleFactor
                                                                                                                                                    WHERE  (
                                                                                                                                                               Table_010_SaleFactor.column02 =
                                                                                                                                                               '" + item10["date"].ToString() + @"'
                                                                                                                                                           )
                                                                                                                                                           AND (
                                                                                                                                                                   Table_010_SaleFactor.column13 =
                                                                                                                                                                   '" + _user + @"'
                                                                                                                                                               )
                                                                                                                                                             AND     Table_010_SaleFactor.column03=   " + item10["Column03"].ToString() + @" 
                                                                                                                                                            AND  Table_010_SaleFactor.ColumnId= " + item10["ColumnId"].ToString() + @"
                                                                                                                                                           AND (Table_010_SaleFactor.column17 = 0)--باطل نيست
                                                                                                                                                           AND (Table_010_SaleFactor.column19 = 0)--مرجوع نيست
                                                                                                                                                           AND Table_010_SaleFactor.Column45 = 1--نسويه شده
                                                                                                                                                           AND Table_010_SaleFactor.Column53 = 0-- بسته نشده
                                                                                                                                                             AND  Table_010_SaleFactor.column09=0---حواله نخورده
                                                                                                                                                            AND  Table_010_SaleFactor.Column50=1)--تحویل داده شده ", Con1);
                                        Update.ExecuteNonQuery();
                                    }

                                }

                            }
                            he = new DataTable();
                            //برای فاکتورهایی که تحویل کالا نداشته اند حواله صادر نمی شود
                            ag = new SqlDataAdapter(
                                                            @"  SELECT Table_010_SaleFactor.column02 AS date,
                                                                                Table_010_SaleFactor.column42 as column40,
                                                                                Table_010_SaleFactor.Column03
                                                                            FROM   Table_010_SaleFactor
                                                                                   
                                                                            WHERE  (
                                                                                       Table_010_SaleFactor.column02 <=
                                                                                       N'" + Date + @"'
                                                                                   )
                                                                                   AND (
                                                                                           Table_010_SaleFactor.column13 =
                                                                                           N'" + _user + @"'
                                                                                       )
                                                                                    
                                                                                 AND (Table_010_SaleFactor.column17 = 0)--باطل نيست
                                                                                   AND (Table_010_SaleFactor.column19 = 0)--مرجوع نيست
                                                                                   AND Table_010_SaleFactor.Column45 = 1--نسويه شده
                                                                                   AND Table_010_SaleFactor.Column53 = 0-- بسته نشده
                                                                                  
                                                                                   GROUP BY
                                                                                   Table_010_SaleFactor.column02,
                                                                                    Table_010_SaleFactor.column42,
                                                                                    Table_010_SaleFactor.Column03", ConSale);

                            ag.Fill(he);


                            foreach (DataRow item6 in he.Rows)
                            {

                                //صدور سند

                                string headercomman = string.Empty;
                                SqlParameter DocNum;
                                DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                                DocNum.Direction = ParameterDirection.Output;
                                SqlParameter DocID;
                                DocID = new SqlParameter("DocID", SqlDbType.Int);
                                DocID.Direction = ParameterDirection.Output;

                                headercomman = @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + item6["date"].ToString() + "',2,0,N'بستن صندوق فاکتور فروش فروشگاه " + item6["date"].ToString() + "','" + Class_BasicOperation._UserName +
                       "',getdate()); SET @DocID=SCOPE_IDENTITY()";

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

                                }

                                //DocNum = clDoc.LastDocNum() + 1;

                                //DocID = clDoc.ExportDoc_Header(DocNum, item6["date"].ToString(), " بستن صندوق فاکتور فروش فروشگاه " + item6["date"].ToString() + "", Class_BasicOperation._UserName);

                                if (Convert.ToInt32(DocID.Value) > 0)
                                {

                                    if (!chk_person.Checked)
                                    {
                                        SqlDataAdapter Adapter = new SqlDataAdapter(@"
                                                                                        
                                                                    SELECT DISTINCT FactorTable.columnid, FactorTable.column01, FactorTable.date,
                                                                           SUM(FactorTable.NetTotal) AS NetTotal,
                                                                           SUM(FactorTable. Column46) AS naghd,
                                                                           SUM(FactorTable. Column47) AS cart,
                                                                           SUM(FactorTable. Column48) AS sayer,
                                                                           FactorTable.hesab,
                                                                           FactorTable.Column42 AS Column40,
                                                                           NULL AS column03,
                                                                           SUM(FactorTable.Column52) AS [check],
                                                                           SUM(FactorTable.Column54) AS [bon],

                                                                           --FactorTable.Column50 AS Delivary,
                                                                           FactorTable.column10 AS sanad
                                                                    FROM   (
                                                                               SELECT dbo.Table_010_SaleFactor.columnid,
                                                                                      dbo.Table_010_SaleFactor.column01,
                                                                                      dbo.Table_010_SaleFactor.column02 AS Date,
                                                                                      dbo.Table_010_SaleFactor.Column28 AS NetTotal,
                                                                                      Table_010_SaleFactor. Column46,
                                                                                      Table_010_SaleFactor. Column47,
                                                                                      Table_010_SaleFactor. Column48,
                                                                                      (
                                                                                          SELECT ISNULL(
                                                                                                     (
                                                                                                         SELECT tbcai.Column12
                                                                                                         FROM   " + ConBNK.Database + @".dbo.Table_020_BankCashAccInfo 
                                                                                                                tbcai
                                                                                                         WHERE  tbcai.columnid = CAST(Table_010_SaleFactor.Column49 AS SMALLINT)
                                                                                                     ),
                                                                                                     ''
                                                                                                 )
                                                                                      ) AS hesab,
                                                                                      Table_010_SaleFactor. Column42,
                                                                                      Table_010_SaleFactor. column03,
                                                                                      Table_010_SaleFactor. column13,
                                                                                      Table_010_SaleFactor. column10,
                                                                                      Table_010_SaleFactor. column17,
                                                                                      Table_010_SaleFactor. column09,
                                                                                      Table_010_SaleFactor. column19,
                                                                                      Table_010_SaleFactor. Column45,
                                                                                      Table_010_SaleFactor. Column52,
                                                                                      Table_010_SaleFactor. Column54,

                                                                                      Table_010_SaleFactor.Column53,
                                                                                      Table_010_SaleFactor.Column50
                                                                               FROM   dbo.Table_010_SaleFactor
                                                                           ) AS FactorTable
                                                                    WHERE  (FactorTable.Date <= '" + item6["date"].ToString() + @"')
                                                                           AND (FactorTable. column13 = '" + _user + @"')
                                                                           AND FactorTable.column03 = " + item6["Column03"].ToString() + @"
                                                                           AND (FactorTable.column17 = 0)--باطل نيست
                                                                           AND (FactorTable.column19 = 0)--مرجوع نيست
                                                                           AND FactorTable.Column45 = 1--نسويه شده
                                                                           AND FactorTable.Column53 = 0-- بسته نشده
                                                                           AND FactorTable.Column48 = 0
                                                                           AND FactorTable.Column52 = 0
                                                                           AND FactorTable.Column54 = 0

                                                                    GROUP BY
                                                                           FactorTable.date,
                                                                           FactorTable.column03,
                                                                           FactorTable.column10,
                                                                           FactorTable.hesab,
                                                                           FactorTable. Column42,
                                                                          -- FactorTable.Column50,
                                                                           FactorTable.Column10,
                                                                            FactorTable.columnid , FactorTable.column01
                                                                    HAVING SUM(FactorTable. Column48) = 0 AND SUM(FactorTable.Column52) = 0 AND SUM(FactorTable.Column54) = 0
                                                                    UNION  ALL                                                                                        
                                                                    

                                                                    SELECT DISTINCT   FactorTable.columnid, FactorTable.column01,FactorTable.date,
                                                                           SUM(FactorTable.NetTotal) AS NetTotal,
                                                                           SUM(FactorTable. Column46) AS naghd,
                                                                           SUM(FactorTable. Column47) AS cart,
                                                                           SUM(FactorTable. Column48) AS sayer,
                                                                           FactorTable.hesab,
                                                                           FactorTable. Column42 AS Column40,
                                                                           FactorTable. column03,
                                                                           SUM(FactorTable.Column52) AS [check],
                                                                           SUM(FactorTable.Column54) AS [bon],

                                                                         --  FactorTable.Column50 AS Delivary,
                                                                           FactorTable.column10 AS sanad
                                                                    FROM   (
                                                                               SELECT dbo.Table_010_SaleFactor.columnid,
                                                                                      dbo.Table_010_SaleFactor.column01,
                                                                                      dbo.Table_010_SaleFactor.column02 AS Date,
                                                                                      dbo.Table_010_SaleFactor.Column28 AS NetTotal,
                                                                                      Table_010_SaleFactor. Column46,
                                                                                      Table_010_SaleFactor. Column47,
                                                                                      Table_010_SaleFactor. Column48,
                                                                                      (
                                                                                          SELECT ISNULL(
                                                                                                     (
                                                                                                         SELECT tbcai.Column12
                                                                                                         FROM   " + ConBNK.Database + @".dbo.Table_020_BankCashAccInfo 
                                                                                                                tbcai
                                                                                                         WHERE  tbcai.columnid = CAST(Table_010_SaleFactor.Column49 AS SMALLINT)
                                                                                                     ),
                                                                                                     ''
                                                                                                 )
                                                                                      ) AS hesab,
                                                                                      Table_010_SaleFactor. Column42,
                                                                                      Table_010_SaleFactor. column03,
                                                                                      Table_010_SaleFactor. column13,
                                                                                      Table_010_SaleFactor. column10,
                                                                                      Table_010_SaleFactor. column17,
                                                                                      Table_010_SaleFactor. column09,
                                                                                      Table_010_SaleFactor. column19,
                                                                                      Table_010_SaleFactor. Column45,
                                                                                      Table_010_SaleFactor. Column52,
                                                                                      Table_010_SaleFactor. Column54,

                                                                                      Table_010_SaleFactor.Column53,
                                                                                      Table_010_SaleFactor.Column50
                                                                               FROM   dbo.Table_010_SaleFactor
                                                                           ) AS FactorTable
                                                                    WHERE  (FactorTable.Date <= '" + item6["date"].ToString() + @"')
                                                                           AND (FactorTable. column13 = '" + _user + @"')
                                                                           AND FactorTable.column03 = " + item6["Column03"].ToString() + @"
                                                                           AND (FactorTable.column17 = 0)--باطل نيست
                                                                           AND (FactorTable.column19 = 0)--مرجوع نيست
                                                                           AND FactorTable.Column45 = 1--نسويه شده
                                                                           AND FactorTable.Column53 = 0-- بسته نشده
                                                                           AND (FactorTable.Column48 > 0
                                                                           or FactorTable.Column52 > 0 or FactorTable.Column54 > 0)
                                                                    GROUP BY
                                                                           FactorTable.date,
                                                                           FactorTable.column03,
                                                                           FactorTable.column10,
                                                                           FactorTable.hesab,
                                                                           FactorTable. Column42,
                                                                          -- FactorTable.Column50,
                                                                           FactorTable.Column10,
                                                                           FactorTable.columnid , FactorTable.column01
                                                                    HAVING SUM(FactorTable. Column48) > 0 or SUM(FactorTable.Column52) > 0 or SUM(FactorTable.Column54) > 0

                                                                 


                                                                             ", ConSale);
                                        CloseTable = new DataTable();
                                        Adapter.Fill(CloseTable);


                                        foreach (DataRow item in this.CloseTable.Rows)
                                        {
                                            if (Convert.ToInt32(DocID.Value) > 0)
                                            {

                                                ////فروش

                                                if (Convert.ToInt32(item["sanad"]) == 0)
                                                {
                                                    string[] _AccInfo = clDoc.ACC_Info(setting.Rows[0]["Column13"].ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting.Rows[0]["Column13"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , null,
                                                                         null,
                                                                        null,
                                                                       "بستن صندوق فروش- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),
                                                                        0,
                                                                     Convert.ToInt64(Math.Round(Convert.ToDouble(item["NetTotal"].ToString()), 3))
                                                                        ,
                                                                         0,
                                                                      0,
                                                                       -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                       0, (short?)null);
                                                }
                                                ////صندوق
                                                if (item["naghd"] != null && item["naghd"].ToString() != string.Empty && Convert.ToDouble(item["naghd"]) > 0)
                                                {

                                                    string[] _AccInfo = clDoc.ACC_Info(this.mlt_ACC.Value.ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), mlt_ACC.Value.ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , null,
                                                                         null,
                                                                        null,
                                                                       "بستن صندوق فروش-دریافت نقد- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),
                                                                      Convert.ToInt64(Math.Round(Convert.ToDouble(item["naghd"].ToString()), 3))
                                                                       , 0
                                                                       , 0,
                                                                      0,
                                                                     -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                       0, (short?)null);
                                                }
                                                // سایر
                                                if (item["sayer"] != null && item["sayer"].ToString() != string.Empty && Convert.ToDouble(item["sayer"]) > 0)
                                                {


                                                    string[] _AccInfo = clDoc.ACC_Info(setting2.Rows[0]["Column07"].ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting2.Rows[0]["Column07"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , item["Column03"].ToString(),
                                                                         null,
                                                                        null,
                                                                       "بستن صندوق فروش-اعتباری- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),
                                                                      Convert.ToInt64(Math.Round(Convert.ToDouble(item["sayer"].ToString()), 3))
                                                                       , 0
                                                                       , 0,
                                                                      0,
                                                                      -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                    0, (short?)null);
                                                }//چک
                                                if (item["check"] != null && item["check"].ToString() != string.Empty && Convert.ToDouble(item["check"]) > 0)
                                                {


                                                    string[] _AccInfo = clDoc.ACC_Info(setting2.Rows[0]["Column07"].ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting2.Rows[0]["Column07"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , item["Column03"].ToString(),
                                                                         null,
                                                                        null,
                                                                       "بستن صندوق فروش-چک- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),
                                                                      Convert.ToInt64(Math.Round(Convert.ToDouble(item["check"].ToString()), 3))
                                                                       , 0
                                                                       , 0,
                                                                      0,
                                                                      -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                    0, (short?)null);
                                                }
                                                //بن
                                                if (item["bon"] != null && item["bon"].ToString() != string.Empty && Convert.ToDouble(item["bon"]) > 0)
                                                {


                                                    string[] _AccInfo = clDoc.ACC_Info(setting5.Rows[0]["Column07"].ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting5.Rows[0]["Column07"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , item["Column03"].ToString(),
                                                                         null,
                                                                        null,
                                                                       "بستن صندوق فروش-بن- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),
                                                                      Convert.ToInt64(Math.Round(Convert.ToDouble(item["bon"].ToString()), 3))
                                                                       , 0
                                                                       , 0,
                                                                      0,
                                                                      -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                    0, (short?)null);
                                                }

                                                //کارت
                                                if (item["cart"] != null && item["cart"].ToString() != string.Empty && Convert.ToDouble(item["cart"]) > 0)
                                                {
                                                    string[] _AccInfo = clDoc.ACC_Info(item["hesab"].ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), item["hesab"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , null,
                                                                         null,
                                                                        null,
                                                                       "بستن صندوق فروش-کارت- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),
                                                                      Convert.ToInt64(Math.Round(Convert.ToDouble(item["cart"].ToString()), 3))
                                                                       , 0
                                                                       , 0,
                                                                      0,
                                                                      -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                      0, (short?)null);
                                                }





                                            }
                                        }

                                        Adapter = new SqlDataAdapter(@"
                                                                                       
                                                                            SELECT distinct FactorTable.Date,
                                                                                   SUM(ISNULL(FactorTable.Ezafat, 0)) AS Ezafat,
                                                                                   SUM(ISNULL(FactorTable.Kosoorat, 0)) AS Kosoorat,
                                                                                   FactorTable.column16 AS bestankar,
                                                                                   FactorTable.bedehkar,
                                                                                   FactorTable.columnid
                                                                            FROM   (
                                                                                       SELECT dbo.Table_010_SaleFactor.column02 AS Date,
                                                                                              OtherPrice.PlusPrice AS Ezafat,
                                                                                              OtherPrice.MinusPrice AS Kosoorat,
                                                                                              Table_010_SaleFactor. column13,
                                                                                              Table_010_SaleFactor. column10,
                                                                                              Table_010_SaleFactor. column17,
                                                                                              Table_010_SaleFactor. column09,
                                                                                              Table_010_SaleFactor. column19,
                                                                                              Table_010_SaleFactor. Column45,
                                                                                                Table_010_SaleFactor. Column53,
                                                                                              otherprice.column16,
                                                                                              otherprice.column10 AS bedehkar,
                                                                                                Table_010_SaleFactor. column03,Table_010_SaleFactor.columnid
                                                                                       FROM   dbo.Table_010_SaleFactor
                                                                                             
                                                                                              LEFT OUTER JOIN (
                                                                                                       SELECT columnid,
                                                                                                              SUM(PlusPrice) AS PlusPrice,
                                                                                                              SUM(MinusPrice) AS MinusPrice,
                                                                                                              column16,
                                                                                                              column10
                                                                                                       FROM   (
                                                                                                                  SELECT Table_010_SaleFactor_2.columnid,
                                                                                                                         SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                                                                                         PlusPrice,
                                                                                                                         0 AS MinusPrice,
                                                                                                                         td.column16 AS column16,
                                                                                                                         NULL AS column10
                                                                                                                  FROM   dbo.Table_012_Child2_SaleFactor
                                                                                                                         INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                                                              Table_010_SaleFactor_2
                                                                                                                              ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                                                                                                  Table_010_SaleFactor_2.columnid
                                                                                                                         JOIN Table_024_Discount td
                                                                                                                              ON  td.columnid = dbo.Table_012_Child2_SaleFactor.column02
                                                                                                                  WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                                                                  GROUP BY
                                                                                                                         Table_010_SaleFactor_2.columnid,
                                                                                                                         dbo.Table_012_Child2_SaleFactor.column05,
                                                                                                                         td.column16
                                                                                                                  UNION ALL
                                                                                                                  SELECT Table_010_SaleFactor_1.columnid,
                                                                                                                         0 AS PlusPrice,
                                                                                                                         SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                                                                                         MinusPrice,
                                                                                                                         NULL AS column16,
                                                                                                                         td.column10 AS column10
                                                                                                                  FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                                                                                         Table_012_Child2_SaleFactor_1
                                                                                                                         INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                                                              Table_010_SaleFactor_1
                                                                                                                              ON  
                                                                                                                                  Table_012_Child2_SaleFactor_1.column01 = 
                                                                                                                                  Table_010_SaleFactor_1.columnid
                                                                                                                         JOIN Table_024_Discount td
                                                                                                                              ON  td.columnid = 
                                                                                                                                  Table_012_Child2_SaleFactor_1.column02
                                                                                                                  WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                                                                  GROUP BY
                                                                                                                         Table_010_SaleFactor_1.columnid,
                                                                                                                         Table_012_Child2_SaleFactor_1.column05,
                                                                                                                         td.column10
                                                                                                              ) AS OtherPrice_1
                                                                                                       GROUP BY
                                                                                                              columnid,
                                                                                                              column10,
                                                                                                              column16
                                                                                                   ) AS OtherPrice
                                                                                                   ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid
                                                                                   ) AS FactorTable
                                                                            WHERE  (
                                                                                       FactorTable.Date <= '" + item6["date"].ToString() + @"'
                                                                                   )
                                                                                   AND (
                                                                                           FactorTable. column13 =
                                                                                           '" + _user + @"'
                                                                                       )
                                                                                 AND     FactorTable.column03=   " + item6["Column03"].ToString() + @" 
                                                                                   AND (FactorTable.column10 = 0)--فاکتورهایی که سندشون صفر برای مشخص شدن مغایرت
                                                                                  AND (FactorTable.column17 = 0)--باطل نيست
                                                                                   AND (FactorTable.column19 = 0)--مرجوع نيست
                                                                                   AND FactorTable.Column45 = 1--نسويه شده
                                                                                   AND FactorTable.Column53 = 0-- بسته نشده
                                                                                   
                                                                                    
                                                                            GROUP BY
                                                                                   FactorTable.date,
                                                                                   FactorTable.column10,
                                                                                   FactorTable.column16,
                                                                                   FactorTable.bedehkar,FactorTable.columnid
 

                                                                             ", ConSale);
                                        DataTable takhfif = new DataTable();
                                        Adapter.Fill(takhfif);


                                        foreach (DataRow dr in takhfif.Rows)
                                        {
                                            ///تخفیف
                                            ///
                                            if (dr["Kosoorat"] != null && dr["Kosoorat"].ToString() != string.Empty && Convert.ToDouble(dr["Kosoorat"]) > 0)
                                            {

                                                string[] _AccInfo = clDoc.ACC_Info(dr["bedehkar"].ToString());
                                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), dr["bedehkar"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                   _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                   , null,
                                                                     null,
                                                                    null,
                                                                   "بستن صندوق فروش-تخفیف-فاکتور فروش تاریخ" + item6["date"].ToString(),
                                                                  Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3))
                                                                   , 0
                                                                   , 0,
                                                                  0,
                                                                   -1,
                                                                      15, Convert.ToInt32(dr["columnid"]), Class_BasicOperation._UserName,
                                                                   0, (short?)null);
                                            }
                                            //ارزش افزوده
                                            if (dr["Ezafat"] != null && dr["Ezafat"].ToString() != string.Empty && Convert.ToDouble(dr["Ezafat"]) > 0)
                                            {


                                                string[] _AccInfo = clDoc.ACC_Info(dr["bestankar"].ToString());
                                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), dr["bestankar"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                   _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                   , null,
                                                                     null,
                                                                    null,
                                                                   "بستن صندوق فروش-ارزش افزوده-فاکتور فروش تاریخ" + item6["date"].ToString(),
                                                                     0
                                                                 , Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3))
                                                                   , 0,
                                                                  0,
                                                                  -1,
                                                                      15, Convert.ToInt32(dr["columnid"]), Class_BasicOperation._UserName,
                                                                   0, (short?)null);
                                            }

                                        }
                                    }
                                    ////////////////////////////////////////////به تفکیک شخص
                                    else
                                    {
                                        SqlDataAdapter Adapter = new SqlDataAdapter(@"

                                                                    SELECT DISTINCT   FactorTable.columnid,FactorTable.column01 , FactorTable.date,
                                                                           SUM(FactorTable.NetTotal) AS NetTotal,
                                                                           SUM(FactorTable. Column46) AS naghd,
                                                                           SUM(FactorTable. Column47) AS cart,
                                                                           SUM(FactorTable. Column48) AS sayer,
                                                                           FactorTable.hesab,
                                                                           FactorTable. Column42 AS Column40,
                                                                           FactorTable. column03,
                                                                           SUM(FactorTable.Column52) AS [check],
                                                                           SUM(FactorTable.Column54) AS [bon],

                                                                          -- FactorTable.Column50 AS Delivary,
                                                                           FactorTable.column10 AS sanad
                                                                    FROM   (
                                                                               SELECT dbo.Table_010_SaleFactor.columnid,
                                                                                      dbo.Table_010_SaleFactor.column01,
                                                                                      dbo.Table_010_SaleFactor.column02 AS Date,
                                                                                      dbo.Table_010_SaleFactor.Column28 AS NetTotal,
                                                                                      Table_010_SaleFactor. Column46,
                                                                                      Table_010_SaleFactor. Column47,
                                                                                      Table_010_SaleFactor. Column48,
                                                                                      (
                                                                                          SELECT ISNULL(
                                                                                                     (
                                                                                                         SELECT tbcai.Column12
                                                                                                         FROM   " + ConBNK.Database + @".dbo.Table_020_BankCashAccInfo 
                                                                                                                tbcai
                                                                                                         WHERE  tbcai.columnid = CAST(Table_010_SaleFactor.Column49 AS SMALLINT)
                                                                                                     ),
                                                                                                     ''
                                                                                                 )
                                                                                      ) AS hesab,
                                                                                      Table_010_SaleFactor. Column42,
                                                                                      Table_010_SaleFactor. column03,
                                                                                      Table_010_SaleFactor. column13,
                                                                                      Table_010_SaleFactor. column10,
                                                                                      Table_010_SaleFactor. column17,
                                                                                      Table_010_SaleFactor. column09,
                                                                                      Table_010_SaleFactor. column19,
                                                                                      Table_010_SaleFactor. Column45,
                                                                                      Table_010_SaleFactor. Column52,
                                                                                      Table_010_SaleFactor. Column54,

                                                                                      Table_010_SaleFactor.Column53,
                                                                                      Table_010_SaleFactor.Column50
                                                                               FROM   dbo.Table_010_SaleFactor
                                                                           ) AS FactorTable
                                                                    WHERE  (FactorTable.Date <= '" + item6["date"].ToString() + @"')
                                                                           AND (FactorTable. column13 = '" + _user + @"')
                                                                           AND FactorTable.column03 = " + item6["Column03"].ToString() + @"
                                                                           AND (FactorTable.column17 = 0)--باطل نيست
                                                                           AND (FactorTable.column19 = 0)--مرجوع نيست
                                                                           AND FactorTable.Column45 = 1--نسويه شده
                                                                           AND FactorTable.Column53 = 0-- بسته نشده
                                                                          
                                                                    GROUP BY
                                                                           FactorTable.date,
                                                                           FactorTable.column03,
                                                                           FactorTable.column10,
                                                                           FactorTable.hesab,
                                                                           FactorTable. Column42,
                                                                          -- FactorTable.Column50,
                                                                           FactorTable.Column10,
                                                                             FactorTable.columnid, FactorTable.column01
                                                                             ", ConSale);
                                        CloseTable = new DataTable();
                                        Adapter.Fill(CloseTable);

                                        foreach (DataRow item in this.CloseTable.Rows)
                                        {
                                            if (Convert.ToInt32(DocID.Value) > 0)
                                            {

                                                ////فروش

                                                if (Convert.ToInt32(item["sanad"]) == 0)
                                                {
                                                    string[] _AccInfo = clDoc.ACC_Info(setting.Rows[0]["Column13"].ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting.Rows[0]["Column13"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , null,
                                                                         null,
                                                                        null,
                                                                       "بستن صندوق فروش- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),
                                                                        0,
                                                                     Convert.ToInt64(Math.Round(Convert.ToDouble(item["NetTotal"].ToString()), 3))
                                                                        ,
                                                                         0,
                                                                      0,
                                                                       -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                       0, (short?)null);



                                                    _AccInfo = clDoc.ACC_Info(setting.Rows[0]["Column07"].ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting.Rows[0]["Column07"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , item["Column03"].ToString(),
                                                                         null,
                                                                        null,

                                                                       "بستن صندوق فروش- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),

                                                                     Convert.ToInt64(Math.Round(Convert.ToDouble(item["NetTotal"].ToString()), 3))
                                                                        , 0,
                                                                         0,
                                                                      0,
                                                                       -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                       0, (short?)null);


                                                }
                                                ////صندوق
                                                if (item["naghd"] != null && item["naghd"].ToString() != string.Empty && Convert.ToDouble(item["naghd"]) > 0)
                                                {

                                                    string[] _AccInfo = clDoc.ACC_Info(setting1.Rows[0]["Column07"].ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting1.Rows[0]["Column07"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , null,
                                                                         null,
                                                                        null,
                                                                       "دریافت نقدی بستن صندوق فروش- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),

                                                                      Convert.ToInt64(Math.Round(Convert.ToDouble(item["naghd"].ToString()), 3))
                                                                       , 0
                                                                       , 0,
                                                                      0,
                                                                     -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                       0, (short?)null);

                                                    _AccInfo = clDoc.ACC_Info(setting1.Rows[0]["Column13"].ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting1.Rows[0]["Column13"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , item["Column03"].ToString(),
                                                                         null,
                                                                        null,
                                                                       "دریافت نقدی بستن صندوق فروش- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),

                                                                       0,
                                                                      Convert.ToInt64(Math.Round(Convert.ToDouble(item["naghd"].ToString()), 3))
                                                                       , 0,
                                                                      0,
                                                                     -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                       0, (short?)null);
                                                }
                                                //کارت
                                                if (item["cart"] != null && item["cart"].ToString() != string.Empty && Convert.ToDouble(item["cart"]) > 0)
                                                {
                                                    string[] _AccInfo = clDoc.ACC_Info(item["hesab"].ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), item["hesab"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , null,
                                                                         null,
                                                                        null,
                                                                       "بستن صندوق فروش-کارت- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),
                                                                      Convert.ToInt64(Math.Round(Convert.ToDouble(item["cart"].ToString()), 3))
                                                                       , 0
                                                                       , 0,
                                                                      0,
                                                                      -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                      0, (short?)null);
                                                    _AccInfo = clDoc.ACC_Info(setting.Rows[0]["Column07"].ToString());
                                                    clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), setting.Rows[0]["Column07"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                       _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                       , item["Column03"].ToString(),
                                                                         null,
                                                                        null,
                                                                       "بستن صندوق فروش-کارت- فاکتورفروش" + item["column01"] + " تاریخ " + item["date"].ToString(),
                                                                        0,
                                                                      Convert.ToInt64(Math.Round(Convert.ToDouble(item["cart"].ToString()), 3))
                                                                       , 0,
                                                                      0,
                                                                      -1,
                                                                          15, Convert.ToInt32(item["columnid"]), Class_BasicOperation._UserName,
                                                                      0, (short?)null);
                                                }

                                            }
                                        }

                                        Adapter = new SqlDataAdapter(@"
                                                                                       
                                                                            SELECT distinct FactorTable.columnid,FactorTable.Date,
                                                                                   SUM(ISNULL(FactorTable.Ezafat, 0)) AS Ezafat,
                                                                                   SUM(ISNULL(FactorTable.Kosoorat, 0)) AS Kosoorat,
                                                                                   FactorTable.column16 AS bestankar,
                                                                                   FactorTable.bedehkar,
                                                                                   FactorTable.column03 as person
                                                                            FROM   (
                                                                                       SELECT dbo.Table_010_SaleFactor.column02 AS Date,
                                                                                              OtherPrice.PlusPrice AS Ezafat,
                                                                                              OtherPrice.MinusPrice AS Kosoorat,
                                                                                              Table_010_SaleFactor. column13,
                                                                                              Table_010_SaleFactor. column10,
                                                                                              Table_010_SaleFactor. column17,
                                                                                              Table_010_SaleFactor. column09,
                                                                                              Table_010_SaleFactor. column19,
                                                                                              Table_010_SaleFactor. Column45,
                                                                                                Table_010_SaleFactor. Column53,
                                                                                              otherprice.column16,
                                                                                              otherprice.column10 AS bedehkar,
                                                                                                Table_010_SaleFactor. column03,
                                                                                                Table_010_SaleFactor.columnid
                                                                                       FROM   dbo.Table_010_SaleFactor
                                                                                             
                                                                                              LEFT OUTER JOIN (
                                                                                                       SELECT columnid,
                                                                                                              SUM(PlusPrice) AS PlusPrice,
                                                                                                              SUM(MinusPrice) AS MinusPrice,
                                                                                                              column16,
                                                                                                              column10
                                                                                                       FROM   (
                                                                                                                  SELECT Table_010_SaleFactor_2.columnid,
                                                                                                                         SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                                                                                         PlusPrice,
                                                                                                                         0 AS MinusPrice,
                                                                                                                         td.column16 AS column16,
                                                                                                                         td.column10 AS column10
                                                                                                                  FROM   dbo.Table_012_Child2_SaleFactor
                                                                                                                         INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                                                              Table_010_SaleFactor_2
                                                                                                                              ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                                                                                                  Table_010_SaleFactor_2.columnid
                                                                                                                         JOIN Table_024_Discount td
                                                                                                                              ON  td.columnid = dbo.Table_012_Child2_SaleFactor.column02
                                                                                                                  WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                                                                  GROUP BY
                                                                                                                         Table_010_SaleFactor_2.columnid,
                                                                                                                         dbo.Table_012_Child2_SaleFactor.column05,
                                                                                                                         td.column16,td.column10
                                                                                                                  UNION ALL
                                                                                                                  SELECT Table_010_SaleFactor_1.columnid,
                                                                                                                         0 AS PlusPrice,
                                                                                                                         SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                                                                                         MinusPrice,
                                                                                                                         td.column16 AS column16,
                                                                                                                         td.column10 AS column10
                                                                                                                  FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                                                                                         Table_012_Child2_SaleFactor_1
                                                                                                                         INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                                                              Table_010_SaleFactor_1
                                                                                                                              ON  
                                                                                                                                  Table_012_Child2_SaleFactor_1.column01 = 
                                                                                                                                  Table_010_SaleFactor_1.columnid
                                                                                                                         JOIN Table_024_Discount td
                                                                                                                              ON  td.columnid = 
                                                                                                                                  Table_012_Child2_SaleFactor_1.column02
                                                                                                                  WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                                                                  GROUP BY
                                                                                                                         Table_010_SaleFactor_1.columnid,
                                                                                                                         Table_012_Child2_SaleFactor_1.column05,
                                                                                                                         td.column10,td.column16
                                                                                                              ) AS OtherPrice_1
                                                                                                       GROUP BY
                                                                                                              columnid,
                                                                                                              column10,
                                                                                                              column16
                                                                                                   ) AS OtherPrice
                                                                                                   ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid
                                                                                   ) AS FactorTable
                                                                            WHERE  (
                                                                                       FactorTable.Date <= '" + item6["date"].ToString() + @"'
                                                                                   )
                                                                                   AND (
                                                                                           FactorTable. column13 =
                                                                                           '" + _user + @"'
                                                                                       )
                                                                                 AND     FactorTable.column03=   " + item6["Column03"].ToString() + @" 
                                                                                   AND (FactorTable.column10 = 0)--فاکتورهایی که سندشون صفر برای مشخص شدن مغایرت
                                                                                  AND (FactorTable.column17 = 0)--باطل نيست
                                                                                   AND (FactorTable.column19 = 0)--مرجوع نيست
                                                                                   AND FactorTable.Column45 = 1--نسويه شده
                                                                                   AND FactorTable.Column53 = 0-- بسته نشده
                                                                                   
                                                                                    
                                                                            GROUP BY
                                                                                   FactorTable.date,
                                                                                   FactorTable.column10,
                                                                                   FactorTable.column16,
                                                                                   FactorTable.bedehkar,
                                                                                   FactorTable.column03,FactorTable.columnid
 

                                                                             ", ConSale);
                                        DataTable takhfif = new DataTable();
                                        Adapter.Fill(takhfif);


                                        foreach (DataRow dr in takhfif.Rows)
                                        {

                                            ///تخفیف
                                            ///
                                            if (dr["Kosoorat"] != null && dr["Kosoorat"].ToString() != string.Empty && Convert.ToDouble(dr["Kosoorat"]) > 0)
                                            {

                                                string[] _AccInfo = clDoc.ACC_Info(dr["bedehkar"].ToString());
                                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), dr["bedehkar"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                   _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                   , null,
                                                                     null,
                                                                    null,
                                                                   "بستن صندوق فروش-تخفیف-فاکتور فروش تاریخ" + item6["date"].ToString(),
                                                                  Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3))
                                                                   , 0
                                                                   , 0,
                                                                  0,
                                                                   -1,
                                                                      15, Convert.ToInt32(dr["columnid"]), Class_BasicOperation._UserName,
                                                                   0, (short?)null);




                                                _AccInfo = clDoc.ACC_Info(dr["bestankar"].ToString());
                                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), dr["bestankar"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                   _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                   , dr["person"].ToString(),
                                                                     null,
                                                                    null,
                                                                   "بستن صندوق فروش-تخفیف-فاکتور فروش تاریخ" + item6["date"].ToString(),
                                                                   0,
                                                                  Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3))

                                                                   , 0,
                                                                  0,
                                                                   -1,
                                                                      15, Convert.ToInt32(dr["columnid"]), Class_BasicOperation._UserName,
                                                                   0, (short?)null);


                                            }
                                            //ارزش افزوده
                                            if (dr["Ezafat"] != null && dr["Ezafat"].ToString() != string.Empty && Convert.ToDouble(dr["Ezafat"]) > 0)
                                            {


                                                string[] _AccInfo = clDoc.ACC_Info(dr["bestankar"].ToString());
                                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), dr["bestankar"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                   _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                   , null,
                                                                     null,
                                                                    null,
                                                                   "بستن صندوق فروش-ارزش افزوده-فاکتور فروش تاریخ" + item6["date"].ToString(),
                                                                     0
                                                                 , Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3))
                                                                   , 0,
                                                                  0,
                                                                  -1,
                                                                       15, Convert.ToInt32(dr["columnid"]), Class_BasicOperation._UserName,
                                                                   0, (short?)null);


                                                _AccInfo = clDoc.ACC_Info(dr["bedehkar"].ToString());
                                                clDoc.ExportDoc_Detail(Convert.ToInt32(DocID.Value), dr["bedehkar"].ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                                                   _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                                                   , dr["person"].ToString(),
                                                                     null,
                                                                    null,
                                                                   "بستن صندوق فروش-ارزش افزوده-فاکتور فروش تاریخ" + item6["date"].ToString(),
                                                                   Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3))
                                                                   , 0
                                                                   , 0,
                                                                  0,
                                                                  -1,
                                                                       15, Convert.ToInt32(dr["columnid"]), Class_BasicOperation._UserName,
                                                                   0, (short?)null);

                                            }

                                        }

                                    }


                                    using (SqlConnection Con = new SqlConnection(ConSale.ConnectionString))
                                    {
                                        Con.Open();
                                        SqlCommand Update = new SqlCommand(@"UPDATE Table_010_SaleFactor SET Column10=" + DocID.Value + @",Column53=1,Column15='" + Class_BasicOperation._UserName + @"',Column16=getdate() where ColumnId IN (SELECT Table_010_SaleFactor.ColumnId  
                                                                        
                                                                                                                                                    FROM   Table_010_SaleFactor
                                                                                                                                                    WHERE  (
                                                                                                                                                               Table_010_SaleFactor.column02 <=
                                                                                                                                                               '" + item6["date"].ToString() + @"'
                                                                                                                                                           )
                                                                                                                                                           AND (
                                                                                                                                                                   Table_010_SaleFactor.column13 =
                                                                                                                                                                   '" + _user + @"'
                                                                                                                                                               )
                                                                                                                                                   AND     Table_010_SaleFactor.column03=   " + item6["Column03"].ToString() + @" 
                                                                                                                                                           AND (Table_010_SaleFactor.column17 = 0)--باطل نيست
                                                                                                                                                           AND (Table_010_SaleFactor.column19 = 0)--مرجوع نيست
                                                                                                                                                           AND Table_010_SaleFactor.Column45 = 1--نسويه شده
                                                                                                                                                           AND Table_010_SaleFactor.Column53 = 0-- بسته نشده
                                                                                                                                                           )", Con);
                                        Update.ExecuteNonQuery();
                                    }

                                    using (SqlConnection Con = new SqlConnection(this.ConWare.ConnectionString))
                                    {
                                        Con.Open();
                                        SqlCommand Update = new SqlCommand(@"UPDATE Table_007_PwhrsDraft
                                                                                SET    Column07             = ISNULL( (
                                                                                           SELECT Column10
                                                                                           FROM   " + ConSale.Database + @".dbo.Table_010_SaleFactor
                                                                                           WHERE  Column09  = Table_007_PwhrsDraft.ColumnId
                                                                                                  AND Column10 > 0
                                                                                ),Column07)
                                                                                WHERE Column07=0", Con);
                                        Update.ExecuteNonQuery();
                                    }





                                }

                            }
                        }


                    }

                    Btn_Save.Enabled = false;
                    Class_BasicOperation.ShowMsg("", "ثیت اسناد  با موفقیت انجام شد", "Information");
                    Btn_Save.Enabled = true;
                    this.Close();
                }
                else
                {
                    Class_BasicOperation.ShowMsg("", "شماره حساب نا معتبر", "Information");
                    return;
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }

        }
        private void CheckEssentialItems(object sender, EventArgs e)
        {



            SqlDataAdapter Adapter = new SqlDataAdapter(@"
                                                                 SELECT DISTINCT FactorTable.date,
                                                                           SUM(ISNULL(FactorTable.Ezafat, 0)) AS Ezafat,
                                                                           SUM(ISNULL(FactorTable.Kosoorat, 0)) AS Kosoorat,
                                                                           SUM(FactorTable.NetTotal) AS NetTotal,
                                                                           SUM(FactorTable. Column46) AS naghd,
                                                                           SUM(FactorTable. Column47) AS cart,
                                                                           SUM(FactorTable. Column48) AS sayer,
                                                                           FactorTable.column16 AS bestankar,
                                                                           FactorTable.bedehkar,
                                                                           FactorTable.hesab,
                                                                           FactorTable.column03,
                                                                           SUM(FactorTable.Column52) AS [check],
                                                                           SUM(FactorTable.Column54) AS [bon]

                                                                    FROM   (
                                                                               SELECT dbo.Table_010_SaleFactor.columnid,
                                                                                      dbo.Table_010_SaleFactor.column01,
                                                                                      dbo.Table_010_SaleFactor.column02 AS Date,
                                                                                      OtherPrice.PlusPrice AS Ezafat,
                                                                                      OtherPrice.MinusPrice AS Kosoorat,
                                                                                      dbo.Table_010_SaleFactor.Column28 AS NetTotal,
                                                                                      Table_010_SaleFactor. Column46,
                                                                                      Table_010_SaleFactor. Column47,
                                                                                      Table_010_SaleFactor. Column48,
                                                                                      Table_010_SaleFactor. Column42,
                                                                                      Table_010_SaleFactor. column03,
                                                                                      Table_010_SaleFactor. column13,
                                                                                      Table_010_SaleFactor. column10,
                                                                                      Table_010_SaleFactor. column17,
                                                                                      Table_010_SaleFactor. column09,
                                                                                      Table_010_SaleFactor. column19,
                                                                                      Table_010_SaleFactor. Column45,
                                                                                      Table_010_SaleFactor. Column52,
                                                                                      Table_010_SaleFactor. Column54,

                                                                                      Table_010_SaleFactor.Column53,
                                                                                      Table_010_SaleFactor.Column50,
                                                                                      (
                                                                                          SELECT ISNULL(
                                                                                                     (
                                                                                                         SELECT tbcai.Column12
                                                                                                         FROM   " + ConBNK.Database + @".dbo.Table_020_BankCashAccInfo 
                                                                                                                tbcai
                                                                                                         WHERE  tbcai.columnid = CAST(Table_010_SaleFactor.Column49 AS SMALLINT)
                                                                                                     ),
                                                                                                     ''
                                                                                                 )
                                                                                      ) AS hesab,
                                                                                      otherprice.column16,
                                                                                      otherprice.column10 AS bedehkar
                                                                               FROM   dbo.Table_010_SaleFactor
                                                                                      LEFT OUTER JOIN (
                                                                                               SELECT columnid,
                                                                                                      SUM(PlusPrice) AS PlusPrice,
                                                                                                      SUM(MinusPrice) AS MinusPrice,
                                                                                                      column16,
                                                                                                      column10
                                                                                               FROM   (
                                                                                                          SELECT Table_010_SaleFactor_2.columnid,
                                                                                                                 SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                                                                                 PlusPrice,
                                                                                                                 0 AS MinusPrice,
                                                                                                                 td.column16 AS column16,
                                                                                                                 NULL AS column10
                                                                                                          FROM   dbo.Table_012_Child2_SaleFactor
                                                                                                                 INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                                                      Table_010_SaleFactor_2
                                                                                                                      ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                                                                                          Table_010_SaleFactor_2.columnid
                                                                                                                 JOIN Table_024_Discount td
                                                                                                                      ON  td.columnid = dbo.Table_012_Child2_SaleFactor.column02
                                                                                                          WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                                                          GROUP BY
                                                                                                                 Table_010_SaleFactor_2.columnid,
                                                                                                                 dbo.Table_012_Child2_SaleFactor.column05,
                                                                                                                 td.column16
                                                                                                          UNION ALL
                                                                                                          SELECT Table_010_SaleFactor_1.columnid,
                                                                                                                 0 AS PlusPrice,
                                                                                                                 SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                                                                                 MinusPrice,
                                                                                                                 NULL AS column16,
                                                                                                                 td.column10 AS column10
                                                                                                          FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                                                                                 Table_012_Child2_SaleFactor_1
                                                                                                                 INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                                                      Table_010_SaleFactor_1
                                                                                                                      ON  
                                                                                                                          Table_012_Child2_SaleFactor_1.column01 = 
                                                                                                                          Table_010_SaleFactor_1.columnid
                                                                                                                 JOIN Table_024_Discount td
                                                                                                                      ON  td.columnid = 
                                                                                                                          Table_012_Child2_SaleFactor_1.column02
                                                                                                          WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                                                          GROUP BY
                                                                                                                 Table_010_SaleFactor_1.columnid,
                                                                                                                 Table_012_Child2_SaleFactor_1.column05,
                                                                                                                 td.column10
                                                                                                      ) AS OtherPrice_1
                                                                                               GROUP BY
                                                                                                      columnid,
                                                                                                      column10,
                                                                                                      column16
                                                                                           ) AS OtherPrice
                                                                                           ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid
                                                                           ) AS FactorTable
                                                                    WHERE  (FactorTable.Date <= '" + Date + @"')
                                                                           AND (FactorTable. column13 = '" + _user + @"')
                                                                            AND (FactorTable.column17 = 0)--باطل نيست
                                                                           AND (FactorTable.column19 = 0)--مرجوع نيست
                                                                           AND FactorTable.Column45 = 1--نسويه شده
                                                                           AND FactorTable.Column53 = 0-- بسته نشده
    
                                                                    GROUP BY
                                                                           FactorTable.date,
                                                                           FactorTable.column16,
                                                                           FactorTable.hesab,
                                                                           FactorTable.bedehkar,
                                                                           FactorTable.column03
 

                                                                             ", ConSale);
            DataTable cheches = new DataTable();
            Adapter.Fill(cheches);


            if (txt_Cover.Text.Trim() == "")
                throw new Exception("اطلاعات مورد نیاز جهت صدور سند را کامل کنید");




            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            //clDoc.CheckForValidationDate(faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

            //if (uiTab2.Enabled)
            //{
            //    foreach (DataRowView item in Child1Bind)
            //    {
            //        if (!clGood.IsGoodInWare(short.Parse(mlt_Ware.Value.ToString()),
            //            int.Parse(item["Column02"].ToString())))
            //            throw new Exception("کالای " +
            //                clDoc.ExScalar(ConWare.ConnectionString, "table_004_CommodityAndIngredients", "Column02",
            //                "ColumnId", item["Column02"].ToString()) + " در انبار انتخاب شده فعال نمی باشد");

            //    }
            //}

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
            TPerson.Rows.Clear();
            TAccounts.Rows.Clear();
            foreach (DataRow item in cheches.Rows)
            {
                if (Convert.ToDouble(item["NetTotal"]) == Convert.ToDouble(0))
                    throw new Exception("فاکتور فروش با مبلغ صفر وجود دارد");


                if (!chk_person.Checked)
                {
                    Person = null;
                    Center = null;
                    Project = null;

                    All_Controls_Row1(setting.Rows[0]["Column13"].ToString(), null, Center, Project);
                    All_Controls_Row1(this.mlt_ACC.Value.ToString(), null, Center, Project);
                    All_Controls_Row1(setting2.Rows[0]["Column07"].ToString(), int.Parse(item["column03"].ToString()), Center, Project);
                    All_Controls_Row1(item["hesab"].ToString(), null, Center, Project);


                    if (item["cart"] != null && item["cart"].ToString() != string.Empty && Convert.ToDouble(item["cart"]) > 0)

                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Con.Open();

                            SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + item["hesab"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                            ok1 = int.Parse(Comm.ExecuteScalar().ToString());

                        }
                    if (ok1 == 0)
                        throw new Exception("شماره حساب معتبر را در حساب بانکی وارد کنید");

                    if (item["NetTotal"] != null && item["NetTotal"].ToString() != string.Empty && Convert.ToDouble(item["NetTotal"]) > 0)
                        TAccounts.Rows.Add(setting.Rows[0]["Column13"].ToString(), Convert.ToDouble(item["NetTotal"].ToString()) * (-1));

                    if (item["naghd"] != null && item["naghd"].ToString() != string.Empty && Convert.ToDouble(item["naghd"]) > 0)
                        TAccounts.Rows.Add(this.mlt_ACC.Value.ToString(), Convert.ToDouble(item["naghd"].ToString()));

                    if (item["sayer"] != null && item["sayer"].ToString() != string.Empty && Convert.ToDouble(item["sayer"]) > 0)
                        TAccounts.Rows.Add(setting2.Rows[0]["Column07"].ToString(), Convert.ToDouble(item["sayer"].ToString()));

                    if (item["check"] != null && item["check"].ToString() != string.Empty && Convert.ToDouble(item["check"]) > 0)
                        TAccounts.Rows.Add(setting2.Rows[0]["Column07"].ToString(), Convert.ToDouble(item["check"].ToString()));
                    if (item["bon"] != null && item["bon"].ToString() != string.Empty && Convert.ToDouble(item["bon"]) > 0)
                        TAccounts.Rows.Add(setting5.Rows[0]["Column07"].ToString(), Convert.ToDouble(item["bon"].ToString()));

                    if (item["Kosoorat"] != null && item["Kosoorat"].ToString() != string.Empty && Convert.ToDouble(item["Kosoorat"]) > 0)
                        TAccounts.Rows.Add(item["bedehkar"].ToString(), Convert.ToDouble(item["Kosoorat"].ToString()));

                    if (item["Ezafat"] != null && item["Ezafat"].ToString() != string.Empty && Convert.ToDouble(item["Ezafat"]) > 0)
                        TAccounts.Rows.Add(item["bestankar"].ToString(), Convert.ToDouble(item["Ezafat"].ToString()) * (-1));

                    if (item["cart"] != null && item["cart"].ToString() != string.Empty && Convert.ToDouble(item["cart"]) > 0)
                        TAccounts.Rows.Add(item["hesab"].ToString(), Convert.ToDouble(item["cart"].ToString()));

                    TPerson.Rows.Add(Int32.Parse(item["Column03"].ToString()), setting2.Rows[0]["Column07"].ToString(), Convert.ToDouble(item["sayer"].ToString()));
                    TPerson.Rows.Add(Int32.Parse(item["Column03"].ToString()), setting2.Rows[0]["Column07"].ToString(), Convert.ToDouble(item["check"].ToString()));
                    TPerson.Rows.Add(Int32.Parse(item["Column03"].ToString()), setting5.Rows[0]["Column07"].ToString(), Convert.ToDouble(item["bon"].ToString()));

                }
                else
                {

                    Person = null;
                    Center = null;
                    Project = null;

                    All_Controls_Row1(setting.Rows[0]["Column13"].ToString(), null, Center, Project);
                    All_Controls_Row1(setting.Rows[0]["Column07"].ToString(), int.Parse(item["column03"].ToString()), Center, Project);


                    All_Controls_Row1(this.mlt_ACC.Value.ToString(), null, Center, Project);
                    All_Controls_Row1(setting1.Rows[0]["Column13"].ToString(), int.Parse(item["column03"].ToString()), Center, Project);

                    All_Controls_Row1(item["hesab"].ToString(), null, Center, Project);
                    All_Controls_Row1(setting.Rows[0]["Column07"].ToString(), int.Parse(item["column03"].ToString()), Center, Project);


                    if (item["cart"] != null && item["cart"].ToString() != string.Empty && Convert.ToDouble(item["cart"]) > 0)

                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Con.Open();

                            SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + item["hesab"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                            ok1 = int.Parse(Comm.ExecuteScalar().ToString());

                        }
                    if (ok1 == 0)
                        throw new Exception("شماره حساب معتبر را در حساب بانکی وارد کنید");

                    if (item["NetTotal"] != null && item["NetTotal"].ToString() != string.Empty && Convert.ToDouble(item["NetTotal"]) > 0)
                    {
                        TAccounts.Rows.Add(setting.Rows[0]["Column13"].ToString(), Convert.ToDouble(item["NetTotal"].ToString()) * (-1));
                        TAccounts.Rows.Add(setting.Rows[0]["Column07"].ToString(), Convert.ToDouble(item["NetTotal"].ToString()));

                    }

                    if (item["naghd"] != null && item["naghd"].ToString() != string.Empty && Convert.ToDouble(item["naghd"]) > 0)
                    {
                        TAccounts.Rows.Add(this.mlt_ACC.Value.ToString(), Convert.ToDouble(item["naghd"].ToString()));
                        TAccounts.Rows.Add(setting1.Rows[0]["Column13"].ToString(), Convert.ToDouble(item["naghd"].ToString()) * (-1));

                    }

                    if (item["Kosoorat"] != null && item["Kosoorat"].ToString() != string.Empty && Convert.ToDouble(item["Kosoorat"]) > 0)
                    {
                        TAccounts.Rows.Add(item["bedehkar"].ToString(), Convert.ToDouble(item["Kosoorat"].ToString()));
                        TAccounts.Rows.Add(item["bestankar"].ToString(), Convert.ToDouble(item["Kosoorat"].ToString()) * (-1));

                    }

                    if (item["Ezafat"] != null && item["Ezafat"].ToString() != string.Empty && Convert.ToDouble(item["Ezafat"]) > 0)
                    {
                        TAccounts.Rows.Add(item["bestankar"].ToString(), Convert.ToDouble(item["Ezafat"].ToString()) * (-1));
                        TAccounts.Rows.Add(item["bedehkar"].ToString(), Convert.ToDouble(item["Ezafat"].ToString()));

                    }

                    if (item["cart"] != null && item["cart"].ToString() != string.Empty && Convert.ToDouble(item["cart"]) > 0)
                    {
                        TAccounts.Rows.Add(item["hesab"].ToString(), Convert.ToDouble(item["cart"].ToString()));
                        TAccounts.Rows.Add(setting.Rows[0]["Column07"].ToString(), Convert.ToDouble(item["cart"].ToString()) * (-1));

                    }




                    TPerson.Rows.Add(Int32.Parse(item["Column03"].ToString()), setting.Rows[0]["Column13"].ToString(), Convert.ToDouble(item["NetTotal"].ToString()));
                    TPerson.Rows.Add(Int32.Parse(item["Column03"].ToString()), this.mlt_ACC.Value.ToString(), Convert.ToDouble(item["naghd"].ToString()));
                    TPerson.Rows.Add(Int32.Parse(item["Column03"].ToString()), item["bedehkar"].ToString(), Convert.ToDouble(item["Kosoorat"].ToString()));
                    TPerson.Rows.Add(Int32.Parse(item["Column03"].ToString()), item["bestankar"].ToString(), Convert.ToDouble(item["Ezafat"].ToString()));
                    TPerson.Rows.Add(Int32.Parse(item["Column03"].ToString()), setting.Rows[0]["Column07"].ToString(), Convert.ToDouble(item["cart"].ToString()));

                }
            }


            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);


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


        }
        private void chk_person_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.chksanadPerson = chk_person.Checked;
            Properties.Settings.Default.Save();
        }

        private void mlt_ACC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);

        }

        private void mlt_ACC_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                Class_BasicOperation.FilterMultiColumns(sender, "ACC_Name", "ACC_Code");
            }
            catch
            {
            }
        }

        private void mlt_ACC_Leave(object sender, EventArgs e)
        {
            try
            {
                Class_BasicOperation.MultiColumnsRemoveFilter(sender);
            }
            catch
            {
            }
        }
    }
}
