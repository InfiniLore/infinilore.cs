// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core.ApiEndpoints;
using JetBrains.Annotations;

namespace InfiniLore.Server.Modules.MarkdownFiles.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record MarkdownFileResponse : OwnedResponse {
    public required string Name { [UsedImplicitly] get; init; }
    public string Source { [UsedImplicitly] get; init; }
}
