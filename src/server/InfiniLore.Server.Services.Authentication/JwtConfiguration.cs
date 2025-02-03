// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Services.Authentication;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IJwtConfiguration>(ServiceLifetime.Singleton)]
public class JwtConfiguration(IConfiguration configuration) : IJwtConfiguration {
    public int AccessExpiresInMinutes {get;} = int.Parse(configuration["Jwt:AccessExpiresInMinutes"]!);
    public string Audience {get;} = configuration["Jwt:Audience"]!;
    public string Issuer {get;} = configuration["Jwt:Issuer"]!;
    public string Key {get;} = configuration["Jwt:Key"]!;
    public int RefreshExpiresInDays {get;} = int.Parse(configuration["Jwt:RefreshExpiresInDays"]!);
}
