using Group_project.Classes;
using Microsoft.Win32;
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
using Xceed.Document.NET;
using Xceed.Words.NET;
using System.Threading;
using System.Windows.Threading;

namespace Group_project.Windows
{
    /// <summary>
    /// Логика взаимодействия для Order_messageBox.xaml
    /// </summary>
    public partial class Order_messageBox : Window
    {
        static MySqlConnection connection = new MySqlConnection(ConnectionString.GetConnectionString());
        static MySqlCommand cmd;
        static MySqlDataReader reader;
        int id = 0;
        decimal sum = 0;
        List<orderItem> orderItems = new List<orderItem>();
        string manager = "";
        string address = "";
        string date = "";
        bool HasRows = false;
        bool HasOrders = true;
        public Order_messageBox()
        {
            InitializeComponent();
        }

        // Обновление данных
        private void Update()
        {
            try
            {
                id = 0;
                sum = 0;
                Order_grid.Items.Clear();
                orderItems.Clear();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = "select id_order,address_delivery.address,date_order, FIO from `order` join address_delivery on address_delivery.id_address_delivery = `order`.address_delivery join store_manager on address_delivery.id_address_delivery = store_manager.id_address where status = 'отправлен на рассмотрение' limit 1";
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // Заполняем лейблы
                        id = reader.GetInt32(0);
                        Address_label.Text = "Магазин \"Advanced Technology\" " + reader.GetString(1);
                        Date_order_label.Text = "Дата заказа: " + reader.GetDateTime(2).ToString("dd-MM-yyyy");
                        Manager_label.Text = "Заказчик: " + reader.GetString(3);
                        address = reader.GetString(1);
                        date = reader.GetString(2);
                        manager = reader.GetString(3);
                    }
                    // Если заказов нет, то все очищаем
                    if (reader.HasRows==false)
                    {
                        Address_label.Text = "";
                        Date_order_label.Text = "";
                        Manager_label.Text = "";
                        sum = 0;
                        Summa_label.Text = string.Format("Итоговая цена: {0} ₽", sum);
                        HasRows = false;
                        HasOrders = false;
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(delegate () { MessageBox.Show("На данный момент заказов нет!"); }));
                    }
                    else
                    {
                        HasRows = true;
                    }
                }
                connection.Close();
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = string.Format("select order_content.Article, Name_product, Count, Price from order_content join product on product.Article = order_content.Article where id_order = '{0}'",id);
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        orderItem item = new orderItem()
                        {
                            Article = reader.GetString(0),
                            Name = reader.GetString(1),
                            Count = reader.GetInt32(2),
                            Price = reader.GetDecimal(3),
                            Sum = reader.GetInt32(2) * reader.GetDecimal(3)
                        };
                        Order_grid.Items.Add(item);
                        orderItems.Add(item);
                        sum = sum + item.Count * item.Price;
                    }
                }
                connection.Close();
                Summa_label.Text = string.Format("Итоговая цена: {0} ₽",sum);
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

        private void Decline_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    string query = string.Format("Update `order` set status = 'отклонен' where id_order = '{0}'",id);
                    cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                connection.Close();
                Update();
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

        private void Accept_button_Click(object sender, RoutedEventArgs e)
        {
            int id_nakl = 1;
            try
            {
                if (HasRows == true)
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        string query = "select id_documents_purchases_invoices from documents_purchases_invoices order by id_documents_purchases_invoices asc";
                        cmd = new MySqlCommand(query, connection);
                        reader = cmd.ExecuteReader();
                        while (reader.Read() && reader.HasRows)
                        {
                            id_nakl = reader.GetInt32(0) + 1;
                        }
                    }
                    connection.Close();
                    connection.Open();
                    string sql = string.Format("insert into documents_purchases_invoices values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", id_nakl, ID.getStoreManagerID(manager), ID.getWarehouseManagerID(WarehouseManager.FIO), ID.getAddressID(address), Convert.ToDateTime(date).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), sum.ToString().Replace(',', '.'));
                    cmd = new MySqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    //Самое страшное
                    //Открываем меню для выбора файла для сохрания
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Title = "Выбор файла для сохранения";
                    saveFileDialog.Filter = "Word File(*.docx)|*.docx";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        decimal sums = 0;
                        int counts = 0;
                        decimal prices = 0;
                        DocX document = DocX.Create(saveFileDialog.FileName); // Создаем документ
                        Xceed.Document.NET.Table table = document.AddTable(orderItems.Count + 2, 6); //Создаем таблицу с размерностью (длина листа+2,6)
                        table.Alignment = Alignment.center; // Ориентация таблицы - центр
                        table.Design = TableDesign.TableGrid; // Дефолт дизайн
                        // Заполняем заголовки
                        table.Rows[0].Cells[0].Paragraphs[0].Append("№ п.п.").FontSize(16);
                        table.Rows[0].Cells[1].Paragraphs[0].Append("Артикул").FontSize(16);
                        table.Rows[0].Cells[2].Paragraphs[0].Append("Название").FontSize(16);
                        table.Rows[0].Cells[3].Paragraphs[0].Append("Цена за ед.").FontSize(16);
                        table.Rows[0].Cells[4].Paragraphs[0].Append("Количество").FontSize(16);
                        table.Rows[0].Cells[5].Paragraphs[0].Append("Сумма").FontSize(16);
                        // Заполняем содержимое ячеек под заголовками
                        for (int i = 0; i < orderItems.Count; i++)
                        {
                            table.Rows[i + 1].Cells[0].Paragraphs[0].Append((i + 1).ToString()).FontSize(16);
                            table.Rows[i + 1].Cells[1].Paragraphs[0].Append(orderItems[i].Article).FontSize(16);
                            table.Rows[i + 1].Cells[2].Paragraphs[0].Append(orderItems[i].Name).FontSize(16);
                            table.Rows[i + 1].Cells[3].Paragraphs[0].Append(orderItems[i].Price.ToString()).FontSize(16);
                            prices += orderItems[i].Price;
                            counts += orderItems[i].Count;
                            sums += orderItems[i].Price * orderItems[i].Count;
                            table.Rows[i + 1].Cells[4].Paragraphs[0].Append(orderItems[i].Count.ToString()).FontSize(16);
                            table.Rows[i + 1].Cells[5].Paragraphs[0].Append((orderItems[i].Price * orderItems[i].Count).ToString()).FontSize(16);
                        }
                        table.Rows[orderItems.Count + 1].MergeCells(0, 2); // Соединяем ненужные нам ячейки
                        // Заполняем нижние строки итого: и тд.
                        table.Rows[orderItems.Count + 1].Cells[0].Paragraphs[0].Append("Итого:").FontSize(16);
                        table.Rows[orderItems.Count + 1].Cells[0].Paragraphs[0].Alignment = Alignment.right;
                        table.Rows[orderItems.Count + 1].Cells[1].Paragraphs[0].Append(prices.ToString()).FontSize(16);
                        table.Rows[orderItems.Count + 1].Cells[2].Paragraphs[0].Append(counts.ToString()).FontSize(16);
                        table.Rows[orderItems.Count + 1].Cells[3].Paragraphs[0].Append(sums.ToString()).FontSize(16);
                        // Вставляем строки
                        document.InsertParagraph("Поставщик: " + manager).Font("Calibri").
                             FontSize(16).
                             Alignment = Xceed.Document.NET.Alignment.right;
                        document.InsertParagraph("Адрес доставки: " + address).Font("Calibri").
                             FontSize(16).
                             Alignment = Xceed.Document.NET.Alignment.right;
                        // Пустые строки - имитация enter
                        document.InsertParagraph();
                        document.InsertParagraph();
                        document.InsertParagraph();
                        document.InsertParagraph("Приходная накладная № " + id_nakl).Font("Calibri").
                            FontSize(24).
                            Alignment = Xceed.Document.NET.Alignment.center;
                        table.AutoFit = AutoFit.Contents; // размер ячеек по содержанию
                        document.InsertParagraph().InsertTableAfterSelf(table); // Вставляем нашу таблицу
                        document.InsertParagraph();
                        document.InsertParagraph();
                        document.InsertParagraph("Получил: " + WarehouseManager.FIO).Font("Calibri").
                        FontSize(16).
                        Alignment = Xceed.Document.NET.Alignment.right;
                        document.InsertParagraph("Дата получения: " + DateTime.Now.ToString("dd-MM-yyyy")).Font("Calibri").
                             FontSize(16).
                             Alignment = Xceed.Document.NET.Alignment.right;
                        document.InsertParagraph();
                        document.PageLayout.Orientation = Xceed.Document.NET.Orientation.Landscape; // альбомная ориантация листа
                        document.Save();
                        connection.Open();
                        if (connection.State == ConnectionState.Open)
                        {
                            string query = string.Format("Update `order` set status = 'одобрен' where id_order = '{0}'", id);
                            cmd = new MySqlCommand(query, connection);
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                        connection.Close();
                        MessageBox.Show("Выполнено!");
                    }
                }
                Update();
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

        private void Order_grid_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}
