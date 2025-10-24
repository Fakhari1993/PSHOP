using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSHOP
{
    public partial class Uc_Menu : UserControl
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        public Uc_Menu()
        {
            InitializeComponent();
            clDoc.RunSqlCommand(Properties.Settings.Default.SALE, 
@"DECLARE @projectId SMALLINT
DECLARE @wareID SMALLINT
DECLARE @storeID INT



IF NOT EXISTS(
       SELECT *
       FROM  PBASE_"+ Class_BasicOperation._OrgCode + @".dbo. Table_035_ProjectInfo AS tpi
   )
BEGIN
    INSERT INTO  PBASE_" + Class_BasicOperation._OrgCode + @".dbo.Table_035_ProjectInfo
      (
        -- Column00 -- this column value is auto-generated
        Column01,
        Column02
      )
    VALUES
      (
        1,
        N'پروژه پیش فرض'
      )
    SET @projectId = SCOPE_IDENTITY()
END
ELSE
BEGIN
    SET @projectId = (
            SELECT TOP 1 tpi.Column00
            FROM   PBASE_" + Class_BasicOperation._OrgCode + @".dbo.Table_035_ProjectInfo AS tpi
        )
END


IF NOT EXISTS (
       SELECT *
       FROM  PWHRS_" + Class_BasicOperation._OrgCode + "_" + Class_BasicOperation._Year + @".dbo.Table_001_PWHRS AS tp
   )
BEGIN
    INSERT INTO PWHRS_" + Class_BasicOperation._OrgCode + "_" + Class_BasicOperation._Year + @".dbo.Table_001_PWHRS
      (
        -- columnid -- this column value is auto-generated
        column01,
        column02
      )
    VALUES
      (
        1,
        N'انبار پیش فرض'
      )
    SET @wareID = SCOPE_IDENTITY()
END
ELSE
BEGIN
    SET @wareID = (
            SELECT TOP 1 tp.columnid
            FROM   PWHRS_" + Class_BasicOperation._OrgCode+"_"+Class_BasicOperation._Year+@".dbo.Table_001_PWHRS AS tp
        ) END
IF NOT EXISTS (
       SELECT *
       FROM   PBASE_" + Class_BasicOperation._OrgCode + @".dbo.Table_295_StoreInfo 
   )
BEGIN
INSERT INTO PBASE_" + Class_BasicOperation._OrgCode + @".dbo.Table_295_StoreInfo
  (
    [Column00],
    [Column01],
    [Column02],
    [Column03],
    [Column04],
    [Column05],
    [Column06],
    [Column07],
    [Column08],
    [Column09],
    [Column10],
    [Column11],
    [Column12],
    [Column13],
    [Column14],
    [Column15]
  )
VALUES
  (
    1,
    N'فروشگاه پیش فرض',
    NULL,
    NULL,
    @wareID,
    @projectId,
    NULL,
    NULL,
    0,
    NULL,
    'Admin',
    GETDATE(),
    'Admin',
    GETDATE(),
    NULL,
    NULL
  )
SET @storeID = SCOPE_IDENTITY()

END 

IF NOT EXISTS (
       SELECT *
       FROM   PBASE_" + Class_BasicOperation._OrgCode + @".dbo.Table_296_StoreUsers 
   )
BEGIN
INSERT INTO PBASE_" + Class_BasicOperation._OrgCode + @".dbo.Table_296_StoreUsers
SELECT DISTINCT @storeID,
       tui.Column00,
       'Admin',
       GETDATE(),
       'Admin',
       GETDATE()
FROM   PERP_MAIN.dbo.Table_010_UserInfo AS tui
WHERE   tui.Column05 = " + Class_BasicOperation._OrgCode + @"
END");
        }
       
        private void bt_StoreSaleType_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 3))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_007_SaleType")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_007_SaleType ob = new _02_BasicInfo.Frm_007_SaleType(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 4));

                ob.MdiParent = this.ParentForm;
                ob.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");


        }

        private void bt_StoreCustomer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 240))
            {

                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_008_PersonSaleType")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_008_PersonSaleType ob = new _02_BasicInfo.Frm_008_PersonSaleType();

                ob.MdiParent = this.ParentForm;
                ob.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void btn_StoreAssignSaleType_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 239))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_008_AssignSaleType")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_008_AssignSaleType ob = new _02_BasicInfo.Frm_008_AssignSaleType();

                ob.MdiParent = this.ParentForm;
                ob.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreDefineGood_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 5))
            {

                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_009_AdditionalGoodsInfo2")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_009_AdditionalGoodsInfo ob = new _02_BasicInfo.Frm_009_AdditionalGoodsInfo(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 6));

                ob.MdiParent = this.ParentForm;
                ob.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void btn_StoreGoodList_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 170))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_009_ViewAdditionalGoodsInfo")
                    {
                        child.Focus();
                        return;
                    }
                }
                _02_BasicInfo.Frm_009_ViewAdditionalGoodsInfo frm = new _02_BasicInfo.Frm_009_ViewAdditionalGoodsInfo();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_SoreVehicle_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 58))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_004_Moarefi_Anvae_VNaghliye")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_004_Moarefi_Anvae_VNaghliye ob = new _02_BasicInfo.Frm_004_Moarefi_Anvae_VNaghliye();

                ob.MdiParent = this.ParentForm;
                ob.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreCityAndProvince_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 55))
            {

                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_001_Moarefi_Ostan_Shahr")
                    {
                        child.Focus();
                        return;
                    }
                }
                _02_BasicInfo.Frm_001_Moarefi_Ostan_Shahr ob = new _02_BasicInfo.Frm_001_Moarefi_Ostan_Shahr(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 56));

                ob.MdiParent = this.ParentForm;
                ob.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreBuyAdditionReduction_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 47))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_003_TakhfifEzafeBuy")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_003_TakhfifEzafeBuy ob = new _02_BasicInfo.Frm_003_TakhfifEzafeBuy(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 48));

                ob.MdiParent = this.ParentForm;
                ob.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreSaleAdditionReduction_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 45))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_002_TakhfifEzafeSale")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_002_TakhfifEzafeSale ob = new _02_BasicInfo.Frm_002_TakhfifEzafeSale(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 46));

                ob.MdiParent = this.ParentForm;
                ob.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");


        }

        private void bt_StoreSetting_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 68))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_006_Transactions")
                    {
                        child.Focus();
                        return;
                    }
                }
                _02_BasicInfo.Frm_006_Transactions frm = new _02_BasicInfo.Frm_006_Transactions();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreBuyFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 28))
            {
                int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers  WHERE Column01='" + Class_BasicOperation._UserName + @"'  Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");

                if (name > 0 && Storpshop == "True")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_003_FaktorKharid")
                        {
                            child.Focus();
                            return;
                        }
                    }
                    _04_Buy.Frm_003_FaktorKharid ob = new _04_Buy.Frm_003_FaktorKharid(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 29), 0);
                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else if (Storpshop == "False")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_003_FaktorKharid")
                        {
                            child.Focus();
                            return;
                        }
                    }
                    _04_Buy.Frm_003_FaktorKharid ob = new _04_Buy.Frm_003_FaktorKharid(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 29), 0);
                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }

                else
                    MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                return;
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreViewBuyFactors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 79))
            {
               
                int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers    WHERE Column01='" + Class_BasicOperation._UserName + @"'   Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");

                if (name > 0 && Storpshop == "True")
                {

                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_012_ViewBuyFactors")
                        {
                            child.Focus();
                            return;
                        }
                    }
                    _04_Buy.Frm_012_ViewBuyFactors frm = new _04_Buy.Frm_012_ViewBuyFactors();
                    frm.MdiParent = this.ParentForm;
                    frm.Show();
                }

                else if (Storpshop == "False")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_012_ViewBuyFactors")
                        {
                            child.Focus();
                            return;
                        }
                    }
                    _04_Buy.Frm_012_ViewBuyFactors frm = new _04_Buy.Frm_012_ViewBuyFactors();
                    frm.MdiParent = this.ParentForm;
                    frm.Show();
                }

                else
                    MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                return;
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreBuyReturn_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 30))
            {
                int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers    WHERE Column01='" + Class_BasicOperation._UserName + @"'   Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");


                if (name > 0 && Storpshop == "True")
                {

                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_014_ReturnFactor")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _04_Buy.Frm_014_ReturnFactor ob = new _04_Buy.Frm_014_ReturnFactor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 31));

                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else if (Storpshop == "False")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_014_ReturnFactor")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _04_Buy.Frm_014_ReturnFactor ob = new _04_Buy.Frm_014_ReturnFactor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 31));

                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else
                    MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                return;

            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreViewBuyReturnFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 96))
            {
                int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers    WHERE Column01='" + Class_BasicOperation._UserName + @"'   Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");


                if (name > 0 && Storpshop == "True")
                {

                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_017_ViewReturnBuyFactors")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _04_Buy.Frm_017_ViewReturnBuyFactors ob = new _04_Buy.Frm_017_ViewReturnBuyFactors();

                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else if (Storpshop == "False")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_017_ViewReturnBuyFactors")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _04_Buy.Frm_017_ViewReturnBuyFactors ob = new _04_Buy.Frm_017_ViewReturnBuyFactors();
                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else
                    MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                return;
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreStoreSaleFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 20))
            {
                int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers    WHERE Column01='" + Class_BasicOperation._UserName + @"'   Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");


                if (name > 0 && Storpshop == "True")
                {


                    _05_Sale.Frm_002_StoreFaktor ob = new _05_Sale.Frm_002_StoreFaktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 21));

                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }

                else if (Storpshop == "False")
                {

                    _05_Sale.Frm_002_StoreFaktor ob = new _05_Sale.Frm_002_StoreFaktor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 21));
                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else
                    MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                return;

            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");


        }

        private void btn_StoreViewStorefactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 67))
            {
                int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers    WHERE Column01='" + Class_BasicOperation._UserName + @"'   Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");


                if (name > 0 && Storpshop == "True")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_008_ViewStoreSaleFactors")
                        {
                            child.Focus();
                            return;
                        }
                    }
                    _05_Sale.Frm_008_ViewStoreSaleFactors frm = new _05_Sale.Frm_008_ViewStoreSaleFactors();
                    frm.MdiParent = this.ParentForm;
                    frm.Show();
                }
                else if (Storpshop == "False")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_008_ViewStoreSaleFactors")
                        {
                            child.Focus();
                            return;
                        }
                    }
                    _05_Sale.Frm_008_ViewStoreSaleFactors frm = new _05_Sale.Frm_008_ViewStoreSaleFactors();
                    frm.MdiParent = this.ParentForm;
                    frm.Show();
                }
                else
                    MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                return;
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void btn_StoreCloseCash_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 166))
            {
                int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers    WHERE Column01='" + Class_BasicOperation._UserName + @"'   Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");

                if (name > 0 && Storpshop == "True")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_029_CloseCash")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _05_Sale.Frm_029_CloseCash frm = new _05_Sale.Frm_029_CloseCash();
                    frm.MdiParent = this.ParentForm;
                    frm.Show();
                }
                else if (Storpshop == "False")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_029_CloseCash")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _05_Sale.Frm_029_CloseCash frm = new _05_Sale.Frm_029_CloseCash();
                    frm.MdiParent = this.ParentForm;
                    frm.Show();
                }
                else
                    MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                return;
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreSaleReturnFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 22))
            {
                int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers    WHERE Column01='" + Class_BasicOperation._UserName + @"'   Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");


                if (name > 0 && Storpshop == "True")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_013_ReturnFactor")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _05_Sale.Frm_013_ReturnFactor ob = new _05_Sale.Frm_013_ReturnFactor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 23));

                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else if (Storpshop == "False")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_013_ReturnFactor")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _05_Sale.Frm_013_ReturnFactor ob = new _05_Sale.Frm_013_ReturnFactor(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 23));

                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else
                    MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                return;
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreViewReturnSaleFactors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 92))
            {
                int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers    WHERE Column01='" + Class_BasicOperation._UserName + @"'   Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");

                if (name > 0 && Storpshop == "True")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_016_ViewReturnSaleFactors")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _05_Sale.Frm_016_ViewReturnSaleFactors ob = new _05_Sale.Frm_016_ViewReturnSaleFactors();

                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else if (Storpshop == "False")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_016_ViewReturnSaleFactors")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _05_Sale.Frm_016_ViewReturnSaleFactors ob = new _05_Sale.Frm_016_ViewReturnSaleFactors();

                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else
                    MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                return;
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreSaleReport_SaleDoc_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 32))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form01_SaleDocuments")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form01_SaleDocuments frm = new _06_Reports._02_Sale.Form01_SaleDocuments();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreSaleReport_Customer_Good2_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 34))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form14_CustomerGoods2")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form14_CustomerGoods2 frm = new _06_Reports._02_Sale.Form14_CustomerGoods2();

                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreSaleReport_Customer_GoodnItem379_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 34))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form14_CustomerGoods")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form14_CustomerGoods frm = new _06_Reports._02_Sale.Form14_CustomerGoods();

                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreSaleReport_GoodBase_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 34))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form03_SaleReport_Goods2")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form03_SaleReport_Goods2 frm = new _06_Reports._02_Sale.Form03_SaleReport_Goods2();

                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreSaleReport_CustomerBase_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 33))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form02_SaleReport_Customer2")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form02_SaleReport_Customer2 frm = new _06_Reports._02_Sale.Form02_SaleReport_Customer2();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreCompReport_Goods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 126))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form15_CompReport_Goods")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form15_CompReport_Goods frm = new _06_Reports._02_Sale.Form15_CompReport_Goods();

                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreCompReport_SaleFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 127))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form16_CompReport_Factors")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form16_CompReport_Factors frm =
                    new _06_Reports._02_Sale.Form16_CompReport_Factors();

                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void StoreSale_Report_Comp_MergeReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 140))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form22_MergeReport")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form22_MergeReport frm = new _06_Reports._02_Sale.Form22_MergeReport();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void btn_SaleSaleReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 216))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form45_SaleReport")
                    {
                        child.Focus(); return;
                    }
                }
                _06_Reports._02_Sale.Form45_SaleReport ob = new _06_Reports._02_Sale.Form45_SaleReport();
                ob.MdiParent = this.ParentForm; ob.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreSale_CompReport_CustomerSaleFactors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 138))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form21_Faktor_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form21_Faktor_Customer frm = new _06_Reports._02_Sale.Form21_Faktor_Customer();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreCompReportWithReturn_Goods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 174))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form34_CompReportWithReturn_Goods")
                    {
                        child.Focus();
                        return;
                    }
                }
                _06_Reports._02_Sale.Form34_CompReportWithReturn_Goods frm = new _06_Reports._02_Sale.Form34_CompReportWithReturn_Goods();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreMarginReportGood_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Form18_TotalMarginReport_GoodCustomerFactor"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 175))
                {
                    _06_Reports._02_Sale.Form18_TotalMarginReport_GoodCustomerFactor frm = new _06_Reports._02_Sale.Form18_TotalMarginReport_GoodCustomerFactor();
                    frm.MdiParent = this.ParentForm;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal;
                        frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

            }
        }

        private void bt_StoreMarginReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 97))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form09_MarginReport")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form09_MarginReport frm = new _06_Reports._02_Sale.Form09_MarginReport();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreMarginReport_GoodCustomerFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 132))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form18_MarginReport_GoodCustomerFactor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form18_MarginReport_GoodCustomerFactor frm =
                    new _06_Reports._02_Sale.Form18_MarginReport_GoodCustomerFactor();

                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreBuy_Doc_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 38))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form01_BuyDocuments")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form01_BuyDocuments frm = new _06_Reports._03_Buy.Form01_BuyDocuments();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");


        }

        private void bt_StoreBuyReport_Customer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 39))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form02_BuyReport_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form02_BuyReport_Customer frm = new _06_Reports._03_Buy.Form02_BuyReport_Customer();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreBuyReport_Goods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 40))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form03_BuyReport_Goods")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form03_BuyReport_Goods frm = new _06_Reports._03_Buy.Form03_BuyReport_Goods();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreBuy_CompReport_Factor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 152))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form10_CompReport_Factor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form10_CompReport_Factor frm = new _06_Reports._03_Buy.Form10_CompReport_Factor();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_StoreBuy_CompReport_Good_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 153))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form11_CompReport_Goods")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form11_CompReport_Goods frm = new _06_Reports._03_Buy.Form11_CompReport_Goods();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void btn_AggFactorPrint_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
            {
                int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers    WHERE Column01='" + Class_BasicOperation._UserName + @"'   Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");

                if (name > 0 && Storpshop == "True")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_002_StoreFaktorPrint")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _05_Sale.Frm_002_StoreFaktorPrint ob = new _05_Sale.Frm_002_StoreFaktorPrint();
                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else if (Storpshop == "False")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_002_StoreFaktorPrint")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _05_Sale.Frm_002_StoreFaktorPrint ob = new _05_Sale.Frm_002_StoreFaktorPrint();
                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else
                    MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                return;
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_PersonContract_Click(object sender, EventArgs e)
        {

            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 203))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form008_PersonContact")
                    {
                        child.Focus(); return;
                    }
                }
                _02_BasicInfo.Form008_PersonContact ob = new _02_BasicInfo.Form008_PersonContact();
                ob.MdiParent = this.ParentForm; ob.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }
        private void btn_ViewCloseCash_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 298))
            {
                int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers    WHERE Column01='" + Class_BasicOperation._UserName + @"'   Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");

                if (name > 0 && Storpshop == "True")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_030_ViewCloseCash")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _05_Sale.Frm_030_ViewCloseCash ob = new _05_Sale.Frm_030_ViewCloseCash();
                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else if (Storpshop == "False")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_030_ViewCloseCash")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _05_Sale.Frm_030_ViewCloseCash ob = new _05_Sale.Frm_030_ViewCloseCash();
                    ob.MdiParent = this.ParentForm;
                    ob.Show();
                }
                else
                    MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                return;

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }
        private void btn_DiffrencReport_Click(object sender, EventArgs e)
        {

            foreach (Form child in Application.OpenForms)
            {
                if (child.Name == "DiffrenceReport")
                {
                    child.Focus();
                    return;
                }
            }

            _06_Reports._02_Sale.DiffrenceReport frm = new _06_Reports._02_Sale.DiffrenceReport();
            frm.MdiParent = this.ParentForm;
            frm.Show();

        }

        private void btn_GoodProfit_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 336))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form46_GoodProfit")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form46_GoodProfit frm = new _06_Reports._02_Sale.Form46_GoodProfit();
                frm.MdiParent = this.ParentForm;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }
        private void bt_AggFactorPrint_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_002_BuyFaktorPrint"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 129))
                {
                    int name = Convert.ToInt16(clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"SELECT COUNT(*) FROM dbo.Table_296_StoreUsers    WHERE Column01='" + Class_BasicOperation._UserName + @"'   Or ((Select Column02 from PERP_MAIN.dbo.Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" + Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + @"') = 1  ) "));
                    string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");


                    if (name > 0 && Storpshop == "True")
                    {
                        _04_Buy.Frm_002_BuyFaktorPrint frm = new _04_Buy.Frm_002_BuyFaktorPrint();
                        frm.MdiParent = this.ParentForm;
                        if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                        {
                            frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                        }
                        frm.Show(); frm.Focus();
                    }

                    else if (Storpshop == "False")
                    {
                        _04_Buy.Frm_002_BuyFaktorPrint frm = new _04_Buy.Frm_002_BuyFaktorPrint();
                        frm.MdiParent = this.ParentForm;
                        if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                        {
                            frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                        }
                        frm.Show(); frm.Focus();
                    }

                    else
                        MessageBox.Show("کاربر جاری به این شعبه دسترسی ندارد");
                    return;
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
        }

        private void btn_ReportDay_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_Rpt_SanadTypeSale"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 349))
                {
                    _06_Reports.Frm_Rpt_SanadTypeSale frm = new _06_Reports.Frm_Rpt_SanadTypeSale();
                    frm.MdiParent = this.ParentForm;
                    if (frm.MdiParent.MdiChildren.Length > 1 && frm.MdiParent.MdiChildren[0].WindowState == FormWindowState.Maximized)
                    {
                        frm.MdiParent.MdiChildren[0].WindowState = FormWindowState.Normal; frm.WindowState = FormWindowState.Maximized;
                    }
                    frm.Show(); frm.Focus();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
        }
        private void buttonItem350_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation. _UserName, "Column11", 347))
            {
               

                string Storpshop = clDoc.ExScalarQuery(Properties.Settings.Default.BASE, @"select Column02 from Table_030_Setting where ColumnId=83");
                if (Storpshop == "True")
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_036_StoreInfo")
                        {
                            child.Focus();
                            return;
                        }
                    }

                    _02_BasicInfo.Frm_036_StoreInfo frm = new _02_BasicInfo.Frm_036_StoreInfo();
                    frm.MdiParent = this.ParentForm;
                    frm.Show();
                }
                else
                {
                    Class_BasicOperation.ShowMsg("", " کاربر گرامی امکان دسترسی  به این نسخه فروشگاهی را ندارید ", "None");
                    return;
                }

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }
        private void btn_HighSellingGoods_Click(object sender, EventArgs e)
        {

            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation. _UserName, "Column11", 286))
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_035_HighSellingGoods")
                        {
                            child.Focus(); return;
                        }
                    }
                    _02_BasicInfo.Frm_035_HighSellingGoods ob = new _02_BasicInfo.Frm_035_HighSellingGoods();
                    ob.MdiParent = this.ParentForm; ob.Show();
                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
        }
        private void btn_RptBlancaBuy_Click(object sender, EventArgs e)
        {
            if (!CheckOpenForms("Frm_021_ReportBalanceBuy"))
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation. _UserName, "Column11", 341))
                {
                    foreach (Form child in Application.OpenForms)
                    {
                        if (child.Name == "Frm_021_ReportBalanceBuy")
                        { child.Focus(); return; }
                    }
                    _04_Buy.Frm_021_ReportBalanceBuy frm = new _04_Buy.Frm_021_ReportBalanceBuy();
                    frm.MdiParent = this.ParentForm;
                    frm.Show();
                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

            }
        }

        private bool CheckOpenForms(string FormName)
        {
            foreach (Form item in Application.OpenForms)
            {
                if (item.Name == FormName)
                {
                    item.BringToFront();
                    return true;
                }
            }
            return false;
        }


    }
}
