// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Base;
using InfiniLoreLib.Results;

namespace InfiniLore.Server.Contracts.Repositories;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IAuditLogRepository<T> where T : BaseContent<T> {
    Task<Result<bool>> AddAsync(AuditLog<T> entity, CancellationToken ct = default);
}
