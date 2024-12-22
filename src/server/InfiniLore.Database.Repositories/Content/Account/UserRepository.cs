// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using InfiniLore.Database.Models.Content.Account;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories;
using InfiniLore.Server.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace InfiniLore.Database.Repositories.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IUserRepository>(ServiceLifetime.Scoped)]
public class UserRepository(
    IUnitOfWork unitOfWork,
    UserManager<InfiniLoreUser> userManager
) : IUserRepository, IRepository {
    /// <inheritdoc />
    public async ValueTask<RepoResult<InfiniLoreUser>> TryGetByClaimsPrincipalAsync(ClaimsPrincipal principal, CancellationToken ct = default) {
        if (await userManager.GetUserAsync(principal) is not {} user) return "User not found.";

        return user;
    }

    /// <inheritdoc />
    public async ValueTask<RepoResult<InfiniLoreUser>> TryGetByIdAsync(Guid userId, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);

        InfiniLoreUser? result = await dbContext.Users
            .FirstOrDefaultAsync(predicate: u => u.Id == userId, ct);

        if (result is null) return "User not found.";

        return result;
    }

    public async ValueTask<RepoResult<InfiniLoreUser>> UserHasAllRolesAsync(Guid userId, IEnumerable<string> roles, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);

        // If the user hasn't been found yet, we need to actually grab it
        InfiniLoreUser? user = await dbContext.Users
            .FirstOrDefaultAsync(predicate: u => u.Id == userId, ct);
        if (user is null) return "User not found.";

        // Normalize the names
        HashSet<string> roleSet = roles
            .Select(r => r.ToUpperInvariant())
            .ToHashSet();

        // Because we already got a user (either fed to this method or from the database), we can assume that the user exists.
        HashSet<string> userWithRoles = await dbContext.UserRoles
            // If the user is an InfiniLoreUser, we can use the NoTracking query to avoid loading the user's roles
            .Where(ur => ur.UserId == userId)
            .Join(dbContext.Roles,
                outerKeySelector: ur => ur.RoleId,
                innerKeySelector: r => r.Id,
                resultSelector: (ur, r) => r.NormalizedName!)
            .ToHashSetAsync(ct);

        if (!userWithRoles.IsSupersetOf(roleSet)) {
            return "User does not have all roles.";
        }
        
        return user;
    }
}
