using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.Common.Security;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the JWT token generator.
    /// </summary>
    /// <param name="configuration">Application configuration containing the necessary keys for token generation.</param>
    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Generates a JWT token for a specific user.
    /// </summary>
    /// <param name="user">User for whom the token will be generated.</param>
    /// <returns>Valid JWT token as string.</returns>
    /// <remarks>
    /// The generated token includes the following claims:
    /// - NameIdentifier (User ID)
    /// - Name (Username)
    /// - Role (User role)
    /// 
    /// The token is valid for 8 hours from the moment of generation.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Thrown when user or secret key is not provided.</exception>
    public string GenerateToken(IUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = JwtKeyProvider.GetSecurityKey();
        var now = DateTime.UtcNow;
        var expires = now.AddHours(8);

        Console.WriteLine($"Token creation time (UTC): {now}");
        Console.WriteLine($"Token expiration time (UTC): {expires}");

        var signingCredentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256Signature
        );

        var header = new JwtHeader(signingCredentials);
        header.Add("kid", "1");

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            },
            notBefore: now,
            expires: expires,
            signingCredentials: signingCredentials
        );

        var tokenString = tokenHandler.WriteToken(token);
        Console.WriteLine($"Generated token: {tokenString}");
        return tokenString;
    }
}