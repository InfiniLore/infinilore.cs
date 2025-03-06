// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using InfiniLore.Server.DataSeeder.Options;
using InfiniLore.Server.Services.CQRS.Commands.Account;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace InfiniLore.Server.DataSeeder.Seeders;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<UserSeeder>(ServiceLifetime.Scoped)]
public class UserSeeder(IOptions<SeedingConfig> options, IUnitOfWorkFactory unitOfWorkFactory, ILogger logger, IMediator mediator) : Seeder{
    private readonly SeedingConfig _options = options.Value;
    private int _totalUsersToSeed = -1;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<bool> ShouldSeedAsync(CancellationToken ct = new()) {
        if (_options is not {Data.Users : var users, Enabled: true}) return logger.InformationAsFalse("User seeding is {State}", false);
        if (users.IsEmpty()) return logger.InformationAsFalse("User seeding is {State}", false);

        _totalUsersToSeed = users.Length;
        
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var repo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);
        
        RepoResult<InfiniLoreUser[]> result = await repo.TryGetAllByAuth0IdsAsync(ct: ct, users.Select(u => u.Auth0Id).ToHashSet());

        bool shouldSeed = result switch {
            // No users that are supposed to be seeded are present, meaning we have to seed all of them
            { IsError: true, AsError.Value: RepositoryFailures.ModelsNotFound } => true,
            // Some users are found but all of them
            { IsSuccess: true, AsSuccess: var foundUsers } when foundUsers.Length < _totalUsersToSeed => true,
            // All is already seeded
            _ => false
        };

        logger.Information("User seeding is {State}", shouldSeed);
        return shouldSeed;
    }

    public override async Task SeedAsync(CancellationToken ct = new()) {
        if (_options.Data is null) return;
        if (_totalUsersToSeed == -1) _totalUsersToSeed = _options.Data.Users.Length;
        if (_totalUsersToSeed == 0) return;
        
        await using IUnitOfWork unitOfWork = await unitOfWorkFactory.CreateWithTransactionAsync(ct);
        var repo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);
        
        foreach (SeedingUser userToBeSeeded in _options.Data.Users ) {
            // Only seed those which dont exist yet
            if (await repo.TryGetByAuth0IdAsync(userToBeSeeded.Auth0Id, ct) is { IsSuccess: true }) {
                logger.Information("User {Auth0Id} already exists, skipping", userToBeSeeded.Auth0Id);
                continue;
            }
            
            // Some complicated task to send out the original request to create a user
            await mediator.Send(new UserCreateRequest(userToBeSeeded.Auth0Id, userToBeSeeded.Username), ct);
        }
    }
}
