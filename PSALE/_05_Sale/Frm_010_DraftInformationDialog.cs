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
    public partial class Frm_010_DraftInformationDialog : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);

        BindingSource Child1Bind;
        DataRowView SaleRow;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        string _Date = null;
        int DraftID = 0, DraftNum = 0;
        DataTable _Table = new DataTable();
        Int16 _ware = 0, _func = 0;

        public Frm_010_DraftInformationDialog(BindingSource _Child1, DataRowView _SaleRow
            , string Date, DataTable Table, Int16 ware, Int16 func)
        {
            InitializeComponent();
            Child1Bind = _Child1;
            SaleRow = _SaleRow;
            _Date = Date;
            _Table = Table;
            _ware = ware;
            _func = func;
        }

        private void Frm_010_DraftInformationDialog_Load(object sender, EventArgs e)
        {
            DataSet DS = new DataSet();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_001_PWHRS", ConWare);
            Adapter.Fill(DS, "Ware");
            mlt_Ware.DataSource = DS.Tables["Ware"];

            Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=1", ConWare);
            Adapter.Fill(DS, "Fun");
            mlt_Function.DataSource = DS.Tables["Fun"];
            mlt_Ware.Value = _ware;
            mlt_Function.Value = _func;

            chk_DraftNum.Checked = false;
            txt_DraftNum.Enabled = false;
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
                if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", SaleRow["ColumnId"].ToString()) != 0)
                    throw new Exception("برای این فاکتور حواله انبار صادر شده است");

                if (mlt_Function.Text.Trim() == "" || mlt_Ware.Text.Trim() == "")
                    throw new Exception("اطلاعات مورد نیاز را تکمیل کنید");

                if ((chk_DraftNum.Checked && Convert.ToInt32(txt_DraftNum.Value) <= 0))
                    throw new Exception("اطلاعات مورد نیاز جهت صدور حواله انبار را کامل کنید");

                if ((chk_DraftNum.Checked))
                {
                    int ok1 = 0;
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
                        ok1 = Convert.ToInt32(Select.ExecuteScalar().ToString());
                    }
                    if (ok1 == 0)
                        throw new Exception("این شماره حواله استفاده شده است");

                }


                foreach (DataRow item in _Table.Rows)
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(mlt_Ware.Value.ToString()),
                        int.Parse(item["GoodID"].ToString())))
                        throw new Exception("کالای " + item["GoodName"].ToString() +
                            " در انبار انتخاب شده فعال نمی باشد");
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
                    if (chk_DraftNum.Checked && Convert.ToInt32(txt_DraftNum.Value) > 0)
                        command = @"  declare @Key int     set @DraftNum=" + txt_DraftNum.Value + @"  INSERT INTO Table_007_PwhrsDraft ([column01]
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
                                                                                               ,[Column26]) VALUES(@DraftNum,'" + SaleRow["Column02"].ToString() + "'," + mlt_Ware.Value.ToString()
                              + "," + mlt_Function.Value.ToString() + "," + SaleRow["Column03"].ToString() + ",'" + "حواله صادره بابت فاکتور فروش ش" + SaleRow["Column01"].ToString() +
                              "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + SaleRow["ColumnId"].ToString() + ",0," +
                              SaleRow["Column07"].ToString() + ",0,0,0,0," +
                              (SaleRow["Column12"].ToString() == "False" ? "0" : "1") + "," + (SaleRow["Column40"].ToString().Trim() == "" ? "NULL" : SaleRow["Column40"].ToString())
                              + "," +
                               SaleRow["Column41"].ToString() + ",1); SET @Key=SCOPE_IDENTITY()";


                    else
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
                           ,[Column26]) VALUES(@DraftNum,'" + SaleRow["Column02"].ToString() + "'," + mlt_Ware.Value.ToString()
                                  + "," + mlt_Function.Value.ToString() + "," + SaleRow["Column03"].ToString() + ",'" + "حواله صادره بابت فاکتور فروش ش" + SaleRow["Column01"].ToString() +
                                  "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + SaleRow["ColumnId"].ToString() + ",0," +
                                  SaleRow["Column07"].ToString() + ",0,0,0,0," +
                                  (SaleRow["Column12"].ToString() == "False" ? "0" : "1") + "," + (SaleRow["Column40"].ToString().Trim() == "" ? "NULL" : SaleRow["Column40"].ToString())
                                  + "," +
                                   SaleRow["Column41"].ToString() + ",1); SET @Key=SCOPE_IDENTITY()";



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
           ,[Column35],Column36,Column37) VALUES(@Key," + item["Column02"].ToString() + "," +
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
                                     + "," + item["Column32"].ToString() + "," + item["Column36"].ToString() + "," + item["Column37"].ToString() + "," + (item["Column38"].ToString().Trim() == "" ? "NULL" : "'" + item["Column38"].ToString() + "'") + "," + (item["Column39"].ToString().Trim() == "" ? "NULL" : "'" + item["Column39"].ToString() + "'") + ")";

                        }

                    }



                    command += "Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column09=@Key,Column15='" + Class_BasicOperation._UserName + "',Column16=getdate() where ColumnId=" + int.Parse(SaleRow["ColumnId"].ToString());

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
                                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value.ToString(), Con);
                                    DataTable TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftID + " and DetailID=" + item["Columnid"].ToString());
                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                        + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                    UpdateCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value.ToString() + ",@Date='" + SaleRow["Column02"].ToString() + "',@id=" + DraftID + ",@residid=0", ConWare);
                                    DataTable TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                  + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                    UpdateCommand.ExecuteNonQuery();
                                }
                            }


                            Class_BasicOperation.ShowMsg("", "حواله انبار به شماره " + DraftNum.Value + " با موفقیت ثبت شد ", "Information");
                            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                            this.Close();

                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }
                        this.Cursor = Cursors.Default;
                    }
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
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
                CommandText = string.Format(CommandText, mlt_Ware.Value.ToString(), GoodCode, _Date);
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
                CommandText = string.Format(CommandText, mlt_Ware.Value.ToString(), GoodCode);
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

        private void txt_DraftNum_KeyPress(object sender, KeyPressEventArgs e)
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

        private void chk_DraftNum_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_DraftNum.Checked)
                txt_DraftNum.Enabled = true;
            else
                txt_DraftNum.Enabled = false;
        }
    }
}
