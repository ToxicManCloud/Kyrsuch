using Kyrsuch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kyrsuch
{
    public class CityCollection : List<City>
    {
        // Завантаження з файлу міста
        public static CityCollection LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var items = JsonConvert.DeserializeObject<List<City>>(json);
                var collection = new CityCollection();
                collection.AddRange(items);
                return collection;
            }

            return new CityCollection();
        }

        // Збереження файлів в міста
        public void SaveToFile(string filePath)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
