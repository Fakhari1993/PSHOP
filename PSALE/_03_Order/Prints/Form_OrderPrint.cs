using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.Xml;

namespace PSHOP._03_Order.Prints
{
    public partial class Form_OrderPrint : Form
    {
       Classes.Class_Settle cl_Settle = new Classes.Class_Settle();
       SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
       SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
       SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        int  _Number;
        DataTable HeaderTable;
        short _StyleNumber = 1;

        public Form_OrderPrint(int OrderNumber)
        {
            InitializeComponent();
            _Number = OrderNumber;
        }
        public Form_OrderPrint()
        {
            InitializeComponent();
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            string HeaderSelectText = @"SELECT     dbo.Table_005_OrderHeader.column01 AS OrderNo, 
                    dbo.Table_005_OrderHeader.column02 AS OrderDate, {0}.dbo.Table_045_PersonInfo.Column01 AS 
                    CustomerCode, {0}.dbo.Table_045_PersonInfo.Column02 AS CustomerName, 

                    {0}.dbo.Table_045_PersonInfo.Column09 AS P2NationalCode,
                       {0}.dbo.Table_045_PersonInfo.Column141 AS P2ECode,
                       {0}.dbo.Table_045_PersonInfo.Column142 AS P2SabtCode,
                   {0}.dbo.Table_002_SalesTypes.column02 AS SaleType, dbo.Table_005_OrderHeader.column06 AS SendTo, 
                     ISNULL(
                                           (
                                               SELECT tpi.Column01
                                               FROM   {0}.dbo.Table_060_ProvinceInfo tpi
                                               WHERE  tpi.Column00 = {0}.dbo.Table_045_PersonInfo.Column21
                                           ),
                                           ' '
                                       ) + ' ' + ISNULL(
                                           (
                                               SELECT tpi.Column02
                                               FROM   {0}.dbo.Table_065_CityInfo tpi
                                               WHERE  tpi.Column01 = {0}.dbo.Table_045_PersonInfo.Column22
                                           ),
                                           ' '
                                       ) + ' ' + ISNULL({0}.dbo.Table_045_PersonInfo.Column06, ' ') AS Address, dbo.Table_005_OrderHeader.column26 AS 
                    SaleDes, dbo.Table_005_OrderHeader.column27 AS WareDes, {0}.dbo.Table_115_VehicleType.Column01 AS 
                    SendType, 
                                            {1}.dbo.table_004_CommodityAndIngredients.column01 AS GoodCode, 
                    {1}.dbo.table_004_CommodityAndIngredients.column02 AS GoodName, 
                    dbo.Table_006_OrderDetails.column04 AS CartoonNum, dbo.Table_006_OrderDetails.column06 AS TotalNum, 
                    dbo.Table_006_OrderDetails.column28 AS NumInCartoon, 
                    dbo.Table_005_OrderHeader.columnid,Table_006_OrderDetails.Column03 as PackNumber,
                    Table_006_OrderDetails.Column05 as Detail,Table_006_OrderDetails.Column10 BoxPrice,
                    Table_006_OrderDetails.Column09 as PackPrice,Table_006_OrderDetails.Column08 as SinglePrice,
                    Table_006_OrderDetails.Column13 as TotalPrice,dbo.Table_005_OrderHeader.column12 as operatorforosh,dbo.Table_005_OrderHeader.column21 as operatormali
                    ,dbo.Table_005_OrderHeader.column29 as operatorsabt
                    FROM         dbo.Table_005_OrderHeader INNER JOIN
                                            dbo.Table_006_OrderDetails ON dbo.Table_005_OrderHeader.columnid = 
                    dbo.Table_006_OrderDetails.column01 INNER JOIN
                                            {0}.dbo.Table_045_PersonInfo ON dbo.Table_005_OrderHeader.column03 = 
                    {0}.dbo.Table_045_PersonInfo.ColumnId LEFT OUTER JOIN
                                            {0}.dbo.Table_002_SalesTypes ON dbo.Table_005_OrderHeader.column08 = 
                  {0}.  dbo.Table_002_SalesTypes.columnid LEFT OUTER JOIN
                                            {0}.dbo.Table_115_VehicleType ON dbo.Table_005_OrderHeader.column07 = 
                    {0}.dbo.Table_115_VehicleType.Column00 INNER JOIN
                                            {1}.dbo.table_004_CommodityAndIngredients ON 
                    dbo.Table_006_OrderDetails.column02 = {1}.dbo.table_004_CommodityAndIngredients.columnid  ";

            HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);


            if (rdb_CurrentNumber.Checked)
            {
                HeaderSelectText += " WHERE     (dbo.Table_005_OrderHeader.column01 = " + _Number + ")";

            }
            else if (rdb_FromNumber.Checked &&  txt_From.Text.Trim() != "" && txt_To.Text.Trim() != "")
            {
                HeaderSelectText += " WHERE     (dbo.Table_005_OrderHeader.column01 between  " + txt_From.Text + " and " + txt_To.Text + ")";
            }
            else if (rdb_Date.Checked && faDatePicker2.SelectedDateTime.HasValue && faDatePicker1.SelectedDateTime.HasValue &&
                faDatePicker1.Text.Length == 10 && faDatePicker2.Text.Length == 10)
            {
                HeaderSelectText += " WHERE     (dbo.Table_005_OrderHeader.Column02 between  '" + faDatePicker1.Text
                    + "' and '" + faDatePicker2.Text+ "')";
            }
            HeaderSelectText += " ORDER BY " + ConWare.Database + ".dbo.table_004_CommodityAndIngredients.column02,dbo.Table_006_OrderDetails.column04 ";
            HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);


            switch (_StyleNumber)
            {
                case 1:
                    bt_First_Click(sender, e);
                    break;
                case 2:
                    bt_Second_Click(sender, e);
                    break;
            }
        }

        private void Form_FactorPrint_Load(object sender, EventArgs e)
        {
            faDatePicker1.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePicker2.SelectedDateTime = DateTime.Now;
            _StyleNumber = Properties.Settings.Default.OrderStyle;
            chk_Logo.Checked = Properties.Settings.Default.DontShowLogo;
            bt_Display_Click(sender, e);
        }

        private void Form_FactorPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.DontShowLogo = chk_Logo.Checked;
            Properties.Settings.Default.OrderStyle = _StyleNumber;
            Properties.Settings.Default.Save();
        }

        private void txt_From_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker1.HideDropDown();
                    faDatePicker2.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = (FarsiLibrary.Win.Controls.FADatePicker)sender;


                if (textBox.Text.Length == 4)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                else if (textBox.Text.Length == 7)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void faDatePicker2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePicker2.HideDropDown();
                    bt_Display.Focus();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void bt_First_Click(object sender, EventArgs e)
        {
            //Show Reports
            _03_Order.Prints.CrystalReport_001_PrintSefaresh Rpt1 = new CrystalReport_001_PrintSefaresh();
            DataTable Org = Class_BasicOperation.LogoTable();
            if (!chk_Logo.Checked)
                Rpt1.Subreports[0].SetDataSource(Org);
            else Rpt1.Subreports[0].SetDataSource(Org.Clone());
            Rpt1.SetDataSource(HeaderTable);
            crystalReportViewer1.ReportSource = Rpt1;
            _StyleNumber = 1;
        }

        private void bt_Second_Click(object sender, EventArgs e)
        {
            _03_Order.Prints.Rpt_02_PrintOrder Rpt1 = new Rpt_02_PrintOrder();
            DataTable Org = Class_BasicOperation.LogoTable();
       
            if (!chk_Logo.Checked)
                Rpt1.Subreports[0].SetDataSource(Org);
            else Rpt1.Subreports[0].SetDataSource(Org.Clone());

            Rpt1.SetDataSource(HeaderTable);
            crystalReportViewer1.ReportSource = Rpt1;
            _StyleNumber = 2;
        }
    }
}