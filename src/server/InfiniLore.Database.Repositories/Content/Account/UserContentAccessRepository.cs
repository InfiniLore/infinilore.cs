// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using InfiniLore.Database.Models;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Account;
using InfiniLore.Server.Contracts.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Database.Repositories.Content.Account;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IUserContentAccessRepository>(ServiceLifetime.Scoped)]
public class UserContentAccessRepository(IUnitOfWork unitOfWork) : IUserContentAccessRepository {
    public async ValueTask<bool> UserHasKindAsync(Guid contentId, UserIdUnion accessorId, AccessKind accessKind, CancellationToken ct = default) {
        var dbContext = await unitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);
        UserContentAccessModel[] potentialAccesses = await dbContext.UserContentAccesses.Where(
            access => access.ContentId == contentId
                && access.UserId == accessorId.ToGuid()
        ).ToArrayAsync(cancellationToken: ct);

        return potentialAccesses.Any(access => access.AccessKind.HasFlag(accessKind));
    }
}
