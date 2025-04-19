// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Api.Responses.Data.Project.MarkdownFiles;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Database.Models.Data.Project;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Api.Mappers.Data.Project.MarkdownFiles;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<MarkdownFilesMapper>(ServiceLifetime.Singleton)]
public class MarkdownFilesMapper : ResponseMapper<MarkdownFilesResponse, PaginatedData<MarkdownFile>> {
    public override MarkdownFilesResponse FromEntity(PaginatedData<MarkdownFile> entities) {
        var singleMapper = Resolve<MarkdownFileMapper>();

        MarkdownFileResponse[] items = entities.Items.Select(singleMapper.FromEntity).ToArray();
        var response = new MarkdownFilesResponse {
            Items = items,
            TotalCount = entities.TotalCount,
            TotalPages = entities.TotalPages,
            CurrentPage = entities.CurrentPage
        };

        return response;
    }
}
