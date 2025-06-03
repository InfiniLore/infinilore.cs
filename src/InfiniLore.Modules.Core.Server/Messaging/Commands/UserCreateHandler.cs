// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Server.Messaging.Handlers;
using InfiniLore.Modules.Core.Server.Messaging.Notifications;
using InfiniLore.Modules.Core.Shared;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace InfiniLore.Modules.Core.Server.Messaging.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public partial class UserCreateHandler(
    IUnitOfWorkFactory unitOfWorkFactory,
    ILogger<UserCreateHandler> logger,
    IValidator<InfiniLoreUserModel> validator,
    IAccessProtectionRules protectionRules
) : AccessProtectedCommandHandler<CreateInfiniLoreUserRequest, Guid>(logger) {
    private static readonly Dictionary<string, Action<InfiniLoreUserModel, string>> Auth0Handlers = new() {
        { "google", (user, id) => user.Auth0IdGoogle = id },
        { "github", (user, id) => user.Auth0Github = id },
        { "auth0", (user, id) => user.Auth0MailPassword = id }
    };

    [GeneratedRegex("^(google|github|auth0)")]
    private static partial Regex Auth0Regex { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    protected override async Task<Shared.Outcome<Guid>> HandleCommandAsync(CreateInfiniLoreUserRequest command, CancellationToken ct = default){
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var userRepo = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        // Create a new used based on the request
        var newUserId = Guid.CreateVersion7();
        var user = new InfiniLoreUserModel {
            Id = newUserId,
            Username = command.UserName
        };

        SetAuth0Id(user, command.Auth0UserId, logger);

        // Validate the user model
        ValidationResult? validationResult = await validator.ValidateAsync(user, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Validation failed: {Reason}", validationResult.Errors);
            return Shared.Outcome<Guid>.FromError("Validation failed");
        }

        // Save to Db
        Outcome result = await userRepo.AddAsync(user, ct);
        if (result.IsError) return Shared.Outcome<Guid>.FromError("Failed to save user to database");

        await new InfiniLoreUserCreatedEvent(user.Id).PublishAsync(Mode.WaitForAll, ct);
        return newUserId;
    }

    private static void SetAuth0Id(InfiniLoreUserModel user, string auth0UserId, ILogger logger) {
        Match match = Auth0Regex.Match(auth0UserId.ToLowerInvariant());
        if (!match.Success || !Auth0Handlers.TryGetValue(match.Groups[1].Value, out Action<InfiniLoreUserModel, string>? handler)) {
            logger.Warning("Unknown auth0 id: {Auth0Id}", auth0UserId);
            return;
        }

        handler(user, auth0UserId);
    }
    
    protected override ValueTask<bool> ValidateAccessAsync(CreateInfiniLoreUserRequest command, CancellationToken ct = default) 
        => protectionRules.IsServerAsync(command.AccessingUser, ct);  
}
