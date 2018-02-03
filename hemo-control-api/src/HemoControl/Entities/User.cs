using System;
using System.Collections.Generic;

namespace HemoControl.Entities
{

    public class User
    {

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string LastName { get; private set; }

        public DateTime? Birthday { get; private set; }

        public string Email { get; private set; }

        public decimal Weigth { get; private set; }

        public List<Infusion> Infusions { get; private set; }

        private User()
        {
            // EF
        }

        public User(string name, string lastName, string email, decimal weigth)
        {
            this.Name = name;
            this.LastName = lastName;
            this.ChangeEmail(email);
            this.ChangeWeight(weigth);
            this.Infusions = new List<Infusion>();
        }

        public void ChangeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Deve informar o novo e-mail");
            }

            this.Email = email;
        }

        public void ChangeWeight(decimal weigth)
        {
            if (weigth <= 0)
            {
                throw new ArgumentException("Peso não pode ser menor ou igual a zero");
            }

            this.Weigth = weigth;
        }

        public void ChangeBirthday(DateTime? birthday)
        {
            this.Birthday = birthday;
        }

        public void AddInfusion(Infusion infusion)
        {
            this.Infusions.Add(infusion);
        }
    }
}