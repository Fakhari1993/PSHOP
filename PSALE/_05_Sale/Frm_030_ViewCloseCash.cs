using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._05_Sale
{
    public partial class Frm_030_ViewCloseCash : Form
    {
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        bool Isadmin = false;
        Int16 projectId;
        public Frm_030_ViewCloseCash()
        {
            InitializeComponent();
        }

        private void Frm_030_ViewCloseCash_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
            gx_losefromcash.RemoveFilters();
            gx_recivefrombank.RemoveFilters();
            gx_recivefromcustomer.RemoveFilters();
        }

        private void Frm_030_ViewCloseCash_Load(object sender, EventArgs e)
        {

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


            foreach (Janus.Windows.GridEX.GridEXColumn col in gridEX1.RootTable.Columns)
            {
                col.EditType = Janus.Windows.GridEX.EditType.NoEdit;
                col.FilterEditType = Janus.Windows.GridEX.FilterEditType.TextBox;
            }


            DataTable sanad = clDoc.ReturnTable(ConAcnt.ConnectionString, "select * from Table_060_SanadHead");
            gridEX1.DropDowns[0].DataSource = sanad;

            DataTable Person = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_045_PersonInfo where Column12=1");
            this.gx_recivefromcustomer.DropDowns[0].SetDataBinding(Person, "");

            DataTable Person2 = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_045_PersonInfo where Column12=1");
            this.gx_losefromcash.DropDowns["Person"].SetDataBinding(Person2, "");


            DataTable ACCINfo = clDoc.ReturnTable(Properties.Settings.Default.BANK, "Select ColumnId,Column01,Column02,isnull(Column12,'') as Column12 from Table_020_BankCashAccInfo where Column01=0");
            this.gx_recivefrombank.DropDowns[0].SetDataBinding(ACCINfo, "");


            DataTable ACC = clDoc.ReturnTable(Properties.Settings.Default.ACNT, "SELECT ACC_Code,ACC_Name from AllHeaders()");
            this.gx_losefromcash.DropDowns["ACC"].SetDataBinding(ACC, "");
            gridEX1.DropDowns["Numsanad"].DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, @"select columnid,column00 from Table_060_SanadHead");

            if (Isadmin)
            {

                this.table_96_CloseCashTableAdapter.Fill(this.dataSet6.Table_96_CloseCash);
                this.table_97_ReceivedFromCustomersTableAdapter.Fill(this.dataSet6.Table_97_ReceivedFromCustomers);
                this.table_98_ReceivedFromBankTableAdapter.Fill(this.dataSet6.Table_98_ReceivedFromBank);
                this.table_99_LosesFromCashTableAdapter.Fill(this.dataSet6.Table_99_LosesFromCash);
            }
            else
            {

                this.table_96_CloseCashTableAdapter.FillByProject(this.dataSet6.Table_96_CloseCash, projectId);
                this.table_97_ReceivedFromCustomersTableAdapter.FillByProject(this.dataSet6.Table_97_ReceivedFromCustomers, projectId);
                this.table_98_ReceivedFromBankTableAdapter.FillByProject(this.dataSet6.Table_98_ReceivedFromBank, projectId);
                this.table_99_LosesFromCashTableAdapter.FillByProject(this.dataSet6.Table_99_LosesFromCash, projectId);
            }
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {


            gridEX1.DropDowns["Numsanad"].DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, @"select columnid,column00 from Table_060_SanadHead");
            dataSet6.EnforceConstraints = false;

            if (Isadmin)
            {
                this.table_96_CloseCashTableAdapter.Fill(this.dataSet6.Table_96_CloseCash);
                this.table_97_ReceivedFromCustomersTableAdapter.Fill(this.dataSet6.Table_97_ReceivedFromCustomers);
                this.table_98_ReceivedFromBankTableAdapter.Fill(this.dataSet6.Table_98_ReceivedFromBank);
                this.table_99_LosesFromCashTableAdapter.Fill(this.dataSet6.Table_99_LosesFromCash);
            }
            else
            {

                this.table_96_CloseCashTableAdapter.FillByProject(this.dataSet6.Table_96_CloseCash, projectId);
                this.table_97_ReceivedFromCustomersTableAdapter.FillByProject(this.dataSet6.Table_97_ReceivedFromCustomers, projectId);
                this.table_98_ReceivedFromBankTableAdapter.FillByProject(this.dataSet6.Table_98_ReceivedFromBank, projectId);
                this.table_99_LosesFromCashTableAdapter.FillByProject(this.dataSet6.Table_99_LosesFromCash, projectId);
            }
            dataSet6.EnforceConstraints = true;


        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                //int i = gridEX1.GetRow().Position;
                DataRowView Row = (DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current;

                //dataSet6.EnforceConstraints = false;
                //this.table_96_CloseCashTableAdapter.FillByID(this.dataSet6.Table_96_CloseCash, Convert.ToInt32(Row["ColumnId"]));
                //this.table_97_ReceivedFromCustomersTableAdapter.FillByHeaderID(this.dataSet6.Table_97_ReceivedFromCustomers, Convert.ToInt32(Row["ColumnId"]));
                //this.table_98_ReceivedFromBankTableAdapter.FillByHeaderID(this.dataSet6.Table_98_ReceivedFromBank, Convert.ToInt32(Row["ColumnId"]));
                //this.table_99_LosesFromCashTableAdapter.FillByHeaderID(this.dataSet6.Table_99_LosesFromCash, Convert.ToInt32(Row["ColumnId"]));
                //dataSet6.EnforceConstraints= true;

                DataTable CloseCash = clDoc.ReturnTable(ConSale.ConnectionString, "select * from Table_96_CloseCash where ColumnId=" + Row["ColumnId"]);

                DataTable ReceivedFromCustomers = clDoc.ReturnTable(ConSale.ConnectionString, @"select h.*,j.column02 as Person from Table_97_ReceivedFromCustomers h
                                                                                            join " + ConBase.Database + @".dbo.Table_045_PersonInfo j on h.Column02=j.columnid
                                                                                        where h.Column01=" + Row["ColumnId"]);
                DataTable ReceivedFromBank = clDoc.ReturnTable(ConSale.ConnectionString, @"select h.*,j.column02 as Bank from Table_98_ReceivedFromBank h
                                                join " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo j on j.columnid=h.column02 where h.Column01=" + Row["ColumnId"]);


                DataTable LosesFromCash = clDoc.ReturnTable(ConSale.ConnectionString, @"select h.*,j.column02 as Person,k.ACC_Name from Table_99_LosesFromCash h left join
                                                                                           " + ConBase.Database + @".dbo.Table_045_PersonInfo j on h.Column03=j.columnid 
                                                                                            join " + ConAcnt.Database + @".dbo.AllHeaders() k on h.Column02=k.ACC_Code    
                                                                                            where h.Column01=" + Row["ColumnId"]);

                string Numsanad = "";

                Numsanad = clDoc.ExScalar(ConSale.ConnectionString, @"select isnull(( SELECT    " + ConAcnt.Database + @".dbo.Table_060_SanadHead.Column00
FROM            dbo.Table_96_CloseCash INNER JOIN
                         " + ConAcnt.Database + @".dbo.Table_060_SanadHead ON dbo.Table_96_CloseCash.Column16 = " + ConAcnt.Database + @".dbo.Table_060_SanadHead.ColumnId
WHERE        (" + ConAcnt.Database + @".dbo.Table_060_SanadHead.ColumnId = " + gridEX1.GetValue("Column16") + ")),0)");

                _05_Sale.Frm_030_CloseCashPrint frm =
                      new Frm_030_CloseCashPrint("", CloseCash,
                          ReceivedFromCustomers,
                          ReceivedFromBank,
                          LosesFromCash, Numsanad);

                frm.ShowDialog();
                //bt_Search_Click(null, null);
                //gridEX1.MoveTo(i);
            }
            catch (Exception es)
            {

                this.Cursor = Cursors.Default;
                Class_BasicOperation.CheckExceptionType(es, this.Name);
            }
        }

        private void Frm_030_ViewCloseCash_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F && bt_Search.Enabled)
            {
                bt_Search_Click(sender, e);
            }

            else if (e.KeyCode == Keys.P && bt_Print.Enabled)
            {
                bt_Print_Click(sender, e);
            }
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {

                if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 338))
                    throw new Exception("کاربر گرامی شما امکان اصلاح صندوق را ندارید");

                if (this.gridEX1.RowCount > 0)
                {

                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Frm_029_CloseCash_Edit")
                        {
                            item.BringToFront();
                            _05_Sale.Frm_029_CloseCash_Edit frm = (_05_Sale.Frm_029_CloseCash_Edit)item;
                            frm.txt_Search.Text = gridEX1.GetRow().Cells["ColumnId"].Text;
                            frm.bt_Search_Click(sender, e);
                            return;
                        }
                    }
                    _05_Sale.Frm_029_CloseCash_Edit frms = new _05_Sale.Frm_029_CloseCash_Edit(Convert.ToInt32(gridEX1.GetValue("columnid")));
                    try
                    {
                        frms.MdiParent = MainForm.ActiveForm;
                    }
                    catch { }
                    frms.ShowDialog();

                    int i = gridEX1.CurrentRow.Position;
                    bt_Search_Click(null, null);
                    try
                    {
                        gridEX1.MoveTo(i);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

        private void gridEX1_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            btn_Edit_Click(sender, e);
        }

        private void gridEX1_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }
    }
}
