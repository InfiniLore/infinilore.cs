// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.AspNetCore;
using Fakers.InfiniLore.Server;
using InfiniLore.Server.Database;
using InfiniLore.Server.Modules.Core;
using InfiniLore.Server.Modules.LoreScopes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Testcontainers.Minio;
using Testcontainers.MsSql;

namespace DataSources.InfiniLore.Server;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class ServiceProviderDataSource {
    public readonly IServiceProvider SharedServiceProvider = CreateSharedServiceProvider().GetAwaiter().GetResult();

    public T GetRequiredService<T>() where T : notnull => SharedServiceProvider.CreateScope().ServiceProvider.GetRequiredService<T>();
    public T? GetService<T>() => SharedServiceProvider.CreateScope().ServiceProvider.GetService<T>();
    public object? GetService(Type type) => SharedServiceProvider.CreateScope().ServiceProvider.GetService(type);
    public IEnumerable<T> GetServices<T>() => SharedServiceProvider.CreateScope().ServiceProvider.GetServices<T>();
    public IEnumerable<object?> GetServices(Type type) => SharedServiceProvider.CreateScope().ServiceProvider.GetServices(type);
    
    // -----------------------------------------------------------------------------------------------------------------
    // Creation
    // -----------------------------------------------------------------------------------------------------------------
    private static async Task<IServiceProvider> CreateSharedServiceProvider() {
        #region Setup DbConnection
        ILoggerFactory containerLoggerFactory = LoggingFactoryExtensions.CreateWithSerilog("TEST docker");
        MsSqlContainer contentDbContainer = new MsSqlBuilder()
            .WithLogger(containerLoggerFactory.CreateLogger<MsSqlContainer>())
            .WithPortBinding(MsSqlBuilder.MsSqlPort, true)
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
            .Build();
        
        MinioContainer minIoContainer = new MinioBuilder()
            .WithLogger(containerLoggerFactory.CreateLogger<MinioContainer>())
            .WithPortBinding(MinioBuilder.MinioPort, true)
            .WithImage("minio/minio")
            .Build(); 

        await Task.WhenAll(
            contentDbContainer.StartAsync(),
            minIoContainer.StartAsync()
        );
        #endregion
        
        var services = new ServiceCollection();
        services.AddLogging();

        ServerModuleBuilder moduleBuilder = ServerModuleBuilder.Create(services)
            .AddModule<IServerModuleEntryCore>()
            .AddModule<IServerModuleEntryLoreScopes>();

        services.RegisterServicesFromFakersInfiniLoreServer();
        services.RegisterServicesFromDataSourcesInfiniLoreServer();

        ContentDbFactory.RegisterDatabase(
            services,
            moduleBuilder.ModuleAssemblies, 
            optionsAction: builder => builder.UseSqlServer(contentDbContainer.GetConnectionString())
        );
        
        S3FileDbFactory.RegisterDatabase(
            services,
            minIoContainer.IpAddress,
            minIoContainer.GetAccessKey(),
            minIoContainer.GetSecretKey()       
        );
        
        ServiceProvider provider = services.BuildServiceProvider();
        var populator = new ContentDbPopulator(provider);
        await populator.MigrateAsync();
        await populator.PopulateAsync();
        return provider;
    }
}
