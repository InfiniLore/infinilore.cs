// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Outcomes;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.Users.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class UserRepository : BaseModelRepository<UserModel> {
    public async ValueTask<Outcome<UserModel>> GetByUserNameAsync(string username, QueryConfig config = QueryConfig.None, CancellationToken ct = default) {
        DbSet<UserModel> dbSet = GetCachedDbSet<UserModel>();

        // Form Query
        IQueryable<UserModel> query = GetConfiguredQueryable(dbSet, config)
            .Where(model => model.UserName == username);
        
        // Execute Query
        UserModel? result = await query.FirstOrDefaultAsync(cancellationToken: ct);
        
        // Format Result
        return result is not null 
            ? Outcome<UserModel>.FromSuccess(result)
            : Outcome<UserModel>.FromError("Model not found");
    }
}
