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

namespace Kyrsuch
{
    /// <summary>
    /// Логика взаимодействия для RegisterGmailWindow.xaml
    /// </summary>
    public partial class RegisterGmailWindow : Window
    {
        public RegisterGmailViewModel ViewModel { get; set; }

        public RegisterGmailWindow()
        {
            InitializeComponent();
            ViewModel = new RegisterGmailViewModel();
            this.DataContext = ViewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.Password = ((PasswordBox)sender).Password;
            }

        }



        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }



        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
