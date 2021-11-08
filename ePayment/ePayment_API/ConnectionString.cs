using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePayment_API
{
    public static class ConnectionString
    {
        private static string _connectionstring;

        public static string DBConnectionString
        {
            get
            {
                return _connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
            }
        }
    }
}
