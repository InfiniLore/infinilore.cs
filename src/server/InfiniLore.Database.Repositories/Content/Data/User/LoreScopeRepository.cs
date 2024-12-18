// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.DependencyInjection;
using AterraEngine.Unions;
using InfiniLore.Database.Models.Content.Data.User;
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.User;
using InfiniLore.Server.Contracts.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepoResult=InfiniLore.Server.Contracts.Database.Repositories.RepoResult;

namespace InfiniLore.Database.Repositories.Content.Data.User;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<ILorescopeRepository>(ServiceLifetime.Scoped)]
public class LorescopeRepository(IUnitOfWork unitOfWork) : UserContentRepository<LorescopeModel>(unitOfWork), ILorescopeRepository {
    private readonly IUnitOfWork _unitOfWork1 = unitOfWork;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask<RepoResult> IsValidNewNameAsync(UserIdUnion userId, string name, CancellationToken ct = default) {
        var dbContext = await _unitOfWork1.GetDbContextAsync<MsSqlDbContext>(ct);

        LorescopeModel? existing = await dbContext.Lorescopes
            .FirstOrDefaultAsync(predicate: model => model.OwnerId == userId.ToGuid() && model.Name == name, ct);

        if (existing != null) return "A lorescope with that name already exists";

        return new Success();
    }
}
