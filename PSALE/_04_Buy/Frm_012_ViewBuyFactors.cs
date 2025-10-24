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
    public partial class Frm_012_ViewBuyFactors : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlDataAdapter HeaderAdapter, Child1Adapter, Child2Adapter, ResidAdapter, DocAdapter, ReturnAdapter;
        DataSet DS = new DataSet();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        bool Isadmin = false;
        Int16 projectId;
        public Frm_012_ViewBuyFactors()
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
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.MAIN))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("Select Column02 from Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" +
                Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + "'", Con);
                Isadmin = (bool.Parse(Select.ExecuteScalar().ToString()));

            }

            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT tsi.Column05
                                                                        FROM   dbo.Table_295_StoreInfo AS tsi
                                                                               JOIN dbo.Table_296_StoreUsers AS tsu
                                                                                    ON  tsu.Column00 = tsi.ColumnId
                                                                        WHERE tsu.Column01='" + Class_BasicOperation._UserName + "'", ConBase);
            DataTable StoreTable = new DataTable();

            Adapter.Fill(StoreTable);

            if (!Isadmin && StoreTable.Rows.Count == 0) { Class_BasicOperation.ShowMsg("", "کاربر گرامی، فروشگاه شما تعیین نشده است و امکان کار با این فرم را ندارید ", "Stop"); this.Dispose(); }

            else if (StoreTable.Rows.Count > 0) projectId = Convert.ToInt16(StoreTable.Rows[0]["Column05"]);

            //ResidAdapter = new SqlDataAdapter("SELECT ColumnId,Column01 from Table_011_PwhrsReceipt", ConWare);
            //ResidAdapter.Fill(DS, "Resid");
            //gridEX1.DropDowns["Resid"].SetDataBinding(DS.Tables["Resid"], "");

            //DocAdapter = new SqlDataAdapter("Select ColumnId,Column00 from Table_060_SanadHead", ConAcnt);
            //DocAdapter.Fill(DS, "Doc");
            //gridEX1.DropDowns["Doc"].SetDataBinding(DS.Tables["Doc"], "");

            //ReturnAdapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_021_MarjooiBuy", ConSale);
            //ReturnAdapter.Fill(DS, "Return");
            //gridEX1.DropDowns["Return"].SetDataBinding(DS.Tables["Return"], "");

            DataTable GoodTable = clGood.GoodInfo();
            gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            //gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from ListPeople(5)"), "");

              Adapter = new SqlDataAdapter("SELECT * FROM Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 FROM Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(DS, "Center");
            gridEX_List.DropDowns["Center"].SetDataBinding(DS.Tables["Center"], "");

            Adapter = new SqlDataAdapter("SELECT Column00,Column01,Column02 FROM Table_035_ProjectInfo", ConBase);
            Adapter.Fill(DS, "Project");
            gridEX_List.DropDowns["Project"].SetDataBinding(DS.Tables["Project"], "");

            //gridEX1.DropDowns["Buyer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,4)"), "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount_Buy", ConSale);
            Adapter.Fill(DS, "Discount");
            gridEX_Extra.DropDowns["Extra"].SetDataBinding(DS.Tables["Discount"], "");

            Adapter = new SqlDataAdapter("select ColumnId,Column01 from Table_013_RequestBuy", ConSale);
            Adapter.Fill(DS, "Request");
            gridEX_List.DropDowns["Request"].SetDataBinding(DS.Tables["Request"], "");

            //DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_055_CurrencyInfo");
            // gridEX1.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            // gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");

            // DataTable dt = new DataTable();
            //SqlDataAdapter ProjectAdapter = new SqlDataAdapter("SELECT * from Table_035_ProjectInfo", ConBase);
            //ProjectAdapter.Fill(dt);
            //gridEX1.DropDowns["project"].SetDataBinding(dt, "");

            string[] Dates = Properties.Settings.Default.ViewBuyFactors.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);

            //*******************************************
            //            HeaderAdapter = new SqlDataAdapter("Select * from Table_015_BuyFactor where Column02>='" + faDatePickerStrip1.FADatePicker.Text + "' and Column02<='" + faDatePickerStrip2.FADatePicker.Text + "'", ConSale);
            //            HeaderAdapter.Fill(DS, "Header");

            //            Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_016_Child1_BuyFactor.*    FROM         dbo.Table_015_BuyFactor INNER JOIN
            //            dbo.Table_016_Child1_BuyFactor ON dbo.Table_015_BuyFactor.columnid = dbo.Table_016_Child1_BuyFactor.column01 WHERE     (dbo.Table_015_BuyFactor.column02 >='" + 
            //              faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_015_BuyFactor.column02<='" 
            //              + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
            //            Child1Adapter.Fill(DS, "Child1");

            //            Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_017_Child2_BuyFactor.*    FROM         dbo.Table_015_BuyFactor INNER JOIN
            //            dbo.Table_017_Child2_BuyFactor ON dbo.Table_015_BuyFactor.columnid = dbo.Table_017_Child2_BuyFactor.column01 WHERE     (dbo.Table_015_BuyFactor.column02 >='"
            //                + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_015_BuyFactor.column02<='" +
            //                faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
            //            Child2Adapter.Fill(DS, "Child2");

            HeaderAdapter = new SqlDataAdapter(@"Select sale.[columnid]
      ,sale.[column01]
      ,sale.[column02]
      ,a.column02 [column03]
      ,sale.[column04]
      ,sale.[column05]
      ,sale.[column06]
      ,sale.[column07]
      ,sale.[column08]
      ,sale.[column09]
      ,rd.Column01 [column10]
      ,sh.Column00 [column11]
      ,sale.[column12]
      ,sale.[column13]
      ,a2.column02 [column14]
      ,sale.[column15]
      ,sale.[column16]
      ,sale.[column17]
      ,ms.Column01 [column18]
      ,sale.[column19]
      ,sale.[Column20]
      ,sale.[Column21]
      ,sale.[Column22]
      ,sale.[Column23]
      ,sale.[Column24]
      ,sale.[Column25]
      ,sale.[Column26]
      ,sale.[Column27]
      ,sale.[Column28]
      ,sale.[Column29]
      ,sale.[Column30]
      ,sale.[Column31]
      ,sale.[Column32]
      ,sale.[Column33]
      ,sale.[Column34] from Table_015_BuyFactor sale
            left join " + ConBase.Database + @".dbo.Table_045_PersonInfo a on a.ColumnId=sale.column03
        left join " + ConBase.Database + @".dbo.Table_045_PersonInfo a2 on a2.ColumnId=sale.column14
        left join " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt rd on rd.ColumnId=sale.column10
        left join " + ConAcnt.Database + @".dbo.Table_060_SanadHead sh on sh.ColumnId=sale.column11
        left join  Table_021_MarjooiBuy ms on ms.ColumnId=sale.Column18
            
            
            where sale.columnid=1", ConSale);
            HeaderAdapter.Fill(DS, "Header");

            Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_016_Child1_BuyFactor.*    FROM    dbo.Table_016_Child1_BuyFactor  WHERE     (dbo.Table_016_Child1_BuyFactor.column01=1) ", ConSale);
            Child1Adapter.Fill(DS, "Child1");

            Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_017_Child2_BuyFactor.*    FROM dbo.Table_017_Child2_BuyFactor  WHERE     (dbo.Table_017_Child2_BuyFactor.column01=1)", ConSale);
            Child2Adapter.Fill(DS, "Child2");


            DataRelation Relation1 = new DataRelation("R_Header_Child1", DS.Tables["Header"].Columns["ColumnId"], DS.Tables["Child1"].Columns["Column01"]);
            DataRelation Relation2 = new DataRelation("R_Header_Child2", DS.Tables["Header"].Columns["ColumnId"], DS.Tables["Child2"].Columns["Column01"]);

            ForeignKeyConstraint Fkc1 = new ForeignKeyConstraint("F_Header_Child1", DS.Tables["Header"].Columns["ColumnId"], DS.Tables["Child1"].Columns["Column01"]);
            ForeignKeyConstraint Fkc2 = new ForeignKeyConstraint("F_Header_Child2", DS.Tables["Header"].Columns["ColumnId"], DS.Tables["Child2"].Columns["Column01"]);
            Fkc1.UpdateRule = Rule.Cascade;
            Fkc1.AcceptRejectRule = AcceptRejectRule.None;
            Fkc1.DeleteRule = Rule.None;
            Fkc2.UpdateRule = Rule.Cascade;
            Fkc2.AcceptRejectRule = AcceptRejectRule.None;
            Fkc2.DeleteRule = Rule.None;

            DS.Tables["Child1"].Constraints.Add(Fkc1);
            DS.Tables["Child2"].Constraints.Add(Fkc2);

            DS.Relations.Add(Relation1);
            DS.Relations.Add(Relation2);

            gridEX1.DataSource = DS.Tables["Header"];
            gridEX_List.DataSource = DS.Tables["Header"];
            gridEX_Extra.DataSource = DS.Tables["Header"];
            gridEX_List.DataMember = "R_Header_Child1";
            gridEX_Extra.DataMember = "R_Header_Child2";
            gridEX_Header_CurrentCellChanged(sender, e);

            txt_TotalPrice.DataBindings.Add("Value", DS.Tables["Header"], "Column20");
            txt_Extra.DataBindings.Add("Value", DS.Tables["Header"], "Column21");
            txt_Reductions.DataBindings.Add("Value", DS.Tables["Header"], "Column22");
            bt_Search_Click(null, null);
            gridEX1.Select();
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.Text.Length == 10 && faDatePickerStrip1.FADatePicker.Text.Length == 10)
            {
                DS.EnforceConstraints = false;
                DS.Tables["Header"].Clear();
                DS.Tables["Child1"].Clear();
                DS.Tables["Child2"].Clear();
                //DS.Tables["Doc"].Clear();
                //DS.Tables["Return"].Clear();
                //DS.Tables["Resid"].Clear();

                //DocAdapter.Fill(DS, "Doc");
                //ReturnAdapter.Fill(DS, "Return");
                //ResidAdapter.Fill(DS, "Resid");

                HeaderAdapter = new SqlDataAdapter(@"Select sale.[columnid]
      ,sale.[column01]
      ,sale.[column02]
      ,a.column02 [column03]
      ,sale.[column04]
      ,sale.[column05]
      ,sale.[column06]
      ,sale.[column07]
      ,sale.[column08]
      ,sale.[column09]
      ,rd.Column01 [column10]
      ,sh.Column00 [column11]
      ,sale.[column12]
      ,sale.[column13]
      ,a2.column02 [column14]
      ,sale.[column15]
      ,sale.[column16]
      ,sale.[column17]
      ,ms.Column01 [column18]
      ,sale.[column19]
      ,sale.[Column20]
      ,sale.[Column21]
      ,sale.[Column22]
      ,sale.[Column23]
      ,sale.[Column24]
      ,sale.[Column25]
      ,sale.[Column26]
      ,sale.[Column27]
      ,sale.[Column28]
      ,sale.[Column29]
      ,sale.[Column30]
      ,sale.[Column31]
      ,sale.[Column32]
      ,sale.[Column33]
      ,sale.[Column34] from Table_015_BuyFactor sale
            left join " + ConBase.Database + @".dbo.Table_045_PersonInfo a on a.ColumnId=sale.column03
        left join " + ConBase.Database + @".dbo.Table_045_PersonInfo a2 on a2.ColumnId=sale.column14
        left join " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt rd on rd.ColumnId=sale.column10
        left join " + ConAcnt.Database + @".dbo.Table_060_SanadHead sh on sh.ColumnId=sale.column11
        left join  Table_021_MarjooiBuy ms on ms.ColumnId=sale.Column18  where sale.Column02>='" +
                    faDatePickerStrip1.FADatePicker.Text + "' and sale.Column02<='" + faDatePickerStrip2.FADatePicker.Text + "' and (sale.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')", ConSale);
                HeaderAdapter.Fill(DS, "Header");

                Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_016_Child1_BuyFactor.*    FROM         dbo.Table_015_BuyFactor INNER JOIN
                 dbo.Table_016_Child1_BuyFactor ON dbo.Table_015_BuyFactor.columnid = dbo.Table_016_Child1_BuyFactor.column01 WHERE     (dbo.Table_015_BuyFactor.column02 >='"
                    + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_015_BuyFactor.column02<='" +
                    faDatePickerStrip2.FADatePicker.Text + "') and (dbo.Table_015_BuyFactor.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')", ConSale);
                Child1Adapter.Fill(DS, "Child1");

                Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_017_Child2_BuyFactor.*    FROM         dbo.Table_015_BuyFactor INNER JOIN
                dbo.Table_017_Child2_BuyFactor ON dbo.Table_015_BuyFactor.columnid = dbo.Table_017_Child2_BuyFactor.column01 WHERE     (dbo.Table_015_BuyFactor.column02 >='"
                    + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_015_BuyFactor.column02<='" +
                    faDatePickerStrip2.FADatePicker.Text + "') and (dbo.Table_015_BuyFactor.Column29=" + projectId + " or '" + (Isadmin) + @"'=N'True')", ConSale);
                Child2Adapter.Fill(DS, "Child2");

                DS.EnforceConstraints = true;
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
            gridEX_List.RemoveFilters();
            gridEX1.RemoveFilters();
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.ViewBuyFactors = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
        }

        private void bt_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridEX1.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 28))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_003_FaktorKharid")
                            {
                                item.BringToFront();
                                Frm_003_FaktorKharid frm = (Frm_003_FaktorKharid)item;
                                frm.txt_Search.Text = gridEX1.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        Frm_003_FaktorKharid frms = new Frm_003_FaktorKharid(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 29), Convert.ToInt32(gridEX1.GetValue("columnid")));
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

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 129))
                {
                    string Number = "";
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetCheckedRows())
                    {
                        Number += item.Cells["Column01"].Value.ToString() + ",";
                    }
                    if (Number == "")
                    {
                        MessageBox.Show("گزینه ای برای چاپ انتخاب نشده است");
                        return;
                    }
                    _04_Buy.Reports.Form_BuyFactorPrint frm =
                        new Reports.Form_BuyFactorPrint(Number);
                    frm.ShowDialog();
                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
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
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
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
                _05_Sale.Frm_002_ViewPrefactors frm = new _05_Sale.Frm_002_ViewPrefactors();
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
            if (gridEX1.RowCount > 0)
            {
                int SanadId = (gridEX1.GetRow().Cells["Column10"].Text.Trim() == "" ? 0 : int.Parse(gridEX1.GetRow().Cells["Column10"].Value.ToString()));
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
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
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
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



        private void mnu_ViewResid_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
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

        private void gridEX1_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            bt_Edit_Click(sender, e);
        }

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
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

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }



    }
}
