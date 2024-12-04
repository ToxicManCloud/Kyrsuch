﻿using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;

namespace Kyrsuch
{
    public class RegisterGmailViewModel : BaseViewModel
    {
        private const string UsersFilePath = "users.json";
        private const string CitiesFilePath = "cities.json";

        private string _username;
        private string _password;
        private string _confirmPassword;
        private string _name;
        private string _gender;
        private int _age;
        private City _selectedCity;

        private ObservableCollection<City> _cities;

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string SelectedGender
        {
            get { return _gender; }
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    OnPropertyChanged();
                }
            }
        }



        public int Age
        {
            get { return _age; }
            set { _age = value; OnPropertyChanged(); }
        }

        public ObservableCollection<City> Cities
        {
            get { return _cities; }
            set { _cities = value; OnPropertyChanged(); }
        }

        public City SelectedCity
        {
            get { return _selectedCity; }
            set { _selectedCity = value; OnPropertyChanged(); }
        }

        public ICommand RegisterCommand { get; }
        public ICommand BackCommand { get; }

        public RegisterGmailViewModel()
        {
            RegisterCommand = new RelayCommand(Register);
            BackCommand = new RelayCommand(Back);

            LoadCities();
        }

        private void LoadCities()
        {
            if (File.Exists(CitiesFilePath))
            {
                var json = File.ReadAllText(CitiesFilePath);
                var cities = JsonConvert.DeserializeObject<List<City>>(json);
                if (cities != null && cities.Count > 0)
                {
                    Cities = new ObservableCollection<City>(cities);
                    OnPropertyChanged(nameof(Cities));
                }
                else
                {
                    MessageBox.Show("Не знайдено міст у файлі.");
                    Cities = new ObservableCollection<City>();
                }
            }
        }

        private void Register()
        {
            Username = Username.Trim();

            // Перевірка, чи є Username правильним логіном типу gmail.com
            if (!Username.EndsWith("@gmail.com", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Логін повинен бути поштою типу gmail.com");
                return;
            }

            // Перевірка ім'я (тільки літери, без цифр чи спеціальних символів)
            if (string.IsNullOrWhiteSpace(Name) || Name.Any(c => !char.IsLetter(c)))
            {
                MessageBox.Show("Ім'я повинно містити тільки літери.");
                return;
            }

            // Перевірка пароля
            if (Password.Length < 6 || !Password.Any(char.IsLetter) || !Password.Any(char.IsDigit))
            {
                MessageBox.Show("Пароль повинен містити щонайменше 6 символів, включаючи літери та цифри");
                return;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Паролі не співпадають");
                return;
            }
            if (SelectedGender == null)
            {
                MessageBox.Show("Будь ласка оберіть стать");
                return;
            }

            if (SelectedCity == null)
            {
                MessageBox.Show("Будь ласка, виберіть місто");
                return;
            }

            if (Age < 18)
            {
                MessageBox.Show("Ваш вік недостатній для реєстрації");
                return;
            }
            var users = LoadUsers().ToList();
            if (users.Any(u => u.Username == Username))
            {
                MessageBox.Show("Користувач з таким логіном вже існує");
                return;
            }

            var newUser = new User(Username, Password, Name, SelectedGender, Age, SelectedCity.CityName, SelectedCity.ChatLink);
            users.Add(newUser);
            SaveUsers(users.ToArray());

            MessageBox.Show("Реєстрація успішна! Ви можете увійти.");
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is RegisterGmailWindow)?.Close();
        }



        private void Back()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is RegisterGmailWindow)?.Close();
        }

        private User[] LoadUsers()
        {
            if (!File.Exists(UsersFilePath))
            {
                return new User[0];  // Возвращаем пустой массив, если файл не существует
            }

            var json = File.ReadAllText(UsersFilePath);

            // Попытка десериализации в массив
            try
            {
                var users = JsonConvert.DeserializeObject<User[]>(json);
                return users ?? new User[0];
            }
            catch (JsonSerializationException)
            {
                // Если файл содержит один объект пользователя, преобразуем его в массив
                var singleUser = JsonConvert.DeserializeObject<User>(json);
                return singleUser != null ? new[] { singleUser } : new User[0];
            }
        }


        private void SaveUsers(User[] users)
        {
            var json = JsonConvert.SerializeObject(users, Formatting.Indented);  // Сериализуем массив пользователей
            File.WriteAllText(UsersFilePath, json);
        }

    }
}