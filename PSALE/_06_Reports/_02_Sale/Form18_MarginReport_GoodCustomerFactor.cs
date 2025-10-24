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
    public partial class Form18_MarginReport_GoodCustomerFactor : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        bool _BackSpace = false;
        string Date1, Date2;

        public Form18_MarginReport_GoodCustomerFactor()
        {
            InitializeComponent();
        }

        private void Form18_MarginReport_GoodCustomerFactor_Load(object sender, EventArgs e)
        {
            DataTable GoodTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients");
            gridEX1.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX1.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            DataTable CustomerTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Columnid,Column01,column02 from Table_045_PersonInfo");
            gridEX1.DropDowns["PersonCode"].SetDataBinding(CustomerTable, "");
            gridEX1.DropDowns["PersonName"].SetDataBinding(CustomerTable, "");

            //faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            //faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            string[] Dates = Properties.Settings.Default.GoodCustomerFactor.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
            gridEX1.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes"), "");

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
                    bt_Display_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                Date1 = null; Date2 = null;
                Date1 = faDatePickerStrip1.FADatePicker.Text;
                Date2 = faDatePickerStrip2.FADatePicker.Text;

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT factor.*,
                                                                               ChildDraftTable.column15 AS SingleValue,
                                                                               ChildDraftTable.column16 AS TotalValue,
                                                                               factor.TotalPrice - ChildDraftTable.column16 AS SingleMargin,
                                                                             (  (factor.TotalPrice - ChildDraftTable.column16)/nullif(ISNULL(factor.Number, 0),0)) as UMRatio ,
                                                                           isnull( ( (  (factor.TotalPrice - ChildDraftTable.column16)/nullif(ISNULL(factor.Number, 0),0))/nullif(ChildDraftTable.column15,0)),0) *100 as DRatio      
    
                                                                        FROM   (
                                                                                   SELECT ROW_NUMBER() OVER( PARTITION BY Table_011_Child1_SaleFactor.column01
                                                                                              ORDER BY Table_011_Child1_SaleFactor.columnid,
                                                                                              Table_011_Child1_SaleFactor.column02
                                                                                          ) AS Row,
                                                                                          dbo.Table_010_SaleFactor.columnid AS FactorID,
                                                                                          dbo.Table_010_SaleFactor.column01 AS FactorNumber,
                                                                                          dbo.Table_010_SaleFactor.column02 AS FactorDate,
                                                                                          dbo.Table_010_SaleFactor.column03 AS Customer,
                                                                                          dbo.Table_011_Child1_SaleFactor.column02 AS GoodCode,
                                                                                          dbo.Table_011_Child1_SaleFactor.column07 AS Number,
                                                                                          dbo.Table_011_Child1_SaleFactor.column10 AS SinglePrice,
                                                                                          dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice,
                                                                                          0.000 AS TotalMargin,
                                                                                          0.000 Ratio,
                                                                                          dbo.Table_010_SaleFactor.column09,
                                                                                          dbo.Table_010_SaleFactor.column36 as SaleType,
                                                                                          g.Column02 as project
                                                                                   FROM   dbo.Table_010_SaleFactor
                                                                                          INNER JOIN dbo.Table_011_Child1_SaleFactor
                                                                                               ON  dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01
                                                                                            left join " + ConBase.Database + @".dbo.Table_035_ProjectInfo g on g.Column00=dbo.Table_011_Child1_SaleFactor.column22
                                                                               ) AS factor
                                                                               INNER JOIN (
                                                                                        SELECT ROW_NUMBER() OVER( PARTITION BY column01 ORDER BY columnid, column02) AS Row,
                                                                                               columnid,
                                                                                               column01,
                                                                                               column02,
                                                                                               column03,
                                                                                               column04,
                                                                                               column05,
                                                                                               column06,
                                                                                               column07,
                                                                                               column08,
                                                                                               column09,
                                                                                               column10,
                                                                                               column11,
                                                                                               column12,
                                                                                               column13,
                                                                                               column14,
                                                                                               column15,
                                                                                               column16,
                                                                                               column17,
                                                                                               column18,
                                                                                               column19,
                                                                                               column20,
                                                                                               column21,
                                                                                               column22,
                                                                                               column23,
                                                                                               column24,
                                                                                               column25,
                                                                                               column26,
                                                                                               column27,
                                                                                               column28,
                                                                                               column29,
                                                                                               Column30,
                                                                                               Column31,
                                                                                               Column32,
                                                                                               Column33
                                                                                        FROM  " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft
                                                                                    ) AS ChildDraftTable
                                                                                    ON  factor.column09 = ChildDraftTable.column01
                                                                                    AND factor.GoodCode = ChildDraftTable.column02
                                                                                    AND factor.Row = ChildDraftTable.Row
                                                                        WHERE  (
                                                                                   factor.FactorDate BETWEEN '" + Date1 +
                                                                                        "' AND '" + Date2 + "')");

                gridEX1.DataSource = Table;
                gridEX1_FilterApplied(sender, e);
                gridEX1.Row = gridEX1.FilterRow.Position;
            }
        }

        private void Form18_MarginReport_GoodCustomerFactor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
             else if (e.Control && e.KeyCode == Keys.D)
                bt_Display_Click(sender, e);
        }

        private void Form18_MarginReport_GoodCustomerFactor_Activated(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.Select();
        }

        private void Form18_MarginReport_GoodCustomerFactor_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.GoodCustomerFactor = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX1.RemoveFilters();
        }

        private void bt_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void gridEX1_FilterApplied(object sender, EventArgs e)
        {
            try
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    item.BeginEdit();
                    item.Cells["TotalMargin"].Value = gridEX1.GetTotalRow().Cells["SingleMargin"].Value;
                    item.Cells["Ratio"].Value =Convert.ToDouble(item.Cells["SingleMargin"].Value.ToString()) /
                        Convert.ToDouble(gridEX1.GetTotalRow().Cells["SingleMargin"].Value.ToString());
                    item.EndEdit();


                }
            }
            catch
            {
            }
        }
    }
}
