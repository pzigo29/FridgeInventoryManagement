using FridgeInventory;
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
            var items = db.FridgeItem.Where(i => i.FridgeId == fridge.Id);
            db.FridgeItem.RemoveRange(items);
            db.SaveChanges();
            db.Fridge.Remove(fridge);
            db.SaveChanges();
            FridgeModified?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
