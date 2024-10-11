// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.UserData;
using InfiniLoreLib.Results;

namespace InfiniLore.Server.Contracts.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface ILoreScopesRepository {
    Task<Result<IEnumerable<LoreScopeModel>>> GetAllAsync(CancellationToken ct);
    Task<Result<IEnumerable<LoreScopeModel>>> GetAllAsync(Func<IQueryable<LoreScopeModel>,IQueryable<LoreScopeModel>> predicate, CancellationToken ct);

    Task<Result<bool>> DeleteAsync(Guid loreScopeId, CancellationToken ct = default);
    Task<Result<bool>> DeleteAsync(LoreScopeModel model, CancellationToken ct = default);
}
