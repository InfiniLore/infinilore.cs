// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Core.Database;
using InfiniLore.Core.Messaging;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Users.Messaging;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CreateUserCommandHandler(
    IValidator<UserModel> validator,
    IUnitOfWork<InfiniLoreDb> unitOfWork,
    ILogger<CreateUserCommandHandler> logger
) : BaseCommandHandler<CreateUserCommand, Guid> {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<Outcome<Guid>> ExecuteAsync(CreateUserCommand command, CancellationToken ct) {
        var userModel = new UserModel {
            UserName = command.Username
        };

        ValidationResult? validationResult = await validator.ValidateAsync(userModel, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Validation failed for user creation: {errors}", validationResult.Errors);
            return Outcome.FromError(new ValidationFailed(validationResult.Errors.Select(e => e.ErrorMessage).ToArray()));
        }

        try {
            await unitOfWork.TryCreateTransactionAsync(ct);
            var userRepo = unitOfWork.GetRepository<UserRepository>();
            
            // Check for duplicate username
            bool isUserNameTaken = await userRepo.IsUserNameTakenAsync(command.Username, ct:ct);
            if (isUserNameTaken) {
                await unitOfWork.TryRollbackTransactionAsync(ct);
                logger.Warning("Another user with username {username} already exists", command.Username);
                return Outcome.Failure;
            }

            RepoOutcome outcome = await userRepo.AddAsync(userModel, ct);
            if (outcome.IsError) {
                await unitOfWork.TryRollbackTransactionAsync(ct);
                logger.Warning("Failed to create user: {error}", outcome.AsError);
                return Outcome.Failure;
            }

            await unitOfWork.TryCommitTransactionAsync(ct);
            await unitOfWork.SaveChangesAsync(ct);
            return Outcome.FromSuccess(userModel.Id);
        }
        catch (Exception exception) {
            await unitOfWork.TryRollbackTransactionAsync(ct);
            logger.Error(exception, "Failed to create user.");
            return Outcome.Failure;
        }
    }
}
