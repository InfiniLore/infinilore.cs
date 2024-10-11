// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Repositories;
using InfiniLore.Server.Data.Models.UserData;
using InfiniLoreLib.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace InfiniLore.Server.API.Controllers.LoreScopes.GetSpecificLoreScope;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetSpecificLoreScopeEndpoint(IInfiniLoreUserRepository userRepository) :
    Endpoint<
        GetSpecificLoreScopeRequest,
        Results<
            Ok<LoreScopeResponse>,
            NotFound
        >,
        LoreScopeResponseMapper
    > {

    public override void Configure() {
        Get("/api/{UserId:guid}/lore-scopes/{LoreScopeId:guid}");
        AllowAnonymous();
    }

    public async override Task<Results<Ok<LoreScopeResponse>, NotFound>> ExecuteAsync(GetSpecificLoreScopeRequest req, CancellationToken ct) {
        ResultMany<LoreScopeModel> result = await userRepository.GetLoreScopesAsync(req.UserId, ct);
        if (result.IsFailure || result.Values is null)  return TypedResults.NotFound();
        
        LoreScopeModel? scope = result.Values.FirstOrDefault(x => x.Id == req.LoreScopeId);
        if (scope is null) return TypedResults.NotFound();
        
        return TypedResults.Ok(Map.FromEntity(scope));
    }
}
