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
[InjectableSingleton<LoreScopeMapper>]
public class LoreScopeMapper : ResponseMapper<LoreScopeResponse, LoreScope> {
    public override LoreScopeResponse FromEntity(LoreScope loreScope) => new() {
        Name = loreScope.Name,
        Description = loreScope.ShortDescription,
        Id = loreScope.Id,
        CreatedDate = loreScope.CreatedDate,
        LastModifiedDate = loreScope.LastModifiedDate,
        OwnerId = loreScope.OwnerId
    };
}
