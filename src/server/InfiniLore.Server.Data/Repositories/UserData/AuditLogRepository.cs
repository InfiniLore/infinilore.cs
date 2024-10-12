// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Data;
using InfiniLore.Server.Data.Models.Base;
using InfiniLoreLib.Results;

namespace InfiniLore.Server.Data.Repositories.UserData;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
// Manually add this to the DI container.
public class AuditLogRepository<T>(IDbUnitOfWork<InfiniLoreDbContext> unitOfWork) : IAuditLogRepository<T> where T : BaseContent<T> {
    public async Task<Result<bool>> AddAsync(AuditLog<T> entity, CancellationToken ct = default) {
        InfiniLoreDbContext dbContext = unitOfWork.GetDbContext();

        dbContext.Set<AuditLog<T>>().Add(entity);
        await dbContext.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
