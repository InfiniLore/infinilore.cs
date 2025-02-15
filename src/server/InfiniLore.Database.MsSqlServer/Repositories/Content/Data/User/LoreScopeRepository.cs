// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Database.Models.Content.Data.User;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.User;
using InfiniLore.Server.Types;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Database.MsSqlServer.Repositories.Content.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<ILorescopeRepository>(ServiceLifetime.Scoped)]
public class LorescopeRepository : UserContentRepository<LorescopeModel>, ILorescopeRepository {
    protected override IQueryable<LorescopeModel> IncludeOnGet(IQueryable<LorescopeModel> query) => query;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<RepoResult> IsValidNewNameAsync(Guid userId, string name, CancellationToken ct = default) {
        var dbContext = await UnitOfWork.GetDbContextAsync<MsSqlDbContext>(ct);

        LorescopeModel? existing = await dbContext.Lorescopes
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate: model => model.OwnerId == userId && model.Name == name, ct);

        if (existing != null) return "A lore scope with that name already exists";
        return new Success();
    }
}
