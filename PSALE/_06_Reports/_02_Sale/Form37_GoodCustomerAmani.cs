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
    public partial class Form37_GoodCustomerAmani : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Class_UserScope UserScope = new Class_UserScope();
        bool _BackSpace = false;
        string Date1, Date2;
        public Form37_GoodCustomerAmani()
        {
            InitializeComponent();
        }

        private void Form36_GoodReportByVisitors_Load(object sender, EventArgs e)
        {
            string[] Dates = Properties.Settings.Default.GoodReportByVisitors.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
            gridEX_Goods.DropDowns["Customer"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, @"select Columnid,Column02 from Table_045_PersonInfo");
            gridEX_Goods.DropDowns["Good"].DataSource = clDoc.ReturnTable(ConWare.ConnectionString, @"select   columnid, column01, column02 from table_004_CommodityAndIngredients");
            bt_Search_Click(null, null);




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

        private void Form19_Visitors_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            //else if (e.Control && e.KeyCode == Keys.P)
            //    bt_Print_Click(sender, e);
        }

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                string CommandText = null;
                CommandText = @"SELECT     tbAmani.column03 AS Customer, tbAmani.column02 AS GoodName, ISNULL(tbSale.Count, 0) AS CountSale, ISNULL(tbAmani.Count, 0) AS CountAmani, 
                      ISNULL(tbMarjoAmani.Count, 0) AS CountMarjo, ISNULL(tbAmani.Count, 0) - ISNULL(tbMarjoAmani.Count, 0) - ISNULL(tbSale.Count, 0) AS CountRemain
FROM         (SELECT     SUM(dbo.Table_075_Child_AmaniFactor.column07) AS Count, dbo.Table_075_Child_AmaniFactor.column02, dbo.Table_070_AmaniFactor.column03
                       FROM          dbo.Table_070_AmaniFactor INNER JOIN
                                              dbo.Table_075_Child_AmaniFactor ON dbo.Table_070_AmaniFactor.columnid = dbo.Table_075_Child_AmaniFactor.column01
                       WHERE      (dbo.Table_070_AmaniFactor.column02 >= N'" + Date1 + @"') 
AND (dbo.Table_070_AmaniFactor.column02 <= N'" + Date2 + @"')
                       GROUP BY dbo.Table_075_Child_AmaniFactor.column02, dbo.Table_070_AmaniFactor.column03) AS tbAmani LEFT OUTER JOIN
                          (SELECT     SUM(dbo.Table_085_ReturnAmaniChild.column07) AS Count, dbo.Table_085_ReturnAmaniChild.column02, dbo.Table_080_ReturnAmani.column03
                            FROM          dbo.Table_080_ReturnAmani INNER JOIN
                                                   dbo.Table_085_ReturnAmaniChild ON dbo.Table_080_ReturnAmani.columnid = dbo.Table_085_ReturnAmaniChild.column01
                            GROUP BY dbo.Table_085_ReturnAmaniChild.column02, dbo.Table_080_ReturnAmani.column03) AS tbMarjoAmani ON 
                      tbAmani.column02 = tbMarjoAmani.column02 AND tbAmani.column03 = tbMarjoAmani.column03 LEFT OUTER JOIN
                          (SELECT     SUM(dbo.Table_011_Child1_SaleFactor.column07) AS Count, dbo.Table_011_Child1_SaleFactor.column02, dbo.Table_010_SaleFactor.column03
                            FROM          dbo.Table_010_SaleFactor INNER JOIN
                                                   dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                            WHERE      (dbo.Table_010_SaleFactor.Column56 = 1)
                            GROUP BY dbo.Table_011_Child1_SaleFactor.column02, dbo.Table_010_SaleFactor.column03) AS tbSale ON tbAmani.column02 = tbSale.column02 AND 
                      tbAmani.column03 = tbSale.column03

 ";

                //CommandText = string.Format(CommandText, Date1, Date2,
                //    ConBase.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                gridEX_Goods.DataSource = Table;
             
            }
        }

//        private void bt_Print_Click(object sender, EventArgs e)
//        {
//            if (gridEX_visitors.GetCheckedRows().Count() != 0)
//            {
//                string visitors = string.Empty;
//                string visitorsName = string.Empty;
//                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_visitors.GetCheckedRows())
//                {
//                    visitors += Row.Cells["ColumnId"].Value + ",";
//                    using (SqlConnection ConBASE = new SqlConnection(Properties.Settings.Default.BASE))
//                    {

//                        ConBASE.Open();
//                        SqlCommand Command = new SqlCommand("Select top 1 Column02 from Table_045_PersonInfo where ColumnId=" + Row.Cells["ColumnId"].Value, ConBASE);
//                        visitorsName += Command.ExecuteScalar().ToString() + " ,";

//                    }
//                }
//                visitors = visitors.TrimEnd(',');
//                visitorsName = visitorsName.TrimEnd(',');
//                Date1 = null; Date2 = null;
//                Date1 = faDatePickerStrip1.FADatePicker.Text;
//                Date2 = faDatePickerStrip2.FADatePicker.Text;

//                string CommandText = null;
//                CommandText = @"
//                                            SELECT 
//                                                   tmg.column02 AS MainGroup,
//                                                   tsg.column03 AS SubGroup,
//                                                   tcai.column01 AS GoodCode,
//                                                   tcai.column02 AS GoodName,
//                                                   SUM(tcsf.column07) AS [Count],
//                                                   SUM(tcsf.column11) AS [Amount],
//                                                   SUM(tcsf.column20) AS [NetAmount],
//                                                   SUM(tcsf.column20) / NULLIF(SUM(ISNULL(tcsf.column07, 0)), 0) AS Avrage
//                                            FROM   dbo.Table_010_SaleFactor
//                                                   JOIN Table_011_Child1_SaleFactor tcsf
//                                                        ON  tcsf.column01 = dbo.Table_010_SaleFactor.columnid
//                                                   JOIN " + ConWare.Database+ @".dbo.table_004_CommodityAndIngredients tcai
//                                                        ON  tcai.columnid = tcsf.column02
//                                                   LEFT JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
//                                                        ON  tmg.columnid = tcai.column03
//                                                   LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
//                                                        ON  tsg.column01 = tcai.column03
//                                                        AND tsg.columnid = tcai.column04
//                                                            WHERE  (dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND '{1}') and dbo.Table_010_SaleFactor.column05 in (" + visitors + @")
//                                            GROUP BY
//                                                   tcsf.column02,
//                                                   tmg.column02,
//                                                   tsg.column03,
//                                                   tcai.column01,
//                                             tcai.column02";

//                CommandText = string.Format(CommandText, Date1, Date2,
//                    ConBase.Database);
//                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
//                DataTable Table = new DataTable();
//                Adapter.Fill(Table);


//                if (Table.Rows.Count > 0)
//                {
//                    _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 46, Date1, Date2, visitorsName);
//                    frm.ShowDialog();
//                }
//            }
//        }
        private void Form19_Visitors_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.GoodReportByVisitors = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_Goods.RootTable.Groups.Clear();
            gridEX_Goods.RemoveFilters();
        }

        private void tbl_VitorsBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
              //  bindingSource1.Filter = "Visitor=" + gridEX_visitors.GetValue("ColumnId").ToString();
            }
            catch
            {
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {

        }

    }
}
