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
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Design;
using Stimulsoft.Database;

namespace PSALE._08_Order2
{
    public partial class Form01_RegisterOrders : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();

        bool _del = false;
        int _id = -1;

        public Form01_RegisterOrders(bool del, int id)
        {
            _id = id;
            _del = del;
            InitializeComponent();
        }

        public Form01_RegisterOrders()
        {
            InitializeComponent();
        }

        private void Form06_RegisterOrders2_Load(object sender, EventArgs e)
        {
           
            try
            {

            DataSet DS = new DataSet();
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT [columnid]
                                                              ,[column01]
                                                              ,[column02] from Table_002_SalesTypes", ConBase);
            Adapter.Fill(DS, "SaleType");
            mlt_SaleType.DataSource = DS.Tables["SaleType"];


            mlt_WHR.DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from Table_001_PWHRS");
            mlt_DraftType.DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from table_005_PwhrsOperation where Column16=1");

            mlt_SaleBoss.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
                                                                                           dbo.Table_045_PersonInfo.Column01 AS code,
                                                                                           dbo.Table_045_PersonInfo.Column02 AS name
                                                                                    FROM   dbo.Table_045_PersonInfo
                                                                                    WHERE  dbo.Table_045_PersonInfo.ColumnId 
                                                                                           IN (SELECT tps.Column01
                                                                                               FROM   Table_045_PersonScope tps
                                                                                               WHERE  tps.Column02 = 3)");
            DataTable CustomerTable = clDoc.ReturnTable
           (ConBase.ConnectionString, @"SELECT     dbo.Table_045_PersonInfo.ColumnId AS id, dbo.Table_045_PersonInfo.Column01 AS code, dbo.Table_045_PersonInfo.Column02 AS name, 
                      dbo.Table_065_CityInfo.Column02 AS shahr, dbo.Table_060_ProvinceInfo.Column01 AS ostan, dbo.Table_045_PersonInfo.Column06 AS Address, 
                      dbo.Table_045_PersonInfo.Column30,Table_045_PersonInfo.Column07 
                        FROM         dbo.Table_060_ProvinceInfo INNER JOIN
                                              dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00 INNER JOIN
                                              dbo.Table_045_PersonInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                        WHERE     (dbo.Table_045_PersonInfo.Column12 = 1)");
            this.mlt_Customer.DataSource = CustomerTable;

            // TODO: This line of code loads data into the 'dataSet_Foroosh.Table_005_OrderHeader' table. You can move, or remove it, as needed.
           


                mnu_CalculatePrice.Checked = Properties.Settings.Default.SalePriceCompute;




                DataTable GoodTable = clGood.MahsoolInfo();
                gridEX_Order.DropDowns[0].SetDataBinding(GoodTable, "");
                gridEX_Order.DropDowns[1].SetDataBinding(GoodTable, "");

                this.WindowState = FormWindowState.Maximized;

                if (_id != -1)
                {
                    this.table_005_OrderHeaderTableAdapter1.FillByID(
                        this.dataSet_Foroosh.Table_005_OrderHeader, _id);
                    this.table_006_OrderDetailsTableAdapter1.FillByHeaderID(
                        this.dataSet_Foroosh.Table_006_OrderDetails, _id);
                }
                //else
                //    bt_New_Click(this, e);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }



        }

        private void maskedEditBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void bt_New_Click(object sender, EventArgs e)
        {
            table_005_OrderHeaderBindingSource1.AddNew();
            txt_SabtUser.Text = Class_BasicOperation._UserName;
            txt_SabtDate.Text = Class_BasicOperation.ServerDate().ToString();
            column30DateTimePicker.Value = Class_BasicOperation.ServerDate();
            column31TextBox.Text = Class_BasicOperation._UserName;
            txt_Date.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##");
            mlt_Customer.Select();
        }

        private void gridEX_Order_Enter(object sender, EventArgs e)
        {
            try
            {
                if (!txt_Date.IsTextValid() || txt_Date.Text == string.Empty)
                    throw new Exception("تاریخ را وارد کنید");
                    
                this.table_005_OrderHeaderBindingSource1.EndEdit();

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void gridEX_Order_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_Order_AddingRecord(object sender, CancelEventArgs e)
        {
            gridEX_Order.SetValue("column19", Class_BasicOperation._UserName.ToString());
            gridEX_Order.SetValue("column20", Class_BasicOperation.ServerGetDate());
        }

        private void Form06_RegisterOrders2_KeyDown(object sender, KeyEventArgs e)
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
            //else if (e.Control && e.KeyCode == Keys.E)
            //    bt_ExportPrefactor_Click(sender, e);
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
                bt_Search_Click(sender, e);
        }

        public void bt_Search_Click(object sender, EventArgs e)
        {
            if (txt_Search.Text.Trim() != "")
            {
                try
                {
                    int _id = ReturnIDNumber(int.Parse(txt_Search.Text));
                    dataSet_Foroosh.EnforceConstraints = false;
                    this.table_005_OrderHeaderTableAdapter1.FillByID(dataSet_Foroosh.Table_005_OrderHeader, _id);
                    this.table_006_OrderDetailsTableAdapter1.FillByHeaderID(dataSet_Foroosh.Table_006_OrderDetails, _id);
                    dataSet_Foroosh.EnforceConstraints = true;
                    txt_Search.SelectAll();
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);
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

        private void gridEX_Order_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
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

        private void gridEX_Order_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
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


                    gridEX_Order.SetValue("column10",
                        Convert.ToInt64(Convert.ToDouble(Row["SaleBoxPrice"].ToString())));
                    gridEX_Order.SetValue("column09",
                        Convert.ToInt64(Convert.ToDouble(Row["SalePackPrice"].ToString())));
                    gridEX_Order.SetValue("column08",
                    Convert.ToInt64(Convert.ToDouble(Row["SalePrice"].ToString())));
                    DataRowView Row1 = (DataRowView)this.table_005_OrderHeaderBindingSource1.CurrencyManager.Current;

                    double TotalGoodRemain = 0;
                    double TotalGoodReservations = 0;
                    TotalGoodRemain = Convert.ToDouble(clDoc.TotalGoodRemain(GoodId.ToString(), txt_Date.Text,Convert.ToInt16(mlt_WHR.Value)).Rows[0]["Remain"]);
                    TotalGoodReservations = Convert.ToDouble(clDoc.TotalGoodReservations(GoodId.ToString(), Convert.ToInt32(Row1["columnid"])).Rows[0]["Reservations"]);
                    gridEX_Order.SetValue("column30",Convert.ToDecimal( TotalGoodRemain));
                    gridEX_Order.SetValue("column32",Convert.ToDecimal( TotalGoodReservations));
                    gridEX_Order.SetValue("Available", (TotalGoodRemain - TotalGoodReservations));


                }
                if (e.Column.Key == "column04" || e.Column.Key == "column28"
                    || e.Column.Key == "column03" || e.Column.Key == "column29" || e.Column.Key == "column05")
                {
                    double Total = 0;
                    Total = (Convert.ToDouble(gridEX_Order.GetValue("column04")) *
                       Convert.ToDouble(gridEX_Order.GetValue("column28"))) +
                         (Convert.ToDouble(gridEX_Order.GetValue("column03")) *
                       Convert.ToDouble(gridEX_Order.GetValue("column29"))) +
                       Convert.ToDouble(gridEX_Order.GetValue("column05"));
                    gridEX_Order.SetValue("column06", Total);

                    if (Total > Convert.ToDouble(gridEX_Order.GetValue("Available")))
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
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {

            try
            {

                if (!txt_Date.IsTextValid() || txt_Date.Text == string.Empty)
                {
                    Class_BasicOperation.ShowMsg("", "تاریخ را وارد کنید", "Information");
                    return;
                }
                if ((mlt_WHR.Value == null || mlt_WHR.Value.ToString() == string.Empty))
                {
                    Class_BasicOperation.ShowMsg("", "انبار را انتخاب کنید", "Information");
                    return;

                }

                if (mlt_DraftType == null || mlt_DraftType.Value.ToString() == string.Empty)
                {
                    Class_BasicOperation.ShowMsg("", "نوع حواله را انتخاب کنید", "Information");
                    return;
                }


                DataRowView Row = (DataRowView)this.table_005_OrderHeaderBindingSource1.CurrencyManager.Current;

                if (int.Parse(txt_Num.Value.ToString()) < 0)
                {
                    int _MaxOrderNo = 0;
                    SqlCommand MaxOrderNoCmd = new SqlCommand(
                        "SELECT IsNull(MAX(column01),0) AS MaxOrderNo FROM dbo.Table_005_OrderHeader",
                        new SqlConnection(Properties.Settings.Default.SALE));
                    MaxOrderNoCmd.Connection.Open();
                    _MaxOrderNo = int.Parse(MaxOrderNoCmd.ExecuteScalar().ToString());
                    MaxOrderNoCmd.Connection.Close();

                    _MaxOrderNo += 1;
                    txt_Num.Value = _MaxOrderNo;
                }
                else
                {
                    bool ok = true;
                    SqlCommand HavaleID = new SqlCommand(@"IF EXISTS (
                                                               SELECT *
                                                               FROM   Table_007_PwhrsDraft tpd
                                                               WHERE  tpd.column17 = " + Row["columnid"] + @"
                                                           )
                                                            SELECT 0 AS ok
                                                        ELSE
                                                            SELECT 1 AS ok ", new SqlConnection(Properties.Settings.Default.WHRS))

                         ;

                    HavaleID.Connection.Open();
                    ok = Convert.ToBoolean(HavaleID.ExecuteScalar());
                    HavaleID.Connection.Close();
                    if (!ok)
                    {
                        Class_BasicOperation.ShowMsg("", "برای سفارش حواله صادر شده است امکان ویرایش وجود ندارد", "Information");
                        return;
                    }


                }
                string goodname = string.Empty;
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Order.GetRows())
                {
                    double TotalGoodRemain = 0;
                    double TotalGoodReservations = 0;
                    TotalGoodRemain = Convert.ToDouble(clDoc.TotalGoodRemain(item.Cells["Column02"].Value.ToString(), txt_Date.Text,Convert.ToInt16(mlt_WHR.Value)).Rows[0]["Remain"]);
                    TotalGoodReservations = Convert.ToDouble(clDoc.TotalGoodReservations(item.Cells["Column02"].Value.ToString(), Convert.ToInt32(Row["columnid"])).Rows[0]["Reservations"]);
                    if (Convert.ToDouble(item.Cells["column06"].Value) > (TotalGoodRemain - TotalGoodReservations))
                    {
                        goodname += item.Cells["column002"].Text + " " + item.Cells["column02"].Text + " ";
                    }
                }

                if ((goodname != string.Empty && DialogResult.Yes == MessageBox.Show("موجودی کالاهای زیر کافی نیست آیا مایل به ثبت سفارش هستید؟" + Environment.NewLine + goodname, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)) || goodname == string.Empty)
                {

                    table_005_OrderHeaderBindingSource1.EndEdit();
                    table_006_OrderDetailsBindingSource1.EndEdit();
                    table_005_OrderHeaderTableAdapter1.Update(this.dataSet_Foroosh.Table_005_OrderHeader);
                    table_006_OrderDetailsTableAdapter1.Update(this.dataSet_Foroosh.Table_006_OrderDetails);

                    if (sender == bt_Save || sender == this)
                        Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد", "Information");
                }
                else
                {
                    Class_BasicOperation.ShowMsg("", "موجودی کالاهای  " + goodname + " کافی نیست", "Information");

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            try
            {
                DataRowView Row = (DataRowView)this.table_005_OrderHeaderBindingSource1.CurrencyManager.Current;

                bool ok = true;
                SqlCommand HavaleID = new SqlCommand(@"IF EXISTS (
                                                               SELECT *
                                                               FROM   Table_007_PwhrsDraft tpd
                                                               WHERE  tpd.column17 = " + Row["columnid"] + @"
                                                           )
                                                            SELECT 0 AS ok
                                                        ELSE
                                                            SELECT 1 AS ok ", new SqlConnection(Properties.Settings.Default.WHRS))

                                                         ;

                HavaleID.Connection.Open();
                ok = Convert.ToBoolean(HavaleID.ExecuteScalar());
                HavaleID.Connection.Close();
                if (!ok)
                {
                    Class_BasicOperation.ShowMsg("", "برای سفارش حواله صادر شده است امکان حذف وجود ندارد", "Information");
                    return;
                }


                if (_del)
                {
                    if (this.table_005_OrderHeaderBindingSource1.Count > 0)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف این سفارش هستید؟",
                            "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            foreach (DataRowView item in this.table_006_OrderDetailsBindingSource1)
                            {
                                item.Delete();
                            }

                            table_006_OrderDetailsBindingSource1.EndEdit();
                            this.table_005_OrderHeaderBindingSource1.RemoveCurrent();

                            table_005_OrderHeaderBindingSource1.EndEdit();
                            tableAdapterManager3.UpdateAll(dataSet_Foroosh);

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
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_005_OrderHeaderBindingSource1.Count > 0)
                {
                    bt_Save_Click(sender, e);
                    Prints.Form_OrderPrint frm = new Prints.Form_OrderPrint(
                        int.Parse(txt_Num.Value.ToString()));
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.ShowMsg("", ex.Message, "Warning");
            }
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
                        DataTable Table = clDoc.GoodRemain(gridEX_Order.GetValue("Column02").ToString(), txt_Date.Text);
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

        private void mlt_Customer_ValueChanged(object sender, EventArgs e)
        {
            if (((System.Data.DataRowView)(mlt_Customer.SelectedItem)) != null)
            {
                txt_tell.Text = ((System.Data.DataRowView)(mlt_Customer.SelectedItem)).Row.ItemArray[7].ToString();
                txt_Address.Text = ((System.Data.DataRowView)(mlt_Customer.SelectedItem)).Row.ItemArray[6].ToString();
            }

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


                mlt_SaleBoss.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
                                                                                           dbo.Table_045_PersonInfo.Column01 AS code,
                                                                                           dbo.Table_045_PersonInfo.Column02 AS name
                                                                                    FROM   dbo.Table_045_PersonInfo
                                                                                    WHERE  dbo.Table_045_PersonInfo.ColumnId 
                                                                                           IN (SELECT tps.Column01
                                                                                               FROM   Table_045_PersonScope tps
                                                                                               WHERE  tps.Column02 = 3)");
                DataTable CustomerTable = clDoc.ReturnTable
               (ConBase.ConnectionString, @"SELECT     dbo.Table_045_PersonInfo.ColumnId AS id, dbo.Table_045_PersonInfo.Column01 AS code, dbo.Table_045_PersonInfo.Column02 AS name, 
                      dbo.Table_065_CityInfo.Column02 AS shahr, dbo.Table_060_ProvinceInfo.Column01 AS ostan, dbo.Table_045_PersonInfo.Column06 AS Address, 
                      dbo.Table_045_PersonInfo.Column30,Table_045_PersonInfo.Column07 
                        FROM         dbo.Table_060_ProvinceInfo INNER JOIN
                                              dbo.Table_065_CityInfo ON dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00 INNER JOIN
                                              dbo.Table_045_PersonInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                        WHERE     (dbo.Table_045_PersonInfo.Column12 = 1)");
                this.mlt_Customer.DataSource = CustomerTable;

            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void editBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //gridEX_Order.Select();
            //gridEX_Order.Col = 3;
            if (e.KeyChar == 13)
            {
                gridEX_Order.Focus();
                gridEX_Order.Select();
                 gridEX_Order.Col = 3;
            }

        }

        private void txt_Address_TextChanged(object sender, EventArgs e)
        {

        }

        private void mnu_SaleType_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 3))
            {
                _02_BasicInfo.Frm_007_SaleType ob =
                    new _02_BasicInfo.Frm_007_SaleType(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 4));
                ob.ShowDialog();

                DataSet DS = new DataSet();
                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT [columnid]
                                                              ,[column01]
                                                              ,[column02] from Table_002_SalesTypes", ConBase);
                Adapter.Fill(DS, "SaleType");
                mlt_SaleType.DataSource = DS.Tables["SaleType"];

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

                _08_Order2.Form02_ViewOrders frm = new _08_Order2.Form02_ViewOrders();
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

        private void mnu_GoodInfo_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 5))
            {
                _02_BasicInfo.Frm_009_AdditionalGoodsInfo ob =
                    new _02_BasicInfo.Frm_009_AdditionalGoodsInfo(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 6));
                ob.ShowDialog();

                DataTable GoodTable = clGood.MahsoolInfo();
                gridEX_Order.DropDowns[0].SetDataBinding(GoodTable, "");
                gridEX_Order.DropDowns[1].SetDataBinding(GoodTable, "");


            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void viewPrint_Click(object sender, EventArgs e)
        {

            {
                try
                {
                    if (table_005_OrderHeaderBindingSource1.Count > 0)
                    {
                        _08_Order2.DataSet_Foroosh ds = new DataSet_Foroosh();

                        DataTable dtOrder = ds.Order.Clone();
                        this.Cursor = Cursors.WaitCursor;

                        foreach (Janus.Windows.GridEX.GridEXRow dr in gridEX_Order.GetRows())
                        {
                            dtOrder.Rows.Add(txt_Num.Text, txt_Date.Text, mlt_Customer.Text, txt_tell.Text, txt_Address.Text, dr.Cells["Column002"].Text, dr.Cells["Column02"].Text, dr.Cells["Column04"].Value, dr.Cells["Column05"].Value, dr.Cells["Column08"].Value, dr.Cells["Column13"].Value);
                        }

                        if (dtOrder.Rows.Count > 0)
                        {

                            StiReport stireport = new StiReport();
                            stireport.Load("Order2.mrt");
                            stireport.Pages["Page1"].Enabled = true;
                            stireport.Compile();
                            StiOptions.Viewer.AllowUseDragDrop = false;
                            stireport.RegData("Order", dtOrder);



                            this.Cursor = Cursors.Default;
                            stireport.Render();

                            stireport.Show();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void btDesign_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("Order2.mrt");
            stireport.Design();
        }

        
    }
}
