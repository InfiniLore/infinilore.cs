// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FastEndpoints.Security;
using InfiniLore.Server.Contracts.Repositories;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data.Models.Account;
using InfiniLoreLib.Results;
using Microsoft.Extensions.Configuration;

namespace InfiniLore.Server.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RegisterService<IJwtTokenService>(LifeTime.Scoped)]
public class JwtTokenService(IConfiguration configuration, IJwtRefreshTokenRepository jwtRefreshTokenRepository) : IJwtTokenService {

    public async Task<JwtResult> GenerateTokensAsync(InfiniLoreUser user, string[] roles, string[] permissions, CancellationToken ct = default) {
        try {
            string? key = configuration["Jwt:Key"];
            // TODO only do this in dev env, else send a basic "failed" result
            if (key == null) return JwtResult.Failure("Jwt:Key not found in configuration");

            DateTime accessTokenExpiryUtc = DateTime.UtcNow.AddMinutes(int.Parse(configuration["Jwt:AccessExpiresInMinutes"]!));
            DateTime refreshTokenExpiryUtc = DateTime.UtcNow.AddDays(int.Parse(configuration["Jwt:RefreshExpiresInDays"]!));
            
            string accessToken = GenerateAccessToken(user, roles, permissions,accessTokenExpiryUtc );
            Guid refreshToken = await GenerateRefreshTokenAsync(user, refreshTokenExpiryUtc, ct);
        
            return JwtResult.Success(
                accessToken: accessToken,
                accessTokenExpiryUtc: accessTokenExpiryUtc,
                refreshToken: refreshToken,
                refreshTokenExpiryUtc: refreshTokenExpiryUtc
            );
            
        } catch (Exception ex) {
            // TODO only send the direct message in dev environment
            return JwtResult.Failure(ex.Message);
        }
    }

    private string GenerateAccessToken(InfiniLoreUser user, string[] roles, string[] permissions, DateTime expiresAt) {
        string jwtToken = JwtBearer.CreateToken(
            o => {
                o.SigningKey = configuration["Jwt:Key"]!;
                o.ExpireAt = expiresAt;
                
                o.User.Roles.Add(roles);
                o.User.Permissions.Add(permissions);
                
                o.User["UserId"] = user.Id;
            });
        
        return jwtToken;
    }

    private async Task<Guid> GenerateRefreshTokenAsync(InfiniLoreUser user, DateTime expiresAt, CancellationToken ct = default) {
        var token = Guid.NewGuid();
        await jwtRefreshTokenRepository.AddAsync(user, token, expiresAt, ct);
        return token;
    }
}
