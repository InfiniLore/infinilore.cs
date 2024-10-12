// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Account;
using InfiniLore.Server.Data.Models.UserData;
using InfiniLoreLib.Results;

namespace InfiniLore.Server.Contracts.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInfiniLoreUserRepository {
    public Task<ResultMany<LoreScopeModel>> GetLoreScopesAsync(InfiniLoreUser user, CancellationToken ct = default);
    public Task<ResultMany<LoreScopeModel>> GetLoreScopesAsync(Guid userId, CancellationToken ct = default);
    public Task<ResultMany<LoreScopeModel>> GetLoreScopesAsync(string userId, CancellationToken ct = default);

    public Task<ResultMany<MultiverseModel>> GetMultiversesAsync(InfiniLoreUser user, CancellationToken ct = default);
    public Task<ResultMany<MultiverseModel>> GetMultiversesAsync(Guid userId, CancellationToken ct = default);
    public Task<ResultMany<MultiverseModel>> GetMultiversesAsync(string userId, CancellationToken ct = default);

    public Task<ResultMany<UniverseModel>> GetUniversesAsync(InfiniLoreUser user, CancellationToken ct = default);
    public Task<ResultMany<UniverseModel>> GetUniversesAsync(Guid userId, CancellationToken ct = default);
    public Task<ResultMany<UniverseModel>> GetUniversesAsync(string userId, CancellationToken ct = default);
}
