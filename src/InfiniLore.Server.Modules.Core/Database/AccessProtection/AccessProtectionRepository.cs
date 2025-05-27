// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableScoped<IAccessProtectionRepository>]
public class AccessProtectionRepository : BasicModelRepository<AccessProtectionModel>, IAccessProtectionRepository {
    
}
