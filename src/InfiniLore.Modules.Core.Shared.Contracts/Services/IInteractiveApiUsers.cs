// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Modules.Core.Shared.Database;
using Microsoft.AspNetCore.Components.Forms;

namespace InfiniLore.Modules.Core.Shared;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInteractiveApiUsers {
    ValueTask<Outcome<IInfiniLoreUserModel>> GetUserAsync(string userId, CancellationToken ct = default);
    ValueTask<Outcome> UpsertProfileImageAsync(string userId, IBrowserFile file, CancellationToken ct = default);
}
