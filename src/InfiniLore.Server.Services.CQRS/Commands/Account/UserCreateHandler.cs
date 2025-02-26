// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.CQRS.Commands.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserCreateHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerFactory factory, IValidator<InfiniLoreUser> validator ) : IRequestHandler<UserCreateRequest, InfiniLoreUser?> {
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
            if (request.Auth0UserId.StartsWith("github")) user.Auth0Github = request.Auth0UserId;
            if (request.Auth0UserId.StartsWith("auth0")) user.Auth0MailPassword = request.Auth0UserId;

            // Validate the user model
            var validationResult = await validator.ValidateAsync(user, ct);
            if (!validationResult.IsValid) {
                logger.Warning("Validation failed: {Reason}", validationResult.Errors );
                return null;
            }

            // Save to Db
            RepoResult result = await userRepo.TryAddAsync(user, ct);
            if (result.IsFailure) return null;

            RepoResult<InfiniLoreUser> userResult = await userRepo.TryGetByIdAsync(newUserId, ct);
            return !userResult.TryGetAsSuccess(out InfiniLoreUser? userSuccess) ? null : userSuccess;

        }
        catch (Exception e) {
            logger.Error(e, "An error occurred while trying to create a user");
            return null;
        }
    }
}
