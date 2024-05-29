using System.Windows;
using System.Windows.Controls;
using FridgeInventory;

namespace WPF
{
    /// <summary>
    /// Interaction logic for ShoppingListWindow.xaml
    /// </summary>
    public partial class ShoppingListWindow : Window
    {
        public delegate void ItemBoughtEventHandler(object sender, EventArgs e);
        public event ItemBoughtEventHandler? ItemBought;

        private AddItemToFridgeWindow? AddItemWindow { get; set; }
        private int? OwnerId { get; }
        private List<Fridge> Fridges { get; set; }
        private List<FridgeItem>? SelectedItems { get; set; }
        public ShoppingListWindow()
        {
            InitializeComponent();
            Fridges = [];
            Modify.IsEnabled = false;
            Delete.IsEnabled = false;
            Closed += ShoppingListWindow_Closed;
        }

        public ShoppingListWindow(int? ownerId) : this()
        {
            OwnerId = ownerId;
            RefreshShoppingListView();

        }

        private void ShoppingListWindow_Closed(object? sender, EventArgs e)
        {
            AddItemWindow?.Close();
        }

        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            AddItemWindow = new AddItemToFridgeWindow();
            AddItemWindow.ItemAdded += AddItemWindow_ItemAdded;
            AddItemWindow.InitFridges(OwnerId);
            AddItemWindow.ShopList.IsChecked = true;
            AddItemWindow.ShopList.Visibility = Visibility.Collapsed;
            AddItemWindow.Show();
        }

        private void AddItemWindow_ItemAdded(object sender, EventArgs e)
        {
            RefreshShoppingListView();
        }

        private void RefreshShoppingListView()
        {
            using var db = new FridgeContext();
            Fridges = [.. db.Fridge.Where(i => i.OwnerId == OwnerId)];
            UpdateFridges(db);
        }

        private void UpdateFridges(FridgeContext db)
        {
            foreach (var fridge in Fridges)
            {
                fridge.ItemsList = [.. db.FridgeItem.Where(i => i.FridgeId == fridge.Id && i.InShoppingList == true)];
            }
            ShoppingListView.ItemsSource = Fridges;
        }

        private void ModifyItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems?.Count != 1)
            {
                MessageBox.Show("Please select one item to modify");
                return;
            }
            var item = SelectedItems.First();
            AddItemWindow = new(item)
            {
                Title = "Modify Item",
                AddButton =
                {
                    Content = "Modify",
                    IsDefault = true
                }
            };
            AddItemWindow.InitFridges(OwnerId);
            AddItemWindow.ItemAdded += AddItemWindow_ItemAdded;
            AddItemWindow.ShopList.IsChecked = true;
            AddItemWindow.ShopList.Visibility = Visibility.Collapsed;
            AddItemWindow.Show();
        }

        private void DeleteItem_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete the selected item(s)?", "Delete Item", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;
            using var db = new FridgeContext();
            if (SelectedItems == null) return;
            foreach (FridgeItem item in SelectedItems)
            {
                FridgeContext.RemoveItem(item.Id);
            }
            db.SaveChanges();
            RefreshShoppingListView();
        }

        private void ShoppingListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListView listView) return;
            Modify.IsEnabled = listView.SelectedItems.Count == 1;
            Delete.IsEnabled = listView.SelectedItems.Count > 0;
            SelectedItems = listView.SelectedItems.Cast<FridgeItem>().ToList();
        }

        private void Bought_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not CheckBox bought) return;
            if (bought.DataContext is not FridgeItem item) return;
            item.InShoppingList = !bought.IsChecked;
            using var db = new FridgeContext();
            db.FridgeItem.Update(item);
            db.SaveChanges();
            RefreshShoppingListView();
            ItemBought?.Invoke(this, EventArgs.Empty);
        }
    }
}
