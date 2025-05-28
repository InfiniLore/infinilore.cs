// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.AspNetCore;
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
// ReSharper disable once InconsistentNaming
public class DiDataSourceAttribute : DependencyInjectionDataSourceAttribute<IServiceScope> {
    private static readonly IServiceProvider ServiceProvider = CreateSharedServiceProvider().GetAwaiter().GetResult();

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override IServiceScope CreateScope(DataGeneratorMetadata dataGeneratorMetadata) => ServiceProvider.CreateAsyncScope();
    public override object? Create(IServiceScope scope, Type type) => scope.ServiceProvider.GetService(type);
    
    private static async Task<IServiceProvider> CreateSharedServiceProvider() {
        #region Setup DbConnection
        ILoggerFactory containerLoggerFactory = LoggingFactoryExtensions.CreateWithSerilog("TEST docker");
        MsSqlContainer contentDbContainer = new MsSqlBuilder()
            .WithLogger(containerLoggerFactory.CreateLogger<MsSqlContainer>())
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU10-ubuntu-22.04")
            .WithName("infinilore-testing-db")
            .Build();
        
        MinioContainer minIoContainer = new MinioBuilder()
            .WithLogger(containerLoggerFactory.CreateLogger<MinioContainer>())
            .WithImage("minio/minio")
            .WithName("infinilore-testing-db-file")
            .Build(); 

        await contentDbContainer.StartAsync();
        await minIoContainer.StartAsync();
        #endregion
        
        var services = new ServiceCollection();
        services.AddLogging();

        ServerModuleBuilder moduleBuilder = ServerModuleBuilder.Create(services)
            .AddModule<IServerModuleEntryCore>()
            .AddModule<IServerModuleEntryLoreScopes>();

        services.AddSingleton<GuidStore>();
        
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
        
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        var populator = new ContentDbPopulator(ServiceProvider);
        await populator.MigrateAsync();
        await populator.PopulateAsync();
        return serviceProvider;
    }

}
