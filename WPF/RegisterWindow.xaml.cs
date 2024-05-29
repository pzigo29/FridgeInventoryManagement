using System.Windows;
using FridgeInventory;

namespace WPF
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public delegate void RegisterWindowDelegate();
        public event RegisterWindowDelegate? UserAdded;
        public Person? Person { get; set; }
        public RegisterWindow()
        {
            InitializeComponent();
        }

        public RegisterWindow(int ownerId)
        {
            InitializeComponent();
            using var db = new FridgeContext();
            Person = db.Person.Find(ownerId);
            if (Person == null) return;
            UsernameBox.Text = Person.Username;
            FirstNameBox.Text = Person.FirstName;
            LastNameBox.Text = Person.LastName;
            AgeBox.Text = Person.Age.ToString();
            AddressBox.Text = Person.Address ?? "";
            PhoneNumberBox.Text = Person.PhoneNumber ?? "";
            EmailBox.Text = Person.Email ?? "";
        }

        private void Register_OnClick(object sender, RoutedEventArgs e)
        {
            using var db = new FridgeContext();
            if (PasswordBox.Password == PasswordRepeatBox.Password)
            {
                Person? person;
                if (Person != null)
                {
                    person = db.Person.FirstOrDefault(p => p.Username == UsernameBox.Text && p.Id != Person.Id);
                    if (person != null)
                    {
                        MessageBox.Show("Username already exists");
                        return;
                    }
                    RegisterUser();
                    return;
                }
                person = db.Person.FirstOrDefault(p => p.Username == UsernameBox.Text);
                if (person != null)
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
            if (FirstNameBox.Text == "" ||
                LastNameBox.Text == "" || AgeBox.Text == "")
            {
                MessageBox.Show("Please fill in all required fields");
                return;
            }
            if (UsernameBox.Text == "" || PasswordBox.Password == "")
            {
                if (Person == null)
                {
                    MessageBox.Show("Please fill in all required fields");
                    return;
                }
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
            if (age < 0)
            {
                MessageBox.Show("Age must be positive number!");
                return;
            }

            string seed;
            string passwordHash;
            if (PasswordBox.Password != "")
            {
                seed = FridgeContext.GenerateSeed();
                passwordHash = FridgeContext.HashPassword(PasswordBox.Password, seed);
            }
            else
            {
                seed = Person?.Seed ?? "";
                passwordHash = Person?.PasswordHash ?? "";
            }
            
            using var db = new FridgeContext();
            if (Person == null)
            {
                Person = new Person(0, UsernameBox.Text, passwordHash, seed, FirstNameBox.Text, LastNameBox.Text,
                    age, AddressBox.Text, PhoneNumberBox.Text, EmailBox.Text);
                db.Person.Add(Person);
            }
            else
            {
                Person.Username = UsernameBox.Text;
                Person.PasswordHash = passwordHash;
                Person.Seed = seed;
                Person.FirstName = FirstNameBox.Text;
                Person.LastName = LastNameBox.Text;
                Person.Age = age;
                Person.Address = AddressBox.Text;
                Person.PhoneNumber = PhoneNumberBox.Text;
                Person.Email = EmailBox.Text;
                Person.FirstName = FirstNameBox.Text;
                Person.LastName = LastNameBox.Text;
                db.Person.Update(Person);
            }
            db.SaveChanges();
            UserAdded?.Invoke();
            Close();
        }
    }
}
