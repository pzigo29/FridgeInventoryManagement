using System.Collections.ObjectModel;
using System.Text.Json;

namespace FridgeInventory
{
    public class Fridge(Person? owner, FridgeInventory? items = null)
    {
        public Person? Owner { get; set; } = owner;
        public FridgeInventory Items { get; init; } = items ?? new FridgeInventory();

        public void SaveToJson()
        {
            var json = JsonSerializer.Serialize(this);
            File.WriteAllText("fridge.json", json);
        }

        public static Fridge? LoadFromJson(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Fridge>(json);
        }

        public void AddItem(FridgeItem item)
        {
            Items.Add(item);
        }

        public void RemoveItem(FridgeItem item)
        {
            Items.Remove(item);
        }

        public override string ToString()
        {
            return $"Owner: {Owner}, Items: {Items}";
        }
    }
}
