namespace HemoControl.Settings
{
    public class AccessTokenSettings
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpiresIn { get; set; }
    }
}