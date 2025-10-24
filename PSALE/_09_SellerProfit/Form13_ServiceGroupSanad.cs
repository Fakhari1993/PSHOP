using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._09_SellerProfit
{

    public partial class Form13_ServiceGroupSanad : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Janus.Windows.GridEX.GridEX gr;
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        public Form13_ServiceGroupSanad(Janus.Windows.GridEX.GridEX _gr)
        {
            InitializeComponent();
            gr = _gr;


            SqlDataAdapter ViewAdapter = new SqlDataAdapter("Select * from AllHeaders()", ConAcnt);

            DataTable Header2 = new DataTable();
            ViewAdapter.Fill(Header2);
            mlt_Bed.DataSource = Header2;


            

            DataTable Header6 = new DataTable();
            ViewAdapter.Fill(Header6);
            mlt_Bes.DataSource = Header6;





            

            rdb_New_CheckedChanged(null, null);



        }
        private void rdb_New_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_New.Checked)
            {
                faDatePicker1.Enabled = true;
                faDatePicker1.SelectedDateTime = DateTime.Now;
                txt_LastNum.Text = null;
                txt_To.Text = null;

                txt_sharh.Enabled = true;
                txt_sharh.Text = "صدور سند ثبت خدمات";
            }
            else
            {
                faDatePicker1.Enabled = false;
            }
        }

        private void rdb_last_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_last.Checked)
            {
                faDatePicker1.Enabled = false;
                int LastNum = clDoc.LastDocNum();
                txt_LastNum.Text = LastNum.ToString();
                faDatePicker1.Text = clDoc.DocDate(LastNum);
                txt_sharh.Text = string.Empty;
                txt_sharh.Enabled = false;

            }
            else
            {
                faDatePicker1.Enabled = true;
                faDatePicker1.SelectedDateTime = DateTime.Now;
            }
        }

        private void rdb_To_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_To.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_LastNum.Text = null;
                txt_To.Text = null;
                txt_sharh.Text = string.Empty;
                txt_sharh.Enabled = false;

            }
            else
            {
                txt_To.Text = null;
                faDatePicker1.Enabled = true;
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
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
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
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker1.HideDropDown();
                    txt_sharh.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void btn_sanad_Click(object sender, EventArgs e)
        {



            try
            {
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

                CheckEssentialItems();


                if (DialogResult.Yes == MessageBox.Show("آیا مایل به صدور سند حسابداری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {

                   
                    
                        this.Cursor = Cursors.WaitCursor;

                        SqlParameter DocNum;
                        DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                        DocNum.Direction = ParameterDirection.Output;
                        string CommandTxt = "declare @Key int ";

                        //صدور سند
                        if (rdb_New.Checked)
                        {
                            CommandTxt += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                                             VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + faDatePicker1.Text + "',2,0,'" + txt_sharh.Text + "','" + Class_BasicOperation._UserName + "',getdate()); SET @Key=SCOPE_IDENTITY()";
                                 
                        }
                        else if (rdb_last.Checked)
                        {
                            CommandTxt += " set @DocNum=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))  SET @Key=(Select ColumnId from Table_060_SanadHead where Column00=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0)))";
                        }
                        else if (rdb_To.Checked)
                        {

                            CommandTxt += " set @DocNum=" + int.Parse(txt_To.Text.Trim()) + "    SET @Key=(Select ColumnId from Table_060_SanadHead where Column00=" + int.Parse(txt_To.Text.Trim()) + ")";
                        }

                        foreach (Janus.Windows.GridEX.GridEXRow item in gr.GetCheckedRows())
                        {
                            string[] _BedInfo = clDoc.ACC_Info(mlt_Bed.Value.ToString());
                            string[] _BesInfo = clDoc.ACC_Info(mlt_Bes.Value.ToString());
                            string[] _RevenueInfo = clDoc.ACC_Info(item.Cells["Column08"].Value.ToString());

                            if (Convert.ToDouble(item.Cells["customeramount"].Value) > 0)
                                CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                           ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                                VALUES(@Key,'" + mlt_Bed.Value.ToString() + @"',
                                " + Int16.Parse(_BedInfo[0].ToString()) + ",'" + _BedInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_BedInfo[2].ToString()) ? "NULL" : "'" + _BedInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_BedInfo[3].ToString()) ? "NULL" : "'" + _BedInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_BedInfo[4].ToString()) ? "NULL" : "'" + _BedInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["customerid"].Value.ToString().Trim()) ? "NULL" : item.Cells["customerid"].Value.ToString()) + @",
                                NULL,NULL,"
                                        + "N' سند بدهکار خدمت شماره " + item.Cells["num"].Value + "در تاریخ " + item.Cells["date"].Value + "'," + item.Cells["customeramount"].Value + @",0,0,0,-1,99," + int.Parse(item.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";


                            if (Convert.ToDouble(item.Cells["peymankaramount"].Value) > 0)

                                CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                           ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                                VALUES(@Key,'" + mlt_Bes.Value.ToString() + @"',
                                " + Int16.Parse(_BesInfo[0].ToString()) + ",'" + _BesInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_BesInfo[2].ToString()) ? "NULL" : "'" + _BesInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_BesInfo[3].ToString()) ? "NULL" : "'" + _BesInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_BesInfo[4].ToString()) ? "NULL" : "'" + _BesInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["peymankarid"].Value.ToString().Trim()) ? "NULL" : item.Cells["peymankarid"].Value.ToString()) + @",
                                NULL,NULL,"
                                        + "N' سند بستانکار خدمت شماره " + item.Cells["num"].Value + "در تاریخ " + item.Cells["date"].Value + "',0," + item.Cells["peymankaramount"].Value + @",0,0,-1,99," + int.Parse(item.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";


                            if (Convert.ToDouble(item.Cells["customeramount"].Value) - Convert.ToDouble(item.Cells["peymankaramount"].Value) > 0)//سود 
                            {
                                CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                           ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                                VALUES(@Key,'" + item.Cells["Column08"].Value.ToString() + @"',
                                " + Int16.Parse(_RevenueInfo[0].ToString()) + ",'" + _RevenueInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_RevenueInfo[2].ToString()) ? "NULL" : "'" + _RevenueInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_RevenueInfo[3].ToString()) ? "NULL" : "'" + _RevenueInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_RevenueInfo[4].ToString()) ? "NULL" : "'" + _RevenueInfo[4].ToString() + "'") + @",
                               NULL,NULL,NULL,"
                                  + "N' سود حاصل از خدمت شماره " + item.Cells["num"].Value + "در تاریخ " + item.Cells["date"].Value + "',0," + (Math.Abs(Convert.ToDouble(item.Cells["customeramount"].Value) - Convert.ToDouble(item.Cells["peymankaramount"].Value))) + @",0,0,-1,99," + int.Parse(item.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";

                            }
                            else if (Convert.ToDouble(item.Cells["customeramount"].Value) - Convert.ToDouble(item.Cells["peymankaramount"].Value) < 0)//ضرر
                            {
                                CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                           ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                                VALUES(@Key,'" + item.Cells["Column08"].Value.ToString() + @"',
                                " + Int16.Parse(_RevenueInfo[0].ToString()) + ",'" + _RevenueInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_RevenueInfo[2].ToString()) ? "NULL" : "'" + _RevenueInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_RevenueInfo[3].ToString()) ? "NULL" : "'" + _RevenueInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_RevenueInfo[4].ToString()) ? "NULL" : "'" + _RevenueInfo[4].ToString() + "'") + @",
                               NULL,NULL,NULL,"
                                  + "N'  ضرر حاصل از خدمت شماره" + item.Cells["num"].Value + "در تاریخ " + item.Cells["date"].Value + "'," + (Math.Abs(Convert.ToDouble(item.Cells["customeramount"].Value) - Convert.ToDouble(item.Cells["peymankaramount"].Value))) + @",0,0,0,-1,99," + int.Parse(item.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";

                            }
                            CommandTxt += @"Update " + ConSale.Database + ".dbo.Table_85_ServicesRegistration  set Column09=@Key,Column13='" + Class_BasicOperation._UserName + "', Column14=getdate() where ColumnId= " + item.Cells["ColumnId"].Value;

                        }

                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Con.Open();

                            SqlTransaction sqlTran = Con.BeginTransaction();
                            SqlCommand Command = Con.CreateCommand();
                            Command.Transaction = sqlTran;

                            try
                            {
                                Command.CommandText = CommandTxt;
                                Command.Parameters.Add(DocNum);
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();
                                Class_BasicOperation.ShowMsg("", "سند حسابداری با شماره " + DocNum.Value + " با موفقیت ثبت گردید", "Information");
                                this.DialogResult = DialogResult.Yes;
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
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
            this.Cursor = Cursors.Default;

        }



        private void mlt_BuyBed_KeyPress(object sender, KeyPressEventArgs e)
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
                    faDatePicker1.HideDropDown();
                }
            }
            else if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void mlt_BuyBed_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                Class_BasicOperation.FilterMultiColumns(sender, "ACC_Name", "ACC_Code");
            }
            catch
            {
            }
        }

        private void mlt_BuyBed_Leave(object sender, EventArgs e)
        {
            try
            {
                Class_BasicOperation.MultiColumnsRemoveFilter(sender);
            }
            catch
            {
            }
        }

        private void mlt_SaleBed_Leave(object sender, EventArgs e)
        {
            try
            {
                Class_BasicOperation.MultiColumnsRemoveFilter(sender);
            }
            catch
            {
            }
        }

        private void mlt_BuyBes_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private bool CheckNotExported(long Id)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                Con.Open();
                SqlCommand Com = new SqlCommand(@"SELECT  ISNULL(Column09,0)   FROM   dbo.Table_85_ServicesRegistration where ColumnId=" + Id, Con);
                if (int.Parse(Com.ExecuteScalar().ToString()) == 0)
                    return true;
                else return false;

            }
        }

        private void CheckEssentialItems()
        {
            if (

                 
                mlt_Bes.Text.Trim() == "" ||
                mlt_Bed.Text.Trim() == "" ||
                !faDatePicker1.SelectedDateTime.HasValue)
                throw new Exception("اطلاعات مورد نیاز را کامل کنید");

            if (rdb_New.Checked && string.IsNullOrWhiteSpace(txt_sharh.Text))
            {
                throw new Exception("اطلاعات مورد نیاز را کامل کنید");
            }

            clDoc.CheckForValidationDate(faDatePicker1.Text);

            int ok = 0;


            //**********Check Person Credit************//

            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));


            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));

                foreach (Janus.Windows.GridEX.GridEXRow item in gr.GetCheckedRows())
                {
                   if(  !CheckNotExported(Convert.ToInt64(item.Cells["ColumnId"].Value)))
                       throw new Exception("خدمت شماره " + item.Cells["num"].Value + "دارای سند است");

                   if (item.Cells["Column08"].Value == null || string.IsNullOrWhiteSpace(item.Cells["Column08"].Value.ToString()))
                   {
                       throw new Exception("سرفصل درآمد خدمت شماره " + item.Cells["num"].Value + " مشخص نشده است");

                   }

                   using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                   {
                       Con.Open();
                       SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + item.Cells["Column08"].Value + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                       ok = int.Parse(Comm.ExecuteScalar().ToString());
                   }

                   if (ok == 0)
                       throw new Exception("سرفصل درآمد خدمت شماره " + item.Cells["num"].Value + "معتبر نیست");


                   TPerson.Rows.Add(Int32.Parse(item.Cells["customerid"].Value.ToString()), mlt_Bed.Value.ToString(), Convert.ToDouble(item.Cells["customeramount"].Value.ToString()));
                   TPerson.Rows.Add(Int32.Parse(item.Cells["peymankarid"].Value.ToString()), mlt_Bes.Value.ToString(), Convert.ToDouble(item.Cells["peymankaramount"].Value.ToString()));

                   TAccounts.Rows.Add(mlt_Bed.Value.ToString(), Convert.ToDouble(item.Cells["customeramount"].Value.ToString()));
                   TAccounts.Rows.Add(mlt_Bes.Value.ToString(), Convert.ToDouble(item.Cells["peymankaramount"].Value.ToString()));
                   TAccounts.Rows.Add(item.Cells["Column08"].Value.ToString(), Math.Abs(Convert.ToDouble(item.Cells["customeramount"].Value.ToString()) - Convert.ToDouble(item.Cells["peymankaramount"].Value.ToString())));



                }

            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + mlt_Bed.Value + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                ok = int.Parse(Comm.ExecuteScalar().ToString());
            }

            if (ok == 0)
                throw new Exception("شماره حساب معتبر را در تنظیمات وارد کنید");


            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + mlt_Bes.Value + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                ok = int.Parse(Comm.ExecuteScalar().ToString());
            }

            if (ok == 0)
                throw new Exception("شماره حساب معتبر را در تنظیمات وارد کنید");


            clDoc.CheckForValidationDate(faDatePicker1.Text);
            clCredit.CheckPersonCredit(TPerson, 0);
            clCredit.CheckAccountCredit(TAccounts, 0);

        }



        private void txt_sharh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
            else
                Class_BasicOperation.isEnter(e.KeyChar);
        }


    }
}
