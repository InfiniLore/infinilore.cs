// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using AterraEngine.Unions;
using InfiniLore.Database.Models.Content.Data.User;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.User;
using InfiniLore.Server.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Database.Repositories.Content.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<ILorescopeRepository>(ServiceLifetime.Scoped)]
public class LorescopeRepository : UserContentRepository<LorescopeModel>, ILorescopeRepository {
    protected override IQueryable<LorescopeModel> IncludeOnGet(IQueryable<LorescopeModel> query) => query
        .Include(model => model.Multiverses)
            .ThenInclude(multiverse => multiverse.Universes)
        .Include(model => model.Owner)    
    ;

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
