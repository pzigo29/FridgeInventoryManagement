using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeInventory
{
    public class FridgeInventory
    {
        public ObservableCollection<FridgeItem> Items { get; } = [];

        internal void Add(FridgeItem item)
        {
            Items.Add(item);
        }

        public void Remove(FridgeItem item)
        {
            Items.Remove(item);
        }
    }
}
