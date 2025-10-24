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
 
    public partial class Frm_008_SortSaleFactors : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlDataAdapter HeaderAdapter, Child1Adapter, Child2Adapter;
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;

        public Frm_008_SortSaleFactors()
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

            Adapter = new SqlDataAdapter("Select * from Table_024_Discount", ConSale);
            Adapter.Fill(dataSet1, "Extra");
            gridEX_Extra.DropDowns["Extra"].SetDataBinding(dataSet1.Tables["Extra"], "");

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
            gridEX_Header.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from ListPeople(5)"), "");
            gridEX_Header.DropDowns["CustomerClub"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select Columnid,Column03+' '+Column02 as Column03 from Table_215_CustomerClub");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_055_CurrencyInfo");
            gridEX_Header.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");


            string[] Dates = Properties.Settings.Default.ViewSaleFactors.Split('-');


            //*******************************************
            HeaderAdapter = new SqlDataAdapter(@"Select Table_010_SaleFactor.*,
isnull(" + ConWare.Database + @".dbo.Table_007_PwhrsDraft.Column22,0) as CarryPrice from Table_010_SaleFactor left outer join " + ConWare.Database + @".dbo.Table_007_PwhrsDraft
on Table_010_SaleFactor.column09=" + ConWare.Database + ".dbo.Table_007_PwhrsDraft.columnid ", ConSale);
            HeaderAdapter.Fill(dataSet1, "Header");

            Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_011_Child1_SaleFactor.*    FROM         dbo.Table_010_SaleFactor INNER JOIN
            dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01", ConSale);
            Child1Adapter.Fill(dataSet1, "Child1");

            Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_012_Child2_SaleFactor.*    FROM         dbo.Table_010_SaleFactor INNER JOIN
            dbo.Table_012_Child2_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_012_Child2_SaleFactor.column01 ", ConSale);
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
            gridEX_Extra.DataSource = dataSet1.Tables["Header"];
            gridEX_List.DataMember = "R_Header_Child1";
            gridEX_Extra.DataMember = "R_Header_Child2";
            gridEX_Header_CurrentCellChanged(sender, e);

            txt_TotalPrice.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column28");
            txt_VolumeGroup.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column29");
            txt_SpecialGroup.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column30");
            txt_SpecialCustomer.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column31");
            txt_Extra.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column32");
            txt_Reductions.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column33");
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
                on Table_010_SaleFactor.column09=" + ConWare.Database + ".dbo.Table_007_PwhrsDraft.columnid ", ConSale);
                HeaderAdapter.Fill(dataSet1, "Header");

                Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_011_Child1_SaleFactor.*    FROM         dbo.Table_010_SaleFactor INNER JOIN
                dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01", ConSale);
                Child1Adapter.Fill(dataSet1, "Child1");

                Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_012_Child2_SaleFactor.*    FROM         dbo.Table_010_SaleFactor INNER JOIN
                dbo.Table_012_Child2_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_012_Child2_SaleFactor.column01 ", ConSale);
                Child2Adapter.Fill(dataSet1, "Child2");

                gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");

                dataSet1.EnforceConstraints = true;
            }
        }

        private void Form_008_ViewSaleFactors_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.E)
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
                gridEX_Extra.RemoveFilters();
                gridEX_Header.RemoveFilters();
                gridEX_List.RemoveFilters();

            }
            catch { }
        }

        private void bt_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridEX_Header.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 20))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_002_Faktor")
                            {
                                item.BringToFront();
                                Frm_002_Faktor frm = (Frm_002_Faktor)item;
                                frm.txt_Search.Text = gridEX_Header.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        Frm_002_Faktor frms = new Frm_002_Faktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 21), Convert.ToInt32(gridEX_Header.GetValue("columnid")));
                        try
                        {
                            frms.MdiParent = MainForm.ActiveForm;
                        }
                        catch { }
                        frms.Show();
                    }
                    else
                        Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
                }
            }
            catch { }

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

                    bt_Search_Click(sender, e);
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
            try
            {
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) - Convert.ToDouble(txt_VolumeGroup.Value.ToString()) - Convert.ToDouble(txt_SpecialGroup.Value.ToString()) - Convert.ToDouble(txt_SpecialCustomer.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
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

        private void bt_PrintSelectedFactors_Click(object sender, EventArgs e)
        {
            if (gridEX_Header.GetCheckedRows().Length > 0)
            {
                try
                {
                    if (this.gridEX_Header.GetCheckedRows().Length > 0)
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                        {

                            DataTable HeaderTable = dataSet_Sale2.Rpt_SaleTable.Clone();
                            DataTable DetailTable = dataSet_Sale2.Rpt_SaleExtra_Table.Clone();

                            Janus.Windows.GridEX.GridEXRow item = gridEX_Header.GetCheckedRows().Last();
                            DataTable CustomerTable = clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT     dbo.Table_045_PersonInfo.ColumnId, dbo.Table_045_PersonInfo.Column01, dbo.Table_045_PersonInfo.Column02, dbo.Table_045_PersonInfo.Column03, 
                            dbo.Table_045_PersonInfo.Column04, dbo.Table_045_PersonInfo.Column05, dbo.Table_045_PersonInfo.Column06, dbo.Table_045_PersonInfo.Column07, 
                            dbo.Table_045_PersonInfo.Column08, dbo.Table_045_PersonInfo.Column09, dbo.Table_045_PersonInfo.Column13, dbo.Table_045_PersonInfo.Column21, 
                            dbo.Table_045_PersonInfo.Column22, dbo.Table_060_ProvinceInfo.Column01 AS Province, dbo.Table_065_CityInfo.Column02 AS City
                            FROM         dbo.Table_060_ProvinceInfo INNER JOIN
                            dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00 RIGHT OUTER JOIN
                            dbo.Table_045_PersonInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column21 where ColumnId=" + item.Cells["Column03"].Value.ToString());

                            if (CustomerTable.Rows.Count > 0)
                            {


                                Double NetTotal = 0, Ezafat = 0, Kosoorat = 0, VolumeGroup = 0, SpecialGroup = 0, SpecialCustomer = 0;
                                Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(
                                    gridEX_Header.RootTable.Columns["Selector"], Janus.Windows.GridEX.ConditionOperator.Equal, true);

                                NetTotal = Convert.ToDouble(gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column28"], Janus.Windows.GridEX.AggregateFunction.Sum, filter).ToString());
                                Ezafat = Convert.ToDouble(gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column32"], Janus.Windows.GridEX.AggregateFunction.Sum, filter).ToString());
                                Kosoorat = Convert.ToDouble(gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column33"], Janus.Windows.GridEX.AggregateFunction.Sum, filter).ToString());
                                VolumeGroup = Convert.ToDouble(gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column29"], Janus.Windows.GridEX.AggregateFunction.Sum, filter).ToString());
                                SpecialGroup = Convert.ToDouble(gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column30"], Janus.Windows.GridEX.AggregateFunction.Sum, filter).ToString());
                                SpecialCustomer = Convert.ToDouble(gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column31"], Janus.Windows.GridEX.AggregateFunction.Sum, filter).ToString());

                                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_Header.GetCheckedRows())
                                {
                                    gridEX_Header.MoveTo(Row);

                                    foreach (Janus.Windows.GridEX.GridEXRow Child2 in gridEX_Extra.GetRows())
                                    {
                                        DetailTable.Rows.Add(Child2.Cells["Column01"].Value.ToString(),
                                            Child2.Cells["Column02"].Text,
                                            Convert.ToInt64(Convert.ToDouble(Child2.Cells["Column04"].Value.ToString())),
                                            (Child2.Cells["Column05"].Value.ToString() == "True" ? "-" : "+"));
                                    }

                                    foreach (Janus.Windows.GridEX.GridEXRow Child1 in gridEX_List.GetRows())
                                    {
                                        HeaderTable.Rows.Add(item.Cells["ColumnId"].Value.ToString(),
                                                       item.Cells["Column01"].Value.ToString(),
                                                       item.Cells["Column37"].Text,
                                                       item.Cells["Column02"].Value,
                                                       null,
                                                       item.Cells["Column03"].Value,
                                                       item.Cells["Column03"].Text,
                                                       CustomerTable.Rows[0]["Column09"].ToString(),
                                                       CustomerTable.Rows[0]["Column06"].ToString(),
                                                       CustomerTable.Rows[0]["Column07"].ToString(),
                                                       CustomerTable.Rows[0]["Column08"].ToString(),
                                                       CustomerTable.Rows[0]["Column13"].ToString(),
                                                       CustomerTable.Rows[0]["Column01"].ToString(),
                                                       Child1.Cells["Column002"].Text.Trim(),
                                                       Child1.Cells["Column02"].Text.ToString(),
                                                       Child1.Cells["Column04"].Value.ToString(),
                                                       Convert.ToInt64(Convert.ToDouble(Child1.Cells["Column08"].Value.ToString())),
                                                       Child1.Cells["Column05"].Value.ToString(),
                                                        Convert.ToInt64(Convert.ToDouble(Child1.Cells["Column09"].Value.ToString())),
                                                       Child1.Cells["Column06"].Value.ToString(),
                                                       Child1.Cells["Column07"].Value.ToString(),
                                                        Convert.ToInt64(Convert.ToDouble(Child1.Cells["Column10"].Value.ToString())),
                                                        Convert.ToInt64(Convert.ToDouble(Child1.Cells["Column11"].Value.ToString())),
                                                       Child1.Cells["Column16"].Value.ToString(),
                                                        Convert.ToInt64(Convert.ToDouble(Child1.Cells["Column17"].Value.ToString())),
                                                        Convert.ToInt64(Convert.ToDouble(Child1.Cells["Column19"].Value.ToString())),
                                                        Convert.ToInt64(Convert.ToDouble(Child1.Cells["Column20"].Value.ToString())),
                                                       Convert.ToInt64(Convert.ToDouble(Ezafat)),
                                                       Convert.ToInt64(Convert.ToDouble(Kosoorat)),
                                                       item.Cells["Column05"].Text,
                                                       Convert.ToInt64(Convert.ToDouble(VolumeGroup)),
                                                       Convert.ToInt64(Convert.ToDouble(SpecialGroup)),
                                                       Convert.ToInt64(Convert.ToDouble(SpecialCustomer)),
                                                       Child1.Cells["Column06"].Text,
                                                       Child1.Cells["Column03"].Text,
                                                       DBNull.Value, "-", "SettleInfo",
                                                       (item.Cells["Column12"].Value.ToString() == "True" ? "***فاکتور ارزی***" : "***فاکتور ریالی***"),
                                                       Child1.Cells["column31"].Value.ToString(),
                                                       Child1.Cells["Column23"].Text,
                                                       Child1.Cells["Column33"].Text,
                                                       Child1.Cells["Column32"].Value.ToString(),
                                                       CustomerTable.Rows[0]["Province"].ToString(),
                                                       CustomerTable.Rows[0]["City"].ToString(),
                                                       item.Cells["Column21"].Value.ToString(),
                                                       NetTotal,
                                                       Child1.Cells["Column18"].Value.ToString(),
                                                       item.Cells["Column09"].Text,
                                                       Child1.Cells["Column37"].Value.ToString(),
                                                       Child1.Cells["Column23"].Text.ToString(),
                                                       null,
                                                       item.Cells["Column10"].Value.ToString()
                                                       );






                                    }

                                }
                                _05_Sale.Reports.Form_SaleFactorPrint frm = new Reports.Form_SaleFactorPrint(HeaderTable, DetailTable, false);
                                frm.ShowDialog();

                            }
                            else Class_BasicOperation.ShowMsg("", "مشتری با این مشخصات وجود ندارد", "Warning");


                        }
                        else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
            else Class_BasicOperation.ShowMsg("", "فاکتورهای مورد نظر را جهت تجمیع انتخاب کنید", "Warning");
        }

        private void bt_Sort_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("مرتب سازی، از ابتدای فاکتورهای فروش صورت خواهد گرفت و در اثر این عملیات شماره فاکتورهای فروش تغییر خواهد کرد" +
               Environment.NewLine + "آیا مایل به مرتب سازی هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
            {
                SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE);
                Con.Open();
                SqlDataAdapter Adapter = new SqlDataAdapter(
                    "select *, row_Number() over (order by column02,ColumnId) as RowNumber from Table_010_SaleFactor " +
                    " order by column02,ColumnId", Con);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                SqlCommand Update = new SqlCommand(" ", Con);
                foreach (DataRow item in Table.Rows)
                {
                    Update.CommandText = "Update Table_010_SaleFactor SET column01=" +
                        item["RowNumber"].ToString() + " where ColumnId=" + item["ColumnId"].ToString();
                    Update.ExecuteNonQuery();
                }
                Class_BasicOperation.ShowMsg("", "مرتب سازی فاکتورهای فروش با موفقیت صورت گرفت", "Information");
                Con.Close();
                bt_Search_Click(null, null);
            }
        }
    }
}
