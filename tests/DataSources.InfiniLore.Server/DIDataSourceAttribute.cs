// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Modules.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DataSources.InfiniLore.Server;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// ReSharper disable once InconsistentNaming
public class DiDataSourceAttribute : DependencyInjectionDataSourceAttribute<IServiceScope> {
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override IServiceScope CreateScope(DataGeneratorMetadata dataGeneratorMetadata) => CreateServiceProvider().CreateScope();
    public override object? Create(IServiceScope scope, Type type) => scope.ServiceProvider.GetService(type);

    private static ServiceProvider CreateServiceProvider() {

        var services = new ServiceCollection();
        services.AddLogging();

        ServerModuleBuilder moduleBuilder = ServerModuleBuilder.Create(services)
            .AddModule<IServerModuleEntryCore>();


        return services.BuildServiceProvider();
    }
}
