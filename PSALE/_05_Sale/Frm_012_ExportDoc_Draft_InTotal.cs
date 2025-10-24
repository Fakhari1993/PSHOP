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
    public partial class Frm_012_ExportDoc_Draft_InTotal : Form
    {
        bool _ExportDraft = false;
        bool _ExportDoc = false;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        DataSet DS = new DataSet();
        bool _BackSpace = false;
        BindingSource ExRedBindingSource = new BindingSource();
        string CommandTxt = string.Empty;
        public Frm_012_ExportDoc_Draft_InTotal(bool ExportDoc, bool ExportDraft)
        {
            InitializeComponent();
            _ExportDoc = ExportDoc;
            _ExportDraft = ExportDraft;
        }

        private void Frm_012_ExportDoc_Draft_InTotal_Load(object sender, EventArgs e)
        {
            faDatePicker1.SelectedDateTime = DateTime.Now;

            SqlDataAdapter Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(DS, "Center");
            gridEX_Detail.DropDowns["Center"].SetDataBinding(DS.Tables["Center"], "");


            Adapter = new SqlDataAdapter("select ACC_Code,ACC_Name,Control_Type,Control_Action,Control_Person,Control_Center,Control_Project from AllHeaders()", ConAcnt);
            Adapter.Fill(DS, "Header");
            mlt_DisBed.DataSource = DS.Tables["Header"];
            mlt_DisBes.DataSource = DS.Tables["Header"];
            mlt_SaleBed.DataSource = DS.Tables["Header"];
            mlt_SaleBes.DataSource = DS.Tables["Header"];
            mlt_ValueBed.DataSource = DS.Tables["Header"];
            mlt_ValueBes.DataSource = DS.Tables["Header"];

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
            Adapter.Fill(DS, "Project");
            gridEX_Detail.DropDowns["Project"].SetDataBinding(DS.Tables["Project"], "");

            Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
            DataTable GoodTable = clGood.GoodInfo();
            gridEX_Detail.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_Detail.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            Adapter = new SqlDataAdapter("Select * from Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_Detail.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

            Adapter = new SqlDataAdapter("Select * from Table_024_Discount", ConSale);
            Adapter.Fill(DS, "Extra");
            gridEX_Extra.DropDowns["Extra"].SetDataBinding(DS.Tables["Extra"], "");
            ExRedBindingSource.DataSource = DS.Tables["Extra"];

            Adapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_007_FactorBefore", ConSale);
            Adapter.Fill(DS, "Prefactor");
            gridEX_Header.DropDowns["Prefactor"].SetDataBinding(DS.Tables["Prefactor"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
            Adapter.Fill(DS, "Customer");
            gridEX_Header.DropDowns["Customer"].SetDataBinding(DS.Tables["Customer"], "");

            Adapter = new SqlDataAdapter("Select * from Table_002_SalesTypes", ConBase);
            Adapter.Fill(DS, "SaleType");
            gridEX_Header.DropDowns["SaleType"].SetDataBinding(DS.Tables["SaleType"], "");

            gridEX_Header.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from ListPeople(5)"), "");


            Adapter = new SqlDataAdapter("Select * from Table_105_SystemTransactionInfo where Column16=6", ConBase);
            Adapter.Fill(DS, "Transaction");
            mlt_SaleBed.Value = DS.Tables["Transaction"].Rows[0]["Column07"].ToString();
            mlt_SaleBes.Value = DS.Tables["Transaction"].Rows[0]["Column13"].ToString();
            mlt_DisBed.Value = DS.Tables["Transaction"].Rows[1]["Column07"].ToString();
            mlt_DisBes.Value = DS.Tables["Transaction"].Rows[1]["Column13"].ToString();

            //اگر سیستم دائمی باشد، سرفصلهای مورد نظر برای صدور ارزش گرفته میشود
            if (Class_BasicOperation._FinType)
            {
                mlt_ValueBed.Value = DS.Tables["Transaction"].Rows[4]["Column07"].ToString();
                mlt_ValueBes.Value = DS.Tables["Transaction"].Rows[4]["Column13"].ToString();
                mlt_ValueBed.Enabled = true;
                mlt_ValueBes.Enabled = true;
            }
            else
            {
                mlt_ValueBed.Enabled = false;
                mlt_ValueBes.Enabled = false;
            }


            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_001_PWHRS", ConWare);
            Adapter.Fill(DS, "Ware");
            mlt_Ware.DataSource = DS.Tables["Ware"];

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=1", ConWare);
            Adapter.Fill(DS, "Operation");
            mlt_Function.DataSource = DS.Tables["Operation"];

            this.table_010_SaleFactor_NoDoc_DraftTableAdapter.Fill(this.dataSet_Sale.Table_010_SaleFactor_NoDoc_Draft);
            this.table_011_Child1_SaleFactor_NoDoc_DraftTableAdapter.Fill(this.dataSet_Sale.Table_011_Child1_SaleFactor_NoDoc_Draft);
            this.table_012_Child2_SaleFactor_NoDoc_DraftTableAdapter.Fill(this.dataSet_Sale.Table_012_Child2_SaleFactor_NoDoc_Draft);
            this.table_010_SaleFactor_NoDoc_DraftBindingSource_PositionChanged(sender, e);


        }

        private void bt_ExportDraft_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX_Header.MoveToNewRecord();
                List<string> lsNumbers = new List<string>();
                if (gridEX_Header.GetCheckedRows().Length > 0)
                {
                    if (!_ExportDraft)
                        throw new WarningException("کاربر گرامی شما امکان صدور حواله انبار را ندارید");


                    if (mlt_Function.Text.Trim() == "" || mlt_Ware.Text.Trim() == "")
                        throw new WarningException("اطلاعات مورد نیاز جهت صدور حواله را تکمیل کنید");

                    uiProgressBar1.Value = 5;

                    //******************چک مانده کالا
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetCheckedRows())
                    {
                        gridEX_Header.MoveTo(item);

                        foreach (Janus.Windows.GridEX.GridEXRow ChildItem in gridEX_Detail.GetRows())
                        {
                            if (!clGood.IsGoodInWare(short.Parse(mlt_Ware.Value.ToString())
                                , int.Parse(ChildItem.Cells["Column02"].Value.ToString())))
                                throw new WarningException("کالای " + item.Cells["Column02"].Text.Trim() +
                                    " در انبار انتخاب شده فعال نمی باشد");
                            if (clDoc.IsGood(ChildItem.Cells["Column02"].Value.ToString()))
                            {

                                float Remain = FirstRemain(int.Parse(ChildItem.Cells["Column02"].Value.ToString()), item.Cells["Column02"].Value.ToString());
                                bool mojoodimanfi = false;
                                try
                                {
                                    using (SqlConnection ConWareGood = new SqlConnection(Properties.Settings.Default.WHRS))
                                    {

                                        ConWareGood.Open();
                                        SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                               (
                                                                                   SELECT ISNULL(Column16, 0) AS Column16
                                                                                   FROM   table_004_CommodityAndIngredients
                                                                                   WHERE  ColumnId = " + ChildItem.Cells["Column02"].Value + @"
                                                                               ),
                                                                               0
                                                                           ) AS Column16", ConWareGood);
                                        mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                                    }
                                }
                                catch
                                {
                                }
                                if (Remain < float.Parse(ChildItem.Cells["Column07"].Value.ToString()) && !mojoodimanfi)
                                {
                                    throw new WarningException("موجودی کالای " + ChildItem.Cells["Column02"].Text + " کمتر از تعداد مشخص شده در فاکتور شماره " + item.Cells["Column01"].Text + " است " + Environment.NewLine + "موجودی: " + Remain.ToString());
                                }
                            }
                        }
                    }
                    if (uiProgressBar1.Value < 100)
                        uiProgressBar1.Value += 10;
                    //***********************
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetCheckedRows())
                    {
                        gridEX_Header.MoveTo(item);

                        if ((clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", item.Cells["ColumnId"].Value.ToString()) == 0) &&
                            (clDoc.OperationalColumnValue("Table_010_SaleFactor", "column10", item.Cells["ColumnId"].Value.ToString()) == 0) &&
                            (clDoc.OperationalColumnValue("Table_010_SaleFactor", "column20", item.Cells["ColumnId"].Value.ToString()) == 0))
                        {

                            if (!clDoc.AllService(table_011_Child1_SaleFactor_NoDoc_DraftBindingSource))
                            {

                                //*********Insert Header
                                int DraftID = InsertDraftHeader1(item, 0, lsNumbers);
                                if (uiProgressBar1.Value < 100)
                                    uiProgressBar1.Value += 1;
                                //*************درج کالاها و محاسبه ارزش
                                foreach (Janus.Windows.GridEX.GridEXRow ChildItem in gridEX_Detail.GetRows())
                                {
                                    InsertDraftDetail1(ChildItem, DraftID, 0, item.Cells["Column02"].Value.ToString());
                                    if (uiProgressBar1.Value < 100)
                                        uiProgressBar1.Value += 1;
                                }
                            }
                        }
                    }

                    uiProgressBar1.Value = 100;
                    if (lsNumbers.Count > 0)
                        MessageBox.Show("ثبت حواله برای فاکتورهای زیر با موفقیت صورت گرفت" + Environment.NewLine +
                            string.Join(",", lsNumbers.ToArray()), "", MessageBoxButtons.OK, MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                    else Class_BasicOperation.ShowMsg("", "حواله ای برای فاکتوری صادر نشد", "Stop");
                    bt_Refresh_Click(sender, e);
                    uiProgressBar1.Value = 0;
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                WarningException es = new WarningException();
                if (es.GetBaseException().GetType() != ex.GetBaseException().GetType())
                {
                    bt_Refresh_Click(sender, e);
                }
            }
        }
        private int InsertDraftHeader1(Janus.Windows.GridEX.GridEXRow _HeaderItem, int DocID, List<string> lsNumbers)
        {
            int _DraftID = 0;
            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
            Key.Direction = ParameterDirection.Output;
            int _DraftNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01");
            //, int.Parse(mlt_Ware.Value.ToString()));
            using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                conware.Open();
                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_007_PwhrsDraft ([column01]
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
                                                                                               ,[Column26]) VALUES(" +
                    _DraftNum + ",'" + _HeaderItem.Cells["Column02"].Value.ToString() + "'," +
                    mlt_Ware.Value.ToString()
                       + "," + mlt_Function.Value.ToString() + "," +
                       _HeaderItem.Cells["Column03"].Value.ToString() +
                       ",'" + "حواله صادره بابت فاکتور فروش ش" +
                       _HeaderItem.Cells["Column01"].Value.ToString() +
                       "'," + DocID + ",'" + Class_BasicOperation._UserName +
                       "',getdate(),'" + Class_BasicOperation._UserName +
                       "',getdate(),0,NULL,NULL,0," + _HeaderItem.Cells["ColumnId"].Value.ToString() + ",0," +
                       _HeaderItem.Cells["Column07"].Value.ToString().Trim() +
                       ",0,0,0,0,0,null,0,1); SET @Key=SCOPE_IDENTITY()", conware);
                Insert.Parameters.Add(Key);
                Insert.ExecuteNonQuery();
                _DraftID = int.Parse(Key.Value.ToString());


                clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_010_SaleFactor SET Column09=" +
                                                  _DraftID + "  where ColumnId=" +
                                                  _HeaderItem.Cells["ColumnId"].Value.ToString());
                lsNumbers.Add(_HeaderItem.Cells["Column01"].Value.ToString());

                return _DraftID;
            }
        }
        private void InsertDraftHeader(Janus.Windows.GridEX.GridEXRow _HeaderItem, int DocID, List<string> lsNumbers)
        {
            //            int _DraftID = 0;
            //            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
            //            Key.Direction = ParameterDirection.Output;
            //            int _DraftNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01");
            //                //, int.Parse(mlt_Ware.Value.ToString()));
            //            using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.WHRS))
            //            {
            //                conware.Open();
            //                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_007_PwhrsDraft ([column01]
            //                                                                                               ,[column02]
            //                                                                                               ,[column03]
            //                                                                                               ,[column04]
            //                                                                                               ,[column05]
            //                                                                                               ,[column06]
            //                                                                                               ,[column07]
            //                                                                                               ,[column08]
            //                                                                                               ,[column09]
            //                                                                                               ,[column10]
            //                                                                                               ,[column11]
            //                                                                                               ,[column12]
            //                                                                                               ,[column13]
            //                                                                                               ,[column14]
            //                                                                                               ,[column15]
            //                                                                                               ,[column16]
            //                                                                                               ,[column17]
            //                                                                                               ,[column18]
            //                                                                                               ,[column19]
            //                                                                                               ,[Column20]
            //                                                                                               ,[Column21]
            //                                                                                               ,[Column22]
            //                                                                                               ,[Column23]
            //                                                                                               ,[Column24]
            //                                                                                               ,[Column25]
            //                                                                                               ,[Column26]) VALUES(" +
            //                    _DraftNum + ",'" + _HeaderItem.Cells["Column02"].Value.ToString() + "'," +
            //                    mlt_Ware.Value.ToString()
            //                       + "," + mlt_Function.Value.ToString() + "," +
            //                       _HeaderItem.Cells["Column03"].Value.ToString() +
            //                       ",'" + "حواله صادره بابت فاکتور فروش ش" +
            //                       _HeaderItem.Cells["Column01"].Value.ToString() +
            //                       "'," + DocID + ",'" + Class_BasicOperation._UserName +
            //                       "',getdate(),'" + Class_BasicOperation._UserName +
            //                       "',getdate(),0,NULL,NULL,0," + _HeaderItem.Cells["ColumnId"].Value.ToString() + ",0," +
            //                       _HeaderItem.Cells["Column07"].Value.ToString().Trim() +
            //                       ",0,0,0,0,0,null,0,1); SET @Key=SCOPE_IDENTITY()", conware);
            //                Insert.Parameters.Add(Key);
            //                Insert.ExecuteNonQuery();
            //                _DraftID = int.Parse(Key.Value.ToString());


            //                clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_010_SaleFactor SET Column09=" +
            //                                                  _DraftID + "  where ColumnId=" +
            //                                                  _HeaderItem.Cells["ColumnId"].Value.ToString());
            //                lsNumbers.Add(_HeaderItem.Cells["Column01"].Value.ToString());

            //                return _DraftID;
            //            }

            CommandTxt += @"INSERT INTO " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ([column01]
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
                                                                                               ,[Column26]) VALUES( (SELECT ISNULL(Max( Column01  ),0)+1 as ID from " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ),'" + _HeaderItem.Cells["Column02"].Value.ToString() + "'," +
                    mlt_Ware.Value.ToString()
                       + "," + mlt_Function.Value.ToString() + "," +
                       _HeaderItem.Cells["Column03"].Value.ToString() +
                       ",'" + "حواله صادره بابت فاکتور فروش ش" +
                       _HeaderItem.Cells["Column01"].Value.ToString() +
                       "',0,'" + Class_BasicOperation._UserName +
                       "',getdate(),'" + Class_BasicOperation._UserName +
                       "',getdate(),0,NULL,NULL,0," + _HeaderItem.Cells["ColumnId"].Value.ToString() + ",0," +
                       _HeaderItem.Cells["Column07"].Value.ToString().Trim() +
                       ",0,0,0,0,0,null,0,1); SET @DraftID=SCOPE_IDENTITY()";
        }
        private double InsertDraftDetail1(Janus.Windows.GridEX.GridEXRow item, int DraftID, double TotalValue, string date)
        {
            SqlParameter DetailIDParam = new SqlParameter("Key", SqlDbType.Int);
            DetailIDParam.Direction = ParameterDirection.Output;

            //درج کالاهای موجود در حواله 
            using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                conware.Open();
                SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_008_Child_PwhrsDraft ([column01]
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
           ,[Column35]) VALUES(" + DraftID + "," + item.Cells["Column02"].Value.ToString() + "," +
                    item.Cells["Column03"].Value.ToString() + "," + item.Cells["Column04"].Value.ToString() + "," + item.Cells["Column05"].Value.ToString() + "," + item.Cells["Column06"].Value.ToString() + "," +
                    item.Cells["Column07"].Value.ToString() + "," + item.Cells["Column08"].Value.ToString() + "," + item.Cells["Column09"].Value.ToString() + "," + item.Cells["Column10"].Value.ToString() + "," +
                    item.Cells["Column11"].Value.ToString() + ",NULL,NULL," + (item.Cells["Column22"].Text.ToString().Trim() == "" ? "NULL" : item.Cells["Column22"].Value.ToString())
                    + ",0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL,NULL,0," + item.Cells["ColumnId"].Value.ToString() +
                        ",0,0,0," + (item.Cells["Column30"].Value.ToString() == "True" ? "1" : "0") + "," +
                        (item.Cells["Column34"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column34"].Value.ToString().Trim() + "'")
                                + "," + (item.Cells["Column35"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column35"].Value.ToString().Trim() + "'") +
                                "," + item.Cells["Column31"].Value.ToString()
                        + "," + item.Cells["Column32"].Value.ToString() + "," + item.Cells["Column36"].Value.ToString() + "," + item.Cells["Column37"].Value.ToString() + "); SET @Key=SCOPE_IDENTITY()", conware);
                InsertDetail.Parameters.Add(DetailIDParam);
                InsertDetail.ExecuteNonQuery();
                int DetailID = int.Parse(DetailIDParam.Value.ToString());

                //محاسبه ارزش
                if (Class_BasicOperation._WareType)
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO] " : "  [dbo].[PR_05_AVG] ") + "   @GoodParameter = " + item.Cells["Column02"].Value.ToString() + ", @WareCode = " + mlt_Ware.Value.ToString(), conware);
                    DataTable TurnTable = new DataTable();
                    Adapter.Fill(TurnTable);
                    DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftID + " and DetailID=" + DetailID);
                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                        + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + DetailID, conware);
                    UpdateCommand.ExecuteNonQuery();
                    TotalValue += Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4);

                }
                else
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item.Cells["Column02"].Value.ToString() + ", @WareCode = " + mlt_Ware.Value.ToString() + ",@Date='" + date + "',@id=" + DraftID + ",@residid=0", ConWare);
                    DataTable TurnTable = new DataTable();
                    Adapter.Fill(TurnTable);
                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                  + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item.Cells["Column07"].Value.ToString()), 4) + " where ColumnId=" + DetailID, conware);
                    UpdateCommand.ExecuteNonQuery();
                }



                return TotalValue;
            }
        }
        private void InsertDraftDetail(Janus.Windows.GridEX.GridEXRow item, int DraftID, double TotalValue, string date)
        {
            //SqlParameter DetailIDParam = new SqlParameter("Key", SqlDbType.Int);
            //DetailIDParam.Direction = ParameterDirection.Output;

            ////درج کالاهای موجود در حواله 
            //using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.WHRS))
            //{
            //    conware.Open();
            //    SqlCommand InsertDetail = new SqlCommand("INSERT INTO Table_008_Child_PwhrsDraft VALUES(" + DraftID + "," + item.Cells["Column02"].Value.ToString() + "," +
            //        item.Cells["Column03"].Value.ToString() + "," + item.Cells["Column04"].Value.ToString() + "," + item.Cells["Column05"].Value.ToString() + "," + item.Cells["Column06"].Value.ToString() + "," +
            //        item.Cells["Column07"].Value.ToString() + "," + item.Cells["Column08"].Value.ToString() + "," + item.Cells["Column09"].Value.ToString() + "," + item.Cells["Column10"].Value.ToString() + "," +
            //        item.Cells["Column11"].Value.ToString() + ",NULL,NULL," + (item.Cells["Column22"].Text.ToString().Trim() == "" ? "NULL" : item.Cells["Column22"].Value.ToString())
            //        + ",0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL,NULL,0," + item.Cells["ColumnId"].Value.ToString() +
            //            ",0,0,0," + (item.Cells["Column30"].Value.ToString() == "True" ? "1" : "0") + "," +
            //            (item.Cells["Column34"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column34"].Value.ToString().Trim() + "'")
            //                    + "," + (item.Cells["Column35"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column35"].Value.ToString().Trim() + "'") +
            //                    "," + item.Cells["Column31"].Value.ToString()
            //            + "," + item.Cells["Column32"].Value.ToString() + "," + item.Cells["Column36"].Value.ToString() + "," + item.Cells["Column37"].Value.ToString() + "); SET @Key=SCOPE_IDENTITY()", conware);
            //    InsertDetail.Parameters.Add(DetailIDParam);
            //    InsertDetail.ExecuteNonQuery();
            //    int DetailID = int.Parse(DetailIDParam.Value.ToString());

            //    //محاسبه ارزش

            //        SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	"+(Class_BasicOperation._WareType? " [dbo].[PR_00_NewFIFO] ":"  [dbo].[PR_05_AVG] ")+"   @GoodParameter = " + item.Cells["Column02"].Value.ToString() + ", @WareCode = " + mlt_Ware.Value.ToString(), conware);
            //        DataTable TurnTable = new DataTable();
            //        Adapter.Fill(TurnTable);
            //        DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftID + " and DetailID=" + DetailID);
            //        SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
            //            + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + DetailID, conware);
            //        UpdateCommand.ExecuteNonQuery();
            //        TotalValue += Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4);

            //    return TotalValue;
            //}
            #region Moallemi
            //اضافه شدن لیست ستونها به کامند تکست
            #endregion
            CommandTxt += @"  INSERT INTO " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft ([column01]
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
           ,[Column35]) VALUES(@DraftID  ," + item.Cells["Column02"].Value.ToString() + "," +
                    item.Cells["Column03"].Value.ToString() + "," + item.Cells["Column04"].Value.ToString() + "," + item.Cells["Column05"].Value.ToString() + "," + item.Cells["Column06"].Value.ToString() + "," +
                    item.Cells["Column07"].Value.ToString() + "," + item.Cells["Column08"].Value.ToString() + "," + item.Cells["Column09"].Value.ToString() + "," + item.Cells["Column10"].Value.ToString() + "," +
                    item.Cells["Column11"].Value.ToString() + ",NULL,NULL," + (item.Cells["Column22"].Text.ToString().Trim() == "" ? "NULL" : item.Cells["Column22"].Value.ToString())
                    + ",0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL,NULL,0," + item.Cells["ColumnId"].Value.ToString() +
                        ",0,0,0," + (item.Cells["Column30"].Value.ToString() == "True" ? "1" : "0") + "," +
                        (item.Cells["Column34"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column34"].Value.ToString().Trim() + "'")
                                + "," + (item.Cells["Column35"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column35"].Value.ToString().Trim() + "'") +
                                "," + item.Cells["Column31"].Value.ToString()
                        + "," + item.Cells["Column32"].Value.ToString() + "," + item.Cells["Column36"].Value.ToString() + "," + item.Cells["Column37"].Value.ToString() + "); SET @DetailID=SCOPE_IDENTITY()";


            CommandTxt += @"        if '" + Convert.ToBoolean(Class_BasicOperation._WareType).ToString() + @"'='True'
                                    BEGIN
                                    INSERT INTO @FIFOTurnTable
                                   EXEC	" + ConWare.Database + ".[dbo].[PR_00_NewFIFO]    @GoodParameter = " + item.Cells["Column02"].Value.ToString() + ", @WareCode = " + mlt_Ware.Value.ToString() + @"
  
                                    UPDATE " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft
                                    SET    Column15  = j.DsinglePrice,
                                           Column16  = j.DTotalPrice
                                    FROM   " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft
                                           JOIN @FIFOTurnTable AS j
                                                ON  j.DetailID = " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.columnid
                                    WHERE " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.columnid=@DetailID and j.Kind=2 AND j.ID=@DraftID end
                                     else begin  INSERT INTO @AVGTurnTable
                                   EXEC	" + ConWare.Database + ".[dbo].[PR_05_NewAVG]    @GoodParameter = " + item.Cells["Column02"].Value.ToString() + ", @WareCode = " + mlt_Ware.Value.ToString() + @",@Date='" + date + @"',@id=@DraftID,@residid=0
  
                                    UPDATE " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft
                                    SET    Column15  = (SELECT  TOP 1 DsinglePrice FROM @AVGTurnTable ),
                                           Column16  = (SELECT  TOP 1 DsinglePrice FROM @AVGTurnTable ) * " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft.column07
                                     where columnid=@DetailID
                                            end ";
        }

        private float FirstRemain(int GoodCode, string _Date)
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
                CommandText = string.Format(CommandText, mlt_Ware.Value.ToString(), GoodCode, _Date);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            uiProgressBar1.Value = 0;
            try
            {
                gridEX_Header.MoveToNewRecord();
                if (gridEX_Header.GetCheckedRows().Length > 0)
                {
                    if (!_ExportDoc)
                        throw new WarningException("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");

                    //اعمال کنترلهای مورد نیاز
                    CheckEssentialItems(sender, e);

                    if (gridEX_Header.Find(gridEX_Header.RootTable.Columns["ErrorDes"], Janus.Windows.GridEX.ConditionOperator.NotIsNull, null, null, -1, 1))
                    {
                        gridEX_Header_SelectionChanged(sender, e);
                        Class_BasicOperation.ShowMsg("", "با توجه به هشدارهای نمایش داده شده امکان صدور سند حسابداری وجود ندارد", "Warning");
                        return;
                    }
                    uiProgressBar1.Value = 10;

                    if (DialogResult.Yes == MessageBox.Show(
                        "آیا مایل به صدور سند حسابداری و حواله انبار هستید؟", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        bt_ExportDoc.Enabled = false;

                        //******************چک مانده کالا
                        foreach (Janus.Windows.GridEX.GridEXRow item1 in gridEX_Header.GetCheckedRows())
                        {
                            gridEX_Header.MoveTo(item1);

                            foreach (Janus.Windows.GridEX.GridEXRow ChildItem in gridEX_Detail.GetRows())
                            {
                                if (clDoc.IsGood(ChildItem.Cells["Column02"].Value.ToString()))
                                {
                                    bool mojoodimanfi = false;
                                    try
                                    {
                                        using (SqlConnection ConWareGood = new SqlConnection(Properties.Settings.Default.WHRS))
                                        {

                                            ConWareGood.Open();
                                            SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                               (
                                                                                   SELECT ISNULL(Column16, 0) AS Column16
                                                                                   FROM   table_004_CommodityAndIngredients
                                                                                   WHERE  ColumnId = " + ChildItem.Cells["Column02"].Value + @"
                                                                               ),
                                                                               0
                                                                           ) AS Column16", ConWareGood);
                                            mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                                        }
                                    }
                                    catch
                                    {
                                    }
                                    float Remain = FirstRemain(int.Parse(ChildItem.Cells["Column02"].Value.ToString()), item1.Cells["Column02"].Value.ToString());
                                    if (Remain < float.Parse(ChildItem.Cells["Column07"].Value.ToString()) && !mojoodimanfi)
                                    {
                                        bt_ExportDoc.Enabled = true;

                                        throw new WarningException("موجودی کالای " + ChildItem.Cells["Column02"].Text + " کمتر از تعداد مشخص شده در فاکتور شماره " + item1.Cells["Column01"].Text + " است " + Environment.NewLine + "موجودی: " + Remain.ToString());
                                    }
                                }
                            }
                        }
                        List<string> lsNumbers = new List<string>();
                        //صدور سند
                        //int DocNum = 0;
                        //int DocID = 0;
                        //if (rdb_last.Checked)
                        //{
                        //    DocNum = clDoc.LastDocNum();
                        //    DocID = clDoc.DocID(DocNum);
                        //}
                        //else if (rdb_To.Checked)
                        //{
                        //    DocNum = int.Parse(txt_To.Text.Trim());
                        //    DocID = clDoc.DocID(DocNum);
                        //}
                        string notok = string.Empty;
                        string headercomman = string.Empty;
                        SqlParameter DocNum;
                        DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                        DocNum.Direction = ParameterDirection.Output;
                        SqlParameter DocID;
                        DocID = new SqlParameter("DocID", SqlDbType.Int);
                        DocID.Direction = ParameterDirection.Output;
                        if (rdb_last.Checked)
                        {
                            //DocNum = clDoc.LastDocNum();
                            //DocID = clDoc.DocID(DocNum);
                            headercomman = " set @DocNum=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0)))";

                        }
                        else if (rdb_To.Checked)
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
                            foreach (Janus.Windows.GridEX.GridEXRow item1 in gridEX_Header.GetCheckedRows())
                            {

                                CommandTxt = @"declare @Key int declare @DetailID int declare @DraftID int declare @TotalValue decimal(18,3) declare @value decimal(18,3) DECLARE @FIFOTurnTable TABLE (
                                                RowNumber INT,
                                                Kind NVARCHAR(1),
                                                ID INT,
                                                DetailID INT,
                                                Date NVARCHAR(10),
                                                GoodID INT,
                                                RNumber DECIMAL(18, 4),
                                                RsinglePrice DECIMAL(18, 4),
                                                RTotalPrice DECIMAL(18, 4),
                                                DNumber DECIMAL(18, 4),
                                                DSinglePrice DECIMAL(18, 4),
                                                DTotalPrice DECIMAL(18, 4),
                                                Remain DECIMAL(18, 4),
                                                RemainFee DECIMAL(18, 4),
                                                TotalFee DECIMAL(18, 4),
                                                LastRemain DECIMAL(18, 4),
                                                Marjoo INT
                                            )  	DECLARE @AVGTurnTable TABLE ( DSinglePrice DECIMAL(18, 4))";

                                if ((clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", item1.Cells["ColumnId"].Value.ToString()) == 0) &&
                               (clDoc.OperationalColumnValue("Table_010_SaleFactor", "column10", item1.Cells["ColumnId"].Value.ToString()) == 0) &&
                               (clDoc.OperationalColumnValue("Table_010_SaleFactor", "column20", item1.Cells["ColumnId"].Value.ToString()) == 0))
                                {
                                    if (uiProgressBar1.Value < 100)
                                        uiProgressBar1.Value += 1;
                                    gridEX_Header.MoveTo(item1);


                                    if (uiProgressBar1.Value < 100)
                                        uiProgressBar1.Value += 1;
                                    //صدور حواله
                                    int DraftID = 0;
                                    double TotalValue = 0;
                                    if (!clDoc.AllService(table_011_Child1_SaleFactor_NoDoc_DraftBindingSource))
                                    {
                                        InsertDraftHeader(item1, Convert.ToInt32(DocID.Value), lsNumbers);
                                        if (uiProgressBar1.Value < 100)
                                            uiProgressBar1.Value += 1;


                                        //*************درج کالاها و محاسبه ارزش
                                        foreach (Janus.Windows.GridEX.GridEXRow ChildItem in gridEX_Detail.GetRows())
                                        {
                                            if (clDoc.IsGood(ChildItem.Cells["Column02"].Value.ToString()))
                                            {

                                                InsertDraftDetail(ChildItem, DraftID, TotalValue, item1.Cells["Column02"].Value.ToString());
                                                if (uiProgressBar1.Value < 100)
                                                    uiProgressBar1.Value += 1;
                                            }
                                        }
                                        CommandTxt += "  UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column09=@DraftID where ColumnId=" + item1.Cells["ColumnId"].Value.ToString();

                                    }
                                    Int64 NetPrice = Convert.ToInt64(Convert.ToDouble(item1.Cells["Column28"].Value.ToString()));
                                    Int64 TotalDiscount = Convert.ToInt64(Convert.ToDouble(item1.Cells["Column29"].Value.ToString())) +
                                        Convert.ToInt64(Convert.ToDouble(item1.Cells["Column30"].Value.ToString())) +
                                        Convert.ToInt64(Convert.ToDouble(item1.Cells["Column31"].Value.ToString()));
                                    Int64 TotalExtra = Convert.ToInt64(Convert.ToDouble(item1.Cells["Column32"].Value.ToString()));
                                    Int64 Reduction = Convert.ToInt64(Convert.ToDouble(item1.Cells["Column33"].Value.ToString()));

                                    //ثبت مربوط به خالص فاکتور فروش
                                    if (NetPrice > 0)
                                    {
                                        //بدهکار
                                        string[] _Bed = clDoc.ACC_Info(mlt_SaleBed.Value.ToString());
                                        string[] _Bes = clDoc.ACC_Info(mlt_SaleBes.Value.ToString());

                                        //clDoc.ExportDoc_Detail(DocID, mlt_SaleBed.Value.ToString(),
                                        //    short.Parse(_Bed[0]), _Bed[1], _Bed[2], _Bed[3], _Bed[4],
                                        //    item1.Cells["Column03"].Value.ToString(), "NULL", "NULL", "فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                        //    NetPrice, 0, 0, 0, -1, 15, int.Parse(
                                        //    item1.Cells["ColumnId"].Value.ToString()),
                                        //    Class_BasicOperation._UserName, 0);


                                        CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                    VALUES(" + DocID.Value + ",'" + mlt_SaleBed.Value.ToString() + @"',
                                " + short.Parse(_Bed[0]) + ",'" + _Bed[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bed[2].ToString()) ? "NULL" : "'" + _Bed[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[3].ToString()) ? "NULL" : "'" + _Bed[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[4].ToString()) ? "NULL" : "'" + _Bed[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item1.Cells["Column03"].Text.Trim()) ? "NULL" : item1.Cells["Column03"].Value.ToString()) + @",
                                 NULL ,
                                 NULL ,
                   " + "'فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                        " + NetPrice + @",
                        0,0,0,-1,15," + item1.Cells["ColumnId"].Value.ToString() + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                        Class_BasicOperation._UserName + "',getdate(),0); ";

                                        //بستانکار
                                        SqlDataAdapter Adapter = new SqlDataAdapter(
        @"SELECT     Project, Total, Discount, Adding, column01, Total - Discount + Adding AS Net
                             FROM         (SELECT     column22 AS Project, ISNULL(SUM(column11), 0) AS Total, ISNULL(SUM(column17), 0) AS Discount, ISNULL(SUM(column19), 0) AS Adding, column01
                             FROM          dbo.Table_011_Child1_SaleFactor
                             GROUP BY column22, column01
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                                        Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, item1.Cells["ColumnId"].Value.ToString());
                                        DataTable Table = new DataTable();
                                        Adapter.Fill(Table);
                                        foreach (DataRow GroupRow in Table.Rows)
                                        {
                                            //clDoc.ExportDoc_Detail(DocID, mlt_SaleBes.Value.ToString(), short.Parse(_Bes[0]), _Bes[1], _Bes[2], _Bes[3], _Bes[4], "NULL", "NULL",
                                            //    (GroupRow["Project"].ToString().Trim() == "" ? null : GroupRow["Project"].ToString()), "فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                            //    0, Convert.ToInt64(Convert.ToDouble(GroupRow["Net"].ToString())), 0, 0, -1, 15, int.Parse(item1.Cells["ColumnID"].Value.ToString()), Class_BasicOperation._UserName, 0);

                                            CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                    VALUES(" + DocID.Value + ",'" + mlt_SaleBes.Value.ToString() + @"',
                                " + Int16.Parse(_Bes[0].ToString()) + ",'" + _Bes[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bes[2].ToString()) ? "NULL" : "'" + _Bes[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[3].ToString()) ? "NULL" : "'" + _Bes[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[4].ToString()) ? "NULL" : "'" + _Bes[4].ToString() + "'") + @",
                                 NULL ,
                                 NULL ,
                               " + (string.IsNullOrWhiteSpace(GroupRow["Project"].ToString().Trim()) ? "NULL" : GroupRow["Project"].ToString()) + @",
                   " + "'فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                        0,
                        " + Convert.ToInt64(Convert.ToDouble(GroupRow["Net"].ToString())) + ",0,0,-1,15," + int.Parse(item1.Cells["ColumnID"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                            Class_BasicOperation._UserName + "',getdate(),0); ";

                                        }
                                        if (uiProgressBar1.Value < 100)
                                            uiProgressBar1.Value += 1;
                                    }

                                    //ثبت مربوط به تخفیفات انتهای فاکتور
                                    if (TotalDiscount > 0)
                                    {
                                        string[] _Bed = clDoc.ACC_Info(mlt_DisBed.Value.ToString());
                                        string[] _Bes = clDoc.ACC_Info(mlt_DisBes.Value.ToString());

                                        //clDoc.ExportDoc_Detail(DocID, mlt_DisBed.Value.ToString(), short.Parse(_Bed[0]), _Bed[1], _Bed[2], _Bed[3], _Bed[4],
                                        //   "NULL", "NULL", "NULL", "تخفیفات انتهای فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                        //   TotalDiscount, 0, 0, 0, -1, 15, int.Parse(item1.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0);

                                        CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                             ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                          VALUES(" + DocID.Value + ",'" + mlt_DisBed.Value.ToString() + @"',
                                                " + Int16.Parse(_Bed[0].ToString()) + ",'" + _Bed[1].ToString() + @"',
                                                " + (string.IsNullOrEmpty(_Bed[2].ToString()) ? "NULL" : "'" + _Bed[2].ToString() + "'") + @",
                                                " + (string.IsNullOrEmpty(_Bed[3].ToString()) ? "NULL" : "'" + _Bed[3].ToString() + "'") + @",
                                                " + (string.IsNullOrEmpty(_Bed[4].ToString()) ? "NULL" : "'" + _Bed[4].ToString() + "'") + @",
                                                 NULL ,
                                                 NULL ,
                                                NULL ,
                                    ' تخفیفات انتهای فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                        " + TotalDiscount + @",
                                       0,0,0,-1,15," + int.Parse(item1.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                          Class_BasicOperation._UserName + "',getdate(),0); ";




                                        //clDoc.ExportDoc_Detail(DocID, mlt_DisBes.Value.ToString(), short.Parse(_Bes[0]), _Bes[1], _Bes[2], _Bes[3], _Bes[4],
                                        // item1.Cells["Column03"].Value.ToString(), "NULL", "NULL", "تخفیفات انتهای فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                        //  0, TotalDiscount, 0, 0, -1, 15, int.Parse(item1.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0);

                                        CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                            ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                 VALUES(" + DocID.Value + ",'" + mlt_DisBes.Value.ToString() + @"',
                                " + Int16.Parse(_Bes[0].ToString()) + ",'" + _Bes[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bes[2].ToString()) ? "NULL" : "'" + _Bes[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[3].ToString()) ? "NULL" : "'" + _Bes[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[4].ToString()) ? "NULL" : "'" + _Bes[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item1.Cells["Column03"].Value.ToString()) ? "NULL" : item1.Cells["Column03"].Value.ToString()) + @",
                                 NULL ,
                                NULL ,
                              ' تخفیفات انتهای فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                    0,
                                   " + TotalDiscount + ",0,0,-1,15," + int.Parse(item1.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                      Class_BasicOperation._UserName + "',getdate(),0); ";

                                        if (uiProgressBar1.Value < 100)
                                            uiProgressBar1.Value += 1;
                                    }
                                    //ثبت مربوط به اضافات و کسورات
                                    foreach (Janus.Windows.GridEX.GridEXRow item3 in gridEX_Extra.GetRows())
                                    {
                                        ExRedBindingSource.Filter = "ColumnId=" + item3.Cells["Column02"].Value.ToString();

                                        // کسورات
                                        if (item3.Cells["Column05"].Value.ToString() == "True")
                                        {
                                            string[] _Bed = clDoc.ACC_Info(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString());
                                            string[] _Bes = clDoc.ACC_Info(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString());

                                            //clDoc.ExportDoc_Detail(DocID, ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString(), short.Parse(_Bed[0]),
                                            //    _Bed[1], _Bed[2], _Bed[3], _Bed[4], null, null, null, item3.Cells["Column02"].Text + "-" + " فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                            //    Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())), 0, 0, 0, -1, 15, int.Parse(item1.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0);



                                            CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                            ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                 VALUES(" + DocID.Value + ",'" + ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString() + @"',
                                " + Int16.Parse(_Bed[0].ToString()) + ",'" + _Bed[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bed[2].ToString()) ? "NULL" : "'" + _Bed[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[3].ToString()) ? "NULL" : "'" + _Bed[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[4].ToString()) ? "NULL" : "'" + _Bed[4].ToString() + "'") + @",
                                 NULL ,
                                 NULL ,
                                NULL ,
                              ' تخفیف فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                   " + Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())) + @",
                                   0,0,0,-1,15," + int.Parse(item1.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                     Class_BasicOperation._UserName + "',getdate(),0); ";




                                            //clDoc.ExportDoc_Detail(DocID, ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString(), short.Parse(_Bes[0]),
                                            //    _Bes[1], _Bes[2], _Bes[3], _Bes[4], item1.Cells["Column03"].Value.ToString(), null, null, item3.Cells["Column02"].Text + "-" + " فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                            //    0, Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())), 0, 0, -1, 15, int.Parse(item1.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0);

                                            CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                            ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                 VALUES(" + DocID.Value + ",'" + ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString() + @"',
                                " + Int16.Parse(_Bes[0].ToString()) + ",'" + _Bes[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bes[2].ToString()) ? "NULL" : "'" + _Bes[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[3].ToString()) ? "NULL" : "'" + _Bes[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[4].ToString()) ? "NULL" : "'" + _Bes[4].ToString() + "'") + @",
                              " + (item3.Cells["column07"].Value != null && !string.IsNullOrWhiteSpace(item3.Cells["column07"].Value.ToString()) ? item3.Cells["column07"].Value.ToString() : (string.IsNullOrWhiteSpace(item1.Cells["Column03"].Value.ToString()) ? "NULL" : item1.Cells["Column03"].Value.ToString())) + @",

                                 NULL ,
                                NULL ,
                              'تخفیف فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                  0,
                                   " + Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())) + ",0,0,-1,15," + int.Parse(item1.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                      Class_BasicOperation._UserName + "',getdate(),0); ";


                                        }
                                        else if (item3.Cells["Column05"].Value.ToString() == "False")
                                        {
                                            string[] _Bed = clDoc.ACC_Info(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString());
                                            string[] _Bes = clDoc.ACC_Info(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString());

                                            //clDoc.ExportDoc_Detail(DocID, ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString(), short.Parse(_Bed[0]),
                                            //    _Bed[1], _Bed[2], _Bed[3], _Bed[4], item1.Cells["Column03"].Value.ToString(), null, null, item3.Cells["Column02"].Text + "-" + " فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                            //    Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())), 0, 0, 0, -1, 15, int.Parse(item1.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0);






                                            CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                            ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                 VALUES(" + DocID.Value + ",'" + ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString() + @"',
                                " + Int16.Parse(_Bed[0].ToString()) + ",'" + _Bed[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bed[2].ToString()) ? "NULL" : "'" + _Bed[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[3].ToString()) ? "NULL" : "'" + _Bed[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[4].ToString()) ? "NULL" : "'" + _Bed[4].ToString() + "'") + @",
                              " + (item3.Cells["column07"].Value != null && !string.IsNullOrWhiteSpace(item3.Cells["column07"].Value.ToString()) ? item3.Cells["column07"].Value.ToString() : (string.IsNullOrWhiteSpace(item1.Cells["Column03"].Value.ToString()) ? "NULL" : item1.Cells["Column03"].Value.ToString())) + @",

                                 NULL ,
                                NULL ,
                              '" + item3.Cells["Column02"].Text + "-" + " اضافه فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                  " + Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())) + @",
                                   0,0,0,-1,15," + int.Parse(item1.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                     Class_BasicOperation._UserName + "',getdate(),0); ";









                                            //clDoc.ExportDoc_Detail(DocID, ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString(), short.Parse(_Bes[0]),
                                            //    _Bes[1], _Bes[2], _Bes[3], _Bes[4], null, null, null, item3.Cells["Column02"].Text + "-" + " فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                            //    0, Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())), 0, 0, -1, 15, int.Parse(item1.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0);






                                            CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                            ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                 VALUES(" + DocID.Value + ",'" + ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString() + @"',
                                " + Int16.Parse(_Bes[0].ToString()) + ",'" + _Bes[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bes[2].ToString()) ? "NULL" : "'" + _Bes[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[3].ToString()) ? "NULL" : "'" + _Bes[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[4].ToString()) ? "NULL" : "'" + _Bes[4].ToString() + "'") + @",
                              NULL,
                                 NULL ,
                                NULL ,
                              ' اضافه فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                  0,
                                   " + Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())) + ",0,0,-1,15," + int.Parse(item1.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                        Class_BasicOperation._UserName + "',getdate(),0); ";





                                        }
                                        if (uiProgressBar1.Value < 100)
                                            uiProgressBar1.Value += 1;
                                    }

                                    //ثبت مربوط به ارزش حواله
                                    if (mlt_ValueBed.Enabled && mlt_ValueBes.Enabled)
                                    {

                                        CommandTxt += "SET @TotalValue=( select isnull((SELECT SUM(ISNULL(Column16, 0))FROM   " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft WHERE  column01 = @DraftID),0))";
                                        string[] _Bed = clDoc.ACC_Info(mlt_ValueBed.Value.ToString());
                                        string[] _Bes = clDoc.ACC_Info(mlt_ValueBes.Value.ToString());

                                        //clDoc.ExportDoc_Detail(DocID, mlt_ValueBed.Value.ToString(), short.Parse(_Bed[0]), _Bed[1], _Bed[2], _Bed[3], _Bed[4], null, null, null,
                                        // "بهای تمام شده- فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                        // Convert.ToInt64(TotalValue), 0, 0, 0, -1, 26, DraftID, Class_BasicOperation._UserName, 0);

                                        CommandTxt += @" if @TotalValue>0  begin    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                            ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                 VALUES(" + DocID.Value + ",'" + mlt_ValueBed.Value.ToString() + @"',
                                " + Int16.Parse(_Bed[0].ToString()) + ",'" + _Bed[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bed[2].ToString()) ? "NULL" : "'" + _Bed[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[3].ToString()) ? "NULL" : "'" + _Bed[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[4].ToString()) ? "NULL" : "'" + _Bed[4].ToString() + "'") + @",
                              NULL,
                                 NULL ,
                                NULL ,
                              'بهای تمام شده- فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                 @TotalValue,
                                   0,0,0,-1,26,@DraftID,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                       Class_BasicOperation._UserName + @"',getdate(),0);  INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                            ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                 VALUES(" + DocID.Value + ",'" + mlt_ValueBes.Value.ToString() + @"',
                                " + Int16.Parse(_Bes[0].ToString()) + ",'" + _Bes[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bes[2].ToString()) ? "NULL" : "'" + _Bes[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[3].ToString()) ? "NULL" : "'" + _Bes[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[4].ToString()) ? "NULL" : "'" + _Bes[4].ToString() + "'") + @",
                              NULL,  NULL , NULL ,
                                 'بهای تمام شده- فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                 0,
                                   @TotalValue,0,0,-1,26,@DraftID,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                       Class_BasicOperation._UserName + "',getdate(),0); end  ";



                                        //clDoc.ExportDoc_Detail(DocID, mlt_ValueBes.Value.ToString(), short.Parse(_Bes[0]), _Bes[1], _Bes[2], _Bes[3], _Bes[4], null, null, null,
                                        //  "بهای تمام شده- فاکتور فروش ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                        //  0, Convert.ToInt64(TotalValue), 0, 0, -1, 26, DraftID, Class_BasicOperation._UserName, 0);
                                        if (uiProgressBar1.Value < 100)
                                            uiProgressBar1.Value += 1;
                                    }

                                    //clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_010_SaleFactor SET Column09=" +
                                    //DraftID + " , Column10=" + DocID + " where ColumnId=" +
                                    //item1.Cells["ColumnId"].Value.ToString());
                                    if (NetPrice > 0)
                                    {
                                        CommandTxt += "UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column09=@DraftID,Column10=" + DocID.Value + " where ColumnId=" +
                                        item1.Cells["ColumnId"].Value.ToString();

                                        CommandTxt += "UPDATE " + this.ConWare.Database + ".dbo.Table_007_PwhrsDraft SET  column07=" + DocID.Value + " where ColumnId= @DraftID";
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
                                            Command.ExecuteNonQuery();
                                            sqlTran.Commit();
                                            lsNumbers.Add(item1.Cells["Column01"].Value.ToString());

                                            this.DialogResult = DialogResult.Yes;
                                        }
                                        catch (Exception es)
                                        {
                                            sqlTran.Rollback();
                                            this.Cursor = Cursors.Default;
                                            // Class_BasicOperation.CheckExceptionType(es, this.Name);
                                            notok += item1.Cells["Column01"].Value.ToString() + ",";
                                            // Class_BasicOperation.ShowMsg("", "سندی برای فاکتور شماره" + item1.Cells["Column01"].Value.ToString() + " صادر نشد" + Environment.NewLine + es.Message, "Information");
                                        }

                                        this.Cursor = Cursors.Default;

                                    }
                                }
                            }
                            uiProgressBar1.Value = 100;
                            if (!string.IsNullOrWhiteSpace(notok))
                                MessageBox.Show("سند و حواله فاکتورهای زیر صادر نشد" + Environment.NewLine + notok.TrimEnd(','));
                            else
                                MessageBox.Show("سند و حواله فاکتورها با موفقیت صادر شد");
                        }
                        bt_Refresh_Click(sender, e);
                        uiProgressBar1.Value = 0;
                    }

                }
                bt_ExportDoc.Enabled = true;

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                WarningException es = new WarningException();
                if (es.GetBaseException().GetType() != ex.GetBaseException().GetType())
                {
                    bt_Refresh_Click(sender, e);
                }
            }

        }

        private void CheckEssentialItems(object sender, EventArgs e)
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

            //چک ورود اطلاعات الزامی
            if (txt_Cover.Text.Trim() == "" || !faDatePicker1.SelectedDateTime.HasValue || mlt_SaleBed.Text.Trim() == "" || mlt_SaleBes.Text.Trim() == "")
                throw new WarningException("اطلاعات مربوط به صدور سند را کامل کنید");
            if (mlt_Ware.Text.Trim() == "" || mlt_Function.Text.ToString() == "")
                throw new WarningException("اطلاعات مربوط به صدور حواله را کامل کنید");
            if (Class_BasicOperation._FinType && (mlt_ValueBed.Text.Trim() == "" || mlt_ValueBes.Text.Trim() == ""))
            {
                throw new WarningException("سرفصلهای مربوط به ثبت بهای تمام شده را انتخاب  کنید");
            }


            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

            //کنترل فعال بودن  کالا در انبار
            foreach (Janus.Windows.GridEX.GridEXRow Pitem in gridEX_Header.GetCheckedRows())
            {
                gridEX_Header.MoveTo(Pitem);
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Detail.GetRows())
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(mlt_Ware.Value.ToString()),
                        int.Parse(item.Cells["Column02"].Value.ToString())))
                        throw new WarningException("کالای " + item.Cells["Column02"].Text.Trim() +
                            " در انبار انتخاب شده فعال نمی باشد");
                }

            }

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetRows())
            {
                item.BeginEdit();
                item.Cells["Error"].Value = false;
                item.Cells["ErrorDes"].Value = DBNull.Value;
                item.EndEdit();
            }

            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));

            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));

            //کنترل الزام پروژه مرکز هزینه و شخص
            //Janus.Windows.GridEX.GridEXFilterCondition filter;
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetCheckedRows())
            {
                gridEX_Header.MoveTo(item);
                //کنترل حساب بدهکار فاکتور و شخص فاکتور
                try
                {
                    clCredit.All_Controls_2(mlt_SaleBed.Value.ToString(), int.Parse(item.Cells["Column03"].Value.ToString()), null, null);
                }
                catch (Exception es)
                {
                    item.BeginEdit();
                    item.Cells["Error"].Value = true;
                    item.Cells["ErrorDes"].Value = es.Message;
                    item.EndEdit();
                }
                TPerson.Rows.Add(int.Parse(item.Cells["Column03"].Value.ToString()), mlt_SaleBed.Value.ToString(),
                    Convert.ToDouble(item.Cells["Column28"].Value.ToString()));
                TAccounts.Rows.Add(mlt_SaleBed.Value.ToString(), Convert.ToDouble(item.Cells["Column28"].Value.ToString()));
                TAccounts.Rows.Add(mlt_SaleBes.Value.ToString(), Convert.ToDouble(item.Cells["Column28"].Value.ToString()) * -1);

                //کنترل حساب بستانکار فاکتور و پروژه در کالاها
                //filter = new Janus.Windows.GridEX.GridEXFilterCondition(gridEX_Detail.RootTable.Columns["Column22"], Janus.Windows.GridEX.ConditionOperator.NotIsNull, null);
                //if (gridEX_Detail.FindAll(filter) > 0)
                foreach (Janus.Windows.GridEX.GridEXRow Ditem in gridEX_Detail.GetRows())
                {
                    try
                    {
                        if (Ditem.Cells["Column22"].Text.Trim() != "")
                            clCredit.All_Controls_2(mlt_SaleBes.Value.ToString(), null, null, short.Parse(Ditem.Cells["Column22"].Value.ToString()));
                        else
                            clCredit.All_Controls_2(mlt_SaleBes.Value.ToString(), null, null, null);
                    }
                    catch (Exception es)
                    {
                        item.BeginEdit();
                        item.Cells["Error"].Value = true;
                        item.Cells["ErrorDes"].Value = es.Message;
                        item.EndEdit();
                    }
                }

                //کنترل حسابهای تخفیفات
                Double TotalDiscount = Convert.ToDouble(item.Cells["Column29"].Value.ToString()) +
                    Convert.ToDouble(item.Cells["Column30"].Value.ToString()) +
                    Convert.ToDouble(item.Cells["Column31"].Value.ToString());

                if (TotalDiscount > 0)
                {
                    if (mlt_DisBed.Text.Trim() == "" || mlt_DisBes.Text.Trim() == "")
                        throw new WarningException("سرفصلهای مربوط به تخفیفات را انتخاب کنید");

                    try
                    {
                        clCredit.All_Controls_2(mlt_DisBed.Value.ToString(), null, null, null);
                    }
                    catch (Exception es)
                    {
                        item.BeginEdit();
                        item.Cells["Error"].Value = true;
                        item.Cells["ErrorDes"].Value = es.Message;
                        item.EndEdit();
                    }
                    try
                    {
                        clCredit.All_Controls_2(mlt_SaleBes.Value.ToString(), int.Parse(item.Cells["Column03"].Value.ToString()), null, null);
                    }
                    catch (Exception es)
                    {
                        item.BeginEdit();
                        item.Cells["Error"].Value = true;
                        item.Cells["ErrorDes"].Value = es.Message;
                        item.EndEdit();
                    }
                    TPerson.Rows.Add(int.Parse(item.Cells["Column03"].Value.ToString()), mlt_DisBes.Value.ToString(), TotalDiscount * -1);
                    TAccounts.Rows.Add(mlt_DisBed.Value.ToString(), TotalDiscount);
                    TAccounts.Rows.Add(mlt_DisBes.Value.ToString(), TotalDiscount * -1);
                }

                //کنترل حساب اضافات یا کسورات
                foreach (Janus.Windows.GridEX.GridEXRow item3 in gridEX_Extra.GetRows())
                {
                    ExRedBindingSource.Filter = "ColumnId=" + item3.Cells["Column02"].Value.ToString();

                    //کنترل کسورات
                    if (item3.Cells["Column05"].Value.ToString() == "True")
                    {
                        try
                        {
                            clCredit.All_Controls_2(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString(), null, null, null);
                        }
                        catch (Exception es)
                        {
                            item.BeginEdit();
                            item.Cells["Error"].Value = true;
                            item.Cells["ErrorDes"].Value = es.Message;
                            item.EndEdit();
                        }
                        try
                        {
                            clCredit.All_Controls_2(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString(), int.Parse(item.Cells["Column03"].Value.ToString()), null, null);
                        }
                        catch (Exception es)
                        {
                            item.BeginEdit();
                            item.Cells["Error"].Value = true;
                            item.Cells["ErrorDes"].Value = es.Message;
                            item.EndEdit();
                        }
                        TPerson.Rows.Add(int.Parse(item.Cells["Column03"].Value.ToString()), ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"]
                            .ToString(), Convert.ToDouble(item3.Cells["Column04"].Value.ToString()) * -1);
                    }
                    //کنترل اضافات
                    else
                    {
                        try
                        {
                            clCredit.All_Controls_2(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString(), int.Parse(item.Cells["Column03"].Value.ToString()), null, null);
                        }
                        catch (Exception es)
                        {
                            item.BeginEdit();
                            item.Cells["Error"].Value = true;
                            item.Cells["ErrorDes"].Value = es.Message;
                            item.EndEdit();
                        }
                        try
                        {
                            clCredit.All_Controls_2(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString(), null, null, null);
                        }
                        catch (Exception es)
                        {
                            item.BeginEdit();
                            item.Cells["Error"].Value = true;
                            item.Cells["ErrorDes"].Value = es.Message;
                            item.EndEdit();
                        }
                        TPerson.Rows.Add(int.Parse(item.Cells["Column03"].Value.ToString()), ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"]
                            .ToString(), Convert.ToDouble(item3.Cells["Column04"].Value.ToString()));

                    }
                    TAccounts.Rows.Add(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString(), Convert.ToDouble(item3.Cells["Column04"].Value.ToString()));
                    TAccounts.Rows.Add(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString(), Convert.ToDouble(item3.Cells["Column04"].Value.ToString()) * -1);
                }

                //کنترل حسابهای مربوط به بهای تمام شده را فعلا نمیتونم انجام بدم. حواله نخورده که!!!

            }
            //انتهای کنترل الزامها
            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);




        }

        private void rdb_New_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_New.Checked)
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                txt_Cover.Text = "صدور سند فاکتور فروش";
                faDatePicker1.SelectedDateTime = DateTime.Now;
                txt_LastNum.Text = null;
                txt_To.Text = null;
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
                int LastNum = clDoc.LastDocNum();
                txt_LastNum.Text = LastNum.ToString();
                faDatePicker1.Text = clDoc.DocDate(LastNum);
                txt_Cover.Text = clDoc.Cover(LastNum);

            }
            else
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                faDatePicker1.SelectedDateTime = DateTime.Now;
                txt_Cover.Text = null;
            }
        }

        private void rdb_To_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_To.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
                txt_LastNum.Text = null;
                txt_To.Text = null;

            }
            else
            {
                txt_To.Text = null;
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
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
                    txt_Cover.Text = clDoc.Cover(int.Parse(txt_To.Text.Trim()));
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
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

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker1.HideDropDown();
                    Class_BasicOperation.isEnter(e.KeyChar);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void mlt_Ware_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void table_010_SaleFactor_NoDoc_DraftBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactor_NoDoc_DraftBindingSource.Count > 0)
            {
                try
                {
                    txt_EndPrice.Value = Convert.ToInt64(Convert.ToDouble(txt_TotalPrice.Value.ToString())) - Convert.ToInt64(Convert.ToDouble(txt_VolumeGroup.Value.ToString())) - Convert.ToInt64(Convert.ToDouble(txt_SpecialGroup.Value.ToString())) - Convert.ToInt64(Convert.ToDouble(txt_SpecialCustomer.Value.ToString())) + Convert.ToInt64(Convert.ToDouble(txt_Extra.Value.ToString())) - Convert.ToInt64(Convert.ToDouble(txt_Reductions.Value.ToString()));
                }
                catch { }
                try
                {
                    foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_Extra.GetRows())
                        if (Row.RowType == Janus.Windows.GridEX.RowType.Record)
                        {
                            if (Row.Cells["column05"].Value.ToString() == "True")
                                Row.RowHeaderImageIndex = 1;
                            else
                                Row.RowHeaderImageIndex = 0;
                        }
                }
                catch { }
                try
                {
                    foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_Detail.GetRows())
                        if (Row.RowType == Janus.Windows.GridEX.RowType.Record)
                        {
                            if (Row.Cells["column30"].Value.ToString() == "True")
                                Row.RowHeaderImageIndex = 2;
                        }
                }
                catch { }
            }
        }

        private void Frm_012_ExportDoc_Draft_InTotal_KeyDown(object sender, KeyEventArgs e)
        {
            if (bt_ExportDraft.Enabled && e.Control && e.KeyCode == Keys.D)
                bt_ExportDraft_Click(sender, e);
            else if (bt_ExportDoc.Enabled && e.Control && e.KeyCode == Keys.E)
                bt_ExportDoc_Click(sender, e);
            else if (e.KeyCode == Keys.F5)
                bt_Refresh_Click(sender, e);
        }


        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            dataSet_Sale.EnforceConstraints = false;
            DS.Tables["Extra"].Rows.Clear();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_024_Discount", ConSale);
            Adapter.Fill(DS, "Extra");
            this.table_010_SaleFactor_NoDoc_DraftTableAdapter.Fill(dataSet_Sale.Table_010_SaleFactor_NoDoc_Draft);
            this.table_011_Child1_SaleFactor_NoDoc_DraftTableAdapter.Fill(dataSet_Sale.Table_011_Child1_SaleFactor_NoDoc_Draft);
            this.table_012_Child2_SaleFactor_NoDoc_DraftTableAdapter.Fill(dataSet_Sale.Table_012_Child2_SaleFactor_NoDoc_Draft);
            dataSet_Sale.EnforceConstraints = true;
            this.table_010_SaleFactor_NoDoc_DraftBindingSource_PositionChanged(sender, e);
            bt_ExportDoc.Enabled = true;

        }

        private void mnu_ViewDocs_Click(object sender, EventArgs e)
        {
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 22))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form04_ViewDocument")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                PACNT._2_DocumentMenu.Form04_ViewDocument frm = new PACNT._2_DocumentMenu.Form04_ViewDocument();
                try
                {
                    frm.MdiParent = MainForm.ActiveForm;
                }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ViewDrafts_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 26))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form07_ViewDrafts")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                PWHRS._03_AmaliyatAnbar.Form07_ViewDrafts frm = new PWHRS._03_AmaliyatAnbar.Form07_ViewDrafts();
                try
                {
                    frm.MdiParent = MainForm.ActiveForm;
                }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX_Header_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                uiPanel6.Text = gridEX_Header.GetValue("ErrorDes").ToString();
            }
            catch
            {
            }
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }






    }
}
