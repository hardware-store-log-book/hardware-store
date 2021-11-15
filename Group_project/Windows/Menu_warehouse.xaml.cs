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
    /// Логика взаимодействия для Menu_warehouse.xaml
    /// </summary>
    public partial class Menu_warehouse : Window
    {
        public Menu_warehouse()
        {
            InitializeComponent();
        }

        private void Back_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        //перехад на окно с заказами
        private void Orders_button_Click(object sender, RoutedEventArgs e)
        {
            Order_messageBox messageBox = new Order_messageBox();
            if (messageBox.ShowDialog()==true)
            {

            }
        }

        private void Minimize_button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
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
    }
}
