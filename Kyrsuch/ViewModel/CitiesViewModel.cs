using Kyrsuch;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

public class CitiesViewModel : BaseViewModel
{
    private City _selectedCity;
    private ObservableCollection<City> _cities;

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

    public ICommand SaveCommand { get; }
    public ICommand AddCityCommand { get; }
    public ICommand DeleteCityCommand { get; }
    public ICommand BackCommand { get; }

    public CitiesViewModel()
    {
        LoadCities(); // Завантажуємо міста з файлу
        SaveCommand = new RelayCommand(SaveChanges, CanSaveChanges); 
        AddCityCommand = new RelayCommand(AddCity);
        DeleteCityCommand = new RelayCommand(DeleteCity, CanDeleteCity); //Додаємо видалення
        BackCommand = new RelayCommand(Back);
    }

    private void LoadCities()
    {
        if (File.Exists("cities.json"))
        {
            var json = File.ReadAllText("cities.json");
            var cities = JsonConvert.DeserializeObject<City[]>(json);
            Cities = new ObservableCollection<City>(cities ?? new City[0]);
        }
        else
        {
            Cities = new ObservableCollection<City>();
        }
    }

    private void SaveChanges()
    {
        if (SelectedCity != null)
        {
            var cityToUpdate = Cities.FirstOrDefault(c => c.CityName == SelectedCity.CityName);
            if (cityToUpdate != null)
            {
                cityToUpdate.ChatLink = SelectedCity.ChatLink; // Зберігаємо зміни
                SaveCities();
                MessageBox.Show("Місто оновлено.");
            }
            else
            {
                MessageBox.Show("Місто не знайдено.");
            }
        }
    }

    private bool CanSaveChanges()
    {
        // Доступ лише колди обране місто
        return SelectedCity != null;
    }

    private void AddCity()
    {
        var newCity = new City { CityName = "Нове місто", ChatLink = "Нове посилання" };
        Cities.Add(newCity); // Додаємо місто в колекцію
        SelectedCity = newCity; // Встановлюємо місто для редагування
        SaveCities();
        MessageBox.Show("Нове місто додано.");
    }

    private void DeleteCity()
    {
        if (SelectedCity != null)
        {
            Cities.Remove(SelectedCity); // Видаляємо місто з колекції
            SaveCities(); // Збереження змін у файл 
            MessageBox.Show("Місто видалено видалено.");
        }
        else
        {
            MessageBox.Show("Оберіть місто для видалення.");
        }
    }

    private bool CanDeleteCity()
    {
        // перевірка на обрання міста
        return SelectedCity != null;
    }

    private void Back()
    {
        var adminWindow = new AdminWindow();
        adminWindow.Show();
        Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is CitiesWindow)?.Close();
    }
    private void SaveCities()
    {
        var cities = Cities.ToArray();
        var json = JsonConvert.SerializeObject(cities, Formatting.Indented);
        File.WriteAllText("cities.json", json);
    }
}
