using DevComponents.DotNetBar;
using Janus.Windows.GridEX;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace PSHOP._05_Sale
{
    public partial class Frm_029_CloseCash : Form
    {
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Class_UserScope UserScope = new Class_UserScope();

        string ReturnId = "";
        DataTable CheckTable = new DataTable();

        DataTable dtreturn = new DataTable();
        DataTable dtreturn1 = new DataTable();
        DataTable waredt = new DataTable();
        DataTable discountdt = new DataTable();
        DataTable taxdt = new DataTable();
        DataTable factordt = new DataTable();
        DataTable Sanaddt = new DataTable();
        DataTable TableNoDocNoDraft = new DataTable();
        DataTable bahaDT = new DataTable();
        DataTable dtbahaReturn = new DataTable();
        DataTable allfactor = new DataTable();
        DataTable dtRemain = new DataTable();
        DataTable dtResult = new DataTable();
        string bahas = string.Empty;
        string sanadcmd = string.Empty;
        string CmdArzesh = string.Empty;
        string CmdClose = string.Empty;

        double valueHavale = 0;
        double valueRecipt = 0;
        DataTable dtCash = new DataTable();
        DataTable idsdt = new DataTable();
        DataTable BedBesReturn = new DataTable();
        bool _BackSpace = false;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_CheckAccess ChA = new Classes.Class_CheckAccess();
        string Projectreturn = "";
        int Number = 0;
        string saleIds = "";
        short sanadtype;
        string projectId = "";
        bool isadmin=false;
        public Frm_029_CloseCash()
        {
            InitializeComponent();
        }

        private void Frm_029_CloseCash_Load(object sender, EventArgs e)
        {


            this.WindowState = FormWindowState.Maximized;


            DataTable Person = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_045_PersonInfo where Column12=1");
            this.gx_recivefromcustomer.DropDowns[0].SetDataBinding(Person, "");

            DataTable Person2 = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_045_PersonInfo where Column12=1");
            this.gx_losefromcash.DropDowns["Person"].SetDataBinding(Person2, "");

            
            DataTable ACCINfo = clDoc.ReturnTable(Properties.Settings.Default.BANK, "Select ColumnId,Column01,Column02,isnull(Column12,'') as Column12 from Table_020_BankCashAccInfo where Column01=0");
            this.gx_recivefrombank.DropDowns[0].SetDataBinding(ACCINfo, "");


            DataTable ACC = clDoc.ReturnTable(Properties.Settings.Default.ACNT, "SELECT ACC_Code,ACC_Name from AllHeaders()");
            this.gx_losefromcash.DropDowns["ACC"].SetDataBinding(ACC, "");


            DataTable CustomerTable = clDoc.ReturnTable
            (ConBase.ConnectionString, @"select [ColumnId]
                                                              ,[Column00]
                                                              ,[Column01]
                                                              ,[Column02]
                                    FROM   dbo.Table_045_PersonInfo
                                            
                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");
            gx_factors.DropDowns[3].SetDataBinding(CustomerTable, "");
            gx_factorReturn.DropDowns["DropDown1"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, @"select ColumnId,Column02 from Table_045_PersonInfo");

            DataTable saletype = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_002_SalesTypes");
            gx_factors.DropDowns["saletype"].SetDataBinding(saletype, "");


            DataTable sanad = clDoc.ReturnTable(ConAcnt.ConnectionString, "select * from Table_060_SanadHead");
            gx_factors.DropDowns["sanad"].SetDataBinding(sanad, "");

            DataTable StoreTable = new DataTable();
           

             isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._Year);

            if (isadmin)
            {
               StoreTable = clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT DISTINCT dbo.Table_295_StoreInfo.ColumnId, dbo.Table_295_StoreInfo.Column01, dbo.Table_295_StoreInfo.Column05
                            FROM            dbo.Table_295_StoreInfo  ");

                if (StoreTable.Rows.Count > 0)
                {
                    mlt_Stor.DataSource = StoreTable;

                    
                    mlt_Stor.ReadOnly = false;
                }
            }
            else
            {
               StoreTable = clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT DISTINCT dbo.Table_295_StoreInfo.ColumnId, 
                                                                            dbo.Table_295_StoreInfo.Column01, dbo.Table_296_StoreUsers.Column01 AS Users, dbo.Table_295_StoreInfo.Column05
                                                                FROM            dbo.Table_295_StoreInfo INNER JOIN
                                                                                         dbo.Table_296_StoreUsers ON dbo.Table_295_StoreInfo.ColumnId = dbo.Table_296_StoreUsers.Column00
                                                                WHERE        (dbo.Table_296_StoreUsers.Column01 = N'" + Class_BasicOperation._UserName + "')");
                if (StoreTable.Rows.Count>0)
                {
                    mlt_Stor.DataSource = StoreTable;
                    mlt_Stor.Value = StoreTable.Rows[0]["ColumnId"];
                    projectId = StoreTable.Rows[0]["Column05"].ToString();

                    mlt_Stor.ReadOnly = true;
                }
               
            }
            DataTable draft = clDoc.ReturnTable(ConWare.ConnectionString, "select * from Table_007_PwhrsDraft");
            gx_factors.DropDowns["draft"].SetDataBinding(draft, "");

            gx_losefromcash.DropDowns["Numsanad"].DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, @"select * from Table_060_SanadHead");
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;

            ToastNotification.ToastForeColor = Color.Black;
            ToastNotification.ToastBackColor = Color.SkyBlue;

        }

        private float FirstRemain(int GoodCode, string ware, string date, int? drafid)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                string CommandText = string.Empty;
                if (drafid == null)
                    CommandText = @"  SELECT Sum(InValue)-Sum(OutValue) as Remain
           FROM   (
                      SELECT ISNULL(
                                 (
                                     SELECT SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS 
                                            InValue
                                     FROM   dbo.Table_011_PwhrsReceipt
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt
                                                 ON  dbo.Table_011_PwhrsReceipt.columnid = 
                                                     dbo.Table_012_Child_PwhrsReceipt.column01
                                     WHERE  (dbo.Table_011_PwhrsReceipt.column03 = {0})
                                            AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                                            AND dbo.Table_011_PwhrsReceipt.column02 
                                                <= '{2}'
                                     GROUP BY
                                            dbo.Table_012_Child_PwhrsReceipt.column02
                                 ),
                                 0
                             ) AS InValue,
                             (
                                 SELECT ISNULL(
                                            (
                                                SELECT ISNULL(SUM(dbo.Table_008_Child_PwhrsDraft.column07), 0) AS 
                                                       OutValue
                                                FROM   dbo.Table_007_PwhrsDraft
                                                       INNER JOIN dbo.Table_008_Child_PwhrsDraft
                                                            ON  dbo.Table_007_PwhrsDraft.columnid = 
                                                                dbo.Table_008_Child_PwhrsDraft.column01
                                                WHERE  (dbo.Table_007_PwhrsDraft.column03 = {0})
                                                       AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                                                       AND dbo.Table_007_PwhrsDraft.column02 
                                                           <= '{2}'
                                                GROUP BY
                                                       dbo.Table_008_Child_PwhrsDraft.column02
                                            ),
                                            0
                                        )
                             ) AS OutValue
                  ) AS f
      ";
                else
                    CommandText = @"SELECT j.InValue -j.OutValue AS Remain
FROM   (
           SELECT *
           FROM   (
                      SELECT ISNULL(
                                 (
                                     SELECT SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS 
                                            InValue
                                     FROM   dbo.Table_011_PwhrsReceipt
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt
                                                 ON  dbo.Table_011_PwhrsReceipt.columnid = 
                                                     dbo.Table_012_Child_PwhrsReceipt.column01
                                     WHERE  (dbo.Table_011_PwhrsReceipt.column03 = {0})
                                            AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                                            AND dbo.Table_011_PwhrsReceipt.column02 
                                                <= '{2}'
                                     GROUP BY
                                            dbo.Table_012_Child_PwhrsReceipt.column02
                                 ),
                                 0
                             ) AS InValue,
                             (
                                 SELECT ISNULL(
                                            (
                                                SELECT ISNULL(SUM(dbo.Table_008_Child_PwhrsDraft.column07), 0) AS 
                                                       OutValue
                                                FROM   dbo.Table_007_PwhrsDraft
                                                       INNER JOIN dbo.Table_008_Child_PwhrsDraft
                                                            ON  dbo.Table_007_PwhrsDraft.columnid = 
                                                                dbo.Table_008_Child_PwhrsDraft.column01
                                                WHERE  (dbo.Table_007_PwhrsDraft.column03 = {0})
                                                       AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                                                       AND dbo.Table_007_PwhrsDraft.column02 
                                                           <= '{2}' and dbo.Table_007_PwhrsDraft.columnid!=" + drafid + @"
                                                GROUP BY
                                                       dbo.Table_008_Child_PwhrsDraft.column02
                                            ),
                                            0
                                        )
                             ) AS OutValue
                  ) AS f
       ) AS j";
                CommandText = string.Format(CommandText, ware, GoodCode, date);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        /*
        private float FirstRemain(int GoodCode, string ware, string date, int? drafid)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                string CommandText = string.Empty;
                if (drafid == null)
                    CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
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
                else
                    CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
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
                       WHERE     (dbo.Table_007_PwhrsDraft.column03 = {0}) AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1}) and dbo.Table_007_PwhrsDraft.columnid!=" + drafid + @"
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02) AS derivedtbl_1
                       WHERE     (Date <= '{2}')";
                CommandText = string.Format(CommandText, ware, GoodCode, date);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }
       */


        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable NotExist = new DataTable();

            try
            {
                //if (gx_factors.GetRows().Length > 0)
                //{
                #region update form data
                dtCash = clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT Column29 FROM Table_295_StoreInfo where Columnid = " + mlt_Stor.Value + "");
                if (dtCash.Rows[0]["Column29"].ToString() == "")
                {
                    Class_BasicOperation.ShowMsg("", "لطفا برای محاسبه فروش نقدی، از تنظیمات فروشگاه اقدام نمایید", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }

                gx_recivefrombank.UpdateData();
                gx_recivefromcustomer.UpdateData();
                gx_losefromcash.UpdateData();
                decimal customerrecive1 = Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum));
                decimal customerrecive = Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
                decimal losefromcash = Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["Column07"] = customerrecive1;
                ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["Column12"] = customerrecive;
                ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["Column11"] = losefromcash;

                if (dtCash.Rows[0][0].ToString() == "True")
                {
                    txt_CashSystem.Value = Convert.ToDecimal(txt_CashSale.Value) + customerrecive1 - losefromcash;
                    txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_RemainBank.Value);


                }
                else if (dtCash.Rows[0][0].ToString() == "False")
                {
                    txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + customerrecive1 - losefromcash;
                    txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value);

                }


                txt_RemainBank.Value = Convert.ToDecimal(txt_Cart.Value) + customerrecive;
                txt_CashAcountDifference.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashAcountRemain.Value)+ losefromcash;
                #endregion

                    #region چک کردن تکس های الارمی
                    if (txt_CashDeductionAddition.Text== "0" || txt_CashDeductionAddition.Text== "")
                    {
                        txt_CashDeductionAddition.BackColor = Color.White;
                    }
                    if (txt_CashAcountDifference.Text == "0" || txt_CashAcountDifference.Text == "")
                    {
                        txt_CashAcountDifference.BackColor = Color.White;
                    }

               

                if (txt_CashDeductionAddition.Text != "0")
                    {
                        
                         txt_CashDeductionAddition.BackColor = Color.LightPink;
                        //MessageBox.Show("مغایرت در کسر و اضافه صندوق ! ");
                        this.Cursor = Cursors.Default;
                        //return;

                    }
                 


             
                    #endregion
                    if (txt_CashAcountDifference.Text != "0")
                    {
                        txt_CashAcountDifference.BackColor = Color.LightPink;

                        if (DialogResult.No == MessageBox.Show("اختلاف حساب صندوق صفر نمی باشد آیا مایل به ادامه هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                        {

                            this.Cursor = Cursors.Default;
                            return;
                        }

                    }

                chehckessentioal();



                if (faDatePickerStrip1.FADatePicker.Text != string.Empty)
                {
                    bt_ExportDoc.Enabled = false;

                    #region چک کردن موجودی
                    if (saleIds!="")
                    {
                        
                    NotExist = clDoc.ReturnTable(ConSale.ConnectionString, string.Format(@"select 
Null as factornum, good.column01 goodcode,good.Columnid goodid, good.column02 as goodname,good.Column06 as goodbarcode , Remain As currentStock 
,needcount as needStock,Remain-needcount as  diff ,  date ,ware as wareid
,w.column01 as warecode,w.column02 as warename
 from (
select sch.Column02 as goodid,s.column42 as ware,sum(sch.Column07) needcount,s.column02 as date,
( SELECT InValue -OutValue AS Remain
           FROM   (
                      SELECT ISNULL(
                                 (
                                     SELECT SUM(rch.column07) AS 
                                            InValue
                                     FROM   PWHRS_{0}_{1}.dbo.Table_011_PwhrsReceipt r
                                            INNER JOIN PWHRS_{0}_{1}.dbo.Table_012_Child_PwhrsReceipt rch
                                                 ON  r.columnid = 
                                                     rch.column01
                                     WHERE  (r.column03 = s.column42)
                                            AND (rch.column02 = sch.column02)
                                            AND r.column02  <=s.column02
								    and rch.column01 not in (select Column09 from Table_018_MarjooiSale where  Column02>s.column02 and  column17 in ({2}))
                                     GROUP BY
                                            rch.column02
                                 ),
                                 0
                             ) AS InValue,
                             (
                                 SELECT ISNULL(
                                            (
                                                SELECT ISNULL(SUM(dch.column07), 0) AS 
                                                       OutValue
                                                FROM   PWHRS_{0}_{1}.dbo.Table_007_PwhrsDraft d
                                                       INNER JOIN  PWHRS_{0}_{1}.dbo.Table_008_Child_PwhrsDraft dch
                                                            ON  d.columnid = 
                                                                dch.column01
                                                WHERE  (d.column03 =s.column42)
                                                       AND (dch.column02 = sch.column02)
                                                       AND d.column02  <= s.column02 
										    and   dch.column01 not in (select column09 from Table_010_SaleFactor where Columnid in ({2}))
                                                GROUP BY  dch.column02
                                            ),
                                            0
                                        )
                             ) AS OutValue
                  ) AS f) as Remain
from Table_011_Child1_SaleFactor sch
left join Table_010_SaleFactor as s on s.columnid=sch.column01
where s.columnid in ({2})
group by sch.Column02,s.column42,s.column02

) as t
left join PWHRS_{0}_{1}.dbo.table_004_CommodityAndIngredients as good 
on good.columnid=goodid
left join PWHRS_{0}_{1}.dbo.Table_001_PWHRS as w on w.columnid=t.ware

where Remain<needcount ", Class_BasicOperation._OrgCode, Class_BasicOperation._Year, saleIds.TrimEnd(',')));


                    if (NotExist.Rows.Count > 0)
                    {
                        _05_Sale.GoodList frm1 = new GoodList(NotExist, "لیست کالاهای ناموجود برای حواله هایی که قرار است صادر شوند");
                        frm1.ShowDialog();
                        this.Cursor = Cursors.Default;
                        bt_ExportDoc.Enabled = bt_Search.Enabled = true;

                        return;


                    }
                    }


                    else
                        this.Cursor = Cursors.WaitCursor;
                    #endregion
                    #region صدور حواله و فاکتور فروش

                    if (TableNoDocNoDraft.Rows.Count>0)
                    {
                        
                    sanadcmd = @"
                                            declare @draftkey int 
                                            declare @DraftNum int 
                                            declare @DocNum int   
                                            declare @DocID int  
                                          
                                           ";
                    foreach (DataRow idro in TableNoDocNoDraft.Rows)
                    {

                       

                        DataTable Sanaddt = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT FactorTable.columnid,
                                               FactorTable.column01,
                                               FactorTable.date,
                                               ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
                                               ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
                                               FactorTable.Bed,
                                               FactorTable.Bes,
                                               FactorTable.NetTotal,FactorTable.person,FactorTable.saletype,FactorTable.Func,FactorTable.ware,FactorTable.Project
                                        FROM   (
                                                   SELECT dbo.Table_010_SaleFactor.columnid,
                                                          dbo.Table_010_SaleFactor.column01,
                                                          dbo.Table_010_SaleFactor.column02 AS Date,
                                                          dbo.Table_010_SaleFactor.column03 AS person,
                                                          dbo.Table_010_SaleFactor.column36 AS saletype,
                                                          dbo.Table_010_SaleFactor.column42 AS ware,
                                                          dbo.Table_010_SaleFactor.column43 AS Func,
                                                          OtherPrice.PlusPrice AS Ezafat,
                                                          OtherPrice.MinusPrice AS Kosoorat,
                                                          OtherPrice.Bed,
                                                          OtherPrice.Bes,
                                                          dbo.Table_010_SaleFactor.Column28 AS NetTotal,
                                                          dbo.Table_010_SaleFactor.Column44 AS Project
                                                   FROM   dbo.Table_010_SaleFactor
                                                         
                                                          LEFT OUTER JOIN (
                                                                   SELECT columnid,
                                                                          SUM(PlusPrice) AS PlusPrice,
                                                                          SUM(MinusPrice) AS MinusPrice,
                                                                          Bed,
                                                                          Bes
                                                                   FROM   (
                                                                              SELECT Table_010_SaleFactor_2.columnid,
                                                                                     SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                                                     PlusPrice,
                                                                                     0 AS MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = dbo.Table_012_Child2_SaleFactor.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_2
                                                                                          ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                                                              Table_010_SaleFactor_2.columnid
                                                                              WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_2.columnid,
                                                                                     dbo.Table_012_Child2_SaleFactor.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                              UNION ALL
                                                                              SELECT Table_010_SaleFactor_1.columnid,
                                                                                     0 AS PlusPrice,
                                                                                     SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                                                     MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                                                     Table_012_Child2_SaleFactor_1
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = 
                                                                                              Table_012_Child2_SaleFactor_1.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_1
                                                                                          ON  
                                                                                              Table_012_Child2_SaleFactor_1.column01 = 
                                                                                              Table_010_SaleFactor_1.columnid
                                                                              WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_1.columnid,
                                                                                     Table_012_Child2_SaleFactor_1.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                          ) AS OtherPrice_1
                                                                   GROUP BY
                                                                          columnid,
                                                                          OtherPrice_1.Bed,
                                                                          OtherPrice_1.Bes
                                                               ) AS OtherPrice
                                                               ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid where dbo.Table_010_SaleFactor.Column68 in (" + mlt_Stor.Value + ") and ('" + isadmin.ToString() + "'='True' or Column13='" + Class_BasicOperation._UserName + @"')
                                               ) AS FactorTable
                                        WHERE  FactorTable.columnid=" + int.Parse(idro["columnid"].ToString()) + @"");

                      
                        if (int.Parse(idro["Column09"].ToString()) == 0)
                        {
                            #region صدور حواله های نخورده
                            sanadcmd += " set @DraftNum=( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft)";
                            sanadcmd += @" INSERT INTO " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ([column01]
                                                                                               ,[column02]
                                                                                               ,[column03]
                                                                                               ,[column04]
                                                                                               ,[column05]
                                                                                               ,[column06]
                                                                                               ,[column07]
                                                                                               ,[column08]
                                                                                               ,[column09]
                                                                                               ,[column10]
                                                                                               ,[column11]
                                                                                               ,[column12]
                                                                                               ,[column13]
                                                                                               ,[column14]
                                                                                               ,[column15]
                                                                                               ,[column16]
                                                                                               ,[column17]
                                                                                               ,[column18]
                                                                                               ,[column19]
                                                                                               ,[Column20]
                                                                                               ,[Column21]
                                                                                               ,[Column22]
                                                                                               ,[Column23]
                                                                                               ,[Column24]
                                                                                               ,[Column25]
                                                                                               ,[Column26]) VALUES(@DraftNum,'" + Sanaddt.Rows[0]["date"] + "'," + Sanaddt.Rows[0]["ware"]
                                                                                                     + "," + (Sanaddt.Rows[0]["Func"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Func"].ToString()) ? Sanaddt.Rows[0]["Func"] : waredt.Rows[0]["Column02"]) + @", " + Sanaddt.Rows[0]["person"] + ",'" + "حواله صادره بابت فاکتور فروش ش" + Sanaddt.Rows[0]["column01"] +
                                                                                                 "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + Sanaddt.Rows[0]["columnid"] + ",0,0,0,0,0,0,0,null,0,0); SET @draftkey=SCOPE_IDENTITY()";

                            DataTable Child1 = clDoc.ReturnTable(ConSale.ConnectionString,
                                                             @"SELECT  [columnid] ,[column01] ,[column02] ,[column03] ,[column04] ,[column05] ,[column06] ,[column07] ,[column08] ,[column09]
                                                                      ,[column10]
                                                                      ,[column11]
                                                                      ,[column12]
                                                                      ,[column13]
                                                                      ,[column14]
                                                                      ,[column15]
                                                                      ,[column16]
                                                                      ,[column17]
                                                                      ,[column18]
                                                                      ,[column19]
                                                                      ,[column20]
                                                                      ,[column21]
                                                                      ,[column22]
                                                                      ,[column23]
                                                                      ,[column24]
                                                                      ,[column25]
                                                                      ,[column26]
                                                                      ,[column27]
                                                                      ,[column28]
                                                                      ,[column29]
                                                                      ,[column30]
                                                                      ,[Column31]
                                                                      ,[Column32]
                                                                      ,[Column33]
                                                                    ,Column34,Column35,Column36,Column37
                                                                  FROM  [dbo].[Table_011_Child1_SaleFactor] WHERE column01 =" + Sanaddt.Rows[0]["columnid"]);


                            foreach (DataRow item1 in Child1.Rows)
                            {
                                if (clDoc.IsGood(item1["Column02"].ToString()))
                                {
                                    double value = Convert.ToDouble(item1["Column07"]);
                                    string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                                        "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                        item1["Column02"].ToString());


                                    if (item1["column03"].ToString() != orginalunit)
                                    {
                                        float h = clDoc.GetZarib(Convert.ToInt32(item1["Column02"]), Convert.ToInt16(item1["column03"]), Convert.ToInt16(orginalunit));
                                        value = value * h;
                                    }

                                    sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]) VALUES(@draftkey," + item1["Column02"].ToString() + "," +
                                       orginalunit + "," + item1["Column04"].ToString() + "," + item1["Column05"].ToString() + "," + value + "," +
                                        value + "," + item1["Column08"].ToString() + "," + item1["Column09"].ToString() + "," + item1["Column10"].ToString() + "," +
                                        item1["Column11"].ToString() + ",NULL,NULL," + (item1["Column22"].ToString().Trim() == "" ? "NULL" : item1["Column22"].ToString())
                                        + ",0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL," +
                                        (item1["Column14"].ToString().Trim() == "" ? "NULL" : item1["Column14"].ToString()) + "," +
                                        item1["Column15"].ToString() +
                                            ",0,0,0,0," + (item1["Column30"].ToString() == "True" ? "1" : "0") + "," +
                                            (item1["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column34"].ToString() + "'") + "," +
                                            (item1["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column35"].ToString() + "'")
                                            + "," + item1["Column31"].ToString()
                                            + "," + item1["Column32"].ToString() + "," + item1["Column36"].ToString() + "," + item1["Column37"].ToString() + @")";




                                }
                                sanadcmd += " Update Table_010_SaleFactor set Column09=@draftkey where ColumnId =" + item1["Column01"];
                            }
                            #endregion
                        }
                        //else
                        //sanadcmd += "SET @draftkey=" + idro["Column09"];
                        if (int.Parse(idro["Column10"].ToString()) == 0)
                        {
                            #region صدور سندفاکتور های سند نخورده
                            int LastDocnum = LastDocNum(Sanaddt.Rows[0]["date"].ToString());
                            if (LastDocnum > 0)
                                sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from " + ConAcnt.Database + @".dbo.Table_060_SanadHead where Column00=" + LastDocnum + ")";
                            else
                                sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   " + ConAcnt.Database + @".dbo.Table_060_SanadHead ), 0 )) + 1   INSERT INTO " + ConAcnt.Database + @".dbo.Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from " + ConAcnt.Database + @".dbo.Table_060_SanadHead),0))+1,'" + Sanaddt.Rows[0]["date"] + "',2,0,N'فاکتور فروش','" + Class_BasicOperation._UserName +
                           "',getdate()); SET @DocID=SCOPE_IDENTITY()";


                            DataTable factordt = clDoc.ReturnTable(ConBase.ConnectionString,
                                                      @"SELECT       isnull( column08,'') as Column07,isnull(column14,'') as Column13
                                                                        FROM            Table_002_SalesTypes
                                                                        WHERE        (columnid = " + Sanaddt.Rows[0]["saletype"] + ") ");


                            string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());

                            sanadcmd += @"    INSERT INTO  " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + Sanaddt.Rows[0]["person"] + @", NULL ,  " + Sanaddt.Rows[0]["Project"] + @" ,
                   " + "'فاکتور فروش " + Sanaddt.Rows[0]["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";


                            _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                            sanadcmd += @"    INSERT INTO  " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , "+Sanaddt.Rows[0]["Project"]+@" ,
                   " + "'فاکتور فروش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";

                            foreach (DataRow dr in Sanaddt.Rows)
                            {
                                if (dr["Kosoorat"] != null &&
                                    dr["Kosoorat"].ToString() != string.Empty &&
                                    Convert.ToDouble(dr["Kosoorat"]) > Convert.ToDouble(0))
                                {


                                    _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                                    sanadcmd += @"    INSERT INTO  " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'تخفیف فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                    _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                                    sanadcmd += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               " + int.Parse(Sanaddt.Rows[0]["person"].ToString()) + @", NULL , " + Sanaddt.Rows[0]["Project"] + @" ,
                   " + "'تخفیف فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                }

                                if (dr["Ezafat"] != null &&
                                  dr["Ezafat"].ToString() != string.Empty &&
                                  Convert.ToDouble(dr["Ezafat"]) > Convert.ToDouble(0))
                                {

                                    _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                                    sanadcmd += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + int.Parse(Sanaddt.Rows[0]["person"].ToString()) + @", NULL , " + Sanaddt.Rows[0]["Project"] + @" ,
                   " + "'ارزش افزوده فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                    _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                                    sanadcmd += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + Sanaddt.Rows[0]["Project"] + @" ,
                   " + "'ارزش افزوده فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                }


                            }

                          
                            #endregion
                        }
                        sanadcmd += " Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07=@DocID,column10='" + Class_BasicOperation._UserName + "',column11=getdate() where ColumnId=(select Column09 from Table_010_SaleFactor where Columnid=" + Sanaddt.Rows[0]["columnid"].ToString() + ")";
                        sanadcmd += " Update Table_010_SaleFactor set Column10=@DocID where ColumnId =" + Sanaddt.Rows[0]["columnid"].ToString();
                    }


                    }
                    if (sanadcmd != "")
                        Class_BasicOperation.SqlTransactionMethodExecuteNonQuery(ConSale.ConnectionString, sanadcmd);
                    this.Cursor = Cursors.Default;

                    #endregion

                    #region صدور رسید فاکتور مرجوعی
                    string CmdReturn = "";
                    if (idsdt.Rows.Count > 0)
                    {
                        CmdReturn = @" declare @Residid int 
                                            declare @ResidNum int            
                                            declare @DocIDReturn int 
                                            declare @DocNumReturn int";
                        foreach (DataRow item in idsdt.Rows)
                        {
                            DataTable sanadreturn = clDoc.ReturnTable(ConSale.ConnectionString, @"select * from (   SELECT        dbo.Table_018_MarjooiSale.columnid AS idhedear,dbo.Table_018_MarjooiSale.column17 AS SaleId, dbo.Table_018_MarjooiSale.column01 AS number, dbo.Table_018_MarjooiSale.column02 AS date, dbo.Table_018_MarjooiSale.column03 AS Person, 
                         dbo.Table_018_MarjooiSale.Column18 AS NetTotal, dbo.Table_018_MarjooiSale.Column28 AS Whrs, dbo.Table_018_MarjooiSale.Column29 AS func, dbo.Table_020_Child2_MarjooiSale.columnid AS idchild, 
                         SUM(dbo.Table_020_Child2_MarjooiSale.column04) AS Ezafat,0  Kosorat, dbo.Table_024_Discount.column10 AS Bed, dbo.Table_024_Discount.column16 AS Bes,dbo.Table_018_MarjooiSale.Column30 AS Project
FROM            dbo.Table_024_Discount INNER JOIN
                         dbo.Table_020_Child2_MarjooiSale ON dbo.Table_024_Discount.columnid = dbo.Table_020_Child2_MarjooiSale.column02 Right OUTER JOIN
                         dbo.Table_018_MarjooiSale ON dbo.Table_020_Child2_MarjooiSale.column01 = dbo.Table_018_MarjooiSale.columnid
					where  dbo.Table_020_Child2_MarjooiSale.column05=0 or dbo.Table_020_Child2_MarjooiSale.column05 is null  and Table_018_MarjooiSale.Column27<>1 

GROUP BY dbo.Table_018_MarjooiSale.columnid, dbo.Table_018_MarjooiSale.column01,dbo.Table_018_MarjooiSale.column17, dbo.Table_018_MarjooiSale.column02, dbo.Table_018_MarjooiSale.column03, dbo.Table_018_MarjooiSale.Column18, 
                         dbo.Table_018_MarjooiSale.Column28, dbo.Table_018_MarjooiSale.Column29, dbo.Table_020_Child2_MarjooiSale.columnid, dbo.Table_024_Discount.column10, dbo.Table_024_Discount.column16,dbo.Table_018_MarjooiSale.Column30

					union all 

					SELECT        dbo.Table_018_MarjooiSale.columnid AS idhedear,dbo.Table_018_MarjooiSale.column17 AS SaleId, dbo.Table_018_MarjooiSale.column01 AS number, dbo.Table_018_MarjooiSale.column02 AS date, dbo.Table_018_MarjooiSale.column03 AS Person, 
                         dbo.Table_018_MarjooiSale.Column18 AS NetTotal, dbo.Table_018_MarjooiSale.Column28 AS Whrs, dbo.Table_018_MarjooiSale.Column29 AS func, dbo.Table_020_Child2_MarjooiSale.columnid AS idchild, 
                        0 AS Ezafat, SUM(dbo.Table_020_Child2_MarjooiSale.column04) as  Kosorat, dbo.Table_024_Discount.column10 AS Bed, dbo.Table_024_Discount.column16 AS Bes,dbo.Table_018_MarjooiSale.Column30 AS Project
FROM            dbo.Table_024_Discount INNER JOIN
                         dbo.Table_020_Child2_MarjooiSale ON dbo.Table_024_Discount.columnid = dbo.Table_020_Child2_MarjooiSale.column02 Right OUTER JOIN
                         dbo.Table_018_MarjooiSale ON dbo.Table_020_Child2_MarjooiSale.column01 = dbo.Table_018_MarjooiSale.columnid
					where  dbo.Table_020_Child2_MarjooiSale.column05=1 or dbo.Table_020_Child2_MarjooiSale.column05 is null  and Table_018_MarjooiSale.Column27<>1 

GROUP BY dbo.Table_018_MarjooiSale.columnid, dbo.Table_018_MarjooiSale.column01, dbo.Table_018_MarjooiSale.column02, dbo.Table_018_MarjooiSale.column03,dbo.Table_018_MarjooiSale.column17, dbo.Table_018_MarjooiSale.Column18, 
                         dbo.Table_018_MarjooiSale.Column28, dbo.Table_018_MarjooiSale.Column29, dbo.Table_020_Child2_MarjooiSale.columnid, dbo.Table_024_Discount.column10, dbo.Table_024_Discount.column16,dbo.Table_018_MarjooiSale.Column30) as dt  
                                                where dt.idhedear=" + item["Columnid"] + "");
                            if (sanadreturn.Rows.Count > 0)
                            {
                                
                                if (int.Parse(item["Column09"].ToString()) == 0)
                                {
                                    #region صدور رسید نخورده شده
                                    CmdReturn += (@"    set @ResidNum=(SELECT ISNULL((SELECT MAX(Column01)  FROM   " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ), 0 )) + 1

                                                            INSERT INTO " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt (
                                                                            [column01],
                                                                            [column02],
                                                                            [column03],
                                                                            [column04],
                                                                            [column05],
                                                                            [column06],
                                                                            [column07],
                                                                            [column08],
                                                                            [column09],
                                                                            [column10],
                                                                            [column11],
                                                                            [column12],
                                                                            [column13],
                                                                            [column14],
                                                                            [Column15],
                                                                            [Column16],
                                                                            [Column17],
                                                                            [Column18],
                                                                            [Column19],
                                                                            [Column20]
                                                                          )  VALUES (  @ResidNum ,'" + sanadreturn.Rows[0]["date"] + "'," +
     sanadreturn.Rows[0]["Whrs"] + "," + sanadreturn.Rows[0]["func"] + "," + sanadreturn.Rows[0]["Person"] + ",'" + "رسید صادرشده از فاکتور مرجوعی شماره " +
       sanadreturn.Rows[0]["number"] + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,0," + sanadreturn.Rows[0]["idhedear"] + ",0,0,0,0,1,null); SET @Residid=Scope_Identity()");


                                    DataTable ChildRecipt = clDoc.ReturnTable(ConWare.ConnectionString, @" select * from " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale where Column01=" + item["Columnid"] + "");

                                    foreach (DataRow Rows in ChildRecipt.Rows)
                                    {


                                        CmdReturn += (@"INSERT INTO " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35],[Column36],[Column37]) VALUES (@Residid ," + Rows["Column02"].ToString() + "," +
             Rows["Column03"].ToString() + "," + Rows["Column04"].ToString() + "," + Rows["Column05"].ToString() + "," + Rows["Column06"].ToString() + "," + Rows["Column07"].ToString() + "," + Rows["Column08"].ToString() + " ," + Rows["Column09"].ToString() + "," + Rows["Column10"].ToString() + "," + Rows["Column11"].ToString() + ",'" + Rows["Column23"].ToString() + "'," +
             (Rows["Column13"].ToString().Trim() == "" ? "NULL" : Rows["Column13"].ToString()) + "," + (Rows["Column14"].ToString().Trim() == "" ? "NULL" : Rows["Column14"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
             + "',getdate(),0,0,0,0,0,0,0,0,0,0,0," +
             (Rows["Column32"].ToString().Trim() == "" ? "NULL" : "'" + Rows["Column32"].ToString() + "'") + "," +
             (Rows["Column33"].ToString().Trim() == "" ? "NULL" : "'" + Rows["Column33"].ToString() + "'") + ",0,0" +
               "," + Rows["Column34"].ToString() + "," +
             Rows["Column35"].ToString() + "," + (Rows["Column36"].ToString().Trim() == "" ? "NULL" : "" + Rows["Column36"].ToString()) + "," + (Rows["Column37"].ToString().Trim() == "" ? "NULL" : "" + Rows["Column37"].ToString()) + ")");


                                    }



                                    #endregion

                                }
                                if (int.Parse(item["Column10"].ToString()) == 0)
                                {
                                    #region صدور سند مرجوعی نخورده شده
                                    int LastDocnumReturn = LastDocNum(sanadreturn.Rows[0]["date"].ToString());
                                    if (LastDocnumReturn > 0)
                                        CmdReturn += " set @DocNumReturn=" + LastDocnumReturn + "  SET @DocIDReturn=(Select ColumnId from " + ConAcnt.Database + @".dbo.Table_060_SanadHead where Column00=" + LastDocnumReturn + ")";
                                    else
                                        CmdReturn += @" set @DocNumReturn=(SELECT ISNULL((SELECT MAX(Column00)  FROM   " + ConAcnt.Database + @".dbo.Table_060_SanadHead ), 0 )) + 1   
                                INSERT INTO " + ConAcnt.Database + @".dbo.Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                                VALUES((Select Isnull((Select Max(Column00) from " + ConAcnt.Database + @".dbo.Table_060_SanadHead),0))+1,'" + sanadreturn.Rows[0]["date"] + "',2,0,N'فاکتور مرجوعی','" + Class_BasicOperation._UserName +
                                   "',getdate()); SET @DocIDReturn=SCOPE_IDENTITY()";

                                    if (BedBesReturn.Rows.Count > 0)
                                    {


                                        if (int.Parse(sanadreturn.Rows[0]["SaleId"].ToString()) == 0)//دستی
                                        {

                                            #region سند فاکتور مرجوعی های دستی
                                            string[] _AccInfo = clDoc.ACC_Info(BedBesReturn.Rows[0]["Bed"].ToString());

                                            CmdReturn += @"    INSERT INTO  " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDReturn,'" + BedBesReturn.Rows[0]["Bed"] + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + sanadreturn.Rows[0]["person"] + @", NULL , "+ sanadreturn.Rows[0]["Project"] + @" ,
                   " + "'فاکتور مرجوعی " + sanadreturn.Rows[0]["number"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(sanadreturn.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,17," + int.Parse(sanadreturn.Rows[0]["idhedear"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";


                                            _AccInfo = clDoc.ACC_Info(BedBesReturn.Rows[0]["Bes"].ToString());

                                            CmdReturn += @"    INSERT INTO  " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDReturn,'" + BedBesReturn.Rows[0]["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL ,  " + sanadreturn.Rows[0]["Project"] + @" ,
                   " + "'فاکتور مرجوعی " + int.Parse(sanadreturn.Rows[0]["number"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(sanadreturn.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,17," + int.Parse(sanadreturn.Rows[0]["idhedear"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";



                                            foreach (DataRow drreturn in sanadreturn.Rows)
                                            {
                                                if (drreturn["Ezafat"] != null &&
                                                    drreturn["Kosorat"].ToString() != string.Empty &&
                                                    Convert.ToDouble(drreturn["Kosorat"]) > Convert.ToDouble(0))
                                                {


                                                    _AccInfo = clDoc.ACC_Info(drreturn["Bed"].ToString());

                                                    CmdReturn += @"    INSERT INTO  " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDReturn,'" + drreturn["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL ,  " + sanadreturn.Rows[0]["Project"] + @" ,
                   " + "'تخفیف فاکتور مرجوعی ش " + int.Parse(sanadreturn.Rows[0]["number"].ToString()) + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(drreturn["Kosorat"].ToString()), 3)) + @",0,0,0,-1,17," + int.Parse(sanadreturn.Rows[0]["idhedear"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                    _AccInfo = clDoc.ACC_Info(drreturn["Bes"].ToString());

                                                    CmdReturn += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDReturn,'" + drreturn["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               " + int.Parse(sanadreturn.Rows[0]["person"].ToString()) + @", NULL ,  " + sanadreturn.Rows[0]["Project"] + @" ,
                   " + "'تخفیف فاکتور مرجوعی ش " + int.Parse(sanadreturn.Rows[0]["number"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(drreturn["Kosorat"].ToString()), 3)) + @",0,0,-1,17," + int.Parse(sanadreturn.Rows[0]["idhedear"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                }

                                                if (drreturn["Ezafat"] != null &&
                                                  drreturn["Ezafat"].ToString() != string.Empty &&
                                                  Convert.ToDouble(drreturn["Ezafat"]) > Convert.ToDouble(0))
                                                {

                                                    _AccInfo = clDoc.ACC_Info(drreturn["Bed"].ToString());

                                                    CmdReturn += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDReturn,'" + drreturn["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + int.Parse(sanadreturn.Rows[0]["person"].ToString()) + @", NULL ,  " + sanadreturn.Rows[0]["Project"] + @" ,
                   " + "'ارزش افزوده فاکتور مرجوعی ش " + int.Parse(sanadreturn.Rows[0]["number"].ToString()) + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(drreturn["Ezafat"].ToString()), 3)) + @",0,0,0,-1,17," + int.Parse(sanadreturn.Rows[0]["idhedear"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                    _AccInfo = clDoc.ACC_Info(drreturn["Bes"].ToString());

                                                    CmdReturn += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDReturn,'" + drreturn["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL ,  " + sanadreturn.Rows[0]["Project"] + @" ,
                   " + "'ارزش افزوده فاکتور مرجوعی ش " + int.Parse(sanadreturn.Rows[0]["number"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(drreturn["Ezafat"].ToString()), 3)) + @",0,0,-1,17," + int.Parse(sanadreturn.Rows[0]["idhedear"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                                }


                                            }
                                            #endregion

                                        }
                                        // else
                                        // {
                                        #region سند فاکتور مرجوعی های وابسته به فاکتور فروش
                                        //                                           DataTable sale = clDoc.ReturnTable(ConSale.ConnectionString, @"select * from Table_010_SaleFactor where ColumnId=" + item["SaleId"] + "");
                                        //                                           DataTable PreDoc = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select * from Table_065_SanadDetail where Column00=" +
                                        //             sale.Rows[0]["Column10"].ToString() + " and (Column16=15 and Column17=" + sale.Rows[0]["ColumnId"].ToString() +
                                        //                ") or (Column16=26 and Column17=" + sale.Rows[0]["Column09"].ToString() + @") 
                                        //       or (column16=24 and Column17=" + sale.Rows[0]["ColumnId"].ToString() + @") 
                                        //           or (  Column17 in (select Columnid from  " + ConBank.Database + ".dbo.Table_065_TurnReception  where Column01=" + (sale.Rows[0]["Column66"].ToString() == "" ? "0" : sale.Rows[0]["Column66"].ToString()) + ") and column27=28) ");


                                        //                                           foreach (DataRow items in PreDoc.Rows)
                                        //                                           {

                                        //                                               if (sale.Rows[0]["Column52"].ToString() != "" || sale.Rows[0]["Column52"].ToString() != "0")
                                        //                                               {

                                        //                                                   foreach (DataRow dt in PreDoc.Rows)
                                        //                                                   {
                                        //                                                       if (dt["Column27"].ToString() == "28")
                                        //                                                       {
                                        //                                                           Number++;
                                        //                                                       }

                                        //                                                   }

                                        //                                               }
                                        //                                               if (Number != 0)
                                        //                                               {
                                        //                                                   DataTable setting = clDoc.ReturnTable(ConBase.ConnectionString, @"Select * from Table_030_Setting");
                                        //                                                   string ACC_Code = clDoc.ExScalar(ConBank.ConnectionString, @"SELECT        " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.Column12 AS AccCode
                                        //                                    FROM           
                                        //                      " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo WHERE  " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.ColumnId = " + setting.Rows[79]["Column02"] + "");


                                        //                                                   string[] _ACCDoc = clDoc.ACC_Info(ACC_Code);

                                        //                                                   CmdReturn += @"

                                        //                   INSERT INTO  " + ConBank.Database + @".dbo.Table_065_TurnReception
                                        //                                                ([Column01]      ,[Column02]         ,[Column04]      ,[Column05]
                                        //                                                 ,[Column06]       ,[Column07]     ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                        //                                                   ,[Column12]      ,[Column13]     ,[Column16]   ,[Column18])  

                                        //Values (" + sale.Rows[0]["Column66"] + ", " + setting.Rows[81]["Column02"] + " ,'" + sale.Rows[0]["Column02"] + "'," + sale.Rows[0]["Column03"] + ",'" + setting.Rows[79]["Column02"] + "'," + ACC_Code + "," + Int16.Parse(_ACCDoc[0].ToString()) + ",'" + _ACCDoc[1].ToString() + "'," + (string.IsNullOrEmpty(_ACCDoc[2].ToString()) ? "NULL" : "'" + _ACCDoc[2].ToString() + "'") + "," +
                                        //                                    (string.IsNullOrEmpty(_ACCDoc[3].ToString()) ? "NULL" : "'" + _ACCDoc[3].ToString() + "'") + "," +
                                        //                                    (string.IsNullOrEmpty(_ACCDoc[4].ToString()) ? "NULL" : "'" + _ACCDoc[4].ToString() + "'") + ",@ReturnDocNum,'" + Class_BasicOperation._UserName + "',getdate()); SET @KeyReturn=SCOPE_IDENTITY()";


                                        //                                               }
                                        //                                               string[] _AccInfo = clDoc.ACC_Info(item["Column01"].ToString());



                                        //                                               if (items["column16"].ToString() == "15")
                                        //                                               {
                                        //                                                   sanadtype = 17;
                                        //                                               }
                                        //                                               else if (items["column16"].ToString() == "24")
                                        //                                               {
                                        //                                                   sanadtype = 17;

                                        //                                               }
                                        //                                               else if (items["column16"].ToString() == "26")
                                        //                                               {
                                        //                                                   sanadtype = 27;

                                        //                                               }
                                        //                                               else
                                        //                                               {
                                        //                                                   sanadtype = 29;

                                        //                                               }




                                        //                                               if (items["column16"].ToString() == "15")
                                        //                                               {
                                        //                                                   //RefId = ReturnId;


                                        //                                                   CmdReturn += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                        //             ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                        //             ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                        //             ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
                                        //               VALUES (@DocIDReturn,'" + items["Column01"].ToString() + @"',
                                        //                              " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                        //                               '" + _AccInfo[1].ToString() + @"',
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                        //                                " + (string.IsNullOrWhiteSpace(items["Column07"].ToString().Trim()) ? "NULL" : items["Column07"].ToString()) + @",
                                        //                               " + (string.IsNullOrWhiteSpace(items["Column08"].ToString().Trim()) ? "NULL" : items["Column08"].ToString()) + @",
                                        //                              " + (string.IsNullOrWhiteSpace(items["Column09"].ToString().Trim()) ? "NULL" : items["Column09"].ToString()) + @",
                                        //                               ' مرجوعی " + items["Column10"].ToString().Trim() + @"',
                                        //                               " + Convert.ToInt64(items["Column12"].ToString()) + @",
                                        //                               " + Convert.ToInt64(items["Column11"].ToString()) + @",
                                        //                               " + Convert.ToDouble(items["Column13"].ToString()) + @",
                                        //                               " + Convert.ToDouble(items["Column14"].ToString()) + @",
                                        //                               " + (items["Column15"].ToString().Trim() != "" ? Int16.Parse(items["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
                                        //                               " + sanadtype + @",@ReturnId,
                                        //                               '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                        //                               " + Convert.ToDouble(item["Column22"].ToString()) + @",
                                        //                               NULL)";


                                        //                                               }
                                        //                                               else if (items["column16"].ToString() == "24")
                                        //                                               {
                                        //                                                   //RefId = ReturnId;

                                        //                                                   CmdReturn += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                        //             ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                        //             ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                        //             ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
                                        //               VALUES (@DocIDReturn,'" + items["Column01"].ToString() + @"',
                                        //                              " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                        //                               '" + _AccInfo[1].ToString() + @"',
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                        //                                " + (string.IsNullOrWhiteSpace(items["Column07"].ToString().Trim()) ? "NULL" : items["Column07"].ToString()) + @",
                                        //                               " + (string.IsNullOrWhiteSpace(items["Column08"].ToString().Trim()) ? "NULL" : items["Column08"].ToString()) + @",
                                        //                              " + (string.IsNullOrWhiteSpace(items["Column09"].ToString().Trim()) ? "NULL" : items["Column09"].ToString()) + @",
                                        //                               ' مرجوعی " + items["Column10"].ToString().Trim() + @"',
                                        //                               " + Convert.ToInt64(items["Column12"].ToString()) + @",
                                        //                               " + Convert.ToInt64(items["Column11"].ToString()) + @",
                                        //                               " + Convert.ToDouble(items["Column13"].ToString()) + @",
                                        //                               " + Convert.ToDouble(items["Column14"].ToString()) + @",
                                        //                               " + (items["Column15"].ToString().Trim() != "" ? Int16.Parse(items["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
                                        //                               " + sanadtype + @", @ReturnId,
                                        //                               '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                        //                               " + Convert.ToDouble(items["Column22"].ToString()) + @",
                                        //                               NULL)";


                                        //                                               }
                                        //                                               else if (items["column16"].ToString() == "26")
                                        //                                               {
                                        //                                                   //RefId = ResidId;


                                        //                                                   CmdReturn += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                        //             ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                        //             ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                        //             ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
                                        //               VALUES (@DocIDReturn,'" + items["Column01"].ToString() + @"',
                                        //                              " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                        //                               '" + _AccInfo[1].ToString() + @"',
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                        //                                " + (string.IsNullOrWhiteSpace(items["Column07"].ToString().Trim()) ? "NULL" : items["Column07"].ToString()) + @",
                                        //                               " + (string.IsNullOrWhiteSpace(items["Column08"].ToString().Trim()) ? "NULL" : items["Column08"].ToString()) + @",
                                        //                              " + (string.IsNullOrWhiteSpace(items["Column09"].ToString().Trim()) ? "NULL" : items["Column09"].ToString()) + @",
                                        //                               ' مرجوعی " + items["Column10"].ToString().Trim() + @"',
                                        //                               " + Convert.ToInt64(items["Column12"].ToString()) + @",
                                        //                               " + Convert.ToInt64(items["Column11"].ToString()) + @",
                                        //                               " + Convert.ToDouble(items["Column13"].ToString()) + @",
                                        //                               " + Convert.ToDouble(items["Column14"].ToString()) + @",
                                        //                               " + (items["Column15"].ToString().Trim() != "" ? Int16.Parse(items["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
                                        //                               " + sanadtype + @", @ResidId ,
                                        //                               '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                        //                               " + Convert.ToDouble(items["Column22"].ToString()) + @",
                                        //                               NULL)";



                                        //                                               }
                                        //                                               else
                                        //                                               {


                                        //                                                   CmdReturn += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                        //             ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                        //             ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                        //             ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
                                        //               VALUES (@DocIDReturn,'" + items["Column01"].ToString() + @"',
                                        //                              " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                        //                               '" + _AccInfo[1].ToString() + @"',
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                        //                               " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                        //                                " + (string.IsNullOrWhiteSpace(items["Column07"].ToString().Trim()) ? "NULL" : items["Column07"].ToString()) + @",
                                        //                               " + (string.IsNullOrWhiteSpace(items["Column08"].ToString().Trim()) ? "NULL" : items["Column08"].ToString()) + @",
                                        //                              " + (string.IsNullOrWhiteSpace(items["Column09"].ToString().Trim()) ? "NULL" : items["Column09"].ToString()) + @",
                                        //                               ' مرجوعی " + items["Column10"].ToString().Trim() + @"',
                                        //                               " + Convert.ToInt64(items["Column12"].ToString()) + @",
                                        //                               " + Convert.ToInt64(items["Column11"].ToString()) + @",
                                        //                               " + Convert.ToDouble(items["Column13"].ToString()) + @",
                                        //                               " + Convert.ToDouble(items["Column14"].ToString()) + @",
                                        //                               " + (items["Column15"].ToString().Trim() != "" ? Int16.Parse(items["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
                                        //                               " + sanadtype + @", @KeyReturn ,
                                        //                               '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                        //                               " + Convert.ToDouble(item["Column22"].ToString()) + @",
                                        //                               29)";


                                        //                                               }


                                        //                                           }


                                        #endregion
                                        //}
                                        CmdReturn += @" Update " + ConSale.Database + @".dbo.Table_018_MarjooiSale set Column10=@DocIDReturn where ColumnId=" + sanadreturn.Rows[0]["idhedear"].ToString();


                                    }

                                 

                                    #endregion

                                }

                            }

                            CmdReturn += @" Update " + ConSale.Database + @".dbo.Table_018_MarjooiSale set Column09=@ResidId where ColumnId=" + item["ColumnId"];
                            if (CmdReturn != "")
                                Class_BasicOperation.SqlTransactionMethodExecuteNonQuery(ConSale.ConnectionString, CmdReturn);
                            this.Cursor = Cursors.Default;
                        }
                    }
                    #endregion
                    string DraftSetValue = string.Empty;
                    string DraftNotSetValue = string.Empty;
                    #region محاسبه ارزش کالا های حواله و سطر بهای تمام شده

                    if (saleIds != "" )
                    {

                        CmdArzesh = "";
                        DataTable ArzeshTable = clDoc.ReturnTable(ConSale.ConnectionString,
                                                           @"SELECT  
                                                                           tsf.columnid,tsf.column01, tsf.Column09, tsf.Column10 as DocID,dff.column02 as date,dff.column03 as ware,
                                                                            tsf.Column44 as Project
                                                                    FROM   Table_010_SaleFactor tsf join " + ConWare.Database + @".dbo.Table_007_PwhrsDraft dff on dff.columnid=tsf.Column09
                                                                    WHERE  (
                                                                               tsf.columnid in (" + saleIds.TrimEnd(',') + @")
                                                                           )
                                                                          
                                                                           AND (tsf.column17 = 0)--باطل نيست
                                                                          -- AND (tsf.column19 = 0)--مرجوع نيست
                                                                           AND tsf.Column53 = 0-- بسته نشده
                                                                           order by   tsf.column02,tsf.column14,tsf.columnid

                                                                           
                                                                             ");


                        foreach (DataRow ar in ArzeshTable.Rows)
                        {
                            try
                            {

                                DataTable Table = clDoc.ReturnTable(ConWare.ConnectionString, @"Select * from Table_008_Child_PwhrsDraft where Column01=" + ar["Column09"]);
                                valueHavale = 0;


                                //محاسبه ارزش و ذخیره آن در جدول Child1
                                foreach (DataRow item2 in Table.Rows)
                                {
                                    if (Class_BasicOperation._WareType)
                                    {
                                        DataTable TurnTable = clDoc.ReturnTable(ConWare.ConnectionString, @"EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + ar["ware"].ToString());

                                        DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + ar["Column09"] + " and DetailID=" + item2["Columnid"].ToString());
                                        clDoc.RunSqlCommand(ConWare.ConnectionString, @"UPDATE " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                            + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString() + "");
                                        valueHavale += Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4);
                                    }

                                    else
                                    {
                                        DataTable TurnTable = clDoc.ReturnTable(ConWare.ConnectionString, @"EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + ar["ware"].ToString() + ",@Date='" + ar["date"].ToString() + "',@id=" + ar["Column09"] + ",@residid=0");


                                        clDoc.RunSqlCommand(ConWare.ConnectionString, @"UPDATE " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                       + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString() + "");
                                        valueHavale += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4);
                                    }
                                }



                                if (Class_BasicOperation._FinType)//بهای تمام شده
                                {
                                    if (valueHavale > 0 && Convert.ToInt32(ar["DocID"]) > 0)
                                    {
                                        string[] _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column07"].ToString());

                                        CmdArzesh += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + ar["DocID"] + @",'" + bahaDT.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NUll, NULL , "+ar["Project"] +@" ,
                   " + "'بهای تمام شده فاکتور فروش ش " + ar["column01"] + "'," + Convert.ToInt64(valueHavale) + @",0,0,0,-1,26," + int.Parse(ar["Column09"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                        _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column13"].ToString());

                                        CmdArzesh += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + ar["DocID"] + @",'" + bahaDT.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ar["Project"] + @" ,
                   " + "'بهای تمام شده فاکتور فروش ش " + ar["column01"] + "',0," + Convert.ToInt64(valueHavale) + @",0,0,-1,26," + int.Parse(ar["Column09"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                                    }

                                  else  if (valueHavale == 0 && Convert.ToInt32(ar["DocID"]) > 0)
                                    {
                                        string[] _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column07"].ToString());

                                        CmdArzesh += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + ar["DocID"] + @",'" + bahaDT.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NUll, NULL , " + ar["Project"] + @" ,
                   " + "'بهای تمام شده فاکتور فروش ش " + ar["column01"] + "'," + 1 + @",0,0,0,-1,26," + int.Parse(ar["Column09"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                        _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column13"].ToString());

                                        CmdArzesh += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + ar["DocID"] + @",'" + bahaDT.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ar["Project"] + @" ,
                   " + "'بهای تمام شده فاکتور فروش ش " + ar["column01"] + "',0," + 1 + @",0,0,-1,26," + int.Parse(ar["Column09"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                                    }


                                }
                                DraftSetValue += ar["columnid"].ToString() + ",";

                            }
                            catch
                            {
                                DraftNotSetValue += ar["column01"].ToString() + ",";
                            }


                        }

                    }
                    #endregion
                    string ReceiptSetValue = "";
                    string ReceiptNotSetValue = "";
                    #region محاسبه ارزش کالاهای رسید وسطر بهای تمام شده
                    if (ReturnId != "" )
                    {
                        DataTable dttablearzesh = new DataTable();
                        DataTable dtresipt = new DataTable();


                        DataTable dtsalereturn = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT        dbo.Table_018_MarjooiSale.columnid AS idreturn, dbo.Table_018_MarjooiSale.column01 AS Numreturn,dbo.Table_018_MarjooiSale.column02 AS date, dbo.Table_018_MarjooiSale.column09 AS idresid, dbo.Table_018_MarjooiSale.column10 AS iddoc
FROM            dbo.Table_018_MarjooiSale INNER JOIN
                         " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ON dbo.Table_018_MarjooiSale.column09 = " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid
WHERE        (dbo.Table_018_MarjooiSale.Column27 <> 1) AND Table_018_MarjooiSale.Columnid in (" + ReturnId.TrimEnd(',') + @")
                                        ORDER BY date");

                        if (dtsalereturn.Rows.Count > 0)
                        {
                            foreach (DataRow dtretrn in dtsalereturn.Rows)
                            {
                                try
                                {
                                    dttablearzesh = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT        dbo.Table_018_MarjooiSale.columnid AS idreturn, dbo.Table_018_MarjooiSale.column01 AS numreturn, dbo.Table_018_MarjooiSale.column09 AS residid, dbo.Table_018_MarjooiSale.column10 AS docid, dbo.Table_018_MarjooiSale.column30 AS Project,
                                             " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column02 AS dateresid, " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.column03 AS whrs
                                        FROM            dbo.Table_018_MarjooiSale INNER JOIN
                                                                 " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ON dbo.Table_018_MarjooiSale.column09 = " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt.columnid
                                        WHERE        (dbo.Table_018_MarjooiSale.columnid IN (" + dtretrn["idreturn"].ToString() + "))");

                                    string okreturn = "";


                                    foreach (DataRow dtarzeshreturn in dttablearzesh.Rows)
                                    {
                                        okreturn += dtarzeshreturn["idreturn"].ToString() + ",";


                                        valueRecipt = 0;

                                        dtresipt = clDoc.ReturnTable(ConWare.ConnectionString, @"select * from Table_012_Child_PwhrsReceipt where column01=" + dtarzeshreturn["residid"] + "");
                                        foreach (DataRow rows in dtresipt.Rows)
                                        {
                                            if (Class_BasicOperation._WareType)
                                            {
                                                DataTable TurnTable = clDoc.ReturnTable(ConWare.ConnectionString, @"EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + rows["Column02"].ToString() + ", @WareCode = " + dtarzeshreturn["whrs"]);

                                                DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + dtarzeshreturn["residid"] + " and DetailID=" + rows["Columnid"].ToString());


                                                clDoc.RunSqlCommand(ConWare.ConnectionString, @"UPDATE " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                                     + " , Column21=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + rows["ColumnId"].ToString() + "");
                                                valueRecipt += Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4);
                                            }
                                            else
                                            {

                                                string draftid = clDoc.ExScalar(ConSale.ConnectionString, @"select isnull(( select Column09 from Table_010_SaleFactor where Column20=" + dtarzeshreturn["idreturn"].ToString() + "),0)");

                                                DataTable TurnTable = clDoc.ReturnTable(ConWare.ConnectionString, @"EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + rows["Column02"].ToString() + ", @WareCode = " + dtarzeshreturn["whrs"] + ",@Date='" + dtarzeshreturn["dateresid"] + "',@id=" + draftid + ", @residid=" + dtarzeshreturn["residid"] + "");


                                                clDoc.RunSqlCommand(ConWare.ConnectionString, @"UPDATE " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt SET  Column20=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                                + " , Column21=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(rows["Column07"].ToString()), 4) + " where ColumnId=" + rows["ColumnId"].ToString() + "");
                                                valueRecipt += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(rows["Column07"].ToString()), 4);
                                            }

                                        }


                                        ///سطر بهای تمام شده 

                                        if (Class_BasicOperation._FinType)//بهای تمام شده
                                        {
                                            if (valueRecipt > 0 && Convert.ToInt32(dtarzeshreturn["docid"]) > 0)
                                            {
                                                string[] _AccInfo = clDoc.ACC_Info(dtbahaReturn.Rows[0]["bed"].ToString());

                                                CmdArzesh += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + dtarzeshreturn["docid"] + @",'" + dtbahaReturn.Rows[0]["bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NUll, NULL , " + dtarzeshreturn["Project"] + @" ,
                   " + "'بهای تمام شده فاکتور مرجوعی ش " + dtarzeshreturn["numreturn"] + "'," + Convert.ToInt64(valueRecipt) + @",0,0,0,-1,27," + int.Parse(dtarzeshreturn["residid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                _AccInfo = clDoc.ACC_Info(dtbahaReturn.Rows[0]["bes"].ToString());

                                                CmdArzesh += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + dtarzeshreturn["docid"] + @",'" + dtbahaReturn.Rows[0]["bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + dtarzeshreturn["Project"] + @" ,
                   " + "'بهای تمام شده فاکتور مرجوعی ش " + dtarzeshreturn["numreturn"] + "',0," + Convert.ToInt64(valueRecipt) + @",0,0,-1,27," + int.Parse(dtarzeshreturn["residid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                            }

                                            else  if (valueRecipt == 0 && Convert.ToInt32(dtarzeshreturn["docid"]) > 0)
                                            {
                                                string[] _AccInfo = clDoc.ACC_Info(dtbahaReturn.Rows[0]["bed"].ToString());

                                                CmdArzesh += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + dtarzeshreturn["docid"] + @",'" + dtbahaReturn.Rows[0]["bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NUll, NULL , " + dtarzeshreturn["Project"] + @" ,
                   " + "'بهای تمام شده فاکتور مرجوعی ش " + dtarzeshreturn["numreturn"] + "'," + 1 + @",0,0,0,-1,27," + int.Parse(dtarzeshreturn["residid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                _AccInfo = clDoc.ACC_Info(dtbahaReturn.Rows[0]["bes"].ToString());

                                                CmdArzesh += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + dtarzeshreturn["docid"] + @",'" + dtbahaReturn.Rows[0]["bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + dtarzeshreturn["Project"] + @" ,
                   " + "'بهای تمام شده فاکتور مرجوعی ش " + dtarzeshreturn["numreturn"] + "',0," + 1 + @",0,0,-1,27," + int.Parse(dtarzeshreturn["residid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                            }

                                        }



                                    }

                                    ReceiptSetValue += dtretrn["idreturn"] + ",";

                                }
                                catch
                                {

                                    ReceiptNotSetValue += dtretrn["numreturn"] + ",";


                                }

                            }

                        }



                    }
                    #endregion
                    if (DraftNotSetValue != "" || ReceiptNotSetValue != "")
                    {
                        string msg = "";
                        if (ReceiptNotSetValue != "")
                            msg = "ارزش گذاری و محاسبه بهای تمام شده برای فاکتور های مرجوعی ذیل امکان پذیر نمی باشد:" + Environment.NewLine +
                           ReceiptNotSetValue.TrimEnd(',');
                        if (DraftNotSetValue != "")
                            msg = "ارزش گذاری و محاسبه بهای تمام شده برای فاکتور های فروش ذیل امکان پذیر نمی باشد:" + Environment.NewLine +
                           DraftNotSetValue.TrimEnd(',');
                        Class_BasicOperation.ShowMsg("", msg, "Information");
                        return;
                    }
                    else if (CmdArzesh != "")
                    {
                        Class_BasicOperation.SqlTransactionMethodExecuteNonQuery(ConSale.ConnectionString, CmdArzesh);
                    }

                    #region بستن صندوق

                    table_96_CloseCashBindingSource.EndEdit();
                    table_97_ReceivedFromCustomersBindingSource.EndEdit();
                    table_98_ReceivedFromBankBindingSource.EndEdit();
                    table_99_LosesFromCashBindingSource.EndEdit();
                    this.table_96_CloseCashTableAdapter.Update(this.dataSet6.Table_96_CloseCash);
                    this.table_99_LosesFromCashTableAdapter.Update(this.dataSet6.Table_99_LosesFromCash);
                    this.table_98_ReceivedFromBankTableAdapter.Update(this.dataSet6.Table_98_ReceivedFromBank);
                    this.table_97_ReceivedFromCustomersTableAdapter.Update(this.dataSet6.Table_97_ReceivedFromCustomers);
                    //edite insert
                    string CloseId = ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    #endregion

                    #region سند بستن صندوق
                    if (gx_recivefrombank.RowCount > 0 || gx_losefromcash.RowCount > 0
                        || txt_CashDeductionAddition.Value.ToString() != "0" || txt_CashAcountDifference.Value.ToString() != "0")
                    {

                        CmdClose = @"  declare @DocIDClose int 
                                                 declare @DocNumClose int  ";

                        CmdClose += @" set @DocNumClose=(SELECT ISNULL((SELECT MAX(Column00)  FROM   " + ConAcnt.Database + @".dbo.Table_060_SanadHead ), 0 )) + 1   INSERT INTO " + ConAcnt.Database + @".dbo.Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from " + ConAcnt.Database + @".dbo.Table_060_SanadHead),0))+1,'" + faDatePickerStrip1.FADatePicker.Text + "',2,0,' صدور سند بستن صندوق شماره " + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + "','" + Class_BasicOperation._UserName +
             "',getdate()); SET @DocIDClose=SCOPE_IDENTITY()";


                        foreach (Janus.Windows.GridEX.GridEXRow item in gx_losefromcash.GetDataRows())
                        {

                            if (Convert.ToDouble(item.Cells["Column04"].Value) > 0)
                            {
                                string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column02"].Value.ToString());

                                CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDClose,'" + item.Cells["Column02"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (item.Cells["Column03"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["Column03"].Value.ToString()) ? (item.Cells["Column03"].Value.ToString()) : "NULL") + @", NULL , NULL ,
                   " + "'بابت واریز/برداشت روزانه از صندوق'," + Convert.ToInt64(item.Cells["Column04"].Value) + @",0,0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                            }
                        }

                        foreach (Janus.Windows.GridEX.GridEXRow item in gx_recivefrombank.GetDataRows())
                        {
                            if (Convert.ToDouble(item.Cells["Column03"].Value) > 0)
                            {
                                string acc = clDoc.ExScalarQuery(ConBank.ConnectionString, "select Column12 from Table_020_BankCashAccInfo where ColumnId=" + item.Cells["Column02"].Value);
                                //item.Cells["Column02"].Column.DropDown.GetValue("Column12").ToString();

                                string[] _AccInfo = clDoc.ACC_Info(acc);

                                CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDClose,'" + acc + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'واریز/برداشت روزانه صندوق'," + Convert.ToInt64(item.Cells["Column03"].Value) + @",0,0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            }
                        }
                        customerrecive = Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
                        losefromcash = Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                        if (customerrecive + losefromcash > 0)
                        {

                            DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");

                            string[] _AccInfo = clDoc.ACC_Info(acc.Rows[0]["column08"].ToString());

                            CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDClose,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'بابت واریز/برداشت روزانه از صندوق' " + ",0," + Convert.ToInt64(customerrecive + losefromcash) + @",0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                        }
                        DataTable ddt = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT isnull(tst.Column02,'') as bed,isnull(tst.Column02,'') as bes
                                                                                                                FROM  Table_030_Setting tst
                                                                                                                WHERE  tst.ColumnId = 64");
                        decimal CashDeductionAddition = Convert.ToDecimal(txt_CashDeductionAddition.Value);

                        if (CashDeductionAddition != Convert.ToDecimal(0))
                        {
                            DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");



                            if (CashDeductionAddition < Convert.ToDecimal(0))
                            {


                                string[] _AccInfo = clDoc.ACC_Info(acc.Rows[0]["column08"].ToString());

                                CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDClose,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'بابت کسر و اضافه صندوق'," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                _AccInfo = clDoc.ACC_Info((ddt.Rows[0]["bes"].ToString()));

                                CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDClose,'" + ddt.Rows[0]["bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'بابت کسر و اضافه صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";




                            }
                            else
                            {


                                string[] _AccInfo = clDoc.ACC_Info(ddt.Rows[0]["bed"].ToString());

                                CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDClose,'" + ddt.Rows[0]["bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'بابت کسر و اضافه صندوق'," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                _AccInfo = clDoc.ACC_Info((acc.Rows[0]["column08"].ToString()));

                                CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDClose,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'بابت کسر و اضافه صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashDeductionAddition)) + @",0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                            }

                        }
                        decimal CashAcountDifference = Convert.ToDecimal(txt_CashAcountDifference.Value);
                        if (CashAcountDifference != Convert.ToDecimal(0))
                        {
                            DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");

                            if (CashAcountDifference < Convert.ToDecimal(0))
                            {


                                string[] _AccInfo = clDoc.ACC_Info(ddt.Rows[0]["bed"].ToString());

                                CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDClose,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'بابت اختلاف حساب صندوق'," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                _AccInfo = clDoc.ACC_Info((acc.Rows[0]["column08"].ToString()));

                                CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDClose,'" + ddt.Rows[0]["bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'بابت اختلاف حساب صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                            }
                            else
                            {



                                string[] _AccInfo = clDoc.ACC_Info(acc.Rows[0]["column08"].ToString());

                                CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDClose,'" + ddt.Rows[0]["bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'بابت اختلاف حساب صندوق'," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                _AccInfo = clDoc.ACC_Info((ddt.Rows[0]["bes"].ToString()));

                                CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocIDClose,'" + acc.Rows[0]["column08"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'بابت اختلاف حساب صندوق'," + "0," + Convert.ToInt64(Math.Abs(CashAcountDifference)) + @",0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                            }


                        }

                        CmdClose += " update Table_96_CloseCash set Column16=@DocIDClose where ColumnId=" + CloseId;
                        CmdClose += "select   @DocIDClose  ,   @DocNumClose ";





              //          #region به ازای مبلغ کسرو اضافه صندوق
              //          if (txt_CashDeductionAddition.Text != "0")
              //          {

              //              DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
              //                                                                                                  FROM  Table_002_SalesTypes tst
              //                                                                                                  WHERE  tst.Column21 = 1");



              //              DataTable ddt1 = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT isnull(tst.Column02,'') as bed,isnull(tst.Column02,'') as bes
              //                                                                                                  FROM  Table_030_Setting tst
              //                                                                                                  WHERE  tst.ColumnId = 64");

              //              if (Convert.ToDecimal(txt_CashDeductionAddition.Text) < 0)
              //              {

              //                  string[] _AccInfo = clDoc.ACC_Info(ddt1.Rows[0]["bed"].ToString());


              //                  CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              //,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
              //      VALUES(@DocIDClose,'" + ddt1.Rows[0]["bed"].ToString() + @"',
              //                  " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
              //                  " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
              //                  " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
              //                  " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
              //                  NULL, NULL , NULL ,
              //     " + "'بابت کسر و اضافه صندوق'," + Convert.ToDecimal(txt_CashDeductionAddition.Text) + @",0,0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
              //                    Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


              //                  _AccInfo = clDoc.ACC_Info(acc.Rows[0]["column08"].ToString());


              //                  CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              //,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
              //      VALUES(@DocIDClose,'" + acc.Rows[0]["column08"].ToString() + @"',
              //                  " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
              //                  " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
              //                  " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
              //                  " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
              //                  NULL, NULL , NULL ,
              //     " + "'بابت کسر و اضافه صندوق'," + ",0" + @"," + Convert.ToDecimal(txt_CashDeductionAddition.Text) + ",0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
              //                                                Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

              //              }

              //              else if (Convert.ToDecimal(txt_CashDeductionAddition.Text) > 0)
              //              {
              //                  string[] _AccInfo = clDoc.ACC_Info(acc.Rows[0]["column08"].ToString());

              //                  CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              //,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
              //      VALUES(@DocIDClose,'" + acc.Rows[0]["column08"].ToString() + @"',
              //                  " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
              //                  " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
              //                  " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
              //                  " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
              //                  NULL, NULL , NULL ,
              //     " + "'بابت کسر و اضافه صندوق'," + Convert.ToDecimal(txt_CashDeductionAddition.Text) + @",0,0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
              //                    Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



              //                  _AccInfo = clDoc.ACC_Info(ddt1.Rows[0]["bes"].ToString());

              //                  CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              //,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
              //      VALUES(@DocIDClose,'" + ddt1.Rows[0]["bes"].ToString() + @"',
              //                  " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
              //                  " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
              //                  " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
              //                  " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
              //                  NULL, NULL , NULL ,
              //     " + "'بابت کسر و اضافه صندوق'," + " 0," + Convert.ToDecimal(txt_CashDeductionAddition.Text) + ",0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
              //                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";
              //              }

              //          }

              //          #endregion 




                    }


             //       #region به ازای مبلغ کسرو اضافه صندوق

             //       else if (txt_CashDeductionAddition.Text != "0" && gx_recivefrombank.RowCount == 0 &&
             //          gx_losefromcash.RowCount == 0 && txt_CashDeductionAddition.Value.ToString() == "0"
             //          && txt_CashAcountDifference.Value.ToString() == "0")
             //       {
             //           DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
             //                                                                                                   FROM  Table_002_SalesTypes tst
             //                                                                                                   WHERE  tst.Column21 = 1");

             //           DataTable ddt1 = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT isnull(tst.Column02,'') as bed,isnull(tst.Column02,'') as bes
             //                                                                                                   FROM  Table_030_Setting tst
             //                                                                                                   WHERE  tst.ColumnId = 64");

             //           CmdClose = @"  declare @DocIDClose int 
             //                                    declare @DocNumClose int  ";

             //           CmdClose += @" set @DocNumClose=(SELECT ISNULL((SELECT MAX(Column00)  FROM   " + ConAcnt.Database + @".dbo.Table_060_SanadHead ), 0 )) + 1   INSERT INTO " + ConAcnt.Database + @".dbo.Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
             //   VALUES((Select Isnull((Select Max(Column00) from " + ConAcnt.Database + @".dbo.Table_060_SanadHead),0))+1,'" + faDatePickerStrip1.FADatePicker.Text + "',2,0,' صدور سند بستن صندوق شماره " + ((DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current)["ColumnId"] + "','" + Class_BasicOperation._UserName +
             //"',getdate()); SET @DocIDClose=SCOPE_IDENTITY()";


             //           if (Convert.ToDecimal(txt_CashDeductionAddition.Text) < 0)
             //           {
             //               string[] _AccInfo = clDoc.ACC_Info(ddt1.Rows[0]["bed"].ToString());


             //               CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
             // ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
             //       VALUES(@DocIDClose,'" + ddt1.Rows[0]["bed"].ToString() + @"',
             //                   " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
             //                   " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
             //                   " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
             //                   " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
             //                   NULL, NULL , NULL ,
             //      " + "'بابت کسر و اضافه صندوق'," + Convert.ToDecimal(txt_CashDeductionAddition.Text) + @",0,0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
             //                 Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


             //               _AccInfo = clDoc.ACC_Info(acc.Rows[0]["column08"].ToString());


             //               CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
             // ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
             //       VALUES(@DocIDClose,'" + acc.Rows[0]["column08"].ToString() + @"',
             //                   " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
             //                   " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
             //                   " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
             //                   " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
             //                   NULL, NULL , NULL ,
             //      " + "'بابت کسر و اضافه صندوق'," + ",0" + @"," + Convert.ToDecimal(txt_CashDeductionAddition.Text) + ",0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
             //                                             Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";




             //           }

             //           else if (Convert.ToDecimal(txt_CashDeductionAddition.Text) > 0)
             //           {

             //               string[] _AccInfo = clDoc.ACC_Info(acc.Rows[0]["column08"].ToString());

             //               CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
             // ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
             //       VALUES(@DocIDClose,'" + acc.Rows[0]["column08"].ToString() + @"',
             //                   " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
             //                   " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
             //                   " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
             //                   " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
             //                   NULL, NULL , NULL ,
             //      " + "'بابت کسر و اضافه صندوق'," + Convert.ToDecimal(txt_CashDeductionAddition.Text) + @",0,0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
             //                 Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



             //               _AccInfo = clDoc.ACC_Info(ddt1.Rows[0]["bes"].ToString());

             //               CmdClose += @"    INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
             // ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
             //       VALUES(@DocIDClose,'" + ddt1.Rows[0]["bes"].ToString() + @"',
             //                   " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
             //                   " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
             //                   " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
             //                   " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
             //                   NULL, NULL , NULL ,
             //      " + "'بابت کسر و اضافه صندوق'," + " 0," + Convert.ToDecimal(txt_CashDeductionAddition.Text) + ",0,0,-1,300," + CloseId + @",'" + Class_BasicOperation._UserName + "',getdate(),'" +
             //               Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";
             //           }

             //       }

             //       #endregion




                    if (saleIds != "")
                        {
                            CmdClose += " update Table_010_SaleFactor set Column67=" + CloseId + ",column53=1,Column62='" + Class_BasicOperation._UserName + "',Column63=getdate() where columnid in  (" + saleIds.TrimEnd(',') + ")";

                        }
                        if (ReturnId != "")
                        {
                            CmdClose += " update Table_018_MarjooiSale set Column31=" + CloseId + ",Column27=1 where   Columnid in (" + ReturnId.TrimEnd(',') + ")";

                        }



                        dtResult = Class_BasicOperation.SqlTransactionMethod(ConSale.ConnectionString, CmdClose);
                        table_96_CloseCashBindingSource_PositionChanged(null, null);

                    
                    if (dtResult.Rows.Count==0)
                    {
                 
                  Class_BasicOperation.ShowMsg("","بستن صندوق با موفقیت انجام شد"+Environment.NewLine+"به علت صفر بودن صندوق سندی صادر نشد ","Information");
                    bt_ExportDoc.Enabled = false;
                    bt_Search.Enabled = true;  
                    }

                    else
                    {

                        
                        Class_BasicOperation.ShowMsg("","عملیات و صدور سند شماره " + dtResult.Rows[0][1] + " با موفقیت انجام شد","Information");
                        bt_ExportDoc.Enabled = false;
                        bt_Search.Enabled = true;
                    }

         
                    #endregion



                }


            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;

        }
        int stor ;
        private void bt_Search_Click(object sender, EventArgs e)
        {
            try
            {
                if (faDatePickerStrip1.FADatePicker.Text != string.Empty)
                {


                    this.table_99_LosesFromCashBindingSource.CancelEdit();
                    this.table_98_ReceivedFromBankBindingSource.CancelEdit();
                    this.table_97_ReceivedFromCustomersBindingSource.CancelEdit();
                    this.table_96_CloseCashBindingSource.CancelEdit();

                    txt_Otherness.Value = 0;
                    txt_CashAcountDifference.Value = 0;
                    projectId = "";
                    if (mlt_Stor.Text == "" || mlt_Stor.Text.All(char.IsDigit))
                    {
                        //Class_BasicOperation.ShowMsg("", "لطفا فروشگاه مورد نظر را وارد نمایید", "warning");
                        MessageBox.Show("لطفا فروشگاه مورد نظر را وارد نمایید");
                        return;
                    }

                    //foreach (Janus.Windows.GridEX.GridEXRow dr in mlt_Stor.DropDownList.GetCheckedRows())
                    //{
                    //    projectId += dr.Cells["ColumnId"].Value.ToString() + ",";

                    //}
                     stor =int.Parse( mlt_Stor.Value.ToString());
                    bt_New_Click(null, null);

                }
            }
            catch (System.Exception ex)
            {
                if (mlt_Stor.Text == "" || mlt_Stor.Text.All(char.IsDigit))
                {
                    Class_BasicOperation.ShowMsg("", "لطفا فروشگاه مورد نظر را وارد نمایید", "warning");
                    return;
                }
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Frm_029_CloseCash_FormClosing(object sender, FormClosingEventArgs e)
        {
            gx_factors.RemoveFilters();

        }

        private void chehckessentioal()
        {

            discountdt = new DataTable();
            taxdt = new DataTable();
            factordt = new DataTable();
            waredt = new DataTable();
            Sanaddt = new DataTable();
            TableNoDocNoDraft = new DataTable();
            bahaDT = new DataTable();
            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));
            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));

            if (Class_BasicOperation._FinType)///سیستم دائمی
            {
                SqlDataAdapter Adapter1 = new SqlDataAdapter(
                                                                       @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                    Column14, Column15, Column16
                                                                        FROM            Table_105_SystemTransactionInfo
                                                                        WHERE        (Column00 = 8) ", ConBase);
                Adapter1.Fill(bahaDT);

                dtbahaReturn = clDoc.ReturnTable(ConBase.ConnectionString, @" select Column07 as bed, column13 as bes from Table_105_SystemTransactionInfo where Column00=14");
                //if (Class_BasicOperation._WareType)
                //{
                    if (dtbahaReturn.Rows[0]["bed"].ToString() == "" || dtbahaReturn.Rows[0]["bes"].ToString() == "")
                    {
                        throw new Exception("شماره حساب معتبر برای بهای تمام شده فروش، از قسمت تنظیمات تراکنش سیستم تنظیم نمایید");

                    }
                    if (ReturnId!="")
                    {
                        idsdt = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT DISTINCT columnid,Column09,Column10
                                            FROM   Table_018_MarjooiSale 
                                            WHERE  (column09 = 0 OR column10 = 0)
                                                   AND Column27 = 0    AND columnid in (" + ReturnId.TrimEnd(',') + ")");
                    }
                    
                    if (idsdt.Rows.Count > 0)
                    {
                        BedBesReturn = clDoc.ReturnTable(ConBase.ConnectionString, @" select Column07 as Bed , Column13 As Bes from Table_105_SystemTransactionInfo where Column00=12");

                        if (BedBesReturn.Rows[0]["Bed"].ToString() == "" || BedBesReturn.Rows[0]["Bes"].ToString() == "")
                        {
                            MessageBox.Show("لطفا سر فصل فاکتور مرجوعی فروش رو از قسمت تنظیمات تراکنش سیستم تنظیم نمایید");
                            return;
                        }
                    }
                //}
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + bahaDT.Rows[0]["Column13"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("شماره حساب معتبر برای بهای تمام شده فروش، از قسمت تنظیمات تراکنش سیستم تنظیم نمایید");


                }



                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + bahaDT.Rows[0]["Column07"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("شماره حساب معتبر برای بهای تمام شده را از قسمت تنظیمات تراکنش سیستم تنظیم نمایید");
                }

            }

            if (gx_factors.RowCount > 0)
            {
                if (saleIds!="")
                {
                    TableNoDocNoDraft = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT DISTINCT tsf.columnid,tsf.Column09,Column10
                                            FROM   Table_010_SaleFactor tsf
                                            WHERE  (tsf.column09 = 0 OR tsf.column10 = 0)
                                                   AND tsf.Column53 = 0    AND tsf.columnid in( " + saleIds.TrimEnd(',') + ")");

                }
               





                if (TableNoDocNoDraft.Rows.Count > 0)
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 69))
                        throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");

                    waredt = clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT        isnull(Column02,0) as Column02
                                                                        FROM           Table_030_Setting
                                                                        WHERE        (ColumnId in (45,46)) order by ColumnId  ");

                    if (waredt.Rows.Count >= 1)
                    {
                        if (clDoc.ExScalarQuery(ConWare.ConnectionString, @"IF EXISTS ( SELECT *
                                                                   FROM   table_005_PwhrsOperation
                                                                   WHERE  columnid = " + waredt.Rows[0]["Column02"] + @"  )
                                                                SELECT 1 AS ok ELSE SELECT 0 AS ok") == "0")
                            throw new Exception("عملکرد انتخاب نشده است");
                    }

                    else
                        throw new Exception("  انبار تعریف نشده است");


                    discountdt = clDoc.ReturnTable(ConSale.ConnectionString,
                                                     @"SELECT       isnull( column10,'') as column10 , isnull(column16,'') as column16
                                                                    FROM            Table_024_Discount
                                                                     group by column10,column16
                                                                     ");

                    foreach (DataRow dr in discountdt.Rows)
                    {
                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Con.Open();
                            SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + dr["Column10"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                            if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                                throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");

                        }





                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Con.Open();
                            SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + dr["Column16"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                            if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                                throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");
                        }


                    }

                }


                foreach (DataRow idro in TableNoDocNoDraft.Rows)
                {

                    Sanaddt = new DataTable();
                    factordt = new DataTable();
                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT FactorTable.columnid,
                                               FactorTable.column01,
                                               FactorTable.date,
                                               ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
                                               ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
                                               FactorTable.Bed,
                                               FactorTable.Bes,
                                               FactorTable.NetTotal,FactorTable.person,FactorTable.saletype,FactorTable.ware,FactorTable.func,FactorTable.ware,FactorTable.Project
                                        FROM   (
                                                   SELECT dbo.Table_010_SaleFactor.columnid,
                                                          dbo.Table_010_SaleFactor.column01,
                                                          dbo.Table_010_SaleFactor.column02 AS Date,
                                                          dbo.Table_010_SaleFactor.column03 AS person,
                                                          dbo.Table_010_SaleFactor.column36 AS saletype,
                                                          dbo.Table_010_SaleFactor.column42 AS ware,
                                                          dbo.Table_010_SaleFactor.column43 AS func,
                                                          OtherPrice.PlusPrice AS Ezafat,
                                                          OtherPrice.MinusPrice AS Kosoorat,
                                                          OtherPrice.Bed,
                                                          OtherPrice.Bes,
                                                          dbo.Table_010_SaleFactor.Column28 AS NetTotal,
                                                          dbo.Table_010_SaleFactor.Column44 AS Project
                                                   FROM   dbo.Table_010_SaleFactor
                                                         
                                                          LEFT OUTER JOIN (
                                                                   SELECT columnid,
                                                                          SUM(PlusPrice) AS PlusPrice,
                                                                          SUM(MinusPrice) AS MinusPrice,
                                                                          Bed,
                                                                          Bes
                                                                   FROM   (
                                                                              SELECT Table_010_SaleFactor_2.columnid,
                                                                                     SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                                                     PlusPrice,
                                                                                     0 AS MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = dbo.Table_012_Child2_SaleFactor.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_2
                                                                                          ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                                                              Table_010_SaleFactor_2.columnid
                                                                              WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_2.columnid,
                                                                                     dbo.Table_012_Child2_SaleFactor.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                              UNION ALL
                                                                              SELECT Table_010_SaleFactor_1.columnid,
                                                                                     0 AS PlusPrice,
                                                                                     SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                                                     MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                                                     Table_012_Child2_SaleFactor_1
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = 
                                                                                              Table_012_Child2_SaleFactor_1.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_1
                                                                                          ON  
                                                                                              Table_012_Child2_SaleFactor_1.column01 = 
                                                                                              Table_010_SaleFactor_1.columnid
                                                                              WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_1.columnid,
                                                                                     Table_012_Child2_SaleFactor_1.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                          ) AS OtherPrice_1
                                                                   GROUP BY
                                                                          columnid,
                                                                          OtherPrice_1.Bed,
                                                                          OtherPrice_1.Bes
                                                               ) AS OtherPrice
                                                               ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid
                                               ) AS FactorTable
                                        WHERE  FactorTable.columnid=" + int.Parse(idro["columnid"].ToString()) + @"
                                                                                                           ", ConSale);

                    Adapter.Fill(Sanaddt);


                    Adapter = new SqlDataAdapter(
                                                                    @"SELECT       isnull( column08,'') as Column07,isnull(column14,'') as Column13
                                                                        FROM            Table_002_SalesTypes
                                                                        WHERE        (columnid = " + Sanaddt.Rows[0]["saletype"] + ") ", ConBase);
                    Adapter.Fill(factordt);


                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                    {
                        Con.Open();
                        SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + factordt.Rows[0]["Column13"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                        if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                            throw new Exception("شماره حساب معتبر را در تعریف انواع فروش وارد کنید");


                    }



                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                    {
                        Con.Open();
                        SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + factordt.Rows[0]["Column07"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                        if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                            throw new Exception("شماره حساب معتبر را در تعریف انواع فروش وارد کنید");
                    }
                    TPerson.Rows.Clear();
                    TAccounts.Rows.Clear();
                    //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
                    clDoc.CheckForValidationDate(Sanaddt.Rows[0]["date"].ToString());


                    int LastDocnum = LastDocNum(Sanaddt.Rows[0]["date"].ToString());
                    if (LastDocnum > 0)
                        clDoc.IsFinal(LastDocnum);
                    if (Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()) <= Convert.ToDouble(0))
                        throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

                    if (Class_BasicOperation._FinType)
                    {
                        All_Controls_Row1(bahaDT.Rows[0]["Column13"].ToString(), null, null, null);
                        All_Controls_Row1(bahaDT.Rows[0]["Column07"].ToString(), null, null, null);

                    }


                    All_Controls_Row1(factordt.Rows[0]["Column07"].ToString(), int.Parse(Sanaddt.Rows[0]["person"].ToString()), null, null);
                    All_Controls_Row1(factordt.Rows[0]["Column13"].ToString(), null, null, null);
                    TPerson.Rows.Add(Int32.Parse(Sanaddt.Rows[0]["person"].ToString()), factordt.Rows[0]["Column07"].ToString(), Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()));
                    TAccounts.Rows.Add(factordt.Rows[0]["Column13"].ToString(), (-1 * Convert.ToDouble((Sanaddt.Rows[0]["NetTotal"]))));
                    TAccounts.Rows.Add(factordt.Rows[0]["Column07"].ToString(), (Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"])));

                    foreach (DataRow dr in Sanaddt.Rows)
                    {
                        if (dr["ware"] == DBNull.Value || dr["ware"] == null || string.IsNullOrWhiteSpace(dr["ware"].ToString()))
                            throw new Exception("انبار در فاکتور فروش ش " + dr["column01"] + "تعریف نشده است");


                        using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Conacnt.Open();
                            SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + dr["ware"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                            if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0)
                                throw new Exception("برای صدور حواله فاکتور فروش ش " + dr["column01"] + " به انبار انتخاب شده دسترسی ندارید");

                        }

                        if (Convert.ToDouble(dr["Ezafat"]) > 0)
                        {
                            All_Controls_Row1(dr["Bed"].ToString(), int.Parse(dr["person"].ToString()), null, null);
                            All_Controls_Row1(dr["Bes"].ToString(), null, null, null);
                            TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Ezafat"])));
                            TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Ezafat"])));
                            TPerson.Rows.Add(Int32.Parse(dr["person"].ToString()), dr["Bed"].ToString(), Convert.ToDouble(dr["Ezafat"]));


                        }
                        if (Convert.ToDouble(dr["Kosoorat"]) > 0)
                        {
                            All_Controls_Row1(dr["Bes"].ToString(), int.Parse(dr["person"].ToString()), null, null);
                            All_Controls_Row1(dr["Bed"].ToString(), null, null, null);
                            TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Kosoorat"])));
                            TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Kosoorat"])));
                            TPerson.Rows.Add(Int32.Parse(dr["person"].ToString()), dr["Bes"].ToString(), Convert.ToDouble(dr["Kosoorat"]));
                        }
                    }
                }
                Classes.CheckCredits clCredit = new Classes.CheckCredits();

                clCredit.CheckAccountCredit(TAccounts, 0);
                clCredit.CheckPersonCredit(TPerson, 0);
                //سند اختتامیه صادر نشده باشد
                clDoc.CheckExistFinalDoc();

            }
            #region بستن صندوق

            foreach (Janus.Windows.GridEX.GridEXRow item in gx_recivefrombank.GetDataRows())
            {
                //string acc = item.Cells["Column02"].Column.DropDown.GetValue("Column12").ToString();
                string acc = clDoc.ExScalarQuery(ConBank.ConnectionString, "select isnull(Column12,'') as Column12 from Table_020_BankCashAccInfo where ColumnId=" + item.Cells["Column02"].Value);

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + acc + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("سرفصل معتبر در حساب های بانکی  وارد کنید");
                }



            }
            DataTable SalesTypes = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");
            foreach (DataRow dr1 in SalesTypes.Rows)
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + dr1["column08"] + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("سرفصل معتبر در انواع فروش وارد کنید");
                }
            }
            DataTable ddt = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT isnull(tst.Column02,'') as bed,isnull(tst.Column02,'') as bes
                                                                                                                FROM  Table_030_Setting tst
                                                                                                                WHERE  tst.ColumnId = 64");
            if (ddt.Rows.Count == 1)
            {

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + ddt.Rows[0]["bed"] + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("سرفصل معتبر برای کسر/اضافه صندوق در تنطیمات وارد کنید");




                }

            }
            else
                throw new Exception("سرفصل کسر و اضافه صندوق تعریف نشده است");

            if (faDatePickerStrip1.FADatePicker.SelectedDateTime == null)
                throw new Exception("تاتاریخ را وارد کنید");


            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(faDatePickerStrip1.FADatePicker.Text);
            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

            TPerson.Rows.Clear();
            TAccounts.Rows.Clear();
            foreach (Janus.Windows.GridEX.GridEXRow item in gx_losefromcash.GetDataRows())
            {
                if (item.Cells["Column03"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["Column03"].Value.ToString()))
                {
                    All_Controls_Row1(item.Cells["Column02"].Value.ToString(), 1, null, null);
                    TPerson.Rows.Add(Int32.Parse(item.Cells["Column03"].Value.ToString()), item.Cells["Column02"].Value, Convert.ToDouble(item.Cells["Column04"].Value));

                }
                else
                    All_Controls_Row1(item.Cells["Column02"].Value.ToString(), null, null, null);

                TAccounts.Rows.Add(item.Cells["Column02"].Value, (Convert.ToDouble(item.Cells["Column04"].Value)));


            }

            foreach (Janus.Windows.GridEX.GridEXRow item in gx_recivefrombank.GetDataRows())
            {
                //string acc = item.Cells["Column02"].Column.DropDown.GetValue("Column12").ToString();
                string acc = clDoc.ExScalarQuery(ConBank.ConnectionString, "select Column12 from Table_020_BankCashAccInfo where ColumnId=" + item.Cells["Column02"].Value);

                All_Controls_Row1(acc, null, null, null);
                TAccounts.Rows.Add(acc, (Convert.ToDouble(item.Cells["Column03"].Value)));

            }

            decimal customerrecive = Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
            decimal losefromcash = Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

            if (customerrecive + losefromcash > 0)
            {

                DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");

                if (acc.Rows.Count != 1)
                    throw new Exception("سرفصل نوع فروش نقدی تعریف نشده است");


                All_Controls_Row1(acc.Rows[0]["column08"].ToString(), null, null, null);

                TAccounts.Rows.Add(acc.Rows[0]["column08"].ToString(), (-1 * Convert.ToDouble(customerrecive + losefromcash)));

            }
            decimal CashDeductionAddition = Convert.ToDecimal(txt_CashDeductionAddition.Value);
            if (CashDeductionAddition != Convert.ToDecimal(0))
            {
                DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");

                if (acc.Rows.Count != 1)
                    throw new Exception("سرفصل نوع فروش نقدی تعریف نشده است");

                if (CashDeductionAddition < Convert.ToDecimal(0))
                {
                    TAccounts.Rows.Add(acc.Rows[0]["column08"].ToString(), (Convert.ToDouble(Math.Abs(CashDeductionAddition))));
                    TAccounts.Rows.Add(ddt.Rows[0]["bes"], (-1 * Convert.ToDouble(Math.Abs(CashDeductionAddition))));

                    All_Controls_Row1(ddt.Rows[0]["bes"].ToString(), null, null, null);
                    All_Controls_Row1(acc.Rows[0]["column08"].ToString(), null, null, null);

                }
                else
                {
                    TAccounts.Rows.Add(ddt.Rows[0]["bed"], (Convert.ToDouble(CashDeductionAddition)));
                    TAccounts.Rows.Add(acc.Rows[0]["column08"].ToString(), (-1 * Convert.ToDouble(CashDeductionAddition)));
                    All_Controls_Row1(ddt.Rows[0]["bed"].ToString(), null, null, null);


                }

            }
            decimal CashAcountDifference = Convert.ToDecimal(txt_CashAcountDifference.Value);
            if (CashAcountDifference != Convert.ToDecimal(0))
            {
                DataTable acc = clDoc.ReturnTable(ConBase.ConnectionString, @" SELECT top 1 isnull(column08,'') as column08
                                                                                                                FROM  Table_002_SalesTypes tst
                                                                                                                WHERE  tst.Column21 = 1");

                if (acc.Rows.Count != 1)
                    throw new Exception("سرفصل نوع فروش نقدی تعریف نشده است");

                if (CashAcountDifference < Convert.ToDecimal(0))
                {
                    TAccounts.Rows.Add(acc.Rows[0]["column08"].ToString(), (-1 * Convert.ToDouble(Math.Abs(CashAcountDifference))));
                    TAccounts.Rows.Add(ddt.Rows[0]["bed"], (Convert.ToDouble(Math.Abs(CashAcountDifference))));

                }
                else
                {
                    TAccounts.Rows.Add(acc.Rows[0]["column08"].ToString(), (Convert.ToDouble(CashAcountDifference)));
                    TAccounts.Rows.Add(ddt.Rows[0]["bes"], (-1 * Convert.ToDouble(CashAcountDifference)));

                }

            }


            Classes.CheckCredits clCredit1 = new Classes.CheckCredits();
            clCredit1.CheckAccountCredit(TAccounts, 0);
            clCredit1.CheckPersonCredit(TPerson, 0);





            #endregion
        }

        public void All_Controls_Row1(string AccountCode, int? Person, Int16? Center, Int16? Project)
        {

            //*********Control Person
            if (AccHasPerson(AccountCode) == 1)
            {
                if (Person == null)
                {

                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasPerson(AccountCode) == 0)
            {
                if (Person != null)
                {

                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************ Control Center
            if (AccHasCenter(AccountCode) == 1)
            {
                if (Center == null)
                {

                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasCenter(AccountCode) == 0)
            {
                if (Center != null)
                {

                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************* Control Project
            if (AccHasProject(AccountCode) == 1)
            {
                if (Project == null)
                {

                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasProject(AccountCode) == 0)
            {
                if (Project != null)
                {

                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }


            //using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            //{
            //    ConAcnt.Open();
            //    SqlCommand Command = new SqlCommand("Select Control_Person from AllHeaders() where ACC_Code='" + AccountCode + "'", ConAcnt);
            //    if (Person == null && bool.Parse(Command.ExecuteScalar().ToString()))
            //    {
            //        Row.Cells["Column07"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            //        Row.Cells["Column07"].FormatStyle.BackColor = Color.Yellow;
            //        throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }

            ////    Command.CommandText = "Select Control_Center from AllHeaders() where ACC_Code='" + AccountCode + "'";
            ////    if (Center == null && bool.Parse(Command.ExecuteScalar().ToString()))
            ////    {
            ////        Row.Cells["Column08"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            ////        Row.Cells["Column08"].FormatStyle.BackColor = Color.Yellow;
            ////        throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }

            ////    Command.CommandText = "Select Control_Project from AllHeaders() where ACC_Code='" + AccountCode + "'";
            ////    if (Project == null && bool.Parse(Command.ExecuteScalar().ToString()))
            ////    {
            ////        Row.Cells["Column09"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            ////        Row.Cells["Column09"].FormatStyle.BackColor = Color.Yellow;
            ////        throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }
            //}

        }
        public Int16 AccHasPerson(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Person from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        private string AccountName(string AccountCode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("Select ACC_Name from AllHeaders() where ACC_Code='" + AccountCode + "'", Con);
                string _AccountName = Select.ExecuteScalar().ToString();
                return _AccountName;
            }
        }
        private Int16 AccHasCenter(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Center from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        private Int16 AccHasProject(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Project from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        public int LastDocNum(string date)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Isnull((Select Max(isnull( h.Column00,0)) from Table_060_SanadHead h INNER JOIN Table_065_SanadDetail d On d.Column00=h.ColumnId where h.Column01='"+date+"' AND d.Column16=15),0)", ConAcnt);
                int Result = int.Parse(Select.ExecuteScalar().ToString());
                return Result;
            }
        }

        private void Frm_029_CloseCash_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control && bt_ExportDoc.Enabled)
                bt_ExportDoc_Click(sender, e);
            else if (e.KeyCode == Keys.F && e.Control && bt_Search.Enabled)
            {
                bt_Search_Click(sender, e);
            }

            else if (e.KeyCode == Keys.P && bt_Print.Enabled)
            {
                bt_Print_Click(sender, e);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                _05_Sale.Frm_003_MojoodiMaghtaiTedadi frm1 = new Frm_003_MojoodiMaghtaiTedadi();
                frm1.ShowDialog();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gx_recivefromcustomer_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gx_recivefromcustomer.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "Column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column02", "Column01", "Column02", gx_recivefromcustomer.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
                if (e.Column.Key=="Column03")
                {
                    if (dtCash.Rows[0][0].ToString() == "True")
                    {
                        txt_CashSystem.Value = Convert.ToDecimal(txt_CashSale.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                    }
                    else if (dtCash.Rows[0][0].ToString() == "False")
                    {
                        txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                    }
                }
            
            }
            catch { }



        }

        private void gx_recivefromcustomer_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gx_recivefromcustomer_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column02");
            }
            catch { }
            try
            {
                if (e.Column.Key == "Column02")
                    gx_recivefromcustomer.SetValue("Person", gx_recivefromcustomer.CurrentRow.Cells["Column02"].Text);
            }
            catch
            {
            }
            try
            {
                if (e.Column.Key == "Column03")
                {
                    //gx_recivefromcustomer.UpdateData();
                    decimal customerrecive = Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum));

                    if (dtCash.Rows[0][0].ToString() == "True")
                    {
                        txt_CashSystem.Value = Convert.ToDecimal(txt_CashSale.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                    }
                    else if (dtCash.Rows[0][0].ToString() == "False")
                    {
                        txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                    }

                    calcut();


                }
            }
            catch
            {
            }
        }

        private void gx_recivefrombank_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gx_recivefrombank.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "Column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column02", "Column02", "Column02", gx_recivefrombank.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
                if (e.Column.Key == "Column03" )
                {

                    txt_RemainBank.Value = Convert.ToDecimal(txt_Cart.Value) + Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
                }
            }


            catch { }
        }

        private void gx_losefromcash_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gx_losefromcash.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "Column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column02", "ACC_Code", "ACC_Name", gx_losefromcash.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }

            try
            {
                if (e.Column.Key == "Column03")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column03", "Column01", "Column02", gx_losefromcash.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

                
            }
            catch { }
            try
            {
                if (e.Column.Key == "Column04")

                    if (dtCash.Rows[0][0].ToString() == "True")
                    {
                        txt_CashSystem.Value = Convert.ToDecimal(txt_CashSale.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                    }
                    else if (dtCash.Rows[0][0].ToString() == "False")
                    {
                        txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                    }




            }
            catch { }
           
        }

        private void gx_losefromcash_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column02");
            }
            catch { }
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column03");
            }
            catch { }
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column07");
            }
            catch { }
            try
            {
                if (e.Column.Key == "Column02")
                    gx_losefromcash.SetValue("ACC_Name", gx_losefromcash.CurrentRow.Cells["Column02"].Text);
            }
            catch
            {
            }

            try
            {
                if (e.Column.Key == "Column03")
                    gx_losefromcash.SetValue("Person", gx_losefromcash.CurrentRow.Cells["Column03"].Text);
            }
            catch
            {
            }
            try
            {
                

                if (dtCash.Rows[0][0].ToString() == "True")
                {
                    txt_CashSystem.Value = Convert.ToDecimal(txt_CashSale.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                }
                else if (dtCash.Rows[0][0].ToString() == "False")
                {
                    txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

                }
            }
            catch { }

            try
            {
                if (e.Column.Key == "Column04" )
                {
                    calcut();
                }
            }

            catch
            {
            }


           
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                if (faDatePickerStrip1.FADatePicker.Text != string.Empty)
                {


                    
                    string Docclose = clDoc.ExScalar(ConBase.ConnectionString, @"select Column02 from Table_030_Setting where ColumnId=64");
                     isadmin = ChA.IsAdmin(Class_BasicOperation._UserName, Class_BasicOperation._OrgCode.ToString(), Class_BasicOperation._Year);

                    dtCash = clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT Column29 FROM Table_295_StoreInfo where Columnid = " + mlt_Stor.Value + "");
                    if (dtCash.Rows[0]["Column29"].ToString() == "")
                    {
                        Class_BasicOperation.ShowMsg("", "لطفا برای محاسبه فروش نقدی، از تنظیمات فروشگاه اقدام نمایید", "Warning");
                        return;
                    }
                    //پر کردن گرید فروش
                    gx_factors.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT        columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, 
                         column15, column16, column17, column18, column19, column20, column21, column23, column24, column25, column27, Column37, Column38, Column39, Column40, 
                         Column42, Column43, Column44, Column45, Column49, Column50, Column51, Column53, Column54, Column55, Column56, Column57, Column58, Column59, 
                         Column61, Column62, Column63, Column60, Column64, column22, column26, Column28, Column29, Column30, Column31, Column32, Column33, Column34, 
                         Column35, Column36, Column41, Column46, Column47, Column48, Column52,Column28 - Column29 -Column30 - Column31 + Column32- Column33 as FinalPrice
FROM            Table_010_SaleFactor
WHERE      column02<='" + faDatePickerStrip1.FADatePicker.Text + @"'     and column53=0      and column17=0    AND  Column68 in (" + mlt_Stor.Value + ") and ('" + isadmin.ToString() + "'='True' or Column13='" + Class_BasicOperation._UserName + "') ");


                    Projectreturn = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column05 from Table_295_StoreInfo where ColumnId="+ mlt_Stor.Value + "),0)");
                    //پر کردن گرید مرجوعی 
                    gx_factorReturn.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, @" select *, Column18 + Column19 - Column20  as FinalPrice
FROM            Table_018_MarjooiSale
WHERE      column02<='" + faDatePickerStrip1.FADatePicker.Text + @"'     and column27=0        AND  Column30 in (" + Projectreturn + ") and ('" + isadmin.ToString() + "'='True' or Column13='" + Class_BasicOperation._UserName + "')    ");

                    this.Update();





                    if (gx_factors.RowCount == 0 && gx_factorReturn.RowCount == 0)
                    {
                        Class_BasicOperation.ShowMsg("", "فاکتوری برای بستن صندوق وجود ندارد", "Stop");
                        bt_ExportDoc.Enabled = false;
                        return;
                    }
                    if (Docclose=="" || Docclose=="0")
                    {
                        Class_BasicOperation.ShowMsg("", " لطفا برای بستن صندوق از تنظیمات فیلد سر فصل صندوق را مشخص کنید  ", "Stop");
                        return;
                    }


                   



                    dataSet6.EnforceConstraints = false;
                    this.table_96_CloseCashTableAdapter.FillByID(this.dataSet6.Table_96_CloseCash, -1);
                    this.table_99_LosesFromCashTableAdapter.FillByHeaderID(this.dataSet6.Table_99_LosesFromCash, -1);
                    this.table_98_ReceivedFromBankTableAdapter.FillByHeaderID(this.dataSet6.Table_98_ReceivedFromBank, -1);
                    this.table_97_ReceivedFromCustomersTableAdapter.FillByHeaderID(this.dataSet6.Table_97_ReceivedFromCustomers, -1);
                    dataSet6.EnforceConstraints = true;
                    mlt_Stor.Value = stor;

                    table_96_CloseCashBindingSource.AddNew();
                    DataRowView Row = (DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current;
                    Row["Column01"] = faDatePickerStrip1.FADatePicker.Text;
                    Row["Column17"] = Class_BasicOperation._UserName;
                    Row["Column18"] = Class_BasicOperation.ServerDate();
                    Row["Column26"] = mlt_Stor.Value;

                    DataTable InfoTable = clDoc.ReturnTable(Properties.Settings.Default.SALE, @"SELECT (
                                                                                           SELECT ISNULL(
                                                                                                      SUM(
                                                                                                          Column28 - Column29 -Column30 - Column31 +
                                                                                                          Column32 - Column33
                                                                                                      ),
                                                                                                      0
                                                                                                  ) AS SalePrice
                                                                                           FROM   Table_010_SaleFactor tsf
                                                                                           WHERE  column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                  AND column53 = 0
                                                                                                  AND column17 = 0 AND  Column68 in (" + mlt_Stor.Value + ") and ('" + isadmin.ToString() + "'='True' or Column13='" + Class_BasicOperation._UserName + @"')
                                                                                       ) AS txt_FactorsSum,
                                                                                       (
                                                                                           SELECT ISNULL(SUM(tms.Column18 + tms.Column19 -tms.Column20), 0) AS 
                                                                                                  ReturnPrice
                                                                                           FROM   Table_018_MarjooiSale tms
                                                                                           WHERE  tms.column02 <= 
                                                                                                  '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                  AND tms.Column27 = 0 AND Column30 in (" + Projectreturn + ") and ('" + isadmin.ToString() + "'='True' or Column13='" + Class_BasicOperation._UserName + @"')
                                                                                       ) AS txt_ReturnFactorsSum,
                                                                                       (
                                                                                           SELECT ISNULL(
                                                                                                      SUM(
                                                                                                          Column28 - Column29 -Column30 - Column31 +
                                                                                                          Column32 - Column33
                                                                                                      ),
                                                                                                      0
                                                                                                  ) -(
                                                                                                      SELECT ISNULL(SUM(tms.Column18 + tms.Column19 -tms.Column20), 0)
                                                                                                      FROM   Table_018_MarjooiSale tms
                                                                                                      WHERE  tms.column02 <= 
                                                                                                             '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                             AND tms.Column27 = 0  AND Column30 in (" + Projectreturn + ") and ('" + isadmin.ToString() + "'='True' or Column13='" + Class_BasicOperation._UserName + @"')
                                                                                                  ) AS NetPrice
                                                                                           FROM   Table_010_SaleFactor tsf
                                                                                           WHERE  column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                  AND column53 = 0
                                                                                                  AND column17 = 0  AND  Column68 in (" + mlt_Stor.Value + ") and ('" + isadmin.ToString() + "'='True' or Column13='" + Class_BasicOperation._UserName + @"')
                                                                                       ) AS txt_NetAmount,
                                                                                       (
                                                                                           SELECT ISNULL(
                                                                                                      SUM(
                                                                                                          Column28 - Column29 -Column30 - Column31 +
                                                                                                          Column32 - Column33
                                                                                                      ),
                                                                                                      0
                                                                                                  )  AS CashSale
                                                                                           FROM   Table_010_SaleFactor tsf
                                                                                                  JOIN " + ConBase.Database + @".dbo.Table_002_SalesTypes tst
                                                                                                       ON  tst.columnid = tsf.Column36
                                                                                           WHERE  tsf.column02 <= 
                                                                                                  '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                  AND tsf.column53 = 0
                                                                                                  AND tsf.column17 = 0
                                                                                                  AND tst.Column21 = 1   AND  tsf.Column68 in (" + mlt_Stor.Value + ") and ('" + isadmin.ToString() + "'='True' or tsf.Column13='" + Class_BasicOperation._UserName + @"')
                                                                                       ) AS txt_CashSale,
                                                                                       (
                                                                                     Select isnull((SELECT        SUM(dbo.Table_010_SaleFactor.Column28) AS NetTotal
                                                                                        FROM            dbo.Table_010_SaleFactor 
                                                                                        LEFT OUTER JOIN
                                                                                                            " + ConBase.Database + @".dbo.Table_002_SalesTypes ON dbo.Table_010_SaleFactor.Column36 = " + ConBase.Database + @".dbo.Table_002_SalesTypes.columnid
  		                                                                                        WHERE         (dbo.Table_010_SaleFactor.column19 = 0) AND (dbo.Table_010_SaleFactor.Column53 = 0) AND (dbo.Table_010_SaleFactor.column17 = 0)  AND  dbo.Table_010_SaleFactor.Column44 in (" + mlt_Stor.Value + ") and ('" + isadmin.ToString() + "'='True' or dbo.Table_010_SaleFactor.Column13='" + Class_BasicOperation._UserName + @"')
                                                                                        AND ( " + ConBase.Database + @".dbo.Table_002_SalesTypes.Column21 = 0 ) AND  (dbo.Table_010_SaleFactor.column02<='" + faDatePickerStrip1.FADatePicker.Text + @"') AND  " + ConBase.Database + @".dbo.Table_002_SalesTypes.column21 =0),0) AS txt_EtebariSale
                                                                                    
                                                                                        ) AS txt_EtebariSale,
                                                                                        (
                                                                                           SELECT ISNULL(SUM(ISNULL(tsd.Column11, 0) -ISNULL(tsd.Column12, 0)), 0) AS txt_CashAcountRemain
                                                                                          FROM   " + ConAcnt.Database + @".dbo.Table_065_SanadDetail tsd
                                                                                                   JOIN " + ConAcnt.Database + @".dbo.Table_060_SanadHead tsh
                                                                                                        ON  tsh.ColumnId = tsd.Column00
                                                                                        WHERE  tsd.Column01 ='" + Docclose + "'  AND tsh.Column01 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'   AND  tsd.Column09 in (" + mlt_Stor.Value + ") and ('" + isadmin.ToString() + "'='True' or tsd.Column18='" + Class_BasicOperation._UserName + @"') 
                                                                                       ) AS txt_CashAcountRemain,

(
                                                                                       	SELECT        SUM(Column46) AS Naghd
																							FROM            dbo.Table_010_SaleFactor
																							WHERE     (Column45=1) AND (Column17=0)  AND (Column53 = 0) and Column02<= '" + faDatePickerStrip1.FADatePicker.Text + @"'   AND  Column68 in (" + mlt_Stor.Value + ") and ('" + isadmin.ToString() + "'='True' or Column13='" + Class_BasicOperation._UserName + @"') 
                                                                                       
                                                                                       ) AS txt_Cash,
                                                                                       (
                                                                                       	SELECT        SUM(Column47) AS Pos
																							FROM            dbo.Table_010_SaleFactor
																							WHERE      (Column45=1)  AND (Column17=0) AND (Column53 = 0) AND Column02<= '" + faDatePickerStrip1.FADatePicker.Text + @"'  AND  Column68 in (" + mlt_Stor.Value + ") and ('" + isadmin.ToString() + "'='True' or Column13='" + Class_BasicOperation._UserName + @"')
                                                                                       
                                                                                       ) AS txt_Cart,
                                                                                       (
                                                                                       	SELECT        SUM(Column52) AS cheek
																							FROM            dbo.Table_010_SaleFactor
																							WHERE       (Column45=1)  AND (Column17=0)  AND (Column53 = 0) and Column02<= '" + faDatePickerStrip1.FADatePicker.Text + @"'   AND  Column68 in (" + mlt_Stor.Value + ") and ('" + isadmin.ToString() + "'='True' or Column13='" + Class_BasicOperation._UserName + @"')
                                                                                       
                                                                                       ) AS txt_Chek,
                                                                                         (
                                                                                       	SELECT        SUM(Column54) AS Other
																							FROM            dbo.Table_010_SaleFactor
																							WHERE       (Column45=1)  AND (Column17=0)  AND (Column53 = 0) and Column02<= '" + faDatePickerStrip1.FADatePicker.Text + @"'    AND  Column68 in (" + mlt_Stor.Value + ") and ('" + isadmin.ToString() + "'='True' or Column13='" + Class_BasicOperation._UserName + @"') 
                                                                                       
                                                                                       ) AS txt_Other ");






                    ReturnId = "";
                    foreach (Janus.Windows.GridEX.GridEXRow item in gx_factorReturn.GetRows())
                    {
                        ReturnId += item.Cells["ColumnId"].Value.ToString() + ",";
                    }


                    saleIds = "";

                    foreach (Janus.Windows.GridEX.GridEXRow item in gx_factors.GetRows())
                    {
                        saleIds += item.Cells["ColumnId"].Value.ToString() + ",";
                    }


                    if (InfoTable.Rows.Count > 0)
                    {
                        txt_FactorsSum.Value = InfoTable.Rows[0]["txt_FactorsSum"];
                        txt_ReturnFactorsSum.Value = InfoTable.Rows[0]["txt_ReturnFactorsSum"];
                        txt_NetAmount.Value = InfoTable.Rows[0]["txt_NetAmount"];
                        txt_CashSale.Value = InfoTable.Rows[0]["txt_CashSale"];
                        txt_EtebariSale.Value = InfoTable.Rows[0]["txt_EtebariSale"];
                        txt_CashAcountRemain.Value = InfoTable.Rows[0]["txt_CashAcountRemain"];
                        txt_Cash.Value = InfoTable.Rows[0]["txt_Cash"];
                        txt_Cart.Value = InfoTable.Rows[0]["txt_Cart"];
                        txt_Chek.Value = InfoTable.Rows[0]["txt_Chek"];
                        txt_Other.Value = InfoTable.Rows[0]["txt_Other"];
                        txt_Sum.Value =Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(txt_Cart.Value) + Convert.ToDecimal(txt_Chek.Value) + Convert.ToDecimal(txt_Other.Value);
                        txt_Otherness.Value = Convert.ToDecimal(txt_NetAmount.Value) - Convert.ToDecimal(txt_Sum.Value);

                        if (dtCash.Rows[0][0].ToString() == "True")
                        {
                            txt_CashSystem.Value = Convert.ToDecimal(txt_CashSale.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                            txt_Cash.Value = 0;
                            txt_Cart.Value = 0;
                            txt_Chek.Value = 0;
                            txt_Other.Value = 0;
                            txt_Sum.Value = 0;
                            txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value) - Convert.ToDecimal(txt_RemainBank.Value);

                        }
                        else if (dtCash.Rows[0][0].ToString() == "False")
                        {
                            txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                            txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value);

                        }

                        //txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                        txt_RemainBank.Value = Convert.ToDecimal(txt_Cart.Value) + Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
                        txt_CashAcountDifference.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashAcountRemain.Value)+ Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                    }


                    gx_recivefrombank.Enabled = true;
                    gx_recivefromcustomer.Enabled = true;
                    gx_losefromcash.Enabled = true;
                    bt_ExportDoc.Enabled = true;
                    //bt_Search.Enabled = false;
                    table_96_CloseCashBindingSource.EndEdit();



                    #region چک کردن تکس های الارمی
                    if (txt_CashDeductionAddition.Text == "0" || txt_CashDeductionAddition.Text == "")
                    {
                        txt_CashDeductionAddition.BackColor = Color.White;
                    }
                    if (txt_CashAcountDifference.Text == "0" || txt_CashAcountDifference.Text == "")
                    {
                        txt_CashAcountDifference.BackColor = Color.White;
                    }

                    if (txt_CashAcountDifference.Text != "0")
                        txt_CashAcountDifference.BackColor = Color.LightPink;

                    if (txt_CashDeductionAddition.Text != "0")
                    {
                        txt_CashDeductionAddition.BackColor = Color.LightPink;
                        //MessageBox.Show("مغایرت در کسر و اضافه صندوق ! ");
                        this.Cursor = Cursors.Default;
                        //return;

                    }




                    #endregion



                    this.Cursor = Cursors.Default;
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gx_recivefromcustomer_Enter(object sender, EventArgs e)
        {
            try
            {
                table_96_CloseCashBindingSource.EndEdit();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }






        }

        private void gx_recivefromcustomer_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                decimal customerrecive = Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum));
                Janus.Windows.GridEX.GridEXRow M = gx_recivefromcustomer.GetTotalRow();
                Janus.Windows.GridEX.GridEXRow N = gx_losefromcash.GetTotalRow();
                txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(M.Cells["Column03"].Value) - Convert.ToDecimal(N.Cells["Column04"].Value);
                //txt_CashStockSum.Value = Convert.ToDecimal(txt_CashSale.Value) + customerrecive;
            }
            catch
            {
            }
        }

        private void gx_recivefromcustomer_AddingRecord(object sender, CancelEventArgs e)
        {
            gx_recivefromcustomer.SetValue("Column04", Class_BasicOperation._UserName);
            gx_recivefromcustomer.SetValue("Column05", Class_BasicOperation.ServerDate());
          


        }

        private void gx_recivefromcustomer_RegionChanged(object sender, EventArgs e)
        {

        }

        private void gx_recivefrombank_AddingRecord(object sender, CancelEventArgs e)
        {
            gx_recivefrombank.SetValue("Column04", Class_BasicOperation._UserName);
            gx_recivefrombank.SetValue("Column05", Class_BasicOperation.ServerDate());
            calcut();

        }

        private void gx_recivefrombank_RowCountChanged(object sender, EventArgs e)
        {
            calcut();
        }

        private void calcut()
        {
            try
            {
                if (gx_recivefrombank.CurrentRow.RowIndex != -1)
                {
                    gx_recivefrombank.UpdateData();
                }
                if (gx_losefromcash.CurrentRow.RowIndex != -1)
                    gx_losefromcash.UpdateData();
                txt_RemainBank.Value = Convert.ToDecimal(txt_Cart.Value) + Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));


                if (dtCash.Rows[0][0].ToString() == "True")
                {
                    txt_CashSystem.Value = Convert.ToDecimal(txt_CashSale.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                    txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value) - Convert.ToDecimal(txt_RemainBank.Value);

                }
                else if (dtCash.Rows[0][0].ToString() == "False")
                {
                    txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                    txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value);
                }

                decimal customerrecive = Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
                decimal losefromcash = Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                decimal temp = 0;
                temp = (Convert.ToDecimal(txt_CashCashStock.Value) + customerrecive) + losefromcash;
                //txt_CashAcountDifference.Value = Convert.ToDecimal(txt_CashAcountRemain.Value) - customerrecive - losefromcash - Convert.ToDecimal(txt_CashDeductionAddition.Value);
                txt_CashAcountDifference.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashAcountRemain.Value)+ Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
              
            }
            catch
            {
            }
        }

        private void gx_recivefrombank_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column02");
            }
            catch { }

           
            try
            {
                if (e.Column.Key == "Column02")
                    gx_recivefrombank.SetValue("Bank", gx_recivefrombank.CurrentRow.Cells["Column02"].Text);
            }
            catch
            {
            }

            try
            {
                if (e.Column.Key == "Column03" ||  e.Column.Key == "Column06")
                {
                    //gx_recivefrombank.UpdateData();
                    txt_RemainBank.Value = Convert.ToDecimal(txt_Cart.Value) + Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
                    txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value) - Convert.ToDecimal(txt_RemainBank.Value);

                }
                else if (e.Column.Key == "Column03")
                {
                    //gx_recivefrombank.UpdateData();

                    txt_RemainBank.Value = Convert.ToDecimal(txt_Cart.Value) + Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));
                    txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value) ;


                }
            }
            catch
            {
            }


            try
            {

                if (e.Column.Key == "Column03" )
                {
                    calcut();
                }

            }

            catch { }


        }

        private void gx_losefromcash_RowCountChanged(object sender, EventArgs e)
        {
            calcut();

        }

        private void txt_CashCashStock_TextChanged(object sender, EventArgs e)
        {
            calcut();
            if (txt_CashDeductionAddition.Text == "0" || txt_CashDeductionAddition.Text == "")
            {
                txt_CashDeductionAddition.BackColor = Color.White;
            }
            if (txt_CashAcountDifference.Text == "0" || txt_CashAcountDifference.Text == "")
            {
                txt_CashAcountDifference.BackColor = Color.White;
            }

            if (txt_CashAcountDifference.Text != "0")
                txt_CashAcountDifference.BackColor = Color.LightPink;
            if (txt_CashDeductionAddition.Text != "0")
            {
                txt_CashDeductionAddition.BackColor = Color.LightPink;
               

            }


        }

        private void gx_losefromcash_AddingRecord(object sender, CancelEventArgs e)
        {
            gx_losefromcash.SetValue("Column05", Class_BasicOperation._UserName);
            gx_losefromcash.SetValue("Column06", Class_BasicOperation.ServerDate());
            if (dtCash.Rows[0][0].ToString() == "True")
            {
                txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value) - Convert.ToDecimal(txt_RemainBank.Value);



            }
            else if (dtCash.Rows[0][0].ToString() == "False")
            {
                txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value);


            }
            calcut();

        }

        private void txt_CashStockSum_TextChanged(object sender, EventArgs e)
        {
            calcut();

        }

        private void txt_CashAcountRemain_TextChanged(object sender, EventArgs e)
        {
            calcut();

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {

                string numsanad = clDoc.ExScalar(ConSale.ConnectionString, @"  SELECT        " + ConAcnt.Database + @".dbo.Table_060_SanadHead.Column00
FROM            dbo.Table_96_CloseCash INNER JOIN
                         " + ConAcnt.Database + @".dbo.Table_060_SanadHead ON dbo.Table_96_CloseCash.Column16 = " + ConAcnt.Database + @".dbo.Table_060_SanadHead.ColumnId
WHERE        (dbo.Table_96_CloseCash.ColumnId = " + txt_Id.Text + ")");
                _05_Sale.Frm_030_CloseCashPrint frm =
                     new Frm_030_CloseCashPrint(faDatePickerStrip1.FADatePicker.Text, ((PSHOP._05_Sale.DataSet6)(table_96_CloseCashBindingSource.DataSource)).Table_96_CloseCash,
                         ((PSHOP._05_Sale.DataSet6)(table_96_CloseCashBindingSource.DataSource)).Table_97_ReceivedFromCustomers,
                         ((PSHOP._05_Sale.DataSet6)(table_96_CloseCashBindingSource.DataSource)).Table_98_ReceivedFromBank,
                         ((PSHOP._05_Sale.DataSet6)(table_96_CloseCashBindingSource.DataSource)).Table_99_LosesFromCash, numsanad);

                frm.ShowDialog();
            }
            catch (Exception es)
            {

                this.Cursor = Cursors.Default;
                Class_BasicOperation.CheckExceptionType(es, this.Name);
            }


        }

        private void table_96_CloseCashBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (table_96_CloseCashBindingSource.Count > 0)
            {
                DataRowView Row = (DataRowView)this.table_96_CloseCashBindingSource.CurrencyManager.Current;

                if (Convert.ToInt32(Row["ColumnId"]) > 0)
                    bt_Print.Enabled = true;
                else
                    bt_Print.Enabled = false;

            }
            else
                bt_Print.Enabled = false;

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                _05_Sale.Frm_003_MojoodiMaghtai_NegativeDate frm2 = new Frm_003_MojoodiMaghtai_NegativeDate();
                frm2.ShowDialog();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
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

        private void faDatePickerStrip1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip1.FADatePicker.HideDropDown();
                    faDatePickerStrip1.FADatePicker.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void txt_Cart_Click(object sender, EventArgs e)
        {


        }

        private void panelEx1_Click(object sender, EventArgs e)
        {

        }

        private void txt_CashRealStock_Click(object sender, EventArgs e)
        {

        }

        private void txt_RemainBank_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void gx_losefromcash_FormattingRow(object sender, RowLoadEventArgs e)
        {

        }

        //private void fill_IDToolStripButton_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.table_018_MarjooiSaleTableAdapter.Fill_ID(this.dataSet_Sale.Table_018_MarjooiSale, ((int)(System.Convert.ChangeType(headerIDToolStripTextBox.Text, typeof(int)))));
        //    }
        //    catch (System.Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.Message);
        //    }

        //}

        private void gx_factorReturn_FormattingRow(object sender, RowLoadEventArgs e)
        {

        }

        private void fill_IDToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void txt_CashSystem_ValueChanged(object sender, EventArgs e)
        {

            if (dtCash.Rows[0][0].ToString() == "True")
            {
                txt_CashSystem.Value = Convert.ToDecimal(txt_CashSale.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value) - Convert.ToDecimal(txt_RemainBank.Value);

            }
            else if (dtCash.Rows[0][0].ToString() == "False")
            {
                txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value);

            }


        }

        private void txt_RemainBank_ValueChanged(object sender, EventArgs e)
        {
            txt_RemainBank.Value = Convert.ToDecimal(txt_Cart.Value) + Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));

        }

        private void txt_CashAcountDifference_ValueChanged(object sender, EventArgs e)
        {
            txt_CashAcountDifference.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashAcountRemain.Value)+ Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
          

        }

        private void txt_CashDeductionAddition_ValueChanged(object sender, EventArgs e)
        {
            if (dtCash.Rows[0][0].ToString() == "True")
            {
                txt_CashSystem.Value = Convert.ToDecimal(txt_CashSale.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value) - Convert.ToDecimal(txt_RemainBank.Value);

            }
            else if (dtCash.Rows[0][0].ToString() == "False")
            {
                txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));
                txt_CashDeductionAddition.Value = Convert.ToDecimal(txt_CashSystem.Value) - Convert.ToDecimal(txt_CashCashStock.Value);

            }


        }

        private void txt_CashDeductionAddition_TextChanged(object sender, EventArgs e)
        {
            if (txt_CashDeductionAddition.Text == "0" || txt_CashDeductionAddition.Text == "")
            {
                txt_CashDeductionAddition.BackColor = Color.White;
            }
            if (txt_CashAcountDifference.Text == "0" || txt_CashAcountDifference.Text == "")
            {
                txt_CashAcountDifference.BackColor = Color.White;
            }

            if (txt_CashAcountDifference.Text != "0")
                txt_CashAcountDifference.BackColor = Color.LightPink;

            if (txt_CashDeductionAddition.Text != "0")
            {
                txt_CashDeductionAddition.BackColor = Color.LightPink;
            }

        }

        private void uiPanel5Container_Click(object sender, EventArgs e)
        {

        }

        private void txt_CashSystem_TextChanged(object sender, EventArgs e)
        {
            if (dtCash.Rows[0][0].ToString() == "True")
            {
                txt_CashSystem.Value = Convert.ToDecimal(txt_CashSale.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

            }
            else if (dtCash.Rows[0][0].ToString() == "False")
            {
                txt_CashSystem.Value = Convert.ToDecimal(txt_Cash.Value) + Convert.ToDecimal(gx_recivefromcustomer.GetTotal(gx_recivefromcustomer.RootTable.Columns["Column03"], AggregateFunction.Sum)) - Convert.ToDecimal(gx_losefromcash.GetTotal(gx_losefromcash.RootTable.Columns["Column04"], AggregateFunction.Sum));

            }
        }

        private void txt_RemainBank_TextChanged(object sender, EventArgs e)
        {
            txt_RemainBank.Value = Convert.ToDecimal(txt_Cart.Value) + Convert.ToDecimal(gx_recivefrombank.GetTotal(gx_recivefrombank.RootTable.Columns["Column03"], AggregateFunction.Sum));

        }


    }
}
