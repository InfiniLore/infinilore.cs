// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Contracts;
using InfiniLore.Server.Database.Models.Data.User;

namespace InfiniLore.Server.Api.Endpoints.Data.User.Lorescopes;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class LoreScopesMapper : ResponseMapper<LoreScopesResponse, PaginatedResult<LoreScope>> {
    public override LoreScopesResponse FromEntity(PaginatedResult<LoreScope> entities) {
        var singleMapper = Resolve<LoreScopeMapper>();
        
        var response = new LoreScopesResponse {
            Items = entities.Items.Select(singleMapper.FromEntity).ToArray(),
            TotalCount = entities.TotalCount,
            TotalPages = entities.TotalPages,
            CurrentPage = entities.CurrentPage,
        };
        
        return response;
    }
}
