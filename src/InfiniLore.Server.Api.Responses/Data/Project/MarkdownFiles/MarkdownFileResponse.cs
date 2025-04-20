// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using JetBrains.Annotations;

namespace InfiniLore.Server.Api.Responses.Data.Project.MarkdownFiles;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record MarkdownFileResponse : ProjectDataResponse {
    public required string Name { [UsedImplicitly] get; init; }
    public string Source { [UsedImplicitly] get; init; }
}
