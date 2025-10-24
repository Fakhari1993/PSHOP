using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
using Janus.Windows.GridEX;

namespace PSHOP
{
    public static class Class_BasicOperation
    {
        public static Int16 _OrgCode;

        public static string _UserName;
        public static string _Year;
        public static bool _WareType;
        public static bool _FinType;
        public enum FilterColumnType { GoodCode, Others, ACCColumn };
        public enum MessageType { Warning, Information, Stop, None };
        public static void SetCurrentConnection(string ConStr, string Org = "", string Year = "")
        {
            Class_ChangeConnectionString.CurrentConnection = ConStr.Replace("PERP_MAIN", "PSALE_" + Org + "_" + Year);
        }
        public static bool isNotDigit(char C)
        {
            if (!char.IsControl(C) && !char.IsDigit(C))
                return true;
            return false;
        }

        public static void isEnter(char C)
        {
            if (C == 13)
                SendKeys.Send("{TAB}");
        }

        public static void ChangeLanguage(string Culture)
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(
                new System.Globalization.CultureInfo(Culture));
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
        public static string SqlTransactionMethodExecuteNonQuery(string ConStr, string command, params object[] args)
        {
            string result = "";
            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();

                SqlTransaction sqlTran = Con.BeginTransaction();
                SqlCommand cmd = Con.CreateCommand();
                cmd.Transaction = sqlTran;
                try
                {
                    cmd.CommandText = command;
                    for (int i = 0; i < args.Length; )
                    {
                        if (args[i + 1] == null || args[i + 1].ToString() == "")
                            args[i + 1] = DBNull.Value;
                        cmd.Parameters.Add(args[i].ToString(), args[i + 1]);
                        i += 2;
                    }

                    result = cmd.ExecuteNonQuery().ToString();
                    sqlTran.Commit();

                }
                catch (Exception es)
                {
                    sqlTran.Rollback();
                    throw es;
                }

            }
            return result;
        }
        public static void CheckExceptionType(Exception ex, string FormName)
        {
            System.Data.NoNullAllowedException Notnull = new System.Data.NoNullAllowedException();
            System.Data.ConstraintException Unique = new System.Data.ConstraintException();
            System.OverflowException Overflow = new OverflowException();
            System.Data.InvalidConstraintException Relation = new System.Data.InvalidConstraintException();
            InvalidOperationException invalid = new InvalidOperationException();

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
                    if (ex.Message.StartsWith("Item cannot be added to a read-only or fixed-size list") &&
                        FormName == "Form06_AccountingHeaders")
                    {
                        MessageBox.Show("سرفصل حساب در سطح بالاتر مشخص نگردیده است");
                        return;
                    }
                    else if (ex.Message.StartsWith(
                        "Item cannot be added to a read-only or fixed-size list") &&
                        FormName == "Form01_AccDocument")
                    {
                        MessageBox.Show("در ابتدا، سند جدید درج نمایید");
                        return;
                    }
                }


                else
                {
                    if (ex.Message.StartsWith(
                        "The DELETE statement conflicted with the REFERENCE constraint"))
                    {
                        MessageBox.Show("حذف اطلاعات امکانپذیر نمی باشد");
                        return;

                    }
                    else if (ex.Message.StartsWith("Cannot insert duplicate key row in object"))
                    {
                        if (FormName == "Frm_002_Faktor")
                        {
                            MessageBox.Show("شماره فاکتور فروش وارد شده تکراریست");
                            return;
                        }
                        else if (FormName == "Frm_013_ReturnFactor" || FormName == "Frm_014_ReturnFactor")
                        {
                            MessageBox.Show("شماره فاکتور تکراریست");
                            return;

                        }
                        else if (FormName == "Form01_RegisterOrders")
                        {
                            MessageBox.Show("شماره سفارش وارد شده تکراریست");
                            return;
                        }
                        else if (FormName == "Frm_003_FaktorKharid")
                        {
                            MessageBox.Show("شماره فاکتور خرید وارد شده تکراریست");
                            return;
                        }
                    }
                    else if (ex.Message.StartsWith("Concurrency violation: the UpdateCommand affected 0 of the expected 1 records"))
                    {
                        MessageBox.Show("اطلاعات را به روز رسانی کنید");
                        return;
                    }

                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        public static void CheckSqlExp(System.Data.SqlClient.SqlException ex, string FormName)
        {
            if (ex.Message.StartsWith("Violation of UNIQUE KEY constraint"))
            {
                //if (FormName == "Form06_AccountingHeaders")
                {
                    MessageBox.Show("کد وارد شده تکراری می باشد");
                    return;
                }
            }
            else if (ex.Message.StartsWith("The DELETE statement conflicted with"))
            {
                MessageBox.Show("حذف ردیف جاری اطلاعات امکانپذیر نمی باشد");
                return;
            }
            else if (ex.Message.Contains("Cannot insert duplicate key row in object "))
            {
                MessageBox.Show("کد وارد شده تکراری می باشد");
                return;
            }
            else
                throw new Exception(ex.Message);
        }

        public static string HeadCompName(string ACC_Code)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand(
                    "Select ACC_NameComplete from View001_Headers where ACC_Code='" + ACC_Code + "'", Con);
                return Select.ExecuteScalar().ToString();
            }
        }

        public static DataTable LogoTable()
        {
            SqlConnection Con = new SqlConnection(Properties.Settings.Default.MAIN);
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_000_OrgInfo where ColumnId=" +
                _OrgCode, Con);
            DataTable T000 = new DataTable();
            Adapter.Fill(T000);
            return T000;
        }

        public static DateTime ServerDate()
        {
            using (SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN))
            {
                ConMain.Open();
                SqlCommand Serverdate = new SqlCommand("Select getdate()", ConMain);
                DateTime DateTime = DateTime.Parse(Serverdate.ExecuteScalar().ToString());
                return DateTime;
            }

        }

        public static string ServerGetDate()
        {
            string DateTime;
            SqlCommand Serverdate = new SqlCommand("Select getdate()",
                new SqlConnection(Properties.Settings.Default.SALE));
            Serverdate.Connection.Open();
            DateTime = Serverdate.ExecuteScalar().ToString();
            Serverdate.Connection.Close();
            return DateTime;
        }

        /// <summary>
        /// Seprate Price in 3 digits
        /// </summary>
        /// <param name="Price"></param>
        /// <returns></returns>
        public static string ToRial(Int64 Price)
        {
            string Rial = Price.ToString();
            int i = Price.ToString().Length - 3;
            while (i > 0)
            {
                Rial = Rial.Insert(i, ",");
                i -= 3;
            }
            return Rial;
        }

        public static bool CalLinearDis(int CustomerId)
        {
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
            SqlDataAdapter Adapter = new SqlDataAdapter("Select cast( ISNULL((SELECT Column02 from Table_105_SystemTransactionInfo where Column00=65),0) as bit)", ConBase);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            return Convert.ToBoolean(Table.Rows[0][0].ToString());
        }

        public static void FilterMultiColumns(object sender, string NameColumn, string CodeColumn)
        {
            if (!string.IsNullOrEmpty(CodeColumn) && ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text.All(char.IsDigit))
            {
                Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
                    DropDownList.RootTable.Columns[CodeColumn], Janus.Windows.GridEX.ConditionOperator.BeginsWith,
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text);

                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
                    .DropDownList.ApplyFilter(filter);
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DropDownList.Update();
            }
            else
            {
                Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
                        DropDownList.RootTable.Columns[NameColumn], Janus.Windows.GridEX.ConditionOperator.Contains,
                        ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text);

                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
                    .DropDownList.ApplyFilter(filter);
            }

        }


        public static void FilterMultiColumns_Shop(object sender, string NameColumn, string CodeColumn)
        {
            //if (!string.IsNullOrEmpty(CodeColumn) && ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text.All(char.IsDigit))
            //{
                //Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
                //    DropDownList.RootTable.Columns[CodeColumn], Janus.Windows.GridEX.ConditionOperator.Contains,
                //    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text );
                //Janus.Windows.GridEX.GridEXFilterCondition filter1 = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
                //      DropDownList.RootTable.Columns[NameColumn], Janus.Windows.GridEX.ConditionOperator.Contains,
                //      ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text);


                //Janus.Windows.GridEX.GridEXFilterCondition filter2 = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
                //    DropDownList.RootTable.Columns[NameSecound], Janus.Windows.GridEX.ConditionOperator.Contains,
                //    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text);

                //((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
                //    .DropDownList.ApplyFilter(filter);

                //((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
                //  .DropDownList.ApplyFilter(filter1);
                //((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
                //      .DropDownList.ApplyFilter(filter2);
                //((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DropDownList.Update();
            //}
            //else
            //{
            //    Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
            //            DropDownList.RootTable.Columns[NameColumn], Janus.Windows.GridEX.ConditionOperator.Contains,
            //            ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text);

            //    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
            //        .DropDownList.ApplyFilter(filter);
            //}
            if (!string.IsNullOrEmpty(CodeColumn) && ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text.All(char.IsDigit))
            {
                Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
                    DropDownList.RootTable.Columns[CodeColumn], Janus.Windows.GridEX.ConditionOperator.BeginsWith,
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text);

                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
                    .DropDownList.ApplyFilter(filter);
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DropDownList.Update();
            }
            else
            {
                Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).
                        DropDownList.RootTable.Columns[NameColumn], Janus.Windows.GridEX.ConditionOperator.Contains,
                        ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).Text);

                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender)
                    .DropDownList.ApplyFilter(filter);
            }

        }
        
        
        public static void MultiColumnsRemoveFilter(object sender)
        {
            ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DropDownList.RemoveFilters();
        }

        public static void FilterGridExDropDown(object sender, string ColumnName, string NumericalSearch, string TextualText, string SearchText, FilterColumnType ColumnType)
        {
            try
            {
                if (ColumnType == FilterColumnType.GoodCode)
                {
                    Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                         ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[NumericalSearch],
                         ConditionOperator.BeginsWith, SearchText);
                    ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
                }
                else if (ColumnType == FilterColumnType.ACCColumn)
                {
                    if (SearchText.All(char.IsDigit))
                    {
                        Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                             ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[NumericalSearch],
                             ConditionOperator.BeginsWith, SearchText);
                        ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
                    }
                    else
                    {
                        Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                             ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[TextualText],
                             ConditionOperator.Contains, SearchText);
                        ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
                    }
                }
                else
                {
                    Janus.Windows.GridEX.GridEXFilterCondition filter = new GridEXFilterCondition(
                         ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RootTable.Columns[TextualText],
                         ConditionOperator.Contains, SearchText);
                    ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.ApplyFilter(filter);
                }

            }
            catch { }
        }


        public static void GridExDropDownRemoveFilter(object sender, string ColumnName)
        {
            ((Janus.Windows.GridEX.GridEX)sender).RootTable.Columns[ColumnName].DropDown.RemoveFilters();

        }


        public static string SqlTransactionMethodScaler(string ConStr, string command, params object[] args)
        {
            string result = "";
            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();

                SqlTransaction sqlTran = Con.BeginTransaction();
                SqlCommand cmd = Con.CreateCommand();
                cmd.Transaction = sqlTran;
                try
                {
                    cmd.CommandText = command;
                    for (int i = 0; i < args.Length; )
                    {
                        if (args[i + 1] == null || args[i + 1].ToString() == "")
                            args[i + 1] = DBNull.Value;
                        cmd.Parameters.Add(args[i].ToString(), args[i + 1]);
                        i += 2;
                    }
                    if (command.Contains("@outid"))
                    {
                        SqlParameter sqlp = new SqlParameter();
                        sqlp.Direction = ParameterDirection.Output;
                        sqlp.ParameterName = "@outid";
                        sqlp.SqlDbType = SqlDbType.Int;
                        sqlp.Value = "";
                        cmd.Parameters.Add(sqlp);
                        cmd.ExecuteNonQuery();
                        result = cmd.Parameters["@outid"].Value.ToString();
                    }
                    else
                        cmd.ExecuteNonQuery();
                    sqlTran.Commit();

                }
                catch (Exception es)
                {
                    sqlTran.Rollback();
                    throw es;
                }

            }
            return result;
        }
        public static DataTable SqlTransactionMethod(string ConStr, string command, params object[] args)
        {

            using (SqlConnection Con = new SqlConnection(ConStr))
            {
                Con.Open();

                SqlTransaction sqlTran = Con.BeginTransaction();
                SqlCommand cmd = Con.CreateCommand();

                cmd.Transaction = sqlTran;
                try
                {
                    cmd.CommandText = command;
                    for (int i = 0; i < args.Length; )
                    {
                        if (args[i + 1] == null || args[i + 1].ToString() == "")
                            args[i + 1] = DBNull.Value;
                        cmd.Parameters.Add(args[i].ToString(), args[i + 1]);
                        i += 2;
                    }
                    SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    Adapter.Fill(dt);
                    sqlTran.Commit();
                    return dt;

                }
                catch (Exception es)
                {
                    sqlTran.Rollback();
                    throw es;
                }

            }

        }





    }



}
