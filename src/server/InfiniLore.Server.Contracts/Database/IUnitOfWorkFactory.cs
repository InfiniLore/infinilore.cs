// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;

namespace InfiniLore.Server.Contracts.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IUnitOfWorkFactory : IFactoryService<IUnitOfWork> {
    Task<IUnitOfWork> CreateWithTransactionAsync(CancellationToken ct = default);
}
