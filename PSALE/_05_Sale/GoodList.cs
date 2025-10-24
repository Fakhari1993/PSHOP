using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PSHOP._05_Sale
{
    public partial class GoodList : Form
    {
        DataTable _goodlist = new DataTable();
        public GoodList()
        {
            InitializeComponent();
        }

        public GoodList(DataTable goodlist,string name)
        {
            InitializeComponent();
            _goodlist = goodlist;
            this.Text = name;
        }

        private void GoodList_Load(object sender, EventArgs e)
        {
            try
            {
                gridEX_SaleFactor.DataSource = _goodlist;
            }
            catch
            {
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    string j = " تاریخ تهیه گزارش:" + FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");
                    gridEXPrintDocument1.PageHeaderLeft = j;
                    gridEXPrintDocument1.PageHeaderRight = "گزارش کسر موجودی";
                    printPreviewDialog1.ShowDialog();
                }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_SaleFactor;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream File = (FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);

                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام شده", "Information");
            }
        }
    }
}
