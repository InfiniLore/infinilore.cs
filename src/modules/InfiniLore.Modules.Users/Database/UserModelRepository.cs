// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Core.Database;
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
    public async ValueTask<UserModel?> GetByUserNameAsync(string username, QueryConfig config = QueryConfig.None, CancellationToken ct = default) {
        if (username.IsNullOrWhiteSpace()) return null;
        
        // Form Query
        IQueryable<UserModel> query = GetConfiguredQueryable(config)
            .Where(model => model.UserName == username);
        
        // Execute Query
        UserModel? result = await query.FirstOrDefaultAsync(cancellationToken: ct);

        // Format Result
        return result;
    }

    public async ValueTask<bool> IsUserNameTakenAsync(string username, QueryConfig config = QueryConfig.None, CancellationToken ct = default) {
        if (username.IsNullOrWhiteSpace()) return false;
        
        IQueryable<UserModel> query = GetConfiguredQueryable(config)
            .Where(model => model.UserName == username);
        
        return await query.AnyAsync(cancellationToken: ct);
    }
}
