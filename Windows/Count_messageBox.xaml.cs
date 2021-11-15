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
    /// Логика взаимодействия для Count_messageBox.xaml
    /// </summary>
    public partial class Count_messageBox : Window
    {
        public Count_messageBox(string pN)
        {
            InitializeComponent();
            ProductName_label.Text = pN;
        }

        private void OK_button_Click(object sender, RoutedEventArgs e)
        {
            int count;
            // Проверка на нуль и можно ли преобразовать эту строку в число
            if (Count_textbox.Text!="" && Int32.TryParse(Count_textbox.Text,out count))
            {
                if (count > 0)
                {
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Введено неверное число!");
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

        private void Count_textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Ввод только цифр и пробела(пробел на результат не влияет)
            try
            {
                if (!Char.IsNumber(e.Text, 0)) e.Handled = true;
            }
            catch
            {

            }
        }
    }
}
