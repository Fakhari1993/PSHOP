using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Design;
using Stimulsoft.Database;
using Stimulsoft.Report.Dictionary;
namespace PSHOP._06_Reports
{

    public partial class Form01_ReportForm : Form
    {
        DataTable _Table = new DataTable();
        DataTable _Table2 = new DataTable();
        int _FormNumber = 0;
        string _Param1, _Param2, _Param3;
        public Form01_ReportForm(DataTable Table, int FormNumber)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table;
        }
        public Form01_ReportForm(DataTable Table, int FormNumber, string Param1, string Param2)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table;
            _Param1 = Param1;
            _Param2 = Param2;
        }
        public Form01_ReportForm(DataTable Table1, DataTable Table2, int FormNumber, string Param1, string Param2)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table1;
            _Table2 = Table2;
            _Param1 = Param1;
            _Param2 = Param2;
        }
        public Form01_ReportForm(DataTable Table1, int FormNumber, string Param1, string Param2, string Param3)
        {
            InitializeComponent();
            _FormNumber = FormNumber;
            _Table = Table1;
            _Param1 = Param1;
            _Param2 = Param2;
            _Param3 = Param3;
        }
        private void Form01_ReportForm_Load(object sender, EventArgs e)
        {
            //نمایش گزارش لیست آخرین قیمت محصولات
            if (_FormNumber == 1)
            {
                desplay();
                _06_Reports._01_Orders.Report02_FinalPriceList rpt = new _01_Orders.Report02_FinalPriceList();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش گزارش روند سفارش محصولات
            else if (_FormNumber == 2)
            {
                desplay();

                _06_Reports._01_Orders.Rpt03_OrderGoods rpt = new _01_Orders.Rpt03_OrderGoods();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Date1", _Param1);
                rpt.SetParameterValue("Date2", _Param2);
                //rpt.SetParameterValue("Param1", _Param3);
                TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
                Title.Text = "نمودار روند سفارشات  " + _Table.Rows[0]["Good"].ToString() + " از تاریخ " + _Param1 + " تا تاریخ " + _Param2;

                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش گزارش سفارشات محصول مشتری
            else if (_FormNumber == 3)
            {
                desplay();

                _06_Reports._01_Orders.Rpt04_Goods_Customer rpt = new _01_Orders.Rpt04_Goods_Customer();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Date1", _Param1);
                rpt.SetParameterValue("Date2", _Param2);
                TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
                Title.Text = "نمودار سفارشات مشتریان از " + _Table.Rows[0]["Good"].ToString() + " از تاریخ " + _Param1 + " تا تاریخ " + _Param2;

                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش گزارش سفارشات مشتری محصول
            else if (_FormNumber == 4)
            {
                desplay();

                _06_Reports._01_Orders.Rpt05_Customer_Goods rpt = new _01_Orders.Rpt05_Customer_Goods();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Date1", _Param1);
                rpt.SetParameterValue("Date2", _Param2);
                TextObject Title = (TextObject)rpt.ReportDefinition.ReportObjects["Text4"];
                Title.Text = "نمودار سفارشات  " + _Table.Rows[0]["Good"].ToString() + " از تاریخ " + _Param1 + " تا تاریخ " + _Param2;

                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش گزارش سفارشات  محصول استانها
            else if (_FormNumber == 5)
            {
                desplay();

                _06_Reports._01_Orders.Rpt06_Good_Province rpt = new _01_Orders.Rpt06_Good_Province();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Date1", _Param1);
                rpt.SetParameterValue("Date2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            // نمایش گزارش سفارشات استانها محصول
            else if (_FormNumber == 6)
            {
                desplay();

                _06_Reports._01_Orders.Rpt07_Province_Good rpt = new _01_Orders.Rpt07_Province_Good();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Date1", _Param1);
                rpt.SetParameterValue("Date2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش گزارش جامع سفارشات
            else if (_FormNumber == 7)
            {
                desplay();

                _06_Reports._01_Orders.Rpt08_CompeleteOrderReport rpt = new _01_Orders.Rpt08_CompeleteOrderReport();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Date1", _Param1);
                rpt.SetParameterValue("Date2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش گزارش هزینه های ارسال
            else if (_FormNumber == 8)
            {
                desplay();

                _06_Reports._01_Orders.Rpt09_SendCostReport rpt = new _01_Orders.Rpt09_SendCostReport();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Date1", _Param1);
                rpt.SetParameterValue("Date2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش دفتر فروش
            else if (_FormNumber == 9)
            {
                desplay();

                _06_Reports._02_Sale.Rpt_01_SaleDoc rpt = new _02_Sale.Rpt_01_SaleDoc();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.Subreports["Extra"].SetDataSource(_Table2);
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش فروش بر اساس مشتریان
            else if (_FormNumber == 10)
            {
                desplay();

                _06_Reports._02_Sale.Rpt03_Sale_Customer rpt = new _02_Sale.Rpt03_Sale_Customer();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش فروش بر اساس کالا
            else if (_FormNumber == 11)
            {
                desplay();

                _06_Reports._02_Sale.Rpt04_Sale_Goodsc rpt = new _02_Sale.Rpt04_Sale_Goodsc();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //آمار ماهیانه فروش تعدادی
            else if (_FormNumber == 12)
            {
                desplay();

                try
                {
                    _06_Reports._02_Sale.Rpt06_Numeric_Table rpt = new _02_Sale.Rpt06_Numeric_Table();
                    rpt.SetDataSource(_Table);
                    rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());

                    crystalReportViewer1.ReportSource = rpt;

                }
                catch
                {
                }
            }
            //نمایش آمار ماهیانه تعداد- جدول و نمودار
            else if (_FormNumber == 13)
            {
                desplay();

                _06_Reports._02_Sale.Rpt07_Numeric_Chart_Table rpt = new _02_Sale.Rpt07_Numeric_Chart_Table();
                rpt.SetDataSource(_Table);
                TextObject goodCode = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
                goodCode.Text = _Param1;
                TextObject goodname = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
                goodname.Text = _Param2;
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                crystalReportViewer1.ReportSource = rpt;
            }
            //آمار ماهیانه فروش ریالی
            else if (_FormNumber == 14)
            {
                desplay();

                _06_Reports._02_Sale.Rpt08_Price_Table rpt = new _02_Sale.Rpt08_Price_Table();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Currency", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش آمار ماهیانه ریالی- جدول و نمودار
            else if (_FormNumber == 15)
            {
                desplay();

                _06_Reports._02_Sale.Rpt09_Price_Chart_Table rpt = new _02_Sale.Rpt09_Price_Chart_Table();
                rpt.SetDataSource(_Table);
                TextObject goodCode = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
                goodCode.Text = _Param1;
                TextObject goodname = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
                goodname.Text = _Param2;
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                crystalReportViewer1.ReportSource = rpt;
            }
            //دفتر خرید
            else if (_FormNumber == 16)
            {
                desplay();

                _06_Reports._03_Buy.Rpt_01_BuyDoc rpt = new _03_Buy.Rpt_01_BuyDoc();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.Subreports["Extra"].SetDataSource(_Table2);
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش خرید بر اساس تأمین کنندگان
            else if (_FormNumber == 17)
            {
                desplay();

                _06_Reports._03_Buy.Rpt03_Buy_Customer rpt = new _03_Buy.Rpt03_Buy_Customer();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش خرید بر اساس کالا
            else if (_FormNumber == 18)
            {
                desplay();

                _06_Reports._03_Buy.Rpt04_Buy_Goodsc rpt = new _03_Buy.Rpt04_Buy_Goodsc();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                crystalReportViewer1.ReportSource = rpt;
            }
            //آمار ماهیانه خرید تعدادی
            else if (_FormNumber == 19)
            {
                desplay();

                _06_Reports._03_Buy.Rpt06_Buy_Numeric_Table rpt = new _03_Buy.Rpt06_Buy_Numeric_Table();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش آمار ماهیانه تعداد- جدول و نمودار
            else if (_FormNumber == 20)
            {
                desplay();

                _06_Reports._03_Buy.Rpt07_Buy_Numeric_Chart_Table rpt = new _03_Buy.Rpt07_Buy_Numeric_Chart_Table();
                rpt.SetDataSource(_Table);
                TextObject goodCode = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
                goodCode.Text = _Param1;
                TextObject goodname = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
                goodname.Text = _Param2;
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                crystalReportViewer1.ReportSource = rpt;
            }
            //آمار ماهیانه فروش ریالی
            else if (_FormNumber == 21)
            {
                desplay();

                _06_Reports._03_Buy.Rpt08_Buy_Price_Table rpt = new _03_Buy.Rpt08_Buy_Price_Table();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                crystalReportViewer1.ReportSource = rpt;
            }
            //نمایش آمار ماهیانه ریالی- جدول و نمودار
            else if (_FormNumber == 22)
            {
                desplay();

                _06_Reports._03_Buy.Rpt09_Buy_Price_Chart_Table rpt = new _03_Buy.Rpt09_Buy_Price_Chart_Table();
                rpt.SetDataSource(_Table);
                TextObject goodCode = (TextObject)rpt.ReportDefinition.ReportObjects["Text8"];
                goodCode.Text = _Param1;
                TextObject goodname = (TextObject)rpt.ReportDefinition.ReportObjects["Text9"];
                goodname.Text = _Param2;
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش جوایز تعلق گرفته
            else if (_FormNumber == 23)
            {
                desplay();

                _06_Reports._02_Sale.Rpt10_Sale_GiftGoods rpt = new _02_Sale.Rpt10_Sale_GiftGoods();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش مرجوعی فروش بر اساس مشتریان
            else if (_FormNumber == 24)
            {
                desplay();

                _06_Reports._02_Sale.Rpt11_ReturnSale_Customer rpt = new _02_Sale.Rpt11_ReturnSale_Customer();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش فاکتور فروش مرجوعی بر اساس کالا
            else if (_FormNumber == 25)
            {
                desplay();

                _06_Reports._02_Sale.Rpt12_ReturnSale_Goods rpt = new _02_Sale.Rpt12_ReturnSale_Goods();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش مرجوعی خرید بر اساس تأمین کنندگان
            else if (_FormNumber == 26)
            {
                desplay();

                _06_Reports._03_Buy.Rpt10_ReturnBuy_Customer rpt = new _03_Buy.Rpt10_ReturnBuy_Customer();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش فاکتور خرید مرجوعی بر اساس کالا
            else if (_FormNumber == 27)
            {
                desplay();

                _06_Reports._03_Buy.Rpt11_ReturnBuy_Goods rpt = new _03_Buy.Rpt11_ReturnBuy_Goods();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش سود ناویژه
            else if (_FormNumber == 28)
            {
                desplay();

                _06_Reports._02_Sale.Rpt13_Margin rpt = new _02_Sale.Rpt13_Margin();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", "از تاریخ:" + _Param1);
                rpt.SetParameterValue("Param2", "تا تاریخ:" + _Param2);
                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش اضافات و کسورات فروش
            else if (_FormNumber == 29)
            {
                desplay();

                _06_Reports._02_Sale.Rpt14_ExtraReductionList rpt = new _02_Sale.Rpt14_ExtraReductionList();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param3", _Param3);
                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش اضافات و کسورات فروش فاکتورهای مرجوعی
            else if (_FormNumber == 30)
            {
                desplay();

                _06_Reports._02_Sale.Rpt14_ExtraReductionList_Return rpt = new _02_Sale.Rpt14_ExtraReductionList_Return();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param3", _Param3);
                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش تخفیفات فاکتورهای فروش
            else if (_FormNumber == 31)
            {
                desplay();

                _06_Reports._02_Sale.Rpt15_DiscountsList rpt = new _02_Sale.Rpt15_DiscountsList();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش اضافات و کسورات خرید
            else if (_FormNumber == 32)
            {
                desplay();

                _06_Reports._03_Buy.Rpt12_ExtraReductionList rpt = new _03_Buy.Rpt12_ExtraReductionList();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param3", _Param3);
                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش اضافات و کسورات خرید فاکتورهای مرجوعی
            else if (_FormNumber == 33)
            {
                desplay();

                _06_Reports._03_Buy.Rpt12_ExtraReductionList_Return rpt = new _03_Buy.Rpt12_ExtraReductionList_Return();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param3", _Param3);
                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش فروش تعدادی ماهیانه بر اساس مشتری
            else if (_FormNumber == 34)
            {
                desplay();

                _06_Reports._02_Sale.Rpt16_Numeric_Customer rpt = new _02_Sale.Rpt16_Numeric_Customer();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param3", _Param3);
                crystalReportViewer1.ReportSource = rpt;
            }
            //آمار ماهیانه فروش ریالی
            else if (_FormNumber == 35)
            {
                desplay();

                _06_Reports._02_Sale.Rpt08_Price_Table_Customer rpt = new _02_Sale.Rpt08_Price_Table_Customer();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Currency", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
            //آمار ماهیانه فروش تعدادی مشتری-کل
            else if (_FormNumber == 36)
            {
                desplay();

                _06_Reports._02_Sale.Rpt16_Numeric_TotalCustomer rpt = new _02_Sale.Rpt16_Numeric_TotalCustomer();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                crystalReportViewer1.ReportSource = rpt;
            }
            //آمار فروش مشتری بر اساس محصول        
            else if (_FormNumber == 37)
            {
                desplay();

                _06_Reports._02_Sale.Rpt17_Sale_CustomerGood rpt = new _02_Sale.Rpt17_Sale_CustomerGood();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                crystalReportViewer1.ReportSource = rpt;
            }
            else if (_FormNumber == 38)
            {
                desplay();

                _06_Reports._02_Sale.Rpt18_Visitors rpt = new _02_Sale.Rpt18_Visitors();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش بستانکاران و بدهکاران بر اساس مسئول فروش
            else if (_FormNumber == 39)
            {
                desplay();

                _06_Reports._02_Sale.Rpt03_Visitors_PeopleInAccounts rpt = new _02_Sale.Rpt03_Visitors_PeopleInAccounts();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);

                crystalReportViewer1.ReportSource = rpt;
            }
            //آمار ماهیانه فروش تعدادی کالا بر اساس مسئول فروش
            else if (_FormNumber == 40)
            {
                desplay();

                _06_Reports._02_Sale.Rpt21_NumericMonthly_Goods_Visitor rpt = new _02_Sale.Rpt21_NumericMonthly_Goods_Visitor();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                crystalReportViewer1.ReportSource = rpt;
            }
            //آمار ماهیانه فروش ریالی کالا بر اساس مسئول فروش
            else if (_FormNumber == 41)
            {
                desplay();

                _06_Reports._02_Sale.Rpt22_Monthly_Value_Visitor_Good rpt = new _02_Sale.Rpt22_Monthly_Value_Visitor_Good();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
            //آمار فروش ماهیانه ریالی مشتری بر اساس ویزیتور
            else if (_FormNumber == 42)
            {
                desplay();

                _06_Reports._02_Sale.Rpt23_Monthly_Value_Visitor_Customer rpt = new _02_Sale.Rpt23_Monthly_Value_Visitor_Customer();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                crystalReportViewer1.ReportSource = rpt;
            }
            //آخرین قیمت خرید کالا
            else if (_FormNumber == 43)
            {
                desplay();

                _06_Reports._03_Buy.Rpt13_LastBuyPrice rpt = new _03_Buy.Rpt13_LastBuyPrice();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Date", _Param1);

                crystalReportViewer1.ReportSource = rpt;
            }
            //مرجوعی فروش بر اساس ویزیتورها
            else if (_FormNumber == 44)
            {
                desplay();

                _06_Reports._02_Sale.Rpt24_ReturnFactor_Visitors rpt = new _02_Sale.Rpt24_ReturnFactor_Visitors();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                crystalReportViewer1.ReportSource = rpt;
            }
            //گزارش جامع خرید- کالا
            else if (_FormNumber == 45)
            {
                crystalReportViewer1.Visible = false;
                stiViewerControl1.Visible = true;
                bar1.Visible = true;

                //_06_Reports._03_Buy.Rpt14_CompReport_Goods rpt = new _03_Buy.Rpt14_CompReport_Goods();
                //rpt.SetDataSource(_Table);
                //rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                //rpt.SetParameterValue("Param1", _Param1);
                //rpt.SetParameterValue("Param2", _Param2);
                //crystalReportViewer1.ReportSource = rpt;
                StiReport stireport = new StiReport();
                stireport.Load("Rpt14_CompReport_Goods.mrt");
                stireport.Pages["Page1"].Enabled = true;
                stireport.Compile();
                StiOptions.Viewer.AllowUseDragDrop = false;
                stireport.RegData("Rpt_BuyCompReport_Goods", _Table);
                stireport.RegData("Table_000_OrgInfo", Class_BasicOperation.LogoTable());
                stireport["Param1"] = _Param1;
                stireport["Param2"] = _Param2;
                stireport.IsSelected = true;
                stireport.Select();
                //stireport.Show();
                stireport.Render(false);
                stiViewerControl1.Report = stireport;
                stiViewerControl1.Refresh();

            }
            else if (_FormNumber == 46)
            {
                desplay();

                _06_Reports._02_Sale.Rpt25_GoodReportByVisitors rpt = new _02_Sale.Rpt25_GoodReportByVisitors();
                rpt.SetDataSource(_Table);
                rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                rpt.SetParameterValue("Param1", _Param1);
                rpt.SetParameterValue("Param2", _Param2);
                rpt.SetParameterValue("Param3", _Param3);
                crystalReportViewer1.ReportSource = rpt;
            }
            else if (_FormNumber == 47)
            {
                desplay();

                try
                {
                    _06_Reports._02_Sale.Rpt03_Sale_Customer2 rpt = new _02_Sale.Rpt03_Sale_Customer2();
                    rpt.SetDataSource(_Table);
                    rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                    rpt.SetParameterValue("Param1", _Param1);
                    rpt.SetParameterValue("Param2", _Param2);

                    crystalReportViewer1.ReportSource = rpt;
                }
                catch
                {
                }
            }

            else if (_FormNumber == 48)
            {
                desplay();

                try
                {
                    _06_Reports._02_Sale.Rpt26_Value_Visitor_Customer rpt = new _02_Sale.Rpt26_Value_Visitor_Customer();
                    rpt.SetDataSource(_Table);
                    rpt.Subreports[0].SetDataSource(Class_BasicOperation.LogoTable());
                    rpt.SetParameterValue("Param1", _Param1);
                    rpt.SetParameterValue("Parsam2", _Param2);

                    crystalReportViewer1.ReportSource = rpt;
                }
                catch
                {
                }
            }
        }

        private void Form01_ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Dispose();
        }

        private void btn_Design_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("Rpt14_CompReport_Goods.mrt");
            stireport.Design();
        }

        private void desplay()
        {
            stiViewerControl1.Visible = false;
            bar1.Visible = false;
            crystalReportViewer1.Visible = true;

        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            Form01_ReportForm_Load(null, null);
        }

    }
}
