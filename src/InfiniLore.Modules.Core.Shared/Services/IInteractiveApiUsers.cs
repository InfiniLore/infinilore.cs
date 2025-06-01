// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Modules.Core.Shared.Database;
using Microsoft.AspNetCore.Components.Forms;

namespace InfiniLore.Modules.Core.Shared;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInteractiveApiUsers {
    ValueTask<Result<IInfiniLoreUserModel>> GetUserAsync(string userId, CancellationToken ct = default);
    ValueTask<Result> UpsertProfileImageAsync(string userId, IBrowserFile file, CancellationToken ct = default);
}
