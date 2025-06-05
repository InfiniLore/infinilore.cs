// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Fakers.InfiniLore.Server;
using InfiniLore.Modules.Core.Server;
using InfiniLore.Server.Database;
using InfiniLore.Modules.LoreScopes.Server;
using InfiniLore.Modules.LsMarkdownFiles.Server;
using InfiniLore.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataSources.InfiniLore.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ServiceProviderDataSource {
    private static readonly IServiceProvider ServiceProvider = CreateSharedServiceProvider().GetAwaiter().GetResult();

    public T GetRequiredService<T>() where T : notnull => ServiceProvider.CreateScope().ServiceProvider.GetRequiredService<T>();
    public T? GetService<T>() => ServiceProvider.CreateScope().ServiceProvider.GetService<T>();
    public object? GetService(Type type) => ServiceProvider.CreateScope().ServiceProvider.GetService(type);
    public IEnumerable<T> GetServices<T>() => ServiceProvider.CreateScope().ServiceProvider.GetServices<T>();
    public IEnumerable<object?> GetServices(Type type) => ServiceProvider.CreateScope().ServiceProvider.GetServices(type);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Creation
    // -----------------------------------------------------------------------------------------------------------------
    private static async Task<IServiceProvider> CreateSharedServiceProvider() {
        #region Setup Containers
        var devEnv = InfiniLoreContainers.CreateForTesting();
        await devEnv.InitializeAsync();
        #endregion
        
        var services = new ServiceCollection();
        services.AddLogging();

        ServerModuleBuilder moduleBuilder = ServerModuleBuilder.Create(services)
            .AddModule<IServerModuleEntryCore>()
            .AddModule<IServerModuleEntryLoreScopes>()
            .AddModule<IModulelsLsMarkdownFilesServer>();

        services.RegisterServicesFromFakersInfiniLoreServer();
        services.RegisterServicesFromDataSourcesInfiniLoreServer();

        ContentDbFactory.RegisterDatabase(
            services,
            moduleBuilder.ModuleAssemblies, 
            optionsAction: builder => builder.UseSqlServer(devEnv.GetSqlConnectionString())
        );
        
        S3FileDbFactory.RegisterDatabase(
            services,
            devEnv.GetMinioConnectionString(),
            devEnv.GetMinioAccessKey(),
            devEnv.GetMinioSecretKey()       
        );
        
        ServiceProvider provider = services.BuildServiceProvider();
        var populator = new ContentDbPopulator(provider);
        await populator.MigrateAsync();
        await populator.PopulateAsync();
        return provider;
    }
}
