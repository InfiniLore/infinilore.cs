// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Core.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tests.Core.Database.TestData;

namespace Tests.Core.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class TestInfiniLoreDbContextFactory {
    public static IServiceCollection AddTestDbContext(this IServiceCollection services) =>
        services.AddInfiniLoreDb(
            options => {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                
                options.UseSqlite(connection);
            },
            typeof(TestInfiniLoreDbContextFactory).Assembly,
            typeof(SimpleModel).Assembly
        );

    public static InfiniLoreDb CreateInfiniLoreDbContext() {
        var services = new ServiceCollection();

        services.AddTestDbContext();

        ServiceProvider provider = services.BuildServiceProvider();
        var context = provider.GetRequiredService<InfiniLoreDb>();

        context.Database.EnsureCreated();
        return context;
    }
}
