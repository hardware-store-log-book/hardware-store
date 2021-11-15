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
    /// Логика взаимодействия для Ordering_employee.xaml
    /// </summary>
    public partial class Ordering_employee : Window
    {
        decimal sum = 0;
        string sort = "name_product";
        string search = "";
        static MySqlConnection connection = new MySqlConnection(ConnectionString.GetConnectionString());
        static MySqlCommand cmd;
        static MySqlDataReader reader;
        List<productItem> productItems = new List<productItem>();
        List<orderItem> orderItems = new List<orderItem>();
        public Ordering_employee()
        {
            InitializeComponent();
        }

        private void Update_order()
        {
            Order_grid.Items.Clear();
            for (int i = 0; i < orderItems.Count; i++)
            {
                orderItem item = new orderItem()
                {
                    Article = orderItems[i].Article,
                    Name = orderItems[i].Name,
                    Count = orderItems[i].Count,
                    Price = orderItems[i].Price,
                    Sum = orderItems[i].Price * orderItems[i].Count
                };
                Order_grid.Items.Add(item);
            }
        }

        // Обновление данных
        private void Update()
        {
            productItems.Clear();
            Products_grid.ItemsSource = null;
            Products_grid.Items.Clear();
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = string.Format("select Article, Name_product, Name, Price,additional_info from product join manufactures on product.manufacturer = manufactures.ID where {0} like '%{1}%'", sort, search);
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        productItem item = new productItem()
                        {
                            Article = reader.GetString(0),
                            Name = reader.GetString(1),
                            Manufacturer = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            Info = reader.GetString(4)
                        };
                        productItems.Add(item);
                    }
                    Products_grid.ItemsSource = productItems;
                }
                connection.Close();
                if (Products_grid.Items.Count == 0)
                {
                    NotFound.Visibility = Visibility.Visible;
                }
                else
                {
                    NotFound.Visibility = Visibility.Hidden;
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
            Menu_employee window = new Menu_employee();
            window.Show();
            this.Close();
        }

        private void Search_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Search_textbox.IsInitialized == true)
            {
                search = Search_textbox.Text;
                Update();
            }
        }

        private void Products_grid_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }


        // Принажатии на ячейку грида
        private void Products_grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Products_grid.SelectedIndex != -1)
            {
                // Вызываем окно с выбором кол-ва товаров
                Count_messageBox messageBox = new Count_messageBox(productItems[Products_grid.SelectedIndex].Name);
                if (messageBox.ShowDialog()==true)
                {
                    bool exists = false; // переменная, для проверки на наличие товара уже в заказе
                    // Заполняем объект
                    orderItem item = new orderItem()
                    {
                        Article = productItems[Products_grid.SelectedIndex].Article,
                        Name = productItems[Products_grid.SelectedIndex].Name,
                        Count = Convert.ToInt32(messageBox.Count_textbox.Text),
                        Price = productItems[Products_grid.SelectedIndex].Price,
                        Sum = productItems[Products_grid.SelectedIndex].Price * Convert.ToInt32(messageBox.Count_textbox.Text)
                    };
                    // ищем, есть ли объект уже в заказе
                    for (int i = 0; i < orderItems.Count; i++)
                    {
                        if (orderItems[i].Article == item.Article)
                        {
                            exists = true;
                            orderItems[i].Count = orderItems[i].Count + item.Count;
                            Update_order();
                        }
                    }
                    sum = sum + productItems[Products_grid.SelectedIndex].Price * item.Count;
                    Summa_label.Text = "Итоговая цена: " + Convert.ToString(sum) + " ₽";
                    if (exists == false)
                    {
                        orderItems.Add(item);
                        Order_grid.Items.Add(item);
                    }
                    Products_grid.SelectedIndex = -1;
                    messageBox.Close();
                }
            }
        }

        private void Sort_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Sort_combobox.IsInitialized == true)
            {
                if (Sort_combobox.SelectedIndex == 0)
                {
                    sort = "name_product";
                }
                else
                {
                    sort = "article";
                }
                Update();
            }
        }

        private void Purchase_invoice_button_Click(object sender, RoutedEventArgs e)
        {
            if (orderItems.Count != 0)
            {
                // Для конструирования запросов
                int max = 0;
                string id_order_content = "values('1'";
                string from = "";
                try
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        string sql = string.Format("select ID_order from `order`");
                        cmd = new MySqlCommand(sql, connection);
                        reader = cmd.ExecuteReader();
                        while (reader.Read() && reader.HasRows)
                        {
                            // Для конструирования запросов
                            max = reader.GetInt32(0);
                            id_order_content = "(select max(ID) + 1";
                            from = "from order_content";
                        }
                    }
                    connection.Close();
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        string sql = string.Format("insert into `order` values ('{0}','{1}','{2}','отправлен на рассмотрение')", max + 1, Manager.Address, DateTime.Now.ToString("yyyy-MM-dd"));
                        cmd = new MySqlCommand(sql, connection);
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                    for (int i = 0; i < orderItems.Count; i++)
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string sql = string.Format("insert into order_content {0},'{1}','{2}','{3}' {4})", id_order_content, max + 1, orderItems[i].Article, orderItems[i].Count, from);
                            id_order_content = "(select max(ID) + 1";
                            from = "from order_content";
                            cmd = new MySqlCommand(sql, connection);
                            cmd.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                    MessageBox.Show("Выполнено!");
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
                Order_grid.Items.Clear();
            }
        }

        private void Clear_button_Click(object sender, RoutedEventArgs e)
        {
            // Очистка всего, что движется
            orderItems.Clear();
            Order_grid.Items.Clear();
            sum = 0;
            Summa_label.Text = "Итоговая цена: 0 ₽";
        }

        private void Delete_button_Click(object sender, RoutedEventArgs e)
        {
            if (Order_grid.SelectedIndex != -1)
            {
                sum = sum - orderItems[Order_grid.SelectedIndex].Price * orderItems[Order_grid.SelectedIndex].Count;
                Summa_label.Text = "Итоговая цена: " + Convert.ToString(sum) + " ₽";
                orderItems.Remove(orderItems[Order_grid.SelectedIndex]);
                Update_order();
            }
        }
    }
}
