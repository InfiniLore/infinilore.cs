// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Modules.MarkdownFiles.Database;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.MarkdownFiles.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<MarkdownFileMapper>(ServiceLifetime.Singleton)]
public class MarkdownFileMapper : ResponseMapper<MarkdownFileResponse, MarkdownFile> {
    public override MarkdownFileResponse FromEntity(MarkdownFile markdownFile) => new() {
        Name = markdownFile.Name,
        Source = markdownFile.Source,
        Id = markdownFile.Id,
        CreatedDate = markdownFile.CreatedDate,
        LastModifiedDate = markdownFile.LastModifiedDate,
        LoreScopeId = markdownFile.LoreScopeId
    };
}
