using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERSDAL.DB
{
    public static class DBConnection
    {
        public static string GetConnectionString()
        {
            string conn = ConfigurationManager.ConnectionStrings["ERS"].ConnectionString;

            return conn;
        }
        public static int GetConnectionTimeOut()
        {
            int result = 0;
            return result;
        }
    }
}
