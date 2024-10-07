// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Database.Models.UserData;
using JetBrains.Annotations;

namespace InfiniLore.Server.API.Models;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class LoreScopeResponseMapper : ResponseMapper<IEnumerable<LoreScopeResponse>, IEnumerable<LoreScopeModel>> {
    public override IEnumerable<LoreScopeResponse> FromEntity(IEnumerable<LoreScopeModel> e) => e
        .Select(ls => new LoreScopeResponse(
            ls.Id,
            ls.UserId,
            ls.Name,
            ls.Description,
            ls.Multiverses.Select(m => m.Id).ToArray())
        );

    public override Task<IEnumerable<LoreScopeResponse>> FromEntityAsync(IEnumerable<LoreScopeModel> e, CancellationToken ct = new CancellationToken()) {
        return Task.FromResult(FromEntity(e));
    }
}

