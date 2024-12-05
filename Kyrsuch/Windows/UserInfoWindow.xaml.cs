using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kyrsuch.Windows
{
    public partial class UserInfoWindow : Window
    {
        public UserInfoWindow(User user)
        {
            InitializeComponent();
            var viewModel = new UserInfoViewModel(user);
            DataContext = viewModel;

            
        }

        // Добавляем метод для обработки изменений в PasswordBox
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            var viewModel = (UserInfoViewModel)DataContext;

            if (passwordBox != null && viewModel != null)
            {
                viewModel.Password = passwordBox.Password;  // Передаем новый пароль в модель представления
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (UserInfoViewModel)DataContext;
            viewModel.Edit();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (UserInfoViewModel)DataContext;
            viewModel.Save();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (UserInfoViewModel)DataContext;
            viewModel.Back();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
