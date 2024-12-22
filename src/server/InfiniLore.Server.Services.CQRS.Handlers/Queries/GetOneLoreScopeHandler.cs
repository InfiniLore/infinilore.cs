// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Database.Models.Content.Data.User;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.User;
using InfiniLore.Server.Contracts.Services.Auth.Authorization;
using InfiniLore.Server.Services.CQRS.Requests.Queries;
using InfiniLore.Server.Types;
using MediatR;
using Serilog;

namespace InfiniLore.Server.Services.CQRS.Handlers.Queries;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class GetOneLorescopeHandler(
    ILorescopeRepository lorescopeRepository,
    ILogger logger,
    IUserContentAuthorizationService authService
) : IRequestHandler<GetOneLorescopeQuery, SuccessOrFailure<LorescopeModel>> {

    public async Task<SuccessOrFailure<LorescopeModel>> Handle(GetOneLorescopeQuery request, CancellationToken ct) {
        try {
            if (!await authService.HasAccessRead(request.LorescopeId, ct)) return "Access Denied";

            RepoResult<LorescopeModel> result = await lorescopeRepository.TryGetByHashedTokenAsync(request.LorescopeId, ct);
            return result.ToSuccessOrFailure();
        }
        catch (Exception e) {
            logger.Error(e, "An error occurred while trying to get a lore scope");
            return "An unknown error occurred";
        }
    }
}
