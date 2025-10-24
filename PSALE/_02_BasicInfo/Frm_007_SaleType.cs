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
    public partial class Frm_007_SaleType : Form
    {
        bool _Del;
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);

        public Frm_007_SaleType(bool Del)
        {
            InitializeComponent();
            _Del = Del;
        }

        private void Frm_007_SaleType_Load(object sender, EventArgs e)
        {
            this.table_002_SalesTypesTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_002_SalesTypes);
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from AllHeaders()", ConAcnt);
            DataTable Headers = new DataTable();
            DataTable Headers2 = new DataTable();
            Adapter.Fill(Headers);
            Adapter.Fill(Headers2);
            gridEX1.DropDowns["Header"].SetDataBinding(Headers, "");
            gridEX1.DropDowns["Header2"].SetDataBinding(Headers, "");
            HeaderBindingSource.DataSource = Headers2;
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(Int16));
            dt.Columns.Add("Name", typeof(string));
            dt.Rows.Add(0, "صندوقدار");
            dt.Rows.Add(0, "خریدار");

            gridEX1.DropDowns["Type"].SetDataBinding(dt, "");
            

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (this.table_002_SalesTypesBindingSource.Count > 0)
            {
                try
                {
                    this.Validate();
                    this.table_002_SalesTypesBindingSource.EndEdit();
                    this.table_002_SalesTypesTableAdapter.Update(dataSet_EtelaatPaye.Table_002_SalesTypes);
                    Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "Information");
                }
                catch (System.Data.SqlClient.SqlException es)
                {
                    Class_BasicOperation.CheckSqlExp(es,  this.Name);
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex,  this.Name);

                }
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (_Del)
            {
                if (this.table_002_SalesTypesBindingSource.Count > 0)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف رکورد جاری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            this.table_002_SalesTypesBindingSource.RemoveCurrent();
                            this.table_002_SalesTypesBindingSource.EndEdit();
                            this.table_002_SalesTypesTableAdapter.Update(dataSet_EtelaatPaye.Table_002_SalesTypes);
                            Class_BasicOperation.ShowMsg("", "حذف اطلاعات انجام شد", "Information");
                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex,  this.Name);

                        }
                    }
                }
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف رکورد جاری را ندارید", "Warning");
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception,  this.Name);
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;

        }

        private void Frm_007_SaleType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.D && e.Control)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.N)
                gridEX1.Row = -1;
        }

        private void gridEX1_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Column.Key == "column08")
                {
                    HeaderBindingSource.Filter = "ACC_Code='" + e.Value.ToString() + "'";
                    if (HeaderBindingSource.Count > 0)
                    {
                        DataRowView Row=(DataRowView)this.HeaderBindingSource.CurrencyManager.Current;
                        gridEX1.SetValue("Column03",Row["GroupCode"].ToString().Trim());
                        gridEX1.SetValue("Column04",Row["KolCode"].ToString());
                      
                        if (Row["MoeinCode"].ToString().Trim() != "")
                            gridEX1.SetValue("Column05", Row["MoeinCode"].ToString());
                        else
                            gridEX1.SetValue("Column05", DBNull.Value);

                        if (Row["TafsiliCode"].ToString().Trim() != "")
                            gridEX1.SetValue("Column06", Row["TafsiliCode"].ToString());
                        else
                            gridEX1.SetValue("Column06", DBNull.Value);

                        if (Row["JozCode"].ToString().Trim() != "")
                            gridEX1.SetValue("Column07", Row["JozCode"].ToString());
                        else
                            gridEX1.SetValue("Column07", DBNull.Value);
                    }
                }
                else if (e.Column.Key == "column14")
                {
                    HeaderBindingSource.Filter = "ACC_Code='" + e.Value.ToString() + "'";
                    if (HeaderBindingSource.Count > 0)
                    {
                        DataRowView Row = (DataRowView)this.HeaderBindingSource.CurrencyManager.Current;
                        gridEX1.SetValue("Column09", Row["GroupCode"].ToString());
                        gridEX1.SetValue("Column10", Row["KolCode"].ToString());
                  
                        if (Row["MoeinCode"].ToString().Trim() != "")
                            gridEX1.SetValue("Column11", Row["MoeinCode"].ToString());
                        else
                            gridEX1.SetValue("Column11", DBNull.Value);

                        if (Row["TafsiliCode"].ToString().Trim() != "")
                            gridEX1.SetValue("Column12", Row["TafsiliCode"].ToString());
                        else
                            gridEX1.SetValue("Column12", DBNull.Value);

                        if (Row["JozCode"].ToString().Trim() != "")
                            gridEX1.SetValue("Column13", Row["JozCode"].ToString());
                        else
                            gridEX1.SetValue("Column13", DBNull.Value);
                    }
                }
            }
            catch     {   }

            try
            {
                    if (e.Value.ToString().Trim() == "")
                        e.Value = DBNull.Value;
            }
            catch
            {
                e.Value = DBNull.Value;
            }
        }



    }
}
