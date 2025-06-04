// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared;

namespace InfiniLore.Modules.Core.Server.Database.RepoMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByOwnerAsync<TOwner, TInterface> 
    where TInterface : OwnedModel<TOwner>
    where TOwner : BasicModel 
{
    ValueTask<Outcome<TInterface[]>> GetByOwnerAsync(Guid owner, QueryConfig config = default, CancellationToken ct = default);
    
    ValueTask<PaginatedOutcome<TInterface>> GetByOwnerAsync(Guid owner, Pagination pageInfo, QueryConfig config = default, CancellationToken ct = default);
}
