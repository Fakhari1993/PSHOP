using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Janus.Windows.GridEX;
using System.IO;

namespace PSHOP._06_Reports._02_Sale
{
    public partial class Form32_UsersActions : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);

        Classes.Class_Documents clDoc = new Classes.Class_Documents();

        public Form32_UsersActions()
        {
            InitializeComponent();
        }

        private void Form32_UsersActions_Load(object sender, EventArgs e)
        {

            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddDays(-1);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;

            DataSet DS = new DataSet();

            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT  [Column00]
      
             FROM  [dbo].[Table_010_UserInfo] WHERE Column03=1 and Column06='" + Class_BasicOperation._Year + "' ", ConMain);
            Adapter.Fill(DS, "USer");
            cmb_User.ComboBox.DataSource = DS.Tables["USer"];
            cmb_User.ComboBox.DisplayMember = "Column00";
            cmb_User.ComboBox.ValueMember = "Column00";


            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_030_ExpenseCenterInfo", ConBase);
            Adapter.Fill(dataSet1, "Center");
            gridEX_List.DropDowns["Center"].SetDataBinding(dataSet1.Tables["Center"], "");

            Adapter = new SqlDataAdapter("Select Column00,Column01,Column02 from Table_035_ProjectInfo", ConBase);
            Adapter.Fill(dataSet1, "Project");
            gridEX_List.DropDowns["Project"].SetDataBinding(dataSet1.Tables["Project"], "");

            Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
            DataTable GoodTable = clGood.GoodInfo();
            gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");

            Adapter = new SqlDataAdapter("Select * from Table_070_CountUnitInfo", ConBase);
            Adapter.Fill(dataSet1, "CountUnit");
            gridEX_List.DropDowns["CountUnit"].SetDataBinding(dataSet1.Tables["CountUnit"], "");



            gridEX_Header.DropDowns["Doc"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnId,Column00 from Table_060_SanadHead"), "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_007_PwhrsDraft", ConWare);
            Adapter.Fill(dataSet1, "Draft");
            gridEX_Header.DropDowns["Draft"].SetDataBinding(dataSet1.Tables["Draft"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_007_FactorBefore", ConSale);
            Adapter.Fill(dataSet1, "Prefactor");
            gridEX_Header.DropDowns["Prefactor"].SetDataBinding(dataSet1.Tables["Prefactor"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01 from Table_018_MarjooiSale", ConSale);
            Adapter.Fill(dataSet1, "Return");
            gridEX_Header.DropDowns["Return"].SetDataBinding(dataSet1.Tables["Return"], "");

            Adapter = new SqlDataAdapter("Select ColumnId,Column01,Column02 from Table_045_PersonInfo", ConBase);
            Adapter.Fill(dataSet1, "Customer");
            gridEX_Header.DropDowns["Customer"].SetDataBinding(dataSet1.Tables["Customer"], "");

            gridEX_Header.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT ColumnId,Column01,Column02 from Table_002_SalesTypes"), "");
            gridEX_Header.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select Columnid,Column01,Column02 from Table_045_PersonInfo"), "");
            gridEX_Header.DropDowns["CustomerClub"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select Columnid,Column03+' '+Column02 as Column03 from Table_215_CustomerClub");

            DataTable CurrencyTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01 from Table_055_CurrencyInfo");
            gridEX_Header.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");
            gridEX_List.DropDowns["Currency"].SetDataBinding(CurrencyTable, "");



        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue && faDatePickerStrip2.FADatePicker.Text.Length == 10 && faDatePickerStrip1.FADatePicker.Text.Length == 10)
            {

                DataTable dt = new DataTable();
                SqlDataAdapter Adapter;
                if (chk_All.Checked)
                {
                    Adapter = new SqlDataAdapter(@"Select   N'فاکتور فروش' AS [type],0 as ttype,
                                                                               [columnid],
                                                                               [column01],
                                                                               [column02],
                                                                               [column03],
                                                                               [column05],
                                                                               [Column36],
                                                                               [column09],
                                                                               [column10],
                                                                               [column17],
                                                                               [column19],
                                                                               [Column13],
                                                                               column14,
                                                                               [Column45],
                                                                               (Column28 + Column32 - Column33 -Column29 -Column30 -Column31) as Column28,
                                                                               Column32,
                                                                               Column33,
                                                                               Column34,
                                                                               Column35,
                                                                               [Column46],
                                                                               [Column47],
                                                                               [Column48],
                                                                               [Column52],
                                                                               [Column54],
                                                                               [Column50],
                                                                               [Column51],
                                                                               [Column53],
                                                                               [column15],
                                                                               [column16]
                                                                        FROM   [Table_010_SaleFactor]
                                                                        WHERE    column02>='" + faDatePickerStrip1.FADatePicker.Text + @"' AND column02<='" + faDatePickerStrip2.FADatePicker.Text + @"'
                                                                        UNION all
                                                                        SELECT N'مرجوعي فاکتور فروش' AS [type],1 as ttype,
                                                                               [columnid],
                                                                               [column01],
                                                                               [column02],
                                                                               [column03],
                                                                               [column05],
                                                                               NULL AS [Column36],
                                                                               [column09],
                                                                               [column10],
                                                                               CAST(0 AS BIT) AS [column17],
                                                                               CAST(0 AS BIT) AS [column19],
                                                                               [Column13],
                                                                               column14,
                                                                               CAST(0 AS BIT) AS [Column45],
                                                                               (Column18+Column19-Column20) as Column28,
                                                                               Column19 AS Column32,
                                                                               Column20 AS Column33,
                                                                               Column21 AS Column34,
                                                                               Column22 AS Column35,
                                                                               CAST(0 AS DECIMAL) AS [Column46],
                                                                               CAST(0 AS DECIMAL) AS [Column47],
                                                                               CAST(0 AS DECIMAL) AS [Column48],
                                                                               CAST(0 AS DECIMAL) AS [Column52],
                                                                               CAST(0 AS DECIMAL) AS [Column54],
                                                                               CAST(0 AS BIT) AS [Column50],
                                                                               NULL AS [Column51],
                                                                               CAST(0 AS BIT) AS [Column53],
                                                                               [column15],
                                                                               [column16]
                                                                        FROM   [Table_018_MarjooiSale]
                                                                        WHERE    column02>='" + faDatePickerStrip1.FADatePicker.Text + @"' AND column02<='" + faDatePickerStrip2.FADatePicker.Text + @"'
                                                                        UNION all

 
                                                                        SELECT N'فاکتور خريد' AS [type],2 as ttype,
                                                                               [columnid],
                                                                               [column01],
                                                                               [column02],
                                                                               [column03],
                                                                               NULL AS [column05],
                                                                               NULL AS [Column36],
                                                                               column10 AS [column09],
                                                                               column11 AS [column10],
                                                                               column19 AS [column17],
                                                                               column17 AS [column19],
                                                                               column05 AS [Column13],
                                                                               column06 AS [Column14],
                                                                               CAST(0 AS BIT) [Column45],
                                                                               (Column20 +Column21 -Column22) AS [Column28],
                                                                               Column21 AS Column32,
                                                                               Column22 AS Column33,
                                                                               Column23 AS Column34,
                                                                               Column24 AS Column35,
                                                                               CAST(0 AS DECIMAL) [Column46],
                                                                               CAST(0 AS DECIMAL) [Column47],
                                                                               CAST(0 AS DECIMAL) [Column48],
                                                                               CAST(0 AS DECIMAL) [Column52],
                                                                               CAST(0 AS DECIMAL) [Column54],
                                                                               CAST(0 AS BIT) [Column50],
                                                                               NULL AS [Column51],
                                                                               CAST(0 AS BIT) [Column53],
                                                                               column07 AS [column15],
                                                                               column08 AS [column16]
                                                                        FROM   [Table_015_BuyFactor]
                                                                        WHERE    column02>='" + faDatePickerStrip1.FADatePicker.Text + @"' AND column02<='" + faDatePickerStrip2.FADatePicker.Text + @"'
                                                                        UNION all
                                                                        SELECT N'مرجوعي فاکتور خريد' AS [type],3 as ttype,
                                                                               [columnid],
                                                                               [column01],
                                                                               [column02],
                                                                               [column03],
                                                                               NULL AS [column05],
                                                                               NULL AS [Column36],
                                                                               column10 AS [column09],
                                                                               column11 AS [column10],
                                                                               CAST(0 AS BIT) AS [column17],
                                                                               CAST(0 AS BIT) AS [column19],
                                                                               column05 AS [Column13],
                                                                               column06 AS [Column14],
                                                                               CAST(0 AS BIT) [Column45],
                                                                              ( Column18 +Column19-Column20) AS [Column28],
                                                                               Column19 AS Column32,
                                                                               Column20 AS Column33,
                                                                               Column21 AS Column34,
                                                                               Column22 AS Column35,
                                                                               CAST(0 AS DECIMAL) [Column46],
                                                                               CAST(0 AS DECIMAL) [Column47],
                                                                               CAST(0 AS DECIMAL) [Column48],
                                                                               CAST(0 AS DECIMAL) [Column52],
                                                                               CAST(0 AS DECIMAL) [Column54],
                                                                               CAST(0 AS BIT) [Column50],
                                                                               NULL AS [Column51],
                                                                               CAST(0 AS BIT) [Column53],
                                                                               column07 AS [column15],
                                                                               column08 AS [column16]
                                                                        FROM   [Table_021_MarjooiBuy]
                                                                        WHERE    column02>='" + faDatePickerStrip1.FADatePicker.Text + @"' AND column02<='" + faDatePickerStrip2.FADatePicker.Text + @"' 

   
                                                                        ", ConSale);
                    Adapter.Fill(dt);

                }
                else
                {

                    if (cmb_User.ComboBox.SelectedValue != null && !string.IsNullOrWhiteSpace(cmb_User.ComboBox.SelectedValue.ToString()))
                    {
                        Adapter = new SqlDataAdapter(@"Select   N'فاکتور فروش' AS [type],0 as ttype,
                                                                               [columnid],
                                                                               [column01],
                                                                               [column02],
                                                                               [column03],
                                                                               [column05],
                                                                               [Column36],
                                                                               [column09],
                                                                               [column10],
                                                                               [column17],
                                                                               [column19],
                                                                               [Column13],
                                                                               column14,
                                                                               [Column45],
                                                                               [Column28],
                                                                               Column32,
                                                                               Column33,
                                                                               Column34,
                                                                               Column35,
                                                                               [Column46],
                                                                               [Column47],
                                                                               [Column48],
                                                                               [Column52],
                                                                               [Column54],
                                                                               [Column50],
                                                                               [Column51],
                                                                               [Column53],
                                                                               [column15],
                                                                               [column16]
                                                                        FROM   [Table_010_SaleFactor]
                                                                        WHERE  Column13 = N'" + cmb_User.ComboBox.SelectedValue.ToString() + @"' AND column02>='" + faDatePickerStrip1.FADatePicker.Text + @"' AND column02<='" + faDatePickerStrip2.FADatePicker.Text + @"'
                                                                        UNION  all
                                                                        SELECT N'مرجوعي فاکتور فروش' AS [type],1 as ttype,
                                                                               [columnid],
                                                                               [column01],
                                                                               [column02],
                                                                               [column03],
                                                                               [column05],
                                                                               NULL AS [Column36],
                                                                               [column09],
                                                                               [column10],
                                                                               CAST(0 AS BIT) AS [column17],
                                                                               CAST(0 AS BIT) AS [column19],
                                                                               [Column13],
                                                                               column14,
                                                                               CAST(0 AS BIT) AS [Column45],
                                                                               Column18 AS Column28,
                                                                               Column19 AS Column32,
                                                                               Column20 AS Column33,
                                                                               Column21 AS Column34,
                                                                               Column22 AS Column35,
                                                                               CAST(0 AS DECIMAL) AS [Column46],
                                                                               CAST(0 AS DECIMAL) AS [Column47],
                                                                               CAST(0 AS DECIMAL) AS [Column48],
                                                                               CAST(0 AS DECIMAL) AS [Column52],
                                                                               CAST(0 AS DECIMAL) AS [Column54],
                                                                               CAST(0 AS BIT) AS [Column50],
                                                                               NULL AS [Column51],
                                                                               CAST(0 AS BIT) AS [Column53],
                                                                               [column15],
                                                                               [column16]
                                                                        FROM   [Table_018_MarjooiSale]
                                                                        WHERE  Column13 = N'" + cmb_User.ComboBox.SelectedValue.ToString() + @"' AND column02>='" + faDatePickerStrip1.FADatePicker.Text + @"' AND column02<='" + faDatePickerStrip2.FADatePicker.Text + @"'
                                                                        UNION all

 
                                                                        SELECT N'فاکتور خريد' AS [type],2 as ttype,
                                                                               [columnid],
                                                                               [column01],
                                                                               [column02],
                                                                               [column03],
                                                                               NULL AS [column05],
                                                                               NULL AS [Column36],
                                                                               column10 AS [column09],
                                                                               column11 AS [column10],
                                                                               column19 AS [column17],
                                                                               column17 AS [column19],
                                                                               column05 AS [Column13],
                                                                               column06 AS [Column14],
                                                                               CAST(0 AS BIT) [Column45],
                                                                               Column20 AS [Column28],
                                                                               Column21 AS Column32,
                                                                               Column22 AS Column33,
                                                                               Column23 AS Column34,
                                                                               Column24 AS Column35,
                                                                               CAST(0 AS DECIMAL) [Column46],
                                                                               CAST(0 AS DECIMAL) [Column47],
                                                                               CAST(0 AS DECIMAL) [Column48],
                                                                               CAST(0 AS DECIMAL) [Column52],
                                                                               CAST(0 AS DECIMAL) [Column54],
                                                                               CAST(0 AS BIT) [Column50],
                                                                               NULL AS [Column51],
                                                                               CAST(0 AS BIT) [Column53],
                                                                               column07 AS [column15],
                                                                               column08 AS [column16]
                                                                        FROM   [Table_015_BuyFactor]
                                                                        WHERE  column05 = N'" + cmb_User.ComboBox.SelectedValue.ToString() + @"' AND column02>='" + faDatePickerStrip1.FADatePicker.Text + @"' AND column02<='" + faDatePickerStrip2.FADatePicker.Text + @"'
                                                                        UNION all
                                                                        SELECT N'مرجوعي فاکتور خريد' AS [type],3 as ttype,
                                                                               [columnid],
                                                                               [column01],
                                                                               [column02],
                                                                               [column03],
                                                                               NULL AS [column05],
                                                                               NULL AS [Column36],
                                                                               column10 AS [column09],
                                                                               column11 AS [column10],
                                                                               CAST(0 AS BIT) AS [column17],
                                                                               CAST(0 AS BIT) AS [column19],
                                                                               column05 AS [Column13],
                                                                               column06 AS [Column14],
                                                                               CAST(0 AS BIT) [Column45],
                                                                               Column18 AS [Column28],
                                                                               Column19 AS Column32,
                                                                               Column20 AS Column33,
                                                                               Column21 AS Column34,
                                                                               Column22 AS Column35,
                                                                               CAST(0 AS DECIMAL) [Column46],
                                                                               CAST(0 AS DECIMAL) [Column47],
                                                                               CAST(0 AS DECIMAL) [Column48],
                                                                               CAST(0 AS DECIMAL) [Column52],
                                                                               CAST(0 AS DECIMAL) [Column54],
                                                                               CAST(0 AS BIT) [Column50],
                                                                               NULL AS [Column51],
                                                                               CAST(0 AS BIT) [Column53],
                                                                               column07 AS [column15],
                                                                               column08 AS [column16]
                                                                        FROM   [Table_021_MarjooiBuy]
                                                                        WHERE  column05 = N'" + cmb_User.ComboBox.SelectedValue.ToString() + @"' AND column02>='" + faDatePickerStrip1.FADatePicker.Text + @"' AND column02<='" + faDatePickerStrip2.FADatePicker.Text + @"' 

   
                                                                        ", ConSale);
                        Adapter.Fill(dt);

                    }
                }
                gridEX_Header.DataSource = dt;
            }
        }

        private void Form32_UsersActions_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                gridEX_Header.RemoveFilters();
                gridEX_List.RemoveFilters();

            }
            catch { }
        }

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Header;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream File = (FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "None");
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    string j = " تاریخ تهیه گزارش:" + FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");
                    gridEXPrintDocument1.PageHeaderLeft = j;
                    // gridEXPrintDocument1.PageHeaderCenter = "گزارش خرید از شرکت مفید گستر غلات راستین" + Environment.NewLine + "از تاریخ " + faDatePickerStrip1.FADatePicker.Text + " تا تاریخ" + faDatePickerStrip2.FADatePicker.Text;

                    printPreviewDialog1.ShowDialog();
                }
        }

        private void gridEX_Header_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                if (Convert.ToInt32(gridEX_Header.GetValue("ttype")) == 0)
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT [columnid]
      ,[column01]
      ,[column02]
      ,[column03]
      ,[column04]
      ,[column05]
      ,[column06]
      ,[column07]
      ,[column08]
      ,[column09]
      ,[column10]
      ,[column11]
      ,[column12]
      ,[column13]
      ,[column14]
      ,[column15]
      ,[column16]
      ,[column17]
      ,[column18]
      ,[column19]
      ,[column20]
      ,[column21]
      ,[column22]
      ,[column23]
      ,[column24]
      ,[column25]
      ,[column26]
      ,[column27]
      ,[column28]
      ,[column29]
      ,[column30]
      ,[Column31]
      ,[Column32]
      ,[Column33]
      ,[Column34]
      ,[Column35]
      ,[Column36]
      ,[Column37]
  FROM  [Table_011_Child1_SaleFactor] where column01=" + gridEX_Header.GetValue("columnid") + "", ConSale);
                    Adapter.Fill(dt);
                }
                if (Convert.ToInt32(gridEX_Header.GetValue("ttype")) == 1)
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT [columnid]
      ,[column01]
      ,[column02]
      ,[column03]
      ,[column04]
      ,[column05]
      ,[column06]
      ,[column07]
      ,[column08]
      ,[column09]
      ,[column10]
      ,[column11]
      ,[column12]
      ,[column13]
      ,[column14]
      ,[column15]
      ,[column16]
      ,[column17]
      ,[column18]
      ,[column19]
      ,[column20]
      ,[column21]
      ,[column22]
      ,[column23]
      ,[column24]
      ,[column25]
      ,[column26]
      ,[column27]
      ,[column28]
      ,[column29]
      ,[column30] as Column31
      ,[Column31]as Column32
      ,[Column32]as Column34
      ,[Column33] as Column35 
      ,[Column34] as Column36
      ,[Column35]as Column37
      
  FROM  [Table_019_Child1_MarjooiSale] where column01=" + gridEX_Header.GetValue("columnid") + "", ConSale);
                    Adapter.Fill(dt);
                }
                if (Convert.ToInt32(gridEX_Header.GetValue("ttype")) == 2)
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT [columnid]
      ,[column01]
      ,[column02]
      ,[column03]
      ,[column04]
      ,[column05]
      ,[column06]
      ,[column07]
      ,[column08]
      ,[column09]
      ,[column10]
      ,[column11]
      ,[column12]
      ,[column13]
      ,[column14]
      ,[column15]
      ,[column16]
      ,[column17]
      ,[column18]
      ,[column19]
      ,[column20]
      ,[column21]
      ,[column22]
      ,[column23]
      ,[column24]
      ,[column25]
      ,[column26]
      ,[column27]
      ,[column28]
      ,[column29]
      ,[column30]
      ,[Column31] as Column33
      ,[Column32]as Column34
      ,[Column33] as Column35
      ,[Column34]as Column36
      ,[Column35] as Column37
       
  FROM  [Table_016_Child1_BuyFactor] where column01=" + gridEX_Header.GetValue("columnid") + "", ConSale);
                    Adapter.Fill(dt);
                }

                if (Convert.ToInt32(gridEX_Header.GetValue("ttype")) == 3)
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT [columnid]
      ,[column01]
      ,[column02]
      ,[column03]
      ,[column04]
      ,[column05]
      ,[column06]
      ,[column07]
      ,[column08]
      ,[column09]
      ,[column10]
      ,[column11]
      ,[column12]
      ,[column13]
      ,[column14]
      ,[column15]
      ,[column16]
      ,[column17]
      ,[column18]
      ,[column19]
      ,[column20]
      ,[column21]
      ,[column22]
      ,[column23]
      ,[column24]
      ,[column25]
      ,[column26]
      ,[column27]
      ,[column28]
      ,[column29]
      ,[column30]as Column34
      ,[Column31] as Column35
      ,[Column32] as Column36
      ,[Column33] as Column37
      
  FROM  [Table_022_Child1_MarjooiBuy] where column01=" + gridEX_Header.GetValue("columnid") + "", ConSale);
                    Adapter.Fill(dt);
                }
                gridEX_List.DataSource = dt;
            }
            catch
            {
            }
        }


    }
}
