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
        
        // Form Query
        IQueryable<UserModel> query = GetConfiguredQueryable(config)
            .Where(model => model.UserName == username);
        
        // Execute Query
        UserModel? result = await query.FirstOrDefaultAsync(cancellationToken: ct);
        
        // Format Result
        if (result is null) return RepoOutcome.NotFound;
        return result;
    }

    public async ValueTask<bool> IsUserNameTakenAsync(string username, CancellationToken ct = default) {
        IQueryable<UserModel> query = GetConfiguredQueryable(QueryConfig.None)
            .Where(model => model.UserName == username);
        
        return await query.AnyAsync(cancellationToken: ct);
        
    }
}
