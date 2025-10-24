using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace PSALE.Classes
{
  public  class anbar
    {
        db alldatabase = new db();

        public Decimal arzesh(int idanbar, int idkala,ref Decimal mojoodi)
        {
            db.constr = Properties.Settings.Default.PWHRS_ConnectionString;
            alldatabase.close();
            alldatabase.Connect();
           
            SqlParameter Baghimande = new SqlParameter("Baghimande", SqlDbType.Float, 20);
            Baghimande.Direction = ParameterDirection.Output;

            SqlParameter ArzeshVahedout = new SqlParameter("ArzeshVahedout", SqlDbType.Float, 20);
            ArzeshVahedout.Direction = ParameterDirection.Output;

            SqlParameter ArzeshKolout = new SqlParameter("ArzeshKolout", SqlDbType.Float, 20);
            ArzeshKolout.Direction = ParameterDirection.Output;

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Properties.Settings.Default.PWHRS_ConnectionString;
            SqlCommand com = new SqlCommand();
            SqlParameter IdKalaa = new SqlParameter("IdKalaa", SqlDbType.Int, 10);
            IdKalaa.Direction = ParameterDirection.Input;

            SqlParameter CodeAnbarr = new SqlParameter("CodeAnbarr", SqlDbType.Int, 10);
            CodeAnbarr.Direction = ParameterDirection.Input;

            SqlParameter Datetime = new SqlParameter("Datetime", SqlDbType.DateTime, 10);
            Datetime.Direction = ParameterDirection.Input;

            SqlParameter Tarikhh = new SqlParameter("Tarikhh", SqlDbType.NVarChar, 10);
            Tarikhh.Direction = ParameterDirection.Input;

            com.Connection = con;
            con.Open();
            com.CommandText = "AVG";
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.Add(Baghimande);
            com.Parameters.Add(IdKalaa);
            com.Parameters.Add(CodeAnbarr);
            com.Parameters.Add(Datetime);
            com.Parameters.Add(Tarikhh);
            com.Parameters.Add(ArzeshKolout);
            com.Parameters.Add(ArzeshVahedout);

            IdKalaa.Value =idkala;
            CodeAnbarr.Value = idanbar;
            Datetime.Value = alldatabase.get_one_fiald("SELECT getdate()");
            Tarikhh.Value = FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##");
            
            com.ExecuteNonQuery();
            mojoodi = Decimal.Parse(Baghimande.Value.ToString());
          
            if (ArzeshVahedout.Value.ToString() == "")
                return 0;
            else
            return Decimal.Parse(ArzeshVahedout.Value.ToString());
        }

        

    }
}
