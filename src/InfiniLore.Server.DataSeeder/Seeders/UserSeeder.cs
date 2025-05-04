// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using Auth0.ManagementApi.Models;
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Server.DataSeeder.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace InfiniLore.Server.DataSeeder.Seeders;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<UserSeeder>(ServiceLifetime.Scoped)]
public class UserSeeder(IOptions<SeedingConfig> options, IReadonlyUnitOfWorkFactory readonlyUnitOfWorkFactory, ILogger<UserSeeder> logger) : Seeder {
    private readonly SeedingConfig _options = options.Value;
    private readonly ConcurrentQueue<SeedingUser> _usersToSeed = new();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override async Task<bool> ShouldSeedAsync(CancellationToken ct = new()) {
        if (_options is not { Data.Users : var users, Enabled: true }) return logger.InformationAsFalse("User seeding is {State}", false);
        if (users.IsEmpty()) return logger.InformationAsFalse("User seeding is {State}", false);

        await using IReadonlyUnitOfWork unitOfWork = readonlyUnitOfWorkFactory.Create();
        var repo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result<InfiniLoreUser[]> result = await repo.TryGetAllByAuth0IdsAsync(default, ct, users.Select(u => u.Auth0Id).ToHashSet());
        if (!result.TryGetAsSuccess(out InfiniLoreUser[]? foundUsers)) return logger.InformationAsTrue("User seeding is {State}", true);

        HashSet<string> foundUserAuth0Ids = foundUsers.SelectMany(user => user.GetAuth0Ids()).ToHashSet();

        // Assign the users to _usersToSeed queue for seeding.
        foreach (SeedingUser user in users.Where(user => !foundUserAuth0Ids.Contains(user.Auth0Id))) {
            _usersToSeed.Enqueue(user);
        }

        bool shouldSeed = !_usersToSeed.IsEmpty;

        logger.Information("User seeding is {State}", shouldSeed);
        return shouldSeed;
    }

    public override async Task SeedAsync(CancellationToken ct = new()) {
        int totalUsersToSeed = _usersToSeed.Count;
        if (totalUsersToSeed == 0) return;

        var tasks = new Task[totalUsersToSeed];
        int i = 0;
        while (_usersToSeed.TryDequeue(out SeedingUser? userToBeSeeded)) {
            tasks[i++] = new UserCreateRequest(userToBeSeeded.Auth0Id, userToBeSeeded.Username).ExecuteAsync(ct: ct);
        }

        await Task.WhenAny(tasks);
    }
}
