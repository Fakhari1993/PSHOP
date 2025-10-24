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

    public partial class Frm_008_PersonSaleType : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        public Frm_008_PersonSaleType()
        {
            InitializeComponent();
        }

        private void Frm_008_PersonSaleType_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_05_Awards.Table_045_PersonInfo' table. You can move, or remove it, as needed.
            DataTable PersonGroup = clDoc.ReturnTable(ConBase.ConnectionString, @"Select * from(
            Select distinct Tbl2.PersonId, 
            substring((Select ','+Tbl1.GroupName   AS [text()]
            From (SELECT     dbo.Table_045_PersonInfo.ColumnId AS PersonId, ISNULL(dbo.Table_040_PersonGroups.Column01, 'عمومی') AS GroupName
            FROM         dbo.Table_040_PersonGroups INNER JOIN
            dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 RIGHT OUTER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId) Tbl1
            Where Tbl1.PersonId = Tbl2.PersonId
              
            For XML PATH ('')),2, 1000) [PersonGroup]
            From (SELECT     dbo.Table_045_PersonInfo.ColumnId AS PersonId, ISNULL(dbo.Table_040_PersonGroups.Column01, 'عمومی') AS GroupName
            FROM         dbo.Table_040_PersonGroups INNER JOIN
            dbo.Table_045_PersonScope ON dbo.Table_040_PersonGroups.Column00 = dbo.Table_045_PersonScope.Column02 RIGHT OUTER JOIN
            dbo.Table_045_PersonInfo ON dbo.Table_045_PersonScope.Column01 = dbo.Table_045_PersonInfo.ColumnId) Tbl2) as PersonGroup");

            gridEX_Goods.DropDowns["PersonGroup"].SetDataBinding(PersonGroup, "");

            gridEX_Goods.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_002_SalesTypes"), "");
            mlt_ACC.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_002_SalesTypes");
            this.table_045_PersonInfoTableAdapter.Fill(this.dataSet_05_Awards.Table_045_PersonInfo);

        }

        private void Frm_008_PersonSaleType_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Goods.RemoveFilters();
        }

        private void bt_Apply_Click(object sender, EventArgs e)
        {
            if (gridEX_Goods.GetCheckedRows().Length > 0 && mlt_ACC.Text.Trim() != "")
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Goods.GetCheckedRows())
                {
                    item.BeginEdit();
                    item.Cells["Column30"].Value = mlt_ACC.Value;
                    item.EndEdit();
                }
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            gridEX_Goods.RemoveFilters();
            gridEX_Goods.UpdateData();
            try
            {
                table_045_PersonInfoBindingSource.EndEdit();
                this.table_045_PersonInfoTableAdapter.Update(this.dataSet_05_Awards.Table_045_PersonInfo);
                Class_BasicOperation.ShowMsg("", " ثبت با موفقیت انجام شد", "Information");
                this.table_045_PersonInfoTableAdapter.Fill(this.dataSet_05_Awards.Table_045_PersonInfo);


            }
            catch (SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, this.Name);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void bt_Refresh_Click(object sender, EventArgs e)
        {
            this.table_045_PersonInfoTableAdapter.Fill(this.dataSet_05_Awards.Table_045_PersonInfo);

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void Frm_008_PersonSaleType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.R && e.Control)
                bt_Refresh_Click(sender, e);
        }
    }
}
