// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Api.Responses.Data.Project.MarkdownFiles;
using InfiniLore.Server.Database.Models.Data.Project;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Api.Mappers.Data.Project.MarkdownFiles;

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
