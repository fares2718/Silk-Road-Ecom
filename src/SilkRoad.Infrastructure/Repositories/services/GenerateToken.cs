using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SilkRoad.Core;

namespace SilkRoad.Infrastructure;

internal class GenerateToken : IGenerateToken
{
    public IConfiguration _config;

    public GenerateToken(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateAccessToken(AppUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        // 2. Safely loop and append your roles to the list
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT_SECRET_KEY"]!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenValue = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );


        string token = new JwtSecurityTokenHandler().WriteToken(tokenValue);
        return token;
    }

    public string GenerateRefreshToken()
{
    var bytes = new byte[64];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(bytes);
    return Convert.ToBase64String(bytes);
}

}
