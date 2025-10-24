using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;


namespace PSHOP
{
    public partial class MainForm : Form
    {
        public string _CompName, _FinYear, _ConnectionString;
        public string _UserName;
        public Int16 _CompID;
        bool _WareType, _FinType, _ShowMsg = true;
        Classes.Class_Documents clDoc = new Classes.Class_Documents();

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string CompName, string UserName, string FinYear, Int16 CompID, bool WareType, bool FinType, string ConnectionString)
        {
            InitializeComponent();
            _UserName = UserName;
            _CompName = CompName;
            _CompID = CompID;
            _FinYear = FinYear;
            _WareType = WareType;
            _FinType = FinType;
            _ConnectionString = ConnectionString;
            Class_BasicOperation._OrgCode = _CompID;
            Class_BasicOperation._UserName = _UserName;
            Class_BasicOperation._Year = FinYear;
            Class_BasicOperation._WareType = _WareType;
            Class_BasicOperation._FinType = _FinType;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //SerialLicenceKey SLK = new SerialLicenceKey();
            //string MachineID = SLK.GetMachineId();
            //string CurrentLicence = SLK.ReadRegistry("PERPLicenceKey");

            //if (CurrentLicence == null)
            //{
            //    _ShowMsg = false;
            //    this.Close();
            //}
            //else if (!SLK.CheckLicenceKey(CurrentLicence, MachineID))
            //{
            //    _ShowMsg = false;
            //    this.Close();
            //}
            //else
            //{

            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fa-IR");
            Class_BasicOperation.ChangeLanguage("fa-IR");

            lbl_Year.Text = "سال مالی:" + _FinYear;
            lbl_Org.Text = _CompName;
            lbl_Today.Text = " امروز: " + FarsiLibrary.Utils.PersianDate.Now.ToWritten();
            lbl_User.Text = "کاربر جاری: " + _UserName;

            if (_ConnectionString != null)
            {
                Class_ChangeConnectionString.CurrentConnection = _ConnectionString;
            }
            styleManager1.ManagerColorTint = Properties.Settings.Default.BackColor;
            foreach (Control control in this.Controls)
            {
                MdiClient client = control as MdiClient;
                if (!(client == null))
                {
                    client.BackColor = Properties.Settings.Default.BackColor;
                    break;
                }
            }
            //}

        }

        private void bt_MinimizeMenu_Click(object sender, EventArgs e)
        {
            if (ribbonControl1.Expanded)
            {
                ribbonControl1.Expanded = false;
                bt_MinimizeMenu.Text = "بزرگ کردن نوار منو";
            }
            else
            {
                ribbonControl1.Expanded = true;
                bt_MinimizeMenu.Text = "کوچک کردن نوار منو";
            }

        }


        private void bt_RegisterOrder_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 16))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form01_RegisterOrders")
                    {
                        child.Focus();
                        return;
                    }
                }

                _03_Order.Form01_RegisterOrders ob = new _03_Order.Form01_RegisterOrders(UserScope.CheckScope(_UserName, "Column11", 17), -1);

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_Confirm_SaleManager_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 10))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form03_ConfirmBySaleManager")
                    {
                        child.Focus();
                        return;
                    }
                }


                _03_Order.Form03_ConfirmBySaleManager ob = new _03_Order.Form03_ConfirmBySaleManager();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_Confirm_FinManager_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 13))
            {

                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form04_ConfirmByFinManager")
                    {
                        child.Focus();
                        return;
                    }
                }

                _03_Order.Form04_ConfirmByFinManager ob = new _03_Order.Form04_ConfirmByFinManager();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem9_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 20))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_002_Faktor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _05_Sale.Frm_002_Faktor ob = new _05_Sale.Frm_002_Faktor(UserScope.CheckScope(_UserName, "Column11", 21));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }


        private void buttonItem12_Click(object sender, EventArgs e)
        {
            //Class_UserScope UserScope = new Class_UserScope();
            //if (UserScope.CheckScope(_UserName, "Column11", 24))
            //{
            //    foreach (Form child in Application.OpenForms)
            //    {
            //        if (child.Name == "Frm_001_SabteDarkhastKharid")
            //        {
            //            child.Focus();
            //            return;
            //        }
            //    }

            //    _04_Buy.Frm_001_SabteDarkhastKharid ob = new _04_Buy.Frm_001_SabteDarkhastKharid(UserScope.CheckScope(_UserName, "Column11", 25), -1);

            //    ob.MdiParent = this;
            //    ob.Show();
            //}
            //else
            //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem13_Click(object sender, EventArgs e)
        {
            //Class_UserScope UserScope = new Class_UserScope();
            //if (UserScope.CheckScope(_UserName, "Column11", 26))
            //{
            //    foreach (Form child in Application.OpenForms)
            //    {
            //        if (child.Name == "Frm_002_MoshahedeTaeed_DarkhastKharid")
            //        {
            //            child.Focus();
            //            return;
            //        }
            //    }

            //    _04_Buy.Frm_002_ConfirmBuyRequests ob = new _04_Buy.Frm_002_ConfirmBuyRequests(
            //        UserScope.CheckScope(_UserName, "Column11", 27));

            //    ob.MdiParent = this;
            //    ob.Show();
            //}
            //else
            //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem14_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 28))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_003_FaktorKharid")
                    {
                        child.Focus();
                        return;
                    }
                }

                _04_Buy.Frm_003_FaktorKharid ob = new _04_Buy.Frm_003_FaktorKharid(UserScope.CheckScope(_UserName, "Column11", 29), 0);

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem39_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 59))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_001_TakhfifatGoroohMoshtari")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Takhfifat.Frm_001_TakhfifatGoroohMoshtari ob = new _02_BasicInfo.Takhfifat.Frm_001_TakhfifatGoroohMoshtari();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem40_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 60))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_002_TakhfifVijheMoshtari")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Takhfifat.Frm_002_TakhfifVijheMoshtari ob = new _02_BasicInfo.Takhfifat.Frm_002_TakhfifVijheMoshtari();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem41_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 61))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_003_TakhfifKalaiiVijheMoshtari")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Takhfifat.Frm_003_TakhfifKalaiiVijheMoshtari ob = new _02_BasicInfo.Takhfifat.Frm_003_TakhfifKalaiiVijheMoshtari();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ViewOrders_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 63))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form02_ViewOrders")
                    {
                        child.Focus();
                        return;
                    }
                }

                _03_Order.Form02_ViewOrders frm = new _03_Order.Form02_ViewOrders();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Prefactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 18))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_001_PishFaktor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _05_Sale.Frm_001_PishFaktor ob = new _05_Sale.Frm_001_PishFaktor(UserScope.CheckScope(_UserName, "Column11", 19));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_ViewPrefactors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 64))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_002_ViewPrefactors")
                    {
                        child.Focus();
                        return;
                    }
                }

                _05_Sale.Frm_002_ViewPrefactors ob = new _05_Sale.Frm_002_ViewPrefactors();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }



        private void bt_BackUp_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 65))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form01_Backup")
                    {
                        child.Focus();
                        return;
                    }
                }

                _01_Accessories.Form01_Backup frm = new _01_Accessories.Form01_Backup();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ViewSaleFactors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 67))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_008_ViewSaleFactors")
                    {
                        child.Focus();
                        return;
                    }
                }
                _05_Sale.Frm_008_ViewSaleFactors frm = new _05_Sale.Frm_008_ViewSaleFactors();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Transactions_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 68))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ViewBuyFactors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 79))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ViewRequests_Click(object sender, EventArgs e)
        {
            //Class_UserScope UserScope = new Class_UserScope();
            //if (UserScope.CheckScope(_UserName, "Column11", 80))
            //{
            //    foreach (Form child in Application.OpenForms)
            //    {
            //        if (child.Name == "Frm_002_ViewBuyRequests")
            //        {
            //            child.Focus();
            //            return;
            //        }
            //    }
            //    _04_Buy.Frm_002_ViewBuyRequests frm = new _04_Buy.Frm_002_ViewBuyRequests();
            //    frm.MdiParent = this;
            //    frm.Show();
            //}
            //else
            //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Vehicle_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 58))
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

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_CityAndProvince_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 55))
            {

                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_001_Moarefi_Ostan_Shahr")
                    {
                        child.Focus();
                        return;
                    }
                }
                _02_BasicInfo.Frm_001_Moarefi_Ostan_Shahr ob = new _02_BasicInfo.Frm_001_Moarefi_Ostan_Shahr(UserScope.CheckScope(_UserName, "Column11", 56));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_DefineGood_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 5))
            {

                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_009_AdditionalGoodsInfo2")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_009_AdditionalGoodsInfo ob = new _02_BasicInfo.Frm_009_AdditionalGoodsInfo(UserScope.CheckScope(_UserName, "Column11", 6));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Customer_Click(object sender, EventArgs e)
        {

        }

        private void bt_SaleType_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 3))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_007_SaleType")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_007_SaleType ob = new _02_BasicInfo.Frm_007_SaleType(UserScope.CheckScope(_UserName, "Column11", 4));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleAdditionReduction_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 45))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_002_TakhfifEzafeSale")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_002_TakhfifEzafeSale ob = new _02_BasicInfo.Frm_002_TakhfifEzafeSale(UserScope.CheckScope(_UserName, "Column11", 46));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_BuyAdditionReduction_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 47))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_003_TakhfifEzafeBuy")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_003_TakhfifEzafeBuy ob = new _02_BasicInfo.Frm_003_TakhfifEzafeBuy(UserScope.CheckScope(_UserName, "Column11", 48));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void colorPickerDropDown1_SelectedColorChanged(object sender, EventArgs e)
        {
            this.styleManager1.ManagerColorTint = colorPickerDropDown1.SelectedColor;
            foreach (Control control in this.Controls)
            {
                // #2
                MdiClient client = control as MdiClient;
                if (!(client == null))
                {
                    // #3
                    client.BackColor = colorPickerDropDown1.SelectedColor;
                    // 4#
                    break;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_ShowMsg)
            {
                if (DialogResult.No == MessageBox.Show("آیا مایلید از برنامه خارج شوید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    e.Cancel = true;
                else
                {
                    Properties.Settings.Default.BackColor = colorPickerDropDown1.SelectedColor;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void bt_SaleFactorOperation_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 82))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_012_ExportDoc_Draft_InTotal")
                    {
                        child.Focus();
                        return;
                    }
                }

                _05_Sale.Frm_012_ExportDoc_Draft_InTotal ob = new _05_Sale.Frm_012_ExportDoc_Draft_InTotal(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 69),
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 71));
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ProduceReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 83))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form01_ProduceReport")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form01_ProduceReport frm = new _06_Reports._01_Orders.Form01_ProduceReport();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_LastPrice_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 53))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form02_FinalPriceList")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form02_FinalPriceList frm = new _06_Reports._01_Orders.Form02_FinalPriceList();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_TotalFOrderReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 85))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form08_TotalOrderReport")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form08_TotalOrderReport frm = new _06_Reports._01_Orders.Form08_TotalOrderReport();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_OrderWay_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 84))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form03_OrderGoodsWay")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form03_OrderGoodsWay frm = new _06_Reports._01_Orders.Form03_OrderGoodsWay();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_OrderReport_Product_Customer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 49))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form04_Order_Goods_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form04_Order_Goods_Customer frm = new _06_Reports._01_Orders.Form04_Order_Goods_Customer();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_OrderReport_Customer_Product_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 50))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form05_Order_Customer_Good")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form05_Order_Customer_Good frm = new _06_Reports._01_Orders.Form05_Order_Customer_Good();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_OrderReport_Product_City_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 51))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form06_Order_Good_City")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form06_Order_Good_City frm = new _06_Reports._01_Orders.Form06_Order_Good_City();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_OrderReport_City_Product_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 52))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form07_Order_City_Good")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form07_Order_City_Good frm = new _06_Reports._01_Orders.Form07_Order_City_Good();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_OrderReport_SendCost_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 86))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form09_SendCostReport")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form09_SendCostReport frm = new _06_Reports._01_Orders.Form09_SendCostReport();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ExportDoc_Receipt_ForBuyFactors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 87))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_013_ExportDoc_Receipt_InTotal")
                    {
                        child.Focus();
                        return;
                    }
                }

                _04_Buy.Frm_013_ExportDoc_Receipt_InTotal ob = new _04_Buy.Frm_013_ExportDoc_Receipt_InTotal(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 75),
                    UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 74));
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_SaleDoc_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 32))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_CustomerBase_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 33))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_GoodBase_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 34))
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

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_MonthlyNumber_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 34))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form04_SaleReport_NumericMonthly")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form04_SaleReport_NumericMonthly ob = new _06_Reports._02_Sale.Form04_SaleReport_NumericMonthly();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_MonthlyRial_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 35))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form05_SaleReport_Monthly_Value")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value ob = new _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bt_Buy_Doc_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 38))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_BuyReport_Customer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 39))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_BuyReport_Goods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 40))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_BuyReport_Monthly_Numeric_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 42))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form04_BuyReport_Montly_Numeric")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form04_BuyReport_Montly_Numeric frm = new _06_Reports._03_Buy.Form04_BuyReport_Montly_Numeric();

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_BuyReport_Monthly_Riali_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 41))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form05_BuyReport_Monthly_Price")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form05_BuyReport_Monthly_Price frm = new _06_Reports._03_Buy.Form05_BuyReport_Monthly_Price();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_Gift_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 88))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form06_SaleReport_Gifts")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form06_SaleReport_Gifts frm = new _06_Reports._02_Sale.Form06_SaleReport_Gifts();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReturnFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 22))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_013_ReturnFactor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _05_Sale.Frm_013_ReturnFactor ob = new _05_Sale.Frm_013_ReturnFactor(UserScope.CheckScope(_UserName, "Column11", 23));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ViewReturnSaleFactors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 92))
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

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_BuyReturn_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 30))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_014_ReturnFactor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _04_Buy.Frm_014_ReturnFactor ob = new _04_Buy.Frm_014_ReturnFactor(UserScope.CheckScope(_UserName, "Column11", 31));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ViewBuyReturnFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 96))
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

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Report_ReturnSale_Customer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 36))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form07_ReturnSaleReport_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form07_ReturnSaleReport_Customer frm = new _06_Reports._02_Sale.Form07_ReturnSaleReport_Customer();

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ReturnSaleReport_Goods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 37))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form08_ReturnSaleReport_Good")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form08_ReturnSaleReport_Good frm = new _06_Reports._02_Sale.Form08_ReturnSaleReport_Good();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ReportReturnBuyFactor_Supplier_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 43))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form06_ReturnBuyReport_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form06_ReturnBuyReport_Customer ob = new _06_Reports._03_Buy.Form06_ReturnBuyReport_Customer();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_BuyReturnFactor_Report_Goods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 44))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_007_AmarKharidKalaMarjooi")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form07_ReturnBuyReport_Good ob = new _06_Reports._03_Buy.Form07_ReturnBuyReport_Good();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }


        private void bt_MarginReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 97))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleExtraReductionReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 98))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form10_ExtraReduction")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form10_ExtraReduction frm = new _06_Reports._02_Sale.Form10_ExtraReduction();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_DiscountsReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 99))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form11_DiscountReport")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form11_DiscountReport frm = new _06_Reports._02_Sale.Form11_DiscountReport();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ExtraReduction_BuyFactors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 100))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form08_ExtraReduction")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form08_ExtraReduction frm = new _06_Reports._03_Buy.Form08_ExtraReduction();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_PricingGoodsForGroups_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 101))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_009_CustomerGroupPricing")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_009_CustomerGroupPricing frm = new _02_BasicInfo.Frm_009_CustomerGroupPricing(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 102));
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }


        private void bt_DefineServices_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 103))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form01_DefineServices")
                    {
                        child.Focus();
                        return;
                    }
                }

                _07_Services.Form01_DefineServices frm = new _07_Services.Form01_DefineServices(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 104));
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_RegisterServiceFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 105))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form02_RegisterServiceFactor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _07_Services.Form02_RegisterServiceFactor frm = new _07_Services.Form02_RegisterServiceFactor(0, UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 106));
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ViewServiceFactors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 109))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form03_ViewServiceFactors")
                    {
                        child.Focus();
                        return;
                    }
                }

                _07_Services.Form03_ViewServiceFactors frm = new _07_Services.Form03_ViewServiceFactors();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ServiceReport_Document_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 110))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form05_ServiceReport_Doc")
                    {
                        child.Focus();
                        return;
                    }
                }

                _07_Services.Form05_ServiceReport_Doc frm = new _07_Services.Form05_ServiceReport_Doc();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ServiceReport_Customers_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 111))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form06_ServiceReport_Customers")
                    {
                        child.Focus();
                        return;
                    }
                }

                _07_Services.Form06_ServiceReport_Customers frm = new _07_Services.Form06_ServiceReport_Customers();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ServiceReport_Services_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 112))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form07_ServiceReport_Services")
                    {
                        child.Focus();
                        return;
                    }
                }

                _07_Services.Form07_ServiceReport_Services frm = new _07_Services.Form07_ServiceReport_Services();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ServiceReport_ExtraReductions_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 113))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form08_ServiceReport_ExtraRed")
                    {
                        child.Focus();
                        return;
                    }
                }

                _07_Services.Form08_ServiceReport_ExtraRed frm = new _07_Services.Form08_ServiceReport_ExtraRed();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem10_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 85))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form08_TotalOrderQtyRpt")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form08_TotalOrderQtyRpt frm08 =
                    new _06_Reports._01_Orders.Form08_TotalOrderQtyRpt();
                frm08.MdiParent = this;
                frm08.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem11_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 62))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_005_Javayez")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_005_Javayez ob = new _02_BasicInfo.Frm_005_Javayez(UserScope.CheckScope(_UserName, "Column11", 63));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem13_Click_1(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 62))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_010_RialAwards")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_010_RialAwards ob = new _02_BasicInfo.Frm_010_RialAwards(
                    UserScope.CheckScope(_UserName, "Column11", 63));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem14_Click_1(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 62))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_015_TQtylAwards")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_015_TQtyAwards ob = new _02_BasicInfo.Frm_015_TQtyAwards(
                    UserScope.CheckScope(_UserName, "Column11", 63));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem15_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 62))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_020_VehicleAwards")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_020_VehicleAwards ob = new _02_BasicInfo.Frm_020_VehicleAwards(
                    UserScope.CheckScope(_UserName, "Column11", 63));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Sale_MonthlyNumeric_Customer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 114))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form04_SaleReport_NumericMonthly_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form04_SaleReport_NumericMonthly_Customer frm = new _06_Reports._02_Sale.Form04_SaleReport_NumericMonthly_Customer();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_MonthlyRial_Customer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 115))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form05_SaleReport_Monthly_Value_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value_Customer frm = new _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value_Customer();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_LegalFactors_Click(object sender, EventArgs e)
        {
            //Class_UserScope UserScope = new Class_UserScope();
            //if (UserScope.CheckScope(_UserName, "Column11", 116))
            //{
            //    foreach (Form child in Application.OpenForms)
            //    {
            //        if (child.Name == "Frm_018_LegalFactors")
            //        {
            //            child.Focus();
            //            return;
            //        }
            //    }

            //    _05_Sale.LegalFactors.Frm_018_LegalFactors frm = new _05_Sale.LegalFactors.Frm_018_LegalFactors();
            //    frm.MdiParent = this;
            //    frm.Show();
            //}
            //else
            //    Class_BasicOperation.ShowMsg("",
            //        "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Report_LegalFactor_Customer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 117))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form12_LegalFactors_Report_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form12_LegalFactors_Report_Customer frm = new _06_Reports._02_Sale.Form12_LegalFactors_Report_Customer();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Report_LegalFactor_ExtraReduction_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 118))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form13_LegalFactors_ExtraReduction")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form13_LegalFactors_ExtraReduction frm = new _06_Reports._02_Sale.Form13_LegalFactors_ExtraReduction();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Report_Order_Good_Customer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 119))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form03_OrderGoodsWay_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form03_OrderGoodsWay_Customer frm = new _06_Reports._01_Orders.Form03_OrderGoodsWay_Customer();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Report_Order_Good_Provice_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 120))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form03_OrderGoodsWay_Province")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._01_Orders.Form03_OrderGoodsWay_Province frm = new _06_Reports._01_Orders.Form03_OrderGoodsWay_Province();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_EditLegalFactors_Click(object sender, EventArgs e)
        {
            //Class_UserScope UserScope = new Class_UserScope();
            //if (UserScope.CheckScope(_UserName, "Column11", 122))
            //{
            //    foreach (Form child in Application.OpenForms)
            //    {
            //        if (child.Name == "Frm_021_LegalFactor")
            //        {
            //            child.Focus();
            //            return;
            //        }
            //    }

            //    _05_Sale.LegalFactors.Frm_021_LegalFactor frm = new _05_Sale.LegalFactors.Frm_021_LegalFactor();
            //    frm.MdiParent = this;
            //    frm.Show();
            //}
            //else
            //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ViewLegalFactors_Click(object sender, EventArgs e)
        {
            //Class_UserScope UserScope = new Class_UserScope();
            //if (UserScope.CheckScope(_UserName, "Column11", 124))
            //{
            //    foreach (Form child in Application.OpenForms)
            //    {
            //        if (child.Name == "Frm_022_ViewLegalFactors")
            //        {
            //            child.Focus();
            //            return;
            //        }
            //    }

            //    _05_Sale.LegalFactors.Frm_022_ViewLegalFactors frm = new _05_Sale.LegalFactors.Frm_022_ViewLegalFactors();
            //    frm.MdiParent = this;
            //    frm.Show();
            //}
            //else
            //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_MonthlyRial_Currency_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 35))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form05_SaleReport_Monthly_Value_Currency")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value_Currency ob = new _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value_Currency();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_MonthlyRial_Customer_Currency_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 115))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form05_SaleReport_Monthly_Value_Customer_Currency")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value_Customer_Currency frm = new _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value_Customer_Currency();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_BuyReport_Monthly_Currency_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 41))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form05_BuyReport_Monthly_Currency")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form05_BuyReport_Monthly_Currency frm = new _06_Reports._03_Buy.Form05_BuyReport_Monthly_Currency();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_Customer_Good_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 34))
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

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_CompReport_Goods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 126))
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

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_CompReport_SaleFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 127))
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

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_MarginReport_GoodCustomerFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 132))
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

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_Sale_Visitors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 133))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form19_Visitors")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form19_Visitors frm = new _06_Reports._02_Sale.Form19_Visitors();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Visitors_Customers_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 134))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form20_Visitors_Customers")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form20_Visitors_Customers frm = new _06_Reports._02_Sale.Form20_Visitors_Customers();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Sale_Monthly_Numeric_Visitor_Goods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 135))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form04_SaleReport_NumericMonthly_Goods_Visitor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form04_SaleReport_NumericMonthly_Goods_Visitor frm = new _06_Reports._02_Sale.Form04_SaleReport_NumericMonthly_Goods_Visitor();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Sale_Monthly_Value_Visitor_Goods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 136))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form05_SaleReport_Monthly_Value_Goods_Visitor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value_Goods_Visitor frm = new _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value_Goods_Visitor();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Sale_Monthly_Value_Visitor_Customer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 137))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form05_SaleReport_Monthly_Value_Customer_Visitor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value_Customer_Visitor frm = new _06_Reports._02_Sale.Form05_SaleReport_Monthly_Value_Customer_Visitor();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Sale_CompReport_CustomerSaleFactors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 138))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void Serv_bt_CustomerBill_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 139))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form09_CustomerBill")
                    {
                        child.Focus();
                        return;
                    }
                }

                _07_Services.Form09_CustomerBill frm = new _07_Services.Form09_CustomerBill();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void Sale_Report_Comp_MergeReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 140))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void Sale_Report_VarianceSaleAcc_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 141))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form23_VarianceSaleAndACC")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form23_VarianceSaleAndACC frm = new _06_Reports._02_Sale.Form23_VarianceSaleAndACC();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Legal_Report_CustomerGood_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 142))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form24_CustomerGoods_Legal")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form24_CustomerGoods_Legal frm = new _06_Reports._02_Sale.Form24_CustomerGoods_Legal();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Legal_Report_CompleteReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 143))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form25_Legals_CompReport_Goods")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form25_Legals_CompReport_Goods frm = new _06_Reports._02_Sale.Form25_Legals_CompReport_Goods();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_LegalReport_Monthly_Numeric_Goods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 144))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form26_Legal_NumericMonthly_Goods")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form26_Legal_NumericMonthly_Goods frm = new _06_Reports._02_Sale.Form26_Legal_NumericMonthly_Goods();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_LegalReport_Monthly_Numeric_Customer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 145))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form27_Legal_NumericMonthly_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form27_Legal_NumericMonthly_Customer frm = new _06_Reports._02_Sale.Form27_Legal_NumericMonthly_Customer();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_LegalReport_Monthly_Riali_Goods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 146))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form28_Legal_Monthly_Value_Good")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form28_Legal_Monthly_Value_Good frm = new _06_Reports._02_Sale.Form28_Legal_Monthly_Value_Good();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_LegalReport_Monthly_Riali_Customer_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 147))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form29_Legal_Report_Monthly_Value_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form29_Legal_Report_Monthly_Value_Customer frm = new _06_Reports._02_Sale.Form29_Legal_Report_Monthly_Value_Customer();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_SeasonReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 148))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form30_SeasonReport")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form30_SeasonReport frm = new _06_Reports._02_Sale.Form30_SeasonReport();
                frm.MdiParent = this;
                frm.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_TransferInfo_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 149))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form03_TransferOpenOrders")
                    {
                        child.Focus();
                        return;
                    }
                }

                _01_Accessories.Form03_TransferOpenOrders frm = new _01_Accessories.Form03_TransferOpenOrders();
                frm.MdiParent = this;
                frm.Show();
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SMSSetting_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 150))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_025_SMSSetting")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_025_SMSSetting frm = new _02_BasicInfo.Frm_025_SMSSetting();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_BuyReport_LastBuyPrice_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 151))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form09_LastBuyPrice")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form09_LastBuyPrice frm = new _06_Reports._03_Buy.Form09_LastBuyPrice();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Buy_CompReport_Factor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 152))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Buy_CompReport_Good_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 153))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Sale_Prefactor_CompReport_Facotr_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 154))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form01_CompReport_Factor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._04_Prefactor.Form01_CompReport_Factor frm = new _06_Reports._04_Prefactor.Form01_CompReport_Factor();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Sale_Prefactor_CompReport_Good_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 155))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form02_CompReport_Goods")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._04_Prefactor.Form02_CompReport_Goods frm = new _06_Reports._04_Prefactor.Form02_CompReport_Goods();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void bt_ReturnSaleReport_Responsible_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 156))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form31_ReturnFactors_Visitors")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form31_ReturnFactors_Visitors frm = new _06_Reports._02_Sale.Form31_ReturnFactors_Visitors();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_UpdateFiles_Click(object sender, EventArgs e)
        {
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.MAIN.Replace("PERP_MAIN", "PBASE_" + Class_BasicOperation._OrgCode));
            DataTable Table = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column15 from Table_105_SystemTransactionInfo where Column00=74");
            if (Table.Rows.Count > 0)
            {
                if (Directory.Exists(Table.Rows[0][0].ToString()))
                {
                    string txtpath = Table.Rows[0][0].ToString();
                    ProcessStartInfo startInfo = new ProcessStartInfo(Application.StartupPath + "\\UpdateSystems.exe");
                    startInfo.UseShellExecute = false;
                    startInfo.Arguments = txtpath;
                    Process.Start(startInfo);


                    _ShowMsg = false;
                    Application.Exit();




                }
            }
        }

        private void btn_RegisterOrder2_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 16))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form01_RegisterOrders")
                    {
                        child.Focus();
                        return;
                    }
                }

                _08_Order2.Form01_RegisterOrders ob = new _08_Order2.Form01_RegisterOrders(UserScope.CheckScope(_UserName, "Column11", 17), -1);

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void ribbonPanel10_Click(object sender, EventArgs e)
        {

        }

        private void btn_ViewOrders2_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 63))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form02_ViewOrders")
                    {
                        child.Focus();
                        return;
                    }
                }

                _08_Order2.Form02_ViewOrders frm = new _08_Order2.Form02_ViewOrders();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_OrderDraft_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 157))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form07_OrderDraft")
                    {
                        child.Focus();
                        return;
                    }
                }

                _08_Order2.Form07_OrderDraft frm = new _08_Order2.Form07_OrderDraft();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem26_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 63))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form08_TotalReport")
                    {
                        child.Focus();
                        return;
                    }
                }

                _08_Order2.Form08_TotalReport frm = new _08_Order2.Form08_TotalReport();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("",
                    "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem28_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 20))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_002_NewFaktor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _05_Sale.Frm_002_NewFaktor ob = new _05_Sale.Frm_002_NewFaktor(UserScope.CheckScope(_UserName, "Column11", 21));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem29_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 158))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_008_SortSaleFactors")
                    {
                        child.Focus();
                        return;
                    }
                }
                _05_Sale.Frm_008_SortSaleFactors frm = new _05_Sale.Frm_008_SortSaleFactors();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_SortReturnSaleFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 159))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_016_SortReturnSaleFactors")
                    {
                        child.Focus();
                        return;
                    }
                }
                _05_Sale.Frm_016_SortReturnSaleFactors frm = new _05_Sale.Frm_016_SortReturnSaleFactors();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_SortBuyFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 160))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_012_SortBuyFactors")
                    {
                        child.Focus();
                        return;
                    }
                }
                _04_Buy.Frm_012_SortBuyFactors frm = new _04_Buy.Frm_012_SortBuyFactors();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_ReturnBuyFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 161))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_017_SortReturnBuyFactors")
                    {
                        child.Focus();
                        return;
                    }
                }
                _04_Buy.Frm_017_SortReturnBuyFactors frm = new _04_Buy.Frm_017_SortReturnBuyFactors();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_CloseCash_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 166))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_StoreSaleFactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 20))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_002_StoreFaktor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _05_Sale.Frm_002_StoreFaktor ob = new _05_Sale.Frm_002_StoreFaktor(UserScope.CheckScope(_UserName, "Column11", 21));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SpeedySaleFactor_Click(object sender, EventArgs e)
        {
            //Class_UserScope UserScope = new Class_UserScope();
            //if (UserScope.CheckScope(_UserName, "Column11", 20))
            //{
            //    foreach (Form child in Application.OpenForms)
            //    {
            //        if (child.Name == "Frm_002_ClubFactor")
            //        {
            //            child.Focus();
            //            return;
            //        }
            //    }

            //    _05_Sale.Frm_002_ClubFactor ob = new _05_Sale.Frm_002_ClubFactor(UserScope.CheckScope(_UserName, "Column11", 21));

            //    ob.MdiParent = this;
            //    ob.Show();
            //}
            //else
            //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }



        private void btn_ViewStorefactor_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 67))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem28_Click_1(object sender, EventArgs e)
        {

        }

        private void btn_DelivaryGood_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 168))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_008_DelivaryGood")
                    {
                        child.Focus();
                        return;
                    }
                }
                _05_Sale.Frm_008_DelivaryGood frm = new _05_Sale.Frm_008_DelivaryGood();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_GoodList_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 170))
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
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_Sale_Users_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 172))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form32_UsersActions")
                    {
                        child.Focus();
                        return;
                    }
                }
                _06_Reports._02_Sale.Form32_UsersActions frm = new _06_Reports._02_Sale.Form32_UsersActions();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_SaleReport_SaleByTime_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 173))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form33_SaleByTime")
                    {
                        child.Focus();
                        return;
                    }
                }
                _06_Reports._02_Sale.Form33_SaleByTime frm = new _06_Reports._02_Sale.Form33_SaleByTime();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_CompReportWithReturn_Goods_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem30_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 175))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form18_TotalMarginReport_GoodCustomerFactor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form18_TotalMarginReport_GoodCustomerFactor frm =
                    new _06_Reports._02_Sale.Form18_TotalMarginReport_GoodCustomerFactor();

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_Sale_CompReport_GoodByVisitors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 176))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form36_GoodReportByVisitors")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form36_GoodReportByVisitors frm =
                    new _06_Reports._02_Sale.Form36_GoodReportByVisitors();

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_Sale_VisitorsShare_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 177))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form35_VisitorsShare")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form35_VisitorsShare frm =
                    new _06_Reports._02_Sale.Form35_VisitorsShare();

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_GoodReportByGroup_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 178))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form37_GoodReportByGroup")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form37_GoodReportByGroup frm =
                    new _06_Reports._02_Sale.Form37_GoodReportByGroup();

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void btn_ProfitReportByGroup_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 179))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form38_ProfitReportByGoodGroup")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form38_ProfitReportByGoodGroup frm =
                    new _06_Reports._02_Sale.Form38_ProfitReportByGoodGroup();

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_ProfitReportByVisitors_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 180))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form39_ProfitReportByVisitors")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form39_ProfitReportByVisitors frm =
                    new _06_Reports._02_Sale.Form39_ProfitReportByVisitors();

                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_ViewTeap2SaleFactor_Click(object sender, EventArgs e)
        {

            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 67))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_008_ViewNewSaleFactors")
                    {
                        child.Focus();
                        return;
                    }
                }
                _05_Sale.Frm_008_ViewNewSaleFactors frm = new _05_Sale.Frm_008_ViewNewSaleFactors();
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_AmaniFactor_Click(object sender, EventArgs e)
        {
            //Class_UserScope UserScope = new Class_UserScope();
            //if (UserScope.CheckScope(_UserName, "Column11", 181))
            //{
            //    foreach (Form child in Application.OpenForms)
            //    {
            //        if (child.Name == "Frm_002_AmaniFaktor")
            //        {
            //            child.Focus();
            //            return;
            //        }
            //    }
            //    _05_Sale.Frm_002_AmaniFaktor frm = new _05_Sale.Frm_002_AmaniFaktor(UserScope.CheckScope(_UserName, "Column11", 183));
            //    frm.MdiParent = this;
            //    frm.Show();
            //}
            //else
            //    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void btnMarjoieamani_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 185))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_037_ReturnAmaniFaktor")
                    {
                        child.Focus();
                        return;
                    }
                }
                _05_Sale.Frm_037_ReturnAmaniFaktor frm = new _05_Sale.Frm_037_ReturnAmaniFaktor(UserScope.CheckScope(_UserName, "Column11", 187));
                frm.MdiParent = this;
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");


        }

        private void bt_Prefactor2_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 18))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_001_PishFaktor2")
                    {
                        child.Focus();
                        return;
                    }
                }

                _05_Sale.Frm_001_PishFaktor2 ob = new _05_Sale.Frm_001_PishFaktor2(UserScope.CheckScope(_UserName, "Column11", 19));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_ViewPrefactors2_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 64))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_002_ViewPrefactors2")
                    {
                        child.Focus();
                        return;
                    }
                }

                _05_Sale.Frm_002_ViewPrefactors2 ob = new _05_Sale.Frm_002_ViewPrefactors2();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_FactorDiffReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 189))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form40_DiffReport")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form40_DiffReport ob = new _06_Reports._02_Sale.Form40_DiffReport();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem32_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 190))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form41_VAT")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form41_VAT ob = new _06_Reports._02_Sale.Form41_VAT();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_ReturnSaleVTA_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 191))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form42_ReturnVAT")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form42_ReturnVAT ob = new _06_Reports._02_Sale.Form42_ReturnVAT();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_BuyVTA_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 192))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form41_BuyVAT")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form41_BuyVAT ob = new _06_Reports._03_Buy.Form41_BuyVAT();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_ReturnBuyVTA_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 193))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form42_ReturnBuyVAT")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._03_Buy.Form42_ReturnBuyVAT ob = new _06_Reports._03_Buy.Form42_ReturnBuyVAT();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_SetteleFactore_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 194))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_008_ViewSettelSaleFactors")
                    {
                        child.Focus();
                        return;
                    }
                }

                _05_Sale.Frm_008_ViewSettelSaleFactors ob = new _05_Sale.Frm_008_ViewSettelSaleFactors();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_Good_CustomerReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 195))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form43_Goods_Customer")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form43_Goods_Customer ob = new _06_Reports._02_Sale.Form43_Goods_Customer();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_CustomerSaleBySaleMan_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 196))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form44_SaleReport_Customer_Visitor")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form44_SaleReport_Customer_Visitor ob = new _06_Reports._02_Sale.Form44_SaleReport_Customer_Visitor();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_SaleManChart_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 198))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_030_SaleManChart")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_030_SaleManChart ob = new _02_BasicInfo.Frm_030_SaleManChart();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem32_Click_1(object sender, EventArgs e)
        {

        }

        private void buttonItem31_Click(object sender, EventArgs e)
        {

        }

        private void btn_SaleReport_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 216))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form45_SaleReport")
                    {
                        child.Focus();
                        return;
                    }
                }

                _06_Reports._02_Sale.Form45_SaleReport ob = new _06_Reports._02_Sale.Form45_SaleReport();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }



        private void btn_GoodPricing_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 219))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form01_GoodPrice")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form01_GoodPrice ob = new _09_SellerProfit.Form01_GoodPrice();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_HolidayRatio_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 220))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form02_HolidayRatio")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form02_HolidayRatio ob = new _09_SellerProfit.Form02_HolidayRatio();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_TimeRatio_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 222))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form03_TimeRatio")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form03_TimeRatio ob = new _09_SellerProfit.Form03_TimeRatio();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_CustomerGroupRatio_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 224))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form04_CustomerGroupRatio")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form04_CustomerGroupRatio ob = new _09_SellerProfit.Form04_CustomerGroupRatio();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_SaleManEvaluation_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 226))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form05_SellerEvaluation")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form05_SellerEvaluation ob = new _09_SellerProfit.Form05_SellerEvaluation();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_GoodSaleType_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 228))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form06_SaleType")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form06_SaleType ob = new _09_SellerProfit.Form06_SaleType();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_AssignGoodType_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 230))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form07_AssignGoodType")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form07_AssignGoodType ob = new _09_SellerProfit.Form07_AssignGoodType();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_SaleGoals_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 231))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form08_SaleGoals")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form08_SaleGoals ob = new _09_SellerProfit.Form08_SaleGoals();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_DiscountRatio_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 233))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form09_DiscountRatio")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form09_DiscountRatio ob = new _09_SellerProfit.Form09_DiscountRatio();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_CalcuteSellerProfit_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 235))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form14_CalcuteSellerProfit")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form14_CalcuteSellerProfit ob = new _09_SellerProfit.Form14_CalcuteSellerProfit();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_FactorSaleTypeRation_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 238))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form09_FactorSaleTypeRatio")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form09_FactorSaleTypeRatio ob = new _09_SellerProfit.Form09_FactorSaleTypeRatio();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_AssignSaleType_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 239))
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

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_SaleFactorGroupConfirm_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 236))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_040_SaleFactorGroupConfirm")
                    {
                        child.Focus();
                        return;
                    }
                }

                _05_Sale.Frm_040_SaleFactorGroupConfirm ob = new _05_Sale.Frm_040_SaleFactorGroupConfirm();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_BuyFactorGroupConfirm_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 237))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_020_BuyFactorGroupConfirm")
                    {
                        child.Focus();
                        return;
                    }
                }

                _04_Buy.Frm_020_BuyFactorGroupConfirm ob = new _04_Buy.Frm_020_BuyFactorGroupConfirm();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_PriceStatement_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 243))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_032_PriceStatement")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_032_PriceStatement ob = new _02_BasicInfo.Frm_032_PriceStatement();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_PriceStatementList_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 245))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_033_PriceStatementList")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_033_PriceStatementList ob = new _02_BasicInfo.Frm_033_PriceStatementList();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_Services_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 245))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form11_Services")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form11_Services ob = new _09_SellerProfit.Form11_Services();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_ServicesRegistration_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 248))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form12_ServicesRegistration")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form12_ServicesRegistration ob = new _09_SellerProfit.Form12_ServicesRegistration();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_Class_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 241))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_031_Class")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_031_Class ob = new _02_BasicInfo.Frm_031_Class();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_ServicesList_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 252))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form12_ViewServices")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form12_ViewServices ob = new _09_SellerProfit.Form12_ViewServices();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_ViewSellerProft_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 253))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form15_ViewSellereProfits")
                    {
                        child.Focus();
                        return;
                    }
                }

                _09_SellerProfit.Form15_ViewSellereProfits ob = new _09_SellerProfit.Form15_ViewSellereProfits();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_BuyForecast_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 255))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form43_BuyForecast")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._06_Reports._03_Buy.Form43_BuyForecast ob = new PSHOP._06_Reports._03_Buy.Form43_BuyForecast();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_AggFactorPrint_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 128))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_002_StoreFaktorPrint")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._05_Sale.Frm_002_StoreFaktorPrint ob = new PSHOP._05_Sale.Frm_002_StoreFaktorPrint();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void bt_PersonContract_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column08", 203))

            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Form008_PersonContact")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._02_BasicInfo.Form008_PersonContact ob = new PSHOP._02_BasicInfo.Form008_PersonContact();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_HighSellingGoods_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column08", 286))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_035_HighSellingGoods")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._02_BasicInfo.Frm_035_HighSellingGoods ob = new PSHOP._02_BasicInfo.Frm_035_HighSellingGoods();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_ViewCloseCash_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 298))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_030_ViewCloseCash")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._05_Sale.Frm_030_ViewCloseCash ob = new PSHOP._05_Sale.Frm_030_ViewCloseCash();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }
      
        private void buttonItem4_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 338))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_029_CloseCash_Edit")
                    {
                        child.Focus();
                        return;
                    }
                }

                PSHOP._05_Sale.Frm_029_CloseCash_Edit ob = new PSHOP._05_Sale.Frm_029_CloseCash_Edit(1);
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem5_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 338))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_029_CloseCash_Edit")
                    {
                        child.Focus();
                        return;
                    }
                }

            PSHOP._04_Buy.Frm_021_ReportBalanceBuy    ob = new PSHOP._04_Buy.Frm_021_ReportBalanceBuy  ();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_AggBuyFactorPrint_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 129))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_002_BuyFaktorPrint")
                    {
                        child.Focus();
                        return;
                    }
                }

                _04_Buy.Frm_002_BuyFaktorPrint ob = new _04_Buy.Frm_002_BuyFaktorPrint();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void btn_StoreInfo_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(_UserName, "Column11", 347))
            {
                foreach (Form child in Application.OpenForms)
                {
                    if (child.Name == "Frm_036_StoreInfo")
                    {
                        child.Focus();
                        return;
                    }
                }

                _02_BasicInfo.Frm_036_StoreInfo ob = new _02_BasicInfo.Frm_036_StoreInfo();
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }



    }
}
