using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace PSHOP._08_Order2
{
    public partial class Form08_TotalReport : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        string Date1 = null, Date2 = null;
        DataTable Person = new DataTable();
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents ClsDoc = new Classes.Class_Documents();
        public Form08_TotalReport()
        {
            InitializeComponent();
        }

        private void Form08_TotalReport_Load(object sender, EventArgs e)
        {
           

            Person = ClsDoc.ReturnTable(ConBase.ConnectionString, "SELECT     ColumnId, Column01, Column02 FROM         dbo.Table_045_PersonInfo");

            gridEX1.DropDowns["Customer"].SetDataBinding(Person, "");
           
            // clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            SqlDataAdapter Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients", ConWare);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            gridEX1.DropDowns["GoodCode"].SetDataBinding(Table, "");
            gridEX1.DropDowns["GoodName"].SetDataBinding(Table, "");
             
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

                this.Cursor = Cursors.WaitCursor;
                SqlDataAdapter Adapter = new SqlDataAdapter(@"
                                                                       SELECT Table_005_OrderHeader.column01 AS num,
                                                                       Table_005_OrderHeader.column02 AS date,
                                                                       dbo.Table_005_OrderHeader.column03 AS CustomerID,
                                                                       GoodTable.MainGroup,
                                                                       GoodTable.SubGroup,
                                                                       GoodTable.column02 AS kalaname,
                                                                       dbo.Table_006_OrderDetails.column02 AS GoodID,
                                                                       (dbo.Table_006_OrderDetails.column03) AS Order_Pack,
                                                                       (dbo.Table_006_OrderDetails.column04) AS Order_Box,
                                                                       (dbo.Table_006_OrderDetails.column05) AS Order_Detail,
                                                                       (dbo.Table_006_OrderDetails.column06) AS Order_Total,
                                                                       (dbo.Table_006_OrderDetails.column14) AS Exit_Detail,
                                                                       (dbo.Table_006_OrderDetails.column15) AS Exit_Pack,
                                                                       (dbo.Table_006_OrderDetails.column16) AS Exit_Box,
                                                                       (dbo.Table_006_OrderDetails.column17) AS Exit_Total,
                                                                        dbo.Table_006_OrderDetails.column08 AS DetailCost,
                                                                       dbo.Table_006_OrderDetails.column09 AS PackCost,
                                                                       dbo.Table_006_OrderDetails.column10 AS BoxCost,
                                                                       dbo.Table_006_OrderDetails.column13 AS Total,
                                                                          (dbo.Table_006_OrderDetails.column04) - (dbo.Table_006_OrderDetails.column16) AS 
                                                                       BoxDiff,
                                                                        (dbo.Table_006_OrderDetails.column03) - (dbo.Table_006_OrderDetails.column15) AS 
                                                                       PackDiff,
                                                                        (dbo.Table_006_OrderDetails.column06) - (dbo.Table_006_OrderDetails.column17) AS 
                                                                       TotalDiff,
                                                                dbo.Table_006_OrderDetails.Column33,
                                                                dbo.Table_006_OrderDetails.columnid
                                                                FROM   dbo.Table_005_OrderHeader
                                                                       INNER JOIN dbo.Table_006_OrderDetails
                                                                            ON  dbo.Table_005_OrderHeader.columnid = dbo.Table_006_OrderDetails.column01
                                                                       LEFT OUTER JOIN (
                                                                                SELECT {2}.dbo.table_004_CommodityAndIngredients.columnid,
                                                                                       {2}.dbo.table_004_CommodityAndIngredients.column01,
                                                                                       {2}.dbo.table_004_CommodityAndIngredients.column02,
                                                                                       {2}.dbo.table_002_MainGroup.column02 AS MainGroup,
                                                                                       {2}.dbo.table_003_SubsidiaryGroup.column03 AS SubGroup
                                                                                FROM   {2}.dbo.table_004_CommodityAndIngredients
                                                                                       INNER JOIN {2}.dbo.table_003_SubsidiaryGroup
                                                                                            ON  {2}.dbo.table_004_CommodityAndIngredients.column04 = 
                                                                                                {2}.dbo.table_003_SubsidiaryGroup.columnid
                                                                                       INNER JOIN {2}.dbo.table_002_MainGroup
                                                                                            ON  {2}.dbo.table_003_SubsidiaryGroup.column01 = {2}.dbo.table_002_MainGroup.columnid
                                                                            ) AS GoodTable
                                                                            ON  dbo.Table_006_OrderDetails.column02 = GoodTable.columnid
                                                                WHERE  (dbo.Table_005_OrderHeader.column02 >= '{0}') AND dbo.Table_005_OrderHeader.column13=0 
                                                                       AND (dbo.Table_005_OrderHeader.column02 <= '{1}')", ConSale);
                Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText,
                    Date1, Date2, ConWare.Database);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                gridEX1.DataSource = Table;
               
               
                this.Cursor = Cursors.Default;
            }
        }

        private void Form08_TotalReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();

        }

        private void Form08_TotalReport_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form08_TotalReport_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Control && e.KeyCode == Keys.P)
            //    bt_Print.ShowDropDown();
           
             if (e.Control && e.KeyCode == Keys.D)
                bt_Search_Click(sender, e);
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream File = (FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);

                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام شده", "Information");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    string j = " تاریخ از:" + faDatePickerStrip1.FADatePicker.Text + " " + "تاریخ تا" + faDatePickerStrip2.FADatePicker.Text;
                    gridEXPrintDocument1.PageHeaderLeft = j;
                    gridEXPrintDocument1.PageHeaderRight = "گزارش جامع سفارشات";
                    printPreviewDialog1.ShowDialog();
                }
        }

        private void gridEX1_ColumnButtonClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {

            try
            {
                if (e.Column.Key == "Add")
                {
                    clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_006_OrderDetails SET  Column33=0 , column26='" + Class_BasicOperation._UserName + "',column25=getdate() where ColumnId=" + gridEX1.GetValue("columnid"));

                    bt_Search_Click(null, null);
                }
                if (e.Column.Key == "Cancle")
                {
                    clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_006_OrderDetails SET  Column33=1 , column26='" + Class_BasicOperation._UserName + "',column25=getdate() where ColumnId=" + gridEX1.GetValue("columnid"));

                    bt_Search_Click(null, null);

                }



            }
            catch(Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
        }
    }
}
