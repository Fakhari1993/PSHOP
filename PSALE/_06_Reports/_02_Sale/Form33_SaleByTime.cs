using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Janus.Windows.GridEX;
using System.IO;
using System.Data.SqlClient;
namespace PSHOP._06_Reports._02_Sale
{
    public partial class Form33_SaleByTime : Form
    {
        Classes.Class_Documents class1 = new Classes.Class_Documents();

        public Form33_SaleByTime()
        {
            InitializeComponent();
        }

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX2;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream File = (FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "None");
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string j = " از تاریخ:" + faDatePickerStrip1.FADatePicker.Text + " تا تاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                        gridEXPrintDocument1.PageHeaderLeft = j;
                        gridEXPrintDocument1.PageHeaderRight = "گزارش ساعتی";
                        printPreviewDialog1.ShowDialog();
                    }
            }
            catch { }
        }

        private void Form33_SaleByTime_Load(object sender, EventArgs e)
        {
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;

            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
        }

        private void Form33_SaleByTime_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.gridEX2.RemoveFilters();
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            try
            {
                if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue && this.faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue)
                {




                    DataSet ds = new DataSet();

                    using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.SALE))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("Proc_01_SeprateHourlyReport", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 120;
                        var param1 = cmd.Parameters.AddWithValue("@startDate", faDatePickerStrip1.FADatePicker.SelectedDateTime);
                        var param2 = cmd.Parameters.AddWithValue("@endDate", faDatePickerStrip2.FADatePicker.SelectedDateTime);

                        SqlDataAdapter Adapter = new SqlDataAdapter(cmd);
                        Adapter.Fill(ds);
                        connection.Close();
                        gridEX2.DataSource = ds.Tables[0];
                    }

                    ChartDataBinding(ds.Tables[0], "count", "amount", "time");


                }
            }
            catch
            {
            }
        }
        private void ChartDataBinding(DataTable DT, string YValueMembers, string YValueMembers2, string XValueMember)
        {
            // reset chart control
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.ChartAreas.Add("ChartArea1");
            chart1.ChartAreas.Add("ChartArea2");


            // connect data source to series
            chart1.Series.Add("تعداد");
            chart1.Series.Add("مقدار فروش");

            //chart1.Series[0].IsXValueIndexed = true;
            chart1.Series[0].YValueMembers = YValueMembers;
            chart1.Series[0].XValueMember = XValueMember;
            chart1.Series[0].ChartArea = "ChartArea1";
            chart1.Series[0].ChartType = class1.SetChartType("Column");
            chart1.Series[0].Font = new Font("Tahoma", 12);



            //chart1.Series[1].IsXValueIndexed = true;
            chart1.Series[1].YValueMembers = YValueMembers2;
            chart1.Series[1].XValueMember = XValueMember;
            chart1.Series[1].ChartArea = "ChartArea2";
            chart1.Series[1].ChartType = class1.SetChartType("Column");
            chart1.Series[1].Font = new Font("Tahoma", 12);

            // bind

            chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoom(2, 3);
            chart1.ChartAreas["ChartArea1"].CursorX.IsUserEnabled = true;
            chart1.ChartAreas["ChartArea1"].CursorY.IsUserEnabled = true;
            chart1.ChartAreas["ChartArea1"].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas["ChartArea1"].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas["ChartArea1"].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas["ChartArea1"].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas["ChartArea1"].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas["ChartArea1"].AxisY.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas["ChartArea2"].AxisX.ScaleView.Zoom(2, 3);
            chart1.ChartAreas["ChartArea2"].CursorX.IsUserEnabled = true;
            chart1.ChartAreas["ChartArea2"].CursorY.IsUserEnabled = true;
            chart1.ChartAreas["ChartArea2"].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas["ChartArea2"].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas["ChartArea2"].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas["ChartArea2"].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas["ChartArea2"].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas["ChartArea2"].AxisY.ScrollBar.IsPositionedInside = true;
            chart1.DataSource = DT;
            chart1.DataBind();




        }
    }
}
