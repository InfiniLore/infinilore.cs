// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.Users.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<UserRepository>]
public class UserRepository : BaseModelRepository<UserModel> {
    public async ValueTask<RepoOutcome<UserModel>> GetByUserNameAsync(string username, QueryConfig config = QueryConfig.None, CancellationToken ct = default) {
        if (username.IsNullOrWhiteSpace()) return RepoOutcome.NotFound;
        
        DbSet<UserModel> dbSet = GetCachedDbSet<UserModel>();

        // Form Query
        IQueryable<UserModel> query = GetConfiguredQueryable(dbSet, config)
            .Where(model => model.UserName == username);
        
        // Execute Query
        UserModel? result = await query.FirstOrDefaultAsync(cancellationToken: ct);
        
        // Format Result
        return result is not null 
            ? result
            : RepoOutcome.NotFound;
    }

    public async ValueTask<bool> IsUserNameTakenAsync(string username, CancellationToken ct = default) {
        DbSet<UserModel> dbSet = GetCachedDbSet<UserModel>();

        IQueryable<UserModel> query = GetConfiguredQueryable(dbSet, QueryConfig.None)
            .Where(model => model.UserName == username);
        
        return await query.AnyAsync(cancellationToken: ct);
        
    }
}
