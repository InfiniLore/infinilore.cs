// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Database.Models.Data.User;

namespace InfiniLore.Server.Api.Responses.Data.User.LoreScopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
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
