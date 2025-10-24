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

namespace PSHOP._05_Sale
{
    public partial class Frm_039_ExportDocInformationReturnAmaniFactor : Form
    {
        bool _BackSpace = false, _Tab1 = false, _Tab2 = false, _Tab3 = false;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        DataTable setting = new DataTable();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Class_UserScope UserScope = new Class_UserScope();
        int _SaleID;
        SqlDataAdapter SaleAdapter, Child1Adapter;
        BindingSource SaleBind, Child1Bind;
        DataSet DS = new DataSet();
        DataRowView SaleRow;
        DataTable SourceTable = new DataTable();
        Int16 wareDraft, wareRecipt;
        SqlParameter DraftNum;
        int ResidID; int ResidNum = 0;
        bool newDraft = false;
        public Frm_039_ExportDocInformationReturnAmaniFactor(bool Tab1, bool Tab2, bool Tab3, int SaleID, Int16 WareDraft, Int16 WareRecipt)
        {
            InitializeComponent();
            _Tab1 = Tab1;
            _Tab2 = Tab2;
            _Tab3 = Tab3;
            _SaleID = SaleID;
            wareDraft = WareDraft;
            wareRecipt = WareRecipt;

        }

        private void Frm_009_ExportDocInformation_Load(object sender, EventArgs e)
        {
            try
            {
                DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
                DraftNum.Direction = ParameterDirection.Output;
                DraftNum.Value = 0;
            }
            catch
            {
            }
            SourceTable.Columns.Add("Type", Type.GetType("System.Int16"));
            SourceTable.Columns.Add("Column01", Type.GetType("System.String"));
            SourceTable.Columns.Add("Column001", Type.GetType("System.String"));
            SourceTable.Columns.Add("Column07", Type.GetType("System.Int32"));
            SourceTable.Columns["Column07"].AllowDBNull = true;
            SourceTable.Columns.Add("Column08", Type.GetType("System.Int16"));
            SourceTable.Columns["Column08"].AllowDBNull = true;
            SourceTable.Columns.Add("Column09", Type.GetType("System.Int16"));
            SourceTable.Columns["Column09"].AllowDBNull = true;
            SourceTable.Columns.Add("Column10", Type.GetType("System.String"));
            SourceTable.Columns.Add("Column11", Type.GetType("System.Double"));
            SourceTable.Columns.Add("Column12", Type.GetType("System.Double"));
            //ارزش ارز
            SourceTable.Columns.Add("Column13", Type.GetType("System.Double"));
            //نوع ارز
            SourceTable.Columns.Add("Column14", Type.GetType("System.Int16"));
            SourceTable.Columns.Add("Bed", Type.GetType("System.Int16"));

            gridEX1.DataSource = SourceTable;

            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * from AllHeaders()", ConAcnt);
            Adapter.Fill(DS, "Header");
            mlt_DisBed.DataSource = DS.Tables["Header"];
            mlt_DisBes.DataSource = DS.Tables["Header"];
            mlt_LinAddBed.DataSource = DS.Tables["Header"];
            mlt_LinAddBes.DataSource = DS.Tables["Header"];
            mlt_LinDisBed.DataSource = DS.Tables["Header"];
            mlt_LinDisBes.DataSource = DS.Tables["Header"];
            mlt_SaleBed.DataSource = DS.Tables["Header"];
            mlt_SaleBes.DataSource = DS.Tables["Header"];
            mlt_ValueBed.DataSource = DS.Tables["Header"];
            mlt_ValueBes.DataSource = DS.Tables["Header"];

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_001_PWHRS", ConWare);
            Adapter.Fill(DS, "Ware");
            mlt_WareDraft.DataSource = DS.Tables["Ware"];
            mlt_WareRecipt.DataSource = DS.Tables["Ware"];
            Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=1", ConWare);
            Adapter.Fill(DS, "Fun");
            mlt_FunctionDraft.DataSource = DS.Tables["Fun"];
            Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=0", ConWare);
            Adapter.Fill(DS, "Fun2");
            mlt_FunctionRecipt.DataSource = DS.Tables["Fun2"];
            //mlt_Function.Value = func;
            //*********************
            SaleAdapter = new SqlDataAdapter("Select * from Table_080_ReturnAmani where ColumnId=" + _SaleID, ConSale);
            SaleAdapter.Fill(DS, "Sale");
            SaleBind = new BindingSource();
            SaleBind.DataSource = DS.Tables["Sale"];
            SaleRow = (DataRowView)this.SaleBind.CurrencyManager.Current;
            mlt_WareDraft.Value = SaleRow["Column58"].ToString();
            mlt_WareRecipt.Value = SaleRow["Column59"].ToString();
            Child1Adapter = new SqlDataAdapter("Select *,CAST(0 as decimal(18, 4)) as UnitValue,CAST(0 as decimal(18, 4)) as TotalValue from Table_085_ReturnAmaniChild where Column01=" + _SaleID, ConSale);
            Child1Adapter.Fill(DS, "Child1");
            Child1Bind = new BindingSource();
            Child1Bind.DataSource = DS.Tables["Child1"];

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
            DataTable Person = new DataTable();
            Adapter.Fill(Person);
            gridEX1.DropDowns["Person"].SetDataBinding(Person, "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo", ConBase);
            DataTable Center = new DataTable();
            Adapter.Fill(Center);
            gridEX1.DropDowns["Center"].SetDataBinding(Center, "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
            DataTable Project = new DataTable();
            Adapter.Fill(Project);
            gridEX1.DropDowns["Project"].SetDataBinding(Project, "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_055_CurrencyInfo", ConBase);
            DataTable Currency = new DataTable();
            Adapter.Fill(Currency);
            mlt_CurrencyType.DataSource = Currency;

            Adapter = new SqlDataAdapter("Select * from AllHeaders()", ConAcnt);
            DataTable Headers = new DataTable();
            Adapter.Fill(Headers);
            gridEX1.DropDowns["Header_Code"].SetDataBinding(Headers, "");
            gridEX1.DropDowns["Header_Name"].SetDataBinding(Headers, "");




            mlt_SaleBed.Value = clDoc.Account(47, "Column13");
            mlt_SaleBes.Value = clDoc.Account(47, "Column07");


            mlt_DisBed.Value = clDoc.Account(5, "Column07");
            mlt_DisBes.Value = clDoc.Account(5, "Column13");
            mlt_LinAddBed.Value = clDoc.Account(7, "Column07");
            mlt_LinAddBes.Value = clDoc.Account(7, "Column13");
            mlt_LinDisBed.Value = clDoc.Account(6, "Column07");
            mlt_LinDisBes.Value = clDoc.Account(6, "Column13");

            ////else
            //{
            //    mlt_SaleBed.Value = clDoc.Account(18, "Column07");
            //    mlt_SaleBes.Value = clDoc.Account(18, "Column13");
            //    mlt_LinAddBed.Value = clDoc.Account(20, "Column07");
            //    mlt_LinAddBes.Value = clDoc.Account(20, "Column13");
            //    mlt_LinDisBed.Value = clDoc.Account(19, "Column07");
            //    mlt_LinDisBes.Value = clDoc.Account(19, "Column13");
            //}

            mlt_ValueBed.Value = clDoc.Account(8, "Column07");
            mlt_ValueBes.Value = clDoc.Account(8, "Column13");
            faDatePicker1.Text = SaleRow["Column02"].ToString();

            uiTab1.Enabled = _Tab1;
            uiTab2.Enabled = _Tab2;
            uiTab3.Enabled = _Tab3;
            mlt_SaleBed.Select();

            GridEXColumn dateColumn = gridEX1.RootTable.Columns["Column11"];
            GridEXFilterCondition composite = new GridEXFilterCondition();
            GridEXFilterCondition filter1 = new GridEXFilterCondition(dateColumn,
            ConditionOperator.Equal, 0);
            GridEXFilterCondition filter2 = new GridEXFilterCondition(gridEX1.RootTable.Columns["Column12"],
            ConditionOperator.Equal, 0);
            composite.AddCondition(filter1);
            composite.AddCondition(LogicalOperator.And, filter2);
            GridEXFormatCondition fc = new GridEXFormatCondition();
            fc.FilterCondition = composite;
            fc.FormatStyle.ForeColor = Color.Blue;
            gridEX1.RootTable.FormatConditions.Add(fc);
            Adapter = new SqlDataAdapter("Select * from Table_030_Setting", ConMain);
            Adapter.Fill(setting);
            chk_Baha.Checked = Properties.Settings.Default.ExtraMethod;
            chk_RegisterGoods.Checked = Properties.Settings.Default.RegisterSaleFactorWithGoods;
            chk_Nots.Checked = Properties.Settings.Default.RegisterSaleFactorNoteGoods;
            this.chk_Net.Checked = Properties.Settings.Default.chk_Net;
            chk_AggDoc.Checked = Properties.Settings.Default.AggregationSaleDoc;

            if (!chk_Nots.Checked && !chk_RegisterGoods.Checked)
                chk_without.Checked = true;
            PrepareDoc();



        }

        private void multiColumnCombo1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void rdb_New_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_New.Checked)
            {
                faDatePicker1.Enabled = true;
                txt_Cover.Text = "فاکتور فروش امانی";
                faDatePicker1.Text = SaleRow["Column02"].ToString();
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
                faDatePicker1.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00");
                txt_Cover.Text = null;
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

        private void rdb_TO_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_TO.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;

            }
            else
            {
                txt_To.Text = null;
                faDatePicker1.Enabled = true;
                txt_Cover.Enabled = true;
            }
        }

        private void txt_To_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void bt_ViewDocs_Click(object sender, EventArgs e)
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
                        ((PACNT._2_DocumentMenu.Form04_ViewDocument)item).table_060_SanadHeadBindingSource.Position =
                            ((PACNT._2_DocumentMenu.Form04_ViewDocument)item).table_060_SanadHeadBindingSource.Find("ColumnId", 0);
                        return;
                    }
                }
                PACNT._2_DocumentMenu.Form04_ViewDocument frm = new PACNT._2_DocumentMenu.Form04_ViewDocument(0);
                try
                {
                    frm.MdiParent = MainForm.ActiveForm;
                }
                catch { }
                frm.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان مشاهده این فرم را ندارید", "None");
        }

        private void bt_ViewDrafts_Click(object sender, EventArgs e)
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

        private void PrepareDoc()
        {

            # region به تفکیک پروژه
            if (chk_Baha.Checked)
            {

                SqlDataAdapter Adapter;
                SourceTable.Rows.Clear();


                Adapter = new SqlDataAdapter(@" SELECT * FROM  [Table_085_ReturnAmaniChild]
                                                    WHERE column01=  " + SaleRow["ColumnId"].ToString() + " AND [column22] is NOT NULL", ConSale);
                DataTable Project = new DataTable();
                Adapter.Fill(Project);

                # region کالاها پروژه دارند
                if (Project.Rows.Count > 0)//////////کالاها پروژه دارند
                {
                    Adapter = new SqlDataAdapter(@"SELECT      Project,Total, Discount, Adding, column01, Total - Discount + Adding AS Net,TotalValue
                             FROM         (SELECT       ISNULL(SUM(column11), 0) AS Total, 
                             ISNULL(SUM(column17), 0) AS Discount, ISNULL(SUM(column19), 0) AS Adding, column01,
                               ISNULL(Sum(column07),0) as TotalValue,column22 as Project
                             FROM          dbo.Table_085_ReturnAmaniChild
                             GROUP BY   column01 ,column22
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                    Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, SaleRow["ColumnId"].ToString());
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);





                    // فاکتور فروش بدون احتساب تخفیفات و اضافات خطی


                    DataTable detali = new DataTable();

                    Adapter = new SqlDataAdapter(@"SELECT  tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02,tcsf.column22 as Project
                                            FROM   Table_085_ReturnAmaniChild tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + SaleRow["ColumnId"].ToString() + "  ", ConSale);
                    Adapter.Fill(detali);
                    //********Bed
                    if (chk_RegisterGoods.Checked)//ریز اقلام
                    {
                        foreach (DataRow dt in detali.Rows)
                        {
                            string sharhjoz = string.Empty;
                            sharhjoz = " کالای  " + dt["column02"].ToString();
                            if (Convert.ToBoolean(setting.Rows[32]["Column02"]))
                                sharhjoz += " قیمت  " + string.Format("{0:#,##0.###}", dt["column10"]);
                            if (Convert.ToBoolean(setting.Rows[34]["Column02"]))
                                sharhjoz += " مقدار  " + string.Format("{0:#,##0.###}", dt["column07"]);
                            if (!chk_Net.Checked)
                            {
                                SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                            (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                            null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null), CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                            Convert.ToDouble(dt["column11"].ToString()),
                            0, 0, -1);

                                SourceTable.Rows.Add(94, (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null),
                            (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null), null,
                            null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null), CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                             0,
                             Convert.ToDouble(dt["column11"].ToString()), 0, -1);
                            }
                            else
                            {
                                SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                        (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                        null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null), CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                        Convert.ToDouble(dt["Net"].ToString()),
                        0, 0, -1);

                                SourceTable.Rows.Add(94, (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null),
                            (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null), null,
                            null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null),
                            CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                             0,
                             Convert.ToDouble(dt["Net"].ToString()), 0, -1);
                            }

                        }
                    }

                    else
                    {
                        foreach (DataRow item in Table.Rows)
                        {

                            if (!chk_Net.Checked)
                            {
                                //*********Bes
                                SourceTable.Rows.Add(94, (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null), (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null),
                                    null, null,
                                    ((item["Project"] != DBNull.Value && item["Project"] != null && !string.IsNullOrWhiteSpace(item["Project"].ToString())) ? item["Project"] : null),
                                    CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])), 0,
                                    Convert.ToDouble(item["Total"].ToString()),
                                     Convert.ToDouble(0),
                                         -1);

                                //*********Bed

                                SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                   (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                   null,
                                   ((item["Project"] != DBNull.Value && item["Project"] != null && !string.IsNullOrWhiteSpace(item["Project"].ToString())) ? item["Project"] : null),
                                     CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                   Convert.ToDouble(item["Total"].ToString()),
                                   0, 0, -1);
                            }
                            else
                            {
                                //*********Bes
                                SourceTable.Rows.Add(94, (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null), (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null),
                                    null, null,
                                    ((item["Project"] != DBNull.Value && item["Project"] != null && !string.IsNullOrWhiteSpace(item["Project"].ToString())) ? item["Project"] : null),
                                    CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])), 0,
                                    Convert.ToDouble(item["Net"].ToString()),
                                     Convert.ToDouble(0),
                                         -1);

                                //*********Bed

                                SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                   (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                   null,
                                   ((item["Project"] != DBNull.Value && item["Project"] != null && !string.IsNullOrWhiteSpace(item["Project"].ToString())) ? item["Project"] : null),
                                     CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                   Convert.ToDouble(item["Net"].ToString()),
                                   0, 0, -1);
                            }
                        }
                    }

                    // مربوط به اضافات خطی فاکتور
                    if (Convert.ToDouble(SaleRow["Column34"].ToString()) != 0 && !chk_Net.Checked)
                    {


                        DataTable ezafeTable = clDoc.ReturnTable(ConSale.ConnectionString,
                   "Select SUM(column19) as ezafe, column22 from Table_085_ReturnAmaniChild where column01=" + SaleRow["ColumnId"].ToString() + "  group by column22");
                        foreach (DataRow d in ezafeTable.Rows)
                        {
                            if (Convert.ToDouble(d["ezafe"].ToString()) != 0)
                            {
                                //********Bed
                                SourceTable.Rows.Add(94, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                    (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                    null, ((d["column22"] != DBNull.Value && d["column22"] != null && !string.IsNullOrWhiteSpace(d["column22"].ToString())) ? d["column22"].ToString() : null), (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور مرجوعی فروش امانی ش " : "تخفیف خطی2 فاکتور مرجوعی فروش امانی ش ") +
                                    SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                    Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                                //*********Bes
                                SourceTable.Rows.Add(94, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                    (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null), null, null, ((d["column22"] != DBNull.Value && d["column22"] != null && !string.IsNullOrWhiteSpace(d["column22"].ToString())) ? d["column22"].ToString() : null),
                                    (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور مرجوعی فروش امانی ش " : "تخفیف خطی2 فاکتور مرجوعی فروش امانی ش ")
                                    + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                    0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                            }
                        }



                    }

                    //ثبت مربوط به تخفیفات خطی فاکتور
                    if (Convert.ToDouble(SaleRow["Column35"].ToString()) > 0 && !chk_Net.Checked)
                    {
                        DataTable takhfifTable = clDoc.ReturnTable(ConSale.ConnectionString,
                            "Select SUM(column17) as takhfif, column22 from Table_085_ReturnAmaniChild where column01=" + SaleRow["ColumnId"].ToString() + "  group by column22");


                        foreach (DataRow h in takhfifTable.Rows)
                        {

                            if (Convert.ToInt64(h["takhfif"]) > 0)
                            {
                                //********Bed
                                SourceTable.Rows.Add(94, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), null, null, ((h["column22"] != DBNull.Value && h["column22"] != null && !string.IsNullOrWhiteSpace(h["column22"].ToString())) ? h["column22"].ToString() : null), "تخفیف خطی فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);
                                //*********Bes
                                SourceTable.Rows.Add(94, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, ((h["column22"] != DBNull.Value && h["column22"] != null && !string.IsNullOrWhiteSpace(h["column22"].ToString())) ? h["column22"].ToString() : null), "تخفیف خطی فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                            }
                        }


                    }
                    //ثبت مربوط به تخفیفات انتهای فاکتور

                    ////تخفیف حجمی گروه مشتری
                    //if (Convert.ToDouble(SaleRow["Column29"].ToString()) > 0)
                    //{
                    //    //********Bed
                    //    SourceTable.Rows.Add(94, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف حجمی گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column29"].ToString()), 0, 0, -1);
                    //    //*********Bes
                    //    SourceTable.Rows.Add(94, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف حجمی گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column29"].ToString()), 0, -1);
                    //}
                    ////تخفیف ویژه گروه مشتری
                    //if (Convert.ToDouble(SaleRow["Column30"].ToString()) > 0)
                    //{
                    //    //********Bed
                    //    SourceTable.Rows.Add(94, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف ویژه گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column30"].ToString()), 0, 0, -1);
                    //    //*********Bes
                    //    SourceTable.Rows.Add(94, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف ویژه گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column30"].ToString()), 0, -1);
                    //}
                    ////تخفیف ویژه مشتری
                    //if (Convert.ToDouble(SaleRow["Column31"].ToString()) > 0)
                    //{
                    //    //********Bed
                    //    SourceTable.Rows.Add(94, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف ویژه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column31"].ToString()), 0, 0, -1);
                    //    //*********Bes
                    //    SourceTable.Rows.Add(94, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف ویژه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column31"].ToString()), 0, -1);
                    //}



                    ////صدور سند ارزش حواله 
                    //if (uiTab3.Enabled)
                    //{
                    //    int HavaleID = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_080_ReturnAmani", "Column09", "ColumnId", SaleRow["ColumnId"].ToString()));
                    //    double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_008_Child_PwhrsDraft", "ISNULL(SUM(Column16),0)", "Column01", HavaleID.ToString()));


                    //    if (HavaleID > 0)
                    //    {
                    //        newDraft = false;

                    //        DataTable baha = new DataTable();
                    //        Adapter = new SqlDataAdapter("SELECT column14,ISNULL(SUM(Column16),0) as Column16  from Table_008_Child_PwhrsDraft Where Column01=" + HavaleID + " Group by column14 ", ConWare);
                    //        Adapter.Fill(baha);

                    //        foreach (DataRow row in baha.Rows)
                    //        {
                    //            if (Convert.ToDouble(row["Column16"]) > 0)
                    //            {
                    //                if (row["column14"] != DBNull.Value && row["column14"] != null && !string.IsNullOrWhiteSpace(row["column14"].ToString()))
                    //                {
                    //                    //********Bed
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);
                    //                    //*********Bes
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                    //                }
                    //                else
                    //                {
                    //                    //********Bed
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);
                    //                    //*********Bes
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (row["column14"] != DBNull.Value && row["column14"] != null && !string.IsNullOrWhiteSpace(row["column14"].ToString()))
                    //                {
                    //                    //********Bed
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);
                    //                    //*********Bes
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                    //                }
                    //                else
                    //                {
                    //                    //********Bed
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);
                    //                    //*********Bes
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                    //                }
                    //            }

                    //        }
                    //    }
                    //    else
                    //    {
                    //        newDraft = true;
                    //        DataTable baha = new DataTable();
                    //        Adapter = new SqlDataAdapter("SELECT column22 as column14,ISNULL(SUM(column20),0) as Column16  from Table_085_ReturnAmaniChild Where Column01=" + SaleRow["columnid"].ToString() + " Group by column22 ", ConSale);
                    //        Adapter.Fill(baha);
                    //        foreach (DataRow row in baha.Rows)
                    //        {

                    //            if (row["column14"] != DBNull.Value && row["column14"] != null && !string.IsNullOrWhiteSpace(row["column14"].ToString()))
                    //            {
                    //                //********Bed
                    //                SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 1);
                    //                //*********Bes
                    //                SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 0);
                    //            }
                    //            else
                    //            {
                    //                //********Bed
                    //                SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 1);
                    //                //*********Bes
                    //                SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 0);
                    //            }
                    //        }
                    //    }




                    //}
                }
                #endregion

                #region کالاها پروژه ندارند
                else//کالاها پروژه ندارد
                {
                    Adapter = new SqlDataAdapter(@" SELECT Column44 FROM  Table_080_ReturnAmani
                                                    WHERE columnid=  " + SaleRow["ColumnId"].ToString(), ConSale);
                    Project = new DataTable();
                    Adapter.Fill(Project);



                    Adapter = new SqlDataAdapter(@"SELECT       Total, Discount, Adding, column01, Total - Discount + Adding AS Net,TotalValue
                             FROM         (SELECT       ISNULL(SUM(column11), 0) AS Total, 
                             ISNULL(SUM(column17), 0) AS Discount, ISNULL(SUM(column19), 0) AS Adding, column01,
                               ISNULL(Sum(column07),0) as TotalValue 
                             FROM          dbo.Table_085_ReturnAmaniChild
                             GROUP BY   column01  
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                    Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, SaleRow["ColumnId"].ToString());
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);





                    // فاکتور فروش بدون احتساب تخفیفات و اضافات خطی


                    DataTable detali = new DataTable();

                    Adapter = new SqlDataAdapter(@"SELECT  tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02,tcsf.column22 as Project
                                            FROM   Table_085_ReturnAmaniChild tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + SaleRow["ColumnId"].ToString() + "  ", ConSale);
                    Adapter.Fill(detali);
                    //********Bed
                    if (chk_RegisterGoods.Checked)//ریز اقلام
                    {
                        foreach (DataRow dt in detali.Rows)
                        {
                            string sharhjoz = string.Empty;
                            sharhjoz = " کالای  " + dt["column02"].ToString();
                            if (Convert.ToBoolean(setting.Rows[32]["Column02"]))
                                sharhjoz += " قیمت  " + string.Format("{0:#,##0.###}", dt["column10"]);
                            if (Convert.ToBoolean(setting.Rows[34]["Column02"]))
                                sharhjoz += " مقدار  " + string.Format("{0:#,##0.###}", dt["column07"]);
                            if (!chk_Net.Checked)
                            {
                                SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                            (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                            null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null),
                            CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                            Convert.ToDouble(dt["column11"].ToString()),
                            0, 0, -1);

                                SourceTable.Rows.Add(94, (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null),
                            (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null), null,
                            null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                            , CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                             0,
                             Convert.ToDouble(dt["column11"].ToString()), 0, -1);
                            }
                            else
                            {
                                SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                           (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                           null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null),
                           CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                           Convert.ToDouble(dt["Net"].ToString()),
                           0, 0, -1);

                                SourceTable.Rows.Add(94, (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null),
                            (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null), null,
                            null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                            , CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                             0,
                             Convert.ToDouble(dt["Net"].ToString()), 0, -1);
                            }

                        }
                    }

                    else
                    {
                        foreach (DataRow item in Table.Rows)
                        {
                            if (!chk_Net.Checked)
                            {
                                //*********Bes
                                SourceTable.Rows.Add(94, (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null), (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null),
                                    null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                    , CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                    0,
                                    Convert.ToDouble(item["Total"].ToString()),
                                     0,
                                         -1);

                                //*********Bed

                                SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                   (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                   null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                   , CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                     Convert.ToDouble(item["Total"].ToString()),
                                   0, 0, -1);
                            }
                            else
                            {
                                //*********Bes
                                SourceTable.Rows.Add(94, (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null), (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null),
                                    null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                    , CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                    0,
                                    Convert.ToDouble(item["Net"].ToString()),
                                     0,
                                         -1);

                                //*********Bed

                                SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                   (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                   null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                   , CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                     Convert.ToDouble(item["Net"].ToString()),
                                   0, 0, -1);
                            }
                        }
                    }

                    // مربوط به اضافات خطی فاکتور
                    if (Convert.ToDouble(SaleRow["Column34"].ToString()) != 0 && !chk_Net.Checked)
                    {


                        DataTable ezafeTable = clDoc.ReturnTable(ConSale.ConnectionString,
                   "Select SUM(column19) as ezafe  from Table_085_ReturnAmaniChild where column01=" + SaleRow["ColumnId"].ToString() + "   ");
                        foreach (DataRow d in ezafeTable.Rows)
                        {
                            if (Convert.ToDouble(d["ezafe"].ToString()) != 0)
                            {
                                //********Bed
                                SourceTable.Rows.Add(94, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                    (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                    null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                    , (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور مرجوعی فروش امانی ش " : "تخفیف خطی2 فاکتور مرجوعی فروش امانی ش ") +
                                    SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                    Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                                //*********Bes
                                SourceTable.Rows.Add(94, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                    (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null), null, null,
                                    ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null),
                                    (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور مرجوعی فروش امانی ش " : "تخفیف خطی2 فاکتور مرجوعی فروش امانی ش ")
                                    + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                    0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                            }
                        }



                    }

                    //ثبت مربوط به تخفیفات خطی فاکتور
                    if (Convert.ToDouble(SaleRow["Column35"].ToString()) > 0 && !chk_Net.Checked)
                    {
                        DataTable takhfifTable = clDoc.ReturnTable(ConSale.ConnectionString,
                            "Select SUM(column17) as takhfif from Table_085_ReturnAmaniChild where column01=" + SaleRow["ColumnId"].ToString() + " ");


                        foreach (DataRow h in takhfifTable.Rows)
                        {

                            if (Convert.ToInt64(h["takhfif"]) > 0)
                            {
                                //********Bed
                                SourceTable.Rows.Add(94, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف خطی فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);
                                //*********Bes
                                SourceTable.Rows.Add(94, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null,
                                   ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف خطی فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                            }
                        }


                    }
                    //ثبت مربوط به تخفیفات انتهای فاکتور

                    ////تخفیف حجمی گروه مشتری
                    //if (Convert.ToDouble(SaleRow["Column29"].ToString()) > 0)
                    //{
                    //    //********Bed
                    //    SourceTable.Rows.Add(94, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف حجمی گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column29"].ToString()), 0, 0, -1);
                    //    //*********Bes
                    //    SourceTable.Rows.Add(94, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف حجمی گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column29"].ToString()), 0, -1);
                    //}
                    ////تخفیف ویژه گروه مشتری
                    //if (Convert.ToDouble(SaleRow["Column30"].ToString()) > 0)
                    //{
                    //    //********Bed
                    //    SourceTable.Rows.Add(94, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف ویژه گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column30"].ToString()), 0, 0, -1);
                    //    //*********Bes
                    //    SourceTable.Rows.Add(94, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف ویژه گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column30"].ToString()), 0, -1);
                    //}
                    ////تخفیف ویژه مشتری
                    //if (Convert.ToDouble(SaleRow["Column31"].ToString()) > 0)
                    //{
                    //    //********Bed
                    //    SourceTable.Rows.Add(94, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف ویژه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column31"].ToString()), 0, 0, -1);
                    //    //*********Bes
                    //    SourceTable.Rows.Add(94, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف ویژه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column31"].ToString()), 0, -1);
                    //}



                    //صدور سند ارزش حواله 
                    //if (uiTab3.Enabled)
                    //{
                    //    int HavaleID = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_080_ReturnAmani", "Column56", "ColumnId", SaleRow["ColumnId"].ToString()));
                    //    double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_008_Child_PwhrsDraft", "ISNULL(SUM(Column16),0)", "Column01", HavaleID.ToString()));


                    //    if (HavaleID > 0)
                    //    {
                    //        newDraft = false;

                    //        DataTable baha = new DataTable();
                    //        Adapter = new SqlDataAdapter("SELECT ISNULL(SUM(Column16),0) as Column16  from Table_008_Child_PwhrsDraft Where Column01=" + HavaleID + "  ", ConWare);
                    //        Adapter.Fill(baha);

                    //        foreach (DataRow row in baha.Rows)
                    //        {
                    //            if (Convert.ToDouble(row["Column16"]) > 0)
                    //            {

                    //                {
                    //                    //********Bed
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);
                    //                    //*********Bes
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                    //                }
                    //            }
                    //            else
                    //            {

                    //                {
                    //                    //********Bed
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);
                    //                    //*********Bes
                    //                    SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                    //                }
                    //            }

                    //        }
                    //    }
                    //    else
                    //    {
                    //        newDraft = true;
                    //        DataTable baha = new DataTable();
                    //        Adapter = new SqlDataAdapter("SELECT ISNULL(SUM(column20),0) as Column16  from Table_085_ReturnAmaniChild Where Column01=" + SaleRow["columnid"].ToString() + "  ", ConSale);
                    //        Adapter.Fill(baha);
                    //        foreach (DataRow row in baha.Rows)
                    //        {
                    //            //********Bed
                    //            SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 1);
                    //            //*********Bes
                    //            SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 0);

                    //        }
                    //    }

                    //}

                }
                #endregion

            }
            # endregion

            #region عدم تفکیک پروژه
            else
            {


                SourceTable.Rows.Clear();
                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT      Total, Discount, Adding, column01, Total - Discount + Adding AS Net,TotalValue
                             FROM         (SELECT       ISNULL(SUM(column11), 0) AS Total, 
                             ISNULL(SUM(column17), 0) AS Discount, ISNULL(SUM(column19), 0) AS Adding, column01,
                               ISNULL(Sum(column07),0) as TotalValue
                             FROM          dbo.Table_085_ReturnAmaniChild
                             GROUP BY   column01 
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, SaleRow["ColumnId"].ToString());
                DataTable Table = new DataTable();
                Adapter.Fill(Table);





                // فاکتور فروش بدون احتساب تخفیفات و اضافات خطی


                DataTable detali = new DataTable();

                Adapter = new SqlDataAdapter(@"SELECT tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02
                                            FROM   Table_085_ReturnAmaniChild tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + SaleRow["ColumnId"].ToString() + "", ConSale);
                Adapter.Fill(detali);
                //********Bed
                if (chk_RegisterGoods.Checked)//ریز اقلام
                {
                    foreach (DataRow dt in detali.Rows)
                    {
                        string sharhjoz = string.Empty;
                        sharhjoz = " کالای  " + dt["column02"].ToString();
                        if (Convert.ToBoolean(setting.Rows[32]["Column02"]))
                            sharhjoz += " قیمت  " + string.Format("{0:#,##0.###}", dt["column10"]);
                        if (Convert.ToBoolean(setting.Rows[34]["Column02"]))
                            sharhjoz += " مقدار  " + string.Format("{0:#,##0.###}", dt["column07"]);
                        if (!chk_Net.Checked)

                            SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                        (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                        null, null,
                        CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                        Convert.ToDouble(dt["column11"].ToString()),
                        0, 0, -1);
                        else
                            SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                        (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                        null, null,
                        CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                        Convert.ToDouble(dt["Net"].ToString()),
                        0, 0, -1);

                    }
                }
                else
                {
                    if (!chk_Net.Checked)

                        SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                            (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                            null,
                            null,
                            CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())),
                            Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()),
                            0, 0, -1);
                    else
                        SourceTable.Rows.Add(94, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                      (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                      null,
                      null,
                      CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())),
                      Convert.ToDouble(Table.Rows[0]["Net"]),
                      0, 0, -1);

                }
                //*********Bes
                foreach (DataRow item in Table.Rows)
                {
                    if (!chk_Net.Checked)
                    {
                        SourceTable.Rows.Add(94, (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null), (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null),
                            null, null, null,
                            CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                            0,
                            Convert.ToDouble(item["Total"].ToString()),
                             Convert.ToDouble(0),
                                 -1);
                    }
                    else
                    {
                        SourceTable.Rows.Add(94, (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null), (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null),
                           null, null, null,
                           CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                           0,
                           Convert.ToDouble(item["Net"].ToString()),
                            Convert.ToDouble(0),
                                -1);
                    }
                }

                // مربوط به اضافات خطی فاکتور
                if (Convert.ToDouble(SaleRow["Column34"].ToString()) != 0 && !chk_Net.Checked)
                {



                    //********Bed
                    SourceTable.Rows.Add(94, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                        (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                        null, null, (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور مرجوعی فروش امانی ش " : "تخفیف خطی2 فاکتور مرجوعی فروش امانی ش ") +
                        SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                        Math.Abs(Convert.ToDouble(SaleRow["Column34"].ToString())), 0, 0, -1);
                    //*********Bes
                    SourceTable.Rows.Add(94, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                        (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null), null, null, null,
                        (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور مرجوعی فروش امانی ش " : "تخفیف خطی2 فاکتور مرجوعی فروش امانی ش ")
                        + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                        0, Math.Abs(Convert.ToDouble(SaleRow["Column34"].ToString())), 0, -1);



                }

                //ثبت مربوط به تخفیفات خطی فاکتور
                if (Convert.ToDouble(SaleRow["Column35"].ToString()) > 0 && !chk_Net.Checked)
                {


                    //********Bed
                    SourceTable.Rows.Add(94, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), null, null, null, "تخفیف خطی فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column35"].ToString()), 0, 0, -1);
                    //*********Bes
                    SourceTable.Rows.Add(94, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف خطی فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column35"].ToString()), 0, -1);



                }
                //ثبت مربوط به تخفیفات انتهای فاکتور

                ////تخفیف حجمی گروه مشتری
                //if (Convert.ToDouble(SaleRow["Column29"].ToString()) > 0)
                //{
                //    //********Bed
                //    SourceTable.Rows.Add(94, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف حجمی گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column29"].ToString()), 0, 0, -1);
                //    //*********Bes
                //    SourceTable.Rows.Add(94, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف حجمی گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column29"].ToString()), 0, -1);
                //}
                ////تخفیف ویژه گروه مشتری
                //if (Convert.ToDouble(SaleRow["Column30"].ToString()) > 0)
                //{
                //    //********Bed
                //    SourceTable.Rows.Add(94, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف ویژه گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column30"].ToString()), 0, 0, -1);
                //    //*********Bes
                //    SourceTable.Rows.Add(94, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف ویژه گروه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column30"].ToString()), 0, -1);
                //}
                ////تخفیف ویژه مشتری
                //if (Convert.ToDouble(SaleRow["Column31"].ToString()) > 0)
                //{
                //    //********Bed
                //    SourceTable.Rows.Add(94, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف ویژه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column31"].ToString()), 0, 0, -1);
                //    //*********Bes
                //    SourceTable.Rows.Add(94, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف ویژه مشتری- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column31"].ToString()), 0, -1);
                //}



                ////صدور سند ارزش حواله 
                //if (uiTab3.Enabled)
                //{
                //    int HavaleID = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_080_ReturnAmani", "Column56", "ColumnId", SaleRow["ColumnId"].ToString()));
                //    double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_008_Child_PwhrsDraft", "ISNULL(SUM(Column16),0)", "Column01", HavaleID.ToString()));

                //    if (TotalValue > 0)
                //    {
                //        //********Bed
                //        SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), TotalValue, 0, 0, -1, 1);
                //        //*********Bes
                //        SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, TotalValue, 0, -1, 0);
                //    }
                //    else
                //    {
                //        //********Bed
                //        SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), TotalValue, 0, 0, -1, 1);
                //        //*********Bes
                //        SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, TotalValue, 0, -1, 0);
                //    }



                //}
                //if (chk_AggDoc.Checked)
                //    AggDoc();
            }
            # endregion

            gridEX1.DataSource = SourceTable;

            if (chk_AggDoc.Checked)
                AggDoc();

        }

        private void AggDoc()
        {
            try
            {
                //تجمیع سطرها

                DataTable _1Table = SourceTable.DefaultView.ToTable("_1Table", true, new string[] { "Type", "Column01", "Column07", "Column08", "Column09", "Column13", "Column14", "Bed" });
                DataTable _2Table = SourceTable.Clone();
                foreach (DataRow item in _1Table.Rows)
                {
                    SourceTable.DefaultView.RowFilter = "Column01='" + item["Column01"].ToString() + "' and Type=" + item["Type"].ToString() +
                         (item["Column07"].ToString().Trim() != "" ? " and Column07=" + item["Column07"].ToString() : " and Column07 is null") +
                         (item["Column08"].ToString().Trim() != "" ? " and Column08=" + item["Column08"].ToString() : " and Column08 is null") +
                         (item["Column09"].ToString().Trim() != "" ? " and Column09=" + item["Column09"].ToString() : " and Column09 is null");

                    if (SourceTable.DefaultView.ToTable().Rows.Count > 0)
                    {
                        string Description = SourceTable.DefaultView.ToTable().Rows[0]["Column10"].ToString();
                        Double Bed = Convert.ToDouble(SourceTable.DefaultView.ToTable().Rows[0]["Column11"].ToString());
                        Double Bes = Convert.ToDouble(SourceTable.DefaultView.ToTable().Rows[0]["Column12"].ToString());
                        if (SourceTable.DefaultView.ToTable().Rows.Count > 1)
                        {
                            Description = " فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString();
                            Bed = Convert.ToDouble(SourceTable.DefaultView.ToTable().Compute("SUM(Column11)", "").ToString());
                            Bes = Convert.ToDouble(SourceTable.DefaultView.ToTable().Compute("SUM(Column12)", "").ToString());
                            if (Bed - Bes > 0)
                            {
                                Bed = Bed - Bes;
                                Bes = 0;
                            }
                            else
                            {
                                Bes = Math.Abs(Bed - Bes);
                                Bed = 0;
                            }
                        }

                        if (Bed - Bes != 0 && item["Type"].ToString() == "94")
                            _2Table.Rows.Add(item["Type"], item["Column01"].ToString(), item["Column01"].ToString(),
                                (item["Column07"].ToString().Trim() == "" ? null : item["Column07"].ToString()),
                                (item["Column08"].ToString().Trim() == "" ? null : item["Column08"].ToString()),
                                (item["Column09"].ToString().Trim() == "" ? null : item["Column09"].ToString()),
                                Description, Bed, Bes, Convert.ToDouble(item["Column13"].ToString()),
                                (item["Column14"].ToString().Trim() == "" ? (object)null : Convert.ToInt16(item["Column14"].ToString())));
                        else if (item["Type"].ToString() == "26")
                            _2Table.Rows.Add(item["Type"], item["Column01"].ToString(), item["Column01"].ToString(),
                         (item["Column07"].ToString().Trim() == "" ? null : item["Column07"].ToString()),
                         (item["Column08"].ToString().Trim() == "" ? null : item["Column08"].ToString()),
                         (item["Column09"].ToString().Trim() == "" ? null : item["Column09"].ToString()),
                         Description, Bed, Bes, Convert.ToDouble(item["Column13"].ToString()),
                         (item["Column14"].ToString().Trim() == "" ? (object)null : Convert.ToInt16(item["Column14"].ToString())), item["Bed"]);

                    }

                }
                SourceTable.DefaultView.RowFilter = "";
                gridEX1.DataSource = _2Table;

            }
            catch { }
        }

        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            //*********Just Accounting Document
            SqlParameter DocNum;
            DocNum = new SqlParameter("DocNum", SqlDbType.Int);
            DocNum.Direction = ParameterDirection.Output;
            string CommandTxt = "declare @Key int declare @DetialID int declare @HavaleID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)  declare @ResidID int ";

            try
            {
                gridEX1.UpdateData();
                CheckEssentialItems(sender, e);

                string Message = "آیا مایل به صدور سند حسابداری هستید؟";
                if (uiTab2.Enabled)
                    Message = "آیا مایل به صدور سند حسابداری و حواله و رسید انبار هستید؟";



                if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {

                    //ثبت حواله
                    if (uiTab2.Enabled)
                    {
                        if (clDoc.OperationalColumnValue("Table_080_ReturnAmani", "Column56", _SaleID.ToString()) != 0)
                            throw new Exception("برای این فاکتور حواله و رسید انبار صادر شده است");
                        else
                            ExportDraft();
                    }

                    if (clDoc.OperationalColumnValue("Table_080_ReturnAmani", "Column57", _SaleID.ToString()) != 0)
                        throw new Exception("برای این فاکتور سند حسابداری صادر شده است");


                    this.Cursor = Cursors.WaitCursor;

                    //صدور سند
                    if (rdb_New.Checked)
                    {

                        CommandTxt += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + faDatePicker1.Text + "',2,0,'" + txt_Cover.Text + "','" + Class_BasicOperation._UserName +
                         "',getdate()); SET @Key=SCOPE_IDENTITY()";

                    }
                    else if (rdb_last.Checked)
                    {

                        CommandTxt += " set @DocNum=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))  SET @Key=(Select ColumnId from Table_060_SanadHead where Column00=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0)))";
                    }
                    else if (rdb_TO.Checked)
                    {


                        CommandTxt += " set @DocNum=" + int.Parse(txt_To.Text.Trim()) + "    SET @Key=(Select ColumnId from Table_060_SanadHead where Column00=" + int.Parse(txt_To.Text.Trim()) + ")";


                    }
                    //if (DocID > 0)
                    {
                        gridEX1.UpdateData();
                        CommandTxt += @" set @ResidID=(select Column55 from " + ConSale.Database + ".dbo.Table_080_ReturnAmani where  ColumnId=" + SaleRow["ColumnId"].ToString() + ")";
                        CommandTxt += @" set @HavaleID=(select Column56 from " + ConSale.Database + ".dbo.Table_080_ReturnAmani where  ColumnId=" + SaleRow["ColumnId"].ToString() + ")";
                        CommandTxt += @"set @TotalValue=isnull((select ISNULL(SUM(Column16),0) from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where Column01=@HavaleID),0)";

                        CommandTxt += "set @value=@TotalValue   ";


                        foreach (GridEXRow item in gridEX1.GetRows())
                        {
                            string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column01"].Value.ToString());

                            if (item.Cells["Type"].Value.ToString() == "94" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0 || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                            {

                                CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                    VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                   " + "'" + item.Cells["Column10"].Text.Trim() + "'," + (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0) + @",
                        " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0) + ",0,0,-1,94," + int.Parse(SaleRow["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                 Class_BasicOperation._UserName + "',getdate(),0); SET @DetialID=SCOPE_IDENTITY()";

                                // اضافه کردن اقلام کالا به آرتیکل بدهکار فاکتور فروش
                                if (item.RowIndex == 0 && this.chk_Nots.Checked)
                                {
                                    foreach (DataRowView items in Child1Bind)
                                    {
                                        CommandTxt += @"INSERT INTO Table_075_SanadDetailNotes (Column00,Column01,Column02,Column03,Column04) Values (@DetialID,1,(select Column02 from " + ConWare.Database + ".dbo.table_004_CommodityAndIngredients where ColumnId=" + items["Column02"].ToString() + " ) ," +
                                            items["Column07"].ToString() + "," + items["Column10"].ToString() + ")";

                                    }
                                }
                            }
                            else if (item.Cells["Type"].Value.ToString() == "26" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0 || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                            {


                                CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                    VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                   " + "'" + item.Cells["Column10"].Text.Trim() + "'," + (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0) + @",
                        " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0) + ",0,0,-1,26,@HavaleID ,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                 Class_BasicOperation._UserName + "',getdate(),0);  ";



                            }
                            else if (item.Cells["Type"].Value.ToString() == "26" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 && Convert.ToDouble(item.Cells["Column12"].Value.ToString()) == 0))
                            {

                                SqlDataAdapter Adapter = new SqlDataAdapter(@" SELECT * FROM  [Table_085_ReturnAmaniChild]
                                                    WHERE column01=  " + SaleRow["ColumnId"].ToString() + " AND [column22] is NOT NULL", ConSale);
                                DataTable Project = new DataTable();
                                Adapter.Fill(Project);///کالاها پروژه دارند
                                if (Project.Rows.Count > 0)
                                {
                                    CommandTxt += @"set @TotalValue=isnull((select ISNULL(SUM(Column16),0) from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where Column01=@HavaleID" + (item.Cells["Column09"].Text.Trim() == "" ? " and (Column14 is null or Column14='') " : " and Column14=" + item.Cells["Column09"].Value) + "),0)";
                                    CommandTxt += "set @value=@TotalValue   ";
                                }
                                else
                                {
                                    CommandTxt += @"set @TotalValue=isnull((select ISNULL(SUM(Column16),0) from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where Column01=@HavaleID),0)";
                                    CommandTxt += "set @value=@TotalValue   ";
                                }

                                if (Convert.ToInt16(item.Cells["Bed"].Value) == 1)
                                {
                                    CommandTxt += @" if @TotalValue>0 begin  INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                    VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                               " + "'" + item.Cells["Column10"].Text.Trim() + @"',
                                @TotalValue ,0,0,0,-1,26,@HavaleID  ,'" + Class_BasicOperation._UserName + "',getdate(),'" +
             Class_BasicOperation._UserName + "',getdate(),0); set @value = 0 end ";
                                }
                                else
                                {
                                    CommandTxt += @" if @TotalValue>0 begin  INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22]) 
                    VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                               " + "'" + item.Cells["Column10"].Text.Trim() + @"',
                                0 ,@TotalValue,0,0,-1,26,@HavaleID  ,'" + Class_BasicOperation._UserName + "',getdate(),'" +
            Class_BasicOperation._UserName + "',getdate(),0); set @value = 0 end ";
                                }

                            }

                        }



                        //CommandTxt += @"Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07= @Key  where ColumnId =@HavaleID    ";
                        CommandTxt += @"Update " + ConSale.Database + ".dbo.Table_080_ReturnAmani set Column57= @Key,Column56= @HavaleID,Column55=@ResidID,Column15='" + Class_BasicOperation._UserName + "',Column16=getdate()  where ColumnId =" + int.Parse(SaleRow["ColumnId"].ToString());





                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Con.Open();

                            SqlTransaction sqlTran = Con.BeginTransaction();
                            SqlCommand Command = Con.CreateCommand();
                            Command.Transaction = sqlTran;

                            try
                            {
                                Command.CommandText = CommandTxt;
                                Command.Parameters.Add(DocNum);
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();
                                if (Convert.ToInt32(DraftNum.Value) == 0)
                                    Class_BasicOperation.ShowMsg("", "سند حسابداری با شماره " + DocNum.Value + " با موفقیت ثبت گردید", "Information");
                                else Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                                    "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره حواله انبار: " + DraftNum.Value + Environment.NewLine + "شماره رسید انبار: " + ResidNum.ToString(), "Information");

                                bt_ExportDoc.Enabled = false;
                                this.DialogResult = DialogResult.Yes;
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

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }



        }

        private void CheckEssentialItems(object sender, EventArgs e)
        {
            if (rdb_last.Checked && txt_LastNum.Text.Trim() != "")
            {
                clDoc.IsFinal(int.Parse(txt_LastNum.Text.Trim()));
            }
            else if (rdb_TO.Checked && txt_To.Text.Trim() != "")
            {
                clDoc.IsValidNumber(int.Parse(txt_To.Text.Trim()));
                clDoc.IsFinal(int.Parse(txt_To.Text.Trim()));
                txt_To_Leave(sender, e);
            }

            foreach (GridEXRow item in gridEX1.GetRows())
            {
                if (item.Cells["Column01"].Text.Trim() == "" || item.Cells["Column10"].Text.Trim() == "")
                    throw new Exception("اطلاعات مورد نیاز جهت صدور سند را کامل کنید");
                if (item.Cells["Column01"].Text.Trim().All(char.IsDigit))
                {
                    throw new Exception("سرفصل" + item.Cells["Column01"].Text + "نامعتبر است");

                }
            }
            if (Convert.ToDouble(gridEX1.GetTotalRow().Cells["Column11"].Value.ToString()) == 0)
                throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

            if (txt_Cover.Text.Trim() == "" || faDatePicker1.Text.Length != 10)
                throw new Exception("اطلاعات مورد نیاز جهت صدور سند را کامل کنید");

            if (uiTab2.Enabled && (mlt_WareDraft.Text.Trim() == "" || mlt_FunctionDraft.Text.Trim() == ""))
                throw new Exception("اطلاعات مورد نیاز جهت صدور حواله انبار را کامل کنید");



            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();

            if (uiTab2.Enabled)
            {
                foreach (DataRowView item in Child1Bind)
                {
                    if (!clGood.IsGoodInWare(short.Parse(wareDraft.ToString()),
                        int.Parse(item["Column02"].ToString())))
                        throw new Exception("کالای " +
                            clDoc.ExScalar(ConWare.ConnectionString, "table_004_CommodityAndIngredients", "Column02",
                            "ColumnId", item["Column02"].ToString()) + " در انبار انتخاب شده فعال نمی باشد");

                }
            }

            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));

            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));

            //Person--Center--Project//
            int? Person = null;
            Int16? Center = null;
            Int16? Project = null;
            TPerson.Rows.Clear();
            TAccounts.Rows.Clear();

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            {
                Person = null;
                Center = null;
                Project = null;
                if (item.Cells["Column07"].Text.Trim() != "")
                    Person = int.Parse(item.Cells["Column07"].Value.ToString());

                if (item.Cells["Column08"].Text.Trim() != "")
                    Center = Int16.Parse(item.Cells["Column08"].Value.ToString());

                if (item.Cells["Column09"].Text.Trim() != "")
                    Project = Int16.Parse(item.Cells["Column09"].Value.ToString());

                clCredit.All_Controls_Row(item.Cells["Column01"].Value.ToString(), Person, Center, Project, item);

                //**********Check Person Credit************//
                if (item.Cells["Column07"].Text.Trim() != "")
                {
                    if (item.Cells["Column07"].Text.Trim() != "")
                        TPerson.Rows.Add(Int32.Parse(item.Cells["Column07"].Value.ToString()), item.Cells["Column01"].Value.ToString()
                            , (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) * -1 :
                            Convert.ToDouble(item.Cells["Column11"].Value.ToString())));
                }
                //**********Check Account's nature****//
                TAccounts.Rows.Add(item.Cells["Column01"].Value.ToString(), (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) * -1 :
                            Convert.ToDouble(item.Cells["Column11"].Value.ToString())));
            }

            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);

        }

        private void Frm_009_ExportDocInformation_KeyDown(object sender, KeyEventArgs e)
        {
            if (bt_ExportDoc.Enabled && e.Control && e.KeyCode == Keys.S)
                bt_ExportDoc_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.W)
                bt_ViewDocs_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_ViewDrafts_Click(sender, e);
        }

        private void
            ExportDraft()
        {
            string command = string.Empty;



            if (clDoc.OperationalColumnValue("Table_080_ReturnAmani", "Column56", _SaleID.ToString()) != 0)
                throw new Exception("برای این فاکتور حواله انبار صادر شده است");
            if (clDoc.OperationalColumnValue("Table_080_ReturnAmani", "Column55", SaleRow["ColumnId"].ToString()) != 0)
                throw new Exception("برای این فاکتور، رسید انبار صادر شده است");
            if (mlt_FunctionDraft.Text.Trim() == "" || mlt_FunctionRecipt.Text.Trim() == "")
                throw new Exception("اطلاعات مورد نیاز را تکمیل کنید");



            if (!clDoc.AllService(Child1Bind))
            {
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
                                item["Column02"].ToString()) + " کمتر از تعداد مشخص شده در فاکتور است");
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
                    {
                        ok = false;
                        throw new Exception("موجودی کل کالاهای زیر منفی می شود" + Environment.NewLine + good1);
                    }
                }
                if (ok)
                {
                    //درج هدر حواله
                    command = @"  declare @Key int     set @DraftNum=(SELECT ISNULL(Max(Column01),0)+1  from Table_007_PwhrsDraft)  INSERT INTO Table_007_PwhrsDraft ([column01]
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
                                                                                               ,[Column26]) VALUES(@DraftNum,'" + SaleRow["Column02"].ToString() + "'," + wareDraft
                              + "," + mlt_FunctionDraft.Value.ToString() + "," + SaleRow["Column03"].ToString() + ",'" + "حواله صادره بابت انتقالی بین انبارهای مربوط به فاکتور مرجوعی امانی شماره" + SaleRow["Column01"].ToString() +
                              "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0," +
                            0 + ",0,0,0,0," +
                            "0" + "," + "NULL" + "," +
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

                    command += "Update " + ConSale.Database + ".dbo.Table_080_ReturnAmani set Column56=@Key,Column15='" + Class_BasicOperation._UserName + "',Column16=getdate() where ColumnId=" + int.Parse(SaleRow["ColumnId"].ToString());




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
                                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO] " : "  [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + wareDraft, ConWare);
                                    DataTable TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftID + " and DetailID=" + item["Columnid"].ToString());

                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                        + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                    UpdateCommand.ExecuteNonQuery();
                                }
                                else
                                {
                                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + wareDraft + ",@Date='" + SaleRow["Column02"].ToString() + "',@id=" + DraftID + ",@residid=0", ConWare);
                                    DataTable TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                  + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["Columnid"].ToString(), Con);
                                    UpdateCommand.ExecuteNonQuery();
                                }

                            }

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
                        wareRecipt + "," + mlt_FunctionRecipt.Value + ",NULL,'" + "رسید صادره بابت انتقالی بین انبارهای مربوط به فاکتور مرجوعی امانی شماره" +
                         SaleRow["Column01"].ToString() + " تاریخ " + SaleRow["Column02"].ToString() + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate()," +
                        "0" + ",0,0,0," +
                        "0" + "," +
                          "NULL" + "," +
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
                                                                                    FROM   Table_085_ReturnAmaniChild tcbf
                                                                                    WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                           AND tcbf.column01 = " + SaleRow["columnid"].ToString() + @"
                                                                                ),
                                                                                0
                                                                            ) / nullif( ISNULL(
                                                                                (
                                                                                    SELECT SUM(tcbf.column07)
                                                                                    FROM   Table_085_ReturnAmaniChild tcbf
                                                                                    WHERE  tcbf.column02 = " + item["Column02"].ToString() + @"
                                                                                           AND tcbf.column01 = " + SaleRow["columnid"].ToString() + @"
                                                                                ),
                                                                                0
                                                                            ),0)
                                                                        ) * (1 + @share) else isnull(" + Convert.ToDouble(item["Column20"].ToString()) + @" /nullif( " + Convert.ToDouble(item["Column07"].ToString()) + @",0),0) END)

                                                                    SET @totalvalue = @unitvalue * ISNULL(
                                                                            (
                                                                                SELECT SUM(tcbf.column07)
                                                                                FROM   Table_085_ReturnAmaniChild tcbf
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
                               0 + "," + 0 + "," + 0 + "," +
                               0 + ")", Con);
                            InsertDetail.ExecuteNonQuery();
                        }




                        clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_080_ReturnAmani", "Column55", "ColumnId", int.Parse(SaleRow["ColumnId"].ToString()), ResidID);

                    }

                }
            }
        }

        private float FirstRemain(int GoodCode)
        {
            using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                ConWare.Open();
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
                CommandText = string.Format(CommandText, wareDraft, GoodCode, SaleRow["Column02"].ToString());
                SqlCommand Command = new SqlCommand(CommandText, ConWare);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        private void chk_AggDis_CheckedChanged(object sender, EventArgs e)
        {
            //if (chk_LinearDisAdd.Checked || chk_AggDis.Checked)
            //    chk_Baha.Checked = false;
            PrepareDoc();
        }

        private void gridEX1_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            if (Control.ModifierKeys != Keys.Control)
                gridEX1.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "Column01")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column01", "ACC_Code", "ACC_Name", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.ACCColumn);
                else if (e.Column.Key == "Column07")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column07", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.ACCColumn);
                else if (e.Column.Key == "Column08")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column08", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.ACCColumn);
                else if (e.Column.Key == "Column09")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column09", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.ACCColumn);
            }
            catch
            {
            }
        }

        private void chk_RegisterGoods_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.RegisterSaleFactorWithGoods = chk_RegisterGoods.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();

        }

        private void gridEX1_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column01");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column07");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column08");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column09");

            }
            catch
            {
            }
        }

        private void gridEX1_CellEditCanceled(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column01");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column07");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column08");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column09");
            }
            catch
            {
            }
        }

        private void chk_AggDoc_CheckedChanged(object sender, EventArgs e)
        {
            //if (chk_AggDoc.Checked)
            //    chk_Baha.Checked = false;
            Properties.Settings.Default.AggregationSaleDoc = chk_AggDoc.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();

        }

        private string CerateSharh(double TotalValue, double TotalAmount, double TotalNetPrice)
        {

            string Sharh = string.Empty;
            try
            {
                if (Convert.ToBoolean(setting.Rows[3]["Column02"]))
                    Sharh = "فاکتور مرجوعی فروش امانی ش " + SaleRow["Column01"].ToString();
                if (Convert.ToBoolean(setting.Rows[4]["Column02"]))
                    Sharh += " به تاریخ " + SaleRow["Column02"].ToString();
                if (Convert.ToBoolean(setting.Rows[5]["Column02"]))
                    Sharh += " مقدارکل " + TotalValue;
                if (Convert.ToBoolean(setting.Rows[6]["Column02"]))
                    Sharh += " قیمت کل " + string.Format("{0:#,##0.###}", TotalAmount);
                if (Convert.ToBoolean(setting.Rows[8]["Column02"]))
                    Sharh += " مدت اعتبار " + SaleRow["column24"].ToString();
                if (Convert.ToBoolean(setting.Rows[9]["Column02"]))
                    Sharh += " مبلغ خالص " + string.Format("{0:#,##0.###}", TotalNetPrice);
                if (Convert.ToBoolean(setting.Rows[10]["Column02"]))
                    Sharh += " مبلغ قابل پرداخت " + string.Format("{0:#,##0.###}", (TotalNetPrice + Convert.ToDouble(SaleRow["Column32"]) - Convert.ToDouble(SaleRow["Column33"])));

                if (Convert.ToBoolean(setting.Rows[11]["Column02"]))
                    Sharh += " جمع تخفیف " + string.Format("{0:#,##0.###}", SaleRow["Column35"]);
                if (Convert.ToBoolean(setting.Rows[12]["Column02"]))
                    Sharh += " جمع اضافه " + string.Format("{0:#,##0.###}", SaleRow["Column34"]);

                if (Convert.ToBoolean(setting.Rows[13]["Column02"]))
                {
                    string name = string.Empty;

                    using (SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE))
                    {
                        ConBase.Open();
                        string CommandText = @"SELECT     Column02 From Table_045_PersonInfo WHERE ColumnId=" + SaleRow["column03"] + "";

                        SqlCommand Command = new SqlCommand(CommandText, ConBase);
                        name = Command.ExecuteScalar().ToString();
                    }
                    if (name != string.Empty)
                        Sharh += " خریدار " + name;

                }


                if (Convert.ToBoolean(setting.Rows[14]["Column02"]) || Convert.ToBoolean(setting.Rows[16]["Column02"]))
                {
                    string Ware = string.Empty;
                    string Func = string.Empty;

                    using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        ConWare.Open();
                        string CommandText = @"SELECT     Column02 From Table_001_PWHRS WHERE ColumnId=" + SaleRow["Column42"] + "";

                        SqlCommand Command = new SqlCommand(CommandText, ConWare);
                        Ware = Command.ExecuteScalar().ToString();



                        string CommandText1 = @"SELECT     Column02 From table_005_PwhrsOperation WHERE ColumnId=" + SaleRow["Column43"] + "";

                        SqlCommand Command1 = new SqlCommand(CommandText1, ConWare);
                        Func = Command1.ExecuteScalar().ToString();


                    }
                    if (Ware != string.Empty)
                        Sharh += " انبار " + Ware;

                    if (Func != string.Empty)
                        Sharh += " نوع حواله " + Func;

                }


                if (Convert.ToBoolean(setting.Rows[7]["Column02"]) && SaleRow["column06"] != null && SaleRow["column06"].ToString() != string.Empty)
                    Sharh += "  " + SaleRow["column06"].ToString();
                if (Convert.ToBoolean(setting.Rows[15]["Column02"]) && SaleRow["column04"] != null && SaleRow["column04"].ToString() != string.Empty)
                    Sharh += "  " + SaleRow["column04"].ToString();


                //if (Convert.ToBoolean(setting.Rows[32]["Column02"]))
                //    Sharh += " میانگین فی " + string.Format("{0:#,##0.###}", (TotalAmount / TotalValue));


                //if (Convert.ToBoolean(setting.Rows[33]["Column02"]))
                //{

                //    using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS))
                //    {
                //        ConWare.Open();
                //        string CommandText = @"SELECT     Column02 From table_004_CommodityAndIngredients WHERE ColumnId=(select top 1 column02 from " + ConSale.Database + ".dbo.Table_085_ReturnAmaniChild where column01 =" + SaleRow["columnid"] + ")   ";

                //        SqlCommand Command = new SqlCommand(CommandText, ConWare);
                //        string good = Command.ExecuteScalar().ToString();

                //        Sharh += "  " + good;


                //    }
                //}
            }
            catch
            { }
            return Sharh;
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
                CommandText = string.Format(CommandText, wareDraft, GoodCode);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        private void chk_Nots_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.RegisterSaleFactorNoteGoods = chk_Nots.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();

        }

        private void chk_without_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.whtoutgoods = chk_without.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();
        }

        private void chk_Baha_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ExtraMethod = chk_Baha.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();

        }

        private void chk_Net_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.chk_Net = chk_Net.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();
        }
    }
}
