using System.Windows;
using FridgeInventory;

namespace WPF
{
    /// <summary>
    /// Interaction logic for AddFridgeWindow.xaml
    /// </summary>
    public partial class AddFridgeWindow : Window
    {
        private int OwnerId { get; }
        public AddFridgeWindow(int ownerId)
        {
            InitializeComponent();
            OwnerId = ownerId;
        }

        private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new FridgeContext();
            var fridge = new Fridge(NameTextBox.Text, OwnerId);
            db.Fridge.Add(fridge);
            db.SaveChanges();
            Close();
        }
    }
}
