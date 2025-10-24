using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;


namespace PSALE
{
    public partial class MainForm : Form
    {
       public string _CompName, _FinYear,_ConnectionString;
        public string _UserName;
        public Int16 _CompID;
        bool _WareType,_FinType;
        Class_UserScope UserScope = new Class_UserScope();
       
        public MainForm()
        {
            InitializeComponent();
        }
        public MainForm(string CompName,string UserName,string FinYear,Int16 CompID,bool WareType,bool FinType,string ConnectionString)
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
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fa-IR");
            Class_BasicOperation.ChangeLanguage("fa-IR");
            
            lbl_Year.Text ="سال مالی:" + _FinYear;
            lbl_Org.Text = _CompName;
            lbl_Today.Text = " امروز: " + FarsiLibrary.Utils.PersianDate.Now.ToWritten();
            lbl_User.Text= "کاربر جاری: " + _UserName;
           
            if (_ConnectionString != null)
            {
                Class_ChangeConnectionString Change = new Class_ChangeConnectionString();
                Change.SetConnection(_ConnectionString);
            }

        }

        private void ribbonTabItem1_Click(object sender, EventArgs e)
        {

        }

        private void buttonItem1_Click(object sender, EventArgs e)
        {

        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

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

        private void buttonItem2_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 1))
            {

                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "frm_takmili_moshtari")
                    {
                        child.Focus();
                        return;
                    }
                }
                EtelaatePaye.frm_takmili_moshtari ob = new EtelaatePaye.frm_takmili_moshtari(UserScope.CheckScope(_UserName, "Column11", 2));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
      

        }

        private void buttonItem3_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 3))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "frm_anvae_foroosh")
                    {
                        child.Focus();
                        return;
                    }
                }

                EtelaatePaye.frm_anvae_foroosh ob = new EtelaatePaye.frm_anvae_foroosh(UserScope.CheckScope(_UserName, "Column11", 4));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
      
        }

        private void buttonItem4_Click(object sender, EventArgs e)
        {

            if (UserScope.CheckScope(_UserName, "Column11", 5))
            {

                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "frm_takmili_kala")
                    {
                        child.Focus();
                        return;
                    }
                }

                EtelaatePaye.frm_takmili_kala ob = new EtelaatePaye.frm_takmili_kala(UserScope.CheckScope(_UserName, "Column11", 6));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem5_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 16))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "frm_Sabt_sefareshat")
                    {
                        child.Focus();
                        return;
                    }
                }

                foroosh.frm_Sabt_sefareshat ob = new foroosh.frm_Sabt_sefareshat(UserScope.CheckScope(_UserName, "Column11", 17));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem6_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11",10))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "frm_moshahede_sefareshat_foroosh")
                    {
                        child.Focus();
                        return;
                    }
                }


                foroosh.frm_moshahede_sefareshat_foroosh ob = new foroosh.frm_moshahede_sefareshat_foroosh(UserScope.CheckScope(_UserName, "Column11", 11));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem7_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 13))
            {

                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "frm_moshahede_sefareshat_mali")
                    {
                        child.Focus();
                        return;
                    }
                }

                foroosh.frm_moshahede_sefareshat_mali ob = new foroosh.frm_moshahede_sefareshat_mali(UserScope.CheckScope(_UserName, "Column11", 14));
                
                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem8_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 7))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_001_MoshahadeSefareshat")
                    {
                        child.Focus();
                        return;
                    }
                }

                foroosh.Frm_001_MoshahadeSefareshat ob = new foroosh.Frm_001_MoshahadeSefareshat(UserScope.CheckScope(_UserName, "Column11", 8), UserScope.CheckScope(_UserName, "Column11", 9), false, -1, UserScope.CheckScope(_UserName, "Column11", 12));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem10_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 18))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_001_PishFaktor")
                    {
                        child.Focus();
                        return;
                    }
                }

                Sale.Frm_001_PishFaktor ob = new Sale.Frm_001_PishFaktor(UserScope.CheckScope(_UserName, "Column11", 19));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem9_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 20))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_002_Faktor")
                    {
                        child.Focus();
                        return;
                    }
                }

                Sale.Frm_002_Faktor ob = new Sale.Frm_002_Faktor(UserScope.CheckScope(_UserName, "Column11", 21));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem11_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 22))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_005_FaktorForooshMarjooi")
                    {
                        child.Focus();
                        return;
                    }
                }

                Sale.Frm_005_FaktorForooshMarjooi ob = new Sale.Frm_005_FaktorForooshMarjooi(UserScope.CheckScope(_UserName, "Column11", 23));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem12_Click(object sender, EventArgs e)
        {
          
            if (UserScope.CheckScope(_UserName, "Column11", 24))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_001_SabteDarkhastKharid")
                    {
                        child.Focus();
                        return;
                    }
                }

                Buy.Frm_001_SabteDarkhastKharid ob = new Buy.Frm_001_SabteDarkhastKharid(UserScope.CheckScope(_UserName, "Column11", 25),-1);

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");



        }

        private void buttonItem13_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 26))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_002_MoshahedeTaeed_DarkhastKharid")
                    {
                        child.Focus();
                        return;
                    }
                }

                Buy.Frm_002_MoshahedeTaeed_DarkhastKharid ob = new Buy.Frm_002_MoshahedeTaeed_DarkhastKharid(UserScope.CheckScope(_UserName, "Column11", 27));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem14_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 28))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_003_FaktorKharid")
                    {
                        child.Focus();
                        return;
                    }
                }

                Buy.Frm_003_FaktorKharid ob = new Buy.Frm_003_FaktorKharid(UserScope.CheckScope(_UserName, "Column11", 29));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem15_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 30))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_006_FaktorKharidMarjooi")
                    {
                        child.Focus();
                        return;
                    }
                }

                Buy.Frm_006_FaktorKharidMarjooi ob = new Buy.Frm_006_FaktorKharidMarjooi(UserScope.CheckScope(_UserName, "Column11", 31));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");


        }

        private void buttonItem16_Click(object sender, EventArgs e)
        {

            if (UserScope.CheckScope(_UserName, "Column11", 32))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_001_DaftarForoosh")
                    {
                        child.Focus();
                        return;
                    }
                }

                Sale.Reports.Frm_001_DaftarForoosh ob = new Sale.Reports.Frm_001_DaftarForoosh();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem17_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 33))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_002_AmarMoshtariyan")
                    {
                        child.Focus();
                        return;
                    }
                }

                Sale.Reports.Frm_002_AmarMoshtariyan ob = new Sale.Reports.Frm_002_AmarMoshtariyan();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem18_Click(object sender, EventArgs e)
        {




            if (UserScope.CheckScope(_UserName, "Column11", 34))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_003_AmarForooshKala")
                    {
                        child.Focus();
                        return;
                    }
                }

                Sale.Reports.Frm_003_AmarForooshKala ob = new PSALE.Sale.Reports.Frm_003_AmarForooshKala();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

          
        }

        private void buttonItem19_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 34))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_004_ForooshMah")
                    {
                        child.Focus();
                        return;
                    }
                }

                Sale.Reports.Frm_004_ForooshMah ob = new PSALE.Sale.Reports.Frm_004_ForooshMah();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem22_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 35))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_005_ForooshMahMeghdari")
                    {
                        child.Focus();
                        return;
                    }
                }

                Sale.Reports.Frm_005_ForooshMahMeghdari ob = new PSALE.Sale.Reports.Frm_005_ForooshMahMeghdari();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");


        }

        private void buttonItem20_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 36))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_006_AmarMoshtariyanMarjooi")
                    {
                        child.Focus();
                        return;
                    }
                }

                Sale.Reports.Frm_006_AmarMoshtariyanMarjooi ob = new PSALE.Sale.Reports.Frm_006_AmarMoshtariyanMarjooi();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");


        }

        private void buttonItem21_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 37))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_007_AmarMarjooiKala")
                    {
                        child.Focus();
                        return;
                    }
                }

                Sale.Reports.Frm_007_AmarMarjooiKala ob = new PSALE.Sale.Reports.Frm_007_AmarMarjooiKala();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");


        }

        private void buttonItem23_Click(object sender, EventArgs e)
        {

            if (UserScope.CheckScope(_UserName, "Column11", 38))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_001_DaftarKharid")
                    {
                        child.Focus();
                        return;
                    }
                }

                Buy.Reports.Frm_001_DaftarKharid ob = new Buy.Reports.Frm_001_DaftarKharid();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem24_Click(object sender, EventArgs e)
        {

            if (UserScope.CheckScope(_UserName, "Column11", 39))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_002_AmarMoshtariyan")
                    {
                        child.Focus();
                        return;
                    }
                }

                Buy.Reports.Frm_002_AmarMoshtariyan ob = new Buy.Reports.Frm_002_AmarMoshtariyan();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem25_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 40))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_003_AmarKharidKala")
                    {
                        child.Focus();
                        return;
                    }
                }

                Buy.Reports.Frm_003_AmarKharidKala ob = new Buy.Reports.Frm_003_AmarKharidKala();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem28_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 41))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_004_KharidMahh")
                    {
                        child.Focus();
                        return;
                    }
                }

                Buy.Reports.Frm_004_KharidMahh ob = new Buy.Reports.Frm_004_KharidMahh();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem29_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 42))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_005_KharidMahMeghdari")
                    {
                        child.Focus();
                        return;
                    }
                }

                Buy.Reports.Frm_005_KharidMahMeghdari ob = new Buy.Reports.Frm_005_KharidMahMeghdari();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void buttonItem26_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 43))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_006_AmarForooshandeganMarjooi")
                    {
                        child.Focus();
                        return;
                    }
                }

                Buy.Reports.Frm_006_AmarForooshandeganMarjooi ob = new Buy.Reports.Frm_006_AmarForooshandeganMarjooi();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem27_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 44))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_007_AmarKharidKalaMarjooi")
                    {
                        child.Focus();
                        return;
                    }
                }
                
                Buy.Reports.Frm_007_AmarKharidKalaMarjooi ob = new Buy.Reports.Frm_007_AmarKharidKalaMarjooi();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem30_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 45))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_002_TakhfifEzafeSale")
                    {
                        child.Focus();
                        return;
                    }
                }

                EtelaatePaye.Frm_002_TakhfifEzafeSale ob = new EtelaatePaye.Frm_002_TakhfifEzafeSale(UserScope.CheckScope(_UserName, "Column11", 46));

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem32_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 49))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_001_GardeshMahsoolBaHarMoshtari")
                    {
                        child.Focus();
                        return;
                    }
                }

                foroosh.Reports.Frm_001_GardeshMahsoolBaHarMoshtari ob = new foroosh.Reports.Frm_001_GardeshMahsoolBaHarMoshtari();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem33_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 50))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_002_GardeshMoshtariBaHarMahsool")
                    {
                        child.Focus();
                        return;
                    }
                }

                foroosh.Reports.Frm_002_GardeshMoshtariBaHarMahsool ob = new foroosh.Reports.Frm_002_GardeshMoshtariBaHarMahsool();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem34_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 51))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_003_GardeshMahsoolDarHarOstan")
                    {
                        child.Focus();
                        return;
                    }
                }

                foroosh.Reports.Frm_003_GardeshMahsoolDarHarOstan ob = new foroosh.Reports.Frm_003_GardeshMahsoolDarHarOstan();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem35_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 52))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_004_GardeshOstanBaHarMahsool")
                    {
                        child.Focus();
                        return;
                    }
                }

                foroosh.Reports.Frm_004_GardeshOstanBaHarMahsool ob = new foroosh.Reports.Frm_004_GardeshOstanBaHarMahsool();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem36_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(_UserName, "Column11", 53))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_005_ListNahayGheymat")
                    {
                        child.Focus();
                        return;
                    }
                }

                foroosh.Reports.Frm_005_ListNahayGheymat ob = new foroosh.Reports.Frm_005_ListNahayGheymat();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void buttonItem37_Click(object sender, EventArgs e)
        {

            if (UserScope.CheckScope(_UserName, "Column11", 54))
            {
                foreach (Form child in MdiChildren)
                {
                    if (child.Name == "Frm_006_GozareshTedadi")
                    {
                        child.Focus();
                        return;
                    }
                }

                foroosh.Reports.Frm_006_GozareshTedadi ob = new foroosh.Reports.Frm_006_GozareshTedadi();

                ob.MdiParent = this;
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }



     
    

        

     


  

  
       
      

        

     

    }
}
