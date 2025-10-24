using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
namespace PSALE.Buy.Reports
{
    public partial class Frm_005_KharidMahMeghdari : Form
    {
        db alldatabase = new db();
        public Frm_005_KharidMahMeghdari()
        {
            InitializeComponent();
        }

        private void fillToolStripButton_Click(object sender, EventArgs e)
        {


        }

        private void Frm_005_KharidMahMeghdari_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'DataSet_Buy_Reports.View_012_BuyReportByMahForChart' table. You can move, or remove it, as needed.
            this.View_012_BuyReportByMahForChartTableAdapter.Fill(this.DataSet_Buy_Reports.View_012_BuyReportByMahForChart,"1390",14);
            // TODO: This line of code loads data into the 'DataSet_Reports.View_012_BuyReportByMahForChart' table. You can move, or remove it, as needed.
         //   this.View_012_BuyReportByMahForChartTableAdapter.Fill(this.DataSet_Reports.View_012_BuyReportByMahForChart);
            // TODO: This line of code loads data into the 'DataSet_Reports.View_012_BuyReportByMahForChart' table. You can move, or remove it, as needed.

            reportViewer1.RefreshReport();
            
            alldatabase.close();
            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.Connect();
            view_012_BuyReportByMahTableAdapter.Fill(dataSet_Reports1.View_012_BuyReportByMah,Class_BasicOperation._Year);
            gridEX2.DropDowns["d"].DataSource = alldatabase.get_list("SELECT * FROM  table_004_CommodityAndIngredients");


    
        }

        private void gridEX2_DoubleClick(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_Reports.View_005_SaleReportByMahForChart' table. You can move, or remove it, as needed.
            try
            {
         

                int id_kala;
                id_kala = int.Parse(gridEX2.GetValue("Idkala").ToString());


                ReportParameter[] parameters = new ReportParameter[1];
                string kalaname;
                parameters[0] = new ReportParameter("Kala", alldatabase.get_one_fiald("SELECT column02 FROM table_004_CommodityAndIngredients WHERE columnid=" + id_kala));
                
                this.View_012_BuyReportByMahForChartTableAdapter.Fill(this.dataSet_Reports1.View_012_BuyReportByMahForChart,"1390", 14);
              
         
                reportViewer1.LocalReport.SetParameters(parameters);
                this.reportViewer1.RefreshReport();
            }
            catch
            {
            }
        }

        private void gridEX2_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }



    }
}

