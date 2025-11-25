// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Application;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class InfiniLoreBuilder {
    public static IServiceCollection AddInfiniLoreApplication(this IServiceCollection services) {
        
        return services;
    }

    public static WebApplication UseInfiniLoreApplication(this WebApplication app) {
        
        EnsureDatabaseCreated(app);

        return app;
    }
    
    private static void EnsureDatabaseCreated(WebApplication app) {
        using IServiceScope scope = app.Services.CreateScope();
        using var db = scope.ServiceProvider.GetRequiredService<InfiniLoreDb>();
        db.Database.EnsureCreated();
    }
}
