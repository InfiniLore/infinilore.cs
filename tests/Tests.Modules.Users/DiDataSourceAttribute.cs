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
    public override IServiceScope CreateScope(DataGeneratorMetadata dataGeneratorMetadata)
        => CreateServiceProvider().CreateScope();

    public override object Create(IServiceScope scope, Type type)
        => scope.ServiceProvider.GetRequiredService(type);

    private static ServiceProvider CreateServiceProvider()
        => new ServiceCollection()
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
                    var connection = new SqliteConnection("DataSource=:memory:");
                    connection.Open();
                    options.UseSqlite(connection);
                },
                moduleProvider.Assemblies
            )
            .BuildServiceProvider();

}
