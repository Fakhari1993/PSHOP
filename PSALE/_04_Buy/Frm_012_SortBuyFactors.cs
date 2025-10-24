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
    public partial class Frm_012_SortBuyFactors : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlDataAdapter HeaderAdapter, Child1Adapter, Child2Adapter,ResidAdapter,DocAdapter,ReturnAdapter;
        DataSet DS = new DataSet();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;

        public Frm_012_SortBuyFactors()
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
            ResidAdapter = new SqlDataAdapter("SELECT ColumnId,Column01 from Table_011_PwhrsReceipt", ConWare);
            ResidAdapter.Fill(DS, "Resid");
            gridEX1.DropDowns["Resid"].SetDataBinding(DS.Tables["Resid"], "");

            DocAdapter = new SqlDataAdapter("Select ColumnId,Column00 from Table_060_SanadHead", ConAcnt);
            DocAdapter.Fill(DS, "Doc");
            gridEX1.DropDowns["Doc"].SetDataBinding(DS.Tables["Doc"], "");

            ReturnAdapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_021_MarjooiBuy", ConSale);
            ReturnAdapter.Fill(DS, "Return");
            gridEX1.DropDowns["Return"].SetDataBinding(DS.Tables["Return"], "");

            DataTable GoodTable = clGood.GoodInfo();
            gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from ListPeople(5)"), "");

            SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(DS, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 FROM Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(DS, "Center");
            gridEX_List.DropDowns["Center"].SetDataBinding(DS.Tables["Center"], "");

            Adapter = new SqlDataAdapter("SELECT Column00,Column01,Column02 FROM Table_035_ProjectInfo", ConBase);
            Adapter.Fill(DS, "Project");
            gridEX_List.DropDowns["Project"].SetDataBinding(DS.Tables["Project"], "");

            gridEX1.DropDowns["Buyer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,4)"), "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount_Buy", ConSale);
            Adapter.Fill(DS, "Discount");
            gridEX_Extra.DropDowns["Extra"].SetDataBinding(DS.Tables["Discount"], "");

            Adapter = new SqlDataAdapter("select ColumnId,Column01 from Table_013_RequestBuy", ConSale);
            Adapter.Fill(DS, "Request");
            gridEX_List.DropDowns["Request"].SetDataBinding(DS.Tables["Request"], "");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_055_CurrencyInfo");
            gridEX1.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");

            string[] Dates = Properties.Settings.Default.ViewBuyFactors.Split('-');
            

            //*******************************************
            HeaderAdapter = new SqlDataAdapter("Select * from Table_015_BuyFactor", ConSale);
            HeaderAdapter.Fill(DS, "Header");

            Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_016_Child1_BuyFactor.*    FROM         dbo.Table_015_BuyFactor INNER JOIN
            dbo.Table_016_Child1_BuyFactor ON dbo.Table_015_BuyFactor.columnid = dbo.Table_016_Child1_BuyFactor.column01", ConSale);
            Child1Adapter.Fill(DS, "Child1");

            Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_017_Child2_BuyFactor.*    FROM         dbo.Table_015_BuyFactor INNER JOIN
            dbo.Table_017_Child2_BuyFactor ON dbo.Table_015_BuyFactor.columnid = dbo.Table_017_Child2_BuyFactor.column01 ", ConSale);
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
            gridEX1.Select();
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            {
                DS.EnforceConstraints = false;
                DS.Tables["Header"].Clear();
                DS.Tables["Child1"].Clear();
                DS.Tables["Child2"].Clear();
                DS.Tables["Doc"].Clear();
                DS.Tables["Return"].Clear();
                DS.Tables["Resid"].Clear();

                DocAdapter.Fill(DS, "Doc");
                ReturnAdapter.Fill(DS, "Return");
                ResidAdapter.Fill(DS, "Resid");

                HeaderAdapter = new SqlDataAdapter("Select * from Table_015_BuyFactor ", ConSale);
                HeaderAdapter.Fill(DS, "Header");

                Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_016_Child1_BuyFactor.*    FROM         dbo.Table_015_BuyFactor INNER JOIN
                 dbo.Table_016_Child1_BuyFactor ON dbo.Table_015_BuyFactor.columnid = dbo.Table_016_Child1_BuyFactor.column01 ", ConSale);
                Child1Adapter.Fill(DS, "Child1");

                Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_017_Child2_BuyFactor.*    FROM         dbo.Table_015_BuyFactor INNER JOIN
                dbo.Table_017_Child2_BuyFactor ON dbo.Table_015_BuyFactor.columnid = dbo.Table_017_Child2_BuyFactor.column01 ", ConSale);
                Child2Adapter.Fill(DS, "Child2");

                DS.EnforceConstraints = true;
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
            gridEX_List.RemoveFilters();
            gridEX1.RemoveFilters();
           
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
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 129))
                {
                    _04_Buy.Reports.Form_BuyFactorPrint frm =
                        new Reports.Form_BuyFactorPrint(int.Parse(gridEX1.GetValue("Column01").ToString()).ToString());
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

        private void bt_Sort_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("مرتب سازی، از ابتدای فاکتورها صورت خواهد گرفت و در اثر این عملیات شماره فاکتورها تغییر خواهد کرد" +
                Environment.NewLine + "آیا مایل به مرتب سازی هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
            {
                SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE);
                Con.Open();
                SqlDataAdapter Adapter = new SqlDataAdapter(
                    "select *, row_Number() over (order by column02,ColumnId) as RowNumber from Table_015_BuyFactor " +
                    " order by column02,ColumnId", Con);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                SqlCommand Update = new SqlCommand(" ", Con);
                foreach (DataRow item in Table.Rows)
                {
                    Update.CommandText = "Update Table_015_BuyFactor SET column01=" +
                        item["RowNumber"].ToString() + " where ColumnId=" + item["ColumnId"].ToString();
                    Update.ExecuteNonQuery();
                }
                Class_BasicOperation.ShowMsg("", "مرتب سازی اسناد با موفقیت صورت گرفت", "Information");
                Con.Close();
                bt_Search_Click(null, null);
            }
        }

      

    }
}
