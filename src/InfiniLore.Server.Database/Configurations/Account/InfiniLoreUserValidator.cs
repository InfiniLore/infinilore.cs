// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using FluentValidation;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Database.Configurations.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IValidator<InfiniLoreUser>>(ServiceLifetime.Scoped)]
public class InfiniLoreUserValidator : AbstractValidator<InfiniLoreUser> {
    private readonly ILogger _logger;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public InfiniLoreUserValidator(IUnitOfWorkFactory unitOfWorkFactory, ILoggerFactory factory) {
        _unitOfWorkFactory = unitOfWorkFactory;
        _logger = factory.CreateLogger("VALIDATE InfiniLoreUser");

        RuleFor(x => x.Auth0IdGoogle)
            .NotEmpty().WithMessage("Auth0 ID Google is required")
            .MaximumLength(InfiniLoreUser.Defaults.Auth0IdGoogleMaxLength).WithMessage($"Auth0 ID Google must be less than {InfiniLoreUser.Defaults.Auth0IdGoogleMaxLength} characters");

        RuleFor(x => x.Auth0Github)
            .NotEmpty().WithMessage("Auth0 ID Github is required")
            .MaximumLength(InfiniLoreUser.Defaults.Auth0IdGithubMaxLength).WithMessage($"Auth0 ID Github must be less than {InfiniLoreUser.Defaults.Auth0IdGithubMaxLength} characters");

        RuleFor(x => x.Username)
            .MustAsync(CheckValidUsernameAuth0).WithMessage("Username already exists")
            .NotEmpty().WithMessage("Username is required")
            .MaximumLength(InfiniLoreUser.Defaults.UsernameMaxLength).WithMessage("Username must be less than 64 characters");
    }

    private async Task<bool> CheckValidUsernameAuth0(InfiniLoreUser user, string userName, CancellationToken ct) {
        try {
            await using IUnitOfWork unitOfWork = _unitOfWorkFactory.Create();
            var repo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);
            return await repo.IsUsernameNotTakenAsync(userName, user.Id, ct);
        }
        catch (Exception ex) {
            _logger.Error(ex, "Error checking username existence in Auth0");
            return false;// Consider failing validation if the check cannot complete.
        }
    }
}
