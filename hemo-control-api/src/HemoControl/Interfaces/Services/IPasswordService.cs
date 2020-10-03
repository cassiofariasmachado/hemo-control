namespace HemoControl.Interfaces.Services
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool Verify(string plainPassword, string hashedPassword);
    }
}