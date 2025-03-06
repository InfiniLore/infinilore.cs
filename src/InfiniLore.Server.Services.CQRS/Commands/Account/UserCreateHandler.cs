// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.Services.CQRS.Notifications.Account;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace InfiniLore.Server.Services.CQRS.Commands.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public partial class UserCreateHandler(IUnitOfWorkFactory unitOfWorkFactory, ILoggerFactory factory, IValidator<InfiniLoreUser> validator, IMediator mediator) : IRequestHandler<UserCreateRequest, MediatorResponse<Guid>> {
    private readonly ILogger logger = factory.CreateLogger("ACCOUNT CreateUser");

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
    public async Task<MediatorResponse<Guid>> Handle(UserCreateRequest request, CancellationToken ct) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var userRepo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        // Create a new used based on the request
        var newUserId = Guid.CreateVersion7();
        var user = new InfiniLoreUser {
            Id = newUserId,
            Username = request.UserName
        };
        SetAuth0Id(user, request.Auth0UserId);

        // Validate the user model
        ValidationResult? validationResult = await validator.ValidateAsync(user, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Validation failed: {Reason}", validationResult.Errors );
            return MediatorResponse<Guid>.FromFailureString("Validation failed");
        }

        // Save to Db
        RepoResult result = await userRepo.AddAsync(user, ct);
        if (result.IsError) return MediatorResponse<Guid>.FromFailureString("Failed to save user to database");  ;

        var notification = new NewUserCreatedNotification(user.Id);
        await mediator.Publish(notification, ct);
        
        return newUserId;
    }
    
    internal void SetAuth0Id(InfiniLoreUser user, string auth0UserId) {
        Match match = Auth0Regex.Match(auth0UserId.ToLowerInvariant());
        if (!match.Success || !Auth0Handlers.TryGetValue(match.Groups[1].Value, out Action<InfiniLoreUser, string>? handler)) {
            logger.Warning("Unknown auth0 id: {Auth0Id}", auth0UserId);
            return;
        }

        handler(user, auth0UserId);
    }
}
