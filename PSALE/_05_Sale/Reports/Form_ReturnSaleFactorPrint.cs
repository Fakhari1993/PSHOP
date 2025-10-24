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

namespace PSHOP._05_Sale.Reports
{
    public partial class Form_ReturnSaleFactorPrint : Form
    {
        Classes.Class_Settle cl_Settle = new Classes.Class_Settle();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        int _SaleNumber;
        bool _BackSpace = false;
        DataTable HeaderTable;
        DataTable DetailTable;
        DataTable Org;
        string[] Sign;
        short _StyleNumber = 1;

        public Form_ReturnSaleFactorPrint(int SaleNumber)
        {
            InitializeComponent();
            _SaleNumber = SaleNumber;
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            string HeaderSelectText = @" SELECT FactorTable.FactorID AS ID,
       FactorTable.Serial,
       FactorTable.LegalNumber,
       FactorTable.Date,
       FactorTable.Responsible,
       FactorTable.CustomerID,
       FactorTable.P2Name,
       FactorTable.P2NationalCode,
       FactorTable.P2ECode,
       FactorTable.P2SabtCode,
       FactorTable.P2Address,
       FactorTable.P2Tel,
       FactorTable.P2Fax,
       FactorTable.P2PostalCode,
       FactorTable.P2Code,
       FactorTable.GoodCode,
       FactorTable.GoodName,
       FactorTable.Box,
       FactorTable.BoxPrice,
       FactorTable.Pack,
       FactorTable.PackPrice,
       FactorTable.Number,
       FactorTable.TotalNumber,
       FactorTable.SinglePrice,
       FactorTable.TotalPrice,
       FactorTable.DiscountPercent,
       FactorTable.DiscountPrice,
       FactorTable.TaxPrice,
       FactorTable.NetPrice,
       ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
       ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
       PersonInfoTable.Column02,
       FactorTable.NetTotal,
       0 AS VolumeGroup,
       0 AS SpecialGroup,
       0 AS SpecialCustomer,
       FactorTable.Description,
       FactorTable.CountUnitName,
       derivedtbl_1.Groups,
       'Zero' AS charPrice,
       'SettleInfo' AS SettleInfo,
       CASE 
            WHEN FactorTable.FactorType = 1 THEN '***ارزي***'
            ELSE '***ريالي***'
       END AS FactorType
FROM   (
           SELECT dbo.Table_018_MarjooiSale.columnid AS FactorID,
                  dbo.Table_018_MarjooiSale.column01 AS Serial,
                  0 AS LegalNumber,
                  dbo.Table_018_MarjooiSale.column02 AS Date,
                  dbo.Table_018_MarjooiSale.column03 AS CustomerID,
                  PersonTable.Column02 AS P2Name,
                     PersonTable.Column09 AS P2NationalCode,
                       PersonTable.Column141 AS P2ECode,
                       PersonTable.Column142 AS P2SabtCode,
                  PersonTable.Column01 AS P2Code,
                  PersonTable.Column06 AS P2Address,
                  PersonTable.Column07 AS P2Tel,
                  PersonTable.Column08 AS P2Fax,
                  PersonTable.Column13 AS P2PostalCode,
                  GoodTable.column01 AS GoodCode,
                  dbo.Table_019_Child1_MarjooiSale.column04 AS Box,
                  dbo.Table_019_Child1_MarjooiSale.column08 AS BoxPrice,
                  dbo.Table_019_Child1_MarjooiSale.column05 AS Pack,
                  dbo.Table_019_Child1_MarjooiSale.column09 AS PackPrice,
                  dbo.Table_019_Child1_MarjooiSale.column06 AS Number,
                  dbo.Table_019_Child1_MarjooiSale.column07 AS TotalNumber,
                  dbo.Table_019_Child1_MarjooiSale.column10 AS SinglePrice,
                  dbo.Table_019_Child1_MarjooiSale.column11 AS TotalPrice,
                  dbo.Table_019_Child1_MarjooiSale.column16 AS DiscountPercent,
                  dbo.Table_019_Child1_MarjooiSale.column17 AS DiscountPrice,
                  dbo.Table_019_Child1_MarjooiSale.column19 AS TaxPrice,
                  dbo.Table_019_Child1_MarjooiSale.column20 AS NetPrice,
                  GoodTable.column02 AS GoodName,
                  OtherPrice.PlusPrice AS Ezafat,
                  OtherPrice.MinusPrice AS Kosoorat,
                  dbo.Table_018_MarjooiSale.column05 AS Responsible,
                  dbo.Table_018_MarjooiSale.Column18 AS NetTotal,
                  dbo.Table_018_MarjooiSale.column06 AS DESCRIPTION,
                  CountUnitTable.Column01 AS CountUnitName,
                  dbo.Table_018_MarjooiSale.Column12 AS FactorType
           FROM   dbo.Table_018_MarjooiSale
                  INNER JOIN dbo.Table_019_Child1_MarjooiSale
                       ON  dbo.Table_018_MarjooiSale.columnid = dbo.Table_019_Child1_MarjooiSale.column01
                  INNER JOIN (
                           SELECT ColumnId,
                                  Column00,
                                  Column01,
                                  Column02,
                                  Column03,
                                  Column04,
                                  Column05,
                                  Column06,
                                  Column07,
                                  Column08,
                                  Column09,
                                  Column10,
                                  Column11,
                                  Column12,
                                  Column13,
                                   Column141,
                                       Column142
                           FROM   {0}.dbo.Table_045_PersonInfo
                       ) AS PersonTable
                       ON  dbo.Table_018_MarjooiSale.column03 = PersonTable.ColumnId
                  LEFT OUTER JOIN (
                           SELECT columnid,
                                  SUM(PlusPrice) AS PlusPrice,
                                  SUM(MinusPrice) AS MinusPrice
                           FROM   (
                                      SELECT Table_018_MarjooiSale_2.columnid,
                                             SUM(dbo.Table_020_Child2_MarjooiSale.column04) AS 
                                             PlusPrice,
                                             0 AS MinusPrice
                                      FROM   dbo.Table_020_Child2_MarjooiSale
                                             INNER JOIN dbo.Table_018_MarjooiSale AS 
                                                  Table_018_MarjooiSale_2
                                                  ON  dbo.Table_020_Child2_MarjooiSale.column01 = 
                                                      Table_018_MarjooiSale_2.columnid
                                      WHERE  (dbo.Table_020_Child2_MarjooiSale.column05 = 0)
                                      GROUP BY
                                             Table_018_MarjooiSale_2.columnid,
                                             dbo.Table_020_Child2_MarjooiSale.column05
                                      UNION ALL
                                      SELECT Table_018_MarjooiSale_1.columnid,
                                             0 AS PlusPrice,
                                             SUM(Table_020_Child2_MarjooiSale_1.column04) AS 
                                             MinusPrice
                                      FROM   dbo.Table_020_Child2_MarjooiSale AS 
                                             Table_020_Child2_MarjooiSale_1
                                             INNER JOIN dbo.Table_018_MarjooiSale AS 
                                                  Table_018_MarjooiSale_1
                                                  ON  
                                                      Table_020_Child2_MarjooiSale_1.column01 = 
                                                      Table_018_MarjooiSale_1.columnid
                                      WHERE  (Table_020_Child2_MarjooiSale_1.column05 = 1)
                                      GROUP BY
                                             Table_018_MarjooiSale_1.columnid,
                                             Table_020_Child2_MarjooiSale_1.column05
                                  ) AS OtherPrice_1
                           GROUP BY
                                  columnid
                       ) AS OtherPrice
                       ON  dbo.Table_018_MarjooiSale.columnid = OtherPrice.columnid
                  LEFT OUTER JOIN (
                           SELECT columnid,
                                  column01,
                                  column02,
                                  column03,
                                  column04,
                                  column05,
                                  column06,
                                  column07,
                                  column08,
                                  column09,
                                  column10,
                                  column11,
                                  column12,
                                  column13,
                                  column14,
                                  column15,
                                  column16,
                                  column17,
                                  column18,
                                  column19,
                                  column20,
                                  column21,
                                  column22,
                                  column23,
                                  column24,
                                  column25,
                                  column26,
                                  column27,
                                  column28,
                                  column29,
                                  column30,
                                  column31
                           FROM   {1}.dbo.table_004_CommodityAndIngredients
                       ) AS GoodTable
                       ON  dbo.Table_019_Child1_MarjooiSale.column02 = GoodTable.columnid
                  LEFT OUTER JOIN (
                           SELECT Column00,
                                  Column01
                           FROM   {0}.dbo.Table_070_CountUnitInfo
                       ) AS CountUnitTable
                       ON  dbo.Table_019_Child1_MarjooiSale.column03 = 
                           CountUnitTable.Column00
       ) AS FactorTable
       LEFT OUTER JOIN (
                SELECT PersonId,
                       Groups
                FROM   {0}.dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1
            ) AS derivedtbl_1
            ON  FactorTable.CustomerID = derivedtbl_1.PersonId
       LEFT OUTER JOIN (
                SELECT ColumnId,
                       Column01,
                       Column02
                FROM   {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1
            ) AS PersonInfoTable
            ON  FactorTable.Responsible = PersonInfoTable.ColumnId";


            HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);

            string DetailSelectText = @"SELECT     dbo.Table_024_Discount.column01 AS Name, dbo.Table_020_Child2_MarjooiSale.column04 AS Price, 
                      CASE WHEN Table_024_Discount.column02 = 0 THEN '+' ELSE '-' END AS Type, dbo.Table_020_Child2_MarjooiSale.column01 AS Column01 ,
                      dbo.Table_018_MarjooiSale.column01 AS HeaderNum, dbo.Table_018_MarjooiSale.column02 AS HeaderDate
                      FROM         dbo.Table_020_Child2_MarjooiSale INNER JOIN
                      dbo.Table_018_MarjooiSale ON dbo.Table_020_Child2_MarjooiSale.column01 = dbo.Table_018_MarjooiSale.columnid LEFT OUTER JOIN
                      dbo.Table_024_Discount ON dbo.Table_020_Child2_MarjooiSale.column02 = dbo.Table_024_Discount.columnid";


            if (rdb_CurrentNumber.Checked)
            {
                HeaderSelectText += " WHERE     (FactorTable.Serial = " + _SaleNumber + ")";
                DetailSelectText += " WHERE (Table_018_MarjooiSale.Column01= " + _SaleNumber + ")";
            }
            else if (rdb_FromNumber.Checked && txt_From.Text.Trim() != "" && txt_To.Text.Trim() != "")
            {
                HeaderSelectText += " WHERE     (FactorTable.Serial between  " + txt_From.Text + " and " + txt_To.Text + ")";
                DetailSelectText += @" WHERE (Table_018_MarjooiSale.Column01 between " + txt_From.Text + " and " + txt_To.Text + ")";
            }
            else if (rdb_Date.Checked && faDatePicker1.SelectedDateTime.HasValue && faDatePicker2.SelectedDateTime.HasValue
                && faDatePicker1.Text.Length == 10 && faDatePicker2.Text.Length == 10)
            {
                HeaderSelectText += " WHERE     (FactorTable.Date between  '" + faDatePicker1.Text + "' and '" + faDatePicker2.Text + "')";
                DetailSelectText += @" WHERE (Table_018_MarjooiSale.Column02 between '" + faDatePicker1.Text + "' and '" + faDatePicker2.Text + "')";
            }

             HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
            foreach (DataRow item in HeaderTable.Rows)
            {
                double FinalPrice = Convert.ToDouble(item["NetTotal"].ToString()) +
                            Convert.ToDouble(item["Ezafat"].ToString()) - Convert.ToDouble(item["Kosoorat"].ToString()) -
                            Convert.ToDouble(item["SpecialGroup"].ToString()) - Convert.ToDouble(item["SpecialCustomer"].ToString())
                            - Convert.ToDouble(item["VolumeGroup"].ToString());
                item["CharPrice"] = FarsiLibrary.Utils.ToWords.ToString(long.Parse(Math.Round(FinalPrice, 0).ToString()));
            }
             DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);
           

            //فراخوانی عناوین امضا
            Sign = clDoc.Signature(12);
            Org = Class_BasicOperation.LogoTable();

            switch (_StyleNumber)
            {
                case 1:
                    bt_First_Click(sender, e);
                    break;

                case 2:
                    bt_Second_Click(sender, e);
                    break;

                case 3:
                    bt_Third_Click(sender, e);
                    break;

                case 4:
                    bt_Fourth_Click(sender, e);
                    break;

                case 5:
                    bt_Fifth_Click(sender, e);
                    break;

            }
           
        }

        private void Form_FactorPrint_Load(object sender, EventArgs e)
        {
            faDatePicker1.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePicker2.SelectedDateTime = DateTime.Now;
            chk_Logo.Checked = Properties.Settings.Default.DontShowLogo;
            _StyleNumber = Properties.Settings.Default.ReturnSaleFactor;
            bt_Display_Click(sender, e);
        }

        private void txt_From_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
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
        { //طرح رسمی
            this.Cursor = Cursors.WaitCursor;
            _05_Sale.Reports.Rpt_03_ReturnSaleFactor Rpt1 = new Rpt_03_ReturnSaleFactor();
            if (!chk_Logo.Checked)
                Rpt1.Subreports[0].SetDataSource(Org);
            else Rpt1.Subreports[0].SetDataSource(Org.Clone());

            Rpt1.SetDataSource(HeaderTable);
            Rpt1.Subreports["x1"].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                Rpt1.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt1.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt1.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt1.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt1.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt1.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt1.SetParameterValue("P1Name", " ");
                Rpt1.SetParameterValue("P1ECode", " ");
                Rpt1.SetParameterValue("P1NCode", " ");
                Rpt1.SetParameterValue("P1Address", " ");
                Rpt1.SetParameterValue("P1Tel", " ");
                Rpt1.SetParameterValue("P1PostalCode", " ");

            }
            Rpt1.SetParameterValue("Param3", Sign[0]);
            Rpt1.SetParameterValue("Param4", Sign[1]);
            Rpt1.SetParameterValue("Param5", Sign[2]);
            Rpt1.SetParameterValue("Param6", Sign[3]);
            Rpt1.SetParameterValue("Param7", Sign[4]);
            Rpt1.SetParameterValue("Param8", Sign[5]);
            Rpt1.SetParameterValue("Param9", Sign[6]);
            Rpt1.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = Rpt1;
            this.Cursor = Cursors.Default;
            _StyleNumber = 1;

        }

        private void bt_Second_Click(object sender, EventArgs e)
        {
            //طرح عمودی
            this.Cursor = Cursors.WaitCursor;
            _05_Sale.Reports.Rpt_03_ReturnSaleFactor2 Rpt2 = new Rpt_03_ReturnSaleFactor2();
           
            if (!chk_Logo.Checked)
                Rpt2.Subreports[0].SetDataSource(Org);
            else Rpt2.Subreports[0].SetDataSource(Org.Clone());

            Rpt2.SetDataSource(HeaderTable);
            Rpt2.Subreports["X1"].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt2.SetParameterValue("P1Name", " ");
                Rpt2.SetParameterValue("P1ECode", " ");
                Rpt2.SetParameterValue("P1NCode", " ");
                Rpt2.SetParameterValue("P1Address", " ");
                Rpt2.SetParameterValue("P1Tel", " ");
                Rpt2.SetParameterValue("P1PostalCode", " ");

            }
            Rpt2.SetParameterValue("Param3", Sign[0]);
            Rpt2.SetParameterValue("Param4", Sign[1]);
            Rpt2.SetParameterValue("Param5", Sign[2]);
            Rpt2.SetParameterValue("Param6", Sign[3]);
            Rpt2.SetParameterValue("Param7", Sign[4]);
            Rpt2.SetParameterValue("Param8", Sign[5]);
            Rpt2.SetParameterValue("Param9", Sign[6]);
            Rpt2.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = Rpt2;
            this.Cursor = Cursors.Default;
            _StyleNumber =2;
        }

        private void bt_Third_Click(object sender, EventArgs e)
        {
            //طرح افقی اول
            this.Cursor = Cursors.WaitCursor;
            _05_Sale.Reports.Rpt_03_ReturnSaleFactor3 Rpt3 = new Rpt_03_ReturnSaleFactor3();
            if (!chk_Logo.Checked)
                Rpt3.Subreports[0].SetDataSource(Org);
            else Rpt3.Subreports[0].SetDataSource(Org.Clone());

            Rpt3.SetDataSource(HeaderTable);
            Rpt3.Subreports["X1"].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                Rpt3.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt3.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt3.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt3.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt3.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt3.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt3.SetParameterValue("P1Name", " ");
                Rpt3.SetParameterValue("P1ECode", " ");
                Rpt3.SetParameterValue("P1NCode", " ");
                Rpt3.SetParameterValue("P1Address", " ");
                Rpt3.SetParameterValue("P1Tel", " ");
                Rpt3.SetParameterValue("P1PostalCode", " ");

            }
            Rpt3.SetParameterValue("Param3", Sign[0]);
            Rpt3.SetParameterValue("Param4", Sign[1]);
            Rpt3.SetParameterValue("Param5", Sign[2]);
            Rpt3.SetParameterValue("Param6", Sign[3]);
            Rpt3.SetParameterValue("Param7", Sign[4]);
            Rpt3.SetParameterValue("Param8", Sign[5]);
            Rpt3.SetParameterValue("Param9", Sign[6]);
            Rpt3.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = Rpt3;
            this.Cursor = Cursors.Default;
            _StyleNumber = 3;

        }

        private void bt_Fourth_Click(object sender, EventArgs e)
        {
            //طرح افقی دوم
            this.Cursor = Cursors.WaitCursor;
            _05_Sale.Reports.Rpt_03_ReturnSaleFactor4 Rpt4 = new Rpt_03_ReturnSaleFactor4();
          
            if (!chk_Logo.Checked)
                Rpt4.Subreports[0].SetDataSource(Org);
            else Rpt4.Subreports[0].SetDataSource(Org.Clone());

            Rpt4.SetDataSource(HeaderTable);
            Rpt4.Subreports["X1"].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                Rpt4.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt4.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt4.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt4.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt4.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt4.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt4.SetParameterValue("P1Name", " ");
                Rpt4.SetParameterValue("P1ECode", " ");
                Rpt4.SetParameterValue("P1NCode", " ");
                Rpt4.SetParameterValue("P1Address", " ");
                Rpt4.SetParameterValue("P1Tel", " ");
                Rpt4.SetParameterValue("P1PostalCode", " ");

            }
            Rpt4.SetParameterValue("Param3", Sign[0]);
            Rpt4.SetParameterValue("Param4", Sign[1]);
            Rpt4.SetParameterValue("Param5", Sign[2]);
            Rpt4.SetParameterValue("Param6", Sign[3]);
            Rpt4.SetParameterValue("Param7", Sign[4]);
            Rpt4.SetParameterValue("Param8", Sign[5]);
            Rpt4.SetParameterValue("Param9", Sign[6]);
            Rpt4.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = Rpt4;
            this.Cursor = Cursors.Default;
            _StyleNumber = 4;
        }

        private void bt_Fifth_Click(object sender, EventArgs e)
        {
            //طرح افقی چهارم
            this.Cursor = Cursors.WaitCursor;
            _05_Sale.Reports.Rpt_03_ReturnSaleFactor5 Rpt5 = new Rpt_03_ReturnSaleFactor5();
           
            if (!chk_Logo.Checked)
                Rpt5.Subreports[0].SetDataSource(Org);
            else Rpt5.Subreports[0].SetDataSource(Org.Clone());

            Rpt5.SetDataSource(HeaderTable);
            Rpt5.Subreports["X1"].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                Rpt5.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt5.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt5.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt5.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt5.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt5.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt5.SetParameterValue("P1Name", " ");
                Rpt5.SetParameterValue("P1ECode", " ");
                Rpt5.SetParameterValue("P1NCode", " ");
                Rpt5.SetParameterValue("P1Address", " ");
                Rpt5.SetParameterValue("P1Tel", " ");
                Rpt5.SetParameterValue("P1PostalCode", " ");

            }
            Rpt5.SetParameterValue("Param3", Sign[0]);
            Rpt5.SetParameterValue("Param4", Sign[1]);
            Rpt5.SetParameterValue("Param5", Sign[2]);
            Rpt5.SetParameterValue("Param6", Sign[3]);
            Rpt5.SetParameterValue("Param7", Sign[4]);
            Rpt5.SetParameterValue("Param8", Sign[5]);
            Rpt5.SetParameterValue("Param9", Sign[6]);
            Rpt5.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = Rpt5;
            this.Cursor = Cursors.Default;
            _StyleNumber = 5;
        }

        private void Form_ReturnSaleFactorPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.DontShowLogo = chk_Logo.Checked;
            Properties.Settings.Default.ReturnSaleFactor = _StyleNumber;
            Properties.Settings.Default.Save();
            try
            {
                var Rpt = (CrystalDecisions.CrystalReports.Engine.ReportDocument)crystalReportViewer1.ReportSource;
                Rpt.Database.Dispose();
                Rpt.Close();
                Rpt.Dispose();
                GC.Collect();
            }
            catch
            {
            }
        }
    }
}