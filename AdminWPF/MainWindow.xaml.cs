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

namespace AdminWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            if (UsernameBox.Text == "" || PasswordBox.Password == "") return;
            using var db = new FridgeContext();
            var person = db.Person.FirstOrDefault(p => p.Username == UsernameBox.Text);
            if (person is not { Admin: true })
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
                var adminWindow = new AdminWindow();
                adminWindow.Closed += (o, args) => Show();
                adminWindow.Show();
            }
            else
            {
                MessageBox.Show("Login failed");
            }
        }
    }
}