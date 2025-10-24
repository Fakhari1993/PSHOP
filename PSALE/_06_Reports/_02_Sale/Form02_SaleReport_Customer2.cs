using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._06_Reports._02_Sale
{
    public partial class Form02_SaleReport_Customer2 : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        string Date1, Date2;

        public Form02_SaleReport_Customer2()
        {
            InitializeComponent();
        }

        private void Form02_SaleReport_Customer_Load(object sender, EventArgs e)
        {
            crystalReportViewer1.BackColor = Color.White;




            DataTable PersonGroup = clDoc.ReturnTable(ConBase.ConnectionString, @"Select * from(
            Select distinct Tbl2.PersonId, 
            substring((Select ','+Tbl1.GroupName   AS [text()]
            From (SELECT     dbo.Table_045_PersonInfo.ColumnId AS PersonId, ISNULL(dbo.Table_040_PersonGroups.Column01, 'عمومی') AS GroupName
            FROM         dbo.Table_040_PersonGroups INNER JOIN
            dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 RIGHT OUTER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId) Tbl1
            Where Tbl1.PersonId = Tbl2.PersonId
              
            For XML PATH ('')),2, 1000) [PersonGroup]
            From (SELECT     dbo.Table_045_PersonInfo.ColumnId AS PersonId, ISNULL(dbo.Table_040_PersonGroups.Column01, 'عمومی') AS GroupName
            FROM         dbo.Table_040_PersonGroups INNER JOIN
            dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 RIGHT OUTER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId) Tbl2) as PersonGroup");

            gridEX_Customers.DropDowns["PersonGroup"].SetDataBinding(PersonGroup, "");
            gridEX_Customers.DropDowns["Person"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo"), "");
            gridEX_Customers.DropDowns["PersonCode"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo"), "");

            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-2);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            string[] Dates = Properties.Settings.Default.SaleReport_Customer.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
            cmb_Cancel.SelectedIndex = 0;

            //*******************************************

        }

        private void mnu_ExportToExcel_Customers_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Customers;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
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

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                if (cmb_Cancel.ComboBox.SelectedIndex == 0)
                {
                    this.table_010_SaleFactor_ReportCustomer2TableAdapter.FillByAll(this.dataSet_Sale.Table_010_SaleFactor_ReportCustomer2, Date1, Date2);
                }
                else
                {
                    switch (cmb_Cancel.ComboBox.SelectedIndex)
                    {
                        case 1:
                            this.table_010_SaleFactor_ReportCustomer2TableAdapter.FillByCancle(this.dataSet_Sale.Table_010_SaleFactor_ReportCustomer2, Date1, Date2, true);

                            break;
                        case 2:
                            this.table_010_SaleFactor_ReportCustomer2TableAdapter.FillByCancle(this.dataSet_Sale.Table_010_SaleFactor_ReportCustomer2, Date1, Date2, false);

                            break;
                    }
                }
            }
        }



        private void Form02_SaleReport_Customer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print.ShowDropDown();

        }

        private void Form02_SaleReport_Customer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.SaleReport_Customer = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_Customers.RemoveFilters();
        }

        private void gridEX_Customers_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (gridEX_Customers.HitTest(e.X, e.Y) == Janus.Windows.GridEX.GridArea.ColumnHeader)
            {
                Janus.Windows.GridEX.GridEXColumn ColClicked = gridEX_Customers.ColumnFromPoint(e.X, e.Y);
                if (ColClicked.Key == "Total")
                {
                    DataTable Table = dataSet_Report.Rpt_Chart.Clone();
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Customers.GetRows())
                    {
                        Table.Rows.Add(item.Cells["Customer"].Text, item.Cells["Total"].Value.ToString());
                    }
                    if (Table.Rows.Count > 0)
                    {
                        _06_Reports._02_Sale.Rpt02_Charts rpt = new Rpt02_Charts();
                        rpt.SetDataSource(Table);
                        crystalReportViewer1.ReportSource = rpt;
                        uiPanel6.Text = "نمودار-جمع مبلغ فروخته شده به مشتری";
                    }
                }

            }
        }





        private void mnu_PrintThis_Click(object sender, EventArgs e)
        {
            if (gridEX_Customers.GetRows().Count() == 0)
                return;
            DataTable Table = dataSet_Report.Rpt_Sale_CustomerBase.Clone();
            if (cmb_Cancel.ComboBox.SelectedIndex == 0)

                Table = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT tsf.column01 AS FactorNum,
                                                               tsf.column02 [Date],
                                                               tpi2.Column02 AS Customer,
                                                               tpi.Column02 AS Seller,
                                                               0 AS PreFactor,
                                                               0 AS DraftNum,
                                                               0 AS DocNum,
                                                               tsf.column17 AS Cancel,
                                                               tsf.column19 AS [Return],
                                                               0 AS ReturnNum,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) AS NetPrice,
                                                               0 AS SpecGroupDis,
                                                               0 AS SpecCustomer,
                                                               (tsf.Column32) + (tsf. Column34) AS Extra,
                                                               (
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) AS Dis,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) -(
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) + ((tsf.Column32) + (tsf. Column34)) AS FinalPrice,
                                                               '' AS [Description]
                                                        FROM   Table_010_SaleFactor tsf
                                                               LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi
                                                                    ON  tpi.ColumnId = tsf.column05
                                                               LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi2
                                                                    ON  tpi2.ColumnId = tsf.column03
                                                                         WHERE  tsf.column03=" + gridEX_Customers.GetRow().Cells["Customer"].Value + @"
                                                                       AND   (tsf.column02 >= '" + faDatePickerStrip1.FADatePicker.Text + @"')
                                                                                 AND (tsf.column02 <=  '" + faDatePickerStrip2.FADatePicker.Text + @"')");
            else if (cmb_Cancel.ComboBox.SelectedIndex == 1)

                Table = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT tsf.column01 AS FactorNum,
                                                               tsf.column02 [Date],
                                                               tpi2.Column02 AS Customer,
                                                               tpi.Column02 AS Seller,
                                                               0 AS PreFactor,
                                                               0 AS DraftNum,
                                                               0 AS DocNum,
                                                               tsf.column17 AS Cancel,
                                                               tsf.column19 AS [Return],
                                                               0 AS ReturnNum,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) AS NetPrice,
                                                               0 AS SpecGroupDis,
                                                               0 AS SpecCustomer,
                                                               (tsf.Column32) + (tsf. Column34) AS Extra,
                                                               (
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) AS Dis,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) -(
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) + ((tsf.Column32) + (tsf. Column34)) AS FinalPrice,
                                                               '' AS [Description]
                                                        FROM   Table_010_SaleFactor tsf
                                                               LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi
                                                                    ON  tpi.ColumnId = tsf.column05
                                                               LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi2
                                                                    ON  tpi2.ColumnId = tsf.column03
                                                                         WHERE tsf.Column17=1 and   tsf.column03=" + gridEX_Customers.GetRow().Cells["Customer"].Value + @"
                                                                       AND   (tsf.column02 >= '" + faDatePickerStrip1.FADatePicker.Text + @"')
                                                                                 AND (tsf.column02 <=  '" + faDatePickerStrip2.FADatePicker.Text + @"')");
            else

                Table = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT tsf.column01 AS FactorNum,
                                                               tsf.column02 [Date],
                                                               tpi2.Column02 AS Customer,
                                                               tpi.Column02 AS Seller,
                                                               0 AS PreFactor,
                                                               0 AS DraftNum,
                                                               0 AS DocNum,
                                                               tsf.column17 AS Cancel,
                                                               tsf.column19 AS [Return],
                                                               0 AS ReturnNum,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) AS NetPrice,
                                                               0 AS SpecGroupDis,
                                                               0 AS SpecCustomer,
                                                               (tsf.Column32) + (tsf. Column34) AS Extra,
                                                               (
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) AS Dis,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) -(
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) + ((tsf.Column32) + (tsf. Column34)) AS FinalPrice,
                                                               '' AS [Description]
                                                        FROM   Table_010_SaleFactor tsf
                                                               LEFT JOIN "+ConBase.Database+@".dbo.Table_045_PersonInfo tpi
                                                                    ON  tpi.ColumnId = tsf.column05
                                                               LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi2
                                                                    ON  tpi2.ColumnId = tsf.column03
                                                                         WHERE tsf.Column17=0 and   tsf.column03=" + gridEX_Customers.GetRow().Cells["Customer"].Value + @"
                                                                       AND   (tsf.column02 >= '" + faDatePickerStrip1.FADatePicker.Text + @"')
                                                                                 AND (tsf.column02 <=  '" + faDatePickerStrip2.FADatePicker.Text + @"')");


            //foreach (Janus.Windows.GridEX.GridEXRow item in this.gridEX_Customers.GetRows())
            //{
            //    DataRow NewRow = Table.NewRow();
            //    //*************Header Information
            //    NewRow["FactorNum"] = item.Cells["Column01"].Value.ToString();
            //    NewRow["Date"] = item.Cells["Column02"].Value.ToString();
            //    NewRow["Customer"] = gridEX_Customers.GetRow().Cells["Customer"].Text;
            //    NewRow["Seller"] = item.Cells["Column05"].Text.ToString();
            //    NewRow["PreFactor"] = item.Cells["Column07"].Text.ToString();
            //    NewRow["DraftNum"] = item.Cells["Column09"].Text.ToString();
            //    NewRow["DocNum"] = item.Cells["Column10"].Text.ToString();
            //    NewRow["Cancel"] = item.Cells["Column17"].Value.ToString();
            //    NewRow["Return"] = item.Cells["Column19"].Value.ToString();
            //    NewRow["ReturnNum"] = item.Cells["Column20"].Text.ToString();
            //    NewRow["NetPrice"] = item.Cells["Column28"].Value.ToString();
            //    NewRow["VolDis"] = item.Cells["Column29"].Value.ToString();
            //    NewRow["SpecGroupDis"] = item.Cells["Column30"].Value.ToString();
            //    NewRow["SpecCustomer"] = item.Cells["Column31"].Value.ToString();
            //    NewRow["Extra"] = item.Cells["Column32"].Value.ToString();
            //    NewRow["Dis"] = item.Cells["Column33"].Value.ToString();
            //    NewRow["FinalPrice"] = item.Cells["FinalPrice"].Value.ToString();
            //    NewRow["Description"] = item.Cells["Column06"].Text;
            //    Table.Rows.Add(NewRow);
            //}

            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 47, Date1, Date2);
                frm.ShowDialog();
            }
        }

        private void mnu_PrintAll_Click(object sender, EventArgs e)
        {

            DataTable Table = dataSet_Report.Rpt_Sale_CustomerBase.Clone();
            if (cmb_Cancel.ComboBox.SelectedIndex == 0)


                Table = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT tsf.column01 AS FactorNum,
                                                               tsf.column02 [Date],
                                                               tpi2.Column02 AS Customer,
                                                               tpi.Column02 AS Seller,
                                                               0 AS PreFactor,
                                                               0 AS DraftNum,
                                                               0 AS DocNum,
                                                               tsf.column17 AS Cancel,
                                                               tsf.column19 AS [Return],
                                                               0 AS ReturnNum,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) AS NetPrice,
                                                               0 AS SpecGroupDis,
                                                               0 AS SpecCustomer,
                                                               (tsf.Column32) + (tsf. Column34) AS Extra,
                                                               (
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) AS Dis,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) -(
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) + ((tsf.Column32) + (tsf. Column34)) AS FinalPrice,
                                                               '' AS [Description]
                                                        FROM   Table_010_SaleFactor tsf
                                                               LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi
                                                                    ON  tpi.ColumnId = tsf.column05
                                                               LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi2
                                                                    ON  tpi2.ColumnId = tsf.column03
                                                                         WHERE  
                                                                           (tsf.column02 >= '" + faDatePickerStrip1.FADatePicker.Text + @"')
                                                                                 AND (tsf.column02 <=  '" + faDatePickerStrip2.FADatePicker.Text + @"')");


            else if (cmb_Cancel.ComboBox.SelectedIndex == 1)

                Table = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT tsf.column01 AS FactorNum,
                                                               tsf.column02 [Date],
                                                               tpi2.Column02 AS Customer,
                                                               tpi.Column02 AS Seller,
                                                               0 AS PreFactor,
                                                               0 AS DraftNum,
                                                               0 AS DocNum,
                                                               tsf.column17 AS Cancel,
                                                               tsf.column19 AS [Return],
                                                               0 AS ReturnNum,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) AS NetPrice,
                                                               0 AS SpecGroupDis,
                                                               0 AS SpecCustomer,
                                                               (tsf.Column32) + (tsf. Column34) AS Extra,
                                                               (
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) AS Dis,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) -(
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) + ((tsf.Column32) + (tsf. Column34)) AS FinalPrice,
                                                               '' AS [Description]
                                                        FROM   Table_010_SaleFactor tsf
                                                               LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi
                                                                    ON  tpi.ColumnId = tsf.column05
                                                               LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi2
                                                                    ON  tpi2.ColumnId = tsf.column03
                                                                         WHERE tsf.Column17=1  
                                                                       AND   (tsf.column02 >= '" + faDatePickerStrip1.FADatePicker.Text + @"')
                                                                                 AND (tsf.column02 <=  '" + faDatePickerStrip2.FADatePicker.Text + @"')");
            else

                Table = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT tsf.column01 AS FactorNum,
                                                               tsf.column02 [Date],
                                                               tpi2.Column02 AS Customer,
                                                               tpi.Column02 AS Seller,
                                                               0 AS PreFactor,
                                                               0 AS DraftNum,
                                                               0 AS DocNum,
                                                               tsf.column17 AS Cancel,
                                                               tsf.column19 AS [Return],
                                                               0 AS ReturnNum,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) AS NetPrice,
                                                               0 AS SpecGroupDis,
                                                               0 AS SpecCustomer,
                                                               (tsf.Column32) + (tsf. Column34) AS Extra,
                                                               (
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) AS Dis,
                                                               (
                                                                   SELECT SUM(tcsf.column11)
                                                                   FROM   Table_011_Child1_SaleFactor tcsf
                                                                   WHERE  tcsf.column01 = tsf.columnid
                                                               ) -(
                                                                   tsf.Column29 
                                                                   + (tsf.Column30) + (tsf.Column31) + (tsf.Column33) + (tsf.Column35)
                                                               ) + ((tsf.Column32) + (tsf. Column34)) AS FinalPrice,
                                                               '' AS [Description]
                                                        FROM   Table_010_SaleFactor tsf
                                                               LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi
                                                                    ON  tpi.ColumnId = tsf.column05
                                                               LEFT JOIN " + ConBase.Database + @".dbo.Table_045_PersonInfo tpi2
                                                                    ON  tpi2.ColumnId = tsf.column03
                                                                         WHERE tsf.Column17=0  
                                                                       AND   (tsf.column02 >= '" + faDatePickerStrip1.FADatePicker.Text + @"')
                                                                                 AND (tsf.column02 <=  '" + faDatePickerStrip2.FADatePicker.Text + @"')");

            //foreach (Janus.Windows.GridEX.GridEXRow CustomerRow in gridEX_Customers.GetRows())
            //{
            //    gridEX_Customers.MoveTo(CustomerRow);
            //    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Customers.GetRows())
            //    {
            //        DataRow NewRow = Table.NewRow();
            //        //*************Header Information
            //        NewRow["FactorNum"] = item.Cells["Column01"].Value.ToString();
            //        NewRow["Date"] = item.Cells["Column02"].Value.ToString();
            //        NewRow["Customer"] = CustomerRow.Cells["Customer"].Text;
            //        NewRow["Seller"] = item.Cells["Column05"].Text.ToString();
            //        NewRow["PreFactor"] = item.Cells["Column07"].Text.ToString();
            //        NewRow["DraftNum"] = item.Cells["Column09"].Text.ToString();
            //        NewRow["DocNum"] = item.Cells["Column10"].Text.ToString();
            //        NewRow["Cancel"] = item.Cells["Column17"].Value.ToString();
            //        NewRow["Return"] = item.Cells["Column19"].Value.ToString();
            //        NewRow["ReturnNum"] = item.Cells["Column20"].Text.ToString();
            //        NewRow["NetPrice"] = item.Cells["Column28"].Value.ToString();
            //        NewRow["VolDis"] = item.Cells["Column29"].Value.ToString();
            //        NewRow["SpecGroupDis"] = item.Cells["Column30"].Value.ToString();
            //        NewRow["SpecCustomer"] = item.Cells["Column31"].Value.ToString();
            //        NewRow["Extra"] = item.Cells["Column32"].Value.ToString();
            //        NewRow["Dis"] = item.Cells["Column33"].Value.ToString();
            //        NewRow["FinalPrice"] = item.Cells["FinalPrice"].Value.ToString();
            //        NewRow["Description"] = item.Cells["Column06"].Text;
            //        Table.Rows.Add(NewRow);
            //    }
            //}
            if (Table.Rows.Count > 0)
            {
                _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 47, Date1, Date2);
                frm.ShowDialog();
            }
        }

        private void fillByCancleToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void fillByCancleToolStripButton_Click_1(object sender, EventArgs e)
        {
            try
            {
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }




    }
}
