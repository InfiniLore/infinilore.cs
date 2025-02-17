// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Database.Models.Account;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Database.Repositories.Account;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IJwtRefreshTokenRepository>(ServiceLifetime.Scoped)]
public class JwtRefreshTokenRepository : UserDataRepository<JwtRefreshTokenData>, IJwtRefreshTokenRepository {
    
}
