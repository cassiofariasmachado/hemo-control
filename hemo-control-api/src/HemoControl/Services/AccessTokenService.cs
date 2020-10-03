using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using HemoControl.Entities;
using HemoControl.Interfaces.Services;
using HemoControl.Settings;
using Microsoft.IdentityModel.Tokens;

namespace HemoControl.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly AccessTokenSettings _accessTokenSettings;
        private readonly SigningSettings _sigingSettings;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

        public AccessTokenService(AccessTokenSettings accessTokenSettings, SigningSettings signingSettings)
        {
            _accessTokenSettings = accessTokenSettings;
            _sigingSettings = signingSettings;
        }

        public string GenerateToken(User user)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.Username, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
                    }
                );

            var creationDate = DateTime.Now;
            var expirationDate = creationDate +
                TimeSpan.FromSeconds(_accessTokenSettings.ExpiresIn);

            var securityToken = _jwtSecurityTokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _accessTokenSettings.Issuer,
                Audience = _accessTokenSettings.Audience,
                SigningCredentials = _sigingSettings.SigningCredentials,
                Subject = identity,
                NotBefore = creationDate,
                Expires = expirationDate
            });

            return _jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}