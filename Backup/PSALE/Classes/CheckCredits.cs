using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSALE.Classes
{
    class CheckCredits
    {
        private string PersonName(int PersonCode)
        {
            SqlConnection Con = new SqlConnection(Properties.Settings.Default.PERP_ConnectionString);
            Con.Open();
            SqlCommand Select = new SqlCommand("Select Column02 from Table_045_PersonInfo where ColumnId=" + PersonCode, Con);
            string _PersonName= Select.ExecuteScalar().ToString();
            Con.Close();
            return _PersonName;
        }

        private string AccountName(string AccountCode)
        {
            SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT_ConnectionString);
            Con.Open();
            SqlCommand Select = new SqlCommand("Select ACC_Name from AllHeaders() where ACC_Code='" + AccountCode + "'", Con);
            string _AccountName = Select.ExecuteScalar().ToString();
            Con.Close();
            return _AccountName;
        }

        public void CheckPersonCredit(DataTable Table,int _SanadNumber)
        {
            SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT_ConnectionString);
            Con.Open();
            SqlCommand Com=new SqlCommand("",Con);
            DataTable AggTable = Table.Clone();
            AggTable = Table.DefaultView.ToTable();

            //Group Rows by LINQ//
            var query = from p in AggTable.AsEnumerable()
                        group p by new  {Account = p.Field<string>("Account"), Person = p.Field<int>("Person")} into groupedTable
                        select new
                        {
                            Person=groupedTable.Key.Person,
                            Account=groupedTable.Key.Account,
                            sumOfValue = groupedTable.Sum(p => p.Field<Int64>("Price"))

                        };

            DataTable T = AggTable.Clone();
            foreach (var x in query)
            {
                T.Rows.Add(x.Person, x.Account, x.sumOfValue);
            }
            
            foreach (DataRow item in T.Rows)
            {
                
                Com.CommandText = "Select Column01,Column08,Column09 from Table_050_CerditInfo where "
                    + "Column00=" + item["Person"].ToString() + " and Column02='" + item["Account"].ToString() + "'";
                using (SqlDataReader Reader = Com.ExecuteReader())
                {
                    Reader.Read();
                    if (Reader.HasRows)
                    {
                        // Catch Suitable Action//
                        bool CreditKind = bool.Parse(Reader["Column01"].ToString());
                        Int64 CreditPrice = Int64.Parse(Reader["Column08"].ToString());
                        Int16 Action = Int16.Parse(Reader["Column09"].ToString());

                        Reader.Close();
                        //Catch  Remain for this person in this Account///
                        Com.CommandText = "Select ISNULL(Sum(Column11)-Sum(Column12),0) from Table_065_SanadDetail where Column01='" + item["Account"].ToString() + "'" +
                            " And Column07=" + item["Person"].ToString()+" And Column00<>"+_SanadNumber;
                        Int64 Remain = Int64.Parse(Com.ExecuteScalar().ToString());
                        Int64 Price=Int64.Parse(item["Price"].ToString());

                        if (CreditKind && (Remain+Price)<0)
                        {
                            //if ((Remain + Price) > 0)
                            //{
                            //    switch (Action)
                            //    {
                            //        case 1:
                            //            MessageBox.Show("عدم هماهنگی نوع اعتبار معرفی شده و مانده اعتبار شخص " + Environment.NewLine +
                            //                PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                            //                AccountName(item["Account"].ToString()));
                            //            break;
                            //        case 2:
                            //            throw new Exception("عدم هماهنگی نوع اعتبار معرفی شده و مانده اعتبار شخص " + Environment.NewLine +
                            //               PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                            //               AccountName(item["Account"].ToString()) + Environment.NewLine + "جهت ادامه عملیات اصلاح مبالغ الزامیست");
                            //    }
                            //}
                             if ( CreditPrice<Math.Abs((Remain + Price)))
                            {
                                switch (Action)
                                {
                                    case 1:
                                        MessageBox.Show("مانده اعتبار "+
                                            PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                                            AccountName(item["Account"].ToString())+" بیش از میزان اعتبار تعیین شده است ");
                                        break;
                                    case 2:
                                        throw new Exception(" مانده اعتبار "+
                                           PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                                           AccountName(item["Account"].ToString())+" بیش از میزان اعتبار تعیین شده است " + Environment.NewLine + "جهت ادامه عملیات اصلاح مبالغ الزامیست");
                                }
                            }
                        }
                        else if(!CreditKind && (Remain+Price)>0)
                        {
                            if ((Remain + Price) > CreditPrice)
                            {
                                switch (Action)
                                {
                                    case 1:
                                        MessageBox.Show("مانده اعتبار " +
                                            PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                                            AccountName(item["Account"].ToString()) + " بیش از میزان اعتبار تعیین شده است ");
                                        break;
                                    case 2:
                                        throw new Exception(" مانده اعتبار " +
                                           PersonName(int.Parse(item["Person"].ToString())) + " در حساب " +
                                           AccountName(item["Account"].ToString()) + " بیش از میزان اعتبار تعیین شده است " + Environment.NewLine + "جهت ادامه عملیات اصلاح مبالغ الزامیست");
                                }
                            }
                        }
                    }
                }

            }
        }

        public void CheckAccountCredit(DataTable Table,int _SanadNumber)
        {
            SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT_ConnectionString);
            Con.Open();
            SqlCommand Com = new SqlCommand("", Con);
            DataTable AggTable = Table.Clone();
            AggTable = Table.DefaultView.ToTable();

            //Group Rows by LINQ//
            var query = from p in AggTable.AsEnumerable()
                        group p by new { Account = p.Field<string>("Account")} into groupedTable
                        select new
                        {
                            Account = groupedTable.Key.Account,
                            sumOfValue = groupedTable.Sum(p => p.Field<Int64>("Price"))

                        };

            DataTable T = AggTable.Clone();
            foreach (var x in query)
            {
                T.Rows.Add(x.Account, x.sumOfValue);
            }

            foreach (DataRow item in T.Rows)
            {

                Com.CommandText = "Select Control_Type,Control_Action from AllHeaders() where "
                    + " ACC_Code='" + item["Account"].ToString() + "'";
                using (SqlDataReader Reader = Com.ExecuteReader())
                {
                    Reader.Read();
                    if (Reader.HasRows)
                    {
                        // Catch Suitable Action//
                        Int16 Control_Type = Int16.Parse(Reader["Control_Type"].ToString());
                        Int16 Control_Action = Int16.Parse(Reader["Control_Action"].ToString());

                        Reader.Close();
                        //Catch Before Ramin for this person in this Account///
                        Com.CommandText = "Select ISNULL(Sum(Column11)-Sum(Column12),0) from Table_065_SanadDetail where Column01='" + item["Account"].ToString() +
                            "'  And Column00<>"+_SanadNumber;
                        Int64 Remain = Int64.Parse(Com.ExecuteScalar().ToString());
                        Int64 Price = Int64.Parse(item["Price"].ToString());



                        switch (Control_Type)
                        {
                            ///////فقط بدهکار
                            case 1:
                                {
                                    if ((Remain+Price) < 0)
                                    {
                                        switch (Control_Action)
                                        {
                                            case 1:
                                                {
                                                    MessageBox.Show(" ماهيت حساب به شماره :   " + item["Account"].ToString() + Environment.NewLine + "به نام:" + AccountName(item["Account"].ToString()) + Environment.NewLine + "    بدهکار مي باشد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    throw new Exception(" ماهيت حساب به شماره :   " + item["Account"].ToString() + Environment.NewLine + "به نام:" + AccountName(item["Account"].ToString()) + Environment.NewLine + "    بدهکار مي باشد و با توجه نوع اقدام حساب،تا اصلاح حساب قادر به ذخيره کردن نمي باشيد");
                                                }
                                        }
                                    }
                                    break;

                                }
                            ///////////فقط بستانکار
                            case 2:
                                {
                                    if ((Remain + Price) > 0)
                                    {

                                        switch (Control_Action)
                                        {
                                            case 1:
                                                {
                                                    MessageBox.Show(" ماهيت حساب به شماره :   " + item["Account"].ToString() + Environment.NewLine + "به نام:" + AccountName(item["Account"].ToString()) + Environment.NewLine + "    بستانکار مي باشد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    throw new Exception(" ماهيت حساب به شماره :   " + item["Account"].ToString() + Environment.NewLine + "به نام:" + AccountName(item["Account"].ToString()) + Environment.NewLine + "    بستانکار مي باشد و با توجه نوع اقدام حساب، تا اصلاح حساب قادر به ذخيره کردن نمي باشيد");
                                                }
                                        }
                                    }
                                    break;

                                }

                        }
                    }
                }
            }
        }

        public bool Control_Person(string AccountCode)
        {
            SqlConnection Con=new SqlConnection(Properties.Settings.Default.PACNT_ConnectionString);
            Con.Open();
            SqlCommand Command = new SqlCommand("Select Control_Person from AllHeaders() where ACC_Code='"+AccountCode+"'", Con);
            bool Result= bool.Parse(Command.ExecuteScalar().ToString());
            Con.Close();
            return Result;

        }

        public bool Control_Center(string AccountCode)
        {
            SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT_ConnectionString);
            Con.Open();
            SqlCommand Command = new SqlCommand("Select Control_Center from AllHeaders() where ACC_Code='" + AccountCode + "'", Con);
            bool Result = bool.Parse(Command.ExecuteScalar().ToString());
            Con.Close();
            return Result;
        }

        public bool Control_Projcet(string AccountCode)
        {
            SqlConnection Con = new SqlConnection(Properties.Settings.Default.PACNT_ConnectionString);
            Con.Open();
            SqlCommand Command = new SqlCommand("Select Control_Project from AllHeaders() where ACC_Code='" + AccountCode + "'", Con);
            bool Result = bool.Parse(Command.ExecuteScalar().ToString());
            Con.Close();
            return Result;
        }
    }
}
