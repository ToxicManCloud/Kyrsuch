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
        LoadCities(); // Загружаем города из файла
        SaveCommand = new RelayCommand(SaveChanges, CanSaveChanges); // Добавляем CanExecute
        AddCityCommand = new RelayCommand(AddCity);
        DeleteCityCommand = new RelayCommand(DeleteCity, CanDeleteCity); // Добавляем команду для удаления
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
                cityToUpdate.ChatLink = SelectedCity.ChatLink; // Сохраняем изменения
                SaveCities();
                MessageBox.Show("Город обновлен.");
            }
            else
            {
                MessageBox.Show("Город не найден.");
            }
        }
    }

    private bool CanSaveChanges()
    {
        // Команда доступна только если выбран город
        return SelectedCity != null;
    }

    private void AddCity()
    {
        var newCity = new City { CityName = "Новое город", ChatLink = "Новая ссылка" };
        Cities.Add(newCity); // Добавляем город в коллекцию
        SelectedCity = newCity; // Устанавливаем новый город для редактирования
        SaveCities();
        MessageBox.Show("Новый город добавлен.");
    }

    private void DeleteCity()
    {
        if (SelectedCity != null)
        {
            Cities.Remove(SelectedCity); // Удаление города из коллекции
            SaveCities(); // Сохранение изменений в файл
            MessageBox.Show("Город удален.");
        }
        else
        {
            MessageBox.Show("Не выбран город для удаления.");
        }
    }

    private bool CanDeleteCity()
    {
        // Проверяем, выбран ли город
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
