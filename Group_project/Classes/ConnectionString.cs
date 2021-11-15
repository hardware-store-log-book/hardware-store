using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group_project.Classes
{
    class ConnectionString
    {
        public static string GetConnectionString()
        {
            return "datasource = localhost; database = hardware_store; port = 3306; username = root; password = 12345678";
        }
    }
}
