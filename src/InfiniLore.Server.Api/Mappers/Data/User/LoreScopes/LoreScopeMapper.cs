// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Api.Responses.Data.User.LoreScopes;
using InfiniLore.Server.Database.Models.Data.Project;
using InfiniLore.Server.Database.Models.Data.User;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Api.Mappers.Data.User.LoreScopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<LoreScopeMapper>(ServiceLifetime.Singleton)]
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
