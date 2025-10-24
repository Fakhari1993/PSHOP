using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
namespace PSHOP._06_Reports._03_Buy
{
    public partial class Form45_ShowForecast : Form
    {
        DataTable orgdt = new DataTable();
        DataTable _dt = new DataTable();
        int from = 0;
        int to = 0;
        int currentw = 0;
        bool _BackSpace = false;

        public Form45_ShowForecast(DataTable dt, DataTable _orgdt)
        {
            InitializeComponent();
            _dt = dt;
            orgdt = _orgdt;
            PersianCalendar pc2 = new PersianCalendar();
            DateTime endofyear = new DateTime(FarsiLibrary.Utils.PersianDate.Now.Year, 12, 29, pc2);

            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip2.FADatePicker.SelectedDateTime = endofyear;


            CultureInfo myCI = new CultureInfo("fa-IR");
            Calendar myCal = myCI.Calendar;
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            PersianCalendar pc = new PersianCalendar();
            DateTime gmindate = new DateTime(Convert.ToInt32(faDatePickerStrip1.FADatePicker.Text.Substring(0, 4)), Convert.ToInt32(faDatePickerStrip1.FADatePicker.Text.Substring(5, 2)), Convert.ToInt32(faDatePickerStrip1.FADatePicker.Text.Substring(8, 2)), pc);
            from = pc.GetWeekOfYear(gmindate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);




            DateTime todate = new DateTime(Convert.ToInt32(faDatePickerStrip2.FADatePicker.Text.Substring(0, 4)), Convert.ToInt32(faDatePickerStrip2.FADatePicker.Text.Substring(5, 2)), Convert.ToInt32(faDatePickerStrip2.FADatePicker.Text.Substring(8, 2)), pc);
            to = pc.GetWeekOfYear(todate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            DateTime current = new DateTime(Convert.ToInt32(FarsiLibrary.Utils.PersianDate.Now.Year), Convert.ToInt32(FarsiLibrary.Utils.PersianDate.Now.Month), Convert.ToInt32(FarsiLibrary.Utils.PersianDate.Now.Day), pc);
            currentw = pc.GetWeekOfYear(current, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            faDatePickerStrip1.Select();
        }

        private void Form44_ShowForecast_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Factors.RemoveFilters();
            gridEX1.RemoveFilters();
        }

        private void mnu_ExportToExcel_Header_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Factors;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                //  gridEXPrintDocument1.GridEX = gridEXGroup;
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        // string j = " از تاریخ:" + faDatePickerStrip1.FADatePicker.Text + " تا تاریخ:" + faDatePickerStrip2.FADatePicker.Text;
                        // gridEXPrintDocument1.PageHeaderLeft = j;
                        gridEXPrintDocument1.PageHeaderRight = "پیش بینی خرید";
                        printPreviewDialog1.ShowDialog();
                    }
            }
            catch { }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (gridEX_Factors.GetCheckedRows().Count() == 0)
            {
                Class_BasicOperation.ShowMsg("", "برای ثبت درخواست کالا انتخاب کنید", "None");
                return;
            }
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 77))
            {
                Form46_RequestBuy f = new Form46_RequestBuy(gridEX_Factors);
                f.ShowDialog();

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX_Factors_SelectionChanged(object sender, EventArgs e)
        {
            try
            {

                DataTable goodd = new DataTable();

                goodd.Columns.Add("week", typeof(int));
                goodd.Columns.Add("value", typeof(Decimal));
                goodd.Columns.Add("forecastvalue", typeof(Decimal));

                if (faDatePickerStrip1.FADatePicker.Text.Substring(0, 4) == faDatePickerStrip2.FADatePicker.Text.Substring(0, 4))
                {
                    for (int i = from + 1; i <= to; i++)
                    {


                        var newDt = orgdt.AsEnumerable()
                                      .GroupBy(r => r.Field<int>("id"))
                                      .Select(g => new
                                      {
                                          id = g.Key,
                                          value = g.Where(order => order.Field<int>("week") == i).Sum(r => r.Field<decimal>("value"))

                                      }).Where(k => k.id == Convert.ToInt32(gridEX_Factors.GetValue("id"))).ToList();


                        if (newDt.Count > 0)
                            foreach (var row in newDt)
                                goodd.Rows.Add(i, row.value, Convert.ToDecimal(row.value) + ((Convert.ToDecimal(row.value) * Convert.ToDecimal(gridEX_Factors.GetValue("increasepercent"))) / Convert.ToDecimal(100)));
                        else
                            goodd.Rows.Add(i, 0.000, 0.000);
                    }
                }
                else
                {
                    int tempt = to;
                    int tempf = from;

                    to = 53;
                    for (int i = from + 1; i <= to; i++)
                    {


                        var newDt = orgdt.AsEnumerable()
                                      .GroupBy(r => r.Field<int>("id"))
                                      .Select(g => new
                                      {
                                          id = g.Key,
                                          value = g.Where(order => order.Field<int>("week") == i).Sum(r => r.Field<decimal>("value"))

                                      }).Where(k => k.id == Convert.ToInt32(gridEX_Factors.GetValue("id"))).ToList();


                        if (newDt.Count > 0)
                            foreach (var row in newDt)
                                goodd.Rows.Add(i, row.value, Convert.ToDecimal(row.value) + ((Convert.ToDecimal(row.value) * Convert.ToDecimal(gridEX_Factors.GetValue("increasepercent"))) / Convert.ToDecimal(100)));
                        else
                            goodd.Rows.Add(i, 0.000, 0.000);
                    }


                    to = tempt;
                    for (int i = 1; i <= to; i++)
                    {


                        var newDt = orgdt.AsEnumerable()
                                      .GroupBy(r => r.Field<int>("id"))
                                      .Select(g => new
                                      {
                                          id = g.Key,
                                          value = g.Where(order => order.Field<int>("week") == i).Sum(r => r.Field<decimal>("value"))

                                      }).Where(k => k.id == Convert.ToInt32(gridEX_Factors.GetValue("id"))).ToList();


                        if (newDt.Count > 0)
                            foreach (var row in newDt)
                                goodd.Rows.Add(i, row.value, Convert.ToDecimal(row.value) + ((Convert.ToDecimal(row.value) * Convert.ToDecimal(gridEX_Factors.GetValue("increasepercent"))) / Convert.ToDecimal(100)));
                        else
                            goodd.Rows.Add(i, 0.000, 0.000);
                    }

                }
                gridEX1.DataSource = goodd;

            }
            catch
            {
            }

        }

        private void uiPanel1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Forecast_Click(object sender, EventArgs e)
        {

            CultureInfo myCI = new CultureInfo("fa-IR");
            Calendar myCal = myCI.Calendar;
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            PersianCalendar pc = new PersianCalendar();
            DateTime gmindate = new DateTime(Convert.ToInt32(faDatePickerStrip1.FADatePicker.Text.Substring(0, 4)), Convert.ToInt32(faDatePickerStrip1.FADatePicker.Text.Substring(5, 2)), Convert.ToInt32(faDatePickerStrip1.FADatePicker.Text.Substring(8, 2)), pc);
            from = pc.GetWeekOfYear(gmindate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);




            DateTime todate = new DateTime(Convert.ToInt32(faDatePickerStrip2.FADatePicker.Text.Substring(0, 4)), Convert.ToInt32(faDatePickerStrip2.FADatePicker.Text.Substring(5, 2)), Convert.ToInt32(faDatePickerStrip2.FADatePicker.Text.Substring(8, 2)), pc);
            to = pc.GetWeekOfYear(todate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            if (!faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue || !faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue)
            {
                MessageBox.Show("اطلاعات را تکمیل کنید");
                return;
            }
            if (faDatePickerStrip2.FADatePicker.SelectedDateTime.Value.Date < faDatePickerStrip1.FADatePicker.SelectedDateTime.Value.Date)
            {
                MessageBox.Show("اطلاعات ورودی معتبر نیست");
                return;
            }
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("پیش بینی برای هفته های پس از هفته جاری امکان پذیر است");
                return;
            }



            try
            {

                DataTable goodd = new DataTable();

                goodd.Columns.Add("id", typeof(int));
                goodd.Columns.Add("code", typeof(string));
                goodd.Columns.Add("name", typeof(string));
                goodd.Columns.Add("subgroup", typeof(string));
                goodd.Columns.Add("maingroup", typeof(string));
                goodd.Columns.Add("sum", typeof(Decimal));
                goodd.Columns.Add("increasepercent", typeof(Decimal));
                goodd.Columns.Add("forecastvalue", typeof(Decimal));



                if (faDatePickerStrip1.FADatePicker.Text.Substring(0, 4) == faDatePickerStrip2.FADatePicker.Text.Substring(0, 4))
                {
                    foreach (DataRow j in _dt.Rows)
                    {
                        decimal sum = 0;
                        for (int i = from + 1; i <= to; i++)
                        {
                            var newDt = orgdt.AsEnumerable()
                                          .GroupBy(r => r.Field<int>("id"))
                                          .Select(g => new
                                          {
                                              id = g.Key,
                                              value = g.Where(order => order.Field<int>("week") == i).Sum(r => r.Field<decimal>("value"))

                                          }).Where(k => k.id == Convert.ToInt32(j["id"].ToString())).ToList();


                            if (newDt.Count > 0)
                                foreach (var row in newDt)
                                    sum += Convert.ToDecimal(row.value);

                            else

                                sum += 0;

                        }
                        goodd.Rows.Add(j["id"],
                                        j["code"],
                                        j["name"],
                                        j["subgroup"],
                                        j["maingroup"],
                                        sum,
                                        j["increasepercent"],
                                        Convert.ToDecimal(sum) + ((Convert.ToDecimal(sum) * Convert.ToDecimal(j["increasepercent"])) / Convert.ToDecimal(100)));
                    }

                }
                else
                {

                    foreach (DataRow j in _dt.Rows)
                    {
                        decimal sum = 0;
                        for (int i = from + 1; i <= 53; i++)
                        {
                            var newDt = orgdt.AsEnumerable()
                                          .GroupBy(r => r.Field<int>("id"))
                                          .Select(g => new
                                          {
                                              id = g.Key,
                                              value = g.Where(order => order.Field<int>("week") == i).Sum(r => r.Field<decimal>("value"))

                                          }).Where(k => k.id == Convert.ToInt32(j["id"].ToString())).ToList();


                            if (newDt.Count > 0)
                                foreach (var row in newDt)
                                    sum += Convert.ToDecimal(row.value);

                            else

                                sum += 0;

                        }

                        for (int i = 1; i <= to; i++)
                        {
                            var newDt = orgdt.AsEnumerable()
                                          .GroupBy(r => r.Field<int>("id"))
                                          .Select(g => new
                                          {
                                              id = g.Key,
                                              value = g.Where(order => order.Field<int>("week") == i).Sum(r => r.Field<decimal>("value"))

                                          }).Where(k => k.id == Convert.ToInt32(j["id"].ToString())).ToList();


                            if (newDt.Count > 0)
                                foreach (var row in newDt)
                                    sum += Convert.ToDecimal(row.value);

                            else

                                sum += 0;

                        }

                        goodd.Rows.Add(j["id"],
                                        j["code"],
                                        j["name"],
                                        j["subgroup"],
                                        j["maingroup"],
                                        sum,
                                        j["increasepercent"],
                                        Convert.ToDecimal(sum) + ((Convert.ToDecimal(sum) * Convert.ToDecimal(j["increasepercent"])) / Convert.ToDecimal(100)));
                    }



                }

                gridEX_Factors.DataSource = goodd;

            }
            catch
            {
            }

        }

        private void faDatePickerStrip1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip1.FADatePicker.HideDropDown();
                    faDatePickerStrip2.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePickerStrip2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip1.FADatePicker.HideDropDown();
                    btn_Forecast.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDatePickerStrip1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox = (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


                if (textBox.FADatePicker.Text.Length == 4)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
                else if (textBox.FADatePicker.Text.Length == 7)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
            }
        }

        private void faDatePickerStrip2_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePickerStrip textBox = (FarsiLibrary.Win.Controls.FADatePickerStrip)sender;


                if (textBox.FADatePicker.Text.Length == 4)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
                else if (textBox.FADatePicker.Text.Length == 7)
                {
                    textBox.FADatePicker.Text += "/";
                    textBox.FADatePicker.SelectionStart = textBox.FADatePicker.Text.Length;
                }
            }
        }
    }
}
