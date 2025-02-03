// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Server.Contracts.Database;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace InfiniLore.Database.MsSqlServer.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IUnitOfWorkFactory>(ServiceLifetime.Scoped)]
public class UnitOfWorkFactory(IDbContextFactory<MsSqlDbContext> dbContextFactory, IServiceProvider provider, ILogger logger) : IUnitOfWorkFactory{
    public IUnitOfWork Create() {
        // Each unit of work should have their own scope which they pull their repositories from
        //      This, if the factory is used correctly, should enforce correct usage and limit dbcontext concurrency issues.
        var scope = provider.CreateScope();
        
        // Because our factory doesn't create the actual dbcontext, yet we are safe, and we can just inject it downwards.
        return new UnitOfWork(dbContextFactory, scope);
    }

    public async Task<IUnitOfWork> CreateWithTransactionAsync(CancellationToken ct = default) {
        IUnitOfWork unitOfWork = Create();
        
        // ReSharper disable once InvertIf
        if (!await unitOfWork.TryCreateTransactionAsync(ct)) {
            logger.Error("Failed to create transaction");
            throw new Exception("Failed to create transaction");
        }
        return unitOfWork;
    }
}
