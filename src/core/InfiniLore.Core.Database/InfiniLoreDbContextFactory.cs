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
public static class InfiniLoreDbContextFactory {
    private static ImmutableArray<Assembly> _assembliesToImport = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static IServiceCollection AddInfiniLoreDbContext(
        this IServiceCollection services, 
        Action<DbContextOptionsBuilder> optionsAction,
        params ImmutableArray<Assembly> assembliesToImport
    ) {
        _assembliesToImport = assembliesToImport;
        
        services.AddDbContextFactory<InfiniLoreDbContext>(optionsAction);
        
        services.AddUnitOfWork<InfiniLoreDbContext>();
        services.AddReadonlyUnitOfWork<InfiniLoreDbContext>();
        
        return services;
    }
    
    internal static void ConfigureModel(ModelBuilder modelBuilder) {
        foreach (Assembly assembly in _assembliesToImport) {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
