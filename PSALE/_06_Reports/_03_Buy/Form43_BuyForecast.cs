using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
namespace PSHOP._06_Reports._03_Buy
{
    public partial class Form43_BuyForecast : Form
    {
        bool _BackSpace = false;
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        DataTable Table = new DataTable();

        public Form43_BuyForecast()
        {
            InitializeComponent();
            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now.AddMonths(-3);
            faDatePickerStrip2.FADatePicker.SelectedDateTime = DateTime.Now;
            faDatePickerStrip1.Select();
        }

        private void bt_Search_Click(object sender, EventArgs e)
        {

            Table = new DataTable();
            if (faDatePickerStrip1.FADatePicker.SelectedDateTime.HasValue
                && faDatePickerStrip2.FADatePicker.SelectedDateTime.HasValue
                && (rb_DB.Checked || rb_excel.Checked))
            {
                if (faDatePickerStrip1.FADatePicker.Text.Substring(0, 4) != faDatePickerStrip2.FADatePicker.Text.Substring(0, 4))
                {
                    MessageBox.Show("حداکثر بازه زمانی یکسال است ");
                    return;
                }
                else if (faDatePickerStrip1.FADatePicker.SelectedDateTime >= faDatePickerStrip2.FADatePicker.SelectedDateTime)
                {
                    MessageBox.Show("بازه زمانی معتبر نیست ");
                    return;
                }
                else
                {


                    string CommandText = null;
                    if (rb_DB.Checked)
                    {
                        CommandText = @"SELECT tsf.column02 AS [date],
                                                   tsf.column01 AS [num],
                                                   tcsf.column07 AS [value],
                                                   tcai.column01 AS [code],
                                                   tcai.column02 AS [name],
                                                   tsg.column03 AS subgroup,
                                                   tmg.column02 AS maingroup,
                                                   tcai.columnid as id,
                                                    0 as week
                                            FROM   Table_010_SaleFactor tsf
                                                   JOIN Table_011_Child1_SaleFactor tcsf
                                                        ON  tcsf.column01 = tsf.columnid
                                                   JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                        ON  tcai.columnid = tcsf.column02
                                                   JOIN " + ConWare.Database + @".dbo.table_003_SubsidiaryGroup tsg
                                                        ON  tsg.column01 = tcai.column03
                                                        AND tsg.columnid = tcai.column04
                                                   JOIN " + ConWare.Database + @".dbo.table_002_MainGroup tmg
                                                        ON  tmg.columnid = tsg.column01
                                            WHERE  tsf.column02 >= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                   AND tsf.column02 <= '" + faDatePickerStrip2.FADatePicker.Text + @"'
                                                   AND tsf.column17 = 0
                                                   AND tsf.column19 = 0 order by tsf.column02";

                        SqlDataAdapter Adapter = new SqlDataAdapter(CommandText, ConSale);
                        Adapter.Fill(Table);
                        gridEX_Factors.DataSource = Table;
                    }
                    else if (rb_excel.Checked)
                    {
                        Form44_ImportInfoFromExcel fr = new Form44_ImportInfoFromExcel();
                        fr.ShowDialog();
                        Table = fr.good;
                        gridEX_Factors.DataSource = Table;

                    }

                }
            }
            else
            {
                MessageBox.Show("اطلاعات را تکمیل کنید ");
                return;
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
                    faDatePickerStrip2.FADatePicker.Select();
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

        private void faDatePickerStrip2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDatePickerStrip2.FADatePicker.HideDropDown();
                    rb_excel.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
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

        private void Form43_BuyForecast_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Factors.RemoveFilters();
        }

        private void btn_Forecast_Click(object sender, EventArgs e)
        {
            try
            {
                if (Table.Rows.Count > 0)
                {
                    //string _PreFactorCode = InputBox.Show(
                    //           "پیش بینی برای چند هفته آینده انجام شود؟", "");
                    //if (_PreFactorCode.ToString().Trim() != "")
                    {
                        //ArrayList a1 = new ArrayList();
                        //                        string com = string.Empty;
                        //                        com = @"SELECT tcai.columnid AS id,
                        //                                   0.000 AS [1],
                        //                                   0.000 AS [2],
                        //                                   0.000 AS [3],
                        //                                   0.000 AS [4],
                        //                                   0.000 AS [5],
                        //                                   0.000 AS [6],
                        //                                   0.000 AS [7],
                        //                                   0.000 AS [8],
                        //                                   0.000 AS [9],
                        //                                   0.000 AS [10],
                        //                                   0.000 AS [11],
                        //                                   0.000 AS [12],
                        //                                   0.000 AS [13],
                        //                                   0.000 AS [14],
                        //                                   0.000 AS [15],
                        //                                   0.000 AS [16],
                        //                                   0.000 AS [17],
                        //                                   0.000 AS [18],
                        //                                   0.000 AS [19],
                        //                                   0.000 AS [20],
                        //                                   0.000 AS [21],
                        //                                   0.000 AS [22],
                        //                                   0.000 AS [23],
                        //                                   0.000 AS [24],
                        //                                   0.000 AS [25],
                        //                                   0.000 AS [26],
                        //                                   0.000 AS [27],
                        //                                   0.000 AS [28],
                        //                                   0.000 AS [29],
                        //                                   0.000 AS [30],
                        //                                   0.000 AS [31],
                        //                                   0.000 AS [32],
                        //                                   0.000 AS [33],
                        //                                   0.000 AS [34],
                        //                                   0.000 AS [35],
                        //                                   0.000 AS [36],
                        //                                   0.000 AS [37],
                        //                                   0.000 AS [38],
                        //                                   0.000 AS [39],
                        //                                   0.000 AS [40],
                        //                                   0.000 AS [41],
                        //                                   0.000 AS [42],
                        //                                   0.000 AS [43],
                        //                                   0.000 AS [44],
                        //                                   0.000 AS [45],
                        //                                   0.000 AS [46],
                        //                                   0.000 AS [47],
                        //                                   0.000 AS [48],
                        //                                   0.000 AS [49],
                        //                                   0.000 AS [50],
                        //                                   0.000 AS [51],
                        //                                   0.000 AS [52],
                        //                                   0.000 AS [53]
                        //                            FROM   table_004_CommodityAndIngredients tcai
                        //                            WHERE  tcai.column28 = 1";

                        //                        DataTable good = new DataTable();
                        //                        SqlDataAdapter Adapter = new SqlDataAdapter(com, ConWare);
                        //                        Adapter.Fill(good);

                        this.Cursor = Cursors.WaitCursor;

                        CultureInfo myCI = new CultureInfo("fa-IR");
                        Calendar myCal = myCI.Calendar;
                        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;









                        var grouped = Table.AsEnumerable()
                                                   .GroupBy(r => r.Field<int>("id"))
                                                    .Select(grp => new
                                                                                 {
                                                                                     id = grp.Key,
                                                                                     MaxDate = grp.Max(e1 => e1.Field<string>("date")),
                                                                                     MinDate = grp.Min(e1 => e1.Field<string>("date"))

                                                                                 }).ToList();
                        DataTable newd = new DataTable();
                        newd.Columns.Add("id", typeof(int));
                        newd.Columns.Add("code", typeof(string));
                        newd.Columns.Add("name", typeof(string));
                        newd.Columns.Add("subgroup", typeof(string));
                        newd.Columns.Add("maingroup", typeof(string));
                        newd.Columns.Add("maxdate", typeof(string));
                        newd.Columns.Add("mindate", typeof(string));
                        newd.Columns.Add("maxweek", typeof(int));
                        newd.Columns.Add("minweek", typeof(int));
                        newd.Columns.Add("maxvalue", typeof(decimal));
                        newd.Columns.Add("minvalue", typeof(decimal));
                        newd.Columns.Add("sum", typeof(decimal));
                        newd.Columns.Add("increasepercent", typeof(float));
                        newd.Columns.Add("forecastvalue", typeof(decimal));


                        foreach (var row in grouped)
                        {



                            FarsiLibrary.Utils.PersianDate pmindate = new FarsiLibrary.Utils.PersianDate(row.MinDate);
                            PersianCalendar pc = new PersianCalendar();
                            DateTime gmindate = new DateTime(Convert.ToInt32(row.MinDate.Substring(0, 4)), Convert.ToInt32(row.MinDate.Substring(5, 2)), Convert.ToInt32(row.MinDate.Substring(8, 2)), pc);
                            int minweekofyear = pc.GetWeekOfYear(gmindate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);


                            FarsiLibrary.Utils.PersianDate pmaxdate = new FarsiLibrary.Utils.PersianDate(row.MaxDate);
                            DateTime gmaxdate = new DateTime(Convert.ToInt32(row.MaxDate.Substring(0, 4)), Convert.ToInt32(row.MaxDate.Substring(5, 2)), Convert.ToInt32(row.MaxDate.Substring(8, 2)), pc);
                            int maxweekofyear = pc.GetWeekOfYear(gmaxdate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

                            newd.Rows.Add(row.id, "", "", "", "", row.MaxDate, row.MinDate, maxweekofyear, minweekofyear, 0.000, 0.000, 0.000, 0.000, 0.000);

                        }

                        foreach (DataRow dr in Table.Rows)
                        {
                            foreach (DataRow gooddr in newd.Rows)
                            {
                                FarsiLibrary.Utils.PersianDate pdate = new FarsiLibrary.Utils.PersianDate(dr["date"].ToString());
                                PersianCalendar pc = new PersianCalendar();
                                DateTime gdate = new DateTime(Convert.ToInt32(dr["date"].ToString().Substring(0, 4)), Convert.ToInt32(dr["date"].ToString().Substring(5, 2)), Convert.ToInt32(dr["date"].ToString().Substring(8, 2)), pc);
                                int weekofyear = pc.GetWeekOfYear(gdate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

                                dr.BeginEdit();
                                dr["week"] = weekofyear;
                                dr.EndEdit();

                                if (Convert.ToInt32(dr["id"]) == Convert.ToInt32(gooddr["id"]))
                                {
                                    gooddr.BeginEdit();
                                    gooddr["sum"] = Convert.ToDecimal(gooddr["sum"]) + Convert.ToDecimal(dr["value"]);
                                    gooddr["code"] = dr["code"];
                                    gooddr["name"] = dr["name"];
                                    gooddr["subgroup"] = dr["subgroup"];
                                    gooddr["maingroup"] = dr["maingroup"];

                                    gooddr.EndEdit();
                                }
                                if (Convert.ToInt32(dr["id"]) == Convert.ToInt32(gooddr["id"]) && weekofyear == Convert.ToInt32(gooddr["maxweek"]))
                                {
                                    gooddr.BeginEdit();
                                    gooddr["maxvalue"] = Convert.ToDecimal(gooddr["maxvalue"]) + Convert.ToDecimal(dr["value"]);
                                    gooddr.EndEdit();
                                }


                                if (Convert.ToInt32(dr["id"]) == Convert.ToInt32(gooddr["id"]) && weekofyear == Convert.ToInt32(gooddr["minweek"]))
                                {
                                    gooddr.BeginEdit();
                                    gooddr["minvalue"] = Convert.ToDecimal(gooddr["minvalue"]) + Convert.ToDecimal(dr["value"]);
                                    gooddr.EndEdit();
                                }

                            }

                        }

                        foreach (DataRow gooddr in newd.Rows)
                        {
                            if (Convert.ToDecimal(gooddr["minvalue"]) > 0)
                            {
                                gooddr.BeginEdit();
                                gooddr["increasepercent"] = ((Convert.ToDecimal(gooddr["maxvalue"]) - Convert.ToDecimal(gooddr["minvalue"])) / Math.Abs(Convert.ToDecimal(gooddr["minvalue"]))) * 100;
                                gooddr["forecastvalue"] = Convert.ToDecimal(gooddr["sum"]) + ((Convert.ToDecimal(gooddr["sum"]) * (((Convert.ToDecimal(gooddr["maxvalue"]) - Convert.ToDecimal(gooddr["minvalue"])) / Math.Abs(Convert.ToDecimal(gooddr["minvalue"]))) * 100)) /Convert.ToDecimal( 100));
                                gooddr.EndEdit();
                            }
                        }
                        this.Cursor = Cursors.Default;

                        Form45_ShowForecast fr = new Form45_ShowForecast(newd, Table);
                        fr.ShowDialog();

                        //foreach (DataRow dt in Table.Rows)
                        //{

                        //    FarsiLibrary.Utils.PersianDate pdate = new FarsiLibrary.Utils.PersianDate(dt["date"].ToString());
                        //    DateTime gdate = new DateTime(Convert.ToInt32(dt["date"].ToString().Substring(0, 4)), Convert.ToInt32(dt["date"].ToString().Substring(5, 2)), Convert.ToInt32(dt["date"].ToString().Substring(8, 2)), pc);
                        //    int weekofyear = pc.GetWeekOfYear(gdate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

                        //    DataRow dr = good.Select("id=" + dt["id"]).FirstOrDefault();

                        //    if (dr != null)
                        //    {
                        //        if (!a1.Contains(dt["id"]))
                        //            a1.Add(dt["id"]);
                        //        dr[weekofyear.ToString()] = Convert.ToDecimal(dr[weekofyear.ToString()]) + Convert.ToDecimal(dt["value"]);
                        //    }

                        //}

                        //foreach (object on in a1)
                        //{
                        //    decimal max = 0;
                        //    decimal min = 0;
                        //    DataRow dr = good.Select("id=" + on).FirstOrDefault();


                        //    max = Convert.ToDecimal(dr.ItemArray.Max(element => Convert.ToDecimal(element)));
                        //    min = Convert.ToDecimal(dr.ItemArray.Min(element => Convert.ToDecimal(element)));



                        //}
                    }
                }
            }
            catch
            {
            }
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }
    }
}
