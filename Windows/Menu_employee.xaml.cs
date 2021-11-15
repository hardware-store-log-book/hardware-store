using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Menu_employee.xaml
    /// </summary>
    public partial class Menu_employee : Window
    {
        public Menu_employee()
        {
            InitializeComponent();
        }

        private void Employee_button_Click(object sender, RoutedEventArgs e)
        {
            Edit_products window = new Edit_products();
            window.Show();
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
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void Accept_button_Click(object sender, RoutedEventArgs e)
        {
            Accept_products window = new Accept_products();
            window.Show();
            this.Close();
        }

        private void Send_button_Click(object sender, RoutedEventArgs e)
        {
            Ordering_employee window = new Ordering_employee();
            window.Show();
            this.Close();
        }

        private void Report_button_Click(object sender, RoutedEventArgs e)
        {
            Report_window window = new Report_window();
            window.Show();
            this.Close();
        }
    }
}
