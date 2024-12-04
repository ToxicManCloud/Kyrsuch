using Kyrsuch;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using System;
using System.Linq;
using System.IO;

public class UserInfoViewModel : ViewModelBase
{
    private User _currentUser;
    private List<User> _users;
    private List<City> _cities;
    private const string UsersFilePath = "users.json";
    private const string CitiesFilePath = "cities.json";

    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    private string _selectedGender;
    public string SelectedGender
    {
        get => _selectedGender;
        set
        {
            if (_selectedGender != value)
            {
                _selectedGender = value;
                OnPropertyChanged(nameof(SelectedGender));
            }
        }
    }


    public City SelectedCity { get; set; }
    public bool IsPasswordVisible { get; set; }
    public bool IsEditing { get; set; }
    public string PasswordDisplay => IsPasswordVisible ? Password : new string('*', Password.Length);
    public ICommand EditCommand { get; set; }
    public ICommand SaveCommand { get; set; }
    public ICommand BackCommand { get; set; }
    public ICommand TogglePasswordVisibilityCommand { get; set; }
    public ICommand GetChatLinkCommand { get; set; }
    public ICommand DeleteProfileCommand { get; set; }
    public string ChatLink { get; set; }
    

    public List<City> Cities
    {
        get => _cities;
        set
        {
            _cities = value;
            OnPropertyChanged(nameof(Cities));
        }
    }

    public UserInfoViewModel(User user)
    {
        _currentUser = user;
        _users = LoadUsers();
        _cities = LoadCities();

        // Инициализация свойств
        Username = _currentUser.Username;
        Password = _currentUser.Password;
        Name = _currentUser.Name;
        Age = _currentUser.Age;
        SelectedGender = _currentUser.SelectedGender;
        SelectedCity = _cities.FirstOrDefault(c => c.CityName == _currentUser.CityName);

        // Инициализация команд
        EditCommand = new RelayCommand(Edit);
        SaveCommand = new RelayCommand(Save);
        BackCommand = new RelayCommand(Back);
        TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);
        GetChatLinkCommand = new RelayCommand(GetChatLink);
        DeleteProfileCommand = new RelayCommand(DeleteProfile);

        IsEditing = false;
    }

    private List<City> LoadCities()
    {
        if (File.Exists(CitiesFilePath))
        {
            try
            {
                string json = File.ReadAllText(CitiesFilePath);
                return JsonConvert.DeserializeObject<List<City>>(json) ?? new List<City>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading cities: " + ex.Message);
                return new List<City>();
            }
        }
        return new List<City>();
    }

    private List<User> LoadUsers()
    {
        if (File.Exists(UsersFilePath))
        {
            try
            {
                string json = File.ReadAllText(UsersFilePath);
                return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
                return new List<User>();
            }
        }
        return new List<User>();
    }

    public void Edit()
    {
        IsEditing = true;
        OnPropertyChanged(nameof(IsEditing));
    }

    public void Save()
    {
        if (string.IsNullOrEmpty(Username) ||
            string.IsNullOrEmpty(Name) ||
            Age < 18 ||
            string.IsNullOrEmpty(SelectedGender) ||
            SelectedCity == null)
        {
            MessageBox.Show("Будь ласка, заповніть всі поля!");
            return;
        }

        var userToUpdate = _users.FirstOrDefault(u => u.Username == _currentUser.Username);
        if (userToUpdate != null)
        {
            userToUpdate.Username = Username;
            userToUpdate.Password = Password;  // Оновлення пароля
            userToUpdate.Name = Name;
            userToUpdate.Age = Age;
            userToUpdate.SelectedGender = SelectedGender;
            userToUpdate.CityName = SelectedCity.CityName;
            userToUpdate.ChatLink = SelectedCity.ChatLink;

            try
            {
                string json = JsonConvert.SerializeObject(_users, Formatting.Indented);
                File.WriteAllText(UsersFilePath, json);
                IsEditing = false;
                OnPropertyChanged(nameof(IsEditing));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving users: " + ex.Message);
            }
        }
        else
        {
            MessageBox.Show("User not found for update.");
        }
    }
    private void DeleteProfile()
    {
        var result = MessageBox.Show("Ви впевнені, що хочете видалити профіль?", "Підтвердження видалення", MessageBoxButton.YesNo);

        if (result == MessageBoxResult.Yes)
        {
           

            var userToDelete = _users.FirstOrDefault(u => u.Username == _currentUser.Username);
            if (userToDelete != null)
            {
                _users.Remove(userToDelete);
            }
            else
            {
                MessageBox.Show("Профіль не знайдено для видалення.");
            }   

            try
            {
                string json = JsonConvert.SerializeObject(_users, Formatting.Indented);
                File.WriteAllText(UsersFilePath, json);
                Back();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при записи файла: " + ex.Message);
            }

        }
    }



    public void Back()
    {
        // Логика для возврата на главное окно
        var mainWindow = new MainWindow();
        mainWindow.Show();
        Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is UserInfoWindow)?.Close();
    }

    public void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
        OnPropertyChanged(nameof(PasswordDisplay));
    }

    public void GetChatLink()
    {
        if (SelectedCity != null)
        {
            ChatLink = SelectedCity.ChatLink;
            OnPropertyChanged(nameof(ChatLink));
        }
        else
        {
            MessageBox.Show("Будь ласка, виберіть місто.");
        }
    }
}
