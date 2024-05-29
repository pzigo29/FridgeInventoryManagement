using System.Windows;
using FridgeInventory;

namespace WPF
{
    /// <summary>
    /// Interaction logic for InputPasswordWindow.xaml
    /// </summary>
    public partial class InputPasswordWindow : Window
    {
        public delegate void ProfileDeletedEventHandler(object sender, EventArgs e);
        public event ProfileDeletedEventHandler? ProfileDeleted;
        private int OwnerId { get; }
        public InputPasswordWindow(int ownerId)
        {
            InitializeComponent();
            OwnerId = ownerId;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using var db = new FridgeContext();
            var person = db.Person.Find(OwnerId);
            if (person == null)
            {
                MessageBox.Show("Person not found");
                Close();
                return;
            }
            if (FridgeContext.VerifyPassword(PasswordBox.Password, person.Seed, person.PasswordHash))
            {
                var result = MessageBox.Show("Are you sure you want to delete your profile?", "Delete profile", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No) return;
                if (FridgeContext.RemovePerson(OwnerId))
                {
                    MessageBox.Show("Profile deleted");
                    ProfileDeleted?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("Error deleting profile");
                }
                
            }
            else
            {
                MessageBox.Show("Incorrect password");
                return;
            }
            Close();
        }
    }
}
