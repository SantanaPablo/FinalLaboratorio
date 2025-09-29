namespace MSCustomer.API.DTOs
{
    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class ValidateTokenRequest
    {
        public string Token { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    public class ValidateTokenResponse
    {
        public bool IsValid { get; set; }
        public int? CustomerId { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Message { get; set; }
    }
}