using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._02_BasicInfo
{
    public partial class Frm_006_Transactions : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        SqlConnection ConSALE = new SqlConnection(Properties.Settings.Default.SALE);

        bool Tasveh = false;
        string statuse, reciptbank;
        public Frm_006_Transactions()
        {
            InitializeComponent();
        }
        public Frm_006_Transactions(bool _Tasveh , string _Statuse ,string _ReciptBank )
        {
            InitializeComponent();
            Tasveh = _Tasveh;
            statuse = _Statuse;
            reciptbank = _ReciptBank;
          
        }
      
        public void Frm_006_Transactions_Load(object sender, EventArgs e)
        {

        table_107_SettingPoseTableAdapter.Fill(dataSet_Sale.Table_107_SettingPose);

            SqlConnection conACNT = new SqlConnection(Properties.Settings.Default.ACNT);
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from AllHeaders()", conACNT);
            DataTable Headers = new DataTable();
            Adapter.Fill(Headers);
            gridEX1.DropDowns["Header1"].SetDataBinding(Headers, "");
            gridEX1.DropDowns["Header2"].SetDataBinding(Headers, "");
            //mlt_Ware.DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from Table_001_PWHRS where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + "')");
            mlt_Function.DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from table_005_PwhrsOperation where Column16=1");
            mlt_ReciptFunction.DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from table_005_PwhrsOperation where Column16=0");
            this.table_105_SystemTransactionInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_105_SystemTransactionInfo);
            DataTable CustomerTable = clDoc.ReturnTable
      (ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
                                           dbo.Table_045_PersonInfo.Column01 AS code,
                                           dbo.Table_045_PersonInfo.Column02 AS NAME,
                                           dbo.Table_065_CityInfo.Column02 AS shahr,
                                           dbo.Table_060_ProvinceInfo.Column01 AS ostan,
                                           dbo.Table_045_PersonInfo.Column06 AS ADDRESS,
                                           dbo.Table_045_PersonInfo.Column30,
                                           Table_045_PersonInfo.Column07,
                                           Table_045_PersonInfo.Column19 AS Mobile
                                    FROM   dbo.Table_045_PersonInfo
                                           LEFT JOIN dbo.Table_065_CityInfo
                                                ON  dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                                           LEFT JOIN dbo.Table_060_ProvinceInfo
                                                ON  dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00
                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");
            //mlt_Customer.DataSource = CustomerTable;
            //mlt_SaleType.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes");
            //mlt_saler.DataSource = (clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)"));

            mlt_ReciptBank.DataSource = clDoc.ReturnTable(ConBank.ConnectionString, @"Select * from Table_020_BankCashAccInfo");
            mlt_Statuse.DataSource = clDoc.ReturnTable(ConBank.ConnectionString, @"select * from Table_060_ChequeStatus");

            mlt_Return_status.DataSource = clDoc.ReturnTable(ConBank.ConnectionString, @"select * from Table_060_ChequeStatus");

            mlt_BankPose.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, @"select * from Table_106_POS");

          


          //mlt_saler.Value = Properties.Settings.Default.Saler;
           
            DataTable Header5 = new DataTable();
            Adapter = new SqlDataAdapter("Select * from AllHeaders()", ConAcnt);
            Adapter.Fill(Header5);
            multiColumnCombo1.DataSource = Header5;



            LoadDefaultSettings();
            mlt_BankPose.Value = clDoc.ExScalar(ConBase.ConnectionString, @"select column02 from Table_030_Setting  where Columnid=81");
        }

        private void LoadDefaultSettings()
        {
            //DataTable DefaltTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_105_SystemTransactionInfo where Column00>=107 and Column00<=115");

            //Properties.Settings.Default.ShowPriceAlert = Convert.ToInt32(DefaltTable.Rows[4]["Column02"]);
            switch (Properties.Settings.Default.ShowPriceAlert)
            {
                case 0:
                    rdb_Alert_No.Checked = true;
                    rdb_alert_Alert.Checked = false;
                    rdb_Alert_Forbid.Checked = false;
                    break;
                case 1:
                    rdb_Alert_No.Checked = false;
                    rdb_alert_Alert.Checked = true;
                    rdb_Alert_Forbid.Checked = false;
                    break;
                case 2:
                    rdb_Alert_No.Checked = false;
                    rdb_alert_Alert.Checked = false;
                    rdb_Alert_Forbid.Checked = true;
                    break;
            }
            if (Properties.Settings.Default.FactorPrice == 0)
            {
                rb_saletype.Checked = true;
                rb_statment.Checked = false;

            }
            else
            {
                rb_saletype.Checked = false;
                rb_statment.Checked = true;

            }
            //Properties.Settings.Default.ShowCalculateGiftDuringSave = Convert.ToBoolean(DefaltTable.Rows[1]["Column02"]);
            chk_CalGift.Checked = Properties.Settings.Default.ShowCalculateGiftDuringSave;
            DataTable Table = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_105_SystemTransactionInfo where Column00=65");
            if (Table.Rows.Count == 0)
            {
                clDoc.RunSqlCommand(ConBase.ConnectionString, "INSERT INTO Table_105_SystemTransactionInfo (Column00,Column02,Column01) Values(65,0,'محاسبه تخفیفات خطی بر اساس آخرین فاکتور فروش')");
                chk_LinearDiscount.Checked = false;
            }
            else
                chk_LinearDiscount.Checked = Convert.ToBoolean(short.Parse(Table.Rows[0]["Column02"].ToString()));

            //Properties.Settings.Default.ShowChangePriceInSale = Convert.ToBoolean(DefaltTable.Rows[0]["Column02"]);
            chk_ChangePrice.Checked = Properties.Settings.Default.ShowChangePriceInSale;


            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_030_Setting", ConBase);
            DataTable setting = new DataTable();
            Adapter.Fill(setting);




            chkSaleNum.Checked = Convert.ToBoolean(setting.Rows[3]["Column02"]);
            chkSaleDate.Checked = Convert.ToBoolean(setting.Rows[4]["Column02"]);
            chkSaleValue.Checked = Convert.ToBoolean(setting.Rows[5]["Column02"]);
            chkSaleAmount.Checked = Convert.ToBoolean(setting.Rows[6]["Column02"]);
            chkSaleDesc.Checked = Convert.ToBoolean(setting.Rows[7]["Column02"]);
            chkSaleEtebar.Checked = Convert.ToBoolean(setting.Rows[8]["Column02"]);
            chkSaleNetPrice.Checked = Convert.ToBoolean(setting.Rows[9]["Column02"]);
            chkSaleTotalAmount.Checked = Convert.ToBoolean(setting.Rows[10]["Column02"]);
            chkSaleSumDiscount.Checked = Convert.ToBoolean(setting.Rows[11]["Column02"]);
            chkSaleSumExtra.Checked = Convert.ToBoolean(setting.Rows[12]["Column02"]);
            chkSaleCustomer.Checked = Convert.ToBoolean(setting.Rows[13]["Column02"]);
            chkSaleWare.Checked = Convert.ToBoolean(setting.Rows[14]["Column02"]);
            chkSaleFunc.Checked = Convert.ToBoolean(setting.Rows[16]["Column02"]);
            chk_SaleCondition.Checked = Convert.ToBoolean(setting.Rows[15]["Column02"]);


            chkBuyNum.Checked = Convert.ToBoolean(setting.Rows[17]["Column02"]);
            chkBuyDate.Checked = Convert.ToBoolean(setting.Rows[18]["Column02"]);
            chkBuyValue.Checked = Convert.ToBoolean(setting.Rows[19]["Column02"]);
            chkBuyAmount.Checked = Convert.ToBoolean(setting.Rows[20]["Column02"]);
            chkBuyDesc.Checked = Convert.ToBoolean(setting.Rows[21]["Column02"]);
            chkBuyEtebar.Checked = Convert.ToBoolean(setting.Rows[22]["Column02"]);
            chkBuyNetPrice.Checked = Convert.ToBoolean(setting.Rows[23]["Column02"]);
            chkBuyTotalAmount.Checked = Convert.ToBoolean(setting.Rows[24]["Column02"]);
            chkBuySumDiscount.Checked = Convert.ToBoolean(setting.Rows[25]["Column02"]);
            chkBuySumExtra.Checked = Convert.ToBoolean(setting.Rows[26]["Column02"]);
            chkBuyCustomer.Checked = Convert.ToBoolean(setting.Rows[27]["Column02"]);
            chkBuyWare.Checked = Convert.ToBoolean(setting.Rows[28]["Column02"]);
            chkBuyFunc.Checked = Convert.ToBoolean(setting.Rows[30]["Column02"]);
            chk_BuyCondition.Checked = Convert.ToBoolean(setting.Rows[29]["Column02"]);

            chkBuyFee.Checked = Convert.ToBoolean(setting.Rows[31]["Column02"]);
            chkSaleFee.Checked = Convert.ToBoolean(setting.Rows[32]["Column02"]);
            chkBuyMeghdar.Checked = Convert.ToBoolean(setting.Rows[33]["Column02"]);
            chkSaleMeghdar.Checked = Convert.ToBoolean(setting.Rows[34]["Column02"]);
            chk_TaeedSefareshat.Checked = Convert.ToBoolean(setting.Rows[39]["Column02"]);




            txt_ZaribEmtiaz.Value = Convert.ToDouble(setting.Rows[40]["Column02"]);
            txt_ZaribRialiPoorsant.Value = Convert.ToDouble(setting.Rows[41]["Column02"]);
            if (setting.Rows[44]["Column02"] != DBNull.Value && setting.Rows[44]["Column02"] != null)
                mlt_Function.Value = Convert.ToInt32(setting.Rows[44]["Column02"]);

            if (setting.Rows[48]["Column02"] != DBNull.Value && setting.Rows[48]["Column02"] != null)
                mlt_ReciptFunction.Value = Convert.ToInt32(setting.Rows[48]["Column02"]);


            //if (setting.Rows[45]["Column02"] != DBNull.Value && setting.Rows[45]["Column02"] != null)
            //    mlt_Ware.Value = Convert.ToInt32(setting.Rows[45]["Column02"]);


            if (setting.Rows[63]["Column02"] != DBNull.Value && setting.Rows[63]["Column02"] != null)
                multiColumnCombo1.Value = setting.Rows[63]["Column02"].ToString();
            if (setting.Rows[62]["Column02"] != DBNull.Value && setting.Rows[62]["Column02"] != null)
                chk_detail.Checked = Convert.ToBoolean(setting.Rows[62]["Column02"]);


            if (setting.Rows[78]["Column02"] != DBNull.Value && setting.Rows[78]["Column02"] != null)
                this.mlt_Statuse.Value = Convert.ToInt32(setting.Rows[78]["Column02"]);

            if (setting.Rows[79]["Column02"] != DBNull.Value && setting.Rows[79]["Column02"] != null)
                this.mlt_ReciptBank.Value = Convert.ToInt32(setting.Rows[79]["Column02"]);


            if (setting.Rows[79]["Column02"] != DBNull.Value && setting.Rows[81]["Column02"] != null)
                this.mlt_Return_status.Value = Convert.ToInt32(setting.Rows[81]["Column02"]);
            //mlt_Customer.Value = Properties.Settings.Default.Customer;
            //mlt_SaleType.Value = Properties.Settings.Default.SaleType;

            //Properties.Settings.Default.ShowExportSanad = Convert.ToBoolean(DefaltTable.Rows[2]["Column02"]);
            chk_ShowExportSanad.Checked = Properties.Settings.Default.ShowExportSanad;

            //Properties.Settings.Default.ExtraMethod = Convert.ToBoolean(DefaltTable.Rows[3]["Column02"]);
           // chk_ExtraMethod.Checked = Properties.Settings.Default.ExtraMethod;
            chk_ExtraMethod.Checked = Convert.ToBoolean(setting.Rows[70]["Column02"]);


            //chkSaleNum.Checked = Properties.Settings.Default.SaleNum;
            //chkSaleDate.Checked = Properties.Settings.Default.SaleDate;
            //chkSaleValue.Checked = Properties.Settings.Default.SaleValue;
            //chkSaleAmount.Checked = Properties.Settings.Default.SaleAmount;
            //chkSaleDesc.Checked = Properties.Settings.Default.SaleDesc;
            //chkSaleEtebar.Checked = Properties.Settings.Default.SaleEtebar;
            //chkSaleNetPrice.Checked = Properties.Settings.Default.SaleNetPrice;
            //chkSaleTotalAmount.Checked = Properties.Settings.Default.SaleTotalAmount;
            //chkSaleSumDiscount.Checked = Properties.Settings.Default.SaleSumDiscount;
            //chkSaleSumExtra.Checked = Properties.Settings.Default.SaleSumExtra;
            //chkSaleCustomer.Checked = Properties.Settings.Default.SaleCustomer;
            //chkSaleWare.Checked = Properties.Settings.Default.SaleWare;
            //chkSaleFunc.Checked = Properties.Settings.Default.SaleFunc;
            //chk_SaleCondition.Checked = Properties.Settings.Default.SaleCondition;


            //chkBuyNum.Checked = Properties.Settings.Default.BuyNum;
            //chkBuyDate.Checked = Properties.Settings.Default.BuyDate;
            //chkBuyValue.Checked = Properties.Settings.Default.BuyValue;
            //chkBuyAmount.Checked = Properties.Settings.Default.BuyAmount;
            //chkBuyDesc.Checked = Properties.Settings.Default.BuyDesc;
            //chkBuyEtebar.Checked = Properties.Settings.Default.BuyEtebar;
            //chkBuyNetPrice.Checked = Properties.Settings.Default.BuyNetPrice;
            //chkBuyTotalAmount.Checked = Properties.Settings.Default.BuyTotalAmount;
            //chkBuySumDiscount.Checked = Properties.Settings.Default.BuySumDiscount;
            //chkBuySumExtra.Checked = Properties.Settings.Default.BuySumExtra;
            //chkBuyCustomer.Checked = Properties.Settings.Default.BuyCustomer;
            //chkBuyWare.Checked = Properties.Settings.Default.BuyWare;
            //chkBuyFunc.Checked = Properties.Settings.Default.BuyFunc;
            //chk_BuyCondition.Checked = Properties.Settings.Default.BuyCondition;

            //Properties.Settings.Default.tax = Convert.ToDecimal(DefaltTable.Rows[5]["Column02"]);
            //Properties.Settings.Default.taxes = Convert.ToDecimal(DefaltTable.Rows[6]["Column02"]);
            //Properties.Settings.Default.buytax = Convert.ToDecimal(DefaltTable.Rows[7]["Column02"]);
            //Properties.Settings.Default.buytaxes = Convert.ToDecimal(DefaltTable.Rows[8]["Column02"]);
            txt_tax.Value = Properties.Settings.Default.tax;
            txt_taxes.Value = Properties.Settings.Default.taxes;

            txt_buytax.Value = Properties.Settings.Default.buytax;
            txt_buytaxes.Value = Properties.Settings.Default.buytaxes;
            chk_showremain.Checked = Properties.Settings.Default.ShowMojodi;

        }
        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;
            try
            {
                //if (gridEX1.GetValue("Column07").ToString() != "")
                //{
                gridEX1.SetValue("Column02", gridEX1.DropDowns["Header1"].GetValue("GroupCode"));
                gridEX1.SetValue("Column03", gridEX1.DropDowns["Header1"].GetValue("KolCode"));
                gridEX1.SetValue("Column04", gridEX1.DropDowns["Header1"].GetValue("MoeinCode"));
                gridEX1.SetValue("Column05", gridEX1.DropDowns["Header1"].GetValue("TafsiliCode"));
                gridEX1.SetValue("Column06", gridEX1.DropDowns["Header1"].GetValue("JozCode"));
                //}

                //if (gridEX1.GetValue("Column07").ToString() != "")
                //{
                gridEX1.SetValue("Column08", gridEX1.DropDowns["Header2"].GetValue("GroupCode"));
                gridEX1.SetValue("Column09", gridEX1.DropDowns["Header2"].GetValue("KolCode"));
                gridEX1.SetValue("Column10", gridEX1.DropDowns["Header2"].GetValue("MoeinCode"));
                gridEX1.SetValue("Column11", gridEX1.DropDowns["Header2"].GetValue("TafsiliCode"));
                gridEX1.SetValue("Column12", gridEX1.DropDowns["Header2"].GetValue("JozCode"));
                //}
            }
            catch
            {
            }
        }

        private void gridEX1_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Column.Key == "Column03" || e.Column.Key == "Column04" || e.Column.Key == "Column05" || e.Column.Key == "Column06" || e.Column.Key == "Column08" || e.Column.Key == "Column09" || e.Column.Key == "Column10" || e.Column.Key == "Column11" || e.Column.Key == "Column12")
                {
                    if (e.Value.ToString().Trim() == "")
                        e.Value = DBNull.Value;
                }
            }
            catch
            {
                if (e.Column.Key == "Column03" || e.Column.Key == "Column04" || e.Column.Key == "Column05" || e.Column.Key == "Column06" || e.Column.Key == "Column08" || e.Column.Key == "Column09" || e.Column.Key == "Column10" || e.Column.Key == "Column11" || e.Column.Key == "Column12")
                {
                    e.Value = DBNull.Value;
                }
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (this.table_105_SystemTransactionInfoBindingSource.Count > 0)
            {
                try
                {
                    this.Validate();
                    this.table_105_SystemTransactionInfoBindingSource.EndEdit();
                    this.table_105_SystemTransactionInfoTableAdapter.Update(dataSet_EtelaatPaye.Table_105_SystemTransactionInfo);
                    Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "Information");
                }
                catch (System.Data.SqlClient.SqlException es)
                {
                    Class_BasicOperation.CheckSqlExp(es, "Form01_Transactions");
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;

                }
            }
        }
        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, "Form01_Transactions");
        }

        private void Form01_Transactions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
            {
                if (uiTab1.SelectedTab == uiTabPage1)
                    bt_Save_Click(sender, e);
                else if (uiTab1.SelectedTab == uiTabPage2)
                    bt_SaveSetting_Click(sender, e);
                else if (uiTab1.SelectedTab == uiTabPage3)
                    toolStripButton1_Click(sender, e);
            }

        }

        private void gridEX1_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                if (gridEX1.GetRow().Cells["Column07"].Text.Trim() == "")
                {

                    gridEX1.SetValue("Column02", DBNull.Value);
                    gridEX1.SetValue("Column03", DBNull.Value);
                    gridEX1.SetValue("Column04", DBNull.Value);
                    gridEX1.SetValue("Column05", DBNull.Value);
                    gridEX1.SetValue("Column06", DBNull.Value);
                }
                if (gridEX1.GetRow().Cells["Column12"].Text.Trim() == "")
                {
                    gridEX1.SetValue("Column08", DBNull.Value);
                    gridEX1.SetValue("Column09", DBNull.Value);
                    gridEX1.SetValue("Column10", DBNull.Value);
                    gridEX1.SetValue("Column11", DBNull.Value);
                    gridEX1.SetValue("Column12", DBNull.Value);
                }


                if (gridEX1.GetValue("Column07").ToString().Length == 3)
                {
                    gridEX1.SetValue("Column04", DBNull.Value);
                    gridEX1.SetValue("Column05", DBNull.Value);
                    gridEX1.SetValue("Column06", DBNull.Value);
                }
                else if (gridEX1.GetValue("Column07").ToString().Length == 6)
                {
                    gridEX1.SetValue("Column05", DBNull.Value);
                    gridEX1.SetValue("Column06", DBNull.Value);
                }
                else if (gridEX1.GetValue("Column07").ToString().Length == 9)
                {
                    gridEX1.SetValue("Column06", DBNull.Value);
                }

                if (gridEX1.GetValue("Column12").ToString().Length == 3)
                {
                    gridEX1.SetValue("Column09", DBNull.Value);
                    gridEX1.SetValue("Column10", DBNull.Value);
                    gridEX1.SetValue("Column11", DBNull.Value);
                }
                else if (gridEX1.GetValue("Column12").ToString().Length == 6)
                {
                    gridEX1.SetValue("Column10", DBNull.Value);
                    gridEX1.SetValue("Column11", DBNull.Value);
                }
                else if (gridEX1.GetValue("Column12").ToString().Length == 9)
                {
                    gridEX1.SetValue("Column11", DBNull.Value);
                }
            }
            catch
            {
            }
        }

        private void bt_SaveSetting_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ShowChangePriceInSale = chk_ChangePrice.Checked;
            Properties.Settings.Default.ShowPriceAlert = (rdb_Alert_No.Checked ? 0 : (rdb_alert_Alert.Checked ? 1 : 2));
            Properties.Settings.Default.ShowCalculateGiftDuringSave = chk_CalGift.Checked;
            //Properties.Settings.Default.ExtraMethod = chk_ExtraMethod.Checked;
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chk_ExtraMethod.Checked ? "N'True'" : "N'False'") + " where ColumnId=71");

            Properties.Settings.Default.ShowExportSanad = chk_ShowExportSanad.Checked;
            Properties.Settings.Default.ShowMojodi = chk_showremain.Checked;

            if (rb_saletype.Checked)

                Properties.Settings.Default.FactorPrice = 0;
            else

                Properties.Settings.Default.FactorPrice = 1;


            //Properties.Settings.Default.SaleNum = chkSaleNum.Checked;
            //Properties.Settings.Default.SaleDate = chkSaleDate.Checked;
            //Properties.Settings.Default.SaleValue = chkSaleValue.Checked;
            //Properties.Settings.Default.SaleAmount = chkSaleAmount.Checked;
            //Properties.Settings.Default.SaleDesc = chkSaleDesc.Checked;
            //Properties.Settings.Default.SaleEtebar = chkSaleEtebar.Checked;
            //Properties.Settings.Default.SaleNetPrice = chkSaleNetPrice.Checked;
            //Properties.Settings.Default.SaleTotalAmount = chkSaleTotalAmount.Checked;
            //Properties.Settings.Default.SaleSumDiscount = chkSaleSumDiscount.Checked;
            //Properties.Settings.Default.SaleSumExtra = chkSaleSumExtra.Checked;
            //Properties.Settings.Default.SaleCustomer = chkSaleCustomer.Checked;
            //Properties.Settings.Default.SaleWare = chkSaleWare.Checked;
            //Properties.Settings.Default.SaleFunc = chkSaleFunc.Checked;
            //Properties.Settings.Default.SaleCondition = chk_SaleCondition.Checked;


            //Properties.Settings.Default.BuyNum = chkBuyNum.Checked;
            //Properties.Settings.Default.BuyDate = chkBuyDate.Checked;
            //Properties.Settings.Default.BuyValue = chkBuyValue.Checked;
            //Properties.Settings.Default.BuyAmount = chkBuyAmount.Checked;
            //Properties.Settings.Default.BuyDesc = chkBuyDesc.Checked;
            //Properties.Settings.Default.BuyEtebar = chkBuyEtebar.Checked;
            //Properties.Settings.Default.BuyNetPrice = chkBuyNetPrice.Checked;
            //Properties.Settings.Default.BuyTotalAmount = chkBuyTotalAmount.Checked;
            //Properties.Settings.Default.BuySumDiscount = chkBuySumDiscount.Checked;
            //Properties.Settings.Default.BuySumExtra = chkBuySumExtra.Checked;
            //Properties.Settings.Default.BuyCustomer = chkBuyCustomer.Checked;
            //Properties.Settings.Default.BuyWare = chkBuyWare.Checked;
            //Properties.Settings.Default.BuyFunc = chkBuyFunc.Checked;
            //Properties.Settings.Default.BuyCondition = chk_BuyCondition.Checked;




            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleNum.Checked ? "N'True'" : "N'False'") + " where ColumnId=4");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleDate.Checked ? "N'True'" : "N'False'") + " where ColumnId=5");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleValue.Checked ? "N'True'" : "N'False'") + " where ColumnId=6");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleAmount.Checked ? "N'True'" : "N'False'") + " where ColumnId=7");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleDesc.Checked ? "N'True'" : "N'False'") + " where ColumnId=8");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleEtebar.Checked ? "N'True'" : "N'False'") + " where ColumnId=9");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleNetPrice.Checked ? "N'True'" : "N'False'") + " where ColumnId=10");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleTotalAmount.Checked ? "N'True'" : "N'False'") + " where ColumnId=11");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleSumDiscount.Checked ? "N'True'" : "N'False'") + " where ColumnId=12");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleSumExtra.Checked ? "N'True'" : "N'False'") + " where ColumnId=13");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleCustomer.Checked ? "N'True'" : "N'False'") + " where ColumnId=14");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleWare.Checked ? "N'True'" : "N'False'") + " where ColumnId=15");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chk_SaleCondition.Checked ? "N'True'" : "N'False'") + " where ColumnId=16");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkSaleFunc.Checked ? "N'True'" : "N'False'") + " where ColumnId=17");



            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuyNum.Checked ? "N'True'" : "N'False'") + " where ColumnId=18");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuyDate.Checked ? "N'True'" : "N'False'") + " where ColumnId=19");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuyValue.Checked ? "N'True'" : "N'False'") + " where ColumnId=20");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuyAmount.Checked ? "N'True'" : "N'False'") + " where ColumnId=21");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuyDesc.Checked ? "N'True'" : "N'False'") + " where ColumnId=22");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuyEtebar.Checked ? "N'True'" : "N'False'") + " where ColumnId=23");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuyNetPrice.Checked ? "N'True'" : "N'False'") + " where ColumnId=24");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuyTotalAmount.Checked ? "N'True'" : "N'False'") + " where ColumnId=25");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuySumDiscount.Checked ? "N'True'" : "N'False'") + " where ColumnId=26");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuySumExtra.Checked ? "N'True'" : "N'False'") + " where ColumnId=27");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuyCustomer.Checked ? "N'True'" : "N'False'") + " where ColumnId=28");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuyWare.Checked ? "N'True'" : "N'False'") + " where ColumnId=29");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chk_BuyCondition.Checked ? "N'True'" : "N'False'") + " where ColumnId=30");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chkBuyFunc.Checked ? "N'True'" : "N'False'") + " where ColumnId=31");

            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (this.chkSaleFee.Checked ? "N'True'" : "N'False'") + " where ColumnId=33");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (this.chkBuyFee.Checked ? "N'True'" : "N'False'") + " where ColumnId=32");

            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (this.chkBuyMeghdar.Checked ? "N'True'" : "N'False'") + " where ColumnId=34");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (this.chkSaleMeghdar.Checked ? "N'True'" : "N'False'") + " where ColumnId=35");

            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + txt_ZaribEmtiaz.Value + " where ColumnId=41");
            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + txt_ZaribRialiPoorsant.Value + " where ColumnId=42");

            clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=" + (chk_LinearDiscount.Checked ? "1" : "0") + " where Column00=65");

            ///



            //clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=" + (chk_ChangePrice.Checked ? "1" : "0") + " where Column00=107");
            //clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=" + (chk_CalGift.Checked ? "1" : "0") + " where Column00=108");
            //clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=" + (chk_ShowExportSanad.Checked ? "1" : "0") + " where Column00=109");
            //clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=" + (chk_ExtraMethod.Checked ? "1" : "0") + " where Column00=110");
            //if (rdb_Alert_No.Checked)
            //    clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=0 where Column00=111");
            //if (rdb_alert_Alert.Checked)
            //    clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=1 where Column00=111");
            //if (rdb_Alert_Forbid.Checked)
            //    clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=2 where Column00=111");




            ///

            Properties.Settings.Default.Save();
            Class_BasicOperation.ShowMsg("", "ثبت اطلاعات با موفقیت انجام شد", "Information");
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            //clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=" + txt_tax.Value + " where Column00=112");
            //clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=" + this.txt_taxes.Value + " where Column00=113");
            //clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=" + this.txt_buytax.Value + " where Column00=114");
            //clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_105_SystemTransactionInfo SET Column02=" + this.txt_buytaxes.Value + " where Column00=115");

            Properties.Settings.Default.tax = Convert.ToDecimal(txt_tax.Value);
            Properties.Settings.Default.taxes = Convert.ToDecimal(txt_taxes.Value);
            Properties.Settings.Default.buytax = Convert.ToDecimal(txt_buytax.Value);
            Properties.Settings.Default.buytaxes = Convert.ToDecimal(txt_buytaxes.Value);
            Properties.Settings.Default.Save();
            Class_BasicOperation.ShowMsg("", "ثبت اطلاعات با موفقیت انجام شد", "Information");
        }

        private void txt_tax_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txt_tax_KeyPress(object sender, KeyPressEventArgs e)
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

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {

                clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + (chk_TaeedSefareshat.Checked ? "N'True'" : "N'False'") + " where ColumnId=40");
                Class_BasicOperation.ShowMsg("", "ثبت اطلاعات با موفقیت انجام شد", "Information");

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        string setting = "";
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Statuse.Text.All(char.IsDigit) || mlt_ReciptBank.Text.All(char.IsDigit) || mlt_ReciptBank.Text == "" || mlt_Statuse.Text == "")
                {
                    MessageBox.Show("لطفا اطلاعات مربوط به تسویه فاکتور فروش را تنظیم کنید  ");
                    return;
                }
               
                setting+= "UPDATE Table_030_Setting SET Column02=" + (mlt_Function.Value != null && !string.IsNullOrWhiteSpace(mlt_Function.Value.ToString()) ? mlt_Function.Value : "NULL") + " where ColumnId=45 ";
                setting += "UPDATE Table_030_Setting SET Column02=" + (mlt_ReciptFunction.Value != null && !string.IsNullOrWhiteSpace(mlt_ReciptFunction.Value.ToString()) ? mlt_ReciptFunction.Value : "NULL") + " where ColumnId=49 ";
                setting += "UPDATE Table_030_Setting SET Column02=" + (multiColumnCombo1.Value != null && !string.IsNullOrWhiteSpace(multiColumnCombo1.Value.ToString()) ? multiColumnCombo1.Value : "NULL") + " where ColumnId=64 ";
                setting += "UPDATE Table_030_Setting SET Column02=" + (mlt_Statuse.Value != null && !string.IsNullOrWhiteSpace(mlt_Statuse.Value.ToString()) ? mlt_Statuse.Value : "NULL") + " where ColumnId=79 ";
                setting += "UPDATE Table_030_Setting SET Column02=" + (mlt_ReciptBank.Value != null && !string.IsNullOrWhiteSpace(mlt_ReciptBank.Value.ToString()) ? mlt_ReciptBank.Value : "NULL") + " where ColumnId=80 ";
                setting += "UPDATE Table_030_Setting SET Column02=" + (mlt_Return_status.Value != null && !string.IsNullOrWhiteSpace(mlt_Return_status.Value.ToString()) ? mlt_Return_status.Value : "NULL") + " where ColumnId=82 ";
                setting += "UPDATE Table_030_Setting SET Column02=" + (chk_showbuydescinnoot.Checked ? "N'True'" : "N'False'") + " where ColumnId=73";




                if (chk_detail.Checked)
                    setting += "UPDATE Table_030_Setting SET Column02='True' where ColumnId=63 ";
                else
                    setting += "UPDATE Table_030_Setting SET Column02='False' where ColumnId=63 ";
                Class_BasicOperation.SqlTransactionMethodExecuteNonQuery(ConBase.ConnectionString, setting);

                Class_BasicOperation.ShowMsg("", "ثبت اطلاعات با موفقیت انجام شد", "Information");

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mlt_Ware_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_Ware_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");

        }

        private void mlt_Ware_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void mlt_Function_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");


        }

        private void mlt_Function_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }


        private void mlt_Customer_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_Customer_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "name", "code");

        }

        private void mlt_Customer_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void Frm_006_Transactions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mlt_Statuse.Text.All(char.IsDigit) ||  mlt_ReciptBank.Text.All(char.IsDigit) || mlt_ReciptBank.Text=="" || mlt_Statuse.Text=="")
            {

                if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {
                    MessageBox.Show("لطفا اطلاعات مربوط به تسویه فاکتور فروش را تنظیم کنید  ");
                    return;
                }

                
            }

          

        }

        private void mlt_ReciptBank_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_ReciptBank, "Column02", "ColumnId");
        }

        private void mlt_Statuse_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Statuse, "Column02", "ColumnId");
        }

        private void mlt_Statuse_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }

      

        private void mlt_Statuse_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_ReciptBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;

            }
        }

        //private void mlt_saler_ValueChanged(object sender, EventArgs e)
        //{
        //   Class_BasicOperation.FilterMultiColumns(mlt_saler, "Column02", "Column01");
        //}

        private void mlt_saler_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;

            }
        }

        private void uiTab1_SelectedTabChanged(object sender, Janus.Windows.UI.Tab.TabEventArgs e)
        {

        }

        private void Save_Click(object sender, EventArgs e)
        {
            string cmd = "";
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
            {
                cmd += " Update Table_107_SettingPose set Column02='" + item.Cells["Column02"].Value + "' where Column03=" + mlt_BankPose.Value + " and columnid=" + item.Cells["Columnid"].Value.ToString() + "";
            }

            cmd += " Update Table_030_Setting set Column02=" + mlt_BankPose.Value + " where Columnid=81";

            Class_BasicOperation.SqlTransactionMethodScaler(ConBase.ConnectionString, cmd);
            MessageBox.Show("اطلاعات با موفقیت ذخیره شد ");

        }

        private void mlt_BankPose_ValueChanged(object sender, EventArgs e)
        {
            gridEX2.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, @"select * from Table_107_SettingPose where Column03=" + mlt_BankPose.Value + "");
        }

        private void mlt_Return_status_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_Function_KeyPress(object sender, KeyPressEventArgs e)
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

        private void mlt_ReciptFunction_KeyPress(object sender, KeyPressEventArgs e)
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

        private void multiColumnCombo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    mlt_Statuse.Select();

            }
            else
                mlt_Statuse.Select();
        }

        private void mlt_Function_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Function, "Column02", "Column01");

        }

        private void mlt_ReciptFunction_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_ReciptFunction, "Column02", "Column01");

        }

        private void multiColumnCombo1_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_ReciptFunction, "ACC_Name", "ACC_Code");

        }

        private void mlt_Return_status_ValueChanged(object sender, EventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(mlt_Return_status, "Column02", "ColumnId");

        }

        private void mlt_Function_Leave_1(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }
    }
}
