using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddItemToFridgeWindow.xaml
    /// </summary>
    public partial class AddItemToFridgeWindow : Window
    {
        public delegate void ItemAddedEventHandler(object sender, EventArgs e);
        public event ItemAddedEventHandler? ItemAdded;
        public int? OwnerId { get; set; }
        public ICollection<Fridge> Fridges { get; set; }
        public FridgeItem? Item { get; set; }
        public ObservableCollection<string> FridgeNames
        {
            get
            {
                var fridgeNames = new ObservableCollection<string>();
                foreach (var fridge in Fridges)
                {
                    fridgeNames.Add(fridge.Name);
                }
                return fridgeNames;
            }
        }


        public AddItemToFridgeWindow()
        {
            InitializeComponent();
            Fridges = [];
            DataContext = this;
        }

        public AddItemToFridgeWindow(FridgeItem item)
        {
            InitializeComponent();
            Fridges = [];
            DataContext = this;
            Item = item;
            ItemName.Text = item.Name;
            Quantity.Text = item.Quantity.ToString();
            ExpiryDate.SelectedDate = item.ExpiryDate;
            Type.SelectedItem = item.Type;
            ShopList.IsChecked = item.InShoppingList;
        }

        public void InitFridges(int? ownerId)
        {
            OwnerId = ownerId;
            using var db = new FridgeContext();
            Fridges = db.Fridge
                .Where(i => i.OwnerId == OwnerId)
                .ToList();
            DataContext = this;
            Fridge.SelectedItem = Fridges.FirstOrDefault(f => f.Id == Item?.FridgeId);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using var db = new FridgeContext();
            var selectedFridge = Fridge.SelectedItem as Fridge;
            var fridge = db.Fridge.FirstOrDefault(i => selectedFridge != null && i.Id == selectedFridge.Id);
            if (fridge == null)
            {
                MessageBox.Show("Fridge not found");
                return;
            }

            if (Item != null)
            {
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
                var item = new FridgeItem(null, ItemName.Text, int.Parse(Quantity.Text), ExpiryDate.SelectedDate, (FridgeItemType)Type.SelectedItem, ShopList.IsChecked);
                fridge.AddItem(item);
                selectedFridge?.ItemsList?.Add(item);
            }
            ItemAdded?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
