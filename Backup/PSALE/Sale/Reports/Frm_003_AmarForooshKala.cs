using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace PSALE.Sale.Reports
{
    public partial class Frm_003_AmarForooshKala : Form
    {
        db alldatabase = new db();
        
        public Frm_003_AmarForooshKala()
        {
            InitializeComponent();
        }

        private void Frm_003_AmarForooshKala_Load(object sender, EventArgs e)
        {
            toolStripComboBox1.SelectedIndex = 0;
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            get_data("0");

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            bool type;
            if (toolStripComboBox1.SelectedIndex == 0)
                get_data("0");
            else
                get_data("1");
           
            




        }




        public void get_data(string batel)
        {
            db alldatabase = new db();
            SqlDataAdapter headerAdapter, child1, child2;

            SqlConnection con = new SqlConnection();
            DataSet dataSet1 = new DataSet();

            string dbname;
            dbname = table_008_Child_PwhrsDraftTableAdapter1.Connection.Database.ToString();



            con.Close();
            con.ConnectionString = Properties.Settings.Default.PSALE_ConnectionString;
            con.Open();



            string date1 = faDatePickerStrip1.FADatePicker.Text;
            string date2 = faDatePickerStrip2.FADatePicker.Text;





            string s = @"SELECT        Table_011_Child1_SaleFactor.column02, ISNULL(SUM(Table_011_Child1_SaleFactor.column07), 0) AS TedadKol, derivedtbl_1.Arzesh, 
                    SUM(Table_011_Child1_SaleFactor.column20) AS JamMablagh, PWHRS_XX_XXXX.dbo.table_004_CommodityAndIngredients.column02 AS namekala
                    FROM            Table_011_Child1_SaleFactor INNER JOIN
                    Table_010_SaleFactor ON Table_011_Child1_SaleFactor.column01 = Table_010_SaleFactor.columnid INNER JOIN
                    (SELECT        IdKala, Arzesh
                    FROM            PWHRS_XX_XXXX.dbo.AvgPrice() AS AvgPrice_1) AS derivedtbl_1 ON Table_011_Child1_SaleFactor.column02 = derivedtbl_1.IdKala INNER JOIN
                    PWHRS_XX_XXXX.dbo.table_004_CommodityAndIngredients ON 
                    Table_011_Child1_SaleFactor.column02 = PWHRS_XX_XXXX.dbo.table_004_CommodityAndIngredients.columnid
                    WHERE        (Table_010_SaleFactor.column02 BETWEEN '" + date1 + @"' AND '" + date2 + @"') AND (Table_010_SaleFactor.column17 = " + batel + @")
                    GROUP BY Table_011_Child1_SaleFactor.column02, derivedtbl_1.Arzesh, PWHRS_XX_XXXX.dbo.table_004_CommodityAndIngredients.column02";
            s = s.Replace("PWHRS_XX_XXXX", dbname);
            headerAdapter = new SqlDataAdapter(s, con);
            headerAdapter.Fill(dataSet1, "h1");


            s = @"SELECT        PERP_Base.dbo.Table_045_PersonInfo.ColumnId AS PersonId, PERP_Base.dbo.Table_045_PersonInfo.Column02 AS Name, 
                         SUM(Table_011_Child1_SaleFactor.column07) AS Tedad, SUM(Table_011_Child1_SaleFactor.column20) AS Mablagh, 
                         Table_011_Child1_SaleFactor.column02 AS Idkala, CAST(PERP_Base.dbo.Table_045_PersonInfo.ColumnId AS nvarchar) 
                         + CAST(Table_011_Child1_SaleFactor.column02 AS nvarchar) AS IdPersonKala
                        FROM            Table_011_Child1_SaleFactor INNER JOIN
                                                 Table_010_SaleFactor ON Table_011_Child1_SaleFactor.column01 = Table_010_SaleFactor.columnid INNER JOIN
                                                 PERP_Base.dbo.Table_045_PersonInfo ON Table_010_SaleFactor.column03 = PERP_Base.dbo.Table_045_PersonInfo.ColumnId
                        WHERE        (Table_010_SaleFactor.column02 BETWEEN '" + date1 + @"' AND '" + date2 + @"') AND (Table_010_SaleFactor.column17 =" + batel + @")
                        GROUP BY PERP_Base.dbo.Table_045_PersonInfo.ColumnId, PERP_Base.dbo.Table_045_PersonInfo.Column02, Table_011_Child1_SaleFactor.column02";
            s = s.Replace("PWHRS_XX_XXXX", dbname);



            child1 = new SqlDataAdapter(s, con);




            child1.Fill(dataSet1, "c1");

            s = @"SELECT        Table_010_SaleFactor.column01 AS CodeFaktor, Table_011_Child1_SaleFactor.column02, Table_010_SaleFactor.column03, 
                         Table_010_SaleFactor.column02 AS Tarikh, CAST(Table_010_SaleFactor.column03 AS nvarchar) + CAST(Table_011_Child1_SaleFactor.column02 AS nvarchar) 
                         AS Idshakhkala, SUM(Table_011_Child1_SaleFactor.column07) AS Tedad, SUM(Table_011_Child1_SaleFactor.column20) AS Mablagh, 
                         CAST(Table_010_SaleFactor.column03 AS nvarchar) + CAST(Table_011_Child1_SaleFactor.column02 AS nvarchar) 
                         + CAST(Table_010_SaleFactor.column01 AS nvarchar) AS id
        FROM            Table_010_SaleFactor INNER JOIN
                                 Table_011_Child1_SaleFactor ON Table_010_SaleFactor.columnid = Table_011_Child1_SaleFactor.column01
        WHERE        (Table_010_SaleFactor.column17 = " + batel + @")
        GROUP BY Table_010_SaleFactor.column01, Table_011_Child1_SaleFactor.column02, Table_010_SaleFactor.column03, Table_010_SaleFactor.column02, 
                                 CAST(Table_010_SaleFactor.column03 AS nvarchar) + CAST(Table_011_Child1_SaleFactor.column02 AS nvarchar), 
                                 CAST(Table_010_SaleFactor.column03 AS nvarchar) + CAST(Table_011_Child1_SaleFactor.column02 AS nvarchar) 
                                 + CAST(Table_010_SaleFactor.column01 AS nvarchar)
        HAVING        (Table_010_SaleFactor.column02 BETWEEN '" + date1 + @"' AND '" + date2 + "')";

            s = s.Replace("PWHRS_XX_XXXX", dbname);

            child2 = new SqlDataAdapter(s, con);



            child2.Fill(dataSet1, "c2");

            dataSet1.Tables["h1"].PrimaryKey = new DataColumn[] { dataSet1.Tables["h1"].Columns["column02"] };
            dataSet1.Tables["c1"].PrimaryKey = new DataColumn[] { dataSet1.Tables["c1"].Columns["IdPersonKala"] };
            dataSet1.Tables["c2"].PrimaryKey = new DataColumn[] { dataSet1.Tables["c2"].Columns["id"] };

            DataRelation r1 = new DataRelation("r1", dataSet1.Tables["h1"].Columns["column02"], dataSet1.Tables["c1"].Columns["Idkala"]);
            dataSet1.Relations.Add(r1);
            DataRelation r2 = new DataRelation("r2", dataSet1.Tables["c1"].Columns["IdPersonKala"], dataSet1.Tables["c2"].Columns["Idshakhkala"]);
            dataSet1.Relations.Add(r2);

            BindingSource bs1 = new BindingSource();
            bs1.DataSource = dataSet1.Tables["h1"];

            gridEX2.DataSource = bs1;

            BindingSource bs2 = new BindingSource();
            bs2.DataSource = bs1;
            bs2.DataMember = "r1";
            gridEX1.DataSource = bs2;

            BindingSource bs3 = new BindingSource();
            bs3.DataSource = bs2;
            bs3.DataMember = "r2";
            gridEX3.DataSource = bs3;
            bindingNavigator1.BindingSource = bs1;
        }
      









    }
}
