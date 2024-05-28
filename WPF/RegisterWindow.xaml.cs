using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_OnClick(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == PasswordRepeatBox.Password)
            {
                using var db = new FridgeContext();
                if (db.Person.Any(p => p.Username == UsernameBox.Text))
                {
                    MessageBox.Show("Username already exists");
                    return;
                }
                RegisterUser();
            }
            else
            {
                MessageBox.Show("Passwords do not match");
            }
        }

        private void RegisterUser()
        {
            var seed = GenerateSeed();
            var passwordHash = HashPassword(PasswordBox.Password, seed);
            if (UsernameBox.Text == "" || PasswordBox.Password == "" || FirstNameBox.Text == "" ||
                LastNameBox.Text == "" || AgeBox.Text == "")
            {
                MessageBox.Show("Please fill in all required fields");
                return;
            }
            int age;
            try
            {
                age = int.Parse(AgeBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Age must be number!");
                return;
            }
            var person = new Person(0, UsernameBox.Text, passwordHash, seed, FirstNameBox.Text, LastNameBox.Text,
                age, AddressBox.Text, PhoneNumberBox.Text, EmailBox.Text);
            using var db = new FridgeContext();
            db.Person.Add(person);
            db.SaveChanges();
            Close();
        }

        public static string HashPassword(string password, string salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 100000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);
            return Convert.ToBase64String(hash);
        }

        private string GenerateSeed()
        {
            byte[] seed = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(seed);
            }
            return Convert.ToBase64String(seed);
        }
    }
}
