using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeInventory
{
    public class Person(
        string firstName,
        string lastName,
        int age,
        string? address = null,
        string? phoneNumber = null,
        string? email = null)
    {
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public int Age { get; set; } = age;
        public string? Address { get; set; } = address;
        public string? PhoneNumber { get; set; } = phoneNumber;
        public string? Email { get; set; } = email;

        public override string ToString()
        {
            return $"FirstName: {FirstName}, LastName: {LastName} Age: {Age}, Address: {Address}, Phone Number: {PhoneNumber}, Email: {Email}";
        }
    }
}
