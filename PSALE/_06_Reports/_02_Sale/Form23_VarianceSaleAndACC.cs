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
    public partial class Form23_VarianceSaleAndACC : Form
    {
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;

        public Form23_VarianceSaleAndACC()
        {
            InitializeComponent();
        }

        private void Form23_VarianceSaleAndACC_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            DataTable DiscountTable = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId,Column01,Column10,Column16 from Table_024_Discount");
            DataTable HeaderTable = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ACC_Code,ACC_Name from AllHeaders()");
            gridEX_Headers.DropDowns["ACC"].SetDataBinding(HeaderTable, "");

            DataTable Headers = new DataTable();
            Headers.Columns.Add("Name", Type.GetType("System.String"));
            Headers.Columns.Add("ACC", Type.GetType("System.String"));
            Headers.Columns.Add("Type", Type.GetType("System.String"));
            Headers.Rows.Add("فاکتور فروش", DBNull.Value, "Sale");
            Headers.Rows.Add("تخفیف خطی فروش", DBNull.Value, "LinearDis");
            Headers.Rows.Add("اضافه خطی فروش", DBNull.Value, "LinearEx");

            foreach (DataRow item in DiscountTable.Rows)
            {
                Headers.Rows.Add(item["Column01"].ToString(), DBNull.Value, item["ColumnId"].ToString());
            }
            Headers.Rows.Add("فاکتور مرجوعی فروش", DBNull.Value, "Return");
            gridEX_Headers.DataSource = Headers;
            

        }

        private void bt_ExportToExcel_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Variance;
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
                    bt_Display_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue &&
                faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                DataTable ListTable = new DataTable();
                ListTable.Columns.Add("Name", Type.GetType("System.String"));
                ListTable.Columns.Add("Header", Type.GetType("System.String"));
                ListTable.Columns.Add("ACC", Type.GetType("System.Double"));
                ListTable.Columns.Add("Sale", Type.GetType("System.Double"));
                ListTable.Columns.Add("Diff", Type.GetType("System.Double"),"Sale-ACC");

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Headers.GetRows())
                {
                    if(item.Cells["ACC"].Text.Trim()!="" && !item.Cells["ACC"].Text.All(char.IsDigit))
                    {
                            if(item.Cells["Type"].Value.ToString()=="Sale")
                              ListTable.Rows.Add("فاکتور فروش", item.Cells["ACC"].Value.ToString(), ACCRemain(item.Cells["ACC"].Value.ToString()), SaleRemain("Column11"));
                            
                            else if(item.Cells["Type"].Value.ToString()=="LinearDis")
                                ListTable.Rows.Add("تخفیف خطی فروش",item.Cells["ACC"].Value.ToString(), ACCRemain(item.Cells["ACC"].Value.ToString()), SaleRemain("Column17"));
                                
                            else if(item.Cells["Type"].Value.ToString()=="LinearEx")
                                ListTable.Rows.Add("اضافه خطی فروش",item.Cells["ACC"].Value.ToString(), ACCRemain(item.Cells["ACC"].Value.ToString()), SaleRemain("Column19"));

                            else if (item.Cells["Type"].Value.ToString() == "Return")
                                ListTable.Rows.Add(" مرجوعی فروش", item.Cells["ACC"].Value.ToString(),ACCRemain(item.Cells["ACC"].Value.ToString()), ReturnRemain("Column20"));

                            else
                                ListTable.Rows.Add(item.Cells["Name"].Text,item.Cells["ACC"].Value.ToString(), ACCRemain(item.Cells["ACC"].Value.ToString()), ExtraRedRemain(item.Cells["Type"].Value.ToString()));
                    }
                }

                DataTable ListTable2 = ListTable.Clone();

                var grouped = from DataRow Row in ListTable.Rows
                              group Row by Row["Header"];
                foreach (var group in grouped)
                {
                    string Name = null;
                    Double Sale = 0, ACC=0;
                    foreach (DataRow item in ListTable.Select("Header ='" + group.Key + "'"))
                    {
                        Name += item["Name"].ToString() + "-";
                        Sale+= Convert.ToDouble(item["Sale"].ToString());
                        ACC = Convert.ToDouble(item["ACC"].ToString());
                    }
                    ListTable2.Rows.Add(Name, DBNull.Value, ACC, Sale);
                }
                gridEX_Variance.DataSource = ListTable2;
            }
        }

        private void Form23_VarianceSaleAndACC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Display_Click(sender, e);
        }

        private Double ACCRemain(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Com = new SqlCommand(@"SELECT     ISNULL(SUM(dbo.Table_065_SanadDetail.Column11) - SUM(dbo.Table_065_SanadDetail.Column12), 0) AS Remain
                FROM         dbo.Table_065_SanadDetail INNER JOIN
                dbo.Table_060_SanadHead ON dbo.Table_065_SanadDetail.Column00 = dbo.Table_060_SanadHead.ColumnId
                WHERE     (dbo.Table_065_SanadDetail.Column01 ='"+ACC+"') AND (dbo.Table_060_SanadHead.Column01 >= '"+faDatePickerStrip1.FADatePicker.Text+@"') AND 
                (dbo.Table_060_SanadHead.Column01 <= '"+faDatePickerStrip2.FADatePicker.Text+"')", Con);
                return Convert.ToDouble(Com.ExecuteScalar().ToString());
            }
        }

        private Double SaleRemain(string Column)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand(@"SELECT     ISNULL(SUM(Price), 0) AS Price
                FROM         (SELECT     CASE WHEN Table_010_SaleFactor.Column12 = 1 THEN Table_011_Child1_SaleFactor."+Column+@" * 
                Table_010_SaleFactor.Column41 ELSE Table_011_Child1_SaleFactor."+Column+@"
                END AS Price
                FROM         dbo.Table_011_Child1_SaleFactor INNER JOIN
                dbo.Table_010_SaleFactor ON dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
                WHERE     (dbo.Table_010_SaleFactor.column02 >= '"+faDatePickerStrip1.FADatePicker.Text+
                "') AND (dbo.Table_010_SaleFactor.column02 <= '"+faDatePickerStrip2.FADatePicker.Text+"')) AS Tbl", Con);

                return Convert.ToDouble(Comm.ExecuteScalar().ToString());
            }
        }

        private Double ReturnRemain(string Column)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand(@"SELECT     ISNULL(SUM(Price), 0) AS Price
                FROM         (SELECT     CASE WHEN Table_018_MarjooiSale.Column12 = 1 THEN Table_019_Child1_MarjooiSale." + Column + @" * 
                Table_018_MarjooiSale.Column24 ELSE Table_019_Child1_MarjooiSale." + Column + @"
                END AS Price
                FROM         dbo.Table_019_Child1_MarjooiSale INNER JOIN
                dbo.Table_018_MarjooiSale ON dbo.Table_019_Child1_MarjooiSale.column01 = dbo.Table_018_MarjooiSale.columnid
                WHERE     (dbo.Table_018_MarjooiSale.column02 >= '" + faDatePickerStrip1.FADatePicker.Text +
                "') AND (dbo.Table_018_MarjooiSale.column02 <= '" + faDatePickerStrip2.FADatePicker.Text + "')) AS Tbl", Con);

                return Convert.ToDouble(Comm.ExecuteScalar().ToString());
            }
        }

        private Double ExtraRedRemain(string ExRedID)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand(@"SELECT     ISNULL(SUM(Price), 0) AS Price
                FROM         (SELECT     CASE WHEN Table_010_SaleFactor.Column12 = 1 THEN Table_012_Child2_SaleFactor.Column04 * Table_010_SaleFactor.Column41 ELSE Table_012_Child2_SaleFactor.Column04
                END AS Price
                FROM         dbo.Table_012_Child2_SaleFactor INNER JOIN
                dbo.Table_010_SaleFactor ON dbo.Table_012_Child2_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid
                WHERE     (dbo.Table_010_SaleFactor.column02 >= '"+faDatePickerStrip1.FADatePicker.Text+
                "') AND (dbo.Table_010_SaleFactor.column02 <= '"+faDatePickerStrip2.FADatePicker.Text+@"') AND 
                (dbo.Table_012_Child2_SaleFactor.column02 = "+ExRedID+")) AS Tbl", Con);

                return Convert.ToDouble(Comm.ExecuteScalar().ToString());
            }
        }

        private void gridEX_Headers_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.FilterGridExDropDown(sender, "ACC", "ACC_Code", "ACC_Name", gridEX_Headers.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch 
            {
            }
            gridEX_Headers.CurrentCellDroppedDown = true;
        }

        private void gridEX_Headers_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "ACC");
            }
            catch 
            {
            }
        }

        private void gridEX_Headers_CancelingCellEdit(object sender, Janus.Windows.GridEX.ColumnActionCancelEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "ACC");
            }
            catch
            {
            }
        }
    }
}
