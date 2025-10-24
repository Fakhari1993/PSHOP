using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._07_Services
{
    public partial class Form04_ExportDocDialog : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        int _FactorId = 0 ;
        DataTable SourceTable = new DataTable();
        BindingSource Header = new BindingSource();
        BindingSource Child1Binding = new BindingSource();
        BindingSource Child2Binding = new BindingSource();
        DataTable Child1Tbl = new DataTable();
        DataTable Child2Tbl = new DataTable();
        DataRowView HeaderRow;
        SqlParameter DocNum;
        SqlParameter DocID;
        public Form04_ExportDocDialog(int FactorId)
        {
            InitializeComponent();
            _FactorId = FactorId;
        }

        private void Form04_ExportDocDialog_Load(object sender, EventArgs e)
        {
            faDatePicker1.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00");

            SourceTable.Columns.Add("Column01", Type.GetType("System.String"));
            SourceTable.Columns.Add("Column001", Type.GetType("System.String"));
            SourceTable.Columns.Add("Column07", Type.GetType("System.Int32"));
            SourceTable.Columns["Column07"].AllowDBNull = true;
            SourceTable.Columns.Add("Column08", Type.GetType("System.Int16"));
            SourceTable.Columns["Column08"].AllowDBNull = true;
            SourceTable.Columns.Add("Column09", Type.GetType("System.Int16"));
            SourceTable.Columns["Column09"].AllowDBNull = true;
            SourceTable.Columns.Add("Column10", Type.GetType("System.String"));
            SourceTable.Columns.Add("Column11", Type.GetType("System.Int64"));
            SourceTable.Columns.Add("Column12", Type.GetType("System.Int64"));
            gridEX1.DataSource = SourceTable;

            DataTable HeaderTb = ClDoc.ReturnTable(ConAcnt.ConnectionString, "Select * from AllHeaders()");
            gridEX1.DropDowns["Header_Code"].SetDataBinding(HeaderTb, "");
            gridEX1.DropDowns["Header_Name"].SetDataBinding(HeaderTb, "");

            gridEX1.DropDowns["Project"].SetDataBinding(ClDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_035_ProjectInfo"), "");
            gridEX1.DropDowns["Center"].SetDataBinding(ClDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo"), "");
            gridEX1.DropDowns["Person"].SetDataBinding(ClDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo"), "");


            //Fill Gridex
            Header.DataSource = ClDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_031_ServiceFactor where ColumnId=" + _FactorId);
            HeaderRow=(DataRowView)Header.CurrencyManager.Current;
            Child1Tbl = ClDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_032_ServiceFactor_Child1 where Column01=" + _FactorId);
            Child1Binding.DataSource = Child1Tbl;
            Child2Tbl = ClDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_033_ServiceFactor_Child2 where Column01=" + _FactorId);
            Child2Binding.DataSource =Child2Tbl;

            //سرفصل بدهکار و بستانکار از روی تراکنش مربوط خوانده می شود
            //درج سطر بدهکار
            string Bed = ClDoc.Account( 15, "Column07").Trim();
            SourceTable.Rows.Add(Bed, Bed,HeaderRow["Column03"].ToString() , null, null, "فاکتور خدمات- ش " + HeaderRow["Column01"].ToString()+" تاریخ "+
            HeaderRow["Column02"].ToString(), Int64.Parse(Child1Tbl.Compute("SUM(Column06)", "").ToString()), 0);

            //درج سطرهای بستانکار
            string Bes = ClDoc.Account( 15, "Column13").Trim(); 
            SourceTable.Rows.Add(Bes, Bes,null, null, null, "فاکتور خدمات- ش " + HeaderRow["Column01"].ToString() + " تاریخ " +
            HeaderRow["Column02"].ToString(),0, Int64.Parse(Child1Tbl.Compute("SUM(Column06)", "").ToString()));

            //سایر اضافات و کسورات
            foreach (DataRowView item in Child2Binding)
            {
                string ChildBed = ClDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column10", "ColumnId", item["Column02"].ToString());
                string ChildBes = ClDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column16", "ColumnId", item["Column02"].ToString());
                string Name = ClDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column01", "ColumnId", item["Column02"].ToString());

                //********Bed
                SourceTable.Rows.Add(ChildBed, ChildBed, (item["Column05"].ToString() == "False" ? HeaderRow["Column03"].ToString() : null), null, null, Name + " فاکتور خدمات- ش " + HeaderRow["Column01"].ToString() + " به تاریخ " + HeaderRow["Column02"].ToString(), long.Parse(item["Column04"].ToString()), 0);
                //*********Bes
                SourceTable.Rows.Add(ChildBes, ChildBes, (item["Column05"].ToString() == "True" ? HeaderRow["Column03"].ToString() : null), null, null, Name + " فاکتور خدمات- ش " + HeaderRow["Column01"].ToString() + " به تاریخ " + HeaderRow["Column02"].ToString(), 0, long.Parse(item["Column04"].ToString()));
            }
        }

        private void rdb_New_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_New.Checked)
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Text = "فاکتور خدمات";
                faDatePicker1.Text = ((DataRowView)Header.CurrencyManager.Current)["Column02"].ToString();
            }
            else
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
            }
        }

        private void rdb_last_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_last.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
                int LastNum = ClDoc.LastDocNum();
                txt_LastNum.Text = LastNum.ToString();
                faDatePicker1.Text = ClDoc.DocDate( LastNum);
                txt_Cover.Text = ClDoc.Cover( LastNum);

            }
            else
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                faDatePicker1.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00");
                txt_Cover.Text = null;
            }
        }

        private void txt_To_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txt_To.Text.Trim() != "")
                {
                    ClDoc.IsValidNumber(int.Parse(txt_To.Text.Trim()));
                    faDatePicker1.Text = ClDoc.DocDate( int.Parse(txt_To.Text.Trim()));
                    txt_Cover.Text = ClDoc.Cover( int.Parse(txt_To.Text.Trim()));
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void rdb_TO_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_TO.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;

            }
            else
            {
                txt_To.Text = null;
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
            }
        }

        private void txt_To_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void bt_ViewDocs_Click(object sender, EventArgs e)
        {
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 19))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form01_AccDocument")
                    {
                        item.BringToFront();
                        TextBox txt_S = (TextBox)item.ActiveControl;
                        txt_S.Text = (Convert.ToInt32( DocNum.Value) != 0 ? DocNum.Value.ToString() : "1");
                        PACNT._2_DocumentMenu.Form01_AccDocument frms = (PACNT._2_DocumentMenu.Form01_AccDocument)item;
                        frms.bt_Search_Click(sender, e);
                        return;
                    }
                }
                PACNT._2_DocumentMenu.Form01_AccDocument frm = new PACNT._2_DocumentMenu.Form01_AccDocument(
                  UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 20), int.Parse(DocID.Value.ToString()));
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
             //*********Just Accounting Document
           

                try
                {
                    CheckEssentialItems(sender, e);

                    string Message = "آیا مایل به صدور سند حسابداری هستید؟";

                    if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        //صدور سند
                        //if (rdb_New.Checked)
                        //{
                        //    DocNum = ClDoc.LastDocNum() + 1;
                        //    DocID = ClDoc.ExportDoc_Header( DocNum, faDatePicker1.Text, txt_Cover.Text, Class_BasicOperation._UserName);
                        //}
                        //else if (rdb_last.Checked)
                        //{
                        //    DocNum = ClDoc.LastDocNum();
                        //    DocID = ClDoc.DocID(DocNum);
                        //}
                        //else if (rdb_TO.Checked)
                        //{
                        //    DocNum = int.Parse(txt_To.Text.Trim());
                        //    DocID = ClDoc.DocID(DocNum);

                        //}

                        DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                        DocID = new SqlParameter("DocID", SqlDbType.Int);

                        DocNum.Direction = ParameterDirection.Output;
                        DocID.Direction = ParameterDirection.Output;
                        string headercomman = string.Empty;
                        if (rdb_last.Checked)
                        {
                            //DocNum = clDoc.LastDocNum();
                            //DocID = clDoc.DocID(DocNum);
                            headercomman = " set @DocNum=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0)))";

                        }
                        else if (rdb_TO.Checked)
                        {
                            //DocNum = int.Parse(txt_To.Text.Trim());
                            //DocID = clDoc.DocID(DocNum);
                            headercomman = " set @DocNum=" + int.Parse(txt_To.Text.Trim()) + "    SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + int.Parse(txt_To.Text.Trim()) + ")";

                        }
                        else if (rdb_New.Checked)
                        {
                            //DocNum = clDoc.LastDocNum() + 1;
                            //DocID = clDoc.ExportDoc_Header( DocNum, faDatePicker1.Text, txt_Cover.Text, Class_BasicOperation._UserName);
                            headercomman = @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + faDatePicker1.Text + "',2,0,'" + txt_Cover.Text + "','" + Class_BasicOperation._UserName +
                         "',getdate()); SET @DocID=SCOPE_IDENTITY()";
                        }
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

                            this.Cursor = Cursors.Default;



                        }
                        if (Convert.ToInt32(DocID.Value) > 0)
                        {

                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                            {
                                string[] _AccInfo = ClDoc.ACC_Info(item.Cells["Column01"].Value.ToString());

                                    ClDoc.ExportDoc_Detail(Convert.ToInt32( DocID.Value), item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                        , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                        (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(), (item.Cells["Column12"].Text == "0" ? long.Parse(item.Cells["Column11"].Value.ToString()) : 0),
                                        (item.Cells["Column11"].Text == "0" ? long.Parse(item.Cells["Column12"].Value.ToString()) : 0), 0, 0, -1,
                                           22, int.Parse(HeaderRow["ColumnId"].ToString()), Class_BasicOperation._UserName, 0, (short?)null);

                            }
                          
                            ClDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_031_ServiceFactor SET Column05=" + DocID.Value + " where ColumnId=" + HeaderRow["ColumnId"].ToString());
                                Class_BasicOperation.ShowMsg("", "سند حسابداری با شماره " + DocNum.Value + " با موفقیت ثبت گردید", "Information");

                            bt_ExportDoc.Enabled = false;
                            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                        }

                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            


        }

        private void CheckEssentialItems(object sender,EventArgs e)
        {
            if (rdb_last.Checked && txt_LastNum.Text.Trim() != "")
            {
                ClDoc.IsFinal(int.Parse(txt_LastNum.Text.Trim()));
            }
            else if (rdb_TO.Checked && txt_To.Text.Trim() != "")
            {
                ClDoc.IsValidNumber(int.Parse(txt_To.Text.Trim()));
                ClDoc.IsFinal(int.Parse(txt_To.Text.Trim()));
                txt_To_Leave(sender, e);
            }

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                if (item.Cells["Column01"].Text.Trim() == "" || item.Cells["Column10"].Text.Trim() == "")
                    throw new Exception("اطلاعات مورد نیاز جهت صدور سند را کامل کنید");
                if (item.Cells["Column01"].Text.Trim().All(char.IsDigit))
                {
                    throw new Exception("سرفصل" + item.Cells["Column01"].Text + "نامعتبر است");

                }
            }

            if (txt_Cover.Text.Trim() == "" || faDatePicker1.Text.Length!=10)
                throw new Exception("اطلاعات مورد نیاز جهت صدور سند را کامل کنید");
            if (Convert.ToDouble(gridEX1.GetTotalRow().Cells["Column11"].Value.ToString()) == 0)
                throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            ClDoc.CheckForValidationDate( faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            ClDoc.CheckExistFinalDoc();

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

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                Person = null;
                Center = null;
                Project = null;
                if (item.Cells["Column07"].Text.Trim() != "")
                    Person = int.Parse(item.Cells["Column07"].Value.ToString());

                if (item.Cells["Column08"].Text.Trim() != "")
                    Center = Int16.Parse(item.Cells["Column08"].Value.ToString());

                if (item.Cells["Column09"].Text.Trim() != "")
                    Project = Int16.Parse(item.Cells["Column09"].Value.ToString());

                clCredit.All_Controls_Row(item.Cells["Column01"].Value.ToString(), Person, Center, Project, item);

                //**********Check Person Credit************//
                if (item.Cells["Column07"].Text.Trim() != "")
                {
                    if (item.Cells["Column07"].Text.Trim() != "")
                        TPerson.Rows.Add(Int32.Parse(item.Cells["Column07"].Value.ToString()), item.Cells["Column01"].Value.ToString()
                            , (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) * -1 :
                            Convert.ToDouble(item.Cells["Column11"].Value.ToString())));
                }
                //**********Check Account's nature****//
                TAccounts.Rows.Add(item.Cells["Column01"].Value.ToString(), (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) * -1 :
                            Convert.ToDouble(item.Cells["Column11"].Value.ToString())));
            }

            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);


        }


        private void Form04_ExportDocDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_ExportDoc_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.W)
                bt_ViewDocs_Click(sender, e);
        }

    }
}
