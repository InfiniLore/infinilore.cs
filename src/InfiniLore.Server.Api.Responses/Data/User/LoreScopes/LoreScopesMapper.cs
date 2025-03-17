// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Database.Models.Data.User;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Api.Responses.Data.User.LoreScopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<LoreScopesMapper>(ServiceLifetime.Singleton)]
public class LoreScopesMapper : ResponseMapper<LoreScopesResponse, PaginatedResult<LoreScope>> {
    public override LoreScopesResponse FromEntity(PaginatedResult<LoreScope> entities) {
        var singleMapper = Resolve<LoreScopeMapper>();

        return new LoreScopesResponse {
            Items = entities.Items.Select(singleMapper.FromEntity).ToArray(),
            TotalCount = entities.TotalCount,
            TotalPages = entities.TotalPages,
            CurrentPage = entities.CurrentPage
        };
    }
}
