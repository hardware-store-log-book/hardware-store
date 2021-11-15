using Group_project.Classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Group_project.Windows
{
    /// <summary>
    /// Логика взаимодействия для Accept_products.xaml
    /// </summary>
    public partial class Accept_products : Window
    {
        static MySqlConnection connection = new MySqlConnection(ConnectionString.GetConnectionString());
        static MySqlCommand cmd;
        static MySqlDataReader reader;
        List<orderItem> orderItems = new List<orderItem>();
        List<int> ordersID = new List<int>();
        public Accept_products()
        {
            InitializeComponent();
        }

        //Метод обновления данных в гриде
        private void Update()
        {
            //очистка данных в гриде
            orderItems.Clear();
            Order_grid.ItemsSource = null;
            Order_grid.Items.Clear();
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    // Выбираем данные для грида и лейблов на форме
                    string query = string.Format("select order_content.Article, Name_product, Count, Price,Date_order, address,Status from order_content join product on product.Article = order_content.Article join `order` on `order`.id_order = order_content.id_order join address_delivery on address_delivery.id_address_delivery = `order`.address_delivery where `order`.id_order = '{0}'", ordersID[Order_combobox.SelectedIndex]);
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // Заполняем объект для заполнения его в грид
                        orderItem item = new orderItem()
                        {
                            Article = reader.GetString(0),
                            Name = reader.GetString(1),
                            Count = reader.GetInt32(2),
                            Price = reader.GetDecimal(3),
                            Sum = reader.GetInt32(2) * reader.GetDecimal(3)
                        };
                        // Заполняем лейблы
                        Date_label.Content = reader.GetDateTime(4);
                        Address_label.Content = reader.GetString(5);
                        Status_label.Content = reader.GetString(6);
                        //Если заказ одобрен, то его можно принять, иначе нельзя. Блокировка и разблокировка кнопок "Принять" и "Отклонить"
                        if (reader.GetString(6) == "одобрен")
                        {
                            Send_order_button.IsEnabled = true;
                            Decline_order_button.IsEnabled = true;
                        }
                        else
                        {
                            Send_order_button.IsEnabled = false;
                            Decline_order_button.IsEnabled = false;
                        }
                        //заполнение грида и списка с объектами "Заказ"
                        orderItems.Add(item);
                        Order_grid.Items.Add(item);
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

        private void Close_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Minimize_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Back_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Menu_employee window1 = new Menu_employee();
            window1.Show();
            this.Close();
        }

        private void Accept_products_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch
            {

            }
        }
        
        //Заполнение комбобокса
        private void FillOrderBox()
        {
            Order_combobox.Items.Clear();
            ordersID.Clear();
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = string.Format("select id_order,date_order from `order` where address_delivery = '{0}' and (status = 'отправлен на рассмотрение' or status = 'одобрен')", Manager.Address);
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Order_combobox.Items.Add(string.Format("Заказ №{0} от {1}", reader.GetString(0), reader.GetString(1).Split(' ')[0]));
                        ordersID.Add(reader.GetInt32(0));
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

        private void Order_combobox_Loaded(object sender, RoutedEventArgs e)
        {
            FillOrderBox();
        }

        private void Order_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Order_combobox.SelectedIndex!=-1)
            {
                Update();
            }
        }

        private void Send_order_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id_report = 1;
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    // Достаем максимальный ИД из таблицы с отчетами
                    string query = string.Format("select id_report_log from report_log order by id_report_log asc");
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read() && reader.HasRows)
                    {
                        id_report = reader.GetInt32(0) + 1;
                    }
                }
                connection.Close();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    // Заносим отчет в базу
                    string query = string.Format("insert into report_log values('{0}','{1}','{2}')", id_report, ordersID[Order_combobox.SelectedIndex], DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    // Ставим статус доставлен
                    string query = string.Format("update `order` set status = 'доставлен' where id_order = '{0}'", ordersID[Order_combobox.SelectedIndex]);
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
                // Возвращаем все к первоначальному виду
                FillOrderBox();
                Order_grid.Items.Clear();
                Date_label.Content = "";
                Address_label.Content = "";
                Status_label.Content = "";
                Send_order_button.IsEnabled = false;
                Decline_order_button.IsEnabled = false;
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

        private void Decline_order_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    // Ставим статус отклонен
                    string query = string.Format("update `order` set status = 'отослан' where id_order = '{0}'", ordersID[Order_combobox.SelectedIndex]);
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
                FillOrderBox();
                Order_grid.Items.Clear();
                Date_label.Content = "";
                Address_label.Content = "";
                Status_label.Content = "";
                Send_order_button.IsEnabled = false;
                Decline_order_button.IsEnabled = false;
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
    }
}
