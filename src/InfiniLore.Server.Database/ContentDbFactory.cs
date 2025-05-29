// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Types.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;

namespace InfiniLore.Server.Database;
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
        
        services.AddDbContextFactory<ContentDb>(options => {
            ILoggerFactory databaseLoggerFactory = LoggerFactory.Create(builder =>
                builder.AddSerilog(Log.Logger.ForContext("Section", "EFCORE ContentDb"))
            );

            options.UseLoggerFactory(databaseLoggerFactory);
            optionsAction.Invoke(options);
        });

        // Our UnitOfWork is integral to the correct execution of the repo pattern
        services.AddReadonlyUnitOfWork<ContentDb>();
        services.RegisterServicesFromInfiniLoreServerDatabase();
    }
    
    internal static void ConfigureModel(ModelBuilder modelBuilder) {
        foreach (Assembly assembly in _assembliesToImport) {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}
