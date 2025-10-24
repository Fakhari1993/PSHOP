using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._05_Sale
{
    public partial class Frm_016_SortReturnSaleFactors : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlDataAdapter HeaderAdapter, Child1Adapter, Child2Adapter;
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;

        public Frm_016_SortReturnSaleFactors()
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

            Adapter = new SqlDataAdapter("Select ColumnId,Column00 from Table_060_SanadHead", ConAcnt);
            Adapter.Fill(dataSet1, "Doc");
            gridEX_Header.DropDowns["Doc"].SetDataBinding(dataSet1.Tables["Doc"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_011_PwhrsReceipt", ConWare);
            Adapter.Fill(dataSet1, "Receipt");
            gridEX_Header.DropDowns["Receipt"].SetDataBinding(dataSet1.Tables["Receipt"], "");

            gridEX_Header.DropDowns["Sale"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_010_SaleFactor"), "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_007_FactorBefore", ConSale);
            Adapter.Fill(dataSet1, "Prefactor");
            gridEX_Header.DropDowns["Prefactor"].SetDataBinding(dataSet1.Tables["Prefactor"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
            Adapter.Fill(dataSet1, "Customer");
            gridEX_Header.DropDowns["Customer"].SetDataBinding(dataSet1.Tables["Customer"], "");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_055_CurrencyInfo");
            gridEX_Header.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");


            gridEX_Header.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from ListPeople(5)"), "");

            string[] Dates = Properties.Settings.Default.ViewReturnSale.Split('-');
          

            //*******************************************
            HeaderAdapter = new SqlDataAdapter("Select * from Table_018_MarjooiSale", ConSale);
            HeaderAdapter.Fill(dataSet1, "Header");

            Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_019_Child1_MarjooiSale.*    FROM         dbo.Table_018_MarjooiSale INNER JOIN
            dbo.Table_019_Child1_MarjooiSale ON dbo.Table_018_MarjooiSale.columnid = dbo.Table_019_Child1_MarjooiSale.column01 ", ConSale);
            Child1Adapter.Fill(dataSet1, "Child1");

            Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_020_Child2_MarjooiSale.*    FROM         dbo.Table_018_MarjooiSale INNER JOIN
            dbo.Table_020_Child2_MarjooiSale ON dbo.Table_018_MarjooiSale.columnid = dbo.Table_020_Child2_MarjooiSale.column01", ConSale);
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

            txt_TotalPrice.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column18");
            txt_Extra.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column19");
            txt_Reductions.DataBindings.Add("Value", dataSet1.Tables["Header"], "Column20");
            gridEX_Header.Select();
        }
        private void bt_Search_Click(object sender, EventArgs e)
        {
            {
                dataSet1.EnforceConstraints = false;
                dataSet1.Tables["Header"].Clear();
                dataSet1.Tables["Child1"].Clear();
                dataSet1.Tables["Child2"].Clear();

                HeaderAdapter = new SqlDataAdapter("Select * from Table_018_MarjooiSale", ConSale);
                HeaderAdapter.Fill(dataSet1, "Header");

                Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_019_Child1_MarjooiSale.*    FROM         dbo.Table_018_MarjooiSale INNER JOIN
                 dbo.Table_019_Child1_MarjooiSale ON dbo.Table_018_MarjooiSale.columnid = dbo.Table_019_Child1_MarjooiSale.column01 ", ConSale);
                Child1Adapter.Fill(dataSet1, "Child1");

                Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_020_Child2_MarjooiSale.*    FROM         dbo.Table_018_MarjooiSale INNER JOIN
                dbo.Table_020_Child2_MarjooiSale ON dbo.Table_018_MarjooiSale.columnid = dbo.Table_020_Child2_MarjooiSale.column01 ", ConSale);
                Child2Adapter.Fill(dataSet1, "Child2");

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
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 22))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_013_ReturnFactor")
                            {
                                item.BringToFront();
                                Frm_013_ReturnFactor frm = (Frm_013_ReturnFactor)item;
                                frm.txt_Search.Text = gridEX_Header.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        Frm_013_ReturnFactor frms = new Frm_013_ReturnFactor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 23), Convert.ToInt32(gridEX_Header.GetValue("columnid")));
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
                if (this.gridEX_Header.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 130))
                    {
                        _05_Sale.Reports.Form_ReturnSaleFactorPrint frm = new Reports.Form_ReturnSaleFactorPrint
                            (int.Parse(gridEX_Header.GetRow().Cells["Column01"].Value.ToString()));
                        frm.ShowDialog();
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
                txt_EndPrice.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            if (gridEX_Header.RowCount > 0)
            {
                int SanadId = (gridEX_Header.GetRow().Cells["Column10"].Text.Trim() == "" ? 0 : int.Parse(gridEX_Header.GetRow().Cells["Column10"].Value.ToString()));
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
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
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

        private void gridEX_List_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            try
            {
                if (e.Row.RowType == Janus.Windows.GridEX.RowType.Record && e.Row.Cells["column30"].Value.ToString() == "True")
                    e.Row.RowHeaderImageIndex =2;
            }
            catch { }
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

        private void bt_Sort_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("مرتب سازی، از ابتدای فاکتورها صورت خواهد گرفت و در اثر این عملیات شماره فاکتورها تغییر خواهد کرد" +
                Environment.NewLine + "آیا مایل به مرتب سازی هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
            {
                SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE);
                Con.Open();
                SqlDataAdapter Adapter = new SqlDataAdapter(
                    "select *, row_Number() over (order by column02,ColumnId) as RowNumber from Table_018_MarjooiSale " +
                    " order by column02,ColumnId", Con);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                SqlCommand Update = new SqlCommand(" ", Con);
                foreach (DataRow item in Table.Rows)
                {
                    Update.CommandText = "Update Table_018_MarjooiSale SET column01=" +
                        item["RowNumber"].ToString() + " where ColumnId=" + item["ColumnId"].ToString();
                    Update.ExecuteNonQuery();
                }
                Class_BasicOperation.ShowMsg("", "مرتب سازی فاکتورها با موفقیت صورت گرفت","Information");
                Con.Close();
                bt_Search_Click(null, null);
            }
        }

    }
}
