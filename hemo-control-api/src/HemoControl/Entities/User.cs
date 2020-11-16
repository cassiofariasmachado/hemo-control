using System;
using System.Collections.Generic;
using HemoControl.Services;

namespace HemoControl.Entities
{
    public class User
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string LastName { get; private set; }
        public DateTime? Birthday { get; private set; }
        public string Email { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public decimal? Weigth { get; private set; }

        private readonly List<Infusion> _infusions = new List<Infusion>();
        public IReadOnlyCollection<Infusion> Infusions => _infusions.AsReadOnly();

        private User() { }

        public User(string name, string lastName, string email, string username, string password)
        {
            Name = name;
            LastName = lastName;
            ChangeEmail(email);
            ChangeUsername(username);
            ChangePassword(password);
        }

        public void ChangeUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required");

            Username = username;
        }

        public void ChangePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            if (password.Length < 8)
                throw new ArgumentException("Password length must be greater than 8");

            Password = new PasswordService().HashPassword(password);
        }

        public void ChangeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            Email = email;
        }

        public void ChangeWeight(decimal weigth)
        {
            if (weigth <= 0)
                throw new ArgumentException("Weight must be greater than zero");

            Weigth = weigth;
        }

        public void ChangeBirthday(DateTime? birthday)
        {
            Birthday = birthday;
        }

        public void AddInfusion(Infusion infusion)
        {
            _infusions.Add(infusion);
        }
    }
}