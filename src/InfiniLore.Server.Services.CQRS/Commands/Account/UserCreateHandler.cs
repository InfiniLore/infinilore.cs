// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.CQRS.Commands.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserCreateHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerFactory factory) : IRequestHandler<UserCreateRequest, InfiniLoreUser?> {
    private readonly ILogger logger = factory.CreateLogger("ACCOUNT CreateUser");

    public async Task<InfiniLoreUser?> Handle(UserCreateRequest request, CancellationToken ct) {
        try {
            await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
            var userRepo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

            // Create a new used based on the request
            var newUserId = Guid.CreateVersion7();
            var user = new InfiniLoreUser {
                Id = newUserId,
                Username = request.UserName
            };
            if (request.Auth0UserId.StartsWith("google")) user.Auth0IdGoogle = request.Auth0UserId;
            
            // Validate the user model
            // TODO Add Fluent Validation for InfiniLoreUser
            
            // Save to Db
            RepoResult result = await userRepo.TryAddAsync(user, ct);
            if (result.IsFailure) return null;
            
            RepoResult<InfiniLoreUser> userResult = await userRepo.TryGetByIdAsync(newUserId, ct);
            if (!userResult.TryGetAsSuccess(out InfiniLoreUser? userSuccess)) return null;
            return userSuccess;

        }
        catch (Exception e) {
            logger.Error(e, "An error occurred while trying to create a user");
            return null;
        }
    }
}
