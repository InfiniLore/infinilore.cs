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
[InjectableService<MarkdownFilesMapper>(ServiceLifetime.Singleton)]
public class MarkdownFilesMapper : ResponseMapper<MarkdownFilesResponse, PaginatedData<IMarkdownFile>> {
    public override MarkdownFilesResponse FromEntity(PaginatedData<IMarkdownFile> entities) {
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
