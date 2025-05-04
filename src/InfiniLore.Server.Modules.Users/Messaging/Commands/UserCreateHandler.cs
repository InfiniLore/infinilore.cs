// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Modules.Core.Messaging;
using InfiniLore.Server.Modules.Users.Database;
using InfiniLore.Server.Modules.Users.Messaging.Noticiations;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace InfiniLore.Server.Modules.Users.Messaging.Commands;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public partial class UserCreateHandler(IUnitOfWorkFactory unitOfWorkFactory, ILogger<UserCreateHandler> logger, IValidator<InfiniLoreUser> validator) : CommandHandler<UserCreateRequest, MessageResponse<Guid>> {
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
    public override async Task<MessageResponse<Guid>> ExecuteAsync(UserCreateRequest command, CancellationToken ct = default) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var userRepo = await unitOfWork.GetRepositoryAsync<IInfiniLoreUserRepository>(ct);

        // Create a new used based on the request
        var newUserId = Guid.CreateVersion7();
        var user = new InfiniLoreUser {
            Id = newUserId,
            Username = command.UserName
        };

        SetAuth0Id(user, command.Auth0UserId, logger);

        // Validate the user model
        ValidationResult? validationResult = await validator.ValidateAsync(user, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Validation failed: {Reason}", validationResult.Errors);
            return MessageResponse<Guid>.FromErrorString("Validation failed");
        }

        // Save to Db
        Result result = await userRepo.AddAsync(user, ct);
        if (result.IsError) return MessageResponse<Guid>.FromErrorString("Failed to save user to database");

        await new NewUserCreatedEvent(user.Id).PublishAsync(Mode.WaitForAll, ct);
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
