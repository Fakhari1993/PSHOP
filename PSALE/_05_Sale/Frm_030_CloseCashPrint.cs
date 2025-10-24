using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Design;
using Stimulsoft.Database;
namespace PSHOP._05_Sale
{
    public partial class Frm_030_CloseCashPrint : Form
    {
        string date = string.Empty;
        DataTable CloseCash = new DataTable();
        DataTable ReceivedFromCustomers = new DataTable();
        DataTable ReceivedFromBank = new DataTable();
        DataTable LosesFromCash = new DataTable();
        string Numsanad = "";
        public Frm_030_CloseCashPrint(string _date, DataTable _CloseCash, DataTable _ReceivedFromCustomers,
              DataTable _ReceivedFromBank, DataTable _LosesFromCash,string _Numsanad)
        {
            InitializeComponent();
            date = _date;
            CloseCash = _CloseCash;
            ReceivedFromCustomers = _ReceivedFromCustomers;
            ReceivedFromBank = _ReceivedFromBank;
            LosesFromCash = _LosesFromCash;
            Numsanad = _Numsanad;
            buttonX1_Click(null, null);
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("CloseCash.mrt");
            stireport.Design();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;


            StiReport stireport = new StiReport();
            stireport.Load("CloseCash.mrt");


            stireport.Compile();
            StiOptions.Viewer.AllowUseDragDrop = false;


            stireport.RegData("Table_96_CloseCash", CloseCash);
            stireport.RegData("Table_97_ReceivedFromCustomers", ReceivedFromCustomers);
            stireport.RegData("Table_98_ReceivedFromBank", ReceivedFromBank);
            stireport.RegData("Table_99_LosesFromCash", LosesFromCash);
            stireport["Numsanad"] = Numsanad;


            //if (!chk_Logo.Checked)
            //{
            //    stireport.RegData("Table_000_OrgInfo", Org);
            //    stireport["P1Tel"] = Org.Rows[0]["Column03"].ToString();
            //    stireport["P1Name"] = Org.Rows[0]["Column01"].ToString();
            //    stireport["P1ECode"] = Org.Rows[0]["Column06"].ToString();
            //    stireport["P1NCode"] = Org.Rows[0]["Column07"].ToString();
            //    stireport["P1Address"] = Org.Rows[0]["Column02"].ToString();
            //    stireport["P1PostalCode"] = Org.Rows[0]["Column14"].ToString();
            //}
            //else
            //{
            //    stireport.RegData("Table_000_OrgInfo", Org.Clone());
            //    stireport["P1Tel"] = "";
            //    stireport["P1Name"] = "";
            //    stireport["P1ECode"] = "";
            //    stireport["P1NCode"] = "";
            //    stireport["P1Address"] = "";
            //    stireport["P1PostalCode"] = "";
            //}

            //stireport.RegData("FN_01_SettleInfo", TotalSettleInfo);






            //stireport["Param3"] = Sign[0];
            //stireport["Param4"] = Sign[1];
            //stireport["Param5"] = Sign[2];
            //stireport["Param6"] = Sign[3];
            //stireport["Param7"] = Sign[4];
            //stireport["Param8"] = Sign[5];
            //stireport["Param9"] = Sign[6];
            //stireport["Param10"] = Sign[7];
            //stireport["ShowSettleInfo"] = chk_ShowCustomerBill.Checked;
            //stireport["ShowSentence"] = chk_ShowSen.Checked;
            //stireport["NotShowDate"] = chk_ShowDate.Checked;
            //stireport["ShowEcoCode"] = ChkEcoCode.Checked;

            this.Cursor = Cursors.Default;
            stireport.Select();

            //stireport.Show();
            stireport.Render(false);
            stiViewerControl1.Report = stireport;
            stiViewerControl1.Refresh();
        }

        private void Frm_030_CloseCashPrint_Load(object sender, EventArgs e)
        {

        }
    }
}
