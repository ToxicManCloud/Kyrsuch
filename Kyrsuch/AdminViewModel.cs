using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kyrsuch
{
    public class AdminViewModel : BaseViewModel
    {
        public ICommand ViewUsersCommand { get; }
        public ICommand ViewCitiesCommand { get; }
        public ICommand BackCommand { get; }

        public AdminViewModel()
        {
            ViewUsersCommand = new RelayCommand(ViewUsers);
            ViewCitiesCommand = new RelayCommand(ViewCities);
            BackCommand = new RelayCommand(Back);
        }

        private void ViewUsers()
        {
            var usersWindow = new UsersWindow();
            usersWindow.Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AdminWindow)?.Close();
        }

        private void ViewCities()
        {
            var citiesWindow = new CitiesWindow();
            citiesWindow.Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AdminWindow)?.Close();
        }

        private void Back()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AdminWindow)?.Close();
        }
    }
}
