// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Modules.Core.Shared.Database;

namespace InfiniLore.Modules.Core.Shared;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUserInteractiveApi {
    ValueTask<Result<IInfiniLoreUserModel>> GetUserAsync(string userId, CancellationToken ct = default);
    ValueTask<Result> UpsertProfileImageAsync(string userId,  string contentType, Stream file, CancellationToken ct = default);
}
