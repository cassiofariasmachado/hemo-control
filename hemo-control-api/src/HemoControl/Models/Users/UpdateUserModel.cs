using System;

namespace HemoControl.Models.Users
{
    public class UpdateUserModel
    {
        public DateTime? Birthday { get; set; }

        public string Email { get; set; }

        public decimal Weigth { get; set; }
    }
}