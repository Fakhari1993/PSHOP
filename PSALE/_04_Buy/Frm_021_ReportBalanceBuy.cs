using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace PSHOP._04_Buy
{
    public partial class Frm_021_ReportBalanceBuy : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        bool _BackSpace = false;
        public Frm_021_ReportBalanceBuy()
        {
            InitializeComponent();
        }

        private void faDatePickerStrip1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDate1.FADatePicker.HideDropDown();
                    faDate2.FADatePicker.Select();
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
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox =
                    (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


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
                    faDate2.FADatePicker.HideDropDown();
                    //toolStripMenuItem1_Click(sender, e);
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
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox =
                    (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


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

        private void Frm_ReportBalanceBuy_Load(object sender, EventArgs e)
        {
            gridEX_Goods.RemoveFilters();
            gridEX_Goods.UpdateData();

            string[] Dates = Properties.Settings.Default.ViewOrders.Split('-');
            faDate1.FADatePicker.SelectedDateTime =
                  FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDate2.FADatePicker.SelectedDateTime =
                 DateTime.Now;

            btn_Sarch_Click(sender, e);


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
           
        }
        DataTable dt = new DataTable();

        private void btn_Sarch_Click(object sender, EventArgs e)
        {
          dt = ClDoc.ReturnTable(ConSale.ConnectionString, @"SELECT      " + ConWare.Database + @".dbo.table_002_MainGroup.column02 AS MainGroup, 
        " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup.column03 AS SubsidiaryGroup, 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS CodeKala, 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column02 AS NameKala, SUM(dbo.Table_011_Child1_SaleFactor.column07) AS TotalCount, 
                         SUM(dbo.Table_011_Child1_SaleFactor.column11) AS FiSale, " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.Column35 AS LastFiBuy, 
                         SUM(dbo.Table_011_Child1_SaleFactor.column11) / SUM(dbo.Table_011_Child1_SaleFactor.column07) AS FiMediumSale, 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.Column35 * SUM(dbo.Table_011_Child1_SaleFactor.column07) AS SumlastBuy, 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.Column35 * SUM(dbo.Table_011_Child1_SaleFactor.column07) 
                         - SUM(dbo.Table_011_Child1_SaleFactor.column11) AS Total, dbo.Table_010_SaleFactor.column02 AS Date
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients ON 
                         dbo.Table_011_Child1_SaleFactor.column02 = " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
                         " + ConWare.Database + @".dbo.table_002_MainGroup ON 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column03 = " + ConWare.Database + @".dbo.table_002_MainGroup.columnid INNER JOIN
                         " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup ON 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column04 = " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup.columnid
GROUP BY " + ConWare.Database + @".dbo.table_002_MainGroup.column02, " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column02, 
                        " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01, " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.Column35, 
                         " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup.column03, dbo.Table_010_SaleFactor.column02
HAVING        (dbo.Table_010_SaleFactor.column02 >= '" + faDate1.FADatePicker.Text + @"') AND  (dbo.Table_010_SaleFactor.column02 <= '" + faDate2.FADatePicker.Text + @"')");

          bindingSource1.DataSource = dt;
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            if (gridEX_Goods.RowCount>0)
            {
                
            
             string CodeKala = "";
                foreach (Janus.Windows.GridEX.GridEXRow Row in gridEX_Goods.GetRows())
                {
                    CodeKala +="'"+ Row.Cells["CodeKala"].Value.ToString() + "',";
                }
                _04_Buy.Reports.Frm_Rpt_Blance frm = new _04_Buy.Reports.Frm_Rpt_Blance(faDate1.FADatePicker.Text, faDate2.FADatePicker.Text, (gridEX_Goods.RootTable.FilterApplied == null ? " " : ".این گزارش بر اساس پارامتر های انتخابی کاربر تهیه شده است و ممکن است شامل تمام موارد ثبت شده نباشید*"), CodeKala.TrimEnd(','));
            frm.ShowDialog();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

       
    }
}
