// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Kiota.Models;
using InfiniLore.Modules.Core.Shared.Database;

namespace InfiniLore.Modules.Core.Wasm.KiotaModels;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record WasmInfiniLoreUserModel(
    Guid Id,
    string Username,
    string? Auth0Id,
    string? ProfileImageUrl
) : IInfiniLoreUserModel{
    public static IInfiniLoreUserModel FromKiotaModel(KiotaUserUserProfileResponse response)
        => new WasmInfiniLoreUserModel(
            Guid.Parse(response.Id!),
            response.Username ?? "unknown user",
            null,
            response.ProfileImageUrl
        );
}
