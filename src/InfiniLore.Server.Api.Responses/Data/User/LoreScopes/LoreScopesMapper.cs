// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Database.Models.Data.User;

namespace InfiniLore.Server.Api.Responses.Data.User.LoreScopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
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
