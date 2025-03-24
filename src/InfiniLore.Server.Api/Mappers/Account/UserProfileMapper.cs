// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Api.Responses.Account;
using InfiniLore.Server.Database.Models.Account;

namespace InfiniLore.Server.Api.Mappers.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserProfileMapper : ResponseMapper<UserProfileResponse, InfiniLoreUser> {
    public override UserProfileResponse FromEntity(InfiniLoreUser entity) => new() {
        Username = entity.Username,
        Id = entity.Id,
        CreatedDate = entity.CreatedDate,
        LastModifiedDate = entity.LastModifiedDate
    };
}
