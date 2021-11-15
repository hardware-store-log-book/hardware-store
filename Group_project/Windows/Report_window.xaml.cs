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

namespace Group_project.Windows
{
    /// <summary>
    /// Логика взаимодействия для Report_window.xaml
    /// </summary>
    public partial class Report_window : Window
    {
        static MySqlConnection connection = new MySqlConnection(ConnectionString.GetConnectionString());
        static MySqlCommand cmd;
        static MySqlDataReader reader;
        List<reportItem> reportItems = new List<reportItem>();
        string dateFrom = "";
        string dateTo = "";
        string search = "";
        public Report_window()
        {
            InitializeComponent();
            // Выставляем даты в пикерах по умолчанию
            From.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("01.MM.yyyy"));
            To.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString("dd.MM.yyyy"));
        }

        // Обновление данных
        private void Update()
        {
            Order_grid.Items.Clear();
            reportItems.Clear();
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    // Проверка нажатий радиобаттонов
                    if (Last_month.IsChecked==true)
                    {
                        dateFrom = DateTime.Now.ToString("yyyy-MM-01");
                        dateTo = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        dateFrom = From.SelectedDate.Value.ToString("yyyy-MM-dd");
                        dateTo = To.SelectedDate.Value.ToString("yyyy-MM-dd");
                    }
                    string query = string.Format("select product.Article,product.Name_product, manufactures.Name, sum(product.Price), Sum(order_content.Count), Sum(product.Price * order_content.Count) as Summa from product join manufactures on product.manufacturer = manufactures.id join order_content on product.Article = order_content.Article join report_log on report_log.id_order = order_content.id_order where (product.Article like '%{2}%' or product.Name_product like '%{2}%') and date_delivery between '{0}' and '{1}' group by Article",dateFrom,dateTo,search);
                    cmd = new MySqlCommand(query, connection);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reportItem item = new reportItem()
                        {
                            Article = reader.GetString(0),
                            Name = reader.GetString(1),
                            Manufacturer = reader.GetString(2),
                            Price = reader.GetDecimal(3),
                            Count = reader.GetInt32(4),
                            Sum = reader.GetInt32(4) * reader.GetDecimal(3)
                        };
                        reportItems.Add(item);
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

        private void Last_month_Checked(object sender, RoutedEventArgs e)
        {
            From.IsEnabled = false;
            To.IsEnabled = false;
            Update();
        }

        private void Srok_Checked(object sender, RoutedEventArgs e)
        {
            From.IsEnabled = true;
            To.IsEnabled = true;
            Update();
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

        private void Export_button_Click(object sender, RoutedEventArgs e)
        {
            // Смотрите объяснение в окне Order_messageBox
            if (From.SelectedDate.Value < To.SelectedDate.Value)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Выбор файла для сохранения";
                saveFileDialog.Filter = "Word File(*.docx)|*.docx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    decimal sums = 0;
                    int counts = 0;
                    decimal prices = 0;
                    DocX document = DocX.Create(saveFileDialog.FileName);
                    Xceed.Document.NET.Table table = document.AddTable(reportItems.Count + 2, 7);
                    table.Alignment = Alignment.center;
                    table.Design = TableDesign.TableGrid;
                    table.Rows[0].Cells[0].Paragraphs[0].Append("№ п.п.").FontSize(13);
                    table.Rows[0].Cells[1].Paragraphs[0].Append("Артикул").FontSize(13);
                    table.Rows[0].Cells[2].Paragraphs[0].Append("Наименование").FontSize(13);
                    table.Rows[0].Cells[3].Paragraphs[0].Append("Производитель").FontSize(13);
                    table.Rows[0].Cells[4].Paragraphs[0].Append("Цена за ед.").FontSize(13);
                    table.Rows[0].Cells[5].Paragraphs[0].Append("Количество").FontSize(13);
                    table.Rows[0].Cells[6].Paragraphs[0].Append("Общая стоимость").FontSize(13);
                    for (int i = 0; i < reportItems.Count; i++)
                    {
                        table.Rows[i + 1].Cells[0].Paragraphs[0].Append((i + 1).ToString()).FontSize(13);
                        table.Rows[i + 1].Cells[1].Paragraphs[0].Append(reportItems[i].Article).FontSize(13);
                        table.Rows[i + 1].Cells[2].Paragraphs[0].Append(reportItems[i].Name).FontSize(13);
                        table.Rows[i + 1].Cells[3].Paragraphs[0].Append(reportItems[i].Manufacturer).FontSize(13);
                        prices += reportItems[i].Price;
                        counts += reportItems[i].Count;
                        sums += reportItems[i].Price * reportItems[i].Count;
                        table.Rows[i + 1].Cells[4].Paragraphs[0].Append(reportItems[i].Price.ToString()).FontSize(13);
                        table.Rows[i + 1].Cells[5].Paragraphs[0].Append(reportItems[i].Count.ToString()).FontSize(13);
                        table.Rows[i + 1].Cells[6].Paragraphs[0].Append((reportItems[i].Price * reportItems[i].Count).ToString()).FontSize(13);
                    }
                    table.Rows[reportItems.Count + 1].MergeCells(0, 3);
                    table.Rows[reportItems.Count + 1].Cells[0].Paragraphs[0].Append("Итого:").FontSize(13);
                    table.Rows[reportItems.Count + 1].Cells[0].Paragraphs[0].Alignment = Alignment.right;
                    table.Rows[reportItems.Count + 1].Cells[1].Paragraphs[0].Append(prices.ToString()).FontSize(13);
                    table.Rows[reportItems.Count + 1].Cells[2].Paragraphs[0].Append(counts.ToString()).FontSize(13);
                    table.Rows[reportItems.Count + 1].Cells[3].Paragraphs[0].Append(sums.ToString()).FontSize(13);
                    document.InsertParagraph("Сформировал: " + Manager.FIO).Font("Calibri").
                         FontSize(16).
                         Alignment = Xceed.Document.NET.Alignment.right;
                    document.InsertParagraph();
                    document.InsertParagraph();
                    document.InsertParagraph();
                    document.InsertParagraph("Отчет по получению товаров за период: " + Convert.ToDateTime(dateFrom).ToString("dd.MM.yyyy") + " — " + Convert.ToDateTime(dateTo).ToString("dd.MM.yyyy")).Font("Calibri").
                        FontSize(22).
                        Alignment = Xceed.Document.NET.Alignment.center;
                    table.AutoFit = AutoFit.Contents;
                    document.InsertParagraph().InsertTableAfterSelf(table);
                    document.InsertParagraph();
                    document.InsertParagraph();
                    document.InsertParagraph("Дата составления: " + DateTime.Now.ToString("dd.MM.yyyy")).Font("Calibri").
                         FontSize(16).
                         Alignment = Xceed.Document.NET.Alignment.right;
                    document.InsertParagraph();
                    document.PageLayout.Orientation = Xceed.Document.NET.Orientation.Landscape;
                    try
                    {
                        document.Save();
                        MessageBox.Show("Выполнено!");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Что-то пошло не так! Попробуйте еще раз или обратитесь к администратору!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Указан неверный диапазон дат!");
            }
        }

        private void From_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void To_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void Search_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Search_textbox.IsInitialized ==true)
            {
                search = Search_textbox.Text;
                Update();
            }
        }
    }
}
