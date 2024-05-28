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
    /// Interaction logic for FridgeWindow.xaml
    /// </summary>
    public partial class FridgeWindow : Window
    {
        
        public bool IsSortedAscending { get; set; }
        public List<Fridge> Fridges { get; set; }
        public int OwnerId { get; set; }
        public List<FridgeItem>? SelectedItems { get; set; }
        public FridgeWindow(int ownerId)
        {
            InitializeComponent();
            OwnerId = ownerId;
            IsSortedAscending = false;
            RefreshFridges();
            ModifyItem.IsEnabled = false;
            DeleteItem.IsEnabled = false;
            Fridges = new List<Fridge>();
        }


        private void FridgeListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            //if (sender is not GridViewColumnHeader columnHeader) return;
            //var sortPropertyName = columnHeader.Tag.ToString();
            //if (sortPropertyName == null) return;
            //if (IsSortedAscending)
            //{
            //    IsSortedAscending = false;
            //    foreach (var fridge in Fridges)
            //    {
            //        if (fridge.ItemsList != null)
            //            fridge.ItemsList = fridge.ItemsList
            //                .OrderByDescending(item => item.GetType().GetProperty(sortPropertyName)?.GetValue(item))
            //                ?.ToList();
            //    }
            //    FridgesControl.ItemsSource = Fridges;
            //}
            //else
            //{
            //    IsSortedAscending = true;
            //    foreach (var fridge in Fridges)
            //    {
            //        if (fridge.ItemsList != null)
            //            fridge.ItemsList = fridge.ItemsList
            //                .OrderBy(item => item.GetType().GetProperty(sortPropertyName)?.GetValue(item))
            //                ?.ToList();
            //    }
            //    FridgesControl.ItemsSource = Fridges;
            //}
        }


        private void AddItem_OnClick(object sender, RoutedEventArgs e)
        {
            var addItemWindow = new AddItemToFridgeWindow();
            addItemWindow.ItemAdded += AddItemWindow_ItemAdded;
            addItemWindow.InitFridges(OwnerId);
            addItemWindow.ShowDialog();
        }

        private void AddItemWindow_ItemAdded(object sender, EventArgs e)
        {
            RefreshFridges();
        }

        private void RefreshFridges()
        {
            using var db = new FridgeContext();
            Fridges = db.Fridge
                .Where(i => i.OwnerId == OwnerId)
                .ToList();
            //if (Fridges.Count == 0)
            //{
            //    MessageBox.Show("You do not have a fridge yet");
            //    AddFridge(db);
            //}
            UpdateFridges(db);
        }

        private void UpdateFridges(FridgeContext db)
        {
            foreach (var fridge in Fridges)
            {
                fridge.ItemsList = db.FridgeItem.Where(i => i.FridgeId == fridge.Id)?.ToList();
            }

            FridgesControl.ItemsSource = Fridges;
        }

        private void AddFridge(FridgeContext db)
        {
            var addFridgeWindow = new AddFridgeWindow(OwnerId);
            addFridgeWindow.ShowDialog();
            Fridges = db.Fridge
                .Where(i => i.OwnerId == OwnerId)
                .ToList();
        }

        private void ModifyItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems?.Count != 1)
            {
                MessageBox.Show("Please select one item to modify");
                return;
            }
            var item = SelectedItems.First();
            var window = new AddItemToFridgeWindow(item);
            window.ItemAdded += AddItemWindow_ItemAdded;
            window.InitFridges(OwnerId);
            window.Show();
            SelectedItems.RemoveAt(0);
            ModifyItem.IsEnabled = false;
        }

        private void DeleteItem_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete the selected items?", "Delete Items", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
            using var db = new FridgeContext();
            if (SelectedItems == null) return;
            foreach (var selectedItem in SelectedItems)
            {
                db.FridgeItem.Remove(selectedItem);
            }
            db.SaveChanges();
            RefreshFridges();
            DeleteItem.IsEnabled = false;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListView listView) return;
            ModifyItem.IsEnabled = listView.SelectedItems.Count == 1;
            DeleteItem.IsEnabled = listView.SelectedItems.Count > 0;
            SelectedItems = listView.SelectedItems.Cast<FridgeItem>().ToList();
        }

        private void AddFridge_OnClick_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new FridgeContext();
            AddFridge(db);
            UpdateFridges(db);
        }

        private void ModifyFridges_OnClick(object sender, RoutedEventArgs e)
        {
            var modifyFridgeWindow = new ModifyFridgeWindow(OwnerId);
            modifyFridgeWindow.FridgeModified += ModifyFridgeWindow_FridgeModified;
            modifyFridgeWindow.ShowDialog();
        }

        private void ModifyFridgeWindow_FridgeModified(object sender, EventArgs e)
        {
            RefreshFridges();
        }

        private void DeleteFridge_OnClick(object sender, RoutedEventArgs e)
        {
            var deleteFridgeWindow = new DeleteFridgeWindow(OwnerId);
            deleteFridgeWindow.FridgeModified += DeleteFridgeWindow_FridgeModified;
            deleteFridgeWindow.ShowDialog();
        }

        private void DeleteFridgeWindow_FridgeModified(object sender, EventArgs e)
        {
            RefreshFridges();
        }

        private void Profile_OnClick(object sender, RoutedEventArgs e)
        {
            var profileWindow = new ProfileWindow(OwnerId);
            profileWindow.ShowDialog();
        }
    }
}
