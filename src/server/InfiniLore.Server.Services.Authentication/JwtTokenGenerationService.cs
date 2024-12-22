// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Unions;
using FastEndpoints.Security;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Account;
using InfiniLore.Server.Contracts.Services.Auth.Authentication;
using InfiniLore.Server.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InfiniLore.Server.Services.Authentication;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IJwtTokenGenerationService>(ServiceLifetime.Scoped)]
public class JwtTokenGenerationService(
    IConfiguration configuration,
    IJwtRefreshTokenRepository repository,
    ILogger logger,
    UserManager<InfiniLoreUser> userManager
) : IJwtTokenGenerationService {
    private readonly int _jwtAccessExpiresInMinutes = int.Parse(configuration["Jwt:AccessExpiresInMinutes"]!);
    private readonly string _jwtAudience = configuration["Jwt:Audience"]!;
    private readonly string _jwtIssuer = configuration["Jwt:Issuer"]!;
    private readonly string _jwtKey = configuration["Jwt:Key"]!;
    private readonly int _jwtRefreshExpiresInDays = int.Parse(configuration["Jwt:RefreshExpiresInDays"]!);

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<SuccessOrFailure<JwtTokenData>> GenerateTokensAsync(InfiniLoreUser user, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default) {
        try {
            // Check if all provided roles exist in the user's roles
            // TODO move to IUserRepository usage
            IList<string> userRoles = await userManager.GetRolesAsync(user);
            if (!roles.All(role => userRoles.Contains(role))) {
                logger.Warning("User {UserId} does not have all specified roles", user.Id);
                return "User does not have the required roles";
            }

            DateTime accessTokenExpiryUtc = DateTime.UtcNow.AddMinutes(_jwtAccessExpiresInMinutes);
            DateTime refreshTokenExpiryUtc = DateTime.UtcNow.AddDays(_jwtRefreshExpiresInDays);

            string accessToken = GenerateAccessToken(user, roles, permissions, accessTokenExpiryUtc);
            SuccessOrFailure<Guid> resultGenerate = await GenerateRefreshTokenAsync(user, roles, permissions, refreshTokenExpiryUtc, ct);
            if (!resultGenerate.TryGetAsSuccessValue(out Guid refreshToken)) return "Refresh token could not be generated";

            return new JwtTokenData(
                accessToken,
                accessTokenExpiryUtc,
                refreshToken,
                refreshTokenExpiryUtc);
        }
        catch (Exception ex) {
            logger.Error(ex, "Error generating tokens");
            return "Unexpected error generating tokens";
        }
    }

    public async ValueTask<SuccessOrFailure<JwtTokenData>> RefreshTokensAsync(Guid refreshToken, CancellationToken ct = default) {
        RepoResult<JwtRefreshTokenModel> getResult = await repository.TryGetByIdAsync(refreshToken, ct);
        if (!getResult.TryGetSuccessValue(out JwtRefreshTokenModel? oldToken)) return "Refresh token not found";
        if (oldToken.ExpiresAt < DateTime.UtcNow) return "Refresh token has expired";

        await repository.TryRemoveAsync(oldToken, ct);
        return await GenerateTokensAsync(
            oldToken.Owner,
            oldToken.Roles,
            oldToken.Permissions,
            oldToken.ExpiresInDays ?? int.Parse(configuration["Jwt:RefreshExpiresInDays"]!),
            ct
        );
    }

    public async ValueTask<bool> RevokeTokensAsync(InfiniLoreUser user, Guid refreshToken, CancellationToken ct = default) {
        RepoResult<JwtRefreshTokenModel> getResult = await repository.TryGetByIdAsync(refreshToken, ct);
        if (getResult.IsFailure) return false;

        JwtRefreshTokenModel oldToken = getResult.AsSuccess.Value;
        if (oldToken.Owner.Id != user.Id) return false;

        RepoResult deleteResult = await repository.TryRemoveAsync(oldToken, ct);
        return deleteResult.IsSuccess;
    }

    public async ValueTask<bool> RevokeAllTokensFromUserAsync(InfiniLoreUser user, CancellationToken ct = default) {
        RepoResult deleteResult = await repository.TryPermanentRemoveAllForUserAsync(user.Id, ct);
        return deleteResult.IsSuccess;
    }

    private static string HashToken(Guid token) {
        byte[] tokenBytes = Encoding.UTF8.GetBytes(token.ToString());
        byte[] hashBytes = SHA256.HashData(tokenBytes);
        return Convert.ToBase64String(hashBytes);
    }

    private string GenerateAccessToken(InfiniLoreUser user, string[] roles, string[] permissions, DateTime expiresAt) {
        string jwtToken = JwtBearer.CreateToken(
            o => {
                o.SigningKey = _jwtKey;
                o.ExpireAt = expiresAt;
                o.Audience = _jwtAudience;
                o.Issuer = _jwtIssuer;

                o.User.Roles.Add(roles);
                o.User.Permissions.Add(permissions);
                o.User[ClaimTypes.NameIdentifier] = user.Id.ToString();
            });

        return jwtToken;
    }

    private async ValueTask<SuccessOrFailure<Guid>> GenerateRefreshTokenAsync(InfiniLoreUser user, string[] roles, string[] permissions, DateTime expiresAt, CancellationToken ct = default) {
        var token = Guid.NewGuid();
        var refreshToken = new JwtRefreshTokenModel {
            OwnerId = user.Id,// Use the user's ID instead of the user object
            ExpiresAt = expiresAt,
            TokenHash = HashToken(token),
            Roles = roles,
            Permissions = permissions
        };

        RepoResult result = await repository.TryAddAsync(refreshToken, ct);
        if (result.IsSuccess) return token;

        return new Failure<string>("Failed to save refresh token.");
    }
}
