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
[InjectableScoped<IUserModelRepository>, InjectableScoped<UserModelRepository>]
public class UserModelRepository : BaseModelRepository<UserModel>, IUserModelRepository {

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<RepoOutcome<UserModel>> GetByUserNameAsync(string username, QueryConfig config = QueryConfig.None, CancellationToken ct = default) {
        if (username.IsNullOrWhiteSpace()) return RepoErrorOutcome.NotFound;
        
        // Form Query
        IQueryable<UserModel> query = GetConfiguredQueryable(config)
            .Where(model => model.UserName == username);
        
        // Execute Query
        UserModel? result = await query.FirstOrDefaultAsync(cancellationToken: ct);
        
        // Format Result
        if (result is null) return RepoErrorOutcome.NotFound;
        return result;
    }

    public async ValueTask<bool> IsUserNameTakenAsync(string username, CancellationToken ct = default) {
        IQueryable<UserModel> query = GetConfiguredQueryable(QueryConfig.None)
            .Where(model => model.UserName == username);
        
        return await query.AnyAsync(cancellationToken: ct);
        
    }
}
