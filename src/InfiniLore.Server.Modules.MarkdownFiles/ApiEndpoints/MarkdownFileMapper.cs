// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Modules.MarkdownFiles.DataBase;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.MarkdownFiles.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<MarkdownFileMapper>(ServiceLifetime.Singleton)]
public class MarkdownFileMapper : ResponseMapper<MarkdownFileResponse, MarkdownFileModel> {
    public override MarkdownFileResponse FromEntity(MarkdownFileModel markdownFile) => new() {
        Name = markdownFile.Name,
        Source = markdownFile.Source,
        Id = markdownFile.Id,
        CreatedDate = markdownFile.CreatedDate,
        LastModifiedDate = markdownFile.LastModifiedDate,
        OwnerId = markdownFile.OwnerId
    };
}
