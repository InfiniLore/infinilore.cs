// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using InfiniLore.Core.Modular;
using InfiniLore.Modules.Users;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Modules.Users;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DiDataSourceAttribute : DependencyInjectionDataSourceAttribute<IServiceScope> {
    public static Lazy<SqliteConnection> Connection { get; } = new(() => {
        // var connection = new SqliteConnection($"DataSource=test_{Guid.NewGuid()}.db");
        // if (File.Exists("test.db")) File.Delete("test.db");
        var connection = new SqliteConnection("DataSource=:memory:");    
        connection.Open();
        return connection;
    }); 
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override IServiceScope CreateScope(DataGeneratorMetadata dataGeneratorMetadata) => CreateServiceProvider().CreateScope();
    public override object Create(IServiceScope scope, Type type) => scope.ServiceProvider.GetRequiredService(type);

    private static ServiceProvider CreateServiceProvider() {
        
        
        return new ServiceCollection()
            .AddLogging()
            .RegisterServicesFromTestsModulesUsers()
            .AddInfiniModuleProvider(
                moduleCollection => {
                    moduleCollection.AddModule<UserInfiniModule>();
                },
                out InfiniModuleProvider moduleProvider
            )
            .AddInfiniLoreDb(
                options => {
                    options.UseSqlite(Connection.Value);
                },
                moduleProvider.Assemblies
            )
            .BuildServiceProvider();
    }

}
