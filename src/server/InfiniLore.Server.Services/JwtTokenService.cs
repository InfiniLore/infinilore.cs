// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FastEndpoints.Security;
using InfiniLore.Server.Contracts.Data.Repositories;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data.Models.Account;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Identity;
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
public class JwtTokenService(IConfiguration configuration, IJwtRefreshTokenRepository jwtRefreshTokenRepository, ILogger logger, UserManager<InfiniLoreUser> userManager) : IJwtTokenService {

    public async Task<JwtResult> GenerateTokensAsync(InfiniLoreUser user, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default) {
        try {
            string? key = configuration["Jwt:Key"];
            if (key == null) {
                logger.Error("Jwt:Key not found in configuration");
                return JwtResult.Failure("Jwt:Key not found in configuration");
            }

            // Check if all provided roles exist in the user's roles
            IList<string> userRoles = await userManager.GetRolesAsync(user);
            if (!roles.All(role => userRoles.Contains(role))) {
                logger.Warning("User {UserId} does not have all specified roles", user.Id);
                return JwtResult.Failure("User does not have the required roles");
            }

            DateTime accessTokenExpiryUtc = DateTime.UtcNow.AddMinutes(int.Parse(configuration["Jwt:AccessExpiresInMinutes"]!));
            DateTime refreshTokenExpiryUtc = DateTime.UtcNow.AddDays(expiresInDays ?? int.Parse(configuration["Jwt:RefreshExpiresInDays"]!));

            string accessToken = GenerateAccessToken(user, roles, permissions, accessTokenExpiryUtc);
            Guid refreshToken = await GenerateRefreshTokenAsync(user, roles, permissions, refreshTokenExpiryUtc, expiresInDays, ct);

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
                o.User[ClaimTypes.NameIdentifier] = user.Id;
            });

        return jwtToken;
    }

    private async Task<Guid> GenerateRefreshTokenAsync(InfiniLoreUser user, string[] roles, string[] permissions, DateTime expiresAt, int? expiresInDays, CancellationToken ct = default) {
        var token = Guid.NewGuid();
        try {
            await jwtRefreshTokenRepository.AddAsync(user, token, expiresAt, roles, permissions, expiresInDays, ct);
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

    public async Task<JwtResult> RefreshTokensAsync(Guid refreshToken, CancellationToken ct = default) {
        if (await jwtRefreshTokenRepository.GetAsync(refreshToken, ct) is not {} oldToken) return JwtResult.Failure("Invalid refresh token");
        if (oldToken.ExpiresAt < DateTime.UtcNow) return JwtResult.Failure("Refresh token has expired");

        await jwtRefreshTokenRepository.RemoveAsync(oldToken, ct);

        return await GenerateTokensAsync(
            oldToken.User,
            oldToken.Roles,
            oldToken.Permissions,
            oldToken.ExpiresInDays ?? int.Parse(configuration["Jwt:RefreshExpiresInDays"]!),
            ct
        );
    }

    public async Task<BoolResult> RevokeTokensAsync(InfiniLoreUser user, Guid refreshToken, CancellationToken ct = default) {
        if (await jwtRefreshTokenRepository.GetAsync(refreshToken, ct) is not {} oldToken) return BoolResult.Failure("Invalid refresh token");
        if (oldToken.User.Id != user.Id) return BoolResult.Failure("Refresh token does not belong to user");

        await jwtRefreshTokenRepository.RemoveAsync(oldToken, ct);

        return BoolResult.Success();
    }

    public async Task<BoolResult> RevokeAllTokensFromUserAsync(InfiniLoreUser user, CancellationToken ct = default) =>
        await jwtRefreshTokenRepository.RemoveAllAsync(user.Id, ct)
            ? BoolResult.Success()
            : BoolResult.Failure("Error revoking all tokens from user");
}
