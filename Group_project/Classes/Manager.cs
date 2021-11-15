using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Group_project.Classes
{
    static class Manager
    {
        public static int ID { get; set; }
        public static string FIO { get; set; }
        public static string Phone { get; set; }
        public static string Gender { get; set; }
        public static string Birthday { get; set; }
        public static string Login { get; set; }
        public static string Password { get; set; }
        public static int Address { get; set; }
    }
}
