using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrsuch
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string SelectedGender { get; set; }
        public int Age { get; set; }
        public string CityName { get; set; }
        public string ChatLink { get; set; }

        public User(string username, string password, string name, string gender, int age, string cityName, string chatLink)
        {
            Username = username;
            Password = password;
            Name = name;
            SelectedGender = gender;
            Age = age;
            CityName = cityName;
            ChatLink = chatLink;
        }
    }


}
