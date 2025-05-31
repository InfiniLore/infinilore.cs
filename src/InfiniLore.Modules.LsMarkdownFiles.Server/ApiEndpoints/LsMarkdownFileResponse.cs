// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using InfiniLore.Modules.Core.Server.ApiEndpoints;
using JetBrains.Annotations;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record LsMarkdownFileResponse : OwnedResponse {
    public required string Name { [UsedImplicitly] get; init; }
    public required Guid LastUserToEditId { [UsedImplicitly] get; init; }
    public required string? ResourceUrl { [UsedImplicitly] get; init; }
}
