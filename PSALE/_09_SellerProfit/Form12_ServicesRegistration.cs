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
    public partial class Form12_ServicesRegistration : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);

        public Form12_ServicesRegistration()
        {
            InitializeComponent();
        }

        private void bt_New_Click(object sender, EventArgs e)
        {



            this.table_85_ServicesRegistrationTableAdapter.FillByID(this.dataSet1.Table_85_ServicesRegistration, 0);

            table_85_ServicesRegistrationBindingSource.AddNew();

            txt_num.Value =
                 clDoc.MaxNumber(Properties.Settings.Default.SALE, "Table_85_ServicesRegistration", "Column00").ToString();
            txt_date.Text =
                FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");

            column11TextBox.Text = Class_BasicOperation._UserName;
            column13TextBox.Text = Class_BasicOperation._UserName;
            column12DateTimePicker.Value = Class_BasicOperation.ServerDate();
            column14DateTimePicker.Value = Class_BasicOperation.ServerDate();
            mlt_services.Select();
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                save();
            }
            catch (SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, this.Name);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }

            this.Cursor = Cursors.Default;

        }

        private void save()
        {
            if (Convert.ToInt32(((DataRowView)this.table_85_ServicesRegistrationBindingSource.CurrencyManager.Current)["Column09"]) > 0)
            {
                Class_BasicOperation.ShowMsg("", "برای تراکنش جاری سند صادر شده است و امکان ذخیره اطلاعات وجود ندارد", "Stop");
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            column13TextBox.Text = Class_BasicOperation._UserName;
            column14DateTimePicker.Value = Class_BasicOperation.ServerDate();
            table_85_ServicesRegistrationBindingSource.EndEdit();
            this.table_85_ServicesRegistrationTableAdapter.Update(this.dataSet1.Table_85_ServicesRegistration);
            Class_BasicOperation.ShowMsg("", " ثبت با موفقیت انجام شد", "Information");
            this.Cursor = Cursors.Default;

        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (this.table_85_ServicesRegistrationBindingSource.Count > 0)
            {
                if (Convert.ToInt32(((DataRowView)this.table_85_ServicesRegistrationBindingSource.CurrencyManager.Current)["Column09"]) > 0)
                {
                    Class_BasicOperation.ShowMsg("", "برای تراکنش جاری سند صادر شده است و امکان حذف اطلاعات وجود ندارد", "Stop");
                    return;
                }
                try
                {
                    Class_UserScope UserScope = new Class_UserScope();
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 249))
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف خدمت جاری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {

                        this.table_85_ServicesRegistrationBindingSource.RemoveCurrent();
                        this.table_85_ServicesRegistrationBindingSource.EndEdit();
                        this.table_85_ServicesRegistrationTableAdapter.Update(this.dataSet1.Table_85_ServicesRegistration);
                        this.table_85_ServicesRegistrationTableAdapter.FillByID(this.dataSet1.Table_85_ServicesRegistration, 0);
                        txt_Search.Text = string.Empty;
                        Class_BasicOperation.ShowMsg("", " حذف با موفقیت انجام شد", "Information");

                    }

                }

                catch (SqlException es)
                {
                    Class_BasicOperation.CheckSqlExp(es, this.Name);
                    table_85_ServicesRegistrationTableAdapter.FillByID(this.dataSet1.Table_85_ServicesRegistration, Convert.ToInt32(((DataRowView)this.table_85_ServicesRegistrationBindingSource.CurrencyManager.Current)["ColumnId"]));
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    table_85_ServicesRegistrationTableAdapter.FillByID(this.dataSet1.Table_85_ServicesRegistration, Convert.ToInt32(((DataRowView)this.table_85_ServicesRegistrationBindingSource.CurrencyManager.Current)["ColumnId"]));

                }
            }
        }


        private void Form12_ServicesRegistration_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            if (e.KeyCode == Keys.D && e.Control)
                bt_Del_Click(sender, e);
            if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);

        }

        private void Form12_ServicesRegistration_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txt_num_KeyPress(object sender, KeyPressEventArgs e)
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

        private void Form12_ServicesRegistration_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.Table_75_SaleType' table. You can move, or remove it, as needed.
            this.table_75_SaleTypeTableAdapter.Fill(this.dataSet1.Table_75_SaleType);
            // TODO: This line of code loads data into the 'dataSet1.Table_84_Services' table. You can move, or remove it, as needed.
            try
            {
                dataSet1.EnforceConstraints = false;
                this.table_84_ServicesTableAdapter.Fill(this.dataSet1.Table_84_Services);
                dataSet1.EnforceConstraints = true;

            }
            catch
            {
            }
            SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
            DataTable Header = new DataTable();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from AllHeaders()", ConAcnt);
            Adapter.Fill(Header);
            mlt_sarfasl.DataSource = Header;


            DataTable dst = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_84_Services ");
            mlt_services.DataSource = dst;


            mlt_saleman.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)");
            mlt_customer.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_045_PersonInfo where Column12=1");
            mlt_peymankar.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_045_PersonInfo where Column12=1");
            mlt_Sanad.DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select [ColumnId],[Column00] from Table_060_SanadHead");

        }

        private void mlt_services_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column01", "Column00");

        }

        private void mlt_services_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void mlt_saleman_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");

        }

        private void mlt_sarfasl_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "ACC_Name", "ACC_Code");

        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
                bt_Search_Click(sender, e);
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
            {
                try
                {
                    table_85_ServicesRegistrationBindingSource.EndEdit();
                    int RowID = ReturnIDNumber(int.Parse(txt_Search.Text));
                    this.table_85_ServicesRegistrationTableAdapter.FillByID(this.dataSet1.Table_85_ServicesRegistration, RowID);
                    txt_Search.SelectAll();

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
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
                SqlCommand Commnad = new SqlCommand("Select ISNULL(ColumnId,0) from Table_85_ServicesRegistration where Column00=" + FactorNum, con);
                try
                {
                    ID = int.Parse(Commnad.ExecuteScalar().ToString());
                    return ID;
                }
                catch
                {
                    throw new Exception("شماره خدمت وارد شده نامعتبر است");
                }
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {

                table_85_ServicesRegistrationBindingSource.EndEdit();


                if (dataSet1.Table_85_ServicesRegistration.GetChanges() != null || dataSet1.Table_85_ServicesRegistration.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        bt_Save_Click(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select max(Column00) from Table_85_ServicesRegistration),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_85_ServicesRegistration where Column00=" + Table.Rows[0]["Row"].ToString());
                    this.table_85_ServicesRegistrationTableAdapter.FillByID(this.dataSet1.Table_85_ServicesRegistration, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));

                }

            }
            catch
            {
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.table_85_ServicesRegistrationBindingSource.Count > 0)
            {

                try
                {

                    table_85_ServicesRegistrationBindingSource.EndEdit();

                    if (dataSet1.Table_85_ServicesRegistration.GetChanges() != null || dataSet1.Table_85_ServicesRegistration.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            bt_Save_Click(sender, e);
                        }
                    }

                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select Min(Column00) from Table_85_ServicesRegistration where Column00>" + ((DataRowView)this.table_85_ServicesRegistrationBindingSource.CurrencyManager.Current)["Column00"].ToString() + "),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_85_ServicesRegistration where Column00=" + Table.Rows[0]["Row"].ToString());
                        this.table_85_ServicesRegistrationTableAdapter.FillByID(this.dataSet1.Table_85_ServicesRegistration, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));



                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.table_85_ServicesRegistrationBindingSource.Count > 0)
            {
                try
                {

                    table_85_ServicesRegistrationBindingSource.EndEdit();

                    if (dataSet1.Table_85_ServicesRegistration.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            bt_Save_Click(sender, e);
                        }
                    }



                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString,
                        "Select ISNULL((Select max(Column00) from Table_85_ServicesRegistration where Column00<" +
                        ((DataRowView)this.table_85_ServicesRegistrationBindingSource.CurrencyManager.Current)["Column00"].ToString() + "),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_85_ServicesRegistration where Column00=" + Table.Rows[0]["Row"].ToString());
                        this.table_85_ServicesRegistrationTableAdapter.FillByID(this.dataSet1.Table_85_ServicesRegistration, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));


                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {

                table_85_ServicesRegistrationBindingSource.EndEdit();

                if (dataSet1.Table_85_ServicesRegistration.GetChanges() != null || dataSet1.Table_85_ServicesRegistration.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        bt_Save_Click(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select min(Column00) from Table_85_ServicesRegistration),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_85_ServicesRegistration where Column00=" + Table.Rows[0]["Row"].ToString());
                    this.table_85_ServicesRegistrationTableAdapter.FillByID(this.dataSet1.Table_85_ServicesRegistration, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));



                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void صدورسندToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_85_ServicesRegistrationBindingSource.Count > 0)
                {
                    Class_UserScope UserScope = new Class_UserScope();


                    if (Convert.ToInt32(((DataRowView)this.table_85_ServicesRegistrationBindingSource.CurrencyManager.Current)["Column09"]) > 0)
                    {
                        Class_BasicOperation.ShowMsg("", "تراکنش جاری سند دارد", "Stop");
                        return;
                    }
                    else
                    {
                        DataRowView dr = ((DataRowView)this.table_85_ServicesRegistrationBindingSource.CurrencyManager.Current);
                        if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 250))
                            throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");
                        save();


                        Form13_ServiceSanad frm = new Form13_ServiceSanad(dr);
                        frm.ShowDialog();
                        mlt_Sanad.DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select [ColumnId],[Column00] from Table_060_SanadHead");
                        this.table_85_ServicesRegistrationTableAdapter.FillByID(this.dataSet1.Table_85_ServicesRegistration, int.Parse(dr["ColumnId"].ToString()));

                    }
                }
            }
            catch (SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, this.Name);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            int SanadId = 0;
            if (this.table_85_ServicesRegistrationBindingSource.Count > 0)
                SanadId = (((DataRowView)this.table_85_ServicesRegistrationBindingSource.CurrencyManager.Current)["Column09"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_85_ServicesRegistrationBindingSource.CurrencyManager.Current)["Column09"].ToString()));

            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;

            Class_UserScope UserScope = new Class_UserScope();

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

        private void حذفسندToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();

            string command = string.Empty;
            DataTable Table = new DataTable();
            try
            {
                if (this.table_85_ServicesRegistrationBindingSource.Count > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 251))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    int RowID = int.Parse(((DataRowView)this.table_85_ServicesRegistrationBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int SanadID = clDoc.OperationalColumnValue("Table_85_ServicesRegistration", "Column09", RowID.ToString());
                   

                    if (SanadID != 0)
                    {
                        string Message = "آیا مایل به حذف سند مربوط به این خدمت هستید؟";

                        
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            this.Cursor = Cursors.WaitCursor;

                            clDoc.IsFinal_ID(SanadID);

                            Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=99 and Column17=" + RowID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=99 and Column17=" + RowID;




                            command += " UPDATE " + ConSale.Database + ".dbo.Table_85_ServicesRegistration SET Column09=0,Column13='" + Class_BasicOperation._UserName + "', Column14=getdate() where ColumnId=" + RowID;


                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                            {
                                Con.Open();

                                SqlTransaction sqlTran = Con.BeginTransaction();
                                SqlCommand Command = Con.CreateCommand();
                                Command.Transaction = sqlTran;
                                try
                                {
                                    Command.CommandText = command;
                                    Command.ExecuteNonQuery();
                                    sqlTran.Commit();
                                    Class_BasicOperation.ShowMsg("", "حذف سند حسابداری با موفقیت صورت گرفت", "Information");

                                }
                                catch (Exception es)
                                {
                                    sqlTran.Rollback();
                                    this.Cursor = Cursors.Default;
                                    Class_BasicOperation.CheckExceptionType(es, this.Name);
                                }
                            }
                        }
                    }
                    this.table_85_ServicesRegistrationTableAdapter.FillByID(this.dataSet1.Table_85_ServicesRegistration, RowID);


                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;

        }
    }
}
