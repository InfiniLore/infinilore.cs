// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Services.Mediator.Notifications.Account;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Wolverine;

namespace InfiniLore.Server.Services.Mediator.Commands.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static partial class UserCreateHandler{
    private static readonly Dictionary<string, Action<InfiniLoreUser, string>> Auth0Handlers = new() {
        { "google", (user, id) => user.Auth0IdGoogle = id },
        { "github", (user, id) => user.Auth0Github = id },
        { "auth0", (user, id) => user.Auth0MailPassword = id }
    };

    [GeneratedRegex("^(google|github|auth0)")]
    private static partial Regex Auth0Regex { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static async Task<MediatorResponse<Guid>> HandleAsync(
        // Message
        UserCreateMediatorRequest message,
        // Services
        IUnitOfWorkFactory unitOfWorkFactory, ILogger logger, IValidator<InfiniLoreUser> validator, IMessageBus messageBus,
        // CT
        CancellationToken ct
    ) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var userRepo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        // Create a new used based on the request
        var newUserId = Guid.CreateVersion7();
        var user = new InfiniLoreUser {
            Id = newUserId,
            Username = message.UserName
        };

        SetAuth0Id(user, message.Auth0UserId, logger);

        // Validate the user model
        ValidationResult? validationResult = await validator.ValidateAsync(user, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Validation failed: {Reason}", validationResult.Errors);
            return MediatorResponse<Guid>.FromErrorString("Validation failed");
        }

        // Save to Db
        Result result = await userRepo.AddAsync(user, ct);
        if (result.IsError) return MediatorResponse<Guid>.FromErrorString("Failed to save user to database");

        await messageBus.PublishAsync(new NewUserCreatedEvent(user.Id));
        return newUserId;
    }

    private static void SetAuth0Id(InfiniLoreUser user, string auth0UserId, ILogger logger) {
        Match match = Auth0Regex.Match(auth0UserId.ToLowerInvariant());
        if (!match.Success || !Auth0Handlers.TryGetValue(match.Groups[1].Value, out Action<InfiniLoreUser, string>? handler)) {
            logger.Warning("Unknown auth0 id: {Auth0Id}", auth0UserId);
            return;
        }

        handler(user, auth0UserId);
    }
}
