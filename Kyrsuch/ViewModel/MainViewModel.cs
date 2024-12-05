using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using System.IO;
using Kyrsuch.Windows;
using System.Windows;

namespace Kyrsuch
{
    public class MainViewModel : BaseViewModel
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private ICommand _loginCommand;
        private ICommand _registerCommand;
        private ICommand _gmailRegisterCommand;
        private ICommand _exitCommand;
        


        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand
        {
            get
            {
                if (_loginCommand == null)
                    _loginCommand = new RelayCommand(Login);
                return _loginCommand;
            }
        }

        public ICommand RegisterCommand
        {
            get
            {
                if (_registerCommand == null)
                    _registerCommand = new RelayCommand(Register);
                return _registerCommand;
            }
        }

        public ICommand GmailRegisterCommand
        {
            get
            {
                if (_gmailRegisterCommand == null)
                    _gmailRegisterCommand = new RelayCommand(GmailRegister);
                return _gmailRegisterCommand;
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                if (_exitCommand == null)
                    _exitCommand = new RelayCommand(Exit);
                return _exitCommand;
            }
        }

        private void Login()
        {
            var users = LoadUsers();
            var user = users.FirstOrDefault(u => u.Username == Username && u.Password == Password);

            if (Username == "admin" && Password == "admin")
            {
                
                // Open Admin Window
                new AdminWindow().Show();
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is MainWindow)?.Close();
            }
            else if (user != null)
            {
                // Open User Info Window
                
                var userInfoWindow = new UserInfoWindow(user);
                userInfoWindow.Show();
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is MainWindow)?.Close();
            }
            else
            {
                MessageBox.Show("Невірне ім'я користувача або пароль.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Register()
        {
           
            new RegisterWindow().Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is MainWindow)?.Close();
        }

        private void GmailRegister()
        {

            
            // Open Gmail Register Window
            new RegisterGmailWindow().Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is MainWindow)?.Close();

        }
        
        private void Exit()
        {
            Application.Current.Shutdown(); // Close the application
        }

        private List<User> LoadUsers()
        {
            if (!File.Exists("users.json"))
                return new List<User>();

            string json = File.ReadAllText("users.json");
            return JsonConvert.DeserializeObject<List<User>>(json);
        }

    }
}
