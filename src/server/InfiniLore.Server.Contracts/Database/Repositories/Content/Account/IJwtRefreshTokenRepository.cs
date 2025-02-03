// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Server.Contracts.Database.Repositories.RepositoryMethods;
using InfiniLore.Server.Types;

namespace InfiniLore.Server.Contracts.Database.Repositories.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtRefreshTokenRepository :
    IHasTryAddAsync<JwtRefreshTokenModel>,
    IHasTryRemoveAsync<JwtRefreshTokenModel>,
    IHasTryPermanentRemoveAllForUserAsync ,
    IRepository
{
    
    ValueTask<RepoResult<JwtRefreshTokenModel>> TryGetByHashedTokenAsync(string hashedToken, CancellationToken ct = default);
}
