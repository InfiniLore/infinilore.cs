// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Database.Models;
using InfiniLore.Contracts.Database.Repositories.Content.Account;
using InfiniLore.Server.Types;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Database.Repositories.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IUserContentAccessRepository>(ServiceLifetime.Scoped)]
public class UserContentAccessRepository: UnitOfWorkRepository<ContentDbContext>, IUserContentAccessRepository {
    public async ValueTask<RepoResult> UserHasKindAsync(Guid contentId, Guid accessorId, AccessKind accessKind, CancellationToken ct = default) {
        ContentDbContext dbContext = GetDbContext();
        UserContentAccessModel[] potentialAccesses = await dbContext.UserContentAccesses
            .AsNoTracking()
            .Where(access => access.ContentId == contentId
                && access.UserId == accessorId
            ).ToArrayAsync(cancellationToken: ct);

        return potentialAccesses.Any(access => access.AccessKind.HasFlag(accessKind));
    }
}
