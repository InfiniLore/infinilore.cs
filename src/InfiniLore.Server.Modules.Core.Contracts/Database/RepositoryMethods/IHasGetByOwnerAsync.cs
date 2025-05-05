// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Server.Modules.Core.Database.Models;

namespace InfiniLore.Server.Modules.Core.Database.RepositoryMethods;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IHasGetByOwnerAsync<TOwner, TInterface> where TInterface : IOwnedData<TOwner> {
    ValueTask<Result<TInterface[]>> GetByOwnerAsync(Guid owner, QueryConfig config = default, CancellationToken ct = default);
    
    ValueTask<PaginatedResult<TInterface>> GetByOwnerAsync(Guid owner, PaginationInfo pageInfo, QueryConfig config = default, CancellationToken ct = default);
}
