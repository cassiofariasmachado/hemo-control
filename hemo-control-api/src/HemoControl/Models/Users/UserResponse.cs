using System;
using HemoControl.Entities;

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

        public static UserResponse Map(User user)
        {
            if (user == default)
                return default;

            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Birthday = user.Birthday,
                Email = user.Email,
                Username = user.Username,
                Weigth = user.Weigth
            };
        }
    }
}