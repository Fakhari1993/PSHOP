using System;
using System.Collections.Generic;
using System.Text;

namespace PSALE
{
    public partial class Class_ChangeConnectionString
    {
        public static string CurrentConnection;
        public void SetConnection(string Connection)
        {
            CurrentConnection = Connection;
        }
    }
}
