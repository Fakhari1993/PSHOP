using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;



/// <summary>
/// Summary description for db
/// </summary>
public class db
{
    public static string constr = "";

    public string last_er = "";

    SqlConnection con;
    SqlCommand cmd;
    SqlDataAdapter da;

    public db()
    {
        con = new SqlConnection();
        cmd = new SqlCommand();
        da = new SqlDataAdapter();

        cmd.Connection = con;
        da.SelectCommand = cmd;

    }

    public void Connect()
    {

        try
        {
            con.ConnectionString = constr;
            con.Open();
        }

        catch (Exception ex)
        {
            last_er = "«‘ò«· œ— « ’«· »Â »«‰ò";
        }


    }

    public DataTable get_list(string sql_text)
    {

        cmd.CommandText = sql_text;
        da.SelectCommand = cmd;

        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt;

    }


    public void delete_update_all(string sql_text)
    {

        cmd.CommandText = sql_text;
        cmd.ExecuteNonQuery();

    }

    public Int32 get_count(string sql_text)
    {

        cmd.CommandText = sql_text;
        da.SelectCommand = cmd;
        DataTable dt = new DataTable();
        da.Fill(dt);
        return dt.Rows.Count;

    }


    public string get_one_fiald(string sql_text)
    {
        
        cmd.CommandText = sql_text;
        if (cmd.ExecuteScalar() == DBNull.Value)
            return "0";
        else
            try
            {

                return cmd.ExecuteScalar().ToString();
            }
            catch
            {
                return "";
            }


    }


    public void close()
    {
        con.Close();


    }




    internal void delete_update_all()
    {
        throw new NotImplementedException();
    }
}
