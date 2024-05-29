using System.Collections.ObjectModel;
using System.Windows;
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

        public AddItemToFridgeWindow(FridgeItem item) : this()
        {
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
            Fridges = [.. db.Fridge.Where(i => i.OwnerId == OwnerId)];
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
                    item.FridgeId = fridge.Id;
                }
                db.SaveChanges();
            }
            else
            {
                try
                {
                    Type.SelectedItem ??= FridgeItemType.Other;
                    var item = new FridgeItem(null, ItemName.Text, int.Parse(Quantity.Text), ExpiryDate.SelectedDate, (FridgeItemType)Type.SelectedItem, ShopList.IsChecked);
                    fridge.AddItem(item);
                    selectedFridge?.ItemsList?.Add(item);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                    return;
                }
            }
            ItemAdded?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
