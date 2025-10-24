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
using Stimulsoft.Report;

namespace PSHOP._04_Buy.Reports
{
    public partial class Frm_Rpt_Blance : Form
    {
        string Param1, Param2,Param3,Code;
        DataTable dt = new DataTable();
     
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        public Frm_Rpt_Blance(string _Param1,string _Param2,string _Param3,string _Code)
        {
            InitializeComponent();
            Param1 = _Param1;
            Param2 = _Param2;
            Param3 = _Param3;
            Code = _Code;
        }

        private void Frm_Rpt_Blance_Load(object sender, EventArgs e)
        {
            bt_Print_Click(sender, e);
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                dt = clDoc.ReturnTable(ConSale.ConnectionString, @"SELECT      " + ConWare.Database + @".dbo.table_002_MainGroup.column02 AS MainGroup, 
        " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup.column03 AS SubsidiaryGroup, 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01 AS CodeKala, 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column02 AS NameKala, SUM(dbo.Table_011_Child1_SaleFactor.column07) AS TotalCount, 
                         SUM(dbo.Table_011_Child1_SaleFactor.column11) AS FiSale, " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.Column35 AS LastFiBuy, 
                         SUM(dbo.Table_011_Child1_SaleFactor.column11) / SUM(dbo.Table_011_Child1_SaleFactor.column07) AS FiMediumSale, 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.Column35 * SUM(dbo.Table_011_Child1_SaleFactor.column07) AS SumlastBuy, 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.Column35 * SUM(dbo.Table_011_Child1_SaleFactor.column07) 
                         - SUM(dbo.Table_011_Child1_SaleFactor.column11) AS Total, dbo.Table_010_SaleFactor.column02 AS Date
FROM            dbo.Table_010_SaleFactor INNER JOIN
                         dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients ON 
                         dbo.Table_011_Child1_SaleFactor.column02 = " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
                         " + ConWare.Database + @".dbo.table_002_MainGroup ON 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column03 = " + ConWare.Database + @".dbo.table_002_MainGroup.columnid INNER JOIN
                         " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup ON 
                         " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column04 = " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup.columnid
GROUP BY " + ConWare.Database + @".dbo.table_002_MainGroup.column02, " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column02, 
                        " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01, " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.Column35, 
                         " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup.column03, dbo.Table_010_SaleFactor.column02
HAVING        (dbo.Table_010_SaleFactor.column02 >= '" + Param1 + @"') AND  (dbo.Table_010_SaleFactor.column02 <= '" + Param2 + @"') AND (" + ConWare.Database + @".dbo.table_004_CommodityAndIngredients.column01 in (" + Code.TrimEnd(',') + @"))");

                StiReport stireport = new StiReport();

                stireport.Load("Rpt_FiBuyBlance.mrt");
                stireport.Pages["Page1"].Enabled = true;
                stireport.Compile();
                stireport.RegData("ReportProfit", dt);
                stireport["Param1"] = Param1;
                stireport["Param2"] = Param2;
                stireport["Param3"] = Param3;

                try
                {
                    DataTable Org = Class_BasicOperation.LogoTable();
                    if (!checkwithLogo.Checked)
                    {
                        stireport.RegData("Table_000_OrgInfo", Class_BasicOperation.LogoTable());
                    }
                    else { stireport.RegData("Table_000_OrgInfo", Class_BasicOperation.LogoTable().Clone()); }
                }
                catch { }
                this.Cursor = Cursors.Default;
                stireport.Render(false);
                stiViewerControl1.Report = stireport;

            }

            catch (Exception ex)
            { Class_BasicOperation.CheckExceptionType(ex, this.Name); }
        }

        private void btDesign_Click(object sender, EventArgs e)
        {

            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("Rpt_FiBuyBlance.mrt");
            r.Design();
        }
    }
}
