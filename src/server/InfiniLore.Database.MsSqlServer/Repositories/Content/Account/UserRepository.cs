// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Database.Repositories.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IUserRepository>(ServiceLifetime.Scoped)]
public class UserRepository : Repository<InfiniLoreUser>, IUserRepository {
    public async ValueTask<RepoResult> UserHasAllRolesAsync(Guid userId, IEnumerable<string> roles, CancellationToken ct = default) {
        var dbContext = await UnitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);

        // If the user hasn't been found yet, we need to actually grab it
        InfiniLoreUser? user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate: u => u.Id == userId, ct);
        if (user is null) return "User not found.";

        // Normalize the names
        HashSet<string> roleSet = roles
            .Select(r => r.ToUpperInvariant())
            .ToHashSet();

        // Because we already got a user (either fed to this method or from the database), we can assume that the user exists.
        HashSet<string> userWithRoles = await dbContext.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Join(dbContext.Roles,
                outerKeySelector: ur => ur.RoleId,
                innerKeySelector: r => r.Id,
                resultSelector: (ur, r) => r.NormalizedName!)
            .ToHashSetAsync(ct);

        if (!userWithRoles.IsSupersetOf(roleSet)) {
            return "User does not have all roles.";
        }
        
        return true;
    }
}
