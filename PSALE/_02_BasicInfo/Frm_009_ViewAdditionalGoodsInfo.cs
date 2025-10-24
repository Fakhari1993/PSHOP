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
    public partial class Frm_009_ViewAdditionalGoodsInfo : Form
    {
        bool _Del = false;
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);

        public Frm_009_ViewAdditionalGoodsInfo( )
        {
            InitializeComponent();
            
        }

        private void Frm_009_AdditionalGoodsInfo_Load(object sender, EventArgs e)
        {




            SqlDataAdapter Adapter = new SqlDataAdapter("Select Columnid,Column01,Column02 from table_002_MainGroup", ConWare);
            DataTable MainTable = new DataTable();
            Adapter.Fill(MainTable);
            gridEX_Goods.DropDowns["MainGroup"].SetDataBinding(MainTable, "");

            Adapter = new SqlDataAdapter("Select Columnid,Column01,Column02,Column03 from table_003_SubsidiaryGroup", ConWare);
            DataTable SubTable = new DataTable();
            Adapter.Fill(SubTable);
            gridEX_Goods.DropDowns["SubGroup"].SetDataBinding(SubTable, "");


            Adapter = new SqlDataAdapter(@"SELECT tcai.*,
                                                       tp.column02 as ware,
                                                       ISNULL(kj.Remain, 0) AS Remain,
                                                       ISNULL(kj.RealRemain, 0) AS RealRemain,
                                                       ISNULL(kj.RealRemain, 0)/NULLIF(tcai.column08,0) as RemainBaste,
                                                       ISNULL(kj.RealRemain, 0)/NULLIF(tcai.column09,0) as RemainKarton,
                                                        po.column02 as MainGroup,
                                                        po1.column03 as SubGroup
                                                FROM   table_004_CommodityAndIngredients tcai
                                                       LEFT JOIN (
                                                                SELECT ware,
                                                                       SUM(Resid) -SUM(Draft)  -SUM(isnull(NotDelivary,0)) AS 
                                                                       Remain,
                                                                       SUM(Resid) -SUM(Draft)  AS RealRemain,
                                                                       GoodID
                                                                FROM   (
                                                                           SELECT Table_008_Child_PwhrsDraft.column02 AS GoodID,
                                                                                  Table_007_PwhrsDraft.column03 AS ware,
                                                                                  0.000 AS Resid,
                                                                                  SUM(Table_008_Child_PwhrsDraft.column07) AS 
                                                                                  Draft,
                                                                                  0.000 AS Delivary,
                                                                                  0.000 AS NotDelivary
                                                                           FROM   Table_008_Child_PwhrsDraft
                                                                                  INNER JOIN Table_007_PwhrsDraft
                                                                                       ON  Table_007_PwhrsDraft.columnid = 
                                                                                           Table_008_Child_PwhrsDraft.column01
                                                                           GROUP BY
                                                                                  Table_008_Child_PwhrsDraft.column02,
                                                                                  Table_007_PwhrsDraft.column03,
                                                                                  Table_008_Child_PwhrsDraft.column02 
                           
                                                                           UNION ALL
                           
                                                                           SELECT Table_012_Child_PwhrsReceipt.column02 AS 
                                                                                  GoodID,
                                                                                  Table_011_PwhrsReceipt.column03 AS ware,
                                                                                  SUM(Table_012_Child_PwhrsReceipt.column07) AS 
                                                                                  Resid,
                                                                                  0.000 AS Draft,
                                                                                  0.000 AS Delivary,
                                                                                  0.000 AS NotDelivary
                                                                           FROM   Table_012_Child_PwhrsReceipt
                                                                                  INNER JOIN Table_011_PwhrsReceipt
                                                                                       ON  Table_011_PwhrsReceipt.columnid = 
                                                                                           Table_012_Child_PwhrsReceipt.column01
                                                                           GROUP BY
                                                                                  Table_012_Child_PwhrsReceipt.column02,
                                                                                  Table_011_PwhrsReceipt.column03,
                                                                                  Table_012_Child_PwhrsReceipt.column02 
                           
                                                                           UNION ALL
                                                                           SELECT tcsf.column02 AS GoodID,
                                                                                  tsf.Column42 AS ware,
                                                                                  0 AS Resid,
                                                                                  0.000 AS Draft,
                                                                                  SUM(ISNULL(tcsf.column07, 0)) AS Delivary,
                                                                                  0.000 AS NotDelivary
                                                                           FROM   " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor 
                                                                                  tcsf
                                                                                  JOIN " + ConSale.Database + @".dbo.Table_010_SaleFactor 
                                                                                       tsf
                                                                                       ON  tsf.columnid = tcsf.column01
                                                                           WHERE  tsf.Column17 = 0
                                                                                  AND tsf.Column19 = 0
                                                                                  AND tsf.column09 > 0  --تحويل شده پس حواله دارد
                                                                                  
                                                                           GROUP BY
                                                                                  tcsf.column02,
                                                                                  tsf.Column42,
                                                                                  tcsf.column02 
                                                                           UNION ALL
                                                                           SELECT tcsf.column02 AS GoodID,
                                                                                  tsf.Column42 AS ware,
                                                                                  0 AS Resid,
                                                                                  0.000 AS Draft,
                                                                                  0 AS Delivary,
                                                                                  SUM(ISNULL(tcsf.column07, 0)) AS NotDelivary
                                                                           FROM   " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor 
                                                                                  tcsf
                                                                                  JOIN " + ConSale.Database + @".dbo.Table_010_SaleFactor 
                                                                                       tsf
                                                                                       ON  tsf.columnid = tcsf.column01
                                                                           WHERE  tsf.Column17 = 0
                                                                                  AND tsf.Column19 = 0
                                                                                  AND tsf.column09 = 0--تحويل نشده پس حواله ندارد
                                                                           GROUP BY
                                                                                  tcsf.column02,
                                                                                  tsf.Column42,
                                                                                  tcsf.column02
                                                                       ) AS tbl
                                                                GROUP BY
                                                                       ware,
                                                                       GoodID
                                                            ) AS kj
                                                            ON  kj.GoodID = tcai.columnid  
                                                                left JOIN Table_001_PWHRS tp ON tp.columnid= kj.ware  
                                                                left join  table_002_MainGroup po on po.columnid=tcai.column03
                                                                left join  table_003_SubsidiaryGroup po1 on po1.columnid=tcai.column04
                                                                ", ConWare);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            gridEX_Goods.DataSource = Table;


            gridEX_Goods.MoveFirst();

        }



        private void Frm_009_AdditionalGoodsInfo_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Control && e.KeyCode == Keys.F)
            {
                gridEX_Goods.Select();
                gridEX_Goods.Row = gridEX_Goods.FilterRow.Position;
            }
        }




        private void gridEX_Riali_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }





        private void mnu_Stock_Current_Numeric_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;

            Class_UserScope UserScope = new Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 31))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_003_MojoodiMaghtaiTedadi")
                    {
                        child.Focus();
                        return;
                    }
                }

                PWHRS._05_Gozareshat.Frm_003_MojoodiMaghtaiTedadi ob = new PWHRS._05_Gozareshat.Frm_003_MojoodiMaghtaiTedadi();

                try
                {
                    ob.MdiParent = this.MdiParent;
                }
                catch { }
                ob.Show();

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_Stock_Current_Riali_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            Class_UserScope UserScope = new Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 32))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_005_MojoodiMaghtaiRiyali")
                    {
                        child.Focus();
                        return;
                    }
                }

                PWHRS._05_Gozareshat.Frm_005_MojoodiMaghtaiRiyali ob = new PWHRS._05_Gozareshat.Frm_005_MojoodiMaghtaiRiyali();

                try
                {
                    ob.MdiParent = this.MdiParent;
                }
                catch { }
                ob.Show();

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_Stock_Periodic_Numeric_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            Class_UserScope UserScope = new Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 33))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_004_MojoodiDoreyiTedadi")
                    {
                        child.Focus();
                        return;
                    }
                }

                PWHRS._05_Gozareshat.Frm_004_MojoodiDoreyiTedadi ob = new PWHRS._05_Gozareshat.Frm_004_MojoodiDoreyiTedadi();

                try
                {
                    ob.MdiParent = this.MdiParent;
                }
                catch { }
                ob.Show();

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_Stock_Periodic_Riali_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            Class_UserScope UserScope = new Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 34))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_006_MojoodiDoreyiRiyali")
                    {
                        child.Focus();
                        return;
                    }
                }

                PWHRS._05_Gozareshat.Frm_006_MojoodiDoreyiRiyali ob = new PWHRS._05_Gozareshat.Frm_006_MojoodiDoreyiRiyali();

                try
                {
                    ob.MdiParent = this.MdiParent;
                }
                catch { }
                ob.Show();

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_NumericCardex_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            Class_UserScope UserScope = new Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 37))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_009_KardexTedadi")
                    {
                        child.Focus();
                        return;
                    }
                }

                PWHRS._05_Gozareshat.Frm_009_KardexTedadi ob = new PWHRS._05_Gozareshat.Frm_009_KardexTedadi();

                try
                {
                    ob.MdiParent = this.MdiParent;
                }
                catch { }
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_RialiCardex_Click(object sender, EventArgs e)
        {

            Class_UserScope UserScope = new Class_UserScope();

            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 38))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_010_KardexRiyali")
                    {
                        child.Focus();
                        return;
                    }
                }
                PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
                PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
                PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PWHRS._05_Gozareshat.Frm_010_KardexRiyali ob = new PWHRS._05_Gozareshat.Frm_010_KardexRiyali();

                try
                {
                    ob.MdiParent = this.MdiParent;
                }
                catch { }
                ob.Show();

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_DefineGoods_Click(object sender, EventArgs e)
        {
            Class_UserScope us = new Class_UserScope();
            if (us.CheckScope(Class_BasicOperation._UserName, "Column11", 57))
            {
                PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
                PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
                PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PWHRS._02_EtelaatePaye.Frm_001_DefineGoods ob = new PWHRS._02_EtelaatePaye.Frm_001_DefineGoods(us.CheckScope(Class_BasicOperation._UserName, "Column10", 6));
                ob.ShowDialog();
               
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }

        }

        private void bt_ExportToExcel_Top_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Goods;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void Frm_009_ViewAdditionalGoodsInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Goods.RemoveFilters();

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            try
            {
                gridEXPrintDocument1.GridEX = gridEX_Goods;
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                    if (printDialog1.ShowDialog() == DialogResult.OK)
                    {
                       
                        gridEXPrintDocument1.PageHeaderRight = "لیست کالا";
                        printPreviewDialog1.ShowDialog();
                    }
            }
            catch { }
        }

        private void چاپگزارشToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridEX_Goods.GetDataRows().Count() > 0)
            {
                _06_Reports.Form01_StimulateReportForm frm = new _06_Reports.Form01_StimulateReportForm((DataTable) gridEX_Goods.DataSource, 2, "", "", "");
                frm.ShowDialog();
            }
        }




    }
}
