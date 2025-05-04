// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Server.Modules.Users.Database;

namespace InfiniLore.Server.Modules.Users.ApiEndpoints;
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
