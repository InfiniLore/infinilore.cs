// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data.Repositories.Commands;
using InfiniLore.Server.Contracts.Services;
using InfiniLore.Server.Data.Models.UserData;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InfiniLore.Server.API.Controllers.LoreScopes.GetAll;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetAllLoreScopesEndpoint(IInfiniLoreUserCommands userRepository, IJwtParsingService jwtParsing, ILogger logger) :
    Endpoint<
        GetAllLoreScopesRequest,
        Results<
            Ok<IEnumerable<LoreScopeResponse>>,
            NotFound,
            ForbidHttpResult
        >,
        LoreScopeResponseMapper
    > {

    public override void Configure() {
        Get("/{UserId:guid}/lore-scopes/");
        Permissions("read:lore-scopes");
        // AllowAnonymous();
    }

    public async override Task<Results<Ok<IEnumerable<LoreScopeResponse>>, NotFound, ForbidHttpResult>> ExecuteAsync(GetAllLoreScopesRequest req, CancellationToken ct) {
        if (jwtParsing.TryGetPermissions(out string[]? permissions)) return TypedResults.Forbid();
        
        ResultMany<LoreScopeModel> result = await userRepository.GetLoreScopesAsync(req.UserId, ct);
        if (result.IsFailure || result.Values is null) return TypedResults.NotFound();

        return TypedResults.Ok(result.Values.Select(ls => Map.FromEntity(ls)));
    }
}
