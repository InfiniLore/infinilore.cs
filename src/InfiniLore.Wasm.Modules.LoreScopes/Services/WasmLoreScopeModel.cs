// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Kiota.Models;
using InfiniLore.Shared.Modules.LoreScopes.Database;

namespace InfiniLore.Wasm.Modules.LoreScopes.Services;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record WasmLoreScopeModel(
    Guid Id,
    DateTime CreatedDate,
    DateTime LastModifiedDate,
    Guid OwnerId,
    string Name,
    string? Description
) : ILoreScopeModel {

    public static ILoreScopeModel FromKiotaModel(KiotaLoreScopeResponse response) {
        return new WasmLoreScopeModel(
            Guid.Parse(response.Id!),
            response.CreatedDate?.DateTime ?? DateTime.MinValue,
            response.LastModifiedDate?.DateTime ?? DateTime.MinValue,
            Guid.Parse(response.OwnerId!),
            response.Name!,
            response.AdditionalData.TryGetValue("description", out object? description) ? description as string : null
        );
    }
}
