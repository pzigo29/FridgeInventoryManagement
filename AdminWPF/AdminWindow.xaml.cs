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
using WPF;

namespace AdminWPF
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        
        public List<Person> UsersList { get; set; } = [];
        public RegisterWindow? RegisterWindow { get; set; }
        public AdminWindow()
        {
            InitializeComponent();
            RefreshUsers();
            DataContext = this;
            ModifyUser.IsEnabled = false;
            DeleteUser.IsEnabled = false;
        }

        private void RefreshUsers()
        {
            using var db = new FridgeContext();
            UsersList = [.. db.Person.Where(i => i.Admin != true)];
            UsersView.ItemsSource = UsersList;
        }

        private void AddUser_OnClick(object sender, RoutedEventArgs e)
        {
            RegisterWindow = new RegisterWindow();
            RegisterWindow.UserAdded += RefreshUsers;
            RegisterWindow.Show();
        }

        private void ModifyUser_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new FridgeContext();
            if (UsersView.SelectedItem is not Person selectedPerson) return;
            var person = db.Person.Find(selectedPerson.Id);
            if (person != null)
            {
                RegisterWindow = new RegisterWindow(person.Id)
                {
                    Title = "Modify user"
                };
                RegisterWindow.UserAdded += RefreshUsers;
                RegisterWindow.Show();
            }
        }

        private void DeleteUser_OnClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this user(s)?", "Delete user",
                               MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
            using var db = new FridgeContext();
            UsersView.SelectedItems.Cast<Person>().ToList().ForEach(selectedPerson =>
            {
                var person = db.Person.Find(selectedPerson.Id);
                if (person == null) return;
                db.Person.Remove(person);
            });
            db.SaveChanges();
            RefreshUsers();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListView listView) return;
            ModifyUser.IsEnabled = listView.SelectedItems.Count == 1;
            DeleteUser.IsEnabled = listView.SelectedItems.Count > 0;
        }
    }
}
