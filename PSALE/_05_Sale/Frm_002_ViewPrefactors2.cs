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
    public partial class Frm_002_ViewPrefactors2 : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlDataAdapter HeaderAdapter, Child1Adapter, Child2Adapter,FactorAdapter;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;

        public Frm_002_ViewPrefactors2()
        {
            InitializeComponent();
        }

        private void gridEX_Extra_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            try
            {
                if (e.Row.Cells["column05"].Value.ToString() == "True")
                    e.Row.RowHeaderImageIndex = 1;
                else
                    e.Row.RowHeaderImageIndex = 0;
            }
            catch { }
        }

        private void Frm_002_ViewPrefactors_Load(object sender, EventArgs e)
        {
            //SqlDataAdapter Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo", ConBase);
            //Adapter.Fill(dataSet1, "Center");
            //gridEX_List.DropDowns["Center"].SetDataBinding(dataSet1.Tables["Center"], "");

            //Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
            //Adapter.Fill(dataSet1, "Project");
            //gridEX_List.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Project"], "");

            //Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
            //DataTable GoodTable=clGood.GoodInfo();
            //gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            //gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            //Adapter = new SqlDataAdapter("Select * from Table_070_CountUnitInfo", ConBase);
            //Adapter.Fill(dataSet1, "CountUnit");
            //gridEX_List.DropDowns["CountUnit"].SetDataBinding(dataSet1.Tables["CountUnit"], "");

           

            FactorAdapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_010_SaleFactor", ConSale);
            FactorAdapter.Fill(dataSet1, "Factor");
            gridEX_Header.DropDowns["Factor"].SetDataBinding(dataSet1.Tables["Factor"], "");

            gridEX_Header.DropDowns["Prefactor"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01 from Table_005_OrderHeader"), "");
            gridEX_Header.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_002_SalesTypes"), "");

            DataTable PersonTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX_Header.DropDowns["Customer"].SetDataBinding(PersonTable, "");
            gridEX_Header.DropDowns["Seller"].SetDataBinding(PersonTable, "");

            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            //*******************************************
            HeaderAdapter = new SqlDataAdapter("Select * from Table_007_FactorBefore2 where Column02>='"+faDatePickerStrip1.FADatePicker.Text+"' and Column02<='"+faDatePickerStrip2.FADatePicker.Text+"'", ConSale);
            HeaderAdapter.Fill(dataSet1,"Header");

            Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_008_Child1_FactorBefore2.*    FROM         dbo.Table_007_FactorBefore2 INNER JOIN
            dbo.Table_008_Child1_FactorBefore2 ON dbo.Table_007_FactorBefore2.columnid = dbo.Table_008_Child1_FactorBefore2.column00 WHERE     (dbo.Table_007_FactorBefore2.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_007_FactorBefore2.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
            Child1Adapter.Fill(dataSet1, "Child1");

            Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_009_Child2_FactorBefore2.*    FROM         dbo.Table_007_FactorBefore2 INNER JOIN
            dbo.Table_009_Child2_FactorBefore2 ON dbo.Table_007_FactorBefore2.columnid = dbo.Table_009_Child2_FactorBefore2.column00 WHERE     (dbo.Table_007_FactorBefore2.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_007_FactorBefore2.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
            Child2Adapter.Fill(dataSet1, "Child2");

            DataRelation Relation1 = new DataRelation("R_Header_Child1", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child1"].Columns["Column00"]);
            DataRelation Relation2 = new DataRelation("R_Header_Child2", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child2"].Columns["Column00"]);

            ForeignKeyConstraint Fkc1 = new ForeignKeyConstraint("F_Header_Child1", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child1"].Columns["Column00"]);
            ForeignKeyConstraint Fkc2 = new ForeignKeyConstraint("F_Header_Child2", dataSet1.Tables["Header"].Columns["ColumnId"], dataSet1.Tables["Child2"].Columns["Column00"]);
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
            DataTable type = new DataTable();
            type.Columns.Add("ID", typeof(Int32));
            type.Columns.Add("Name", typeof(string));
            type.Rows.Add(0, "اضافه");
            type.Rows.Add(1, "تخفیف");



            gridEX_Extra.DropDowns["d"].DataSource = type;
        }

        private void dataTable_Changed(object sender,System.Data.DataRowChangeEventArgs e)
        {
            MessageBox.Show("yew");
        }
     
        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.Text.Length == 10 && faDatePickerStrip1.FADatePicker.Text.Length == 10)
            {
                dataSet1.EnforceConstraints = false;
                dataSet1.Tables["Header"].Clear();
                dataSet1.Tables["Child1"].Clear();
                dataSet1.Tables["Child2"].Clear();
                dataSet1.Tables["Factor"].Clear();
                HeaderAdapter = new SqlDataAdapter("Select * from Table_007_FactorBefore2 where Column02>='" + faDatePickerStrip1.FADatePicker.Text + "' and Column02<='" + faDatePickerStrip2.FADatePicker.Text + "'", ConSale);
                HeaderAdapter.Fill(dataSet1, "Header");

                Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_008_Child1_FactorBefore2.*    FROM         dbo.Table_007_FactorBefore2 INNER JOIN
                 dbo.Table_008_Child1_FactorBefore2 ON dbo.Table_007_FactorBefore2.columnid = dbo.Table_008_Child1_FactorBefore2.column00 WHERE     (dbo.Table_007_FactorBefore2.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_007_FactorBefore2.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
                Child1Adapter.Fill(dataSet1, "Child1");

                Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_009_Child2_FactorBefore2.*    FROM         dbo.Table_007_FactorBefore2 INNER JOIN
                dbo.Table_009_Child2_FactorBefore2 ON dbo.Table_007_FactorBefore2.columnid = dbo.Table_009_Child2_FactorBefore2.column00 WHERE     (dbo.Table_007_FactorBefore2.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_007_FactorBefore2.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
                Child2Adapter.Fill(dataSet1, "Child2");

                FactorAdapter.Fill(dataSet1, "Factor");
                
                dataSet1.EnforceConstraints = true;
            }
        }

        private void Frm_002_ViewPrefactors_KeyDown(object sender, KeyEventArgs e)
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

        private void bt_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridEX_Header.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 18))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Frm_001_PishFaktor2")
                            {
                                item.BringToFront();
                                Frm_001_PishFaktor2 frm = (Frm_001_PishFaktor2)item;
                                frm.txt_Search.Text = gridEX_Header.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        Frm_001_PishFaktor2 frms = new Frm_001_PishFaktor2(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 19), Convert.ToInt32(gridEX_Header.GetValue("columnid")));
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
                if (this.gridEX_Header.RowCount > 0)
                {
                    _05_Sale.Reports.Form_PreFactorPrint2 frm = new Reports.Form_PreFactorPrint2
                        (int.Parse(gridEX_Header.GetRow().Cells["Column01"].Value.ToString()));
                    frm.ShowDialog();
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
                txt_Factor.Value = gridEX_List.GetTotalRow().Cells["Column05"].Value.ToString();
                Janus.Windows.GridEX.GridEXFilterCondition Fitler1 = new Janus.Windows.GridEX.GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column04"], Janus.Windows.GridEX.ConditionOperator.Equal, "اضافه");
                txt_Reduction.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column03"], Janus.Windows.GridEX.AggregateFunction.Sum, Fitler1);
                Fitler1.Clear();
                Fitler1 = new Janus.Windows.GridEX.GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column04"], Janus.Windows.GridEX.ConditionOperator.Equal, "تخفیف");
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column03"], Janus.Windows.GridEX.AggregateFunction.Sum, Fitler1);
                txt_EndOfFactor.Value = Int64.Parse(txt_Factor.Value.ToString()) + Int64.Parse(txt_Extra.Value.ToString()) - Int64.Parse(txt_Reduction.Value.ToString());
            }
            catch 
            {
            }
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

        private void gridEX_Header_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            bt_Edit_Click(sender, e);
        }

        private void Frm_002_ViewPrefactors2_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Header.RemoveFilters();
            gridEX_List.RemoveFilters();
        }
    }
}
