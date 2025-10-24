using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._06_Reports._03_Buy
{
    public partial class Form09_LastBuyPrice : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        DataTable Table = new DataTable();
        bool _BackSpace = false;

        public Form09_LastBuyPrice()
        {
            InitializeComponent();
        }

        private void Form09_LastBuyPrice_Load(object sender, EventArgs e)
        {

            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            bt_Display_Click(sender, e);

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

        private void bt_Print_Click(object sender, EventArgs e)
        {

            _06_Reports.Form01_ReportForm frm = new Form01_ReportForm(Table, 43, faDatePickerStrip2.FADatePicker.Text, "");
            frm.ShowDialog();
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                string SelectText = string.Format(@"declare @t table(GoodID nvarchar(50),Date nvarchar(50), Price decimal(18,3),Number int);
            insert into @t SELECT     TOP (100) PERCENT dbo.Table_016_Child1_BuyFactor.column02, MAX(dbo.Table_015_BuyFactor.column02) AS Date, 1 AS Expr1, 
                      MAX(dbo.Table_015_BuyFactor.column01) AS Number
            FROM         dbo.Table_016_Child1_BuyFactor INNER JOIN
                                  dbo.Table_015_BuyFactor ON dbo.Table_016_Child1_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid
            WHERE  dbo.Table_015_BuyFactor.column02 <= '" + faDatePickerStrip2.FADatePicker.Text + @"'
            GROUP BY dbo.Table_016_Child1_BuyFactor.column02
            ORDER BY dbo.Table_016_Child1_BuyFactor.column02;
            
            declare @t2 table(codekala2 nvarchar(50) , gheymat2 int,date2 nvarchar(50)
            ,UNIQUE (codekala2,gheymat2,date2)
            );

            insert into @t2 SELECT   dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, 
            dbo.Table_015_BuyFactor.column02 AS Date
            FROM         dbo.Table_016_Child1_BuyFactor INNER JOIN
            dbo.Table_015_BuyFactor ON dbo.Table_016_Child1_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid
       
            GROUP BY dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, dbo.Table_015_BuyFactor.column02;
            update @t set Price=gheymat2 from @t2 as main_table where GoodID=codekala2 and Date=date2; 
            
            
            
            select Tbl1.*,PersonTable.Column02 as PersonName,PersonTable.Column07 as Tel,
            GoodTable.Column02 as GoodName,
            GoodTable.Column01 as GoodCode
            from @t as Tbl1 inner join Table_015_BuyFactor on Table_015_BuyFactor.Column01=Tbl1.Number 
            left outer join {0}.dbo.Table_045_PersonInfo as PersonTable on 
            Table_015_BuyFactor.Column03=PersonTable.ColumnId
            left outer join {1}.dbo.table_004_CommodityAndIngredients  as GoodTable on
            GoodTable.ColumnId=Tbl1.GoodID WHERE Tbl1.Date<='" + faDatePickerStrip2.FADatePicker.Text + "'", ConBase.Database, ConWare.Database);

                Table = new DataTable();
                Table = clDoc.ReturnTable(ConSale.ConnectionString, SelectText);
                gridEX1.DataSource = Table;
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
                    bt_Display_Click(sender, e);
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
    }
}
