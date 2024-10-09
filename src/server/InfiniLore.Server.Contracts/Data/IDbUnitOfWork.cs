// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Contracts.Data;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IDbUnitOfWork<out T> where T : DbContext {
    T Db { get; } 
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    Task BeginTransactionAsync(CancellationToken ct = default);
    
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task<bool> TryCommitTransactionAsync(CancellationToken ct = default);
    
    Task RollbackTransactionAsync(CancellationToken ct = default);
    Task<bool> TryRollbackTransactionAsync(CancellationToken ct = default);
    
    T GetDbContext();
}
