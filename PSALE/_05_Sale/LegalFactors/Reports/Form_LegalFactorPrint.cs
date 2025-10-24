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
using System.Drawing.Printing;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Design;
using Stimulsoft.Database;
namespace PSHOP._05_Sale.LegalFactors.Reports
{
    public partial class Form_LegalFactorPrint : DevComponents.DotNetBar.OfficeForm
    {
        Classes.Class_Settle cl_Settle = new Classes.Class_Settle();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        int _LegalNumber;
        List<string> List = new List<string>();
        string[] Sign;
        DataTable Org, HeaderTable, DetailTable;
        short _PrintStyle = 1;

        public Form_LegalFactorPrint(int LegalNumber)
        {
            InitializeComponent();
            _LegalNumber = LegalNumber;
        }
        public Form_LegalFactorPrint(List<string> _List)
        {
            InitializeComponent();
            List = _List;
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            string HeaderSelectText = @"SELECT     FactorTable.FactorID AS ID, FactorTable.Serial, FactorTable.LegalNumber, FactorTable.Date, FactorTable.Responsible, FactorTable.CustomerID, FactorTable.P2Name, 
             FactorTable.P2NationalCode,
       FactorTable.P2ECode,
       FactorTable.P2SabtCode, FactorTable.P2Address, FactorTable.P2Tel, FactorTable.P2Fax, FactorTable.P2PostalCode, FactorTable.P2Code, FactorTable.GoodCode, 
            FactorTable.GoodName, FactorTable.Box, FactorTable.BoxPrice, FactorTable.Pack, FactorTable.PackPrice, FactorTable.Number, FactorTable.TotalNumber, 
            FactorTable.SinglePrice, FactorTable.TotalPrice, FactorTable.DiscountPercent, FactorTable.DiscountPrice, FactorTable.TaxPrice, FactorTable.NetPrice, 
            ISNULL(FactorTable.Ezafat, 0) AS Ezafat, ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat, PersonInfoTable.Column02, FactorTable.NetTotal, FactorTable.VolumeGroup, 
            FactorTable.SpecialGroup, FactorTable.SpecialCustomer, FactorTable.Description, FactorTable.CountUnitName, derivedtbl_1.Groups, 'Zero' AS charPrice, 
            FactorTable.ProvinceId, FactorTable.CityId, FactorTable.PayCash, CityTable.Column02 AS City, ProvinceTbl.Column01 AS Province
            FROM         (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
            FROM         {0}.dbo.Table_060_ProvinceInfo) AS ProvinceTbl INNER JOIN
            (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
            FROM         {0}.dbo.Table_065_CityInfo) AS CityTable ON ProvinceTbl.Column00 = CityTable.Column00 RIGHT OUTER JOIN
            (SELECT     dbo.Table_055_LegalFactors.columnid AS FactorID, dbo.Table_055_LegalFactors.column01 AS Serial, 
            dbo.Table_055_LegalFactors.column01 AS LegalNumber, dbo.Table_055_LegalFactors.column02 AS Date, 
            dbo.Table_055_LegalFactors.column03 AS CustomerID, PersonTable.Column02 AS P2Name,  PersonTable.Column09 AS P2NationalCode,
                       PersonTable.Column141 AS P2ECode,
                       PersonTable.Column142 AS P2SabtCode,
            PersonTable.Column01 AS P2Code, PersonTable.Column06 AS P2Address, PersonTable.Column07 AS P2Tel, PersonTable.Column08 AS P2Fax, 
            PersonTable.Column13 AS P2PostalCode, GoodTable.column01 AS GoodCode, dbo.Table_060_Child1_LegalFactors.column04 AS Box, 
            dbo.Table_060_Child1_LegalFactors.column08 AS BoxPrice, dbo.Table_060_Child1_LegalFactors.column05 AS Pack, 
            dbo.Table_060_Child1_LegalFactors.column09 AS PackPrice, dbo.Table_060_Child1_LegalFactors.column06 AS Number, 
            dbo.Table_060_Child1_LegalFactors.column07 AS TotalNumber, dbo.Table_060_Child1_LegalFactors.column10 AS SinglePrice, 
            dbo.Table_060_Child1_LegalFactors.column11 AS TotalPrice, dbo.Table_060_Child1_LegalFactors.column16 AS DiscountPercent, 
            dbo.Table_060_Child1_LegalFactors.column17 AS DiscountPrice, dbo.Table_060_Child1_LegalFactors.column19 AS TaxPrice, 
            dbo.Table_060_Child1_LegalFactors.column20 AS NetPrice, GoodTable.column02 AS GoodName, OtherPrice.PlusPrice AS Ezafat, 
            OtherPrice.MinusPrice AS Kosoorat, dbo.Table_055_LegalFactors.column05 AS Responsible, dbo.Table_055_LegalFactors.Column28 AS NetTotal, 
            dbo.Table_055_LegalFactors.Column29 AS VolumeGroup, dbo.Table_055_LegalFactors.Column30 AS SpecialGroup, 
            dbo.Table_055_LegalFactors.Column31 AS SpecialCustomer, dbo.Table_055_LegalFactors.column06 AS Description, 
            CountUnitTable.Column01 AS CountUnitName, PersonTable.ProvinceId, PersonTable.CityId, dbo.Table_055_LegalFactors.column21 AS PayCash
            FROM         dbo.Table_055_LegalFactors INNER JOIN
            dbo.Table_060_Child1_LegalFactors ON dbo.Table_055_LegalFactors.columnid = dbo.Table_060_Child1_LegalFactors.column01  LEFT OUTER JOIN
            (SELECT     ColumnId, Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, 
            Column11, Column12, Column13, Column21 AS ProvinceId, Column22 AS CityId,  Column141,
                                       Column142
            FROM         {0}.dbo.Table_045_PersonInfo) AS PersonTable ON 
            dbo.Table_055_LegalFactors.column03 = PersonTable.ColumnId LEFT OUTER JOIN
            (SELECT     columnid, SUM(PlusPrice) AS PlusPrice, SUM(MinusPrice) AS MinusPrice
            FROM         (SELECT     Table_055_LegalFactors_2.columnid, SUM(dbo.Table_065_Child2_LegalFactors.column04) AS PlusPrice, 0 AS MinusPrice
            FROM         dbo.Table_065_Child2_LegalFactors INNER JOIN
            dbo.Table_055_LegalFactors AS Table_055_LegalFactors_2 ON 
            dbo.Table_065_Child2_LegalFactors.column01 = Table_055_LegalFactors_2.columnid
            WHERE     (dbo.Table_065_Child2_LegalFactors.column05 = 0)
            GROUP BY Table_055_LegalFactors_2.columnid, dbo.Table_065_Child2_LegalFactors.column05
            UNION ALL
            SELECT     Table_055_LegalFactors_1.columnid, 0 AS PlusPrice, SUM(Table_065_Child2_LegalFactors_1.column04) AS MinusPrice
            FROM         dbo.Table_065_Child2_LegalFactors AS Table_065_Child2_LegalFactors_1 INNER JOIN
            dbo.Table_055_LegalFactors AS Table_055_LegalFactors_1 ON 
            Table_065_Child2_LegalFactors_1.column01 = Table_055_LegalFactors_1.columnid
            WHERE     (Table_065_Child2_LegalFactors_1.column05 = 1)
            GROUP BY Table_055_LegalFactors_1.columnid, Table_065_Child2_LegalFactors_1.column05) AS OtherPrice_1
            GROUP BY columnid) AS OtherPrice ON dbo.Table_055_LegalFactors.columnid = OtherPrice.columnid LEFT OUTER JOIN
            (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, 
            column12, column13, column14, column15, column16, column17, column18, column19, column20, column21, column22, column23, 
            column24, column25, column26, column27, column28, column29, column30, column31
            FROM         {1}.dbo.table_004_CommodityAndIngredients) AS GoodTable ON 
            dbo.Table_060_Child1_LegalFactors.column02 = GoodTable.columnid LEFT OUTER JOIN
            (SELECT     Column00, Column01
            FROM         {0}.dbo.Table_070_CountUnitInfo) AS CountUnitTable ON dbo.Table_060_Child1_LegalFactors.column03 = CountUnitTable.Column00)
            AS FactorTable ON CityTable.Column01 = FactorTable.CityId LEFT OUTER JOIN
            (SELECT     PersonId, Groups
            FROM         {0}.dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1) AS derivedtbl_1 ON FactorTable.CustomerID = derivedtbl_1.PersonId LEFT OUTER JOIN
            (SELECT     ColumnId, Column01, Column02, Column21, Column22
            FROM         {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1) AS PersonInfoTable ON FactorTable.Responsible = PersonInfoTable.ColumnId";
            HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);

            string DetailSelectText = @"SELECT    Table_065_Child2_LegalFactors.column01, Table_024_Discount.column01 as Name, 
                        Table_065_Child2_LegalFactors.column04 as Price,CASE when Table_024_Discount.column02=0 then 
                        '+' else '-' end as Type FROM Table_065_Child2_LegalFactors LEFT OUTER JOIN  
                            Table_024_Discount ON Table_065_Child2_LegalFactors.column02 = Table_024_Discount.columnid where Table_065_Child2_LegalFactors.column04>0";

            if (List.Count > 0)
            {
                HeaderSelectText += " WHERE     FactorTable.FactorID IN(" + string.Join(",", List.ToArray()) + ")";
                DetailSelectText += " AND     dbo.Table_065_Child2_LegalFactors.column01 IN (" + string.Join(",", List.ToArray()) + ")";

            }
            else if (rdb_CurrentNumber.Checked)
            {
                HeaderSelectText += " WHERE     (FactorTable.LegalNumber = " + _LegalNumber + ")";
                DetailSelectText += " AND Table_065_Child2_LegalFactors.Column01 IN (Select ColumnId from Table_055_LegalFactors where Column01=" +
                    _LegalNumber + ")";
            }
            else if (txt_From.Text.Trim() != "" && txt_To.Text.Trim() != "")
            {
                HeaderSelectText += " WHERE     (FactorTable.LegalNumber between  " + txt_From.Text + " and " + txt_To.Text + ")";
                DetailSelectText += @" AND Table_065_Child2_LegalFactors.Column01 IN (Select ColumnId from Table_055_LegalFactors where 
                            Column01 Between " + txt_From.Text + " and " + txt_To.Text + ")";
            }
            HeaderTable = new DataTable();
            HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
            foreach (DataRow item in HeaderTable.Rows)
            {
                Double FinalPrice = Convert.ToDouble(item["NetTotal"].ToString()) +
                    Convert.ToDouble(item["Ezafat"].ToString()) - Convert.ToDouble(item["Kosoorat"].ToString()) -
                    Convert.ToDouble(item["SpecialGroup"].ToString()) - Convert.ToDouble(item["SpecialCustomer"].ToString())
                    - Convert.ToDouble(item["VolumeGroup"].ToString());
                item["CharPrice"] = FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64(FinalPrice));
            }

            DetailTable = new DataTable();
            DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);

            //فراخوانی عناوین امضا
            Sign = clDoc.Signature(10);


            //Show Reports
            Org = Class_BasicOperation.LogoTable();

            switch (_PrintStyle)
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
                case 5: bt_Fifth_Click(sender, e);
                    break;
                case 6: btn_Custom_Click(sender, e);
                    break;
            }



        }

        public void Form_FactorPrint_Load(object sender, EventArgs e)
        {
            _PrintStyle = Properties.Settings.Default.LegalFactorPrintLayout;
            bt_Display_Click(sender, e);
        }

        private void Form_FactorPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.LegalFactorPrintLayout = _PrintStyle;
            Properties.Settings.Default.Save();

        }

        private void txt_From_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void bt_First_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            _05_Sale.LegalFactors.Reports.Rpt_SaleFactor_1 Rpt1 = new Rpt_SaleFactor_1();
            Rpt1.Subreports[0].SetDataSource(Org);
            Rpt1.SetDataSource(HeaderTable);
            Rpt1.Subreports[1].SetDataSource(DetailTable);
            Rpt1.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
            Rpt1.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
            Rpt1.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
            Rpt1.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
            Rpt1.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
            Rpt1.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            Rpt1.SetParameterValue("Param3", Sign[0]);
            Rpt1.SetParameterValue("Param4", Sign[1]);
            Rpt1.SetParameterValue("Param5", Sign[2]);
            Rpt1.SetParameterValue("Param6", Sign[3]);
            Rpt1.SetParameterValue("Param7", Sign[4]);
            Rpt1.SetParameterValue("Param8", Sign[5]);
            Rpt1.SetParameterValue("Param9", Sign[6]);
            Rpt1.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = Rpt1;
            _PrintStyle = 1;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt1.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt1.PrintToPrinter(1, true, 1, 1000);
            }


        }

        private void bt_Second_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            _05_Sale.LegalFactors.Reports.Rpt_SaleFactor_2 Rpt2 = new Rpt_SaleFactor_2();
            Rpt2.Subreports[0].SetDataSource(Org);
            Rpt2.SetDataSource(HeaderTable);
            Rpt2.Subreports[1].SetDataSource(DetailTable);
            Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
            Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
            Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
            Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
            Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
            Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            Rpt2.SetParameterValue("Param3", Sign[0]);
            Rpt2.SetParameterValue("Param4", Sign[1]);
            Rpt2.SetParameterValue("Param5", Sign[2]);
            Rpt2.SetParameterValue("Param6", Sign[3]);
            Rpt2.SetParameterValue("Param7", Sign[4]);
            Rpt2.SetParameterValue("Param8", Sign[5]);
            Rpt2.SetParameterValue("Param9", Sign[6]);
            Rpt2.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = Rpt2;
            _PrintStyle = 2;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt2.PrintToPrinter(1, true, 1, 1000);
            }
        }

        private void bt_Third_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            _05_Sale.LegalFactors.Reports.Rpt_SaleFactor_3 Rpt3 = new Rpt_SaleFactor_3();
            Rpt3.SetDataSource(HeaderTable);
            Rpt3.Subreports[0].SetDataSource(DetailTable);
            Rpt3.SetParameterValue("Param3", Sign[0]);
            Rpt3.SetParameterValue("Param4", Sign[1]);
            Rpt3.SetParameterValue("Param5", Sign[2]);
            Rpt3.SetParameterValue("Param6", Sign[3]);
            Rpt3.SetParameterValue("Param7", Sign[4]);
            Rpt3.SetParameterValue("Param8", Sign[5]);
            Rpt3.SetParameterValue("Param9", Sign[6]);
            Rpt3.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = Rpt3;
            _PrintStyle = 3;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt3.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt3.PrintToPrinter(1, true, 1, 1000);
            }
        }

        private void bt_Fourth_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            _05_Sale.LegalFactors.Reports.Rpt_SaleFactor_4 Rpt4 = new Rpt_SaleFactor_4();
            Rpt4.SetDataSource(HeaderTable);
            Rpt4.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
            Rpt4.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
            Rpt4.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
            Rpt4.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
            Rpt4.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
            Rpt4.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            crystalReportViewer1.ReportSource = Rpt4;
            _PrintStyle = 4;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt4.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt4.PrintToPrinter(1, true, 1, 1000);
            }
        }

        private void bt_Fifth_Click(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            crystalReportViewer1.Visible = true;
            _05_Sale.LegalFactors.Reports.Rpt_02_SaleFactor8 Rpt2 = new Rpt_02_SaleFactor8();
            Rpt2.Subreports[0].SetDataSource(Org);
            Rpt2.SetDataSource(HeaderTable);
            Rpt2.Subreports[1].SetDataSource(DetailTable);
            Rpt2.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
            Rpt2.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
            Rpt2.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
            Rpt2.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
            Rpt2.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
            Rpt2.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            Rpt2.SetParameterValue("Param3", Sign[0]);
            Rpt2.SetParameterValue("Param4", Sign[1]);
            Rpt2.SetParameterValue("Param5", Sign[2]);
            Rpt2.SetParameterValue("Param6", Sign[3]);
            Rpt2.SetParameterValue("Param7", Sign[4]);
            Rpt2.SetParameterValue("Param8", Sign[5]);
            Rpt2.SetParameterValue("Param9", Sign[6]);
            Rpt2.SetParameterValue("Param10", Sign[7]);
            crystalReportViewer1.ReportSource = Rpt2;
            _PrintStyle = 5;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt2.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt2.PrintToPrinter(1, true, 1, 1000);
            }
        }

        private void btn_Design_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("Rpt_02_SaleFactor8.mrt");
            stireport.Design();
        }

        private void btn_Custom_Click(object sender, EventArgs e)
        {

            try
            {
                stiViewerControl1.Visible = true;
                crystalReportViewer1.Visible = false;
                this.Cursor = Cursors.WaitCursor;

                _PrintStyle = 6;


                StiReport stireport = new StiReport();
                stireport.Load("Rpt_02_SaleFactor8.mrt");
                stireport.Pages["Page1"].Enabled = true;
                stireport.Compile();
                StiOptions.Viewer.AllowUseDragDrop = false;
                stireport.RegData("Table_000_OrgInfo", Org);

                stireport.RegData("Rpt_SaleTable", HeaderTable);
                stireport.RegData("Rpt_SaleExtra_Table", DetailTable);


                 


                stireport["P1Name"] = Org.Rows[0]["Column01"].ToString();
                stireport["P1ECode"] = Org.Rows[0]["Column06"].ToString();
                stireport["P1NCode"] = Org.Rows[0]["Column07"].ToString();
                stireport["P1Address"] = Org.Rows[0]["Column02"].ToString();
                stireport["P1PostalCode"] = Org.Rows[0]["Column14"].ToString();
                stireport["Param3"] = Sign[0];
                stireport["Param4"] = Sign[1];
                stireport["Param5"] = Sign[2];
                stireport["Param6"] = Sign[3];
                stireport["Param7"] = Sign[4];
                stireport["Param8"] = Sign[5];
                stireport["Param9"] = Sign[6];
                stireport["Param10"] = Sign[7];
                this.Cursor = Cursors.Default;
                stireport.IsSelected = true;
                stireport.Select();
                //stireport.Show();
                stireport.Render(false);
                stiViewerControl1.Report = stireport;
                stiViewerControl1.Refresh();

            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
                this.Cursor = Cursors.Default;

            }
        }


    }
}