using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._02_BasicInfo.Reports
{
    public partial class Frm_001_ReportOstanShahr : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        int _code;
        public Frm_001_ReportOstanShahr(int code)
        {
            _code = code;
            InitializeComponent();
        }

        private void FRM_001_ReportOstanShahr_Load(object sender, EventArgs e)
        {
            _02_BasicInfo.Reports.Rpt_02_ShahrOstan cr = new Rpt_02_ShahrOstan();
            cr.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
            string s = "";

            if (_code == 0)
            {
                s = @"SELECT         dbo.Table_060_ProvinceInfo.Column01 AS NameOstan,  dbo.Table_060_ProvinceInfo.Column02 AS MarkazOstan, 
                dbo.Table_060_ProvinceInfo.Column03 AS TozihatOstan,  dbo.Table_060_ProvinceInfo.Column04 AS masahat, 
                dbo.Table_060_ProvinceInfo.Column05 AS jameat,  dbo.Table_060_ProvinceInfo.Column06 AS fasele, 
                dbo.Table_065_CityInfo.Column02 AS NameShahr,  dbo.Table_065_CityInfo.Column03 AS PishShomare, 
                dbo.Table_065_CityInfo.Column04 AS PishShomareKeshvar,  dbo.Table_065_CityInfo.Column05 AS fasele, 
                dbo.Table_065_CityInfo.Column06 AS tozihat,  dbo.Table_065_CityInfo.Column07 AS masahat, 
                dbo.Table_065_CityInfo.Column08 AS jameeat,  dbo.Table_065_CityInfo.Column00 AS idostan
                FROM             dbo.Table_065_CityInfo INNER JOIN
                dbo.Table_060_ProvinceInfo ON  dbo.Table_065_CityInfo.Column00 =  dbo.Table_060_ProvinceInfo.Column00";
            }

            else
            {
                s = @"SELECT         dbo.Table_060_ProvinceInfo.Column01 AS NameOstan,  dbo.Table_060_ProvinceInfo.Column02 AS MarkazOstan, 
                dbo.Table_060_ProvinceInfo.Column03 AS TozihatOstan,  dbo.Table_060_ProvinceInfo.Column04 AS masahat, 
                dbo.Table_060_ProvinceInfo.Column05 AS jameat,  dbo.Table_060_ProvinceInfo.Column06 AS fasele, 
                dbo.Table_065_CityInfo.Column02 AS NameShahr,  dbo.Table_065_CityInfo.Column03 AS PishShomare, 
                dbo.Table_065_CityInfo.Column04 AS PishShomareKeshvar,  dbo.Table_065_CityInfo.Column05 AS fasele, 
                dbo.Table_065_CityInfo.Column06 AS tozihat,  dbo.Table_065_CityInfo.Column07 AS masahat, 
                dbo.Table_065_CityInfo.Column08 AS jameeat,  dbo.Table_065_CityInfo.Column00 AS idostan
                FROM             dbo.Table_065_CityInfo INNER JOIN
                dbo.Table_060_ProvinceInfo ON  dbo.Table_065_CityInfo.Column00 =  dbo.Table_060_ProvinceInfo.Column00
                WHERE  dbo.Table_065_CityInfo.Column00=" + _code.ToString();

            }


            DataTable dt = new DataTable();
            dt = clDoc.ReturnTable(ConBase.ConnectionString, s);

            cr.SetDataSource(dt);
            crystalReportViewer2.ReportSource = cr;

        }
    }
}
