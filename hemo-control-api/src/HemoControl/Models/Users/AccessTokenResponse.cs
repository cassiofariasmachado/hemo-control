using HemoControl.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HemoControl.Models.Users
{
    public class AccessTokenResponse
    {
        public string AccessToken { get; set; }
        public string Type => JwtBearerDefaults.AuthenticationScheme;
        public int ExpiresIn { get; set; }

        public AccessTokenResponse(string accessToken, AccessTokenSettings accessTokenSettings)
        {
            AccessToken = accessToken;
            ExpiresIn = accessTokenSettings.ExpiresIn;
        }
    }
}