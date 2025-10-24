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

namespace PSHOP._06_Reports
{
    public partial class Frm_003_PrintSearchDoc : Form
    {
        string Param1, Param2, Param3;
        DataTable dt = new DataTable();
        string Number;

        string commandtext = "";
        Classes.Class_Documents cldoc = new Classes.Class_Documents();
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);

        public Frm_003_PrintSearchDoc()
        {
            InitializeComponent();


        }


        public Frm_003_PrintSearchDoc(string _Param1, string _Param2, string _Param3, string _Number)
        {
            InitializeComponent();

            Param1 = _Param1;
            Param2 = _Param2;
            Param3 = _Param3;
            Number = _Number;

        }
        private void Frm_003_PrintSearchDoc_Load(object sender, EventArgs e)
        {

            try
            {
                StiReport rpt = new StiReport();
                rpt.Load("Rpt_PrintDayDoc.mrt");
                for (int i = 0; i < rpt.Pages.Count; i++)
                {
                    cmbPage.Items.Add(rpt.Pages[i].Name);
                }
                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.TypSeachDoc.ToString()))

                    cmbPage.SelectedIndex = Convert.ToInt32(Properties.Settings.Default.TypSeachDoc.ToString());

            }
            catch { }

            bt_Print_Click(sender, e);



        }

        private void bt_Print_Click(object sender, EventArgs e)
        {


            commandtext = @"SELECT        dt.Id, dt.Number, dt.Date, dt.ACC_C, dt.PersonId, dt.Center1, dt.Project1, dt.Description, dt.Bed, dt.Bes, dt.SanadTyp1, dt.Refrenc, dt.DetailId, dt.SaleType1, ah.ACC_Code, ah.ACC_Name, 
                         dt.DetailId AS DetailId, " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column02 AS Person, " + ConBase.Database + @".dbo.Table_035_ProjectInfo.Column02 AS Project, " + ConBase.Database + @".dbo.Table_030_ExpenseCenterInfo.Column02 AS Center, 
                         " + ConBase.Database + @".dbo.Table_002_SalesTypes.column02 as TypeSale, " + ConBase.Database + @".dbo.Table_075_SanadTypes.Column01 as SanadTyp
FROM            (SELECT DISTINCT 
                                                    Table_060_SanadHead_1.ColumnId AS Id, Table_060_SanadHead_1.Column00 AS Number, Table_060_SanadHead_1.Column01 AS Date, Table_065_SanadDetail_1.Column01 AS ACC_C, 
                                                    Table_065_SanadDetail_1.Column07 AS PersonId, Table_065_SanadDetail_1.Column08 AS Center1, Table_065_SanadDetail_1.Column09 AS Project1, 
                                                    Table_065_SanadDetail_1.Column10 AS Description, Table_065_SanadDetail_1.Column11 AS Bed, Table_065_SanadDetail_1.Column12 AS Bes, Table_065_SanadDetail_1.Column16 AS SanadTyp1, 
                                                    Table_065_SanadDetail_1.Column17 AS Refrenc, Table_065_SanadDetail_1.ColumnId AS DetailId, CASE WHEN (Table_065_SanadDetail_1.column16 = 15) THEN
                                                        (SELECT        Column36
                                                          FROM            " + ConSale.Database + @".dbo.Table_010_SaleFactor
                                                          WHERE        Columnid = Table_065_SanadDetail_1.column17) ELSE NULL END AS SaleType1
                          FROM            dbo.Table_060_SanadHead AS Table_060_SanadHead_1 LEFT OUTER JOIN
                                                    dbo.Table_065_SanadDetail AS Table_065_SanadDetail_1 ON Table_060_SanadHead_1.ColumnId = Table_065_SanadDetail_1.Column00) AS dt LEFT OUTER JOIN
                         " + ConBase.Database + @".dbo.Table_075_SanadTypes ON dt.SanadTyp1 = " + ConBase.Database + @".dbo.Table_075_SanadTypes.Column00 LEFT OUTER JOIN
                         " + ConBase.Database + @".dbo.Table_002_SalesTypes ON dt.SaleType1 = " + ConBase.Database + @".dbo.Table_002_SalesTypes.columnid LEFT OUTER JOIN
                         " + ConBase.Database + @".dbo.Table_030_ExpenseCenterInfo ON dt.Center1 = " + ConBase.Database + @".dbo.Table_030_ExpenseCenterInfo.Column00 LEFT OUTER JOIN
                         " + ConBase.Database + @".dbo.Table_035_ProjectInfo ON dt.Project1 = " + ConBase.Database + @".dbo.Table_035_ProjectInfo.Column00 LEFT OUTER JOIN
                         " + ConBase.Database + @".dbo.Table_045_PersonInfo ON dt.PersonId = " + ConBase.Database + @".dbo.Table_045_PersonInfo.ColumnId LEFT OUTER JOIN
                         dbo.AllHeaders() AS ah ON ah.ACC_Code = dt.ACC_C
WHERE        dt.DetailId in(" + Number.TrimEnd(',') + ")  ";
            dt = cldoc.ReturnTable(ConAcnt.ConnectionString, commandtext);

            try
            {


                StiReport stireport = new StiReport();
                stireport.Load("Rpt_PrintDayDoc.mrt");
                try
                {
                    for (int i = 0; i < stireport.Pages.Count; i++)
                    {
                        stireport.Pages[i].Enabled = false;
                    }
                    stireport.Pages[cmbPage.SelectedIndex].Enabled = true;
                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("Index was out of range. Must be non-negative and less than the size of the collection.Parameter name: index"))

                        MessageBox.Show("لطفا الگو چاپ را انتخاب نمایید");
                    return;
                }



                stireport.Compile();
                stireport.RegData("dt_DocDay", dt);
                stireport["Param1"] = Param1;
                stireport["Param2"] = Param2;
                stireport["Param3"] = Param3;

                //stireport["Image"] = Image.FromStream(stream);
                //stireport["Namecompany"] = Namecompany;

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
                Properties.Settings.Default.TypSeachDoc = cmbPage.SelectedIndex.ToString();
                Properties.Settings.Default.Save();

            }

            catch (Exception ex)
            { Class_BasicOperation.CheckExceptionType(ex, this.Name); }




        }

        private void btDesign_Click(object sender, EventArgs e)
        {
            Stimulsoft.Report.StiReport r = new Stimulsoft.Report.StiReport();
            r.Load("Rpt_PrintDayDoc.mrt");
            r.Design();
        }
    }
}
