namespace Takwene.Infrastructure.Authentication
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

        // Loaded from user-secrets / environment variables, never committed.
        public string Key { get; set; } = string.Empty;

        public int ExpiryMinutes { get; set; } = 60;
    }
}
