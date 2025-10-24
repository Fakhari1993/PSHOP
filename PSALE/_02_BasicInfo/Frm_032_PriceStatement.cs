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
    public partial class Frm_032_PriceStatement : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);

        public Frm_032_PriceStatement()
        {
            InitializeComponent();
           
        }

        private void Frm_032_PriceStatement_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_000_Class' table. You can move, or remove it, as needed.


            if (Convert.ToInt32(Properties.Settings.Default.FactorPrice) == 0)
            {
                MessageBox.Show("نحوه محاسبه قیمت در فاکتور فروش را براساس اعلامیه قیمت انتخاب کنید");
               

            }

            this.table_000_ClassTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_000_Class);
         


            gridEX1.DropDowns["GoodCode"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from table_004_CommodityAndIngredients where column19=1 and column28=1");
            gridEX1.DropDowns["GoodName"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from table_004_CommodityAndIngredients where column19=1 and column28=1");
            mlt_gride.DataSource = clDoc.ReturnTable(Properties.Settings.Default.SALE, "Select * from Table_000_Class ");
            mlt_saletype.DataSource = clDoc.ReturnTable(Properties.Settings.Default.BASE, "Select * from Table_002_SalesTypes ");

            DataTable dt1 = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT tcai.columnid,
                                                                           tmg.column02 AS MainGroup,
                                                                           tsg.column03 AS SubGroup
                                                                    FROM   table_004_CommodityAndIngredients tcai
                                                                           JOIN table_003_SubsidiaryGroup tsg
                                                                                ON  tsg.column01 = tcai.column03
                                                                                AND tsg.columnid = tcai.column04
                                                                           JOIN table_002_MainGroup tmg
                                                                                ON  tmg.columnid = tsg.column01");
            gridEX1.DropDowns["MainGroup"].DataSource = dt1;
            gridEX1.DropDowns["SubGroup"].DataSource = dt1;
        }

        private void txt_num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
            else
                Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void Frm_032_PriceStatement_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_AddingRecord(object sender, CancelEventArgs e)
        {
            gridEX1.SetValue("Column06", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column07", Class_BasicOperation.ServerDate());
            gridEX1.SetValue("Column08", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column09", Class_BasicOperation.ServerDate());
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "GoodName")
                    Class_BasicOperation.FilterGridExDropDown(sender, "GoodName", "column01", "column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

                else if (e.Column.Key == "GoodCode")
                    Class_BasicOperation.FilterGridExDropDown(sender, "GoodCode", "column01", "column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);
            }
            catch { }


            gridEX1.SetValue("Column08", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column09", Class_BasicOperation.ServerDate());
        }

        private void gridEX1_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodName");

            }
            catch { }


            if (e.Column.Key == "GoodName")
                gridEX1.SetValue("GoodCode", gridEX1.GetValue("GoodName").ToString());
            else if (e.Column.Key == "GoodCode")
                gridEX1.SetValue("GoodName", gridEX1.GetValue("GoodCode").ToString());
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            gridEX1.Enabled = true;
            dataSet_EtelaatPaye.EnforceConstraints = false;
            this.table_82_PriceStatementTableAdapter.FillByID(this.dataSet_EtelaatPaye.Table_82_PriceStatement, 0);
            this.table_83_PriceStatementChildTableAdapter.FillByHeaderID(this.dataSet_EtelaatPaye.Table_83_PriceStatementChild, 0);
            dataSet_EtelaatPaye.EnforceConstraints = true;
            table_82_PriceStatementBindingSource.AddNew();
            txt_num.Value =
                 clDoc.MaxNumber(Properties.Settings.Default.SALE, "Table_82_PriceStatement", "Column00").ToString();
            txt_date.Text =
                FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");

            txt_user.Text = Class_BasicOperation._UserName;
            txt_edituser.Text = Class_BasicOperation._UserName;
            column07DateTimePicker.Value = Class_BasicOperation.ServerDate();
            column09DateTimePicker.Value = Class_BasicOperation.ServerDate();
            txt_title.Select();

        }

        private void btn_goodcall_Click(object sender, EventArgs e)
        {
            try
            {

                table_82_PriceStatementBindingSource.EndEdit();
                this.Cursor = Cursors.WaitCursor;

                DataTable gooddt = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT tcai.columnid AS GoodID 
                                                                                    FROM   table_004_CommodityAndIngredients tcai
                                                                                    WHERE  tcai.column28 = 1
                                                                                     AND tcai.column19 = 1 
                                                                                        and tcai.columnid not in (select Column01 from " + ConSale.Database + ".dbo.Table_83_PriceStatementChild where Column00=" + ((DataRowView)this.table_82_PriceStatementBindingSource.CurrencyManager.Current)["ColumnId"] + ") ");



                gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;

                int p = 0;
                foreach (DataRow dr in gooddt.Rows)
                {
                    table_83_PriceStatementChildBindingSource.AddNew();
                    DataRowView HeaderRow = (DataRowView)this.table_83_PriceStatementChildBindingSource.CurrencyManager.Current;
                    HeaderRow["Column00"] = ((DataRowView)this.table_82_PriceStatementBindingSource.CurrencyManager.Current)["ColumnId"];
                    HeaderRow["Column01"] = dr["GoodID"];
                    HeaderRow["Column02"] = 0;
                    HeaderRow["Column03"] = 0;
                    HeaderRow["Column04"] = 0;
                    HeaderRow["Column06"] = Class_BasicOperation._UserName;
                    HeaderRow["Column07"] = Class_BasicOperation.ServerDate();
                    HeaderRow["Column08"] = Class_BasicOperation._UserName;
                    HeaderRow["Column09"] = Class_BasicOperation.ServerDate();
                    table_83_PriceStatementChildBindingSource.EndEdit();
                    if (p == 0)
                        p = table_83_PriceStatementChildBindingSource.Position;

                }
                gridEX1.UpdateData();
                gridEX1.MoveTo(p);
                gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;

                this.Cursor = Cursors.Default;



            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridEX1.UpdateData();
                gridEX1.RemoveFilters();

                if (gridEX1.GetDataRows().Count() == 0)
                {
                    Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(txt_date.Text.Substring(0, 4)),
                  Convert.ToInt32(txt_date.Text.Substring(5, 2)),
                  Convert.ToInt32(txt_date.Text.Substring(8, 2))))
                {

                    Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
                    txt_date.Select();
                    this.Cursor = Cursors.Default;
                    return;

                }

                string command = string.Empty;
                bt_Save.Enabled = false;
                ((DataRowView)this.table_82_PriceStatementBindingSource.CurrencyManager.Current)["Column08"] = Class_BasicOperation._UserName;
                ((DataRowView)this.table_82_PriceStatementBindingSource.CurrencyManager.Current)["Column09"] = Class_BasicOperation.ServerDate();
                table_82_PriceStatementBindingSource.EndEdit();
                this.table_82_PriceStatementTableAdapter.Update(this.dataSet_EtelaatPaye.Table_82_PriceStatement);
                table_83_PriceStatementChildBindingSource.EndEdit();
                this.table_83_PriceStatementChildTableAdapter.Update(this.dataSet_EtelaatPaye.Table_83_PriceStatementChild);
               
                Class_BasicOperation.ShowMsg("", " ثبت با موفقیت انجام شد", "Information");

            }
            catch (SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, this.Name);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }

            this.Cursor = Cursors.Default;
            bt_Save.Enabled = true;
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (gridEX1.GetCheckedRows().Count() == 0)
            {
                Class_BasicOperation.ShowMsg("", " کالایی انتخاب نشده است", "Stop");
                return;
            }
            if (!chk_carton.Checked && !chk_joz.Checked && !chk_pack.Checked)
            {
                Class_BasicOperation.ShowMsg("", " قیمتی برای ویرایش انتخاب نشده است", "Stop");
                return;
            }
            if (
                (txt_carton.Value != null
                && !string.IsNullOrWhiteSpace(txt_carton.Value.ToString())
                && Convert.ToDouble(txt_carton.Value) > 0
                && !chk_carton.Checked) ||
                (txt_pack.Value != null
                && !string.IsNullOrWhiteSpace(txt_pack.Value.ToString())
                && Convert.ToDouble(txt_pack.Value) > 0
                && !chk_pack.Checked) ||
                 (txt_joz.Value != null
                && !string.IsNullOrWhiteSpace(txt_joz.Value.ToString())
                && Convert.ToDouble(txt_joz.Value) > 0
                && !chk_joz.Checked)
                )
            {
                Class_BasicOperation.ShowMsg("", " قیمتی برای ویرایش انتخاب نشده است", "Stop");
                return;
            }
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetCheckedRows())
            {
                item.BeginEdit();
                if (chk_carton.Checked)
                    item.Cells["Column02"].Value = txt_carton.Value;
                if (chk_pack.Checked)
                    item.Cells["Column03"].Value = txt_pack.Value;
                if (chk_joz.Checked)
                    item.Cells["Column04"].Value = txt_joz.Value;
                item.Cells["Column08"].Value = Class_BasicOperation._UserName;
                item.Cells["Column09"].Value = Class_BasicOperation.ServerDate();

                item.EndEdit();
            }
            gridEX1.UpdateData();
            btn_clear_Click(null, null);
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
            {
                try
                {
                    this.table_82_PriceStatementBindingSource.EndEdit();
                    this.table_83_PriceStatementChildBindingSource.EndEdit();


                    dataSet_EtelaatPaye.EnforceConstraints = false;
                    int RowID = ReturnIDNumber(int.Parse(txt_Search.Text));
                    this.table_82_PriceStatementTableAdapter.FillByID(this.dataSet_EtelaatPaye.Table_82_PriceStatement, RowID);
                    this.table_83_PriceStatementChildTableAdapter.FillByHeaderID(this.dataSet_EtelaatPaye.Table_83_PriceStatementChild, RowID);
                    dataSet_EtelaatPaye.EnforceConstraints = true;
                    txt_Search.SelectAll();

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                    txt_Search.SelectAll();
                }
            }
        }
      
        private int ReturnIDNumber(int FactorNum)
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                con.Open();
                int ID = 0;
                SqlCommand Commnad = new SqlCommand("Select ISNULL(ColumnId,0) from Table_82_PriceStatement where Column00=" + FactorNum, con);
                try
                {
                    ID = int.Parse(Commnad.ExecuteScalar().ToString());
                    return ID;
                }
                catch
                {
                    throw new Exception("شماره اعلامیه قیمت وارد شده نامعتبر است");
                }
            }
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
                bt_Search_Click(sender, e);
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            chk_carton.Checked = false;
            chk_joz.Checked = false;
            chk_pack.Checked = false;
            txt_carton.Value = 0;
            txt_joz.Value = 0;
            txt_pack.Value = 0;
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (this.table_82_PriceStatementBindingSource.Count > 0)
            {
                try
                {
                    Class_UserScope UserScope = new Class_UserScope();
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 244))
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف اعلامیه قیمت جاری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                    {
                        foreach (DataRowView item in this.table_83_PriceStatementChildBindingSource)
                        {
                            item.Delete();
                        }
                        this.table_83_PriceStatementChildBindingSource.EndEdit();
                        this.table_83_PriceStatementChildTableAdapter.Update(this.dataSet_EtelaatPaye.Table_83_PriceStatementChild);
                        this.table_82_PriceStatementBindingSource.RemoveCurrent();
                        this.table_82_PriceStatementBindingSource.EndEdit();
                        this.table_82_PriceStatementTableAdapter.Update(this.dataSet_EtelaatPaye.Table_82_PriceStatement);
                        dataSet_EtelaatPaye.EnforceConstraints = false;
                        this.table_82_PriceStatementTableAdapter.FillByID(this.dataSet_EtelaatPaye.Table_82_PriceStatement, 0);
                        this.table_83_PriceStatementChildTableAdapter.FillByHeaderID(this.dataSet_EtelaatPaye.Table_83_PriceStatementChild, 0);
                        dataSet_EtelaatPaye.EnforceConstraints = true;
                        txt_Search.Text = string.Empty;
                        Class_BasicOperation.ShowMsg("", " حذف با موفقیت انجام شد", "Information");

                    }

                }

                catch (SqlException es)
                {
                    Class_BasicOperation.CheckSqlExp(es, this.Name);
                    dataSet_EtelaatPaye.EnforceConstraints = false;
                    this.table_82_PriceStatementTableAdapter.FillByID(this.dataSet_EtelaatPaye.Table_82_PriceStatement, Convert.ToInt32(((DataRowView)this.table_82_PriceStatementBindingSource.CurrencyManager.Current)["ColumnId"]));
                    this.table_83_PriceStatementChildTableAdapter.FillByHeaderID(this.dataSet_EtelaatPaye.Table_83_PriceStatementChild, Convert.ToInt32(((DataRowView)this.table_82_PriceStatementBindingSource.CurrencyManager.Current)["ColumnId"]));
                    dataSet_EtelaatPaye.EnforceConstraints = true;
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                    dataSet_EtelaatPaye.EnforceConstraints = false;
                    this.table_82_PriceStatementTableAdapter.FillByID(this.dataSet_EtelaatPaye.Table_82_PriceStatement, Convert.ToInt32(((DataRowView)this.table_82_PriceStatementBindingSource.CurrencyManager.Current)["ColumnId"]));
                    this.table_83_PriceStatementChildTableAdapter.FillByHeaderID(this.dataSet_EtelaatPaye.Table_83_PriceStatementChild, Convert.ToInt32(((DataRowView)this.table_82_PriceStatementBindingSource.CurrencyManager.Current)["ColumnId"]));
                    dataSet_EtelaatPaye.EnforceConstraints = true;
                }
            }
        }

        private void Frm_032_PriceStatement_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            if (e.KeyCode == Keys.D && e.Control)
                bt_Del_Click(sender, e);
             if (e.KeyCode == Keys.N && e.Control)
                 bt_New_Click(sender, e);
            if (e.KeyCode == Keys.E && e.Control)
                toolStripDropDownButton1_Click(sender, e);
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void btn_Copy_Click(object sender, EventArgs e)
        {
            gridEX1.UpdateData();
           
            if (table_82_PriceStatementBindingSource.Count > 0)
            {
                try
                {
                    string  Code = InputBox.Show(
                         "شماره اعلامیه قیمت مورد نظر را وارد نمائید", "");
                    if (Code.ToString().Trim() != "")
                    {
                        int ID = 0;
                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                        {
                            Con.Open();
                            SqlCommand Command = new SqlCommand("Select ISNULL((Select ColumnId from Table_82_PriceStatement where Column00=" + Code + " ),0)", Con);
                            ID = int.Parse(Command.ExecuteScalar().ToString());
                        }
                        if (ID == 0)
                            throw new Exception("شماره اعلامیه قیمت مورد نظر معتبر نیست");
                        bool ok = true;
                        if (gridEX1.GetDataRows().Count() > 0)
                        {
                            if (DialogResult.Yes == MessageBox.Show("سایر کالاهای تعریف شده حذف میشود و کالاهای اعلامیه قیمت انتخابی کپی میشود،آیا مایل به ادامه هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                ok = true;
                            }
                            else
                                ok = false;
                        }
                        if (ok)
                        {
                            table_82_PriceStatementBindingSource.EndEdit();

                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                            {
                                item.Delete();
                            }
                            this.table_83_PriceStatementChildBindingSource.EndEdit();

                            DataTable DetailHeader = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_83_PriceStatementChild where Column00=" + ID);
                            foreach (DataRow item in DetailHeader.Rows)
                            {
                                table_83_PriceStatementChildBindingSource.AddNew();
                                DataRowView HeaderRow = (DataRowView)this.table_83_PriceStatementChildBindingSource.CurrencyManager.Current;
                                HeaderRow["Column00"] = ((DataRowView)this.table_82_PriceStatementBindingSource.CurrencyManager.Current)["ColumnId"];
                                HeaderRow["Column01"] = item["Column01"];
                                HeaderRow["Column02"] = item["Column02"];
                                HeaderRow["Column03"] = item["Column03"];
                                HeaderRow["Column04"] = item["Column04"];
                                HeaderRow["Column06"] = Class_BasicOperation._UserName;
                                HeaderRow["Column07"] = Class_BasicOperation.ServerDate();
                                HeaderRow["Column08"] = Class_BasicOperation._UserName;
                                HeaderRow["Column09"] = Class_BasicOperation.ServerDate();
                                table_83_PriceStatementChildBindingSource.EndEdit();
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
                }
            }
        }

        private void mlt_gride_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column01", "Column00");
        }

        private void mlt_gride_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void mlt_saletype_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "column02", "column01");

        }

        private void mlt_saletype_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void gridEX1_Enter(object sender, EventArgs e)
        {
            try
            {

                table_82_PriceStatementBindingSource.EndEdit();


            }
            catch (Exception ex)
            {
                gridEX1.Focus();
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
                this.Cursor = Cursors.Default;
            }
        }

        private void txt_desc_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                {
                    Class_BasicOperation.isEnter(e.KeyChar);

                }
            }
            else
            {
                Class_BasicOperation.isEnter(e.KeyChar);

            }



        }

        private void txt_Search_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_82_PriceStatementBindingSource.EndEdit();
                this.table_83_PriceStatementChildBindingSource.EndEdit();

                if (dataSet_EtelaatPaye.Table_82_PriceStatement.GetChanges() != null || dataSet_EtelaatPaye.Table_83_PriceStatementChild.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        bt_Save_Click(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select max(Column00) from Table_82_PriceStatement),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_82_PriceStatement where Column00=" + Table.Rows[0]["Row"].ToString());
                  
                    dataSet_EtelaatPaye.EnforceConstraints = false;
                    this.table_82_PriceStatementTableAdapter.FillByID(this.dataSet_EtelaatPaye.Table_82_PriceStatement, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_83_PriceStatementChildTableAdapter.FillByHeaderID(this.dataSet_EtelaatPaye.Table_83_PriceStatementChild, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_EtelaatPaye.EnforceConstraints = true;
                }

            }
            catch
            {
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.table_82_PriceStatementBindingSource.Count > 0)
            {

                try
                {
                    gridEX1.UpdateData();
                    table_82_PriceStatementBindingSource.EndEdit();
                    this.table_83_PriceStatementChildBindingSource.EndEdit();
                    if (dataSet_EtelaatPaye.Table_82_PriceStatement.GetChanges() != null || dataSet_EtelaatPaye.Table_83_PriceStatementChild.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            bt_Save_Click(sender, e);
                        }
                    }

                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select Min(Column00) from Table_82_PriceStatement where Column00>" + ((DataRowView)this.table_82_PriceStatementBindingSource.CurrencyManager.Current)["Column00"].ToString() + "),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_82_PriceStatement where Column00=" + Table.Rows[0]["Row"].ToString());
                        dataSet_EtelaatPaye.EnforceConstraints = false;
                        this.table_82_PriceStatementTableAdapter.FillByID(this.dataSet_EtelaatPaye.Table_82_PriceStatement, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_83_PriceStatementChildTableAdapter.FillByHeaderID(this.dataSet_EtelaatPaye.Table_83_PriceStatementChild, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_EtelaatPaye.EnforceConstraints = true;


                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.table_82_PriceStatementBindingSource.Count > 0)
            {
                try
                {
                    gridEX1.UpdateData();
                    table_82_PriceStatementBindingSource.EndEdit();
                    this.table_83_PriceStatementChildBindingSource.EndEdit();
                    if (dataSet_EtelaatPaye.Table_82_PriceStatement.GetChanges() != null || dataSet_EtelaatPaye.Table_83_PriceStatementChild.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            bt_Save_Click(sender, e);
                        }
                    }



                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString,
                        "Select ISNULL((Select max(Column00) from Table_82_PriceStatement where Column00<" +
                        ((DataRowView)this.table_82_PriceStatementBindingSource.CurrencyManager.Current)["Column00"].ToString() + "),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_82_PriceStatement where Column00=" + Table.Rows[0]["Row"].ToString());
                        dataSet_EtelaatPaye.EnforceConstraints = false;
                        this.table_82_PriceStatementTableAdapter.FillByID(this.dataSet_EtelaatPaye.Table_82_PriceStatement, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_83_PriceStatementChildTableAdapter.FillByHeaderID(this.dataSet_EtelaatPaye.Table_83_PriceStatementChild, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        dataSet_EtelaatPaye.EnforceConstraints = true;

                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_82_PriceStatementBindingSource.EndEdit();
                this.table_83_PriceStatementChildBindingSource.EndEdit();
                if (dataSet_EtelaatPaye.Table_82_PriceStatement.GetChanges() != null || dataSet_EtelaatPaye.Table_83_PriceStatementChild.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        bt_Save_Click(sender, e);
                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select min(Column00) from Table_82_PriceStatement),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_82_PriceStatement where Column00=" + Table.Rows[0]["Row"].ToString());
                    dataSet_EtelaatPaye.EnforceConstraints = false;
                    this.table_82_PriceStatementTableAdapter.FillByID(this.dataSet_EtelaatPaye.Table_82_PriceStatement, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_83_PriceStatementChildTableAdapter.FillByHeaderID(this.dataSet_EtelaatPaye.Table_83_PriceStatementChild, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    dataSet_EtelaatPaye.EnforceConstraints = true;


                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }


    }
}
