// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Account;

namespace InfiniLore.Contracts.Services.Auth.Authorization;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IPermissionValidator {
    ValueTask<bool> UserHasPermission(InfiniLoreUser user, string permission, CancellationToken ct = default);
    ValueTask<bool> UserHasPermissions(InfiniLoreUser user, string[] permissions, CancellationToken ct = default);
}
