// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Contracts.Database.Repositories.RepositoryMethods;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Server.Types;

namespace InfiniLore.Contracts.Database.Repositories.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IJwtRefreshTokenRepository :
    IHasTryAddAsync<JwtRefreshTokenModel>,
    IHasTryRemoveAsync<JwtRefreshTokenModel>,
    IHasTryPermanentRemoveAllForUserAsync ,
    IUnitOfWorkRepository
{
    
    ValueTask<RepoResult<JwtRefreshTokenModel>> TryGetByHashedTokenAsync(string hashedToken, CancellationToken ct = default);
}
