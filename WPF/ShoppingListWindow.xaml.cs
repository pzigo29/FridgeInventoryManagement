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
using Microsoft.EntityFrameworkCore;

namespace WPF
{
    /// <summary>
    /// Interaction logic for ShoppingListWindow.xaml
    /// </summary>
    public partial class ShoppingListWindow : Window
    {
        private bool IsSortedAscending { get; set; }
        private AddItemToShopListWindow? AddItemWindow { get; set; }
        public ShoppingListWindow()
        {
            InitializeComponent();
            RefreshShoppingListView();
            IsSortedAscending = false;
            Modify.IsEnabled = false;
            Closed += ShoppingListWindow_Closed;
        }

        private void ShoppingListWindow_Closed(object? sender, System.EventArgs e)
        {
            AddItemWindow?.Close();
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not GridViewColumnHeader columnHeader) return;
            var sortPropertyName = columnHeader.Tag.ToString();
            if (sortPropertyName == null) return;
            using var db = new FridgeContext();
            var items = db.FridgeItem.Where(i => i.InShoppingList == true).ToList();

            if (IsSortedAscending)
            {
                IsSortedAscending = false;
                items = items.OrderByDescending(item => item.GetType().GetProperty(sortPropertyName)?.GetValue(item))?.ToList();
                ShoppingListView.ItemsSource = items;
            }
            else
            {
                IsSortedAscending = true;
                items = items.OrderBy(item => item.GetType().GetProperty(sortPropertyName)?.GetValue(item))?.ToList();
                ShoppingListView.ItemsSource = items;
            }
        }

        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            AddItemWindow = new AddItemToShopListWindow();
            AddItemWindow.ItemAdded += AddItemWindow_ItemAdded;
            AddItemWindow.Show();
        }

        private void AddItemWindow_ItemAdded(object sender, EventArgs e)
        {
            RefreshShoppingListView();
        }

        private void RefreshShoppingListView()
        {
            using var db = new FridgeContext();
            var shoppingList = db.FridgeItem.Where(i => i.InShoppingList == true).ToList();
            ShoppingListView.ItemsSource = shoppingList;
        }

        private void ModifyItem_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (FridgeItem)ShoppingListView.SelectedItem;
            AddItemWindow = new AddItemToShopListWindow(item);
            AddItemWindow.ItemAdded += AddItemWindow_ItemAdded;
            
            
            AddItemWindow.Show();
        }

        private void DeleteItem_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete the selected item(s)?", "Delete Item", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;
            using var db = new FridgeContext();
            foreach (FridgeItem item in ShoppingListView.SelectedItems)
            {
                db.FridgeItem.Remove(item);
            }
            db.SaveChanges();
            RefreshShoppingListView();
        }

        private void ShoppingListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListView listView) return;
            Modify.IsEnabled = listView.SelectedItems.Count == 1;
        }
    }
}
