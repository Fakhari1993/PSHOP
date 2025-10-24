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
    public partial class Frm_006_ResidDialog : Form
    {
        public int state = 0;
        Decimal mjdi;
        public int code_resid= 0;
        public int id_resid;
        int id_marjooiforoosh;
        db alldatabase = new db();
        public Frm_006_ResidDialog(int id_faktorforooshmarjooi)
        {
            id_marjooiforoosh = id_faktorforooshmarjooi;
            InitializeComponent();
        }

        private void Frm_006_ResidDialog_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_Sale1.Table_012_Child_PwhrsReceipt' table. You can move, or remove it, as needed.
             this.table_011_PwhrsReceiptTableAdapter.Fill(this.dataSet_Sale1.Table_011_PwhrsReceipt);
               this.table_012_Child_PwhrsReceiptTableAdapter.Fill(this.dataSet_Sale1.Table_012_Child_PwhrsReceipt);
            // TODO: This line of code loads data into the 'dataSet_Sale1.Table_011_PwhrsReceipt' table. You can move, or remove it, as needed.
        
            
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
                code_resid = Int32.Parse(alldatabase.get_one_fiald("SELECT MAX(column01) FROM Table_011_PwhrsReceipt"));
            }
            catch
            {
            }
            code_resid++;

            

            db.constr=Properties.Settings.Default.PSALE_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            string id_pishfaktor = alldatabase.get_one_fiald("SELECT column01 FROM Table_018_MarjooiSale WHERE columnid=" + id_marjooiforoosh);
            string id_girande = alldatabase.get_one_fiald("SELECT column03 FROM Table_018_MarjooiSale WHERE columnid=" + id_marjooiforoosh);
            string tozihat = alldatabase.get_one_fiald("SELECT column06 FROM Table_018_MarjooiSale WHERE columnid=" + id_marjooiforoosh);
            string zmansabt = alldatabase.get_one_fiald("SELECT getdate()");
            DataTable dt = alldatabase.get_list("SELECT * FROM Table_019_Child1_MarjooiSale WHERE column01=" + id_marjooiforoosh);
            DataRow dr= dataSet_Sale1.Table_011_PwhrsReceipt.NewRow();






             db.constr=Properties.Settings.Default.PSALE_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();





            if (tozihat == "0") tozihat = "";
            dr["column01"] = code_resid.ToString();
            dr["column02"] = faDatePicker1.Text;
            dr["column03"] = multiColumnCombo2.Value.ToString();
            dr["column04"] = multiColumnCombo1.Value.ToString();
            dr["column05"]=id_girande;
            dr["column06"] = " فاکتور مرجوعی فروش شماره " + alldatabase.get_one_fiald("SELECT column01 FROM Table_018_MarjooiSale WHERE columnid=" + id_marjooiforoosh);
            dr["column08"] = Class_BasicOperation._UserName;
            dr["column09"] = zmansabt;
            dr["column10"] = Class_BasicOperation._UserName;
            dr["column11"] = zmansabt;
            dr["column14"] = id_marjooiforoosh;


            dataSet_Sale1.Table_011_PwhrsReceipt.Rows.Add(dr);

            table_011_PwhrsReceiptTableAdapter.Update(dataSet_Sale1.Table_011_PwhrsReceipt);

            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();

            string id_rsid = alldatabase.get_one_fiald("SELECT columnid FROM Table_011_PwhrsReceipt WHERE column01=" + code_resid);

            Classes.anbar ob = new PSALE.Classes.anbar();
            
            foreach (DataRow row in dt.Select())
            {
                db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                dr = dataSet_Sale1.Table_012_Child_PwhrsReceipt.NewRow();
                dr["column01"] = id_rsid;
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

                dr["column15"] = Class_BasicOperation._UserName;
                dr["column16"] = zmansabt;
                dr["column17"] = Class_BasicOperation._UserName;
                dr["column18"] = zmansabt;


                
                dr["column20"] = ob.arzesh(int.Parse(multiColumnCombo2.Value.ToString()),int.Parse(row["column02"].ToString()),ref mjdi).ToString();

      
                dr["column21"] = (decimal.Parse(dr["column07"].ToString()) * decimal.Parse(dr["column20"].ToString())).ToString();




                dr["column23"] = row["column12"];
                dr["column24"] = row["column13"];
                dr["column25"] = row["column14"];
                dr["column26"] = row["column15"];
                dr["column29"] = row["columnid"];
                dataSet_Sale1.Table_012_Child_PwhrsReceipt.Rows.Add(dr);
                table_012_Child_PwhrsReceiptTableAdapter.Update(dataSet_Sale1.Table_012_Child_PwhrsReceipt);


                string id_child_resid= alldatabase.get_one_fiald("SELECT columnid  FROM Table_012_Child_PwhrsReceipt WHERE column29=" + row["columnid"]);


                db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                string s = "UPDATE Table_019_Child1_MarjooiSale SET column26={0},column29={1} WHERE columnid={2}";
                s = string.Format(s, id_rsid, id_child_resid, row["columnid"].ToString());
                alldatabase.delete_update_all(s);




                
            }

            state = 1;
            id_resid =int.Parse(id_rsid);
            MessageBox.Show(" رسید با شماره " + code_resid + " ذخیره شد ");
            this.Close();


            


        }





    }
}
