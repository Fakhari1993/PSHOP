using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._03_Order
{
    public partial class Form02_ViewOrders : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare=new SqlConnection(Properties.Settings.Default.WHRS);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();

        bool _BackSpace = false;

        public Form02_ViewOrders()
        {
            InitializeComponent();
        }

        private void Form01_ViewOrders_Load(object sender, EventArgs e)
        {
            System.Globalization.PersianCalendar persianCalendar =
                new System.Globalization.PersianCalendar();
            string[] Dates = Properties.Settings.Default.ViewOrders.Split('-');
            faDatePickerStrip1.FADatePicker.SelectedDateTime =
                FarsiLibrary.Utils.PersianDate.Parse(Dates[0]);
            faDatePickerStrip2.FADatePicker.SelectedDateTime =
                FarsiLibrary.Utils.PersianDate.Parse(Dates[1]);

            DataSet DataSet01 = new DataSet();
            SqlDataAdapter Adapter = new SqlDataAdapter(
                "Select ColumnId,Column01,Column02,Column29 from Table_045_PersonInfo", ConBase);
            Adapter.Fill(DataSet01, "Customer");
            gridEX1.DropDowns["Customer"].SetDataBinding(DataSet01.Tables["Customer"], "");
            gridEX1_Orders.DropDowns["Customer"].SetDataBinding(DataSet01.Tables["Customer"], "");
            gridEX1.DropDowns["Region"].SetDataBinding(ClDoc.ReturnTable(ConBase.ConnectionString, @"SELECT 
                    Table_160_States.ColumnId, Table_160_States.Column03
                    FROM  Table_160_States"), "");

            Adapter.SelectCommand.CommandText = "Select Column00,Column01 from Table_060_ProvinceInfo";
            Adapter.Fill(DataSet01, "Province");
            gridEX1.DropDowns["Province"].SetDataBinding(DataSet01.Tables["Province"], "");

            Adapter.SelectCommand.CommandText = "Select Column00,Column01,Column02 from Table_065_CityInfo";
            Adapter.Fill(DataSet01, "City");
            gridEX1.DropDowns["City"].SetDataBinding(DataSet01.Tables["City"], "");

            Adapter.SelectCommand.CommandText = @"SELECT     dbo.Table_065_CityInfo.Column01 AS id, 
            dbo.Table_065_CityInfo.Column00 AS idostan,
                        dbo.Table_060_ProvinceInfo.Column01 + ' - ' +
            dbo.Table_065_CityInfo.Column02 AS shahr FROM         
            dbo.Table_060_ProvinceInfo INNER JOIN dbo.Table_065_CityInfo ON 
            dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00";

            Adapter.Fill(DataSet01, "Ostan");
            gridEX1_Orders.DropDowns["City"].SetDataBinding(DataSet01.Tables["Ostan"], "");

            Adapter.SelectCommand.CommandText = "Select Column00,Column01 from Table_115_VehicleType";
            Adapter.Fill(DataSet01, "Vehicle");
            gridEX1.DropDowns["Vehicle"].SetDataBinding(DataSet01.Tables["Vehicle"], "");
            gridEX1_Orders.DropDowns["Vehicle"].SetDataBinding(DataSet01.Tables["Vehicle"], "");

            Adapter = new SqlDataAdapter("SELECT * FROM Table_002_SalesTypes", ConBase);
            Adapter.Fill(DataSet01, "SaleType");
            gridEX1.DropDowns["SaleType"].SetDataBinding(DataSet01.Tables["SaleType"], "");
            gridEX1_Orders.DropDowns["SaleType"].SetDataBinding(DataSet01.Tables["SaleType"], "");


            gridEX2.DropDowns["Good"].SetDataBinding(clGood.GoodInfo(), "");
            gridEX2.DropDowns[1].SetDataBinding(clGood.GoodInfo(), "");

            gridEX1.DropDowns["Prefactor"].SetDataBinding(ClDoc.ReturnTable(ConSale.ConnectionString,
                "Select ColumnId,Column01 from Table_007_FactorBefore"), "");

            this.table_040_PersonGroupsTableAdapter.Fill(this.dataSet_Foroosh.Table_040_PersonGroups);
            gridEX1.DropDowns["Group"].SetDataBinding(table_040_PersonGroupsBindingSource, "");
           
            this.table_045_PersonScopeTableAdapter.Fill(this.dataSet_Foroosh.Table_045_PersonScope);
            this.table_160_StatesTableAdapter.Fill(this.dataSet_Foroosh.Table_160_States);


            toolStripMenuItem1_Click(sender, e);

        }

        private void gridEX2_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {
            try
            {
                if (e.Row.RowType == Janus.Windows.GridEX.RowType.Record &&
                    e.Row.Cells["Column31"].Value.ToString() == "True")
                    e.Row.RowHeaderImageIndex = 0;
            }
            catch { }
        }

       

        private void Form01_ViewOrders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                faDatePickerStrip1.FADatePicker.Select();
                faDatePickerStrip1.Select();
            }
            else if (e.Control && e.KeyCode == Keys.P)
            {
                bt_Print_Click(sender, e);
            }
            else if (e.Control && e.KeyCode == Keys.E)
                bt_Edit_Click(sender, e);
        }

        private void Form01_ViewOrders_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
            gridEX2.RemoveFilters();
            Properties.Settings.Default.ViewOrders = 
                faDatePickerStrip1.FADatePicker.Text + "-" + faDatePickerStrip2.FADatePicker.Text;
            Properties.Settings.Default.Save();
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (this.gridEX1.RowCount > 0)
            {
                Prints.Form_OrderPrint frm = new Prints.Form_OrderPrint(
                    int.Parse(gridEX1.GetRow().Cells["Column01"].Text));
                frm.ShowDialog();
            }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1_Click(sender, e);
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
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox = 
                    (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


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
                    toolStripMenuItem1_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void bt_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.gridEX1.RowCount > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 16))
                    {
                        foreach (Form item in Application.OpenForms)
                        {
                            if (item.Name == "Form01_RegisterOrders")
                            {
                                item.BringToFront();
                                Form01_RegisterOrders frm = (Form01_RegisterOrders)item;
                                frm.txt_Search.Text = gridEX1.GetRow().Cells["Column01"].Text;
                                frm.bt_Search_Click(sender, e);
                                return;
                            }
                        }
                        Form01_RegisterOrders frms = new Form01_RegisterOrders(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 17), Convert.ToInt32(gridEX1.GetValue("columnid")));
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
            catch { }

        }

        private void mnu_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RowCount > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 81))
                        throw new Exception("کاربر گرامی شما امکان ابطال/لغو ابطال یک سفارش را ندارید");

                    if (gridEX1.GetRow().Cells["Column13"].Value.ToString() == "True")
                        throw new Exception("این سفارش قبلاً باطل شده است");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ابطال این سفارش هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        _03_Order.Form05_CancelOrdersDialog frm = new Form05_CancelOrdersDialog();
                        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                        {
                            ClDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_005_OrderHeader SET Column13=1 , Column14='" + Class_BasicOperation._UserName + "', Column15='" +
                                frm.Date + "', Column16=getdate(), Column17=" + (frm.Cause.Trim() == "" ? "NULL" : "'" + frm.Cause + "'") + " where ColumnId=" + gridEX1.GetRow().Cells["ColumnId"].Value.ToString());

                        }
                        int Id = int.Parse(gridEX1.GetValue("ColumnId").ToString());
                        toolStripMenuItem1_Click(sender, e);
                        Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(gridEX1.RootTable.Columns["ColumnId"], Janus.Windows.GridEX.ConditionOperator.Equal, Id);
                        gridEX1.Find(filter, 0, 1);
                        Class_BasicOperation.ShowMsg("", "ابطال سفارش انجام گرفت", "Information");
                    }

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void mnu_CancelCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RowCount > 0)
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 81))
                        throw new Exception("کاربر گرامی شما امکان ابطال/لغو ابطال یک سفارش را ندارید");

                    if (gridEX1.GetValue("Column13").ToString() == "False")
                        throw new Exception("این سفارش باطل نشده است");

                    if (gridEX1.GetValue("Column13").ToString() == "True")
                    {
                        ClDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_005_OrderHeader SET Column13=0 , Column14=NULL, Column15=NULL, Column16=NULL, Column17=NULL where ColumnId=" + gridEX1.GetRow().Cells["ColumnId"].Value.ToString());

                        int Id = int.Parse(gridEX1.GetValue("ColumnId").ToString());
                        toolStripMenuItem1_Click(sender, e);
                        Janus.Windows.GridEX.GridEXFilterCondition filter = new Janus.Windows.GridEX.GridEXFilterCondition(gridEX1.RootTable.Columns["ColumnId"], Janus.Windows.GridEX.ConditionOperator.Equal, Id);
                        gridEX1.Find(filter, 0, 1);
                        Class_BasicOperation.ShowMsg("", "لغو ابطال سفارش انجام گرفت", "Information");
                    }

                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void bt_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void bt_ExportToExcel_Detail_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX2;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip2.FADatePicker.Text.Length == 10 &&
                 faDatePickerStrip1.FADatePicker.Text.Length == 10 &&
                 faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue &&
                 faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {

                try
                {

                    this.table_005_OrderHeaderTableAdapter.Adapter.SelectCommand = new SqlCommand(
                    OrderSelectCommands("All", faDatePickerStrip1.FADatePicker.Text, 
                    faDatePickerStrip2.FADatePicker.Text), ConSale);
                    dataSet_Foroosh.Table_005_OrderHeader.Clear();
                    table_005_OrderHeaderTableAdapter.Adapter.Fill(dataSet_Foroosh);
                    this.table_006_OrderDetailsTableAdapter.Fill(
                        this.dataSet_Foroosh.Table_006_OrderDetails);
                }
                catch
                {
                }

            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip2.FADatePicker.Text.Length == 10 &&
                faDatePickerStrip1.FADatePicker.Text.Length == 10 &&
                faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue &&
                faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                table_005_OrderHeaderTableAdapter.Adapter.SelectCommand = new SqlCommand(
                    OrderSelectCommands("WaitForConfirmFinance",
                    faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text),ConSale);
                dataSet_Foroosh.Table_005_OrderHeader.Clear();
                table_005_OrderHeaderTableAdapter.Adapter.Fill(dataSet_Foroosh);
                //this.table_005_OrderHeaderTableAdapter.FillByWaitFin(this.dataSet_Foroosh.Table_005_OrderHeader,
                //    faDatePickerStrip1.FADatePicker.Text,
                //    faDatePickerStrip2.FADatePicker.Text);
                    this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip2.FADatePicker.Text.Length == 10 &&
                faDatePickerStrip1.FADatePicker.Text.Length == 10 &&
                faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue &&
                faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {

                    //this.table_005_OrderHeaderTableAdapter.FillByConfirmFin(this.dataSet_Foroosh.Table_005_OrderHeader,
                    //    faDatePickerStrip1.FADatePicker.Text,
                    //    faDatePickerStrip2.FADatePicker.Text);
                table_005_OrderHeaderTableAdapter.Adapter.SelectCommand = new SqlCommand(OrderSelectCommands("ConfirmFinance",
                    faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text), ConSale);
                dataSet_Foroosh.Table_005_OrderHeader.Clear();
                table_005_OrderHeaderTableAdapter.Adapter.Fill(dataSet_Foroosh);
                    this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip2.FADatePicker.Text.Length == 10 &&
                faDatePickerStrip1.FADatePicker.Text.Length == 10 &&
                faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue &&
                faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                //this.table_005_OrderHeaderTableAdapter.FillByWaitExit(this.dataSet_Foroosh.Table_005_OrderHeader,
                //    faDatePickerStrip1.FADatePicker.Text,
                //    faDatePickerStrip2.FADatePicker.Text);
                table_005_OrderHeaderTableAdapter.Adapter.SelectCommand = new SqlCommand(OrderSelectCommands("WaitForExit",
                   faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text), ConSale);
                dataSet_Foroosh.Table_005_OrderHeader.Clear();
                table_005_OrderHeaderTableAdapter.Adapter.Fill(dataSet_Foroosh, "Table_005_OrderHeader");
                    this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip2.FADatePicker.Text.Length == 10 &&
                faDatePickerStrip1.FADatePicker.Text.Length == 10 &&
                faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue &&
                faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                //this.table_005_OrderHeaderTableAdapter.FillByExit(this.dataSet_Foroosh.Table_005_OrderHeader,
                //    faDatePickerStrip1.FADatePicker.Text,
                //    faDatePickerStrip2.FADatePicker.Text);

                table_005_OrderHeaderTableAdapter.Adapter.SelectCommand = new SqlCommand(OrderSelectCommands("Exit",
                  faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text), ConSale);
                dataSet_Foroosh.Table_005_OrderHeader.Clear();
                table_005_OrderHeaderTableAdapter.Adapter.Fill(dataSet_Foroosh, "Table_005_OrderHeader");
                    this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip2.FADatePicker.Text.Length == 10 &&
                faDatePickerStrip1.FADatePicker.Text.Length == 10 &&
                faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue &&
                faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                    //this.table_005_OrderHeaderTableAdapter.FillByCancel(this.dataSet_Foroosh.Table_005_OrderHeader,
                    //    faDatePickerStrip1.FADatePicker.Text,
                    //    faDatePickerStrip2.FADatePicker.Text);

                    table_005_OrderHeaderTableAdapter.Adapter.SelectCommand = new SqlCommand(OrderSelectCommands("Cancel",
                    faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text), ConSale);
                    dataSet_Foroosh.Table_005_OrderHeader.Clear();
                    table_005_OrderHeaderTableAdapter.Adapter.Fill(dataSet_Foroosh, "Table_005_OrderHeader");
                    this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);
            }
        }

        private void bt_WaintForConfirmBySaleManager_Click(object sender, EventArgs e)
        {
            if (faDatePickerStrip2.FADatePicker.Text.Length == 10 &&
               faDatePickerStrip1.FADatePicker.Text.Length == 10 &&
               faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue &&
               faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                //this.table_005_OrderHeaderTableAdapter.FillBy_WaitForSaleManagerConfrim(this.dataSet_Foroosh.Table_005_OrderHeader,
                //    faDatePickerStrip1.FADatePicker.Text,
                //    faDatePickerStrip2.FADatePicker.Text);

                table_005_OrderHeaderTableAdapter.Adapter.SelectCommand = new SqlCommand(OrderSelectCommands("WaitForSaleManagerConfirm",
                   faDatePickerStrip1.FADatePicker.Text, faDatePickerStrip2.FADatePicker.Text), ConSale);
                dataSet_Foroosh.Table_005_OrderHeader.Clear();
                table_005_OrderHeaderTableAdapter.Adapter.Fill(dataSet_Foroosh, "Table_005_OrderHeader");
                this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);
            }
        }

        private void gridEX1_RowDoubleClick(object sender, Janus.Windows.GridEX.RowActionEventArgs e)
        {
            bt_Edit_Click(sender, e);
        }

        private string OrderSelectCommands(string WhereType,string Date1,string Date2)
        {
            string CommandText = null;

            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 125))
            {
                CommandText =
                    @"SELECT     dbo.Table_005_OrderHeader.column01, dbo.Table_005_OrderHeader.column02, dbo.Table_005_OrderHeader.column03, dbo.Table_005_OrderHeader.column04, 
                    dbo.Table_005_OrderHeader.column05, dbo.Table_005_OrderHeader.column06, dbo.Table_005_OrderHeader.column07, dbo.Table_005_OrderHeader.column08, 
                    dbo.Table_005_OrderHeader.column09, dbo.Table_005_OrderHeader.column10, dbo.Table_005_OrderHeader.column11, dbo.Table_005_OrderHeader.column12, 
                    dbo.Table_005_OrderHeader.column13, dbo.Table_005_OrderHeader.column14, dbo.Table_005_OrderHeader.column15, dbo.Table_005_OrderHeader.column16, 
                    dbo.Table_005_OrderHeader.column17, dbo.Table_005_OrderHeader.column18, dbo.Table_005_OrderHeader.column19, dbo.Table_005_OrderHeader.column20, 
                    dbo.Table_005_OrderHeader.column21, dbo.Table_005_OrderHeader.column22, dbo.Table_005_OrderHeader.column23, dbo.Table_005_OrderHeader.column24, 
                    dbo.Table_005_OrderHeader.column25, dbo.Table_005_OrderHeader.column26, dbo.Table_005_OrderHeader.column27, dbo.Table_005_OrderHeader.column28, 
                    dbo.Table_005_OrderHeader.column29, dbo.Table_005_OrderHeader.column30, dbo.Table_005_OrderHeader.column31, dbo.Table_005_OrderHeader.column32, 
                    dbo.Table_005_OrderHeader.column33, dbo.Table_005_OrderHeader.column34, dbo.Table_005_OrderHeader.column35, dbo.Table_005_OrderHeader.column36, 
                    dbo.Table_005_OrderHeader.column37, dbo.Table_005_OrderHeader.column38, dbo.Table_005_OrderHeader.columnid, derivedtbl_1.TotalBox, 
                    derivedtbl_1.TotalExitBox, derivedtbl_1.Difference, CityTable.Column00 AS Province, ExitTable.column02 AS ExitDate
                    FROM         dbo.Table_005_OrderHeader INNER JOIN
                    (SELECT     column01, SUM(column04) AS TotalBox, SUM(column16) AS TotalExitBox, SUM(column04) - SUM(column16) AS Difference
                    FROM         dbo.Table_006_OrderDetails
                    GROUP BY column01) AS derivedtbl_1 ON dbo.Table_005_OrderHeader.columnid = derivedtbl_1.column01 LEFT OUTER JOIN
                    (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, 
                    column14, column15, column16, column17, column18, column19, column20, column21, column22, column23, column24
                    FROM         "+ConWare.Database+@".dbo.Table_009_ExitPwhrs) AS ExitTable ON dbo.Table_005_OrderHeader.columnid = ExitTable.column18 LEFT OUTER JOIN
                    (SELECT     Column00, Column01
                    FROM         "+ConBase.Database+@".dbo.Table_065_CityInfo) AS CityTable ON dbo.Table_005_OrderHeader.column05 = CityTable.Column01 
                    WHERE     (NOT (dbo.Table_005_OrderHeader.column03 IN
                    (SELECT     "+ConBase.Database+@".dbo.Table_045_PersonScope.Column01
                    FROM         "+ConBase.Database+@".dbo.Table_040_PersonGroups INNER JOIN
                    "+ConBase.Database+@".dbo.Table_045_PersonScope ON 
                    "+ConBase.Database+@".dbo.Table_040_PersonGroups.Column00 = "+ConBase.Database+
@".dbo.Table_045_PersonScope.Column02
                    WHERE     (" + 
                                 ConBase.Database +
                                  ".dbo.Table_040_PersonGroups.Column29 = 1))))";

                switch (WhereType)
                {
                    case "WaitForConfirmFinance":
                        CommandText += @" And        (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') AND (Table_005_OrderHeader.column09 = 1) AND
                         (Table_005_OrderHeader.column13 = 0) AND (Table_005_OrderHeader.column18 IS NULL) AND (Table_005_OrderHeader.column22 IS NULL) AND 
                         (Table_005_OrderHeader.column33 = 0) ";
                        break;

                    case "ConfirmFinance":
                        CommandText += @" And        (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') AND (Table_005_OrderHeader.column09 = 1) AND
                         (Table_005_OrderHeader.column13 = 0) AND (Table_005_OrderHeader.column18=1) AND (Table_005_OrderHeader.column22 IS NULL) AND 
                         (Table_005_OrderHeader.column33 = 0) ";
                        break;

                    case "WaitForExit":
                        CommandText += @"And (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') AND (Table_005_OrderHeader.column09 = 1)
                         AND (Table_005_OrderHeader.column13 = 0) AND (Table_005_OrderHeader.column18 =1) AND (Table_005_OrderHeader.column22 =1) AND 
                         (Table_005_OrderHeader.column33 = 0)";
                        break;

                    case "Exit":
                        CommandText += @"And (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') AND (Table_005_OrderHeader.column09 = 1)
                         AND (Table_005_OrderHeader.column13 = 0) AND (Table_005_OrderHeader.column18 =1) AND (Table_005_OrderHeader.column22 =1) AND 
                         (Table_005_OrderHeader.column33 = 1)";
                        break;

                    case "Cancel":
                        CommandText += @" And         (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') and (Table_005_OrderHeader.column13 = 1)";
                        break;

                    case "WaitForSaleManagerConfirm":
                        CommandText += @" and         (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') AND (Table_005_OrderHeader.column09 IS NULL) 
                         AND (Table_005_OrderHeader.column13 = 0) AND (Table_005_OrderHeader.column18 IS NULL) AND (Table_005_OrderHeader.column22 IS NULL) AND 
                         (Table_005_OrderHeader.column33 = 0)";
                        break;

                    case "All":
                        CommandText += @" And         (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"')";
                        break;
                }

            }

            else
            {
                CommandText =
                        @"SELECT     dbo.Table_005_OrderHeader.column01, dbo.Table_005_OrderHeader.column02, dbo.Table_005_OrderHeader.column03, dbo.Table_005_OrderHeader.column04, 
                    dbo.Table_005_OrderHeader.column05, dbo.Table_005_OrderHeader.column06, dbo.Table_005_OrderHeader.column07, dbo.Table_005_OrderHeader.column08, 
                    dbo.Table_005_OrderHeader.column09, dbo.Table_005_OrderHeader.column10, dbo.Table_005_OrderHeader.column11, dbo.Table_005_OrderHeader.column12, 
                    dbo.Table_005_OrderHeader.column13, dbo.Table_005_OrderHeader.column14, dbo.Table_005_OrderHeader.column15, dbo.Table_005_OrderHeader.column16, 
                    dbo.Table_005_OrderHeader.column17, dbo.Table_005_OrderHeader.column18, dbo.Table_005_OrderHeader.column19, dbo.Table_005_OrderHeader.column20, 
                    dbo.Table_005_OrderHeader.column21, dbo.Table_005_OrderHeader.column22, dbo.Table_005_OrderHeader.column23, dbo.Table_005_OrderHeader.column24, 
                    dbo.Table_005_OrderHeader.column25, dbo.Table_005_OrderHeader.column26, dbo.Table_005_OrderHeader.column27, dbo.Table_005_OrderHeader.column28, 
                    dbo.Table_005_OrderHeader.column29, dbo.Table_005_OrderHeader.column30, dbo.Table_005_OrderHeader.column31, dbo.Table_005_OrderHeader.column32, 
                    dbo.Table_005_OrderHeader.column33, dbo.Table_005_OrderHeader.column34, dbo.Table_005_OrderHeader.column35, dbo.Table_005_OrderHeader.column36, 
                    dbo.Table_005_OrderHeader.column37, dbo.Table_005_OrderHeader.column38, dbo.Table_005_OrderHeader.columnid, derivedtbl_1.TotalBox, 
                    derivedtbl_1.TotalExitBox, derivedtbl_1.Difference, CityTable.Column00 AS Province, ExitTable.column02 AS ExitDate
                    FROM         dbo.Table_005_OrderHeader INNER JOIN
                    (SELECT     column01, SUM(column04) AS TotalBox, SUM(column16) AS TotalExitBox, SUM(column04) - SUM(column16) AS Difference
                    FROM         dbo.Table_006_OrderDetails
                    GROUP BY column01) AS derivedtbl_1 ON dbo.Table_005_OrderHeader.columnid = derivedtbl_1.column01 LEFT OUTER JOIN
                    (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, 
                    column14, column15, column16, column17, column18, column19, column20, column21, column22, column23, column24
                    FROM         "+ConWare.Database+@".dbo.Table_009_ExitPwhrs) AS ExitTable ON dbo.Table_005_OrderHeader.columnid = ExitTable.column18 LEFT OUTER JOIN
                    (SELECT     Column00, Column01
                    FROM         "+ConBase.Database+@".dbo.Table_065_CityInfo) AS CityTable ON dbo.Table_005_OrderHeader.column05 = CityTable.Column01 ";

                switch (WhereType)
                {
                    case "WaitForConfirmFinance":
                        CommandText += @" where        (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') AND (Table_005_OrderHeader.column09 = 1) AND
                         (Table_005_OrderHeader.column13 = 0) AND (Table_005_OrderHeader.column18 IS NULL) AND (Table_005_OrderHeader.column22 IS NULL) AND 
                         (Table_005_OrderHeader.column33 = 0) ";
                        break;

                    case "ConfirmFinance":
                        CommandText += @" WHERE          (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') AND (Table_005_OrderHeader.column09 = 1) AND
                         (Table_005_OrderHeader.column13 = 0) AND (Table_005_OrderHeader.column18 =1) AND (Table_005_OrderHeader.column22 IS NULL) AND 
                         (Table_005_OrderHeader.column33 = 0)";
                        break;

                    case "WaitForExit":
                        CommandText += @"WHERE (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') AND (Table_005_OrderHeader.column09 = 1)
                         AND (Table_005_OrderHeader.column13 = 0) AND (Table_005_OrderHeader.column18 =1) AND (Table_005_OrderHeader.column22 =1) AND 
                         (Table_005_OrderHeader.column33 = 0)";
                        break;

                    case "Exit":
                        CommandText += @"WHERE (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') AND (Table_005_OrderHeader.column09 = 1)
                         AND (Table_005_OrderHeader.column13 = 0) AND (Table_005_OrderHeader.column18 =1) AND (Table_005_OrderHeader.column22 =1) AND 
                         (Table_005_OrderHeader.column33 =1)";
                        break;

                    case "Cancel":
                        CommandText += @" where         (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') and (Table_005_OrderHeader.column13 = 1)";
                        break;


                    case "WaitForSaleManagerConfirm":
                        CommandText += @" WHERE         (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"') AND (Table_005_OrderHeader.column09 IS NULL) 
                         AND (Table_005_OrderHeader.column13 = 0) AND (Table_005_OrderHeader.column18 IS NULL) AND (Table_005_OrderHeader.column22 IS NULL) AND 
                         (Table_005_OrderHeader.column33 = 0)";
                        break;

                    case "All":
                        CommandText += @" where         (Table_005_OrderHeader.column02 BETWEEN '" + Date1 + "' AND '" + Date2 + @"')";
                        break;
                }
            }
               
           
        
            return CommandText;
        }

    }
}
