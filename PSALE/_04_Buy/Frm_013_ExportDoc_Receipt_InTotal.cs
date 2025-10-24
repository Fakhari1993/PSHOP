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
    public partial class Frm_013_ExportDoc_Receipt_InTotal : Form
    {
        bool _ExportReceipt = false;
        bool _ExportDoc = false;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        DataSet DS = new DataSet();
        bool _BackSpace = false;
        BindingSource ExRedBindingSource = new BindingSource();
        string CommandTxt = string.Empty;

        public Frm_013_ExportDoc_Receipt_InTotal(bool ExportDoc, bool ExportRecepit)
        {
            InitializeComponent();
            _ExportDoc = ExportDoc;
            _ExportReceipt = ExportRecepit;
        }

        private void Frm_013_ExportDoc_Receipt_InTotal_Load(object sender, EventArgs e)
        {
            faDatePicker1.SelectedDateTime = DateTime.Now;

            SqlDataAdapter Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(DS, "Center");
            gridEX_Detail.DropDowns["Center"].SetDataBinding(DS.Tables["Center"], "");


            Adapter = new SqlDataAdapter("select ACC_Code,ACC_Name,Control_Type,Control_Action,Control_Person,Control_Center,Control_Project from AllHeaders()", ConAcnt);
            Adapter.Fill(DS, "Header");
            mlt_BuyBed.DataSource = DS.Tables["Header"];
            mlt_BuyBes.DataSource = DS.Tables["Header"];

            gridEX_Header.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from ListPeople(5)"), "");

            gridEX_Header.DropDowns["Buyer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,4)"), "");


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

            Adapter = new SqlDataAdapter("select ColumnId,Column01 from Table_013_RequestBuy", ConSale);
            Adapter.Fill(DS, "Request");
            gridEX_Detail.DropDowns["Request"].SetDataBinding(DS.Tables["Request"], "");


            Adapter = new SqlDataAdapter("Select * from Table_024_Discount_Buy", ConSale);
            Adapter.Fill(DS, "Extra");
            gridEX_Extra.DropDowns["Extra"].SetDataBinding(DS.Tables["Extra"], "");
            ExRedBindingSource.DataSource = DS.Tables["Extra"];

            mlt_BuyBed.Value = clDoc.Account(9, "Column07");
            mlt_BuyBes.Value = clDoc.Account(9, "Column13");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_001_PWHRS", ConWare);
            Adapter.Fill(DS, "Ware");
            mlt_Ware.DataSource = DS.Tables["Ware"];

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from table_005_PwhrsOperation where Column16=0", ConWare);
            Adapter.Fill(DS, "Operation");
            mlt_Function.DataSource = DS.Tables["Operation"];

            this.table_015_BuyFactor_NoDoc_NoReceiptTableAdapter.Fill_NoDoc_NoReceipt(this.dataSet_Buy.Table_015_BuyFactor_NoDoc_NoReceipt);
            this.table_016_Child1_BuyFactor_NoDoc_NoReceiptTableAdapter.Fill(this.dataSet_Buy.Table_016_Child1_BuyFactor_NoDoc_NoReceipt);
            this.table_017_Child2_BuyFactor_NoDoc_NoReceiptTableAdapter.Fill(this.dataSet_Buy.Table_017_Child2_BuyFactor_NoDoc_NoReceipt);

            this.table_015_BuyFactor_NoDoc_NoReceiptBindingSource_PositionChanged(sender, e);

        }

        private void rdb_New_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_New.Checked)
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
                txt_Cover.Text = "صدور سند فاکتور خرید";
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

        private void mlt_Ware_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void bt_ExportReceipts_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> FactorLs = new List<string>();

                gridEX_Header.MoveToNewRecord();
                if (gridEX_Header.GetCheckedRows().Length > 0)
                {
                    if (!_ExportReceipt)
                        throw new WarningException("کاربر گرامی شما امکان صدور رسید انبار را ندارید");


                    if (mlt_Function.Text.Trim() == "" || mlt_Ware.Text.Trim() == "")
                        throw new WarningException("اطلاعات مورد نیاز جهت صدور رسید را تکمیل کنید");

                    if (uiProgressBar1.Value < 100)
                        uiProgressBar1.Value = 5;

                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetCheckedRows())
                    {
                        gridEX_Header.MoveTo(item);

                        foreach (Janus.Windows.GridEX.GridEXRow ChildItem in gridEX_Detail.GetRows())
                        {
                            if (!clGood.IsGoodInWare(short.Parse(mlt_Ware.Value.ToString()),
                                int.Parse(ChildItem.Cells["Column02"].Value.ToString())))
                                throw new WarningException("کالای " + ChildItem.Cells["Column02"].Text
                                    + " در انبار انتخاب شده فعال نمی باشد");
                            if (uiProgressBar1.Value < 100)
                                uiProgressBar1.Value += 1;
                        }
                    }

                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetCheckedRows())
                    {
                        gridEX_Header.MoveTo(item);

                        //در صورتی که فاکتور فاقد رسید، سند، باطل، و مرجوعی باشد رسید انبار زده می شود
                        if (clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column10", item.Cells["ColumnId"].Value.ToString()) == 0 &&
                            clDoc.OperationalColumnValue("Table_015_BuyFactor", "Column11", item.Cells["ColumnId"].Value.ToString()) == 0 &&
                            clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column17", "ColumnId", item.Cells["ColumnId"].Value.ToString()) == "False" &&
                            clDoc.ExScalar(ConSale.ConnectionString, "Table_015_BuyFactor", "Column19", "ColumnId", item.Cells["ColumnId"].Value.ToString()) == "False")
                        {
                            //*********Insert Header
                            int ResidID = InsertResidHeader(item, 0, FactorLs);
                            if (uiProgressBar1.Value < 100)
                                uiProgressBar1.Value += 1;
                            //*************درج کالاها و محاسبه ارزش
                            foreach (Janus.Windows.GridEX.GridEXRow ChildItem in gridEX_Detail.GetRows())
                            {
                                InsertResidDetail(ChildItem, ResidID, 0, Convert.ToInt32(item.Cells["columnid"].Value));
                                if (uiProgressBar1.Value < 100)
                                    uiProgressBar1.Value += 1;
                            }
                        }
                    }

                    uiProgressBar1.Value = 100;
                    if (FactorLs.Count > 0)
                        MessageBox.Show("ثبت رسید برای فاکتورهای زیر با موفقیت صورت گرفت" + Environment.NewLine +
                            string.Join(",", FactorLs.ToArray()), "", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);

                    else Class_BasicOperation.ShowMsg("", "رسیدی برای فاکتوری صادر نشد", "Stop");
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

        private int InsertResidHeader(Janus.Windows.GridEX.GridEXRow _HeaderItem, int DocID, List<string> NumbersLs)
        {
            int _ResidID = 0;
            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
            Key.Direction = ParameterDirection.Output;
            int _ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
            //, int.Parse(mlt_Ware.Value.ToString()));

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
                                                                          ) VALUES (" + _ResidNum + ",'" + _HeaderItem.Cells["Column02"].Value.ToString() + "'," +
                  mlt_Ware.Value.ToString() + "," + mlt_Function.Value.ToString() + "," + _HeaderItem.Cells["Column03"].Value.ToString() + ",'" + "رسید صادره بابت فاکتور خرید ش" +
                  _HeaderItem.Cells["Column01"].Value.ToString() + " تاریخ " + _HeaderItem.Cells["Column02"].Value.ToString() + "'," + DocID + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                  _HeaderItem.Cells["Column09"].Value.ToString() + "," + _HeaderItem.Cells["ColumnId"].Value.ToString()
                  + ",0,0,0,NULL,0,1,null); SET @Key=Scope_Identity()", Con);
                Insert.Parameters.Add(Key);
                Insert.ExecuteNonQuery();
                _ResidID = int.Parse(Key.Value.ToString());
                clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_015_BuyFactor SET Column10=" + _ResidID + " , Column11=" + DocID + " where ColumnId=" + _HeaderItem.Cells["ColumnId"].Value.ToString());
                NumbersLs.Add(_HeaderItem.Cells["Column01"].Value.ToString());
                return _ResidID;
            }
        }

        private void InsertResidHeader1(Janus.Windows.GridEX.GridEXRow _HeaderItem, int DocID, List<string> NumbersLs)
        {
            //            int _ResidID = 0;
            //            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
            //            Key.Direction = ParameterDirection.Output;
            //            int _ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
            //            //, int.Parse(mlt_Ware.Value.ToString()));

            //            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            //            {
            //                Con.Open();
            //                SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_011_PwhrsReceipt (
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
            //                                                                          ) VALUES (" + _ResidNum + ",'" + _HeaderItem.Cells["Column02"].Value.ToString() + "'," +
            //                  mlt_Ware.Value.ToString() + "," + mlt_Function.Value.ToString() + "," + _HeaderItem.Cells["Column03"].Value.ToString() + ",'" + "رسید صادره بابت فاکتور خرید ش" +
            //                  _HeaderItem.Cells["Column01"].Value.ToString() + " تاریخ " + _HeaderItem.Cells["Column02"].Value.ToString() + "'," + DocID + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
            //                  _HeaderItem.Cells["Column09"].Value.ToString() + "," + _HeaderItem.Cells["ColumnId"].Value.ToString()
            //                  + ",0,0,0,NULL,0,1,null); SET @Key=Scope_Identity()", Con);
            //                Insert.Parameters.Add(Key);
            //                Insert.ExecuteNonQuery();
            //                _ResidID = int.Parse(Key.Value.ToString());
            //                clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_015_BuyFactor SET Column10=" + _ResidID + " , Column11=" + DocID + " where ColumnId=" + _HeaderItem.Cells["ColumnId"].Value.ToString());
            //                NumbersLs.Add(_HeaderItem.Cells["Column01"].Value.ToString());
            //                return _ResidID;

            CommandTxt += @"INSERT INTO " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt (
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
                                                                          ) VALUES ((SELECT ISNULL(Max( Column01  ),0)+1 as ID from " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ),'" + _HeaderItem.Cells["Column02"].Value.ToString() + "'," +
              mlt_Ware.Value.ToString() + "," + mlt_Function.Value.ToString() + "," + _HeaderItem.Cells["Column03"].Value.ToString() + ",'" + "رسید صادره بابت فاکتور خرید ش" +
              _HeaderItem.Cells["Column01"].Value.ToString() + " تاریخ " + _HeaderItem.Cells["Column02"].Value.ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
              _HeaderItem.Cells["Column09"].Value.ToString() + "," + _HeaderItem.Cells["ColumnId"].Value.ToString()
              + ",0,0,0,NULL,0,1,null); SET @ResidID=Scope_Identity()";

        }

        private double InsertResidDetail(Janus.Windows.GridEX.GridEXRow item, int ResidID, double TotalValue, Int32 FactorID)
        {



            //// added by Roostaee 96/07/05 
            // اعمال تسهیم
            DataTable value = new DataTable();
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
                                                                                              AND tcbf.column01 = " + FactorID + @"
                                                                                   ) AS tt
                                                                        )

                                                                    SET @Net =isnull( (
                                                                            SELECT tbf.Column20
                                                                            FROM   Table_015_BuyFactor tbf
                                                                            WHERE  tbf.columnid = " + FactorID + @"
                                                                        ),0)
    
                                                                    SET @share = isnull(@sum /nullif( @Net,0),0)
                                                                    DECLARE @unitvalue   DECIMAL(18, 3),
                                                                            @totalvalue  DECIMAL(18, 3)

                                                                            SET @unitvalue =(CASE WHEN @share>0 then (
                                                                            ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column20)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + item.Cells["Column02"].Value + @"
                                                                                           AND tcbf.column01 = " + FactorID + @"
                                                                                ),
                                                                                0
                                                                            ) / nullif( ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column07)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + item.Cells["Column02"].Value + @"
                                                                                           AND tcbf.column01 = " + FactorID + @"
                                                                                ),
                                                                                0
                                                                            ),0)
                                                                        ) * (1 + @share) else isnull(" + Convert.ToDouble(item.Cells["Column20"].Value) + @" /nullif( " + Convert.ToDouble(item.Cells["Column07"].Value) + @",0),0) END)

                                                                    SET @totalvalue = @unitvalue * ISNULL(
                                                                            (
                                                                                SELECT SUM(tcbf.column07)
                                                                                FROM   Table_016_Child1_BuyFactor tcbf
                                                                                WHERE  tcbf.column02 = " + item.Cells["Column02"].Value + @"
                                                                                       AND tcbf.column01 = " + FactorID + @"
                                                                            ),
                                                                            0
                                                                        )  

                                                                    SELECT 1+@share AS share,
                                                                          isnull( @unitvalue,0) AS unitvalue,
                                                                         isnull(  @totalvalue,0) AS totalvalue

                                                                    ", ConSale);
            Adapter.Fill(value);



            //درج کالاهای موجود در رسید 
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
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
           ,[Column35]) VALUES (" + ResidID + "," + item.Cells["Column02"].Value.ToString() + "," +
                            item.Cells["Column03"].Value.ToString() + "," + item.Cells["Column04"].Value.ToString() + "," + item.Cells["Column05"].Value.ToString() + "," + item.Cells["Column06"].Value.ToString() + "," + item.Cells["Column07"].Value.ToString() + "," +
                            item.Cells["Column08"].Value.ToString() + "," + item.Cells["Column09"].Value.ToString() + "," + item.Cells["Column10"].Value.ToString() + "," + item.Cells["Column11"].Value.ToString() + ",NULL," +
                            (item.Cells["Column21"].Value.ToString().Trim() == "" ? "NULL" : item.Cells["Column21"].Value.ToString()) + "," + (item.Cells["Column22"].Value.ToString().Trim() == "" ? "NULL" : item.Cells["Column22"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                            + "',getdate(),0," + (Convert.ToDouble(item.Cells["Column07"].Value.ToString()) > 0 ? (Convert.ToDouble(item.Cells["Column20"].Value.ToString()) / Convert.ToDouble(item.Cells["Column07"].Value.ToString())) * Convert.ToDouble(value.Rows[0]["share"]) : 0) +
                            "," + (Convert.ToDouble(item.Cells["Column20"].Value) * Convert.ToDouble(value.Rows[0]["share"])) + "," + item.Cells["ColumnId"].Value.ToString()
                            + ",NULL,NULL,NULL,0,0,0,0," +
                            (item.Cells["Column32"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column32"].Value.ToString() + "'") + "," +
                            (item.Cells["Column33"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column33"].Value.ToString() + "'") + "," + item.Cells["Column29"].Value.ToString() +
                            "," + item.Cells["Column30"].Value.ToString() + "," + item.Cells["Column34"].Value.ToString() + "," + item.Cells["Column35"].Value.ToString() + ")", Con);
                InsertDetail.ExecuteNonQuery();
                TotalValue += Convert.ToDouble(item.Cells["Column20"].Value.ToString());
                return TotalValue;
            }
            //using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            //{
            //    Con.Open();
            //    SqlCommand InsertDetail = new SqlCommand("INSERT INTO Table_012_Child_PwhrsReceipt VALUES (" + ResidID + "," + item.Cells["Column02"].Value.ToString() + "," +
            //                item.Cells["Column03"].Value.ToString() + "," + item.Cells["Column04"].Value.ToString() + "," + item.Cells["Column05"].Value.ToString() + "," + item.Cells["Column06"].Value.ToString() + "," + item.Cells["Column07"].Value.ToString() + "," +
            //                item.Cells["Column08"].Value.ToString() + "," + item.Cells["Column09"].Value.ToString() + "," + item.Cells["Column10"].Value.ToString() + "," + item.Cells["Column11"].Value.ToString() + ",NULL," +
            //                (item.Cells["Column21"].Value.ToString().Trim() == "" ? "NULL" : item.Cells["Column21"].Value.ToString()) + "," + (item.Cells["Column22"].Value.ToString().Trim() == "" ? "NULL" : item.Cells["Column22"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
            //                + "',getdate(),0," +Convert.ToDouble( item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(value.Rows[0]["share"]) + "," + Convert.ToDouble(item.Cells["Column11"].Value.ToString()) * Convert.ToDouble(value.Rows[0]["share"]) + "," + item.Cells["ColumnId"].Value.ToString()
            //                + ",NULL,NULL,NULL,0,0,0,0,NULL,NULL," + item.Cells["Column29"].Value.ToString() +
            //                "," + item.Cells["Column30"].Value.ToString() + ")", Con);
            //    InsertDetail.ExecuteNonQuery();
            //    TotalValue += Convert.ToDouble(item.Cells["Column20"].Value.ToString());
            //    return TotalValue;
            //}
        }
        private void InsertResidDetail1(Janus.Windows.GridEX.GridEXRow item, int ResidID, double TotalValue, Int32 FactorID)
        {



            //// added by Roostaee 96/07/05 
            // اعمال تسهیم
            DataTable value = new DataTable();
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
                                                                                              AND tcbf.column01 = " + FactorID + @"
                                                                                   ) AS tt
                                                                        )

                                                                    SET @Net =isnull( (
                                                                            SELECT tbf.Column20
                                                                            FROM   Table_015_BuyFactor tbf
                                                                            WHERE  tbf.columnid = " + FactorID + @"
                                                                        ),0)
    
                                                                    SET @share = isnull(@sum /nullif( @Net,0),0)
                                                                    DECLARE @unitvalue   DECIMAL(18, 3),
                                                                            @totalvalue  DECIMAL(18, 3)

                                                                            SET @unitvalue =(CASE WHEN @share>0 then (
                                                                            ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column20)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + item.Cells["Column02"].Value + @"
                                                                                           AND tcbf.column01 = " + FactorID + @"
                                                                                ),
                                                                                0
                                                                            ) / nullif( ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column07)
                                                                                    FROM   Table_016_Child1_BuyFactor tcbf
                                                                                    WHERE  tcbf.column02 = " + item.Cells["Column02"].Value + @"
                                                                                           AND tcbf.column01 = " + FactorID + @"
                                                                                ),
                                                                                0
                                                                            ),0)
                                                                        ) * (1 + @share) else isnull(" + Convert.ToDouble(item.Cells["Column20"].Value) + @" /nullif( " + Convert.ToDouble(item.Cells["Column07"].Value) + @",0),0) END)

                                                                    SET @totalvalue = @unitvalue * ISNULL(
                                                                            (
                                                                                SELECT SUM(tcbf.column07)
                                                                                FROM   Table_016_Child1_BuyFactor tcbf
                                                                                WHERE  tcbf.column02 = " + item.Cells["Column02"].Value + @"
                                                                                       AND tcbf.column01 = " + FactorID + @"
                                                                            ),
                                                                            0
                                                                        )  

                                                                    SELECT 1+@share AS share,
                                                                          isnull( @unitvalue,0) AS unitvalue,
                                                                         isnull(  @totalvalue,0) AS totalvalue

                                                                    ", ConSale);
            Adapter.Fill(value);




            CommandTxt += @"INSERT INTO " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt([column01]
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
           ,[Column35]) VALUES (@ResidID," + item.Cells["Column02"].Value.ToString() + "," +
                            item.Cells["Column03"].Value.ToString() + "," + item.Cells["Column04"].Value.ToString() + "," + item.Cells["Column05"].Value.ToString() + "," + item.Cells["Column06"].Value.ToString() + "," + item.Cells["Column07"].Value.ToString() + "," +
                            item.Cells["Column08"].Value.ToString() + "," + item.Cells["Column09"].Value.ToString() + "," + item.Cells["Column10"].Value.ToString() + "," + item.Cells["Column11"].Value.ToString() + ",NULL," +
                            (item.Cells["Column21"].Value.ToString().Trim() == "" ? "NULL" : item.Cells["Column21"].Value.ToString()) + "," + (item.Cells["Column22"].Value.ToString().Trim() == "" ? "NULL" : item.Cells["Column22"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                            + "',getdate(),0," + (Convert.ToDouble(item.Cells["Column07"].Value.ToString()) > 0 ? (Convert.ToDouble(item.Cells["Column20"].Value.ToString()) / Convert.ToDouble(item.Cells["Column07"].Value.ToString())) * Convert.ToDouble(value.Rows[0]["share"]) : 0) +
                            "," + (Convert.ToDouble(item.Cells["Column20"].Value) * Convert.ToDouble(value.Rows[0]["share"])) + "," + item.Cells["ColumnId"].Value.ToString()
                            + ",NULL,NULL,NULL,0,0,0,0," +
                            (item.Cells["Column32"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column32"].Value.ToString() + "'") + "," +
                            (item.Cells["Column33"].Text.ToString().Trim() == "" ? "NULL" : "'" + item.Cells["Column33"].Value.ToString() + "'") + "," + item.Cells["Column29"].Value.ToString() +
                            "," + item.Cells["Column30"].Value.ToString() + "," + item.Cells["Column34"].Value.ToString() + "," + item.Cells["Column35"].Value.ToString() + ")";

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
                        Class_BasicOperation.ShowMsg("", "با توجه به اخطارهای نمایش داده شده امکان صدور سند حسابداری وجود ندارد", "Warning");
                        return;
                    }

                    uiProgressBar1.Value = 10;

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به صدور سند حسابداری و رسید انبار هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        List<string> NumbersLs = new List<string>();

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

                                bt_ExportDoc.Enabled = false;
                                this.DialogResult = DialogResult.Yes;
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
                                CommandTxt = @"declare @Key int  declare @DocNum int  declare @DetailID int declare @ResidID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)  ";


                                //صدور سند
                                //int DocNum = 0;
                                //int DocID = 0;



                                if (uiProgressBar1.Value < 100)
                                    uiProgressBar1.Value += 1;
                                gridEX_Header.MoveTo(item1);


                                //صدور رسید
                                int ResidID = 0;
                                //  InsertResidHeader1(item1, DocID, NumbersLs);
                                CommandTxt += @"INSERT INTO " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt (
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
                                                                          ) VALUES ((SELECT ISNULL(Max( Column01  ),0)+1 as ID from " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ),'" + item1.Cells["Column02"].Value.ToString() + "'," +
                      mlt_Ware.Value.ToString() + "," + mlt_Function.Value.ToString() + "," + item1.Cells["Column03"].Value.ToString() + ",'" + "رسید صادره بابت فاکتور خرید ش" +
                      item1.Cells["Column01"].Value.ToString() + " تاریخ " + item1.Cells["Column02"].Value.ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                      item1.Cells["Column09"].Value.ToString() + "," + item1.Cells["ColumnId"].Value.ToString()
                      + ",0,0,0,NULL,0,1,null); SET @ResidID=Scope_Identity()";


                                if (uiProgressBar1.Value < 100)
                                    uiProgressBar1.Value += 1;

                                double TotalValue = 0;
                                //*************درج کالاها و محاسبه ارزش
                                foreach (Janus.Windows.GridEX.GridEXRow ChildItem in gridEX_Detail.GetRows())
                                {
                                    InsertResidDetail1(ChildItem, ResidID, TotalValue, Convert.ToInt32(item1.Cells["columnid"].Value));
                                    if (uiProgressBar1.Value < 100)
                                        uiProgressBar1.Value += 1;
                                }
                                CommandTxt += "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor SET Column10=@ResidID  where ColumnId=" + item1.Cells["ColumnId"].Value.ToString();


                                Int64 NetPrice = Convert.ToInt64(Convert.ToDouble(item1.Cells["Column20"].Value.ToString()));
                                Int64 TotalExtra = Convert.ToInt64(Convert.ToDouble(item1.Cells["Column21"].Value.ToString()));
                                Int64 Reduction = Convert.ToInt64(Convert.ToDouble(item1.Cells["Column22"].Value.ToString()));

                                //ثبت مربوط به خالص فاکتور فروش
                                if (NetPrice > 0)
                                {
                                    string[] _Bed = clDoc.ACC_Info(mlt_BuyBed.Value.ToString());
                                    string[] _Bes = clDoc.ACC_Info(mlt_BuyBes.Value.ToString());

                                    //بدهکار
                                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT    Center, Project, Total, Discount, Adding, column01, Total - Discount + Adding AS Net
                               FROM         (SELECT     Column21 as Center,column22 AS Project, ISNULL(SUM(column11), 0) AS Total, ISNULL(SUM(column17), 0) AS Discount, ISNULL(SUM(column19), 0) AS Adding, column01
                                 FROM          dbo.Table_016_Child1_BuyFactor
                                 GROUP BY column21,column22, column01
                                  HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                                    Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, item1.Cells["ColumnId"].Value.ToString());
                                    DataTable Table = new DataTable();
                                    Adapter.Fill(Table);
                                    foreach (DataRow GroupRow in Table.Rows)
                                    {
                                        //clDoc.ExportDoc_Detail(DocID, mlt_BuyBed.Value.ToString(), short.Parse(_Bed[0]), _Bed[1], _Bed[2], _Bed[3], _Bed[4], "NULL", (GroupRow["Center"].ToString().Trim() == "" ? null : GroupRow["Center"].ToString()),
                                        //    (GroupRow["Project"].ToString().Trim() == "" ? null : GroupRow["Project"].ToString()), "فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                        //    Convert.ToInt64(Convert.ToDouble(GroupRow["Net"].ToString())), 0, 0, 0, -1, 19, int.Parse(item1.Cells["ColumnID"].Value.ToString()), Class_BasicOperation._UserName, 0);

                                        CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                    VALUES(" + DocID.Value + @",'" + mlt_BuyBed.Value.ToString() + @"',
                                " + Int16.Parse(_Bed[0].ToString()) + ",'" + _Bed[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bed[2].ToString()) ? "NULL" : "'" + _Bed[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[3].ToString()) ? "NULL" : "'" + _Bed[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[4].ToString()) ? "NULL" : "'" + _Bed[4].ToString() + "'") + @",
                                 NULL ,
                                 " + (string.IsNullOrWhiteSpace(GroupRow["Center"].ToString().Trim()) ? "NULL" : GroupRow["Center"].ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(GroupRow["Project"].ToString().Trim()) ? "NULL" : GroupRow["Project"].ToString()) + @",
                   " + "'فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                        " + Convert.ToInt64(Convert.ToDouble(GroupRow["Net"].ToString())) + @",
                        0,0,0,-1,19," + int.Parse(item1.Cells["ColumnID"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                               Class_BasicOperation._UserName + "',getdate(),0); ";

                                    }

                                    //بستانکار
                                    //clDoc.ExportDoc_Detail( DocID, mlt_BuyBes.Value.ToString(), short.Parse(_Bes[0]), _Bes[1], _Bes[2], _Bes[3], _Bes[4],
                                    //    item1.Cells["Column03"].Value.ToString(), "NULL", "NULL", "فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                    //    0,NetPrice, 0, 0, -1, 19, int.Parse(item1.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0);


                                    CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                    VALUES(" + DocID.Value + @",'" + mlt_BuyBes.Value.ToString() + @"',
                                " + Int16.Parse(_Bes[0].ToString()) + ",'" + _Bes[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bes[2].ToString()) ? "NULL" : "'" + _Bes[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[3].ToString()) ? "NULL" : "'" + _Bes[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[4].ToString()) ? "NULL" : "'" + _Bes[4].ToString() + "'") + @",
                                 " + (string.IsNullOrWhiteSpace(item1.Cells["Column03"].Value.ToString()) ? "NULL" : item1.Cells["Column03"].Value.ToString()) + @",

                                 NULL ,
                                NULL ,
                   " + "'فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                        0,
                        " + NetPrice + ",0,0,-1,19," + int.Parse(item1.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                               Class_BasicOperation._UserName + "',getdate(),0); ";



                                    if (uiProgressBar1.Value < 100)
                                        uiProgressBar1.Value += 1;
                                }

                                //ثبت مربوط به اضافات و کسورات
                                foreach (Janus.Windows.GridEX.GridEXRow item3 in gridEX_Extra.GetRows())
                                {
                                    ExRedBindingSource.Filter = "ColumnId=" + item3.Cells["Column02"].Value.ToString();

                                    // کسورات
                                    //اگر کسورات بود شخص بدهکار می شود
                                    if (item3.Cells["Column05"].Value.ToString() == "True")
                                    {
                                        string[] _Bed = clDoc.ACC_Info(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString());
                                        string[] _Bes = clDoc.ACC_Info(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString());

                                        //clDoc.ExportDoc_Detail( DocID, ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString(), short.Parse(_Bed[0]),
                                        //    _Bed[1], _Bed[2], _Bed[3], _Bed[4], item1.Cells["Column03"].Value.ToString(), null, null, item3.Cells["Column02"].Text + "-" + " فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                        //    Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())), 0, 0, 0, -1, 19, int.Parse(item1.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0);



                                        CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                             ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                            VALUES(" + DocID.Value + @",'" + ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString() + @"',
                                " + Int16.Parse(_Bed[0].ToString()) + ",'" + _Bed[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bed[2].ToString()) ? "NULL" : "'" + _Bed[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[3].ToString()) ? "NULL" : "'" + _Bed[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[4].ToString()) ? "NULL" : "'" + _Bed[4].ToString() + "'") + @",
                                 " + (string.IsNullOrWhiteSpace(item1.Cells["Column03"].Value.ToString()) ? "NULL" : item1.Cells["Column03"].Value.ToString()) + @",
                                 NULL ,
                                NULL ,
                               '" + item3.Cells["Column02"].Text + "-" + " فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                    " + Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())) + @",
                                   0,0,0,-1,19," + int.Parse(item1.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                          Class_BasicOperation._UserName + "',getdate(),0); ";




                                        //clDoc.ExportDoc_Detail( DocID, ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString(), short.Parse(_Bes[0]),
                                        //    _Bes[1], _Bes[2], _Bes[3], _Bes[4], null, null, null, item3.Cells["Column02"].Text + "-" + " فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                        //    0, Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())), 0, 0, -1, 19, int.Parse(item1.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0);


                                        CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                             ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                            VALUES(" + DocID.Value + @",'" + ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString() + @"',
                                " + Int16.Parse(_Bes[0].ToString()) + ",'" + _Bes[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bes[2].ToString()) ? "NULL" : "'" + _Bes[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[3].ToString()) ? "NULL" : "'" + _Bes[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[4].ToString()) ? "NULL" : "'" + _Bes[4].ToString() + "'") + @",
                                  NULL ,
                                 NULL ,
                                NULL ,
                               '" + item3.Cells["Column02"].Text + "-" + " فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                    0,
                                   " + Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())) + ",0,0,-1,19," + int.Parse(item1.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                              Class_BasicOperation._UserName + "',getdate(),0); ";


                                    }
                                    else if (item3.Cells["Column05"].Value.ToString() == "False")
                                    {
                                        string[] _Bed = clDoc.ACC_Info(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString());
                                        string[] _Bes = clDoc.ACC_Info(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString());

                                        //clDoc.ExportDoc_Detail( DocID, ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString(), short.Parse(_Bed[0]),
                                        //    _Bed[1], _Bed[2], _Bed[3], _Bed[4],null , null, null, item3.Cells["Column02"].Text + "-" + " فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                        //    Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())), 0, 0, 0, -1, 19, int.Parse(item1.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0);



                                        CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                             ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                            VALUES(" + DocID.Value + @",'" + ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString() + @"',
                                " + Int16.Parse(_Bed[0].ToString()) + ",'" + _Bed[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bed[2].ToString()) ? "NULL" : "'" + _Bed[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[3].ToString()) ? "NULL" : "'" + _Bed[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bed[4].ToString()) ? "NULL" : "'" + _Bed[4].ToString() + "'") + @",
                                  NULL ,
                                 NULL ,
                                NULL ,
                               ' فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                    " + Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())) + @",
                                   0,0,0,-1,19," + int.Parse(item1.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                               Class_BasicOperation._UserName + "',getdate(),0); ";







                                        //clDoc.ExportDoc_Detail( DocID, ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString(), short.Parse(_Bes[0]),
                                        //    _Bes[1], _Bes[2], _Bes[3], _Bes[4], item1.Cells["Column03"].Value.ToString(), null, null, item3.Cells["Column02"].Text + "-" + " فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString(),
                                        //    0, Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())), 0, 0, -1, 19, int.Parse(item1.Cells["ColumnId"].Value.ToString()), Class_BasicOperation._UserName, 0);



                                        CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                             ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                                            VALUES(" + DocID.Value + @",'" + ((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString() + @"',
                                " + Int16.Parse(_Bes[0].ToString()) + ",'" + _Bes[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_Bes[2].ToString()) ? "NULL" : "'" + _Bes[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[3].ToString()) ? "NULL" : "'" + _Bes[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_Bes[4].ToString()) ? "NULL" : "'" + _Bes[4].ToString() + "'") + @",
                                 " + (string.IsNullOrWhiteSpace(item1.Cells["Column03"].Value.ToString()) ? "NULL" : item1.Cells["Column03"].Value.ToString()) + @",
                                
                                 NULL ,
                                NULL ,
                               ' " + item3.Cells["Column02"].Text + "-" + " فاکتور خرید ش " + item1.Cells["Column01"].Value.ToString() + " به تاریخ " + item1.Cells["Column02"].Value.ToString() + @"',
                                   0,
                                   " + Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())) + ",0,0,-1,19," + int.Parse(item1.Cells["ColumnId"].Value.ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                 Class_BasicOperation._UserName + "',getdate(),0); ";







                                    }



                                    if (uiProgressBar1.Value < 100)
                                        uiProgressBar1.Value += 1;
                                }
                                if (NetPrice > 0)
                                {
                                    CommandTxt += "UPDATE " + ConSale.Database + ".dbo.Table_015_BuyFactor SET Column10=@ResidID, Column11=" + DocID.Value + @" where ColumnId=" + item1.Cells["ColumnId"].Value.ToString();
                                    CommandTxt += " UPDATE " + this.ConWare.Database + ".dbo.Table_011_PwhrsReceipt SET  column07=" + DocID.Value + @" where ColumnId= @ResidID";
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
                                        NumbersLs.Add(item1.Cells["Column01"].Value.ToString());
                                        bt_ExportDoc.Enabled = false;
                                        this.DialogResult = DialogResult.Yes;
                                    }
                                    catch (Exception es)
                                    {
                                        sqlTran.Rollback();
                                        notok += item1.Cells["Column01"].Value.ToString() + ",";
                                    }

                                    this.Cursor = Cursors.Default;



                                }
                            }
                            uiProgressBar1.Value = 100;

                            if (!string.IsNullOrWhiteSpace(notok))
                                MessageBox.Show("سند و رسید فاکتورهای زیر صادر نشد" + Environment.NewLine + notok.TrimEnd(','));
                            else
                                MessageBox.Show("سند و رسید فاکتورها با موفقیت صادر شد");
                        }
                        bt_Refresh_Click(sender, e);
                        uiProgressBar1.Value = 0;
                    }

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

        private void CheckEssentialItems(object sender, EventArgs e)
        {
            uiPanel4.Text = "";
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

            //جک ورود اطلاعات الزامی
            if (txt_Cover.Text.Trim() == "" || !faDatePicker1.SelectedDateTime.HasValue || mlt_BuyBed.Text.Trim() == "" || mlt_BuyBes.Text.Trim() == "")
                throw new WarningException("اطلاعات مربوط به صدور سند را کامل کنید");
            if (mlt_Ware.Text.Trim() == "" || mlt_Function.Text.ToString() == "")
                throw new WarningException("اطلاعات مربوط به صدور رسید را کامل کنید");



            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

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
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetCheckedRows())
            {
                gridEX_Header.MoveTo(item);
                //کنترل حساب بستانکار فاکتور و شخص فاکتور
                try
                {
                    clCredit.All_Controls_2(mlt_BuyBes.Value.ToString(), int.Parse(item.Cells["Column03"].Value.ToString()), null, null);
                }
                catch (Exception es)
                {
                    item.BeginEdit();
                    item.Cells["Error"].Value = true;
                    item.Cells["ErrorDes"].Value = es.Message;
                    item.EndEdit();
                }
                //کنترل اعتبار شخص در حساب بستانکاری
                TPerson.Rows.Add(int.Parse(item.Cells["Column03"].Value.ToString()), mlt_BuyBes.Value.ToString(), Convert.ToDouble(item.Cells["Column20"].Value.ToString()) * -1);
                TAccounts.Rows.Add(mlt_BuyBed.Value.ToString(), Convert.ToDouble(item.Cells["Column20"].Value.ToString()));
                TAccounts.Rows.Add(mlt_BuyBes.Value.ToString(), Convert.ToDouble(item.Cells["Column20"].Value.ToString()) * -1);

                //کنترل حساب بدهکار فاکتور و پروژه در کالاها
                short? Center = null;
                short? Project = null;
                foreach (Janus.Windows.GridEX.GridEXRow Ditem in gridEX_Detail.GetRows())
                {
                    if (!clGood.IsGoodInWare(short.Parse(mlt_Ware.Value.ToString()),
                           int.Parse(Ditem.Cells["Column02"].Value.ToString())))
                        throw new WarningException("کالای " + Ditem.Cells["Column02"].Text
                            + " در انبار انتخاب شده فعال نمی باشد");

                    Center = null;
                    Project = null;

                    if (Ditem.Cells["Column22"].Text.Trim() != "" || Ditem.Cells["Column21"].Text.Trim() != "")
                    {
                        if (Ditem.Cells["Column21"].Text.Trim() != "")
                            Center = short.Parse(Ditem.Cells["Column21"].Value.ToString());
                        if (Ditem.Cells["Column22"].Text.Trim() != "")
                            Project = short.Parse(Ditem.Cells["Column22"].Value.ToString());
                    }
                    try
                    {
                        clCredit.All_Controls_2(mlt_BuyBed.Value.ToString(), null, Center, Project);
                    }
                    catch (Exception es)
                    {
                        item.BeginEdit();
                        item.Cells["Error"].Value = true;
                        item.Cells["ErrorDes"].Value = es.Message;
                        item.EndEdit();
                    }

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
                    //کنترل اضافات
                    else
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
                            .ToString(), Convert.ToInt64(Convert.ToDouble(item3.Cells["Column04"].Value.ToString())) * -1);

                    }
                    TAccounts.Rows.Add(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column10"].ToString(), Convert.ToDouble(item3.Cells["Column04"].Value.ToString()));
                    TAccounts.Rows.Add(((DataRowView)ExRedBindingSource.CurrencyManager.Current)["Column16"].ToString(), Convert.ToDouble(item3.Cells["Column04"].Value.ToString()) * -1);
                }

            }
            //انتهای کنترل الزامها
            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);
            gridEX_Header.UpdateData();



        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            dataSet_Buy.EnforceConstraints = false;
            DS.Tables["Extra"].Rows.Clear();
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_024_Discount_Buy", ConSale);
            Adapter.Fill(DS, "Extra");
            this.table_015_BuyFactor_NoDoc_NoReceiptTableAdapter.Fill_NoDoc_NoReceipt(this.dataSet_Buy.Table_015_BuyFactor_NoDoc_NoReceipt);
            this.table_016_Child1_BuyFactor_NoDoc_NoReceiptTableAdapter.Fill(this.dataSet_Buy.Table_016_Child1_BuyFactor_NoDoc_NoReceipt);
            this.table_017_Child2_BuyFactor_NoDoc_NoReceiptTableAdapter.Fill(this.dataSet_Buy.Table_017_Child2_BuyFactor_NoDoc_NoReceipt);
            dataSet_Buy.EnforceConstraints = true;
            this.table_015_BuyFactor_NoDoc_NoReceiptBindingSource_PositionChanged(sender, e);
        }

        private void Frm_013_ExportDoc_Receipt_InTotal_KeyDown(object sender, KeyEventArgs e)
        {
            if (bt_ExportReceipts.Enabled && e.Control && e.KeyCode == Keys.D)
                bt_ExportReceipts_Click(sender, e);
            else if (bt_ExportDoc.Enabled && e.Control && e.KeyCode == Keys.E)
                bt_ExportDoc_Click(sender, e);
            else if (e.KeyCode == Keys.F5)
                bt_Refresh_Click(sender, e);
        }

        private void table_015_BuyFactor_NoDoc_NoReceiptBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (this.table_015_BuyFactor_NoDoc_NoReceiptBindingSource.Count > 0)
            {
                try
                {
                    txt_EndPrice.Value = Convert.ToInt64(txt_TotalPrice.Value.ToString()) +
                        Convert.ToInt64(txt_Extra.Value.ToString()) -
                        Convert.ToInt64(txt_Reductions.Value.ToString());
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
            }
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

        private void mnu_ViewReceipts_Click(object sender, EventArgs e)
        {
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 22))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form04_ViewWareReceipt")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                PWHRS._03_AmaliyatAnbar.Form04_ViewWareReceipt frm = new PWHRS._03_AmaliyatAnbar.Form04_ViewWareReceipt();
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX_Header_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                uiPanel4.Text = gridEX_Header.GetValue("ErrorDes").ToString();
            }
            catch
            {
            }
        }
    }
}
