namespace Api.Infrastructure.Authentication
{
    public class JwtSettings
    {
        public string SecretKey { get; init; } = null!;
        public int ExpiryInMinutes { get; init; }
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
    }
}
