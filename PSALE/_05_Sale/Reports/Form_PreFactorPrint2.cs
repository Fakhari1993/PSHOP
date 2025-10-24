using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Design;
using Stimulsoft.Database;
namespace PSHOP._05_Sale.Reports
{
    public partial class Form_PreFactorPrint2 : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        int _FactorNumber;
        string[] Sign;
        DataTable HeaderTable;
        DataTable DetailTable;
        DataTable Org;
        DataTable Table;
        short _StyleNumber = 1;

        public Form_PreFactorPrint2(int FactorNumber)
        {
            InitializeComponent();
            _FactorNumber = FactorNumber;
        }
        public Form_PreFactorPrint2()
        {
            InitializeComponent();
        }

        private void Form_PreFactorPrint_Load(object sender, EventArgs e)
        {
            txt_Number.Text = _FactorNumber.ToString();
            _StyleNumber = Properties.Settings.Default.PrefactorStyle;
            chk_Logo.Checked = Properties.Settings.Default.DontShowLogo;
            bt_Display_Click(sender, e);
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            string HeaderSelectText = @" 
                            SELECT FactorTable.FactorID AS ID,
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
                                   derivedtbl_2.NetTotal,
                                   PersonInfoTable.Column02,
                                   FactorTable.Description,
                                   FactorTable.DetailDescription,
                                   FactorTable.CountUnitName,
                                   derivedtbl_1.Groups,
                                   '-' AS charPrice,
                                    GoodDesc,CostCenter,Project
                            FROM   (
                                       SELECT dbo.Table_007_FactorBefore2.columnid AS FactorID,
                                              dbo.Table_007_FactorBefore2.column01 AS Serial,
                                              0 AS LegalNumber,
                                              dbo.Table_007_FactorBefore2.column02 AS Date,
                                              dbo.Table_007_FactorBefore2.column03 AS CustomerID,
                                              PersonTable.Column02 AS P2Name,
                                                                    PersonTable.Column09 AS P2NationalCode,
                                                   PersonTable.Column141 AS P2ECode,
                                                   PersonTable.Column142 AS P2SabtCode,
                                              PersonTable.Column01 AS P2Code,
                                              PersonTable.Column06 AS P2Address,
                                              PersonTable.Column07 AS P2Tel,
                                              PersonTable.Column08 AS P2Fax,
                                              PersonTable.Column13 AS P2PostalCode,
                                              dbo.Table_008_Child1_FactorBefore2.column01 AS GoodCode,
                                              0 AS Box,
                                              0 AS BoxPrice,
                                              0 AS Pack,
                                              0 AS PackPrice,
                                              dbo.Table_008_Child1_FactorBefore2.column03 AS Number,
                                              dbo.Table_008_Child1_FactorBefore2.column03 AS TotalNumber,
                                              dbo.Table_008_Child1_FactorBefore2.column04 AS SinglePrice,
                                              dbo.Table_008_Child1_FactorBefore2.column05 AS TotalPrice,
                                              0 AS DiscountPercent,
                                              0 AS DiscountPrice,
                                              0 AS TaxPrice,
                                              Table_008_Child1_FactorBefore2.Column07 AS DetailDescription,
                                              dbo.Table_008_Child1_FactorBefore2.column05 AS NetPrice,
                                              dbo.Table_008_Child1_FactorBefore2.column02 AS GoodName,
                                              OtherPrice.PlusPrice AS Ezafat,
                                              OtherPrice.MinusPrice AS Kosoorat,
                                              dbo.Table_007_FactorBefore2.column05 AS Responsible,
                                              dbo.Table_007_FactorBefore2.column06 AS DESCRIPTION,
                                              dbo.Table_008_Child1_FactorBefore2.column06 AS CountUnitName,
                                              dbo.Table_008_Child1_FactorBefore2.column07 as GoodDesc,
                                              pp1.Column02 as CostCenter,
                                               pp.Column02 as Project
                                       FROM   dbo.Table_007_FactorBefore2
                                              INNER JOIN dbo.Table_008_Child1_FactorBefore2
                                                   ON  dbo.Table_007_FactorBefore2.columnid = dbo.Table_008_Child1_FactorBefore2.column00
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
                                                   ON  dbo.Table_007_FactorBefore2.column03 = PersonTable.ColumnId
                                              LEFT OUTER JOIN (
                                                       SELECT columnid,
                                                              SUM(PlusPrice) AS PlusPrice,
                                                              SUM(MinusPrice) AS MinusPrice
                                                       FROM   (
                                                                  SELECT Table_007_FactorBefore_2.columnid,
                                                                         SUM(dbo.Table_009_Child2_FactorBefore2.column03) AS 
                                                                         PlusPrice,
                                                                         0 AS MinusPrice
                                                                  FROM   dbo.Table_009_Child2_FactorBefore2
                                                                         INNER JOIN dbo.Table_007_FactorBefore2 AS 
                                                                              Table_007_FactorBefore_2
                                                                              ON  dbo.Table_009_Child2_FactorBefore2.column00 = 
                                                                                  Table_007_FactorBefore_2.columnid
                                                                  WHERE  (dbo.Table_009_Child2_FactorBefore2.column04 = 0)
                                                                  GROUP BY
                                                                         Table_007_FactorBefore_2.columnid,
                                                                         dbo.Table_009_Child2_FactorBefore2.column04
                                                                  UNION ALL
                                                                  SELECT Table_007_FactorBefore_1.columnid,
                                                                         0 AS PlusPrice,
                                                                         SUM(Table_009_Child2_FactorBefore_1.column03) AS 
                                                                         MinusPrice
                                                                  FROM   dbo.Table_009_Child2_FactorBefore2 AS 
                                                                         Table_009_Child2_FactorBefore_1
                                                                         INNER JOIN dbo.Table_007_FactorBefore2 AS 
                                                                              Table_007_FactorBefore_1
                                                                              ON  
                                                                                  Table_009_Child2_FactorBefore_1.column00 = 
                                                                                  Table_007_FactorBefore_1.columnid
                                                                  WHERE  (Table_009_Child2_FactorBefore_1.column04 = 1)
                                                                  GROUP BY
                                                                         Table_007_FactorBefore_1.columnid,
                                                                         Table_009_Child2_FactorBefore_1.column04
                                                              ) AS OtherPrice_1
                                                       GROUP BY
                                                              columnid
                                                   ) AS OtherPrice
                                                   ON  dbo.Table_007_FactorBefore2.columnid = OtherPrice.columnid
                                                left join " + ConBase.Database + @".dbo.Table_035_ProjectInfo pp on pp.Column00= dbo.Table_008_Child1_FactorBefore2.column23 
                                                left join " + ConBase.Database + @".dbo.Table_030_ExpenseCenterInfo pp1 on pp1.Column00= dbo.Table_008_Child1_FactorBefore2.column22
                                              
                                   ) AS FactorTable
                                   INNER JOIN (
                                            SELECT column00,
                                                   SUM(column05) AS NetTotal
                                            FROM   dbo.Table_008_Child1_FactorBefore2 AS 
                                                   Table_008_Child1_FactorBefore_1
                                            GROUP BY
                                                   column00
                                        ) AS derivedtbl_2
                                        ON  FactorTable.FactorID = derivedtbl_2.column00
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
                                        ON  FactorTable.Responsible = PersonInfoTable.ColumnId


";
                    
            HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);

            string DetailSelectText = @"SELECT     dbo.Table_009_Child2_FactorBefore2.column01 AS Name, dbo.Table_009_Child2_FactorBefore2.column03 AS Price, 
                      CASE WHEN dbo.Table_009_Child2_FactorBefore2.column04 = 0 THEN '+' ELSE '-' END AS Type, dbo.Table_009_Child2_FactorBefore2.column00 AS Column01 ,
                      dbo.Table_007_FactorBefore2.column01 AS HeaderNum, dbo.Table_007_FactorBefore2.column02 AS HeaderDate
                      FROM         dbo.Table_009_Child2_FactorBefore2 INNER JOIN
                      dbo.Table_007_FactorBefore2 ON dbo.Table_009_Child2_FactorBefore2.column00 = dbo.Table_007_FactorBefore2.columnid  ";


            //if (rdb_CurrentNumber.Checked)
            //{
            HeaderSelectText += " WHERE     (FactorTable.Serial = " + txt_Number.Text + ")";
            DetailSelectText += " WHERE (Table_007_FactorBefore2.Column01= " + txt_Number.Text + ")";
            //}


            HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);

            foreach (DataRow item in HeaderTable.Rows)
            {
                double FinalPrice = double.Parse(item["NetTotal"].ToString()) +
                            double.Parse(item["Ezafat"].ToString()) - double.Parse(item["Kosoorat"].ToString());
                item["CharPrice"] = FarsiLibrary.Utils.ToWords.ToString(long.Parse(Math.Round(FinalPrice, 0).ToString()));
            }

            DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);


            //فراخوانی عناوین امضا
            Sign = clDoc.Signature(13);
            Org = Class_BasicOperation.LogoTable();
          
            SqlDataAdapter Adapter = new SqlDataAdapter("Select * from Table_007_FactorBefore2 where Column01=" + _FactorNumber, ConSale);
            Table = new DataTable();
            Adapter.Fill(Table);

            switch (_StyleNumber)
            {
                case 1:
                    bt_First_Click(sender, e);
                    break;
                case 2: bt_Second_Click(sender, e);
                    break;
                case 3: bt_Third_Click(sender, e);
                    break;
                case 4: bt_Fourth_Click(sender, e);
                    break;
                case 5: bt_5_Click(sender, e);
                    break;
                case 6: btn_Custome_Click(sender, e);
                    break;
                case 7: btn_6_Click(sender, e);
                    break;
                case 8: btn_7_Click(sender, e);
                    break;
                    
            }
           
        }

        private void txt_Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
                bt_Display_Click(sender, e);
        }

        private void Form_PreFactorPrint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                txt_Number.Focus();
                txt_Number.SelectAll();
            }
        }

        private void bt_First_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            //طرح یک
            _05_Sale.Reports.Rpt_01_PreFactor_1 Rpt1 = new Rpt_01_PreFactor_1();
            if (!chk_Logo.Checked)
                Rpt1.Subreports[0].SetDataSource(Org);
            else Rpt1.Subreports[0].SetDataSource(Org.Clone());

            Rpt1.SetDataSource(HeaderTable);
            Rpt1.Subreports["X1"].SetDataSource(DetailTable);
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
          
            Rpt1.SetParameterValue("Cash", (Table.Rows[0]["Column15"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column15"].ToString()) : false));
            Rpt1.SetParameterValue("CashPrice", (Table.Rows[0]["Column16"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column16"].ToString()) : 0));
            Rpt1.SetParameterValue("Chq", (Table.Rows[0]["Column19"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column19"].ToString()) : false));
            Rpt1.SetParameterValue("ChqPrice", (Table.Rows[0]["Column20"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column20"].ToString()) : 0));
            Rpt1.SetParameterValue("ChqDay", (Table.Rows[0]["Column21"].ToString().Trim() != "" ? Table.Rows[0]["Column21"].ToString() : "0"));
            Rpt1.SetParameterValue("Validation", (Table.Rows[0]["Column17"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column17"].ToString()) : false));
            Rpt1.SetParameterValue("ValidationDay", (Table.Rows[0]["Column18"].ToString().Trim() != "" ? Table.Rows[0]["Column18"].ToString() : " "));
            Rpt1.SetParameterValue("ExpDay", (Table.Rows[0]["Column14"].ToString().Trim() != "" ? Table.Rows[0]["Column14"].ToString() : " "));
            Rpt1.SetParameterValue("ExpDate", (Table.Rows[0]["Column22"].ToString().Trim() != "" ? Table.Rows[0]["Column22"].ToString() : " "));
            crystalReportViewer1.ReportSource = Rpt1;
            _StyleNumber = 1;
        }

        private void bt_Second_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            //طرح دو
            _05_Sale.Reports.Rpt_01_PreFactor_2 Rpt2 = new Rpt_01_PreFactor_2();
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
            Rpt2.SetParameterValue("Cash", (Table.Rows[0]["Column15"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column15"].ToString()) : false));
            Rpt2.SetParameterValue("CashPrice", (Table.Rows[0]["Column16"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column16"].ToString()) : 0));
            Rpt2.SetParameterValue("Chq", (Table.Rows[0]["Column19"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column19"].ToString()) : false));
            Rpt2.SetParameterValue("ChqPrice", (Table.Rows[0]["Column20"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column20"].ToString()) : 0));
            Rpt2.SetParameterValue("ChqDay", (Table.Rows[0]["Column21"].ToString().Trim() != "" ? Table.Rows[0]["Column21"].ToString() : "0"));
            Rpt2.SetParameterValue("Validation", (Table.Rows[0]["Column17"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column17"].ToString()) : false));
            Rpt2.SetParameterValue("ValidationDay", (Table.Rows[0]["Column18"].ToString().Trim() != "" ? Table.Rows[0]["Column18"].ToString() : " "));
            Rpt2.SetParameterValue("ExpDay", (Table.Rows[0]["Column14"].ToString().Trim() != "" ? Table.Rows[0]["Column14"].ToString() : " "));
            Rpt2.SetParameterValue("ExpDate", (Table.Rows[0]["Column22"].ToString().Trim() != "" ? Table.Rows[0]["Column22"].ToString() : " "));
            crystalReportViewer1.ReportSource = Rpt2;
            _StyleNumber = 2;
        }

        private void bt_Third_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            //طرح سه
            _05_Sale.Reports.Rpt_01_PreFactor_3 Rpt3 = new Rpt_01_PreFactor_3();
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
            Rpt3.SetParameterValue("Cash", (Table.Rows[0]["Column15"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column15"].ToString()) : false));
            Rpt3.SetParameterValue("CashPrice", (Table.Rows[0]["Column16"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column16"].ToString()) : 0));
            Rpt3.SetParameterValue("Chq", (Table.Rows[0]["Column19"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column19"].ToString()) : false));
            Rpt3.SetParameterValue("ChqPrice", (Table.Rows[0]["Column20"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column20"].ToString()) : 0));
            Rpt3.SetParameterValue("ChqDay", (Table.Rows[0]["Column21"].ToString().Trim() != "" ? Table.Rows[0]["Column21"].ToString() : "0"));
            Rpt3.SetParameterValue("Validation", (Table.Rows[0]["Column17"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column17"].ToString()) : false));
            Rpt3.SetParameterValue("ValidationDay", (Table.Rows[0]["Column18"].ToString().Trim() != "" ? Table.Rows[0]["Column18"].ToString() : " "));
            Rpt3.SetParameterValue("ExpDay", (Table.Rows[0]["Column14"].ToString().Trim() != "" ? Table.Rows[0]["Column14"].ToString() : " "));
            Rpt3.SetParameterValue("ExpDate", (Table.Rows[0]["Column22"].ToString().Trim() != "" ? Table.Rows[0]["Column22"].ToString() : " "));
            crystalReportViewer1.ReportSource = Rpt3;
            _StyleNumber = 3;
        }

        private void Form_PreFactorPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            chk_Logo.Checked = Properties.Settings.Default.DontShowLogo;
            Properties.Settings.Default.PrefactorStyle = _StyleNumber;
            Properties.Settings.Default.DontShowLogo = chk_Logo.Checked;
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

        private void bt_Fourth_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            _05_Sale.Reports.Rpt_01_PreFactor_4 Rpt3 = new Rpt_01_PreFactor_4();
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
            Rpt3.SetParameterValue("Cash", (Table.Rows[0]["Column15"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column15"].ToString()) : false));
            Rpt3.SetParameterValue("CashPrice", (Table.Rows[0]["Column16"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column16"].ToString()) : 0));
            Rpt3.SetParameterValue("Chq", (Table.Rows[0]["Column19"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column19"].ToString()) : false));
            Rpt3.SetParameterValue("ChqPrice", (Table.Rows[0]["Column20"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column20"].ToString()) : 0));
            Rpt3.SetParameterValue("ChqDay", (Table.Rows[0]["Column21"].ToString().Trim() != "" ? Table.Rows[0]["Column21"].ToString() : "0"));
            Rpt3.SetParameterValue("Validation", (Table.Rows[0]["Column17"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column17"].ToString()) : false));
            Rpt3.SetParameterValue("ValidationDay", (Table.Rows[0]["Column18"].ToString().Trim() != "" ? Table.Rows[0]["Column18"].ToString() : " "));
            Rpt3.SetParameterValue("ExpDay", (Table.Rows[0]["Column14"].ToString().Trim() != "" ? Table.Rows[0]["Column14"].ToString() : " "));
            Rpt3.SetParameterValue("ExpDate", (Table.Rows[0]["Column22"].ToString().Trim() != "" ? Table.Rows[0]["Column22"].ToString() : " "));
            crystalReportViewer1.ReportSource = Rpt3;
            _StyleNumber = 4;
        }

        private void bt_5_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            _05_Sale.Reports.Rpt_01_PreFactor_5 Rpt3 = new Rpt_01_PreFactor_5();
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
            Rpt3.SetParameterValue("Cash", (Table.Rows[0]["Column15"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column15"].ToString()) : false));
            Rpt3.SetParameterValue("CashPrice", (Table.Rows[0]["Column16"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column16"].ToString()) : 0));
            Rpt3.SetParameterValue("Chq", (Table.Rows[0]["Column19"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column19"].ToString()) : false));
            Rpt3.SetParameterValue("ChqPrice", (Table.Rows[0]["Column20"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column20"].ToString()) : 0));
            Rpt3.SetParameterValue("ChqDay", (Table.Rows[0]["Column21"].ToString().Trim() != "" ? Table.Rows[0]["Column21"].ToString() : "0"));
            Rpt3.SetParameterValue("Validation", (Table.Rows[0]["Column17"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column17"].ToString()) : false));
            Rpt3.SetParameterValue("ValidationDay", (Table.Rows[0]["Column18"].ToString().Trim() != "" ? Table.Rows[0]["Column18"].ToString() : " "));
            Rpt3.SetParameterValue("ExpDay", (Table.Rows[0]["Column14"].ToString().Trim() != "" ? Table.Rows[0]["Column14"].ToString() : " "));
            Rpt3.SetParameterValue("ExpDate", (Table.Rows[0]["Column22"].ToString().Trim() != "" ? Table.Rows[0]["Column22"].ToString() : " "));
            crystalReportViewer1.ReportSource = Rpt3;
            _StyleNumber = 5;
        }

        private void btn_Custome_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = true;
                crystalReportViewer1.Visible = false;
                this.Cursor = Cursors.WaitCursor;

                _StyleNumber =6;


                StiReport stireport = new StiReport();
                stireport.Load("PreFactorReport.mrt");
                stireport.Pages["Page1"].Enabled = true;
                stireport.Compile();
                StiOptions.Viewer.AllowUseDragDrop = false;
                if (DetailTable.Rows.Count > 0)
                    stireport.Pages["X1"].Enabled = true;
                else
                    stireport.Pages["X1"].Enabled = false;
                if (!chk_Logo.Checked)
                    stireport.RegData("Table_000_OrgInfo", Org);
                else

                    stireport.RegData("Table_000_OrgInfo", Org.Clone());

                stireport.RegData("Rpt_SaleTable", HeaderTable);
                stireport.RegData("Rpt_SaleExtra_Table", DetailTable);

                if (!chk_Logo.Checked)
                {
                  

                    stireport["P1Name"] = Org.Rows[0]["Column01"].ToString();
                    stireport["P1ECode"] = Org.Rows[0]["Column06"].ToString();
                    stireport["P1NCode"] = Org.Rows[0]["Column07"].ToString();
                    stireport["P1Address"] = Org.Rows[0]["Column02"].ToString();
                    stireport["P1Tel"] = Org.Rows[0]["Column03"].ToString();
                    stireport["P1PostalCode"] = Org.Rows[0]["Column14"].ToString();
                  



                }
                else
                {

                    stireport["P1Name"] = "";
                    stireport["P1ECode"] = "";
                    stireport["P1NCode"] = "";
                    stireport["P1Address"] = "";
                    stireport["P1Tel"] = "";
                    stireport["P1PostalCode"] = "";
                  

                }
                stireport["Param3"] = Sign[0];
                stireport["Param4"] = Sign[1];
                stireport["Param5"] = Sign[2];
                stireport["Param6"] = Sign[3];
                stireport["Param7"] = Sign[4];
                stireport["Param8"] = Sign[5];
                stireport["Param9"] = Sign[6];
                stireport["Param10"] = Sign[7];
                stireport["Cash"] = (Table.Rows[0]["Column15"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column15"].ToString()) : false);
                stireport["CashPrice"] = (Table.Rows[0]["Column16"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column16"].ToString()) : 0);
                stireport["Chq"] = (Table.Rows[0]["Column19"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column19"].ToString()) : false);
                stireport["ChqPrice"] = (Table.Rows[0]["Column20"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column20"].ToString()) : 0);
                stireport["ChqDay"] = (Table.Rows[0]["Column21"].ToString().Trim() != "" ? Table.Rows[0]["Column21"].ToString() : "0");
                stireport["Validation"] = (Table.Rows[0]["Column17"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column17"].ToString()) : false);
                stireport["ValidationDay"] = (Table.Rows[0]["Column18"].ToString().Trim() != "" ? Table.Rows[0]["Column18"].ToString() : " ");
                stireport["ExpDay"] = (Table.Rows[0]["Column14"].ToString().Trim() != "" ? Table.Rows[0]["Column14"].ToString() : " ");
                stireport["ExpDate"] = (Table.Rows[0]["Column22"].ToString().Trim() != "" ? Table.Rows[0]["Column22"].ToString() : " ");

                stireport.Render(false);
                stiViewerControl1.Report = stireport;
                stiViewerControl1.Refresh();


            }
            catch
            {
            }
        }

        private void btn_Design_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("PreFactorReport.mrt");
            stireport.Design();
        }

        private void btn_6_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            _05_Sale.Reports.Rpt_01_PreFactor_6 Rpt3 = new Rpt_01_PreFactor_6();
            if (!chk_Logo.Checked)
                Rpt3.Subreports[0].SetDataSource(Org);
            else Rpt3.Subreports[0].SetDataSource(Org.Clone());

            Rpt3.SetDataSource(HeaderTable);
            Rpt3.Subreports["x1"].SetDataSource(DetailTable);
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
            //Rpt3.SetParameterValue("Cash", (Table.Rows[0]["Column15"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column15"].ToString()) : false));
            //Rpt3.SetParameterValue("CashPrice", (Table.Rows[0]["Column16"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column16"].ToString()) : 0));
            //Rpt3.SetParameterValue("Chq", (Table.Rows[0]["Column19"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column19"].ToString()) : false));
            //Rpt3.SetParameterValue("ChqPrice", (Table.Rows[0]["Column20"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column20"].ToString()) : 0));
            //Rpt3.SetParameterValue("ChqDay", (Table.Rows[0]["Column21"].ToString().Trim() != "" ? Table.Rows[0]["Column21"].ToString() : "0"));
            //Rpt3.SetParameterValue("Validation", (Table.Rows[0]["Column17"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column17"].ToString()) : false));
            //Rpt3.SetParameterValue("ValidationDay", (Table.Rows[0]["Column18"].ToString().Trim() != "" ? Table.Rows[0]["Column18"].ToString() : " "));
            //Rpt3.SetParameterValue("ExpDay", (Table.Rows[0]["Column14"].ToString().Trim() != "" ? Table.Rows[0]["Column14"].ToString() : " "));
            //Rpt3.SetParameterValue("ExpDate", (Table.Rows[0]["Column22"].ToString().Trim() != "" ? Table.Rows[0]["Column22"].ToString() : " "));
            crystalReportViewer1.ReportSource = Rpt3;
            _StyleNumber =7;
        }

        private void btn_7_Click(object sender, EventArgs e)
        {
            try
            {
                stiViewerControl1.Visible = false;
                crystalReportViewer1.Visible = true;
                _05_Sale.Reports.Rpt_01_PreFactor_7 Rpt3 = new Rpt_01_PreFactor_7();
                if (!chk_Logo.Checked)
                    Rpt3.Subreports[0].SetDataSource(Org);
                else Rpt3.Subreports[0].SetDataSource(Org.Clone());

                Rpt3.SetDataSource(HeaderTable);
                Rpt3.Subreports["X1"].SetDataSource(DetailTable);
                Rpt3.Subreports["X2"].SetDataSource(DetailTable);

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
                //Rpt3.SetParameterValue("Cash", (Table.Rows[0]["Column15"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column15"].ToString()) : false));
                //Rpt3.SetParameterValue("CashPrice", (Table.Rows[0]["Column16"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column16"].ToString()) : 0));
                //Rpt3.SetParameterValue("Chq", (Table.Rows[0]["Column19"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column19"].ToString()) : false));
                //Rpt3.SetParameterValue("ChqPrice", (Table.Rows[0]["Column20"].ToString().Trim() != "" ? Int64.Parse(Table.Rows[0]["Column20"].ToString()) : 0));
                //Rpt3.SetParameterValue("ChqDay", (Table.Rows[0]["Column21"].ToString().Trim() != "" ? Table.Rows[0]["Column21"].ToString() : "0"));
                //Rpt3.SetParameterValue("Validation", (Table.Rows[0]["Column17"].ToString().Trim() != "" ? bool.Parse(Table.Rows[0]["Column17"].ToString()) : false));
                //Rpt3.SetParameterValue("ValidationDay", (Table.Rows[0]["Column18"].ToString().Trim() != "" ? Table.Rows[0]["Column18"].ToString() : " "));
                //Rpt3.SetParameterValue("ExpDay", (Table.Rows[0]["Column14"].ToString().Trim() != "" ? Table.Rows[0]["Column14"].ToString() : " "));
                //Rpt3.SetParameterValue("ExpDate", (Table.Rows[0]["Column22"].ToString().Trim() != "" ? Table.Rows[0]["Column22"].ToString() : " "));
                crystalReportViewer1.ReportSource = Rpt3;
                _StyleNumber = 8;

            }
            catch
            {
            }
        }

       
    
    }
}
