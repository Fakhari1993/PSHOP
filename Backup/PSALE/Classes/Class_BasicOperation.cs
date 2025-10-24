using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;

namespace PSALE
{
    public static class Class_BasicOperation
    {
        public static Int16 _OrgCode;
    
        public static string _UserName;
        public static string _Year;
        public static bool isNotDigit(char C)
        {
            if (!char.IsControl(C) && !char.IsDigit(C))
                return true;
            return false;
        }

        public static void isEnter(char C)
        {
            if (C==13)
                SendKeys.Send("{TAB}");
        }

        public static void ChangeLanguage(string Culture)
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo(Culture));
        }

        public static void ShowMsg(string Title, string Text, string Type)
        {
            if (Type == "Warning")
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            else if (Type == "Information")
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            else if (Type == "Stop")
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            else if (Type == "None")
                MessageBox.Show(Text, Title, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
        }

        public static string ConStringM()
        {
            return (Properties.Settings.Default.PSALE_ConnectionString);
        }

        public static string ConStringB()
        {
            return (Properties.Settings.Default.PERP_ConnectionString);
        }

        public static void CheckExceptionType(Exception ex,string FormName)
        {
            System.Data.NoNullAllowedException Notnull = new System.Data.NoNullAllowedException();
            System.Data.ConstraintException Unique = new System.Data.ConstraintException();
            System.OverflowException Overflow = new OverflowException();
            System.Data.InvalidConstraintException Relation = new System.Data.InvalidConstraintException();
            InvalidOperationException invalid=new InvalidOperationException();

         



            if (ex.GetBaseException().GetType() == Notnull.GetBaseException().GetType())
            {
                MessageBox.Show("اطلاعات مورد نیاز را تکمیل کنید");
                return;
            }
            else if (ex.GetBaseException().GetType() == Unique.GetBaseException().GetType())
            {
                MessageBox.Show("کد/اطلاعات وارد شده تکراری می باشد");
                return;
            }
            else if (ex.GetBaseException().GetType() == Overflow.GetBaseException().GetType())
            {
                MessageBox.Show("طول داده وارده بیش از حد مجاز است");
                return;
            }
            else if (ex.GetBaseException().GetType() == Relation.GetBaseException().GetType())
            {
                MessageBox.Show("حذف اطلاعات امکانپذیر نمی باشد");
                return;
            }
          
            else
            {
                if (ex.GetBaseException().GetType() == invalid.GetBaseException().GetType())
                {
                    if (ex.Message.StartsWith("Item cannot be added to a read-only or fixed-size list") && FormName=="Form06_AccountingHeaders")
                    {
                        MessageBox.Show("سرفصل حساب در سطح بالاتر مشخص نگردیده است");
                        return;
                    }
                    else if (ex.Message.StartsWith("Item cannot be added to a read-only or fixed-size list") && FormName == "Form01_AccDocument")
                    {
                        MessageBox.Show("در ابتدا، سند جدید درج نمایید");
                        return;
                    }
             


                }
    

                else
                {
                    if (ex.Message.StartsWith("The DELETE statement conflicted with the REFERENCE constraint"))
                    {
                        MessageBox.Show("حذف اطلاعات امکانپذیر نمی باشد");
                        return;

                    }
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        public static void CheckSqlExp(System.Data.SqlClient.SqlException ex,string FormName)
        {
            if (ex.Message.StartsWith("Violation of UNIQUE KEY constraint"))
            {
                if (FormName == "Form06_AccountingHeaders")
                {
                    MessageBox.Show("کد وارد شده تکراری می باشد");
                    return;
                }
            }
            else if(ex.Message.StartsWith("The DELETE statement conflicted with"))
            {
                MessageBox.Show("حذف ردیف جاری اطلاعات امکانپذیر نمی باشد");
                return;
            }
            else
                throw new Exception(ex.Message);
        }

        public static int LastIDNumber()
        {
            SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE_ConnectionString);
            Con.Open();
            SqlCommand Command=new SqlCommand ("Select ISNULL(Max(Column00),0)+1 from Table_060_SanadHead", Con);
            int SanadNo = int.Parse(Command.ExecuteScalar().ToString());
            return SanadNo;
        }

        public static string HeadCompName(string ACC_Code)
        {
            SqlConnection Con = new SqlConnection(Properties.Settings.Default.PSALE_ConnectionString);
            Con.Open();
            SqlCommand Select = new SqlCommand("Select ACC_NameComplete from View001_Headers where ACC_Code='"+ACC_Code+"'", Con);
            return Select.ExecuteScalar().ToString();
        }

        
    }
}
