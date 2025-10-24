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

namespace PSHOP._06_Reports._03_Buy
{
    public partial class Form44_ImportInfoFromExcel : DevComponents.DotNetBar.OfficeForm
    {
        OleDbConnectionStringBuilder ConnectionBuilder = new OleDbConnectionStringBuilder();
        BindingSource SheetBindingsource = new BindingSource();
        DataTable Sheet;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        public DataTable good;
        public Form44_ImportInfoFromExcel()
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
                    Sheet = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    SheetBindingsource.DataSource = Sheet;
                    if (lbl_SheetName.DataBindings == null)
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
            }
        }

        private void bt_Confirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridEX_Detail.GetRows().Length > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    Janus.Windows.GridEX.GridEXFormatCondition format = new Janus.Windows.GridEX.GridEXFormatCondition();
                    format.Column = gridEX_Detail.RootTable.Columns[4];
                    format.ConditionOperator = Janus.Windows.GridEX.ConditionOperator.NotIsNull;
                    format.FormatStyle.BackColor = Color.Yellow;
                    gridEX_Detail.RootTable.FormatConditions.Add(format);
                    SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
                    SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
                    DataTable GoodTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select ColumnId,Column01,Column02 from table_004_CommodityAndIngredients");
                    BindingSource GoodBind = new BindingSource();
                    GoodBind.DataSource = GoodTable;

                    for (int i = 0; i < gridEX_Detail.GetRows().Length; i++)
                    {
                        Janus.Windows.GridEX.GridEXRow item = gridEX_Detail.GetRow(i);

                        GoodBind.RemoveFilter();

                        item.BeginEdit();
                        item.Cells[4].Value = DBNull.Value;

                        if (item.Cells[0].Text.Trim() == "")
                            item.Cells[4].Value = "/تاریخ فاکتور مشخص نشده است/";

                        else if (item.Cells[2].Text.Trim() == "")
                            item.Cells[4].Value += "/مقدار کالا مشخص نشده است/";

                        else if (item.Cells[3].Text.Trim() == "")
                            item.Cells[4].Value += "/کد کالا مشخص نشده است/";


                        GoodBind.Filter = "Column01='" + item.Cells[3].Text.Trim() + "'";
                        if (GoodBind.Count == 0)
                            item.Cells[4].Value += "/کد کالا نامعتبر است/";

                        item.EndEdit();
                    }

                    this.Cursor = Cursors.Default;
                    if (DialogResult.Yes == MessageBox.Show("در صورت وجود خطا در بین سطور، سطرهای فاقد خطا انتقال خواهند یافت" +
                        Environment.NewLine + "آیا مایل به ادامه هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        this.Cursor = Cursors.WaitCursor;

                        try
                        {
                            bool _Insert = false;


                            good = new DataTable();
                            good.Columns.Add("date", typeof(string));
                            good.Columns.Add("num", typeof(int));
                            good.Columns.Add("value", typeof(decimal));
                            good.Columns.Add("code", typeof(string));
                            good.Columns.Add("name", typeof(string));
                            good.Columns.Add("subgroup", typeof(string));
                            good.Columns.Add("maingroup", typeof(string));
                            good.Columns.Add("id", typeof(int));
                            good.Columns.Add("week", typeof(int));

                            
                            for (int i = 0; i < gridEX_Detail.GetRows().Length; i++)
                            {
                                Janus.Windows.GridEX.GridEXRow item = gridEX_Detail.GetRow(i);
                                if (item.Cells[4].Text.Trim() == "")
                                {
                                    DataTable dff = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT tcai.columnid AS id,
                                                                                                       tsg.column03 AS subgroup,
                                                                                                       tmg.column02 AS maingroup,
                                                                                                       tcai.column02 AS name
                                                                                                FROM   table_004_CommodityAndIngredients tcai
                                                                                                       JOIN table_003_SubsidiaryGroup tsg
                                                                                                            ON  tsg.column01 = tcai.column03
                                                                                                            AND tsg.columnid = tcai.column04
                                                                                                       JOIN table_002_MainGroup tmg
                                                                                                            ON  tmg.columnid = tsg.column01
                                                                                                                WHERE  tcai.column01 = N'" + item.Cells[3].Text.Trim() + "'");
                                    if (dff.Rows.Count > 0)
                                        good.Rows.Add(item.Cells[0].Text.Trim(),
                                            (item.Cells[1].Text.Trim() == "" ? "0" : item.Cells[1].Value),
                                            item.Cells[2].Value,
                                            item.Cells[3].Text.Trim(),
                                            dff.Rows[0]["name"],
                                            dff.Rows[0]["subgroup"],
                                            dff.Rows[0]["maingroup"],
                                            dff.Rows[0]["id"]

                                            );

                                    _Insert = true;

                                }
                            }
                            if (_Insert)
                                Class_BasicOperation.ShowMsg("", "انتقال رکورد(ها) با موفقیت صورت گرفت", "Information");
                            this.Cursor = Cursors.Default;
                            this.DialogResult = System.Windows.Forms.DialogResult.Yes;

                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, this.Name);
                            this.Cursor = Cursors.Default;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
                this.Cursor = Cursors.Default;

            }
        }
    }
}