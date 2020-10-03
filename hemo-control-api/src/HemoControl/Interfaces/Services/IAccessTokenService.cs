using HemoControl.Entities;

namespace HemoControl.Interfaces.Services
{
    public interface IAccessTokenService
    {
        string GenerateToken(User user);
    }
}