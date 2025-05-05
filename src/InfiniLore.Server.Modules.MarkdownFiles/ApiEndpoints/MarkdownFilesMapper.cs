// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Modules.MarkdownFiles.Database;
using InfiniLore.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.MarkdownFiles.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<MarkdownFilesMapper>(ServiceLifetime.Singleton)]
public class MarkdownFilesMapper : ResponseMapper<MarkdownFilesResponse, PaginatedData<MarkdownFileModel>> {
    public override MarkdownFilesResponse FromEntity(PaginatedData<MarkdownFileModel> entities) {
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
