// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.CQRS.Queries.Account.Auth0;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class Auth0UserExistsHandler(IReadonlyUnitOfWorkFactory factory, ILoggerFactory loggerFactory) : IRequestHandler<Auth0UserExistsQuery, bool> {
    private readonly ILogger logger = loggerFactory.CreateLogger("ACCOUNT Auth0UserExist");

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task<bool> Handle(Auth0UserExistsQuery request, CancellationToken ct) {
        if (request.Auth0UserId.IsNullOrEmpty()) return false;

        try {
            await using IReadonlyUnitOfWork unitOfWork = factory.Create();
            var userRepository = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

            RepoResult<InfiniLoreUser> result = await userRepository.TryGetByAuth0IdAsync(request.Auth0UserId, ct);
            return result.IsSuccess;
        }
        catch (Exception e) {
            logger.Error(e, "An error occurred while trying to check if a user exists");
            return false;
        }
    }
}
