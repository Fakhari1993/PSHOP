using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PSALE.Sale
{
    public partial class Frm_003_HavaleDialog : Form
    {
        public int state = 0;
        Decimal mjdi;
        public int code_havale = 0;
        public int id_havale;
        int id_FaktorForoosh;
        db alldatabase = new db();
        public Frm_003_HavaleDialog(int id_faktor)
        {
            id_FaktorForoosh = id_faktor;
            InitializeComponent();
        }

        private void Frm_003_HavaleDialog_Load(object sender, EventArgs e)
        {
            faDatePicker1.SelectedDateTime = DateTime.Now;
            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            multiColumnCombo2.DataSource = alldatabase.get_list("SELECT * FROM Table_001_PWHRS");
            multiColumnCombo1.DataSource = alldatabase.get_list("SELECT * FROM table_005_PwhrsOperation");

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void buttonX1_Click(object sender, EventArgs e)
        {


            if(multiColumnCombo1.SelectedIndex<0)
            {

                MessageBox.Show("لطفا عملکرد انبار را انتخاب نمایید");
                return;
            }
            if (multiColumnCombo2.SelectedIndex < 0)
            {

                MessageBox.Show("لطفا انبار را انتخاب نمایید");
                return;
            }

            if (faDatePicker1.Text == "" || faDatePicker1.Text.Length>10)
            {
                MessageBox.Show("لطفا تاریخ حواله را وارد نمایید");
                return;
            }








            try
            {
                code_havale = Int32.Parse(alldatabase.get_one_fiald("SELECT MAX(column01) FROM Table_007_PwhrsDraft"));
            }
            catch
            {
            }
            code_havale++;

            

            db.constr=Properties.Settings.Default.PSALE_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            string id_pishfaktor=alldatabase.get_one_fiald("SELECT column01 FROM Table_010_SaleFactor WHERE columnid=" + id_FaktorForoosh);
            string id_girande = alldatabase.get_one_fiald("SELECT column03 FROM Table_010_SaleFactor WHERE columnid=" + id_FaktorForoosh);
            string tozihat=alldatabase.get_one_fiald("SELECT column06 FROM Table_010_SaleFactor WHERE columnid=" + id_FaktorForoosh);
            string zmansabt = alldatabase.get_one_fiald("SELECT getdate()");
            DataTable dt = alldatabase.get_list("SELECT * FROM Table_011_Child1_SaleFactor WHERE column01=" + id_FaktorForoosh);
            DataRow dr= dataSet_Sale1.Table_007_PwhrsDraft.NewRow();





            Classes.anbar ob = new PSALE.Classes.anbar();

            db.constr=Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            foreach (DataRow row in dt.Select())
            {
                ob.arzesh(int.Parse(multiColumnCombo2.Value.ToString()), int.Parse(row["column02"].ToString()), ref mjdi).ToString();
                if (mjdi < Decimal.Parse(row["column07"].ToString()))
                {
                   MessageBox.Show(" تعداد درخواستی کالا " + alldatabase.get_one_fiald("SELECT column02 FROM table_004_CommodityAndIngredients WHERE columnid=" + row["column02"]) + " بیش از موجودی انبار می باشد.موجودی انبار برابر با " + mjdi.ToString() + " می باشد ");
                   return;
                }
                

            }







             db.constr=Properties.Settings.Default.PSALE_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();





            if (tozihat == "0") tozihat = "";
            dr["column01"] = code_havale.ToString();
            dr["column02"] = faDatePicker1.Text;
            dr["column03"] = multiColumnCombo2.Value.ToString();
            dr["column04"] = multiColumnCombo1.Value.ToString();
            dr["column05"]=id_girande;
            dr["column06"] = " فاکتور فروش شماره " + alldatabase.get_one_fiald("SELECT column01 FROM Table_010_SaleFactor WHERE columnid="+ id_FaktorForoosh);
            dr["column08"] = Class_BasicOperation._UserName;
            dr["column09"] = zmansabt;
            dr["column10"] = Class_BasicOperation._UserName;
            dr["column11"] = zmansabt;
            dr["column16"] = id_FaktorForoosh.ToString();
            dr["column18"] = id_pishfaktor;


            dataSet_Sale1.Table_007_PwhrsDraft.Rows.Add(dr);

            table_007_PwhrsDraftTableAdapter1.Update(dataSet_Sale1.Table_007_PwhrsDraft);

            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            string id_havalde=alldatabase.get_one_fiald("SELECT columnid FROM Table_007_PwhrsDraft WHERE column01="+code_havale);
            

            
            foreach (DataRow row in dt.Select())
            {
                db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                dr = dataSet_Sale1.Table_008_Child_PwhrsDraft.NewRow();
                dr["column01"] = id_havalde;
                dr["column02"] = row["column02"];
                dr["column03"] = row["column03"];
                dr["column04"] = row["column04"];
                dr["column05"] = row["column05"];
                dr["column06"] = row["column06"];
                dr["column07"] = row["column07"];
                dr["column08"] = row["column08"];
                dr["column09"] = row["column09"];
                dr["column10"] = row["column10"];
                dr["column11"] = row["column20"];
                dr["column12"] = row["column23"];
                dr["column13"] = row["column21"];
                dr["column14"] = row["column22"];
                dr["column15"] = ob.arzesh(int.Parse(multiColumnCombo2.Value.ToString()),int.Parse(row["column02"].ToString()),ref mjdi).ToString();
                dr["column16"] = (decimal.Parse(dr["column07"].ToString()) * decimal.Parse(dr["column15"].ToString())).ToString();



                dr["column17"] = Class_BasicOperation._UserName;
                dr["column18"] = zmansabt;
                dr["column19"] = Class_BasicOperation._UserName;
                dr["column20"] = zmansabt;
                dr["column21"] = row["column12"];
                dr["column22"] = row["column13"];
                dr["column23"] = row["column14"];
                dr["column24"] = row["column15"];
                dr["column25"] = row["columnid"];
                dataSet_Sale1.Table_008_Child_PwhrsDraft.Rows.Add(dr);
                table_008_Child_PwhrsDraftTableAdapter1.Update(dataSet_Sale1.Table_008_Child_PwhrsDraft);


                string child_havale=alldatabase.get_one_fiald("SELECT columnid FROM Table_008_Child_PwhrsDraft WHERE column25="+row["columnid"]);


                db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                string s = "UPDATE Table_011_Child1_SaleFactor SET column26={0},column29={1} WHERE columnid={2}";
                s = string.Format(s, id_havalde, child_havale, row["columnid"].ToString());
                alldatabase.delete_update_all(s);




                
            }

            state = 1;
            id_havale =int.Parse(id_havalde);
            MessageBox.Show(" حواله با شماره " + code_havale + " ذخیره شد ");
            this.Close();


            


        }



    }
}
