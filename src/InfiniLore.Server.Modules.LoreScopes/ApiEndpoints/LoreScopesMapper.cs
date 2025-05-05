// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Modules.LoreScopes.Database;

namespace InfiniLore.Server.Modules.LoreScopes.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<LoreScopesMapper>]
public class LoreScopesMapper : ResponseMapper<LoreScopesResponse, PaginatedData<LoreScopeModel>> {
    public override LoreScopesResponse FromEntity(PaginatedData<LoreScopeModel> entities) {
        var singleMapper = Resolve<LoreScopeMapper>();

        LoreScopeResponse[] items = entities.Items.Select(singleMapper.FromEntity).ToArray();
        var response = new LoreScopesResponse {
            Items = items,
            TotalCount = entities.TotalCount,
            TotalPages = entities.TotalPages,
            CurrentPage = entities.CurrentPage
        };

        return response;
    }
}
