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
    public partial class Frm_009_ExportDocInformation : Form
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
        SqlDataAdapter SaleAdapter, Child2Adapter, Child1Adapter;
        BindingSource SaleBind, Child1Bind, Child2Bind;
        DataSet DS = new DataSet();
        DataRowView SaleRow;
        DataTable SourceTable = new DataTable();
        Int16 ware, func;
        SqlParameter DraftNum;

        bool newDraft = false;
        public Frm_009_ExportDocInformation(bool Tab1, bool Tab2, bool Tab3, int SaleID, Int16 Ware, Int16 Func)
        {
            InitializeComponent();
            _Tab1 = Tab1;
            _Tab2 = Tab2;
            _Tab3 = Tab3;
            _SaleID = SaleID;
            ware = Ware;
            func = Func;

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
            mlt_Ware.DataSource = DS.Tables["Ware"];

            Adapter = new SqlDataAdapter("Select * from table_005_PwhrsOperation where Column16=1", ConWare);
            Adapter.Fill(DS, "Fun");
            mlt_Function.DataSource = DS.Tables["Fun"];
            mlt_Ware.Value = ware;
            mlt_Function.Value = func;
            //*********************
            SaleAdapter = new SqlDataAdapter("Select * from Table_010_SaleFactor where ColumnId=" + _SaleID, ConSale);
            SaleAdapter.Fill(DS, "Sale");
            SaleBind = new BindingSource();
            SaleBind.DataSource = DS.Tables["Sale"];
            SaleRow = (DataRowView)this.SaleBind.CurrencyManager.Current;

            Child2Adapter = new SqlDataAdapter("Select * from Table_012_Child2_SaleFactor where Column01=" + _SaleID, ConSale);
            Child2Adapter.Fill(DS, "Child2");
            Child2Bind = new BindingSource();
            Child2Bind.DataSource = DS.Tables["Child2"];

            Child1Adapter = new SqlDataAdapter("Select *,CAST(0 as decimal(18, 4)) as UnitValue,CAST(0 as decimal(18, 4)) as TotalValue from Table_011_Child1_SaleFactor where Column01=" + _SaleID, ConSale);
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


            if (SaleRow["Column12"].ToString() == "False")
            {
                if (SaleRow["Column36"].ToString().Trim() == "")
                {
                    mlt_SaleBed.Value = clDoc.Account(4, "Column07");
                    mlt_SaleBes.Value = clDoc.Account(4, "Column13");
                }
                else
                {
                    mlt_SaleBed.Value = clDoc.ExScalar(ConBase.ConnectionString, "Table_002_SalesTypes", "Column08", "ColumnId", SaleRow["Column36"].ToString());
                    mlt_SaleBes.Value = clDoc.ExScalar(ConBase.ConnectionString, "Table_002_SalesTypes", "Column14", "ColumnId", SaleRow["Column36"].ToString());
                }

                mlt_DisBed.Value = clDoc.Account(5, "Column07");
                mlt_DisBes.Value = clDoc.Account(5, "Column13");
                mlt_LinAddBed.Value = clDoc.Account(7, "Column07");
                mlt_LinAddBes.Value = clDoc.Account(7, "Column13");
                mlt_LinDisBed.Value = clDoc.Account(6, "Column07");
                mlt_LinDisBes.Value = clDoc.Account(6, "Column13");
            }
            else
            {
                mlt_SaleBed.Value = clDoc.Account(18, "Column07");
                mlt_SaleBes.Value = clDoc.Account(18, "Column13");
                mlt_LinAddBed.Value = clDoc.Account(20, "Column07");
                mlt_LinAddBes.Value = clDoc.Account(20, "Column13");
                mlt_LinDisBed.Value = clDoc.Account(19, "Column07");
                mlt_LinDisBes.Value = clDoc.Account(19, "Column13");
            }

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
            Adapter = new SqlDataAdapter("Select * from Table_030_Setting", ConBase);
            Adapter.Fill(setting);
            chk_Baha.Checked = Properties.Settings.Default.ExtraMethod;
            if (Properties.Settings.Default.SalePCBes)
                chk_PCBes.Checked = true;
            else
                chk_PCBes.Checked = false;
            if (Properties.Settings.Default.SalePCBed)

                chk_PCBed.Checked = true;
            else
                chk_PCBed.Checked = false;
            chk_RegisterGoods.Checked = Properties.Settings.Default.RegisterSaleFactorWithGoods;
            chk_Nots.Checked = Properties.Settings.Default.RegisterSaleFactorNoteGoods;
            this.chk_Net.Checked = Properties.Settings.Default.chk_Net;
            chk_AggDoc.Checked = Properties.Settings.Default.AggregationSaleDoc;
            chk_GoodACCNum.Checked = Properties.Settings.Default.SaleGoodACCNum;

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
                txt_Cover.Text = "فاکتور فروش";
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
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default; this.Cursor = Cursors.Default;
            }
        }

        private void rdb_TO_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_TO.Checked)
            {
                faDatePicker1.Enabled = false;
                txt_Cover.Enabled = false;
                this.txt_serial.Enabled = false;
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


                Adapter = new SqlDataAdapter(@" SELECT * FROM  [Table_011_Child1_SaleFactor]
                                                    WHERE column01=  " + SaleRow["ColumnId"].ToString() + " AND [column22] is NOT NULL", ConSale);
                DataTable Project = new DataTable();
                Adapter.Fill(Project);

                # region کالاها پروژه دارند
                if (Project.Rows.Count > 0)//////////کالاها پروژه دارند
                {
                    if (chk_GoodACCNum.Checked)
                        Adapter = new SqlDataAdapter(@"SELECT      Project,Total, Discount, Adding, column01, Total - Discount + Adding AS Net,TotalValue,column33
                             FROM         (SELECT       ISNULL(SUM(dbo.Table_011_Child1_SaleFactor.column11), 0) AS Total, 
                             ISNULL(SUM(dbo.Table_011_Child1_SaleFactor.column17), 0) AS Discount, ISNULL(SUM(dbo.Table_011_Child1_SaleFactor.column19), 0) AS Adding, dbo.Table_011_Child1_SaleFactor.column01,
                               ISNULL(Sum(dbo.Table_011_Child1_SaleFactor.column07),0) as TotalValue,dbo.Table_011_Child1_SaleFactor.column22 as Project,tcai.column33
                             FROM          dbo.Table_011_Child1_SaleFactor JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = dbo.Table_011_Child1_SaleFactor.column02 
                             GROUP BY   dbo.Table_011_Child1_SaleFactor.column01 ,dbo.Table_011_Child1_SaleFactor.column22,tcai.column33
                             HAVING      (dbo.Table_011_Child1_SaleFactor.column01 = {0})) AS derivedtbl_1", ConSale);
                    else
                        Adapter = new SqlDataAdapter(@"SELECT      Project,Total, Discount, Adding, column01, Total - Discount + Adding AS Net,TotalValue,column33
                             FROM         (SELECT       ISNULL(SUM(column11), 0) AS Total, 
                             ISNULL(SUM(column17), 0) AS Discount, ISNULL(SUM(column19), 0) AS Adding, column01,
                               ISNULL(Sum(column07),0) as TotalValue,column22 as Project,null as column33
                             FROM          dbo.Table_011_Child1_SaleFactor
                             GROUP BY   column01 ,column22
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                    Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, SaleRow["ColumnId"].ToString());
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);





                    // فاکتور فروش بدون احتساب تخفیفات و اضافات خطی


                    DataTable detali = new DataTable();
                    if (chk_GoodACCNum.Checked)

                        Adapter = new SqlDataAdapter(@"SELECT  tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02,tcsf.column22 as Project,tcai.column33
                                            FROM   Table_011_Child1_SaleFactor tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + SaleRow["ColumnId"].ToString() + "  ", ConSale);



                    else

                        Adapter = new SqlDataAdapter(@"SELECT  tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02,tcsf.column22 as Project,Null as column33
                                            FROM   Table_011_Child1_SaleFactor tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + SaleRow["ColumnId"].ToString() + "  ", ConSale);
                    Adapter.Fill(detali);

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
                                //********Bed
                                if (chk_PCBed.Checked)
                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                    (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                    null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null), CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                    Convert.ToDouble(dt["column11"].ToString()),
                                    0, 0, -1);

                                else
                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                 (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                 null,
                                 null,
                                  CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                 Convert.ToDouble(dt["column11"].ToString()),
                                 0, 0, -1);

                                //********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15,
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                            null,
                                            null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null), CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                             0,
                                             Convert.ToDouble(dt["column11"].ToString()), 0, -1);
                                else
                                    SourceTable.Rows.Add(15,
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        null,
                                        null,
                                        null,
                                        CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                         0,
                                         Convert.ToDouble(dt["column11"].ToString()), 0, -1);

                            }
                            else
                            {
                                //********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                    (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                    null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null), CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                    Convert.ToDouble(dt["Net"].ToString()),
                                    0, 0, -1);
                                else

                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                   (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                   null,
                                   null,
                                   CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                   Convert.ToDouble(dt["Net"].ToString()),
                                   0, 0, -1);

                                //********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15,
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        null,
                                        null, ((dt["Project"] != DBNull.Value && dt["Project"] != null && !string.IsNullOrWhiteSpace(dt["Project"].ToString())) ? dt["Project"] : null),
                                        CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                         0,
                                         Convert.ToDouble(dt["Net"].ToString()), 0, -1);
                                else
                                    SourceTable.Rows.Add(15,
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        null, null,
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


                                //*********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(15,
                                        (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                       (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                       SaleRow["Column03"].ToString(),
                                       null,
                                       ((item["Project"] != DBNull.Value && item["Project"] != null && !string.IsNullOrWhiteSpace(item["Project"].ToString())) ? item["Project"] : null),
                                         CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                       Convert.ToDouble(item["Total"].ToString()),
                                       0, 0, -1);
                                else
                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                 (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                 null,
                                 null,
                                   CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                 Convert.ToDouble(item["Total"].ToString()),
                                 0, 0, -1);

                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15,
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        null, null,
                                        ((item["Project"] != DBNull.Value && item["Project"] != null && !string.IsNullOrWhiteSpace(item["Project"].ToString())) ? item["Project"] : null),
                                        CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])), 0,
                                        Convert.ToDouble(item["Total"].ToString()),
                                         Convert.ToDouble(0),
                                             -1);
                                else
                                    SourceTable.Rows.Add(15,
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                       null, null, null,
                                       CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])), 0,
                                       Convert.ToDouble(item["Total"].ToString()),
                                        Convert.ToDouble(0),
                                            -1);

                            }
                            else
                            {


                                //*********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                       (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                       null,
                                       ((item["Project"] != DBNull.Value && item["Project"] != null && !string.IsNullOrWhiteSpace(item["Project"].ToString())) ? item["Project"] : null),
                                         CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                       Convert.ToDouble(item["Net"].ToString()),
                                       0, 0, -1);

                                else
                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                  (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                  null,
                                  null,
                                    CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                  Convert.ToDouble(item["Net"].ToString()),
                                  0, 0, -1);

                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15,
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        null, null,
                                        ((item["Project"] != DBNull.Value && item["Project"] != null && !string.IsNullOrWhiteSpace(item["Project"].ToString())) ? item["Project"] : null),
                                        CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])), 0,
                                        Convert.ToDouble(item["Net"].ToString()),
                                         Convert.ToDouble(0),
                                             -1);
                                else
                                    SourceTable.Rows.Add(15,
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                  null, null, null,
                                  CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])), 0,
                                  Convert.ToDouble(item["Net"].ToString()),
                                   Convert.ToDouble(0),
                                       -1);

                            }
                        }
                    }

                    // مربوط به اضافات خطی فاکتور
                    if (Convert.ToDouble(SaleRow["Column34"].ToString()) != 0 && !chk_Net.Checked)
                    {


                        DataTable ezafeTable = clDoc.ReturnTable(ConSale.ConnectionString,
                   "Select SUM(column19) as ezafe, column22 from Table_011_Child1_SaleFactor where column01=" + SaleRow["ColumnId"].ToString() + "  group by column22");
                        foreach (DataRow d in ezafeTable.Rows)
                        {
                            if (Convert.ToDouble(d["ezafe"].ToString()) != 0)
                            {

                                //********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(15, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                        (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                        null, ((d["column22"] != DBNull.Value && d["column22"] != null && !string.IsNullOrWhiteSpace(d["column22"].ToString())) ? d["column22"].ToString() : null), (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور فروش ش " : "تخفیف خطی2 فاکتور فروش ش ") +
                                        SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                        Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                                else

                                    SourceTable.Rows.Add(15, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                  (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                  null,
                                  null,
                                  (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور فروش ش " : "تخفیف خطی2 فاکتور فروش ش ") +
                                  SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                  Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);

                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                        (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null), null, null, ((d["column22"] != DBNull.Value && d["column22"] != null && !string.IsNullOrWhiteSpace(d["column22"].ToString())) ? d["column22"].ToString() : null),
                                        (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور فروش ش " : "تخفیف خطی2 فاکتور فروش ش ")
                                        + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                        0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                                else
                                    SourceTable.Rows.Add(15, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                   (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null), null, null, null,
                                   (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور فروش ش " : "تخفیف خطی2 فاکتور فروش ش ")
                                   + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                   0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);

                            }
                        }



                    }

                    //ثبت مربوط به تخفیفات خطی فاکتور
                    if (Convert.ToDouble(SaleRow["Column35"].ToString()) > 0 && !chk_Net.Checked)
                    {
                        DataTable takhfifTable = clDoc.ReturnTable(ConSale.ConnectionString,
                            "Select SUM(column17) as takhfif, column22 from Table_011_Child1_SaleFactor where column01=" + SaleRow["ColumnId"].ToString() + "  group by column22");


                        foreach (DataRow h in takhfifTable.Rows)
                        {

                            if (Convert.ToInt64(h["takhfif"]) > 0)
                            {
                                //********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(15, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), null, null, ((h["column22"] != DBNull.Value && h["column22"] != null && !string.IsNullOrWhiteSpace(h["column22"].ToString())) ? h["column22"].ToString() : null), "تخفیف خطی فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);
                                else
                                    SourceTable.Rows.Add(15, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), null, null, null,
                                        "تخفیف خطی فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);


                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, ((h["column22"] != DBNull.Value && h["column22"] != null && !string.IsNullOrWhiteSpace(h["column22"].ToString())) ? h["column22"].ToString() : null), "تخفیف خطی فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                                else
                                    SourceTable.Rows.Add(15, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null,
                                    "تخفیف خطی فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);


                            }
                        }


                    }
                    //ثبت مربوط به تخفیفات انتهای فاکتور

                    //تخفیف حجمی گروه مشتری
                    if (Convert.ToDouble(SaleRow["Column29"].ToString()) > 0)
                    {
                        //********Bed
                        SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف حجمی گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column29"].ToString()), 0, 0, -1);
                        //*********Bes
                        SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف حجمی گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column29"].ToString()), 0, -1);
                    }
                    //تخفیف ویژه گروه مشتری
                    if (Convert.ToDouble(SaleRow["Column30"].ToString()) > 0)
                    {
                        //********Bed
                        SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف ویژه گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column30"].ToString()), 0, 0, -1);
                        //*********Bes
                        SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف ویژه گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column30"].ToString()), 0, -1);
                    }
                    //تخفیف ویژه مشتری
                    if (Convert.ToDouble(SaleRow["Column31"].ToString()) > 0)
                    {
                        //********Bed
                        SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف ویژه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column31"].ToString()), 0, 0, -1);
                        //*********Bes
                        SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف ویژه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column31"].ToString()), 0, -1);
                    }


                    //سایر اضافات و کسورات
                    foreach (DataRowView item in Child2Bind)
                    {
                        string Bed = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column10", "ColumnId", item["Column02"].ToString());
                        string Bes = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column16", "ColumnId", item["Column02"].ToString());
                        string Name = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column01", "ColumnId", item["Column02"].ToString());

                        //********Bed
                        SourceTable.Rows.Add(15, Bed, Bed, (item["Column05"].ToString() == "False" ? (item["column07"] != null && !string.IsNullOrWhiteSpace(item["column07"].ToString()) ? item["column07"].ToString() : SaleRow["Column03"]) : null), null, null, Name + " فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(item["Column04"].ToString()), 0, (SaleRow["Column41"].ToString().Trim() == "" ? "0" : SaleRow["Column41"].ToString()), (SaleRow["Column40"].ToString().Trim() == "" ? "-1" : SaleRow["Column40"].ToString()));
                        //*********Bes
                        SourceTable.Rows.Add(15, Bes, Bes, (item["Column05"].ToString() == "True" ? (item["column07"] != null && !string.IsNullOrWhiteSpace(item["column07"].ToString()) ? item["column07"].ToString() : SaleRow["Column03"]) : null), null, null, Name + " فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(item["Column04"].ToString()), (SaleRow["Column41"].ToString().Trim() == "" ? "0" : SaleRow["Column41"].ToString()), (SaleRow["Column40"].ToString().Trim() == "" ? "-1" : SaleRow["Column40"].ToString()));
                    }
                    //صدور سند ارزش حواله 
                    if (uiTab3.Enabled)
                    {
                        int HavaleID = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", SaleRow["ColumnId"].ToString()));
                        double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_008_Child_PwhrsDraft", "ISNULL(SUM(Column16),0)", "Column01", HavaleID.ToString()));


                        if (HavaleID > 0)
                        {
                            newDraft = false;

                            DataTable baha = new DataTable();
                            Adapter = new SqlDataAdapter("SELECT column14,ISNULL(SUM(Column16),0) as Column16  from Table_008_Child_PwhrsDraft Where Column01=" + HavaleID + " Group by column14 ", ConWare);
                            Adapter.Fill(baha);

                            foreach (DataRow row in baha.Rows)
                            {
                                if (Convert.ToDouble(row["Column16"]) > 0)
                                {
                                    if (row["column14"] != DBNull.Value && row["column14"] != null && !string.IsNullOrWhiteSpace(row["column14"].ToString()))
                                    {
                                        //********Bed
                                        if (chk_PCBed.Checked)

                                            SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);

                                        else
                                            SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);

                                        //*********Bes
                                        if (chk_PCBes.Checked)

                                            SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                                        else
                                            SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);


                                    }
                                    else
                                    {
                                        //********Bed
                                        SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);
                                        //*********Bes
                                        SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                                    }
                                }
                                else
                                {
                                    if (row["column14"] != DBNull.Value && row["column14"] != null && !string.IsNullOrWhiteSpace(row["column14"].ToString()))
                                    {
                                        //********Bed
                                        if (chk_PCBed.Checked)

                                            SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);
                                        else
                                            SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);

                                        //*********Bes
                                        if (chk_PCBes.Checked)

                                            SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                                        else
                                            SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);


                                    }
                                    else
                                    {
                                        //********Bed
                                        SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);
                                        //*********Bes
                                        SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                                    }
                                }

                            }
                        }
                        else
                        {
                            newDraft = true;
                            DataTable baha = new DataTable();
                            Adapter = new SqlDataAdapter("SELECT column22 as column14,ISNULL(SUM(column20),0) as Column16  from Table_011_Child1_SaleFactor Where Column01=" + SaleRow["columnid"].ToString() + " Group by column22 ", ConSale);
                            Adapter.Fill(baha);
                            foreach (DataRow row in baha.Rows)
                            {

                                if (row["column14"] != DBNull.Value && row["column14"] != null && !string.IsNullOrWhiteSpace(row["column14"].ToString()))
                                {
                                    //********Bed
                                    if (chk_PCBed.Checked)
                                        SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 1);
                                    else
                                        SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 1);

                                    //*********Bes
                                    if (chk_PCBes.Checked)

                                        SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, Convert.ToInt16(row["column14"]), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 0);
                                    else
                                        SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 0);

                                }
                                else
                                {
                                    //********Bed
                                    SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 1);
                                    //*********Bes
                                    SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 0);
                                }
                            }
                        }




                    }
                }
                #endregion

                #region کالاها پروژه ندارند
                else//کالاها پروژه ندارد
                {
                    Adapter = new SqlDataAdapter(@" SELECT Column44 FROM  Table_010_SaleFactor
                                                    WHERE columnid=  " + SaleRow["ColumnId"].ToString(), ConSale);
                    Project = new DataTable();
                    Adapter.Fill(Project);


                    if (chk_GoodACCNum.Checked)
                        Adapter = new SqlDataAdapter(@"SELECT       Total, Discount, Adding, column01, Total - Discount + Adding AS Net,TotalValue,column33
                             FROM         (SELECT       ISNULL(SUM(dbo.Table_011_Child1_SaleFactor.column11), 0) AS Total, 
                             ISNULL(SUM(dbo.Table_011_Child1_SaleFactor.column17), 0) AS Discount, ISNULL(SUM(dbo.Table_011_Child1_SaleFactor.column19), 0) AS Adding, dbo.Table_011_Child1_SaleFactor.column01,
                               ISNULL(Sum(dbo.Table_011_Child1_SaleFactor.column07),0) as TotalValue ,tcai.column33
                             FROM          dbo.Table_011_Child1_SaleFactor
                                    JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = dbo.Table_011_Child1_SaleFactor.column02
                             GROUP BY   dbo.Table_011_Child1_SaleFactor.column01,tcai.column33
                             HAVING      (dbo.Table_011_Child1_SaleFactor.column01 = {0})) AS derivedtbl_1", ConSale);
                    else

                        Adapter = new SqlDataAdapter(@"SELECT       Total, Discount, Adding, column01, Total - Discount + Adding AS Net,TotalValue,column33
                             FROM         (SELECT       ISNULL(SUM(column11), 0) AS Total, 
                             ISNULL(SUM(column17), 0) AS Discount, ISNULL(SUM(column19), 0) AS Adding, column01,
                               ISNULL(Sum(column07),0) as TotalValue ,null as column33
                             FROM          dbo.Table_011_Child1_SaleFactor
                                    
                             GROUP BY   column01 
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);

                    Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, SaleRow["ColumnId"].ToString());
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);





                    // فاکتور فروش بدون احتساب تخفیفات و اضافات خطی


                    DataTable detali = new DataTable();
                    if (chk_GoodACCNum.Checked)
                        Adapter = new SqlDataAdapter(@"SELECT  tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02,tcsf.column22 as Project,tcai.column33
                                            FROM   Table_011_Child1_SaleFactor tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + SaleRow["ColumnId"].ToString() + "  ", ConSale);
                    else


                        Adapter = new SqlDataAdapter(@"SELECT  tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02,tcsf.column22 as Project,   null as column33
                                            FROM   Table_011_Child1_SaleFactor tcsf
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
                                //********Bed

                                if (chk_PCBed.Checked)
                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                    (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                    null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null),
                                    CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                    Convert.ToDouble(dt["column11"].ToString()),
                                    0, 0, -1);
                                else
                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                    (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                    null,
                                    null,
                                    CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                    Convert.ToDouble(dt["column11"].ToString()),
                                    0, 0, -1);

                                //********Bes

                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15,
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        null,
                                        null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                        , CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                         0,
                                         Convert.ToDouble(dt["column11"].ToString()), 0, -1);
                                else
                                    SourceTable.Rows.Add(15,
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                    null,
                                    null,
                                    null,
                                      CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                     0,
                                     Convert.ToDouble(dt["column11"].ToString()), 0, -1);

                            }
                            else
                            {
                                //********Bed

                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                   (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                   null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null),
                                   CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                   Convert.ToDouble(dt["Net"].ToString()),
                                   0, 0, -1);

                                else
                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                    (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                    null,
                                    null,
                                    CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                    Convert.ToDouble(dt["Net"].ToString()),
                                    0, 0, -1);
                                //********Bes

                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15,
                                         ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                            null,
                                            null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                            , CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                                             0,
                                             Convert.ToDouble(dt["Net"].ToString()), 0, -1);
                                else

                                    SourceTable.Rows.Add(15,
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((dt["column33"] != null && !string.IsNullOrWhiteSpace(dt["column33"].ToString())) ? dt["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                    null,
                                    null,
                                    null,
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
                                //*********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                       (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                       null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                       , CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                         Convert.ToDouble(item["Total"].ToString()),
                                       0, 0, -1);

                                else
                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                      (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                      null,
                                      null,
                                        CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                        Convert.ToDouble(item["Total"].ToString()),
                                      0, 0, -1);
                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15,
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),

                                        null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                        , CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                        0,
                                        Convert.ToDouble(item["Total"].ToString()),
                                         0,
                                             -1);
                                else
                                    SourceTable.Rows.Add(15,
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),

                                   null, null, null
                                   , CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                   0,
                                   Convert.ToDouble(item["Total"].ToString()),
                                    0,
                                        -1);


                            }
                            else
                            {



                                //*********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                       (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                       null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                       , CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                         Convert.ToDouble(item["Net"].ToString()),
                                       0, 0, -1);
                                else

                                    SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                                  (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                  null, null
                                  , CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                    Convert.ToDouble(item["Net"].ToString()),
                                  0, 0, -1);

                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15,
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),

                                        null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                        , CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                        0,
                                        Convert.ToDouble(item["Net"].ToString()),
                                         0,
                                             -1);
                                else
                                    SourceTable.Rows.Add(15,
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                                        ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),

                                    null, null, null
                                    , CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                                    0,
                                    Convert.ToDouble(item["Net"].ToString()),
                                     0,
                                         -1);

                            }
                        }
                    }

                    // مربوط به اضافات خطی فاکتور
                    if (Convert.ToDouble(SaleRow["Column34"].ToString()) != 0 && !chk_Net.Checked)
                    {


                        DataTable ezafeTable = clDoc.ReturnTable(ConSale.ConnectionString,
                   "Select SUM(column19) as ezafe  from Table_011_Child1_SaleFactor where column01=" + SaleRow["ColumnId"].ToString() + "   ");
                        foreach (DataRow d in ezafeTable.Rows)
                        {
                            if (Convert.ToDouble(d["ezafe"].ToString()) != 0)
                            {
                                //********Bed

                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(15, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                    (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                    null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null)
                                    , (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور فروش ش " : "تخفیف خطی2 فاکتور فروش ش ") +
                                    SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                    Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                                else

                                    SourceTable.Rows.Add(15, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                                     (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                                     null, null
                                     , (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور فروش ش " : "تخفیف خطی2 فاکتور فروش ش ") +
                                     SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                     Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, 0, -1);
                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                     (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null), null, null,
                                     ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null),
                                     (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور فروش ش " : "تخفیف خطی2 فاکتور فروش ش ")
                                     + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                     0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);
                                else

                                    SourceTable.Rows.Add(15, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                                     (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null), null, null, null,
                                     (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور فروش ش " : "تخفیف خطی2 فاکتور فروش ش ")
                                     + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                                     0, Math.Abs(Convert.ToDouble(d["ezafe"].ToString())), 0, -1);

                            }
                        }



                    }

                    //ثبت مربوط به تخفیفات خطی فاکتور
                    if (Convert.ToDouble(SaleRow["Column35"].ToString()) > 0 && !chk_Net.Checked)
                    {
                        DataTable takhfifTable = clDoc.ReturnTable(ConSale.ConnectionString,
                            "Select SUM(column17) as takhfif from Table_011_Child1_SaleFactor where column01=" + SaleRow["ColumnId"].ToString() + " ");


                        foreach (DataRow h in takhfifTable.Rows)
                        {

                            if (Convert.ToInt64(h["takhfif"]) > 0)
                            {
                                //********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(15, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف خطی فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);

                                else
                                    SourceTable.Rows.Add(15, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), null, null, null,
                                        "تخفیف خطی فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(h["takhfif"].ToString()), 0, 0, -1);

                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(15, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null,
                                       ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف خطی فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);
                                else
                                    SourceTable.Rows.Add(15, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null,
                                  "تخفیف خطی فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(h["takhfif"].ToString()), 0, -1);


                            }
                        }


                    }
                    //ثبت مربوط به تخفیفات انتهای فاکتور

                    //تخفیف حجمی گروه مشتری
                    if (Convert.ToDouble(SaleRow["Column29"].ToString()) > 0)
                    {
                        //********Bed
                        if (chk_PCBed.Checked)

                            SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف حجمی گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column29"].ToString()), 0, 0, -1);
                        else

                            SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null,
                                "تخفیف حجمی گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column29"].ToString()), 0, 0, -1);

                        //*********Bes
                        if (chk_PCBes.Checked)

                            SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف حجمی گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column29"].ToString()), 0, -1);
                        else
                            SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null,
                                "تخفیف حجمی گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column29"].ToString()), 0, -1);


                    }
                    //تخفیف ویژه گروه مشتری
                    if (Convert.ToDouble(SaleRow["Column30"].ToString()) > 0)
                    {
                        //********Bed
                        if (chk_PCBed.Checked)

                            SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف ویژه گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column30"].ToString()), 0, 0, -1);
                        else
                            SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null,
                                "تخفیف ویژه گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column30"].ToString()), 0, 0, -1);

                        //*********Bes
                        if (chk_PCBes.Checked)

                            SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف ویژه گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column30"].ToString()), 0, -1);
                        else
                            SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null,
                                "تخفیف ویژه گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column30"].ToString()), 0, -1);


                    }
                    //تخفیف ویژه مشتری
                    if (Convert.ToDouble(SaleRow["Column31"].ToString()) > 0)
                    {
                        //********Bed
                        if (chk_PCBed.Checked)

                            SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف ویژه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column31"].ToString()), 0, 0, -1);
                        else
                            SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null,
                                "تخفیف ویژه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column31"].ToString()), 0, 0, -1);


                        //*********Bes
                        if (chk_PCBes.Checked)

                            SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "تخفیف ویژه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column31"].ToString()), 0, -1);
                        else
                            SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null,
                                "تخفیف ویژه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column31"].ToString()), 0, -1);


                    }


                    //سایر اضافات و کسورات
                    foreach (DataRowView item in Child2Bind)
                    {
                        string Bed = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column10", "ColumnId", item["Column02"].ToString());
                        string Bes = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column16", "ColumnId", item["Column02"].ToString());
                        string Name = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column01", "ColumnId", item["Column02"].ToString());

                        //********Bed
                        if (chk_PCBed.Checked)

                            SourceTable.Rows.Add(15, Bed, Bed, (item["Column05"].ToString() == "False" ? (item["column07"] != null && !string.IsNullOrWhiteSpace(item["column07"].ToString()) ? item["column07"].ToString() : SaleRow["Column03"]) : null), null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), Name + " فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(item["Column04"].ToString()), 0, (SaleRow["Column41"].ToString().Trim() == "" ? "0" : SaleRow["Column41"].ToString()), (SaleRow["Column40"].ToString().Trim() == "" ? "-1" : SaleRow["Column40"].ToString()));
                        else
                            SourceTable.Rows.Add(15, Bed, Bed, (item["Column05"].ToString() == "False" ? (item["column07"] != null && !string.IsNullOrWhiteSpace(item["column07"].ToString()) ? item["column07"].ToString() : SaleRow["Column03"]) : null), null, null,
                                Name + " فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(item["Column04"].ToString()), 0, (SaleRow["Column41"].ToString().Trim() == "" ? "0" : SaleRow["Column41"].ToString()), (SaleRow["Column40"].ToString().Trim() == "" ? "-1" : SaleRow["Column40"].ToString()));

                        //*********Bes
                        if (chk_PCBes.Checked)

                            SourceTable.Rows.Add(15, Bes, Bes, (item["Column05"].ToString() == "True" ? (item["column07"] != null && !string.IsNullOrWhiteSpace(item["column07"].ToString()) ? item["column07"].ToString() : SaleRow["Column03"]) : null), null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), Name + " فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(item["Column04"].ToString()), (SaleRow["Column41"].ToString().Trim() == "" ? "0" : SaleRow["Column41"].ToString()), (SaleRow["Column40"].ToString().Trim() == "" ? "-1" : SaleRow["Column40"].ToString()));
                        else
                            SourceTable.Rows.Add(15, Bes, Bes, (item["Column05"].ToString() == "True" ? (item["column07"] != null && !string.IsNullOrWhiteSpace(item["column07"].ToString()) ? item["column07"].ToString() : SaleRow["Column03"]) : null), null, null,
                                Name + " فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(item["Column04"].ToString()), (SaleRow["Column41"].ToString().Trim() == "" ? "0" : SaleRow["Column41"].ToString()), (SaleRow["Column40"].ToString().Trim() == "" ? "-1" : SaleRow["Column40"].ToString()));

                    }

                    //صدور سند ارزش حواله 
                    if (uiTab3.Enabled)
                    {
                        int HavaleID = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", SaleRow["ColumnId"].ToString()));
                        double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_008_Child_PwhrsDraft", "ISNULL(SUM(Column16),0)", "Column01", HavaleID.ToString()));


                        if (HavaleID > 0)
                        {
                            newDraft = false;

                            DataTable baha = new DataTable();
                            Adapter = new SqlDataAdapter("SELECT ISNULL(SUM(Column16),0) as Column16  from Table_008_Child_PwhrsDraft Where Column01=" + HavaleID + "  ", ConWare);
                            Adapter.Fill(baha);

                            foreach (DataRow row in baha.Rows)
                            {
                                if (Convert.ToDouble(row["Column16"]) > 0)
                                {

                                    {
                                        //********Bed
                                        if (chk_PCBed.Checked)

                                            SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);
                                        else
                                            SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null,
                                                "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);

                                        //*********Bes
                                        if (chk_PCBes.Checked)

                                            SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                                        else
                                            SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null,
                                                "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);

                                    }
                                }
                                else
                                {

                                    {
                                        //********Bed
                                        if (chk_PCBed.Checked)

                                            SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);
                                        else
                                            SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null,
                                                "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(row["Column16"]), 0, 0, -1, 1);

                                        //*********Bes
                                        if (chk_PCBes.Checked)

                                            SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);
                                        else
                                            SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null,
                                                "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(row["Column16"]), 0, -1, 0);

                                    }
                                }

                            }
                        }
                        else
                        {
                            newDraft = true;
                            DataTable baha = new DataTable();
                            Adapter = new SqlDataAdapter("SELECT ISNULL(SUM(column20),0) as Column16  from Table_011_Child1_SaleFactor Where Column01=" + SaleRow["columnid"].ToString() + "  ", ConSale);
                            Adapter.Fill(baha);
                            foreach (DataRow row in baha.Rows)
                            {
                                //********Bed
                                if (chk_PCBed.Checked)

                                    SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 1);
                                else
                                    SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null,
                                        "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 1);


                                //*********Bes
                                if (chk_PCBes.Checked)

                                    SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, ((Project.Rows.Count > 0 && Project.Rows[0]["Column44"] != DBNull.Value && Project.Rows[0]["Column44"] != null && !string.IsNullOrWhiteSpace(Project.Rows[0]["Column44"].ToString())) ? Project.Rows[0]["Column44"] : null), "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 0);

                                else
                                    SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null,
                                        "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, 0, 0, -1, 0);


                            }
                        }

                    }

                }
                #endregion

            }
            # endregion

            #region عدم تفکیک پروژه
            else
            {


                SourceTable.Rows.Clear();
                SqlDataAdapter Adapter;
                if (chk_GoodACCNum.Checked)
                    Adapter = new SqlDataAdapter(@"SELECT      Total, Discount, Adding, column01, Total - Discount + Adding AS Net,TotalValue,column33
                             FROM         (SELECT       ISNULL(SUM(dbo.Table_011_Child1_SaleFactor.column11), 0) AS Total, 
                             ISNULL(SUM(dbo.Table_011_Child1_SaleFactor.column17), 0) AS Discount, ISNULL(SUM(dbo.Table_011_Child1_SaleFactor.column19), 0) AS Adding, dbo.Table_011_Child1_SaleFactor.column01,
                               ISNULL(Sum(dbo.Table_011_Child1_SaleFactor.column07),0) as TotalValue,tcai.column33
                             FROM          dbo.Table_011_Child1_SaleFactor
                                            JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = dbo.Table_011_Child1_SaleFactor.column02
                             GROUP BY   dbo.Table_011_Child1_SaleFactor.column01,tcai.column33 
                             HAVING      (dbo.Table_011_Child1_SaleFactor.column01 = {0})) AS derivedtbl_1", ConSale);
                else
                    Adapter = new SqlDataAdapter(@"SELECT      Total, Discount, Adding, column01, Total - Discount + Adding AS Net,TotalValue,column33
                             FROM         (SELECT       ISNULL(SUM(column11), 0) AS Total, 
                             ISNULL(SUM(column17), 0) AS Discount, ISNULL(SUM(column19), 0) AS Adding, column01,
                               ISNULL(Sum(column07),0) as TotalValue,null as column33
                             FROM          dbo.Table_011_Child1_SaleFactor
                             GROUP BY   column01 
                             HAVING      (column01 = {0})) AS derivedtbl_1", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, SaleRow["ColumnId"].ToString());
                DataTable Table = new DataTable();
                Adapter.Fill(Table);





                // فاکتور فروش بدون احتساب تخفیفات و اضافات خطی


                DataTable detali = new DataTable();
                if (chk_GoodACCNum.Checked)
                    Adapter = new SqlDataAdapter(@"SELECT tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02,tcai.column33
                                            FROM   Table_011_Child1_SaleFactor tcsf
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                            WHERE  tcsf.column01 = " + SaleRow["ColumnId"].ToString() + "", ConSale);

                else

                    Adapter = new SqlDataAdapter(@"SELECT tcsf.column11,tcsf.column07,tcsf.column10,tcsf.column11-tcsf.column17+tcsf.column19 as Net,
                                                   tcai.column02,null as column33
                                            FROM   Table_011_Child1_SaleFactor tcsf
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

                            SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                        (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                        null, null,
                        CerateSharh(Convert.ToDouble(dt["column07"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())) + sharhjoz,
                        Convert.ToDouble(dt["column11"].ToString()),
                        0, 0, -1);
                        else
                            SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
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

                        SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
                            (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                            null,
                            null,
                            CerateSharh(Convert.ToDouble(Table.Rows[0]["TotalValue"]), Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()), Convert.ToDouble(SaleRow["Column28"].ToString())),
                            Convert.ToDouble(SaleRow["Column28"].ToString()) + Convert.ToDouble(SaleRow["Column35"].ToString()) - Convert.ToDouble(SaleRow["Column34"].ToString()),
                            0, 0, -1);
                    else
                        SourceTable.Rows.Add(15, (mlt_SaleBed.Text.Trim() != "" ? mlt_SaleBed.Value.ToString() : null),
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
                        SourceTable.Rows.Add(15,
                            ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                            ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),

                            null, null, null,
                            CerateSharh(Convert.ToDouble(item["TotalValue"]), Convert.ToDouble(item["Total"]), Convert.ToDouble(item["Net"])),
                            0,
                            Convert.ToDouble(item["Total"].ToString()),
                             Convert.ToDouble(0),
                                 -1);
                    }
                    else
                    {
                        SourceTable.Rows.Add(15,
                           ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),
                            ((item["column33"] != null && !string.IsNullOrWhiteSpace(item["column33"].ToString())) ? item["column33"].ToString() : (mlt_SaleBes.Text.Trim() != "" ? mlt_SaleBes.Value.ToString() : null)),

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
                    SourceTable.Rows.Add(15, (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null),
                        (mlt_LinAddBed.Text.Trim() != "" ? mlt_LinAddBed.Value.ToString() : null), SaleRow["Column03"].ToString(),
                        null, null, (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور فروش ش " : "تخفیف خطی2 فاکتور فروش ش ") +
                        SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                        Math.Abs(Convert.ToDouble(SaleRow["Column34"].ToString())), 0, 0, -1);
                    //*********Bes
                    SourceTable.Rows.Add(15, (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null),
                        (mlt_LinAddBes.Text.Trim() != "" ? mlt_LinAddBes.Value.ToString() : null), null, null, null,
                        (Convert.ToDouble(SaleRow["Column34"].ToString()) > 0 ? "اضافه خطی فاکتور فروش ش " : "تخفیف خطی2 فاکتور فروش ش ")
                        + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(),
                        0, Math.Abs(Convert.ToDouble(SaleRow["Column34"].ToString())), 0, -1);



                }

                //ثبت مربوط به تخفیفات خطی فاکتور
                if (Convert.ToDouble(SaleRow["Column35"].ToString()) > 0 && !chk_Net.Checked)
                {


                    //********Bed
                    SourceTable.Rows.Add(15, (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), (mlt_LinDisBed.Text.Trim() != "" ? mlt_LinDisBed.Value.ToString() : null), null, null, null, "تخفیف خطی فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column35"].ToString()), 0, 0, -1);
                    //*********Bes
                    SourceTable.Rows.Add(15, (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), (mlt_LinDisBes.Text.Trim() != "" ? mlt_LinDisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف خطی فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column35"].ToString()), 0, -1);



                }
                //ثبت مربوط به تخفیفات انتهای فاکتور

                //تخفیف حجمی گروه مشتری
                if (Convert.ToDouble(SaleRow["Column29"].ToString()) > 0)
                {
                    //********Bed
                    SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف حجمی گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column29"].ToString()), 0, 0, -1);
                    //*********Bes
                    SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف حجمی گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column29"].ToString()), 0, -1);
                }
                //تخفیف ویژه گروه مشتری
                if (Convert.ToDouble(SaleRow["Column30"].ToString()) > 0)
                {
                    //********Bed
                    SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف ویژه گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column30"].ToString()), 0, 0, -1);
                    //*********Bes
                    SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف ویژه گروه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column30"].ToString()), 0, -1);
                }
                //تخفیف ویژه مشتری
                if (Convert.ToDouble(SaleRow["Column31"].ToString()) > 0)
                {
                    //********Bed
                    SourceTable.Rows.Add(15, (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), (mlt_DisBed.Text.Trim() != "" ? mlt_DisBed.Value.ToString() : null), null, null, null, "تخفیف ویژه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(SaleRow["Column31"].ToString()), 0, 0, -1);
                    //*********Bes
                    SourceTable.Rows.Add(15, (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), (mlt_DisBes.Text.Trim() != "" ? mlt_DisBes.Value.ToString() : null), SaleRow["Column03"].ToString(), null, null, "تخفیف ویژه مشتری- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(SaleRow["Column31"].ToString()), 0, -1);
                }


                //سایر اضافات و کسورات
                foreach (DataRowView item in Child2Bind)
                {
                    string Bed = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column10", "ColumnId", item["Column02"].ToString());
                    string Bes = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column16", "ColumnId", item["Column02"].ToString());
                    string Name = clDoc.ExScalar(ConSale.ConnectionString, "Table_024_Discount", "Column01", "ColumnId", item["Column02"].ToString());

                    //********Bed
                    SourceTable.Rows.Add(15, Bed, Bed, (item["Column05"].ToString() == "False" ? (item["column07"] != null && !string.IsNullOrWhiteSpace(item["column07"].ToString()) ? item["column07"].ToString() : SaleRow["Column03"]) : null), null, null, Name + " فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), Convert.ToDouble(item["Column04"].ToString()), 0, (SaleRow["Column41"].ToString().Trim() == "" ? "0" : SaleRow["Column41"].ToString()), (SaleRow["Column40"].ToString().Trim() == "" ? "-1" : SaleRow["Column40"].ToString()));
                    //*********Bes
                    SourceTable.Rows.Add(15, Bes, Bes, (item["Column05"].ToString() == "True" ? (item["column07"] != null && !string.IsNullOrWhiteSpace(item["column07"].ToString()) ? item["column07"].ToString() : SaleRow["Column03"]) : null), null, null, Name + " فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, Convert.ToDouble(item["Column04"].ToString()), (SaleRow["Column41"].ToString().Trim() == "" ? "0" : SaleRow["Column41"].ToString()), (SaleRow["Column40"].ToString().Trim() == "" ? "-1" : SaleRow["Column40"].ToString()));
                }
                ////صدور سند ارزش حواله 
                if (uiTab3.Enabled)
                {
                    int HavaleID = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", SaleRow["ColumnId"].ToString()));
                    double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_008_Child_PwhrsDraft", "ISNULL(SUM(Column16),0)", "Column01", HavaleID.ToString()));

                    if (TotalValue > 0)
                    {
                        //********Bed
                        SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), TotalValue, 0, 0, -1, 1);
                        //*********Bes
                        SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, TotalValue, 0, -1, 0);
                    }
                    else
                    {
                        //********Bed
                        SourceTable.Rows.Add(26, (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), (mlt_ValueBed.Text.Trim() != "" ? mlt_ValueBed.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), TotalValue, 0, 0, -1, 1);
                        //*********Bes
                        SourceTable.Rows.Add(26, (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), (mlt_ValueBes.Text.Trim() != "" ? mlt_ValueBes.Value.ToString() : null), null, null, null, "بهای تمام شده- فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString(), 0, TotalValue, 0, -1, 0);
                    }



                }
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
                            Description = " فاکتور فروش ش " + SaleRow["Column01"].ToString() + " به تاریخ " + SaleRow["Column02"].ToString();
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

                        if (Bed - Bes != 0 && item["Type"].ToString() == "15")
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
            string CommandTxt = "declare @Key int declare @DetialID int declare @HavaleID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)   ";

            try
            {
                gridEX1.UpdateData();
               
                CheckEssentialItems(sender, e);

                string Message = "آیا مایل به صدور سند حسابداری هستید؟";
                if (uiTab2.Enabled)
                    Message = "آیا مایل به صدور سند حسابداری و حواله انبار هستید؟";

                //if (chk_AggDoc.Checked)
                //{
                //    AggDoc();
                //}

                if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {

                    //ثبت حواله
                    if (uiTab2.Enabled)
                    {
                        if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", _SaleID.ToString()) != 0)
                            throw new Exception("برای این فاکتور حواله انبار صادر شده است");
                        else
                            ExportDraft();
                    }

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", _SaleID.ToString()) != 0)
                        throw new Exception("برای این فاکتور سند حسابداری صادر شده است");


                    this.Cursor = Cursors.WaitCursor;

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
                        CommandTxt += " set @DocNum=(Select Column00 from Table_060_SanadHead where ColumnId=" + int.Parse(this.txt_serial.Text.Trim()) + ")    SET @Key="+int.Parse(this.txt_serial.Text.Trim())+"";

                    //if (DocID > 0)
                    {
                        gridEX1.UpdateData();

                        CommandTxt += @" set @HavaleID=(select Column09 from " + ConSale.Database + ".dbo.Table_010_SaleFactor where  ColumnId=" + SaleRow["ColumnId"].ToString() + ")";
                        //int HavaleID = int.Parse(clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", SaleRow["ColumnId"].ToString()));
                        CommandTxt += @"set @TotalValue=isnull((select ISNULL(SUM(Column16),0) from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where Column01=@HavaleID),0)";
                        // double TotalValue = double.Parse(clDoc.ExScalar(ConWare.ConnectionString, "Table_008_Child_PwhrsDraft", "ISNULL(SUM(Column16),0)", "Column01", HavaleID.ToString()));

                        CommandTxt += "set @value=@TotalValue   ";
                        //double value = TotalValue;

                        DateTime BaseDate;
                        DateTime SecDate;
                        BaseDate = Convert.ToDateTime(FarsiLibrary.Utils.PersianDate.Parse(faDatePicker1.Text));
                        FarsiLibrary.Win.Controls.FADatePicker fa;
                        fa = new FarsiLibrary.Win.Controls.FADatePicker();
                        fa.Text = faDatePicker1.Text;
                        try
                        {
                            SecDate = BaseDate.AddDays(double.Parse(SaleRow["column24"].ToString()));
                            fa.SelectedDateTime = SecDate;
                            fa.UpdateTextValue();
                        }
                        catch
                        {
                        }

                        foreach (GridEXRow item in gridEX1.GetRows())
                        {
                            string[] _AccInfo = clDoc.ACC_Info(item.Cells["Column01"].Value.ToString());

                            if (item.Cells["Type"].Value.ToString() == "15" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0 || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                            {

                                CommandTxt += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@Key,'" + item.Cells["Column01"].Value.ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column07"].Text.Trim()) ? "NULL" : item.Cells["Column07"].Value.ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item.Cells["Column08"].Text.Trim()) ? "NULL" : item.Cells["Column08"].Value.ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item.Cells["Column09"].Text.Trim()) ? "NULL" : item.Cells["Column09"].Value.ToString()) + @",
                   " + "'" + item.Cells["Column10"].Text.Trim() + "'," + (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0) + @",
                        " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0) + ",0,0,-1,15," + int.Parse(SaleRow["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0," + (SaleRow["column24"] != null && !string.IsNullOrWhiteSpace(SaleRow["column24"].ToString()) ? SaleRow["column24"] : "0") + ",N'" + fa.Text + "'); SET @DetialID=SCOPE_IDENTITY()";
                                //int DetialID = clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                //     , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                //     (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(),
                                //     (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0),
                                //     (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0), 0, 0, -1,
                                //        15, int.Parse(SaleRow["ColumnId"].ToString()), Class_BasicOperation._UserName, 0);
                                // اضافه کردن اقلام کالا به آرتیکل بدهکار فاکتور فروش
                                if (item.RowIndex == 0 && this.chk_Nots.Checked)
                                {
                                    foreach (DataRowView items in Child1Bind)
                                    {
                                        CommandTxt += @"INSERT INTO Table_075_SanadDetailNotes (Column00,Column01,Column02,Column03,Column04) Values (@DetialID,1,(select Column02 from " + ConWare.Database + ".dbo.table_004_CommodityAndIngredients where ColumnId=" + items["Column02"].ToString() + " ) ," +
                                            items["Column07"].ToString() + "," + items["Column10"].ToString() + ")";
                                        //clDoc.RunSqlCommand(ConAcnt.ConnectionString, "INSERT INTO Table_075_SanadDetailNotes (Column00,Column01,Column02,Column03,Column04) Values (" + DetialID + ",1,'" +
                                        //    clDoc.ExScalar(ConWare.ConnectionString, "table_004_CommodityAndIngredients", "Column02", "ColumnId", items["Column02"].ToString()) + "'," +
                                        //    items["Column07"].ToString() + "," + items["Column10"].ToString() + ")");
                                    }
                                }
                            }
                            else if (item.Cells["Type"].Value.ToString() == "26" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) > 0 || Convert.ToDouble(item.Cells["Column12"].Value.ToString()) > 0))
                            {


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
                   " + "'" + item.Cells["Column10"].Text.Trim() + "'," + (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0) + @",
                        " + (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0) + ",0,0,-1,26,@HavaleID ,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                 Class_BasicOperation._UserName + "',getdate(),0," + (SaleRow["column24"] != null && !string.IsNullOrWhiteSpace(SaleRow["column24"].ToString()) ? SaleRow["column24"] : "0") + ",N'" + fa.Text + "');  ";



                                //clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                //    , (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                //    (item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(),
                                //    (item.Cells["Column12"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column11"].Value.ToString())) : 0),
                                //    (item.Cells["Column11"].Text == "0" ? Convert.ToInt64(Convert.ToDouble(item.Cells["Column12"].Value.ToString())) : 0), 0, 0, -1,
                                //       26, HavaleID, Class_BasicOperation._UserName, 0);
                            }
                            else if (item.Cells["Type"].Value.ToString() == "26" && (Convert.ToDouble(item.Cells["Column11"].Value.ToString()) == 0 && Convert.ToDouble(item.Cells["Column12"].Value.ToString()) == 0))
                            {

                                SqlDataAdapter Adapter = new SqlDataAdapter(@" SELECT * FROM  [Table_011_Child1_SaleFactor]
                                                    WHERE column01=  " + SaleRow["ColumnId"].ToString() + " AND [column22] is NOT NULL", ConSale);
                                DataTable Project = new DataTable();
                                Adapter.Fill(Project);///کالاها پروژه دارند
                                if (Project.Rows.Count > 0 && chk_Baha.Checked)
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
                                @TotalValue ,0,0,0,-1,26,@HavaleID  ,'" + Class_BasicOperation._UserName + "',getdate(),'" +
             Class_BasicOperation._UserName + "',getdate(),0," + (SaleRow["column24"] != null && !string.IsNullOrWhiteSpace(SaleRow["column24"].ToString()) ? SaleRow["column24"] : "0") + ",N'" + fa.Text + "'); set @value = 0 end ";
                                }
                                else
                                {
                                    CommandTxt += @" if @TotalValue>0 begin  INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
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
                                0 ,@TotalValue,0,0,-1,26,@HavaleID  ,'" + Class_BasicOperation._UserName + "',getdate(),'" +
            Class_BasicOperation._UserName + "',getdate(),0," + (SaleRow["column24"] != null && !string.IsNullOrWhiteSpace(SaleRow["column24"].ToString()) ? SaleRow["column24"] : "0") + ",N'" + fa.Text + "'); set @value = 0 end ";
                                }
                                // clDoc.ExportDoc_Detail(DocID, item.Cells["Column01"].Value.ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
                                //, (item.Cells["Column07"].Text.Trim() == "" ? "NULL" : item.Cells["Column07"].Value.ToString()), (item.Cells["Column08"].Text.Trim() == "" ? "NULL" : item.Cells["Column08"].Value.ToString()),
                                //(item.Cells["Column09"].Text.Trim() == "" ? "NULL" : item.Cells["Column09"].Value.ToString()), item.Cells["Column10"].Text.Trim(),
                                //Convert.ToInt64(value), (value > 0 ? 0 : Convert.ToInt64(TotalValue)), 0, 0, -1,
                                //   26, HavaleID, Class_BasicOperation._UserName, 0);
                                // value = 0;
                                //  }
                            }

                        }



                        CommandTxt += @"Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07= @Key  where ColumnId =@HavaleID    ";
                        CommandTxt += @"Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column10= @Key,Column15='" + Class_BasicOperation._UserName + "',Column16=getdate()  where ColumnId =" + int.Parse(SaleRow["ColumnId"].ToString());

                        //clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column07", "ColumnId", HavaleID, DocID);
                        //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column10", "ColumnId", int.Parse(SaleRow["ColumnId"].ToString()), DocID);
                        //using (SqlConnection Consale = new SqlConnection(Properties.Settings.Default.SALE))
                        //{
                        //    Consale.Open();
                        //    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_010_SaleFactor SET  Column15='" + Class_BasicOperation._UserName
                        //            + "', Column16=getdate() where ColumnId=" + int.Parse(SaleRow["ColumnId"].ToString()), Consale);
                        //    UpdateCommand.ExecuteNonQuery();
                        //    Consale.Close();
                        //}




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
                                    "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره حواله انبار: " + DraftNum.Value, "Information");

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
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default; this.Cursor = Cursors.Default;
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
                mlt_ToSerial_Leave(sender, e);
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


            if (SaleRow["Column12"].ToString() == "True" && Class_BasicOperation._FinType)
            {
                if (mlt_CurrencyType.Text.Trim() == "" || txt_CurrencyValue.Text.Trim() == "")
                    throw new Exception("اطلاعات مربوط به ارز را کامل کنید");
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

        private void ExportDraft()
        {
            string command = string.Empty;



            if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", _SaleID.ToString()) != 0)
                throw new Exception("برای این فاکتور حواله انبار صادر شده است");
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
                } if (ok)
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
          + "," + mlt_Function.Value.ToString() + "," + SaleRow["Column03"].ToString() + ",'" +
          "حواله صادره بابت فاکتور فروش ش" + SaleRow["Column01"].ToString() +
          "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" +
          Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," +
          SaleRow["ColumnId"].ToString() + ",0," +
          (SaleRow["Column07"].ToString().Trim() == "" ? "0" : SaleRow["Column07"].ToString()) +
          ",0,0,0,0," +
          (SaleRow["Column12"].ToString() == "True" ? "1" : "0") + "," +
          (SaleRow["Column40"].ToString().Trim() == "" ? "Null" : SaleRow["Column40"].ToString()) + "," +
          SaleRow["Column41"].ToString()
          + ",1); SET @Key=SCOPE_IDENTITY()";

                    else
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
                                                                                               ,[Column26]) VALUES(@DraftNum,'" + SaleRow["Column02"].ToString() + "'," + mlt_Ware.Value.ToString()
                                + "," + mlt_Function.Value.ToString() + "," + SaleRow["Column03"].ToString() + ",'" +
                                "حواله صادره بابت فاکتور فروش ش" + SaleRow["Column01"].ToString() +
                                "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," +
                                SaleRow["ColumnId"].ToString() + ",0," +
                                (SaleRow["Column07"].ToString().Trim() == "" ? "0" : SaleRow["Column07"].ToString()) +
                                ",0,0,0,0," +
                                (SaleRow["Column12"].ToString() == "True" ? "1" : "0") + "," +
                                (SaleRow["Column40"].ToString().Trim() == "" ? "Null" : SaleRow["Column40"].ToString()) + "," +
                                SaleRow["Column41"].ToString()
                                + ",1); SET @Key=SCOPE_IDENTITY()";





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
                                item["Column15"].ToString() + "," + item["ColumnId"].ToString() +
                                    ",0,0,0," + (item["Column30"].ToString().Trim() != "" && item["Column30"].ToString() == "True" ? "1" : "0")
                                    + "," + (item["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item["Column34"].ToString().Trim() + "'")
                                + "," + (item["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item["Column35"].ToString().Trim() + "'") +
                                "," + item["Column31"].ToString() + "," + item["Column32"].ToString() + "," +
                                item["Column36"].ToString() + "," + item["Column37"].ToString() + "," + (item["Column38"].ToString().Trim() == "" ? "NULL" : "'" + item["Column38"].ToString() + "'") + "," + (item["Column39"].ToString().Trim() == "" ? "NULL" : "'" + item["Column39"].ToString() + "'") + ")";

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
                            try
                            {
                                string DraftID = clDoc.ExScalar(ConWare.ConnectionString, "Table_007_PwhrsDraft", "columnid", "column01", DraftNum.Value.ToString());
                                SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + DraftID, ConWare);
                                DataTable Table = new DataTable();
                                goodAdapter.Fill(Table);

                                //محاسبه ارزش و ذخیره آن در جدول Child1

                                foreach (DataRow item in Table.Rows)
                                {
                                    if (Class_BasicOperation._WareType)
                                    {
                                        SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	" + (Class_BasicOperation._WareType ? " [dbo].[PR_00_NewFIFO] " : "  [dbo].[PR_05_AVG] ") + " @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value.ToString(), ConWare);
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
                            }
                            catch
                            {
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
                CommandText = string.Format(CommandText, mlt_Ware.Value.ToString(), GoodCode, SaleRow["Column02"].ToString());
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
            //if (chk_AggDoc.Checked)
            //    AggDoc();

        }

        private string CerateSharh(double TotalValue, double TotalAmount, double TotalNetPrice)
        {

            string Sharh = string.Empty;
            try
            {
                if (Convert.ToBoolean(setting.Rows[3]["Column02"]))
                    Sharh = "فاکتور فروش ش " + SaleRow["Column01"].ToString();
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


                if (Convert.ToBoolean(setting.Rows[14]["Column02"]))
                {
                    string Ware = string.Empty;

                    using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        ConWare.Open();
                        string CommandText = @"SELECT     Column02 From Table_001_PWHRS WHERE ColumnId=" + SaleRow["Column42"] + "";

                        SqlCommand Command = new SqlCommand(CommandText, ConWare);
                        Ware = Command.ExecuteScalar().ToString();
                    }
                    if (Ware != string.Empty)
                        Sharh += " انبار " + Ware;


                }
                if (Convert.ToBoolean(setting.Rows[16]["Column02"]))
                {
                    string Func = string.Empty;

                    using (SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        string CommandText1 = @"SELECT     Column02 From table_005_PwhrsOperation WHERE ColumnId=" + SaleRow["Column43"] + "";

                        SqlCommand Command1 = new SqlCommand(CommandText1, ConWare);
                        Func = Command1.ExecuteScalar().ToString();
                    }

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
                //        string CommandText = @"SELECT     Column02 From table_004_CommodityAndIngredients WHERE ColumnId=(select top 1 column02 from " + ConSale.Database + ".dbo.Table_011_Child1_SaleFactor where column01 =" + SaleRow["columnid"] + ")   ";

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
                CommandText = string.Format(CommandText, mlt_Ware.Value.ToString(), GoodCode);
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
            Properties.Settings.Default.chk_Net = chk_Net.Checked;
            Properties.Settings.Default.Save();
            PrepareDoc();
        }

        private void Frm_009_ExportDocInformation_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.SalePCBes = chk_PCBes.Checked;
            Properties.Settings.Default.SalePCBed = chk_PCBed.Checked;
            Properties.Settings.Default.SaleGoodACCNum = chk_GoodACCNum.Checked;
            Properties.Settings.Default.Save();
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

        private void chk_GoodACCNum_CheckedChanged(object sender, EventArgs e)
        {
            PrepareDoc();

        }

        private void chk_DraftNum_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_DraftNum.Checked)
                txt_DraftNum.Enabled = true;
            else
                txt_DraftNum.Enabled = false;


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
                this.txt_serial.Text =null;

                faDatePicker1.Enabled = true;
            }
        }

        private void mlt_ToSerial_Leave(object sender, EventArgs e)
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
