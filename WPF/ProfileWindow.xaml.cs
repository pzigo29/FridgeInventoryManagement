using System.Windows;
using FridgeInventory;

namespace WPF
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        public delegate void ProfileDeletedEventHandler(object sender, EventArgs e);
        public event ProfileDeletedEventHandler? ProfileDeleted;
        public Person? Person { get; set; }
        public ProfileWindow(int ownerId)
        {
            InitializeComponent();
            DataContext = this;
            using var db = new FridgeContext();
            Person = db.Person.Find(ownerId);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using var db = new FridgeContext();
            var person = db.Person.Find(Person?.Id);
            if (person == null)
            {
                MessageBox.Show("Person not found");
                return;
            }
            person.FirstName = FirstNameBox.Text;
            person.LastName = LastNameBox.Text;
            person.Age = int.Parse(AgeBox.Text);
            person.Address = AddressBox.Text;
            person.PhoneNumber = PhoneNumberBox.Text;
            person.Email = EmailBox.Text;
            db.SaveChanges();
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete your profile?", "Delete profile", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) return;
            using var db = new FridgeContext();
            var person = db.Person.Find(Person?.Id);
            if (person == null) return;
            var passwordWindow = new InputPasswordWindow(person.Id);
            passwordWindow.ProfileDeleted += Close;
            passwordWindow.Show();
            Close();
        }

        private void Close(object sender, EventArgs e)
        {
            ProfileDeleted?.Invoke(this, EventArgs.Empty);
            Close();
        }   
    }
}
