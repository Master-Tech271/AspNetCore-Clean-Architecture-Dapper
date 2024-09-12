namespace Api.Application.Dtos.Authentication
{
    public class AuthResponse
    {
        public UserDto User { get; set; } = default!;
        public string Token { get; set; } = default!;
    }

    public class UserDto
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
