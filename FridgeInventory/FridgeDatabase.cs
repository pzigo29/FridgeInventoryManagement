using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FridgeInventory
{
    public class FridgeDatabase
    {
        public ObservableCollection<Fridge> Fridges { get; } = [];

        public void SaveToJson()
        {
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText("fridge.json", json);
        }

        public static FridgeDatabase? LoadFromJson(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<FridgeDatabase>(json);
        }
    }
}
