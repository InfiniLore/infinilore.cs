// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Modules.LoreScopes.Database;

namespace InfiniLore.Modules.LoreScopes.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ------------------------------------------------- --------------------------------------------------------------------
[InjectableSingleton<LoreScopeMapper>]
public class LoreScopeMapper : ResponseMapper<LoreScopeResponse, LoreScopeModel> {
    public override LoreScopeResponse FromEntity(LoreScopeModel loreScope) => new() {
        Name = loreScope.Name,
        Id = loreScope.Id,
        CreatedDate = loreScope.CreatedDate,
        LastModifiedDate = loreScope.LastModifiedDate,
        OwnerId = loreScope.OwnerId,
        Description = loreScope.Description,
        ImageUrl = loreScope.S3PosterImageUrl
    };
}
