// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.UserData;
using JetBrains.Annotations;

namespace InfiniLore.Server.API.Models;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LoreScopeResponseMapper : ResponseMapper<LoreScopeResponse, LoreScopeModel> {
    public override LoreScopeResponse FromEntity(LoreScopeModel ls) => new(
        ls.Id,
        ls.UserId,
        ls.Name,
        ls.Description,
        ls.Multiverses.Select(m => m.Id).ToArray()
    );
}
