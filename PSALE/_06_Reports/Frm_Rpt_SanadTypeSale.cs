using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._06_Reports
{
    public partial class Frm_Rpt_SanadTypeSale : Form
    {
        bool _BackSpace = false;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        DataTable dt = new DataTable();
        public Frm_Rpt_SanadTypeSale()
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

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
              
                dt = clDoc.ReturnTable(ConAcnt.ConnectionString, @"SELECT        dt.Id, dt.Number, dt.Date, dt.ACC_C, dt.PersonId, dt.Center1, dt.Project1, dt.Description, dt.Bed, dt.Bes, dt.SanadTyp1, dt.Refrenc, dt.DetailId, dt.SaleType1, ah.ACC_Code, ah.ACC_Name, 
                         dt.DetailId AS DetailId, " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column02 AS Person, " + ConBase.Database + @".dbo.Table_035_ProjectInfo.Column02 AS Project, " + ConBase.Database + @".dbo.Table_030_ExpenseCenterInfo.Column02 AS Center, 
                         " + ConBase.Database + @".dbo.Table_002_SalesTypes.column02 as TypeSale, " + ConBase.Database + @".dbo.Table_075_SanadTypes.Column01 as SanadTyp
FROM            (SELECT DISTINCT 
                                                    Table_060_SanadHead_1.ColumnId AS Id, Table_060_SanadHead_1.Column00 AS Number, Table_060_SanadHead_1.Column01 AS Date, Table_065_SanadDetail_1.Column01 AS ACC_C, 
                                                    Table_065_SanadDetail_1.Column07 AS PersonId, Table_065_SanadDetail_1.Column08 AS Center1, Table_065_SanadDetail_1.Column09 AS Project1, 
                                                    Table_065_SanadDetail_1.Column10 AS Description, Table_065_SanadDetail_1.Column11 AS Bed, Table_065_SanadDetail_1.Column12 AS Bes, Table_065_SanadDetail_1.Column16 AS SanadTyp1, 
                                                    Table_065_SanadDetail_1.Column17 AS Refrenc, Table_065_SanadDetail_1.ColumnId AS DetailId, CASE WHEN (Table_065_SanadDetail_1.column16 = 15) THEN
                                                        (SELECT        Column36
                                                          FROM            " + ConSale.Database + @".dbo.Table_010_SaleFactor
                                                          WHERE        Columnid = Table_065_SanadDetail_1.column17) ELSE NULL END AS SaleType1
                          FROM            dbo.Table_060_SanadHead AS Table_060_SanadHead_1 LEFT OUTER JOIN
                                                    dbo.Table_065_SanadDetail AS Table_065_SanadDetail_1 ON Table_060_SanadHead_1.ColumnId = Table_065_SanadDetail_1.Column00) AS dt LEFT OUTER JOIN
                         " + ConBase.Database + @".dbo.Table_075_SanadTypes ON dt.SanadTyp1 = " + ConBase.Database + @".dbo.Table_075_SanadTypes.Column00 LEFT OUTER JOIN
                         " + ConBase.Database + @".dbo.Table_002_SalesTypes ON dt.SaleType1 = " + ConBase.Database + @".dbo.Table_002_SalesTypes.columnid LEFT OUTER JOIN
                         " + ConBase.Database + @".dbo.Table_030_ExpenseCenterInfo ON dt.Center1 = " + ConBase.Database + @".dbo.Table_030_ExpenseCenterInfo.Column00 LEFT OUTER JOIN
                         " + ConBase.Database + @".dbo.Table_035_ProjectInfo ON dt.Project1 = " + ConBase.Database + @".dbo.Table_035_ProjectInfo.Column00 LEFT OUTER JOIN
                         " + ConBase.Database + @".dbo.Table_045_PersonInfo ON dt.PersonId = " + ConBase.Database + @".dbo.Table_045_PersonInfo.ColumnId LEFT OUTER JOIN
                         dbo.AllHeaders() AS ah ON ah.ACC_Code = dt.ACC_C
WHERE        (dt.Date >= N'" + faDatePickerStrip1.FADatePicker.Text + "') AND (dt.Date <= N'" + faDatePickerStrip2.FADatePicker.Text + "') ORDER BY dt.DetailId");
                gridEX_Doc.DataSource = dt;
            }
           
        }

        private void Frm_Rpt_SanadTypeSale_Load(object sender, EventArgs e)
        {
         
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now; 
        }

        private void filterEditor1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                bt_Search_Click(sender, e);
            }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {

        }
       
        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.Text!="" && faDatePickerStrip2.FADatePicker.Text!="" && gridEX_Doc.RowCount>0)
            {
                string Number = "";
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Doc.GetRows()) 
                {
                    Number += item.Cells["DetailId"].Value.ToString() + ",";
                }
                _06_Reports.Frm_003_PrintSearchDoc frm = new _06_Reports.Frm_003_PrintSearchDoc(faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text, (gridEX_Doc.RootTable.FilterApplied == null ? " " : ".این گزارش بر اساس پارامتر های انتخابی کاربر تهیه شده است و ممکن است شامل تمام موارد ثبت شده نباشید*"),Number);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("اطلاعات برای چاپ نامعتبر است");
            }
        }
    }
}
