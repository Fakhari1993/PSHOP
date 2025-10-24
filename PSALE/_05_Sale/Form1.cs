using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace PSHOP._05_Sale
{
    public partial class Form1 : Form
    {
        SqlDataAdapter sda;
        SqlConnection constr = new SqlConnection(
            Properties.Settings.Default.WHRS);
        DataTable dt;
        string cmd,_kalacode;
        decimal _remain = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                cmd = @"SELECT Table_011_PwhrsReceipt.column02 AS turndate,
                           Table_011_PwhrsReceipt.column01 AS no,
                           Table_012_Child_PwhrsReceipt.column02 AS kalaid,
                           table_004_CommodityAndIngredients.column01 AS kalacode,
                           table_004_CommodityAndIngredients.column02 AS kalaname,
                           Table_012_Child_PwhrsReceipt.column07 AS recqty,
                           0 AS draftqty,
                           0 AS remain,
                           Table_011_PwhrsReceipt.column03 AS ware,
                           tp.column02 AS warename
                    FROM   Table_011_PwhrsReceipt
                           INNER JOIN Table_012_Child_PwhrsReceipt
                                ON  Table_011_PwhrsReceipt.columnid = Table_012_Child_PwhrsReceipt.column01
                           INNER JOIN table_004_CommodityAndIngredients
                                ON  Table_012_Child_PwhrsReceipt.column02 = 
                                    table_004_CommodityAndIngredients.columnid
                           JOIN Table_001_PWHRS tp
                                ON  tp.columnid = Table_011_PwhrsReceipt.column03
                    UNION ALL
                    SELECT Table_007_PwhrsDraft.column02 AS turndate,
                           Table_007_PwhrsDraft.column01 AS no,
                           Table_008_Child_PwhrsDraft.column02 AS kalaid,
                           table_004_CommodityAndIngredients_1.column01 AS kalacode,
                           table_004_CommodityAndIngredients_1.column02 AS kalaname,
                           0 AS recqty,
                           Table_008_Child_PwhrsDraft.column07 AS draftqty,
                           0 AS remain,
                           Table_007_PwhrsDraft.column03 AS ware,
                           tp.column02 AS warename
                    FROM   Table_007_PwhrsDraft
                           INNER JOIN Table_008_Child_PwhrsDraft
                                ON  Table_007_PwhrsDraft.columnid = Table_008_Child_PwhrsDraft.column01
                           INNER JOIN table_004_CommodityAndIngredients AS 
                                table_004_CommodityAndIngredients_1
                                ON  Table_008_Child_PwhrsDraft.column02 = 
                                    table_004_CommodityAndIngredients_1.columnid
                           JOIN Table_001_PWHRS tp
                                ON  tp.columnid = Table_007_PwhrsDraft.column03
                    ORDER BY
                           ware,
                           kalaid,
                           turndate,
                           recqty DESC";
                sda = new SqlDataAdapter(cmd, constr);
                dt = new DataTable();
                sda.Fill(dt);


                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = dt.Rows.Count;

                progressBar1.Step = 1;

                _kalacode = dt.Rows[0]["kalacode"].ToString();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    progressBar1.PerformStep();

                    if (_kalacode != dt.Rows[i]["kalacode"].ToString())
                        _remain = 0;

                    _remain +=
                    Convert.ToDecimal(dt.Rows[i]["recqty"]) -
                    Convert.ToDecimal(dt.Rows[i]["draftqty"]);

                    dt.Rows[i]["remain"] = _remain;

                    _kalacode = dt.Rows[i]["kalacode"].ToString();
                }

                progressBar1.Visible = false;
                gridEX1.DataSource = dt;
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream File = (FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);

                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام شده", "Information");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    string j = " تاریخ تهیه گزارش:" + FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");
                    gridEXPrintDocument1.PageHeaderLeft = j;
                    gridEXPrintDocument1.PageHeaderRight = "کاردکس کالا";
                    printPreviewDialog1.ShowDialog();
                }
        }
    }
}
