// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Kiota.Models;
using InfiniLore.Modules.LoreScopes.Shared.Database;

namespace InfiniLore.Modules.LoreScopes.Wasm;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record WasmLoreScopeModel(
    Guid Id,
    DateTime CreatedDate,
    DateTime LastModifiedDate,
    Guid OwnerId,
    string Name,
    string? Description,
    string? S3PosterImageUrl
) : ILoreScopeModel {
    public string Name { get; set; } = Name;

    public static WasmLoreScopeModel Empty { get; } = new(
        Guid.Empty,
        DateTime.MinValue,
        DateTime.MinValue,
        Guid.Empty,
        string.Empty,
        null,
        null
    );

    public static ILoreScopeModel FromKiotaModel(KiotaLoreScopeResponse response)
        => new WasmLoreScopeModel(
            Guid.Parse(response.Id!),
            response.CreatedDate?.DateTime ?? DateTime.MinValue,
            response.LastModifiedDate?.DateTime ?? DateTime.MinValue,
            Guid.Parse(response.OwnerId!),
            response.Name!,
            response.Description,
            response.ImageUrl
        );
}
