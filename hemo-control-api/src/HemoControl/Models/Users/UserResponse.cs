using System;

namespace HemoControl.Models.Users
{
    public class UserResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public decimal? Weigth { get; set; }
    }
}