using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FridgeInventory;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsWindow? SettingsWindow { get; set; }
        private ShoppingListWindow? ShoppingListWindow { get; set; }
        private RegisterWindow? RegisterWindow { get; set; }
        private FridgeWindow? FridgeWindow { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Closed += MainWindow_Closed;
        }

        private void Settings_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow = new SettingsWindow();
            SettingsWindow.Show();
        }

        private void MainWindow_Closed(object? sender, System.EventArgs e)
        {
            SettingsWindow?.Close();
            ShoppingListWindow?.Close();
        }

        private void FridgeWindow_Closed(object? sender, EventArgs e)
        {
            Show();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ShoppingListWindow = new ShoppingListWindow();
            ShoppingListWindow.Show();
        }

        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            if (UsernameBox.Text == "" || PasswordBox.Password == "") return;
            using var db = new FridgeContext();
            var person = db.Person.FirstOrDefault(p => p.Username == UsernameBox.Text);
            if (person == null)
            {
                MessageBox.Show("User not found");
                return;
            }

            if (VerifyPassword(PasswordBox.Password, person.Seed, person.PasswordHash))
            {
                MessageBox.Show("Login successful");
                Hide();
                PasswordBox.Password = "";
                UsernameBox.Text = "";
                FridgeWindow = new FridgeWindow(person.Id); // Open fridge window
                FridgeWindow.Closed += FridgeWindow_Closed;
                FridgeWindow.Show();
            }
            else
            {
                MessageBox.Show("Login failed");
            }
        }


        private bool VerifyPassword(string enteredPassword, string storedSalt, string storedHash)
        {
            var enteredHash = RegisterWindow.HashPassword(enteredPassword, storedSalt);
            return enteredHash == storedHash;
        }

        private void Register_OnClick(object sender, RoutedEventArgs e)
        {
            RegisterWindow = new RegisterWindow();
            Hide();
            RegisterWindow.ShowDialog();
            Show();
        }
    }
}