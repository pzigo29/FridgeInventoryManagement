using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeInventory
{
    internal class ShoppingList
    {
        public ObservableCollection<FridgeItem> Items { get; } = [];

        public void Add(FridgeItem item)
        {
            Items.Add(item);
        }

        public void Remove(FridgeItem item)
        {
            Items.Remove(item);
        }

        public ObservableCollection<FridgeItem> GetItems(FridgeItemType typeOfItem)
        {
            return new ObservableCollection<FridgeItem>(Items.Where(i => i.Type == typeOfItem));
        }

        public void Print()
        {
            foreach (var item in Items)
            {
                Console.WriteLine(item);
            }
        }
    }
}
