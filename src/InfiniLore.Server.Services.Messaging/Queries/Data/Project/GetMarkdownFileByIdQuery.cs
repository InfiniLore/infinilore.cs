// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.Data.Project;

namespace InfiniLore.Server.Services.Messaging.Queries.Data.Project;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public record GetMarkdownFileByIdQuery(Guid MarkdownFileId, Guid LorescopeId = default, bool AutoInclude = false) : MessageRequest<MarkdownFile>;
