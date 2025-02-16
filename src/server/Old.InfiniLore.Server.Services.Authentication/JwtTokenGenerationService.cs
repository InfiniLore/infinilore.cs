// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints.Security;
using Old.InfiniLore.Database.Models.Content.Account;
using Old.InfiniLore.Contracts;
using Old.InfiniLore.Contracts.Database.Repositories.Content.Account;
using Old.InfiniLore.Contracts.Services.Auth.Authentication;
using Old.InfiniLore.Server.Types;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Old.InfiniLore.Server.Services.Authentication;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IJwtTokenGenerationService>(ServiceLifetime.Scoped)]
public class JwtTokenGenerationService(
    IUnitOfWork unitOfWork,
    IJwtConfiguration jwtConfiguration
) : IJwtTokenGenerationService {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<SuccessOrFailure<JwtTokenData>> GenerateTokensAsync(InfiniLoreUser user, string[] roles, string[] permissions, int? expiresInDays, CancellationToken ct = default) {
        DateTime accessTokenExpiryUtc = DateTime.UtcNow.AddMinutes(jwtConfiguration.AccessExpiresInMinutes);
        DateTime refreshTokenExpiryUtc = DateTime.UtcNow.AddDays(expiresInDays ?? jwtConfiguration.RefreshExpiresInDays);

        string accessToken = GenerateAccessToken(user, roles, permissions, accessTokenExpiryUtc);
        Guid refreshToken = await GenerateRefreshTokenAsync(user, roles, permissions, refreshTokenExpiryUtc, ct);
        if (refreshToken == Guid.Empty) return "Refresh token could not be generated";

        return new JwtTokenData(
            user.Id,
            accessToken,
            accessTokenExpiryUtc,
            refreshToken,
            refreshTokenExpiryUtc,
            permissions,
            roles
        );
    }

    public async ValueTask<SuccessOrFailure<JwtTokenData>> RefreshTokensAsync(Guid refreshToken, CancellationToken ct = default) {
        string hashedToken = HashToken(refreshToken);
        var repository = await unitOfWork.GetRepositoryAsync<IJwtRefreshTokenRepository>();
        
        RepoResult<JwtRefreshTokenModel> getResult = await repository.TryGetByHashedTokenAsync(hashedToken, ct);
        if (!getResult.TryGetAsSuccess(out JwtRefreshTokenModel? oldToken)) return "Refresh token not found";

        await repository.TryRemoveAsync(oldToken, ct); // If it is expired or not, we can remove it.

        if (oldToken.ExpiresAt < DateTime.UtcNow) return "Refresh token has expired";

        return await GenerateTokensAsync(
            oldToken.Owner,
            oldToken.Roles,
            oldToken.Permissions,
            oldToken.ExpiresInDays,
            ct
        );
    }

    public async ValueTask<bool> RevokeTokensAsync(InfiniLoreUser user, Guid refreshToken, CancellationToken ct = default) {
        string hashedToken = HashToken(refreshToken);
        var repository = await unitOfWork.GetRepositoryAsync<IJwtRefreshTokenRepository>();
        
        RepoResult<JwtRefreshTokenModel> getResult = await repository.TryGetByHashedTokenAsync(hashedToken, ct);
        if (getResult.IsFailure) return false;

        JwtRefreshTokenModel oldToken = getResult.AsSuccess;
        if (oldToken.Owner.Id != user.Id) return false;

        RepoResult deleteResult = await repository.TryRemoveAsync(oldToken, ct);
        return deleteResult.IsSuccess;
    }

    public async ValueTask<bool> RevokeAllTokensFromUserAsync(InfiniLoreUser user, CancellationToken ct = default) {
        var repository = await unitOfWork.GetRepositoryAsync<IJwtRefreshTokenRepository>();
        RepoResult deleteResult = await repository.TryPermanentRemoveAllForUserAsync(user.Id, ct);
        return deleteResult.IsSuccess;
    }

    private static string HashToken(Guid token) {
        byte[] tokenBytes = Encoding.UTF8.GetBytes(token.ToString());
        byte[] hashBytes = SHA256.HashData(tokenBytes);
        return Convert.ToBase64String(hashBytes);
    }

    private string GenerateAccessToken(InfiniLoreUser user, string[] roles, string[] permissions, DateTime expiresAt)
        => JwtBearer.CreateToken(o => {
            o.SigningKey = jwtConfiguration.Key;
            o.ExpireAt = expiresAt;
            o.Audience = jwtConfiguration.Audience;
            o.Issuer = jwtConfiguration.Issuer;

            o.User.Roles.Add(roles);
            o.User.Permissions.Add(permissions);
            o.User[ClaimTypes.NameIdentifier] = user.Id.ToString();
        });

    private async ValueTask<Guid> GenerateRefreshTokenAsync(InfiniLoreUser user, string[] roles, string[] permissions, DateTime expiresAt, CancellationToken ct = default) {
        var token = Guid.NewGuid();
        var refreshToken = new JwtRefreshTokenModel {
            OwnerId = user.Id,// Use the user's ID instead of the user object
            ExpiresAt = expiresAt,
            TokenHash = HashToken(token),
            Roles = roles,
            Permissions = permissions
        };

        var repository = await unitOfWork.GetRepositoryAsync<IJwtRefreshTokenRepository>();
        RepoResult result = await repository.TryAddAsync(refreshToken, ct);
        return result.IsSuccess 
            ? token 
            : Guid.Empty;
    }
}
