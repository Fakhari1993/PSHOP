using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._02_BasicInfo
{
    public partial class Frm_035_HighSellingGoods : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;

        public Frm_035_HighSellingGoods()
        {
            InitializeComponent();
        }

        private void Frm_035_HighSellingGoods_Load(object sender, EventArgs e)
        {
          

            gridEX_Header.DropDowns["maingroup"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select * from table_002_MainGroup"), "");
            gridEX_Header.DropDowns["subgroup"].SetDataBinding(clDoc.ReturnTable(ConWare.ConnectionString, "Select * from table_003_SubsidiaryGroup"), "");

            faDatePickerStrip1.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(FarsiLibrary.Utils.PersianDate.Now.Year.ToString() + "/01/01");
            faDatePickerStrip2.FADatePicker.SelectedDateTime = FarsiLibrary.Utils.PersianDate.Parse(FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd"));
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.table_004_CommodityAndIngredients' table. You can move, or remove it, as needed.
            bt_Search_Click(null, null);
        }

        private void Frm_035_HighSellingGoods_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Header.RemoveFilters();
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
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.Text.Length == 10 && faDatePickerStrip1.FADatePicker.Text.Length == 10)
            {
                this.table_004_CommodityAndIngredientsTableAdapter.Fill(this.dataSet_EtelaatPaye.table_004_CommodityAndIngredients);

                table_004_CommodityAndIngredientsBindingSource.DataSource = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT columnid
                                                                                                                                  ,column01
                                                                                                                                  ,column02
                                                                                                                                  ,column03
                                                                                                                                  ,column04
                                                                                                                                  ,column05
                                                                                                                                  ,column06
                                                                                                                                  ,column07
                                                                                                                                  ,Column51
                                                                                                                                  ,Column34
                                                                                                                                  ,Column35
                                                                                                                                  ,Column36
                                                                                                                                  ,Column37
                                                                                                                                  ,Column38
                                                                                                                                  ,Column39
                                                                                                                                  ,Column40
                                                                                                                                  ,Column58
                                                                                                                                  ,ISNULL(
                                                                                                                                       (
                                                                                                                                           SELECT SUM(tcsf.column07)
                                                                                                                                           FROM   " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor tcsf
                                                                                                                                                  JOIN  " + ConSale.Database + @".dbo.Table_010_SaleFactor tsf
                                                                                                                                                       ON  tsf.columnid = tcsf.column01
                                                                                                                                           WHERE  tcsf.column02 = tcai.columnid
                                                                                                                                                  AND tsf.column17 = 0
                                                                                                                                                  AND tsf.column19 = 0
                                                                                                                                                  AND tsf.column02>= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                                                                                                  AND tsf.column02<= '" + faDatePickerStrip2.FADatePicker.Text + @"'
                                                                                                                                       )
                                                                                                                                      ,0
                                                                                                                                   ) AS SaleCount
                                                                                                                            FROM   table_004_CommodityAndIngredients tcai
                                                                                                                            WHERE  tcai.column28 = 1
                                                                                                                                   AND tcai.column19 = 1");

            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            gridEX_Header.UpdateData();
            gridEX_Header.RemoveFilters();

            try
            {



               
                string cmm = string.Empty;
                foreach (Janus.Windows.GridEX.GridEXRow dr in gridEX_Header.GetRows())
                {
                    if (Convert.ToBoolean(dr.Cells["Column51"].Value))

                        cmm += " update table_004_CommodityAndIngredients set Column51=1,column58=" + ((dr.Cells["column58"].Value == DBNull.Value || dr.Cells["column58"].Value == null ) ? "NULL" : dr.Cells["column58"].Value.ToString()) + " where columnid=" + dr.Cells["columnid"].Value;
                   
                    else

                        cmm += " update table_004_CommodityAndIngredients set Column51=0,column58=" + ((dr.Cells["column58"].Value == DBNull.Value || dr.Cells["column58"].Value == null) ? "NULL" : dr.Cells["column58"].Value.ToString()) + " where columnid=" + dr.Cells["columnid"].Value;


                }

                if (!string.IsNullOrWhiteSpace(cmm))
                {

                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        Con.Open();

                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;
                        try
                        {
                            Command.CommandText = cmm;
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();
                     Class_BasicOperation.ShowMsg("", "ثبت اطلاعات با موفقیت انجام شد", "None");

                        }



                        catch (Exception es)
                        {

                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;

                            Class_BasicOperation.CheckExceptionType(es, this.Name);

                        }
                    }

                    //MessageBox.Show("ثبت اطلاعات با موفقیت انجام شد");

                          
                }
            }
            catch (Exception ex)
            {
            }

            this.Cursor = Cursors.Default;
        }

        private void Frm_035_HighSellingGoods_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F5)
                bt_Search_Click(sender, e);
        }
    }
}
