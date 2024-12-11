// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.Models.Content.Data.User;

namespace InfiniLore.Server.API.Controllers.Data.User.Lorescopes.GetLorescope;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class GetLorescopeMapper : ResponseMapper<LorescopeResponse, LorescopeModel> {
    public override LorescopeResponse FromEntity(LorescopeModel ls) => new(
        ls.Id,
        ls.OwnerId,
        ls.Name,
        ls.Description,
        ls.Multiverses.Select(selector: m => m.Id).ToArray()
    );
}
