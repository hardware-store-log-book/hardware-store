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
    /// Логика взаимодействия для Edit_products.xaml
    /// </summary>
    public partial class Edit_products : Window
    {
        // Переменные для конструирования запросов
        string filter = "";
        string order = "order by name_product asc";
        string sort = "name_product";
        string search = "";
        int selectedIndex = -1;
        MySqlConnection connection = new MySqlConnection(ConnectionString.GetConnectionString());
        MySqlCommand cmd;
        MySqlDataReader reader;
        List<productItem> productItems = new List<productItem>();

        public Edit_products()
        {
            InitializeComponent();
        }

        // Заполнение комбобокса с производителями
        private void FillManufacters(ComboBox comboBox)
        {
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "select Name from manufactures";
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBox.Items.Add(reader.GetString(0));
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
                    string query = string.Format("select Article, Name_product, Name, Price,additional_info from product join manufactures on product.manufacturer = manufactures.ID where {0} like '%{1}%' {2} {3}", sort, search, filter, order);
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
                    // Указываем список как ресурс для грида
                    Products_grid.ItemsSource = productItems;
                }
                connection.Close();
                // Если ничего не нашлось, то выведется запись
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

        private void Add_button_Click(object sender, RoutedEventArgs e)
        {
            if (Article_textbox.Text != "" && NameProduct_textbox.Text != "" && Manufacturer_ComboBox.Text != "" && Price_textbox.Text != "")
            {
                decimal price = 0;
                if (Decimal.TryParse(Price_textbox.Text, out price))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string query = string.Format("insert into product values('{0}','{1}','{2}','{3}','{4}')", Article_textbox.Text, NameProduct_textbox.Text, ID.getManufacterID(Manufacturer_ComboBox.Text), Price_textbox.Text.Replace(',', '.'), Info_textbox.Text);
                            cmd = new MySqlCommand(query, connection);
                            cmd.ExecuteNonQuery();
                        }
                        connection.Close();
                        Update();
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
                }
                else
                {
                    MessageBox.Show("Неверный формат числа в поле 'Цена'");
                }
            }
            else
            {
                MessageBox.Show("Необходимо заполнить все поля!");
            }
        }

        private void Delete_button_Click(object sender, RoutedEventArgs e)
        {
            if (Article_textbox.Text != "" && NameProduct_textbox.Text != "" && Manufacturer_ComboBox.Text != "" && Price_textbox.Text != "")
            {
                decimal price = 0;
                if (Decimal.TryParse(Price_textbox.Text, out price))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string query = string.Format("delete from product where article = '{0}' and name_product = '{1}' and Manufacturer = '{2}' and Price = '{3}' and Additional_info = '{4}'", Article_textbox.Text, NameProduct_textbox.Text, ID.getManufacterID(Manufacturer_ComboBox.Text), Price_textbox.Text.Replace(',', '.'), Info_textbox.Text);
                            cmd = new MySqlCommand(query, connection);
                            cmd.ExecuteNonQuery();
                        }
                        connection.Close();
                        Update();
                        Article_textbox.Text = "";
                        NameProduct_textbox.Text = "";
                        Manufacturer_ComboBox.Text = "";
                        Price_textbox.Text = "";
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
                }
                else
                {
                    MessageBox.Show("Неверный формат числа в поле 'Цена'");
                }
            }
            else
            {
                MessageBox.Show("Необходимо заполнить все поля!");
            }
        }

        private void Edit_button_Click(object sender, RoutedEventArgs e)
        {
            if (Article_textbox.Text != "" && NameProduct_textbox.Text != "" && Manufacturer_ComboBox.Text != "" && Price_textbox.Text != "")
            {
                decimal price = 0;
                if (Decimal.TryParse(Price_textbox.Text, out price))
                {
                    try
                    {
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string query = string.Format("Update product set article = '{0}', name_product = '{1}', manufacturer = '{2}', price = '{3}',additional_info = '{4}' where article = '{5}'", Article_textbox.Text, NameProduct_textbox.Text, ID.getManufacterID(Manufacturer_ComboBox.Text), Price_textbox.Text.Replace(',', '.'), Info_textbox.Text, productItems[selectedIndex].Article);
                            cmd = new MySqlCommand(query, connection);
                            cmd.ExecuteNonQuery();
                        }
                        connection.Close();
                        Update();
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
                }
                else
                {
                    MessageBox.Show("Неверный формат числа в поле 'Цена'");
                }
            }
            else
            {
                MessageBox.Show("Необходимо заполнить все поля!");
            }
        }

        private void Clear_button_Click(object sender, RoutedEventArgs e)
        {
            selectedIndex = -1;
            Article_textbox.Text = "";
            NameProduct_textbox.Text = "";
            Manufacturer_ComboBox.Text = "";
            Price_textbox.Text = "";
            Info_textbox.Text = "";
        }

        private void Search_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Search_textbox.IsInitialized == true)
            {
                search = Search_textbox.Text;
                Update();
            }
        }

        private void Asc_Checked(object sender, RoutedEventArgs e)
        {
            if (Asc.IsInitialized == true)
            {
                order = "order by name_product asc";
                Update();
            }
        }

        private void Desc_Checked(object sender, RoutedEventArgs e)
        {
            if (Desc.IsInitialized == true)
            {
                order = "order by name_product desc";
                Update();
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

        private void Filter_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Filter_combobox.IsInitialized == true)
            {
                if (Filter_combobox.SelectedValue != null)
                {
                    if (Filter_combobox.SelectedValue.ToString() == "")
                    {
                        Filter_combobox.SelectedIndex = 0;
                    }
                    else if (Filter_combobox.SelectedValue.ToString() == "System.Windows.Controls.ComboBoxItem: Все производители")
                    {
                        filter = "";
                    }
                    else
                    {
                        filter = string.Format("and Name = '{0}'", Filter_combobox.SelectedValue);
                    }
                    Update();
                }
                else if(Filter_combobox.SelectedValue == null)
                {
                    filter = "and 1!=1";
                    Update();
                }
            }
        }

        private void Filter_combobox_Loaded(object sender, RoutedEventArgs e)
        {
            FillManufacters(Filter_combobox);
        }

        private void Products_grid_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void Products_grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Products_grid.SelectedIndex !=-1)
            {
                selectedIndex = Products_grid.SelectedIndex;
                Article_textbox.Text = productItems[Products_grid.SelectedIndex].Article;
                NameProduct_textbox.Text = productItems[Products_grid.SelectedIndex].Name;
                Manufacturer_ComboBox.Text = productItems[Products_grid.SelectedIndex].Manufacturer;
                Price_textbox.Text = productItems[Products_grid.SelectedIndex].Price.ToString();
                Info_textbox.Text = productItems[Products_grid.SelectedIndex].Info;
            }
        }

        private void Manufacturer_ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            FillManufacters(Manufacturer_ComboBox);
        }
    }
}
