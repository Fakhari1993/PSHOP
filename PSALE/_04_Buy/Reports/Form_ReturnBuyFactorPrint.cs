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

namespace PSHOP._04_Buy.Reports
{
    public partial class Form_ReturnBuyFactorPrint : DevComponents.DotNetBar.OfficeForm
    {
       Classes.Class_Settle cl_Settle = new Classes.Class_Settle();
       SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
       SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
       SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
       string[] Sign;
       DataTable HeaderTable;
       DataTable DetailTable;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        bool _BackSpace = false;
        int _ReturnBuyNumber;
        DataTable Org;
        short _StyleNumber = 1;

        public Form_ReturnBuyFactorPrint(int ReturnBuyNumber)
        {
            InitializeComponent();
            _ReturnBuyNumber = ReturnBuyNumber;
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            string HeaderSelectText = @"SELECT FactorTable.Serial,
       FactorTable.Date,
       FactorTable.Responsible,
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
       FactorTable.NetTotal,
       ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
       ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
       PersonInfoTable.Column02,
       FactorTable.Description,
       FactorTable.CountUnitName,
       'Zero' AS CharPrice,
       FactorType
FROM   (
           SELECT dbo.Table_021_MarjooiBuy.columnid AS FactorID,
                  dbo.Table_021_MarjooiBuy.column01 AS Serial,
                  dbo.Table_021_MarjooiBuy.column02 AS Date,
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
                  dbo.Table_022_Child1_MarjooiBuy.column04 AS Box,
                  dbo.Table_022_Child1_MarjooiBuy.column08 AS BoxPrice,
                  dbo.Table_022_Child1_MarjooiBuy.column05 AS Pack,
                  dbo.Table_022_Child1_MarjooiBuy.column09 AS PackPrice,
                  dbo.Table_022_Child1_MarjooiBuy.column06 AS Number,
                  dbo.Table_022_Child1_MarjooiBuy.column07 AS TotalNumber,
                  dbo.Table_022_Child1_MarjooiBuy.column10 AS SinglePrice,
                  dbo.Table_022_Child1_MarjooiBuy.column11 AS TotalPrice,
                  dbo.Table_022_Child1_MarjooiBuy.column16 AS DiscountPercent,
                  dbo.Table_022_Child1_MarjooiBuy.column17 AS DiscountPrice,
                  dbo.Table_022_Child1_MarjooiBuy.column19 AS TaxPrice,
                  dbo.Table_022_Child1_MarjooiBuy.column20 AS NetPrice,
                  dbo.Table_021_MarjooiBuy.Column18 AS NetTotal,
                  GoodTable.column02 AS GoodName,
                  OtherPrice.PlusPrice AS Ezafat,
                  OtherPrice.MinusPrice AS Kosoorat,
                  dbo.Table_021_MarjooiBuy.column14 AS Responsible,
                  dbo.Table_021_MarjooiBuy.column04 AS DESCRIPTION,
                  CountUnitTable.Column01 AS CountUnitName,
                  CASE 
                       WHEN Table_021_MarjooiBuy.Column15 = 0 THEN 
                            '*** ريالي ***'
                       ELSE '*** ارزي ***'
                  END AS FactorType
           FROM   dbo.Table_021_MarjooiBuy
                  INNER JOIN dbo.Table_022_Child1_MarjooiBuy
                       ON  dbo.Table_021_MarjooiBuy.columnid = dbo.Table_022_Child1_MarjooiBuy.column01
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
                       ON  dbo.Table_021_MarjooiBuy.column03 = PersonTable.ColumnId
                  LEFT OUTER JOIN (
                           SELECT columnid,
                                  SUM(PlusPrice) AS PlusPrice,
                                  SUM(MinusPrice) AS MinusPrice
                           FROM   (
                                      SELECT Table_021_MarjooiBuy_2.columnid,
                                             SUM(dbo.Table_023_Child2_MarjooiBuy.column04) AS 
                                             PlusPrice,
                                             0 AS MinusPrice
                                      FROM   dbo.Table_023_Child2_MarjooiBuy
                                             INNER JOIN dbo.Table_021_MarjooiBuy AS 
                                                  Table_021_MarjooiBuy_2
                                                  ON  dbo.Table_023_Child2_MarjooiBuy.column01 = 
                                                      Table_021_MarjooiBuy_2.columnid
                                      WHERE  (dbo.Table_023_Child2_MarjooiBuy.column05 = 0)
                                      GROUP BY
                                             Table_021_MarjooiBuy_2.columnid,
                                             dbo.Table_023_Child2_MarjooiBuy.column05
                                      UNION ALL
                                      SELECT Table_021_MarjooiBuy_1.columnid,
                                             0 AS PlusPrice,
                                             SUM(Table_023_Child2_MarjooiBuy_1.column04) AS 
                                             MinusPrice
                                      FROM   dbo.Table_023_Child2_MarjooiBuy AS 
                                             Table_023_Child2_MarjooiBuy_1
                                             INNER JOIN dbo.Table_021_MarjooiBuy AS 
                                                  Table_021_MarjooiBuy_1
                                                  ON  
                                                      Table_023_Child2_MarjooiBuy_1.column01 = 
                                                      Table_021_MarjooiBuy_1.columnid
                                      WHERE  (Table_023_Child2_MarjooiBuy_1.column05 = 1)
                                      GROUP BY
                                             Table_021_MarjooiBuy_1.columnid,
                                             Table_023_Child2_MarjooiBuy_1.column05
                                  ) AS OtherPrice_1
                           GROUP BY
                                  columnid
                       ) AS OtherPrice
                       ON  dbo.Table_021_MarjooiBuy.columnid = OtherPrice.columnid
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
                       ON  dbo.Table_022_Child1_MarjooiBuy.column02 = GoodTable.columnid
                  LEFT OUTER JOIN (
                           SELECT Column00,
                                  Column01
                           FROM   {0}.dbo.Table_070_CountUnitInfo
                       ) AS CountUnitTable
                       ON  dbo.Table_022_Child1_MarjooiBuy.column03 = 
                           CountUnitTable.Column00
       ) AS FactorTable
       LEFT OUTER JOIN (
                SELECT ColumnId,
                       Column01,
                       Column02
                FROM   {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1
            ) AS PersonInfoTable
            ON  FactorTable.Responsible = PersonInfoTable.ColumnId";
            HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);

            string DetailSelectText = @"SELECT     dbo.Table_024_Discount_Buy.column01 AS Name, dbo.Table_023_Child2_MarjooiBuy.column04 AS Price, 
                      CASE WHEN Table_024_Discount_Buy.column02 = 0 THEN '+' ELSE '-' END AS Type, dbo.Table_023_Child2_MarjooiBuy.column01 AS HeaderId, 
                      dbo.Table_021_MarjooiBuy.column01 AS HeaderNum, dbo.Table_021_MarjooiBuy.column02 AS HeaderDate
                      FROM         dbo.Table_023_Child2_MarjooiBuy INNER JOIN
                      dbo.Table_021_MarjooiBuy ON dbo.Table_023_Child2_MarjooiBuy.column01 = dbo.Table_021_MarjooiBuy.columnid LEFT OUTER JOIN
                      dbo.Table_024_Discount_Buy ON dbo.Table_023_Child2_MarjooiBuy.column02 = dbo.Table_024_Discount_Buy.columnid";


            if (rdb_CurrentNumber.Checked)
            {
                HeaderSelectText += " WHERE     (FactorTable.Serial = "+_ReturnBuyNumber+")";
                DetailSelectText += " WHERE (Table_021_MarjooiBuy.Column01= " + _ReturnBuyNumber + ")";
            }
            else if (rdb_FromNumber.Checked && txt_From.Text.Trim() != "" && txt_To.Text.Trim() != "")
            {
                HeaderSelectText += " WHERE     (FactorTable.Serial between  " + txt_From.Text + " and " + txt_To.Text + ")";
                DetailSelectText += @" WHERE (Table_021_MarjooiBuy.Column01 between " + txt_From.Text + " and " + txt_To.Text + ")";
            }
            else if (rdb_Date.Checked && faDatePicker1.SelectedDateTime.HasValue && faDatePicker2.SelectedDateTime.HasValue
                && faDatePicker1.Text.Length == 10 && faDatePicker2.Text.Length == 10)
            {
                HeaderSelectText += " WHERE     (FactorTable.Date between  '" + faDatePicker1.Text + "' and '" + faDatePicker2.Text + "')";
                DetailSelectText += @" WHERE (Table_021_MarjooiBuy.Column02 between '" + faDatePicker1.Text + "' and '" + faDatePicker2.Text + "')";
            }

            HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
            foreach (DataRow item in HeaderTable.Rows)
            {
                double FinalPrice = Convert.ToDouble(item["NetTotal"].ToString()) +
                    Convert.ToDouble(item["Ezafat"].ToString()) - Convert.ToDouble(item["Kosoorat"].ToString());
                  
                item["CharPrice"] = FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(Math.Round(FinalPrice,0)));
            }

            DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);


            //فراخوانی عناوین امضا
            Sign = clDoc.Signature(11);

            Org = Class_BasicOperation.LogoTable();

            switch (_StyleNumber)
            {
                case 1:
                    bt_First_Click(sender, e);
                    break;
                case 2: bt_Second_Click(sender, e);
                    break;
                case 3: bt_Last_Click(sender, e);
                    break;
            }
        }

        private void Form_FactorPrint_Load(object sender, EventArgs e)
        {
            faDatePicker1.SelectedDateTime = DateTime.Now.AddMonths(-1);
            faDatePicker2.SelectedDateTime = DateTime.Now;
            _StyleNumber = Properties.Settings.Default.ReturnBuyFactorStyle;
            chk_Logo.Checked = Properties.Settings.Default.DontShowLogo;
            bt_Display_Click(sender, e);
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

        private void bt_First_Click(object sender, EventArgs e)
        {
            //طرح اول
            this.Cursor = Cursors.WaitCursor;
            _04_Buy.Reports.Rpt_02_ReturnBuyFactor rpt = new Reports.Rpt_02_ReturnBuyFactor();

            if (!chk_Logo.Checked)
                rpt.Subreports[0].SetDataSource(Org);
            else rpt.Subreports[0].SetDataSource(Org.Clone());

            rpt.SetDataSource(HeaderTable);
            rpt.Subreports[1].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                rpt.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                rpt.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                rpt.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                rpt.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                rpt.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                rpt.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                rpt.SetParameterValue("P1Name", " ");
                rpt.SetParameterValue("P1ECode", " ");
                rpt.SetParameterValue("P1NCode", " ");
                rpt.SetParameterValue("P1Address", " ");
                rpt.SetParameterValue("P1Tel", " ");
                rpt.SetParameterValue("P1PostalCode", " ");
            }

            rpt.SetParameterValue("Param3", Sign[0]);
            rpt.SetParameterValue("Param4", Sign[1]);
            rpt.SetParameterValue("Param5", Sign[2]);
            rpt.SetParameterValue("Param6", Sign[3]);
            rpt.SetParameterValue("Param7", Sign[4]);
            rpt.SetParameterValue("Param8", Sign[5]);
            rpt.SetParameterValue("Param9", Sign[6]);
            rpt.SetParameterValue("Param10", Sign[7]);

            crystalReportViewer1.ReportSource = rpt;
            _StyleNumber = 1;
            this.Cursor = Cursors.Default;
        }

        private void bt_Second_Click(object sender, EventArgs e)
        {
            //طرح دوم
            this.Cursor = Cursors.WaitCursor;
            _04_Buy.Reports.Rpt_02_ReturnBuyFactor2 rpt2 = new Rpt_02_ReturnBuyFactor2();
            if (!chk_Logo.Checked)
                rpt2.Subreports[0].SetDataSource(Org);
            else rpt2.Subreports[0].SetDataSource(Org.Clone());

            rpt2.SetDataSource(HeaderTable);
            rpt2.Subreports[1].SetDataSource(DetailTable);
            if (!chk_Logo.Checked)
            {
                rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                rpt2.SetParameterValue("P1Name", " ");
                rpt2.SetParameterValue("P1ECode", " ");
                rpt2.SetParameterValue("P1NCode", " ");
                rpt2.SetParameterValue("P1Address", " ");
                rpt2.SetParameterValue("P1Tel", " ");
                rpt2.SetParameterValue("P1PostalCode", " ");
            }

            rpt2.SetParameterValue("Param3", Sign[0]);
            rpt2.SetParameterValue("Param4", Sign[1]);
            rpt2.SetParameterValue("Param5", Sign[2]);
            rpt2.SetParameterValue("Param6", Sign[3]);
            rpt2.SetParameterValue("Param7", Sign[4]);
            rpt2.SetParameterValue("Param8", Sign[5]);
            rpt2.SetParameterValue("Param9", Sign[6]);
            rpt2.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = rpt2;
            _StyleNumber = 2;
            this.Cursor = Cursors.Default;
        }

        private void bt_Last_Click(object sender, EventArgs e)
        {
            //طرح عمودی
            this.Cursor = Cursors.WaitCursor;
            _04_Buy.Reports.Rpt_02_ReturnBuyFactor3 rpt3 = new Rpt_02_ReturnBuyFactor3();
            
            rpt3.SetDataSource(HeaderTable);
            rpt3.Subreports[1].SetDataSource(DetailTable);

            if (!chk_Logo.Checked)
                rpt3.Subreports[0].SetDataSource(Org);
            else rpt3.Subreports[0].SetDataSource(Org.Clone());

            if (!chk_Logo.Checked)
            {
                rpt3.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                rpt3.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                rpt3.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                rpt3.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                rpt3.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                rpt3.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                rpt3.SetParameterValue("P1Name", " ");
                rpt3.SetParameterValue("P1ECode", " ");
                rpt3.SetParameterValue("P1NCode", " ");
                rpt3.SetParameterValue("P1Address", " ");
                rpt3.SetParameterValue("P1Tel", " ");
                rpt3.SetParameterValue("P1PostalCode", " ");
            }

            rpt3.SetParameterValue("Param3", Sign[0]);
            rpt3.SetParameterValue("Param4", Sign[1]);
            rpt3.SetParameterValue("Param5", Sign[2]);
            rpt3.SetParameterValue("Param6", Sign[3]);
            rpt3.SetParameterValue("Param7", Sign[4]);
            rpt3.SetParameterValue("Param8", Sign[5]);
            rpt3.SetParameterValue("Param9", Sign[6]);
            rpt3.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = rpt3;
            _StyleNumber = 3;
            this.Cursor = Cursors.Default;
        }

        private void Form_ReturnBuyFactorPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.DontShowLogo = chk_Logo.Checked;
            Properties.Settings.Default.ReturnBuyFactorStyle = _StyleNumber;
            Properties.Settings.Default.Save();
        }
    }
}