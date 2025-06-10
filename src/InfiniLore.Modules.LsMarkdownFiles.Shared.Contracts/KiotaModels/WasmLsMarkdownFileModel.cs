// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Kiota.Models;
using InfiniLore.Modules.LsMarkdownFiles.Shared.Database;

namespace InfiniLore.Modules.LsMarkdownFiles.Shared;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record WasmLsMarkdownFileModel(
    Guid Id,
    DateTime CreatedDate,
    DateTime LastModifiedDate,
    Guid OwnerId,
    string Name,
    string? ResourceUrl
) : ILsMarkdownFileModel {
    public string Name { get; set; } = Name;

    public static WasmLsMarkdownFileModel Empty { get; } = new(
        Guid.Empty,
        DateTime.MinValue,
        DateTime.MinValue,
        Guid.Empty,
        string.Empty,
        null
    );
    
    public static ILsMarkdownFileModel FromKiotaModel(KiotaLsMarkdownFileResponse response)
        => new WasmLsMarkdownFileModel(
            Guid.Parse(response.Id!),
            response.CreatedDate?.DateTime ?? DateTime.MinValue,
            response.LastModifiedDate?.DateTime ?? DateTime.MinValue,
            Guid.Parse(response.OwnerId!),
            response.Name!,
            response.ResourceUrl
        );
}
