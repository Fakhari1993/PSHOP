using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSALE.Buy
{
    public partial class Frm_002_MoshahedeTaeed_DarkhastKharid : Form
    {
        bool _del;
        db alldatabase = new db();
        Class_UserScope UserScope = new Class_UserScope();
        public Frm_002_MoshahedeTaeed_DarkhastKharid(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        private void Frm_002_MoshahedeTaeed_DarkhastKharid_Load(object sender, EventArgs e)
        {   string s = "";
            db.constr = Properties.Settings.Default.PSALE_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            s = @"SELECT        PERP_Base.dbo.Table_045_PersonInfo.*
                FROM            dbo.GetListPepleForOneSystemAndOneGroup(8,4) AS GetListPepleForOneSystemAndOneGroup_1 INNER JOIN
                         PERP_Base.dbo.Table_045_PersonInfo ON GetListPepleForOneSystemAndOneGroup_1.ColumnId = PERP_Base.dbo.Table_045_PersonInfo.ColumnId";

            gridEX2.DropDowns["d"].SetDataBinding(alldatabase.get_list(s), "");

            table_013_RequestBuyTableAdapter.Fill(dataSet_Buy.Table_013_RequestBuy);
            table_014_Child_RequestBuyTableAdapter.Fill(dataSet_Buy.Table_014_Child_RequestBuy);

            
            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();

            s = @"SELECT     dbo.table_004_CommodityAndIngredients.columnid AS id, dbo.table_004_CommodityAndIngredients.column01 AS code, 
                      dbo.table_004_CommodityAndIngredients.column02 AS name, dbo.table_002_MainGroup.column02 AS goroohasli, 
                      dbo.table_003_SubsidiaryGroup.column03 AS goroohfari, dbo.table_004_CommodityAndIngredients.column09 AS tedad_dar_kartoon, 
                      dbo.table_004_CommodityAndIngredients.column08 AS tedad_dar_baste, 
                        dbo.table_004_CommodityAndIngredients.column07 AS vahedshomareshid,
                      {0}.dbo.Table_004_ProductAdditionalInformation.column09 AS gheymat_kartoon, 
                      {0}.dbo.Table_004_ProductAdditionalInformation.column08 AS gheymat_baste, 
                      {0}.dbo.Table_004_ProductAdditionalInformation.column02 AS gheymat_vahed, 
                      {0}.dbo.Table_004_ProductAdditionalInformation.column05 AS takhfif, 
                      {0}.dbo.Table_004_ProductAdditionalInformation.column06 AS ezafe
                      FROM         dbo.table_004_CommodityAndIngredients INNER JOIN
                      dbo.table_002_MainGroup ON dbo.table_004_CommodityAndIngredients.column03 = dbo.table_002_MainGroup.columnid INNER JOIN
                      dbo.table_003_SubsidiaryGroup ON dbo.table_004_CommodityAndIngredients.column04 = dbo.table_003_SubsidiaryGroup.columnid INNER JOIN
                      {0}.dbo.Table_004_ProductAdditionalInformation ON 
                      dbo.table_004_CommodityAndIngredients.columnid = {0}.dbo.Table_004_ProductAdditionalInformation.column01
                      WHERE dbo.table_004_CommodityAndIngredients.column28=1";



            s = string.Format(s, table_014_Child_RequestBuyTableAdapter.Connection.Database.ToString());
            gridEX1.DropDowns["d"].SetDataBinding(alldatabase.get_list(s), "");
        }

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                table_013_RequestBuyBindingSource.EndEdit();
                table_013_RequestBuyBindingSource.MoveLast();


            }
            catch(Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex,"");
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                table_013_RequestBuyBindingSource.EndEdit();
                table_013_RequestBuyBindingSource.MoveNext(); 


            }
            catch(Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex,"");
            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                table_013_RequestBuyBindingSource.EndEdit();
                table_013_RequestBuyBindingSource.MovePrevious(); 


            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                table_013_RequestBuyBindingSource.EndEdit();
                table_013_RequestBuyBindingSource.MoveFirst(); 


            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, "");
            }
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            if (table_013_RequestBuyBindingSource.Count > 0)
            {

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 24))
                {
     

                    Buy.Frm_001_SabteDarkhastKharid ob = new Buy.Frm_001_SabteDarkhastKharid(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 25),int.Parse(gridEX2.GetValue("columnid").ToString()));

                    //ob.MdiParent = this.MdiParent;
                    ob.Text = "ویرایش درخواست خرید";
                    ob.ShowDialog();
                    dataSet_Buy.EnforceConstraints = true;
                    dataSet_Buy.EnforceConstraints = false;
                    table_013_RequestBuyTableAdapter.Fill(dataSet_Buy.Table_013_RequestBuy);
                    table_014_Child_RequestBuyTableAdapter.Fill(dataSet_Buy.Table_014_Child_RequestBuy);
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
      


            }
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (e.Column.Key == "column09")
            {

                if (gridEX1.GetValue("column09").ToString() == "True")
                {
                    gridEX1.SetValue("column08", gridEX1.GetValue("column04"));
                    gridEX1.SetValue("column10", Class_BasicOperation._UserName);
                    gridEX1.SetValue("column11", alldatabase.get_one_fiald("SELECT getdate()"));
                }
                else
                {
                    gridEX1.SetValue("column08","0");

                }
                

            }
            if (e.Column.Key == "column08")
            {
                if (gridEX1.GetValue("column08").ToString() != "" || gridEX1.GetValue("column08").ToString() != "0")
                {
                    gridEX1.SetValue("column09", "True");
                    gridEX1.SetValue("column10", Class_BasicOperation._UserName);
                    gridEX1.SetValue("column11", alldatabase.get_one_fiald("SELECT getdate()"));
                }
                else
                {
                   gridEX1.SetValue("column09", "False");
                }
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (_del)
            {
                try
                {
                    gridEX1.MoveToNewRecord();
                    table_014_Child_RequestBuyBindingSource.EndEdit();
                    table_014_Child_RequestBuyTableAdapter.Update(dataSet_Buy.Table_014_Child_RequestBuy);
                    MessageBox.Show("اطلاعات با موفقیت ذخیره شد");
                }
                catch(Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex,"");
                }
            }
            else
                MessageBox.Show("کاربر گرامی شما اجازه تایید یا عدم تایید درخواست خرید را ندارید");
        }

        private void Frm_002_MoshahedeTaeed_DarkhastKharid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
        }
    }
}
