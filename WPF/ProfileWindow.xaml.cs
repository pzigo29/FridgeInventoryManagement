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
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
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
    }
}
