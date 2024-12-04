using Kyrsuch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Kyrsuch
{
    public class InterestsCollection : List<City>
    {
        // Загрузка интересов из файла
        public static InterestsCollection LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var items = JsonConvert.DeserializeObject<List<City>>(json);
                var collection = new InterestsCollection();
                collection.AddRange(items);
                return collection;
            }

            return new InterestsCollection();
        }

        // Сохранение интересов в файл
        public void SaveToFile(string filePath)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
