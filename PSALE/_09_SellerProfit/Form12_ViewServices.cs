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


    public partial class Form12_ViewServices : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        bool _BackSpace = false;

        public Form12_ViewServices()
        {
            InitializeComponent();
        }

        private void Form12_ViewServices_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Goods.RemoveFilters();
        }

        private void Form12_ViewServices_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.Select();
        }

        private void btn_Desplay_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime != null
                && faDatePickerStrip2.FADatePicker.SelectedDateTime != null)
            {
                DataTable dt = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT tsr.ColumnId,
                                                                               tsr.Column00 AS num,
                                                                               tsr.Column01 AS [date],
                                                                               ts.Column01 AS [service],
                                                                               tpi.Column02 AS saleman,
                                                                               tpi2.Column02 AS customer,
                                                                               tsr.Column04 as customerid,
                                                                               tsr.Column05 AS customeramount,
                                                                               tpi3.Column02 AS peymankar,
                                                                               tsr.Column06 as peymankarid,
                                                                               tsr.Column07 AS peymankaramount,
                                                                               ISNULL(tsh.Column00, 0) AS sanadnum,
                                                                               isnull(tsr.Column09,0) as sanadid,
                                                                               tsr.Column10 AS [desc],
                                                                               tsr.Column11,
                                                                               tsr.Column12,
                                                                               tsr.Column13,
                                                                               tsr.Column14,tsr.Column08
                                                                        FROM   Table_85_ServicesRegistration tsr
                                                                               JOIN Table_84_Services ts
                                                                                    ON  ts.ColumnId = tsr.Column02
                                                                               JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi
                                                                                    ON  tpi.ColumnId = tsr.Column03
                                                                               JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi2
                                                                                    ON  tpi2.ColumnId = tsr.Column04
                                                                               JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi3
                                                                                    ON  tpi3.ColumnId = tsr.Column06
                                                                               LEFT JOIN " + ConAcnt.Database + @".dbo.Table_060_SanadHead tsh
                                                                                    ON  tsh.ColumnId = tsr.Column09
                                                                        WHERE  tsr.Column01 >= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                               AND tsr.Column01 <= '" + faDatePickerStrip2.FADatePicker.Text + @"'");
                gridEX_Goods.DataSource = dt;
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
                    faDatePickerStrip2.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePickerStrip2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip2.FADatePicker.HideDropDown();
                    btn_Desplay.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
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

        private void faDatePickerStrip2_TextChanged(object sender, EventArgs e)
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

        private void bt_Sanad_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 250))
                throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");
            if (gridEX_Goods.GetCheckedRows().Count() == 0)
            {

                MessageBox.Show("برای صدور سند خدمت انتخاب کنید");
                return;
            }
            else
            {
                Form13_ServiceGroupSanad frm = new Form13_ServiceGroupSanad(gridEX_Goods);
                frm.ShowDialog();
                btn_Desplay_Click(null, null);
            }
        }

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            int SanadId = 0;
            if (gridEX_Goods.GetRow() != null)
                SanadId = (gridEX_Goods.GetValue("sanadid").ToString() == "" ? 0 : int.Parse(gridEX_Goods.GetValue("sanadid").ToString()));

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

        private void bt_DelSanad_Click(object sender, EventArgs e)
        {
          
            if (gridEX_Goods.GetCheckedRows().Count() == 0)
            {

                MessageBox.Show("برای حذف سند خدمت انتخاب کنید");
                return;
            }
            else
            {
                Class_UserScope UserScope = new Class_UserScope();

                string command = string.Empty;
                DataTable Table = new DataTable();
                try
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 251))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");
                    string Message = "آیا مایل به حذف اسناد خدمات انتخاب شده هستید؟";


                    if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetCheckedRows())
                        {
                            int RowID = int.Parse(item.Cells["ColumnId"].Value.ToString());
                            int SanadID = clDoc.OperationalColumnValue("Table_85_ServicesRegistration", "Column09", RowID.ToString());


                            if (SanadID != 0)
                            {

                                this.Cursor = Cursors.WaitCursor;

                                clDoc.IsFinal_ID(SanadID);

                                Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=99 and Column17=" + RowID);
                                foreach (DataRow item1 in Table.Rows)
                                {
                                    command += " Delete from Table_075_SanadDetailNotes where Column00=" + item1["ColumnId"].ToString();
                                }

                                command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=99 and Column17=" + RowID;




                                command += " UPDATE " + ConSale.Database + ".dbo.Table_85_ServicesRegistration SET Column09=0,Column13='" + Class_BasicOperation._UserName + "', Column14=getdate() where ColumnId=" + RowID;

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
                        btn_Desplay_Click(null, null);


                    }



                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                this.Cursor = Cursors.Default;
            }
        }

        private void btn_Sanad_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();

            if (gridEX_Goods.GetCheckedRows().Count() == 0)
            {

                MessageBox.Show("برای حذف،  خدمت انتخاب کنید");
                return;
            }
            else
            {
                if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 249))
                    throw new Exception("کاربر گرامی شما امکان حذف خدمت را ندارید");
                string hassanad = string.Empty;
                string id = string.Empty;
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetCheckedRows())
                {
                    id += item.Cells["ColumnId"].Value + ",";
                    if (Convert.ToInt32(item.Cells["sanadid"].Value) > 0)
                        hassanad += item.Cells["num"].Value + ",";
                }
                if (!string.IsNullOrWhiteSpace(hassanad))
                {

                    MessageBox.Show("خدمات زیر دارای سند هستند" + Environment.NewLine + hassanad.TrimEnd(','));
                    return;
                }
                else
                {
                    string command = " Delete from " + ConSale.Database + ".dbo.Table_85_ServicesRegistration where ColumnId in (" + id.TrimEnd(',') + ")  and (Column09 is null or Column09=0) ";
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
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
                            Class_BasicOperation.ShowMsg("", "حذف با موفقیت صورت گرفت", "Information");

                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }
                    }
                    btn_Desplay_Click(null, null);

                }
            }
        }

        private void cmb_Print_Click(object sender, EventArgs e)
        {
            try
            {
                gridEXPrintDocument1.GridEX = gridEX_Goods;
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {

                        string j = " از تاریخ: " + faDatePickerStrip1.FADatePicker.Text + " تا تاریخ: " + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageHeaderLeft = j;
                        gridEXPrintDocument1.PageHeaderRight = "لیست خدمات ثبت شده";
                        printPreviewDialog1.ShowDialog();
                        gridEXPrintDocument1.PageFooterLeft =
                      FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd") +
                      "**" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                        gridEXPrintDocument1.PageFooterRight =
                          " کاربر " + Class_BasicOperation._UserName;
                    }
            }
            catch { }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }
    }
}
