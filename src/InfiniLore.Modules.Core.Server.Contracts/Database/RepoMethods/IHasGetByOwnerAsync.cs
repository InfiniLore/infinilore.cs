// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByOwnerAsync<TOwner, TInterface> 
    where TInterface : OwnedModel<TOwner>
    where TOwner : BasicModel 
{
    ValueTask<Result<TInterface[]>> GetByOwnerAsync(Guid owner, QueryConfig config = default, CancellationToken ct = default);
    
    ValueTask<PaginatedResult<TInterface>> GetByOwnerAsync(Guid owner, Pagination pageInfo, QueryConfig config = default, CancellationToken ct = default);
}
