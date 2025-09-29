using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MSCustomer.Infrastructure
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(int userId, string username, string role, string secretKey)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "CustomerMicroservice",
                audience: "CustomerMicroservice",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static ClaimsPrincipal? ValidateJwtToken(string token, string secretKey)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = "CustomerMicroservice",
                    ValidateAudience = true,
                    ValidAudience = "CustomerMicroservice",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        public static (int UserId, string Email, string Role)? GetTokenClaims(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var userId = int.Parse(jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
                var email = jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value;
                var role = jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value;

                return (userId, email, role);
            }
            catch
            {
                return null;
            }
        }
    }
}