using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat_Backend.Models.Backend;
using Microsoft.IdentityModel.Tokens;
using Storage.Models.Backend;

namespace Chat_Backend.Models.Options;

public class JwtOptions
{
    private const int AccessTime = 60;
    private const int RefreshTime = 44650;
    private const string Secret = "qwerfdsa1234asdasdasdzzxcASASQWQWQW123123";
    private const string Issuer = "Chat-Backend";
    private const string Audience = "Chat-Client";
    private static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Secret));

    public Tokens GetJwtTokens(string login)
    {
        if (login is null)
        {
            throw new ArgumentNullException(nameof(login));
        }
        var claims = new List<Claim>() { new(ClaimTypes.Name, login) };

        var access = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AccessTime)),
            signingCredentials: new(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );
        var accessToken = new JwtSecurityTokenHandler().WriteToken(access);

        var refresh = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(RefreshTime)),
            signingCredentials: new(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );
        var refreshToken = new JwtSecurityTokenHandler().WriteToken(refresh);

        return new Tokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public string GetLoginFromJwtToken(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.ReadToken(jwtToken) is not JwtSecurityToken securityToken)
        {
            throw new SecurityTokenException("Invalid token.");
        }

        var loginClaim = securityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
        if (loginClaim != null)
        {
            return loginClaim.Value;
        }
        throw new SecurityTokenException("Login claim not found in the token.");
    }
}