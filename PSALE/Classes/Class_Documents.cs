using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;

namespace PSHOP.Classes
{
    class Class_Documents
    {
        public enum FactorType { Sale, ReturnSale, Buy, ReturnBuy };
        SqlConnection ConMAIN = new SqlConnection(Properties.Settings.Default.MAIN);

        #region انبار

        public void ConfirmedDraftReceipt(string Type, string Id)
        {

            using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS))
            {

                ConWare.Open();
                if (Type != "Draft")
                {
                    SqlCommand Select = new SqlCommand(@"SELECT ISNULL(Cast((Select Column19 from Table_011_PwhrsReceipt where columnid=" + Id + ") as nvarchar(10)),'No')", ConWare);
                    string Result = Select.ExecuteScalar().ToString();
                    if (Result == "No")
                        throw new Exception("چنین رسیدی وجود ندارد");
                    else if (Result == "1")
                        throw new Exception("این رسید قطعی شده است");
                }
                else
                {
                    SqlCommand Select = new SqlCommand(@"SELECT ISNULL(Cast((Select Column26 from Table_007_PwhrsDraft where columnid=" + Id + ") as nvarchar(10)),'No')", ConWare);
                    string Result = Select.ExecuteScalar().ToString();
                    if (Result == "No")
                        throw new Exception("چنین حواله ای وجود ندارد");
                    else if (Result == "1")
                        throw new Exception("این حواله قطعی شده است");
                }
            }
        }


        #endregion
        //اجرای دستور سی کولی که فقط یک مقدار را بر می گرداند
        public string ExScalarQuery(string ConString, string query)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand(query, Con);
                return Select.ExecuteScalar().ToString();
            }
        }
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);

        //شماره سند وارد شده معتبر است؟
        public void IsValidNumberS(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select Count(Column00) from Table_060_SanadHead where ColumnId=" + DocNum, ConAcnt);
                int Num = int.Parse(Command.ExecuteScalar().ToString());
                if (Num == 0)
                    throw new Exception("شماره سند مورد نظر نامعتبر می باشد");
            }
        }

        public string CoverS(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Column04 from Table_060_SanadHead where ColumnId=" + DocNum, ConAcnt);
                string Result = Select.ExecuteScalar().ToString();
                return Result;
            }
        }

        //تاریخ سند مورد نظر را بر می گرداند
        public string DocDateS(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Column01 from Table_060_SanadHead where ColumnId=" + DocNum, ConAcnt);
                string Result = Select.ExecuteScalar().ToString();
                return Result;
            }
        }


        //حذف دیتیل با آی دی سند
        public int DeleteDetail_ID(int DocID, Int16 Type, int Ref)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                try
                {
                    DataTable Table = ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocID + " and Column16=" + Type + " and Column17=" + Ref);
                    foreach (DataRow item in Table.Rows)
                    {
                        RunSqlCommand(ConAcnt.ConnectionString, @"Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString());
                    }

                    SqlCommand Del = new SqlCommand("Delete  from Table_065_SanadDetail where Column00=" + DocID + " and Column16=" + Type + " and Column17=" + Ref, ConAcnt);
                    int Count = Del.ExecuteNonQuery();
                    return Count;
                }
                catch
                {
                    throw new Exception("حذف سند حسابداری " + DocNum(DocID) + " امکان پذیر نمی باشد");
                }
            }
        }


        //شماره سند وارد شده معتبر است؟
        public void IsValidNumber(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select Count(Column00) from Table_060_SanadHead where Column00=" + DocNum, ConAcnt);
                int Num = int.Parse(Command.ExecuteScalar().ToString());
                if (Num == 0)
                    throw new Exception("شماره سند مورد نظر نامعتبر می باشد");
            }
        }


        //کنترل قطعی بودن یا نبودن شماره سند وارد شده توسط کاربر
        public void IsFinal(int DocNum)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                       (
                                                                           SELECT ISNULL(Column03, 0)
                                                                           FROM   Table_060_SanadHead
                                                                           WHERE  Column00 = " + DocNum + @"
                                                                       ),
                                                                       0
                                                                   )", Con);
                bool Final = bool.Parse(Command.ExecuteScalar().ToString());
                if (Final)
                    throw new Exception("سند مورد نظر قطعی/تأیید شده می باشد");
            }
        }
        //کنترل قطعی بودن یا نبودن آی دی سند وارد شده توسط کاربر
        public void IsFinal_ID(int DocID)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                       (
                                                                           SELECT ISNULL(Column03, 0)
                                                                           FROM   Table_060_SanadHead
                                                                           WHERE  ColumnId = " + DocID + @"
                                                                       ),
                                                                       0
                                                                   )", Con);
                bool Final = bool.Parse(Command.ExecuteScalar().ToString());
                if (Final)
                    throw new Exception("سند مورد نظر قطعی/تأیید شده می باشد");
            }
        }

        //سند در حالت یادداشت قرار دارد؟
        public void IsDraft(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select Column02 from Table_060_SanadHead where Column00=" + DocNum, ConAcnt);
                Int16 Type = Int16.Parse(Command.ExecuteScalar().ToString());
                if (Type == 3)
                    throw new Exception("سند مورد نظر در حالت یادداشت می باشد");
            }

        }


        //آی دی شماره سند انتخاب شده را بر می گرداند
        public int DocID(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Commnad = new SqlCommand("Select ColumnId from Table_060_SanadHead where Column00=" + DocNum, ConAcnt);
                int ID = int.Parse(Commnad.ExecuteScalar().ToString());
                return ID;
            }
        }


        //شماره سند معادل آی دی سند را بر می گرداند
        public int DocNum(int DocId)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select Column00 from Table_060_SanadHead where ColumnId=" + DocId, ConAcnt);
                int Num = int.Parse(Command.ExecuteScalar().ToString());
                return Num;
            }
        }

        //تغییر حالت سند 
        public void ChangeDocType(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL(Sum(Column11),0) as Bed,ISNULL(Sum(Column12),0) as Bes  from Table_065_SanadDetail where Column00=" +
                    DocID(DocNum), ConAcnt);
                SqlDataReader Reader = Command.ExecuteReader();
                Reader.Read();
                if (Reader.HasRows)
                {
                    Int64 Bed = Int64.Parse(Reader["Bed"].ToString());
                    Int64 Bes = Int64.Parse(Reader["Bes"].ToString());
                    Reader.Close();
                    if (Bed != Bes)
                    {
                        SqlCommand Update = new SqlCommand("Update Table_060_SanadHead Set Column02=3 where ColumnId=" +
                            DocID(DocNum), ConAcnt);
                        Update.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand Update = new SqlCommand("Update Table_060_SanadHead Set Column02=1 where ColumnId=" +
                            DocID(DocNum), ConAcnt);
                        Update.ExecuteNonQuery();
                    }
                }
            }
        }


        //برگرداندن آخرین تاریخ قطعی سازی
        public string LastFinalDate()
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL(( select Column00 from Table_085_CloseSettingHeader where ColumnId=5),'NoColumn')", Con);
                string Result = Command.ExecuteScalar().ToString();
                return Result;
            }
        }



        //تاریخ سند مورد نظر را بر می گرداند
        public string DocDate(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Column01 from Table_060_SanadHead where ColumnId=" + DocID(DocNum), ConAcnt);
                string Result = Select.ExecuteScalar().ToString();
                return Result;
            }
        }


        //شماره آخرین سند را بر می گرداند
        public int LastDocNum()
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Isnull((Select Max(Column00) from Table_060_SanadHead),0)", ConAcnt);
                int Result = int.Parse(Select.ExecuteScalar().ToString());
                return Result;
            }
        }

        //شرح سند شماره سند دلخواه را بر می گرداند
        public string Cover(int DocNum)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Column04 from Table_060_SanadHead where ColumnId=" + DocID(DocNum), ConAcnt);
                string Result = Select.ExecuteScalar().ToString();
                return Result;
            }
        }


        //بدهکار یا بستانکار تراکنش خاصی را بر می گرداند
        public string Account(Int16 ID, string ColumnName)
        {
            using (SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE))
            {
                ConBase.Open();
                SqlCommand Select = new SqlCommand("Select ISNULL((Select " + ColumnName + " from Table_105_SystemTransactionInfo where Column00=" + ID + "),' ')", ConBase);
                string Result = Select.ExecuteScalar().ToString();
                return Result.Trim();
            }
        }

        //صدور هدر سند//
        public int ExportDoc_Header(int Number, string Date, string Cover, string UserName)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                int SanadNumber = 0;
                SqlParameter Key = new SqlParameter("Key", System.Data.SqlDbType.Int);
                Key.Direction = System.Data.ParameterDirection.Output;
                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES(" + Number + ",'" + Date + "',2,0,'" + Cover + "','" + UserName +
                         "',getdate()); SET @Key=SCOPE_IDENTITY()", ConAcnt);
                Insert.Parameters.Add(Key);
                Insert.ExecuteNonQuery();
                SanadNumber = int.Parse(Key.Value.ToString());
                return SanadNumber;
            }


        }

        //صدور دیتیل سند//
        public int ExportDoc_Detail(int HeaderID, string ACC, Int16 Group, string Kol, string Moein,
            string Tafsili, string Joz, string Person, string Center, string Project, string Sharh, Int64 Bed,
            Int64 Bes, double CurBed, double CurBes, Int16 CurType, Int16 SanadType, int Ref, string RegUser, double CurPrice, short? SubSys)
        {
            if (string.IsNullOrEmpty(Moein))
                Moein = "NULL";
            else Moein = "'" + Moein + "'";
            if (string.IsNullOrEmpty(Tafsili))
                Tafsili = "NULL";
            else Tafsili = "'" + Tafsili + "'";
            if (string.IsNullOrEmpty(Joz))
                Joz = "NULL";
            else Joz = "'" + Joz + "'";

            if (string.IsNullOrEmpty(Person))
                Person = "NULL";

            if (string.IsNullOrEmpty(Center))
                Center = "NULL";

            if (string.IsNullOrEmpty(Project))
                Project = "NULL";


            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                Key.Direction = ParameterDirection.Output;
                ConAcnt.Open();
                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) VALUES(" + HeaderID + ",'" + ACC + "'," + Group + ",'" + Kol + "'," + Moein + "," + Tafsili + "," + Joz + "," + Person + "," + Center + "," + Project
                    + ",'" + Sharh + "'," + Bed + "," + Bes + "," + CurBed + "," + CurBes + "," +
                    CurType + "," + SanadType + "," + Ref + ",'" + RegUser + "',getdate(),'" +
                    RegUser + "',getdate()," + CurPrice + "," + (SubSys == (short?)null ? "NULL" : SubSys.ToString()) + "); SET @Key=SCOPE_IDENTITY()", ConAcnt);
                Insert.Parameters.Add(Key);
                Insert.ExecuteNonQuery();
                return int.Parse(Key.Value.ToString());
            }
        }

        //آپدیت شماره سند جداول//
        public void Update_Des_Table(string ConString, string TableName, string MainColumn, string ConditionColumn, int ConditionValue, int Value)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Update = new SqlCommand("UPDATE " + TableName + " SET " + MainColumn + "=" + Value + " where " + ConditionColumn + "=" + ConditionValue, Con);
                Update.ExecuteNonQuery();
            }
        }

        //اجرای دستور سی کولی که فقط یک مقدار را بر می گرداند
        public string ExScalar(string ConString, string TableName, string ColumnName, string ConditionColumn, string ConValue)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("SELECT " + ColumnName + " FROM " + TableName + " WHERE " + ConditionColumn + "="
                    + ConValue, Con);
                return Select.ExecuteScalar().ToString();
            }
        }

        public string ExScalar(string ConString, string query)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand(query, Con);
                return Select.ExecuteScalar().ToString();
            }
        }


        //برگرداندن اطلاعات سرفصل انتخاب شده در هنگام صدور سند
        public string[] ACC_Info(string ACC_Code)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select GroupCode,KolCode,MoeinCode,TafsiliCode,JozCode from AllHeaders() where ACC_Code='" + ACC_Code + "'", ConAcnt);
                using (SqlDataReader Reader = Select.ExecuteReader())
                {
                    string[] Codes = new string[5];
                    while (Reader.Read())
                    {
                        Codes[0] = Reader[0].ToString();
                        Codes[1] = Reader[1].ToString();
                        Codes[2] = Reader[2].ToString();
                        Codes[3] = Reader[3].ToString();
                        Codes[4] = Reader[4].ToString();
                    }
                    return Codes;
                }
            }
        }


        //تاریخ اولین سند صادر شده
        public string FirstDocDate()
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL(Min(Column01),'NoColumn') from Table_060_SanadHead", ConAcnt);
                string Result = Command.ExecuteScalar().ToString();
                return Result;
            }
        }



        //عدم صدور سند تا آخرین تاریخ قطعی سازی
        public void CheckForValidationDate(string Date)
        {
            string LastFinalDate = this.LastFinalDate();
            if (LastFinalDate != "NoColumn")
            {
                using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    ConAcnt.Open();
                    SqlCommand Command = new SqlCommand("Select Case When '" + Date + "'<='" +
                        LastFinalDate + "' then 1 else 0 end As Result", ConAcnt);
                    if (Command.ExecuteScalar().ToString() == "1")
                    {
                        throw new Exception("اسناد تا این تاریخ قطعی شده اند");
                    }
                }

            }
        }


        //آیا سند اختتامیه صادر شده است
        public void CheckExistFinalDoc()
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Command = new SqlCommand("Select ISNULL(Count(ColumnId),0) as Expr from Table_060_SanadHead where Column02=5", ConAcnt);
                if (int.Parse(Command.ExecuteScalar().ToString()) > 0)
                    throw new Exception("به علت وجود سند اختتامیه، صدور سند جدید امکانپذیر نمی باشد");
            }
        }

        //برگرداندن مانده حساب یک شخص در یک حساب
        public Int64 PersonRemain(int PersonId, string Acc)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                Int64 Remain = 0;
                SqlCommand Command = new SqlCommand("Select ISNULL(SUM(Column11)-SUM(Column12),0) as Remain from Table_065_SanadDetail where Column07=" + PersonId + " and Column01='" + Acc + "'", ConAcnt);
                Remain = Int64.Parse(Command.ExecuteScalar().ToString());
                return Remain;
            }

        }

        //برگرداندن ماکزیمم شماره در یک جدول
        public int MaxNumber(string ConString, string TableName, string ColumnName)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand("SELECT ISNULL(Max(" + ColumnName + "),0)+1 as ID from " + TableName, Con);
                int ID = int.Parse(Command.ExecuteScalar().ToString());
                return ID;
            }
        }

        ///// <summary>
        ///// برگرداندن شماره یک برگه با در نظر گرفتن انبار آن
        ///// </summary>
        ///// <param name="ConString"></param>
        ///// <param name="TableName"></param>
        ///// <param name="ColumnName"></param>
        ///// <returns></returns>
        //public int MaxNumber(string ConString, string TableName, string ColumnName,int WareId)
        //{
        //    using (SqlConnection Con = new SqlConnection(ConString))
        //    {
        //        Con.Open();
        //        SqlCommand Command = new SqlCommand("SELECT ISNULL(Max(" + ColumnName + "),0)+1 as ID from " + TableName+" where Column03="+WareId, Con);
        //        int ID = int.Parse(Command.ExecuteScalar().ToString());
        //        return ID;
        //    }
        //}

        //اجرای دستور سیکول بدون خروجی
        public void RunSqlCommand(string ConString, string CommandText)
        {
            using (SqlConnection Con = new SqlConnection(ConString))
            {
                Con.Open();
                SqlCommand Command = new SqlCommand(CommandText, Con);
                Command.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// Add Cheque turn to Table_065_TurnReception
        /// </summary>
        /// <param name="ConBank">SqlConnection</param>
        /// <param name="PaperID">Receive Paper Id</param>
        /// <param name="Status">Cheque Status</param>
        /// <param name="Date">Doc Date</param>
        /// <param name="Person">Bed Person</param>
        /// <param name="BoxBank"></param>
        /// <param name="ACC">Bed Account</param>
        /// <param name="Group"></param>
        /// <param name="Kol"></param>
        /// <param name="Moein"></param>
        /// <param name="Tafsili"></param>
        /// <param name="Joz"></param>
        /// <param name="SanadID"></param>
        /// <param name="Center"></param>
        /// <param name="Project">Bed Project</param>
        /// <param name="User"></param>
        /// <param name="Description"></param>
        /// <returns></returns>
        //اضافه کردن گردش چکهای دریافتی به جدول گردش دریافتها
        public int AddTurnReception(int PaperID, Int16 Status, string Date, string Person, string BoxBank, string ACC, Int16 Group, string Kol, string Moein, string Tafsili, string Joz,
            int SanadID, string Center, string Project, string User, string Description, string Currency, string CurrencyType, Double CurrencyValue)
        {
            using (SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK))
            {
                ConBank.Open();
                int ID = 0;
                SqlCommand Insert = new SqlCommand("INSERT INTO Table_065_TurnReception VALUES(" + PaperID + "," + Status + ",'" + Date + "'," +
                    Person + "," + BoxBank + ",'" + ACC + "'," + Group + "," + Kol + "," + Moein + "," + Tafsili + "," + Joz + "," + SanadID + "," + Center + "," + Project + ",'" + User + "',getdate()," + Description +
                    "," + (Currency == "True" ? 1 : 0) +
                    "," + (string.IsNullOrEmpty(CurrencyType) ? "NULL" : CurrencyType) + "," + CurrencyValue +
                    "); SET @ID=SCOPE_IDENTITY()", ConBank);
                SqlParameter _ID = new SqlParameter("ID", SqlDbType.BigInt);
                _ID.Direction = ParameterDirection.Output;
                Insert.Parameters.Add(_ID);
                Insert.ExecuteNonQuery();
                ID = int.Parse(_ID.Value.ToString());
                return ID;
            }
        }

        //اجرای دستوری که فقط یک دیتاتیبل برمیگرداند
        /// <summary>
        ///*** Run sql command that return Data Table ***
        /// </summary>
        ///<param name="CommandText">
        ///Sql Command
        ///</param>
        ///<param name="Con">
        ///Sql Connection
        ///</param>
        public DataTable ReturnTable(string ConString, string CommandText)
        {
            SqlConnection Con = new SqlConnection(ConString);
            DataTable Table = new DataTable();
            SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, Con);
            Adapter.Fill(Table);
            return Table;
        }

        //برگرداندن عناوین امضای برگه مشخص شده
        public string[] Signature(int RowNumber)
        {
            using (SqlConnection ConSub = new SqlConnection(Properties.Settings.Default.BASE))
            {
                ConSub.Open();
                SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_125_Signature where ColumnId=" + RowNumber, ConSub);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                string[] Names = new string[8];
                Names[0] = (Table.Rows[0]["Column01"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column01"].ToString());
                Names[1] = (Table.Rows[0]["Column02"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column02"].ToString());
                Names[2] = (Table.Rows[0]["Column03"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column03"].ToString());
                Names[3] = (Table.Rows[0]["Column04"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column04"].ToString());
                Names[4] = (Table.Rows[0]["Column05"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column05"].ToString());
                Names[5] = (Table.Rows[0]["Column06"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column06"].ToString());
                Names[6] = (Table.Rows[0]["Column07"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column07"].ToString());
                Names[7] = (Table.Rows[0]["Column08"].ToString().Trim() == "" ? " " : Table.Rows[0]["Column08"].ToString());
                return Names;
            }
        }

        //چک صدور سند/حواله/رسید برای فاکتورها      
        /// <summary>
        /// Check If for this paper exported Doc/Receipt/Draft before.
        /// </summary>
        /// <param name="ConString"></param>
        /// <param name="TableName"></param>
        /// <param name="IDColumn">Paper Id Column</param>
        /// <param name="IDValue">Paper Id</param>
        /// <param name="ColumnName">Draft/Receipt/Doc Column in that table</param>
        /// <param name="CheckTypeName">Doc/Draft/Receipt Name in farsi to show proper message to user</param>

        public void CheckPapers(string ConString, string TableName, string IDColumn, string IDValue, string ColumnName, string CheckTypeName)
        {
            DataTable Table = ReturnTable(ConString, "Select " + ColumnName + " from " + TableName + " where " + IDColumn + "=" + IDValue);
            if (Table.Rows.Count == 0)
                throw new Exception("برگه مورد نظر حذف شده است");
            else
            {
                if (Table.Rows[0][0].ToString() == "True")
                    throw new Exception("برای این برگه قبلاً،  " + CheckTypeName + "  صادر شده است");
                else return;

                if (Table.Rows[0][0].ToString().Trim() != "" && int.Parse(Table.Rows[0][0].ToString()) > 0)
                    throw new Exception("برای این برگه قبلاً،  " + CheckTypeName + "  صادر شده است");
            }
        }

        /// <summary>
        ///  مقدار ستونهای عملیاتی فاکتورهای خرید و فروش و مرجوعی ها را را جهت انجام عملیات های مربوط چک می کند   
        /// </summary>
        /// <param name="TableName">نام جدول</param>
        /// <param name="ColumnName">ستون عملیاتی</param>
        /// <param name="IDValue">آی دی سطر</param>
        /// <returns>آی دی برگه عملیاتی</returns>
        public int OperationalColumnValue(string TableName, string ColumnName, string IDValue)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL(" + ColumnName + ",0) from " + TableName + " where ColumnId=" + IDValue + "),0)", Con);
                return int.Parse(Comm.ExecuteScalar().ToString());
            }
        }
        public int WHRSOperationalColumnValue(string TableName, string ColumnName, string IDValue)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL(" + ColumnName + ",0) from " + TableName + " where ColumnId=" + IDValue + "),0)", Con);
                return int.Parse(Comm.ExecuteScalar().ToString());
            }
        }

        /// <summary>
        /// Return Paper's Sanad Type
        /// </summary>
        /// <param name="ConnectinString"></param>
        /// <param name="IDColumn"></param>
        /// <param name="PaperID"></param>
        /// <returns></returns>
        public int SanadType(string ConnectinString, int DocID, int PaperID, int PaperType)
        {
            using (SqlConnection Con = new SqlConnection(ConnectinString))
            {
                Con.Open();
                SqlCommand SelectCommand = new SqlCommand(@"Select ISNULL((select distinct Column16 from Table_065_SanadDetail where Column00=" + DocID + " and Column17=" + PaperID + " and Column16=" + PaperType + "),0)", Con);
                return int.Parse(SelectCommand.ExecuteScalar().ToString());
            }
        }


        /// <summary>
        /// موجودی کالا در تمام انبارها برگردانده می شود
        /// </summary>
        /// <param name="GoodId">آی دی کالا</param>
        /// <param name="Date">موجودی تا چه تاریخی محاسبه شود؟</param>
        /// <returns></returns>

        public decimal Avaliable(string GoodId, string Date, Int16 ware)
        {

            decimal avalibale = 0;
            try
            {
                string Commandtxt = @"SELECT SUM(Resid) -SUM(Draft)  -SUM(isnull(NotDelivary,0)) AS Remain
FROM   (
           SELECT Table_008_Child_PwhrsDraft.column02 AS GoodID,
                  0.000 AS Resid,
                  SUM(Table_008_Child_PwhrsDraft.column07) AS Draft,
                  0.000 AS Delivary,
                  0.000 AS NotDelivary
           FROM   Table_008_Child_PwhrsDraft
                  INNER JOIN Table_007_PwhrsDraft
                       ON  Table_007_PwhrsDraft.columnid = 
                           Table_008_Child_PwhrsDraft.column01
           WHERE  (Table_007_PwhrsDraft.column02 <= '{0}')
                  AND Table_007_PwhrsDraft.column03 = {2}
           GROUP BY
                  Table_008_Child_PwhrsDraft.column02
           HAVING Table_008_Child_PwhrsDraft.column02 = {1}
           
           UNION ALL
           
           SELECT Table_012_Child_PwhrsReceipt.column02 AS GoodID,
                  SUM(Table_012_Child_PwhrsReceipt.column07) AS Resid,
                  0.000 AS Draft,
                  0.000 AS Delivary,
                  0.000 AS NotDelivary
           FROM   Table_012_Child_PwhrsReceipt
                  INNER JOIN Table_011_PwhrsReceipt
                       ON  Table_011_PwhrsReceipt.columnid = 
                           Table_012_Child_PwhrsReceipt.column01
           WHERE  (Table_011_PwhrsReceipt.column02 <= '{0}')
                  AND Table_011_PwhrsReceipt.column03 = {2}
           GROUP BY
                  Table_012_Child_PwhrsReceipt.column02
           HAVING Table_012_Child_PwhrsReceipt.column02 = {1}
           
           UNION ALL
           SELECT tcsf.column02 AS GoodID,
                  0 AS Resid,
                  0.000 AS Draft,
                  SUM(ISNULL(tcsf.column07, 0)) AS Delivary,
                  0.000 AS NotDelivary
           FROM   " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor tcsf
                  JOIN " + ConSale.Database + @".dbo.Table_010_SaleFactor tsf
                       ON  tsf.columnid = tcsf.column01
           WHERE  tsf.Column17 = 0
                  AND tsf.Column19 = 0
                  AND tsf.Column50 = 1--تحويل شده
                  AND tsf.Column53 = 0-- بسته نشده
                  AND tsf.Column42 = {2}
           GROUP BY
                  tcsf.column02
           HAVING tcsf.column02 = {1}
           UNION ALL
           SELECT tcsf.column02 AS GoodID,
                  0 AS Resid,
                  0.000 AS Draft,
                  0 AS Delivary,
                  SUM(ISNULL(tcsf.column07, 0)) AS NotDelivary
           FROM   " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor tcsf
                  JOIN " + ConSale.Database + @".dbo.Table_010_SaleFactor tsf
                       ON  tsf.columnid = tcsf.column01
           WHERE  tsf.Column17 = 0 --باطل نیست
                  AND tsf.Column19 = 0--مرجوعی نشده
                  AND tsf.Column09 = 0--تحويل نشده
                  AND tsf.Column42 = {2}
           GROUP BY
                  tcsf.column02
           HAVING tcsf.column02 = {1}
       ) AS tbl
 
       
            ";
                Commandtxt = string.Format(Commandtxt, Date, GoodId, ware);

                using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS))
                {
                    ConWare.Open();
                    SqlCommand Com = new SqlCommand(Commandtxt, ConWare);
                    avalibale = Convert.ToDecimal(Com.ExecuteScalar().ToString());

                }

            }
            catch
            {
            }
            return avalibale;

        }
        public DataTable GoodRemain(string GoodId, string Date)
        {
            string Commandtxt = @"Select GoodID,Ware,Table_001_PWHRS.column02 as WareName, SUM(Resid)-SUM(Draft) as Remain from(
            SELECT     Table_008_Child_PwhrsDraft.column02 AS GoodID, Table_007_PwhrsDraft.column03 AS Ware,0.000 as Resid,
             SUM(Table_008_Child_PwhrsDraft.column07) AS Draft
                  
            FROM         Table_008_Child_PwhrsDraft INNER JOIN
                                  Table_007_PwhrsDraft ON Table_007_PwhrsDraft.columnid = Table_008_Child_PwhrsDraft.column01 
            WHERE     (Table_007_PwhrsDraft.column02 <= '{0}')
            GROUP BY Table_008_Child_PwhrsDraft.column02, Table_007_PwhrsDraft.column03
            having Table_008_Child_PwhrsDraft.column02={1}
            union all

            SELECT     Table_012_Child_PwhrsReceipt.column02 AS GoodID, Table_011_PwhrsReceipt.column03 AS Ware, SUM(Table_012_Child_PwhrsReceipt.column07) AS Resid, 0.000 as Draft
            FROM       Table_012_Child_PwhrsReceipt
                              INNER JOIN
                                  Table_011_PwhrsReceipt ON Table_011_PwhrsReceipt.columnid = Table_012_Child_PwhrsReceipt.column01
            WHERE     (Table_011_PwhrsReceipt.column02 <= '{0}')
            GROUP BY Table_012_Child_PwhrsReceipt.column02, Table_011_PwhrsReceipt.column03
            having Table_012_Child_PwhrsReceipt.column02={1}

            ) as tbl
            inner join Table_001_PWHRS on Table_001_PWHRS.columnid=tbl.Ware
            group by GoodID,Ware,Table_001_PWHRS.Column02
            ";
            Commandtxt = string.Format(Commandtxt, Date, GoodId);
            return ReturnTable(Properties.Settings.Default.WHRS, Commandtxt);

        }

        public DataTable TotalGoodRemain(string GoodId, string Date, Int16 ware)
        {
            string Commandtxt = @"  SELECT SUM(Resid) -SUM(Draft) AS Remain
                                    FROM   (
                                               SELECT 0.000 AS Resid,
                                                      SUM(Table_008_Child_PwhrsDraft.column07) AS Draft
                                               FROM   Table_008_Child_PwhrsDraft
                                                      INNER JOIN Table_007_PwhrsDraft
                                                           ON  Table_007_PwhrsDraft.columnid = 
                                                               Table_008_Child_PwhrsDraft.column01
                                               WHERE  (Table_007_PwhrsDraft.column02 <= '{0}')
                                                      AND Table_008_Child_PwhrsDraft.column02 = {1} and Table_007_PwhrsDraft.column03=" + ware + @"
                                               UNION ALL
           
                                               SELECT SUM(Table_012_Child_PwhrsReceipt.column07) AS Resid,
                                                      0.000 AS Draft
                                               FROM   Table_012_Child_PwhrsReceipt
                                                      INNER JOIN Table_011_PwhrsReceipt
                                                           ON  Table_011_PwhrsReceipt.columnid = 
                                                               Table_012_Child_PwhrsReceipt.column01
                                               WHERE  (Table_011_PwhrsReceipt.column02 <= '{0}')
                                                      AND Table_012_Child_PwhrsReceipt.column02 = {1} and Table_011_PwhrsReceipt.column03=" + ware + @"
                                           ) AS tbl
                                
            ";
            Commandtxt = string.Format(Commandtxt, Date, GoodId);
            return ReturnTable(Properties.Settings.Default.WHRS, Commandtxt);

        }


        public DataTable OldTotalGoodRemain(string GoodId, string Date)
        {
            string Commandtxt = @"  SELECT SUM(Resid) -SUM(Draft) AS Remain
                                    FROM   (
                                               SELECT 0.000 AS Resid,
                                                      SUM(Table_008_Child_PwhrsDraft.column07) AS Draft
                                               FROM   Table_008_Child_PwhrsDraft
                                                      INNER JOIN Table_007_PwhrsDraft
                                                           ON  Table_007_PwhrsDraft.columnid = 
                                                               Table_008_Child_PwhrsDraft.column01
                                               WHERE  (Table_007_PwhrsDraft.column02 <= '{0}')
                                                      AND Table_008_Child_PwhrsDraft.column02 = {1}  
                                               UNION ALL
           
                                               SELECT SUM(Table_012_Child_PwhrsReceipt.column07) AS Resid,
                                                      0.000 AS Draft
                                               FROM   Table_012_Child_PwhrsReceipt
                                                      INNER JOIN Table_011_PwhrsReceipt
                                                           ON  Table_011_PwhrsReceipt.columnid = 
                                                               Table_012_Child_PwhrsReceipt.column01
                                               WHERE  (Table_011_PwhrsReceipt.column02 <= '{0}')
                                                      AND Table_012_Child_PwhrsReceipt.column02 = {1}  
                                           ) AS tbl
                                
            ";
            Commandtxt = string.Format(Commandtxt, Date, GoodId);
            return ReturnTable(Properties.Settings.Default.WHRS, Commandtxt);

        }
        public DataTable OldTotalGoodReservations(string GoodId, int Orderid)
        {
            SqlConnection ConWHRS = new SqlConnection(Properties.Settings.Default.WHRS);

            string Commandtxt = string.Empty;
            if (Orderid > 0)

                Commandtxt = @"  SELECT ISNULL(SUM(ISNULL(tod.column06, 0) - ISNULL(tod.column17, 0)), 0) AS 
                                                   Reservations
                                            FROM   Table_005_OrderHeader toh
                                                   JOIN Table_006_OrderDetails tod
                                                        ON  tod.column01 = toh.columnid
                                            WHERE  tod.column02 = {0}
                                                   AND toh.columnid != " + Orderid + @"   
                                        
                                     ";
            else
                Commandtxt = @"  SELECT ISNULL(SUM(ISNULL(tod.column06, 0) - ISNULL(tod.column17, 0)), 0) AS 
                               Reservations
                        FROM   Table_005_OrderHeader toh
                               JOIN Table_006_OrderDetails tod
                                    ON  tod.column01 = toh.columnid
                        WHERE  tod.column02 = {0}
                                
                                                   
                                        
                                      ";

            Commandtxt = string.Format(Commandtxt, GoodId);
            return ReturnTable(Properties.Settings.Default.SALE, Commandtxt);

        }

        public DataTable TotalGoodReservations(string GoodId, int Orderid, Int16 ware)
        {
            SqlConnection ConWHRS = new SqlConnection(Properties.Settings.Default.WHRS);

            string Commandtxt = string.Empty;
            if (Orderid > 0)

                Commandtxt = @" SELECT ISNULL(SUM(ISNULL(tod.column06, 0) - ISNULL(tod.column17, 0)), 0) AS 
                                                   Reservations
                                            FROM   Table_005_OrderHeader toh
                                                   JOIN Table_006_OrderDetails tod
                                                        ON  tod.column01 = toh.columnid
                                            WHERE  tod.column02 = {0}
                                                   AND toh.column40 = " + ware + @"
                                                   AND toh.columnid != " + Orderid + @"  
                                
                             ";
            else
                Commandtxt = @"  SELECT ISNULL(SUM(ISNULL(tod.column06, 0) - ISNULL(tod.column17, 0)), 0) AS 
                               Reservations
                        FROM   Table_005_OrderHeader toh
                               JOIN Table_006_OrderDetails tod
                                    ON  tod.column01 = toh.columnid
                        WHERE  tod.column02 = {0}
                               AND toh.column40 = " + ware + @"
                                
                              ";

            Commandtxt = string.Format(Commandtxt, GoodId);
            return ReturnTable(Properties.Settings.Default.SALE, Commandtxt);

        }



        //حذف گردشهای چک دریافتی از جدول گردش دریافتها
        public void DeleteTurnReception(Int64 TurnID)
        {
            using (SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK))
            {
                ConBank.Open();
                SqlCommand Delete = new SqlCommand("Delete from Table_065_TurnReception where ColumnId=" + TurnID, ConBank);
                Delete.ExecuteNonQuery();
            }
        }

        internal bool IsGood(string GoodId)
        {
            using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                ConWare.Open();
                SqlCommand Com = new SqlCommand("Select Column29 from table_004_CommodityAndIngredients where ColumnId=" + GoodId, ConWare);
                bool Result = Convert.ToBoolean(Com.ExecuteScalar().ToString());
                return !Result;
            }
        }

        internal bool AllService(System.Windows.Forms.BindingSource TableBindSource)
        {
            bool Check = true;
            foreach (DataRowView item in TableBindSource)
            {
                if (IsGood(item["Column02"].ToString()))
                    Check = false;
            }
            return Check;
        }


        /// <summary>
        /// برگردان آخرین تخفیف و اضافه خطی بر اساس فاکتور فروش یک مشتری
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="GoodId"></param>
        /// <returns></returns>
        public double[] LastLinearDiscount(int CustomerId, int GoodId)
        {
            SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     TOP (1) PERCENT dbo.Table_010_SaleFactor.column02 AS Date, dbo.Table_010_SaleFactor.column03 AS CustomerId, 
            dbo.Table_011_Child1_SaleFactor.column02 AS GoodID, dbo.Table_011_Child1_SaleFactor.column16 AS Discount, 
            dbo.Table_011_Child1_SaleFactor.column18 AS Extra, 
            dbo.Table_010_SaleFactor.columnid AS SaleID
            FROM         dbo.Table_011_Child1_SaleFactor INNER JOIN
            dbo.Table_010_SaleFactor ON dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
            WHERE     (dbo.Table_010_SaleFactor.column03 = " + CustomerId + @") AND (dbo.Table_011_Child1_SaleFactor.column02 = " + GoodId + @")
            ORDER BY Date DESC, SaleID DESC", ConSale);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            double[] array = new double[2];
            if (Table.Rows.Count == 0)
                return array;
            else
            {
                array[0] = Convert.ToDouble(Table.Rows[0]["Discount"].ToString());
                array[1] = Convert.ToDouble(Table.Rows[0]["Extra"].ToString());
                return array;
            }
        }
        public DataTable FillUnitCountByKala(Int32 GoodID)
        {
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
            string Commandtxt = @"SELECT    t.kalaID,
                                            tcui.Column00 AS countiD,
                                            ISNULL(t.buy, 0) AS buy,
                                            ISNULL(t.sale, 0) AS sale,
                                            ISNULL(t.customer, 0) AS customer,
                                            ISNULL(t.zarib, 0) AS zarib,
                                            tcui.Column01 AS countName
                                    FROM   " + ConBase.Database + @".dbo.Table_070_CountUnitInfo tcui
                                            LEFT JOIN (
                                                    SELECT tc.[Column00] AS kalaID,
                                                            tc.[Column01] AS countiD,
                                                            tc.[Column02] AS buy,
                                                            tc.[Column03] AS sale,
                                                            tc.[Column04] AS customer,
                                                            tc.[Column05] AS zarib,
                                                            tcui.Column01 AS countName
                                                    FROM   [dbo].[Table_031_CountUnit] tc
                                                            JOIN " + ConBase.Database + @".[dbo].Table_070_CountUnitInfo tcui
                                                                ON  tcui.Column00 = tc.Column01
                                                    WHERE  tc.[Column00] = " + GoodID + @" AND tcui.Column00 NOT IN (SELECT column07 FROM  table_004_CommodityAndIngredients WHERE columnid=" + GoodID + @")
                                                    UNION  all                                                                             
                                                    SELECT tcai.columnid AS kalaID,
                                                            tcai.column07 AS countiD,
                                                            CASE 
                                                                WHEN TS003.Column03 IS NULL THEN tcai.Column35
                                                                ELSE TS003.Column03
                                                            END AS buy,
                                                            CASE 
                                                                WHEN TS003.Column07 IS NULL THEN tcai.Column34
                                                                ELSE TS003.Column07
                                                            END AS sale,
                                                            tcai.Column36 AS customer,
                                                            1 AS zarib,
                                                            tcui.Column01 AS countName
                                                    FROM   table_004_CommodityAndIngredients tcai
                                                            JOIN " + ConBase.Database + @".[dbo].Table_070_CountUnitInfo tcui
                                                                ON  tcui.Column00 = tcai.column07
                                                            LEFT OUTER JOIN (
                                                                    SELECT columnid,
                                                                            column01,
                                                                            column02,
                                                                            column03,
                                                                            column04,
                                                                            column05,
                                                                            column06,
                                                                            column07,
                                                                            column08,
                                                                            column09,
                                                                            column10,
                                                                            Column11
                                                                    FROM   dbo.Table_003_InformationProductCash
                                                                ) AS TS003
                                                                ON  tcai.columnid = TS003.column01
                                                    WHERE  tcai.columnid = " + GoodID + @"
                                                ) AS t
                                                ON  t.countiD = tcui.Column00 ORDER BY t.kalaID Desc";

            return ReturnTable(Properties.Settings.Default.WHRS, Commandtxt);

        }

        public float GetZarib(int GoodID, Int16 FromCountUnit, Int16 ToCountUnit)
        {
            float zarib = 1;
            try
            {
                SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);

                using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS))
                {
                    ConWare.Open();

                    SqlCommand command = ConWare.CreateCommand();
                    SqlTransaction transaction;

                    // Start a local transaction.
                    transaction = ConWare.BeginTransaction(
                        IsolationLevel.ReadCommitted, "SampleTransaction");

                    // Must assign both transaction object and connection
                    // to Command object for a pending local transaction.
                    command.Connection = ConWare;
                    command.Transaction = transaction;
                    try
                    {
                        command.CommandText = @"
                                                                DECLARE @from SMALLINT = " + FromCountUnit + @"
                                                                DECLARE @to SMALLINT = " + ToCountUnit + @"
                                                                DECLARE @fromzarib   FLOAT = 0,
                                                                        @fromVzarib  FLOAT = 0,
                                                                        @tozarib     FLOAT = 0,
                                                                        @toVzarib    FLOAT = 0

                                                                IF OBJECT_ID('tempdb.dbo.#Temp') IS NOT NULL
                                                                    DROP TABLE #Temp
		
                                                                SELECT *
                                                                       INTO #Temp
                                                                FROM   (
                                                                           SELECT tc.[Column00] AS kalaID,
                                                                                  tc.[Column01] AS countiD,
                                                                                  tc.[Column02] AS buy,
                                                                                  tc.[Column03] AS sale,
                                                                                  tc.[Column04] AS customer,
                                                                                  tc.[Column05] AS zarib,
                                                                                  (1 / NULLIF(CAST(tc.[Column05] AS FLOAT), 0)) AS vzarib,
                                                                                  tcui.Column01 AS countName
                                                                           FROM   [dbo].[Table_031_CountUnit] tc
                                                                                  JOIN " + ConBase.Database + @".[dbo].Table_070_CountUnitInfo 
                                                                                       tcui
                                                                                       ON  tcui.Column00 = tc.Column01
                                                                           WHERE  tc.[Column00] = " + GoodID + @"
                                                                           UNION ALL                                                                             
                                                                           SELECT tcai.columnid AS kalaID,
                                                                                  tcai.column07 AS countiD,
                                                                                  CASE 
                                                                                       WHEN TS003.Column03 IS NULL THEN tcai.Column35
                                                                                       ELSE TS003.Column03
                                                                                  END AS buy,
                                                                                  CASE 
                                                                                       WHEN TS003.Column07 IS NULL THEN tcai.Column34
                                                                                       ELSE TS003.Column07
                                                                                  END AS sale,
                                                                                  tcai.Column36 AS customer,
                                                                                  1 AS zarib,
                                                                                  1 AS vzarib,
                                                                                  tcui.Column01 AS countName
                                                                           FROM   table_004_CommodityAndIngredients tcai
                                                                                  JOIN " + ConBase.Database + @".[dbo].Table_070_CountUnitInfo 
                                                                                       tcui
                                                                                       ON  tcui.Column00 = tcai.column07
                                                                                  LEFT OUTER JOIN (
                                                                                           SELECT columnid,
                                                                                                  column01,
                                                                                                  column02,
                                                                                                  column03,
                                                                                                  column04,
                                                                                                  column05,
                                                                                                  column06,
                                                                                                  column07,
                                                                                                  column08,
                                                                                                  column09,
                                                                                                  column10,
                                                                                                  Column11
                                                                                           FROM   dbo.Table_003_InformationProductCash
                                                                                       ) AS TS003
                                                                                       ON  tcai.columnid = TS003.column01
                                                                           WHERE  tcai.columnid = " + GoodID + @"
                                                                       ) AS zaribtable

                                                                SET @fromzarib = (
                                                                        SELECT zarib
                                                                        FROM   #Temp AS j
                                                                        WHERE  countiD = @from
                                                                    )

                                                                SET @fromVzarib = (
                                                                        SELECT vzarib
                                                                        FROM   #Temp AS j
                                                                        WHERE  countiD = @from
                                                                    )

                                                                SET @tozarib = (
                                                                        SELECT zarib
                                                                        FROM   #Temp AS j
                                                                        WHERE  countiD = @to
                                                                    )

                                                                SET @toVzarib = (
                                                                        SELECT vzarib
                                                                        FROM   #Temp AS j
                                                                        WHERE  countiD = @to
                                                                    )

                                                                IF @fromzarib IS NULL
                                                                   OR @fromVzarib IS NULL
                                                                   OR @tozarib IS NULL
                                                                   OR @toVzarib IS NULL
                                                                    SELECT 1

                                                                IF @fromzarib = 1
                                                                    SELECT ISNULL(@tozarib, 1)

                                                                IF @fromzarib != 1
                                                                   AND @tozarib = 1
                                                                    SELECT ISNULL(@fromVzarib, 1)

                                                                IF @fromzarib != 1
                                                                   AND @tozarib != 1
                                                                    SELECT ISNULL(CAST(@fromVzarib * @tozarib AS FLOAT), 1)

                                                                --SELECT *
                                                                --FROM   #Temp

                                                                IF OBJECT_ID('tempdb.dbo.#Temp') IS NOT NULL
                                                                    DROP TABLE #Temp
		                                                                ";
                        zarib = float.Parse(command.ExecuteScalar().ToString());
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (SqlException ex)
                        {
                            if (transaction.Connection != null)
                            {

                            }
                        }


                    }
                }
            }
            catch
            {
            }
            return zarib;
        }
        public System.Windows.Forms.DataVisualization.Charting.SeriesChartType SetChartType(string ChartType)
        {
            if (ChartType == "StackedBar100")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedBar100;

            if (ChartType == "StackedColumn")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;

            if (ChartType == "StackedColumn100")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn100;

            if (ChartType == "StepLine")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;

            if (ChartType == "Stock")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Stock;

            if (ChartType == "ThreeLineBreak")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.ThreeLineBreak;

            if (ChartType == "Radar")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;

            if (ChartType == "Range")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Range;

            if (ChartType == "RangeBar")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeBar;

            if (ChartType == "RangeColumn")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeColumn;

            if (ChartType == "Renko")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Renko;

            if (ChartType == "Spline")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;

            if (ChartType == "SplineArea")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;

            if (ChartType == "SplineRange")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineRange;

            if (ChartType == "StackedArea")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedArea;

            if (ChartType == "StackedArea100")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedArea100;

            if (ChartType == "StackedBar")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedBar;

            if (ChartType == "FastLine")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;

            if (ChartType == "FastPoint")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;

            if (ChartType == "Funnel")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Funnel;

            if (ChartType == "Kagi")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Kagi;

            if (ChartType == "Line")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            if (ChartType == "Pie")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            if (ChartType == "Point")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

            if (ChartType == "PointAndFigure")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.PointAndFigure;

            if (ChartType == "Polar")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;

            if (ChartType == "Pyramid")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pyramid;

            if (ChartType == "Area")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;

            if (ChartType == "Bar")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;

            if (ChartType == "BoxPlot")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.BoxPlot;

            if (ChartType == "Bubble")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bubble;

            if (ChartType == "Candlestick")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;

            if (ChartType == "Column")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

            if (ChartType == "Doughnut")
                return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;


            return System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
        }

        public DataTable GetDefaultValues()
        {
            SqlConnection ConBASE = new SqlConnection(Properties.Settings.Default.BASE);

            DataTable StoreTable = new DataTable();
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT  
                                                                   tsi.ColumnId                          Number,
                                                                   tsi.Column05                          project,
                                                                   tsi.Column04                          ware,
                                                                   tsi.Column06                          saleman,
                                                                   tsi.Column07                          costcenter,
                                                                   tsi.Column08                          stock,
                                                                   tsi.Column14                          buyer,
                                                                   tsi.Column15                          saletype,
                                                                   tui.Column02 [admin]
                                                            FROM   " + ConMAIN.Database + @".dbo.Table_010_UserInfo   AS tui
                                                                   LEFT JOIN dbo.Table_295_StoreInfo  AS tsi
                                                                   JOIN dbo.Table_296_StoreUsers      AS tsu
                                                                        ON  tsu.Column00 = tsi.ColumnId
                                                                        ON  tsu.Column01 = tui.Column00
                                                            WHERE  tui.Column00 = '" + Class_BasicOperation._UserName + @"'
                                                                   AND tui.Column05 = " + Class_BasicOperation._OrgCode + @"
                                                                   AND tui.Column06 = '" + Class_BasicOperation._Year + @"'", ConBASE);
            Adapter.Fill(StoreTable);
            return StoreTable;
        }

        public bool CheckAccess(Int32 FactorID, FactorType Type)
        {
            bool ok = false;

            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.MAIN))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("Select Column02 from Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" +
                Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + "'", Con);
                if (bool.Parse(Select.ExecuteScalar().ToString()))
                    return true;
            }
            SqlConnection ConBASE = new SqlConnection(Properties.Settings.Default.BASE);
            DataTable StoreTable = new DataTable();
            DataTable FactorTable = new DataTable();

            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT tsi.Column05
                                                        FROM   dbo.Table_295_StoreInfo AS tsi
                                                               JOIN dbo.Table_296_StoreUsers AS tsu
                                                                    ON  tsu.Column00 = tsi.ColumnId
                                                        WHERE tsu.Column01='" + Class_BasicOperation._UserName + "'", ConBASE);
            Adapter.Fill(StoreTable);
            if (StoreTable.Rows.Count == 0)
                return false;

            if (Type == FactorType.Sale)
            {
                Adapter = new SqlDataAdapter(@"SELECT tsf.Column44 as project
                                                    FROM   Table_010_SaleFactor AS tsf
                                                    WHERE  tsf.columnid = " + FactorID, ConSale);
                Adapter.Fill(FactorTable);
                if (FactorTable.Rows.Count == 0)
                    return false;
                if (FactorTable.Rows[0]["project"] != DBNull.Value &&
                    FactorTable.Rows[0]["project"] != null &&
                    !string.IsNullOrWhiteSpace(FactorTable.Rows[0]["project"].ToString()) &&
                    Convert.ToInt16(FactorTable.Rows[0]["project"]) == Convert.ToInt16(StoreTable.Rows[0]["Column05"]))
                    return true;
                else
                    return false;


            }
            else if (Type == FactorType.Buy)
            {
                Adapter = new SqlDataAdapter(@"SELECT tsf.Column29 as project
                                                    FROM   Table_015_BuyFactor AS tsf
                                                    WHERE  tsf.columnid = " + FactorID, ConSale);
                Adapter.Fill(FactorTable);
                if (FactorTable.Rows.Count == 0)
                    return false;
                if (FactorTable.Rows[0]["project"] != DBNull.Value &&
                    FactorTable.Rows[0]["project"] != null &&
                    !string.IsNullOrWhiteSpace(FactorTable.Rows[0]["project"].ToString()) &&
                    Convert.ToInt16(FactorTable.Rows[0]["project"]) == Convert.ToInt16(StoreTable.Rows[0]["Column05"]))
                    return true;
                else
                    return false;


            }
            else if (Type == FactorType.ReturnSale)
            {
                Adapter = new SqlDataAdapter(@"SELECT tsf.Column30 as project
                                                    FROM   Table_018_MarjooiSale AS tsf
                                                    WHERE  tsf.columnid = " + FactorID, ConSale);
                Adapter.Fill(FactorTable);
                if (FactorTable.Rows.Count == 0)
                    return false;
                if (FactorTable.Rows[0]["project"] != DBNull.Value &&
                    FactorTable.Rows[0]["project"] != null &&
                    !string.IsNullOrWhiteSpace(FactorTable.Rows[0]["project"].ToString()) &&
                    Convert.ToInt16(FactorTable.Rows[0]["project"]) == Convert.ToInt16(StoreTable.Rows[0]["Column05"]))
                    return true;
                else
                    return false;


            }
            else if (Type == FactorType.ReturnBuy)
            {
                Adapter = new SqlDataAdapter(@"SELECT tsf.Column28 as project
                                                    FROM   Table_021_MarjooiBuy AS tsf
                                                    WHERE  tsf.columnid = " + FactorID, ConSale);
                Adapter.Fill(FactorTable);
                if (FactorTable.Rows.Count == 0)
                    return false;
                if (FactorTable.Rows[0]["project"] != DBNull.Value &&
                    FactorTable.Rows[0]["project"] != null &&
                    !string.IsNullOrWhiteSpace(FactorTable.Rows[0]["project"].ToString()) &&
                    Convert.ToInt16(FactorTable.Rows[0]["project"]) == Convert.ToInt16(StoreTable.Rows[0]["Column05"]))
                    return true;
                else
                    return false;


            }


            return ok;

        }

    }
}
