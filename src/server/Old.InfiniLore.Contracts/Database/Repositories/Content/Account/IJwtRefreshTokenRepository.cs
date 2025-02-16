// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using Old.InfiniLore.Database.Models.Content.Account;
using Old.InfiniLore.Server.Types;
using Old.InfiniLore.Contracts.Database.Repositories.RepositoryMethods;

namespace Old.InfiniLore.Contracts.Database.Repositories.Content.Account;
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
