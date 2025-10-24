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
    public partial class Form37_GoodReportByGroup : Form
    {
        bool _BackSpace = false;

        Classes.Class_Documents ClsDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        string Date1, Date2;

        public Form37_GoodReportByGroup()
        {
            InitializeComponent();
        }

        private void Form37_GoodReportByGroup_Load(object sender, EventArgs e)
        {
            DataTable GroupTable = ClsDoc.ReturnTable(ConWare.ConnectionString, "Select columnid,column01,column02 from table_002_MainGroup");
            gridEX_Groups.DataSource = GroupTable;
            string[] Dates = Properties.Settings.Default.GoodReportByGroup.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);
          
            bt_Search_Click(null, null);

        }
        private void mnu_Excel_Table_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void Form37_GoodReportByGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
                Properties.Settings.Default.GoodReportByGroup = faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
            gridEX_Goods.RootTable.Groups.Clear();
          
            gridEX_Goods.RemoveFilters();
            gridEX_Groups.RemoveFilters();
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

                string CommandText = null;
                CommandText = @"
                                            SELECT  
                                                   tcai.column03 AS MainGroup,
                                                   tsg.column03 AS SubGroup,
                                                   tcai.column01 AS GoodCode,
                                                   tcai.column02 AS GoodName,
                                                   SUM(tcsf.column07) AS [Count],
                                                   SUM(tcsf.column11) AS [Amount],
                                                   SUM(tcsf.column20) AS [NetAmount],
                                                   SUM(tcsf.column20) / NULLIF(SUM(ISNULL(tcsf.column07, 0)), 0) AS Avrage
                                            FROM   dbo.Table_010_SaleFactor
                                                   JOIN Table_011_Child1_SaleFactor tcsf
                                                        ON  tcsf.column01 = dbo.Table_010_SaleFactor.columnid
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                                    
                                                   LEFT JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                        ON  tsg.column01 = tcai.column03
                                                        AND tsg.columnid = tcai.column04
                                                            WHERE  (dbo.Table_010_SaleFactor.column02 BETWEEN '{0}' AND '{1}')
                                                                    AND dbo.Table_010_SaleFactor.column17 = 0 AND dbo.Table_010_SaleFactor.column19 = 0
                                            GROUP BY
                                                   tcsf.column02,
                                                   tcai.column03,
                                                   tsg.column03,
                                                   tcai.column01,
       tcai.column02";

                CommandText = string.Format(CommandText, Date1, Date2,
                    ConBase.Database);
                SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                bindingSource1.DataSource = Table;
                gridEX_Groups_SelectionChanged(sender, e);
            }
        }

        private void Form37_GoodReportByGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDatePickerStrip1.FADatePicker.Select();
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string j = " از تاریخ:" + faDatePickerStrip1.FADatePicker.Text + " تا تاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageHeaderLeft = j;
                        gridEXPrintDocument1.PageHeaderRight = " کزارش فروش کالا گروه " + this.gridEX_Groups.GetRow().Cells["column02"].Text;
                        printPreviewDialog1.ShowDialog();
                    }
            }
            catch { }
        }

        private void gridEX_Groups_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                bindingSource1.Filter = "MainGroup=" + gridEX_Groups.GetValue("columnid").ToString();


                txt_amount.Value =0;
                txt_avarge.Value = 0;
                txt_Count.Value = gridEX_Goods.GetCheckedRows().Length;
                txt_netamount.Value =0;
                txt_count1.Value =0;
            }
            catch
            {
            }
        }

        private void gridEX_Goods_RowCheckStateChanged(object sender, Janus.Windows.GridEX.RowCheckStateChangeEventArgs e)
        {
            try
            {
                if (gridEX_Goods.GetCheckedRows().Length > 0)
                {
                    uiGroupBox1.Visible = true;
               

                    txt_amount.Value = gridEX_Goods.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Amount"].Value.ToString()));
                    txt_avarge.Value = gridEX_Goods.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Avrage"].Value.ToString()));
                    txt_Count.Value = gridEX_Goods.GetCheckedRows().Length;
                    txt_netamount.Value = gridEX_Goods.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["NetAmount"].Value.ToString()));
                    txt_count1.Value = gridEX_Goods.GetCheckedRows().AsEnumerable().Sum(row => Convert.ToDecimal(row.Cells["Count"].Value.ToString()));
                }
                else uiGroupBox1.Visible = false;


            }
            catch { }
        }

        private void txt_Remain_Click(object sender, EventArgs e)
        {

        }

    }
}
