using System.Windows;
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

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            SettingsWindow?.Close();
            ShoppingListWindow?.Close();
        }

        private void FridgeWindow_Closed(object? sender, EventArgs e)
        {
            Show();
        }

        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            if (UsernameBox.Text == "" || PasswordBox.Password == "") return;
            using var db = new FridgeContext();
            var person = db.Person.FirstOrDefault(p => p.Username == UsernameBox.Text);
            if (person == null || person.Admin == true)
            {
                MessageBox.Show("User not found");
                return;
            }

            if (FridgeContext.VerifyPassword(PasswordBox.Password, person.Seed, person.PasswordHash))
            {
                MessageBox.Show("Login successful");
                Hide();
                PasswordBox.Password = "";
                UsernameBox.Text = "";
                FridgeWindow = new FridgeWindow(person.Id);
                FridgeWindow.Closed += FridgeWindow_Closed;
                FridgeWindow.Show();
            }
            else
            {
                MessageBox.Show("Login failed");
            }
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