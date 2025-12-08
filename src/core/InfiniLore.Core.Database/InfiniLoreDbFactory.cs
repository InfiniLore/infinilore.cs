// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Reflection;

namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class InfiniLoreDbFactory {
    private static ImmutableArray<Assembly> _assembliesToImport = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IServiceCollection AddInfiniLoreDb(
        this IServiceCollection services, 
        Action<DbContextOptionsBuilder> optionsAction,
        params IEnumerable<Assembly> assembliesToImport
    ) {
        _assembliesToImport = [..assembliesToImport];
        
        services.AddDbContextFactory<InfiniLoreDb>(optionsAction);
        
        services.AddUnitOfWork<InfiniLoreDb>();
        services.AddReadonlyUnitOfWork<InfiniLoreDb>();
        
        return services;
    }
    
    internal static void ConfigureModel(ModelBuilder modelBuilder) {
        foreach (Assembly assembly in _assembliesToImport) {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
