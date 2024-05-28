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
            var fridge = new FridgeInventory.Fridge(NameTextBox.Text, OwnerId);
            db.Fridge.Add(fridge);
            db.SaveChanges();
            Close();
        }
    }
}
