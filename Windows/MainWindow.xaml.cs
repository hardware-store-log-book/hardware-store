using Group_project.Classes;
using Group_project.Windows;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Group_project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string login = "";
        string password = "";
        MySqlConnection connection = new MySqlConnection(ConnectionString.GetConnectionString());
        MySqlCommand cmd;
        MySqlDataReader reader;
        string ID = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Close_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Minimize_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Запоминаем данные менеджера, который зашел в систему по ID
        private void GetStoreManagetInfo()
        {
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = string.Format("select * from store_manager where id_store_manager = '{0}'", ID);
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Manager.ID = reader.GetInt32(0);
                        Manager.FIO = reader.GetString(1);
                        Manager.Phone = reader.GetString(2);
                        Manager.Gender = reader.GetString(3);
                        Manager.Birthday = reader.GetString(4);
                        Manager.Login = reader.GetString(5);
                        Manager.Password = reader.GetString(6);
                        Manager.Address = reader.GetInt32(7);
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
        }

        // Запоминаем данные заведующего складом, который зашел в систему по ID
        private void GetWarehouseManagetInfo()
        {
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = string.Format("select * from warehouse_manager where id_warehouse_manager = '{0}'", ID);
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        WarehouseManager.ID = reader.GetInt32(0);
                        WarehouseManager.FIO = reader.GetString(1);
                        WarehouseManager.Login = reader.GetString(2);
                        WarehouseManager.Password = reader.GetString(3);
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
        }

        private void Join_button_Click(object sender, RoutedEventArgs e)
        {
            login = Login_textbox.Text;
            if (login == "" & password == "")
            {
                MessageBox.Show("Заполните поля!");
            }
            else if (login != "" & password == "")
            {
                MessageBox.Show("Поле 'Пароль' не должно быть пустым!");
            }
            else if (login == "" & password != "")
            {
                MessageBox.Show("Поле 'Логин' не должно быть пустым!");
            }
            else
            {
                if (Authorization.CheckLogPas(login, password) != "None")
                {
                    // Получаем статус человека, который зашел в систему
                    string response = Authorization.CheckLogPas(login, password).Split(' ')[0];
                    // ID менеджера/заведующего складом
                    ID = Authorization.CheckLogPas(login, password).Split(' ')[1];
                    if (response == "warehouse_manager")
                    {
                        GetWarehouseManagetInfo();
                        Menu_warehouse menu = new Menu_warehouse();
                        menu.Show();
                        this.Close();
                    }
                    else if (response == "manager")
                    {
                        GetStoreManagetInfo();
                        Menu_employee menu = new Menu_employee();
                        menu.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!");
                    }
                }
            }
        }

        // Скрытие/раскрытие пароля
        private void Eye_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Password_textbox.Visibility == Visibility.Visible)
            {
                Password_textbox.Visibility = Visibility.Hidden;
                Password_passwordbox.Visibility = Visibility.Visible;
                password = Password_textbox.Text;
                Password_passwordbox.Password = password;
            }
            else
            {
                Password_textbox.Visibility = Visibility.Visible;
                Password_passwordbox.Visibility = Visibility.Hidden;
                password = Password_passwordbox.Password;
                Password_textbox.Text = password;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch
            {

            }
        }

        private void Password_passwordbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password = Password_passwordbox.Password;
        }

        private void Password_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            password = Password_textbox.Text;
        }
    }
}
