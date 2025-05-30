// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Modules.Core.Server.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IAccessProtectionRepository>]
public class AccessProtectionRepository : BasicModelRepository<AccessProtectionModel>, IAccessProtectionRepository {
    protected override IQueryable<AccessProtectionModel> AlwaysInclude(IQueryable<AccessProtectionModel> query)
        => query.Include(access => access.Rules);
}
