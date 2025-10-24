using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._05_Sale.LegalFactors
{
    public partial class Frm_018_LegalFactors : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlDataAdapter HeaderAdapter, DetailAdapter;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        SqlParameter ParamDate1, ParamDate2;
        DataSet DS = new DataSet();

        public Frm_018_LegalFactors()
        {

            InitializeComponent();
        }

        private void Frm_018_LegalFactors_Load(object sender, EventArgs e)
        {

            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddDays(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            ParamDate1 = new SqlParameter("Date1", SqlDbType.NVarChar, 10);
            ParamDate1.Direction = ParameterDirection.Input;
            ParamDate1.Value = faDatePickerStrip1.FADatePicker.Text;
            ParamDate2 = new SqlParameter("Date2", SqlDbType.NVarChar, 10);
            ParamDate2.Direction = ParameterDirection.Input;
            ParamDate2.Value = faDatePickerStrip2.FADatePicker.Text;

            DataTable GoodTable = clDoc.ReturnTable(ConWare.ConnectionString,
                "Select ColumnId as GoodID,Column01 as GoodCode,Column02 as GoodName from table_004_CommodityAndIngredients");
            gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            gridEX_List.DropDowns["Project"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_035_ProjectInfo"), "");
            gridEX_List.DropDowns["Center"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo"), "");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_070_CountUnitInfo"), "");

            gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
            gridEX_Header.DropDowns["Draft"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from Table_007_PwhrsDraft"), "");
            gridEX_Header.DropDowns["Customer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo"), "");

            HeaderAdapter = new SqlDataAdapter("SELECT * FROM Table_010_SaleFactor WHERE (column02>=@Date1 AND Column02<=@Date2)", ConSale);
            HeaderAdapter.SelectCommand.Parameters.Add(ParamDate1);
            HeaderAdapter.SelectCommand.Parameters.Add(ParamDate2);
            HeaderAdapter.Fill(DS, "Header");
            gridEX_Header.DataSource = DS.Tables["Header"];

            DetailAdapter = new SqlDataAdapter(@"SELECT        Table_011_Child1_SaleFactor.columnid, Table_011_Child1_SaleFactor.column01, Table_011_Child1_SaleFactor.column02, Table_011_Child1_SaleFactor.column03, 
            Table_011_Child1_SaleFactor.column04, Table_011_Child1_SaleFactor.column05, Table_011_Child1_SaleFactor.column06, Table_011_Child1_SaleFactor.column07,
            Table_011_Child1_SaleFactor.column08, Table_011_Child1_SaleFactor.column09, Table_011_Child1_SaleFactor.column10, 
            Table_011_Child1_SaleFactor.column11, Table_011_Child1_SaleFactor.column12, Table_011_Child1_SaleFactor.column13, Table_011_Child1_SaleFactor.column14,
            Table_011_Child1_SaleFactor.column15, Table_011_Child1_SaleFactor.column16, Table_011_Child1_SaleFactor.column17, 
            Table_011_Child1_SaleFactor.column18, Table_011_Child1_SaleFactor.column19, Table_011_Child1_SaleFactor.column20, Table_011_Child1_SaleFactor.column21,
            Table_011_Child1_SaleFactor.column22, Table_011_Child1_SaleFactor.column23, Table_011_Child1_SaleFactor.column24, 
            Table_011_Child1_SaleFactor.column25, Table_011_Child1_SaleFactor.column26, Table_011_Child1_SaleFactor.column27, Table_011_Child1_SaleFactor.column28,
            Table_011_Child1_SaleFactor.column29, Table_011_Child1_SaleFactor.column30, Table_011_Child1_SaleFactor.Column31, 
            Table_011_Child1_SaleFactor.Column32
            FROM            Table_011_Child1_SaleFactor INNER JOIN
            Table_010_SaleFactor ON Table_011_Child1_SaleFactor.column01 = Table_010_SaleFactor.columnid
            WHERE        (Table_010_SaleFactor.column02>=@Date1 AND Table_010_SaleFactor.column02<=@Date2)", ConSale);

            ParamDate1 = new SqlParameter("Date1", faDatePickerStrip1.FADatePicker.Text);
            ParamDate2 = new SqlParameter("Date2", faDatePickerStrip2.FADatePicker.Text);
            DetailAdapter.SelectCommand.Parameters.Add(ParamDate1);
            DetailAdapter.SelectCommand.Parameters.Add(ParamDate2);
            DetailAdapter.Fill(DS, "Detail");



            DataRelation R_Header_Detail = new DataRelation("R_Header_Detail", DS.Tables["Header"].Columns["ColumnId"],
                DS.Tables["Detail"].Columns["column01"], false);

            ForeignKeyConstraint Fkc = new ForeignKeyConstraint("F_Header_Detail", DS.Tables["Header"].Columns["ColumnId"],
                DS.Tables["Detail"].Columns["column01"]);
            Fkc.AcceptRejectRule = AcceptRejectRule.None;
            Fkc.UpdateRule = Rule.Cascade;
            Fkc.DeleteRule = Rule.None;
            DS.Tables["Detail"].Constraints.Add(Fkc);
            DS.Relations.Add(R_Header_Detail);


            gridEX_List.DataSource = DS.Tables["Header"];
            gridEX_List.DataMember = "R_Header_Detail";


            SqlCommandBuilder Headerbuilder = new SqlCommandBuilder(HeaderAdapter);
            Headerbuilder.GetUpdateCommand();
            HeaderAdapter.UpdateCommand = Headerbuilder.GetUpdateCommand();

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            gridEX_Header.MoveToNewRecord();
            if (DialogResult.Yes == MessageBox.Show("آیا مایل به شماره گذاری فاکتورهای انتخاب شده هستید؟", "", MessageBoxButtons.YesNo,
                 MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
            {
                this.Cursor = Cursors.WaitCursor;
                HeaderAdapter.Update(DS.Tables["Header"]);
                DS.Tables["Detail"].AcceptChanges();

                using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE))
                {
                    ConSale.Open();
                    SqlCommand Command = new SqlCommand("EXEC	[dbo].[FactorNumbering]", ConSale);
                    Command.ExecuteNonQuery();

                    InsertInLegalfactors();

                    bt_Search_Click(sender, e);

                }
                this.Cursor = Cursors.Default;
                Class_BasicOperation.ShowMsg("", "ذخیره/شماره گذاری اطلاعات با موفقیت انجام شد", "Information");


            }
        }

        private void InsertInLegalfactors()
        {
            DataTable Table = clDoc.ReturnTable(Properties.Settings.Default.SALE,
            @"Select ColumnId,Column37,Column39 from Table_010_SaleFactor
                    where Column38=1 order by Column37");
                for(int i=0; i<Table.Rows.Count;i++)
            {
                if (Table.Rows[i]["Column39"].ToString() != "")
                {
                    clDoc.RunSqlCommand(Properties.Settings.Default.SALE,
                    "UPDATE Table_055_LegalFactors SET Column01=" +
                    Table.Rows[i]["Column37"].ToString() + " where ColumnId=" + Table.Rows[i]["Column39"].ToString());
                }
                else
                {
                    using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE))
                    {
                        ConSale.Open();
                        SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                        key.Direction = ParameterDirection.Output;
                        SqlCommand InsertHeader = new SqlCommand(
                        @"insert into Table_055_LegalFactors ([column01],
                    [column02], [column03], [column04], [column05], 
                    [column06], [column11], [column12], [column13],
                    [column14], [column15], [column16], [column17],
                    [column18], [column19], [column20], [column21],
                    [column22], [column23], [column24], [column25],
                    [column26], [column27], [Column28], [Column29],
                    [Column30], [Column31], [Column32], [Column33],
                    [Column34], [Column35], [Column36], 
                    [Column40], [Column41],[Column37] 
                    )
                    select " + Table.Rows[i]["Column37"].ToString() + @", [column02], [column03], [column04], 
                    [column05], [column06],[column11], [column12], [column13], [column14],
                    [column15], [column16], [column17], [column18], [column19], 
                    [column20], [column21], [column22], [column23], [column24], 
                    [column25], [column26], [column27], [Column28], [Column29],
                    [Column30], [Column31], [Column32], [Column33], [Column34],
                    [Column35], [Column36], [Column40], [Column41], " + Table.Rows[i]["ColumnId"].ToString() + @"
                    from Table_010_SaleFactor where columnid=" + Table.Rows[i]["ColumnId"].ToString()
                        + " ; SET @Key=SCOPE_IDENTITY()", ConSale);
                        InsertHeader.Parameters.Add(key);
                        InsertHeader.ExecuteNonQuery();

                        SqlCommand InsertDetail = new SqlCommand(@"insert Into Table_060_Child1_LegalFactors 
                    ([column01],[column02],[column03],[column04],[column05]
                    ,[column06],[column07],[column08],[column09],[column10]
                    ,[column11],[column12],[column13],[column14],[column15]
                    ,[column16],[column17],[column18],[column19],[column20]
                    ,[column21],[column22],[column23],[column24],[column25]
                    ,[column26],[column27],[column28],[column29],[column30]
                    ,[Column31],[Column32])
                    select " + key.Value.ToString() + @",[column02],[column03],[column04],[column05]
                    ,[column06],[column07],[column08],[column09],[column10]
                    ,[column11],[column12],[column13],[column14],[column15]
                    ,[column16],[column17],[column18],[column19],[column20]
                    ,[column21],[column22],[column23],[column24],[column25]
                    ,[column26],[column27],[column28],[column29],[column30]
                    ,[Column31],[Column32] from Table_011_Child1_SaleFactor where column01=" +
                        Table.Rows[i]["ColumnId"].ToString(), ConSale);
                        InsertDetail.ExecuteNonQuery();

                        SqlCommand InsertSecondDetail =
                        new SqlCommand(@"insert into Table_065_Child2_LegalFactors 
                    ([column01],[column02],[column03],[column04],[column05],[column06])
                    select       " + key.Value.ToString() +
                        @",[column02],[column03],[column04],[column05],[column06] 
                    from Table_012_Child2_SaleFactor where column01=" +
                        Table.Rows[i]["ColumnId"].ToString(), ConSale);
                        InsertSecondDetail.ExecuteNonQuery();

                        clDoc.RunSqlCommand(ConSale.ConnectionString,
                        "UPDATE Table_010_SaleFactor set Column39=" +
                        key.Value.ToString() + " where ColumnId=" + Table.Rows[i]["ColumnId"].ToString());
                    }
                }
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
                    faDatePickerStrip2.FADatePicker.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePickerStrip2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip2.FADatePicker.HideDropDown();
                    bt_Search_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                DS.EnforceConstraints = false;

                DS.Tables["Header"].Clear();
                HeaderAdapter.SelectCommand.Parameters["Date1"].Value = faDatePickerStrip1.FADatePicker.Text;
                HeaderAdapter.SelectCommand.Parameters["Date2"].Value = faDatePickerStrip2.FADatePicker.Text;
                HeaderAdapter.Fill(DS, "Header");

                DS.Tables["Detail"].Clear();
                DetailAdapter.SelectCommand.Parameters["Date1"].Value = faDatePickerStrip1.FADatePicker.Text;
                DetailAdapter.SelectCommand.Parameters["Date2"].Value = faDatePickerStrip2.FADatePicker.Text;
                DetailAdapter.Fill(DS, "Detail");

                DS.EnforceConstraints = true;
            }
        }

        private void Frm_018_LegalFactors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                faDatePickerStrip1.FADatePicker.Select();
                faDatePickerStrip1.Select();
            }
            else if (e.Alt && e.KeyCode == Keys.A)
                bt_DeselectAll_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.A)
                bt_SelectAll_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Search_Click(sender, e);
         
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (this.gridEX_Header.RowCount> 0)
            {
                this.Cursor = Cursors.WaitCursor;
                int Id=0;
                if (gridEX_Header.GetRow().Cells["Column37"].Text.Trim() == "")
                    Id = 0;
                else Id = int.Parse(gridEX_Header.GetValue("Column37").ToString());
                _05_Sale.LegalFactors.Reports.Form_LegalFactorPrint frm = new  Reports.Form_LegalFactorPrint(Id);
                this.Cursor = Cursors.Default;
                frm.ShowDialog();
            }
        }

        private void bt_SelectAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            for (int i = 0; i < DS.Tables["Header"].Rows.Count; i++)
            {
                DS.Tables["Header"].Rows[i]["Column38"] = true;
            }
            gridEX_Header.Refresh();
            this.Cursor = Cursors.Default;
        }

        private void bt_DeselectAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            for (int i = 0; i < DS.Tables["Header"].Rows.Count; i++)
            {
                DS.Tables["Header"].Rows[i]["Column38"] = false;
            }
            gridEX_Header.Refresh();
            this.Cursor = Cursors.Default;

        }

    }
}
 

//toolStripProgressBar1.Visible = true;
//            toolStripProgressBar1.Minimum = 0;
//            toolStripProgressBar1.Maximum = gridEX_Header.RowCount;
//            toolStripProgressBar1.Step = 1;
//            toolStripProgressBar1.Value = 0;
            
//            int _MaxPosRowOfDetailFactor = 0, _QtyOfLegalFactor = 0;

//            SqlCmd = new SqlCommand(@"Delete from Table_055_LegalFactors", ConSale);

//            SqlCmd.CommandType = CommandType.Text;
//            SqlCmd.Connection.Open();
//            SqlCmd.ExecuteNonQuery();
//            SqlCmd.Connection.Close();

//            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetRows())
//            {
//                toolStripProgressBar1.PerformStep();

//                //اگر برای فاکتور انتخاب شده قبلا فاکتور قانونی ثبت شده باشد
//                //فاکتور قانونی مربوطه پاک می شود

//                if (item.Cells["Column38"].Value.ToString() == "True")
//                {
                  


//                    //به ازای هر فاکتور انتخاب شده یک فاکتور قانونی درج می شود
//                    //اگر اقلام فاکتور انتخابی بیشتر از 12 بود یک فاکتور قانونی دیگر درج میشود
//                    //به عبارتی در هر فاکتور قانونی فقط 12 سطر درج می شود
//                    //جایزه های فاکتور انتخابی لحاظ نمیشود
//                    //در این مرحله شماره فاکتور قانونی صادر شده با آی دی فاکتور اصلی یکی است
//                    //در مرحله ای دیگر جهت مرتب سازی و شماره گذاری مجدد این شماره ها تغییر می کند

//                    _dtDetailOfFactor = new DataTable();

//                    sqlda =
//                        new SqlDataAdapter(@"SELECT * FROM   Table_011_Child1_SaleFactor
//WHERE     (column30 = 0) AND (column01 = " + item.Cells["columnid"].Value.ToString() + ")", ConSale);
//                    sqlda.Fill(_dtDetailOfFactor);

//                    _QtyOfLegalFactor = _dtDetailOfFactor.Rows.Count / 12;
//                    if ((_dtDetailOfFactor.Rows.Count % 12) > 0) _QtyOfLegalFactor++;


//                    for (int i = 0; i < _QtyOfLegalFactor; i++)
//                    {
//                        SqlCmd = new SqlCommand(
//    @"insert into Table_055_LegalFactors ([column01],
//                    [column02], [column03], [column04], [column05], 
//                    [column06], [column11], [column12], [column13],
//                    [column15], [column17],
//                    [column18], [column19], [column20], [column21],
//                    [column22], [column23], [column24], [column25],
//                    [column26], [column27], [Column36], 
//                    [Column40], [Column41], [Column37] 
//                    ) Select " + item.Cells["columnid"].Value.ToString() + @",
//                    [column02], [column03], [column04], [column05], 
//                    [column06], [column11], [column12], '" + Class_BasicOperation._UserName + @"',
//                    '" + Class_BasicOperation._UserName + @"', [column17],
//                    [column18], [column19], [column20], [column21],
//                    [column22], [column23], [column24], [column25],
//                    [column26], [column27], [Column36], 
//                    [Column40], [Column41], " + item.Cells["columnid"].Value.ToString() +
//                                                  @" From Table_010_SaleFactor Where columnid = " +
//                                                  item.Cells["columnid"].Value.ToString() +
//                                    "; SET @Key=SCOPE_IDENTITY()", ConSale);

//                        key = new SqlParameter("Key", SqlDbType.Int);
//                        key.Direction = ParameterDirection.Output;

//                        SqlCmd.CommandType = CommandType.Text;
//                        SqlCmd.Connection.Open();
//                        SqlCmd.Parameters.Add(key);
//                        SqlCmd.ExecuteNonQuery();
//                        SqlCmd.Connection.Close();



//                        if ((_dtDetailOfFactor.Rows.Count - _MaxPosRowOfDetailFactor) <= 12)
//                            _MaxPosRowOfDetailFactor = _dtDetailOfFactor.Rows.Count;
//                        else _MaxPosRowOfDetailFactor = (i + 1) * 12;


//                        //درج اقلام فاکتور اصلی در فاکتور قانونی به صورت 12 تا 12 تا

//                        for (int j = i * 12; j < _MaxPosRowOfDetailFactor; j++)
//                        {
//                            SqlCmd = new SqlCommand(@"insert Into Table_060_Child1_LegalFactors 
//                    ([column01],[column02],[column03],[column04],[column05]
//                    ,[column06],[column07],[column08],[column09],[column10]
//                    ,[column11],[column15]
//                    ,[column16],[column17],[column18],[column19],[column20]
//                    ,[column24],[column25]
//                    ,[column26],[column27],[column28],[column29]
//                    ,[Column31],[Column32]) Values (" + key.Value.ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column02"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column03"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column04"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column05"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column06"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column07"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column08"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column09"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column10"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column11"].ToString() +
//                                //"," + _dtDetailOfFactor.Rows[j]["column12"].ToString() +
//                                //"," + _dtDetailOfFactor.Rows[j]["column13"].ToString() +
//                                //"," + _dtDetailOfFactor.Rows[j]["column14"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column15"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column16"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column17"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column18"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column19"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column20"].ToString() +
//                                //"," + _dtDetailOfFactor.Rows[j]["column21"].ToString() +
//                                //"," + _dtDetailOfFactor.Rows[j]["column22"].ToString() +
//                                //"," + _dtDetailOfFactor.Rows[j]["column23"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column24"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column25"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column26"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column27"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column28"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column29"].ToString() +
//                                //"," + _dtDetailOfFactor.Rows[j]["column30"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column31"].ToString() +
//                        "," + _dtDetailOfFactor.Rows[j]["column32"].ToString() + ")", ConSale);

//                            SqlCmd.CommandType = CommandType.Text;
//                            SqlCmd.Connection.Open();
//                            SqlCmd.ExecuteNonQuery();
//                            SqlCmd.Connection.Close();
//                        }

//                    }//for i header of legal factor

//                }//if checked

//            }//for each gridex row checked


//                //در این مرحله فاکتورهای قانونی صادر شده به ترتیب تاریخ و ش ف اصلی مرتب می شوند
//                //سپس از نو شماره گذاری می شوند
//                //و بعد از آن شماره فاکتورهای قانونی در فاکتورهای اصلی آپدیت می شوند

//                _dtLegalFactor = new DataTable();

//                sqlda = new SqlDataAdapter(
//                        "SELECT * FROM Table_055_LegalFactors ORDER BY column02, Column37",
//                        ConSale);
//                sqlda.Fill(_dtLegalFactor);


//                toolStripProgressBar1.Value = 0;
//                toolStripProgressBar1.Maximum = _dtLegalFactor.Rows.Count;

//                for (int i = 1; i <= _dtLegalFactor.Rows.Count; i++)
//                {
//                    toolStripProgressBar1.PerformStep();

//                    _dtLegalFactor.Rows[i - 1]["column01"] = i;

//                    SqlCmd = new SqlCommand(
//    @"Update Table_010_SaleFactor Set Column39 = " + _dtLegalFactor.Rows[i - 1]["columnid"].ToString() +
//    ", Column37 = " + i.ToString() + " Where columnid = " +
//    _dtLegalFactor.Rows[i - 1]["Column37"].ToString(), ConSale);

//                    SqlCmd.CommandType = CommandType.Text;
//                    SqlCmd.Connection.Open();
//                    SqlCmd.ExecuteNonQuery();
//                    SqlCmd.Connection.Close();


//                    SqlCmd = new SqlCommand(
//    @"Update Table_055_LegalFactors Set column01 = " + i.ToString() +
//    " Where columnid = " + _dtLegalFactor.Rows[i - 1]["columnid"].ToString(), ConSale);

//                    SqlCmd.CommandType = CommandType.Text;
//                    SqlCmd.Connection.Open();
//                    SqlCmd.ExecuteNonQuery();
//                    SqlCmd.Connection.Close();
//                }




//                //در این مرحله فاکتورهای قانونی صادر شده مجددا محاسبه می شوند
//                //تا مبالغ حذف شده و تخفیفات 100 درصدی جوایز لحاظ نشود

//                toolStripProgressBar1.Value = 0;
//                toolStripProgressBar1.Maximum = _dtLegalFactor.Rows.Count;

//                for (int i = 1; i <= _dtLegalFactor.Rows.Count; i++)
//                {
//                    toolStripProgressBar1.PerformStep();

//                    _dtSumOfDetail = new DataTable();

//                    sqlda = new SqlDataAdapter(
//                            @"SELECT SUM(column17) AS c17takhfif, SUM(column19) AS c19ezafeh,
//                                SUM(column20) AS c20khales
//                                FROM         dbo.Table_060_Child1_LegalFactors
//                                WHERE     (column01 = " + 
//                                          _dtLegalFactor.Rows[i - 1]["columnid"].ToString() + ")",
//                            ConSale);
//                    sqlda.Fill(_dtSumOfDetail);


//                    SqlCmd = new SqlCommand(
//    @"Update Table_055_LegalFactors Set column28 = " + _dtSumOfDetail.Rows[0]["c20khales"].ToString() +
//    ", column34 = " + _dtSumOfDetail.Rows[0]["c19ezafeh"].ToString() +
//    ",column35 = " + _dtSumOfDetail.Rows[0]["c17takhfif"].ToString() +
//    " Where columnid = " + _dtLegalFactor.Rows[i - 1]["columnid"].ToString(), ConSale);

//                    SqlCmd.CommandType = CommandType.Text;
//                    SqlCmd.Connection.Open();
//                    SqlCmd.ExecuteNonQuery();
//                    SqlCmd.Connection.Close();
//                }

//            toolStripProgressBar1.Visible = false;







//gridEX_Header.MoveToNewRecord();
//if (DialogResult.Yes == MessageBox.Show(
//    "آیا مایل به شماره گذاری فاکتورهای انتخاب شده هستید؟", "", MessageBoxButtons.YesNo,
//     MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
//{
//    this.table_010_SaleFactorBindingSource.EndEdit();
//    this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale2.Table_010_SaleFactor);

//    InsertInLegalfactors();

//    dataSet_Sale2.EnforceConstraints = false;
//    this.table_010_SaleFactorTableAdapter.Fill(
//        this.dataSet_Sale2.Table_010_SaleFactor,
//        faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
//    this.table_011_Child1_SaleFactorTableAdapter.Fill(
//        this.dataSet_Sale2.Table_011_Child1_SaleFactor,
//        faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
//    dataSet_Sale2.EnforceConstraints = true;

//    //using (SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE))
//    //{
//    //    ConSale.Open();
//    //    SqlCommand Command = new SqlCommand("EXEC	[dbo].[FactorNumbering]", ConSale);
//    //    Command.ExecuteNonQuery();

//    //    InsertInLegalfactors();

//    //    dataSet_Sale2.EnforceConstraints = false;
//    //    this.table_010_SaleFactorTableAdapter.Fill(this.dataSet_Sale2.Table_010_SaleFactor, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
//    //    this.table_011_Child1_SaleFactorTableAdapter.Fill(this.dataSet_Sale2.Table_011_Child1_SaleFactor, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
//    //    dataSet_Sale2.EnforceConstraints = true;

//    //}

//    Class_BasicOperation.ShowMsg(
//        "", "ذخیره/شماره گذاری اطلاعات با موفقیت انجام شد", "Information");
//}