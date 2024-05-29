using FridgeInventory;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPF
{
    /// <summary>
    /// Interaction logic for DeleteFridgeWindow.xaml
    /// </summary>
    public partial class DeleteFridgeWindow : Window
    {
        public delegate void FridgeDeletedEventHandler(object sender, EventArgs e);
        public event FridgeDeletedEventHandler? FridgeModified;
        public ObservableCollection<Fridge> Fridges { get; set; }
        public int OwnerId { get; set; }
        public DeleteFridgeWindow(int ownerId)
        {
            InitializeComponent();
            using var db = new FridgeContext();
            OwnerId = ownerId;
            Fridges = new ObservableCollection<Fridge>(db.Fridge.Where(f => f.OwnerId == ownerId));
            DataContext = this;
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this fridge?", "Delete Fridge", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
            using var db = new FridgeContext();
            var selectedFridge = FridgeBox.SelectedItem as Fridge;
            var fridge = db.Fridge.Find(selectedFridge?.Id);
            if (fridge == null)
            {
                MessageBox.Show("Fridge not found");
                return;
            }

            FridgeContext.RemoveFridge(fridge.Id);
            FridgeModified?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
