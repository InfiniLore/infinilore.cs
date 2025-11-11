// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfiniLore.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class ContentDbFactory {
    private static IEnumerable<Assembly> _assembliesToImport = [];

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static void RegisterDatabase(
        IServiceCollection services, 
        IEnumerable<Assembly> assembliesToImport,
        Action<DbContextOptionsBuilder> optionsAction
    ) {
        _assembliesToImport = assembliesToImport;
        
        services.AddDbContextFactory<ContentDb>(optionsAction.Invoke);
        services.AddReadonlyUnitOfWork<ContentDb>();
    }
    
    internal static void ConfigureModel(ModelBuilder modelBuilder) {
        foreach (Assembly assembly in _assembliesToImport) {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
