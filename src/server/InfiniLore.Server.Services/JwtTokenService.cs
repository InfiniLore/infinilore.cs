// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FastEndpoints.Security;
using InfiniLore.Server.Contracts.Data;
using InfiniLore.Server.Contracts.Repositories;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data;
using InfiniLore.Server.Data.Models.Account;
using InfiniLoreLib.Results;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Security.Claims;

namespace InfiniLore.Server.Services;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[RegisterService<IJwtTokenService>(LifeTime.Scoped)]
public class JwtTokenService(IConfiguration configuration, IJwtRefreshTokenRepository jwtRefreshTokenRepository, ILogger logger, IDbUnitOfWork<InfiniLoreDbContext> unitOfWork) : IJwtTokenService {

    public async Task<JwtResult> GenerateTokensAsync(InfiniLoreUser user, string[] roles, string[] permissions, CancellationToken ct = default) {
        try {
            string? key = configuration["Jwt:Key"];
            if (key == null) {
                logger.Error("Jwt:Key not found in configuration");
                return JwtResult.Failure("Jwt:Key not found in configuration");
            }

            DateTime accessTokenExpiryUtc = DateTime.UtcNow.AddMinutes(int.Parse(configuration["Jwt:AccessExpiresInMinutes"]!));
            DateTime refreshTokenExpiryUtc = DateTime.UtcNow.AddDays(int.Parse(configuration["Jwt:RefreshExpiresInDays"]!));

            // InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();
            // await dbContext.Roles.FindAsync(roles, ct);

            string accessToken = GenerateAccessToken(user, roles, permissions, accessTokenExpiryUtc);
            Guid refreshToken = await GenerateRefreshTokenAsync(user, refreshTokenExpiryUtc, ct);

            return JwtResult.Success(
                accessToken,
                accessTokenExpiryUtc: accessTokenExpiryUtc,
                refreshToken: refreshToken,
                refreshTokenExpiryUtc: refreshTokenExpiryUtc
            );

        }
        catch (Exception ex) {
            // TODO only send the direct message in dev environment
            logger.Error(ex, "Error generating tokens");
            return JwtResult.Failure(ex.Message);
        }
    }

    private string GenerateAccessToken(InfiniLoreUser user, string[] roles, string[] permissions, DateTime expiresAt) {
        string jwtToken = JwtBearer.CreateToken(
            o => {
                o.SigningKey = configuration["Jwt:Key"]!;
                o.ExpireAt = expiresAt;
                o.Audience = configuration["JWT:Audience"];
                o.Issuer = configuration["JWT:Issuer"];

                o.User.Roles.Add(roles);
                o.User.Permissions.Add(permissions);
                o.User[ClaimTypes.Name] = user.Id;
            });

        return jwtToken;
    }

    private async Task<Guid> GenerateRefreshTokenAsync(InfiniLoreUser user, DateTime expiresAt, CancellationToken ct = default) {
        var token = Guid.NewGuid();
        try {
            await jwtRefreshTokenRepository.AddAsync(user, token, expiresAt, ct);
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqliteException { SqliteErrorCode: 19 }) {
            logger.Error(ex, "Unique constraint violation while adding refresh token for user {UserId}", user.Id);
            throw;
        }
        catch (Exception ex) {
            logger.Error(ex, "Error adding refresh token for user {UserId}", user.Id);
            throw;
        }

        return token;
    }
}
