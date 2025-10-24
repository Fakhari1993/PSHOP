using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace PSHOP
{
    public partial class Class_ChangeConnectionString
    {
        public static string CurrentConnection;
        public static string SetConnection(string dbName)
        {
            if (CurrentConnection != null)
            {
                string ConnectionString = null;
                System.Data.SqlClient.SqlConnection Con = new System.Data.SqlClient.SqlConnection(CurrentConnection);
                if (dbName == "PBASE")
                {
                    dbName += "_" + Con.Database.Substring(Con.Database.IndexOf("_") + 1, Con.Database.LastIndexOf("_") - Con.Database.IndexOf("_") - 1);
                    ConnectionString = CurrentConnection.Replace(Con.Database, dbName);
                }
                else if (dbName == "PERP_BASE")
                {
                    ConnectionString = CurrentConnection.Replace(Con.Database, "PERP_BASE");
                }
                else if (dbName != "PERP_MAIN")
                    ConnectionString = CurrentConnection.Replace("PSALE", dbName);
                else
                    ConnectionString = CurrentConnection.Replace(Con.Database, dbName);
                return ConnectionString;
            }
            else
                return null;
        }

    }
}
