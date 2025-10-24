using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Janus.Windows.GridEX;
using System.Net.Sockets;
using Newtonsoft.Json;
using PosInterface;
using SSP1126.PcPos.BaseClasses;
using SSP1126.PcPos.Infrastructure;
namespace PSHOP._05_Sale
{
    public partial class Frm_028_Settelment : Form
    {
        double Amount = 0;
        int Factroid;
        Int32 _Num, _Person;
        string _datecheek;
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        DataSet DS = new DataSet();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection Conbase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        DataTable setting = new DataTable();
        PosResult posResult = new PosResult();

        bool Isadmin = false;
        DataTable storefactor = new DataTable();

        public Frm_028_Settelment()
        {
            InitializeComponent();
        }
        public Frm_028_Settelment(int factroid, Int32 Num, Int32 Person, string datecheek)
        {


            InitializeComponent();
            _Num = Num;
            _Person = Person;
            Factroid = factroid;
            _datecheek = datecheek;

            try
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand("SELECT isnull( isnull(Column28,0)+isnull(Column32,0)-isnull(Column33,0),0) FROM   Table_010_SaleFactor   WHERE  columnid=" + Factroid + "", Con);
                    Amount = Convert.ToDouble(Comm.ExecuteScalar());
                    txt_EndPrice.Value = Amount;

                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            //if (Convert.ToDouble(txt_cash.Value) > 0 || Convert.ToDouble(txtcart.Value) > 0 || Convert.ToDecimal(txtother.Value) > 0)
            //    btn_save.Enabled = false;

            mande();
            column25CheckBox.Checked = true;
            CHK_Delivary.Checked = true;
        }

        private void Frm_028_Settelment_Load(object sender, EventArgs e)
        {
            try
            {
                cmb_bank.DataSource = clDoc.ReturnTable(Properties.Settings.Default.BANK, "Select ColumnId,Column01,Column02,Column35 from Table_020_BankCashAccInfo where Column01=0");
                column25CheckBox_CheckedChanged(null, null);
                //mlt_ReciptCheek.DataSource = clDoc.ReturnTable(ConBank.ConnectionString, @"SELECT    ColumnId   from Table_035_ReceiptCheques");
                //txt_cash.Value = Amount;
                mlt_Bank.DataSource = clDoc.ReturnTable(ConBank.ConnectionString, @"select * from Table_010_BankNames ");
                SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_030_Setting", Conbase);
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.BankName.ToString()))
                    Bank_Name.SelectedIndex = Convert.ToInt32(Properties.Settings.Default.BankName.ToString());

                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.Hesab_Bank.ToString()))
                    cmb_bank.SelectedIndex = Convert.ToInt32(Properties.Settings.Default.Hesab_Bank.ToString());
                //cmb_bank.Value=Properties.Settings.Default.Hesab_Bank;
                setting = new DataTable();
                Adapter.Fill(setting);

            }
            catch { }
        }
        SqlParameter DocNum;
        string sanad = "0";
        private PcPosFactory _PcPosFactory;
        private AsyncType _asyncType;

        private int _PosClient_CardSwiped;
        private int _PosClient_PosResultReceived;
        private PcPosFactory _mediaType;

        private void bt_Save_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            dt = clDoc.ReturnTable(Conbase.ConnectionString, @"select * from  " + Conbase.Database + ".dbo. Table_105_SystemTransactionInfo");

            if (Convert.ToDouble(txtcart.Value) < 0 || Convert.ToDouble(txt_cash.Value) < 0 || Convert.ToDouble(this.txtcheck.Value) < 0 || Convert.ToDouble(txt_other.Value) < 0)
            {
                Class_BasicOperation.ShowMsg("", "یکی از مقادیر منفی است", "Information");

                return;
            }
            if (CHK_Delivary.Checked && string.IsNullOrWhiteSpace(txt_DelivaryDate.Text))
            {
                Class_BasicOperation.ShowMsg("", "تاریخ تحویل کالا را وارد کنید", "Information");


                return;
            }
            if (Convert.ToInt32(txtcart.Value) > 0 && (cmb_bank.Value == null || cmb_bank.Value.ToString() == string.Empty))
            {
                Class_BasicOperation.ShowMsg("", "حساب بانکی را انتخاب کنید", "Information");
                return;

            }
            if (chk_cart.Checked)
            {

            if (Convert.ToInt32(txtcart.Value) == 0 && (cmb_bank.Value != null && cmb_bank.Value.ToString() != string.Empty))
            {
                Class_BasicOperation.ShowMsg("", "مبلغ کارت را وارد کنید", "Information");
                return;
            }
            }


            if (Convert.ToDouble(txtcart.Value) == 0 && Convert.ToDouble(txt_cash.Value) == 0 && Convert.ToDouble(this.txtcheck.Value) == 0 && Convert.ToDouble(this.txt_other.Value) == 0)
            {

                if (MessageBox.Show("هیچ مقداری برای تسویه انتخاب نشده است، آیا مایل به تسویه فاکتور هستید؟", "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    clDoc.ReturnTable(ConSale.ConnectionString, " UPDATE Table_010_SaleFactor SET Column45=1 where columnid=" + Factroid + "");
                }
                else
                {
                    this.Close();
                    return;
                }

            }

            bool cash = false;
            try
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(tst.Column21, 0) AS cash
                                                           FROM   Table_010_SaleFactor
                                                                  LEFT  JOIN " + Conbase.Database + @".dbo.Table_002_SalesTypes tst
                                                                       ON  tst.columnid = Table_010_SaleFactor.Column36
                                                           WHERE  Table_010_SaleFactor.columnid = " + Factroid + "", Con);
                    cash = Convert.ToBoolean(Comm.ExecuteScalar());


                }
            }
            catch
            {
            }
            //if (cash && (Convert.ToDouble(txtcheck.Value) > 0 ||  Convert.ToDouble(this.txt_other.Value) > 0))
            //{
            //    Class_BasicOperation.ShowMsg("", "نوع فروش نقدی است امکان ثبت چک و اعتباری و بن وجود ندارد", "Information");

            //    return;
            //}


            if (Convert.ToDouble(txt_cash.Value) + Convert.ToDouble(txtcart.Value) + Convert.ToDouble(this.txtcheck.Value) + Convert.ToDouble(this.txt_other.Value) == Convert.ToDouble(txt_EndPrice.Value))
            {
                try
                {
                    SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
                    Class_UserScope UserScope = new Class_UserScope();


                    string Commandtext = "";
                    Commandtext += @"DECLARE @DocId INT DECLARE @ProjectID SMALLINT
                    SET @DocId = (select Column10 FROM Table_010_SaleFactor  WHERE ColumnId=" + Factroid + @")
                    SET @ProjectID = (select Column44 FROM Table_010_SaleFactor  WHERE ColumnId=" + Factroid + @")
                    
                    ";

                    if (Convert.ToDouble(txt_cash.Value) > 0)
                    {

                        string[] _ACC_Info = clDoc.ACC_Info(dt.Select("Column00=89")[0]["Column07"].ToString());
                        string[] _ACCInfo = clDoc.ACC_Info(dt.Select("Column00=89")[0]["Column13"].ToString());

                        Commandtext += @"
                   

                        INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail  ([Column00]  ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06]         ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column09 )
                                                                              
                                                                              Values(@DocId,'" + dt.Select("Column00=89")[0]["Column07"].ToString() + "'," + Int16.Parse(_ACC_Info[0].ToString()) + ",'" + _ACC_Info[1].ToString() + "'," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[2].ToString()) ? "NULL" : "'" + _ACC_Info[2].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[3].ToString()) ? "NULL" : "'" + _ACC_Info[3].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[4].ToString()) ? "NULL" : "'" + _ACC_Info[4].ToString() + "'") + ",'"
                                                                                                   + "تسویه - نقد به شماره فاکتور " + _Num + "'," + txt_cash.Value + ",0,0,0,0,24," + Factroid + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,@ProjectID) ";
                        Commandtext += @"
                        INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail  ([Column00]  ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06],      [Column07]      ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column09 )
                                                                              
                                                                              Values(@DocId,'" + dt.Select("Column00=89")[0]["Column13"].ToString() + "'," + Int16.Parse(_ACC_Info[0].ToString()) + ",'" + _ACC_Info[1].ToString() + "'," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[2].ToString()) ? "NULL" : "'" + _ACC_Info[2].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[3].ToString()) ? "NULL" : "'" + _ACC_Info[3].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[4].ToString()) ? "NULL" : "'" + _ACC_Info[4].ToString() + "'") + "," + _Person + ",'" + " تسویه - نقد به شماره فاکتور " + _Num + "'," + "0," + txt_cash.Value + ",0,0,0,24," + Factroid + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,@ProjectID) ";



                        Commandtext += " UPDATE  " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column50=" + (CHK_Delivary.Checked ? "1" : "0") + ",Column51='" + txt_DelivaryDate.Text + "', Column46=" + Convert.ToDouble(txt_cash.Value) + "  , Column47=" + Convert.ToDouble(txtcart.Value) +
                          " ,Column52=" + Convert.ToDouble(this.txtcheck.Value) + ",Column54=" + Convert.ToDouble(this.txt_other.Value) + ",Column45=1,column15='" + Class_BasicOperation._UserName + "',column16=getdate(),Column49=" + (Convert.ToInt32(txtcart.Value) == 0 && (cmb_bank.Value == null || cmb_bank.Value.ToString() == string.Empty) ? "NULL" : cmb_bank.Value.ToString()) + "  where columnid=" + Factroid;

                    }

                    if (Convert.ToDouble(txtcart.Value) > 0)
                    {
                        string Bankcart = clDoc.ExScalar(ConBank.ConnectionString, @"select isnull((select Column12 from  " + ConBank.Database + ".dbo.Table_020_BankCashAccInfo where Column01=0 and Columnid=" + cmb_bank.Value + "),0)");
                        if (Bankcart == "")
                        {
                            MessageBox.Show("برای این حساب بانکی سرفصلی تعریف نشده است ");
                            return;
                        }
                        string[] _ACC_Info = clDoc.ACC_Info(Bankcart);
                        string[] _ACCInfo = clDoc.ACC_Info(dt.Select("Column00=91")[0]["Column13"].ToString());
                        Commandtext += @"
            

                        INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail  ([Column00]  ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06]           ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22] ,Column09)
                                                                              
                                                                              Values(@DocId,'" + Bankcart + "'," + Int16.Parse(_ACC_Info[0].ToString()) + ",'" + _ACC_Info[1].ToString() + "'," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[2].ToString()) ? "NULL" : "'" + _ACC_Info[2].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[3].ToString()) ? "NULL" : "'" + _ACC_Info[3].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[4].ToString()) ? "NULL" : "'" + _ACC_Info[4].ToString() + "'") + ",'" + "تسویه - کارت بانکی به شماره فاکتور  " + _Num + "'," + txtcart.Value + ", 0,0,0,0,24," + Factroid + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,@ProjectID) ";
                        Commandtext += @"
                        INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail  ([Column00]  ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06]      ,[Column07]      ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column09 )
                                                                              
                                                                              Values(@DocId,'" + dt.Select("Column00=91")[0]["Column13"].ToString() + "'," + Int16.Parse(_ACC_Info[0].ToString()) + ",'" + _ACC_Info[1].ToString() + "'," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[2].ToString()) ? "NULL" : "'" + _ACC_Info[2].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[3].ToString()) ? "NULL" : "'" + _ACC_Info[3].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[4].ToString()) ? "NULL" : "'" + _ACC_Info[4].ToString() + "'") + "," + _Person + ",'" + " تسویه - کارت بانکی به شماره فاکتور " + _Num + "'," + "0," + txtcart.Value + ",0,0,0,24," + Factroid + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,@ProjectID) ";


                        Commandtext += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column50=" + (CHK_Delivary.Checked ? "1" : "0") + ",Column51='" + txt_DelivaryDate.Text + "', Column46=" + Convert.ToDouble(txt_cash.Value) + "  , Column47=" + Convert.ToDouble(txtcart.Value) +
                       " ,Column52=" + Convert.ToDouble(this.txtcheck.Value) + ",Column54=" + Convert.ToDouble(this.txt_other.Value) + ",Column45=1,column15='" + Class_BasicOperation._UserName + "',column16=getdate(),Column49=" + (Convert.ToInt32(txtcart.Value) == 0 && (cmb_bank.Value == null || cmb_bank.Value.ToString() == string.Empty) ? "NULL" : cmb_bank.Value.ToString()) + "  where columnid=" + Factroid;

                    }

                    if (Convert.ToDouble(txt_other.Value) > 0)
                    {
                        string[] _ACC_Info = clDoc.ACC_Info(dt.Select("Column00=92")[0]["Column07"].ToString());
                        string[] _ACCInfo = clDoc.ACC_Info(dt.Select("Column00=92")[0]["Column13"].ToString());
                        Commandtext += @"
                    

                        INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail  ([Column00]  ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06]          ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column09 )
                                                                              
                                                                              Values(@DocId,'" + dt.Select("Column00=92")[0]["Column07"].ToString() + "'," + Int16.Parse(_ACC_Info[0].ToString()) + ",'" + _ACC_Info[1].ToString() + "',"
                                                                                                   + (string.IsNullOrEmpty(_ACC_Info[2].ToString()) ? "NULL" : "'" + _ACC_Info[2].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[3].ToString()) ? "NULL" : "'" + _ACC_Info[3].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[4].ToString()) ? "NULL" : "'" + _ACC_Info[4].ToString() + "'") + ",'" + "تسویه - سایر به شماره فاکتور " + _Num + "'," + txt_other.Value + ",0,0,0,0,24," + Factroid + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,@ProjectID) ";
                        Commandtext += @"
                        INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail  ([Column00]  ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06]      ,[Column07]          ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column09 )
                                                                              
                                                                              Values(@DocId,'" + dt.Select("Column00=92")[0]["Column13"].ToString() + "'," + Int16.Parse(_ACC_Info[0].ToString()) + ",'" + _ACC_Info[1].ToString() + "'," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[2].ToString()) ? "NULL" : "'" + _ACC_Info[2].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[3].ToString()) ? "NULL" : "'" + _ACC_Info[3].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[4].ToString()) ? "NULL" : "'" + _ACC_Info[4].ToString() + "'") + "," + _Person + ",'" + "تسویه - سایر به شماره فاکتور " + _Num + "',0," + txt_other.Value + ",0,0,0,24," + Factroid + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,@ProjectID) ";

                        Commandtext += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column50=" + (CHK_Delivary.Checked ? "1" : "0") + ",Column51='" + txt_DelivaryDate.Text + "', Column46=" + Convert.ToDouble(txt_cash.Value) + "  , Column47=" + Convert.ToDouble(txtcart.Value) +
                 " ,Column52=" + Convert.ToDouble(this.txtcheck.Value) + ",Column54=" + Convert.ToDouble(this.txt_other.Value) + ",Column45=1,column15='" + Class_BasicOperation._UserName + "',column16=getdate(),Column49=" + (Convert.ToInt32(txtcart.Value) == 0 && (cmb_bank.Value == null || cmb_bank.Value.ToString() == string.Empty) ? "NULL" : cmb_bank.Value.ToString()) + "  where columnid=" + Factroid;

                    }


                    if (Convert.ToDouble(txtcheck.Value) > 0)
                    {

                        if (txt_Date.Text == "" || mlt_Bank.Text == "" || mlt_Bank.Text.All(char.IsDigit) || txt_Branch.Text == "" || txt_NumberCheek.Text == "")
                        {
                            MessageBox.Show("لطفا اطلاعات مربوط به چک را تکمیل نمایید");
                            return;
                        }
                        if (setting.Rows[78]["Column02"].ToString() == "-1" || setting.Rows[79]["Column02"].ToString() == "-1")
                        {
                            MessageBox.Show("لطفا وضعیت چک و صندوق دریافت کننده را از قسمت اطلاعات پایه فرم تنظیمات تراکنش، تب تنظیمات فروشگاه تکمیل نمایید");
                            return;
                        }
                        string countstatuse = clDoc.ExScalar(ConBank.ConnectionString, @"   SELECT ISNULL(( SELECT        COUNT(ColumnId) AS COUNT
                                                                                                                FROM            dbo.Table_060_ChequeStatus
                                                                                                                GROUP BY ColumnId, Column02
                          
                                                                                      HAVING       ColumnId=" + setting.Rows[78]["Column02"] + "),0) AS Count");


                        string countrecbank = clDoc.ExScalar(ConBank.ConnectionString, @"SELECT        COUNT(ColumnId) AS COUNT
                                                                                        FROM            dbo.Table_020_BankCashAccInfo
                                                                                        GROUP BY ColumnId, Column02
                                                                                        HAVING        (ColumnId = " + setting.Rows[79]["Column02"] + ")");


                        if (int.Parse(countstatuse) <= 0 || int.Parse(countrecbank) <= 0)
                        {
                            MessageBox.Show("وضعیت چک یا بانک دریافت کننده نامعبر است لطفا از فرم تنظیمات بررسی کنید");
                            return;
                        }




                        DataTable dtcheek = clDoc.ReturnTable(ConBank.ConnectionString, @"SELECT        " + ConBank.Database + @".dbo.Table_060_ChequeStatus.Column03 AS Bed, " + ConBank.Database + @".dbo.Table_060_ChequeStatus.Column09 AS Bes
                         FROM       
                         " + ConBank.Database + @".dbo.Table_060_ChequeStatus where Columnid = " + setting.Rows[78]["Column02"] + "");


                        string ACC_Code = clDoc.ExScalar(ConBank.ConnectionString, @"SELECT        " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.Column12 AS AccCode
                                     FROM           
                       " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo WHERE  " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.ColumnId = " + setting.Rows[79]["Column02"] + "");



                        Commandtext += @"
                                declare @checkId int
                               
                            INSERT INTO  " + ConBank.Database + @".dbo.Table_035_ReceiptCheques
                                                 ([Column00]         ,[Column01]      ,[Column02]  ,[Column03]   ,[Column04] , [Column05]
                                                  ,[Column06]       ,[Column07]     ,[Column08]      ,[Column09] ,
                                                [Column42] , [Column43] , [Column44] ,[Column45] , [Column48])
                                                       
                            Values ((Select Isnull (Max(Column00),0)+1 as SugNum from " + ConBank.Database + @".dbo.Table_035_ReceiptCheques)," + setting.Rows[79]["Column02"] + ",'" + _datecheek + "','" + txt_NumberCheek.Text + "','" + txt_Date.Text + "',"
                            + txtcheck.Value + ",'چک دریافت شده بابت تسویه فاکتور به شماره " + _Num + "'," + _Person + ","
                            + mlt_Bank.Value + ",'" + txt_Branch.Text + "','" + Class_BasicOperation._UserName + "',getdate(), '" + Class_BasicOperation._UserName + "',getdate()," + setting.Rows[78]["Column02"] + " ) SET @checkId=SCOPE_IDENTITY(); ";





                        string[] _ACC_Info = clDoc.ACC_Info(dtcheek.Rows[0][0].ToString());
                        string[] _ACCInfo = clDoc.ACC_Info(dtcheek.Rows[0][1].ToString());
                        string[] _ACCDoc = clDoc.ACC_Info(ACC_Code);


                        Commandtext += @"
                                declare @Key int
                    INSERT INTO  " + ConBank.Database + @".dbo.Table_065_TurnReception
                                                 ([Column01]      ,[Column02]         ,[Column04]      ,[Column05]
                                                  ,[Column06]       ,[Column07]     ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                                    ,[Column12]      ,[Column13]     ,[Column16]   ,[Column18])
                                                       
                            Values (@checkId, " + setting.Rows[78]["Column02"] + " ,'" + FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd") + "'," + _Person + ",'" + setting.Rows[79]["Column02"] + "'," + ACC_Code + "," + Int16.Parse(_ACCDoc[0].ToString()) + ",'" + _ACCDoc[1].ToString() + "'," + (string.IsNullOrEmpty(_ACCDoc[2].ToString()) ? "NULL" : "'" + _ACCDoc[2].ToString() + "'") + "," +
                                                                     (string.IsNullOrEmpty(_ACCDoc[3].ToString()) ? "NULL" : "'" + _ACCDoc[3].ToString() + "'") + "," +
                                                                      (string.IsNullOrEmpty(_ACCDoc[4].ToString()) ? "NULL" : "'" + _ACCDoc[4].ToString() + "'") + ", @DocId,'" + Class_BasicOperation._UserName + "',getdate()); SET @Key=SCOPE_IDENTITY()";



                        Commandtext += @"

                        INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail  ([Column00]  ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06]          ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],     [Column27],Column09 )
                                                                              
                                                                              Values(@DocId,'" + dtcheek.Rows[0][0].ToString() + "'," + Int16.Parse(_ACC_Info[0].ToString()) + ",'" + _ACC_Info[1].ToString() + "'," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[2].ToString()) ? "NULL" : "'" + _ACC_Info[2].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[3].ToString()) ? "NULL" : "'" + _ACC_Info[3].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[4].ToString()) ? "NULL" : "'" + _ACC_Info[4].ToString() + "'") + ",'" + "تسویه - دریافت چک به شماره فاکتور " + _Num + "'," + txtcheck.Value + ",0,0,0,0," + setting.Rows[78]["Column02"] + ",@Key,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,28,@ProjectID) ";
                        Commandtext += @"
                        INSERT INTO " + ConAcnt.Database + @".dbo.Table_065_SanadDetail  ([Column00]  ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                                              ,[Column06]      ,[Column07]      ,[Column10]      ,[Column11]
                                                                              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
                                                                              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],     [Column27],Column09 )
                                                                              
                                                                              Values(@DocId,'" + dtcheek.Rows[0][1].ToString() + "'," + Int16.Parse(_ACC_Info[0].ToString()) + ",'" + _ACC_Info[1].ToString() + "'," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[2].ToString()) ? "NULL" : "'" + _ACC_Info[2].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[3].ToString()) ? "NULL" : "'" + _ACC_Info[3].ToString() + "'") + "," +
                                                                                                   (string.IsNullOrEmpty(_ACC_Info[4].ToString()) ? "NULL" : "'" + _ACC_Info[4].ToString() + "'") + "," + _Person + ",'" + "تسویه - دریافت چک به شماره فاکتور " + _Num + "',0," + txtcheck.Value + ",0,0,0," + setting.Rows[78]["Column02"] + ", @Key ,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,28,@ProjectID) ";




                        Commandtext += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column50=" + (CHK_Delivary.Checked ? "1" : "0") + ",Column51='" + txt_DelivaryDate.Text + "', Column46=" + Convert.ToDouble(txt_cash.Value) + "  , Column47=" + Convert.ToDouble(txtcart.Value) +
                      " ,Column52=" + Convert.ToDouble(this.txtcheck.Value) + ",Column54=" + Convert.ToDouble(this.txt_other.Value) + ",Column45=1,column15='" + Class_BasicOperation._UserName + "',column16=getdate(),Column49=" + (Convert.ToInt32(txtcart.Value) == 0 && (cmb_bank.Value == null || cmb_bank.Value.ToString() == string.Empty) ? "NULL" : cmb_bank.Value.ToString()) + "  where columnid=" + Factroid;
                        Commandtext += @"UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column66=@checkId where Columnid=" + Factroid + "";
                    }



                    string EndText = Commandtext;

                    Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.SALE, EndText);
                    if (chk_cart.Checked)
                    {

                    Properties.Settings.Default.BankName = Bank_Name.SelectedIndex.ToString();
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Hesab_Bank = cmb_bank.Value.ToString();
                    Properties.Settings.Default.Save();
                    }


                    Class_BasicOperation.ShowMsg("", "تسویه با موفقیت انجام شد", "Information");
                    this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    this.Close();
                }
                catch (Exception ex)
                {

                }


            }
            else
            {

                Class_BasicOperation.ShowMsg("", "مقادیر ثبت شده با مقدار فاکتور برابر نیست", "Information");


            }
        }








        private void column25CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            column25CheckBox.Checked = true;
        }












        private void cmb_bank_KeyPress(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);

            cmb_bank.DroppedDown = true;
        }

        private void txt_EndPrice_KeyPress(object sender, KeyPressEventArgs e)
        {

            //Class_BasicOperation.isEnter(e.KeyChar);
            //{
            if (e.KeyChar == 13)
                txt_Date.Focus();
            //}

        }

        private void mande()
        {


            txt_mande.Value = Convert.ToDouble(txt_EndPrice.Value) - (Convert.ToDouble(txtcart.Value) + Convert.ToDouble(txt_cash.Value) + Convert.ToDouble(this.txtcheck.Value) + Convert.ToDouble(this.txt_other.Value));


        }

        private void txt_cash_ValueChanged(object sender, EventArgs e)
        {
            mande();
        }

        private void txtcart_ValueChanged(object sender, EventArgs e)
        {
            mande();
        }

        private void txtother_ValueChanged(object sender, EventArgs e)
        {
            mande();
        }

        private void txt_cash_Click(object sender, EventArgs e)
        {

        }

        private void CHK_Delivary_CheckedChanged(object sender, EventArgs e)
        {
            if (CHK_Delivary.Checked)
            {

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                {
                    Con.Open();

                    SqlCommand Comm = new SqlCommand("SELECT column02  FROM   Table_010_SaleFactor   WHERE  columnid=" + Factroid + "", Con);
                    txt_DelivaryDate.Text = Comm.ExecuteScalar().ToString();
                }
            }
            else
            {
                txt_DelivaryDate.Text = string.Empty;
            }

        }

        private void txtcheck_ValueChanged(object sender, EventArgs e)
        {
            mande();
        }

        private void btn_setcheck_Click(object sender, EventArgs e)
        {

            foreach (Form child in Application.OpenForms)
            {
                if (child.Name == "Form01_ReceiveChq")
                {
                    child.Focus();
                    return;
                }
            }

            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;


            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(PACNT.Class_BasicOperation._UserName, "Column09", 27))
            {
                PACNT._3_Cheque_Operation.Form01_ReceiveChq frm = new PACNT._3_Cheque_Operation.Form01_ReceiveChq(
                      UserScope.CheckScope(PACNT.Class_BasicOperation._UserName, "Column09", 28), UserScope.CheckScope(PACNT.Class_BasicOperation._UserName, "Column09", 29),
                       UserScope.CheckScope(PACNT.Class_BasicOperation._UserName, "Column09", 30), 0, true, Convert.ToDouble(txtcheck.Value));



                frm.ShowDialog();
                //mlt_ReciptCheek.DataSource = clDoc.ReturnTable(ConBank.ConnectionString, @"select columnid  from Table_035_ReceiptCheques");



            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");


        }

        private void btn_setcheck_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13)
                Class_BasicOperation.isEnter(e.KeyChar);
            else if (e.KeyChar == 13)
                btn_setcheck_Click(null, null);
        }

        private void btn_setcheck_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                CHK_Delivary.Select();
            //else if (e.KeyCode == Keys.Space)
            //    btn_setcheck_Click(null, null);
        }

        private void txtother_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                CHK_Delivary.Select();
            //if (e.KeyChar == 32)
            //    txtcheck.Select();
        }

        private void txtother_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void chk_cash_Click(object sender, EventArgs e)
        {
            if (chk_cash.Checked == true)
            {
                txt_cash.Text = txt_mande.Text;
            }
            else if (chk_cash.Checked == false)
            {
                txt_cash.Text = "0";
                txt_mande.Value = Convert.ToDouble(txt_EndPrice.Value) - (Convert.ToDouble(txtcart.Value) + Convert.ToDouble(txt_cash.Value) + Convert.ToDouble(this.txtcheck.Value) + Convert.ToDouble(this.txt_other.Value));
            }


        }

        private void chk_cart_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_cart.Checked == true)
            {
                txtcart.Text = txt_mande.Text;
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.Hesab_Bank))
                    cmb_bank.Value = Properties.Settings.Default.Hesab_Bank;

            }
            else if (chk_cart.Checked == false)
            {
                txtcart.Text = "0";
                txt_mande.Value = Convert.ToDouble(txt_EndPrice.Value) - (Convert.ToDouble(txtcart.Value) + Convert.ToDouble(txt_cash.Value) + Convert.ToDouble(this.txtcheck.Value) + Convert.ToDouble(this.txt_other.Value));

            }


        }

        private void chk_other_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_other.Checked == true)
            {
                txt_other.Text = txt_mande.Text;
            }
            else if (chk_other.Checked == false)
            {
                txt_other.Text = "0";
                txt_mande.Value = Convert.ToDouble(txt_EndPrice.Value) - (Convert.ToDouble(txtcart.Value) + Convert.ToDouble(txt_cash.Value) + Convert.ToDouble(this.txtcheck.Value) + Convert.ToDouble(this.txt_other.Value));
            }
        }



        private void chk_cheq_CheckStateChanged(object sender, EventArgs e)
        {
            if (chk_cheq.Checked == true)
            {
                txtcheck.Text = txt_mande.Text;
            }
            else if (chk_cheq.Checked == false)
            {
                txtcheck.Text = "0";
                txt_mande.Value = Convert.ToDouble(txt_EndPrice.Value) - (Convert.ToDouble(txtcart.Value) + Convert.ToDouble(txt_cash.Value) + Convert.ToDouble(this.txtcheck.Value) + Convert.ToDouble(this.txt_other.Value));
            }
        }

        private void cmb_bank_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtcheck_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(this.txtcheck.Value) > 0)
            {

                uiGroupBox2.Height = 146;
                uiGroupBox2.Width = 344;
                this.uiGroupBox3.Location = new System.Drawing.Point(6, 385);
                this.Height = 520;
                this.Width = 361;
                label6.Enabled = true;
                label7.Enabled = true;
                label16.Enabled = true;
                label12.Enabled = true;
                txt_Branch.Enabled = true;
                txt_Date.Enabled = true;
                mlt_Bank.Enabled = true;
                txt_NumberCheek.Enabled = true;
            }

            else
            {

                uiGroupBox2.Height = 0;
                uiGroupBox2.Width = 344;
                this.uiGroupBox3.Location = new System.Drawing.Point(10, 244);
                this.Height = 382;
                this.Width = 361;
                label6.Enabled = false;
                label7.Enabled = false;
                label16.Enabled = false;
                label12.Enabled = false;
                txt_Branch.Enabled = false;
                txt_Date.Enabled = false;
                mlt_Bank.Enabled = false;
                txt_NumberCheek.Enabled = false;

            }




        }

        private void txtcart_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToDouble(txtcart.Value) > 0)
            {
                label5.Enabled = true;
                cmb_bank.Enabled = true;
                btn_Send.Enabled = true;
            }
            else
            {
                label5.Enabled = false;
                cmb_bank.Enabled = false;
                btn_Send.Enabled = false;
            }
        }

        private void uiGroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void txtcheck_Click(object sender, EventArgs e)
        {

        }

        private void mlt_ReciptCheek_ValueChanged(object sender, EventArgs e)
        {
            //Class_BasicOperation.FilterMultiColumns_Shop(mlt_ReciptCheek, "Column00", "Column03", "Columnid");
            //Class_BasicOperation.FilterMultiColumns(mlt_ReciptCheek, "Name", "Columnid");
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void mlt_ReciptCheek_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void mlt_ReciptCheek_Leave(object sender, EventArgs e)
        {
            //Class_BasicOperation.MultiColumnsRemoveFilter(mlt_ReciptCheek);
        }

        private void mlt_ReciptCheek_TextChanged(object sender, EventArgs e)
        {
            //Class_BasicOperation.FilterMultiColumns(mlt_ReciptCheek, "Name", "Columnid");

        }

        private void txt_Date_KeyPress(object sender, KeyPressEventArgs e)
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

            //txt_NumberCheek.Focus();
        }

        private void txt_NumberCheek_KeyPress(object sender, KeyPressEventArgs e)
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
            //mlt_Bank.Focus();
        }

        private void mlt_Bank_KeyPress(object sender, KeyPressEventArgs e)
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
            //txt_Branch.Focus();
        }

        private void mlt_Bank_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Bank, "Column01", "Column00");
        }

        private void mlt_Bank_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }

        private void btn_Send_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.BankName = Bank_Name.SelectedIndex.ToString();
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Hesab_Bank = cmb_bank.Value.ToString();
            Properties.Settings.Default.Save();
            string bank = Properties.Settings.Default.BankName;

            #region  بانک صادرات
            if (bank == "0")
            {

                string IpAddress = clDoc.ExScalar(Conbase.ConnectionString, @"select isnull((select Column02 from Table_107_SettingPose where Column03=1 and Columnid=2),0)");
                string TerminalId = clDoc.ExScalar(Conbase.ConnectionString, @"select isnull((select Column02 from Table_107_SettingPose where Column03=1 and Columnid=1),0)");
                string Port = clDoc.ExScalar(Conbase.ConnectionString, @"select isnull((select Column02 from Table_107_SettingPose where Column03=1 and Columnid=3),0)");

                if (IpAddress != "0" && TerminalId != "0" && Port != "0")
                {


                    VPCPOS.clsCommunication communication = new VPCPOS.clsCommunication();
                    communication.ConnType = 1;
                    communication.IPAddress = IpAddress;
                    communication.IPPort = Convert.ToInt32(Port);
                    VPCPOS.clsMessage transaction = new VPCPOS.clsMessage();
                    transaction.CancelTimeout = 60;//in second
                    VPCPOS.clsrequests clsrequests = new VPCPOS.clsrequests();
                    clsrequests.msgTyp = VPCPOS.clsMessage.MsgType.Sale;
                    clsrequests.bankId = 1;
                    clsrequests.POSSerialEnable = false;
                    //clsrequests.POSSerialData = "";
                    clsrequests.terminalID = TerminalId;
                    clsrequests.amount = txtcart.Value.ToString();
                    clsrequests._printStr = "فروشگاه ارمغان";
                    clsrequests.MultiAccEnable = false;

                    clsrequests.MultiAccData =
                    @"21M01000000001000123456789012345678900M0200000000100009
                        876543210987654321";
                    clsrequests.MultiAccTableVer = false;
                    clsrequests.MultiAccTableVerData = string.Empty;
                    transaction.crequest = clsrequests;

                    int transactionResult = transaction.SendMessage(1);
                    int retResponse = int.Parse(transactionResult.ToString());
                    string result = string.Empty;

                    switch (retResponse)
                    {
                        case 0:
                            result = "دریافت با موفقیت انجام شد";
                            MessageBox.Show(result);
                            bt_Save_Click(sender, e);
                            break;
                        case 1:
                            result = "خطا در ارتباط";
                            MessageBox.Show(result);
                            break;
                        case 2:
                            result = "خطا در دریافت اطلاعات";
                            MessageBox.Show(result);
                            break;
                        case 3:
                            result = "خطا در ارسال اطلاعات";
                            MessageBox.Show(result);
                            break;
                        case 4:
                            result = "خطا در تراکنش بازگشتی";
                            MessageBox.Show(result);

                            break;
                        case 5:
                            result = "لغو توسط کاربر";
                            MessageBox.Show(result);

                            break;
                        case 6:
                            result = "خط ارتباطی مشغول است";
                            MessageBox.Show(result);

                            break;
                        case 7:
                            result = "پاسخ دریافت نشد";
                            MessageBox.Show(result);

                            break;
                        case 8:
                            result = "خطا در ابطال";
                            MessageBox.Show(result);

                            break;
                        case 9:
                            result = "خطا در خط تلفن ";
                            MessageBox.Show(result);

                            break;
                        case 10:
                            result = "خطای داخلی";
                            MessageBox.Show(result);

                            break;
                        case 11:
                            result = "خطای زمان انجام تراکنش";
                            MessageBox.Show(result);

                            break;
                        case 12:
                            result = "خطا در خواندن اطلاعات کارت";
                            MessageBox.Show(result);

                            break;
                        case 50:
                            result = "خطا در داده های ترمینال";
                            MessageBox.Show(result);

                            break;
                        case 51:
                            result = "دستگاه آماده انجام تراکنش نیست";
                            MessageBox.Show(result);

                            break;
                        case 98:
                            result = "خطای سوئیچ بانکی";
                            MessageBox.Show(result);

                            break;
                        case 99:
                            result = "خطای غیر متبط";
                            MessageBox.Show(result);

                            break;
                        default:
                            result = "خطا";
                            MessageBox.Show(result);

                            break;
                    }


                }
                else
                {
                    MessageBox.Show("لطفا تنظیمات دستگاه پوز از قسمت اطلاعات پایه فرم تنظیمات تراکنش انجام شود");
                }

            }
            #endregion

            #region بانک ملت
            else if (bank == "1")
            {

                string IpAddress = clDoc.ExScalar(Conbase.ConnectionString, @"select isnull((select Column02 from Table_107_SettingPose where Column03=2 and Columnid=4),0)");
                //string TerminalId = clDoc.ExScalar(Conbase.ConnectionString, @"select isnull((select Column02 from Table_107_SettingPose where Column03=1 and Columnid=1),0)");
                string Port = clDoc.ExScalar(Conbase.ConnectionString, @"select isnull((select Column02 from Table_107_SettingPose where Column03=2 and Columnid=5),0)");
                if (IpAddress != "0" && IpAddress != "" && Port != "0" && Port != "")
                {
                    //btDebitConfirm.Enabled = false;
                    //----------------- Transaction config --------------------------------
                    int AmountInt;
                    bool isNumeric6 = int.TryParse(txtcart.Value.ToString(), out AmountInt);
                    if (txtcart.Value.ToString().Length < 4 || isNumeric6 == false)
                    {
                        MessageBox.Show(" مبلغ خرید کالا نامعتبر است");
                    }
                    else
                    {
                        System.Net.Sockets.TcpClient client = null;
                        try
                        {
                            System.Net.ServicePointManager.Expect100Continue = false;
                            byte[] resvCommand = new byte[10025];
                            client = new System.Net.Sockets.TcpClient(IpAddress, UInt16.Parse(Port)); // Create a new connection  
                            if (!client.Connected)
                            {
                                //btDebitConfirm.Enabled = true;
                                MessageBox.Show("pleas check Service Port");
                                return;
                            }
                            NetworkStream stream = client.GetStream();
                            string str_comm = "" + "{\"ServiceCode\" :\"" + "1";
                            if (txtcart.Value.ToString().Length > 0)
                                str_comm += "\",\"Amount\":\"" + txtcart.Value.ToString();

                            str_comm += "\"}";


                            byte[] sendCommand = System.Text.Encoding.ASCII.GetBytes(str_comm);
                            stream.Write(sendCommand, 0, sendCommand.Length);
                            stream.ReadTimeout = 180000;
                            int recvSize = stream.Read(resvCommand, 0, resvCommand.Length);

                            string jsonStr = Encoding.UTF8.GetString(resvCommand);
                            Dictionary<String, String> values = JsonConvert.DeserializeObject<Dictionary<String, String>>(jsonStr);
                            string Result = ParseJson(values);
                            if (Result == "100")
                            {

                                bt_Save_Click(sender, e);
                                return;
                            }

                            else
                            {
                                MessageBox.Show("خطا به شماره " + Result + "در ارسال اطلاعات");
                                return;
                            }
                            //btDebitConfirm.Enabled = true;
                            client.Close();


                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                if (ex.Message.ToString() == "No connection could be made because the target machine actively refused it 127.0.0.1:1024")
                                {
                                    MessageBox.Show("ارتباط با دستگاه برقرار نمی باشد");
                                    return;
                                    client.Close();
                                }
                                //MessageBox.Show(ex.Message.ToString());



                            }
                            catch
                            { }
                        }

                    }
                    //btDebitConfirm.Enabled = true;
                }
                else
                {
                    MessageBox.Show("لطفا تنظیمات دستگاه پوز مورد نظر را از قسمت اطلاعات پایه فرم تنظیمات تراکنش انجام شود");
                    return;
                }

            }
            #endregion

            #region آسان پرداخت
            else if (bank == "2")
            {
                //try
                //{


                string IpAddress = clDoc.ExScalar(Conbase.ConnectionString, @"select isnull((select Column02 from Table_107_SettingPose where Column03=3 and Columnid=6),0)");

                //string Sheba = clDoc.ExScalar(Conbase.ConnectionString, @"select isnull((select Column02 from Table_107_SettingPose where Column03=3 and Columnid=7),0)");

                if (IpAddress != "" || IpAddress != "0")
                {
                    PCPos pcPos = new PCPos();

                    pcPos.InitLAN(IpAddress, 17000);
                    pcPos.TimeOutPerTransaction = 60000;

                    PaymentResult result = pcPos.DoSyncPayment(Convert.ToInt64(txtcart.Text.Replace(",", "")).ToString(), "", _Num.ToString(), DateTime.Now);

                    if (result.ErrorCode == 600)
                    {
                        Class_BasicOperation.ShowMsg("", " تراکنش ناموفق می باشد ", "Stop");
                        return;
                    }
                    else if (result.ErrorCode == -1)
                    {
                        Class_BasicOperation.ShowMsg("", " لطفا اتصال دستگاه به سیستم را بررسی کنید ", "Stop");
                        return;
                    }
                    else if (result.ErrorCode == 61)
                    {
                        Class_BasicOperation.ShowMsg("", " مبلغ تراکنش بیش از حد مجاز می باشد ", "Warning");
                        return;
                    }
                    else if (result.ErrorCode == 0)
                    {
                        bt_Save_Click(sender, e);
                        //MessageBox.Show("عملیات با موفقیت انجام شد");
                        return;
                    }


                }

            }
            #endregion

            #region بانک سامان

            else if (bank == "3")
            {
                if (_PcPosFactory == null)
                {
                    _PcPosFactory = new PcPosFactory();
                }
                MediaType _mediaType = new MediaType();
                SSP1126.PcPos.Infrastructure.ResponseLanguage _responseLanguage = new SSP1126.PcPos.Infrastructure.ResponseLanguage();
                SSP1126.PcPos.Infrastructure.AsyncType _asyncType = new SSP1126.PcPos.Infrastructure.AsyncType();
                _mediaType = MediaType.Network;


                string IpAddress = clDoc.ExScalar(Conbase.ConnectionString, @"select isnull((select Column02 from Table_107_SettingPose where Column03=4 and Columnid=7),0)");
                string Terminal = clDoc.ExScalar(Conbase.ConnectionString, @"select isnull((select Column02 from Table_107_SettingPose where Column03=4 and Columnid=8),0)");

                if (string.IsNullOrEmpty(IpAddress) || IpAddress == "0")
                {
                    Class_BasicOperation.ShowMsg("", "آدرس شبکه وارد شده معتبر نمی باشد", "Stop");
                    return;
                }
                _PcPosFactory.SetLan(IpAddress);
                _PcPosFactory.Initialization(_responseLanguage, 0, _asyncType);

                posResult = _PcPosFactory.PcStarterPurchase(txtcart.Value.ToString(), txtcart.Value.ToString(), "", string.Empty, Terminal, string.Empty, string.Empty, -1);

                if (_asyncType == SSP1126.PcPos.Infrastructure.AsyncType.Sync && posResult != null)
                    if (posResult.ResponseCode == "00")
                    {
                        bt_Save_Click(sender, e);

                        return;
                    }
                    else
                    {
                        Respons();
                    }
            }
            #endregion

        }
        public enum ResponseLanguage
        {
            Persian = 0,
            English = 1
        }
        public enum AsyncType
        {
            Sync = 0,
            Async = 1,
        }
        public enum MediaType
        {
            Com = 1,
            Network = 2,
        }
        public enum AccountType
        {
            Single = 0,
            Share = 1,
            ShareByIban = 2
        }
       
        private void Respons()
        {
           
             if (posResult.ResponseCode == "58")

            {
                Class_BasicOperation.ShowMsg("", "انجام تراکنش غیر مجاز می باشد", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "2")
            {
                Class_BasicOperation.ShowMsg("", "مبلغ تراکنش نمی تواند از حداقل مبلغ کوچکتر باشد", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "3")
            {
                Class_BasicOperation.ShowMsg("", "عدم ارتباط با دستگاه", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "79" || posResult.ResponseCode == "13")
            {
                Class_BasicOperation.ShowMsg("", "مبلغ نامعتبر", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "51")
            {
                Class_BasicOperation.ShowMsg("", "موجودی کافی نمی باشد", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "97")
            {
                Class_BasicOperation.ShowMsg("", "عدم ارتباط با مرکز", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "78")
            {
                Class_BasicOperation.ShowMsg("", "کارت غیرفعال میباشد", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "61")
            {
                Class_BasicOperation.ShowMsg("", "مبلغ تراکنش بیش از حد مجاز می باشد", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "61")
            {
                Class_BasicOperation.ShowMsg("", "مبلغ تراکنش بیش از حد مجاز می باشد", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "68")
            {
                Class_BasicOperation.ShowMsg("", "عدم دریافت پاسخ در زمان مناسب", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "38")
            {
                Class_BasicOperation.ShowMsg("", "عداد دفعات ورود رمز غلط بیش از حدمجاز است", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "12")
            {
                Class_BasicOperation.ShowMsg("", "تراکنش نامعتبر", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "26")
            {
                Class_BasicOperation.ShowMsg("", "خطا در تراکنش", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "64")
            {
                Class_BasicOperation.ShowMsg("", "مبلغ نامعتبر می باشد", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "55")
            {
                Class_BasicOperation.ShowMsg("", "رمز کارت نا معتبر است", "Stop");
                return;
            }
            else if (posResult.ResponseCode == "75" || posResult.ResponseCode == "69")
            {
                Class_BasicOperation.ShowMsg("", " تعداد دفعات ورود رمزغلط بیش از حدمجاز است", "Stop");
                return;

            }
            else if (posResult.ResponseCode == "98" )
            {
                Class_BasicOperation.ShowMsg("", "لغو عملیات توسط کاربر", "Stop");
                return;

            }
            else if (posResult.ResponseCode == "99")
            {
                Class_BasicOperation.ShowMsg("", "عدم دریافت پاسخ در زمان مناسب در کارتخوان", "Stop");
                return;

            }
            else if (posResult.ResponseCode == "72")
            {
                Class_BasicOperation.ShowMsg("", "عدم دریافت پاسخ در زمان مناسب از دستگاه کارتخوان", "Stop");
                return;

            }
            else if (posResult.ResponseCode == "33")
            {
                Class_BasicOperation.ShowMsg("", "تاریخ انقضای کارت سپری شده است", "Stop");
                return;

            }
            else
            {
                Class_BasicOperation.ShowMsg("", "پاسخ نامعتبر می باشد", "Stop");
                return;
            }
        }


        private string ParseJson(Dictionary<string, string> values)
        {
            string result = "";
            foreach (KeyValuePair<String, String> d in values)
            {
                if (d.Key == "ReturnCode")
                {
                    result = d.Value;


                }
            }

            return result;
        }




    }
}
