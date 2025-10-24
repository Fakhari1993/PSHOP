using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._07_Services
{
    public partial class Form03_ViewServiceFactors : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlDataAdapter HeaderAdapter, Child1Adapter, Child2Adapter;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;

        public Form03_ViewServiceFactors()
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
                gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_070_CountUnitInfo"), "");
                gridEX_List.DropDowns["Service"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_030_Services"), "");
                gridEX_Extra.DropDowns["Extra"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount"), "");
                gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");
                gridEX_Header.DropDowns["Customer"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT   * from ListPeople(5)"), "");

                string[] Dates = Properties.Settings.Default.ViewServiceFactors.Split('-');
                faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
                faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);

                //*******************************************
                HeaderAdapter = new SqlDataAdapter("Select * from Table_031_ServiceFactor where Column02>='" + faDatePickerStrip1.FADatePicker.Text + "' and Column02<='" + faDatePickerStrip2.FADatePicker.Text + "'", ConSale);
                HeaderAdapter.Fill(dataSet1, "Header");

                Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_032_ServiceFactor_Child1.*    FROM         dbo.Table_032_ServiceFactor_Child1 INNER JOIN
            dbo.Table_031_ServiceFactor ON dbo.Table_031_ServiceFactor.columnid = dbo.Table_032_ServiceFactor_Child1.column01 WHERE     (dbo.Table_031_ServiceFactor.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_031_ServiceFactor.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
                Child1Adapter.Fill(dataSet1, "Child1");

                Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_033_ServiceFactor_Child2.*    FROM         dbo.Table_033_ServiceFactor_Child2 INNER JOIN
            dbo.Table_031_ServiceFactor ON dbo.Table_031_ServiceFactor.columnid = dbo.Table_033_ServiceFactor_Child2.column01 WHERE     (dbo.Table_031_ServiceFactor.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_031_ServiceFactor.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
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
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.Text.Length == 10 && faDatePickerStrip1.FADatePicker.Text.Length == 10)
            {
                dataSet1.EnforceConstraints = false;
                dataSet1.Tables["Header"].Clear();
                dataSet1.Tables["Child1"].Clear();
                dataSet1.Tables["Child2"].Clear();
                HeaderAdapter = new SqlDataAdapter("Select * from Table_031_ServiceFactor where Column02>='" + faDatePickerStrip1.FADatePicker.Text + "' and Column02<='" + faDatePickerStrip2.FADatePicker.Text + "'", ConSale);
                HeaderAdapter.Fill(dataSet1, "Header");

                Child1Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_032_ServiceFactor_Child1.*    FROM         dbo.Table_031_ServiceFactor INNER JOIN
                 dbo.Table_032_ServiceFactor_Child1 ON dbo.Table_031_ServiceFactor.columnid = dbo.Table_032_ServiceFactor_Child1.column01 WHERE     (dbo.Table_031_ServiceFactor.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_031_ServiceFactor.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
                Child1Adapter.Fill(dataSet1, "Child1");

                Child2Adapter = new SqlDataAdapter(@"SELECT     dbo.Table_033_ServiceFactor_Child2.*    FROM         dbo.Table_031_ServiceFactor INNER JOIN
                dbo.Table_033_ServiceFactor_Child2 ON dbo.Table_031_ServiceFactor.columnid = dbo.Table_033_ServiceFactor_Child2.column01 WHERE     (dbo.Table_031_ServiceFactor.column02 >='" + faDatePickerStrip1.FADatePicker.Text + "' and dbo.Table_031_ServiceFactor.column02<='" + faDatePickerStrip2.FADatePicker.Text + "')", ConSale);
                Child2Adapter.Fill(dataSet1, "Child2");
                gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_070_CountUnitInfo"), "");
                gridEX_List.DropDowns["Service"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_030_Services"), "");
                gridEX_Extra.DropDowns["Extra"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount"), "");
                gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");

            
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

        private void Frm_002_ViewPrefactors_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Extra.RemoveFilters();
            gridEX_Header.RemoveFilters();
            gridEX_List.RemoveFilters();
    
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.ViewServiceFactors = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
        }

        private void bt_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridEX_Header.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 106))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Form02_RegisterServiceFactor")
                            {
                                item.BringToFront();
                                Form02_RegisterServiceFactor frm = (Form02_RegisterServiceFactor)item;
                                frm.txt_Search.Text = gridEX_Header.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        Form02_RegisterServiceFactor frms = new Form02_RegisterServiceFactor(int.Parse(gridEX_Header.GetValue("ColumnId").ToString()), UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 107));
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
                    try
                    {
                        //مشتری
                        string CommandText = @"select Column02 as Name,Column01 as Code,Column09 as Economic,
                         Column07 as Tel,Column09 as NationalCode,Column13 as PostalCode,Column06 as Address from {0}.dbo.Table_045_PersonInfo 
                         where ColumnId=" + gridEX_Header.GetValue("Column03").ToString();
                        CommandText = string.Format(CommandText, ConBase.Database);

                        DataTable CustomerTable = clDoc.ReturnTable(ConBase.ConnectionString, CommandText);

                        //خدمات
                        DataTable ItemTable = dataset_Services.Rpt_PrintFactor_Items.Clone();
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                        {
                            ItemTable.Rows.Add(item.Cells["Column02"].Text, item.Cells["Column04"].Text,
                                item.Cells["Column03"].Value.ToString(),
                                item.Cells["Column05"].Value.ToString(),
                                item.Cells["Column06"].Value.ToString(),
                                item.Cells["Column07"].Text.Trim(),
                                gridEX_Header.GetValue("Column01").ToString(),
                                gridEX_Header.GetValue("Column02").ToString(),
                                gridEX_Header.GetValue("Column04").ToString(),
                                txt_Extra.Value, txt_Reduction.Value);
                        }

                        //اضافات و کسورات
                        DataTable ExReTable = dataset_Services.Rpt_PrintFactor_ExtraReducitons.Clone();
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                        {
                            ExReTable.Rows.Add(item.Cells["Column02"].Text,
                                (item.Cells["Column05"].Value.ToString() == "True" ? "-" : "+"),
                                item.Cells["Column03"].Value.ToString(), item.Cells["Column04"].Value.ToString());
                        }

                        _07_Services.Reports.PrintFactor frm = new _07_Services.Reports.PrintFactor(2, ItemTable, CustomerTable, ExReTable,
                            FarsiLibrary.Utils.ToWords.ToString(Int64.Parse(txt_EndOfFactor.Value.ToString()))," ",Convert.ToInt32( gridEX_Header.GetValue("Column01")));
                        frm.ShowDialog();
                    }
                    catch
                    {
                    }
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
                txt_Factor.Value = gridEX_List.GetTotalRow().Cells["Column06"].Value.ToString();
                Janus.Windows.GridEX.GridEXFilterCondition Fitler1=new Janus.Windows.GridEX.GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], Janus.Windows.GridEX.ConditionOperator.Equal,true);
                txt_Reduction.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], Janus.Windows.GridEX.AggregateFunction.Sum, Fitler1);
                Fitler1.Clear();
                Fitler1 = new Janus.Windows.GridEX.GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], Janus.Windows.GridEX.ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], Janus.Windows.GridEX.AggregateFunction.Sum, Fitler1);
                txt_EndOfFactor.Value = Int64.Parse(txt_Factor.Value.ToString()) + Int64.Parse(txt_Extra.Value.ToString()) - Int64.Parse(txt_Reduction.Value.ToString());
            }
            catch 
            {
            }
        }

      
    }
}
