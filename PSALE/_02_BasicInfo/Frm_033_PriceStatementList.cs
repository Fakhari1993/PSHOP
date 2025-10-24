using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._02_BasicInfo
{
    public partial class Frm_033_PriceStatementList : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBASE = new SqlConnection(Properties.Settings.Default.BASE);

        public Frm_033_PriceStatementList()
        {
            InitializeComponent();
        }

        private void Frm_033_PriceStatementList_Load(object sender, EventArgs e)
        {

            if (Convert.ToInt32(Properties.Settings.Default.FactorPrice) == 0)
            {
                MessageBox.Show("نحوه محاسبه قیمت در فاکتور فروش را براساس اعلامیه قیمت انتخاب کنید");


            }
            gridEX1.DataSource = clDoc.ReturnTable(Properties.Settings.Default.SALE, @"SELECT tps.Column00 AS num,
                                                                                           tps.Column01 AS [date],
                                                                                           tps.Column02 AS [title],
                                                                                           tc.Column01 AS [gride],
                                                                                           tst.column02 AS [saletype],
                                                                                           tps.Column05 AS [hederdesc],
                                                                                           tps.Column06 AS hederinsetuser,
                                                                                           tps.Column07 AS hederinserttime,
                                                                                           tps.Column08 AS hederedituser,
                                                                                           tps.Column09 AS hederedittime,
                                                                                          tcai.column02 AS goodname,
                                                                                          tcai.column01 AS goodcode,
                                                                                          tpsc.Column02 AS cartonfee,
                                                                                          tpsc.Column03 AS packfee,
                                                                                          tpsc.Column04 AS jozfee,
                                                                                          tps.Column05 AS childdesc,
                                                                                          tpsc.Column06 AS childinsertuser,
                                                                                          tpsc.Column07 AS childinsertdate,
                                                                                          tpsc.Column08 AS childedituser,
                                                                                          tpsc.Column09 AS chilfeditdate
                                                                                    FROM   Table_82_PriceStatement tps
                                                                                           JOIN Table_83_PriceStatementChild tpsc
                                                                                                ON  tpsc.Column00 = tps.ColumnId
                                                                                           JOIN Table_000_Class tc
                                                                                                ON  tc.ColumnId = tps.Column03
                                                                                           JOIN "+ConBASE.Database+@".dbo.Table_002_SalesTypes tst
                                                                                                ON  tst.columnid = tps.Column04
                                                                                           JOIN "+ConWare.Database+@".dbo.table_004_CommodityAndIngredients tcai
                                                                                                ON  tcai.columnid=tpsc.Column01");

        }

        private void Frm_033_PriceStatementList_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            gridEX1.DataSource = clDoc.ReturnTable(Properties.Settings.Default.SALE, @"SELECT tps.Column00 AS num,
                                                                                           tps.Column01 AS [date],
                                                                                           tps.Column02 AS [title],
                                                                                           tc.Column01 AS [gride],
                                                                                           tst.column02 AS [saletype],
                                                                                           tps.Column05 AS [hederdesc],
                                                                                           tps.Column06 AS hederinsetuser,
                                                                                           tps.Column07 AS hederinserttime,
                                                                                           tps.Column08 AS hederedituser,
                                                                                           tps.Column09 AS hederedittime,
                                                                                          tcai.column02 AS goodname,
                                                                                          tcai.column01 AS goodcode,
                                                                                          tpsc.Column02 AS cartonfee,
                                                                                          tpsc.Column03 AS packfee,
                                                                                          tpsc.Column04 AS jozfee,
                                                                                          tps.Column05 AS childdesc,
                                                                                          tpsc.Column06 AS childinsertuser,
                                                                                          tpsc.Column07 AS childinsertdate,
                                                                                          tpsc.Column08 AS childedituser,
                                                                                          tpsc.Column09 AS chilfeditdate
                                                                                    FROM   Table_82_PriceStatement tps
                                                                                           JOIN Table_83_PriceStatementChild tpsc
                                                                                                ON  tpsc.Column00 = tps.ColumnId
                                                                                           JOIN Table_000_Class tc
                                                                                                ON  tc.ColumnId = tps.Column03
                                                                                           JOIN " + ConBASE.Database + @".dbo.Table_002_SalesTypes tst
                                                                                                ON  tst.columnid = tps.Column04
                                                                                           JOIN " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients tcai
                                                                                                ON  tcai.columnid=tpsc.Column01");
        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void cmb_Print_Click(object sender, EventArgs e)
        {
            try
            {
                gridEXPrintDocument1.GridEX = gridEX1;
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                        
                        
                        gridEXPrintDocument1.PageHeaderRight = "لیست اعلامیه قیمت ها";
                        printPreviewDialog1.ShowDialog();
                        gridEXPrintDocument1.PageFooterLeft =
                      FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd") +
                      "**" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                        gridEXPrintDocument1.PageFooterRight =
                          " کاربر " + Class_BasicOperation._UserName;
                    }
            }
            catch { }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }
    }
}
