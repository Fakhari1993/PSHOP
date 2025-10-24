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
    public partial class Frm_016_DraftInformation_ReturnFactor : Form
    {
        SqlConnection Conware = new SqlConnection(Properties.Settings.Default.WHRS);
        public string Function=null;
        public string Ware = null;
        public int havalenum =0;

        DataTable _Table = new DataTable();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);

        int _RowId = 0; string _Date=null;

        public Frm_016_DraftInformation_ReturnFactor(DataTable Table, int RowID,string Date)
        {
            InitializeComponent();
            _Table = Table;
            _RowId = RowID;
            _Date = Date;
        }

        private void Frm_010_DraftInformationDialog_Load(object sender, EventArgs e)
        {
            DataSet DS = new DataSet();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_001_PWHRS  where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + "')", Conware);
            Adapter.Fill(DS, "Ware");
            mlt_Ware.DataSource = DS.Tables["Ware"];

            Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=1", Conware);
            Adapter.Fill(DS, "Fun");
            mlt_Function.DataSource = DS.Tables["Fun"];

            chk_DraftNum.Checked = false;
            txt_DraftNum.Enabled = false;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Function.Text.Trim() == "" || mlt_Ware.Text.Trim() == "")
                    throw new Exception("اطلاعات مورد نیاز را تکمیل کنید");

                if (  (chk_DraftNum.Checked && Convert.ToInt32(txt_DraftNum.Value) <= 0))
                    throw new Exception("اطلاعات مورد نیاز جهت صدور حواله انبار را کامل کنید");

                if (  (chk_DraftNum.Checked))
                {
                    int ok = 0;
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        Con.Open();
                        SqlCommand Select = new SqlCommand(@"IF EXISTS(
                                                       SELECT *
                                                       FROM   Table_007_PwhrsDraft tpd
                                                       WHERE  tpd.column01 = " + txt_DraftNum.Value + @"
                                                   )
                                                    SELECT 0 AS ok 
                                                ELSE
                                                    SELECT 1 ok", Con);
                        ok = Convert.ToInt32(Select.ExecuteScalar().ToString());
                    }
                    if (ok == 0)
                        throw new Exception("این شماره حواله استفاده شده است");

                }

                foreach (DataRow item in _Table.Rows)
                {
                    if (!clGood.IsGoodInWare(short.Parse(mlt_Ware.Value.ToString()),
                        int.Parse(item["GoodID"].ToString())))
                        throw new Exception("کالای " + item["GoodName"].ToString() + " در انبار انتخاب شده فعال نمی باشد");
                }
                DataTable Child1 = clDoc.ReturnTable(ConSale.ConnectionString, "Select Column02,SUM(Column07) as Column07 from Table_022_Child1_MarjooiBuy where Column01=" + _RowId + " group by Column02");
                foreach (DataRow item in Child1.Rows)
                {
                    if (clDoc.IsGood(item["Column02"].ToString()))
                    {
                        float Remain = FirstRemain(int.Parse(item["Column02"].ToString()));
                        if (Remain < float.Parse(item["Column07"].ToString()))
                        {
                            throw new Exception("موجودی کالای " + clDoc.ExScalar(ConWare.ConnectionString, "table_004_CommodityAndIngredients", "Column02", "ColumnId", item["Column02"].ToString()) + " کمتر از تعداد مشخص شده در فاکتور است");
                        }
                    }

                }
                havalenum = Convert.ToInt32(txt_DraftNum.Value);
                Function = mlt_Function.Value.ToString();
                Ware = mlt_Ware.Value.ToString();
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }

               
        }
        private float FirstRemain(int GoodCode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                string CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE      (dbo.Table_011_PwhrsReceipt.column03 = {0}) AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE     (dbo.Table_007_PwhrsDraft.column03 = {0}) AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02) AS derivedtbl_1
                       WHERE     (Date <= '{2}')";
                CommandText = string.Format(CommandText, mlt_Ware.Value.ToString(), GoodCode,
                     _Date);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        private void mlt_Function_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chk_DraftNum_CheckedChanged(object sender, EventArgs e)
        {

            if (chk_DraftNum.Checked)
                txt_DraftNum.Enabled = true;
            else
                txt_DraftNum.Enabled = false;

        }
    }
}
