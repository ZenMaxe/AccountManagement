using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountManagement.Domain.DTOs;
using AccountManagement.Domain.Entities.Identity;
using AccountManagement.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AccountManagement.BLL.Services;

public class JwtHandler : IJwtHandler
{
    private readonly IConfiguration _configuration;

    public JwtHandler(IConfiguration configuration)
    { _configuration = configuration; }

    public async Task<TokenDto> CreateToken(AppUser user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var refreshToken = GenerateRefreshToken(); // it's For Now and will change later

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expiry = tokenOptions.ValidTo
        };
    }

    public TokenDto RefreshToken(string expiredToken, string refreshToken)
    {
        throw new NotImplementedException();
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signincred, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtDetails");

        var token = new JwtSecurityToken(
            issuer: jwtSettings.GetSection("Issuer").Value,
            claims: claims,
            expires: DateTime.Now.AddHours(10),
            signingCredentials: signincred
            );

        return token;
    }

    private async Task<List<Claim>> GetClaims(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),

        };
        return claims;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}