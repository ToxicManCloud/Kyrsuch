using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Kyrsuch
{
    public class UsersViewModel : BaseViewModel
    {
        public ObservableCollection<User> Users { get; set; }
        public User SelectedUser { get; set; }

        public ICommand DeleteUserCommand { get; }
        public ICommand BackCommand { get; }

        public UsersViewModel()
        {
            // Завантажуємо користувачів з файлу
            Users = new ObservableCollection<User>(LoadUsers());

            DeleteUserCommand = new RelayCommand(DeleteUser, CanDeleteUser);
            BackCommand = new RelayCommand(Back);
        }

        private void DeleteUser()
        {
            // Видалення користувача
            if (SelectedUser != null)
            {
                Users.Remove(SelectedUser);
                var updatedUsers = Users.ToArray();
                SaveUsers(updatedUsers);
            }
        }

        private bool CanDeleteUser()
        {
            return SelectedUser != null;
        }

        private User[] LoadUsers()
        {
            if (!File.Exists("users.json"))
            {
                return new User[0];
            }

            var json = File.ReadAllText("users.json");
            try
            {
                return JsonConvert.DeserializeObject<User[]>(json) ?? new User[0];
            }
            catch
            {
                return new User[0];
            }
        }
        private void Back()
        {
            var adminWindow = new AdminWindow();
            adminWindow.Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is UsersWindow)?.Close();
        }
        private void SaveUsers(User[] users)
        {
            var json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText("users.json", json);
        }
    }
}
