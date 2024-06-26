﻿using System.ComponentModel.DataAnnotations;

namespace FridgeInventory
{
    public class Person(
        int id,
        string username,
        string passwordHash,
        string seed,
        string firstName,
        string lastName,
        int age,
        string? address = null,
        string? phoneNumber = null,
        string? email = null,
        bool? admin = null)
    {
        [Key]
        public int Id { get; set; } = id;

        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
        public int Age { get; set; } = age;
        public string? Address { get; set; } = address;
        public string? PhoneNumber { get; set; } = phoneNumber;
        public string? Email { get; set; } = email;
        public string Username { get; set; } = username;
        public string PasswordHash { get; set; } = passwordHash;
        public string Seed { get; set; } = seed;
        public bool? Admin { get; set; } = admin;
    }
}
