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
    public partial class Frm_035_TransferInformationDialog : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        BindingSource Child1Bind;
        DataRowView SaleRow;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        string _Date = null;
        int DraftID2 = 0, DraftNum = 0;
        int ResidID = 0, ResidNum = 0;
        DataTable _Table = new DataTable();
        Int16 _wareDraft = 0, _wareRecipt = 0;

        public Frm_035_TransferInformationDialog(BindingSource _Child1, DataRowView _SaleRow
            , string Date, DataTable Table, Int16 wareDraft, Int16 wareRecipt)
        {
            InitializeComponent();
            Child1Bind = _Child1;
            SaleRow = _SaleRow;
            _Date = Date;
            _Table = Table;
            _wareRecipt = wareRecipt;
            _wareDraft = wareDraft;
        }

        private void Frm_010_DraftInformationDialog_Load(object sender, EventArgs e)
        {
            DataSet DS = new DataSet();
            //SqlDataAdapter Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_001_PWHRS", ConWare);
            //Adapter.Fill(DS, "Ware");
            //mlt_Ware.DataSource = DS.Tables["Ware"];

            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=1", ConWare);
            Adapter.Fill(DS, "Fun1");
            mlt_FunctionDraft.DataSource = DS.Tables["Fun1"];

             Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=0", ConWare);
            Adapter.Fill(DS, "Fun2");
            mlt_FunctionRecipt.DataSource = DS.Tables["Fun2"];
         


        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            string command = string.Empty;
            SqlParameter DraftNum;
            DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
            DraftNum.Direction = ParameterDirection.Output;
            DraftNum.Value = 0;
            try
            {
                if (clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column56", SaleRow["ColumnId"].ToString()) != 0)
                    throw new Exception("برای این فاکتور حواله انبار صادر شده است");

                if (clDoc.OperationalColumnValue("Table_070_AmaniFactor", "Column55", SaleRow["ColumnId"].ToString()) != 0)
                    throw new Exception("برای این فاکتور، رسید انبار صادر شده است");


                if (mlt_FunctionDraft.Text.Trim() == "" || mlt_FunctionRecipt.Text.Trim() == "")
                    throw new Exception("اطلاعات مورد نیاز را تکمیل کنید");
               ///check kardan dar anbar mabda
                foreach (DataRow item in _Table.Rows)
                {
                    if (!clGood.IsGoodInWare(_wareDraft,
                        int.Parse(item["GoodID"].ToString())))
                        throw new Exception("کالای " + item["GoodName"].ToString() +
                            " در انبار انتخاب شده فعال نمی باشد");
                }
                ///check kardan dar anbar maghsad
                foreach (DataRow item in _Table.Rows)
                {
                    if (!clGood.IsGoodInWare(_wareRecipt,
                        int.Parse(item["GoodID"].ToString())))
                    {
                        throw new Exception("کالای " + item["GoodName"].ToString() + " در انبار انتخاب شده فعال نمی باشد");
                    }

                }

                //چک باقی مانده کالا
                foreach (DataRowView item in Child1Bind)
                {
                    if (clDoc.IsGood(item["Column02"].ToString()))
                    {
                        float Remain = FirstRemain(int.Parse(item["Column02"].ToString()));
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
                                                                                   WHERE  ColumnId = " + item["Column02"] + @"
                                                                               ),
                                                                               0
                                                                           ) AS Column16", ConWareGood);
                                mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                            }
                        }
                        catch
                        {
                        }
                        if (Remain < float.Parse(item["Column07"].ToString()) && !mojoodimanfi)
                        {
                            throw new Exception("موجودی کالای " + clDoc.ExScalar(ConWare.ConnectionString,
                                "table_004_CommodityAndIngredients", "Column02", "ColumnId",
                                item["Column02"].ToString()) +
                                " کمتر از تعداد مشخص شده در فاکتور است" + Environment.NewLine +
                                "موجودی: " + Remain.ToString());
                        }
                    }
                }
                bool ok = true;
                string good = string.Empty;


                foreach (DataRowView item in Child1Bind)
                {
                    if (clDoc.IsGood(item["Column02"].ToString()))
                    {
                        float Remain = TotalRemain(int.Parse(item["Column02"].ToString()));
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
                                                                                   WHERE  ColumnId = " + item["Column02"] + @"
                                                                               ),
                                                                               0
                                                                           ) AS Column16", ConWareGood);
                                mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                            }
                        }
                        catch
                        {
                        }
                        if (Remain < float.Parse(item["Column07"].ToString()) && !mojoodimanfi)
                        {
                            good += clDoc.ExScalar(ConWare.ConnectionString,
                                "table_004_CommodityAndIngredients", "Column02", "ColumnId",
                                item["Column02"].ToString()) + " , ";


                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(good))
                {

                    string good1 = good.Remove(good.Length - 2, 2);
                    string Message = "موجودی کل کالاهای زیر منفی می شود،آیا مایل به ادامه کار هستید" + Environment.NewLine + good1;
                    if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        ok = true;
                    }
                    else
                        ok = false;
                }
                if (ok)
                {
                    //درج هدر حواله
                    command = @" declare @Key int     set @DraftNum=(SELECT ISNULL(Max(Column01),0)+1  from Table_007_PwhrsDraft) INSERT INTO Table_007_PwhrsDraft  ([column01]
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
           ,[Column26]) VALUES(@DraftNum,'" + SaleRow["Column02"].ToString() + "'," + _wareDraft
                              + "," + mlt_FunctionDraft.Value.ToString() + "," + SaleRow["Column03"].ToString() + ",'" + "حواله صادره بابت انتقالی بین انبارهای مربوط به فاکتور امانی شماره" + SaleRow["Column01"].ToString() +
                              "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0," +
                            0 + ",0,0,0,0," +
                            "0" + "," +  "NULL"  + "," +
                              "0" + ",1); SET @Key=SCOPE_IDENTITY()";



                    //درج کالاهای موجود در حواله 
                    foreach (DataRowView item in Child1Bind)
                    {
                        if (clDoc.IsGood(item["Column02"].ToString()))
                        {

                            command += @"INSERT INTO Table_008_Child_PwhrsDraft ([column01]
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
           ,[Column35]) VALUES(@Key," + item["Column02"].ToString() + "," +
                                 item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                                 item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," +
                                 item["Column11"].ToString() + ",NULL,NULL," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString())
                                 + ",0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL," +
                                 (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                                 item["Column15"].ToString() +
                                     ",0,0,0,0," + (item["Column30"].ToString() == "True" ? "1" : "0") + "," +
                                     (item["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item["Column34"].ToString() + "'") + "," +
                                     (item["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item["Column35"].ToString() + "'")
                                     + "," + item["Column31"].ToString()
                                     + "," + item["Column32"].ToString() + "," + item["Column36"].ToString() + "," + item["Column37"].ToString() + ")";

                        }

                    }



                    command += "Update " + ConSale.Database + ".dbo.Table_070_AmaniFactor set Column56=@Key,Column15='" + Class_BasicOperation._UserName + "',Column16=getdate() where ColumnId=" + int.Parse(SaleRow["ColumnId"].ToString());

                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        Con.Open();

                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;

                        try
                        {
                            Command.CommandText = command;
                            Command.Parameters.Add(DraftNum);
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();
                            string DraftID = clDoc.ExScalar(ConWare.ConnectionString, "Table_007_PwhrsDraft", "columnid", "column01", DraftNum.Value.ToString());

                            SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + DraftID, ConWare);
                            DataTable Table = new DataTable();
                            goodAdapter.Fill(Table);

                            //محاسبه ارزش و ذخیره آن در جدول Child1

                            foreach (DataRow item in Table.Rows)
                            {
                                if (Class_BasicOperation._WareType)
                                {
                                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + _wareDraft, Con);
                                    DataTable TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftID + " and DetailID=" + item["Columnid"].ToString());
                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                        + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                    UpdateCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + _wareDraft + ",@Date='" + SaleRow["Column02"].ToString() + "',@id=" + DraftID + ",@residid=0", ConWare);
                                    DataTable TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                  + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["Columnid"].ToString(), Con);
                                    UpdateCommand.ExecuteNonQuery();
                                }
                            }


                            //Class_BasicOperation.ShowMsg("", "حواله انبار به شماره " + DraftNum.Value + " با موفقیت ثبت شد ", "Information");
                            //this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                            //this.Close();

                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }
                        this.Cursor = Cursors.Default;
                    }
                    //////////////
                    //Export Resid
                    //**Resid Header
                    ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
                    //, int.Parse(mlt_Ware.Value.ToString()));
                    SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                    key.Direction = ParameterDirection.Output;
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        Con.Open();
                        SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_011_PwhrsReceipt (
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
                                                                          ) VALUES (" + ResidNum + ",'" + SaleRow["Column02"].ToString() + "'," +
                        _wareRecipt + "," + mlt_FunctionRecipt.Value + ",NULL,'" + "رسید صادره بابت انتقالی بین انبارهای مربوط به فاکتور امانی شماره" +
                         SaleRow["Column01"].ToString() + " تاریخ " + SaleRow["Column02"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                        "0" + ",0,0,0," +
                        "0" + "," +
                          "NULL"   + "," +
                        "0"
                         + ",1,null); SET @Key=Scope_Identity()", Con);
                        Insert.Parameters.Add(key);
                        Insert.ExecuteNonQuery();
                        ResidID = int.Parse(key.Value.ToString());


                        //Resid Detail



                        foreach (DataRowView item in Child1Bind)
                        {

                            //// added by Roostaee 96/07/05 
                            // اعمال تسهیم
                            DataTable value = new DataTable();
                            SqlDataAdapter Adapter = new SqlDataAdapter(@"DECLARE @share FLOAT,
                                                                            @sum    DECIMAL(18, 3),
                                                                            @Net    DECIMAL(18, 3)

                                                                    SET @sum = (
                                                                            SELECT SUM(ISNULL(tt.VE, 0))
                                                                            FROM   (
                                                                                       SELECT (
                                                                                                  CASE 
                                                                                                       WHEN tcbf.column05 = 0 THEN tcbf.column04
                                                                                                       ELSE ((-1) * tcbf.column04)
                                                                                                  END
                                                                                              ) AS VE
                                                                                       FROM   Table_017_Child2_BuyFactor tcbf
                                                                                              JOIN Table_024_Discount_Buy tdb
                                                                                                   ON  tdb.columnid = tcbf.column02
                                                                                       WHERE  tdb.Column18 = 1
                                                                                              AND tcbf.column01 = " + SaleRow["columnid"].ToString() + @"
                                                                                   ) AS tt
                                                                        )

                                                                    SET @Net =isnull( (
                                                                            SELECT tbf.Column20
                                                                            FROM   Table_015_BuyFactor tbf
                                                                            WHERE  tbf.columnid = " + SaleRow["columnid"].ToString() + @"
                                                                        ),0)
    
                                                                    SET @share =isnull( @sum /nullif( @Net,0),0)
                                                                    DECLARE @unitvalue   DECIMAL(18, 3),
                                                                            @totalvalue  DECIMAL(18, 3)
    
                                                                    SET @unitvalue =(CASE WHEN @share>0 then (
                                                                            ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column20)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                           AND tcbf.column01 = " + SaleRow["columnid"].ToString() + @"
                                                                                ),
                                                                                0
                                                                            ) / nullif( ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column07)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                           AND tcbf.column01 = " + SaleRow["columnid"].ToString() + @"
                                                                                ),
                                                                                0
                                                                            ),0)
                                                                        ) * (1 + @share) else isnull(" + Convert.ToDouble(item["Column20"].ToString()) + @" /nullif( " + Convert.ToDouble(item["Column07"].ToString()) + @",0),0) END)

                                                                    SET @totalvalue = @unitvalue * ISNULL(
                                                                            (
                                                                                SELECT SUM(tcbf.column07)
                                                                                FROM   Table_016_Child1_BuyFactor tcbf
                                                                                WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                       AND tcbf.column01 = " + SaleRow["columnid"].ToString() + @"
                                                                            ),
                                                                            0
                                                                        )  

                                                                    SELECT 1+@share AS share,
                                                                           isnull( @unitvalue,0) AS unitvalue,
                                                                         isnull(  @totalvalue,0) AS totalvalue

                                                                    ", ConSale);
                            Adapter.Fill(value);



                            SqlCommand InsertDetail = new SqlCommand(
                               @"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35]) VALUES (" + ResidID + "," + item["Column02"].ToString() + "," +
                               item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + "," +
                               item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                               (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                               + "',getdate(),0," + (double.Parse(item["Column20"].ToString()) / double.Parse(item["Column07"].ToString())) * Convert.ToDouble(value.Rows[0]["share"]) + "," +
                               Convert.ToDouble(item["Column20"].ToString()) * Convert.ToDouble(value.Rows[0]["share"]) + "," + item["ColumnId"].ToString() + ",NULL,NULL," +
                              0 + "," +
                             0 + ",0,0,0," +
                               (item["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item["Column34"].ToString() + "'") + "," +
                               (item["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item["Column35"].ToString() + "'") + "," +
                               0 + "," + 0 + "," +0 + "," +
                               0+ ")", Con);
                            InsertDetail.ExecuteNonQuery();
                        }




                        clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_070_AmaniFactor", "Column55", "ColumnId", int.Parse(SaleRow["ColumnId"].ToString()), ResidID);

                    }

                    if (ResidNum != 0 && Convert.ToInt32( DraftNum.Value) != 0)
                    {
                        Class_BasicOperation.ShowMsg("", "حواله انبار به شماره " + DraftNum.Value + "و رسید انبار به شماره " + ResidNum + " با موفقیت ثبت شد ", "Information");
                        this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                        this.Close();
                    }


                }

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
                CommandText = string.Format(CommandText, _wareDraft, GoodCode, _Date);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        private float TotalRemain(int GoodCode)
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
                       ";
                CommandText = string.Format(CommandText,_wareDraft.ToString(), GoodCode);
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

        private void Frm_010_DraftInformationDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                uiButton1_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.Q)
                uiButton2_Click(sender, e);
        }
    }
}
