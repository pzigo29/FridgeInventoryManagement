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
    /// Interaction logic for ModifyFridgeWindow.xaml
    /// </summary>
    public partial class ModifyFridgeWindow : Window
    {
        public delegate void FridgeModifiedEventHandler(object sender, EventArgs e);
        public event FridgeModifiedEventHandler? FridgeModified;
        public ObservableCollection<Fridge> Fridges { get; set; }
        public int OwnerId { get; set; }

        public ModifyFridgeWindow(int ownerId)
        {
            InitializeComponent();
            using var db = new FridgeContext();
            OwnerId = ownerId;
            Fridges = new ObservableCollection<Fridge>(db.Fridge.Where(f => f.OwnerId == ownerId));
            DataContext = this;
            
        }

        private void Modify_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new FridgeContext();
            var selectedFridge = FridgeBox.SelectedItem as Fridge;
            var fridge = db.Fridge.Find(selectedFridge?.Id);
            if (fridge == null)
            {
                MessageBox.Show("Fridge not found");
                return;
            }
            fridge.Name = NameTextBox.Text;
            db.SaveChanges();
            FridgeModified?.Invoke(this, EventArgs.Empty);
            Close();
        }
    }
}
