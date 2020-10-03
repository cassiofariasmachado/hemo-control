using HemoControl.Interfaces.Services;
using BCryptNet = BCrypt.Net;

namespace HemoControl.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
            => BCryptNet.BCrypt.HashPassword(password);

        public bool Verify(string plainPassword, string hashedPassword)
            => BCryptNet.BCrypt.Verify(plainPassword, hashedPassword);
    }
}