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
    public partial class Frm_009_AdditionalGoodsInfo : Form
    {
        bool _Del = false;
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);

        public Frm_009_AdditionalGoodsInfo(bool Del)
        {
            InitializeComponent();
            _Del = Del;
        }

        private void Frm_009_AdditionalGoodsInfo_Load(object sender, EventArgs e)
        {
          
                this.table_004_CommodityAndIngredientsTableAdapter.Fill(this.dataSet_EtelaatPaye.table_004_CommodityAndIngredients);
                this.table_003_InformationProductCashTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_003_InformationProductCash);
                

                SqlDataAdapter Adapter = new SqlDataAdapter("Select Columnid,Column01,Column02 from table_002_MainGroup", ConWare);
                DataTable MainTable = new DataTable();
                Adapter.Fill(MainTable);
                gridEX_Goods.DropDowns["MainGroup"].SetDataBinding(MainTable, "");

                Adapter = new SqlDataAdapter("Select Columnid,Column01,Column02,Column03 from table_003_SubsidiaryGroup", ConWare);
                DataTable SubTable = new DataTable();
                Adapter.Fill(SubTable);
                gridEX_Goods.DropDowns["SubGroup"].SetDataBinding(SubTable, "");

                gridEX_Goods.MoveFirst();
       
        }


        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX_Riali.UpdateData();
                this.table_004_CommodityAndIngredientsBindingSource.EndEdit();
                this.table_003_InformationProductCashBindingSource.EndEdit();
                this.table_004_CommodityAndIngredientsTableAdapter.Update(dataSet_EtelaatPaye.table_004_CommodityAndIngredients);
                this.table_003_InformationProductCashTableAdapter.Update(dataSet_EtelaatPaye.Table_003_InformationProductCash);
                Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "Information");
            }
            catch (SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, this.Name);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void Frm_009_AdditionalGoodsInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
            {
                gridEX_Goods.Select();
                gridEX_Goods.Row = gridEX_Goods.FilterRow.Position;
            }
        }

        private void gridEX_Riali_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (_Del)
            {
                if (this.table_003_InformationProductCashBindingSource.Count > 0)
                {
                    if (DialogResult.No == MessageBox.Show("آیا مایل به حذف شناسه ریالی مورد نظر هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                e.Cancel = true;

            }
        }


        private void gridEX_Riali_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_Riali_RecordsDeleted(object sender, EventArgs e)
        {
            try
            {
                table_003_InformationProductCashBindingSource.EndEdit();
                table_003_InformationProductCashTableAdapter.Update(dataSet_EtelaatPaye.Table_003_InformationProductCash);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

     
        private void gridEX_Riali_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
                if (gridEX_Riali.GetRow().Cells["column11"].Value.ToString() == "True")
                {
                    foreach (DataRowView item in this.table_003_InformationProductCashBindingSource)
                    {
                        if (item["ColumnId"].ToString() != gridEX_Riali.GetRow().Cells["columnid"].Text.Trim())
                            item["Column11"] = false;
                    }
                    this.table_003_InformationProductCashBindingSource.EndEdit();
                }
            }
            catch { }
        }

        private void gridEX_Riali_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {
            if (e.Column.Key == "Column11" && gridEX_Riali.Row != -1)
            {
                if (e.Value.ToString() == "True")
                {
                    foreach (DataRowView item in this.table_003_InformationProductCashBindingSource)
                    {
                        if (item["ColumnId"].ToString() != gridEX_Riali.GetRow().Cells["columnid"].Text.Trim())
                            item["Column11"] = false;
                    }
                    this.table_003_InformationProductCashBindingSource.EndEdit();
                }
            }
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
                dataSet_EtelaatPaye.EnforceConstraints = false;
                this.table_004_CommodityAndIngredientsTableAdapter.Fill(this.dataSet_EtelaatPaye.table_004_CommodityAndIngredients);
                this.table_003_InformationProductCashTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_003_InformationProductCash);
                dataSet_EtelaatPaye.EnforceConstraints = true;
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

        private void bt_ExportToExcel_Bottom_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX_Riali;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }

        }

        private void Frm_009_AdditionalGoodsInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Goods.RemoveFilters();
            gridEX_Riali.RemoveFilters();
        }

        
    }
}
