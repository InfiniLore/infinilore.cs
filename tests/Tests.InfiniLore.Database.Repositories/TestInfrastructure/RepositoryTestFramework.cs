// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Database.MsSqlServer;
using InfiniLore.Server.Contracts.Database;
using Microsoft.Extensions.DependencyInjection;
using Tests.InfiniLore.Database.Repositories.TestInfrastructure;
using TUnit.Core.Interfaces;

namespace Tests.InfiniLore.Database.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Represents a base class providing a test framework for repository-related unit tests. This test framework
/// is designed to work with a specific repository type and facilitates managing database transactions and
/// service scopes for testing purposes.
/// </summary>
/// <typeparam name="TRepository">
/// The type of the repository being tested, which must implement the <see cref="IRepository"/> interface.
/// </typeparam>
public abstract class RepositoryTestFramework<TRepository>(DatabaseInfrastructure infrastructure) : IAsyncInitializer, IAsyncDisposable
    where TRepository : class, IRepository {

    /// <summary>
    /// Represents the generic repository instance used for performing database operations in test cases.
    /// </summary>
    protected TRepository Repository => ActivatorUtilities.CreateInstance<TRepository>(_scope.ServiceProvider);

    /// <summary>
    /// Represents the unit of work for managing database transactions and operations across multiple repositories.
    /// </summary>
    protected IDbUnitOfWork<MsSqlDbContext> UnitOfWork = default!;

    /// <summary>
    /// Represents a private instance of <see cref="IServiceScope"/> used for managing
    /// a scoped lifetime of services within the test framework.
    /// </summary>
    private IServiceScope _scope = default!;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    /// <inheritdoc/>
    public async virtual Task InitializeAsync() {
        _scope = infrastructure.ServiceProvider.CreateScope();
        UnitOfWork = _scope.ServiceProvider.GetRequiredService<IDbUnitOfWork<MsSqlDbContext>>();

        await UnitOfWork.BeginTransactionAsync();
        await UnitOfWork.CreateSavepointAsync(nameof(RepositoryTestFramework<TRepository>));
    }

    /// <inheritdoc/>
    public async virtual ValueTask DisposeAsync() {
        await UnitOfWork.TryRollbackToSavepointAsync(nameof(RepositoryTestFramework<TRepository>));
        await UnitOfWork.DisposeAsync();

        _scope.Dispose();
    }
}
