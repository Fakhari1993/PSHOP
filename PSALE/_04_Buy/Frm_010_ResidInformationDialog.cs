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
    public partial class Frm_010_ResidInformationDialog : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        BindingSource Child1Bind;
        DataRowView BuyRow;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        string _Date = null;
        int ResidID = 0, ResidNum = 0;
        DataTable _Table;
        Int16 _ware = 0, _func = 0;

        public Frm_010_ResidInformationDialog(BindingSource _Child1, DataRowView _BuyRow, DataTable Table, Int16 ware, Int16 func)
        {
            InitializeComponent();
            Child1Bind = _Child1;
            BuyRow = _BuyRow;
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

            Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=0", ConWare);
            Adapter.Fill(DS, "Fun");
            mlt_Function.DataSource = DS.Tables["Fun"];
            mlt_Ware.Value = _ware;
            mlt_Function.Value = _func;
            chk_DraftNum.Checked = false;
            txt_DraftNum.Enabled = false;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            string commandtxt = string.Empty;
            commandtxt = "Declare @Key int";
            try
            {
                if (clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", BuyRow["ColumnId"].ToString()) != 0)
                    throw new Exception("برای این فاکتور، رسید انبار صادر شده است");

                if (mlt_Function.Text.Trim() == "" || mlt_Ware.Text.Trim() == "")
                    throw new Exception("اطلاعات مورد نیاز را تکمیل کنید");

                if ((chk_DraftNum.Checked && Convert.ToInt32(txt_DraftNum.Value) <= 0))
                    throw new Exception("اطلاعات مورد نیاز جهت صدور رسید انبار را کامل کنید");

                if ((chk_DraftNum.Checked))
                {
                    int ok = 0;
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        Con.Open();
                        SqlCommand Select = new SqlCommand(@"IF EXISTS(
                                                       SELECT *
                                                       FROM   Table_011_PwhrsReceipt tpd
                                                       WHERE  tpd.column01 = " + txt_DraftNum.Value + @"
                                                   )
                                                    SELECT 0 AS ok 
                                                ELSE
                                                    SELECT 1 ok", Con);
                        ok = Convert.ToInt32(Select.ExecuteScalar().ToString());
                    }
                    if (ok == 0)
                        throw new Exception("این شماره رسید استفاده شده است");

                }

                foreach (DataRow item in _Table.Rows)
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(mlt_Ware.Value.ToString()),
                        int.Parse(item["GoodID"].ToString())))
                    {
                        throw new Exception("کالای " + item["GoodName"].ToString() + " در انبار انتخاب شده فعال نمی باشد");
                    }

                }
                //Export Resid
                //**Resid Header

                // SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                //key.Direction = ParameterDirection.Output;
                //using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                //{
                //                    Con.Open();
                //                    SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_011_PwhrsReceipt (
                //                                                                            [column01],
                //                                                                            [column02],
                //                                                                            [column03],
                //                                                                            [column04],
                //                                                                            [column05],
                //                                                                            [column06],
                //                                                                            [column07],
                //                                                                            [column08],
                //                                                                            [column09],
                //                                                                            [column10],
                //                                                                            [column11],
                //                                                                            [column12],
                //                                                                            [column13],
                //                                                                            [column14],
                //                                                                            [Column15],
                //                                                                            [Column16],
                //                                                                            [Column17],
                //                                                                            [Column18],
                //                                                                            [Column19],
                //                                                                            [Column20]
                //                                                                          ) VALUES (" + ResidNum + ",'" + BuyRow["Column02"].ToString() + "'," +
                //                     mlt_Ware.Value.ToString() + "," + mlt_Function.Value.ToString() + "," + BuyRow["Column03"].ToString() + ",'" + "فاکتور خرید ش" +
                //                     BuyRow["Column01"].ToString() + " تاریخ " + BuyRow["Column02"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                //                     (BuyRow["Column09"].ToString().Trim() == "" ? "NULL" : BuyRow["Column09"].ToString()) + "," + BuyRow["ColumnId"].ToString() + ",0,0,"+
                //                     (BuyRow["Column15"].ToString()=="True"?"1":"0")+","+
                //                     (BuyRow["Column25"].ToString().Trim()==""?"NULL":BuyRow["Column25"].ToString())+","+
                //                     BuyRow["Column26"].ToString()
                //                     + ",1,null); SET @Key=Scope_Identity()", Con);
                //                    Insert.Parameters.Add(key);
                //                    Insert.ExecuteNonQuery();
                //                    ResidID = int.Parse(key.Value.ToString());

                if (chk_DraftNum.Checked)

                    ResidNum = Convert.ToInt32(txt_DraftNum.Value);
                else

                    ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");

                commandtxt += @" INSERT INTO Table_011_PwhrsReceipt (
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
                                                                          ) VALUES (" + ResidNum + ",'" + BuyRow["Column02"].ToString() + "'," +
                 mlt_Ware.Value.ToString() + "," + mlt_Function.Value.ToString() + "," + BuyRow["Column03"].ToString() + ",'" + "فاکتور خرید ش" +
                 BuyRow["Column01"].ToString() + " تاریخ " + BuyRow["Column02"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                 (BuyRow["Column09"].ToString().Trim() == "" ? "NULL" : BuyRow["Column09"].ToString()) + "," + BuyRow["ColumnId"].ToString() + ",0,0," +
                 (BuyRow["Column15"].ToString() == "True" ? "1" : "0") + "," +
                 (BuyRow["Column25"].ToString().Trim() == "" ? "NULL" : BuyRow["Column25"].ToString()) + "," +
                 BuyRow["Column26"].ToString()
                 + ",1,null); SET @Key=Scope_Identity() ";



                //Resid Detail



                foreach (DataRowView item in Child1Bind)
                {

                    //// added by Roostaee 96/07/05 
                    // اعمال تسهیم
                    DataTable value = new DataTable();
                    try
                    {
                        SqlDataAdapter Adapter = new SqlDataAdapter(@"DECLARE @share  FLOAT,
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
                                                                                              AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                                   ) AS tt
                                                                        )

                                                                    SET @Net =isnull( (
                                                                            SELECT tbf.Column20
                                                                            FROM   Table_015_BuyFactor tbf
                                                                            WHERE  tbf.columnid = " + BuyRow["columnid"].ToString() + @"
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
                                                                                           AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                                ),
                                                                                0
                                                                            ) / nullif( ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column07)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                           AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                                ),
                                                                                0
                                                                            ),0)
                                                                        ) * (1 + @share) else isnull(" + Convert.ToDouble(item["Column20"].ToString()) + @" /nullif( " + Convert.ToDouble(item["Column07"].ToString()) + @",0),0) END)

                                                                    SET @totalvalue = @unitvalue * ISNULL(
                                                                            (
                                                                                SELECT SUM(tcbf.column07)
                                                                                FROM   Table_016_Child1_BuyFactor tcbf
                                                                                WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                       AND tcbf.column01 = " + BuyRow["columnid"].ToString() + @"
                                                                            ),
                                                                            0
                                                                        )  

                                                                    SELECT 1+@share AS share,
                                                                           isnull( @unitvalue,0) AS unitvalue,
                                                                         isnull(  @totalvalue,0) AS totalvalue

                                                                    ", ConSale);
                        Adapter.Fill(value);
                    }
                    catch
                    {
                        SqlDataAdapter Adapter1 = new SqlDataAdapter(@"SELECT 1  AS share,
                                                                           0 AS unitvalue,
                                                                         0 AS totalvalue", ConSale);
                        Adapter1.Fill(value);
                    }

                    //SqlCommand InsertDetail = new SqlCommand(
                    //    "INSERT INTO Table_012_Child_PwhrsReceipt VALUES (" + ResidID + "," + item["Column02"].ToString() + "," +
                    //    item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + "," +
                    //    item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                    //    (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                    //    + "',getdate(),0," + Convert.ToDouble(value.Rows[0]["unitvalue"]) + "," +
                    //    Convert.ToDouble(value.Rows[0]["unitvalue"]) * Convert.ToDouble(item["Column07"].ToString())+ "," + item["ColumnId"].ToString() + ",NULL,NULL," +
                    //    (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," +
                    //    item["Column14"].ToString() + ",0,0,0," +
                    //    (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                    //    (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," +
                    //    item["Column29"].ToString() + "," + item["Column30"].ToString() + "," + item["Column34"].ToString() + "," +
                    //    item["Column35"].ToString() + ")", Con);
                    //InsertDetail.ExecuteNonQuery();
                    //                    SqlCommand InsertDetail = new SqlCommand(
                    //                       @"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
                    //           ,[column02]
                    //           ,[column03]
                    //           ,[column04]
                    //           ,[column05]
                    //           ,[column06]
                    //           ,[column07]
                    //           ,[column08]
                    //           ,[column09]
                    //           ,[column10]
                    //           ,[column11]
                    //           ,[column12]
                    //           ,[column13]
                    //           ,[column14]
                    //           ,[column15]
                    //           ,[column16]
                    //           ,[column17]
                    //           ,[column18]
                    //           ,[column19]
                    //           ,[column20]
                    //           ,[column21]
                    //           ,[column22]
                    //           ,[column23]
                    //           ,[column24]
                    //           ,[column25]
                    //           ,[column26]
                    //           ,[column27]
                    //           ,[column28]
                    //           ,[column29]
                    //           ,[Column30]
                    //           ,[Column31]
                    //           ,[Column32]
                    //           ,[Column33]
                    //           ,[Column34]
                    //           ,[Column35]) VALUES ( @Key," + item["Column02"].ToString() + "," +
                    //                       item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + "," +
                    //                       item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                    //                       (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                    //                       + "',getdate(),0," + (double.Parse(item["Column20"].ToString()) / double.Parse(item["Column07"].ToString())) * Convert.ToDouble(value.Rows[0]["share"]) + "," +
                    //                       Convert.ToDouble(item["Column20"].ToString()) * Convert.ToDouble(value.Rows[0]["share"]) + "," + item["ColumnId"].ToString() + ",NULL,NULL," +
                    //                       (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," +
                    //                       item["Column14"].ToString() + ",0,0,0," +
                    //                       (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                    //                       (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," +
                    //                       item["Column29"].ToString() + "," + item["Column30"].ToString() + "," + item["Column34"].ToString() + "," +
                    //                       item["Column35"].ToString() + ")", Con);
                    //                    InsertDetail.ExecuteNonQuery();

                    commandtxt += @" INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35]) VALUES (@Key," + item["Column02"].ToString() + "," +
                       item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + "," +
                       item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL," +
                       (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                       + "',getdate(),0," + (double.Parse(item["Column20"].ToString()) / double.Parse(item["Column07"].ToString())) * Convert.ToDouble(value.Rows[0]["share"]) + "," +
                       Convert.ToDouble(item["Column20"].ToString()) * Convert.ToDouble(value.Rows[0]["share"]) + "," + item["ColumnId"].ToString() + ",NULL,NULL," +
                       (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," +
                       item["Column14"].ToString() + ",0,0,0," +
                       (item["Column32"].ToString().Trim() == "" ? "NULL" : "'" + item["Column32"].ToString() + "'") + "," +
                       (item["Column33"].ToString().Trim() == "" ? "NULL" : "'" + item["Column33"].ToString() + "'") + "," +
                       item["Column29"].ToString() + "," + item["Column30"].ToString() + "," + item["Column34"].ToString() + "," +
                       item["Column35"].ToString() + ") ";
                }



                commandtxt += " Update " + ConSale.Database + ".dbo.Table_015_BuyFactor set Column10=@Key where ColumnId = " + int.Parse(BuyRow["ColumnId"].ToString());
                //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_015_BuyFactor", "Column10", "ColumnId", int.Parse(BuyRow["ColumnId"].ToString()), ResidID);

                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                {
                    Con.Open();

                    SqlTransaction sqlTran = Con.BeginTransaction();
                    SqlCommand Command = Con.CreateCommand();
                    Command.Transaction = sqlTran;

                    try
                    {
                        Command.CommandText = commandtxt;

                        Command.ExecuteNonQuery();
                        sqlTran.Commit();

                        this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                        Class_BasicOperation.ShowMsg("", "رسید انبار به شماره " + ResidNum.ToString() + " با موفقیت ثبت شد ", "Information");

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
                //}
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
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
