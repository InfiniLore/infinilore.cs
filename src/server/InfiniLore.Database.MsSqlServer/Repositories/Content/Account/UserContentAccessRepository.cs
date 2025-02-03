// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Database.Models;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Account;
using InfiniLore.Server.Types;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Database.MsSqlServer.Repositories.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IUserContentAccessRepository>(ServiceLifetime.Scoped)]
public class UserContentAccessRepository(IUnitOfWork unitOfWork) : IUserContentAccessRepository {
    public async ValueTask<RepoResult> UserHasKindAsync(Guid contentId, Guid accessorId, AccessKind accessKind, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        UserContentAccessModel[] potentialAccesses = await dbContext.UserContentAccesses
            .AsNoTracking()
            .Where(access => access.ContentId == contentId
                && access.UserId == accessorId
            ).ToArrayAsync(cancellationToken: ct);

        return potentialAccesses.Any(access => access.AccessKind.HasFlag(accessKind));
    }
}
