using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FridgeInventory;

namespace WPF
{
    /// <summary>
    /// Interaction logic for AddItemToShopListWindow.xaml
    /// </summary>
    public partial class AddItemToShopListWindow : Window
    {
        public delegate void ItemAddedEventHandler(object sender, EventArgs e);
        public event ItemAddedEventHandler? ItemAdded;
        private FridgeItem? Item { get; }
        public AddItemToShopListWindow()
        {
            InitializeComponent();
            Item = null;
        }

        public AddItemToShopListWindow(FridgeItem item) : this()
        {
            Item = item;
            ItemName.Text = item.Name;
            Quantity.Text = item.Quantity.ToString();
            ExpiryDate.SelectedDate = item.ExpiryDate;
            item.Type = item.Type;
            ShopList.IsChecked = item.InShoppingList;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Item != null)
            {
                using var db = new FridgeContext();
                var item = db.FridgeItem.Find(Item.Id);
                if (item != null)
                {
                    item.Name = ItemName.Text;
                    item.Quantity = int.Parse(Quantity.Text);
                    item.Type = (FridgeItemType)Type.SelectedItem;
                    item.ExpiryDate = ExpiryDate.SelectedDate;
                    item.InShoppingList = ShopList.IsChecked;
                }
                db.SaveChanges();
            }
            else
            {
                var item = new FridgeItem(ItemName.Text, int.Parse(Quantity.Text), (FridgeItemType)Type.SelectedItem, ExpiryDate.SelectedDate, ShopList.IsChecked);
                using var db = new FridgeContext();
                db.FridgeItem.Add(item);
                db.SaveChanges();
            }
            ItemAdded?.Invoke(this, EventArgs.Empty);
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
