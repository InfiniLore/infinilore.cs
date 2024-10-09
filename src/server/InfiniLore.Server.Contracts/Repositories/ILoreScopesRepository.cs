// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.UserData;
using InfiniLoreLib;

namespace InfiniLore.Server.Contracts.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopesRepository {
    Task<Result<IEnumerable<LoreScopeModel>>> GetAllAsync(CancellationToken ct);
    Task<Result<IEnumerable<LoreScopeModel>>> GetAllAsync(Func<IQueryable<LoreScopeModel>,IQueryable<LoreScopeModel>> predicate, CancellationToken ct);
}
