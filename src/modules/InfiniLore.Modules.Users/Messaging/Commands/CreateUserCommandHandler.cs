// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Core.Database;
using InfiniLore.Core.Messaging;
using InfiniLore.Core.Outcomes;
using InfiniLore.Modules.Users.Database;
using InfiniLore.Modules.Users.Messaging.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Modules.Users.Messaging.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class CreateUserCommandHandler(IServiceScopeFactory serviceScopeFactory) : BaseCommandHandler<CreateUserCommand, Guid>(serviceScopeFactory) {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<Outcome<Guid>> ExecuteAsync(IServiceProvider provider, CreateUserCommand command, CancellationToken ct) {
        var validator = provider.GetRequiredService<IValidator<UserModel>>();
        var unitOfWorkFactory = provider.GetRequiredService<IUnitOfWorkFactory<InfiniLoreDb>>();
        var logger = provider.GetRequiredService<ILogger<CreateUserCommandHandler>>();
        
        var userModel = new UserModel {
            UserName = command.Username
        };

        ValidationResult? validationResult = await validator.ValidateAsync(userModel, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Validation failed for user creation: {errors}", validationResult.Errors);
            return ErrorOutcome.FromValidationFailed(new ValidationFailed(validationResult.Errors.Select(e => e.ErrorMessage).ToArray()));
        }
        
        await using IUnitOfWork<InfiniLoreDb> unitOfWork = unitOfWorkFactory.Create();

        try {
            await unitOfWork.TryCreateTransactionAsync(ct);
            var userRepo = await unitOfWork.GetRepositoryAsync<UserModelRepository>(ct);
            
            // Check for duplicate username
            bool isUserNameTaken = await userRepo.IsUserNameTakenAsync(command.Username, ct:ct);
            if (isUserNameTaken) {
                await unitOfWork.TryRollbackTransactionAsync(ct);
                logger.Warning("Another user with username {username} already exists", command.Username);
                return ErrorOutcome.Failure;
            }

            RepoOutcome<Guid> outcome = await userRepo.AddAsync(userModel, ct);
            if (outcome.IsError) {
                await unitOfWork.TryRollbackTransactionAsync(ct);
                logger.Warning("Failed to create user: {error}", outcome.AsError);
                return ErrorOutcome.Failure;
            }

            await unitOfWork.TryCommitTransactionAsync(ct);
            await unitOfWork.SaveChangesAsync(ct);
        }
        catch (Exception exception) {
            await unitOfWork.TryRollbackTransactionAsync(ct);
            logger.Error(exception, "Failed to create user.");
            return ErrorOutcome.Failure;
        }

        try {
            var @event = new UserCreatedEvent(userModel.Id);
            await @event.PublishAsync(Mode.WaitForNone, cancellation: ct);
        }
        catch (Exception e) {
            logger.Warning(e, "Failed to publish UserCreatedEvent.");
        }
        
        return userModel.Id;
    }
}
