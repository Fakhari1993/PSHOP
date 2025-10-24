using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace PSHOP._06_Reports._02_Sale
{
    public partial class Form44_SaleReport_Customer_Visitor : Form
    {
        bool _BackSpace = false;

        public Form44_SaleReport_Customer_Visitor()
        {
            InitializeComponent();
        }
        Classes.Class_Documents ClsDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);


        private void Form04_SaleReport_NumericMonthly_Visitor_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            DataTable PersonTable = ClsDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            gridEX_Visitors.DropDowns["Seller1"].SetDataBinding(PersonTable, "");
            gridEX_Visitors.DropDowns["Seller2"].SetDataBinding(PersonTable, "");
            gridEX_Goods.DropDowns["CustomerCode"].SetDataBinding(PersonTable, "");
            gridEX_Goods.DropDowns["CustomerName"].SetDataBinding(PersonTable, "");

            gridEX1.DropDowns["CustomerCode"].SetDataBinding(PersonTable, "");
            gridEX1.DropDowns["CustomerName"].SetDataBinding(PersonTable, "");

            DataTable PersonGroup = ClsDoc.ReturnTable(ConBase.ConnectionString, @"Select * from(
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

            gridEX_Goods.DropDowns["PersonGroup"].SetDataBinding(PersonGroup, "");
            gridEX1.DropDowns["PersonGroup"].SetDataBinding(PersonGroup, "");

            crystalReportViewer1.BackColor = Color.White;
            bt_Search_Click(null, null);


        }

        private void mnu_Excel_Table_Click(object sender, EventArgs e)
        {

        }

        private void mnu_Send_Chart_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.ExportReport();
        }


        private void mnu_PrintAll_Click(object sender, EventArgs e)
        {
            //DataTable Table = dataSet_Report.Rpt_Customer_Visitor.Clone();
            //foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_Visitors.GetCheckedRows())
            //{
            //    gridEX_Visitors.MoveTo(Row);
            //    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetRows())
            //    {
            //        Table.Rows.Add(
            //       Row.Cells["VisitorId"].Value.ToString(),
            //            item.Cells["CustomerId"].Text.ToString(),
            //            item.Cells["Net"].Text.ToString(),
            //            item.Cells["Marjoo"].Text.ToString(),
            //            item.Cells["Khales"].Text.ToString(),
            //            item.Cells["CustomerCode"].Text.ToString(),
            //            item.Cells["CustomerName"].Text,
            //            Row.Cells["VisitorCode"].Text.ToString(),
            //            Row.Cells["VisitorName"].Text.ToString());
            //    }
            //}
            //if (Table.Rows.Count > 0)
            //{
            //    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 48, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
            //    frm.ShowDialog();
            //}
            if (gridEX1.Focused)
            {
                gridEXPrintDocument1.GridEX = this.gridEX1;
                string name = string.Empty;
                foreach (Janus.Windows.GridEX.GridEXRow Row in this.gridEX_Visitors.GetCheckedRows())
                {
                    name += Row.Cells["VisitorId"].Text + ",";
                }
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        //  string j = " تاریخ تهیه گزارش:" + FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");
                        gridEXPrintDocument1.PageHeaderLeft = "ازتاریخ: " + faDatePickerStrip1.FADatePicker.Text + "تاتاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageHeaderRight = " نام مسئول فروش: " + name.TrimEnd(',');
                        gridEXPrintDocument1.PageHeaderCenter = "گزارش فروش مشتریان";
                        printPreviewDialog1.ShowDialog();
                    }
            }
            if (gridEX_Goods.Focused)
            {
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        gridEXPrintDocument1.GridEX = this.gridEX_Goods;

                        //  string j = " تاریخ تهیه گزارش:" + FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");
                        gridEXPrintDocument1.PageHeaderLeft = "ازتاریخ: " + faDatePickerStrip1.FADatePicker.Text + "تاتاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageHeaderRight = " نام مسئول فروش: " + gridEX_Visitors.CurrentRow.Cells["VisitorId"].Text;
                        gridEXPrintDocument1.PageHeaderCenter = "گزارش فروش مشتریان";
                        printPreviewDialog1.ShowDialog();
                    }
            }
        }

        private void Form04_SaleReport_NumericMonthly_Goods_Visitor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                mnu_PrintAll_Click(sender, e);
        }

        private void gridEX_Goods_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            //try
            //{
            //    DataTable Table = dataSet_Report.Rpt_Monthly_Numeric.Clone();
            //    for (int i = 3; i <= 14; i++)
            //    {
            //        Table.Rows.Add(gridEX_Goods.RootTable.Columns[i].Caption, e.Row.Cells[i].Value.ToString());
            //    }
            //    if (Table.Rows.Count > 0)
            //    {
            //        _06_Reports._02_Sale.Rpt05_Monthly_Charts rpt = new Rpt05_Monthly_Charts();
            //        rpt.SetDataSource(Table);
            //        TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text3"];
            //        Title.Text = "نمودار ماهیانه میزان فروش ریالی   " + e.Row.Cells["CustomerName"].Text;
            //        crystalReportViewer1.ReportSource = rpt;
            //        uiPanel0.AutoHideActive = true;
            //    }
            //}
            //catch
            //{
            //}
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                dataSet_Report.EnforceConstraints = false;

                this.rpt_CustomerTableAdapter.Fill(this.dataSet_Report.Rpt_Customer, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
                this.rpt_Customer_VisitorTableAdapter.Fill(this.dataSet_Report.Rpt_Customer_Visitor, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
                dataSet_Report.EnforceConstraints = true;

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

        private void faDatePickerStrip2_TextChanged(object sender, EventArgs e)
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

        private void cmb_ExportToExcel_Click(object sender, EventArgs e)
        {
            if (gridEX_Goods.Focused)
                gridEXExporter1.GridEX = gridEX_Goods;
            if (gridEX1.Focused)
                gridEXExporter1.GridEX = gridEX1;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if (gridEX_Visitors.GetCheckedRows().Count() != 0)
            {
                string visitors = string.Empty;

                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_Visitors.GetCheckedRows())
                {
                    visitors += Row.Cells["VisitorId"].Value + ",";

                }
                visitors = visitors.TrimEnd(',');

                if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                {
                    dataSet_Report.EnforceConstraints = false;

                    //this.rpt_CustomerTableAdapter.Fill(this.dataSet_Report.Rpt_Customer, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text);
                    //this.rpt_Customer_Visitor2TableAdapter.FillBy(this.dataSet_Report.Rpt_Customer_Visitor2, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, visitors);


                    string CommandText = null;


                    CommandText = @"/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.0.86
 * Time: 02/25/2019 09:10:27 Þ.Ù
 ************************************************************/

SELECT VisitorId,
       CustomerID,
       SUM(ISNULL(Net, 0)) AS Net,
       SUM(ISNULL(Marjoo, 0)) AS Marjoo,
       SUM(ISNULL(Net, 0)) - SUM(ISNULL(Marjoo, 0)) AS Khales
FROM   (
           SELECT  columnid,column05 AS VisitorId,
                  column03 AS CustomerID,
                  (
                      (dbo.Table_010_SaleFactor.Column28) 
                      -(dbo.Table_010_SaleFactor.Column29) -(dbo.Table_010_SaleFactor.Column30) 
                      -(dbo.Table_010_SaleFactor.Column31) 
                      + (dbo.Table_010_SaleFactor.Column32) -(dbo.Table_010_SaleFactor.Column33)
                  ) AS Net,
                  0 AS Marjoo
           FROM   Table_010_SaleFactor
           WHERE  (column12 = 0)
                  AND (NOT (column05 IS NULL))
                  AND (LEN(column05) > 0)
                  AND column17 = 0
                  AND column19 = 0
           AND column02 >= '{0}'
           AND column02 <= '{1}' AND column05 in (" + visitors + @")
           
           UNION all
           
           SELECT columnid,column05 AS VisitorId,
                  column03 AS CustomerID,
                  0 AS Net,
                  (
                      Table_018_MarjooiSale.Column18 + Table_018_MarjooiSale.Column19 
                      -Table_018_MarjooiSale.Column20
                  ) AS Marjoo
           FROM   Table_018_MarjooiSale
           WHERE  (column12 = 0)
                  AND (NOT (column05 IS NULL))
                  AND (LEN(column05) > 0)
                  
                      AND column02 >= '{0}'
                      AND column02 <= '{1}' AND column05 in (" + visitors + @")
       ) AS Step1
GROUP BY
       CustomerID,
       VisitorId";

                    CommandText = string.Format(CommandText, faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text,
                        ConBase.Database);
                    SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                    DataTable Table = new DataTable();
                    Adapter.Fill(Table);
                    rpt_Customer_Visitor2BindingSource.DataSource = Table;






                    dataSet_Report.EnforceConstraints = true;

                }

            }
            else
            {
                DataTable dt = new DataTable();
                rpt_Customer_Visitor2BindingSource.DataSource = dt;
            }
        }

        private void Form44_SaleReport_Customer_Visitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Goods.RemoveFilters();
            gridEX1.RemoveFilters();
            gridEX_Visitors.RemoveFilters();
        }





    }
}
