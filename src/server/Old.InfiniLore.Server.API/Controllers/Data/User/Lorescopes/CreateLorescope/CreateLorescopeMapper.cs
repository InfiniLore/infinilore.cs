// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Old.InfiniLore.Database.Models.Content.Data.User;

namespace Old.InfiniLore.Server.API.Controllers.Data.User.Lorescopes.CreateLorescope;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class CreateLorescopeMapper : Mapper<CreateLorescopeRequest, LorescopeResponse, LorescopeModel> {
    public override LorescopeResponse FromEntity(LorescopeModel ls) => new(
        ls.Id,
        ls.OwnerId,
        ls.Name,
        ls.Description
    );

    public override LorescopeModel ToEntity(CreateLorescopeRequest request)
        => new() {
            OwnerId = request.UserId,
            Name = request.Name,
            Description = request.Description
        };
}
