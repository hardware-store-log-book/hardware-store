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
    class Authorization
    {
        public static string CheckLogPas(string Login, string Password)
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString.GetConnectionString());
            MySqlCommand cmd;
            MySqlDataReader reader;
            string pass = "";
            string state = "";
            string id = "";
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    cmd = new MySqlCommand(string.Format("select id_store_manager,password from store_manager where login = '{0}'", Login), connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetString(0);
                        pass = reader.GetString(1);
                        state = "manager";
                    }
                    connection.Close();
                    if (reader.HasRows == false)
                    {
                        connection.Open();
                        cmd = new MySqlCommand(string.Format("select id_warehouse_manager,pass from warehouse_manager where login = '{0}'", Login), connection);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            id = reader.GetString(0);
                            pass = reader.GetString(1);
                            state = "warehouse_manager";
                        }
                        connection.Close();
                    }
                }
                if (pass == Password)
                {
                    return state + " " + id;
                }
                else
                {
                    MessageBox.Show("Неверные данные!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Что-то пошло не так! Попробуйте еще раз или обратитесь к администратору!");
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

            return "None";
        }
    }
}
