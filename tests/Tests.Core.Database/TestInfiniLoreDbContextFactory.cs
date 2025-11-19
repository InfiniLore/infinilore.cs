// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Core.Database.TestData;

namespace Tests.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class TestInfiniLoreDbContextFactory {
    public static IServiceCollection AddTestDbContext(this IServiceCollection services) {
        string dbName = $"{nameof(DbContext)}_{Guid.NewGuid()}";
        
        
        
        return services.AddInfiniLoreDbContext(
            options => {
                options.UseInMemoryDatabase(dbName);
            },
            typeof(TestInfiniLoreDbContextFactory).Assembly,
            typeof(SimpleModel).Assembly
        );
    }
    
    public static InfiniLoreDbContext CreateInfiniLoreDbContext() {
        var services = new ServiceCollection();

        services.AddTestDbContext();
        
        ServiceProvider provider = services.BuildServiceProvider();
        var context = provider.GetRequiredService<InfiniLoreDbContext>();

        context.Database.EnsureCreated();
        return context;
    }
}
