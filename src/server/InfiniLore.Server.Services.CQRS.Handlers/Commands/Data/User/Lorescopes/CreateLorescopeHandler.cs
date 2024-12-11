// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using InfiniLore.Database.Models.Content.UserData;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.User;
using InfiniLore.Server.Contracts.Services.Auth.Authorization;
using InfiniLore.Server.Services.CQRS.Requests.Commands;
using MediatR;

namespace InfiniLore.Server.Services.CQRS.Handlers.Commands.Data.User.Lorescopes;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CreateLorescopeHandler(
    ILorescopeRepository lorescopeRepository,
    IDbUnitOfWork<MsSqlDbContext> unitOfWork,
    IUserContentAuthorizationService authService
) : IRequestHandler<CreateLorescopeCommand, SuccessOrFailure<LorescopeModel>> {

    public async Task<SuccessOrFailure<LorescopeModel>> Handle(CreateLorescopeCommand request, CancellationToken ct) {
        try {
            if (!await authService.InDevelopmentAsync()) {
                if (!await authService.ValidateIsOwnerAsync(request.Lorescope.OwnerId, ct)) return "Access Denied";
            }

            // Pre-check if we can use the name
            // Done to get more human-readable error strings back
            RepoResult resultCanUseName = await lorescopeRepository.IsValidNewNameAsync(request.Lorescope.OwnerId, request.Lorescope.Name, ct);
            if (!resultCanUseName) return resultCanUseName.AsFailure;

            // Actually add the lore scope to the db
            RepoResult<LorescopeModel> resultAddition = await lorescopeRepository.TryAddWithResultAsync(request.Lorescope, ct);
            if (!resultAddition) return resultAddition.AsFailure;

            await unitOfWork.CommitAsync(ct);

            // Because we already checked for IsFailure above, we know that the result is a Success
            return resultAddition.AsSuccess;
        }
        catch {
            return "An unknown error occurred";
        }
    }
}
