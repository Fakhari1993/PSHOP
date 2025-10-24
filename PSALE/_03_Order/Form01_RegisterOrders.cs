using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevComponents.DotNetBar;

namespace PSHOP._03_Order
{
    public partial class Form01_RegisterOrders : Form
    {

        bool _del = false;
        int _id = -1;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWhr = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);

        Class_UserScope UserScope = new Class_UserScope();
        List<string> CustomerGroupCodeList = new List<string>();
        string[] CustomerGroupCodeArry;

        public Form01_RegisterOrders(bool del, int id)
        {
            _id = id;
            _del = del;
            InitializeComponent();
        }

        private void Form01_RegisterOrders_Load(object sender, EventArgs e)
        {
            foreach (Janus.Windows.GridEX.GridEXColumn col in this.gridEX_Header.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
            }
            foreach (Janus.Windows.GridEX.GridEXColumn col in this.gridEX_OrderInfo.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
            }
            try
            {

                mnu_CalculatePrice.Checked = Properties.Settings.Default.SalePriceCompute;
                this.table_002_SalesTypesTableAdapter.Fill(this.dataSet_Foroosh.Table_002_SalesTypes);
                this.table_045_PersonInfoTableAdapter.Fill(this.dataSet_Foroosh.Table_045_PersonInfo);
                this.table_115_VehicleTypeTableAdapter.Fill(this.dataSet_Foroosh.Table_115_VehicleType);
                this.table_065_CityInfoTableAdapter.Fill(this.dataSet_Foroosh.Table_065_CityInfo);

                gridEX_Header.DropDowns["CustomerName"].SetDataBinding(
                    this.dataSet_Foroosh, "Table_045_PersonInfo");
                gridEX_Header.DropDowns["CustomerAddress"].SetDataBinding(
                    this.dataSet_Foroosh, "Table_045_PersonInfo");
                gridEX_Header.DropDowns["CustomerTel"].SetDataBinding(
                    this.dataSet_Foroosh, "Table_045_PersonInfo");
                gridEX_Header.DropDowns["VehicleType"].SetDataBinding(
                    this.dataSet_Foroosh, "Table_115_VehicleType");
                gridEX_Header.DropDowns["SaleType"].SetDataBinding(
                    this.dataSet_Foroosh, "Table_002_SalesTypes");
                gridEX_Header.DropDowns["CityTo"].SetDataBinding(
                    this.dataSet_Foroosh, "Table_065_CityInfo");

                DataTable GoodTable = clGood.MahsoolInfo(0);
                gridEX_Order.DropDowns[0].SetDataBinding(GoodTable, "");
                gridEX_Order.DropDowns[1].SetDataBinding(GoodTable, "");

                this.WindowState = FormWindowState.Maximized;

                if (_id != -1)
                {
                    this.table_005_OrderHeaderTableAdapter.FillByID(
                        this.dataSet_Foroosh.Table_005_OrderHeader, _id);
                    this.table_006_OrderDetailsTableAdapter.FillByHeaderID(
                        this.dataSet_Foroosh.Table_006_OrderDetails, _id);
                }
                else
                    bt_New_Click(this, e);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }

        }


        public void bt_Search_Click(object sender, EventArgs e)
        {
            if (txt_Search.Text.Trim() != "")
            {
                try
                {
                    string Command = @"SELECT     column03
                    FROM         " + ConSale.Database + @".dbo.Table_005_OrderHeader
                    WHERE     (column01 = " + txt_Search.Text + @") AND (column03 IN
                    (SELECT     dbo.Table_045_PersonScope.Column01
                    FROM         dbo.Table_040_PersonGroups INNER JOIN
                    dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02
                    WHERE     (dbo.Table_040_PersonGroups.Column29 = 1) AND (dbo.Table_040_PersonGroups.Column00 > 13)))";
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 125) &&
                        clDoc.ReturnTable(ConBase.ConnectionString, Command).Rows.Count > 0)
                        throw new Exception("کاربر گرامی شما امکان مشاهده این سفارش را ندارید");


                    int _id = ReturnIDNumber(int.Parse(txt_Search.Text));
                    dataSet_Foroosh.EnforceConstraints = false;
                    this.table_005_OrderHeaderTableAdapter.FillByID(dataSet_Foroosh.Table_005_OrderHeader, _id);
                    this.table_006_OrderDetailsTableAdapter.FillByHeaderID(dataSet_Foroosh.Table_006_OrderDetails, _id);
                    dataSet_Foroosh.EnforceConstraints = true;
                    txt_Search.SelectAll();
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }

        private int ReturnIDNumber(int Number)
        {
            int ID = 0;
            SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE);
            Con.Open();
            SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_005_OrderHeader where column01=" + Number, Con);
            try
            {
                ID = int.Parse(Commnad.ExecuteScalar().ToString());
                return ID;
            }
            catch
            {
                txt_Search.SelectAll();
                throw new Exception("شماره سفارش وارد شده نامعتبر است");
            }
            finally
            {
                Con.Close();
            }
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {
                dataSet_Foroosh.EnforceConstraints = false;
                this.table_005_OrderHeaderTableAdapter.Fill_Fornew(this.dataSet_Foroosh.Table_005_OrderHeader, 0);
                this.table_006_OrderDetailsTableAdapter.Fill_Fornew(this.dataSet_Foroosh.Table_006_OrderDetails, 0);
                dataSet_Foroosh.EnforceConstraints = true;
                gridEX_Header.MoveToNewRecord();

                gridEX_Header.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##"));
                gridEX_Header.SetValue("column29", Class_BasicOperation._UserName);
                gridEX_Header.SetValue("column28", Class_BasicOperation.ServerGetDate());

                gridEX_Header.Focus();
                gridEX_Header.Select();
                gridEX_Header.Col = 2;//gridEX_Header.RootTable.Columns["column08"].Index;
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Header_CellValueChanged(object sender,
            Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_Header.CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "column03")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column03", "Column01", "Column02", gridEX_Header.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }
        }

        private void gridEX_Header_CellUpdated(object sender,
            Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column03");
            }
            catch { }

            gridEX_Header.SetValue("column031", gridEX_Header.GetValue("column03"));
            gridEX_Header.SetValue("column032", gridEX_Header.GetValue("column03"));
            if (e.Column.Key == "column03")
            {
                try
                {
                    gridEX_Header.SetValue("Column05", gridEX_Header.RootTable.Columns["column03"].DropDown.GetValue("Column22").ToString());
                }
                catch
                {
                    gridEX_Header.SetValue("Column05", DBNull.Value);
                }
                try
                {
                    gridEX_Header.SetValue("Column08",
                        gridEX_Header.RootTable.Columns["column03"].DropDown.GetValue("Column30").ToString());
                }
                catch (Exception)
                {
                    gridEX_Header.SetValue("Column08", DBNull.Value);
                }
            }
            //gridEX_Header.SetValue("Column06", gridEX_Header.GetRow().Cells["Column031"].Text);

            if (gridEX_Header.GetValue("column35").ToString() == "True")
            {
                gridEX_Header.SetValue("column36", 0);
                gridEX_Header.SetValue("column37", 0);
            }

            if (gridEX_Header.GetValue("column37").ToString() == "True")
                gridEX_Header.SetValue("column35", 0);
            if (gridEX_Header.GetValue("column36").ToString() == "True")
                gridEX_Header.SetValue("column35", 0);

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridEX_Header.GetValue("column09").ToString() == "True" ||
                    gridEX_Header.GetValue("column13").ToString() == "True")
                    MessageBox.Show("این سفارش قطعی شده و امکان تغییر آن وجود ندارد");
                else
                {
                    gridEX_Header.UpdateData();
                    gridEX_Order.UpdateData();
                    if (int.Parse(gridEX_Header.GetValue("Column01").ToString()) < 0)
                    {
                        int _MaxOrderNo = 0;
                        SqlCommand MaxOrderNoCmd = new SqlCommand(
                            "SELECT IsNull(MAX(column01),0) AS MaxOrderNo FROM dbo.Table_005_OrderHeader",
                            new SqlConnection(Properties.Settings.Default.SALE));
                        MaxOrderNoCmd.Connection.Open();
                        _MaxOrderNo = int.Parse(MaxOrderNoCmd.ExecuteScalar().ToString());
                        MaxOrderNoCmd.Connection.Close();

                        _MaxOrderNo += 1;
                        gridEX_Header.SetValue("Column01", _MaxOrderNo);
                    }
                    DataRowView Row = (DataRowView)this.table_005_OrderHeaderBindingSource.CurrencyManager.Current;

                    string goodname = string.Empty;
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Order.GetRows())
                    {
                        double TotalGoodRemain = 0;
                        double TotalGoodReservations = 0;
                        TotalGoodRemain = Convert.ToDouble(clDoc.OldTotalGoodRemain(item.Cells["Column02"].Value.ToString(), Row["column02"].ToString()).Rows[0]["Remain"]);
                        TotalGoodReservations = Convert.ToDouble(clDoc.OldTotalGoodReservations(item.Cells["Column02"].Value.ToString(), Convert.ToInt32(Row["columnid"])).Rows[0]["Reservations"]);
                        if (Convert.ToDouble(item.Cells["column06"].Value) > (TotalGoodRemain - TotalGoodReservations))
                        {
                            if (!goodname.Contains(item.Cells["column02"].Text))

                            goodname +=
                                //item.Cells["column002"].Text ;
                                item.Cells["column02"].Text + "   ,   ";
                        }
                    }

                    if ((goodname != string.Empty && DialogResult.Yes == MessageBox.Show("موجودی کالاهای زیر کافی نیست آیا مایل به ثبت سفارش هستید؟" + Environment.NewLine + goodname.TrimEnd(','), "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)) || goodname == string.Empty)
                    {

                        table_005_OrderHeaderBindingSource.EndEdit();

                        foreach (DataRowView item in this.table_006_OrderDetailsBindingSource)
                        {
                            if (bool.Parse(item["column31"].ToString()).Equals(true))
                                item.Delete();
                        }

                        table_006_OrderDetailsBindingSource.EndEdit();
                        tableAdapterManager.UpdateAll(dataSet_Foroosh);

                        if (gridEX_Header.GetValue("column34").ToString() == "False")
                        {



                            CalculatingAwards();

                            table_006_OrderDetailsBindingSource.EndEdit();
                            tableAdapterManager.UpdateAll(dataSet_Foroosh);
                        }
                        if (sender == bt_Save || sender == this)
                            Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                    }
                    else
                    {
                        Class_BasicOperation.ShowMsg("", "موجودی کالاهای  " + goodname + " کافی نیست", "Information");

                    }
                }
                bt_New.Enabled = true;

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                bt_New.Enabled = true;

            }
        }


        private void CalculatingAwards()
        {
            DataTable _TblAward = new DataTable();
            DataTable _TempTblAward = new DataTable();

            _TblAward.Columns.Add("GiftGood");
            _TblAward.Columns.Add("GiftNumber");
            _TblAward.Columns.Add("InCartoon");


            //محاسبه جایزه بر اساس کالا
            if (gridEX_Header.GetValue("column35").ToString() == "True")
            {
                _TempTblAward = Classes.Class_Award.OrderAwardByGoods_Box(
                    int.Parse(gridEX_Header.GetValue("columnid").ToString()),
                     gridEX_Header.GetValue("column02").ToString());


                if (_TempTblAward.Rows.Count > 0)
                {
                    foreach (DataRow dr in _TempTblAward.Rows)
                    {
                        foreach (DataRow ExistDr in _TblAward.Rows)
                        {
                            if (ExistDr["GiftGood"].ToString() == dr["GiftGood"].ToString())
                            {
                                ExistDr["GiftNumber"] =
                                    Convert.ToDouble(ExistDr["GiftNumber"]) +
                                    Convert.ToDouble(dr["GiftNumber"]);
                                dr["GiftNumber"] = 0;
                            }
                        }

                        if (Convert.ToDouble(dr["GiftNumber"]) > 0)
                        {
                            DataRow ndr = _TblAward.NewRow();
                            ndr["GiftGood"] = dr["GiftGood"];
                            ndr["GiftNumber"] = dr["GiftNumber"];
                            ndr["InCartoon"] = dr["InCartoon"];
                            _TblAward.Rows.Add(ndr);
                        }
                    }
                }
            }

            //محاسبه جایزه بر اساس حجم ریالی سفارش
            if (gridEX_Header.GetValue("column36").ToString() == "True")
            {
                _TempTblAward = Classes.Class_Award.OrderAwardByRials(
                    int.Parse(gridEX_Header.GetValue("columnid").ToString()),
                     gridEX_Header.GetValue("column02").ToString());

                if (_TempTblAward.Rows.Count > 0)
                {
                    foreach (DataRow dr in _TempTblAward.Rows)
                    {
                        foreach (DataRow ExistDr in _TblAward.Rows)
                        {
                            if (ExistDr["GiftGood"].ToString() == dr["GiftGood"].ToString())
                            {
                                ExistDr["GiftNumber"] =
                                    Convert.ToDouble(ExistDr["GiftNumber"]) +
                                    Convert.ToDouble(dr["GiftNumber"]);
                                dr["GiftNumber"] = 0;
                            }
                        }

                        if (Convert.ToDouble(dr["GiftNumber"]) > 0)
                        {
                            DataRow ndr = _TblAward.NewRow();
                            ndr["GiftGood"] = dr["GiftGood"];
                            ndr["GiftNumber"] = dr["GiftNumber"];
                            ndr["InCartoon"] = dr["InCartoon"];
                            _TblAward.Rows.Add(ndr);
                        }
                    }
                }
            }

            //محاسبه جایزه بر اساس حجم تعدادی سفارش
            if (gridEX_Header.GetValue("column37").ToString() == "True")
            {
                _TempTblAward = Classes.Class_Award.OrderAwardByTotalQty(
                    int.Parse(gridEX_Header.GetValue("columnid").ToString()),
                     gridEX_Header.GetValue("column02").ToString());

                if (_TempTblAward.Rows.Count > 0)
                {
                    foreach (DataRow dr in _TempTblAward.Rows)
                    {
                        foreach (DataRow ExistDr in _TblAward.Rows)
                        {
                            if (ExistDr["GiftGood"].ToString() == dr["GiftGood"].ToString())
                            {
                                ExistDr["GiftNumber"] =
                                    Convert.ToDouble(ExistDr["GiftNumber"]) +
                                    Convert.ToDouble(dr["GiftNumber"]);
                                dr["GiftNumber"] = 0;
                            }
                        }

                        if (Convert.ToDouble(dr["GiftNumber"]) > 0)
                        {
                            DataRow ndr = _TblAward.NewRow();
                            ndr["GiftGood"] = dr["GiftGood"];
                            ndr["GiftNumber"] = dr["GiftNumber"];
                            ndr["InCartoon"] = dr["InCartoon"];
                            _TblAward.Rows.Add(ndr);
                        }
                    }
                }
            }

            //محاسبه جایزه بر اساس نوع وسیله نقلیه
            if (gridEX_Header.GetValue("column38").ToString() == "True")
            {
                _TempTblAward = Classes.Class_Award.OrderAwardByVehcile(
                    int.Parse(gridEX_Header.GetValue("columnid").ToString()),
                     gridEX_Header.GetValue("column02").ToString());

                if (_TempTblAward.Rows.Count > 0)
                {
                    foreach (DataRow dr in _TempTblAward.Rows)
                    {
                        foreach (DataRow ExistDr in _TblAward.Rows)
                        {
                            if (ExistDr["GiftGood"].ToString() == dr["GiftGood"].ToString())
                            {
                                ExistDr["GiftNumber"] =
                                    Convert.ToDouble(ExistDr["GiftNumber"]) +
                                    Convert.ToDouble(dr["GiftNumber"]);
                                dr["GiftNumber"] = 0;
                            }
                        }

                        if (Convert.ToDouble(dr["GiftNumber"]) > 0)
                        {
                            DataRow ndr = _TblAward.NewRow();
                            ndr["GiftGood"] = dr["GiftGood"];
                            ndr["GiftNumber"] = dr["GiftNumber"];
                            ndr["InCartoon"] = dr["InCartoon"];
                            _TblAward.Rows.Add(ndr);
                        }
                    }
                }
            }

            //در پایان هر بخش از محاسبات جایزه تعداد سطرهای جایزه به جدول
            //_TblAward
            //اضافه می شود و نهایتا اطلاعات این جدول در ریز سفارش درج می شود

            if (_TblAward.Rows.Count > 0)
            {
                for (int i = 0; i < _TblAward.Rows.Count; i++)
                {
                    gridEX_Order.MoveToNewRecord();
                    gridEX_Order.SetValue("column02", _TblAward.Rows[i]["GiftGood"]);
                    gridEX_Order.SetValue("column28", _TblAward.Rows[i]["InCartoon"]);
                    gridEX_Order.SetValue("column04", _TblAward.Rows[i]["GiftNumber"]);
                    gridEX_Order.SetValue("column06",
                        Convert.ToDouble(_TblAward.Rows[i]["InCartoon"]) *
                        Convert.ToDouble(_TblAward.Rows[i]["GiftNumber"]));
                    gridEX_Order.SetValue("column10", 0);//unit price
                    gridEX_Order.SetValue("column13", 0);//total price
                    gridEX_Order.SetValue("column07", 1);
                    gridEX_Order.SetValue("column31", 1);

                    gridEX_Order.UpdateData();
                }
            }

        }


        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }


        private void gridEX2_AddingRecord(object sender, CancelEventArgs e)
        {
            gridEX_Order.SetValue("column19", Class_BasicOperation._UserName.ToString());
            gridEX_Order.SetValue("column20", Class_BasicOperation.ServerGetDate());
        }

        private void frm_sefareshat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                bt_New_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
            {
                txt_Search.Focus();
                txt_Search.SelectAll();
            }
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.E)
                bt_ExportPrefactor_Click(sender, e);
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            try
            {
                if ( gridEX_Header.GetValue("column09").ToString() == "True" ||  gridEX_Header.GetValue("Column18").ToString() == "True")
                    throw new Exception("این سفارش قطعی شده و حذف آن امکان پذیر نیست");

                if (_del)
                {
                    if (this.table_005_OrderHeaderBindingSource.Count > 0)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف این سفارش هستید؟",
                            "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            foreach (DataRowView item in this.table_006_OrderDetailsBindingSource)
                            {
                                item.Delete();
                            }

                            table_006_OrderDetailsBindingSource.EndEdit();
                            this.table_005_OrderHeaderBindingSource.RemoveCurrent();

                            table_005_OrderHeaderBindingSource.EndEdit();
                            table_006_OrderDetailsTableAdapter.Update(dataSet_Foroosh.Table_006_OrderDetails);
                            table_005_OrderHeaderTableAdapter.Update(dataSet_Foroosh.Table_005_OrderHeader);


                           // tableAdapterManager.UpdateAll(dataSet_Foroosh);

                            Class_BasicOperation.ShowMsg("", "سفارش مورد نظر حذف شد", "Information");
                        }
                    }
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید",
                        "Stop");
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }


        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_Order.CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column02", "GoodCode", "GoodName", gridEX_Order.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

                else if (e.Column.Key == "column002")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column002", "GoodCode", "GoodName", gridEX_Order.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);

            }
            catch { }
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
                bt_Search_Click(sender, e);
        }

        private void frm_Sabt_sefareshat_Activated(object sender, EventArgs e)
        {
            txt_Search.Focus();
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_005_OrderHeaderBindingSource.Count > 0)
                {
                    bt_Save_Click(sender, e);
                    Prints.Form_OrderPrint frm = new Prints.Form_OrderPrint(
                        int.Parse(gridEX_Header.GetValue("Column01").ToString()));
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.ShowMsg("", ex.Message, "Warning");
            }
        }


        private void mnu_GoodInfo_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 5))
            {
                _02_BasicInfo.Frm_009_AdditionalGoodsInfo ob =
                    new _02_BasicInfo.Frm_009_AdditionalGoodsInfo(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 6));
                ob.ShowDialog();

                DataTable GoodTable = clGood.MahsoolInfo( 0);
                gridEX_Order.DropDowns[0].SetDataBinding(GoodTable, "");
                gridEX_Order.DropDowns[1].SetDataBinding(GoodTable, "");


            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_CustomerInfo_Click(object sender, EventArgs e)
        {
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 5))
            {
                PACNT._1_BasicMenu.Form03_Persons frm = new PACNT._1_BasicMenu.Form03_Persons(
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 6));
                frm.ShowDialog();


                dataSet_Foroosh.EnforceConstraints = false;
                this.table_045_PersonInfoTableAdapter.Fill(this.dataSet_Foroosh.Table_045_PersonInfo);
                dataSet_Foroosh.EnforceConstraints = true;

            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_VehicleType_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 58))
            {
                _02_BasicInfo.Frm_004_Moarefi_Anvae_VNaghliye ob =
                    new _02_BasicInfo.Frm_004_Moarefi_Anvae_VNaghliye();
                ob.ShowDialog();

                dataSet_Foroosh.EnforceConstraints = false;
                this.table_115_VehicleTypeTableAdapter.Fill(this.dataSet_Foroosh.Table_115_VehicleType);
                dataSet_Foroosh.EnforceConstraints = true;

            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_SaleType_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 3))
            {
                _02_BasicInfo.Frm_007_SaleType ob =
                    new _02_BasicInfo.Frm_007_SaleType(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 4));
                ob.ShowDialog();

                dataSet_Foroosh.EnforceConstraints = false;
                this.table_002_SalesTypesTableAdapter.Fill(this.dataSet_Foroosh.Table_002_SalesTypes);
                dataSet_Foroosh.EnforceConstraints = true;
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_Province_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 55))
            {
                _02_BasicInfo.Frm_001_Moarefi_Ostan_Shahr ob =
                    new _02_BasicInfo.Frm_001_Moarefi_Ostan_Shahr(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 56));
                ob.ShowDialog();

                dataSet_Foroosh.EnforceConstraints = false;
                this.table_065_CityInfoTableAdapter.Fill(this.dataSet_Foroosh.Table_065_CityInfo);
                dataSet_Foroosh.EnforceConstraints = true;
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ViewOrders_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 63))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form02_ViewOrders")
                    {
                        item.BringToFront();
                        return;
                    }
                }

                _03_Order.Form02_ViewOrders frm = new _03_Order.Form02_ViewOrders();
                try
                {
                    frm.MdiParent = this.MdiParent;
                }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX_Order_CellUpdated(object sender,
            Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column002");
            }
            catch { }


            try
            {

                //if (e.Column.Key == "column02")
                //    gridEX_Order.SetValue("column002", gridEX_Order.GetValue("column02").ToString());
                //else if (e.Column.Key == "column002")
                //    gridEX_Order.SetValue("column02", gridEX_Order.GetValue("column002").ToString());

                if (e.Column.Key == "column02")
                    gridEX_Order.SetValue("column002", gridEX_Order.GetValue("column02"));
                else if (e.Column.Key == "column002")
                    gridEX_Order.SetValue("column02", gridEX_Order.GetValue("column002"));

                //نام محصول
                if (e.Column.Key == "column02" || e.Column.Key == "column002")
                {
                    object GoodId = gridEX_Order.GetValue("Column02").ToString();
                    DataRowView Row = (DataRowView)gridEX_Order.RootTable.Columns["column02"].DropDown.FindItem(GoodId);
                    gridEX_Order.SetValue("column28",
                    Row["NumberInBox"].ToString());
                    gridEX_Order.SetValue("Column29", Row["NumberInPack"].ToString());



                    if (CustomerGroupCodeList.Count == 0)
                    {
                        gridEX_Order.SetValue("column10",
                            Convert.ToInt64(Convert.ToDouble(Row["SaleBoxPrice"].ToString())));
                        gridEX_Order.SetValue("column09",
                            Convert.ToInt64(Convert.ToDouble(Row["SalePackPrice"].ToString())));
                        gridEX_Order.SetValue("column08",
                        Convert.ToInt64(Convert.ToDouble(Row["SalePrice"].ToString())));
                    }
                    else
                    {
                        CustomerGroupBindingSource.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, @"select * from Table_029_CustomerGroupGoodPricing   
                            where Column01 IN (" + string.Join(",", CustomerGroupCodeArry) + ") and Column02=" +
                            gridEX_Order.GetValue("column02").ToString() +
                            " and Column03<='" + gridEX_Header.GetValue("Column02").ToString() +
                            "' and Column04>='" + gridEX_Header.GetValue("Column02").ToString() + "'  order by Column01,Column02,Column03,Column04");

                        if (CustomerGroupBindingSource.Count > 0)
                        {

                            gridEX_Order.SetValue("column10", Convert.ToInt64(Convert.ToDouble(((DataRowView)CustomerGroupBindingSource.CurrencyManager.Current)["Column06"].ToString())));
                            gridEX_Order.SetValue("column09", Convert.ToInt64(Convert.ToDouble(((DataRowView)CustomerGroupBindingSource.CurrencyManager.Current)["Column05"].ToString())));
                            gridEX_Order.SetValue("column08", Convert.ToInt64(Convert.ToDouble(((DataRowView)CustomerGroupBindingSource.CurrencyManager.Current)["Column07"].ToString())));

                        }
                        else
                        {
                            gridEX_Order.SetValue("column10",
                             Convert.ToInt64(Convert.ToDouble(Row["SaleBoxPrice"].ToString())));
                            gridEX_Order.SetValue("column09",
                              Convert.ToInt64(Convert.ToDouble(Row["SalePackPrice"].ToString())));
                            gridEX_Order.SetValue("column08",
                              Convert.ToInt64(Convert.ToDouble(Row["SalePrice"].ToString())));
                        }
                    }
                    //gridEX_Order.SetValue("column30",
                    //Convert.ToDouble(Row["Tavan"].ToString()) *
                    //Convert.ToDouble(gridEX_Order.GetValue("column04")));
                    //gridEX_Order.SetValue("column32",
                    //    Convert.ToDouble(Row["Hajm"].ToString()) *
                    //    Convert.ToDouble(gridEX_Order.GetValue("column04")));

                    double carton = 1;
                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT column09 from table_004_CommodityAndIngredients where columnid=" + GoodId + "", ConWare);
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
                    TotalGoodRemain = Convert.ToDouble(clDoc.OldTotalGoodRemain(GoodId.ToString(), gridEX_Header.GetValue("Column02").ToString()).Rows[0]["Remain"]);
                    TotalGoodReservations = Convert.ToDouble(clDoc.OldTotalGoodReservations(GoodId.ToString(), Convert.ToInt32(Row1["columnid"])).Rows[0]["Reservations"]);
                    gridEX_Order.SetValue("column30", (TotalGoodRemain) / carton);
                    gridEX_Order.SetValue("column32", (TotalGoodReservations) / carton);
                    gridEX_Order.SetValue("Availabe", (TotalGoodRemain - TotalGoodReservations) / carton);




                }


                double Total = 0;
                Total = (Convert.ToDouble(gridEX_Order.GetValue("column04")) *
                   Convert.ToDouble(gridEX_Order.GetValue("column28"))) +
                     (Convert.ToDouble(gridEX_Order.GetValue("column03")) *
                   Convert.ToDouble(gridEX_Order.GetValue("column29"))) +
                   Convert.ToDouble(gridEX_Order.GetValue("column05"));
                gridEX_Order.SetValue("column06",
                 Total
                 );

                if (e.Column.Key == "column04" || e.Column.Key == "column28"
                   || e.Column.Key == "column03" || e.Column.Key == "column29" || e.Column.Key == "column05")
                {

                    if (gridEX_Order.GetValue("Availabe") != null && !string.IsNullOrWhiteSpace(gridEX_Order.GetValue("Availabe").ToString()))
                    if (Total > Convert.ToDouble(gridEX_Order.GetValue("Availabe")))
                    {
                        MessageBox.Show("موجودی کافی نیست");
                    }

                }
                //gridEX_Order.SetValue("column32",
                //   (Convert.ToDouble(gridEX_Order.GetValue("column04")) *
                //   Convert.ToDouble(((DataRowView)gridEX_Order.RootTable.Columns["column02"].DropDown.FindItem
                //   (gridEX_Order.GetValue("column02")))["Hajm"].ToString())));


                if (!mnu_CalculatePrice.Checked)
                {
                    //gridEX_Order.SetValue("column13",
                    //    Convert.ToDouble(gridEX_Order.GetValue("column04")) *
                    //    Convert.ToDouble(gridEX_Order.GetValue("column10")));

                    gridEX_Order.SetValue("column13",
                   (Convert.ToDouble(gridEX_Order.GetValue("column04")) *
                   Convert.ToDouble(gridEX_Order.GetValue("column10"))) +
                     (Convert.ToDouble(gridEX_Order.GetValue("column03")) *
                   Convert.ToDouble(gridEX_Order.GetValue("column09"))) +
                     (Convert.ToDouble(gridEX_Order.GetValue("column05")) *
                   Convert.ToDouble(gridEX_Order.GetValue("column08")))
                   );

                }
                else
                {
                    gridEX_Order.SetValue("column13",
                            Convert.ToInt64(Convert.ToDouble(gridEX_Order.GetValue("column08").ToString()) *
                         Convert.ToDouble(gridEX_Order.GetValue("column06"))));
                }


                gridEX_Order.SetValue("column21", Class_BasicOperation._UserName);
                gridEX_Order.SetValue("column22", Class_BasicOperation.ServerGetDate());

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Order_Enter(object sender, EventArgs e)
        {
            try
            {
                gridEX_Header.UpdateData();
                this.table_005_OrderHeaderBindingSource.EndEdit();
                //حذف محتویات قبلی
                CustomerGroupCodeArry = CustomerGroupCodeList.ToArray();
                CustomerGroupCodeList.Clear();
                if (CustomerGroupCodeArry.Length > 0)
                    Array.Clear(CustomerGroupCodeArry, 0, CustomerGroupCodeArry.Length);

                DataTable Table = clDoc.ReturnTable(Properties.Settings.Default.BASE,
                    "select Column02 from Table_045_PersonScope where Column01=" +
                    gridEX_Header.GetValue("Column03").ToString());
                foreach (DataRow item in Table.Rows)
                {
                    CustomerGroupCodeList.Add(item["Column02"].ToString());
                }
                if (CustomerGroupCodeList.Count > 0)
                    CustomerGroupCodeArry = CustomerGroupCodeList.ToArray();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Header_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {

                if (gridEX_Header.Col == 17)
                {
                    gridEX_Header.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.None;
                    gridEX_Order.Focus();
                    gridEX_Order.Select();
                    gridEX_Order.MoveToNewRecord();
                }
                else gridEX_Header.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;


            }
            catch
            {
            }
        }

        private void gridEX_Order_FormattingRow(object sender,
            Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            try
            {
                if (e.Row.Cells["column31"].Value.ToString() == "True")
                    e.Row.RowHeaderImageIndex = 0;
            }
            catch
            {
            }
        }

        private void table_005_OrderHeaderBindingSource_PositionChanged(object sender, EventArgs e)
        {
            if (this.table_005_OrderHeaderBindingSource.Count > 0)
            {
                try
                {
                    CustomerGroupCodeList.Clear();
                    CustomerGroupCodeArry = CustomerGroupCodeList.ToArray();
                    //حذف آیتم های قبلی از آرایه
                    if (CustomerGroupCodeArry.Length > 0)
                        Array.Clear(CustomerGroupCodeArry, 0, CustomerGroupCodeArry.Length);

                    DataTable Table = clDoc.ReturnTable(Properties.Settings.Default.BASE, "select Column02 from Table_045_PersonScope where Column01="
                        + gridEX_Header.GetValue("Column03").ToString());
                    foreach (DataRow item in Table.Rows)
                    {
                        CustomerGroupCodeList.Add(item["Column02"].ToString());
                    }
                    if (CustomerGroupCodeList.Count > 0)
                        CustomerGroupCodeArry = CustomerGroupCodeList.ToArray();
                }
                catch { }
            }
        }

        private void bt_ExportPrefactor_Click(object sender, EventArgs e)
        {
            if (table_006_OrderDetailsBindingSource.Count > 0)
            {
                try
                {
                    if (clDoc.OperationalColumnValue("Table_005_OrderHeader", "Column32", gridEX_Header.GetValue("ColumnId").ToString()) != 0)
                        throw new Exception("برای این سفارش، پیش فاکتور صادر شده است");


                    if (gridEX_Header.GetValue("Column13").ToString() == "True")
                        throw new Exception("این سفارش باطل شده است");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به صدور پیش فاکتور بر اساس سفارش جاری هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                        {
                            Con.Open();

                            int PaperNumber = clDoc.MaxNumber(ConSale.ConnectionString, "Table_007_FactorBefore", "Column01");
                            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                            Key.Direction = ParameterDirection.Output;
                            SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_007_FactorBefore Values (" + PaperNumber + ",'" +
                                gridEX_Header.GetValue("Column02").ToString() + "'," + gridEX_Header.GetValue("Column03").ToString() + ",NULL,NULL,'" + "پیش فاکتور صادر شده از سفارش شماره " +
                                gridEX_Header.GetValue("Column01").ToString() + " ', '" + Class_BasicOperation._UserName + "',getdate(),'" +
                                Class_BasicOperation._UserName + "',getdate(), " + gridEX_Header.GetValue("ColumnId").ToString() +
                                ",NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL," +
                                (gridEX_Header.GetRow().Cells["Column08"].Text.Trim() == "" ? "NULL" :
                                gridEX_Header.GetValue("Column08").ToString()) +
                                "); SET @Key=SCOPE_IDENTITY()", Con);
                            Insert.Parameters.Add(Key);
                            Insert.ExecuteNonQuery();
                            int PaperId = int.Parse(Key.Value.ToString());

                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Order.GetRows())
                            {
                                Insert = new SqlCommand("INSERT INTO Table_008_Child1_FactorBefore VALUES(" + PaperId + "," + item.Cells["Column02"].Value.ToString() + "," +
                                    ((DataRowView)gridEX_Order.RootTable.Columns["Column02"].DropDown.FindItem(item.Cells["Column02"].Value))["CountUnit"].ToString() + "," +
                                    item.Cells["Column04"].Value.ToString() + "," + item.Cells["Column03"].Value.ToString() + "," +
                                    item.Cells["Column05"].Value.ToString() + "," + item.Cells["Column06"].Value.ToString() + "," +
                                    item.Cells["Column10"].Value.ToString() + "," + item.Cells["Column09"].Value.ToString() + "," +
                                    item.Cells["Column08"].Value.ToString() + "," + item.Cells["Column13"].Value.ToString() + ",NULL,NULL,NULL,NULL,0,0,0,0,NULL," +
                                    item.Cells["Column13"].Value.ToString() + ",NULL,NULL,NULL,NULL,NULL," + item.Cells["Column28"].Value.ToString() + "," +
                                    item.Cells["Column29"].Value.ToString() + ")", Con);
                                Insert.ExecuteNonQuery();
                            }

                            clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_005_OrderHeader SET Column32=" + PaperId + " where ColumnId=" +
                                gridEX_Header.GetValue("ColumnId").ToString());
                            Class_BasicOperation.ShowMsg("", "پیش فاکتور با شماره " + PaperNumber + " برای این سفارش صادر شد", "Information");



                        }
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }

        }

        private void mnu_ViewPrefactor_Click(object sender, EventArgs e)
        {
            if (((DataRowView)this.table_005_OrderHeaderBindingSource.CurrencyManager.Current)["Column32"].ToString().Trim() == "" ||
                ((DataRowView)this.table_005_OrderHeaderBindingSource.CurrencyManager.Current)["Column32"].ToString().Trim() == "0")
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 64))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Frm_002_ViewPrefactors")
                        {
                            item.BringToFront();
                            return;
                        }
                    }
                    _05_Sale.Frm_002_ViewPrefactors frm = new _05_Sale.Frm_002_ViewPrefactors();
                    try
                    {
                        frm.MdiParent = this.MdiParent;
                    }
                    catch { }
                    frm.Show();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
            else
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 18))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Frm_001_PishFaktor")
                        {
                            item.BringToFront();
                            _05_Sale.Frm_001_PishFaktor frm = (_05_Sale.Frm_001_PishFaktor)item;
                            frm.txt_Search.Text = clDoc.ExScalar(ConSale.ConnectionString, "Table_007_FactorBefore", "Column01", "ColumnId",
                                ((DataRowView)this.table_005_OrderHeaderBindingSource.CurrencyManager.Current)["Column32"].ToString());
                            frm.bt_Search_Click(sender, e);
                            return;
                        }
                    }
                    _05_Sale.Frm_001_PishFaktor frms = new _05_Sale.Frm_001_PishFaktor
                          (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 19),
                          Convert.ToInt32(((DataRowView)this.table_005_OrderHeaderBindingSource.CurrencyManager.Current)["Column32"].ToString()));
                    try
                    {
                        frms.MdiParent = this.MdiParent;
                    }
                    catch { }
                    frms.Show();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
        }

        private void mnu_CalculatePrice_Click(object sender, EventArgs e)
        {
            if (mnu_CalculatePrice.Checked)
                mnu_CalculatePrice.CheckState = CheckState.Unchecked;
            else
                mnu_CalculatePrice.CheckState = CheckState.Checked;

            Properties.Settings.Default.SalePriceCompute = mnu_CalculatePrice.Checked;
            Properties.Settings.Default.Save();

        }

        private void gridEX_Order_CancelingCellEdit(object sender, Janus.Windows.GridEX.ColumnActionCancelEventArgs e)
        {
            try
            {
                if (e.Column.Key == "column02")
                {
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                    Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column002");
                }
            }
            catch { }
        }

        private void gridEX_Order_ColumnButtonClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 141))
            {
                try
                {
                    if (gridEX_Order.GetValue("Column02").ToString() != "")
                    {
                        string Txt = "";
                        DataTable Table = clDoc.GoodRemain(gridEX_Order.GetValue("Column02").ToString(), gridEX_Header.GetValue("Column02").ToString());
                        foreach (DataRow item in Table.Rows)
                        {
                            Txt += " انبار:" + item["WareName"].ToString() + " " + Convert.ToDouble(item["Remain"].ToString()).ToString("#,##0.###")
                                + Environment.NewLine;
                        }

                        if (Txt.Trim() != "")
                            ToastNotification.Show(this, Txt, 3000, eToastPosition.MiddleCenter);
                    }
                }
                catch
                {

                }
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به اطلاعات را ندارید", "None");
        }

        private void bt_Attachments_Click(object sender, EventArgs e)
        {
            if (this.table_005_OrderHeaderBindingSource.Count > 0)
            {
                // if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 95))
                {
                    try
                    {
                        DataRowView Row = (DataRowView)this.table_005_OrderHeaderBindingSource.CurrencyManager.Current;
                        _05_Sale.Form03_OrderAppendix frm = new _05_Sale.Form03_OrderAppendix(
                            int.Parse(Row["ColumnId"].ToString()),
                            int.Parse(Row["Column01"].ToString()));
                        frm.ShowDialog();
                    }
                    catch
                    {
                    }
                }
                //  else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
        }

        private void btn_FromPisgfactor_Click(object sender, EventArgs e)
        {
            try
            {
                bt_New.Enabled = false;
                string _PreFactorCode = InputBox.Show(
                    "در صورت تمایل به بازخوانی اطلاعات مربوط به پیش فاکتور، شماره پیش فاکتور مورد نظر را وارد نمایید", "");
                if (_PreFactorCode.ToString().Trim() != "")
                {
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                    {
                        Con.Open();
                        int _MaxCode = clDoc.MaxNumber(Con.ConnectionString,
                            "Table_005_OrderHeader", "Column01");
                        SqlCommand Select = new SqlCommand(
                            "Select * from Table_007_FactorBefore where Column01=" +
                            _PreFactorCode, Con);
                        SqlDataReader Reader = Select.ExecuteReader();
                        Reader.Read();
                        if (Reader.HasRows)
                        {
                            int _ID;
                            int _PrefactorID = int.Parse(Reader["ColumnId"].ToString());
                            string city = string.Empty;
                            using (SqlConnection Con1 = new SqlConnection(Properties.Settings.Default.BASE))
                            {
                                Con1.Open();
                                SqlCommand CityCommand = new SqlCommand(
                                   @"SELECT ISNULL((Select ISNULL(Column22,0) 
                                from Table_045_PersonInfo where ColumnId=" + Reader["Column03"] + "),0)", Con1);
                                city=CityCommand.ExecuteScalar().ToString();
                            }
                            if (city == "0")
                            {

                                bt_New.Enabled = true;
                                throw new Exception("برای مشتری شهر مقصد مشخص نشده است");
                            }
                            if (Reader["Column23"] == DBNull.Value || Reader["Column23"] == null || string.IsNullOrWhiteSpace(Reader["Column23"].ToString()))
                            {

                                bt_New.Enabled = true;
                                throw new Exception("نوع فروش مشخص نشده است");
                            }
                            SqlDataAdapter Adapter;
                            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                            Key.Direction = ParameterDirection.Output;

                            //***************************INSERT HEADER

                            SqlCommand Insert = new SqlCommand(
                                @"INSERT INTO Table_005_OrderHeader   ([column01]
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
                                                                       ,[column31]
                                                                       ,[column32]
                                                                       ,[column33]
                                                                       ,[column34]
                                                                       ,[column35]
                                                                       ,[column36]
                                                                       ,[column37]
                                                                       ,[column38]
                                                                       ,[column39]
                                                                       ,[column40]
                                                                       ,[column41])VALUES( " + _MaxCode + " ,'" +
                                Reader["Column02"].ToString()
                                + "' , " + Reader["Column03"].ToString() +
                                 @",NULL," + Convert.ToInt16(city) +
                                @",NULL,NULL," + Convert.ToInt16(Reader["Column23"]) + ",0,null,getdate(),null,0,null,null,getdate(),null,null,null,getdate(),null,null,null,getdate(),null,null,null,getdate(),'" +
                                Class_BasicOperation._UserName + "',getdate(),'" +
                                Class_BasicOperation._UserName
                                + @"',"+_PrefactorID+@",0,0,0,0,0,0,   
                                NULL,NULL,NULL); SET @Key= SCOPE_IDENTITY()", Con);
                            Reader.Close();

                            SqlCommand CountCommand = new SqlCommand(
                               @"SELECT ISNULL((Select ISNULL(ColumnId,0) 
from Table_005_OrderHeader where column32=" + _PrefactorID + "),0)", Con);
                            if (CountCommand.ExecuteScalar().ToString() != "0")
                            {

                                bt_New.Enabled = true;
                                throw new Exception("برای این پیش فاکتور، قبلا سفارش صادر شده است");
                            }

                            Insert.Parameters.Add(Key);
                            Insert.ExecuteNonQuery();
                            _ID = int.Parse(Key.Value.ToString());


                            //***************************INSERT GOOD LIST
                            Adapter = new SqlDataAdapter(
                                "Select * from Table_008_Child1_FactorBefore where Column01=" +
                                _PrefactorID, Con);
                            DataTable Child1 = new DataTable();
                            Adapter.Fill(Child1);
                            foreach (DataRow item in Child1.Rows)
                            {
                                SqlCommand ChildInsert = new SqlCommand(
                                    @"INSERT INTO Table_006_OrderDetails  ([column01]
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
                                                                               ,[column31]
                                                                               ,[column32]) VALUES(" + _ID + "," +
                                    item["Column02"].ToString()
                                    + "," + item["Column05"].ToString() + "," +
                                    item["Column04"].ToString() + "," + item["Column06"].ToString() +
                                    "," + item["Column07"].ToString() + ",1," + item["Column10"].ToString() +
                                    "," + item["Column09"].ToString() + "," + item["Column08"].ToString() + ",0,0,"+item["Column11"].ToString() +
                                    @",0,0,0,0,'" + Class_BasicOperation._UserName + "',getdate(),'"
                                    + Class_BasicOperation._UserName + "',getdate(),0,null,getdate(),null,0,0,0,0,0,0)", Con);

                                ChildInsert.ExecuteNonQuery();
                            }

                            //***************************INSERT EXTRA/Reductions

                            clDoc.Update_Des_Table(Con.ConnectionString,
                                "Table_007_FactorBefore", "column11", "ColumnId", _PrefactorID, _ID);




                            dataSet_Foroosh.EnforceConstraints = false;
                            this.table_005_OrderHeaderTableAdapter.FillByID(dataSet_Foroosh.Table_005_OrderHeader, _ID);
                            this.table_006_OrderDetailsTableAdapter.FillByHeaderID(dataSet_Foroosh.Table_006_OrderDetails, _ID);
                            dataSet_Foroosh.EnforceConstraints = true;
                            bt_New.Enabled = true;

                            Class_BasicOperation.ShowMsg("", "عملیات بازخوانی با موفقیت انجام شد", "Information");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }



    }
}
