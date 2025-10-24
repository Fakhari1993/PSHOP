using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSALE.foroosh.Reports
{
    public partial class Frm_005_ListNahayGheymat : Form
    {
        public Frm_005_ListNahayGheymat()
        {
            InitializeComponent();
        }

        private void Frm_005_ListNahayGheymat_Load(object sender, EventArgs e)
        {
            FarsiLibrary.Utils.PersianDate ob = new FarsiLibrary.Utils.PersianDate();
            db.constr = Properties.Settings.Default.PERP_ConnectionString;
            db database = new db();
            database.Connect();


          





            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            database.close();
            database.Connect();
            
            gridEX2.DropDowns["d"].DataSource = database.get_list("SELECT columnid,column01,column02 FROM table_004_CommodityAndIngredients");
            gridEX2.DropDowns["d2"].DataSource = database.get_list("SELECT columnid,column01,column02 FROM table_004_CommodityAndIngredients");

            listNahaiiGheymatMahsoolatTableAdapter.Fill(datatSet_Sefareshat.ListNahaiiGheymatMahsoolat, ob.ToString("####/##/##"));
        
        }

      
 
  

        private void bindingNavigatorMoveLastItem1_Click(object sender, EventArgs e)
        {
            try
            {
                listNahaiiGheymatMahsoolatBindingSource.EndEdit();
                listNahaiiGheymatMahsoolatBindingSource.MoveLast();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveNextItem1_Click(object sender, EventArgs e)
        {
            try
            {
                listNahaiiGheymatMahsoolatBindingSource.EndEdit();
                listNahaiiGheymatMahsoolatBindingSource.MoveNext();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMovePreviousItem1_Click(object sender, EventArgs e)
        {
            try
            {
                listNahaiiGheymatMahsoolatBindingSource.EndEdit();
                listNahaiiGheymatMahsoolatBindingSource.MovePrevious();
            }
            catch
            {
            }
        }

        private void bindingNavigatorMoveFirstItem1_Click(object sender, EventArgs e)
        {
            try
            {
                listNahaiiGheymatMahsoolatBindingSource.EndEdit();
                listNahaiiGheymatMahsoolatBindingSource.MoveFirst();
            }
            catch
            {
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FarsiLibrary.Utils.PersianDate ob = new FarsiLibrary.Utils.PersianDate();
            listNahaiiGheymatMahsoolatTableAdapter.Fill(datatSet_Sefareshat.ListNahaiiGheymatMahsoolat, ob.ToString("####/##/##"));
    
        }


    }
}
