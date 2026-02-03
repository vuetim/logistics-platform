using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LogisticsPlatform.Application.Interfaces.Security;
using LogisticsPlatform.Application.Interfaces.Services.Security;
using LogisticsPlatform.Application.Options;
using LogisticsPlatform.Domain.Entities;
using LogisticsPlatform.Domain.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LogisticsPlatform.Infrastructure.Security;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwt;
    private readonly IPermissionService _permissions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions, IPermissionService permissions)
    {
        _jwt = jwtOptions.Value;
        _permissions = permissions;
    }

    public async Task<string> GenerateAccessTokenAsync(User user, CancellationToken ct = default)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("name", user.FullName ?? "")
        };

        // Roles
        var roleNames = user.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new();
        foreach (var r in roleNames)
            claims.Add(new Claim(ClaimTypes.Role, r));

        if (roleNames.Count > 0)
            claims.Add(new Claim("roles", string.Join(",", roleNames)));

        // Permissions (effective)
        var effectivePermissions = await _permissions.GetEffectivePermissionsAsync(user.Id);

        if (roleNames.Contains("Admin"))
        {
            effectivePermissions = Enum.GetValues<Permission>().ToHashSet();
        }

        claims.Add(new Claim(
            "permissions",
            string.Join(",", effectivePermissions.Select(p => p.ToString()))
        ));

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.AccessTokenMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
