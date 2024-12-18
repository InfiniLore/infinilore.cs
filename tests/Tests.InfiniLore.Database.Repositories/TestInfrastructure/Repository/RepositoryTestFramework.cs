// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using Microsoft.Extensions.DependencyInjection;
using TUnit.Core.Interfaces;

namespace Tests.InfiniLore.Database.Repositories.TestInfrastructure.Repository;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
///     Represents a base class providing a test framework for repository-related unit tests. This test framework
///     is designed to work with a specific repository type and facilitates managing database transactions and
///     service scopes for testing purposes.
/// </summary>
/// <typeparam name="TRepository">
///     The type of the repository being tested, which must implement the <see cref="IRepository" /> interface.
/// </typeparam>
public abstract class RepositoryTestFramework<TRepository>(DatabaseInfrastructure infrastructure) : IAsyncInitializer, IAsyncDisposable
    where TRepository : class, IRepository {

    private readonly Guid _transactionId = Guid.NewGuid();

    /// <summary>
    ///     Represents a private instance of <see cref="IServiceScope" /> used for managing
    ///     a scoped lifetime of services within the test framework.
    /// </summary>
    private IServiceScope _scope = null!;


    /// <summary>
    ///     Represents the unit of work for managing database transactions and operations across multiple repositories.
    /// </summary>
    protected IUnitOfWork UnitOfWork = null!;

    /// <summary>
    ///     Represents the generic repository instance used for performing database operations in test cases.
    /// </summary>
    protected TRepository Repository => ActivatorUtilities.CreateInstance<TRepository>(_scope.ServiceProvider);

    /// <inheritdoc />
    public async virtual ValueTask DisposeAsync() {
        await UnitOfWork.DisposeAsync();
        _scope.Dispose();
    }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc />
    public async virtual Task InitializeAsync() {
        _scope = infrastructure.ServiceProvider.CreateScope();
        UnitOfWork = _scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        await UnitOfWork.TryCreateTransactionAsync();
    }
    protected async Task RollbackToSavepointAsync() {
        bool result = await UnitOfWork.TryRollbackToSavepointAsync(_transactionId);
        await Assert.That(result).IsTrue();
    }

    protected async Task CreateSavepointAsync() => await UnitOfWork.TryCreateSavepointAsync(_transactionId);
}
