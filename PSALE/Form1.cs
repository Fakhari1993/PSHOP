using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VPCPOS;

namespace PSHOP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
//            clsCommunication communication = new clsCommunication();

//communication.ConnType = 1;
// communication.IPAddress = config.PosIpAddress;
// communication.IPPort = config.Port;


//clsMessage transaction = new clsMessage();
// transaction.CancelTimeout = 60;//in second

//clsrequests clsrequests = new clsrequests();
// clsrequests.msgTyp = clsMessage.MsgType.Sale;
// clsrequests.bankId = 1;
// clsrequests.POSSerialEnable = true;

// clsrequests.terminalID = config.TerminalId.ToString();
// clsrequests.amount = paymentRequest.Amount.ToString();
//clsrequests._printStr="فروشگاه ارمغان";
//clsrequests.MultiAccEnable = false;

//int transactionResult = transaction.SendMessage(1);
//int retResponse = int.Parse(transactionResult.ToString());
//string result = string.Empty;

//switch (retResponse)
// {
// case 0:
//result="دریافت با موفقیت انجام شد" ;
// break;
// case 1:
//  result="خطا در ارتباط";
// break;
// case 2:
//  result="خطا در دریافت اطلاعات";
// break;
// case 3:
//  result = "خطا در ارسال اطلاعات";
// break;
// case 4:
// result="خطا در تراکنش بازگشتی" ;
// break;
// case 5:
//result="لغو توسط کاربر" ;
// break;
// case 6:
//result="خط ارتباطی مشغول است" ;
// break;
// case 7:
//result="پاسخ دریافت نشد"  ;
// break;
// case 8:
//result="خطا در ابطال"  ;
// break;
// case 9:
//result ="خطا در خط تلفن "  ;
// break;
// case 10:
//result="خطای داخلی" ;
// break;
// case 11:
//result="خطای زمان انجام تراکنش" ;
// break;
// case 12:
//result="خطا در خواندن اطلاعات کارت" ;
// break;
// case 50:
//result="خطا در داده های ترمینال" ;
// break;
// case 51:
//result="دستگاه آماده انجام تراکنش نیست" ;
// break;
// case 98:
//result="خطای سوئیچ" ;
// break;
// case 99:
//result="خطای غیر متبط" ;
// break;
// default:
//result="خطا" ;
// break;
// }




 //}
        }
    }
}
