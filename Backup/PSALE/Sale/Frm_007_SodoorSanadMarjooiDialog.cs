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
    public partial class Frm_007_SodoorSanadMarjooiDialog : Form
    {
       
        db alldatabase = new db();
        DataTable dt_ezafe = new DataTable();
        DataTable dt_kasr = new DataTable();
        DataTable dt_faktor_h = new DataTable();
        DataTable dt_faktor_d = new DataTable();
        public string codesanad="0";
        int _id_faktor;
        public Frm_007_SodoorSanadMarjooiDialog(int id_faktor)
        {
            _id_faktor = id_faktor;
            InitializeComponent();
        }

        private void Frm_007_SodoorSanadMarjooiDialog_Load(object sender, EventArgs e)
        {
            editBox3.Text = "فروش - صدور سند فاکتور مرجوعی فروش";
            faDatePicker1.SelectedDateTime = DateTime.Now;
            db.constr = Properties.Settings.Default.PACNT_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();

            multiColumnCombo1.DataSource = alldatabase.get_list("SELECT     * FROM         dbo.AllHeaders()");
            multiColumnCombo2.DataSource = alldatabase.get_list("SELECT     * FROM         dbo.AllHeaders()");



            db.constr = Properties.Settings.Default.PSALE_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
            
            dt_kasr = alldatabase.get_list(@"SELECT     dbo.Table_024_Discount.column01 AS name, SUM(dbo.Table_020_Child2_MarjooiSale.column04) AS mablagh, dbo.Table_024_Discount.column05, 
                      dbo.Table_024_Discount.column06, dbo.Table_024_Discount.column07, dbo.Table_024_Discount.column08, dbo.Table_024_Discount.column09, 
                      dbo.Table_024_Discount.column10, dbo.Table_024_Discount.column11, dbo.Table_024_Discount.column12, dbo.Table_024_Discount.column13, 
                      dbo.Table_024_Discount.column14, dbo.Table_024_Discount.column15, dbo.Table_024_Discount.column16, dbo.Table_024_Discount.columnid
FROM         dbo.Table_020_Child2_MarjooiSale INNER JOIN
                      dbo.Table_024_Discount ON dbo.Table_020_Child2_MarjooiSale.column02 = dbo.Table_024_Discount.columnid
WHERE     (dbo.Table_020_Child2_MarjooiSale.column01 = "  + _id_faktor +  @") AND (dbo.Table_024_Discount.column02 = 'True')
GROUP BY dbo.Table_024_Discount.column01, dbo.Table_024_Discount.column05, dbo.Table_024_Discount.column06, dbo.Table_024_Discount.column07, 
                      dbo.Table_024_Discount.column08, dbo.Table_024_Discount.column09, dbo.Table_024_Discount.column10, dbo.Table_024_Discount.column11, 
                      dbo.Table_024_Discount.column12, dbo.Table_024_Discount.column13, dbo.Table_024_Discount.column14, dbo.Table_024_Discount.column15, 
                      dbo.Table_024_Discount.column16, dbo.Table_024_Discount.columnid");


            dt_ezafe = alldatabase.get_list(@"SELECT     dbo.Table_024_Discount.column01 AS name, SUM(dbo.Table_020_Child2_MarjooiSale.column04) AS mablagh, dbo.Table_024_Discount.column05, 
                      dbo.Table_024_Discount.column06, dbo.Table_024_Discount.column07, dbo.Table_024_Discount.column08, dbo.Table_024_Discount.column09, 
                      dbo.Table_024_Discount.column10, dbo.Table_024_Discount.column11, dbo.Table_024_Discount.column12, dbo.Table_024_Discount.column13, 
                      dbo.Table_024_Discount.column14, dbo.Table_024_Discount.column15, dbo.Table_024_Discount.column16, dbo.Table_024_Discount.columnid
FROM         dbo.Table_020_Child2_MarjooiSale INNER JOIN
                      dbo.Table_024_Discount ON dbo.Table_020_Child2_MarjooiSale.column02 = dbo.Table_024_Discount.columnid
WHERE     (dbo.Table_020_Child2_MarjooiSale.column01 = " + _id_faktor + @") AND (dbo.Table_024_Discount.column02 = 'False')
GROUP BY dbo.Table_024_Discount.column01, dbo.Table_024_Discount.column05, dbo.Table_024_Discount.column06, dbo.Table_024_Discount.column07, 
                      dbo.Table_024_Discount.column08, dbo.Table_024_Discount.column09, dbo.Table_024_Discount.column10, dbo.Table_024_Discount.column11, 
                      dbo.Table_024_Discount.column12, dbo.Table_024_Discount.column13, dbo.Table_024_Discount.column14, dbo.Table_024_Discount.column15, 
                      dbo.Table_024_Discount.column16, dbo.Table_024_Discount.columnid");





            dt_faktor_h = alldatabase.get_list("SELECT * FROM Table_018_MarjooiSale WHERE columnid=" + _id_faktor);

        dt_faktor_d = alldatabase.get_list(@"SELECT     dbo.Table_018_MarjooiSale.column03 AS shakhs, dbo.Table_019_Child1_MarjooiSale.column21 AS hazine, dbo.Table_019_Child1_MarjooiSale.column22 AS proje, 
                      dbo.Table_018_MarjooiSale.columnid AS idfaktor, SUM(dbo.Table_019_Child1_MarjooiSale.column20) AS jam
FROM         dbo.Table_019_Child1_MarjooiSale INNER JOIN
                      dbo.Table_018_MarjooiSale ON dbo.Table_019_Child1_MarjooiSale.column01 = dbo.Table_018_MarjooiSale.columnid
GROUP BY dbo.Table_018_MarjooiSale.column03, dbo.Table_019_Child1_MarjooiSale.column21, dbo.Table_019_Child1_MarjooiSale.column22, 
                      dbo.Table_018_MarjooiSale.columnid
HAVING      (dbo.Table_018_MarjooiSale.columnid = " + _id_faktor  +  ")");
            
                    
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
                        string date;

            if (faDatePicker1.SelectedDateTime.ToString() == "")
            {

                date = "";
            }
            else
                date = faDatePicker1.Text;
            try
            {

                string id_person = "0";





                Int64 price = 0;
                int sanadno = 0;

                db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                try
                {
                    price = Int64.Parse(alldatabase.get_one_fiald("SELECT SUM(column20) FROM Table_019_Child1_MarjooiSale WHERE column01=" + _id_faktor));
                      
                }
                catch
                {
                }

                db.constr = Properties.Settings.Default.PACNT_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                try
                {
                    sanadno = Convert.ToInt32(alldatabase.get_one_fiald(@"SELECT     MAX(Column00) AS sanadno FROM         Table_060_SanadHead"));
                }
                catch
                {
                }




                if (date == "")
                {
                    MessageBox.Show("لطفا تاریخ سند را وارد نمایید");
                    return;
                }

                if (editBox3.Text == "")
                {
                    MessageBox.Show("لطفا شرح سند را وارد نمایید");
                    return;
                }

                if (multiColumnCombo1.SelectedIndex == -1 || multiColumnCombo2.SelectedIndex == -1)
                {

                    MessageBox.Show("لطفا سرفصل بدهکار و بستانکار را مشخص نمایید");
                    return;
                }


                Classes.CheckCredits ob = new PSALE.Classes.CheckCredits();
                DataTable TAccounts = new DataTable();
                TAccounts.Columns.Add("Account", Type.GetType("System.String"));
                TAccounts.Columns.Add("Price", Type.GetType("System.Int64"));

                TAccounts.Rows.Add(multiColumnCombo1.Value.ToString(), (price));
                TAccounts.Rows.Add(multiColumnCombo2.Value.ToString(), -(price));

     
                ob.CheckAccountCredit(TAccounts, sanadno);



                string sanadid = "0";
                string s;


                if (radioButton1.Checked == true)
                {
                    sanadno++;

                    db.constr = Properties.Settings.Default.PACNT_ConnectionString;
                    alldatabase.close();
                    alldatabase.Connect();

                    s = @"INSERT INTO Table_060_SanadHead
                      (Column00, Column01, Column02, Column03, Column04, Column05, Column06)
                        VALUES     ({0}, N'{1}', {2}, {3}, N'{4}', N'{5}', {6})";

                    s = string.Format(s, sanadno.ToString(), date, "2", "0", editBox3.Text, Class_BasicOperation._UserName, "getdate()");
                    alldatabase.delete_update_all(s);
                    sanadid = alldatabase.get_one_fiald("SELECT Columnid FROM Table_060_SanadHead WHERE Column00=" + sanadno);
                    
                }
                else if (radioButton2.Checked)
                {
                    editBox2.Text = sanadno.ToString();
                    sanadid = alldatabase.get_one_fiald("SELECT Columnid FROM Table_060_SanadHead WHERE Column00=" + sanadno);
            

                }
                else if (radioButton3.Checked)
                {

                    sanadno = Convert.ToInt32(editBox2.Text);

                    try
                    {
                        sanadid = alldatabase.get_one_fiald("SELECT Columnid FROM Table_060_SanadHead WHERE Column00=" + sanadno);
                    }
                    catch
                    {
                        MessageBox.Show("شماره سند وارد شده معتبر نمی باشد");
                        return;
                    }
                   

                }
               

      
                //
                db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                id_person = alldatabase.get_one_fiald("SELECT column03 FROM Table_018_MarjooiSale WHERE columnid=" + _id_faktor);
                string sharh = " فاکتور مرجوعی فروش شماره " + alldatabase.get_one_fiald("SELECT column01 FROM Table_018_MarjooiSale WHERE columnid=" + _id_faktor);
                db.constr = Properties.Settings.Default.PACNT_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                string moincode = "NULL";
                string tafsilicode = "NULL";
                string jozcode = "NULL";
                string idhazine = "NULL";
                string idproje = "NULL";




                if (multiColumnCombo1.DropDownList.GetValue("MoeinCode").ToString() == "")
                    moincode = "NULL";
                else
                    moincode = multiColumnCombo1.DropDownList.GetValue("MoeinCode").ToString();

                if (multiColumnCombo1.DropDownList.GetValue("TafsiliCode").ToString() == "")
                    tafsilicode = "NULL";
                else
                    tafsilicode = multiColumnCombo1.DropDownList.GetValue("TafsiliCode").ToString();

                if (multiColumnCombo1.DropDownList.GetValue("JozCode").ToString() == "")
                    jozcode = "NULL";
                else
                    jozcode = multiColumnCombo1.DropDownList.GetValue("JozCode").ToString();


            // sabte bedehkar


                s = @"INSERT INTO Table_065_SanadDetail
                      (Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column16, 
                      Column17, Column18, Column19, column20, column21)
                      VALUES     ({0},{1},{2},{3},{4},{5},{6},{7},{8},
                    {9},N'{10}',{11},{12},{13},{14},N'{15}',{16},N'{17}',{18})";




                s = string.Format(s, sanadid.ToString(), multiColumnCombo1.DropDownList.GetValue("ACC_Code").ToString(),
                 multiColumnCombo1.DropDownList.GetValue("GroupCode").ToString(), multiColumnCombo1.DropDownList.GetValue("KolCode").ToString(),
                 moincode, tafsilicode, jozcode, id_person, idhazine, idproje, sharh,
                 "0",price.ToString(), "17", _id_faktor, Class_BasicOperation._UserName, "getdate()", Class_BasicOperation._UserName, "getdate()");

               
                alldatabase.delete_update_all(s);

                //sabte bestankar



                if (multiColumnCombo2.DropDownList.GetValue("MoeinCode").ToString() == "")
                    moincode = "NULL";
                else
                    moincode = multiColumnCombo2.DropDownList.GetValue("MoeinCode").ToString();

                if (multiColumnCombo2.DropDownList.GetValue("TafsiliCode").ToString() == "")
                    tafsilicode = "NULL";
                else
                    tafsilicode = multiColumnCombo2.DropDownList.GetValue("TafsiliCode").ToString();

                if (multiColumnCombo2.DropDownList.GetValue("JozCode").ToString() == "")
                    jozcode = "NULL";
                else
                    jozcode = multiColumnCombo2.DropDownList.GetValue("JozCode").ToString();

                db.constr = Properties.Settings.Default.PSALE_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();

                DataTable dtdetals = new DataTable();
                dtdetals = alldatabase.get_list(@"SELECT     SUM(column20) AS mablagh, column21 AS hazine, column22 AS proje
                FROM         dbo.Table_019_Child1_MarjooiSale
                WHERE     (column01 = " + _id_faktor + @")
                GROUP BY column21, column22");

                string h;
                string p;
                db.constr = Properties.Settings.Default.PACNT_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();






                foreach (DataRow rows in dtdetals.Select())
                {

                    s = @"INSERT INTO Table_065_SanadDetail
                      (Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column16, 
                      Column17, Column18, Column19, column20, column21)
                      VALUES     ({0},{1},{2},{3},{4},{5},{6},{7},{8},
                    {9},N'{10}',{11},{12},{13},{14},N'{15}',{16},N'{17}',{18})";
                    h = rows["hazine"].ToString();
                    p = rows["proje"].ToString();
                    if (h == "") h = "NULL";
                    if (p == "") p = "NULL";


                    s = string.Format(s, sanadid.ToString(), multiColumnCombo2.DropDownList.GetValue("ACC_Code").ToString(),
                     multiColumnCombo2.DropDownList.GetValue("GroupCode").ToString(), multiColumnCombo2.DropDownList.GetValue("KolCode").ToString(),
                     moincode, tafsilicode, jozcode, "NULL",h , p, sharh,
                    rows["mablagh"], "0" ,  "17", _id_faktor, Class_BasicOperation._UserName, "getdate()", Class_BasicOperation._UserName, "getdate()");

                    alldatabase.delete_update_all(s);


                }
















                //bedehkar ezafe
                foreach (DataRow row in dt_ezafe.Select())
                {

                    moincode = "NULL";
                    tafsilicode = "NULL";
                    jozcode = "NULL";
                    idhazine = "NULL";
                    idproje = "NULL";

                    if (row["column07"].ToString() == "")
                        moincode = "NULL";
                    else
                        moincode = row["column07"].ToString();

                    if (row["column08"].ToString() == "")
                        tafsilicode = "NULL";
                    else
                        tafsilicode = row["column08"].ToString();

                    if (row["column09"].ToString() == "")
                        jozcode = "NULL";
                    else
                        jozcode = row["column09"].ToString();

                    s = @"INSERT INTO Table_065_SanadDetail
                      (Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column16, 
                      Column17, Column18, Column19, column20, column21)
                      VALUES     ({0},{1},{2},{3},{4},{5},{6},{7},{8},
                    {9},N'{10}',{11},{12},{13},{14},N'{15}',{16},N'{17}',{18})";

                    s = string.Format(s, sanadid, row["column10"].ToString(),
                     row["column05"].ToString(), row["column06"].ToString(),
                     moincode, tafsilicode, jozcode, id_person, idhazine, idproje, row["name"].ToString(),
                   "0",row["mablagh"].ToString(),  "17", _id_faktor, Class_BasicOperation._UserName, "getdate()", Class_BasicOperation._UserName, "getdate()");



                    alldatabase.delete_update_all(s);


                }


                //bestankar ezafe
                foreach (DataRow row in dt_ezafe.Select())
                {

                     moincode = "NULL";
                     tafsilicode = "NULL";
                     jozcode = "NULL";
                     idhazine = "NULL";
                     idproje = "NULL";

                    if (row["column13"].ToString() == "")
                        moincode = "NULL";
                    else
                        moincode = row["column13"].ToString();

                    if (row["column14"].ToString()=="")
                        tafsilicode = "NULL";
                    else
                        tafsilicode = row["column14"].ToString();

                    if (row["column15"].ToString() == "")
                        jozcode = "NULL";
                    else
                        jozcode = row["column15"].ToString();

                    s = @"INSERT INTO Table_065_SanadDetail
                      (Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column16, 
                      Column17, Column18, Column19, column20, column21)
                      VALUES     ({0},{1},{2},{3},{4},{5},{6},{7},{8},
                    {9},N'{10}',{11},{12},{13},{14},N'{15}',{16},N'{17}',{18})";

                    s = string.Format(s,sanadid , row["column16"].ToString(),
                     row["column11"].ToString(), row["column12"].ToString(),
                     moincode, tafsilicode, jozcode, "NULL", idhazine, idproje, row["name"].ToString(),
                   row["mablagh"].ToString(), "0", "17", _id_faktor, Class_BasicOperation._UserName, "getdate()", Class_BasicOperation._UserName, "getdate()");


               
                    alldatabase.delete_update_all(s);

                
                }





                //bedehka kasr

                foreach (DataRow row in dt_kasr.Select())
                {

                    moincode = "NULL";
                    tafsilicode = "NULL";
                    jozcode = "NULL";
                    idhazine = "NULL";
                    idproje = "NULL";

                    if (row["column07"].ToString() == "")
                        moincode = "NULL";
                    else
                        moincode = row["column07"].ToString();

                    if (row["column08"].ToString() == "")
                        tafsilicode = "NULL";
                    else
                        tafsilicode = row["column08"].ToString();

                    if (row["column09"].ToString() == "")
                        jozcode = "NULL";
                    else
                        jozcode = row["column09"].ToString();

                    s = @"INSERT INTO Table_065_SanadDetail
                      (Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column16, 
                      Column17, Column18, Column19, column20, column21)
                      VALUES     ({0},{1},{2},{3},{4},{5},{6},{7},{8},
                    {9},N'{10}',{11},{12},{13},{14},N'{15}',{16},N'{17}',{18})";

                    s = string.Format(s, sanadid, row["column10"].ToString(),
                     row["column05"].ToString(), row["column06"].ToString(),
                     moincode, tafsilicode, jozcode, "NULL", idhazine, idproje, row["name"].ToString(),"0" ,row["mablagh"].ToString()
                    , "17", _id_faktor, Class_BasicOperation._UserName, "getdate()", Class_BasicOperation._UserName, "getdate()");



                    alldatabase.delete_update_all(s);

                }

                //bestankar kasr

                foreach (DataRow row in dt_kasr.Select())
                {

                    moincode = "NULL";
                    tafsilicode = "NULL";
                    jozcode = "NULL";
                    idhazine = "NULL";
                    idproje = "NULL";

                    if (row["column13"].ToString() == "")
                        moincode = "NULL";
                    else
                        moincode = row["column13"].ToString();

                    if (row["column14"].ToString() == "")
                        tafsilicode = "NULL";
                    else
                        tafsilicode = row["column14"].ToString();

                    if (row["column15"].ToString() == "")
                        jozcode = "NULL";
                    else
                        jozcode = row["column015"].ToString();

                    s = @"INSERT INTO Table_065_SanadDetail
                      (Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column16, 
                      Column17, Column18, Column19, column20, column21)
                      VALUES     ({0},{1},{2},{3},{4},{5},{6},{7},{8},
                    {9},N'{10}',{11},{12},{13},{14},N'{15}',{16},N'{17}',{18})";

                    s = string.Format(s, sanadid, row["column16"].ToString(),
                     row["column11"].ToString(), row["column12"].ToString(),
                     moincode, tafsilicode, jozcode, id_person, idhazine, idproje, row["name"].ToString(), row["mablagh"].ToString(), "0"
                    , "17", _id_faktor, Class_BasicOperation._UserName, "getdate()", Class_BasicOperation._UserName, "getdate()");



                    alldatabase.delete_update_all(s);

                }


                codesanad = sanadno.ToString();

                MessageBox.Show(" سند با شماره " + sanadno.ToString() + " ذخیره شد ");
                this.Close();

            }
            catch(Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, "");
            }


        }

        private void uiGroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                editBox2.Text = "";
                editBox2.Enabled = false;

            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                db.constr = Properties.Settings.Default.PACNT_ConnectionString;
                alldatabase.close();
                alldatabase.Connect();
                try
                {
                    editBox2.Text = alldatabase.get_one_fiald(@"SELECT     MAX(Column00) AS sanadno FROM         Table_060_SanadHead");
                }
                catch
                {
                }
                editBox2.Enabled = false;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {

                editBox2.Enabled = true;
                editBox2.Focus();
            }
        }

        private void editBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
