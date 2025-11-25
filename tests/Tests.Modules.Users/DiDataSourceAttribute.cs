// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Core.Database;
using InfiniLore.Core.Modular;
using InfiniLore.Modules.Users;
using InfiniLore.Modules.Users.Messaging.Events;
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

    private static ServiceProvider CreateServiceProvider() {
        var collection = new ServiceCollection()
            .AddLogging()
            .RegisterServicesFromTestsModulesUsers()
            .AddInfiniModuleProvider(
                moduleCollection => {
                    moduleCollection.AddModule<UsersInfiniModule>();
                },
                out InfiniModuleProvider moduleProvider
            )
            .AddInfiniLoreDb(
                options => {
                    string testDbFile = $"test_{Guid.NewGuid()}.db";
                    var connection = new SqliteConnection($"DataSource={testDbFile}");
                    connection.Open();
                    options.UseSqlite(connection);
                },
                moduleProvider.Assemblies
            );

        collection.AddFastEndpoints();
        collection.RegisterTestEventHandler<UserCreatedEvent, BlankEventHandler<UserCreatedEvent>>();
        collection.RegisterTestEventReceivers();
        
        return collection.BuildServiceProvider();
    }
    
}

public class BlankEventHandler<T> : IEventHandler<T> {
    public Task HandleAsync(T eventModel, CancellationToken ct) => Task.CompletedTask;
}
