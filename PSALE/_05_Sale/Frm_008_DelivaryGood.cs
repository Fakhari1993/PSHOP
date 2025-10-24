using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing;
namespace PSHOP._05_Sale
{
    public partial class Frm_008_DelivaryGood : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlDataAdapter HeaderAdapter, Child1Adapter, Child2Adapter;
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();

        bool _BackSpace = false;

        public Frm_008_DelivaryGood()
        {
            InitializeComponent();
        }
        private void gridEX_Extra_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            try
            {
                if (e.Row.RowType == Janus.Windows.GridEX.RowType.Record)
                {
                    if (e.Row.Cells["column05"].Value.ToString() == "True")
                        e.Row.RowHeaderImageIndex = 1;
                    else
                        e.Row.RowHeaderImageIndex = 0;
                }
            }
            catch { }
        }

        private void Form_008_ViewSaleFactors_Load(object sender, EventArgs e)
        {
            SqlDataAdapter Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(dataSet1, "Center");
            gridEX_List.DropDowns["Center"].SetDataBinding(dataSet1.Tables["Center"], "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
            Adapter.Fill(dataSet1, "Project");
            gridEX_List.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Project"], "");

            Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
            DataTable GoodTable = clGood.GoodInfo();
            gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            Adapter = new SqlDataAdapter("Select * from Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(dataSet1, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(dataSet1.Tables["CountUnit"], "");



            gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_007_PwhrsDraft", ConWare);
            Adapter.Fill(dataSet1, "Draft");
            gridEX_Header.DropDowns["Draft"].SetDataBinding(dataSet1.Tables["Draft"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_007_FactorBefore", ConSale);
            Adapter.Fill(dataSet1, "Prefactor");
            gridEX_Header.DropDowns["Prefactor"].SetDataBinding(dataSet1.Tables["Prefactor"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_018_MarjooiSale", ConSale);
            Adapter.Fill(dataSet1, "Return");
            gridEX_Header.DropDowns["Return"].SetDataBinding(dataSet1.Tables["Return"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
            Adapter.Fill(dataSet1, "Customer");
            gridEX_Header.DropDowns["Customer"].SetDataBinding(dataSet1.Tables["Customer"], "");

            gridEX_Header.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT ColumnId,Column01,Column02 from Table_002_SalesTypes"), "");
            gridEX_Header.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Columnid,Column01,Column02 from Table_045_PersonInfo"), "");
            gridEX_Header.DropDowns["CustomerClub"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select Columnid,Column03+' '+Column02 as Column03 from Table_215_CustomerClub");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_055_CurrencyInfo");
            gridEX_Header.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");


            string[] Dates = Properties.Settings.Default.ViewSaleFactors.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;

            //*******************************************
            HeaderAdapter = new SqlDataAdapter(@"Select Table_010_SaleFactor.*,
isnull(" + ConWare.Database + @".dbo.Table_007_PwhrsDraft.Column22,0) as CarryPrice from Table_010_SaleFactor left outer join " + ConWare.Database + @".dbo.Table_007_PwhrsDraft
on Table_010_SaleFactor.column09=" + ConWare.Database + @".dbo.Table_007_PwhrsDraft.columnid where  
                                                                                                Table_010_SaleFactor.Column17=0 and--ابطال نباشد
                                                                                                Table_010_SaleFactor.Column19=0 and--مرجوعی نباشد
                                                                                                Table_010_SaleFactor.column09=0 and-- حواله نداشته باشد
                                                                                                Table_010_SaleFactor.Column45=1 --تسویه شده باشد", ConSale);
            HeaderAdapter.Fill(dataSet1, "Header");

            Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_011_Child1_SaleFactor.*    FROM         dbo.Table_010_SaleFactor INNER JOIN
            dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 where  
                                                                                                Table_010_SaleFactor.Column17=0 and--ابطال نباشد
                                                                                                Table_010_SaleFactor.Column19=0 and--مرجوعی نباشد
                                                                                                Table_010_SaleFactor.column09=0 and-- حواله نداشته باشد
                                                                                                Table_010_SaleFactor.Column45=1 --تسویه شده باشد", ConSale);
            Child1Adapter.Fill(dataSet1, "Child1");

            Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_012_Child2_SaleFactor.*    FROM         dbo.Table_010_SaleFactor INNER JOIN
            dbo.Table_012_Child2_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_012_Child2_SaleFactor.column01 where  
                                                                                                Table_010_SaleFactor.Column17=0 and--ابطال نباشد
                                                                                                Table_010_SaleFactor.Column19=0 and--مرجوعی نباشد
                                                                                                Table_010_SaleFactor.column09=0 and-- حواله نداشته باشد
                                                                                                Table_010_SaleFactor.Column45=1 --تسویه شده باشد", ConSale);
            Child2Adapter.Fill(dataSet1, "Child2");

            DataRelation Relation1 = new DataRelation("R_Header_Child1", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child1"].Columns["Column01"]);
            DataRelation Relation2 = new DataRelation("R_Header_Child2", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child2"].Columns["Column01"]);

            ForeignKeyConstraint Fkc1 = new ForeignKeyConstraint("F_Header_Child1", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child1"].Columns["Column01"]);
            ForeignKeyConstraint Fkc2 = new ForeignKeyConstraint("F_Header_Child2", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child2"].Columns["Column01"]);
            Fkc1.UpdateRule = Rule.Cascade;
            Fkc1.AcceptRejectRule = AcceptRejectRule.None;
            Fkc1.DeleteRule = Rule.None;
            Fkc2.UpdateRule = Rule.Cascade;
            Fkc2.AcceptRejectRule = AcceptRejectRule.None;
            Fkc2.DeleteRule = Rule.None;

            dataSet1.Tables["Child1"].Constraints.Add(Fkc1);
            dataSet1.Tables["Child2"].Constraints.Add(Fkc2);

            dataSet1.Relations.Add(Relation1);
            dataSet1.Relations.Add(Relation2);

            gridEX_Header.DataSource = dataSet1.Tables["Header"];
            gridEX_List.DataSource = dataSet1.Tables["Header"];

            gridEX_List.DataMember = "R_Header_Child1";

            gridEX_Header_CurrentCellChanged(sender, e);

            gridEX_Header.Select();
            gridEX_Header.Focus();
            gridEX_Header.Row = gridEX_Header.FilterRow.Position;

        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            {
                dataSet1.EnforceConstraints = false;
                dataSet1.Tables["Header"].Clear();
                dataSet1.Tables["Child1"].Clear();
                dataSet1.Tables["Child2"].Clear();

                HeaderAdapter = new SqlDataAdapter(@"Select Table_010_SaleFactor.*,
                isnull(" + ConWare.Database + @".dbo.Table_007_PwhrsDraft.Column22,0) as CarryPrice from Table_010_SaleFactor left outer join " + ConWare.Database + @".dbo.Table_007_PwhrsDraft
                on Table_010_SaleFactor.column09=" + ConWare.Database + @".dbo.Table_007_PwhrsDraft.columnid where  
                                                                                                Table_010_SaleFactor.Column17=0 and--ابطال نباشد
                                                                                                Table_010_SaleFactor.Column19=0 and--مرجوعی نباشد
                                                                                                Table_010_SaleFactor.column09=0 and-- حواله نداشته باشد
                                                                                                Table_010_SaleFactor.Column45=1 --تسویه شده باشد", ConSale);
                HeaderAdapter.Fill(dataSet1, "Header");

                Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_011_Child1_SaleFactor.*    FROM         dbo.Table_010_SaleFactor INNER JOIN
                dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 where  
                                                                                                Table_010_SaleFactor.Column17=0 and--ابطال نباشد
                                                                                                Table_010_SaleFactor.Column19=0 and--مرجوعی نباشد
                                                                                                Table_010_SaleFactor.column09=0 and-- حواله نداشته باشد
                                                                                                Table_010_SaleFactor.Column45=1 --تسویه شده باشد", ConSale);
                Child1Adapter.Fill(dataSet1, "Child1");

                Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_012_Child2_SaleFactor.*    FROM         dbo.Table_010_SaleFactor INNER JOIN
                dbo.Table_012_Child2_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_012_Child2_SaleFactor.column01 where  
                                                                                                Table_010_SaleFactor.Column17=0 and--ابطال نباشد
                                                                                                Table_010_SaleFactor.Column19=0 and--مرجوعی نباشد
                                                                                                Table_010_SaleFactor.column09=0 and-- حواله نداشته باشد
                                                                                                Table_010_SaleFactor.Column45=1 --تسویه شده باشد", ConSale);
                Child2Adapter.Fill(dataSet1, "Child2");

                gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");

                dataSet1.EnforceConstraints = true;
            }
        }

        private void Form_008_ViewSaleFactors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                faDatePickerStrip1.FADatePicker.Select();
                faDatePickerStrip1.Select();
            }
            else if (e.Control && e.KeyCode == Keys.E)
                bt_Edit_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.KeyCode == Keys.F5)
                bt_Refresh_Click(sender, e);
        }

        private void Form_008_ViewSaleFactors_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                gridEX_Header.RemoveFilters();
                gridEX_List.RemoveFilters();

            }
            catch { }
        }
        private float FirstRemain(int GoodCode, Int16 ware, string date)
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
                CommandText = string.Format(CommandText, ware, GoodCode, date);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }

        private float TotalRemain(int GoodCode, Int16 ware)
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
                CommandText = string.Format(CommandText, ware, GoodCode);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }
        private void bt_Edit_Click(object sender, EventArgs e)
        {

            if (gridEX_Header.GetCheckedRows().Count() != 1)
            {
                Class_BasicOperation.ShowMsg("", "یک فاکتور انتخاب کنید", "Warning");
                return;

            }
            foreach (Janus.Windows.GridEX.GridEXRow item2 in gridEX_Header.GetCheckedRows())
            {
                try
                {
                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", item2.Cells["columnid"].Value.ToString()) == 0)
                    {

                        DataTable _Table = new DataTable();
                        _Table = clDoc.ReturnTable(this.ConSale.ConnectionString, @"SELECT tcsf.*,tcai.column02 as GoodName
                                                                                FROM   Table_011_Child1_SaleFactor tcsf
                                                                                       JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                                            ON  tcai.columnid = tcsf.column02
                                                                                WHERE  tcsf.column01 =" + item2.Cells["columnid"].Value + " ");
                        foreach (DataRow item1 in _Table.Rows)
                        {
                            if (!clGood.IsGoodInWare(Int16.Parse(item2.Cells["Column42"].Value.ToString()),
                                int.Parse(item1["column02"].ToString())))
                                throw new Exception("کالای " + item1["GoodName"].ToString() +
                                    " در انبار انتخاب شده فعال نمی باشد");
                        }


                        //چک باقی مانده کالا
                        foreach (DataRow item1 in _Table.Rows)
                        {
                            if (clDoc.IsGood(item1["Column02"].ToString()))
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
                                                                                   WHERE  ColumnId = " + item1["Column02"] + @"
                                                                               ),
                                                                               0
                                                                           ) AS Column16", ConWareGood);
                                        mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                                    }
                                }
                                catch
                                {
                                }
                                float Remain = FirstRemain(int.Parse(item1["Column02"].ToString()), Int16.Parse(item2.Cells["Column42"].Value.ToString()), faDatePickerStrip1.FADatePicker.Text);
                                if (Remain < float.Parse(item1["Column07"].ToString()) && !mojoodimanfi)
                                {
                                    throw new Exception("موجودی کالای " + clDoc.ExScalar(ConWare.ConnectionString,
                                        "table_004_CommodityAndIngredients", "Column02", "ColumnId",
                                        item1["Column02"].ToString()) +
                                        " کمتر از تعداد مشخص شده در فاکتور است" + Environment.NewLine +
                                        "موجودی: " + Remain.ToString());
                                }
                            }
                        }
                        bool ok = true;
                        string good = string.Empty;


                        foreach (DataRow item in _Table.Rows)
                        {
                            if (clDoc.IsGood(item["Column02"].ToString()))
                            {
                                float Remain = TotalRemain(int.Parse(item["Column02"].ToString()), Int16.Parse(item2.Cells["Column42"].Value.ToString()));
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
                            int DraftNum = 0, DraftID = 0;
                            //درج هدر حواله
                            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                            Key.Direction = ParameterDirection.Output;
                            DraftNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01");
                            //, int.Parse(mlt_Ware.Value.ToString()));
                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                            {
                                Con.Open();
                                SqlCommand InsertHeader = new SqlCommand(@"INSERT INTO Table_007_PwhrsDraft  ([column01]
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
           ,[Column26]) VALUES(" + DraftNum + ",'" + faDatePickerStrip1.FADatePicker.Text + "'," + item2.Cells["Column42"].Value
                                    + "," + item2.Cells["Column43"].Value + "," + item2.Cells["column03"].Value + ",'" + "حواله صادره بابت فاکتور فروش ش" + item2.Cells["column01"].Value +
                                    "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + item2.Cells["columnid"].Value + ",0," +
                                    item2.Cells["column07"].Value + ",0,0,0,0," +
                                    (item2.Cells["Column12"].Value.ToString() == "False" ? "0" : "1") + "," + (item2.Cells["Column40"].Value.ToString().Trim() == "" ? "NULL" : item2.Cells["Column40"].Value.ToString())
                                    + "," +
                                    item2.Cells["Column41"].Value.ToString() + ",1); SET @Key=SCOPE_IDENTITY()", Con);
                                InsertHeader.Parameters.Add(Key);
                                InsertHeader.ExecuteNonQuery();
                                DraftID = int.Parse(Key.Value.ToString());
                                clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column09", "ColumnId", int.Parse(item2.Cells["columnid"].Value.ToString()), DraftID);

                                //درج کالاهای موجود در حواله 
                                foreach (DataRow item in _Table.Rows)
                                {
                                    if (clDoc.IsGood(item["Column02"].ToString()))
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
           ,[Column35]) VALUES(" + DraftID + "," + item["Column02"].ToString() + "," +
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
                                                + "," + item["Column32"].ToString() + "," + item["Column36"].ToString() + "," + item["Column37"].ToString() + ")", Con);
                                        InsertDetail.ExecuteNonQuery();
                                    }

                                }

                                SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + DraftID, ConWare);
                                DataTable Table = new DataTable();
                                goodAdapter.Fill(Table);

                                //محاسبه ارزش و ذخیره آن در جدول Child1
                                try
                                {
                                    foreach (DataRow item in Table.Rows)
                                    {
                                        if (Class_BasicOperation._WareType)
                                        {
                                            SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + item2.Cells["Column42"].Value, Con);
                                            DataTable TurnTable = new DataTable();
                                            Adapter.Fill(TurnTable);
                                            DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftID + " and DetailID=" + item["Columnid"].ToString());
                                            SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                                + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                            UpdateCommand.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + item2.Cells["Column42"].Value + ",@Date='" + faDatePickerStrip1.FADatePicker.Text + "',@id=" + DraftID + ",@residid=0", ConWare);
                                            DataTable TurnTable = new DataTable();
                                            Adapter.Fill(TurnTable);
                                            SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                               + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(TurnTable.Rows[0]["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString(), Con);
                                            UpdateCommand.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch
                                {
                                }
                                clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_010_SaleFactor SET Column50=1,Column51='" + this.faDatePickerStrip1.FADatePicker.Text + "'  where columnid=" + item2.Cells["columnid"].Value.ToString());


                                Class_BasicOperation.ShowMsg("", "حواله انبار به شماره " + DraftNum.ToString() + " با موفقیت ثبت شد ", "Information");
                                bt_Search_Click(null, null);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
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
                    this.bt_Edit.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }



        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridEX_Header.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        if (gridEX_Header.GetCheckedRows().Length == 0)
                        {

                            _05_Sale.Reports.Form_SaleFactorPrint frm =
                                new Reports.Form_SaleFactorPrint(int.Parse(gridEX_Header.GetValue("Column01").ToString()), false);
                            frm.ShowDialog();
                        }
                        else
                        {
                            List<string> List = new List<string>();
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Header.GetCheckedRows())
                            {
                                List.Add(item.Cells["ColumnId"].Value.ToString());
                            }
                            _05_Sale.Reports.Form_SaleFactorPrint frm = new Reports.Form_SaleFactorPrint(List, false);
                            frm.Form_FactorPrint_Load(sender, e);
                        }
                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }

            }
            catch { }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            bt_Search_Click(sender, e);
        }

        private void gridEX_Header_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void mnu_Prefactors_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 64))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_002_ViewPrefactors")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                _05_Sale.Frm_002_ViewPrefactors frm = new Frm_002_ViewPrefactors();
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

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            if (gridEX_Header.RowCount > 0)
            {
                int SanadId = (gridEX_Header.GetRow().Cells["Column10"].Text.Trim() == "" ? 0 : int.Parse(gridEX_Header.GetRow().Cells["Column10"].Value.ToString()));
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
                                ((PACNT._2_DocumentMenu.Form04_ViewDocument)item).table_060_SanadHeadBindingSource.Find("ColumnId", SanadId);
                            return;
                        }
                    }
                    PACNT._2_DocumentMenu.Form04_ViewDocument frm = new PACNT._2_DocumentMenu.Form04_ViewDocument(SanadId);
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
        }

        private void mnu_Drafts_Click(object sender, EventArgs e)
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

        private void gridEX_List_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            try
            {
                if (e.Row.RowType == Janus.Windows.GridEX.RowType.Record && e.Row.Cells["column30"].Value.ToString() == "True")
                    e.Row.RowHeaderImageIndex = 2;
            }
            catch { }
        }

        private void bt_RepairFactors_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_010_SaleFactor.columnid, ISNULL(Child1.TotalNet, 0) AS TotalNet, ISNULL(Child1.ExLin, 0) AS ExLin, ISNULL(Child1.DisLin, 0) AS DisLin, 
                      ISNULL(ExTable.Extra, 0) AS Extra, ISNULL(DisTable.Discount, 0) AS Discount
                           FROM         dbo.Table_010_SaleFactor LEFT OUTER JOIN
                          (SELECT     column01, SUM(column04) AS Discount
                            FROM          dbo.Table_012_Child2_SaleFactor AS Table_012_Child2_SaleFactor_1
                            WHERE      (column05 = 1)
                            GROUP BY column01) AS DisTable ON dbo.Table_010_SaleFactor.columnid = DisTable.column01 LEFT OUTER JOIN
                          (SELECT     column01, SUM(column04) AS Extra
                            FROM          dbo.Table_012_Child2_SaleFactor
                            WHERE      (column05 = 0)
                            GROUP BY column01) AS ExTable ON dbo.Table_010_SaleFactor.columnid = ExTable.column01 LEFT OUTER JOIN
                          (SELECT     column01, SUM(column20) AS TotalNet, SUM(column19) AS ExLin, SUM(column17) AS DisLin
                            FROM          dbo.Table_011_Child1_SaleFactor
                            GROUP BY column01) AS Child1 ON dbo.Table_010_SaleFactor.columnid = Child1.column01
                            WHERE     (dbo.Table_010_SaleFactor.Column28 = 0)", ConSale);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            foreach (DataRow item in Table.Rows)
            {
                clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE     dbo.Table_010_SaleFactor SET Column28=" +
                    item["TotalNet"].ToString() + " , Column32=" + item["Extra"].ToString() +
                    " , Column33=" + item["Discount"].ToString() + " , Column34=" + item["ExLin"].ToString()
                    + " , Column35=" + item["DisLin"].ToString() + " where ColumnId=" + item["ColumnId"].ToString());

            }
            Class_BasicOperation.ShowMsg("", "بازسازی فاکتورهای فروش با موفقیت صورت گرفت", "Information");
            this.Cursor = Cursors.Default;

        }

        private void gridEX_Header_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
             bt_Edit_Click(sender, e);
        }

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Header;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void mnu_ExportToExcel_Detail_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_List;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }


    }
}
