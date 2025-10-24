using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Janus.Windows.GridEX;
using System.Data.SqlClient;

namespace PSHOP._03_Order
{
    public partial class Form03_ConfirmBySaleManager : Form
    {
        Class_UserScope UserScope = new Class_UserScope();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();

        public Form03_ConfirmBySaleManager()
        {
            InitializeComponent();
        }

        private void frm_moshahede_sefareshat_mali_Load(object sender, EventArgs e)
        {

            //this.table_005_OrderHeaderTableAdapter.FillBy_SaleManager(this.dataSet_Foroosh.Table_005_OrderHeader);
            //this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);


            DataSet DS = new DataSet();
            string s = @"SELECT     dbo.Table_045_PersonInfo.ColumnId AS id, dbo.Table_045_PersonInfo.Column01 AS code, dbo.Table_045_PersonInfo.Column02 AS name, 
            dbo.Table_065_CityInfo.Column02 AS shahr, dbo.Table_060_ProvinceInfo.Column01 AS ostan, dbo.Table_045_PersonInfo.Column06 AS adresmoshtari,
            dbo.Table_045_PersonInfo.Column07 as telmoshtari,Table_045_PersonInfo.Column08 as faxmoshtari,Table_045_PersonInfo.Column13 as codepostimoshtari
            FROM         dbo.Table_060_ProvinceInfo INNER JOIN
            dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00 INNER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
            WHERE     (dbo.Table_045_PersonInfo.Column12 = 1)";


            s = string.Format(s, ConSale.Database);
            SqlDataAdapter Adapter = new SqlDataAdapter(s, ConBase);
            Adapter.Fill(DS, "Customer");
            gridEX_Header.DropDowns["Customer"].SetDataBinding(DS.Tables["Customer"], "");

            Adapter = new SqlDataAdapter("SELECT     dbo.Table_065_CityInfo.Column01 AS id, dbo.Table_060_ProvinceInfo.Column01 + ' - ' + dbo.Table_065_CityInfo.Column02 AS shahr FROM         dbo.Table_060_ProvinceInfo INNER JOIN dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00", ConBase);
            Adapter.Fill(DS, "City");
            gridEX_Header.DropDowns["City"].SetDataBinding(DS.Tables["City"], "");

            Adapter = new SqlDataAdapter("SELECT Column00,Column01 FROM Table_115_VehicleType", ConBase);
            Adapter.Fill(DS, "Vehicle");
            gridEX_Header.DropDowns["d3"].SetDataBinding(DS.Tables["Vehicle"], "");

            Adapter = new SqlDataAdapter("SELECT columnid,column02 FROM table_004_CommodityAndIngredients", ConWare);
            Adapter.Fill(DS, "Good");
            gridEX_Detail.DropDowns["Good"].SetDataBinding(DS.Tables["Good"], "");

            multicombo1.DataSource = DS.Tables["Customer"];
            gridEX_Header.DropDowns["d4"].DataSource = DS.Tables["Customer"];

            gridEX_Remain.DropDowns["ACC_Name"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ACC_Code,ACC_Name from  dbo.AllHeaders()"), "");

            //عدم مشاهده سفارشات اشخاص خاص
            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 125))
            {
                table_005_OrderHeaderTableAdapter.Adapter.SelectCommand = new SqlCommand(
                    @"SELECT column01, column02, column03, column04, column05, column06, column07, column08, column09, 
                       column10, column11, column12, column13, column14, column15, column16, column17, column18, column19, 
                       column20, column21, column22, column23, column24, column25, column26, column27, column28, column29,
                       column30, column31, column32, column33, column34, column35, column36, column37, column38, columnid 
                       FROM Table_005_OrderHeader WHERE (column13 = 0) AND (column18 IS NULL) AND (column33 = 0) 
                       AND (NOT (dbo.Table_005_OrderHeader.column03 IN      (SELECT     " + ConBase.Database + @".
                       dbo.Table_045_PersonScope.Column01   FROM         " + ConBase.Database + @".
                       dbo.Table_040_PersonGroups INNER JOIN  " + ConBase.Database + @".dbo.Table_045_PersonScope ON 
                       " + ConBase.Database + @".dbo.Table_040_PersonGroups.Column00 = " + ConBase.Database +
                     @".dbo.Table_045_PersonScope.Column02
                       WHERE     (" +
                               ConBase.Database +
                                ".dbo.Table_040_PersonGroups.Column29 = 1))))"
                    , ConSale);
                dataSet_Foroosh.Table_005_OrderHeader.Clear();
                table_005_OrderHeaderTableAdapter.Adapter.Fill(dataSet_Foroosh, "Table_005_OrderHeader");
                this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);
            }
            else
            {
                table_005_OrderHeaderTableAdapter.Adapter.SelectCommand = new SqlCommand(
                   @"SELECT column01, column02, column03, column04, column05, column06, column07, column08, column09, 
                       column10, column11, column12, column13, column14, column15, column16, column17, column18, column19, 
                       column20, column21, column22, column23, column24, column25, column26, column27, column28, column29,
                       column30, column31, column32, column33, column34, column35, column36, column37, column38, columnid 
                       FROM Table_005_OrderHeader WHERE (column13 = 0) AND (column18 IS NULL) AND (column33 = 0) "
                   , ConSale);
                dataSet_Foroosh.Table_005_OrderHeader.Clear();
                table_005_OrderHeaderTableAdapter.Adapter.Fill(dataSet_Foroosh, "Table_005_OrderHeader");
                this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);
            }

        }
        private void multicombo1_ValueChanged(object sender, EventArgs e)
        {
            try
            {

                if (multicombo1.SelectedIndex == -1)
                {
                    multicombo1.Value = DBNull.Value;
                    editBox11.Text = "";
                    editBox1.Text = "";
                    editBox2.Text = "";
                    editBox3.Text = "";
                    editBox4.Text = "";
                    editBox5.Text = "";
                    editBox6.Text = "";
                    editBox7.Text = "";
                    editBox8.Text = "";
                    editBox9.Text = "";
                    editBox10.Text = "";
                    return;
                }

            }
            catch
            {
            }

            try
            {
                editBox11.Text = multicombo1.DropDownList.GetValue("name").ToString();
                editBox1.Text = multicombo1.DropDownList.GetValue("ostan").ToString();
                editBox2.Text = multicombo1.DropDownList.GetValue("shahr").ToString();
                editBox3.Text = multicombo1.DropDownList.GetValue("telmoshtari").ToString();
                editBox4.Text = multicombo1.DropDownList.GetValue("faxmoshtari").ToString();
                editBox5.Text = multicombo1.DropDownList.GetValue("codepostimoshtari").ToString();
                editBox6.Text = multicombo1.DropDownList.GetValue("adresmoshtari").ToString();
                editBox7.Text = multicombo1.DropDownList.GetValue("column06").ToString();
                editBox8.Text = multicombo1.DropDownList.GetValue("column07").ToString();
                editBox9.Text = multicombo1.DropDownList.GetValue("column05").ToString();
                editBox10.Text = multicombo1.DropDownList.GetValue("column04").ToString();
            }
            catch
            {
            }

        }

        private void gridEX_Header_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (gridEX_Header.GetValue("Column09").ToString() == "True" || gridEX_Header.GetValue("Column09").ToString() == "False")
                {
                    gridEX_Header.SetValue("Column10", FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00"));
                    gridEX_Header.SetValue("Column11", Class_BasicOperation.ServerDate());
                    gridEX_Header.SetValue("Column12", Class_BasicOperation._UserName);

                }
                else
                {
                    gridEX_Header.SetValue("Column09", DBNull.Value);
                    gridEX_Header.SetValue("Column10", DBNull.Value);
                    gridEX_Header.SetValue("Column11", DBNull.Value);
                    gridEX_Header.SetValue("Column12", DBNull.Value);

                }
            }
            catch
            {
                gridEX_Header.SetValue("Column09", DBNull.Value);
                gridEX_Header.SetValue("Column10", DBNull.Value);
                gridEX_Header.SetValue("Column11", DBNull.Value);
                gridEX_Header.SetValue("Column12", DBNull.Value);
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            string good = string.Empty;
            bool ok = true;
            if (this.table_005_OrderHeaderBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 11))
                        throw new Exception("کاربر گرامی شما امکان ذخیره تغییرات را ندارید");

                    foreach (Janus.Windows.GridEX.GridEXRow item1 in gridEX_Detail.GetRows())
                    {

                        DataRowView Row1 = (DataRowView)this.table_005_OrderHeaderBindingSource.CurrencyManager.Current;
                        double TotalGoodRemain = 0;
                        double TotalGoodReservations = 0;
                        TotalGoodRemain = Convert.ToDouble(clDoc.OldTotalGoodRemain(item1.Cells["column02"].Value.ToString(), FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##")).Rows[0]["Remain"]);
                        TotalGoodReservations = Convert.ToDouble(clDoc.OldTotalGoodReservations(item1.Cells["column02"].Value.ToString(), Convert.ToInt32(Row1["columnid"])).Rows[0]["Reservations"]);
                        if (Convert.ToDecimal(item1.Cells["column06"].Value) > (Convert.ToDecimal(TotalGoodRemain)))
                        {
                            if (!good.Contains(item1.Cells["column02"].Text))
                                good += item1.Cells["column02"].Text + "  ,  ";
                        }



                    }
                    if (!string.IsNullOrWhiteSpace(good))
                    {
                        good = good.TrimEnd(',');
                        if (DialogResult.Yes == MessageBox.Show("موجودی کالاهای زیر کافی نیست آیا مایل به ذخیره هستید" + Environment.NewLine + good, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            ok = true;
                        }
                        else
                            ok = false;
                    }
                    else
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            ok = true;
                        else
                            ok = false;
                    }

                    if (ok)
                    {
                        gridEX_Header.UpdateData();
                        this.table_005_OrderHeaderBindingSource.EndEdit();
                        this.table_005_OrderHeaderTableAdapter.Update(dataSet_Foroosh.Table_005_OrderHeader);
                        Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }

        private void gridEX_Detail_FormattingRow(object sender, RowLoadEventArgs e)
        {
            try
            {
                if (e.Row.RowType == Janus.Windows.GridEX.RowType.Record && e.Row.Cells["Column31"].Value.ToString() == "True")
                    e.Row.RowHeaderImageIndex = 0;
            }
            catch
            {
            }
        }

        private void gridEX_Header_FormattingRow(object sender, RowLoadEventArgs e)
        {
            try
            {
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Header.RootTable.Columns["Column09"], ConditionOperator.Equal, true);
                lbl_CheckedStatus.Text = "تعداد تأیید شده:" + gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column09"], AggregateFunction.Count, Filter).ToString();
                Filter.Clear();
                Filter = new GridEXFilterCondition(gridEX_Header.RootTable.Columns["Column09"], ConditionOperator.Equal, 0);
                lbl_UnCheckedStatus.Text = "تعداد تأیید نشده:" + gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column09"], AggregateFunction.Count, Filter).ToString();
                Filter.Clear();
                Filter = new GridEXFilterCondition(gridEX_Header.RootTable.Columns["Column09"], ConditionOperator.IsNull, null);
                lbl_MilddleStatus.Text = "تعداد بررسی نشده:" + gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column09"], AggregateFunction.Count, Filter).ToString();
                Filter.Clear();
            }
            catch { }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            this.table_005_OrderHeaderTableAdapter.FillBy_SaleManager(dataSet_Foroosh.Table_005_OrderHeader);
            this.table_006_OrderDetailsTableAdapter.Fill(dataSet_Foroosh.Table_006_OrderDetails);
        }

        private void Form03_ConfirmBySaleManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.F5)
                bt_Refresh_Click(sender, e);
        }

        private void bt_ExportToExcel_Detail_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Detail;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void bt_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Header;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void gridEX_Header_SelectionChanged(object sender, EventArgs e)
        {
            try
            {

                //مانده حسابها
                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT     TOP (100) PERCENT dbo.Table_065_SanadDetail.Column07, 
                SUM(dbo.Table_065_SanadDetail.Column11) - SUM(dbo.Table_065_SanadDetail.Column12) AS Remain, 
                dbo.Table_065_SanadDetail.Column01
                FROM         dbo.Table_065_SanadDetail INNER JOIN
                dbo.Table_060_SanadHead ON dbo.Table_065_SanadDetail.Column00 = dbo.Table_060_SanadHead.ColumnId
                WHERE     (NOT (dbo.Table_065_SanadDetail.Column07 IS NULL))
                GROUP BY dbo.Table_065_SanadDetail.Column07, dbo.Table_065_SanadDetail.Column01
                HAVING      (dbo.Table_065_SanadDetail.Column07 = " + gridEX_Header.GetValue("Column03").ToString() + @")
                ORDER BY dbo.Table_065_SanadDetail.Column07", ConAcnt);
                DataTable Table = new DataTable();
                Adapter.Fill(Table);
                gridEX_Remain.DataSource = Table;

                //اسناد دریافتنی
                Adapter = new SqlDataAdapter(@"SELECT     SUM(Tbl1.Price) AS Price, Tbl1.Person, Tbl1.LastStatus, COUNT(Tbl1.Column01) AS Count, dbo.Table_060_ChequeStatus.Column02 AS StatusName
                FROM         (SELECT     dbo.Table_035_ReceiptCheques.Column05 AS Price, dbo.Table_035_ReceiptCheques.Column07 AS Person, Rec_LastTurn_1.Column02 AS LastTurn, 
                CASE WHEN Rec_LastTurn_1.Column02 IS NULL THEN Column48 ELSE Rec_LastTurn_1.Column02 END AS LastStatus, Rec_LastTurn_1.Column01
                FROM         dbo.Table_035_ReceiptCheques LEFT OUTER JOIN
                dbo.Rec_LastTurn() AS Rec_LastTurn_1 ON dbo.Table_035_ReceiptCheques.ColumnId = Rec_LastTurn_1.Column01) AS Tbl1 INNER JOIN
                dbo.Table_060_ChequeStatus ON Tbl1.LastStatus = dbo.Table_060_ChequeStatus.ColumnId
                GROUP BY Tbl1.Person, Tbl1.LastStatus, dbo.Table_060_ChequeStatus.Column02
                HAVING      (Tbl1.Person = " + gridEX_Header.GetValue("Column03").ToString() + ")", ConBank);
                DataTable Table2 = new DataTable();
                Adapter.Fill(Table2);
                gridEX_Chq.DataSource = Table2;
                multicombo1.Value = gridEX_Header.GetValue("column03");
                //نمایش اطلاعات مشتری
                //CustomerTable.DefaultView.RowFilter = "PersonId=" + gridEX_Header.GetValue("Column03").ToString();
                //gridEX_Customer.DataSource = CustomerTable.DefaultView;

            }
            catch (Exception)
            {
            }
        }

        private void table_005_OrderHeaderBindingSource_PositionChanged(object sender, EventArgs e)
        {
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Detail.GetRows())
            {

                double carton = 1;
                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT column09 from table_004_CommodityAndIngredients where columnid=" + item.Cells["column02"].Value + "", ConWare);
                DataTable Table2 = new DataTable();
                Adapter.Fill(Table2);
                if (Table2.Rows.Count > 0)
                {
                    if (Table2.Rows[0]["column09"] != DBNull.Value &&
                        Table2.Rows[0]["column09"] != null &&
                        !string.IsNullOrWhiteSpace(Table2.Rows[0]["column09"].ToString())
                        && Convert.ToDouble(Table2.Rows[0]["column09"]) > 0)
                        carton = Convert.ToDouble(Table2.Rows[0]["column09"]);

                }

                DataRowView Row1 = (DataRowView)this.table_005_OrderHeaderBindingSource.CurrencyManager.Current;
                double TotalGoodRemain = 0;
                double TotalGoodReservations = 0;
                TotalGoodRemain = Convert.ToDouble(clDoc.OldTotalGoodRemain(item.Cells["column02"].Value.ToString(), FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##")).Rows[0]["Remain"]);
                TotalGoodReservations = Convert.ToDouble(clDoc.OldTotalGoodReservations(item.Cells["column02"].Value.ToString(), Convert.ToInt32(Row1["columnid"])).Rows[0]["Reservations"]);
                item.BeginEdit();
                item.Cells["column30"].Value = Convert.ToDouble(TotalGoodRemain) / carton;
                item.Cells["column32"].Value = Convert.ToDouble(TotalGoodReservations) / carton;
                item.Cells["Availabe"].Value = (TotalGoodRemain - TotalGoodReservations) / carton;
                item.EndEdit();

            }

        }





    }
}
