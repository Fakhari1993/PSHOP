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
    public partial class Form04_ConfirmByFinManager : Form
    {
        Class_UserScope UserScope = new Class_UserScope();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        DataTable CustomerTable = new DataTable();

        public Form04_ConfirmByFinManager()
        {
            InitializeComponent();
        }

        private void Form04_ConfirmByFinManager_Load(object sender, EventArgs e)
        {
            //جدول اطلاعات مشتری
            CustomerTable = clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT     dbo.Table_045_PersonInfo.ColumnId AS PersonId, dbo.Table_060_ProvinceInfo.Column01 AS Province, dbo.Table_065_CityInfo.Column02 AS City, 
            dbo.Table_045_PersonInfo.Column06 AS Address, dbo.Table_045_PersonInfo.Column07 AS Tel, dbo.Table_045_PersonInfo.Column08 AS Fax, 
            dbo.Table_045_PersonInfo.Column13 AS PostalCode, dbo.Table_045_PersonInfo.Column02 AS Name
            FROM         dbo.Table_065_CityInfo INNER JOIN
            dbo.Table_060_ProvinceInfo ON dbo.Table_065_CityInfo.Column00 = dbo.Table_060_ProvinceInfo.Column00 RIGHT OUTER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22");

            this.table_050_CerditInfoTableAdapter.Fill(this.dataSet_Foroosh.Table_050_CerditInfo);


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

            gridEX_Header.DropDowns["d4"].DataSource = DS.Tables["Customer"];

            gridEX_Remain.DropDowns["ACC_Name"].SetDataBinding(clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ACC_Code,ACC_Name from  dbo.AllHeaders()"), "");

            //عدم مشاهده سفارشات اشخاص خاص
            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 125))
            {
                table_005_OrderHeaderTableAdapter.Adapter.SelectCommand = new SqlCommand(
                    @"SELECT column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11,
                    column12, column13, column14, column15, column16, column17, column18, column19, column20, column21, column22, column23,
                    column24, column25, column26, column27, column28, column29, column30, column31, column32, column33, column34, 
                    column35, column36, column37, column38, columnid FROM Table_005_OrderHeader
                    WHERE (column13 = 0) AND (column33 = 0) AND (column22 IS NULL) AND (column09 = 1)
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
                   @"SELECT column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11,
                    column12, column13, column14, column15, column16, column17, column18, column19, column20, column21, column22, column23,
                    column24, column25, column26, column27, column28, column29, column30, column31, column32, column33, column34, 
                    column35, column36, column37, column38, columnid FROM Table_005_OrderHeader
                    WHERE (column13 = 0) AND (column33 = 0) AND (column22 IS NULL) AND (column09 = 1)"
                   , ConSale);
                dataSet_Foroosh.Table_005_OrderHeader.Clear();
                table_005_OrderHeaderTableAdapter.Adapter.Fill(dataSet_Foroosh, "Table_005_OrderHeader");
                this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);
            }

         

        }

        private void gridEX_Header_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            try
            {
                if (gridEX_Header.GetValue("Column18").ToString() == "True" || gridEX_Header.GetValue("Column18").ToString() == "False")
                {
                    gridEX_Header.SetValue("Column19", FarsiLibrary.Utils.PersianDate.Now.ToString("0000/00/00"));
                    gridEX_Header.SetValue("Column20", Class_BasicOperation.ServerDate());
                    gridEX_Header.SetValue("Column21", Class_BasicOperation._UserName);

                }
                else
                {
                    gridEX_Header.SetValue("Column18", DBNull.Value);
                    gridEX_Header.SetValue("Column19", DBNull.Value);
                    gridEX_Header.SetValue("Column20", DBNull.Value);
                    gridEX_Header.SetValue("Column21", DBNull.Value);

                }
            }
            catch
            {
                gridEX_Header.SetValue("Column18", DBNull.Value);
                gridEX_Header.SetValue("Column19", DBNull.Value);
                gridEX_Header.SetValue("Column20", DBNull.Value);
                gridEX_Header.SetValue("Column21", DBNull.Value);
            }

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (this.table_005_OrderHeaderBindingSource.Count > 0)
            {
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 14))
                        throw new Exception("کاربر گرامی شما امکان ذخیره تغییرات را ندارید");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
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
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Header.RootTable.Columns["Column18"], ConditionOperator.Equal, true);
                lbl_CheckedStatus.Text = "تعداد تأیید شده:" + gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column18"], AggregateFunction.Count, Filter).ToString();
                Filter.Clear();
                Filter = new GridEXFilterCondition(gridEX_Header.RootTable.Columns["Column18"], ConditionOperator.Equal, 0);
                lbl_UnCheckedStatus.Text = "تعداد تأیید نشده:" + gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column18"], AggregateFunction.Count, Filter).ToString();
                Filter.Clear();
                Filter = new GridEXFilterCondition(gridEX_Header.RootTable.Columns["Column18"], ConditionOperator.IsNull, null);
                lbl_MilddleStatus.Text = "تعداد بررسی نشده:" + gridEX_Header.GetTotal(gridEX_Header.RootTable.Columns["Column18"], AggregateFunction.Count, Filter).ToString();
                Filter.Clear();
            }
            catch { }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            //this.table_005_OrderHeaderTableAdapter.FillBy_FinanManager(dataSet_Foroosh.Table_005_OrderHeader);
            //this.table_006_OrderDetailsTableAdapter.Fill(dataSet_Foroosh.Table_006_OrderDetails);
            //this.table_050_CerditInfoTableAdapter.Fill(dataSet_Foroosh.Table_050_CerditInfo);
             dataSet_Foroosh.Table_005_OrderHeader.Clear();
                table_005_OrderHeaderTableAdapter.Adapter.Fill(dataSet_Foroosh, "Table_005_OrderHeader");
                this.table_006_OrderDetailsTableAdapter.Fill(this.dataSet_Foroosh.Table_006_OrderDetails);
        }

        private void Form04_ConfirmByFinManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.F5)
                bt_Refresh_Click(sender, e);
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
                DataTable Table2=new DataTable();
                Adapter.Fill(Table2);
                gridEX_Chq.DataSource = Table2; 

                //نمایش اطلاعات مشتری
                CustomerTable.DefaultView.RowFilter = "PersonId=" + gridEX_Header.GetValue("Column03").ToString();
                gridEX_Customer.DataSource = CustomerTable.DefaultView;

            }
            catch (Exception)
            {
            }
        }

        private void bt_ViewPersonBook_Click_1(object sender, EventArgs e)
        {
            try
            {
                PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
                PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;

                string Date = Class_BasicOperation._Year + "/01/01";
             

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 41))
                {
                    PACNT._4_Person_Menu.Form02_PersonBook frm = new PACNT._4_Person_Menu.Form02_PersonBook(
                         int.Parse(gridEX_Header.GetValue("Column03").ToString()),
                         FarsiLibrary.Utils.PersianDate.Parse(Date),
                          DateTime.Now);
                    frm.ShowDialog();
                }
                else
                    Class_BasicOperation.ShowMsg("",
                        "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
            catch { }
        }

    
    }
}
