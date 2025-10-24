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

namespace PSHOP._04_Buy
{
    public partial class Frm_015_ExportDocInformation_ReturnFactor : Form
    {
        bool _BackSpace = false, _Tab1 = false, _Tab2 = false;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);

        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        Class_UserScope UserScope = new Class_UserScope();
        int _ReturnBuyID, DocNum = 0, DocID = 0, _DraftID = 0, _DraftNum = 0;
        SqlDataAdapter ReturnBuyAdapter, Child2Adapter, Child1Adapter;
        BindingSource ReturnBuyBind, Child1Bind, Child2Bind;
        DataSet DS = new DataSet();
        DataRowView ReturnBuyRow;
        DataTable SourceTable = new DataTable();
        DataTable setting = new DataTable();

        public Frm_015_ExportDocInformation_ReturnFactor(bool Tab1, bool Tab2, int ReturnBuyID)
        {
            InitializeComponent();
            _Tab1 = Tab1;
            _Tab2 = Tab2;
            _ReturnBuyID = ReturnBuyID;
        }

        private void Frm_009_ExportDocInformation_Load(object sender, EventArgs e)
        {
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
            SourceTable.Columns.Add("Column13", Type.GetType("System.Double"));
            SourceTable.Columns.Add("Column14", Type.GetType("System.Int16"));
            gridEX1.DataSource = SourceTable;

            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * from AllHeaders()", ConAcnt);
            Adapter.Fill(DS, "Header");
            gridEX1.DropDowns["Header_Code"].SetDataBinding(DS.Tables["Header"], "");
            gridEX1.DropDowns["Header_Name"].SetDataBinding(DS.Tables["Header"], "");
            mlt_ReturnBuyBed.DataSource = DS.Tables["Header"];
            mlt_ReturnBuyBes.DataSource = DS.Tables["Header"];
            mlt_LinAddBed.DataSource = DS.Tables["Header"];
            mlt_LinAddBes.DataSource = DS.Tables["Header"];
            mlt_LinDisBed.DataSource = DS.Tables["Header"];
            mlt_LinDisBes.DataSource = DS.Tables["Header"];
            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_001_PWHRS  where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + "')", ConWare);
            Adapter.Fill(DS, "Ware");
            mlt_Ware.DataSource = DS.Tables["Ware"];

            Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=1", ConWare);
            Adapter.Fill(DS, "Fun");
            mlt_Function.DataSource = DS.Tables["Fun"];

            //*********************
            ReturnBuyAdapter = new SqlDataAdapter("Select * from Table_021_MarjooiBuy where ColumnId=" + _ReturnBuyID, ConSale);
            ReturnBuyAdapter.Fill(DS, "ReturnBuy");
            ReturnBuyBind = new BindingSource();
            ReturnBuyBind.DataSource = DS.Tables["ReturnBuy"];
            ReturnBuyRow = (DataRowView)this.ReturnBuyBind.CurrencyManager.Current;

            Child2Adapter = new SqlDataAdapter("Select * from  Table_023_Child2_MarjooiBuy where Column01=" + _ReturnBuyID, ConSale);
            Child2Adapter.Fill(DS, "Child2");
            Child2Bind = new BindingSource();
            Child2Bind.DataSource = DS.Tables["Child2"];

            Child1Adapter = new SqlDataAdapter("Select *,CAST(0 as decimal(18, 4)) as UnitValue,CAST(0 as decimal(18, 4)) as TotalValue from Table_022_Child1_MarjooiBuy where Column01=" + _ReturnBuyID, ConSale);
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

            if (ReturnBuyRow["Column15"].ToString() == "False")
            {
                mlt_ReturnBuyBed.Value = clDoc.Account(13, "Column07");
                mlt_ReturnBuyBes.Value = clDoc.Account(13, "Column13");


                mlt_LinAddBed.Value = clDoc.Account(11, "Column13");
                mlt_LinAddBes.Value = clDoc.Account(11, "Column07");

                mlt_LinDisBed.Value = clDoc.Account(10, "Column13");
                mlt_LinDisBes.Value = clDoc.Account(10, "Column07");

            }
            else
            {
                mlt_ReturnBuyBed.Value = clDoc.Account(25, "Column07");
                mlt_ReturnBuyBes.Value = clDoc.Account(25, "Column13");
            }
            faDatePicker1.Text = ReturnBuyRow["Column02"].ToString();
            Adapter = new SqlDataAdapter("Select * from Table_030_Setting", ConBase);
            Adapter.Fill(setting);

            uiTab1.Enabled = _Tab1;
            uiTab2.Enabled = _Tab2;
            gridEX1.MoveFirst();

            chk_Baha.Checked = Properties.Settings.Default.BuyBaha;
            if (Properties.Settings.Default.PCBes)
                chk_PCBes.Checked = true;
            else
                chk_PCBes.Checked = false;
            if (Properties.Settings.Default.PCBed)

                chk_PCBed.Checked = true;
            else
                chk_PCBed.Checked = false;
            chk_AggDoc.Checked = Properties.Settings.Default.BuyAggregationSaleDoc;
            chk_Net.Checked = Properties.Settings.Default.chk_ByuNet;
            chk_GoodACCNum.Checked = Properties.Settings.Default.BuyGoodACCNum;

            chk_RegisterGoods.Checked = Properties.Settings.Default.RegisterBuyFactorWithGoods;
            chk_Nots.Checked = Properties.Settings.Default.RegisterBuyFactorNoteGoods;
            if (!chk_Nots.Checked && !chk_RegisterGoods.Checked)
                chk_without.Checked = true;
            PrepareDoc();
            chk_DraftNum.Checked = false;
            txt_DraftNum.Enabled = false;
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
                txt_Cover.Text = "فاکتور مرجوعی خرید";
                faDatePicker1.Text = ReturnBuyRow["Column02"].ToString();
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
                txt_serial.Enabled = false;
                txt_serial.Text = null;
                txt_To.Enabled = true;
                txt_To.Select();
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

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 20))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form01_AccDocument")
                    {
                        item.BringToFront();
                        TextBox txt_S = (TextBox)item.ActiveControl;
                        txt_S.Text = (DocNum != 0 ? DocNum.ToString() : "1");
                        PACNT._2_DocumentMenu.Form01_AccDocument frms = (PACNT._2_DocumentMenu.Form01_AccDocument)item;
                        frms.bt_Search_Click(sender, e);
                        return;
                    }
                }
                PACNT._2_DocumentMenu.Form01_AccDocument frm = new PACNT._2_DocumentMenu.Form01_AccDocument(
                  UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 20), int.Parse(DocID.ToString()));
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
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
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void PrepareDoc()
        {
            SourceTable.Rows.Clear();
            #region به تفکیک پروژه
            if (chk_Baha.Checked)
            {
                SqlDataAdapter Adapter;
                if (chk_GoodACCNum.Checked)
                    Adapter = new SqlDataAdapter(@"SELECT    Center, Project, column01, 
                            Column20 AS Net ,column11,TotalValue,column50
                             FROM         (SELECT     dbo.Table_022_Child1_MarjooiBuy.Column21 as Center,dbo.Table_022_Child1_MarjooiBuy.column22 AS Project,
                             SUM(ISNULL(dbo.Table_022_Child1_MarjooiBuy.Column20,0)) as Column20, dbo.Table_022_Child1_MarjooiBuy.column01,sum(dbo.Table_022_Child1_MarjooiBuy.column11) as column11,ISNULL(Sum(dbo.Table_022_Child1_MarjooiBuy.column07),0) as TotalValue
                              ,tcai.column50
                             FROM          dbo.Table_022_Child1_MarjooiBuy
                                    JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = dbo.Table_022_Child1_MarjooiBuy.column02
                             GROUP BY dbo.Table_022_Child1_MarjooiBuy.column21,dbo.Table_022_Child1_MarjooiBuy.column22, dbo.Table_022_Child1_MarjooiBuy.column01 ,tcai.column50
                             HAVING      (dbo.Table_022_Child1_MarjooiBuy.column01 = {0})) AS derivedtbl_1", ConSale);
                else

                    Adapter = new SqlDataAdapter(@"SELECT    Center, Project, column01, 
                            Column20 AS Net ,column11,TotalValue,null as column50
                             FROM         (SELECT     Column21 as Center,column22 AS Project,
                             SUM(ISNULL(Column20,0)) as Column20, column01,sum(column11) as column11,ISNULL(Sum(column07),0) as TotalValue
                              
                             FROM          dbo.Table_022_Child1_MarjooiBuy
                                     
                             GROUP BY column21,column22, column01 
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, ReturnBuyRow["ColumnId"].ToString());
                DataTable Table = new DataTable();
                Adapter.Fill(Table);

                DataTable detali = new DataTable();


                if (chk_GoodACCNum.Checked)
                    Adapter = new SqlDataAdapter(@"SELECT tcsf.column20,tcsf.column07,tcsf.column10,tcsf.column11,tcsf.Column20 AS Net,
                                                   tcai.column02,tcsf.column22,tcsf.column21,tcai.column50
                                            FROM   Table_022_Child1_MarjooiBuy tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + ReturnBuyRow["ColumnId"].ToString() + "", ConSale);
                else

                    Adapter = new SqlDataAdapter(@"SELECT tcsf.column20,tcsf.column07,tcsf.column10,tcsf.column11,tcsf.Column20 AS Net,
                                                   tcai.column02,tcsf.column22,tcsf.column21,null as column50
                                            FROM   Table_022_Child1_MarjooiBuy tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + ReturnBuyRow["ColumnId"].ToString() + "", ConSale);
                Adapter.Fill(detali);


                //  فاکتور مرجوعی خرید با احتساب تخفیفات و اضافات خطی

                if (chk_RegisterGoods.Checked)
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


                            if (chk_PCBed.Checked)
                                SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                    (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                     ReturnBuyRow["Column03"].ToString(),
                                    ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                                     ((dt["column22"] != null && !string.IsNullOrWhiteSpace(dt["column22"].ToString())) ? dt["column22"] : null),
                                    "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                                    Convert.ToDouble(dt["column11"])
                                    , 0, 0, -1);

                            else
                                SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                              (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                               ReturnBuyRow["Column03"].ToString(),
                               null,
                               null,
                              "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                              Convert.ToDouble(dt["column11"])
                              , 0, 0, -1);

                            if (chk_PCBes.Checked)

                                SourceTable.Rows.Add(20,
                                ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                               null,
                              ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                               ((dt["column22"] != null && !string.IsNullOrWhiteSpace(dt["column22"].ToString())) ? dt["column22"] : null),
                             "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                               0
                               , Convert.ToDouble(dt["column11"]), 0, -1);
                            else

                                SourceTable.Rows.Add(20,
                                 ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),

                           null,
                           null,
                           null,
                         "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                           0
                           , Convert.ToDouble(dt["column11"]), 0, -1);

                        }
                        else
                        {
                            if (chk_PCBed.Checked)

                                SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                    (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                     ReturnBuyRow["Column03"].ToString(),
                                    ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                                     ((dt["column22"] != null && !string.IsNullOrWhiteSpace(dt["column22"].ToString())) ? dt["column22"] : null),
                                    "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                                    Convert.ToDouble(dt["Net"])
                                    , 0, 0, -1);
                            else

                                SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                            (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                             ReturnBuyRow["Column03"].ToString(),
                             null,
                             null,
                            "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                            Convert.ToDouble(dt["Net"])
                            , 0, 0, -1);
                            if (chk_PCBes.Checked)

                                SourceTable.Rows.Add(20,
                                    ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),

                               null,
                              ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                               ((dt["column22"] != null && !string.IsNullOrWhiteSpace(dt["column22"].ToString())) ? dt["column22"] : null),
                               "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                               0
                               , Convert.ToDouble(dt["Net"]), 0, -1);
                            else
                                SourceTable.Rows.Add(20,
                                 ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                         null,
                                         null,
                                         null,
                                         "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                                         0
                                         , Convert.ToDouble(dt["Net"]), 0, -1);


                        }

                    }

                }
                else
                {

                    foreach (DataRow item in Table.Rows)
                    {

                        if (!chk_Net.Checked)
                        {
                            //********Bed
                            if (chk_PCBed.Checked)

                                SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                    (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                    ReturnBuyRow["Column03"].ToString(),
                                    ((item["Center"] != null && !string.IsNullOrWhiteSpace(item["Center"].ToString())) ? item["Center"].ToString() : null),
                                    ((item["Project"] != null && !string.IsNullOrWhiteSpace(item["Project"].ToString())) ? item["Project"].ToString() : null),
                                    "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                    Convert.ToDouble(item["column11"].ToString()), 0, 0, -1);
                            else
                                SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                ReturnBuyRow["Column03"].ToString(),
                                null,
                                null,
                                "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                Convert.ToDouble(item["column11"].ToString()), 0, 0, -1);
                            //*********Bes
                            if (chk_PCBes.Checked)

                                SourceTable.Rows.Add(20,
                                     ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),


                                    null,
                                    (item["Center"].ToString().Trim() == "" ? null : item["Center"].ToString()),
                                    (item["Project"].ToString().Trim() == "" ? null : item["Project"].ToString()),
                                    "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                    0,
                                    Convert.ToDouble(item["column11"].ToString()),
                                    0, -1);
                            else
                                SourceTable.Rows.Add(20,
                                    ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),

                                null,
                                null,
                                null,
                                "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                0,
                                Convert.ToDouble(item["column11"].ToString()),
                                0, -1);
                        }
                        else
                        {
                            //********Bed
                            if (chk_PCBed.Checked)

                                SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                    (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                    ReturnBuyRow["Column03"].ToString(),
                                    ((item["Center"] != null && !string.IsNullOrWhiteSpace(item["Center"].ToString())) ? item["Center"].ToString() : null),
                                    ((item["Project"] != null && !string.IsNullOrWhiteSpace(item["Project"].ToString())) ? item["Project"].ToString() : null),
                                    "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                    Convert.ToDouble(item["Net"].ToString()), 0, 0, -1);
                            else
                                SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                               (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                               ReturnBuyRow["Column03"].ToString(),
                               null,
                               null,
                               "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                               Convert.ToDouble(item["Net"].ToString()), 0, 0, -1);

                            //*********Bes
                            if (chk_PCBes.Checked)

                                SourceTable.Rows.Add(20,
                                   ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),

                                    null,
                                    (item["Center"].ToString().Trim() == "" ? null : item["Center"].ToString()),
                                    (item["Project"].ToString().Trim() == "" ? null : item["Project"].ToString()),
                                    "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                    0,
                                    Convert.ToDouble(item["Net"].ToString()),
                                    0, -1);
                            else
                                SourceTable.Rows.Add(20,
                                    ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),

                                null,
                                null,
                                null,
                                "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                0,
                                Convert.ToDouble(item["Net"].ToString()),
                                0, -1);

                        }
                    }

                }

                // مربوط به اضافات خطی فاکتور
                if (Convert.ToDouble(ReturnBuyRow["Column21"].ToString()) != 0 && !chk_Net.Checked)
                {


                    DataTable ezafeTable = clDoc.ReturnTable(ConSale.ConnectionString,
               "Select SUM(column19) as ezafe, column22,column21 from Table_022_Child1_MarjooiBuy where column01=" + ReturnBuyRow["ColumnId"].ToString() + "  group by column22,column21");
                    foreach (DataRow d in ezafeTable.Rows)
                    {
                        if (Convert.ToDouble(d["ezafe"].ToString()) != 0)
                        {
                            if (chk_PCBed.Checked)

                                //********Bed
                                SourceTable.Rows.Add(20, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                    (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                    ReturnBuyRow["Column03"].ToString(),
                                    ((d["column21"] != DBNull.Value && d["column21"] != null && !string.IsNullOrWhiteSpace(d["column21"].ToString())) ? d["column21"].ToString() : null),
                                    ((d["column22"] != DBNull.Value && d["column22"] != null && !string.IsNullOrWhiteSpace(d["column22"].ToString())) ? d["column22"].ToString() : null),
                                    (Convert.ToDouble(ReturnBuyRow["Column21"].ToString()) > 0 ? "اضافه خطی مرجوعی فاکتور خرید ش " : "تخفیف خطی2 مرجوعی فاکتور خرید ش ") +
                                    ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                    Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                            else

                                SourceTable.Rows.Add(20, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                ReturnBuyRow["Column03"].ToString(),
                                null,
                                null,
                                (Convert.ToDouble(ReturnBuyRow["Column21"].ToString()) > 0 ? "اضافه خطی مرجوعی فاکتور خرید ش " : "تخفیف خطی2 مرجوعی فاکتور خرید ش ") +
                                ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);

                            //*********Bes

                            if (chk_PCBes.Checked)

                                SourceTable.Rows.Add(20, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                    (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                    null,
                                    ((d["column21"] != DBNull.Value && d["column21"] != null && !string.IsNullOrWhiteSpace(d["column21"].ToString())) ? d["column21"].ToString() : null),
                                    ((d["column22"] != DBNull.Value && d["column22"] != null && !string.IsNullOrWhiteSpace(d["column22"].ToString())) ? d["column22"].ToString() : null),
                                    (Convert.ToDouble(ReturnBuyRow["Column21"].ToString()) > 0 ? "اضافه خطی مرجوعی فاکتور خرید ش " : "تخفیف خطی2 مرجوعی فاکتور خرید ش ")
                                    + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                    0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                            else

                                SourceTable.Rows.Add(20, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                null,
                                null,
                                null,
                                (Convert.ToDouble(ReturnBuyRow["Column21"].ToString()) > 0 ? "اضافه خطی مرجوعی فاکتور خرید ش " : "تخفیف خطی2 مرجوعی فاکتور خرید ش ")
                                + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);

                        }
                    }



                }

                //ثبت مربوط به تخفیفات خطی فاکتور
                if (Convert.ToDouble(ReturnBuyRow["Column22"].ToString()) > 0 && !chk_Net.Checked)
                {
                    DataTable takhfifTable = clDoc.ReturnTable(ConSale.ConnectionString,
                        "Select SUM(column17) as takhfif, column22,column21 from Table_022_Child1_MarjooiBuy where column01=" + ReturnBuyRow["ColumnId"].ToString() + "  group by column22,column21");


                    foreach (DataRow h in takhfifTable.Rows)
                    {

                        if (Convert.ToInt64(h["takhfif"]) > 0)
                        {
                            //********Bed
                            if (chk_PCBed.Checked)

                                SourceTable.Rows.Add(20, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                    (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                   null,
                                    ((h["column21"] != DBNull.Value && h["column21"] != null && !string.IsNullOrWhiteSpace(h["column21"].ToString())) ? h["column21"].ToString() : null),
                                    ((h["column22"] != DBNull.Value && h["column22"] != null && !string.IsNullOrWhiteSpace(h["column22"].ToString())) ? h["column22"].ToString() : null),
                                    "تخفیف خطی مرجوعی فاکتور خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);
                            else
                                SourceTable.Rows.Add(20, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                               (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                              null,
                              null,
                              null,
                               "تخفیف خطی مرجوعی فاکتور خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);

                            //*********Bes
                            if (chk_PCBes.Checked)

                                SourceTable.Rows.Add(20, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                    (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                    ReturnBuyRow["Column03"].ToString(),
                                    ((h["column21"] != DBNull.Value && h["column21"] != null && !string.IsNullOrWhiteSpace(h["column21"].ToString())) ? h["column21"].ToString() : null),
                                    ((h["column22"] != DBNull.Value && h["column22"] != null && !string.IsNullOrWhiteSpace(h["column22"].ToString())) ? h["column22"].ToString() : null), "تخفیف خطی مرجوعی فاکتور خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                            else

                                SourceTable.Rows.Add(20, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                               (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                               ReturnBuyRow["Column03"].ToString(),
                               null,
                               null,
                               "تخفیف خطی مرجوعی فاکتور خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);

                        }
                    }


                }
            }
            #endregion

            #region عدم تفکیک پروژه
            else
            {
                SqlDataAdapter Adapter;
                if (chk_GoodACCNum.Checked)
                    Adapter = new SqlDataAdapter(@"SELECT      column01, 
                            Column20 AS Net ,column11,TotalValue,column50
                             FROM         (SELECT      
                             SUM(ISNULL(dbo.Table_022_Child1_MarjooiBuy.Column20,0)) as Column20, dbo.Table_022_Child1_MarjooiBuy.column01,sum(dbo.Table_022_Child1_MarjooiBuy.column11) as column11,ISNULL(Sum(dbo.Table_022_Child1_MarjooiBuy.column07),0) as TotalValue
                              ,tcai.column50
                             FROM          dbo.Table_022_Child1_MarjooiBuy
                                      JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = dbo.Table_022_Child1_MarjooiBuy.column02 
                             GROUP BY   dbo.Table_022_Child1_MarjooiBuy.column01 ,tcai.column50
                             HAVING      (dbo.Table_022_Child1_MarjooiBuy.column01 = {0})) AS derivedtbl_1", ConSale);
                else

                    Adapter = new SqlDataAdapter(@"SELECT      column01, 
                            Column20 AS Net ,column11,TotalValue,null as column50
                             FROM         (SELECT      
                             SUM(ISNULL(Column20,0)) as Column20, column01,sum(column11) as column11,ISNULL(Sum(column07),0) as TotalValue
                              
                             FROM          dbo.Table_022_Child1_MarjooiBuy
                             GROUP BY   column01 
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, ReturnBuyRow["ColumnId"].ToString());
                DataTable Table = new DataTable();
                Adapter.Fill(Table);

                DataTable detali = new DataTable();

                if (chk_GoodACCNum.Checked)
                    Adapter = new SqlDataAdapter(@"SELECT tcsf.column20,tcsf.column07,tcsf.column10,tcsf.column11,tcsf.Column20 AS Net,
                                                   tcai.column02 ,tcai.column50
                                            FROM   Table_022_Child1_MarjooiBuy tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + ReturnBuyRow["ColumnId"].ToString() + "", ConSale);
                else
                    Adapter = new SqlDataAdapter(@"SELECT tcsf.column20,tcsf.column07,tcsf.column10,tcsf.column11,tcsf.Column20 AS Net,
                                                   tcai.column02 ,null as column50
                                            FROM   Table_022_Child1_MarjooiBuy tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + ReturnBuyRow["ColumnId"].ToString() + "", ConSale);
                Adapter.Fill(detali);


                //  فاکتور مرجوعی خرید با احتساب تخفیفات و اضافات خطی

                if (chk_RegisterGoods.Checked)
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



                            SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                 ReturnBuyRow["Column03"].ToString(),
                                null,
                                null,
                                "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                                Convert.ToDouble(dt["column11"])
                                , 0, 0, -1);

                            SourceTable.Rows.Add(20,
                                  ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                  ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                   null,
                                  null,
                                   null,
                                 "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                                   0
                                   , Convert.ToDouble(dt["column11"]), 0, -1);

                        }
                        else
                        {

                            SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                 ReturnBuyRow["Column03"].ToString(),
                                null,
                                null,
                                "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                                Convert.ToDouble(dt["Net"])
                                , 0, 0, -1);
                            SourceTable.Rows.Add(20,
                               ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                  ((dt["column50"] != null && !string.IsNullOrWhiteSpace(dt["column50"].ToString())) ? dt["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),

                           null,
                          ((dt["column21"] != null && !string.IsNullOrWhiteSpace(dt["column21"].ToString())) ? dt["column21"] : null),
                           ((dt["column22"] != null && !string.IsNullOrWhiteSpace(dt["column22"].ToString())) ? dt["column22"] : null),
                           "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString() + sharhjoz,
                           0
                           , Convert.ToDouble(dt["Net"]), 0, -1);


                        }

                    }

                }
                else
                {

                    foreach (DataRow item in Table.Rows)
                    {

                        if (!chk_Net.Checked)
                        {
                            //********Bed

                            SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                ReturnBuyRow["Column03"].ToString(),
                                null,
                                null,
                                "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                Convert.ToDouble(item["column11"].ToString()), 0, 0, -1);


                            //*********Bes

                            SourceTable.Rows.Add(20,
                               ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                  ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),

                                null,
                                null,
                                null,
                                "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                0,
                                Convert.ToDouble(item["column11"].ToString()),
                                0, -1);
                        }
                        else
                        {
                            //********Bed

                            SourceTable.Rows.Add(20, (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                (mlt_ReturnBuyBed.Text.Trim() != "" ? mlt_ReturnBuyBed.Value.ToString() : null),
                                ReturnBuyRow["Column03"].ToString(),
                                null,
                                null,
                                "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                Convert.ToDouble(item["Net"].ToString()), 0, 0, -1);


                            //*********Bes

                            SourceTable.Rows.Add(20,
                               ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),
                                  ((item["column50"] != null && !string.IsNullOrWhiteSpace(item["column50"].ToString())) ? item["column50"].ToString() : (mlt_ReturnBuyBes.Text.Trim() != "" ? mlt_ReturnBuyBes.Value.ToString() : null)),

                                null,
                                null,
                                null,
                                "فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                0,
                                Convert.ToDouble(item["Net"].ToString()),
                                0, -1);
                        }

                    }

                }

                // مربوط به اضافات خطی فاکتور
                if (Convert.ToDouble(ReturnBuyRow["Column21"].ToString()) != 0 && !chk_Net.Checked)
                {


                    DataTable ezafeTable = clDoc.ReturnTable(ConSale.ConnectionString,
               "Select SUM(column19) as ezafe  from Table_022_Child1_MarjooiBuy where column01=" + ReturnBuyRow["ColumnId"].ToString() + "  ");
                    foreach (DataRow d in ezafeTable.Rows)
                    {
                        if (Convert.ToDouble(d["ezafe"].ToString()) != 0)
                        {
                            //********Bed
                            SourceTable.Rows.Add(20, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                ReturnBuyRow["Column03"].ToString(),
                                null,
                                null,
                                (Convert.ToDouble(ReturnBuyRow["Column21"].ToString()) > 0 ? "اضافه خطی مرجوعی فاکتور خرید ش " : "تخفیف خطی2 مرجوعی فاکتور خرید ش ") +
                                ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                            //*********Bes
                            SourceTable.Rows.Add(20, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                null,
                                null,
                                null,
                                (Convert.ToDouble(ReturnBuyRow["Column21"].ToString()) > 0 ? "اضافه خطی مرجوعی فاکتور خرید ش " : "تخفیف خطی2 مرجوعی فاکتور خرید ش ")
                                + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                                0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                        }
                    }



                }

                //ثبت مربوط به تخفیفات خطی فاکتور
                if (Convert.ToDouble(ReturnBuyRow["Column22"].ToString()) > 0 && !chk_Net.Checked)
                {
                    DataTable takhfifTable = clDoc.ReturnTable(ConSale.ConnectionString,
                        "Select SUM(column17) as takhfif  from Table_022_Child1_MarjooiBuy where column01=" + ReturnBuyRow["ColumnId"].ToString() + "   ");


                    foreach (DataRow h in takhfifTable.Rows)
                    {

                        if (Convert.ToInt64(h["takhfif"]) > 0)
                        {
                            //********Bed
                            SourceTable.Rows.Add(20, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null),
                                null,
                                null,
                                null,
                                "تخفیف خطی مرجوعی فاکتور خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);
                            //*********Bes
                            SourceTable.Rows.Add(20, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null),
                                ReturnBuyRow["Column03"].ToString(),
                                null,
                                null, "تخفیف خطی مرجوعی فاکتور خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                        }
                    }


                }
            }


            #endregion
            //سایر اضافات و کسورات
            foreach (DataRowView item in Child2Bind)
            {
                string Bed = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column16", "ColumnId", item["Column02"].ToString());
                string Bes = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column10", "ColumnId", item["Column02"].ToString());
                string Name = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount_Buy", "Column01", "ColumnId", item["Column02"].ToString());

                //********Bed
                SourceTable.Rows.Add(20, Bed, Bed, (item["Column05"].ToString() == "False" ? ReturnBuyRow["Column03"].ToString() : null), null, null,
                     Name + " فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(),
                    Convert.ToDouble(item["Column04"].ToString()), 0, (ReturnBuyRow["Column24"].ToString().Trim() == "" ? "0" : ReturnBuyRow["Column24"].ToString()),
                     (ReturnBuyRow["Column23"].ToString().Trim() == "" ? "-1" : ReturnBuyRow["Column23"].ToString()));

                //*********Bes
                SourceTable.Rows.Add(20, Bes, Bes, (item["Column05"].ToString() == "True" ? ReturnBuyRow["Column03"].ToString() : null), null, null,
                 Name + " فاکتور مرجوعی خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString(), 0,
                Convert.ToDouble(item["Column04"].ToString()), (ReturnBuyRow["Column24"].ToString().Trim() == "" ? "0" : ReturnBuyRow["Column24"].ToString()),
                 (ReturnBuyRow["Column23"].ToString().Trim() == "" ? "-1" : ReturnBuyRow["Column23"].ToString()));
            }
            gridEX1.DataSource = SourceTable;

            if (chk_AggDoc.Checked)
                AggDoc();
        }


        private void AggDoc()
        {
            try
            {
                //تجمیع سطرها

                DataTable _1Table = SourceTable.DefaultView.ToTable("_1Table", true, new string[] { "Type", "Column01", "Column07", "Column08", "Column09", "Column13", "Column14" });
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
                            Description = " مرجوعی فاکتور خرید ش " + ReturnBuyRow["Column01"].ToString() + " به تاریخ " + ReturnBuyRow["Column02"].ToString();
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

                        if (Bed - Bes != 0 && item["Type"].ToString() == "20")
                            _2Table.Rows.Add(item["Type"], item["Column01"].ToString(), item["Column01"].ToString(),
                                (item["Column07"].ToString().Trim() == "" ? null : item["Column07"].ToString()),
                                (item["Column08"].ToString().Trim() == "" ? null : item["Column08"].ToString()),
                                (item["Column09"].ToString().Trim() == "" ? null : item["Column09"].ToString()),
                                Description, Bed, Bes, Convert.ToDouble(item["Column13"].ToString()),
                                (item["Column14"].ToString().Trim() == "" ? (object)null : Convert.ToInt16(item["Column14"].ToString())));


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
            gridEX1.UpdateData();

            SqlParameter DocNum;
            DocNum = new SqlParameter("DocNum", SqlDbType.Int);
            DocNum.Direction = ParameterDirection.Output;
            string CommandTxt = "declare @Key int declare @DetialID int declare @DraftID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)   ";

            try
            {
                CheckEssentialItems(sender, e);

                string Message = "آیا مایل به صدور سند حسابداری هستید؟";
                if (uiTab2.Enabled)
                    Message = "آیا مایل به صدور سند حسابداری و حواله انبار هستید؟";

                if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {

                    //ثبت حواله
                    if (uiTab2.Enabled)
                    {
                        if (clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column10", ReturnBuyRow["ColumnId"].ToString()) != 0)
                            throw new Exception("برای این فاکتور، حواله انبار صادر شده است");
                        else
                            ExportDraft();
                    }

                    if (clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column11", ReturnBuyRow["ColumnId"].ToString()) != 0)
                        throw new Exception("برای این فاکتور، سند حسابداری صادر شده است");
                    //صدور سند
                    if (rdb_New.Checked)
                    {
                        //DocNum = clDoc.LastDocNum() + 1;
                        CommandTxt += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + faDatePicker1.Text + "',2,0,'" + txt_Cover.Text + "','" + Class_BasicOperation._UserName +
                         "',getdate()); SET @Key=SCOPE_IDENTITY()";
                        //DocID = clDoc.ExportDoc_Header(DocNum,
                        //    faDatePicker1.Text, txt_Cover.Text, Class_BasicOperation._UserName);
                    }
                    else if (rdb_last.Checked)
                    {
                        //DocNum = clDoc.LastDocNum();
                        //DocID = clDoc.DocID(DocNum);
                        CommandTxt += " set @DocNum=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))  SET @Key=(Select ColumnId from Table_060_SanadHead where Column00=(Select Isnull((Select Max(Column00) from Table_060_SanadHead),0)))";
                    }
                    else if (rdb_TO.Checked)
                    {
                        //DocNum = int.Parse(txt_To.Text.Trim());
                        //DocID = clDoc.DocID(DocNum);
                        CommandTxt += " set @DocNum=" + int.Parse(txt_To.Text.Trim()) + "    SET @Key=(Select ColumnId from Table_060_SanadHead where Column00=" + int.Parse(txt_To.Text.Trim()) + ")";



                    }
                    else if (rb_toSerial.Checked)
                        CommandTxt += " set @DocNum=(Select Column00 from Table_060_SanadHead where ColumnId=" + int.Parse(this.txt_serial.Text.Trim()) + ")    SET @Key=" + int.Parse(this.txt_serial.Text.Trim()) + "";

                    // if (DocID > 0)
                    {
                        gridEX1.UpdateData();
                        // int DraftID = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column10", "ColumnId", ReturnBuyRow["ColumnId"].ToString()));
                        CommandTxt += @" set @DraftID=(select Column10 from " + ConSale.Database + ".dbo.Table_021_MarjooiBuy where  ColumnId=" + ReturnBuyRow["ColumnId"].ToString() + ")";

                        DateTime BaseDate;
                        DateTime SecDate;
                        BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDate.Parse(faDatePicker1.Text));
                        FarsiLibrary.Win.Controls.FADatePicker fa;
                        fa = new FarsiLibrary.Win.Controls.FADatePicker();
                        fa.Text = faDatePicker1.Text;
                        try
                        {
                            SecDate = BaseDate.AddDays(double.Parse(ReturnBuyRow["Column25"].ToString()));
                            fa.SelectedDateTime = SecDate;
                            fa.UpdateTextValue();
                        }
                        catch
                        {
                        }



                        if (ReturnBuyRow["Column15"].ToString() == "False")
                        {
                            foreach (GridEXRow item in gridEX1.GetRows())
                            {
                                string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column01"].Value.ToString());
                                if (item.Cells["Type"].Value.ToString() == "20" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0
                                || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                                {
                                    //int DetialID = clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                    //    _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                    //    , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()),
                                    //    (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                    //    (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()),
                                    //    item.Cells["Column10"].Text.Trim(),
                                    //    (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0),
                                    //    (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0),
                                    //    0, 0, -1, 20, int.Parse(ReturnBuyRow["ColumnId"].ToString()), Class_BasicOperation._UserName, 0);



                                    CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],column23,column24) 
                    VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                               " + "'" + item.Cells["Column10"].Text.Trim() + @"',
                               " + (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0) + @",
                               " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0) + ",0,0,-1,20," + int.Parse(ReturnBuyRow["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                  Class_BasicOperation._UserName + "',getdate(),0," + (ReturnBuyRow["Column25"] != null && !string.IsNullOrWhiteSpace(ReturnBuyRow["Column25"].ToString()) ? ReturnBuyRow["Column25"] : "0") + ",N'" + fa.Text + "'); SET @DetialID=SCOPE_IDENTITY()";

                                    //اضافه کردن اقلام کالا به آرتیکل اول فاکتور مرجوعی
                                    if (item.RowIndex == 0 && chk_Nots.Checked)
                                    {
                                        foreach (DataRowView items in Child1Bind)
                                        {
                                            //clDoc.RunSqlCommand(ConAcnt.ConnectionString, "INSERT INTO Table_075_SanadDetailNotes (Column00,Column01,Column02,Column03,Column04) Values (" + DetialID + ",1,'" +
                                            //    clDoc.ExScalar(ConWare.ConnectionString, "table_004_CommodityAndIngredients", "Column02", "ColumnId", items["Column02"].ToString()) + "'," +
                                            //    items["Column07"].ToString() + "," + items["Column10"].ToString() + ")");
                                            CommandTxt += @"INSERT INTO Table_075_SanadDetailNotes (Column00,Column01,Column02,Column03,Column04) Values (@DetialID,1,(select Column02 from " + ConWare.Database + ".dbo.table_004_CommodityAndIngredients where ColumnId=" + items["Column02"].ToString() + " ) ," +
                                        items["Column07"].ToString() + "," + items["Column10"].ToString() + ")";
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            foreach (GridEXRow item in gridEX1.GetRows())
                            {
                                string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column01"].Value.ToString());
                                if (item.Cells["Type"].Value.ToString() == "20" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0
                                || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                                {

                                    //clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()),
                                    //    _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                    //    , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()),
                                    //    (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                    //    (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()),
                                    //    item.Cells["Column10"].Text.Trim(), 
                                    //    (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString()) *
                                    //    Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0)
                                    //    , (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())
                                    //    * Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0)
                                    //    , (item.Cells["Column12"].Text == "0" ? Convert.ToDouble(item.Cells["Column11"].Value.ToString()) : 0),
                                    //    (item.Cells["Column11"].Text == "0" ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) : 0),
                                    //    Int16.Parse(item.Cells["Column14"].Value.ToString()),
                                    //       20, int.Parse(ReturnBuyRow["ColumnId"].ToString()), Class_BasicOperation._UserName,
                                    //    float.Parse(item.Cells["Column13"].Value.ToString()));

                                    CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
                                                  ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],column23,column24) 
                                                        VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                                                    " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                                                    " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                                                    " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                                                    " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                                                    " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                                                    " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                                                                   " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                                                                   " + "'" + item.Cells["Column10"].Text.Trim() + @"',
                                                                   " + (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString()) *
                                                               Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0) + @",
                                                                    " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())
                                                                * Convert.ToDouble(item.Cells["Column13"].Value.ToString())) : 0) + @",
                                                                   " + (item.Cells["Column12"].Text == "0" ? Convert.ToDouble(item.Cells["Column11"].Value.ToString()) : 0) + @",
                                                                    " + (item.Cells["Column11"].Text == "0" ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) : 0) + @",
                                                                " + Int16.Parse(item.Cells["Column14"].Value.ToString()) + ",20," + int.Parse(ReturnBuyRow["ColumnId"].ToString()) + " ,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                       Class_BasicOperation._UserName + "',getdate()," + float.Parse(item.Cells["Column13"].Value.ToString()) + "," + (ReturnBuyRow["Column25"] != null && !string.IsNullOrWhiteSpace(ReturnBuyRow["Column25"].ToString()) ? ReturnBuyRow["Column25"] : "0") + ",N'" + fa.Text + "')";

                                }

                            }
                        }
                        CommandTxt += @"Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07= @Key  where ColumnId =@DraftID    ";
                        CommandTxt += @"Update " + ConSale.Database + ".dbo.Table_021_MarjooiBuy set Column11= @Key,column07='" + Class_BasicOperation._UserName + "',column08=getdate()  where ColumnId =" + int.Parse(ReturnBuyRow["ColumnId"].ToString());


                        //if (DraftID == 0)
                        //    clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column07", "ColumnId", int.Parse(ReturnBuyRow["Column10"].ToString()), DocID);
                        //else
                        //    clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column07", "ColumnId", DraftID, DocID);
                        //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column11", "ColumnId", int.Parse(ReturnBuyRow["ColumnId"].ToString()), DocID);

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
                                if (_DraftNum == 0)
                                    Class_BasicOperation.ShowMsg("", "سند حسابداری با شماره " + DocNum.Value + " با موفقیت ثبت گردید", "Information");
                                else Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                                    "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره حواله انبار: " + _DraftNum.ToString(), "Information");
                                bt_ExportDoc.Enabled = false;
                                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
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
            else if (rb_toSerial.Checked && this.txt_serial.Text.Trim() != "")
            {
                clDoc.IsValidNumber(clDoc.DocNum(int.Parse(txt_serial.Text.Trim())));
                clDoc.IsFinal(clDoc.DocNum(int.Parse(txt_serial.Text.Trim())));
                txt_serial_Leave(sender, e);
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

            if (uiTab2.Enabled && (mlt_Ware.Text.Trim() == "" || mlt_Function.Text.Trim() == ""))
                throw new Exception("اطلاعات مورد نیاز جهت صدور حواله انبار را کامل کنید");
            if (uiTab2.Enabled && (chk_DraftNum.Checked && Convert.ToInt32(txt_DraftNum.Value) <= 0))
                throw new Exception("اطلاعات مورد نیاز جهت صدور حواله انبار را کامل کنید");

            if (uiTab2.Enabled && (chk_DraftNum.Checked))
            {
                int ok = 0;
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
                    ok = Convert.ToInt32(Select.ExecuteScalar().ToString());
                }
                if (ok == 0)
                    throw new Exception("این شماره حواله استفاده شده است");

            }
            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(faDatePicker1.Text);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();


            if (uiTab2.Enabled)
            {
                foreach (DataRowView item in Child1Bind)
                {
                    if (!clGood.IsGoodInWare(short.Parse(mlt_Ware.Value.ToString()),
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
                TAccounts.Rows.Add(item.Cells["Column01"].Value.ToString(),
                    (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 ? Convert.ToDouble(item.Cells["Column12"].Value.ToString()) * -1 :
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

        private void ExportDraft()
        {
            if (clDoc.OperationalColumnValue("Table_021_MarjooiBuy", "Column10", ReturnBuyRow["ColumnId"].ToString()) != 0)
                throw new Exception("برای این فاکتور، حواله انبار صادر شده است");
            if (!clDoc.AllService(Child1Bind))
            {
                //چک باقی مانده کالا
                foreach (DataRowView item in Child1Bind)
                {
                    if (clDoc.IsGood(item["Column02"].ToString()))
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
                        float Remain = FirstRemain(int.Parse(item["Column02"].ToString()));
                        if (Remain < float.Parse(item["Column07"].ToString()) && !mojoodimanfi)
                        {
                            throw new Exception("موجودی کالای " + clDoc.ExScalar(ConWare.ConnectionString,
                                "table_004_CommodityAndIngredients", "Column02", "ColumnId",
                                item["Column02"].ToString()) + " کمتر از تعداد مشخص شده در فاکتور است");
                        }
                    }
                }



                //**Resid Header
                //, int.Parse(mlt_Ware.Value.ToString()));
                SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                key.Direction = ParameterDirection.Output;
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                {
                    SqlCommand InsertHeader;
                    Con.Open();
                    if (chk_DraftNum.Checked)
                        _DraftNum = Convert.ToInt32(txt_DraftNum.Value);

                    else
                        _DraftNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01");

                    InsertHeader = new SqlCommand(@"INSERT INTO Table_007_PwhrsDraft ([column01]
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
                                                                                               ,[Column26])  VALUES(" + _DraftNum + ",'" + ReturnBuyRow["Column02"].ToString() + "'," + mlt_Ware.Value.ToString()
                 + "," + mlt_Function.Value.ToString() + "," + ReturnBuyRow["Column03"].ToString() + ",'" + "حواله صادره از فاکتور مرجوعی خرید ش" + ReturnBuyRow["Column01"].ToString() +
                 "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0,0,0,0," +
                 ReturnBuyRow["Columnid"].ToString() + ",0,0,0," +
                  (ReturnBuyRow["Column15"].ToString() == "True" ? "1" : "0") + "," +
                         (ReturnBuyRow["Column23"].ToString().Trim() == "" ? "NULL" : ReturnBuyRow["Column23"].ToString()) + "," +
                         ReturnBuyRow["Column24"].ToString()
                 + ",1); SET @Key=SCOPE_IDENTITY()", Con);
                    InsertHeader.Parameters.Add(key);
                    InsertHeader.ExecuteNonQuery();
                    _DraftID = int.Parse(key.Value.ToString());

                    //Resid Detail
                    foreach (DataRowView item in Child1Bind)
                    {
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
           ,[Column35]) VALUES(" + _DraftID + "," + item["Column02"].ToString() + "," +
                               item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," +
                               item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," +
                               item["Column11"].ToString() + ",NULL," +
                               (item["Column21"].ToString().Trim() == "" ? "NULL" : item["Column21"].ToString()) + "," + (item["Column22"].ToString().Trim() == "" ? "NULL" : item["Column22"].ToString())
                               + "," + (double.Parse(item["Column07"].ToString()) > 0 ? double.Parse(item["Column20"].ToString()) / double.Parse(item["Column07"].ToString()) : 0) + "," +
                               item["Column20"].ToString() + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                               + "',getdate(),NULL,NULL," +
                               (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," +
                               item["Column14"].ToString() + ",0,0,0,0,0," + (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column30"].ToString().Trim() + "'")
                                    + "," + (item["Column31"].ToString().Trim() == "" ? "NULL" : "'" + item["Column31"].ToString().Trim() + "'") + "," +
                               item["Column28"].ToString() + "," + item["Column29"].ToString() + "," + item["Column32"].ToString() + "," + item["Column33"].ToString() + ")", Con);
                        InsertDetail.ExecuteNonQuery();
                    }

                    try
                    {
                        SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + _DraftID, Con);
                        DataTable Table = new DataTable();
                        goodAdapter.Fill(Table);

                        //محاسبه ارزش و ذخیره آن در جدول Child1

                     

                        foreach (DataRow item in Table.Rows)
                        {
                            // اگر فاکتور مرجوعی فاکتور خرید داشت ارزش رسید ان فاکتور خرید خوانده میشود
                            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT ISNULL(Table_012_Child_PwhrsReceipt.column20,0) AS column20
                                                                                    FROM   Table_012_Child_PwhrsReceipt
                                                                                           JOIN Table_011_PwhrsReceipt tpr
                                                                                                ON  tpr.columnid = Table_012_Child_PwhrsReceipt.column01
                                                                                           JOIN " + ConSale.Database + @".dbo.Table_015_BuyFactor tbf
                                                                                                ON  tbf.column10 = tpr.columnid
                                                                                           JOIN " + ConSale.Database + @".dbo.Table_021_MarjooiBuy tmb
                                                                                                ON  tmb.column17 = tbf.columnid
                                                                                    WHERE  tmb.columnid = " + ReturnBuyRow["ColumnId"] + @"
                                                                                           AND Table_012_Child_PwhrsReceipt.column02 =" + item["Column02"].ToString(), Con);
                            DataTable TurnTable = new DataTable();
                            Adapter.Fill(TurnTable);
                            if (TurnTable.Rows.Count > 0)
                            {
                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["column20"].ToString()), 4)
                                   + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["column20"].ToString()) * double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                UpdateCommand.ExecuteNonQuery();
                            }
                            // در غیر این صورت ارزش میانیگین محاسبه میشود
                            else
                            {
                                if (Class_BasicOperation._WareType)
                                {
                                    Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO] " : "  [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value.ToString(), Con);
                                    TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    int LastExit = int.Parse(TurnTable.Compute("Max(RowNumber)", " Date<='" + ReturnBuyRow["Column02"].ToString() + "'").ToString());
                                    DataRow[] SelectedRow = TurnTable.Select("RowNumber=" + LastExit);
                                    if (SelectedRow.Count() > 0)
                                    {
                                        SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(SelectedRow[0]["RemainFee"].ToString()), 4)
                                            + " , Column16=" + Math.Round(double.Parse(SelectedRow[0]["RemainFee"].ToString()) * double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                        UpdateCommand.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value + ",@Date='" + ReturnBuyRow["Column02"].ToString() + "',@id=" + _DraftID + ",@residid=0", Con);
                                    TurnTable = new DataTable();
                                    Adapter.Fill(TurnTable);
                                    if (double.Parse(TurnTable.Rows[0]["Avrage"].ToString()) > 0)
                                    {
                                        SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                        UpdateCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }


                    }
                    catch
                    {
                    }

                    clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_021_MarjooiBuy", "Column10", "ColumnId", int.Parse(ReturnBuyRow["ColumnId"].ToString()), _DraftID);
                }
            }
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
            Properties.Settings.Default.RegisterReturnBuyFactorWithGoods = chk_RegisterGoods.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();
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
                CommandText = string.Format(CommandText, mlt_Ware.Value.ToString(), GoodCode, ReturnBuyRow["Column02"].ToString());
                SqlCommand Command = new SqlCommand(CommandText, ConWare);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

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
            PrepareDoc();
        }

        private void chk_Baha_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Baha.Checked)
            {
                chk_PCBed.Checked = true;
                chk_PCBes.Checked = true;

                chk_PCBed.Enabled = true;
                chk_PCBes.Enabled = true;
            }
            else
            {
                chk_PCBed.Enabled = false;
                chk_PCBes.Enabled = false;

                chk_PCBed.Checked = false;
                chk_PCBes.Checked = false;
            }

            PrepareDoc();
        }

        private void chk_Net_CheckedChanged(object sender, EventArgs e)
        {
            PrepareDoc();

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

        private void chk_PCBed_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_PCBed.Checked && !chk_PCBes.Checked)
            {
                chk_Baha.Checked = false;
                chk_PCBed.Enabled = false;
                chk_PCBes.Enabled = false;
            }
            PrepareDoc();
        }

        private void chk_PCBes_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_PCBed.Checked && !chk_PCBes.Checked)
            {
                chk_Baha.Checked = false;
                chk_PCBed.Enabled = false;
                chk_PCBes.Enabled = false;
            }
            PrepareDoc();

        }

        private void chk_DraftNum_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_DraftNum.Checked)
            {
                txt_DraftNum.Enabled = true;
                txt_DraftNum.Select();

            }
            else
                txt_DraftNum.Enabled = false;
        }

        private void chk_GoodACCNum_CheckedChanged(object sender, EventArgs e)
        {
            PrepareDoc();

        }

        private void rb_toSerial_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_toSerial.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_LastNum.Text = null;
                this.txt_serial.Text = string.Empty;
                txt_Cover.Text = string.Empty;
                txt_Cover.Enabled = false;
                txt_serial.Enabled = true;
                txt_To.Enabled = false;
                txt_To.Text = null;
                txt_serial.Select();
            }
            else
            {
                this.txt_serial.Text = null;

                faDatePicker1.Enabled = true;
            }
        }

        private void txt_serial_Leave(object sender, EventArgs e)
        {
            try
            {
                if (this.txt_serial.Text.Trim() != "")
                {
                    clDoc.IsValidNumberS(int.Parse(txt_serial.Text.Trim()));
                    faDatePicker1.Text = clDoc.DocDateS(int.Parse(txt_serial.Text.Trim()));
                    txt_Cover.Text = clDoc.CoverS(int.Parse(txt_serial.Text.Trim()));

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }


    }
}
