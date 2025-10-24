using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._04_Buy
{
    public partial class Frm_002_BuyFaktorPrint : Form
    {
        bool _del, _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        bool Isadmin = false;
        Int16 projectId;
        public Frm_002_BuyFaktorPrint()
        {
            InitializeComponent();
        }

        private void mlt_Customer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {

                if ((!char.IsControl(e.KeyChar) && e.KeyChar != 13) || e.KeyChar == 8)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    uiButton1.Select();
            }
            else
                uiButton1.Select();
        }

        private void mlt_Customer_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "name", "code");

        }

        private void mlt_Customer_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;

            Class_BasicOperation.isEnter(e.KeyChar);

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void mlt_Customer_TextChanged(object sender, EventArgs e)
        {



        }

        private void Frm_002_StoreFaktorPrint_Load(object sender, EventArgs e)
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
            DataTable CustomerTable = clDoc.ReturnTable
//       (ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
//                                           dbo.Table_045_PersonInfo.Column01 AS code,
//                                           dbo.Table_045_PersonInfo.Column02 AS NAME,
//                                           dbo.Table_065_CityInfo.Column02 AS shahr,
//                                           dbo.Table_060_ProvinceInfo.Column01 AS ostan,
//                                           dbo.Table_045_PersonInfo.Column06 AS ADDRESS,
//                                           dbo.Table_045_PersonInfo.Column30,
//                                           Table_045_PersonInfo.Column07,
//                                           Table_045_PersonInfo.Column19 AS Mobile
//                                    FROM   dbo.Table_045_PersonInfo
//                                           LEFT JOIN dbo.Table_065_CityInfo
//                                                ON  dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
//                                           LEFT JOIN dbo.Table_060_ProvinceInfo
//                                                ON  dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00
//                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");


                 (ConBase.ConnectionString, @"SELECT columnid AS id,
                                           column01 AS code,
                                           column02 AS NAME,
                                           mobile AS Mobile
                                    FROM   ListPeople(5)");
            mlt_Customer.DataSource = CustomerTable;
            faDatePicker1.SelectedDateTime = DateTime.Now;
            faDatePicker2.SelectedDateTime = DateTime.Now;

            mlt_Customer.Select();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (mlt_Customer.Value != null && !string.IsNullOrWhiteSpace(mlt_Customer.Value.ToString()) 
                && faDatePicker1.SelectedDateTime != null
                && faDatePicker2.SelectedDateTime != null

                )
            {
                this.Cursor = Cursors.WaitCursor;
                Class_UserScope UserScope = new Class_UserScope();

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                {


                    using (SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.SALE))
                    {

                        ConMain.Open();
                        SqlCommand Command = new SqlCommand(@"
                                                           IF EXISTS(
                                                                   SELECT tsf.columnid
                                                                   FROM   Table_015_BuyFactor tsf
                                                                   WHERE  tsf.column03 = " + Convert.ToInt32(mlt_Customer.Value) + @"
                                                                          AND tsf.column02 >= '" + faDatePicker1.Text + @"'  AND tsf.column02 <= '" + faDatePicker2.Text + @"'
                                                                            and (tsf.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')
                                                            )
                                                            SELECT 1
                                                            ELSE
	                                                            SELECT 0", ConMain);
                        if (Convert.ToInt32(Command.ExecuteScalar()) == 0)
                        {
                            Class_BasicOperation.ShowMsg("", "اطلاعاتی برای چاپ وجود ندارد", "Warning");
                            this.Cursor = Cursors.Default;
                            return;

                        }

                    }

                    Reports.Form_BuyFactorPrint frm =
                       new Reports.Form_BuyFactorPrint(Convert.ToInt32(mlt_Customer.Value), faDatePicker1.Text, faDatePicker2.Text);
                    frm.ShowDialog();
                    mlt_Customer.Value = null;
                    mlt_Customer.Select();

                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
            }
            this.Cursor = Cursors.Default;

        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox =
                    (FarsiLibrary.Win.Controls.FADatePicker)sender;


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

        private void Frm_002_StoreFaktorPrint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
                uiButton1_Click(sender, e);
        }
    }
}
