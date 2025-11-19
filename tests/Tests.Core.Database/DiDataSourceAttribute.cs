// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class DiDataSourceAttribute : DependencyInjectionDataSourceAttribute<IServiceScope> {
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public override IServiceScope CreateScope(DataGeneratorMetadata dataGeneratorMetadata) => CreateServiceProvider().CreateScope();
    public override object Create(IServiceScope scope, Type type) => scope.ServiceProvider.GetRequiredService(type);

    private static ServiceProvider CreateServiceProvider() => new ServiceCollection()
        .AddLogging()
        .RegisterServicesFromTestsCoreDatabase()
        .AddTestDbContext()
        .BuildServiceProvider();

}
