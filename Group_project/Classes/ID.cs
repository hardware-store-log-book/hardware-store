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
    class ID
    {
        static MySqlConnection connection = new MySqlConnection(ConnectionString.GetConnectionString());
        static MySqlCommand cmd;
        static MySqlDataReader reader;
        public static int getManufacterID(string Name)
        {
            int id = 0;
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = string.Format("select id from manufactures where Name = '{0}'",Name);
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                }
                connection.Close();
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
            return id;
        }

        public static int getAddressID(string Address)
        {
            int id = 0;
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = string.Format("select id_address_delivery from address_delivery where address = '{0}'", Address);
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                }
                connection.Close();
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
            return id;
        }

        public static int getStoreManagerID(string FIO)
        {
            int id = 0;
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = string.Format("select id_store_manager from store_manager where FIO = '{0}'", FIO);
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                }
                connection.Close();
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
            return id;
        }

        public static int getWarehouseManagerID(string FIO)
        {
            int id = 0;
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = string.Format("select id_warehouse_manager from warehouse_manager where FIO = '{0}'", FIO);
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        id = reader.GetInt32(0);
                    }
                }
                connection.Close();
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
            return id;
        }
    }
}
