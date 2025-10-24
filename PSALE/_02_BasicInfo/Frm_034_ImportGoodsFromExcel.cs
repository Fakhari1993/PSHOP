using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace  PSHOP._02_BasicInfo
{
    public partial class Frm_034_ImportGoodsFromExcel : DevComponents.DotNetBar.OfficeForm
    {
        OleDbConnectionStringBuilder ConnectionBuilder = new OleDbConnectionStringBuilder();
        BindingSource SheetBindingsource = new BindingSource();
        DataTable Sheet;

        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        public DataTable dtGood = new DataTable();
        public Frm_034_ImportGoodsFromExcel()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Define Table
            DataTable ExcelTable = new DataTable();
            ExcelTable.Columns.Add("Column", Type.GetType("System.String"));
            ExcelTable.Columns.Add("Elzam", Type.GetType("System.Boolean"));
            foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX1.RootTable.Columns)
            {
                ExcelTable.Rows.Add(item.Caption, !item.Visible);
            }
            gridEX_Excel.DataSource = ExcelTable;
            dtGood.Columns.Add("Column01", Type.GetType("System.String"));
            dtGood.Columns.Add("Column02", Type.GetType("System.Double"));
        }

        private void bt_OpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_FileName.Text = openFileDialog1.FileName;
                ConnectionBuilder.Provider = "Microsoft.ACE.OLEDB.12.0";
                ConnectionBuilder.DataSource = txt_FileName.Text;
                ConnectionBuilder.Add("Extended Properties", "Excel 12.0 Xml;HDR=YES");
                using (OleDbConnection connection = new OleDbConnection(ConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    Sheet= connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    SheetBindingsource.DataSource = Sheet;
                    if(lbl_SheetName.DataBindings== null)
                    lbl_SheetName.DataBindings.Add("Text", SheetBindingsource, "TABLE_NAME");
                }
                bt_PrevSheet_Click(sender, e);
           
            }
        }

        private void bt_NextSheet_Click(object sender, EventArgs e)
        {
            if (SheetBindingsource.Count > 0)
            {
                SheetBindingsource.MoveNext();
                string selectSql = @"SELECT * FROM [" + ((DataRowView)SheetBindingsource.CurrencyManager.Current)["TABLE_NAME"].ToString()
                    + "]";
                using (OleDbConnection connection = new OleDbConnection(ConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(selectSql, connection))
                    {
                        Sheet = new DataTable();
                        adapter.Fill(Sheet);
                        gridEX_Detail.DataSource = Sheet;
                        gridEX_Detail.RetrieveStructure();
                    }
                }
                string ErrorRows = CheckValidation();
            }
        }

        private void bt_PrevSheet_Click(object sender, EventArgs e)
        {
            if (SheetBindingsource.Count > 0)
            {
                SheetBindingsource.MovePrevious();
                string selectSql = @"SELECT * FROM [" + ((DataRowView)SheetBindingsource.CurrencyManager.Current)["TABLE_NAME"].ToString()
                    + "]";
                using (OleDbConnection connection = new OleDbConnection(ConnectionBuilder.ConnectionString))
                {
                    connection.Open();
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(selectSql, connection))
                    {
                          Sheet = new DataTable();
                        adapter.Fill(Sheet);
                        gridEX_Detail.DataSource = Sheet;
                        gridEX_Detail.RetrieveStructure();

                      

                    }
                }
                string ErrorRows = CheckValidation();
            }
        }

        private void bt_Confirm_Click(object sender, EventArgs e)
        {
            if (gridEX_Detail.GetRows().Length > 0)
            {
                string ErrorRows = CheckValidation();
                
                if (ErrorRows!="") 
                    if(DialogResult.No == MessageBox.Show( "سطرهای "+ErrorRows.TrimEnd(',')+" شامل خطا می باشد در صورت تایید سطرهای بدون خطا انتقال خواهند یافت"+
                    Environment.NewLine + "آیا مایل به ادامه هستید؟", 
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        return;
                
                    try
                    {
                        bool _Insert = false;
                        for (int i = 0; i < gridEX_Detail.GetRows().Length; i++)
                        {
                            Janus.Windows.GridEX.GridEXRow item = gridEX_Detail.GetRow(i);
                            if (item.Cells[2].Text.Trim() == "")
                            {

                                //dtGood.Rows.Add(gridEX_Detail.GetValue(0).ToString(), Convert.ToDecimal(gridEX_Detail.GetValue(1).ToString()));
                                dtGood.Rows.Add(item.Cells[0].Text, Convert.ToDecimal(item.Cells[1].Text));
                                _Insert = true;

                            }
                        }
                        if (_Insert)
                            Class_BasicOperation.ShowMsg("", "انتقال رکورد(ها) با موفقیت صورت گرفت", "Information");
                        this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    }
                    catch(Exception ex)
                    {
                        Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    }
                }
            }

        private string CheckValidation()
        {
            this.Cursor = Cursors.WaitCursor;
            Janus.Windows.GridEX.GridEXFormatCondition format = new Janus.Windows.GridEX.GridEXFormatCondition();
            format.Column = gridEX_Detail.RootTable.Columns[2];
            format.ConditionOperator = Janus.Windows.GridEX.ConditionOperator.NotIsNull;
            format.FormatStyle.BackColor = Color.Yellow;
            gridEX_Detail.RootTable.FormatConditions.Add(format);
            SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
            DataTable MainTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01 from table_002_MainGroup");
            DataTable SubTable = clDoc.ReturnTable(ConWare.ConnectionString, "select ColumnId,Column01,column02 from table_003_SubsidiaryGroup");
            DataTable CountTable = clDoc.ReturnTable(ConBase.ConnectionString, "select Column00,Column01 from Table_070_CountUnitInfo");
            DataTable GoodTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients");


            BindingSource MainBind = new BindingSource();
            BindingSource SubBind = new BindingSource();
            BindingSource CountBind = new BindingSource();
            BindingSource GoodBind = new BindingSource();
            MainBind.DataSource = MainTable;
            SubBind.DataSource = SubTable;
            CountBind.DataSource = CountTable;
            GoodBind.DataSource = GoodTable;
            string ErrorRows = "";
            for (int i = 0; i < gridEX_Detail.GetRows().Length; i++)
            {
                Janus.Windows.GridEX.GridEXRow item = gridEX_Detail.GetRow(i);

                MainBind.RemoveFilter();
                SubBind.RemoveFilter();
                CountBind.RemoveFilter();
                GoodBind.RemoveFilter();
                item.BeginEdit();
                item.Cells[2].Value = DBNull.Value;





                GoodBind.Filter = "Column01=" + item.Cells[0].Text.Trim();
                if (GoodBind.Count == 0)
                {
                    item.Cells[2].Value = "/کد کالا نامعتبر است/";
                    ErrorRows += i + " ,";
                }



                if (item.Cells[1].Text.Trim() == "")
                {
                    item.Cells[2].Value = "/قیمت کالا مشخص نشده است/";
                    ErrorRows += i + " ,";
                }

                item.EndEdit();
            }

            this.Cursor = Cursors.Default;
            return ErrorRows;
        }

    }
}